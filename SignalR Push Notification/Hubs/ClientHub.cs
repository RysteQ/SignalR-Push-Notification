using Microsoft.AspNetCore.SignalR;
using SignalR_Push_Notification.Models.Notifications;
using SignalR_Push_Notification.Models.Users;
using SignalR_Push_Notification.Services.DatabaseController;

namespace SignalR_Push_Notification.Hubs;

// This will be slow if a lot of users are present, I suspect that this is capable of handling like 20.000
// users at once (minimum) before having performanc issues. It might be more or less, but it this isn't
// to be used in any serious capacity without slight modifications on the users static list
public class ClientHub : Hub
{
    public override Task OnConnectedAsync()
    {
        _users.Add(new()
        {
            UUID = Guid.Empty,
            ConnectionId = Context.ConnectionId
        });

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _users.Remove(_users.First(selectedUser => selectedUser.ConnectionId == Context.ConnectionId));

        return base.OnDisconnectedAsync(exception);
    }

    public void Register(Guid userUUID, string? group = null)
    {
        _users.Last(selectedUser => selectedUser.ConnectionId == Context.ConnectionId).UUID = userUUID;
        _users.Last(selectedUser => selectedUser.ConnectionId == Context.ConnectionId).Group = group;

        using (DatabaseController databaseController = new())
        {
            List<NotificationDb> notifications = databaseController.Read().Where(selectedNotification => selectedNotification.User == userUUID).ToList();

            foreach (NotificationDb notification in notifications)
            {
                Clients.Caller.SendAsync("Notify", notification);
                databaseController.Delete(notification.UUID);
            }
        }
    }

    public static ConnectedUser? FindUser(Guid userUUID)
    {
        return _users.FirstOrDefault(selectedUser => selectedUser.UUID == userUUID);
    }

    private static List<ConnectedUser> _users = [];
}
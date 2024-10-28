using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR_Push_Notification.Hubs;
using SignalR_Push_Notification.Models.Notifications;
using SignalR_Push_Notification.Models.Users;
using SignalR_Push_Notification.Services.DatabaseController;

namespace SignalR_Push_Notification.Controllers;

[ApiController]
[Route("v1")]
public class PushNotificationController : ControllerBase
{
    public PushNotificationController(IHubContext<ClientHub> clientHub)
    {
        _clientHub = clientHub;
    }

    [HttpPost("users/{userUUID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult SendPushNotification(Guid userUUID, [FromBody] NotificationRequest notification)
    {
        NotificationIntermediate notificationIntermediate = NotificationIntermediate.ConvertFrom(notification);
        ConnectedUser? user = ClientHub.FindUser(userUUID);

        if (string.IsNullOrEmpty(notificationIntermediate.Group) == false)
        {
            return BadRequest($"Cannot supply the value {nameof(notificationIntermediate.Group)} for the {nameof(SendPushNotification)} method");
        }

        notificationIntermediate.User = userUUID;

        if (user != null)
        {
            _clientHub.Clients.Client(user.ConnectionId).SendAsync("Notify", notification);
        }
        else
        {
            try
            {
                DatabaseController databaseController = new();
                databaseController.Create(NotificationDb.ConvertFrom(notificationIntermediate));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return Ok();
    }

    [HttpPost("users/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult SendPushNotificationToAll([FromBody] NotificationRequest notification)
    {
        NotificationIntermediate notificationIntermediate = NotificationIntermediate.ConvertFrom(notification);

        _clientHub.Clients.All.SendAsync("Notify", notification);

        return Ok();
    }

    [HttpPost("groups/{group}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult SendNotificationToGroup(string group, [FromBody] NotificationRequest notification)
    {
        NotificationIntermediate notificationIntermediate = NotificationIntermediate.ConvertFrom(notification);

        if (string.IsNullOrEmpty(group))
        {
            return BadRequest($"Value {group} cannot be empty for the method {nameof(SendNotificationToGroup)}");
        }

        notificationIntermediate.Group = group;
        _clientHub.Clients.Group(group).SendAsync("Notify", notification);

        try
        {
            DatabaseController databaseController = new();
            databaseController.Create(NotificationDb.ConvertFrom(notificationIntermediate));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    private readonly IHubContext<ClientHub> _clientHub;
}
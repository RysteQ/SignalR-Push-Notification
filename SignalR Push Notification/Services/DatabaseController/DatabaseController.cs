using Microsoft.EntityFrameworkCore;
using SignalR_Push_Notification.Models.Notifications;
using SignalR_Push_Notification.Services.DatabaseController.Internal;

namespace SignalR_Push_Notification.Services.DatabaseController;

public class DatabaseController : IDisposable
{
    public async void Create(NotificationDb notification)
    {
        using LocalDatabase localDatabase = new();

        localDatabase.Notifications.Add(notification);

        await localDatabase.SaveChangesAsync();
    }

    public List<NotificationDb> Read()
    {
        using LocalDatabase localDatabase = new();

        return localDatabase.Notifications.ToList();
    }

    public async Task<NotificationDb?> Read(Guid UUID)
    {
        using LocalDatabase localDatabase = new();

        return await localDatabase.Notifications.FirstOrDefaultAsync(selectedNotification => selectedNotification.UUID == UUID);
    }

    public async void Update(Guid UUID, NotificationDb notification)
    {
        using LocalDatabase localDatabase = new();
        
        NotificationDb toUpdate = localDatabase.Notifications.First(selectedNotification => selectedNotification.UUID == UUID);

        toUpdate.Clone(notification);

        await localDatabase.SaveChangesAsync();
    }

    public async void Delete(Guid UUID)
    {
        using LocalDatabase localDatabase = new();

        NotificationDb? toDelete = await localDatabase.Notifications.FirstOrDefaultAsync(selectedNotification => selectedNotification.UUID == UUID);

        if (toDelete != null)
        {
            localDatabase.Remove(toDelete);
        }

        await localDatabase.SaveChangesAsync();
    }

    public void Dispose()
    {
        return;
    }
}
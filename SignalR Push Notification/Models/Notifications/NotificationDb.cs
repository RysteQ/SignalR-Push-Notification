using Microsoft.EntityFrameworkCore;
using SignalR_Push_Notification.Interfaces;

namespace SignalR_Push_Notification.Models.Notifications;

[PrimaryKey(nameof(UUID))]
public class NotificationDb : ICopy<NotificationDb>
{
    public static NotificationDb ConvertFrom(NotificationIntermediate request)
    {
        return new()
        {
            UUID = Guid.NewGuid(),
            User = request.User,
            Group = request.Group,
            CreationTime = DateTime.UtcNow,
            Title = request.Title,
            NotifyTime = request.NotifyTime,
            Contents = request.Contents,
            NotificationImage = request.NotificationImage,
            Data = request.Data,
        };
    }

    public void Clone(NotificationDb toCopy)
    {
        User = toCopy.User;
        Group = toCopy.Group;
        Title = toCopy.Title;
        NotifyTime = toCopy.NotifyTime;
        Contents = toCopy.Contents;
        NotificationImage = toCopy.NotificationImage;
        Data = toCopy.Data;
    }

    public required Guid UUID { get; init; }
    public Guid User { get; private set; }
    public string Group { get; private set; } = string.Empty;
    public required DateTime CreationTime { get; init; }

    public string Title { get; private set; } = string.Empty;
    public DateTime NotifyTime { get; private set; }
    public string Contents { get; private set; } = string.Empty;
    public byte[] NotificationImage { get; private set; } = [];
    public byte[] Data { get; private set; } = [];
}
namespace SignalR_Push_Notification.Models.Notifications;

public class NotificationIntermediate
{
    public static NotificationIntermediate ConvertFrom(NotificationRequest request)
    {
        return new()
        {
            Title = request.Title,
            NotifyTime = request.NotifyTime,
            Contents = request.Contents,
            NotificationImage = request.NotificationImage,
            Data = request.Data,
        };
    }

    public Guid User { get; set; } = Guid.Empty;
    public string Group { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public DateTime NotifyTime { get; set; }
    public string Contents { get; set; } = string.Empty;
    public byte[] NotificationImage { get; set; } = [];
    public byte[] Data { get; set; } = [];
}
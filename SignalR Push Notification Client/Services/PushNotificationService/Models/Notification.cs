namespace SignalR_Push_Notification_Client.Services.PushNotificationService.Models;

public class Notification
{
    public required string Title { get; init; }
    public DateTime NotifyTime { get; init; }
    public required string Contents { get; init; }
    public byte[] NotificationImage { get; init; } = [];
    public byte[] Data { get; init; } = [];
}

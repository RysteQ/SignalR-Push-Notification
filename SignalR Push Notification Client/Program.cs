using SignalR_Push_Notification_Client.Services.PushNotificationService;
using SignalR_Push_Notification_Client.Services.PushNotificationService.Models;

void Notified(object? sender, Notification e)
{
    Console.WriteLine($"{e.Title} - {e.NotifyTime} - {e.Contents}");
}

PushNotificationService notificationService = new(Guid.NewGuid());

notificationService.Notified += Notified;
notificationService.Connect("localhost", 5147);

Console.WriteLine(notificationService.ConnectionGuid);

while (true)
{
    Thread.Sleep(1000);
}
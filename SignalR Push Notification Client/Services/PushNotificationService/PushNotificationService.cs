using SignalR_Push_Notification_Client.Services.PushNotificationService.Models;
using Microsoft.AspNetCore.SignalR.Client;
using SignalR_Push_Notification_Client.Services.PushNotificationService.Internal;

namespace SignalR_Push_Notification_Client.Services.PushNotificationService;

public delegate void Notified(Notification notification);

public class PushNotificationService
{
    public PushNotificationService(Guid UUID)
    {
        ConnectionGuid = UUID;
    }

    public async void Connect(string url, int port, string? group = null)
    {
        if (Connected)
        {
            throw new InvalidOperationException($"Already connected to a server, run the {nameof(Disconnect)} method first");
        }

        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"http://{url}:{port}/hub")
            .WithAutomaticReconnect(new RetryPolicy())
            .Build();

        _hubConnection.On<Notification>("Notify", (notification) =>
        {
            Notify(notification);
        });

        _hubConnection.Closed += LostConnection;

        Connected = true;
        Group = group;

        await _hubConnection.StartAsync();
        await _hubConnection.InvokeAsync("Register", ConnectionGuid, Group);
    }

    public async void Disconnect()
    {
        if (Connected == false)
        {
            throw new InvalidOperationException($"Not connected to a server, run the {nameof(Connect)} method first");
        }

        Connected = false;
        Group = null;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        await _hubConnection.DisposeAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    private void Notify(Notification notification)
    {
        Notified?.Invoke(null, notification);
    }

    private Task LostConnection(Exception? exception)
    {
        ConnectionLost?.Invoke(null, EventArgs.Empty);
        
        return Task.CompletedTask;
    }

    public bool Connected { get; private set; }
    public Guid ConnectionGuid { get; init; }
    public string? Group { get; private set; }

    public event EventHandler<Notification>? Notified;
    public event EventHandler? ConnectionLost;

    private HubConnection? _hubConnection;
}
namespace SignalR_Push_Notification.Models.Users;

public record ConnectedUser
{
    public Guid UUID { get; set; }
    public string? Group { get; set; } = null;
    public string ConnectionId { get; init; } = string.Empty;
}
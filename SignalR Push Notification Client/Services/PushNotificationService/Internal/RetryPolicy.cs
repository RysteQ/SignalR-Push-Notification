using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR_Push_Notification_Client.Services.PushNotificationService.Internal;

internal class RetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        return new(0, 0, 15);
    }
}

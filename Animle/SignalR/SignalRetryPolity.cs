using Microsoft.AspNetCore.SignalR.Client;

namespace Animle.SignalR;

public class SignalRRetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        return TimeSpan.FromSeconds(2);
    }
}
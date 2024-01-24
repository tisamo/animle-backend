using Microsoft.AspNetCore.SignalR.Client;

public class SignalRRetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        //replace this with whatever logic you want. Below is just example how to retry every 30 seconds and never stop. I would advise putting in place some sort of a stop after maybe few hours or such. But it depends on your scenario.
        return TimeSpan.FromSeconds(2);
    }
}
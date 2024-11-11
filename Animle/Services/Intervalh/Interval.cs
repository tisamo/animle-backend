using System;
using System.Threading;

public class RestartableInterval
{
    private Timer _timer;
    private int _interval;
    private Action _action;


    public RestartableInterval(Action action, int intervalMilliseconds)
    {
        _action = action;
        _interval = intervalMilliseconds;
        _timer = new Timer(OnTick, null, Timeout.Infinite, Timeout.Infinite);
 
    }

    private void OnTick(object state)
    {
        _action.Invoke();
    }

    public void InvokeAction()
    {
        _action.Invoke();

    }

    public void Start()
    {
        _timer.Change(0, _interval); 
    }

    public void Stop()
    {
        _timer.Change(Timeout.Infinite, Timeout.Infinite); 
    }

    public void Restart()
    {
        Stop();  
        Start(); 
    }
    public void ChangeInterval(int newIntervalMilliseconds)
    {
        _interval = newIntervalMilliseconds;
        Restart(); 
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}



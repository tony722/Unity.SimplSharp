using System;

namespace AET.Unity.SimplSharp.Timer {
  public class TestTimer : ITimer {
    private object callbackObject;

    public override Action<object> TimerCallback { protected get; set; }
    
    public override bool IsRunning { get; protected set; }

    public bool ElapseImmediately { get; set; }
    public long TimeoutMs { get; private set; }
    public long RepeatMs { get; private set; }

    public override void Start(long timeoutMs) {
      Start(timeoutMs, (object)null);
    }

    public override void Start(long timeoutMs, long repeatMs) {
      Start(timeoutMs, repeatMs, (object)null);
    }

    public override void Start(long timeoutMs, object callbackObject) {
      IsRunning = true;
      TimeoutMs = timeoutMs;
      this.callbackObject = callbackObject;
      if (ElapseImmediately) TimerElapsed();
    }

    public override void Start(long timeoutMs, long repeatMs, object callbackObject) {
      IsRunning = true;
      TimeoutMs = timeoutMs;
      RepeatMs = repeatMs;
      this.callbackObject = callbackObject;
      if (ElapseImmediately) TimerElapsed();
    }

    public override void Start(long timeout, Action<object> timerCallback) {
      TimerCallback = timerCallback;
      Start(TimeoutMs);
    }

    public override void Start(long timeoutMs, long repeatMs, Action<object> timerCallback) {
      TimerCallback = timerCallback;
      Start(timeoutMs, repeatMs, (object)null);
    }

    public override void Restart() {
      IsRunning = true;
    }

    public override void Stop() {
      IsRunning = false;
    }

    public override void Dispose() {
      IsRunning = false;
    }

    public void TimerElapsed() {
      TimerCallback(callbackObject);
    }
  }
}
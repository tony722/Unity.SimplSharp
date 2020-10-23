using System;

namespace AET.Unity.SimplSharp.Timer {
  public class TestTimer : ITimer {
    private object callbackObject;

    public override Action<object> TimerCallback { protected get; set; }
    
    public override bool IsRunning { get; protected set; }

    public bool ElapseImmediately { get; set; }
    public long TimeoutMs { get; private set; }

    public override void Start(long timeoutMs) {
      Start(timeoutMs, null);
    }

    public override void Start(long timeoutMs, object callbackObject) {
      IsRunning = true;
      TimeoutMs = timeoutMs;
      this.callbackObject = callbackObject;
      if (ElapseImmediately) TimerElapsed();
    }

    public override void Restart() { }
    public override void Dispose() { }

    public void TimerElapsed() {
      if (!IsRunning) return;
      IsRunning = false;
      TimerCallback(callbackObject);
    }
  }
}
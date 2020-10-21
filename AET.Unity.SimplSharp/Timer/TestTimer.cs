using System;

namespace AET.Unity.SimplSharp.Timer {
  public class TestTimer : ITimer {

    public override Action TimerCallback { protected get; set; }
    
    public override bool IsRunning { get; protected set; }

    public bool ElapseImmediately { get; set; }
    public long TimeoutMs { get; private set; }

    public override void Start(long timeoutMs) {
      IsRunning = true;
      TimeoutMs = timeoutMs;
      if (ElapseImmediately) TimerElapsed();
    }

    public override void Restart() { }
    public override void Dispose() { }

    public void TimerElapsed() {
      if (!IsRunning) return;
      IsRunning = false;
      TimerCallback();
    }
  }
}
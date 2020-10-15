using System;

namespace AET.Unity.SimplSharp.Timer {
  public class TestTimer : ITimer {
    public Action TimerCallback { private get; set; }

    public bool IsRunning { get; private set; }

    public bool ElapseImmediately { get; set; }
    public long TimeoutMs { get; private set; }

    public void Start(long timeoutMs) {
      IsRunning = true;
      TimeoutMs = timeoutMs;
      if (ElapseImmediately) TimerElapsed();
    }

    public void Restart() { }

    public void TimerElapsed() {
      if (!IsRunning) return;
      IsRunning = false;
      TimerCallback();
    }
  }
}
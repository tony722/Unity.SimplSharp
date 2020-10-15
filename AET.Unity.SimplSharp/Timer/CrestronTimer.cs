using System;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp.Timer {
  public class CrestronTimer : ITimer {
    #region ITimer Members

    private CTimer timer = null;

    public Action TimerCallback { private get; set; }
    public bool IsRunning { get; private set; }
    public long TimeoutMs { get; set; }

    public void Start() {
      if (timer != null && !timer.Disposed) timer.Dispose();
      IsRunning = true;
      timer = new CTimer(CTimerCallback, TimeoutMs);
    }

    public void Start(long timeoutMs) {
      TimeoutMs = timeoutMs;
      Start();
    }

    public void Restart() {
      Start();
    }

    private void CTimerCallback(object o) {
      IsRunning = false;
      timer.Dispose();
      TimerCallback();
    }

    #endregion
  }
}
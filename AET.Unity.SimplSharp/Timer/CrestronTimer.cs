using System;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp.Timer {
  public class CrestronTimer : ITimer {
    #region ITimer Members

    private CTimer timer = null;

    public override Action TimerCallback { protected get; set; }
    public override bool IsRunning { get; protected set; }
    public long TimeoutMs { get; set; }

    public void Start() {
      if (timer != null && !timer.Disposed) timer.Dispose();
      IsRunning = true;
      timer = new CTimer(CTimerCallback, TimeoutMs);
    }

    public override void Start(long timeoutMs) {
      TimeoutMs = timeoutMs;
      Start();
    }

    public override void Restart() {
      Start();
    }

    public override void Dispose() {
      timer.Dispose();
    }

    private void CTimerCallback(object o) {
      IsRunning = false;
      timer.Dispose();
      TimerCallback();
    }

    #endregion
  }
}
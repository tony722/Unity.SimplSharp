using System;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp.Timer {
  public class CrestronTimer : ITimer {
    #region ITimer Members

    private CTimer timer = null;

    public override Action<object> TimerCallback { protected get; set; }
    public override bool IsRunning { get; protected set; }
    public long TimeoutMs { get; set; }

    private void Start(object callbackObject) {
      if (timer != null && !timer.Disposed) timer.Dispose();
      IsRunning = true;
      timer = new CTimer(CTimerCallback, callbackObject, TimeoutMs);
    }

    public override void Start(long timeoutMs, object callbackObject) {
      TimeoutMs = timeoutMs;
      Start(callbackObject);
    }

    public override void Start(long timeoutMs) {
      Start(timeoutMs, null);
    }

    public override void Restart() {
      Start(null);
    }

    public override void Dispose() {
      timer.Dispose();
    }

    private void CTimerCallback(object o) {
      IsRunning = false;
      timer.Dispose();
      TimerCallback(o);
    }

    #endregion
  }
}
using System;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp.Timer {
  public class CrestronTimer : ITimer {
    #region ITimer Members

    private CTimer timer = null;

    public override Action<object> TimerCallback { protected get; set; }
    public override bool IsRunning { get; protected set; }
    public long TimeoutMs { get; set; }
    public long RepeatMs { get; set; }


    public override void Start(long timeoutMs, object callbackObject) {
      TimeoutMs = timeoutMs;
      if (timer != null && !timer.Disposed) timer.Dispose();
      IsRunning = true;
      timer = new CTimer(CTimerCallback, callbackObject, timeoutMs);
    }

    public override void Start(long timeoutMs, long repeatMs, object callbackObject) {
      if (timer != null && !timer.Disposed) timer.Dispose();
      TimeoutMs = timeoutMs;
      RepeatMs = repeatMs;
      IsRunning = true;
      timer = new CTimer(CTimerCallback, callbackObject, timeoutMs, repeatMs);
    }

    public override void Start(long timeout, Action<object> timerCallback) {
      TimerCallback = timerCallback;
      Start(timeout);
    }

    public override void Start(long timeoutMs, long repeatMs, Action<object> timerCallback) {
      TimerCallback = timerCallback;
      Start(timeoutMs, repeatMs);
    }

    public override void Start(long timeoutMs, long repeatMs) {
      Start(timeoutMs, repeatMs,  (object) null);
    }

    public override void Start(long timeoutMs) {
      Start(timeoutMs, (object) null);
    }

    public override void Restart() {
      timer.Reset();
    }

    public override void Stop() {
      timer.Dispose();
    }

    public override void Dispose() {
      timer.Dispose();
    }

    private void CTimerCallback(object o) {
      IsRunning = false;
      TimerCallback(o);
    }

    #endregion
  }
}
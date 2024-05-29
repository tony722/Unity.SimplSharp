using System;
using AET.Unity.SimplSharp.Timer;

namespace AET.Unity.SimplSharp {
  public class PressHoldRepeater {
    private ITimer repeatTimer;
    private ITimer delayTimer;

    public PressHoldRepeater() { }


    /// <summary>
    /// Allow injection of ITimer
    /// </summary>
    public ITimer DelayTimer { 
      get { return delayTimer ?? (DelayTimer = new CrestronTimer()); }
      set {
        delayTimer = value; 
        delayTimer.TimerCallback = StartRepeating;
    } }

    /// <summary>
    /// Allow injection of ITimer
    /// </summary>
    public ITimer RepeatTimer { 
      get { return repeatTimer ?? (RepeatTimer = new CrestronTimer()); }
      set {
        repeatTimer = value; 
        repeatTimer.TimerCallback = Repeat;
    } }

    public long DelayTimeMs { get; set; }
    public long RepeatTimeMs { get; set; }

    public Action Action { get; set; }

    public void Press() {
      DelayTimer.Start(DelayTimeMs);
    }

    private void StartRepeating(object o) {
      RepeatTimer.Start(RepeatTimeMs, RepeatTimeMs, Repeat);
    }

    private void Repeat(object o) {
      Action();
    }

    public void Release() {
      if (DelayTimer.IsRunning) {
        DelayTimer.Stop();
        Action();
      }
      if (RepeatTimer.IsRunning) RepeatTimer.Stop();
    }
  }
}
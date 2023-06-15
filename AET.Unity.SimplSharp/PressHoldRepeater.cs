using System;
using AET.Unity.SimplSharp.Timer;

namespace AET.Unity.SimplSharp {
  public class PressHoldRepeater {
    
    public PressHoldRepeater() {
      DelayTimer = new CrestronTimer();
      DelayTimer.TimerCallback = StartRepeating;
      RepeatTimer = new CrestronTimer();
      RepeatTimer.TimerCallback = Repeat;
    }


    internal ITimer DelayTimer { get; set; }
    internal ITimer RepeatTimer { get; set; }

    public long DelayTimeMs { get; set; }
    public long RepeatTimeMs { get; set; }

    public Action Action { get; set; }

    public void Press() {
      DelayTimer.Start(DelayTimeMs);
    }

    private void StartRepeating(object o) {
      Action();
      RepeatTimer.Start(RepeatTimeMs, RepeatTimeMs, null);
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
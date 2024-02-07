using System;
using AET.Unity.SimplSharp.Timer;

namespace AET.Unity.SimplSharp {
  public class PressHold {
    private ITimer delayTimer;

    public ITimer Timer {
      get {
        return delayTimer ?? (delayTimer = new CrestronTimer { TimerCallback = TimerCallback });
      }
      set {
        delayTimer = value;
        delayTimer.TimerCallback = TimerCallback;
      }
    }

    public Action PressAction { get; set; }
    public Action HoldStartAction { get; set; }
    public Action HoldStopAction { get; set; }
    public bool Holding { get; private set; }
    public int HoldTimeMs { get; set; }

    public void Press() {
      Timer.Start(HoldTimeMs);
    }

    private void TimerCallback(object o) {
      Holding = true;
      HoldStartAction();
    }

    public void Release() {
      if (Timer.IsRunning) {
        Timer.Stop();
        PressAction();
      } else {
        if (!Holding) return;
        HoldStopAction();
        Holding = false;
      }
    }
  }
}
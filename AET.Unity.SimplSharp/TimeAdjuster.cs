﻿using AET.Unity.SimplSharp.Timer;
using System;

namespace AET.Unity.SimplSharp {
  public class TimeAdjuster {

    protected readonly ITimer repeatTimer = new CrestronTimer();

    public TimeAdjuster() {
      SetTime = new TimeSpan();
      SetDefaultDelayTimes();
    }

    public TimeAdjuster(TimeSpan setTime) {
      SetTime = setTime;
      SetDefaultDelayTimes();
    }

    private void SetDefaultDelayTimes() {
      HourDelayTime = 750;
      HourRepeatTime = 500;
      MinuteDelayTime = 333;
      MinuteRepeatTime = 150;
    }

    public long HourDelayTime { get; set; }
    public long HourRepeatTime { get; set; }
    public long MinuteDelayTime { get; set; }
    public long MinuteRepeatTime { get; set; }


    public TimeSpan SetTime { get; set; }

    public virtual void HourUpDnPressHold(int amount) { 
      IncrementHour(amount);
      repeatTimer.Start(HourDelayTime, HourRepeatTime, (o) => IncrementHour(amount));
    }

    public virtual void MinuteUpDnPressHold(int amount) {
      IncrementMinute(amount);
      repeatTimer.Start(MinuteDelayTime, MinuteRepeatTime, (o) => IncrementMinute(amount));
    }

    public virtual void HourMinuteRelease() {
      repeatTimer.Stop();
    }

    public void IncrementHour(int amount) { SetTime = SetTime.AddHours(amount); }
    public void IncrementMinute(int amount) { SetTime = SetTime.AddMinutes(amount); }
  }
}

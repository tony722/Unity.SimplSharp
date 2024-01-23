using AET.Unity.SimplSharp.Timer;
using System;

namespace AET.Unity.SimplSharp {
  public class DateTimeAdjuster {
    protected readonly ITimer repeatTimer = new CrestronTimer();

    public DateTimeAdjuster() {
      DateTime = DateTime.MinValue;
      SetDefaultDelayTimes();
    }

    public DateTimeAdjuster(DateTime dateTime) {
      DateTime = dateTime;
      SetDefaultDelayTimes();
    }

    private void SetDefaultDelayTimes() {
      HourDelayTimeMs = 750;
      HourRepeatTimeMs = 500;
      MinuteDelayTimeMs = 333;
      MinuteRepeatTimeMs = 150;
    }

    public long HourDelayTimeMs { get; set; }
    public long HourRepeatTimeMs { get; set; }
    public long MinuteDelayTimeMs { get; set; }
    public long MinuteRepeatTimeMs { get; set; }


    public DateTime DateTime { get; set; }

    public ushort Hour12 {
      get { return DateTime.Hours12(); }
      set { DateTime = DateTime.SetHour12(value); }
    }

    public ushort Minute {
      get { return (ushort)DateTime.Minute;  }
      set { DateTime = DateTime.SetMinute(value);  }
    }

    public bool IsPm {
      get { return DateTime.IsPm();  }
      set { DateTime.SetPm(value); }
    }

    public bool IsAm {
      get { return DateTime.IsAm(); }
      set { DateTime.SetAm(value); }
    }

    public virtual void HourUpDnPressHold(int amount) {
      IncrementHour(amount);
      repeatTimer.Start(HourDelayTimeMs, HourRepeatTimeMs, (o) => IncrementHour(amount));
    }

    public virtual void MinuteUpDnPressHold(int amount) {
      IncrementMinute(amount);
      repeatTimer.Start(MinuteDelayTimeMs, MinuteRepeatTimeMs, (o) => IncrementMinute(amount));
    }

    public virtual void HourMinuteRelease() {
      repeatTimer.Stop();
    }

    public void IncrementHour(int amount) {
      DateTime = DateTime.AddHours(amount);
    }

    public void IncrementMinute(int amount) {
      DateTime = DateTime.AddMinutes(amount);
    }
  }
}

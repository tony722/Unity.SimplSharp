using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AET.Unity.SimplSharp {
  public static class TimeSpanExtensions {
    public static int Hours12(this TimeSpan timeSpan) {
      var hour = timeSpan.Hours;
      if (hour == 0 || hour == 12) return 12;
      return hour % 12;
    }

    public static bool IsAm(this TimeSpan timeSpan) {
      var hour = timeSpan.Hours;
      if (hour < 12) return true;
      return false;
    }

    public static bool IsPm(this TimeSpan timeSpan) {
      return !timeSpan.IsAm();
    }

    public static TimeSpan To24HourTimeSpan(this TimeSpan timeSpan) {
      return timeSpan.TotalMinutes < 0 ? TimeSpan.FromMinutes((timeSpan.TotalMinutes % 1440) + 1440) : TimeSpan.FromMinutes(timeSpan.TotalMinutes % 1440);
    }
  }
}

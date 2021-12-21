using System;

namespace AET.Unity.SimplSharp.DateTimeProvider {
  public static class DateTimeProvider {
    private static IDateTimeProvider timeProvider;

    public static IDateTimeProvider TimeProvider {
      get {
        if (timeProvider == null) timeProvider = new CrestronDateTimeProvider();
        return timeProvider;
      }
      set {
        timeProvider = value;
      }
    }

    public static DateTime Now {
      get { return TimeProvider.Now; }
    }

    public static DateTime Today {
      get { return TimeProvider.Today; }
    } 

  }
}

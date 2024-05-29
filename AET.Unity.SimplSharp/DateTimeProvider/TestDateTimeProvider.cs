using System;

namespace AET.Unity.SimplSharp.DateTimeProvider {
  public class TestDateTimeProvider : IDateTimeProvider {
    static TestDateTimeProvider() {
      TestDateTime = DateTime.Now; 
    }

    public static DateTime TestDateTime { get; set; }

    public DateTime Now {
      get {
        return TestDateTime;
      }
    }

    public DateTime Today {
      get {
        return new DateTime(TestDateTime.Year, TestDateTime.Month, TestDateTime.Day);
      }
    }

    public TimeSpan TimeOfDay {
      get { return TestDateTime.TimeOfDay;  }
    }
  }
}
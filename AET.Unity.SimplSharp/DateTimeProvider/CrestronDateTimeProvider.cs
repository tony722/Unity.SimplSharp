using System;

namespace AET.Unity.SimplSharp.DateTimeProvider {
  public class CrestronDateTimeProvider : IDateTimeProvider {
    public DateTime Now {
      get { return DateTime.Now; }
    }

    public DateTime Today {
      get { return DateTime.Today; }
    }

  }
}

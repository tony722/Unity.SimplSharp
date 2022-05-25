using System;

namespace AET.Unity.SimplSharp.DateTimeProvider {
  public interface IDateTimeProvider {
    DateTime Now { get; }
    DateTime Today { get; }
    TimeSpan TimeOfDay { get;  }
  }
}
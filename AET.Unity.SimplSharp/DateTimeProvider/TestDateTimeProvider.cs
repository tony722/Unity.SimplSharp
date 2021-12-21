﻿using System;

namespace AET.Unity.SimplSharp.DateTimeProvider {
  public class TestDateTimeProvider : IDateTimeProvider {
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
  }
}
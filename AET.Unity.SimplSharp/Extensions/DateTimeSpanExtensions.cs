using System;

namespace AET.Unity.SimplSharp {
  public static class DateTimeSpanExtensions {
    public static ushort Hours12(this TimeSpan timeSpan) {
      return timeSpan.Hours.ToHours12();
    }

    public static ushort Hours12(this DateTime dateTime) {
      return dateTime.Hour.ToHours12();
    }

    public static ushort ToHours12(this int hour) {
      if (hour == 0 || hour == 12) return 12;
      return (ushort)(hour % 12);
    }

    public static ushort ToHours24(this int hour, bool isPm) {
      if (hour == 12) return (ushort)(isPm ? 12 : 0);
      if (isPm) return (ushort)(hour + 12);
      return (ushort)hour;
    }

    public static bool IsAm(this TimeSpan timeSpan) {
      return timeSpan.Hours.IsAm();
    }

    public static bool IsAm(this DateTime dateTime) {
      return dateTime.Hour.IsAm();
    }

    public static bool IsAm(this int hour24) {
      if (hour24 < 12) return true;
      return false;
    }

    public static bool IsPm(this TimeSpan timeSpan) {
      return !timeSpan.IsAm();
    }

    public static bool IsPm(this DateTime dateTime) {
      return !dateTime.IsAm();
    }

    public static TimeSpan To24HourTimeSpan(this TimeSpan timeSpan) {
      return timeSpan.TotalMinutes < 0 ? TimeSpan.FromMinutes((timeSpan.TotalMinutes % 1440) + 1440) : TimeSpan.FromMinutes(timeSpan.TotalMinutes % 1440);
    }

    public static DateTime SetHour(this DateTime dateTime, int hour) {
      if (hour < 0 || hour > 23) throw new ArgumentOutOfRangeException("hour", "Should be between 0 and 23");
      return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, dateTime.Minute, dateTime.Second);
    }

    public static TimeSpan SetHour(this TimeSpan timeSpan, int hour) {
      if (hour < 0 || hour > 23) throw new ArgumentOutOfRangeException("hour", "Should be between 0 and 23");
      return new TimeSpan(hour, timeSpan.Minutes, timeSpan.Seconds);
    }

    public static DateTime SetHour12(this DateTime dateTime, int hour, bool isPm) {
      if (hour < 1 || hour > 12) throw new ArgumentOutOfRangeException("hour", "Should be between 1 and 12");
      var hour12 = hour.ToHours24(isPm);
      return dateTime.SetHour(hour12);
    }

    public static DateTime SetHour12(this DateTime dateTime, int hour) {
      return dateTime.SetHour12(hour, dateTime.IsPm());
    }

    public static TimeSpan SetHour12(this TimeSpan timeSpan, int hour, bool isPm) {
      if (hour < 1 || hour > 12) throw new ArgumentOutOfRangeException("hour", "Should be between 1 and 12");
      var hour12 = hour.ToHours24(isPm);
      return timeSpan.SetHour(hour12);
    }

    public static DateTime SetMinute(this DateTime dateTime, int minute) {
      if (minute < 0 || minute > 59) throw new ArgumentOutOfRangeException("minute", "Should be between 0 and 59");
      return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, dateTime.Second);
    }

    public static TimeSpan SetMinute(this TimeSpan timeSpan, int minute) {
      if (minute < 0 || minute > 59) throw new ArgumentOutOfRangeException("minute", "Should be between 0 and 59");
      return new TimeSpan(timeSpan.Hours, minute, timeSpan.Seconds);
    }

    public static DateTime SetPm(this DateTime dateTime, bool isPm) {
      return dateTime.SetHour12(dateTime.Hours12(), isPm);
    }

    public static DateTime SetAm(this DateTime dateTime, bool isAm) {
      return dateTime.SetHour12(dateTime.Hours12(), !isAm);
    }

    public static TimeSpan SetPm(this TimeSpan timeSpan, bool isPm) {
      return timeSpan.SetHour12(timeSpan.Hours12(), isPm);
    }

    public static TimeSpan AddMinutes(this TimeSpan timeSpan, int amount) {
      var newTime = timeSpan.Add(TimeSpan.FromMinutes(amount));
      return newTime.To24HourTimeSpan();
    }

    public static TimeSpan AddHours(this TimeSpan timeSpan, int amount) {
      var newTime = timeSpan + TimeSpan.FromHours(amount);
      return newTime.To24HourTimeSpan();
    }
    
    public static TimeSpan SetAm(this TimeSpan timeSpan, bool isAm) {
      return timeSpan.SetHour12(timeSpan.Hours12(), !isAm);
    }

    public static DateTime UnixUtcToDateTime(this long unixUtc) {
      return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixUtc);
    }

    public static long UtcDateTimeToUnix(this DateTime dateTime) {
      return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

  }
}

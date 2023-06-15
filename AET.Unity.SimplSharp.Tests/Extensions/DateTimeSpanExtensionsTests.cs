using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AET.Unity.SimplSharp.Tests.Extensions {
  [TestClass]
  public class DateTimeSpanExtensionsTests {
    [TestMethod]
    public void Hour12_TimeIsMidnight_Returns12() {
      var time = TimeSpan.Parse("0:00");
      time.Hours12().Should().Be(12);
    }

    [DataTestMethod]
    [DataRow(1, 1)]
    [DataRow(3, 3)]
    [DataRow(6, 6)]
    [DataRow(9, 9)]
    [DataRow(12, 12)]
    [DataRow(13, 1)]
    [DataRow(16, 4)]
    [DataRow(20, 8)]
    [DataRow(23, 11)]
    public void Hours12_CalculatesHourCorrectly(int hour24, int hour12) {
      var time = TimeSpan.FromHours(hour24);
      time.Hours12().Should().Be((ushort)hour12);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(3)]
    [DataRow(6)]
    [DataRow(9)]
    [DataRow(11)]
    [TestMethod]
    public void IsAm_HourIsAm_ReturnsTrue(int hour24) {
      var time = TimeSpan.FromHours(hour24);
      time.IsAm().Should().BeTrue();
      time.IsPm().Should().BeFalse();
    }

    [DataTestMethod]
    [DataRow(12, false, 0)]
    [DataRow(1, false, 1)]
    [DataRow(2, false, 2)]
    [DataRow(11, false, 11)]
    [DataRow(12, true, 12)]
    [DataRow(1, true, 13)]
    [DataRow(2, true, 14)]
    [DataRow(11, true, 23)]
    [TestMethod]
    public void ToHours24_RetursCorrectValue(int hour12, bool isPm, int expected) {
      var hour = hour12.ToHours24(isPm);
      hour.Should().Be((ushort)expected);
    }

    [DataTestMethod]
    [DataRow(12)]
    [DataRow(14)]
    [DataRow(18)]
    [DataRow(20)]
    [DataRow(23)]
    public void IsAm_HourIsPm_ReturnsTrue(int hour24) {
      var time = TimeSpan.FromHours(hour24);
      time.IsAm().Should().BeFalse();
      time.IsPm().Should().BeTrue();
    }

    [DataTestMethod]
    [DataRow("0:00:00", -15, "23:45:00")]
    [DataRow("0:00:00", 15, "00:15:00")]
    [DataRow("0:00:00", -1455, "23:45:00")]
    [DataRow("0:00:00", 1455, "00:15:00")]
    [DataRow("0:15:00", -15, "00:00:00")]
    [DataRow("23:45:00", 15, "00:00:00")]
    public void To24HourTimeSpan_CalculatesCorrectResult(string startTime, int interval, string expectedTime) {
      var time = TimeSpan.Parse(startTime).Add(TimeSpan.FromMinutes(interval));
      time.To24HourTimeSpan().Should().Be(TimeSpan.Parse(expectedTime));
    }

    [TestMethod]
    public void DateTime_WorksSameAsTimeSpan() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      time.Hours12().Should().Be(3);
      time.Minute.Should().Be(30);
      time.IsPm().Should().Be(true);
    }

    [TestMethod]
    public void DateTimeSetHour_HourOutOfRange_DoesNotSetHour() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      Action a = () => time.SetHour(29);
      a.ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Should be between 0 and 23\r\nParameter name: hour");
    }

    [TestMethod]
    public void DateTimeSetHour_ValidHour_SetsHourCorrectly() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      var newTime = time.SetHour(10);
      newTime.Hour.Should().Be(10);
    }

    [TestMethod]
    public void TimeSpanSetHour_ValidHour_SetsHourCorrectly() {
      var time = TimeSpan.Parse("3:30:00");
      var newTime = time.SetHour(10);
      newTime.Hours.Should().Be(10);
    }

    [TestMethod]
    public void DateTimeSetHour12_InvalidHour_ThrowsOutOfRangeException() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      Action a = () => time.SetHour12(0, true);
      a.ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Should be between 1 and 12\r\nParameter name: hour");
    }

    [TestMethod]
    public void DateTimeSetHour12_ValidHour_SetsHourCorrectly() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      var newTime = time.SetHour12(10, true);
      newTime.Hour.Should().Be(22);
    }

    [TestMethod]
    public void DateTimeSetHour12_NoAmPmSpecified_PreservesAmPm() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      var newTime = time.SetHour12(10);
      newTime.Hour.Should().Be(22);
      time = DateTime.Parse("2022/10/05 5:30:00 am");
      newTime = time.SetHour12(10);
      newTime.Hour.Should().Be(10);
    }


    [TestMethod]
    public void TimeSpanSetHour12_ValidHour_SetsHourCorrectly() {
      var time = TimeSpan.Parse("3:30:00");
      var newTime = time.SetHour12(10, true);
      newTime.Hours.Should().Be(22);
    }

    [TestMethod]
    public void DateTimeSetMinute_InvalidMinute_ThrowsOutOfRangeException() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      Action a = () => time.SetMinute(66);
      a.ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Should be between 0 and 59\r\nParameter name: minute");
    }

    [TestMethod]
    public void DateTimeSetMinute_ValidMinute_SetsMinuteCorrectly() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      var newTime = time.SetMinute(10);
      newTime.Minute.Should().Be(10);
    }

    [TestMethod]
    public void TimeSpanSetMinute_ValidMinute_SetsMinuteCorrectly() {
      var time = TimeSpan.Parse("3:30:00");
      var newTime = time.SetMinute(10);
      newTime.Minutes.Should().Be(10);
    }

    [TestMethod]
    public void DateTimeSetPm_TrueButAlreadyPm_ReturnsOriginalTime() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      var newTime = time.SetPm(true);
      newTime.ShouldBeEquivalentTo(DateTime.Parse("2022/10/05 3:30:00 pm"));
    }
    
    [TestMethod]
    public void DateTimeSetPm_FalseAndTimeIsPm_ReturnsSameDayAm() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      var newTime = time.SetPm(false);
      newTime.ShouldBeEquivalentTo(DateTime.Parse("2022/10/05 3:30:00 am"));
    }

    [TestMethod]
    public void DateTimeSetPm_TrueAndTimeIsAm_ReturnsSameDayPm() {
      var time = DateTime.Parse("2022/10/05 3:30:00 am");
      var newTime = time.SetPm(true);
      newTime.ShouldBeEquivalentTo(DateTime.Parse("2022/10/05 3:30:00 pm"));
    }

    [TestMethod]
    public void DateTimeSetAm_TrueButAlreadyAm_ReturnsOriginalTime() {
      var time = DateTime.Parse("2022/10/05 3:30:00 am");
      var newTime = time.SetAm(true);
      newTime.ShouldBeEquivalentTo(DateTime.Parse("2022/10/05 3:30:00 am"));
    }
    
    [TestMethod]
    public void DateTimeSetAm_TrueAndTimeIsPm_ReturnsSameDayAm() {
      var time = DateTime.Parse("2022/10/05 3:30:00 pm");
      var newTime = time.SetAm(true);
      newTime.ShouldBeEquivalentTo(DateTime.Parse("2022/10/05 3:30:00 am"));
    }

    [TestMethod]
    public void DateTimeSetAm_FalseAndTimeIsAm_ReturnsSameDayPm() {
      var time = DateTime.Parse("2022/10/05 3:30:00 am");
      var newTime = time.SetAm(false);
      newTime.ShouldBeEquivalentTo(DateTime.Parse("2022/10/05 3:30:00 pm"));
    }

    [TestMethod]
    public void TimeSpanSetPm_TrueButAlreadyPm_ReturnsOriginalTime() {
      var time = TimeSpan.Parse("15:30:00");
      var newTime = time.SetPm(true);
      newTime.ShouldBeEquivalentTo(TimeSpan.Parse("15:30:00"));
    }
    
    [TestMethod]
    public void TimeSpanSetPm_FalseAndTimeIsPm_ReturnsSameDayAm() {
      var time = TimeSpan.Parse("15:30:00");
      var newTime = time.SetPm(false);
      newTime.ShouldBeEquivalentTo(TimeSpan.Parse("3:30:00"));
    }

    [TestMethod]
    public void TimeSpanSetAm_FalseAndTimeIsAm_ReturnsSameDayPm() {
      var time = TimeSpan.Parse("3:30:00");
      var newTime = time.SetAm(false);
      newTime.ShouldBeEquivalentTo(TimeSpan.Parse("15:30:00"));
    }

  }
}

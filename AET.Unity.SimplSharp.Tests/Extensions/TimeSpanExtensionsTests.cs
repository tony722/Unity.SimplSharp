using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AET.Unity.SimplSharp.Tests.Extensions {
  [TestClass]
  public class TimeSpanExtensionsTests {
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
      time.Hours12().Should().Be(hour12);
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
  }
}

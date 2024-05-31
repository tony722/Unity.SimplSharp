using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AET.Unity.SimplSharp.Timer;
using FluentAssertions;

namespace AET.Unity.SimplSharp.Tests {
  [TestClass]
  public class PressHoldRepeaterTests {
    private PressHoldRepeater repeater = new PressHoldRepeater();
    private TestTimer delayTimer = new TestTimer(), repeatTimer = new TestTimer();
    private int counter = 0;

    [TestInitialize]
    public void TestInit() {
      repeater.RepeatTimer = repeatTimer;
      repeater.DelayTimer = delayTimer;
      repeater.Action = () => counter++;
      repeater.RepeatTimeMs = 100;

    }

    [TestMethod]
    public void Press_Release_Increments1() {
      repeater.Press();
      repeater.Release();
      counter.Should().Be(1);
    }

    [TestMethod]
    public void Press_Hold_IncrementsAsLongAsHeld() {
      repeater.Press();
      delayTimer.TimerElapsed();
      repeatTimer.TimerElapsed();
      repeatTimer.TimerElapsed();
      repeatTimer.TimerElapsed();
      repeater.Release();
      repeatTimer.TimerElapsed();
      repeatTimer.TimerElapsed();
      repeatTimer.TimerElapsed();
      counter.Should().Be(3);
    }
  }
}

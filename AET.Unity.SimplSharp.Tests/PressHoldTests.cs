using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AET.Unity.SimplSharp.Timer;
using FluentAssertions;

namespace AET.Unity.SimplSharp.Tests {
  [TestClass]
  public class PressHoldTests {
    private TestTimer timer = new TestTimer();
    private PressHold pressHold = new PressHold();
    private bool pressed = false;
    private bool held = false;
    private bool holding = false;

    [TestInitialize]
    public void Init() {
      pressHold.HoldTimeMs = 100;
      pressHold.Timer = timer;
      pressHold.HoldStartAction = () => {
        held = true;
        holding = true;
      };
      pressHold.HoldStopAction = () => holding = false;

      pressHold.PressAction = () => pressed = true;
    }

    [TestMethod]
    public void PressHold_NotHeld_NotHeldActionRan() {
      pressHold.Press();
      held.Should().BeFalse();
      holding.Should().BeFalse();
      pressed.Should().BeFalse();
      pressHold.Release();
      timer.IsRunning.Should().BeFalse();
      held.Should().BeFalse();
      holding.Should().BeFalse();
      pressed.Should().BeTrue();
    }

    [TestMethod]
    public void Press_Held_HeldActionRan() {
      pressHold.Press();
      held.Should().BeFalse();
      holding.Should().BeFalse();
      pressed.Should().BeFalse();
      timer.TimerElapsed();
      holding.Should().BeTrue();
      held.Should().BeTrue();
      pressHold.Release();
      held.Should().BeTrue();
      holding.Should().BeFalse();
      pressed.Should().BeFalse();
    }
  }
}

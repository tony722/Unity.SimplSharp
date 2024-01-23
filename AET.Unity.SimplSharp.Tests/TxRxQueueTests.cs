using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using AET.Unity.SimplSharp.Timer;
using FluentAssertions;

namespace AET.Unity.SimplSharp.Tests {
  [TestClass]
  public class TxRxQueueTests {
    private TxRxQueue<string> queue;
    private readonly List<string> sentCommands = new List<string>();
    private readonly TestTimer timer = new TestTimer();

    [TestInitialize]
    public void Setup() {
      queue = new TxRxQueue<string> {
        SpaceBetweenCommandsMs = 1,
        TimeoutMs = 1,
        Timer = timer,
        TransmitHandler = (s) => sentCommands.Add(s)
      };
    }


    [TestMethod]
    public void Send_NothingInQueue_SendsImmediately() {
      queue.Send("Test1");
      sentCommands.Should().BeEquivalentTo("Test1");
    }

    [TestMethod]
    public void Send_TwoItemsSimultaneously_OnlySendsFirst() {
      queue.Send("Test1");
      queue.Send("Test2");
      sentCommands.Should().BeEquivalentTo("Test1");
    }

    [TestMethod]
    public void Send_TwoItems_TimeoutExpires_SendsSecondWithoutSpaceBetween() {
      queue.Send("Test1");
      queue.Send("Test2");
      sentCommands.Should().BeEquivalentTo("Test1");
      timer.TimerElapsed(); //timeout
      sentCommands.Should().BeEquivalentTo("Test1","Test2");
    }

    [TestMethod]
    public void Send_TwoItems_TimeoutIsZero_SendsOnlyWithSpaceBetween() {
      queue.TimeoutMs = 0;
      queue.Send("Test1");
      queue.Send("Test2");
      sentCommands.Should().BeEquivalentTo("Test1");
      timer.TimerElapsed(); //space between commands
      sentCommands.Should().BeEquivalentTo("Test1","Test2");
    }

    [TestMethod]
    public void Send_ResponseReceived_ReturnsResponse() {
      string response = null;
      queue.ReceiveHandler("IgnoredResponse1");
      queue.Send("Test1", (value) => response = value);
      queue.ReceiveHandler("Response1");
      queue.ReceiveHandler("IgnoredResponse2");
      response.Should().Be("Response1");
    }

    [TestMethod]
    public void Send_TwoCommands_ResponseReceived_SendSecondCommandBeforeTimerExpires() {
      queue.SpaceBetweenCommandsMs = 0;
      queue.Send("Test1");
      queue.Send("Test2");
      sentCommands.Should().BeEquivalentTo("Test1");
      queue.ReceiveHandler("Response1");
      sentCommands.Should().BeEquivalentTo("Test1","Test2");
    }
    [TestMethod]
    public void Send_TwoCommands_TwoResponsesReceived_SendSecondCommandBeforeTimerExpires() {
      string response1 = null;
      string response2 = null;
      queue.Send("Test1", (s) => response1 = s);
      queue.Send("Test2", (s) => response2 = s);
      sentCommands.Should().BeEquivalentTo("Test1");
      queue.ReceiveHandler("Response1");
      timer.TimerElapsed(); //Space between commands
      queue.ReceiveHandler("Response2");
      sentCommands.Should().BeEquivalentTo("Test1","Test2");
      response1.Should().Be("Response1");
      response2.Should().Be("Response2");
    }
  }
}

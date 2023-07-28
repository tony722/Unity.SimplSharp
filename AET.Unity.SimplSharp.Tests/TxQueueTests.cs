using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AET.Unity.SimplSharp;
using AET.Unity.SimplSharp.Concurrent;
using AET.Unity.SimplSharp.Timer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Unity.SimplSharp.Tests {
  [TestClass]
  public class TxQueueTests {
    private TxQueue<string> queue;
    private readonly List<string> sentCommands = new List<string>();
    private readonly TestTimer timer = new TestTimer();

    [TestInitialize]
    public void Setup() {
      queue = new TxQueue<string>(s => sentCommands.Add(s), 1) {
        Timer = timer,
        Mutex = new TestMutex()
      };
    }


    [TestMethod]
    public void TxQueueIntegrationTest() {
      timer.ElapseImmediately = true;
      var commandsToSend = new string[] {"tx1", "tx2", "tx3", "tx4"};
      Parallel.ForEach(commandsToSend, c => queue.Send(c));
      sentCommands.Count.Should().Be(4);
      sentCommands.Should().Contain(commandsToSend);
    }

    [TestMethod]
    public void Send_WithDelayBefore_CommandSentAfterDelay() {
      queue.Send("T1", 100, 100);
      sentCommands.Count.Should().Be(0, "because no commands should have been sent yet");
      timer.TimerElapsed();
      sentCommands[0].Should().Be("T1");
      timer.TimerElapsed();
      sentCommands.Count.Should().Be(1);
    }

    [TestMethod]
    public void SendLowPriority_QueueIsEmpty_SendsImmediately() {
      queue.SendLowPriority("Test1", "TX");
      queue.SendLowPriority("Test2", "TX");
      sentCommands.Count.Should().Be(1);
      sentCommands[0].Should().Be("Test1");
    }

    [TestMethod]
    public void SendLowPriority_QueueIsBusySoTwoCommandsQueueUp_OnlySecondIsSent() {
      queue.Send("PreCommand");
      queue.SendLowPriority("Test1", "TX");
      queue.SendLowPriority("Test2", "TX");
      timer.TimerElapsed();
      sentCommands.Count.Should().Be(2);
      sentCommands[1].Should().Be("Test2");
    }

    [TestMethod]
    public void SendLowPriority_QueueIsBusySoCommandsInTwoCategoriesQueueUp_BothAreSent() {
      queue.Send("PreCommand");
      queue.SendLowPriority("Test1", "TX1");
      timer.TimerElapsed();
      queue.SendLowPriority("Test2", "TX2");
      queue.Send("MidCommand");
      timer.TimerElapsed();
      queue.SendLowPriority("Test3", "TX2");
      timer.TimerElapsed();
      sentCommands.Count.Should().Be(4);
      sentCommands.Should().BeEquivalentTo(new[] { "PreCommand", "Test1", "MidCommand", "Test3" });
    }
  }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AET.Unity.SimplSharp;
using AET.Unity.SimplSharp.Timer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.SimplSharp.Tests {
  [TestClass]
  public class TxQueueTests {
    [TestMethod]
    public void TxQueueIntegrationTest() {
      var tx = new List<string>();
      var queue = new TxQueue((s) => tx.Add(s),1);
      var timer = new TestTimer();
      queue.Mutex = new TestMutex();
      queue.Timer = timer;
      
      var commandsToSend = new string[] {"tx1", "tx2", "tx3", "tx4"};
      Parallel.ForEach(commandsToSend, (c) => queue.Send(c));
      tx.Count.Should().Be(1);
      timer.TimerElapsed();
      tx.Count.Should().Be(2);
      timer.TimerElapsed();
      tx.Count.Should().Be(3); 
      timer.TimerElapsed();
      tx.Count.Should().Be(4);
      tx.Should().Contain(commandsToSend);
    }
  }
}

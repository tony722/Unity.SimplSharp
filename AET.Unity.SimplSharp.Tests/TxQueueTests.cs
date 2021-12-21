using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AET.Unity.SimplSharp;
using AET.Unity.SimplSharp.Timer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Unity.SimplSharp.Tests {
  [TestClass]
  public class TxQueueTests {
    [TestMethod]
    public void TxQueueIntegrationTest() {
      var tx = new List<string>();
      var queue = new TxQueue<string>((s) => tx.Add(s),1);
      var timer = new TestTimer();
      queue.Mutex = new TestMutex();
      queue.Timer = timer;
      timer.ElapseImmediately = true;

      var commandsToSend = new string[] {"tx1", "tx2", "tx3", "tx4"};
      Parallel.ForEach(commandsToSend, (c) => queue.Send(c));
      Thread.Sleep(10);
      tx.Count.Should().Be(4);
      tx.Should().Contain(commandsToSend);
    }
  }
}

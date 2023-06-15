using System;
using System.Collections.Generic;
using System.Linq;
using AET.Unity.SimplSharp.Timer;

namespace AET.Unity.SimplSharp {
  public class TxQueue<T> {
    private readonly Queue<QueueItem> queue = new Queue<QueueItem>();
    private readonly Action<T> txAction;
    private ITimer timer;
    private readonly Dictionary<string, QueueItem> lowPriorityQueue = new Dictionary<string, QueueItem>();

    public TxQueue(Action<T> txAction, int spaceBetweenCommandsMs) {
      this.txAction = txAction;
      SpaceBetweenCommandsMs = spaceBetweenCommandsMs;
      Timer = new CrestronTimer();
      Mutex = new CrestronMutex();
    }

    #region For Mocking

    public IMutex Mutex { get; set; }
    public IMutex ActionMutex { get; set; }

    public ITimer Timer {
      get { return timer; }
      set {
        timer = value;
        timer.TimerCallback = TimerCallback;
      }
    }

    #endregion

    /// <summary>
    /// The space between commands sent out. If 0 then sends immediately.
    /// </summary>
    public int SpaceBetweenCommandsMs { get; set; }

    public bool Busy {
      get { return Timer.IsRunning || queue.Count > 0; }
    }

    public void Send(T value) {
      Send(value, SpaceBetweenCommandsMs);
    }

    public void Send(T value, int spaceAfterCommand) {
      var valToSend = new QueueItem(value, spaceAfterCommand);
      Mutex.Enter();
      queue.Enqueue(valToSend);
      TriggerSend();
      Mutex.Exit();
    }

    /// <summary>
    /// Queues a low-priority command to be sent if the queue is empty.
    /// If it recevives a new command while there's one waiting here (in the same category), it will be replaced by the new one
    /// </summary>
    /// <param name="value">The value to send</param>
    /// <param name="category">One command per category can be sent</param>
    public void SendLowPriority(T value, string category) {
      SendLowPriority(value, category, SpaceBetweenCommandsMs);
    }

    public void SendLowPriority(T value, string category, int spaceAfterCommand) {
      var valueToSend = new QueueItem(value, spaceAfterCommand);
      Mutex.Enter();
      if (Busy) {
        lowPriorityQueue[category] = valueToSend;
      }
      else {
        queue.Enqueue(valueToSend);
        TriggerSend();
      }
      Mutex.Exit();
    }

    private void TriggerSend() {
      if (queue.Count == 0) {
        SendWaitingLowPriorityCommand();
        return;
      }
      if (Timer.IsRunning) return;
      var item = queue.Peek();
      txAction(item.Value);
      if (item.SpaceBetweenCommandsMs > 0) Timer.Start(item.SpaceBetweenCommandsMs);
      else TimerCallback(null);
    }

    private void SendWaitingLowPriorityCommand() {
      if(lowPriorityQueue.Count == 0) return;
      var firstKey = lowPriorityQueue.Keys.First();
      var value = lowPriorityQueue[firstKey];
      lowPriorityQueue.Remove(firstKey);
      queue.Enqueue(value);
      TriggerSend();
    }

    private void TimerCallback(object o) {
      Mutex.Enter();
      queue.Dequeue();
      TriggerSend();
      Mutex.Exit();
    }

    private class QueueItem {
      public QueueItem(T value, int spaceBetweenCommandsMs) {
        Value = value;
        SpaceBetweenCommandsMs = spaceBetweenCommandsMs;
      }
      public T Value { get; private set; }
      public int SpaceBetweenCommandsMs { get; private set; }
    }
  }
}
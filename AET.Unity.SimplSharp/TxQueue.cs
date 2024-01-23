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
    private readonly AnyKeyDictionary<string, bool> lowPriorityQueueDisabled = new AnyKeyDictionary<string, bool>();
    private IMutex mutex;

    public TxQueue(Action<T> txAction, int spaceBetweenCommandsMs) {
      this.txAction = txAction;
      SpaceBetweenCommandsMs = spaceBetweenCommandsMs;
    }

    #region For Mocking

    public IMutex Mutex {
      get { return mutex ?? (mutex = new CrestronMutex()); } 
      set { mutex = value; }
    }

    public ITimer Timer {
      get { return timer ?? (Timer = new CrestronTimer()); }
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

    public void Send(T value, int spaceAfterCommandMs) {
      Send(value, 0, spaceAfterCommandMs);
    }

    public void Send(T value, int spaceBeforeCommandMs, int spaceAfterCommandMs) {
      var valToSend = new QueueItem(value, spaceBeforeCommandMs, spaceAfterCommandMs);
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
      if (lowPriorityQueueDisabled[category]) return;
      var valueToSend = new QueueItem(value, 0, spaceAfterCommand);
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

    public void DisableLowPriorityQueue(string category) {
      lowPriorityQueueDisabled[category] = true;
      lowPriorityQueue.Remove(category);
    }

    public void EnableLowPriorityQueue(string category) {
      lowPriorityQueueDisabled[category] = false;
    }

    private void TriggerSend() {
      if (queue.Count == 0) {
        SendWaitingLowPriorityCommand();
        return;
      }
      if (Timer.IsRunning) return;
      var item = queue.Peek();
      if (item.SpaceBeforeCommandMs > 0) {
        Timer.Start(item.SpaceBeforeCommandMs);
        return;
      }
      txAction(item.Value);
      if (item.SpaceBetweenCommandsMs > 0) Timer.Start(item.SpaceBetweenCommandsMs);
      else TimerCallback(null);
    }

    private void SendWaitingLowPriorityCommand() {
      if(lowPriorityQueue.Count == 0) return;
      var firstKey = lowPriorityQueue.Keys.First();
      var value = lowPriorityQueue[firstKey];
      lowPriorityQueue.Remove(firstKey);
      if (!lowPriorityQueueDisabled[firstKey]) {
        queue.Enqueue(value);
        TriggerSend();
      }
    }

    private void TimerCallback(object o) {
      Mutex.Enter();
      var item = queue.Peek();
      if (item.SpaceBeforeCommandMs > 0) {
        item.SpaceBeforeCommandMs = 0;
      } else {
        queue.Dequeue();
      }
      TriggerSend();
      Mutex.Exit();
    }

    private class QueueItem {
      public QueueItem(T value, int spaceBeforeCommandMs, int spaceBetweenCommandsMs) {
        Value = value;
        SpaceBeforeCommandMs = spaceBeforeCommandMs;
        SpaceBetweenCommandsMs = spaceBetweenCommandsMs;
      }
      public T Value { get; private set; }
      public int SpaceBetweenCommandsMs { get; private set; }
      public int SpaceBeforeCommandMs { get; set; }
    }
  }
}
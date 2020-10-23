using System;
using System.Collections.Generic;
using AET.Unity.SimplSharp.Timer;

namespace AET.Unity.SimplSharp {
  public class TxQueue<T> {
    private readonly Queue<T> queue = new Queue<T>();
    private readonly Action<T> txAction;
    private ITimer timer;

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
      Mutex.Enter();
      queue.Enqueue(value);
      TriggerSend();
      Mutex.Exit();
    }

    private void TriggerSend() {
      if (queue.Count == 0) return;
      if (Timer.IsRunning) return;
      txAction(queue.Peek());
      if (SpaceBetweenCommandsMs > 0) Timer.Start(SpaceBetweenCommandsMs);
      else TimerCallback(null);
    }

    private void TimerCallback(object o) {
      Mutex.Enter();
      queue.Dequeue();
      TriggerSend();
      Mutex.Exit();
    }
  }
}
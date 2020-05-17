using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AET.Unity.SimplSharp.Timer;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp {
  public class TxQueue {
    private Queue<string> queue = new Queue<string>();
    private Action<string> txAction;
    private ITimer timer;

    public TxQueue(Action<string> txAction, int spaceBetweenCommandsMs) {
      this.txAction = txAction;
      SpaceBetweenCommandsMs = spaceBetweenCommandsMs;
      Timer = new CrestronTimer();
      Mutex = new CrestronMutex();
    }

    public IMutex Mutex { get; set; }
    public IMutex ActionMutex { get; set; }
    public ITimer Timer {
      get { return timer; }
      set {
        timer = value;
        timer.TimerCallback = TimerCallback;
      }
    }
    
    /// <summary>
    /// The space between commands sent out. Should be at least a few ms. 0 may cause problems.
    /// </summary>
    public int SpaceBetweenCommandsMs { get; set; }

    public bool Busy {
      get { return Timer.IsRunning || queue.Count > 0; }
    }

    public void Send(string value) {
      Mutex.Enter();
      queue.Enqueue(value);
      TriggerSend();
      Mutex.Exit();
    }

    private void TriggerSend() {
      if (queue.Count == 0) return;
      if (Timer.IsRunning) return;
      txAction(queue.Peek());
      Timer.Start(SpaceBetweenCommandsMs);
    }
    private void TimerCallback() {
      Mutex.Enter();
      queue.Dequeue();
      TriggerSend();
      Mutex.Exit();
    }
  }
}

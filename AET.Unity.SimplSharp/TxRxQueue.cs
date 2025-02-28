using System;
using System.Collections.Generic;
using AET.Unity.SimplSharp.Timer;

namespace AET.Unity.SimplSharp {
  /// <summary>
  /// This class is designed to handle communication with a device or object
  /// that only transmits (to ReceiveHandler) in response to a command sent
  /// (via TransmitHandler), and does not prefix its transmitted data with an
  /// indication of what the response is.
  ///
  /// This class can also handle sending commands where a response is not received
  /// by specifying timeout = 0 for that command;
  /// </summary>
  public class TxRxQueue<T> {
    private readonly Queue<QueueItem> queue = new Queue<QueueItem>();
    private ITimer timer;
    private CrestronMutex mutex = new CrestronMutex();

    public TxRxQueue() { }

    public Action<T> TransmitHandler { get; set; }

    public void ReceiveHandler(T response) {
      var item = mutex.ThreadsafeExecute(() => {
        if (queue.Count == 0) return null;
        var i = queue.Peek();
        if (!i.Sent) return null;
        Timer.Stop();
        queue.Dequeue();
        return i;
      });
      if(item == null) return;
      if (item.ResponseHandler != null) item.ResponseHandler(response);
      if (item.SpaceBetweenCommandsMs > 0) Timer.Start(SpaceBetweenCommandsMs);
      else TriggerSend();
    }

    public ITimer Timer {
      get { return timer ?? (Timer = new CrestronTimer()); }
      set { 
        timer = value; 
        timer.TimerCallback = TimerCallback;
      }
    }

    public int SpaceBetweenCommandsMs { get; set; }
    public int TimeoutMs { get; set; }

    public bool Busy {
      get { return Timer.IsRunning || queue.Count > 0; }
    }

    public void Send(T value) { Send(value, true); }

    public void Send(T value, bool waitForResponse) {
      var valToSend = new QueueItem(value, SpaceBetweenCommandsMs, waitForResponse ? TimeoutMs : 0);
      Send(valToSend);
    }

    public void Send(T value, Action<T> responseHandler) {
      var valToSend = new QueueItem(value, SpaceBetweenCommandsMs, TimeoutMs) { ResponseHandler = responseHandler };
      Send(valToSend);
    }

    private void Send(QueueItem value) {
      queue.Enqueue(value);
      TriggerSend();
    }
    
    private void TriggerSend() {
      if (queue.Count == 0) return;
      if (Timer.IsRunning) return;
      var item = mutex.ThreadsafeExecute(() => queue.Peek());
      if (item == null) {
        ErrorMessage.Warn("Unity.SimplSharp.TxRxQueue: queue.Peek item is null.");
        return;
      }
      if (TransmitHandler == null) {
        ErrorMessage.Warn("Unity.SimplSharp.TxRxQueue: TransmitHandler is null.");
        return;        
      }
      item.Sent = true;
      if (item.TimeoutMs > 0) {
        Timer.Start(item.TimeoutMs);
        TransmitHandler(item.Value);
      } else {
        TransmitHandler(item.Value);
        if (item.SpaceBetweenCommandsMs > 0) Timer.Start(item.SpaceBetweenCommandsMs);
        else TimerCallback(null);
      }
    }

    private void TimerCallback(object o) {
      mutex.ThreadsafeExecute(() => {
        if (queue.Count != 0 && queue.Peek().Sent) queue.Dequeue();
      });
      TriggerSend();
    }

    private class QueueItem {
      public QueueItem(T value, int spaceBetweenCommandsMs, int timeoutMs) {
        Value = value;
        SpaceBetweenCommandsMs = spaceBetweenCommandsMs;
        TimeoutMs = timeoutMs;
      }
      public bool Sent { get; set; }
      public T Value { get; private set; }
      public int TimeoutMs { get; set; }
      public int SpaceBetweenCommandsMs { get; set; }
      public Action<T> ResponseHandler { get; set; }
    }
  }
}

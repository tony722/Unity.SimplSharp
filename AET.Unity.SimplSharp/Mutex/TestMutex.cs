using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AET.Unity.SimplSharp {
  public class TestMutex : IMutex {
    private readonly object mutex = new object();
    public void Enter() {
      Monitor.Enter(mutex);
    }

    public bool TryEnter() {
      return Monitor.TryEnter(mutex);
    }

    public void Exit() {
      Monitor.Exit(mutex);
    }
  }
}

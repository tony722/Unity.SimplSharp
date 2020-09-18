using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp {
  public class CrestronMutex : IMutex {
    private readonly object mutex = new object();
    public void Enter() {
      CMonitor.Enter(mutex);
    }

    public bool TryEnter() {
      return CMonitor.TryEnter(mutex);
    }

    public void Exit() {
      try {
        CMonitor.Exit(mutex);
      } catch { }
    }
  }
}

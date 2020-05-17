using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AET.Unity.SimplSharp {
  public interface IMutex {
    void Enter();
    bool TryEnter();
    void Exit();
  }
}

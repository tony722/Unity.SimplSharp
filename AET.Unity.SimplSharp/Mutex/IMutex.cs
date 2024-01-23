using System;

namespace AET.Unity.SimplSharp {
  public abstract class IMutex {
    public abstract void Enter();
    public abstract bool TryEnter();
    public abstract void Exit();

    public void ThreadsafeExecute(Action action) {
      Enter();
      try { action(); } 
      finally { Exit(); }
    }

    public T ThreadsafeExecute<T>(Func<T> action) {
      Enter();
      try { return action(); } 
      finally { Exit(); }
    }
   }
}
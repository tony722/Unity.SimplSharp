using System;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp {
  public class IndexedProperty<TIndex, TValue> {
    private Func<TIndex, TValue> getFunc;
    private Action<TIndex, TValue> setAction;
    
    public IndexedProperty(Func<TIndex, TValue> getFunc) {
      this.getFunc = getFunc;
    }
    public IndexedProperty(Action<TIndex, TValue> setAction) {
      this.setAction = setAction;
    }
    public IndexedProperty(Func<TIndex, TValue> getFunc, Action<TIndex, TValue> setAction) {
      this.getFunc = getFunc;
      this.setAction = setAction;
    }
    
    public TValue this[TIndex index] {
      get { return getFunc(index); }
      set { setAction(index, value); }
    }
  }
}
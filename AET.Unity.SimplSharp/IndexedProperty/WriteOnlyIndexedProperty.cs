using System;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp {
  public class WriteOnlyIndexedProperty<TIndex, TValue> {
    private Action<TIndex, TValue> setAction;
    
    public WriteOnlyIndexedProperty(Action<TIndex, TValue> setAction) {
      this.setAction = setAction;
    }
    public TValue this[TIndex index] {
      set { setAction(index, value); }
    }
  }
}
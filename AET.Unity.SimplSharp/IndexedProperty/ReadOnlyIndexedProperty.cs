using System;

namespace AET.Unity.SimplSharp {
  public class ReadOnlyIndexedProperty<TIndex, TValue> {
    private Func<TIndex, TValue> getFunc;
    
    public ReadOnlyIndexedProperty(Func<TIndex, TValue> getFunc) {
      this.getFunc = getFunc;
    }
    
    public TValue this[TIndex index] {
      get { return getFunc(index); }
    }
  }
}
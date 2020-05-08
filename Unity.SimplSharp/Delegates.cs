 using Crestron.SimplSharp;

namespace AET.SimplSharp {
  public delegate void SetUshortOutputArrayDelegate(ushort index, ushort value);
  public delegate void SetAllUshortOutputArrayDelegate(ushort value);
  public delegate void SetUshortOutputDelegate(ushort value);

  public delegate void SetStringOutputArrayDelegate(ushort index, SimplSharpString value);
  public delegate void SetAllStringOutputArrayDelegate(SimplSharpString value);
  public delegate void SetStringOutputDelegate(SimplSharpString value);

  public delegate void PulseBooleanOutputDelegate();
  public delegate void PulseBooleanOutputArrayDelegate(ushort index);

  public delegate void PulseSequenceOutputDelegate(SimplSharpString value);

  public delegate ushort GetUshortInputDelegate();
  public delegate ushort GetUshortOutputDelegate();
  public delegate ushort GetUshortOutputArrayDelegate(ushort index); 

  public delegate void UshortInputChangedDelegate(ushort value);

  public class StringOutputArrayIndexer : IIndexer<string> {
    SetStringOutputArrayDelegate setAction;
    public StringOutputArrayIndexer(SetStringOutputArrayDelegate setAction) { this.setAction = setAction;  }
    public string this[int index] { set { setAction((ushort)(index + 1), value); } }
  }

  public class UshortOutputArrayIndexer : IIndexer<int> {
    SetUshortOutputArrayDelegate setAction;
    public UshortOutputArrayIndexer(SetUshortOutputArrayDelegate setAction) { this.setAction = setAction; }
    public int this[int index] { set { setAction((ushort)(index + 1), (ushort)value); } }
  }

  public class BoolOutputArrayIndexer : IIndexer<bool> {
    SetUshortOutputArrayDelegate setAction;
    public BoolOutputArrayIndexer(SetUshortOutputArrayDelegate setAction) { this.setAction = setAction; }
    public bool this[int index] { set { setAction((ushort)(index + 1), value.ToUshort()); } }

  }
}
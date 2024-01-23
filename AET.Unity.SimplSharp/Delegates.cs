using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp {
  public delegate void TriggerDelegate();

  public delegate void TriggerArrayDelegate(ushort index);
  
  public delegate void SetBoolOutputDelegate(bool value);

  public delegate void SetUshortOutputArrayDelegate(ushort index, ushort value);

  public delegate void SetAllUshortOutputArrayDelegate(ushort value);

  public delegate void SetUshortOutputDelegate(ushort value);

  public delegate void SetShortOutputArrayDelegate(ushort index, short value);

  public delegate void SetAllShortOutputArrayDelegate(short value);

  public delegate void SetShortOutputDelegate(short value);

  public delegate void SetStringOutputArrayDelegate(ushort index, SimplSharpString value);

  public delegate void SetAllStringOutputArrayDelegate(SimplSharpString value);

  public delegate void SetStringOutputDelegate(SimplSharpString value);

  public delegate void PulseBooleanOutputDelegate();

  public delegate void PulseBooleanOutputArrayDelegate(ushort index);

  public delegate void PulseSequenceOutputDelegate(SimplSharpString value);

  public delegate ushort GetUshortInputDelegate();

  public delegate ushort GetUshortOutputDelegate();

  public delegate ushort GetUshortInputArrayDelegate(ushort index);

  public delegate ushort GetUshortOutputArrayDelegate(ushort index);


}
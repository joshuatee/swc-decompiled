using StaRTS.Main.Models.Battle.Replay;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1100 : $UnityType
	{
		public unsafe $UnityType1100()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 639628) = ldftn($Invoke0);
			*(data + 639656) = ldftn($Invoke1);
			*(data + 639684) = ldftn($Invoke2);
			*(data + 639712) = ldftn($Invoke3);
			*(data + 639740) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new HeroAbilityAction((UIntPtr)0);
		}
	}
}

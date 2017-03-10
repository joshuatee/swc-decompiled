using StaRTS.Main.Models.Commands;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1111 : $UnityType
	{
		public unsafe $UnityType1111()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 641084) = ldftn($Invoke0);
			*(data + 641112) = ldftn($Invoke1);
			*(data + 641140) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BattleIdResponse((UIntPtr)0);
		}
	}
}

using StaRTS.Main.Models.Battle;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1087 : $UnityType
	{
		public unsafe $UnityType1087()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 635316) = ldftn($Invoke0);
			*(data + 635344) = ldftn($Invoke1);
			*(data + 635372) = ldftn($Invoke2);
			*(data + 635400) = ldftn($Invoke3);
			*(data + 635428) = ldftn($Invoke4);
			*(data + 635456) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DefenseWave((UIntPtr)0);
		}
	}
}

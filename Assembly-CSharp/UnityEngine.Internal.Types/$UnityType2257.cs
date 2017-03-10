using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2257 : $UnityType
	{
		public unsafe $UnityType2257()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 900224) = ldftn($Invoke0);
			*(data + 900252) = ldftn($Invoke1);
			*(data + 900280) = ldftn($Invoke2);
			*(data + 900308) = ldftn($Invoke3);
			*(data + 900336) = ldftn($Invoke4);
			*(data + 900364) = ldftn($Invoke5);
			*(data + 900392) = ldftn($Invoke6);
			*(data + 900420) = ldftn($Invoke7);
			*(data + 900448) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new UXInput((UIntPtr)0);
		}
	}
}

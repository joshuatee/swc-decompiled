using StaRTS.Main.Models.Commands;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1304 : $UnityType
	{
		public unsafe $UnityType1304()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 649316) = ldftn($Invoke0);
			*(data + 649344) = ldftn($Invoke1);
			*(data + 649372) = ldftn($Invoke2);
			*(data + 649400) = ldftn($Invoke3);
			*(data + 649428) = ldftn($Invoke4);
			*(data + 649456) = ldftn($Invoke5);
			*(data + 649484) = ldftn($Invoke6);
			*(data + 649512) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new GetReplayResponse((UIntPtr)0);
		}
	}
}

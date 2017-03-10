using StaRTS.Main.Controllers.ServerMessages;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType905 : $UnityType
	{
		public unsafe $UnityType905()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 590180) = ldftn($Invoke0);
			*(data + 590208) = ldftn($Invoke1);
			*(data + 590236) = ldftn($Invoke2);
			*(data + 590264) = ldftn($Invoke3);
			*(data + 590292) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SquadServerMessage((UIntPtr)0);
		}
	}
}

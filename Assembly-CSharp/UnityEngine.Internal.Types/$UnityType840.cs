using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType840 : $UnityType
	{
		public unsafe $UnityType840()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 574976) = ldftn($Invoke0);
			*(data + 575004) = ldftn($Invoke1);
			*(data + 575032) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new WarBoardState((UIntPtr)0);
		}
	}
}

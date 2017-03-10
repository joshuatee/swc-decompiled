using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType837 : $UnityType
	{
		public unsafe $UnityType837()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 574500) = ldftn($Invoke0);
			*(data + 574528) = ldftn($Invoke1);
			*(data + 574556) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new NeighborVisitState((UIntPtr)0);
		}
	}
}

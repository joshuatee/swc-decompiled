using StaRTS.GameBoard.Pathfinding;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType662 : $UnityType
	{
		public unsafe $UnityType662()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 516736) = ldftn($Invoke0);
			*(data + 516764) = ldftn($Invoke1);
			*(data + 516792) = ldftn($Invoke2);
			*(data + 516820) = ldftn($Invoke3);
			*(data + 516848) = ldftn($Invoke4);
			*(data + 516876) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new PathingManager((UIntPtr)0);
		}
	}
}

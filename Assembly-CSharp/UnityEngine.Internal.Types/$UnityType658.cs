using StaRTS.GameBoard.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType658 : $UnityType
	{
		public unsafe $UnityType658()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 516260) = ldftn($Invoke0);
			*(data + 516288) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SizeComponent((UIntPtr)0);
		}
	}
}

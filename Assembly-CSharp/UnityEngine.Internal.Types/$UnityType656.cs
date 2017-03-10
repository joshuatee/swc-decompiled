using StaRTS.GameBoard.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType656 : $UnityType
	{
		public unsafe $UnityType656()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 515952) = ldftn($Invoke0);
			*(data + 515980) = ldftn($Invoke1);
			*(data + 516008) = ldftn($Invoke2);
			*(data + 516036) = ldftn($Invoke3);
			*(data + 516064) = ldftn($Invoke4);
			*(data + 516092) = ldftn($Invoke5);
			*(data + 516120) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new FilterComponent((UIntPtr)0);
		}
	}
}

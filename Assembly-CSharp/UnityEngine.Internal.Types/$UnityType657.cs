using StaRTS.GameBoard.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType657 : $UnityType
	{
		public unsafe $UnityType657()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 516148) = ldftn($Invoke0);
			*(data + 516176) = ldftn($Invoke1);
			*(data + 516204) = ldftn($Invoke2);
			*(data + 516232) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new FlagStamp((UIntPtr)0);
		}
	}
}

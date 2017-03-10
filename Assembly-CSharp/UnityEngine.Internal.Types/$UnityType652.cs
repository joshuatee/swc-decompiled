using StaRTS.GameBoard;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType652 : $UnityType
	{
		public unsafe $UnityType652()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 515028) = ldftn($Invoke0);
			*(data + 515056) = ldftn($Invoke1);
			*(data + 515084) = ldftn($Invoke2);
			*(data + 515112) = ldftn($Invoke3);
			*(data + 515140) = ldftn($Invoke4);
			*(data + 515168) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new BoardUtils((UIntPtr)0);
		}
	}
}

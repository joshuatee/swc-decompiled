using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType686 : $UnityType
	{
		public unsafe $UnityType686()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 523428) = ldftn($Invoke0);
			*(data + 523456) = ldftn($Invoke1);
			*(data + 523484) = ldftn($Invoke2);
			*(data + 523512) = ldftn($Invoke3);
			*(data + 523540) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new BoardController((UIntPtr)0);
		}
	}
}

using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType830 : $UnityType
	{
		public unsafe $UnityType830()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 573884) = ldftn($Invoke0);
			*(data + 573912) = ldftn($Invoke1);
			*(data + 573940) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new EditBaseState((UIntPtr)0);
		}
	}
}

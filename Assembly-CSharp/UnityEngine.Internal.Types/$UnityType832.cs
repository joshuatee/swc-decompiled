using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType832 : $UnityType
	{
		public unsafe $UnityType832()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 573996) = ldftn($Invoke0);
			*(data + 574024) = ldftn($Invoke1);
			*(data + 574052) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GalaxyState((UIntPtr)0);
		}
	}
}

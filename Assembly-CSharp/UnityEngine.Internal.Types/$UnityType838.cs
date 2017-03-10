using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType838 : $UnityType
	{
		public unsafe $UnityType838()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 574584) = ldftn($Invoke0);
			*(data + 574612) = ldftn($Invoke1);
			*(data + 574640) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new VideoPlayBackState((UIntPtr)0);
		}
	}
}

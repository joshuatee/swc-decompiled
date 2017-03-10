using StaRTS.Main.Models.Commands.Player.Building.Collect;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1483 : $UnityType
	{
		public unsafe $UnityType1483()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 656428) = ldftn($Invoke0);
			*(data + 656456) = ldftn($Invoke1);
			*(data + 656484) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BuildingCollectRequest((UIntPtr)0);
		}
	}
}

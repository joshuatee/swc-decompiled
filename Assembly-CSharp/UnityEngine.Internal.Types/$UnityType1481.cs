using StaRTS.Main.Models.Commands.Player.Building.Clear;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1481 : $UnityType
	{
		public unsafe $UnityType1481()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 656288) = ldftn($Invoke0);
			*(data + 656316) = ldftn($Invoke1);
			*(data + 656344) = ldftn($Invoke2);
			*(data + 656372) = ldftn($Invoke3);
			*(data + 656400) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new BuildingClearRequest((UIntPtr)0);
		}
	}
}

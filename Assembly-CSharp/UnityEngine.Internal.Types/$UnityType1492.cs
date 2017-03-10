using StaRTS.Main.Models.Commands.Player.Building.Move;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1492 : $UnityType
	{
		public unsafe $UnityType1492()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 656960) = ldftn($Invoke0);
			*(data + 656988) = ldftn($Invoke1);
			*(data + 657016) = ldftn($Invoke2);
			*(data + 657044) = ldftn($Invoke3);
			*(data + 657072) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new BuildingMoveRequest((UIntPtr)0);
		}
	}
}

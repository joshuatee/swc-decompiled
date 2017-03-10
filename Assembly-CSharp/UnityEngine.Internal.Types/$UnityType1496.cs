using StaRTS.Main.Models.Commands.Player.Building.Move;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1496 : $UnityType
	{
		public unsafe $UnityType1496()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 657184) = ldftn($Invoke0);
			*(data + 657212) = ldftn($Invoke1);
			*(data + 657240) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new WarBaseSaveRequest((UIntPtr)0);
		}
	}
}

using StaRTS.Main.Controllers.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType798 : $UnityType
	{
		public unsafe $UnityType798()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 569488) = ldftn($Invoke0);
			*(data + 569516) = ldftn($Invoke1);
			*(data + 569544) = ldftn($Invoke2);
			*(data + 569572) = ldftn($Invoke3);
			*(data + 569600) = ldftn($Invoke4);
			*(data + 569628) = ldftn($Invoke5);
			*(data + 569656) = ldftn($Invoke6);
			*(data + 569684) = ldftn($Invoke7);
			*(data + 569712) = ldftn($Invoke8);
			*(data + 569740) = ldftn($Invoke9);
			*(data + 569768) = ldftn($Invoke10);
			*(data + 569796) = ldftn($Invoke11);
			*(data + 569824) = ldftn($Invoke12);
			*(data + 569852) = ldftn($Invoke13);
			*(data + 569880) = ldftn($Invoke14);
			*(data + 569908) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new EntityFactory((UIntPtr)0);
		}
	}
}

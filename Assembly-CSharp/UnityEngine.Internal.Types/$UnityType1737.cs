using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1737 : $UnityType
	{
		public unsafe $UnityType1737()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 674796) = ldftn($Invoke0);
			*(data + 674824) = ldftn($Invoke1);
			*(data + 674852) = ldftn($Invoke2);
			*(data + 674880) = ldftn($Invoke3);
			*(data + 674908) = ldftn($Invoke4);
			*(data + 674936) = ldftn($Invoke5);
			*(data + 674964) = ldftn($Invoke6);
			*(data + 674992) = ldftn($Invoke7);
			*(data + 675020) = ldftn($Invoke8);
			*(data + 675048) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new TransformComponent((UIntPtr)0);
		}
	}
}

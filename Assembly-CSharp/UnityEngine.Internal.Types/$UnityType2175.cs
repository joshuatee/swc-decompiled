using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2175 : $UnityType
	{
		public unsafe $UnityType2175()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 867856) = ldftn($Invoke0);
			*(data + 867884) = ldftn($Invoke1);
			*(data + 867912) = ldftn($Invoke2);
			*(data + 867940) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new EntityViewParams((UIntPtr)0);
		}
	}
}

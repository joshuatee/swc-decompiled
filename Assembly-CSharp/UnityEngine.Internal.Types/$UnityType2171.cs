using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2171 : $UnityType
	{
		public unsafe $UnityType2171()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 866708) = ldftn($Invoke0);
			*(data + 866736) = ldftn($Invoke1);
			*(data + 866764) = ldftn($Invoke2);
			*(data + 866792) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new AddOnViewParams((UIntPtr)0);
		}
	}
}

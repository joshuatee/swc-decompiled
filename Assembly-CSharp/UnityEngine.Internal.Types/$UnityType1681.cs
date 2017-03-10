using StaRTS.Main.Models.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1681 : $UnityType
	{
		public unsafe $UnityType1681()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 666340) = ldftn($Invoke0);
			*(data + 666368) = ldftn($Invoke1);
			*(data + 666396) = ldftn($Invoke2);
			*(data + 666424) = ldftn($Invoke3);
			*(data + 666452) = ldftn($Invoke4);
			*(data + 666480) = ldftn($Invoke5);
			*(data + 666508) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new EntityRef((UIntPtr)0);
		}
	}
}

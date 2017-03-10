using StaRTS.Main.Models.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1682 : $UnityType
	{
		public unsafe $UnityType1682()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 666536) = ldftn($Invoke0);
			*(data + 666564) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SmartEntity((UIntPtr)0);
		}
	}
}

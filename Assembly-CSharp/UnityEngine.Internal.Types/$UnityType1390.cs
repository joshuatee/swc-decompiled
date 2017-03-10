using StaRTS.Main.Models.Commands.Crates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1390 : $UnityType
	{
		public unsafe $UnityType1390()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 651920) = ldftn($Invoke0);
			*(data + 651948) = ldftn($Invoke1);
			*(data + 651976) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new CrateDataResponse((UIntPtr)0);
		}
	}
}

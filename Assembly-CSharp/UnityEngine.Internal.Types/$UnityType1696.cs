using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1696 : $UnityType
	{
		public unsafe $UnityType1696()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 667936) = ldftn($Invoke0);
			*(data + 667964) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new CivilianComponent((UIntPtr)0);
		}
	}
}

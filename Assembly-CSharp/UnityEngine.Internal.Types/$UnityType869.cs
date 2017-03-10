using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType869 : $UnityType
	{
		public unsafe $UnityType869()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581668) = ldftn($Invoke0);
			*(data + 581696) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DestroyBuildingIdObjectiveProcessor((UIntPtr)0);
		}
	}
}

using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType882 : $UnityType
	{
		public unsafe $UnityType882()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 583040) = ldftn($Invoke0);
			*(data + 583068) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TrainTroopTypeObjectiveProcessor((UIntPtr)0);
		}
	}
}

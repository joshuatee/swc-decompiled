using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType881 : $UnityType
	{
		public unsafe $UnityType881()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 582984) = ldftn($Invoke0);
			*(data + 583012) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TrainTroopIdObjectiveProcessor((UIntPtr)0);
		}
	}
}

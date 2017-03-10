using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2435 : $UnityType
	{
		public unsafe $UnityType2435()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 956224) = ldftn($Invoke0);
			*(data + 956252) = ldftn($Invoke1);
			*(data + 956280) = ldftn($Invoke2);
			*(data + 956308) = ldftn($Invoke3);
			*(data + 956336) = ldftn($Invoke4);
			*(data + 956364) = ldftn($Invoke5);
			*(data + 956392) = ldftn($Invoke6);
			*(data + 956420) = ldftn($Invoke7);
			*(data + 956448) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new PlanetDetailsPlanetInfoViewModule((UIntPtr)0);
		}
	}
}

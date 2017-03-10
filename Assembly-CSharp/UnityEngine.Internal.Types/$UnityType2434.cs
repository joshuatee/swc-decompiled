using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2434 : $UnityType
	{
		public unsafe $UnityType2434()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 956000) = ldftn($Invoke0);
			*(data + 956028) = ldftn($Invoke1);
			*(data + 956056) = ldftn($Invoke2);
			*(data + 956084) = ldftn($Invoke3);
			*(data + 956112) = ldftn($Invoke4);
			*(data + 956140) = ldftn($Invoke5);
			*(data + 956168) = ldftn($Invoke6);
			*(data + 956196) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new PlanetDetailsObjectivesViewModule((UIntPtr)0);
		}
	}
}

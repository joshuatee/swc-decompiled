using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2441 : $UnityType
	{
		public unsafe $UnityType2441()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 957708) = ldftn($Invoke0);
			*(data + 957736) = ldftn($Invoke1);
			*(data + 957764) = ldftn($Invoke2);
			*(data + 957792) = ldftn($Invoke3);
			*(data + 957820) = ldftn($Invoke4);
			*(data + 957848) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new AbstractSquadScreenViewModule((UIntPtr)0);
		}
	}
}

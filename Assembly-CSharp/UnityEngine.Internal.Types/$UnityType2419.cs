using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2419 : $UnityType
	{
		public unsafe $UnityType2419()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 951268) = ldftn($Invoke0);
			*(data + 951296) = ldftn($Invoke1);
			*(data + 951324) = ldftn($Invoke2);
			*(data + 951352) = ldftn($Invoke3);
			*(data + 951380) = ldftn($Invoke4);
			*(data + 951408) = ldftn($Invoke5);
			*(data + 951436) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new DevNotesHolonetTab((UIntPtr)0);
		}
	}
}

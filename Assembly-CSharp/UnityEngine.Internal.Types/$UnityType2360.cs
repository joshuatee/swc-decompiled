using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2360 : $UnityType
	{
		public unsafe $UnityType2360()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 933712) = ldftn($Invoke0);
			*(data + 933740) = ldftn($Invoke1);
			*(data + 933768) = ldftn($Invoke2);
			*(data + 933796) = ldftn($Invoke3);
			*(data + 933824) = ldftn($Invoke4);
			*(data + 933852) = ldftn($Invoke5);
			*(data + 933880) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new StarshipInfoScreen((UIntPtr)0);
		}
	}
}

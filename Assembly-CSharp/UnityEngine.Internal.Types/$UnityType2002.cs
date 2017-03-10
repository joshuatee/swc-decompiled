using StaRTS.Main.Story;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2002 : $UnityType
	{
		public unsafe $UnityType2002()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 836832) = ldftn($Invoke0);
			*(data + 836860) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PlanetIntroStoryUtil((UIntPtr)0);
		}
	}
}

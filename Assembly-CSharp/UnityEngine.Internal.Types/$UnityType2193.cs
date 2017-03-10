using StaRTS.Main.Views.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2193 : $UnityType
	{
		public unsafe $UnityType2193()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 869732) = ldftn($Invoke0);
			*(data + 869760) = ldftn($Invoke1);
			*(data + 869788) = ldftn($Invoke2);
			*(data + 869816) = ldftn($Invoke3);
			*(data + 869844) = ldftn($Invoke4);
			*(data + 869872) = ldftn($Invoke5);
			*(data + 869900) = ldftn($Invoke6);
			*(data + 869928) = ldftn($Invoke7);
			*(data + 869956) = ldftn($Invoke8);
			*(data + 869984) = ldftn($Invoke9);
			*(data + 870012) = ldftn($Invoke10);
			*(data + 870040) = ldftn($Invoke11);
			*(data + 870068) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new GalaxyManipulator((UIntPtr)0);
		}
	}
}

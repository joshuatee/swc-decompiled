using StaRTS.Main.Controllers.CombineMesh;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType796 : $UnityType
	{
		public unsafe $UnityType796()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 568956) = ldftn($Invoke0);
			*(data + 568984) = ldftn($Invoke1);
			*(data + 569012) = ldftn($Invoke2);
			*(data + 569040) = ldftn($Invoke3);
			*(data + 569068) = ldftn($Invoke4);
			*(data + 569096) = ldftn($Invoke5);
			*(data + 569124) = ldftn($Invoke6);
			*(data + 569152) = ldftn($Invoke7);
			*(data + 569180) = ldftn($Invoke8);
			*(data + 569208) = ldftn($Invoke9);
			*(data + 569236) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new CombineMeshManager((UIntPtr)0);
		}
	}
}

using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2194 : $UnityType
	{
		public unsafe $UnityType2194()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 870096) = ldftn($Invoke0);
			*(data + 870124) = ldftn($Invoke1);
			*(data + 870152) = ldftn($Invoke2);
			*(data + 870180) = ldftn($Invoke3);
			*(data + 870208) = ldftn($Invoke4);
			*(data + 870236) = ldftn($Invoke5);
			*(data + 870264) = ldftn($Invoke6);
			*(data + 870292) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new AbstractProjectorRenderer((UIntPtr)0);
		}
	}
}

using StaRTS.Main.Views.UX.Tags;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2481 : $UnityType
	{
		public unsafe $UnityType2481()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 974536) = ldftn($Invoke0);
			*(data + 974564) = ldftn($Invoke1);
			*(data + 974592) = ldftn($Invoke2);
			*(data + 974620) = ldftn($Invoke3);
			*(data + 974648) = ldftn($Invoke4);
			*(data + 974676) = ldftn($Invoke5);
			*(data + 974704) = ldftn($Invoke6);
			*(data + 974732) = ldftn($Invoke7);
			*(data + 974760) = ldftn($Invoke8);
			*(data + 974788) = ldftn($Invoke9);
			*(data + 974816) = ldftn($Invoke10);
			*(data + 974844) = ldftn($Invoke11);
			*(data + 974872) = ldftn($Invoke12);
			*(data + 974900) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new TroopTrainingTag((UIntPtr)0);
		}
	}
}

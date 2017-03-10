using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType79 : $UnityType
	{
		public unsafe $UnityType79()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 362092) = ldftn($Invoke0);
			*(data + 362120) = ldftn($Invoke1);
			*(data + 362148) = ldftn($Invoke2);
			*(data + 362176) = ldftn($Invoke3);
			*(data + 362204) = ldftn($Invoke4);
			*(data + 362232) = ldftn($Invoke5);
			*(data + 362260) = ldftn($Invoke6);
			*(data + 362288) = ldftn($Invoke7);
			*(data + 362316) = ldftn($Invoke8);
			*(data + 1524088) = ldftn($Get0);
			*(data + 1524092) = ldftn($Set0);
			*(data + 1524104) = ldftn($Get1);
			*(data + 1524108) = ldftn($Set1);
			*(data + 1524120) = ldftn($Get2);
			*(data + 1524124) = ldftn($Set2);
			*(data + 1524136) = ldftn($Get3);
			*(data + 1524140) = ldftn($Set3);
			*(data + 1524152) = ldftn($Get4);
			*(data + 1524156) = ldftn($Set4);
			*(data + 1524168) = ldftn($Get5);
			*(data + 1524172) = ldftn($Set5);
		}

		public override object CreateInstance()
		{
			return new SpringPosition((UIntPtr)0);
		}
	}
}

using StaRTS.Utils.Core;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2553 : $UnityType
	{
		public unsafe $UnityType2553()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 995088) = ldftn($Invoke0);
			*(data + 995116) = ldftn($Invoke1);
			*(data + 995144) = ldftn($Invoke2);
			*(data + 995172) = ldftn($Invoke3);
			*(data + 995200) = ldftn($Invoke4);
			*(data + 995228) = ldftn($Invoke5);
			*(data + 995256) = ldftn($Invoke6);
			*(data + 995284) = ldftn($Invoke7);
			*(data + 995312) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new MutableIterator((UIntPtr)0);
		}
	}
}

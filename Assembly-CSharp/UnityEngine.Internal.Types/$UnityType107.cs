using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType107 : $UnityType
	{
		public unsafe $UnityType107()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 372088) = ldftn($Invoke0);
			*(data + 372116) = ldftn($Invoke1);
			*(data + 372144) = ldftn($Invoke2);
			*(data + 372172) = ldftn($Invoke3);
			*(data + 372200) = ldftn($Invoke4);
			*(data + 372228) = ldftn($Invoke5);
			*(data + 372256) = ldftn($Invoke6);
			*(data + 372284) = ldftn($Invoke7);
			*(data + 1524472) = ldftn($Get0);
			*(data + 1524476) = ldftn($Set0);
			*(data + 1524488) = ldftn($Get1);
			*(data + 1524492) = ldftn($Set1);
			*(data + 1524504) = ldftn($Get2);
			*(data + 1524508) = ldftn($Set2);
		}

		public override object CreateInstance()
		{
			return new TweenTransform((UIntPtr)0);
		}
	}
}

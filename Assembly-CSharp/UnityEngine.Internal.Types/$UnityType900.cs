using StaRTS.Main.Controllers.ServerMessages;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType900 : $UnityType
	{
		public unsafe $UnityType900()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 589312) = ldftn($Invoke0);
			*(data + 589340) = ldftn($Invoke1);
			*(data + 589368) = ldftn($Invoke2);
			*(data + 589396) = ldftn($Invoke3);
			*(data + 589424) = ldftn($Invoke4);
			*(data + 589452) = ldftn($Invoke5);
			*(data + 589480) = ldftn($Invoke6);
			*(data + 589508) = ldftn($Invoke7);
			*(data + 589536) = ldftn($Invoke8);
			*(data + 589564) = ldftn($Invoke9);
			*(data + 589592) = ldftn($Invoke10);
			*(data + 589620) = ldftn($Invoke11);
			*(data + 589648) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new DeltaMessage((UIntPtr)0);
		}
	}
}

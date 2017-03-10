using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType126 : $UnityType
	{
		public unsafe $UnityType126()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 377828) = ldftn($Invoke0);
			*(data + 377856) = ldftn($Invoke1);
			*(data + 377884) = ldftn($Invoke2);
			*(data + 377912) = ldftn($Invoke3);
			*(data + 377940) = ldftn($Invoke4);
			*(data + 377968) = ldftn($Invoke5);
			*(data + 377996) = ldftn($Invoke6);
			*(data + 378024) = ldftn($Invoke7);
			*(data + 378052) = ldftn($Invoke8);
			*(data + 378080) = ldftn($Invoke9);
			*(data + 378108) = ldftn($Invoke10);
			*(data + 378136) = ldftn($Invoke11);
			*(data + 378164) = ldftn($Invoke12);
			*(data + 1525240) = ldftn($Get0);
			*(data + 1525244) = ldftn($Set0);
			*(data + 1525256) = ldftn($Get1);
			*(data + 1525260) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new UIButtonMessage((UIntPtr)0);
		}
	}
}

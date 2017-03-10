using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType149 : $UnityType
	{
		public unsafe $UnityType149()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 384380) = ldftn($Invoke0);
			*(data + 384408) = ldftn($Invoke1);
			*(data + 384436) = ldftn($Invoke2);
			*(data + 384464) = ldftn($Invoke3);
			*(data + 384492) = ldftn($Invoke4);
			*(data + 384520) = ldftn($Invoke5);
			*(data + 384548) = ldftn($Invoke6);
			*(data + 384576) = ldftn($Invoke7);
			*(data + 384604) = ldftn($Invoke8);
			*(data + 384632) = ldftn($Invoke9);
			*(data + 384660) = ldftn($Invoke10);
			*(data + 384688) = ldftn($Invoke11);
			*(data + 384716) = ldftn($Invoke12);
			*(data + 384744) = ldftn($Invoke13);
			*(data + 1526072) = ldftn($Get0);
			*(data + 1526076) = ldftn($Set0);
			*(data + 1526088) = ldftn($Get1);
			*(data + 1526092) = ldftn($Set1);
			*(data + 1526104) = ldftn($Get2);
			*(data + 1526108) = ldftn($Set2);
			*(data + 1526120) = ldftn($Get3);
			*(data + 1526124) = ldftn($Set3);
			*(data + 1526136) = ldftn($Get4);
			*(data + 1526140) = ldftn($Set4);
		}

		public override object CreateInstance()
		{
			return new UIDraggableCamera((UIntPtr)0);
		}
	}
}

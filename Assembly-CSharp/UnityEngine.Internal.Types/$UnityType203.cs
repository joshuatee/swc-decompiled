using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType203 : $UnityType
	{
		public unsafe $UnityType203()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 405940) = ldftn($Invoke0);
			*(data + 405968) = ldftn($Invoke1);
			*(data + 405996) = ldftn($Invoke2);
			*(data + 406024) = ldftn($Invoke3);
			*(data + 406052) = ldftn($Invoke4);
			*(data + 406080) = ldftn($Invoke5);
			*(data + 406108) = ldftn($Invoke6);
			*(data + 406136) = ldftn($Invoke7);
			*(data + 1528056) = ldftn($Get0);
			*(data + 1528060) = ldftn($Set0);
			*(data + 1528072) = ldftn($Get1);
			*(data + 1528076) = ldftn($Set1);
			*(data + 1528088) = ldftn($Get2);
			*(data + 1528092) = ldftn($Set2);
			*(data + 1528104) = ldftn($Get3);
			*(data + 1528108) = ldftn($Set3);
		}

		public override object CreateInstance()
		{
			return new UIShowControlScheme((UIntPtr)0);
		}
	}
}

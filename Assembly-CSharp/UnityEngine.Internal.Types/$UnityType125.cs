using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType125 : $UnityType
	{
		public unsafe $UnityType125()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 377632) = ldftn($Invoke0);
			*(data + 377660) = ldftn($Invoke1);
			*(data + 377688) = ldftn($Invoke2);
			*(data + 377716) = ldftn($Invoke3);
			*(data + 377744) = ldftn($Invoke4);
			*(data + 377772) = ldftn($Invoke5);
			*(data + 377800) = ldftn($Invoke6);
			*(data + 1525160) = ldftn($Get0);
			*(data + 1525164) = ldftn($Set0);
			*(data + 1525176) = ldftn($Get1);
			*(data + 1525180) = ldftn($Set1);
			*(data + 1525192) = ldftn($Get2);
			*(data + 1525196) = ldftn($Set2);
			*(data + 1525208) = ldftn($Get3);
			*(data + 1525212) = ldftn($Set3);
			*(data + 1525224) = ldftn($Get4);
			*(data + 1525228) = ldftn($Set4);
		}

		public override object CreateInstance()
		{
			return new UIButtonKeys((UIntPtr)0);
		}
	}
}

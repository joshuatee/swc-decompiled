using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType839 : $UnityType
	{
		public unsafe $UnityType839()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 574668) = ldftn($Invoke0);
			*(data + 574696) = ldftn($Invoke1);
			*(data + 574724) = ldftn($Invoke2);
			*(data + 574752) = ldftn($Invoke3);
			*(data + 574780) = ldftn($Invoke4);
			*(data + 574808) = ldftn($Invoke5);
			*(data + 574836) = ldftn($Invoke6);
			*(data + 574864) = ldftn($Invoke7);
			*(data + 574892) = ldftn($Invoke8);
			*(data + 574920) = ldftn($Invoke9);
			*(data + 574948) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new WarBaseEditorState((UIntPtr)0);
		}
	}
}

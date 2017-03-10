using StaRTS.Main.Models.Commands.Tournament;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1677 : $UnityType
	{
		public unsafe $UnityType1677()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 665836) = ldftn($Invoke0);
			*(data + 665864) = ldftn($Invoke1);
			*(data + 665892) = ldftn($Invoke2);
			*(data + 665920) = ldftn($Invoke3);
			*(data + 665948) = ldftn($Invoke4);
			*(data + 665976) = ldftn($Invoke5);
			*(data + 666004) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new TournamentResponse((UIntPtr)0);
		}
	}
}

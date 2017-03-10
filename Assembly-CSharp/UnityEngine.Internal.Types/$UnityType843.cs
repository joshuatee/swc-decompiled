using StaRTS.Main.Controllers.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType843 : $UnityType
	{
		public unsafe $UnityType843()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 575256) = ldftn($Invoke0);
			*(data + 575284) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DevNotesHolonetController((UIntPtr)0);
		}
	}
}

using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2009 : $UnityType
	{
		public unsafe $UnityType2009()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837364) = ldftn($Invoke0);
			*(data + 837392) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new AllowDeployStoryAction((UIntPtr)0);
		}
	}
}

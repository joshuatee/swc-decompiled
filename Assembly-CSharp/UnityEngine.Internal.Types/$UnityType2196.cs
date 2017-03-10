using StaRTS.Main.Views.Projectors;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2196 : $UnityType
	{
		public unsafe $UnityType2196()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 870404) = ldftn($Invoke0);
			*(data + 870432) = ldftn($Invoke1);
			*(data + 870460) = ldftn($Invoke2);
			*(data + 870488) = ldftn($Invoke3);
			*(data + 870516) = ldftn($Invoke4);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IProjectorRenderer)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IProjectorRenderer)GCHandledObjects.GCHandleToObject(instance)).DoesRenderTextureNeedReload());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IProjectorRenderer)GCHandledObjects.GCHandleToObject(instance)).GetProjectorUITexture());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((IProjectorRenderer)GCHandledObjects.GCHandleToObject(instance)).PostRender((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IProjectorRenderer)GCHandledObjects.GCHandleToObject(instance)).Render((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

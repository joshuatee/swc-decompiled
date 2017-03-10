using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.Projectors
{
	public class SurfaceProjectorRenderer : AbstractProjectorRenderer, IProjectorRenderer
	{
		public SurfaceProjectorRenderer(int projectorIndex) : base(projectorIndex)
		{
		}

		public override void Render(ProjectorConfig config)
		{
			if (!config.AssetReady)
			{
				return;
			}
			if (config.RenderCallback == null)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Neither a sprite nor a RenderCallback was not provided for the projector {0}", new object[]
				{
					config.AssetName
				});
			}
			base.SetupCamera(config.AssetName, config.MainAsset, config.Sharpness, config.RenderWidth, config.RenderHeight, config.CameraPosition, config.CameraInterest);
			base.Render(config);
		}

		public override void PostRender(ProjectorConfig config)
		{
			base.PostRender(config);
		}

		public UITexture GetProjectorUITexture()
		{
			return null;
		}

		protected internal SurfaceProjectorRenderer(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SurfaceProjectorRenderer)GCHandledObjects.GCHandleToObject(instance)).GetProjectorUITexture());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SurfaceProjectorRenderer)GCHandledObjects.GCHandleToObject(instance)).PostRender((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SurfaceProjectorRenderer)GCHandledObjects.GCHandleToObject(instance)).Render((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

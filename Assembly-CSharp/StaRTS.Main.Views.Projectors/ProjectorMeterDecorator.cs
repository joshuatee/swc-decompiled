using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Views.Entities;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.Projectors
{
	public class ProjectorMeterDecorator : IProjectorRenderer
	{
		private IProjectorRenderer baseRenderer;

		private MeterShaderComponent shaderMeterComp;

		public ProjectorMeterDecorator(IProjectorRenderer baseRenderer)
		{
			this.baseRenderer = baseRenderer;
		}

		public void Render(ProjectorConfig config)
		{
			if (!config.AssetReady)
			{
				return;
			}
			this.shaderMeterComp = Service.Get<EntityViewManager>().CreateMeterShaderComponentIfApplicable(config.MainAsset);
			this.shaderMeterComp.UpdatePercentage(config.MeterValue);
			this.baseRenderer.Render(config);
		}

		public void PostRender(ProjectorConfig config)
		{
			this.baseRenderer.PostRender(config);
		}

		public void Destroy()
		{
			this.baseRenderer.Destroy();
			this.baseRenderer = null;
			this.shaderMeterComp = null;
		}

		public bool DoesRenderTextureNeedReload()
		{
			return this.baseRenderer.DoesRenderTextureNeedReload();
		}

		public UITexture GetProjectorUITexture()
		{
			return this.baseRenderer.GetProjectorUITexture();
		}

		protected internal ProjectorMeterDecorator(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ProjectorMeterDecorator)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectorMeterDecorator)GCHandledObjects.GCHandleToObject(instance)).DoesRenderTextureNeedReload());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectorMeterDecorator)GCHandledObjects.GCHandleToObject(instance)).GetProjectorUITexture());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ProjectorMeterDecorator)GCHandledObjects.GCHandleToObject(instance)).PostRender((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ProjectorMeterDecorator)GCHandledObjects.GCHandleToObject(instance)).Render((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

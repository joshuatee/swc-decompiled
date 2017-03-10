using StaRTS.Utils;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Projectors
{
	public class ProjectorTurretDecorator : IProjectorRenderer
	{
		private static readonly Quaternion DEFAULT_ROTATION = Quaternion.Euler(0f, -90f, 0f);

		private IProjectorRenderer baseRenderer;

		public ProjectorTurretDecorator(IProjectorRenderer baseRenderer)
		{
			this.baseRenderer = baseRenderer;
		}

		public void Render(ProjectorConfig config)
		{
			if (!config.AssetReady)
			{
				return;
			}
			GameObject gameObject = UnityUtils.FindGameObject(config.MainAsset, config.TrackerName);
			if (gameObject != null)
			{
				gameObject.transform.localRotation = ProjectorTurretDecorator.DEFAULT_ROTATION;
			}
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
		}

		public bool DoesRenderTextureNeedReload()
		{
			return this.baseRenderer.DoesRenderTextureNeedReload();
		}

		public UITexture GetProjectorUITexture()
		{
			return this.baseRenderer.GetProjectorUITexture();
		}

		protected internal ProjectorTurretDecorator(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ProjectorTurretDecorator)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectorTurretDecorator)GCHandledObjects.GCHandleToObject(instance)).DoesRenderTextureNeedReload());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectorTurretDecorator)GCHandledObjects.GCHandleToObject(instance)).GetProjectorUITexture());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ProjectorTurretDecorator)GCHandledObjects.GCHandleToObject(instance)).PostRender((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ProjectorTurretDecorator)GCHandledObjects.GCHandleToObject(instance)).Render((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

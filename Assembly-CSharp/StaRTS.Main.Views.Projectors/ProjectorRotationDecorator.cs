using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Projectors
{
	public class ProjectorRotationDecorator : IProjectorRenderer, IViewFrameTimeObserver
	{
		private IProjectorRenderer baseRenderer;

		private GameObject targetObject;

		private float iconRotationSpeed;

		public ProjectorRotationDecorator(IProjectorRenderer baseRenderer)
		{
			this.baseRenderer = baseRenderer;
		}

		public void Render(ProjectorConfig config)
		{
			if (!config.AssetReady)
			{
				return;
			}
			this.targetObject = config.MainAsset.gameObject;
			this.iconRotationSpeed = config.IconRotationSpeed;
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
			this.baseRenderer.Render(config);
		}

		public void PostRender(ProjectorConfig config)
		{
			this.baseRenderer.PostRender(config);
		}

		public void OnViewFrameTime(float dt)
		{
			this.targetObject.transform.Rotate(Vector3.up, this.iconRotationSpeed * dt);
		}

		public void Destroy()
		{
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
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

		protected internal ProjectorRotationDecorator(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ProjectorRotationDecorator)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectorRotationDecorator)GCHandledObjects.GCHandleToObject(instance)).DoesRenderTextureNeedReload());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectorRotationDecorator)GCHandledObjects.GCHandleToObject(instance)).GetProjectorUITexture());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ProjectorRotationDecorator)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ProjectorRotationDecorator)GCHandledObjects.GCHandleToObject(instance)).PostRender((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ProjectorRotationDecorator)GCHandledObjects.GCHandleToObject(instance)).Render((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.Projectors
{
	public class GeometryProjector : IEventObserver
	{
		public IProjectorRenderer Renderer;

		public int ProjectorIndex;

		public ProjectorConfig Config;

		public ProjectorAssetProcessor AssetProcessor;

		public ProjectorForceReloadHelper ReloadHelper;

		public GeometryProjector(ProjectorConfig config)
		{
			this.Config = config;
			this.ProjectorIndex = Service.Get<ProjectorManager>().AddProjector(this);
			this.AssetProcessor = new ProjectorAssetProcessor(this);
			Service.Get<EventManager>().RegisterObserver(this, EventId.ForceGeometryReload, EventPriority.Default);
		}

		public void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ForceGeometryReload);
			Service.Get<ProjectorManager>().RemoveProjector(this);
			if (this.AssetProcessor != null)
			{
				this.AssetProcessor.UnloadAllAssets(null);
				this.AssetProcessor = null;
			}
			if (this.Renderer != null)
			{
				this.Renderer.Destroy();
				this.Renderer = null;
			}
			if (this.Config != null)
			{
				this.Config.Destroy();
				this.Config = null;
			}
			if (this.ReloadHelper != null)
			{
				this.ReloadHelper.Destroy();
				this.ReloadHelper = null;
			}
		}

		private void ReCreate()
		{
			ProjectorUtils.ResetProjectorMembers(this);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.ForceGeometryReload)
			{
				return EatResponse.NotEaten;
			}
			this.ReCreate();
			return EatResponse.NotEaten;
		}

		protected internal GeometryProjector(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((GeometryProjector)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeometryProjector)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GeometryProjector)GCHandledObjects.GCHandleToObject(instance)).ReCreate();
			return -1L;
		}
	}
}

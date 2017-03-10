using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.Projectors
{
	public class ProjectorForceReloadHelper : IEventObserver
	{
		private GeometryProjector Projector;

		public ProjectorForceReloadHelper(GeometryProjector projector)
		{
			this.Projector = projector;
			Service.Get<EventManager>().RegisterObserver(this, EventId.ForceGeometryReload, EventPriority.Default);
		}

		public void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ForceGeometryReload);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.ForceGeometryReload)
			{
				return EatResponse.NotEaten;
			}
			if (this.Projector.Renderer != null)
			{
				bool flag = this.Projector.Renderer.DoesRenderTextureNeedReload();
				if (flag)
				{
					this.Projector.Renderer.Render(this.Projector.Config);
				}
			}
			return EatResponse.NotEaten;
		}

		protected internal ProjectorForceReloadHelper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ProjectorForceReloadHelper)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectorForceReloadHelper)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}

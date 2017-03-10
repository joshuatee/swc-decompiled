using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Main.Models.Entities.Shared;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.Systems
{
	public class DroidSystem : ViewSystemBase
	{
		private const float FULL_CYCLE_ANIMATION = 3.033f;

		private NodeList<DroidNode> nodeList;

		private DroidController droidController;

		public override void AddToGame(IGame game)
		{
			this.nodeList = Service.Get<EntityController>().GetNodeList<DroidNode>();
			this.droidController = Service.Get<DroidController>();
		}

		public override void RemoveFromGame(IGame game)
		{
		}

		protected override void Update(float dt)
		{
			Entity droidHut = this.droidController.GetDroidHut();
			DroidNode next;
			for (DroidNode droidNode = this.nodeList.Head; droidNode != null; droidNode = next)
			{
				next = droidNode.Next;
				if (droidNode.State.Dirty && droidNode.State.CurState == EntityState.Moving)
				{
					this.droidController.AssignDroidPath(droidNode);
				}
				if (droidNode.State.CurState == EntityState.Idle)
				{
					if (droidNode.Droid.Target != null)
					{
						droidNode.State.CurState = EntityState.Moving;
						this.droidController.AssignDroidPath(droidNode);
					}
				}
				else if (droidNode.State.CurState == EntityState.Moving)
				{
					if (this.droidController.UpdateDroidTransform(droidNode, dt))
					{
						if (droidNode.Droid.Target == droidHut)
						{
							droidNode.State.CurState = EntityState.CoolingDown;
							this.droidController.DestroyDroid(droidNode);
						}
						else
						{
							droidNode.State.CurState = EntityState.Attacking;
							droidNode.Droid.Delay = (float)Service.Get<Rand>().ViewRangeInt(1, 4) * 3.033f;
						}
					}
				}
				else if (droidNode.State.CurState == EntityState.CoolingDown || droidNode.State.CurState == EntityState.Attacking)
				{
					droidNode.Droid.Delay -= dt;
					if (droidNode.Droid.Delay <= 0f)
					{
						droidNode.State.CurState = EntityState.Moving;
						this.droidController.AssignDroidPath(droidNode);
					}
				}
			}
		}

		public DroidSystem()
		{
		}

		protected internal DroidSystem(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DroidSystem)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DroidSystem)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DroidSystem)GCHandledObjects.GCHandleToObject(instance)).Update(*(float*)args);
			return -1L;
		}
	}
}

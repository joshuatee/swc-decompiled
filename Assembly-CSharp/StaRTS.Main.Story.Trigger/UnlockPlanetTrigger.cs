using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class UnlockPlanetTrigger : AbstractStoryTrigger, IEventObserver
	{
		public UnlockPlanetTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ContractCompletedForStoryAction)
			{
				ContractTO contractTO = (ContractTO)cookie;
				if (contractTO.ContractType == ContractType.Upgrade)
				{
					BuildingTypeVO buildingTypeVO = Service.Get<IDataController>().Get<BuildingTypeVO>(contractTO.Uid);
					if (buildingTypeVO.Type == BuildingType.NavigationCenter)
					{
						this.CheckSatisfyTrigger(contractTO.Tag);
					}
				}
			}
			else if (id == EventId.PlanetUnlocked)
			{
				this.CheckSatisfyTrigger((string)cookie);
			}
			return EatResponse.NotEaten;
		}

		private void CheckSatisfyTrigger(string uid)
		{
			if (string.IsNullOrEmpty(this.vo.PrepareString) || this.vo.PrepareString.Equals(uid))
			{
				this.UnregisterObservers();
				this.parent.SatisfyTrigger(this);
			}
		}

		public override void Activate()
		{
			base.Activate();
			Service.Get<EventManager>().RegisterObserver(this, EventId.ContractCompletedForStoryAction, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.PlanetUnlocked, EventPriority.Default);
		}

		private void UnregisterObservers()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ContractCompletedForStoryAction);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.PlanetUnlocked);
		}

		protected internal UnlockPlanetTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((UnlockPlanetTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UnlockPlanetTrigger)GCHandledObjects.GCHandleToObject(instance)).CheckSatisfyTrigger(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UnlockPlanetTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((UnlockPlanetTrigger)GCHandledObjects.GCHandleToObject(instance)).UnregisterObservers();
			return -1L;
		}
	}
}

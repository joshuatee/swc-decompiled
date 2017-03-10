using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class PlanetRelocateStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		private const string FIRST_RELOCATION = "FIRST_RELOC";

		private const string SERVER_PREF_DEFAULT = "0";

		private const int FIRST_RELOC_ARG = 0;

		private const int PLANET_UID_ARG = 0;

		private const int SAVE_VAR_ARG = 1;

		private bool isFirstRelocation;

		private string saveVarName;

		private PlanetVO planetVO;

		public PlanetRelocateStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			if (this.prepareArgs[0].Equals("FIRST_RELOC"))
			{
				this.isFirstRelocation = true;
				if (this.prepareArgs.Length > 1)
				{
					this.saveVarName = this.prepareArgs[1];
				}
			}
			else
			{
				string text = this.prepareArgs[0];
				this.planetVO = Service.Get<IDataController>().GetOptional<PlanetVO>(text);
				if (this.planetVO == null)
				{
					Service.Get<StaRTSLogger>().Error("PlanetRelocateStoryTrigger: invalid planet uid: " + text);
					return;
				}
			}
			base.Activate();
			Service.Get<EventManager>().RegisterObserver(this, EventId.PlanetRelocate, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.PlanetRelocate)
			{
				string text = (string)cookie;
				if (this.isFirstRelocation)
				{
					if (!string.IsNullOrEmpty(this.saveVarName))
					{
						AutoStoryTriggerUtils.SaveTriggerValue(this.saveVarName, text);
					}
					this.parent.SatisfyTrigger(this);
				}
				else if (text.Equals(this.planetVO.Uid))
				{
					this.parent.SatisfyTrigger(this);
				}
			}
			return EatResponse.NotEaten;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.PlanetRelocate);
			base.Destroy();
		}

		protected internal PlanetRelocateStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlanetRelocateStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlanetRelocateStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetRelocateStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}

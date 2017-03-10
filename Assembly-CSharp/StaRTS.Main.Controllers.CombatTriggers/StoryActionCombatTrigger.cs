using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Story;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.CombatTriggers
{
	public class StoryActionCombatTrigger : ICombatTrigger
	{
		private const uint EFFECTIVELY_NEVER = 1800000u;

		private string storyAction;

		private bool triggered;

		public object Owner
		{
			get;
			private set;
		}

		public CombatTriggerType Type
		{
			get;
			private set;
		}

		public uint LastDitchDelayMillis
		{
			get;
			private set;
		}

		public StoryActionCombatTrigger(object owner, CombatTriggerType triggerType, string storyAction)
		{
			this.Owner = owner;
			this.Type = triggerType;
			this.LastDitchDelayMillis = 1800000u;
			this.storyAction = storyAction;
		}

		public void Trigger(Entity intruder)
		{
			if (this.Type == CombatTriggerType.Area && intruder != null && intruder.Get<TeamComponent>().TeamType != TeamType.Attacker)
			{
				return;
			}
			this.triggered = true;
			new ActionChain(this.storyAction);
		}

		public bool IsAlreadyTriggered()
		{
			return this.triggered;
		}

		protected internal StoryActionCombatTrigger(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryActionCombatTrigger)GCHandledObjects.GCHandleToObject(instance)).Owner);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryActionCombatTrigger)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryActionCombatTrigger)GCHandledObjects.GCHandleToObject(instance)).IsAlreadyTriggered());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((StoryActionCombatTrigger)GCHandledObjects.GCHandleToObject(instance)).Owner = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((StoryActionCombatTrigger)GCHandledObjects.GCHandleToObject(instance)).Type = (CombatTriggerType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((StoryActionCombatTrigger)GCHandledObjects.GCHandleToObject(instance)).Trigger((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

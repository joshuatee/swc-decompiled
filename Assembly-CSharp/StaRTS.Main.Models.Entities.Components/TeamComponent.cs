using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Battle;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TeamComponent : ComponentBase
	{
		public TeamType TeamType
		{
			get;
			protected set;
		}

		public TeamComponent(TeamType teamType)
		{
			this.TeamType = teamType;
		}

		public bool CanDestructBuildings()
		{
			return this.TeamType == TeamType.Attacker;
		}

		public bool IsDefender()
		{
			return this.TeamType == TeamType.Defender;
		}

		protected internal TeamComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TeamComponent)GCHandledObjects.GCHandleToObject(instance)).CanDestructBuildings());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TeamComponent)GCHandledObjects.GCHandleToObject(instance)).TeamType);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TeamComponent)GCHandledObjects.GCHandleToObject(instance)).IsDefender());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TeamComponent)GCHandledObjects.GCHandleToObject(instance)).TeamType = (TeamType)(*(int*)args);
			return -1L;
		}
	}
}

using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Missions
{
	public class MissionProcessorFactory
	{
		public static AbstractMissionProcessor CreateProcessor(MissionConductor mission, CampaignMissionVO vo)
		{
			switch (vo.MissionType)
			{
			case MissionType.Attack:
				return new OffensiveCombatMissionProcessor(mission);
			case MissionType.Defend:
			case MissionType.RaidDefend:
				return new DefensiveCombatMissionProcessor(mission);
			case MissionType.Own:
				return new OwnMissionProcessor(mission);
			case MissionType.Event:
				return new MultiCombatEventMissionProcessor(mission);
			case MissionType.Pvp:
				return new PvpStarsMissionProcessor(mission);
			}
			return new AbstractMissionProcessor(mission);
		}

		public MissionProcessorFactory()
		{
		}

		protected internal MissionProcessorFactory(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(MissionProcessorFactory.CreateProcessor((MissionConductor)GCHandledObjects.GCHandleToObject(*args), (CampaignMissionVO)GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}

using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class ChampionPlatformComponent : ComponentBase
	{
		public SmartEntity DefensiveChampion
		{
			get;
			set;
		}

		public ChampionPlatformComponent()
		{
		}

		protected internal ChampionPlatformComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionPlatformComponent)GCHandledObjects.GCHandleToObject(instance)).DefensiveChampion);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ChampionPlatformComponent)GCHandledObjects.GCHandleToObject(instance)).DefensiveChampion = (SmartEntity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

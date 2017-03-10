using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class ChampionComponent : ComponentBase
	{
		public TroopTypeVO ChampionType
		{
			get;
			private set;
		}

		public ChampionComponent(TroopTypeVO championType)
		{
			this.ChampionType = championType;
		}

		protected internal ChampionComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionComponent)GCHandledObjects.GCHandleToObject(instance)).ChampionType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ChampionComponent)GCHandledObjects.GCHandleToObject(instance)).ChampionType = (TroopTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

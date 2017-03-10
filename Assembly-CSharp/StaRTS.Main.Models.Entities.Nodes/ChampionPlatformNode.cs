using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class ChampionPlatformNode : Node<ChampionPlatformNode>
	{
		public ChampionPlatformComponent ChampionPlatformComp
		{
			get;
			set;
		}

		public BuildingComponent BuildingComp
		{
			get;
			set;
		}

		public ChampionPlatformNode()
		{
		}

		protected internal ChampionPlatformNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionPlatformNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionPlatformNode)GCHandledObjects.GCHandleToObject(instance)).ChampionPlatformComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ChampionPlatformNode)GCHandledObjects.GCHandleToObject(instance)).BuildingComp = (BuildingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ChampionPlatformNode)GCHandledObjects.GCHandleToObject(instance)).ChampionPlatformComp = (ChampionPlatformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class ChampionNode : Node<ChampionNode>
	{
		public ChampionComponent ChampionComp
		{
			get;
			set;
		}

		public ChampionNode()
		{
		}

		protected internal ChampionNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionNode)GCHandledObjects.GCHandleToObject(instance)).ChampionComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ChampionNode)GCHandledObjects.GCHandleToObject(instance)).ChampionComp = (ChampionComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

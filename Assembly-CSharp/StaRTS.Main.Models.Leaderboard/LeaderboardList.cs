using StaRTS.Main.Models.Squads;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Leaderboard
{
	public class LeaderboardList<T>
	{
		public List<T> List
		{
			get;
			private set;
		}

		public bool AlwaysRefresh
		{
			get;
			set;
		}

		public uint LastRefreshTime
		{
			get;
			set;
		}

		public LeaderboardList()
		{
			this.List = new List<T>();
			this.AlwaysRefresh = false;
			this.LastRefreshTime = 0u;
		}

		protected internal LeaderboardList(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardList<PlayerLBEntity>)GCHandledObjects.GCHandleToObject(instance)).AlwaysRefresh);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((LeaderboardList<PlayerLBEntity>)GCHandledObjects.GCHandleToObject(instance)).AlwaysRefresh = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LeaderboardList<Squad>)GCHandledObjects.GCHandleToObject(instance)).AlwaysRefresh);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LeaderboardList<Squad>)GCHandledObjects.GCHandleToObject(instance)).AlwaysRefresh = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}

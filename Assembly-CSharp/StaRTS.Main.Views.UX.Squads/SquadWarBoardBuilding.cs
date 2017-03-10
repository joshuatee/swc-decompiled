using StaRTS.Main.Models.Squads.War;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Squads
{
	public class SquadWarBoardBuilding
	{
		public GameObject Building;

		public SquadWarBoardPlayerInfo PlayerInfo;

		public bool IsEmpire;

		public SquadWarBoardBuilding(SquadWarParticipantState participantState, GameObject building, bool isEmpire)
		{
			this.Building = building;
			this.IsEmpire = isEmpire;
			this.PlayerInfo = new SquadWarBoardPlayerInfo(participantState, building.transform);
		}

		public void Destroy()
		{
			UnityEngine.Object.Destroy(this.Building);
			this.PlayerInfo.Destroy();
		}

		public void ToggleVisibility(bool flag)
		{
			this.Building.SetActive(flag);
			this.PlayerInfo.ToggleDisplay(flag);
		}

		protected internal SquadWarBoardBuilding(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadWarBoardBuilding)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadWarBoardBuilding)GCHandledObjects.GCHandleToObject(instance)).ToggleVisibility(*(sbyte*)args != 0);
			return -1L;
		}
	}
}

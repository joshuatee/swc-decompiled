using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.FX;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StaRTS.Main.Controllers
{
	public class SquadTroopAttackController : AbstractDeployableController
	{
		private List<TroopTypeVO> spawnQueue;

		private IntPosition spawnPosition;

		private uint currentTimerId;

		private SimTimerManager simTimerManager;

		private int currentOffsetIndex;

		public bool Spawning
		{
			get;
			private set;
		}

		public SquadTroopAttackController()
		{
			Service.Set<SquadTroopAttackController>(this);
			this.simTimerManager = Service.Get<SimTimerManager>();
			this.Reset();
		}

		public void Reset()
		{
			if (this.currentTimerId != 0u)
			{
				this.simTimerManager.KillSimTimer(this.currentTimerId);
			}
			this.currentTimerId = 0u;
			this.Spawning = false;
			this.spawnQueue = null;
		}

		public void DeploySquadTroops(IntPosition boardPos)
		{
			this.Reset();
			this.spawnPosition = boardPos;
			IDataController dataController = Service.Get<IDataController>();
			this.spawnQueue = new List<TroopTypeVO>();
			List<SquadDonatedTroop> troops = Service.Get<SquadController>().StateManager.Troops;
			if (troops != null)
			{
				int i = 0;
				int count = troops.Count;
				while (i < count)
				{
					TroopTypeVO item = dataController.Get<TroopTypeVO>(troops[i].TroopUid);
					int j = 0;
					int totalAmount = troops[i].GetTotalAmount();
					while (j < totalAmount)
					{
						this.spawnQueue.Add(item);
						j++;
					}
					i++;
				}
			}
			this.OnDeploy();
		}

		public void DeploySquadTroops(IntPosition boardPos, Dictionary<string, int> squadTroops)
		{
			this.Reset();
			this.spawnPosition = boardPos;
			IDataController dataController = Service.Get<IDataController>();
			this.spawnQueue = new List<TroopTypeVO>();
			foreach (KeyValuePair<string, int> current in squadTroops)
			{
				TroopTypeVO item = dataController.Get<TroopTypeVO>(current.Key);
				for (int i = 0; i < current.Value; i++)
				{
					this.spawnQueue.Add(item);
				}
			}
			this.OnDeploy();
		}

		private void OnDeploy()
		{
			if (this.spawnQueue.Count > 0)
			{
				this.spawnQueue.Sort(new Comparison<TroopTypeVO>(this.CompareTroops));
				this.currentOffsetIndex = 0;
				TroopTypeVO troopTypeVO = this.spawnQueue[0];
				if (Service.Get<TroopController>().ValidateAttackerTroopPlacement(this.spawnPosition, troopTypeVO.SizeX, true))
				{
					base.EnsureBattlePlayState();
					this.Spawning = true;
					this.PlaceSquadFlag(troopTypeVO.Faction);
					Service.Get<EventManager>().SendEvent(EventId.SquadTroopsDeployedByPlayer, this.spawnPosition);
					Service.Get<BattleController>().SendSquadDeployedCommand(this.spawnPosition.x, this.spawnPosition.z);
				}
			}
		}

		public void UpdateSquadTroopSpawnQueue(TeamType type)
		{
			if (this.Spawning && this.spawnQueue.Count > 0 && this.currentTimerId == 0u)
			{
				this.CreateSpawnTimer(type);
			}
		}

		private void CreateSpawnTimer(TeamType type)
		{
			this.currentTimerId = this.simTimerManager.CreateSimTimer(GameConstants.SQUAD_TROOP_DEPLOY_STAGGER, false, new TimerDelegate(this.OnSpawnTimer), type);
		}

		private void OnSpawnTimer(uint id, object cookie)
		{
			TroopTypeVO troopVO = this.spawnQueue[0];
			TeamType teamType = (TeamType)((int)cookie);
			Entity entity = Service.Get<TroopController>().DeployTroopWithOffset(troopVO, ref this.currentOffsetIndex, this.spawnPosition, false, teamType);
			if (entity != null)
			{
				this.spawnQueue.RemoveAt(0);
				if (this.spawnQueue.Count > 0)
				{
					this.CreateSpawnTimer(teamType);
				}
				else
				{
					this.Spawning = false;
				}
			}
		}

		private int CompareTroops(TroopTypeVO a, TroopTypeVO b)
		{
			if (a == b)
			{
				return 0;
			}
			return b.SizeX.CompareTo(a.SizeX);
		}

		private void PlaceSquadFlag(FactionType faction)
		{
			string assetName;
			if (faction == FactionType.Empire)
			{
				assetName = GameConstants.SQUAD_TROOP_DEPLOY_FLAG_EMPIRE_ASSET;
			}
			else
			{
				assetName = GameConstants.SQUAD_TROOP_DEPLOY_FLAG_REBEL_ASSET;
			}
			IntPosition position = new IntPosition(this.spawnPosition.x - 1, this.spawnPosition.z);
			Vector3 worldPos = Units.BoardToWorldVec(position);
			Service.Get<FXManager>().CreateFXAtPosition(assetName, worldPos, Quaternion.AngleAxis(-90f, Vector3.up));
		}
	}
}

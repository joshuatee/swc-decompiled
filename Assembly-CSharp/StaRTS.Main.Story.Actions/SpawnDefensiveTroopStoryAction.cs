using StaRTS.DataStructures;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.World;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class SpawnDefensiveTroopStoryAction : AbstractStoryAction
	{
		private const int ARG_TROOP_UID = 0;

		private const int ARG_X = 1;

		private const int ARG_Z = 2;

		private const int ARG_NUM = 3;

		private const int TIMER_DELAY = 1500;

		private string troopUid;

		private int gridX;

		private int gridZ;

		private int amount;

		public SpawnDefensiveTroopStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(4);
			this.troopUid = this.prepareArgs[0];
			this.gridX = Convert.ToInt32(this.prepareArgs[1], CultureInfo.InvariantCulture);
			this.gridZ = Convert.ToInt32(this.prepareArgs[2], CultureInfo.InvariantCulture);
			this.amount = Convert.ToInt32(this.prepareArgs[3], CultureInfo.InvariantCulture);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			IntPosition position = new IntPosition(Units.GridToBoardX(this.gridX), Units.GridToBoardZ(this.gridZ));
			IDataController dataController = Service.Get<IDataController>();
			List<IAssetVO> assets = new List<IAssetVO>();
			ProjectileUtils.AddTroopProjectileAssets(this.troopUid, assets, dataController);
			Service.Get<ProjectileViewManager>().LoadProjectileAssetsAndCreatePools(assets);
			TroopTypeVO troop = dataController.Get<TroopTypeVO>(this.troopUid);
			TroopSpawnData troopSpawnData = new TroopSpawnData(troop, position, TroopSpawnMode.LeashedToSpawnPoint, this.amount);
			Service.Get<SimTimerManager>().CreateSimTimer(1500u, false, new TimerDelegate(this.OnSpawnTimer), troopSpawnData);
			this.parent.ChildComplete(this);
		}

		private void OnSpawnTimer(uint id, object cookie)
		{
			TroopSpawnData troopSpawnData = (TroopSpawnData)cookie;
			if (Service.Get<TroopController>().SpawnTroop(troopSpawnData.TroopType, TeamType.Defender, troopSpawnData.BoardPosition, troopSpawnData.SpawnMode, true) == null)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Can't spawn defensive troop {2} at position {0}, {1}! ", new object[]
				{
					this.gridX,
					this.gridZ,
					this.troopUid
				});
				return;
			}
			int num = troopSpawnData.Amount - 1;
			troopSpawnData.Amount = num;
			if (num > 0)
			{
				Service.Get<SimTimerManager>().CreateSimTimer(1500u, false, new TimerDelegate(this.OnSpawnTimer), troopSpawnData);
			}
		}

		protected internal SpawnDefensiveTroopStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SpawnDefensiveTroopStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SpawnDefensiveTroopStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}

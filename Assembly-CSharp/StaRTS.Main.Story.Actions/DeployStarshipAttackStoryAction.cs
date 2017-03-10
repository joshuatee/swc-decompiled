using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.World;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class DeployStarshipAttackStoryAction : AbstractStoryAction
	{
		private const int SHIP_UID_ARG = 0;

		private const int BOARDX_ARG = 1;

		private const int BOARDZ_ARG = 2;

		private int boardX;

		private int boardZ;

		public DeployStarshipAttackStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(3);
			this.boardX = Convert.ToInt32(this.prepareArgs[1], CultureInfo.InvariantCulture);
			this.boardZ = Convert.ToInt32(this.prepareArgs[2], CultureInfo.InvariantCulture);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Vector3 zero = Vector3.zero;
			zero.x = Units.BoardToWorldX(this.boardX);
			zero.z = Units.BoardToWorldX(this.boardZ);
			IDataController dataController = Service.Get<IDataController>();
			SpecialAttackTypeVO specialAttackTypeVO = dataController.Get<SpecialAttackTypeVO>(this.prepareArgs[0]);
			SpecialAttack specialAttack = Service.Get<SpecialAttackController>().DeploySpecialAttack(specialAttackTypeVO, TeamType.Attacker, zero);
			if (specialAttack != null)
			{
				List<IAssetVO> assets = new List<IAssetVO>();
				ProjectileUtils.AddProjectileAssets(specialAttackTypeVO.ProjectileType, assets, dataController);
				Service.Get<ProjectileViewManager>().LoadProjectileAssetsAndCreatePools(assets);
				Service.Get<EventManager>().SendEvent(EventId.SpecialAttackDeployed, specialAttack);
			}
			this.parent.ChildComplete(this);
		}

		protected internal DeployStarshipAttackStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeployStarshipAttackStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DeployStarshipAttackStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}

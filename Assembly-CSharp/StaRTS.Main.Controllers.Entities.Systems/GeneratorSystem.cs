using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.Systems
{
	public class GeneratorSystem : ViewSystemBase
	{
		private EntityController entityController;

		private ICurrencyController currencyController;

		private PostBattleRepairController postBattleRepairController;

		private NodeList<GeneratorViewNode> nodeList;

		private StorageEffects storageEffects;

		private const float VIEW_UPDATE_TIME = 1f;

		private float timeSinceViewUpdate;

		private const float FULLNESS_UPDATE_TIME = 5f;

		private float timeSinceFullnessUpdate;

		public override void AddToGame(IGame game)
		{
			this.entityController = Service.Get<EntityController>();
			this.currencyController = Service.Get<ICurrencyController>();
			this.postBattleRepairController = Service.Get<PostBattleRepairController>();
			this.nodeList = this.entityController.GetNodeList<GeneratorViewNode>();
			this.storageEffects = Service.Get<StorageEffects>();
			for (GeneratorViewNode generatorViewNode = this.nodeList.Head; generatorViewNode != null; generatorViewNode = generatorViewNode.Next)
			{
				if (generatorViewNode.GeneratorView != null)
				{
					generatorViewNode.GeneratorView.SetEnabled(true);
				}
			}
		}

		public override void RemoveFromGame(IGame game)
		{
			for (GeneratorViewNode generatorViewNode = this.nodeList.Head; generatorViewNode != null; generatorViewNode = generatorViewNode.Next)
			{
				if (generatorViewNode.GeneratorView != null)
				{
					generatorViewNode.GeneratorView.ShowCollectButton(false);
					generatorViewNode.GeneratorView.SetEnabled(false);
				}
			}
			this.entityController = null;
			this.nodeList = null;
		}

		protected override void Update(float dt)
		{
			if (this.nodeList == null)
			{
				return;
			}
			this.timeSinceViewUpdate += dt;
			this.timeSinceFullnessUpdate += dt;
			if (this.timeSinceViewUpdate >= 1f)
			{
				bool flag = false;
				if (this.timeSinceFullnessUpdate >= 5f)
				{
					flag = true;
					this.timeSinceFullnessUpdate = 0f;
				}
				for (GeneratorViewNode generatorViewNode = this.nodeList.Head; generatorViewNode != null; generatorViewNode = generatorViewNode.Next)
				{
					if (!this.postBattleRepairController.IsEntityInRepair(generatorViewNode.Entity))
					{
						this.currencyController.UpdateGeneratorAccruedCurrency((SmartEntity)generatorViewNode.Entity);
						if (flag)
						{
							this.storageEffects.UpdateFillState(generatorViewNode.Entity, generatorViewNode.BuildingComp.BuildingType);
						}
					}
				}
				this.timeSinceViewUpdate = 0f;
			}
		}

		public GeneratorSystem()
		{
		}

		protected internal GeneratorSystem(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((GeneratorSystem)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GeneratorSystem)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GeneratorSystem)GCHandledObjects.GCHandleToObject(instance)).Update(*(float*)args);
			return -1L;
		}
	}
}

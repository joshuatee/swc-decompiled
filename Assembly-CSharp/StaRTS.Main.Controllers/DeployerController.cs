using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.World.Deploying;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class DeployerController
	{
		private AbstractDeployer[] deployers;

		public TroopDeployer TroopDeployer
		{
			get;
			private set;
		}

		public SpecialAttackDeployer SpecialAttackDeployer
		{
			get;
			private set;
		}

		public HeroDeployer HeroDeployer
		{
			get;
			private set;
		}

		public ChampionDeployer ChampionDeployer
		{
			get;
			private set;
		}

		public SquadTroopDeployer SquadTroopDeployer
		{
			get;
			private set;
		}

		public DeployerController()
		{
			Service.Set<DeployerController>(this);
			this.TroopDeployer = new TroopDeployer();
			this.SpecialAttackDeployer = new SpecialAttackDeployer();
			this.HeroDeployer = new HeroDeployer();
			this.ChampionDeployer = new ChampionDeployer();
			this.SquadTroopDeployer = new SquadTroopDeployer();
			this.deployers = new AbstractDeployer[]
			{
				this.TroopDeployer,
				this.SpecialAttackDeployer,
				this.HeroDeployer,
				this.ChampionDeployer,
				this.SquadTroopDeployer
			};
		}

		public void EnterTroopPlacementMode(TroopTypeVO troopType)
		{
			this.ExitAllDeployModesExcept(this.TroopDeployer);
			this.TroopDeployer.EnterPlacementMode(troopType);
		}

		public void EnterSpecialAttackPlacementMode(SpecialAttackTypeVO specialAttackType)
		{
			this.ExitAllDeployModesExcept(this.SpecialAttackDeployer);
			this.SpecialAttackDeployer.EnterPlacementMode(specialAttackType);
		}

		public void EnterHeroDeployMode(TroopTypeVO troopType)
		{
			this.ExitAllDeployModesExcept(this.HeroDeployer);
			this.HeroDeployer.EnterMode(troopType);
		}

		public void EnterChampionDeployMode(TroopTypeVO troopType)
		{
			this.ExitAllDeployModesExcept(this.ChampionDeployer);
			this.ChampionDeployer.EnterMode(troopType);
		}

		public void EnterSquadTroopPlacementMode()
		{
			this.ExitAllDeployModesExcept(this.SquadTroopDeployer);
			this.SquadTroopDeployer.EnterPlacementMode();
		}

		private void ExitAllDeployModesExcept(AbstractDeployer deployer)
		{
			int i = 0;
			int num = this.deployers.Length;
			while (i < num)
			{
				if (this.deployers[i] != deployer)
				{
					this.deployers[i].ExitMode();
				}
				i++;
			}
		}

		public void ExitAllDeployModes()
		{
			int i = 0;
			int num = this.deployers.Length;
			while (i < num)
			{
				this.deployers[i].ExitMode();
				i++;
			}
		}

		protected internal DeployerController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).EnterChampionDeployMode((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).EnterHeroDeployMode((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).EnterSpecialAttackPlacementMode((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).EnterSquadTroopPlacementMode();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).EnterTroopPlacementMode((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).ExitAllDeployModes();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).ExitAllDeployModesExcept((AbstractDeployer)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployerController)GCHandledObjects.GCHandleToObject(instance)).ChampionDeployer);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployerController)GCHandledObjects.GCHandleToObject(instance)).HeroDeployer);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployerController)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackDeployer);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployerController)GCHandledObjects.GCHandleToObject(instance)).SquadTroopDeployer);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployerController)GCHandledObjects.GCHandleToObject(instance)).TroopDeployer);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).ChampionDeployer = (ChampionDeployer)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).HeroDeployer = (HeroDeployer)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackDeployer = (SpecialAttackDeployer)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).SquadTroopDeployer = (SquadTroopDeployer)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((DeployerController)GCHandledObjects.GCHandleToObject(instance)).TroopDeployer = (TroopDeployer)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

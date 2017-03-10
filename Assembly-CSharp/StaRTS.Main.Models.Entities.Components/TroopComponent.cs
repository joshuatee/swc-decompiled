using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TroopComponent : WalkerComponent
	{
		public ITroopDeployableVO TroopType;

		public ITroopShooterVO TroopShooterVO
		{
			get;
			set;
		}

		public ITroopShooterVO OriginalTroopShooterVO
		{
			get;
			set;
		}

		public IAudioVO AudioVO
		{
			get;
			set;
		}

		public TroopAbilityVO AbilityVO
		{
			get;
			private set;
		}

		public int TargetCount
		{
			get;
			set;
		}

		public bool UpdateWallAttackerTroop
		{
			get;
			set;
		}

		public bool IsAbilityModeActive
		{
			get;
			set;
		}

		public TroopComponent(TroopTypeVO troopType) : base(troopType.AssetName, troopType)
		{
			this.TroopType = troopType;
			this.TroopShooterVO = troopType;
			this.OriginalTroopShooterVO = troopType;
			this.AudioVO = troopType;
			if (!string.IsNullOrEmpty(troopType.Ability))
			{
				this.AbilityVO = Service.Get<IDataController>().Get<TroopAbilityVO>(troopType.Ability);
			}
			base.SetVOData(troopType);
			this.TargetCount = 0;
			this.UpdateWallAttackerTroop = false;
			this.IsAbilityModeActive = false;
		}

		public void SetVOData(ITroopShooterVO troopVO, ISpeedVO speedVO)
		{
			this.TroopShooterVO = troopVO;
			base.SetVOData(speedVO);
		}

		protected internal TroopComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).AbilityVO);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).AudioVO);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).IsAbilityModeActive);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).OriginalTroopShooterVO);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).TargetCount);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).TroopShooterVO);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).UpdateWallAttackerTroop);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).AbilityVO = (TroopAbilityVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).AudioVO = (IAudioVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).IsAbilityModeActive = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).OriginalTroopShooterVO = (ITroopShooterVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).TargetCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).TroopShooterVO = (ITroopShooterVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).UpdateWallAttackerTroop = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((TroopComponent)GCHandledObjects.GCHandleToObject(instance)).SetVOData((ITroopShooterVO)GCHandledObjects.GCHandleToObject(*args), (ISpeedVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}

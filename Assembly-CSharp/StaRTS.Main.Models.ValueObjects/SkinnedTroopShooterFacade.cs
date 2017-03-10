using System;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class SkinnedTroopShooterFacade : SkinnedShooterFacade, ITroopShooterVO, IShooterVO
	{
		public bool TargetLocking
		{
			get
			{
				return ((ITroopShooterVO)this.original).TargetLocking;
			}
		}

		public bool TargetSelf
		{
			get
			{
				return ((ITroopShooterVO)this.original).TargetSelf;
			}
		}

		public SkinnedTroopShooterFacade(ITroopShooterVO original, SkinOverrideTypeVO skinned) : base(original, skinned)
		{
		}

		protected internal SkinnedTroopShooterFacade(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinnedTroopShooterFacade)GCHandledObjects.GCHandleToObject(instance)).TargetLocking);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SkinnedTroopShooterFacade)GCHandledObjects.GCHandleToObject(instance)).TargetSelf);
		}
	}
}

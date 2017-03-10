using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers.Entities.StateMachines.Attack;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class ShooterComponent : ComponentBase
	{
		public uint MinAttackRangeSquared;

		public uint MaxAttackRangeSquared;

		public uint RetargetingOffsetSquared;

		private SmartEntity mTarget;

		public IShooterVO ShooterVO
		{
			get;
			set;
		}

		public IShooterVO OriginalShooterVO
		{
			get;
			set;
		}

		public bool FirstTargetAcquired
		{
			get;
			set;
		}

		public uint ShotsRemainingInClip
		{
			get;
			set;
		}

		public bool ShouldCountClips
		{
			get;
			set;
		}

		public uint NumClipsUsed
		{
			get;
			set;
		}

		public bool TargetingDelayed
		{
			get;
			set;
		}

		public int TargetingDelayAmount
		{
			get;
			set;
		}

		public uint MinimumFrameCountForNextTargeting
		{
			get;
			set;
		}

		public bool Searching
		{
			get;
			set;
		}

		public bool ReevaluateTarget
		{
			get;
			set;
		}

		public bool IsMelee
		{
			get;
			private set;
		}

		public bool isSkinned
		{
			get;
			set;
		}

		public AttackFSM AttackFSM
		{
			get;
			set;
		}

		public int TargetOriginalBoardX
		{
			get;
			set;
		}

		public int TargetOriginalBoardZ
		{
			get;
			set;
		}

		public SmartEntity Target
		{
			get
			{
				return this.mTarget;
			}
			set
			{
				this.mTarget = value;
				if (this.mTarget != null && this.mTarget.TransformComp != null)
				{
					TransformComponent transformComp = this.mTarget.TransformComp;
					this.TargetOriginalBoardX = transformComp.CenterGridX();
					this.TargetOriginalBoardZ = transformComp.CenterGridZ();
				}
			}
		}

		public ShooterComponent(IShooterVO shooterVO)
		{
			this.OriginalShooterVO = shooterVO;
			this.SetVOData(shooterVO);
			this.Reset();
		}

		public void Reset()
		{
			this.Searching = true;
			this.ReevaluateTarget = false;
			this.FirstTargetAcquired = false;
			this.MinimumFrameCountForNextTargeting = 0u;
			this.ShotsRemainingInClip = 0u;
			this.NumClipsUsed = 0u;
			this.ShouldCountClips = false;
			this.TargetingDelayed = false;
			this.TargetingDelayAmount = 0;
			this.TargetOriginalBoardX = 0;
			this.TargetOriginalBoardZ = 0;
			this.AttackFSM = null;
			this.mTarget = null;
		}

		public void SetVOData(IShooterVO shooterVO)
		{
			this.ShooterVO = shooterVO;
			this.MinAttackRangeSquared = shooterVO.MinAttackRange * shooterVO.MinAttackRange;
			this.MaxAttackRangeSquared = shooterVO.MaxAttackRange * shooterVO.MaxAttackRange;
			this.RetargetingOffsetSquared = shooterVO.RetargetingOffset * shooterVO.RetargetingOffset;
			if (shooterVO is TroopTypeVO)
			{
				this.IsMelee = (shooterVO.MaxAttackRange < 4u);
				return;
			}
			this.IsMelee = false;
		}

		public bool PrimaryTargetMoved()
		{
			if (this.mTarget == null || this.mTarget.TransformComp == null)
			{
				return false;
			}
			TransformComponent transformComp = this.mTarget.TransformComp;
			bool flag = (long)GameUtils.SquaredDistance(this.TargetOriginalBoardX, this.TargetOriginalBoardZ, transformComp.CenterGridX(), transformComp.CenterGridZ()) > (long)((ulong)this.RetargetingOffsetSquared);
			if (flag)
			{
				this.TargetOriginalBoardX = transformComp.CenterGridX();
				this.TargetOriginalBoardZ = transformComp.CenterGridZ();
			}
			return flag;
		}

		protected internal ShooterComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).AttackFSM);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).FirstTargetAcquired);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).IsMelee);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).isSkinned);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).OriginalShooterVO);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).ReevaluateTarget);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).Searching);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).ShooterVO);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).ShouldCountClips);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).Target);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).TargetingDelayAmount);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).TargetingDelayed);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).TargetOriginalBoardX);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).TargetOriginalBoardZ);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).PrimaryTargetMoved());
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).AttackFSM = (AttackFSM)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).FirstTargetAcquired = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).IsMelee = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).isSkinned = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).OriginalShooterVO = (IShooterVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).ReevaluateTarget = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).Searching = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).ShooterVO = (IShooterVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).ShouldCountClips = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).Target = (SmartEntity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).TargetingDelayAmount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).TargetingDelayed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).TargetOriginalBoardX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).TargetOriginalBoardZ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((ShooterComponent)GCHandledObjects.GCHandleToObject(instance)).SetVOData((IShooterVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

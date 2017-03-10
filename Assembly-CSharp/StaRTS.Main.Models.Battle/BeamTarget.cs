using StaRTS.Main.Models.Entities;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle
{
	public class BeamTarget
	{
		private int maxDamagePercent;

		public SmartEntity Target
		{
			get;
			private set;
		}

		public bool IsFirstHit
		{
			get;
			private set;
		}

		public bool HitThisSegment
		{
			get;
			private set;
		}

		public int TotalHitCount
		{
			get;
			private set;
		}

		public int CurDamagePercent
		{
			get;
			private set;
		}

		public BeamTarget(SmartEntity target)
		{
			this.Target = target;
			this.IsFirstHit = true;
			this.HitThisSegment = false;
			this.TotalHitCount = 0;
			this.CurDamagePercent = 0;
			this.maxDamagePercent = 0;
		}

		public void ApplyBeamDamage(int damagePercent)
		{
			if (damagePercent > this.maxDamagePercent)
			{
				this.CurDamagePercent += damagePercent - this.maxDamagePercent;
				this.maxDamagePercent = damagePercent;
			}
			this.HitThisSegment = true;
			int totalHitCount = this.TotalHitCount;
			this.TotalHitCount = totalHitCount + 1;
		}

		public void OnBeamAdvance()
		{
			this.IsFirstHit = false;
			this.HitThisSegment = false;
			this.CurDamagePercent = 0;
		}

		protected internal BeamTarget(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).ApplyBeamDamage(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).CurDamagePercent);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).HitThisSegment);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).IsFirstHit);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).Target);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).TotalHitCount);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).OnBeamAdvance();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).CurDamagePercent = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).HitThisSegment = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).IsFirstHit = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).Target = (SmartEntity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((BeamTarget)GCHandledObjects.GCHandleToObject(instance)).TotalHitCount = *(int*)args;
			return -1L;
		}
	}
}

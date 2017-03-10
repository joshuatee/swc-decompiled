using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers.ServerMessages
{
	public class DeltaMessage : AbstractMessage
	{
		private const string ATTACK_RATING_DELTA = "scalars.attackRating";

		private const string DEFENSE_RATING_DELTA = "scalars.defenseRating";

		public int CreditsDelta
		{
			get;
			private set;
		}

		public int MaterialsDelta
		{
			get;
			private set;
		}

		public int ContrabandDelta
		{
			get;
			private set;
		}

		public int AttackRatingDelta
		{
			get;
			private set;
		}

		public int DefenseRatingDelta
		{
			get;
			private set;
		}

		public override object MessageCookie
		{
			get
			{
				return this;
			}
		}

		public override EventId MessageEventId
		{
			get
			{
				return EventId.PvpNewBattleOccured;
			}
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("credits"))
			{
				this.CreditsDelta = Convert.ToInt32(dictionary["credits"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("materials"))
			{
				this.MaterialsDelta = Convert.ToInt32(dictionary["materials"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("contraband"))
			{
				this.ContrabandDelta = Convert.ToInt32(dictionary["contraband"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("scalars.attackRating"))
			{
				this.AttackRatingDelta = Convert.ToInt32(dictionary["scalars.attackRating"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("scalars.defenseRating"))
			{
				this.AttackRatingDelta = Convert.ToInt32(dictionary["scalars.defenseRating"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		public DeltaMessage()
		{
		}

		protected internal DeltaMessage(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).AttackRatingDelta);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).ContrabandDelta);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).CreditsDelta);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).DefenseRatingDelta);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).MaterialsDelta);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).MessageCookie);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).MessageEventId);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).AttackRatingDelta = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).ContrabandDelta = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).CreditsDelta = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).DefenseRatingDelta = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((DeltaMessage)GCHandledObjects.GCHandleToObject(instance)).MaterialsDelta = *(int*)args;
			return -1L;
		}
	}
}

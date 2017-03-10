using StaRTS.Main.Models.Commands.Tournament;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Misc
{
	public class Tournament : AbstractTimedEvent
	{
		public int Rating
		{
			get;
			set;
		}

		public List<string> RedeemedRewards
		{
			get;
			set;
		}

		public TournamentRank CurrentRank
		{
			get;
			set;
		}

		public TournamentRank FinalRank
		{
			get;
			set;
		}

		public Tournament()
		{
			this.RedeemedRewards = new List<string>();
			this.CurrentRank = new TournamentRank();
			this.FinalRank = new TournamentRank();
		}

		public override ISerializable FromObject(object obj)
		{
			base.FromObject(obj);
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				if (base.Collected)
				{
					this.FinalRank.FromObject(obj);
				}
				if (dictionary.ContainsKey("rating"))
				{
					this.Rating = Convert.ToInt32(dictionary["rating"], CultureInfo.InvariantCulture);
				}
				this.RedeemedRewards.Clear();
				if (dictionary.ContainsKey("redeemedRewards"))
				{
					List<object> list = dictionary["redeemedRewards"] as List<object>;
					if (list != null)
					{
						int count = list.Count;
						for (int i = 0; i < count; i++)
						{
							this.RedeemedRewards.Add(Convert.ToString(list[i], CultureInfo.InvariantCulture));
						}
					}
				}
			}
			return this;
		}

		public void Sync(Tournament tournament)
		{
			if (tournament == null)
			{
				return;
			}
			if (base.Uid != tournament.Uid)
			{
				Service.Get<StaRTSLogger>().Error("Trying to sync mismatched tournament data.");
				return;
			}
			base.Collected = tournament.Collected;
			this.Rating = tournament.Rating;
			this.FinalRank = tournament.FinalRank;
			this.RedeemedRewards = new List<string>(tournament.RedeemedRewards);
		}

		public void UpdateRatingAndCurrentRank(object obj)
		{
			if (obj != null)
			{
				Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
				if (dictionary != null && dictionary.ContainsKey("value"))
				{
					this.Rating = Convert.ToInt32(dictionary["value"], CultureInfo.InvariantCulture);
				}
				this.CurrentRank.FromObject(obj);
			}
		}

		protected internal Tournament(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Tournament)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Tournament)GCHandledObjects.GCHandleToObject(instance)).CurrentRank);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Tournament)GCHandledObjects.GCHandleToObject(instance)).FinalRank);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Tournament)GCHandledObjects.GCHandleToObject(instance)).Rating);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Tournament)GCHandledObjects.GCHandleToObject(instance)).RedeemedRewards);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((Tournament)GCHandledObjects.GCHandleToObject(instance)).CurrentRank = (TournamentRank)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((Tournament)GCHandledObjects.GCHandleToObject(instance)).FinalRank = (TournamentRank)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((Tournament)GCHandledObjects.GCHandleToObject(instance)).Rating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((Tournament)GCHandledObjects.GCHandleToObject(instance)).RedeemedRewards = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((Tournament)GCHandledObjects.GCHandleToObject(instance)).Sync((Tournament)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((Tournament)GCHandledObjects.GCHandleToObject(instance)).UpdateRatingAndCurrentRank(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Tournament
{
	public class TournamentRank : AbstractResponse
	{
		public const double MAX_PERCENTILE = 100.0;

		private const double MIN_PERCENTILE = 0.01;

		public double Percentile
		{
			get;
			set;
		}

		public string TierUid
		{
			get;
			set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				bool flag = false;
				if (dictionary.ContainsKey("percentile"))
				{
					this.Percentile = 100.0 - Convert.ToDouble(dictionary["percentile"], CultureInfo.InvariantCulture);
					if (this.Percentile <= 0.01)
					{
						this.Percentile = 0.01;
					}
					flag = true;
				}
				else
				{
					this.Percentile = 100.0;
					Service.Get<StaRTSLogger>().Error("PERCENTILE value not found in TournamentRank response");
				}
				if (dictionary.ContainsKey("tier"))
				{
					this.TierUid = Convert.ToString(dictionary["tier"], CultureInfo.InvariantCulture);
				}
				else if (flag)
				{
					this.TierUid = this.GetTierIdForPercentage(this.Percentile);
				}
			}
			return this;
		}

		private string GetTierIdForPercentage(double percentile)
		{
			TournamentTierVO tournamentTierVO = null;
			IDataController dataController = Service.Get<IDataController>();
			foreach (TournamentTierVO current in dataController.GetAll<TournamentTierVO>())
			{
				if (percentile <= (double)current.Percentage && (tournamentTierVO == null || tournamentTierVO.Percentage > current.Percentage))
				{
					tournamentTierVO = current;
				}
			}
			if (tournamentTierVO != null)
			{
				return tournamentTierVO.Uid;
			}
			return null;
		}

		public TournamentRank()
		{
		}

		protected internal TournamentRank(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRank)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRank)GCHandledObjects.GCHandleToObject(instance)).TierUid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TournamentRank)GCHandledObjects.GCHandleToObject(instance)).GetTierIdForPercentage(*(double*)args));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TournamentRank)GCHandledObjects.GCHandleToObject(instance)).Percentile = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TournamentRank)GCHandledObjects.GCHandleToObject(instance)).TierUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}

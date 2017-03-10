using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Raids
{
	public class RaidDefenseCompleteResponse : AbstractResponse
	{
		public string AwardedCrateUid
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("raidData"))
			{
				Dictionary<string, object> raidData = dictionary["raidData"] as Dictionary<string, object>;
				Service.Get<CurrentPlayer>().SetupRaidFromDictionary(raidData);
			}
			if (dictionary.ContainsKey("awardedCrateUid"))
			{
				this.AwardedCrateUid = (string)dictionary["awardedCrateUid"];
			}
			if (dictionary.ContainsKey("crates"))
			{
				Dictionary<string, object> obj2 = dictionary["crates"] as Dictionary<string, object>;
				bool flag = !string.IsNullOrEmpty(this.AwardedCrateUid);
				InventoryCrates crates = Service.Get<CurrentPlayer>().Prizes.Crates;
				if (flag)
				{
					crates.UpdateAndBadgeFromServerObject(obj2);
				}
				else
				{
					crates.FromObject(obj2);
				}
			}
			return this;
		}

		public RaidDefenseCompleteResponse()
		{
		}

		protected internal RaidDefenseCompleteResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseCompleteResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RaidDefenseCompleteResponse)GCHandledObjects.GCHandleToObject(instance)).AwardedCrateUid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((RaidDefenseCompleteResponse)GCHandledObjects.GCHandleToObject(instance)).AwardedCrateUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}

using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands
{
	public class SharedPrefRequest : PlayerIdRequest
	{
		private string key;

		private string value;

		public SharedPrefRequest(string key, string value)
		{
			this.key = key;
			this.value = value;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", Service.Get<CurrentPlayer>().PlayerId);
			serializer.AddDictionary<string>("sharedPrefs", new Dictionary<string, string>
			{
				{
					this.key,
					this.value
				}
			});
			return serializer.End().ToString();
		}

		protected internal SharedPrefRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SharedPrefRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}

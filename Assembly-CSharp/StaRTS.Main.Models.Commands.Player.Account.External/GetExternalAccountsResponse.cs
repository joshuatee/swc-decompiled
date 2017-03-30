using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Models.Commands.Player.Account.External
{
	public class GetExternalAccountsResponse : AbstractResponse
	{
		public string FacebookAccountId
		{
			get;
			set;
		}

		public string GameCenterAccountId
		{
			get;
			set;
		}

		public string GooglePlayAccountId
		{
			get;
			set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				string key = AccountProvider.FACEBOOK.ToString();
				string key2 = AccountProvider.GAMECENTER.ToString();
				string key3 = AccountProvider.GOOGLEPLAY.ToString();
				if (dictionary.ContainsKey(key))
				{
					List<object> list = dictionary[key] as List<object>;
					if (list != null && list.Count > 0)
					{
						this.FacebookAccountId = (list[0] as string);
					}
				}
				if (dictionary.ContainsKey(key2))
				{
					List<object> list = dictionary[key2] as List<object>;
					if (list != null && list.Count > 0)
					{
						this.GameCenterAccountId = (list[0] as string);
					}
				}
				if (dictionary.ContainsKey(key3))
				{
					List<object> list = dictionary[key3] as List<object>;
					if (list != null && list.Count > 0)
					{
						this.GooglePlayAccountId = (list[0] as string);
					}
				}
			}
			return this;
		}
	}
}

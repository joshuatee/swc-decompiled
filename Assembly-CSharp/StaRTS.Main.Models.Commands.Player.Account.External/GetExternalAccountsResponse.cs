using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

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

		public GetExternalAccountsResponse()
		{
		}

		protected internal GetExternalAccountsResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetExternalAccountsResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetExternalAccountsResponse)GCHandledObjects.GCHandleToObject(instance)).FacebookAccountId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetExternalAccountsResponse)GCHandledObjects.GCHandleToObject(instance)).GameCenterAccountId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetExternalAccountsResponse)GCHandledObjects.GCHandleToObject(instance)).GooglePlayAccountId);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GetExternalAccountsResponse)GCHandledObjects.GCHandleToObject(instance)).FacebookAccountId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GetExternalAccountsResponse)GCHandledObjects.GCHandleToObject(instance)).GameCenterAccountId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GetExternalAccountsResponse)GCHandledObjects.GCHandleToObject(instance)).GooglePlayAccountId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}

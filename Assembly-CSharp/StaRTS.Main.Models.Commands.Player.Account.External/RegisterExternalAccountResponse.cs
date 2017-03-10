using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Account.External
{
	public class RegisterExternalAccountResponse : AbstractResponse
	{
		public Dictionary<string, PlayerIdentityInfo> PlayerIdentities
		{
			get;
			private set;
		}

		public string Secret
		{
			get;
			private set;
		}

		public int LastSyncedTimeStamp
		{
			get;
			private set;
		}

		public RegisterExternalAccountResponse()
		{
			this.PlayerIdentities = new Dictionary<string, PlayerIdentityInfo>();
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				if (dictionary.ContainsKey("identities"))
				{
					Dictionary<string, object> dictionary2 = dictionary["identities"] as Dictionary<string, object>;
					if (dictionary2 != null)
					{
						foreach (KeyValuePair<string, object> current in dictionary2)
						{
							PlayerIdentityInfo playerIdentityInfo = new PlayerIdentityInfo();
							playerIdentityInfo.FromObject(current.get_Value());
							this.PlayerIdentities.Add(current.get_Key(), playerIdentityInfo);
						}
					}
				}
				if (dictionary.ContainsKey("secret"))
				{
					this.Secret = (string)dictionary["secret"];
				}
				if (dictionary.ContainsKey("registrationTime"))
				{
					this.LastSyncedTimeStamp = Convert.ToInt32((string)dictionary["registrationTime"], CultureInfo.InvariantCulture);
				}
			}
			return this;
		}

		protected internal RegisterExternalAccountResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountResponse)GCHandledObjects.GCHandleToObject(instance)).LastSyncedTimeStamp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountResponse)GCHandledObjects.GCHandleToObject(instance)).PlayerIdentities);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountResponse)GCHandledObjects.GCHandleToObject(instance)).Secret);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((RegisterExternalAccountResponse)GCHandledObjects.GCHandleToObject(instance)).LastSyncedTimeStamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((RegisterExternalAccountResponse)GCHandledObjects.GCHandleToObject(instance)).PlayerIdentities = (Dictionary<string, PlayerIdentityInfo>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((RegisterExternalAccountResponse)GCHandledObjects.GCHandleToObject(instance)).Secret = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}

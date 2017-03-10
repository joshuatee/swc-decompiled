using StaRTS.Main.Models.Leaderboard;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class SocialFriendData : ISerializable
	{
		public const string KEY_ID = "id";

		public const string KEY_NAME = "name";

		public const string KEY_PICTURE = "picture";

		public const string KEY_DATA = "data";

		public const string KEY_URL = "url";

		public const string KEY_INSTALLED = "installed";

		public string Id
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public string PictureURL
		{
			get;
			private set;
		}

		public bool Installed
		{
			get;
			private set;
		}

		public PlayerLBEntity PlayerData
		{
			get;
			set;
		}

		public string ToJson()
		{
			return "";
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Id = (dictionary["id"] as string);
			this.Name = (dictionary["name"] as string);
			if (dictionary.ContainsKey("installed"))
			{
				this.Installed = Convert.ToBoolean(dictionary["installed"], CultureInfo.InvariantCulture);
			}
			else
			{
				this.Installed = false;
			}
			if (dictionary.ContainsKey("picture"))
			{
				Dictionary<string, object> dictionary2 = dictionary["picture"] as Dictionary<string, object>;
				Dictionary<string, object> dictionary3 = dictionary2["data"] as Dictionary<string, object>;
				this.PictureURL = WWW.UnEscapeURL((string)dictionary3["url"]);
			}
			return this;
		}

		public ISerializable FromFriendObject(Dictionary<string, object> data)
		{
			this.Id = (data["id"] as string);
			this.Name = (data["name"] as string);
			if (data.ContainsKey("installed"))
			{
				this.Installed = Convert.ToBoolean(data["installed"], CultureInfo.InvariantCulture);
			}
			else
			{
				this.Installed = false;
			}
			if (data.ContainsKey("picture"))
			{
				this.PictureURL = WWW.UnEscapeURL((string)data["picture"]);
			}
			return this;
		}

		public SocialFriendData()
		{
		}

		protected internal SocialFriendData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).FromFriendObject((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).Id);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).Installed);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).Name);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).PictureURL);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).PlayerData);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).Id = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).Installed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).Name = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).PictureURL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).PlayerData = (PlayerLBEntity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SocialFriendData)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}

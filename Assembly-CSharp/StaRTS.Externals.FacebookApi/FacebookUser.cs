using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.FacebookApi
{
	public class FacebookUser
	{
		public const string ANONYMOUS_FBID = "Anonymous";

		public string FBUserID
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public long Score
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string AvatarURL
		{
			get;
			set;
		}

		public Texture2D Avatar
		{
			get;
			set;
		}

		public bool IsAppFriend
		{
			get;
			set;
		}

		public float ExchangeRate
		{
			get;
			set;
		}

		public FacebookUser()
		{
			this.FBUserID = "";
			this.Name = "Player";
			this.FirstName = "Player";
			this.Score = 0L;
		}

		protected internal FacebookUser(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).Avatar);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).AvatarURL);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).ExchangeRate);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).FBUserID);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).FirstName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).IsAppFriend);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).LastName);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).Name);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).Score);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).Avatar = (Texture2D)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).AvatarURL = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).ExchangeRate = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).FBUserID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).FirstName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).IsAppFriend = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).LastName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).Name = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((FacebookUser)GCHandledObjects.GCHandleToObject(instance)).Score = *args;
			return -1L;
		}
	}
}

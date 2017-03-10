using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Tags
{
	public class PlayerVisitTag
	{
		public bool IsSquadMate
		{
			get;
			private set;
		}

		public bool IsFriend
		{
			get;
			private set;
		}

		public string TabName
		{
			get;
			private set;
		}

		public string PlayerId
		{
			get;
			private set;
		}

		public PlayerVisitTag(bool isSquadMate, bool isFriend, string tabName, string playerId)
		{
			this.IsSquadMate = isSquadMate;
			this.IsFriend = isFriend;
			this.TabName = tabName;
			this.PlayerId = playerId;
		}

		protected internal PlayerVisitTag(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerVisitTag)GCHandledObjects.GCHandleToObject(instance)).IsFriend);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerVisitTag)GCHandledObjects.GCHandleToObject(instance)).IsSquadMate);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerVisitTag)GCHandledObjects.GCHandleToObject(instance)).PlayerId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerVisitTag)GCHandledObjects.GCHandleToObject(instance)).TabName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PlayerVisitTag)GCHandledObjects.GCHandleToObject(instance)).IsFriend = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PlayerVisitTag)GCHandledObjects.GCHandleToObject(instance)).IsSquadMate = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((PlayerVisitTag)GCHandledObjects.GCHandleToObject(instance)).PlayerId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PlayerVisitTag)GCHandledObjects.GCHandleToObject(instance)).TabName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}

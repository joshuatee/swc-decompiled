using StaRTS.Externals.GameServices;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType417 : $UnityType
	{
		public unsafe $UnityType417()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 444720) = ldftn($Invoke0);
			*(data + 444748) = ldftn($Invoke1);
			*(data + 444776) = ldftn($Invoke2);
			*(data + 444804) = ldftn($Invoke3);
			*(data + 444832) = ldftn($Invoke4);
			*(data + 444860) = ldftn($Invoke5);
			*(data + 444888) = ldftn($Invoke6);
			*(data + 444916) = ldftn($Invoke7);
			*(data + 444944) = ldftn($Invoke8);
			*(data + 444972) = ldftn($Invoke9);
			*(data + 445000) = ldftn($Invoke10);
			*(data + 445028) = ldftn($Invoke11);
			*(data + 445056) = ldftn($Invoke12);
			*(data + 445084) = ldftn($Invoke13);
			*(data + 445112) = ldftn($Invoke14);
			*(data + 445140) = ldftn($Invoke15);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).AddScoreToLeaderboard(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).GetAuthToken());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).GetExternalNetworkCode());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).GetUserDomain());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).GetUserId());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).HandleAuthTokenCallback(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).HandleUserIdCallback(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).HasBeenPromptedForSignIn());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).IsUserAuthenticated());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).OnReady();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).Share(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).ShowAchievements();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).ShowLeaderboard(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).SignIn();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).SignOut();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((IGameServicesHelper)GCHandledObjects.GCHandleToObject(instance)).UnlockAchievement(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}

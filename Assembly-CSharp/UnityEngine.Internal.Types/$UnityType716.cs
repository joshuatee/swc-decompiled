using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player.Account.External;
using StaRTS.Main.Utils.Events;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType716 : $UnityType
	{
		public unsafe $UnityType716()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 536532) = ldftn($Invoke0);
			*(data + 536560) = ldftn($Invoke1);
			*(data + 536588) = ldftn($Invoke2);
			*(data + 536616) = ldftn($Invoke3);
			*(data + 536644) = ldftn($Invoke4);
			*(data + 536672) = ldftn($Invoke5);
			*(data + 536700) = ldftn($Invoke6);
			*(data + 536728) = ldftn($Invoke7);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAccountSyncController)GCHandledObjects.GCHandleToObject(instance)).GetAccountProviderId((AccountProvider)(*(int*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IAccountSyncController)GCHandledObjects.GCHandleToObject(instance)).LoadAccount(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAccountSyncController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((IAccountSyncController)GCHandledObjects.GCHandleToObject(instance)).OnFacebookSignIn();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IAccountSyncController)GCHandledObjects.GCHandleToObject(instance)).RegisterExternalAccount((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((IAccountSyncController)GCHandledObjects.GCHandleToObject(instance)).UnregisterFacebookAccount();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((IAccountSyncController)GCHandledObjects.GCHandleToObject(instance)).UnregisterGameServicesAccount();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((IAccountSyncController)GCHandledObjects.GCHandleToObject(instance)).UpdateExternalAccountInfo((OnUpdateExternalAccountInfoResponseReceived)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

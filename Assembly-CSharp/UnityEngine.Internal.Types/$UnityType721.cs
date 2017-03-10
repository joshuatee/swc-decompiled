using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType721 : $UnityType
	{
		public unsafe $UnityType721()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 537260) = ldftn($Invoke0);
			*(data + 537288) = ldftn($Invoke1);
			*(data + 537316) = ldftn($Invoke2);
			*(data + 537344) = ldftn($Invoke3);
			*(data + 537372) = ldftn($Invoke4);
			*(data + 537400) = ldftn($Invoke5);
			*(data + 537428) = ldftn($Invoke6);
			*(data + 537456) = ldftn($Invoke7);
			*(data + 537484) = ldftn($Invoke8);
			*(data + 537512) = ldftn($Invoke9);
			*(data + 537540) = ldftn($Invoke10);
			*(data + 537568) = ldftn($Invoke11);
			*(data + 537596) = ldftn($Invoke12);
			*(data + 537624) = ldftn($Invoke13);
			*(data + 537652) = ldftn($Invoke14);
			*(data + 537680) = ldftn($Invoke15);
			*(data + 537708) = ldftn($Invoke16);
			*(data + 537736) = ldftn($Invoke17);
			*(data + 537764) = ldftn($Invoke18);
			*(data + 537792) = ldftn($Invoke19);
			*(data + 537820) = ldftn($Invoke20);
			*(data + 537848) = ldftn($Invoke21);
			*(data + 537876) = ldftn($Invoke22);
			*(data + 537904) = ldftn($Invoke23);
			*(data + 537932) = ldftn($Invoke24);
			*(data + 537960) = ldftn($Invoke25);
			*(data + 537988) = ldftn($Invoke26);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).CheckFacebookLoginOnStartup();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).DestroyFriendPicture((Texture2D)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).FacebookId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).FacebookLocale);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).FirstName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).Friends);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).FriendsDetailsCB);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).FullName);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).Gender);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).HaveAllData);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).HaveFriendData);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).HaveSelfData);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).InstalledFBIDs);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).InviteFriendsCB);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).IsLoggedIn);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).LastName);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).PlayerIdToFriendData);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).GetFriendPicture((SocialFriendData)GCHandledObjects.GCHandleToObject(*args), (OnGetProfilePicture)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).GetSelfPicture((OnGetProfilePicture)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).InviteFriends((OnRequestDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).Login((OnAllDataFetchedDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).Logout();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).PopulateFacebookData();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).FriendsDetailsCB = (OnFBFriendsDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).InviteFriendsCB = (OnRequestDelegate)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).StaticReset();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((ISocialDataController)GCHandledObjects.GCHandleToObject(instance)).UpdateFriends();
			return -1L;
		}
	}
}

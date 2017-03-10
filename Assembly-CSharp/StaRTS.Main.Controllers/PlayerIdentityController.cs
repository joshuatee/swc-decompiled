using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.Notifications;
using StaRTS.Main.Models.Commands.Player.Identity;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class PlayerIdentityController
	{
		public delegate void GetOtherPlayerIdentityCallback(PlayerIdentityInfo info);

		private const char ID_DELIMITER = '_';

		private const int DEFAULT_IDENTITY_INDEX = 0;

		private const int SECOND_IDENTITY_INDEX = 1;

		private const int MAX_NUM_FORCED_RELOADS = 3;

		private PlayerIdentityInfo otherPlayerIdentityInfo;

		public PlayerIdentityController()
		{
			Service.Set<PlayerIdentityController>(this);
		}

		public void GetOtherPlayerIdentity(PlayerIdentityController.GetOtherPlayerIdentityCallback callback)
		{
			if (this.otherPlayerIdentityInfo != null)
			{
				if (callback != null)
				{
					callback(this.otherPlayerIdentityInfo);
					return;
				}
			}
			else
			{
				int identityIndex = this.IsFirstIdentity(Service.Get<CurrentPlayer>().PlayerId) ? 1 : 0;
				PlayerIdentityGetCommand playerIdentityGetCommand = new PlayerIdentityGetCommand(new PlayerIdentityRequest
				{
					IdentityIndex = identityIndex
				});
				playerIdentityGetCommand.AddSuccessCallback(new AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>.OnSuccessCallback(this.OnGetOtherPlayerIdentity));
				playerIdentityGetCommand.Context = callback;
				Service.Get<ServerAPI>().Sync(playerIdentityGetCommand);
			}
		}

		private void OnGetOtherPlayerIdentity(PlayerIdentityGetResponse response, object cookie)
		{
			this.otherPlayerIdentityInfo = response.Info;
			PlayerIdentityController.GetOtherPlayerIdentityCallback getOtherPlayerIdentityCallback = cookie as PlayerIdentityController.GetOtherPlayerIdentityCallback;
			if (getOtherPlayerIdentityCallback != null)
			{
				getOtherPlayerIdentityCallback(this.otherPlayerIdentityInfo);
			}
		}

		public void SwitchToNewIdentity()
		{
			this.SwitchIdentity(1);
		}

		public void SwitchIdentity(string playerId)
		{
			int identityIndex = this.GetIdentityIndex(playerId);
			this.SwitchIdentity(identityIndex);
		}

		public void SwitchIdentity(int identityIndex)
		{
			PlayerIdentitySwitchCommand playerIdentitySwitchCommand = new PlayerIdentitySwitchCommand(new PlayerIdentityRequest
			{
				IdentityIndex = identityIndex
			});
			playerIdentitySwitchCommand.AddSuccessCallback(new AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>.OnSuccessCallback(this.OnPlayerIdentitySwitched));
			Service.Get<ServerAPI>().Sync(playerIdentitySwitchCommand);
		}

		private void OnPlayerIdentitySwitched(PlayerIdentitySwitchResponse response, object cookie)
		{
			this.InternalSwitchPlayer(response.PlayerId);
		}

		public void HandleInactiveIdentityError(string activePlayerId)
		{
			if (string.IsNullOrEmpty(activePlayerId))
			{
				Service.Get<StaRTSLogger>().Error("Inactive identity error but no active player id.");
				return;
			}
			if (Engine.NumReloads >= 3)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Faction flipping error: Max number of forced reloads reached for {0}", new object[]
				{
					activePlayerId
				});
				return;
			}
			this.InternalSwitchPlayer(activePlayerId);
		}

		private void InternalSwitchPlayer(string playerId)
		{
			Service.Get<NotificationController>().ClearAllPendingLocalNotifications();
			if (Service.IsSet<ServerPlayerPrefs>())
			{
				Service.Get<ServerPlayerPrefs>().SavePrefsLocally();
			}
			PlayerPrefs.SetString("prefPlayerId", playerId);
			Service.Get<Engine>().Reload();
		}

		public bool IsFirstIdentity(string playerId)
		{
			int identityIndex = this.GetIdentityIndex(playerId);
			return identityIndex == 0;
		}

		private int GetIdentityIndex(string playerId)
		{
			int result = 0;
			int num = playerId.LastIndexOf('_');
			if (num >= 0 && num < playerId.get_Length() - 1)
			{
				string text = playerId.Substring(num + 1);
				int.TryParse(text, ref result);
			}
			return result;
		}

		protected internal PlayerIdentityController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerIdentityController)GCHandledObjects.GCHandleToObject(instance)).GetIdentityIndex(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlayerIdentityController)GCHandledObjects.GCHandleToObject(instance)).GetOtherPlayerIdentity((PlayerIdentityController.GetOtherPlayerIdentityCallback)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlayerIdentityController)GCHandledObjects.GCHandleToObject(instance)).HandleInactiveIdentityError(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PlayerIdentityController)GCHandledObjects.GCHandleToObject(instance)).InternalSwitchPlayer(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerIdentityController)GCHandledObjects.GCHandleToObject(instance)).IsFirstIdentity(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PlayerIdentityController)GCHandledObjects.GCHandleToObject(instance)).OnGetOtherPlayerIdentity((PlayerIdentityGetResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((PlayerIdentityController)GCHandledObjects.GCHandleToObject(instance)).OnPlayerIdentitySwitched((PlayerIdentitySwitchResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PlayerIdentityController)GCHandledObjects.GCHandleToObject(instance)).SwitchIdentity(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((PlayerIdentityController)GCHandledObjects.GCHandleToObject(instance)).SwitchIdentity(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((PlayerIdentityController)GCHandledObjects.GCHandleToObject(instance)).SwitchToNewIdentity();
			return -1L;
		}
	}
}

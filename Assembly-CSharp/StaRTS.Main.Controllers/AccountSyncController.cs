using StaRTS.Externals.FacebookApi;
using StaRTS.Externals.GameServices;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player.Account.External;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class AccountSyncController : IEventObserver, IAccountSyncController
	{
		private GetExternalAccountsResponse externalAccountInfo;

		private bool facebookRegisterPending;

		private bool gameServicesRegisterPending;

		private OnUpdateExternalAccountInfoResponseReceived updateCallback;

		public AccountSyncController()
		{
			Service.Set<IAccountSyncController>(this);
			this.facebookRegisterPending = false;
			this.gameServicesRegisterPending = false;
			Service.Get<EventManager>().RegisterObserver(this, EventId.GameServicesSignedIn, EventPriority.Default);
		}

		public string GetAccountProviderId(AccountProvider provider)
		{
			string result = string.Empty;
			if (this.externalAccountInfo == null)
			{
				return result;
			}
			if (provider == AccountProvider.FACEBOOK)
			{
				result = this.externalAccountInfo.FacebookAccountId;
			}
			else if (provider == AccountProvider.GAMECENTER)
			{
				result = this.externalAccountInfo.GameCenterAccountId;
			}
			else if (provider == AccountProvider.GOOGLEPLAY)
			{
				result = this.externalAccountInfo.GooglePlayAccountId;
			}
			return result;
		}

		public void UpdateExternalAccountInfo(OnUpdateExternalAccountInfoResponseReceived callback)
		{
			this.updateCallback = callback;
			GetExternalAccountsCommand getExternalAccountsCommand = new GetExternalAccountsCommand(new PlayerIdRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId
			});
			getExternalAccountsCommand.AddSuccessCallback(new AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>.OnSuccessCallback(this.OnGetExternalAccountInfo));
			this.SendServerCommand(getExternalAccountsCommand, false);
		}

		private void OnGetExternalAccountInfo(GetExternalAccountsResponse response, object cookie)
		{
			this.externalAccountInfo = response;
			if (this.facebookRegisterPending)
			{
				this.RegisterFacebookAccount();
				this.facebookRegisterPending = false;
			}
			if (this.gameServicesRegisterPending)
			{
				this.RegisterGameServicesAccount();
				this.gameServicesRegisterPending = false;
			}
			if (this.updateCallback != null)
			{
				this.updateCallback();
				this.updateCallback = null;
			}
		}

		public void LoadAccount(string playerId, string playerSecret)
		{
			PlayerPrefs.SetString("prefPlayerId", playerId);
			PlayerPrefs.SetString("prefPlayerSecret", playerSecret);
			PlayerPrefs.Save();
			Service.Get<Engine>().Reload();
		}

		public void OnFacebookSignIn()
		{
			this.RegisterFacebookAccount();
		}

		private void RegisterGameServicesAccount()
		{
			string userId = GameServicesManager.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return;
			}
			if (this.externalAccountInfo == null)
			{
				this.gameServicesRegisterPending = true;
				return;
			}
			string text = null;
			if (text == userId)
			{
				return;
			}
			string authToken = GameServicesManager.GetAuthToken();
			if (string.IsNullOrEmpty(authToken))
			{
				return;
			}
			this.RegisterExternalAccount(new RegisterExternalAccountRequest
			{
				OverrideExistingAccountRegistration = false,
				ExternalAccountId = userId,
				ExternalAccountSecurityToken = authToken,
				PlayerId = Service.Get<CurrentPlayer>().PlayerId,
				OtherLinkedProvider = AccountProvider.FACEBOOK
			});
		}

		private void RegisterFacebookAccount()
		{
			string userId = FacebookManager.Instance.getUserId();
			if (FacebookManager.IsLoggedIn && !string.IsNullOrEmpty(userId))
			{
				if (this.externalAccountInfo == null)
				{
					this.facebookRegisterPending = true;
					return;
				}
				if (this.externalAccountInfo.FacebookAccountId == userId)
				{
					return;
				}
				this.RegisterExternalAccount(new RegisterExternalAccountRequest
				{
					OverrideExistingAccountRegistration = false,
					ExternalAccountId = userId,
					ExternalAccountSecurityToken = FacebookManager.Instance.getAccessToken(),
					Provider = AccountProvider.FACEBOOK,
					PlayerId = Service.Get<CurrentPlayer>().PlayerId
				});
			}
		}

		public void RegisterExternalAccount(RegisterExternalAccountRequest request)
		{
			RegisterExternalAccountCommand registerExternalAccountCommand = new RegisterExternalAccountCommand(request);
			registerExternalAccountCommand.AddSuccessCallback(new AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>.OnSuccessCallback(this.OnAccountRegisterSuccess));
			registerExternalAccountCommand.AddFailureCallback(new AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>.OnFailureCallback(this.OnAccountRegisterFailure));
			registerExternalAccountCommand.Context = registerExternalAccountCommand;
			this.SendServerCommand(registerExternalAccountCommand, true);
		}

		public void UnregisterFacebookAccount()
		{
			if (this.externalAccountInfo == null || this.externalAccountInfo.FacebookAccountId == null)
			{
				return;
			}
			if (this.externalAccountInfo.GameCenterAccountId != null || this.externalAccountInfo.GooglePlayAccountId != null)
			{
				return;
			}
			UnregisterExternalAccountRequest unregisterExternalAccountRequest = new UnregisterExternalAccountRequest();
			unregisterExternalAccountRequest.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			unregisterExternalAccountRequest.Provider = AccountProvider.FACEBOOK;
			UnregisterExternalAccountCommand unregisterExternalAccountCommand = new UnregisterExternalAccountCommand(unregisterExternalAccountRequest);
			unregisterExternalAccountCommand.AddSuccessCallback(new AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>.OnSuccessCallback(this.OnAccountUnregisterSuccess));
			unregisterExternalAccountCommand.Context = unregisterExternalAccountRequest.Provider;
			this.SendServerCommand(unregisterExternalAccountCommand, true);
		}

		public void UnregisterGameServicesAccount()
		{
			if (this.externalAccountInfo == null || (this.externalAccountInfo.GameCenterAccountId == null && this.externalAccountInfo.GooglePlayAccountId == null))
			{
				return;
			}
			if (this.externalAccountInfo.FacebookAccountId != null)
			{
				return;
			}
			UnregisterExternalAccountRequest unregisterExternalAccountRequest = new UnregisterExternalAccountRequest();
			unregisterExternalAccountRequest.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			UnregisterExternalAccountCommand unregisterExternalAccountCommand = new UnregisterExternalAccountCommand(unregisterExternalAccountRequest);
			unregisterExternalAccountCommand.AddSuccessCallback(new AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>.OnSuccessCallback(this.OnAccountUnregisterSuccess));
			unregisterExternalAccountCommand.Context = unregisterExternalAccountRequest.Provider;
			this.SendServerCommand(unregisterExternalAccountCommand, true);
		}

		private void SendServerCommand(ICommand command, bool immediate)
		{
			ServerAPI serverAPI = Service.Get<ServerAPI>();
			if (!serverAPI.Enabled && Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				if (immediate)
				{
					serverAPI.Enabled = true;
					serverAPI.Sync(command);
					serverAPI.Enabled = false;
					return;
				}
				serverAPI.Enqueue(command);
				return;
			}
			else
			{
				if (immediate)
				{
					serverAPI.Sync(command);
					return;
				}
				serverAPI.Enqueue(command);
				return;
			}
		}

		private void OnFacebookAccountRegisterSuccess(RegisterExternalAccountResponse response, object cookie)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (!currentPlayer.IsConnectedAccount)
			{
				currentPlayer.IsConnectedAccount = true;
				currentPlayer.Inventory.ModifyCrystals(GameConstants.FB_CONNECT_REWARD);
			}
		}

		private void OnAccountRegisterSuccess(RegisterExternalAccountResponse response, object cookie)
		{
			RegisterExternalAccountCommand registerExternalAccountCommand = (RegisterExternalAccountCommand)cookie;
			switch (registerExternalAccountCommand.RequestArgs.Provider)
			{
			case AccountProvider.FACEBOOK:
				this.externalAccountInfo.FacebookAccountId = registerExternalAccountCommand.RequestArgs.ExternalAccountId;
				this.OnFacebookAccountRegisterSuccess(response, cookie);
				return;
			case AccountProvider.GAMECENTER:
				this.externalAccountInfo.GameCenterAccountId = registerExternalAccountCommand.RequestArgs.ExternalAccountId;
				return;
			case AccountProvider.GOOGLEPLAY:
				this.externalAccountInfo.GooglePlayAccountId = registerExternalAccountCommand.RequestArgs.ExternalAccountId;
				return;
			default:
				return;
			}
		}

		private void OnAccountRegisterFailure(uint status, object cookie)
		{
			RegisterExternalAccountCommand registerExternalAccountCommand = (RegisterExternalAccountCommand)cookie;
			Lang lang = Service.Get<Lang>();
			string title = lang.Get("ACCOUNT_SYNC_ERROR", new object[0]);
			string message = null;
			if (status == 1318u)
			{
				switch (registerExternalAccountCommand.RequestArgs.Provider)
				{
				case AccountProvider.FACEBOOK:
					message = lang.Get("ACCOUNT_SYNC_AUTH_ERROR_FACEBOOK", new object[0]);
					break;
				case AccountProvider.GAMECENTER:
					message = lang.Get("ACCOUNT_SYNC_AUTH_ERROR_GAMECENTER", new object[0]);
					break;
				case AccountProvider.GOOGLEPLAY:
					message = lang.Get("ACCOUNT_SYNC_AUTH_ERROR_GOOGLEPLAY", new object[0]);
					break;
				}
				ProcessingScreen.Hide();
				AlertScreen.ShowModal(false, title, message, null, null);
				return;
			}
			if (status == 2200u)
			{
				ProcessingScreen.Hide();
				AccountSyncScreen screen = AccountSyncScreen.CreateSyncConflictScreen(registerExternalAccountCommand);
				Service.Get<ScreenController>().AddScreen(screen);
				return;
			}
			if (status != 2201u)
			{
				return;
			}
			switch (registerExternalAccountCommand.RequestArgs.Provider)
			{
			case AccountProvider.FACEBOOK:
				if (this.externalAccountInfo.FacebookAccountId != null && this.externalAccountInfo.FacebookAccountId != FacebookManager.Instance.getUserId())
				{
					message = lang.Get("ACCOUNT_SYNC_ERROR_FACEBOOK", new object[0]);
				}
				else if (this.externalAccountInfo.FacebookAccountId != null)
				{
					message = lang.Get("ACCOUNT_SYNC_ERROR_FACEBOOK", new object[0]);
				}
				break;
			case AccountProvider.GAMECENTER:
				if (this.externalAccountInfo.GameCenterAccountId != null && this.externalAccountInfo.GameCenterAccountId != GameServicesManager.GetUserId())
				{
					message = lang.Get("ACCOUNT_SYNC_ERROR_GAMECENTER", new object[0]);
				}
				else if (this.externalAccountInfo.FacebookAccountId != null)
				{
					message = lang.Get("ACCOUNT_SYNC_ERROR_GAMECENTER_FACEBOOK", new object[0]);
				}
				break;
			case AccountProvider.GOOGLEPLAY:
				if (this.externalAccountInfo.GooglePlayAccountId != null && this.externalAccountInfo.GooglePlayAccountId != GameServicesManager.GetUserId())
				{
					message = lang.Get("ACCOUNT_SYNC_ERROR_GOOGLEPLAY", new object[0]);
				}
				else if (this.externalAccountInfo.FacebookAccountId != null)
				{
					message = lang.Get("ACCOUNT_SYNC_ERROR_GOOGLEPLAY_FACEBOOK", new object[0]);
				}
				break;
			}
			ProcessingScreen.Hide();
			AlertScreen.ShowModal(false, title, message, null, null);
		}

		private void OnAccountUnregisterSuccess(DefaultResponse response, object cookie)
		{
			switch ((AccountProvider)cookie)
			{
			case AccountProvider.FACEBOOK:
				this.externalAccountInfo.FacebookAccountId = null;
				return;
			case AccountProvider.GAMECENTER:
				this.externalAccountInfo.GameCenterAccountId = null;
				return;
			case AccountProvider.GOOGLEPLAY:
				this.externalAccountInfo.GooglePlayAccountId = null;
				return;
			default:
				return;
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.GameServicesSignedIn)
			{
				this.RegisterGameServicesAccount();
			}
			return EatResponse.NotEaten;
		}

		protected internal AccountSyncController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).GetAccountProviderId((AccountProvider)(*(int*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).LoadAccount(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).OnAccountRegisterSuccess((RegisterExternalAccountResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).OnAccountUnregisterSuccess((DefaultResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).OnFacebookAccountRegisterSuccess((RegisterExternalAccountResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).OnFacebookSignIn();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).OnGetExternalAccountInfo((GetExternalAccountsResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).RegisterExternalAccount((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).RegisterFacebookAccount();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).RegisterGameServicesAccount();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).SendServerCommand((ICommand)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).UnregisterFacebookAccount();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).UnregisterGameServicesAccount();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AccountSyncController)GCHandledObjects.GCHandleToObject(instance)).UpdateExternalAccountInfo((OnUpdateExternalAccountInfoResponseReceived)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

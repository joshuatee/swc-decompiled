using Source.StaRTS.Main.Models.Commands.Player;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.ServerMessages;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Commands.Test.Config;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class ServerStartupTask : StartupTask
	{
		private bool useRealAuthentication;

		private bool hasSecondary;

		private bool isHandlingMismatch;

		private bool isAttemptingAuth;

		private GetAuthTokenCommand getAuthTokenCommand;

		private GeneratePlayerCommand generatePlayerCommand;

		private ProtocolResult protocolResult;

		public ServerStartupTask(float startPercentage)
		{
			this.isAttemptingAuth = true;
			base..ctor(startPercentage);
		}

		public override void Start()
		{
			this.hasSecondary = true;
			new QuietCorrectionController();
			new ServerAPI(Service.Get<AppServerEnvironmentController>().Server, Convert.ToUInt32("60", CultureInfo.InvariantCulture), Service.Get<ViewTimerManager>(), Service.Get<Engine>(), new ServerAPI.DesynHandler(this.OnDesync), new ServerAPI.MessageHandler(this.MessageHandler));
			new ServerController();
			new GameIdleController();
			this.FigureOutAuth();
		}

		private void FigureOutAuth()
		{
			this.useRealAuthentication = !AppServerEnvironmentController.IsLocalServer();
			this.GetOrCreatePlayer();
		}

		private void OnFigureOutAuthFailure(uint errorCode, object cookie)
		{
			Service.Get<StaRTSLogger>().Error("Recieved failure response on test.authtoken.isPlayerId command. Cannot continue.");
		}

		private void OnFigureOutAuthReponse(AuthTokenIsPlayerIdResponse response, object cookie)
		{
			this.useRealAuthentication = !response.AuthIsPlayerId;
			this.GetOrCreatePlayer();
		}

		private void GetOrCreatePlayer()
		{
			Service.Get<ServerAPI>().IsUsingRealAuthentication = this.useRealAuthentication;
			this.isAttemptingAuth = true;
			this.isHandlingMismatch = false;
			if (PlayerPrefs.HasKey("prefPlayerId"))
			{
				this.GetAuthToken();
				return;
			}
			this.CreatePlayer();
		}

		private void InitializeCurrentPlayer()
		{
			if (!Service.IsSet<CurrentPlayer>())
			{
				CurrentPlayer currentPlayer = new CurrentPlayer();
				currentPlayer.Init();
				new SquadController();
			}
		}

		private void GetAuthToken()
		{
			ServerAPI serverAPI = Service.Get<ServerAPI>();
			if (this.useRealAuthentication)
			{
				string requestToken = ServerStartupTask.GenerateRequestToken(PlayerPrefs.GetString("prefPlayerId"), PlayerPrefs.GetString("prefPlayerSecret"));
				this.getAuthTokenCommand = new GetAuthTokenCommand(new GetAuthTokenRequest
				{
					PlayerId = PlayerPrefs.GetString("prefPlayerId"),
					RequestToken = requestToken
				});
				this.getAuthTokenCommand.AddSuccessCallback(new AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>.OnSuccessCallback(this.OnGetAuthTokenComplete));
				this.getAuthTokenCommand.AddFailureCallback(new AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>.OnFailureCallback(this.OnGetAuthTokenFailure));
				serverAPI.Async(this.getAuthTokenCommand);
				return;
			}
			this.InitializeCurrentPlayer();
			string playerId = Service.Get<CurrentPlayer>().PlayerId;
			serverAPI.SetAuth(playerId);
			this.CompleteTask();
		}

		private void OnGetAuthTokenComplete(GetAuthTokenResponse response, object cookie)
		{
			Service.Get<ServerAPI>().SetAuth(response.AuthToken);
			this.InitializeCurrentPlayer();
			this.CompleteTask();
		}

		private void OnGetAuthTokenFailure(uint status, object cookie)
		{
			if (status == 801u || status == 800u)
			{
				Service.Get<StaRTSLogger>().Error("Locally stored authentication is invalid");
				this.CreatePlayer();
			}
		}

		private void CreatePlayer()
		{
			this.generatePlayerCommand = new GeneratePlayerCommand(new GeneratePlayerRequest
			{
				LocalePreference = Service.Get<Lang>().Locale
			});
			this.generatePlayerCommand.AddSuccessCallback(new AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>.OnSuccessCallback(this.OnGeneratePlayerComplete));
			this.generatePlayerCommand.AddFailureCallback(new AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>.OnFailureCallback(this.OnGeneratePlayerFailure));
			Service.Get<ServerAPI>().Async(this.generatePlayerCommand);
		}

		private static string GenerateRequestToken(string playerId, string secret)
		{
			Serializer serializer = new Serializer();
			serializer.AddString("userId", playerId);
			serializer.Add<long>("expires", GameUtils.GetNowJavaEpochTime());
			string text = serializer.End().ToString();
			byte[] value = CryptographyUtils.ComputeHmacHash("HmacSHA256", secret, text);
			string text2 = BitConverter.ToString(value).Replace("-", string.Empty);
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(text2 + "." + text));
		}

		private void OnGeneratePlayerComplete(GeneratePlayerResponse response, object cookie)
		{
			PlayerPrefs.SetString("prefPlayerId", response.PlayerId);
			PlayerPrefs.SetString("prefPlayerSecret", response.Secret);
			PlayerPrefs.Save();
			this.GetAuthToken();
		}

		private void OnGeneratePlayerFailure(uint status, object cookie)
		{
			Service.Get<StaRTSLogger>().Error("Error generating new player after auth fail.");
		}

		private void MessageHandler(Dictionary<string, object> messages)
		{
			foreach (KeyValuePair<string, object> current in messages)
			{
				bool flag;
				IMessage message = MessageFactory.CreateMessage(current.get_Key(), current.get_Value(), out flag);
				if (message != null)
				{
					Service.Get<EventManager>().SendEvent(message.MessageEventId, message.MessageCookie);
				}
				else if (!flag)
				{
					Service.Get<StaRTSLogger>().WarnFormat("Got unrecognized message from server of type {0}", new object[]
					{
						current.get_Key()
					});
				}
			}
		}

		private void OnDesync(string message, uint status, ProtocolResult result)
		{
			ServerAPI serverAPI = Service.Get<ServerAPI>();
			if (result == ProtocolResult.Match)
			{
				string title = null;
				Lang lang = Service.Get<Lang>();
				if (this.hasSecondary && serverAPI.GetDispatcher().Url.StartsWith("https://n7-startswin-integration-app-active.playdom.com:443/app") && message.StartsWith(lang.Get(LangUtils.DESYNC_BATCH_MAX_RETRY, new object[0])))
				{
					title = lang.Get("UPDATE_COMING_TITLE", new object[0]);
					message = lang.Get("UPDATE_COMING_MESSAGE", new object[0]);
					Service.Get<StaRTSLogger>().Warn("Maintenance message shown due to client update before server deploy");
				}
				if (status != 1999u)
				{
					AlertScreen.ShowModalWithBI(true, title, message, message);
				}
				this.KillStartup();
				return;
			}
			if (this.isHandlingMismatch)
			{
				return;
			}
			this.isHandlingMismatch = true;
			this.protocolResult = result;
			if (result == ProtocolResult.Higher && !serverAPI.GetDispatcher().Url.StartsWith("https://n7-startswin-integration-app-active.playdom.com:443/app") && this.hasSecondary && this.isAttemptingAuth)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Trying secondary API due to higher protocol", new object[0]);
				if (this.getAuthTokenCommand != null)
				{
					this.getAuthTokenCommand.RemoveAllCallbacks();
				}
				if (this.generatePlayerCommand != null)
				{
					this.generatePlayerCommand.RemoveAllCallbacks();
				}
				serverAPI.SetDispatcher("https://n7-startswin-integration-app-active.playdom.com:443/app", Service.Get<Engine>());
				this.GetOrCreatePlayer();
				return;
			}
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			if (currentState is BattleStartState || currentState is BattlePlayState || currentState is BattleEndState)
			{
				this.isHandlingMismatch = false;
				return;
			}
			if (result != ProtocolResult.Higher)
			{
				if (result == ProtocolResult.Lower)
				{
					Service.Get<StaRTSLogger>().Warn(message);
				}
			}
			else
			{
				Service.Get<StaRTSLogger>().Error(message);
			}
			this.DisplayUpdateWindow();
		}

		private void KillStartup()
		{
			ApplicationLoadState applicationLoadState = Service.Get<GameStateMachine>().CurrentState as ApplicationLoadState;
			if (applicationLoadState != null)
			{
				applicationLoadState.KillStartup();
			}
		}

		private void DisplayUpdateWindow()
		{
			Lang lang = Service.Get<Lang>();
			ProtocolResult protocolResult = this.protocolResult;
			if (protocolResult != ProtocolResult.Higher)
			{
				if (protocolResult == ProtocolResult.Lower)
				{
					string title = lang.Get("FORCED_UPDATE_TITLE", new object[0]);
					string message = lang.Get("FORCED_UPDATE_MESSAGE", new object[0]);
					UpdateClientScreen.ShowModal(title, message, new OnScreenModalResult(this.OnUpdateWindowClosed), null);
				}
			}
			else
			{
				string title = lang.Get("UPDATE_COMING_TITLE", new object[0]);
				string message = lang.Get("UPDATE_COMING_MESSAGE", new object[0]);
				AlertScreen.ShowModal(true, title, message, null, null);
			}
			this.KillStartup();
		}

		private void OnUpdateWindowClosed(object result, object cookie)
		{
			Application.OpenURL("ms-windows-store:PDP?PFN=Disney.StarWarsCommander_6rarf9sa4v8jt");
		}

		private void CompleteTask()
		{
			this.isAttemptingAuth = false;
			base.Complete();
		}

		protected internal ServerStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).CompleteTask();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).CreatePlayer();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).DisplayUpdateWindow();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).FigureOutAuth();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ServerStartupTask.GenerateRequestToken(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).GetAuthToken();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).GetOrCreatePlayer();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).InitializeCurrentPlayer();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).KillStartup();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).MessageHandler((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnFigureOutAuthReponse((AuthTokenIsPlayerIdResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnGeneratePlayerComplete((GeneratePlayerResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnGetAuthTokenComplete((GetAuthTokenResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnUpdateWindowClosed(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ServerStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}

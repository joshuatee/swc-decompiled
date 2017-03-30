using StaRTS.Audio;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StaRTS.Externals.Tapjoy
{
	public class TapjoyManager : MonoBehaviour
	{
		private static bool mOpeningFullScreenAd;

		private static bool offerWallOpen;

		private static bool isTapJoyEnabled;

		private static bool isUserIDSet;

		private static TapjoyManager m_instance;

		public static bool DidConnectSucceed
		{
			get;
			private set;
		}

		public static bool OpeningFullScreenAd
		{
			get
			{
				return TapjoyManager.mOpeningFullScreenAd;
			}
		}

		public static TapjoyManager Instance
		{
			get
			{
				return TapjoyManager.m_instance;
			}
		}

		private void Awake()
		{
			TapjoyManager.m_instance = this;
			if (base.gameObject.GetComponent<TapjoyPlugin>() == null)
			{
				base.gameObject.AddComponent<TapjoyPlugin>();
			}
		}

		public void ShowOffers()
		{
			if (!TapjoyManager.isTapJoyEnabled || TapjoyManager.offerWallOpen)
			{
				return;
			}
			if (!TapjoyManager.DidConnectSucceed)
			{
				return;
			}
			if (!TapjoyManager.isUserIDSet)
			{
				string playerId = Service.Get<CurrentPlayer>().PlayerId;
				if (string.IsNullOrEmpty(playerId))
				{
					return;
				}
				this.SetUserId();
			}
			if (Service.IsSet<AudioManager>())
			{
				Service.Get<AudioManager>().ToggleAllSounds(false);
			}
			TapjoyManager.offerWallOpen = true;
			if (Service.IsSet<GameIdleController>())
			{
				Service.Get<GameIdleController>().Enabled = false;
			}
			TapjoyPlugin.ShowOffers();
		}

		public void DisableTapjoy()
		{
			TapjoyManager.isTapJoyEnabled = false;
			this.UnRegisterCallbacks();
		}

		public void EnableTapjoy()
		{
			TapjoyManager.isTapJoyEnabled = true;
			this.RegisterCallbacks();
		}

		public static bool IsEnabled()
		{
			return TapjoyManager.isTapJoyEnabled;
		}

		public static bool IsOfferWallOpen()
		{
			return TapjoyManager.offerWallOpen;
		}

		protected virtual void Start()
		{
			TapjoyPlugin.EnableLogging(true);
			TapjoyManager.offerWallOpen = false;
			TapjoyManager.isUserIDSet = false;
			AndroidJNI.AttachCurrentThread();
			TapjoyPlugin.RequestTapjoyConnect("64348e6c-f353-41f8-8929-34564747f2d3", "CbTh6tz00S9ugMuDSbE1", new Dictionary<string, object>
			{
				{
					"disable_persistent_ids",
					true
				}
			});
		}

		private void RegisterCallbacks()
		{
			TapjoyPlugin.connectCallSucceeded += new Action(this.HandleTapjoyConnectSuccess);
			TapjoyPlugin.connectCallFailed += new Action(this.HandleTapjoyConnectFailed);
			TapjoyPlugin.getTapPointsSucceeded += new Action<int>(this.HandleGetTapPointsSucceeded);
			TapjoyPlugin.getTapPointsFailed += new Action(this.HandleGetTapPointsFailed);
			TapjoyPlugin.spendTapPointsSucceeded += new Action<int>(this.HandleSpendTapPointsSucceeded);
			TapjoyPlugin.spendTapPointsFailed += new Action(this.HandleSpendTapPointsFailed);
			TapjoyPlugin.awardTapPointsSucceeded += new Action(this.HandleAwardTapPointsSucceeded);
			TapjoyPlugin.awardTapPointsFailed += new Action(this.HandleAwardTapPointsFailed);
			TapjoyPlugin.tapPointsEarned += new Action<int>(this.HandleTapPointsEarned);
			TapjoyPlugin.getDisplayAdSucceeded += new Action(this.HandleGetDisplayAdSucceeded);
			TapjoyPlugin.getDisplayAdFailed += new Action(this.HandleGetDisplayAdFailed);
			TapjoyPlugin.videoAdStarted += new Action(this.HandleVideoAdStarted);
			TapjoyPlugin.videoAdFailed += new Action(this.HandleVideoAdFailed);
			TapjoyPlugin.videoAdCompleted += new Action(this.HandleVideoAdCompleted);
			TapjoyPlugin.viewOpened += new Action<TapjoyViewType>(this.HandleViewOpened);
			TapjoyPlugin.viewClosed += new Action<TapjoyViewType>(this.HandleViewClosed);
			TapjoyPlugin.showOffersFailed += new Action(this.HandleShowOffersFailed);
		}

		private void OnDisable()
		{
			this.UnRegisterCallbacks();
		}

		private void UnRegisterCallbacks()
		{
			TapjoyPlugin.connectCallSucceeded -= new Action(this.HandleTapjoyConnectSuccess);
			TapjoyPlugin.connectCallFailed -= new Action(this.HandleTapjoyConnectFailed);
			TapjoyPlugin.getTapPointsSucceeded -= new Action<int>(this.HandleGetTapPointsSucceeded);
			TapjoyPlugin.getTapPointsFailed -= new Action(this.HandleGetTapPointsFailed);
			TapjoyPlugin.spendTapPointsSucceeded -= new Action<int>(this.HandleSpendTapPointsSucceeded);
			TapjoyPlugin.spendTapPointsFailed -= new Action(this.HandleSpendTapPointsFailed);
			TapjoyPlugin.awardTapPointsSucceeded -= new Action(this.HandleAwardTapPointsSucceeded);
			TapjoyPlugin.awardTapPointsFailed -= new Action(this.HandleAwardTapPointsFailed);
			TapjoyPlugin.tapPointsEarned -= new Action<int>(this.HandleTapPointsEarned);
			TapjoyPlugin.getFullScreenAdSucceeded -= new Action(this.HandleGetFullScreenAdSucceeded);
			TapjoyPlugin.getFullScreenAdFailed -= new Action(this.HandleGetFullScreenAdFailed);
			TapjoyPlugin.getDisplayAdSucceeded -= new Action(this.HandleGetDisplayAdSucceeded);
			TapjoyPlugin.getDisplayAdFailed -= new Action(this.HandleGetDisplayAdFailed);
			TapjoyPlugin.videoAdStarted -= new Action(this.HandleVideoAdStarted);
			TapjoyPlugin.videoAdFailed -= new Action(this.HandleVideoAdFailed);
			TapjoyPlugin.videoAdCompleted -= new Action(this.HandleVideoAdCompleted);
			TapjoyPlugin.viewOpened -= new Action<TapjoyViewType>(this.HandleViewOpened);
			TapjoyPlugin.viewClosed -= new Action<TapjoyViewType>(this.HandleViewClosed);
			TapjoyPlugin.showOffersFailed -= new Action(this.HandleShowOffersFailed);
		}

		public void HandleTapjoyConnectSuccess()
		{
			TapjoyManager.DidConnectSucceed = true;
			this.SetUserId();
		}

		public void HandleTapjoyConnectFailed()
		{
			TapjoyManager.DidConnectSucceed = false;
			Service.Get<Logger>().Error("Tapjoy Connect Failed");
		}

		private void HandleGetTapPointsSucceeded(int points)
		{
		}

		public void HandleGetTapPointsFailed()
		{
		}

		public void HandleSpendTapPointsSucceeded(int points)
		{
		}

		public void HandleSpendTapPointsFailed()
		{
		}

		public void HandleAwardTapPointsSucceeded()
		{
		}

		public void HandleAwardTapPointsFailed()
		{
		}

		public void HandleTapPointsEarned(int points)
		{
			TapjoyPlugin.ShowDefaultEarnedCurrencyAlert();
		}

		public void HandleGetFullScreenAdSucceeded()
		{
		}

		public void HandleGetFullScreenAdFailed()
		{
		}

		public void HandleGetDisplayAdSucceeded()
		{
			if (!TapjoyManager.mOpeningFullScreenAd)
			{
				TapjoyPlugin.ShowDisplayAd();
			}
		}

		public void HandleGetDisplayAdFailed()
		{
		}

		public void HandleVideoAdStarted()
		{
		}

		public void HandleVideoAdFailed()
		{
		}

		public void HandleVideoAdCompleted()
		{
		}

		public void HandleViewOpened(TapjoyViewType viewType)
		{
			TapjoyManager.mOpeningFullScreenAd = true;
			TapjoyManager.offerWallOpen = true;
		}

		public void HandleViewClosed(TapjoyViewType viewType)
		{
			TapjoyManager.mOpeningFullScreenAd = false;
			if (viewType == TapjoyViewType.OFFERWALL)
			{
				TapjoyManager.offerWallOpen = false;
				this.HandleOffersClosed();
			}
		}

		private void HandleOffersClosed()
		{
			if (Service.IsSet<GameIdleController>())
			{
				Service.Get<GameIdleController>().Enabled = true;
			}
			if (Service.IsSet<AudioManager>())
			{
				Service.Get<AudioManager>().ToggleAllSounds(true);
			}
			this.CheckTapjoyRewards();
		}

		public void CheckTapjoyRewards()
		{
			if (!Service.IsSet<CurrentPlayer>() || !Service.IsSet<ServerAPI>())
			{
				return;
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			TapjoyPlayerSyncCommand command = new TapjoyPlayerSyncCommand(new PlayerIdRequest
			{
				PlayerId = currentPlayer.PlayerId
			});
			Service.Get<ServerAPI>().Enqueue(command);
		}

		public void HandleShowOffersFailed()
		{
			if (Service.IsSet<Logger>())
			{
				Service.Get<Logger>().Warn("HandleShowOffersFailed");
			}
		}

		private void SetUserId()
		{
			if (!Service.IsSet<CurrentPlayer>())
			{
				return;
			}
			string playerId = Service.Get<CurrentPlayer>().PlayerId;
			if (!string.IsNullOrEmpty(playerId))
			{
				TapjoyPlugin.SetUserID(Service.Get<CurrentPlayer>().PlayerId);
				TapjoyManager.isUserIDSet = true;
			}
		}
	}
}

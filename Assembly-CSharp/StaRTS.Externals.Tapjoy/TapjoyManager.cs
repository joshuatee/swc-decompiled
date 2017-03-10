using StaRTS.Audio;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.Tapjoy
{
	public class TapjoyManager : MonoBehaviour, IUnitySerializable
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
			Service.Get<StaRTSLogger>().Error("Tapjoy Connect Failed");
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
			if (Service.IsSet<StaRTSLogger>())
			{
				Service.Get<StaRTSLogger>().Warn("HandleShowOffersFailed");
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

		public TapjoyManager()
		{
		}

		public override void Unity_Serialize(int depth)
		{
		}

		public override void Unity_Deserialize(int depth)
		{
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public override void Unity_NamedSerialize(int depth)
		{
		}

		public override void Unity_NamedDeserialize(int depth)
		{
		}

		protected internal TapjoyManager(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).Awake();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).CheckTapjoyRewards();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).DisableTapjoy();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).EnableTapjoy();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TapjoyManager.DidConnectSucceed);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TapjoyManager.Instance);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TapjoyManager.OpeningFullScreenAd);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleAwardTapPointsFailed();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleAwardTapPointsSucceeded();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleGetDisplayAdFailed();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleGetDisplayAdSucceeded();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleGetFullScreenAdFailed();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleGetFullScreenAdSucceeded();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleGetTapPointsFailed();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleGetTapPointsSucceeded(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleOffersClosed();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleShowOffersFailed();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleSpendTapPointsFailed();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleSpendTapPointsSucceeded(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleTapjoyConnectFailed();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleTapjoyConnectSuccess();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleTapPointsEarned(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleVideoAdCompleted();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleVideoAdFailed();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleVideoAdStarted();
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleViewClosed((TapjoyViewType)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).HandleViewOpened((TapjoyViewType)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TapjoyManager.IsEnabled());
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TapjoyManager.IsOfferWallOpen());
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).RegisterCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			TapjoyManager.DidConnectSucceed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).SetUserId();
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).ShowOffers();
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((TapjoyManager)GCHandledObjects.GCHandleToObject(instance)).UnRegisterCallbacks();
			return -1L;
		}
	}
}

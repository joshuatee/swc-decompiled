using StaRTS.Main.Views.UX.Elements;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Squads
{
	public class ChatItemElements
	{
		public const string CI_PRIMARY_BTN = "BtnPrimary";

		public const string CI_PRIMARY_BTN_LABEL = "LabelBtnPrimary";

		public const string CI_SECONDARY_BTN = "BtnSecondary";

		public const string CI_SECONDARY_BTN_LABEL = "LabelBtnSecondary";

		public const string CI_DONATE_PBAR = "PbarDonate";

		public const string CI_DONATE_PBAR_LABEL = "LabelPbarDonate";

		public const string CI_MESSAGE_BG = "SpritePlayerMessage";

		public const string CI_REPLAY_SECTION = "Replay";

		public const string CI_RP_DAMAGE_LABEL = "LabelDamage";

		public const string CI_RP_OPPONENT_LABEL = "LabelOpponentName";

		public const string CI_RP_TYPE_LABEL = "LabelReplayType";

		public const string CI_RP_STAR_1 = "SpriteStar1";

		public const string CI_RP_STAR_2 = "SpriteStar2";

		public const string CI_RP_STAR_3 = "SpriteStar3";

		public const string CI_PLAYER_MESSAGE_LABEL = "LabelPlayerMessage";

		public const string CI_PLAYER_NAME_LABEL = "LabelPlayerName";

		public const string CI_PLAYER_ROLE_LABEL = "LabelPlayerRole";

		public const string CI_TIMESTAMP_LABEL = "LabelTimeStamp";

		public const string CI_STATUS_LABEL = "LabelSquadUpdate";

		public const string CI_MESSSAGE_ARROW_SPRITE = "SpritePlayerMessageArrow";

		public const string CI_VIDEO_PLAYER_MESSAGE_LABEL = "MakerLabelPlayerMessage";

		public const string CI_VIDEO_PLAYER_NAME_LABEL = "MakerLabelPlayerNameChatTop";

		public const string CI_VIDEO_PLAYER_ROLE_LABEL = "MakerLabelPlayerRoleChatTop";

		public const string CI_VIDEO_TIMESTAMP_LABEL = "MakerLabelTimeStampChatTop";

		public const string CI_VIDEO_STATUS_LABEL = "MakerLabelSquadUpdateChatTop";

		public const string CI_VIDEO_MESSAGE_BG = "MakerSpritePlayerMessage";

		public const string CI_LABLE_ITEM_REPLAYMEDALS = "LabelReplayMedals";

		public const string CI_REWARD_LABEL = "LabelDonateReward";

		public const string CI_CONTAINER_CHAT = "ContainerChat";

		public const string CI_CONTAINER_CHAT_WAR = "ContainerChatWar";

		public const string CI_SPRITE_WAR_ICON = "SpriteWarIcon";

		public const string CI_WAR_REQUEST_TEXTURE = "TextureWarRequest";

		public const string WAR_REQUEST_TEXTURE_NAME = "squadwars_chatrequest_row";

		public UXElement parent;

		public UXButton PrimaryButton
		{
			get;
			set;
		}

		public UXLabel PrimaryButtonLabel
		{
			get;
			set;
		}

		public UXButton SecondaryButton
		{
			get;
			set;
		}

		public UXLabel SecondaryButtonLabel
		{
			get;
			set;
		}

		public UXSlider DonateProgBar
		{
			get;
			set;
		}

		public UXLabel DonateProgBarLabel
		{
			get;
			set;
		}

		public UXLabel DonateRewardLabel
		{
			get;
			set;
		}

		public UXSprite MessageBG
		{
			get;
			set;
		}

		public UXElement ReplayParent
		{
			get;
			set;
		}

		public UXLabel ReplayTypeLabel
		{
			get;
			set;
		}

		public UXLabel ReplayDamageLabel
		{
			get;
			set;
		}

		public UXLabel ReplayOpponentNameLabel
		{
			get;
			set;
		}

		public UXLabel ReplayMedals
		{
			get;
			set;
		}

		public UXSprite ReplayStar1
		{
			get;
			set;
		}

		public UXSprite ReplayStar2
		{
			get;
			set;
		}

		public UXSprite ReplayStar3
		{
			get;
			set;
		}

		public UXLabel PlayerMessageLabel
		{
			get;
			set;
		}

		public UXLabel PlayerNameLabel
		{
			get;
			set;
		}

		public UXLabel PlayerRoleLabel
		{
			get;
			set;
		}

		public UXLabel LabelSquadUpdate
		{
			get;
			set;
		}

		public UXSprite SpriteMessageArrow
		{
			get;
			set;
		}

		public UXLabel TimestampLabel
		{
			get;
			set;
		}

		public UXElement ContainerChat
		{
			get;
			set;
		}

		public UXElement ContainerChatWar
		{
			get;
			set;
		}

		public UXSprite SpriteWarIcon
		{
			get;
			set;
		}

		public UXTexture WarRequestTexture
		{
			get;
			set;
		}

		public ChatItemElements()
		{
		}

		protected internal ChatItemElements(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ContainerChat);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ContainerChatWar);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).DonateProgBar);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).DonateProgBarLabel);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).DonateRewardLabel);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).LabelSquadUpdate);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).MessageBG);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).PlayerMessageLabel);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).PlayerNameLabel);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).PlayerRoleLabel);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).PrimaryButton);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).PrimaryButtonLabel);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayDamageLabel);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayMedals);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayOpponentNameLabel);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayParent);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayStar1);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayStar2);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayStar3);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayTypeLabel);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).SecondaryButton);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).SecondaryButtonLabel);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).SpriteMessageArrow);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).SpriteWarIcon);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).TimestampLabel);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).WarRequestTexture);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ContainerChat = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ContainerChatWar = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).DonateProgBar = (UXSlider)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).DonateProgBarLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).DonateRewardLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).LabelSquadUpdate = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).MessageBG = (UXSprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).PlayerMessageLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).PlayerNameLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).PlayerRoleLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).PrimaryButton = (UXButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).PrimaryButtonLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayDamageLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayMedals = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayOpponentNameLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayParent = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayStar1 = (UXSprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayStar2 = (UXSprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayStar3 = (UXSprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).ReplayTypeLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).SecondaryButton = (UXButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).SecondaryButtonLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).SpriteMessageArrow = (UXSprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).SpriteWarIcon = (UXSprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).TimestampLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((ChatItemElements)GCHandledObjects.GCHandleToObject(instance)).WarRequestTexture = (UXTexture)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

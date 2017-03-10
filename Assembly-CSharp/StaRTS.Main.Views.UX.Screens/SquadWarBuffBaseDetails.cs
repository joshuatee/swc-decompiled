using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class SquadWarBuffBaseDetails : ClosableScreen
	{
		private const string WAR_BOARD_BUFF_BASE_TITLE = "WAR_BOARD_BUFF_BASE_TITLE";

		private const string WAR_BOARD_BUFF_BASE_LEVEL = "WAR_BOARD_BUFF_BASE_LEVEL";

		private const string WAR_BOARD_BUFF_BASE_CAPTURE_REQUIREMENT = "WAR_BOARD_BUFF_BASE_CAPTURE_REQUIREMENT";

		private const string BUFF_BASE_PRODUCES = "BUFF_BASE_PRODUCES";

		private const string BUFF_BASE_UNOWNED_FACTION_NAME = "BUFF_BASE_UNOWNED_FACTION_NAME";

		private const string SCOUT = "SCOUT";

		private const string PLANET_LOCKED_REQUIREMENT = "PLANET_LOCKED_REQUIREMENT";

		private const string BTN_PLAYER_PREV = "BtnPlayerPrev";

		private const string BTN_PLAYER_NEXT = "BtnPlayerNext";

		private const string LABEL_BUFF_BASE_NAME = "LabelBuffBaseName";

		private const string LABEL_BUFF_BASE_LEVEL = "LabelBuffBaseLevel";

		private const string LABEL_CAPTURE_REQUIREMENT = "LabelCaptureRequirement";

		private const string SPRITE_FACTION_ICON = "SpriteFactionIcon";

		private const string SPRITE_BUFF_BASE_ICON = "SpriteBuffBaseIcon";

		private const string LABEL_PRODUCES = "LabelProduces";

		private const string LABEL_BUFF_DESCRIPTION = "LabelBuffDescription";

		private const string SPRITE_BUFF_ICON = "SpriteBuffIcon";

		private const string BTN_SCOUT = "BtnScout";

		private const string LABEL_SCOUT = "LabelBtnScout";

		private const string TEXTURE_BASE_DETAIL = "TextureBaseDetails";

		private const string TEXTURE_BASE_DETAIL_TFA = "squadwars_basedetails_tfa";

		private const string TEXTURE_BASE_DETAIL_HOTH = "squadwars_basedetails_hoth";

		private const string TEXTURE_BASE_DETAIL_YAVIN = "squadwars_basedetails_yavin";

		private const string TEXTURE_BASE_DETAIL_ERKIT = "squadwars_basedetails_erkit";

		private const string TEXTURE_BASE_DETAIL_DANDORAN = "squadwars_basedetails_dandoran";

		private const string TEXTURE_BASE_DETAIL_TATOOINE = "squadwars_basedetails_tatooine";

		private const string LABEL_PLANET_LOCKED = "LabelPlanetLocked";

		private UXButton btnPlayerPrev;

		private UXButton btnPlayerNext;

		private UXLabel labelBuffBaseName;

		private UXLabel labelBuffBaseLevel;

		private UXLabel labelCaptureRequirement;

		private UXSprite spriteFactionIcon;

		private UXSprite spriteBuffBaseIcon;

		private UXLabel labelProduces;

		private UXLabel labelBuffDescription;

		private UXSprite spriteBuffIcon;

		private UXButton btnScout;

		private UXLabel labelScout;

		private UXTexture textureBaseDetail;

		private UXLabel labelLocked;

		private SquadWarBuffBaseData buffBaseData;

		public SquadWarBuffBaseDetails(SquadWarBuffBaseData buffBaseData) : base("gui_squadwar_buffbasedetails")
		{
			this.buffBaseData = buffBaseData;
		}

		protected override void OnScreenLoaded()
		{
			this.InitButtons();
			this.btnPlayerPrev = base.GetElement<UXButton>("BtnPlayerPrev");
			this.btnPlayerNext = base.GetElement<UXButton>("BtnPlayerNext");
			this.labelBuffBaseName = base.GetElement<UXLabel>("LabelBuffBaseName");
			this.labelBuffBaseLevel = base.GetElement<UXLabel>("LabelBuffBaseLevel");
			this.labelCaptureRequirement = base.GetElement<UXLabel>("LabelCaptureRequirement");
			this.spriteFactionIcon = base.GetElement<UXSprite>("SpriteFactionIcon");
			this.spriteBuffBaseIcon = base.GetElement<UXSprite>("SpriteBuffBaseIcon");
			this.labelProduces = base.GetElement<UXLabel>("LabelProduces");
			this.labelBuffDescription = base.GetElement<UXLabel>("LabelBuffDescription");
			this.spriteBuffIcon = base.GetElement<UXSprite>("SpriteBuffIcon");
			this.btnScout = base.GetElement<UXButton>("BtnScout");
			this.labelScout = base.GetElement<UXLabel>("LabelBtnScout");
			this.textureBaseDetail = base.GetElement<UXTexture>("TextureBaseDetails");
			this.labelLocked = base.GetElement<UXLabel>("LabelPlanetLocked");
			this.btnPlayerPrev.OnClicked = new UXButtonClickedDelegate(this.OnPlayerButtonPrevious);
			this.btnPlayerNext.OnClicked = new UXButtonClickedDelegate(this.OnPlayerButtonNext);
			this.btnScout.OnClicked = new UXButtonClickedDelegate(this.OnScoutButton);
			this.labelScout.Text = this.lang.Get("SCOUT", new object[0]);
			this.Refresh();
		}

		private void Refresh()
		{
			WarBuffVO warBuffVO = Service.Get<IDataController>().Get<WarBuffVO>(this.buffBaseData.BuffBaseId);
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			SquadWarSquadData squadData = warManager.GetSquadData(this.buffBaseData.OwnerId);
			FactionType factionType = FactionType.Neutral;
			string text = this.buffBaseData.GetDisplayBaseLevel().ToString();
			if (squadData != null)
			{
				factionType = squadData.Faction;
			}
			string uid;
			string text2;
			if (factionType == FactionType.Empire)
			{
				uid = warBuffVO.MasterEmpireBuildingUid;
				text2 = LangUtils.GetFactionName(factionType);
			}
			else if (factionType == FactionType.Rebel)
			{
				uid = warBuffVO.MasterRebelBuildingUid;
				text2 = LangUtils.GetFactionName(factionType);
			}
			else
			{
				uid = warBuffVO.MasterNeutralBuildingUid;
				text2 = this.lang.Get("BUFF_BASE_UNOWNED_FACTION_NAME", new object[0]);
			}
			this.spriteFactionIcon.SpriteName = UXUtils.GetIconNameFromFactionType(factionType);
			this.labelBuffBaseName.Text = this.lang.Get("WAR_BOARD_BUFF_BASE_TITLE", new object[]
			{
				this.lang.Get(warBuffVO.BuffBaseName, new object[0]),
				LangUtils.GetPlanetDisplayName(warBuffVO.PlanetId)
			});
			this.labelBuffBaseLevel.Text = this.lang.Get("WAR_BOARD_BUFF_BASE_LEVEL", new object[]
			{
				text,
				text2
			});
			this.labelCaptureRequirement.Text = this.lang.Get("WAR_BOARD_BUFF_BASE_CAPTURE_REQUIREMENT", new object[0]);
			this.labelProduces.Text = this.lang.Get("BUFF_BASE_PRODUCES", new object[0]);
			this.labelBuffDescription.Text = this.lang.Get(warBuffVO.BuffStringDesc, new object[0]);
			this.labelLocked.Text = this.lang.Get("PLANET_LOCKED_REQUIREMENT", new object[0]);
			this.labelLocked.Visible = !Service.Get<CurrentPlayer>().IsPlanetUnlocked(warBuffVO.PlanetId);
			TextureVO textureVO = null;
			IDataController dataController = Service.Get<IDataController>();
			if (warBuffVO.PlanetId == GameConstants.TATOOINE_PLANET_UID)
			{
				textureVO = dataController.GetOptional<TextureVO>("squadwars_basedetails_tatooine");
			}
			else if (warBuffVO.PlanetId == GameConstants.YAVIN_PLANET_UID)
			{
				textureVO = dataController.GetOptional<TextureVO>("squadwars_basedetails_yavin");
			}
			else if (warBuffVO.PlanetId == GameConstants.DANDORAN_PLANET_UID)
			{
				textureVO = dataController.GetOptional<TextureVO>("squadwars_basedetails_dandoran");
			}
			else if (warBuffVO.PlanetId == GameConstants.ERKIT_PLANET_UID)
			{
				textureVO = dataController.GetOptional<TextureVO>("squadwars_basedetails_erkit");
			}
			else if (warBuffVO.PlanetId == GameConstants.TFA_PLANET_UID)
			{
				textureVO = dataController.GetOptional<TextureVO>("squadwars_basedetails_tfa");
			}
			else if (warBuffVO.PlanetId == GameConstants.HOTH_PLANET_UID)
			{
				textureVO = dataController.GetOptional<TextureVO>("squadwars_basedetails_hoth");
			}
			if (textureVO != null)
			{
				this.textureBaseDetail.LoadTexture(textureVO.AssetName);
			}
			BuildingTypeVO data = Service.Get<IDataController>().Get<BuildingTypeVO>(uid);
			UXUtils.SetupGeometryForIcon(this.spriteBuffBaseIcon, data);
			this.spriteBuffIcon.SpriteName = warBuffVO.BuffIcon;
			string empty = string.Empty;
			if (warManager.CanScoutBuffBase(this.buffBaseData, ref empty))
			{
				this.btnScout.VisuallyEnableButton();
				this.labelScout.TextColor = this.labelScout.OriginalTextColor;
				return;
			}
			this.btnScout.VisuallyDisableButton();
			this.labelScout.TextColor = UXUtils.COLOR_LABEL_DISABLED;
		}

		private void OnPlayerButtonNext(UXButton next)
		{
			this.SwitchToAnotherBuffBase(1);
		}

		private void OnPlayerButtonPrevious(UXButton previous)
		{
			this.SwitchToAnotherBuffBase(-1);
		}

		private void OnScoutButton(UXButton button)
		{
			if (this.buffBaseData != null)
			{
				SquadWarManager warManager = Service.Get<SquadController>().WarManager;
				string empty = string.Empty;
				if (warManager.CanScoutBuffBase(this.buffBaseData, ref empty))
				{
					string buffBaseId = this.buffBaseData.BuffBaseId;
					if (Service.Get<SquadController>().WarManager.ScoutBuffBase(buffBaseId))
					{
						this.Close(null);
						return;
					}
				}
				else
				{
					Service.Get<UXController>().MiscElementsManager.ShowPlayerInstructions(empty);
				}
			}
		}

		private void SwitchToAnotherBuffBase(int relativeIndex)
		{
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			List<SquadWarBuffBaseData> buffBases = warManager.CurrentSquadWar.BuffBases;
			int num = buffBases.IndexOf(this.buffBaseData);
			if (num == -1 || buffBases.Count == 0)
			{
				return;
			}
			num += relativeIndex;
			if (num >= buffBases.Count)
			{
				num = 0;
			}
			if (num < 0)
			{
				num = buffBases.Count - 1;
			}
			this.buffBaseData = buffBases[num];
			this.Refresh();
		}

		protected internal SquadWarBuffBaseDetails(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadWarBuffBaseDetails)GCHandledObjects.GCHandleToObject(instance)).OnPlayerButtonNext((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadWarBuffBaseDetails)GCHandledObjects.GCHandleToObject(instance)).OnPlayerButtonPrevious((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadWarBuffBaseDetails)GCHandledObjects.GCHandleToObject(instance)).OnScoutButton((UXButton)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadWarBuffBaseDetails)GCHandledObjects.GCHandleToObject(instance)).OnScreenLoaded();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadWarBuffBaseDetails)GCHandledObjects.GCHandleToObject(instance)).Refresh();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadWarBuffBaseDetails)GCHandledObjects.GCHandleToObject(instance)).SwitchToAnotherBuffBase(*(int*)args);
			return -1L;
		}
	}
}

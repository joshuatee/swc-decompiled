using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Controls
{
	public class BuildingTooltip : IViewFrameTimeObserver
	{
		private const float SHARPNESS_FACTOR = 4f;

		private const string TITLE_LABEL = "LabelName";

		private const string LEVEL_LABEL = "LabelLevel";

		private const string BUBBLE_LABEL = "Label";

		private const string BUBBLE_SPRITE = "SpriteBkg";

		private const string TIME_LABEL = "LabelTime";

		private const string PROGRESS_ELEMENT = "Pbar";

		private const string ICON_ELEMENT = "SpriteImage";

		private const string SELECTED_GROUP = "Selected";

		private UXLabel titleLabel;

		private UXLabel levelLabel;

		private UXLabel bubbleLabel;

		private UXSprite bubbleSprite;

		private UXLabel timeLabel;

		private UXSlider progressSlider;

		private UXSprite iconSprite;

		private GeometryProjector iconGeometry;

		private UXElement selectedGroup;

		private readonly Color redToolTipColor;

		private float lastTimeLeft;

		private float lastTimeTotal;

		public UXElement TooltipElement
		{
			get;
			private set;
		}

		public BuildingTooltipKind Kind
		{
			get;
			private set;
		}

		public BuildingTooltip(UXFactory factory, UXElement tooltipElement, string subElementPrefix, string parentName, BuildingTooltipKind kind)
		{
			this.redToolTipColor = new Color(0.737f, 0.074f, 0.074f);
			base..ctor();
			this.TooltipElement = tooltipElement;
			this.Kind = kind;
			this.lastTimeLeft = 0f;
			this.lastTimeTotal = 0f;
			string name = UXUtils.FormatAppendedName(subElementPrefix + "LabelName", parentName);
			this.titleLabel = factory.GetElement<UXLabel>(name);
			string name2 = UXUtils.FormatAppendedName(subElementPrefix + "LabelLevel", parentName);
			this.levelLabel = factory.GetElement<UXLabel>(name2);
			string name3 = UXUtils.FormatAppendedName(subElementPrefix + "Label", parentName);
			this.bubbleLabel = factory.GetOptionalElement<UXLabel>(name3);
			string name4 = UXUtils.FormatAppendedName(subElementPrefix + "SpriteBkg", parentName);
			this.bubbleSprite = factory.GetOptionalElement<UXSprite>(name4);
			string name5 = UXUtils.FormatAppendedName(subElementPrefix + "PbarLabelTime", parentName);
			this.timeLabel = factory.GetOptionalElement<UXLabel>(name5);
			string name6 = UXUtils.FormatAppendedName(subElementPrefix + "Pbar", parentName);
			this.progressSlider = factory.GetOptionalElement<UXSlider>(name6);
			string name7 = UXUtils.FormatAppendedName(subElementPrefix + "SpriteImage", parentName);
			this.iconSprite = factory.GetOptionalElement<UXSprite>(name7);
			string name8 = UXUtils.FormatAppendedName(subElementPrefix + "Selected", parentName);
			this.selectedGroup = factory.GetOptionalElement<UXElement>(name8);
			this.SetupBGBasedOnKind();
		}

		private void SetupBGBasedOnKind()
		{
			if (this.Kind == BuildingTooltipKind.HQBubble || this.Kind == BuildingTooltipKind.ShardUpgradeBubble)
			{
				this.bubbleSprite.Color = this.redToolTipColor;
			}
		}

		public void DestroyTooltip()
		{
			this.DestroyIconGeometry();
			this.EnableViewTimeObserving(false);
			Service.Get<UXController>().MiscElementsManager.DestroyBuildingTooltip(this);
		}

		private void DestroyIconGeometry()
		{
			if (this.iconGeometry != null)
			{
				this.iconGeometry.Destroy();
				this.iconGeometry = null;
			}
		}

		public void SetTitle(string title)
		{
			this.titleLabel.Text = title;
		}

		public void SetLevel(BuildingTypeVO building)
		{
			this.levelLabel.Text = string.Empty;
		}

		public void SetBubbleText(string bubbleText)
		{
			if (this.bubbleLabel != null)
			{
				this.bubbleLabel.Visible = !string.IsNullOrEmpty(bubbleText);
				this.bubbleLabel.Text = bubbleText;
				Vector4 border = this.bubbleSprite.Border;
				this.bubbleSprite.Width = this.bubbleLabel.TextWidth + border.x + border.z;
			}
		}

		public void SetIconAsset(IUpgradeableVO iconAsset)
		{
			if (this.iconSprite != null)
			{
				bool flag = iconAsset != null;
				this.iconSprite.Visible = flag;
				this.DestroyIconGeometry();
				if (flag)
				{
					ProjectorConfig projectorConfig = ProjectorUtils.GenerateGeometryConfig(iconAsset, this.iconSprite);
					Service.Get<EventManager>().SendEvent(EventId.ButtonCreated, new GeometryTag(iconAsset, projectorConfig, Service.Get<CurrentPlayer>().ActiveArmory));
					projectorConfig.AnimPreference = AnimationPreference.AnimationPreferred;
					projectorConfig.Sharpness = 4f;
					this.iconGeometry = ProjectorUtils.GenerateProjector(projectorConfig);
				}
			}
		}

		public void SetTime(int timeLeft)
		{
			if (this.timeLabel != null)
			{
				if (timeLeft < 0)
				{
					this.timeLabel.Visible = false;
					return;
				}
				this.timeLabel.Visible = true;
				this.timeLabel.Text = GameUtils.GetTimeLabelFromSeconds(timeLeft);
			}
		}

		public void SetProgress(int timeLeft, int timeTotal)
		{
			if (this.progressSlider != null)
			{
				if (timeLeft <= 0 || timeTotal <= 0)
				{
					this.progressSlider.Visible = false;
					this.EnableViewTimeObserving(false);
					this.lastTimeLeft = 0f;
					this.lastTimeTotal = 0f;
					return;
				}
				this.progressSlider.Visible = true;
				this.EnableViewTimeObserving(true);
				this.lastTimeLeft = (float)timeLeft;
				this.lastTimeTotal = (float)timeTotal;
				this.InternalSetProgress();
			}
		}

		private void InternalSetProgress()
		{
			this.progressSlider.Value = MathUtils.NormalizeRange(this.lastTimeTotal - this.lastTimeLeft, 0f, this.lastTimeTotal);
		}

		private void EnableViewTimeObserving(bool enable)
		{
			if (enable)
			{
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
				return;
			}
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public void OnViewFrameTime(float dt)
		{
			this.lastTimeLeft -= dt;
			if (this.lastTimeLeft <= 0f)
			{
				this.lastTimeLeft = 0f;
				this.EnableViewTimeObserving(false);
			}
			this.InternalSetProgress();
		}

		public void SetSelected(bool selected)
		{
			if (this.selectedGroup != null)
			{
				this.selectedGroup.Visible = false;
			}
		}

		protected internal BuildingTooltip(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).DestroyIconGeometry();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).DestroyTooltip();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).EnableViewTimeObserving(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).Kind);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).TooltipElement);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).InternalSetProgress();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).Kind = (BuildingTooltipKind)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).TooltipElement = (UXElement)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).SetBubbleText(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).SetIconAsset((IUpgradeableVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).SetLevel((BuildingTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).SetProgress(*(int*)args, *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).SetSelected(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).SetTime(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).SetTitle(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((BuildingTooltip)GCHandledObjects.GCHandleToObject(instance)).SetupBGBasedOnKind();
			return -1L;
		}
	}
}

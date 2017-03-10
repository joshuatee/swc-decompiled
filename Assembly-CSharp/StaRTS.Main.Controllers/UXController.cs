using StaRTS.Assets;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Anchors;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class UXController
	{
		public const int WORLD_DEPTH = -1;

		public const int UI_DEPTH = 0;

		public const int BUTTON_HIGHLIGHT_DEPTH = 9999;

		public const int DEBUG_CURSOR_DEPTH = 10000;

		public const int SCREEN_SINK_DEPTH = 10001;

		public const float WORLD_DEPTH_GRANULARITY = 10000f;

		private const string WORLD_UI_PANEL_NAME = "WorldUIPanel";

		private int waitingFor;

		public HUD HUD
		{
			get;
			private set;
		}

		public IntroCameraAnimation Intro
		{
			get;
			set;
		}

		public MiscElementsManager MiscElementsManager
		{
			get;
			private set;
		}

		public UXAnchor PerformanceAnchor
		{
			get;
			private set;
		}

		public UXAnchor WorldAnchor
		{
			get;
			private set;
		}

		public GameObject WorldUIParent
		{
			get;
			private set;
		}

		public UXController()
		{
			Service.Set<UXController>(this);
			this.HUD = null;
			this.PerformanceAnchor = new UXPerformanceAnchor();
			this.WorldAnchor = new UXWorldAnchor();
			this.WorldUIParent = UnityUtils.CreateChildGameObject("WorldUIPanel", this.WorldAnchor.Root);
			UIPanel uIPanel = this.WorldUIParent.AddComponent<UIPanel>();
			uIPanel.depth = -1;
			this.waitingFor = 1;
			this.MiscElementsManager = new MiscElementsManager(new AssetsCompleteDelegate(this.OnManagerComplete), null);
			if (Service.Get<CurrentPlayer>().HasNotCompletedFirstFueStep())
			{
				this.waitingFor++;
				this.Intro = new IntroCameraAnimation(new AssetsCompleteDelegate(this.OnManagerComplete), null);
			}
		}

		private void OnManagerComplete(object cookie)
		{
			int num = this.waitingFor - 1;
			this.waitingFor = num;
			if (num == 0)
			{
				this.OnSharedElementsLoaded();
			}
		}

		private void OnSharedElementsLoaded()
		{
			this.HUD = new HUD();
			Service.Get<ScreenController>().AddScreen(this.HUD, false);
			this.HUD.Visible = false;
		}

		public void HideAll()
		{
			Service.Get<ScreenController>().HideAll();
			if (this.PerformanceAnchor != null)
			{
				this.PerformanceAnchor.Visible = false;
			}
			if (this.WorldAnchor != null)
			{
				this.WorldAnchor.Visible = false;
			}
		}

		public void RestoreVisibilityToAll()
		{
			Service.Get<ScreenController>().RestoreVisibilityToAll();
			if (this.PerformanceAnchor != null)
			{
				this.PerformanceAnchor.Visible = true;
			}
			if (this.WorldAnchor != null)
			{
				this.WorldAnchor.Visible = true;
			}
		}

		public int ComputeDepth(Vector3 worldPosition)
		{
			float num = worldPosition.x + worldPosition.z;
			float num2 = num - -276f;
			float num3 = 552f;
			return -1 - (int)(10000f * num2 / num3);
		}

		protected internal UXController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXController)GCHandledObjects.GCHandleToObject(instance)).ComputeDepth(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXController)GCHandledObjects.GCHandleToObject(instance)).HUD);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXController)GCHandledObjects.GCHandleToObject(instance)).Intro);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXController)GCHandledObjects.GCHandleToObject(instance)).MiscElementsManager);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXController)GCHandledObjects.GCHandleToObject(instance)).PerformanceAnchor);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXController)GCHandledObjects.GCHandleToObject(instance)).WorldAnchor);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXController)GCHandledObjects.GCHandleToObject(instance)).WorldUIParent);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((UXController)GCHandledObjects.GCHandleToObject(instance)).HideAll();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((UXController)GCHandledObjects.GCHandleToObject(instance)).OnManagerComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((UXController)GCHandledObjects.GCHandleToObject(instance)).OnSharedElementsLoaded();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((UXController)GCHandledObjects.GCHandleToObject(instance)).RestoreVisibilityToAll();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXController)GCHandledObjects.GCHandleToObject(instance)).HUD = (HUD)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UXController)GCHandledObjects.GCHandleToObject(instance)).Intro = (IntroCameraAnimation)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((UXController)GCHandledObjects.GCHandleToObject(instance)).MiscElementsManager = (MiscElementsManager)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((UXController)GCHandledObjects.GCHandleToObject(instance)).PerformanceAnchor = (UXAnchor)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((UXController)GCHandledObjects.GCHandleToObject(instance)).WorldAnchor = (UXAnchor)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((UXController)GCHandledObjects.GCHandleToObject(instance)).WorldUIParent = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

using StaRTS.Assets;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens
{
	public class LoadingScreen : UXFactory
	{
		private const string LOADING_LABEL = "LabelLoading";

		private const string LOADING_BAR = "PBarLoading";

		private const string LOADING_LOGO_ASIAN = "SpriteLogoAsian";

		private const string LOADING_LOGO = "SpriteLogo";

		private const string LOGO_DEFAULT = "LoadingLogo";

		private const string LOGO_RU = "SWC_Logo_Russian_B";

		private const string LOGO_CN = "SWC_Logo_Chinese_SC";

		private const string LOGO_TW = "SWC_Logo_Chinese_TC";

		private const string LOGO_KR = "SWC_Logo_Korean";

		private AssetHandle assetHandle;

		private UXSlider loadingSlider;

		private UXSprite logo;

		private UXSprite logoAsian;

		public LoadingScreen() : base(Service.Get<CameraManager>().UXSceneCamera)
		{
			base.Load(ref this.assetHandle, "gui_loading_screen", new UXFactoryLoadDelegate(this.LoadSuccess), null, null);
		}

		private void OnWipeComplete(object cookie)
		{
			this.uxCamera.Camera.enabled = false;
			base.DestroyFactory();
		}

		private void LoadSuccess(object cookie)
		{
			this.uxCamera.Camera.enabled = true;
			this.InitSlider();
			this.InitLogo();
		}

		private void InitLogo()
		{
			this.logo = base.GetElement<UXSprite>("SpriteLogo");
			this.logoAsian = base.GetElement<UXSprite>("SpriteLogoAsian");
			string locale = Service.Get<Lang>().Locale;
			if (locale == "ru_RU")
			{
				this.logo.SpriteName = "SWC_Logo_Russian_B";
				this.logo.Visible = true;
				this.logoAsian.Visible = false;
				return;
			}
			if (locale == "zh_CN")
			{
				this.logo.Visible = false;
				this.logoAsian.Visible = true;
				this.logoAsian.SpriteName = "SWC_Logo_Chinese_SC";
				return;
			}
			if (locale == "zh_TW")
			{
				this.logo.Visible = false;
				this.logoAsian.Visible = true;
				this.logoAsian.SpriteName = "SWC_Logo_Chinese_TC";
				return;
			}
			if (locale == "ko_KR")
			{
				this.logo.Visible = false;
				this.logoAsian.Visible = true;
				this.logoAsian.SpriteName = "SWC_Logo_Korean";
				return;
			}
			this.logo.SpriteName = "LoadingLogo";
			this.logo.Visible = true;
			this.logoAsian.Visible = false;
		}

		private void InitSlider()
		{
			UXLabel element = base.GetElement<UXLabel>("LabelLoading");
			element.Text = Service.Get<Lang>().Get("LOADING", new object[0]);
			this.loadingSlider = base.GetElement<UXSlider>("PBarLoading");
		}

		public override void SetupRootCollider()
		{
		}

		public override void OnDestroyElement()
		{
			if (this.assetHandle != AssetHandle.Invalid)
			{
				base.Unload(this.assetHandle, "gui_loading_screen");
				this.assetHandle = AssetHandle.Invalid;
			}
			base.OnDestroyElement();
		}

		public void Progress(float percentage, string description)
		{
			if (base.IsLoaded())
			{
				this.loadingSlider.Value = percentage * 0.01f;
			}
		}

		public void Fade()
		{
			if (Service.Get<CurrentPlayer>().HasNotCompletedFirstFueStep())
			{
				this.OnWipeComplete(null);
				return;
			}
			Service.Get<CameraManager>().WipeCamera.StartLinearWipe(WipeTransition.FromIntroToBase, 1.57079637f, new WipeCompleteDelegate(this.OnWipeComplete), null);
			Service.Get<WorldInitializer>().View.SetIsoVantage(CameraFeel.Medium);
		}

		protected internal LoadingScreen(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((LoadingScreen)GCHandledObjects.GCHandleToObject(instance)).Fade();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((LoadingScreen)GCHandledObjects.GCHandleToObject(instance)).InitLogo();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((LoadingScreen)GCHandledObjects.GCHandleToObject(instance)).InitSlider();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LoadingScreen)GCHandledObjects.GCHandleToObject(instance)).LoadSuccess(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LoadingScreen)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((LoadingScreen)GCHandledObjects.GCHandleToObject(instance)).OnWipeComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((LoadingScreen)GCHandledObjects.GCHandleToObject(instance)).Progress(*(float*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((LoadingScreen)GCHandledObjects.GCHandleToObject(instance)).SetupRootCollider();
			return -1L;
		}
	}
}

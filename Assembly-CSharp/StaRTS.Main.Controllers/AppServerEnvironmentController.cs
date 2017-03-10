using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class AppServerEnvironmentController
	{
		public const string REMOTE_PRIMARY_QA01 = "https://qa.api.disney.com/starts/qa01/v1/app";

		public const string REMOTE_PRIMARY_QA02 = "https://qa.api.disney.com/starts/qa02/v1/app";

		public const string REMOTE_PRIMARY_DEV01 = "https://int.api.disney.network/starts/dev01/v1/app";

		public const string REMOTE_PRIMARY_DEV02 = "https://int.api.disney.network/starts/dev02/v1/app";

		public const string REMOTE_PRIMARY_DEV03 = "https://int.api.disney.network/starts/dev03/v1/app";

		public const string REMOTE_PRIMARY_DEV04 = "https://int.api.disney.network/starts/dev04/v1/app";

		public const string REMOTE_PRIMARY_DEV05 = "https://int.api.disney.network/starts/dev05/v1/app";

		public const string REMOTE_PRIMARY_DEV06 = "https://int.api.disney.network/starts/dev06/v1/app";

		public const string REMOTE_PRIMARY_DEV07 = "https://int.api.disney.network/starts/dev07/v1/app";

		public const string REMOTE_PRIMARY_DEV08 = "https://int.api.disney.network/starts/dev08/v1/app";

		public const string REMOTE_SECONDARY = "https://n7-startswin-integration-app-active.playdom.com:443/app";

		public const string REMOTE_PRIMARY = "https://n7-startswin-web-active.playdom.com:443/app";

		public const string LOCAL = "http://localhost:8080/starts";

		private const string PREF_ENVIRONMENT = "Environment";

		public string Server
		{
			get;
			private set;
		}

		public AppServerEnvironmentController()
		{
			Service.Set<AppServerEnvironmentController>(this);
			this.Server = AppServerEnvironmentController.GetCompileTimeServer();
			this.OverrideEnvironment();
		}

		private static string GetCompileTimeServer()
		{
			return "https://n7-startswin-web-active.playdom.com:443/app";
		}

		public static bool IsLocalServer()
		{
			return AppServerEnvironmentController.GetCompileTimeServer() == "http://localhost:8080/starts";
		}

		private void OverrideEnvironment()
		{
			string environmentOverridePreference = this.GetEnvironmentOverridePreference();
			if (!this.IsValidEnvironmentOverride(environmentOverridePreference))
			{
				this.SetEnvironmentOverride(AppServerEnvironmentOverride.DEFAULT.ToString());
				return;
			}
			string server;
			switch (this.GetEnvironmentOverride())
			{
			case AppServerEnvironmentOverride.DEV01:
				server = "https://int.api.disney.network/starts/dev01/v1/app";
				break;
			case AppServerEnvironmentOverride.DEV02:
				server = "https://int.api.disney.network/starts/dev02/v1/app";
				break;
			case AppServerEnvironmentOverride.DEV03:
				server = "https://int.api.disney.network/starts/dev03/v1/app";
				break;
			case AppServerEnvironmentOverride.DEV04:
				server = "https://int.api.disney.network/starts/dev04/v1/app";
				break;
			case AppServerEnvironmentOverride.DEV05:
				server = "https://int.api.disney.network/starts/dev05/v1/app";
				break;
			case AppServerEnvironmentOverride.DEV06:
				server = "https://int.api.disney.network/starts/dev06/v1/app";
				break;
			case AppServerEnvironmentOverride.DEV07:
				server = "https://int.api.disney.network/starts/dev07/v1/app";
				break;
			case AppServerEnvironmentOverride.DEV08:
				server = "https://int.api.disney.network/starts/dev08/v1/app";
				break;
			case AppServerEnvironmentOverride.QA01:
				server = "https://qa.api.disney.com/starts/qa01/v1/app";
				break;
			case AppServerEnvironmentOverride.QA02:
				server = "https://qa.api.disney.com/starts/qa02/v1/app";
				break;
			default:
				server = this.Server;
				break;
			}
			this.Server = server;
		}

		private string GetEnvironmentOverridePreference()
		{
			return PlayerPrefs.GetString("Environment", AppServerEnvironmentOverride.DEFAULT.ToString());
		}

		public AppServerEnvironmentOverride GetEnvironmentOverride()
		{
			string environmentOverridePreference = this.GetEnvironmentOverridePreference();
			return AppServerEnvironmentController.GetAppServerEnvironmentOverrideFromString(environmentOverridePreference);
		}

		public void SetEnvironmentOverride(string environment)
		{
			environment = environment.ToUpper();
			if (!this.IsValidEnvironmentOverride(environment))
			{
				environment = AppServerEnvironmentOverride.DEFAULT.ToString();
			}
			PlayerPrefs.SetString("Environment", environment);
		}

		private static AppServerEnvironmentOverride GetAppServerEnvironmentOverrideFromString(string environmentOverrideString)
		{
			return (AppServerEnvironmentOverride)Enum.Parse(typeof(AppServerEnvironmentOverride), environmentOverrideString.ToUpper());
		}

		public bool IsValidEnvironmentOverride(string environment)
		{
			environment = environment.ToUpper();
			if (!Enum.IsDefined(typeof(AppServerEnvironmentOverride), environment))
			{
				return false;
			}
			AppServerEnvironmentOverride appServerEnvironmentOverrideFromString = AppServerEnvironmentController.GetAppServerEnvironmentOverrideFromString(environment);
			return appServerEnvironmentOverrideFromString == AppServerEnvironmentOverride.DEFAULT || (appServerEnvironmentOverrideFromString != AppServerEnvironmentOverride.QA01 && appServerEnvironmentOverrideFromString != AppServerEnvironmentOverride.QA02);
		}

		public bool IsEnvironmentOverrideSupported()
		{
			return true;
		}

		protected internal AppServerEnvironmentController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AppServerEnvironmentController)GCHandledObjects.GCHandleToObject(instance)).Server);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AppServerEnvironmentController.GetAppServerEnvironmentOverrideFromString(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AppServerEnvironmentController.GetCompileTimeServer());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AppServerEnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetEnvironmentOverride());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AppServerEnvironmentController)GCHandledObjects.GCHandleToObject(instance)).GetEnvironmentOverridePreference());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AppServerEnvironmentController)GCHandledObjects.GCHandleToObject(instance)).IsEnvironmentOverrideSupported());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(AppServerEnvironmentController.IsLocalServer());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AppServerEnvironmentController)GCHandledObjects.GCHandleToObject(instance)).IsValidEnvironmentOverride(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AppServerEnvironmentController)GCHandledObjects.GCHandleToObject(instance)).OverrideEnvironment();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AppServerEnvironmentController)GCHandledObjects.GCHandleToObject(instance)).Server = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AppServerEnvironmentController)GCHandledObjects.GCHandleToObject(instance)).SetEnvironmentOverride(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}

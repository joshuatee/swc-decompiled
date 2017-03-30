using StaRTS.Utils.Core;
using System;
using UnityEngine;

namespace StaRTS.Main.Controllers
{
	public class AppServerEnvironmentController
	{
		public const string REMOTE_PRIMARY = "https://n7-starts-web-active.playdom.com/j";

		public const string REMOTE_SECONDARY = "https://n7-starts-integration-app-active.playdom.com/j";

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
			return "https://n7-starts-web-active.playdom.com/j";
		}

		public static bool IsLocalServer()
		{
			return AppServerEnvironmentController.GetCompileTimeServer() == "http://localhost:8080/starts";
		}

		private void OverrideEnvironment()
		{
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
			return (AppServerEnvironmentOverride)((int)Enum.Parse(typeof(AppServerEnvironmentOverride), environmentOverrideString.ToUpper()));
		}

		public bool IsValidEnvironmentOverride(string environment)
		{
			environment = environment.ToUpper();
			return Enum.IsDefined(typeof(AppServerEnvironmentOverride), environment) && AppServerEnvironmentController.GetAppServerEnvironmentOverrideFromString(environment) == AppServerEnvironmentOverride.DEFAULT;
		}

		public bool IsEnvironmentOverrideSupported()
		{
			return false;
		}
	}
}

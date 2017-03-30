using StaRTS.Externals.Tapjoy;
using StaRTS.Main.Models;
using StaRTS.Main.Utils;
using System;
using UnityEngine;

namespace StaRTS.Main.Controllers.Startup
{
	public class TapjoyStartupTask : StartupTask
	{
		private const string TAPJOY_OBJ = "TapjoyPlugin";

		public TapjoyStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			if (GameConstants.TAPJOY_ENABLED && this.IsDeviceCountryTapjoyAuthorized())
			{
				if (GameObject.Find("TapjoyPlugin") == null)
				{
					new GameObject
					{
						name = "TapjoyPlugin"
					}.AddComponent<TapjoyManager>();
				}
				TapjoyManager.Instance.EnableTapjoy();
			}
			base.Complete();
		}

		public bool IsDeviceCountryTapjoyAuthorized()
		{
			bool flag = GameUtils.IsDeviceCountryInList(GameConstants.TAPJOY_BLACKLIST);
			return !flag;
		}
	}
}

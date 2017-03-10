using StaRTS.Main.Views.UX.Elements;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Controls
{
	public class JewelControl
	{
		public const string JEWEL_ID_CLANS = "Clans";

		public const string JEWEL_ID_LOG = "Log";

		public const string JEWEL_ID_LEADERBOARD = "Leaderboard";

		public const string JEWEL_ID_SETTINGS = "Settings";

		public const string JEWEL_ID_STORE = "Store";

		public const string JEWEL_ID_LEI = "StoreSpecial";

		public const string JEWEL_ID_BATTLE = "Battle";

		public const string JEWEL_ID_INVITES = "Invites";

		public const string JEWEL_ID_GALAXY = "Galaxy";

		public const string JEWEL_ID_HOLONET = "NewsHolo";

		public const string JEWEL_ID_WAR = "War";

		public const string JEWEL_ID_WAR_PREP = "Prep";

		public const string JEWEL_ID_PERK = "Perks";

		public const string JEWEL_ID_ACT_PERK = "ActCardPerks";

		public const string JEWEL_ID_UP_PERK = "UpCardPerks";

		public const string JEWEL_ID_CHAT = "Chat";

		public const string JEWEL_ID_SOCIAL_CHAT = "SocialChat";

		public const string JEWEL_ID_SOCIAL_WAR_LOG = "SocialWarLog";

		public const string JEWEL_ID_SOCIAL_MESSAGES = "Messages";

		public const string JEWEL_ID_SOCIAL_REQUESTS = "Requests";

		public const string JEWEL_ID_SOCIAL_REPLAYS = "Replays";

		public const string JEWEL_ID_SOCIAL_UPDATES = "Updates";

		public const string JEWEL_ID_SOCIAL_VIDEOS = "Videos";

		public const string JEWEL_ID_INVENTORY_CRATES = "Crates";

		public const string JEWEL_ID_INVENTORY_BUILDINGS = "Buildings";

		public const string JEWEL_ID_INVENTORY_TROOPS = "Troops";

		public const string JEWEL_ID_INVENTORY_CURRENCY = "Currency";

		public const string CONTAINER_PREFIX = "ContainerJewel";

		private const string LABEL_PREFIX = "LabelMessageCount";

		private const string LABEL_PREFIX_ALT = "LabelMesageCount";

		private const int MAX_JEWEL_AMOUNT = 99;

		private bool useMaxJewelAmount;

		private UXElement container;

		private UXLabel label;

		public int Value
		{
			set
			{
				this.container.Visible = (value > 0);
				string text;
				if (this.useMaxJewelAmount && value > 99)
				{
					text = 99.ToString() + "+";
				}
				else
				{
					text = value.ToString();
				}
				this.label.Text = text;
			}
		}

		public string Text
		{
			set
			{
				this.container.Visible = !string.IsNullOrEmpty(value);
				this.label.Text = value;
			}
		}

		public static JewelControl Create(UXFactory uxFactory, string name)
		{
			bool flag = name == "Clans";
			UXElement optionalElement = uxFactory.GetOptionalElement<UXElement>("ContainerJewel" + name);
			UXLabel optionalElement2 = uxFactory.GetOptionalElement<UXLabel>("LabelMessageCount" + name);
			if (optionalElement2 == null)
			{
				optionalElement2 = uxFactory.GetOptionalElement<UXLabel>("LabelMesageCount" + name);
			}
			if (optionalElement != null && optionalElement2 != null)
			{
				return new JewelControl(optionalElement, optionalElement2, flag);
			}
			return null;
		}

		private JewelControl(UXElement c, UXLabel l, bool useMaxJewelAmount)
		{
			this.container = c;
			this.label = l;
			this.useMaxJewelAmount = useMaxJewelAmount;
			this.Value = 0;
		}

		protected internal JewelControl(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(JewelControl.Create((UXFactory)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((JewelControl)GCHandledObjects.GCHandleToObject(instance)).Text = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((JewelControl)GCHandledObjects.GCHandleToObject(instance)).Value = *(int*)args;
			return -1L;
		}
	}
}

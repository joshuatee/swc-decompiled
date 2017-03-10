using StaRTS.Main.Views.UX.Elements;
using System;

namespace StaRTS.Externals.Maker
{
	public struct ListItem
	{
		public object Cookie;

		public UXElement UIItem;

		public ListItem(object cookie, UXElement uiItem)
		{
			this.Cookie = cookie;
			this.UIItem = uiItem;
		}
	}
}

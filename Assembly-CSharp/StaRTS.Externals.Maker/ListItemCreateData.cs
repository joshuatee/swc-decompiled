using StaRTS.Main.Views.UX.Controls;
using System;

namespace StaRTS.Externals.Maker
{
	public struct ListItemCreateData
	{
		public DynamicScrollingList ParentList;

		public object Cookie;

		public int Location;

		public int OldLocation;

		public ListItemCreateData(DynamicScrollingList list, object cookie, int location, int oldLocation)
		{
			this.ParentList = list;
			this.Cookie = cookie;
			this.Location = location;
			this.OldLocation = oldLocation;
		}
	}
}

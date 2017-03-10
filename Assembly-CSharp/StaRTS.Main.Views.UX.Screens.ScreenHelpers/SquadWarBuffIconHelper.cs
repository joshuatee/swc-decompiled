using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers
{
	public class SquadWarBuffIconHelper
	{
		public static void SetupBuffIcons(AbstractUXList list, string templateName, string squadId)
		{
			list.SetTemplateItem(templateName);
			list.Clear();
			IDataController dc = Service.Get<IDataController>();
			List<SquadWarBuffBaseData> buffBases = Service.Get<SquadController>().WarManager.CurrentSquadWar.BuffBases;
			int i = 0;
			int count = buffBases.Count;
			while (i < count)
			{
				SquadWarBuffBaseData squadWarBuffBaseData = buffBases[i];
				if (squadWarBuffBaseData.OwnerId == squadId)
				{
					SquadWarBuffIconHelper.AddBuffIcon(list, squadWarBuffBaseData.BuffBaseId, i, dc);
				}
				i++;
			}
			list.RepositionItems();
		}

		public static void SetupBuffIcons(AbstractUXList list, string templateName, List<string> buffBases)
		{
			list.SetTemplateItem(templateName);
			list.Clear();
			if (buffBases != null)
			{
				IDataController dc = Service.Get<IDataController>();
				int i = 0;
				int count = buffBases.Count;
				while (i < count)
				{
					SquadWarBuffIconHelper.AddBuffIcon(list, buffBases[i], i, dc);
					i++;
				}
				list.RepositionItems();
			}
		}

		private static void AddBuffIcon(AbstractUXList list, string buffBaseId, int order, IDataController dc)
		{
			WarBuffVO warBuffVO = dc.Get<WarBuffVO>(buffBaseId);
			UXElement uXElement = list.CloneTemplateItem(buffBaseId);
			UXSprite uXSprite = uXElement as UXSprite;
			if (uXSprite != null)
			{
				uXSprite.SpriteName = warBuffVO.BuffIcon;
			}
			list.AddItem(uXElement, order);
		}

		public SquadWarBuffIconHelper()
		{
		}

		protected internal SquadWarBuffIconHelper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			SquadWarBuffIconHelper.AddBuffIcon((AbstractUXList)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2), (IDataController)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			SquadWarBuffIconHelper.SetupBuffIcons((AbstractUXList)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (List<string>)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			SquadWarBuffIconHelper.SetupBuffIcons((AbstractUXList)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)));
			return -1L;
		}
	}
}

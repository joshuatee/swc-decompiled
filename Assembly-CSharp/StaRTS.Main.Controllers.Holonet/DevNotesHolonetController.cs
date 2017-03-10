using StaRTS.Externals.Manimal;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Holonet
{
	public class DevNotesHolonetController : IHolonetContoller
	{
		private const int MAX_DEVNOTES_BADGE_COUNT = 3;

		public List<DevNoteEntryVO> DevNotes;

		public HolonetControllerType ControllerType
		{
			get
			{
				return HolonetControllerType.DevNotes;
			}
		}

		public DevNotesHolonetController()
		{
			this.DevNotes = new List<DevNoteEntryVO>();
		}

		public void PrepareContent(int lastTimeViewed)
		{
			int num = 0;
			this.DevNotes.Clear();
			int serverTime = (int)Service.Get<ServerAPI>().ServerTime;
			IDataController dataController = Service.Get<IDataController>();
			this.DevNotes = new List<DevNoteEntryVO>();
			foreach (DevNoteEntryVO current in dataController.GetAll<DevNoteEntryVO>())
			{
				if (current.StartTime <= serverTime && serverTime < current.EndTime)
				{
					this.DevNotes.Add(current);
					if (current.StartTime > lastTimeViewed)
					{
						num++;
					}
				}
			}
			HolonetController holonetController = Service.Get<HolonetController>();
			this.DevNotes.Sort(new Comparison<DevNoteEntryVO>(holonetController.CompareTimestamps));
			holonetController.ContentPrepared(this, Math.Min(num, 3));
		}

		protected internal DevNotesHolonetController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DevNotesHolonetController)GCHandledObjects.GCHandleToObject(instance)).ControllerType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DevNotesHolonetController)GCHandledObjects.GCHandleToObject(instance)).PrepareContent(*(int*)args);
			return -1L;
		}
	}
}

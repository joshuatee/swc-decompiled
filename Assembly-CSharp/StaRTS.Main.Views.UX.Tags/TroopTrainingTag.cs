using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Elements;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Tags
{
	public class TroopTrainingTag : TroopUpgradeTag
	{
		public bool AutoQueuing
		{
			get;
			set;
		}

		public UXLabel QueueCountLabel
		{
			get;
			set;
		}

		public UXButton TroopButton
		{
			get;
			set;
		}

		public UXButton QueueButton
		{
			get;
			set;
		}

		public UXLabel CostLabel
		{
			get;
			set;
		}

		public UXSprite Dimmer
		{
			get;
			set;
		}

		public GeometryProjector Projector
		{
			get;
			set;
		}

		public TroopTrainingTag(IDeployableVO troop, bool reqMet) : base(troop, reqMet)
		{
		}

		protected internal TroopTrainingTag(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).AutoQueuing);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).CostLabel);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).Dimmer);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).Projector);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).QueueButton);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).QueueCountLabel);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).TroopButton);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).AutoQueuing = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).CostLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).Dimmer = (UXSprite)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).Projector = (GeometryProjector)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).QueueButton = (UXButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).QueueCountLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TroopTrainingTag)GCHandledObjects.GCHandleToObject(instance)).TroopButton = (UXButton)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

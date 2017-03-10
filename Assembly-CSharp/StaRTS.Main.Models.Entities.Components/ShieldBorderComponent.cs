using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class ShieldBorderComponent : ComponentBase
	{
		public Entity ShieldGeneratorEntity
		{
			get;
			set;
		}

		public ShieldBorderComponent()
		{
		}

		protected internal ShieldBorderComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBorderComponent)GCHandledObjects.GCHandleToObject(instance)).ShieldGeneratorEntity);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ShieldBorderComponent)GCHandledObjects.GCHandleToObject(instance)).ShieldGeneratorEntity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

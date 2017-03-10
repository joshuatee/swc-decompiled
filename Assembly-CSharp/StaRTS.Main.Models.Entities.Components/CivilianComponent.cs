using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class CivilianComponent : WalkerComponent
	{
		public CivilianTypeVO CivilianType
		{
			get;
			set;
		}

		public CivilianComponent(CivilianTypeVO civilianType) : base(civilianType.AssetName, civilianType)
		{
			this.CivilianType = civilianType;
		}

		protected internal CivilianComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CivilianComponent)GCHandledObjects.GCHandleToObject(instance)).CivilianType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CivilianComponent)GCHandledObjects.GCHandleToObject(instance)).CivilianType = (CivilianTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

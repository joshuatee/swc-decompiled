using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class WalkerComponent : AssetComponent
	{
		public ISpeedVO SpeedVO;

		public ISpeedVO OriginalSpeedVO
		{
			get;
			set;
		}

		public WalkerComponent(string assetName, ISpeedVO speedVO) : base(assetName)
		{
			this.OriginalSpeedVO = speedVO;
			this.SetVOData(speedVO);
		}

		public void SetVOData(ISpeedVO speedVO)
		{
			this.SpeedVO = speedVO;
		}

		protected internal WalkerComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WalkerComponent)GCHandledObjects.GCHandleToObject(instance)).OriginalSpeedVO);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WalkerComponent)GCHandledObjects.GCHandleToObject(instance)).OriginalSpeedVO = (ISpeedVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WalkerComponent)GCHandledObjects.GCHandleToObject(instance)).SetVOData((ISpeedVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}

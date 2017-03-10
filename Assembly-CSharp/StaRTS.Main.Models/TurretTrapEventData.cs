using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class TurretTrapEventData : ITrapEventData
	{
		public string TurretUid
		{
			get;
			private set;
		}

		public string TurretAnimatorName
		{
			get;
			private set;
		}

		public ITrapEventData Init(string rawData)
		{
			if (string.IsNullOrEmpty(rawData))
			{
				Service.Get<StaRTSLogger>().Error("All Turret Traps must list the uid of the turret and the animator name");
				return null;
			}
			if (!rawData.Contains(","))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Turret Trap Data must have 2 comma-delimited values for turret type and turret animator game object.  ({0})", new object[]
				{
					rawData
				});
				return null;
			}
			string[] array = rawData.TrimEnd(new char[]
			{
				' '
			}).Split(new char[]
			{
				','
			});
			if (array.Length > 2)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Turret Trap Data must have exactly 2 values, one for turret type and one for turret animator game object.  ({0})", new object[]
				{
					rawData
				});
				return null;
			}
			this.TurretUid = array[0];
			this.TurretAnimatorName = array[1];
			return this;
		}

		public TurretTrapEventData()
		{
		}

		protected internal TurretTrapEventData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTrapEventData)GCHandledObjects.GCHandleToObject(instance)).TurretAnimatorName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTrapEventData)GCHandledObjects.GCHandleToObject(instance)).TurretUid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretTrapEventData)GCHandledObjects.GCHandleToObject(instance)).Init(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TurretTrapEventData)GCHandledObjects.GCHandleToObject(instance)).TurretAnimatorName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TurretTrapEventData)GCHandledObjects.GCHandleToObject(instance)).TurretUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}

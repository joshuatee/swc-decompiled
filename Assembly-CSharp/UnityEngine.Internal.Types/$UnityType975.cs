using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Player.World;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType975 : $UnityType
	{
		public unsafe $UnityType975()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 601996) = ldftn($Invoke0);
			*(data + 602024) = ldftn($Invoke1);
			*(data + 602052) = ldftn($Invoke2);
			*(data + 602080) = ldftn($Invoke3);
			*(data + 602108) = ldftn($Invoke4);
			*(data + 602136) = ldftn($Invoke5);
			*(data + 602164) = ldftn($Invoke6);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetFactionAssetName());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPlanetData());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetPreloads());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetProjectilePreloads((Map)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldName());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).GetWorldType());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((IMapDataLoader)GCHandledObjects.GCHandleToObject(instance)).LoadMapData((MapLoadedDelegate)GCHandledObjects.GCHandleToObject(*args), (MapLoadFailDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}

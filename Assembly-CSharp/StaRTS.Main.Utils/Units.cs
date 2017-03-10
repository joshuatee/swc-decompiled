using StaRTS.DataStructures;
using StaRTS.GameBoard.Components;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class Units
	{
		public const float BASE = 1f;

		public const int BOARD_UNITS_PER_GRID_UNIT = 1;

		public const int WORLD_UNITS_PER_GRID_UNIT = 3;

		public const int BUILDABLE_GRID_SIZE = 42;

		public const int DEPLOYABLE_GRID_SIZE = 46;

		public const int DEPLOYABLE_HALF_GRID_SIZE = 23;

		public const float WORLD_TO_GRID = 0.333333343f;

		public const float BOARD_TO_GRID = 1f;

		public const float GRID_TO_BOARD = 1f;

		public const float GRID_TO_WORLD = 3f;

		public const float WORLD_TO_BOARD = 0.333333343f;

		public const float BOARD_TO_WORLD = 3f;

		public const int BUILDABLE_BOARD_SIZE = 42;

		public const int BUILDABLE_WORLD_SIZE = 126;

		public const int DEPLOYABLE_BOARD_SIZE = 46;

		public const int DEPLOYABLE_WORLD_SIZE = 138;

		public static int WorldToGridX(float worldX)
		{
			return (int)Mathf.Floor(worldX * 0.333333343f);
		}

		public static int WorldToGridZ(float worldZ)
		{
			return (int)Mathf.Floor(worldZ * 0.333333343f);
		}

		public static int WorldToBoardX(float worldX)
		{
			return (int)Mathf.Floor(worldX * 0.333333343f);
		}

		public static int WorldToBoardZ(float worldZ)
		{
			return (int)Mathf.Floor(worldZ * 0.333333343f);
		}

		public static IntPosition NormalizeDeployPosition(IntPosition boardPosition)
		{
			if (boardPosition.z <= -23)
			{
				boardPosition.z = -22;
			}
			if (boardPosition.x <= -23)
			{
				boardPosition.x = -22;
			}
			return boardPosition;
		}

		public static IntPosition WorldToBoardIntDeployPosition(Vector3 vec)
		{
			IntPosition boardPosition = new IntPosition(Units.WorldToBoardX(vec.x), Units.WorldToBoardZ(vec.z));
			return Units.NormalizeDeployPosition(boardPosition);
		}

		public static IntPosition WorldToBoardIntPosition(Vector3 vec)
		{
			return new IntPosition(Units.WorldToBoardX(vec.x), Units.WorldToBoardZ(vec.z));
		}

		public static int GridToBoardX(int gridX)
		{
			return (int)Mathf.Floor((float)gridX * 1f);
		}

		public static int GridToBoardZ(int gridZ)
		{
			return (int)Mathf.Floor((float)gridZ * 1f);
		}

		public static float GridToWorldX(int gridX)
		{
			return (float)gridX * 3f;
		}

		public static float GridToWorldZ(int gridZ)
		{
			return (float)gridZ * 3f;
		}

		public static float BoardToWorldX(int boardX)
		{
			return (float)boardX * 3f;
		}

		public static float BoardToWorldZ(int boardZ)
		{
			return (float)boardZ * 3f;
		}

		public static float BoardToWorldX(float boardX)
		{
			return boardX * 3f;
		}

		public static float BoardToWorldZ(float boardZ)
		{
			return boardZ * 3f;
		}

		public static Vector3 BoardToWorldVec(IntPosition position, float yPosition)
		{
			return new Vector3(Units.BoardToWorldX(position.x), yPosition, Units.BoardToWorldZ(position.z));
		}

		public static Vector3 BoardToWorldVec(IntPosition position)
		{
			return Units.BoardToWorldVec(position, 0f);
		}

		public static int BoardToGridX(int boardX)
		{
			return (int)Mathf.Floor((float)boardX * 1f);
		}

		public static int BoardToGridZ(int boardZ)
		{
			return (int)Mathf.Floor((float)boardZ * 1f);
		}

		public static SizeComponent SizeCompFromGrid(int gridWidth, int gridDepth)
		{
			return new SizeComponent(Units.GridToBoardX(gridWidth), Units.GridToBoardZ(gridDepth));
		}

		public static SizeComponent SizeCompFromWorld(int worldWidth, int worldDepth)
		{
			return new SizeComponent(Units.WorldToBoardX((float)worldWidth), Units.WorldToBoardZ((float)worldDepth));
		}

		public static void SnapWorldToGridX(ref float worldX)
		{
			worldX = Mathf.Round(worldX * 0.333333343f) * 3f;
		}

		public static void SnapWorldToGridZ(ref float worldZ)
		{
			worldZ = Mathf.Round(worldZ * 0.333333343f) * 3f;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.BoardToGridX(*(int*)args));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.BoardToGridZ(*(int*)args));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.BoardToWorldVec(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.BoardToWorldVec(*(*(IntPtr*)args), *(float*)(args + 1)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.BoardToWorldX(*(int*)args));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.BoardToWorldX(*(float*)args));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.BoardToWorldZ(*(int*)args));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.BoardToWorldZ(*(float*)args));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.GridToBoardX(*(int*)args));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.GridToBoardZ(*(int*)args));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.GridToWorldX(*(int*)args));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.GridToWorldZ(*(int*)args));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.NormalizeDeployPosition(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.SizeCompFromGrid(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.SizeCompFromWorld(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.WorldToBoardIntDeployPosition(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.WorldToBoardIntPosition(*(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.WorldToBoardX(*(float*)args));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.WorldToBoardZ(*(float*)args));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.WorldToGridX(*(float*)args));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(Units.WorldToGridZ(*(float*)args));
		}
	}
}

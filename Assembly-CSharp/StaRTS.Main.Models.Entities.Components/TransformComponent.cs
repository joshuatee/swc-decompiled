using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class TransformComponent : ComponentBase
	{
		public int X;

		public int Z;

		public float Rotation;

		public bool RotationInitialized;

		public float RotationVelocity;

		private int boardWidth;

		private int boardDepth;

		public int BoardWidth
		{
			get
			{
				return this.boardWidth;
			}
		}

		public int BoardDepth
		{
			get
			{
				return this.boardDepth;
			}
		}

		public TransformComponent(int boardX, int boardZ, float rotation, bool rotationInitialized, int boardWidth, int boardDepth)
		{
			this.X = boardX;
			this.Z = boardZ;
			this.Rotation = rotation;
			this.RotationInitialized = rotationInitialized;
			this.RotationVelocity = 0f;
			this.boardWidth = boardWidth;
			this.boardDepth = boardDepth;
		}

		public float CenterX()
		{
			return (float)this.X + (float)this.boardWidth / 2f;
		}

		public float CenterZ()
		{
			return (float)this.Z + (float)this.boardDepth / 2f;
		}

		public int CenterGridX()
		{
			return this.X + this.boardWidth / 2;
		}

		public int CenterGridZ()
		{
			return this.Z + this.boardDepth / 2;
		}

		public int MinX()
		{
			return this.X;
		}

		public int MaxX()
		{
			return this.X + this.boardWidth - 1;
		}

		public int MinZ()
		{
			return this.Z;
		}

		public int MaxZ()
		{
			return this.Z + this.boardDepth - 1;
		}

		protected internal TransformComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransformComponent)GCHandledObjects.GCHandleToObject(instance)).CenterGridX());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransformComponent)GCHandledObjects.GCHandleToObject(instance)).CenterGridZ());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransformComponent)GCHandledObjects.GCHandleToObject(instance)).CenterX());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransformComponent)GCHandledObjects.GCHandleToObject(instance)).CenterZ());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransformComponent)GCHandledObjects.GCHandleToObject(instance)).BoardDepth);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransformComponent)GCHandledObjects.GCHandleToObject(instance)).BoardWidth);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransformComponent)GCHandledObjects.GCHandleToObject(instance)).MaxX());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransformComponent)GCHandledObjects.GCHandleToObject(instance)).MaxZ());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransformComponent)GCHandledObjects.GCHandleToObject(instance)).MinX());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransformComponent)GCHandledObjects.GCHandleToObject(instance)).MinZ());
		}
	}
}

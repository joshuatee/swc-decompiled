using Net.RichardLord.Ash.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.GameBoard.Components
{
	public class SizeComponent : ComponentBase
	{
		public int Width;

		public int Depth;

		public SizeComponent(int boardWidth, int boardDepth)
		{
			this.Width = boardWidth;
			this.Depth = boardDepth;
		}

		public Vector3 ToVector3(float height)
		{
			return new Vector3((float)this.Width, height, (float)this.Depth);
		}

		public Vector3 ToVector3()
		{
			return this.ToVector3(0f);
		}

		protected internal SizeComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SizeComponent)GCHandledObjects.GCHandleToObject(instance)).ToVector3());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SizeComponent)GCHandledObjects.GCHandleToObject(instance)).ToVector3(*(float*)args));
		}
	}
}

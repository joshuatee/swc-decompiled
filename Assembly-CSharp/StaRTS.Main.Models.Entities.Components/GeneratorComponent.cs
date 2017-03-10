using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class GeneratorComponent : ComponentBase, IResourceFillable
	{
		public float CurrentFullnessPercentage
		{
			get;
			set;
		}

		public float PreviousFullnessPercentage
		{
			get;
			set;
		}

		public GeneratorComponent()
		{
		}

		protected internal GeneratorComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).CurrentFullnessPercentage);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).PreviousFullnessPercentage);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).CurrentFullnessPercentage = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((GeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).PreviousFullnessPercentage = *(float*)args;
			return -1L;
		}
	}
}

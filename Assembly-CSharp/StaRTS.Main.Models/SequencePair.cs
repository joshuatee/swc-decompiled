using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class SequencePair
	{
		public int[] GunSequence
		{
			get;
			private set;
		}

		public Dictionary<int, int> Sequences
		{
			get;
			private set;
		}

		public SequencePair(int[] gunSequence, Dictionary<int, int> sequences)
		{
			this.GunSequence = gunSequence;
			this.Sequences = sequences;
		}

		protected internal SequencePair(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SequencePair)GCHandledObjects.GCHandleToObject(instance)).GunSequence);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SequencePair)GCHandledObjects.GCHandleToObject(instance)).Sequences);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SequencePair)GCHandledObjects.GCHandleToObject(instance)).GunSequence = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SequencePair)GCHandledObjects.GCHandleToObject(instance)).Sequences = (Dictionary<int, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

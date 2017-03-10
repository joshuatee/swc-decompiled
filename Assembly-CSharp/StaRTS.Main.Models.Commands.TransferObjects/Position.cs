using StaRTS.Utils.Json;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.TransferObjects
{
	public class Position : ISerializable
	{
		public int X
		{
			get;
			set;
		}

		public int Z
		{
			get;
			set;
		}

		public string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.Add<int>("x", this.X);
			serializer.Add<int>("z", this.Z);
			return serializer.End().ToString();
		}

		public ISerializable FromObject(object obj)
		{
			return null;
		}

		public Position()
		{
		}

		protected internal Position(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Position)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Position)GCHandledObjects.GCHandleToObject(instance)).X);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Position)GCHandledObjects.GCHandleToObject(instance)).Z);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Position)GCHandledObjects.GCHandleToObject(instance)).X = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Position)GCHandledObjects.GCHandleToObject(instance)).Z = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Position)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}

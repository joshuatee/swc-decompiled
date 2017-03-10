using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.TransferObjects
{
	public class PositionMap : ISerializable
	{
		private Dictionary<string, Position> positions;

		public PositionMap()
		{
			this.positions = new Dictionary<string, Position>();
		}

		public Position GetPosition(string id)
		{
			if (this.positions.ContainsKey(id))
			{
				return this.positions[id];
			}
			return null;
		}

		public void AddPosition(string id, Position pos)
		{
			this.positions.Add(id, pos);
		}

		public void ClearAllPositions()
		{
			this.positions.Clear();
		}

		public string ToJson()
		{
			Serializer serializer = Serializer.Start();
			foreach (string current in this.positions.Keys)
			{
				serializer.AddObject<Position>(current, this.positions[current]);
			}
			return serializer.End().ToString();
		}

		public ISerializable FromObject(object obj)
		{
			return null;
		}

		protected internal PositionMap(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PositionMap)GCHandledObjects.GCHandleToObject(instance)).AddPosition(Marshal.PtrToStringUni(*(IntPtr*)args), (Position)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PositionMap)GCHandledObjects.GCHandleToObject(instance)).ClearAllPositions();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PositionMap)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PositionMap)GCHandledObjects.GCHandleToObject(instance)).GetPosition(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PositionMap)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}

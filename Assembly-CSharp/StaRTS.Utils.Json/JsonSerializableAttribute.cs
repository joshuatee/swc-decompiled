using System;
using WinRTBridge;

namespace StaRTS.Utils.Json
{
	[AttributeUsage]
	public class JsonSerializableAttribute : Attribute
	{
		public MappingType Type
		{
			get;
			set;
		}

		public JsonSerializableAttribute(MappingType type)
		{
			this.Type = type;
		}

		public JsonSerializableAttribute() : this(MappingType.Properties)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((JsonSerializableAttribute)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((JsonSerializableAttribute)GCHandledObjects.GCHandleToObject(instance)).Type = (MappingType)(*(int*)args);
			return -1L;
		}
	}
}

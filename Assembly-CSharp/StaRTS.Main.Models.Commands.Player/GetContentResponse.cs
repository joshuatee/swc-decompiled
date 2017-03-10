using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class GetContentResponse : AbstractResponse
	{
		public List<string> CdnRoots
		{
			get;
			private set;
		}

		public string AppCode
		{
			get;
			private set;
		}

		public string Environment
		{
			get;
			private set;
		}

		public string ManifestVersion
		{
			get;
			private set;
		}

		public List<string> Patches
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			List<object> list = dictionary["secureCdnRoots"] as List<object>;
			this.CdnRoots = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				this.CdnRoots.Add((string)list[i]);
			}
			this.AppCode = (string)dictionary["appCode"];
			this.Environment = (string)dictionary["environment"];
			this.ManifestVersion = (string)dictionary["manifestVersion"];
			list = (dictionary["patches"] as List<object>);
			this.Patches = new List<string>();
			for (int j = 0; j < list.Count; j++)
			{
				this.Patches.Add((string)list[j]);
			}
			return this;
		}

		public GetContentResponse()
		{
		}

		protected internal GetContentResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).AppCode);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).CdnRoots);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).Environment);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).ManifestVersion);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).Patches);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).AppCode = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).CdnRoots = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).Environment = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).ManifestVersion = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((GetContentResponse)GCHandledObjects.GCHandleToObject(instance)).Patches = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}

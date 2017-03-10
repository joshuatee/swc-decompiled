using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class Deployable : ISerializable
	{
		private const string AMOUNT_KEY = "amount";

		private const string UID_KEY = "uid";

		public int Amount
		{
			get;
			set;
		}

		public string Uid
		{
			get;
			set;
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Amount = Convert.ToInt32(dictionary["amount"], CultureInfo.InvariantCulture);
			this.Uid = (dictionary["uid"] as string);
			return this;
		}

		public string ToJson()
		{
			return Serializer.Start().Add<int>("amount", this.Amount).AddString("uid", this.Uid).End().ToString();
		}

		public Deployable()
		{
		}

		protected internal Deployable(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Deployable)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Deployable)GCHandledObjects.GCHandleToObject(instance)).Amount);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Deployable)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Deployable)GCHandledObjects.GCHandleToObject(instance)).Amount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Deployable)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Deployable)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}

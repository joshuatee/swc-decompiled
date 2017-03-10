using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Request
{
	public class Batch : AbstractRequest
	{
		private string authKey;

		private uint lastLoginTime;

		private bool pickupMessages;

		public List<ICommand> Commands
		{
			get;
			private set;
		}

		public bool Sync
		{
			get;
			set;
		}

		public uint Tries
		{
			get;
			set;
		}

		public Batch()
		{
			this.Commands = new List<ICommand>();
			this.Init();
		}

		public Batch(ICommand command)
		{
			this.Commands = new List<ICommand>(1);
			this.Commands.Add(command);
			this.Init();
		}

		public Batch(List<ICommand> commands)
		{
			this.Commands = new List<ICommand>(commands);
			this.Init();
		}

		private void Init()
		{
			this.Sync = true;
			this.Tries = 1u;
		}

		public void Prepare(string authKey, uint lastLoginTime, bool pickupMessages)
		{
			this.authKey = authKey;
			this.lastLoginTime = lastLoginTime;
			this.pickupMessages = pickupMessages;
		}

		public override string ToJson()
		{
			return Serializer.Start().AddString("authKey", this.authKey).AddBool("pickupMessages", this.pickupMessages).Add<uint>("lastLoginTime", this.lastLoginTime).AddArray<ICommand>("commands", this.Commands).End().ToString();
		}

		protected internal Batch(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Batch)GCHandledObjects.GCHandleToObject(instance)).Commands);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Batch)GCHandledObjects.GCHandleToObject(instance)).Sync);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Batch)GCHandledObjects.GCHandleToObject(instance)).Init();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Batch)GCHandledObjects.GCHandleToObject(instance)).Commands = (List<ICommand>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Batch)GCHandledObjects.GCHandleToObject(instance)).Sync = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Batch)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}

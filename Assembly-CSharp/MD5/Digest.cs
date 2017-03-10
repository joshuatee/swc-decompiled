using System;
using WinRTBridge;

namespace MD5
{
	public sealed class Digest
	{
		public uint A;

		public uint B;

		public uint C;

		public uint D;

		public Digest()
		{
			this.A = 1732584193u;
			this.B = 4023233417u;
			this.C = 2562383102u;
			this.D = 271733878u;
		}

		public override string ToString()
		{
			return MD5Helper.ReverseByte(this.A).ToString("X8") + MD5Helper.ReverseByte(this.B).ToString("X8") + MD5Helper.ReverseByte(this.C).ToString("X8") + MD5Helper.ReverseByte(this.D).ToString("X8");
		}

		internal Digest(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Digest)GCHandledObjects.GCHandleToObject(instance)).ToString());
		}
	}
}

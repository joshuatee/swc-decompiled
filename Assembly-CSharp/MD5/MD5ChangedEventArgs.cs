using System;

namespace MD5
{
	public class MD5ChangedEventArgs : EventArgs
	{
		public readonly byte[] NewData;

		public readonly string FingerPrint;

		public MD5ChangedEventArgs(byte[] data, string HashedValue)
		{
			byte[] array = new byte[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = data[i];
			}
			this.FingerPrint = HashedValue;
		}

		public MD5ChangedEventArgs(string data, string HashedValue)
		{
			byte[] array = new byte[data.get_Length()];
			for (int i = 0; i < data.get_Length(); i++)
			{
				array[i] = (byte)data.get_Chars(i);
			}
			this.FingerPrint = HashedValue;
		}

		protected internal MD5ChangedEventArgs(UIntPtr dummy)
		{
		}
	}
}

using System;

namespace MD5
{
	public class MD5ChangingEventArgs : EventArgs
	{
		public readonly byte[] NewData;

		public MD5ChangingEventArgs(byte[] data)
		{
			byte[] array = new byte[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = data[i];
			}
		}

		public MD5ChangingEventArgs(string data)
		{
			byte[] array = new byte[data.get_Length()];
			for (int i = 0; i < data.get_Length(); i++)
			{
				array[i] = (byte)data.get_Chars(i);
			}
		}

		protected internal MD5ChangingEventArgs(UIntPtr dummy)
		{
		}
	}
}

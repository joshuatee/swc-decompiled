using System;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Utils
{
	public class EncryptionUtil
	{
		private const int kCCBlockSizeAES128 = 16;

		private const int kCCKeySizeAES128 = 16;

		public static string EncryptString(string rawData)
		{
			if (rawData == null)
			{
				return null;
			}
			string result = "";
			byte[] bytes = Encoding.UTF8.GetBytes(rawData);
			int num = bytes.Length;
			if (num % 16 > 0)
			{
				num = (num + 16) / 16 * 16;
			}
			byte[] array = new byte[num];
			Array.Copy(bytes, array, bytes.Length);
			int num2 = 20;
			byte[] array2 = new byte[]
			{
				45,
				55,
				162,
				205,
				40,
				214,
				99,
				105,
				236,
				91,
				177,
				233,
				174,
				28,
				10,
				94,
				12,
				192,
				243,
				93
			};
			byte[] array3 = new byte[16];
			int num3 = 0;
			for (int i = 0; i < num2; i++)
			{
				if (i != 1 && i != 4 && i != 11 && i != 19)
				{
					array3[num3++] = array2[i];
				}
			}
			return result;
		}

		public EncryptionUtil()
		{
		}

		protected internal EncryptionUtil(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EncryptionUtil.EncryptString(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}

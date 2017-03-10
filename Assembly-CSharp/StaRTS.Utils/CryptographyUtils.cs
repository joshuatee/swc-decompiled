using MD5;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Utils
{
	public class CryptographyUtils
	{
		public static byte[] ComputeHmacHash(string algorithm, string secret, string plainText)
		{
			return CryptoUtility.ComputeHash(plainText, secret);
		}

		public static string ComputeMD5Hash(string input)
		{
			string fingerPrint = new MD5
			{
				Value = input
			}.FingerPrint;
			return fingerPrint.ToLower();
		}

		public CryptographyUtils()
		{
		}

		protected internal CryptographyUtils(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CryptographyUtils.ComputeHmacHash(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CryptographyUtils.ComputeMD5Hash(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}
	}
}

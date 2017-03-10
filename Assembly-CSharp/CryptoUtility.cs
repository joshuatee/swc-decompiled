using System;
using System.Runtime.InteropServices;
using WinRTBridge;

public class CryptoUtility
{
	public delegate byte[] ComputeHashCallBack(string plainText, string secret);

	public static string errorMsg = "Error - ComputeHash callback not declared";

	public static CryptoUtility.ComputeHashCallBack ComputeHashExtern;

	public static byte[] ComputeHash(string plainText, string secret)
	{
		if (CryptoUtility.ComputeHashExtern != null)
		{
			return CryptoUtility.ComputeHashExtern(plainText, secret);
		}
		return new byte[0];
	}

	public CryptoUtility()
	{
	}

	protected internal CryptoUtility(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(CryptoUtility.ComputeHash(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}
}

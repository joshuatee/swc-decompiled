using System;
using WinRTBridge;

public class CompressionUtility
{
	public delegate string GetDecompressedDataCallBack(byte[] data);

	public static CompressionUtility.GetDecompressedDataCallBack GetDecompressedDataExtern;

	public static string GetDecompressedData(byte[] data)
	{
		if (CompressionUtility.GetDecompressedDataExtern != null)
		{
			return CompressionUtility.GetDecompressedDataExtern(data);
		}
		return "Error - GetDecompressedData callback not declared";
	}

	public CompressionUtility()
	{
	}

	protected internal CompressionUtility(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(CompressionUtility.GetDecompressedData((byte[])GCHandledObjects.GCHandleToPinnedArrayObject(*args)));
	}
}

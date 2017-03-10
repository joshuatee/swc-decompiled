using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Utils.IO
{
	public static class FileUtils
	{
		public static string Read(string absFilePath)
		{
			string result;
			try
			{
				result = StorageUtility.FileReadAllText(absFilePath);
			}
			catch (Exception)
			{
				throw new Exception("Failed to read file: " + absFilePath);
			}
			return result;
		}

		public static void Write(string absFilePath, string text)
		{
		}

		public static string GetAbsFilePathInMyDocuments(string fileName, string dir)
		{
			return FileUtils.GetAbsDirPathInMyDocuments(dir) + "/" + fileName;
		}

		public static string GetAbsDirPathInMyDocuments(string dir)
		{
			dir = StorageUtility.GetSpecialFolderPath() + dir;
			return dir;
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FileUtils.GetAbsDirPathInMyDocuments(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FileUtils.GetAbsFilePathInMyDocuments(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(FileUtils.Read(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			FileUtils.Write(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}
	}
}

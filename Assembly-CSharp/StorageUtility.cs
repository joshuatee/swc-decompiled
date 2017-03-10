using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using Windows.Storage;
using Windows.Storage.Streams;
using WinRTBridge;

public class StorageUtility
{
	public const char PATH_SEPARATOR = '\\';

	public const string INVALID_PATH_SEPARATORS = "/";

	public static string ConvertPathSeparators(string path)
	{
		string text = "/";
		for (int i = 0; i < text.get_Length(); i++)
		{
			char c = text.get_Chars(i);
			path = path.Replace(c, '\\');
		}
		return path;
	}

	public static int GetPathSeparatorIndex(string path, bool fromStart)
	{
		int num = fromStart ? path.IndexOf('\\') : path.LastIndexOf('\\');
		if (num > 0 && path.get_Chars(num - 1) == ':')
		{
			num = (fromStart ? path.IndexOf('\\', num + 1) : path.LastIndexOf('\\', num + 1));
		}
		return num;
	}

	public static bool IsPathSeparator(char c)
	{
		return c == '\\';
	}

	private static async Task<StorageFolder> AsyncGetFolderFromPath(string path)
	{
		return await StorageFolder.GetFolderFromPathAsync(path);
	}

	public static StorageFolder GetFolderFromPath(string path)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		Task<StorageFolder> task = StorageUtility.AsyncGetFolderFromPath(path);
		task.Wait();
		return task.Result;
	}

	private static async Task<StorageFolder> AsyncGetFolder(StorageFolder folder, string name)
	{
		return await folder.GetFolderAsync(name);
	}

	public static StorageFolder GetFolder(StorageFolder folder, string name)
	{
		Task<StorageFolder> task = StorageUtility.AsyncGetFolder(folder, name);
		task.Wait();
		return task.Result;
	}

	private static async Task<StorageFolder> AsyncCreateFolder(StorageFolder folder, string name, CreationCollisionOption collisionOption)
	{
		return await folder.CreateFolderAsync(name, collisionOption);
	}

	public static StorageFolder CreateFolder(StorageFolder folder, string name, CreationCollisionOption collisionOption)
	{
		Task<StorageFolder> task = StorageUtility.AsyncCreateFolder(folder, name, collisionOption);
		task.Wait();
		return task.Result;
	}

	public static StorageFolder CreateFolder(StorageFolder folder, string name)
	{
		return StorageUtility.CreateFolder(folder, name, 3);
	}

	private static async Task<StorageFile> AsyncCreateFile(StorageFolder folder, string name, CreationCollisionOption collisionOption)
	{
		return await folder.CreateFileAsync(name, collisionOption);
	}

	public static StorageFile CreateFile(StorageFolder folder, string name, CreationCollisionOption collisionOption)
	{
		Task<StorageFile> task = StorageUtility.AsyncCreateFile(folder, name, collisionOption);
		task.Wait();
		return task.Result;
	}

	public static StorageFile CreateFile(StorageFolder folder, string name)
	{
		return StorageUtility.CreateFile(folder, name, 1);
	}

	private static async Task<StorageFile> AsyncCreateFileAtPath(string path, string name, CreationCollisionOption collisionOption)
	{
		StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(path);
		StorageFolder storageFolder2 = storageFolder;
		return await storageFolder2.CreateFileAsync(name, collisionOption);
	}

	public static StorageFile CreateFileAtPath(string path, string name, CreationCollisionOption collisionOption)
	{
		Task<StorageFile> task = StorageUtility.AsyncCreateFileAtPath(path, name, collisionOption);
		task.Wait();
		return task.Result;
	}

	private static async Task<StorageFile> AsyncGetFileFromPath(string path)
	{
		return await StorageFile.GetFileFromPathAsync(path);
	}

	public static StorageFile GetFileFromPath(string path)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		Task<StorageFile> task = StorageUtility.AsyncGetFileFromPath(path);
		task.Wait();
		return task.Result;
	}

	private static async Task<bool> AsyncMoveAndReplaceFile(StorageFile replacer, StorageFile fileToReplace)
	{
		await replacer.MoveAndReplaceAsync(fileToReplace);
		return true;
	}

	public static void MoveAndReplaceFile(StorageFile replacer, StorageFile fileToReplace)
	{
		Task task = StorageUtility.AsyncMoveAndReplaceFile(replacer, fileToReplace);
		task.Wait();
	}

	private static async Task<bool> FileExistsAsync(string path)
	{
		StorageFile storageFile = null;
		bool result;
		try
		{
			StorageFile storageFile2 = await StorageFile.GetFileFromPathAsync(path);
			storageFile = storageFile2;
		}
		catch (Exception var_5_88)
		{
			result = false;
			return result;
		}
		result = (storageFile != null);
		return result;
	}

	public static bool FileExists(string path)
	{
		if (path == null)
		{
			return false;
		}
		path = StorageUtility.ConvertPathSeparators(path);
		Task<bool> task = StorageUtility.FileExistsAsync(path);
		task.Wait();
		return task.Result;
	}

	private static async Task<bool> FileDeleteAsync(string path)
	{
		StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);
		StorageFile storageFile2 = storageFile;
		await storageFile2.DeleteAsync();
		return true;
	}

	public static void FileDelete(string path)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		Task<bool> task = StorageUtility.FileDeleteAsync(path);
		task.Wait();
	}

	private static async Task<bool> FileRenameAsync(string sourcePath, string destPath)
	{
		StorageFile storageFile = await StorageFile.GetFileFromPathAsync(sourcePath);
		StorageFile storageFile2 = storageFile;
		int pathSeparatorIndex = StorageUtility.GetPathSeparatorIndex(destPath, false);
		await storageFile2.RenameAsync(destPath.Substring(pathSeparatorIndex + 1));
		return true;
	}

	public static void FileRename(string sourcePath, string destPath)
	{
		sourcePath = StorageUtility.ConvertPathSeparators(sourcePath);
		destPath = StorageUtility.ConvertPathSeparators(destPath);
		Task<bool> task = StorageUtility.FileRenameAsync(sourcePath, destPath);
		task.Wait();
	}

	private static async Task<IBuffer> AsyncFileReadBuffer(string path)
	{
		StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);
		StorageFile storageFile2 = storageFile;
		return await FileIO.ReadBufferAsync(storageFile2);
	}

	public static IBuffer FileReadBuffer(string path)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		Task<IBuffer> task = StorageUtility.AsyncFileReadBuffer(path);
		task.Wait();
		return task.Result;
	}

	private static async Task<bool> AsyncFileWriteLines(string path, string[] contents)
	{
		StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);
		StorageFile storageFile2 = storageFile;
		await FileIO.WriteLinesAsync(storageFile2, contents);
		return true;
	}

	public static void FileWriteLines(string path, string[] contents)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		Task<bool> task = StorageUtility.AsyncFileWriteLines(path, contents);
		task.Wait();
	}

	private static async Task<IEnumerable<string>> AsyncFileReadLines(string path)
	{
		StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);
		StorageFile storageFile2 = storageFile;
		return await FileIO.ReadLinesAsync(storageFile2);
	}

	public static string[] FileReadLines(string path)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		Task<IEnumerable<string>> task = StorageUtility.AsyncFileReadLines(path);
		task.Wait();
		return task.Result.Cast<string>().ToArray<string>();
	}

	private static async Task<bool> AsyncFileWriteText(string path, string text)
	{
		StorageFile file = StorageUtility.GetFile(path, FileOperationMode.Append);
		await FileIO.WriteTextAsync(file, text);
		return true;
	}

	public static void FileWriteText(string path, string text)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		Task<bool> task = StorageUtility.AsyncFileWriteText(path, text);
		task.Wait();
	}

	private static async Task<bool> AsyncFileCopy(string sourcePath, string destPath, bool overwrite)
	{
		StorageFile storageFile = await StorageFile.GetFileFromPathAsync(sourcePath);
		StorageFile storageFile2 = storageFile;
		int num = destPath.LastIndexOf('\\');
		if (num == -1)
		{
			num = destPath.LastIndexOf('/');
		}
		StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(destPath.Substring(0, num));
		await storageFile2.CopyAsync(storageFolder, destPath.Substring(num + 1), overwrite ? 1 : 2);
		return true;
	}

	public static void FileCopy(string sourcePath, string destPath)
	{
		sourcePath = StorageUtility.ConvertPathSeparators(sourcePath);
		destPath = StorageUtility.ConvertPathSeparators(destPath);
		StorageUtility.FileCopy(sourcePath, destPath, false);
	}

	public static void FileCopy(string sourcePath, string destPath, bool overwrite)
	{
		Task<bool> task = StorageUtility.AsyncFileCopy(sourcePath, destPath, overwrite);
		task.Wait();
	}

	public static string CreateFolderAtPath(string path)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		while (StorageUtility.IsPathSeparator(path.get_Chars(path.get_Length() - 1)))
		{
			path = path.Substring(0, path.get_Length() - 1);
		}
		int num = path.get_Length();
		string text = path;
		List<string> list = new List<string>();
		StorageFolder storageFolder = null;
		while (storageFolder == null && num != -1)
		{
			text = text.Substring(0, num);
			num = StorageUtility.GetPathSeparatorIndex(text, false);
			try
			{
				storageFolder = StorageUtility.GetFolderFromPath(text);
			}
			catch (Exception var_4_5D)
			{
				if (num != -1)
				{
					list.Add(text.Substring(num + 1));
				}
				storageFolder = null;
			}
		}
		if (storageFolder == null)
		{
			throw new Exception("No accesible folder found in path: " + path);
		}
		int count = list.Count;
		if (list.Count > 0)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				storageFolder = StorageUtility.CreateFolder(storageFolder, list[i]);
			}
		}
		return storageFolder.get_Path();
	}

	public static StorageFile GetFile(string path, FileOperationMode mode)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		StorageFile storageFile;
		try
		{
			storageFile = StorageUtility.GetFileFromPath(path);
		}
		catch (Exception ex)
		{
			if (mode == FileOperationMode.Append || mode == FileOperationMode.Create)
			{
				return StorageUtility.CreateFile(path);
			}
			throw ex;
		}
		if (mode == FileOperationMode.Create)
		{
			StorageFile storageFile2 = StorageUtility.CreateFileAtPath(StorageUtility.ConvertPathSeparators(Application.temporaryCachePath), "temp", 0);
			StorageUtility.MoveAndReplaceFile(storageFile2, storageFile);
			storageFile = storageFile2;
		}
		return storageFile;
	}

	public static StorageFile CreateFile(string path)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		int pathSeparatorIndex = StorageUtility.GetPathSeparatorIndex(path, false);
		if (pathSeparatorIndex == -1)
		{
			throw new Exception("No folder in file path: " + path);
		}
		int num = pathSeparatorIndex + 1;
		StorageFolder storageFolder = null;
		string text = path;
		List<string> list = new List<string>();
		while (storageFolder == null && pathSeparatorIndex != -1)
		{
			text = text.Substring(0, pathSeparatorIndex);
			pathSeparatorIndex = StorageUtility.GetPathSeparatorIndex(text, false);
			try
			{
				storageFolder = StorageUtility.GetFolderFromPath(text);
			}
			catch (Exception var_6_55)
			{
				if (pathSeparatorIndex != -1)
				{
					list.Add(text.Substring(pathSeparatorIndex + 1));
				}
				storageFolder = null;
			}
		}
		if (storageFolder == null)
		{
			throw new Exception("No accesible folder found in path: " + path);
		}
		int count = list.Count;
		if (list.Count > 0)
		{
			for (int i = list.Count; i >= 0; i--)
			{
				storageFolder = StorageUtility.CreateFolder(storageFolder, list[i]);
			}
		}
		return StorageUtility.CreateFile(storageFolder, path.Substring(num));
	}

	public static Stream GetInputStream(string path, FileOperationMode mode)
	{
		StorageFile file = StorageUtility.GetFile(path, mode);
		Task<Stream> task = WindowsRuntimeStorageExtensions.OpenStreamForReadAsync(file);
		task.Wait();
		Stream result = task.Result;
		if (mode == FileOperationMode.Append)
		{
			result.Seek(0L, SeekOrigin.End);
		}
		return result;
	}

	public static Stream GetOutputStream(string path, FileOperationMode mode)
	{
		StorageFile file = StorageUtility.GetFile(path, mode);
		Task<Stream> task = WindowsRuntimeStorageExtensions.OpenStreamForWriteAsync(file);
		task.Wait();
		Stream result = task.Result;
		if (mode == FileOperationMode.Append)
		{
			result.Seek(0L, SeekOrigin.End);
		}
		return result;
	}

	private static async Task<string> AsyncReadTextAsync(string path)
	{
		path = StorageUtility.ConvertPathSeparators(path);
		StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);
		StorageFile storageFile2 = storageFile;
		return await FileIO.ReadTextAsync(storageFile2);
	}

	public static string FileReadAllText(string file)
	{
		Task<string> task = StorageUtility.AsyncReadTextAsync(file);
		task.Wait();
		return task.Result;
	}

	private static async Task<bool> FolderExistsAsync(string path)
	{
		StorageFolder storageFolder = null;
		bool result;
		try
		{
			StorageFolder storageFolder2 = await StorageFolder.GetFolderFromPathAsync(path);
			storageFolder = storageFolder2;
		}
		catch
		{
			result = false;
			return result;
		}
		result = (storageFolder != null);
		return result;
	}

	public static bool FolderExists(string path)
	{
		if (path == null)
		{
			return false;
		}
		path = StorageUtility.ConvertPathSeparators(path);
		Task<bool> task = StorageUtility.FolderExistsAsync(path);
		task.Wait();
		return task.Result;
	}

	private static async Task<byte[]> ReadBytesAsync(string path)
	{
		byte[] result;
		if (path == null)
		{
			result = null;
		}
		else
		{
			path = StorageUtility.ConvertPathSeparators(path);
			StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);
			StorageFile storageFile2 = storageFile;
			IBuffer buffer = await FileIO.ReadBufferAsync(storageFile2);
			DataReader var_10 = DataReader.FromBuffer(buffer);
			try
			{
				byte[] var_11_128 = new byte[var_10.get_UnconsumedBufferLength()];
				var_10.ReadBytes(var_11_128);
				result = var_11_128;
			}
			finally
			{
				int num;
				if (num < 0 && var_10 != null)
				{
					var_10.Dispose();
				}
			}
		}
		return result;
	}

	public static byte[] FileReadAllBytes(string file)
	{
		Task<byte[]> task = StorageUtility.ReadBytesAsync(file);
		task.Wait();
		return task.Result;
	}

	private static async Task<bool> WriteBytesAsync(string path, byte[] buffer)
	{
		bool result;
		if (path == null)
		{
			result = false;
		}
		else
		{
			path = StorageUtility.ConvertPathSeparators(path);
			int num = path.LastIndexOf('\\');
			StorageFile storageFile = StorageUtility.CreateFileAtPath(path.Substring(0, num), path.Substring(num + 1), 3);
			await FileIO.WriteBytesAsync(storageFile, buffer);
			result = true;
		}
		return result;
	}

	public static void FileWriteAllBytes(string file, byte[] buffer)
	{
		Task<bool> task = StorageUtility.WriteBytesAsync(file, buffer);
		task.Wait();
	}

	public static string GetSpecialFolderPath()
	{
		return ApplicationData.get_Current().get_LocalFolder().get_Path();
	}

	public StorageUtility()
	{
	}

	protected internal StorageUtility(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncCreateFile((StorageFolder)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2)));
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncCreateFileAtPath(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2)));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncCreateFolder((StorageFolder)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2)));
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncFileCopy(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncFileReadBuffer(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncFileReadLines(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncFileWriteLines(Marshal.PtrToStringUni(*(IntPtr*)args), (string[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1])));
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncFileWriteText(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncGetFileFromPath(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncGetFolder((StorageFolder)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncGetFolderFromPath(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncMoveAndReplaceFile((StorageFile)GCHandledObjects.GCHandleToObject(*args), (StorageFile)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.AsyncReadTextAsync(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.ConvertPathSeparators(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.CreateFile(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.CreateFile((StorageFolder)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.CreateFile((StorageFolder)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2)));
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.CreateFileAtPath(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2)));
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.CreateFolder((StorageFolder)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.CreateFolder((StorageFolder)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2)));
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.CreateFolderAtPath(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		StorageUtility.FileCopy(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		StorageUtility.FileCopy(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(sbyte*)(args + 2) != 0);
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		StorageUtility.FileDelete(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.FileDeleteAsync(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.FileExists(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.FileExistsAsync(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.FileReadAllBytes(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.FileReadAllText(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.FileReadBuffer(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.FileReadLines(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		StorageUtility.FileRename(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.FileRenameAsync(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		StorageUtility.FileWriteAllBytes(Marshal.PtrToStringUni(*(IntPtr*)args), (byte[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]));
		return -1L;
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		StorageUtility.FileWriteLines(Marshal.PtrToStringUni(*(IntPtr*)args), (string[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]));
		return -1L;
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		StorageUtility.FileWriteText(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.FolderExists(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.FolderExistsAsync(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke38(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.GetFile(Marshal.PtrToStringUni(*(IntPtr*)args), (FileOperationMode)(*(int*)(args + 1))));
	}

	public unsafe static long $Invoke39(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.GetFileFromPath(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke40(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.GetFolder((StorageFolder)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
	}

	public unsafe static long $Invoke41(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.GetFolderFromPath(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke42(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.GetInputStream(Marshal.PtrToStringUni(*(IntPtr*)args), (FileOperationMode)(*(int*)(args + 1))));
	}

	public unsafe static long $Invoke43(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.GetOutputStream(Marshal.PtrToStringUni(*(IntPtr*)args), (FileOperationMode)(*(int*)(args + 1))));
	}

	public unsafe static long $Invoke44(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.GetPathSeparatorIndex(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0));
	}

	public unsafe static long $Invoke45(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.GetSpecialFolderPath());
	}

	public unsafe static long $Invoke46(long instance, long* args)
	{
		StorageUtility.MoveAndReplaceFile((StorageFile)GCHandledObjects.GCHandleToObject(*args), (StorageFile)GCHandledObjects.GCHandleToObject(args[1]));
		return -1L;
	}

	public unsafe static long $Invoke47(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.ReadBytesAsync(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke48(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(StorageUtility.WriteBytesAsync(Marshal.PtrToStringUni(*(IntPtr*)args), (byte[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1])));
	}
}

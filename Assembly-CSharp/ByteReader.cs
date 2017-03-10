using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using WinRTBridge;

public class ByteReader
{
	private byte[] mBuffer;

	private int mOffset;

	private static BetterList<string> mTemp = new BetterList<string>();

	public bool canRead
	{
		get
		{
			return this.mBuffer != null && this.mOffset < this.mBuffer.Length;
		}
	}

	public ByteReader(byte[] bytes)
	{
		this.mBuffer = bytes;
	}

	public ByteReader(TextAsset asset)
	{
		this.mBuffer = asset.bytes;
	}

	public static ByteReader Open(string path)
	{
		return null;
	}

	private static string ReadLine(byte[] buffer, int start, int count)
	{
		return Encoding.UTF8.GetString(buffer, start, count);
	}

	public string ReadLine()
	{
		return this.ReadLine(true);
	}

	public string ReadLine(bool skipEmptyLines)
	{
		int num = this.mBuffer.Length;
		if (skipEmptyLines)
		{
			while (this.mOffset < num && this.mBuffer[this.mOffset] < 32)
			{
				this.mOffset++;
			}
		}
		int i = this.mOffset;
		if (i < num)
		{
			while (i < num)
			{
				int num2 = (int)this.mBuffer[i++];
				if (num2 == 10 || num2 == 13)
				{
					IL_62:
					string result = ByteReader.ReadLine(this.mBuffer, this.mOffset, i - this.mOffset - 1);
					this.mOffset = i;
					return result;
				}
			}
			i++;
			goto IL_62;
		}
		this.mOffset = num;
		return null;
	}

	public Dictionary<string, string> ReadDictionary()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		char[] array = new char[]
		{
			'='
		};
		while (this.canRead)
		{
			string text = this.ReadLine();
			if (text == null)
			{
				break;
			}
			if (!text.StartsWith("//"))
			{
				string[] array2 = text.Split(array, 2, 1);
				if (array2.Length == 2)
				{
					string key = array2[0].Trim();
					string value = array2[1].Trim().Replace("\\n", "\n");
					dictionary[key] = value;
				}
			}
		}
		return dictionary;
	}

	public BetterList<string> ReadCSV()
	{
		ByteReader.mTemp.Clear();
		string text = "";
		bool flag = false;
		int num = 0;
		while (this.canRead)
		{
			if (flag)
			{
				string text2 = this.ReadLine(false);
				if (text2 == null)
				{
					return null;
				}
				text2 = text2.Replace("\\n", "\n");
				text = text + "\n" + text2;
			}
			else
			{
				text = this.ReadLine(true);
				if (text == null)
				{
					return null;
				}
				text = text.Replace("\\n", "\n");
				num = 0;
			}
			int i = num;
			int length = text.get_Length();
			while (i < length)
			{
				char c = text.get_Chars(i);
				if (c == ',')
				{
					if (!flag)
					{
						ByteReader.mTemp.Add(text.Substring(num, i - num));
						num = i + 1;
					}
				}
				else if (c == '"')
				{
					if (flag)
					{
						if (i + 1 >= length)
						{
							ByteReader.mTemp.Add(text.Substring(num, i - num).Replace("\"\"", "\""));
							return ByteReader.mTemp;
						}
						if (text.get_Chars(i + 1) != '"')
						{
							ByteReader.mTemp.Add(text.Substring(num, i - num).Replace("\"\"", "\""));
							flag = false;
							if (text.get_Chars(i + 1) == ',')
							{
								i++;
								num = i + 1;
							}
						}
						else
						{
							i++;
						}
					}
					else
					{
						num = i + 1;
						flag = true;
					}
				}
				i++;
			}
			if (num < text.get_Length())
			{
				if (flag)
				{
					continue;
				}
				ByteReader.mTemp.Add(text.Substring(num, text.get_Length() - num));
			}
			return ByteReader.mTemp;
		}
		return null;
	}

	protected internal ByteReader(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((ByteReader)GCHandledObjects.GCHandleToObject(instance)).canRead);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(ByteReader.Open(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((ByteReader)GCHandledObjects.GCHandleToObject(instance)).ReadCSV());
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((ByteReader)GCHandledObjects.GCHandleToObject(instance)).ReadDictionary());
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((ByteReader)GCHandledObjects.GCHandleToObject(instance)).ReadLine());
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((ByteReader)GCHandledObjects.GCHandleToObject(instance)).ReadLine(*(sbyte*)args != 0));
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(ByteReader.ReadLine((byte[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), *(int*)(args + 1), *(int*)(args + 2)));
	}
}

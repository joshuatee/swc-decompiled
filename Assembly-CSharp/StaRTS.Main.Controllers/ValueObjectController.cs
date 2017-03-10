using StaRTS.Main.Models;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class ValueObjectController
	{
		private Dictionary<string, List<StrIntPair>> strIntPairLists;

		private Dictionary<string, SequencePair> sequencePairs;

		public ValueObjectController()
		{
			Service.Set<ValueObjectController>(this);
			this.strIntPairLists = new Dictionary<string, List<StrIntPair>>();
			this.sequencePairs = new Dictionary<string, SequencePair>();
		}

		public List<StrIntPair> GetStrIntPairs(string uid, string blob)
		{
			if (string.IsNullOrEmpty(blob))
			{
				return null;
			}
			if (this.strIntPairLists.ContainsKey(blob))
			{
				return this.strIntPairLists[blob];
			}
			List<StrIntPair> list = this.ParseBlob(uid, blob);
			this.strIntPairLists.Add(blob, list);
			return list;
		}

		public SequencePair GetGunSequences(string uid, string rawSequence)
		{
			if (string.IsNullOrEmpty(rawSequence))
			{
				Service.Get<StaRTSLogger>().Warn("Blank gunSequence in " + uid);
				return this.GetDefaultGunSequence(uid);
			}
			if (this.sequencePairs.ContainsKey(rawSequence))
			{
				return this.sequencePairs[rawSequence];
			}
			string[] array = rawSequence.Split(new char[]
			{
				','
			});
			int num = array.Length;
			if (num == 0)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Empty gunSequence in {0} '{1}'", new object[]
				{
					uid,
					rawSequence
				});
				return this.GetDefaultGunSequence(uid);
			}
			int[] array2 = new int[num];
			for (int i = 0; i < num; i++)
			{
				int num2;
				if (!int.TryParse(array[i], ref num2))
				{
					Service.Get<StaRTSLogger>().WarnFormat("Illegal gunSequence value in {0} '{1}'", new object[]
					{
						uid,
						rawSequence
					});
					return this.GetDefaultGunSequence(uid);
				}
				array2[i] = num2;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int j = 0; j < num; j++)
			{
				int num3 = array2[j];
				if (dictionary.ContainsKey(num3))
				{
					Dictionary<int, int> arg_ED_0 = dictionary;
					int key = num3;
					int num4 = arg_ED_0[key];
					arg_ED_0[key] = num4 + 1;
				}
				else
				{
					dictionary[num3] = 1;
				}
			}
			SequencePair sequencePair = new SequencePair(array2, dictionary);
			this.sequencePairs.Add(rawSequence, sequencePair);
			return sequencePair;
		}

		private SequencePair GetDefaultGunSequence(string uid)
		{
			return this.GetGunSequences(uid, "1");
		}

		private List<StrIntPair> ParseBlob(string uid, string blob)
		{
			List<StrIntPair> result = null;
			List<StrIntPair> list = null;
			StrIntState strIntState = StrIntState.StrStart;
			int num = -1;
			string strKey = null;
			int i = 0;
			int length = blob.get_Length();
			while (i < length)
			{
				char c = blob.get_Chars(i);
				bool flag = c == '"';
				switch (strIntState)
				{
				case StrIntState.StrStart:
					if (flag)
					{
						num = i + 1;
						strIntState++;
					}
					else if (c != ',' && !char.IsWhiteSpace(c))
					{
						this.AddError(ref list, "Missing initial quote");
						num = i;
						strIntState++;
					}
					break;
				case StrIntState.StrEnd:
					if (flag)
					{
						if (i == num)
						{
							this.AddError(ref list, "Empty string inside");
						}
						strKey = blob.Substring(num, i - num);
						num = -1;
						strIntState++;
					}
					else if (c == ':')
					{
						this.AddError(ref list, "Missing final quote");
						strKey = blob.Substring(num, i - num);
						num = -1;
						strIntState++;
					}
					break;
				case StrIntState.IntStart:
					if (char.IsDigit(c) || (num == -1 && c == '-'))
					{
						num = i;
						strIntState++;
					}
					else if (c != ':' && !char.IsWhiteSpace(c))
					{
						this.AddError(ref list, "Unsupported delimiter");
					}
					break;
				case StrIntState.IntEnd:
					if (!char.IsDigit(c))
					{
						string text = blob.Substring(num, i - num);
						this.AddPair(ref result, strKey, int.Parse(text));
						num = -1;
						strIntState = StrIntState.StrStart;
						if (flag)
						{
							if (i == length - 1)
							{
								this.AddError(ref list, "Extra quote");
							}
							else
							{
								this.AddError(ref list, "Missing comma");
								num = i + 1;
								strIntState++;
							}
						}
					}
					break;
				}
				i++;
			}
			if (strIntState != StrIntState.StrStart)
			{
				if (strIntState == StrIntState.IntEnd)
				{
					string text2 = blob.Substring(num);
					this.AddPair(ref result, strKey, int.Parse(text2));
				}
				else
				{
					this.AddError(ref list, "String-int mismatch");
				}
			}
			if (list != null)
			{
				string text3 = string.Format("Formatting errors in {0} '{1}'", new object[]
				{
					uid,
					blob
				});
				int j = 0;
				int count = list.Count;
				while (j < count)
				{
					StrIntPair strIntPair = list[j];
					text3 += string.Format("; {0} ({1})", new object[]
					{
						strIntPair.StrKey,
						strIntPair.IntVal
					});
					j++;
				}
				Service.Get<StaRTSLogger>().Warn(text3);
			}
			return result;
		}

		private void AddPair(ref List<StrIntPair> list, string strKey, int intVal)
		{
			if (list == null)
			{
				list = new List<StrIntPair>();
			}
			list.Add(new StrIntPair(strKey, intVal));
		}

		private void AddError(ref List<StrIntPair> errors, string error)
		{
			if (errors == null)
			{
				errors = new List<StrIntPair>();
			}
			else
			{
				int i = 0;
				int count = errors.Count;
				while (i < count)
				{
					StrIntPair strIntPair = errors[i];
					if (strIntPair.StrKey == error)
					{
						strIntPair.IntVal++;
						return;
					}
					i++;
				}
			}
			errors.Add(new StrIntPair(error, 1));
		}

		protected internal ValueObjectController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ValueObjectController)GCHandledObjects.GCHandleToObject(instance)).GetDefaultGunSequence(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ValueObjectController)GCHandledObjects.GCHandleToObject(instance)).GetGunSequences(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ValueObjectController)GCHandledObjects.GCHandleToObject(instance)).GetStrIntPairs(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ValueObjectController)GCHandledObjects.GCHandleToObject(instance)).ParseBlob(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}
	}
}

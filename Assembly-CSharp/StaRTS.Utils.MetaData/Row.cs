using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils.MetaData
{
	public class Row
	{
		private Sheet dataSheet;

		private Sheet masterSheet;

		private int startIndex;

		private List<Row> patchRows;

		private bool hasPatches;

		private bool remapColumns;

		public string Uid
		{
			get;
			private set;
		}

		public Row(string uid, Sheet sheet, int rowStartIndex)
		{
			this.Uid = uid;
			this.dataSheet = sheet;
			this.masterSheet = sheet;
			this.startIndex = rowStartIndex;
			this.patchRows = null;
			this.hasPatches = false;
			this.remapColumns = false;
		}

		public void PatchColumns(Row row)
		{
			if (row.hasPatches)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Cannot patch with a row {0} that has patches", new object[]
				{
					row.Uid
				});
				return;
			}
			if (!this.hasPatches)
			{
				this.patchRows = new List<Row>();
				this.hasPatches = true;
			}
			this.patchRows.Add(row);
		}

		public void InternalSetMasterSheet(Sheet sheet)
		{
			this.masterSheet = sheet;
			this.remapColumns = (this.masterSheet != this.dataSheet);
		}

		public void Invalidate()
		{
			this.dataSheet = null;
			this.masterSheet = null;
			this.patchRows = null;
		}

		public string TryGetString(int column)
		{
			return this.TryGetString(column, null);
		}

		public string TryGetString(int column, string fallback)
		{
			string result;
			if (this.hasPatches)
			{
				for (int i = this.patchRows.Count - 1; i >= 0; i--)
				{
					Row row = this.patchRows[i];
					int columnIndex = column;
					if (this.RemapColumn(row, ref columnIndex) && row.dataSheet.InternalGetString(row.startIndex, columnIndex, out result))
					{
						return result;
					}
				}
			}
			if (this.remapColumns && !this.RemapColumn(this, ref column))
			{
				return fallback;
			}
			if (!this.dataSheet.InternalGetString(this.startIndex, column, out result))
			{
				return fallback;
			}
			return result;
		}

		public bool TryGetBool(int column)
		{
			bool result;
			if (this.hasPatches)
			{
				for (int i = this.patchRows.Count - 1; i >= 0; i--)
				{
					Row row = this.patchRows[i];
					int columnIndex = column;
					if (this.RemapColumn(row, ref columnIndex) && row.dataSheet.InternalGetBool(row.startIndex, columnIndex, out result))
					{
						return result;
					}
				}
			}
			if (this.remapColumns && !this.RemapColumn(this, ref column))
			{
				return false;
			}
			this.dataSheet.InternalGetBool(this.startIndex, column, out result);
			return result;
		}

		public int TryGetInt(int column)
		{
			return this.TryGetInt(column, 0);
		}

		public int TryGetInt(int column, int fallback)
		{
			int result;
			if (this.hasPatches)
			{
				for (int i = this.patchRows.Count - 1; i >= 0; i--)
				{
					Row row = this.patchRows[i];
					int columnIndex = column;
					if (this.RemapColumn(row, ref columnIndex) && row.dataSheet.InternalGetInt(row.startIndex, columnIndex, out result))
					{
						return result;
					}
				}
			}
			if (this.remapColumns && !this.RemapColumn(this, ref column))
			{
				return fallback;
			}
			if (!this.dataSheet.InternalGetInt(this.startIndex, column, out result))
			{
				return fallback;
			}
			return result;
		}

		public uint TryGetUint(int column)
		{
			return this.TryGetUint(column, 0u);
		}

		public uint TryGetUint(int column, uint fallback)
		{
			uint result;
			if (this.hasPatches)
			{
				for (int i = this.patchRows.Count - 1; i >= 0; i--)
				{
					Row row = this.patchRows[i];
					int columnIndex = column;
					if (this.RemapColumn(row, ref columnIndex) && row.dataSheet.InternalGetUint(row.startIndex, columnIndex, out result))
					{
						return result;
					}
				}
			}
			if (this.remapColumns && !this.RemapColumn(this, ref column))
			{
				return fallback;
			}
			if (!this.dataSheet.InternalGetUint(this.startIndex, column, out result))
			{
				return fallback;
			}
			return result;
		}

		public float TryGetFloat(int column)
		{
			return this.TryGetFloat(column, 0f);
		}

		public float TryGetFloat(int column, float fallback)
		{
			float result;
			if (this.hasPatches)
			{
				for (int i = this.patchRows.Count - 1; i >= 0; i--)
				{
					Row row = this.patchRows[i];
					int columnIndex = column;
					if (this.RemapColumn(row, ref columnIndex) && row.dataSheet.InternalGetFloat(row.startIndex, columnIndex, out result))
					{
						return result;
					}
				}
			}
			if (this.remapColumns && !this.RemapColumn(this, ref column))
			{
				return fallback;
			}
			if (!this.dataSheet.InternalGetFloat(this.startIndex, column, out result))
			{
				return fallback;
			}
			return result;
		}

		public string[] TryGetStringArray(int column)
		{
			string[] result;
			if (this.hasPatches)
			{
				for (int i = this.patchRows.Count - 1; i >= 0; i--)
				{
					Row row = this.patchRows[i];
					int columnIndex = column;
					if (this.RemapColumn(row, ref columnIndex) && row.dataSheet.InternalGetStringArray(row.startIndex, columnIndex, out result))
					{
						return result;
					}
				}
			}
			if (this.remapColumns && !this.RemapColumn(this, ref column))
			{
				return null;
			}
			this.dataSheet.InternalGetStringArray(this.startIndex, column, out result);
			return result;
		}

		public int[] TryGetIntArray(int column)
		{
			string text = this.TryGetString(column);
			if (text == null)
			{
				return null;
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			int num = array.Length;
			int[] array2 = new int[num];
			for (int i = 0; i < num; i++)
			{
				int num2;
				if (int.TryParse(array[i], ref num2))
				{
					array2[i] = num2;
				}
				else
				{
					array2[i] = 0;
				}
			}
			return array2;
		}

		public float[] TryGetFloatArray(int column)
		{
			string text = this.TryGetString(column);
			if (text == null)
			{
				return null;
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			int num = array.Length;
			float[] array2 = new float[num];
			for (int i = 0; i < num; i++)
			{
				float num2;
				if (float.TryParse(array[i], 167, CultureInfo.InvariantCulture, ref num2))
				{
					array2[i] = num2;
				}
				else
				{
					array2[i] = 0f;
				}
			}
			return array2;
		}

		public Vector3 TryGetVector3(int column)
		{
			return this.TryGetVector3(column, Vector3.zero);
		}

		public Vector3 TryGetVector3(int column, Vector3 fallback)
		{
			float[] array = this.TryGetFloatArray(column);
			if (array == null || array.Length != 3)
			{
				return fallback;
			}
			return new Vector3(array[0], array[1], array[2]);
		}

		public string TryGetHexValueString(int column)
		{
			string text = this.TryGetString(column);
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			int length = text.get_Length();
			if (length < 6)
			{
				int num = 6 - length;
				text = new string('0', num) + text;
			}
			return text;
		}

		private bool RemapColumn(Row row, ref int columnIndex)
		{
			string columnName = this.masterSheet.GetColumnName(columnIndex);
			columnIndex = row.dataSheet.GetColumnIndex(columnName);
			return columnIndex >= 0;
		}

		protected internal Row(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((Row)GCHandledObjects.GCHandleToObject(instance)).InternalSetMasterSheet((Sheet)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Row)GCHandledObjects.GCHandleToObject(instance)).Invalidate();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Row)GCHandledObjects.GCHandleToObject(instance)).PatchColumns((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Row)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetBool(*(int*)args));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetFloat(*(int*)args));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetFloat(*(int*)args, *(float*)(args + 1)));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetFloatArray(*(int*)args));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetHexValueString(*(int*)args));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetInt(*(int*)args));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetInt(*(int*)args, *(int*)(args + 1)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetIntArray(*(int*)args));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetString(*(int*)args));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetString(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetStringArray(*(int*)args));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetVector3(*(int*)args));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Row)GCHandledObjects.GCHandleToObject(instance)).TryGetVector3(*(int*)args, *(*(IntPtr*)(args + 1))));
		}
	}
}

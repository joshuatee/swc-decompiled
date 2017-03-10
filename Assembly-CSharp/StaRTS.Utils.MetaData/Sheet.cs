using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Utils.MetaData
{
	public class Sheet
	{
		private const int COLUMN_UID = 0;

		private const string COLUMN_PREFIX = "COLUMN_";

		private Column[] columns;

		private Dictionary<string, int> columnIndexes;

		private List<string> extraColumnNames;

		private string[] strings;

		private float[] floats;

		private string[][] arrays;

		private uint[] cells;

		private int rowCount;

		private int columnCount;

		private int stringCount;

		private int floatCount;

		private int arrayCount;

		private int cellCount;

		private Dictionary<string, Row> rows;

		public string SheetName
		{
			get;
			private set;
		}

		public Sheet(string sheetName, string[] strings, float[] floats, string[][] arrays)
		{
			this.SheetName = sheetName;
			this.strings = strings;
			this.floats = floats;
			this.arrays = arrays;
			this.stringCount = strings.Length;
			this.floatCount = floats.Length;
			this.arrayCount = arrays.Length;
		}

		public void SetupColumns(Column[] columns)
		{
			this.columns = columns;
			this.columnCount = columns.Length;
			this.columnIndexes = new Dictionary<string, int>(this.columnCount);
			for (int i = 0; i < this.columnCount; i++)
			{
				this.columnIndexes.Add(columns[i].ColName, i);
			}
		}

		public void SetupCells(uint[] cells)
		{
			this.cells = cells;
			this.cellCount = cells.Length;
		}

		public void SetupComplete()
		{
			this.rowCount = ((this.columnCount == 0) ? 0 : (this.cellCount / this.columnCount));
			this.rows = new Dictionary<string, Row>(this.rowCount);
			for (int i = 0; i < this.rowCount; i++)
			{
				int num = i * this.columnCount;
				string text;
				if (!this.InternalGetString(num, 0, out text))
				{
					Service.Get<StaRTSLogger>().WarnFormat("Unable to get uid for row {0} in sheet {1}", new object[]
					{
						num,
						this.SheetName
					});
				}
				else if (string.IsNullOrEmpty(text))
				{
					Service.Get<StaRTSLogger>().WarnFormat("Ignoring empty row {0} uid in sheet {1}", new object[]
					{
						num,
						this.SheetName
					});
				}
				else if (this.rows.ContainsKey(text))
				{
					Service.Get<StaRTSLogger>().WarnFormat("Ignoring duplicate row {0} uid {1} in sheet {2}", new object[]
					{
						num,
						text,
						this.SheetName
					});
				}
				else
				{
					Row value = new Row(text, this, num);
					this.rows.Add(text, value);
				}
			}
		}

		public void PatchRows(Sheet sheet)
		{
			if (!this.VerifyValid())
			{
				return;
			}
			Dictionary<string, Row> allRows = sheet.GetAllRows();
			if (allRows != null)
			{
				foreach (KeyValuePair<string, Row> current in allRows)
				{
					string key = current.get_Key();
					Row value = current.get_Value();
					value.InternalSetMasterSheet(this);
					if (this.rows.ContainsKey(key))
					{
						this.rows[key].PatchColumns(value);
					}
					else
					{
						this.rows.Add(key, value);
					}
				}
			}
		}

		public void Invalidate()
		{
			this.columns = null;
			this.columnIndexes = null;
			this.extraColumnNames = null;
			this.strings = null;
			this.floats = null;
			this.arrays = null;
			this.cells = null;
			this.rows = null;
		}

		private bool VerifyValid()
		{
			if (this.rows == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Sheet {0} is invalid", new object[]
				{
					this.SheetName
				});
				return false;
			}
			return true;
		}

		public Dictionary<string, Row> GetAllRows()
		{
			if (!this.VerifyValid())
			{
				return null;
			}
			return this.rows;
		}

		public Column[] InternalGetAllColumns()
		{
			return this.columns;
		}

		public string GetColumnName(int columnIndex)
		{
			if (columnIndex >= 0)
			{
				if (columnIndex < this.columnCount)
				{
					return this.columns[columnIndex].ColName;
				}
				if (this.extraColumnNames != null)
				{
					columnIndex -= this.columnCount;
					if (columnIndex < this.extraColumnNames.Count)
					{
						return this.extraColumnNames[columnIndex];
					}
				}
			}
			return null;
		}

		public int GetColumnIndex(string columnName)
		{
			if (columnName == null || !this.columnIndexes.ContainsKey(columnName))
			{
				return -1;
			}
			return this.columnIndexes[columnName];
		}

		public void SetupColumnIndexes<T>() where T : IValueObject
		{
			if (!this.VerifyValid())
			{
				return;
			}
			global::BindingFlags bindingFlags = (global::BindingFlags)33;
			PropertyInfo[] properties = typeof(T).GetProperties(bindingFlags);
			int i = 0;
			int num = properties.Length;
			while (i < num)
			{
				PropertyInfo propertyInfo = properties[i];
				if (propertyInfo.PropertyType == typeof(int))
				{
					string name = propertyInfo.Name;
					if (name.StartsWith("COLUMN_"))
					{
						string text = name.Substring("COLUMN_".get_Length());
						int num2;
						if (this.columnIndexes.ContainsKey(text))
						{
							num2 = this.columnIndexes[text];
						}
						else
						{
							if (this.extraColumnNames == null)
							{
								this.extraColumnNames = new List<string>();
							}
							num2 = this.columnCount + this.extraColumnNames.Count;
							this.columnIndexes.Add(text, num2);
							this.extraColumnNames.Add(text);
						}
						propertyInfo.SetValue(null, num2, null);
					}
				}
				i++;
			}
		}

		public bool InternalGetString(int rowStartIndex, int columnIndex, out string value)
		{
			value = null;
			if (columnIndex < 0 || columnIndex >= this.columnCount)
			{
				return false;
			}
			int num = rowStartIndex + columnIndex;
			if (num < 0 || num >= this.cellCount)
			{
				return false;
			}
			uint num2 = this.cells[num];
			if (num2 == 0u)
			{
				return false;
			}
			Column column = this.columns[columnIndex];
			switch (column.ColType)
			{
			case ColumnType.String:
			{
				int num3 = (int)(num2 - 1u);
				if (num3 < 0 || num3 >= this.stringCount)
				{
					return false;
				}
				value = this.strings[num3];
				return true;
			}
			case ColumnType.Boolean:
			{
				uint num4 = num2 - 1u;
				value = (num4 > 0u).ToString();
				return true;
			}
			case ColumnType.NonNegativeInt:
			case ColumnType.RawInt:
				value = ((int)(((num2 & 2147483648u) == 0u) ? (num2 - 1u) : num2)).ToString();
				return true;
			case ColumnType.Float:
			{
				int num5 = (int)(num2 - 1u);
				if (num5 < 0 || num5 >= this.floatCount)
				{
					return false;
				}
				value = this.floats[num5].ToString();
				return true;
			}
			case ColumnType.StringArray:
				return false;
			default:
				return false;
			}
		}

		public bool InternalGetStringArray(int rowStartIndex, int columnIndex, out string[] value)
		{
			value = null;
			if (columnIndex < 0 || columnIndex >= this.columnCount)
			{
				return false;
			}
			int num = rowStartIndex + columnIndex;
			if (num < 0 || num >= this.cellCount)
			{
				return false;
			}
			uint num2 = this.cells[num];
			if (num2 == 0u)
			{
				return false;
			}
			Column column = this.columns[columnIndex];
			ColumnType colType = column.ColType;
			if (colType != ColumnType.StringArray)
			{
				return false;
			}
			int num3 = (int)(num2 - 1u);
			if (num3 < 0 || num3 >= this.arrayCount)
			{
				return false;
			}
			value = this.arrays[num3];
			return true;
		}

		public bool InternalGetBool(int rowStartIndex, int columnIndex, out bool value)
		{
			value = false;
			if (columnIndex < 0 || columnIndex >= this.columnCount)
			{
				return false;
			}
			int num = rowStartIndex + columnIndex;
			if (num < 0 || num >= this.cellCount)
			{
				return false;
			}
			uint num2 = this.cells[num];
			if (num2 == 0u)
			{
				return false;
			}
			Column column = this.columns[columnIndex];
			switch (column.ColType)
			{
			case ColumnType.String:
			case ColumnType.StringArray:
				value = true;
				return true;
			case ColumnType.Boolean:
			case ColumnType.NonNegativeInt:
			{
				uint num3 = num2 - 1u;
				value = (num3 > 0u);
				return true;
			}
			case ColumnType.RawInt:
				value = ((((num2 & 2147483648u) == 0u) ? (num2 - 1u) : num2) > 0u);
				return true;
			case ColumnType.Float:
			{
				int num4 = (int)(num2 - 1u);
				if (num4 < 0 || num4 >= this.floatCount)
				{
					return false;
				}
				value = (this.floats[num4] != 0f);
				return true;
			}
			default:
				return false;
			}
		}

		public bool InternalGetInt(int rowStartIndex, int columnIndex, out int value)
		{
			value = 0;
			if (columnIndex < 0 || columnIndex >= this.columnCount)
			{
				return false;
			}
			int num = rowStartIndex + columnIndex;
			if (num < 0 || num >= this.cellCount)
			{
				return false;
			}
			uint num2 = this.cells[num];
			if (num2 == 0u)
			{
				return false;
			}
			Column column = this.columns[columnIndex];
			switch (column.ColType)
			{
			case ColumnType.String:
			{
				int num3 = (int)(num2 - 1u);
				return num3 >= 0 && num3 < this.stringCount && int.TryParse(this.strings[num3], ref value);
			}
			case ColumnType.Boolean:
			case ColumnType.NonNegativeInt:
			case ColumnType.RawInt:
				value = (int)(((num2 & 2147483648u) == 0u) ? (num2 - 1u) : num2);
				return true;
			case ColumnType.Float:
			{
				int num4 = (int)(num2 - 1u);
				if (num4 < 0 || num4 >= this.floatCount)
				{
					return false;
				}
				value = (int)this.floats[num4];
				return true;
			}
			case ColumnType.StringArray:
				return false;
			default:
				return false;
			}
		}

		public bool InternalGetUint(int rowStartIndex, int columnIndex, out uint value)
		{
			value = 0u;
			if (columnIndex < 0 || columnIndex >= this.columnCount)
			{
				return false;
			}
			int num = rowStartIndex + columnIndex;
			if (num < 0 || num >= this.cellCount)
			{
				return false;
			}
			uint num2 = this.cells[num];
			if (num2 == 0u)
			{
				return false;
			}
			Column column = this.columns[columnIndex];
			switch (column.ColType)
			{
			case ColumnType.String:
			{
				int num3 = (int)(num2 - 1u);
				return num3 >= 0 && num3 < this.stringCount && uint.TryParse(this.strings[num3], ref value);
			}
			case ColumnType.Boolean:
			case ColumnType.NonNegativeInt:
			case ColumnType.RawInt:
				value = (((num2 & 2147483648u) == 0u) ? (num2 - 1u) : num2);
				return true;
			case ColumnType.Float:
			{
				int num4 = (int)(num2 - 1u);
				if (num4 < 0 || num4 >= this.floatCount)
				{
					return false;
				}
				value = (uint)this.floats[num4];
				return true;
			}
			case ColumnType.StringArray:
				return false;
			default:
				return false;
			}
		}

		public bool InternalGetFloat(int rowStartIndex, int columnIndex, out float value)
		{
			value = 0f;
			if (columnIndex < 0 || columnIndex >= this.columnCount)
			{
				return false;
			}
			int num = rowStartIndex + columnIndex;
			if (num < 0 || num >= this.cellCount)
			{
				return false;
			}
			uint num2 = this.cells[num];
			if (num2 == 0u)
			{
				return false;
			}
			Column column = this.columns[columnIndex];
			switch (column.ColType)
			{
			case ColumnType.String:
			{
				int num3 = (int)(num2 - 1u);
				return num3 >= 0 && num3 < this.stringCount && float.TryParse(this.strings[num3], ref value);
			}
			case ColumnType.Boolean:
			case ColumnType.NonNegativeInt:
			case ColumnType.RawInt:
			{
				int num4 = (int)(((num2 & 2147483648u) == 0u) ? (num2 - 1u) : num2);
				value = (float)num4;
				return true;
			}
			case ColumnType.Float:
			{
				int num5 = (int)(num2 - 1u);
				if (num5 < 0 || num5 >= this.floatCount)
				{
					return false;
				}
				value = this.floats[num5];
				return true;
			}
			case ColumnType.StringArray:
				return false;
			default:
				return false;
			}
		}

		protected internal Sheet(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Sheet)GCHandledObjects.GCHandleToObject(instance)).SheetName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Sheet)GCHandledObjects.GCHandleToObject(instance)).GetAllRows());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Sheet)GCHandledObjects.GCHandleToObject(instance)).GetColumnIndex(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Sheet)GCHandledObjects.GCHandleToObject(instance)).GetColumnName(*(int*)args));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Sheet)GCHandledObjects.GCHandleToObject(instance)).InternalGetAllColumns());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((Sheet)GCHandledObjects.GCHandleToObject(instance)).Invalidate();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((Sheet)GCHandledObjects.GCHandleToObject(instance)).PatchRows((Sheet)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((Sheet)GCHandledObjects.GCHandleToObject(instance)).SheetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((Sheet)GCHandledObjects.GCHandleToObject(instance)).SetupColumns((Column[])GCHandledObjects.GCHandleToPinnedArrayObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((Sheet)GCHandledObjects.GCHandleToObject(instance)).SetupComplete();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Sheet)GCHandledObjects.GCHandleToObject(instance)).VerifyValid());
		}
	}
}

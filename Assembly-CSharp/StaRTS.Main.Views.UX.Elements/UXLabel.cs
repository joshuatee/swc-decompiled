using StaRTS.Main.Views.Cameras;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXLabel : UXElement
	{
		private UXLabelComponent component;

		public Color OriginalTextColor
		{
			get;
			private set;
		}

		public virtual string Text
		{
			get
			{
				return this.component.Text;
			}
			set
			{
				this.component.Text = value;
			}
		}

		public Color TextColor
		{
			get
			{
				return this.component.TextColor;
			}
			set
			{
				this.component.TextColor = value;
			}
		}

		public int FontSize
		{
			get
			{
				return this.component.FontSize;
			}
			set
			{
				this.component.FontSize = value;
			}
		}

		public Font Font
		{
			get
			{
				return this.component.Font;
			}
			set
			{
				this.component.Font = value;
			}
		}

		public float TotalHeight
		{
			get
			{
				return this.component.TotalHeight * this.uxCamera.Scale;
			}
		}

		public float LineHeight
		{
			get
			{
				return Mathf.Round(this.component.LineHeight * this.uxCamera.Scale);
			}
		}

		public float TextWidth
		{
			get
			{
				return Mathf.Round(this.component.TextWidth * this.uxCamera.Scale);
			}
		}

		public UIWidget.Pivot Pivot
		{
			get
			{
				return this.component.Pivot;
			}
			set
			{
				this.component.Pivot = value;
			}
		}

		public string GetURLAtPosition
		{
			get
			{
				return this.component.NGUILabel.GetUrlAtPosition(UICamera.lastWorldPosition);
			}
		}

		public bool UseFontSharpening
		{
			get
			{
				return this.component.UseFontSharpening;
			}
			set
			{
				this.component.UseFontSharpening = value;
			}
		}

		public UXLabel(UXCamera uxCamera, UXLabelComponent component) : base(uxCamera, component.gameObject, null)
		{
			this.component = component;
			this.OriginalTextColor = component.TextColor;
			if (Service.Get<Lang>().IsKorean())
			{
				string text = SystemInfo.operatingSystem.ToLower();
				if (text.Contains("phone") && text.Contains("(10."))
				{
					component.NGUILabel.trueTypeFont = Service.Get<Lang>().CustomKoreanFont;
				}
			}
		}

		public override void InternalDestroyComponent()
		{
			this.component.Label = null;
			UnityEngine.Object.Destroy(this.component);
		}

		public int CalculateLineCount()
		{
			float lineHeight = this.LineHeight;
			if (lineHeight != 0f)
			{
				return (int)Mathf.Round(this.TotalHeight / this.LineHeight);
			}
			return 0;
		}

		public override void OnDestroyElement()
		{
			this.component.TextColor = this.OriginalTextColor;
			base.OnDestroyElement();
		}

		protected internal UXLabel(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).CalculateLineCount());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).Font);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).FontSize);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).GetURLAtPosition);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).LineHeight);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).OriginalTextColor);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).Pivot);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).Text);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).TextColor);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).TextWidth);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).TotalHeight);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabel)GCHandledObjects.GCHandleToObject(instance)).UseFontSharpening);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UXLabel)GCHandledObjects.GCHandleToObject(instance)).InternalDestroyComponent();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((UXLabel)GCHandledObjects.GCHandleToObject(instance)).OnDestroyElement();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((UXLabel)GCHandledObjects.GCHandleToObject(instance)).Font = (Font)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((UXLabel)GCHandledObjects.GCHandleToObject(instance)).FontSize = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((UXLabel)GCHandledObjects.GCHandleToObject(instance)).OriginalTextColor = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((UXLabel)GCHandledObjects.GCHandleToObject(instance)).Pivot = (UIWidget.Pivot)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((UXLabel)GCHandledObjects.GCHandleToObject(instance)).Text = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((UXLabel)GCHandledObjects.GCHandleToObject(instance)).TextColor = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((UXLabel)GCHandledObjects.GCHandleToObject(instance)).UseFontSharpening = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}

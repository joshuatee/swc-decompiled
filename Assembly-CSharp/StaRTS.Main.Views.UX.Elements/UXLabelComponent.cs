using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Elements
{
	public class UXLabelComponent : MonoBehaviour, IUnitySerializable
	{
		public UILabel NGUILabel
		{
			get;
			set;
		}

		public UXLabel Label
		{
			get;
			set;
		}

		public string Text
		{
			get
			{
				if (!(this.NGUILabel == null))
				{
					return this.NGUILabel.text;
				}
				return null;
			}
			set
			{
				if (this.NGUILabel != null)
				{
					this.NGUILabel.text = value;
				}
			}
		}

		public Color TextColor
		{
			get
			{
				if (!(this.NGUILabel == null))
				{
					return this.NGUILabel.color;
				}
				return Color.white;
			}
			set
			{
				if (this.NGUILabel != null)
				{
					this.NGUILabel.color = value;
				}
			}
		}

		public int FontSize
		{
			get
			{
				if (!(this.NGUILabel == null))
				{
					return this.NGUILabel.fontSize;
				}
				return 0;
			}
			set
			{
				if (this.NGUILabel != null)
				{
					this.NGUILabel.fontSize = value;
				}
			}
		}

		public Font Font
		{
			get
			{
				if (!(this.NGUILabel == null))
				{
					return this.NGUILabel.trueTypeFont;
				}
				return null;
			}
			set
			{
				if (this.NGUILabel != null)
				{
					this.NGUILabel.trueTypeFont = value;
				}
			}
		}

		public float TotalHeight
		{
			get
			{
				if (!(this.NGUILabel == null))
				{
					return this.NGUILabel.localSize.y;
				}
				return 0f;
			}
		}

		public float LineHeight
		{
			get
			{
				if (!(this.NGUILabel == null))
				{
					return (float)this.NGUILabel.fontSize;
				}
				return 0f;
			}
		}

		public float TextWidth
		{
			get
			{
				if (!(this.NGUILabel == null))
				{
					return (float)this.NGUILabel.width;
				}
				return 0f;
			}
		}

		public UIWidget.Pivot Pivot
		{
			get
			{
				if (!(this.NGUILabel == null))
				{
					return this.NGUILabel.pivot;
				}
				return UIWidget.Pivot.Center;
			}
			set
			{
				if (this.NGUILabel != null)
				{
					this.NGUILabel.pivot = value;
				}
			}
		}

		public bool UseFontSharpening
		{
			get
			{
				return this.NGUILabel == null || this.NGUILabel.UseFontSharpening;
			}
			set
			{
				if (this.NGUILabel != null)
				{
					this.NGUILabel.UseFontSharpening = value;
				}
			}
		}

		public UXLabelComponent()
		{
		}

		public override void Unity_Serialize(int depth)
		{
		}

		public override void Unity_Deserialize(int depth)
		{
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public override void Unity_NamedSerialize(int depth)
		{
		}

		public override void Unity_NamedDeserialize(int depth)
		{
		}

		protected internal UXLabelComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Font);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).FontSize);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Label);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).LineHeight);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).NGUILabel);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Pivot);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Text);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).TextColor);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).TextWidth);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).TotalHeight);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).UseFontSharpening);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Font = (Font)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).FontSize = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Label = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).NGUILabel = (UILabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Pivot = (UIWidget.Pivot)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Text = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).TextColor = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).UseFontSharpening = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((UXLabelComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}

using StaRTS.Main.Views.Cameras;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using UnityEngine;

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
			if (Service.Get<Lang>().IsJapanese())
			{
				component.NGUILabel.trueTypeFont = Service.Get<Lang>().CustomJapaneseFont;
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
			return (lineHeight != 0f) ? ((int)Mathf.Round(this.TotalHeight / this.LineHeight)) : 0;
		}

		public override void OnDestroyElement()
		{
			this.component.TextColor = this.OriginalTextColor;
			base.OnDestroyElement();
		}
	}
}

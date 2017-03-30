using StaRTS.Externals.Manimal;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StaRTS.Main.Views.UX.Controls
{
	public class CountdownControl : IEventObserver, IViewClockTimeObserver
	{
		private UXLabel label;

		private string localizedFormat;

		private Color originalLabelColor;

		private int deadline;

		private KeyValuePair<int, Color> threshold;

		public CountdownControl(UXLabel label, string localizedFormat)
		{
			this.init(label, localizedFormat);
		}

		public CountdownControl(UXLabel label, string localizedFormat, int deadline)
		{
			this.init(label, localizedFormat);
			this.SetDeadline(deadline);
		}

		private void init(UXLabel label, string localizedFormat)
		{
			this.localizedFormat = localizedFormat;
			this.label = label;
			this.originalLabelColor = label.TextColor;
			this.label.SendDestroyEvent = true;
			Service.Get<EventManager>().RegisterObserver(this, EventId.ElementDestroyed);
		}

		public void OnViewClockTime(float dt)
		{
			int num = Math.Max(0, this.deadline - (int)Service.Get<ServerAPI>().ServerTime);
			if (this.label != null)
			{
				string timeLabelFromSeconds = GameUtils.GetTimeLabelFromSeconds(num);
				this.label.Text = string.Format(this.localizedFormat, timeLabelFromSeconds);
				this.label.TextColor = ((num >= this.threshold.Key) ? this.originalLabelColor : this.threshold.Value);
			}
			if (num <= 0)
			{
				this.Destroy();
			}
		}

		public void SetThreshold(int thresholdSeconds, Color color)
		{
			this.threshold = new KeyValuePair<int, Color>(thresholdSeconds, color);
		}

		public void SetDeadline(int deadline)
		{
			this.deadline = deadline;
			Service.Get<ViewTimeEngine>().RegisterClockTimeObserver(this, 1f);
			this.OnViewClockTime(0f);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (this.label != null && (UXLabel)cookie == this.label)
			{
				this.Destroy();
			}
			return EatResponse.NotEaten;
		}

		public void Destroy()
		{
			Service.Get<ViewTimeEngine>().UnregisterClockTimeObserver(this);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ElementDestroyed);
			this.label = null;
			this.deadline = 0;
			this.localizedFormat = null;
		}
	}
}

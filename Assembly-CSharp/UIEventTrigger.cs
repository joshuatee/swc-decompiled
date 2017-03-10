using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Event Trigger")]
public class UIEventTrigger : MonoBehaviour, IUnitySerializable
{
	public static UIEventTrigger current;

	public List<EventDelegate> onHoverOver;

	public List<EventDelegate> onHoverOut;

	public List<EventDelegate> onPress;

	public List<EventDelegate> onRelease;

	public List<EventDelegate> onSelect;

	public List<EventDelegate> onDeselect;

	public List<EventDelegate> onClick;

	public List<EventDelegate> onDoubleClick;

	public List<EventDelegate> onDragStart;

	public List<EventDelegate> onDragEnd;

	public List<EventDelegate> onDragOver;

	public List<EventDelegate> onDragOut;

	public List<EventDelegate> onDrag;

	public bool isColliderEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	private void OnHover(bool isOver)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (isOver)
		{
			EventDelegate.Execute(this.onHoverOver);
		}
		else
		{
			EventDelegate.Execute(this.onHoverOut);
		}
		UIEventTrigger.current = null;
	}

	private void OnPress(bool pressed)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (pressed)
		{
			EventDelegate.Execute(this.onPress);
		}
		else
		{
			EventDelegate.Execute(this.onRelease);
		}
		UIEventTrigger.current = null;
	}

	private void OnSelect(bool selected)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (selected)
		{
			EventDelegate.Execute(this.onSelect);
		}
		else
		{
			EventDelegate.Execute(this.onDeselect);
		}
		UIEventTrigger.current = null;
	}

	private void OnClick()
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onClick);
		UIEventTrigger.current = null;
	}

	private void OnDoubleClick()
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDoubleClick);
		UIEventTrigger.current = null;
	}

	private void OnDragStart()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragStart);
		UIEventTrigger.current = null;
	}

	private void OnDragEnd()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragEnd);
		UIEventTrigger.current = null;
	}

	private void OnDragOver(GameObject go)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOver);
		UIEventTrigger.current = null;
	}

	private void OnDragOut(GameObject go)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOut);
		UIEventTrigger.current = null;
	}

	private void OnDrag(Vector2 delta)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDrag);
		UIEventTrigger.current = null;
	}

	public UIEventTrigger()
	{
		this.onHoverOver = new List<EventDelegate>();
		this.onHoverOut = new List<EventDelegate>();
		this.onPress = new List<EventDelegate>();
		this.onRelease = new List<EventDelegate>();
		this.onSelect = new List<EventDelegate>();
		this.onDeselect = new List<EventDelegate>();
		this.onClick = new List<EventDelegate>();
		this.onDoubleClick = new List<EventDelegate>();
		this.onDragStart = new List<EventDelegate>();
		this.onDragEnd = new List<EventDelegate>();
		this.onDragOver = new List<EventDelegate>();
		this.onDragOut = new List<EventDelegate>();
		this.onDrag = new List<EventDelegate>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			if (this.onHoverOver == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onHoverOver.Count);
				for (int i = 0; i < this.onHoverOver.Count; i++)
				{
					((this.onHoverOver[i] != null) ? this.onHoverOver[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onHoverOut == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onHoverOut.Count);
				for (int i = 0; i < this.onHoverOut.Count; i++)
				{
					((this.onHoverOut[i] != null) ? this.onHoverOut[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onPress == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onPress.Count);
				for (int i = 0; i < this.onPress.Count; i++)
				{
					((this.onPress[i] != null) ? this.onPress[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onRelease == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onRelease.Count);
				for (int i = 0; i < this.onRelease.Count; i++)
				{
					((this.onRelease[i] != null) ? this.onRelease[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onSelect == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onSelect.Count);
				for (int i = 0; i < this.onSelect.Count; i++)
				{
					((this.onSelect[i] != null) ? this.onSelect[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDeselect == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onDeselect.Count);
				for (int i = 0; i < this.onDeselect.Count; i++)
				{
					((this.onDeselect[i] != null) ? this.onDeselect[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onClick == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onClick.Count);
				for (int i = 0; i < this.onClick.Count; i++)
				{
					((this.onClick[i] != null) ? this.onClick[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDoubleClick == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onDoubleClick.Count);
				for (int i = 0; i < this.onDoubleClick.Count; i++)
				{
					((this.onDoubleClick[i] != null) ? this.onDoubleClick[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDragStart == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onDragStart.Count);
				for (int i = 0; i < this.onDragStart.Count; i++)
				{
					((this.onDragStart[i] != null) ? this.onDragStart[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDragEnd == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onDragEnd.Count);
				for (int i = 0; i < this.onDragEnd.Count; i++)
				{
					((this.onDragEnd[i] != null) ? this.onDragEnd[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDragOver == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onDragOver.Count);
				for (int i = 0; i < this.onDragOver.Count; i++)
				{
					((this.onDragOver[i] != null) ? this.onDragOver[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDragOut == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onDragOut.Count);
				for (int i = 0; i < this.onDragOut.Count; i++)
				{
					((this.onDragOut[i] != null) ? this.onDragOut[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDrag == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onDrag.Count);
				for (int i = 0; i < this.onDrag.Count; i++)
				{
					((this.onDrag[i] != null) ? this.onDrag[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onHoverOver = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				eventDelegate.Unity_Deserialize(depth + 1);
				this.onHoverOver.Add(eventDelegate);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onHoverOut = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate2 = new EventDelegate();
				eventDelegate2.Unity_Deserialize(depth + 1);
				this.onHoverOut.Add(eventDelegate2);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onPress = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate3 = new EventDelegate();
				eventDelegate3.Unity_Deserialize(depth + 1);
				this.onPress.Add(eventDelegate3);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onRelease = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate4 = new EventDelegate();
				eventDelegate4.Unity_Deserialize(depth + 1);
				this.onRelease.Add(eventDelegate4);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onSelect = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate5 = new EventDelegate();
				eventDelegate5.Unity_Deserialize(depth + 1);
				this.onSelect.Add(eventDelegate5);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onDeselect = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate6 = new EventDelegate();
				eventDelegate6.Unity_Deserialize(depth + 1);
				this.onDeselect.Add(eventDelegate6);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onClick = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate7 = new EventDelegate();
				eventDelegate7.Unity_Deserialize(depth + 1);
				this.onClick.Add(eventDelegate7);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onDoubleClick = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate8 = new EventDelegate();
				eventDelegate8.Unity_Deserialize(depth + 1);
				this.onDoubleClick.Add(eventDelegate8);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onDragStart = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate9 = new EventDelegate();
				eventDelegate9.Unity_Deserialize(depth + 1);
				this.onDragStart.Add(eventDelegate9);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onDragEnd = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate10 = new EventDelegate();
				eventDelegate10.Unity_Deserialize(depth + 1);
				this.onDragEnd.Add(eventDelegate10);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onDragOver = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate11 = new EventDelegate();
				eventDelegate11.Unity_Deserialize(depth + 1);
				this.onDragOver.Add(eventDelegate11);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onDragOut = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate12 = new EventDelegate();
				eventDelegate12.Unity_Deserialize(depth + 1);
				this.onDragOut.Add(eventDelegate12);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onDrag = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate13 = new EventDelegate();
				eventDelegate13.Unity_Deserialize(depth + 1);
				this.onDrag.Add(eventDelegate13);
			}
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (depth <= 7)
		{
			if (this.onHoverOver != null)
			{
				for (int i = 0; i < this.onHoverOver.Count; i++)
				{
					EventDelegate eventDelegate = this.onHoverOver[i];
					if (eventDelegate != null)
					{
						eventDelegate.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onHoverOut != null)
			{
				for (int i = 0; i < this.onHoverOut.Count; i++)
				{
					EventDelegate eventDelegate2 = this.onHoverOut[i];
					if (eventDelegate2 != null)
					{
						eventDelegate2.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onPress != null)
			{
				for (int i = 0; i < this.onPress.Count; i++)
				{
					EventDelegate eventDelegate3 = this.onPress[i];
					if (eventDelegate3 != null)
					{
						eventDelegate3.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onRelease != null)
			{
				for (int i = 0; i < this.onRelease.Count; i++)
				{
					EventDelegate eventDelegate4 = this.onRelease[i];
					if (eventDelegate4 != null)
					{
						eventDelegate4.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onSelect != null)
			{
				for (int i = 0; i < this.onSelect.Count; i++)
				{
					EventDelegate eventDelegate5 = this.onSelect[i];
					if (eventDelegate5 != null)
					{
						eventDelegate5.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDeselect != null)
			{
				for (int i = 0; i < this.onDeselect.Count; i++)
				{
					EventDelegate eventDelegate6 = this.onDeselect[i];
					if (eventDelegate6 != null)
					{
						eventDelegate6.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onClick != null)
			{
				for (int i = 0; i < this.onClick.Count; i++)
				{
					EventDelegate eventDelegate7 = this.onClick[i];
					if (eventDelegate7 != null)
					{
						eventDelegate7.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDoubleClick != null)
			{
				for (int i = 0; i < this.onDoubleClick.Count; i++)
				{
					EventDelegate eventDelegate8 = this.onDoubleClick[i];
					if (eventDelegate8 != null)
					{
						eventDelegate8.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDragStart != null)
			{
				for (int i = 0; i < this.onDragStart.Count; i++)
				{
					EventDelegate eventDelegate9 = this.onDragStart[i];
					if (eventDelegate9 != null)
					{
						eventDelegate9.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDragEnd != null)
			{
				for (int i = 0; i < this.onDragEnd.Count; i++)
				{
					EventDelegate eventDelegate10 = this.onDragEnd[i];
					if (eventDelegate10 != null)
					{
						eventDelegate10.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDragOver != null)
			{
				for (int i = 0; i < this.onDragOver.Count; i++)
				{
					EventDelegate eventDelegate11 = this.onDragOver[i];
					if (eventDelegate11 != null)
					{
						eventDelegate11.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDragOut != null)
			{
				for (int i = 0; i < this.onDragOut.Count; i++)
				{
					EventDelegate eventDelegate12 = this.onDragOut[i];
					if (eventDelegate12 != null)
					{
						eventDelegate12.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onDrag != null)
			{
				for (int i = 0; i < this.onDrag.Count; i++)
				{
					EventDelegate eventDelegate13 = this.onDrag[i];
					if (eventDelegate13 != null)
					{
						eventDelegate13.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			if (this.onHoverOver == null)
			{
				ISerializedNamedStateWriter arg_29_0 = SerializedNamedStateWriter.Instance;
				var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
				var_0_cp_1 = 0;
				arg_29_0.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 770, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 770, this.onHoverOver.Count);
				for (int i = 0; i < this.onHoverOver.Count; i++)
				{
					EventDelegate arg_92_0 = (this.onHoverOver[i] != null) ? this.onHoverOver[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_92_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onHoverOut == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 782, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 782, this.onHoverOut.Count);
				for (int i = 0; i < this.onHoverOut.Count; i++)
				{
					EventDelegate arg_149_0 = (this.onHoverOut[i] != null) ? this.onHoverOut[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_149_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onPress == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 793, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 793, this.onPress.Count);
				for (int i = 0; i < this.onPress.Count; i++)
				{
					EventDelegate arg_200_0 = (this.onPress[i] != null) ? this.onPress[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_200_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onRelease == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 801, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 801, this.onRelease.Count);
				for (int i = 0; i < this.onRelease.Count; i++)
				{
					EventDelegate arg_2B7_0 = (this.onRelease[i] != null) ? this.onRelease[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_2B7_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onSelect == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 811, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 811, this.onSelect.Count);
				for (int i = 0; i < this.onSelect.Count; i++)
				{
					EventDelegate arg_36E_0 = (this.onSelect[i] != null) ? this.onSelect[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_36E_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onDeselect == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 820, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 820, this.onDeselect.Count);
				for (int i = 0; i < this.onDeselect.Count; i++)
				{
					EventDelegate arg_425_0 = (this.onDeselect[i] != null) ? this.onDeselect[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_425_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onClick == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 257, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 257, this.onClick.Count);
				for (int i = 0; i < this.onClick.Count; i++)
				{
					EventDelegate arg_4DC_0 = (this.onClick[i] != null) ? this.onClick[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_4DC_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onDoubleClick == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 831, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 831, this.onDoubleClick.Count);
				for (int i = 0; i < this.onDoubleClick.Count; i++)
				{
					EventDelegate arg_593_0 = (this.onDoubleClick[i] != null) ? this.onDoubleClick[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_593_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onDragStart == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 845, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 845, this.onDragStart.Count);
				for (int i = 0; i < this.onDragStart.Count; i++)
				{
					EventDelegate arg_64A_0 = (this.onDragStart[i] != null) ? this.onDragStart[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_64A_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onDragEnd == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 857, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 857, this.onDragEnd.Count);
				for (int i = 0; i < this.onDragEnd.Count; i++)
				{
					EventDelegate arg_701_0 = (this.onDragEnd[i] != null) ? this.onDragEnd[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_701_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onDragOver == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 867, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 867, this.onDragOver.Count);
				for (int i = 0; i < this.onDragOver.Count; i++)
				{
					EventDelegate arg_7B8_0 = (this.onDragOver[i] != null) ? this.onDragOver[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_7B8_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onDragOut == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 878, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 878, this.onDragOut.Count);
				for (int i = 0; i < this.onDragOut.Count; i++)
				{
					EventDelegate arg_86F_0 = (this.onDragOut[i] != null) ? this.onDragOut[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_86F_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onDrag == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 888, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 888, this.onDrag.Count);
				for (int i = 0; i < this.onDrag.Count; i++)
				{
					EventDelegate arg_926_0 = (this.onDrag[i] != null) ? this.onDrag[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_926_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateReader arg_1E_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			int num = arg_1E_0.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 770);
			this.onHoverOver = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_4A_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_4A_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onHoverOver.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 782);
			this.onHoverOut = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate2 = new EventDelegate();
				EventDelegate arg_BC_0 = eventDelegate2;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_BC_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onHoverOut.Add(eventDelegate2);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 793);
			this.onPress = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate3 = new EventDelegate();
				EventDelegate arg_12F_0 = eventDelegate3;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_12F_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onPress.Add(eventDelegate3);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 801);
			this.onRelease = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate4 = new EventDelegate();
				EventDelegate arg_1A2_0 = eventDelegate4;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_1A2_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onRelease.Add(eventDelegate4);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 811);
			this.onSelect = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate5 = new EventDelegate();
				EventDelegate arg_215_0 = eventDelegate5;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_215_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onSelect.Add(eventDelegate5);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 820);
			this.onDeselect = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate6 = new EventDelegate();
				EventDelegate arg_288_0 = eventDelegate6;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_288_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onDeselect.Add(eventDelegate6);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 257);
			this.onClick = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate7 = new EventDelegate();
				EventDelegate arg_2FB_0 = eventDelegate7;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_2FB_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onClick.Add(eventDelegate7);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 831);
			this.onDoubleClick = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate8 = new EventDelegate();
				EventDelegate arg_36E_0 = eventDelegate8;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_36E_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onDoubleClick.Add(eventDelegate8);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 845);
			this.onDragStart = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate9 = new EventDelegate();
				EventDelegate arg_3E1_0 = eventDelegate9;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_3E1_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onDragStart.Add(eventDelegate9);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 857);
			this.onDragEnd = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate10 = new EventDelegate();
				EventDelegate arg_454_0 = eventDelegate10;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_454_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onDragEnd.Add(eventDelegate10);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 867);
			this.onDragOver = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate11 = new EventDelegate();
				EventDelegate arg_4C7_0 = eventDelegate11;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_4C7_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onDragOver.Add(eventDelegate11);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 878);
			this.onDragOut = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate12 = new EventDelegate();
				EventDelegate arg_53A_0 = eventDelegate12;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_53A_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onDragOut.Add(eventDelegate12);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 888);
			this.onDrag = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate13 = new EventDelegate();
				EventDelegate arg_5AD_0 = eventDelegate13;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_5AD_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onDrag.Add(eventDelegate13);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
	}

	protected internal UIEventTrigger(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).isColliderEnabled);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).OnDoubleClick();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).OnDragEnd();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).OnDragOut((GameObject)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).OnDragOver((GameObject)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).OnDragStart();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).OnHover(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).OnSelect(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIEventTrigger)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}

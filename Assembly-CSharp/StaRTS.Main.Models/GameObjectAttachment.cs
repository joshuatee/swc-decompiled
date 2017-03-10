using System;
using UnityEngine;

namespace StaRTS.Main.Models
{
	public class GameObjectAttachment
	{
		public string Key;

		public GameObject AttachedGameObject;

		public Vector3 Offset;

		public bool FloorPin;

		public bool CenterOfMassPin;

		public GameObjectAttachment()
		{
		}

		protected internal GameObjectAttachment(UIntPtr dummy)
		{
		}
	}
}

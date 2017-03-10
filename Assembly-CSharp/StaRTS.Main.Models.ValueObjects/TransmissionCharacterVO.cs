using StaRTS.Utils;
using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TransmissionCharacterVO : IValueObject
	{
		public static int COLUMN_planetId
		{
			get;
			private set;
		}

		public static int COLUMN_faction
		{
			get;
			private set;
		}

		public static int COLUMN_characterName
		{
			get;
			private set;
		}

		public static int COLUMN_characterId
		{
			get;
			private set;
		}

		public static int COLUMN_image
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string PlanetId
		{
			get;
			set;
		}

		public FactionType Faction
		{
			get;
			set;
		}

		public string CharacterName
		{
			get;
			set;
		}

		public string CharacterId
		{
			get;
			set;
		}

		public string Image
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.PlanetId = row.TryGetString(TransmissionCharacterVO.COLUMN_planetId);
			this.CharacterName = row.TryGetString(TransmissionCharacterVO.COLUMN_characterName);
			this.CharacterId = row.TryGetString(TransmissionCharacterVO.COLUMN_characterId);
			this.Image = row.TryGetString(TransmissionCharacterVO.COLUMN_image);
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(TransmissionCharacterVO.COLUMN_faction));
		}

		public TransmissionCharacterVO()
		{
		}

		protected internal TransmissionCharacterVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).CharacterId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).CharacterName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionCharacterVO.COLUMN_characterId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionCharacterVO.COLUMN_characterName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionCharacterVO.COLUMN_faction);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionCharacterVO.COLUMN_image);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionCharacterVO.COLUMN_planetId);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).Image);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).CharacterId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).CharacterName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			TransmissionCharacterVO.COLUMN_characterId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			TransmissionCharacterVO.COLUMN_characterName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			TransmissionCharacterVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			TransmissionCharacterVO.COLUMN_image = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			TransmissionCharacterVO.COLUMN_planetId = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).Image = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).PlanetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((TransmissionCharacterVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}

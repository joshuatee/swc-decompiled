using StaRTS.Main.Models.ValueObjects;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Controllers
{
	public interface IDataController
	{
		void Add<T>(string uid, T vo) where T : IValueObject;

		T Get<T>(string uid) where T : IValueObject;

		T GetOptional<T>(string uid) where T : IValueObject;

		Dictionary<string, T>.ValueCollection GetAll<T>() where T : IValueObject;

		void Unload<T>() where T : IValueObject;

		void Exterminate();
	}
}

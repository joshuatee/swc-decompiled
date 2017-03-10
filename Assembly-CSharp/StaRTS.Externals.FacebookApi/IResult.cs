using System;

namespace StaRTS.Externals.FacebookApi
{
	public interface IResult
	{
		string Error
		{
			get;
		}

		string RawResult
		{
			get;
		}
	}
}

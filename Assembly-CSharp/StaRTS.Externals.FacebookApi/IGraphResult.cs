using System;
using UnityEngine;

namespace StaRTS.Externals.FacebookApi
{
	public interface IGraphResult : IResult
	{
		Texture2D Texture
		{
			get;
		}
	}
}

using System;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Exception for the various parts of the game.
	/// </summary>
	public class WordplayException
	: Exception
	{
		public WordplayException(string msg)
			: base(msg)
		{
		}

		public WordplayException(string msg, Exception e)
			: base(msg, e)
		{
		}
	}
}

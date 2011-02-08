using System;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Defines the arguments for a token removed.
	/// </summary>
	public class TokenArgs
	: EventArgs
	{
		public Token Token;
		public bool IsBurnt = false;

		public TokenArgs(Token token)
		{
			Token = token;
		}
	}

	/// <summary>
	/// The handler for tokens.
	/// </summary>
	public delegate void TokenHandler(object sender, TokenArgs args);
}

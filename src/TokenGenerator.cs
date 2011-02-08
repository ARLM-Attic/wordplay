using C5;
using MfGames.Utility;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Handles the generation and assignment of the token types and
	/// letters for the game. As the game goes on, the weights and
	/// values are reduced close to "near random".
	/// </summary>
	public class TokenGenerator
	: Logable
	{
		private WeightedSelector types = new WeightedSelector();

		/// <summary>
		/// Constructs the token generator and sets up the various
		/// scores and frequencies.
		/// </summary>
		public TokenGenerator()
		{
			// Register the types
			types[TokenType.Normal] = 200;
			types[TokenType.Burning] = 20;
			types[TokenType.Poisoned] = 10;
			types[TokenType.Contagious] = 10;
			types[TokenType.Flooded] = 10;
			types[TokenType.Copper] = 15;
			types[TokenType.Silver] = 10;
			types[TokenType.Gold] = 5;
		}

		/// <summary>
		/// Generates a token.
		/// </summary>
		public Token CreateToken(int row, int column)
		{
			// Generate the values
			TokenType type = (TokenType) types.RandomObject;
			char value = Game.Language.CreateValue();

			// Make sure we aren't near the bottom (that is being
			// rude to the player).
			if (row > Game.Board.Rows / 2)
				type = TokenType.Normal;

			// Decrease the probability
			if (types[type] > 1)
				types[type]--;

			// Return the results
			return new Token(type, value, row, column);
		}
	}
}

#region Copyright and License

// Copyright (c) 2009-2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using MfGames.Collections;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Handles the generation and assignment of the token types and
	/// letters for the game. As the game goes on, the weights and
	/// values are reduced close to "near random".
	/// </summary>
	public class TokenGenerator
	{
		private readonly WeightedSelector<TokenType> types = new WeightedSelector<TokenType>();

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
		public Token CreateToken(
			int row,
			int column)
		{
			// Generate the values
			TokenType type = (TokenType) types.GetRandomItem();
			char value = Game.Language.CreateValue();

			// Make sure we aren't near the bottom (that is being
			// rude to the player).
			if (row > Game.Board.Rows / 2)
			{
				type = TokenType.Normal;
			}

			// Decrease the probability
			if (types[type] > 1)
			{
				types[type]--;
			}

			// Return the results
			return new Token(type, value, row, column);
		}
	}
}
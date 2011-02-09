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

#region Namespaces

using System.Text;

using C5;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// Handles the scoring for the game, including both storing the
	/// scores and also handling the scoring subsystem.
	/// </summary>
	public class Score
	{
		#region Score

		/// <summary>
		/// Contains the score of the game.
		/// </summary>
		public int CurrentScore { get; set; }

		/// <summary>
		/// Contains the highest scoring word.
		/// </summary>
		public string HighestWord { get; set; }

		/// <summary>
		/// Contains the score of the highest word.
		/// </summary>
		public int HighestWordScore { get; set; }

		/// <summary>
		/// Contains the longest word selected.
		/// </summary>
		public string LongestWord { get; set; }

		/// <summary>
		/// Contains the total number of words.
		/// </summary>
		public int TotalWords { get; set; }

		#endregion

		#region Scoring

		/// <summary>
		/// Gets the multiplier for the given chain.
		/// </summary>
		public int GetMultiplier(IList<Token> chain)
		{
			int multiplier = 1;

			foreach (Token token in chain)
			{
				switch (token.Type)
				{
					case TokenType.Silver:
						multiplier += 1;
						break;
					case TokenType.Gold:
						multiplier += 2;
						break;
					case TokenType.Contagious:
						multiplier -= 1;
						break;
				}
			}

			return multiplier;
		}

		/// <summary>
		/// Gets the score for a given chain. This returns 
		/// </summary>
		public int? GetScore(
			IList<Token> chain,
			bool modifiers)
		{
			// Ignore too short
			if (chain.Count < 3)
			{
				return null;
			}

			// Get the string value and the multiplier
			StringBuilder text = new StringBuilder();
			int? score = null;

			foreach (Token token in chain)
			{
				// Add to the chain
				text.Append(token.Value);

				// Get the score for the chain
				int localScore = Game.Language.GetScore(token.Value);

				if (modifiers)
				{
					// Figure out any alterations
					switch (token.Type)
					{
						case TokenType.Copper:
							localScore *= 2;
							break;
						case TokenType.Silver:
							localScore *= 3;
							break;
						case TokenType.Gold:
							localScore *= 4;
							break;
						case TokenType.Poisoned:
							localScore = 0;
							break;
					}
				}

				// Add the score
				if (score == null)
				{
					score = 0;
				}

				score += localScore;
			}

			// Check for sanity
			string word = text.ToString();

			if (!Game.Language.IsValidWord(word))
			{
				return null;
			}

			// Return the score
			return score * word.Length;
		}

		#endregion
	}
}
using C5;
using System.Text;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Handles the scoring for the game, including both storing the
	/// scores and also handling the scoring subsystem.
	/// </summary>
	public class Score
	{
#region Score
		private int score;
		private string longestWord;
		private string highestWord;
		private int highestWordScore;
		private int totalWords;

		/// <summary>
		/// Contains the score of the game.
		/// </summary>
		public int CurrentScore
		{
			get { return score; }
			set { score = value; }
		}

		/// <summary>
		/// Contains the highest scoring word.
		/// </summary>
		public string HighestWord
		{
			get { return highestWord; }
			set { highestWord = value; }
		}

		/// <summary>
		/// Contains the score of the highest word.
		/// </summary>
		public int HighestWordScore
		{
			get { return highestWordScore; }
			set { highestWordScore = value; }
		}

		/// <summary>
		/// Contains the longest word selected.
		/// </summary>
		public string LongestWord
		{
			get { return longestWord; }
			set { longestWord = value; }
		}

		/// <summary>
		/// Contains the total number of words.
		/// </summary>
		public int TotalWords
		{
			get { return totalWords; }
			set { totalWords = value; }
		}
#endregion

#region Scoring
		/// <summary>
		/// Gets the multiplier for the given chain.
		/// </summary>
		public int GetMultiplier(IList <Token> chain)
		{
			int multiplier = 1;

			foreach (Token token in chain)
			{
				switch (token.Type)
				{
				case TokenType.Silver:     multiplier += 1; break;
				case TokenType.Gold:       multiplier += 2; break;
				case TokenType.Contagious: multiplier -= 1; break;
				}
			}

			return multiplier;
		}

		/// <summary>
		/// Gets the score for a given chain. This returns 
		/// </summary>
		public int? GetScore(IList <Token> chain, bool modifiers)
		{
			// Ignore too short
			if (chain.Count < 3)
				return null;

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
					case TokenType.Copper:     localScore *= 2; break;
					case TokenType.Silver:     localScore *= 3; break;
					case TokenType.Gold:       localScore *= 4; break;
					case TokenType.Poisoned:   localScore = 0; break;
					}
				}
					
				// Add the score
				if (score == null)
					score = 0;

				score += localScore;
			} 

			// Check for sanity
			string word = text.ToString();

			if (!Game.Language.IsValidWord(word))
				return null;

			// Return the score
			return score * word.Length;
		}
#endregion
	}
}

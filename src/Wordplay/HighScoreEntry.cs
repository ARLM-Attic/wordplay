using System;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Contains a single entry into one of the high score lists.
	/// </summary>
	public class HighScoreEntry
	: IComparable <HighScoreEntry>
	{
		private string name;
		private DateTime when;
		private string word;
		private int score;

		/// <summary>
		/// Contains the name of the user that had this score.
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// Contains the numeric score.
		/// </summary>
		public int Score
		{
			get { return score; }
			set { score = value; }
		}

		/// <summary>
		/// When the score was gained.
		/// </summary>
		public DateTime UtcWhen
		{
			get { return when; }
			set { when = value; }
		}

		/// <summary>
		/// Contains the word that generated the score. For the high
		/// score lists that don't use a word, this isn't shown.
		/// </summary>
		public string Word
		{
			get { return word; }
			set { word = value; }
		}

		/// <summary>
		/// Compares the entries.
		/// </summary>
		public int CompareTo(HighScoreEntry hse)
		{
			if (Score != hse.Score)
				return hse.Score.CompareTo(Score);

			return UtcWhen.CompareTo(hse.UtcWhen);
		}
	}
}

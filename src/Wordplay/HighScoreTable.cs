using C5;
using System;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Contains a high score entry for a given condition (selection
	/// type, language, etc). This keeps track of both the conditions
	/// and also the four different types of high scores.
	///
	/// There are four types of high scores: highest total score,
	/// highest number of words, longest word, highest valued word.
	/// </summary>
	public class HighScoreTable
	: IComparable <HighScoreTable>
	{
#region Constants
		public static readonly int MaximumEntries = 10;
#endregion

		/// <summary>
		/// Contains the selection type used for this score.
		/// </summary>
		public SelectionType SelectionType;

		/// <summary>
		/// Contains the board size.
		/// </summary>
		public int BoardSize;

		/// <summary>
		/// Contains the language.
		/// </summary>
		public string LanguageName;

		public LinkedList <HighScoreEntry> TotalScores =
			new LinkedList <HighScoreEntry> ();
		public LinkedList <HighScoreEntry> TotalWords =
			new LinkedList <HighScoreEntry> ();
		public LinkedList <HighScoreEntry> LongestWords =
			new LinkedList <HighScoreEntry> ();
		public LinkedList <HighScoreEntry> HighestWords =
			new LinkedList <HighScoreEntry> ();

		/// <summary>
		/// Compares this table to other tables.
		/// </summary>
		public int CompareTo(HighScoreTable table)
		{
			return ToString().CompareTo(table.ToString());
		}

		/// <summary>
		/// Gives a formatted name of this table.
		/// </summary>
		public override string ToString()
		{
			return String.Format("{0} {1} ({2})",
				Locale.Translate("HighScore/" + SelectionType),
				BoardSize,
				LanguageName);
		}

#region High Score Processing
		/// <summary>
		/// This function adds a high score to a given list.
		/// </summary>
		private void AddHighScore(HighScoreEntry entry,
			LinkedList <HighScoreEntry> list)
		{
			// Don't bother if we aren't on the list
			if (!IsHighScore(entry, list))
				return;

			// Add it, sort it, and remove any excess
			list.Add(entry);
			list.Sort();

			while (list.Count > MaximumEntries)
				list.RemoveLast();
		}

		/// <summary>
		/// Internal function to create the four high score entries.
		/// </summary>
		private HighScoreEntry [] CreateEntries(string username, Score score)
		{
			// Create a list of four entries
			HighScoreEntry [] entries = new HighScoreEntry [4];

			entries[0]         = new HighScoreEntry();
			entries[0].Name    = username;
			entries[0].UtcWhen = DateTime.UtcNow;
			entries[0].Score   = score.CurrentScore;
			
			entries[1]         = new HighScoreEntry();
			entries[1].Name    = username;
			entries[1].UtcWhen = DateTime.UtcNow;
			entries[1].Score   = score.TotalWords;

			entries[2]         = new HighScoreEntry();
			entries[2].Name    = username;
			entries[2].UtcWhen = DateTime.UtcNow;
			entries[2].Word    = score.LongestWord;
			entries[2].Score   = score.LongestWord.Length;

			entries[3]         = new HighScoreEntry();
			entries[3].Name    = username;
			entries[3].UtcWhen = DateTime.UtcNow;
			entries[3].Word    = score.HighestWord;
			entries[3].Score   = score.HighestWordScore;

			// Return the entries
			return entries;
		}

		/// <summary>
		/// Returns true if this score is in the list.
		/// </summary>
		public bool IsHighScore(HighScoreEntry entry,
			LinkedList <HighScoreEntry> list)
		{
			// If we haven't hit the highest entries, then yes
			if (list.Count < MaximumEntries)
				return true;

			// See if our score is higher than the last one
			return list.Last.Score < entry.Score;
		}

		/// <summary>
		/// Returns true if this score is somewhere on the high score
		/// list.
		/// </summary>
		public bool IsHighScore(Score score)
		{
			// Check the entries
			HighScoreEntry [] entries = CreateEntries("<internal>", score);

			if (IsHighScore(entries[0], TotalScores) ||
				IsHighScore(entries[1], TotalWords) ||
				IsHighScore(entries[2], LongestWords) ||
				IsHighScore(entries[3], HighestWords))
			{
				return true;
			}

			// We are not a high score
			return false;
		}

		/// <summary>
		/// Registers a high score on the table. If there is none,
		/// this won't do anything.
		/// </summary>
		public void RegisterScores(string username, Score score)
		{
			// Create the entries
			HighScoreEntry [] entries = CreateEntries(username, score);

			AddHighScore(entries[0], TotalScores);
			AddHighScore(entries[1], TotalWords);
			AddHighScore(entries[2], LongestWords);
			AddHighScore(entries[3], HighestWords);
		}
#endregion
	}
}

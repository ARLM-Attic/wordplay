using C5;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Contains the master list of all high score entries in the
	/// system. This also handles the creation and construction of
	/// high score tables as needed.
	/// </summary>
	public class HighScoreTableList
	{
		/// <summary>
		/// Contains the list of loaded tables.
		/// </summary>
		public LinkedList <HighScoreTable> Tables =
			new LinkedList <HighScoreTable> ();

		/// <summary>
		/// Gets or creates a high score table for the current game
		/// settings.
		/// </summary>
		public HighScoreTable GetTable(Config config)
		{
			// Go through the list
			foreach (HighScoreTable table in Tables)
			{
				// See if it matches
				if (config.SelectionType == table.SelectionType &&
					config.BoardSize     == table.BoardSize &&
					config.LanguageName  == table.LanguageName)
				{
					// This is it
					return table;
				}
			}

			// Create a new one
			HighScoreTable hst = new HighScoreTable();
			hst.SelectionType  = config.SelectionType;
			hst.BoardSize      = config.BoardSize;
			hst.LanguageName   = config.LanguageName;

			// Add and return it
			Tables.Add(hst);
			return hst;
		}
	}
}

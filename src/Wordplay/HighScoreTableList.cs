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

using C5;

#endregion

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
		public LinkedList<HighScoreTable> Tables = new LinkedList<HighScoreTable>();

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
				    config.BoardSize == table.BoardSize &&
				    config.LanguageName == table.LanguageName)
				{
					// This is it
					return table;
				}
			}

			// Create a new one
			HighScoreTable hst = new HighScoreTable();
			hst.SelectionType = config.SelectionType;
			hst.BoardSize = config.BoardSize;
			hst.LanguageName = config.LanguageName;

			// Add and return it
			Tables.Add(hst);
			return hst;
		}
	}
}
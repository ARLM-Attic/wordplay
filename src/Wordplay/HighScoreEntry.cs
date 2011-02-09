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

using System;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// Contains a single entry into one of the high score lists.
	/// </summary>
	public class HighScoreEntry : IComparable<HighScoreEntry>
	{
		/// <summary>
		/// Contains the name of the user that had this score.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Contains the numeric score.
		/// </summary>
		public int Score { get; set; }

		/// <summary>
		/// When the score was gained.
		/// </summary>
		public DateTime UtcWhen { get; set; }

		/// <summary>
		/// Contains the word that generated the score. For the high
		/// score lists that don't use a word, this isn't shown.
		/// </summary>
		public string Word { get; set; }

		/// <summary>
		/// Compares the entries.
		/// </summary>
		public int CompareTo(HighScoreEntry hse)
		{
			if (Score != hse.Score)
			{
				return hse.Score.CompareTo(Score);
			}

			return UtcWhen.CompareTo(hse.UtcWhen);
		}
	}
}
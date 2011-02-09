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
	/// Controls the user-configurable options for the game.
	/// </summary>
	public class Config
	{
		private int boardSize = 7;
		private uint fps = 20;
		private string language = "en-US";
		private string name = Environment.UserName;
		private SelectionType selectionType = SelectionType.Diagonal;
		private string theme = "default";

		/// <summary>
		/// Contains the board size.
		/// </summary>
		public int BoardSize
		{
			get { return boardSize; }
			set
			{
				if (value < 3)
				{
					throw new WordplayException("Board size is too small");
				}

				boardSize = value;
			}
		}

		/// <summary>
		/// Contains the frames per second.
		/// </summary>
		public uint FramesPerSecond
		{
			get { return fps; }
			set
			{
				fps = value;

				if (fps < 1)
				{
					fps = 1;
				}
			}
		}

		/// <summary>
		/// Contains the language.
		/// </summary>
		public Language Language
		{
			get { return new Language(LanguageName); }
		}

		/// <summary>
		/// Contains the language name.
		/// </summary>
		public string LanguageName
		{
			get { return language; }
			set
			{
				if (value == null || value.Length <= 2)
				{
					throw new WordplayException("Cannot set null language");
				}

				language = value;
			}
		}

		/// <summary>
		/// Contains the selection type.
		/// </summary>
		public SelectionType SelectionType
		{
			get { return selectionType; }
			set { selectionType = value; }
		}

		/// <summary>
		/// Contains the theme.
		/// </summary>
		public Theme Theme
		{
			get { return new Theme(ThemeName); }
		}

		/// <summary>
		/// Contains the name of the theme.
		/// </summary>
		public string ThemeName
		{
			get { return theme; }
			set { theme = value; }
		}

		/// <summary>
		/// Contains the name of the user. If this is set to null or
		/// blank, it sets to "Anonymous."
		/// </summary>
		public string UserName
		{
			get { return name; }
			set
			{
				if (value == null || value.Trim() == "")
				{
					name = Locale.Translate("Anonymous");
				}
				else
				{
					name = value;
				}
			}
		}
	}
}
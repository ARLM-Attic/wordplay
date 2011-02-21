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

using System.IO;

using C5;

using MfGames.Sprite;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// Contains the code for retrieving the references to the sound
	/// and image code. A theme does not actually render the themes,
	/// it just identifies the locations and streams needed to do so.
	/// </summary>
	public class Theme
	{
		private DirectoryInfo directory;
		private string themeName;
		private TilesetDrawableFactory tilesetFactory = new TilesetDrawableFactory();

		/// <summary>
		/// Creates the theme with the given theme name.
		/// </summary>
		public Theme(string themeName)
		{
			// Populate our internal values
			this.themeName = themeName;
			directory = new DirectoryInfo(Path.Combine(ThemeDirectory, this.themeName));

			// Do some sanity checking
			if (!directory.Exists)
			{
				throw new WordplayException(
					"Theme directory does not exist: " + directory.FullName);
			}

			// Load in the tileset
			FileInfo tilesetFile =
				new FileInfo(Path.Combine(directory.FullName, "tileset.xml"));
			tilesetFactory.Load(tilesetFile);
		}

		/// <summary>
		/// Contains the drawable factory.
		/// </summary>
		public TilesetDrawableFactory DrawableFactory
		{
			get { return tilesetFactory; }
		}

		/// <summary>
		/// Contains the name of the theme.
		/// </summary>
		public string ThemeName
		{
			get { return themeName; }
			set { themeName = value; }
		}

		#region Constants

		// Used to identify the graphic to use when the Game.State is
		// GameState.NotStarted.

		// Used to identify the graphic used when the game is over
		// (also known as GameState.Completed).
		public static readonly string Completed = "completed";
		public static readonly string NotStarted = "not-started";

		#endregion

		#region Theme

		/// <summary>
		/// Contains the name of the theme directory.
		/// </summary>
		public static string ThemeDirectory
		{
			get { return Path.Combine(Game.BaseDirectory, "themes"); }
		}

		/// <summary>
		/// Returns all themes in the given directory.
		/// </summary>
		public static IList<Theme> GetThemes()
		{
			// Create the list
			LinkedList<Theme> list = new LinkedList<Theme>();

			// Go through the directories
			foreach (string din
				in Directory.GetDirectories(ThemeDirectory))
			{
				// Check for the theme.xml file
				if (!File.Exists(Path.Combine(din, "tileset.xml")))
				{
					continue;
				}

				// We have a theme
				DirectoryInfo di = new DirectoryInfo(din);
				Theme theme = new Theme(di.Name);
				list.Add(theme);
			}

			// Return the results
			return list;
		}

		#endregion
	}
}
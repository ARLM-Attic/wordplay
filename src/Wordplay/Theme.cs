using C5;
using Gdk;
using MfGames.Sprite;
using Rsvg;
using System.IO;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Contains the code for retrieving the references to the sound
	/// and image code. A theme does not actually render the themes,
	/// it just identifies the locations and streams needed to do so.
	/// </summary>
	public class Theme
	{
		private string themeName;
		private DirectoryInfo directory;
		private TilesetDrawableFactory tilesetFactory =
			new TilesetDrawableFactory();

		/// <summary>
		/// Creates the theme with the given theme name.
		/// </summary>
		public Theme(string themeName)
		{
			// Populate our internal values
			this.themeName = themeName;
			this.directory = new DirectoryInfo(
				Path.Combine(ThemeDirectory, this.themeName));

			// Do some sanity checking
			if (!directory.Exists)
				throw new WordplayException("Theme directory does not exist: "
					+ directory.FullName);

			// Load in the tileset
			FileInfo tilesetFile = new FileInfo(
				Path.Combine(directory.FullName, "tileset.xml"));
			tilesetFactory.Load(tilesetFile);

			// Noise
			Debug("Loaded theme: {0}", themeName);
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
		public static readonly string NotStarted = "not-started";

		// Used to identify the graphic used when the game is over
		// (also known as GameState.Completed).
		public static readonly string Completed = "completed";
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
		public static IList <Theme> GetThemes()
		{
			// Create the list
			LinkedList <Theme> list = new LinkedList <Theme> ();

			// Go through the directories
			foreach (string din
				in Directory.GetDirectories(ThemeDirectory))
			{
				// Check for the theme.xml file
				if (!File.Exists(Path.Combine(din, "tileset.xml")))
					continue;

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

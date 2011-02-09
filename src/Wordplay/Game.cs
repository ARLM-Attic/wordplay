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
using System.IO;
using System.Xml.Serialization;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// Static singleton to access the various game components. This
	/// is also the class that is used to serialize the game.
	/// </summary>
	public class Game
	{
		#region Persistence

		/// <summary>
		/// Contains the name of the config.
		/// </summary>
		private static string ConfigFilename
		{
			get
			{
				DirectoryInfo dir = ConfigStorage.Singleton.GetDirectory("Wordplay");
				return Path.Combine(dir.FullName, "config.xml");
			}
		}

		/// <summary>
		/// Contains the name of the high score list.
		/// </summary>
		private static string HighScoreFilename
		{
			get
			{
				return Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory, "high-scores.xml");
			}
		}

		/// <summary>
		/// Loads the game configuration into memory.
		/// </summary>
		public static void LoadConfig()
		{
			// Load the high score list
			try
			{
				// Get the filename
				string filename = HighScoreFilename;
				FileInfo file = new FileInfo(filename);

				// Deserialize it
				TextReader reader = file.OpenText();
				XmlSerializer serializer = new XmlSerializer(typeof(HighScoreTableList));
				highScores = (HighScoreTableList) serializer.Deserialize(reader);
				reader.Close();
			}
			catch
			{
			}

			// Load our configuration
			try
			{
				// Get the filename
				string filename = ConfigFilename;
				FileInfo file = new FileInfo(filename);

				// Deserialize it
				TextReader reader = file.OpenText();
				XmlSerializer serializer = new XmlSerializer(typeof(Config));
				config = (Config) serializer.Deserialize(reader);
				reader.Close();

				// Set the theme
				theme = Config.Theme;
			}
			catch
			{
			}
		}

		/// <summary>
		/// Write the configuration value.
		/// </summary>
		public static void WriteConfig()
		{
			// Write out our configuration
			try
			{
				FileInfo file = new FileInfo(ConfigFilename);

				using (FileStream fs = file.Open(FileMode.Create, FileAccess.Write))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Config));
					serializer.Serialize(fs, config);
				}
			}
			catch (Exception e)
			{
				Logger.Error(typeof(Game), "Cannot write config: {0}", e.Message);
			}

			// Write out our high scores
			try
			{
				FileInfo file = new FileInfo(HighScoreFilename);

				using (FileStream fs = file.Open(FileMode.Create, FileAccess.Write))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(HighScoreTableList));
					serializer.Serialize(fs, highScores);
				}
			}
			catch (Exception e)
			{
				Logger.Error(typeof(Game), "Cannot write high scores: {0}", e.Message);
			}
		}

		#endregion

		#region Properties

		private static Board board;
		private static Config config = new Config();
		private static GameState gameState = GameState.NotStarted;
		private static HighScoreTableList highScores = new HighScoreTableList();
		private static Language language;
		private static Score score = new Score();
		private static Theme theme = new Theme("default");
		private static TokenGenerator tokenGenerator;

		/// <summary>
		/// Returns the resource directory for the game.
		/// </summary>
		public static string BaseDirectory
		{
			get { return AppDomain.CurrentDomain.BaseDirectory; }
		}

		/// <summary>
		/// Contains the loaded board.
		/// </summary>
		public static Board Board
		{
			get { return board; }
		}

		/// <summary>
		/// Contains the configuration object for the game.
		/// </summary>
		public static Config Config
		{
			get { return config; }
		}

		/// <summary>
		/// Contains the display for the game.
		/// </summary>
		public static Display Display { get; set; }

		/// <summary>
		/// Contains the high score table list.
		/// </summary>
		public static HighScoreTableList HighScores
		{
			get { return highScores; }
		}

		/// <summary>
		/// Contains access to the given language.
		/// </summary>
		public static Language Language
		{
			get { return language; }
		}

		/// <summary>
		/// Contains the scoring part of the game.
		/// </summary>
		public static Score Score
		{
			get { return score; }
		}

		/// <summary>
		/// Contains the current state of the game.
		/// </summary>
		public static GameState State
		{
			get { return gameState; }
			set { gameState = value; }
		}

		/// <summary>
		/// Contains the currently loaded theme.
		/// </summary>
		public static Theme Theme
		{
			get { return theme; }
			set { theme = value; }
		}

		/// <summary>
		/// Contains the token generator.
		/// </summary>
		public static TokenGenerator TokenGenerator
		{
			get { return tokenGenerator; }
		}

		#endregion

		#region Setup

		/// <summary>
		/// Constructs a new game.
		/// </summary>
		public static void NewGame()
		{
			// Clear the display
			Display.Sprites.Clear();

			// Create the objects
			theme = Config.Theme;
			language = Config.Language;
			score = new Score();
			tokenGenerator = new TokenGenerator();
			gameState = GameState.Started;
			board = new Board(Config.BoardSize);

			// Register the vents
			board.TokenAdded += Display.OnTokenAdded;
			board.TokenRemoved += Display.OnTokenRemoved;
			board.TokenChanged += Display.OnTokenChanged;

			// Initialize the board
			board.Initialize();
		}

		#endregion
	}
}
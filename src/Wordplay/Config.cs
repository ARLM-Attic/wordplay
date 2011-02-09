namespace MfGames.Wordplay
{
	/// <summary>
	/// Controls the user-configurable options for the game.
	/// </summary>
	public class Config
	{
		private int boardSize = 7;
		private string theme = "default";
		private string language = "en-US";
		private SelectionType selectionType = SelectionType.Diagonal;
		private string name = System.Environment.UserName;
		private uint fps = 20;

		/// <summary>
		/// Contains the board size.
		/// </summary>
		public int BoardSize
		{
			get { return boardSize; }
			set
			{
				if (value < 3)
					throw new WordplayException("Board size is too small");

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
					fps = 1;
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
					throw new WordplayException("Cannot set null language");

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
					name = Locale.Translate("Anonymous");
				else
					name = value;
			}
		}
	}
}

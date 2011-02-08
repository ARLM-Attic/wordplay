using Gtk;
using MfGames.MfGtk;
using System;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Handles the configuration of the game.
	/// </summary>
	public class ConfigDialog
	: Dialog
	{
		private ComboBox themes;
		private ComboBox languages;
		private SpinButton boardSize;
		private EnumComboBox selectionType;
		private SpinButton fps;

		/// <summary>
		/// Constructs the dialog box.
		/// </summary>
		public ConfigDialog()
		{
			// Set up our controls
			Title = Locale.Translate("Configuration Dialog");
			AddButton("Close", ResponseType.Close);
			//Response += new ResponseHandler (on_dialog_response);

			// Add the dialog table
			LabelWidgetTable table = new LabelWidgetTable(2, 2);
			uint row = 0;
			VBox.Add(table);

			// Put in a list of themes
			int i = 0;
			themes = ComboBox.NewText();

			foreach (Theme theme in Theme.GetThemes())
			{
				themes.AppendText(theme.ThemeName);

				if (theme.ThemeName == Game.Config.ThemeName)
					themes.Active = 0;

				i++;
			}

			themes.Changed += OnChanged;

			// Languages
			languages = ComboBox.NewText();
			languages.AppendText("en-US");
			languages.Active = 0;
			languages.Changed += OnChanged;

			// Selection type
			selectionType = new EnumComboBox(typeof(SelectionType));
			selectionType.ActiveEnum = Game.Config.SelectionType;
			selectionType.Changed += OnChanged;

			// Board size
			boardSize = new SpinButton(3f, 27f, 1f);
			boardSize.Value = Game.Config.BoardSize;
			boardSize.Changed += OnChanged;

			// Frames per second
			fps = new SpinButton(1f, 60f, 1f);
			fps.Value = Game.Config.FramesPerSecond;
			fps.Changed += OnChanged;

			// Start with theme configuration
			table.AttachExpanded(row++, "Theme", themes);
			table.AttachExpanded(row++, "Board Size", boardSize);
			table.AttachExpanded(row++, "Selection Type", selectionType);
			table.AttachExpanded(row++, "Language", languages);
			table.AttachExpanded(row++, "FPS", fps);

			// Finish up
			ShowAll();
		}

		/// <summary>
		/// Triggered when the theme changes.
		/// </summary>
		private void OnChanged(object sender, EventArgs args)
		{
			Game.Config.ThemeName = themes.ActiveText;
			Game.Config.LanguageName = languages.ActiveText;
			Game.Config.BoardSize = boardSize.ValueAsInt;
			Game.Config.SelectionType =
				(SelectionType) selectionType.ActiveEnum;
			Game.Config.FramesPerSecond = (uint) fps.ValueAsInt;
		}
	}
}

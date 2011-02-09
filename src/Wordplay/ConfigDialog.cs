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

using Gtk;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// Handles the configuration of the game.
	/// </summary>
	public class ConfigDialog : Dialog
	{
		private readonly SpinButton boardSize;
		private readonly SpinButton fps;
		private readonly ComboBox languages;
		private readonly EnumComboBox selectionType;
		private readonly ComboBox themes;

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
				{
					themes.Active = 0;
				}

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
		private void OnChanged(
			object sender,
			EventArgs args)
		{
			Game.Config.ThemeName = themes.ActiveText;
			Game.Config.LanguageName = languages.ActiveText;
			Game.Config.BoardSize = boardSize.ValueAsInt;
			Game.Config.SelectionType = (SelectionType) selectionType.ActiveEnum;
			Game.Config.FramesPerSecond = (uint) fps.ValueAsInt;
		}
	}
}
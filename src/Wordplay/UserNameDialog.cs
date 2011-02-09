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
	/// Handles the user name entry of the game.
	/// </summary>
	public class UserNameDialog : Dialog
	{
		private readonly Gtk.Entry name;

		/// <summary>
		/// Constructs the dialog box.
		/// </summary>
		public UserNameDialog()
		{
			// Set up our controls
			Title = Locale.Translate("User Name");
			AddButton("Close", ResponseType.Close);
			//Response += new ResponseHandler (on_dialog_response);
			DefaultResponse = ResponseType.Close;

			// Add some text
			string text = Locale.Translate("Score/InHighScore");
			Label l = new Label();
			l.Markup = text;
			name = new Gtk.Entry(Game.Config.UserName);
			name.Changed += OnChanged;
			VBox.PackStart(l, false, false, 0);
			VBox.PackStart(name, false, false, 0);

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
			Game.Config.UserName = name.Text;
		}
	}
}
using Gtk;
using System;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Handles the user name entry of the game.
	/// </summary>
	public class UserNameDialog
	: Dialog
	{
		private Gtk.Entry name;

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
		private void OnChanged(object sender, EventArgs args)
		{
			Game.Config.UserName = name.Text;
		}
	}
}

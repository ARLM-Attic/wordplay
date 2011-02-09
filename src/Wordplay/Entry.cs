using Gtk;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Main entry point into the game. This sets up the logging
	/// subsystem and makes sure all the configuration values are
	/// loaded before creating the window.
	/// </summary>
	public class Entry
	{
		/// <summary>
		/// The main function of this class, this is where the system
		/// enters. It handles the initialization, setup of the
		/// logging subsystems, and setup of the GUI elements.
		/// </summary>
		public static void Main(string [] args)
		{
			// Set up config storage
			ConfigStorage.Singleton = new ConfigStorage("MfGames");
			ConfigStorage.Singleton.InitStorage();
			Game.LoadConfig();

			// Set up the Gtk and our structures
			Application.Init();

			// Create the game window and show it
			GameWindow window = new GameWindow();
			window.Show();

			Theme.GetThemes();

			// Start the Gtk application
			Application.Run();
		}
	}
}

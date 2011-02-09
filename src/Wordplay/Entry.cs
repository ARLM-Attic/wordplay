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

using Gtk;

#endregion

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
		public static void Main(string[] args)
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
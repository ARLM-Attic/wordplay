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

using Gtk;

using MfGames.Sprite;

#endregion

public class ViewerEntry : Window
{
	/// <summary>
	/// Constructs a demo object with the appropriate gui.
	/// </summary>
	public ViewerEntry()
		: base("Moonfire Games' Gtk ViewerEntry")
	{
		// Build the GUI
		uiManager = new UIManager();
		CreateGui();
	}

	#region GUI

	private static Statusbar statusbar;
	private readonly UIManager uiManager;

	/// <summary>
	/// Contains the statusbar for the demo.
	/// </summary>
	public static Statusbar Statusbar
	{
		get { return statusbar; }
	}

	/// <summary>
	/// Creates the GUI interface.
	/// </summary>
	private void CreateGui()
	{
		// Create a window
		SetDefaultSize(512, 512);
		DeleteEvent += OnWindowDelete;

		// Create the window frame
		VBox box = new VBox();
		Add(box);

		// Add the menu
		box.PackStart(CreateGuiMenu(), false, false, 0);

		// Create a notebook
		box.PackStart(new Viewer(), true, true, 0);

		// Add the status bar
		statusbar = new Statusbar();
		statusbar.Push(0, "Welcome!");
		statusbar.HasResizeGrip = true;
		box.PackStart(statusbar, false, false, 0);
	}

	private Widget CreateGuiMenu()
	{
		// Defines the menu
		string uiInfo = "<ui>" + "  <menubar name='MenuBar'>" +
		                "    <menu action='FileMenu'>" +
		                "      <menuitem action='Quit'/>" + "    </menu>" +
		                "  </menubar>" + "</ui>";

		// Set up the actions
		ActionEntry[] entries = new[]
		                        {
		                        	// "File" Menu
		                        	new ActionEntry(
		                        		"FileMenu", null, "_File", null, null, null),
		                        	new ActionEntry(
		                        		"Quit",
		                        		Stock.Quit,
		                        		"_Quit",
		                        		"<control>Q",
		                        		"Quit",
		                        		OnQuitAction),
		                        };

		// Build up the actions
		ActionGroup actions = new ActionGroup("group");
		actions.Add(entries);

		uiManager.InsertActionGroup(actions, 0);
		AddAccelGroup(uiManager.AccelGroup);

		// Set up the interfaces from XML
		uiManager.AddUiFromString(uiInfo);
		return uiManager.GetWidget("/MenuBar");
	}

	#endregion

	#region Events

	/// <summary>
	/// Triggers the quit menu.
	/// </summary>
	private void OnQuitAction(
		object sender,
		EventArgs args)
	{
		Application.Quit();
	}

	/// <summary>
	/// Fired when the window is closed.
	/// </summary>
	private void OnWindowDelete(
		object obj,
		DeleteEventArgs args)
	{
		Application.Quit();
	}

	#endregion

	/// <summary>
	/// Main entry point into the system.
	/// </summary>
	public static void Main(string[] args)
	{
		// Set up Gtk
		Application.Init();

		// Load in the tileset
		// Create the demo
		ViewerEntry demo = new ViewerEntry();
		Viewer.TilesetFactory.Load(new FileInfo(args[0]));

		// Load the sprite
		IDrawable drawable = Viewer.TilesetFactory.Create(args[1]);
		DrawableSprite sprite = new DrawableSprite(drawable);
		sprite.Height = sprite.Width = 64;
		sprite.X = sprite.Y = 10;
		sprite.Randomize();
		Viewer.Sprites.Add(sprite);
		Viewer.Sprite = sprite;

		// Start everything running
		demo.ShowAll();
		Application.Run();
	}
}
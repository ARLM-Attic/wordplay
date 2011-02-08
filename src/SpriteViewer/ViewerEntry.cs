using Gtk;
using Pango;
using MfGames.Sprite;
using System;
using System.IO;
using System.Text;

public class ViewerEntry
: Window
{
	/// <summary>
	/// Main entry point into the system.
	/// </summary>
	public static void Main(string [] args)
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
	private UIManager uiManager;

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
		DeleteEvent += new DeleteEventHandler (OnWindowDelete);

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
		string uiInfo =
			"<ui>" +
			"  <menubar name='MenuBar'>" +
			"    <menu action='FileMenu'>" +
			"      <menuitem action='Quit'/>" +
			"    </menu>" +
			"  </menubar>" +
			"</ui>";

		// Set up the actions
		ActionEntry [] entries = new ActionEntry [] {
			// "File" Menu
			new ActionEntry("FileMenu", null, "_File",
							null, null, null),
			new ActionEntry("Quit", Stock.Quit, "_Quit", "<control>Q",
							"Quit", new EventHandler(OnQuitAction)),
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
	/// Fired when the window is closed.
	/// </summary>
	private void OnWindowDelete(object obj, DeleteEventArgs args)
	{
		Application.Quit();
	}

    /// <summary>
    /// Triggers the quit menu.
    /// </summary>
    private void OnQuitAction(object sender, EventArgs args)
    {
		Application.Quit();
    }
#endregion
}

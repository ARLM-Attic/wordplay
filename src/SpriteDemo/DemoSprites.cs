using C5;
using Gdk;
using Gtk;
using MfGames.Sprite;
using System;
using System.IO;
using System.Reflection;
using System.Text;

public class DemoSprites
: VBox
{
	public static PixbufDrawableFactory PixbufFactory =
		new PixbufDrawableFactory();
	public static TilesetDrawableFactory TilesetFactory =
		new TilesetDrawableFactory();
	public static uint DesiredFps = 15;

	private DrawingArea area;
	private Pixmap pixmap;
	private ComboBox paneList;
	private DemoSpritePane currentPane;
	private SpinButton fps;
	private CheckButton showUpdate;

	private long start = DateTime.UtcNow.Ticks;
	private long exposeCount = 0;
	private long tickCount = 0;
	
	private LinkedList <DemoSpritePane> panes =
		new LinkedList <DemoSpritePane> ();

	/// <summary>
	/// Constructs the sprite drawing area, along with the various
	/// rules needed.
	/// </summary>
	public DemoSprites()
	{
		// Create the drop-down list
		HBox box = new HBox();
		paneList = ComboBox.NewText();
		paneList.Changed += OnPaneChanged;
		fps = new SpinButton(1, 100, 1);
		fps.Value = DesiredFps;
		fps.Changed += OnFpsChanged;
		showUpdate = new CheckButton();
		box.PackStart(paneList, false, false, 0);
		box.PackStart(new Label("FPS"), false, false, 5);
		box.PackStart(fps, false, false, 2);
		box.PackStart(new Label("Show Update"), false, false, 5);
		box.PackStart(showUpdate, false, false, 2);
		box.PackStart(new Label(), true, true, 0);
		PackStart(box, false, false, 2);

		// Create the drawing area and pack it
		area = new DrawingArea();
		area.Realized += OnRealized;
		area.ExposeEvent += OnExposed;
		area.ConfigureEvent += OnConfigure;
		PackStart(area, true, true, 2);

		// Set up the search paths for the factory. We need to do this
		// before the sprite pane loading because the panes use the
		// factory for their own loading.
		string dataPath = AppDomain.CurrentDomain.BaseDirectory + "/images";
		PixbufFactory.SearchPaths.Add(new DirectoryInfo(dataPath));

		string tilesetPath = System.IO.Path.Combine(dataPath, "tileset.xml");
		FileInfo tilesetFile = new FileInfo(tilesetPath);
		TilesetFactory.Load(tilesetFile);

		// Use reflection to load the demos in random order
		LoadSpritePanes();

		// Start up a little animation loop
		GLib.Timeout.Add(1000 / DesiredFps, OnTick);
	}

	/// <summary>
	/// This method uses reflection to find all the classes in this
	/// assembly that are a sprite pane demo and loads them into
	/// memory. The order is undefined, but the results are added to
	/// the drop down list and the first one is selected.
	/// </summary>
	private void LoadSpritePanes()
	{
		// Get our assembly
		Assembly assembly = GetType().Assembly;
		
		foreach (Type type in assembly.GetTypes())
		{
			// Ignore the abstract class
			if (type.IsAbstract)
				continue;

			// Look for the pane
			if (type.BaseType != typeof(DemoSpritePane))
				continue;
			
			// See if we can find a constructor
			ConstructorInfo ci = type.GetConstructor(new Type [] {});
			
			if (ci == null)
			{
				continue;
			}
			
			// Grab the link attribute
			DemoSpritePane dsp = ci.Invoke(new object [] {}) as DemoSpritePane;
			
			if (dsp == null)
			{
				continue;
			}
			
			// Register it
			panes.Add(dsp);
			paneList.AppendText(dsp.Name);
		}

		// Check for a change needed
		if (panes.Count > 0)
			paneList.Active = 0;
	}

	/// <summary>
	/// Triggered when the drawing area is configured.
	/// </summary>
	private void OnConfigure(object obj, ConfigureEventArgs args)
	{
		// Pull out some fields
		EventConfigure ev = args.Event;
		Gdk.Window window = ev.Window;
		int width = Allocation.Width;
		int height = Allocation.Height;
		
		// Create the backing pixmap
		pixmap = new Pixmap(window, width, height, -1);

		// Update the current pane
		// TODO, there is a bug with this and I don't know why
		if (currentPane != null)
			currentPane.Configure(width, height - 50);
		
		// Mark ourselves as done
		args.RetVal = true;
	}
	
	/// <summary>
	/// Triggered when the drawing area is exposed.
	/// </summary>
	private void OnExposed(object sender, ExposeEventArgs args)
	{
		// Clear out the entire graphics area with black (just
		// because we can). This also erases the prior rendering.
		Gdk.Rectangle region = args.Event.Area;
		Gdk.GC gc = new Gdk.GC(pixmap);
		gc.ClipRectangle = region;

		// Check for the current pane
		if (currentPane != null)
			currentPane.Render(pixmap, region);

		// Debugging
		if (showUpdate.Active)
			pixmap.DrawRectangle(Style.WhiteGC, false, region);

		// This performs the actual drawing
		args.Event.Window.DrawDrawable(Style.BlackGC,
			pixmap,
			region.X, region.Y,
			region.X, region.Y,
			region.Width, region.Height);
		args.RetVal = false;
		exposeCount++;
	}

	/// <summary>
	/// This event is called when the FPS changes.
	/// </summary>
	private void OnFpsChanged(object sender, EventArgs args)
	{
		DesiredFps = (uint) fps.Value;
	}

	/// <summary>
	/// Triggered when the pane list is changed.
	/// </summary>
	private void OnPaneChanged(object o, EventArgs args)
	{
		// Change the pane
		currentPane = panes[paneList.Active];
		currentPane.Configure(Allocation.Width, Allocation.Height);

		// Trigger a full redraw
		QueueDraw();
	}

	/// <summary>
	/// Called when the drawing area is realized.
	/// </summary>
	private void OnRealized(object o, EventArgs args)
	{
	}

	/// <summary>
	/// Triggered on the animation loop.
	/// </summary>
	private bool OnTick()
	{
		// Render the pane
		if (currentPane != null)
			currentPane.Update(area);

		// Handle the tick updating
		tickCount++;

		if (tickCount % 100 == 0)
		{
			double diff = (DateTime.UtcNow.Ticks - start) / 10000000.0;
			double efps = (double) exposeCount / (double) diff;
			double tfps = (double) tickCount / (double) diff;
			Demo.Statusbar.Push(0, String.Format
				("FPS: Exposed {0:N1} FPS ({3:N1}%), "
					+ "Ticks {1:N1} FPS ({4:N1}%), "
					+ "Maximum {2:N0} FPS",
					efps, tfps, DesiredFps,
					efps * 100 / DesiredFps,
					tfps * 100 / DesiredFps));
			start = DateTime.UtcNow.Ticks;
			exposeCount = tickCount = 0;
		}

		// Re-request the animation
		GLib.Timeout.Add(1000 / DesiredFps, OnTick);
		return false;
	}
 }

using C5;
using Gdk;
using Gtk;
using MfGames.Sprite;
using System;
using System.IO;
using System.Reflection;
using System.Text;

public class Viewer
: VBox
{
	public static TilesetDrawableFactory TilesetFactory =
		new TilesetDrawableFactory();
	public static uint DesiredFps = 30;

	private DrawingArea area;
	private Pixmap pixmap;
	private SpinButton fps;
	private CheckButton showUpdate;
	private SpriteViewport viewport;

	public static SpriteList Sprites;
	public static DrawableSprite Sprite;

	private long start = DateTime.UtcNow.Ticks;
	private long exposeCount = 0;
	private long tickCount = 0;

	/// <summary>
	/// Constructs the sprite drawing area, along with the various
	/// rules needed.
	/// </summary>
	public Viewer()
	{
		// Create the drop-down list
		HBox box = new HBox();
		fps = new SpinButton(1, 100, 1);
		fps.Value = DesiredFps;
		fps.Changed += OnFpsChanged;
		showUpdate = new CheckButton();
		box.PackStart(new Label("FPS"), false, false, 0);
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

		// Create the viewport
		Sprites = new SpriteList();
		viewport = new SpriteViewport(Sprites);

		// Start up a little animation loop
		GLib.Timeout.Add(1000 / DesiredFps, OnTick);
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
		int min = Math.Min(width, height);
		viewport.Height = height;
		viewport.Width = width;

		// Create the backing pixmap
		pixmap = new Pixmap(window, width, height, -1);

		// Update the current pane
		if (Sprite != null)
			Sprite.Height = Sprite.Width = ((int) ((double) min * 0.75));
		
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

		// Render ourselves
		viewport.Render(pixmap, region);

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
		Sprites.Update();
		viewport.FireQueueDraw(area);

		// Handle the tick updating
		tickCount++;

		if (tickCount % 100 == 0)
		{
			double diff = (DateTime.UtcNow.Ticks - start) / 10000000.0;
			double efps = (double) exposeCount / (double) diff;
			double tfps = (double) tickCount / (double) diff;
			ViewerEntry.Statusbar.Push(0, String.Format
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

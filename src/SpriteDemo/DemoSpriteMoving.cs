using Gdk;
using Gtk;
using MfGames.Sprite;
using MfGames.Utility;
using System;

/// <summary>
/// This demostration explores how the sprites can move around via a
/// FPS-netural rate.
/// </summary>
public class DemoSpriteMoving
: DemoSpritePane
{
	private MovingSprite sprite;
	private DrawableSprite target;
	private MovingSprite sprite2;
	private DrawableSprite target2;
	private SpriteList sprites;
	private static SpriteViewport viewport;

	/// <summary>
	/// Constructs the pane with all the sprites.
	/// </summary>
	public DemoSpriteMoving()
	{
		// Create the basics
		sprites = new SpriteList();
		viewport = new SpriteViewport(sprites);
		viewport.BackgroundColor = new Color(0, 0, 50);

		// Load in the static green sphere
		IDrawable drawable = DemoSprites.TilesetFactory.Create("red-sphere");
		DrawableSprite sprite1 = new DrawableSprite(drawable);
		sprite = new MovingSprite(sprite1);
		sprite.StoppedMoving += OnStoppedMoving;
		sprite.Height = sprite.Width = 64;
		sprite.X = sprite.Y = 10;
		sprites.Add(sprite);

		// Create the target
		drawable = DemoSprites.TilesetFactory.Create("green-sphere");
		target = new DrawableSprite(drawable);
		target.Height = target.Width = 32;
		sprites.Add(target);

		// Load in the static green sphere
		drawable = DemoSprites.TilesetFactory.Create("red-sphere2");
		DrawableSprite sprite2a = new DrawableSprite(drawable);
		sprite2 = new MovingSprite(sprite2a);
		sprite2.Height = sprite2.Width = 64;
		sprite2.X = sprite2.Y = 10;
		sprites.Add(sprite2);

		// Create the target
		drawable = DemoSprites.TilesetFactory.Create("green-sphere");
		target2 = new DrawableSprite(drawable);
		target2.Height = target2.Width = 32;
		sprites.Add(target2);
		sprite2.DesiredX = Entropy.Next(0, viewport.Width - sprite.Width);
		sprite2.DesiredY = Entropy.Next(0, viewport.Height - sprite.Height);
		target2.X = sprite2.DesiredX + 16;
		target2.Y = sprite2.DesiredY + 16;

		// Start our timer
		GLib.Timeout.Add(1000, OnNewLocation2);
	}

	/// <summary>
	/// Contains the name of the pane (for the drop-down list).
	/// </summary>
	public override string Name { get { return "Moving Animation"; } }

	/// <summary>
	/// Called when the size of the widget is changed.
	/// </summary>
	public override void Configure(int width, int height)
	{
		// Set our width and height
		viewport.Width = width;
		viewport.Height = height;
		sprite.DesiredX = Entropy.Next(0, width - sprite.Width);
		sprite.DesiredY = Entropy.Next(0, height - sprite.Height);
		sprite2.RateX = sprite.RateX = (double) width / 10;
		sprite2.RateY = sprite.RateY = (double) height / 10;
		target.X = sprite.DesiredX + 16;
		target.Y = sprite.DesiredY + 16;
		target.FireInvalidate();
	}

	/// <summary>
	/// Called when the sprite needs to be relocated.
	/// </summary>
	private bool OnNewLocation()
	{
		// Relocate it
		sprite.DesiredX = Entropy.Next(0, viewport.Width - sprite.Width);
		sprite.DesiredY = Entropy.Next(0, viewport.Height - sprite.Height);
		target.X = sprite.DesiredX + 16;
		target.Y = sprite.DesiredY + 16;
		target.FireInvalidate();
		return false;
	}

	/// <summary>
	/// Called when the sprite needs to be relocated.
	/// </summary>
	private bool OnNewLocation2()
	{
		// Relocate it
		target2.FireInvalidate();
		sprite2.DesiredX = Entropy.Next(0, viewport.Width - sprite.Width);
		sprite2.DesiredY = Entropy.Next(0, viewport.Height - sprite.Height);
		target2.X = sprite2.DesiredX + 16;
		target2.Y = sprite2.DesiredY + 16;
		target2.FireInvalidate();
		return true;
	}

	/// <summary>
	/// Triggers a new location 500 ms after it stops moving.
	/// </summary>
	private void OnStoppedMoving(object sender, EventArgs args)
	{
		GLib.Timeout.Add(500, OnNewLocation);
	}

	/// <summary>
	/// Called when the pane needs to be rendered.
	/// </summary>
	public override void Render(Drawable drawable, Rectangle region)
	{
		// Have the viewport render itself
		viewport.Render(drawable, region);
	}

	/// <summary>
	/// Called when the pane needs to be updated in some manner.
	/// </summary>
	public override void Update(Gtk.Widget widget)
	{
		// We use the built-in updating
		sprites.Update();

		// Check for invalidate
		viewport.FireQueueDraw(widget);
	}
}

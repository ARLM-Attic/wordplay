using System;

using Gdk;
using Gtk;
using MfGames.Sprite;

/// <summary>
/// A bounce demostration sprite pane that just shows various elements
/// in random assortments.
/// </summary>
public class DemoSpriteBounce
: DemoSpritePane
{
	private SpriteList sprites;
	private static SpriteViewport viewport;

	/// <summary>
	/// Constructs the pane with all the sprites.
	/// </summary>
	public DemoSpriteBounce()
	{
		// Create the basics
		sprites = new SpriteList();
		viewport = new SpriteViewport(sprites);
		viewport.BackgroundColor = new Color(0, 0, 50);

		// Load in a basic drawable
		IDrawable drawable = DemoSprites.PixbufFactory.Create("green-sphere");

		// Create the sprites
		for (int i = 0; i < 20; i++)
		{
			Bounce sprite = new Bounce(drawable);
			sprite.Width = sprite.Height = 64;
			sprites.Add(sprite);
		}
	}

	/// <summary>
	/// Contains the name of the pane (for the drop-down list).
	/// </summary>
	public override string Name { get { return "Bounce"; } }

	/// <summary>
	/// Called when the size of the widget is changed.
	/// </summary>
	public override void Configure(int width, int height)
	{
		// Set our width and height
		viewport.Width = width;
		viewport.Height = height;

		// Randomize the sprites
		foreach (Bounce sprite in sprites)
		{
			sprite.X = Entropy.Next(0, width - sprite.Width);
			sprite.Y = Entropy.Next(0, height - sprite.Height);
		}
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

	/// <summary>
	/// Internal class to handle a bouncing sprite.
	/// </summary>
	private class Bounce
		: DrawableSprite
	{
		int dx, dy, count;

		/// <summary>
		/// Creates the bounce widget.
		/// </summary>
		public Bounce(IDrawable drawable)
			: base(drawable)
		{
			// Randomize the deltas
			dx = Entropy.Next(-5, 5);
			dy = Entropy.Next(-5, 5);
		}

		/// <summary>
		/// This method is used to update the sprite's capable of
		/// updating themselves.
		/// </summary>
		public override void Update()
		{
			// Get the bounds
			int width = viewport.Width;
			int height = viewport.Height;
			Rectangle r = new Rectangle(0, 0, width, height);

			// Fire a change
			FireInvalidate();

			// Reset the randomize speed
			if (count > 25)
			{
				count = 0;
				dy += Entropy.Next(-1, 1);
				dx += Entropy.Next(-1, 1);
			}

			// Adjust the coordinates for the random
			X += dx;
			Y += dy;

			// Check for bounds. If we are in it, we are good
			if (r.Contains(Rectangle))
			{
				count++;
				FireInvalidate();
				return;
			}

			// Reset the counter
			count = 0;

			// Check X bounds
			if (X < 0)
			{
				dx = Entropy.Next(0, 5);
				dy += Entropy.Next(-1, 1);
				X = 0;
			}

			if (Rectangle.Right > width)
			{
				dx = Entropy.Next(-5, 0);
				dy += Entropy.Next(-1, 1);
				X = width - Width;
			}

			// Check the Y bounds
			if (Y < 0)
			{
				dy = Entropy.Next(0, 5);
				dx += Entropy.Next(-1, 1);
				Y = 0;
			}

			if (Rectangle.Bottom > height)
			{
				dy = Entropy.Next(-5, 0);
				dx += Entropy.Next(-1, 1);
				Y = height - Height;
			}

			// Fire the invalidate
			FireInvalidate();
		}
	}
}

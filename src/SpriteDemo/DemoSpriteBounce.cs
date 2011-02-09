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

using Gdk;

using Gtk;

using MfGames.Sprite;

#endregion

/// <summary>
/// A bounce demostration sprite pane that just shows various elements
/// in random assortments.
/// </summary>
public class DemoSpriteBounce : DemoSpritePane
{
	private static SpriteViewport viewport;
	private readonly SpriteList sprites;

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
	public override string Name
	{
		get { return "Bounce"; }
	}

	/// <summary>
	/// Called when the size of the widget is changed.
	/// </summary>
	public override void Configure(
		int width,
		int height)
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
	public override void Render(
		Drawable drawable,
		Rectangle region)
	{
		// Have the viewport render itself
		viewport.Render(drawable, region);
	}

	/// <summary>
	/// Called when the pane needs to be updated in some manner.
	/// </summary>
	public override void Update(Widget widget)
	{
		// We use the built-in updating
		sprites.Update();

		// Check for invalidate
		viewport.FireQueueDraw(widget);
	}

	#region Nested type: Bounce

	/// <summary>
	/// Internal class to handle a bouncing sprite.
	/// </summary>
	private class Bounce : DrawableSprite
	{
		private int count;
		private int dx, dy;

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

	#endregion
}
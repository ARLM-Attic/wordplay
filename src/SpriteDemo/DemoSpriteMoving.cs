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

using Gdk;

using Gtk;

using MfGames.Sprite;

using Timeout=GLib.Timeout;

#endregion

/// <summary>
/// This demostration explores how the sprites can move around via a
/// FPS-netural rate.
/// </summary>
public class DemoSpriteMoving : DemoSpritePane
{
	private static SpriteViewport viewport;
	private readonly MovingSprite sprite;
	private readonly MovingSprite sprite2;
	private readonly SpriteList sprites;
	private readonly DrawableSprite target;
	private readonly DrawableSprite target2;

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
		Timeout.Add(1000, OnNewLocation2);
	}

	/// <summary>
	/// Contains the name of the pane (for the drop-down list).
	/// </summary>
	public override string Name
	{
		get { return "Moving Animation"; }
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
	private void OnStoppedMoving(
		object sender,
		EventArgs args)
	{
		Timeout.Add(500, OnNewLocation);
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
}
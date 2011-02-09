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
/// This demostration explores the handle of a tileset loading and
/// processing, along with the internal method for handling animation
/// elements.
/// </summary>
public class DemoSpriteTileset : DemoSpritePane
{
	private static SpriteViewport viewport;
	private readonly SpriteList sprites;

	/// <summary>
	/// Constructs the pane with all the sprites.
	/// </summary>
	public DemoSpriteTileset()
	{
		// Create the basics
		sprites = new SpriteList();
		viewport = new SpriteViewport(sprites);
		viewport.BackgroundColor = new Color(0, 0, 50);

		// Load in the static green sphere
		IDrawable drawable = DemoSprites.TilesetFactory.Create("green-sphere");
		DrawableSprite sprite = new DrawableSprite(drawable);
		sprite.Height = sprite.Width = 64;
		sprite.X = sprite.Y = 10;
		sprites.Add(sprite);

		// Load in the animated red sphere
		int rows = 6;
		int cols = 6;

		for (int i = 0; i < cols; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				if (i == j)
				{
					drawable = DemoSprites.TilesetFactory.Create("red-sphere2");
				}
				else
				{
					drawable = DemoSprites.TilesetFactory.Create("red-sphere");
				}

				sprite = new DrawableSprite(drawable);
				sprite.Height = sprite.Width = 64;
				sprite.X = 100 + i * 100;
				sprite.Y = 10 + j * 100;
				sprite.Randomize();
				sprites.Add(sprite);
			}
		}
	}

	/// <summary>
	/// Contains the name of the pane (for the drop-down list).
	/// </summary>
	public override string Name
	{
		get { return "Tileset Animation"; }
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
using Gdk;
using Gtk;
using MfGames.Sprite;

/// <summary>
/// A simple demostration sprite pane that just shows various elements
/// in random assortments.
/// </summary>
public class DemoSpriteSimple
: DemoSpritePane
{
	private SpriteList sprites;
	private SpriteViewport viewport;
	private DrawableSprite sprite;
	private DrawableSprite sprite2;
	private bool forward = true;

	/// <summary>
	/// Constructs the pane with all the sprites.
	/// </summary>
	public DemoSpriteSimple()
	{
		// Create the basics
		sprites = new SpriteList();
		viewport = new SpriteViewport(sprites);
		viewport.BackgroundColor = new Color(0, 0, 50);

		// Load in a basic sprite
		IDrawable drawable = DemoSprites.PixbufFactory.Create("blue-sphere");
		sprite = new DrawableSprite(drawable);
		sprite.X = 10;
		sprite.Y = 10;
		sprite.Width = sprite.Height = 64;
		sprites.Add(sprite);

		// Load the second sprite
		drawable = DemoSprites.PixbufFactory.Create("green-sphere");
		sprite2 = new DrawableSprite(drawable);
		sprite2.X = 10;
		sprite2.Y = 300;
		sprite2.Width = sprite2.Height = 128;
		sprites.Add(sprite2);

		// Load in just a red sphere
		drawable = DemoSprites.TilesetFactory.Create("red-sphere");
		ISprite s = new DrawableSprite(drawable);
		s.X = 400;
		s.Y = 10;
		s.Width = s.Height = 256;
		sprites.Add(s);
	}

	/// <summary>
	/// Contains the name of the pane (for the drop-down list).
	/// </summary>
	public override string Name { get { return "Simple Examples"; } }

	/// <summary>
	/// Called when the size of the widget is changed.
	/// </summary>
	public override void Configure(int width, int height)
	{
		viewport.Width = width;
		viewport.Height = height;
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
		// We invalidate ourselves to start with
		sprite.FireInvalidate();
		sprite2.FireInvalidate();

		// Handle the sprite movement
		if (forward)
		{
			sprite.X += 5;
			sprite.Y += 5;
			sprite2.X += 5;
			sprite2.Y -= 5;

			if (sprite.X > 200)
				forward = false;
		}
		else
		{
			sprite.X -= 5;
			sprite.Y -= 5;
			sprite2.X -= 5;
			sprite2.Y += 5;

			if (sprite.X < 15)
				forward = true;
		}

		// Since we moved, invalidate again
		sprite.FireInvalidate();
		sprite2.FireInvalidate();

		// Check for invalidate
		sprites.Update();
		viewport.FireQueueDraw(widget);
	}
}


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

#endregion

namespace MfGames.Sprite
{
	/// <summary>
	/// A sprite viewport is a view into a SpriteList and is used for
	/// the actual rendering. A viewport can move, such as to follow
	/// or center a specific sprite, or it can be static. The viewport
	/// is also the primary method for translating the drawing
	/// operations from window coordinates into sprite coordinates.
	/// </summary>
	public class SpriteViewport
	{
		/// <summary>
		/// Constructs a viewport with no sprites.
		/// </summary>
		public SpriteViewport()
			: this(new SpriteList())
		{
		}

		/// <summary>
		/// Constructs a sprite viewport with a given sprite list.
		/// </summary>
		public SpriteViewport(SpriteList sprites)
		{
			SpriteList = sprites;
			sprites.Invalidate += OnInvalidated;
		}

		/// <summary>
		/// This method is used to render a sprite viewport onto a
		/// drawable object, such as Gdk.Pixmap. This handles the
		/// translation of widget space into sprite space and also the
		/// appropriate clipping.
		/// </summary>
		public void Render(
			Drawable drawable,
			Rectangle region)
		{
			// Create the GC
			GC gc = new GC(drawable);

			// Figure out what parts of the rectangle we care about
			Rectangle ourselves = new Rectangle(x, y, width, height);

			if (!ourselves.IntersectsWith(region))
			{
				// No overlap, don't bother
				return;
			}

			// Get the intersection of ourselves and the update
			// region. This helps with keeping to a specific focus.
			ourselves.Intersect(region);

			// Clear everything
			gc.RgbFgColor = BackgroundColor;
			drawable.DrawRectangle(gc, true, ourselves);

			// Go through the sprites
			foreach (ISprite sprite in SpriteList)
			{
				sprite.Render(drawable, gc);
			}
		}

		#region Properties

		private int height;
		private SpriteList sprites;
		private int width;
		private int x, y;

		/// <summary>
		/// Contains the background color for the sprite. This is used
		/// as the "erase" color before rendering.
		/// </summary>
		public Color BackgroundColor { get; set; }

		/// <summary>
		/// Contains the height of the viewport, in widget/drawable
		/// space.
		/// </summary>
		public int Height
		{
			get { return height; }
			set { height = value; }
		}

		/// <summary>
		/// Assigns a sprite list to a viewport. The viewport cannot
		/// be null and an exception will be thrown if a null is
		/// assigned to it.
		/// </summary>
		public SpriteList SpriteList
		{
			get { return sprites; }
			set
			{
				// Check for null
				if (value == null)
				{
					throw new SpriteException("Cannot assign a null sprite list");
				}

				// Assign it
				sprites = value;
			}
		}

		/// <summary>
		/// Contains the width of the viewport, in widget/drawable
		/// space.
		/// </summary>
		public int Width
		{
			get { return width; }
			set { width = value; }
		}

		#endregion

		#region Sprite Space

		// This is the changes in sprite space
		private Rectangle invalidated = Rectangle.Zero;

		/// <summary>
		/// This retrieves an invalidated area (in widget space) and
		/// clears the current invalidation.
		/// </summary>
		public Rectangle GetInvalidatedRegion()
		{
			// Ignore blanks
			if (invalidated.IsEmpty)
			{
				return invalidated;
			}

			// Translate to the widget space

			// Get the intersection
			Rectangle i = new Rectangle(x, y, width, height);

			if (!invalidated.IntersectsWith(i))
			{
				invalidated = Rectangle.Zero;
				return invalidated;
			}

			i.Intersect(invalidated);

			// Clear it
			invalidated = Rectangle.Zero;
			return i;
		}

		#endregion

		#region Events

		/// <summary>
		/// Fires a queue draw, if there is one. If the invalidated
		/// region is empty, there are no updated and the queue is not
		/// called.
		/// </summary>
		public void FireQueueDraw(Widget widget)
		{
			Rectangle r = GetInvalidatedRegion();

			if (!r.IsEmpty)
			{
				widget.QueueDrawArea(r.X, r.Y, r.Width, r.Height);
			}
		}

		/// <summary>
		/// This is triggered when a sprite is invalidated. This is
		/// used to build up the exposer/queue elements.
		/// </summary>
		private void OnInvalidated(
			object sender,
			InvalidateArgs args)
		{
			// Check for a zero
			if (invalidated.IsEmpty)
			{
				invalidated = args.Rectangle;
			}
			else
			{
				invalidated = Rectangle.Union(invalidated, args.Rectangle);
			}
		}

		#endregion
	}
}
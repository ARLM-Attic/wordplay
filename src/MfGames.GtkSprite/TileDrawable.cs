using Gdk;
using System;

namespace MfGames.Sprite
{
	/// <summary>
	/// This semi-complicated class handles the animation of a
	/// tile-based drawable. It is backed by another IDrawable object,
	/// which handles the image production. This class just deals with
	/// the fragmenting and isolation of the data.
	/// </summary>
	public class TileDrawable
	: IDrawable
	{
		/// <summary>
		/// Constructs the tile drawable object.
		/// </summary>
		public TileDrawable(Tile tile, IDrawable drawable)
		{
			this.tile = tile;
			this.drawable = drawable;
		}

		/// <summary>
		/// Every drawable contains a CacheKey which is a unique
		/// string to identify the drawable, it's size, and its
		/// alterations (such as color mapping).
		/// </summary>
		public string GetCacheKey(DrawableState state)
		{
			return String.Format("{0}:f{1}",
				drawable.GetCacheKey(state), state.Frame);
		}

		/// <summary>
		/// Create/retrieves a pixbuf based on the given state.
		/// </summary>
		public Pixbuf GetPixbuf(DrawableState state)
		{
			// Check for optimization (one frame)
			if (tile.Count == 1)
				return drawable.GetPixbuf(state);

			// Figure out our key for the cache
			int frame = state.Frame;
			string cacheKey = GetCacheKey(state);

			// Check the cache
			if (PixbufCache.Contains(cacheKey))
				return PixbufCache[cacheKey];

			// We have to create the pixbuf

			// Adjust our own drawable state
			drawableState.Width = state.Width * tile.Columns;
			drawableState.Height = state.Height * tile.Rows;
			drawableState.X = state.X;
			drawableState.Y = state.Y;
			
			// Figure out the frame and coordinates
			int row = frame / tile.Columns;
			int col = frame % tile.Columns;
			int dx = col * state.Width;
			int dy = row * state.Height;

			// Get the master image from our backing
			Pixbuf image = drawable.GetPixbuf(drawableState);

			if (image == null)
				throw new SpriteException("Cannot create tile drawable: "
					+ tile.ID);

			// Create a new pixbuf
			Colorspace colorspace = image.Colorspace;
			bool hasAlpha         = image.HasAlpha;
			int bitsPerSample     = image.BitsPerSample;
			Pixbuf p              = new Pixbuf(
				colorspace, hasAlpha, bitsPerSample,
				state.Width, state.Height);

			// Scale copy it in. We have to flood fill to get rid of
			// random noise.
			p.Fill(0x00000000);
			image.Composite(p,
				0, 0, state.Width, state.Height,
				-dx, -dy, 1.0, 1.0,
				InterpType.Bilinear,
				255);

			// Cache it and return the sliced results
			PixbufCache[cacheKey] = p;
			return p;
		}

		/// <summary>
		/// Randomizes the drawable state based on this specific
		/// drawable.
		/// </summary>
		public void Randomize(DrawableState state)
		{
			// Pick a random frame. We loop this since a frame may not
			// be randomizeable (the Random property is false).
			while (true)
			{
				// Pick one
				int seq = Entropy.Next(0, tile.Count - 1);
				TileFrame frame = tile.Frames[seq];

				if (frame.Random)
				{
					// We can use this as a random. We also randomize
					// the last update to have not everything updating
					// at the same time.
					state.Frame = seq;
					state.LastUpdate =
						DateTime.UtcNow.Ticks / 10000 +
						Entropy.Next(0, frame.Delay);
					return;
				}
			}
		}

		/// <summary>
		/// This method is called when the drawable has to draw itself
		/// to a Gdk.Drawable object.
		/// </summary>
		public void Render(Drawable dest, Gdk.GC gc, DrawableState state)
		{
			// Check for single frame image
			if (tile.Count == 1)
			{
				drawable.Render(dest, gc, state);
				return;
			}

			// Render ourselves
			Gdk.Pixbuf pixbuf = GetPixbuf(state);

			dest.DrawPixbuf(gc, pixbuf,
				0, 0,
				state.X, state.Y,
				pixbuf.Width, pixbuf.Height,
				RgbDither.None, 0, 0);
		}

		/// <summary>
		/// Updates the drawable. If the drawable changed in any
		/// visible manner, then this returns true.
		/// </summary>
		public bool Update(DrawableState state)
		{
			// Don't bother if we only have one frame
			if (tile.Count == 1)
				return false;

			// Figure out the different in time
			TileFrame frame = tile.Frames[state.Frame];
			long now = DateTime.UtcNow.Ticks / 10000;
			long diff = now - state.LastUpdate;

			// If not enough time has passed, just move on
			if (diff < frame.Delay)
				return false;

			// We need to move to the next one
			long fdiff = diff - frame.Delay;

			if (fdiff > frame.Delay)
				fdiff %= frame.Delay;

			state.Frame = frame.NextFrame;
			state.LastUpdate = now + fdiff;
			return true;
		}

#region Properties
		private IDrawable drawable;
		private Tile tile;
		private DrawableState drawableState = new DrawableState();

		/// <summary>
		/// Contains the drawable image for this tileset.
		/// </summary>
		public IDrawable Drawable
		{
			get { return drawable; }
		}

		/// <summary>
		/// Contains the pixbuf cache to use for drawing.
		/// </summary>
		public virtual PixbufCache PixbufCache
		{
			get { return PixbufCache.Instance; }
		}

		/// <summary>
		/// Contains the Tile object for the drawable which handles
		/// the controls.
		/// </summary>
		public Tile Tile
		{
			get { return tile; }
		}
#endregion
	}
}

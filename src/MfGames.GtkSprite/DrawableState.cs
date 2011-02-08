using Gdk;

namespace MfGames.Sprite
{
	/// <summary>
	/// Contains the basic code for handling current frame or
	/// animation.
	/// </summary>
	public class DrawableState
	{
		private int x, y, width, height, frame;
		private long lastUpdate;

		/// <summary>
		/// Contains the current frame.
		/// </summary>
		public int Frame
		{
			get { return frame; }
			set { frame = value; }
		}

		/// <summary>
		/// Contains the height of the sprite.
		/// </summary>
		public int Height
		{
			get { return height; }
			set { height = value; }
		}

		/// <summary>
		/// The last time the drawable state was updated, in
		/// milliseconds.
		/// </summary>
		public long LastUpdate
		{
			get { return lastUpdate; }
			set { lastUpdate = value; }
		}

		/// <summary>
		/// Constructs a rectangle that represents the area of the
		/// sprite in sprite space.
		/// </summary>
		public Rectangle Rectangle
		{
			get { return new Rectangle(x, y, width, height); }
		}

		/// <summary>
		/// Contains the width of the sprite.
		/// </summary>
		public int Width
		{
			get { return width; }
			set { width = value; }
		}

		/// <summary>
		/// Contains the X coordinate of the sprite.
		/// </summary>
		public int X
		{
			get { return x; }
			set { x = value; }
		}

		/// <summary>
		/// Contains the Y coordinate of the sprite.
		/// </summary>
		public int Y
		{
			get { return y; }
			set { y = value; }
		}
	}
}

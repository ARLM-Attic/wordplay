namespace MfGames.Sprite
{
	/// <summary>
	/// A tile frame is a single frame (image) inside a tile. It has a
	/// small amount of controls, including timing functions and which
	/// frame is next control.
	/// </summary>
	public class TileFrame
	{
		private Tile tile;
		private int delay;
		private int nextFrame;
		private bool random = true;

		/// <summary>
		/// Constructs the tile frame with a tile for the default
		/// values.
		/// </summary>
		public TileFrame(Tile tile)
		{
			this.tile = tile;
		}

		/// <summary>
		/// Contains the delay in milliseconds that the frame is to be
		/// shown. If this is zero or less, it uses
		/// the delay of the tile.
		/// </summary>
		public int Delay
		{
			get
			{
				if (delay <= 0)
					return tile.Delay;
				return delay;
			}
			set { delay = value; }
		}

		/// <summary>
		/// Determines the next frame for this tile. As a default, the
		/// tile assigns this to the next frame inside tile.
		/// </summary>
		public int NextFrame
		{
			get { return nextFrame; }
			set { nextFrame = value; }
		}

		/// <summary>
		/// Is true if this frame can be selected as a random frame.
		/// </summary>
		public bool Random
		{
			get { return random; }
			set { random = value; }
		}
	}
}

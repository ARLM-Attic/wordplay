using C5;
using System.IO;

namespace MfGames.Sprite
{
	/// <summary>
	/// Contains a single tile, which can have multiple frames or
	/// images for a single tile.
	/// </summary>
	public class Tile
	{
#region Properties
		private string id;
		private FileInfo file;
		private int columns = 1;
		private int count = 0;
		private int delay = 100;
		private ArrayList <TileFrame> frames;

		/// <summary>
		/// Constructs the tile.
		/// </summary>
		public Tile()
		{
			Count = 1;
		}
			
		/// <summary>
		/// Contains the number of columns in the file.
		/// </summary>
		public int Columns
		{
			get { return columns; }
			set
			{
				if (value <= 0)
					throw new SpriteException(
						"There need to be at least one column");

				columns = value;
			}
		}

		/// <summary>
		/// Contains the number of tiles in the set. Changing this
		/// count will delete any loaded frames (the code doesn't do
		/// this if Count is set to the current value).
		/// </summary>
		public int Count
		{
			get { return count; }
			set
			{
				// Check for invalid numbers
				if (value <= 0)
					throw new SpriteException(
						"There need to be at least one tile");

				// Check for change. If it changed, then rebuild the
				// internal list.
				if (count != value)
				{
					// Create the frames
					frames = new ArrayList <TileFrame> (value);

					// Assign the frames and set defaults
					for (int i = 0; i < value; i++)
					{
						TileFrame frame = new TileFrame(this);
						frame.NextFrame = i + 1;
						frames.Add(frame);
					}

					// Fix the last one to make a round-robin
					frames[value - 1].NextFrame = 0;
				}

				// Change it
				count = value;
			}
		}

		/// <summary>
		/// Contains the default delay for the animation (100ms is the
		/// default). An individual TileFrame may override this. If
		/// the delay is zero or less, then no automated animation is
		/// given.
		/// </summary>
		public int Delay
		{
			get { return delay; }
			set { delay = value; }
		}

		/// <summary>
		/// Contains the input file for this tile.
		/// </summary>
		public FileInfo File
		{
			get { return file; }
			set
			{
				if (value == null)
					throw new SpriteException(
						"Cannot create a null file tile");

				file = value;
			}
		}

		/// <summary>
		/// Contains an array list of frames.
		/// </summary>
		public ArrayList <TileFrame> Frames
		{
			get { return frames; }
		}

		/// <summary>
		/// Contains the unique ID for this tile.
		/// </summary>
		public string ID
		{
			get { return id; }
			set
			{
				if (value == null || value.Trim() == "")
					throw new SpriteException("Cannot create null ID tile");

				id = value;
			}
		}

		/// <summary>
		/// Contains the number of rows for this tile.
		/// </summary>
		public int Rows
		{
			get
			{
				return 1 + ((Count - 1) / Columns);
			}
		}
#endregion
	}
}

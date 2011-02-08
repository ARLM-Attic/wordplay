using System;
using System.IO;
using System.Xml;

namespace MfGames.Sprite
{
	/// <summary>
	/// A tileset reader is just a reader object that takes a given
	/// XmlTextReader and generates a Tileset from the results.
	/// </summary>
	public class TilesetReader
	{
		/// <summary>
		/// Reads a tileset from the stream and returns the result.
		/// </summary>
		public Tileset Read(DirectoryInfo baseDir, XmlTextReader xtr)
		{
			// Create an empty tileset
			Tileset ts = new Tileset();

			// Loop through the reader and load things
			while (xtr.Read())
			{
				// Check for stop
				if (xtr.NodeType == XmlNodeType.EndElement &&
					xtr.LocalName == "tileset")
					break;

				// Check for new elements
				if (xtr.NodeType == XmlNodeType.Element)
				{
					// Check the name
					if (xtr.LocalName == "tile")
						ts.Tiles.Add(ReadTile(baseDir, xtr));
				}
			}

			// Return the tileset
			return ts;
		}

		/// <summary>
		/// Reads a single frame into memory.
		/// </summary>
		private void ReadFrame(Tile tile, XmlTextReader xtr)
		{
			// Grab the sequence
			int seq = Int32.Parse(xtr["sequence"]);
			TileFrame frame = null;

			try
			{
				frame = tile.Frames[seq];
			}
			catch (Exception e)
			{
				Error("Cannot get frame " + seq + " from " + tile.ID);
				return;
			}

			// Try to read in the rest of the optional elements
			try
			{
				frame.Delay = Int32.Parse(xtr["delay-ms"]);
			} catch {}

			try
			{
				frame.NextFrame = Int32.Parse(xtr["next-frame"]);
			} catch {}

			try
			{
				frame.Random = Boolean.Parse(xtr["random"]);
			}
			catch {}
		}

		/// <summary>
		/// Reads a single tile into memory.
		/// </summary>
		private Tile ReadTile(DirectoryInfo baseDir, XmlTextReader xtr)
		{
			// Grab the attributes
			Tile t = new Tile();
			t.ID = xtr["id"];
			t.File = new FileInfo(Path.Combine(baseDir.FullName, xtr["file"]));

			if (!t.File.Exists)
				Error("{0}: Tile file does not exist", t.File);

			// Try to read in the rest of the optional elements
			try
			{
				t.Columns = t.Count = Int32.Parse(xtr["columns"]);
			} catch {}
			
			try
			{
				t.Count = Int32.Parse(xtr["count"]);
			} catch {}

			try
			{
				t.Delay = Int32.Parse(xtr["delay-ms"]);
			} catch {}

			// Check for empty
			if (xtr.IsEmptyElement)
				return t;

			// Loop through the reader and load things
			while (xtr.Read())
			{
				// Check for stop
				if (xtr.NodeType == XmlNodeType.EndElement &&
					xtr.LocalName == "tile")
					break;

				// Check for new elements
				if (xtr.NodeType == XmlNodeType.Element)
				{
					// Check the name
					if (xtr.LocalName == "frame")
						ReadFrame(t, xtr);
				}
			}

			// Return it
			return t;
		}
	}
}

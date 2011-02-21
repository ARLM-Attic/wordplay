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
using System.IO;
using System.Xml;

#endregion

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
		public Tileset Read(
			DirectoryInfo baseDir,
			XmlTextReader xtr)
		{
			// Create an empty tileset
			Tileset ts = new Tileset();

			// Loop through the reader and load things
			while (xtr.Read())
			{
				// Check for stop
				if (xtr.NodeType == XmlNodeType.EndElement && xtr.LocalName == "tileset")
				{
					break;
				}

				// Check for new elements
				if (xtr.NodeType == XmlNodeType.Element)
				{
					// Check the name
					if (xtr.LocalName == "tile")
					{
						ts.Tiles.Add(ReadTile(baseDir, xtr));
					}
				}
			}

			// Return the tileset
			return ts;
		}

		/// <summary>
		/// Reads a single frame into memory.
		/// </summary>
		private void ReadFrame(
			Tile tile,
			XmlTextReader xtr)
		{
			// Grab the sequence
			int seq = Int32.Parse(xtr["sequence"]);
			TileFrame frame = null;

			try
			{
				frame = tile.Frames[seq];
			}
			catch (Exception)
			{
				return;
			}

			// Try to read in the rest of the optional elements
			if (!String.IsNullOrEmpty(xtr["delay-ms"]))
			{
				frame.Delay = Int32.Parse(xtr["delay-ms"]);
			}

			if (!String.IsNullOrEmpty(xtr["next-frame"]))
			{
				frame.NextFrame = Int32.Parse(xtr["next-frame"]);
			}

			if (!String.IsNullOrEmpty(xtr["random"]))
			{
				frame.Random = Boolean.Parse(xtr["random"]);
			}
		}

		/// <summary>
		/// Reads a single tile into memory.
		/// </summary>
		private Tile ReadTile(
			DirectoryInfo baseDir,
			XmlTextReader xtr)
		{
			// Grab the attributes
			Tile t = new Tile();
			t.ID = xtr["id"];
			t.File = new FileInfo(Path.Combine(baseDir.FullName, xtr["file"]));

			// Try to read in the rest of the optional elements
			if (!String.IsNullOrEmpty(xtr["columns"]))
			{
				t.Columns = t.Count = Int32.Parse(xtr["columns"]);
			}

			if (!String.IsNullOrEmpty(xtr["count"]))
			{
				t.Count = Int32.Parse(xtr["count"]);
			}

			if (!String.IsNullOrEmpty(xtr["delay-ms"]))
			{
				t.Delay = Int32.Parse(xtr["delay-ms"]);
			}

			// Check for empty
			if (xtr.IsEmptyElement)
			{
				return t;
			}

			// Loop through the reader and load things
			while (xtr.Read())
			{
				// Check for stop
				if (xtr.NodeType == XmlNodeType.EndElement && xtr.LocalName == "tile")
				{
					break;
				}

				// Check for new elements
				if (xtr.NodeType == XmlNodeType.Element)
				{
					// Check the name
					if (xtr.LocalName == "frame")
					{
						ReadFrame(t, xtr);
					}
				}
			}

			// Return it
			return t;
		}
	}
}
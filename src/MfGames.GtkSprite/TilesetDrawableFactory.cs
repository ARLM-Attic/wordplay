using C5;
using System.IO;
using System.Xml;

namespace MfGames.Sprite
{
	/// <summary>
	/// This constructs an IDrawable object that uses a tileset XML
	/// file as the control and source data. For simplier, more static
	/// images, it is recommended that PixbufDrawableFactory is used
	/// instead.
	///
	/// The primary use of this class is use the Load() method to load
	/// various tileset XML files into memory. Each tileset XML file
	/// consists of tile descriptions with a key id. As each XML is
	/// loaded, it replaces the prior entries of the same key.
	///
	/// Once the tilesets are loaded, then Create(key) is used to
	/// actually create the drawable and load it into memory.
	/// </summary>
	public class TilesetDrawableFactory
	{
#region Creation
		/// <summary>
		/// Constructs a drawable object using the given key.
		/// </summary>
		public IDrawable Create(string key)
		{
			// Get the tile
			Tile tile = null;

			try
			{
				tile = tiles[key];
			}
			catch
			{
				throw new SpriteException("Tile " + key + " does not exist");
			}

			// Create an SVG drawable
			IDrawable rsvg = new RsvgDrawable(tile.File);

			// Wrap this drawable (which handles some caching) into a
			// tile drawable which understands the tileset
			IDrawable drawable = new TileDrawable(tile, rsvg);

			// Return the drawable
			return drawable;
		}
#endregion

#region Loading
		// Contains all of the tiles in memory
		private HashDictionary <string, Tile> tiles =
			new HashDictionary <string, Tile> ();

		/// <summary>
		/// Loads a tileset into memory and installs the tiles into
		/// the factory's cache.
		/// </summary>
		public void Load(FileInfo file)
		{
			// Make sure it exists
			if (!file.Exists)
				throw new SpriteException(
					"Tileset " + file + " does not exist");

			// Get the relative directory
			DirectoryInfo baseDir = file.Directory;

			// Load the tileset into memory
			TextReader tr = file.OpenText();
			XmlTextReader xtr = new XmlTextReader(tr);
			TilesetReader tsr = new TilesetReader();
			Tileset tileset = tsr.Read(baseDir, xtr);
			xtr.Close();
			tr.Close();

			// Go through the tiles
			foreach (Tile tile in tileset.Tiles)
				tiles[tile.ID] = tile;
		}
#endregion
	}
}

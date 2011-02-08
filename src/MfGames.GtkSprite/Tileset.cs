using C5;

namespace MfGames.Sprite
{
	/// <summary>
	/// This class represents a XML tileset loaded into memory. It
	/// contains zero or more tiles, along with the other required
	/// tileset-scoped data.
	/// </summary>
	public class Tileset
	{
		private LinkedList <Tile> tiles = new LinkedList <Tile> ();

		/// <summary>
		/// Contains the list of tiles in the tileset.
		/// </summary>
		public IList <Tile> Tiles
		{
			get { return tiles; }
		}
	}
}

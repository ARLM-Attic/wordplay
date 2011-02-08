using Gdk;

namespace MfGames.Sprite
{
	/// <summary>
	/// Contains the basic interface for a sprite which may be
	/// rendered into a drawing area (via SpriteManager and
	/// SpriteViewport).
	/// </summary>
	public interface ISprite
	: System.IComparable <ISprite>
	{
		/// <summary>
		/// This method is called when the sprite is told to draw
		/// itself on the window.
		/// </summary>
		void Render(Drawable dest, GC gc);

		/// <summary>
		/// This method is used to update the sprite's capable of
		/// updating themselves.
		/// </summary>
		void Update();

#region Properties
		/// <summary>
		/// Contains the settable height of the sprite.
		/// </summary>
		int Height { get; set; }

		/// <summary>
		/// Contains the settable width of the sprite.
		/// </summary>
		int Width { get; set; }

		/// <summary>
		/// Contains the X coordinate of the sprite.
		/// </summary>
		int X { get; set; }

		/// <summary>
		/// Contains the Y coordinate of the sprite.
		/// </summary>
		int Y { get; set; }

		/// <summary>
		/// Contains the Z coordinate of the sprite. Higher numbers
		/// appear on top.
		/// </summary>
		int Z { get; set; }
#endregion

#region Events
		/// <summary>
		/// This event is used when the sprite invalidated or changes
		/// the appearance of the sprite, or moves within sprite
		/// space.
		/// </summary>
		event InvalidateHandler Invalidate;
#endregion
	}
}

using MfGames.Sprite;

namespace MfGames.Wordplay
{
	/// <summary>
	/// The burnt sprite handles the animation for a tile being
	/// burnt. It stops animating according to the controls, but it
	/// identifies itself as removable at frame eight.
	/// </summary>
	public class BurntSprite
	: DrawableSprite
	{
		/// <summary>
		/// Constructs a burnt sprite.
		/// </summary>
		public BurntSprite(ISprite sprite)
			: base(Game.Theme.DrawableFactory.Create("burnt"))
		{
			// Set our frame to 4
			state.Frame = 4;

			// Copy our coordinates
			X = sprite.X;
			Y = sprite.Y;
			Height = sprite.Height;
			Width = sprite.Width;
		}

		/// <summary>
		/// This is true if the sprite can be removed.
		/// </summary>
		public bool CanRemove { get { return state.Frame == 8; } }
	}
}

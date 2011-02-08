using Gdk;

namespace MfGames.Sprite
{
	/// <summary>
	/// Implements a sprite which has a drawable as the graphic behind
	/// it. This is the most common type of sprite.
	/// </summary>
	public class DrawableSprite
	: SpriteBase
	{
		/// <summary>
		/// Constructs the sprite from the given drawable.
		/// </summary>
		public DrawableSprite(IDrawable drawable)
		{
			this.drawable = drawable;
		}

		/// <summary>
		/// Marks this frame as randomizable.
		/// </summary>
		public virtual void Randomize()
		{
			drawable.Randomize(state);
		}

		/// <summary>
		/// This method is called when the sprite is told to draw
		/// itself on the window.
		/// </summary>
		public override void Render(Drawable dest, GC gc)
		{
			// Draw it out
			drawable.Render(dest, gc, state);
		}

		/// <summary>
		/// This method is used to update the sprite's capable of
		/// updating themselves.
		/// </summary>
		public override void Update()
		{
			// Automatically try to update the animation. If the
			// animation updated, then fire a redraw
			if (drawable.Update(state))
				FireInvalidate();
		}

#region Properties
		protected IDrawable drawable;
#endregion
	}
}

namespace MfGames.MfGtk.Sprite
{
	/// <summary>
	/// This interface represents a single drawable frame. For static
	/// images, there is only one drawable for a given name while
	/// animated setups will have two or more.
	/// </summary>
	public interface ISpriteDrawable
	{
		/// <summary>
		/// Constructs an iterator for the drawable. This should be
		/// called once since the iterator handles the animation or
		/// movement of the frames.
		/// </summary>
		ISpriteDrawableIterator CreateIterator();
	}
}

namespace MfGames.MfGtk.Sprite
{
	/// <summary>
	/// Contains a single instance of a sprite drawable iterator. This
	/// is used for keeping animation handled for a single
	/// sprite. Typically, this is constructed with
	/// ISpriteDrawable.CreateIterator when a ISprite is assigned the
	/// drawable.
	/// </summary>
	public interface ISpriteDrawableIterator
	{
		/// <summary>
		/// Randomized the iterator. For static images, this will do
		/// nothing, but otherwise it will select a random frame in
		/// the drawable.
		/// </summary>
		void Randomize();
	}
}

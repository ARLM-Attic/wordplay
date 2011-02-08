using Gdk;

namespace MfGames.Sprite
{
	/// <summary>
	/// Describes the basic and common functionality of a
	/// drawable. The core of the system is the drawable, which
	/// represents one or more images that are rendered in the
	/// screen. For multi-frame drawables, the DrawableState class is
	/// used to help distinguish which frame is used in these cases.
	///
	/// Each drawable has a CacheKey which uniquely identifies a
	/// single drawable in memory, with all alterations or
	/// modifications done. This is used for the DrawableCache class
	/// to speed up processing and generation.
	/// </summary>
	public interface IDrawable
	{
		/// <summary>
		/// Every drawable contains a CacheKey which is a unique
		/// string to identify the drawable, it's size, and its
		/// alterations (such as color mapping).
		/// </summary>
		string GetCacheKey(DrawableState state);

		/// <summary>
		/// Contains the rendered pixbuf of this element.
		/// </summary>
		Pixbuf GetPixbuf(DrawableState state);

		/// <summary>
		/// Randomizes the drawable state based on this specific
		/// drawable.
		/// </summary>
		void Randomize(DrawableState state);

		/// <summary>
		/// This method is called when the drawable has to draw itself
		/// to a Gdk.Drawable object.
		/// </summary>
		void Render(Drawable dest, GC gc, DrawableState state);

		/// <summary>
		/// Updates the drawable. If the drawable changed in any
		/// visible manner, then this returns true.
		/// </summary>
		bool Update(DrawableState state);
	}
}

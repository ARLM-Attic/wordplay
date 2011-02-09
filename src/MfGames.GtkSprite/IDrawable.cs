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

using Gdk;

#endregion

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
		void Render(
			Drawable dest,
			GC gc,
			DrawableState state);

		/// <summary>
		/// Updates the drawable. If the drawable changed in any
		/// visible manner, then this returns true.
		/// </summary>
		bool Update(DrawableState state);
	}
}
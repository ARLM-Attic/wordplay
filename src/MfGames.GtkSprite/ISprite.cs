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

using Gdk;

using GC=Gdk.GC;

#endregion

namespace MfGames.Sprite
{
	/// <summary>
	/// Contains the basic interface for a sprite which may be
	/// rendered into a drawing area (via SpriteManager and
	/// SpriteViewport).
	/// </summary>
	public interface ISprite : IComparable<ISprite>
	{
		/// <summary>
		/// This method is called when the sprite is told to draw
		/// itself on the window.
		/// </summary>
		void Render(
			Drawable dest,
			GC gc);

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
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
	/// Implements a sprite which has a drawable as the graphic behind
	/// it. This is the most common type of sprite.
	/// </summary>
	public class DrawableSprite : SpriteBase
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
		public override void Render(
			Drawable dest,
			GC gc)
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
			{
				FireInvalidate();
			}
		}

		#region Properties

		protected IDrawable drawable;

		#endregion
	}
}
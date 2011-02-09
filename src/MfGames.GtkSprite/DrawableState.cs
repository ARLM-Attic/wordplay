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
	/// Contains the basic code for handling current frame or
	/// animation.
	/// </summary>
	public class DrawableState
	{
		private int height;
		private int width;
		private int x, y;

		/// <summary>
		/// Contains the current frame.
		/// </summary>
		public int Frame { get; set; }

		/// <summary>
		/// Contains the height of the sprite.
		/// </summary>
		public int Height
		{
			get { return height; }
			set { height = value; }
		}

		/// <summary>
		/// The last time the drawable state was updated, in
		/// milliseconds.
		/// </summary>
		public long LastUpdate { get; set; }

		/// <summary>
		/// Constructs a rectangle that represents the area of the
		/// sprite in sprite space.
		/// </summary>
		public Rectangle Rectangle
		{
			get { return new Rectangle(x, y, width, height); }
		}

		/// <summary>
		/// Contains the width of the sprite.
		/// </summary>
		public int Width
		{
			get { return width; }
			set { width = value; }
		}

		/// <summary>
		/// Contains the X coordinate of the sprite.
		/// </summary>
		public int X
		{
			get { return x; }
			set { x = value; }
		}

		/// <summary>
		/// Contains the Y coordinate of the sprite.
		/// </summary>
		public int Y
		{
			get { return y; }
			set { y = value; }
		}
	}
}
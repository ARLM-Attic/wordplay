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
	/// Contains a concrete version of ISprite which contains the
	/// basic functionality of a sprite.
	/// </summary>
	public abstract class SpriteBase : ISprite
	{
		/// <summary>
		/// Compares a sprite to another sprite.
		/// </summary>
		public virtual int CompareTo(ISprite sprite)
		{
			if (Z != sprite.Z)
			{
				return Z.CompareTo(sprite.Z);
			}

			if (X != sprite.X)
			{
				return X.CompareTo(sprite.X);
			}

			if (Y != sprite.Y)
			{
				return Y.CompareTo(sprite.Y);
			}

			return 0;
		}

		/// <summary>
		/// This method is called when the sprite is told to draw
		/// itself on the window.
		/// </summary>
		public abstract void Render(
			Drawable dest,
			GC gc);

		/// <summary>
		/// This method is used to update the sprite's capable of
		/// updating themselves.
		/// </summary>
		public virtual void Update()
		{
		}

		#region Properties

		protected DrawableState state = new DrawableState();

		/// <summary>
		/// Contains the drawable state.
		/// </summary>
		public DrawableState DrawableState
		{
			get { return state; }
		}

		/// <summary>
		/// Contains the height of the sprite.
		/// </summary>
		public int Height
		{
			get { return state.Height; }
			set { state.Height = value; }
		}

		/// <summary>
		/// Constructs a rectangle that represents the area of the
		/// sprite in sprite space.
		/// </summary>
		public Rectangle Rectangle
		{
			get { return state.Rectangle; }
		}

		/// <summary>
		/// Contains the width of the sprite.
		/// </summary>
		public int Width
		{
			get { return state.Width; }
			set { state.Width = value; }
		}

		/// <summary>
		/// Contains the X coordinate of the sprite.
		/// </summary>
		public int X
		{
			get { return state.X; }
			set { state.X = value; }
		}

		/// <summary>
		/// Contains the Y coordinate of the sprite.
		/// </summary>
		public int Y
		{
			get { return state.Y; }
			set { state.Y = value; }
		}

		/// <summary>
		/// Contains the Z coordinate of the sprite.
		/// </summary>
		public int Z { get; set; }

		#endregion

		#region Events

		/// <summary>
		/// Fires an invalidate event to all of the listeners.
		/// </summary>
		public void FireInvalidate()
		{
			// Ignore if we don't have listeners
			if (Invalidate == null)
			{
				return;
			}

			// Create the rectangle
			InvalidateArgs args = new InvalidateArgs();
			args.Rectangle = Rectangle;

			// Fire it
			Invalidate(this, args);
		}

		/// <summary>
		/// Fires an invalidate event that takes the current sprite's
		/// rectangle and merges it with the given one.
		/// </summary>
		public void FireInvalidate(Rectangle rectangle)
		{
			// Ignore if we don't have listeners
			if (Invalidate == null)
			{
				return;
			}

			// Create the rectangle
			InvalidateArgs args = new InvalidateArgs();
			args.Rectangle = Rectangle.Union(Rectangle, rectangle);

			// Fire it
			Invalidate(this, args);
		}

		/// <summary>
		/// This event is used when the sprite invalidated or changes
		/// the appearance of the sprite, or moves within sprite
		/// space.
		/// </summary>
		public event InvalidateHandler Invalidate;

		#endregion
	}
}
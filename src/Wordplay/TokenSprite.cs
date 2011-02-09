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

using MfGames.Sprite;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// Extends the sprite for token-specific entries.
	/// </summary>
	public class TokenSprite : SpriteBase
	{
		#region Constants

		public static readonly int FloodedStart = 5;

		#endregion

		private Display display;
		private IDrawable letterDrawable;
		private DrawableState letterState;
		private IDrawable selectedDrawable;
		private DrawableState selectedState;
		private IDrawable tileDrawable;
		private Token token;

		private string typeSprite;
		private string valueSprite;

		/// <summary>
		/// Constructs the token sprite based on the individual
		/// components. This is the "view" part of a token.
		/// </summary>
		public TokenSprite(
			Display display,
			Token token)
		{
			this.display = display;
			this.token = token;

			// Create the tile
			typeSprite = token.TypeSpriteName;
			tileDrawable = Game.Theme.DrawableFactory.Create(typeSprite);

			// Create the letter
			valueSprite = token.ValueSpriteName;
			letterDrawable = Game.Theme.DrawableFactory.Create(valueSprite);
			letterState = new DrawableState();

			// Create the selected
			selectedDrawable = Game.Theme.DrawableFactory.Create("selected");
			selectedState = new DrawableState();

			// Randomize it
			Randomize();
		}

		/// <summary>
		/// Returns true if this token can be selected.
		/// </summary>
		public bool CanSelect
		{
			get { return token.Value != ' '; }
		}

		/// <summary>
		/// Contains the token for this sprite.
		/// </summary>
		public Token Token
		{
			get { return token; }
			set { token = value; }
		}

		/// <summary>
		/// Marks this frame as randomizable.
		/// </summary>
		public virtual void Randomize()
		{
			tileDrawable.Randomize(state);
			letterDrawable.Randomize(letterState);
			selectedDrawable.Randomize(selectedState);
		}

		/// <summary>
		/// Renders out the token sprite.
		/// </summary>
		public override void Render(
			Drawable dest,
			GC gc)
		{
			Width = letterState.Width = selectedState.Width = display.TileSize;
			Height = letterState.Height = selectedState.Height = display.TileSize;
			letterState.X = selectedState.X = X;
			letterState.Y = selectedState.Y = Y;

			if (Width == 0 || Height == 0)
			{
				return;
			}

			tileDrawable.Render(dest, gc, state);
			letterDrawable.Render(dest, gc, letterState);

			// Check for in chain
			if (token.InChain)
			{
				selectedDrawable.Render(dest, gc, selectedState);
			}
		}

		/// <summary>
		/// Overrides the string method.
		/// </summary>
		public override string ToString()
		{
			return "TokenSprite(" + token + ")";
		}

		/// <summary>
		/// This method is used to update the sprite's capable of
		/// updating themselves.
		/// </summary>
		public override void Update()
		{
			// Automatically try to update the animation. If the
			// animation updated, then fire a redraw
			bool update = tileDrawable.Update(state);
			update = letterDrawable.Update(letterState) || update;

			// Only update the selected drawable
			if (token.InChain)
			{
				update = selectedDrawable.Update(selectedState) || update;
			}

			// Fire the update
			if (update)
			{
				FireInvalidate();
			}
		}

		/// <summary>
		/// Tells the sprite to update it's appearance and invalidate
		/// if it changes.
		/// </summary>
		public void UpdateDrawable()
		{
			// See if the type or value changed
			bool changed = false;

			if (typeSprite != token.TypeSpriteName)
			{
				changed = true;
				typeSprite = token.TypeSpriteName;
				tileDrawable = Game.Theme.DrawableFactory.Create(typeSprite);
			}

			if (valueSprite != token.ValueSpriteName)
			{
				changed = true;
				valueSprite = token.ValueSpriteName;
				letterDrawable = Game.Theme.DrawableFactory.Create(valueSprite);
			}

			// If we changed, randomize and invalidate
			if (changed)
			{
				Randomize();
				FireInvalidate();
			}
		}
	}
}
using Gdk;
using MfGames.Sprite;
using System;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Extends the sprite for token-specific entries.
	/// </summary>
	public class TokenSprite
	: SpriteBase
	{
#region Constants
		public static readonly int FloodedStart = 5;
#endregion

		private Token token;
		private IDrawable tileDrawable;
		private IDrawable letterDrawable;
		private IDrawable selectedDrawable;
		private Display display;
		private DrawableState letterState;
		private DrawableState selectedState;

		private string typeSprite;
		private string valueSprite;

		/// <summary>
		/// Constructs the token sprite based on the individual
		/// components. This is the "view" part of a token.
		/// </summary>
		public TokenSprite(Display display, Token token)
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
			selectedDrawable =
				Game.Theme.DrawableFactory.Create("selected");
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
		public override void Render(Gdk.Drawable dest, Gdk.GC gc)
		{
			Width = letterState.Width = selectedState.Width = display.TileSize;
			Height = letterState.Height=selectedState.Height= display.TileSize;
			letterState.X = selectedState.X = X;
			letterState.Y = selectedState.Y = Y;

			if (Width == 0 || Height == 0)
				return;

			tileDrawable.Render(dest, gc, state);
			letterDrawable.Render(dest, gc, letterState);

			// Check for in chain
			if (token.InChain)
				selectedDrawable.Render(dest, gc, selectedState);
				
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
				update = selectedDrawable.Update(selectedState) || update;

			// Fire the update
			if (update)
				FireInvalidate();
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
				letterDrawable =
					Game.Theme.DrawableFactory.Create(valueSprite);
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

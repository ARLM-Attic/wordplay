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

using C5;

#endregion

namespace MfGames.Sprite
{
	/// <summary>
	/// A sprite list is a collection of sprites. Typically, sprites
	/// are organized into ones that can interact with each other or
	/// are displayed together. This list represents one set of
	/// them. A sprite can be a member of multiple SpriteList objects.
	///
	/// A SpriteViewport is used to render or display the
	/// sprites. Each viewport has one, and only one, sprite list.
	/// </summary>
	public class SpriteList : LinkedList<ISprite>
	{
		/// <summary>
		/// Constructs a sprite list with the default triggers and
		/// events.
		/// </summary>
		public SpriteList()
		{
			// Set up the events
			ItemsAdded += OnSpriteAdded;
			ItemsRemoved += OnSpriteRemoved;
		}

		/// <summary>
		/// Triggers the update on all the sprites.
		/// </summary>
		public void Update()
		{
			foreach (ISprite sprite in this)
			{
				sprite.Update();
			}
		}

		#region Events

		/// <summary>
		/// Fires an invalidate event with the given arguments. This
		/// acts as a multiplexor for anything listening to this
		/// sprite list's invalidate (instead of registering to every
		/// sprite).
		/// </summary>
		public void FireInvalidate(InvalidateArgs args)
		{
			// Ignore if we don't have listeners
			if (Invalidate == null)
			{
				return;
			}

			// Fire it
			Invalidate(this, args);
		}

		/// <summary>
		/// This event is used when the sprite invalidated or changes
		/// the appearance of the sprite, or moves within sprite
		/// space.
		/// </summary>
		public event InvalidateHandler Invalidate;

		/// <summary>
		/// This event is added when a sprite is added to this sprite
		/// list.
		/// </summary>
		private void OnSpriteAdded(
			object sender,
			ItemCountEventArgs<ISprite> args)
		{
			args.Item.Invalidate += OnSpriteInvalidated;
		}

		/// <summary>
		/// This event is triggered when a sprite invalidate's itself.
		/// </summary>
		private void OnSpriteInvalidated(
			object sender,
			InvalidateArgs args)
		{
			FireInvalidate(args);
		}

		/// <summary>
		/// This event is added when a sprite is removed from this
		/// list.
		/// </summary>
		private void OnSpriteRemoved(
			object sender,
			ItemCountEventArgs<ISprite> args)
		{
			args.Item.Invalidate -= OnSpriteInvalidated;
		}

		#endregion
	}
}
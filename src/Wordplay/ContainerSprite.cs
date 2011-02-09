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

using MfGames.Sprite;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// Extends the moving sprite to include some automatical location
	/// code.
	/// </summary>
	public class ContainerSprite : MovingSprite
	{
		/// <summary>
		/// Constructs the container sprite.
		/// </summary>
		public ContainerSprite(TokenSprite sprite)
			: base(sprite)
		{
		}

		/// <summary>
		/// Wraps the token sprite.
		/// </summary>
		public TokenSprite TokenSprite
		{
			get { return (TokenSprite) ProxiedSprite; }
		}

		/// <summary>
		/// Handles and extends the updating functionality. This tries
		/// to move the sprite into its proper location.
		/// </summary>
		public override void Update()
		{
			Token token = TokenSprite.Token;
			base.Update();
			DesiredY = Height * token.Row;
			DesiredX = Width * token.Column;
		}
	}
}
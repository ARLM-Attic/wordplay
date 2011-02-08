using MfGames.Sprite;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Extends the moving sprite to include some automatical location
	/// code.
	/// </summary>
	public class ContainerSprite
	: MovingSprite
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

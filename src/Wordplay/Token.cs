using System;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Contains the value of a single token in play.
	/// </summary>
	public class Token
	{
		private static uint nextId = 0;
		private uint id;
		private TokenType tokenType = TokenType.Normal;
		private char tokenValue = 'A';
		private int row;
		private int col;
		private bool inChain;

		/// <summary>
		/// Constructs the token.
		/// </summary>
		public Token(TokenType type, char value, int row, int col)
		{
			this.id = nextId++;
			this.tokenType = type;
			this.tokenValue = value;
			this.row = row;
			this.col = col;
		}

		/// <summary>
		/// Contains the column.
		/// </summary>
		public int Column
		{
			get { return col; }
			set { col = value; }
		}

		/// <summary>
		/// Contains the unique identifier for this token.
		/// </summary>
		public uint ID { get { return id; } }

		/// <summary>
		/// Returns true if this is in a chain.
		/// </summary>
		public bool InChain
		{
			get { return inChain; }
			set { inChain = value; }
		}

		/// <summary>
		/// Contains the row.
		/// </summary>
		public int Row
		{
			get { return row; }
			set { row = value; }
		}

		/// <summary>
		/// Contains the type of token.
		/// </summary>
		public TokenType Type
		{
			get { return tokenType; }
			set { tokenType = value; }
		}

		/// <summary>
		/// Contains the type sprite name.
		/// </summary>
		public string TypeSpriteName
		{
			get
			{
				switch (Type)
				{
				case TokenType.Burning:    return "burning";
				case TokenType.Poisoned:   return "poisoned";
				case TokenType.Contagious: return "contagious";
				case TokenType.Flooded:    return "flooded";
				case TokenType.Copper:     return "copper";
				case TokenType.Silver:     return "silver";
				case TokenType.Gold:       return "gold";
				}

				return "normal";
			}
		}

		/// <summary>
		/// Contains the character value.
		/// </summary.
		public char Value
		{
			get { return tokenValue; }
			set { tokenValue = value; }
		}

		/// <summary>
		/// Contains the normalized key for this value.
		/// </summary>
		public string ValueSpriteName
		{
			get
			{
				return System.String.Format("character-{0:x4}", (int) Value);
			}
		}

		/// <summary>
		/// Returns true if the given token is adjacent to this one.
		/// </summary>
		public bool IsAdjacent(Token token)
		{
			// If we are free, always yet
			if (Game.Config.SelectionType == SelectionType.Free)
				return true;

			// Get the difference
			int dr = Row - token.Row;
			int dc = Column - token.Column;

			// Either the row or column must be equal
			if (Game.Config.SelectionType == SelectionType.Cross &&
				dr != 0 && dc != 0)
				return false;

			// Now check to see if they are close
			return !(dr < -1 || dr > 1 || dc < -1 || dc > 1);
		}

#region Operators
		/// <summary>
		/// Returns true if this object is equal.
		/// </summary>
		public override bool Equals(object o)
		{
			if (o is Token)
			{
				Token t = (Token) o;

				return t.tokenValue == tokenValue &&
					t.tokenType == tokenType &&
					t.row == row &&
					t.col == col;
			}
			
			// They don't match
			return false;
		}

		/// <summary>
		/// Gets the hash code of this sprite.
		/// </summary>
		public override int GetHashCode()
		{
			return tokenValue.GetHashCode()
				^ tokenType.GetHashCode()
				^ row
				^ col;
		}

		/// <summary>
		/// Overrides the value for a proper string.
		/// </summary>
		public override string ToString()
		{
			return String.Format("Token({0},{1},{2}x{3})", Value, Type,
				Column, Row);
		}
#endregion
	}
}

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
using System.Text;

using C5;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// A model for the board. This contains the code to handle the
	/// non-graphical elements of the board.
	/// </summary>
	public class Board
	{
		/// <summary>
		/// Constructs the board.
		/// </summary>
		public Board(int size)
		{
			this.size = size;
		}

		/// <summary>
		/// Does a sanity checking
		/// </summary>
		public bool CheckForErrors()
		{
			// Check for the count being off
			bool errors = false;

			if (tokens.Count != Rows * Columns)
			{
				Error(
					"Unexpected number of tokens: {0} instead of {1}",
					tokens.Count,
					Rows * Columns);
				errors = true;
			}

			// Check for duplicates
			HashSet<int> hash = new HashSet<int>();

			foreach (Token token in tokens)
			{
				// Check for out of bounds
				if (token.Row < 0)
				{
					Error("{0}: Row is below zero: {1}", token, token.Row);
					errors = true;
				}

				if (token.Column < 0)
				{
					Error("{0}: Column is below zero: {1}", token, token.Column);
					errors = true;
				}

				if (token.Row >= Rows)
				{
					Error("{0}: Row is too high: {1}", token, token.Row);
					errors = true;
				}

				if (token.Column >= Columns)
				{
					Error("{0}: Column is too high: {1}", token, token.Column);
					errors = true;
				}

				// Check for duplicate points
				int ndx = token.Row * Rows + token.Column;

				if (hash.Contains(ndx))
				{
					Error("{0}: Duplicate location: {1}", token, ndx);
					errors = true;
				}

				hash.Add(ndx);
			}

			// Return it
			return errors;
		}

		/// <summary>
		/// Initalizes the board.
		/// </summary>
		public void Initialize()
		{
			// Initialize the board
			InitializeTokens();
		}

		#region Board

		private int resetCount = 0;

		/// <summary>
		/// Shuffles the board (actually rebuilds it), but keeps some
		/// of the values in place.
		/// </summary>
		public void Shuffle()
		{
			// Reassign the values to the various tokens
			int count = tokens.Count - 1;

			foreach (Token t in tokens)
			{
				// Randomly assign this to a new row and column,
				// without duplicates. We do this by basically finding
				// another random one and moving it (even if it is
				// blank).
				Token t1 = tokens[Entropy.Next(0, count)];

				// Make sure this wouldn't put a burning tile on the
				// bottom row. This will move burning tiles up, if needed.
				if (t1.Type == TokenType.Burning && t1.Row == Rows - 1 && t.Row == Rows - 1)
				{
					continue;
				}

				// Swap the location of the tiles
				int row = t1.Row;
				int col = t1.Column;
				t1.Row = t.Row;
				t1.Column = t.Column;
				t.Row = row;
				t.Column = col;
			}

			// Compare to get rid of the random water tiles. We don't
			// process burning, because that would be just rude to the
			// players.
			UpdateFlooded();
			UpdateGravity();
			UpdatePopulation();

			// Mark some new tiles as being "burning". We don't ever
			// mark any of the burning tiles on the very bottom row as
			// burning, to give them at least one chance.
			resetCount++;

			for (int i = 0; i < resetCount; i++)
			{
				// Set to burning
				Token nb = GetToken(Entropy.Next(0, Rows - 2), Entropy.Next(0, Columns - 1));
				nb.Type = TokenType.Burning;
				OnTokenChanged(nb);
			}
		}

		/// <summary>
		/// Updates the board in various manners, handling most of the
		/// operations.
		/// </summary>
		private void UpdateBoard()
		{
			// Check for flooded tiles
			UpdateFlooded();

			// Update the burning before gravity
			UpdateBurning();

			// Draw everything down then fill from the top
			UpdateGravity();

			// Populate the missing tiles
			UpdatePopulation();
		}

		/// <summary>
		/// Handles any burning tiles in the game, moving from the
		/// bottom up.
		/// </summary>
		private void UpdateBurning()
		{
			// Go backwards
			for (int row = Rows - 1; row >= 0; row--)
			{
				// Go through the columns
				for (int col = 0; col < Columns; col++)
				{
					// Check for missing
					Token token = GetToken(row, col);

					if (token == null || token.Type != TokenType.Burning)
					{
						continue;
					}

					// The two possible conditions are, it is on the
					// bottom and the game is over or it isn't on the
					// bottom and it destroys the one underneath it.
					if (token.Row == Rows - 1)
					{
						// End of Game
						Error("End of Game!");
						Game.State = GameState.Completed;
					}
					else
					{
						// Get the token below
						Token below = GetToken(row + 1, col);

						// To handle two burning tiles on top of each
						// other, we just ignore blanks.
						if (below == null || below.Type == TokenType.Burning)
						{
							continue;
						}

						// Just destroy to the token
						RemoveToken(below, true);
					}
				}
			}
		}

		/// <summary>
		/// Handles flooded tiles which fill empty spaces.
		/// </summary>
		private void UpdateFlooded()
		{
			// Speed check, look for at least one flooded
			bool hasFlooded = false;

			foreach (Token token in tokens)
			{
				if (token.Type == TokenType.Flooded)
				{
					hasFlooded = true;
					break;
				}
			}

			if (!hasFlooded)
			{
				return;
			}

			// Remove all the empty flooded tokens from the list
			LinkedList<Token> toRem = new LinkedList<Token>();

			foreach (Token token in tokens)
			{
				if (token.Type == TokenType.Flooded && token.Value == ' ')
				{
					toRem.Add(token);
					OnTokenRemoved(token, false);
				}
			}

			tokens.RemoveAll(toRem);

			// First go through and make sure we don't have any orphan 
			// Go through all the rows and look for empty spaces
			while (true)
			{
				// Keep going as long as we made at least one change
				LinkedList<Token> toAdd = new LinkedList<Token>();

				for (int row = 0; row < Rows; row++)
				{
					for (int col = 0; col < Columns; col++)
					{
						// Check for null
						Token token = GetToken(row, col);

						if (token != null)
						{
							continue;
						}

						// This is a flooded candidate, so check the
						// tokens next to this one
						bool isFlooded = false;

						foreach (Token next in GetTokensAdjacent(row, col))
						{
							if (next.Type == TokenType.Flooded)
							{
								isFlooded = true;
							}
						}

						// Check for flooded
						if (isFlooded)
						{
							token = new Token(TokenType.Flooded, ' ', row, col);
							toAdd.Add(token);
						}
					}
				}

				// If we didn't have any added tokens, stop processing
				if (toAdd.Count == 0)
				{
					break;
				}

				// Add the tokens we want to add
				foreach (Token t in toAdd)
				{
					tokens.Add(t);
					OnTokenAdded(t);
				}
			}
		}

		/// <summary>
		/// Handles the gravity of the game, moving everything down.
		/// </summary>
		private void UpdateGravity()
		{
			// Go backwards
			for (int row = Rows - 1; row >= 0; row--)
			{
				// Go through the columns
				for (int col = 0; col < Columns; col++)
				{
					// Check for missing
					Token token = GetToken(row, col);

					if (token != null)
					{
						continue;
					}

					// If null, we are missing it so get the one
					// above. If there are none above, then we are at
					// the top.
					Token above = GetTokenAbove(row, col);

					if (above == null)
					{
						continue;
					}

					Debug("Moving {0} to {1}", above, row);

					// Reset the position
					above.Row = row;
				}
			}
		}

		/// <summary>
		/// Creates tokens to fill in the gaps on the board.
		/// </summary>
		private void UpdatePopulation()
		{
			// Go through the spaces
			for (int row = 0; row < Rows; row++)
			{
				for (int col = 0; col < Columns; col++)
				{
					// Get the token
					Token token = GetToken(row, col);

					if (token == null)
					{
						// Create the token as missing
						token = Game.TokenGenerator.CreateToken(row, col);
						tokens.Add(token);
						OnTokenAdded(token);
					}
				}
			}
		}

		#endregion

		#region Chains

		private LinkedList<Token> chain = new LinkedList<Token>();

		/// <summary>
		/// Contains a read-only copy of the current chain.
		/// </summary>
		public IList<Token> CurrentChain
		{
			get { return new GuardedList<Token>(chain); }
		}

		/// <summary>
		/// Contains the current score of the chain.
		/// </summary>
		public int? CurrentChainLetterScore
		{
			get { return Game.Score.GetScore(chain, false); }
		}

		/// <summary>
		/// Contains the current multiplier value of the chain.
		/// </summary>
		public int CurrentChainMultiplier
		{
			get { return Game.Score.GetMultiplier(chain); }
		}

		/// <summary>
		/// Contains the current score of the chain.
		/// </summary>
		public int? CurrentChainScore
		{
			get { return Game.Score.GetScore(chain, true); }
		}

		/// <summary>
		/// Contains the current text of the chain.
		/// </summary>
		public string CurrentChainText
		{
			get
			{
				StringBuilder builder = new StringBuilder();

				foreach (Token token in chain)
				{
					builder.Append(token.Value);
				}

				return builder.ToString();
			}
		}

		/// <summary>
		/// Adds the token to the chain.
		/// </summary>
		public void AddToChain(Token token)
		{
			try
			{
				// Check to see if there is only one and it is this
				// one. If it is, then reset the chain.
				if (chain.Count == 1 && chain.Contains(token))
				{
					chain.Clear();
					token.InChain = false;
					return;
				}

				// Check to see if there is anything in the chain already
				if (chain.Count > 0)
				{
					// Go backwards until we find one that is adjacent to
					// the current one
					bool addRest = false;
					bool containsToken = chain.Contains(token);

					ArrayList<Token> revChain = new ArrayList<Token>();
					revChain.AddAll(chain);
					revChain.Reverse();
					LinkedList<Token> newChain = new LinkedList<Token>();

					foreach (Token test in revChain)
					{
						// Check for containing code
						if (containsToken && !addRest && test != token)
						{
							test.InChain = false;
							continue;
						}

						if (containsToken && test == token)
						{
							// Add the rest, but not this one because we
							// append at the end.
							addRest = true;
							continue;
						}

						// Check for adjacent
						if (!addRest && !test.IsAdjacent(token))
						{
							// Pop off this one
							test.InChain = false;
							continue;
						}

						// Check for adjacent
						if (test.IsAdjacent(token))
						{
							addRest = true;
						}

						// Add the chain
						test.InChain = true;
						newChain.Add(test);
					}

					// Reverse it again
					newChain.Reverse();
					chain = newChain;
				}

				// At this point, we can append the token to the chain
				chain.Add(token);
				token.InChain = true;
			}
			finally
			{
				// Fire a change
				OnChainChanged();
			}
		}

		/// <summary>
		/// Returns true if this is a valid chain.
		/// </summary>
		public bool IsFinalToken(Token token)
		{
			// Check the score first
			if (CurrentChainScore == null)
			{
				return false;
			}

			// Check that this is last
			return token == chain.Last;
		}

		/// <summary>
		/// Scores out the chain and removes those tokens. New tokens
		/// are added and this handles the update.
		/// </summary>
		public void ScoreChain()
		{
			// Ignore nulls
			if (CurrentChainScore != null)
			{
				// Add to the score
				int cs = (int) CurrentChainScore;
				int ss = (int) CurrentChainLetterScore;
				int score = cs * CurrentChainMultiplier;
				int len = CurrentChainText.Length;
				Game.Score.CurrentScore += score;
				Game.Score.TotalWords++;

				// Check the highest word
				if (cs > Game.Score.HighestWordScore)
				{
					Game.Score.HighestWordScore = ss;
					Game.Score.HighestWord = CurrentChainText;
				}

				// Check the lengths
				if (Game.Score.LongestWord == null || len > Game.Score.LongestWord.Length)
				{
					Game.Score.LongestWord = CurrentChainText;
				}
			}

			// Go through it
			foreach (Token token in chain)
			{
				RemoveToken(token, false);
			}

			chain.Clear();

			// Update the board
			UpdateBoard();

			// Finish up
			OnChainChanged();
		}

		#endregion

		#region Events

		public event EventHandler ChainChanged;

		/// <summary>
		/// This is the default operation for when the chain changed.
		/// </summary>
		protected virtual void OnChainChanged()
		{
			if (ChainChanged != null)
			{
				ChainChanged(this, new EventArgs());
			}
		}

		/// <summary>
		/// This is the default operation for when a token is added.
		/// </summary>
		protected virtual void OnTokenAdded(Token token)
		{
			if (TokenAdded != null)
			{
				TokenAdded(this, new TokenArgs(token));
			}
		}

		/// <summary>
		/// This is the default operation for when a token is changed.
		/// </summary>
		protected virtual void OnTokenChanged(Token token)
		{
			if (TokenChanged != null)
			{
				TokenChanged(this, new TokenArgs(token));
			}
		}

		/// <summary>
		/// This is the default operation for when a token is removed.
		/// </summary>
		protected virtual void OnTokenRemoved(
			Token token,
			bool burnt)
		{
			// Build the arguments
			TokenArgs args = new TokenArgs(token);
			args.IsBurnt = burnt;

			// Call it
			if (TokenRemoved != null)
			{
				TokenRemoved(this, args);
			}
		}

		public event TokenHandler TokenAdded;
		public event TokenHandler TokenChanged;
		public event TokenHandler TokenRemoved;

		#endregion

		#region Properties

		//private int rows;
		//private int cols;
		private int size;

		/// <summary>
		/// Contains the number of columns.
		/// </summary>
		public int Columns
		{
			get { return size; }
		}

		/// <summary>
		/// Contains the number of rows.
		/// </summary>
		public int Rows
		{
			get { return size; }
		}

		/// <summary>
		/// Contains the size of the board.
		/// </summay>
		public int Size
		{
			get { return size; }
		}

		#endregion

		#region Tokens

		private ArrayList<Token> tokens = null;

		/// <summary>
		/// Contains a read-only list of tokens.
		/// </summary>
		public IList<Token> Tokens
		{
			get
			{
				GuardedList<Token> list = new GuardedList<Token>(tokens);
				return list;
			}
		}

		/// <summary>
		/// Gets the token at the given row and column.
		/// </summary>
		public Token GetToken(
			int row,
			int column)
		{
			// Just loops through it
			foreach (Token token in tokens)
			{
				if (token.Row == row && token.Column == column)
				{
					return token;
				}
			}

			// Can't find it
			return null;
		}

		/// <summary>
		/// Returns the token above the given spot or none.
		/// </summary>
		private Token GetTokenAbove(
			int row,
			int column)
		{
			// Go through it
			while (row != 0)
			{
				row--;
				Token token = GetToken(row, column);

				if (token != null)
				{
					return token;
				}
			}

			// Can't find it
			return null;
		}

		/// <summary>
		/// Returns all the tokens next to or adjacent (on the cross
		/// axis). This will never have a null in it.
		/// </summary>
		private IList<Token> GetTokensAdjacent(
			int row,
			int col)
		{
			ArrayList<Token> list = new ArrayList<Token>(8);

			foreach (Token token in tokens)
			{
				int dx = token.Row - row;
				int dy = token.Column - col;

				dx = dx > 0 ? dx : -dx;
				dy = dy > 0 ? dy : -dy;

				if (token.Row == row && dy == 1)
				{
					list.Add(token);
				}
				else if (token.Column == col && dx == 1)
				{
					list.Add(token);
				}
			}

			// Return it
			return list;
		}

		/// <summary>
		/// Sets up and seeds the initial tokens.
		/// </summary>
		private void InitializeTokens()
		{
			// Fill in the initial values
			tokens = new ArrayList<Token>(size * size);

			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					// Create the token
					Token t = Game.TokenGenerator.CreateToken(x, y);
					//Token t = new Token(TokenType.Normal, 'A', x, y);
					tokens.Add(t);
					OnTokenAdded(t);
				}
			}
		}

		/// <summary>
		/// Removes a token from the board.
		/// </summary>
		private void RemoveToken(
			Token token,
			bool burnt)
		{
			tokens.Remove(token);
			OnTokenRemoved(token, burnt);
		}

		#endregion
	}
}
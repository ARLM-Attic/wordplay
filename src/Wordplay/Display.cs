using C5;
using Gdk;
using Gtk;
using MfGames.Sprite;
using System;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Encapsulates the display and rendering of the board.
	/// </summary>
	public class Display
	: DrawingArea
	{
		private SpriteViewport viewport;
		private Pixmap pixmap;
		private int exposeCount;
		private int tickCount;
		private long start;
		private bool sortNext = false;
		private SpriteList sprites;

		/// <summary>
		/// Constructs the display and sets up the internal widgets.
		/// </summary>
		public Display()
		{
			// Set up our size
			tileSize = 72;
			SetSizeRequest(tileSize * Game.Config.BoardSize,
				tileSize * Game.Config.BoardSize);

			// Create our viewport
			sprites = new SpriteList();
			viewport = new SpriteViewport(sprites);

			// Set up the events
			ExposeEvent += OnExposed;
			ConfigureEvent += OnConfigure;
			ButtonPressEvent += OnButtonPress;
			Events = EventMask.AllEventsMask;

			// Set up the animation timer
			GLib.Timeout.Add(1000 / Game.Config.FramesPerSecond, OnTick);
		}

#region Board Events
		/// <summary>
		/// Gets a token sprite for a given token, or returns null if
		/// there is one.
		/// </summary>
		public ContainerSprite GetContainerSprite(Token token)
		{
			// Go through the sprites, unwrap the moving sprites, and
			// check for the token.
			foreach (ISprite sprite in sprites)
			{
				if (sprite is ContainerSprite)
				{
					ContainerSprite ms = (ContainerSprite) sprite;
					TokenSprite ts = ms.TokenSprite;

					if (ts != null && ts.Token == token)
						return ms;
				}
			}

			// Can't find it
			return null;
		}

		/// <summary>
		/// Gets the first token sprite in the point.
		/// </summary.
		private TokenSprite GetTokenSprite(double dx, double dy)
		{
			// Adjust for our offset
			int x = (int) dx; //(int) dx - pixmapX;
			int y = (int) dy; //(int) dy - pixmapY;

			// Go through it
			foreach (ISprite sprite in sprites)
			{
				// Don't bother if we aren't a moving one
				ContainerSprite ms = sprite as ContainerSprite;
				if (ms == null) continue;

				TokenSprite ts = ms.TokenSprite;

				// Check it
				if (ts.Rectangle.Contains(x, y))
					return ts;
			}

			// Can't find it
			//log.Debug("GetTokenSprite: {0}x{1}", x, y);
			return null;
		}

		/// <summary>
		/// Gets a token sprite for a given token, or returns null if
		/// there is one.
		/// </summary>
		public TokenSprite GetTokenSprite(Token token)
		{
			// Return it
			ContainerSprite ms = GetContainerSprite(token);

			if (ms == null)
				return null;
			else
				return ms.ProxiedSprite as TokenSprite;
		}

		/// <summary>
		/// Triggered when a token is added to the board.
		/// </summary>
		public void OnTokenAdded(object sender, TokenArgs args)
		{
			// Create a new token
			TokenSprite ts = new TokenSprite(this, args.Token);
			ContainerSprite ms = new ContainerSprite(ts);
			sprites.Add(ms);
			
			// Move the sprite over
			ms.RateX = 100;
			ms.RateY = 100;
			ms.X = ms.DesiredX = TileSize * args.Token.Column;
			ms.Y = ms.DesiredY = TileSize * args.Token.Row;
			ms.Y -= TileSize;

			// Check the state
			if (Game.State == GameState.InProgress &&
				args.Token.Type == TokenType.Flooded &&
				args.Token.Value == ' ')
			{
				// Flooded tokens show up in place, but with a special
				// animation
				ms.Y = ms.DesiredY;
				ms.X = ms.DesiredX;
				ts.DrawableState.Frame = TokenSprite.FloodedStart;
			}

			// We need to sort the list again
			sortNext = true;
			QueueDraw();
			log.Debug("TokenAdded: {0} sprits {1} state {2}",
				ts, sprites.Count, Game.State);
		}

		/// <summary>
		/// Triggered when a token is changed on the board.
		/// </summary>
		public void OnTokenChanged(object sender, TokenArgs args)
		{
			// Remove the old one
			TokenSprite ts = GetTokenSprite(args.Token);
			ts.UpdateDrawable();
		}

		/// <summary>
		/// Triggered when a token is removed from the board.
		/// </summary>
		public void OnTokenRemoved(object sender, TokenArgs args)
		{
			// Check to see if the token above is burning
			Token token = args.Token;

			if (args.IsBurnt)
			{
				// Show the burning state with a new sprite (which we
				// remove when it is done).
				BurntSprite bt = new BurntSprite(GetTokenSprite(token));
				sprites.Add(bt);
			}

			// Get the token sprite
			ContainerSprite ms = GetContainerSprite(token);

			// Remove the sprites from the list
			sprites.Remove(ms);

			// Redraw the entire screen
			QueueDraw();
			log.Debug("TokenRemoved: {0} count {1}", args.Token,
				sprites.Count);
		}
#endregion

#region Display
		private int tileSize;

		/// <summary>
		/// Returns true if at least one sprite is moving.
		/// </summary>
		public bool IsMoving
		{
			get
			{
				foreach (ISprite sprite in sprites)
				{
					ContainerSprite ms = sprite as ContainerSprite;

					if (ms != null && ms.IsMoving)
						return true;
				}

				return false;
			}
		}

		/// <summary>
		/// Contains the sprites.
		/// </summary>
		public SpriteList Sprites { get { return sprites; } }

		/// <summary>
		/// Contains the current height of all the tiles.
		/// </summary>
		public int TileSize { get { return tileSize; } }

		/// <summary>
		/// Adds the game over screen sprites.
		/// </summary>
		private void AddGameOver()
		{
			// TODO should be a sprite scene builder
			IDrawable drawable =
				Game.Theme.DrawableFactory.Create("game-over");
			DrawableSprite ds = new DrawableSprite(drawable);
			ds.X = ds.Y = 0;
			ds.Width = viewport.Width;
			ds.Height = viewport.Height;
			sprites.Add(ds);

			// Queue the redraw
			QueueDraw();

			// Do the high score processing
			HighScoreTable hst = Game.HighScores.GetTable(Game.Config);

			if (hst.IsHighScore(Game.Score))
			{
				// We need to get the username
				UserNameDialog dialog = new UserNameDialog();
				dialog.Run();
				dialog.Destroy();

				// Register it
				hst.RegisterScores(Game.Config.UserName, Game.Score);
			}

			// Show the high score
			HighScoreWindow hsw = new HighScoreWindow();
			hsw.ShowAll();
		}
#endregion

#region GUI Events
		// Contains the last state
		private GameState lastState = GameState.Unknown;

		/// <summary>
		/// Triggered when the button is pressed.
		/// </summary>
		private void OnButtonPress(object sender, ButtonPressEventArgs args)
		{
			// Ignore if we are in the wrong state
			if (Game.State != GameState.InProgress || IsMoving)
				return;

			// Force a redraw
			QueueDraw();

			// Get the token at that point
			Gdk.EventButton ev = args.Event;
			TokenSprite ts = GetTokenSprite(ev.X, ev.Y);

			if (ts == null)
			{
				//log.Debug("OnButtonPress: No sprites at {0}x{1}",
				//ev.X, ev.Y);
				return;
			}

			// If we are in a space, we can't select
			if (!ts.CanSelect)
			{
				//log.Debug("OnButtonPress: Cannot selected {0}", ts);
				return;
			}

			// Check for valid connect
			if (Game.Board.IsFinalToken(ts.Token))
			{
				// We need to remove the pieces from our board
				Game.Board.ScoreChain();

				// Update the display
				QueueDraw();
				return;
			}

			// Add to the chain
			Game.Board.AddToChain(ts.Token);

			// We handled it
			args.RetVal = true;
			log.Debug("OnButtonPress(): {0}", ts);
		}

		/// <summary>
		/// Triggered when the drawing area is configured.
		/// </summary>
		private void OnConfigure(object obj, ConfigureEventArgs args)
		{
			// Pull out some fields and figure out the sizes
			EventConfigure ev = args.Event;
			Gdk.Window window = ev.Window;
			int width = Allocation.Width;
			int height = Allocation.Height;
			int min = Math.Min(width, height);

			// Figure out the tile height
			tileSize = min / Game.Board.Size;
			Logger.Info(typeof(Display), "New tile size: {0}px", tileSize);

			// Adjust the viewport height
			viewport.Height = height;
			viewport.Width = width;

			// Create the backing pixmap
			pixmap = new Pixmap(window, width, height, -1);

			// We moved, so rapid move the tiles
			foreach (ISprite sprite in sprites)
			{
				ContainerSprite cs = sprite as ContainerSprite;

				if (cs != null)
				{
					cs.FireInvalidate();
					cs.X = cs.TokenSprite.X =
						tileSize * cs.TokenSprite.Token.Column;
					cs.FireInvalidate();
				}
			}

			// Mark ourselves as done
			QueueDraw();
			args.RetVal = true;
		}

		/// <summary>
		/// Triggered when the drawing area is exposed.
		/// </summary>
		private void OnExposed(object sender, ExposeEventArgs args)
		{
			// Check for a sort
			if (sortNext)
			{
				sprites.Sort();
				sortNext = false;
			}

			// Clear out the entire graphics area with black (just
			// because we can). This also erases the prior rendering.
			Gdk.Rectangle region = args.Event.Area;
			Gdk.GC gc = new Gdk.GC(pixmap);
			gc.ClipRectangle = region;
			
			// Render ourselves
			viewport.Render(pixmap, region);
			
			// This performs the actual drawing
			args.Event.Window.DrawDrawable(Style.BlackGC,
				pixmap,
				region.X, region.Y,
				region.X, region.Y,
				region.Width, region.Height);
			args.RetVal = false;
			exposeCount++;
		}
		
		/// <summary>
		/// Triggered on the animation loop.
		/// </summary>
		private bool OnTick()
		{
			// Check state
			if (Game.State == GameState.Started)
				Game.State = GameState.InProgress;

			if (lastState != Game.State)
			{
				// Check for game over
				if (Game.State == GameState.Completed)
				{
					// Wipe everything
					sprites.Clear();

					// Add game over screen
					AddGameOver();
				}

				// Save the state
				lastState = Game.State;
			}

			// Update the sprites
			sprites.Update();

			// Remove any completed animations
			LinkedList <BurntSprite> list = new LinkedList <BurntSprite> ();

			foreach (ISprite sprite in sprites)
			{
				BurntSprite bs = sprite as BurntSprite;

				if (bs != null && bs.CanRemove)
					list.Add(bs);
			}

			foreach (BurntSprite bs in list)
				sprites.Remove(bs);

			// Render the pane
			viewport.FireQueueDraw(this);
			
			// Handle the tick updating
			tickCount++;
			
			if (tickCount % 100 == 0)
			{
				int fps = (int) Game.Config.FramesPerSecond;
				double diff = (DateTime.UtcNow.Ticks - start) / 10000000.0;
				double efps = (double) exposeCount / (double) diff;
				double tfps = (double) tickCount / (double) diff;
				System.Console.WriteLine
					("FPS: Exposed {0:N1} FPS ({3:N1}%), "
						+ "Ticks {1:N1} FPS ({4:N1}%), "
						+ "Maximum {2:N0} FPS",
						efps, tfps, fps,
						efps * 100 / fps,
						tfps * 100 / fps);
				start = DateTime.UtcNow.Ticks;
				exposeCount = tickCount = 0;
			}
			
			// Re-request the animation
			GLib.Timeout.Add(1000 / Game.Config.FramesPerSecond, OnTick);
			return false;
		}
#endregion
	}
}

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

using Gtk;

#endregion

namespace MfGames.Wordplay
{
	/// <summary>
	/// Encapsulates the functionality of the game window.
	/// </summary>
	public class GameWindow : Locale
	{
		/// <summary>
		/// Constructs the game window. This actually sets up the
		/// various GUI elements, including the menu.
		/// </summary>
		public GameWindow()
		{
			Create();
			Game.Display = display;
		}

		/// <summary>
		/// A pseudo-show function that actually renders the window,
		/// as normal.
		/// </summary>
		public void Show()
		{
			OnNewGameAction(this, new EventArgs());
			window.ShowAll();
		}

		#region Events

		/// <summary>
		/// Triggered when the chain changes.
		/// </summary>
		private void OnChainChanged(
			object sender,
			EventArgs args)
		{
			UpdateScore();
		}

		/// <summary>
		/// Triggered when the high scores are requested.
		/// </summary>
		private void OnHighScoresAction(
			object sender,
			EventArgs args)
		{
			HighScoreWindow window = new HighScoreWindow();
			window.ShowAll();
		}

		/// <summary>
		/// Triggered by the new game menu item.
		/// </summary>
		private void OnNewGameAction(
			object sender,
			EventArgs args)
		{
			Game.NewGame();
			Game.Board.ChainChanged += OnChainChanged;
			UpdateScore();
		}

		/// <summary>
		/// Triggered when the preferences is requested.
		/// </summary>
		private void OnPreferencesAction(
			object sender,
			EventArgs args)
		{
			// Create the window
			ConfigDialog dialog = new ConfigDialog();
			dialog.Run();
			dialog.Destroy();
		}

		/// <summary>
		/// Triggered by the quit menu item.
		/// </summary>
		private void OnQuitAction(
			object sender,
			EventArgs args)
		{
			Game.WriteConfig();
			Application.Quit();
		}

		/// <summary>
		/// Shuffles the board.
		/// </summary>
		private void OnShuffleBoardAction(
			object sender,
			EventArgs args)
		{
			if (Game.State == GameState.InProgress)
			{
				Game.Board.Shuffle();
				UpdateScore();
			}
		}

		/// <summary>
		/// Submits the currently selected word as a score.
		/// </summary>
		private void OnSubmit(
			object sender,
			EventArgs args)
		{
			Game.Board.ScoreChain();
			Game.Display.QueueDraw();
		}

		/// <summary>
		/// Triggered when the window is destroyed.
		/// </summary>
		private void OnWindowDestroyed(
			object sender,
			EventArgs args)
		{
			OnQuitAction(sender, args);
		}

		#endregion

		#region GUI Creation

		private Label currentWord;
		private Display display;
		private Label highestWord;
		private Label longestWord;
		private Button submit;
		private Label totalScore;
		private Label totalWords;
		private Window window;

		/// <summary>
		/// Contains the display for this window.
		/// </summary>
		public Display Display
		{
			get { return display; }
		}

		/// <summary>
		/// Constructs all the GUI element.
		/// </summary>
		private void Create()
		{
			// Start with the basic window
			window = new Window("Moonfire Games' Wordplay");
			//window.SetDefaultSize(400, 400);
			window.DeleteEvent += OnWindowDestroyed;

			// We start with a simple vbox to handle the menu, main,
			// and bottom components.
			VBox outer = new VBox();
			window.Add(outer);

			// Add the menu
			outer.PackStart(CreateMenu(), false, false, 0);

			// We have the score to the right
			HBox hbox = new HBox();
			outer.PackStart(hbox, true, true, 2);

			// The bulk of the window is the game widget
			display = new Display();
			hbox.PackStart(display, true, true, 0);

			// Add the scoring pane
			hbox.PackStart(CreateScorePane(), false, false, 2);
		}

		/// <summary>
		/// Constructs the menu and related components.
		/// </summary>
		private Widget CreateMenu()
		{
			// Start by loading the UI from a resource
			UIManager ui = new UIManager();
			ui.AddUiFromResource("menu.xml");

			// Set up the actions
			ActionEntry[] entries = new[]
			                        {
			                        	// "File" Menu
			                        	new ActionEntry(
			                        		"GameMenu",
			                        		null,
			                        		Translate("Menu/Game"),
			                        		null,
			                        		null,
			                        		null),
			                        	new ActionEntry(
			                        		"NewGame",
			                        		Stock.New,
			                        		Translate("Menu/NewGame"),
			                        		"<control>N",
			                        		"New",
			                        		OnNewGameAction),
			                        	new ActionEntry(
			                        		"ShuffleBoard",
			                        		null,
			                        		Translate("Menu/ShuffleBoard"),
			                        		"<control>S",
			                        		"Shuffle",
			                        		OnShuffleBoardAction),
			                        	new ActionEntry(
			                        		"Preferences",
			                        		null,
			                        		Translate("Menu/Preferences"),
			                        		"<control>P",
			                        		"Preferences",
			                        		OnPreferencesAction),
			                        	new ActionEntry(
			                        		"HighScores",
			                        		null,
			                        		Translate("Menu/HighScores"),
			                        		"<control>H",
			                        		"HighScores",
			                        		OnHighScoresAction),
			                        	new ActionEntry(
			                        		"Quit",
			                        		Stock.Quit,
			                        		Translate("Menu/Quit"),
			                        		"<control>Q",
			                        		"Quit",
			                        		OnQuitAction),
			                        };

			// Install the actions
			ActionGroup actions = new ActionGroup("group");
			actions.Add(entries);
			ui.InsertActionGroup(actions, 0);
			window.AddAccelGroup(ui.AccelGroup);

			// Construct the results
			return ui.GetWidget("/MenuBar");
		}

		/// <summary>
		/// Creates the scoring pane.
		/// </summary>
		private Widget CreateScorePane()
		{
			// Frame it in a vbox
			VBox vbox = new VBox();
			vbox.BorderWidth = 5;

			// Top is score
			string f = "<b>{0}</b>";
			string n = Translate("Score/None");
			Label l = new Label();
			l.Xalign = 0;
			l.Markup = String.Format(f, Translate("Score/TotalScore"));
			vbox.PackStart(l, false, false, 0);

			totalScore = new Label(n);
			vbox.PackStart(totalScore, false, false, 2);

			// Current Word
			l = new Label();
			l.Xalign = 0;
			l.Markup = String.Format(f, Translate("Score/CurrentWord"));
			vbox.PackStart(l, false, false, 5);

			currentWord = new Label(n);
			vbox.PackStart(currentWord, false, false, 2);

			// Longest Word
			l = new Label();
			l.Xalign = 0;
			l.Markup = String.Format(f, Translate("Score/LongestWord"));
			vbox.PackStart(l, false, false, 5);

			longestWord = new Label(n);
			vbox.PackStart(longestWord, false, false, 2);

			// Highest Word
			l = new Label();
			l.Xalign = 0;
			l.Markup = String.Format(f, Translate("Score/HighestWord"));
			vbox.PackStart(l, false, false, 5);

			highestWord = new Label(n);
			vbox.PackStart(highestWord, false, false, 2);

			// Total Words
			l = new Label();
			l.Xalign = 0;
			l.Markup = String.Format(f, Translate("Score/TotalWords"));
			vbox.PackStart(l, false, false, 5);

			totalWords = new Label(n);
			vbox.PackStart(totalWords, false, false, 2);

			// Add a padding
			vbox.PackStart(new Label(), true, true, 0);

			// Add the submit button
			submit = new Button("Submit");
			submit.Clicked += OnSubmit;
			submit.Sensitive = false;
			vbox.PackStart(submit, false, false, 0);

			// Return the box
			return vbox;
		}

		/// <summary>
		/// Updates the scoring fields.
		/// </summary>
		private void UpdateScore()
		{
			// Pull out the fields
			int score = Game.Score.CurrentScore;
			int? cs = Game.Board.CurrentChainScore;
			int cm = Game.Board.CurrentChainMultiplier;
			string text = Game.Board.CurrentChainText;

			// Set the fields
			totalScore.Text = score.ToString("N0");
			totalWords.Text = Game.Score.TotalWords.ToString("N0");

			// Figure out the current word
			submit.Sensitive = false;

			if (text == "")
			{
				currentWord.Text = Translate("Score/None");
			}
			else if (cs == null) // Invalid word
			{
				currentWord.Text = text;
			}
			else
			{
				// Figure out if we have a multiplier
				string sc = ((int) cs).ToString("N0");

				if (cm != 1)
				{
					sc += "x" + cm;
				}

				// Valid word with score
				submit.Sensitive = true;
				currentWord.Text = String.Format("{0} ({1})", text, sc);
			}

			// See if we have a longest word
			if (Game.Score.LongestWord != null)
			{
				longestWord.Text = Game.Score.LongestWord;
			}
			else
			{
				longestWord.Text = Translate("Score/None");
			}

			// See if we have a highest word
			if (Game.Score.HighestWord != null)
			{
				highestWord.Text = String.Format(
					"{0} ({1})", Game.Score.HighestWord, Game.Score.HighestWordScore);
			}
			else
			{
				highestWord.Text = Translate("Score/None");
			}
		}

		#endregion
	}
}
using C5;
using Gtk;
using System;

namespace MfGames.Wordplay
{
	/// <summary>
	/// Displays the high score listings, for all high scores. This
	/// tries to automatically select the current one.
	/// </summary>
	public class HighScoreWindow
	: Window
	{
		private ArrayList <HighScoreWindowEntry> entries =
			new ArrayList <HighScoreWindowEntry>
			(HighScoreTable.MaximumEntries * 4);
		private ComboBox tableList;

		/// <summary>
		/// Constructs the high score listing.
		/// </summary>
		public HighScoreWindow()
			: base(Locale.Translate("HighScore/Title"))
		{
			// Create our GUI elements
			CreateGui();
			ShowAll();
		}

		/// <summary>
		/// Constructs the GUI elements for this window.
		/// </summary>
		private void CreateGui()
		{
			// Frame everything in a hbox
			VBox box = new VBox();
			Add(box);

			// The top has the selection box and Close
			HBox top = new HBox();
			box.PackStart(top, false, false, 2);
			
			tableList = ComboBox.NewText();

			Button close = new Button(Stock.Close);
			close.Clicked += OnClose;
			
			top.PackStart(tableList, false, false, 2);
			top.PackStart(new Label(), true, true, 0);
			top.PackStart(close, false, false, 2);

			// Create a table to frame the inner stuff
			Table outer = new Table(2, 2, true);
			box.PackStart(outer, true, true, 2);

			outer.RowSpacing = 5;
			outer.ColumnSpacing = 5;
			outer.Attach(CreateScoreTable("Total Scores"),
				0, 1, 0, 1,
				AttachOptions.Fill | AttachOptions.Expand,
				AttachOptions.Fill | AttachOptions.Expand,
				0, 0);

			outer.Attach(CreateScoreTable("Total Words"),
				1, 2, 0, 1,
				AttachOptions.Fill, AttachOptions.Fill,
				0, 0);

			outer.Attach(CreateScoreTable("Longest Words"),
				0, 1, 1, 2,
				AttachOptions.Fill, AttachOptions.Fill,
				0, 0);

			outer.Attach(CreateScoreTable("Highest Scoring Words"),
				1, 2, 1, 2,
				AttachOptions.Fill, AttachOptions.Fill,
				0, 0);

			// Populate the list
			if (Game.HighScores.Tables.Count == 0)
			{
				tableList.AppendText("No High Scores");
				tableList.Active = 0;
				tableList.Sensitive = false;
			}
			else
			{
				Game.HighScores.Tables.Sort();

				foreach (HighScoreTable hst in Game.HighScores.Tables)
				{
					tableList.AppendText(hst.ToString());
				}

				tableList.Changed += OnTableChanged;
				tableList.Active = 0;
			}
		}

		/// <summary>
		/// Constructs the scoring table, including the inner fields.
		/// </summary>
		private Widget CreateScoreTable(string title)
		{
			// The Table
			Frame frame = new Frame(title);

			// Add the entries as needed
			Table table =
				new Table((uint) HighScoreTable.MaximumEntries, 4, false);
			table.ColumnSpacing = 10;
			table.RowSpacing = 2;
			frame.Add(table);

			for (uint row = 0; row < HighScoreTable.MaximumEntries; row++)
			{
				// Create the fields
				HighScoreWindowEntry e = new HighScoreWindowEntry();
				entries.Add(e);
				e.Name.Xalign = 0;
				e.UtcWhen.Xalign = 0;
				e.Word.Xalign = 0;
				e.Score.Xalign = 1;

				// Add the name
				table.Attach(e.Name,
					0, 1, row, row + 1,
					AttachOptions.Fill | AttachOptions.Expand,
					AttachOptions.Fill | AttachOptions.Expand,
					0, 0);

				/*
				// Add the time
				table.Attach(e.UtcWhen,
					1, 2, row, row + 1,
					AttachOptions.Fill, AttachOptions.Fill,
					0, 0);
				*/

				// Add the word
				table.Attach(e.Word,
					1, 2, row, row + 1,
					AttachOptions.Fill | AttachOptions.Expand,
					AttachOptions.Fill | AttachOptions.Expand,
					0, 0);

				// Add the score
				table.Attach(e.Score,
					2, 3, row, row + 1,
					AttachOptions.Fill, AttachOptions.Fill,
					0, 0);
			}

			// Return the frame
			return frame;
		}

		/// <summary>
		// Closes this window.
		/// </summary>
		private void OnClose(object sender, EventArgs args)
		{
			Destroy();
		}

		/// <summary>
		/// Triggers an update of the entries.
		/// </summary>
		private void OnTableChanged(object sender, EventArgs args)
		{
			// Go through the list
			foreach (HighScoreTable hst in Game.HighScores.Tables)
			{
				// Ignore if we aren't the same
				if (hst.ToString() == tableList.ActiveText)
				{
					// Populate the list
					int x = HighScoreTable.MaximumEntries;

					PopulateLists(0 * x, hst.TotalScores);
					PopulateLists(1 * x, hst.TotalWords);
					PopulateLists(2 * x, hst.LongestWords);
					PopulateLists(3 * x, hst.HighestWords);
				}
			}

			// Rebuild the list
			ShowAll();
		}

		/// <summary>
		/// Populates the list entries.
		/// </summary>
		private void PopulateLists(int offset, IList <HighScoreEntry> list)
		{
			// Sort it
			list.Sort();

			// Go through the blow away the list
			for (int i = 0; i < HighScoreTable.MaximumEntries; i++)
			{
				// See if we have it
				int n = i + offset;

				if (i > list.Count - 1)
				{
					entries[n].Reset();
					continue;
				}

				// Populate the list
				entries[n].Name.Text    = list[i].Name;
				entries[n].UtcWhen.Text = list[i].UtcWhen.ToString();
				entries[n].Word.Text    = list[i].Word + "";
				entries[n].Score.Text   = list[i].Score.ToString("N0");
			}
		}
	}

	/// <summary>
	/// Inner class for entries
	/// </summary>
	internal class HighScoreWindowEntry
	{
		public Label Name = new Label("");
		public Label UtcWhen = new Label();
		public Label Word = new Label();
		public Label Score = new Label();

		public void Reset()
		{
			Name.Text    = "";
			UtcWhen.Text = "";
			Word.Text    = "";
			Score.Text   = "";
		}
	}
}

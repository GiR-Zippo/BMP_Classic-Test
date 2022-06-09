using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using System.Threading;
using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Seer;
using System.Threading.Tasks;

namespace FFBardMusicPlayer {
	public partial class BmpProcessSelect : Form {

		private bool hasAutoSelected = false;
		public Game selectedGame;
		public EventHandler<Game> OnSelectGame;

		public class MultiboxProcess {
			public Game game;
			public string characterName;
			public string characterId;
			public bool hostProcess = false;
		}
		public List<MultiboxProcess> multiboxProcesses = new List<MultiboxProcess>();
		public bool useLocalOrchestra;

		//private BackgroundWorker processWorker = new BackgroundWorker();
		//private AutoResetEvent processCancelled = new AutoResetEvent(false);


		public BmpProcessSelect() {
			InitializeComponent();

			LocalOrchestraCheck.Visible = false;

			//processWorker.DoWork += ButtonLabelTask;
			//processWorker.WorkerSupportsCancellation = true;

			BmpSeer.Instance.GameStarted += delegate (BardMusicPlayer.Seer.Events.GameStarted e) {
				this.Invoke(t => t.GameStarted(e));
			};

			BmpSeer.Instance.GameStopped += delegate (BardMusicPlayer.Seer.Events.GameStopped e) {
				this.Invoke(t => t.GameStopped(e));
			};

			BmpSeer.Instance.PlayerNameChanged += delegate (BardMusicPlayer.Seer.Events.PlayerNameChanged e) {
				this.Invoke(t => t.GameReady(e));
			};
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
		}

		public void GameReady(BardMusicPlayer.Seer.Events.PlayerNameChanged e)
		{
			if (this.Visible)
			{
                Game game = e.Game;
				foreach (var t in ProcessList.Controls)
				{
					var but = t as Button;
					if (but.Text.Contains(game.Pid.ToString()))
					{
						but.Name = string.Format("{0} ({1})", game.PlayerName, game.Pid);
						but.Invoke(t => t.Text = but.Name);
					}
				}
			}
		}

		public void GameStarted(BardMusicPlayer.Seer.Events.GameStarted e)
		{
			if (this.Visible)
				RefreshList();
			return;
		}

		public void GameStopped(BardMusicPlayer.Seer.Events.GameStopped e)
		{
			if (this.Visible)
				RefreshList();
		}

		private void ButtonLabelTask(object o, DoWorkEventArgs e) {
			// Update the button label with the character name for FFXIV processes
			Dictionary<Game, Button> buttons = new Dictionary<Game, Button>();
			foreach(Button button in ProcessList.Controls) {
				if(button != null) {
					Game game = (button.Tag as Game);
					if(game != null) {
						buttons[game] = button;
					}
				}
			}
			LocalOrchestraCheck.Invoke(t => t.Visible = false);
			multiboxProcesses.Clear();
			// Loop through all buttons and set the name
			while(buttons.Count > 0)
			{
				KeyValuePair<Game, Button> proc = buttons.First();
				Game game = proc.Key;
				Button button = proc.Value;
				buttons.Remove(game);

				string name = "(Unknown)";
				string origName = string.Empty;
				string id = string.Empty;

				origName = game.PlayerName;
				if (string.IsNullOrEmpty(origName))
					name = string.Format("{0} (?)", button.Text);
				else
				name = string.Format("{0} ({1})", origName, game.Pid);
				id = game.ConfigId;

				button.Invoke(t => t.Text = name);
				multiboxProcesses.Add(new MultiboxProcess
				{
					game = game,
					characterName = origName,
					characterId = id,
				});
			}
			//processCancelled.Set();

			// FIXME enable this after testing
			LocalOrchestraCheck.Invoke(t => t.Visible = true);
			//LocalOrchestraCheck.Invoke(t => t.Visible = (multiboxProcesses.Count > 1));
		}

		public void RefreshList() {
			RefreshList(this, EventArgs.Empty);
		}

		public void RefreshList(object o, EventArgs a)
		{
			ProcessList.Controls.Clear();
			//processWorker.CancelAsync();
			if (BmpSeer.Instance.Games.Count == 0)
			{
				HeaderText.Text = "No FFXIV processes found.\nMake sure you run with DX11 on.";
				return;
			}
			else if (BmpSeer.Instance.Games.Count == 1 && !hasAutoSelected)
			{
				DialogResult = DialogResult.Yes;
				selectedGame = BmpSeer.Instance.Games.First().Value;
				OnSelectGame?.Invoke(this, BmpSeer.Instance.Games.First().Value);
				hasAutoSelected = true;
				return;

			}
			else
			{
				HeaderText.Text = "Select FFXIV process:";
				foreach (var game in BmpSeer.Instance.Games)
				{
					Process proc = Process.GetProcessById(game.Value.Pid);
					string debug = string.Format("{0} - {1}", proc.ProcessName, proc.MainWindowTitle);
					int width = ProcessList.Size.Width - 20;
					int height = 20;
					Button button = new Button()
					{
						Text = debug,
						Size = new Size(width, height),
						Margin = new Padding(0),
						FlatStyle = FlatStyle.Popup,
						Tag = game.Value,
						TextAlign = ContentAlignment.MiddleCenter,
					};
					button.Click += Button_Click;
					ProcessList.Controls.Add(button);
					ButtonLabelTask(null, null);
				}

				//CancelProcessWorkerSync();
				/*if (!processWorker.IsBusy)
					processWorker.RunWorkerAsync();*/
			}
		}

		/*public void CancelProcessWorkerSync() {
			if(processWorker.IsBusy) {
				processWorker.CancelAsync();
				processCancelled.WaitOne();
			}
		}*/

		// Button click
		private void Button_Click(object sender, EventArgs e) {
			//CancelProcessWorkerSync();
			DialogResult = DialogResult.Yes;

			Game game = (sender as Button).Tag as Game;

			useLocalOrchestra = LocalOrchestraCheck.Checked;
			if(Control.ModifierKeys == Keys.Shift) {
				useLocalOrchestra = true;
			}
			if(useLocalOrchestra)
			{
				for(int i = multiboxProcesses.Count - 1; i >= 0; i--)
				{
					if(multiboxProcesses[i].game == game)
					{
						multiboxProcesses[i].hostProcess = true;
					}
				}
			}

			selectedGame = game;
			OnSelectGame?.Invoke(this, game);
		}

		// Form buttons
		private void RefreshButton_Click(object sender, EventArgs e) {
			RefreshList();
		}

		private void AllProcessCheck_CheckedChanged(object sender, EventArgs e) {
			RefreshList();
		}

		private void CancelButton_Click(object sender, EventArgs e) {
			//CancelProcessWorkerSync();
			DialogResult = DialogResult.No;
		}

		private void LocalOrchestraCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			BmpPigeonhole.Instance.LocalOrchestra = LocalOrchestraCheck.Checked;
		}

	}
}

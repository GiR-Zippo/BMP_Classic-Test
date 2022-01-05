using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static FFBardMusicPlayer.BmpProcessSelect;

using Timer = System.Timers.Timer;
using System.Timers;
using System.Diagnostics;
using BardMusicPlayer.Seer;

namespace FFBardMusicPlayer.Controls {
	public partial class BmpLocalOrchestra : UserControl {

		public EventHandler<bool> onMemoryCheck;
		public class SyncData {
			public Dictionary<uint, long> idTimestamp = new Dictionary<uint, long>();
		};

        private BmpSequencer parentSequencer;
        public BmpSequencer Sequencer
        {
            set
            {
                parentSequencer = value;

                this.UpdatePerformers(value);
                this.UpdateMemory();
            }

            get
            {
                return parentSequencer;
            }
        }

        private bool orchestraEnabled = false;
		public bool OrchestraEnabled {
			get { return orchestraEnabled; }
			set {
				orchestraEnabled = value;
			}
		}

		List<BmpLocalPerformer> _performers = new List<BmpLocalPerformer>();

		public BmpLocalOrchestra() {
			InitializeComponent();
		}

		private void StartSyncWorker() {
			BackgroundWorker syncWorker = new BackgroundWorker();
			syncWorker.WorkerSupportsCancellation = true;
			syncWorker.DoWork += SyncWorker_DoWork;
			syncWorker.RunWorkerCompleted += SyncWorker_RunWorkerCompleted;

			this.UpdateMemory();

			List<uint> actorIds = new List<uint>();
			foreach(Control ctl in PerformerPanel.Controls) {
				BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
				if(performer != null && performer.PerformerEnabled && performer.PerformanceUp) {
					actorIds.Add(performer.actorId);
				}
			}
			syncWorker.RunWorkerAsync(actorIds);

			onMemoryCheck.Invoke(this, true);
		}

		private void SyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			SyncData data = (e.Result as SyncData);

			StringBuilder debugDump = new StringBuilder();
			
			if (BmpSeer.Instance.Games.Count > 0)
			{
				foreach (var game in BmpSeer.Instance.Games)
				{
					debugDump.AppendLine(string.Format("{0} MS {1}", game.Value.PlayerName, game.Key));
				}
			}

			onMemoryCheck.Invoke(this, false);

			//MessageBox.Show(this.Parent, debugDump.ToString());
			Console.WriteLine(debugDump.ToString());
		}

		// Start memory poll sync

		private void SyncWorker_DoWork(object sender, DoWorkEventArgs e) {
			BackgroundWorker worker = (sender as BackgroundWorker);
			List<uint> actorIds = (e.Argument as List<uint>);
			SyncData data = new SyncData();
			Dictionary<int, Game> performanceToActor = new Dictionary<int, Game>();
			foreach (var game in BmpSeer.Instance.Games)
			{
				if (actorIds.Contains(game.Value.ActorId))
					performanceToActor[game.Value.Pid] = game.Value;
			}	

			if(e.Cancel) {
				return;
			}

			List<Game> performanceCache;
			List<int> perfKeys = performanceToActor.Keys.ToList();

			Stopwatch msCounter = Stopwatch.StartNew();
			DateTime now = DateTime.Now;

			while(!worker.CancellationPending)
			{
				if(e.Cancel) {
					return;
				}
						/*performanceCache = BmpSeer.Instance.Games.Values.ToList();

						foreach(int pid in perfKeys) {
							if(performanceToActor.ContainsKey(pid)) {
								// Check it
								if(performanceCache[pid].Pid > 0) {
									uint aid = performanceToActor[pid];
									//data.idTimestamp[aid] = msCounter.ElapsedMilliseconds;
									data.idTimestamp[aid] = (long)((DateTime.Now - now).TotalMilliseconds);
									performanceToActor.Remove(pid);
								}
							}
						}
						if(perfKeys.Count != performanceToActor.Keys.Count) {
							perfKeys = performanceToActor.Keys.ToList();
						}
						if(performanceToActor.Keys.Count == 0) {
							break;
						}*/
			}

			e.Result = data;
		}

		public void AddLocalProcesses(Game game, bool hostProcess = false)
		{
			PerformerPanel.Controls.Clear();
			BmpLocalPerformer perf = new BmpLocalPerformer(game);
			perf.Dock = DockStyle.Top;
			if (hostProcess == true)
			{
				perf.hostProcess = true;
				_performers.Insert(0, perf);
			}
			else
				_performers.Add(perf);

			for (int i = 0; i < _performers.Count; i++)
			{
				perf = _performers[i];
				perf.TrackNum = i + 1;
				PerformerPanel.Controls.Add(perf);
			}
		}

		public void RemoveLocalProcesses(int Pid, bool hostProcess = false)
		{
			PerformerPanel.Controls.Clear();
			foreach (var performer in _performers)
			{
				if (performer.game.Pid == Pid)
				{
					_performers.Remove(performer);
					break;
				}
			}

			BmpLocalPerformer perf;
			for (int i = 0; i < _performers.Count; i++)
			{
				perf = _performers[i];
				perf.TrackNum = i + 1;
				PerformerPanel.Controls.Add(perf);
			}
		}

		public void PopulateLocalProcesses(List<MultiboxProcess> processes) {
			PerformerPanel.Controls.Clear();

			int track = 1;
			foreach(MultiboxProcess mp in processes) {
				BmpLocalPerformer perf = new BmpLocalPerformer(mp);
				perf.Dock = DockStyle.Top;

				if(mp.hostProcess == true) {
					perf.hostProcess = true;
					_performers.Insert(0, perf);
				} else {
					_performers.Add(perf);
				}
				track++;
			}
			for(int i = 0; i < _performers.Count; i++) {
				BmpLocalPerformer perf = _performers[i];
				perf.TrackNum = i+1;
				PerformerPanel.Controls.Add(perf);
			}
		}

		public void UpdateMemory()
		{
			List<string> performerNames = GetPerformerNames();
			foreach (Game game in BmpSeer.Instance.Games.Values.ToList())
			{
				if (performerNames.Contains(game.PlayerName))
				{
					BmpLocalPerformer perf = this.FindPerformer(game.PlayerName, game.ActorId);
					if (perf != null)
					{
						perf.PerformanceUp = !game.InstrumentHeld.Equals(BardMusicPlayer.Quotidian.Structs.Instrument.None);
						perf.actorId = game.ActorId;
					}
				}
			}
		}

		public void UpdatePerformers(BmpSequencer seq) {
			if(seq == null) {
				return;
			}
			foreach(Control ctl in PerformerPanel.Controls) {
				BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
				if(performer != null) {
					performer.Sequencer = seq;
				}
			}
		}

		public void PerformerProgress(int prog) {
			foreach(Control ctl in PerformerPanel.Controls) {
				BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
				if(performer != null) {
					performer.SetProgress(prog);
				}
			}
		}

		public void PerformerPlay(bool play) {
			foreach(Control ctl in PerformerPanel.Controls) {
				BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
				if(performer != null) {
					performer.Play(play);
				}
			}
		}
		public void PerformerStop() {
			foreach(Control ctl in PerformerPanel.Controls) {
				BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
				if(performer != null) {
					performer.Stop();
				}
			}
		}

		public List<string> GetPerformerNames() {
			List<string> performerNames = new List<string>();
			foreach(BmpLocalPerformer performer in PerformerPanel.Controls) {
				performerNames.Add(performer.PerformerName);
			}
			return performerNames;
		}

        public BmpLocalPerformer FindPerformer(string name, uint actorId)
        {
            foreach (Control ctl in PerformerPanel.Controls)
            {
                BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
                if (performer != null)
                {
                    // few things can happen here
                    if (performer.PerformerName == name)
                    {
                        if (performer.actorId == 0)
                        {
                            // here, we've found the performer with the name for the first time
                            // after returning here, the perfId and actorId should be set
                            // so, just return them, i guess
                            return performer;
                        }
                        else if (performer.actorId == actorId)
                        {
                            // we've seen this performer before, and can damn well make sure that
                            // this /is/ the character we want to return
                            return performer;
                        }
                    }
                }
            }

            // all else fails, blame the one person in BMP that decided to give
            // muiltiple characters the exact same name.
            return null;
        }

        private void openInstruments_Click(object sender, EventArgs e) {
			
			foreach(Control ctl in PerformerPanel.Controls) {
				BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
				if(performer != null && performer.PerformerEnabled) {
					performer.OpenInstrument();
				}
			}
		}

		private void closeInstruments_Click(object sender, EventArgs e) {
            parentSequencer.Pause();
            foreach (Control ctl in PerformerPanel.Controls) {
				BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
				if(performer != null && performer.PerformerEnabled) {
					performer.CloseInstrument();
				}
			}
		}

		private void muteAll_Click(object sender, EventArgs e) {
			
			foreach(Control ctl in PerformerPanel.Controls) {
				BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
				if(performer != null && performer.PerformerEnabled) {
					performer.ToggleMute();
				}
			}
		}

		private void ensembleCheck_Click(object sender, EventArgs e) {

			Timer openTimer = new Timer {
				Interval = 500
			};
			openTimer.Elapsed += delegate (object o, ElapsedEventArgs ev) {
				openTimer.Stop();
				openTimer = null;
				
				foreach(Control ctl in PerformerPanel.Controls) {
					BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
					if(performer != null && performer.PerformerEnabled && !performer.hostProcess) {
						performer.EnsembleAccept();
					}
				}
			};
			openTimer.Start();
		}

		private void testC_Click(object sender, EventArgs e) {

			//StartSyncWorker();

			foreach(Control ctl in PerformerPanel.Controls) {
				BmpLocalPerformer performer = (ctl as BmpLocalPerformer);
				if(performer != null && performer.PerformerEnabled) {
					performer.NoteKey("C");
				}
			}
		}
	}
}

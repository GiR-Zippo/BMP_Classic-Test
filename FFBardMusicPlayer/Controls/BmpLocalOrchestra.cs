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
using System.Threading.Tasks;
using FFBardMusicPlayer.MidiAndSequencing;

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

		public BmpLocalOrchestra() {
			InitializeComponent();
		}

		public void AddLocalProcesses(Game game, bool hostProcess = false)
		{
			PerformerPanel.Controls.Add(LocalOrchestraHandler.Instance.AddLocalPerformer(game, hostProcess));
			updatePanel(false);
		}

		public void UpdateLocalProcesses(Game game, bool hostProcess = false)
		{
			LocalOrchestraHandler.Instance.UpdateLocalPerformer(game, hostProcess);
			updatePanel(false);
		}

		public void RemoveLocalProcesses(int Pid, bool hostProcess = false)
		{
			LocalOrchestraHandler.Instance.RemoveLocalProcesses(Pid);
			updatePanel();
		}

		public void PopulateLocalProcesses(List<MultiboxProcess> processes) 
		{
			LocalOrchestraHandler.Instance.PopulateLocalProcesses(processes);
			updatePanel();
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
			LocalOrchestraHandler.Instance.UpdatePerformers(seq);
		}

		public void PerformerProgress(int prog) {
			LocalOrchestraHandler.Instance.PerformerProgress(prog);
		}

		public void PerformerPlay(bool play)
		{
			LocalOrchestraHandler.Instance.PerformerPlay(play);
		}
		public void PerformerStop() {
			LocalOrchestraHandler.Instance.PerformerStop();
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

		private void updatePanel(bool initTracknum = true)
		{
			PerformerPanel.Controls.Clear();
			for (int i = 0; i < LocalOrchestraHandler.Instance.Performers.Count; i++)
			{
				BmpLocalPerformer perf = LocalOrchestraHandler.Instance.Performers[i];
				perf.TrackNum = initTracknum ? i + 1 : LocalOrchestraHandler.Instance.Performers[i].TrackNum;
				PerformerPanel.Controls.Add(perf);
			}
		}

		private void openInstruments_Click(object sender, EventArgs e)
		{
			LocalOrchestraHandler.Instance.OpenInstruments();
		}

		private void closeInstruments_Click(object sender, EventArgs e) {
            parentSequencer.Pause();
			LocalOrchestraHandler.Instance.CloseInstruments();
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

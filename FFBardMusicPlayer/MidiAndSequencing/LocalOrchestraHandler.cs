using BardMusicPlayer.Seer;
using FFBardMusicPlayer.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FFBardMusicPlayer.BmpProcessSelect;

namespace FFBardMusicPlayer.MidiAndSequencing
{
    internal class LocalOrchestraHandler
    {
        List<BmpLocalPerformer> _performers = new List<BmpLocalPerformer>();
#region Instance Constructor/Destructor
        private static readonly Lazy<LocalOrchestraHandler> LazyInstance = new(() => new LocalOrchestraHandler());
        private LocalOrchestraHandler()
        {
            _performers.Clear();
        }

        public static LocalOrchestraHandler Instance => LazyInstance.Value;

        ~LocalOrchestraHandler() { Dispose(); }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        public List<BmpLocalPerformer> Performers
        { 
            get
            {
                return _performers;
            } 
        }

        public BmpLocalPerformer AddLocalPerformer(Game game, bool hostProcess = false)
        {
            BmpLocalPerformer perf = new BmpLocalPerformer(game);
            perf.Dock = DockStyle.Top;
            perf.hostProcess = hostProcess;

            List<int> tracks = new List<int>();
            for (int i = 0; i < _performers.Count; i++)
                tracks.Add(_performers[i].TrackNum);

            var result = Enumerable.Range(1, _performers.Count+1).Except(tracks);
            perf.TrackNum = result.First();

            if (hostProcess)
                _performers.Insert(0, perf);
            else
                _performers.Add(perf);

            return perf;
        }

        public void UpdateLocalPerformer(Game game, bool hostProcess = false)
        {
            int index = _performers.FindIndex(t => t.game.Pid == game.Pid);
            if (index != -1)
            {
                var oldPerformer = _performers[index];

                BmpLocalPerformer perf = new BmpLocalPerformer(game);
                perf.Dock = DockStyle.Top;
                perf.hostProcess = oldPerformer.hostProcess;
                perf.TrackNum = oldPerformer.TrackNum;
                _performers[index] = perf;
            }
        }

        public void RemoveLocalProcesses(int Pid)
        {
            var res = _performers.AsParallel().Where(i => i.game.Pid == Pid);
            if (res.Count() <= 0)
                return;
            _performers.Remove(res.First());
        }

        public void PopulateLocalProcesses(List<MultiboxProcess> processes)
        {
            int track = 1;
            foreach (MultiboxProcess mp in processes)
            {
                BmpLocalPerformer perf = new BmpLocalPerformer(mp);
                perf.Dock = DockStyle.Top;

                if (mp.hostProcess == true)
                {
                    perf.hostProcess = true;
                    _performers.Insert(0, perf);
                }
                else
                {
                    _performers.Add(perf);
                }
                track++;
            }
        }

        public void UpdatePerformers(BmpSequencer seq)
        {
            if (seq == null)
                return;

            foreach (var performer in _performers)
                if (performer != null)
                    performer.Sequencer = seq;
        }

        public void PerformerProgress(int prog)
        {
            foreach (var performer in _performers)
                if (performer != null)
                    performer.SetProgress(prog);
        }

        public void PerformerPlay(bool play)
        {
            foreach (var performer in _performers)
                if (performer != null)
                    performer.Play(play);
        }

        public void PerformerStop()
        {
            foreach (var performer in _performers)
                if (performer != null)
                    performer.Stop();
        }

        public void OpenInstruments()
        {
            foreach (var performer in _performers)
                if (performer != null && performer.PerformerEnabled)
                        performer.OpenInstrument();
        }

        public void CloseInstruments()
        {
            foreach (var performer in _performers)
                if (performer != null && performer.PerformerEnabled)
                        performer.CloseInstrument();
        }

    }
}

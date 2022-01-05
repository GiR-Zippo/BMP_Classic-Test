using BardMusicPlayer.Pigeonhole;
using FFBardMusicCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFBardMusicPlayer.Forms
{
	/// <summary>
	/// BmpMain songbrowser routines
	/// </summary>
	public partial class BmpMain
	{
		// Use invoke on gui changing properties
		private void Browser_OnMidiSelect(object o, BmpMidiEntry entry)
		{
			Load_Midi(entry.FilePath.FilePath, entry.Track.Track);
		}

		private void Browser_OnRewindMidi(object o, bool entry)
		{
			if (LocalOrchestra.OrchestraEnabled)
				LocalOrchestra.Sequencer.Stop();
			else
				Player.Player.Stop();
		}

		private void Browser_OnOpenSong(object o, bool entry)
		{
			OpenFileDialog selectMidi = new OpenFileDialog();

			selectMidi.Filter = "midi files (*.mid)|*.mid";
			selectMidi.FilterIndex = 2;
			selectMidi.RestoreDirectory = true;

			if (selectMidi.ShowDialog() != DialogResult.OK)
				return;

			if (LocalOrchestra.OrchestraEnabled)
				LocalOrchestra.Sequencer.Stop();
			else
				Player.Player.Stop();

			Explorer.SelectFile(selectMidi.FileName);
			Explorer.SelectTrack(Explorer.SelectedTrack);
			Load_Midi(selectMidi.FileName, Explorer.SelectedTrack);

		}

		private void Load_Midi(string FileName, int Track)
		{
			bool error = false;
			bool diff = (FileName != Player.Player.LoadedFilename);
			try
			{
				Player.LoadFile(FileName, Track);
				Player.Player.Stop();
			}
			catch (Exception e)
			{
				this.LogMidi(string.Format("[{0}] cannot be loaded:", FileName));
				this.LogMidi(e.Message);
				Console.WriteLine(e.StackTrace);
				error = true;
			}
			if (!error)
			{
				if (diff && BmpPigeonhole.Instance.Verbose)
				{
					this.LogMidi(string.Format("[{0}] loaded.", FileName));
				}
				BmpPigeonhole.Instance.LastLoaded = FileName;
			}
			Playlist.Deselect();

			this.SuspendLayout();
			Explorer.SetTrackName(FileName);
			Explorer.SetTrackNums(Player.Player.CurrentTrack, Player.Player.MaxTrack);
			this.ResumeLayout(true);

			Explorer.SongBrowserVisible = false;

			Statistics.SetBpmCount(Player.Tempo);
			Statistics.SetTotalTrackCount(Player.Player.MaxTrack);
			Statistics.SetTotalNoteCount(Player.TotalNoteCount);
			Statistics.SetTrackNoteCount(Player.CurrentNoteCount);

			if (LocalOrchestra.OrchestraEnabled)
			{
				LocalOrchestra.Sequencer = Player.Player;
			}
		}
	}
}

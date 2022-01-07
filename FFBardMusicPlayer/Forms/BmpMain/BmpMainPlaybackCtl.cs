using BardMusicPlayer.Pigeonhole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFBardMusicPlayer.Forms
{
	/// <summary>
	/// BmpMain playback controls
	/// </summary>
	public partial class BmpMain
	{
		/// <summary>
		/// Called if play status has changed
		/// </summary>
		private void OnPlayStatusChange(Object o, bool playing)
		{
			if (!playing)
			{
				ChatLogAll.AppendRtf(BmpChatParser.FormatRtf("Playback paused."));
				if (LocalOrchestra.OrchestraEnabled)
					LocalOrchestra.PerformerPlay(false);

				FFXIV.hook.ClearLastPerformanceKeybinds();
			}
			else
			{
				ChatLogAll.AppendRtf(BmpChatParser.FormatRtf("Playback resumed."));
				if (LocalOrchestra.OrchestraEnabled)
					LocalOrchestra.PerformerPlay(true);

				Statistics.Restart();
				if (BmpPigeonhole.Instance.BringGametoFront)
					FFXIV.hook.FocusWindow();
			}
		}

		/// <summary>
		/// Called if playback has stopped
		/// </summary>
		private void OnPlayStatusEnded(object o, EventArgs e)
		{
			if (LocalOrchestra.OrchestraEnabled)
				LocalOrchestra.PerformerStop();

			if (Player.Loop)
			{
				Player.Player.Play();
				if (LocalOrchestra.OrchestraEnabled)
					LocalOrchestra.PerformerPlay(true);
			}
			else
			{
				proceedPlaylistMidi = true;
				this.NextSong();
			}
		}

		/// <summary>
		/// Called if progressbar was moved manually
		/// </summary>
		private void OnPlayProgressChange(Object o, int progress)
		{
			if (LocalOrchestra.OrchestraEnabled)
                LocalOrchestra.PerformerProgress(progress);
		}

		/// <summary>
		/// Called if the song should skipped
		/// </summary>
		private void OnSongSkip(Object o, EventArgs a)
		{
			if (LocalOrchestra.OrchestraEnabled)
				LocalOrchestra.PerformerStop();
			else
				Player.Player.Stop();

			proceedPlaylistMidi = true;
			NextSong();
		}
	}
}

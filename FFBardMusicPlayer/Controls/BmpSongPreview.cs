using System;
using System.Linq;
using System.Windows.Forms;
using BardMusicPlayer.Transmogrify.Song;
using BardMusicPlayer.Siren;

namespace FFBardMusicPlayer.Controls {
	public partial class BmpSongPreview : UserControl {

		public BmpSongPreview() {
			InitializeComponent();
			Volume.Value = BmpSiren.Instance.GetVolume();
			BmpSiren.Instance.SynthTimePositionChanged += Instance_SynthTimePositionChanged;
		}

        private void Instance_SynthTimePositionChanged(string songTitle, double currentTime, double endTime, int activeVoices)
        {
			this.Invoke(t => t.TimePosChanged(currentTime, endTime));
		}

		void TimePosChanged(double currentTime, double endTime)
		{
			if (currentTime == endTime)
				return;
			trackBar1.Maximum = (int)endTime/100;
			trackBar1.Value = (int)currentTime / 100;
		}


		public void Restart() {
		}

		public void SetBpmCount(int bpm) {
			songName.Invoke(t => t.Text = bpm.ToString());
		}
		public void SetTotalTrackCount(int trackcount) {
			trkCount.Invoke(t => t.Text = trackcount.ToString());
		}

        private void LoadSong_Click(object sender, EventArgs e)
        {
			var openFileDialog = new OpenFileDialog
			{
				Filter = "MIDI file|*.mid;*.midi|All files (*.*)|*.*",
				Multiselect = true
			};

			openFileDialog.ShowDialog();

			if (!openFileDialog.CheckFileExists)
				return;

			if (openFileDialog.FileName.Length <= 0)
				return;

			BmpSong song = BmpSong.OpenFile(openFileDialog.FileName).Result;
			songName.Text = song.Title;
			trkCount.Text = (song.TrackContainers.Count()-1).ToString();
			_ = BmpSiren.Instance.Load(song);
		}

        private void PlaySong_Click(object sender, EventArgs e)
        {
			if(BmpSiren.Instance.IsReadyForPlayback)
				BmpSiren.Instance.Play();
        }

        private void StopSong_Click(object sender, EventArgs e)
        {
			if (BmpSiren.Instance.IsReadyForPlayback)
				BmpSiren.Instance.Stop();
		}

        private void Volume_Scroll(object sender, EventArgs e)
        {
			var g = Volume.Value;
			BmpSiren.Instance.SetVolume(g);

		}

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
			this.Invoke(t => t.SetPos(trackBar1.Value));
        }

		private void SetPos(int newpos)
		{
			BmpSiren.Instance.SetPosition(newpos * 100);
		}

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
			BmpSiren.Instance.Pause();
		}

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
			BmpSiren.Instance.Play();
		}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFBardMusicPlayer.Components {
	public partial class BmpTrackShift : NumericUpDown {
		public BmpTrackShift() {
			InitializeComponent();

			this.BackColor = Color.FromArgb(200, 200, 200);
			this.ForeColor = Color.FromArgb(20, 20, 20);

			this.TextAlign = HorizontalAlignment.Center;
		}

		protected override void UpdateEditText() {
            this.ChangingText = true;
			this.Text = "t" + this.Value.ToString();
            this.ChangingText = false;
		}
	}
}

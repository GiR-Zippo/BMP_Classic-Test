namespace FFBardMusicPlayer.Controls {
	partial class BmpSongPreview {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.trkCount = new System.Windows.Forms.Label();
            this.trkText = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LoadSong = new System.Windows.Forms.Button();
            this.StopSong = new System.Windows.Forms.Button();
            this.PlaySong = new System.Windows.Forms.Button();
            this.songName = new System.Windows.Forms.Label();
            this.Volume = new System.Windows.Forms.TrackBar();
            this.songnameText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.midiStatsBox = new System.Windows.Forms.GroupBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Volume)).BeginInit();
            this.midiStatsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // trkCount
            // 
            this.trkCount.Location = new System.Drawing.Point(138, 36);
            this.trkCount.Name = "trkCount";
            this.trkCount.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.trkCount.Size = new System.Drawing.Size(71, 20);
            this.trkCount.TabIndex = 7;
            this.trkCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkText
            // 
            this.trkText.Location = new System.Drawing.Point(6, 36);
            this.trkText.Name = "trkText";
            this.trkText.Size = new System.Drawing.Size(126, 20);
            this.trkText.TabIndex = 6;
            this.trkText.Text = "Total tracks";
            this.trkText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel1.Controls.Add(this.LoadSong, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.StopSong, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.PlaySong, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 117);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(81, 31);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // LoadSong
            // 
            this.LoadSong.Location = new System.Drawing.Point(3, 3);
            this.LoadSong.Name = "LoadSong";
            this.LoadSong.Size = new System.Drawing.Size(20, 23);
            this.LoadSong.TabIndex = 14;
            this.LoadSong.Text = "^";
            this.LoadSong.UseVisualStyleBackColor = true;
            this.LoadSong.Click += new System.EventHandler(this.LoadSong_Click);
            // 
            // StopSong
            // 
            this.StopSong.Location = new System.Drawing.Point(55, 3);
            this.StopSong.Name = "StopSong";
            this.StopSong.Size = new System.Drawing.Size(22, 23);
            this.StopSong.TabIndex = 16;
            this.StopSong.Text = "■";
            this.StopSong.UseVisualStyleBackColor = true;
            this.StopSong.Click += new System.EventHandler(this.StopSong_Click);
            // 
            // PlaySong
            // 
            this.PlaySong.Location = new System.Drawing.Point(29, 3);
            this.PlaySong.Name = "PlaySong";
            this.PlaySong.Size = new System.Drawing.Size(20, 23);
            this.PlaySong.TabIndex = 15;
            this.PlaySong.Text = ">";
            this.PlaySong.UseVisualStyleBackColor = true;
            this.PlaySong.Click += new System.EventHandler(this.PlaySong_Click);
            // 
            // songName
            // 
            this.songName.Location = new System.Drawing.Point(138, 16);
            this.songName.Name = "songName";
            this.songName.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.songName.Size = new System.Drawing.Size(281, 20);
            this.songName.TabIndex = 5;
            this.songName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Volume
            // 
            this.Volume.BackColor = System.Drawing.SystemColors.Window;
            this.Volume.Location = new System.Drawing.Point(317, 52);
            this.Volume.Maximum = 100;
            this.Volume.Name = "Volume";
            this.Volume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Volume.Size = new System.Drawing.Size(45, 96);
            this.Volume.SmallChange = 50;
            this.Volume.TabIndex = 10;
            this.Volume.Scroll += new System.EventHandler(this.Volume_Scroll);
            // 
            // songnameText
            // 
            this.songnameText.Location = new System.Drawing.Point(6, 16);
            this.songnameText.Name = "songnameText";
            this.songnameText.Size = new System.Drawing.Size(126, 20);
            this.songnameText.TabIndex = 4;
            this.songnameText.Text = "Song Name";
            this.songnameText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(321, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Volume";
            // 
            // midiStatsBox
            // 
            this.midiStatsBox.Controls.Add(this.trackBar1);
            this.midiStatsBox.Controls.Add(this.label1);
            this.midiStatsBox.Controls.Add(this.songnameText);
            this.midiStatsBox.Controls.Add(this.Volume);
            this.midiStatsBox.Controls.Add(this.songName);
            this.midiStatsBox.Controls.Add(this.tableLayoutPanel1);
            this.midiStatsBox.Controls.Add(this.trkText);
            this.midiStatsBox.Controls.Add(this.trkCount);
            this.midiStatsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.midiStatsBox.Location = new System.Drawing.Point(0, 0);
            this.midiStatsBox.Name = "midiStatsBox";
            this.midiStatsBox.Size = new System.Drawing.Size(522, 293);
            this.midiStatsBox.TabIndex = 0;
            this.midiStatsBox.TabStop = false;
            this.midiStatsBox.Text = "Midi statistics";
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.SystemColors.Window;
            this.trackBar1.Location = new System.Drawing.Point(9, 66);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(283, 45);
            this.trackBar1.TabIndex = 19;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseDown);
            this.trackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseUp);
            // 
            // BmpSongPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.midiStatsBox);
            this.Name = "BmpSongPreview";
            this.Size = new System.Drawing.Size(522, 293);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Volume)).EndInit();
            this.midiStatsBox.ResumeLayout(false);
            this.midiStatsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

		}

        #endregion
        private System.Windows.Forms.Label trkCount;
        private System.Windows.Forms.Label trkText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button LoadSong;
        private System.Windows.Forms.Button StopSong;
        private System.Windows.Forms.Button PlaySong;
        private System.Windows.Forms.Label songName;
        private System.Windows.Forms.TrackBar Volume;
        private System.Windows.Forms.Label songnameText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox midiStatsBox;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}

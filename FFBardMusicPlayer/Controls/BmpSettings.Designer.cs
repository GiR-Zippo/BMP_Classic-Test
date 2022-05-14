namespace FFBardMusicPlayer.Controls {
	partial class BmpSettings {
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
            this.components = new System.ComponentModel.Container();
            this.GeneralSettings = new System.Windows.Forms.GroupBox();
            this.KeyboardTest = new System.Windows.Forms.Button();
            this.SignatureFolder = new System.Windows.Forms.Button();
            this.SettingsScrollPanel = new System.Windows.Forms.Panel();
            this.SettingsTable = new System.Windows.Forms.TableLayoutPanel();
            this.ThanksTable = new System.Windows.Forms.GroupBox();
            this.StaffNames = new System.Windows.Forms.TextBox();
            this.ChatSettings = new System.Windows.Forms.GroupBox();
            this.SettingBringGame = new System.Windows.Forms.CheckBox();
            this.UnequipPause = new System.Windows.Forms.CheckBox();
            this.SettingBringBmp = new System.Windows.Forms.CheckBox();
            this.PlaybackSettings = new System.Windows.Forms.GroupBox();
            this.SettingHoldNotes = new System.Windows.Forms.CheckBox();
            this.ForcePlayback = new System.Windows.Forms.CheckBox();
            this.MidiInputLabel = new System.Windows.Forms.Label();
            this.SettingMidiInput = new System.Windows.Forms.ComboBox();
            this.MiscSettings = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AutostartMethod = new System.Windows.Forms.ComboBox();
            this.SettingChatSave = new System.Windows.Forms.CheckBox();
            this.midibardCheckbox = new System.Windows.Forms.CheckBox();
            this.verboseToggle = new System.Windows.Forms.CheckBox();
            this.HelpTip = new System.Windows.Forms.ToolTip(this.components);
            this.GeneralSettings.SuspendLayout();
            this.SettingsScrollPanel.SuspendLayout();
            this.SettingsTable.SuspendLayout();
            this.ThanksTable.SuspendLayout();
            this.ChatSettings.SuspendLayout();
            this.PlaybackSettings.SuspendLayout();
            this.MiscSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // GeneralSettings
            // 
            this.GeneralSettings.AutoSize = true;
            this.GeneralSettings.BackColor = System.Drawing.Color.Transparent;
            this.GeneralSettings.Controls.Add(this.KeyboardTest);
            this.GeneralSettings.Controls.Add(this.SignatureFolder);
            this.GeneralSettings.Controls.Add(this.SettingsScrollPanel);
            this.GeneralSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GeneralSettings.Location = new System.Drawing.Point(0, 0);
            this.GeneralSettings.Margin = new System.Windows.Forms.Padding(0);
            this.GeneralSettings.Name = "GeneralSettings";
            this.GeneralSettings.Size = new System.Drawing.Size(546, 325);
            this.GeneralSettings.TabIndex = 9;
            this.GeneralSettings.TabStop = false;
            this.GeneralSettings.Text = "Settings";
            // 
            // KeyboardTest
            // 
            this.KeyboardTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.KeyboardTest.Location = new System.Drawing.Point(313, 3);
            this.KeyboardTest.Name = "KeyboardTest";
            this.KeyboardTest.Size = new System.Drawing.Size(97, 23);
            this.KeyboardTest.TabIndex = 0;
            this.KeyboardTest.Text = "Test keyboard";
            this.KeyboardTest.UseVisualStyleBackColor = true;
            // 
            // SignatureFolder
            // 
            this.SignatureFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SignatureFolder.Location = new System.Drawing.Point(416, 3);
            this.SignatureFolder.Name = "SignatureFolder";
            this.SignatureFolder.Size = new System.Drawing.Size(116, 23);
            this.SignatureFolder.TabIndex = 13;
            this.SignatureFolder.Text = "Open Data folder";
            this.SignatureFolder.UseVisualStyleBackColor = true;
            this.SignatureFolder.Visible = false;
            // 
            // SettingsScrollPanel
            // 
            this.SettingsScrollPanel.AutoScroll = true;
            this.SettingsScrollPanel.Controls.Add(this.SettingsTable);
            this.SettingsScrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsScrollPanel.Location = new System.Drawing.Point(3, 18);
            this.SettingsScrollPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsScrollPanel.Name = "SettingsScrollPanel";
            this.SettingsScrollPanel.Padding = new System.Windows.Forms.Padding(8);
            this.SettingsScrollPanel.Size = new System.Drawing.Size(540, 304);
            this.SettingsScrollPanel.TabIndex = 12;
            // 
            // SettingsTable
            // 
            this.SettingsTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsTable.ColumnCount = 2;
            this.SettingsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.SettingsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.SettingsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.SettingsTable.Controls.Add(this.ThanksTable, 1, 1);
            this.SettingsTable.Controls.Add(this.ChatSettings, 0, 0);
            this.SettingsTable.Controls.Add(this.PlaybackSettings, 1, 0);
            this.SettingsTable.Controls.Add(this.MiscSettings, 0, 1);
            this.SettingsTable.Location = new System.Drawing.Point(11, 11);
            this.SettingsTable.Name = "SettingsTable";
            this.SettingsTable.RowCount = 2;
            this.SettingsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SettingsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.SettingsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.SettingsTable.Size = new System.Drawing.Size(518, 252);
            this.SettingsTable.TabIndex = 18;
            // 
            // ThanksTable
            // 
            this.ThanksTable.Controls.Add(this.StaffNames);
            this.ThanksTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThanksTable.Location = new System.Drawing.Point(260, 100);
            this.ThanksTable.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.ThanksTable.Name = "ThanksTable";
            this.ThanksTable.Padding = new System.Windows.Forms.Padding(0);
            this.ThanksTable.Size = new System.Drawing.Size(258, 152);
            this.ThanksTable.TabIndex = 26;
            this.ThanksTable.TabStop = false;
            this.ThanksTable.Text = "Huh, nothing here?";
            // 
            // StaffNames
            // 
            this.StaffNames.Location = new System.Drawing.Point(5, 18);
            this.StaffNames.Multiline = true;
            this.StaffNames.Name = "StaffNames";
            this.StaffNames.ReadOnly = true;
            this.StaffNames.Size = new System.Drawing.Size(290, 120);
            this.StaffNames.TabIndex = 0;
            this.StaffNames.Text = "Big thank you to all members of the Bards of Light!\r\n\r\nThanks to the founders of " +
    "this adventure,\r\nto all of the former members of this band\r\nand in honour of tho" +
    "se whom we lost along the way.\r\n";
            // 
            // ChatSettings
            // 
            this.ChatSettings.Controls.Add(this.SettingBringGame);
            this.ChatSettings.Controls.Add(this.UnequipPause);
            this.ChatSettings.Controls.Add(this.SettingBringBmp);
            this.ChatSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChatSettings.Location = new System.Drawing.Point(0, 0);
            this.ChatSettings.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.ChatSettings.Name = "ChatSettings";
            this.ChatSettings.Padding = new System.Windows.Forms.Padding(0);
            this.ChatSettings.Size = new System.Drawing.Size(258, 100);
            this.ChatSettings.TabIndex = 11;
            this.ChatSettings.TabStop = false;
            this.ChatSettings.Text = "Game";
            // 
            // SettingBringGame
            // 
            this.SettingBringGame.AutoSize = true;
            this.SettingBringGame.Checked = true;
            this.SettingBringGame.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SettingBringGame.Location = new System.Drawing.Point(15, 18);
            this.SettingBringGame.Name = "SettingBringGame";
            this.SettingBringGame.Size = new System.Drawing.Size(127, 17);
            this.SettingBringGame.TabIndex = 3;
            this.SettingBringGame.Text = "Bring FFXIV to front";
            this.HelpTip.SetToolTip(this.SettingBringGame, "When playing the song, bring FFXIV to front.");
            this.SettingBringGame.UseVisualStyleBackColor = true;
            this.SettingBringGame.CheckedChanged += new System.EventHandler(this.SettingBringGame_CheckedChanged);
            // 
            // UnequipPause
            // 
            this.UnequipPause.AutoSize = true;
            this.UnequipPause.Checked = true;
            this.UnequipPause.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UnequipPause.Location = new System.Drawing.Point(15, 63);
            this.UnequipPause.Name = "UnequipPause";
            this.UnequipPause.Size = new System.Drawing.Size(149, 17);
            this.UnequipPause.TabIndex = 19;
            this.UnequipPause.Text = "Pause song on unequip";
            this.HelpTip.SetToolTip(this.UnequipPause, "Pause the playing song when unequipping the instrument.\r\nUseful for switching ins" +
        "trument mid-performance.");
            this.UnequipPause.UseVisualStyleBackColor = true;
            this.UnequipPause.CheckedChanged += new System.EventHandler(this.UnequipPause_CheckedChanged);
            // 
            // SettingBringBmp
            // 
            this.SettingBringBmp.AutoSize = true;
            this.SettingBringBmp.Checked = true;
            this.SettingBringBmp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SettingBringBmp.Location = new System.Drawing.Point(15, 40);
            this.SettingBringBmp.Name = "SettingBringBmp";
            this.SettingBringBmp.Size = new System.Drawing.Size(121, 17);
            this.SettingBringBmp.TabIndex = 4;
            this.SettingBringBmp.Text = "Bring BMP to front";
            this.HelpTip.SetToolTip(this.SettingBringBmp, "When Performance is opened in FFXIV, bring BMP to front.");
            this.SettingBringBmp.UseVisualStyleBackColor = true;
            this.SettingBringBmp.CheckedChanged += new System.EventHandler(this.SettingBringBmp_CheckedChanged);
            // 
            // PlaybackSettings
            // 
            this.PlaybackSettings.Controls.Add(this.SettingHoldNotes);
            this.PlaybackSettings.Controls.Add(this.ForcePlayback);
            this.PlaybackSettings.Controls.Add(this.MidiInputLabel);
            this.PlaybackSettings.Controls.Add(this.SettingMidiInput);
            this.PlaybackSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlaybackSettings.Location = new System.Drawing.Point(260, 0);
            this.PlaybackSettings.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.PlaybackSettings.Name = "PlaybackSettings";
            this.PlaybackSettings.Padding = new System.Windows.Forms.Padding(0);
            this.PlaybackSettings.Size = new System.Drawing.Size(258, 100);
            this.PlaybackSettings.TabIndex = 12;
            this.PlaybackSettings.TabStop = false;
            this.PlaybackSettings.Text = "Playback";
            // 
            // SettingHoldNotes
            // 
            this.SettingHoldNotes.AutoSize = true;
            this.SettingHoldNotes.Location = new System.Drawing.Point(13, 20);
            this.SettingHoldNotes.Name = "SettingHoldNotes";
            this.SettingHoldNotes.Size = new System.Drawing.Size(83, 17);
            this.SettingHoldNotes.TabIndex = 1;
            this.SettingHoldNotes.Text = "Hold notes";
            this.HelpTip.SetToolTip(this.SettingHoldNotes, "Enables held notes.");
            this.SettingHoldNotes.UseVisualStyleBackColor = true;
            this.SettingHoldNotes.CheckedChanged += new System.EventHandler(this.SettingHoldNotes_CheckedChanged);
            // 
            // ForcePlayback
            // 
            this.ForcePlayback.AutoSize = true;
            this.ForcePlayback.Location = new System.Drawing.Point(13, 43);
            this.ForcePlayback.Name = "ForcePlayback";
            this.ForcePlayback.Size = new System.Drawing.Size(102, 17);
            this.ForcePlayback.TabIndex = 16;
            this.ForcePlayback.Text = "Force playback";
            this.HelpTip.SetToolTip(this.ForcePlayback, "Ignores the current performance status and plays anyways.\r\n* Recommended to only " +
        "be used when patches break playback.\r\n* Ignored when hooked to non-FFXIV applica" +
        "tions.");
            this.ForcePlayback.UseVisualStyleBackColor = true;
            this.ForcePlayback.CheckedChanged += new System.EventHandler(this.ForceOpenToggle_CheckedChanged);
            // 
            // MidiInputLabel
            // 
            this.MidiInputLabel.Location = new System.Drawing.Point(11, 65);
            this.MidiInputLabel.Name = "MidiInputLabel";
            this.MidiInputLabel.Size = new System.Drawing.Size(104, 21);
            this.MidiInputLabel.TabIndex = 14;
            this.MidiInputLabel.Text = "MIDI Input device:";
            this.MidiInputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SettingMidiInput
            // 
            this.SettingMidiInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingMidiInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SettingMidiInput.FormattingEnabled = true;
            this.SettingMidiInput.Items.AddRange(new object[] {
            "None"});
            this.SettingMidiInput.Location = new System.Drawing.Point(121, 66);
            this.SettingMidiInput.Name = "SettingMidiInput";
            this.SettingMidiInput.Size = new System.Drawing.Size(124, 21);
            this.SettingMidiInput.TabIndex = 13;
            // 
            // MiscSettings
            // 
            this.MiscSettings.Controls.Add(this.label1);
            this.MiscSettings.Controls.Add(this.AutostartMethod);
            this.MiscSettings.Controls.Add(this.SettingChatSave);
            this.MiscSettings.Controls.Add(this.midibardCheckbox);
            this.MiscSettings.Controls.Add(this.verboseToggle);
            this.MiscSettings.Location = new System.Drawing.Point(3, 103);
            this.MiscSettings.Name = "MiscSettings";
            this.MiscSettings.Size = new System.Drawing.Size(253, 146);
            this.MiscSettings.TabIndex = 27;
            this.MiscSettings.TabStop = false;
            this.MiscSettings.Text = "Misc";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Autostart";
            // 
            // AutostartMethod
            // 
            this.AutostartMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutostartMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AutostartMethod.FormattingEnabled = true;
            this.AutostartMethod.Items.AddRange(new object[] {
            "None",
            "Chat",
            "Metronome"});
            this.AutostartMethod.Location = new System.Drawing.Point(73, 15);
            this.AutostartMethod.Name = "AutostartMethod";
            this.AutostartMethod.Size = new System.Drawing.Size(174, 21);
            this.AutostartMethod.TabIndex = 17;
            this.AutostartMethod.SelectedIndexChanged += new System.EventHandler(this.AutostartMethod_SelectedIndexChanged);
            // 
            // SettingChatSave
            // 
            this.SettingChatSave.AutoSize = true;
            this.SettingChatSave.Location = new System.Drawing.Point(15, 65);
            this.SettingChatSave.Name = "SettingChatSave";
            this.SettingChatSave.Size = new System.Drawing.Size(177, 17);
            this.SettingChatSave.TabIndex = 5;
            this.SettingChatSave.Text = "Save chatlogs to \"logs\" folder";
            this.HelpTip.SetToolTip(this.SettingChatSave, "Toggling this on requires a program restart.");
            this.SettingChatSave.UseVisualStyleBackColor = true;
            this.SettingChatSave.CheckedChanged += new System.EventHandler(this.SettingChatSave_CheckedChanged);
            // 
            // midibardCheckbox
            // 
            this.midibardCheckbox.AutoSize = true;
            this.midibardCheckbox.Location = new System.Drawing.Point(15, 42);
            this.midibardCheckbox.Name = "midibardCheckbox";
            this.midibardCheckbox.Size = new System.Drawing.Size(213, 17);
            this.midibardCheckbox.TabIndex = 15;
            this.midibardCheckbox.Text = "MidiBard compat mode (metronome)";
            this.midibardCheckbox.UseVisualStyleBackColor = true;
            this.midibardCheckbox.CheckedChanged += new System.EventHandler(this.midibardCheckbox_CheckedChanged);
            // 
            // verboseToggle
            // 
            this.verboseToggle.AutoSize = true;
            this.verboseToggle.Location = new System.Drawing.Point(15, 88);
            this.verboseToggle.Name = "verboseToggle";
            this.verboseToggle.Size = new System.Drawing.Size(136, 17);
            this.verboseToggle.TabIndex = 20;
            this.verboseToggle.Text = "Enable verbose mode";
            this.HelpTip.SetToolTip(this.verboseToggle, "Print various kinds of information to the log window.");
            this.verboseToggle.UseVisualStyleBackColor = true;
            this.verboseToggle.CheckedChanged += new System.EventHandler(this.verboseToggle_CheckedChanged);
            // 
            // HelpTip
            // 
            this.HelpTip.AutoPopDelay = 5000;
            this.HelpTip.BackColor = System.Drawing.Color.White;
            this.HelpTip.ForeColor = System.Drawing.Color.Black;
            this.HelpTip.InitialDelay = 100;
            this.HelpTip.ReshowDelay = 100;
            this.HelpTip.UseAnimation = false;
            this.HelpTip.UseFading = false;
            // 
            // BmpSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GeneralSettings);
            this.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.Name = "BmpSettings";
            this.Size = new System.Drawing.Size(546, 325);
            this.GeneralSettings.ResumeLayout(false);
            this.SettingsScrollPanel.ResumeLayout(false);
            this.SettingsTable.ResumeLayout(false);
            this.ThanksTable.ResumeLayout(false);
            this.ThanksTable.PerformLayout();
            this.ChatSettings.ResumeLayout(false);
            this.ChatSettings.PerformLayout();
            this.PlaybackSettings.ResumeLayout(false);
            this.PlaybackSettings.PerformLayout();
            this.MiscSettings.ResumeLayout(false);
            this.MiscSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.GroupBox GeneralSettings;
		private System.Windows.Forms.CheckBox SettingHoldNotes;
		private System.Windows.Forms.CheckBox SettingBringGame;
		private System.Windows.Forms.CheckBox SettingBringBmp;
		private System.Windows.Forms.CheckBox SettingChatSave;
		private System.Windows.Forms.Button KeyboardTest;
		private System.Windows.Forms.ToolTip HelpTip;
		private System.Windows.Forms.GroupBox ChatSettings;
		private System.Windows.Forms.Panel SettingsScrollPanel;
		private System.Windows.Forms.GroupBox PlaybackSettings;
		private System.Windows.Forms.Label MidiInputLabel;
		private System.Windows.Forms.ComboBox SettingMidiInput;
		private System.Windows.Forms.Button SignatureFolder;
		private System.Windows.Forms.CheckBox midibardCheckbox;
		private System.Windows.Forms.CheckBox ForcePlayback;
		private System.Windows.Forms.TableLayoutPanel SettingsTable;
		private System.Windows.Forms.CheckBox UnequipPause;
		private System.Windows.Forms.CheckBox verboseToggle;
        private System.Windows.Forms.GroupBox ThanksTable;
        private System.Windows.Forms.TextBox StaffNames;
        private System.Windows.Forms.GroupBox MiscSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox AutostartMethod;
    }
}

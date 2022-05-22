using FFBardMusicPlayer.Controls;
using NLog;
using NLog.Targets;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using static FFBardMusicPlayer.Controls.BmpPlayer;
using System.Security.Principal;
using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Seer;
using BardMusicPlayer.Quotidian.Structs;
using System.Diagnostics;

namespace FFBardMusicPlayer.Forms {
	public partial class BmpMain : Form
	{

		BmpProcessSelect processSelector = new BmpProcessSelect();
		private bool keyboardWarning = false;

		private DialogResult updateResult = DialogResult.No;
		private string updateTitle = string.Empty;
		private string updateText = string.Empty;

		private bool proceedPlaylistMidi = false;

		private System.Timers.Timer _performanceStartDelayTimer = null!;

		public bool DonationStatus {
			set {
				if(value) {
					this.BackColor = Color.PaleGoldenrod;
					BottomTable.BackColor = Color.DarkKhaki;
					Explorer.BackColor = Color.DarkKhaki;
					// Donation button is the "About" button...
				}
			}
		}


		public BmpMain(string title, string messageText)
		{
			InitializeComponent();

			//init the delay timer
			_performanceStartDelayTimer = new System.Timers.Timer();
			_performanceStartDelayTimer.Enabled = false;
            _performanceStartDelayTimer.Elapsed += StartDelayTimer_Elapsed; ;

			this.UpdatePerformance();

			ChatLogAll.AppendRtf(BmpChatParser.FormatRtf(messageText, Color.LightGreen, true));
			this.Text = title;

			// Clear local orchestra
			InfoTabs.TabPages.Remove(localOrchestraTab);

			LocalOrchestra.onMemoryCheck += delegate (Object o, bool status) {
				if (status)
					this.FFXIV.memory.StopThread();
				else
					this.FFXIV.memory.StartThread();
			};

			FFXIV.findProcessRequest += delegate (Object o, EventArgs empty) {
				this.Invoke(t => t.FindProcess());
			};

			FFXIV.findProcessError += delegate (Object o, BmpHook.ProcessError error) {
				this.Invoke(t => t.ErrorProcess(error));
			};

			FFXIV.hotkeys.OnFileLoad += delegate (Object o, EventArgs empty) {
				this.Invoke(t => t.Hotkeys_OnFileLoad(FFXIV.hotkeys));
			};
			FFXIV.hook.OnKeyPressed += Hook_OnKeyPressed;
			FFXIV.memory.OnProcessLost += delegate (object o, EventArgs arg) {
				this.Log("Attached process exited.");
			};

			FFXIV.memory.OnCurrentPlayerLogin += delegate (object o, Game res) {
				string world = string.Empty;
				world = BmpGlobals.CurrentGame.HomeWorld;
				if(string.IsNullOrEmpty(world)) {
					this.Invoke(t =>  this.Log(string.Format("Character [{0}] logged in.", BmpGlobals.CurrentGame.PlayerName)));
				} else {
					this.Invoke(t =>  this.Log(string.Format("Character [{0}] logged in at [{1}].", res.PlayerName, world)));
				}
				this.Invoke(t => t.UpdatePerformance());
			};
			FFXIV.memory.OnCurrentPlayerLogout += delegate (object o, Game res) {
				string format = string.Format("Character [{0}] logged out.", res.PlayerName);
				this.Log(format);
			};

			BmpSeer.Instance.GameStarted += delegate (BardMusicPlayer.Seer.Events.GameStarted e) {
				this.Invoke(t => t.GameStarted(e));
			};

			BmpSeer.Instance.GameStopped += delegate (BardMusicPlayer.Seer.Events.GameStopped e) {
				this.Invoke(t => t.GameStopped(e));
			};


			BmpSeer.Instance.ChatLog += delegate (BardMusicPlayer.Seer.Events.ChatLog e) {
				this.Invoke(t => t.Memory_OnChatReceived(e));
			};

			//BmpSeer.Instance.IsBardChanged += Instance_IsBardChanged;

			BmpSeer.Instance.InstrumentHeldChanged += delegate (BardMusicPlayer.Seer.Events.InstrumentHeldChanged e) {
				this.Invoke(t => t.Instance_InstrumentHeldChanged(e));
			};

			BmpSeer.Instance.PartyMembersChanged += delegate (BardMusicPlayer.Seer.Events.PartyMembersChanged performance) {
				this.Invoke(t => t.LocalOrchestraUpdate(performance));
			};

			BmpSeer.Instance.PlayerNameChanged += delegate (BardMusicPlayer.Seer.Events.PlayerNameChanged performance) {
				this.Invoke(t => t.PlayerNameChanged(performance));
			};

			BmpSeer.Instance.EnsembleStarted += delegate (BardMusicPlayer.Seer.Events.EnsembleStarted e) {
				this.Invoke(t => t.Machina_EnsembleStarted(e));
			};

			Player.OnStatusChange += delegate (object o, PlayerStatus status) {
				this.Invoke(t => t.UpdatePerformance());
			};

			Player.OnSongSkip += OnSongSkip;

			Player.OnMidiProgressChange += OnPlayProgressChange;

			Player.OnMidiStatusChange += OnPlayStatusChange;
			Player.OnMidiStatusEnded += OnPlayStatusEnded;

			Player.OnMidiNote += OnMidiVoice;
			Player.OffMidiNote += OffMidiVoice;
			Player.ProgChangeMidi += ChangeMidiVoice;

			Player.Player.OpenInputDevice(Settings.GetMidiInput().name);

			Settings.OnMidiInputChange += delegate (object o, MidiInput input) {
				Player.Player.CloseInputDevice();
				if(input.id != -1) {
					Player.Player.OpenInputDevice(input.name);
					Log(string.Format("Switched to {0} ({1})", input.name, input.id));
				}
			};
			Settings.OnKeyboardTest += delegate (object o, EventArgs arg) {
				
				for(int i =0; i < Enum.GetNames(typeof(BardMusicPlayer.Quotidian.Enums.NoteKey)).Length; i++)
				{
					if (!FFXIV.IsPerformanceReady())
						return;

					if (BmpGlobals.CurrentGame.NoteKeys[(BardMusicPlayer.Quotidian.Enums.NoteKey)i] is BardMusicPlayer.Quotidian.Enums.Keys keybind)
						FFXIV.hook.SendSyncKeybind(keybind);
					Thread.Sleep(100);
				}
			};

            Settings.OnForcedOpen += delegate (object o, bool open)
            {
                this.Invoke(t => {
                    if (open)
                    {
                        Log(string.Format("Forced playback was enabled. You will not be able to use keybinds, such as spacebar."));
                        WarningLog("Forced playback enabled.");
                    }

                    t.UpdatePerformance();
               });
            };

			Explorer.OnBrowserVisibleChange += delegate (object o, bool visible) {
				MainTable.SuspendLayout();
				MainTable.RowStyles[MainTable.GetRow(ChatPlaylistTable)].Height = visible ? 0 : 100;
				MainTable.RowStyles[MainTable.GetRow(ChatPlaylistTable)].SizeType = visible ? SizeType.Absolute : SizeType.Percent;
				//ChatPlaylistTable.Invoke(t => t.Visible = !visible);

				MainTable.RowStyles[MainTable.GetRow(Explorer)].Height = visible ? 100 : 30;
				MainTable.RowStyles[MainTable.GetRow(Explorer)].SizeType = visible ? SizeType.Percent : SizeType.Absolute;
				MainTable.ResumeLayout(true);
			};
			Explorer.OnBrowserSelect += Browser_OnMidiSelect;
			Explorer.OnRewindClicked += Browser_OnRewindMidi;
			Explorer.OnOpenSongClicked += Browser_OnOpenSong;
			Playlist.OnMidiSelect += Playlist_OnMidiSelect;
			Playlist.OnPlaylistRequestAdd += Playlist_OnPlaylistRequestAdd;
            Playlist.OnPlaylistManualRequestAdd += Playlist_OnPlaylistManualRequestAdd;

			this.ResizeBegin += (s, e) => {
				LocalOrchestra.SuspendLayout();
			};
			this.ResizeEnd += (s, e) => {
				LocalOrchestra.ResumeLayout(true);
			};

			if(BmpPigeonhole.Instance.SaveChatLog) {
				FileTarget target = new NLog.Targets.FileTarget("chatlog") {
					FileName = "logs/ff14log.txt",
					Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss} ${message}",
					ArchiveDateFormat = "${shortdate}",
					ArchiveEvery = FileArchivePeriod.Day,
					ArchiveFileName = "logs/ff14log-${shortdate}.txt",
					Encoding = Encoding.UTF8,
					KeepFileOpen = true,
				};

				var config = new NLog.Config.LoggingConfiguration();
				config.AddRule(LogLevel.Info, LogLevel.Info, target);
				NLog.LogManager.Configuration = config;
			}

			string upath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming).FilePath;
			//Console.WriteLine(string.Format(".config: [{0}]", upath));

			Settings.RefreshMidiInput();

			Log("Bard Music Player initialized.");
		}

        private void StartDelayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
			_performanceStartDelayTimer.Enabled = false;
			Player.Player.Play();
		}

        private void Instance_InstrumentHeldChanged(BardMusicPlayer.Seer.Events.InstrumentHeldChanged seerEvent)
        {
			if (LocalOrchestra.OrchestraEnabled)
			{
				this.LocalOrchestraUpdate();
				return;
			}
			this.UpdatePerformance();
			this.Memory_OnPerformanceReadyChanged(!seerEvent.InstrumentHeld.Equals(Instrument.None));
		}

        public void LogMidi(string format)
        {
            ChatLogAll.AppendRtf(BmpChatParser.FormatRtf("[MIDI] " + format, Color.LightPink));
        }

        public void Log(string format)
        {
            ChatLogAll.AppendRtf(BmpChatParser.FormatRtf("[SYSTEM] " + format));
        }

        public void WarningLog(string format)
        {
            FFXIV.SetErrorStatus("Warning: " + format);
        }

        public void ErrorLog(string format)
        {
            FFXIV.SetErrorStatus("[ERROR] " + format);
        }

        public void FindProcess()
		{
			processSelector.ShowDialog(this);
			if(processSelector.DialogResult == DialogResult.Yes)
			{
				Game game = processSelector.selectedGame;
				if(game != null)
				{
					BmpGlobals.CurrentGame = game;
					FFXIV.SetProcess(game);
					if(processSelector.useLocalOrchestra) {
						InfoTabs.TabPages.Remove(localOrchestraTab);
						InfoTabs.TabPages.Insert(2, localOrchestraTab);
						Player.Status = PlayerStatus.Conducting;
					} else {
						Player.Status = PlayerStatus.Performing;
						InfoTabs.TabPages.Remove(localOrchestraTab);
					}
					LocalOrchestra.OrchestraEnabled = processSelector.useLocalOrchestra;
					if(LocalOrchestra.OrchestraEnabled) {
						LocalOrchestra.PopulateLocalProcesses(processSelector.multiboxProcesses);
						LocalOrchestra.Sequencer = Player.Player;
						InfoTabs.SelectTab(2);
					}
				}
			}
		}

		public void GameStarted(BardMusicPlayer.Seer.Events.GameStarted e)
		{
			if (BmpGlobals.CurrentGame != null)
			{
				if (LocalOrchestra.OrchestraEnabled)
					LocalOrchestra.AddLocalProcesses(e.Game);
			}
			return;
		}

		public void GameStopped(BardMusicPlayer.Seer.Events.GameStopped e)
		{
			if ((BmpGlobals.CurrentGame != null) && (BmpGlobals.CurrentGame.Pid == e.Pid))
				BmpGlobals.CurrentGame = null;

			if (LocalOrchestra.OrchestraEnabled)
				LocalOrchestra.RemoveLocalProcesses(e.Pid);
		}

		private void PlayerNameChanged(BardMusicPlayer.Seer.Events.PlayerNameChanged seerEvent)
		{
			if (BmpGlobals.CurrentGame != null)
			{
				if (LocalOrchestra.OrchestraEnabled)
					LocalOrchestra.UpdateLocalProcesses(seerEvent.Game);
			}
		}

		public void ErrorProcess(BmpHook.ProcessError error) {
			if(error == BmpHook.ProcessError.ProcessFailed) {
				Log("Process hooking failed.");
			}
			else if(error == BmpHook.ProcessError.ProcessNonAccessible) {
				bool admin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
				if(!admin) {
					MessageBox.Show(this, "Process memory cannot be read.\nPlease start BMP in administrator mode.", "Process not accessible");
				} else {
					Log("Process hooking failed due to lack of privilege. Please make sure the game is not running in administrator mode.");
				}
			}
		}

		public bool IsOnScreen(Form form) {
			// Create rectangle
			Rectangle formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);

			// Test
			return Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(formRectangle));
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			this.Location = BmpPigeonhole.Instance.BmpLocation;
			this.Size = BmpPigeonhole.Instance.BmpSize;

			if(!this.IsOnScreen(this)) {
				this.Location = new Point(100, 100);
			}

			if(this.WindowState == FormWindowState.Minimized) {
				this.WindowState = FormWindowState.Maximized;
			}

			if(BmpPigeonhole.Instance.SigIgnore) {
				this.Log("Using local signature cache.");
			}
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			this.FindProcess();

			string ll = BmpPigeonhole.Instance.LastLoaded;
			if(!string.IsNullOrEmpty(ll)) {
				if(Explorer.SelectFile(ll)) {
					Playlist.Select(ll);
					Explorer.EnterFile();
				}
			} else {
				if(Playlist.HasMidi()) {
					Playlist.PlaySelectedMidi();
				}
			}

			if(this.updateResult == DialogResult.Yes) {
				this.Invoke(new Action(() => {
					MessageBox.Show(this, updateText, updateTitle);
				}));
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e) {
			base.OnFormClosing(e);

			if (!this.IsDisposed) {
				BmpPigeonhole.Instance.BmpLocation = this.Location;
				BmpPigeonhole.Instance.BmpSize = this.Size;
			}
		}

		protected override void OnClosing(CancelEventArgs e) {

			FFXIV.ShutdownMemory();

			Player.Player.CloseInputDevice();
			Player.Player.Pause();

			FFXIV.hook.ClearLastPerformanceKeybinds();

			base.OnClosing(e);
		}

		private void Hotkeys_OnFileLoad(FFXIVKeybindDat hotkeys) {
			Player.Keyboard.UpdateNoteKeys(hotkeys);

			if(!hotkeys.ExtendedKeyboardBound && !keyboardWarning) {
				keyboardWarning = true;

				BmpKeybindWarning keybindWarning = new BmpKeybindWarning();
				keybindWarning.ShowDialog(this);

				//Log(string.Format("Your performance keybinds aren't set up correctly, songs will be played incomplete."));
			}
		}

		private void Memory_OnChatReceived(BardMusicPlayer.Seer.Events.ChatLog item)
		{
			string rtf = BmpChatParser.FormatChat(item);
			ChatLogAll.AppendRtf(rtf);

			if (!WantsAutostartPlaying)
				return;

			if (Player.Player.IsPlaying)
				return;
	
			if (BmpPigeonhole.Instance.AutostartMethod == 1)
            {
				if (item.Code == "0039")
				{
					if (item.Line.Contains(@"Anzählen beginnt") ||
						item.Line.Contains("The count-in will now commence.") ||
						item.Line.Contains("orchestre est pr"))
					{
						if (_performanceStartDelayTimer.Enabled)
							return;

						_performanceStartDelayTimer.Interval = 3000;
						_performanceStartDelayTimer.Enabled = true;
                    }
                }
            }
		}

		private void Machina_EnsembleStarted(BardMusicPlayer.Seer.Events.EnsembleStarted seerEvent)
		{
			if (Player.Player.IsPlaying)
				return;

			if (!WantsAutostartPlaying)
				return;

			if (BmpPigeonhole.Instance.AutostartMethod != 2)
				return;

			if (_performanceStartDelayTimer.Enabled)
				return;

			if (BmpPigeonhole.Instance.MidiBardCompatMode)
				_performanceStartDelayTimer.Interval = (2500 + 3405);
			else
				_performanceStartDelayTimer.Interval = 2500;
			_performanceStartDelayTimer.Enabled = true;
		}

		private void Memory_OnPerformanceReadyChanged(bool performance)
		{
			if(performance) {
				if(BmpPigeonhole.Instance.BringBMPtoFront) {
					this.BringFront();
				}
			} else {
				if(!BmpPigeonhole.Instance.ForcePlayback) {
					// If playing alone, stop playing
					if(BmpPigeonhole.Instance.UnequipPause) {
						if(Player.Status == PlayerStatus.Performing) {
							if(Player.Player.IsPlaying) {
								Player.Player.Pause();
								FFXIV.hook.ClearLastPerformanceKeybinds();
							}
						}
					}
				}
			}
			this.UpdatePerformance();
		}

		private void LocalOrchestraUpdate(BardMusicPlayer.Seer.Events.PartyMembersChanged seerEvent)
		{
			if (LocalOrchestra.OrchestraEnabled)
				this.LocalOrchestraUpdate();
		}

		private void LocalOrchestraUpdate() {
			LocalOrchestra.UpdateMemory();
		}

		private void UpdatePerformance() {
			if(Player.Status == PlayerStatus.Conducting) {
				Player.Interactable = true;
				Player.Keyboard.OverrideText = "Conducting in progress.";
				Player.Keyboard.Enabled = false;
			} else {
				Player.Interactable = FFXIV.IsPerformanceReady();
				Player.Keyboard.OverrideText = FFXIV.IsPerformanceReady() ? string.Empty : "Open Bard Performance mode to play.";
				Player.Keyboard.Enabled = true;
			}
		}

		private void BringFront() {
			this.TopMost = true;
			this.Activate();
			this.TopMost = false;
		}

		private void Hook_OnKeyPressed(Object o, Keys key) {
			if(BmpPigeonhole.Instance.ForcePlayback)
				return;

			if(FFXIV.IsPerformanceReady() && !BmpGlobals.CurrentGame.ChatStatus)
			{
				if(key == Keys.F10) {
					foreach(FFXIVKeybindDat.Keybind keybind in FFXIV.hotkeys.GetPerformanceKeybinds()) {
						FFXIV.hook.SendAsyncKey((BardMusicPlayer.Quotidian.Enums.Keys)keybind.GetKey());
						System.Threading.Thread.Sleep(100);
					}
				}
				if (key == Keys.Space)
				{
					if (Player.Player.IsPlaying)
						Player.Player.Pause();
					else
						Player.Player.Play();
				}
				if(key == Keys.Right)
				{
					if(Player.Player.IsPlaying)
						Player.Player.Seek(1000);
				}
				if(key == Keys.Left)
				{
					if(Player.Player.IsPlaying)
						Player.Player.Seek(-1000);
				}
				if(key == Keys.Up)
				{
					if(Player.Player.IsPlaying)
						Player.Player.Seek(10000);
				}
				if(key == Keys.Down)
				{
					if(Player.Player.IsPlaying)
						Player.Player.Seek(-10000);
				}
			}
		}

		private bool WantsAutostartPlaying
		{
			get
			{
				if (BmpPigeonhole.Instance.AutostartMethod != 0)
					return true;
				return false;
			}
		}

		private void DonationButton_Click(object sender, EventArgs e) {
			BmpAbout about = new BmpAbout();
			about.ShowDialog(this);
		}
	}
}

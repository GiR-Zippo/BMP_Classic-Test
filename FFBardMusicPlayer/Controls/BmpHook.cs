using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using BardMusicPlayer.Quotidian.Structs;
using Timer = System.Timers.Timer;
using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Seer;

namespace FFBardMusicPlayer.Controls {
	public partial class BmpHook : UserControl {

		// Keep each BMP+FFXIV instance hidden in proc list
		private Mutex procMutex = null;

		public FFXIVHook hook = new FFXIVHook();
		public FFXIVMemory memory = new FFXIVMemory();

		public FFXIVKeybindDat hotkeys = new FFXIVKeybindDat();
		public FFXIVHotbarDat hotbar = new FFXIVHotbarDat();
		public FFXIVAddonDat addon = new FFXIVAddonDat();

		public event EventHandler<bool> forceModeChanged;
		public event EventHandler perfSettingsChanged;
		public event EventHandler findProcessRequest;

        private Timer errorMessageTimer = null;

		public enum ProcessError {
			ProcessFailed,
			ProcessNonAccessible,
		}
		public event EventHandler<ProcessError> findProcessError;

		private string CurrentCharId {
			get {
				if(CharIdSelector.SelectedValue != null)
					return CharIdSelector.SelectedText;
				return string.Empty;
			}
			set {
				CharIdSelector.Invoke(t => t.SelectedIndex = CharIdSelector.FindStringExact(value));
			}
		}

		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public BmpHook() {
			InitializeComponent();

			UpdateCharIds();

			memory.OnProcessLost += Memory_OnProcessLost;
            BmpSeer.Instance.ConfigIdChanged += Memory_OnCharacterIdChanged;
		}

        private void Memory_OnProcessLost(object sender, EventArgs e) {
			if(procMutex != null) {
				try {
					this.Invoke(new Action(() => procMutex.ReleaseMutex()));
				} catch(Exception ex) {
					Log(ex.Message);
				}
			}
			memory.UnsetProcess();
			this.ShutdownMemory();
			hook.Unhook();
			this.Invoke(t => t.SetHookStatus());
		}

		private void Log(string text) {
			string str = string.Format("[BMP] {0}", text);
			Console.WriteLine(str);
			logger.Debug(str);
		}

		private void Memory_OnCharacterIdChanged(BardMusicPlayer.Seer.Events.ConfigIdChanged seerEvent) {
			hook.ClearLastPerformanceKeybinds();
			hotkeys.LoadKeybindDat(seerEvent.ConfigId);
			hotbar.LoadHotbarDat(seerEvent.ConfigId);
			addon.LoadAddonDat(seerEvent.ConfigId);

			BmpPigeonhole.Instance.LastCharId = seerEvent.ConfigId;
			CurrentCharId = seerEvent.ConfigId;
		}

		public bool IsPerformanceReady() {
			// Force keyboard up
			if(BmpPigeonhole.Instance.ForcePlayback) {
				return true;
			}
			if (BmpGlobals.CurrentGame != null)
			{
				if (!BmpGlobals.CurrentGame.InstrumentHeld.Equals(Instrument.None))
					return true;
			}
			return false;
		}

		public void SetHookStatus(string status = null) {
			if(string.IsNullOrEmpty(status)) {
				status = "Hook process";
			}
			HookButton.Text = status;
		}

        public void SetErrorStatus(string status)
        {
            // remove it, since we'll be updating the text again
            if (errorMessageTimer != null)
            {
                errorMessageTimer.Stop();
                errorMessageTimer.Dispose();
                errorMessageTimer = null;
            }

            // set the label text
            HookGlobalMessageLabel.Text = status;

            // dispatch hiding the system error
            errorMessageTimer = new Timer
            {
                Interval = 10 * 1000, // 10 seconds
                Enabled = true
            };
            errorMessageTimer.Elapsed += delegate (object o, System.Timers.ElapsedEventArgs e)
            {
                this.Invoke(t => { HookGlobalMessageLabel.Text = ""; });
                errorMessageTimer.Stop();
                errorMessageTimer.Dispose();
                errorMessageTimer = null;
            };
        }

		public void SetProcess(Game game) {
			try {
				var a = game.Process.HasExited;
			} catch (Win32Exception ex) {
				Log(string.Format(ex.Message));
				findProcessError.Invoke(this, ProcessError.ProcessNonAccessible);
				return;
			}
			if(hook.Hook(game.Process)) {
				Log(string.Format("Process hooking succeeded."));

				string str = string.Format("Hooked: {0} ({1})", game.Process.ProcessName, game.Process.Id);
				this.Invoke(t => t.SetHookStatus(str));

				procMutex = new Mutex(true, string.Format("bard-music-player-{0}", game.Process.Id));
				if(procMutex.WaitOne(TimeSpan.Zero, true)) {
					SetupMemory(game.Process);
				}
			} else {
				Log(string.Format("Process hooking failed."));
				SetHookStatus("F: Hook process...");
				findProcessError.Invoke(this, ProcessError.ProcessFailed);
			}
		}

		public void SetupMemory(Process proc) {
			if(memory.IsAttached()) {
				memory.UnsetProcess();
			}
			if(proc.ProcessName == "ffxiv_dx11") {
				Log(string.Format("FFXIV memory parsing..."));

				// memory setprocess
				memory.SetProcess(proc);
				memory.StartThread();
			}
		}

		public void ShutdownMemory() {
			memory.StopThread();
			while(memory.IsThreadAlive()) {
				// ...
			}
		}

		public void UpdateCharIds() {
			CharIdSelector.Items.Clear();
			foreach(string id in FFXIVDatFile.GetIdList()) {
				ToolStripMenuItem item = new ToolStripMenuItem(id);
				if(id.Equals(BmpPigeonhole.Instance.LastCharId)) {
					item.Checked = true;
				}
				CharIdSelector.Items.Add(item);
			}
		}

		private void HookButton_Click(object sender, EventArgs e) {
			if(memory.IsAttached()) {
				memory.UnsetProcess();
				this.ShutdownMemory();
				hook.Unhook();
				this.SetHookStatus();
			} else {
				findProcessRequest?.Invoke(this, EventArgs.Empty);
			}
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
		}

		private void CharIdSelector_SelectedIndexChanged(object sender, EventArgs e) {
			string id = (sender as ComboBox).Text as string;

			Log(string.Format("Forced FFXIV character ID: [{0}].", id));

			hook.ClearLastPerformanceKeybinds();
			hotkeys.LoadKeybindDat(id);
			hotbar.LoadHotbarDat(id);

			perfSettingsChanged?.Invoke(this, EventArgs.Empty);

		}

		private void ForceModeToggle_CheckedChanged(object sender, EventArgs e) {

			bool value = (sender as CheckBox).Checked;
			BmpPigeonhole.Instance.ForcePlayback = value;
			forceModeChanged?.Invoke(sender, value);
		}
	}
}

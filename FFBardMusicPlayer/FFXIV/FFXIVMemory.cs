using System;
using System.Threading;
using System.Diagnostics;
using BardMusicPlayer.Seer;

namespace FFBardMusicPlayer {
	public class FFXIVMemory
	{
		public bool performanceReady;

		private string characterId;
		public string CharacterID {
			get {
				return characterId;
			}
			set {
				characterId = value;
				OnCharacterIdChanged?.Invoke(this, characterId);
			}
		}

		public event EventHandler OnProcessSeek;
		public event EventHandler OnProcessLost;
		public event EventHandler<Game> OnCurrentPlayerLogin;
		public event EventHandler<Game> OnCurrentPlayerLogout;

		public event EventHandler<string> OnCharacterIdChanged;

		bool hasLost = true;

		Process ffxivProcess;
		Thread thread;
		bool hasScanned;
		bool isLoggedIn;

		public FFXIVMemory() {
			Reset();
		}

		private void Reset()
		{
			hasScanned = false;
			isLoggedIn = false;

			BmpSeer.Instance.IsLoggedInChanged += e => Instance_IsLoggedInChanged(e);
		}

        private void Instance_IsLoggedInChanged(BardMusicPlayer.Seer.Events.IsLoggedInChanged seerEvent)
        {
			if (BmpGlobals.CurrentGame != null)
			{
				if (!BmpGlobals.CurrentGame.IsLoggedIn && isLoggedIn)
				{
					OnCurrentPlayerLogout?.Invoke(this, BmpGlobals.CurrentGame);
					isLoggedIn = false;
				}
			}
		}

		public void SetProcess(Process process) {
			if(process == null || process.HasExited) {
				return;
			}
			ffxivProcess = process;
			Reset();
		}

		public void UnsetProcess() {
			ffxivProcess = null;
		}

		public void StartThread() {
            // run the refresh through once first, so other things aren't waiting
            // for valid information on initialization
            // this is a hack around threading
            Refresh();
			if(thread == null) {
				ThreadStart memoryThread = new ThreadStart(FFXIVMemory_Main);
				thread = new Thread(memoryThread);
				thread.Start();
			}
		}

		public void StopThread() {
			if(thread != null) {
				thread.Interrupt();
				thread = null;
			}
		}

		public bool IsThreadAlive() {
			if(thread == null) {
				return false;
			}
			return thread.IsAlive;
		}

		public void FFXIVMemory_Main() {
			Console.WriteLine("Memory main loop");
			bool alive = true;
			try {
				while(alive) {
					if(!Refresh()) {
						// Restart?
					}
					Thread.Sleep(100);
				}
			} catch(ThreadInterruptedException) {
				alive = false;
			}
			Console.WriteLine("Reached end");
		}

		public bool Refresh() {

			if(ffxivProcess != null) {
				ffxivProcess.Refresh();
				if(ffxivProcess.HasExited) {
					OnProcessLost?.Invoke(this, EventArgs.Empty);
					ffxivProcess = null;
					hasLost = true;
					Reset();

					Console.WriteLine("Exited game");
				}
				if(IsScanning() && !hasScanned) {
					Console.WriteLine("Scanning...");
					/*while(IsScanning()) {
						Thread.Sleep(100);
					}*/
					Console.WriteLine("Finished scanning");
					//OnProcessReady?.Invoke(this, ffxivProcess);
					hasScanned = true;
				}
			}
			if((ffxivProcess == null) && hasLost) {
				OnProcessSeek?.Invoke(this, EventArgs.Empty);
				hasLost = false;
				return false;
			}

			if(BmpGlobals.CurrentGame != null)
			{
				if ((BmpGlobals.CurrentGame.ConfigId.Length > 0) &&
					(!BmpGlobals.CurrentGame.PlayerName.Equals("Unknown")) &&
					(!BmpGlobals.CurrentGame.HomeWorld.Equals("Unknown")) &&
					(BmpGlobals.CurrentGame.IsLoggedIn)&&
					!isLoggedIn)
				{
					OnCurrentPlayerLogin?.Invoke(this, BmpGlobals.CurrentGame);
					isLoggedIn = true;
				}

				string id = BmpGlobals.CurrentGame.ConfigId;
				if(!string.IsNullOrEmpty(id)) {
					if(string.IsNullOrEmpty(CharacterID) ||
						(!string.IsNullOrEmpty(CharacterID) && !CharacterID.Equals(id))) {
						CharacterID = id;
					}
				}
			}
			return true;
		}

		public bool IsScanning() {
			if (BmpGlobals.CurrentGame != null)
				if (BmpGlobals.CurrentGame.ConfigId.Length > 0)
					return false;

			return true;
		}
		public bool IsAttached() {
			return ffxivProcess != null;
		}
		public bool IsReady() {
			return (IsAttached() && !IsScanning());
		}
	}
}

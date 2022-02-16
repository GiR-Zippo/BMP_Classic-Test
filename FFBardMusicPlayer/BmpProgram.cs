using FFBardMusicPlayer.Forms;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Seer;

namespace FFBardMusicPlayer {

	 public class Program {

		public static string urlBase = "https://bardmusicplayer.com/";
        public static string appBase = Application.StartupPath;

		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[STAThread]
        static void Main(string[] args)
        {
            string titleVersion = "BardMusicPlayer 1.5.3 - BoL Version";
            string messageText = "      This is an unsupported version of BardMusicPlayer";

			Application.EnableVisualStyles();

			CultureInfo nonInvariantCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentCulture = nonInvariantCulture;

			if(GetConsoleWindow() != IntPtr.Zero) {
				Console.OutputEncoding = System.Text.Encoding.UTF8;
			}

			string DataPath = @"data\";
			BmpPigeonhole.Initialize(DataPath + @"\Configuration.json");
			BmpSeer.Instance.SetupFirewall("BardMusicPlayer");
			BmpSeer.Instance.Start();

			BmpMain app = new BmpMain(titleVersion, messageText);
			Application.Run(app);

			BmpSeer.Instance.Stop();
			BmpSeer.Instance.DestroyFirewall("BardMusicPlayer");
		}
	}
}

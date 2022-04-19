using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Sanford.Multimedia.Midi;
using BardMusicPlayer.Pigeonhole;


namespace FFBardMusicPlayer.Controls {
	public partial class BmpSettings : UserControl {

		#region API Stuff

		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, ref Point lParam);

		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();

		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		public static extern int GetScrollPos(IntPtr hWnd, int nBar);

		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		private static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		public static extern int RegisterWindowMessage(string message);

		private const int WM_USER = 0x400;
		private const int SB_HORZ = 0x0;
		private const int SB_VERT = 0x1;
		private const int EM_SETSCROLLPOS = WM_USER + 222;
		private const int EM_GETSCROLLPOS = WM_USER + 221;
		private const int WM_NCLBUTTONDOWN = 0xA1;
		private const int HT_CAPTION = 0x2;

		public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");

		#endregion

		public EventHandler<bool> OnForcedOpen;
		public EventHandler OnKeyboardTest;
		public EventHandler<int> OnGuitarKeybind;

		public EventHandler<MidiInput> OnMidiInputChange;
		List<MidiInput> midiInputs = new List<MidiInput>();

		public BmpSettings() {
			InitializeComponent();

			KeyboardTest.Click += delegate (object o, EventArgs e) {
				OnKeyboardTest?.Invoke(o, e);
			};

			SignatureFolder.Click += delegate (object o, EventArgs e) {
				//Process.Start(Sharlayan.Reader.JsonPath);
			};

			RefreshMidiInput();

			string midiInput = BmpPigeonhole.Instance.MidiInputDev;
			SetMidiInput(midiInput);
			SettingMidiInput.SelectedValueChanged += SettingMidiInput_SelectedValueChanged;

            // initialize UI element values here
            SettingBringGame.Checked = BmpPigeonhole.Instance.BringGametoFront;
			SettingBringBmp.Checked = BmpPigeonhole.Instance.BringBMPtoFront;
			SettingChatSave.Checked = BmpPigeonhole.Instance.SaveChatLog;
			ForcePlayback.Checked = BmpPigeonhole.Instance.ForcePlayback;
			UnequipPause.Checked = BmpPigeonhole.Instance.UnequipPause;
			verboseToggle.Checked = BmpPigeonhole.Instance.Verbose;
			sigCheckbox.Checked = BmpPigeonhole.Instance.SigIgnore;
			SettingHoldNotes.Checked = BmpPigeonhole.Instance.HoldNotes;
			AutostartMethod.SelectedIndex = BmpPigeonhole.Instance.AutostartMethod;
		}

		private void SettingMidiInput_SelectedValueChanged(object sender, EventArgs e) {
			if(GetMidiInput() is MidiInput input) {
				BmpPigeonhole.Instance.MidiInputDev = input.name;
				SetMidiInput(input.name);
			}
		}

		private void ForceOpenToggle_CheckedChanged(object sender, EventArgs e) {
			bool check = (sender as CheckBox).Checked;
			BmpPigeonhole.Instance.ForcePlayback = check;
			OnForcedOpen?.Invoke(this, check);
		}

		public MidiInput SetMidiInput(string device) {
			MidiInput input = midiInputs[0];
			foreach(MidiInput inp in midiInputs) {
				if(inp.name == device) {
					Console.WriteLine("Found input: " + inp.name);
					input = inp;
					break;
				}
			}
			if(input != null) {
				BmpPigeonhole.Instance.MidiInputDev = input.name;
				SettingMidiInput.SelectedItem = input;
				OnMidiInputChange?.Invoke(this, input);
				return input;
			}
			return null;
		}

		public MidiInput GetMidiInput() {
			if(SettingMidiInput.SelectedValue is MidiInput input) {
				return input;
			}
			return null;
		}
		public void RefreshMidiInput() {

			// Refresh list of Midi input devices
			midiInputs.Clear();
			midiInputs.Add(new MidiInput("None", -1));
			for(int i = 0; i < InputDevice.DeviceCount; i++) {
				MidiInCaps cap = InputDevice.GetDeviceCapabilities(i);
				midiInputs.Add(new MidiInput(cap.name, i));
			}
			SettingMidiInput.DataSource = midiInputs;
		}

		// Links

		private void SiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(Program.urlBase);
		}

		private void DiscordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(Program.urlBase + "discord/");
		}

		private void AboutLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			BmpAbout about = new BmpAbout();
			about.ShowDialog(this);
		}

        private void SettingHoldNotes_CheckedChanged(object sender, EventArgs e)
        {
			BmpPigeonhole.Instance.HoldNotes = SettingHoldNotes.Checked;
        }

        private void SettingBringGame_CheckedChanged(object sender, EventArgs e)
        {
			BmpPigeonhole.Instance.BringGametoFront = SettingBringGame.Checked;
        }

        private void SettingBringBmp_CheckedChanged(object sender, EventArgs e)
        {
            BmpPigeonhole.Instance.BringBMPtoFront = SettingBringBmp.Checked;
        }

        private void SettingChatSave_CheckedChanged(object sender, EventArgs e)
        {
			BmpPigeonhole.Instance.SaveChatLog = SettingChatSave.Checked;
        }

        private void sigCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            BmpPigeonhole.Instance.SigIgnore = sigCheckbox.Checked;
        }

        private void UnequipPause_CheckedChanged(object sender, EventArgs e)
        {
			BmpPigeonhole.Instance.UnequipPause = UnequipPause.Checked;
        }

        private void verboseToggle_CheckedChanged(object sender, EventArgs e)
        {
			BmpPigeonhole.Instance.Verbose = verboseToggle.Checked;
		}

		private void AutostartMethod_SelectedIndexChanged(object sender, EventArgs e)
		{
			BmpPigeonhole.Instance.AutostartMethod = AutostartMethod.SelectedIndex;
		}
    }

    public class MidiInput {
		public string name = string.Empty;
		public int id = 0;
		public MidiInput(string n, int i) {
			name = n;
			id = i;
		}
		public override string ToString() {
			return name;
		}
	}
}

using BardMusicPlayer.Pigeonhole;
using System;
using static FFBardMusicPlayer.Controls.BmpPlayer;

namespace FFBardMusicPlayer.Forms
{
	/// <summary>
	/// BmpMain playback routines
	/// </summary>
	public partial class BmpMain
	{
		// OnMidiVoice + OffMidiVoice is called with correct octave shift
		private void OnMidiVoice(Object o, NoteEvent onNote)
		{
			Statistics.AddNoteCount();
			if (BmpPigeonhole.Instance.Verbose)
			{
				FFXIVKeybindDat.Keybind thiskeybind = FFXIV.hotkeys.GetKeybindFromNoteByte(onNote.note);
				if (thiskeybind == null)
				{
					string ns = FFXIVKeybindDat.RawNoteByteToPianoKey(onNote.note);
					if (!string.IsNullOrEmpty(ns))
					{
						string str = string.Format("Note {0} is out of range, it will not be played.", ns);
						this.LogMidi(str);
					}
				}
			}

			if (LocalOrchestra.OrchestraEnabled)
				return;

			if (Player.Status == PlayerStatus.Conducting)
				return;

			if (!FFXIV.IsPerformanceReady())
				return;

			if (onNote.track != null)
			{
				// If from midi file
				if (onNote.track != Player.Player.LoadedTrack)
					return;
			}
			if (BmpGlobals.CurrentGame.ChatStatus)
				return;

			if (FFXIV.hotkeys.GetKeybindFromNoteByte(onNote.note) is FFXIVKeybindDat.Keybind keybind)
			{
				if (BmpPigeonhole.Instance.HoldNotes)
					FFXIV.hook.SendKeybindDown(keybind);
				else
					FFXIV.hook.SendAsyncKeybind(keybind);
			}
		}

		private void OffMidiVoice(Object o, NoteEvent offNote)
		{
			if (LocalOrchestra.OrchestraEnabled)
				return;

			if (Player.Status == PlayerStatus.Conducting)
				return;

			if (!FFXIV.IsPerformanceReady())
				return;

			if (offNote.track != null)
				if (offNote.track != Player.Player.LoadedTrack)
					return;

			if (BmpGlobals.CurrentGame.ChatStatus)
				return;

			if (BmpPigeonhole.Instance.HoldNotes)
			{
				if (FFXIV.hotkeys.GetKeybindFromNoteByte(offNote.note) is FFXIVKeybindDat.Keybind keybind)
				{
					FFXIV.hook.SendKeybindUp(keybind);
				}
			}
		}
		private void ChangeMidiVoice(Object o, ProgChangeEvent progdata)
		{
			if (LocalOrchestra.OrchestraEnabled)
				return;

			if (Player.Status == PlayerStatus.Conducting)
				return;

			if (!FFXIV.IsPerformanceReady())
				return;

			if (progdata.track != null)
			{
				if (progdata.track != Player.Player.LoadedTrack)
				{
					return;
				}
			}

			if (!BmpGlobals.CurrentGame.ChatStatus)
			{
				if (FFXIV.hotkeys.GetKeybindFromVoiceByte(progdata.voice) is FFXIVKeybindDat.Keybind keybind)
					FFXIV.hook.SendSyncKeybind(keybind);
			}
		}
	}
}

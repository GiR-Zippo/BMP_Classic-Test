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

			if (onNote.note < 0 || onNote.note > 36)
				return;

			if (BmpGlobals.CurrentGame.NoteKeys[(BardMusicPlayer.Quotidian.Enums.NoteKey)onNote.note] is BardMusicPlayer.Quotidian.Enums.Keys keybind)
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

			if (offNote.note < 0 || offNote.note > 36)
				return;

			if (BmpPigeonhole.Instance.HoldNotes)
			{
				if (BmpGlobals.CurrentGame.NoteKeys[(BardMusicPlayer.Quotidian.Enums.NoteKey)offNote.note] is BardMusicPlayer.Quotidian.Enums.Keys keybind)
					FFXIV.hook.SendKeybindUp(keybind);
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
				int tone = -1;
				switch (progdata.voice)
				{
					case 29: // overdriven guitar
						tone = 0;
						break;
					case 27: // clean guitar
						tone = 1;
						break;
					case 28: // muted guitar
						tone = 2;
						break;
					case 30: // power chords
						tone = 3;
						break;
					case 31: // special guitar
						tone = 4;
						break;
				}

				if (tone > -1 && tone < 5 && BmpGlobals.CurrentGame.InstrumentToneMenuKeys[(BardMusicPlayer.Quotidian.Enums.InstrumentToneMenuKey)tone] is BardMusicPlayer.Quotidian.Enums.Keys keybind)
					FFXIV.hook.SendSyncKey(keybind);
			}
		}
	}
}

using FFBardMusicCommon;
using FFBardMusicPlayer.Controls;
using System;

namespace FFBardMusicPlayer.Forms
{
	/// <summary>
	/// BmpMain playlist routines
	/// </summary>
	public partial class BmpMain
	{
		/// <summary>
		/// Adds a selected song to the playlist
		/// </summary>
		private void Playlist_OnPlaylistRequestAdd(object o, EventArgs arg)
		{
			// Add from Bmp object
			string filename = Player.Player.LoadedFilename;
			if (!string.IsNullOrEmpty(filename))
			{
				int track = Player.Player.CurrentTrack;

				Playlist.AddPlaylistEntry(filename, track);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void Playlist_OnPlaylistManualRequestAdd(object o, BmpPlaylist.BmpPlaylistRequestAddEvent args)
		{
			if (!string.IsNullOrEmpty(args.filePath))
			{
				// ensure the midi is in our directory before we accept it and add it to the playlist
				if (Explorer.SelectFile(args.filePath))
				{
					Playlist.AddPlaylistEntry(args.filePath, args.track, args.dropIndex);
				}
			}
		}

		/// <summary>
		/// selects a song from the playlist
		/// </summary>
		private void Playlist_OnMidiSelect(object o, BmpMidiEntry entry)
		{
			if (Explorer.SelectFile(entry.FilePath.FilePath))
			{
				Explorer.Invoke(t => t.SelectTrack(entry.Track.Track));
				Explorer.EnterFile();
			}
			Playlist.Select(entry.FilePath.FilePath);
			if (proceedPlaylistMidi && Playlist.AutoPlay)
			{
				Player.Player.Play();
				proceedPlaylistMidi = false;
			}
		}

		/// <summary>
		/// plays the next song from the playlist or stop
		/// </summary>
		private void NextSong()
		{
			if (Playlist.AdvanceNext(out string filename, out int track))
				Playlist.PlaySelectedMidi();
			else
			{
				// If failed playlist when you wanted to, just stop
				if (proceedPlaylistMidi)
					Player.Player.Stop();
			}
		}
	}
}

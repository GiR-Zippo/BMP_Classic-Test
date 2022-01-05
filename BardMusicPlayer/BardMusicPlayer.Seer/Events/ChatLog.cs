/*
 * Copyright(c) 2021 MoogleTroupe
 * Licensed under the GPL v3 license. See https://github.com/BardMusicPlayer/BardMusicPlayer/blob/develop/LICENSE for full license information.
 */

using System;

namespace BardMusicPlayer.Seer.Events
{
    public sealed class ChatLog : SeerEvent
    {
        internal ChatLog(EventSource readerBackendType, Reader.Backend.Sharlayan.Core.ChatLogItem item) : base(readerBackendType, 0, false)
        {
            EventType = GetType();
            ChatTimeStamp = item.TimeStamp;
            Code = item.Code;
            Line = item.Line;
        }


        public DateTime ChatTimeStamp{ get; }
        public string Code { get; }
        public string Line { get; }

        public override bool IsValid() => true;
    }
}
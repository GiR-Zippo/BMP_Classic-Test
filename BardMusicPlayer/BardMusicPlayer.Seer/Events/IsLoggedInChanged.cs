﻿/*
 * Copyright(c) 2021 MoogleTroupe
 * Licensed under the GPL v3 license. See https://github.com/BardMusicPlayer/BardMusicPlayer/blob/develop/LICENSE for full license information.
 */

using BardMusicPlayer.Seer.Reader.Backend.Sharlayan.Core.Enums;
using System;

namespace BardMusicPlayer.Seer.Events
{
    public sealed class IsLoggedInChanged : SeerEvent
    {
        internal IsLoggedInChanged(EventSource readerBackendType, bool status) : base(readerBackendType)
        {
            EventType = GetType();
            IsLoggedIn = status;
        }

        public bool IsLoggedIn { get; }

        public override bool IsValid() => true;
    }
}

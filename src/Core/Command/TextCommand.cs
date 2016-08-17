/*
 *  This file is part of uEssentials project.
 *      https://uessentials.github.io/
 *
 *  Copyright (C) 2015-2016  Leonardosc
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using System.Collections.Generic;
using Essentials.Api.Command;
using Essentials.Api.Command.Source;
using Essentials.Common;
using Essentials.Common.Util;
using Essentials.Configuration;
using UnityEngine;

namespace Essentials.Core.Command {

    internal class TextCommand : ICommand {

        public string Name { get; internal set; }
        public string Usage { get; set; }
        public string[] Aliases { get; set; }
        public string Description { get; set; }
        public string Permission { get; set; }
        public AllowedSource AllowedSource { get; set; }

        private readonly HashSet<TextEntry> _texts; 

        public TextCommand(TextCommands.TextCommandData data) {
            _texts = new HashSet<TextEntry>();
            Name = data.Name;
            Usage = string.Empty;
            Aliases = new string[0];
            Description = string.Empty;
            AllowedSource = AllowedSource.BOTH;
            Permission = $"essentials.textcommand.{Name}";

            data.Text.ForEach(txt => {
                var color = ColorUtil.GetColorFromString(ref txt);
                _texts.Add(new TextEntry { Text = txt, Color = color });
            });
        }

        public CommandResult OnExecute(ICommandSource src, ICommandArgs args) {
            foreach (var entry in _texts) {
                src.SendMessage(entry.Text, entry.Color);
            }
            return CommandResult.Success();
        }

        struct TextEntry {
            public string Text;
            public Color Color;
        }
    }

}
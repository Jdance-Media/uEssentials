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

using System.Linq;
using Essentials.Api.Command;
using Essentials.Api.Command.Source;
using Essentials.I18n;
using static Essentials.Commands.CommandPoll;

namespace Essentials.Commands
{
    [CommandInfo(
        Name = "vote",
        Description = "Vote in an pool",
        Usage = "[yes/no] [poll_name]",
        AllowedSource = AllowedSource.PLAYER
    )]
    public class CommandVote : EssCommand
    {
        public override CommandResult OnExecute( ICommandSource source, ICommandArgs parameters )
        {
            if ( (parameters.Length != 2 && Polls.Count != 1 ) || parameters.Length < 1 )
            {
                return CommandResult.ShowUsage();
            }

            switch ( parameters[0].ToString().ToLower() )
            {
                case "yes":
                case "y":
                    var pollName = parameters.Length == 1 ? Polls.Keys.First() : parameters[1].ToString();

                    if ( !PollExists( pollName, source ) )
                    {
                        return CommandResult.Empty();
                    }
                        
                    lock ( Polls )
                    {
                        var poll = Polls[pollName];
                            
                        if ( poll.Voted.Contains( source.DisplayName ) )
                        {
                            return CommandResult.Lang( EssLang.POLL_ALREADY_VOTED );
                        }
                            
                        poll.YesVotes += 1;
                        poll.Voted.Add( source.DisplayName );
                            
                        Polls[pollName] = poll;
                            
                        EssLang.POLL_VOTED_YES.SendTo( source, pollName );
                    }
                    break;

                case "no":
                case "n":
                    pollName = parameters[1].ToString();

                    if ( !PollExists( pollName, source ) )
                    {
                        return CommandResult.Empty();
                    }
                        
                    lock ( Polls )
                    {
                        var poll = Polls[pollName];
                            
                        if ( poll.Voted.Contains( source.DisplayName ) )
                        {
                            return CommandResult.Lang( EssLang.POLL_ALREADY_VOTED );
                        }
                            
                        poll.NoVotes += 1;
                        poll.Voted.Add( source.DisplayName );
                            
                        Polls[pollName] = poll;
                            
                        EssLang.POLL_VOTED_YES.SendTo( source, pollName );
                    }
                    break;

                default:
                    return CommandResult.ShowUsage();
            }

            return CommandResult.Success();
        }
    }
}

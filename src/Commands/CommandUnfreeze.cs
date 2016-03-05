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
using Essentials.Api.Unturned;
using Essentials.Core.Components.Player;
using Essentials.I18n;

namespace Essentials.Commands
{
    [CommandInfo(
        Name = "unfreeze",
        Usage = "[player/*]",
        Description = "Unfreeze an player/* = all"
    )]
    public class CommandUnfreeze : EssCommand
    {
        public override CommandResult OnExecute ( ICommandSource source, ICommandArgs parameters )
        {
            if ( parameters.Length == 0 )
            {
                return CommandResult.ShowUsage();
            }

            if ( parameters[0].Is( "*" ) )
            {
                foreach ( var player in UServer.Players.Where( player => player.HasComponent<FrozenPlayer>() ) )
                {
                    player.RemoveComponent<FrozenPlayer>();

                    EssLang.UNFROZEN_PLAYER.SendTo( player, source.DisplayName );
                }

                EssLang.UNFROZEN_ALL.SendTo( source );
            }
            else
            {
                var target = UPlayer.From( parameters[0].ToString() );

                if ( target == null )
                {
                    return CommandResult.Lang( EssLang.PLAYER_NOT_FOUND, parameters[0] );
                }

                if ( !target.HasComponent<FrozenPlayer>() )
                {
                    return CommandResult.Lang( EssLang.NOT_FROZEN, target.DisplayName );
                }
                else
                {
                    target.RemoveComponent<FrozenPlayer>();

                    EssLang.UNFROZEN_SENDER.SendTo( source, target.DisplayName );
                    EssLang.UNFROZEN_PLAYER.SendTo( target, source.DisplayName );
                }
            }

            return CommandResult.Success();
        }
    }
}

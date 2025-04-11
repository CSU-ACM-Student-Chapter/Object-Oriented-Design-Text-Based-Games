using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class HelpCommand : Command
    {
        private CommandWords _words;
        public HelpCommand() : this(new CommandWords()){ }

        public HelpCommand(CommandWords commands) : base()
        {
            _words = commands;
            this.Name = "help";
        }

        public override string HelpStatement()
        {
            return "1 word statement. Call whenever in need of help";
        }

        public override bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.WarningMessage($"I cannot help you with {this.SecondWord} {this.ThirdWord}.");
            }
            else
            {
                player.GetObjective();
                player.NormalMessage("White Comments: Normal Message. Common. Also used to display contents of the following three messages below.");
                player.InfoMessage("Blue Commments: Info message to notify or give verification of an action.");
                player.WarningMessage("Yellow Comments: Warning Message to notify your command was able to go through but unable to complete out desired action.");
                player.ErrorMessage("Red Comments: Error Message to notify a failed command execution or worse, Notification for loss of game.");

                foreach(var command in _words.Commands)
                {
                    string commandName = command.Key;
                    Command currCommand = command.Value;
                    player.InfoMessage($"{commandName}");
                    player.NormalMessage($"{currCommand.HelpStatement()}");
                }
            }
            return false;
        }
    }
}

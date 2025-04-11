using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * Will take a list of Commands and insert them into a dictionary with a string
 * This will be used to execute the command once the use gives input
 */
namespace OperationWatergate
{
    public class CommandWords
    {
        private Dictionary<string, Command> _commands;
        private static Command[] _commandArray = {new GoCommand(), new BackCommand(), new ForwardCommand(), new LeaveCommand(), new OpenCommand(), new CloseCommand(), new UnlockCommand(), new LockCommand(),
            new InspectCommand(), new PickupCommand(), new DropCommand(), new StoreCommand(), new ThrowCommand(), new InventoryCommand(), new ObjectiveCommand(), new QuitCommand()};

        public Dictionary<string, Command> Commands { get { return _commands; } }
        public CommandWords() : this(_commandArray) { }

        public CommandWords(Command[] commandList)
        {
            _commands = new Dictionary<string, Command>();
            foreach(Command command in commandList)
            {
                _commands[command.Name] = command;
            }
            // Command help will be in every CommandWord
            Command help = new HelpCommand(this);
            _commands[help.Name] = help;
        }

        public Command Get(string word)
        {
            Command command = null;
            _commands.TryGetValue(word, out command);
            return command;
        }

        public string Description()
        {
            string commandNames = "";
            Dictionary<string, Command>.KeyCollection keys = _commands.Keys;
            foreach(string commandName in keys)
            {
                commandNames += " " + commandName;
            }

            return commandNames;
        }
    }
}

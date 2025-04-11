using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class Parser
    {
        private CommandWords _commands;

        public Parser() : this(new CommandWords()) { }

        public Parser(CommandWords newCommands)
        {
            _commands = newCommands;
        }

        public Command ParseCommand(string commandString)
        {
            Command command = null;
            string[] words = commandString.Split(' ');
            if(words.Length > 0)
            {
                command = _commands.Get(words[0]);
                if (command != null) 
                {
                    if (words.Length > 1)
                    {
                        command.SecondWord = words[1];
                        if(words.Length > 2)
                        {
                            command.ThirdWord = words[2];  
                        }
                        else
                        {
                            command.ThirdWord = null;
                        }
                    }
                    else
                    {                      
                        command.SecondWord = null;
                    }
                }
                else
                {
                    // Debug line of code
                    Console.WriteLine("Not a valid command");
                }
            }
            else
            {
                // Debug line of code
                Console.WriteLine("No words parsed!");
            }
            return command;
        }

        public string Description()
        {
            return _commands.Description();
        }
    }
}

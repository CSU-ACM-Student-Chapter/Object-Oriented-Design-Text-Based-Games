using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class LockCommand : Command
    {
        public LockCommand() : base()
        {
            this.Name = "lock";
        }
        public override string HelpStatement()
        {
            return "3 word statement. Locks the specified exit direction with the correct key." +
                "\"lock + exit direction + key\"";
        }
        
        public override bool Execute(Player player)
        {
            if (this.HasThirdWord())
            {
                player.Lock(SecondWord, ThirdWord);
            }
            else if(!this.HasThirdWord()) 
            {
                player.ErrorMessage($"\nLock {SecondWord} with what?");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.ErrorMessage("\nLock what?");
                player.NormalMessage(HelpStatement());
            }
            return false;
        }
    }
}

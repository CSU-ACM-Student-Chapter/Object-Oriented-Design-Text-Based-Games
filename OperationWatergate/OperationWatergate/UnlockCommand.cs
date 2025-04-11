using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class UnlockCommand : Command
    {
        public UnlockCommand() : base()
        {
            this.Name = "unlock";
        }

        public override string HelpStatement()
        {
            return "3 word statement. Unlocks the specified exit direction with the correct key." +
                "\"unlock + exit direction + key\"";
        }

        public override bool Execute(Player player)
        {
            if (this.HasThirdWord())
            {
                player.Unlock(SecondWord, ThirdWord);
            }
            else if (!this.HasThirdWord())
            {
                player.ErrorMessage($"\nUnlock {SecondWord} with what?");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.ErrorMessage("\nUnlock what?");
                player.NormalMessage(HelpStatement());
            }
            return false;
        }
    }
}

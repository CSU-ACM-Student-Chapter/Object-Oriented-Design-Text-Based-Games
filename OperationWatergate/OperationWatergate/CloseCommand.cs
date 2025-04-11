using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class CloseCommand : Command
    {
        public CloseCommand() : base()
        {
            this.Name = "close";
        }

        public override string HelpStatement()
        {
            return "2 word statement. Closes the door. \"close + exit direction\".";
        }
        
        public override bool Execute(Player player)
        {
            if(!this.HasSecondWord() || this.HasThirdWord())
            {
                player.ErrorMessage("\nClose what?");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.Close(this.SecondWord);
            }
            return false;
        }
    }
}

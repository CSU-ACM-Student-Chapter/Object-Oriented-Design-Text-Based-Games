using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class OpenCommand : Command
    {
        public OpenCommand() : base()
        {
            this.Name = "open";
        }
        public override string HelpStatement()
        {
            return "2 word statement. Opens the door. \"open + exit direction\".";
        }
        override
        public bool Execute(Player player)
        {
            if(!this.HasSecondWord() || this.HasThirdWord())
            {
                player.ErrorMessage("\nOpen What?");
                player.NormalMessage(HelpStatement());  
            }
            else
            {
                player.Open(this.SecondWord);
            }
            return false;
        }
    }
}

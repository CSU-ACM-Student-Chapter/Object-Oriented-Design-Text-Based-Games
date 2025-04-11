using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class ForwardCommand : Command
    {
        public ForwardCommand() : base()
        {
            this.Name = "forward";
        }
        public override string HelpStatement()
        {
            return "1 word statement. Use to going back to container you previously left from.\n" +
                "Back command must have been recently used for command to work. ";
        }

        public override bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.ErrorMessage("\nCannot go forward.");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.ForwardTo();
            }
            return false;
        }
    }
}

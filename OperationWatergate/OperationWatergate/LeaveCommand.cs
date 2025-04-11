using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class LeaveCommand : Command
    {
        public LeaveCommand() : base()
        {
            this.Name = "leave";
        }

        public override string HelpStatement()
        {
            return "1 word statement. Will leave the current item container. Will still remain in the same room.";
        }

        public override bool Execute(Player player)
        {
            if(this.HasSecondWord())
            {
                player.ErrorMessage("Cannot leave.");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.LeaveContainer();
            }
            return false;
        }
    }
}

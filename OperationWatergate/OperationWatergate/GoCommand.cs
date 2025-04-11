using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class GoCommand : Command
    {
        public GoCommand() : base()
        {
            this.Name = "go";
        }

        public override string HelpStatement()
        {
            return "Enter 'go + exit' to enter another room, enter 'go to + component', to go a location/component in a room.";       
        }

        public override bool Execute(Player player)
        {
            if (this.HasThirdWord())
            {
                if (this.SecondWord == "to")
                {
                    player.MoveTo(this.ThirdWord);
                }
                else
                {
                    player.WarningMessage("Go where?");
                    player.NormalMessage(HelpStatement());
                }
            }
            else if(this.HasSecondWord())
            {
                player.WalkTo(this.SecondWord);
            }
            else
            {
                player.WarningMessage("Go where?");
                player.NormalMessage(HelpStatement());
            }
            return false;
        }
    }
}

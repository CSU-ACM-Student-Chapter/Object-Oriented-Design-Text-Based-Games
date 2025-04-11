using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class PickupCommand : Command
    {
        public PickupCommand() : base()
        {
            this.Name = "pickup";
        }
        public override string HelpStatement()
        {
            return "2 word statement. Picks up the desired item or container.\"pickup + item name\"";
        }
        
        public override bool Execute(Player player)
        {
            if(this.HasThirdWord() || !this.HasSecondWord())
            {
                player.ErrorMessage("Pickup what?");
                player.NormalMessage(HelpStatement());
            }
            else if (this.HasSecondWord())
            {
                player.Pickup(SecondWord);
            }
            return false;
        }

    }
}

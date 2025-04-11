using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class InspectCommand : Command
    {
        public InspectCommand() : base()
        {
            this.Name = "inspect";
        }

        public override string HelpStatement()
        {
            return "2 word statement. Inspects an item either being help, or the item open in the room or current item container.\n" +
                "\"inspect + item name\"";
        }

        public override bool Execute(Player player)
        {
            if (!this.HasSecondWord() || this.HasThirdWord())
            {
                player.ErrorMessage("\nInspect what?");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.Inspect(this.SecondWord);
            }
            return false;
        }
    }
}

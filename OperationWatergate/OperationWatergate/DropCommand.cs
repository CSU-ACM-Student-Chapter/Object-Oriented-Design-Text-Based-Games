using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class DropCommand : Command
    {
        public DropCommand() : base()
        {
            this.Name = "drop";
        }

        public override string HelpStatement()
        {
            return "2 word statement. To drop an item, \"drop + item name\".";
        }
        public override bool Execute(Player player)
        {
            if (!this.HasSecondWord() || this.HasThirdWord())
            {
                player.ErrorMessage("\nDrop what?");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.Drop(SecondWord);
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class BackCommand : Command
    {
        public BackCommand() : base()
        {
            this.Name = "back";
        }

        public override string HelpStatement()
        {
            return "1 word statement. To use going from a visited container to a previous container without using the leave command.";
        }

        public override bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.ErrorMessage($"\nBack to what?");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.BackTo();
            }
            return false;
        }
    }
}

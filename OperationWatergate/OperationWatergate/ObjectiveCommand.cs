using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class ObjectiveCommand : Command
    {
        public ObjectiveCommand() : base()
        {
            this.Name = "objective";
        }

        public override string HelpStatement()
        {
            return "1 word statement. Gives the overall and current objective to the game as well as helpful hints.";
        }

        public override bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.ErrorMessage("\nCannot give objective.");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.GetObjective();
            }
            return false;
        }
    }
}

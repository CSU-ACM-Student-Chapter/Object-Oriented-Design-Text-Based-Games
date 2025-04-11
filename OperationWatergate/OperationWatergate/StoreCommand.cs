using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class StoreCommand : Command
    {
        public StoreCommand() : base()
        {
            this.Name = "store";
        }
        public override string HelpStatement()
        {
            return "2 word statement. Stores the desired item in backpack.\"store + item name\"";
        }
        override
        public bool Execute(Player player)
        {
            if(!this.HasSecondWord() || this.HasThirdWord())
            {
                player.ErrorMessage("\nStore what?");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.Store(SecondWord);
            }
            return false;
        }
    }
}

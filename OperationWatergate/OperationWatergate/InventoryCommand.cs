using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class InventoryCommand : Command
    {
        public InventoryCommand() : base()
        {
            this.Name = "inventory";
        }

        public override string HelpStatement()
        {
            return "1 word statement. Gets inventory of both what's in your hands and backpack.\n" +
                "2 word statement. For specifically either hands or backpack inventory \"inventory + hands/backpack\".";
        }
        public override bool Execute(Player player)
        {
            if(this.HasThirdWord())
            {
                player.ErrorMessage("\nInventory of what?");
                player.NormalMessage(HelpStatement());
            }
            else if(this.HasSecondWord())
            {
                if(SecondWord == "backpack")
                {
                    player.BackpackInventory();
                }
                else if(SecondWord == "hands")
                {
                    player.HandsInventory();
                }
                else
                {
                    player.ErrorMessage("\nInventory of what?");
                    player.NormalMessage(HelpStatement());
                }
            }
            else
            {
                player.BackpackInventory();
                player.HandsInventory();
            }
            return false;
        }
    }
}

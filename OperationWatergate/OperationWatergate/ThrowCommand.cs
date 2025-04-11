using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class ThrowCommand : Command
    {
        public ThrowCommand()
        {
            this.Name = "throw";
        }

        public override string HelpStatement()
        {
            return "3 word statement. Throws the selected item at the character. \"throw + item name + character name\"";
        }

        public override bool Execute(Player player)
        {
            if (!this.HasSecondWord())
            {
                player.ErrorMessage($"Throw what at who?");
                player.NormalMessage(HelpStatement());
            }
            else if (!this.HasThirdWord())
            {
                player.ErrorMessage($"Throw {SecondWord} at who?");
                player.NormalMessage(HelpStatement());
            }
            else
            {
                player.Throw(SecondWord, ThirdWord);
            }
            return false;
        }
    }
}

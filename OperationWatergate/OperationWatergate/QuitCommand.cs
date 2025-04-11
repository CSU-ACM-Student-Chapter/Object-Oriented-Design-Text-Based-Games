using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class QuitCommand : Command
    {
        public QuitCommand() : base()
        {
            this.Name = "quit";
        }
        public override string HelpStatement()
        {
            return "1 word statement. Quits the game. Will have to be restarted once command is inserted. ";
        }

        public override bool Execute(Player player)
        {
            bool answer = true;
            if(this.HasSecondWord())
            {
                player.WarningMessage("\nI cannot quit " + this.SecondWord);
                player.NormalMessage(HelpStatement());
                answer = false;
            }
            return answer;
        }
    }
}

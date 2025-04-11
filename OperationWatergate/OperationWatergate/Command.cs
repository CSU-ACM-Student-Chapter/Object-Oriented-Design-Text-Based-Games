using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public abstract class Command
    {
        private string _name;
        private string _secondWord;
        private string _thirdWord;

        public string Name { get { return _name; } set { _name = value; } }
        public string SecondWord { get { return _secondWord; } set { _secondWord = value; } }
        public string ThirdWord { get { return _thirdWord; } set {_thirdWord = value;} }
        
        public Command()
        {
            this.Name = "";
            this.SecondWord = null;
            this.ThirdWord = null;
        }

        public bool HasSecondWord()
        {
            return this.SecondWord != null;
        }

        public bool HasThirdWord()
        {
            return this.ThirdWord != null;
        }

        public abstract string HelpStatement();

        public abstract bool Execute(Player player);
    }
}

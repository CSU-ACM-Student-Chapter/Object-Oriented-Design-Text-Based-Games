using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class Keyed :IKeyed
    {
        private IItem originalKey;
        private IItem insertedKey;

        public bool HasKey { get { return insertedKey == originalKey; } }

        public Keyed(string keyName)
        {
            originalKey = new Item(keyName, 0.1f, 0.01f);
            insertedKey = originalKey;
        }

        public IItem Insert(IItem key)
        {
            IItem oldKey = insertedKey;
            insertedKey = key;
            return oldKey;
        }

        public IItem Remove()
        {
            IItem oldKey = insertedKey;
            insertedKey = null;
            return oldKey;  
        }
    }
}

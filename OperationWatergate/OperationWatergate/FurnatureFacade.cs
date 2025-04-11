/*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class FurnatureFacade
    {
        private static FurnatureFacade _instance = null;
        private int numOfChairs;

        private int numOfBinders;
        private int numOfFilingCabinet;
        private int numOfShoppingbags;

        private int numOf
        private int numOfDocuments;
        private int numOfKeys;
        private int numOfFilingCabinet;


        public static FurnatureFacade Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FurnatureFacade();
                }
                return _instance;
            }
        }

        public static IItemContainer MakeFurnature(string furnatureName, bool containsSecretFile)
        {
            IItemContainer furnature = null;
            switch (furnatureName)
            {
                case "Desk":
                    furnature = new ItemContainer("desk");
                    IItemContainer newContainer = new ItemContainer("binder");

                    IItem newItem = new Item("Computer");
                    IItem decorator = new Item("keyboard")
                    newContainer.AddContainer(furnature);

                    break;
                case "RoundTable":
                    furnature = new ItemContainer("roundtable");
                    break;
                case "Cabinet":
                    furnature = new ItemContainer("cabinet");
                    break;
                case "Chair":
                    furnature = new ItemContainer("Chair{");
                    break;
                case "Sofa":
                    furnature = new ItemContainer("Sofa");
                    break;

                default:
                    furnature = new ItemContainer($);
                    break;
            }

            return furnature;
        }
    }
}
*/

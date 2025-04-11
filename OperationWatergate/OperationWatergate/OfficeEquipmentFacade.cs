/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class OfficeEquipmentFacade
    {
        private static OfficeEquipmentFacade _instance = null;
        private int numOfBinders;
        private int numOfFolders;
        private int num

        public static OfficeEquipmentFacade Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OfficeEquipmentFacade();
                }
                return _instance;
            }
        }
        private OfficeEquipmentFacade()
        {

        }

        public static IItemContainer MakeEquipment(string furnature, string furnatureName)
        {
            IItemContainer equipment = null;
            switch (furnature)
            {
                case "Binder":
                    equipment = new ItemContainer($"binder{i + 1}");
                    break;
                case "Folder":
                    furnatur = new ItemContainer($"Desk{i + 1}");
                    break;
                case "File":
                    furnature = new ItemContainer("Cabinet");
                    break;
                case "PencilContainer":
                    furnature = new ItemContainer($"Shelf{i + 1}");
                    break;
                case "":
                    furnatur = new ItemContainer($"Chair{i + 1}");
                    break;
                case "Sofa":
                    furnatur = new ItemContainer($"Sofa{i + 1}");
                    break;

                default:
                    furnatur = new ItemContainer($);
                    break;
            }
        }
    }
}
*/
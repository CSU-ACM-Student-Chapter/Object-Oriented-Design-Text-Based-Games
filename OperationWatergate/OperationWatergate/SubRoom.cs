using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    // Created this class to have entrances contatin sub locations in a room, Office -> Desk, Filing Cabinet; Main Lobby -> Front Desk, Magazine Stand
        //To implement this class, refer to drawing below
    public class SubRoom
    {

        private Dictionary<string, SubRoom> _sections;
        private string _tag;

        public string Tag { get { return _tag; } set { _tag = value; } }

        public SubRoom() : this("No tag") { }

        public SubRoom(string tag)
        {
            _sections = new Dictionary<string, SubRoom>();
            this.Tag = tag;
        }

        public void SetSection(string sectionPath, SubRoom subRoomName)
        {
            _sections[sectionPath] = subRoomName;
        }

        public SubRoom GetSection(string sectionPath)
        {
            SubRoom subRoom = null;
            _sections.TryGetValue(sectionPath, out subRoom);
            return subRoom;
        }

        public string GetSections()
        {
            string sectionPaths = "Sections: ";
            foreach(string sections in _sections.Keys)
            {
                sectionPaths += _sections.Keys;
            }
            return sectionPaths;
        }

        public string Description()
        {
            return $"\nYou are in {this.Tag}\n***{this.GetSections()}\n";
        }
    }
}

/*
                       *                    Room                    <- Office
                   /       \               
                  *         *          IItemContainers              <- Desk, Drawer
                / | \     /   \   
               *  *  *   *     *   Containers within Containers     <- ^Binder, Computer, Phone      ^Drawer1/2       
              /\         |\   /||\
             *  *        * *  ****          IItems                  <- ^DocumentA/B     ^Magazines, Books, Documents1/2/3/4
                                

        Root = Room
        Internal Nodes = Containers
        Leaf Nodes = IItems 

        None leaf nodes (Rooms/IItemContainers) should contain Add/RemoveChildren.
        All should implement some form of an execute(); <- Delegates work to children. This will be Weight. 
                                            
                                                
 */


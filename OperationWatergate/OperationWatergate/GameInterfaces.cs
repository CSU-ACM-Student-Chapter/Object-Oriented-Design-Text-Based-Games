using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public interface ITrigger
    {
    }

    public interface IGameEvent
    {
        bool Triggered { get; }
        public void Execute(Player player);
    }

    public interface IRoomDelegate
    {
        public Room ContainingRoom { set; get; }

        public void RoomDidSetExit(string exitName, Door door);

        public void RoomDidGetExit(string exitName, Door door);

        public void RoomDidGetExits(string exits);

        public void RoomDidGetDescription(string description);
    }
    
    public interface ICloseable
    {
        bool IsClosed { get; }
    
        bool IsOpen { get; }
    
        bool Close();
    
        bool Open();
    
        bool CanClose { get; }
    
        bool CanOpen { get; }   
    }

    public interface ILockable : IKeyed
    {
        bool IsLocked { get; }
    
        bool IsUnlocked { get; }
    
        bool Lock();
    
        bool Unlock();
    
        bool CanLock { get; }
    
        bool CanUnlock { get; }
    
        bool CanClose { get; }
    
        bool CanOpen { get; }
    
        IKeyed Keyed { set; get; }
    }

    public interface IKeyed
    {
        bool HasKey { get; }
        IItem Insert(IItem key);
        IItem Remove();
    }

    public interface IItem : IComponent, ITrigger
    {
        string Name { get; }

        string LongName { get; }

        string Description { get; }

        bool IsContainer { get; }

        bool AddDecorator(IItem decorator);
    }


    public interface IContainer  
    {
        public float VolumeLeft { get; }
        IItemContainer GetContainer(string containerName);
        IItem GetItem(string itemName);
        string GetContainers();
        string GetItems();
        bool AddContainer(IItemContainer container);
        IItemContainer RemoveContainer(string containerName);
        bool AddItem(IItem item);
        IItem RemoveItem(string itemName);
    }

    // Allows IItemContainer to still container IItem while room does not 
    // Extends IItem which contains IComponent
    public interface IItemContainer : IContainer, IItem
    {

    }

    // Extends IContainer which contains IComponent
    public interface IRoom : IComponent, IContainer
    {

    }

    // Composite Pattern
    public interface IComponent
    {
        // An Execute() like method where work is delegated to children.
        float Weight { get; }

        float Volume { get; }
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

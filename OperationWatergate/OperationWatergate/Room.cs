using System; 


using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class Room : ITrigger, IRoom
    {
        private Dictionary<string, IItemContainer> _containers;
        private Dictionary<string, IItem> _items;
        private float _weight;
        private float _volume;
        private readonly float _initialVolume;
        private Dictionary<string, Door> _exits;
        private string _tag;

        public string Tag { get { return _tag; } set { _tag = value; } }

        public float Weight
        {
            get
            {
                Dictionary<string, IItem>.ValueCollection itemValues = _items.Values;
                Dictionary<string, IItemContainer>.ValueCollection containerValues = _containers.Values;
              
                float totalContainedWeight = 0;
                foreach (IItemContainer container in itemValues)
                {
                    totalContainedWeight += container.Weight;
                }
                foreach (IItem item in itemValues)
                {
                    totalContainedWeight += item.Weight;
                }
                return _weight + totalContainedWeight;
            }
        }

        public float VolumeLeft
        {
            get
            {
                Dictionary<string, IItem>.ValueCollection itemValues = _items.Values;
                Dictionary<string, IItemContainer>.ValueCollection containerValues = _containers.Values;
                float totalContainedVolume = 0;
                foreach (IItemContainer container in containerValues)
                {
                    totalContainedVolume += container.Volume;
                }
                foreach (IItem item in itemValues)
                {
                    totalContainedVolume += item.Volume;
                }
                return _initialVolume - totalContainedVolume;
            }
        }

        public float Volume { get { return _initialVolume; } }

        public Room() : this("No tag", 1000f) { }

        public Room(string tag) : this(tag, 1000f) { }

        public Room(string tag, float volume)
        {
            _exits = new Dictionary<string, Door>();
            _containers = new Dictionary<string, IItemContainer>();
            _items = new Dictionary<string, IItem>();
            _weight = 0f;
            _initialVolume = volume;
            this.Tag = tag;

        }
        public void SetExit(string exitName, Door door)
        {
            _exits[exitName] = door;
        }

        public Door GetExit(string exitName)
        {
            Door door = null;
            _exits.TryGetValue(exitName, out door);
            return door;
        }

        public string GetExits()
        {
            string exitNames = "Exits: ";
            Dictionary<string, Door>.KeyCollection keys = _exits.Keys;
            foreach(string exitName in keys)
            {
                exitNames += " " + exitName;
            }
            return exitNames;
        }

        public IItemContainer GetContainer(string containerName)
        {
            IItemContainer containerToReturn = null;
            _containers.TryGetValue(containerName, out containerToReturn);
            return containerToReturn;
        }
        public IItem GetItem(string itemName)
        {
            IItem itemToReturn = null;
            _items.TryGetValue(itemName, out itemToReturn);
            return itemToReturn;
        }
        public string GetContainers()
        {
            string containerNames = "Floor: ";
            Dictionary<string, IItemContainer>.KeyCollection keys= _containers.Keys;
            foreach (string containerName in keys)
            {
                containerNames += " " + containerName;
            }
            return containerNames;
        }

        public string GetItems()
        {
            string itemNames = "Items:";
            Dictionary<string, IItem>.KeyCollection keys = _items.Keys;
            foreach (string itemName in keys)
            {
                itemNames += " " + itemName;
            }

            return itemNames;
        }

        public bool AddContainer(IItemContainer itemContainer)
        {
            bool containerAdded = false;
            if(VolumeLeft - itemContainer.Volume >= 0 && !_containers.ContainsKey(itemContainer.Name))
            {     
                _containers.Add(itemContainer.Name, itemContainer);
                containerAdded = true;
                
            }
            return containerAdded;
        }

        public IItemContainer RemoveContainer(string containerName)
        {
            IItemContainer containerToReturn = null;
            _containers.TryGetValue(containerName, out containerToReturn);
            if (containerToReturn != null)
            {
                containerToReturn = _containers[containerName];
                _containers.Remove(containerName);
            }
            return containerToReturn;
        }

        public bool AddItem(IItem item)
        {
            bool itemAdded = false;
            if(VolumeLeft - item.Volume >= 0 && !_items.ContainsKey(item.Name))
            {   
                    _items.Add(item.Name, item);
                    itemAdded = true;
                
            }
            return itemAdded; 
        }

        public IItem RemoveItem(string itemName)
        {
            IItem itemToReturn = null;
            _items.TryGetValue(itemName, out itemToReturn);
            if(itemToReturn != null)
            {
                itemToReturn = _items[itemName];
                _items.Remove(itemName);
            }
            return itemToReturn;
        }

        public string Description()
        {
            return $"\n***{this.GetExits()}" +
                $"\n***{this.GetContainers()}" +
                $"\n***{this.GetItems()}";
        }
    }
}




                                
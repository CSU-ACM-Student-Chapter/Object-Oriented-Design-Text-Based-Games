using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class Item : IItem
    {
        private string _name;
        private float _weight;
        private float _volume;
        public IItem _decorator;

        public string Name { get { return _name; } }    

        public string LongName
        {
            get
            {
                return Name + (_decorator == null ? "" : $" with {_decorator.LongName} ");
            }
        }
        public float Weight { get { return _weight + (_decorator == null?0: _decorator.Weight); } }

        public float Volume { get { return _volume + (_decorator == null? 0 : _decorator.Volume); } }
        
        public string Description { get { return LongName + ", " + Weight; } }
    
        public bool IsContainer { get { return false; } }

        public Item() : this("Nameless") { }

        public Item(string name) : this(name, 1f, .3f) { }
        public Item(string name, float weight, float volume)
        {
            _name = name;
            _weight = weight;
            _volume = volume;
            _decorator = null;
        }

        public bool AddDecorator(IItem decorator)
        {
            bool decoratorAdded = false;
            if (Volume - decorator.Volume <= 0)
            {
                if (_decorator == null)
                {
                    _decorator = decorator;
                }
                else
                {
                    _decorator.AddDecorator(decorator);
                }
                decoratorAdded = true;
            }
            return decoratorAdded;
        }

        public string Inspect()
        {
            return Description;
        }
    }
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/readonly
    public class ItemContainer : IItemContainer
    {
        private string _name;
        private float _weight;
        private float _weightCapacity;
        private float _volume;
        private readonly float _initialVolume;

        public IItem _decorator;
        private Dictionary<string, IItem> _items;
        private Dictionary<string, IItemContainer> _containers;

        public string Name { get { return _name; } }

        public string LongName
        {
            get
            {
                return Name + (_decorator == null ? "" : $" with {_decorator.Name}");
            }
        }

        public float Weight
        {
            get
            {
                Dictionary<string, IItem>.ValueCollection itemValues = _items.Values;
                Dictionary<string, IItemContainer>.ValueCollection containerValues = _containers.Values;
                float totalContainedWeight = 0;
                foreach (IItemContainer container in containerValues)
                {
                    totalContainedWeight += container.Weight;
                }
                foreach (IItem item in itemValues)
                {
                    totalContainedWeight += item.Weight;
                }
                return _weight + totalContainedWeight + (_decorator == null ? 0 : _decorator.Weight);
            }
        }
        public float WeightCapacity { get { return _weightCapacity; } }

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

        public float Volume { get { return _initialVolume + (_decorator == null ? 0 : _decorator.Volume); } }
        public bool IsContainer { get { return true; } }

        public ItemContainer(string name) : this(name, 0f, 10f, 100f) { }

        public ItemContainer(string name, float weight) : this(name, weight, 10f, 100f) { }

        public ItemContainer(string name, float weight, float weightCapacity, float volume)
        { 
            _name = name;
            _weight = weight;
            _weightCapacity = weightCapacity;
            _volume = volume;
            _initialVolume = volume;
            _decorator = null;
            _items = new Dictionary<string, IItem>();
            _containers = new Dictionary<string, IItemContainer>();
        }

        public bool AddDecorator(IItem decorator)
        {
            bool decoratorAdded = false;
            if(Volume - decorator.Volume >= 0)
            {
                if (_decorator == null)
                {
                    _decorator = decorator;
                }
                else
                {
                    _decorator.AddDecorator(decorator);
                }
                decoratorAdded = true;
            }
            return decoratorAdded;
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
            string containerNames = "Components: ";
            Dictionary<string, IItemContainer>.KeyCollection keys = _containers.Keys;
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
            if (VolumeLeft - itemContainer.Volume >= 0)
            {
                containerAdded = true;
                _containers.Add(itemContainer.Name, itemContainer);
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
            if (VolumeLeft - item.Volume >= 0)
            {
                itemAdded = true;
                _items.Add(item.Name, item);
            }
            return itemAdded;
        }

        public IItem RemoveItem(string itemName)
        {
            IItem itemToReturn = null;
            _items.TryGetValue(itemName, out itemToReturn);
            if (itemToReturn != null)
            {
                itemToReturn = _items[itemName];
                _items.Remove(itemName);
            }
            return itemToReturn;
        }

        public string Inspect()
        {
            string itemNames = "";
            foreach(string item in _items.Keys)
            {
                itemNames += item;
            }
            return $"This contains {itemNames} for a total of {this.Weight}lbs.";
            
        }
        public string Description
        {
            get
            {
                return $"{this.LongName}" +
                    $"\n***{this.GetContainers()}" +
                    $"\n***{this.GetItems()}";
            }
        }
    }
}

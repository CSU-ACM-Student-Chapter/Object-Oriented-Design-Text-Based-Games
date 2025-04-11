using System.ComponentModel;

namespace OperationWatergate
{
    public class Player
    {
        private int _strength;
        private IItemContainer _hands; 
        private IItemContainer _slingBackpack;
        private int _currentObjective;
        private int _totalObjectivesToComplete;

        private Room _currentRoom = null;
        private IItemContainer _currentContainer = null;
        private List<IItemContainer> _visitedComponents;
        private int _currentComponent;

        public int Strength { get { return _strength; } set { _strength = value; } }
        public int CurrentObjective { get { return _currentObjective; } set { _currentObjective = value; } }
        public int TotalObjectivesToComplete { get { return _totalObjectivesToComplete; } }
        public Room CurrentRoom { get { return _currentRoom;} set{_currentRoom = value; } }
        public IItemContainer CurrentContainer { get { return _currentContainer; } set { _currentContainer = value; } }
        
        public IItemContainer Backpack { get { return _slingBackpack; } }

        public IItemContainer Hands { get { return _hands; } }
        
        public Player(Room room)
        {
            _strength = 10;
            _hands = new ItemContainer("hands", 0f, _strength, 1f);
            _slingBackpack = new ItemContainer("backpack", 1f, _strength*2, 2f);
            _currentObjective = 1;
            _totalObjectivesToComplete = 3;
            _currentRoom = room;
            _visitedComponents = new List<IItemContainer>();
            _currentComponent = -1;
            NotificationCenter.Instance.AddObserver("KnockedDownPlayer", KnockedDownPlayer);
        }

        public void HandsInventory()
        {
            NormalMessage(_hands.Description);
        }

        public void BackpackInventory()
        {
            NormalMessage(_slingBackpack.Description);
        }

        public void GetObjective()
        {
            InfoMessage("Game Objective:");
            NormalMessage(PlayersObjective());
            InfoMessage("Current Objective:");
            switch (CurrentObjective)
            {
                case 1:
                    NormalMessage(PlayersObjective1());
                    break;
                case 2:
                    NormalMessage(PlayersObjective2());
                    break;
                case 3:
                    NormalMessage(PlayersObjective3());
                    break;
                default:
                    break;
            }
        }

        public void KnockedDownPlayer(Notification notification)
        {
            Character character = (Character)notification.Object;
            InfoMessage($"You've knocked down {character.Name}. You have 45 seconds till he's back up.");
        }


        public void WalkTo(string direction)
        {
            Door door = this.CurrentRoom.GetExit(direction);
            if (door != null)
            {
                if (door.IsOpen)
                {
                    CurrentRoom = door.RoomOnTheOtherSideFrom(CurrentRoom);
                    _visitedComponents.Clear();
                    _currentComponent = -1;
                    
                    NotificationCenter.Instance.PostNotification(new Notification("PlayerDidEnterRoom", this));
                    InfoMessage($"You are {this.CurrentRoom.Tag}");
                    NormalMessage($"{this.CurrentRoom.Description()}");
                }
                else
                {
                    WarningMessage($"The door on {direction} is closed");
                }
            }
            else
            {
                WarningMessage($"Cannot go {direction}");
                NormalMessage($"{this.CurrentRoom.Description()}");
            }
        }

        public void MoveTo(string containerName)
        {
            IItemContainer container = this.CurrentRoom.GetContainer(containerName);
            if (container != null)
            {
                if(container != this.CurrentContainer)
                {
                    UpdateVisitedComponentsHistory(container);
                    this.CurrentContainer = container;
                    InfoMessage($"\nYou are now looking at {container.Name}");
                    NormalMessage($"\n{this.CurrentContainer.Description}");
                }
                else
                {
                    WarningMessage($"You are already here at the {this.CurrentContainer.Name}");
                }
            }
            else
            {
                if (this.CurrentContainer != null)
                {
                    container = this.CurrentContainer.GetContainer(containerName);
                    if (container != null)
                    {
                        UpdateVisitedComponentsHistory(container);
                        this.CurrentContainer = container;
                        InfoMessage($"\nYou are now looking at {container.Name}");
                        NormalMessage($"\n{this.CurrentContainer.Description}");
                    }
                    else
                    {
                        WarningMessage($"Cannot go to {containerName}");
                    }
                }
                else
                {
                    WarningMessage($"Cannot go to {containerName}");
                }
            }
        }
        
        public void BackTo()
        {
            if(_currentComponent > 0)
            {
                _currentComponent--;
                this.CurrentContainer = _visitedComponents[_currentComponent];
                InfoMessage($"\nYou have moved back to {this.CurrentContainer.Name}");
                NormalMessage($"\n{this.CurrentContainer.Description}");
            }
            else
            {
                WarningMessage("Cannot go back any further.");
            }  
        }

        public void ForwardTo()
        {
            if (_currentComponent < _visitedComponents.Count-1)
            {
                _currentComponent++;
                this.CurrentContainer = _visitedComponents[_currentComponent];
                InfoMessage($"\nYou have moved forward to {this.CurrentContainer.Name}");
                NormalMessage($"\n{this.CurrentContainer.Description}");
            }
            else
            {
                WarningMessage("Cannot go forward any further.");
            }
        }

        public void LeaveContainer()
        {
            string containerName = this.CurrentContainer.Name;
            if (this.CurrentContainer != null)
            {
                this.CurrentContainer = null;
                _visitedComponents.Clear();
                _currentComponent = -1;
                InfoMessage($"You have left the {containerName}.");
            }
            else
            {
                WarningMessage("You are not currently at a place that contains items. Just in the room itself.");
            }
        }
        
        // Will update either to an empty list or not.
            // WalkTo (walking to another room), clears the list. Going to a new subtree container within the room clears the list up to current point
        public void UpdateVisitedComponentsHistory(IItemContainer container)
        {
            // Do not have any container ahead of us. 
            if (_currentComponent == -1 || _visitedComponents[_currentComponent] != container)
            {
                _visitedComponents.Add(container);
                _currentComponent++;
            }

            // Will remove container ahead of us.
                // Example: If we go YouTube>Reddit>Twitch>Facebook. If I go undo to YouTube then go to Amazon, everything after YouTube clears
            if (_currentComponent < _visitedComponents.Count - 1)
            {
                _visitedComponents.RemoveRange(_currentComponent + 1, _visitedComponents.Count - _currentComponent - 1);
            }
        }

        // Drops item. Player can drop from hands and even backpack without picking up the item;
        public void Drop(string itemName)
        {
            // Will check to see if item being dropped is in backpack
            Object item = _hands?.RemoveContainer(itemName) ?? _hands?.RemoveItem(itemName) ??
                _slingBackpack?.RemoveContainer(itemName) ?? _slingBackpack?.RemoveItem(itemName);
            if (item != null)
            {
                // Will check for itemType that was found in backpack. 
                Type t = item.GetType();
                if (t.Name == "ItemContainer")
                {
                    // Will drop item from backpack into container, else will drop into room
                    if (this.CurrentContainer != null)
                    {
                        if (this.CurrentContainer.AddContainer((IItemContainer)item))
                        {
                            InfoMessage($"Dropped {itemName} \n"); 
                            NormalMessage($"Update list: {this.CurrentContainer.GetContainer(itemName).Description}");
                        }
                        else
                        {
                            WarningMessage("Current container is too full.");
                        }
                    }
                    else
                    {
                        if (this.CurrentRoom.AddContainer((ItemContainer)item))
                        {
                            InfoMessage($"Dropped {itemName}\n");
                            NormalMessage($"Update list: {this.CurrentRoom.GetContainer(itemName).Description}");
                        }
                        else
                        {
                            WarningMessage("Current room is too full.");
                        }
                    }
                }
                else if (t.Name == "Item")
                {
                    if (this.CurrentContainer != null)
                    {
                        if (this.CurrentContainer.AddItem((IItem)item))
                        {
                            InfoMessage($"Dropped {itemName}\n");
                            NormalMessage($"Update list: {this.CurrentContainer.GetItem(itemName).Description}");
                        }
                        else
                        {
                            WarningMessage("Current container is too full.");
                        }
                    }
                    else
                    {
                        if (this.CurrentRoom.AddItem((IItem)item))
                        {
                            InfoMessage($"Dropped {itemName} \n");
                            NormalMessage($"Update list: {this.CurrentRoom.GetItem(itemName).Description}");

                        }
                        else
                        {
                            WarningMessage("Current room is too full.");
                        }
                    }
                }
                else
                {
                    WarningMessage($"\nUnable to drop {itemName} from hands.");
                }
            }
            else
            {
                WarningMessage($"You are not holding {itemName}.");
            }
        }

        // https://stackoverflow.com/questions/3296526/what-does-really-happen-when-you-do-gettype
        // https://stackoverflow.com/questions/33775887/how-to-find-an-object-type-in-c
        // Will pickup an item either from backpack, current room, or current container
        public Object Pickup(string itemName)
        {
            Object item = this.CurrentContainer?.RemoveContainer(itemName) ?? this.CurrentContainer?.RemoveItem(itemName)
                ?? this.CurrentRoom?.RemoveContainer(itemName) ?? this.CurrentRoom.RemoveItem(itemName)
                ?? _slingBackpack?.RemoveContainer(itemName) ?? _slingBackpack?.RemoveItem(itemName);
            
            if(item != null)
            {
                Type t = item.GetType();
                if (t.Name == "ItemContainer")
                {
                    if (_hands.AddContainer((ItemContainer)item))
                    {
                        InfoMessage($"Picked up {itemName}");
                    }
                    else
                    {
                        WarningMessage("\nThis item may be too big to carry. Drop something else to pickup");
                    }
                }
                else if(t.Name == "Item")
                {
                    if (_hands.AddItem((IItem)item))
                    {
                        InfoMessage($"Picked up {itemName}");
                        NotificationCenter.Instance.PostNotification(new Notification("PlayerDidPickupItem", this));
                    }
                    else
                    {
                        WarningMessage("\nThis item may be too big to carry. Drop something else to pickup");
                    }
                }
                else
                {
                    WarningMessage("\nUnable to grab your item");
                }
            }
            else
            {
                WarningMessage($"\nThere is no {itemName}");
                InfoMessage("If you'd like to grab an item in container held in your bag, use \"go to backpack\" " +
                    "then \"go to + itemName\"");
            }
            return item;
        }

        public void Throw(string itemName, string characterName)
        {
            Object item = _hands?.RemoveContainer(itemName) ?? _hands?.RemoveItem(itemName);
            Character character = GameWorld.Instance.GetCharacter(characterName);
            if(item != null)
            {

                if (character != null)
                {
                    ThrowTo(item, character);
                }
                else
                {
                    WarningMessage($"There is no player, {characterName}");
                }
            }
            else if((item = Pickup(itemName)) !=  null)
            {
                if (character != null)
                {
                    ThrowTo(item, character);
                }
                else
                {
                    WarningMessage($"There is no player, {characterName}");
                }
            }
            else
            {
                WarningMessage($"There is no item, {itemName}");
            }
        }

        public void ThrowTo(Object item, Character character)
        {
            Type t = item.GetType();
            if (t.Name == "Item")
            {
                IItem itemFound = (IItem)item;
                if (GameWorld.Instance.CheckForCharacterInSameRoom(this))
                {
                    if (this.CurrentRoom.AddItem(itemFound))
                    {
                        character.ReceiveHit(itemFound.Weight);
                    }
                    else
                    {
                        WarningMessage("\nThe room is currently full?? What all you got in there!?");
                    }
                }
                else
                {
                    WarningMessage("The character is not in the room");
                }
            }
            else if (t.Name == "ItemContainer")
            {
                IItemContainer containerFound = (IItemContainer)item;
                if (GameWorld.Instance.CheckForCharacterInSameRoom(this))
                {
                    if (this.CurrentRoom.AddItem(containerFound))
                    {
                        character.ReceiveHit(containerFound.Weight);
                    }
                    else
                    {
                        WarningMessage("\nThe room is currently full?? What all you got in there!?");
                    }
                }
                else
                {
                    WarningMessage("The character is not in the room");
                }
            }
            else
            {
                WarningMessage($"\nUnable to throw the item");
            }
        }

        // Will add to backpack
        public void Store(string itemName)
        {
            Object item = _hands?.RemoveContainer(itemName) ?? _hands?.RemoveItem(itemName);
            if (item != null)
            {
                Type t = item.GetType();
                if (t.Name == "Item")
                {
                    IItem itemFound = (IItem)item;
                    if (_slingBackpack.AddItem(itemFound))
                    {
                        InfoMessage($"{itemName} has been stored.");
                    }
                    else
                    {
                        WarningMessage($"\nThe backpack is currently too full for {itemName}.");
                    }

                }
                else if (t.Name == "ItemContainer")
                {
                    IItemContainer itemFound = (IItemContainer)item;

                    if (_slingBackpack.AddContainer(itemFound))
                    {
                        InfoMessage($"{itemName} has been stored.");
                    }
                    
                }
                else
                {
                    WarningMessage($"\nUnable to throw {itemName}.");
                }
            }
            else
            {
                WarningMessage($"\nThere is no {itemName}.");
            }
        }

        // Will an inspect an item if it's in pplayers hands
        public void Inspect(string itemName)
        {
            Object item = _hands?.RemoveContainer(itemName) ?? _hands?.RemoveItem(itemName);

            if (item != null)
            {
                Type t = item.GetType();
                if(t.Name == "Item")
                {
                    IItem itemFound = (IItem)item;
                    InfoMessage($"The item is {itemFound.Description}. ");
                    this.CurrentRoom.AddItem(itemFound); 
                }
                else if(t.Name == "ItemContainer")
                {
                    IItemContainer containerFound = (IItemContainer)item;
                    InfoMessage($"The item is {containerFound.Description}. ");
                }
                else
                {
                    WarningMessage("\nUnable to find the item.");
                }
            }
            else
            {
                WarningMessage("You are currently not holding {itemName}.");
            }

        }

        //Next 4 methods are for Opening, Closing, and Unlocking, Locking doors
        public void Open(string direction)
        {
            Door door = this.CurrentRoom.GetExit(direction);
            if (door != null)
            {
                if (door.IsClosed)
                {
                    if (door.Open())
                    {
                        InfoMessage($"The door on {direction} is now open.");
                    }
                    else
                    {
                        InfoMessage($"The door on {direction} did not open.");
                    }
                }
                else
                {
                    InfoMessage($"The door on {direction} is already open");
                }
            }
            else
            {
                WarningMessage($"\nThere is no door on {direction}.");
            }
        }

        public void Close(string direction)
        {
            Door door = this.CurrentRoom.GetExit(direction);
            if (door != null)
            {
                if (door.IsOpen)
                {
                    if (door.Close())
                    {
                        InfoMessage($"The door on {direction} is now closed.");
                    }
                    else
                    {
                        InfoMessage($"The door on {direction} did not close.");
                    }
                }
                else
                {
                    InfoMessage($"The door on {direction} is already closed");
                }
            }
            else
            {
                WarningMessage($"\nThere is no door on {direction}.");
            }
        }

        public void Unlock(string direction, string keyName)
        {
            Door door = this.CurrentRoom.GetExit(direction);
            IItem key = _hands.GetItem(keyName);
            if(door != null )
            {
                if(key != null)
                {
                    UnlockWithKey(door, key);
                }
                else
                {
                    WarningMessage($"There is no {keyName} in your {_hands.Name}");
                }
            }
            else
            {
                WarningMessage($"There is no door going {direction}");
            }
        }

        public void Lock(string direction, string keyName)
        {
            Door door = this.CurrentRoom.GetExit(direction);
            IItem key = _hands.GetItem(keyName);
            if (door != null)
            {
                if (key != null)
                {
                    LockWithKey(door, key);
                }
                else
                {
                    WarningMessage($"There is no {keyName} in your {_hands.Name}");
                }
            }
            else
            {
                WarningMessage($"There is no door going {direction}");
            }
        }

        public void UnlockWithKey(Door door, IItem key)
        {
            if (door.IsLocked)
            {
                door.Insert(key);
                if (door.Unlock())
                {
                    InfoMessage($"You unlocked the door {door.RoomOnTheOtherSideFrom(this.CurrentRoom).Tag} with {key.Name}.");
                }
                else
                {
                    WarningMessage($"The door {door.RoomOnTheOtherSideFrom(this.CurrentRoom).Tag} did not unlock.");
                }
            }
            else
            {
                WarningMessage($"The door {door.RoomOnTheOtherSideFrom(this.CurrentRoom).Tag} is already unlocked.");
            }
            door.Remove();
        }

        public void LockWithKey(Door door, IItem key)
        {
            if (door.IsUnlocked)
            {
                door.Insert(key);
                if (door.Lock())
                {
                    InfoMessage($"You locked the door {door.RoomOnTheOtherSideFrom(this.CurrentRoom).Tag} with {key.Name}.");
                }
                else
                {
                    WarningMessage($"The door {door.RoomOnTheOtherSideFrom(this.CurrentRoom).Tag} did not lock.");
                }
            }
            else
            {
                WarningMessage($"The door {door.RoomOnTheOtherSideFrom(this.CurrentRoom).Tag} is already locked.");
            }
            door.Remove();
        }

        // Objectives that are needed to be completed by the player. 
        public string PlayersObjective()
        {
            return "Sneak into the office and steal the top secret files located on the 5th floor.\n " +
                "There are a total of three files. Watch out for the Security Guard, Paul!!";
        }

        public string PlayersObjective1()
        {
            return "Make your way to the custodian/storage room. Find the key to the " +
                "staircase well and make your way up to the 5th floor...Get ready.";
        }

        public string PlayersObjective2()
        {
            return "Retrieve the Secret File. Marked as a \"Secret File\". Be aware, the Security Guard Paul \n" +
                "roams the 5th floor. If you see him, he must be knocked out. However you only have 30 seconds\n" +
                "till he's back up and roaming.";
        }

        public string PlayersObjective3()
        {
            return "Get out of the office...alive and unidentified. Run outside.";
        }

        // Console Print Methods for player
        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }
        public void ColoredMessage(string message, ConsoleColor newColor)
        {
            ConsoleColor oldColer = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            OutputMessage(message);
            Console.ForegroundColor = oldColer;
        }
        public void NormalMessage(String message)
        {
            ColoredMessage(message, ConsoleColor.White);
        }
        public void InfoMessage(string message) 
        {
            ColoredMessage(message, ConsoleColor.Blue);
        }
        public void WarningMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.DarkYellow);
        }
        public void ErrorMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Red);
        }

        public void WinningMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Green);
        }
    }
}

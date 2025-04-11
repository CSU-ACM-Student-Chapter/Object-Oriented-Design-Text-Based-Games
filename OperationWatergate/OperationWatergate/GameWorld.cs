using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class GameWorld
    {
        private static GameWorld _instance = null;
        private List<Room> _rooms;  
        private List<Room> _roomsToCheck;
        private Character _securityGuard;
        private Dictionary<string, Character> _characters;
        private IItem _secretFile;
        
        public static GameWorld Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }

        private Room _entrance;
        private Room _exit;


        public Room Entrance { get { return _entrance; } }

        private Dictionary<ITrigger, IGameEvent> _worldChanges;

        private GameWorld()
        {
            _worldChanges = new Dictionary<ITrigger, IGameEvent>();
            _rooms = new List<Room>();
            _roomsToCheck = new List<Room>();
            _secretFile = new Item("secretfile", .01f, .01f);
            _entrance = CreateWorld();
            _securityGuard = new Character("Paul", "Security Guard", _rooms[6]);
            _characters = new Dictionary<string, Character>();
            _characters.Add(_securityGuard.Name, _securityGuard);
            NotificationCenter.Instance.AddObserver("PlayerDidEnterRoom", PlayerDidEnterRoom);
            NotificationCenter.Instance.AddObserver("PlayerDidPickupItem", PlayerDidPickupItem);
        }

        public Character GetCharacter(string characterName)
        {
            Character characterToReturn = null;
            _characters.TryGetValue(characterName, out characterToReturn);
            return characterToReturn;
        }

        public void PlayerDidEnterRoom(Notification notification)
        {          
            Player player = (Player)notification.Object;
            if (player != null)
            {
                CheckForExit(player);
                CheckForCharacterInSameRoom(player);
                CheckForRoomBasedWorldChanges(player);
            }
        }
        public void PlayerDidPickupItem(Notification notification)
        {
            Player player = (Player)notification.Object;
            if (player != null)
            {
                CheckForItemBasedWorldChanges(player);
            }
        }

        public void MoveAndCheckForPlayer(Player player)
        {
            if (_securityGuard.Conscious)
            {
                _securityGuard.MoveToRandomRoom(_roomsToCheck);
                CheckForCharacterInSameRoom(player);
            }
        }

        public void CheckForExit(Player player)
        {
            if(player.CurrentRoom == _exit)
            {
                player.WinningMessage($"You arrived at the exit {player.CurrentRoom.Tag}.");
                NotificationCenter.Instance.PostNotification(new Notification("PlayerMadeExit", player));   
            }
        }

        public void CheckForRoomBasedWorldChanges(Player player)
        {
            IGameEvent wc = null;
            _worldChanges.TryGetValue(player.CurrentRoom, out wc);
            if (wc != null && !wc.Triggered)
            {
                player.CurrentObjective += 1;
                player.InfoMessage("New Objective. Type \"objective\" to get it.");
                wc.Execute(player);
            }
        }
        public void CheckForItemBasedWorldChanges(Player player)
        {
            IGameEvent wc = null;
            IItem item = player.Hands.GetItem("secretfile");
            if (item != null)
            {
                _worldChanges.TryGetValue(item, out wc);
                if (wc != null && !wc.Triggered)
                {
                    player.CurrentObjective += 1;
                    player.InfoMessage("New Objective. Type \"objective\" to get it.");
                    wc.Execute(player);
                }
            }
        }

        public bool CheckForCharacterInSameRoom(Player player)
        {
            bool inSameRoom = false;
            if (_securityGuard.CurrentRoom == player.CurrentRoom)
            {
                player.InfoMessage($"\n{_securityGuard.Name} is in the room! Throw stuff at him and knock him out. You have 15 seconds.");
                NotificationCenter.Instance.PostNotification(new Notification("PlayerInSameRoom", _securityGuard));
                inSameRoom = true;
            }
            return inSameRoom;
        }

        private Room CreateWorld()
        {
        //Floor 1 Rooms Creation
            Room parkingGarage = new Room("in the Parking Garage");
            Room hallway1 = new Room("in the side Hallway of the 1st Floor");
            Room mainLobby = new Room("in the Main Lobby");
            Room custodianRoom = new Room("in the Custodian Room");
            Room stairCase1 = new Room("in the staircase on the 1st floor");
            Room securityLounge = new Room();   // Neither accessible nor viewable to player

        //Floor 5 Rooms Creation
            Room stairCase5 = new Room("in the staircase on the 5th floor");
            Room hallway5 = new Room("in the hallway of the 5th floor");

            Room conferenceRoom = new Room("in the Conference Room");
            Room breakArea = new Room("in the Break Area");

            Room bathroom = new Room("in the Bathroom");
            Room executiveOfficeLobby = new Room("in the lobby of the executive office");
            Room executiveAssistantOffice = new Room("in the Office of the Executive Assistant");
            Room executiveOffice = new Room("in the Executive Office");
            Room secretVault = new Room("in the Secret Vault");

        // Outside Room Creation (Exit)
            Room outside = new Room("outside the building");
            
            // C#'s version of Javas AddAll.
            _rooms.AddRange(new List<Room> { parkingGarage, hallway1, mainLobby, custodianRoom, stairCase1,
            securityLounge, stairCase5, hallway5, conferenceRoom, breakArea,
                bathroom, executiveOfficeLobby, executiveAssistantOffice, executiveOffice, secretVault});
            
            // Floor 5 rooms for security guard to check
            _roomsToCheck.AddRange(new List<Room> { hallway1 });//hallway5, conferenceRoom, breakArea, bathroom, executiveOfficeLobby, executiveAssistantOffice, executiveOffice, secretVault});



        // Floor 1 Connection
            Door door = Door.Connect(parkingGarage, hallway1, "east", "west");
            door = Door.Connect(parkingGarage, hallway1, "east", "west");
            door = Door.Connect(hallway1, stairCase1, "north", "south");
            door = Door.Connect(hallway1, mainLobby, "east", "west");
            door = Door.Connect(stairCase1, custodianRoom, "west", "east");

        // Floor 5 Connection    
            door = Door.Connect(stairCase1, stairCase5, "up", "down");
                ILockable rl = LockableFacade.MakeLockable("Regular", "key1");
                door.Lockable = rl;
                door.Close();
                door.Lock();
                IItem key = door.Remove();
                custodianRoom.AddItem(key);
                

            door = Door.Connect(hallway5, conferenceRoom, "west", "east");
            door = Door.Connect(hallway5, breakArea, "east", "west");
            door = Door.Connect(hallway5, executiveOfficeLobby, "south", "north");
            door = Door.Connect(executiveOfficeLobby, bathroom, "west", "east");
            door = Door.Connect(executiveOfficeLobby, executiveAssistantOffice, "east", "west");
            door = Door.Connect(executiveOfficeLobby, executiveOffice, "south", "north");

            door = Door.Connect(executiveOffice, secretVault, "west", "east");

        // Floor 1 to 5, WorldChange. -  If player is able to make it in the stairCase, they can go up to 5th floor. 
            WorldChange wc = new WorldChange(stairCase1, stairCase5, hallway5, "south", "north", false);
            _worldChanges[stairCase1] = wc;
            wc = new WorldChange(_secretFile, outside, mainLobby, "north", "south", false);
            _worldChanges[_secretFile] = wc;


            IItem item = new Item("computer", 1f, 0.1f);
            IItem decorator = new Item("keyboard", 0.3f, 0.05f);
            item.AddDecorator(decorator);
            decorator = new Item("mouse", 0.1f, 0.03f);
            item.AddDecorator(decorator);  
            hallway1.AddItem(item);
            hallway1.AddItem(_secretFile);

            
            IItemContainer shoppingbag = new ItemContainer("shoppingbag", 9f, 10f, 1f);
            IItemContainer giftBox = new ItemContainer("giftbox",.9f,0.5f, 0.09f);
            IItem item2 = new Item("V3", 0.1f, 0.2f);

            shoppingbag.AddContainer(giftBox);
            shoppingbag.AddItem(item2);

            hallway1.AddContainer(shoppingbag);


            IItem item3 = new Item("key1", 0.1f, .2f);
            hallway1.AddItem(item3);

            _exit = outside;

            return parkingGarage;
        }
    }
}
/*
 * One event may spawn multiple events.
 * Player won't know when it reached exit, will only send notification each time he moves. Neither will game, will be gameWorld

// Visual Representation of 1st Floor
//      / & \ represent 'possible' entries
//______________________________________________________________
//                                  |  /  StairCase |Elevators  |
//                                  |   |________   |__  ___  __|
//          Parking Garage          |  Storage  |  /|           |
//                                  | & Janitor |    \          |
//                                  |    Room   |   |           |
//                                  |___________|   |           |
//                                  \   Hallway     |           |
//                                  /_______________|           |                       
//                                  |                           |                  
//                                  |        _______________    |    
//                                  |       |   MainLobby   |   | 
//                                  |        ---------------    | 
//______________   _   _____________|____________/ \_/ \________|
                                                  X   X
                            << Outside >>



// Visual Representation of 5th Floor 
//      / & \ represent 'possible' entries
//-----------------------------------------------
//                  |    ^^Stairs   |
//                  |               |
// Conference Room  |    Hallway    |   Break 
//                   \                  Room
//                   /             
//                  |               |
//                  |               |
//__________________|______ \_______|____________
//                  |   Executive   | Executive 
//   Bathroom        \   Office    /  Assistant          
//                  |    Lobby      |  Office
//__________________|_____/  \______|____________
//      |                                                                    
//Secret \             Executive 
// Vault|               Office                                                    
//______|________________________________________
*/
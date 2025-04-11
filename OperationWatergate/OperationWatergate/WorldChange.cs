using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class WorldChange : IGameEvent
    {
        bool _triggered;
        private ITrigger _trigger;
        private Room _inWorldRoom;
        private Room _outWorldRoom;
        private string _inOutDirection;
        private string _outInDirection;

        public bool Triggered {get {return _triggered;} }
        public WorldChange(ITrigger trigger, Room inWorldRoom, Room outWorldRoom, string inOutDirection, string outInDirection, bool _triggered)
        {
            _trigger = trigger;
            _inWorldRoom = inWorldRoom;
            _outWorldRoom = outWorldRoom;
            _inOutDirection = inOutDirection;
            _outInDirection = outInDirection;
            _triggered = false;
        }
        public void Execute(Player player)
        {
            Door door = Door.Connect(_inWorldRoom, _outWorldRoom, _inOutDirection, _outInDirection);
            player.InfoMessage("There is a new exit " + _inWorldRoom.Tag);
            _triggered = true;
        }
    }
}

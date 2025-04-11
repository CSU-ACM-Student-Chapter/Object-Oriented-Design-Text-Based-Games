using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OperationWatergate
{
    public class Character 
    {
        private string _name;
        private string _tag;
        private Room _currentRoom;
        private float _consciousness;
        private bool _conscious;
        private System.Timers.Timer _wakeUpTimer;

        public string Name { get { return _name; } }
        public string Tag { get { return _tag; } }
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }

        public float Consciousness { get { return _consciousness; } set { _consciousness = value; } }

        public bool Conscious { get { return _consciousness > 0; } }

        public Character(string name, string tag, Room room)
        {
            _name = name;
            _tag = tag;
            _currentRoom = room;
            _consciousness = 10;
            _conscious = true;
            _wakeUpTimer = new System.Timers.Timer(45000);
            _wakeUpTimer.AutoReset = false;
            NotificationCenter.Instance.AddObserver("PlayerInSameRoom", PlayerInSameRoom);
        }

        public void PlayerInSameRoom(Notification notification)
        {
                //Set Timer()
                //PressAlarm();
                // On alart check alarm. 
        }

        public void MoveToRandomRoom(List<Room> rooms)
        {
            if (rooms == null || rooms.Count <= 0)
            {
                return;
            }

            Random random = new Random();
            int index = random.Next(0,rooms.Count);
            this.CurrentRoom = rooms[index];
        }

        public void ReceiveHit(float itemWeight)
        {
            _consciousness -= itemWeight;
            if (!Conscious)
            {
                NotificationCenter.Instance.PostNotification(new Notification("KnockedDownPlayer", this));
                TryToWakeUp();
            }
        }

        public void TryToWakeUp()
        {
            _wakeUpTimer.Start();
            _wakeUpTimer.Elapsed += WakeUp;
        }

        public void WakeUp(Object source, ElapsedEventArgs e)
        {
            _wakeUpTimer.Stop();
            _consciousness += 6;
        }
    }
}

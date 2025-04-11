using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace OperationWatergate
{
    public class GameClock
    {
        private System.Timers.Timer timer;
        private System.Timers.Timer _securityGuardMovementTimer;
        private int _timeInGame;

        public int TimeInGame { get { return _timeInGame; } }

        public GameClock(int interval)
        {
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;

            _securityGuardMovementTimer = new System.Timers.Timer(20000);
            _securityGuardMovementTimer.Elapsed += OnSecurityGuardMove;
            _securityGuardMovementTimer.AutoReset = true;
            _securityGuardMovementTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            NotificationCenter.Instance.PostNotification(new Notification("GameClockTimer", this));
        }

        private void OnSecurityGuardMove(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("OnSecurityGuardMove Timer was set");
            NotificationCenter.Instance.PostNotification(new Notification("SecurityGuardRoomCheck", this));
        }
    }
}
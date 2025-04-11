using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class Game
    {
        private Player _player;
        private Parser _parser;
        private bool _playing;
        private GameClock _gameClock;

        public Game()
        {
            _playing = false;
            _parser = new Parser(new CommandWords());
            _player = new Player(GameWorld.Instance.Entrance);
            _gameClock = new GameClock(600000); //600,000 milliseconds = 600 seconds = 10 minutes 
            NotificationCenter.Instance.AddObserver("GameClockTimer", GameClockTimer);
            NotificationCenter.Instance.AddObserver("PlayerMadeExit", PlayerMadeExit);
            NotificationCenter.Instance.AddObserver("SecurityGuardRoomCheck", SecurityGuardRoomCheck);
        }

        public void GameClockTimer(Notification notification)
        {
            _playing = false;
            _player.ErrorMessage("\nTask failed: GameOver");
            _player.NormalMessage($"Your time is up. The entire building is surrounded.\n " +
                $"You completed a total of {_player.CurrentObjective-1}  objectives out of {_player.TotalObjectivesToComplete}");
            NotificationCenter.Instance.RemoveObserver("GameClockTimer", GameClockTimer);
        }

        public void SecurityGuardRoomCheck(Notification notification)
        {
            //Console.WriteLine("SecurityGuardRoomCheck in Game was notified");
            GameWorld.Instance.MoveAndCheckForPlayer(_player);
        }

        public void PlayerMadeExit(Notification notification)
        {
            _playing = false;
            _player.WinningMessage("Congratulations. You made it to the exit. You win");
            NotificationCenter.Instance.RemoveObserver("PlayerMadeExit", PlayerMadeExit);
        }

        public void Play()
        {
            while(_playing)
            {
                Console.Write("\n>");
                string inputLine = Console.ReadLine(); 
                if (!_playing)
                {
                    break;
                }
                Command command = _parser.ParseCommand(inputLine);
                if (command == null)
                {
                    _player.ErrorMessage("I don't understand...");
                }
                else
                {
                    if (command.Execute(_player))
                    {
                        _playing = false;
                    }
                }
            }
        }

        public void Start()
        {
            _playing = true;
            _player.InfoMessage(Welcome());
            _player.GetObjective();
        }

        public void End()
        {           
            _playing = false;
            _player.InfoMessage(GoodBye());
        }

        public string Welcome()
        {
            return "Welcome to Operation Watergate2.0. Joe needs your help!\n" +
                "-\tType 'help' if you need help.\n\n" +
            $"You are {_player.CurrentRoom.Tag}" +
            _player.CurrentRoom.Description();
        }

        public string GoodBye()
        {
            return "Thank You for playing, Goodbye.";
        }
    }
}

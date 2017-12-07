using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesLib
{
    public class GameXO : IGame
    {
        private int[,] _field = null;
        private string _playerXId;
        private string _playerOId;
        private bool test = false;

        public int Id
        {
            private set;
            get;
        }

        public string Name
        {
            private set;
            get;
        }

        public string Winner
        {
            private set;
            get;
        }

        public string WhoseNextMove
        {
            private set;
            get;
        }

        public GameState CurrentState
        {
            private set;
            get;
        }
        public string PlayerNameX { get => _playerXId; private set => _playerXId = value; }
        public string PlayerNameO { get => _playerOId; private set => _playerOId = value; }

        public string GameOwnerName { get => PlayerNameX; }
        public GameXO()
        {
            Id = GetHashCode();
            _field = new int[3,3];
            Name = "XO";
            CurrentState = GameState.WaitingForPlayers;
        }

        public GameXO(string name) : this()
        {
            PlayerNameX = name;
        }

    public void Action(string uId, string action)
        {
            if (CurrentState != GameState.InProgress)
                return;
            if (uId != WhoseNextMove && !test)
                throw new Exception();

            int i, j;           

            try
            {
                string[] pos = action.Split(',');
                i = Convert.ToInt32(pos[0]);
                j = Convert.ToInt32(pos[1]);
            }
            catch (Exception)
            {

                throw;
            }
           
            if(_field[i,j] != 0) 
                throw new Exception();

            if (uId == PlayerNameX)
            {
                _field[i, j] = 1; // X
                WhoseNextMove = PlayerNameO;
            }
            else if (uId == PlayerNameO)
            {
                _field[i, j] = 2; // O
                WhoseNextMove = PlayerNameX;
            }

            ChechGameEnd();
            ChechGameDraw();
        }

        private void ChechGameDraw()
        {
            foreach (int item in _field)
            {
                if (item == 0)
                    return;
            }
            CurrentState = GameState.EndWithDraw;            
        }

        private void ChechGameEnd()
        {
            // оптимизировать
            for (int i = 0; i < 3; i++)
            {
                if(_field[i,0] == 1 && _field[i, 1] == 1 && _field[i, 2] == 1)
                {
                    Winner = PlayerNameX;
                    CurrentState = GameState.EndWithWinner;
                    return;
                }
                if (_field[0, i] == 1 && _field[1, i] == 1 && _field[2, i] == 1)
                {
                    Winner = PlayerNameX;
                    CurrentState = GameState.EndWithWinner;
                    return;
                }

                if (_field[i, 0] == 2 && _field[i, 1] == 2 && _field[i, 2] == 2)
                {
                    Winner = PlayerNameO;
                    CurrentState = GameState.EndWithWinner;
                    return;
                }
                if (_field[0, i] == 2 && _field[1, i] == 2 && _field[2, i] == 2)
                {
                    Winner = PlayerNameO;
                    CurrentState = GameState.EndWithWinner;
                    return;
                }
            }
            if(_field[0, 0] == 2 && _field[1, 1] == 2 && _field[2, 2] == 2)
            {
                Winner = PlayerNameO;
                CurrentState = GameState.EndWithWinner;
                return;
            }
            if (_field[0, 2] == 2 && _field[1, 1] == 2 && _field[2, 0] == 2)
            {
                Winner = PlayerNameO;
                CurrentState = GameState.EndWithWinner;
                return;
            }
            if (_field[0, 0] == 1 && _field[1, 1] == 1 && _field[2, 2] == 1)
            {
                Winner = PlayerNameX;
                CurrentState = GameState.EndWithWinner;
                return;
            }
            if (_field[0, 2] == 1 && _field[1, 1] == 1 && _field[2, 0] == 1)
            {
                Winner = PlayerNameX;
                CurrentState = GameState.EndWithWinner;
                return;
            }
        }

        public string GetField()
        {
            string res = JsonConvert.SerializeObject(_field);
            return res;
        }

        public object GetResources()
        {
            return null;
        }

        public bool StartGame(params object[] p)
        {
            bool res = true;

            try
            {
                Id = (int)p[0];
                PlayerNameX = (string)p[1];
                PlayerNameO = (string)p[2];

            }
            finally
            {
                res = false;
            }

            WhoseNextMove = PlayerNameX;

            CurrentState = GameState.InProgress;

            return res;
        }

        public string WhoWin()
        {
            return Winner;
        }

        public void AddPlayer(string userId)
        {
            if (PlayerNameX != null && PlayerNameO != null)
            {
                throw new Exception();
            }

            if (PlayerNameX == null)
                PlayerNameX = userId;
            else
                PlayerNameO = userId;
        }

        public void StartGame()
        {
            if (PlayerNameX != null && PlayerNameO != null)
            {
                WhoseNextMove = PlayerNameX;
                CurrentState = GameState.InProgress;
            }
        }

        public void AddPlayer(string userId, int order)
        {
            throw new NotImplementedException();
        }

        public void SetTest()
        {
            test = true;
        }

        public void SetReal()
        {
            test = false;
        }

        public bool IsUserInGame(string usrName)
        {
            return PlayerNameX == usrName || PlayerNameO == usrName;
        }
    }
}

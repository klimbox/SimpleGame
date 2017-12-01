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
        private int _gameId;
        private int[,] _field = null;
        private string _playerXId;
        private string _playerOId;
        private bool _isFiniShed; //0 - NULL 1-X 2-O 
        private string _gameName = null;
        private string _winner;
        private string _nextPlayerTurn;

        public GameXO()
        {
            _field = new int[3,3];
            _isFiniShed = false;
            _gameName = "XO";
        }

        public void Action(string uId, string action)
        {
            string[] pos;
            int i, j;           

            try
            {
                pos = action.Split(',');
                i = Convert.ToInt32(pos[0]);
                j = Convert.ToInt32(pos[1]);
            }
            catch (Exception)
            {

                throw;
            }
           
            if(_field[i,j] != 0) 
                throw new Exception();

            if (uId == _playerXId)
            {
                _field[i, j] = 1; // X
                _nextPlayerTurn = _playerOId;
            }
            else if (uId == _playerOId)
            {
                _field[i, j] = 2; // O
                _nextPlayerTurn = _playerXId;
            }

            ChechGameEnd();
        }

        private void ChechGameEnd()
        {
            // оптимизировать
            for (int i = 0; i < 3; i++)
            {
                if(_field[i,0] == 1 && _field[i, 1] == 1 && _field[i, 2] == 1)
                {
                    _winner = _playerXId;
                    _isFiniShed = true;
                    return;
                }
                if (_field[0, i] == 1 && _field[1, i] == 1 && _field[2, i] == 1)
                {
                    _winner = _playerXId;
                    _isFiniShed = true;
                    return;
                }

                if (_field[i, 0] == 2 && _field[i, 1] == 2 && _field[i, 2] == 2)
                {
                    _winner = _playerOId;
                    _isFiniShed = true;
                    return;
                }
                if (_field[0, i] == 2 && _field[1, i] == 2 && _field[2, i] == 2)
                {
                    _winner = _playerOId;
                    _isFiniShed = true;
                    return;
                }
            }
            if(_field[0, 0] == 2 && _field[1, 1] == 2 && _field[2, 2] == 2)
            {
                _winner = _playerOId;
                _isFiniShed = true;
                return;
            }
            if (_field[0, 2] == 2 && _field[1, 1] == 2 && _field[2, 0] == 2)
            {
                _winner = _playerOId;
                _isFiniShed = true;
                return;
            }
            if (_field[0, 0] == 1 && _field[1, 1] == 1 && _field[2, 2] == 1)
            {
                _winner = _playerXId;
                _isFiniShed = true;
                return;
            }
            if (_field[0, 2] == 1 && _field[1, 1] == 1 && _field[2, 0] == 1)
            {
                _winner = _playerXId;
                _isFiniShed = true;
                return;
            }
        }

        public string GetField()
        {
            string res = JsonConvert.SerializeObject(_field);
            return res;
        }

        public string GetGame()
        {
            return _gameName;
        }

        public object GetResources()
        {
            return null;
        }

        public bool IsFinished()
        {
            return _isFiniShed;
        }

        public bool IsYourGame(int id)
        {
            bool res = false;

            if (_gameId == id)
            {
                res = true;
            }

            return res;
        }

        public bool StartGame(params object[] p)
        {
            bool res = true;

            try
            {
                _gameId = (int)p[0];
                _playerXId = (string)p[1];
                _playerOId = (string)p[2];

            }
            finally
            {
                res = false;
            }

            _nextPlayerTurn = _playerXId;

            return res;
        }

        public string WhoWin()
        {
            return _winner;
        }

        public string WhoNextTurn()
        {
            return _nextPlayerTurn;
        }
    }
}

using SimpleGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleGame.Services
{
    public class UserManager
    {
        private List<User> _usrLst;

        public UserManager()
        {
            _usrLst = new List<User>();
        }

        internal void AddUser(string name, string connectionId, bool isInGame = false, int gameId = 0)
        {
            if (_usrLst.Any(u => u.Name == name))
            {
                var usr = _usrLst.Find(u => u.Name == name);
                usr.ConnectionId = connectionId;
                usr.IsInGame = isInGame;
                usr.GameId = gameId;
                return;
            }
            _usrLst.Add(new User(name, connectionId, isInGame, gameId));
        }

        internal List<User> GetAvailableUsers()
        {
            return _usrLst.FindAll(u => u.IsInGame == false);
        }

        internal List<User> GetUsersInGame()
        {
            return _usrLst.FindAll(u => u.IsInGame == true);
        }

        internal User GetUserByName(string usrName)
        {
            return _usrLst.FirstOrDefault(u => u.Name == usrName);
        }
    }
}
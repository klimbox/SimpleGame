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

        internal void AddUser(string name, string connectionId, bool isInGame = false)
        {
            if (_usrLst.Any(u => u.Name == name))
            {
                var usr = _usrLst.Find(u => u.Name == name);
                usr.ConnectionId = connectionId;
                usr.IsInGame = isInGame;
                return;
            }
            _usrLst.Add(new User(name, connectionId, isInGame));
        }
    }
}
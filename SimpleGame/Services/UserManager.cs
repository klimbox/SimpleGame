using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
            if (name != "")
            {
                _usrLst.Add(new User(name, connectionId, isInGame, gameId));
            }
        }

        internal bool IsNewUser(string name)
        {
            return !_usrLst.Any(u => u.Name == name);
        }

        internal List<User> GetAvailableUsers()
        {
            return _usrLst.FindAll(u => u.IsInGame == false);
        }

        internal void UpdateUserInfo(string name, string connectionId)
        {
            User user = _usrLst.FirstOrDefault(u => u.Name == name);
            user.ConnectionId = connectionId;
        }
        internal void UpdateUserInfo(string name, string connectionId, bool isInGame, int gameId)
        {
            if (IsNewUser(name))
            {
                AddUser(name, connectionId, isInGame, gameId);
                return;
            }
            User user = _usrLst.FirstOrDefault(u => u.Name == name);
            user.ConnectionId = connectionId;
            user.IsInGame = isInGame;
            user.GameId = gameId;
        }

        internal List<User> GetAllUsersInGame()
        {
            return _usrLst.FindAll(u => u.IsInGame == true);
        }

        internal User GetUserByName(string usrName)
        {
            return _usrLst.FirstOrDefault(u => u.Name == usrName);
        }

        internal List<User> GetUsersInGame(int gameId)
        {
            return _usrLst.FindAll(u => u.GameId == gameId);
        }

        internal void UpdateRating(int gameId, string winnerName)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);

            var users = GetUsersInGame(gameId);
            for (int i = 0; i < users.Count; i++)
            {
                ApplicationUser user = manager.FindByName(users[i].Name);
                if (users[i].Name == winnerName)
                {
                    user.Rating += 1;
                    continue;
                }
                user.Rating -= 1;
            }
            var ctx = store.Context;
            ctx.SaveChanges();
        }
    }
}
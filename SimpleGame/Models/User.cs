using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleGame.Models
{
    public class User
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public bool IsInGame { get; set; }

        public User(string name, string connectId, bool isInGame = false)
        {
            Name = name;
            ConnectionId = connectId;
            IsInGame = isInGame;
        }
    }
}
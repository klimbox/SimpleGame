using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using SimpleGame.Models;
using GamesLib;
using System.Threading.Tasks;
using SimpleGame.Services;

namespace SimpleGame.Hubs
{
    public class GameHub : Hub
    {
        //static List<User> Users = new List<User>();
        static IGame game = new GameXO();
        private static UserManager _usrMngr = new UserManager();

        // 
        public void JoinHub()
         {
            if (Context.User.Identity.IsAuthenticated)
            {
                _usrMngr.AddUser(Context.User.Identity.Name, Context.ConnectionId);

            }
        }

        public void ConnectToGame()
        {
            _usrMngr.AddUser(Context.User.Identity.Name, Context.ConnectionId, true);
            //create game instance
        }

        // Отправка сообщений
        public void Send(string name, string message)
        {
            if (game.IsFinished())
            {
                Clients.All.showMessage("Игра закончена, победил игрок: " + game.WhoWin());
                return;
            }

            if (game.WhoNextTurn() != null && name != game.WhoNextTurn())
            {
                // Clients.AllExcept(Users.Find(x => x.Name == game.WhoNextTurn()).ConnectionId).showMessage("Сейчас ходит игрок: " + game.WhoNextTurn());
                return;
            }

            game.Action(name, message);
            if (game.IsFinished())
            {
                Clients.All.sendField(game.GetField());
                Clients.All.showMessage("Игра закончена, победил игрок: " + game.WhoWin());
                game = new GameXO();
                //return;
            }
            Clients.All.sendField(game.GetField());
        }

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            _usrMngr.AddUser(Context.User.Identity.Name, Context.ConnectionId, true);

            //var id = Context.ConnectionId;

            //if (!Users.Any(x => x.ConnectionId == id || x.Name == userName))
            //{
            //    Users.Add(new User { ConnectionId = id, Name = userName });

            //    // Посылаем сообщение текущему пользователю
            //    Clients.Caller.onConnected(id, userName, Users);

            //    // Посылаем сообщение всем пользователям, кроме текущего
            //    Clients.AllExcept(id).onNewUserConnected(id, userName);
            //}
            //else
            //{
            //    Clients.Caller.showMessage("Вы уже в игре");
            //}

            //if (game != null && game.IsYourGame(id))
            //{
            //    Clients.Client(id).sendField(game.GetField());
            //}
        }

        //public void Invite(string userId)
        //{
        //    User caller = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        //    string gameName = "крестики-нолики";
        //    Clients.Client(userId).showInvitation(gameName, caller);
        //}

        //public void InvitationConfirm(bool isConfirm, User caller)
        //{
        //    if (isConfirm)
        //    {
        //        game.StartGame(1, caller.Name, Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId).Name);
        //        Clients.Client(caller.ConnectionId).showMessage("Ваше предложение принято!");
        //    }
        //    else
        //    {
        //        Clients.Client(caller.ConnectionId).showMessage("Игрок отклонил ваше предложение");
        //    }
        //}
        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            //var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            //if (item != null)
            //{
            //    Users.Remove(item);
            //    var id = Context.ConnectionId;
            //    Clients.All.onUserDisconnected(id, item.Name);
            //}

            return base.OnDisconnected(stopCalled);
        }
    }
}
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
        //static IGame game = new GameXO();

        private static UserManager _usrMngr = new UserManager();
        private static GameFactory _gameFactory = new GameFactory();

        // 
        public void JoinHub()
         {
            if (Context.User.Identity.IsAuthenticated)
            {
                _usrMngr.AddUser(Context.User.Identity.Name, Context.ConnectionId);
            }
            Clients.Caller.GetAvailableGames(_gameFactory.GetAvailableGames());

            //send to users in game
            var userIds = _usrMngr.GetUsersInGame().Select(u => u.ConnectionId).ToList();
            Clients.Clients(userIds).ShowUsers(_usrMngr.GetAvailableUsers());
        }


        public void StartNewGame(string gameName)
        {
            string usrName = Context.User.Identity.Name;
            //create game instance
            int gameId = _gameFactory.StartNewGame(gameName, usrName);
            _usrMngr.AddUser(usrName, Context.ConnectionId, true, gameId);

            var userIds = _usrMngr.GetAvailableUsers().Select(u => u.ConnectionId).ToList();
            Clients.Clients(userIds).GetAvailableGames(_gameFactory.GetAvailableGames());

            Clients.Caller.ShowUsers(_usrMngr.GetAvailableUsers());
        }

        public void Invite(string usrName, string gameName)
        {
            User caller = _usrMngr.GetUserByName(Context.User.Identity.Name);

            IGame currGame = _gameFactory.GetAvailableGames().Find(g => g.GameOwnerName == caller.Name);

            Clients.Client(_usrMngr.GetUserByName(usrName).ConnectionId).ShowInvitation(currGame);
        }
        public void InvitationConfirm(bool isConfirm, string callerName)
        {
            Clients.Client(_usrMngr.GetUserByName(callerName).ConnectionId).ShowMessage("testing");
        }
        public void InvitationConfirm(bool isConfirm, IGame currGame)
        {
            User caller = _usrMngr.GetUserByName(currGame.GameOwnerName);

            if (isConfirm)
            {
                _gameFactory.AddPlayerToGame(currGame.Id, Context.User.Identity.Name);
                _gameFactory.StartGame(currGame.Id);

                Clients.Client(caller.ConnectionId).ShowMessage("Ваше предложение принято!");
            }
            else
            {
                Clients.Client(caller.ConnectionId).ShowMessage("Игрок отклонил ваше предложение");
            }
        }



        // Отправка сообщений
        public void Send(string name, string message)
        {
            //if (game.IsFinished())
            //{
            //    Clients.All.showMessage("Игра закончена, победил игрок: " + game.WhoWin());
            //    return;
            //}

            //if (game.WhoNextTurn() != null && name != game.WhoNextTurn())
            //{
            //    // Clients.AllExcept(Users.Find(x => x.Name == game.WhoNextTurn()).ConnectionId).showMessage("Сейчас ходит игрок: " + game.WhoNextTurn());
            //    return;
            //}

            //game.Action(name, message);
            //if (game.IsFinished())
            //{
            //    Clients.All.sendField(game.GetField());
            //    Clients.All.showMessage("Игра закончена, победил игрок: " + game.WhoWin());
            //    game = new GameXO();
            //    //return;
            //}
            //Clients.All.sendField(game.GetField());
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
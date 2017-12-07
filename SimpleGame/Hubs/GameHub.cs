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
            var userIds = _usrMngr.GetAllUsersInGame().Select(u => u.ConnectionId).ToList();
            Clients.Clients(userIds).ShowUsers(_usrMngr.GetAvailableUsers());
        }


        public void StartNewGame(string gameName)
        {
            string usrName = Context.User.Identity.Name;
            //try
            //{
            //    if (_usrMngr.GetUserByName(usrName).IsInGame)
            //    {
            //        return;
            //    }
            //}
            //catch (System.Exception)
            //{
            //}
            //create game instance
            int gameId = _gameFactory.StartNewGame(gameName, usrName);
            _usrMngr.AddUser(usrName, Context.ConnectionId, true, gameId);

            var userIds = _usrMngr.GetAvailableUsers().Select(u => u.ConnectionId).ToList();
            Clients.Clients(userIds).GetAvailableGames(_gameFactory.GetAvailableGames());

            Clients.Caller.ShowUsers(_usrMngr.GetAvailableUsers());
        }

        public void Invite(string usrName)
        {
            User caller = _usrMngr.GetUserByName(Context.User.Identity.Name);

            IGame currGame = _gameFactory.GetAvailableGames().Find(g => g.Id == caller.GameId);

            Clients.Client(_usrMngr.GetUserByName(usrName).ConnectionId).ShowInvitation(currGame);
        }

        public void InvitationConfirm(bool isConfirm, int gameId, string callerName)
        {
            User caller = _usrMngr.GetUserByName(callerName);

            if (isConfirm)
            {
                var player = _usrMngr.GetUserByName(Context.User.Identity.Name);
                player.GameId = gameId;
                player.IsInGame = true;
                _gameFactory.AddPlayerToGame(gameId, Context.User.Identity.Name);
                _gameFactory.StartGame(gameId);

                Clients.Client(player.ConnectionId).RedirectToGame();
                Clients.Client(caller.ConnectionId).ShowMessage("Ваше предложение принято!");
            }
            else
            {
                Clients.Client(caller.ConnectionId).ShowMessage("Игрок отклонил ваше предложение");
            }
        }

        public void DoAction(string usrName, string action)
        {
            var gameId = _usrMngr.GetUserByName(Context.User.Identity.Name).GameId;
            var userIds = _usrMngr.GetUsersInGame(gameId)
                    .Select(u => u.ConnectionId).ToList();

            try
            {
                _gameFactory.GameAction(gameId, usrName, action);

                Clients.Clients(userIds).sendField(_gameFactory.GetGameField(gameId));

                switch (_gameFactory.GetGameState(gameId))
                {
                    case GameState.EndWithWinner:
                        Clients.Clients(userIds).showMessageAndRedirect("Игра закончена, победил игрок: " + _gameFactory.GetGameWinner(gameId));
                        Reset(gameId);
                        return;
                    case GameState.EndWithDraw:
                        Clients.Clients(userIds).showMessageAndRedirect("Ничья!");
                        Reset(gameId);
                        return;
                }
            }
            catch (System.Exception)
            {
                return;
            }


        }

        private void Reset(int gameId)
        {
            List<User> players = _usrMngr.GetUsersInGame(gameId);
            for (int i = 0; i < players.Count; i++)
            {
                players[i].IsInGame = false;
                players[i].GameId = 0;
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
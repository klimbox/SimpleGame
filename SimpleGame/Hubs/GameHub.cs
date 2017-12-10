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
        private static UserManager _usrMngr = new UserManager();
        private static GameFactory _gameFactory = new GameFactory();

        // 
        public void JoinHub()
        {
            if (_usrMngr.IsNewUser(Context.User.Identity.Name))
            {
                _usrMngr.AddUser(Context.User.Identity.Name, Context.ConnectionId);
            }
            else
            {
                _usrMngr.UpdateUserInfo(Context.User.Identity.Name, Context.ConnectionId);
            }

            Clients.Caller.GetAvailableGames(_gameFactory.GetAvailableGames());

            //send to users in game
            var userIds = _usrMngr.GetAllUsersInGame().Select(u => u.ConnectionId).ToList();
            Clients.Clients(userIds).ShowUsers(_usrMngr.GetAvailableUsers());
        }


        public void StartNewGame(string gameName)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                string usrName = Context.User.Identity.Name;
                try
                {
                    if (_usrMngr.GetUserByName(usrName).IsInGame)
                    {
                        _usrMngr.UpdateUserInfo(Context.User.Identity.Name, Context.ConnectionId);
                        return;
                    }
                }
                catch (System.Exception)
                {
                }
                //create game instance
                int gameId = _gameFactory.StartNewGame(gameName, usrName);
                _usrMngr.UpdateUserInfo(usrName, Context.ConnectionId, true, gameId);

                var userIds = _usrMngr.GetAvailableUsers().Select(u => u.ConnectionId).ToList();
                Clients.Clients(userIds).GetAvailableGames(_gameFactory.GetAvailableGames());

                Clients.Caller.ShowUsers(_usrMngr.GetAvailableUsers()); 
            }
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
                AddPlayerToGame(gameId);
                Clients.Client(caller.ConnectionId).DisableInvitation(); // hide invitation buttons 
                Clients.Client(caller.ConnectionId).ShowMessage("Ваше предложение принято!");
            }
            else
            {
                Clients.Client(caller.ConnectionId).ShowMessage("Игрок отклонил ваше предложение");
            }
        }

        public void JoinToGame(int gameId, string gameInitiator)
        {
            AddPlayerToGame(gameId);
            Clients.Client(_usrMngr.GetUserByName(gameInitiator).ConnectionId).ShowMessage("К игре присоединился игрок! Ваш ход.");
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
                        _usrMngr.UpdateRating(gameId, _gameFactory.GetGameWinner(gameId));
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
            _gameFactory.Destroy(gameId);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }


        private void AddPlayerToGame(int gameId)
        {
            var player = _usrMngr.GetUserByName(Context.User.Identity.Name);
            player.GameId = gameId;
            player.IsInGame = true;
            _gameFactory.AddPlayerToGame(gameId, Context.User.Identity.Name);
            _gameFactory.StartGame(gameId);

            Clients.Client(player.ConnectionId).RedirectToGame();
        }
    }
}
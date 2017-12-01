using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using SimpleGame.Models;
using GamesLib;

namespace SimpleGame.Hubs
{
    public class GameHub : Hub
    {
        static List<User> Users = new List<User>();
        static IGame game = new GameXO();
        // Отправка сообщений
        public void Send(string name, string message)
        {
            if (game.IsFinished())
            {
                Clients.All.showMessage("Игра закончена, победил игрок: " + game.WhoWin());
                return;
            }

            if (name != game.WhoNextTurn())
            {
                Clients.All.showMessage("Следующим ходит игрок: " + game.WhoNextTurn());
                return;
            }
            
            game.Action(name, message);
            if(game.IsFinished())
            {
                Clients.All.sendField(game.GetField());
                Clients.All.showMessage("Игра закончена, победил игрок: " + game.WhoWin());
                return;
            }
            Clients.All.sendField(game.GetField());
        }

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;

            if (!Users.Any(x => x.ConnectionId == id))
            {
                Users.Add(new User { ConnectionId = id, Name = userName });

                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(id, userName, Users);

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            }

            if (Users.Count == 2)
            {
                game.StartGame(1, Users.First().Name, userName);
            }
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}
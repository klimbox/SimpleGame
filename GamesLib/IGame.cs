using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesLib
{
    public interface IGame
    {
        bool StartGame(params object[] p);
        string GetGame();
        bool IsYourGame(int gameId);
        bool IsYourGame(string playerId);
        void Action(string uId, string action);
        bool IsFinished(); // игра завершена?
        string WhoWin(); // id победившего игрока
        string GetField(); // int[][] 0 - NULL 1-X 2-O 
        object GetResources(); // ??
        string WhoNextTurn();
    }
}

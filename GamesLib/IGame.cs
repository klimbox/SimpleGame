using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesLib
{
    public enum GameState {WaitingForPlayers, InProgress, EndWithWinner,  EndWithDraw}

    public interface IGame
    {
        int Id { get; }
        string Name { get; }
        string Winner { get; }
        string WhoseNextMove { get; }
        GameState CurrentState { get; }
        // -- metdods
        void AddPlayer(string userId);
        void AddPlayer(string userId, int order);
        void StartGame();
        void Action(string userId, string action);
        string GetField(); // int[][] 0 - NULL 1-X 2-O 
        object GetResources(); // ??
    }
}

﻿using GamesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Services
{
    public class GameFactory
    {
        public List<string> GameTypes = new List<string>();
        List<IGame> CurrentGames = new List<IGame>();
        public GameFactory()
        {

        }
        /// <summary>
        /// Start new game
        /// </summary>
        /// <param name="gameName">type of game</param>
        public int StartNewGame(string gameName, string usrName)
        {
            if (CurrentGames.Any(g => g.IsUserInGame(usrName)))
            {
                return CurrentGames.Find(g => g.IsUserInGame(usrName)).Id;
            }
            IGame game;
            switch (gameName)
            {
                case "XO":
                    game = new GameXO(usrName);
                    CurrentGames.Add(game);
                    break;
                default:
                    game = new GameXO();
                    break;
            }

            return game.Id;
        }
        /// <summary>
        /// Add new player to game
        /// </summary>
        /// <param name="gameId">Game Id</param>
        /// <param name="player">Player Id</param>
        public void AddPlayerToGame(int gameId, string player)
        {
            CurrentGames.Find(g => g.Id == gameId).AddPlayer(player);
        }
        /// <summary>
        /// Start game
        /// </summary>
        /// <param name="id">Game Id</param>
        public void StartGame(int id)
        {
            CurrentGames.Find(g => g.Id == id).StartGame();
        }
        /// <summary>
        /// Make action in the game
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <param name="player">Player Id</param>
        /// <param name="action">Action</param>
        public void GameAction(int id, string player, string action)
        {
            CurrentGames.Find(g => g.Id == id).Action(player, action);
        }
        /// <summary>
        /// Get game field in JSON
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <returns></returns>
        public string GetGameField(int id)
        {
            return CurrentGames.Find(g => g.Id == id).GetField();
        }
        /// <summary>
        /// Get Game State 
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <returns></returns>
        public GameState GetGameState(int id)
        {
            return CurrentGames.Find(g => g.Id == id).CurrentState;
        }
        /// <summary>
        /// Get Winner of game 
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <returns></returns>
        public string GetGameWinner(int id)
        {
            string res;

            res = CurrentGames.Find(g => g.Id == id).Winner;

            return res;
        }

        public List<IGame> GetAvailableGames()
        {
            return CurrentGames.FindAll(g => g.CurrentState == GameState.WaitingForPlayers);
        }

        internal void Destroy(int gameId)
        {
            CurrentGames.Remove(CurrentGames.FirstOrDefault(g => g.Id == gameId));
        }
    }
}

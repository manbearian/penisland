﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenIsland
{
    class TttGame
    {
        public int PlayerCount { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int CurrentPlayer { get; private set; }
        public bool GameOver { get; private set; }
        public int Winner { get; private set; }

        private readonly int[,] recordedMoves;

        public TttGame(int playerCount, int boardWidth, int boardHeight)
        {
            PlayerCount = playerCount;
            CurrentPlayer = Player.FirstPlayer;

            Width = boardWidth;
            Height = boardHeight;

            recordedMoves = new int[Width, Height];
        }

        public int GetMove(int x, int y)
        {
            return recordedMoves[x, y];
        }

        public void RecordMove(int x, int y, int player)
        {
            System.Diagnostics.Debug.Assert(player >= Player.FirstPlayer && player < PlayerCount);

            recordedMoves[x, y] = player;

            // check for game over
            bool winner = false;
            bool possibleCatsGame = false;

            winner = true;
            for (int i = 0; i < Width; ++i)
            {
                var check = GetMove(i, y);
                if (check != player)
                {
                    possibleCatsGame = (check != Player.Invalid);
                    winner = false;
                    break;
                }
            }

            if (winner)
            {
                EndGame(player);
            }

            winner = true;
            for (int j = 0; j < Height; ++j)
            {
                var check = GetMove(x, y);
                if (check != player)
                {
                    possibleCatsGame = (check != Player.Invalid);
                    winner = false;
                    break;
                }
            }

            if (winner)
            {
                EndGame(player);
            }

            if (possibleCatsGame && CheckCatsGame())
            {
                EndGame(Player.Invalid);
            }

            EndTurn();
        }

        bool CheckCatsGame(int [] plays)
        {
            int seenPlayer = Player.Invalid;

            for (int i = 0; i < plays.Length; ++i)
            {
                int checkPlayer = plays[i];
                if (checkPlayer != Player.Invalid)
                {
                    if (seenPlayer == Player.Invalid)
                    {
                        seenPlayer = checkPlayer;
                    }
                    else if (checkPlayer != seenPlayer)
                    {
                        return true;
                    }
                }
            }

            // this paht is still open
            return false;
        }

        bool CheckCatsGame()
        {
            System.Diagnostics.Debug.Assert(Width == Height, "NYI: non-square boards!");
            
            for (int i = 0; i < Width; ++i)
            {
                int[] hPlays = new int[Height];
                int[] vPlays = new int[Height];

                for (int j = 0; j < Height; ++j)
                {
                    hPlays[i] = GetMove(i, j);
                    vPlays[i] = GetMove(j, i);
                }

                // there's still a way someone can win
                if (!CheckCatsGame(hPlays))
                    return false;
                if (!CheckCatsGame(vPlays))
                    return false;
            }

            int[] x1Plays = new int[Height];
            int[] x2Plays = new int[Height];

            for (int i = 0; i < Width; ++i)
            {
                x1Plays[i] = GetMove(i, i);
                x2Plays[i] = GetMove(Width - 1 - i, i);
            }

            if (!CheckCatsGame(x1Plays))
                return false;
            if (!CheckCatsGame(x2Plays))
                return false;

            // no moves left, cat's game
            return true;
        }


        void EndTurn()
        {
            CurrentPlayer++;
            if (CurrentPlayer >= PlayerCount)
            {
                CurrentPlayer = 0;
            }
        }

        void EndGame(int winner)
        {
            Winner = winner;
            CurrentPlayer = Player.Invalid;
            GameOver = true;
        }

        public void CheckWinnerOrNoWinner()
        {
            // check verticals
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height; ++j)
                {
                    var player = GetMove(i, j);
                }
            }
        }
    }
}

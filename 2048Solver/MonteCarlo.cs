using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _2048Solver.Game;

namespace _2048Solver
{
	internal class MonteCarlo
	{
		public Game Game { get; set; }
		public MonteCarlo(Game game)
		{
			Game = game;
		}

		struct Run
		{
			Direction direction;

			int finalScore;
		}

		internal void MonteCarloInit (ulong[,] Board)
		{
			ulong[,] FirstBoard = Board;

			Run run = GetBestMove(Board);

			game
			Board = FirstBoard;


		}

		internal Run GetBestMove(ulong[,] Board)
		{






		}



	}
}

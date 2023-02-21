using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static _2048Solver.Game;

namespace _2048Solver
{

	internal class MonteCarlo
	{
	
		public Game Game { get; set; }

		public List<Run> Runs10 = new List<Run>()
					{
			 new Run(){ direction = Direction.Down, finalScore=432}
					};

		//Runs10.AddRange(cities);
		List<Run> Runs100 { get; set; }
		
		List<Run> Runs500;



		public MonteCarlo(Game game)
		{
			Game = game;
			
			var collection = new List<Run>();

			Runs100 = Enumerable.Range(0, 100).Select(x => { if (x <= 25) return new Run() { direction = Direction.Up, finalScore = x };
				if (x > 25 && x <= 50) return new Run() { direction = Direction.Down, finalScore = x };
				if (x > 50 && x <= 75) return new Run() { direction = Direction.Right, finalScore = x };
				if (x > 75 && x <= 100) return new Run() { direction = Direction.Left, finalScore = x };
				else return new Run() { direction = Direction.Up, finalScore = x };

			}).ToList<Run>();

		}

		public struct Run
		{
			internal Direction direction;

			internal int finalScore;
		}
		internal void MonteCarloInit(ulong[,] Board, ulong Score)
		{
			ulong[,] FirstBoard = Board;

			ulong FirstScore = Score;

			List<Run> run = GetBestMove(Board, Score);

			Board = FirstBoard;

			FirstScore = Score;

		}

		internal List<Run> GetBestMove(ulong[,] Board, ulong Score)
		{

			do
			{

				foreach (string i in cars)
				{
					Console.WriteLine(i);
				}



				for (int i = 0; i < length; i++)
				{
					switch (input.Key)
					{
						case ConsoleKey.UpArrow:
							hasUpdated = Update(Direction.Up);
							break;

						case ConsoleKey.DownArrow:
							hasUpdated = Update(Direction.Down);
							break;

						case ConsoleKey.LeftArrow:
							hasUpdated = Update(Direction.Left);
							break;

						case ConsoleKey.RightArrow:
							hasUpdated = Update(Direction.Right);
							break;

						default:
							hasUpdated = false;
							break;
					}



				}





			} while (Game.IsDead());

			return null;
		}



	}
}

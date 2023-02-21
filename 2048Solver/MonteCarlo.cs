using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static _2048Solver.Game;
using static System.Formats.Asn1.AsnWriter;

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

			Runs100 = Enumerable.Range(0, 100).Select(x =>
			{
				if (x <= 25) return new Run() { direction = Direction.Up, finalScore = x };
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
		internal void Init()
		{

			List<Run> run = GetBestMove(Game.Board, Game.Score);


		}

		internal List<Run> GetBestMove(ulong[,] Board, ulong Score)
		{
			ulong[,] FirstBoard = (ulong[,])Board.Clone();

			ulong FirstScore = Score;

			FirstScore = Score;

			do
			{

				foreach (Run item in Runs100)
				{
					Game.PutNewValue();

					Game.Update(Board, item.direction, out Score);

					while (!Game.IsDead())
					{
						if (Game.IsDead())
						{
							break;
						}

						Game.Update(Board, GetRandomDirection(), out Score);

						if (Game.IsDead())
						{
							break;
						}
						Game.PutNewValue();
						

						if (Game.IsDead())
						{
							break;
						}
					}
					Game.Display();

				}

			} while (Game.IsDead());

			return null;
		}

		private Direction GetRandomDirection()
		{
			Random random = new Random();

			Type type = typeof(Direction);

			Array values = type.GetEnumValues();

			int index = random.Next(values.Length);

			return (Direction)values.GetValue(index);
		}


	}
}

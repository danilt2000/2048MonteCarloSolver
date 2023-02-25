using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		private bool repetionFlag = false;

		public List<Run> Runs10 = new List<Run>()
					{
			 new Run(){ direction = Direction.Down, finalScore=432}
					};

		//Runs10.AddRange(cities);
		List<Run> Runs100 { get; set; }

		List<Run> Runs200 { get; set; }



		public MonteCarlo(Game game)
		{
			Game = game;

			var collection = new List<Run>();

			Runs100 = Enumerable.Range(0, 100).Select(x =>
			{
				if (x < 25) return new Run() { direction = Direction.Up, finalScore = x };
				if (x >= 25 && x < 50) return new Run() { direction = Direction.Down, finalScore = x };
				if (x >= 50 && x < 75) return new Run() { direction = Direction.Right, finalScore = x };
				if (x >= 75 && x < 100) return new Run() { direction = Direction.Left, finalScore = x };
				else return new Run() { direction = Direction.Up, finalScore = x };

			}).ToList();

			Runs200 = Enumerable.Range(0, 200).Select(x =>
			{
				if (x < 50) return new Run() { direction = Direction.Up, finalScore = x };
				if (x >= 50 && x < 100) return new Run() { direction = Direction.Down, finalScore = x };
				if (x >= 100 && x < 150) return new Run() { direction = Direction.Right, finalScore = x };
				if (x >= 150 && x < 200) return new Run() { direction = Direction.Left, finalScore = x };
				else return new Run() { direction = Direction.Up, finalScore = x };

			}).ToList();

		}

		public class Run
		{
			internal Direction direction;

			internal int finalScore;
		}
		internal void Init()
		{

			List<Run> run = GetBestRace(Game.Board, Game.Score);


		}

		internal List<Run> GetBestRace(ulong[,] Board, ulong Score)
		{
			Stopwatch sw = new Stopwatch();

			bool hasUpdated = true;

			int countOfRepetion = 0;

			ulong score;

			sw.Start();

			Game.Score = 0;

			do
			{
				//Game.Display();

				if (hasUpdated)
				{
					Game.PutNewValue();
				}

				//Game.Display();

				ulong[,] BeforeBoard = (ulong[,])Game.Board.Clone();

				ulong BeforeScore = Game.Score;

				foreach (Run Run in Runs200)
				{
					Game.Score = BeforeScore;

					score = BeforeScore;

					hasUpdated = Game.Update(Game.Board, Run.direction, out score);

					//Game.Display();

					while (!Game.IsDead())
					{
						if (hasUpdated)
						{
							Game.PutNewValue();
						}

						hasUpdated = Game.Update(Game.Board, GetRandomDirection(), out score);

						Game.Score += score;

						//Game.Display();

						if (Game.IsDead())
						{
							break;
						}

					}

					//Game.Display();

					Run.finalScore = (int)Game.Score;

					Game.Board = (ulong[,])BeforeBoard.Clone();
				}

				Game.Score = BeforeScore;

				score = BeforeScore;

				if (Game.IsDead())
				{
					break;
				}

				//Game.Display();

				Direction bestDirecteion = GetBestDirection(Runs200);

				hasUpdated = Game.Update(Game.Board, bestDirecteion, out score);

				Game.Score += score;

				if (Game.Score == BeforeScore)
				{
					countOfRepetion++;

					if (countOfRepetion == 20)
					{
						repetionFlag = true;
					}
				}
				else
				{
					countOfRepetion = 0;
				}


				//Game.Display();

				//Game.Display();

			} while (true);

			sw.Stop();

			return null;
		}
		private Direction GetBestDirection(List<Run> Runs)
		{
			int UpScores = 0;

			int DownScores = 0;

			int RightScores = 0;

			int LeftScores = 0;

			Runs.ForEach(x =>
			{
				if (x.direction == Direction.Up)
				{
					UpScores = x.finalScore + UpScores;
				}
				if (x.direction == Direction.Down)
				{
					DownScores = x.finalScore + DownScores;

				}
				if (x.direction == Direction.Right)
				{
					RightScores = x.finalScore + RightScores;

				}
				if (x.direction == Direction.Left)
				{
					LeftScores = x.finalScore + LeftScores;

				}

			});

			int Divisor = 0;

			if (Runs.Count == 100)
			{
				Divisor = 25;
			}
			if (Runs.Count == 200)
			{
				Divisor = 50;
			}

			Run AverageUpScore = new Run { direction = Direction.Up, finalScore = UpScores / Divisor };

			Run AverageDownScore = new Run { direction = Direction.Down, finalScore = DownScores / Divisor };

			Run AverageRightScore = new Run { direction = Direction.Right, finalScore = RightScores / Divisor };

			Run AverageLeftScore = new Run { direction = Direction.Left, finalScore = LeftScores / Divisor };

			int bestAverageScore = new List<Run> { AverageUpScore, AverageDownScore, AverageRightScore, AverageLeftScore }.Max(x => x.finalScore);

			Direction bestDirecteion = Direction.Up;

			new List<Run> { AverageUpScore, AverageDownScore, AverageRightScore, AverageLeftScore }.ForEach(x =>
			{

				if (x.finalScore == bestAverageScore)
				{
					bestDirecteion = x.direction;

				}
			}
			);

			if (repetionFlag)
			{
				repetionFlag = false;

				switch (bestDirecteion)
				{
					case Direction.Up:
						return AverageRightScore.finalScore > AverageLeftScore.finalScore ? AverageRightScore.direction : AverageLeftScore.direction;

					case Direction.Down:
						return AverageRightScore.finalScore > AverageLeftScore.finalScore ? AverageRightScore.direction : AverageLeftScore.direction;

					case Direction.Right:
						return AverageDownScore.finalScore > AverageUpScore.finalScore ? AverageDownScore.direction : AverageUpScore.direction;

					case Direction.Left:
						return AverageDownScore.finalScore > AverageUpScore.finalScore ? AverageDownScore.direction : AverageUpScore.direction;
				}
			}

			return bestDirecteion;
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

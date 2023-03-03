using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

		public Game GameItem { get; set; }

		private bool repetionFlag = false;

		public List<Run> Runs10 = new List<Run>()
					{
			 new Run(){ direction = Direction.Down, finalScore=432}
					};

		List<Run> Runs100 { get; set; }

		List<Run> Runs200 { get; set; }



		public MonteCarlo(Game game)
		{
			GameItem = game;

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

			GetBestRace(GameItem.Board, GameItem.Score);


		}

		internal void GetBestRace(ulong[,] Board, ulong Score)
		{
			Stopwatch sw = new Stopwatch();

			List<Game> games = new List<Game>();

			for (int i = 0; i < 30; i++)
			{
				sw.Start();

				var game = GetSolvedGame();

				sw.Stop();

				game.GameTime = $"Minutes = {sw.Elapsed.TotalMinutes.ToString().Substring(0, sw.Elapsed.TotalMinutes.ToString().IndexOf(".") + 3)}" +
					$" or seconds = {sw.Elapsed.TotalSeconds.ToString().Substring(0, sw.Elapsed.TotalSeconds.ToString().IndexOf(".") + 3)}";

				games.Add(game);

				sw.Reset();

				DisposeGame();
			}

			ExcelConverter.СonvertGamesToExcel(games, @"C:\test\SolvedGamesData.xlsx");
		
		}



		private void DisposeGame()
		{
			GameItem.Score = 0;

			GameItem.Board = new ulong[4, 4];

			GameItem.MaxNumber = 0;

			GameItem.GameTime = string.Empty;

		}

		private Game GetSolvedGame()
		{
			bool hasUpdated = true;

			int countOfRepetion = 0;

			ulong score;

			do
			{
				if (hasUpdated)
				{
					GameItem.PutNewValue();
				}

				ulong[,] BeforeBoard = (ulong[,])GameItem.Board.Clone();

				ulong BeforeScore = GameItem.Score;

				foreach (Run Run in Runs200)
				{
					GameItem.Score = BeforeScore;

					score = BeforeScore;

					hasUpdated = Game.Update(GameItem.Board, Run.direction, out score);

					while (!GameItem.IsDead())
					{
						if (hasUpdated)
						{
							GameItem.PutNewValue();
						}

						hasUpdated = Game.Update(GameItem.Board, GetRandomDirection(), out score);

						GameItem.Score += score;

						if (GameItem.IsDead())
						{
							break;
						}

					}

					Run.finalScore = (int)GameItem.Score;

					GameItem.Board = (ulong[,])BeforeBoard.Clone();

				}

				GameItem.Score = BeforeScore;

				score = BeforeScore;

				if (GameItem.IsDead())
				{
					break;
				}

				Direction bestDirecteion = GetBestDirection(Runs200);

				hasUpdated = Game.Update(GameItem.Board, bestDirecteion, out score);

				GameItem.Score += score;

				if (GameItem.Score == BeforeScore)
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

			} while (true);

			GameItem.MaxNumber = GetMaxNumber();

			return new Game() { Board = (ulong[,])GameItem.Board.Clone(), Score = GameItem.Score, MaxNumber = GameItem.MaxNumber };
		}

		private int GetMaxNumber()
		{
			int maxNumber = 0;

			for (int Rows = 0; Rows < 4; Rows++)
			{
				for (int Cols = 0; Cols < 4; Cols++)
				{

					if (maxNumber < (int)GameItem.Board[Rows, Cols])
					{
						maxNumber = (int)GameItem.Board[Rows, Cols];
					}

				}
			}

			return maxNumber;
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

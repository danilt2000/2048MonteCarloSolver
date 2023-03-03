using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048Solver
{
	internal class ConsoleWriter
	{
		internal static void WriteDataStatistics(List<Game> gameDataList)
		{
			WriteUsualGameData(gameDataList);

			WriteStatictics(gameDataList);
		}

		private static void WriteStatictics(List<Game> gameDataList)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"The largest score {gameDataList.Max(x => x.Score)}");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($"The worst score {gameDataList.Min(x => x.Score)}");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Yellow;

			Console.WriteLine($"The average score {gameDataList.Average(x => (decimal)x.Score)}");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Blue;

			Console.WriteLine($"The average value of the maximum cell reached in the game {gameDataList.Average(x => (decimal)x.MaxNumber)}");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Yellow;

			int countOfWins = 0;
			int countOfLosses = 0;
			gameDataList.ForEach(game =>
			{
				if (game.MaxNumber >= 2048)
				{
					countOfWins++;
				}
				else
				{
					countOfLosses++;
				}

			});
			Console.WriteLine($"Count of wins:{countOfWins} Count of losses:{countOfLosses}");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Cyan;

			Console.WriteLine($"Algorithm uses 200 runs in the background ");

			Console.ResetColor();
		}

		private static void WriteUsualGameData(List<Game> gameDataList)
		{
			int index = 1;
			foreach (var gameData in gameDataList)
			{
				Console.ForegroundColor = ConsoleColor.Green;

				Console.Write($"Position:{index}");

				Console.ForegroundColor = ConsoleColor.Red;

				Console.Write($"Max Score:{gameData.Score}");

				Console.ForegroundColor = ConsoleColor.Yellow;

				Console.Write($"Max Number:{gameData.MaxNumber}");

				Console.ForegroundColor = ConsoleColor.Magenta;

				Console.Write($"Time:{gameData.GameTime}");

				Console.WriteLine();

				index++;
			}
		}
	}
}

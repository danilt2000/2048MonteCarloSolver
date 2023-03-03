using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static Google.Cloud.Language.V1.PartOfSpeech.Types;

namespace _2048Solver
{
	internal class ExcelConverter
	{
		internal static List<Game> GetGamesFromExcel(string filePath)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			
			var gameDataList = new List<Game>();
			
			var fileInfo = new FileInfo(filePath);
			
			using (var package = new ExcelPackage(fileInfo))
			{
				var worksheet = package.Workbook.Worksheets[0];

				var rows = worksheet.Dimension.Rows;


				for (var i = 2; i <= rows; i++)
				{
					var finalScore = Convert.ToInt32(worksheet.Cells[i, 1].Value);

					var maxNumber = Convert.ToInt32(worksheet.Cells[i, 2].Value);

					var gameTimeStr = worksheet.Cells[i, 3].Value.ToString();

					var game = new Game { Score = (ulong)finalScore, MaxNumber = maxNumber, GameTime = gameTimeStr };

					gameDataList.Add(game);
				}

			}

			return gameDataList;
		}

		

		internal static void СonvertGamesToExcel(List<Game> games, string filePath)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			ExcelPackage excelPackage = new ExcelPackage();

			// Создание листа в Excel файле
			var worksheet = excelPackage.Workbook.Worksheets.Add("People");

			// Запись заголовков в Excel файл
			worksheet.Cells["A1"].Value = "FinalScore";

			worksheet.Cells["B1"].Value = "MaxNumber";

			worksheet.Cells["C1"].Value = "GameTime";

			// Запись данных о персонах в Excel файл
			int row = 2;

			foreach (var game in games)
			{
				worksheet.Cells["A" + row].Value = game.Score;

				worksheet.Cells["B" + row].Value = game.MaxNumber;

				worksheet.Cells["C" + row].Value = game.GameTime;

				row++;
			}

			SaveExcelFile(excelPackage, filePath);
		}

		private static void SaveExcelFile(ExcelPackage excelPackage, string filePath)
		{
			FileInfo excelFile = new FileInfo(filePath);

			excelPackage.SaveAs(excelFile);
		}

	}
}

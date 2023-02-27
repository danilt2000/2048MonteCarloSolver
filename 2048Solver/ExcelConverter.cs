using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048Solver
{
	internal class ExcelConverter
	{

		internal static void СonvertGamesToExcel(List<Game> games)
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

			SaveExcelFile(excelPackage);
		}

		private static void SaveExcelFile(ExcelPackage excelPackage)
		{
			string filePath = @"C:\test\SolvedGamesData3.xlsx";
			
			FileInfo excelFile = new FileInfo(filePath);
			
			excelPackage.SaveAs(excelFile);
		}

	}
}

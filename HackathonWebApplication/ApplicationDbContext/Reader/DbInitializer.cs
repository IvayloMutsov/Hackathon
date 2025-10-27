using Infrastructure.Models;
using OfficeOpenXml;
using System;
using Data;

namespace Data.Reader
{
    public static class DbInitializer
    {
        public static void SeedProffessors(AppDbContext context)
        {
            ExcelPackage.License.SetNonCommercialPersonal("ivo");
            string filePath = "C:\\Users\\igmut\\source\\repos\\Hackaton\\Hackaton\\Data\\Database.xlsx";

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets["Professors"];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                // Start from row 2 to skip header
                for (int row = 2; row <= rowCount; row++)
                {
                    Proffessors prof = new Proffessors();
                    var cells = new List<string>();
                    for (int col = 2; col <= colCount; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Text;
                        switch (col)
                        {
                            case 2:
                                prof.AcademicRank = cellValue;
                                break;
                            case 3:
                                prof.FullName = cellValue;
                                break;
                            case 4:
                                prof.ScientificFiled = cellValue;
                                break;
                            case 5:
                                prof.University = cellValue;
                                break;
                            case 6:
                                prof.Distance = int.Parse(cellValue);
                                if (prof.Distance == 0)
                                {
                                    prof.UniIsLocal = true;
                                }
                                else
                                {
                                    prof.UniIsLocal = false;
                                }
                                break;
                            case 7:
                                prof.ConsecutiveCounter = int.Parse(cellValue);
                                break;
                            case 8:
                                prof.PrevParticipationDate = DateTime.Parse(cellValue);
                                break;
                            case 9:
                                prof.LastParticipationDate = DateTime.Parse(cellValue);
                                break;
                            default:
                                throw new Exception("Data is in invalid format!");
                        }
                    }
                    context.Proffessors.Add(prof);
                }
                context.SaveChanges();
            }
        }
    }
}

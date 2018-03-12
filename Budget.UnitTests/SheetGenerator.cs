using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Metaproject.Excel;
using Metaproject.Excel.Excel;
using Metaproject.Excel.Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Budget.UnitTests
{
    [TestClass]
    public class SheetGenerator
    {
        [TestMethod]
        public void GenerateSheets()
        {

            int YEAR = 2017;

            object[] headers =
            {
                @"Project/Activity", "Feature", "Employee", "Comment", "Business Line (BL)",
                "Time(date)", "Location", "Work Units"
            };

            string PATH = @"c:\Maciek\Timesheets\2017";

            int COL_FIRST = 1;
            int COL_LAST = 8;
            int ROW_FIRST = 1;
            int ROW_DATA_FIRST = 2;
            
            ExcelApp excel = new ExcelApp();
            excel.CreateNewInstance();

            for (int m = 12; m >= 1; m--)
            {
                IExcelWorkbook workbook = excel.CreateAndActivateNewWorkbook();
                string workBookName = $"Protokol_MSzczudlo_{YEAR}_{m:D2}.xlsx";
                string workBookPath = Path.Combine(PATH, workBookName);

                int daysInMonth = DateTime.DaysInMonth(YEAR, m);
                int ROW_LAST = daysInMonth + 1;
                int ROW_SUM = ROW_LAST + 1;

                ExcelSheetData sheetData = new ExcelSheetData();
                ExcelRowData headerRowData = new ExcelRowData(COL_FIRST, ROW_FIRST, COL_LAST, ROW_FIRST);
                headerRowData.Values = headers.ToList();
                sheetData.Rows.Add(headerRowData);
                ExcelFormatData headerRowFormat = new ExcelFormatData(COL_FIRST, ROW_FIRST, COL_LAST, ROW_FIRST);
                headerRowFormat.Background = Color.LightGray;
                headerRowFormat.IsFontBold = true;
                headerRowFormat.HorizontalAlignment = ExcelHorizontalAlignmentType.Center;
                sheetData.Formats.Add(headerRowFormat);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    object[] values = new object[8];
                    

                    int row = d + 1;
                    DateTime iDay = new DateTime(YEAR, m, d);

                    values[5] = iDay.ToShortDateString();

                    bool isFreeDay = iDay.DayOfWeek == DayOfWeek.Saturday || iDay.DayOfWeek == DayOfWeek.Sunday;
                    if (isFreeDay)
                    {
                        ExcelFormatData iFormatData = new ExcelFormatData(COL_FIRST, row, COL_LAST, row);
                        iFormatData.Background = Color.Salmon;
                        sheetData.Formats.Add(iFormatData);
                        ExcelRowData iRow = new ExcelRowData(COL_FIRST, row, COL_LAST, row);
                        iRow.Values = values.ToList();
                        sheetData.Rows.Add(iRow);
                    }
                    else
                    {
                        values[0] = "IS Treasury";
                        values[2] = "Maciej Szczudło";
                        values[7] = 8;

                        ExcelRowData iRow = new ExcelRowData(COL_FIRST, row, COL_LAST, row);
                        iRow.Values = values.ToList();
                        sheetData.Rows.Add(iRow);
                    }

                    
                    ExcelBorderData borderData = new ExcelBorderData(COL_FIRST, ROW_FIRST, COL_LAST, ROW_LAST);
                    borderData.Borders.AddBorder(ExcelBordersIndex.xlAround, ExcelBorderWeight.xlThin, ExcelLineStyle.xlContinuous);
                    borderData.Borders.AddBorder(ExcelBordersIndex.xlInside, ExcelBorderWeight.xlThin, ExcelLineStyle.xlContinuous);
                    sheetData.Borders.Add(borderData);

                    ExcelFormatData fontFormatData = new ExcelFormatData(borderData.Range);
                    fontFormatData.FontSize = 10;
                    sheetData.Formats.Add(fontFormatData);
                }


                string monthName = DateTimeTools.GetMonthName(m);
                string sheetName = $"{monthName} {YEAR}";

                IExcelSheet newSheet = workbook.CreateSheet(sheetName);
                
                ExcelRangeInfo sumRange = new ExcelRangeInfo(ROW_DATA_FIRST, COL_LAST, ROW_LAST, COL_LAST);
                ExcelFormula formula = ExcelFormulaHelper.GetSum(ROW_SUM, COL_LAST, sumRange);
                sheetData.Formulas.Add(formula);
                ExcelBorderData borderSumData = new ExcelBorderData(formula.Range);
                borderSumData.Borders.AddBorder(ExcelBordersIndex.xlAround, ExcelBorderWeight.xlThin, ExcelLineStyle.xlContinuous);
                sheetData.Borders.Add(borderSumData);

                newSheet.InsertSheetData(sheetData);
                newSheet.SetColumnsAutoFit(COL_FIRST, COL_LAST);
                newSheet.SetCellName(ROW_SUM, COL_LAST, "godziny");

                excel.SetDisplayGridlines(false);

                workbook.Save(workBookPath);
            }
        }

    }
}

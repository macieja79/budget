using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Globalization;




namespace MtExcel
{
   
   	 /*

    /// <summary>
    /// Format komorki Excela
    /// </summary>
 

    /// <summary>
    /// Funkcje wspomagajace tworzenie raportu
    /// </summary>
    public class ExcelGenerator
    {
       
        #region <Constructor>

        public ExcelGenerator()
        {
            LaunchExcel();
     
        }//ExcelGenerator

        #endregion

        #region <IO>

        bool LaunchExcel()
        {

            if (Excel == null)
                Excel = new ApplicationClass();

            return true;
        }

        public bool QuitExcel()
        {
            if (sheet != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                sheet = null;
            }
            if (this.Wrkbook != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(this.Wrkbook);
                this.Wrkbook = null;
            }
            if (Excel != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(Excel);
                Excel = null;
            }

            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            return true;
        }

        public bool OpenWorkbookForImport(string ImportedFileName)
        {
            object[] args =  { ImportedFileName };
            Wrkbook = (Workbook)Excel.Workbooks.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, Excel.Workbooks, args);

            sheet = (Worksheet)Wrkbook.ActiveSheet;
            
            return true;

        }//excel

        public bool OpenCreatedReport(string FileName)
        {
            object[] args =  { FileName };

            try
            {
                Excel.Workbooks.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, Excel.Workbooks, args);
            }
            catch
            {
                return false;
            }
            Excel.Visible = true;

            return true;

        }

        public bool CloseActiveWorkbook()
        {
            Wrkbook.Close(false, Missing.Value, Missing.Value);
            return true;
        }
              
        public bool OpenReportTemplate(string templateName, string sheetName)
        {
            Excel.SheetsInNewWorkbook = 1;

            Wrkbook = Excel.Workbooks.Add(templateName);

            sheet = (Worksheet)Wrkbook.ActiveSheet;

            Excel.ActiveWindow.DisplayGridlines = false;

            sheet.Name = sheetName;

            return true;

        }

        public bool SaveReport(string folderReports, string reportName, out string outputName)
        {

            string FilePath = Path.Combine(folderReports, reportName);

            bool isXLS = true;

            if (File.Exists(FilePath + ".xls") || File.Exists(FilePath + ".xlsx"))
            {
                isXLS = File.Exists(FilePath + ".xls");
                
                for (int i = 1; i < 10; i++)
                {
                    string FilePathNew = FilePath + "(" + i.ToString() + ")";

                    bool isExist;

                    if (isXLS)
                    { isExist = File.Exists(FilePathNew + ".xls"); }
                    else
                    { isExist = File.Exists(FilePathNew + ".xlsx"); }

                    if (!isExist)
                    {
                        FilePath = FilePathNew;
                        break;
                    }

                }//for

            }//if

            outputName = Path.GetFileName(FilePath);

            object[] args =  { FilePath };
            Wrkbook.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, Wrkbook, args);

            CloseActiveWorkbook();

            return true;

        }

        public bool ShowExcel()
        {
            Excel.Visible = true;
            SetForegroundWindow((IntPtr)Excel.Hwnd);  
            return true;

        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        
        #endregion

        #region <Generating>

        /// <summary>
        /// Zwraca excelowski symbol kolumny
        /// </summary>
        /// <param name="nr"></param>
        /// <returns></returns>
        private string ColumnSymbol(int nr)
        {

            int n1 = nr / 26;
            int n2 = nr % 26;

            if (n2 == 0)
            {

                n1--;
                n2 = 26;
            }

            string s1 = "";
            string s2 = "";

            if (n1 > 0)
            {
                s1 = char.ConvertFromUtf32(64 + n1);

            }

            if (n2 > 0)
            {
                s2 = char.ConvertFromUtf32(64 + n2);

            }

            string outStr = s1 + s2;

            return outStr;

        }//ColumnSymbol

        /// <summary>
        /// Symbol komorki na podstawie indeksow wiersza i kolumny
        /// </summary>
        /// <param name="r">Wiersz</param>
        /// <param name="c">Kolumna</param>
        /// <returns></returns>
        public string CellSymbol(int r, int c)
        {

            string col = ColumnSymbol(c);

            string row = r.ToString();

            string cellSymb = col + row;

            return cellSymb;

        }

        public Range GetRange(int r1, int c1, int r2, int c2)
        {
            string cell1 = CellSymbol(r1, c1);

            string cell2 = CellSymbol(r2, c2);

            return sheet.get_Range(cell1, cell2);

        }//GetRange
        
        /// <summary>
        /// Rysuje kratkê na zakresie komórek
        /// </summary>
        /// <param name="r">Zakres</param>
        /// <param name="BorderOut">Kratka zewnêtrzna</param>
        /// <param name="BorderIn">Kratka wewnêtrzna</param>
        public void DrawGrid(Range r, XlBorderWeight BorderOut, XlBorderWeight BorderIn)
        {
            try
            {
                r.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = BorderOut;
                r.Borders.get_Item(XlBordersIndex.xlEdgeTop).Weight = BorderOut;
                r.Borders.get_Item(XlBordersIndex.xlEdgeLeft).Weight = BorderOut;
                r.Borders.get_Item(XlBordersIndex.xlEdgeRight).Weight = BorderOut;


                r.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).Weight = BorderIn;
                r.Borders.get_Item(XlBordersIndex.xlInsideVertical).Weight = BorderIn;

                r = null;
            }
            catch
            {
            }

        }//RangeBorder

        /// <summary>
        /// Rysuje kratkê na zakresie komórek
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="c1"></param>
        /// <param name="r2"></param>
        /// <param name="c2"></param>
        /// <param name="BorderOut">Kratka zewnêtrzna</param>
        /// <param name="BorderIn">Kratka wewnêtrzna</param>
        public void DrawGrid(int r1, int c1, int r2, int c2, XlBorderWeight BorderOut, XlBorderWeight BorderIn)
        {
            Range r = GetRange(r1, c1, r2, c2);

            DrawGrid(r, BorderOut, BorderIn);

        }

        public void DrawBorder(int r1, int c1, int r2, int c2, XlBorderWeight BorderOut)
        {
            Range r = GetRange(r1, c1, r2, c2);

            DrawBorder(r, BorderOut);

        }

        public void DrawBorder(int r1, int c1, int r2, int c2, XlBorderWeight BorderOut, bool isMerged)
        {
            Range r = GetRange(r1, c1, r2, c2);

            DrawBorder(r, BorderOut);

            r.Merge(Missing.Value);

        }
        
        public void DrawBorder(Range r, XlBorderWeight BorderOut)
        {
            r.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = BorderOut;
            r.Borders.get_Item(XlBordersIndex.xlEdgeTop).Weight = BorderOut;
            r.Borders.get_Item(XlBordersIndex.xlEdgeLeft).Weight = BorderOut;
            r.Borders.get_Item(XlBordersIndex.xlEdgeRight).Weight = BorderOut;

        }//RangeBorder

        public void DrawLineHorizontal(int r, int c1, int c2, XlBorderWeight BorderOfLine)
        {

            Range rng = GetRange(r, c1, r, c2);
            rng.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = BorderOfLine;
        }

        public void DrawLineVertical(int c, int r1, int r2, XlBorderWeight BorderOfLine)
        {
            Range r = GetRange(r1, c, r2, c);
            r.Borders.get_Item(XlBordersIndex.xlEdgeRight).Weight = BorderOfLine;

        }

        /// <summary>
        /// Wstawia wartoœæ do komórki
        /// </summary>
        /// <param name="r">Wiersz</param>
        /// <param name="c">Kolumna</param>
        /// <param name="value">Wartoœæ</param>
        public void PutValue(int r, int c, string value)
        {
            sheet.Cells[r, c] = value;
        }

        public string GetValueAsString(int r, int c)
        {
            Range rng = GetRange(r, c, r, c);

            if (rng.Cells.Value2 != null)
            {
                string value = (string)rng.Cells.Value2.ToString();
                return value;
            }
            else
            {
                return null;
            }
        }

        public double GetValueAsDouble(int r, int c)
        {
            string value = GetValueAsString(r, c);

            double doubleValue;

            if (double.TryParse(value, out doubleValue))
                return doubleValue;
            else
                return double.NaN;
        }

        public DateTime GetValueAsDateTime(int r, int c, string [] formats, out bool isParsed)
        {

            string value = GetValueAsString(r, c);
            
            DateTime dateTimeValue;         
                     
            CultureInfo provider = CultureInfo.CurrentCulture;

            if (DateTime.TryParseExact(value, formats, provider, DateTimeStyles.AdjustToUniversal, out dateTimeValue))   
            {
                isParsed = true;
                return dateTimeValue;
            }
            else
            {
                
                isParsed = false;
                return DateTime.MinValue;
            }

        }

        public void PutFormula(int r, int c, string formula)
        {
            Range cell = GetRange(r, c, r, c);
            cell.FormulaLocal = "=" + formula;

        }

        public void PutSum(int targetR, int targetC, int r1, int c1, int r2, int c2)
        {

            string cell_1 = CellSymbol(r1, c1);
            string cell_2 = CellSymbol(r2, c2);

            Range targetCell = GetRange(targetR, targetC, targetR, targetC);

            string formula = "=SUMA(" + cell_1 + ":" + cell_2 + ")";

            targetCell.FormulaLocal = formula;

        }

        public void PutCrossVerticalSum(int targetRow, int targetColumn, int startRow, int n)
        {
            Range targetCell = GetRange(targetRow, targetColumn, targetRow, targetColumn);

            string cell_1 = CellSymbol(startRow, targetColumn);

            string formula = "=" + cell_1;

            if (n > 1)
            {
                for (int i = 1; i < n; i++)
                {
                    string c = CellSymbol(startRow + i * 2, targetColumn);

                    formula += "+" + c;

                }//for
            }

            targetCell.FormulaLocal = formula;

        }

        public void PutCrossVerticalSum(int targetRow, int targetColumn, int startRow, int n, int step)
        {
            Range targetCell = GetRange(targetRow, targetColumn, targetRow, targetColumn);

            string cell_1 = CellSymbol(startRow, targetColumn);

            string formula = "=" + cell_1;

            if (n > 1)
            {
                for (int i = 1; i < n; i++)
                {
                    string c = CellSymbol(startRow + i * step, targetColumn);

                    formula += "+" + c;

                }//for
            }

            targetCell.FormulaLocal = formula;

        }


        public void ClipboardToCell(int r, int c)
        {

            sheet.Paste(sheet.Cells[r, c], Missing.Value);

        }
        /// <summary>
        /// Ustawia szerokoœci kolejnych kolumn
        /// </summary>
        /// <param name="widths">Np. 5, "2x10", 10 itd. </param>
        public void ColumnsAutoFit(int c1, int c2)
        {
            double minimumColumnWidth = (double)((Range)sheet.Columns[c1, Type.Missing]).ColumnWidth;
            
            for (int c = c1; c <= c2; c++)
            {
                Range rng = ((Range)sheet.Columns[c, Type.Missing]);

                rng.AutoFit();
                
                if ((double)rng.ColumnWidth < minimumColumnWidth)
                { rng.ColumnWidth = minimumColumnWidth; }
                
            }//for

        }//ColumnsWidths

        /// <summary>
        /// Ustawia szerokoœci kolejnych kolumn
        /// </summary>
        /// <param name="widths">Np. 5, "2x10", 10 itd. </param>
        public void ColumnsDelete(int c1, int c2)
        {
           
            for (int c = c1; c <= c2; c++)
            {
                Range rng = ((Range)sheet.Columns[c, Type.Missing]);

                rng.Delete(XlDeleteShiftDirection.xlShiftToLeft);

           
            }//for

        }//ColumnsWidths



        /// <summary>
        /// Wstawia bie¿¹c¹ datê do komórki
        /// </summary>
        /// <param name="r">Wiersz</param>
        /// <param name="c">Komórka</param>
        public void PutCurrentDate(int r, int c)
        {
            DateTime dt = DateTime.Now;

            sheet.Cells[r, c] = dt.ToLongDateString();

            Range ran = GetRange(r, c, r, c);

            ran.HorizontalAlignment = XlHAlign.xlHAlignRight;

            ran.Font.Bold = true;


        }

        public void PutCurrentDate(int r, int c, ExcelFormat f)
        {
            PutCurrentDate(r, c);
            PutFormat(r, c, f);
        }

        /// <summary>
        /// Wstawia wartosc do komorki
        /// </summary>
        /// <param name="r">Wiersz</param>
        /// <param name="c">Kolumna</param>
        /// <param name="value">Wartosc</param>
        public void PutValue(int r, int c, object value)
        {
            sheet.Cells[r, c] = value;

        }

        public void PutValue(int r, int c, object value, ExcelFormat f)
        {
            PutFormat(r, c, f);
            PutValue(r, c, value);

        }

        public void PutFormat(int r, int c, ExcelFormat f)
        {
            PutFormat(r, c, r, c, f);

        }

        public void PutFormat(int r1, int c1, int r2, int c2, ExcelFormat f)
        {
            Range rng = GetRange(r1, c1, r2, c2);

            rng.Font.Color = ColorTranslator.ToOle(f.foreColor);
            rng.Interior.Color = ColorTranslator.ToOle(f.backColor);

            rng.Font.Size = f.fontSize;
            rng.Font.Italic = f.isitalic;
            rng.Font.Bold = f.isbold;

            rng.HorizontalAlignment = f.HAlign;
            rng.VerticalAlignment = f.VAlign;

            if (f.NumberFormat == ExcelFormat.NumberFormatType.Text)
            {
                rng.NumberFormat = "@";
            }

            if (f.NumberFormat == ExcelFormat.NumberFormatType.Number)
            {

                string format = "###,###,###,###.";

                for (int i = 0; i < f.Precision; i++) format += "0";

                rng.NumberFormat = format;

            }

            if (f.isTextWrapped)
            {
                rng.WrapText = true;

            }

        }

        public void PutCancelledItemFormat(int r1, int c1, int r2, int c2)
        {
            Range rng = GetRange(r1, c1, r2, c2);
            rng.Font.Color = ColorTranslator.ToOle(Color.DarkGray);
        }


        public void SetRowHeight(int r, double h)
        {
            ((Range)sheet.Rows[r, Type.Missing]).RowHeight = h;
        }

    


        /// <summary>
        /// Wstawia tablice doubli w wierszu poczawszy od podanej komorki
        /// </summary>
        /// <param name="r">Wiersz</param>
        /// <param name="c">Kolumna</param>
        /// <param name="values">Tablica</param>
        public void PutValues(int r, int c, double[] values)
        {
            int n = values.Length;

            for (int i = 0; i < n; i++)
            {
                if (values[i] > 0)
                    sheet.Cells[r, c + i] = values[i];
            }//for
            
        }

        public void PutValues(int r, int c, double[] values, ExcelFormat f)
        {
            PutValues(r, c, values);

            int n = values.Length;

            PutFormat(r, c, r, c + (n - 1), f);

        }
        

        /// <summary>
        /// Kasowanie selekcji - zerzniete z netu
        /// </summary>
        public void ResetSelection()
        {

            Range rng = GetRange(1, 1, 1, 1);
            rng.Select();
            sheet.Application.CutCopyMode = (XlCutCopyMode)0;

        }

        public void LastCell(out int r, out int c)
        {
            object cells = sheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty, null, sheet, null);

            object[] args =  { "*", System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, XlSearchOrder.xlByRows, XlSearchDirection.xlPrevious, false };
            object rng = cells.GetType().InvokeMember("Find", BindingFlags.InvokeMethod, null, cells, args);
            r = (int)rng.GetType().InvokeMember("Row", BindingFlags.GetProperty, null, rng, null);

            object[] args2 =  { "*", System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious, false };
            rng = cells.GetType().InvokeMember("Find", BindingFlags.InvokeMethod, null, cells, args2);
            c = (int)rng.GetType().InvokeMember("Column", BindingFlags.GetProperty, null, rng, null);

        }//LastCell

        public bool isMergedCells(int r1, int c1, int r2, int c2)
        {
            Range ran = GetRange(r1, c1, r2, c2);


            
            for (int r = r1; r <= r2; r++)
            {
                for (int c = c1; c <= c2; c++)
                {

                    Range rng = GetRange(r, c, r, c);
                    if ((bool)rng.MergeCells)
                    {
                        rng = null;
                        return true;
                    }

                }//for

            }//for

            return false;
           
        }

        public void SetPrintArea(int r1, int c1, int r2, int c2)
        {
            string printRange = CellSymbol(r1,c1) + ":" + CellSymbol(r2, c2);

            Excel.ActiveWindow.View = XlWindowView.xlPageBreakPreview;
            sheet.PageSetup.PrintArea = printRange;

        }
        #endregion 

        #region <Data>
        Application Excel = null;
        Workbook Wrkbook;
        Worksheet sheet;
        #endregion

    }//ExcelGenerator
		*/
}

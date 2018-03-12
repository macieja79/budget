

//namespace Common.Office.Excel
//{

	/*
    public class MtExcelWriter : IDisposable
    {
               
        #region <members>
        Application _application;
        Workbook _workbook;
        Worksheet _sheet;
        #endregion

        #region <constructor>
        private MtExcelWriter()
        {
            _application = new Application();
            
            _workbook = _application.ActiveWorkbook;

            if (null == _workbook)
                CreateAndActivateNewWorkbook();

            _sheet = (Worksheet)_workbook.ActiveSheet;

        }

        public static MtExcelWriter Launch()
        {
            return new MtExcelWriter();
        }

        #endregion

        #region <extern methods>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        #region <properties>
        public bool IsToCloseWhenDisposed { get; set; }

        #endregion

        #region <public methods>


        public void SetExcelVisible()
        {
            _application.Visible = true;
            SetForegroundWindow((IntPtr)_application.Hwnd);
           
        }
        
        public void CreateAndActivateNewWorkbook()
        {
            _workbook = _application.Workbooks.Add(string.Empty);
            _workbook.Activate();

            
            
        }
        
        public string GetActiveWorkbookName()
        {

            Workbook workbook = (Workbook)_application.ActiveWorkbook;

            if (null == workbook)
                return null;
            
            return workbook.Name;
        }

        public void LoadWorkbookFromTamplateFile(string templateName)
        {
            Workbook workbookFromTemplate = _application.Workbooks.Add(templateName);
            workbookFromTemplate.Activate();
        }

        public void LoadWorkbook(string path)
        {
            object[] args = { path };

            try
            {
                _workbook = (Workbook)_application.Workbooks.GetType().InvokeMember
                    ("Open", BindingFlags.InvokeMethod, null, _application.Workbooks, args);

                _sheet = (Worksheet)_workbook.ActiveSheet;
               

            }
            catch
            {
               
            }

            

        }

        public int GetWorkbooksCount()
        {
            return _application.Workbooks.Count;
        }
                

        public void SetDisplayGridlines(bool isToDisplay)
        {
            _application.ActiveWindow.DisplayGridlines = isToDisplay;
            
        }

        public void SetDisplayZeros(bool isToDisplay)
        {
            _application.ActiveWindow.DisplayZeros = isToDisplay;
        }


        public string GetActiveSheetName()
        {
            return (_workbook.ActiveSheet as Worksheet).Name;
        }

        public Worksheet GetActiveSheet()
        {
            return (_workbook.ActiveSheet as Worksheet);
        }
        
      
        
        public void Save(string path)
        {
            object[] args = { path };
            _workbook.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, _workbook, args);
        }

        public int GetWorksheetCount()
        {
            return _workbook.Sheets.Count;
        }


        public bool CheckIfMergedCells(int r1, int c1, int r2, int c2)
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

            }

            return false;

        }

        public void SetValue(int r, int c, string value)
        {
            _sheet.Cells[r, c] = value;
        }

        public void SetValue(int r, int c, object value)
        {
            _sheet.Cells[r, c] = value;
        }
        public void SetValue(int r, int c, string value, MtExcelFormat format)
        {
            format.ApplyToRange(_sheet.Cells[r, c] as Range);
            _sheet.Cells[r, c] = value;
        }

        public void SetValue(int r, int c, double value)
        {
            _sheet.Cells[r, c] = value;
        }

        public void SetValue(int r, int c, double value, MtExcelFormat format)
        {
            format.ApplyToRange(_sheet.Cells[r, c] as Range);
            _sheet.Cells[r, c] = value;
        }

        public void SetValue(int r, int c, int value)
        {
            _sheet.Cells[r, c] = value;
        }

        public void SetColumnsAutoFit(int c1, int c2)
        {
            double minimumColumnWidth = (double)((Range)_sheet.Columns[c1, Type.Missing]).ColumnWidth;

            for (int c = c1; c <= c2; c++)
            {
                Range rng = ((Range)_sheet.Columns[c, Type.Missing]);

                rng.AutoFit();

                if ((double)rng.ColumnWidth < minimumColumnWidth)
                { rng.ColumnWidth = minimumColumnWidth; }

            }

        }

        public void ColumnsAutoFit(int c1, int c2)
        {
           

            for (int c = c1; c <= c2; c++)
            {
                Range rng = ((Range)_sheet.Columns[c, Type.Missing]);
                rng.AutoFit();
            }

        }

        public void SetPrintArea(int r1, int c1, int r2, int c2)
        {
            string printRange = MtExcelSupport.GetCellSymbol(r1, c1) + ":" + MtExcelSupport.GetCellSymbol(r2, c2);
            _sheet.PageSetup.PrintArea = printRange;
            _application.ActiveWindow.View = XlWindowView.xlPageBreakPreview;
            
        }

        public void SetZoom(double percent)
        {
            _application.ActiveWindow.Zoom = percent;
        }

        public void AdjustPrintingZoom(int minimumZoom)
        {
            int zoom = (int)_sheet.PageSetup.Zoom;

            while (_sheet.VPageBreaks.Count > 0)
            {
                zoom--;
                if (zoom < minimumZoom) break;
                _sheet.PageSetup.Zoom = (object)zoom;

     
                _application.ActiveWindow.View = XlWindowView.xlNormalView;
                _application.ActiveWindow.View = XlWindowView.xlPageBreakPreview;
            }

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

        public int[] GetLastCellCoords()
        {
            object cells = _sheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty, null, _sheet, null);

            object[] args = { "*", System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, XlSearchOrder.xlByRows, XlSearchDirection.xlPrevious, false };
            object rng = cells.GetType().InvokeMember("Find", BindingFlags.InvokeMethod, null, cells, args);
            int r = (int)rng.GetType().InvokeMember("Row", BindingFlags.GetProperty, null, rng, null);

            object[] args2 = { "*", System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious, false };
            rng = cells.GetType().InvokeMember("Find", BindingFlags.InvokeMethod, null, cells, args2);
            int c = (int)rng.GetType().InvokeMember("Column", BindingFlags.GetProperty, null, rng, null);

            int[] coords = new int[2];
            coords[0] = r;
            coords[1] = c;
            return coords;

        }

        public void DeleteColumns(int c1, int c2)
        {

            for (int c = c1; c <= c2; c++)
            {
                Range rng = ((Range)_sheet.Columns[c, Type.Missing]);

                rng.Delete(XlDeleteShiftDirection.xlShiftToLeft);
            }

        }

        public void SetPanesFreeze(int rowToFreezeAbove)
        {
            if (rowToFreezeAbove < 0)
                _application.ActiveWindow.FreezePanes = false;
            else
            {
                Range e = GetRange(rowToFreezeAbove, 1, rowToFreezeAbove, 1);
                e.Select();
                _application.ActiveWindow.FreezePanes = true;
                SetSelectionReset();
            }
        }

        public void SetSelectionReset()
        {
            Range rng = GetRange(1, 1, 1, 1);
            rng.Select();
            _sheet.Application.CutCopyMode = (XlCutCopyMode)0;
        }

        public void SetPrintTitleRows(int firstRow, int lastRow)
        {
            string range = "$" + firstRow.ToString() + ":$" + lastRow.ToString();
            _sheet.PageSetup.PrintTitleRows = range;
        }

        public void SetRowHeight(int r, double h)
        {
            ((Range)_sheet.Rows[r, Type.Missing]).RowHeight = h;
        }

        public void SetFormat(int r1, int c1, int r2, int c2, MtExcelFormat format)
        {
            Range r = GetRange(r1, c1, r2, c2);
            format.ApplyToRange(r);
        }

        public void SetFormat(int r1, int c1, int r2, int c2, MtExcelFormat.FormatItemType formatType, object value)
        {
            MtExcelFormat format = new MtExcelFormat();
            format.AddItem(formatType, value);

            Range r = GetRange(r1, c1, r2, c2);
            format.ApplyToRange(r);

        }



        public void SetBorders(int r1, int c1, int r2, int c2, MtExcelBorderSet borders)
        {
            Range r = GetRange(r1, c1, r2, c2);
            borders.ApplyToRange(r);
        }

        public void SetBorder(int r1, int c1, int r2, int c2, MtExcelBorderStyle border)
        {
            Range r = GetRange(r1, c1, r2, c2);
            border.ApplyToRange(r);

        }

        public void SetFormat(int r, int c, MtExcelFormat format)
        {
            Range rng = GetRange(r, c, r, c);
            format.ApplyToRange(rng);
        }

        public void SetFormula(int r, int c, MtExcelFormula formula)
        {
            Range cell = GetRange(r, c, r, c);
            cell.Formula = formula.Expression;
        }

        public void SetFormula(int r, int c, MtExcelFormula formula, MtExcelFormat format)
        {
            Range cell = GetRange(r, c, r, c);
            format.ApplyToRange(cell);
            cell.Formula = formula.Expression;
        }
        
        public void MergeCells(int r1, int c1, int r2, int c2)
        {
            Range r = GetRange(r1, c1, r2, c2);
            r.Merge(Missing.Value);
        }

        public void UnmergeCells(int r1, int c1, int r2, int c2)
        {
            Range r = GetRange(r1, c1, r2, c2);
            r.MergeCells = (object)false;
        }


        public Range GetRange(int r1, int c1, int r2, int c2)
        {
            string cell1 = MtExcelSupport.GetCellSymbol(r1, c1);
            string cell2 = MtExcelSupport.GetCellSymbol(r2, c2);


            return _sheet.get_Range(cell1, cell2);

        }

        public Range GetCell(int r, int c)
        {
            string cell = MtExcelSupport.GetCellSymbol(r, c);
            return _sheet.get_Range(cell, cell);
        }
        
        #endregion
        
        #region IDisposable members

        public void Dispose()
        {
            if (null != _sheet)
            {
                Marshal.ReleaseComObject(_sheet);
                _sheet = null;
            }

            if (null != _workbook)
            {

                if (IsToCloseWhenDisposed)
                {
                    _workbook.Close(false, Missing.Value, Missing.Value);
                }

                Marshal.ReleaseComObject(_workbook);
                _workbook = null;
            }
            

          
               

            if (null != _application)
            {
                Marshal.ReleaseComObject(_application);
                _application = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();    
           
        }

        #endregion
        
    }
  	*/
  

  

  
    

 

//}
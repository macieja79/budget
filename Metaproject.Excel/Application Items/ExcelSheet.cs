using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms.VisualStyles;
using System.Xml.Serialization;
using Metaproject.Dialog;
using Metaproject.Excel.Data;
using Metaproject.Excel.Excel;
using Metaproject.Log;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Patterns.EventAggregator.Events;
using Metaproject.Xml;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using MenuItem = Metaproject.Dialog.MenuItem;


namespace Metaproject.Excel
{
    public class ExcelSheet : IExcelSheet
    {

        #region <members>

        Worksheet _sheet;
        private readonly ExcelWorkbook _workbook;
        private IEventAggregator _aggregator = EventAggregator.Empty;
        private readonly ILogger _logger;

        private bool _isFreezed = false;


        #endregion

        #region <ctor>

        public ExcelSheet(Worksheet sheet, ExcelWorkbook workbook, IEventAggregator aggregator, ILogger logger)
        {
            _sheet = sheet;
            _workbook = workbook;
            _aggregator = aggregator;
            _logger = logger;

            if (null == _logger) _logger = FakeLogger.Instance;

            if (null == _aggregator)
                _aggregator = EventAggregator.Empty;

            Name = sheet.Name;

            _sheet.SelectionChange += SheetOnSelectionChange;
            _sheet.Change += SheetOnChange;

        }

        


        private ExcelSelectionChangedAggEvent _previousSelection = new ExcelSelectionChangedAggEvent();

        private int _col = 0;
        private int _row = 0;

        private void SheetOnSelectionChange(Range target)
        {
            if (_isFreezed) return;

            if (_col == target.Column &&
                _row == target.Row)
                return;
            
            _logger.Log($"Sheet.SelectionChange {target.Count} {target.Column} {target.Row} ");
            _logger.Log($"{_col}={target.Column}");
            _logger.Log($"{_row}={target.Row}");



            ExcelSelectionChangedAggEvent e = new ExcelSelectionChangedAggEvent();
            e.Previous = _previousSelection;
            e.IsSingleCell = target.Cells.Count == 1;
            e.RangeInfo = getRangeName(target);
            e.Column = target.Column;
            e.Row = target.Row;

            _col = target.Column;
            _row = target.Row;

            _previousSelection = e;

            _isFreezed = true;
            _aggregator.PublishEvent<ExcelSelectionChangedAggEvent>(e);
            _isFreezed = false;
        }

        private void SheetOnChange(Range target)
        {
            if (_isFreezed) return;
            _logger.Log("Sheet.Change");

            ExcelValueChangedAggEvent e = new ExcelValueChangedAggEvent();
            e.Row = target.Row;
            e.Column = target.Column;
            e.Value = target.Value2;

            _isFreezed = true;
            _aggregator.PublishEvent<ExcelValueChangedAggEvent>(e);
            _isFreezed = false;
        }


        #endregion

        #region <pub>

        public string Name { get; }

        public bool CheckIfMergedCells(int r1, int c1, int r2, int c2)
        {
            Range ran = GetRange(r1, c1, r2, c2);

            for (int r = r1; r <= r2; r++)
            {
                for (int c = c1; c <= c2; c++)
                {

                    Range rng = GetRange(r, c, r, c);
                    bool isMergeCells = (bool) rng.MergeCells;
                    if (isMergeCells)
                    {
                        rng = null;
                        return true;
                    }

                } //for

            }

            return false;

        }

        public void SetColumnsAutoFit(int c1, int c2)
        {
            double minimumColumnWidth = (double) ((Range) _sheet.Columns[c1, Type.Missing]).ColumnWidth;

            for (int c = c1; c <= c2; c++)
            {
                Range rng = ((Range) _sheet.Columns[c, Type.Missing]);

                rng.AutoFit();

                if ((double) rng.ColumnWidth < minimumColumnWidth)
                {
                    rng.ColumnWidth = minimumColumnWidth;
                }

            }

        }


        public void SetFilter(ExcelRangeInfo rangeInfo, int field, string value)
        {
            Range range = GetRange(rangeInfo);
            range.AutoFilter(Criteria1: value, Field: field);
        }

        public ExcelRangeInfo GetLastCellCoords()
        {
            object cells = _sheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty, null, _sheet, null);

            object[] args =
            {
                "*", System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                System.Reflection.Missing.Value, XlSearchOrder.xlByRows, XlSearchDirection.xlPrevious, false
            };
            object rng = cells.GetType().InvokeMember("Find", BindingFlags.InvokeMethod, null, cells, args);
            int r = (int) rng.GetType().InvokeMember("Row", BindingFlags.GetProperty, null, rng, null);

            object[] args2 =
            {
                "*", System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                System.Reflection.Missing.Value, XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious, false
            };
            rng = cells.GetType().InvokeMember("Find", BindingFlags.InvokeMethod, null, cells, args2);
            int c = (int) rng.GetType().InvokeMember("Column", BindingFlags.GetProperty, null, rng, null);

            return new ExcelRangeInfo(r, c);

        }

        public ExcelRangeInfo GetSelection()
        {
            return new ExcelRangeInfo(_row, _col);
        }

        public void DeleteColumns(int c1, int c2)
        {

            for (int c = c1; c <= c2; c++)
            {
                Range rng = ((Range) _sheet.Columns[c, Type.Missing]);

                rng.Delete(XlDeleteShiftDirection.xlShiftToLeft);
            }

        }

        public void SetSelectionReset()
        {
            Range rng = GetRange(1, 1, 1, 1);
            rng.Select();
            _sheet.Application.CutCopyMode = (XlCutCopyMode) 0;
        }

        public void SetRowHeight(int r, double h)
        {
            ((Range) _sheet.Rows[r, Type.Missing]).RowHeight = h;
        }

        public void MergeCells(int r1, int c1, int r2, int c2)
        {
            Range r = GetRange(r1, c1, r2, c2);
            r.Merge(Missing.Value);
        }

        public void UnmergeCells(int r1, int c1, int r2, int c2)
        {
            Range r = GetRange(r1, c1, r2, c2);
            r.MergeCells = (object) false;
        }


        public void SetPrintTitleRows(int firstRow, int lastRow)
        {
            string range = "$" + firstRow.ToString() + ":$" + lastRow.ToString();
            _sheet.PageSetup.PrintTitleRows = range;
        }

        public void SetName(string name)
        {
            _sheet.Name = name;

        }

      

        public void InsertSheetData(ExcelSheetData data)
        {
            insertData(data);
        }


        public ExcelSheetData GetSheetData(params int[] rowNumbers)
        {

            ExcelSheetData data = new ExcelSheetData();

            ExcelRangeInfo lastCell = GetLastCellCoords();

            int numberOfRows = lastCell.Start.Row;
            int numberOfColumns = lastCell.Start.Column;

            int iRow = 1;
            for (int r = 1; r <= numberOfRows; ++r)
            {
                if (!r.In(rowNumbers)) continue;

                object[] values = new object[numberOfColumns];
                for (int c = 1; c <= numberOfColumns; ++c)
                {
                    Range cellRange = GetCell(r, c);
                    values[c - 1] = cellRange.Value2;
                }

                ExcelRowData rowData = new ExcelRowData(1, iRow, values);
                data.Rows.Add(rowData);
                iRow++;
            }

            return data;
        }


        public ExcelSheetData GetSheetData()
        {
            ExcelSheetData data = new ExcelSheetData();

            ExcelRangeInfo lastCell = GetLastCellCoords();

            int numberOfRows = lastCell.Start.Row;
            int numberOfColumns = lastCell.Start.Column;

            int iRow = 1;
            for (int r = 1; r <= numberOfRows; ++r)
            {
                object[] values = new object[numberOfColumns];
                for (int c = 1; c <= numberOfColumns; ++c)
                {
                    Range cellRange = GetCell(r, c);
                    values[c - 1] = cellRange.Value2;
                }

                ExcelRowData rowData = new ExcelRowData(1, iRow, values);
                data.Rows.Add(rowData);
                iRow++;
            }

            return data;
        }



        public void InsertPivotTable(PivotTableData data)
        {

            Range dataRange = GetRange(data.Range);
            PivotCache pivotCache = _workbook.WrappedWorkbook.PivotCaches().Add(
                XlPivotTableSourceType.xlDatabase, dataRange);

            Range insertRange = GetCell(data.InsertCell.Start.Row, data.InsertCell.Start.Column);
            PivotTables sheetPivotTables = _sheet.PivotTables(Type.Missing);


            PivotTable pivotTable = sheetPivotTables.Add(
                pivotCache, insertRange, data.Name,
                Type.Missing, Type.Missing);

            pivotTable.TableStyle = data.StyleName;

            foreach (string column in data.Columns)
            {
                PivotField pageField = (PivotField) pivotTable.PivotFields(column);
                pageField.Orientation = XlPivotFieldOrientation.xlColumnField;
            }

            foreach (string row in data.Rows)
            {
                PivotField rowField = (PivotField) pivotTable.PivotFields(row);
                rowField.Orientation = XlPivotFieldOrientation.xlRowField;
            }

            foreach (string value in data.Values)
            {
                PivotField valueField = (PivotField) pivotTable.PivotFields(value);
                valueField.Orientation = XlPivotFieldOrientation.xlDataField;
            }

            pivotTable.GrandTotalName = "Suma";
            pivotTable.SmallGrid = true;


        }

        public void RefreshPivotRable(string name)
        {
            PivotTables sheetPivotTables = _sheet.PivotTables(Type.Missing);
            PivotTable pivotTable = sheetPivotTables.Item(name);
            pivotTable.RefreshTable();
        }

        public void SetAutoFilter(int r1, int c1, int r2, int c2)
        {
            var range = GetRange(r1, c1, r2, c2);
            _sheet.AutoFilterMode = false;
            range.AutoFilter(Field: 1);
        }

        public void SetCellSelectionList(int r, int c, List<string> items)
        {


            try
            {
                var range = GetCell(r, c);
                range.Validation.Delete();

                if (!items.IsNullOrEmpty())
                {
                    string formula = string.Join(";", items);
                    range.Validation.Add(Type: XlDVType.xlValidateList, Formula1: formula);
                }

            }
            catch
            {
                //
            }
        }

        public object GetValue(int r, int c)
        {
            var range = GetCell(r, c);
            return range.Value2;
        }

        public void SetValue(int r, int c, object value)
        {
            var range = GetCell(r, c);
            range.Value2 = value;
        }


        

        public void SetContextMenu(int r, int c, MenuItem contextMenu, string applicationId)
        {
            CommandBar bar = _sheet.Application.CommandBars["Cell"];
            
            CommandBarControl control = bar.Controls.Add(Type: MsoControlType.msoControlPopup);
            CommandBarPopup popup = control as CommandBarPopup;
            popup.Caption = contextMenu.Caption;
            popup.Tag = MenuItemTagData.GetStr(applicationId, string.Empty);

            foreach (MenuItem item in contextMenu.Children)
            {
                CommandBarControl iControl = popup.Controls.Add(Type: MsoControlType.msoControlButton);
                CommandBarButton iButton = iControl as CommandBarButton;
                iButton.Caption = item.Caption;
                iButton.Tag = MenuItemTagData.GetStr(applicationId, item.CommandId);
                iButton.Click += ButtonOnClick;
            }
        }

        public void RemoveContextMenuItem(string applicationId)
        {
            CommandBar bar = _sheet.Application.CommandBars["Cell"];
            List<CommandBarControl> toRemove = new List<CommandBarControl>();
            foreach (CommandBarControl ctr in bar.Controls)
            {
                string tag = ctr.Tag;
                bool hasApplicationId = MenuItemTagData.HasApplicationId(tag, applicationId);
                if (hasApplicationId)
                {
                    toRemove.Add(ctr);
                    continue;
                }

                if (ctr.Caption.Contains("Budget") || ctr.Caption.Contains("ommand"))
                {
                    toRemove.Add(ctr);
                    continue;
                }
            }

            foreach (var commandBarControl in toRemove)
            {
                commandBarControl.Delete();
            }
        }

        public void SetCellName(int r, int c, string name)
        {
            Range rng = GetCell(r, c);
            rng.Name = name;
        }

        public void ClearFilters()
        {
            if (_sheet.AutoFilterMode)
            {
                _sheet.ShowAllData();
            }
        }

        private void ButtonOnClick(CommandBarButton ctrl, ref bool cancelDefault)
        {
            string commandId = MenuItemTagData.GetCommandId(ctrl.Tag);
            ExcelCommandSelectedAggEvent e = new ExcelCommandSelectedAggEvent
            {
                CommandId = commandId,
                Column = _col,
                Row = _row
            };

            _aggregator.PublishEvent<ExcelCommandSelectedAggEvent>(e);
        }

        #endregion

        #region <prv>

        Range GetRange(int r1, int c1, int r2, int c2)
        {
            string cell1 = ExcelSupport.GetCellSymbol(r1, c1);
            string cell2 = ExcelSupport.GetCellSymbol(r2, c2);
            return _sheet.get_Range(cell1, cell2);

        }

        Range GetRange(ExcelRangeInfo excelRange)
        {
            if (excelRange.TypeOfRange.HasValue(ExcelRangeInfoType.Column, ExcelRangeInfoType.Row))
            {
                return _sheet.get_Range(excelRange.Range, Type.Missing);
            }
            else if (excelRange.TypeOfRange.HasValue(ExcelRangeInfoType.Cell))
            {
                return _sheet.get_Range(excelRange.Start.Address, excelRange.Start.Address);
            }
            else
            {
                return _sheet.get_Range(excelRange.Start.Address, excelRange.End.Address);
            }
        }



        Range GetCell(int r, int c)
        {
            string cell = ExcelSupport.GetCellSymbol(r, c);
            return _sheet.get_Range(cell, cell);
        }

        void insertData(ExcelSheetData data)
        {
            foreach (ExcelRowData row in data.Rows)
                insertRowData(row);
            foreach (ExcelBorderData border in data.Borders)
                insertBorderData(border);
            foreach (ExcelFormatData format in data.Formats)
                insertFormatData(format);
            foreach (ExcelCellData cell in data.Cells)
                insertCellData(cell);
            foreach (ExcelFormula formula in data.Formulas)
                insertFormula(formula);

        }

       

        void insertRowData(ExcelRowData rowData)
        {
            for (int c = 0; c < rowData.Values.Count; ++c)
            {

                int c0 = rowData.Range.Start.Column + c;
                int r0 = rowData.Range.Start.Row;
                insertValue(r0, c0, rowData.Values[c]);
            }
        }

        private void insertCellData(ExcelCellData cell)
        {


            insertValue(cell.Range.Row, cell.Range.Column, cell.Value);

            //Range cellRange = GetRange(cell.Range.Row, cell.Range.Column, cell.Range.Row, cell.Range.Column);
            //if (cell.Formula.IsNotNull())
            //{
            //    cellRange.Formula = cell.Formula;
            //}
        }

        private void insertFormula(ExcelFormula formula)
        {
            Range range = GetRange(formula.Range);
            range.FormulaLocal = formula.Expression;
        }


        void insertValue(int r, int c, object value)
        {
            if (null == value) return;
            _sheet.Cells[r, c] = value;
        }

        void insertBorderData(ExcelBorderData borderData)
        {


            Range range = GetRange(borderData.Range);

            foreach (ExcelBorderItem item in borderData.Borders)
            {

                XlLineStyle lineSyle = (XlLineStyle) item.LineStyle;
                XlBorderWeight weight = (XlBorderWeight) item.Weight;

                if (item.Index == ExcelBordersIndex.xlAround)
                {
                    range.BorderAround(lineSyle, weight);
                }
                else if (item.Index == ExcelBordersIndex.xlInside)
                {

                    range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = lineSyle;
                    range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = lineSyle;

                    range.Borders[XlBordersIndex.xlInsideHorizontal].Weight = weight;
                    range.Borders[XlBordersIndex.xlInsideVertical].Weight = weight;
                }
                else
                {
                    XlBordersIndex index = (XlBordersIndex) item.Index;
                    range.Borders[index].Weight = weight;
                    range.Borders[index].LineStyle = lineSyle;
                }
            }
        }

        void insertFormatData(ExcelFormatData formatData)
        {

            foreach (ExcelRangeInfo range in formatData.Ranges)
            {
                Range rng = GetRange(range);


                try
                {

                    if (formatData.Foreground.HasValue)
                        rng.Font.Color = ColorTranslator.ToOle(formatData.Foreground.Value);

                    if (formatData.Background.HasValue)
                        rng.Interior.Color = ColorTranslator.ToOle(formatData.Background.Value);

                    if (formatData.FontSize.HasValue)
                        rng.Font.Size = formatData.FontSize.Value;

                    if (formatData.IsFontBold.HasValue)
                        rng.Font.Bold = formatData.IsFontBold.Value;

                    if (formatData.IsFontItalic.HasValue)
                        rng.Font.Italic = formatData.IsFontItalic.Value;

                    if (formatData.IsWrapped.HasValue)
                        rng.WrapText = formatData.IsWrapped.Value;

                    if (formatData.IsCoumnAutofit.HasValue)
                    {
                        foreach (Range col in rng.Columns)
                        {
                            col.EntireColumn.AutoFit();
                        }
                    }

                    if (formatData.HorizontalAlignment.HasValue)
                    {
                        XlHAlign horizAlignment = (XlHAlign) formatData.HorizontalAlignment.Value;
                        rng.HorizontalAlignment = horizAlignment;
                    }

                    if (formatData.Vertical.HasValue)
                    {
                        XlVAlign verticalAlignment = (XlVAlign) formatData.Vertical.Value;
                        rng.VerticalAlignment = verticalAlignment;
                    }

                    if (formatData.IsPrintArea.HasValue && formatData.IsPrintArea.Value)
                    {
                        _sheet.PageSetup.PrintArea = rng.Address;
                        _sheet.DisplayPageBreaks = true;
                    }

                    if (formatData.ColumnWidths != null)
                    {
                        for (int c = 0; c < formatData.ColumnWidths.Length; ++c)
                        {

                            int col = range.Start.Column + c;

                            string colSymbol = ExcelSupport.GetColumnSymbol(col);

                            string colRange = string.Format("{0}:{1}", colSymbol, colSymbol);

                            int width = formatData.ColumnWidths[c];
                            _sheet.get_Range(colRange).EntireColumn.ColumnWidth = width;




                        }
                    }

                }
                catch (Exception exc)
                {

                }
            }
        }

        string getRangeName(Range range)
        {
            try
            {
                return range.Address[true, true, XlReferenceStyle.xlA1, false, null];
            }
            catch (Exception)
            {
                // ingored
            }

            return string.Empty;
        }

        #endregion

	}
}

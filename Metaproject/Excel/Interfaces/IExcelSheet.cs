using System;
using System.Collections;
using System.Collections.Generic;
using Metaproject.Dialog;
using Metaproject.Excel.Data;
using Microsoft.Win32;

namespace Metaproject.Excel
{
    public interface IExcelSheet
    {
        string Name { get;  }
        bool CheckIfMergedCells(int r1, int c1, int r2, int c2);
        void DeleteColumns(int c1, int c2);
        ExcelRangeInfo GetLastCellCoords();
        ExcelRangeInfo GetSelection();
        ExcelSheetData GetSheetData();
        ExcelSheetData GetSheetData(params int[] rowNumbers);
        void InsertSheetData(ExcelSheetData data);
        void MergeCells(int r1, int c1, int r2, int c2);
        void SetColumnsAutoFit(int c1, int c2);
        void SetName(string name);
        void SetPrintTitleRows(int firstRow, int lastRow);
        void SetRowHeight(int r, double h);
        void SetSelectionReset();
        void UnmergeCells(int r1, int c1, int r2, int c2);

        void InsertPivotTable(PivotTableData pivotTableData);
        void RefreshPivotRable(string name);
        void SetAutoFilter(int r1, int c1, int r2, int c2);
        void SetCellSelectionList(int r, int c, List<string> items);
        object GetValue(int r, int c);
        void SetValue(int r, int c, object value);
        void SetContextMenu(int r, int c, MenuItem contextMenu, string applicationId);
        void RemoveContextMenuItem(string applicationId);
        void SetCellName(int r, int c, string name);

        void ClearFilters();
        void SetFilter(ExcelRangeInfo rangeInfo, int field, string value);
    }
}

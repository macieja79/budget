using System;
using System.Collections.Generic;

namespace Metaproject.Excel
{
    public interface IExcelWorkbook
    {
        IExcelSheet GetActiveSheet();
        IExcelSheet GetSheet(string name);

        string Name { get; }

        void AddVbaModule(string module);
        string GetActiveSheetName();
        int GetWorksheetCount();
        List<string> GetWorksheetNames();
        void Save(string path);

        IExcelSheet CreateSheet(string name);
    }
}

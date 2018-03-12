using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Threading.Tasks;


using System.Reflection;
using Metaproject.Log;
using Metaproject.Patterns.EventAggregator;
using Microsoft.Office.Interop.Excel;

namespace Metaproject.Excel
{
	public class ExcelWorkbook : IExcelWorkbook
	{
		
		#region <members>
		Workbook _workbook;
	    private readonly IEventAggregator _aggregator;
	    private readonly ILogger _logger;

        List<IExcelSheet> _sheets = new List<IExcelSheet>();

	    #endregion

		#region <ctor>
		public ExcelWorkbook(Workbook workbook, IEventAggregator aggregator, ILogger logger)
		{
		    _workbook = workbook;
		    _aggregator = aggregator;
		    _logger = logger;
		}
		#endregion

		#region <pub>

	    public Workbook WrappedWorkbook
	    {
	        get { return _workbook; }
	    }

		public string GetActiveSheetName()
		{
			return (_workbook.ActiveSheet as Worksheet).Name;
		}

		public IExcelSheet GetActiveSheet()
		{
			Worksheet worksheet = (_workbook.ActiveSheet as Worksheet);

		    foreach (var wrapper in _sheets)
		    {
		        if (wrapper.Name == worksheet.Name)
                    return wrapper;
		    }

            return CreateNewWrapper(worksheet);
		}

	    private IExcelSheet CreateNewWrapper(Worksheet worksheet)
	    {
	        ExcelSheet newWrapper = new ExcelSheet(worksheet, this, _aggregator, _logger);
	        _sheets.Add(newWrapper);
	        return newWrapper;
	    }

	    public IExcelSheet GetSheet(string name)
	    {
	        foreach (var wrapper in _sheets)
	        {
	            if (wrapper.Name == name)
	            {
	                return wrapper;
	            }	            
	        }

            foreach (Worksheet sheet in _workbook.Sheets)
            {
                if (sheet.Name == name)
                {
                    return CreateNewWrapper(sheet);
                }
            }

	        return null;
	    }

	    public string Name => _workbook.Name;

	    public void Save(string path)
		{
			object[] args = { path };
			_workbook.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, _workbook, args);
		}

		public int GetWorksheetCount()
		{
			return _workbook.Sheets.Count;
		}

		public List<string> GetWorksheetNames()
		{
			List<string> names = new List<string>();
			foreach (Worksheet sheet in _workbook.Sheets)
			{
				names.Add(sheet.Name);
			}
			return names;
		}

        public IExcelSheet CreateSheet(string name)
        {
            Worksheet sheet = _workbook.Sheets.Add();
            sheet.Name = name;

            return CreateNewWrapper(sheet);
        }

        public void AddVbaModule(string codeText)
        {
            var newStandardModule = _workbook.VBProject.VBComponents.Add(Microsoft.Vbe.Interop.vbext_ComponentType.vbext_ct_StdModule);
            var codeModule = newStandardModule.CodeModule;
            var lineNum = codeModule.CountOfLines + 1;

            codeModule.InsertLines(lineNum, codeText);
        }




        #endregion

    }
}

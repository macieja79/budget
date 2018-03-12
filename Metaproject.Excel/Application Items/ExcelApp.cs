using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Metaproject.Log;
using Metaproject.Patterns.EventAggregator;
using Microsoft.Office.Interop.Excel;

namespace Metaproject.Excel
{
	public class ExcelApp : IExcelApp
	{
		#region <members>
		Application _application;
	    private IEventAggregator _aggregator;
	    private ILogger _logger;

        List<IExcelWorkbook> _workbooks = new List<IExcelWorkbook>();

        #endregion

        #region <ctor>
        public ExcelApp(Application application)
		{
			_application = application;
		    _application.SheetsInNewWorkbook = 1;
		}

		public ExcelApp()
		{

		}
		#endregion

		#region <pub>


		public void Connect()
		{

			_application = (Application)Marshal.GetActiveObject("Excel.Application");

		}

	    public void AttachAggregator(IEventAggregator aggregator, ILogger logger)
	    {
	        _aggregator = aggregator;
	        _logger = logger;
	    }

	    public void CreateNewInstance()
		{
			_application = new Application();
			SetExcelVisible(true);
		}

		public void SetExcelVisible(bool isVisible)
		{
			_application.Visible = isVisible;
		}

		public IExcelWorkbook LoadWorkbook(string path)
		{
			Workbook wb = _application.Workbooks.Open(path);
		    return CreateNewWorkbookWrapper(wb);

			
		}

	    private IExcelWorkbook CreateNewWorkbookWrapper(Workbook wb)
	    {
            var wrapper = new ExcelWorkbook(wb, _aggregator, _logger);
            _workbooks.Add(wrapper);
	        return wrapper;

	    }

	    public string GetActiveWorkbookName()
		{

			Workbook workbook = (Workbook)_application.ActiveWorkbook;

			if (null == workbook)
				return null;

			return workbook.Name;
		}

		public IExcelWorkbook GetActiveWorkbook()
		{
		    Workbook activeWorkbook = (Workbook) _application.ActiveWorkbook;


            foreach (var wrapper in _workbooks)
		    {
		        if (wrapper.Name == activeWorkbook.Name)
		        {
		            return wrapper;
		        }
		    }

            return CreateNewWorkbookWrapper(activeWorkbook);
			
		}

		public int GetWorkbooksCount()
		{
			return _application.Workbooks.Count;
		}


		public IExcelWorkbook CreateAndActivateNewWorkbook()
		{
			Workbook workbook = _application.Workbooks.Add(string.Empty);
			workbook.Activate();
		    return CreateNewWorkbookWrapper(workbook);
		}

        public void SetDisplayGridlines(bool isToDisplay)
        {
            _application.ActiveWindow.DisplayGridlines = isToDisplay;
        }

        public void SetZoom(double percent)
        {
            _application.ActiveWindow.Zoom = percent;
        }

	    public void SetScreenUpdating(bool isUpdate)
	    {
	        _application.ScreenUpdating = isUpdate;
	    }

	    public void SetDisplayZeros(bool isToDisplay)
        {
            _application.ActiveWindow.DisplayZeros = isToDisplay;
        }

        

		#endregion

	}
}

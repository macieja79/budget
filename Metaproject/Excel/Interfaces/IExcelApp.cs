using System;
using Metaproject.Log;
using Metaproject.Patterns.EventAggregator;

namespace Metaproject.Excel
{
    public interface IExcelApp
    {
        void Connect();
        void AttachAggregator(IEventAggregator aggregator, ILogger logger);
        IExcelWorkbook CreateAndActivateNewWorkbook();
        IExcelWorkbook GetActiveWorkbook();
        IExcelWorkbook LoadWorkbook(string path);
        
        void CreateNewInstance();
        string GetActiveWorkbookName();
        int GetWorkbooksCount();
        void SetDisplayGridlines(bool isToDisplay);
        void SetDisplayZeros(bool isToDisplay);
        void SetExcelVisible(bool isVisible);
        void SetZoom(double percent);
        void SetScreenUpdating(bool isUpdate);
    }
}

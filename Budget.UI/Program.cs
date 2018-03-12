using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Budget.Core;
using Budget.Core.Engine;
using Budget.Core.Entities;
using Budget.Core.Import;
using Budget.Excel;
using Budget.Import;
using Budget.UI.Forms;
using Metaproject.Dialog;
using Metaproject.Files;
using Metaproject.JSON;
using Metaproject.Log;
using Metaproject.Patterns;
using Metaproject.Patterns.EventAggregator;
using Metaproject.WinForms.Adapters;

namespace Budget.UI
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            EventAggregator eventAggregator = new EventAggregator();
            FormReportMain reportForm = new FormReportMain(eventAggregator, null);

            OutputLogger logger = new OutputLogger();
            ExcelOutput excelOutput = new ExcelOutput(eventAggregator, logger);
            excelOutput.Connect();



            ImportImpl import = new ImportImpl();

            
            IFileDialog fileDialog = new WinFormDialog();

            var categoryJsonSerializer = new JsonSerializer<CategoryCollection>();
            var categoryFileSerializer = new FileSerializer<CategoryCollection>(categoryJsonSerializer);

            var optionsJsonSerializer = new JsonSerializer<Options>();
            var optionsFileSerializer = new FileSerializer<Options>(optionsJsonSerializer);

            TransactionReportCollection reports = new TransactionReportCollection();
            var reportsJsonSerializer = new JsonSerializer<TransactionReportCollection>();
            var reportsFileSerializer = new FileSerializer<TransactionReportCollection>(reportsJsonSerializer);


            Options options = new Options()
            {
                CategoryPath = @"..\..\..\Budget.UnitTests\Resources\categories_new_16.bgt"/*,
                DocumentPath = @"..\..\..\Budget.UnitTests\Resources\Raport_Listopad_2016.rpt"*/
            };

            IBudgetRepository fileData = new BudgetRepository();
            fileData.Categories = new FileRepository<CategoryCollection>(categoryFileSerializer, fileDialog);
            fileData.Options = new FileRepository<Options>(options, optionsFileSerializer, fileDialog);
            fileData.Reports = new FileRepository<TransactionReportCollection>(reports, reportsFileSerializer,
                fileDialog);
            
            AppEngine engine = new AppEngine(excelOutput, eventAggregator, reportForm, import, fileData);
            Application.Run(reportForm);

        }
    }
}

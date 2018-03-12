using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Actions;
using Budget.Core.Commands;
using Budget.Core.Entities;
using Budget.Core.Events;
using Budget.Core.Excel;
using Budget.Core.Import;
using Budget.Core.Logic;
using Budget.Core.UI;
using Metaproject.Dialog;
using Metaproject.Files;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Patterns.EventAggregator.Events;
using Metaproject.Excel;

namespace Budget.Core.Engine
{
    public class AppEngine : ISubscriber<CommandSelectedAggregatedEvent>, 
        ISubscriber<ReportDeletedEvent>, 
        ISubscriber<ApplicationClosingAggEvent>, 
        ISubscriber<AggEventCategoriesNeeded>,
        ISubscriber<ExcelCommandSelectedAggEvent>
    {
        IExcel _IExcel; 
        IEventAggregator _iAggregator;
        IOutput _iOutput;
        IImport _import;
        private readonly IBudgetRepository _repository;
        

        public AppEngine(IExcel iExcel, IEventAggregator iAgregator, IOutput iOutput, IImport import, IBudgetRepository repository)
        {
            _IExcel = iExcel;
            _iAggregator = iAgregator;
            _iOutput = iOutput;
            _import = import;
            _repository = repository;

            _iAggregator.SubsribeEvent(this);
            
            string path = _repository.Options.Item.CategoryPath;
            string reportPath = _repository.Options.Item.DocumentPath;
            _repository.Categories.Load(path);
            _repository.Reports.Load(reportPath);
            _iOutput.ShowReports(_repository.Reports.Item.Reports);
            _iAggregator.PublishEvent(new PathChangedEvent() {Path = path});

            MenuItem menuStructure = MenuFactory.GetMenu();
            _iOutput.AttachMenu(menuStructure);

            ProceedEditRules();
        }

        #region <ISubscriber>

        public void OnEventHandler(CommandSelectedAggregatedEvent e)
        {
            if (e.CommandId == BudgetConst.COMMAND_IMPORT)
                ProceedImport();

            if (e.CommandId == BudgetConst.COMMAND_INSERT)
                ProceedInsert();

            if (e.CommandId == BudgetConst.COMMAND_RULES)
                ProceedAttachRules();

            if (e.CommandId == BudgetConst.COMMAND_INSERT_PIVOT)
                ProcedInsertPivot();

            if (e.CommandId == BudgetConst.COMMAND_EDIT_RULES)
                ProceedEditRules();

            if (e.CommandId == BudgetConst.COMMAND_IMPORT_EXCEL)
                ProceedImportExcel();

            if (e.CommandId == BudgetConst.COMMAND_UPDATE_EXCEL)
                ProceedUpdateExcel();

            if (e.CommandId == BudgetConst.COMMAND_LOAD_RULES)
                ProcedLoadRules();

            if (e.CommandId == BudgetConst.COMMAND_SAVE_RULES)
                ProceedSaveRules();

            if (e.CommandId == BudgetConst.COMMAND_LOAD)
                ProceedLoadReports();

            if (e.CommandId == BudgetConst.COMMAND_SAVE)
                ProceedSaveReports();

            if (e.CommandId == BudgetConst.COMMAND_SORT)
                ProceedSort();

            if (e.CommandId == BudgetConst.COMMAND_CLEAR_ITEM)
            {
                ProceedClearItem();
            }

            if (e.CommandId == BudgetConst.EXCEL_SHOW_ALL_DATA)
            {
                _IExcel.ClearFilters();
                return;
            }

            if (e.CommandId == BudgetConst.EXCEL_ATTACH_FILTER)
            {
                ExcelTransactionHitInfo hitInfo = _IExcel.GetTransactionHitInfo();
                _IExcel.SetFilter("xxx", hitInfo.Transaction.Category);
                return;
            }
        }

        private void ProceedClearItem()
        {
            Transaction selected = _iOutput.GetSelectedTransaction();
            selected.Category = "";
            selected.SubCategory = "";
            selected.IsEdited = false;

            _iOutput.Refresh();
        }

        private void ProceedSort()
        {
            var item = _repository.Categories.Item;

            TransactionComparer comparer = new TransactionComparer(_repository.Categories.Item);

            var reports = _repository.Reports.Item.Reports.ToList();
            foreach (var report in reports)
            {
                report.Transactions.Sort(comparer);
            }

            _iOutput.ShowReports(reports);
        }

        public void OnEventHandler(ExcelCommandSelectedAggEvent e)
        {
            if (e.CommandId == BudgetConst.EXCEL_ATTACH_RULE)
            {
                ExcelTransactionHitInfo hitInfo = _IExcel.GetTransactionHitInfo();
                return;
            }

            CommandSelectedAggregatedEvent fakeEvent = new CommandSelectedAggregatedEvent()
            {
                CommandId =  e.CommandId
            };

            OnEventHandler(fakeEvent);

        }




        public void OnEventHandler(ReportDeletedEvent e)
        {
            _repository.Reports.Item.Reports.RemoveWhere(r => r.Name == e.ReportName);
        }

        void ProceedImport()
        {
            List<string> paths = _iOutput.FilePathProvider.GetFilePaths();
            if (paths.IsNullObj()) return;
            
            List<FileData> fileDatas = new List<FileData>();
            FileTransactionDataProvider provider = new FileTransactionDataProvider();
            foreach (string path in paths)
            {
                provider.FilePath = path;
                FileData fileData = provider.GetTransactionData();
                fileDatas.Add(fileData);
            }

            GetTransactionsAction.Data getTransactionData = new GetTransactionsAction.Data()
            {
                FileDatas = fileDatas,
                Parsers = _import.GetParsers(),
                
            };
            GetTransactionsAction getTransaction = new GetTransactionsAction();
            GetTransactionsAction.Result getTransactionResult = getTransaction.Execute(getTransactionData);

            var transactions = getTransactionResult.Reports.SelectMany(r => r.Transactions).ToList();
            AttachRulesAction.Data attachRulesData = new AttachRulesAction.Data
            {
                Transactions = transactions,
                Categories = _repository.Categories.Item
            };

            AttachRulesAction attachRulesAction = new AttachRulesAction();
            attachRulesAction.Execute(attachRulesData);

            TransactionManager transactionManager = new TransactionManager();
            List<TransactionReport> reports = transactionManager.GetMonthReports(transactions);
            transactionManager.RemoveInternalTransactions(reports);

            _repository.Reports.Item.Reports = reports;
            _iOutput.ShowReports(reports);
        }

        private void ProceedLoadReports()
        {
            LoadSaveDialogOptions options = new LoadSaveDialogOptions()
            {
                Caption = "Select file",
                Filter = FileTools.Instance.GetFileFilter("Pliki raportow", "rpt"),
                IsMultiselect = false
            };

            _repository.Reports.Load(options);
            _iOutput.ShowReports(_repository.Reports.Item.Reports);
        }

        private void ProceedSaveReports()
        {
            LoadSaveDialogOptions options = new LoadSaveDialogOptions()
            {
                Caption = "Select file",
                Filter = FileTools.Instance.GetFileFilter("Pliki raportow", "rpt"),
                IsMultiselect = false
            };
            
            _repository.Reports.Save(options);
        }

        void ProceedInsert()
        {
            var data = new InsertTransactionAction.Data()
            {
                ExcelInstance = _IExcel,
                Reports = _repository.Reports.Item.Reports
            };

            var action = new InsertTransactionAction();
            action.Execute(data);

            return;
        }

        void ProceedEditRules()
        {

            CategoryCollection edited;
            string path;

            bool isEdited = _iOutput.EditCategories(_repository.Categories, out edited, out path);
            if (!isEdited) return;

            _repository.Categories.Update(edited);
            if (path.IsNotNull())
            {
                _repository.Options.Item.CategoryPath = path;
            }

            ProceedAttachRules();

        }
        
        void ProcedInsertPivot()
        {
            InsertPivotTableAction.Data data = new InsertPivotTableAction.Data()
            {
                Excel = _IExcel
            };

            InsertPivotTableAction insertPivotTableAction = new InsertPivotTableAction();
            insertPivotTableAction.Execute(data);
        }

        void ProceedAttachRules()
        {

            if (_repository.Reports.Item.Reports.IsNullObj()) return;


            List<Transaction> transactions = _repository.Reports.Item.Reports.SelectMany(r => r.Transactions).ToList();
            AttachRulesAction.Data data = new AttachRulesAction.Data
            {

                Transactions = transactions,
                Categories = _repository.Categories.Item,
                IsToOverrideExisting = true
            };
            AttachRulesAction attachRulesAction = new AttachRulesAction();
            attachRulesAction.Execute(data);

            _iOutput.ShowReports(_repository.Reports.Item.Reports);

        }

        void ProceedImportExcel()
        {


            List<string> names = _IExcel.GetSheetNames();
            List<string> checkedNames = names.ToList();

            SelectEntitiesDialogOptions options = new SelectEntitiesDialogOptions()
            {
                Caption = "Arkusze",
                Description = "Wybierz arkusze",
                Options = names,
                Checked = checkedNames
            };
            
            List<string> selectedNames;
            bool isOk = _iOutput.FileDialog.ShowSelectEntitiesDialog(options, out selectedNames);
            if (!isOk) return;

            List<ITransactionParser> parsers = _import.GetParsers();
                new List<ITransactionParser>();
        
            GetExcelTransactionsAction.Data getExcelData = new GetExcelTransactionsAction.Data()
            {
                SheetFileNames = selectedNames,
                Excel = _IExcel,
                Parsers = parsers
            };

            GetExcelTransactionsAction getExcelAction = new GetExcelTransactionsAction();
            GetTransactionsAction.Result result = getExcelAction.Execute(getExcelData);

            _repository.Reports.Item.Reports = result.Reports;
            _iOutput.ShowReports(result.Reports);

        }

        void ProceedUpdateExcel()
        {

            var data = new UpdateTransactionAction.Data()
            {
                ExcelInstance = _IExcel,
                Reports = _repository.Reports.Item.Reports
            };

            var action = new UpdateTransactionAction();
            action.Execute(data);

            return;

        }


        private void ProcedLoadRules()
        {
            LoadSaveDialogOptions options = new LoadSaveDialogOptions();
            options.IsMultiselect = false;

            List<string> paths;
            bool isOk = _iOutput.FileDialog.ShowLoadDialog(options, out paths);
            if (!isOk) return;

            string path = paths.FirstOrDefault();
            if (path.IsNullObj()) return;
            
            
            _repository.Categories.Load(path);
            _repository.Options.Item.CategoryPath = path;
        }


        private void ProceedSaveRules()
        {
            
        }

       

        



        #endregion

        public void OnEventHandler(ApplicationClosingAggEvent e)
        {

            if (_repository.Categories.IsChanged)
            {
                string msg = $"Plik {_repository.Categories.Path} zmienił się, czy wyjść?";
                bool isYes = _iOutput.FileDialog.ShowQuestionDialog(msg, "Pytanie");
                if (!isYes)
                {
                    e.IsCancel = true;
                }
            }
        }

        public void OnEventHandler(AggEventCategoriesNeeded e)
        {
            e.Categories = _repository.Categories.Item;
        }

        
    }

}

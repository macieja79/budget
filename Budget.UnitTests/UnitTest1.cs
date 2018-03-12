using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Budget.Core;
using Budget.Import;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using Budget.Core.Entities;
using System.Linq;
using System.Windows.Forms;
using Budget.UI;
using Budget.Core.Actions;
using Budget.Core.Import;
using Budget.Core.Logic;
using Budget.Excel;
using Budget.UI.Forms;
using Metaproject.Dialog;
using Metaproject.Files;
using Metaproject.JSON;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Reflection;
using Metaproject.WinForms.Adapters;
using Metaproject.WinForms.Tools;

namespace Budget.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        public object FormReports { get; private set; }

        [TestMethod]
        public void ShowReportOnUI()
        {
            List<TransactionReport> reports = TestDataFactory.GetMonthReports();
            FormReportMain form = new FormReportMain(EventAggregator.Empty, null);
            form.ShowReports(reports);
            form.ShowDialog();
        }

        [TestMethod]
        public void ShowNoCategoryItemsOnUI()
        {
            List<TransactionReport> reports = TestDataFactory.GetMonthReports();

            foreach (TransactionReport report in reports)
            {
                report.Transactions.RemoveWhere(t => t.Category.IsNotNull() && t.SubCategory.IsNotNull());
            }

            FormReportMain form = new FormReportMain(EventAggregator.Empty, null);
            form.ShowReports(reports);
            form.ShowDialog();
        }

        [TestMethod]
        public void ExportReportsToExcel()
        {
            List<TransactionReport> reports = TestDataFactory.GetMonthReports();
            ExcelOutput excel = new ExcelOutput();
            excel.InsertReportToNewSheet(reports, null);
        }

        [TestMethod]
        public void ExportSingleReportToExcel()
        {
            List<TransactionReport> reports = TestDataFactory.GetMonthReports();
            List<TransactionReport> single = reports.GetRange(3, 1);
            ExcelOutput excel = new ExcelOutput();
            excel.InsertReportToNewSheet(single, null);
        }

        [TestMethod]
        public void AttachPivotTable()
        {
            ExcelOutput excel = new ExcelOutput();
            InsertPivotTableAction.Data actionData = new InsertPivotTableAction.Data {Excel = excel};
            InsertPivotTableAction action = new InsertPivotTableAction();
            action.Execute(actionData);
        }

        [TestMethod]
        public void RefreshPivotTable()
        {
            ExcelOutput excel = new ExcelOutput();
            RefreshPivotTableAction.Data actionData = new RefreshPivotTableAction.Data {Excel = excel};
            RefreshPivotTableAction action = new RefreshPivotTableAction();
            action.Execute(actionData);
        }

        [TestMethod]
        public void ShowRulesOnUI()
        {


            //RulesForm form = new RulesForm(EventAggregator.Empty);
            //TestRulesRepository repo = new TestRulesRepository();

            //List<Rule> rules = repo.GetRules().Cast<Rule>().ToList();
            //form.DisplayRules(rules);
            //form.ShowDialog();
        }

        [TestMethod]
        public void GetCategoriesFromString()
        {

            var categories = TestDataFactory.GetCategoriesFromStr();

            Debug.Assert(categories.Any());

        }

        [TestMethod]
        public void SaveCategoryCollection()
        {
            string testPath = @"..\..\Resources\categories.bgt";

            CategoryCollection savedCategories = TestDataFactory.GetCategoryCollectionFromStr();
            JsonSerializer<CategoryCollection> serializer = new JsonSerializer<CategoryCollection>();

            FileTools.Instance.Save(savedCategories, serializer, testPath);

            CategoryCollection loadedCategories = FileTools.Instance.Load(serializer, testPath);

            Assert.AreEqual(savedCategories.Categories.Count, loadedCategories.Categories.Count);
        }

        [TestMethod]
        public void LoadCategoryCollection()
        {
            string testPath = @"..\..\Resources\categories.bgt";
            JsonSerializer<CategoryCollection> serializer = new JsonSerializer<CategoryCollection>();
            CategoryCollection loadedCategories = FileTools.Instance.Load(serializer, testPath);

            CategoryTreeEditor editor = new CategoryTreeEditor();

            // FileRepository<CategoryCollection> categoryRepo = new FileRepository<CategoryCollection>(loadedCategories, new FileSerializer<CategoryCollection>(JsonSerializer<CategoryCollection>());
            //editor.AttachCategories(loadedCategories);
            //editor.ShowDialog();
        }


        [TestMethod]
        public void CloneCategoryCollection()
        {
            string testPath = @"..\..\Resources\categories.bgt";
            JsonSerializer<CategoryCollection> serializer = new JsonSerializer<CategoryCollection>();
            CategoryCollection loadedCategories = FileTools.Instance.Load(serializer, testPath);
            CategoryCollection clone = new CategoryCollection();
            
            PropertyMapper.Instance.MapProperties(loadedCategories, clone);

            loadedCategories.Categories.Remove(loadedCategories.Categories.First());



        }

        [TestMethod]
        public void ParseExcelTransactions()
        {
            ExcelOutput excel = new ExcelOutput();
            excel.Connect();

            List<string> names = excel.GetSheetNames();
            List<string> checkedNames = names.First().AsList();

            if (false)
            {

                WinFormDialog dialog = new WinFormDialog();
                SelectEntitiesDialogOptions options = new SelectEntitiesDialogOptions()
                {
                    Caption = "Arkusze",
                    Description = "Wybierz arkusze",
                    Options = names,
                    Checked = checkedNames
                };

                List<string> selectedNames;
                bool isOk = dialog.ShowSelectEntitiesDialog(options, out selectedNames);
                if (!isOk) return;


            }

            List<ITransactionParser> parsers = new List<ITransactionParser>();
            parsers.Add(new ExcelSheetParser());

            GetExcelTransactionsAction.Data getExcelData = new GetExcelTransactionsAction.Data()
            {
                SheetFileNames = checkedNames,
                Excel = excel,
                Parsers = parsers
            };

            GetExcelTransactionsAction getExcelAction = new GetExcelTransactionsAction();

            GetTransactionsAction.Result result = getExcelAction.Execute(getExcelData);



        }

        [TestMethod]
        public void ParseSingleSheetExcelTransaction()
        {

            string file = Properties.Resources.transactionsToParseToCategories;
            List<string> lines =
                file.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

            ExcelSheetParser parser = new ExcelSheetParser();
            TransactionReport report;
            FileData fileData = new FileData() {Lines = lines};
            parser.TryGetMonthReports(fileData, out report);

        }

        [TestMethod]
        public void ShowCategories()
        {

            
            WinFormDialog winForms = new WinFormDialog();
            JsonSerializer<CategoryCollection> objectSerializer = new JsonSerializer<CategoryCollection>();
            FileSerializer<CategoryCollection> fileSerializer = new FileSerializer<CategoryCollection>(objectSerializer);
            FileRepository<CategoryCollection> fileRepository = new FileRepository<CategoryCollection>(null, fileSerializer, winForms);

            string testPath = @"..\..\Resources\categories.bgt";
            fileRepository.Load(testPath);

            CategoryControl control = new CategoryControl();
            EventAggregator aggregator = new EventAggregator();
            control.AttachCategories(fileRepository, aggregator);

            Form form = FormTools.GetFormForControl(control);
            form.ShowDialog();


        }


    }
}
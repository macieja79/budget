using Budget.Core;
using Budget.Core.Actions;
using Budget.Core.Entities;
using Budget.Core.Logic;
using Budget.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.PropertyGridInternal;
using Budget.Core.Import;
using Budget.UI;
using Budget.UnitTests.Properties;
using Metaproject.Patterns.EventAggregator.Events;


namespace Budget.UnitTests
{
    public class TestDataFactory
    {

        public static List<TransactionReport> GetMonthReports()
        {

            ImportImpl import = new ImportImpl();
            TestDataProvider ingDataProvider = new TestDataProvider(Resources._2018_06_All, "\n");
            FileData ingFileData = ingDataProvider.GetTransactionData();

            TestDataProvider mobileDataProvider = new TestDataProvider(Resources.czerwiec_2016_Maciek, "\n");
            FileData mobileFileData = mobileDataProvider.GetTransactionData();

            List<FileData> filedataCollection = new List<FileData>() { ingFileData, mobileFileData };

            GetTransactionsAction.Data getTransactionData = new GetTransactionsAction.Data()
            {
                Parsers   =  import.GetParsers(),
                FileDatas = filedataCollection
            };

            GetTransactionsAction getTransaction = new GetTransactionsAction();
            GetTransactionsAction.Result getTransactionResult = getTransaction.Execute(getTransactionData);

            var allTransactions = getTransactionResult.Reports.
                SelectMany(r => r.Transactions).ToList();

                
            AttachRulesAction.Data attachRulesData = new AttachRulesAction.Data
            {
                Transactions = allTransactions
                /*Rules = GetTestRules()*/
            };

            AttachRulesAction attachRulesAction = new AttachRulesAction();
            attachRulesAction.Execute(attachRulesData);

            TransactionManager transactionManager = new TransactionManager();
            List<TransactionReport> reports = transactionManager.GetMonthReports(allTransactions);
            transactionManager.RemoveInternalTransactions(reports);

            return reports;

        }

        public static List<Rule> GetTestRules()
        {
            TestRulesRepository testRulesRepo = new TestRulesRepository();
            return testRulesRepo.GetRules();
        }

        public static List<Category> GetCategoriesFromStr()
        {
            TestRulesRepository repo = new TestRulesRepository();
            var categories = repo.GetCategoriesFromFile(_categoryRawList);
            return categories;
        }

        public static CategoryCollection GetCategoryCollectionFromStr()
        {
            //TestRulesRepository repo = new TestRulesRepository();
            //var categories = repo.GetCategories(_categoryRawList);
            //return categories;
            return null;
        }



        static string _categoryRawList = @"
AUTO: PALIWO,SERWIS
JEDZENIE: MARKET, RESTAURACJA
DOM: INTERNET, WODA, CZYNSZ, GAZ, PRAD, TELEFON, SPRZET, KREDYT, ELEKTRONIKA
FINANSE: SOCJAL, BANKOMAT, OSZCZĘDNOŚCI, PROWIZJE
ZUS: ZDROWOTNE, SPOLECZNE, FUNDUSZ
KULTURA: KSIĄŻKI
CIUCHY: RESERVED
FV: ANTAL, GSBK, GSBK, DPK, ANGLOKOM
DZIECI: PRZEDSZKOLE
TAX: PIT, VAT
ZDROWIE: LEKI, WELLNESS, DENTYSTA
SPORT: SPRZET, BASEN
ROZRYWKA: KNAJPA
";

    }
}

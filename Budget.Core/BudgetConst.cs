using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metaproject.Files;

namespace Budget.Core
{
    public class BudgetConst
    {
        public const string CategoryInternal = "WEWNETRZNY";
        public const string TransactionIncome = "PRZYCHÓD";
        public const string TransactionCost = "WYDATEK";

        public const string COMMAND_IMPORT = "COMMAND_IMPORT";
        public const string COMMAND_RULES = "COMMAND_RULES";
        public const string COMMAND_INSERT = "COMMAND_INSERT";
        public const string COMMAND_INSERT_PIVOT = "COMMAND_INSERT_PIVOT";
        public const string COMMAND_EDIT_RULES = "COMMAND_EDIT_RULES";
        public const string COMMAND_IMPORT_EXCEL = "COMMAND_IMPORT_EXCEL";
        public const string COMMAND_UPDATE_EXCEL = "COMMAND_UPDATE_EXCEL";
        public const string COMMAND_LOAD_RULES = "COMMAND_UPDATE_EXCEL";
        public const string COMMAND_SAVE_RULES = "COMMAND_SAVE_RULES";
        public const string COMMAND_LOAD = "COMMAND_LOAD";
        public const string COMMAND_SAVE = "COMMAND_SAVE";
        public const string COMMAND_SORT = "COMMAND_SORT";
        
        public const string EXCEL_ATTACH_RULE = "EXCEL_ATTACH_RULE";
        public const string EXCEL_SHOW_ALL_DATA = "EXCEL_SHOW_ALL_DATA";

        public const string EXCEL_ATTACH_FILTER = "EXCEL_ATTACH_FILTER";


        public static readonly string FILE_EXTENSION = FileTools.Instance.GetFileFilter("Pliki z kategoriami", "bgt");
        public static string COMMAND_CLEAR_ITEM = "COMMAND_CLEAR_ITEM";
    }
}

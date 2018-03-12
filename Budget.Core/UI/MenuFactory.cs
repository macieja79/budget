using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metaproject.Dialog;

namespace Budget.Core.UI
{
    public class MenuFactory
    {
        public static MenuItem GetMenu()
        {

            MenuItem main = new MenuItem
            {
                Caption = "Main",
                Children =
                {
                    new MenuItem()
                    {
                        Caption = "Report",
                        Children =
                        {
                            new MenuItem("Load", BudgetConst.COMMAND_LOAD),
                            new MenuItem("Save", BudgetConst.COMMAND_SAVE),
                            new MenuItem("Import", BudgetConst.COMMAND_IMPORT),
                            new MenuItem("Sort", BudgetConst.COMMAND_SORT)
                        }
                    },

                    new MenuItem()
                    {
                        Caption = "Actions",
                        Children =
                        {
                            new MenuItem("Attach rules", BudgetConst.COMMAND_RULES),
                            new MenuItem("Clear item", BudgetConst.COMMAND_CLEAR_ITEM)
                        }
                    },

                    new MenuItem()
                    {
                        Caption = "Excel",
                        Children =
                        {
                            new MenuItem("Insert", BudgetConst.COMMAND_INSERT),
                            new MenuItem("Pivot", BudgetConst.COMMAND_INSERT_PIVOT),
                            new MenuItem("Import from Excel", BudgetConst.COMMAND_IMPORT_EXCEL),
                            new MenuItem("Update Excel", BudgetConst.COMMAND_UPDATE_EXCEL),
                            new MenuItem("Clear filters", BudgetConst.EXCEL_SHOW_ALL_DATA),
                            new MenuItem("Attach filter", BudgetConst.EXCEL_ATTACH_FILTER)
                        }
                    },
                }
            };

            return main;

        }

    }
}

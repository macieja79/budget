using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;
using Budget.UI;
using Metaproject.Dialog;
using Metaproject.Excel;
using Metaproject.Patterns.EventAggregator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Budget.UnitTests
{
    [TestClass]
    public class ExcelTests
    {

        [TestMethod]
        public void Excel_Context_Menu_Tests()
        {
            IExcelApp app = new ExcelApp();
            app.CreateNewInstance();

            IExcelWorkbook workbook = app.CreateAndActivateNewWorkbook();
            IExcelSheet sheet = workbook.CreateSheet("Test");

            MenuItem contextMenu = new MenuItem {ItemType = MenuItemType.ContextMenu};
            contextMenu.Children.Add(new MenuItem {Caption = "First Command", CommandId = "FC01"});
            contextMenu.Children.Add(new MenuItem {ItemType = MenuItemType.Separator});
            contextMenu.Children.Add(new MenuItem { Caption = "Second Command", CommandId = "FC01" });

            sheet.SetContextMenu(2,2, contextMenu, "Application.Id");
        }


    }
}

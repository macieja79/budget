using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MenuItem = Metaproject.Dialog.MenuItem;

namespace Metaproject.WinForms.Tools
{
    public class FormTools
    {
        public static Form GetFormForControl(Control control)
        {
            Form form = new Form();
            form.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            return form;
        }

        public static void CreateMenu(MenuStrip menu, MenuItem menuData, EventHandler clickEventHandler)
        {
            AddMenuItem(menu.Items, menuData.Children, clickEventHandler);
        }

        static void AddMenuItem(ToolStripItemCollection collection, List<MenuItem> items, EventHandler clickEventHandler)
        {
            foreach (MenuItem item in items)
            {
                if (item.Children.Any())
                {
                    ToolStripDropDownItem dropDownItem = new ToolStripMenuItem(item.Caption);
                    collection.Add(dropDownItem);
                    AddMenuItem(dropDownItem.DropDownItems, item.Children, clickEventHandler);
                }
                else
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(item.Caption);
                    menuItem.Tag = item.CommandId;
                    menuItem.Click += clickEventHandler;
                    collection.Add(menuItem);
                }
            }
        }

        
    }
}

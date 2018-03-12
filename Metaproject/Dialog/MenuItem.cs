using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Dialog
{
    public class MenuItem
    {
        public MenuItem()
        {
            
        }

        public MenuItem(string caption, string commandId)
        {
            Caption = caption;
            CommandId = commandId;
        }

        public List<MenuItem> Children = new List<MenuItem>();
        public string Caption { get; set; }
        public string CommandId { get; set; }
        public bool IsEnabled { get; set; }

        public MenuItemType ItemType { get; set; } = MenuItemType.Commmand;
    }
}

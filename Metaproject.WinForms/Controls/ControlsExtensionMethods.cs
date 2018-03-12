using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.Controls
{
    public static class ControlsExtensionMethods
    {

        public static bool IsEnabledAndChecked (this CheckBox chb)
        {
            return chb.Visible && chb.Enabled && chb.Checked;


        }

        public static List<TabPage> AsList(this TabControl.TabPageCollection collection)
        {
            List<TabPage> pages = new List<TabPage>();
            foreach (TabPage page in collection)
                pages.Add(page);

            return pages;


        }

        public static List<RowStyle> AsList(this TableLayoutRowStyleCollection colection)
        {
            List<RowStyle> styles = new List<RowStyle>();
            foreach (RowStyle style in colection)
                styles.Add(style);

            return styles;

        }

     





    }
}

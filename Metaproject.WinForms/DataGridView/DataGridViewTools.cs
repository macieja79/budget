using Metaproject.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.DataGrid
{
    public class DataGridViewTools
    {

        public static void PasteClipboard(DataGridView dgData)
        {
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int iFail = 0, iRow = dgData.CurrentCell.RowIndex;
                int iCol = dgData.CurrentCell.ColumnIndex;

                if (dgData.Rows.Count < lines.Length)
                    dgData.Rows.Add(lines.Length - dgData.Rows.Count);

                DataGridViewCell oCell;
                foreach (string line in lines)
                {
                    if (iRow < dgData.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < dgData.ColumnCount)
                            {
                                oCell = dgData[iCol + i, iRow];
                                if (!oCell.ReadOnly)
                                {
                                    //if (oCell.Value.ToString() != sCells[i])
                                    //{


                                    string convertedValue = sCells[i].Replace("\r", "");
                                    oCell.Value = Convert.ChangeType(convertedValue,
                                                              oCell.ValueType);

                                    //}
                                    //else
                                    //    iFail++;
                                    //only traps a fail if the data has changed 
                                    //and you are pasting into a read only cell
                                }
                            }
                            else
                            { break; }
                        }
                        iRow++;
                    }
                    else
                    { break; }
                    if (iFail > 0)
                        MessageBox.Show(string.Format("{0} updates failed due" +
                                        " to read only column setting", iFail));
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }

        public static Bitmap CaptureDataGrid(DataGridView datagridview)
        {

            int tableHeight = 0;
            int tableWidth = 0;

            tableHeight += datagridview.Rows.GetRowsHeight(DataGridViewElementStates.Displayed)
                + datagridview.ColumnHeadersHeight;

            tableWidth += datagridview.Columns.GetColumnsWidth(DataGridViewElementStates.Displayed)
                + datagridview.RowHeadersWidth;

            tableHeight = Math.Min(tableHeight, datagridview.Height);
            tableWidth = Math.Min(tableWidth, datagridview.Width);

            Bitmap bitmap = ControlsTools.CaptureControl(datagridview);

            Bitmap newBitmap = new Bitmap(tableWidth, tableHeight);

            Graphics g = Graphics.FromImage(newBitmap);

            g.DrawImageUnscaledAndClipped(bitmap, new Rectangle(1, 1, newBitmap.Width, newBitmap.Height));

            return newBitmap;

        }

        public static void SetDoubleBuffer(DataGridView grid, bool isSet)
        {
            Type dgvType = grid.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(grid, isSet, null);
        

        }
        
    }
}

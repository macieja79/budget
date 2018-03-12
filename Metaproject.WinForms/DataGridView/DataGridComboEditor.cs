using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Metaproject.Patterns.EventAggregator;
using Metaproject.WinForms;


namespace Metaproject.DataGrid
{
    public class DataGridComboEditor : IDisposable  
    {
        ComboBox _combo;
        DataGridView _grid;

        object _item;
        string _propertyName;
        private bool _isEscaping;

        private IEventAggregator _aggregator;

        public DataGridComboEditor(DataGridView grid, IEventAggregator aggregator = null)
        {
            _grid = grid;
            _combo = new ComboBox();
            _combo.Visible = false;

            if (aggregator == null)
                _aggregator = EventAggregator.Empty;
            else
                _aggregator = aggregator;

            Initalize(true);
        }


        void Initalize(bool isStart)
        {
            Form form = _grid.FindForm();
            if (form.IsNullObj()) return;

            if (isStart)
            {

                form.Controls.Add(_combo);
                _combo.LostFocus += comboBox1_LostFocus;
                _combo.SelectedValueChanged += comboBox1_SelectedValueChanged;
                _combo.KeyDown += comboBox1_KeyDown;
            }
            else
            {
                form.Controls.Remove(_combo);
                _combo.LostFocus -= comboBox1_LostFocus;
                _combo.SelectedValueChanged -= comboBox1_SelectedValueChanged;
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _isEscaping = true;
                _combo.Visible = false;
                _combo.Invalidate();
                _grid.Invalidate();
                _grid.Focus();
            }
        }


        public void Show_Combobox(int iRowIndex, int iColumnIndex, List<string> items, string current, object item,
            string propertyName)
        {
            // DESCRIPTION: SHOW THE COMBO BOX IN THE SELECTED CELL OF THE GRID.
            // PARAMETERS: iRowIndex - THE ROW ID OF THE GRID.
            //             iColumnIndex - THE COLUMN ID OF THE GRID.

            _isEscaping = false;

            _item = item;
            _propertyName = propertyName;

            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;

            // GET THE ACTIVE CELL'S DIMENTIONS TO BIND THE COMBOBOX WITH IT.
            Rectangle rect = default(Rectangle);

            int deltaX = 0;
            int deltaY = 0;
            TabPage tabPage = _grid.Parent as TabPage;
            if (null != tabPage)
            {
                TabControl tabControl = tabPage.Parent as TabControl;
                if (null != tabPage && null != tabControl)
                {
                    deltaX = tabControl.Left + tabPage.Left;
                    deltaY = tabControl.Top + tabPage.Top;
                }
            }
            else
            {
                GetDeltas(_grid.Parent, ref deltaX, ref deltaY);
            }

            rect = _grid.GetCellDisplayRectangle(iColumnIndex, iRowIndex, false);
            x = rect.X + _grid.Left + deltaX;
            y = rect.Y + _grid.Top + deltaY; 

            width = rect.Width;
            height = rect.Height;

            if (null == _combo)
                _combo = new ComboBox();

            _combo.DataSource = new BindingList<string>(items);
            _combo.SelectedItem = current;

            _combo.SetBounds(x, y, width, height);
            _combo.BringToFront();
            _combo.Visible = true;
            _combo.DropDownStyle = ComboBoxStyle.DropDownList;
            _combo.DroppedDown = true;
            _combo.Focus();
            
        }


        void GetDeltas(Control control, ref int x, ref int y)
        {

            Control parent = control.Parent;
            if (parent.IsNullObj()) return;

            x += control.Left;
            y += control.Top;

           
            if (parent.IsNotNullObj())
            {
                GetDeltas(parent, ref x, ref y);
            }
        }

        void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_isEscaping) return;
            

            _combo.Invalidate();
            _grid.Invalidate();
            _grid.Focus();
        }

        void comboBox1_LostFocus(object sender, EventArgs e)
        {
            _combo.Visible = false;
            if (_isEscaping) return;

            PropertyInfo propInfo = _item.GetType().GetProperty(_propertyName);

            object oldValue = propInfo.GetValue(_item);

            object value = _combo.SelectedItem;

            propInfo.SetValue(_item, value, null);

            ComboValueSelectedAggEvent valueSelectedEvent = new ComboValueSelectedAggEvent()
            {
                EditedObject = _item,
                NewValue = value as string,
                OldValue = oldValue as string,
                PropertyName = _propertyName
            };
            _aggregator.PublishEvent<ComboValueSelectedAggEvent>(valueSelectedEvent);
        }

        public void Dispose()
        {
            Initalize(false);
        }
    }
}

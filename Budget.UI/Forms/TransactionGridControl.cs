using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Budget.Core.Entities;
using Budget.UI.ViewModels;
using Metaproject.DataGrid;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Forms;
using Metaproject.Patterns.EventAggregator.Events;

namespace Budget.UI.Forms
{
    public partial class TransactionGridControl : UserControl
    {
        public TransactionGridControl()
        {
            InitializeComponent();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.SetImageColumn("Color");
            dataGridView1.SetImageColumn("ColumnIsEdited");

            DataGridViewTools.SetDoubleBuffer(dataGridView1, true);
            
        }

        TransactionGridModel _gridModel;

        private IEventAggregator _aggregator;

        public void DisplayTransactions(List<Transaction> transactions, IEventAggregator aggregator)
        {
            if (null == _aggregator)
            {
                _aggregator = aggregator;
            }

            if (_gridModel.IsNullObj())
            {
                _gridModel = new TransactionGridModel(dataGridView1, null, _aggregator);
                gridNavigator1.AttachTreeView(_gridModel);
            }

            _gridModel.UpdateModel(transactions);
        }

        public int GetRow()
        {
            return dataGridView1.FirstDisplayedCell.RowIndex;
        }

        public void SetRow(int row)
        {
            var cell = dataGridView1[0, row];
            dataGridView1.FirstDisplayedCell = cell;
        }

        public Transaction GetSelectedTransaction()
        {
            return _gridModel.SelectedItem.Item;

        }
        
    }
}

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
    public partial class RulesGridControl : UserControl, ISubscriber<AggregatedEvent>
    {

        private RuleGridModel _gridModel;
        private IEventAggregator _aggregator;


        public RulesGridControl()
        {
            InitializeComponent();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowDrop = true;
            dataGridView1.SetImageColumn("Color");

            

        }

        public void AttachEventAggregator(IEventAggregator eventAggregator)
        {
            _aggregator = eventAggregator;
            _gridModel = new RuleGridModel(dataGridView1, _aggregator);
            gridNavigator1.AttachTreeView(_gridModel);
        }


        public void DisplayRules(List<Rule> rules)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            _gridModel.UpdateModel(rules);
        }

        public void OnEventHandler(AggregatedEvent e)
        {
            if (e.Name == "REFRESH")
            {
                dataGridView1.Refresh();
            }
        }
        
    }
}

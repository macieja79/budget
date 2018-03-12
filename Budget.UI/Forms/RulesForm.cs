using Budget.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Metaproject.Forms;
using Metaproject.Patterns.EventAggregator;
using Rule = Budget.Core.Entities.Rule;

namespace Budget.UI.Forms
{
    public partial class RulesForm : Form , IStandardEventListener
    {
        private readonly IEventAggregator _eventAggregator;

        public RulesForm(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();

            EventAttacher.Instance.AttachEvents(this, this);

        }

        List<Rule> _originalRules = new List<Rule>();
        List<Rule> _clonedRules = new List<Rule>();

        public void DisplayRules(List<Rule> rules)
        {

            _originalRules = rules;
            foreach (var rule in rules)
            {
                _clonedRules.Add((Rule)rule.Clone());
            }

            rulesGridControl1.DisplayRules(rules);
        }

        public void OnButtonClick(object sender, EventArgs e)
        {
            if (sender == button2)
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            if (sender == button1)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

        }

        public void OnCheckedChanged(object sender, EventArgs e)
        {
            
        }
    }

    
}

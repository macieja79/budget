using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Budget.Core;
using Budget.Core.Engine;
using Budget.Core.Entities;
using Budget.Core.Events;
using Budget.Core.Import;
using Budget.Core.UI;
using Metaproject.Forms;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Patterns.EventAggregator.Events;
using Budget.Import;
using Budget.UI.Forms;
using Metaproject.DataGrid;
using Metaproject.Dialog;
using DialogResult = System.Windows.Forms.DialogResult;
using Rule = Budget.Core.Entities.Rule;

namespace Budget.UI
{
    public partial class FormReports : Form, IStandardEventListener, IOutput    
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly AppEngine _engine;
        private TabControl _tabContainer;

        public FormReports(IEventAggregator eventAggregator, AppEngine engine)
        {
            _eventAggregator = eventAggregator;
            _engine = engine;

           
            FilePathProvider = new FilePathProvider();
            RulesProvider = new TestRulesRepository();
            TransactionParser = new IngCsvFileParser();

            InitializeComponent();
            EventAttacher.Instance.AttachEvents(this, this);
            
        }

        #region <IStandardEventListener>

        public void OnButtonClick(object sender, EventArgs e)
        {
            var control = sender as ToolStripButton;
            if (control != null)
            {
                var commandId = control.Tag as string;
                _eventAggregator.PublishEvent<CommandSelectedAggregatedEvent>(CommandSelectedAggregatedEvent.Create(commandId));
            }
        }

        public void OnCheckedChanged(object sender, EventArgs e)
        {
        
        }

        #endregion  

        #region <IOutput>

        public void ShowReports(List<TransactionReport> reports)
        {

            if (null == _tabContainer)
            {
                _tabContainer = new TabControl {Dock = DockStyle.Fill};
                _tabContainer.KeyUp += TabContanerOnKeyUp;
                this.toolStripContainer1.ContentPanel.Controls.Add(_tabContainer);
            }

            UpdateRecords(reports);

            
        }

        public bool EditRules(List<Rule> rules)
        {
            RulesForm editForm = new RulesForm(_eventAggregator);
            editForm.DisplayRules(rules);
            bool isOk = editForm.ShowDialog() == DialogResult.OK;
            return isOk;
        }

        public bool EditCategories(CategoryCollection categories, out CategoryCollection edited, out string path)
        {
            edited = null;
            path = null;

            CategoriesEditorForm editor = new CategoriesEditorForm(categories);
            var result = editor.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                edited = editor.GetCollection();
                return true;
            }

            return false;

        }

        void UpdateRecords(List<TransactionReport> reports)
        {

            _tabContainer.TabPages.Clear();

            foreach (var report in reports)
            {
                TabPage page = new TabPage() { Text = report.Name };
                TransactionGridControl control = new TransactionGridControl();

                page.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                control.DisplayTransactions(report.Transactions);
                _tabContainer.Controls.Add(page);
            }
        }

        private void TabContanerOnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {

            if (keyEventArgs.KeyCode == Keys.Delete)
            {
                TabControl tabContaner = sender as TabControl;
                if (null != tabContaner)
                {
                    string name = tabContaner.SelectedTab.Text;
                    tabContaner.TabPages.Remove(tabContaner.SelectedTab);

                    _eventAggregator.PublishEvent(new ReportDeletedEvent() {ReportName = name});
                }

            }

        }

        public IFilePathProvider FilePathProvider { get; set; }
        public IRulesRepository RulesProvider { get; set; }
        public ITransactionParser TransactionParser { get; set; }
        public IFileDialog FileDialog { get; set; }

        #endregion
    }
}

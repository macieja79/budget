using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Metaproject.Patterns.Persistence;
using Metaproject.WinForms.Adapters;
using Metaproject.WinForms.Tools;
using DialogResult = System.Windows.Forms.DialogResult;
using MenuItem = Metaproject.Dialog.MenuItem;
using Rule = Budget.Core.Entities.Rule;

namespace Budget.UI
{
    public partial class FormReportMain : Form, IStandardEventListener, IOutput
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly AppEngine _engine;
        private TabControl _tabContainer;

        public FormReportMain(IEventAggregator eventAggregator, AppEngine engine)
        {
            _eventAggregator = eventAggregator;
            _engine = engine;


            FilePathProvider = new FilePathProvider();
            RulesProvider = new TestRulesRepository();
            TransactionParser = new IngCsvFileParser();
            FileDialog = new WinFormDialog();

            InitializeComponent();
            EventAttacher.Instance.AttachEvents(this, this);

            this.Closing += OnClosing;
           

        }

        

        #region <IStandardEventListener>

        public void OnButtonClick(object sender, EventArgs e)
        {
            var control = sender as ToolStripButton;
            if (control != null)
            {
                var commandId = control.Tag as string;
                _eventAggregator.PublishEvent<CommandSelectedAggregatedEvent>(
                    CommandSelectedAggregatedEvent.Create(commandId));
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

                this.splitContainer1.Panel1.Controls.Add(_tabContainer);
            }

            UpdateRecords(reports);


        }

        public bool EditRules(List<Rule> rules)
        {
            //RulesForm editForm = new RulesForm(_eventAggregator);
            //editForm.DisplayRules(rules);
            //bool isOk = editForm.ShowDialog() == DialogResult.OK;
            //return isOk;
            return true;

        }

        public bool EditCategories(IFileRepository<CategoryCollection> categories, out CategoryCollection edited,
            out string path)
        {
            edited = null;
            path = null;

            categoryControl1.AttachCategories(categories, _eventAggregator);

            return false;



        }



        void UpdateRecords(List<TransactionReport> reports)
        {

            if (reports.IsNullObj()) return;

            StorePosition(true);

            _tabContainer.TabPages.Clear();


            for (int i = 0; i < reports.Count; i++)
            {
                var report = reports[i];
                TabPage page = new TabPage() {Text = report.Name};
                _tabContainer.Controls.Add(page);
                TransactionGridControl control = new TransactionGridControl();
                control.Name = $"gridControl{i}";

                page.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                control.DisplayTransactions(report.Transactions, _eventAggregator);
              
            }

            StorePosition(false);

        }

        int _selectedPage;
        int _selectedRow;

        void StorePosition(bool isToStore)
        {
            if (_tabContainer.SelectedIndex < 0)
                return;
            
            if (isToStore)
            {
                _selectedPage = _tabContainer.SelectedIndex;

                if (_selectedPage >= 0)
                {
                    TransactionGridControl gridCtrl = GetCurrentGridControl();
                    if (gridCtrl.IsNotNullObj())
                    {
                        _selectedRow = gridCtrl.GetRow();
                    }
                }
            }
            else
            {
                _tabContainer.SelectedIndex = _selectedPage;
                string name = $"gridControl{_selectedPage}";
                var page = _tabContainer.TabPages[_selectedPage];
                TransactionGridControl gridCtrl = page.Controls.Find(name, false).FirstOrDefault() as TransactionGridControl;
                if (gridCtrl.IsNotNullObj())
                {
                    gridCtrl.SetRow(_selectedRow);
                }

            }
        }



        TransactionGridControl GetCurrentGridControl()
        {
            var page = _tabContainer.TabPages[_selectedPage];
            string name = $"gridControl{_selectedPage}";

            TransactionGridControl gridCtrl = page.Controls.Find(name, false).FirstOrDefault() as TransactionGridControl;
            return gridCtrl;
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

        public void AttachMenu(MenuItem menu)
        {
            FormTools.CreateMenu(menuStrip1, menu, OnMenuItemClick);
        }

        public void ClearCurrentItem()
        {
         
        }

        public Transaction GetSelectedTransaction()
        {
            var grid = GetCurrentGridControl();
            Transaction tr = grid.GetSelectedTransaction();
            return tr;
            
        }

        void OnMenuItemClick(object sender, EventArgs e)
        {
           ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item.IsNullObj()) return;

            string commandId = item.Tag as string;
            if (commandId.IsNull()) return;
          
            CommandSelectedAggregatedEvent aggregatedEvent = new CommandSelectedAggregatedEvent();
            aggregatedEvent.CommandId = commandId;

            _eventAggregator.PublishEvent(aggregatedEvent);

        } 

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            ApplicationClosingAggEvent e = new ApplicationClosingAggEvent();
            _eventAggregator.PublishEvent<ApplicationClosingAggEvent>(e);
            cancelEventArgs.Cancel = e.IsCancel;
        }

        #endregion
    }
}

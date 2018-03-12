using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Budget.Core.Entities;
using Budget.UI.Events;
using Metaproject.DataGrid;
using Metaproject.Dialog;
using Metaproject.Enums;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Patterns.EventAggregator.Events;
using DialogResult = System.Windows.Forms.DialogResult;

namespace Budget.UI.ViewModels
{
    public class RuleGridModel : GridModel<Rule>, IDisposable, IGridController, ISubscriber<EventTransactionDropped>
    {
        private Rule _previous;
        private DataGridComboEditor _comboEditor;

        public RuleGridModel(DataGridView grid, IEventAggregator eventAggregator) : base(grid, eventAggregator)
        {
            _comboEditor = new DataGridComboEditor(_grid);
            _eventAggregator.SubsribeEvent(this);


        }

        public void OnEventHandler(EventTransactionDropped e)
        {

            if (e?.TransactionDropDownData?.Transaction == null)
                return;

            if (e.IsHandled) 
                return;
            e.IsHandled = true;


            Transaction transaction = e.TransactionDropDownData.Transaction;
            string property = e.TransactionDropDownData.PickedProperty;


            DoDrop(property, transaction);
        }

        private void DoDrop(string property, Transaction transaction)
        {
            Rule newRule = new Rule();
            if (property == transaction.GetName(() => transaction.AccountNumber))
            {
                newRule.TypeOfRule = Rule.RuleType.Account;
                newRule.Value = transaction.AccountNumber;
            }
            else if (property == transaction.GetName(() => transaction.CounterPartData))
            {
                newRule.TypeOfRule = Rule.RuleType.CounterpartData;
                newRule.Value = transaction.CounterPartData;
            }
            else if (property == transaction.GetName(() => transaction.Title))
            {
                newRule.TypeOfRule = Rule.RuleType.Title;
                newRule.Value = transaction.Title;
            }
            else if (property == transaction.GetName(() => transaction.Details))
            {
                newRule.TypeOfRule = Rule.RuleType.Details;
                newRule.Value = transaction.Details;
            }

            AddItem(newRule);
        }


        protected override object createRow(Rule item)
        {
            var rowModel = new RuleRowModel(item, _previous);
            _previous = item;
            return rowModel;
        }

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {

            

            if (e.ColumnIndex == 1)
            {
                var ruleRowModel = SelectedItem as RuleRowModel;
                if (ruleRowModel == null) return;
                List<string> items = EnumTools<Rule.RuleType>.GetDisplayValues(typeof(Rule.RuleType)).ToList();
                string current = ruleRowModel.TypeOfRule;
                string propertyName = ruleRowModel.GetName(() => ruleRowModel.TypeOfRule);

                _comboEditor.Show_Combobox(e.RowIndex, e.ColumnIndex, items, current, ruleRowModel, propertyName);
                return;
            }

            if (e.ColumnIndex == 2)
            {
                _grid.BeginEdit(true);
            }




            base.OnCellClick(e);
            return;

        }

        protected override void OnDragDrop(DragEventArgs dragEventArgs)
        {

            IDataObject data = dragEventArgs.Data;

            TransactionDropDownData transactionDropDownData =
                data.GetData(typeof(TransactionDropDownData)) as TransactionDropDownData;
            if (transactionDropDownData.IsNullObj()) return;
            
            DoDrop(transactionDropDownData.PickedProperty, transactionDropDownData.Transaction);
            
        }

        protected override bool CanDrop(DragEventArgs dragEventArgs)
        {
            IDataObject data = dragEventArgs.Data;
            if (data.IsNullObj()) return false;

            TransactionDropDownData transactionDropDownData =
                data.GetData(typeof(TransactionDropDownData)) as TransactionDropDownData;
            if (transactionDropDownData.IsNullObj()) return false;

            if (transactionDropDownData.Transaction.IsNullObj()) return false;

            return true;

        }


        public void Dispose()
        {
           _comboEditor.Dispose();
           registerEvents(false);
        }

        public void Add()
        {
            Rule newRule = new Rule();
            AddItem(newRule);
        }

        public void Delete()
        {
            RemoveObject();
        }

        public void Copy()
        {
           CopyToClipboard();
        }

        public void Cut()
        {
            
        }

        public void Paste()
        {
            PasteFromClipboard();
        }


      
    }
}

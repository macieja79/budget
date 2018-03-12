using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Budget.Core.Entities;
using Budget.Core.Events;
using Budget.UI.Events;
using Metaproject.DataGrid;
using Metaproject.Enums;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Patterns.EventAggregator.Events;
using Metaproject.WinForms;
using Metaproject.Dialog;

namespace Budget.UI.ViewModels
{
    public class TransactionGridModel : GridModel<Transaction>, ISubscriber<ComboValueSelectedAggEvent>, IGridController
    {
        private readonly List<Rule> _rules;
        private DataGridComboEditor _comboEditor;

        public TransactionGridModel(DataGridView grid, List<Rule> rules, IEventAggregator eventAggregator)
            : base(grid, eventAggregator)
        {
            _rules = rules;
            _comboEditor = new DataGridComboEditor(grid, eventAggregator);
            _eventAggregator.SubsribeEvent(this);
        }

        protected override object createRow(Transaction item)
        {
            var rowModel = new TransactionRowModel(item);
            return rowModel;
        }

        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            if (_grid == null) return;

            TransactionRowModel transactionRowNModel = _grid.Rows[e.RowIndex].DataBoundItem as TransactionRowModel;
            if (null == transactionRowNModel) return;

            if (transactionRowNModel.TransactionColor.HasValue)
            {
                e.CellStyle.BackColor = transactionRowNModel.TransactionColor.Value;
            }
        }

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 2)
            {
                var rowModel = SelectedItem as TransactionRowModel;
                AggEventCategoriesNeeded eventCategories = new AggEventCategoriesNeeded();
                _eventAggregator.PublishEvent<AggEventCategoriesNeeded>(eventCategories);


                Transaction tempTransaction = new Transaction();
                string propertyName = tempTransaction.GetName(() => tempTransaction.Category);

                if (null != eventCategories.Categories)
                {
                    List<string> categoriesName = eventCategories.Categories.Categories.Select(c => c.Name).ToList();
                    string current = categoriesName.FirstOrDefault();

                    _comboEditor.Show_Combobox(e.RowIndex, e.ColumnIndex, categoriesName, current, rowModel,
                        propertyName);
                }
            }

            if (e.ColumnIndex == 3)
            {
                var rowModel = SelectedItem as TransactionRowModel;

                string selectedCategory = rowModel.Category;

                if (selectedCategory.IsNotNull())
                {
                    AggEventCategoriesNeeded eventCategories = new AggEventCategoriesNeeded();
                    _eventAggregator.PublishEvent<AggEventCategoriesNeeded>(eventCategories);

                    Category category =
                        eventCategories.Categories.Categories.FirstOrDefault(i => i.Name == selectedCategory);
                    List<string> subCategories = category.SubCategories.Select(s => s.Name).ToList();

                    Transaction tempTransaction = new Transaction();
                    string propertyName = tempTransaction.GetName(() => tempTransaction.SubCategory);

                    string current = subCategories.FirstOrDefault();

                    _comboEditor.Show_Combobox(e.RowIndex, e.ColumnIndex, subCategories, current, rowModel,
                        propertyName);


                }


            }



        }

        protected override bool CanDrag(ref object data, string propertyName)
        {
            if (SelectedItem?.Item == null) return false;
            Transaction transaction = SelectedItem.Item;

            List<string> allowedProperties = transaction.GetNames(
                () => transaction.AccountNumber,
                () => transaction.Details,
                () => transaction.Title,
                () => transaction.CounterPartData);


            List<string> lowercased = allowedProperties.Select(p => p.ToLower()).ToList();
            string propertyLowerCased = propertyName.ToLower();


            if (!lowercased.Contains(propertyLowerCased))
                return false;
            TransactionDropDownData transactionDropDownData = new TransactionDropDownData()
            {
                Transaction = transaction,
                PickedProperty = propertyName
            };

            data = transactionDropDownData;
            return true;
        }


        public void OnEventHandler(ComboValueSelectedAggEvent e)
        {
            Transaction tempTransaction = new Transaction();
            string catName = tempTransaction.GetName(() => tempTransaction.Category);
            string subName = tempTransaction.GetName(() => tempTransaction.SubCategory);

            TransactionRowModel row = e.EditedObject as TransactionRowModel;


            if (e.PropertyName == catName)
            {

                row.Item.IsEdited = true;


                AggEventCategoriesNeeded eventCategories = new AggEventCategoriesNeeded();
                _eventAggregator.PublishEvent<AggEventCategoriesNeeded>(eventCategories);

                Category category = eventCategories.Categories.GetCategoryByName(e.NewValue);
                row.Item.Color = category.Color;

                SubCategory firstSubCategory = category.SubCategories.FirstOrDefault();
                if (firstSubCategory.IsNotNullObj())
                {
                    row.Item.SubCategory = firstSubCategory.Name;
                }

            }

            if (e.PropertyName == subName)
            {
                row.Item.IsEdited = true;

            }

        _grid.Refresh();
        _grid.Invalidate();

        }

        #region <IGridNavigator>

        public void Add()
        {
           
        }

        public void Delete()
        {
            RemoveObject();
        }

        public void Copy()
        {
           
        }

        public void Cut()
        {
           
        }

        public void Paste()
        {
            
        }
        #endregion
    }
}


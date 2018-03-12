using System;
using System.Drawing;
using Budget.Core.Entities;
using Budget.UI.Properties;
using Metaproject.DataGrid;
using Metaproject.WinForms.Properties;

namespace Budget.UI.ViewModels
{
    public class TransactionRowModel : GridModelRow<Transaction>
    {
        public TransactionRowModel(Transaction t) : base(t)
        {
            if (t.Color.IsNotNull())
            {
                ColorConverter converter = new ColorConverter();
                TransactionColor = (Color) converter.ConvertFromString(t.Color);
            }
        }

        #region <bindables>

        public Color? TransactionColor { get; set; }

        public string Date
        {
            get { return _item.TransactionDate.ToShortDateString(); }
            set { }
        }


        public Image IsEditedImage => _item.IsEdited ? Resources.Icon16_edit : null;

        public string Owner
        {
            get { return _item.Owner; }
            set { _item.Owner = value; }
        }


        public string Category
        {
            get { return _item.Category; }
            set { _item.Category = value; }
        }

        public string SubCategory
        {
            get { return _item.SubCategory; }
            set { _item.SubCategory = value; }
        }

        public string Title
        {
            get { return _item.Title; }
            set { _item.Title = value; }
        }

        public string CounterPartData
        {
            get { return _item.CounterPartData; }
            set { _item.CounterPartData = value; }
        }

        public string Details
        {
            get { return _item.Details; }
            set { _item.Details = value; }
        }

        public string AccountNumber
        {
            get { return _item.AccountNumber; }
            set { _item.AccountNumber = value; }
        }

        public string Comment
        {
            get { return _item.Comment; }
            set { _item.Comment = value; }
        }

        public string Amount
        {
            get { return $"{_item.Amount:0.00}"; }
            set { }
        }

        #endregion
    }
}
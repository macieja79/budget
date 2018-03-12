using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Budget.Core.Entities;
using Budget.UI.Events;
using Metaproject.Collections;
using Metaproject.WinForms.TreeView;

namespace Budget.UI.ViewModels
{
    public class CategoryTreeViewModel : TreeViewModel
    {
        protected override void OnDragDrop(ITreeViewItem item, IDataObject data)
        {
            SubCategory subCategory = item as SubCategory;
            if (subCategory.IsNullObj()) return;

            TransactionDropDownData transactionDropDownData =
                data.GetData(typeof(TransactionDropDownData)) as TransactionDropDownData;
            if (transactionDropDownData.IsNullObj()) return;
           
            EventTransactionDropped eventTransactionDropped = new EventTransactionDropped();
            eventTransactionDropped.TransactionDropDownData = transactionDropDownData;

            _aggregator.PublishEvent<EventTransactionDropped>(eventTransactionDropped);
        }
    }
}

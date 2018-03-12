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
using Budget.Core.Entities;
using Metaproject.Collections;
using Metaproject.Dialog;
using Metaproject.Forms;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Patterns.Persistence;
using Metaproject.WinForms;
using Metaproject.WinForms.TreeView;

namespace Budget.UI.Forms
{
    public partial class CategoryTreeEditor : Form, IStandardEventListener
    {

        #region <members>

        private IFileRepository<CategoryCollection> _collection;
        private TreeViewModel _treeViewModel = new TreeViewModel();

        #endregion

        #region <ctor>

        public CategoryTreeEditor()
        {
            InitializeComponent();
            treeViewNavigator1.AttachTreeView(_treeViewModel);
           
            treeView1.AfterSelect += TreeView1OnAfterSelect;
            EventAttacher.Instance.AttachEvents(this, this);
        }

        

        

        #endregion

        #region <pub>

        public void AttachCategories(IFileRepository<CategoryCollection> collection)
        {
            _collection = collection;
            _treeViewModel.CreateTree(treeView1, collection.Item, EventAggregator.Empty);
        }

        #endregion

        #region MyRegion

        


       

        void TreeView1OnAfterSelect(object sender, TreeViewEventArgs treeViewEventArgs)
        {
            ITreeViewItem item = (ITreeViewItem) treeViewEventArgs.Node.Tag;
            Category category = item as Category;
            if (category.IsNotNullObj())
            {
                
                var obj = new {category.Name, category.Color};
                Control ctr = PropertyFormGenerator.Instance.CreateControlFor(obj, null, EventAggregator.Empty);
                panel1.Controls.Clear();
                panel1.Controls.Add(ctr);
                return;
            }

            SubCategory subCategory = item as SubCategory;
            if (subCategory.IsNotNullObj() && subCategory.Rules.IsNotNullObj())  
            {
                rulesGridControl1.DisplayRules(subCategory.Rules);
            }
        }

     

   
        #endregion

        #region  IStandardEventListener

        public void OnButtonClick(object sender, EventArgs e)
        {

            if (sender == btnLoad)
            {
                LoadSaveDialogOptions options = new LoadSaveDialogOptions()
                {
                    Filter = BudgetConst.FILE_EXTENSION,
                    IsMultiselect = false,
                    Caption = "Plik z kategoriami"
                };

                _collection.Load(options);
                AttachCategories(_collection);
            }

            if (sender == btnSaveAs)
            {
                LoadSaveDialogOptions options = new LoadSaveDialogOptions()
                {
                    Filter = BudgetConst.FILE_EXTENSION,
                    IsMultiselect = false,
                    Caption = "Plik z kategoriami"
                };

                _collection.Save(options);

                
            }


            if (sender == btnSave)
            {
                _collection.Save();
            }
        }

        public void OnCheckedChanged(object sender, EventArgs e)
        {
           
        }

        #endregion
    }
}

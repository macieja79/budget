using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Budget.Core;
using Budget.Core.Entities;
using Budget.Core.Events;
using Budget.UI.ViewModels;
using Metaproject.Collections;
using Metaproject.Dialog;
using Metaproject.Forms;
using Metaproject.Graphic;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Patterns.Persistence;
using Metaproject.WinForms;
using Metaproject.WinForms.TreeView;

namespace Budget.UI.Forms
{
    public partial class CategoryControl : UserControl, IStandardEventListener, ISubscriber<PathChangedEvent>, ISubscriber<TreeViewModelEvent>
    {


        public CategoryControl()
        {
            InitializeComponent();
            treeViewNavigator1.AttachTreeView(_treeViewModel);
           

          
            EventAttacher.Instance.AttachEvents(this, this);
        }

        #region <members>

        private IFileRepository<CategoryCollection> _collection;
        private CategoryTreeViewModel _treeViewModel = new CategoryTreeViewModel();
        private IEventAggregator _aggregator;

        #endregion


        #region <pub>

        public void AttachCategories(IFileRepository<CategoryCollection> collection, IEventAggregator aggregator)
        {
            _collection = collection;
            if (aggregator.IsNotNullObj())
            {
                _aggregator = aggregator;
                _aggregator.SubsribeEvent(this);
                rulesGridControl1.AttachEventAggregator(_aggregator);
            }

            _treeViewModel.CreateTree(treeView1, collection.Item, _aggregator);
            UpdateColors();
        }

        #endregion

        #region MyRegion






    

        void UpdateColors()
        {

            TreeNode mainNode = treeView1.Nodes[0];


            ImageList imageList = new ImageList();
            for (int i = 0; i < mainNode.Nodes.Count; i++)
            {
                TreeNode node = mainNode.Nodes[i];
                Category category = node.Tag as Category;
                if (category.IsNullObj()) continue;

                string colorStr = category.Color;
                Color color = GraphicsTools.Instance.GetColorFromStr(colorStr);
                Bitmap bmp = GraphicsTools.Instance.GetSquareOfColor(color, 20);
                imageList.Images.Add(bmp);
            }

            treeView1.ImageList = imageList;
            for (int i = 0; i < mainNode.Nodes.Count; i++)
            {
                TreeNode node = mainNode.Nodes[i];
                node.ImageIndex = i;
            }
        }

        void DisplayEditControl(ITreeViewItem item)
        {

            panel1.Controls.Clear();
            rulesGridControl1.Visible = false;
            

            Category category = item as Category;
            if (category.IsNotNullObj())
            {
                PropertyFormGenerator.Options options = new PropertyFormGenerator.Options();
                string colorName = category.Color.GetName(() => category.Color);
                options.Edited.Add(colorName);

                Control ctr = PropertyFormGenerator.Instance.CreateControlFor(category, options, _aggregator);
                
                panel1.Controls.Add(ctr);
                return;
            }

            SubCategory subCategory = item as SubCategory;
            if (subCategory.IsNotNullObj() && subCategory.Rules.IsNotNullObj())
            {
                rulesGridControl1.DisplayRules(subCategory.Rules);
                rulesGridControl1.Visible = true;
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
                _aggregator.PublishEvent<PathChangedEvent>(new PathChangedEvent() {Path = _collection.Path});
                AttachCategories(_collection, null);
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


        public void OnEventHandler(PathChangedEvent e)
        {
            textBox1.Text = e.Path;
        }

        #region ISubscriber<TreeViewModelEvent>
        public void OnEventHandler(TreeViewModelEvent e)
        {
            if (e.TypeOfAction == TreeViewModelEvent.ActionType.Selected)
            {
                DisplayEditControl(e.Item);
            }
        }
        #endregion
    }
}

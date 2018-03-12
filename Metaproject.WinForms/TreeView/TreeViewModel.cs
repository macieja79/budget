using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using WindowsForms = System.Windows.Forms;
using System.Windows.Forms;
using Metaproject.Collections;
using Metaproject.Dialog;
using Metaproject.Patterns.EventAggregator;


namespace Metaproject.WinForms.TreeView
{
    public class TreeViewModel : ITreeViewModel
    {
        #region <members>

        private WindowsForms.TreeView _tree;
        private bool _isCreated = false;
        Dictionary<int, Type> _types = new Dictionary<int, Type>();
        protected IEventAggregator _aggregator;
        #endregion

        #region <pub>

        public void CreateTree(WindowsForms.TreeView tree, ITreeViewItem node, IEventAggregator aggregator)
        {
            _aggregator = aggregator;

            _tree = tree;
            _tree.Nodes.Clear();

            if (!_isCreated)
            {
                tree.KeyDown += TreeOnKeyDown;
                tree.DragDrop += treeView1_DragDrop;
                tree.ItemDrag += treeView1_ItemDrag;
                tree.DragEnter += treeView1_DragEnter;
                tree.NodeMouseDoubleClick += TreeView1OnNodeMouseDoubleClick;
                tree.AfterLabelEdit += TreeView1OnAfterLabelEdit;
                tree.AfterSelect += TreeView1OnAfterSelect;

            }

            TreeNode mainNode = _tree.Nodes.Add(node.TreeDisplayName);
            mainNode.Tag = node;
            AttachNode(mainNode, node);
         
            _isCreated = true;
        }

        void AttachNode(TreeNode treeNode, ITreeViewItem node)
        {
            if (treeNode.Tag.IsNotNullObj())
            {
                int level = treeNode.Level;
                Type type = treeNode.Tag.GetType();
                _types[level] = type;
            }

            var children = node.Children;
            if (children.IsNullObj()) return;

            foreach (var child in children)
            {
                TreeNode iNode = new TreeNode(child.TreeDisplayName);
                iNode.Tag = child;
                treeNode.Nodes.Add(iNode);
                AttachNode(iNode, child);
            }
        }

        #endregion


        private void TreeOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                HandleDelete();
                return;
            }

            if (e.KeyCode == Keys.Insert)
            {
                HandleInsert();
                return;
            }

            if (e.KeyCode == Keys.Oemplus & e.Shift)
            {
                HandleMoveUp();
                return;
            }

            if (e.KeyCode == Keys.OemMinus & e.Shift)
            {
                HandleMoveDown();
                return;
            }
        }

        private void HandleMoveUp()
        {
            TreeNode selected = _tree.SelectedNode;
            if (selected.IsNullObj()) return;

            var items = GetHierarchy(selected);
            if (items.Count < 2) return;

            ITreeViewItem item = items[0];
            ITreeViewItem parent = items[1];

            var treeParent = selected.Parent;
            int index = treeParent.Nodes.IndexOf(selected);
            selected.Remove();
            parent.RemoveTreeItem(item);

            treeParent.Nodes.Insert(index-1, selected);
            parent.AddItem(index-1, item);

            treeParent.TreeView.SelectedNode = selected;

        }


        private void HandleMoveDown()
        {
            TreeNode selected = _tree.SelectedNode;
            if (selected.IsNullObj()) return;

            var items = GetHierarchy(selected);
            if (items.Count < 2) return;

            ITreeViewItem item = items[0];
            ITreeViewItem parent = items[1];

            var treeParent = selected.Parent;
            int index = treeParent.Nodes.IndexOf(selected);
            selected.Remove();
            parent.RemoveTreeItem(item);

            treeParent.Nodes.Insert(index + 1, selected);
            parent.AddItem(index + 1, item);

            treeParent.TreeView.SelectedNode = selected;


        }



        private void HandleDelete()
        {

            TreeNode selected = _tree.SelectedNode;
            if (selected.IsNullObj()) return;

            var items = GetHierarchy(selected);
            if (items.Count < 2) return;

            ITreeViewItem item = items[0];
            ITreeViewItem parent = items[1];

            parent.RemoveTreeItem(item);
            selected.Remove();
        }


        private void HandleInsert()
        {
            TreeNode selected = _tree.SelectedNode;
            if (selected.IsNullObj()) return;

            var items = GetHierarchy(selected);

            ITreeViewItem item = items[0];
            selected.ExpandAll();

            int level = selected.Level;
            int childLevel = level + 1;
            if (_types.IsIndexOutOfRange(childLevel)) return;

            Type childType = _types[childLevel];

            object childItem = Activator.CreateInstance(childType);
            ITreeViewItem iTreeViewChild = childItem as ITreeViewItem;
            if (iTreeViewChild.IsNullObj()) return;

            iTreeViewChild.TreeDisplayName = "[NEW]";
            item.AddItem(iTreeViewChild);

            TreeNode newNode = selected.Nodes.Add(iTreeViewChild.TreeDisplayName);
            newNode.Tag = childItem;
            newNode.BeginEdit();
        }




        public List<ITreeViewItem> GetHierarchy(TreeNode node)
        {
            List<ITreeViewItem> items = new List<ITreeViewItem>();

            TreeNode iNode = node;
            while (iNode.IsNotNullObj())
            {
                ITreeViewItem item = iNode.Tag as ITreeViewItem;
                if (null != item)
                    items.Add(item);

                iNode = iNode.Parent;
            }

            ITreeViewItem rootItem = _tree.Tag as ITreeViewItem;
            if (rootItem.IsNotNullObj())
            {
                items.Add(rootItem);
            }



            return items;
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var node = (TreeNode) e.Item;
            var hierarchy = GetHierarchy(node);
            if (hierarchy.Count == 1) return;

            _tree.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {

            Point targetPoint = _tree.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = _tree.GetNodeAt(targetPoint);
            e.Effect = DragDropEffects.All;
            

        }


        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {



            Point targetPoint = _tree.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = _tree.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode) e.Data.GetData(typeof(TreeNode));

            if (draggedNode.IsNotNullObj())
            {
                // Confirm that the node at the drop location is not 
                // the dragged node and that target node isn't null
                // (for example if you drag outside the control)
                if (!draggedNode.Equals(targetNode) && targetNode != null)
                {
                    var srcHierarchy = GetHierarchy(draggedNode);
                    var destHierarchy = GetHierarchy(targetNode);

                    srcHierarchy[1].RemoveTreeItem(srcHierarchy[0]);
                    destHierarchy[0].AddItem(srcHierarchy[0]);

                    // Remove the node from its current 
                    // location and add it to the node at the drop location.
                    draggedNode.Remove();
                    targetNode.Nodes.Add(draggedNode);


                    // Expand the node at the location 
                    // to show the dropped node.
                    targetNode.Expand();
                }
            }
            else
            {
                var srcHierarchy = GetHierarchy(targetNode);
                var dropTarget = srcHierarchy.FirstOrDefault();
                
                OnDragDrop(dropTarget, e.Data);

            }



            
        }

        protected virtual void OnDragDrop(ITreeViewItem item, IDataObject data)
        {
            
        }

        private void TreeView1OnAfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label.IsNull()) return;

            ITreeViewItem item = e.Node.Tag as ITreeViewItem;
            if (item.IsNullObj()) return;
            item.TreeDisplayName = e.Label;
            e.Node.EndEdit(false);
            e.Node.Text = e.Label;
        }

        private void TreeView1OnNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.BeginEdit();
        }

        #region #ITreeViewModel

        public void Expand()
        {
            _tree.ExpandAll();
        }

        public void Collapse()
        {
            _tree.CollapseAll();
        }

        public void AddSelected()
        {
            HandleInsert();
        }

        public void DeleteSelected()
        {
            HandleDelete();
        }

        void TreeView1OnAfterSelect(object sender, TreeViewEventArgs treeViewEventArgs)
        {
            ITreeViewItem item = (ITreeViewItem) treeViewEventArgs.Node.Tag;
            if (_aggregator.IsNotNullObj())
            {
                _aggregator.PublishEvent(new TreeViewModelEvent()
                {
                    Item = item,
                    TypeOfAction = TreeViewModelEvent.ActionType.Selected
                });
            }
        }

        #endregion
    }
}

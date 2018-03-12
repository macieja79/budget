using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Metaproject.Generic;
using System.Windows.Forms;
using Metaproject.Files;
using Metaproject.Patterns.EventAggregator;

namespace Metaproject.DataGrid
{
    public abstract class GridModel<T> where T : class
    {

        #region <members>

        protected BindingList<object> _items = new BindingList<object>();
        protected List<T> _orgItems = new List<T>();
        protected DataGridView _grid;
        protected IEventAggregator _eventAggregator;
        
        #endregion

        #region <ctor>
        public GridModel(DataGridView grid, IEventAggregator eventAggregator)
        {
            _grid = grid;
            _eventAggregator = eventAggregator;

			registerEvents(true);
        }
		


        #endregion
		protected void registerEvents(bool isToRegister)
		{

			if (isToRegister) {
				_grid.CellMouseDoubleClick += _grid_CellMouseDoubleClick;
                _grid.KeyDown += _grid_KeyDown;
                _grid.CellClick += _grid_CellClick;
                _grid.CellContentClick += _grid_CellContentClick;
                _grid.CellValueChanged += _grid_CellValueChanged;
			    _grid.CellFormatting += _grid_CellFormatting;
			    _grid.MouseDown += _grid_MouseDown;
                _grid.DragDrop += _grid_DragDrop;
                _grid.DragEnter += _grid_DragEnter;
                
               

			} else {
                _grid.CellMouseDoubleClick -= _grid_CellMouseDoubleClick;
                _grid.KeyDown -= _grid_KeyDown;
                _grid.CellClick -= _grid_CellClick;
                _grid.CellContentClick -= _grid_CellContentClick;
                _grid.CellValueChanged -= _grid_CellValueChanged;
                _grid.CellFormatting -= _grid_CellFormatting;
                _grid.MouseDown -= _grid_MouseDown;
                _grid.DragDrop -= _grid_DragDrop;
                _grid.DragEnter -= _grid_DragEnter;
            }


		}


        private void _grid_DragDrop(object sender, DragEventArgs dragEventArgs)
        {
            OnDragDrop(dragEventArgs);
        }

     

        private void _grid_MouseDown(object sender, MouseEventArgs e)
        {
            object data = null;

            var cells = _grid.SelectedCells;
            var hitTestInfo = _grid.HitTest(e.X, e.Y);

            if (hitTestInfo.RowIndex < 0)
                return;

            string property = string.Empty;
            foreach (var column in _grid.Columns.ToList())
            {
                if (column.Index == hitTestInfo.ColumnIndex)
                {
                    property = column.DataPropertyName;
                }
            }
          
            

        bool canDragDrop = CanDrag(ref data, property);
            if (!canDragDrop) return;

            _grid.DoDragDrop(data, DragDropEffects.Move);
        }


        void _grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            OnCellValueChanged(e);
        }

        void _grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            OnCellContentClick(e);
        }

        void _grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            OnCellClick(e);
        }

        void _grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           OnCellFormatting(e);
        }


        private void _grid_DragEnter(object sender, DragEventArgs e)
        {
            
            bool canDrop = CanDrop(e);
            if (canDrop)
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }


        void _grid_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

		void _grid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			OnCellDoubleClick();
		}

		#region <prv>
		


		#endregion

		#region <nested>

		class ModelSynchroItem : SynchroItem<T, GridModelRow<T>>
        {

        }
        
        #endregion

        #region <props>
        
        public BindingList<object> Items
        {
            get
            {
                return _items;
            }

        }

        public virtual GridModelRow<T> SelectedItem
        {
            get
            {
				if (_grid.SelectedRows.Count == 1)
					return (GridModelRow<T>)_grid.SelectedRows[0].DataBoundItem;
				else if (_grid.SelectedCells.Count == 1)
					return (GridModelRow<T>)_grid.SelectedCells[0].OwningRow.DataBoundItem;

                return null;
            }
        }

        public virtual IEnumerable<GridModelRow<T>> SelectedItems
        {
            get
            {
                if (_grid.SelectedRows.Count == 0) yield return null;
                List<GridModelRow<T>> items = new List<GridModelRow<T>>();
                foreach (DataGridViewRow row in _grid.SelectedRows)
                {
                    yield return (GridModelRow<T>) row.DataBoundItem;
                }
            }
        }

        public void CopyToClipboard()
        {
            if (SelectedItems.IsNullObj()) return;
            List<T> items = SelectedItems.Select(i => i.Item).ToList();
            string xml = Xml.Serializer.Instance.SerializeToXml<List<T>>(items);
            Clipboard.SetText(xml);
        }

        public void PasteFromClipboard()
        {

            string xml = Clipboard.GetText();
            List<T> items = Xml.Serializer.Instance.DeserializeFromXml<List<T>>(xml);

            _orgItems.AddRange(items);
            UpdateModel(_orgItems);
        }

        public T GetSelectedItem()
        {
            GridModelRow<T> row = null;
            
            if (_grid.SelectedRows.Count == 1) 
            {
                row = _grid.SelectedRows[0].DataBoundItem as GridModelRow<T>;
            }
            else if (_grid.SelectedCells.Count == 1)
            {
                row = _grid.SelectedCells[0].OwningRow.DataBoundItem as GridModelRow<T>;
            }


            if (null == row) return default(T);

            return row.Item;

        }
        
        #endregion

        #region <to override>

        public virtual void AddItem(T item)
        {
            _orgItems.Add(item);
            UpdateModel(_orgItems);

        }
        
        protected abstract object createRow(T item);

        public void Refresh()
        {
            _grid.Refresh();
        }

        public  virtual void UpdateModel(List<T> items)
        {
            List<int> columnWidths = new List<int>();
            for (int i = 0; i < _grid.Columns.Count; i++)
            {
                var column = _grid.Columns[i];
                columnWidths.Add(column.Width);
            }

            _orgItems = items;

            SynchroTableFactory<T, GridModelRow<T>> factory = new SynchroTableFactory<T, GridModelRow<T>>();
            Func<T, GridModelRow<T>, bool> predicate = (i, r) =>
            {
                return object.ReferenceEquals(r.Item, i);
            };

            List<GridModelRow<T>> unbinded = new List<GridModelRow<T>>();
            foreach(object obj in _items)
            {
                unbinded.Add((GridModelRow<T>)obj);
            }

            List<ModelSynchroItem> table = factory.CreateTable<ModelSynchroItem>(items, unbinded, predicate);
            List<T> originUnmatched = factory.GetFirstUnmatched(table);
            foreach (T newItem in originUnmatched)
            {
                object newRow = createRow(newItem);
                _items.Add(newRow);
            }

            List<GridModelRow<T>> existingUnmatched = factory.GetSecondUnmatched(table);
            foreach (GridModelRow<T> row in existingUnmatched)
            {
                _items.Remove(row);
            }

            if (null == _grid.DataSource && _items.Count > 0 )
                _grid.DataSource = _items;

            for (int i = 0; i < _grid.Columns.Count; i++)
                _grid.Columns[i].Width = columnWidths[i];
        }

		protected virtual void OnCellDoubleClick()
		{
            EditObject();
		}

        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EditObject();
            }
            if (e.KeyCode == Keys.Delete)
            {
                RemoveObject();
            }
        }

        protected virtual void OnCellClick(DataGridViewCellEventArgs e)
        {
           
        }

        protected virtual void OnCellContentClick(DataGridViewCellEventArgs e)
        {

        }


		public virtual void EditObject()
		{

		}

        protected virtual void RemoveObject()
        {
            if (SelectedItem.IsNullObj()) return;
            _orgItems.Remove(SelectedItem.Item);
            UpdateModel(_orgItems);
        }

        protected virtual void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            
        }

        protected virtual void OnCellFormatting(DataGridViewCellFormattingEventArgs dataGridViewCellFormattingEventArgs)
        {

        }

        

        protected virtual bool CanDrag(ref object data, string propertyName)
        {
            return false;
        }

        protected virtual bool CanDrop(DragEventArgs dragEventArgs)
        {
            return false;
        }




        protected virtual void OnDragDrop(DragEventArgs dragEventArgs)
        {
            
        }

        #endregion
    }
}

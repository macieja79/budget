using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Metaproject.Files;
using Metaproject.Dialog;
using Metaproject.WinForms.Adapters;

namespace Metaproject.Forms
{
	public partial class CheckedListForm : Form
	{

		#region <ctor>
		public CheckedListForm()
		{
			InitializeComponent();
		}
		#endregion

		#region <pub>

		public void InitializeDialog(string message, List<string> allItems, List<string> selectedItems, IFileDialog loadDialog, IFileDialog saveDialog)
		{

            checkedListBox1.Items.Clear();
			txtMessage.Text = message;

			List<string> sorted = allItems.OrderBy(i => i).ToList();
			foreach (string item in sorted)
			{
				CheckState iCheckState = (selectedItems.Contains(item)) ? CheckState.Checked : CheckState.Unchecked;
				checkedListBox1.Items.Add(item, iCheckState);
			}

		}

		public List<string> GetSelectedItems()
		{
			CheckedListBox.CheckedItemCollection collection = checkedListBox1.CheckedItems;
			List<string> checkedItems = new List<string>();
			foreach (var item in collection)
				checkedItems.Add(item.ToString());

			return checkedItems;
		}

		#endregion

		#region <handlers>
		private void onButtonClick(object sender, EventArgs e)
		{

			if (sender == btnOk)
			{
	
				return;
			}

			if (sender == btnCancel)
			{

				return;
			}

			if (sender == btnSelectAll)
			{

				selectAll(true);
				return;
			}

			if (sender == btnDeselectAll)
			{

				selectAll(false);
				return;
			}

			if (sender == btnInvert)
			{
				invertSelection();
				return;
			}

			if (sender == btnSave) {
				saveSelection();

				return;
			}

			if (sender == btnLoad) {
				loadSelection();
				return;
			
			}
			


		}

	

		
		#endregion

		#region <prv>

		void selectAll(bool isSelected)
		{
			for (int i = 0; i < checkedListBox1.Items.Count; ++i)
			{
				checkedListBox1.SetItemChecked(i, isSelected);
			}

		}

		void invertSelection()
		{
			for (int i = 0; i < checkedListBox1.Items.Count; ++i)
			{
				checkedListBox1.SetItemChecked(i, !checkedListBox1.GetItemChecked(i));
			}


		}

		void loadSelection()
		{
            string path;
            WinFormDialog openDialogAdapter = new WinFormDialog();
            List<string> selected = XmlFileManager.Instance.LoadWithDialog<List<string>>(openDialogAdapter, "Pliki xml|*.xml", out path);
            setSelection(selected);
		}

        private void setSelection(List<string> selected)
        {

            int i = 0;
            foreach (var item in checkedListBox1.Items)
            {
                string text = item.ToString();
                bool isOnList = selected.Contains(text);
                checkedListBox1.SetItemChecked(i, isOnList);
                i++;
            }

        }

		private void saveSelection()
		{

			try {
				List<string> selected = GetSelectedItems();
                WinFormDialog saveAdapter = new WinFormDialog();
				XmlFileManager.Instance.SaveWithDialog<List<string>>(saveAdapter, selected, "Pliki xml|*.xml");
			} catch (Exception exc) {

			}
		}

		#endregion
	}
}

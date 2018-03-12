using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Budget.Core.Entities;
using Metaproject.Files;
using Metaproject.Forms;
using Metaproject.JSON;
using Metaproject.WinForms.Adapters;

namespace Budget.UI.Forms
{
    public partial class CategoriesEditorForm : Form, IStandardEventListener
    {
        JsonSerializer<CategoryCollection> _serializer = new JsonSerializer<CategoryCollection>();
        private string _original;

        public CategoriesEditorForm(CategoryCollection collection)
        {
            InitializeComponent();
            EventAttacher.Instance.AttachEvents(this, this);
            ShowCollection(collection);
        }

        private void ShowCollection(CategoryCollection collection)
        {
            string str = _serializer.Serialize(collection);
            textBox1.Text = str;
            _original = str;
        }

        

        #region IStandardEventListener

        public void OnButtonClick(object sender, EventArgs e)
        {
            if (sender == button2)
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            if (sender == button1)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            if (sender == button3)
            {
                textBox1.Text = _original;
                return;
            }

            if (sender == button4)
            {
                ProceedLoadFile();
                return;
            }

            if (sender == button5)
            {
                ProceedSaveFile();
                return;
            }

        }

        

        public void OnCheckedChanged(object sender, EventArgs e)
        {
            
        }

        #endregion  

        public CategoryCollection GetCollection()
        {
            string str = textBox1.Text;
            CategoryCollection collection = _serializer.Deserialize(str);
            return collection;
        }

        private void ProceedSaveFile()
        {           
            WinFormDialog winFormDialog = new WinFormDialog();
            string filter = FileTools.Instance.GetFileFilter("Pliki z kategoriami", "bgt");

            string path = FileTools.Instance.GetPathForSave(winFormDialog, filter);
            File.WriteAllText(path, filter);

        }

        private void ProceedLoadFile()
        {
            WinFormDialog winFormDialog = new WinFormDialog();
            string filter = FileTools.Instance.GetFileFilter("Pliki z kategoriami", "bgt");

            string path = FileTools.Instance.GetPathForOpen(winFormDialog, filter);
            string str = File.ReadAllText(path);

            textBox1.Text = str;
            _original = str;
        }
    }
}

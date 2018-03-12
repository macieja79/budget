using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Metaproject.Mail;

namespace Metaproject.Controls
{
    public partial class TextViewer : UserControl
    {

        string _fileName;
        
        
        public TextViewer()
        {
            InitializeComponent();
        }
        public string MailAddress { get; set; }

        public string Txt
        {
            get
            {
                return txtContent.Text;
            }
            set
            {
                txtContent.Text = value;
            }
        }

        public void AppendLines(List<string> lines)
        {
            foreach (string str in lines)
            {
                AppendLine(str);
            }
        }

        public void AppendLine(string line)
        {
            txtContent.AppendText(line);
            txtContent.AppendText(Environment.NewLine);
        }

        public void Clear()
        {
            txtContent.Text = string.Empty;
        }

        #region <events>
        private void onnButtonClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                saveFile();
                return;
            }

            if (sender == btnClip)
            {
                copyToClipBoard();
                return;
            }

            if (sender == btnMail)
            {
                mailTo();
                return;

            }
        }
        #endregion


        #region <prv>

        private void mailTo()
        {
            MailTools.Instance.MailTo(MailAddress, "Raport", Txt);
        }

        private void copyToClipBoard()
        {
            txtContent.SelectAll();
            txtContent.Copy();
            txtContent.Select(0, 0);
        }

        private void saveFile()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Pliki tekstowe (*.txt)|*.txt";
            saveDialog.Title = "Zapis pliku tekstowego";
            saveDialog.RestoreDirectory = true;
            saveDialog.FileName = _fileName;
            //if (!string.IsNullOrEmpty(_fileName))
            //	saveDialog.InitialDirectory= _fileName;

            DialogResult dialogResult = saveDialog.ShowDialog(this);
            if (dialogResult != System.Windows.Forms.DialogResult.OK) return;

            try
            {
                string fileName = saveDialog.FileName;
                File.AppendAllText(fileName, txtContent.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

    }
}

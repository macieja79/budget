using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Metaproject.Log;
using Metaproject.Files;
using Metaproject.Reflection;
using Metaproject.Xml;
using System.IO;
using Metaproject.Mail;
using Metaproject.Dialog;

namespace Metaproject.Controls
{
    public partial class LogReportViewer : UserControl
	{

		#region <fields>
		LogReportPack _pack;

        IFileDialog _iOpenDialog;
        IFileDialog _iSaveDialog;

        #endregion

        #region <ctor, init>

        public LogReportViewer()
        {
            InitializeComponent();

            
            list.OnReportSelected += list_OnReportSelected;
        
        }

        void AttachDialogs(IFileDialog iOpenDialog, IFileDialog iSaveDialog)
        {
            _iOpenDialog = iOpenDialog;
            _iSaveDialog = iSaveDialog;

        }

        #endregion

        #region <pub>

        public void LoadFromFile(string path)
        {
            load(path);
        }

		public void SetReportPack(LogReportPack pack)
        {
			_pack = pack;
            list.SetReports(pack);
        }
		#endregion

		#region <handlers>
		void list_OnReportSelected(LogReport report)
        {
            reportDetails.SetReport(report);
		}

		private void onButtonClick(object sender, EventArgs e)
		{

			if (sender == btnLoad) {
                load();
                return;
            }

			if (sender == btnSave) {
                save();
                return;
			}

			if (sender == btnMail) {
				sendMail();
				return;
			}

			if (sender == btnClipboard) {
				copyToClipboard();
				return;
			}

		}

	
	
		#endregion

		#region <prv>

        void load(string path)
        {
            LogReportPack pack = LogReportIO.Instance.CreateFromFile(path);
            SetReportPack(pack);
        }

        void load()
        {
            string path = FileTools.Instance.GetPathForOpen(_iOpenDialog, "Pliki *.mtr|*.mtr");
            if (!string.IsNullOrEmpty(path))
            {
                load(path);
            }
        }

        void save()
        {
            string path = FileTools.Instance.GetPathForSave(_iSaveDialog, "Pliki *.mtr|*.mtr");
            if (!string.IsNullOrEmpty(path))
            {
                LogReportIO.Instance.SaveToFile(_pack, path);
            }
        }

		void sendMail()
		{
			string xml = Serializer.Instance.SerializeToXml<LogReportPack>(_pack);

			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Raport.xml");

			File.WriteAllText(path, xml);

			MailTools.Instance.MailTo("maciek.szczudlo@gmail.com", "Raport", "Oto raport", path);

		}

		void copyToClipboard()
		{

            string[] lines = LogReportIO.Instance.GetAsLines(_pack);

			TextBox tmpTextBox = new TextBox();
			tmpTextBox.Multiline = true;
			tmpTextBox.Lines = lines.ToArray();
			tmpTextBox.SelectAll();
			tmpTextBox.Copy();
			tmpTextBox = null;

		}

		#endregion

		


	}
}

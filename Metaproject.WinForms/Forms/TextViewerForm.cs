using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Metaproject.Forms
{
	public partial class TextViewerForm : Form
	{
		public TextViewerForm()
		{
			InitializeComponent();
		}



		string _fileName;

		public void SetData(string caption, string message, string content, string fileName)
		{

			this.Text = caption;
            this.textViewer1.Txt = content;
			this.txtMessage.Text = message;
			_fileName = GetFileNameWithoutInvalidChars(fileName);
		}

		string GetFileNameWithoutInvalidChars(string fileName)
		{

			if (string.IsNullOrEmpty(fileName)) return fileName;

			char CHAR_TO_REPLACE = '_';
			char[] invalidChars = Path.GetInvalidFileNameChars();

			string newFileName = fileName;
			foreach (char c in invalidChars)
				newFileName = newFileName.Replace(c, CHAR_TO_REPLACE);

			return newFileName;



		}


		void onButtonClick(object sender, EventArgs e)
		{

			if (sender == btnOk)
			{
				CloseDialog();
				return;
			}


			
		}


		

		void CloseDialog()
		{
			Close();

		}

		

       

   

   

		

	}
}

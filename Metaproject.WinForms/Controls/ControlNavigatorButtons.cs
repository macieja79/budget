using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.Controls
{
	public partial class ControlNavigatorButtons : UserControl
	{
		INavigatorListener _listener;

		public ControlNavigatorButtons()
		{
			InitializeComponent();

			List<ToolStripButton> buttons = this.Controls.OfType<ToolStripButton>().ToList();
			buttons.ForEach(b => b.Click += b_Click);
		}

		void b_Click(object sender, EventArgs e)
		{

			NavigatorButtonType buttonType = NavigatorButtonType.Uknown;

			if (sender == btnOpen) buttonType = NavigatorButtonType.Open;
			if (sender == btnSave) buttonType = NavigatorButtonType.Save;
			if (sender == btnWord) buttonType = NavigatorButtonType.Word;
			if (sender == btnExcel) buttonType = NavigatorButtonType.Excel;
			if (sender == btnMail) buttonType = NavigatorButtonType.Mail;
			if (sender == btnClipboard) buttonType = NavigatorButtonType.Csv;

			if (null != _listener)
				_listener.OnButtonClicked(buttonType);
	
		
		}

		public void AttachListener(INavigatorListener listener)
		{
			_listener = listener;
		}


	}
}

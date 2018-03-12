using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Metaproject.Log;


namespace Metaproject.Controls
{
	public partial class LogReportControl : UserControl
	{

		#region <members>
		bool _isDateVisible;
		#endregion

		#region <ctor>
		public LogReportControl()
		{
			InitializeComponent();
		}
		#endregion

		#region <const>
		int COL_ICON = 0;
		int COL_DATE = 1;
		int COL_DESC = 2;
		#endregion

		#region <props>


		LogReportDateDisplayType _typeOfDateDisplay;
		public LogReportDateDisplayType TypeOfDateDisplay
		{
			get
			{
				return _typeOfDateDisplay;
			}
			set
			{
				_typeOfDateDisplay = value;
				grid.Columns[COL_DATE].Visible = (value != LogReportDateDisplayType.None);
			}
		}

		#endregion

		#region <pub>
		public void SetReport(LogReport report)
		{

			if (null == report)
				return;

			grid.Rows.Clear();
			grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

			foreach (LogReportItem item in report.Items) {
				int r = grid.Rows.Add();
				grid[COL_ICON, r].Value = GetBitmap(item.TypeOfLogItem);

				string date = LogReportItem.GetDate(item.Date, TypeOfDateDisplay);

				grid[COL_DATE, r].Value = date;
				grid[COL_DESC, r].Value = item.Message;
				grid[COL_DESC, r].Style.WrapMode = DataGridViewTriState.True;
				grid.Rows[r].Tag = item;


			}

			
		}
		#endregion


		#region <prv>




	
		#endregion

		#region <static>
		public static Bitmap GetOkBitmap()
		{
			return Properties.Resources.IconOK16;
		}

		public static Bitmap GetBitmap(LogReportItemType itemType)
		{
			if (itemType == LogReportItemType.Error)
				return Properties.Resources.IconError16;
			else if (itemType == LogReportItemType.Exception)
				return Properties.Resources.IconError16;
			else if (itemType == LogReportItemType.Warning)
				return Properties.Resources.IconWarning16;
			else if (itemType == LogReportItemType.Message)
				return Properties.Resources.IconInfo16;

			return null;

		}

		#endregion

	}
}

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
	public partial class LogReportListControl : UserControl
	{

        public delegate void ReportSelectedHandler(LogReport report);

        public event ReportSelectedHandler OnReportSelected;


		const int COL_BMP = 0;
		const int COL_DATE = 1;
		const int COL_NAME = 2;

		public LogReportListControl()
		{
			InitializeComponent();
		}

		public void SetReports(LogReportPack pack)
		{
			grid.Rows.Clear();
			grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (null == pack) return;

			foreach (LogReport report in pack.Reports) 
            {
				int r = grid.Rows.Add();
                grid[COL_BMP, r].Value = LogReportControl.GetBitmap(report.MostCriticalType);
				grid[COL_DATE, r].Value = report.CreationDate.ToShortDateString();
				grid[COL_NAME, r].Value = report.Name;
				grid.Rows[r].Tag = report;
			}
		}

        private void onCellClick(object sender, DataGridViewCellEventArgs e)
        {
            object obj = grid.Rows[e.RowIndex].Tag;
            LogReport report = (LogReport)obj;
            if (null != OnReportSelected)
                OnReportSelected(report);
        }

	}
}

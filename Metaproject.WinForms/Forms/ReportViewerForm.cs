using Metaproject.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.Forms
{
    public partial class ReportViewerForm : Form
    {
        public ReportViewerForm()
        {
            InitializeComponent();
        }

        public void SetReport(LogReportPack pack)
        {
            viewer.SetReportPack(pack);
        }

        public void LoadFromFile(string path)
        {
            viewer.LoadFromFile(path);
        }

   
    }
}

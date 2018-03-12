using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;


namespace Metaproject.Excel
{
	public partial class TestForm : Form
	{
		public TestForm()
		{
			InitializeComponent();
			init();
		}

		void init()
		{

			int rowCount = 20;
			int colCount = 20;


			for (int c = 0; c < colCount; c++)
			{
				dataGridView1.Columns.Add(c.ToString(), c.ToString());

			}

			for (int r = 0; r < rowCount; r++)
			{

				dataGridView1.Rows.Add();


				for (int c = 0; c < colCount; c++)
				{

					string value = TestTool.Instance.GenerateRandomWord(10, 20);
					dataGridView1[c, r].Value = value;


				}
			}


		}

		private void button1_Click(object sender, EventArgs e)
		{

			//GridToExcelSheetConverter converter = new GridToExcelSheetConverter();
			//ExcelSheetData sheetData = converter.CreateFromGrid(dataGridView1);

			//ExcelFormatData format = new ExcelFormatData(1, 1, 20, 20);
			//format.Background = Color.LightGray;
			//format.IsCoumnAutofit = true;
			//sheetData.Formats.Add(format);


			//ExcelApp app = new ExcelApp();
			//app.CreateNewInstance();

			//IExcelWorkbook workbook = app.CreateAndActivateNewWorkbook();
			//IExcelSheet sheet = workbook.GetActiveSheet();
			//sheet.InsertSheetData(sheetData);


		}
	}
}

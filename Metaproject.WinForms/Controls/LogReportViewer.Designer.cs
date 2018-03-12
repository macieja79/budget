namespace Metaproject.Controls
{
    partial class LogReportViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.list = new Metaproject.Controls.LogReportListControl();
			this.reportDetails = new Metaproject.Controls.LogReportControl();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnMail = new System.Windows.Forms.Button();
			this.btnClipboard = new System.Windows.Forms.Button();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.list);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.reportDetails);
			this.splitContainer1.Size = new System.Drawing.Size(852, 500);
			this.splitContainer1.SplitterDistance = 282;
			this.splitContainer1.TabIndex = 2;
			// 
			// list
			// 
			this.list.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list.Location = new System.Drawing.Point(0, 0);
			this.list.Name = "list";
			this.list.Size = new System.Drawing.Size(282, 500);
			this.list.TabIndex = 1;
			// 
			// reportDetails
			// 
			this.reportDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.reportDetails.Location = new System.Drawing.Point(0, 0);
			this.reportDetails.Name = "reportDetails";
			this.reportDetails.Size = new System.Drawing.Size(566, 500);
			this.reportDetails.TabIndex = 0;
			this.reportDetails.TypeOfDateDisplay =  Metaproject.Log.LogReportDateDisplayType.Time;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.Location = new System.Drawing.Point(13, 509);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 3;
			this.btnSave.Text = "Zapisz";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.onButtonClick);
			// 
			// btnLoad
			// 
			this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnLoad.Location = new System.Drawing.Point(94, 509);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(75, 23);
			this.btnLoad.TabIndex = 4;
			this.btnLoad.Text = "Wczytaj";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.onButtonClick);
			// 
			// btnMail
			// 
			this.btnMail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnMail.Location = new System.Drawing.Point(175, 509);
			this.btnMail.Name = "btnMail";
			this.btnMail.Size = new System.Drawing.Size(75, 23);
			this.btnMail.TabIndex = 5;
			this.btnMail.Text = "Mail";
			this.btnMail.UseVisualStyleBackColor = true;
			this.btnMail.Click += new System.EventHandler(this.onButtonClick);
			// 
			// btnClipboard
			// 
			this.btnClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClipboard.Location = new System.Drawing.Point(256, 509);
			this.btnClipboard.Name = "btnClipboard";
			this.btnClipboard.Size = new System.Drawing.Size(75, 23);
			this.btnClipboard.TabIndex = 6;
			this.btnClipboard.Text = "Schowek";
			this.btnClipboard.UseVisualStyleBackColor = true;
			this.btnClipboard.Click += new System.EventHandler(this.onButtonClick);
			// 
			// LogReportViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnClipboard);
			this.Controls.Add(this.btnMail);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.splitContainer1);
			this.Name = "LogReportViewer";
			this.Size = new System.Drawing.Size(858, 546);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private LogReportControl reportDetails;
        private LogReportListControl list;
        private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Button btnMail;
		private System.Windows.Forms.Button btnClipboard;
    }
}

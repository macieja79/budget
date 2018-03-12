namespace Budget.UI.Forms
{
    partial class RulesGridControl
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Kolumna = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Wartości = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridNavigator1 = new Metaproject.WinForms.Controls.GridNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Kolumna,
            this.Wartości});
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 36);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(434, 362);
            this.dataGridView1.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.DataPropertyName = "IsEnabled";
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.Width = 30;
            // 
            // Kolumna
            // 
            this.Kolumna.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Kolumna.DataPropertyName = "TypeOfRule";
            this.Kolumna.HeaderText = "Kolumna";
            this.Kolumna.Name = "Kolumna";
            this.Kolumna.ReadOnly = true;
            this.Kolumna.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Kolumna.Width = 70;
            // 
            // Wartości
            // 
            this.Wartości.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Wartości.DataPropertyName = "Value";
            this.Wartości.HeaderText = "Wartości";
            this.Wartości.Name = "Wartości";
            // 
            // gridNavigator1
            // 
            this.gridNavigator1.Location = new System.Drawing.Point(3, 5);
            this.gridNavigator1.MaximumSize = new System.Drawing.Size(200, 25);
            this.gridNavigator1.MinimumSize = new System.Drawing.Size(100, 25);
            this.gridNavigator1.Name = "gridNavigator1";
            this.gridNavigator1.Size = new System.Drawing.Size(118, 25);
            this.gridNavigator1.TabIndex = 1;
            // 
            // RulesGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridNavigator1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "RulesGridControl";
            this.Size = new System.Drawing.Size(434, 398);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private Metaproject.WinForms.Controls.GridNavigator gridNavigator1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Kolumna;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wartości;
    }
}

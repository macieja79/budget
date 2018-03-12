namespace Budget.UI.Forms
{
    partial class TransactionGridControl
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
            this.gridNavigator1 = new Metaproject.WinForms.Controls.GridNavigator();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIsEdited = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Podkategoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Kolumna = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Wartości = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Komentarz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.ColumnIsEdited,
            this.Column2,
            this.Podkategoria,
            this.Amount,
            this.Column1,
            this.Column3,
            this.Kolumna,
            this.Wartości,
            this.Komentarz});
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 34);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(681, 388);
            this.dataGridView1.TabIndex = 0;
            // 
            // gridNavigator1
            // 
            this.gridNavigator1.Location = new System.Drawing.Point(3, 3);
            this.gridNavigator1.MaximumSize = new System.Drawing.Size(200, 25);
            this.gridNavigator1.MinimumSize = new System.Drawing.Size(100, 25);
            this.gridNavigator1.Name = "gridNavigator1";
            this.gridNavigator1.Size = new System.Drawing.Size(108, 25);
            this.gridNavigator1.TabIndex = 1;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.DataPropertyName = "Date";
            this.Column4.HeaderText = "Data";
            this.Column4.MinimumWidth = 60;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 60;
            // 
            // ColumnIsEdited
            // 
            this.ColumnIsEdited.DataPropertyName = "IsEditedImage";
            this.ColumnIsEdited.HeaderText = "";
            this.ColumnIsEdited.MinimumWidth = 20;
            this.ColumnIsEdited.Name = "ColumnIsEdited";
            this.ColumnIsEdited.ReadOnly = true;
            this.ColumnIsEdited.Width = 62;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.DataPropertyName = "Category";
            this.Column2.HeaderText = "Kategoria";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.Width = 70;
            // 
            // Podkategoria
            // 
            this.Podkategoria.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Podkategoria.DataPropertyName = "SubCategory";
            this.Podkategoria.HeaderText = "Podkategoria";
            this.Podkategoria.Name = "Podkategoria";
            this.Podkategoria.ReadOnly = true;
            this.Podkategoria.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Podkategoria.Width = 70;
            // 
            // Amount
            // 
            this.Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Amount.DataPropertyName = "Amount";
            this.Amount.HeaderText = "Kwota";
            this.Amount.Name = "Amount";
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "CounterPartData";
            this.Column1.HeaderText = "Dane";
            this.Column1.Name = "Column1";
            this.Column1.Width = 62;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Title";
            this.Column3.HeaderText = "Tytuł";
            this.Column3.Name = "Column3";
            this.Column3.Width = 62;
            // 
            // Kolumna
            // 
            this.Kolumna.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Kolumna.DataPropertyName = "Details";
            this.Kolumna.HeaderText = "Szczegóły";
            this.Kolumna.Name = "Kolumna";
            this.Kolumna.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Kolumna.Width = 70;
            // 
            // Wartości
            // 
            this.Wartości.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Wartości.DataPropertyName = "AccountNumber";
            this.Wartości.HeaderText = "Konto";
            this.Wartości.Name = "Wartości";
            // 
            // Komentarz
            // 
            this.Komentarz.DataPropertyName = "Comment";
            this.Komentarz.HeaderText = "Komentarz";
            this.Komentarz.Name = "Komentarz";
            this.Komentarz.Width = 62;
            // 
            // TransactionGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridNavigator1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "TransactionGridControl";
            this.Size = new System.Drawing.Size(681, 422);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private Metaproject.WinForms.Controls.GridNavigator gridNavigator1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewImageColumn ColumnIsEdited;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Podkategoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Kolumna;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wartości;
        private System.Windows.Forms.DataGridViewTextBoxColumn Komentarz;
    }
}

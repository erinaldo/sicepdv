namespace SICEpdv
{
    partial class MesclarItens
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgItens = new System.Windows.Forms.DataGridView();
            this.btnTodos = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.vendadavBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.inc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemDAVDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codigoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.produtoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantidadeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descontovalorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.documento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgItens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendadavBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgItens
            // 
            this.dgItens.AutoGenerateColumns = false;
            this.dgItens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.inc,
            this.itemDAVDataGridViewTextBoxColumn,
            this.codigoDataGridViewTextBoxColumn,
            this.produtoDataGridViewTextBoxColumn,
            this.precoDataGridViewTextBoxColumn,
            this.quantidadeDataGridViewTextBoxColumn,
            this.descontovalorDataGridViewTextBoxColumn,
            this.total,
            this.documento});
            this.dgItens.DataSource = this.vendadavBindingSource;
            this.dgItens.Location = new System.Drawing.Point(5, 8);
            this.dgItens.Name = "dgItens";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgItens.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgItens.RowHeadersVisible = false;
            this.dgItens.RowTemplate.Height = 35;
            this.dgItens.RowTemplate.ReadOnly = true;
            this.dgItens.Size = new System.Drawing.Size(668, 384);
            this.dgItens.TabIndex = 0;
            this.dgItens.DoubleClick += new System.EventHandler(this.dgItens_DoubleClick);
            // 
            // btnTodos
            // 
            this.btnTodos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnTodos.Location = new System.Drawing.Point(5, 398);
            this.btnTodos.Name = "btnTodos";
            this.btnTodos.Size = new System.Drawing.Size(110, 40);
            this.btnTodos.TabIndex = 1;
            this.btnTodos.Text = "Incluir Todos";
            this.btnTodos.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button1.Location = new System.Drawing.Point(121, 398);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 40);
            this.button1.TabIndex = 2;
            this.button1.Text = "Tirar Todos";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // btnContinuar
            // 
            this.btnContinuar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnContinuar.Location = new System.Drawing.Point(553, 398);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(110, 40);
            this.btnContinuar.TabIndex = 3;
            this.btnContinuar.Text = "&Continuar";
            this.btnContinuar.UseVisualStyleBackColor = false;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(253, 413);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Clique duplo marca ou desmarca o item.";
            // 
            // vendadavBindingSource
            // 
            this.vendadavBindingSource.DataSource = typeof(SICEpdv.vendadav);
            // 
            // inc
            // 
            this.inc.DataPropertyName = "inc";
            this.inc.HeaderText = "inc";
            this.inc.Name = "inc";
            this.inc.Visible = false;
            // 
            // itemDAVDataGridViewTextBoxColumn
            // 
            this.itemDAVDataGridViewTextBoxColumn.DataPropertyName = "itemDAV";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemDAVDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.itemDAVDataGridViewTextBoxColumn.HeaderText = "";
            this.itemDAVDataGridViewTextBoxColumn.Name = "itemDAVDataGridViewTextBoxColumn";
            this.itemDAVDataGridViewTextBoxColumn.Width = 25;
            // 
            // codigoDataGridViewTextBoxColumn
            // 
            this.codigoDataGridViewTextBoxColumn.DataPropertyName = "codigo";
            this.codigoDataGridViewTextBoxColumn.HeaderText = "Código";
            this.codigoDataGridViewTextBoxColumn.Name = "codigoDataGridViewTextBoxColumn";
            this.codigoDataGridViewTextBoxColumn.Width = 50;
            // 
            // produtoDataGridViewTextBoxColumn
            // 
            this.produtoDataGridViewTextBoxColumn.DataPropertyName = "produto";
            this.produtoDataGridViewTextBoxColumn.HeaderText = "Produto";
            this.produtoDataGridViewTextBoxColumn.Name = "produtoDataGridViewTextBoxColumn";
            this.produtoDataGridViewTextBoxColumn.Width = 350;
            // 
            // precoDataGridViewTextBoxColumn
            // 
            this.precoDataGridViewTextBoxColumn.DataPropertyName = "preco";
            this.precoDataGridViewTextBoxColumn.HeaderText = "Preço";
            this.precoDataGridViewTextBoxColumn.Name = "precoDataGridViewTextBoxColumn";
            this.precoDataGridViewTextBoxColumn.Width = 70;
            // 
            // quantidadeDataGridViewTextBoxColumn
            // 
            this.quantidadeDataGridViewTextBoxColumn.DataPropertyName = "quantidade";
            this.quantidadeDataGridViewTextBoxColumn.HeaderText = "Qtd.";
            this.quantidadeDataGridViewTextBoxColumn.Name = "quantidadeDataGridViewTextBoxColumn";
            this.quantidadeDataGridViewTextBoxColumn.Width = 60;
            // 
            // descontovalorDataGridViewTextBoxColumn
            // 
            this.descontovalorDataGridViewTextBoxColumn.DataPropertyName = "descontovalor";
            this.descontovalorDataGridViewTextBoxColumn.HeaderText = "Desc";
            this.descontovalorDataGridViewTextBoxColumn.Name = "descontovalorDataGridViewTextBoxColumn";
            this.descontovalorDataGridViewTextBoxColumn.Width = 40;
            // 
            // total
            // 
            this.total.DataPropertyName = "total";
            this.total.HeaderText = "Total";
            this.total.Name = "total";
            this.total.Width = 70;
            // 
            // documento
            // 
            this.documento.DataPropertyName = "documento";
            this.documento.HeaderText = "Dav Nr.";
            this.documento.Name = "documento";
            // 
            // MesclarItens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 446);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnContinuar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnTodos);
            this.Controls.Add(this.dgItens);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MesclarItens";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Escolha os itens do DAV";
            ((System.ComponentModel.ISupportInitialize)(this.dgItens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendadavBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource vendadavBindingSource;
        private System.Windows.Forms.DataGridView dgItens;
        private System.Windows.Forms.Button btnTodos;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnContinuar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn inc;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemDAVDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn produtoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn precoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidadeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descontovalorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn documento;
    }
}
namespace SICEpdv
{
    partial class ListaCompraItens
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
            this.lblIQCARD = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.dtgItens = new System.Windows.Forms.DataGridView();
            this.codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.preco = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnImprimirPreco = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtgItens)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIQCARD
            // 
            this.lblIQCARD.AutoSize = true;
            this.lblIQCARD.BackColor = System.Drawing.Color.Transparent;
            this.lblIQCARD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIQCARD.Location = new System.Drawing.Point(31, 50);
            this.lblIQCARD.Name = "lblIQCARD";
            this.lblIQCARD.Size = new System.Drawing.Size(184, 20);
            this.lblIQCARD.TabIndex = 0;
            this.lblIQCARD.Text = "0000 0000 0000 0000";
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.BackColor = System.Drawing.Color.Transparent;
            this.lblNome.Location = new System.Drawing.Point(32, 72);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(48, 13);
            this.lblNome.TabIndex = 1;
            this.lblNome.Text = "IQCARD";
            // 
            // dtgItens
            // 
            this.dtgItens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigo,
            this.descricao,
            this.quantidade,
            this.preco,
            this.total});
            this.dtgItens.Location = new System.Drawing.Point(35, 100);
            this.dtgItens.Name = "dtgItens";
            this.dtgItens.RowHeadersVisible = false;
            this.dtgItens.RowTemplate.Height = 40;
            this.dtgItens.Size = new System.Drawing.Size(701, 450);
            this.dtgItens.TabIndex = 2;
            // 
            // codigo
            // 
            this.codigo.DataPropertyName = "codigo";
            this.codigo.HeaderText = "Cód.Barras";
            this.codigo.Name = "codigo";
            this.codigo.Width = 120;
            // 
            // descricao
            // 
            this.descricao.DataPropertyName = "descricao";
            this.descricao.HeaderText = "Descrição";
            this.descricao.Name = "descricao";
            this.descricao.Width = 250;
            // 
            // quantidade
            // 
            this.quantidade.DataPropertyName = "quantidade";
            this.quantidade.HeaderText = "QTD.";
            this.quantidade.Name = "quantidade";
            // 
            // preco
            // 
            this.preco.DataPropertyName = "preco";
            this.preco.HeaderText = "Preço";
            this.preco.Name = "preco";
            // 
            // total
            // 
            this.total.DataPropertyName = "total";
            this.total.HeaderText = "Total R$";
            this.total.Name = "total";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(262, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(252, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "LISTA DE COMPRA DO USUÁRIO IQCARD";
            // 
            // btnImprimir
            // 
            this.btnImprimir.Location = new System.Drawing.Point(35, 571);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(102, 36);
            this.btnImprimir.TabIndex = 4;
            this.btnImprimir.Text = "IMPRIMIR";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // btnImprimirPreco
            // 
            this.btnImprimirPreco.BackColor = System.Drawing.Color.Transparent;
            this.btnImprimirPreco.Location = new System.Drawing.Point(155, 571);
            this.btnImprimirPreco.Name = "btnImprimirPreco";
            this.btnImprimirPreco.Size = new System.Drawing.Size(137, 36);
            this.btnImprimirPreco.TabIndex = 5;
            this.btnImprimirPreco.Text = "IMPRIMIR C/PREÇO";
            this.btnImprimirPreco.UseVisualStyleBackColor = false;
            this.btnImprimirPreco.Click += new System.EventHandler(this.btnImprimirPreco_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(527, 583);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "TOTAL R$";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(676, 583);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(32, 13);
            this.lblTotal.TabIndex = 7;
            this.lblTotal.Text = "0,00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(34, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "IQCARD";
            // 
            // ListaCompraItens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SICEpdv.Properties.Resources.backgroundpadrao;
            this.ClientSize = new System.Drawing.Size(774, 660);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnImprimirPreco);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtgItens);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.lblIQCARD);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ListaCompraItens";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ITENS LISTA COMPRA";
            this.Load += new System.EventHandler(this.ListaCompraItens_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgItens)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIQCARD;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.DataGridView dtgItens;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnImprimirPreco;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn preco;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.Label label3;
    }
}
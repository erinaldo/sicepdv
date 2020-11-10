namespace SICEpdv
{
    partial class frmLotes
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
            this.dgLotes = new System.Windows.Forms.DataGridView();
            this.quantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vencimento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fabricacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codigoProduto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codigoFornecedor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantidadeDigitada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgLotes)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgLotes
            // 
            this.dgLotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLotes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.quantidade,
            this.lote,
            this.vencimento,
            this.fabricacao,
            this.codigoProduto,
            this.inc,
            this.codigoFornecedor,
            this.quantidadeDigitada});
            this.dgLotes.Location = new System.Drawing.Point(0, 73);
            this.dgLotes.Name = "dgLotes";
            this.dgLotes.RowTemplate.Height = 30;
            this.dgLotes.Size = new System.Drawing.Size(477, 334);
            this.dgLotes.TabIndex = 0;
            this.dgLotes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgLotes_KeyDown);
            this.dgLotes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgLotes_KeyPress);
            this.dgLotes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgLotes_KeyUp);
            // 
            // quantidade
            // 
            this.quantidade.DataPropertyName = "quantidade";
            this.quantidade.HeaderText = "Quantidade";
            this.quantidade.Name = "quantidade";
            // 
            // lote
            // 
            this.lote.DataPropertyName = "lote";
            this.lote.HeaderText = "Lote";
            this.lote.Name = "lote";
            // 
            // vencimento
            // 
            this.vencimento.DataPropertyName = "vencimento";
            this.vencimento.HeaderText = "Vencimento";
            this.vencimento.Name = "vencimento";
            // 
            // fabricacao
            // 
            this.fabricacao.DataPropertyName = "fabricacao";
            this.fabricacao.HeaderText = "Fabricacao";
            this.fabricacao.Name = "fabricacao";
            // 
            // codigoProduto
            // 
            this.codigoProduto.DataPropertyName = "codigoProduto";
            this.codigoProduto.HeaderText = "Produto";
            this.codigoProduto.Name = "codigoProduto";
            this.codigoProduto.Visible = false;
            // 
            // inc
            // 
            this.inc.DataPropertyName = "inc";
            this.inc.HeaderText = "inc";
            this.inc.Name = "inc";
            this.inc.Visible = false;
            // 
            // codigoFornecedor
            // 
            this.codigoFornecedor.DataPropertyName = "codigoFornecedor";
            this.codigoFornecedor.HeaderText = "codigoFornecedor";
            this.codigoFornecedor.Name = "codigoFornecedor";
            this.codigoFornecedor.Visible = false;
            // 
            // quantidadeDigitada
            // 
            this.quantidadeDigitada.DataPropertyName = "quantidadeDigitada";
            this.quantidadeDigitada.HeaderText = "quantidadeDigitada";
            this.quantidadeDigitada.Name = "quantidadeDigitada";
            this.quantidadeDigitada.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Lime;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(307, 413);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 44);
            this.button1.TabIndex = 1;
            this.button1.Text = "Escolher";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(395, 413);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 44);
            this.button2.TabIndex = 2;
            this.button2.Text = "Sair";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GrayText;
            this.panel2.Controls.Add(this.lblTitulo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(478, 67);
            this.panel2.TabIndex = 12;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTitulo.Location = new System.Drawing.Point(15, 19);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(179, 23);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Controle de Lotes";
            // 
            // frmLotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 459);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgLotes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmLotes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lotes";
            this.Load += new System.EventHandler(this.frmLotes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgLotes)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgLotes;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn lote;
        private System.Windows.Forms.DataGridViewTextBoxColumn vencimento;
        private System.Windows.Forms.DataGridViewTextBoxColumn fabricacao;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigoProduto;
        private System.Windows.Forms.DataGridViewTextBoxColumn inc;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigoFornecedor;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidadeDigitada;
    }
}
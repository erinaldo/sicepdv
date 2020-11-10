namespace SICEpdv
{
    partial class FrmPosicaoEstoqueFiliais
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
            this.dtgProdutos = new System.Windows.Forms.DataGridView();
            this.colFilial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSair = new System.Windows.Forms.Button();
            this.lblDado = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnTransf = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgProdutos)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgProdutos
            // 
            this.dtgProdutos.AllowUserToAddRows = false;
            this.dtgProdutos.AllowUserToDeleteRows = false;
            this.dtgProdutos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgProdutos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFilial,
            this.colQtd,
            this.colCodigo,
            this.colDescricao,
            this.colPrat,
            this.colDep});
            this.dtgProdutos.Location = new System.Drawing.Point(10, 78);
            this.dtgProdutos.Name = "dtgProdutos";
            this.dtgProdutos.ReadOnly = true;
            this.dtgProdutos.RowHeadersVisible = false;
            this.dtgProdutos.Size = new System.Drawing.Size(568, 279);
            this.dtgProdutos.TabIndex = 0;
            // 
            // colFilial
            // 
            this.colFilial.DataPropertyName = "filial";
            this.colFilial.HeaderText = "Filial";
            this.colFilial.Name = "colFilial";
            this.colFilial.ReadOnly = true;
            this.colFilial.Width = 60;
            // 
            // colQtd
            // 
            this.colQtd.DataPropertyName = "quantidade";
            this.colQtd.HeaderText = "Qtd.";
            this.colQtd.Name = "colQtd";
            this.colQtd.ReadOnly = true;
            this.colQtd.Width = 70;
            // 
            // colCodigo
            // 
            this.colCodigo.DataPropertyName = "codigo";
            this.colCodigo.HeaderText = "Código";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.ReadOnly = true;
            this.colCodigo.Width = 60;
            // 
            // colDescricao
            // 
            this.colDescricao.DataPropertyName = "descricao";
            this.colDescricao.HeaderText = "Descrição";
            this.colDescricao.Name = "colDescricao";
            this.colDescricao.ReadOnly = true;
            this.colDescricao.Width = 250;
            // 
            // colPrat
            // 
            this.colPrat.DataPropertyName = "prateleiras";
            this.colPrat.HeaderText = "Qtd.Prat";
            this.colPrat.Name = "colPrat";
            this.colPrat.ReadOnly = true;
            this.colPrat.Width = 70;
            // 
            // colDep
            // 
            this.colDep.DataPropertyName = "deposito";
            this.colDep.HeaderText = "Déposito";
            this.colDep.Name = "colDep";
            this.colDep.ReadOnly = true;
            this.colDep.Width = 70;
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnSair.FlatAppearance.BorderSize = 0;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.ForeColor = System.Drawing.Color.White;
            this.btnSair.Location = new System.Drawing.Point(492, 363);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(92, 43);
            this.btnSair.TabIndex = 1;
            this.btnSair.Text = "SAIR";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // lblDado
            // 
            this.lblDado.AutoSize = true;
            this.lblDado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDado.ForeColor = System.Drawing.Color.White;
            this.lblDado.Location = new System.Drawing.Point(7, 9);
            this.lblDado.Name = "lblDado";
            this.lblDado.Size = new System.Drawing.Size(47, 15);
            this.lblDado.TabIndex = 2;
            this.lblDado.Text = "label1";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.Color.White;
            this.lblInfo.Location = new System.Drawing.Point(9, 47);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(45, 16);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "label1";
            // 
            // btnTransf
            // 
            this.btnTransf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(185)))), ((int)(((byte)(93)))));
            this.btnTransf.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnTransf.FlatAppearance.BorderSize = 0;
            this.btnTransf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransf.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnTransf.ForeColor = System.Drawing.Color.White;
            this.btnTransf.Location = new System.Drawing.Point(360, 363);
            this.btnTransf.Name = "btnTransf";
            this.btnTransf.Size = new System.Drawing.Size(125, 43);
            this.btnTransf.TabIndex = 4;
            this.btnTransf.Text = "GERAR TRANSFERÊNCIA ";
            this.btnTransf.UseVisualStyleBackColor = false;
            this.btnTransf.Click += new System.EventHandler(this.btnTransf_Click);
            // 
            // FrmPosicaoEstoqueFiliais
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(590, 413);
            this.Controls.Add(this.btnTransf);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblDado);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.dtgProdutos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmPosicaoEstoqueFiliais";
            this.Opacity = 1D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estoque nas filiais";
            this.Load += new System.EventHandler(this.FrmPosicaoEstoqueFiliais_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgProdutos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgProdutos;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Label lblDado;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnTransf;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDep;
    }
}
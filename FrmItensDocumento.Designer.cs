namespace SICEpdv
{
    partial class FrmItensDocumento
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
            this.btnFechar = new System.Windows.Forms.Button();
            this.lblDocumento = new System.Windows.Forms.Label();
            this.bntAjustar = new System.Windows.Forms.Button();
            this.dtgItens = new System.Windows.Forms.DataGridView();
            this.codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barras = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tributacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.icms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cfop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cstpis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cstcofins = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cofins = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtgItens)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFechar
            // 
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Location = new System.Drawing.Point(828, 406);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(104, 43);
            this.btnFechar.TabIndex = 1;
            this.btnFechar.Text = "&Fechar";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // lblDocumento
            // 
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumento.Location = new System.Drawing.Point(4, 13);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Size = new System.Drawing.Size(51, 16);
            this.lblDocumento.TabIndex = 2;
            this.lblDocumento.Text = "label1";
            // 
            // bntAjustar
            // 
            this.bntAjustar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.bntAjustar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntAjustar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bntAjustar.Location = new System.Drawing.Point(707, 407);
            this.bntAjustar.Name = "bntAjustar";
            this.bntAjustar.Size = new System.Drawing.Size(115, 42);
            this.bntAjustar.TabIndex = 3;
            this.bntAjustar.Text = "Ajustar Itens";
            this.bntAjustar.UseVisualStyleBackColor = false;
            this.bntAjustar.Click += new System.EventHandler(this.bntAjustar_Click);
            // 
            // dtgItens
            // 
            this.dtgItens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigo,
            this.barras,
            this.descricao,
            this.tributacao,
            this.icms,
            this.cfop,
            this.cest,
            this.cstpis,
            this.pis,
            this.cstcofins,
            this.cofins});
            this.dtgItens.Location = new System.Drawing.Point(12, 48);
            this.dtgItens.Name = "dtgItens";
            this.dtgItens.Size = new System.Drawing.Size(913, 342);
            this.dtgItens.TabIndex = 4;
            // 
            // codigo
            // 
            this.codigo.DataPropertyName = "codigo";
            this.codigo.HeaderText = "Cod";
            this.codigo.Name = "codigo";
            // 
            // barras
            // 
            this.barras.DataPropertyName = "codigobarras";
            this.barras.HeaderText = "Cod Barras";
            this.barras.Name = "barras";
            // 
            // descricao
            // 
            this.descricao.DataPropertyName = "produto";
            this.descricao.HeaderText = "Descricao";
            this.descricao.Name = "descricao";
            // 
            // tributacao
            // 
            this.tributacao.DataPropertyName = "tributacao";
            this.tributacao.HeaderText = "CST";
            this.tributacao.Name = "tributacao";
            // 
            // icms
            // 
            this.icms.DataPropertyName = "icms";
            this.icms.HeaderText = "ICMS";
            this.icms.Name = "icms";
            // 
            // cfop
            // 
            this.cfop.DataPropertyName = "cfop";
            this.cfop.HeaderText = "CFOP";
            this.cfop.Name = "cfop";
            // 
            // cest
            // 
            this.cest.DataPropertyName = "cest";
            this.cest.HeaderText = "CEST";
            this.cest.Name = "cest";
            // 
            // cstpis
            // 
            this.cstpis.DataPropertyName = "cstpis";
            this.cstpis.HeaderText = "CSTPIS";
            this.cstpis.Name = "cstpis";
            // 
            // pis
            // 
            this.pis.DataPropertyName = "pis";
            this.pis.HeaderText = "PIS";
            this.pis.Name = "pis";
            // 
            // cstcofins
            // 
            this.cstcofins.DataPropertyName = "cstcofins";
            this.cstcofins.HeaderText = "CSTCOFINS";
            this.cstcofins.Name = "cstcofins";
            // 
            // cofins
            // 
            this.cofins.DataPropertyName = "cofins";
            this.cofins.HeaderText = "COFINS";
            this.cofins.Name = "cofins";
            // 
            // FrmItensDocumento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 450);
            this.Controls.Add(this.dtgItens);
            this.Controls.Add(this.bntAjustar);
            this.Controls.Add(this.lblDocumento);
            this.Controls.Add(this.btnFechar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmItensDocumento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Relação de Itens por Documento";
            this.Load += new System.EventHandler(this.FrmItensDocumento_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgItens)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Label lblDocumento;
        private System.Windows.Forms.Button bntAjustar;
        private System.Windows.Forms.DataGridView dtgItens;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn barras;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn tributacao;
        private System.Windows.Forms.DataGridViewTextBoxColumn icms;
        private System.Windows.Forms.DataGridViewTextBoxColumn cfop;
        private System.Windows.Forms.DataGridViewTextBoxColumn cest;
        private System.Windows.Forms.DataGridViewTextBoxColumn cstpis;
        private System.Windows.Forms.DataGridViewTextBoxColumn pis;
        private System.Windows.Forms.DataGridViewTextBoxColumn cstcofins;
        private System.Windows.Forms.DataGridViewTextBoxColumn cofins;
    }
}
namespace SICEpdv
{
    partial class FrmCOF
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
            this.btnAplicar = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dtgCOF = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cfop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.csticms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.picms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.picmsst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mvaicmsst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.predbcicms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.predicmsst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cstpis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ppis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cstcofins = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pcofins = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cstipi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pipi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtgCOF)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAplicar
            // 
            this.btnAplicar.BackColor = System.Drawing.Color.Green;
            this.btnAplicar.ForeColor = System.Drawing.Color.White;
            this.btnAplicar.Location = new System.Drawing.Point(463, 438);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(85, 35);
            this.btnAplicar.TabIndex = 0;
            this.btnAplicar.Text = "&Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = false;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.button2.Location = new System.Drawing.Point(554, 438);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 35);
            this.button2.TabIndex = 1;
            this.button2.Text = "&Sair";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dtgCOF
            // 
            this.dtgCOF.AllowUserToAddRows = false;
            this.dtgCOF.AllowUserToDeleteRows = false;
            this.dtgCOF.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgCOF.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.descricao,
            this.tipo,
            this.operacao,
            this.cfop,
            this.csticms,
            this.picms,
            this.picmsst,
            this.mvaicmsst,
            this.predbcicms,
            this.predicmsst,
            this.cstpis,
            this.ppis,
            this.cstcofins,
            this.pcofins,
            this.cstipi,
            this.pipi,
            this.serie});
            this.dtgCOF.Location = new System.Drawing.Point(2, 0);
            this.dtgCOF.Name = "dtgCOF";
            this.dtgCOF.ReadOnly = true;
            this.dtgCOF.RowHeadersVisible = false;
            this.dtgCOF.Size = new System.Drawing.Size(633, 432);
            this.dtgCOF.TabIndex = 2;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 30;
            // 
            // descricao
            // 
            this.descricao.DataPropertyName = "descricao";
            this.descricao.HeaderText = "Operação Descrição";
            this.descricao.Name = "descricao";
            this.descricao.ReadOnly = true;
            this.descricao.Width = 350;
            // 
            // tipo
            // 
            this.tipo.DataPropertyName = "tipo";
            this.tipo.HeaderText = "E-Entrada S-Saída";
            this.tipo.Name = "tipo";
            this.tipo.ReadOnly = true;
            this.tipo.Width = 70;
            // 
            // operacao
            // 
            this.operacao.DataPropertyName = "operacao";
            this.operacao.HeaderText = "Operação";
            this.operacao.Name = "operacao";
            this.operacao.ReadOnly = true;
            // 
            // cfop
            // 
            this.cfop.DataPropertyName = "cfop";
            this.cfop.HeaderText = "CFOP";
            this.cfop.Name = "cfop";
            this.cfop.ReadOnly = true;
            // 
            // csticms
            // 
            this.csticms.DataPropertyName = "csticms";
            this.csticms.HeaderText = "CST ICMS";
            this.csticms.Name = "csticms";
            this.csticms.ReadOnly = true;
            // 
            // picms
            // 
            this.picms.DataPropertyName = "picms";
            this.picms.HeaderText = "Aliq.ICMS";
            this.picms.Name = "picms";
            this.picms.ReadOnly = true;
            // 
            // picmsst
            // 
            this.picmsst.DataPropertyName = "picmsst";
            this.picmsst.HeaderText = "ALIQ.ICMS ST";
            this.picmsst.Name = "picmsst";
            this.picmsst.ReadOnly = true;
            // 
            // mvaicmsst
            // 
            this.mvaicmsst.DataPropertyName = "mvaicmsst";
            this.mvaicmsst.HeaderText = "MVA ICMS-ST";
            this.mvaicmsst.Name = "mvaicmsst";
            this.mvaicmsst.ReadOnly = true;
            // 
            // predbcicms
            // 
            this.predbcicms.DataPropertyName = "predbcicms";
            this.predbcicms.HeaderText = "BC ICMS";
            this.predbcicms.Name = "predbcicms";
            this.predbcicms.ReadOnly = true;
            // 
            // predicmsst
            // 
            this.predicmsst.DataPropertyName = "predicmsst";
            this.predicmsst.HeaderText = "BC ICMS ST";
            this.predicmsst.Name = "predicmsst";
            this.predicmsst.ReadOnly = true;
            // 
            // cstpis
            // 
            this.cstpis.DataPropertyName = "cstpis";
            this.cstpis.HeaderText = "CST PIS";
            this.cstpis.Name = "cstpis";
            this.cstpis.ReadOnly = true;
            // 
            // ppis
            // 
            this.ppis.DataPropertyName = "ppis";
            this.ppis.HeaderText = "%PIS";
            this.ppis.Name = "ppis";
            this.ppis.ReadOnly = true;
            // 
            // cstcofins
            // 
            this.cstcofins.DataPropertyName = "cstcofins";
            this.cstcofins.HeaderText = "CST COFINS";
            this.cstcofins.Name = "cstcofins";
            this.cstcofins.ReadOnly = true;
            // 
            // pcofins
            // 
            this.pcofins.DataPropertyName = "pcofins";
            this.pcofins.HeaderText = "%COFINS";
            this.pcofins.Name = "pcofins";
            this.pcofins.ReadOnly = true;
            // 
            // cstipi
            // 
            this.cstipi.DataPropertyName = "cstipi";
            this.cstipi.HeaderText = "CST IPI";
            this.cstipi.Name = "cstipi";
            this.cstipi.ReadOnly = true;
            // 
            // pipi
            // 
            this.pipi.DataPropertyName = "pipi";
            this.pipi.HeaderText = "%IPI";
            this.pipi.Name = "pipi";
            this.pipi.ReadOnly = true;
            // 
            // serie
            // 
            this.serie.DataPropertyName = "serie";
            this.serie.HeaderText = "Série";
            this.serie.Name = "serie";
            this.serie.ReadOnly = true;
            // 
            // FrmCOF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 475);
            this.Controls.Add(this.dtgCOF);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnAplicar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmCOF";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Código de Operações Fiscais";
            this.Load += new System.EventHandler(this.FrmCOF_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgCOF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dtgCOF;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn operacao;
        private System.Windows.Forms.DataGridViewTextBoxColumn cfop;
        private System.Windows.Forms.DataGridViewTextBoxColumn csticms;
        private System.Windows.Forms.DataGridViewTextBoxColumn picms;
        private System.Windows.Forms.DataGridViewTextBoxColumn picmsst;
        private System.Windows.Forms.DataGridViewTextBoxColumn mvaicmsst;
        private System.Windows.Forms.DataGridViewTextBoxColumn predbcicms;
        private System.Windows.Forms.DataGridViewTextBoxColumn predicmsst;
        private System.Windows.Forms.DataGridViewTextBoxColumn cstpis;
        private System.Windows.Forms.DataGridViewTextBoxColumn ppis;
        private System.Windows.Forms.DataGridViewTextBoxColumn cstcofins;
        private System.Windows.Forms.DataGridViewTextBoxColumn pcofins;
        private System.Windows.Forms.DataGridViewTextBoxColumn cstipi;
        private System.Windows.Forms.DataGridViewTextBoxColumn pipi;
        private System.Windows.Forms.DataGridViewTextBoxColumn serie;
    }
}
namespace SICEpdv
{
    partial class FrmSEF2
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
            this.label7 = new System.Windows.Forms.Label();
            this.txtAnoInv = new System.Windows.Forms.MaskedTextBox();
            this.txtNrInventario = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cboAtividade = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSair = new System.Windows.Forms.Button();
            this.btnGerar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cboFinalidade = new System.Windows.Forms.ComboBox();
            this.cboPerfil = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboEstrutura = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataFinal = new System.Windows.Forms.DateTimePicker();
            this.dataInicial = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.txtNrInventario)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(214, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 46;
            this.label7.Text = "Ano:";
            // 
            // txtAnoInv
            // 
            this.txtAnoInv.Location = new System.Drawing.Point(249, 150);
            this.txtAnoInv.Mask = "0000";
            this.txtAnoInv.Name = "txtAnoInv";
            this.txtAnoInv.Size = new System.Drawing.Size(46, 20);
            this.txtAnoInv.TabIndex = 45;
            this.txtAnoInv.Text = "0000";
            // 
            // txtNrInventario
            // 
            this.txtNrInventario.Location = new System.Drawing.Point(127, 150);
            this.txtNrInventario.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.txtNrInventario.Name = "txtNrInventario";
            this.txtNrInventario.Size = new System.Drawing.Size(81, 20);
            this.txtNrInventario.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 43;
            this.label6.Text = "Nr. Inventário:";
            // 
            // cboAtividade
            // 
            this.cboAtividade.DisplayMember = "descricao";
            this.cboAtividade.FormattingEnabled = true;
            this.cboAtividade.Items.AddRange(new object[] {
            "0 - Industrial ou equiparado a industrial;",
            "1 - Outros;"});
            this.cboAtividade.Location = new System.Drawing.Point(127, 125);
            this.cboAtividade.Name = "cboAtividade";
            this.cboAtividade.Size = new System.Drawing.Size(389, 21);
            this.cboAtividade.TabIndex = 42;
            this.cboAtividade.Text = "1 - Outros;";
            this.cboAtividade.ValueMember = "codigo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Tipo atividade:";
            // 
            // btnSair
            // 
            this.btnSair.Location = new System.Drawing.Point(433, 249);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(83, 31);
            this.btnSair.TabIndex = 40;
            this.btnSair.Text = "&Sair";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // btnGerar
            // 
            this.btnGerar.Location = new System.Drawing.Point(127, 249);
            this.btnGerar.Name = "btnGerar";
            this.btnGerar.Size = new System.Drawing.Size(109, 31);
            this.btnGerar.TabIndex = 39;
            this.btnGerar.Text = "Gerar";
            this.btnGerar.UseVisualStyleBackColor = true;
            this.btnGerar.Click += new System.EventHandler(this.btnGerar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Finalidade do Arquivo:";
            // 
            // cboFinalidade
            // 
            this.cboFinalidade.DisplayMember = "descricao";
            this.cboFinalidade.FormattingEnabled = true;
            this.cboFinalidade.Items.AddRange(new object[] {
            "0 - Remessa do arquivo original;",
            "1 - Remessa do arquivo substituo;"});
            this.cboFinalidade.Location = new System.Drawing.Point(127, 99);
            this.cboFinalidade.Name = "cboFinalidade";
            this.cboFinalidade.Size = new System.Drawing.Size(389, 21);
            this.cboFinalidade.TabIndex = 37;
            this.cboFinalidade.Text = "0 - Remessa do arquivo original;";
            this.cboFinalidade.ValueMember = "codigo";
            // 
            // cboPerfil
            // 
            this.cboPerfil.DisplayMember = "descricao";
            this.cboPerfil.FormattingEnabled = true;
            this.cboPerfil.Items.AddRange(new object[] {
            "A - Perfil A;",
            "B - Perfil B;",
            "C - Perfil C;"});
            this.cboPerfil.Location = new System.Drawing.Point(127, 72);
            this.cboPerfil.Name = "cboPerfil";
            this.cboPerfil.Size = new System.Drawing.Size(389, 21);
            this.cboPerfil.TabIndex = 36;
            this.cboPerfil.Text = "A";
            this.cboPerfil.ValueMember = "codigo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(88, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Perfil:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 26);
            this.label3.TabIndex = 34;
            this.label3.Text = "Código Id. Estrutura \r\ndo Arquivo (layout):";
            // 
            // cboEstrutura
            // 
            this.cboEstrutura.DisplayMember = "descricao";
            this.cboEstrutura.FormattingEnabled = true;
            this.cboEstrutura.Items.AddRange(new object[] {
            "004"});
            this.cboEstrutura.Location = new System.Drawing.Point(127, 45);
            this.cboEstrutura.Name = "cboEstrutura";
            this.cboEstrutura.Size = new System.Drawing.Size(389, 21);
            this.cboEstrutura.TabIndex = 33;
            this.cboEstrutura.Text = "004";
            this.cboEstrutura.ValueMember = "codigo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 26);
            this.label2.TabIndex = 32;
            this.label2.Text = "Período: Use data\r\nincial e final mês:";
            // 
            // dataFinal
            // 
            this.dataFinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dataFinal.Location = new System.Drawing.Point(231, 13);
            this.dataFinal.Name = "dataFinal";
            this.dataFinal.Size = new System.Drawing.Size(86, 20);
            this.dataFinal.TabIndex = 31;
            // 
            // dataInicial
            // 
            this.dataInicial.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dataInicial.Location = new System.Drawing.Point(127, 13);
            this.dataInicial.Name = "dataInicial";
            this.dataInicial.Size = new System.Drawing.Size(98, 20);
            this.dataInicial.TabIndex = 30;
            // 
            // FrmSEF2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 287);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtAnoInv);
            this.Controls.Add(this.txtNrInventario);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboAtividade);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnGerar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboFinalidade);
            this.Controls.Add(this.cboPerfil);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboEstrutura);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataFinal);
            this.Controls.Add(this.dataInicial);
            this.Name = "FrmSEF2";
            this.Text = "Exportação SEF2";
            ((System.ComponentModel.ISupportInitialize)(this.txtNrInventario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox txtAnoInv;
        private System.Windows.Forms.NumericUpDown txtNrInventario;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboAtividade;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button btnGerar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboFinalidade;
        private System.Windows.Forms.ComboBox cboPerfil;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboEstrutura;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dataFinal;
        private System.Windows.Forms.DateTimePicker dataInicial;
    }
}
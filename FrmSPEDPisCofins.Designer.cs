namespace SICEpdv
{
    partial class FrmSPEDPisCofins
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
            this.label6 = new System.Windows.Forms.Label();
            this.chkLstFiliais = new System.Windows.Forms.CheckedListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkTodos = new System.Windows.Forms.CheckBox();
            this.rdMapa = new System.Windows.Forms.RadioButton();
            this.rdSaidas = new System.Windows.Forms.RadioButton();
            this.rdEntradas = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.chkGerarC400 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // cboAtividade
            // 
            this.cboAtividade.DisplayMember = "descricao";
            this.cboAtividade.FormattingEnabled = true;
            this.cboAtividade.Items.AddRange(new object[] {
            "0 – Industrial ou equiparado a industrial;",
            "1 – Prestador de serviços;",
            "2 - Atividade de comércio;",
            "3 – Atividade financeira;",
            "4 – Atividade imobiliária;",
            "9 – Outros."});
            this.cboAtividade.Location = new System.Drawing.Point(121, 131);
            this.cboAtividade.Name = "cboAtividade";
            this.cboAtividade.Size = new System.Drawing.Size(389, 21);
            this.cboAtividade.TabIndex = 42;
            this.cboAtividade.Text = "1 - Outros;";
            this.cboAtividade.ValueMember = "codigo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Tipo atividade:";
            // 
            // btnSair
            // 
            this.btnSair.Location = new System.Drawing.Point(409, 312);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(101, 31);
            this.btnSair.TabIndex = 40;
            this.btnSair.Text = "&Sair";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // btnGerar
            // 
            this.btnGerar.Location = new System.Drawing.Point(409, 280);
            this.btnGerar.Name = "btnGerar";
            this.btnGerar.Size = new System.Drawing.Size(101, 31);
            this.btnGerar.TabIndex = 39;
            this.btnGerar.Text = "Gerar EFD";
            this.btnGerar.UseVisualStyleBackColor = true;
            this.btnGerar.Click += new System.EventHandler(this.btnGerar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 108);
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
            this.cboFinalidade.Location = new System.Drawing.Point(121, 105);
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
            this.cboPerfil.Location = new System.Drawing.Point(121, 78);
            this.cboPerfil.Name = "cboPerfil";
            this.cboPerfil.Size = new System.Drawing.Size(389, 21);
            this.cboPerfil.TabIndex = 36;
            this.cboPerfil.Text = "A";
            this.cboPerfil.ValueMember = "codigo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(82, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Perfil:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 51);
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
            "002"});
            this.cboEstrutura.Location = new System.Drawing.Point(121, 51);
            this.cboEstrutura.Name = "cboEstrutura";
            this.cboEstrutura.Size = new System.Drawing.Size(389, 21);
            this.cboEstrutura.TabIndex = 33;
            this.cboEstrutura.Text = "002";
            this.cboEstrutura.ValueMember = "codigo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 26);
            this.label2.TabIndex = 32;
            this.label2.Text = "Período: Use data\r\nincial e final mês:";
            // 
            // dataFinal
            // 
            this.dataFinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dataFinal.Location = new System.Drawing.Point(225, 19);
            this.dataFinal.Name = "dataFinal";
            this.dataFinal.Size = new System.Drawing.Size(86, 20);
            this.dataFinal.TabIndex = 31;
            // 
            // dataInicial
            // 
            this.dataInicial.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dataInicial.Location = new System.Drawing.Point(121, 19);
            this.dataInicial.Name = "dataInicial";
            this.dataInicial.Size = new System.Drawing.Size(98, 20);
            this.dataInicial.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(329, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(160, 26);
            this.label6.TabIndex = 64;
            this.label6.Text = "Verificar os dados cadastrais da \r\nempresa se estão preenchidos.\r\n";
            // 
            // chkLstFiliais
            // 
            this.chkLstFiliais.FormattingEnabled = true;
            this.chkLstFiliais.Location = new System.Drawing.Point(12, 177);
            this.chkLstFiliais.Name = "chkLstFiliais";
            this.chkLstFiliais.Size = new System.Drawing.Size(299, 169);
            this.chkLstFiliais.TabIndex = 66;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 67;
            this.label7.Text = "Escolhas as filiais ";
            // 
            // chkTodos
            // 
            this.chkTodos.AutoSize = true;
            this.chkTodos.Checked = true;
            this.chkTodos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTodos.Location = new System.Drawing.Point(412, 257);
            this.chkTodos.Name = "chkTodos";
            this.chkTodos.Size = new System.Drawing.Size(56, 17);
            this.chkTodos.TabIndex = 65;
            this.chkTodos.Text = "Todos";
            this.chkTodos.UseVisualStyleBackColor = true;
            this.chkTodos.CheckedChanged += new System.EventHandler(this.chkTodos_CheckedChanged);
            // 
            // rdMapa
            // 
            this.rdMapa.AutoSize = true;
            this.rdMapa.Location = new System.Drawing.Point(412, 207);
            this.rdMapa.Name = "rdMapa";
            this.rdMapa.Size = new System.Drawing.Size(94, 17);
            this.rdMapa.TabIndex = 72;
            this.rdMapa.TabStop = true;
            this.rdMapa.Text = "Mapa Resumo";
            this.rdMapa.UseVisualStyleBackColor = true;
            // 
            // rdSaidas
            // 
            this.rdSaidas.AutoSize = true;
            this.rdSaidas.Location = new System.Drawing.Point(412, 184);
            this.rdSaidas.Name = "rdSaidas";
            this.rdSaidas.Size = new System.Drawing.Size(59, 17);
            this.rdSaidas.TabIndex = 71;
            this.rdSaidas.TabStop = true;
            this.rdSaidas.Text = "Saídas";
            this.rdSaidas.UseVisualStyleBackColor = true;
            // 
            // rdEntradas
            // 
            this.rdEntradas.AutoSize = true;
            this.rdEntradas.Location = new System.Drawing.Point(412, 158);
            this.rdEntradas.Name = "rdEntradas";
            this.rdEntradas.Size = new System.Drawing.Size(67, 17);
            this.rdEntradas.TabIndex = 70;
            this.rdEntradas.TabStop = true;
            this.rdEntradas.Text = "Entradas";
            this.rdEntradas.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(317, 321);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 73;
            this.button1.Text = "Abrir pasta..";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkGerarC400
            // 
            this.chkGerarC400.AutoSize = true;
            this.chkGerarC400.Location = new System.Drawing.Point(412, 230);
            this.chkGerarC400.Name = "chkGerarC400";
            this.chkGerarC400.Size = new System.Drawing.Size(103, 17);
            this.chkGerarC400.TabIndex = 74;
            this.chkGerarC400.Text = "ECF - Reg C400";
            this.toolTip1.SetToolTip(this.chkGerarC400, "Registro C400 para mostrar a movimentação por ECF");
            this.chkGerarC400.UseVisualStyleBackColor = true;
            // 
            // FrmSPEDPisCofins
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 356);
            this.Controls.Add(this.chkGerarC400);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rdMapa);
            this.Controls.Add(this.rdSaidas);
            this.Controls.Add(this.rdEntradas);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkLstFiliais);
            this.Controls.Add(this.chkTodos);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmSPEDPisCofins";
            this.Text = "SPED Pis/Cofins";
            this.Load += new System.EventHandler(this.FrmSPEDPisCofins_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckedListBox chkLstFiliais;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkTodos;
        private System.Windows.Forms.RadioButton rdMapa;
        private System.Windows.Forms.RadioButton rdSaidas;
        private System.Windows.Forms.RadioButton rdEntradas;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkGerarC400;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
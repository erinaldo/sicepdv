namespace SICEpdv
{
    partial class FrmSEF
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
            this.btnSair = new System.Windows.Forms.Button();
            this.btnGerar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cboFinalidade = new System.Windows.Forms.ComboBox();
            this.cboNatureza = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboEstrutura = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataFinal = new System.Windows.Forms.DateTimePicker();
            this.dataInicial = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAnoInv = new System.Windows.Forms.MaskedTextBox();
            this.txtNrInventario = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rdMapa = new System.Windows.Forms.RadioButton();
            this.rdSaidas = new System.Windows.Forms.RadioButton();
            this.rdEntradas = new System.Windows.Forms.RadioButton();
            this.rdInventario = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtNrInventario)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSair
            // 
            this.btnSair.Location = new System.Drawing.Point(430, 270);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(80, 31);
            this.btnSair.TabIndex = 23;
            this.btnSair.Text = "&Sair";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // btnGerar
            // 
            this.btnGerar.Location = new System.Drawing.Point(121, 269);
            this.btnGerar.Name = "btnGerar";
            this.btnGerar.Size = new System.Drawing.Size(109, 31);
            this.btnGerar.TabIndex = 22;
            this.btnGerar.Text = "Gerar SEF";
            this.btnGerar.UseVisualStyleBackColor = true;
            this.btnGerar.Click += new System.EventHandler(this.btnGerar_Click_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Finalidade do Arquivo:";
            // 
            // cboFinalidade
            // 
            this.cboFinalidade.DisplayMember = "descricao";
            this.cboFinalidade.FormattingEnabled = true;
            this.cboFinalidade.Location = new System.Drawing.Point(121, 118);
            this.cboFinalidade.Name = "cboFinalidade";
            this.cboFinalidade.Size = new System.Drawing.Size(389, 21);
            this.cboFinalidade.TabIndex = 20;
            this.cboFinalidade.ValueMember = "codigo";
            // 
            // cboNatureza
            // 
            this.cboNatureza.DisplayMember = "descricao";
            this.cboNatureza.FormattingEnabled = true;
            this.cboNatureza.Location = new System.Drawing.Point(121, 79);
            this.cboNatureza.Name = "cboNatureza";
            this.cboNatureza.Size = new System.Drawing.Size(389, 21);
            this.cboNatureza.TabIndex = 19;
            this.cboNatureza.ValueMember = "codigo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 26);
            this.label4.TabIndex = 18;
            this.label4.Text = "Código ID. Natureza \r\nOperação:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 26);
            this.label3.TabIndex = 17;
            this.label3.Text = "Código Id. Estrutura \r\ndo Arquivo (layout):";
            // 
            // cboEstrutura
            // 
            this.cboEstrutura.DisplayMember = "descricao";
            this.cboEstrutura.FormattingEnabled = true;
            this.cboEstrutura.Location = new System.Drawing.Point(121, 41);
            this.cboEstrutura.Name = "cboEstrutura";
            this.cboEstrutura.Size = new System.Drawing.Size(389, 21);
            this.cboEstrutura.TabIndex = 16;
            this.cboEstrutura.ValueMember = "codigo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 26);
            this.label2.TabIndex = 15;
            this.label2.Text = "Período: Use data\r\nincial e final mês:";
            // 
            // dataFinal
            // 
            this.dataFinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dataFinal.Location = new System.Drawing.Point(225, 9);
            this.dataFinal.Name = "dataFinal";
            this.dataFinal.Size = new System.Drawing.Size(86, 20);
            this.dataFinal.TabIndex = 14;
            // 
            // dataInicial
            // 
            this.dataInicial.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dataInicial.Location = new System.Drawing.Point(121, 9);
            this.dataInicial.Name = "dataInicial";
            this.dataInicial.Size = new System.Drawing.Size(98, 20);
            this.dataInicial.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(208, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 50;
            this.label7.Text = "Ano:";
            // 
            // txtAnoInv
            // 
            this.txtAnoInv.Location = new System.Drawing.Point(243, 147);
            this.txtAnoInv.Mask = "0000";
            this.txtAnoInv.Name = "txtAnoInv";
            this.txtAnoInv.Size = new System.Drawing.Size(46, 20);
            this.txtAnoInv.TabIndex = 49;
            this.txtAnoInv.Text = "0000";
            // 
            // txtNrInventario
            // 
            this.txtNrInventario.Location = new System.Drawing.Point(121, 147);
            this.txtNrInventario.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.txtNrInventario.Name = "txtNrInventario";
            this.txtNrInventario.Size = new System.Drawing.Size(81, 20);
            this.txtNrInventario.TabIndex = 48;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 47;
            this.label6.Text = "Mês Inventário:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(41, 243);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "Verificar os dados cadastrais da empresa se estão preenchidos.\r\n";
            // 
            // rdMapa
            // 
            this.rdMapa.AutoSize = true;
            this.rdMapa.Location = new System.Drawing.Point(125, 186);
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
            this.rdSaidas.Location = new System.Drawing.Point(56, 209);
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
            this.rdEntradas.Location = new System.Drawing.Point(56, 186);
            this.rdEntradas.Name = "rdEntradas";
            this.rdEntradas.Size = new System.Drawing.Size(67, 17);
            this.rdEntradas.TabIndex = 70;
            this.rdEntradas.TabStop = true;
            this.rdEntradas.Text = "Entradas";
            this.rdEntradas.UseVisualStyleBackColor = true;
            // 
            // rdInventario
            // 
            this.rdInventario.AutoSize = true;
            this.rdInventario.Location = new System.Drawing.Point(295, 145);
            this.rdInventario.Name = "rdInventario";
            this.rdInventario.Size = new System.Drawing.Size(72, 17);
            this.rdInventario.TabIndex = 69;
            this.rdInventario.TabStop = true;
            this.rdInventario.Text = "Inventario";
            this.rdInventario.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 277);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 74;
            this.button1.Text = "Abrir pasta..";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmSEF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 302);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rdMapa);
            this.Controls.Add(this.rdSaidas);
            this.Controls.Add(this.rdEntradas);
            this.Controls.Add(this.rdInventario);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtAnoInv);
            this.Controls.Add(this.txtNrInventario);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnGerar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboFinalidade);
            this.Controls.Add(this.cboNatureza);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboEstrutura);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataFinal);
            this.Controls.Add(this.dataInicial);
            this.Name = "FrmSEF";
            this.Text = "SEF";
            this.Load += new System.EventHandler(this.FrmSEF_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtNrInventario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button btnGerar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboFinalidade;
        private System.Windows.Forms.ComboBox cboNatureza;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboEstrutura;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dataFinal;
        private System.Windows.Forms.DateTimePicker dataInicial;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox txtAnoInv;
        private System.Windows.Forms.NumericUpDown txtNrInventario;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdMapa;
        private System.Windows.Forms.RadioButton rdSaidas;
        private System.Windows.Forms.RadioButton rdEntradas;
        private System.Windows.Forms.RadioButton rdInventario;
        private System.Windows.Forms.Button button1;

    }
}
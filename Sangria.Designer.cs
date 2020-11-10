namespace SICEpdv
{
    partial class Sangria
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
            this.pnlTeclado = new System.Windows.Forms.Panel();
            this.lblValor = new System.Windows.Forms.Label();
            this.btnSair = new System.Windows.Forms.Button();
            this.grpDespesas = new System.Windows.Forms.GroupBox();
            this.cboSubConta = new System.Windows.Forms.ComboBox();
            this.cboConta = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpOpcao = new System.Windows.Forms.GroupBox();
            this.chkDebito = new System.Windows.Forms.CheckBox();
            this.chkCredito = new System.Windows.Forms.CheckBox();
            this.grbBanco = new System.Windows.Forms.GroupBox();
            this.txtCheque = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboContaBancaria = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHistorico = new System.Windows.Forms.TextBox();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.erroValor = new System.Windows.Forms.ErrorProvider(this.components);
            this.btConsultar = new System.Windows.Forms.Button();
            this.cboPagamento = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDevolucao = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.cboCaixa = new System.Windows.Forms.ComboBox();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.chkCaixa = new System.Windows.Forms.CheckBox();
            this.grpDespesas.SuspendLayout();
            this.grpOpcao.SuspendLayout();
            this.grbBanco.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.erroValor)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTeclado
            // 
            this.pnlTeclado.Location = new System.Drawing.Point(456, 18);
            this.pnlTeclado.Name = "pnlTeclado";
            this.pnlTeclado.Size = new System.Drawing.Size(225, 275);
            this.pnlTeclado.TabIndex = 0;
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValor.ForeColor = System.Drawing.Color.White;
            this.lblValor.Location = new System.Drawing.Point(39, 23);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(38, 14);
            this.lblValor.TabIndex = 1;
            this.lblValor.Text = "Valor:";
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnSair.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.ForeColor = System.Drawing.Color.White;
            this.btnSair.Location = new System.Drawing.Point(592, 364);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(89, 43);
            this.btnSair.TabIndex = 7;
            this.btnSair.Text = "SAIR";
            this.btnSair.UseVisualStyleBackColor = false;
            // 
            // grpDespesas
            // 
            this.grpDespesas.Controls.Add(this.cboSubConta);
            this.grpDespesas.Controls.Add(this.cboConta);
            this.grpDespesas.Controls.Add(this.label2);
            this.grpDespesas.Controls.Add(this.label1);
            this.grpDespesas.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDespesas.ForeColor = System.Drawing.Color.White;
            this.grpDespesas.Location = new System.Drawing.Point(5, 108);
            this.grpDespesas.Name = "grpDespesas";
            this.grpDespesas.Size = new System.Drawing.Size(438, 84);
            this.grpDespesas.TabIndex = 3;
            this.grpDespesas.TabStop = false;
            this.grpDespesas.Text = "Contas de Despesas";
            // 
            // cboSubConta
            // 
            this.cboSubConta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSubConta.FormattingEnabled = true;
            this.cboSubConta.Location = new System.Drawing.Point(78, 52);
            this.cboSubConta.Name = "cboSubConta";
            this.cboSubConta.Size = new System.Drawing.Size(352, 28);
            this.cboSubConta.TabIndex = 1;
            // 
            // cboConta
            // 
            this.cboConta.DisplayMember = "conta";
            this.cboConta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboConta.FormattingEnabled = true;
            this.cboConta.Location = new System.Drawing.Point(78, 23);
            this.cboConta.Name = "cboConta";
            this.cboConta.Size = new System.Drawing.Size(352, 28);
            this.cboConta.TabIndex = 0;
            this.cboConta.ValueMember = "conta";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(14, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sub-Conta:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(36, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Conta:";
            // 
            // grpOpcao
            // 
            this.grpOpcao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.grpOpcao.Controls.Add(this.chkDebito);
            this.grpOpcao.Controls.Add(this.chkCredito);
            this.grpOpcao.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpOpcao.ForeColor = System.Drawing.Color.White;
            this.grpOpcao.Location = new System.Drawing.Point(5, 243);
            this.grpOpcao.Name = "grpOpcao";
            this.grpOpcao.Size = new System.Drawing.Size(191, 66);
            this.grpOpcao.TabIndex = 4;
            this.grpOpcao.TabStop = false;
            this.grpOpcao.Text = "Opções";
            // 
            // chkDebito
            // 
            this.chkDebito.AutoSize = true;
            this.chkDebito.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDebito.ForeColor = System.Drawing.Color.White;
            this.chkDebito.Location = new System.Drawing.Point(8, 36);
            this.chkDebito.Name = "chkDebito";
            this.chkDebito.Size = new System.Drawing.Size(111, 18);
            this.chkDebito.TabIndex = 1;
            this.chkDebito.Text = "Débito Bancário";
            this.chkDebito.UseVisualStyleBackColor = true;
            // 
            // chkCredito
            // 
            this.chkCredito.AutoSize = true;
            this.chkCredito.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCredito.ForeColor = System.Drawing.Color.White;
            this.chkCredito.Location = new System.Drawing.Point(8, 16);
            this.chkCredito.Name = "chkCredito";
            this.chkCredito.Size = new System.Drawing.Size(177, 18);
            this.chkCredito.TabIndex = 0;
            this.chkCredito.Text = "Crédito Bancário (Depósito)";
            this.chkCredito.UseVisualStyleBackColor = true;
            // 
            // grbBanco
            // 
            this.grbBanco.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.grbBanco.Controls.Add(this.txtCheque);
            this.grbBanco.Controls.Add(this.label4);
            this.grbBanco.Controls.Add(this.cboContaBancaria);
            this.grbBanco.Controls.Add(this.label3);
            this.grbBanco.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbBanco.ForeColor = System.Drawing.Color.White;
            this.grbBanco.Location = new System.Drawing.Point(202, 240);
            this.grbBanco.Name = "grbBanco";
            this.grbBanco.Size = new System.Drawing.Size(241, 69);
            this.grbBanco.TabIndex = 5;
            this.grbBanco.TabStop = false;
            this.grbBanco.Text = "Lançamento Bancário";
            // 
            // txtCheque
            // 
            this.txtCheque.Location = new System.Drawing.Point(78, 44);
            this.txtCheque.MaxLength = 15;
            this.txtCheque.Name = "txtCheque";
            this.txtCheque.Size = new System.Drawing.Size(100, 20);
            this.txtCheque.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "Cheque Nº:";
            // 
            // cboContaBancaria
            // 
            this.cboContaBancaria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboContaBancaria.FormattingEnabled = true;
            this.cboContaBancaria.Items.AddRange(new object[] {
            "Nenhuma"});
            this.cboContaBancaria.Location = new System.Drawing.Point(78, 20);
            this.cboContaBancaria.Name = "cboContaBancaria";
            this.cboContaBancaria.Size = new System.Drawing.Size(145, 22);
            this.cboContaBancaria.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(36, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "Conta:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(26, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 14);
            this.label5.TabIndex = 12;
            this.label5.Text = "Histórico:";
            // 
            // txtHistorico
            // 
            this.txtHistorico.Location = new System.Drawing.Point(23, 341);
            this.txtHistorico.Multiline = true;
            this.txtHistorico.Name = "txtHistorico";
            this.txtHistorico.Size = new System.Drawing.Size(349, 66);
            this.txtHistorico.TabIndex = 6;
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btnConfirmar.FlatAppearance.BorderSize = 0;
            this.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmar.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmar.ForeColor = System.Drawing.Color.White;
            this.btnConfirmar.Location = new System.Drawing.Point(386, 364);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(102, 43);
            this.btnConfirmar.TabIndex = 7;
            this.btnConfirmar.Text = "CONFIRMAR";
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // erroValor
            // 
            this.erroValor.ContainerControl = this;
            // 
            // btConsultar
            // 
            this.btConsultar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btConsultar.FlatAppearance.BorderSize = 0;
            this.btConsultar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btConsultar.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btConsultar.ForeColor = System.Drawing.Color.White;
            this.btConsultar.Location = new System.Drawing.Point(494, 364);
            this.btConsultar.Name = "btConsultar";
            this.btConsultar.Size = new System.Drawing.Size(92, 43);
            this.btConsultar.TabIndex = 8;
            this.btConsultar.Text = "CONSULTAR";
            this.btConsultar.UseVisualStyleBackColor = false;
            this.btConsultar.Click += new System.EventHandler(this.ConsultarSangria);
            // 
            // cboPagamento
            // 
            this.cboPagamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPagamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPagamento.FormattingEnabled = true;
            this.cboPagamento.Items.AddRange(new object[] {
            "DH - Dinheiro"});
            this.cboPagamento.Location = new System.Drawing.Point(272, 30);
            this.cboPagamento.Name = "cboPagamento";
            this.cboPagamento.Size = new System.Drawing.Size(110, 24);
            this.cboPagamento.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(272, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 14);
            this.label6.TabIndex = 14;
            this.label6.Text = "Tp. Pagamento";
            // 
            // btnDevolucao
            // 
            this.btnDevolucao.Location = new System.Drawing.Point(272, 67);
            this.btnDevolucao.Name = "btnDevolucao";
            this.btnDevolucao.Size = new System.Drawing.Size(110, 37);
            this.btnDevolucao.TabIndex = 15;
            this.btnDevolucao.Text = "Devolução Produtos";
            this.toolTip1.SetToolTip(this.btnDevolucao, "Essa devolução acontece quando houver devolução de valores para o cliente. ");
            this.btnDevolucao.UseVisualStyleBackColor = true;
            this.btnDevolucao.Click += new System.EventHandler(this.btnDevolucao_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 1000;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(87, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 14);
            this.label7.TabIndex = 16;
            this.label7.Text = "Transferir para o Caixa:\r\n";
            // 
            // cboCaixa
            // 
            this.cboCaixa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCaixa.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCaixa.FormattingEnabled = true;
            this.cboCaixa.Location = new System.Drawing.Point(70, 76);
            this.cboCaixa.Name = "cboCaixa";
            this.cboCaixa.Size = new System.Drawing.Size(145, 26);
            this.cboCaixa.TabIndex = 2;
            this.cboCaixa.Enter += new System.EventHandler(this.cboCaixa_Enter);
            // 
            // txtValor
            // 
            this.txtValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValor.Location = new System.Drawing.Point(92, 18);
            this.txtValor.MaxLength = 9;
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(124, 26);
            this.txtValor.TabIndex = 0;
            // 
            // chkCaixa
            // 
            this.chkCaixa.AutoSize = true;
            this.chkCaixa.Location = new System.Drawing.Point(70, 55);
            this.chkCaixa.Name = "chkCaixa";
            this.chkCaixa.Size = new System.Drawing.Size(15, 14);
            this.chkCaixa.TabIndex = 17;
            this.chkCaixa.UseVisualStyleBackColor = true;
            this.chkCaixa.Click += new System.EventHandler(this.chkCaixa_Click);
            // 
            // Sangria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(693, 419);
            this.Controls.Add(this.chkCaixa);
            this.Controls.Add(this.cboCaixa);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnDevolucao);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboPagamento);
            this.Controls.Add(this.btConsultar);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.txtHistorico);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.grbBanco);
            this.Controls.Add(this.grpOpcao);
            this.Controls.Add(this.grpDespesas);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.txtValor);
            this.Controls.Add(this.lblValor);
            this.Controls.Add(this.pnlTeclado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "Sangria";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sangria - F7 Menu Fiscal";
            this.Load += new System.EventHandler(this.Sangria_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Sangria_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Sangria_KeyPress);
            this.grpDespesas.ResumeLayout(false);
            this.grpDespesas.PerformLayout();
            this.grpOpcao.ResumeLayout(false);
            this.grpOpcao.PerformLayout();
            this.grbBanco.ResumeLayout(false);
            this.grbBanco.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.erroValor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTeclado;
        private System.Windows.Forms.Label lblValor;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.GroupBox grpDespesas;
        private System.Windows.Forms.ComboBox cboSubConta;
        private System.Windows.Forms.ComboBox cboConta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpOpcao;
        private System.Windows.Forms.CheckBox chkDebito;
        private System.Windows.Forms.CheckBox chkCredito;
        private System.Windows.Forms.GroupBox grbBanco;
        private System.Windows.Forms.ComboBox cboContaBancaria;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCheque;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtHistorico;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.ErrorProvider erroValor;
        private System.Windows.Forms.Button btConsultar;
        private System.Windows.Forms.ComboBox cboPagamento;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDevolucao;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboCaixa;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.CheckBox chkCaixa;
    }
}
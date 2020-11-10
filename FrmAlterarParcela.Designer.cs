namespace SICEpdv
{
    partial class FrmAlterarParcela
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgParcelas = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipopagamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vencimento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parcela = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btCancelar = new System.Windows.Forms.Button();
            this.btAlterar = new System.Windows.Forms.Button();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.pnlAlterar = new System.Windows.Forms.Panel();
            this.pnlCheque = new System.Windows.Forms.Panel();
            this.txtCPFCNPJch = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtNomeCH = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtNrCheque = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAgencia = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCodBanco = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancelarAlteracao = new System.Windows.Forms.Button();
            this.btnConfirmarAlteracao = new System.Windows.Forms.Button();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVencimento = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSugerirParcelas = new System.Windows.Forms.CheckBox();
            this.lblTotalOriginal = new System.Windows.Forms.Label();
            this.lblTotalCalculado = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtgParcelas)).BeginInit();
            this.pnlAlterar.SuspendLayout();
            this.pnlCheque.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtgParcelas
            // 
            this.dtgParcelas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgParcelas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.tipopagamento,
            this.vencimento,
            this.valor,
            this.parcela,
            this.nome});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgParcelas.DefaultCellStyle = dataGridViewCellStyle1;
            this.dtgParcelas.Location = new System.Drawing.Point(3, 1);
            this.dtgParcelas.Name = "dtgParcelas";
            this.dtgParcelas.RowHeadersVisible = false;
            this.dtgParcelas.Size = new System.Drawing.Size(506, 307);
            this.dtgParcelas.TabIndex = 0;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "Id";
            this.id.Name = "id";
            this.id.Visible = false;
            this.id.Width = 20;
            // 
            // tipopagamento
            // 
            this.tipopagamento.DataPropertyName = "tipopagamento";
            this.tipopagamento.HeaderText = "T.Pgt";
            this.tipopagamento.Name = "tipopagamento";
            // 
            // vencimento
            // 
            this.vencimento.DataPropertyName = "vencimento";
            this.vencimento.HeaderText = "Vencimento";
            this.vencimento.Name = "vencimento";
            // 
            // valor
            // 
            this.valor.DataPropertyName = "valor";
            this.valor.HeaderText = "Valor";
            this.valor.Name = "valor";
            this.valor.Width = 80;
            // 
            // parcela
            // 
            this.parcela.DataPropertyName = "parcela";
            this.parcela.HeaderText = "Parc.";
            this.parcela.Name = "parcela";
            this.parcela.Width = 49;
            // 
            // nome
            // 
            this.nome.DataPropertyName = "nome";
            this.nome.HeaderText = "Cliente";
            this.nome.Name = "nome";
            this.nome.Width = 350;
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(378, 314);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(94, 37);
            this.btCancelar.TabIndex = 1;
            this.btCancelar.Text = "&Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // btAlterar
            // 
            this.btAlterar.Location = new System.Drawing.Point(3, 314);
            this.btAlterar.Name = "btAlterar";
            this.btAlterar.Size = new System.Drawing.Size(105, 37);
            this.btAlterar.TabIndex = 2;
            this.btAlterar.Text = "&Alterar";
            this.btAlterar.UseVisualStyleBackColor = true;
            this.btAlterar.Click += new System.EventHandler(this.btAlterar_Click);
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Location = new System.Drawing.Point(114, 314);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(94, 37);
            this.btnConfirmar.TabIndex = 3;
            this.btnConfirmar.Text = "&Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // pnlAlterar
            // 
            this.pnlAlterar.Controls.Add(this.chkSugerirParcelas);
            this.pnlAlterar.Controls.Add(this.pnlCheque);
            this.pnlAlterar.Controls.Add(this.btnCancelarAlteracao);
            this.pnlAlterar.Controls.Add(this.btnConfirmarAlteracao);
            this.pnlAlterar.Controls.Add(this.txtValor);
            this.pnlAlterar.Controls.Add(this.label2);
            this.pnlAlterar.Controls.Add(this.txtVencimento);
            this.pnlAlterar.Controls.Add(this.label1);
            this.pnlAlterar.Location = new System.Drawing.Point(46, 47);
            this.pnlAlterar.Name = "pnlAlterar";
            this.pnlAlterar.Size = new System.Drawing.Size(395, 234);
            this.pnlAlterar.TabIndex = 4;
            this.pnlAlterar.Visible = false;
            // 
            // pnlCheque
            // 
            this.pnlCheque.AutoSize = true;
            this.pnlCheque.Controls.Add(this.txtCPFCNPJch);
            this.pnlCheque.Controls.Add(this.label13);
            this.pnlCheque.Controls.Add(this.txtNomeCH);
            this.pnlCheque.Controls.Add(this.label12);
            this.pnlCheque.Controls.Add(this.txtNrCheque);
            this.pnlCheque.Controls.Add(this.label11);
            this.pnlCheque.Controls.Add(this.txtAgencia);
            this.pnlCheque.Controls.Add(this.label10);
            this.pnlCheque.Controls.Add(this.txtCodBanco);
            this.pnlCheque.Controls.Add(this.label9);
            this.pnlCheque.Location = new System.Drawing.Point(27, 71);
            this.pnlCheque.Name = "pnlCheque";
            this.pnlCheque.Size = new System.Drawing.Size(348, 114);
            this.pnlCheque.TabIndex = 3;
            this.pnlCheque.Visible = false;
            // 
            // txtCPFCNPJch
            // 
            this.txtCPFCNPJch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCPFCNPJch.Location = new System.Drawing.Point(109, 81);
            this.txtCPFCNPJch.MaxLength = 14;
            this.txtCPFCNPJch.Name = "txtCPFCNPJch";
            this.txtCPFCNPJch.Size = new System.Drawing.Size(99, 24);
            this.txtCPFCNPJch.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 81);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "CPF/CNPJ Ch.";
            // 
            // txtNomeCH
            // 
            this.txtNomeCH.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeCH.Location = new System.Drawing.Point(109, 51);
            this.txtNomeCH.MaxLength = 30;
            this.txtNomeCH.Name = "txtNomeCH";
            this.txtNomeCH.Size = new System.Drawing.Size(228, 24);
            this.txtNomeCH.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 52);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Nome do Cheque:";
            // 
            // txtNrCheque
            // 
            this.txtNrCheque.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNrCheque.Location = new System.Drawing.Point(251, 19);
            this.txtNrCheque.MaxLength = 10;
            this.txtNrCheque.Name = "txtNrCheque";
            this.txtNrCheque.Size = new System.Drawing.Size(86, 26);
            this.txtNrCheque.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(248, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Cheque Nr.:";
            // 
            // txtAgencia
            // 
            this.txtAgencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAgencia.Location = new System.Drawing.Point(177, 19);
            this.txtAgencia.MaxLength = 8;
            this.txtAgencia.Name = "txtAgencia";
            this.txtAgencia.Size = new System.Drawing.Size(65, 26);
            this.txtAgencia.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(174, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Agência:";
            // 
            // txtCodBanco
            // 
            this.txtCodBanco.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodBanco.Location = new System.Drawing.Point(109, 19);
            this.txtCodBanco.MaxLength = 10;
            this.txtCodBanco.Name = "txtCodBanco";
            this.txtCodBanco.Size = new System.Drawing.Size(59, 26);
            this.txtCodBanco.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(106, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Banco";
            // 
            // btnCancelarAlteracao
            // 
            this.btnCancelarAlteracao.Location = new System.Drawing.Point(204, 191);
            this.btnCancelarAlteracao.Name = "btnCancelarAlteracao";
            this.btnCancelarAlteracao.Size = new System.Drawing.Size(75, 34);
            this.btnCancelarAlteracao.TabIndex = 5;
            this.btnCancelarAlteracao.Text = "Cancelar";
            this.btnCancelarAlteracao.UseVisualStyleBackColor = true;
            this.btnCancelarAlteracao.Click += new System.EventHandler(this.btnCancelarAlteracao_Click);
            // 
            // btnConfirmarAlteracao
            // 
            this.btnConfirmarAlteracao.Location = new System.Drawing.Point(115, 191);
            this.btnConfirmarAlteracao.Name = "btnConfirmarAlteracao";
            this.btnConfirmarAlteracao.Size = new System.Drawing.Size(83, 34);
            this.btnConfirmarAlteracao.TabIndex = 4;
            this.btnConfirmarAlteracao.Text = "Confirmar";
            this.btnConfirmarAlteracao.UseVisualStyleBackColor = true;
            this.btnConfirmarAlteracao.Click += new System.EventHandler(this.btnConfirmarAlteracao_Click);
            // 
            // txtValor
            // 
            this.txtValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValor.Location = new System.Drawing.Point(89, 43);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(106, 24);
            this.txtValor.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Valor R$:";
            // 
            // txtVencimento
            // 
            this.txtVencimento.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVencimento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtVencimento.Location = new System.Drawing.Point(89, 10);
            this.txtVencimento.Name = "txtVencimento";
            this.txtVencimento.Size = new System.Drawing.Size(106, 24);
            this.txtVencimento.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vencimento:";
            // 
            // chkSugerirParcelas
            // 
            this.chkSugerirParcelas.AutoSize = true;
            this.chkSugerirParcelas.Location = new System.Drawing.Point(201, 46);
            this.chkSugerirParcelas.Name = "chkSugerirParcelas";
            this.chkSugerirParcelas.Size = new System.Drawing.Size(103, 17);
            this.chkSugerirParcelas.TabIndex = 6;
            this.chkSugerirParcelas.Text = "Sugerir Parcelas";
            this.chkSugerirParcelas.UseVisualStyleBackColor = true;
            // 
            // lblTotalOriginal
            // 
            this.lblTotalOriginal.AutoSize = true;
            this.lblTotalOriginal.Location = new System.Drawing.Point(215, 317);
            this.lblTotalOriginal.Name = "lblTotalOriginal";
            this.lblTotalOriginal.Size = new System.Drawing.Size(13, 13);
            this.lblTotalOriginal.TabIndex = 5;
            this.lblTotalOriginal.Text = "0";
            // 
            // lblTotalCalculado
            // 
            this.lblTotalCalculado.AutoSize = true;
            this.lblTotalCalculado.Location = new System.Drawing.Point(215, 333);
            this.lblTotalCalculado.Name = "lblTotalCalculado";
            this.lblTotalCalculado.Size = new System.Drawing.Size(13, 13);
            this.lblTotalCalculado.TabIndex = 6;
            this.lblTotalCalculado.Text = "0";
            // 
            // FrmAlterarParcela
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 387);
            this.Controls.Add(this.lblTotalCalculado);
            this.Controls.Add(this.lblTotalOriginal);
            this.Controls.Add(this.pnlAlterar);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.btAlterar);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.dtgParcelas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "FrmAlterarParcela";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alterar parcelas e vencimento";
            this.Shown += new System.EventHandler(this.FrmAlterarParcela_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmAlterarParcela_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dtgParcelas)).EndInit();
            this.pnlAlterar.ResumeLayout(false);
            this.pnlAlterar.PerformLayout();
            this.pnlCheque.ResumeLayout(false);
            this.pnlCheque.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgParcelas;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btAlterar;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Panel pnlAlterar;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker txtVencimento;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelarAlteracao;
        private System.Windows.Forms.Button btnConfirmarAlteracao;
        public System.Windows.Forms.Panel pnlCheque;
        public System.Windows.Forms.TextBox txtCPFCNPJch;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.TextBox txtNomeCH;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.TextBox txtNrCheque;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.TextBox txtAgencia;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox txtCodBanco;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipopagamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn vencimento;
        private System.Windows.Forms.DataGridViewTextBoxColumn valor;
        private System.Windows.Forms.DataGridViewTextBoxColumn parcela;
        private System.Windows.Forms.DataGridViewTextBoxColumn nome;
        private System.Windows.Forms.CheckBox chkSugerirParcelas;
        private System.Windows.Forms.Label lblTotalOriginal;
        private System.Windows.Forms.Label lblTotalCalculado;
    }
}
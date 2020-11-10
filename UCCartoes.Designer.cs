namespace SICEpdv
{
    partial class UCCartoes
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlValorCA = new System.Windows.Forms.Panel();
            this.txtNomeCartao = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNrCartao = new System.Windows.Forms.TextBox();
            this.btSairCA = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtValorIndCA = new System.Windows.Forms.TextBox();
            this.btConfirmaCA = new System.Windows.Forms.Button();
            this.txtParcelamentoCA = new System.Windows.Forms.TextBox();
            this.lblCAParcelas = new System.Windows.Forms.Label();
            this.pnlCartoes = new System.Windows.Forms.Panel();
            this.txtCodigoCartao = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlValorCA.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlValorCA
            // 
            this.pnlValorCA.Controls.Add(this.txtNomeCartao);
            this.pnlValorCA.Controls.Add(this.label3);
            this.pnlValorCA.Controls.Add(this.txtNrCartao);
            this.pnlValorCA.Controls.Add(this.btSairCA);
            this.pnlValorCA.Controls.Add(this.label2);
            this.pnlValorCA.Controls.Add(this.label1);
            this.pnlValorCA.Controls.Add(this.txtValorIndCA);
            this.pnlValorCA.Controls.Add(this.btConfirmaCA);
            this.pnlValorCA.Controls.Add(this.txtParcelamentoCA);
            this.pnlValorCA.Controls.Add(this.lblCAParcelas);
            this.pnlValorCA.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlValorCA.Enabled = false;
            this.pnlValorCA.Location = new System.Drawing.Point(0, 355);
            this.pnlValorCA.Name = "pnlValorCA";
            this.pnlValorCA.Size = new System.Drawing.Size(488, 176);
            this.pnlValorCA.TabIndex = 0;
            // 
            // txtNomeCartao
            // 
            this.txtNomeCartao.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeCartao.Location = new System.Drawing.Point(153, 45);
            this.txtNomeCartao.MaxLength = 30;
            this.txtNomeCartao.Name = "txtNomeCartao";
            this.txtNomeCartao.Size = new System.Drawing.Size(216, 29);
            this.txtNomeCartao.TabIndex = 5;
            this.txtNomeCartao.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNomeCartao_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(72, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 14);
            this.label3.TabIndex = 15;
            this.label3.Text = "Nome Cartão:";
            // 
            // txtNrCartao
            // 
            this.txtNrCartao.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNrCartao.Location = new System.Drawing.Point(153, 4);
            this.txtNrCartao.MaxLength = 16;
            this.txtNrCartao.Name = "txtNrCartao";
            this.txtNrCartao.Size = new System.Drawing.Size(216, 29);
            this.txtNrCartao.TabIndex = 4;
            this.txtNrCartao.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNrCartao_KeyDown);
            this.txtNrCartao.Leave += new System.EventHandler(this.txtNrCartao_Leave);
            // 
            // btSairCA
            // 
            this.btSairCA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btSairCA.FlatAppearance.BorderSize = 0;
            this.btSairCA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSairCA.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btSairCA.Location = new System.Drawing.Point(203, 133);
            this.btSairCA.Name = "btSairCA";
            this.btSairCA.Size = new System.Drawing.Size(80, 34);
            this.btSairCA.TabIndex = 6;
            this.btSairCA.Text = "CANCELAR";
            this.btSairCA.UseVisualStyleBackColor = false;
            this.btSairCA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btSairCA_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(86, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 13;
            this.label2.Text = "Nr. Cartão:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(107, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 14);
            this.label1.TabIndex = 12;
            this.label1.Text = "Total.:";
            // 
            // txtValorIndCA
            // 
            this.txtValorIndCA.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorIndCA.Location = new System.Drawing.Point(153, 84);
            this.txtValorIndCA.Name = "txtValorIndCA";
            this.txtValorIndCA.Size = new System.Drawing.Size(152, 31);
            this.txtValorIndCA.TabIndex = 0;
            this.txtValorIndCA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValorIndCA_KeyDown);
            // 
            // btConfirmaCA
            // 
            this.btConfirmaCA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btConfirmaCA.FlatAppearance.BorderSize = 0;
            this.btConfirmaCA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btConfirmaCA.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btConfirmaCA.Location = new System.Drawing.Point(289, 133);
            this.btConfirmaCA.Name = "btConfirmaCA";
            this.btConfirmaCA.Size = new System.Drawing.Size(88, 34);
            this.btConfirmaCA.TabIndex = 3;
            this.btConfirmaCA.Text = "CONFIRMAR";
            this.btConfirmaCA.UseVisualStyleBackColor = false;
            this.btConfirmaCA.Click += new System.EventHandler(this.btConfirmaCA_Click);
            this.btConfirmaCA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btConfirmaCA_KeyDown);
            // 
            // txtParcelamentoCA
            // 
            this.txtParcelamentoCA.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParcelamentoCA.Location = new System.Drawing.Point(331, 84);
            this.txtParcelamentoCA.MaxLength = 2;
            this.txtParcelamentoCA.Name = "txtParcelamentoCA";
            this.txtParcelamentoCA.Size = new System.Drawing.Size(38, 31);
            this.txtParcelamentoCA.TabIndex = 2;
            this.txtParcelamentoCA.Text = "1";
            this.txtParcelamentoCA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtParcelamentoCA_KeyDown);
            // 
            // lblCAParcelas
            // 
            this.lblCAParcelas.AutoSize = true;
            this.lblCAParcelas.Location = new System.Drawing.Point(311, 95);
            this.lblCAParcelas.Name = "lblCAParcelas";
            this.lblCAParcelas.Size = new System.Drawing.Size(14, 13);
            this.lblCAParcelas.TabIndex = 7;
            this.lblCAParcelas.Text = "X";
            // 
            // pnlCartoes
            // 
            this.pnlCartoes.AutoScroll = true;
            this.pnlCartoes.Location = new System.Drawing.Point(19, 104);
            this.pnlCartoes.Name = "pnlCartoes";
            this.pnlCartoes.Size = new System.Drawing.Size(454, 245);
            this.pnlCartoes.TabIndex = 2;
            // 
            // txtCodigoCartao
            // 
            this.txtCodigoCartao.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoCartao.Location = new System.Drawing.Point(153, 72);
            this.txtCodigoCartao.Name = "txtCodigoCartao";
            this.txtCodigoCartao.Size = new System.Drawing.Size(183, 29);
            this.txtCodigoCartao.TabIndex = 1;
            this.txtCodigoCartao.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodigoCartao_KeyDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(342, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GrayText;
            this.panel2.Controls.Add(this.lblTitulo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(488, 67);
            this.panel2.TabIndex = 12;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTitulo.Location = new System.Drawing.Point(15, 19);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(274, 23);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Informe os dados do cartão";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(88, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 13;
            this.label4.Text = "Nº Cartão.:";
            // 
            // UCCartoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtCodigoCartao);
            this.Controls.Add(this.pnlCartoes);
            this.Controls.Add(this.pnlValorCA);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "UCCartoes";
            this.Size = new System.Drawing.Size(488, 531);
            this.Load += new System.EventHandler(this.UCCartoes_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UCCartoes_KeyDown);
            this.pnlValorCA.ResumeLayout(false);
            this.pnlValorCA.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCAParcelas;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtParcelamentoCA;
        public System.Windows.Forms.TextBox txtValorIndCA;
        private System.Windows.Forms.Button btSairCA;
        public System.Windows.Forms.Panel pnlValorCA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtNrCartao;
        public System.Windows.Forms.TextBox txtNomeCartao;
        public System.Windows.Forms.Button btConfirmaCA;
        private System.Windows.Forms.Panel pnlCartoes;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtCodigoCartao;
    }
}

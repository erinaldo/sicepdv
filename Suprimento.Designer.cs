namespace SICEpdv
{
    partial class Suprimento
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
            this.pnlTeclado = new System.Windows.Forms.Panel();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSaldoAnt = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnTeclado = new System.Windows.Forms.Button();
            this.btnAbrirGaveta = new System.Windows.Forms.Button();
            this.pnlValor = new System.Windows.Forms.Panel();
            this.pnlValor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTeclado
            // 
            this.pnlTeclado.Location = new System.Drawing.Point(376, 28);
            this.pnlTeclado.Name = "pnlTeclado";
            this.pnlTeclado.Size = new System.Drawing.Size(215, 179);
            this.pnlTeclado.TabIndex = 2;
            this.pnlTeclado.Visible = false;
            // 
            // txtValor
            // 
            this.txtValor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtValor.Font = new System.Drawing.Font("Noto Sans Cond", 18F);
            this.txtValor.Location = new System.Drawing.Point(79, 18);
            this.txtValor.MaxLength = 7;
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(191, 33);
            this.txtValor.TabIndex = 0;
            this.txtValor.Text = "0,00";
            this.txtValor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValor_KeyDown);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(86, 126);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(132, 49);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSaldoAnt
            // 
            this.btnSaldoAnt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnSaldoAnt.Location = new System.Drawing.Point(25, 182);
            this.btnSaldoAnt.Name = "btnSaldoAnt";
            this.btnSaldoAnt.Size = new System.Drawing.Size(65, 40);
            this.btnSaldoAnt.TabIndex = 5;
            this.btnSaldoAnt.Text = "Saldo anterior";
            this.btnSaldoAnt.UseVisualStyleBackColor = false;
            this.btnSaldoAnt.Visible = false;
            this.btnSaldoAnt.Click += new System.EventHandler(this.btnSaldoAnt_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(185)))), ((int)(((byte)(93)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Corbel", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(224, 127);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(137, 48);
            this.button2.TabIndex = 6;
            this.button2.Text = "CONFIRMAR";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Suprimento_KeyDown);
            // 
            // btnTeclado
            // 
            this.btnTeclado.BackColor = System.Drawing.Color.Transparent;
            this.btnTeclado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTeclado.FlatAppearance.BorderSize = 0;
            this.btnTeclado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTeclado.Location = new System.Drawing.Point(272, 14);
            this.btnTeclado.Name = "btnTeclado";
            this.btnTeclado.Size = new System.Drawing.Size(47, 41);
            this.btnTeclado.TabIndex = 7;
            this.btnTeclado.UseVisualStyleBackColor = false;
            this.btnTeclado.Click += new System.EventHandler(this.btnTeclado_Click);
            // 
            // btnAbrirGaveta
            // 
            this.btnAbrirGaveta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbrirGaveta.FlatAppearance.BorderSize = 0;
            this.btnAbrirGaveta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirGaveta.Location = new System.Drawing.Point(25, 126);
            this.btnAbrirGaveta.Name = "btnAbrirGaveta";
            this.btnAbrirGaveta.Size = new System.Drawing.Size(55, 49);
            this.btnAbrirGaveta.TabIndex = 4;
            this.btnAbrirGaveta.UseVisualStyleBackColor = true;
            this.btnAbrirGaveta.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnlValor
            // 
            this.pnlValor.Controls.Add(this.txtValor);
            this.pnlValor.Controls.Add(this.btnTeclado);
            this.pnlValor.Location = new System.Drawing.Point(26, 37);
            this.pnlValor.Name = "pnlValor";
            this.pnlValor.Size = new System.Drawing.Size(335, 65);
            this.pnlValor.TabIndex = 8;
            // 
            // Suprimento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(601, 234);
            this.Controls.Add(this.pnlValor);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnSaldoAnt);
            this.Controls.Add(this.btnAbrirGaveta);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.pnlTeclado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Suprimento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Abertura de Caixa - Suprimento";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VerificaSaldoInicial);
            this.Load += new System.EventHandler(this.Suprimento_Load);
            this.Shown += new System.EventHandler(this.Suprimento_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Suprimento_KeyDown);
            this.pnlValor.ResumeLayout(false);
            this.pnlValor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTeclado;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAbrirGaveta;
        private System.Windows.Forms.Button btnSaldoAnt;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnTeclado;
        private System.Windows.Forms.Panel pnlValor;
    }
}
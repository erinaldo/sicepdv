namespace SICEpdv
{
    partial class FrmInformacoesCliente
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
            this.lblNomeCliente = new System.Windows.Forms.Label();
            this.lblCpfCliente = new System.Windows.Forms.Label();
            this.lblTelefoneCliente = new System.Windows.Forms.Label();
            this.lblEndereco = new System.Windows.Forms.Label();
            this.lblCidadeCliente = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblCepCliente = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNomeCliente
            // 
            this.lblNomeCliente.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomeCliente.ForeColor = System.Drawing.Color.White;
            this.lblNomeCliente.Location = new System.Drawing.Point(220, 42);
            this.lblNomeCliente.Name = "lblNomeCliente";
            this.lblNomeCliente.Size = new System.Drawing.Size(391, 70);
            this.lblNomeCliente.TabIndex = 0;
            this.lblNomeCliente.Text = "NOME DO CLIENTE";
            // 
            // lblCpfCliente
            // 
            this.lblCpfCliente.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCpfCliente.ForeColor = System.Drawing.Color.White;
            this.lblCpfCliente.Location = new System.Drawing.Point(222, 112);
            this.lblCpfCliente.Name = "lblCpfCliente";
            this.lblCpfCliente.Size = new System.Drawing.Size(389, 29);
            this.lblCpfCliente.TabIndex = 1;
            this.lblCpfCliente.Text = "CPF / CNPJ";
            this.lblCpfCliente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTelefoneCliente
            // 
            this.lblTelefoneCliente.AutoSize = true;
            this.lblTelefoneCliente.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblTelefoneCliente.ForeColor = System.Drawing.Color.White;
            this.lblTelefoneCliente.Location = new System.Drawing.Point(21, 201);
            this.lblTelefoneCliente.Name = "lblTelefoneCliente";
            this.lblTelefoneCliente.Size = new System.Drawing.Size(175, 19);
            this.lblTelefoneCliente.TabIndex = 2;
            this.lblTelefoneCliente.Text = "Fone: (87) 3822 - 4299";
            // 
            // lblEndereco
            // 
            this.lblEndereco.AutoSize = true;
            this.lblEndereco.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblEndereco.ForeColor = System.Drawing.Color.White;
            this.lblEndereco.Location = new System.Drawing.Point(21, 230);
            this.lblEndereco.Name = "lblEndereco";
            this.lblEndereco.Size = new System.Drawing.Size(262, 19);
            this.lblEndereco.TabIndex = 3;
            this.lblEndereco.Text = "Rua Alguma Coisa, Nº 56 - Centro";
            // 
            // lblCidadeCliente
            // 
            this.lblCidadeCliente.AutoSize = true;
            this.lblCidadeCliente.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblCidadeCliente.ForeColor = System.Drawing.Color.White;
            this.lblCidadeCliente.Location = new System.Drawing.Point(21, 257);
            this.lblCidadeCliente.Name = "lblCidadeCliente";
            this.lblCidadeCliente.Size = new System.Drawing.Size(123, 19);
            this.lblCidadeCliente.TabIndex = 4;
            this.lblCidadeCliente.Text = "Arcoverde - PE";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(21, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(180, 180);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // lblCepCliente
            // 
            this.lblCepCliente.AutoSize = true;
            this.lblCepCliente.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblCepCliente.ForeColor = System.Drawing.Color.White;
            this.lblCepCliente.Location = new System.Drawing.Point(21, 284);
            this.lblCepCliente.Name = "lblCepCliente";
            this.lblCepCliente.Size = new System.Drawing.Size(94, 19);
            this.lblCepCliente.TabIndex = 6;
            this.lblCepCliente.Text = "56515 - 140";
            // 
            // FrmInformacoesCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(632, 327);
            this.Controls.Add(this.lblCepCliente);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblCidadeCliente);
            this.Controls.Add(this.lblEndereco);
            this.Controls.Add(this.lblTelefoneCliente);
            this.Controls.Add(this.lblCpfCliente);
            this.Controls.Add(this.lblNomeCliente);
            this.Name = "FrmInformacoesCliente";
            this.Text = "FrmInformacoesCliente";
            this.Load += new System.EventHandler(this.FrmInformacoesCliente_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNomeCliente;
        private System.Windows.Forms.Label lblCpfCliente;
        private System.Windows.Forms.Label lblTelefoneCliente;
        private System.Windows.Forms.Label lblEndereco;
        private System.Windows.Forms.Label lblCidadeCliente;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblCepCliente;
    }
}
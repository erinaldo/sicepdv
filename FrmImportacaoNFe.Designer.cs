namespace SICEpdv
{
    partial class FrmImportacaoNFe
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkTransfEntrada = new System.Windows.Forms.RadioButton();
            this.chkDevClienteEmpresa = new System.Windows.Forms.RadioButton();
            this.chkEntrada = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkPerdas = new System.Windows.Forms.RadioButton();
            this.chkTransfSaida = new System.Windows.Forms.RadioButton();
            this.chkDevEmpresaFornecedor = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNumero = new System.Windows.Forms.TextBox();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkTransfEntrada);
            this.groupBox1.Controls.Add(this.chkDevClienteEmpresa);
            this.groupBox1.Controls.Add(this.chkEntrada);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 108);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Importação de Entradas";
            // 
            // chkTransfEntrada
            // 
            this.chkTransfEntrada.AutoSize = true;
            this.chkTransfEntrada.Location = new System.Drawing.Point(10, 65);
            this.chkTransfEntrada.Name = "chkTransfEntrada";
            this.chkTransfEntrada.Size = new System.Drawing.Size(130, 17);
            this.chkTransfEntrada.TabIndex = 2;
            this.chkTransfEntrada.TabStop = true;
            this.chkTransfEntrada.Text = "Transferência Entrada";
            this.chkTransfEntrada.UseVisualStyleBackColor = true;
            // 
            // chkDevClienteEmpresa
            // 
            this.chkDevClienteEmpresa.AutoSize = true;
            this.chkDevClienteEmpresa.Location = new System.Drawing.Point(10, 42);
            this.chkDevClienteEmpresa.Name = "chkDevClienteEmpresa";
            this.chkDevClienteEmpresa.Size = new System.Drawing.Size(156, 17);
            this.chkDevClienteEmpresa.TabIndex = 1;
            this.chkDevClienteEmpresa.TabStop = true;
            this.chkDevClienteEmpresa.Text = "Devolução Cliente Empresa";
            this.chkDevClienteEmpresa.UseVisualStyleBackColor = true;
            // 
            // chkEntrada
            // 
            this.chkEntrada.AutoSize = true;
            this.chkEntrada.Location = new System.Drawing.Point(10, 19);
            this.chkEntrada.Name = "chkEntrada";
            this.chkEntrada.Size = new System.Drawing.Size(128, 17);
            this.chkEntrada.TabIndex = 0;
            this.chkEntrada.TabStop = true;
            this.chkEntrada.Text = "Entradas de uma NFe";
            this.chkEntrada.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkPerdas);
            this.groupBox2.Controls.Add(this.chkTransfSaida);
            this.groupBox2.Controls.Add(this.chkDevEmpresaFornecedor);
            this.groupBox2.Location = new System.Drawing.Point(215, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(207, 108);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Importação de Saídas";
            // 
            // chkPerdas
            // 
            this.chkPerdas.AutoSize = true;
            this.chkPerdas.Location = new System.Drawing.Point(10, 65);
            this.chkPerdas.Name = "chkPerdas";
            this.chkPerdas.Size = new System.Drawing.Size(154, 17);
            this.chkPerdas.TabIndex = 2;
            this.chkPerdas.TabStop = true;
            this.chkPerdas.Text = "Perdas, Avarias e Doações";
            this.chkPerdas.UseVisualStyleBackColor = true;
            // 
            // chkTransfSaida
            // 
            this.chkTransfSaida.AutoSize = true;
            this.chkTransfSaida.Location = new System.Drawing.Point(10, 42);
            this.chkTransfSaida.Name = "chkTransfSaida";
            this.chkTransfSaida.Size = new System.Drawing.Size(122, 17);
            this.chkTransfSaida.TabIndex = 1;
            this.chkTransfSaida.TabStop = true;
            this.chkTransfSaida.Text = "Transferência Saída";
            this.chkTransfSaida.UseVisualStyleBackColor = true;
            // 
            // chkDevEmpresaFornecedor
            // 
            this.chkDevEmpresaFornecedor.AutoSize = true;
            this.chkDevEmpresaFornecedor.Location = new System.Drawing.Point(10, 19);
            this.chkDevEmpresaFornecedor.Name = "chkDevEmpresaFornecedor";
            this.chkDevEmpresaFornecedor.Size = new System.Drawing.Size(178, 17);
            this.chkDevEmpresaFornecedor.TabIndex = 0;
            this.chkDevEmpresaFornecedor.TabStop = true;
            this.chkDevEmpresaFornecedor.Text = "Devolução Empresa Fornecedor";
            this.chkDevEmpresaFornecedor.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 225);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Número:";
            // 
            // txtNumero
            // 
            this.txtNumero.BackColor = System.Drawing.Color.Yellow;
            this.txtNumero.Location = new System.Drawing.Point(57, 222);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Size = new System.Drawing.Size(100, 20);
            this.txtNumero.TabIndex = 3;
            // 
            // btnImportar
            // 
            this.btnImportar.BackColor = System.Drawing.Color.Green;
            this.btnImportar.ForeColor = System.Drawing.Color.White;
            this.btnImportar.Location = new System.Drawing.Point(163, 211);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(79, 40);
            this.btnImportar.TabIndex = 4;
            this.btnImportar.Text = "&Importar";
            this.btnImportar.UseVisualStyleBackColor = false;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnSair.Location = new System.Drawing.Point(342, 211);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(80, 40);
            this.btnSair.TabIndex = 5;
            this.btnSair.Text = "&Sair";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // FrmImportacaoNFe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 263);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.txtNumero);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmImportacaoNFe";
            this.Text = "Importação de Operações Gerenciais";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton chkDevClienteEmpresa;
        private System.Windows.Forms.RadioButton chkEntrada;
        private System.Windows.Forms.RadioButton chkTransfEntrada;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton chkPerdas;
        private System.Windows.Forms.RadioButton chkTransfSaida;
        private System.Windows.Forms.RadioButton chkDevEmpresaFornecedor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNumero;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnSair;
    }
}
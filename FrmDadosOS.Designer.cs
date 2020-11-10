namespace SICEpdv
{
    partial class FrmDadosOS
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
            this.pnlOSAuto = new System.Windows.Forms.Panel();
            this.tbPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtMarca = new System.Windows.Forms.TextBox();
            this.txtRENAVAM = new System.Windows.Forms.TextBox();
            this.txtPlaca = new System.Windows.Forms.MaskedTextBox();
            this.txtAno = new System.Windows.Forms.MaskedTextBox();
            this.txtModelo = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtNrFabricacao = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNF = new System.Windows.Forms.TextBox();
            this.pnlOSAuto.SuspendLayout();
            this.tbPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOSAuto
            // 
            this.pnlOSAuto.Controls.Add(this.txtNF);
            this.pnlOSAuto.Controls.Add(this.label1);
            this.pnlOSAuto.Controls.Add(this.tbPanel);
            this.pnlOSAuto.Location = new System.Drawing.Point(3, 3);
            this.pnlOSAuto.Name = "pnlOSAuto";
            this.pnlOSAuto.Size = new System.Drawing.Size(302, 239);
            this.pnlOSAuto.TabIndex = 48;
            // 
            // tbPanel
            // 
            this.tbPanel.ColumnCount = 2;
            this.tbPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.46154F));
            this.tbPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.53846F));
            this.tbPanel.Controls.Add(this.label12, 0, 1);
            this.tbPanel.Controls.Add(this.label13, 0, 2);
            this.tbPanel.Controls.Add(this.label14, 0, 4);
            this.tbPanel.Controls.Add(this.label15, 0, 5);
            this.tbPanel.Controls.Add(this.txtMarca, 1, 2);
            this.tbPanel.Controls.Add(this.txtRENAVAM, 1, 5);
            this.tbPanel.Controls.Add(this.txtPlaca, 1, 1);
            this.tbPanel.Controls.Add(this.txtAno, 1, 4);
            this.tbPanel.Controls.Add(this.txtModelo, 1, 3);
            this.tbPanel.Controls.Add(this.label17, 0, 3);
            this.tbPanel.Controls.Add(this.txtNrFabricacao, 1, 0);
            this.tbPanel.Controls.Add(this.label16, 0, 0);
            this.tbPanel.Location = new System.Drawing.Point(19, 44);
            this.tbPanel.Name = "tbPanel";
            this.tbPanel.RowCount = 6;
            this.tbPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tbPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tbPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tbPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tbPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tbPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tbPanel.Size = new System.Drawing.Size(263, 186);
            this.tbPanel.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "PLACA:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 57);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Marca";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 123);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Ano:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 159);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 13);
            this.label15.TabIndex = 3;
            this.label15.Text = "RENAVAM:";
            // 
            // txtMarca
            // 
            this.txtMarca.BackColor = System.Drawing.Color.Yellow;
            this.txtMarca.Location = new System.Drawing.Point(91, 60);
            this.txtMarca.MaxLength = 50;
            this.txtMarca.Name = "txtMarca";
            this.txtMarca.Size = new System.Drawing.Size(167, 20);
            this.txtMarca.TabIndex = 2;
            // 
            // txtRENAVAM
            // 
            this.txtRENAVAM.BackColor = System.Drawing.Color.Yellow;
            this.txtRENAVAM.Location = new System.Drawing.Point(91, 162);
            this.txtRENAVAM.MaxLength = 100;
            this.txtRENAVAM.Name = "txtRENAVAM";
            this.txtRENAVAM.Size = new System.Drawing.Size(167, 20);
            this.txtRENAVAM.TabIndex = 5;
            // 
            // txtPlaca
            // 
            this.txtPlaca.BackColor = System.Drawing.Color.Yellow;
            this.txtPlaca.Location = new System.Drawing.Point(91, 30);
            this.txtPlaca.Mask = "AAA-0000";
            this.txtPlaca.Name = "txtPlaca";
            this.txtPlaca.Size = new System.Drawing.Size(100, 20);
            this.txtPlaca.TabIndex = 1;
            // 
            // txtAno
            // 
            this.txtAno.BackColor = System.Drawing.Color.Yellow;
            this.txtAno.Location = new System.Drawing.Point(91, 126);
            this.txtAno.Mask = "0000";
            this.txtAno.Name = "txtAno";
            this.txtAno.Size = new System.Drawing.Size(63, 20);
            this.txtAno.TabIndex = 4;
            // 
            // txtModelo
            // 
            this.txtModelo.BackColor = System.Drawing.Color.Yellow;
            this.txtModelo.Location = new System.Drawing.Point(91, 90);
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Size = new System.Drawing.Size(167, 20);
            this.txtModelo.TabIndex = 3;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 87);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(45, 13);
            this.label17.TabIndex = 8;
            this.label17.Text = "Modelo:";
            // 
            // txtNrFabricacao
            // 
            this.txtNrFabricacao.Location = new System.Drawing.Point(91, 3);
            this.txtNrFabricacao.Name = "txtNrFabricacao";
            this.txtNrFabricacao.Size = new System.Drawing.Size(100, 20);
            this.txtNrFabricacao.TabIndex = 0;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "Nr.Fabricação:";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(217, 248);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(88, 39);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Limpar Dados\r\ne Fechar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnConfirmar.Location = new System.Drawing.Point(115, 248);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(96, 39);
            this.btnConfirmar.TabIndex = 6;
            this.btnConfirmar.Text = "&Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nota Fiscal:";
            // 
            // txtNF
            // 
            this.txtNF.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNF.Location = new System.Drawing.Point(92, 9);
            this.txtNF.MaxLength = 9;
            this.txtNF.Name = "txtNF";
            this.txtNF.Size = new System.Drawing.Size(118, 24);
            this.txtNF.TabIndex = 2;
            // 
            // FrmDadosOS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 289);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.pnlOSAuto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmDadosOS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dados OS";
            this.Load += new System.EventHandler(this.FrmDadosOS_Load);
            this.Shown += new System.EventHandler(this.FrmDadosOS_Shown);
            this.pnlOSAuto.ResumeLayout(false);
            this.pnlOSAuto.PerformLayout();
            this.tbPanel.ResumeLayout(false);
            this.tbPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlOSAuto;
        private System.Windows.Forms.TableLayoutPanel tbPanel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtMarca;
        private System.Windows.Forms.TextBox txtRENAVAM;
        private System.Windows.Forms.MaskedTextBox txtPlaca;
        private System.Windows.Forms.MaskedTextBox txtAno;
        private System.Windows.Forms.TextBox txtNrFabricacao;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtModelo;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.TextBox txtNF;
        private System.Windows.Forms.Label label1;
    }
}
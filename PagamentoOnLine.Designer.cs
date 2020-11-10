namespace SICEpdv
{
    partial class PagamentoOnLine
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
            this.label2 = new System.Windows.Forms.Label();
            this.gerarBoleto = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pagSeguro = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pnlInfo = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gerarBoleto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pagSeguro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(98, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(332, 38);
            this.label2.TabIndex = 3;
            this.label2.Text = "Obrigado por confiar em nossos serviços. \r\nA IQ SISTEMAS agradece a parceria!";
            // 
            // gerarBoleto
            // 
            this.gerarBoleto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gerarBoleto.Image = global::SICEpdv.Properties.Resources.boleto;
            this.gerarBoleto.Location = new System.Drawing.Point(136, 85);
            this.gerarBoleto.Name = "gerarBoleto";
            this.gerarBoleto.Size = new System.Drawing.Size(225, 76);
            this.gerarBoleto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.gerarBoleto.TabIndex = 5;
            this.gerarBoleto.TabStop = false;
            this.gerarBoleto.Click += new System.EventHandler(this.gerarBoleto_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::SICEpdv.Properties.Resources.paypal;
            this.pictureBox1.Location = new System.Drawing.Point(136, 295);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(225, 107);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pagSeguro
            // 
            this.pagSeguro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pagSeguro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pagSeguro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pagSeguro.Image = global::SICEpdv.Properties.Resources.pagseg;
            this.pagSeguro.Location = new System.Drawing.Point(136, 174);
            this.pagSeguro.Name = "pagSeguro";
            this.pagSeguro.Size = new System.Drawing.Size(225, 112);
            this.pagSeguro.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pagSeguro.TabIndex = 0;
            this.pagSeguro.TabStop = false;
            this.pagSeguro.Click += new System.EventHandler(this.pagSeguro_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Location = new System.Drawing.Point(54, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 42);
            this.label1.TabIndex = 6;
            this.label1.Text = "Caso já tenha efetuado  o  pagamento envie \r\no   comprovante  pelo  whatasapp  ou" +
    "   entre \r\nem contato com o financeiro (87) 9 9991 4118";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SICEpdv.Properties.Resources.whatsapp;
            this.pictureBox2.Location = new System.Drawing.Point(18, 38);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(30, 30);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.label1);
            this.pnlInfo.Controls.Add(this.pictureBox2);
            this.pnlInfo.Location = new System.Drawing.Point(81, 408);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(310, 100);
            this.pnlInfo.TabIndex = 8;
            // 
            // PagamentoOnLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(523, 573);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.gerarBoleto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pagSeguro);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PagamentoOnLine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cartão de Crédito";
            this.Load += new System.EventHandler(this.PagamentoOnLine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gerarBoleto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pagSeguro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pagSeguro;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox gerarBoleto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel pnlInfo;
    }
}
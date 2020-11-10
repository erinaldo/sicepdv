namespace SICEpdv
{
    partial class AvisoVencimento
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AvisoVencimento));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlChave = new System.Windows.Forms.Panel();
            this.lblInfoStatus = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlChave.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(214, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 104);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(186, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(255, 39);
            this.label2.TabIndex = 2;
            this.label2.Text = "Prezado cliente,  lembramos  que a  sua fatura no \r\nvalor  de XX,XXX  se vence am" +
    "anhã.  Não deixe de \r\nefetuar o pagamento. Evite o pagamento de juros. \r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(238, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Mensagem para o cliente";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SICEpdv.Properties.Resources.iqcard;
            this.pictureBox1.Location = new System.Drawing.Point(-9, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(177, 179);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pnlChave
            // 
            this.pnlChave.BackColor = System.Drawing.Color.Transparent;
            this.pnlChave.Controls.Add(this.lblInfoStatus);
            this.pnlChave.Controls.Add(this.lblStatus);
            this.pnlChave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlChave.Location = new System.Drawing.Point(241, 218);
            this.pnlChave.Name = "pnlChave";
            this.pnlChave.Size = new System.Drawing.Size(146, 36);
            this.pnlChave.TabIndex = 16;
            // 
            // lblInfoStatus
            // 
            this.lblInfoStatus.AutoSize = true;
            this.lblInfoStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblInfoStatus.ForeColor = System.Drawing.Color.Black;
            this.lblInfoStatus.Location = new System.Drawing.Point(49, 10);
            this.lblInfoStatus.Name = "lblInfoStatus";
            this.lblInfoStatus.Size = new System.Drawing.Size(82, 13);
            this.lblInfoStatus.TabIndex = 5;
            this.lblInfoStatus.Text = "Lembrete ligado";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Lime;
            this.lblStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(12, 7);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(31, 18);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "ON";
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // AvisoVencimento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SICEpdv.Properties.Resources.backgroundpadrao;
            this.ClientSize = new System.Drawing.Size(435, 291);
            this.Controls.Add(this.pnlChave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AvisoVencimento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lembrete vencimento";
            this.Load += new System.EventHandler(this.AvisoVencimento_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlChave.ResumeLayout(false);
            this.pnlChave.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlChave;
        private System.Windows.Forms.Label lblInfoStatus;
        private System.Windows.Forms.Label lblStatus;
    }
}
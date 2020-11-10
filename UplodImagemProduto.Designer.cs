namespace SICEpdv
{
    partial class UplodImagemProduto
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
            this.pnlEnviar = new System.Windows.Forms.Panel();
            this.btnDownload = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblProduto = new System.Windows.Forms.Label();
            this.btnEscolher = new System.Windows.Forms.Button();
            this.fotoPrd = new System.Windows.Forms.PictureBox();
            this.btnApagar = new System.Windows.Forms.PictureBox();
            this.pnlEnviar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fotoPrd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnApagar)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlEnviar
            // 
            this.pnlEnviar.Controls.Add(this.btnDownload);
            this.pnlEnviar.Controls.Add(this.lblInfo);
            this.pnlEnviar.Controls.Add(this.lblProduto);
            this.pnlEnviar.Controls.Add(this.btnEscolher);
            this.pnlEnviar.Location = new System.Drawing.Point(42, 295);
            this.pnlEnviar.Name = "pnlEnviar";
            this.pnlEnviar.Size = new System.Drawing.Size(437, 110);
            this.pnlEnviar.TabIndex = 5;
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btnDownload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDownload.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnDownload.ForeColor = System.Drawing.Color.White;
            this.btnDownload.Image = global::SICEpdv.Properties.Resources.abaixo1;
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnDownload.Location = new System.Drawing.Point(247, 52);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(122, 50);
            this.btnDownload.TabIndex = 5;
            this.btnDownload.Text = "BAIXAR";
            this.btnDownload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Visible = false;
            this.btnDownload.Click += new System.EventHandler(this.btnEnviar_Click_1);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblInfo.Location = new System.Drawing.Point(26, 242);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(0, 13);
            this.lblInfo.TabIndex = 3;
            // 
            // lblProduto
            // 
            this.lblProduto.AutoSize = true;
            this.lblProduto.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblProduto.ForeColor = System.Drawing.Color.White;
            this.lblProduto.Location = new System.Drawing.Point(13, 15);
            this.lblProduto.Name = "lblProduto";
            this.lblProduto.Size = new System.Drawing.Size(125, 14);
            this.lblProduto.TabIndex = 1;
            this.lblProduto.Text = "Descrição do produto";
            // 
            // btnEscolher
            // 
            this.btnEscolher.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btnEscolher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEscolher.FlatAppearance.BorderSize = 0;
            this.btnEscolher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEscolher.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEscolher.ForeColor = System.Drawing.Color.White;
            this.btnEscolher.Image = global::SICEpdv.Properties.Resources.cloud_upload;
            this.btnEscolher.Location = new System.Drawing.Point(82, 52);
            this.btnEscolher.Name = "btnEscolher";
            this.btnEscolher.Size = new System.Drawing.Size(132, 50);
            this.btnEscolher.TabIndex = 0;
            this.btnEscolher.Text = "ESCOLHER IMAGEM";
            this.btnEscolher.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEscolher.UseVisualStyleBackColor = false;
            this.btnEscolher.Click += new System.EventHandler(this.btnEscolher_Click);
            // 
            // fotoPrd
            // 
            this.fotoPrd.Location = new System.Drawing.Point(124, 12);
            this.fotoPrd.Name = "fotoPrd";
            this.fotoPrd.Size = new System.Drawing.Size(250, 250);
            this.fotoPrd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fotoPrd.TabIndex = 6;
            this.fotoPrd.TabStop = false;
            this.fotoPrd.Click += new System.EventHandler(this.fotoPrd_Click);
            // 
            // btnApagar
            // 
            this.btnApagar.Image = global::SICEpdv.Properties.Resources.delete;
            this.btnApagar.Location = new System.Drawing.Point(381, 233);
            this.btnApagar.Name = "btnApagar";
            this.btnApagar.Size = new System.Drawing.Size(30, 25);
            this.btnApagar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnApagar.TabIndex = 7;
            this.btnApagar.TabStop = false;
            this.btnApagar.Click += new System.EventHandler(this.btnApagar_Click);
            // 
            // UplodImagemProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(507, 430);
            this.Controls.Add(this.btnApagar);
            this.Controls.Add(this.fotoPrd);
            this.Controls.Add(this.pnlEnviar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "UplodImagemProduto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Imagem Produto";
            this.Load += new System.EventHandler(this.UplodImagemProduto_Load);
            this.pnlEnviar.ResumeLayout(false);
            this.pnlEnviar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fotoPrd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnApagar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlEnviar;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblProduto;
        private System.Windows.Forms.Button btnEscolher;
        private System.Windows.Forms.PictureBox fotoPrd;
        private System.Windows.Forms.PictureBox btnApagar;
    }
}
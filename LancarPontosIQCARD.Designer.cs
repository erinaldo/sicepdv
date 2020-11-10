namespace SICEpdv
{
    partial class LancarPontosIQCARD
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnLancar = new System.Windows.Forms.Button();
            this.panel10 = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.txtIQCARDFidelidade = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SICEpdv.Properties.Resources.splashIQCard;
            this.pictureBox1.Location = new System.Drawing.Point(1, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(162, 265);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnLancar
            // 
            this.btnLancar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btnLancar.ForeColor = System.Drawing.Color.White;
            this.btnLancar.Location = new System.Drawing.Point(233, 152);
            this.btnLancar.Name = "btnLancar";
            this.btnLancar.Size = new System.Drawing.Size(189, 43);
            this.btnLancar.TabIndex = 2;
            this.btnLancar.Text = "LANÇAR PONTOS";
            this.btnLancar.UseVisualStyleBackColor = false;
            this.btnLancar.Click += new System.EventHandler(this.btnLancar_Click);
            // 
            // panel10
            // 
            this.panel10.BackgroundImage = global::SICEpdv.Properties.Resources.input_search_products;
            this.panel10.Controls.Add(this.pictureBox4);
            this.panel10.Controls.Add(this.txtIQCARDFidelidade);
            this.panel10.Controls.Add(this.label21);
            this.panel10.Location = new System.Drawing.Point(164, 75);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(339, 40);
            this.panel10.TabIndex = 13;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.White;
            this.pictureBox4.BackgroundImage = global::SICEpdv.Properties.Resources.menu_top_user;
            this.pictureBox4.Location = new System.Drawing.Point(4, -5);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(50, 58);
            this.pictureBox4.TabIndex = 14;
            this.pictureBox4.TabStop = false;
            // 
            // txtIQCARDFidelidade
            // 
            this.txtIQCARDFidelidade.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIQCARDFidelidade.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIQCARDFidelidade.Location = new System.Drawing.Point(110, 10);
            this.txtIQCARDFidelidade.MaxLength = 36;
            this.txtIQCARDFidelidade.Name = "txtIQCARDFidelidade";
            this.txtIQCARDFidelidade.Size = new System.Drawing.Size(179, 22);
            this.txtIQCARDFidelidade.TabIndex = 13;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.White;
            this.label21.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.label21.ForeColor = System.Drawing.Color.Gray;
            this.label21.Location = new System.Drawing.Point(53, 12);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(58, 16);
            this.label21.TabIndex = 11;
            this.label21.Text = "IQCARD";
            // 
            // LancarPontosIQCARD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(515, 274);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.btnLancar);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LancarPontosIQCARD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pontos IQCARD";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnLancar;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.TextBox txtIQCARDFidelidade;
        private System.Windows.Forms.Label label21;
    }
}
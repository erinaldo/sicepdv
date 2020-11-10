namespace SICEpdv
{
    partial class frmExportacaoFiscal
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
            this.btnSintegra = new System.Windows.Forms.Button();
            this.btnSped = new System.Windows.Forms.Button();
            this.btnSEF2 = new System.Windows.Forms.Button();
            this.btnSEF = new System.Windows.Forms.Button();
            this.btnSpedPISCOFINS = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSintegra
            // 
            this.btnSintegra.Location = new System.Drawing.Point(12, 12);
            this.btnSintegra.Name = "btnSintegra";
            this.btnSintegra.Size = new System.Drawing.Size(155, 40);
            this.btnSintegra.TabIndex = 0;
            this.btnSintegra.Text = "Sintegra - ICMS 57/94";
            this.btnSintegra.UseVisualStyleBackColor = true;
            this.btnSintegra.Click += new System.EventHandler(this.btnSintegra_Click);
            // 
            // btnSped
            // 
            this.btnSped.Location = new System.Drawing.Point(12, 58);
            this.btnSped.Name = "btnSped";
            this.btnSped.Size = new System.Drawing.Size(155, 36);
            this.btnSped.TabIndex = 1;
            this.btnSped.Text = "SPED Fiscal";
            this.btnSped.UseVisualStyleBackColor = true;
            this.btnSped.Click += new System.EventHandler(this.btnSped_Click);
            // 
            // btnSEF2
            // 
            this.btnSEF2.Location = new System.Drawing.Point(12, 197);
            this.btnSEF2.Name = "btnSEF2";
            this.btnSEF2.Size = new System.Drawing.Size(155, 38);
            this.btnSEF2.TabIndex = 2;
            this.btnSEF2.Text = "SEF2";
            this.btnSEF2.UseVisualStyleBackColor = true;
            this.btnSEF2.Visible = false;
            this.btnSEF2.Click += new System.EventHandler(this.btnSEF2_Click);
            // 
            // btnSEF
            // 
            this.btnSEF.Location = new System.Drawing.Point(12, 153);
            this.btnSEF.Name = "btnSEF";
            this.btnSEF.Size = new System.Drawing.Size(154, 38);
            this.btnSEF.TabIndex = 3;
            this.btnSEF.Text = "SEF";
            this.btnSEF.UseVisualStyleBackColor = true;
            this.btnSEF.Click += new System.EventHandler(this.btnSEF_Click);
            // 
            // btnSpedPISCOFINS
            // 
            this.btnSpedPISCOFINS.Location = new System.Drawing.Point(11, 103);
            this.btnSpedPISCOFINS.Name = "btnSpedPISCOFINS";
            this.btnSpedPISCOFINS.Size = new System.Drawing.Size(155, 36);
            this.btnSpedPISCOFINS.TabIndex = 4;
            this.btnSpedPISCOFINS.Text = "SPED Pis Cofins";
            this.btnSpedPISCOFINS.UseVisualStyleBackColor = true;
            this.btnSpedPISCOFINS.Click += new System.EventHandler(this.btnSpedPISCOFINS_Click);
            // 
            // frmExportacaoFiscal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(176, 242);
            this.Controls.Add(this.btnSpedPISCOFINS);
            this.Controls.Add(this.btnSEF);
            this.Controls.Add(this.btnSEF2);
            this.Controls.Add(this.btnSped);
            this.Controls.Add(this.btnSintegra);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmExportacaoFiscal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Escolha a opção de layout";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSintegra;
        private System.Windows.Forms.Button btnSped;
        private System.Windows.Forms.Button btnSEF2;
        private System.Windows.Forms.Button btnSEF;
        private System.Windows.Forms.Button btnSpedPISCOFINS;
    }
}
namespace SICEpdv
{
    partial class FrmTipoTerminal
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
            this.btnPDV = new System.Windows.Forms.Button();
            this.btnPreVenda = new System.Windows.Forms.Button();
            this.btnDAV = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPDV
            // 
            this.btnPDV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btnPDV.FlatAppearance.BorderSize = 0;
            this.btnPDV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPDV.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPDV.ForeColor = System.Drawing.Color.White;
            this.btnPDV.Location = new System.Drawing.Point(12, 11);
            this.btnPDV.Name = "btnPDV";
            this.btnPDV.Size = new System.Drawing.Size(149, 55);
            this.btnPDV.TabIndex = 0;
            this.btnPDV.Text = "PDV";
            this.btnPDV.UseVisualStyleBackColor = false;
            this.btnPDV.Click += new System.EventHandler(this.btnPDV_Click);
            // 
            // btnPreVenda
            // 
            this.btnPreVenda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btnPreVenda.FlatAppearance.BorderSize = 0;
            this.btnPreVenda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreVenda.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreVenda.ForeColor = System.Drawing.Color.White;
            this.btnPreVenda.Location = new System.Drawing.Point(12, 72);
            this.btnPreVenda.Name = "btnPreVenda";
            this.btnPreVenda.Size = new System.Drawing.Size(149, 55);
            this.btnPreVenda.TabIndex = 1;
            this.btnPreVenda.Text = "Pré-Venda";
            this.btnPreVenda.UseVisualStyleBackColor = false;
            this.btnPreVenda.Click += new System.EventHandler(this.btnPreVenda_Click);
            // 
            // btnDAV
            // 
            this.btnDAV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btnDAV.FlatAppearance.BorderSize = 0;
            this.btnDAV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDAV.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDAV.ForeColor = System.Drawing.Color.White;
            this.btnDAV.Location = new System.Drawing.Point(12, 133);
            this.btnDAV.Name = "btnDAV";
            this.btnDAV.Size = new System.Drawing.Size(149, 55);
            this.btnDAV.TabIndex = 2;
            this.btnDAV.Text = "DAV";
            this.btnDAV.UseVisualStyleBackColor = false;
            this.btnDAV.Click += new System.EventHandler(this.btnDAV_Click);
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnSair.FlatAppearance.BorderSize = 0;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.ForeColor = System.Drawing.Color.White;
            this.btnSair.Location = new System.Drawing.Point(86, 196);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(72, 43);
            this.btnSair.TabIndex = 3;
            this.btnSair.Text = "SAIR";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // FrmTipoTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(170, 248);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnDAV);
            this.Controls.Add(this.btnPreVenda);
            this.Controls.Add(this.btnPDV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmTipoTerminal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TERMINAL";
            this.Load += new System.EventHandler(this.FrmTipoTerminal_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmTipoTerminal_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPDV;
        private System.Windows.Forms.Button btnPreVenda;
        private System.Windows.Forms.Button btnDAV;
        private System.Windows.Forms.Button btnSair;
    }
}
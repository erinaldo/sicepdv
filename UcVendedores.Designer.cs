namespace SICEpdv
{
    partial class UcVendedores
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlVendedores = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlVendedores
            // 
            this.pnlVendedores.AutoScroll = true;
            this.pnlVendedores.Location = new System.Drawing.Point(10, 77);
            this.pnlVendedores.Name = "pnlVendedores";
            this.pnlVendedores.Size = new System.Drawing.Size(656, 310);
            this.pnlVendedores.TabIndex = 0;
            // 
            // UcVendedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlVendedores);
            this.Name = "UcVendedores";
            this.Size = new System.Drawing.Size(683, 407);
            this.Load += new System.EventHandler(this.UcVendedores_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlVendedores;
    }
}

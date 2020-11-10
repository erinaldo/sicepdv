namespace SICEpdv
{
    partial class FrmCupomDAV
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
            this.crvCupom = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crvCupom
            // 
            this.crvCupom.ActiveViewIndex = -1;
            this.crvCupom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crvCupom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crvCupom.Location = new System.Drawing.Point(0, 0);
            this.crvCupom.Name = "crvCupom";
            this.crvCupom.SelectionFormula = "";
            this.crvCupom.Size = new System.Drawing.Size(778, 447);
            this.crvCupom.TabIndex = 0;
            this.crvCupom.ViewTimeSelectionFormula = "";
            // 
            // FrmCupomDAV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 447);
            this.Controls.Add(this.crvCupom);
            this.Name = "FrmCupomDAV";
            this.Text = "FrmCupomDAV";
            this.Load += new System.EventHandler(this.FrmCupomDAV_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crvCupom;
    }
}
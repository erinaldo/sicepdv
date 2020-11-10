namespace SICEpdv
{
    partial class ImpressaoDAV
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.vendadavBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.caixadavBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rptDAV = new Microsoft.Reporting.WinForms.ReportViewer();
            this.btnSair = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.vendadavBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.caixadavBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // vendadavBindingSource
            // 
            this.vendadavBindingSource.DataSource = typeof(SICEpdv.vendadav);
            // 
            // caixadavBindingSource
            // 
            this.caixadavBindingSource.DataSource = typeof(SICEpdv.caixadav);
            // 
            // rptDAV
            // 
            this.rptDAV.Dock = System.Windows.Forms.DockStyle.Top;
            reportDataSource1.Name = "vendaDAV";
            reportDataSource1.Value = this.vendadavBindingSource;
            reportDataSource2.Name = "caixaDAV";
            reportDataSource2.Value = this.caixadavBindingSource;
            this.rptDAV.LocalReport.DataSources.Add(reportDataSource1);
            this.rptDAV.LocalReport.DataSources.Add(reportDataSource2);
            this.rptDAV.LocalReport.ReportEmbeddedResource = "SICEpdv.DAV.rdlc";
            this.rptDAV.Location = new System.Drawing.Point(0, 0);
            this.rptDAV.Name = "rptDAV";
            this.rptDAV.Size = new System.Drawing.Size(764, 478);
            this.rptDAV.TabIndex = 0;
            this.rptDAV.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.FullPage;
            this.rptDAV.RenderingComplete += new Microsoft.Reporting.WinForms.RenderingCompleteEventHandler(this.rptDAV_RenderingComplete);
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.ForeColor = System.Drawing.Color.Black;
            this.btnSair.Location = new System.Drawing.Point(645, 482);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(113, 32);
            this.btnSair.TabIndex = 1;
            this.btnSair.Text = "&Fechar";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // ImpressaoDAV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 517);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.rptDAV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ImpressaoDAV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImpressaoDAV";
            this.Load += new System.EventHandler(this.ImpressaoDAV_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vendadavBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.caixadavBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rptDAV;
        private System.Windows.Forms.BindingSource vendadavBindingSource;
        private System.Windows.Forms.BindingSource caixadavBindingSource;
        private System.Windows.Forms.Button btnSair;
    }
}
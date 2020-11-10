using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SICEpdv
{
	/// <summary>
	/// Summary description for frmImpressao.
	/// </summary>
	public class frmImpressaoTEF : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Label lblMensagem;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmImpressaoTEF()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void  Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lblMensagem = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMensagem
            // 
            this.lblMensagem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(232)))), ((int)(((byte)(185)))));
            this.lblMensagem.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensagem.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMensagem.Location = new System.Drawing.Point(3, 30);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(513, 112);
            this.lblMensagem.TabIndex = 3;
            this.lblMensagem.Text = "lblMensagem";
            this.lblMensagem.TextChanged += new System.EventHandler(this.lblMensagem_TextChanged);
            // 
            // frmImpressaoTEF
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(232)))), ((int)(((byte)(185)))));
            this.ClientSize = new System.Drawing.Size(341, 162);
            this.Controls.Add(this.lblMensagem);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmImpressaoTEF";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmImpressao";
            this.ResumeLayout(false);

		}
		#endregion

		private void lblMensagem_TextChanged(object sender, System.EventArgs e)
		{
			this.Refresh();
			Application.DoEvents();
		}

	}
}

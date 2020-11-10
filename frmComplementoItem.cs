using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class frmComplementoItem : FormIQ
    {

        public frmComplementoItem()
        {
            InitializeComponent();
            // txtComplemento.MaxLength = 5;
        }

        public frmComplementoItem(String descricaoProd) : this()
        {
            txtComplemento.MaxLength = (47 - descricaoProd.Length);
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            txtLote.Text = "";
            txtComplemento.Text = "";
            this.Close();
        }

        private void frmComplementoItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }
        }
    }
}

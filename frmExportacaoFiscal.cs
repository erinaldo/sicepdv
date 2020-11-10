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
    public partial class frmExportacaoFiscal : Form
    {
        public frmExportacaoFiscal()
        {
            InitializeComponent();
        }

        private void btnSintegra_Click(object sender, EventArgs e)
        {
            frmSintegra sintegra = new frmSintegra();
            sintegra.ShowDialog();
        }

        private void btnSped_Click(object sender, EventArgs e)
        {
            FrmSPED frmSped = new FrmSPED();
            frmSped.ShowDialog();
        }

        private void btnSEF2_Click(object sender, EventArgs e)
        {

            FrmSEF2 frmsef2 = new FrmSEF2();
            frmsef2.ShowDialog();
        }

        private void btnSEF_Click(object sender, EventArgs e)
        {
            FrmSEF frmsef = new FrmSEF();
            frmsef.ShowDialog();
        }

        private void btnSpedPISCOFINS_Click(object sender, EventArgs e)
        {
            FrmSPEDPisCofins frmSped = new FrmSPEDPisCofins();
            frmSped.ShowDialog();
        }
       
    }
}

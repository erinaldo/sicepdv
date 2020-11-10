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
    public partial class FrmImportacaoNFe : Form
    {
        public List<itensNFe> itens = new List<itensNFe>();
        public FrmImportacaoNFe()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {

        }
    }
}

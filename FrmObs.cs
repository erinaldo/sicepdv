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
    public partial class FrmObs : Form
    {
        public static bool cancelado = false;
        public static string observacao = "";
        public static bool bntcancelar = true;

        public FrmObs()
        {
            InitializeComponent();
            if (Configuracoes.cancelarvendarejeicaocartao == true && bntcancelar == false)
                btnCancelar.Enabled = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            cancelado = true;
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            cancelado = false;
            this.Close();
            observacao = txtObservacao.Text;
        }
    }
}

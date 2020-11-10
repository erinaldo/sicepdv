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
    public partial class frmDependente : FormIQ
    {
        int codigoCliente = 0;
        public static string nomeDependente = "";
        public frmDependente(int idCliente)
        {
            codigoCliente = idCliente;
            InitializeComponent();
        }

        private void btSair_Click(object sender, EventArgs e)
        {
            nomeDependente = "";
            this.Close();
        }

        private void frmDependente_Shown(object sender, EventArgs e)
        {
            dtgDependentes.DataSource = from n in Conexao.CriarEntidade().dependentes
                                        where n.codigocliente == codigoCliente
                                        select new { nome = n.nome, cpf = n.cpf, rg = n.identidade };
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            nomeDependente = "";
            if (dtgDependentes.RowCount > 0)
                nomeDependente = Convert.ToString(dtgDependentes.CurrentRow.Cells["nome"].Value);
            Close();
        }

        private void frmDependente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }
        }

        private void frmDependente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }
    }
}

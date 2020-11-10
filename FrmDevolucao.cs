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
    public partial class FrmDevolucao : FormIQ
    {
        public static int numeroDevolucao = 0;
        public static decimal totalDevolucao = 0;
        public FrmDevolucao()
        {
            InitializeComponent();
            txtNumero.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto,evento);
            txtNumero.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyData == Keys.Return)
                    Procurar();
                };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            numeroDevolucao = 0;
            totalDevolucao = 0;
            this.Close();
        }

        private void Procurar()
        {
            btnConfirmar.Enabled = false;
            numeroDevolucao = 0;
            totalDevolucao = 0;
            int numeroDev = Convert.ToInt32(txtNumero.Text);
            siceEntities entidade = Conexao.CriarEntidade();

            var dados = (from n in entidade.contdevolucao
                        where n.numero == numeroDev
                        select n).AsQueryable();

            if (dados.Count() == 0)
            {
                MessageBox.Show("Devolução não foi encontrada", "SICEpdv.net", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNumero.Focus();
                txtNumero.SelectAll();
                return;
            }
            if (dados.First().finalizada == "S")
            {
                MessageBox.Show("Devolução finalizada !", "SICEpdv.net", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNumero.Focus();
                txtNumero.SelectAll();
                return;
            }

            var totalDV = (from n in entidade.devolucao
                                  where n.numero == numeroDev
                                  select n.total).Sum();

            

            dtgItens.DataSource = (from n in entidade.devolucao
                                   where n.numero == numeroDev
                                   select new { n.codigo, n.produto, n.quantidade, n.total }).AsQueryable();

            numeroDevolucao = numeroDev;
            totalDevolucao = Math.Round( totalDV == null ? 0 : totalDV,2);            
             

            lblDevolucao.Text = string.Format("{0:N2}", totalDevolucao);
            btnConfirmar.Enabled = true;

        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (chkDescontoGerencial.Checked == true)
                _pdv.descontoGerencial = true;
            else
                _pdv.descontoGerencial = false;

            this.Close();
        }

        private void FrmDevolucao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }
        }

        private void FrmDevolucao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }
    }
}

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
    public partial class FrmConsultarDAVPrevenda : Form
    {
        siceEntities entidade = Conexao.CriarEntidade();
        string tipo = "DAV";

        public FrmConsultarDAVPrevenda(string tipoConsulta)
        {
            InitializeComponent();
            tipo = tipoConsulta;
            txtProcurar.KeyUp += (objeto, evento) =>
                {
                    Procurar(txtProcurar.Text.ToUpper());
                    if (evento.KeyCode == Keys.Return)
                        Close();
                };
            
        }

        private void FrmConsultarDAVPrevenda_Load(object sender, EventArgs e)
        {
            Procurar("");
            txtProcurar.Focus();
        }

        private void Procurar(string procura)
        {
            if (tipo == "DAV")
            {
                var dados = (from n in entidade.contdav
                             where n.finalizada == "N"
                             && n.cancelada != "S"
                             && n.cliente.Contains(procura)
                             orderby n.numero descending
                             select new { n.numeroDAVFilial, n.data, n.valor, n.desconto, n.cliente, n.vendedor, n.operador, n.observacao }).Take(300);                

                dtgDavs.DataSource = dados.AsQueryable();
            }

            if (tipo == "PRE")
            {
                var dados = (from n in entidade.contprevendaspaf
                             where n.finalizada == "N"
                             && n.cancelada != "S"
                             && n.cliente.Contains(procura)
                             orderby n.numero descending
                             select new { n.numeroDAVFilial, n.data, n.valor, n.desconto, n.cliente, n.vendedor, n.operador, n.observacao }).Take(300);
                dtgDavs.DataSource = dados.AsQueryable();
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmConsultarDAVPrevenda_Shown(object sender, EventArgs e)
        {
            txtProcurar.Focus();
        }

        private void bntBuscarDavs_Click(object sender, EventArgs e)
        {
            if (tipo == "DAV")
            {
                var dados = (from n in entidade.contdav
                             where n.finalizada == "N"
                             && n.cancelada != "S"
                             && n.data >= dtInicio.Value.Date && n.data <= dtFinal.Value.Date && n.codigofilial == GlbVariaveis.glb_filial
                             orderby n.numero descending
                             select new { n.numeroDAVFilial, n.data, n.valor, n.desconto, n.cliente, n.vendedor, n.operador, n.observacao }).Take(300);

                dtgDavs.DataSource = dados.AsQueryable();
            }

            if (tipo == "PRE")
            {
                var dados = (from n in entidade.contprevendaspaf
                             where n.finalizada == "N"
                             && n.cancelada == "N"
                             && n.data >= dtInicio.Value.Date && n.data <= dtFinal.Value.Date && n.codigofilial == GlbVariaveis.glb_filial
                             orderby n.numero descending
                             select new { n.numeroDAVFilial, n.data, n.valor, n.desconto, n.cliente, n.vendedor, n.operador, n.observacao }).Take(300);
                dtgDavs.DataSource = dados.AsQueryable();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tipo == "DAV")
            {
                try
                {


                    var sql = "SELECT idTransacaogateway FROM contdav WHERE numeroDAVFilial = '" + dtgDavs.CurrentRow.Cells[0].Value.ToString() + "' AND codigoFilial = '" + GlbVariaveis.glb_filial + "'";

                    string DAV = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

                    if (DAV != null && DAV != "")
                    {
                        MessageBox.Show("Não é possivel Excluir um DAV do IQSHOP com gateway Finalizado ?", "Atenção", MessageBoxButtons.OKCancel);
                            return;
                    }

                    if (DialogResult.OK == MessageBox.Show("Deseja realmente excluir o DAV?", "Atenção", MessageBoxButtons.OKCancel))
                    {
                        Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE contdav SET cancelada = 'S' WHERE numeroDAVFilial = '" + dtgDavs.CurrentRow.Cells[0].Value.ToString() + "' AND codigoFilial = '" + GlbVariaveis.glb_filial + "'");
                        Procurar("");
                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }
            }
        }
    }
}

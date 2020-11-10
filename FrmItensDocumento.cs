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
    public partial class FrmItensDocumento : Form
    {
        Int32 documento = 0;
        public FrmItensDocumento(Int32 doc)
        {
            documento = doc;
            
            InitializeComponent();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmItensDocumento_Load(object sender, EventArgs e)
        {
            carregarProdutos();

        }

        private void carregarProdutos()
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Opa! Carregando... ");
            msg.Show();
            Application.DoEvents();

            lblDocumento.Text = documento.ToString();

            var itens = Conexao.CriarEntidade().ExecuteStoreQuery<itensDocumento>("SELECT n.codigo, n.produto, " +
                "n.tributacao, n.cfop," +
                "n.cstpis, n.pis, n.cstcofins, n.cofins," +
                "n.icms, n.cest, n.codigobarras FROM venda AS n " +
                "WHERE documento = '" + documento + "' AND codigoFilial = '" + GlbVariaveis.glb_filial + "'").ToList();

            if (itens.Count() == 0)
            {
                /*itens = from n in Conexao.CriarEntidade().vendaarquivo
                        where n.documento == documento
                        && n.codigofilial == GlbVariaveis.glb_filial
                        && n.cancelado == "N"
                        select new { n.codigo, n.produto, n.quantidade, n.preco,
                                     n.total, n.vendedor, n.tributacao, n.cfop,
                                     n.cstpis, n.pis, n.cstcofins, n.cofins,
                                     n.icms, n.ncm, n.cest, n.codigobarras };*/

                itens = Conexao.CriarEntidade().ExecuteStoreQuery<itensDocumento>("SELECT n.codigo, n.produto, " +
                "n.tributacao, n.cfop," +
                "n.cstpis, n.pis, n.cstcofins, n.cofins," +
                "n.icms, n.cest, n.codigobarras FROM vendaarquivo AS n " +
                "WHERE documento = '" + documento + "' AND codigoFilial = '" + GlbVariaveis.glb_filial + "'").ToList();
            }

            if (itens.Count() == 0)
            {
                MessageBox.Show("Não foi encontrado itens no documento !");
                return;
            }

            dtgItens.DataSource = itens.ToList();

            msg.Dispose();
        }

        private void bntAjustar_Click(object sender, EventArgs e)
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Opa! Ajustando Itens...");
            msg.Show();
            Application.DoEvents();

            string tabela = GlbVariaveis.glb_filial == "00001" ? "produtos" : "produtosfilial";

            string SQL = "UPDATE vendaarquivo AS c, "+ tabela + " AS p SET " +
                        "c.ncm = p.ncm, " +
                        "c.cfop = IF(p.cfopsaida = '', '5.102', p.cfopsaida), " +
                        "c.ncmespecie = p.ncmespecie," +
                        "c.cstcofins = p.tributacaoCOFINS," +
                        "c.cstpis = p.tributacaoPIS," +
                        "c.pis = p.pis," +
                        "c.cofins = p.cofins," +
                        "c.tributacao = p.tributacao," +
                        "c.origem = p.origem," +
                        "c.icms = p.icms," +
                        "c.codigobarras = p.codigobarras" +
                        "WHERE c.codigo = p.codigo" +
                        "AND p.codigofilial = c.codigofilial" +
                        "AND c.documento = '"+documento+"'";

            msg.Dispose();

            carregarProdutos();
        }
    }
}

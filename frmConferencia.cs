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
    public partial class frmConferencia : Form
    {
        List<itensConferencia> listItensConferencia = new List<itensConferencia>();
        public frmConferencia()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buscarVenda();
        }

        private void buscarVenda()
        {
            siceEntities conexao = Conexao.CriarEntidade();

            int documento = 0;

            bool inteiro = int.TryParse(txtCOODocumento.Text, out documento);


            var produtos = (from d in conexao.contdocs
                            where (d.documento == documento || d.ecfcontadorcupomfiscal == txtCOODocumento.Text) && d.CodigoFilial == GlbVariaveis.glb_filial
                            select new { documento = d.documento, data = d.data}).FirstOrDefault();

           if (produtos.data.Value == DateTime.Now.Date)
           {
               var venda = (from d in conexao.venda
                            where d.documento == produtos.documento && d.codigofilial == GlbVariaveis.glb_filial
                            select new { codigo = d.codigo, descricao = d.produto, quantidade = d.quantidade }).ToList();

               dgItensVenda.DataSource = venda.ToList();

           }
           else
           {
                var venda = (from d in conexao.vendaarquivo
                            where d.documento == produtos.documento && d.codigofilial == GlbVariaveis.glb_filial
                             select new { codigo = d.codigo, descricao = d.produto, quantidade = d.quantidade }).ToList();

                dgItensVenda.DataSource = venda.ToList();
           }

        }

        private void txtCOODocumento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buscarVenda();
                txtCodigo.Focus();
            }
        }

        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Produtos objProdutos = new Produtos();
                objProdutos.ProcurarCodigo(txtCodigo.Text, GlbVariaveis.glb_filial);

                if (objProdutos.codigo != null && objProdutos.codigo != "")
                {
                    pnQuantidade.Visible = true;
                    txtQuantidade.Focus();
                }
            }
        }

        private void txtQuantidade_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Produtos objProdutos = new Produtos();
                objProdutos.ProcurarCodigo(txtCodigo.Text, GlbVariaveis.glb_filial);

                itensConferencia item = new itensConferencia();
                item.codigo = objProdutos.codigo;
                item.codigoFilial = GlbVariaveis.glb_filial;
                item.descricao = objProdutos.descricao;
                item.ipTerminal = GlbVariaveis.glb_IP;
                item.operador = GlbVariaveis.glb_Usuario;
                item.quantidade = decimal.Parse(txtQuantidade.Text);
                listItensConferencia.Add(item);

                /*var itensVenda = (from i in dgItensVenda.sou
                                      where )*/

                var itens = (from i in listItensConferencia
                             select new
                             {
                                 codigo = i.codigo,
                                 descricao = i.descricao,
                                 quantidade = i.quantidade
                             }).ToList();

                dgConferencia.DataSource = itens.ToList();
            }

            if (e.KeyCode == Keys.Escape)
            {
                pnQuantidade.Visible = false;
                txtCodigo.Focus();
            }
        }
    }
}
class itensConferencia
{
    public string codigo { get; set; }
    public string descricao { get; set; }
    public decimal quantidade { get; set; }
    public string operador { get; set; }
    public string codigoFilial { get; set; }
    public string ipTerminal { get; set; }
    public bool conferido { get; set; }
}
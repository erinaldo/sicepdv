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
    public partial class FrmAltQtdDAV : Form
    {
        string codigo;
        int nrcontrole;
        public FrmAltQtdDAV(string codigo, string descricao, int nrcontrole)
        {
            InitializeComponent();
            this.codigo = codigo;
            lblDescricao.Text = descricao;
            this.nrcontrole = nrcontrole;
            txtQuantidade.Focus();
        }


        private void btnAlterarQuantidade_Click(object sender, EventArgs e)
        {
            decimal descontoConcedido = 0;
            try
            {
                siceEntities conexao = Conexao.CriarEntidade();

                var item = (from i in conexao.vendas
                            where i.id == GlbVariaveis.glb_IP && i.codigofilial == GlbVariaveis.glb_filial && i.codigo == codigo && i.nrcontrole == nrcontrole
                            select i).FirstOrDefault();

                descontoConcedido = item.descontovalor;


                if (item.lote != null && item.lote.Length > 0) // if(item.lote != "" || item.lote != null || item.lote != "0")
                {
                    MessageBox.Show("Não é possivel alterar quantidade de produtos com Lote!", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }




                if (decimal.Parse(txtQuantidade.Text) >= item.quantidade)
                    item.descontovalor = ((item.quantidade * item.precooriginal) - item.total) / item.quantidade;
                else
                    item.descontovalor = (item.descontovalor / item.quantidade);

                item.quantidade = decimal.Parse(txtQuantidade.Text);
                item.descontovalor = item.quantidade * item.descontovalor;
                item.embalagem = item.embalagem;
                item.total = (item.quantidade * item.preco.Value);

                Produtos produto = new Produtos();

                var dadosPrd = produto.ProcurarCodigo(codigo, GlbVariaveis.glb_filial);

                if (decimal.Parse(txtQuantidade.Text) < produto.quantidadeMinDesconto && descontoConcedido > 0)
                {
                    throw new Exception("Quantidade não permitida para o desconto concedido!");
                }

                decimal quantidadeDiponivel = produto.quantidadeDisponivel;

                if (Configuracoes.usarQtdPrateleira)
                    quantidadeDiponivel = produto.quantidadePrat;



                if (!Permissoes.venderQtdNegativa && quantidadeDiponivel < item.quantidade)
                {
                    FrmLogon logon = new FrmLogon();
                    logon.campo = "estnegativo";
                    logon.txtDescricao.Text = "Produto negativo: " + codigo + " " + produto.descricao + " Qtd: " + produto.quantidadeDisponivel.ToString();
                    logon.ShowDialog();
                    if (!Operador.autorizado)
                        throw new Exception("Sem permissão para vender com estoque negativo !");

                    siceEntities entidade = Conexao.CriarEntidade();

                    if (Conexao.tipoConexao == 2)
                        entidade = Conexao.CriarEntidade(false);

                    auditoria objAuditoria = new auditoria();
                    objAuditoria.acao = "Venda";
                    objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                    objAuditoria.codigoproduto = codigo;
                    objAuditoria.data = DateTime.Now.Date;
                    objAuditoria.documento = 0;
                    objAuditoria.hora = DateTime.Now.TimeOfDay;
                    objAuditoria.local = "SICE.pdv";
                    objAuditoria.observacao = "Produto negativo: " + codigo + " " + produto.descricao + " Qtd: " + produto.quantidadeDisponivel.ToString();
                    objAuditoria.usuario = Operador.ultimoOperadorAutorizado;
                    entidade.AddToauditoria(objAuditoria);
                    entidade.SaveChanges();

                    try
                    {
                        var lisCodigo = (from a in entidade.auditoria
                                         where a.acao == "Venda" && a.usuario == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == objAuditoria.data
                                         select a.id).ToList().Max();

                        auditoriaVenda n = new auditoriaVenda();
                        n.inc = int.Parse(lisCodigo.ToString());
                        Venda.listAuditoriaVenda.Add(n);
                    }
                    catch (Exception erro)
                    {

                    }
                }


                conexao.SaveChanges();
                Venda.ReservarEstoquePreVenda(item.codigo, item.quantidade);

                MessageBox.Show("Quantidade Alterada com sucesso!", "Atenção", MessageBoxButtons.OK);
                this.Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnAlterarQuantidade.Focus();
            }
        }
    }
}

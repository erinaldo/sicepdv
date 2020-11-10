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
    public partial class FrmPosicaoEstoqueFiliais : Form
    {
        public static string codigo = "";

        public static bool gerarTransfVenda = false;
        public static decimal quantidadeTransf = 0;
        public static string filialEscolhida = "";

        public FrmPosicaoEstoqueFiliais()
        {
            InitializeComponent();

            if (gerarTransfVenda)
            {
                btnTransf.Visible = true;
                btnTransf.Text = string.Format("{0:N2}", quantidadeTransf)+ " GERAR TRANSF. FILIAL";
            }
            else
            {
                btnTransf.Visible = false;
            }

            this.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyCode == Keys.Escape)
                    this.Close();
            };

            dtgProdutos.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyCode == Keys.Escape)
                    this.Close();
            };
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            filialEscolhida = "";
            gerarTransfVenda = false;
            quantidadeTransf = 0;
            this.Close();
        }

        private void Escolher()
        {
            filialEscolhida = "";
            if (dtgProdutos.RowCount > 0)
            {                
                filialEscolhida = Convert.ToString(dtgProdutos.CurrentRow.Cells["colFilial"].Value);
                decimal quantidade = Convert.ToDecimal(dtgProdutos.CurrentRow.Cells["colQtd"].Value);
                gerarTransfVenda = true;

                if (GlbVariaveis.glb_filial == filialEscolhida)
                {
                    MessageBox.Show("Você está pedindo transferência. Escolha uma filial que não seja a sua");
                    gerarTransfVenda = false;
                    filialEscolhida = "";
                    return;
                }
                else if((quantidade<quantidadeTransf) || quantidade == 0)
                {
                    MessageBox.Show("Quantidade solicitada insuficiente na filial escolhida.");
                    gerarTransfVenda = false;
                    filialEscolhida = "";
                    return;
                };
            }

            this.Close();
        }

        private void FrmPosicaoEstoqueFiliais_Load(object sender, EventArgs e)
        {

            if (Conexao.tipoConexao == 1 || Conexao.tipoConexao == 2)
            {
                #region
                List<estoqueFiliais> listEstoque = new List<estoqueFiliais>();

                var filiais = (from f in Conexao.CriarEntidade().filiais
                               where f.ativa == "S"
                               select f.CodigoFilial).ToList();

                foreach (var item in filiais)
                {

                    var estoqueMatriz = (from n in Conexao.CriarEntidade().produtos
                                         where n.codigo == codigo
                                         orderby n.CodigoFilial
                                         select new
                                         {
                                             filial = n.CodigoFilial,
                                             codigo = n.codigo,
                                             descricao = n.descricao,
                                             quantidade = n.quantidade,
                                             prateleiras = n.qtdprateleiras,
                                             deposito = n.quantidade - n.qtdprateleiras
                                         }).First();



                    lblDado.Text = "MATRIZ: 00001  " + estoqueMatriz.codigo + " " + estoqueMatriz.descricao;
                    lblInfo.Text = "Quantidade.: " + string.Format("{0:N2}", estoqueMatriz.quantidade) + "    Qtd.Prat.: " + string.Format("{0:N2}", estoqueMatriz.prateleiras) + "    Qtd.Depósito : " + string.Format("{0:N2}", estoqueMatriz.deposito);

                    if (item == "00001")
                    {
                        estoqueFiliais m = new estoqueFiliais();
                        m.codigo = estoqueMatriz.codigo;
                        m.deposito = estoqueMatriz.deposito;
                        m.descricao = estoqueMatriz.descricao;
                        m.filial = estoqueMatriz.filial;
                        m.prateleiras = estoqueMatriz.prateleiras;
                        m.quantidade = estoqueMatriz.quantidade;
                        m.deposito = estoqueMatriz.deposito;
                        listEstoque.Add(m);
                    }


                    var posicao = (from n in Conexao.CriarEntidade().produtosfilial
                                   where n.codigo == codigo && n.CodigoFilial == item
                                   orderby n.CodigoFilial
                                   select new
                                   {
                                       filial = n.CodigoFilial,
                                       codigo = n.codigo,
                                       descricao = n.descricao,
                                       quantidade = n.quantidade,
                                       prateleiras = n.qtdprateleiras,
                                       deposito = n.quantidade - n.qtdprateleiras
                                   }).FirstOrDefault();


                    if (posicao != null)
                    {
                        estoqueFiliais p = new estoqueFiliais();
                        p.codigo = posicao.codigo;
                        p.deposito = posicao.deposito;
                        p.descricao = posicao.descricao;
                        p.filial = posicao.filial;
                        p.prateleiras = posicao.prateleiras;
                        p.quantidade = posicao.quantidade;
                        p.deposito = posicao.deposito;

                        listEstoque.Add(p);
                    }

                }

                #endregion

                dtgProdutos.DataSource = listEstoque.ToList();
            }
            else
            {
                try
                {
                    #region
                    List<estoqueFiliais> listEstoque = new List<estoqueFiliais>();

                    if (Conexao.VerificaConexaoDB() == true)
                    {


                        var filiais = (from f in Conexao.CriarEntidade().filiais
                                       where f.ativa == "S"
                                       select f.CodigoFilial).ToList();

                        foreach (var item in filiais)
                        {

                            var estoqueMatriz = (from n in Conexao.CriarEntidade(false).produtos
                                                 where n.codigo == codigo
                                                 orderby n.CodigoFilial
                                                 select new
                                                 {
                                                     filial = n.CodigoFilial,
                                                     codigo = n.codigo,
                                                     descricao = n.descricao,
                                                     quantidade = n.quantidade,
                                                     prateleiras = n.qtdprateleiras,
                                                     deposito = n.quantidade - n.qtdprateleiras
                                                 }).First();



                            lblDado.Text = "MATRIZ: 00001  " + estoqueMatriz.codigo + " " + estoqueMatriz.descricao;
                            lblInfo.Text = "Quantidade.: " + string.Format("{0:N2}", estoqueMatriz.quantidade) + "    Qtd.Prat.: " + string.Format("{0:N2}", estoqueMatriz.prateleiras) + "    Qtd.Depósito : " + string.Format("{0:N2}", estoqueMatriz.deposito);

                            if (item == "00001")
                            {
                                estoqueFiliais m = new estoqueFiliais();
                                m.codigo = estoqueMatriz.codigo;
                                m.deposito = estoqueMatriz.deposito;
                                m.descricao = estoqueMatriz.descricao;
                                m.filial = estoqueMatriz.filial;
                                m.prateleiras = estoqueMatriz.prateleiras;
                                m.quantidade = estoqueMatriz.quantidade;
                                m.deposito = estoqueMatriz.deposito;
                                listEstoque.Add(m);
                            }


                            var posicao = (from n in Conexao.CriarEntidade(false).produtosfilial
                                           where n.codigo == codigo && n.CodigoFilial == item
                                           orderby n.CodigoFilial
                                           select new
                                           {
                                               filial = n.CodigoFilial,
                                               codigo = n.codigo,
                                               descricao = n.descricao,
                                               quantidade = n.quantidade,
                                               prateleiras = n.qtdprateleiras,
                                               deposito = n.quantidade - n.qtdprateleiras
                                           }).FirstOrDefault();


                            if (posicao != null)
                            {
                                estoqueFiliais p = new estoqueFiliais();
                                p.codigo = posicao.codigo;
                                p.deposito = posicao.deposito;
                                p.descricao = posicao.descricao;
                                p.filial = posicao.filial;
                                p.prateleiras = posicao.prateleiras;
                                p.quantidade = posicao.quantidade;
                                p.deposito = posicao.deposito;

                                listEstoque.Add(p);
                            }

                        }

                        dtgProdutos.DataSource = listEstoque.ToList();
                    }
                    
                    #endregion
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }
            }

            
        }

        private void btnTransf_Click(object sender, EventArgs e)
        {
            Escolher();
        }
    }
}

class estoqueFiliais
{
    public string filial { get; set; }
    public string codigo { get; set; }
    public string descricao { get; set; }
    public decimal quantidade { get; set; }
    public decimal prateleiras { get; set; }
    public decimal deposito { get; set; }
}

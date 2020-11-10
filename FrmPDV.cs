using System;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Data.EntityClient;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace SICEpdv
{
    public partial class _pdv : Form
    {
        [DllImport("kernel32.dll")]
        public static extern bool Beep(UInt32 frequency, UInt32 duration);

        TeclaNumerico teclado = new TeclaNumerico(Color.White);
        UCCartoes cartoes = new UCCartoes();
        ucClientePdv clientePDV = new ucClientePdv();
        Panel pnlCartoes = new Panel();
        Panel pnlCliente = new Panel();
        Panel pnlTroco = new Panel();
        UCTroco troco = new UCTroco();
        Produtos produto = new Produtos();
        Panel pnlCedulas = new Panel();
        UCMoedas cedulas = new UCMoedas();


        private decimal quantidade = 0, preco;
        private decimal totalTransacao, desconto = 0, dinheiro = 0, cartao = 0, cheque = 0, vendaAV = 0, crediario = 0, restante = 0, descontoJuros = 0;
        private string controle;
        private string tecla;
        private string tipoDesconto = "%";
        private decimal guardaDesconto = 0;
        private decimal descontoMaximoItem = Configuracoes.descontoMaxVenda;
        private decimal encargos = 0.00M;
        public static decimal taxaServicoIqChef = 0;
        private string dpFinanceiro = "Venda";
        const int QTDCARTOES = 3;
        private int qtdCartoesFeitas = 0;
        public static int devolucaoNumero = 0;
        public static bool descontoGerencial = false;
        public static decimal totalDevolucao = 0;
        public string classeVenda = "0000";
        public int parcelamentoMaximo = 0;
        public static int numeroDAV = 0;
        public static int numeroPreVenda = 0;
        public static bool identificarClienteNFCe = false;
        private bool digitarQuantidade = false;
        private decimal valorSelecionadoCedula = 0;
        private bool descontoMax = false;
        public bool perguntarCancelamento = true;
        private int nvezesAnuncio = 0;
        private bool aceitaDesconto = true;
        private decimal valorEntrada = 0;
        bool ativarIndoor = true;
        bool anuncioHabilitado = false;
        bool sugerirProdutos = false;
        public static string codP;
        public bool exportacaoRealizada;
        public bool certificadoVencido;



        Venda venda = new Venda();
        public static List<caixas> listFormaPagamentoAuditoria = new List<caixas>();
        public static List<produtoLote> listLotes = new List<produtoLote>();
        public static List<sugestaoProdutos> sugestao = new List<sugestaoProdutos>();

        public _pdv()
        {

            BringToFront();
            InitializeComponent();
            CriarPnlTroco();


            ItensPedidoIQCard.gerarVenda += ItensPedidoIQCard_gerarVenda;
            // Colocando um metodo assincronico para procucar os usuário que estao procurando promocao
            //contadorProcuraPromocao.DoWork += contadorProcuraPromocao_DoWork;
            //contadorProcuraPromocao.WorkerReportsProgress = false;
            //contadorProcuraPromocao.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(Terminado);

            try
            {
                if (Funcoes.VerificarConexaoInternet() && (DateTime.Now.Subtract(IqCard.horaVerificadoPromocao).TotalMinutes > 120))
                {
                    IqCard iqcard = new IqCard();
                    iqcard.ContadorProcuraPromocao();
                }
            }
            catch (Exception)
            {

            }
          

            if (Funcoes.VerificarConexaoInternet() && !String.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard) && IqCard.mostrarPainelIQCARD && Configuracoes.coefecientePontosIQCard > 0 && Configuracoes.coefecientePontosIQCard!=1)
            {
                IndexPontosAcumulados painel = new IndexPontosAcumulados();
                painel.ShowDialog();
            }

            btTotalizar.Click += (objeto, evento) => Totalizar();
            btnSuprimento.Click += (objeto, evento) => ChamarSuprimento();
            btnSangria.Click += (objeto, evento) => ChamarSangria();
            btnProdutos.Click += (objeto, evento) =>
            {
                ChamarProdutos(false);
                txtcodProduto.Focus();
            };


            btnPED.Click += (objeto, evento) => menuPED.Show(btnPED, new Point(btnPED.Width, 0));

            btnDevolucao.Click += (objeto, evento) => ChamarDV();


            /// Evento na Entra do Controle dastxtDesconto Formas de Pagamento
            #region txtCodProduto
            txtcodProduto.Enter += (objeto, evento) =>
            {
                if (clientePDV.idCliente != 0 || clientePDV.cpfcnpjCliente != "")
                {
                    lblInfoCliente.Text = clientePDV.idCliente.ToString() + " " + clientePDV.nomeCliente;
                }
                else
                {
                    lblInfoCliente.Text = "";
                    if (!string.IsNullOrEmpty(Venda.IQCard))
                    {
                        lblInfoCliente.Text = Venda.IQCard;
                    }
                }

                timeHora.Enabled = true;
                Funcoes.TravarTeclado(false);
                controle = "txtcodProduto";
            };
            txtcodProduto.KeyDown += (objeto, evento) =>
            {
                ativarIndoor = false;
                if (evento.KeyCode == Keys.Enter && quantidade == 0)
                    quantidade = 1;

                // Commanod GRAVAR-R para verificar se o movimento esta sendo gravado ou 
                // se esta apresentando erro
                if (evento.KeyCode == Keys.Return && txtcodProduto.Text.Contains("GRAVAR-R"))
                {

                    GravarRelatorioR_Anterior();
                    evento.SuppressKeyPress = true;
                    return;
                }

                if (evento.KeyCode == Keys.Return && txtcodProduto.Text == "")
                {
                    ChamarProdutos(false);
                    evento.SuppressKeyPress = true;
                    return;
                }

                EntraCodigo(evento);

                if (evento.KeyValue == 33)
                {
                    Totalizar();
                }

                if (evento.KeyCode == Keys.F3)
                    MostrarConsumidor();

                if (evento.KeyCode == Keys.F4)
                {
                    FuncoesECF.AbrirGaveta();

                    if (ConfiguracoesECF.NFC == true)
                    {
                        FuncoesECF.RelatorioGerencial("abrir", "");
                        FuncoesECF.RelatorioGerencial("imprimir", "Abertura de Gaveta!");
                        FuncoesECF.RelatorioGerencial("fechar", "");
                    }
                }

                if (evento.KeyCode == Keys.PageDown)
                {
                    DivulgarPromocaoIQCARD.iniciarmostrandoPedido = true;
                    DivulgarPromocaoIQCARD.iniciarmostrandoPromocao = false;
                    ChamarAnuncio();
                    //ChamarNFe();
                }

            };
            #region quantidade
            txtQtd.Leave += (objeto, evento) =>
                {
                    grpQtd.Visible = false;
                };

            txtQtd.KeyPress += (objeto, evento) =>
                {
                    if (evento.KeyChar == (char)Keys.Escape)
                    {
                        grpPreco.Visible = false;
                        grpQtd.Visible = false;
                        pnDigitacao.Visible = true;
                        txtcodProduto.Focus();
                    }
                };

            txtQtd.KeyPress += (objeto, evento) =>
            {
                if (evento.KeyChar == (char)Keys.Return)
                    grpQtd.Visible = false;

                Funcoes.DigitarNumerosPositivos(objeto, evento);
            };
            txtQtd.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {
                    txtQtd.Text = Funcoes.FormatarDecimal(txtQtd.Text, 3);
                    if (Convert.ToDecimal(txtQtd.Text) < 0)
                    {
                        MessageBox.Show("Quantidade sem valor.");
                        txtcodProduto.Focus();
                        txtQtd.Text = "0";
                        grpPreco.Visible = false;
                        pnDigitacao.Visible = true;
                        txtcodProduto.Focus();
                        evento.SuppressKeyPress = true;
                        return;
                    }

                    grpQtd.Visible = false;
                    txtQtd.Text = Funcoes.FormatarDecimal(txtQtd.Text, 3);
                    quantidade = Convert.ToDecimal(txtQtd.Text);
                    lblQtd.Text = string.Format("{0:N3}", quantidade) + " X";
                    if (Configuracoes.mudarPrecoVenda)
                    {
                        evento.SuppressKeyPress = true;
                        MudarPreco();
                    }
                    else
                        Lancar();
                }
            };

            #endregion

            #endregion
            #region Desconto
            txtDesconto.Enter += (objeto, evento) => controle = "txtDesconto";
            txtDesconto.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);

                if (evento.KeyChar == 27) btCancelarPg_Click(objeto, evento);
            };

            txtDesconto.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {


                    if (txtDesconto.Text.Length >= 12)
                    {
                        try
                        {
                            DescontoVoucher();
                            evento.SuppressKeyPress = true;
                            return;
                        }
                        catch (Exception)
                        {
                            evento.SuppressKeyPress = true;
                            txtDesconto.Enabled = true;
                            txtDesconto.Text = "0.00";
                            txtDesconto.Focus();
                            return;
                        }
                    }


                    txtCrediario.Enabled = false;
                    txtCheque.Enabled = false;
                    txtCartao.Enabled = false;
                    txtDesconto.Text = Funcoes.FormatarDecimal(txtDesconto.Text);
                    if (Convert.ToDecimal(txtDesconto.Text) < 0)
                    {
                        txtDesconto.Text = "0,00";
                    }
                    Desconto();
                    evento.SuppressKeyPress = true;

                    if (dpFinanceiro != "Recebimento")
                        txtCrediario.Enabled = true;

                    txtCheque.Enabled = true;
                    txtCartao.Enabled = true;
                }

                if (evento.KeyValue == 27)
                {
                    AtivarBotoes();
                    txtcodProduto.Visible = true;
                }
            };
            #endregion

            #region totalItem
            txtTotalItem.KeyPress += (objeto, evento) =>
                Funcoes.DigitarNumerosPositivos(objeto, evento);

            txtTotalItem.KeyDown += (objeto, evento) =>
            {


                if (evento.KeyValue == 13)
                {
                    txtTotalItem.Text = Funcoes.FormatarDecimal(txtTotalItem.Text);
                    if (Convert.ToDecimal(txtTotalItem.Text) < 0)
                    {
                        txtTotalItem.Text = "0,00";
                    }

                    txtTotalItem.Text = Funcoes.FormatarDecimal(txtTotalItem.Text);
                }
            };

            #endregion totalItem
            #region Dinheiro

            txtDinheiro.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
                if (evento.KeyChar == 27) btCancelarPg_Click(objeto, evento);

            };
            txtDinheiro.KeyDown += (objeto, evento) =>
            {


                if (evento.KeyValue == 13)
                {
                    txtDinheiro.Text = Funcoes.FormatarDecimal(txtDinheiro.Text);

                    if (Convert.ToDecimal(txtDinheiro.Text) < 0)
                    {
                        txtDinheiro.Text = "0,00";
                    }

                    if (!Conexao.onLine && Convert.ToDecimal(txtDinheiro.Text) < totalTransacao - desconto)
                    {
                        MessageBox.Show("Venda só pode ser em Dinheiro");
                        evento.SuppressKeyPress = true;
                        return;
                    }

                    if (Convert.ToDecimal(txtDinheiro.Text) < valorEntrada && Configuracoes.entradaDH == true && Configuracoes.entradaCA == false && Configuracoes.entradaCH == false)
                    {
                        MessageBox.Show("Valor minimo da entrada é" + valorEntrada.ToString("N2"));
                        return;
                    }

                    Dinheiro();
                    evento.SuppressKeyPress = true;
                }
            };
            txtDinheiro.Enter += (objeto, evento) =>
            {
                try
                {
                    if (ConfiguracoesECF.pdv)
                    {
                        MostrarCedulas();
                    }
                    Funcoes.TravarTeclado(true);
                    controle = "txtDinheiro";
                    VerificarPagamento();
                    txtDinheiro.Focus();
                    txtDinheiro.SelectAll();
                    txtDinheiro.Text = String.Format("{0:N2}", restante);
                    Application.DoEvents();
                }
                finally
                {
                    Funcoes.TravarTeclado(false);
                }
            };
            #endregion
            #region Cartao
            txtCartao.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
                if (evento.KeyChar == 27) btCancelarPg_Click(objeto, evento);
                if (evento.KeyChar != 48) evento.Handled = true;
            };
            txtCartao.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {
                    txtCartao.Text = Funcoes.FormatarDecimal(txtCartao.Text);

                    if (Convert.ToDecimal(txtCartao.Text) > 0 && verificaDescontoPagamento("CA") == false)
                    {
                        return;
                    }

                    MostraCartoes();

                }
            };


            txtCartao.Enter += (objeto, evento) =>
            {

                txtCartao.Text = String.Format("{0:N2}", restante);
                controle = "txtCartao";
                VerificarPagamento();
                txtCartao.SelectAll();
            };
            #endregion
            #region Crediario
            txtCrediario.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
                if (evento.KeyChar == 27) btCancelarPg_Click(objeto, evento);
            };
            txtCrediario.Enter += (objeto, evento) =>
            {
                txtCrediario.Text = string.Format("{0:N2}", restante);
                controle = "txtCrediario";
                VerificarPagamento();
                txtCrediario.SelectAll();
            };

            txtCrediario.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {
                    txtCrediario.Text = Funcoes.FormatarDecimal(txtCrediario.Text);
                    if (Convert.ToDecimal(txtCrediario.Text) < 0)
                    {
                        txtCrediario.Text = "0,00";
                    }

                    if (Convert.ToDecimal(txtCrediario.Text) > restante)
                    {
                        txtCrediario.Text = string.Format("{0:N2}", restante);
                        evento.SuppressKeyPress = true;
                        return;
                    }

                    if (Convert.ToDecimal(txtCrediario.Text) > 0 && verificaDescontoPagamento("CR") == false)
                    {
                        return;
                    }

                    MostraClienteCR();
                    evento.SuppressKeyPress = true;
                };
            };
            #endregion
            #region Cheque
            txtCheque.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
                if (evento.KeyChar == 27) btCancelarPg_Click(objeto, evento);
            };
            txtCheque.Enter += (objeto, evento) =>
            {
                txtCheque.Text = string.Format("{0:n2}", restante);
                controle = "txtCheque";
                VerificarPagamento();
                txtCheque.SelectAll();
            };

            txtCheque.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {
                    if (Convert.ToDecimal(txtCrediario.Text) < 0)
                    {
                        txtCrediario.Text = "0,00";
                    }

                    txtCheque.Text = Funcoes.FormatarDecimal(txtCheque.Text);

                    if (Convert.ToDecimal(txtCheque.Text) > 0 && verificaDescontoPagamento("CH") == false)
                    {
                        return;
                    }

                    MostraClienteCH();
                    evento.SuppressKeyPress = true;
                }
            };

            #endregion

            btnCliente.Click += (objeto, evento) =>
            {
                MostrarConsumidor();
            };
            #region Tecla Desconto Item
            txtDescontoPercItem.Leave += (objeto, evento) =>
            {
                if (string.IsNullOrEmpty(txtDescontoPercItem.Text))
                    txtDescontoPercItem.Text = "0,00";
            };

            txtDescontoPercItem.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            };
            txtDescontoPercItem.Enter += (objeto, evento) => controle = ActiveControl.Name;

            txtDescontoPercItem.KeyPress += (objeto, evento) =>
            {
                if (evento.KeyChar == (char)Keys.Escape)
                {
                    grpPreco.Visible = false;
                    grpQtd.Visible = false;
                    pnDigitacao.Visible = true;
                    txtcodProduto.Focus();
                }
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            };

            txtDescontoPercItem.KeyDown += (objeto, evento) =>
            {

                if (evento.KeyValue == 13)
                {
                    if (Convert.ToDecimal(txtDescontoPercItem.Text) < 0)
                    {
                        txtDescontoPercItem.Text = "0,00";
                    }

                    Funcoes.TravarTeclado(true);
                    if (txtDescontoPercItem.Text == "" || txtDescontoPercItem.Text == "," || txtDescontoPercItem.Text == ".") txtDescontoPercItem.Text = "0";
                    //txtDescontoPercItem.Text = Funcoes.FormatarDecimal(txtDescontoPercItem.Text);                        
                    txtDescontoPercItem.Text = (Math.Truncate(Convert.ToDecimal(txtDescontoPercItem.Text) * 100) / 100).ToString();

                    //txtDescontoPercItem.Text = decimal.Round(Convert.ToDecimal(txtDescontoPercItem.Text), 1).ToString();
                    txtDescontoPercItem.Text = Funcoes.FormatarDecimal(txtDescontoPercItem.Text);

                    if (Convert.ToDecimal(txtDescontoPercItem.Text) > 0)
                    {
                        txtDescontoPercItem.Enabled = false;
                        txtPreco.Enabled = false;
                        Lancar();
                    }
                    Funcoes.TravarTeclado(false);
                    SendKeys.Send("{TAB}");
                    evento.SuppressKeyPress = true;
                };
            };

            #endregion

            #region Tecla Preço de Venda
            txtPreco.Enter += (objeto, evento) => controle = ActiveControl.Name;
            txtPreco.KeyPress += (objeto, evento) =>
                {
                    if (evento.KeyChar == (char)Keys.Escape)
                    {
                        grpPreco.Visible = false;
                        pnDigitacao.Visible = true;
                        txtcodProduto.Focus();
                    }
                    Funcoes.DigitarNumerosPositivos(objeto, evento);
                };
            txtPreco.Leave += (objeto, evento) => txtPreco.Text = Funcoes.FormatarDecimal(txtPreco.Text);
            txtPreco.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {
                    if (Convert.ToDecimal(txtPreco.Text) < 0)
                    {
                        txtPreco.Text = "0,00";
                    }

                    Lancar();
                    evento.SuppressKeyPress = true;
                }
            };

            #endregion

            clientePDV.txtIdCliente.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyCode == Keys.Return && clientePDV.tipoPagamento == "CH")
                {
                    pnlCliente.Height = 401;
                    if (clientePDV.pnlCheque.Visible == true)
                        clientePDV.txtCodBanco.Focus();
                    if (clientePDV.idCliente == 0)
                        pnlCliente.Height = 330;
                    // pnlCliente.Location = new System.Drawing.Point(80, 150); 

                }
            };


            /** Ativar anúncios do PDV */
            try
            {
                var sql = "SELECT exibirAnunciosPDV FROM configfinanc WHERE codigofilial = '" + GlbVariaveis.glb_filial + "';";
                var entidade = Conexao.CriarEntidade();
                var result = entidade.ExecuteStoreQuery<String>(sql).FirstOrDefault();
                if (result == "S")
                {
                    anuncioHabilitado = true;
                    indoor.Visible = true;
                    btnAnuncio.Visible = true;
                }
                else
                {
                    anuncioHabilitado = false;
                }
            }
            catch (Exception)
            {
                anuncioHabilitado = false;
            }

            
            
            /* Mensagem na abertura */
            verificarMensagem();



            /** Verifica se foi realizada a exportação dos XMLs dos cupons fuscais */
            /*try
            {

                exportacaoRealizada = false;

                int hoje = DateTime.Now.Day;
                DateTime primeiroDia = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var sql2 = "SELECT IFNULL(dataexportacaoxmls, DATE_SUB(CURDATE(), INTERVAL 30 DAY)) AS dia FROM filiais WHERE codigofilial = '" + GlbVariaveis.glb_filial + "';";
                var entidade2 = Conexao.CriarEntidade();
                var dataexportacaoxmls = entidade2.ExecuteStoreQuery<DateTime>(sql2).FirstOrDefault();

                if (dataexportacaoxmls < primeiroDia)
                {
                    exportacaoRealizada = false;
                }
                else
                {
                    exportacaoRealizada = true;
                }
            }
            catch (Exception)
            {

            }*/

            try
            {

                //exportacaoRealizada = false;

                //int hoje = DateTime.Now.Day;
                //DateTime primeiroDia = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var sql2Contigencia = " SELECT COUNT(1) FROM contdocs AS c  WHERE c.protocolo = '000000000000000' AND c.modeloDOCFiscal = '65' AND c.dpfinanceiro = 'Venda' AND c.CodigoFilial = '"+GlbVariaveis.glb_filial+"' "+
                                      " AND c.DATA > (CURRENT_DATE - INTERVAL 30 DAY)  AND ABS(ecfcontadorcupomfiscal) = '"+ int.Parse(ConfiguracoesECF.NFCserie).ToString() + "' " +
                                      " AND c.estornado = 'N'";


                var sql2Pendentes = " SELECT COUNT(1) FROM contdocs AS c  WHERE (c.protocolo IS NULL OR c.protocolo = 'Erro' OR c.protocolo = '0') " +
                                     " AND c.modeloDOCFiscal = '65' AND c.dpfinanceiro = 'Venda' AND c.CodigoFilial = '" + GlbVariaveis.glb_filial +"' AND c.DATA > (CURRENT_DATE - INTERVAL 30 DAY)  "+
                                     " AND ABS(ecfcontadorcupomfiscal) = '" + int.Parse(ConfiguracoesECF.NFCserie).ToString() + "' " +
                                     " AND c.estornado = 'N' AND c.operador = '"+GlbVariaveis.glb_Usuario+"'";


                var entidade2 = Conexao.CriarEntidade();
                var qtdContigencia = entidade2.ExecuteStoreQuery<int>(sql2Contigencia).FirstOrDefault();

                var qtdPendentes = entidade2.ExecuteStoreQuery<int>(sql2Pendentes).FirstOrDefault();

                if (qtdPendentes > 0 || qtdContigencia > 0)
                {
                    string mensagem = "Existem " + qtdContigencia.ToString() + " NFC-e em Contigencia e " + qtdPendentes.ToString() + " Pendentes de Autorização";
                    pnlAviso.Visible = true;
                    pnlAviso.BackColor = Color.Red;
                    lblAvisoImportante.Text = mensagem;
                }
            }
            catch (Exception)
            {

            }
            


            // DAV
            if (ConfiguracoesECF.davporImpressoraNaoFiscal || ConfiguracoesECF.davPorECF)
            {
                try
                {
                    var sql = "SELECT impulsionarVendasAnunciosPDV FROM configfinanc WHERE codigofilial = '" + GlbVariaveis.glb_filial + "';";
                    var entidade = Conexao.CriarEntidade();
                    var result = entidade.ExecuteStoreQuery<String>(sql).FirstOrDefault();
                    if (result == "S")
                    {
                        sugerirProdutos = true;
                    }
                    else
                    {
                        sugerirProdutos = false;
                    }
                }
                catch (Exception)
                {
                    sugerirProdutos = false;
                }
            }
        }

      

        private void ItensPedidoIQCard_gerarVenda(object sender, EventArgs e)
        {
            MostrarItens();
            Totalizar();
        }

        private void MostrarCedulas()
        {
            if (!Directory.Exists(@Application.StartupPath + @"\imagens\moeda"))
            {
                return;
            }

            //Limpando buttos
            try
            {
                if (valorSelecionadoCedula == 0)
                {
                    foreach (Control item in cedulas.Controls)
                    {
                        if (item is Button)
                        {
                            item.Text = "";
                            item.BackColor = System.Drawing.SystemColors.Control;
                        };
                    }
                }

                pnlCedulas.Visible = true;
                pnlCedulas.Size = new Size(cedulas.Width, cedulas.Height);
                pnlCedulas.Anchor = AnchorStyles.Right;
                pnlCedulas.Location = new Point(this.Width - 576, 155);
                pnlCedulas.Controls.Add(cedulas);
                pnlCedulas.BringToFront();
            }
            catch
            {
                pnlCedulas.Visible = false;
            }
        }

        private void ValorCedula(decimal valor)
        {
            valorSelecionadoCedula += valor;
            if (valorSelecionadoCedula >= restante)
            {
                txtDinheiro.Text = string.Format("{0:N2}", valorSelecionadoCedula);
                Dinheiro();
            }
            txtDinheiro.Focus();
        }

        private void ChamarDV()
        {
            if (totalTransacao == 0)
            {
                MessageBox.Show("Processe a devolução após digitar os itens");
                return;
            }

            FrmDevolucao dev = new FrmDevolucao();
            dev.ShowDialog();
            if (FrmDevolucao.numeroDevolucao > 0)
            {
                devolucaoNumero = FrmDevolucao.numeroDevolucao;
                totalDevolucao = FrmDevolucao.totalDevolucao;

                if (descontoGerencial == true)
                    this.chamaDescontoGerencial();

                Totalizar();
            }
        }

        private void ChamarPED()
        {
            if (!Permissoes.operadorCaixa)
            {
                MessageBox.Show("Operador sem permissão para lançamento");
                return;
            }
            frmPED pedy = new frmPED();
            pedy.ShowDialog();
            txtcodProduto.Focus();
        }

        private void MostrarConsumidor()
        {
            pnlCliente.Enabled = true;
            pnlCliente.Width = 600;
            clientePDV.Height = 450;
            pnlCliente.Visible = true;
            pnlCliente.BackColor = System.Drawing.SystemColors.ActiveCaption;
            pnlCliente.BringToFront();
            clientePDV.txtIdCliente.Enabled = true;
            clientePDV.txtIdCliente.Text = "0";
            //clientePDV.txtIdCliente.Text = "";
            clientePDV.tipoPagamento = "";
            clientePDV.pnlParcelamentoCR.Visible = false;
            clientePDV.txtIdCliente.Focus();
            clientePDV.pnlCheque.Visible = false;
            pnlCliente.BringToFront();
            DesativarBotoes();
            Application.DoEvents();
        }

        private void MostraClienteCR()
        {
            if (Convert.ToDecimal(txtCrediario.Text) <= 0 && controle == "txtCrediario")
            {
                txtCheque.Focus();
                return;
            }
            if (Convert.ToDecimal(txtCheque.Text) <= 0 && controle == "txtCheque")
            {
                txtCartao.Focus();
                return;
            }

            if (Convert.ToDecimal(txtCrediario.Text) > restante)
            {
                txtCrediario.Text = string.Format("{0:n2}", restante);
                txtCrediario.Focus();
                return;
            }

            if (Convert.ToDecimal(txtCheque.Text) > restante)
            {
                txtCheque.Text = string.Format("{0:n2}", restante);
                txtCheque.Focus();
                return;
            }



            pnlCliente.Enabled = true;
            clientePDV.Height = 450;
            pnlCliente.Height = 450;
            pnlCliente.Width = 450;
            clientePDV.txtIdCliente.Enabled = true;
            clientePDV.pnlCheque.Visible = false;
            pnlCliente.Visible = true;
            pnlCliente.BringToFront();
            clientePDV.tipoPagamento = "CR";
            clientePDV.layoutConsumidor.Visible = false;
            pnlPagamento.Enabled = false;

            if (clientePDV.idCliente == 0)
            {
                clientePDV.pnlParcelamentoCR.Enabled = false;
                clientePDV.txtIdCliente.Focus();
            }
            clientePDV.txtIdCliente.Focus();
        }

        private void MostraClienteCH()
        {
            // Chama aqui somente para constuir o painel
            MostraClienteCR();
            pnlCliente.Enabled = true;
            clientePDV.tipoPagamento = "CH";
            clientePDV.pnlParcelamentoCR.Visible = false;
            clientePDV.txtValorIndCH.Text = String.Format("{0:n2}", Convert.ToDecimal(txtCheque.Text));
            pnlCliente.Refresh();
            if (clientePDV.idCliente != 0)
            {
                /// O cliente tem que ser o mesmo do crediário
                if (crediario != 0)
                    clientePDV.txtIdCliente.Enabled = false;

                //clientePDV.Size = new System.Drawing.Size(370, 400);
                pnlCliente.Width = 450;
                pnlCliente.Height = 530;
                clientePDV.Height = 530;



                clientePDV.pnlCheque.Visible = true;
                clientePDV.txtCodBanco.Focus();
            }
        }

        private void EntraCodigo(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    frmTeclasAtalho teclas = new frmTeclasAtalho();
                    teclas.ShowDialog();
                    break;
                case Keys.F2:
                    ChamarProdutos(false);
                    break;
                case Keys.F5:
                    ChamarSangria();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.F6:
                    ChamarCaixa();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.F7:
                    ChamarMenuFiscal();
                    break;
                case Keys.F8:
                    ChamarExportacao();
                    break;
                case Keys.F9:
                    //ChamarBackup();
                    anuncioIndoor();
                    break;
                case Keys.F10:
                    ChamarPreVenda(false, true);
                    e.SuppressKeyPress = true;
                    break;
                case Keys.F11:
                    ChamarProdutos(true);
                    e.SuppressKeyPress = true;
                    break;
                case Keys.F12:
                    chamarSincronizador();
                    break;
                case Keys.Insert:
                    ChamarDadosOS();
                    break;
                case Keys.Home:
                    MudarOperador();
                    break;
                case Keys.Multiply:
                    x();
                    e.SuppressKeyPress = true;
                    break;
               
                //case Keys.X:
                //    x();
                //    e.SuppressKeyPress = true;
                //    break;
                case Keys.Oemplus:
                    x();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Return:

                    if (txtcodProduto.Text.Length == 17 && txtcodProduto.Text.Contains("#"))
                    {
                        Venda.IQCard = txtcodProduto.Text.Replace("#", "");
                        PedidosIQCard pedido = new PedidosIQCard();
                        pedido.ShowDialog();
                        txtcodProduto.Text = "";
                        return;
                    }


                    if (txtcodProduto.Text == "#" || txtcodProduto.Text == "#0000" || txtcodProduto.Text == "*")
                    {
                        Venda.IQCard = "";
                        txtcodProduto.Text = "";
                        lblInfoCliente.Text = "";
                        break;

                    }

                    if (txtcodProduto.Text == "#tokeniqcard" || txtcodProduto.Text == "#tokenIQCARD" || txtcodProduto.Text == "#token")
                    {
                        ChamarAnuncio();                        
                        //TokenIQCARD token = new TokenIQCARD();
                        //token.ShowDialog();
                        //return;
                    }

                    if (!string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard) && (txtcodProduto.Text.ToUpper() == "#IQCARD" || txtcodProduto.Text.ToUpper() == "#PEDIDOS"))
                    {
                        MostrarPedidos();
                        return;
                    }

                    if(txtcodProduto.Text=="#i")
                    {
                        FrmAnuncio anuncio = new FrmAnuncio();
                        anuncio.ShowDialog();
                    }

                    if (txtcodProduto.Text == "#upimagens")
                    {
                        string diretorioImagens = @"C:\iqsistemas\SICEpdv\imagensupcloud";

                        
                        if (!Directory.Exists(diretorioImagens))
                            {
                            Directory.CreateDirectory(diretorioImagens);
                        }

                        string[] dirs = Directory.GetFiles(diretorioImagens);
                        if (dirs.Count() == 0)
                        {
                            MessageBox.Show("Não existem imagens no diretorio " + diretorioImagens);
                            return;
                        }

                        FrmMsgOperador msg = new FrmMsgOperador("", "Enviando imagens para a nuvem");

                        try
                        {
                            msg.Show();
                            Application.DoEvents();
                            IqCard cloud = new IqCard();
                            var total = cloud.EnviarImagensCloud();
                            MessageBox.Show(total.ToString() +" Imagens enviadas");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            msg.Dispose();
                        }


                    }
                        

                    // IQ CARD
                    //Aqui valida o CPF 

                    

                    if (GlbVariaveis.glb_chaveIQCard != "" && Funcoes.ValidarCPF(txtcodProduto.Text.Trim()))
                    {
                        Venda.IQCard = txtcodProduto.Text;
                        lblInfoCliente.Text = txtcodProduto.Text;
                        txtcodProduto.Text = "";
                        e.SuppressKeyPress = true;
                        break;
                    }


                    if (txtcodProduto.TextLength >= 16 || txtcodProduto.Text.StartsWith("#") || txtcodProduto.Text.StartsWith("*"))
                    {
                        if (txtcodProduto.Text.StartsWith("0359") || txtcodProduto.Text.StartsWith("#") || txtcodProduto.Text.StartsWith("*"))
                        {
                            if (txtcodProduto.Text.Trim().Length == 12)
                            {
                                if (!Funcoes.ValidarCPF(txtcodProduto.Text.Trim().Replace("#", "")))
                                {
                                    MessageBox.Show("CPF inválido");
                                    txtcodProduto.Text = "";
                                    e.SuppressKeyPress = true;
                                    break;
                                }
                            }
                            if (ConfiguracoesECF.pdv && !Funcoes.VerificarConexaoInternet())
                            {
                                MessageBox.Show("Não existe conexão com a internet o cliente não poderá receber os pontos fidelidade agora. ");
                                return;
                            };





                            if (IqCard.VerificarNumeroCartao(txtcodProduto.Text))
                            {
                                Venda.IQCard = txtcodProduto.Text;
                                lblInfoCliente.Text = txtcodProduto.Text;
                                txtcodProduto.Text = "";
                                e.SuppressKeyPress = true;
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Número cartão fidelidade IQCARD incorreto");
                                Venda.IQCard = "";
                                txtcodProduto.Text = "";
                                e.SuppressKeyPress = true;
                                break;
                            }
                        }



                        if (IqCard.saldoPontos < 200)
                        {
                            lblInfoCliente.Text += "SEUS CRÉDITOS FIDELIDADE ESTÃO QUASE ESGOTADOS";
                        }

                    };


                    MudarPreco();
                    if (Configuracoes.mudarPrecoVenda)
                    {
                        DigitarQuantidade(true);
                        e.SuppressKeyPress = true;
                        return;
                    }

                    //if (digitarQuantidade == true || (GlbVariaveis.glb_balanca == true))
                    //{
                    // produto.ProcurarCodigo(txtcodProduto.Text, GlbVariaveis.glb_filial);

                    //if(produto.situacao == "")

                    if (digitarQuantidade) {
                        DigitarQuantidade(true);
                    }
                    else
                        Lancar();

                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void ChamarDadosOS()
        {

            if (ConfiguracoesECF.perfil == "Y")
            {
                MessageBox.Show("Não é permitido para o perfil Y");
                return;
            }

            if (ConfiguracoesECF.idECF == 0)
            {
                MessageBox.Show("Função somente com ECF ativo");
                return;
            }
            FrmDadosOS frmOS = new FrmDadosOS();
            frmOS.ShowDialog();
        }

        private void AtivarDigitacaoPreco()
        {
            if (Configuracoes.mudarPrecoVenda)
                return;

            FrmLogon logon = new FrmLogon();
            logon.campo = "venalterarpreco";
            logon.txtDescricao.Text = "Digitar preço na venda";
            logon.ShowDialog();
            if (Operador.autorizado)
            {
                Configuracoes.mudarPrecoVenda = true;
            }
        }

        private static void ChamarBackup()
        {
            FrmBackup backup = new FrmBackup();
            backup.ShowDialog();
        }

        private void x()
        {
            try
            {
                txtcodProduto.Text = string.Format("{0:n3}", Convert.ToDecimal(txtcodProduto.Text));
                quantidade = Math.Round(Convert.ToDecimal(txtcodProduto.Text), 3);
                txtQtd.Text = string.Format("{0:N3}", quantidade);
                lblQtd.Text = quantidade.ToString() + " X";
                if (quantidade <= 0)
                    throw new Exception();
            }
            catch
            {
                lblQtd.Text = "1 X";
                quantidade = 0;
                txtcodProduto.Text = "";
                txtcodProduto.Focus();
            }
            txtcodProduto.Text = "";
            txtcodProduto.Focus();
        }

        // Formatar(ActiveControl.Name, "{0:n2}");
        private void Formatar(string controle, string formatacao)
        {
            Control[] ctls = this.Controls.Find(controle, true);

            if (ctls.Length > 0)
            {
                TextBox txt = ctls[0] as TextBox;

                try
                {
                    txt.Text = String.Format(formatacao, Convert.ToDecimal(txt.Text));
                    txt.SelectAll();
                }
                catch
                {
                    txt.Text = "0,00";
                    txt.SelectAll();
                    txt.Focus();
                }
            }
        }

        private void TeclaEnter()
        {
            switch (controle)
            {

                case "txtcodProduto":
                    quantidade = Convert.ToDecimal(txtQtd.Text.Replace("X", ""));
                    if (Configuracoes.mudarPrecoVenda)
                        MudarPreco();
                    else
                        Lancar();
                    break;
                case "txtDinheiro":
                    Dinheiro();
                    break;
                case "txtCrediario":
                    MostraClienteCR();
                    break;
                case "txtCheque":
                    MostraClienteCH();
                    break;
                case "txtCartao":
                    MostraCartoes();
                    break;
                case "btConfirmaCA":
                case "txtParcelamentoCA":
                    Cartao();
                    break;
                case "btConfirmarCH":
                case "txtIntervaloCH":
                    Cheque();
                    break;
                case "btConfirmarCR":
                case "txtIntervaloCR":
                    Crediario();
                    break;
                case "txtDesconto":
                    Desconto();
                    //if (txtDesconto.Enabled==false)
                    //SendKeys.Send("{TAB}");                    
                    break;
                case "txtIdCliente":


                    clientePDV.ProcuraCliente();
                    if (clientePDV.idCliente > 0)
                    {
                        clientePDV.btnExtrato.Enabled = true;

                        if (clientePDV.tipoPagamento == "CH")
                        {
                            pnlCliente.Height = 401;
                            clientePDV.Size = new System.Drawing.Size(357, 400);
                            clientePDV.txtCodBanco.Focus();
                            pnlCliente.Height = 455;
                        }
                    }

                    if (clientePDV.idCliente == 0)
                        pnlCliente.Height = 335;

                    clientePDV.txtIdCliente.Focus();
                    break;
                case "txtDescontoPercItem":
                    if (txtDescontoPercItem.Text == "") txtDescontoPercItem.Text = "0";
                    if (Convert.ToDecimal(txtDescontoPercItem.Text) > 0)
                        Lancar();
                    else
                        txtPreco.Focus();

                    break;
                case "txtPreco":
                    Lancar();
                    break;

                default:
                    Control[] ctls = this.Controls.Find(controle, true);
                    ctls[0].Focus();
                    SendKeys.Send("{TAB}");
                    break;
            }
        }

        private void Dinheiro()
        {
            try
            {
                txtDinheiro.Text = Funcoes.FormatarDecimal(txtDinheiro.Text);
                if (Convert.ToDecimal(txtDinheiro.Text) == 0)
                {
                    if (txtCrediario.Enabled == true)
                        txtCrediario.Focus();
                    else
                        txtCheque.Focus();
                    return;
                }
            }
            catch
            {
                txtDinheiro.Text = string.Format("{0:n2}", dinheiro);
                return;
            }

            try
            {
                pnlPagamento.Enabled = false;
                Funcoes.TravarTeclado(true);
                DesativaDesconto();

                // Construindo a Classe e Atribuindo os Valores   
                if (Convert.ToDecimal(txtDinheiro.Text) >= restante && ConfiguracoesECF.pdv)
                {
                    lblDescricaoPrd.Text = "";
                    lblQtd.Text = "";

                    pnlTroco.BringToFront();
                    pnlTroco.Visible = true;
                    pnlCedulas.Visible = false;
                    this.troco.lblTroco.Text = string.Format("{0:C2}", Convert.ToDecimal(txtDinheiro.Text) - restante);
                    Application.DoEvents();
                };


                caixas DH = new caixas();
                DH.valor = Convert.ToDecimal(txtDinheiro.Text);
                DH.tipopagamento = "DH";
                listFormaPagamentoAuditoria.Add(DH);

                ConstruirVenda();
                if (!venda.EfetuarPagamento(dpFinanceiro, "DH", Convert.ToDecimal(txtDinheiro.Text), clientePDV.idCliente, 0, "", "", 0, DateTime.Now, 0, null, true, true, false, false)) return;

                dinheiro = Convert.ToDecimal(txtDinheiro.Text);
                txtDinheiro.Text = string.Format("{0:n2}", dinheiro);
                restante -= dinheiro;
                if (restante < 0) restante = 0;

                if (dinheiro > 0) txtDinheiro.Enabled = false;

                if (venda.vendaFinalizada)
                {
                    // Mensagem enquanto a gaveta estiver aberta
                    //case "gaveta":
                    //lblMensagem.Font = new Font("Arial", 20);
                    //int x = 0;
                    //int y = 0;
                    //while (FuncoesECF.EstadoGaveta() == 0)
                    //{
                    //    y++;
                    //    lblMensagem.Text = mensagem;
                    //    Application.DoEvents();
                    //    if (y > 10)
                    //        x++;
                    //}
                    pnlTroco.Visible = false;
                    AtivarBotoes();
                    limparDados();
                    zerarVariaveis();

                }
            }
            catch (Exception erro)
            {
                Funcoes.TravarTeclado(false);
                MessageBox.Show(erro.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                pnlImpulsionado.Visible = false;
                pnlPagamento.Enabled = true;
                Funcoes.TravarTeclado(false);
                pnlTroco.Visible = false;
            }
            txtCrediario.Focus();
        }


        private void VendaAV()
        {
            decimal valorAbatimento = 0;

            if (clientePDV.idCliente == 0)
                return;

            if (clientePDV.idCliente > 0)
            {
                var sql = "SELECT creditoAV FROM clientes where codigo=' " + clientePDV.idCliente.ToString() + "'";

                decimal? creditoAV = Conexao.CriarEntidade().ExecuteStoreQuery<decimal>(sql).FirstOrDefault();

                if (creditoAV <= 0)
                    return;

                if (creditoAV > 0)
                {
                    if (MessageBox.Show("Cliente possui um crédito de AV, usar esse crédito agora ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (creditoAV <= restante)
                        {
                            valorAbatimento = creditoAV.Value;
                        }
                        else
                        {
                            valorAbatimento = restante;
                        }
                    }
                }
            }

            try
            {
                pnlPagamento.Enabled = false;
                Funcoes.TravarTeclado(true);

                // Construindo a Classe e Atribuindo os Valores   

                ConstruirVenda();
                if (!venda.EfetuarPagamento(dpFinanceiro, "AV", valorAbatimento, clientePDV.idCliente, 0, "", "", 0, DateTime.Now, 0, null, true, true, false, false)) return;
                vendaAV = valorAbatimento;
                restante -= vendaAV;

                if (restante < 0) restante = 0;


                if (venda.vendaFinalizada)
                {
                    // Mensagem enquanto a gaveta estiver aberta
                    //case "gaveta":
                    //lblMensagem.Font = new Font("Arial", 20);
                    //int x = 0;
                    //int y = 0;
                    //while (FuncoesECF.EstadoGaveta() == 0)
                    //{
                    //    y++;
                    //    lblMensagem.Text = mensagem;
                    //    Application.DoEvents();
                    //    if (y > 10)
                    //        x++;
                    //}
                    pnlTroco.Visible = false;
                    AtivarBotoes();
                    limparDados();
                    zerarVariaveis();
                }

            }
            catch (Exception erro)
            {
                Funcoes.TravarTeclado(false);
                MessageBox.Show(erro.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                pnlImpulsionado.Visible = false;
                pnlPagamento.Enabled = true;
                Funcoes.TravarTeclado(false);
                pnlTroco.Visible = false;
            }

            txtDinheiro.Focus();
        }

        private void Devolucao()
        {
            try
            {
                if (totalDevolucao > restante)
                {
                    MessageBox.Show("Total da troca de produto maior que a compra", "SICEpdv.net", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AbortarPagamento();
                }
            }
            catch
            {
                return;
            }

            try
            {
                //this.Enabled = false;
                pnlPagamento.Enabled = false;
                Funcoes.TravarTeclado(true);

                // Construindo a Classe e Atribuindo os Valores   

                ConstruirVenda();
                if (!venda.EfetuarPagamento(dpFinanceiro, "DV", Convert.ToDecimal(string.Format("{0:N2}", totalDevolucao)), clientePDV.idCliente, 0, "", "", 0, DateTime.Now, 0, null, true, true, false, false)) return;

                restante -= totalDevolucao;
                if (restante < 0) restante = 0;

                if (venda.vendaFinalizada)
                {
                    AtivarBotoes();
                    limparDados();
                    zerarVariaveis();
                }

            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                //this.Enabled = true;
                pnlImpulsionado.Visible = false;
                pnlPagamento.Enabled = true;
                Funcoes.TravarTeclado(false);
            }
            txtDinheiro.Focus();
        }

        private void CriarPnlTroco()
        {
            //pnlTroco.Height = dtgItens.Height;
            //pnlTroco.Width = dtgItens.Width;
            troco.Width = (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 370) < 1110 ? (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 370) : 1110;
            troco.Height = (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 500); 

            pnlTroco.Size = new System.Drawing.Size(troco.Size.Width, troco.Size.Height);
            pnlTroco.Location = new System.Drawing.Point(21, 142);
            pnlTroco.Controls.Add(troco);
            this.Controls.Add(pnlTroco);
            System.Threading.Thread.Sleep(5000);
            pnlTroco.Visible = false;
        }

        private void ConstruirVenda()
        {
            venda.dpFinanceiro = "Venda";
            venda.classeVenda = classeVenda;
            venda.valorBruto = totalTransacao - encargos;
            venda.desconto = desconto;
            venda.encargos = encargos;

            venda.idCliente = clientePDV.idCliente;

            venda.valorLiquido = Math.Round(totalTransacao - desconto, 2);
            venda.numeroDevolucao = devolucaoNumero;
            venda.totalDevolucao = totalDevolucao;
            venda.parcelamentoMaximo = parcelamentoMaximo;
            Venda.dependente = clientePDV.dependente;
            venda.numeroDAV = numeroDAV;
            venda.numeroPreVenda = numeroPreVenda;
            venda.restante = restante;
        }

        private void MostraCartoes()
        {
            /// Cartoes sempre será a última forma de pagamento
            /// pois o TEF precisa da finalização do Cupom Fiscal para puder 
            /// imprimir o comprovante de pagamento
            /// 

            ActiveControl = cartoes.txtCodigoCartao;

            if (qtdCartoesFeitas + 1 >= QTDCARTOES)
                cartoes.txtValorIndCA.Enabled = false;

            if (Convert.ToDecimal(txtCartao.Text) <= 0) return;
            txtCartao.Text = string.Format("{0:n2}", restante);
            pnlCartoes.Visible = true;
            pnlCartoes.BringToFront();
            pnlPagamento.Enabled = false;
            cartoes.txtParcelamentoCA.Text = "1";
            cartoes.btConfirmaCA.Enabled = true;
            cartoes.idCartao = 0;
            cartoes.txtValorIndCA.Text = txtCartao.Text;
            cartoes.txtNomeCartao.Text = "";
            cartoes.txtNrCartao.Text = "";
            cartoes.DesTravaCartoes();
            //cartoes.txtValorIndCA.Focus();
            //if (cartoes.txtValorIndCA.Enabled == false)
            cartoes.pnlValorCA.Enabled = false;
            cartoes.txtCodigoCartao.Focus();

            
        }

        private void Crediario()
        {
            try
            {
                txtCrediario.Text = Funcoes.FormatarDecimal(txtCrediario.Text);
                if (Convert.ToDecimal(txtCrediario.Text) == 0)
                {
                    txtCheque.Focus();
                    return;
                }
            }
            catch
            {
                txtCrediario.Text = string.Format("{0:n2}", crediario);
                return;
            }

            try
            {
                VerificarTabelaJuros("CR", dpFinanceiro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            try
            {
                //this.Enabled = false;
                pnlPagamento.Enabled = false;
                if (Convert.ToDecimal(txtCrediario.Text) > restante)
                {
                    txtCrediario.Text = string.Format("{0:n2}", restante);
                    txtCrediario.Focus();
                    return;
                };
                DesativaDesconto();

                caixas CR = new caixas();
                CR.valor = Convert.ToDecimal(txtCrediario.Text);
                CR.tipopagamento = "CR";
                listFormaPagamentoAuditoria.Add(CR);

                ConstruirVenda();
                pnlCliente.Enabled = false;

                if (!venda.EfetuarPagamento(dpFinanceiro, "CR", Convert.ToDecimal(txtCrediario.Text), clientePDV.idCliente, 0, "", "", Convert.ToInt16(clientePDV.txtParcelamentoCR.Text), clientePDV.vencimentoCR.Value, Convert.ToUInt16(clientePDV.txtIntervaloCR.Text), null, true, true, clientePDV.chkAltCR.Checked, false))
                {
                    pnlPagamento.Enabled = false;
                    pnlCliente.Enabled = true;
                    return;
                }


                crediario = Convert.ToDecimal(txtCrediario.Text);
                restante -= crediario;
                txtCrediario.Text = string.Format("{0:n2}", crediario);
                if (crediario > 0) txtCrediario.Enabled = false;


                var cancelado = (from i in Conexao.CriarEntidade().contdocs
                                 where i.documento == venda.documento && i.CodigoFilial == GlbVariaveis.glb_filial
                                 select i.estornado).FirstOrDefault();


                if (venda.vendaFinalizada)
                {
                    FrmMsgOperador msg = new FrmMsgOperador("", "Imprimindo comprovante");

                    try
                    {
                        if ((ConfiguracoesECF.pdv && ConfiguracoesECF.idECF != 9999) || (ConfiguracoesECF.NFC == true && ConfiguracoesECF.pdv == true))
                        {
                            if (cancelado == "N" && MessageBox.Show("Imprimir comprovante do crediário ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                msg.Show();
                                Application.DoEvents();
                                venda.ImprimirComprovante(clientePDV.idCliente, clientePDV.nomeCliente, venda.documento, venda.sincronizada);
                            }

                            if (cancelado == "N" && MessageBox.Show("Imprimir Carnê?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                msg.Show();
                                Application.DoEvents();
                                venda.ImprimirCarne(venda.documento, venda.sincronizada);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Não foi possível imprimir comprovantes. " + ex.Message);
                    }
                    finally
                    {
                        msg.Dispose();
                        AtivarBotoes();
                        limparDados();
                        zerarVariaveis();
                        pnlCliente.Enabled = true;
                    }
                }

                pnlCliente.Visible = false;
                pnlImpulsionado.Visible = false;
                pnlPagamento.Enabled = true;
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                //this.Enabled = true;
                //pnlPagamento.Enabled = true;       
                pnlImpulsionado.Visible = false;
                pnlCliente.Enabled = true;
                Funcoes.TravarTeclado(false);
            }
            txtCheque.Focus();
        }

        private static void VerificarTabelaJuros(string tipoPagamento, string dpFinanceiro = "Venda")
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            // Aqui está passando a forma de pagamento 00 por que o default da tabela de juors é 00 
            // colocando isso para não mexer na regra de negócio pois foi colocado para as outras 
            //formas de pagamentos cartao e cheque

            if (tipoPagamento == "CR")
            {

                var tabJuros = (from n in entidade.juros
                                where n.codigofilial == GlbVariaveis.glb_filial
                                && (n.tipopagamento == tipoPagamento || n.tipopagamento == "00")
                                select n).Take(1);
                if (tabJuros.Count() > 0 && frmTabelaJuros.financiamentoCalculado == false && dpFinanceiro == "Venda")
                {
                    throw new Exception("Você tem tabela de juros cadastradas. Forma de pagamento no crediário escolha antes o financiamento e os juros da tabela correspondente.");
                }
            }
            else
            {
                var tabJuros = (from n in entidade.juros
                                where n.codigofilial == GlbVariaveis.glb_filial
                                && n.tipopagamento == tipoPagamento
                                select n).Take(1);

                if (tabJuros.Count() > 0 && frmTabelaJuros.financiamentoCalculado == false && dpFinanceiro == "Venda")
                {
                    throw new Exception("Você tem tabela de juros cadastradas. Forma de pagamento no crediário escolha antes o financiamento e os juros da tabela correspondente.");
                }
            }
        }

        private static bool verificarSituacaoJuros(int codigoTabela)
        {

            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);


            string situacao = entidade.ExecuteStoreQuery<string>("select IFNULL(situacaoprodutos,'') as situacaoprodutos from juros where codigo = '" + codigoTabela + "';").FirstOrDefault();
            var produtosNpromocao = (from v in entidade.vendas
                                     where v.id == GlbVariaveis.glb_IP && v.codigofilial == GlbVariaveis.glb_filial && v.situacao != "Promoção" && v.cancelado == "N"
                                     select v).ToList();

            var produtosPromocao = (from v in entidade.vendas
                                    where v.id == GlbVariaveis.glb_IP && v.codigofilial == GlbVariaveis.glb_filial && v.situacao == "Promoção" && v.cancelado == "N"
                                    select v).ToList();

            if (situacao == "Promoção" && produtosNpromocao.Count() > 0)
            {
                MessageBox.Show("Tabela de juros apenas com produtos em promoção!");
                return false;
            }
            else if (situacao != "Promoção" && produtosPromocao.Count() > 0)
            {
                MessageBox.Show("Tabela de juros não permitida com produtos em promoção");
                return false;
            }

            return true;

        }

        private void Cartao()
        {
            try
            {
                txtCartao.Text = Funcoes.FormatarDecimal(txtCartao.Text);
                if (Convert.ToDecimal(cartoes.txtValorIndCA.Text) == 0) return;
            }
            catch
            {
                txtCartao.Text = string.Format("{0:n2}", cartao);
                return;
            }

            try
            {

                if (Convert.ToDecimal(cartoes.txtParcelamentoCA.Text) > parcelamentoMaximo && parcelamentoMaximo > 0)
                {
                    throw new Exception("Parcelamento não permitido, máximo de parcelas: " + parcelamentoMaximo.ToString());
                }

                VerificarTabelaJuros("CA", dpFinanceiro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Renomeado em 12.04.2013 por que foi detectado que sempre retornava a mensagem na Sto. Antonio

            //if (dpFinanceiro=="Venda" && !FuncoesECF.CupomFiscalAberto() && ConfiguracoesECF.pdv && ConfiguracoesECF.idECF>0)
            //{
            //    FuncoesECF.VerificaImpressoraLigada();
            //    MessageBox.Show("Cupom fiscal fechado !!", "Atenção!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            try
            {
                //this.Enabled = false;
                cartoes.btConfirmaCA.Enabled = false;
                pnlPagamento.Enabled = false;
                if (Convert.ToDecimal(cartoes.txtValorIndCA.Text) > restante)
                {
                    cartoes.txtValorIndCA.Text = string.Format("{0:n2}", restante);
                    cartoes.txtValorIndCA.Focus();
                    return;
                };

                DesativaDesconto();
                caixas CA = new caixas();
                CA.valor = Convert.ToDecimal(cartoes.txtValorIndCA.Text);
                CA.tipopagamento = "CA";
                listFormaPagamentoAuditoria.Add(CA);
                ConstruirVenda();
                if (!venda.EfetuarPagamento(dpFinanceiro, "CA", Convert.ToDecimal(cartoes.txtValorIndCA.Text), clientePDV.idCliente, cartoes.idCartao, cartoes.txtNrCartao.Text, cartoes.txtNomeCartao.Text, Convert.ToInt16(cartoes.txtParcelamentoCA.Text), GlbVariaveis.Sys_Data.AddMonths(1), 30, null, true, true, false, false))
                    return;


                cartao = Convert.ToDecimal(cartoes.txtValorIndCA.Text);
                restante -= cartao;
                txtCartao.Text = string.Format("{0:n2}", cartao);
                cartoes.txtValorIndCA.Text = string.Format("{0:n2}", restante);
                cartoes.DesTravaCartoes();
                qtdCartoesFeitas++;
                if (cartao > 0) txtCartao.Enabled = false;

                if (venda.vendaFinalizada)
                {
                    if (ConfiguracoesECF.CR == "Crediario" && dpFinanceiro == "Venda" && crediario > 0)
                        ImpressaoCrediario();

                    AtivarBotoes();
                    limparDados();
                    zerarVariaveis();
                }
                cartoes.txtValorIndCA.Focus();
            }
            catch
            {
                //MessageBox.Show(erro.Message, "Atenção!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                //if (Configuracoes.cancelarvendarejeicaocartao == false)
                //{
                //this.Enabled = true;     
                cartoes.btConfirmaCA.Enabled = true;
                Funcoes.TravarTeclado(false);
                /*}
                else
                {
                    DelegateCartoes("btSairCA");
                    AbortarPagamento();
                    limparDados();
                    pnlTroco.Visible = false;
                    AtivarBotoes();
                    zerarVariaveis();
                }*/
            }


        }

        private void Cheque()
        {

            if (!clientePDV.ValidaDadosCheque())
            {
                MessageBox.Show("Campos banco,agência,cheque,nome cheque, cpf/cnpj ch, não podem ser vazio", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            };

            try
            {
                txtCheque.Text = Funcoes.FormatarDecimal(txtCheque.Text);
                if (Convert.ToDecimal(clientePDV.txtValorIndCH.Text) == 0) return;
            }
            catch
            {
                txtCheque.Text = string.Format("{0:n2}", cheque);
                return;
            }


            try
            {
                VerificarTabelaJuros("CH", dpFinanceiro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            try
            {
                //this.Enabled = false;
                pnlPagamento.Enabled = false;
                Funcoes.TravarTeclado(true);
                if (Convert.ToDecimal(clientePDV.txtValorIndCH.Text) > restante)
                {
                    clientePDV.txtValorIndCH.Text = string.Format("{0:n2}", restante);
                    clientePDV.txtValorIndCH.Focus();
                    Funcoes.TravarTeclado(false);
                    return;
                };
                DesativaDesconto();

                caixas CH = new caixas();
                CH.valor = Convert.ToDecimal(clientePDV.txtValorIndCH.Text);
                CH.tipopagamento = "CH";
                listFormaPagamentoAuditoria.Add(CH);

                ConstruirVenda();
                pnlCliente.Enabled = false;
                Funcoes.TravarTeclado(false);
                if (!venda.EfetuarPagamento(dpFinanceiro, "CH", Convert.ToDecimal(clientePDV.txtValorIndCH.Text), clientePDV.idCliente, clientePDV.idCartao, "", "", Convert.ToInt16(clientePDV.txtParcelamentoCH.Text), clientePDV.vencimentoCH.Value, Convert.ToInt16(clientePDV.txtIntervaloCH.Text), clientePDV.dadoCheque, true, true, clientePDV.chkAltCH.Checked, false))
                {
                    Funcoes.TravarTeclado(false);
                    pnlPagamento.Enabled = false;
                    pnlCliente.Enabled = true;
                    return;
                }

                cheque = Convert.ToDecimal(clientePDV.txtValorIndCH.Text);
                restante -= cheque;
                clientePDV.txtValorIndCH.Text = string.Format("{0:n2}", restante);
                if (cheque > 0) txtCheque.Enabled = false;

                if (venda.vendaFinalizada)
                {
                    if (ConfiguracoesECF.CR == "Crediario" && dpFinanceiro == "Venda" && crediario > 0)
                        ImpressaoCrediario();

                    AtivarBotoes();
                    limparDados();
                    zerarVariaveis();
                }
                if (Convert.ToDecimal(txtCheque.Text) < cheque)
                    clientePDV.txtValorIndCH.Focus();
                else
                {
                    txtCheque.Text = string.Format("{0:n2}", cheque);
                    //this.Enabled = true;
                    Funcoes.TravarTeclado(false);
                    pnlCliente.Visible = false;
                    pnlImpulsionado.Visible = false;
                    pnlPagamento.Enabled = true;
                    txtCartao.Focus();
                }

            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                //this.Enabled = true;
                pnlCliente.Enabled = true;
                Funcoes.TravarTeclado(false);
            }
        }

        private void ImpressaoCrediario()
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Imprimindo comprovante");

            if (ConfiguracoesECF.pdv)
            {
                if (MessageBox.Show("Imprimir comprovante do crediário?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    msg.Show();
                    Application.DoEvents();
                    venda.ImprimirComprovante(clientePDV.idCliente, clientePDV.nomeCliente, venda.documento, venda.sincronizada);
                }

                if (MessageBox.Show("Imprimir Carnê?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    msg.Show();
                    Application.DoEvents();
                    venda.ImprimirCarne(venda.documento, venda.sincronizada);
                }
            }
            msg.Dispose();
        }

        private void Lancar()
        {
            pnDigitacao.Visible = true;
            txtcodProduto.BackColor = Color.White;
            txtcodProduto.ForeColor = Color.Black;
            lblAviso.Text = "";
            //txtDescontoPercItem.Text = "0,00";
            codP = produto.codigo;

            if (txtcodProduto.Text.Length == 0)
            {
                txtcodProduto.Focus();
                return;
            };

            if (verificaDadosR02() == false)
            {
                return;
            }

            if (DateTime.Now.Day > Configuracoes.GlbDataGerada.Day)
            {
                if (ConfiguracoesECF.pdv)
                {
                    MessageBox.Show("Por favor fechar o caixa! já passou das 00:00hs ");
                    return;
                }

            }


            if (Configuracoes.AbrirCupomcomCliente && dtgItens.RowCount == 0 && clientePDV.idCliente == 0)
            {
                MessageBox.Show("Configuração só aceita venda com um cliente.");
                MostrarConsumidor();
                return;
            }

            decimal qtdDisponivel = 0;
            decimal descontoPercItem = 0;
            decimal descontoValorItem = 0;
            Venda venda = new Venda();

            Venda.vendedor = Vendedor.codigoVendedor;
            decimal precoMinimo = 0;
            try
            {
                if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString() && ConfiguracoesECF.davporImpressoraNaoFiscal && dtgItens.RowCount == 0)
                {
                    ChamarDadosOS();
                }
                descontoPercItem = 0;
                produto.descontoPromocao = 0;
                var dadosPrd = produto.ProcurarCodigo(txtcodProduto.Text, GlbVariaveis.glb_filial);
                MostrarDadosItem();

                decimal precoTabela = 0;
                
                precoTabela = Produtos.tabelaPrecoQtd(txtcodProduto.Text, Produtos.tabelaPreco,Convert.ToDecimal(txtQtd.Text));
                if (precoTabela > 0)
                    produto.preco = precoTabela;


                if (produto.descontoMaximo > 0)
                {
                    precoMinimo = produto.preco * (-produto.descontoMaximo / 100) + produto.preco;
                    precoMinimo = (Math.Truncate(precoMinimo * 100) / 100);
                };


                qtdDisponivel = produto.quantidadeDisponivel;
                descontoPercItem = produto.descontoPromocao;
                Application.DoEvents();

                if (produto.situacao != "Promoção")
                    preco = produto.preco * (-descontoPercItem / 100) + produto.preco; // produto.preco - (produto.preco * (produto.preco * descontoPercItem / 100));
                else
                {
                    descontoPercItem = Math.Truncate(descontoPercItem * 100) /100;
                    preco = produto.preco * (-(descontoPercItem) / 100) + produto.preco;
                    preco = Math.Truncate(preco * 100) / 100;
                    //txtDescontoPercItem.Text = descontoPercItem.ToString("N2");
                }

                // Aqui pega a quantidade se o item foi da balança                

                if (produto.situacao == "Item da Balança" && txtcodProduto.Text.Length > 10 && quantidade <= 1 && txtcodProduto.Text.Substring(0, 1) == Configuracoes.digitoVerificadorCodBarras)
                {

                    //2000001005101                    
                    //quantidade = (Convert.ToDecimal(String.Format("{0:n3}", txtcodProduto.Text.Substring(6, 7))) / preco / 100) / 10;
                    quantidade = (Convert.ToDecimal(String.Format("{0:n3}", txtcodProduto.Text.Substring(7, 6))) / preco / 100) / 10;
                    if (Configuracoes.totalnoFinalCodBarrasBalanca == false)
                        quantidade = (Convert.ToDecimal(String.Format("{0:n3}", txtcodProduto.Text.Substring(7, 6))) / 100 / 100);

                    lblQtd.Text = string.Format("{0:N3}", quantidade);
                    if (produto.codigoBarras.Length > Configuracoes.tamanhacodBarrasBalanca)
                    {
                        lblQtd.Text = "1 X";
                        quantidade = 1;
                    }
                    DigitarQuantidade();
                    // return;
                }
                else if (produto.situacao == "Item da Balança" && GlbVariaveis.glb_balanca == true)
                {
                    if (FuncoesBalanca.ativo() == true)
                    {
                        quantidade = (Convert.ToDecimal(String.Format("{0:n3}", FuncoesBalanca.lerPeso())));
                    }
                }


                if (Configuracoes.controleLote == true && produto.controleLoteProduto == true)
                {
                    frmLotes objLotes = new frmLotes(produto.codigo, quantidade);
                    objLotes.ShowDialog();

                    if (listLotes.Count() == 0)
                    {
                        MostrarItens();
                        totalTransacao = venda.SomaItens();
                        txtTotal.Text = String.Format("{0:n2}", totalTransacao);
                        limparDados();
                        txtPreco.Enabled = true;
                        txtDescontoPercItem.Enabled = true;
                        txtDescontoPercItem.Text = "0,00";
                        txtDesconto.Text = "0,00";
                        return;
                    }



                    /*
                    string sql = "SELECT ifnull(SUM(quantidade),0) as quantidade FROM produtos_lote where codigofilial='" + GlbVariaveis.glb_filial + "' AND codigo='"+produto.codigo+"' AND data_lote >= current_date";
                    int quantidadeLote = 0;
                    int quantidadeLoteVencida = 0;
                    try
                    {
                        quantidadeLote = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql).FirstOrDefault();
                    }
                    catch
                    {
                        quantidadeLote = Conexao.CriarEntidade(false).ExecuteStoreQuery<int>(sql).FirstOrDefault();
                    }

                    if(quantidadeLote < quantidade)
                    {
                        MessageBox.Show("Saldo insuficiente nos lotes a vencer!", "Atenção", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);

                        sql = "SELECT ifnull(SUM(quantidade),0) as quantidade FROM produtos_lote where codigofilial='" + GlbVariaveis.glb_filial + "' AND codigo='" + produto.codigo + "' AND data_lote < current_date";
                        quantidadeLoteVencida = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql).FirstOrDefault();

                        if(quantidadeLoteVencida > 0)
                        {
                            MessageBox.Show("Existe(m) "+quantidadeLoteVencida+" produtos vencido(s)", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                        return;
                    }*/
                }
 
            }
            catch (Exception ex)
            {
                VerificaConexao();
                lblAviso.Text = ex.Message;
                Beep(1000, 300);
                txtcodProduto.BackColor = Color.Black;
                txtcodProduto.ForeColor = Color.White;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtcodProduto.Focus();
                txtcodProduto.SelectAll();
                return;
            }

            try
            {
                if (quantidade <= 0)
                {
                    MessageBox.Show("Quantidade sem valor!");
                    return;
                }



                txtPreco.Enabled = false;
                if (txtPreco.Text == "") txtPreco.Text = "0";
                
                if (txtDescontoPercItem.Text == "") txtDescontoPercItem.Text = "0";


                Application.DoEvents();


                if (Configuracoes.mudarPrecoVenda && produto.situacao != "Promoção")
                {
                    if (Convert.ToDecimal(txtDescontoPercItem.Text) > 0)
                    {
                        txtDescontoPercItem.Text = Funcoes.FormatarDecimal(txtDescontoPercItem.Text);
                        var precoComDesconto = produto.preco * (-Convert.ToDecimal(txtDescontoPercItem.Text) / 100) + produto.preco;
                        txtPreco.Text = string.Format("{0:n2}", precoComDesconto);
                    }

                    preco = Convert.ToDecimal(txtPreco.Text);
                    descontoPercItem = Convert.ToDecimal(txtDescontoPercItem.Text);
                    if (precoMinimo > 0 && preco < precoMinimo)
                    {
                        MessageBox.Show("Item com configuração de desconto: Preço mínimo do item: " + string.Format("{0:N2}", precoMinimo));
                        return;
                    }
                };


                if (preco <= 0)
                {
                    MessageBox.Show("Item sem valor");
                    txtcodProduto.Focus();
                    return;
                }
                string compDescricao = "";
                string lote = "";
                string grade = "Nenhuma";
                if (produto.descricaoComplementar && Conexao.onLine || (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString() && produto.tipo == "1 - Servico"))
                {
                    frmComplementoItem complemento = new frmComplementoItem();
                    complemento.ShowDialog();
                    lote = complemento.txtLote.Text;
                    compDescricao = complemento.txtComplemento.Text;
                    complemento.Dispose();
                }

                if (produto.grade.ToLower() != "nenhuma" && Conexao.onLine)
                {
                    FrmEscolherGrade frmgrade = new FrmEscolherGrade(produto.codigo, GlbVariaveis.glb_filial);
                    frmgrade.ShowDialog();
                    grade = frmgrade.gradeProduto;
                }

                // Aqui converte o total em quantidade se o produto for fração e o valor do total for maior que 0

                if (Convert.ToDecimal("0" + txtTotalItem.Text) > 0 && (produto.unidade == "FR" || produto.unidade == "L"))
                {
                    quantidade = Math.Round((Convert.ToDecimal("0" + txtTotalItem.Text) / Convert.ToDecimal(txtPreco.Text)) + 0.0005M, 3);
                }

                if (produto.quantidadeMinDesconto > 0 && descontoPercItem > 0 && quantidade < produto.quantidadeMinDesconto)
                {
                    MessageBox.Show("Desconto para o item somente com quantidade mínima de : " + produto.quantidadeMinDesconto);
                    return;
                }

                bool impressaoECF = true;

                if (ConfiguracoesECF.pdv == false)
                    impressaoECF = false;
                else
                    impressaoECF = true;


                venda.InserirItem(impressaoECF, produto.codigo, produto.descricao, compDescricao, qtdDisponivel, produto.quantidadePrat, quantidade, preco,
                    (decimal)produto.preco, produto.custo, produto.unidade, produto.embalagem, descontoPercItem, descontoValorItem, Vendedor.codigoVendedor, Convert.ToInt16(produto.icms),
                    produto.tributacao, produto.reducaoBaseCalcICMS, produto.tipo, lote, grade, produto.aceitaDesconto, "5.102", produto.pis, produto.cofins, produto.aliquotaIPI, produto.ncm, null, true, produto.cest, listLotes.FirstOrDefault(), produto.situacao);

                listLotes.Clear();

                MostrarItens();
                totalTransacao = venda.SomaItens();
                txtTotal.Text = String.Format("{0:n2}", totalTransacao);
                limparDados();
                txtPreco.Enabled = true;
                txtDescontoPercItem.Enabled = true;
                txtDescontoPercItem.Text = "0,00";
                txtDesconto.Text = "0,00";
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pnlImpulsionado.Visible = false;
            }
            finally
            {
                //VerificaConexao();
                MostrarItens();
                totalTransacao = venda.SomaItens();
                txtTotal.Text = String.Format("{0:n2}", totalTransacao);
                limparDados();
                txtPreco.Enabled = true;
                txtDescontoPercItem.Enabled = true;
                txtDescontoPercItem.Text = "0,00";
                txtDesconto.Text = "0,00";

                if (!ConfiguracoesECF.pdv)
                {
                    if(sugerirProdutos)
                    {
                        //sugestaoVenda(produto.codigo);
                        Thread t = new Thread(sugestaoVenda);
                        t.Start();
                    }
                }
            }
        }

      
        private void limparDados()
        {

            produto.codigo = "";
            quantidade = 0;
            txtcodProduto.Text = "";
            lblQtd.Text = "1 X";
            preco = 0;
            txtPreco.Text = "";
            txtDescontoPercItem.Text = "";
            txtTotalItem.Text = "";
            pnlCartoes.Visible = false;
            grpPreco.Visible = false;
            //ConfiguracoesECF.CR = "";
            txtcodProduto.Enabled = true;
            txtcodProduto.Visible = true;
            Venda.RetornoTEF = "";
            perguntarCancelamento = true;
            txtcodProduto.Focus();
            txtQtd.Text = "1";
            // pnlImpulsionado.Visible = false;

        }

        private void zerarVariaveis()
        {
            // Crediário e Cheques
            TEF.mensagemOperador = "";
            TEF.valorDescontoTEF = 0;
            TEF.valorSaqueTEF = 0;
            TEF.valorAprovadoTEF = 0;
            Venda.RetornoTEF = "";
            TEF.lstPagamento.Clear();
            clientePDV.LimparCampos();
            clientePDV.idCliente = 0;
            cartoes.idCartao = 0;
            cartoes.txtNrCartao.Text = "";
            cartoes.txtNomeCartao.Text = "";
            lblDescricaoPrd.Text = "";
            lblQtds.Text = "";
            aceitaDesconto = true;
            

            // Financimento
            frmTabelaJuros.valorFinanciamento = 0;
            frmTabelaJuros.valorEntrada = 0;
            frmTabelaJuros.encargos = 0;
            frmTabelaJuros.parcelamento = 1;
            frmTabelaJuros.classe = "0000";
            frmTabelaJuros.financiamentoCalculado = false;
            parcelamentoMaximo = 0;
            FrmDevolucao.totalDevolucao = 0;
            FrmDevolucao.numeroDevolucao = 0;

            dinheiro = 0;
            cartao = 0;
            crediario = 0;
            cheque = 0;
            dpFinanceiro = "Venda";
            encargos = 0;
            taxaServicoIqChef = 0;
            desconto = 0;
            descontoJuros = 0;
            perguntarCancelamento = true;
            valorEntrada = 0;
            IqCard.voucherDesconto = "";            

            FrmExtratoCliente.parcelas.Clear();
            txtDesconto.Text = "0,00";
            txtDinheiro.Text = "0,00";
            txtCartao.Text = "0,00";
            txtCheque.Text = "0,00";
            txtCrediario.Text = "0,00";
            pnlCliente.Visible = false;
            Venda.dadosConsumidor =
                new DadosConsumidorCupom
                {
                    cpfCnpjConsumidor = "",
                    nomeConsumidor = "",
                    endConsumidor = "",
                    endNumero = "",
                    endBairro = "",
                    endCEP = "",
                    endCidade = "",
                    endEstado = "",
                    idConsumidor = ""
                };

            venda.numeroDevolucao = 0;
            venda.totalDevolucao = 0;
            venda.classeVenda = "0000";
            Venda.dadosEntrega = new DadosEntrega();
            Venda.dadosDAVOS = new DadosDAVOS();
            classeVenda = "0000";
            Venda.IQCard = "";
            Venda.idTransacaoIQCARD = "";
            Venda.idPedidoIQCARD = "";

            frmClasseVenda.codigoClasse = "0000";
            lblClasse.Text = "";
            devolucaoNumero = 0;
            totalDevolucao = 0;
            //pnlPagamento.Location = new Point(698, 44);
            dtgItens.Visible = true;
            //this.Enabled = true;
            Funcoes.TravarTeclado(false);
            descontoMax = false;
            btnDescontoMax.BackColor = Color.FromArgb(91, 191, 223);
            txtcodProduto.Focus();
            VerificaConexao();
            MostrarItens();
            pnlCedulas.Visible = false;
            valorSelecionadoCedula = 0;
            if (Venda.ultimoDocumento > 0)
                lblUltDoc.Text = "DOC: " + Venda.ultimoDocumento.ToString();

            FrmExtratoCliente.jurosAntecipados = 0;

            VendedorAssociado();
            txtcodProduto.BackColor = Color.White;
            txtcodProduto.ForeColor = Color.Black;
            txtDescontoPercItem.Text = "";
            lblAviso.Text = "";
            FrmObs.observacao = "";
            qtdCartoesFeitas = 0;
            Venda.dependente = "";
            Venda.dadosCheque = null;
            _pdv.numeroDAV = 0;
            Venda.numeroPED = 0;
            FrmDocumentos.NumeroDocumento = "";
            descontoGerencial = false;
            identificarClienteNFCe = false;
            if (!string.IsNullOrEmpty(Venda.IQCard))
                lblInfoCliente.Text = Venda.IQCard;

            pnDigitacao.Visible = true;

            FuncoesECF.VerificaImpressoraLigada(true, false);
            FuncoesECF.VerificarStatusPapel(true);
            timerAnuncio.Start();
            ativarIndoor = false;
            nvezesAnuncio = 0;

            pnlImpulsionado.Visible = false;
            verificarMensagem();
            listFormaPagamentoAuditoria.Clear();
        }

        private void VendedorAssociado()
        {
            try
            {
                Vendedor.VendedorAssociado(GlbVariaveis.glb_Usuario);
                lblVendedor.Text = Vendedor.codigoVendedor + " - " + Vendedor.nomeVendedor.PadRight(30, ' ').Substring(0, 30);
                lblVendedor2.Text = Vendedor.codigoVendedor + " - " + Vendedor.nomeVendedor.PadRight(30, ' ').Substring(0, 30);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exceção ao obter vendedor associado. Verifique se o vendedor do operador existe no cadastro de vendedores: " + ex.Message);
            }
        }

        private void ExcluirItem(object sender, EventArgs e)
        {
            if (dtgItens.Rows.Count == 0)
                return;

            if (MessageBox.Show("Excluir o item " + Convert.ToString(dtgItens.CurrentRow.Cells["descricao"].Value) + "?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                Funcoes.TravarTeclado(true);
                Venda venda = new Venda();
                if (Conexao.onLine)
                {
                    venda.ExcluirItem(Convert.ToInt32(dtgItens.CurrentRow.Cells["nrcontrole"].Value), Convert.ToInt32(dtgItens.CurrentRow.Cells["id"].Value));
                }
                else
                {
                    venda.ExcluirItem(Convert.ToInt32(dtgItens.CurrentRow.Cells["nrcontrole"].Value), 0);
                }



                MostrarItens();
                

                txtcodProduto.Focus();
                // dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.CurrentRow.Index]);
            }
            catch (Exception erro)
            {
                Funcoes.TravarTeclado(false);
                MessageBox.Show("Não foi possível excluir o item :" + erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                Funcoes.TravarTeclado(false);
                txtcodProduto.Focus();
            }

            txtcodProduto.Focus();
        }

        private void colorirGrid()
        {

            foreach(DataGridViewRow item in dtgItens.Rows)
            {
                if(item.Cells["cancelado"].Value.ToString().ToLower() == "cancelado")
                {
                  item.DefaultCellStyle.ForeColor = Color.Red;
                  item.DefaultCellStyle.Font = new Font(dtgItens.DefaultCellStyle.Font, FontStyle.Strikeout);
                }
            }
        }

        private void PreencheCampo()
        {
            Control[] ctls = this.Controls.Find(controle, true);

            if (ctls[0] is Button)
                return;

            TextBox txtBox = ctls[0] as TextBox;
            if (txtBox.Enabled == false)
                return;

            switch (controle)
            {
                case "txtDinheiro":
                    if (Convert.ToDecimal(txtDinheiro.Text + 0) == restante)
                        txtDinheiro.Text = "";
                    break;
                case "txtValorIndCA":
                    if (Convert.ToDecimal(cartoes.txtValorIndCA.Text + 0) == restante)
                        cartoes.txtValorIndCA.Text = "";
                    break;
                case "txtCrediario":
                    if (Convert.ToDecimal(txtCrediario.Text + 0) == restante)
                        txtCrediario.Text = "";
                    break;
                case "txtCheque":
                    if (Convert.ToDecimal(txtCheque.Text + 0) == restante)
                        txtCheque.Text = "";
                    break;
                case "txtDesconto":
                    if (txtDesconto.Text == "0,00") txtDesconto.Text = "";
                    break;
            }

            if (ctls[0] is TextBox)
            {
                if (txtBox.Text.Trim().Length >= txtBox.MaxLength)
                    return;
                txtBox.Text += this.tecla;
            };
        }

        private void _pdv_Shown(object sender, EventArgs e)
        {
            string logo = "";
            
            /*if (ConfiguracoesECF.styleMetro == false)
                logo = @"imagens\ocultar.png";
            else
                logo = @"imagensMetro\mostrar.png";

            pnlEsconder.BackgroundImage = new Bitmap(logo);*/

            if (ConfiguracoesECF.tecladoVirtual == false)
            {
                /*pnlBotoes.Visible = false;
                pnlTeclado.Visible = false;
                pnlEsconder.BackgroundImage = new Bitmap(logo);
                dtgItens.Height = 360;
                Posicoes();
                pnlEsconder.Location = new Point(0, 690); 
                btTotalizar.Location = new Point(57, 120);*/

                btTotalizar.Location = new Point(57, 350);
                //Configuracoes.capturarResolucao();
                //dtgItens.Height = (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 500);
                dtgItens.Height = (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 290);
                dtgItens.Width = (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 370) < 1110 ? (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 370) : 1110;
                pnlBotoes.Visible = false;
                pnlTeclado.Visible = false;
                Posicoes();
                pnlEsconder.Location = new Point(1, 500);

                if (ConfiguracoesECF.styleMetro == false)
                    logo = @"imagens\ocultar.png";
                else
                    logo = @"imagensMetro\mostrar.png";

                pnlEsconder.BackgroundImage = new Bitmap(logo);
            }
            else
            {
                // Posições
                //dtgItens.Height = 360;
                //Posicoes();

                btTotalizar.Location = new Point(57, 350);
                Configuracoes.capturarResolucao();
                //MessageBox.Show(Configuracoes.resolucaoHeight.ToString() + "|" + Configuracoes.resolucaoWidth.ToString());
                //if()
                dtgItens.Height = (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 500);
                dtgItens.Width = (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 370) < 1110 ? (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 370) : 1110;
                pnlBotoes.Visible = true;
                pnlTeclado.Visible = true;

                //pnlBotoes.Visible = true;
                //pnlTeclado.Visible = true;
                //dtgItens.Height = (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 290);
                Posicoes();
                pnlEsconder.Location = new Point(680, (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 190));  // 630
                //pnlEsconder.Location = new Point(1, 680);
                //btTotalizar.Location = new Point(57, 120);
                //pnlEsconder.Location = new Point(680, (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 190));  // 630

                if (ConfiguracoesECF.styleMetro == false)
                    logo = @"imagens\mostrar.png";
                else
                    logo = @"imagensMetro\ocultar.png";

                pnlEsconder.BackgroundImage = new Bitmap(logo);
            }
            

            MostrarItens();
            if (!FuncoesECF.CupomFiscalAberto() && dtgItens.Rows.Count > 0 && FuncoesECF.VerificaImpressoraLigada() && ConfiguracoesECF.idECF > 0 && ConfiguracoesECF.NFC == false && ConfiguracoesECF.idECF != 9999)
            {
                DialogResult resultado = MessageBox.Show("Existem itens neste terminal. Deseja apagar os itens e começar uma nova venda?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.OK)
                {
                    Venda.ApagarItensFormaPagamento("Itens");
                }
                MostrarItens();
            }

            if (!FuncoesECF.CupomFiscalAberto() && dtgItens.Rows.Count > 0 && ConfiguracoesECF.idECF == 0 && ConfiguracoesECF.pdv == true)
            {
                Venda.ApagarItensFormaPagamento("Itens");
                MostrarItens();
            }

            txtcodProduto.Focus();

            if (ConfiguracoesECF.idECF > 0 && FuncoesECF.ZPendente())
            {
                lblAlerta.BackColor = Color.Red;
                ConfiguracoesECF.zPendente = true;
                lblAlerta.Text = "Redução Z Pendente";
                MessageBox.Show("Redução Z Pendente. Encerre o caixa do movimento anterior para realizar venda", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ChamarCaixa();
                txtcodProduto.Enabled = false;
                btnSangria.Enabled = false;
                btnSuprimento.Enabled = false;
            }
            else if (ConfiguracoesECF.idECF > 0 && ConfiguracoesECF.caixaPendente == true)
            {
                lblAlerta.BackColor = Color.Red;
                lblAlerta.Text = "Caixa Anteriror Pendente";
                MessageBox.Show("Caixa Pendente. Encerre o caixa do movimento anterior para realizar venda", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ChamarCaixa();
                txtcodProduto.Enabled = false;
                btnSangria.Enabled = false;
                btnSuprimento.Enabled = false;
            }

            if (ConfiguracoesECF.idECF > 0 && FuncoesECF.VerificaReducaZDia())
            {
                ConfiguracoesECF.reducaoZEmitida = true;
                lblAlerta.BackColor = Color.Red;
                ConfiguracoesECF.idECF = 0;
                lblAlerta.Text = "Redução Z já emitida.";
                btnSangria.Enabled = false;
                btnSuprimento.Enabled = false;
            }

            statusbntCancelar();
            verificarVersao();
            //mensagemExportacaoFiscal();

        }

        private void statusbntCancelar()
        {
            if ((ConfiguracoesECF.NFC == true && dtgItens.RowCount == 0 && ConfiguracoesECF.davporImpressoraNaoFiscal == false) || ConfiguracoesECF.idECF == 9999)
            {
                btnCancelarCupom.Text = "RELAÇÃO DOCUMENTOS";
            }
            else if ((ConfiguracoesECF.NFC == true && dtgItens.RowCount > 0 && ConfiguracoesECF.davporImpressoraNaoFiscal == false) || ConfiguracoesECF.idECF == 9999)
            {
                btnCancelarCupom.Text = "CANCELAR DOCUMENTOS";
            }
            else if (ConfiguracoesECF.davporImpressoraNaoFiscal == true)
            {
                btnCancelarCupom.Text = "EXCLUIR ITENS";
            }
        }

        private void Posicoes()
        {
            lblInfoCliente.Location = new Point(13, dtgItens.Size.Height + dtgItens.Location.Y + 2);
            pnlRodapeItens.Location = new Point(21, lblInfoCliente.Size.Height + lblInfoCliente.Location.Y + 2);
            pnlRodapeItens.Width = (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 370) < 1110 ? (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 370) : 1110;
            pnlSoma.Location = new Point((int.Parse(Configuracoes.resolucaoWidth.ToString()) - 822) <= 618 ? (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 822) : 670, 6);
            pnlBotoes.Location = new Point(21, (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 270));
            pnFinalizacao.Location = new Point((int.Parse(Configuracoes.resolucaoWidth.ToString()) - 290), (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 470));
            picLogo.Location = new Point((int.Parse(Configuracoes.resolucaoWidth.ToString()) - 270), (dtgItens.Location.Y - 3));
            pnlImpulsionado.Location = new Point((int.Parse(Configuracoes.resolucaoWidth.ToString()) - 270), (dtgItens.Location.Y - 70));
            pnlPagamento.Location = new Point((int.Parse(Configuracoes.resolucaoWidth.ToString()) - 300), (dtgItens.Location.Y - 3));

            int metadeDaTela = 0;
            metadeDaTela = (Screen.PrimaryScreen.Bounds.Width / 2);
            if(metadeDaTela < 600)
            {
                indoor.Visible = false;
                pnlImpulsionado.Location = new Point((int.Parse(Configuracoes.resolucaoWidth.ToString()) - 270), (dtgItens.Location.Y - 125));
                pnlImpulsionado.Height = 250;
                lblTituloProdutoSugerido.Font = new Font("Arial", 14, FontStyle.Bold);
                // pnlImpulsionado.Location = new Point((int.Parse(Configuracoes.resolucaoWidth.ToString()) - 270), (dtgItens.Location.Y - 50));
            }
            //pnlEsconder.Location = new Point(-7, grpBotoes.Location.Y+12);
        }

        private void MostrarItens()
        {
            Venda venda = new Venda();
            /// If operandus modus = StandAlone
            #region StandAlone
            if (!Conexao.onLine && Conexao.tipoConexao == 1)
            {
                IObjectContainer tabela = Db4oFactory.OpenFile("vendas.yap");
                dtgItens.DataSource = (from StandAloneVenda p in tabela
                                       where p.ip == GlbVariaveis.glb_IP
                                       orderby p.nrcontrole
                                       select new
                                       {
                                           nrcontrole = p.nrcontrole,
                                           codigo = p.codigo,
                                           descricao = p.descricao,
                                           unidade = p.unidade,
                                           quantidade = p.quantidade,
                                           preco = p.preco,
                                           total = p.total,
                                           descontovalor = p.descontovalor,
                                           icms = p.icms,
                                           descontoperc = p.descontoperc,
                                           id = p.id,
                                           situacaoItem = p.cancelado == "S" ? "Cancelado" : " "
                                       }).ToList();
                tabela.Close();
            }
            #endregion
            else
            {
                siceEntities entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                dtgItens.DataSource = from p in entidade.vendas
                                      where p.id == GlbVariaveis.glb_IP
                                      && p.codigofilial == GlbVariaveis.glb_filial
                                      orderby p.nrcontrole
                                      select new
                                      {
                                          nrcontrole = p.nrcontrole,
                                          codigo = p.codigo,
                                          descricao = p.aentregar == "S" ? "(E) " + p.produto : "" + p.produto,
                                          situacaoItem = p.cancelado == "S" ? "Cancelado" : " ",
                                          unidade = p.unidade,
                                          quantidade = p.quantidade,
                                          preco = p.preco,
                                          descontovalor = p.descontovalor,
                                          descontoperc = p.Descontoperc,
                                          total = p.total,
                                          icms = p.icms,
                                          id = p.inc,
                                          atacado = p.vendaatacado
                                      };

            };

            dtgItens.Focus();
            totalTransacao = venda.SomaItens();
            txtTotal.Text = String.Format("{0:n2}", totalTransacao);


            if (dtgItens.Rows.Count > 0)
            {
                dtgItens.Visible = true;
                pnlRodapeItens.Visible = true;
                dtgItens.CurrentCell = dtgItens.Rows[dtgItens.RowCount - 1].Cells[0];
                pnlMensagem.SendToBack();
            }

            //if  (totalTransacao == 0)            
            if (dtgItens.Rows.Count == 0)
            {
                if (dtgItens.Rows.Count == 0)
                {
                    lblDescricaoPrd.Text = "";
                    lblQtds.Text = "";
                }

                pnlMensagem.BringToFront();
                dtgItens.Visible = false;
                pnlRodapeItens.Visible = false;
                lblMsgPDV.Text = Configuracoes.mensagemPDV;
                if (string.IsNullOrEmpty(Configuracoes.mensagemPDV))
                    lblMsgPDV.Text = "Seja bem-vindo";
            }
            colorirGrid();
            txtcodProduto.Focus();
        }

        private void btProximo_Click(object sender, EventArgs e)
        {
            if (dtgItens.Rows.Count == 0 || dtgItens.CurrentRow.Index + 1 >= dtgItens.Rows.Count)
            {
                return;
            };
            dtgItens.CurrentCell = dtgItens.Rows[dtgItens.CurrentRow.Index + 1].Cells[0];
        }

        private void btAnterior_Click(object sender, EventArgs e)
        {
            if (dtgItens.Rows.Count == 0 || dtgItens.CurrentRow.Index - 1 <= -1)
            {
                return;
            };
            dtgItens.CurrentCell = dtgItens.Rows[dtgItens.CurrentRow.Index - 1].Cells[0];
        }

        private void btCancelarPg_Click(object sender, EventArgs e)
        {
            AbortarPagamento();
        }

        private void AbortarPagamento()
        {
            try
            {
                devolucaoNumero = 0;
                totalDevolucao = 0;
                pnlPagamento.Visible = false;
                pnlCliente.Visible = false;
                txtDinheiro.Text = "0.00";
                txtDesconto.Text = "0.00";
                desconto = 0;
                descontoJuros = 0;
                AtivarBotoes();
                txtcodProduto.Focus();
                pnlCartoes.Visible = false;
                dtgItens.Visible = true;
                pnlCedulas.Visible = false;
                valorSelecionadoCedula = 0;
                listFormaPagamentoAuditoria.Clear();

                if (dpFinanceiro == "Recebimento")
                {
                    //pnlPagamento.Location = new Point(461, 35);
                    lblDescricaoPrd.Text = "";
                    lblQtds.Text = " ";
                    dpFinanceiro = "Venda";
                    venda.numeroDevolucao = 0;
                    venda.totalDevolucao = 0;
                    dtgItens.Visible = false;
                    return;
                }

                /*if (!FuncoesECF.CupomFiscalAberto() || _pdv.numeroPreVenda > 0 || _pdv.numeroDAV > 0) alterado por marckvaldo wallas
                    CancelarCupom();
                Venda.GravarGtECF();*/


            }
            catch
            {
                return;
            }
            finally
            {
                /*numeroDAV = 0;
                numeroPreVenda = 0;*/
                if (numeroDAV > 0 && ConfiguracoesECF.pdv == true)
                    txtcodProduto.Enabled = false;

            }
        }

        private void DesativarBotoes()
        {
            timeHora.Enabled = false;
            pnlBotoes.Enabled = false;
            txtcodProduto.Enabled = false;
            btnExcluir.Enabled = false;
            btTotalizar.Enabled = false;
            btnCliente.Enabled = false;
        }

        private void AtivarBotoes()
        {
            pnlBotoes.Enabled = true;
            timeHora.Enabled = true;
            txtcodProduto.Enabled = true;
            btnExcluir.Enabled = true;
            txtDesconto.Enabled = true;
            btDesconto.Enabled = true;
            pnlPagamento.Visible = false;
            btTotalizar.Enabled = true;
            btnCliente.Enabled = true;
            btDesconto.Text = "%";

            MostrarItens();
        }

        private void Totalizar(bool tabelaJuros = false)
        {

            //if(tabelaJuros == true)
            desconto = descontoJuros + desconto;

            if ((Venda.dadosEntrega.recebedor == "" || Venda.dadosEntrega.recebedor == null) && Venda.quantidadeAentregar(GlbVariaveis.glb_IP) > 0)
            {
                chamardadosEntrega();
            }

            if ((GlbVariaveis.glb_clienteDAV == true && ConfiguracoesECF.pdv == false && ConfiguracoesECF.prevenda == false && ConfiguracoesECF.idNFC == 0) || (Configuracoes.limitevendasemidentificacao < totalTransacao && dpFinanceiro == "Venda"))
            {
                if ((Venda.dadosConsumidor.cpfCnpjConsumidor == "" || Venda.dadosConsumidor.cpfCnpjConsumidor == null || Venda.dadosConsumidor.cpfCnpjConsumidor == "0") &&
                    (Venda.dadosConsumidor.ecfCNPJCPFConsumidor == "" || Venda.dadosConsumidor.ecfCNPJCPFConsumidor == null || Venda.dadosConsumidor.ecfCNPJCPFConsumidor == "0"))
                {
                    if (ConfiguracoesECF.pdv == false)
                    {
                        MessageBox.Show("É necessario identificar o cliente antes de encerrar o DAV!");
                        MostrarConsumidor();
                    }
                    else
                    {
                        MessageBox.Show("É necessario identificar o cliente antes de encerrar a Venda!");
                        MostrarConsumidor();
                    }
                    return;
                }
            }

            if ((clientePDV.idCliente > 0 && dpFinanceiro == "Venda") || (Venda.dadosConsumidor.ecfCNPJCPFConsumidor != "" && Venda.dadosConsumidor.ecfCNPJCPFConsumidor != null && clientePDV.idCliente == 0 && dpFinanceiro == "Venda"))
            {
                try
                {
                    FuncoesNFC NFC = new FuncoesNFC();
                    if (ConfiguracoesECF.NFC == true && clientePDV.idCliente > 0)
                        NFC.validaCliente(clientePDV.idCliente);

                    identificarClienteNFCe = true;

                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    identificarClienteNFCe = false;
                    clientePDV.idCliente = 0;
                    clientePDV.Cancelar();
                    lblInfoCliente.Text = "";
                    Application.DoEvents();
                    return;
                }
            }

            FuncoesPAFECF.LerMD5();
            if (ConfiguracoesECF.pdv && ConfiguracoesECF.idECF == 0 && ConfiguracoesECF.NFC == false)
            {
                MessageBox.Show("ECF não está conectado!");
                return;
            }

            //FuncoesNFC.verificarGerenciadorNFCe();


            try
            {
                Venda.ExcluirPagamento(""); // Exclui do caixas as formas de pagamento do terminal
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                limparDados();
                return;
            }


            if ((Conexao.onLine && Configuracoes.vendaPorclasse == true && (classeVenda == "0000" || string.IsNullOrEmpty(classeVenda)) && dpFinanceiro == "Venda"))
            {
                MessageBox.Show("Configuração aceita somente venda com classificação. Escolha uma classificação.", "SICEpdv.net", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChamarClasse();
                if ((Configuracoes.vendaPorclasse == true && (classeVenda == "0000" || string.IsNullOrEmpty(classeVenda)) && dpFinanceiro == "Venda"))
                    return;
            }



            // Tolerância de 18 centavos
            if (totalDevolucao - 0.18M > totalTransacao)
            {
                MessageBox.Show("Total da devolução de produto maior que a compra", "SICEpdv.net", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pnlCliente.Visible = false;
                AtivarBotoes();
                dpFinanceiro = "Venda";
                return;
            }

            if (totalTransacao == 0 && ConfiguracoesECF.idECF != 0) return;
            /// Ativar Campos antes de iniciar a totalização
            ///            

            if (totalTransacao == 0 && ConfiguracoesECF.idECF == 0 && dtgItens.RowCount > 0)
            {
                if (venda.Finalizar(false, false, false, false) > 0)
                {
                    // A venda foi concluida e o cartao foi zerado
                    Venda.IQCard = "";
                }


                zerarVariaveis();
                return;
            }

            // Operador Digitar Senha na Finalizacao Pre-Venda. 
            // Se for dav ou prevenda pega o vendedor das prevendas.
            if (Configuracoes.digitarSenhaFinalizacaoPreVenda && dpFinanceiro == "Venda" && Conexao.onLine && numeroDAV == 0 && numeroPreVenda == 0)
            {
                FrmLogon logon = new FrmLogon();
                logon.campo = "operador";
                logon.txtDescricao.Text = "Vendedor correspondente";
                logon.ShowDialog();
                if (Operador.autorizado)
                {
                    Vendedor.VendedorAssociado(Operador.ultimoOperadorAutorizado);
                }
                else
                {
                    return;
                }
                if (Venda.vendedor == "000")
                {
                    MessageBox.Show("Não existe um vendedor associado ao operador", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                };
            }

            if (dpFinanceiro == "Venda" && Configuracoes.vendasemprecomvendedor == true && (string.IsNullOrEmpty(Venda.vendedor) || Venda.vendedor == "000"))
            {
                MessageBox.Show("Por favor escolha um vendedor.");

                ChamarVendedor();
                if (Venda.vendedor == "000")
                    return;
            }

            lblAviso.Text = "";
            pnlBotoes.Enabled = false;
            txtDinheiro.Enabled = true;
            txtCartao.Enabled = true;
            txtCrediario.Enabled = true;
            txtCheque.Enabled = true;
            btTotalizar.Enabled = false;
            pnlPagamento.Enabled = true;
            pnlPagamento.Visible = true;
            pnlImpulsionado.Visible = false;

            totalTransacao += (encargos + taxaServicoIqChef);
            ///Zerar Variavéis antes da totalização
            restante = totalTransacao - desconto;
            dinheiro = 0;
            cartao = 0;
            crediario = 0;
            cheque = 0;
            vendaAV = 0;
            txtDinheiro.Text = "0,00";
            txtCartao.Text = "0,00";
            txtCrediario.Text = "0,00";
            DesativarBotoes();
            AtivaDesconto();

            clientePDV.vencimentoCR.Value = GlbVariaveis.Sys_Data.AddMonths(1);
            clientePDV.vencimentoCH.Value = GlbVariaveis.Sys_Data;
            txtDinheiro.Text = string.Format("{0:n2}", restante);

            if (dpFinanceiro == "Recebimento")
            {
                txtCrediario.Enabled = false;
                //txtCartao.Enabled = false;
            }

            if (dpFinanceiro == "Venda")
            {
                VendaAV();
            }


            if (aceitaDesconto == false)
                txtDesconto.Enabled = false;

            // Aqui por que se o desconto estiver desativado näo 
            // processa o pagamento das devoluções
            if (txtDesconto.Enabled == false)
                DevolucaoPagamento();

            if (!Conexao.onLine)
            {
                txtCrediario.Enabled = false;
                txtCartao.Enabled = false;
                txtCheque.Enabled = false;
            }

            if (encargos > 0 || desconto > 0)
            {
                txtDesconto.Enabled = false;
                txtDesconto.Text = string.Format("{0:N2}", desconto);
                DesativaDesconto();
                txtDinheiro.Focus();
            }


            //venda.ImprimirComprovanteEntrega(venda.documento);

            //VerificaConexao();

            //txtcodProduto.Enabled = true;

        }


        private void verificarSangria()
        {

            // Mensagem da sangria
            if (Configuracoes.alertaSangria > 0)
            {


                decimal somatorioCaixaLiquido = (from i in Conexao.CriarEntidade().caixa
                                                 where i.CodigoFilial == GlbVariaveis.glb_filial &&
                                                 i.EnderecoIP == GlbVariaveis.glb_IP &&
                                                 i.data == DateTime.Today &&
                                                 (i.tipopagamento == "SI" || i.tipopagamento == "DH")
                                                 && i.estornado == "N"
                                                 select i.valor).Sum();


                decimal somatorioSangria = (from i in Conexao.CriarEntidade().movdespesas
                                            where i.codigofilial == GlbVariaveis.glb_filial &&
                                            i.id == GlbVariaveis.glb_IP && i.data == DateTime.Today &&
                                            i.sangria == "S"
                                            select i.valor).Sum();





                if (Configuracoes.alertaSangria <= (somatorioCaixaLiquido - somatorioSangria))
                {
                    MessageBox.Show("O valor do saldo líquido atingiu o limite, por favor realize uma sangria! \n Valor limite: R$" + Configuracoes.alertaSangria +
                                    "\n Valor Líquido (DH): R$ " + (somatorioCaixaLiquido - somatorioSangria), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }


        }

        private void VerificaConexao()
        {

            if (ConfiguracoesECF.pdv)
            {
                lblModo.Text = "PDV";
                Conexao.VerificaConexaoDB();
            }

            if (ConfiguracoesECF.davporImpressoraNaoFiscal || ConfiguracoesECF.davPorECF)
                lblModo.Text = "DAV";
            if (ConfiguracoesECF.prevenda)
                lblModo.Text = "PRE";


            #region StandAlone
            if (Conexao.tipoConexao == 1)
            {
                if (Conexao.onLine && StandAlone.QuantidadeRegistro() > 0)
                {
                    lblAlerta.Text = "dados p/sincronização";
                    //MessageBox.Show("Conexão ativa e existem dados local para sincronização!");                
                }

                if (!Conexao.onLine)
                {
                    lblStatus.Text = "Off-line";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Font = new System.Drawing.Font("Arial", 12);
                    txtCrediario.Enabled = false;
                    btCancelaCR.Enabled = false;

                    txtCartao.Enabled = false;

                    txtCheque.Enabled = false;
                    btCancelaCH.Enabled = false;
                    btnSangria.Enabled = false;
                    btnSuprimento.Enabled = false;
                    btnEntrega.Enabled = false;
                    btnVendedor.Enabled = false;
                    btnCliente.Enabled = false;
                    btnClasse.Enabled = false;
                    btnTabelajuros.Enabled = false;

                }

                if (Conexao.onLine)
                {
                    lblStatus.Text = "ON-LINE";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Font = new System.Drawing.Font("Arial", 12);
                    txtCrediario.Enabled = true;
                    btCancelaCR.Enabled = true;

                    txtCartao.Enabled = true;

                    txtCheque.Enabled = true;
                    btCancelaCH.Enabled = true;
                    btnSangria.Enabled = true;
                    btnSuprimento.Enabled = true;
                    btnEntrega.Enabled = true;
                    btnVendedor.Enabled = true;
                    btnCliente.Enabled = true;
                }
            }
            else
            {
                lblAlerta.Text = "";

                if (StandAlone.quantidadeDocumentoSincronizar() > 0)
                {
                    lblAlerta.Text = "dados p/sincronização";
                    //MessageBox.Show("Conexão ativa e existem dados local para sincronização!");                
                }


                if (FuncoesECF.ZPendente() && ConfiguracoesECF.idECF > 0)
                {
                    lblAlerta.Text = "Redução Z Pendente";
                }


                try
                {
                    var filial = (from f in Conexao.CriarEntidade().filiais
                                  where f.CodigoFilial == GlbVariaveis.glb_filial
                                  select f.CodigoFilial).FirstOrDefault();

                    lblStatus.Text = "ON-LINE";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Font = new System.Drawing.Font("Arial", 12);

                }
                catch (Exception)
                {
                    lblStatus.Text = "Off-line";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Font = new System.Drawing.Font("Arial", 12);
                }

            }

            #endregion
        }

        private void AtivaDesconto()
        {
            if (desconto > 0)
            {
                return;
            }


            txtDesconto.Text = "0,00";
            tipoDesconto = "%";
            btDesconto.Enabled = true;
            txtDesconto.Enabled = true;
            txtDesconto.Focus();
            if (dpFinanceiro == "Venda")
            {
                if (!Permissoes.descontoFinalizacao) DesativaDesconto();
            }

            // Para dar o desconto se usar o desconto de fidelização

            if (!string.IsNullOrEmpty(Venda.IQCard) && desconto == 0)
            {
                txtDesconto.Text = string.Format("{0:N2}", Configuracoes.descontoCartaoFidelidade);
            }
        }

        private void _pdv_Load(object sender, EventArgs e)
        {

            Configuracoes.dataAberturaCaixa(GlbVariaveis.glb_filial, GlbVariaveis.glb_Usuario);


            //string codPromocao = IqCard.GerarCodigoPromocao();
            //MessageBox.Show(codPromocao);
            lblAviso.Text = "";
            this.Text = "®SICEpdv.net - " + GlbVariaveis.glb_Versao + "   " + GlbVariaveis.glb_filial + " " + Configuracoes.fantasia;
            lblUltDoc.Text = "";
            CarregarLogo();
            int diasRestante = 0;
            DateTime validade;

            // Estava Desativado por que não se podia travar o PDV no PAF. 
            try
            {
                validade = Conexao.CriarEntidade().ExecuteStoreQuery<DateTime>("SELECT validade from iqsistemas").FirstOrDefault();
                diasRestante = Convert.ToInt16(validade.Subtract(DateTime.Now.Date).Days);
                string mensagem = "";
                //if (!Funcoes.VerificarLicencaUso(out diasRestante, out mensagem, "SICE.net"))
                //{
                    //MessageBox.Show("Licença de uso expirada");
                    //PagamentoOnLine.diasRestante = diasRestante;
                    //PagamentoOnLine pagamento = new PagamentoOnLine();
                    //pagamento.ShowDialog();
                    //return;
                //}                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }

            // Essa funcao ativa o pagamento caso os dias restante seja menos que 5
            // Está dentro de catcho pra nao interromper

            try
            {
                siceEntities entidade = Conexao.CriarEntidade();

                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                DateTime dataServidor = Funcoes.GetDataServidor();
                                               
                if (diasRestante < 4)
                {
                    PagamentoOnLine.diasRestante = diasRestante;
                    PagamentoOnLine pagamento = new PagamentoOnLine();
                    pagamento.ShowDialog();
                }

            }
            catch (Exception)
            {
                
            }
           



            if (GlbVariaveis.glb_IP == "" || GlbVariaveis.glb_IP == null)
            {
                MessageBox.Show("Não foi possível obter o ID do Terminal");
                Application.Exit();
            }
            // Iniciando o construtor de dados do consumidor
            Venda.dadosConsumidor =
                new DadosConsumidorCupom
                {
                    cpfCnpjConsumidor = "",
                    nomeConsumidor = "",
                    endConsumidor = "",
                    endNumero = "",
                    endBairro = "",
                    endCEP = "",
                    endCidade = "",
                    endEstado = "",
                    idConsumidor = ""
                };


            FuncoesECF fecf = new FuncoesECF();


            fecf.VerificarStatusTEF();
            // Acrescentado para limpar as transacoes 
            // Pois foi verificado já o status e apos o sistema ser encerrado
            // nao fica nenhuma transação passível de continuidade ou encerramento. 
            // A funcao EncerraTEF já mostra as mensagem.
            // As 3 funcoes seguintes foram colcoa em 07.11.2011 após osclientes 
            // esperaca e mercantil terem ficados com transacoes confirmadas e nao apagados de um 
            // dia para o outro
            if (TEF.Transacoes("ntransacao") > 0 || TEF.Transacoes("ntransacaoConfirmadas") > 0)
            {
                TEF.EncerraTEF();
                TEF.Transacoes("limpar");
            }
            try
            {
                VerificarPromocaoAtiva();
            }
            catch (Exception)
            {
                
            }
            

            // Verifica se existe comunicação com o BD 
            VerificaConexao();
            VendedorAssociado();

            lblDataHora.Text = string.Format("{0: dd/MM/yyyy hh:mm}", DateTime.Now);
            lblOperador.Text = GlbVariaveis.glb_Usuario.ToUpper();
            lblHorasAtual.Text = DateTime.Now.ToShortTimeString();

            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;

            DateTime data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            lblDataAtual.Text = dtfi.GetDayName(data.DayOfWeek) + Environment.NewLine + DateTime.Now.Day + " de " + dtfi.GetMonthName(DateTime.Now.Month);

            //? VerificarGaveta();
            VerificaFuncoesFiscais();


            /// Construindo o Teclado
            /// 
            teclado.TabStop = false;
            pnlTeclado.Controls.Add(teclado);
            this.teclado.clickBotao += new TeclaNumerico.ClicarNoBotao(DelegateTeclado);

            ///Contruindo o Painel para mostrar os Cartoes
            ///            
            pnlCartoes.Location = new System.Drawing.Point(120, 180);
            pnlCartoes.Visible = false;
            pnlCartoes.Width = cartoes.Width;
            pnlCartoes.Height = cartoes.Height;
            this.Controls.Add(pnlCartoes);
            pnlCartoes.Controls.Add(cartoes);
            pnlCartoes.BringToFront();
            this.cartoes.EntraControle += new UCCartoes.Controle(DelegateCartoes);

            /// Construindo o Panel para mostrar o Consumidor / Cliente
            ///          
            // clientePDV.pnlCheques.Visible = false;                        
            pnlCliente.Location = new System.Drawing.Point(90, 180);
            clientePDV.Location = new System.Drawing.Point(0, 0);
            clientePDV.Width = 600;
            clientePDV.Height = 450;
            pnlCliente.Height = 450;
            pnlCliente.Visible = false;
            pnlCliente.Controls.Add(clientePDV);
            pnlCliente.BringToFront();
            pnlCliente.Visible = false;
            pnlCliente.BringToFront();
            this.Controls.Add(pnlCliente);
            pnlCedulas.Visible = false;
            this.Controls.Add(pnlCedulas);
            this.cedulas.clickBotao += new UCMoedas.ClicarNoBotao(ValorCedula);

            this.clientePDV.EntraControle += new ucClientePdv.Controle(DelegateCliente);

            verificaDadosR02();
            timerAnuncio.Start();

            //try
            //{
            //    // Aqui verifica se há procura de promoções
            //    contadorProcuraPromocao.RunWorkerAsync();
            //}
            //catch (Exception)
            //{             
            //}

        }

        private void VerificarPromocaoAtiva()
        {
            try
            {
                pnlProcura.Visible = false;
                if (Configuracoes.promocaoIQCardAtiva)
                {
                    imgAnuncio.Image = Properties.Resources.promocoes_enable1;
                    // lblMarketing.Text = "";
                    // lblAnunciar.Text = "ANÚNCIO ATIVO";

                }
                else
                {
                    imgAnuncio.Image = Properties.Resources.social;
                    lblMarketing.Text = "Ativar marketing digital";
                    // lblAnunciar.Text = "MARKETING DIGITAL";
                    if (IqCard.usuariosProcurandoPromo > 0)
                    {                        
                        // pnlProcura.Visible = true;
                        lblUsersOnline.Text = IqCard.usuariosProcurandoPromo.ToString();
                    }

                }

            }
            catch (Exception)
            {

            }

        }

        private void CarregarLogo()
        {

            if (ConfiguracoesECF.styleMetro == false)
            {
                try
                {
                    #region
                    if (File.Exists(@"imagens\logo.png"))
                    {
                        string logo = @"imagens\logo.png";
                        picLogo.Image = new Bitmap(logo);
                    }

                    if (File.Exists(@"imagens\background.jpg"))
                    {
                        BackgroundImage = new Bitmap(@"imagens\background.jpg");
                    }

                    this.SuspendLayout();
                    this.ResumeLayout();
                    //picLogo.Location = new Point(720, 78);
                    btnAnterior.BackgroundImage = new Bitmap(@"imagens\acima.png");
                    btnProximo.BackgroundImage = new Bitmap(@"imagens\abaixo.png");
                    btnExcluir.BackgroundImage = new Bitmap(@"imagens\menos.png");

                    //pnDigitacao.BackgroundImage = null;

                    pnFinalizacao.Location = new Point(pnFinalizacao.Location.X, pnFinalizacao.Location.Y - 30);
                    //txtcodProduto.BorderStyle = BorderStyle.FixedSingle;
                    pnlPagamento.Location = new Point(pnlPagamento.Location.X, pnlPagamento.Location.Y - 30);

                    //pnFinalizacao.Location = new Point(0, 0);
                    btnAnterior.Size = new Size(65, 38);
                    //btnAnterior.Location = new Point(btnAnterior.Location.X + 30, btnAnterior.Location.Y);
                    btnProximo.Size = new Size(65, 38);
                    btnProximo.Location = new Point(btnProximo.Location.X + 15, btnProximo.Location.Y);
                    btnExcluir.Size = new Size(65, 38);
                    btnExcluir.Location = new Point(btnExcluir.Location.X + 30, btnExcluir.Location.Y);
                    #endregion


                    

                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }
            }
            else
            {
                #region
                if (File.Exists(@"imagensMetro\logo.png"))
                {
                    string logo = @"imagensMetro\logo.png";
                    picLogo.Image = new Bitmap(logo);
                }
                if (File.Exists(@"imagensMetro\background.jpg"))
                {
                    BackgroundImage = new Bitmap(@"imagensMetro\background.jpg");
                }
                this.SuspendLayout();
                this.ResumeLayout();

                btnAnterior.BackgroundImage = new Bitmap(@"imagensMetro\acima.png");
                btnProximo.BackgroundImage = new Bitmap(@"imagensMetro\abaixo.png");
                btnExcluir.BackgroundImage = new Bitmap(@"imagensMetro\menos.png");
                #endregion
            }

        }

        private void VerificaFuncoesFiscais()
        {
            if (!ConfiguracoesECF.pdv)
                digitarQuantidade = true;

            btnSuprimento.Enabled = true;
            btnSangria.Enabled = true;
            btnAdmTef.Enabled = true;
            btnPreVenda.Enabled = true;
            btnDAV.Enabled = true;
            btnFiscal.Enabled = true;
            btnCancelarCupom.Enabled = true;
            btnEncerrar.Enabled = true;

            if (ConfiguracoesECF.zPendente == true)
            {
                txtcodProduto.Enabled = false;
            }

            if (ConfiguracoesECF.idECF == 0 || ConfiguracoesECF.prevenda || ConfiguracoesECF.davporImpressoraNaoFiscal || FuncoesECF.VerificaReducaZDia())
            {
                //btnCliente.Enabled = false;
                btnSuprimento.Enabled = false;
                btnSangria.Enabled = false;
                btnAdmTef.Enabled = false;
                btnPreVenda.Enabled = false;
                //btnDAV.Enabled = false;
                btnFiscal.Enabled = false;
                btnCancelarCupom.Enabled = false;

                if (ConfiguracoesECF.davporImpressoraNaoFiscal == true)
                {
                    btnCancelarCupom.Enabled = true;
                }

                btnEncerrar.Enabled = false;
                txtcodProduto.Enabled = true;
            }
            // Colocado por que näao temos homogacao tef dedicado ainda


            if (ConfiguracoesECF.pdv)
            {
                if (!ConfiguracoesECF.tefDiscado)
                {
                    txtCartao.Enabled = false;
                }
            };
        }

        void DelegateCliente(string crControle)
        {
            controle = crControle;
            switch (controle)
            {
                case "btConfirmarCR":
                case "btConfirmarCH":
                    if (clientePDV.tipoPagamento == "")
                    {
                        pnlCliente.Visible = false;
                        AtivarBotoes();
                        return;
                    }
                    else
                    {
                        if (clientePDV.idCliente == 0)
                            return;
                    }

                    TeclaEnter();
                    break;
                case "btSairCR":
                    pnlPagamento.Enabled = true;
                    pnlImpulsionado.Visible = false;
                    pnlCliente.Visible = false;
                    if (string.IsNullOrEmpty(clientePDV.txtIdCliente.Text))
                    {
                        clientePDV.Cancelar();
                        txtcodProduto.Focus();
                    }

                    if (clientePDV.tipoPagamento == "") AtivarBotoes();

                    if (clientePDV.tipoPagamento == "CR")
                    {
                        CancelarPagamento("CR");
                        txtCrediario.Focus();
                    }

                    if (clientePDV.tipoPagamento == "CH")
                    {
                        CancelarPagamento("CH");
                        txtCheque.Focus();
                    }
                    break;
                case "btnExtrato":
                    FrmExtratoCliente frmExtrato = new FrmExtratoCliente();
                    if (clientePDV.txtIdCliente.Text == "")
                        break;
                    FrmExtratoCliente.jurosAntecipados = 0;
                    FrmExtratoCliente.codigoCliente = clientePDV.idCliente; // Convert.ToInt32("0" + clientePDV.txtIdCliente.Text);
                    frmExtrato.ShowDialog();
                    if (FrmExtratoCliente.parcelas.Count > 0)
                    {
                        ChamarRecebimento();
                    }
                    break;
            }
        }

        private void ChamarRecebimento()
        {
            FuncoesECF.ComprovanteNaoFiscal("cancelar", "RECEBIMENTO", "", "", "", "", "", "", "");

            if (FuncoesECF.CupomFiscalAberto())
            {
                MessageBox.Show("Venda em processo. Finalize primeiro para somente depois " +
                    "efetuar recebimento");
                return;
            }


            //MessageBox.Show(ConfiguracoesECF.idECF.ToString());

            if (ConfiguracoesECF.idECF == 0)
            {
                MessageBox.Show("ECF desativado");
                return;
            }

            pnlAviso.Visible = false;
            lblDescricaoPrd.Text = "Recebimento de conta";
            lblMsgPDV.Text = clientePDV.nomeCliente.PadRight(25, ' ').Substring(0, 25);
            dtgItens.Visible = false;
            //pnlPagamento.Location = new Point(127, 78);
            pnlCliente.Visible = false;

            totalTransacao = (from n in FrmExtratoCliente.parcelas
                              select n.valorPagamento).Sum();

            dpFinanceiro = "Recebimento";

            if (FrmDevolucao.numeroDevolucao > 0)
            {
                devolucaoNumero = FrmDevolucao.numeroDevolucao;
                totalDevolucao = FrmDevolucao.totalDevolucao;
                venda.numeroDevolucao = devolucaoNumero;
                venda.totalDevolucao = totalDevolucao;
            }
            desconto = FrmExtratoCliente.jurosAntecipados;
            Totalizar();
        }

        void DelegateCartoes(string caControle)
        {
            controle = caControle;
            switch (controle)
            {
                case "btConfirmaCA":
                    TeclaEnter();
                    break;
                case "btSairCA":
                    CancelarPagamento("CA");
                    pnlCartoes.Visible = false;
                    pnlPagamento.Enabled = true;
                    pnlImpulsionado.Visible = false;
                    txtCartao.Focus();
                    if (Configuracoes.cancelarvendarejeicaocartao == true && dtgItens.RowCount > 0 && ConfiguracoesECF.pdv == true)
                    {
                        perguntarCancelamento = false;
                        btnCancelarCupom_Click(this, null);
                        DelegateCartoes("btSairCA");
                        AbortarPagamento();
                        limparDados();
                        pnlTroco.Visible = false;
                        AtivarBotoes();
                        zerarVariaveis();
                    }
                    break;
                default:
                    break;
            }

        }

        void DelegateTeclado(object sender, string text)
        {
            tecla = text;
            switch (tecla)
            {
                case "X":
                    x();
                    break;
                case "Enter":
                    TeclaEnter();
                    break;
                case "Limpar":
                    Control[] ctls = this.Controls.Find(controle, true);
                    if (ctls[0] is TextBox)
                    {
                        TextBox txtBox = ctls[0] as TextBox;
                        txtBox.Text = "";
                        txtBox.Focus();
                    };
                    break;
                default:
                    PreencheCampo();
                    break;
            }
        }

        void VerificarPagamento()
        {
            txtDesconto.Enabled = false;

            if (!Conexao.onLine)
            {
                txtCrediario.Enabled = false;
                txtCartao.Enabled = false;
                txtCheque.Enabled = false;
                txtDinheiro.Focus();
            }

            if (dinheiro == 0 && controle != "txtDinheiro") txtDinheiro.Text = "0.00";
            if (cartao == 0 && controle != "txtCartao") txtCartao.Text = "0.00";
            if (crediario == 0 && controle != "txtCrediario") txtCrediario.Text = "0.00";
            if (cheque == 0 && controle != "txtCheque") txtCheque.Text = "0.00";

            if (controle != "txtDinheiro") pnlCedulas.Visible = false;
        }

        private bool verificaDescontoPagamento(string FormaPagamento)
        {
            if (Configuracoes.formaDescontoMaxVenda != "TD")
            {

                try
                {
                    decimal desconto = (from d in Conexao.CriarEntidade().vendas
                                        where d.id == GlbVariaveis.glb_IP && d.codigofilial == GlbVariaveis.glb_filial
                                        select d.descontovalor).Sum();

                    if (FormaPagamento != Configuracoes.formaDescontoMaxVenda && desconto > 0)
                    {
                        MessageBox.Show("Desconto não permitirdo para esse forma de pagamento!");
                        return false;
                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }
            }

            return true;
        }

        private void cancelaDH(object sender, EventArgs e)
        {
            CancelarPagamento("DH");
        }

        private void CancelarPagamento(string formaPagamento)
        {
            switch (formaPagamento)
            {
                case "DH":
                    if (dinheiro == 0) return;
                    try
                    {
                        Venda.ExcluirPagamento("DH");
                        restante += dinheiro;
                        dinheiro = 0;
                        txtDinheiro.Enabled = true;
                        txtDinheiro.Text = "0.00";
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Não foi possível cancelar a forma de pagamento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case "CA":
                    if (cartao == 0) return;

                    // IVAN 20.09.2017 Coloquei essa nova regra e estou saindo com o return para continuar o pagamento quando 1 dos cartões foi aprovado e o outro não;

                    if (TEF.Transacoes("ntransacaoConfirmadas") > 0 || TEF.Transacoes("ntransacao") > 0 || cartao > 0)
                    {
                        txtDinheiro.Enabled = true;
                        txtDinheiro.Focus();
                        txtCrediario.Enabled = true;
                        txtCheque.Enabled = true;
                    }

                    return;




                    try
                    {
                        FuncoesECF fecf = new FuncoesECF();
                        if (TEF.Transacoes("ntransacaoConfirmadas") > 0 || TEF.Transacoes("ntransacao") > 0)
                        {
                            if (TEF.Transacoes("ntransacaoConfirmadas") > 0)
                            {
                                AbortarPagamento();
                            }
                            fecf.CancelarTransacaoTEFPendente();
                        }


                        if (!FuncoesECF.CupomFiscalAberto())
                            AbortarPagamento();

                        Venda.ExcluirPagamento(formaPagamento);
                        restante += cartao;
                        cartao = 0;
                        txtCartao.Enabled = true;
                        txtCartao.Text = "0.00";
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("Não foi possível cancelar a forma de pagamento ! ! ! " + erro.Message.ToString(), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case "CR":
                    if (crediario == 0) return;
                    try
                    {
                        Venda.ExcluirPagamento(formaPagamento);
                        restante += crediario;
                        crediario = 0;
                        txtCrediario.Enabled = true;
                        txtCrediario.Text = "0.00";
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Não foi possível cancelar a forma de pagamento ! !", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtCrediario.Focus();
                    break;
                case "CH":
                    if (cheque == 0) return;
                    try
                    {
                        Venda.ExcluirPagamento(formaPagamento);
                        restante += cheque;
                        cheque = 0;
                        txtCheque.Enabled = true;
                        txtCheque.Text = "0.00";
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Não foi possível cancelar a forma de pagamento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    };
                    txtCheque.Focus();
                    break;
            };
        }

        private void DescontoGerencial()
        {
            desconto = Convert.ToDecimal(string.Format("{0:n2}", txtDesconto.Text));
            txtDesconto.Enabled = false;
            txtDesconto.Text = string.Format("{0:n2}", desconto);
            txtDinheiro.Text = string.Format("{0:n2}", totalTransacao - desconto);
            if (desconto >= totalTransacao)
            {
                desconto = 0;
                txtDesconto.Text = "0,00";
            }

            restante = totalTransacao - desconto;
        }

        private void DescontoVoucher()
        {
            try
            {
                IqCard iqcard = new IqCard();
                var descontoVoucher = iqcard.PegarCodigoDesconto(txtDesconto.Text);
                if (descontoVoucher > 0)
                {
                    IqCard.voucherDesconto = txtDesconto.Text;
                    decimal valorDescontoVoucher = Math.Truncate(Venda.ObterValorDesconto() * (Convert.ToDecimal(descontoVoucher) / 100) * 100) / 100;
                    txtDesconto.Text = string.Format("{0:N2}", valorDescontoVoucher);//  String.Format("{0:N2}", Math.Truncate(Venda.ObterValorDesconto() * (Convert.ToDecimal(descontoVoucher) / 100)));
                    restante = totalTransacao - valorDescontoVoucher;//
                    desconto = valorDescontoVoucher;
                    DevolucaoPagamento();
                    txtDesconto.SelectAll();
                    DesativaDesconto();
                    txtDinheiro.SelectAll();
                    txtDinheiro.Focus();
                }
            }
            catch (Exception ex)
            {
                txtDesconto.Text = "0";
                txtDesconto.Enabled = true;
                txtDesconto.Focus();             
                MessageBox.Show(ex.Message);
            }
        }

        private void Desconto()
        {
            var descontoMaximo = Venda.ObterDescontoMaximo(dpFinanceiro, Venda.IQCard);
            // Aqui para não acontecer do operador do sistema configurar um arredontamento
            // por exemplo de 100,00 e depois fazer uma venda de 10 e dar um desconto de 100 por exemplo
            if (descontoMaximo > totalTransacao)
                descontoMaximo = 0;

            try
            {
                Convert.ToDecimal(txtDesconto.Text);
            }
            catch (Exception)
            {
                txtDesconto.Text = "0,00";
            }
            if (txtDesconto.Text == "0,00" && tipoDesconto == "%")
            {
                TipoDesconto(null, null);
                txtDesconto.SelectAll();
                return;
            };


            if (tipoDesconto == "%")
            {
                //Venda
                if (dpFinanceiro == "Venda")
                {
                    // Desconto sem fidelização
                    if (Convert.ToDecimal(txtDesconto.Text) > Configuracoes.descontoMaxVenda && string.IsNullOrEmpty(Venda.IQCard))
                    {
                        MessageBox.Show("Desconto % permitido: " + String.Format("{0:N2}", Configuracoes.descontoMaxVenda));
                        txtDesconto.Enabled = true;
                        txtDesconto.Text = "0,00";
                        txtDesconto.SelectAll();
                        txtDesconto.Focus();
                        return;

                    };

                    // Desconto com fidelização
                    // Renomeadoor Ivan em 07.05.2018
                    //if (Convert.ToDecimal(txtDesconto.Text) > Configuracoes.descontoCartaoFidelidade && !string.IsNullOrEmpty(Venda.IQCard))
                    //{
                    //    MessageBox.Show("Desconto fidelização % permitido: " + String.Format("{0:N2}", Configuracoes.descontoCartaoFidelidade));
                    //    txtDesconto.Enabled = true;
                    //    txtDesconto.Text = "0,00";
                    //    txtDesconto.SelectAll();
                    //    txtDesconto.Focus();
                    //    return;

                    //};


                    txtDesconto.Text = String.Format("{0:n2}", Math.Truncate(Venda.ObterValorDesconto() * (Convert.ToDecimal(txtDesconto.Text) / 100) * 100) / 100);
                };
                //Recebimento
                if (dpFinanceiro == "Recebimento")
                {
                    var maximoDescontoPerc = Configuracoes.descontoMaxRecJuros;
                    decimal valorDesconto = 0;
                    valorDesconto = FrmExtratoCliente.parcelas.Sum(c => c.valorJuros);
                    if (valorDesconto == 0)
                    {
                        valorDesconto = FrmExtratoCliente.parcelas.Sum(c => c.valorPagamento);
                        maximoDescontoPerc = Configuracoes.descontoMaxRecCapital;
                    }

                    if (Convert.ToDecimal(txtDesconto.Text) > maximoDescontoPerc)
                    {
                        MessageBox.Show("Desconto % permitido: " + String.Format("{0:C2}", maximoDescontoPerc));
                        txtDesconto.Text = "0,00";
                        txtDesconto.SelectAll();
                        return;
                    };
                    txtDesconto.Text = String.Format("{0:n2}", valorDesconto * (Convert.ToDecimal(txtDesconto.Text) / 100));
                };

            }

            if (tipoDesconto == "A")
            {
                if (Convert.ToDecimal(txtDesconto.Text) > Configuracoes.arredondamento)
                {
                    txtDesconto.Focus();
                    txtDesconto.SelectAll();
                    txtDesconto.SelectAll();
                    return;
                };
                txtDesconto.Text = string.Format("{0:n2}", Convert.ToDecimal(txtDesconto.Text) + guardaDesconto);

            };

            if (Convert.ToDecimal(txtDesconto.Text) > (descontoMaximo + Configuracoes.arredondamento))
            {
                MessageBox.Show("Desconto não permitido ! Máximo : " + String.Format("{0:C2}", descontoMaximo + Configuracoes.arredondamento), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDesconto.Text = "0,00";
                guardaDesconto = 0;
                tipoDesconto = "%";
                btDesconto.Text = "%";
                txtDesconto.SelectAll();
                return;
            };


            if (tipoDesconto == "%" && Permissoes.arredondar)
            {
                tipoDesconto = "A";
                btDesconto.Text = "A";
                guardaDesconto = Convert.ToDecimal(txtDesconto.Text);
                txtDesconto.Text = "0,00";
                txtDinheiro.Text = string.Format("{0:n2}", totalTransacao - guardaDesconto);
                txtDesconto.SelectAll();
                txtDesconto.Focus();
                txtDesconto.SelectAll();
                return;
            }

            if (tipoDesconto == "$")
            {
                decimal valorCapital = 0;

                if (dpFinanceiro == "Venda")
                    valorCapital = Venda.ObterValorDesconto();
                else
                    valorCapital = totalTransacao;

                decimal percentualDesconto = 0;
                decimal valorDecontoPermitido = 0;

                if (valorCapital > 0)
                {
                    desconto = Convert.ToDecimal(string.Format("{0:n2}", txtDesconto.Text));
                    percentualDesconto = ((desconto / valorCapital) * 100);
                    valorDecontoPermitido = ((valorCapital * Configuracoes.descontoMaxVenda) / 100);

                }

                if (percentualDesconto > Configuracoes.descontoMaxVenda)
                {
                    MessageBox.Show("Desconto não permitido ! Máximo : " + String.Format("{0:C2}", valorDecontoPermitido), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtDesconto.Text = "0,00";
                    guardaDesconto = 0;
                    tipoDesconto = "$";
                    btDesconto.Text = "$";
                    txtDesconto.Enabled = true;
                    txtDesconto.SelectAll();
                    txtDesconto.Focus();
                    return;
                }

                if (valorCapital == 0)
                    txtDesconto.Text = "0,00";

            }

            desconto = Convert.ToDecimal(string.Format("{0:n2}", txtDesconto.Text));
            txtDesconto.Enabled = false;
            txtDesconto.Text = string.Format("{0:n2}", desconto);
            txtDinheiro.Text = string.Format("{0:n2}", totalTransacao - desconto);
            if (desconto >= totalTransacao)
            {
                desconto = 0;
                txtDesconto.Text = "0,00";
            }

            restante = totalTransacao - desconto;

            DevolucaoPagamento();

            txtDesconto.SelectAll();
            DesativaDesconto();
            this.txtDinheiro.SelectAll();
            this.txtDinheiro.Focus();


        }

        private void DevolucaoPagamento()
        {

            if (devolucaoNumero > 0 && totalDevolucao > 0)
            {

                Funcoes.TravarTeclado(true);
                Devolucao();
                txtDinheiro.Text = string.Format("{0:n2}", restante);
                Funcoes.TravarTeclado(false);
            };

            //            var procuraDV = from n in Conexao.CriarEntidade().caixas
            //                where n.tipopagamento == "DV"
            //                && n.EnderecoIP == GlbVariaveis.glb_IP
            //                select n.tipopagamento;
            //if (procuraDV.Count() == 0)
            //{


        }

        private void DesativaDesconto()
        {
            if (desconto == 0) txtDesconto.Text = "0,00";
            btDesconto.Text = " $ ";
            txtDesconto.Enabled = false;
            btDesconto.Enabled = false;
        }

        private void TipoDesconto(object sender, EventArgs e)
        {
            switch (tipoDesconto)
            {
                case "%":
                    tipoDesconto = "$";
                    btDesconto.Text = "$";
                    break;
                case "$":
                    tipoDesconto = "%";
                    btDesconto.Text = "%";
                    break;
                default:
                    break;
            }
            txtDesconto.Focus();
        }

        //private void button1_Click_1(object sender, EventArgs e)
        //{
        //    siceEntities entidade = new siceEntities();
        //    var pagamento =
        //         from p in entidade.caixas
        //         group p by new { p.tipopagamento } into g
        //         orderby g.Key.tipopagamento ascending
        //         select new
        //         {
        //             tipopagamento = g.Key.tipopagamento,
        //             valor = g.Sum(p => p.valor)
        //         };

        //    foreach (var item in pagamento)
        //    {               

        //    }

        //}

        private void btCancelaCR_Click(object sender, EventArgs e)
        {
            CancelarPagamento("CR");
        }

        private void admTef_Click(object sender, EventArgs e)
        {
            //this.Enabled = false;            
            timeHora.Enabled = false;
            btnAdmTef.Enabled = false;
            RedeCartoes frmrede = new RedeCartoes();
            frmrede.ShowDialog();
            frmrede.Dispose();
            btnAdmTef.Enabled = true;
            timeHora.Enabled = true;
            //this.Enabled = true;
            Funcoes.TravarTeclado(false);
            //fecf.AdministrativoTEF("VISANET");                        
        }

        private void ChamarSuprimento()
        {
            if (ConfiguracoesECF.idECF == 0)
            {
                MessageBox.Show("ECF desligado ou sem papel.");
                return;
            }
            Suprimento.tipoPagamento = "SU";
            Suprimento suprimento = new Suprimento();
            suprimento.ShowDialog();
            txtcodProduto.Focus();
        }

        private void ChamarSangria()
        {
            if (btnSangria.Enabled == false || ConfiguracoesECF.idECF == 0)
            {
                MessageBox.Show("ECF desligado ou  sem papel.");
                return;
            }
            Sangria sangria = new Sangria();
            sangria.ShowDialog();
            txtcodProduto.Focus();
        }

        private void btnPreVenda_Click(object sender, EventArgs e)
        {
            if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
            {
                MessageBox.Show("SICEpdv Off-line Não é possivel visualizar Pre-Vendas", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (Conexao.tipoConexao == 2)
                {
                    Conexao.tipoConexao = 1;
                    ChamarPreVenda(true, false);
                    if (_pdv.numeroPreVenda == 0)
                        Conexao.tipoConexao = 2;
                }
                else
                    ChamarPreVenda(true, false);

                txtcodProduto.Focus();
            }
        }

        private void ChamarPreVenda(bool mostraPrevenda, bool mostraDAV)
        {
            timeHora.Enabled = false;

            if (ConfiguracoesECF.pdv == true && totalTransacao > 0)
            {
                timeHora.Enabled = false;
                MessageBox.Show("Cupom em aberto, não é possível processar pré-venda !!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            frmPreVenda prevenda = new frmPreVenda(mostraPrevenda, mostraDAV);
            prevenda.ShowDialog();

            timeHora.Enabled = false;

            if (numeroPreVenda > 0 || numeroDAV > 0)
            {
                if (numeroPreVenda > 0)
                {
                    clientePDV.idCliente = (from n in Conexao.CriarEntidade().contprevendaspaf
                                            where n.numeroDAVFilial == numeroPreVenda
                                            && n.codigofilial == GlbVariaveis.glb_filial
                                            select n.codigocliente).FirstOrDefault();
                    clientePDV.txtIdCliente.Text = clientePDV.idCliente.ToString();

                    clientePDV.txtParcelamentoCR.Text = (from n in Conexao.CriarEntidade().caixaprevendapaf
                                                         where n.documento == numeroDAV
                                                         select n.documento).Count().ToString() ?? "0";
                }

                if (numeroDAV > 0 && GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Geral.ToString())
                {
                    var dados = (from n in Conexao.CriarEntidade().contdav
                                 where n.numeroDAVFilial == numeroDAV
                                 && n.codigofilial == GlbVariaveis.glb_filial
                                 select n).FirstOrDefault();

                    clientePDV.idCliente = dados.codigocliente;

                    clientePDV.txtIdCliente.Text = clientePDV.idCliente.ToString();

                    clientePDV.txtParcelamentoCR.Text = (from n in Conexao.CriarEntidade().caixadav
                                                         where n.documento == numeroDAV
                                                         select n.documento).Count().ToString() ?? "0";

                    //clientePDV.nomeCliente = dados.ecfcon;
                    clientePDV.cpfcnpjCliente = dados.ecfCPFCNPJconsumidor;

                    var ecfConsumidor = Conexao.CriarEntidade().ExecuteStoreQuery<String>("select IFNULL(ecfconsumidor,'') from contdav where numerodavfilial = '" + numeroDAV + "' limit 1").FirstOrDefault();

                    Venda.dadosConsumidor =
                        new DadosConsumidorCupom
                        {
                            cpfCnpjConsumidor = "",
                            nomeConsumidor = "",
                            endConsumidor = "",
                            endNumero = "",
                            endBairro = "",
                            endCEP = "",
                            endCidade = "",
                            endEstado = "",
                            idConsumidor = "0",
                            ecfConsumidor = ecfConsumidor,
                            ecfCNPJCPFConsumidor = dados.ecfCPFCNPJconsumidor
                        };
                }

                if (numeroDAV > 0 && GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                {
                    clientePDV.idCliente = (from n in Conexao.CriarEntidade().contdavos
                                            where n.numeroDAVFilial == numeroDAV
                                            && n.codigofilial == GlbVariaveis.glb_filial
                                            select n.codigocliente).FirstOrDefault();
                    clientePDV.txtIdCliente.Text = clientePDV.idCliente.ToString();

                    clientePDV.txtParcelamentoCR.Text = (from n in Conexao.CriarEntidade().caixadav
                                                         where n.documento == numeroDAV
                                                         select n.documento).Count().ToString() ?? "0";

                }

                totalTransacao = venda.SomaItens();
                MostrarItens();
                txtcodProduto.Enabled = true;
                //Totalizar();
            }
            // Aqui restaura os dados dovendedor atual na venda. Pois na finalização da Pré-Venda ou DAV 
            // foi repassado dados do vendedor desses documentos. Aqui restaura os dados.
            //Vendedor.VendaVendedor(vendedorSalvo);
            lblVendedor.Text = Vendedor.codigoVendedor + " " + Vendedor.nomeVendedor.PadRight(30, ' ').Substring(0, 30);
            lblVendedor2.Text = Vendedor.codigoVendedor + " " + Vendedor.nomeVendedor.PadRight(30, ' ').Substring(0, 30);
        }

        public void btnCancelarCupom_Click(object sender, EventArgs e)
        {

            if ((ConfiguracoesECF.NFC == true && ConfiguracoesECF.davporImpressoraNaoFiscal == false) || ConfiguracoesECF.idECF == 9999)
            {
                #region
                timeHora.Enabled = false;

                try
                {
                    if (dtgItens.RowCount == 0 && perguntarCancelamento == true)
                    {
                        FrmDocumentos objDocumentos = new FrmDocumentos("NFC");
                        objDocumentos.ShowDialog();

                        if (FrmDocumentos.NumeroDocumento != "")
                        {
                            FrmObs obs = new FrmObs();
                            obs.ShowDialog();
                            if (!FrmObs.cancelado)
                            {

                                if (FrmObs.observacao.Length < 15)
                                {
                                    MessageBox.Show("Justificativa para cancelamento não pode ser menor que 15 caracteres");
                                }
                                else
                                {
                                    int documento = int.Parse(FrmDocumentos.NumeroDocumento);
                                    var doc = (from d in Conexao.CriarEntidade().contdocs
                                               where d.documento == documento
                                               select d).FirstOrDefault();

                                    FuncoesNFC objNFC = new FuncoesNFC();


                                    //FuncoesNFC.verificarGerenciadorNFCe("Fechar");
                                    //Thread.Sleep(700);
                                    //FuncoesNFC.verificarGerenciadorNFCe();
                                    //Thread.Sleep(300);

                                    if (ConfiguracoesECF.idNFC == 1)
                                    {
                                        FuncoesNFC.verificarGerenciadorNFCe();
                                        if (objNFC.cancelarNFCe(doc.ncupomfiscal, doc.ecfcontadorcupomfiscal, doc.chaveNFC, doc.protocolo, FrmObs.observacao) == true)
                                        {
                                            FuncoesNFC.verificarGerenciadorNFCe();
                                            CancelarCupom(int.Parse(FrmDocumentos.NumeroDocumento));
                                            MessageBox.Show("Documento Cancelado com sucesso!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.None);
                                        }
                                    }
                                    else if (ConfiguracoesECF.idNFC == 2 || ConfiguracoesECF.idECF == 9999)
                                    {

                                        if (FrmDocumentos.NumeroDocumento == "")
                                        {
                                            MessageBox.Show("Numero do Documento não pode ser vazio!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            return;
                                        }

                                        CancelarCupom(int.Parse(FrmDocumentos.NumeroDocumento));

                                        MessageBox.Show("Documento Cancelado com sucesso!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.None);

                                        FrmDocumentos.NumeroDocumento = "";

                                    }
                                    else if (ConfiguracoesECF.NFC == false && ConfiguracoesECF.idECF == 9999)
                                    {
                                        CancelarCupom(int.Parse(FrmDocumentos.NumeroDocumento));
                                    }
                                }

                            }
                            FrmDocumentos.NumeroDocumento = "";
                        }

                    }
                    else
                    {

                        if (perguntarCancelamento == true)
                        {
                            if (MessageBox.Show("Deseja Realmente excluir Documento?", "Atenção", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                                return;
                        }

                        if (!Permissoes.excluirDocumento)
                        {
                            FrmLogon Logon = new FrmLogon();
                            Operador.autorizado = false;
                            Logon.campo = "venexcluir";
                            Logon.lblDescricao.Text = "Excluir Itens: ";
                            Logon.txtDescricao.Text = "Excluir Itens: ";
                            Logon.ShowDialog();

                            if (!Operador.autorizado) return;
                        }

                        Venda.ExclulirItensPDV();
                        MessageBox.Show("Itens Cancelado com sucesso!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.None);
                        MostrarItens();
                        zerarVariaveis();

                    }

                }
                catch (Exception erro)
                {
                    MessageBox.Show("Não foi possivel cancelar o documento", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.None);
                    MessageBox.Show(erro.Message);
                }

                this.focarPDV();
                timeHora.Enabled = true;
                #endregion
            }
            else if (ConfiguracoesECF.pdv == false && ConfiguracoesECF.davporImpressoraNaoFiscal == true)
            {
                #region
                if (perguntarCancelamento == true)
                {
                    if (MessageBox.Show("Deseja Realmente excluir Itens do DAV?", "Atenção", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                        return;
                }


                if (Configuracoes.reservarEstoquePreVenda)
                {
                    var dados = (from v in Conexao.CriarEntidade().vendas
                                 where v.id == GlbVariaveis.glb_IP
                                 && v.cancelado == "N"
                                 select new
                                 {
                                     codigo = v.codigo,
                                     quantidade = v.quantidade,
                                 }).ToList();

                    foreach (var item in dados)
                    {

                        using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                        {
                            conn.Open();
                            EntityCommand cmd = conn.CreateCommand();
                            cmd.CommandText = "siceEntities.ReservarPreVenda";
                            cmd.CommandType = CommandType.StoredProcedure;

                            EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                            filial.Direction = ParameterDirection.Input;
                            filial.Value = GlbVariaveis.glb_filial;

                            EntityParameter codigoProduto = cmd.Parameters.Add("codigoProduto", DbType.String);
                            codigoProduto.Direction = ParameterDirection.Input;
                            codigoProduto.Value = item.codigo;

                            EntityParameter qtdPreVenda = cmd.Parameters.Add("quantidade", DbType.Decimal);
                            qtdPreVenda.Direction = ParameterDirection.Input;
                            qtdPreVenda.Value = -item.quantidade;
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }

                Venda.ExclulirItensPDV();

                MessageBox.Show("Cancelado com sucesso!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.None);
                MostrarItens();
                zerarVariaveis();
                txtcodProduto.Focus();
                #endregion
            }
            else
            {
                if (perguntarCancelamento == true)
                {
                    if (MessageBox.Show("Excluir o último cupom ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        txtcodProduto.Focus();
                        return;
                    }
                }

                timeHora.Enabled = false;
                FrmObs.bntcancelar = perguntarCancelamento;
                FrmObs obs = new FrmObs();
                obs.ShowDialog();
                if (FrmObs.cancelado)
                {
                    txtcodProduto.Focus();
                    timeHora.Enabled = true;
                    return;
                }

                CancelarCupom(0);
                timeHora.Enabled = true;
                txtcodProduto.Focus();
            }

            txtcodProduto.Focus();
            


        }



        private void CancelarCupom(int NFCe)
        {
            int? documento = 0;



            // Para nao gravar no contdocs
            if (ConfiguracoesECF.idNFC > 0 || ConfiguracoesECF.idECF == 9999)
            {
                documento = NFCe;
            }
            else
            {
                Venda.ultimoDocumento = 0;
                if (dpFinanceiro == "Recebimento" || ConfiguracoesECF.idECF == 0)
                    return;
            }

            try
            {
                FuncoesECF.VerificaImpressoraLigada(true, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            FuncoesECF.DetectarECF(ConfiguracoesECF.idECF);
            FuncoesECF.RelatorioGerencial("Fechar", "", "", false);


            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);



            string numeroCupomFiscal = "";
            string ccfCupom = "";

            if (ConfiguracoesECF.idNFC > 0 && ConfiguracoesECF.NFC == true)
            {

                var dados = (from d in entidade.contdocs
                             where d.documento == documento
                             select d).FirstOrDefault();


                numeroCupomFiscal = dados.ncupomfiscal;
                ccfCupom = dados.ecfcontadorcupomfiscal;

                if (dados.data < GlbVariaveis.Sys_Data)
                {
                    MessageBox.Show("Não é possivel cancelar um documento de um movimento anterior", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else
            {
                try
                {
                    numeroCupomFiscal = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("1 - Obter COO Cupom: " + ex.Message);
                    return;
                }

                if (String.IsNullOrEmpty(numeroCupomFiscal.Trim()) || numeroCupomFiscal == "0" || numeroCupomFiscal == "000000")
                {
                    MessageBox.Show("Não foi possível obter o COO do cupom !");
                    return;
                }

                // Aqui por a daruma nao pega o numero do cupom atual quando esta em aberto.
                // A daruma sempre retorna o coo do ultimo cupom encerrado
                if (ConfiguracoesECF.idECF == 2 && FuncoesECF.CupomFiscalAberto())
                {
                    numeroCupomFiscal = Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(numeroCupomFiscal) + 1, 6, false);
                }

                ccfCupom = "";
            }

            string ccfCupomFiscal = FuncoesECF.CCFContadorCupomECF();

            if (!Conexao.onLine)
            {
                try
                {
                    venda.ExcluirDocumento(numeroCupomFiscal, "", documento.GetValueOrDefault(), FrmObs.observacao);
                    MostrarItens();
                    zerarVariaveis();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Excluindo cupom Off-line: " + ex.Message);
                }
                return;
            }

            string serie = int.Parse((ConfiguracoesECF.NFCserie.Trim() == null || ConfiguracoesECF.NFCserie.Trim() == "") ? "0" : ConfiguracoesECF.NFCserie).ToString();

            if (ConfiguracoesECF.idNFC > 0 || ConfiguracoesECF.idECF != 9999)
            {
                if (ConfiguracoesECF.idNFC > 0)
                {
                    documento = (from n in entidade.contdocs
                                 where n.ncupomfiscal == numeroCupomFiscal
                                 && n.ecfcontadorcupomfiscal.Contains(serie)
                                 && n.CodigoFilial == GlbVariaveis.glb_filial
                                 && n.data == GlbVariaveis.Sys_Data.Date
                                 && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                 select (int?)n.documento).FirstOrDefault();
                }
                else
                {

                    documento = (from n in entidade.contdocs
                                 where n.ncupomfiscal == numeroCupomFiscal
                                 && n.ecfcontadorcupomfiscal == ccfCupomFiscal
                                 && n.CodigoFilial == GlbVariaveis.glb_filial
                                 && n.data == GlbVariaveis.Sys_Data.Date
                                 && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                 select (int?)n.documento).FirstOrDefault();

                }


                var cdc = (from n in entidade.contdocs
                           where n.ncupomfiscal == numeroCupomFiscal
                           && n.CodigoFilial == GlbVariaveis.glb_filial
                           && n.data == GlbVariaveis.Sys_Data.Date
                           && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                           select n.contadordebitocreditoCDC).FirstOrDefault() ?? "";


                if (!string.IsNullOrEmpty(cdc.Trim()))
                {
                    MessageBox.Show("Ultimo doc emitido foi um Contador de Débito e crédito, não é possível excluir !");
                    return;
                }

            }

            var estornado = (from n in entidade.contdocs
                             where n.documento == documento
                             select n.estornado).FirstOrDefault();

            if (estornado == "S")
            {
                MessageBox.Show("Documento: " + documento.ToString() + ", já foi estornado.");
                return;

            }





            if (!Permissoes.excluirDocumento)
            {

                FrmLogon Logon = new FrmLogon();
                Operador.autorizado = false;
                Logon.campo = "venexcluir";
                Logon.lblDescricao.Text = "Excluir Documento: " + documento.ToString();
                Logon.txtDescricao.Text = "Excluir o Documento: " + documento.ToString();
                Logon.ShowDialog();
                if (!Operador.autorizado) return;

                if (documento > 0)
                {
                    if (MessageBox.Show("Excluir o cupom: Documento de referência: " + documento.ToString() + "  ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }
            };

            FrmMsgOperador msg = new FrmMsgOperador("", "Excluindo cupom ");
            msg.Show();
            Application.DoEvents();

            try
            {
                Venda.IQCard = "";
                //Funcoes.TravarTeclado(true);
                try
                {
                    if (!FuncoesECF.CancelarCupomECF())
                    {
                        Funcoes.TravarTeclado(false);
                        return;
                    };
                }
                catch (Exception ex)
                {
                    Funcoes.TravarTeclado(false);
                    MessageBox.Show(ex.Message);
                    msg.Dispose();
                }


            }

            catch (Exception ex)
            {
                Funcoes.TravarTeclado(false);
                msg.Dispose();
                MessageBox.Show(ex.Message);
            }

            try
            {
                if (ConfiguracoesECF.idNFC == 0 && ConfiguracoesECF.NFC == false)
                {
                    numeroCupomFiscal = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);
                    ccfCupom = "";
                }
            }
            catch (Exception ex)
            {
                Funcoes.TravarTeclado(false);
                MessageBox.Show("2 - Obter COO Cupom: " + ex.Message);
                msg.Dispose();
                return;
            }
            timeHora.Enabled = false;

            var doc = (from d in Conexao.CriarEntidade().contdocs
                       where d.documento == documento
                       select d).FirstOrDefault();


            dadosNFCe nfc = null;

            if (ConfiguracoesECF.idNFC > 0 && ConfiguracoesECF.NFC == true && doc.protocolo != null && doc.protocolo != "" && doc.protocolo != "Erro")
            {
                //FuncoesNFC.verificarGerenciadorNFCe("fechar");
                //FuncoesNFC.verificarGerenciadorNFCe();

                FuncoesNFC objNFC = new FuncoesNFC();

                objNFC.GerarReq(doc.ncupomfiscal, doc.ecfcontadorcupomfiscal, documento.ToString(), "C", GlbVariaveis.glb_IP, FrmObs.observacao);
                nfc = objNFC.LerResp("cancelamento");
                File.Delete(@"C:\iqsistemas\SICENFC-e\Resp\resp.txt");

                objNFC.GerarReq(doc.ncupomfiscal, doc.ecfcontadorcupomfiscal, documento.ToString(), "P", GlbVariaveis.glb_IP, "");

                if (nfc.chaveNFe == "Erro")
                {
                    msg.Dispose();
                    return;
                }

                string SQL="";
                if (Configuracoes.cfgarquivardados == "S")
                {
                    //string SQL = "call atualizarNFe('0','0','65','" + GlbVariaveis.glb_filial + "','"+documento.ToString()+"')";
                    //Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                    //Conexao.CriarEntidade().SaveChanges();

                    try
                    {
                        SQL = "call atualizarNFe('0','0','65','" + GlbVariaveis.glb_filial + "','" + documento.ToString() + "')";
                        string retorno = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                        if (retorno != "1")
                            MessageBox.Show(retorno.ToString());

                        //Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                        //Conexao.CriarEntidade().SaveChanges();
                    }
                    catch (Exception)
                    {
                        SQL = "call atualizarNFe('0','0','65','" + GlbVariaveis.glb_filial + "','" + documento.ToString() + "','S')";
                        string retorno = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                        if (retorno != "1")
                            MessageBox.Show(retorno.ToString());

                        //Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                        //Conexao.CriarEntidade().SaveChanges();
                    }
                }

            }

            try
            {

                if (dtgItens.RowCount > 0)
                {
                    if (Conexao.tipoConexao == 2 && documento == null)
                        entidade = Conexao.CriarEntidade(false);
                    else
                        entidade = Conexao.CriarEntidade(true);

                    var dadosExclusao = (from n in entidade.vendas
                                         where n.id == GlbVariaveis.glb_IP
                                         select n.inc);
                    foreach (var item in dadosExclusao)
                    {
                        siceEntities entidadeExcluir = Conexao.CriarEntidade();

                        if (Conexao.tipoConexao == 2 && documento == null)
                            entidadeExcluir = Conexao.CriarEntidade(false);
                        else
                            entidadeExcluir = Conexao.CriarEntidade(true);

                        var itemCancelar = (from n in entidadeExcluir.vendas
                                            where n.inc == item
                                            select n).First();
                        itemCancelar.cancelado = "S";
                        entidade.SaveChanges();

                    }

                    ConstruirVenda();
                    totalTransacao = (from n in entidade.vendas where n.id == GlbVariaveis.glb_IP select n.total).Sum();
                    venda.valorBruto = totalTransacao;
                    venda.valorLiquido = totalTransacao;
                    venda.desconto = 0;
                    venda.encargos = 0;

                }
                // para nao causar erro 

                // Venda objvenda = new Venda();


                venda.ExcluirDocumento(numeroCupomFiscal, ccfCupom, documento.GetValueOrDefault(), FrmObs.observacao);


                if (ConfiguracoesECF.idNFC > 0 && ConfiguracoesECF.NFC == true)
                    venda.AtualizarDocumentoExcluidoNFCe(nfc);

                MostrarItens();
                zerarVariaveis();

            }
            catch (Exception ex)
            {
                Funcoes.TravarTeclado(false);
                if (ConfiguracoesECF.idNFC < 1)
                {
                    MessageBox.Show("Não foi possível excluir o Cupom verifique ECF", "SICEpdv.net " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Não foi possível Cancelar o NFCe", "SICEpdv.net " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            finally
            {
                zerarVariaveis();
                Funcoes.TravarTeclado(false);
                msg.Dispose();
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (ConfiguracoesECF.idECF > 0 && dtgItens.RowCount > 0)
            {
                MessageBox.Show("Cupom fiscal com itens. Menu Fiscal inacessível !");
                return;
            }
            ChamarMenuFiscal();
        }

        private void ChamarMenuFiscal()
        {
            timeHora.Enabled = false;
            FrmMenuFiscal fiscal = new FrmMenuFiscal();
            fiscal.ShowDialog();
            timeHora.Enabled = true;
            txtcodProduto.Focus();
        }

        private void btnEncerrar_Click(object sender, EventArgs e)
        {
            FuncoesECF.VerificarStatusPapel(true);
            ChamarCaixa();
        }

        private void ChamarCaixa()
        {

            if (!Permissoes.verSaldoCaixa)
            {

                FrmLogon Logon = new FrmLogon();
                Operador.autorizado = false;
                Logon.campo = "relcaixa";
                Logon.lblDescricao.Text = "SALDO DE CAIXA";
                Logon.txtDescricao.Text = GlbVariaveis.glb_Usuario +
                    " Ver saldo caixa ";
                Logon.ShowDialog();
                if (!Operador.autorizado)
                    return;
            };

            timeHora.Enabled = false;
            FrmResumoCaixa frm = new FrmResumoCaixa();
            frm.ShowDialog();
            timeHora.Enabled = true;
            frm.Dispose();
            txtcodProduto.Focus();
        }

        private void timeHora_Tick(object sender, EventArgs e)
        {

            lblDataHora.Text = String.Format("{0:dd/MM/yyyy hh:mm}", DateTime.Now);
            lblHorasAtual.Text = DateTime.Now.ToShortTimeString();

            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;

            DateTime data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            lblDataAtual.Text = dtfi.GetDayName(data.DayOfWeek) + Environment.NewLine + DateTime.Now.Day + " de " + dtfi.GetMonthName(DateTime.Now.Month);

            if (string.IsNullOrEmpty(txtcodProduto.Text))
            {
                int estadoGaveta = FuncoesECF.EstadoGaveta();

                if (estadoGaveta == 0)
                {
                    lblMsgPDV.Text = "POR FAVOR, FECHAR GAVETA !";
                    //Beep(500, 100);
                }
                else
                {
                    lblMsgPDV.Text = Configuracoes.mensagemPDV;
                }
            }

            statusbntCancelar();
        }

        private void ChamarProdutos(bool consultarPreco)
        {
            FrmProdutos frmprd = new FrmProdutos();
            FrmProdutos.consultarPreco = consultarPreco;

            if (consultarPreco)
                frmprd.rdbBarras.Checked = true;

            frmprd.ShowDialog();
            txtcodProduto.Text = FrmProdutos.ultCodigo;
            txtDescontoPercItem.Text = "0,00";
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            try
            {
                Paf PAF = new Paf();
                PAF.GravarRelatorioR();
                return;
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
            FuncoesECF fecf = new FuncoesECF();
            string sMarca = Funcoes.SetLength(15);
            string sModelo = Funcoes.SetLength(20);
            string sTipo = Funcoes.SetLength(7);
            int iRetorno = BemaFI32.Bematech_FI_MarcaModeloTipoImpressoraMFD(ref sMarca, ref sModelo, ref sTipo);

            Paf paf = new Paf();
            paf.GravarRelatorioR();
            return;
        }

        private void btCancelaCH_Click(object sender, EventArgs e)
        {
            CancelarPagamento("CH");
        }

        private void DigitarQuantidade(bool pesquisar = false)
        {

            decimal descontoPercItem = 0;
            if (pesquisar)
            {
                txtcodProduto.BackColor = Color.White;
                txtcodProduto.ForeColor = Color.Black;
                lblAviso.Text = "";
                try
                {
                    // if (string.IsNullOrEmpty(produto.codigo) || ConfiguracoesECF.davporImpressoraNaoFiscal)
                    // {
                    descontoPercItem = 0;
                    var dadosPrd = produto.ProcurarCodigo(txtcodProduto.Text, GlbVariaveis.glb_filial);

                    descontoPercItem = produto.descontoPromocao;
                    preco = produto.preco * (-descontoPercItem / 100) + produto.preco;

                    if (produto.situacao == "Item da Balança" && txtcodProduto.Text.Length > 10 && quantidade <= 1 && txtcodProduto.Text.Substring(0, 1) == Configuracoes.digitoVerificadorCodBarras)
                    {
                        //2000001005101                    
                        //quantidade = (Convert.ToDecimal(String.Format("{0:n3}", txtcodProduto.Text.Substring(6, 7))) / preco / 100) / 10;
                        quantidade = (Convert.ToDecimal(String.Format("{0:n3}", txtcodProduto.Text.Substring(7, 6))) / preco / 100) / 10;
                        if (Configuracoes.totalnoFinalCodBarrasBalanca == false)
                            quantidade = (Convert.ToDecimal(String.Format("{0:n3}", txtcodProduto.Text.Substring(7, 6))) / 100 / 100);

                        lblQtd.Text = string.Format("{0:N3}", quantidade);
                        if (produto.codigoBarras.Length > Configuracoes.tamanhacodBarrasBalanca)
                        {
                            lblQtd.Text = "1 X";
                            quantidade = 1;
                        }


                        DigitarQuantidade();
                        return;
                    }

                    MostrarDadosItem();
                    //  }
                }
                catch (Exception ex)
                {

                    lblAviso.Text = ex.Message;
                    Beep(1000, 300);
                    txtcodProduto.BackColor = Color.Black;
                    txtcodProduto.ForeColor = Color.White;
                    Application.DoEvents();
                    //MessageBox.Show(ex.Message);       
                    txtcodProduto.Focus();
                    return;
                }
            }

            txtQtd.Text = string.Format("{0:N3}", lblQtd.Text.Replace("X", ""));
            MostrarDadosItem();
            txtPreco.Text = string.Format("{0:n2}", produto.preco);

            if (produto.situacao != "Promoção")
                txtDescontoPercItem.Text = string.Format("{0:n2}", produto.descontoPromocao);
            else
                txtDescontoPercItem.Text = (Math.Truncate(produto.descontoPromocao * 100) / 100).ToString();

            //txtDescontoPercItem.Text = string.Format("{0:n2}", produto.descontoPromocao);
            grpQtd.Visible = true;
            txtQtd.Focus();
        }

        private void MostrarDadosItem()
        {
            pnlAviso.Visible = false;
            lblDescricaoPrd.Text = produto.descricao;
            lblQtds.Text = "Quantidades:   Atual: " + string.Format("{0:N2}", produto.quantidade) + "  |  Qtd.Pré-Vendas : " + string.Format("{0:N2}", produto.quantidadeOrcamento) + "  |  Disponível: " + string.Format("{0:N2}", produto.quantidadeDisponivel);

            /*
            if(sugerirProdutos)
            {
                sugestaoVenda(produto.codigo);
            } */
        }

        private void MudarPreco()
        {
            if (!Configuracoes.mudarPrecoVenda)
                return;
            try
            {
                if (string.IsNullOrEmpty(produto.codigo))
                {
                    var dados = produto.ProcurarCodigo(txtcodProduto.Text, GlbVariaveis.glb_filial);
                }

                MostrarDadosItem();

                decimal precoTabela = 0;

                try
                {
                    precoTabela = Produtos.tabelaPrecoQtd(txtcodProduto.Text, Produtos.tabelaPreco, Convert.ToDecimal(txtQtd.Text));
                    
                }
                catch
                {
                  
                }

                if (precoTabela > 0)
                {
                    txtPreco.Text = string.Format("{0:n2}", precoTabela);
                }
                else
                {
                    txtPreco.Text = string.Format("{0:n2}", produto.preco);
                }

                if (produto.situacao != "Promoção")
                    txtDescontoPercItem.Text = string.Format("{0:n2}", produto.descontoPromocao);
                else
                    txtDescontoPercItem.Text = (Math.Truncate(produto.descontoPromocao * 100) / 100).ToString();

                grpPreco.Visible = true;
                txtTotalItem.Visible = false;
                pnDigitacao.Visible = false;
                /*if ((produto.unidade == "FR" || produto.unidade == "L") && quantidade == 1)
                {
                    txtTotalItem.Visible = true;
                    txtTotalItem.Focus();
                    return;
                }*/

                txtDescontoPercItem.Focus();

            }
            catch (Exception erro)
            {
                MessageBox.Show("Item com restrição:" + erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnDAV_Click(object sender, EventArgs e)
        {
            if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
            {
                MessageBox.Show("SICEpdv Off-line Não é possivel visualizar DAVs", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (Conexao.tipoConexao == 2)
                {
                    Conexao.tipoConexao = 1;
                    ChamarPreVenda(false, true);
                    if (_pdv.numeroDAV == 0)
                        Conexao.tipoConexao = 2;
                }
                else
                    ChamarPreVenda(false, true);

                MostrarItens();
                txtcodProduto.Focus();
            }
        }

        private void _pdv_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Habite a propriedadeKeypreview = true com isso 
            // eliminar-se o som quando tecla enter e pula para o próximo campo
            ativarIndoor = false;
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }

            if (e.KeyChar == 19)
            {
                Sincronizar(sender, e);
            }

        }

        private void Sincronizar(object sender, EventArgs e)
        {
            chamarSincronizador();
        }

        private void chamarSincronizador()
        {
            if (Conexao.tipoConexao == 1)
            {
                #region
                using (IObjectContainer tabela = Db4oFactory.OpenFile("contdocs.yap"))
                {
                    var dado = (from StandAloneContdocs n in tabela
                                select n.documento).Count();
                    if (dado == 0)
                    {
                        MessageBox.Show("Não existe dados para sincronização", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                };

                if (dtgItens.RowCount > 0)
                {
                    MessageBox.Show("Finalize a venda. Não é possível transimitir dados ao Servidor com itens em aberto", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                Sincronizar sinc = new Sincronizar();
                sinc.ShowDialog();
                VerificaConexao();

                using (IObjectContainer tabela = Db4oFactory.OpenFile("contdocs.yap"))
                {
                    var dado = (from StandAloneContdocs n in tabela
                                select n.documento).Count();
                    if (dado == 0)
                    {
                        lblAlerta.Text = "";
                    }
                };
                #endregion
            }
            else
            {
                Sincronizar sinc = new Sincronizar();
                sinc.ShowDialog();
                VerificaConexao();
            }
        }

        private void btnVendedor_Click(object sender, EventArgs e)
        {
            ChamarVendedor();
        }

        private void ChamarVendedor()
        {
            FrmVendedores frmVend = new FrmVendedores();
            frmVend.ShowDialog();
            lblVendedor.Text = "VEN: " + Vendedor.codigoVendedor + " - " + Vendedor.nomeVendedor.PadRight(30, ' ').Substring(0, 30);
            lblVendedor2.Text = "VENDEDOR: " + Vendedor.codigoVendedor + " - " + Vendedor.nomeVendedor.PadRight(30, ' ').Substring(0, 30);
            Application.DoEvents();
            txtcodProduto.Focus();
        }

        private void btnEntrega_Click(object sender, EventArgs e)
        {
            chamardadosEntrega();
        }

        private void chamardadosEntrega()
        {
            FrmEnderecoEntrega frmEntrega = new FrmEnderecoEntrega();
            frmEntrega.txtCodigo.Text = clientePDV.idCliente.ToString();
            frmEntrega.ShowDialog();
            txtcodProduto.Focus();
        }

        private void btnPrecoVarejo_Click(object sender, EventArgs e)
        {
            Produtos.tabelaPreco = "varejo";
            btnPrecoVarejo.BackColor = System.Drawing.Color.Green;
            btnPrecoAtacado.BackColor = Color.FromArgb(91, 192, 222);
            //btnPrecoAtacado.BackColor = System.Drawing.SystemColors.Control;
            txtcodProduto.Focus();
        }

        private void btnPrecoAtacado_Click(object sender, EventArgs e)
        {
            if (!Permissoes.vendaatacado)
            {
                FrmLogon logon = new FrmLogon();
                logon.campo = "vendaatacado";
                logon.txtDescricao.Text = "Venda no atacado";
                logon.ShowDialog();
                if (!Operador.autorizado)
                {
                    return;
                }
            }

            Produtos.tabelaPreco = "atacado";
            //btnPrecoVarejo.BackColor = System.Drawing.SystemColors.Control;
            btnPrecoVarejo.BackColor = Color.FromArgb(91, 192, 222);
            btnPrecoAtacado.BackColor = System.Drawing.Color.Green;
            txtcodProduto.Focus();
        }

        private void btnSincronizar_Click(object sender, EventArgs e)
        {
            Sincronizar(sender, e);
            txtcodProduto.Focus();
        }

        private void lblModo_Click(object sender, EventArgs e)
        {
            AlterarFormaVenda();
        }

        private void AlterarFormaVenda()
            {
                if (dtgItens.Rows.Count > 0 && dtgItens.Visible == true)
                {
                    MessageBox.Show("Venda contém itens, finalize primeiro para mudar o terminal");
                    return;
                }
                Venda.numeroPED = 0;
                FrmTipoTerminal terminal = new FrmTipoTerminal();
                terminal.ShowDialog();
                VerificaConexao();
                VerificaFuncoesFiscais();
                txtcodProduto.Focus();
            }

        private void btnClasse_Click(object sender, EventArgs e)
        {
            ChamarClasse();
        }

        private void ChamarClasse()
        {
            frmClasseVenda frmclasse = new frmClasseVenda();
            frmclasse.ShowDialog();
            classeVenda = frmClasseVenda.codigoClasse;
            txtcodProduto.Focus();
            lblClasse.Text = "";
            if (!string.IsNullOrEmpty(classeVenda))
                lblClasse.Text = frmClasseVenda.descricaoClasse;
        }

        private void chamaTabela()
        {
            frmTabelaJuros tabela = new frmTabelaJuros();
            frmTabelaJuros.valorFinanciamento = totalTransacao;
            //frmTabelaJuros.valorOriginalFinanciamento = totalTransacao;
            tabela.ShowDialog();

            encargos = frmTabelaJuros.encargos;

            // Truncar para que nao hava diferenca no ECF
            encargos = (Math.Truncate(encargos * 100) / 100);

            classeVenda = frmTabelaJuros.classe;
            descontoJuros = frmTabelaJuros.desconto;
            aceitaDesconto = frmTabelaJuros.aceiraDesconto;
            valorEntrada = frmTabelaJuros.valorEntrada;

            if (venda.SomaDescontoItens() > 0 && aceitaDesconto == true && descontoJuros > 0 && venda.SomaItensSemDescontoItens() == 0)
            {
                MessageBox.Show("Não é possivel escolher uma tabela de juros com desconto quando o mesmo ja foi aplicando", "Atenção ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Retire o desconto da venda e escolha a tabela de juros","Anteção",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }


            if (!frmTabelaJuros.financiamentoCalculado)
            {
                txtcodProduto.Focus();
                return;
            }

           
            clientePDV.txtParcelamentoCR.Text = frmTabelaJuros.parcelamento.ToString();
            clientePDV.txtIntervaloCR.Text = frmTabelaJuros.intervalo.ToString();
            cartoes.txtParcelamentoCA.Text = frmTabelaJuros.parcelamento.ToString();

            if (!verificarSituacaoJuros(frmTabelaJuros.codigoTabelaJuros))
                return;

            parcelamentoMaximo = frmTabelaJuros.parcelamento;

            lblClasse.Text = classeVenda;
            
            if(frmTabelaJuros.descontoGerencial == false)
                Totalizar(true);
            else
                this.chamaDescontoGerencial();

        }

        private void btnTabelajuros_Click(object sender, EventArgs e)
        {
            if (totalTransacao == 0)
            {
                MessageBox.Show("Venda sem itens. Digite os itens para depois escolher a tabela de juros e finalizar a venda !");
                txtcodProduto.Focus();
                return;
            }

            chamaTabela();

            /*frmTabelaJuros tabela = new frmTabelaJuros();
            frmTabelaJuros.valorFinanciamento = totalTransacao;
            tabela.ShowDialog();

            encargos = frmTabelaJuros.encargos;

            // Truncar para que nao hava diferenca no ECF
            encargos = (Math.Truncate(encargos * 100) / 100);

            classeVenda = frmTabelaJuros.classe;

            if (!frmTabelaJuros.financiamentoCalculado)
            {
                txtcodProduto.Focus();
                return;
            }

            clientePDV.txtParcelamentoCR.Text = frmTabelaJuros.parcelamento.ToString();
            clientePDV.txtIntervaloCR.Text = frmTabelaJuros.intervalo.ToString();
            parcelamentoMaximo = frmTabelaJuros.parcelamento;

            lblClasse.Text = classeVenda;
            Totalizar();*/
        }

        private void menuFiscalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMenuFiscal fiscal = new FrmMenuFiscal();
            fiscal.ShowDialog();
        }

        private void exportarParaLayoutFiscalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timeHora.Enabled = false;
            ChamarExportacao();
            timeHora.Enabled = true;
        }

        private void ChamarExportacao()
        {     
       
            if (!File.Exists("iEFD.exe"))
            {
                MessageBox.Show("Módulo iEFD de exportações não foi encontrado no diretório da aplicação !");
                return;
            }
            System.Diagnostics.Process.Start(@"iEFD.exe",GlbVariaveis.glb_filial+" N  0 " + GlbVariaveis.glb_senhaUsuario);


            //timeHora.Enabled = false;
            //frmExportacaoFiscal layout = new frmExportacaoFiscal();
            //layout.ShowDialog();
            //timeHora.Enabled = true;
        }

        private  void button1_Click_3(object sender, EventArgs e)
        {

            return;

            PAFArquivos paf = new PAFArquivos();
            string arquivo = "ReducaoZ_" + ConfiguracoesECF.nrFabricacaoECF.PadLeft(20, '0').Substring(6, 14) + String.Format("{0:ddMMyyyy}",DateTime.Now.Date)+".xml";
            paf.ReducaoZXML(true, DateTime.Now.Date, DateTime.Now.Date, "001", @ConfigurationManager.AppSettings["dirReducaoZEnvio"] + @"\" +arquivo, false);
            return;

              var dadosECF = (from n in Conexao.CriarEntidade().r01
                                where n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                select n);

              if (dadosECF.Count() == 0 || dadosECF == null)
              {
                  MessageBox.Show("Grava");
                  return;
              }
              MessageBox.Show("Grava nao");
              return;
            
            siceEntities entidade = Conexao.CriarEntidade();
            Venda objVenda = new Venda();
            PreVenda objPreVenda = new PreVenda();
            PreVenda.MontarPreVenda(86);

            
            objPreVenda.FinalizarPreVenda(86, 10M, 0, 0, false, true);

            

            var COO = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).ToString();
            var CCF = FuncoesECF.CCFContadorCupomECF();


            contprevendaspaf dados = (from n in entidade.contprevendaspaf
                                      where n.numero == 86
                                      select n).First();
            dados.cancelada = "S";
            dados.numeroECF = ConfiguracoesECF.numeroECF;
            dados.ecfcontadorcupomfiscal = CCF;
            //?
            dados.datafinalizacao = GlbVariaveis.Sys_Data.Date;
            dados.ncupomfiscal = COO;
            entidade.SaveChanges();            

            string sql = "SELECT documento FROM contdocs WHERE documento=(SELECT MAX(documento) from contdocs)";

            int doc = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql).FirstOrDefault();
            
            FuncoesECF.CancelarCupomECF();
             COO = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).ToString();
             CCF = FuncoesECF.CCFContadorCupomECF();


            objVenda.ExcluirDocumento(COO, CCF, doc, "Cancelamento PV");            

            return;
            
                      

            //GravarRelatorioR_Anterior();
        }

        private void GravarRelatorioR_Anterior()
        {
            Paf paf = new Paf();
            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando Relatório R");
            try
            {
                msg.Show();
                Application.DoEvents();
                ConfiguracoesECF.idECF = Convert.ToInt16(txtcodProduto.Text.Substring(0, 1));
                paf.GravarRelatorioR();
                MessageBox.Show("Gerado com sucesso");
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
               // MessageBox.Show(erro.InnerException.ToString() ?? "nulo");
            }
            finally
            {
                msg.Dispose();
            }
        }

        private void _pdv_FormClosed(object sender, FormClosedEventArgs e)
        {
            FuncoesECF.FecharPorta(999);

            if (File.Exists(@"C:\iqsistemas\IQSync\Todo\aplicarAtualizacao.txt"))
            {
                MessageBox.Show("A plataforma SICE.net irá aplicar a atualização ao encerrar a aplicação!");

                File.Copy(@"C:\iqsistemas\IQSync\Todo\aplicarAtualizacao.txt", @"C:\iqsistemas\IQSync\Todo\aplicarAtualizacaoAgora.txt");
                File.Delete(@"C:\iqsistemas\IQSync\Todo\aplicarAtualizacao.txt");
            }
            else
            {
                Funcoes.escreveArquivo(@"C:\iqsistemas\IQSync\ToDo\fecharSync.txt", "fechaAplicacao");
            }

            /*
             * 
             * if FileExists('C:\iqsistemas\IQSync\Todo\aplicarAtualizacao.txt')=true then
      begin
         mensagem:= 'A plataforma SICE.net irá aplicar a atualização ao encerrar a aplicação!';
         T_frmmensagens.Mensagem(mensagem, 'I',[mbOk]);
      end
      else
        begin
          lstArquivo := TStringList.Create();
          lstArquivo.Clear;
          lstArquivo.Add('fechaAplicacao');
          lstArquivo.SaveToFile('C:\iqsistemas\IQSync\ToDo\fecharSync.txt');
        end;

            */
        }

        private void btnDescEspecial_Click(object sender, EventArgs e)
        {
            /*FrmDescEspecial frmDesc = new FrmDescEspecial(totalTransacao);
            frmDesc.ShowDialog();
            desconto = Convert.ToDecimal(frmDesc.txtDesconto.Text);
            if (desconto > 0)
            {
                txtDesconto.Text = frmDesc.txtDesconto.Text;
                txtDesconto.Enabled = false;
                DescontoGerencial();
                Totalizar();
            }
            txtcodProduto.Focus();*/

            this.chamaDescontoGerencial();
        }

        private void chamaDescontoGerencial()
        {
            try
            {
                decimal totalAtacado = 0;
                if (Configuracoes.descontoAtacado == false)
                {
                    var valorAtacado = (from v in Conexao.CriarEntidade().vendas
                                        where v.vendaatacado == "S" && v.cancelado == "N"
                                        select (v.total - v.ratdesc)).ToList();

                    if (valorAtacado.Count() > 0)
                    {
                        totalAtacado = valorAtacado.Sum();
                        if (totalAtacado < 0)
                            totalAtacado = 0;
                    }


                }

                FrmDescEspecial frmDesc = new FrmDescEspecial(totalTransacao - totalAtacado);
                frmDesc.ShowDialog();
                desconto = Convert.ToDecimal(frmDesc.txtDesconto.Text);
                if (desconto > 0)
                {
                    txtDesconto.Text = frmDesc.txtDesconto.Text;
                    txtDesconto.Enabled = false;
                    DescontoGerencial();
                    Totalizar();
                }
                txtcodProduto.Focus();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void btnDescontoMax_Click(object sender, EventArgs e)
        {
            if (ConfiguracoesECF.idECF != 0 && ConfiguracoesECF.NFC == false)
            {
                MessageBox.Show("Desconto máximo somente em DAV ou Pré-Venda");
                return;
            }

            if (descontoMax == false)
            {
                btnDescontoMax.BackColor = Color.FromArgb(91, 191, 223);
                descontoMax = true;

                using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.DescontoMaximo";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter acao = cmd.Parameters.Add("acao", DbType.String);
                    acao.Direction = ParameterDirection.Input;
                    acao.Value = "desconto";

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter ip = cmd.Parameters.Add("enderecoip", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
                btnDescontoMax.BackColor = Color.FromArgb(91, 191, 223);
                using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.DescontoMaximo";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter acao = cmd.Parameters.Add("acao", DbType.String);
                    acao.Direction = ParameterDirection.Input;
                    acao.Value = "retirar";

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter ip = cmd.Parameters.Add("enderecoip", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                descontoMax = false;

            }

            MostrarItens();
        }

        private void lblOperador_Click(object sender, EventArgs e)
        {
            MudarOperador();
        }

        private void MudarOperador()
        {
            if (totalTransacao > 0)
            {
                MessageBox.Show("Não é possível troca de operador. Finalize primeiro a venda ou DAV.");
                return;
            }
            FrmLogon logon = new FrmLogon();
            logon.campo = "login";
            logon.lblDescricao.Text = "Caixa bloqueado.";
            logon.txtDescricao.Text = "Digite seu acesso.";
            logon.ShowDialog();

            if (Operador.autorizado)
            {
                lblOperador.Text = GlbVariaveis.glb_Usuario;
                Permissoes.Carregar(GlbVariaveis.glb_Usuario);
            }
            else
            {
                MudarOperador();
            }
        }

        private void pnlEsconder_MouseClick(object sender, MouseEventArgs e)
        {
            string logo = "";

            if (pnlBotoes.Visible == true)
            {
                pnlBotoes.Visible = false;
                pnlTeclado.Visible = false;
                dtgItens.Height = (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 290);
                Posicoes();
                pnlEsconder.Location = new Point(1, 500);
                btTotalizar.Location = new Point(57, 120);

                if (ConfiguracoesECF.styleMetro == false)
                    logo = @"imagens\mostrar.png";
                else
                    logo = @"imagensMetro\mostrar.png";

                pnlEsconder.BackgroundImage = new Bitmap(logo);

            }
            else
            {
                btTotalizar.Location = new Point(57, 350);
                Configuracoes.capturarResolucao();
                //MessageBox.Show(Configuracoes.resolucaoHeight.ToString() + "|" + Configuracoes.resolucaoWidth.ToString());
                //if()
                dtgItens.Height = (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 500);
                dtgItens.Width = (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 370) < 1110 ? (int.Parse(Configuracoes.resolucaoWidth.ToString()) - 370) : 1110;
                pnlBotoes.Visible = true;
                pnlTeclado.Visible = true;
                Posicoes();
                pnlEsconder.Location = new Point(680, (int.Parse(Configuracoes.resolucaoHeight.ToString()) - 190));  // 630

                if (ConfiguracoesECF.styleMetro == false)
                    logo = @"imagens\ocultar.png";
                else
                    logo = @"imagensMetro\ocultar.png";

                pnlEsconder.BackgroundImage = new Bitmap(logo);

            }
    
    
        }

        private void optPED_Click(object sender, EventArgs e)
        {
            if (ConfiguracoesECF.perfil == "Y")
            {
                MessageBox.Show("Não é permitido para o perfil Y");
                return;
            }

            string sql = "SELECT COUNT(1) as qtd from venda WHERE ecffabricacao='" + ConfiguracoesECF.nrFabricacaoECF + "'";

            int? nRegistro = (Int16)Conexao.CriarEntidade().ExecuteStoreQuery<int?>(sql).FirstOrDefault();

            if (nRegistro.Value >= 1)
            {
                MessageBox.Show("Não é possível lançar PED. Já foi emitido cupons na data atual.");
                return;
            }

            ChamarPED();
        }

        private void lblPedidos_Click(object sender, EventArgs e)
        {
            MostrarPedidos();
        }

        private void ChamarNFe()
        {
            if (totalTransacao > 0)
            {
                MessageBox.Show("Não é possível emitir NFe. Finalize primeiro a venda ou DAV.");
                return;
            }

            Venda.ApagarItensFormaPagamento("itenspagamentos");
            FrmNotaFiscal nfe = new FrmNotaFiscal();
            nfe.ShowDialog();
            Venda.ApagarItensFormaPagamento("itenspagamentos");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MostrarPedidos();
        }

        private void lblAnunciar_Click(object sender, EventArgs e)
        {
            DivulgarPromocaoIQCARD.iniciarmostrandoPromocao = true;
            DivulgarPromocaoIQCARD.iniciarmostrandoPedido = false;
            ChamarAnuncio();
        }

        public void ChamarAnuncio()
        {
            if(!Funcoes.VerificarConexaoInternet())
            {
                MessageBox.Show("Sem internet");
                return;
            }
            DivulgarPromocaoIQCARD promo = new DivulgarPromocaoIQCARD();
            promo.ShowDialog();
            VerificarPromocaoAtiva();
        }

        private void imgAnuncio_Click(object sender, EventArgs e)
        {
            try
            {
                ChamarAnuncio();
            }
            catch(Exception)
            {

            }
            
        }

        private void lblInfoCliente_Click(object sender, EventArgs e)
        {
            IndexPontosAcumulados pnlIq = new IndexPontosAcumulados();
                pnlIq.ShowDialog();            
        }

        private void optNFe_Click(object sender, EventArgs e)
        {
            ChamarNFe();
        }

        private void timerAnuncio_Tick(object sender, EventArgs e)
        {
            if (ativarIndoor == true && anuncioHabilitado == true && txtcodProduto.Focused == true)
            {
                timerAnuncio.Stop();
                ativarIndoor = false;
                anuncioIndoor();
                return;
            }
            ativarIndoor = true;
        }

        private void imgSuporte_Click(object sender, EventArgs e)
        {
           
                Suporte suporte = new Suporte();
                suporte.ShowDialog();                
             
        }

        private void dtgItens_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                int coluna = dtgItens.CurrentRow.Index;
                venda.MarcarEntrega(Convert.ToInt32(dtgItens.CurrentRow.Cells["id"].Value));                
                MostrarItens();
                dtgItens.Rows[coluna].Selected = true;
                dtgItens.Focus();
            }

        }

        private void bntKitVenda_Click(object sender, EventArgs e)
        {
            ChamarKitVenda();
        }

        private void pnlAviso_Click(object sender, EventArgs e)
        {
            if(!exportacaoRealizada)
            {
                exportarXmlsAutomatico();
            }

            if (certificadoVencido)
            {

            }
            else
            {
                
            }
        }
        
        private void lblAvisoImportante_Click(object sender, EventArgs e)
        {
            if (!exportacaoRealizada)
            {
                exportarXmlsAutomatico();
            }

            if (certificadoVencido)
            {

            }
            else
            {
                
            }
        }

        public void exportarXmlsAutomatico()
        {

            var now = DateTime.Now;
            var first = new DateTime(now.Year, now.Month, 1);
            var last = first.AddMonths(1).AddDays(-1);

            string serie = int.Parse(ConfiguracoesECF.NFCserie).ToString();

            int NFCPendente = (from n in Conexao.CriarEntidade().contdocs
                               where n.protocolo == "000000000000000"
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               && n.ecfcontadorcupomfiscal.Contains(serie)
                               && n.data >= first
                               && n.data <= last
                               select n.documento).Count();


            if (NFCPendente > 0)
            {
                MessageBox.Show("ATENÇÃO, EXISTEM " + NFCPendente + " CUPONS PENDENTES!");

            }
            else
            {
                siceEntities entidade = Conexao.CriarEntidade();
                var dados = (from n in entidade.senhas
                             where n.operador == GlbVariaveis.glb_Usuario
                             select n).First();

                System.Diagnostics.Process.Start(@"iEFD.exe", GlbVariaveis.glb_filial + " S " + dados.codigo.ToString() + " " + GlbVariaveis.glb_senhaUsuario + " " + "automatico");
            }
        }

        private void pnlProcura_Click(object sender, EventArgs e)
        {
            ChamarAnuncio();
        }

        private void lblProcuraPromocao_Click(object sender, EventArgs e)
        {
            ChamarAnuncio();
        }

        private void lblMarketing_Click(object sender, EventArgs e)
        {
            ChamarAnuncio();
        }

        private void lblVendedor_Click(object sender, EventArgs e)
        {
            ChamarVendedor();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MudarOperador();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            MudarOperador();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AlterarFormaVenda();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DivulgarPromocaoIQCARD.iniciarmostrandoPromocao = true;
            DivulgarPromocaoIQCARD.iniciarmostrandoPedido = false;
            ChamarAnuncio();
        }



        private void label1_Click(object sender, EventArgs e)
        {
            ChamarVendedor();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChamarVendedor();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            ChamarAnuncio();
        }

        private void lblUsersOnline_Click(object sender, EventArgs e)
        {
            ChamarAnuncio();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            MostrarPedidos();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            MostrarPedidos();
        }

        private void _pdv_MouseMove(object sender, MouseEventArgs e)
        {
            ativarIndoor = false;
            timerAnuncio.Start();

        }

        private void _pdv_MouseHover(object sender, EventArgs e)
        {
           
            
        }

        private void dtgItens_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
           

        }

        private void dtgItens_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
          
        }

        private void dtgItens_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

        }

        private void _pdv_MouseClick(object sender, MouseEventArgs e)
        {
            ativarIndoor = false;
        }

        private void btnAnuncio_Click(object sender, EventArgs e)
        {
            FrmAnuncio anuncio = new FrmAnuncio();
            anuncio.ShowDialog();
        }

        private void btnAnuncio_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ChamarKitVenda()
        {
            FrmKitVenda.kitNumero = 0;
            FrmKitVenda frmVend = new FrmKitVenda();
            frmVend.ShowDialog();            
            Application.DoEvents();
            txtcodProduto.Focus();
            if (FrmKitVenda.kitNumero > 0)
            {
                var dados = (from n in Conexao.CriarEntidade().vendapadronizada
                             where n.numero == FrmKitVenda.kitNumero
                             select n);

                foreach (var item in dados)
                {
                    Produtos prd = new Produtos();
                    prd.ProcurarCodigo(item.codigo, GlbVariaveis.glb_filial);                    
                    txtcodProduto.Text = item.codigo;
                    txtQtd.Text = item.quantidade.ToString();
                    txtDescontoPercItem.Text = item.descontoitem.ToString();
                    quantidade = Convert.ToDecimal(txtQtd.Text);
                    txtPreco.Text = prd.preco.ToString();
                    Lancar();
                }
            }
        

        }

        private void indoor_Click(object sender, EventArgs e)
        {
            try
            {
                FrmAnuncio anuncio = new FrmAnuncio();
                anuncio.ShowDialog();
            }
            catch(Exception)
            {

            }
        }

        private void dtgItens_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ConfiguracoesECF.pdv == false /*&& Configuracoes.gerarTransferenciaVenda == false*/)
            {
                string codigo = dtgItens.CurrentRow.Cells["codigo"].Value.ToString();
                string descricao = dtgItens.CurrentRow.Cells["descricao"].Value.ToString();
                int nrcontrole = int.Parse(dtgItens.CurrentRow.Cells["nrcontrole"].Value.ToString());

                string SQL = "SELECT count(1) FROM transfvendatemp WHERE codigo = '"+codigo+"' AND ip = '"+GlbVariaveis.glb_IP+"' AND filialdestino = '"+GlbVariaveis.glb_filial+"'";

                siceEntities entidade = Conexao.CriarEntidade();
                int Qtd = entidade.ExecuteStoreQuery<int>(SQL).First();

                if (Qtd > 0)
                {
                    MessageBox.Show("Não é possivel alterar um item de transferencia automatica!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.None);
                    return;
                }

                if (dtgItens.CurrentRow.Cells["cancelado"].Value.ToString() == "" || dtgItens.CurrentRow.Cells["cancelado"].Value.ToString() == " ")
                {
                    FrmAltQtdDAV obJFrmAltQtdDAV = new FrmAltQtdDAV(codigo, descricao, nrcontrole);
                    obJFrmAltQtdDAV.ShowDialog();
                    MostrarItens();
                }
                else
                {
                    MessageBox.Show("Item Já Cancelado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
        }


        private bool verificaDadosR02()
        {
            if (GlbVariaveis.glb_clienteDAV == false)
                return true;

            if (Conexao.ConexaoOnline() == false)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;

            if (ConfiguracoesECF.idECF == 0)
                return true;

            if (ConfiguracoesECF.idNFC > 0)
                return true;

            

            if (FuncoesECF.VerificarR02("CRO") == false)
            {
                MessageBox.Show("Erro no CRO", "Atenção",MessageBoxButtons.OK,MessageBoxIcon.Error);
                desativaFuncoesCRZCRO(true);
                return false;
            }
            

            if (FuncoesECF.VerificarR02("CRZ") == false)
            {
                MessageBox.Show("Erro no CRZ", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                desativaFuncoesCRZCRO(true);
                return false;
            }

            if (FuncoesECF.VerificarR02("VendaBruta") == false)
            {
                MessageBox.Show("Erro na Venda Bruta", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                desativaFuncoesCRZCRO(true);
                return false;
            }

            return true;
        }


        /** Botão de próxima sugestão */
        int iSugestao = 0;
        private void btnNextSugestao_Click(object sender, EventArgs e)
        {
            var itemSelecionado = sugestao[iSugestao];

            lblProdSugerido.Text = itemSelecionado.descricao;

            porcentagemSugestao.Text = (Double.Parse(itemSelecionado.relevancia) * 100) + "%";
            lblProdSugerido.Text = " também incluíram o produto " + itemSelecionado.descricao + " - Cód.: " + itemSelecionado.codigo;
            codP = itemSelecionado.codigo;

            if (iSugestao == sugestao.Count - 1)
                iSugestao = 0;
            else
                iSugestao++;

        }

        /** Botão de sugestão anterior */
        private void btnPrevSugestao_Click(object sender, EventArgs e)
        {
            if (iSugestao == 0)
                iSugestao = sugestao.Count - 1;
            else
                iSugestao--;

            var itemSelecionado = sugestao[iSugestao];

            lblProdSugerido.Text = itemSelecionado.descricao;

            porcentagemSugestao.Text = (Double.Parse(itemSelecionado.relevancia) * 100) + "%";
            lblProdSugerido.Text = " também incluíram o produto " + itemSelecionado.descricao + " - Cód.: " + itemSelecionado.codigo;
            codP = itemSelecionado.codigo;

           
        }

        // Adicionar produto sugerido
        private void button6_Click_1(object sender, EventArgs e)
        {
            txtcodProduto.Text = codP;
            pnlImpulsionado.Visible = false;
            txtcodProduto.Focus();
        }

        private void desativaFuncoesCRZCRO(bool ativar)
        {
            if (ativar == true)
            {
                btnSuprimento.Enabled = false;
                btnSangria.Enabled = false;
                bntKitVenda.Enabled = false;
                btnDescEspecial.Enabled = false;
                btnPreVenda.Enabled = false;
                btnDAV.Enabled = false;
                btnDevolucao.Enabled = false;
                btnCancelarCupom.Enabled = false;
                btnEncerrar.Enabled = false;
            }
            else
            {
                btnSuprimento.Enabled = true;
                btnSangria.Enabled = true;
                bntKitVenda.Enabled = true;
                btnDescEspecial.Enabled = true;
                btnPreVenda.Enabled = true;
                btnDAV.Enabled = true;
                btnDevolucao.Enabled = true;
                btnCancelarCupom.Enabled = true;
                btnEncerrar.Enabled = true;
            }
        }

        private void btnFecharSugestoes_Click(object sender, EventArgs e)
        {
            pnlImpulsionado.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmConferencia objConferencia = new frmConferencia();
            objConferencia.ShowDialog();
        }

        public void MostrarPedidos()
        {
            if (txtcodProduto.Text.StartsWith("#iqcard"))
                {
                IndexPontosAcumulados painel = new IndexPontosAcumulados();
                painel.ShowDialog();
                return;
            }

            if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard) || txtcodProduto.Text.StartsWith("#"))
            {
               
                ChamarAnuncio();
                //MessageBox.Show("Crie sua conta no IQCARD e comece a fidelizar e interagir com mais consumidores.");
                //TokenIQCARD token = new TokenIQCARD();
                //token.ShowDialog();
                return;
            }
            IndexPedidoIQCard ped = new IndexPedidoIQCard();
            ped.ShowDialog();
        }

        public void focarPDV()
        {
            this.BringToFront();
            this.Focus();
            this.Activate();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            DivulgarPromocaoIQCARD.iniciarmostrandoPromocao = true;
            DivulgarPromocaoIQCARD.iniciarmostrandoPedido = false;
            ChamarAnuncio();
        }

        public void verificarVersao()
        {
            try
            {
                if (ConfiguracoesECF.NFC == true)
                {
                    int versao = 0;
                    System.Diagnostics.FileVersionInfo fvi;
                    fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(@"C:\iqsistemas\SICENFC-e\SICENFCe.exe");

                    if (fvi.FileVersion.Length > 2)
                        versao = int.Parse(fvi.FileVersion.ToString().Replace(".", "").Substring(0, 2));

                    if (versao < 77)
                    {
                        pnlAviso.Visible = true;
                        lblAvisoImportante.Text = "Sua versão do SICENFC-e está desatualizada. Atualize para evitar inconsistências fiscais!";
                    }
                }
                

            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }
        }

        private void contadorProcuraPromocao_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            IqCard iqcard = new IqCard();
            iqcard.ContadorProcuraPromocao();
        }

        public void anuncioIndoor()
        {
            try
            {
                FrmAnuncio anuncio = new FrmAnuncio();
                anuncio.ShowDialog();
            }
            catch (Exception e)
            {

            }
        }

        public void sugestaoVenda(/*String codigo*/)
        {
            string codigo = (from n in Conexao.CriarEntidade().vendas where (n.id == GlbVariaveis.glb_IP && n.codigofilial == GlbVariaveis.glb_filial) select n.codigo).Max();


            var entidade = Conexao.CriarEntidade();
            var sql2 = "SELECT codigo FROM vendas WHERE codigofilial = '" + GlbVariaveis.glb_filial + "' and id = '" + GlbVariaveis.glb_IP + "' ORDER BY(inc) desc limit 1;";
            var result = entidade.ExecuteStoreQuery<String>(sql2).FirstOrDefault();

            var sql = "CALL produtosSugeridos('" + result + "', 'ler', " + DateTime.Today.ToString("yyyy") + ", '" + GlbVariaveis.glb_filial +  "');";

            sugestao = entidade.ExecuteStoreQuery<sugestaoProdutos>(sql).ToList();

            this.Invoke((MethodInvoker)delegate
            {
                if (dtgItens.Rows.Count > 0 && ConfiguracoesECF.pdv == false)
                {
                    if (sugestao.Count() > 0)
                    {
                        pnlImpulsionado.Visible = true;
                    }
                    else
                    {
                        pnlImpulsionado.Visible = false;
                        return;
                    }

                    porcentagemSugestao.Text = (Double.Parse(sugestao.FirstOrDefault().relevancia) * 100) + "%"; 
                    lblProdSugerido.Text = " também incluíram o produto " + sugestao.FirstOrDefault().descricao + " - Cód.: " + sugestao.FirstOrDefault().codigo;

                    codP = sugestao.FirstOrDefault().codigo;
                }
            });

            /*if(sugestao.Count() > 0)
            {
                pnlImpulsionado.Visible = true;
            }
            else
            {
                pnlImpulsionado.Visible = false;
                return;
            }

            porcentagemSugestao.Text = (Double.Parse(sugestao.FirstOrDefault().relevancia) * 100) + "%";
            lblProdSugerido.Text = " também incluíram o produto " + sugestao.FirstOrDefault().descricao + " - Cód.: " + sugestao.FirstOrDefault().codigo;*/
        }

        public void mostarSugestao()
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (dtgItens.Rows.Count > 1 && ConfiguracoesECF.pdv)
                {
                    if (sugestao.Count() > 0)
                    {
                        pnlImpulsionado.Visible = true;
                    }
                    else
                    {
                        pnlImpulsionado.Visible = false;
                        return;
                    }

                    porcentagemSugestao.Text = (Double.Parse(sugestao.FirstOrDefault().relevancia) * 100) + "%";
                    lblProdSugerido.Text = " também incluíram o produto " + sugestao.FirstOrDefault().descricao + " - Cód.: " + sugestao.FirstOrDefault().codigo;
                }
            });
        }

        public void mensagemExportacaoFiscal()
        {
            try
            {
                int dia = DateTime.Now.Day;

                if (!exportacaoRealizada)
                {                        
                    pnlAviso.Visible = true;
                    lblAvisoImportante.Text = "DESEJA REALIZAR A EXPORTAÇÃO DE XMLS AGORA?";
                }             
            }
            catch(Exception)
            {

            }
        }

        public void verificarMensagem()
        {
            certificadoDigitalVencido();
            
        }   

        public bool certificadoDigitalVencido()
        {
            try
            {
                var sql = "SELECT DATEDIFF(CertificadoDataVencimento, CURDATE()) FROM configfinanc WHERE codigofilial = '" + GlbVariaveis.glb_filial + "';";
                var entidade = Conexao.CriarEntidade();
                var dia = entidade.ExecuteStoreQuery<Int32>(sql).FirstOrDefault();
                if (15 >= dia)
                {
                    certificadoVencido = true;
                    mensagemCertificadoVencido(dia);
                    return true;
                }
                else
                {
                    certificadoVencido = false;
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void mensagemCertificadoVencido(int dia)
        {
                pnlAviso.Visible = true;
                pnlAviso.BackColor = Color.Red;
                lblAvisoImportante.ForeColor = Color.White;
                lblAvisoImportante.Font = new Font("Arial", 16, FontStyle.Bold);

            if (dia < 1)
            {
                lblAvisoImportante.Text = "Atenção, Seu Certificado Digital Expirou!";
            }
            else if(dia == 1)
            {
                lblAvisoImportante.Text = "Atenção, Seu Certificado Digital Expira Em " + dia.ToString() + " Dia!";
            }
            else
            {
                lblAvisoImportante.Text = "Atenção, Seu Certificado Digital Expira Em " + dia.ToString() + " Dias!";
            }
        }
    }
}

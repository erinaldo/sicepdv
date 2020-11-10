using System;
using System.Windows.Forms;
using Bematech.Fiscal.ECF;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace SICEpdv
{        
    public partial class _pdv: Form
    {
        internal static ImpressoraFiscal Printer = ImpressoraFiscal.Construir();            
        TeclaNumerico teclado = new TeclaNumerico();
        UCCartoes cartoes = new UCCartoes();
        ucClientePdv clientePDV = new ucClientePdv();        
        Panel pnlCartoes = new Panel();
        Panel pnlCliente = new Panel();
        Produtos produto = new Produtos();

                                    
        private decimal quantidade = 1.00M,preco;        
        private decimal totalVenda,desconto =0,dinheiro =0,cartao =0,cheque =0,crediario =0,restante = 0;        
        private string controle;
        private string tecla;           
        private string tipoDesconto = "%";
        private decimal guardaDesconto = 0;
        private decimal descontoMaximoItem = Configuracoes.descontoMaxVenda;
                 
        public _pdv()
        {            
            BringToFront();
            InitializeComponent();


            
            btTotalizar.Click += (objeto, evento) => Totalizar();
            btnSuprimento.Click += (objeto, evento) => ChamarSuprimento();
            btnSangria.Click += (objeto, evento) => ChamarSangria();
            btnProdutos.Click += (objeto, evento) =>
                {
                    ChamarProdutos();
                    txtcodProduto.Focus();
                };


            /// Evento na Entra do Controle das Formas de Pagamento
            #region txtCodProduto
            txtcodProduto.Enter += (objeto, evento) => controle = "txtcodProduto";
            txtcodProduto.KeyDown += (objeto, evento) =>
            {
                EntraCodigo(evento);

                if (evento.KeyValue == 33)
                {
                    Totalizar();
                }
            };
            #endregion
            #region Desconto

            txtDesconto.Enter += (objeto, evento) => controle = "txtDesconto";
            txtDesconto.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumeros(objeto, evento);
                
                if (evento.KeyChar == 27) btCancelarPg_Click(objeto, evento);
            };
            txtDesconto.KeyDown += (objeto, evento) =>
            {
                
                if (evento.KeyValue == 13)
                {
                    txtDesconto.Text = Funcoes.FormatarDecimal(txtDesconto.Text);
                    Desconto();
                    evento.SuppressKeyPress = true;
                }
            };
            #endregion                        
            #region Dinheiro
            txtDinheiro.KeyPress += (objeto, evento) =>
                {
                    Funcoes.DigitarNumeros(objeto,evento);
                    if (evento.KeyChar == 27) btCancelarPg_Click(objeto, evento); 
                };
            txtDinheiro.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyValue == 13)
                    {
                        txtDinheiro.Text = Funcoes.FormatarDecimal(txtDinheiro.Text);
                        Dinheiro();
                        evento.SuppressKeyPress = true;
                    }
                };
            txtDinheiro.Enter += (objeto, evento) =>
            {
                txtDinheiro.Text = String.Format("{0:N2}", restante);
                controle = "txtDinheiro";
                VerificaPagamento();
                txtDinheiro.Focus();
                txtDinheiro.SelectAll();
            };
            #endregion            
            #region Cartao
            txtCartao.KeyPress += (objeto, evento) =>
                {
                    Funcoes.DigitarNumeros(objeto, evento);
                    if (evento.KeyChar == 27) btCancelarPg_Click(objeto, evento);
                    if (evento.KeyChar != 48) evento.Handled = true;
                };
            txtCartao.KeyDown += (objeto,evento) =>
                {
                    if (evento.KeyValue == 13)
                    {
                        txtCartao.Text = Funcoes.FormatarDecimal(txtCartao.Text);
                        MostraCartoes();
                    }
                };
            
           
            txtCartao.Enter += (objeto, evento) =>
            {
                
                txtCartao.Text = String.Format("{0:N2}", restante);
                controle = "txtCartao";
                VerificaPagamento();
                txtCartao.SelectAll();
            };
            #endregion            
            #region Crediario
            txtCrediario.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumeros(objeto, evento);                      
            };
            txtCrediario.Enter += (objeto, evento) =>
                {                    
                    txtCrediario.Text = string.Format("{0:N2}", restante);
                    controle = "txtCrediario";
                    VerificaPagamento();
                    txtCrediario.SelectAll();
                };

            txtCrediario.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyValue == 13 )
                    {
                        txtCrediario.Text = Funcoes.FormatarDecimal(txtCrediario.Text);
                        MostraClienteCR();                        
                        evento.SuppressKeyPress = true;
                    };
                };
            #endregion
            #region Cheque
            txtCheque.KeyPress += (objeto, evento) =>
                {
                    Funcoes.DigitarNumeros(objeto, evento);
                };
            txtCheque.Enter += (objeto, evento) =>
                {
                    txtCheque.Text = string.Format("{0:n2}", restante);
                    controle = "txtCheque";
                    VerificaPagamento();
                    txtCheque.SelectAll();
                };

            txtCheque.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyValue == 13)
                    {
                        txtCheque.Text = Funcoes.FormatarDecimal(txtCheque.Text);
                        MostraClienteCH();
                        evento.SuppressKeyPress = true;
                    }
                };

            #endregion
          
            btnCliente.Click += (objeto, evento) =>
                {
                    MostraConsumidor();
                };
            #region Tecla Desconto Item
            txtDescontoPercItem.KeyPress += (objeto, evento) =>
                {
                    Funcoes.DigitarNumeros(objeto, evento);
                };
            txtDescontoPercItem.Enter += (objeto, evento) => controle = ActiveControl.Name;
            txtDescontoPercItem.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyValue == 13)
                    {
                        if (txtDescontoPercItem.Text=="") txtDescontoPercItem.Text="0";
                        if (Convert.ToDecimal(txtDescontoPercItem.Text) > 0 )
                            Lancar();
                            
                        SendKeys.Send("{TAB}");
                        evento.SuppressKeyPress = true;
                    };
                };

            #endregion

            #region Tecla Preço de Venda
            txtPreco.Enter += (objeto, evento) => controle = ActiveControl.Name;
            txtPreco.KeyPress += (objeto, evento) => Funcoes.DigitarNumeros(objeto, evento);
            txtPreco.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyValue == 13)
                    {
                        Lancar();
                        evento.SuppressKeyPress = true;
                    }
                };

            #endregion
        }

        private void MostraConsumidor()
        {
            clientePDV.Width = 360;
            clientePDV.Height = 230;
            pnlCliente.Visible = true;
            pnlCliente.BringToFront();
            clientePDV.txtIdCliente.Enabled = true;
            clientePDV.tipoPagamento = "";            
            clientePDV.pnlParcelamentoCR.Visible = false;
            clientePDV.txtIdCliente.Focus();
            clientePDV.pnlCheque.Visible = false;
            pnlCliente.BringToFront();
            DesativarBotoes();
        }

        private void MostraClienteCR()
        {            
            if (Convert.ToDecimal(txtCrediario.Text) <= 0 && controle=="txtCrediario")
            {
                txtCheque.Focus();
                return;
            }
            if (Convert.ToDecimal(txtCheque.Text) <= 0 && controle == "txtCheque")
            {
                txtCartao.Focus();
                return;
            }            
            clientePDV.Width = 360;
            clientePDV.Height = 230;
            pnlCliente.Height = clientePDV.Height + 2;
            pnlCliente.Width = clientePDV.Width + 2 ;
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
            clientePDV.tipoPagamento = "CH";            
            clientePDV.Height = 400;
            pnlCliente.Width = 360;            
            pnlCliente.Height = 401;
            pnlCliente.Location = new System.Drawing.Point(80, 150);            
            clientePDV.pnlParcelamentoCR.Visible = false;                        
            clientePDV.txtValorIndCH.Text = String.Format("{0:n2}", Convert.ToDecimal(txtCheque.Text));
            pnlCliente.Refresh();
            if (clientePDV.idCliente != 0)
            {
                /// O cliente tem que ser o mesmo do crediário
                if (crediario!=0)
                clientePDV.txtIdCliente.Enabled = false;
                clientePDV.pnlCheque.Visible = true;
                clientePDV.txtCodBanco.Focus();
            }
        }

        private void EntraCodigo(KeyEventArgs e)
        {            
            switch (e.KeyCode)
            {
                case Keys.F2:
                    ChamarProdutos();
                    break;
                case Keys.F5:
                    ChamarSangria();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.F10:
                    ChamarPreVenda();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Multiply:                   
                    x();                    
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Return:
                    if (Configuracoes.mudarPrecoVenda)
                        MudarPreco();
                    else
                        Lancar();
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void x()
        {
            lblQtd.Text = txtcodProduto.Text + " X ";
            try
            {
                quantidade = Math.Round( Convert.ToDecimal(txtcodProduto.Text),3);
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
                    if (txtDesconto.Enabled==false)
                    SendKeys.Send("{TAB}");                    
                    break;
                case "txtIdCliente":
                    clientePDV.ProcuraCliente();
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
                    ctls[0].Focus() ;                                       
                    SendKeys.Send("{TAB}");                    
                    break;
            }            
            
        }

        private void Dinheiro()
        {            
            try
            {
                if (Convert.ToDecimal(txtDinheiro.Text) == 0)
                {
                    txtCrediario.Focus();
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
                this.Enabled = false;
                DesativaDesconto();

                // Construindo a Classe e Atribuindo os Valores   
                
                Venda venda = ConstruirVenda();
                if (!venda.EfetuarPagamento("Venda", "DH", Convert.ToDecimal(txtDinheiro.Text), 0, 0, 0, DateTime.Now, 0, null)) return;
                dinheiro = Convert.ToDecimal(txtDinheiro.Text);
                txtDinheiro.Text = string.Format("{0:n2}", dinheiro);
                restante -= dinheiro;
                if (restante < 0) restante = 0;

                if (dinheiro > 0) txtDinheiro.Enabled = false;

                if (venda.vendaFinalizada)
                { 
                    AtivarBotoes();
                    limparDados();
                    zerarVariavies();
                }

            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.Enabled = true;
            }

            txtCrediario.Focus();
        }

        private Venda ConstruirVenda()
        {
            Venda venda = new Venda();
            venda.dpFinanceiro = "Venda";
            venda.valorBruto = totalVenda;
            venda.desconto = desconto;
            venda.IdCliente = clientePDV.idCliente;            
            venda.valorLiquido = Math.Round(totalVenda - desconto, 2);  

            return venda;
        }
        private void MostraCartoes()
        {
            /// Cartoes sempre será a última forma de pagamento
            /// pois o TEF precisa da finalização do Cupom Fiscal para puder 
            /// imprimir o comprovante de pagamento
            /// 
            if (Convert.ToDecimal(txtCartao.Text) <= 0) return;
            txtCartao.Text = string.Format("{0:n2}", restante);
            pnlCartoes.Visible = true;
            pnlPagamento.Enabled = false;
            cartoes.txtParcelamentoCA.Text = "1";
            cartoes.idCartao = 0;
            cartoes.txtValorIndCA.Text = txtCartao.Text;
            cartoes.DesTravaCartoes();
            cartoes.txtValorIndCA.Focus();
            cartoes.pnlValorCA.Enabled = false;
        }

        private void Crediario()
        {

            try
            {
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
                this.Enabled = false;
                if (Convert.ToDecimal(txtCrediario.Text) > restante)
                {
                    txtCrediario.Text = string.Format("{0:n2}", restante);
                    txtCrediario.Focus();
                    return;
                };
                DesativaDesconto();

                Venda venda = ConstruirVenda();
                
                if (!venda.EfetuarPagamento("Venda", "CR", Convert.ToDecimal(txtCrediario.Text), clientePDV.idCliente, 0, Convert.ToInt16(clientePDV.txtParcelamentoCR.Text), clientePDV.vencimentoCR.Value, Convert.ToUInt16(clientePDV.txtIntervaloCR.Text), null)) return;
                crediario = Convert.ToDecimal(txtCrediario.Text);
                restante -= crediario;
                txtCrediario.Text = string.Format("{0:n2}", crediario);
                if (crediario > 0) txtCrediario.Enabled = false;

                if (venda.vendaFinalizada)
                {
                    AtivarBotoes();
                    limparDados();
                    zerarVariavies();
                }
                pnlCliente.Visible = false;
                pnlPagamento.Enabled = true;
            }
            catch (Exception erro)
            {                
                MessageBox.Show(erro.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                
            }
            finally
            {
                this.Enabled = true;
            }              
            txtCheque.Focus();
        }

        private void Cartao()
        {
            try
            {
                if (Convert.ToDecimal(cartoes.txtValorIndCA.Text) == 0) return;
            }
            catch
            {
                txtCartao.Text = string.Format("{0:n2}", cartao);
                return;
            }
            
            try
            {
                this.Enabled = false;
                if (Convert.ToDecimal(cartoes.txtValorIndCA.Text) > restante)
                {
                    cartoes.txtValorIndCA.Text = string.Format("{0:n2}", restante);
                    cartoes.txtValorIndCA.Focus();                     
                    return;
                };
                DesativaDesconto();

                
                Venda venda = ConstruirVenda();

                if(!venda.EfetuarPagamento("Venda","CA", Convert.ToDecimal(cartoes.txtValorIndCA.Text),0,cartoes.idCartao,Convert.ToInt16(cartoes.txtParcelamentoCA.Text),GlbVariaveis.Sys_Data.AddMonths(1),30,null) ) return;
                cartao = Convert.ToDecimal(cartoes.txtValorIndCA.Text);
                restante -= cartao;
                txtCartao.Text = string.Format("{0:n2}", cartao);
                cartoes.txtValorIndCA.Text = string.Format("{0:n2}", restante);
                cartoes.DesTravaCartoes();
                if (cartao > 0) txtCartao.Enabled = false;

                if (venda.vendaFinalizada)
                {
                    AtivarBotoes();
                    limparDados();
                    zerarVariavies();
                }
                cartoes.txtValorIndCA.Focus();                
            }
            catch 
            {
                //MessageBox.Show(erro.Message, "Atenção!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.Enabled = true;
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
                if (Convert.ToDecimal(clientePDV.txtValorIndCH.Text) == 0) return;
            }
            catch
            {
                txtCheque.Text = string.Format("{0:n2}",cheque);
                return;
            }

            try
            {
                this.Enabled = false;
                if (Convert.ToDecimal(clientePDV.txtValorIndCH.Text) > restante)
                {
                    clientePDV.txtValorIndCH.Text = string.Format("{0:n2}", restante);
                    clientePDV.txtValorIndCH.Focus();
                    return;
                };
                DesativaDesconto();                
                
                Venda venda = ConstruirVenda();

                if (!venda.EfetuarPagamento("Venda", "CH", Convert.ToDecimal(clientePDV.txtValorIndCH.Text), clientePDV.idCliente, 0, Convert.ToInt16(clientePDV.txtParcelamentoCH.Text), clientePDV.vencimentoCH.Value,Convert.ToInt16(clientePDV.txtIntervaloCH.Text),clientePDV.dadoCheque )) return;
                cheque = Convert.ToDecimal(clientePDV.txtValorIndCH.Text);
                restante -= cheque;               
                clientePDV.txtValorIndCH.Text = string.Format("{0:n2}", restante);                
                if (cheque > 0) txtCheque.Enabled = false;

                if (venda.vendaFinalizada)
                {
                    AtivarBotoes();
                    limparDados();
                    zerarVariavies();
                }
                if (Convert.ToDecimal(txtCheque.Text) < cheque)
                    clientePDV.txtValorIndCH.Focus();
                else
                {
                    txtCheque.Text = string.Format("{0:n2}", cheque);
                    this.Enabled = true;
                    pnlCliente.Visible = false;
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
                this.Enabled = true;
            }
        }

        private void Lancar()
        {            
            decimal qtdDisponivel = 0;
            decimal descontoPercItem = 0;
            decimal descontoValorItem = 0;
            decimal acrescimoPerc = 0;
            decimal acrescimoValor = 0;
            Venda venda = new Venda();
            venda.cpfCnpjConsumidor = clientePDV.cpfcnpjCliente;
            venda.nomeConsumidor = clientePDV.txtConsumidor.Text;            
            venda.endComsumidor = clientePDV.txtEndConsumidor.Text;            
            venda.vendedor = "000";
            
            try
            {  
                var dadosPrd = produto.ProcurarCodigo(txtcodProduto.Text, GlbVariaveis.glb_filial);

                if (txtPreco.Text == "") txtPreco.Text = "0";
                if (txtDescontoPercItem.Text == "") txtDescontoPercItem.Text = "0";
                           
                lblDescricaoPrd.Text = produto.descricao;
                qtdDisponivel = produto.quantidadeDisponivel;
                Application.DoEvents();
                
                descontoPercItem = produto.descontoPromocao;
                preco = produto.preco-(produto.preco*(produto.preco*descontoPercItem/100)) ;

                if (Configuracoes.mudarPrecoVenda)
                {
                    if (Convert.ToDecimal(txtDescontoPercItem.Text) > 0)
                    {
                        txtDescontoPercItem.Text = Funcoes.FormatarDecimal(txtDescontoPercItem.Text);
                        var precoComDesconto = produto.preco * (Convert.ToDecimal(txtDescontoPercItem.Text) / 100);
                        txtPreco.Text = string.Format("{0:n2}", precoComDesconto);
                    }

                    preco = Convert.ToDecimal(txtPreco.Text);
                    descontoPercItem = Convert.ToDecimal(txtDescontoPercItem.Text);
                };                
                // Aqui pega a quantidade se o item foi da balança                
                if (produto.situacao == "Item da Balança")
                {                   
                    quantidade = Convert.ToDecimal(String.Format("{0:n3}", txtcodProduto.Text.Substring(8, 5))) / preco / 100;
                }

                venda.InserirItem(produto.codigo, produto.descricao, qtdDisponivel, quantidade, preco,
                    (decimal)produto.preco, produto.unidade, descontoPercItem, descontoValorItem, "000", Convert.ToInt16(produto.icms), produto.tributacao, 0);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Restrição ao Inserir Item: " + erro.Message,"Atenção",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            finally
            {
                MostrarItens();
                totalVenda = venda.SomaItens();
                txtTotal.Text = String.Format("{0:n2}", totalVenda);
                limparDados();
            }
        }

        private void limparDados()
        {
            quantidade = 1.00M;                        
            txtcodProduto.Text = "";
            lblQtd.Text = "1 X";
            preco = 0;
            txtPreco.Text = "";
            txtDescontoPercItem.Text = "";
            pnlCartoes.Visible = false;
            grpPreco.Visible = false;
            txtcodProduto.Focus();           
        }

        private void zerarVariavies()
        {
            // Crediário e Cheques
            clientePDV.LimparCampos();            
            cartoes.idCartao = 0;


            dinheiro = 0;
            cartao = 0;
            crediario = 0;
            cheque = 0;
            txtDesconto.Text = "0,00";
            txtDinheiro.Text = "0,00";
            txtCartao.Text = "0,00";
            txtCheque.Text = "0,00";
            txtCrediario.Text = "0,00";
            pnlCliente.Visible = false;

            this.Enabled = true;
            txtcodProduto.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void ExcluirItem(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
                return;

            try
            {
                Venda venda = new Venda();
                venda.ExcluirItem(Convert.ToInt32(dataGridView1.CurrentRow.Cells["nrcontrole"].Value) );                
                MostrarItens();
                // dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.CurrentRow.Index]);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível excluir o item :" + erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                 
            }                        
        }

        private void PreencheCampo()
        {
            Control[] ctls = this.Controls.Find(controle, true);

            if (ctls[0] is Button)
                return;

            TextBox txtBox = ctls[0] as TextBox;
            if (txtBox.Enabled == false )
                return;
            
            switch (controle)
            {
                case "txtDinheiro":
                    if (Convert.ToDecimal(txtDinheiro.Text+0) == restante)
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
                if (txtBox.Text.Trim().Length>=txtBox.MaxLength)
                    return;
                txtBox.Text += this.tecla;
            };          
        }

        private void _pdv_Shown(object sender, EventArgs e)
        {                  
            MostrarItens();
            txtcodProduto.Focus();
        }

        private void MostrarItens()
        {            
            Venda venda = new Venda();
            dataGridView1.DataSource = venda.SelectionaItensVenda();
            dataGridView1.Focus();
            totalVenda = venda.SomaItens();
            txtTotal.Text = String.Format("{0:n2}", totalVenda);

            if (dataGridView1.Rows.Count > 0)
                dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0];
            txtcodProduto.Focus();   
        }

        private void btProximo_Click(object sender, EventArgs e)
        {            
            if (dataGridView1.Rows.Count == 0 || dataGridView1.CurrentRow.Index + 1 >= dataGridView1.Rows.Count)
            {
                return;
            };
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.CurrentRow.Index + 1].Cells[0];
        }

        private void btAnterior_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 || dataGridView1.CurrentRow.Index - 1 <=-1)
            {
                return;
            };
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.CurrentRow.Index - 1].Cells[0];           
        }

        private void btCancelarPg_Click(object sender, EventArgs e)
        {
            
            pnlPagamento.Visible = false;
            txtDinheiro.Text = "0.00";
            AtivarBotoes();
            txtcodProduto.Focus();
            pnlCartoes.Visible = false;
        }

        private void DesativarBotoes()
        {
            grpBotoes.Enabled = false;
            txtcodProduto.Enabled = false;
            btnExcluir.Enabled = false;
            btTotalizar.Enabled = false;
            btnCliente.Enabled = false;
        }

        private void AtivarBotoes()
        {
            grpBotoes.Enabled = true;

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

        private void Totalizar()
        {
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

            
            if (totalVenda == 0) return;
            /// Ativar Campos antes de iniciar a totalização
            grpBotoes.Enabled = false;
            txtDinheiro.Enabled = true;
            txtCartao.Enabled = true;
            txtCrediario.Enabled = true;
            txtCheque.Enabled = true; 
            btTotalizar.Enabled = false;            
            pnlPagamento.Enabled = true;
            pnlPagamento.Visible = true;

            ///Zerar Variavéis antes da totalização
            restante = totalVenda;
            dinheiro = 0;
            cartao = 0;
            crediario = 0;
            cheque = 0;
            txtDinheiro.Text = "0,00";
            txtCartao.Text = "0,00";
            txtCrediario.Text = "0,00";            
            DesativarBotoes();            
            AtivaDesconto();
            clientePDV.vencimentoCR.Value = GlbVariaveis.Sys_Data.AddMonths(1);
            clientePDV.vencimentoCH.Value = GlbVariaveis.Sys_Data;
            txtDinheiro.Text = string.Format("{0:n2}", totalVenda);

        }

        private void AtivaDesconto()
        {
            txtDesconto.Text = "0,00";
            tipoDesconto = "%";
            btDesconto.Enabled = true;
            txtDesconto.Enabled = true;            
            txtDesconto.Focus();
            if (!Permissoes.descontoFinalizacao) DesativaDesconto();
        }

        private void _pdv_Load(object sender, EventArgs e)
        {
            lblDataHora.Text = string.Format("{0: dd/MM/yyyy hh:mm}", DateTime.Now);
            lblOperador.Text = GlbVariaveis.glb_Usuario.ToUpper();
            //? VerificarGaveta();
            VerificaFuncoesFiscais();
            FuncoesECF fecf = new FuncoesECF();
            fecf.VerificaStatusTEF();
            /// Construindo o Teclado
            /// 
            teclado.TabStop = false;
            pnlTeclado.Controls.Add(teclado);                        
            this.teclado.clickBotao += new TeclaNumerico.ClicarNoBotao(DelegateTeclado);

            ///Contruindo o Painel para mostrar os Cartoes
            ///            
            pnlCartoes.Location = new System.Drawing.Point(120,180);
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
            clientePDV.Width = 360;
            clientePDV.Height = 230;            
            pnlCliente.Width = clientePDV.Width + 2; ;
            pnlCliente.Height = clientePDV.Height + 2;
            pnlCliente.Visible = false;            
            pnlCliente.Controls.Add(clientePDV);
            pnlCliente.BringToFront();
            pnlCliente.Visible = false;
            pnlCliente.BringToFront();
            this.Controls.Add(pnlCliente);
            this.clientePDV.EntraControle += new ucClientePdv.Controle(DelegateCliente);
        }

        private void VerificaFuncoesFiscais()
        {
            if (ConfiguracoesECF.modeloECF == 0 || ConfiguracoesECF.prevenda)
            {
                //btnCliente.Enabled = false;
                btnSuprimento.Enabled = false;
                btnSangria.Enabled = false;
                btnAdmTef.Enabled = false;
                btnPreVenda.Enabled = false;
                btnFiscal.Enabled = false;
                btnCancelarCupom.Enabled = false;
                btnEncerrar.Enabled = false;
            }
        }
        void DelegateCliente(string crControle)
        {
            controle = crControle;
            switch (controle)
            {
                case "btConfirmarCR":
                case "btConfirmarCH":
                    if (clientePDV.tipoPagamento=="")
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
                    pnlCliente.Visible = false;                    

                    if (clientePDV.tipoPagamento=="") AtivarBotoes();

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
            }
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
                        txtCartao.Focus();                    
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
        void VerificaPagamento()
        {
            if (dinheiro == 0 && controle != "txtDinheiro") txtDinheiro.Text = "0.00";
            if (cartao == 0 && controle!="txtCartao") txtCartao.Text = "0.00";
            if (crediario == 0 && controle != "txtCrediario") txtCrediario.Text = "0.00";
            if (cheque == 0 && controle != "txtCheque") txtCheque.Text = "0.00";              
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
                        MessageBox.Show("Não foi possível cancelar a forma de pagamento!","Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case "CA":
                    if (cartao == 0) return;
                    try
                    {                                 
                        Venda.ExcluirPagamento(formaPagamento);
                        restante += cartao;
                        cartao = 0;
                        txtCartao.Enabled = true;
                        txtCartao.Text = "0.00";
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Não foi possível cancelar a forma de pagamento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                        MessageBox.Show("Não foi possível cancelar a forma de pagamento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void Desconto()
        {
            decimal descontoMaximo = Venda.ObterDescontoMaximo();
            try
            {
                Convert.ToDecimal(txtDesconto.Text);
            }
            catch (Exception)
            {
                txtDesconto.Text = "0,00";
            }
            if (txtDesconto.Text == "0,00" && tipoDesconto=="%")
            {
                TipoDesconto(null,null);
                txtDesconto.SelectAll();
                return;
            };

            if (tipoDesconto == "%")
            {
                if (Convert.ToDecimal(txtDesconto.Text) > Configuracoes.descontoMaxVenda)
                {
                    MessageBox.Show("Desconto % permitido: " + String.Format("{0:C2}", Configuracoes.descontoMaxVenda));
                    txtDesconto.Text = "0,00";
                    txtDesconto.SelectAll();
                    return;
                };                
               txtDesconto.Text = String.Format("{0:n2}",Math.Round( Venda.ObterValorDesconto() *(Convert.ToDecimal(txtDesconto.Text)/100)));
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
                txtDesconto.Text = string.Format("{0:n2}",Convert.ToDecimal(txtDesconto.Text)+guardaDesconto);

            };

            if (Convert.ToDecimal(txtDesconto.Text) > descontoMaximo)
            {
                MessageBox.Show("Desconto não permitido! Máximo: " + String.Format("{0:C2}",descontoMaximo),"Atenção",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
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
                txtDinheiro.Text = string.Format("{0:n2}", totalVenda - guardaDesconto);
                txtDesconto.SelectAll();
                txtDesconto.Focus();
                txtDesconto.SelectAll();
                return;
            }


            desconto = Convert.ToDecimal(txtDesconto.Text);
            txtDesconto.Enabled = false;
            txtDesconto.Text = String.Format("{0:n2}",desconto);
            txtDinheiro.Text = string.Format("{0:n2}",totalVenda-desconto);
            restante = totalVenda - desconto;
            txtDesconto.SelectAll();
            DesativaDesconto();            
            txtDinheiro.SelectAll();
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
            RedeCartoes frmrede = new RedeCartoes();
            frmrede.ShowDialog();
            //fecf.AdministrativoTEF("VISANET");
            
            
        }
        private void ChamarSuprimento()
        {
            Suprimento.tipoPagamento = "SU";
            Suprimento suprimento = new Suprimento();
            suprimento.ShowDialog();
        }
        private void ChamarSangria()
        {
            if (btnSangria.Enabled == false)
                return;
            Sangria sangria = new Sangria();
            sangria.ShowDialog();
        }

        private void btnPreVenda_Click(object sender, EventArgs e)
        {
            ChamarPreVenda();
        }

        private void ChamarPreVenda()
        {
            if (btnPreVenda.Enabled == false)
                return;
            if (ConfiguracoesECF.pdv == true && totalVenda > 0)
            {
                MessageBox.Show("Cupom em aberto, não é possível processar pré-venda !!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            frmPreVenda prevenda = new frmPreVenda();
            prevenda.ShowDialog();
        }


        private void VerificarGaveta()
        {
            if (FuncoesECF.EstadoGaveta() == 0)
            {                
                FrmMsgOperador troco = new FrmMsgOperador("gaveta", "Fechar gaveta !");
                troco.ShowDialog();
            }
        }

        private void btnCancelarCupom_Click(object sender, EventArgs e)
        {
            if ( FuncoesECF.NumeroCupomFiscal(ConfiguracoesECF.modeloECF) != "")
            {
                Venda objvenda = new Venda();
                objvenda.ExcluirDocumento(FuncoesECF.NumeroCupomFiscal(ConfiguracoesECF.modeloECF).ToString());
                MostrarItens();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FrmMenuFiscal fiscal = new FrmMenuFiscal();
            fiscal.ShowDialog();
        }

        private void btnEncerrar_Click(object sender, EventArgs e)
        {
            FrmLogon frmlogon = new FrmLogon();            
            frmlogon.txtDescricao.Text = "Fechamento de Caixa: "+GlbVariaveis.glb_Usuario;
            frmlogon.campo = "rotcaixa";
            frmlogon.ShowDialog();
            
            if (!Operador.autorizado) return;

            FuncoesECF.ReducaoZ();
            System.Threading.Thread.Sleep(200);
            Paf paf = new Paf();
            paf.GravarRelatorioR();
        }
            

        private void timeHora_Tick(object sender, EventArgs e)
        {
            lblDataHora.Text = String.Format("{0:dd/MM/yyyy hh:mm}", DateTime.Now);
        }

        private void ChamarProdutos()
        {
            FrmProdutos frmprd = new FrmProdutos();
            frmprd.ShowDialog();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Paf paf = new Paf();
            paf.GerarMovimentoPorECF(DateTime.Now.Date, DateTime.Now.Date, "001");
            return;      
        }

        private void btCancelaCH_Click(object sender, EventArgs e)
        {
            CancelarPagamento("CH");
        }

        private void MudarPreco()
        {
            try
            {
                var dados = produto.ProcurarCodigo(txtcodProduto.Text, GlbVariaveis.glb_filial);
                lblDescricaoPrd.Text = produto.descricao;
                txtPreco.Text = string.Format("{0:n2}", produto.preco);
                txtDescontoPercItem.Text = string.Format("{0:n2}", produto.descontoPromocao);
                grpPreco.Visible = true;
                txtDescontoPercItem.Focus();

            }
            catch (Exception erro)
            {
                MessageBox.Show("Item com restrição:" + erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //if (prd.Procurar(txtcodProduto.Text, GlbVariaveis.glb_filial))
            //{
            //    txtPreco.Text = string.Format("{0:n2}", prd.preco);
            //    txtDescontoPercItem.Text = string.Format("{0:n2}", prd.descontoPercPromocao);
            //    grpPreco.Visible = true;
            //    txtDescontoPercItem.Focus();
            //};
        }

   }  
   
}

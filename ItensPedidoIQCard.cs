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
    public partial class ItensPedidoIQCard : Form
    {
        public static event EventHandler gerarVenda;
        private string idItem;
        public static List<ServiceReference1.PedidoEntrega> movimentacaoEntrega = new List<ServiceReference1.PedidoEntrega>();



        public ItensPedidoIQCard()
        {
            InitializeComponent();

            

            txtQuantidade.KeyPress += (objeto, evento) =>
            {              
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            };

            txtQuantidade.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {
                    txtQuantidade.Text = Funcoes.FormatarDecimal(txtQuantidade.Text, 3);
                    if (Convert.ToDecimal(txtQuantidade.Text) < 0)
                    {
                        MessageBox.Show("Quantidade sem valor.");
                        evento.SuppressKeyPress = true;
                        return;
                    }
                };
            };
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        public void atualizarStatus(string id)
        {

            // var dados = IqCard.AbrirPedido(IqCard.DadosPedido.RowKey);

            // Essa condicao fica fora dessa logica por que é a data de criacao do pedido

            // TODOS VERDES
            try
            {
                txtRecebido.Text = "Solicitado " + string.Format("{0:dd/MM/yyyy}", IqCard.DadosPedido.data) + " ás " + string.Format("{0:hh:mm}", IqCard.DadosPedido.data);

                // Imagens
                /*
                pnlSeparacao.BackgroundImage = new Bitmap(@"imagensMetro\relogio-verde.png");
                pnlRecebido.BackgroundImage = new Bitmap(@"imagensMetro\ok-verde.png");
                pnEntregaSeparacao.BackgroundImage = new Bitmap(@"imagensMetro\barras-verde.png");
                pnEntregaSaida.BackgroundImage = new Bitmap(@"imagensMetro\carrinho-verde.png");
                */

                pnlRecebido.BackgroundImage = new Bitmap(@"imagensMetro\relogioAmarelo.png");
                //pnlRecebido.BackgroundImage = new Bitmap(@"imagensMetro\ok-cinza.png");
               // pnEntregaSeparacao.BackgroundImage = new Bitmap(@"imagensMetro\barras-cinza.png");
               // pnEntregaSaida.BackgroundImage = new Bitmap(@"imagensMetro\carrinho-cinza.png");

                //pnlEntrega.BackgroundImage = new Bitmap(@"imagensMetro\ok-verde.png");
            }
            catch (Exception)
            {

            }


            IqCard iqcard = new IqCard();

            movimentacaoEntrega = iqcard.DadosMovimentacaoEntregaPedido(id);
            foreach (var item in movimentacaoEntrega)
            {
                if (item.descricaoMov.ToLower() == "pedido em separação")
                {
                    // SEPARAÇÃO VERDE
                    linhaSeparacao.Visible = false;
                    linhaSaida.Visible = false;                                        
                    pnEntregaSaida.Visible = true; //Visibility = ViewStates.Visible;

                    txtDataSeparacao.Text = "Separação " + string.Format("{0:dd/MM/yyyy}", item.data) + " ás " + string.Format("{0:hh:mm}", item.data);

                    // Imagens
                    pnEntregaSeparacao.BackgroundImage = new Bitmap(@"imagensMetro\barras-verde.png");
                    pnEntregaSaida.BackgroundImage = new Bitmap(@"imagensMetro\carrinho-cinza.png");
                    pnlEntrega.BackgroundImage = new Bitmap(@"imagensMetro\ok-cinza.png");
                }

                if (item.descricaoMov.ToLower() == "pedido saiu para entrega")
                {
                    // SEPARAÇÃO E CARRINHO VERDE                    
                    linhaSaida.Visible = false;
                    txtEntregaSaida.Text = "Saiu para entrega " + string.Format("{0:dd/MM/yyyy}", item.data) + " ás " + string.Format("{0:hh:mm}", item.data);

                    // Imagens
                    pnEntregaSeparacao.BackgroundImage = new Bitmap(@"imagensMetro\barras-verde.png");
                    pnEntregaSaida.BackgroundImage = new Bitmap(@"imagensMetro\carrinho-verde.png");
                    pnlEntrega.BackgroundImage = new Bitmap(@"imagensMetro\ok-cinza.png");

                }

                // Essa condicao fica fora dessa logica por que é a data de criacao do pedido
                if (item.descricaoMov.ToLower() == "pedido entregue")
                {
                    // TODOS VERDES
                    txtDataEntregue.Text = "Entregue " + string.Format("{0:dd/MM/yyyy}", item.data) + " ás " + string.Format("{0:hh:mm}", item.data);

                    // Imagens
                    pnEntregaSeparacao.BackgroundImage = new Bitmap(@"imagensMetro\barras-verde.png");
                    pnEntregaSaida.BackgroundImage = new Bitmap(@"imagensMetro\carrinho-verde.png");
                    pnlEntrega.BackgroundImage = new Bitmap(@"imagensMetro\ok-verde.png");
                }

                if (item.descricaoMov.ToLower() == "pedido cancelado")
                {
                    btnStatus.Enabled = false;
                    btnVenda.Enabled = false;
                    btnSeparacao.Enabled = false;                                            
                    IqCard.DadosPedido.encerrado = true;
                }

            }

        }






        private void ItensPedidoIQCard_Load(object sender, EventArgs e)
        {


            var dados = IqCard.AbrirPedido(IqCard.DadosPedido.RowKey);
            // Por que essa condicao por que o pedido pode nao ter tido movimetnacao
            try
            {
                atualizarStatus(IqCard.DadosPedido.RowKey);
            }
            catch (Exception)
            {
                
            }            


            this.Text = Funcoes.FormatarCartao(IqCard.dadosCartao.idCartao) + " " + IqCard.dadosCartao.nome;
            lblNome.Text = IqCard.dadosCartao.nome;
            lblEndereco2.Text = IqCard.DadosPedido.localizacao;
            dtgItens.DataSource = (from n in dados select new { referenciaItem=n.referenciaItem, RowKey =n.RowKey, descricao = n.descricao,atendida=n.quantidadeAtendida, quantidade = n.quantidade, preco = n.preco, total = n.total }).ToList();
            lblStatus.Text = "STATUS: " + IqCard.DadosPedido.status.ToUpper();
            lblTotal.Text = string.Format("{0:N2}", IqCard.DadosPedido.totalOrcamento);
            txtEndereco.Text = IqCard.DadosPedido.localizacao;
            // lblEntrega.Text = IqCard.DadosPedido.entrega == "S" ? "FAZER ENTREGA" : "PEDIDO SERÁ RETIRADO PELO CLIENTE";
            lblObservacao.Text = "OBS.: " + IqCard.DadosPedido.observacao + " - Telefone: " + IqCard.dadosCartao.telefone + " - Melhor hora para entregar/ retirar: " + IqCard.DadosPedido.horarioRetirada;
            lblPagamento.Text = IqCard.DadosPedido.statusPagamento+ "("+IqCard.DadosPedido.descricaoPagamento +")";
            // Sei lá

            // lblHora.Text ="Melhor hora para entregar/retirar: "+ IqCard.DadosPedido.horarioRetirada;
            if(IqCard.DadosPedido.encerrado)
            {
                btnStatus.Enabled = false;                
                btnEncerrar.Enabled = false;
            }

            if (!string.IsNullOrEmpty(IqCard.DadosPedido.documento))
            {
                lblPagamento.Text = IqCard.DadosPedido.statusPagamento;
                lblEncerrado.Text = "ENCERRADO DOC.: " + IqCard.DadosPedido.documento;
                btnVenda.Enabled = false;
                btnStatus.Visible = false;
                btnSeparacao.Visible = false;
                btnEncerrar.Visible = false;
            }

            if(IqCard.DadosPedido.entrega=="N")
            {
                btnStatus.Visible = true;
            }
            else
            {
                btnStatus.Visible = false;
            }

            // lblTelefone.Text = "Tel.: " + IqCard.dadosCartao.telefone;
            lblEndereco.Text = IqCard.dadosCartao.endereco + " " + IqCard.dadosCartao.numero + " " + IqCard.dadosCartao.cep + " " + IqCard.dadosCartao.bairro + " " + IqCard.dadosCartao.cidade + " " + IqCard.dadosCartao.estado;
        }

        private void btnVenda_Click(object sender, EventArgs e)
        {
            if (ConfiguracoesECF.pdv && ConfiguracoesECF.caixaPendente)
            {
                MessageBox.Show("Sem caixa aberto");
                return;
            }

            FrmMsgOperador msg3 = new FrmMsgOperador("", "Gerando venda");
            try
            {
                msg3.Show();
                Application.DoEvents();
                IqCard iqcard = new IqCard();
                iqcard.VerificarItens();
                iqcard.GerarVenda();

                if (!string.IsNullOrEmpty(IqCard.DadosPedido.documento))
                {
                    MessageBox.Show("Pedido já foi encerrado");
                    return;
                }

                // Aqui encerra o pedido direto se o pagamento foi confirmado. Isto é a transacao
                // já foi paga pelo usuário do IQCARD
                if (IqCard.DadosPedido.statusPagamento == "pagamento confirmado")
                {
                    if (MessageBox.Show("Gerar cupom e encerrar o pedido ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Venda venda = new Venda();
                        if (venda.Finalizar(false, false, false, true, false) > 0)
                        {
                            MessageBox.Show("Pedido feito no IQCARD. Gerado e encerrado com sucesso!");
                            Venda.idPedidoIQCARD = "";
                            Venda.IQCard = "";
                            Venda.idTransacaoIQCARD = "";
                        }
                    }
                    msg3.Dispose();
                    return;
                }

                gerarVenda.Invoke(null, null);
                msg3.Dispose();
                Close();

            }
            catch (Exception ex)
            {
                msg3.Dispose();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Alterar status pronto para entrega?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string status = "Pedido saiu para entrega";
                    IqCard iqcard = new IqCard();
                    iqcard.VerificarItens();
                    iqcard.MudarStatusEntrega("Pedido saiu para entrega");
                    lblStatus.Text = status;
                    atualizarStatus(IqCard.DadosPedido.RowKey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
        }

        private void dtgItens_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            return;

            if(IqCard.DadosPedido.encerrado)
            {
                MessageBox.Show("Pedido já foi encerrado");
                return;
            }
            dtgItens.Enabled = false;
            pnlAtendida.Visible = true;          
            idItem = dtgItens.CurrentRow.Cells["RowKey"].Value.ToString();
            txtQuantidade.Text = dtgItens.CurrentRow.Cells["atendida"].Value.ToString();
            txtQuantidade.Focus();

        }

        private void btnAtendida_Click(object sender, EventArgs e)
        {

            

            //try
            //{
            //    IqCard iqcard = new IqCard();
            //    iqcard.AlterarQuantidadeAtendia(idItem, Convert.ToDouble(txtQuantidade.Text));

            //    IqCard.itensPedidoIQCard.Where(c => c.RowKey == idItem).First().quantidadeAtendida = Convert.ToDouble(txtQuantidade.Text);
            //    IqCard.itensPedidoIQCard.Where(c => c.RowKey == idItem).First().total = (IqCard.itensPedidoIQCard.Where(c => c.RowKey == idItem).First().quantidadeAtendida * IqCard.itensPedidoIQCard.Where(c => c.RowKey == idItem).First().preco);
            //    dtgItens.DataSource = (from n in IqCard.itensPedidoIQCard select new { RowKey = n.RowKey, descricao = n.descricao, atendida = n.quantidadeAtendida, quantidade = n.quantidade, preco = n.preco, total = n.total }).ToList();

            //    lblTotal.Text = string.Format("{0:N2}", (from n in IqCard.itensPedidoIQCard select n.total).Sum());

            //    dtgItens.Enabled = true;
            //    pnlAtendida.Visible = false;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}


        }

        private void btlAlterarEnd_Click(object sender, EventArgs e)
        {
            try
            {
                IqCard iqcard = new IqCard();
                iqcard.AlterarEnderecoPedido(txtEndereco.Text);
                MessageBox.Show("Nova localização adicionada com sucesso.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnEncerrar_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("O pedido será encerrado?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string status = "Pedido Cancelado";
                    IqCard iqcard = new IqCard();
                    iqcard.VerificarItens();
                    iqcard.MudarStatusEntrega("Pedido cancelado");
                    lblStatus.Text = status;
                    atualizarStatus(IqCard.DadosPedido.RowKey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSeparacao_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Imprimir a lista de separação?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string status = "Pedido em separação";
                    IqCard iqcard = new IqCard();
                    //iqcard.VerificarItens();

                    var pedidoSep = (from n in movimentacaoEntrega where n.descricaoMov.ToLower() == "pedido em separação" select n.descricaoMov).FirstOrDefault();
                    if (string.IsNullOrEmpty(pedidoSep))
                    {
                        iqcard.MudarStatusEntrega(status);
                        lblStatus.Text = status;
                        atualizarStatus(IqCard.DadosPedido.RowKey);
                    }

                    IQCardListaSeparacao listaImpressao = new IQCardListaSeparacao();
                    listaImpressao.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRecebimento_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Alterar status para pedido recebido?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string status = "Pedido recebido";
                    IqCard iqcard = new IqCard();
                    iqcard.VerificarItens();
                    iqcard.MudarStatusEntrega(status);
                    lblStatus.Text = status;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}

using QRCoder;
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
    public partial class IndexPedidoIQCard : Form
    {
        public static bool iniciarmostrandoPedido = true;

        public IndexPedidoIQCard()
        {
            InitializeComponent();
            ItensPedidoIQCard.gerarVenda += ItensPedidoIQCard_gerarVenda;
        }

        private void IndexPedidoIQCard_Load(object sender, EventArgs e)
        {

            FrmMsgOperador msg3 = new FrmMsgOperador("", "Listagem pedidos IQCARD");
            msg3.Show();
            Application.DoEvents();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            try
            {
                IqCard iqcard = new IqCard();
                lblSolicitados.Text = iqcard.ContadorPedido("solicitado").ToString();
                lblAguardando.Text = iqcard.ContadorPedido("aguardando retirada").ToString();
                lblEncerrados.Text = iqcard.ContadorPedido("encerrado").ToString();
                Listagem("solicitado");
                if(IqCard.contadorUsuarios>0)
                {
                    lblDistancia.Text = "Usuários IQCARD a menos de 3 KM do seu negócio: " + IqCard.contadorUsuarios.ToString();
                }
                else
                {                    
                    lblDistancia.Text = "Usuários IQCARD a menos de 3 KM do seu negócio: " + iqcard.ContadorLocalizacao().ToString();
                }

                if (iqcard.ContadorItensDelivery(GlbVariaveis.glb_chaveIQCard) > 0)
                {
                    btnAtualizarItens.Text = "ATUALIZAR ITENS (" + IqCard.contadorItensDelivery + ")";
                    btnExcluir.Visible = true;
                    GerarQrCodeLojaVirtual();
                }
                else
                {
                    btnAtualizarItens.Text = "ATIVAR LOJA VIRTUAL";
                    btnExcluir.Visible = false;
                }
                

            }
            catch (Exception)
            {
                
            }
            if (iniciarmostrandoPedido)
            {
                pnlGerenciar.Visible = false;
            }

            msg3.Dispose();           
            
        }

        private void GerarQrCodeLojaVirtual()
        {
            pnlLoja.Visible = true;
            QRCodeGenerator qr = new QRCodeGenerator();
            QRCodeData data = qr.CreateQrCode(GlbVariaveis.glb_chaveIQCard + "#pedido", QRCodeGenerator.ECCLevel.Q);
            QRCode code = new QRCode(data);
            imgQRCode.Image = code.GetGraphic(5);
        }

        private void ItensPedidoIQCard_gerarVenda(object sender, EventArgs e)
        {
            Close();
        }

        private void lblSolicitados_Click(object sender, EventArgs e)
        {
            Listagem("solicitado");
        }


        private void Listagem(string status)
        {
            dtPedidos.AutoGenerateColumns = false;
            var dados = IqCard.PedidosIQCardPorStatus(status);            
            dtPedidos.DataSource = (from n in dados select new { idCartao = n.idCartao, nome = n.nomeCartao, data = n.data, valor = n.totalOrcamento,entrega = n.entrega,  observacao = n.observacao, RowKey = n.RowKey, status = n.status.ToUpper(),statusPagamento = n.statusPagamento, documento=n.documento }).ToList();

        }

        private void lblAguardando_Click(object sender, EventArgs e)
        {
            Listagem("aguardando retirada");
        }

        private void lblEncerrados_Click(object sender, EventArgs e)
        {
            Listagem("encerrado");
        }

        private void dtPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtChavePedido.Text = dtPedidos.CurrentRow.Cells["RowKey"].Value.ToString();
            if (e.ColumnIndex == 0)
            {
                string idPedido = dtPedidos.CurrentRow.Cells["RowKey"].Value.ToString();
                IqCard.DadosPedido.RowKey = idPedido;

                ItensPedidoIQCard itens = new ItensPedidoIQCard();
                itens.ShowDialog();
            }
        }

        private void btnAtualizarItens_Click(object sender, EventArgs e)
        {
            try
            {

                FrmLogon Logon = new FrmLogon();
                Operador.autorizado = false;
                Logon.campo = "administrador";
                Logon.lblDescricao.Text = "Enviar os itens para o aplicativo IQCARD";
                Logon.txtDescricao.Text = "PAINEL FINANCEIRO IQCARD";
                Logon.ShowDialog();
                if (!Operador.autorizado) return;

                IqCard iqcard = new IqCard();

                
                if (MessageBox.Show("Confirma carga e atualização dos itens. Isso pode demorar algum tempo ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                var inicio = DateTime.Now;
                FrmMsgOperador msg3 = new FrmMsgOperador("", "Atualizando itens. Primeira carga pode demorar vários minutos. Aguarde...");
                msg3.Show();
                Application.DoEvents();
                try
                {                                                                                                                                                     
                    iqcard.AtualizarItensDelivery(chkMarcados.Checked,chkQuantidade.Checked,chkBarras.Checked,chkPromo.Checked,chkMarket.Checked);
                    btnAtualizarItens.Text = "LOJA VIRTUAL CRIADA";
                    btnExcluir.Visible = true;
                    MessageBox.Show("Produtos sincronizados com sucesso. INÍCIO: " + inicio.ToString()+" FIM: "+DateTime.Now.ToString()+" Sua loja virutal estar pronta e você pode começar a usar no app IQCARD na opção meu negócio!");
                    GerarQrCodeLojaVirtual();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    msg3.Dispose();
                }
                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if(!Permissoes.administrador)
            {
                MessageBox.Show("Sem permissão. Somente administrador pode executar essa ação.");
                return;
            }

            if (MessageBox.Show("Ao excluir os itens os usuários do aplicativo não poderá ver mais os itens nem fazer pedido", "Confirma?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            FrmMsgOperador msg3 = new FrmMsgOperador("", "Excluindo itens no aplicativo IQCARD.");
            msg3.Show();
            Application.DoEvents();
            try
            {
                IqCard iqcard = new IqCard();
                iqcard.ExcluirItensAplicativo(false,false);
                IqCard.contadorItensDelivery = 0;
                btnAtualizarItens.Text = "ATIVAR LOJA VIRTUAL";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                msg3.Dispose();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnCreditos_Click(object sender, EventArgs e)
        {
            FrmLogon Logon = new FrmLogon();
            Operador.autorizado = false;
            Logon.campo = "administrador";
            Logon.lblDescricao.Text = "Verificar créditos e financeiro IQCARD";
            Logon.txtDescricao.Text = "PAINEL FINANCEIRO IQCARD";
            Logon.ShowDialog();
            if (!Operador.autorizado) return;

            IndexPontosAcumulados pnlIQCARD = new IndexPontosAcumulados();
            pnlIQCARD.ShowDialog();
        }

        private void btnProcurarChve_Click(object sender, EventArgs e)
        {
            try
            {
                var dados = IqCard.PegarPedido(txtChavePedido.Text);
                dtPedidos.DataSource = (from n in dados select new { idCartao = n.idCartao, nome = n.nomeCartao, data = n.data, valor = n.totalOrcamento, entrega = n.entrega, observacao = n.observacao, RowKey = n.RowKey, status = n.status.ToUpper(), statusPagamento = n.statusPagamento }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }

        private void dtPedidos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                txtChavePedido.Text = dtPedidos.CurrentRow.Cells["RowKey"].Value.ToString();

            }
            catch (Exception)
            {

                txtChavePedido.Text = "";
            }
           
        }

        private void btnLista_Click(object sender, EventArgs e)
        {
            ListaCompraIQCARD lista = new ListaCompraIQCARD();
            lista.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnlGerenciar.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pnlGerenciar.Visible = true;
        }

        private void pnlProntoEntrega_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pnlProntoEntrega_Click(object sender, EventArgs e)
        {
            Listagem("aguardando retirada");
        }

        private void pnlSolicitados_Click(object sender, EventArgs e)
        {
            Listagem("solicitado");
        }

        private void btnSincronizar_Click(object sender, EventArgs e)
        {

            ProdutosDelivery iqcard = new ProdutosDelivery();
            FrmMsgOperador msg3 = new FrmMsgOperador("", "Atualizando url da imagem no banco local");
            msg3.Show();
            Application.DoEvents();
            try
            {
                var totalAtualizados = iqcard.AtualizarUrlImagens();
                MessageBox.Show("Foram atualizados " + totalAtualizados + " Itens");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                msg3.Dispose();
            }
        }
    }
}

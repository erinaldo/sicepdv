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
    public partial class FrmFinalizarPreVenda : Form
    {        
        TeclaNumerico teclado = new TeclaNumerico(Color.Gray);
        UcAdicionarItens itens = new UcAdicionarItens();

        private string controle = "";
        private string tecla = "";
        private int numeroFinalizacao;
        private int codCliente;
        private decimal totalLiquido;
        private decimal desconto;
        private decimal encargos;
        List<Int32> lstMescladas;
        private string tipoAjuste = "";
        bool FinalizarPreVenda = false;
        bool FinalizarDAV = false;
        private decimal valorAjuste = 0;
        private bool chamadaFinalizacao = false;
        private bool manterVendedorOriginal = false;
        

        public FrmFinalizarPreVenda(int numero,int idCliente,decimal valor,decimal totaldesconto,decimal totalEncargos, List<Int32> prevendas,bool prevenda,bool DAV,bool manterVendedor=false)
        {
            numeroFinalizacao = numero;            
            codCliente = idCliente;
            desconto = totaldesconto;
            encargos = totalEncargos;
            totalLiquido = valor-totaldesconto;
            lstMescladas = prevendas;
            FinalizarPreVenda = prevenda;
            FinalizarDAV = DAV;
            manterVendedorOriginal = manterVendedor;



            /// <summary> FinalizarPrevenda = Quando a Origem é uma PreVenda </summary>
            /// FinalizarDAV = Quando a origem é um DAV
            /// 

            InitializeComponent();
            pnlTeclado.Controls.Add(teclado);
            this.teclado.clickBotao += new TeclaNumerico.ClicarNoBotao(DelegateTeclado);            
            MostrarPreVenda();            
        }

        void DelegateItens(object sender, string acao)
        {            
            switch (acao)
            {
                case "incluir":
                Venda itens = new Venda();
                dtgItens.DataSource = itens.SelectionaItensVenda();                
                dtgItens.CurrentCell = dtgItens.Rows[dtgItens.RowCount-1].Cells[0];
                break;
            }
        }

        void DelegateTeclado(object sender, string acao)
        {            
            switch (acao)
            {
                case "X":
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

        private void TeclaEnter()
        {
            switch (controle)
            {
                case "txtNumero":                    
                    break;
                default:
                    if (controle == "") return;
                    Control[] ctls = this.Controls.Find(controle, true);
                    ctls[0].Focus();
                    SendKeys.Send("{TAB}");
                    break;
            }
        }

        private void PreencheCampo()
       {
            if (controle == "") return;
            Control[] ctls = this.Controls.Find(controle, true);
            TextBox txtBox = ctls[0] as TextBox;
            if (txtBox.Enabled == false)
                return;
            txtBox.Text += this.tecla;
            txtBox.Focus();
        }


        private void Sair()
        {
            // Para zerar as variaveis de entrega
            if (_pdv.numeroPreVenda == 0 && _pdv.numeroDAV == 0)
            {
                Venda.dadosEntrega = new DadosEntrega();
                Venda.dadosDAVOS = new DadosDAVOS();
                this.lstMescladas.Clear();

                if (!FuncoesECF.CupomFiscalAberto())
                {
                    Venda.ApagarItensFormaPagamento("ItensPagamentos");
                }
                if (chamadaFinalizacao == false)
                {
                    Venda.ApagarItensFormaPagamento("ItensPagamentos");
                    Venda.ApagarItensFormaPagamento("transferenciatmp");
                }
               
            }
            this.Close();
        }


        private void MostrarPreVenda()
        {
            Venda.ExcluirPagamento("");
            Venda.ExcluirVendaTransf();
            // Se o array de Prevendas for 0 entao passa o numero da prevenda
            // para ele. Para que o foreach contenha pelo menos uma prevenda e seja executado.



            if (lstMescladas.Count == 0)
                lstMescladas.Add(numeroFinalizacao);
            
            lblNumero.Text = "";
            lblCliente.Text = "";
            lblVendedor.Text = "";

            foreach (var item in lstMescladas)
            {

                var dadosPedido = Conexao.CriarEntidade().ExecuteStoreQuery<dadosDAV>("select ifnull(vendaatacado,'N') as vendaatacado, ifnull(origem,'SICEpdv') as origem, statusPagamento, formapagamento, idTransacaogateway, status   from contdav where numeroDAVFilial = '" + item + "'  and codigofilial = '" + GlbVariaveis.glb_filial + "'").FirstOrDefault();

                if (dadosPedido.origem == "iqShop")
                {
                    btConcluir.Enabled = false;
                    btnPagDH.Enabled = false;
                    btnAlterarPgt.Enabled = false;

                    if (dadosPedido.formapagamento == "CA" && GlbVariaveis.glb_gateway == "S")
                    {
                        pnStatusPedido.Visible = true;

                        if (dadosPedido.statusPagamento == "Pendente" || dadosPedido.statusPagamento == "Não Finalizado" || dadosPedido.statusPagamento == "Cancelado")
                            pnlStatusPagamento.BackColor = Color.FromArgb(255, 128, 0);
                        else if (dadosPedido.statusPagamento == "Autorizado" || dadosPedido.statusPagamento == "Pago")
                        {
                            pnlStatusPagamento.BackColor = Color.FromArgb(91, 184, 93);
                            btConcluir.Enabled = true;
                        }
                        else
                            pnlStatusPagamento.BackColor = Color.FromArgb(216, 84, 79);


                        lblStatusPagamento.Text = (dadosPedido.statusPagamento == null || dadosPedido.statusPagamento == "") ? "Sem Status" : dadosPedido.statusPagamento;
                        lblStatusDAV.Text = dadosPedido.status;
                    }
                    else
                    {
                        btConcluir.Enabled = true;
                    }

                }




                if (FinalizarPreVenda)
                {
                    var dadosPreVenda = (from n in Conexao.CriarEntidade().contprevendaspaf
                                    where n.numeroDAVFilial == item
                                    && n.codigofilial == GlbVariaveis.glb_filial
                                    select n).First();
                    if (dadosPreVenda.codigocliente>0)
                    lblCliente.Text = dadosPreVenda.codigocliente.ToString() + " " + dadosPreVenda.cliente;
                    if (dadosPreVenda.vendedor != "000")
                    {
                        if (manterVendedorOriginal==true)
                        Vendedor.VendaVendedor(dadosPreVenda.vendedor);
                        lblVendedor.Text = "Vend.: " + Vendedor.codigoVendedor + " " + Vendedor.nomeVendedor;
                    }

                    lblDesconto.Text = "Desconto R$: " + string.Format("{0:N2}", dadosPreVenda.desconto);
                    PreVenda.MontarPreVenda(item);
                }


               if (FinalizarDAV)
               {
                   if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Geral.ToString())
                   {
                       var dadosDAV = (from n in Conexao.CriarEntidade().contdav
                                       where n.numeroDAVFilial == item
                                       && n.codigofilial == GlbVariaveis.glb_filial
                                       select n).First();
                       if (dadosDAV.codigocliente > 0)
                       {
                           lblCliente.Text = dadosDAV.codigocliente.ToString() + " " + dadosDAV.cliente;
                           codCliente = dadosDAV.codigocliente;
                       }
                       if (dadosDAV.vendedor != "000")
                       {
                           if (manterVendedorOriginal == true)
                           Vendedor.VendaVendedor(dadosDAV.vendedor);
                           lblVendedor.Text = "Vend.: " + Vendedor.codigoVendedor + " " + Vendedor.nomeVendedor;
                       }


                       lblDesconto.Text = "Desconto R$: " + string.Format("{0:N2}", dadosDAV.desconto);
                       

                       //padar dados de entrega
                       PreVenda.MontarDAV(item);
                       
                       
                   }

                   if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                   {
                       var dadosDAV = (from n in Conexao.CriarEntidade().contdavos
                                       where n.numeroDAVFilial == item
                                       && n.codigofilial == GlbVariaveis.glb_filial
                                       select n).First();

                       if (dadosDAV.codigocliente > 0)
                           lblCliente.Text = dadosDAV.codigocliente.ToString() + " " + dadosDAV.cliente;

                       if (dadosDAV.vendedor != "000")
                           lblVendedor.Text = dadosDAV.vendedor;

                       if (manterVendedorOriginal == false)
                           Vendedor.VendaVendedor(dadosDAV.vendedor);


                       lblDesconto.Text = "Desconto R$: " + string.Format("{0:N2}", dadosDAV.desconto);
                       PreVenda.MontarDAV(item);
                   }

                }

                lblNumero.Text += Convert.ToString(item.ToString()+"; ");
            }
            
            
            Venda itens = new Venda();
            Venda.atualizarEstoqueDAV();
            dtgItens.DataSource = itens.SelectionaItensVenda();
            lblTotal.Text = string.Format("{0:n2}", itens.SomaItens()-desconto+encargos);            
            siceEntities entidade = Conexao.CriarEntidade();
            MostrarPagamento();      
        }

        private void MostrarPagamento()
        {
            siceEntities entidade = Conexao.CriarEntidade();
            lstPagamento.Items.Clear();
            var pagamento = from n in entidade.caixas
                            where n.EnderecoIP == GlbVariaveis.glb_IP
                            select n;
            foreach (var item in pagamento)
            {
                lstPagamento.Items.Add(item.tipopagamento + "  " + string.Format("{0:n2}", item.valor) + " " + item.vencimento.ToString());
            }

            var verificaPgt = (from n in pagamento
                               where n.tipopagamento == "CR"
                               && n.EnderecoIP == GlbVariaveis.glb_IP
                               select n.codigocliente).Distinct();            

            if (verificaPgt.Count() > 1)
            {
                MessageBox.Show("Não é possível encerrar. Selecionados clientes distintos na mesclagem dos documento","SICEpdv.net",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                btConcluir.Enabled = false;
                btnAdicionar.Enabled = false;
                btnAlterarPgt.Enabled = false;                 
            }

        }


        private void btConcluir_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Concluir ? ", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;


            Venda.atualizarEstoqueDAV();


            var contdav = (from n in Conexao.CriarEntidade().contdav
                           where n.numeroDAVFilial == numeroFinalizacao
                           && n.codigofilial == GlbVariaveis.glb_filial
                           select n).FirstOrDefault();

            if(contdav != null && contdav.codigocliente > 0)
                Clientes.AtualizarDebito(contdav.codigocliente, Configuracoes.taxaJurosDiario);

            var negativos = (from n in Conexao.CriarEntidade().vendas
                             where n.id == GlbVariaveis.glb_IP
                             && n.quantidadeatualizada<0 
                             && n.cancelado == "N"
                             select n).ToList();


            if (negativos.Count() > 0 && !Permissoes.venderQtdNegativa)
            {
                MessageBox.Show("Existem itens que após a venda ficarão negativos.");
                FrmLogon logon = new FrmLogon();
                logon.campo = "estnegativo";
                logon.txtDescricao.Text = "Finalização de DAVs com estoque negativo.DAV Nr.: "+numeroFinalizacao.ToString();
                logon.ShowDialog();

                if (!Operador.autorizado)
                    return;

                siceEntities entidade = Conexao.CriarEntidade();

                foreach (var item in negativos)
                {
                    auditoria objAuditoria = new auditoria();
                    objAuditoria.acao = "Liberação de estoque negativo";
                    objAuditoria.CodigoFilial = item.codigofilial;
                    objAuditoria.codigoproduto = item.codigo;
                    objAuditoria.data = DateTime.Now.Date;
                    objAuditoria.documento = 0;
                    objAuditoria.hora = DateTime.Now.TimeOfDay;
                    objAuditoria.local = "SICE.pdv";
                    objAuditoria.observacao = "Produto negativo: " + item.codigo + " " + item.produto + " Qtd: " + item.quantidadeatualizada.ToString();
                    objAuditoria.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                    objAuditoria.usuario = GlbVariaveis.glb_Usuario;
                    objAuditoria.tabela = "Venda";
                    entidade.AddToauditoria(objAuditoria);
                    entidade.SaveChanges();
                }

            }

            if (Configuracoes.gerarTransferenciaVenda == true)
            {
                string SQLNegativos = "SELECT codigo,descricao,filialorigem,quantidaestoqueorigem FROM transfvendatemp WHERE ip = '" + GlbVariaveis.glb_IP + "' AND filialdestino = '" + GlbVariaveis.glb_filial + "' AND quantidadeatualizada < 0 AND cancelado = 'N'";
                var itensNegativos = Conexao.CriarEntidade().ExecuteStoreQuery<transfvenda>(SQLNegativos).ToList();

                if (itensNegativos.Count() > 0 && !Permissoes.venderQtdNegativa)
                {
                    string listNegativos = "";

                    foreach (var item in itensNegativos)
                    {
                        listNegativos += item.codigo + " - " + item.descricao + "\n";
                    }

                    MessageBox.Show("Existem itens que após a venda ficarão negativos na Filial ("+itensNegativos.FirstOrDefault().filialorigem.ToString()+") \n "+ listNegativos + "");
                    FrmLogon logon = new FrmLogon();
                    logon.campo = "estnegativo";
                    logon.txtDescricao.Text = "Finalização de DAVs com estoque negativo.DAV Nr.: " + numeroFinalizacao.ToString();
                    logon.ShowDialog();

                    if (!Operador.autorizado)
                        return;

                    siceEntities entidade = Conexao.CriarEntidade();

                    foreach (var item in itensNegativos)
                    {
                        auditoria objAuditoria = new auditoria();
                        objAuditoria.acao = "Liberação de estoque negativo";
                        objAuditoria.CodigoFilial = item.filialorigem;
                        objAuditoria.codigoproduto = item.codigo;
                        objAuditoria.data = DateTime.Now.Date;
                        objAuditoria.documento = 0;
                        objAuditoria.hora = DateTime.Now.TimeOfDay;
                        objAuditoria.local = "SICE.pdv";
                        objAuditoria.observacao = "Produto negativo: " + item.codigo + " " + item.descricao + " Qtd: " + item.quantidaestoqueorigem.ToString();
                        objAuditoria.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                        objAuditoria.usuario = GlbVariaveis.glb_Usuario;
                        objAuditoria.tabela = "Venda";
                        entidade.AddToauditoria(objAuditoria);
                        entidade.SaveChanges();
                    }

                }
            }


            int documentoNumero = 0;
            chamadaFinalizacao = true;
            Venda venda = new Venda();
            try
            {
               documentoNumero = Encerrar(true);
            }
            catch (Exception ex)
            {
                venda.verificaVenda(true, true); 
                MessageBox.Show("Erro ao chamar encerramento: "+ex.Message);
            }
            

            this.Enabled = true;
            btConcluir.Enabled = false;
            btnAlterarPgt.Enabled = false;
            btnPagDH.Enabled = false;
            btnCancelar.Enabled = false;

            if (documentoNumero > 0)
            {
                bool imprimirComprovante = false;
                foreach (var item in lstPagamento.Items)
                {
                    if (item.ToString().Substring(0, 2) == "CR")
                        imprimirComprovante = true;
                }


                if (imprimirComprovante)
                {
                    int ultDocumento = (from n in Conexao.CriarEntidade().contdocs
                                       where n.ip == GlbVariaveis.glb_IP
                                       select n.documento).Max();

                    

                        
                        FrmMsgOperador msg = new FrmMsgOperador("", "Imprimindo comprovante !");

                        try
                        {

                            if (ConfiguracoesECF.pdv)
                            {

                            var cancelado = (from i in Conexao.CriarEntidade().contdocs
                                             where i.documento == ultDocumento && i.CodigoFilial == GlbVariaveis.glb_filial
                                             select i.estornado).FirstOrDefault();

                            if (cancelado == "N" && MessageBox.Show("Imprimir comprovante do crediário ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    msg.Show();
                                    Application.DoEvents();
                                    venda.ImprimirComprovante(codCliente,null, ultDocumento );
                                }

                                if (cancelado == "N" && MessageBox.Show("Imprimir Carnê ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    msg.Show();
                                    Application.DoEvents();
                                    venda.ImprimirCarne(ultDocumento);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Não foi possível imprimir comprovante: "+ex.Message);
                        }
                        finally
                        {
                            msg.Dispose();
                        }
                    
                }

                Cursor.Current = Cursors.Default;
                this.Close();
            }            
        }

        private int Encerrar(bool processarPagamento)
        {
            int DAVentrega = 0;

            if (ConfiguracoesECF.idECF == 0 || !ConfiguracoesECF.pdv)
            {
                MessageBox.Show("Só é possível finalizar no terminal tipo PDV.");
                return 0;
            }
            if (Convert.ToDecimal(lblTotal.Text) <= 0)
            {
                MessageBox.Show("Sem valor.");
                return 0;
            }
            var valorPagamento = (from n in Conexao.CriarEntidade().caixas
                                  where n.EnderecoIP == GlbVariaveis.glb_IP
                                  select n.valor).Sum();

            var valorItens = (from n in Conexao.CriarEntidade().vendas
                              where n.id == GlbVariaveis.glb_IP
                              && n.cancelado == "N"
                              select n.total).Sum() - desconto + encargos;

            if (valorPagamento != valorItens)
            {                
                if (MessageBox.Show("Existe divergência entre o total dos itens e os valores de pagamentos, Continuar ? ", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return 0;

                /*valorPagamento = valorItens;

                string sql = "UPDATE caixas SET valor=" + valorItens.ToString().Replace(",", ".") + " WHERE enderecoip='" + GlbVariaveis.glb_IP + "' LIMIT 1";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                sql = "DELETE from caixas  WHERE enderecoip='" + GlbVariaveis.glb_IP + "' AND valor<>" + valorItens.ToString().Replace(",", ".");
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);*/

                if (chkAjusteDiferenca.Checked == true)
                    corrigirValores();

                totalLiquido = valorItens;
            }
            

            this.Enabled = false;
            FrmMsgOperador msg = new FrmMsgOperador("", "Efetuando transação");
            msg.Show();
            Application.DoEvents();
            PreVenda prevenda = new PreVenda();
            Cursor.Current = Cursors.WaitCursor;
            // Se for uma pré-venda mesclada
            // Executa os procedimentos de cancelamento da pré-venda
            // e geração da nova pré-venda;
            // Cancelando as pré-vendas

            if (FinalizarPreVenda && lstMescladas.Count > 1)
            {
                foreach (var item in lstMescladas)
                {
                    if (!prevenda.CancelarPreVenda(item))
                    {
                        msg.Dispose();
                        this.Enabled = true;
                        MessageBox.Show("Não foi possível cancelar a pré-venda", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Sair();
                        return 0;
                    }
                }
            }
    
            if (FinalizarPreVenda)
            {
                try
                {
                    return prevenda.FinalizarPreVenda(numeroFinalizacao, totalLiquido+desconto, desconto, codCliente, lstMescladas.Count() > 1 ? true : false, processarPagamento);
                }
                catch (Exception)
                {
                    MessageBox.Show("Não foi possível finalizar a pré-venda", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Sair();
                    return 0;
                }
                finally
                {
                    msg.Dispose();
                }

                //if (prevenda.FinalizarPreVenda(numeroFinalizacao, totalLiquido, desconto, codCliente, lstMescladas.Count() > 1 ? true : false,processarPagamento) == 0)
                //{
                //    this.Enabled = true;
                //    MessageBox.Show("Não foi possível finalizar a pré-venda", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    Sair();
                //}
            };

            if (FinalizarDAV && lstMescladas.Count > 1)
            {
                foreach (var item in lstMescladas)
                {
                    if (!prevenda.CancelarDAV(item))
                    {
                        msg.Dispose();
                        this.Enabled = true;
                        MessageBox.Show("Não foi possível cancelar o DAV", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Sair();
                        return 0;
                    }

                    var DAV = (from p in Conexao.CriarEntidade().contdav
                                  where p.numeroDAVFilial == item
                                  select p).FirstOrDefault();


                    if (DAV.responsavelreceber.Trim() != "" || DAV.enderecoentrega.Trim() != "" || DAV.numeroentrega.Trim() != "" || DAV.bairroentrega.Trim() != "")
                    {
                        DAVentrega = item;
                    }
                }
            }

            if (FinalizarDAV)
            {
                try
                {
                    return prevenda.FinalizarDAV(numeroFinalizacao, totalLiquido+desconto, desconto, codCliente, lstMescladas.Count() > 1 ? true : false, processarPagamento, DAVentrega);                    
                }
                catch (Exception ex)
                {
                    msg.Dispose();
                    this.Enabled = true;
                    MessageBox.Show("Finalizando DAV: "+ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Sair();
                    return 0;
                }
                finally
                {

                    msg.Dispose();
                }
            };
            msg.Dispose();

            return 0;
        }


        private void FrmFinalizarPreVenda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Sair();


            if (e.KeyCode == Keys.F7)
                ChamarMenuFiscal();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (FinalizarDAV) return;

            if (MessageBox.Show("Cancelar Pré-venda ?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                Sair();
                return;
            }

            try
            {
                PreVenda objCancelar = new PreVenda();
                if (!objCancelar.CancelarPreVenda(numeroFinalizacao))
                {
                    MessageBox.Show("Não foi possível cancelar a pré-venda", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                }
            }
            catch
            {
                MessageBox.Show("Não foi possível cancelar a pré-venda", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Sair();
            }

        }

        private void FrmFinalizarPreVenda_Load(object sender, EventArgs e)
        {

            if (FinalizarDAV)
                this.Text = "Finalizar DAV";
            if (FinalizarPreVenda)
                this.Text = "Finalizar Pré-venda";

            Venda.atualizarEstoqueDAV();
            this.itens.clickBotao += new UcAdicionarItens.ClicarNoBotao(DelegateItens);
        }

        private void FrmFinalizarPreVenda_FormClosed(object sender, FormClosedEventArgs e)
        {
            Sair();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (verificaValores() != 0)
            {
                chkAjusteDiferenca.Checked = true;
            }

            tipoAjuste = "excluir";
            EscolherPagamentos();
            
            lblValor.Text = string.Format("{0:n2}", Convert.ToDecimal(dtgItens.CurrentRow.Cells["totalitem"].Value));
            valorAjuste = Convert.ToDecimal(dtgItens.CurrentRow.Cells["totalitem"].Value);
            
            chkPagamentos.Focus();
        }

        private void EscolherPagamentos()
        {
            pnlAjuste.Visible = true;

            foreach (var item in lstPagamento.Items)
            {
                string descricao = "";
                switch (item.ToString().Substring(0, 2))
                {
                    case "DH":
                        descricao = "Dinheiro";
                        break;
                    case "CH":
                        descricao = "Cheque";
                        break;
                    case "CR":
                        descricao = "Crediário";
                        break;
                    case "CA":
                        descricao = "Cartão";
                        break;
                }
                if (!chkPagamentos.Items.Contains(item.ToString().Substring(0, 2) + " " + descricao))
                    chkPagamentos.Items.Add(item.ToString().Substring(0, 2) + " " + descricao);

                for (int i = 0; i < chkPagamentos.Items.Count; i++)
                {
                    chkPagamentos.SetItemChecked(i, true);
                };
            }
        }

        private void AjustarPagamentos()
        {
            //if(chkAjusteDiferenca.Checked == true)
                //corrigirValores();


            siceEntities entidade = Conexao.CriarEntidade();
            decimal ajusteParcela = 0;
            decimal restoDivisao = 0;
            int qtdPagamento = 0;
            decimal rateioDesconto = 0;
            Venda itens = new Venda();
            for (int i = 0; i < chkPagamentos.Items.Count; i++)
            {
                string tipoPagamento = chkPagamentos.Items[i].ToString().Substring(0, 2);                
                if (chkPagamentos.GetItemChecked(i))
                {
                    qtdPagamento += (from n in entidade.caixas
                                     where n.tipopagamento == tipoPagamento
                                     && n.EnderecoIP == GlbVariaveis.glb_IP
                                     select n.tipopagamento).Count();
                }
            }
            if (qtdPagamento == 0)
            {
                pnlAjuste.Visible = false;
                return;
            }

            if (tipoAjuste == "incluir")
            rateioDesconto = (-desconto + encargos) / qtdPagamento;

            ajusteParcela = Math.Round((valorAjuste / qtdPagamento), 2);
            restoDivisao = valorAjuste - Math.Round((ajusteParcela * qtdPagamento), 2);
            
            for (int i = 0; i < chkPagamentos.Items.Count; i++)
            {
                if (chkPagamentos.GetItemChecked(i))
                {
                    string tipoPagamento = chkPagamentos.Items[i].ToString().Substring(0, 2);

                    var dadosAjuste = (from n in entidade.caixas
                                       where n.tipopagamento == tipoPagamento
                                       && n.EnderecoIP == GlbVariaveis.glb_IP
                                       select n);

                    foreach (var item in dadosAjuste)
                    {
                        siceEntities entidadeAjuste = Conexao.CriarEntidade();
                        caixas ajuste = (from n in entidadeAjuste.caixas
                                         where n.id == item.id
                                         && n.EnderecoIP == GlbVariaveis.glb_IP
                                         select n).FirstOrDefault();

                        if (valorAjuste > ajuste.valor && tipoAjuste=="excluir")
                        {
                            MessageBox.Show("Escolha forma de pagamento cujo valor seja igual ou maior que o valor total to item");
                            pnlAjuste.Visible = false;
                            return;
                        }
                        if (tipoAjuste=="excluir")
                        ajuste.valor -= (ajusteParcela + restoDivisao);
                        if (tipoAjuste=="incluir")
                            ajuste.valor += (ajusteParcela + restoDivisao) + rateioDesconto;

                        entidadeAjuste.SaveChanges();                        
                        restoDivisao = 0;
                    };                   
                }
            }

            if (tipoAjuste == "excluir")
            {
                int numeroItem = Convert.ToInt32(dtgItens.CurrentRow.Cells["inc"].Value);
                vendas excluir = (from n in entidade.vendas
                                  where n.inc == numeroItem
                                  select n).FirstOrDefault();
                totalLiquido -= excluir.total;
                desconto -= excluir.ratdesc;
                excluir.cancelado = "S";
                excluir.tipoalteracao = "E";                
                entidade.SaveChanges();



                
                dtgItens.DataSource = itens.SelectionaItensVenda();
                lblTotal.Text = string.Format("{0:n2}", itens.SomaItens()-desconto+encargos);                
            }
            
            dtgItens.DataSource = itens.SelectionaItensVenda();
            lblTotal.Text = string.Format("{0:n2}", itens.SomaItens()-desconto+encargos);                
            MostrarPagamento();
            pnlItens.Visible = false;
            pnlAjuste.Visible = false;
        }


        private decimal verificaValores()
        {
            siceEntities entidade = Conexao.CriarEntidade();

            var listCaixa = (from n in entidade.caixas
                             where n.EnderecoIP == GlbVariaveis.glb_IP
                             //&& n.estornado != "S"
                             select n).ToList();

            var ListVenda = (from n in entidade.vendas
                             where n.id == GlbVariaveis.glb_IP
                             && n.cancelado != "S"
                             select n).ToList();

            decimal valorVenda = ListVenda.Sum(x => x.total) - ListVenda.Sum(x => x.ratdesc);
            decimal valorCaixa = listCaixa.Sum(x => x.valor);
            return valorVenda - valorCaixa;
        }

        private bool corrigirValores()
        {

            try
            {
                siceEntities entidade = Conexao.CriarEntidade();

                decimal diferenca = verificaValores();

                var caixa = (from n in entidade.caixas
                             where n.EnderecoIP == GlbVariaveis.glb_IP
                             //&& n.estornado != "S"
                             select n).FirstOrDefault();

                if (diferenca > 0)
                    caixa.valor -= diferenca;
                else
                    caixa.valor += diferenca;

                entidade.SaveChanges();

            }
            catch(Exception erro)
            {
                return false;
            }

            return true;

        }

        private void btnCancelarAjuste_Click(object sender, EventArgs e)
        {
            pnlAjuste.Visible = false;
            pnlItens.Visible = false;
        }

        private void btnConfirmarAjuste_Click(object sender, EventArgs e)
        {                        
            AjustarPagamentos();            
            Application.DoEvents();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            tipoAjuste = "incluir";
            pnlItens.Location = new Point(10, 120);
            pnlItens.Enabled = true;
            pnlItens.Visible = true;            
            itens.TabIndex = 0;
            pnlItens.Controls.Add(itens);
            Application.DoEvents();
        }

        private void btnEncerrar_Click(object sender, EventArgs e)
        {
            pnlItens.Enabled = false;
            siceEntities entidade = Conexao.CriarEntidade();
            var totalitens = (from n in entidade.vendas
                              where n.id == GlbVariaveis.glb_IP
                              select n.total).Sum();

            var totalPagamentos = (from n in entidade.caixas
                                   where n.EnderecoIP == GlbVariaveis.glb_IP
                                   select n.valor).Sum();
            pnlItens.Visible = false;
            if (totalitens > totalPagamentos)
            {
                EscolherPagamentos();
                valorAjuste = totalitens - totalPagamentos;
                lblValor.Text = string.Format("{0:n2}", valorAjuste);
            }           
            
        }

        private void FrmFinalizarPreVenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }

        }

        private void btnAlterarPgt_Click(object sender, EventArgs e)
        {

            try
            {
                chamadaFinalizacao = true;
                int documentoNumero = Encerrar(false);                
                if (documentoNumero > 0)
                {
                    if (FinalizarDAV)
                    {
                        var DAV = (from n in Conexao.CriarEntidade().contdav
                                   where n.numeroDAVFilial == numeroFinalizacao
                                   select n).FirstOrDefault();

                        _pdv.numeroDAV = documentoNumero;

                        int? devolucaoNr = (from n in Conexao.CriarEntidade().contdav
                                            where n.numeroDAVFilial == numeroFinalizacao
                                            select (int?)n.devolucao).FirstOrDefault();

                        if (devolucaoNr.GetValueOrDefault() > 0)
                        {
                            // Verifica se tem devolução
                            decimal? valorDevolucaoDAV = (from n in Conexao.CriarEntidade().caixadav
                                                          where n.documento == numeroFinalizacao
                                                          && n.EnderecoIP == GlbVariaveis.glb_IP
                                                          && n.tipopagamento == "DV"
                                                          select (decimal?)n.valor).Sum();
                            _pdv.totalDevolucao = valorDevolucaoDAV.GetValueOrDefault();
                            _pdv.devolucaoNumero = devolucaoNr.GetValueOrDefault();
                        }

                        if (DAV.origem == "IQCHEF")
                            _pdv.taxaServicoIqChef = DAV.encargos;
                        else
                            _pdv.taxaServicoIqChef = 0;
                    }

                    if (FinalizarPreVenda)
                    {
                        _pdv.numeroPreVenda = documentoNumero;

                        int? devolucaoNr = (from n in Conexao.CriarEntidade().contprevendaspaf
                                            where n.numero == numeroFinalizacao
                                            select (int?)n.devolucao).FirstOrDefault();

                        if (devolucaoNr.GetValueOrDefault() > 0)
                        {
                            // Verifica se tem devolução
                            decimal? valorDevolucaoDAV = (from n in Conexao.CriarEntidade().caixaprevendapaf
                                                          where n.documento == numeroFinalizacao
                                                          && n.EnderecoIP == GlbVariaveis.glb_IP
                                                          && n.tipopagamento == "DV"
                                                          select (decimal?)n.valor).Sum();
                            _pdv.totalDevolucao = valorDevolucaoDAV.GetValueOrDefault();
                            _pdv.devolucaoNumero = devolucaoNr.GetValueOrDefault();
                        }

                        decimal? valorDevolucaoPRE = (from n in Conexao.CriarEntidade().caixaprevendapaf
                                                      where n.documento == numeroFinalizacao
                                                      && n.EnderecoIP == GlbVariaveis.glb_IP
                                                      && n.tipopagamento == "DV"
                                                      select (decimal?)n.valor).Sum();
                        _pdv.totalDevolucao = valorDevolucaoPRE.GetValueOrDefault();


                    }

                    Venda.ApagarItensFormaPagamento("Pagamentos");
                    Cursor.Current = Cursors.Default;
                    this.Close();
                }
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }
        }

        private void btSair_Click(object sender, EventArgs e)
        {
            Sair();
        }

        private void dtgItens_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtgItens.Columns[e.ColumnIndex].Name.Equals("qtdatualizada"))
            {                
                if ((Decimal)e.Value < 0)
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.SelectionBackColor = Color.DarkRed;
                }
            }

        }

        private void btnPagDH_Click(object sender, EventArgs e)
        {
            siceEntities entidade = Conexao.CriarEntidade();

            lstPagamento.Items.Clear();
            var pagamento = (from n in Conexao.CriarEntidade().caixas
                             where n.EnderecoIP == GlbVariaveis.glb_IP
                             select n.id);

            foreach (var item in pagamento)
            {
                var aPagar = (from n in entidade.caixas
                              where n.id == item
                              && n.EnderecoIP == GlbVariaveis.glb_IP
                              select n).First();
                entidade.DeleteObject(aPagar);
                entidade.SaveChanges();
            }

            Venda pgt = new Venda();
            var valorVenda = pgt.SomaItens();
            pgt.valorLiquido = valorVenda;
            pgt.EfetuarPagamento("Venda", "DH", valorVenda, 0, 0, "", "", 0, GlbVariaveis.Sys_Data, 0, null, false, false);
            MostrarPagamento();
        }
     
        private static void ChamarMenuFiscal()
        {
            FrmMenuFiscal frmFiscal = new FrmMenuFiscal();
            frmFiscal.ShowDialog();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            ImpressaoDAV impDav = new ImpressaoDAV(numeroFinalizacao);
            impDav.ShowDialog();
        }

        private void dtgItens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string lote = "";
            string compDescricao = "";

            frmComplementoItem complemento = new frmComplementoItem();
            complemento.ShowDialog();
            lote = complemento.txtLote.Text;
            compDescricao = complemento.txtComplemento.Text;
            complemento.Dispose();

            

            try
            {
                string sql = "START TRANSACTION;" +
                             "UPDATE vendas SET infadprod='" + compDescricao + "' WHERE inc ='" + dtgItens.CurrentRow.Cells[7].Value.ToString() + "' ;" +
                             "COMMIT;";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
            }
            catch (Exception)
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.EntityClient;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace SICEpdv
{
    public partial class FrmResumoCaixa : Form
    {
        public string operadorCaixa { get; set; }

        public FrmResumoCaixa()
        {
            InitializeComponent();
            ResumoCaixa resumo = new ResumoCaixa();
            if (resumo.SaldoCaixa(GlbVariaveis.glb_Usuario) == true)
            {
                lblSaldoInicial.Text = String.Format("{0:N2}", resumo.saldoInicial.GetValueOrDefault() + resumo.suprimento.GetValueOrDefault());
                lblSUTR.Text = String.Format("{0:N2}", resumo.suprimentoTR.GetValueOrDefault());
                lblAbertura.Text = resumo.abertura.ToString();
                lblDH.Text = String.Format("{0:N2}", resumo.dinheiro.GetValueOrDefault());
                lblCH.Text = String.Format("{0:N2}", resumo.cheque.GetValueOrDefault());

                lblCA.Text = String.Format("{0:N2}", (resumo.cartao.GetValueOrDefault() +resumo.cartaoPresente.GetValueOrDefault() + resumo.financeira.GetValueOrDefault()));
                lblTI.Text = String.Format("{0:N2}", resumo.ticket.GetValueOrDefault());
                lblPF.Text = String.Format("{0:N2}", resumo.fidelidade.GetValueOrDefault());
                lblCR.Text = String.Format("{0:N2}", resumo.crediario.GetValueOrDefault() + resumo.financiamento.GetValueOrDefault());
                lblAV.Text = String.Format("{0:N2}", resumo.vendaAV.GetValueOrDefault());
                lblREC.Text = String.Format("{0:N2}", resumo.recebimento.GetValueOrDefault());
                lblRecDH.Text = String.Format("{0:N2}", resumo.recDH.GetValueOrDefault());
                lblRecCH.Text = String.Format("{0:N2}", resumo.recCH.GetValueOrDefault());
                lblRecCartao.Text = String.Format("{0:N2}", resumo.recCA.GetValueOrDefault());
                lblDevolucao.Text = String.Format("{0:N2}", resumo.devolucao.GetValueOrDefault());
                lblCancelamento.Text = String.Format("{0:N2}", resumo.cancelamento.GetValueOrDefault());
                lblSangria.Text = String.Format("{0:N2}", resumo.sangria.GetValueOrDefault());
                lblSaldoFinal.Text = String.Format("{0:N2}", resumo.saldoFinal);
                lblSaldoLiq.Text = String.Format("{0:N2}", resumo.saldoLiquido);//resumo.saldoFinal-resumo.crediario.GetValueOrDefault()-resumo.cartao.GetValueOrDefault()-resumo.ticket.GetValueOrDefault()-resumo.vendaAV.GetValueOrDefault()-resumo.recebimento.GetValueOrDefault()+(resumo.recDH.GetValueOrDefault()+resumo.recCH.GetValueOrDefault()) );
            }
            else
            {
                MessageBox.Show("Verifique Sua Conexao", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lblOperador.Text = GlbVariaveis.glb_Usuario;
            

            if (Permissoes.fechamentocaixadigitacao)
            {
                pnResumoCaixa.Visible = true;
                btnImprimir.Enabled = false;
            }
            else
            {
                pnResumoCaixa.Visible = false;
                btnImprimir.Enabled = true;
            }


            #region Dinheiro

            txtDinheiro.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);

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

                    evento.SuppressKeyPress = true;
                    txtCartao.Focus();
                    calculaResumoCaixa();
                }
            };
            txtDinheiro.Enter += (objeto, evento) =>
            {
                try
                {
                    txtDinheiro.Focus();
                    txtDinheiro.SelectAll();
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
                if (evento.KeyChar == 27);

            };
            txtCartao.KeyDown += (objeto, evento) =>
            {


                if (evento.KeyValue == 13)
                {
                    txtCartao.Text = Funcoes.FormatarDecimal(txtCartao.Text);

                    if (Convert.ToDecimal(txtCartao.Text) < 0)
                    {
                        txtCartao.Text = "0,00";
                    }

                    evento.SuppressKeyPress = true;
                    txtCheque.Focus();
                    calculaResumoCaixa();
                }
            };
            txtCartao.Enter += (objeto, evento) =>
            {
                try
                {
                    txtCartao.Focus();
                    txtCartao.SelectAll();
                    Application.DoEvents();
                }
                finally
                {
                    Funcoes.TravarTeclado(false);
                }
            };
            #endregion

            #region Cheque

            txtCheque.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
                if (evento.KeyChar == 27) ;

            };
            txtCheque.KeyDown += (objeto, evento) =>
            {


                if (evento.KeyValue == 13)
                {
                    txtCheque.Text = Funcoes.FormatarDecimal(txtCheque.Text);

                    if (Convert.ToDecimal(txtCheque.Text) < 0)
                    {
                        txtCheque.Text = "0,00";
                    }

                    evento.SuppressKeyPress = true;
                    calculaResumoCaixa();
                }
            };
            txtCheque.Enter += (objeto, evento) =>
            {
                try
                {
                    txtCheque.Focus();
                    txtCheque.SelectAll();
                    Application.DoEvents();
                }
                finally
                {
                    Funcoes.TravarTeclado(false);
                }
            };
            #endregion


        }

        private class ResumoCaixa
        {
            public decimal? saldoInicial { get; set; }
            public string abertura { get; set; }
            public decimal? suprimento { get; set; }
            public decimal? suprimentoTR { get; set; }
            /// <summary>
            /// Dinheiro
            /// </summary>
            public decimal? dinheiro { get; set; }
            public decimal? entradaDH { get; set; }
            public decimal? entradaCH { get; set; }
            public decimal? entradaCA { get; set; }
            public decimal? entradaTI { get; set; }
            /// <summary>
            /// Cheque
            /// </summary>
            public decimal? cheque { get; set; }
            public decimal? chequeVISTA { get; set; }
            public decimal? chequePRE { get; set; }
            
            /// <summary>
            /// Cartao
            /// </summary>
            public decimal? cartao { get; set; }
            public decimal? cartaoCR { get; set; }
            public decimal? cartaoDB { get; set; }
            public decimal? financiamento { get; set; }
            public decimal? financeira { get; set; }            
            /// <summary>
            /// Cartao
            /// </summary>                        
            public decimal? ticket { get; set; }
            /// <summary>
            /// Crediario            
            /// </summary>            
            public decimal? crediario { get; set; }
            /// <summary>
            /// Recebimento
            /// </summary>
            /// 
            public decimal? fidelidade { get; set; }
            public decimal? cartaoPresente { get; set; }

            public decimal? vendaAV { get; set; }            
            public decimal? devolucao { get; set; }
            public decimal? devolucaoCR { get; set; }
            public decimal? devolucaoPRD { get; set; }
            public decimal? cancelamento { get; set; }

            /// <summary>
            /// digitacao // markcvaldo
            /// </summary>
            /// 
            public decimal? digitacaoDH { get; set; }
            public decimal? digitacaoCH { get; set; }
            public decimal? digitacaoCA { get; set; }


            public decimal? recebimento { get; set; }
            public decimal? recDH { get; set; }
            public decimal? recCH { get; set; }
            public decimal? recCA { get; set; }
            public decimal? recDC { get; set; }
            public decimal? recBL { get; set; }
            public decimal? recEncargos { get; set; }
            public decimal? encargosREC { get; set; }

            public decimal? receitaJuros { get; set; }
            public decimal? jurosMora { get; set; }
            public decimal? jurosCheque { get; set; }
            public decimal? jurosCartao { get; set; }

            public decimal? renegociacao { get; set; }
            public decimal? jurosRN { get; set; }
            public decimal? descontoCapitalRN { get; set; }

            public decimal? perdao { get; set; }
            public decimal? jurosPerdao { get; set; }

            /// <summary>
            /// Vendas
            /// </summary>
            public decimal? totalVenda { get; set; }
            public decimal? totalCustos { get; set; }
            public decimal? totalLucro { get; set; }
            public decimal? encargosGerados { get; set; }
            public decimal? totalDescontosVenda { get; set; }
            public int? totalCupons { get; set; }

            /// <summary>
            /// Servicos
            /// </summary>

            public decimal? valorServicos { get; set; }
            public decimal? descontoServicos { get; set; }

            public decimal? sangria { get; set; }
            public decimal saldoFinal { get; set; }
            public decimal saldoLiquido { get; set; }

            public bool SaldoCaixa(string operador)
            {                                
                #region StandAlone
                if (!Conexao.onLine)
                {
                    using (IObjectContainer tabela = Db4oFactory.OpenFile("caixa.yap"))
                    {
                        this.saldoInicial = (from StandAloneCaixa n in tabela
                                             where n.operador == operador && n.tipoPagamento == "SI"
                                             && n.encerrado == false
                                             select (decimal?)n.valor).Sum();
                        abertura = Convert.ToString((from StandAloneCaixa n in tabela
                                                     where n.operador == operador && n.tipoPagamento == "SI"
                                                     select n.horaabertura).FirstOrDefault());
                        if (string.IsNullOrEmpty(abertura))
                            abertura = DateTime.Now.TimeOfDay.ToString();


                        dinheiro = (from StandAloneCaixa n in tabela
                                    where n.operador == operador && n.tipoPagamento == "DH"
                                    && n.encerrado == false
                                    select (decimal?)n.valor).Sum();
                    };

                    using (IObjectContainer tabVenda = Db4oFactory.OpenFile("venda.yap"))
                    {
                        totalDescontosVenda = (from StandAloneVenda n in tabVenda
                                               where n.operador == operador
                                               && n.tipo == "0 - Produto"
                                               select (decimal?)n.ratdesc + n.descontovalor * (n.quantidade)).Sum();
                    };

                    return true;
                };

                #endregion

                siceEntities conexao;
               
               if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
                   conexao = Conexao.CriarEntidade(false);
                else
                   conexao = Conexao.CriarEntidade();

                fluxoCaixa saldoCaixaSP = new fluxoCaixa();

                try
                {
                    saldoCaixaSP = conexao.ExecuteStoreQuery<fluxoCaixa>("CALL fluxocaixa('" + operador + "','" + GlbVariaveis.glb_filial + "');").FirstOrDefault();

                    saldoInicial = saldoCaixaSP.saldo;
                    abertura = saldoCaixaSP.horasAbertura.ToShortTimeString();
                    suprimento = 0;//saldoCaixaSP.suprimento;
                    suprimentoTR = 0;
                    dinheiro = saldoCaixaSP.dinheiro;
                    entradaDH = saldoCaixaSP.entradaDH;
                    entradaCH = saldoCaixaSP.entradaCH;
                    entradaCA = saldoCaixaSP.entradaCA;
                    entradaTI = 0;
                    cheque = saldoCaixaSP.cheque;
                    cartao = saldoCaixaSP.cartao;                    
                    financiamento = saldoCaixaSP.financiamento;
                    ticket = saldoCaixaSP.ticket;
                    fidelidade = saldoCaixaSP.fidelidade;
                    cartaoPresente = saldoCaixaSP.cartaoPresente;
                    crediario = saldoCaixaSP.crediario;
                    vendaAV = saldoCaixaSP.vendaAV;
                    devolucao = saldoCaixaSP.devolucao;
                    devolucaoPRD = saldoCaixaSP.devolucaoVenda;
                    devolucaoCR = saldoCaixaSP.devolucaoRec;
                    recebimento = saldoCaixaSP.recebimento;
                    recDH = saldoCaixaSP.recebimentoDH;
                    recCH = saldoCaixaSP.recebimentoCH;
                    recCA = saldoCaixaSP.recebimentoCA;
                    recBL = saldoCaixaSP.recebimentoBL;
                    recDC = saldoCaixaSP.recebimentoDC;
                    encargosREC = saldoCaixaSP.encargosRecebidos;
                    sangria = saldoCaixaSP.sangria;
                    totalCupons = saldoCaixaSP.qtdCupons;
                    saldoFinal = saldoCaixaSP.saldoFinal;
                    valorServicos = saldoCaixaSP.valorServicos;
                    descontoServicos = saldoCaixaSP.descontoServicos;
                    encargosGerados = saldoCaixaSP.encargos;
                    totalDescontosVenda = saldoCaixaSP.descontoTotalVenda;
                    totalVenda = saldoCaixaSP.totalVenda;
                    totalCustos = saldoCaixaSP.totalCustos;
                    receitaJuros = saldoCaixaSP.juros;
                    jurosCheque = saldoCaixaSP.jurosRecebimentoCH;
                    jurosCartao = saldoCaixaSP.jurosRecebimentoCA;
                    renegociacao = saldoCaixaSP.renegociacao;
                    jurosRN = saldoCaixaSP.jurosRenegociacao;
                    descontoCapitalRN = saldoCaixaSP.descontoCapitalRN;
                    perdao = saldoCaixaSP.perdao;
                    jurosPerdao = saldoCaixaSP.jurosPerdao;
                    totalLucro = totalVenda - totalCustos;
                    saldoLiquido = saldoCaixaSP.saldoFinalLiquidoEspecie;
                    financeira = saldoCaixaSP.financeira;
                    cancelamento = saldoCaixaSP.cancelamento;

                    #region saldo PDV
                    /*
                saldoInicial = (from n in entidade.caixa
                                where n.operador == operador && n.tipopagamento == "SI"
                                && n.CodigoFilial == GlbVariaveis.glb_filial
                                select (decimal?)n.valor).Sum();                

                abertura = Convert.ToString((from n in entidade.caixa
                            where n.operador == operador && n.tipopagamento == "SI"
                            && n.CodigoFilial == GlbVariaveis.glb_filial
                            select n.horaabertura).FirstOrDefault());
                // Usado essa conversao aqui por que se nao existirem dados no periodo
                if (string.IsNullOrEmpty(abertura))
                    abertura = DateTime.Now.TimeOfDay.ToString();

                 #region Exemplo de como pode ser sem o uso de nullable ?
                 /// Existe a propriedade dinheiro e a variável dinheiro
                /// embora tenham o mesmo nome possuem valores diferentes. 
                /// Usado para quando o retorno da tabela for null 
                /// 
                /// 
                //System.Nullable<decimal> saldoInicial = (from n in entidade.caixa
                //                                         where n.operador == operador && n.tipopagamento == "SI"
                //                                         select (decimal?)n.valor).Sum();
                //this.saldoInicial = saldoInicial == null ? 0 : saldoInicial.Value;
                 //System.Nullable<decimal> dinheiro = (from n in entidade.caixa
                 //                                     where n.operador == operador && n.tipopagamento == "DH"
                 //                                     select (decimal?)n.valor).Sum();
                 //this.dinheiro = dinheiro.GetValueOrDefault();
                 #endregion
                 suprimento = (from n in entidade.caixa
                                 where n.operador == operador && n.tipopagamento == "SU"
                                 && n.CodigoFilial == GlbVariaveis.glb_filial
                                 select (decimal?)n.valor).Sum();
                 
                //suprimentoTR é apenas informativo não somar pois já está somando na variável suprimento
                 suprimentoTR = (from n in entidade.caixa
                               where n.operador == operador && n.tipopagamento == "SU"
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               && n.historico.Contains("Transferência")
                               select (decimal?)n.valor).Sum();

                 dinheiro = (from n in entidade.caixa
                               where n.operador == operador && n.tipopagamento == "DH"
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               && !n.dpfinanceiro.Contains("Receb")
                               select (decimal?)n.valor).Sum();
                 
                entradaDH = (from n in entidade.caixa
                              where n.operador == operador && n.tipopagamento == "DH"
                              && n.CodigoFilial == GlbVariaveis.glb_filial
                              && !n.dpfinanceiro.Contains("Receb")
                              && n.historico.StartsWith("Entrada")
                              select (decimal?)n.valor).Sum();

                entradaCH = (from n in entidade.caixa
                             where n.operador == operador && n.tipopagamento == "CH"
                             && n.CodigoFilial == GlbVariaveis.glb_filial
                             && !n.dpfinanceiro.Contains("Receb")
                             && n.historico.StartsWith("Entrada")
                             select (decimal?)n.valor).Sum();

                entradaCA = (from n in entidade.caixa
                             where n.operador == operador && n.tipopagamento == "CA"
                             && !n.dpfinanceiro.Contains("Receb")
                             && n.historico.StartsWith("Entrada")
                             select (decimal?)n.valor).Sum();

                entradaTI = (from n in entidade.caixa
                             where n.operador == operador && n.tipopagamento == "TI"
                             && n.CodigoFilial == GlbVariaveis.glb_filial
                             && n.historico.StartsWith("Entrada")
                             select (decimal?)n.valor).Sum();

                 cheque = (from n in entidade.caixa
                             where n.operador == operador && n.tipopagamento == "CH"
                             && !n.dpfinanceiro.Contains("Receb")
                            select (decimal?)n.valor).Sum();

                 cartao = (from n in entidade.caixa
                           where n.operador == operador && (n.tipopagamento == "CA"
                           || n.tipopagamento=="FN")
                           && n.CodigoFilial == GlbVariaveis.glb_filial
                           && !n.dpfinanceiro.Contains("Receb")
                           select (decimal?)n.valor).Sum();

           financiamento = (from n in entidade.caixa
                           where n.operador == operador && n.tipopagamento == "FN"
                           && n.CodigoFilial == GlbVariaveis.glb_filial
                           && !n.dpfinanceiro.Contains("Receb")
                           select (decimal?)n.valor).Sum();

                ticket = (from n in entidade.caixa
                            where n.operador == operador && n.tipopagamento == "TI"
                            && !n.dpfinanceiro.Contains("Receb")
                            select (decimal?)n.valor).Sum();
                

                crediario = (from n in entidade.caixa
                                where n.operador == operador && n.tipopagamento == "CR"
                                && n.CodigoFilial == GlbVariaveis.glb_filial
                                select (decimal?)n.valor).Sum();

                vendaAV = (from n in entidade.caixa
                             where n.operador == operador && n.tipopagamento == "AV"
                             && n.dpfinanceiro=="Venda"
                             && n.CodigoFilial == GlbVariaveis.glb_filial
                             select (decimal?)n.valor).Sum();                

                devolucao = (from n in entidade.caixa
                             where n.operador == operador && n.tipopagamento == "DV"
                             && n.CodigoFilial == GlbVariaveis.glb_filial
                             select (decimal?)n.valor).Sum();

                devolucaoPRD = (from n in entidade.caixa
                                where n.operador == operador && n.tipopagamento == "DV"
                                && n.CodigoFilial == GlbVariaveis.glb_filial
                                && n.dpfinanceiro != "Recebimento"
                                && n.dpfinanceiro != "Recebimento est"
                                && n.dpfinanceiro != "Recebimento s/j"
                                select (decimal?)n.valor).Sum();

                devolucaoCR = (from n in entidade.caixa
                             where n.operador == operador && n.tipopagamento == "DV"
                             && n.CodigoFilial == GlbVariaveis.glb_filial
                             && n.dpfinanceiro == "Recebimento"
                             select (decimal?)n.valor).Sum();


                recebimento = (from n in entidade.caixa
                                 where n.operador == operador && n.dpfinanceiro.Contains("Receb")
                                 && n.CodigoFilial == GlbVariaveis.glb_filial
                                 && !n.tipopagamento.Contains("RN")
                                 && !n.tipopagamento.Contains("DV")
                                 && !n.tipopagamento.Contains("PD")
                                 select (decimal?)n.valor).Sum();

                recDH = (from n in entidade.caixa
                               where n.operador == operador && n.dpfinanceiro.Contains("Receb")
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               && n.tipopagamento=="DH"
                               && !n.tipopagamento.Contains("RN")
                               && !n.tipopagamento.Contains("DV")
                               && !n.tipopagamento.Contains("PD")
                               select (decimal?)n.valor).Sum();
                
                recCH = (from n in entidade.caixa
                         where n.operador == operador && n.dpfinanceiro.Contains("Receb")
                         && n.CodigoFilial == GlbVariaveis.glb_filial
                         && n.tipopagamento == "CH"
                         && !n.tipopagamento.Contains("RN")
                         && !n.tipopagamento.Contains("DV")
                         && !n.tipopagamento.Contains("PD")
                         select (decimal?)n.valor).Sum();

                recCA = (from n in entidade.caixa
                         where n.operador == operador && n.dpfinanceiro.Contains("Receb")
                         && n.CodigoFilial == GlbVariaveis.glb_filial
                         && n.tipopagamento == "CA"
                         && !n.tipopagamento.Contains("RN")
                         && !n.tipopagamento.Contains("DV")
                         && !n.tipopagamento.Contains("PD")
                         select (decimal?)n.valor).Sum();

                recBL = (from n in entidade.caixa
                         where n.operador == operador && n.dpfinanceiro.Contains("Receb")
                         && n.CodigoFilial == GlbVariaveis.glb_filial
                         && n.tipopagamento == "BL"
                         && !n.tipopagamento.Contains("RN")
                         && !n.tipopagamento.Contains("DV")
                         && !n.tipopagamento.Contains("PD")
                         select (decimal?)n.valor).Sum();

                recDC = (from n in entidade.caixa
                         where n.operador == operador && n.dpfinanceiro.Contains("Receb")
                         && n.CodigoFilial == GlbVariaveis.glb_filial
                         && n.tipopagamento == "DC"
                         && !n.tipopagamento.Contains("RN")
                         && !n.tipopagamento.Contains("DV")
                         && !n.tipopagamento.Contains("PD")
                         select (decimal?)n.valor).Sum();


                encargosREC = (from n in entidade.caixa
                               where n.operador == operador && n.dpfinanceiro.Contains("Receb")
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                                && !n.tipopagamento.Contains("RN")
                                && !n.tipopagamento.Contains("DV")
                                && !n.tipopagamento.Contains("PD")
                               select (decimal?)n.encargos).Sum();
                
                // Essa data limite a soma em dias dias por a flag encerrado
                // é que sinalizada se o movimento foi encerrado ou não
                // Foi colocado essa data limite para evitar uma soma de toda a tabela.
                DateTime dataInicio = (GlbVariaveis.Sys_Data.AddDays(-15));
                sangria = (from n in entidade.movdespesas
                             where n.operador == operador 
                             && n.codigofilial == GlbVariaveis.glb_filial
                             && n.sangria == "S" && n.encerrado == "N"
                             && n.data>= dataInicio
                             select (decimal?)n.valor).Sum();
                
                totalCupons = (from n in entidade.contdocs
                               where n.operador == operador
                               && n.data == GlbVariaveis.Sys_Data
                               && n.dpfinanceiro == "Venda"
                               && n.concluido == "S"
                               && n.estornado != "S"
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               select n.operador).Count();

                this.saldoFinal = (saldoInicial.GetValueOrDefault() + suprimento.GetValueOrDefault() + dinheiro.GetValueOrDefault() + cheque.GetValueOrDefault() + cartao.GetValueOrDefault() +vendaAV.GetValueOrDefault()+
                     ticket.GetValueOrDefault() + crediario.GetValueOrDefault() + recebimento.GetValueOrDefault()) - sangria.GetValueOrDefault();


                valorServicos = (from n in entidade.venda
                                 where n.operador == operador
                                 && n.codigofilial == GlbVariaveis.glb_filial
                                 && n.tipo == "1 - Servico"
                                 && n.cancelado =="N"
                                 select (decimal?)n.total).Sum();

                descontoServicos = (from n in entidade.venda
                                    where n.operador == operador
                                    && n.codigofilial == GlbVariaveis.glb_filial
                                    && n.tipo == "1 - Servico"
                                    && n.cancelado == "N"
                                    select (decimal?)n.ratdesc+n.descontovalor).Sum();


                encargosGerados = (from n in entidade.venda
                               where n.operador == operador
                               && n.codigofilial == GlbVariaveis.glb_filial
                               && n.cancelado == "N"
                               select (decimal?)n.rateioencargos).Sum();

                totalDescontosVenda = (from n in entidade.venda
                                    where n.operador == operador
                                    && n.codigofilial == GlbVariaveis.glb_filial
                                    && n.tipo == "0 - Produto"
                                    && n.cancelado == "N"
                                    select (decimal?)n.ratdesc + n.descontovalor).Sum();
                
                totalVenda = (from n in entidade.venda
                              where n.operador == operador
                              && n.codigofilial == GlbVariaveis.glb_filial
                              && n.cancelado == "N"
                              select (decimal?)n.total-n.ratdesc+n.rateioencargos).Sum();

                totalCustos = (from n in entidade.venda
                               where n.operador == operador
                               && n.codigofilial == GlbVariaveis.glb_filial
                               && n.cancelado == "N"
                               select (decimal?)n.quantidade*n.embalagem*n.custo).Sum();

                receitaJuros = (from n in entidade.caixa
                                where n.operador == operador && n.dpfinanceiro.Contains("Receb")
                                && (n.tipopagamento!="DV" && n.tipopagamento!="RN" && n.tipopagamento!="PD")
                                && n.CodigoFilial == GlbVariaveis.glb_filial
                                select (decimal?)n.VrJuros - n.vrdesconto).Sum();

                jurosCheque = (from n in entidade.caixa
                               where n.operador == operador && n.dpfinanceiro.Contains("Receb")
                               && n.tipopagamento == "CH"
                               && (n.tipopagamento != "DV" && n.tipopagamento != "RN" && n.tipopagamento != "PD")
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               select (decimal?)n.VrJuros - n.vrdesconto).Sum();

                jurosCartao = (from n in entidade.caixa
                               where n.operador == operador && n.dpfinanceiro.Contains("Receb")
                               && n.tipopagamento == "CA"
                               && (n.tipopagamento != "DV" && n.tipopagamento != "RN" && n.tipopagamento != "PD")
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               select (decimal?)n.VrJuros - n.vrdesconto).Sum();

                renegociacao = (from n in entidade.caixa
                                where n.operador == operador
                                && n.CodigoFilial == GlbVariaveis.glb_filial
                                && n.tipopagamento == "RN"
                                select (decimal?)n.valor).Sum();

                if (renegociacao.GetValueOrDefault() > 0)
                {
                    jurosRN = (from n in entidade.caixa
                                    where n.operador == operador
                                    && n.CodigoFilial == GlbVariaveis.glb_filial
                                    && n.tipopagamento == "RN"
                                    select (decimal?)n.VrJuros-n.vrdesconto).Sum();
                    descontoCapitalRN = (from n in entidade.caixa
                                       where n.operador == operador
                                       && n.CodigoFilial == GlbVariaveis.glb_filial
                                       && n.tipopagamento == "RN"
                                       select (decimal?)n.vrdesconto).Sum();
                }


                perdao = (from n in entidade.caixa
                                where n.operador == operador
                                && n.CodigoFilial == GlbVariaveis.glb_filial
                                && n.tipopagamento == "PD"
                                select (decimal?)n.valor).Sum();

                jurosPerdao = (from n in entidade.caixa
                          where n.operador == operador
                          && n.CodigoFilial == GlbVariaveis.glb_filial
                          && n.tipopagamento == "PD"
                          select (decimal?)n.VrJuros-n.vrdesconto).Sum();                

                
                totalLucro = totalVenda - totalCustos;
                */
                    #endregion

                    return true;
                }
                catch (Exception erro)
                {
                    //MessageBox.Show(erro.ToString());
                    return false;
                }

                
            }

            public bool EncerrarCaixa(string operador)
            {
                try
                {
                    SaldoCaixa(operador);

                    #region StandAlone
                    if (!Conexao.onLine)
                    {
                        using (IObjectContainer tabelaEncerra = Db4oFactory.OpenFile("caixa.yap"))
                        {
                            var dados = (from StandAloneCaixa n in tabelaEncerra
                                         where n.operador == operador
                                         && n.encerrado == false
                                         select n).ToList();
                            foreach (var item in dados)
                            {
                                item.encerrado = true;
                                tabelaEncerra.Store(item);
                            }
                        };

                        using (IObjectContainer tabelaCaixaSoma = Db4oFactory.OpenFile("caixassoma.yap"))
                        {
                            StandAloneCaixasSoma registro = new StandAloneCaixasSoma();
                            registro.data = GlbVariaveis.Sys_Data;
                            registro.horaabertura = Convert.ToDateTime(abertura).TimeOfDay;
                            registro.horafechamento = DateTime.Now.TimeOfDay;
                            registro.operador = operador;
                            registro.saldoInicial = saldoInicial.GetValueOrDefault()+suprimento.GetValueOrDefault();
                            registro.dinheiro = dinheiro.GetValueOrDefault();
                            registro.descontoVenda = totalDescontosVenda.GetValueOrDefault();
                            registro.saldoFinal = saldoFinal;
                            tabelaCaixaSoma.Store(registro);
                        }

                        return true;
                    }
                    #endregion

                      //o gravar caixaSoma o mesmo está sendo chamado no encerrar caixa
                        #region
                        /*
                         * 
                       try
                       {
                      
                        siceEntities entidade = Conexao.CriarEntidade();

                        DateTime dataEncerramento = GlbVariaveis.Sys_Data;


                        var dataMovimento = (from n in entidade.caixa
                                             where n.operador == GlbVariaveis.glb_Usuario
                                             && n.CodigoFilial == GlbVariaveis.glb_filial
                                             select n.data).FirstOrDefault();

                        if (dataMovimento.HasValue)
                            dataEncerramento = dataMovimento.Value;

                        caixassoma tabela = new caixassoma();
                        tabela.codigofilial = GlbVariaveis.glb_filial;
                        tabela.data = dataEncerramento; // GlbVariaveis.Sys_Data;
                        tabela.operador = operador;
                        tabela.horaabertura = Convert.ToDateTime(abertura).TimeOfDay;
                        tabela.horafechamento = DateTime.Now.TimeOfDay;
                        tabela.saldo = saldoInicial.GetValueOrDefault() + suprimento.GetValueOrDefault();
                        tabela.dinheiro = dinheiro.GetValueOrDefault();
                        tabela.entradadh = entradaDH.GetValueOrDefault();
                        tabela.entradach = entradaCH.GetValueOrDefault();
                        tabela.entradaca = entradaCA.GetValueOrDefault();
                        tabela.cheque = cheque.GetValueOrDefault();
                        tabela.chequepre = chequePRE.GetValueOrDefault();
                        tabela.cartao = cartao.GetValueOrDefault() - financiamento.GetValueOrDefault();
                        tabela.ticket = ticket.GetValueOrDefault();
                        tabela.crediario = crediario.GetValueOrDefault();
                        tabela.vendaAV = vendaAV.GetValueOrDefault();
                        tabela.recebimento = recebimento.GetValueOrDefault();
                        tabela.devolucao = devolucao.GetValueOrDefault();
                        tabela.devolucaocr = devolucaoCR.GetValueOrDefault();
                        tabela.devolucaoprd = devolucaoPRD.GetValueOrDefault();

                        tabela.recebimentodh = recDH.GetValueOrDefault();
                        tabela.recebimentoch = recCH.GetValueOrDefault();
                        tabela.recebimentoca = recCA.GetValueOrDefault();
                        tabela.recebimentobl = recBL.GetValueOrDefault();
                        tabela.recebimentodc = recDC.GetValueOrDefault();
                        tabela.encargosrecebidos = encargosREC.GetValueOrDefault();                        

                        tabela.sangria = sangria.GetValueOrDefault();
                        tabela.SaldoCaixa = saldoFinal;

                        tabela.valorservicos = valorServicos.GetValueOrDefault();
                        tabela.descontoservicos = descontoServicos.GetValueOrDefault();

                        tabela.vendas = totalVenda.GetValueOrDefault();
                        tabela.custos = totalCustos.GetValueOrDefault();
                        tabela.encargos = encargosGerados.GetValueOrDefault();
                        tabela.descontovenda = totalDescontosVenda.GetValueOrDefault();
                        tabela.financeira = financiamento.GetValueOrDefault();
                        tabela.juros = receitaJuros.GetValueOrDefault();
                        tabela.jurosrecca = jurosCartao.GetValueOrDefault();
                        tabela.jurosrecch = jurosCheque.GetValueOrDefault();
                        tabela.recebimentoAV = 0;
                        tabela.renegociacao = renegociacao.GetValueOrDefault();
                        tabela.jurosrenegociacao = jurosRN.GetValueOrDefault();
                        tabela.descontocapitalrn = descontoCapitalRN.GetValueOrDefault();
                        tabela.qtdcupons = totalCupons.GetValueOrDefault();

                        tabela.perdao = perdao.GetValueOrDefault();
                        tabela.jurosperdao = jurosPerdao.GetValueOrDefault();

                        tabela.digitacaodh = digitacaoDH;
                        tabela.digitacaoca = digitacaoCA;
                        tabela.digitacaoch = digitacaoCH;

                        entidade.AddTocaixassoma(tabela);
                        entidade.SaveChanges();

                        if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == true)
                        {

                            siceEntities entidadeOff = Conexao.CriarEntidade(false);
                            caixassoma tabelaOff = new caixassoma();
                            tabelaOff.codigofilial = GlbVariaveis.glb_filial;
                            tabelaOff.data = dataEncerramento; // GlbVariaveis.Sys_Data;
                            tabelaOff.operador = operador;
                            tabelaOff.horaabertura = Convert.ToDateTime(abertura).TimeOfDay;
                            tabelaOff.horafechamento = DateTime.Now.TimeOfDay;
                            tabelaOff.saldo = saldoInicial.GetValueOrDefault() + suprimento.GetValueOrDefault();
                            tabelaOff.dinheiro = dinheiro.GetValueOrDefault();
                            tabelaOff.entradadh = entradaDH.GetValueOrDefault();
                            tabelaOff.entradach = entradaCH.GetValueOrDefault();
                            tabelaOff.entradaca = entradaCA.GetValueOrDefault();
                            tabelaOff.cheque = cheque.GetValueOrDefault();
                            tabelaOff.chequepre = chequePRE.GetValueOrDefault();
                            tabelaOff.cartao = cartao.GetValueOrDefault() - financiamento.GetValueOrDefault();
                            tabelaOff.ticket = ticket.GetValueOrDefault();
                            tabelaOff.crediario = crediario.GetValueOrDefault();
                            tabelaOff.recebimento = recebimento.GetValueOrDefault();
                            tabelaOff.devolucao = devolucao.GetValueOrDefault();
                            tabelaOff.devolucaocr = devolucaoCR.GetValueOrDefault();
                            tabelaOff.devolucaoprd = devolucaoPRD.GetValueOrDefault();

                            tabelaOff.recebimentodh = recDH.GetValueOrDefault();
                            tabelaOff.recebimentoch = recCH.GetValueOrDefault();
                            tabelaOff.recebimentoca = recCA.GetValueOrDefault();
                            tabelaOff.recebimentobl = recBL.GetValueOrDefault();
                            tabelaOff.recebimentodc = recDC.GetValueOrDefault();
                            tabelaOff.encargosrecebidos = encargosREC.GetValueOrDefault();

                            tabelaOff.sangria = sangria.GetValueOrDefault();
                            tabelaOff.SaldoCaixa = saldoFinal;

                            tabelaOff.valorservicos = valorServicos.GetValueOrDefault();
                            tabelaOff.descontoservicos = descontoServicos.GetValueOrDefault();

                            tabelaOff.vendas = totalVenda.GetValueOrDefault();
                            tabelaOff.custos = totalCustos.GetValueOrDefault();
                            tabelaOff.encargos = encargosGerados.GetValueOrDefault();
                            tabelaOff.descontovenda = totalDescontosVenda.GetValueOrDefault();
                            tabelaOff.financeira = financiamento.GetValueOrDefault();
                            tabelaOff.juros = receitaJuros.GetValueOrDefault();
                            tabelaOff.jurosrecca = jurosCartao.GetValueOrDefault();
                            tabelaOff.jurosrecch = jurosCheque.GetValueOrDefault();
                            tabelaOff.recebimentoAV = 0;
                            tabelaOff.renegociacao = renegociacao.GetValueOrDefault();
                            tabelaOff.jurosrenegociacao = jurosRN.GetValueOrDefault();
                            tabelaOff.descontocapitalrn = descontoCapitalRN.GetValueOrDefault();
                            tabelaOff.qtdcupons = totalCupons.GetValueOrDefault();

                            tabelaOff.perdao = perdao.GetValueOrDefault();
                            tabelaOff.jurosperdao = jurosPerdao.GetValueOrDefault();

                            tabelaOff.digitacaodh = digitacaoDH;
                            tabelaOff.digitacaoca = digitacaoCA;
                            tabelaOff.digitacaoch = digitacaoCH;

                            entidadeOff.AddTocaixassoma(tabelaOff);
                            entidadeOff.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Gravando totais no caixassoma: " + ex.Message);
                    }
                    */
                    #endregion


                    using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                    {
                        conn.Open();
                        EntityCommand cmd = conn.CreateCommand();
                        cmd.CommandTimeout = 3600;
                        cmd.CommandText = "siceEntities.EncerrarCaixa";
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.StoredProcedure;

                        EntityParameter idOperador = cmd.Parameters.Add("idOperador", DbType.String);
                        idOperador.Direction = ParameterDirection.Input;
                        idOperador.Value = operador;

                        EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                        filial.Direction = ParameterDirection.Input;
                        filial.Value = GlbVariaveis.glb_filial;

                        EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                        ip.Direction = ParameterDirection.Input;
                        ip.Value = GlbVariaveis.glb_IP;


                        EntityParameter ecfFab = cmd.Parameters.Add("nrFabricaoECF", DbType.String);
                        ip.Direction = ParameterDirection.Input;
                        ecfFab.Value = "";

                        cmd.ExecuteNonQuery();
                    }

                    if ((Conexao.tipoConexao == 2 || Conexao.tipoConexao == 3) && Conexao.ConexaoOnline() == true)
                    {

                        using (EntityConnection conn = new EntityConnection(Conexao.stringConexaoRemoto))
                        {
                            conn.Open();
                            EntityCommand cmd = conn.CreateCommand();
                            cmd.CommandTimeout = 3600;
                            cmd.CommandText = "siceEntities.EncerrarCaixa";
                            cmd.CommandTimeout = 3600;
                            cmd.CommandType = CommandType.StoredProcedure;

                            EntityParameter idOperador = cmd.Parameters.Add("idOperador", DbType.String);
                            idOperador.Direction = ParameterDirection.Input;
                            idOperador.Value = operador;

                            EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                            filial.Direction = ParameterDirection.Input;
                            filial.Value = GlbVariaveis.glb_filial;

                            EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                            ip.Direction = ParameterDirection.Input;
                            ip.Value = GlbVariaveis.glb_IP;


                            EntityParameter ecfFab = cmd.Parameters.Add("nrFabricaoECF", DbType.String);
                            ip.Direction = ParameterDirection.Input;
                            ecfFab.Value = ConfiguracoesECF.nrFabricacaoECF;

                            cmd.ExecuteNonQuery();
                        } 
                    }

                    try
                    { 
                    
                        int codigoCaixa = Conexao.CriarEntidade().ExecuteStoreQuery<int>("SELECT IFNULL(MAX(c.inc),0) as codigo FROM caixassoma AS c WHERE c.codigoFilial ='" + GlbVariaveis.glb_filial + "' AND c.operador = '" + GlbVariaveis.glb_Usuario + "'").SingleOrDefault();

                        siceEntities entidade = Conexao.CriarEntidade();

                        var caixassoma = (from c in entidade.caixassoma
                                          where c.codigofilial == GlbVariaveis.glb_filial && c.operador == GlbVariaveis.glb_Usuario
                                          && c.inc == codigoCaixa
                                          select c).FirstOrDefault();

                        caixassoma.digitacaodh = digitacaoDH;
                        caixassoma.digitacaoca = digitacaoCA;
                        caixassoma.digitacaoch = digitacaoCH;

                        entidade.SaveChanges();
                    }
                    catch(Exception erro)
                    {
                        MessageBox.Show("Finalização de caixa digitação.:"+erro.ToString());
                    }

                    return true;
                }
                catch (Exception e)
                {
                    throw new Exception("Não foi possível encerrar o Caixa " + e.ToString());
                }
            }
        }

        private void FrmResumoCaixa_Load(object sender, EventArgs e)
        {
            if (Conexao.ConexaoOnline() == true)
            {
                string sql = "CALL AtualizarQdtRegistros()";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
            }
            if (ConfiguracoesECF.caixaPendente == true)
                btnSair.Enabled = false;

        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            if(ConfiguracoesECF.caixaPendente == false)
                this.Close();
        }

        private void Encerrar(object sender, EventArgs e)
        {
            string sql = "";

            if (GlbVariaveis.glb_Acbr == false)
            {
                if (FuncoesECF.CupomFiscalAberto())
                {
                    MessageBox.Show("ECF com cupom fiscal em aberto. Exclua o último cupom !");
                    return;
                }
            }
            else
            {
                if (FuncoesECF.estadoECF() != ACBrFramework.ECF.EstadoECF.Livre && FuncoesECF.estadoECF() != ACBrFramework.ECF.EstadoECF.RequerZ)
                {
                    MessageBox.Show("Não é possivel emitir redução Z estado da ecf.: "+FuncoesECF.estadoECF().ToString());
                    return;
                }
            }

            if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
            {
                MessageBox.Show("SICEpdv Off-line Não é possivel fechar o caixa","Anteção",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            if (Conexao.tipoConexao == 2 && StandAlone.quantidadeDocumentoSincronizar() > 0)
            {
                MessageBox.Show("Não é possivel Finalizar o caixa com documentos para sincronizar!", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Sincronizar objSincronizador = new Sincronizar();
                objSincronizador.ShowDialog();
                return;
            }

            if (MessageBox.Show("Confirma o encerramento do caixa ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
            {            
                return;
            }
            
            ResumoCaixa encerrarcaixa = new ResumoCaixa();
            if (Permissoes.fechamentocaixadigitacao)
            {
                encerrarcaixa.digitacaoDH = decimal.Parse(txtDinheiro.Text == "" ? "0" : txtDinheiro.Text);
                encerrarcaixa.digitacaoCH = decimal.Parse(txtCheque.Text == "" ? "0" : txtCheque.Text);
                encerrarcaixa.digitacaoCA = decimal.Parse(txtCartao.Text == "" ? "0" : txtCartao.Text);
            }
            else
            {
                encerrarcaixa.digitacaoDH = 0;
                encerrarcaixa.digitacaoCH = 0;
                encerrarcaixa.digitacaoCA = 0;
            }

            FrmMsgOperador msgEncerrar = new FrmMsgOperador("", "Encerrando caixa");
            try
            {
                operadorCaixa = GlbVariaveis.glb_Usuario;

                if (Conexao.onLine)
                {
                    FrmLogon frmlogon = new FrmLogon();
                    frmlogon.txtDescricao.Text = "Fechamento de Caixa: " + GlbVariaveis.glb_Usuario;
                    // frmlogon.campo = "rotcaixa";
                    frmlogon.ShowDialog();

                    if (!Operador.autorizado) return;
                };
                msgEncerrar.Show();

                if (Permissoes.fechamentocaixadigitacao)
                {
                    //via caixa
                    FuncoesECF.RelatorioGerencial("abrir", "");
                    FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "VIA DO OPERADOR " + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "OPERADOR: " + operadorCaixa + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                    FuncoesECF.RelatorioGerencial("imprimir", "Dinheiro (R$): " + string.Format("{0:N2}", encerrarcaixa.digitacaoDH) + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "Cheque   (R$): " + string.Format("{0:N2}", encerrarcaixa.digitacaoCH) + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "Cartão   (R$): " + string.Format("{0:N2}", encerrarcaixa.digitacaoCA) + Environment.NewLine);

                    FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("fechar", "");

                    System.Threading.Thread.Sleep(2000);

                    FuncoesECF.RelatorioGerencial("abrir", "");
                    FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "VIA DO CAIXA ADMINISTRATIVO " + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "OPERADOR: " + operadorCaixa + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                    FuncoesECF.RelatorioGerencial("imprimir", "Dinheiro (R$): " + string.Format("{0:N2}", encerrarcaixa.digitacaoDH) + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "Cheque   (R$): " + string.Format("{0:N2}", encerrarcaixa.digitacaoCH) + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", "Cartão   (R$): " + string.Format("{0:N2}", encerrarcaixa.digitacaoCA) + Environment.NewLine);

                    FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("fechar", "");
                }

                if (ConfiguracoesECF.NFC == true)
                {
                    FrmMsgOperador msgEncerrarNFce = new FrmMsgOperador("", "Encerrando NFCe");

                    try
                    {
                        siceEntities entidade = Conexao.CriarEntidade();
                     
                        var dataMovimento = (from c in entidade.caixa where c.CodigoFilial == GlbVariaveis.glb_filial && c.operador == operadorCaixa && c.data != null select c.data).Max();

                        FuncoesECF ecf = new FuncoesECF();
                        ecf.LeituraXNFC(dataMovimento.Value, dataMovimento.Value);

                        if (dataMovimento != null)
                        {
                            try
                            {
                                string SQL = "CALL FechamentoNFCe('" + dataMovimento.Value.ToString("yyyy-MM-dd") + "','" + dataMovimento.Value.ToString("yyyy-MM-dd") + "','" + int.Parse(ConfiguracoesECF.NFCserie).ToString() + "','" + GlbVariaveis.glb_filial + "','65');";
                                entidade.ExecuteStoreCommand(SQL);
                            }
                            catch
                            {
                                try
                                {
                                    string SQL = "CALL FechamentoNFCe('" + dataMovimento.Value.ToString("yyyy-MM-dd") + "','" + dataMovimento.Value.ToString("yyyy-MM-dd") + "','" + int.Parse(ConfiguracoesECF.NFCserie).ToString() + "','" + GlbVariaveis.glb_filial + "','65','fechamento');";
                                    entidade.ExecuteStoreCommand(SQL);
                                }
                                catch (Exception erro)
                                {
                                    MessageBox.Show("1 - Não foi possivel fazer o Fechamento NFCe! Verifique a tabela NFe012tmp");
                                }
                            }
                        }
                    }
                    catch(Exception erro)
                    {
                        MessageBox.Show("2 - Não foi possivel fazer o Fechamento NFCe! Verifique a tabela NFe012tmp");
                    }

                    Application.DoEvents();
                    msgEncerrarNFce.Dispose();
                }



                LogSICEpdv.Registrarlog("encerrarcaixa.EncerrarCaixa(GlbVariaveis.glb_Usuario)", "true", "Paf.cs");
                Application.DoEvents();
                encerrarcaixa.EncerrarCaixa(operadorCaixa);

            }
            catch (Exception erro)
            {

                MessageBox.Show(erro.Message);
                return;
            }
            finally
            {
                msgEncerrar.Dispose();
            }

            if (!Conexao.onLine && ConfiguracoesECF.NFC == false)
            {
                MessageBox.Show("Não é possível emitir redução Z pois o sistema está no modo Stand Alone", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
                return;
            }

            /*if (Conexao.onLine && StandAlone.QuantidadeRegistro() > 0)
            {
                MessageBox.Show("Não é possivel emitir redução Z pois existem documentos para sincronizar ", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
                return;
            }*/


            if (ConfiguracoesECF.idECF==9999)
            {                
                Application.Exit();
                return;
            };

            #region
            if (ConfiguracoesECF.NFC == false && MessageBox.Show("Caixa Encerrado. Emitir redução Z. Não será possível imprimir no ECF na data atual. Confirma redução Z ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                Application.Exit();
                return;
            }

            FrmMsgOperador msg = new FrmMsgOperador("", "Gravando informações fiscais. Não desligue o ECF! \r\n Pode demorar de 2 a 5 minutos. ");
            msg.Show();
            bool movimentoAnterior = false;

            LogSICEpdv.Registrarlog("Gravando informações fiscais. Não desligue o ECF! \r\n Pode demorar de 2 a 5 minutos. ", "", "FrmResumoCaixa.cs");

            Application.DoEvents();
            FuncoesECF.ReducaoZ();
            System.Threading.Thread.Sleep(500);            
            try
            {
                if (ConfiguracoesECF.zPendente)
                {
                    movimentoAnterior = true;
                }

                Paf paf = new Paf();
                try
                {
                    if (Conexao.ConexaoOnline() == true)
                    {
                        LogSICEpdv.Registrarlog("CALL AtualizarQdtRegistros()", "true", "Paf.cs");
                        sql = "CALL AtualizarQdtRegistros()";
                        Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                    }
                }
                catch
                {

                }
                LogSICEpdv.Registrarlog("CALL AtualizarQdtRegistros()", "true", "Paf.cs");
                sql = "CALL AtualizarQdtRegistros()";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                paf.GravarRelatorioR(movimentoAnterior);
                Application.DoEvents();
                MessageBox.Show("Informações fiscais gravadas. O Sistema será encerrado. Arquivo Destino: C:\\iqsistemas\\sicepdv\\"+paf.arquivoDestino,"SICEpdv",MessageBoxButtons.OK,MessageBoxIcon.Information);

                /*if (ConfiguracoesECF.NFC == false)
                {
                    FrmBackup backup = new FrmBackup();
                    backup.ShowDialog();
                }*/
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.ToString(), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                msg.Dispose();
                Application.Exit();
            }
            #endregion
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            
                FrmMsgOperador msg = new FrmMsgOperador("", "Imprimindo fluxo de caixa");
                msg.Show();
                Application.DoEvents();
                FuncoesECF.RelatorioGerencial("abrir", "");
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "OPERADOR: " + GlbVariaveis.glb_Usuario + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);


                FuncoesECF.RelatorioGerencial("imprimir", "Saldo Inicial (+) R$: " + string.Format("{0:N2}", lblSaldoInicial.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "SU.Entrada Caixa  R$: " + string.Format("{0:N2}", lblSUTR.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "     Dinheiro (+) R$: " + string.Format("{0:N2}", lblDH.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "       Cheque (+) R$: " + string.Format("{0:N2}", lblCH.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "       Cartão (+) R$: " + string.Format("{0:N2}", lblCA.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "   Fidelidade (+) R$: " + string.Format("{0:N2}", lblPF.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "       Ticket (+) R$: " + string.Format("{0:N2}", lblTI.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "           AV (+) R$: " + string.Format("{0:N2}", lblAV.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "    Crediário (+) R$: " + string.Format("{0:N2}", lblCR.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "  Recebimento (+) R$: " + string.Format("{0:N2}", lblREC.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "  Rec Dinheiro    R$: " + string.Format("{0:N2}", lblRecDH.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "  Rec Cheque      R$: " + string.Format("{0:N2}", lblRecCH.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "  Rec Cartão      R$: " + string.Format("{0:N2}", lblRecCartao.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "    Devolução     R$: " + string.Format("{0:N2}", lblDevolucao.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "    Cancelamento  R$: " + string.Format("{0:N2}", lblCancelamento.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "      Sangria (-) R$: " + string.Format("{0:N2}", lblSangria.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "  TOTAL SALDO (=) R$: " + string.Format("{0:N2}", lblSaldoFinal.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", " SALDO LÍQUIDO(=) R$: " + string.Format("{0:N2}", lblSaldoLiq.Text) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
                FuncoesECF.RelatorioGerencial("fechar", "");
                msg.Dispose();
           
        }

        private void FrmResumoCaixa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }

        private void btnImprimir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();

        }

        private void FrmResumoCaixa_Shown(object sender, EventArgs e)
        {
            btnImprimir.Focus();
        }

        private void calculaResumoCaixa()
        {
            decimal dinheiro = decimal.Parse(txtDinheiro.Text == "" ? "0" : txtDinheiro.Text), cartao = decimal.Parse(txtCartao.Text == "" ? "0" : txtCartao.Text), cheque = decimal.Parse(txtCheque.Text == "" ? "0" : txtCheque.Text);

            txtSaldo.Text = (dinheiro + cartao + cheque).ToString();
            txtSaldoLiq.Text = dinheiro.ToString();

        }

        private void txtDinheiro_Leave(object sender, EventArgs e)
        {
            calculaResumoCaixa();
        }

        private void txtCartao_Leave(object sender, EventArgs e)
        {
            calculaResumoCaixa();
        }

        private void txtCheque_Leave(object sender, EventArgs e)
        {
            calculaResumoCaixa();
        }

        private void FrmResumoCaixa_FormClosed(object sender, FormClosedEventArgs e)
        {
             
        }

        private void FrmResumoCaixa_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }

    class fluxoCaixa
        {
            public string operador { get; set; }
            public string codigoFilial { get; set; }
            public DateTime dataMovimento { get; set; }
            public DateTime horasAbertura { get; set; }
            public decimal saldo { get; set; }
            public decimal dinheiro { get; set; }
            public decimal entradaDH { get; set; }
            public decimal entradaCH { get; set; }
            public decimal entradaCA { get; set; }
            public decimal entradaFI { get; set; }
            public decimal cheque { get; set; }
            public decimal chequePre { get; set; }
            public decimal cartao { get; set; }
            public decimal recebimento { get; set; }
            public decimal recebimentoDH { get; set; }
            public decimal recebimentoCH { get; set; }
            public decimal recebimentoCA { get; set; }
            public decimal crediario { get; set; }
            public decimal sangria { get; set; }
            public decimal totalVenda { get; set; }
            public decimal totalCustos { get; set; }
            public decimal juros { get; set; }
            public decimal renegociacao { get; set; }
            public decimal perdao { get; set; }
            public decimal descontoTotalVenda { get; set; }
            public decimal descontoRecebimento { get; set; }
            public decimal descontoRecebimentoJuros { get; set; }
            public decimal crediariocr { get; set; }
            public decimal jurosPerdao { get; set; }
            public decimal jurosRenegociacao { get; set; }
            public decimal encargos { get; set; }
            public decimal devolucao { get; set; }
            public decimal devolucaoRec { get; set; }
            public decimal devolucaoVenda { get; set; }
            public decimal jurosRecebimentoCH { get; set; }
            public decimal encargosRecebidos { get; set; }
            public string ocorrecia { get; set; }
            public decimal chequeFI { get; set; }
            public decimal chequeFIPre { get; set; }
            public decimal financiamento { get; set; }
            public decimal financeira { get; set; }
            public int qtdCupons { get; set; }
            public decimal suprimento { get; set; }
            public string dpfinanceiro { get; set; }
            public decimal receitas { get; set; }
            public decimal recebimentoBL { get; set; }
            public decimal recebimentoDC { get; set; }
            public decimal emprestimoDH { get; set; }
            public decimal emprestimoCH { get; set; }
            public decimal compraTI { get; set; }
            public decimal trocaCH { get; set; }
            public decimal ticket { get; set; }
            public decimal fidelidade { get; set; }
            public decimal cartaoPresente { get; set; }
            public decimal valorServicos { get; set; }
            public decimal descontoServicos { get; set; }
            public decimal descontoCapitalRN { get; set; }
            public decimal crediarioServicos { get; set; }
            public decimal jurosRecebimentoCA { get; set; }
            public decimal crediarioRecebimentoAV { get; set; }
            public decimal recebimentoAV { get; set; }
            public decimal vendaAV { get; set; }
            public decimal saldoFinal { get; set; }
            public decimal saldoFinalLiquidoEspecie { get; set; }
            public decimal cancelamento { get; set; }
    }
}


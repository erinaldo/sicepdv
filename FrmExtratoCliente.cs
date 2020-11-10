using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.EntityClient;
using System.Windows.Forms.VisualStyles;

namespace SICEpdv
{
    public partial class FrmExtratoCliente : Form
    {
        public static int codigoCliente = 0;
        public static string nomeCliente = "";
        private decimal valorPagamento = 0;
        private decimal taxaJurosDiario = Configuracoes.taxaJurosDiario;
        public static List<PagamentoParcelas> parcelas = new List<PagamentoParcelas>();
        private bool porContaParcela = true;
        private decimal debitoTotal = 0;
        public static decimal jurosAntecipados = 0;
        public bool receber = false;

        public FrmExtratoCliente()
        {
            LimparRecebimento(1);
            InitializeComponent();
            txtPorConta.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtPorConta.Leave += (objeto, evento) =>
            {
                txtPorConta.Text = Funcoes.FormatarDecimal(txtPorConta.Text);
            };
            txtJurosAnt.KeyPress += (objeto, evento) =>
                 Funcoes.DigitarNumerosPositivos(objeto, evento);

            txtJurosAnt.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyCode == Keys.Enter)
                {
                    calcularJurosAnt();
                }
            };
            txtJurosAnt.Leave += (objeto, evento) =>
            {
                txtJurosAnt.Text = Funcoes.FormatarDecimal(txtJurosAnt.Text);
            };
        }

        private void calcularJurosAnt()
        {
            if (string.IsNullOrEmpty(txtJurosAnt.Text))
                txtJurosAnt.Text = "0";


            if (jurosAntecipados < 0)
                jurosAntecipados = 0;
            if (Convert.ToDecimal(txtJurosAnt.Text) > jurosAntecipados)
            {
                txtJurosAnt.Text = string.Format("{0:N2}", jurosAntecipados);
                txtJurosAnt.Focus();
                //evento.SuppressKeyPress = true;
                return;
            }
            jurosAntecipados = Convert.ToDecimal(txtJurosAnt.Text);
            txtJurosAnt.Text = Funcoes.FormatarDecimal(txtJurosAnt.Text);
            receber = true;
            sairCrediario();
        }

        private void FrmExtratoCliente_Load(object sender, EventArgs e)
        {

        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            //LimparRecebimento();
            receber = false;
            sairCrediario();
            Close();
        }

        private void LimparRecebimento(int tipo = 0)
        {

            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            if (tipo == 0)
            {
                foreach (var item in parcelas)
                {

                    var dadosParcela = (from n in entidade.crmovclientes
                                        where n.sequenciainc == item.idParcela
                                        && n.ip == GlbVariaveis.glb_IP
                                        select n).First();
                    dadosParcela.valorpago = dadosParcela.valorcorrigido;
                    dadosParcela.quitado = "N";
                    entidade.SaveChanges();
                }

                parcelas.Clear();
            }
            else
            {
                string sql = "UPDATE crmovclientes SET quitado='N' WHERE codigo='" + codigoCliente.ToString() + "'" +
                    "AND valoratual>0 AND quitado='S' and ip = '"+GlbVariaveis.glb_IP+"'";
                entidade.ExecuteStoreCommand(sql);
            }

        }

        private void dtgDB_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SelecionarParcela(e);
        }

        private void SelecionarParcela(DataGridViewCellEventArgs e)
        {
            if (!Permissoes.receberContas)
            {
                FrmLogon Logon = new FrmLogon();
                Operador.autorizado = false;
                Logon.campo = "clireceber";
                Logon.lblDescricao.Text = "Sem permissão para receber";
                Logon.txtDescricao.Text = "Liberar recebimento de contas " +
                Logon.ShowDialog();
                if (!Operador.autorizado) return;
            }

            if (e.ColumnIndex == 11)
            {
                int parcela = Convert.ToInt32(dtgDB.CurrentRow.Cells["sequenciaincDataGridViewTextBoxColumn"].Value);

                siceEntities entidade = Conexao.CriarEntidade();

                var dadosParcela = (from n in entidade.crmovclientes
                                    where n.sequenciainc == parcela
                                    select n).First();

                if (dadosParcela.quitado == "S" && dadosParcela.ip != GlbVariaveis.glb_IP)
                {
                    MessageBox.Show("Parcela já selecionada em outro terminal");
                    return;
                }

                dadosParcela.valorpago = dadosParcela.valorcorrigido;
                dadosParcela.ip = GlbVariaveis.glb_IP;
                dadosParcela.quitado = "S";
                entidade.SaveChanges();


                dtgDB.CurrentRow.DefaultCellStyle.BackColor = Color.Yellow;
                dtgDB.CurrentRow.Cells["Quitar"].Style.BackColor = Color.Green;


                var procura = parcelas.Where(c => c.idParcela == parcela);
                if (procura.Count() != 0)
                {
                    valorPagamento -= procura.First().valorPagamento; // Convert.ToDecimal(dtgDB.CurrentRow.Cells["valorcorrigidoDataGridViewTextBoxColumn"].Value);
                    parcelas.Remove(procura.ToArray().Last());
                    lblPagamento.Text = string.Format("{0:C2}", valorPagamento);


                    dtgDB.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                    dtgDB.CurrentRow.Cells["Quitar"].Style.BackColor = Color.Green;



                    dadosParcela.valorpago = dadosParcela.valorcorrigido;
                    dadosParcela.ip = GlbVariaveis.glb_IP;
                    dadosParcela.quitado = "N";
                    entidade.SaveChanges();



                    var dados = (from n in Conexao.CriarEntidade().crmovclientes
                                 where n.codigo == codigoCliente
                                 && n.sequenciainc == parcela
                                 && n.vencimento >= GlbVariaveis.Sys_Data.Date
                                 select new { n.Diasdecorrido, n.valoratual });
                    foreach (var item in dados)
                    {
                        jurosAntecipados -= Configuracoes.taxaJurosAnt * Math.Abs(item.Diasdecorrido) * item.valoratual / 100;
                    }
                    if (jurosAntecipados <= 0)
                    {
                        jurosAntecipados = 0;
                        grpJuros.Visible = false;
                    }
                    txtJurosAnt.Text = string.Format("{0:N2}", jurosAntecipados);
                    return;
                }

                var dadosJurosAnt = (from n in Conexao.CriarEntidade().crmovclientes
                                     where n.codigo == codigoCliente
                                     && n.sequenciainc == parcela
                                     && n.vencimento >= GlbVariaveis.Sys_Data.Date
                                     select new { n.Diasdecorrido, n.valoratual });

                foreach (var item in dadosJurosAnt)
                {
                    jurosAntecipados += Configuracoes.taxaJurosAnt * Math.Abs(item.Diasdecorrido) * item.valoratual / 100;
                }
                if (jurosAntecipados > 0)
                {
                    txtJurosAnt.Text = string.Format("{0:N2}", jurosAntecipados);
                    grpJuros.Visible = true;
                }

                parcelas.Add(new PagamentoParcelas
                {
                    codigoCliente = codigoCliente,
                    idParcela = parcela,
                    valorPagamento = Convert.ToDecimal(dtgDB.CurrentRow.Cells["valorcorrigidoDataGridViewTextBoxColumn"].Value),
                    valorJuros = Convert.ToDecimal(dtgDB.CurrentRow.Cells["vrjurosDataGridViewTextBoxColumn"].Value) + Convert.ToDecimal(dtgDB.CurrentRow.Cells["jurosacumuladoDataGridViewTextBoxColumn"].Value)
                });




                valorPagamento += Convert.ToDecimal(dtgDB.CurrentRow.Cells["valorcorrigidoDataGridViewTextBoxColumn"].Value);
                lblPagamento.Text = string.Format("{0:C2}", valorPagamento);


            }

            if (e.ColumnIndex == 12)
            {
               
                int parcela = Convert.ToInt32(dtgDB.CurrentRow.Cells["sequenciaincDataGridViewTextBoxColumn"].Value);

                siceEntities entidade = Conexao.CriarEntidade();

                var dadosParcela = (from n in entidade.crmovclientes
                                    where n.sequenciainc == parcela
                                    select n).First();

                if (dadosParcela.quitado == "S" && dadosParcela.ip != GlbVariaveis.glb_IP)
                {
                    MessageBox.Show("Parcela já selecionada em outro terminal");
                    return;
                }


                var procura = parcelas.Where(c => c.idParcela == parcela);
                if (procura.Count() != 0)
                {
                    valorPagamento -= procura.First().valorPagamento; // Convert.ToDecimal(dtgDB.CurrentRow.Cells["valorcorrigidoDataGridViewTextBoxColumn"].Value);
                    parcelas.Remove(procura.ToArray().Last());
                    lblPagamento.Text = string.Format("{0:C2}", valorPagamento);
                    dtgDB.CurrentRow.Cells["porConta"].Style.BackColor = Color.White;
                    dtgDB.CurrentRow.Cells["Quitar"].Style.BackColor = Color.White;
                    return;
                }
                grpPorConta.Location = new Point(197, 142);
                grpPorConta.Visible = true;
                dtgDB.Enabled = false;
                txtPorConta.Text = string.Format("{0:N2}", dtgDB.CurrentRow.Cells["valorcorrigidoDataGridViewTextBoxColumn"].Value);
                txtPorConta.Focus();
                txtPorConta.SelectAll();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string CPF = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT IF(IFNULL(cpf,0) = '',0,IFNULL(cpf,0)) FROM senhas WHERE operador = '"+GlbVariaveis.glb_Usuario+"'").FirstOrDefault();
            string CPFCliente = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT IF(IFNULL(cpf,0) = '',0,IFNULL(cpf,0)) FROM clientes WHERE codigo = '" + codigoCliente + "'").FirstOrDefault();
            if (CPF != "0" && CPF != "" && CPF == CPFCliente && CPF == CPFCliente)
            {
                MessageBox.Show(GlbVariaveis.glb_Usuario+ " Não é possível fazer um recebimento seu, peça a um outro operador para fazer esta operação", "Atenção", MessageBoxButtons.OK);
                return;
            }

            receber = true;
            calcularJurosAnt();
            sairCrediario();

        }

        private void FrmExtratoCliente_Shown(object sender, EventArgs e)
        {
            if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
                MessageBox.Show("PDV em modo OFF-line!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                MostrarExtrato(false);

            if (ConfiguracoesECF.reducaoZEmitida == true || ConfiguracoesECF.zPendente == true || ConfiguracoesECF.caixaPendente == true)
                btnReceber.Enabled = false;

        }

        private void MostrarExtrato(bool imprimir)
        {

            FrmMsgOperador msgExtrato = new FrmMsgOperador("", "Processando débitos");
            msgExtrato.Show();
            try
            {
                Application.DoEvents();
                Clientes.AtualizarDebito(codigoCliente, taxaJurosDiario);
                parcelas.Clear();

                var dadosClientes = (from n in Conexao.CriarEntidade().clientes
                                     where n.Codigo == codigoCliente
                                     select new
                                     {
                                         n.Codigo,
                                         n.Nome,
                                         n.endereco,
                                         n.numero,
                                         n.cidade,
                                         n.estado,
                                         n.bairro,
                                         n.cep,
                                         n.telefone,
                                         n.email
                                     }).FirstOrDefault();

                lblCliente.Text = dadosClientes.Codigo + " " + dadosClientes.Nome;
                lblEndereco.Text = dadosClientes.endereco + " " + dadosClientes.numero +
                    " " + dadosClientes.cidade + " " + dadosClientes.bairro + " " +
                    dadosClientes.estado + "\n\r" + "Tel: " + dadosClientes.telefone +
                    " Email" + dadosClientes.email;

                var dados = Clientes.Extrato(codigoCliente);
                if (dados.Count() == 0)
                {
                    MessageBox.Show("Cliente não possui débitos");
                    return;
                }

                var dadosSomas = dados.ToList();


                var valorDebito = dadosSomas.Sum(n => (decimal?)n.valoratual) - dadosSomas.Sum(n => n.jurosacumulado);
                decimal? valorAtraso = dadosSomas.Where(n => n.vencimento < GlbVariaveis.Sys_Data).Sum(c => (decimal?)c.valoratual) - dadosSomas.Where(n => n.vencimento < GlbVariaveis.Sys_Data).Sum(c => (decimal?)c.jurosacumulado);
                decimal? valorJuros = dadosSomas.Sum(n => n.vrjuros) + dadosSomas.Sum(n => n.jurosacumulado);
                lblDebito.Text = string.Format("{0:N2}", valorDebito);
                lblTotalAtraso.Text = string.Format("{0:N2}", valorAtraso);
                lblTotalJuros.Text = string.Format("{0:N2}", valorJuros);
                lblTotalAtrasoJuros.Text = string.Format("{0:N2}", valorAtraso + valorJuros);
                lblTotalDebito.Text = string.Format("{0:N2}", valorDebito + valorJuros);
                debitoTotal = valorDebito.GetValueOrDefault() + valorJuros.GetValueOrDefault();

                if (imprimir)
                {
                    #region
                    msgExtrato.Dispose();
                    FrmMsgOperador msg = new FrmMsgOperador("", "Imprimindo extrado. ");
                    msg.Show();
                    Application.DoEvents();
                    try
                    {
                        FuncoesECF.RelatorioGerencial("abrir", "");
                        FuncoesECF.RelatorioGerencial("imprimir", "EXTRATO DE CLIENTE" + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("imprimir", "Cliente: " + dadosClientes.Codigo + " " + dadosClientes.Nome + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("imprimir", "DOC   D.COMPRA   VENCIMENTO DIAS JUROS TOTAL" + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("imprimir", "====================================" + Environment.NewLine);
                        foreach (var item in dados)
                        {
                            FuncoesECF.RelatorioGerencial("imprimir", item.documento.ToString() + "|" + string.Format("{0:dd/MM/yyyy}", item.datacompra) + "|" + string.Format("{0:dd/MM/yyyy}", item.vencimento) + "|" + item.Diasdecorrido.ToString() + " | " + item.vrjuros.ToString() + "|" + item.valoratual.ToString() + Environment.NewLine);

                        }
                        FuncoesECF.RelatorioGerencial("imprimir", "====================================" + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("imprimir", "Total Débito R$: " + string.Format("{0:N2}", valorDebito) + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("imprimir", "Total atraso R$: " + string.Format("{0:N2}", valorAtraso) + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("imprimir", "Total Juros  R$: " + string.Format("{0:N2}", valorJuros) + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("imprimir", "Atraso+Juros R$: " + string.Format("{0:N2}", valorAtraso + valorJuros) + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("imprimir", "Débito Total R$: " + string.Format("{0:N2}", valorDebito + valorJuros) + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("fechar", "");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Não foi possível imprimir extrato " + ex.Message);
                    }
                    finally
                    {
                        msg.Dispose();
                    }
                    return;
                    #endregion
                }

                dtgDB.DataSource = from n in dados
                                   where n.quitado == "N"
                                   orderby n.vencimento
                                   select new
                                   {
                                       n.CodigoFilial,
                                       n.quitado,
                                       n.documento,
                                       n.nrParcela,
                                       n.datacompra,
                                       n.Valor,
                                       n.valoratual,
                                       n.Diasdecorrido,
                                       n.vencimento,
                                       n.vrjuros,
                                       n.valorcorrigido,
                                       n.jurosacumulado,
                                       n.datapagamento,
                                       n.vrultpagamento,
                                       n.porconta,
                                       n.Observacao,
                                       n.bloquete,
                                       n.usuario,
                                       n.dependente,
                                       n.tipopagamento,
                                       n.datarenegociacao,
                                       n.dpfinanceiro,
                                       n.cobrador,
                                       n.sequenciainc
                                   };
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }
            finally
            {
                msgExtrato.Dispose();
            };
        }

        private void btnDispensarJuros_Click(object sender, EventArgs e)
        {
            FrmLogon logon = new FrmLogon();
            logon.campo = "Clidispensa";
            logon.txtDescricao.Text = "Dispensar Juros!";
            logon.ShowDialog();

            if (!Operador.autorizado)
            {
                MessageBox.Show("Usuario Inválido ou não autorizado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (taxaJurosDiario == 0)
                {
                    btnDispensarJuros.Text = "Dispensar Juros";
                    btnDispensarJuros.BackColor = SystemColors.ActiveCaption;
                    taxaJurosDiario = Configuracoes.taxaJurosDiario;
                }
                else
                {
                    btnDispensarJuros.Text = "Calcular Juros";
                    btnDispensarJuros.BackColor = SystemColors.ControlDark;
                    taxaJurosDiario = 0;
                }

                parcelas.Clear();
                valorPagamento = 0;
                lblPagamento.Text = "R$ 0.00";
                MostrarExtrato(false);
            }

        }

        private void btnPorConta_Click(object sender, EventArgs e)
        {
            txtPorConta.Text = Funcoes.FormatarDecimal(txtPorConta.Text);

            if (porContaParcela)
            {


                int parcela = Convert.ToInt32(dtgDB.CurrentRow.Cells["sequenciaincDataGridViewTextBoxColumn"].Value);

                if (Convert.ToDecimal(txtPorConta.Text) > Convert.ToDecimal(dtgDB.CurrentRow.Cells["valorcorrigidoDataGridViewTextBoxColumn"].Value))
                {
                    MessageBox.Show("Por conta não pode ser maior que o valor da parcela.");
                    txtPorConta.Focus();
                    return;
                }

                parcelas.Add(new PagamentoParcelas
                {
                    codigoCliente = codigoCliente,
                    idParcela = parcela,
                    valorPagamento = Convert.ToDecimal(txtPorConta.Text),
                    valorJuros = Convert.ToDecimal(dtgDB.CurrentRow.Cells["vrjurosDataGridViewTextBoxColumn"].Value) + Convert.ToDecimal(dtgDB.CurrentRow.Cells["jurosacumuladoDataGridViewTextBoxColumn"].Value)
                });

                dtgDB.CurrentRow.Cells["porConta"].Style.BackColor = Color.Green;
                dtgDB.CurrentRow.DefaultCellStyle.BackColor = Color.Yellow;


                valorPagamento += Convert.ToDecimal(txtPorConta.Text);
                lblPagamento.Text = string.Format("{0:C2}", valorPagamento);
                grpPorConta.Visible = false;
                dtgDB.Enabled = true;
            }
            else
            {

                if (Convert.ToDecimal(txtPorConta.Text) > debitoTotal)
                {
                    MessageBox.Show("Por conta não pode ser maior que o valor do débito total");
                    txtPorConta.Text = string.Format("{0:N2}", debitoTotal);
                    txtPorConta.Focus();
                    return;
                }
                parcelas.Clear();
                decimal restante = Convert.ToDecimal(txtPorConta.Text);
                decimal valorPago = 0;
                for (int i = 0; i < dtgDB.RowCount; i++)
                {
                    int parcela = Convert.ToInt32(dtgDB.Rows[i].Cells["sequenciaincDataGridViewTextBoxColumn"].Value);

                    if (restante > Convert.ToDecimal(dtgDB.Rows[i].Cells["valorcorrigidoDataGridViewTextBoxColumn"].Value))
                    {
                        valorPago = Convert.ToDecimal(dtgDB.Rows[i].Cells["valorcorrigidoDataGridViewTextBoxColumn"].Value);
                        restante -= valorPago;
                    }
                    else
                    {
                        valorPago = restante;
                        restante = 0;
                    }


                    parcelas.Add(new PagamentoParcelas
                    {
                        codigoCliente = codigoCliente,
                        idParcela = parcela,
                        valorPagamento = valorPago,
                        valorJuros = Convert.ToDecimal(dtgDB.Rows[i].Cells["vrjurosDataGridViewTextBoxColumn"].Value) + Convert.ToDecimal(dtgDB.Rows[i].Cells["jurosacumuladoDataGridViewTextBoxColumn"].Value)
                    });

                    dtgDB.Rows[i].Cells["porConta"].Style.BackColor = Color.Green;
                    dtgDB.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;


                    if (restante <= 0)
                        break;
                }

                valorPagamento = Convert.ToDecimal(txtPorConta.Text);
                lblPagamento.Text = string.Format("{0:C2}", valorPagamento);
                dtgDB.Enabled = true;
            }
            grpPorConta.Visible = false;

            foreach (var item in parcelas)
            {
                siceEntities entidade = Conexao.CriarEntidade();

                var dadosParcela = (from n in entidade.crmovclientes
                                    where n.sequenciainc == item.idParcela
                                    select n).First();

                dadosParcela.ip = GlbVariaveis.glb_IP;
                dadosParcela.quitado = "N";
                entidade.SaveChanges();
            }
        }

        private void FrmExtratoCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }

            if (char.IsNumber(e.KeyChar) && grpPorConta.Visible == false && txtJurosAnt.Focused == false)
            {
                ChamarPorConta();
            }

        }

        private void btnEstornar_Click(object sender, EventArgs e)
        {
            int? documento = (from n in Conexao.CriarEntidade().contdocs
                              where n.data == GlbVariaveis.Sys_Data
                              && n.codigocliente == codigoCliente
                              && n.dpfinanceiro == "Recebimento"
                              && n.estornado == "N"
                              select (Int32?)n.documento).Max();

            if (documento.GetValueOrDefault() == 0)
            {
                MessageBox.Show("Não existe pagamento na data atual para ser estornado!");
                return;
            }

            var DadosDoc = (from n in Conexao.CriarEntidade().contdocs
                              where n.data == GlbVariaveis.Sys_Data
                              && n.codigocliente == codigoCliente
                              && n.dpfinanceiro == "Recebimento"
                              && n.estornado == "N"
                              && n.documento == documento
                              select n).FirstOrDefault();

            string CPF = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT IF(IFNULL(cpf,0) = '',0,IFNULL(cpf,0)) FROM senhas WHERE operador = '" + GlbVariaveis.glb_Usuario + "'").FirstOrDefault();
            string CPFCliente = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT IF(IFNULL(cpf,0) = '',0,IFNULL(cpf,0)) FROM clientes WHERE codigo = '" + DadosDoc.codigocliente + "'").FirstOrDefault();
            if (CPF != "0" && CPF != "" && CPF == CPFCliente)
            {
                MessageBox.Show(GlbVariaveis.glb_Usuario + " Não é possível fazer um recebimento seu, peça a um outro operador para fazer esta operação", "Atenção", MessageBoxButtons.OK);
                return;
            }


            FrmLogon Logon = new FrmLogon();
            Operador.autorizado = false;
            Logon.campo = "cliestornar";
            Logon.lblDescricao.Text = "Permissão para estornar";
            Logon.txtDescricao.Text = "Estornar recebimento de cliente" +
            Logon.ShowDialog();
            if (!Operador.autorizado) return;


            if (MessageBox.Show("Confirma Estorno?", "SICEpdv", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
            {
                conn.Open();
                EntityCommand cmd = conn.CreateCommand();
                cmd.CommandText = "siceEntities.EstornarQuitacao";
                cmd.CommandType = CommandType.StoredProcedure;

                EntityParameter doc = cmd.Parameters.Add("docEstorno", DbType.Int32);
                doc.Direction = ParameterDirection.Input;
                doc.Value = documento;

                EntityParameter codCliente = cmd.Parameters.Add("codCliente", DbType.Int32);
                codCliente.Direction = ParameterDirection.Input;
                codCliente.Value = codigoCliente;


                EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                ip.Direction = ParameterDirection.Input;
                ip.Value = GlbVariaveis.glb_IP;

                EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                filial.Direction = ParameterDirection.Input;
                filial.Value = GlbVariaveis.glb_filial;

                EntityParameter operadorEstorno = cmd.Parameters.Add("operadorEstorno", DbType.String);
                operadorEstorno.Direction = ParameterDirection.Input;
                operadorEstorno.Value = Operador.ultimoOperadorAutorizado;

                cmd.ExecuteNonQuery();
            }
            MostrarExtrato(false);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Faça essa transação no SICE.net!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            /*
            FrmDevolucao dev = new FrmDevolucao();
            dev.ShowDialog();
            btnDevolucao.BackColor = System.Drawing.SystemColors.Control;
            if (FrmDevolucao.numeroDevolucao > 0)
            {
                btnDevolucao.BackColor = System.Drawing.Color.GreenYellow;
                grpPorConta.Visible = true;
                txtPorConta.Text = FrmDevolucao.totalDevolucao.ToString();
                txtPorConta.Enabled = false;
                btnPorConta.Focus();
            }
            */
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MostrarExtrato(true);
        }

        private void dtgDB_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if ((Convert.ToDateTime(dtgDB.CurrentRow.Cells["vencimentoDataGridViewTextBoxColumn"].Value)) < GlbVariaveis.Sys_Data.Date)
            //{
            //    dtgDB.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red; // .CurrentRow.Cells["vencimentoDataGridViewTextBoxColumn"].Style.BackColor = Color.Red;
            //}
        }

        private void dtgDB_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

            if (this.dtgDB.Rows[e.RowIndex].Cells[8].Value == null)
                return;

            DateTime valor = (DateTime)this.dtgDB.Rows[e.RowIndex].Cells[8].Value;
            if (valor.Date < GlbVariaveis.Sys_Data.Date)
                dtgDB.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
            else
                dtgDB.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            grpPorConta.Visible = false;
            FrmDevolucao.numeroDevolucao = 0;
            FrmDevolucao.totalDevolucao = 0;
            btnDevolucao.BackColor = System.Drawing.SystemColors.Control;
        }

        private void btnPorContaGeral_Click(object sender, EventArgs e)
        {
            //valorPagamento = 0;
            LimparRecebimento(1);
            ChamarPorConta();
        }

        private void ChamarPorConta()
        {
            int parcela = Convert.ToInt32(dtgDB.CurrentRow.Cells["sequenciaincDataGridViewTextBoxColumn"].Value);

            siceEntities entidade = Conexao.CriarEntidade();

            var dadosParcela = (from n in entidade.crmovclientes
                                where n.sequenciainc == parcela
                                select n).First();

            if (dadosParcela.quitado == "S" && dadosParcela.ip != GlbVariaveis.glb_IP)
            {
                MessageBox.Show("Parcela já selecionada em outro terminal");
                return;
            }

            txtPorConta.Text = "0,00";
            jurosAntecipados = 0;
            if (valorPagamento > 0)
            {
                parcelas.Clear();
                MostrarExtrato(false);
                lblPagamento.Text = "R$ 0,00";
                valorPagamento = 0;
            }
            grpPorConta.Visible = true;
            porContaParcela = false;
            txtPorConta.Focus();
        }

        private void dtgDB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                ChamarPorConta();
            }
        }

        private void btnReimprimir_Click(object sender, EventArgs e)
        {
            int? documento = (from n in Conexao.CriarEntidade().contdocs
                              where n.data == GlbVariaveis.Sys_Data
                              && n.codigocliente == codigoCliente
                              && n.dpfinanceiro == "Recebimento"
                              && n.estornado == "N"
                              select (Int32?)n.documento).Max();
            if (documento.GetValueOrDefault() == 0)
            {
                MessageBox.Show("Não existe pagamento na data atual para ser impresso");
                return;
            }

            if (ConfiguracoesECF.idECF == 0)
            {
                MessageBox.Show("Sem ECF conectado ! ");
                return;
            }

            if (MessageBox.Show("Confirma reimpressão do último valor pago na data atual?", "SICEpdv.net", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                return;

            Clientes.ImprimirRecibo(documento.GetValueOrDefault());
        }

        private void btnItens_Click(object sender, EventArgs e)
        {
            if (dtgDB.RowCount == 0)
                return;

             FrmMsgOperador msgExtrato = new FrmMsgOperador("", "Buscando itens do documento...");
            msgExtrato.Show();
            try
            {
                Application.DoEvents();
                Int32 doc = Convert.ToInt32(dtgDB.CurrentRow.Cells["documentoDataGridViewTextBoxColumn"].Value);
                FrmItensDocumento frmItens = new FrmItensDocumento(doc);
                frmItens.ShowDialog();
            }
            finally
            {
                msgExtrato.Dispose();
            }
           
        }

        private void btnCarne_Click(object sender, EventArgs e)
        {

            if (ConfiguracoesECF.idECF == 0)
            {
                MessageBox.Show("Sem ECF conectado! ");
                return;
            }

            if (dtgDB.RowCount == 0)
                return;

            Int32 doc = Convert.ToInt32(dtgDB.CurrentRow.Cells["documentoDataGridViewTextBoxColumn"].Value);

            if (doc == 0)
            {
                MessageBox.Show("Não é possível imprimir carnê sem o numero do Documento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                Venda venda = new Venda();
                venda.ImprimirCarne(doc);
            }
        }

        private void FrmExtratoCliente_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (receber == false)
            {
                LimparRecebimento(0);
            }
        }

        private void sairCrediario()
        {
            if (receber == false)
            {
                LimparRecebimento(0);
            }
            else
            {
                Close();
            }
        }

        private void FrmExtratoCliente_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (receber == false)
            {
                LimparRecebimento(0);
            }
        }
    }
}



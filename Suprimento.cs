using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Db4objects.Db4o;
using System.Data.EntityClient;
using System.IO;

namespace SICEpdv
{
    public partial class Suprimento : Form
    {
        public static string tipoPagamento = "SU";
        TeclaNumerico teclado = new TeclaNumerico(Color.White);
        private string controle = "";
        private string tecla = "";
        private Boolean teclado1 = false;


        public Suprimento()
        {
            InitializeComponent();
            teclado.TabStop = false;
            this.Width = 400;
            btnTeclado.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg"); ;

            btnAbrirGaveta.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btnAbrirGaveta2.jpg"); ;


            pnlValor.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\backgroundSuprimento.jpg"); ;
            

            pnlTeclado.Controls.Add(teclado);
            txtValor.Enter += (objeto, evento) => controle = ActiveControl.Name;
            txtValor.KeyPress += (objeto, evento) =>
                {
                    if (!Permissoes.iniciarSaldoSuprimento)
                        evento.Handled = true;
                    Funcoes.DigitarNumerosPositivos(objeto, evento);
                };
            this.teclado.clickBotao += new TeclaNumerico.ClicarNoBotao(DelegateTeclado);
            this.Load += (objeto, evento) =>
                {
                    txtValor.SelectAll();
                    txtValor.Focus();
                };
            txtValor.KeyDown += (objeto, evento) =>
                {

                    if (evento.KeyValue != 13 && !Permissoes.iniciarSaldoSuprimento)
                    {
                        evento.Handled = true;                                                     
                        if (!Permissoes.iniciarSaldoSuprimento)
                        {
                            FrmLogon logon = new FrmLogon();
                            logon.campo = "rotaltersaldo";
                            logon.txtDescricao.Text = "Permissão para alterar ou iniciar saldo de caixa.";
                            logon.ShowDialog();
                            if (!Operador.autorizado)
                            {
                                return;
                            }
                            txtValor.Focus();
                            Permissoes.iniciarSaldoSuprimento = true;
                        }
                    }
                    if (evento.KeyValue == 13)
                    {
                        txtValor.Text = Funcoes.FormatarDecimal(txtValor.Text);                        
                        Lancar();
                    }
                };
            btnCancelar.Click += (objeto, evento) =>
                {
                    this.Close();
                    if (tipoPagamento == "SI")
                        Application.Exit();
                };
            if (tipoPagamento == "SU")
                btnSaldoAnt.Visible = false;
        }

        void DelegateTeclado(object sender, string text)
        {
            tecla = text;
            switch (tecla)
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
                case "txtValor":
                    Lancar();
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
            if (txtBox.Enabled == false ||  txtBox.Text.Trim().Length>=txtBox.MaxLength)                    
                return;
            txtBox.Text += this.tecla;
        }

        private void Lancar()
        {

            if (txtValor.Text=="")  txtValor.Text="0";
            DialogResult dialogo = DialogResult.No;
            dialogo = MessageBox.Show("Confirma Saldo?", "Confirma",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
            if (dialogo == DialogResult.No)
                return;

            if (tipoPagamento == "SI" && Conexao.onLine)
            {
                #region
                try
                {

                    siceEntities entidade;
                    if (Conexao.tipoConexao == 2)
                        entidade = Conexao.CriarEntidade(false);
                    else
                        entidade = Conexao.CriarEntidade();

                    /*EntityConnection conn = new EntityConnection(Conexao.stringConexao);

                    if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
                    {
                        conn = new EntityConnection(Conexao.stringConexaoRemoto);
                    }*/


                    /*using (conn)
                    {
                        conn.Open();
                        EntityCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "siceEntities.AberturaDia";
                        cmd.CommandType = CommandType.StoredProcedure;

                        EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                        filial.Direction = ParameterDirection.Input;
                        filial.Value = GlbVariaveis.glb_filial;

                        EntityParameter data = cmd.Parameters.Add("dataAtual", DbType.Date);
                        data.Direction = ParameterDirection.Input;
                        data.Value = GlbVariaveis.Sys_Data.Date;

                        EntityParameter hora = cmd.Parameters.Add("hora", DbType.Time);
                        hora.Direction = ParameterDirection.Input;
                        hora.Value = DateTime.Now.TimeOfDay;

                        EntityParameter ecf = cmd.Parameters.Add("nrFabricacaoECF", DbType.String);
                        ecf.Direction = ParameterDirection.Input;
                        ecf.Value = ConfiguracoesECF.nrFabricacaoECF;

                        cmd.ExecuteNonQuery();
                    }*/

                    string SQL = "CALL AberturaDia('" + GlbVariaveis.glb_filial + "','" + GlbVariaveis.Sys_Data.Date.ToString("yyyy-M-dd").Substring(0,10) + "','"+ DateTime.Now.TimeOfDay.ToString().Substring(0,8) + "','"+ ConfiguracoesECF.nrFabricacaoECF + "')";
                    entidade.ExecuteStoreCommand(SQL);
                    


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro atualizando estoques: " + ex.ToString());
                }
                #endregion
            }

            try
            {
                FuncoesECF.VerificaImpressoraLigada(true);

                FuncoesECF fecf = new FuncoesECF();
                if (tipoPagamento == "SU")
                {
                    if (!fecf.SuprimentoECF(decimal.Parse(txtValor.Text)))
                    {
                        MessageBox.Show("Não foi possível emitir comprovante no ECF!", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (tipoPagamento == "SI")
                {
                    FrmMsgOperador msg = new FrmMsgOperador("", "Abrindo caixa e cancelando pré-vendas, se houver! ");
                    msg.Show();
                    Application.DoEvents();
                    try
                    {
                        
                        if (!fecf.AberturaDoDiaECF(decimal.Parse(txtValor.Text)))
                        {
                            MessageBox.Show("Não foi possível abrir o movimento diário!", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                       
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }
                    finally
                    {
                        msg.Dispose();
                    }
                }


                var numeroCupom = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);
                var dataHoraCupomECF = FuncoesECF.DataHoraUltDocumentoECF();               

                #region StandAlone
                if (!Conexao.onLine)
                {
                    IObjectContainer tabela = Db4oFactory.OpenFile("caixa.yap");
                    StandAloneCaixa registro = new StandAloneCaixa();
                    registro.id = Guid.NewGuid();
                    registro.ip = GlbVariaveis.glb_IP;
                    registro.data = GlbVariaveis.Sys_Data;
                    registro.dpFinanceiro = "Saldo Inicial";
                    registro.valor = decimal.Parse(txtValor.Text);
                    registro.operador = GlbVariaveis.glb_Usuario;
                    registro.tipoPagamento = tipoPagamento;
                    registro.horaabertura = dataHoraCupomECF == null ? DateTime.Now.TimeOfDay : Convert.ToDateTime(dataHoraCupomECF).TimeOfDay;
                    registro.historico = "E:" + ConfiguracoesECF.numeroECF.ToString().PadRight(3, ' ') + "G:" + FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ') + "C:" + numeroCupom;
                    registro.vendedor = "000";                    
                    tabela.Store(registro);
                    tabela.Close();
                    this.Close();
                    return;
                };

                #endregion

                
                
                siceEntities entidade;
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();

              

                try
                {
                  
                    #region movimento
                    var movimento = (from n in entidade.movimento
                                     where n.codigofilial == GlbVariaveis.glb_filial
                                     && n.finalizado == " "
                                     select n.finalizado);


                    if (movimento.Count() == 0)
                    {

                        // Abrindo o movimento diário   
                        try
                        {

                            movimento novo = new movimento();
                            novo.codigofilial = GlbVariaveis.glb_filial;
                            novo.data = GlbVariaveis.Sys_Data;
                            novo.finalizado = " ";
                            novo.SaldoCaixa = 0;
                            novo.Credito = 0;
                            novo.Debito = 0;
                            novo.Saldocrediario = 0;
                            novo.creditocr = 0;
                            novo.debitocr = 0;
                            novo.saldocartao = 0;
                            novo.creditoca = 0;
                            novo.creditoch = 0;
                            novo.debitoca = 0;
                            novo.saldocheques = 0;
                            novo.debitoch = 0;
                            novo.custofinalestoque = 0;
                            novo.customediofinalestoque = 0;
                            novo.id = GlbVariaveis.glb_IP;
                            entidade.AddTomovimento(novo);
                            entidade.SaveChanges();

                        }
                        catch(Exception erro)
                        {
                            MessageBox.Show(erro.ToString());
                        }


                        if (Conexao.tipoConexao == 2 || Conexao.tipoConexao == 3)
                            StandAlone.salvarHistorico("movimento");

                        #region excluir
                        /*
                        if (Conexao.tipoConexao == 2)
                        {
                            movimento novoOff = new movimento();
                            novoOff.codigofilial = GlbVariaveis.glb_filial;
                            novoOff.data = GlbVariaveis.Sys_Data;
                            novoOff.finalizado = " ";
                            novoOff.SaldoCaixa = 0;
                            novoOff.Credito = 0;
                            novoOff.Debito = 0;
                            novoOff.Saldocrediario = 0;
                            novoOff.creditocr = 0;
                            novoOff.debitocr = 0;
                            novoOff.saldocartao = 0;
                            novoOff.creditoca = 0;
                            novoOff.creditoch = 0;
                            novoOff.debitoca = 0;
                            novoOff.saldocheques = 0;
                            novoOff.debitoch = 0;
                            novoOff.custofinalestoque = 0;
                            novoOff.customediofinalestoque = 0;
                            novoOff.id = GlbVariaveis.glb_IP;
                            siceEntities entidadeOff = Conexao.CriarEntidade(false);
                            entidadeOff.AddTomovimento(novoOff);
                            entidadeOff.SaveChanges();
                        }*/
                        #endregion
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    throw new Exception("Gravando no movimento inicial: " + ex.Message);
                };

                #region caixa

                caixa lancar = new caixa();
                lancar.tipopagamento = tipoPagamento;
                lancar.valor = Convert.ToDecimal(txtValor.Text);
                lancar.CodigoFilial = GlbVariaveis.glb_filial;
                lancar.filialorigem = GlbVariaveis.glb_filial;
                lancar.operador = GlbVariaveis.glb_Usuario;
                lancar.EnderecoIP = GlbVariaveis.glb_IP;
                lancar.dpfinanceiro = "Saldo inicial";
                lancar.data = dataHoraCupomECF == null ? DateTime.Now.Date : Convert.ToDateTime(dataHoraCupomECF).Date;
                lancar.horaabertura = dataHoraCupomECF == null ? DateTime.Now.TimeOfDay : Convert.ToDateTime(dataHoraCupomECF).TimeOfDay;
                lancar.versao = GlbVariaveis.glb_Versao;
                lancar.historico = "*";
                lancar.vendedor = "000";
                // Aqui é usado o histórico para gravar o número do ECF para tirar o 
                // relatório R07 com os documentos não fiscais.
                // Onde: E=NumeroEcF g=Contador Nao Fiscal GNF e C=COO numero do cupom emitido
                lancar.historico = "E:" + ConfiguracoesECF.numeroECF.PadRight(3,' ')+"G:"+FuncoesECF.ContadorNaoFiscalGNF().PadRight(6,' ')+"C:"+numeroCupom;
                lancar.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                lancar.ecfnumero = ConfiguracoesECF.numeroECF;
                lancar.ecfmodelo = ConfiguracoesECF.modeloECF;
                lancar.gnf = FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ');
                lancar.ccf = " ";
                lancar.estornado = "N";
                lancar.coo = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).PadRight(6, ' ');
                lancar.descricaopag = "Dinheiro";
                lancar.tipodoc = "2"; // TipoDoc 2 - Comprovante não fiscal
                lancar.eaddados = Funcoes.CriptografarMD5(lancar.ecffabricacao + lancar.coo + lancar.ccf + lancar.gnf + lancar.ecfmodelo + lancar.valor.ToString().Replace(",", ".") + lancar.tipopagamento);

                entidade.AddTocaixa(lancar);
                entidade.SaveChanges();

                if(Conexao.tipoConexao == 2 || Conexao.tipoConexao == 3)
                    StandAlone.salvarHistorico("caixa");

                //PreVenda.Cancelar();
                #endregion

                #region excluir
                /*
                if (Conexao.tipoConexao == 2)
                {
                    #region OFF caixa
                   
                    caixa lancarOff = new caixa();
                    lancarOff.tipopagamento = tipoPagamento;
                    lancarOff.valor = Convert.ToDecimal(txtValor.Text);
                    lancarOff.CodigoFilial = GlbVariaveis.glb_filial;
                    lancarOff.filialorigem = GlbVariaveis.glb_filial;
                    lancarOff.operador = GlbVariaveis.glb_Usuario;
                    lancarOff.EnderecoIP = GlbVariaveis.glb_IP;
                    lancarOff.dpfinanceiro = "Saldo inicial";
                    lancarOff.data = dataHoraCupomECF == null ? DateTime.Now.Date : Convert.ToDateTime(dataHoraCupomECF).Date;
                    lancarOff.horaabertura = dataHoraCupomECF == null ? DateTime.Now.TimeOfDay : Convert.ToDateTime(dataHoraCupomECF).TimeOfDay;
                    lancarOff.versao = GlbVariaveis.glb_Versao;
                    lancarOff.historico = "*";
                    lancarOff.vendedor = "000";
                    // Aqui é usado o histórico para gravar o número do ECF para tirar o 
                    // relatório R07 com os documentos não fiscais.
                    // Onde: E=NumeroEcF g=Contador Nao Fiscal GNF e C=COO numero do cupom emitido
                    lancarOff.historico = "E:" + ConfiguracoesECF.numeroECF.PadRight(3,' ')+"G:"+FuncoesECF.ContadorNaoFiscalGNF().PadRight(6,' ')+"C:"+numeroCupom;
                    lancarOff.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                    lancarOff.ecfnumero = ConfiguracoesECF.numeroECF;
                    lancarOff.ecfmodelo = ConfiguracoesECF.modeloECF;
                    lancarOff.gnf = FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ');
                    lancarOff.ccf = " ";
                    lancarOff.estornado = "N";
                    lancarOff.coo = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).PadRight(6, ' ');
                    lancarOff.descricaopag = "Dinheiro";
                    lancarOff.tipodoc = "2"; // TipoDoc 2 - Comprovante não fiscal
                    lancarOff.eaddados = Funcoes.CriptografarMD5(lancarOff.ecffabricacao + lancarOff.coo + lancarOff.ccf + lancarOff.gnf + lancarOff.ecfmodelo + lancarOff.valor.ToString().Replace(",", ".") + lancarOff.tipopagamento);
                    siceEntities entidadeOff = Conexao.CriarEntidade(false);
                    entidadeOff.AddTocaixa(lancarOff);
                    entidadeOff.SaveChanges();
                    
                    #endregion
                }
                */
                #endregion

                EntityConnection conn = new EntityConnection(Conexao.stringConexao);
                if (Conexao.tipoConexao == 2)
                {
                    conn = new EntityConnection(Conexao.stringConexaoRemoto);
                }

                using (conn)
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.AtualizarQdtRegistros";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao lancar!"+erro.InnerException.ToString(), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }

        private void VerificaSaldoInicial(object sender, FormClosingEventArgs e)
        {    
            if (txtValor.Text=="") txtValor.Text="0";
            //if (tipoPagamento == "SI" && Convert.ToDecimal(txtValor.Text)==0)
            //{
            //    Application.Exit();
            //    return;
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FuncoesECF.AbrirGaveta();
        }

        private void btnSaldoAnt_Click(object sender, EventArgs e)
        {

            if (!Permissoes.iniciarSaldoSuprimento)
            {
                FrmLogon logon = new FrmLogon();
                logon.campo = "rotaltersaldo";
                logon.txtDescricao.Text = "Permissão para alterar ou iniciar saldo de caixa.";
                logon.ShowDialog();
                if (!Operador.autorizado)
                {
                    return;
                }
            }

            ObterSaldoAnterior();
        }

        private void ObterSaldoAnterior()
        {
            if (Conexao.ConexaoOnline() == true)
            {
                if (!Conexao.onLine)
                    return;


                /*
                if (Configuracoes.saldoDoOperador == true)
                {
                    if (Configuracoes.mostrarsaldoliquido == true)
                        sql = @"SELECT saldoLiquidoEspecie FROM caixassoma WHERE operador='" + GlbVariaveis.glb_Usuario + "'" + " AND inc=(select MAX(inc) from caixassoma WHERE operador='" + GlbVariaveis.glb_Usuario + "' AND codigoFilial = '" + GlbVariaveis.glb_filial + "') AND codigoFilial = '" + GlbVariaveis.glb_filial + "'";
                    else
                        sql = @"SELECT saldocaixa FROM caixassoma WHERE operador='" + GlbVariaveis.glb_Usuario + "'" + " AND inc=(select MAX(inc) from caixassoma WHERE operador='" + GlbVariaveis.glb_Usuario + "' AND codigoFilial = '" + GlbVariaveis.glb_filial + "') AND codigoFilial = '" + GlbVariaveis.glb_filial + "'";
                }
                else
                {
                    if (Configuracoes.mostrarsaldoliquido == true)
                        sql = @"SELECT saldoLiquidoEspecie FROM caixassoma WHERE inc=(select MAX(inc) from caixassoma WHERE codigoFilial = '" + GlbVariaveis.glb_filial + "') AND codigoFilial = '" + GlbVariaveis.glb_filial + "'";
                    else
                        sql = @"SELECT saldocaixa FROM caixassoma WHERE inc=(select MAX(inc) from caixassoma WHERE codigoFilial = '" + GlbVariaveis.glb_filial + "') AND codigoFilial = '" + GlbVariaveis.glb_filial + "'";
                }
                */

                siceEntities entidade;
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();

                string  SQL = "CALL obterSaldo('" + GlbVariaveis.glb_filial + "','" + GlbVariaveis.glb_Usuario + "')";
                
                var contador = entidade.ExecuteStoreQuery<decimal>(SQL);
                txtValor.Text = contador.FirstOrDefault().ToString();
                txtValor.Focus();
            }
        }

        private void Suprimento_Load(object sender, EventArgs e)
        {
            this.focarSuprimento();
            if (Configuracoes.transportarSaldo && tipoPagamento == "SI")
            {
                ObterSaldoAnterior();                
            }
            txtValor.SelectAll();
            txtValor.Focus();
        }

        private void Suprimento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();

            if (e.KeyCode == Keys.Enter)
            {
                txtValor.SelectAll();
                txtValor.Focus();
                Lancar();
            }
        }

        private void txtValor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }

        public void focarSuprimento()
        {
            this.BringToFront();
            this.Focus();
            this.Activate();

            txtValor.SelectAll();
            txtValor.Focus();
        }

        private void MostrarTeclado()
        {

            pnlTeclado.Visible = true;
            teclado1 = true;
            txtValor.Focus();
            btnTeclado.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-enable.jpg"); ;

            this.Width = 617;

            txtValor.SelectAll();
            txtValor.Focus();

        }

        private void EsconderTeclado()
        {

            pnlTeclado.Visible = false;
            teclado1 = false;
            btnTeclado.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg"); ;

            this.Width = 400;

            txtValor.SelectAll();
            txtValor.Focus();

        }

        private void btnTeclado_Click(object sender, EventArgs e)
        {
            if (teclado1 == false)
            {
                MostrarTeclado();
            }
            else
            {
                EsconderTeclado();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtValor.SelectAll();
            txtValor.Focus();
            Lancar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Suprimento_Shown(object sender, EventArgs e)
        {
            txtValor.Focus();
        }
    }
}

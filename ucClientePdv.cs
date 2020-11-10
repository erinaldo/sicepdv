using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class ucClientePdv : UserControl
    {        
        public string tipoPagamento { get; set; }
        public string nomeCliente = "";
       
        public string dependente = "";
        public string cpfcnpjCliente = "";
        public string endCliente = "";
        public string endNumero = "";
        public string endBairro = "";
        public string endCidade = "";
        public string endEstado = "";
        public string endCEP = "";
        public int idCliente = 0;
        public int idCartao = 0;

        public DadosCheque dadoCheque;
        public delegate void Controle(string campo);
        public event Controle EntraControle;
        public ucClientePdv()
        {
            InitializeComponent();

            if (!Permissoes.mudarVencimento)
            {
                vencimentoCR.Enabled = false;
                txtDiasVenc.Enabled = false;
            }

            vencimentoCR.Value = GlbVariaveis.Sys_Data.AddMonths(1);
            txtIdCliente.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            btnExtrato.Click += (objeto, evento) => EntraControle(ActiveControl.Name);
           
            txtIdCliente.KeyPress += (objeto, evento) =>
                {
                    if (evento.KeyChar == Convert.ToChar(Keys.Escape))
                        EntraControle("btSairCR");
                    if (evento.KeyChar == ',' || evento.KeyChar == '.')
                        evento.Handled = true;
                    
                    Funcoes.DigitarNumerosPositivos(objeto, evento);
                };

            txtParcelamentoCH.KeyPress += (objeto, evento) =>
                {
                    Funcoes.DigitarNumerosPositivos(objeto, evento);
                    if (evento.KeyChar == ',' || evento.KeyChar == '.')
                        evento.Handled = true;
                };            

            txtDiasVenc.KeyPress += (objeto,evento) =>
                {
                    Funcoes.DigitarNumerosPositivos(objeto, evento);
                    if (evento.KeyChar == ',' || evento.KeyChar == '.')
                        evento.Handled = true;
                };
            txtDiasVenc.Leave += (objeto, evento) =>
                {
                    if (txtDiasVenc.Text == "")
                        txtDiasVenc.Text = "30";
                    vencimentoCR.Value = DateTime.Now.AddDays(Convert.ToInt16(txtDiasVenc.Text));
                    if (txtDiasVenc.Text == "30" && !Configuracoes.diasCorridos)
                        vencimentoCR.Value = DateTime.Now.AddMonths(1);
                    if (txtDiasVenc.Text != "30" || Configuracoes.diasCorridos)
                        vencimentoCR.Value = DateTime.Now.AddDays(Convert.ToInt16(txtDiasVenc.Text));
                    
                };
            txtDiasVenc.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Return)
                    {
                        vencimentoCR.Focus();
                        evento.SuppressKeyPress = true;
                    }
                };

            txtParcelamentoCR.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
                if (evento.KeyChar == ',' || evento.KeyChar == '.')
                    evento.Handled = true;
            };


            txtIdCliente.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Return && string.IsNullOrEmpty(txtIdCliente.Text))
                        MostrarClientes();
                };
            btSairCR.Click += (objecto, evento) =>
            {
                setarConsumidor();
            };

            btConfirmarCR.Click += (objeto, evento) =>
                {
                    EntraControle(ActiveControl.Name);
                };
            btConfirmarCH.Click += (objeto, evento) =>
                {
                    ValidaDadosCheque();
                    EntraControle(ActiveControl.Name);
                };
            txtIdCliente.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyValue == 13)
                    {
                        ProcuraCliente();
                        btnExtrato.Enabled = true;
                        if (tipoPagamento == "") 
                        ProximoControle(evento);                        
                    }
                };
            
            txtParcelamentoCR.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtParcelamentoCR.KeyPress += (objeto, evento) =>
                {                  
                    Funcoes.DigitarNumerosPositivos(objeto, evento);                 
                };

            txtParcelamentoCR.KeyDown += (objeto, evento) => ProximoControle(evento);
            txtIntervaloCR.KeyDown += (objeto, evento) => ProximoControle(evento);   
            vencimentoCR.KeyDown+= (objeto,evento) => ProximoControle(evento);
            vencimentoCR.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);

            txtConsumidor.KeyDown += (objeto, evento) => ProximoControle(evento);
            txtIntervaloCR.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
                if (evento.KeyChar == ',' || evento.KeyChar == '.')
                    evento.Handled = true;
            };
            txtIntervaloCR.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);

            btSairCR.Click += (objeto, evento) => EntraControle(ActiveControl.Name);
            //Dados Cheques
            txtCodBanco.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtAgencia.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtNrCheque.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtNomeCH.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtCPFCNPJch.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtTelefoneCH.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            //Controle Cheques Parcelamento
            txtValorIndCH.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);            
            txtParcelamentoCH.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            vencimentoCH.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtIntervaloCH.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtTelefoneCH.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto,evento);
            txtDiasVenc.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtCPFCNPJch.Leave += (objeto, evento) =>
                {
                    if (txtCPFCNPJch.Text.Length<=11) 
                    {
                        if (!Funcoes.ValidarCPF(txtCPFCNPJch.Text))
                        {
                            MessageBox.Show("CNPJ ou CPF errado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtCPFCNPJch.Focus();
                        }
                    }
                    if (txtCPFCNPJch.Text.Length > 11)
                    {
                        if (!Funcoes.ValidaCnpj(txtCPFCNPJch.Text))
                        {
                            MessageBox.Show("CNPJ ou CPF errado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtCPFCNPJch.Focus();
                        }
                    }
                };

            btnCancelar.Click += (objeto, evento) =>
                {
                    Cancelar();
                    EntraControle("btSairCR");
                };
      
        }

        private void setarConsumidor()
        {
            if (idCliente == 0)
            {
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
                    ecfConsumidor = txtConsumidor.Text,
                    ecfCNPJCPFConsumidor = txtIdCliente.Text
                };

                nomeCliente = txtConsumidor.Text;
                endCliente = txtEndConsumidor.Text;
                cpfcnpjCliente = txtIdCliente.Text;
            };
        }

        public void Cancelar()
        {
            txtConsumidor.Text = "";
            txtEndConsumidor.Text = "";
            txtIdCliente.Text = "";
            lblNome.Text = "";
            lblCpfCnpj.Text = "";
            lblEndereco.Text = "";
            lblCidade.Text = "";
            //lblStatus.Text = "";
            lblSaldo.Text = "";
            idCliente = 0;
            nomeCliente = "";

            LimparCampos();
        }                

        private static void ProximoControle(KeyEventArgs evento)
        {
            if (evento.KeyValue == 13)
            {                                
                SendKeys.Send("{TAB}");
                evento.SuppressKeyPress = true;
            }
        }

        public void ProcuraCliente()
        {
            string ecfConsumidor = "";
            string ecfcnpjCpfConsumidor = "";

            if (Convert.ToInt64((txtIdCliente.Text == "" || txtIdCliente.Text == null) ? "0" : txtIdCliente.Text) != idCliente)
            {
                //cpfcnpjCliente = "";
                ecfConsumidor = "";
            }

            Clientes cliente = new Clientes();

            ecfcnpjCpfConsumidor = cpfcnpjCliente;
            ecfConsumidor = nomeCliente;

            if (cliente.Procura(txtIdCliente.Text))
            {
                tableLayoutPanel1.Visible = true;
                layoutConsumidor.Visible = false;
                lblNome.Text = cliente.nome;
                lblCpfCnpj.Text = cliente.cpf;
                lblEndereco.Text = cliente.endereco;
                lblCidade.Text = cliente.cidade+" "+cliente.estado;
                //lblStatus.Text = cliente.situacao;
                idCliente = cliente.idCliente;
                nomeCliente = cliente.nome;
                cpfcnpjCliente = cliente.cpf != "" ? cliente.cpf : cliente.cnpj;
                endCliente = cliente.endereco;
                endNumero = cliente.numero;
                endBairro = cliente.bairro;
                endCidade = cliente.cidade;
                endEstado = cliente.estado;
                endCEP = cliente.cep;


               Venda.dadosConsumidor =
               new DadosConsumidorCupom
               {
                   cpfCnpjConsumidor = cpfcnpjCliente,
                   nomeConsumidor = nomeCliente,
                   endConsumidor = endCliente,
                   endNumero = endNumero,
                   endBairro = endBairro,
                   endCEP = endCEP,
                   endCidade = endCidade,
                   endEstado = endEstado,
                   idConsumidor = idCliente.ToString(),
                   ecfConsumidor = (ecfConsumidor == "" || ecfConsumidor == null) ? nomeCliente : ecfConsumidor,
                   ecfCNPJCPFConsumidor = (ecfcnpjCpfConsumidor == "" || ecfcnpjCpfConsumidor == null) ? cpfcnpjCliente : ecfcnpjCpfConsumidor
               };

                txtConsumidor.Text = nomeCliente;
                txtCPFCNPJch.Text = cpfcnpjCliente;
                txtNomeCH.Text = cliente.nome.PadRight(30,' ').Substring(0,30);
                txtEndConsumidor.Text = cliente.endereco;
                lblSaldo.Text = "Saldo: " + string.Format("{0:c2}", cliente.saldoAtual);
                if (cliente.saldoAtual < 0)
                    lblSaldo.ForeColor = Color.Red;
                else
                    lblSaldo.ForeColor = Color.White;

                if ( tipoPagamento == "CR" || tipoPagamento == "CH" || chkVerDependentes.Checked )
                {
                    siceEntities entidade = Conexao.CriarEntidade();
                    if (Conexao.tipoConexao == 2)
                        entidade = Conexao.CriarEntidade(false);

                    int nDependentes = (from n in entidade.dependentes
                                        where n.codigocliente == idCliente
                                        select n.codigocliente).Count();
                    if (nDependentes > 0)
                    {
                        frmDependente frmDependente = new frmDependente(idCliente);
                        frmDependente.ShowDialog();
                        dependente = frmDependente.nomeDependente;
                        frmDependente.Dispose();
                    }

                }

                if (tipoPagamento=="CR")
                {
                    Application.DoEvents();
                    pnlParcelamentoCR.Visible = true;
                    pnlParcelamentoCR.Enabled = true;
                    txtDiasVenc.Focus();
                    if (vencimentoCR.Enabled == false)
                        txtParcelamentoCR.Focus();                    
                    
                }
                if (tipoPagamento == "CH")
                {
                    this.Size = new System.Drawing.Size(370, 400);
                    
                    pnlParcelamentoCR.Visible = false;
                    pnlCheque.Visible = true;
                    txtCodBanco.Focus();
                    Application.DoEvents();
                }
                return;
            };

            if (tipoPagamento=="")
            {
                LimparCampos();
                //lblStatus.Text = "Não identificado";
                /// Aqui para sair o CPF e o nome do Consumidor no Cupom Fiscal
                ///     
                if (txtIdCliente.Text.Length > 10)
                {
                    txtConsumidor.Focus();
                    pnlParcelamentoCR.Enabled = false;
                    layoutConsumidor.Visible = true;
                    txtIdCliente.Focus();
                }                
                //txtConsumidor.Focus();
                tableLayoutPanel1.Visible = false;                
            }
            idCliente = 0;                          
             
            lblNome.Text = "";
            lblEndereco.Text = "";
            lblCpfCnpj.Text = "";
            lblCidade.Text = "";
            lblSaldo.Text = "";
            //lblStatus.Text = "Não identificado";
            //lblStatus.ForeColor = System.Drawing.Color.Red;
            pnlCheque.Visible = false;
            this.Height = 450;
        }

        private void ProximoControle(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            ProximoControle(e);
        }

        private void DigitarNumeros(object sender, KeyPressEventArgs e)
        {
            Funcoes.DigitarNumerosPositivos(sender,e);
        }

        public bool ValidaDadosCheque()
        {
            if (string.IsNullOrEmpty(txtNrCheque.Text))
                txtNrCheque.Text = "0";

            if (txtCodBanco.Text == "" || txtAgencia.Text == "" || txtNomeCH.Text=="" || txtCPFCNPJch.Text == "")
            {
                return false;
            };

            try
            {
                DadosCheque dc = new DadosCheque();
                dc.agencia = txtAgencia.Text;
                dc.codBanco = Convert.ToInt32(txtCodBanco.Text);
                dc.cpfCheque = txtCPFCNPJch.Text;
                dc.nomeCheque = txtNomeCH.Text;
                dc.numeroCheque = int.Parse(txtNrCheque.Text);
                dc.telCheque = txtTelefoneCH.Text;
                dc.conta = txtConta.Text;
                dadoCheque = dc;

                Venda.dadosCheque = dc;
                return true;
            }
            catch (Exception erro)
            {
                MessageBox.Show("Numeração do Cheque muito grande! Não foi possivel converter para inteiro", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void LimparCampos()
        {
            Clientes.restricao = false;
            Clientes.inadimplente = false;
            Clientes.limiteUltrapassado = false;



            //txtIdCliente.Text = "";
            txtConsumidor.Text = "";
            txtEndConsumidor.Text = "";
            txtParcelamentoCH.Text = "1";
            txtParcelamentoCR.Text = "1";
            txtIntervaloCH.Text = "30";
            txtIntervaloCR.Text = "30";
            txtCodBanco.Text = "";
            txtAgencia.Text = "";
            txtNrCheque.Text = "";
            txtNomeCH.Text = "";
            txtCPFCNPJch.Text = "";
            txtTelefoneCH.Text = "";            
            nomeCliente = "";
            cpfcnpjCliente = "";
            endCliente = "";
            dependente = "";
            idCliente = 0;
            txtIdCliente.Enabled = true;
            tableLayoutPanel1.Visible = false;
            layoutConsumidor.Visible = false;

            Venda.dadosConsumidor =
               new DadosConsumidorCupom
               {
                   cpfCnpjConsumidor = "",
                   nomeConsumidor = "",
                   endConsumidor ="",
                   endNumero = "",
                   endBairro = "",
                   endCEP = "",
                   endCidade = "",
                   endEstado = "",
                   idConsumidor = ""
               };
        }

        private void ucClientePdv_Load(object sender, EventArgs e)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            btnExtrato.Enabled = false;
            cboRedeConsulta.Items.Clear();
            cboRedeConsulta.Items.AddRange((from n in entidade.cartoes
                                            where n.transacao == "CHQ"
                                            select n.descricao).ToArray());
            cboRedeConsulta.Items.Add(" Nenhum...");                        
            cboRedeConsulta.Sorted = true;
            cboRedeConsulta.SelectedIndex = 0;
            txtIdCliente.Text = "0";
        }

        private void cboRedeConsulta_Leave(object sender, EventArgs e)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            idCartao = (from n in entidade.cartoes
                        where n.descricao == cboRedeConsulta.Text
                        && n.transacao == "CHQ"
                        select n.id).FirstOrDefault();
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            MostrarClientes();
        }

        private void MostrarClientes()
        {
            FrmClientes frmCli = new FrmClientes();
            frmCli.ShowDialog();
            txtIdCliente.Text = Convert.ToString(FrmClientes.codigoCliente);
            txtIdCliente.Focus();
        }

        private void txtIdCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }

        private void layoutConsumidor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void chkVerDependentes_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void btnExtrato_Click(object sender, EventArgs e)
        {

        }

        private void pnlCheque_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btSairCR_Click(object sender, EventArgs e)
        {

        }

        private void vencimentoCR_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pnlParcelamentoCR_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtIdCliente_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblIdCliente_Click(object sender, EventArgs e)
        {

        }

        private void txtEndConsumidor_KeyDown(object sender, KeyEventArgs e)
        {
            setarConsumidor();
            //btSairCR.Focus();
            ProximoControle(e);

        }
    }

    public class DadosCheque
    {
        public int codBanco { get; set; }
        public string agencia { get; set; }
        public int numeroCheque { get; set; }
        public string nomeCheque { get; set; }
        public string cpfCheque { get; set; }
        public string telCheque { get; set; }
        public string conta { get; set; }
    };
}

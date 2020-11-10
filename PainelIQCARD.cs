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
    public partial class PainelIQCARD : Form
    {
        ServiceReference1.Empresa dadosEmpresa = new ServiceReference1.Empresa();
        public PainelIQCARD()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PainelIQCARD_Load(object sender, EventArgs e)
        {
            try
            {
                MostrarDados();
                this.Text = "E-mail da conta IQCARD: " + dadosEmpresa.email;
                if (GlbVariaveis.glb_chaveIQCard.Length == 16)
                    this.Text = "IQCARD: " +IqCard.FormatarCartao(GlbVariaveis.glb_chaveIQCard);

            }
            catch (Exception)
            {
                
            }
           

            if(!Permissoes.administrador)
            {
                pnlFinanc.Visible = false;
            }
            
        }

        private void MostrarDados()
        {
            IqCard iqcard = new IqCard();
            dadosEmpresa = iqcard.DadosEmpresa(GlbVariaveis.glb_chaveIQCard);

            int totalPontos = iqcard.SaldoEticket();
            if (totalPontos < 10000)
                btnRecarregar.Visible = true;

            lblPontos.Text = dadosEmpresa.saldoEticket.ToString() + " PONTOS";
            lblCreditos.Text = "R$: " + string.Format("{0:N2}", dadosEmpresa.saldo);

            if(dadosEmpresa.saldo<=0)
            {
                
                btnDepositar.Enabled = false;
            }

            try
            {
                var totalPontosRecentes = iqcard.PontosRecentes();
                lblPontosRec.Text = totalPontosRecentes.ToString();
                lblVendasGeradas.Text = string.Format("{0:N2}", ((totalPontosRecentes * 0.05M)*100) / (Configuracoes.coefecientePontosIQCard * 5));
            }
            catch (Exception)
            {
                
            }

           

            try
            {
                if (Convert.ToDouble(Configuracoes.coefecientePontosIQCard) != dadosEmpresa.coeficientepontosiqcard)
                {
                    iqcard.AtualizarCoeficiente(Convert.ToDouble(Configuracoes.coefecientePontosIQCard));
                }
            }
            catch (Exception)
            {
               
            }
            

            try
            {
                //string sql = "SELECT clientes.nome,clientes.cpf FROM clientes,crmovclientes " +
                //        " WHERE clientes.codigo = crmovclientes.codigo AND crmovclientes.vencimento = ADDDATE(CURRENT_DATE, INTERVAL 1 DAY)" +
                //        " GROUP BY clientes.Codigo";

                //var lembretes = Conexao.CriarEntidade().ExecuteStoreQuery<lembrete>(sql).ToList();
                
            }
            catch (Exception)
            {
                
            }
           


            IqCard.mostrarPainelIQCARD = false;
        }

        private void lblPontos_Click(object sender, EventArgs e)
        {
            menuComprar.Show(lblPontos, new Point(lblPontos.Width, 0));
            //System.Diagnostics.Process.Start("https://iqcard.com.br");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRecarregar_Click(object sender, EventArgs e)
        {

            menuComprar.Show(btnRecarregar, new Point(btnRecarregar.Width, 0));
            //System.Diagnostics.Process.Start("https://iqcard.com.br");
        }

        private void btnDepositar_Click(object sender, EventArgs e)
        {
            try
            {
                FrmLogon logon = new FrmLogon();
                logon.campo = "gerente";
                logon.txtDescricao.Text = "Solicitação de depósito. Use senha de gerente";
                logon.ShowDialog();
                if (!Operador.autorizado)
                {
                    return;
                }

                IqCard iqcard = new IqCard();
                var resultado = iqcard.SolicitarDeposito();
                MessageBox.Show("Solicitação de depósito efetuada com sucesso. Até 3 dias úteis efetuaremos o depósito. Obrigado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          


        }

        private void btnConverter_Click(object sender, EventArgs e)
        {
            ComprarPontos(0);
        }

        private void ComprarPontos(double valor)
        {
            FrmLogon logon = new FrmLogon();
            logon.campo = "gerente";
            logon.txtDescricao.Text = "Transformar saldo de crédito em pontos. Use senha de gerente";
            logon.ShowDialog();
            if (!Operador.autorizado)
            {
                return;
            }



            if (MessageBox.Show("Transformar o valor em pontos. Após a confirmação não será mais possível reverter ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            };

            try
            {
                IqCard iqcard = new IqCard();
                var resultado = iqcard.TransformarCreditoEmPontos(valor);
                lblCreditos.Text = "0";
                MessageBox.Show("Saldo convertido com sucesso!");
                MostrarDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pnlPontosRec_Click(object sender, EventArgs e)
        {
            PontosAcumulados();
        }

        private static void PontosAcumulados()
        {
            IndexPontosAcumulados pontos = new IndexPontosAcumulados();
            pontos.ShowDialog();
        }

        private void lblPontosRec_Click(object sender, EventArgs e)
        {
            PontosAcumulados();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            PontosAcumulados();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void menuVenda(object sender, EventArgs e)
        {

        }

        private void comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://iqcard.com.br");
        }

        private void comprarComBoletoBancárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolicitarRecarga(2000);

        }

        private static void SolicitarRecarga(int pontos)
        {
            FrmLogon logon = new FrmLogon();
            logon.campo = "gerente";
            logon.txtDescricao.Text = "Comprar pontos. Use senha de gerente";
            logon.ShowDialog();
            if (!Operador.autorizado)
            {
                return;
            }

            try
            {
                IqCard iqcard = new IqCard();
                iqcard.SolicitarRecargaPontos(pontos);
                MessageBox.Show("Solicitação realizada com sucesso. O boleto será enviado por email. E quando tiver sido processado seu saldo de pontos será atualizado. Obrigado!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void convertoOMeuCréditoEmPontosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnConverter_Click(sender, e);
        }

        private void pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolicitarRecarga(4000);
        }

        private void pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolicitarRecarga(6000);
        }

        private void converterMeuCréditoEmPontosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnConverter_Click(sender, e);
        }

        private void r100002000PONTOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComprarPontos(100);
        }

        private void pnlComentarios_Click(object sender, EventArgs e)
        {
            IQCardComentarios comentarios = new IQCardComentarios();
            comentarios.ShowDialog();
        }

        private void lblCV_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://iqcard.com.br/curriculum/cvpublico?chave="+GlbVariaveis.glb_chaveIQCard);
        }

        private void pnlAviso_Click(object sender, EventArgs e)
        {
            MostrarLembreteVencimento();
        }

        private static void MostrarLembreteVencimento()
        {
            AvisoVencimento aviso = new AvisoVencimento();
            aviso.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MostrarLembreteVencimento();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void lblComentarios_Click(object sender, EventArgs e)
        {

        }

        private void pnlComentarios_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PainelIQCARD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar ==( char)Keys.Escape)
                Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void lblVendasGeradas_Click(object sender, EventArgs e)
        {

        }
    }
}

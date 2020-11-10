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
    public partial class IndexPontosAcumulados : Form
    {
        IqCard iqcard = new IqCard();
        public IndexPontosAcumulados()
        {
            InitializeComponent();
        }

        private void IndexPontosAcumulados_Load(object sender, EventArgs e)
        {
            this.Text = "IQCARD: "+IqCard.FormatarCartao(GlbVariaveis.glb_chaveIQCard);
            ServiceReference1.Empresa dadosEmpresa = new ServiceReference1.Empresa();
            try
            {
                
                dadosEmpresa = iqcard.DadosEmpresa(GlbVariaveis.glb_chaveIQCard);
                var totalPontosRecentes = iqcard.PontosRecentes();
                ptsDistribuidos.Text = totalPontosRecentes.ToString();
                lblVendasGeradas.Text = string.Format("{0:N2}", ((totalPontosRecentes * 0.05M) * 100) / (Configuracoes.coefecientePontosIQCard * 5));
                txtSaldoCreditos.Text = "R$: " + string.Format("{0:N2}", dadosEmpresa.saldo);
                logoEmpresa.ImageLocation = "https://iqcard.blob.core.windows.net/logos/" + GlbVariaveis.idCliente + "_logo";
                lblUsuarios.Text = IqCard.usuariosProcurandoPromo.ToString() + " usuários";
            }
            catch (Exception)
            {
                
            }
           

            /* IqCard iqcard = new IqCard();
            dtgClientes.DataSource = null;
            dtgClientes.AutoGenerateColumns = false;

            
            var dados = iqcard.PontosRecentesListagem();
            int x = 0;
            foreach (var item in dados)
            {
                dados[x].idCartao ="#"+item.idCartao.Substring(12, 4);
                x++;
            }
            dtgClientes.DataSource = dados.AsQueryable().ToList();
            */

            this.Size = new System.Drawing.Size(823, 636);
            pnlTabela.Location = new Point(279, 46);
            pnlSobre.Location = new Point(279, 46);
            pnlRecarga.Location = new Point(279, 46);
            pnlDeposito.Location = new Point(279, 46);
            pnlDashboard.Location = new Point(279, 46);

            txtTitulo.Text = "";
            txtNomeEmpresa.Text = GlbVariaveis.nomeEmpresa;            
            lblPontos.Text = dadosEmpresa.saldoEticket.ToString();
            

            try
            {
                dashboard.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\dashboard.png");
                picPessoas.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\pessoas.png");
                // dashboard.png
                card.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\card.png");

                picWarning.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\warning.png");

                picSetinha.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\distribuicao.png");
                picCasinha.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\casinha.png");
                picSetaBaixo.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\increase.png");
                picDinheiro.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\dinheiro.png");

                pikRecarga.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\card-branco.png");
                pikInfo.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\info-branco.png");
                pikMoney.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\money-branco.png");
                pikRelogio.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\relogio-branco.png");

                grana1.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\grana.png");
                grana2.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\grana.png");

                carrinho6000.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\carrinho-azul.png");
                carrinho8000.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\carrinho-azul.png");
                carrinho10000.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\carrinho-azul.png");
                carrinho20000.Image = Image.FromFile(@Application.StartupPath + @"\imagensMetro\iqcard\carrinho-azul.png");
            }
            catch (Exception)
            {

            }

        }
        /*
        if(ptsDisponiveis< 100)
        {
            pnlFazerRecarga.Visible = true;
        }*/

        private void cartaoPresente()
        {

        }


        public void valorRecarga(int pontos)
        {
            MessageBox.Show("O cliente solicitou " + pontos + " pontos");
            PagamentoOnLine pag = new PagamentoOnLine();
            PagamentoOnLine.produto = "recarga";
            PagamentoOnLine.quantidade = pontos;
            PagamentoOnLine.total = pontos * 0.05;
            pag.ShowDialog();
        }


        private void label2_Click(object sender, EventArgs e)
        {
            recarga();
        }

        private void txtUltimaSemana_Click(object sender, EventArgs e)
        {
            semana();
        }

        private void txtDeposito_Click(object sender, EventArgs e)
        {
            deposito();
        }

        private void txtSaibaMais_Click(object sender, EventArgs e)
        {
            sobre();
        }

        public void deposito()
        {
            txtTitulo.Text = "DEPÓSITO";

            pnlDeposito.Visible = true;
            pnlSobre.Visible = false;
            pnlTabela.Visible = false;
            pnlRecarga.Visible = false;

            pnlDashboard.Visible = false;
        }

        public void sobre()
        {
            txtTitulo.Text = "SOBRE";

            pnlSobre.Visible = true;
            pnlTabela.Visible = false;
            pnlRecarga.Visible = false;
            pnlDeposito.Visible = false;

            pnlDashboard.Visible = false;

        }

        public void semana()
        {
            txtTitulo.Text = "ÚLTIMA SEMANA";

            pnlTabela.Visible = true;
            pnlRecarga.Visible = false;
            pnlSobre.Visible = false;
            pnlDeposito.Visible = false;

            pnlDashboard.Visible = false;
            dtgClientes.DataSource = iqcard.PontosRecentes();
        }

        public void recarga()
        {
            txtTitulo.Text = "RECARGAS";

            pnlRecarga.Visible = true;
            pnlSobre.Visible = false;
            pnlTabela.Visible = false;
            pnlDeposito.Visible = false;

            pnlDashboard.Visible = false;

        }

        
        public void dbDashboard()
        {
            pnlDashboard.Visible = true;
            txtTitulo.Text = "";

            pnlRecarga.Visible = true;
            pnlSobre.Visible = false;
            pnlTabela.Visible = false;
            pnlDeposito.Visible = false;
        }
       


        private void label2_Click_1(object sender, EventArgs e)
        {
            valorRecarga(1000);
        }

        private void lbl2000_Click(object sender, EventArgs e)
        {
            valorRecarga(2000);
        }

        private void lbl6000_Click(object sender, EventArgs e)
        {
            valorRecarga(6000);
        }

        private void lbl8000_Click(object sender, EventArgs e)
        {
            valorRecarga(8000);
        }

        private void lbl10000_Click(object sender, EventArgs e)
        {
            valorRecarga(10000);
        }

        private void lbl20000_Click(object sender, EventArgs e)
        {
            valorRecarga(20000);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            cartaoPresente();
        }

        private void pnl100_Click(object sender, EventArgs e)
        {
            valorRecarga(100);
        }

        private void pnl2000_Click(object sender, EventArgs e)
        {
            valorRecarga(2000);
        }

        private void pnl6000_Click(object sender, EventArgs e)
        {
            valorRecarga(6000);
        }

        private void pnl8000_Click(object sender, EventArgs e)
        {
            valorRecarga(8000);
        }

        private void pnl10000_Click(object sender, EventArgs e)
        {
            valorRecarga(10000);
        }

        private void pnl20000_Click(object sender, EventArgs e)
        {
            valorRecarga(20000);
        }

        private void card_Click(object sender, EventArgs e)
        {
            cartaoPresente();
        }

        private void carrinho6000_Click(object sender, EventArgs e)
        {
            valorRecarga(6000);
        }

        private void carrinho8000_Click(object sender, EventArgs e)
        {
            valorRecarga(8000);
        }

        private void carrinho10000_Click(object sender, EventArgs e)
        {
            valorRecarga(10000);
        }

        private void carrinho20000_Click(object sender, EventArgs e)
        {
            valorRecarga(20000);
        }

        private void pikRecarga_Click(object sender, EventArgs e)
        {
            recarga();
        }

        private void pikRelogio_Click(object sender, EventArgs e)
        {
            semana();
        }

        private void pikMoney_Click(object sender, EventArgs e)
        {
            deposito();
        }

        private void pikInfo_Click(object sender, EventArgs e)
        {
            sobre();
        }

        private void logoEmpresa_Click(object sender, EventArgs e)
        {
            dbDashboard();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            recarga();
        }

        private void picWarning_Click(object sender, EventArgs e)
        {
            recarga();
        }

        private void pnlFazerRecarga_Click(object sender, EventArgs e)
        {
            recarga();
        }

        private void txtNomeEmpresa_Click(object sender, EventArgs e)
        {
            dbDashboard();
        }

        private void lblPontos_Click(object sender, EventArgs e)
        {
            recarga();
        }

        private void ptsDistribuidos_Click(object sender, EventArgs e)
        {
            semana();
        }

        private void txtSaldoCreditos_Click(object sender, EventArgs e)
        {
            deposito();
        }

        private void lblVendasGeradas_Click(object sender, EventArgs e)
        {
            semana();
        }
    }
}

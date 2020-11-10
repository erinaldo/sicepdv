using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using QRCoder;

namespace SICEpdv
{
    public partial class FrmLogon : Form
    {
        TeclaNumerico teclado = new TeclaNumerico(Color.White);
        private string controle = "";
        private string tecla = "";
        public string campo = "login";
        public int idCliente = 0;
        public string codProduto = "";
        public Boolean teclado1 = false;
        public Boolean teclado2 = false;
        string iqcardSuporteNome = "";


        private bool iniciarThead = true;

        public FrmLogon()
        {
            InitializeComponent();
            Operador.autorizado = false;
            txtOperador.Enter += (objeto, evento) => controle = ActiveControl.Name;
            try
            {
                string sql = "SELECT idcliente FROM iqsistemas limit 1";
                string idCliente = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                lblIDCliente.Text = " ID " + idCliente;

            }
            catch (Exception)
            {
                
            }


            //if (File.Exists(@"C:\iqsistemas\IQSync\ToDo\fecharSync.txt"))
            //    File.Delete(@"C:\iqsistemas\IQSync\ToDo\fecharSync.txt");

            try
            {
                Funcoes.escreveArquivo(@"C:\iqsistemas\IQSync\ToDo\fecharSync.txt", "");
            }
            catch (Exception)
            {
                
            }            


            txtOperador.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Return)
                    {
                        SendKeys.Send("{TAB}");
                        evento.SuppressKeyPress = true;
                    }


                };
            txtOperador.Leave += (objeto, evento) =>
            {
                txtOperador.Text = txtOperador.Text.ToUpper();
                if (txtOperador.Text.Contains("0359"))
                    VerificaAutorizacao();
            };

            txtSenha.Enter += (objeto, evento) => controle = ActiveControl.Name;
            txtSenha.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyValue == 13)
                    {
                        VerificaAutorizacao();
                        evento.SuppressKeyPress = true;
                    }

                };

            //progressBar1.Maximum = 100;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            pnlLogin.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\login.jpg");
            logoPdv.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\logo2.png");


        }

        private void DispararThread()
        {
            IqCard iqcard = new IqCard();
            if (!backgroundWorker1.IsBusy)
            {                
                // lblAguarde.Text = "Preparando..";
                //progressBar1.Visible = true;
                backgroundWorker1.RunWorkerAsync();
              
                //progressBar1.Value = 10;
               // Thread.Sleep(500);
            }
        }

        private void FrmLogon_Load(object sender, EventArgs e)
        {

            try
            {                
                Configuracoes.carregar("00001");
                avaliacao();
                //Ivan 28.09.2020 Para mostrar no site as promocoes ativas.                
            }
            catch (Exception)
            {                
            }

            // Desabilitar botões de fechar, minimizar e maximizar
            this.FormBorderStyle = FormBorderStyle.None;

            Funcoes.TravarTeclado(false);
            this.Height = 242;
            if (campo == "" || campo == "login")
            {
                txtDescricao.Font = new Font("Arial", 14);
                if (txtDescricao.Text == "")
                    txtDescricao.Text = "Entrada no sistema";
            };
            teclado.TabStop = false;
            pnlTeclado.Controls.Add(teclado);
            pnlTeclado.Visible = false;
            this.teclado.clickBotao += new TeclaNumerico.ClicarNoBotao(DelegateTeclado);
            this.Text = "®SICEpdv.net versão: " + GlbVariaveis.glb_Versao;

            txtVersao.Text = "Versão: " + GlbVariaveis.glb_Versao;
            btnTecladoLogin.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg");
            btnTecladoSenha.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg");

            // Geração do QrCode do IQ Card

            Application.DoEvents();
            string iqcardSuporte = GlbVariaveis.iqcardsuporte;
            iqcardSuporteNome = GlbVariaveis.iqcardsuporteNome;
            string whatsappsuporte = GlbVariaveis.whatsappsuporte;

            
            if (string.IsNullOrEmpty(iqcardSuporte))
            {

                try
                {
                    ServiceReference2.WSClientesClient suporte = new ServiceReference2.WSClientesClient();
                    var sql = "SELECT idcliente FROM iqsistemas";
                    var id = Conexao.CriarEntidade().ExecuteStoreQuery<String>(sql).FirstOrDefault();
                   
                    var dados = suporte.DadosCliente(Convert.ToInt16(id));
                    iqcardSuporte = dados.iqcardsuporte;
                    GlbVariaveis.iqcardsuporte = iqcardSuporte;
                    GlbVariaveis.iqcardsuporteNome = dados.conjuge;
                    iqcardSuporteNome = dados.conjuge;
                    txtTecnicoIQCard.Text = iqcardSuporteNome;
                }
                catch(Exception ex)
                {                    
                    txtTecnicoIQCard.Text = "";
                    pnlQR.Visible = false;                   

                }
    
            }

            if (string.IsNullOrEmpty(iqcardSuporte))
            {
                var sql = "SELECT iqcardsuporte FROM iqsistemas";
                var iqcard = Conexao.CriarEntidade().ExecuteStoreQuery<String>(sql).FirstOrDefault();
                GlbVariaveis.iqcardsuporte = iqcard;
                iqcardSuporte = iqcard;                
            }

            

            if (!string.IsNullOrEmpty(iqcardSuporte))
            {
                QRCodeGenerator qr = new QRCodeGenerator();
                QRCodeData data = qr.CreateQrCode(iqcardSuporte + "chat", QRCodeGenerator.ECCLevel.Q);
                QRCode code = new QRCode(data);

                imgQrCodeSuporte.Image = code.GetGraphic(5);
                txtTecnicoIQCard.Text = iqcardSuporteNome;
                try
                {
                    string sql = "UPDATE iqsistemas SET iqcardsuporte='" + iqcardSuporte + "'";
                    Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                }
                catch (Exception)
                {                    
                }





            }



            // Whatsapp Suporte


            if (string.IsNullOrEmpty(whatsappsuporte))
            {

                try
                {
                    ServiceReference2.WSClientesClient suporte = new ServiceReference2.WSClientesClient();
                    var sql = "SELECT idcliente FROM iqsistemas";
                    var id = Conexao.CriarEntidade().ExecuteStoreQuery<String>(sql).FirstOrDefault();

                    var dados = suporte.DadosCliente(Convert.ToInt16(id));
                    GlbVariaveis.whatsappsuporte = dados.whatsappsuporte;
                    lblWhats.Text = Funcoes.FormatarTelefone(dados.whatsappsuporte);
                }
                catch (Exception ex)
                {

                    lblWhats.Text = "";
                }

            }

            if (!string.IsNullOrEmpty(whatsappsuporte))
            {
               
                try
                {
                    string sql = "UPDATE iqsistemas SET whatsappsuporte='" + whatsappsuporte + "'";
                    Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                }
                catch (Exception)
                {
                }

            }

            try
            {
                txtEmpresa.Text = GlbVariaveis.nomeEmpresa + " - Filial " + GlbVariaveis.glb_filial;
            }
            catch(Exception)
            {

            }

        }

        void DelegateTeclado(object sender, string text)
        {
           tecla = text;
           //DispararThread();
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
                case "txtSenha":
                    VerificaAutorizacao();
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

            if (ctls[0] is Button)
                return;

            TextBox txtBox = ctls[0] as TextBox;

            if (txtBox.Enabled == false)
                return;

            if (ctls[0] is TextBox)
            {
                if (txtBox.Text.Trim().Length >= txtBox.MaxLength)
                    return;
                txtBox.Text += this.tecla;
            };          
            
        }

        private void VerificaAutorizacao()
        {
            Operador.autorizado = false;            
            if (campo == "login")
            {                
                try
                {
                    // lblAguarde.Text = "Aguarde...";
                    Application.DoEvents();
                    lblAguarde.Refresh();

                    Operador operador = new Operador();
                    
                    operador.Login(txtOperador.Text, txtSenha.Text);
                   

                }
                catch (Exception erro)
                {
                    //progressBar1.Visible = false;
                    lblAguarde.Text = "";
                    txtSenha.Text = "";
                    MessageBox.Show(erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtOperador.Focus();
                   return;
                }
                this.Close();
                return;
            }

                  
            if (txtSenha.Text == "" && !txtOperador.Text.Contains("0359"))
            {
                Close();
                return;
            }


            if (!Operador.Autorizacao(txtOperador.Text, txtSenha.Text, campo))
            {
                MessageBox.Show("Operador não encontrado ou restrito para realizar operação !!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);                
            }
            else
            {                
                Close();
            }
                      
        }

        private void FrmLogon_Shown(object sender, EventArgs e)
        {
            
            timeVerificaRest.Enabled = true;
            MostrarSobre();
            txtOperador.Focus();
            
            if (campo != "login" && Conexao.onLine)
            {
                try
                {

                    siceEntities entidade = Conexao.CriarEntidade();
                    string nomeCliente = "";
                    string descricaoProduto = "";
                    if (idCliente > 0)
                    {
                        nomeCliente = (from n in entidade.clientes
                                       where n.Codigo == idCliente
                                       select n.Nome).First();
                    }

                    restricoes restricao = new restricoes();
                    restricao.operador = GlbVariaveis.glb_Usuario;
                    restricao.descricao = txtDescricao.Text.PadRight(100, ' ').Substring(0, 100);
                    restricao.codigocliente = idCliente;
                    restricao.nome = nomeCliente;
                    restricao.codigoproduto = codProduto;
                    restricao.produto = descricaoProduto;
                    restricao.codigofilial = GlbVariaveis.glb_filial;
                    restricao.liberada = "N";
                    restricao.ip = GlbVariaveis.glb_IP;
                    restricao.camporestricao = campo;
                    entidade.AddTorestricoes(restricao);
                    entidade.SaveChanges();
                    timeVerificaRest.Enabled = true;
                }
                catch (Exception erro)
                {
                    if (Conexao.tipoConexao == 1)
                        new Exception(erro.Message);
                }
            }
            
        }


        private void MostrarSobre()
        {
            //if (campo == "login") //  && !File.Exists("log_teste.txt")
            //{
                this.Height = 334;
                pnlLogin.Visible = true;
            //};
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {            
            for (int i = 0; i < 100; i++)
            {                
                Thread.Sleep(100);                
                backgroundWorker1.ReportProgress(i); //run in back thread                
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) //call back method
        {            
            //progressBar1.Value = e.ProgressPercentage;

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) //call back method
        {
            iniciarThead = false;
            lblAguarde.Text = "";
            //progressBar1.Visible = false;
            Application.DoEvents();

        }

        private void FrmLogon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }
        }

        private void FrmLogon_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (campo != "login" && Conexao.onLine && Conexao.tipoConexao == 1)
                {
                    siceEntities entidade = Conexao.CriarEntidade();

                    var apagar = (from n in entidade.restricoes
                                  where n.operador == GlbVariaveis.glb_Usuario
                                  && n.ip == GlbVariaveis.glb_IP
                                  select n).First();
                    entidade.DeleteObject(apagar);
                    entidade.SaveChanges();
                }
                timeVerificaRest.Enabled = false;
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }
        }

        private void timeVerificaRest_Tick(object sender, EventArgs e)
        {
            
            if (!Conexao.onLine || campo.ToLower() == "login")
            {
                timeVerificaRest.Enabled = false;
                return;
            }

            try
            {
                siceEntities entidade = Conexao.CriarEntidade();

                var restricao = from n in entidade.restricoes
                                where n.operador == GlbVariaveis.glb_Usuario
                                && n.liberada == "S"
                                select n;

                if (restricao.Count() > 0)
                {
                    Operador.autorizado = true;
                    Operador.ultimoOperadorAutorizado = entidade.ExecuteStoreQuery<string>("select if(operadorLiberado = '',operador,operadorLiberado) from restricoes where liberada = 'S'").FirstOrDefault(); //restricao.First().operador;
                    timeVerificaRest.Enabled = false;
                    Close();
                }
            }
            catch (Exception erro)
            {
                if (Conexao.tipoConexao == 1)
                    new Exception(erro.Message);
            }

        }

        private static void ChamarMenuFiscal()
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Carregando configurações do ECF");
            msg.Show();
            Application.DoEvents();

            try
            {
                ConfiguracoesECF.Carregar();
            }
            finally
            {
                msg.Dispose();
            }

            FrmMenuFiscal frmFiscal = new FrmMenuFiscal();
            frmFiscal.ShowDialog();
        }

        private void MostrarTeclado(int t)
        {

            label1.Visible = false;
            label7.Visible = false;
            label10.Visible = false;
            rating1.Visible = false;
            rating2.Visible = false;
            rating3.Visible = false;
            rating4.Visible = false;
            rating5.Visible = false;
            btnOcultarAvaliacao.Visible = false;
            txtAvaliacao.Visible = false;
            txtTecnicoIQCard.Visible = false;
            pnlTeclado.Visible = true;

            if (t == 1)
            {
                teclado1 = true;
                teclado2 = false;
                txtOperador.Focus();
                btnTecladoLogin.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-enable.jpg"); ;
                btnTecladoSenha.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg"); ;
            }
            else
            {
                teclado2 = true;
                teclado1 = false;
                txtSenha.Focus();
                btnTecladoLogin.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg"); ;
                btnTecladoSenha.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-enable.jpg"); ;
            }
            
        }

        private void EsconderTeclado(int t)
        {

            label1.Visible = true;
            label7.Visible = true;
            label10.Visible = true;
            rating1.Visible = true;
            rating2.Visible = true;
            rating3.Visible = true;
            rating4.Visible = true;
            rating5.Visible = true;
            btnOcultarAvaliacao.Visible = true;
            txtAvaliacao.Visible = true;
            txtTecnicoIQCard.Visible = true;
            pnlTeclado.Visible = false;

            if (t == 1)
            {
                teclado1 = false;
                btnTecladoLogin.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg"); ;
            }
            else
            {
                teclado2 = false;
                btnTecladoSenha.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg"); ;
            }

        }


        private void button3_Click(object sender, EventArgs e)
        {

            txtOperador.Focus();
        }
               
        private void pnlIQCard_Paint(object sender, PaintEventArgs e)
        {
          
        }

        private void pnlIQCard_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://iqcard.com.br");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://iqcard.com.br");
        }

        private void label5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://iqcard.com.br");
        }

        private void label6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://iqcard.com.br");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://play.google.com/store/apps/details?id=com.iqsistemas.iqcard");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TeclaEnter();
        }

        private void label7_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://play.google.com/store/apps/details?id=com.iqsistemas.iqcard");
        }

        private void panelTeclado1_Paint(object sender, PaintEventArgs e)
        {

            
        }

        private void btnTecladoLogin_Click(object sender, EventArgs e)
        {
            if(teclado1 == false)
            {
                MostrarTeclado(1);
            }
            else
            {
                EsconderTeclado(1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (teclado2 == false)
            {
                MostrarTeclado(2);
            }
            else
            {
                EsconderTeclado(2);
            }
        }

        private void FrmLogon_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void avaliacao()
        {

            try
            {

                var sql = "SELECT avaliacaotecnicoiqcard FROM iqsistemas;";
                int nota = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql).FirstOrDefault();

                var sql2 = "SELECT mostraravaliacao FROM iqsistemas;";
                String mostrar = Conexao.CriarEntidade().ExecuteStoreQuery<String>(sql2).FirstOrDefault();



                if (mostrar == "S")
                {                    
                    pnlAvaliacao.Visible = true;
                    btnOcultarAvaliacao.Visible = true;
                    btnMostrarEstrelas.Visible = false;
                    txtAvaliacao.Text = "Ocultar avaliação";
                    mudarEstrela(nota);

                }
                else
                {
                    btnMostrarEstrelas.Visible = true;
                    pnlAvaliacao.Visible = false;                    
                    btnOcultarAvaliacao.Visible = false;
                    btnMostrarEstrelas.Visible = true;                    
                    txtAvaliacao.Text = "Avalie o atendimento";
                }
            }
            catch (Exception)
            {
             
            }
           


        }


        private void rating1_Click(object sender, EventArgs e)
        {
            int estrela = 1;
            mudarEstrela(estrela);
            alterarAvaliacao(estrela);
        }

        private void rating2_Click(object sender, EventArgs e)
        {
            int estrela = 2;
            mudarEstrela(estrela);
            alterarAvaliacao(estrela);
        }

        private void rating3_Click(object sender, EventArgs e)
        {
            int estrela = 3;
            mudarEstrela(estrela);
            alterarAvaliacao(estrela);
        }

        private void rating4_Click(object sender, EventArgs e)
        {
            int estrela = 4;
            mudarEstrela(estrela);
            alterarAvaliacao(estrela);
        }

        private void rating5_Click(object sender, EventArgs e)
        {
            int estrela = 5;
            mudarEstrela(estrela);
            alterarAvaliacao(estrela);
        }

        
        private void mudarEstrela(int estrela)
        {
            try
            {

           

            if (estrela == 0)
            {
                rating2.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                rating2.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                rating3.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                rating4.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                rating5.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                txtAvaliacao.Text = "AVALIE O ATENDIMENTO";
            }
            if (estrela == 1)
            {
                rating1.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating2.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                rating3.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                rating4.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                rating5.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
            }
            else if (estrela == 2)
            {
                rating1.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating2.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating3.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                rating4.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                rating5.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
            }
            else if (estrela == 3)
            {
                rating1.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating2.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating3.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating4.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
                rating5.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
            }
            else if (estrela == 4)
            {
                rating1.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating2.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating3.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating4.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating5.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gray.png");
            }
            else if (estrela == 5)
            {
                rating1.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating2.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating3.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating4.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
                rating5.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\rating_gold.png");
            }
            else if (estrela == 6)
            {

                // btnOcultarAvaliacao2.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-times.png");
                txtAvaliacao.Text = "Olá, bom trabalho!";

                btnOcultarAvaliacao2.Visible = false;
                rating1.Visible = false;
                rating2.Visible = false;
                rating3.Visible = false;
                rating4.Visible = false;
                rating5.Visible = false;

            }
            }
            catch (Exception)
            {
                
            }

        }

        private void alterarAvaliacao(int estrela)
        {
            try
            {

                Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE iqsistemas SET avaliacaotecnicoiqcard = '" + estrela + "' ;");

                string estrelas = "";
                if (estrela > 1)
                {
                    estrelas = estrela + " estrelas.";
                }
                else
                {
                    estrelas = estrela + " estrela.";
                }

                try
                {
                    ServiceReference1.WSIQPassClient avaliar = new ServiceReference1.WSIQPassClient();
                    var avaliacao = avaliar.PegarAvaliacao(GlbVariaveis.chavePrivada, GlbVariaveis.iqcardsuporte, GlbVariaveis.iqcardsuporte);
                    avaliar.IncluirResposta(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, GlbVariaveis.iqcardsuporte, GlbVariaveis.iqcardsuporte, estrela, 0, 0, 0, 0);
                }
                catch (Exception ex)
                {

                
                }


                txtAvaliacao.Text = "Obrigado pela avaliação!";
                // MessageBox.Show("Você classificou o técnico " + iqcardSuporteNome + " com " + estrelas + " Obrigado!");
            }
            catch (Exception)
            {

            }
        }

        
        private void btnOcultarAvaliacao_Click(object sender, EventArgs e)
        {
            btnOcultarAvaliacao.Visible = false;
            btnMostrarEstrelas.Visible = true;
            Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE iqsistemas SET mostraravaliacao = 'N';");
            mudarEstrela(6);

            /*
            Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE iqsistemas SET mostraravaliacao = 'N';");
            mudarEstrela(6);*/


        }

        private void btnOcultarAvaliacao_Click_1(object sender, EventArgs e)
        {
            /*
            btnOcultarAvaliacao.Visible = false;
            btnMostrarEstrelas.Visible = true;
            Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE iqsistemas SET mostraravaliacao = 'N';");
            mudarEstrela(6); */

        }

        private void btnMostrarEstrelas_Click(object sender, EventArgs e)
        {
            btnOcultarAvaliacao.Visible = true;
            btnMostrarEstrelas.Visible = false;
            pnlAvaliacao.Visible = true;
            Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE iqsistemas SET mostraravaliacao = 'S';");
            avaliacao();
        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {

        }
    }
}

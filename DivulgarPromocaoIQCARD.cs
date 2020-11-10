using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class DivulgarPromocaoIQCARD : Form
    {
        TeclaNumerico teclado = new TeclaNumerico(Color.White);
        ServiceReference1.Promocao dadosPromocao = new ServiceReference1.Promocao();
        ServiceReference1.WSIQPassClient x = new ServiceReference1.WSIQPassClient();

        OpenFileDialog file = new OpenFileDialog();
        public bool teclado1 = false;
        public string iqcardSorteado = "";

        public static bool iniciarmostrandoPromocao = true;
        public static bool iniciarmostrandoPedido = true;

        public DivulgarPromocaoIQCARD()
        {
            InitializeComponent();
            pnlTeclado.Controls.Add(teclado);
            btnTeclado.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg");
            txtIQCARDFidelidade.Text = GlbVariaveis.glb_chaveIQCard;
            try
            {
                if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                {
                    ServiceReference2.WSClientesClient suporte = new ServiceReference2.WSClientesClient();                   
                    var dados = suporte.DadosCliente(Convert.ToInt16(GlbVariaveis.idCliente));
                    txtIQCARDFidelidade.Text = dados.iqcard;

                    if (!string.IsNullOrEmpty(txtIQCARDFidelidade.Text))
                    {
                        string sql = "UPDATE filiais SET tokeniqcard='" + txtIQCARDFidelidade.Text + "' WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                        Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                        GlbVariaveis.glb_chaveIQCard = txtIQCARDFidelidade.Text;
                    }
                }

                if (!string.IsNullOrEmpty(txtIQCARDFidelidade.Text))
                {
                    txtIQCARDFidelidade.Enabled = false;
                    btnComecar.Visible = false;
                }

            }
            catch (Exception)
            {
                
            }

            txtIQCARDFidelidade.Focus();

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            int dias = 0;
            try
            {
                if (cboDias.Text == "Hoje")
                {
                    dias = 0;
                    cboDias.Text = "0";
                }

                dias = Convert.ToInt16(cboDias.Text);

            }
            catch (Exception)
            {
                dias = 1;
            }

            if (string.IsNullOrEmpty(txtPreco.Text))
                txtPreco.Text = "0";

            ServiceReference1.Promocao promocao = new ServiceReference1.Promocao()
            {
                RowKey = "",
                data = DateTime.Now,
                id = GlbVariaveis.idCliente,
                idEmpresa = GlbVariaveis.glb_chaveIQCard,
                nomeEmpresa = GlbVariaveis.nomeEmpresa,
                cidade = Configuracoes.cidade,
                chamada = txtChamada.Text,
                dataIniciar = DateTime.Now.Date,
                dataFinalizar = DateTime.Now.Date.AddDays(dias),
                link = txtLink.Text,
                codigoItem = txtCodigo.Text,
                preco = Convert.ToDouble(txtPreco.Text)
            };

            if (dadosPromocao != null)
            {
                dadosPromocao.dataIniciar = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());
                dadosPromocao.dataFinalizar = DateTime.Now.Date.AddDays(dias);
            }

            if (dadosPromocao == null)
                dadosPromocao = promocao;


            AlterarPromocao();

        }

        private void AlterarPromocao()
        {
            try
            {


                FrmLogon Logon = new FrmLogon();
                Operador.autorizado = false;
                Logon.idCliente = 0;
                Logon.campo = "gerente";
                Logon.lblDescricao.Text = "CRIAR ANÚNCIO";
                Logon.txtDescricao.Text = "Confirme. É necessário ter a permissão de Gerente";
                Logon.ShowDialog();
                if (!Operador.autorizado)

                    return;


                dadosPromocao.chamada = txtChamada.Text;
                dadosPromocao.link = txtLink.Text;
                dadosPromocao.cidade = Configuracoes.cidade;
                dadosPromocao.nomeEmpresa = GlbVariaveis.nomeEmpresa;
                dadosPromocao.idEmpresa = GlbVariaveis.glb_chaveIQCard;
                dadosPromocao.codigoItem = txtCodigo.Text;
                try
                {
                    dadosPromocao.preco = Convert.ToDouble(txtPreco.Text);
                }
                catch (Exception)
                {

                }

                ServiceReference1.WSIQPassClient x = new ServiceReference1.WSIQPassClient();
                var dados = x.IncluirPromocao(GlbVariaveis.chavePrivada, dadosPromocao,false);


                if (!string.IsNullOrEmpty(dados))
                {
                    pnlChave.Visible = true;
                    lblInfoAlteracao.Text = "Anúncio atualizado";
                }

                if (dadosPromocao.dataIniciar.Date >= DateTime.Now.Date)
                {
                    MessageBox.Show("Seu anúncio pronto e sera divulgado no app IQCARD e nas redes sociais.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if(checkTodos.Checked==false && !string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                {
                    try
                    {
                        IqCard iqcard = new IqCard();
                        iqcard.ExcluirItensAplicativo(true,false);
                    }
                    catch (Exception)
                    {                        
                    }                    
                }

                if(checkTodos.Checked)
                {
                    var inicio = DateTime.Now;
                    FrmMsgOperador msg3 = new FrmMsgOperador("", "Atualizando itens da promoção. Pode demorar alguns instante.");
                    msg3.Show();
                    Application.DoEvents();
                    try
                    {
                        IqCard iqcard = new IqCard();
                        iqcard.ExcluirItensAplicativo(true,false);
                        iqcard.AtualizarItensDelivery(false, false,false,true,false);
                        MessageBox.Show("Itens da promoção enviados com sucesso. Início: " + inicio.ToString() + " Fim: " + DateTime.Now.ToString());
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
                MostrarPromocao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DivulgarPromocaoIQCARD_Load(object sender, EventArgs e)
        {
            //if(IqCard.usuariosProcurandoPromo>0)
            //{
            //    MessageBox.Show("Existe pessoas procurando ofertas. Divulgue suas promoções e nós vamos avisar a esses consumidores no canal adequado.");
            //}

            FrmMsgOperador msg = new FrmMsgOperador("", "ativando app");
            try
            {
                msg.Show();
                Application.DoEvents();

                try
                {
                    string tabela = "produtos";
                    if (GlbVariaveis.glb_filial != "00001")
                        tabela = "produtosfilial";

                    string sql = "SELECT COUNT(1) as quantidade FROM " + tabela + " WHERE situacao='Promoção' AND codigofilial='" + GlbVariaveis.glb_filial + "'";
                    int contadorPromocao = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql).FirstOrDefault();

                    if (contadorPromocao > 0)
                    {
                        lblContadorPromo.Text = contadorPromocao.ToString();
                        pnlTodos.Visible = false;
                    }

                    if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                    {
                        panel6.Visible = true;
                    }
                    else
                    {
                        if(!iniciarmostrandoPedido)
                        panel6.Visible = false;
                    }
                                               

                }
                catch (Exception)
                {
                    
                }
               

                    webBrowser1.DocumentText =
                    "<html>" +
                    "<head>" +
                    "<meta charset='utf-8' />" +
                    "<meta http-equiv = 'X-UA-Compatible' content='IE=edge'>" +
                    "<title>Page Title</title>" +
                    "<meta name='viewport' content = 'width=device-width, initial-scale=1'>" +
                    "<style> html, body, h2 { margin: 0; padding: 0; } </style>" +
                    "</head>" +
                    "<body style=' margin: 0; padding: 0; background' bgcolor='#004177'> " +
                    "<iframe width='100%' style='height:90vh;' src='https://www.youtube.com/embed/8HsinVpNEow' frameborder='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe>" +
                    "</body></html>";                

                           dadosPromocao = x.DadosPromocaoAnunciante(GlbVariaveis.chavePrivada, GlbVariaveis.idCliente, GlbVariaveis.glb_chaveIQCard);

                if (dadosPromocao == null)
                {
                    pnlChave.Visible = false;
                    btnSalvar.Text = "ANUNCIAR";
                }
                else
                {
                    pnlChave.Visible = true;
                    btnSalvar.Text = "SALVAR";
                }

                try
                {
                    if (dadosPromocao != null)
                    {
                        int participantes = x.ContadorParticipacaoPromocao(GlbVariaveis.chavePrivada, GlbVariaveis.idCliente.PadLeft(4,'0'), dadosPromocao.RowKey, 90);
                        if (participantes == 0)
                        {
                            lblUsuariosIqcard.Visible = false;
                        }
                        else
                        {
                            lblUsuariosIqcard.Visible = true;
                            lblUsuariosIqcard.Text = participantes.ToString() + " USUÁRIOS QUE LERAM CUPONS FISCAIS";
                        }
                    }
                }
                catch (Exception)
                {
                    msg.Dispose();

                    throw;
                }

                if (iniciarmostrandoPromocao)
                {
                    MostrarPromocao();                    
                }
               

            }
            catch (Exception)
            {
                MessageBox.Show("Verifique conexão com a internert");
            }
            finally
            {
                msg.Dispose();
            }

            try
            {
                string nomeArquivo = GlbVariaveis.glb_chaveIQCard + "_promocao1";
                if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                    nomeArquivo = GlbVariaveis.idCliente + "_promocao1";


                var request = WebRequest.Create("https://iqcard.blob.core.windows.net/logos/" + nomeArquivo);

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    imgPromocao.Image = Bitmap.FromStream(stream);
                }
               

            }
            catch (Exception)
            {

            }
            //Aqui mostra a publicao por que é a ação mais importante.
            if (iniciarmostrandoPedido)
            {
                chamarPainelIqCard("pedidos");
            }
            if (iniciarmostrandoPromocao)
            {
                chamarPainelIqCard("publicacao");
            }


        }

        private void MostrarPromocao()
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                txtCodigo_Leave(null, null);
            }

            if (dadosPromocao != null && txtCodigo.Text=="")
            {
                txtChamada.Text = dadosPromocao.chamada;
                txtLink.Text = dadosPromocao.link;
                int dias = Math.Abs(dadosPromocao.dataFinalizar.Date.Subtract(DateTime.Now.Date).Days);
                cboDias.Text = Convert.ToString(dias);

                txtCodigo.Text = dadosPromocao.codigoItem;
                txtPreco.Text = string.Format("{0:N2}", dadosPromocao.preco);

                if (dadosPromocao.preco > 0)
                    pnlPreco.Visible = true;

                if (dias == 0)
                {
                    cboDias.Text = "Hoje";
                    lblDuracao.Text = "";
                }
                else
                    lblDuracao.Text = "dia(s).";


                if (dadosPromocao.dataIniciar.Date >= DateTime.Now.Date)
                {
                    lblUsuariosIqcard.BackColor = System.Drawing.Color.Green;
                    // lblUsuariosIqcard.Text = "ANUNCIO ATIVO";
                    lblInfoPromocao.Text = "Ativo. Sua empresa está sinalizando que tem ofertas especiais hoje. ";
                    try
                    {
                        string sql = "UPDATE filiais SET validadepromocaoiqcard='" + string.Format("{0:yyyy/MM/dd}", dadosPromocao.dataFinalizar.Date) + "' WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                        Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                        Configuracoes.promocaoIQCardAtiva = true;
                    }
                    catch (Exception)
                    {
                    }

                }
                if (dadosPromocao.dataFinalizar.Date < DateTime.Now.Date)
                {
                    lblStatus.BackColor = System.Drawing.Color.Red;
                    lblStatus.Text = "";
                    try
                    {
                        string sql = "UPDATE filiais SET validadepromocaoiqcard=DATE_ADD(CURRENT_DATE, INTERVAL -1 DAY) WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                        Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                        Configuracoes.promocaoIQCardAtiva = false;
                    }
                    catch (Exception)
                    {
                    }

                }



            }

            // Aqui para mostrar a imagem caso o código tenha sido atribuido em produto. 
            //Por isso o txtCodigo esta como public
           
                
               

            lblInfoPromocao.Text = lblInfoPromocao.Text.ToUpper();
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {
            AlterarStatus();

        }

        private void AlterarStatus()
        {
            if (dadosPromocao.dataIniciar.Date >= DateTime.Now.Date)
            {


                if (MessageBox.Show("Deseja encerrar o anúncio?", "Atenção", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //Importante essa conversao de data por que senao na table grava sempre 22:00:00 a hora
                    dadosPromocao.dataIniciar = Convert.ToDateTime(DateTime.Now.Date.AddDays(-1).ToShortDateString());
                    dadosPromocao.dataFinalizar = Convert.ToDateTime(DateTime.Now.Date.AddDays(-1).ToShortDateString());
                    AlterarPromocao();
                    MostrarPromocao();
                    return;
                }
                return;

            }

            if (dadosPromocao.dataFinalizar.Date < DateTime.Now.Date)
            {


                if (MessageBox.Show("Deseja ativar o anúncio?", "Atenção", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    int dias = 0;
                    try
                    {
                        if (cboDias.Text == "Hoje")
                        {
                            dias = 0;
                            cboDias.Text = "0";
                        }

                        dias = Convert.ToInt16(cboDias.Text);

                    }
                    catch (Exception)
                    {
                        dias = 1;
                    }


                    //Importante essa conversao de data por que senao na table grava sempre 22:00:00 a hora
                    dadosPromocao.dataIniciar = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());
                    dadosPromocao.dataFinalizar = Convert.ToDateTime(DateTime.Now.Date.AddDays(dias).ToShortDateString());
                    AlterarPromocao();
                    MostrarPromocao();
                    return;
                }
                return;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://iqcard.com.br");
        }

        private void lblInfoPromocao_Click(object sender, EventArgs e)
        {
            AlterarStatus();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://play.google.com/store/apps/details?id=com.iqsistemas.iqcard");

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void lblDicas_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://iqcard.com.br/Home/DicasPromocao");
        }

        private void btnFecharSorteio_Click(object sender, EventArgs e)
        {
            pnlSorteio.Visible = false;
        }

        private void pnlContador_Click(object sender, EventArgs e)
        {
            LancarSorteio();


        }

        private void LancarSorteio()
        {
            try
            {
                var dados = x.SorteioParticipantePromocao(GlbVariaveis.chavePrivada,GlbVariaveis.idCliente.PadLeft(4,'0'), dadosPromocao.RowKey, 120);                
                lblIQCARD.Text = "IQCARD: " + IqCard.FormatarCartao(dados.idCartao);
                lblNome.Text = "NOME: " + dados.nome;
                lblTel.Text = "TEL.:" + dados.telefone;
                pnlSorteio.Visible = true;
                iqcardSorteado = dados.idCartao;
            }
            catch (Exception ex)
            {
                pnlSorteio.Visible = false;
                MessageBox.Show(ex.Message);
                return;
            }


            try
            {
                var dados = x.ContadorParticipacaoPromocao(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, dadosPromocao.RowKey, 90);
                lblPartic.Text = dados.ToString();
                pnlSorteio.Visible = true;
            }
            catch (Exception ex)
            {
                lblPartic.Text = "0";

            }
        }

        private void lblContadorPar_Click(object sender, EventArgs e)
        {
            LancarSorteio();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            escolherImagem();
        }

        public void escolherImagem()
        {
            file.InitialDirectory = @"C:\iqsistemas\SICEpdv\galeriapromocao";
            file.Filter = "Image Files (JPG)|*.JPG";


            if (file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] buffer = File.ReadAllBytes(file.FileName);


                    long tamanhoArquivo = new System.IO.FileInfo(file.FileName).Length;


                    if (tamanhoArquivo > 40000 && tamanhoArquivo < 100000)
                    {
                        VaryQualityLevel();
                        buffer = File.ReadAllBytes(@"promocaoimagepequena.jpg");
                    }


                    if (tamanhoArquivo >= 100000)
                    {
                        VaryQualityLevel2();
                        buffer = File.ReadAllBytes(@"promocaoimagepequena.jpg");
                    }

                    IqCard iqcard = new IqCard();
                    iqcard.UploadFile(buffer, dadosPromocao.RowKey);
                    imgPromocao.Image = new Bitmap(file.FileName);

                    try
                    {
                        byte[] bufferAnuncio = File.ReadAllBytes(file.FileName);
                        DriveCloud cloud = new DriveCloud();
                        if (cloud.UploadFile(buffer,"anuncio"+GlbVariaveis.idCliente, "Anuncio",Convert.ToInt16(GlbVariaveis.idCliente), GlbVariaveis.nomeEmpresa,"",dadosPromocao.RowKey))
                        {
                            MessageBox.Show("Imagem atualizada");
                        }

                    }
                    catch (Exception)
                    {
                      
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Escolha imagem .jpeg com no máximo 100KB");
                }

            }
        }

        private void lblEscolher_Click(object sender, EventArgs e)
        {
            pictureBox1_Click(sender, e);
        }

        private void VaryQualityLevel()
        {
            // Get a bitmap.
            Bitmap bmp1 = new Bitmap(file.FileName);
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder,
                50L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp1.Save("promocaoimagepequena.jpg", jgpEncoder,
                myEncoderParameters);

            //myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            //myEncoderParameters.Param[0] = myEncoderParameter;
            //bmp1.Save(@"c:\TestPhotoQualityHundred.jpg", jgpEncoder,
            //    myEncoderParameters);

            //// Save the bitmap as a JPG file with zero quality level compression.
            //myEncoderParameter = new EncoderParameter(myEncoder, 0L);
            //myEncoderParameters.Param[0] = myEncoderParameter;
            //bmp1.Save(@"c:\TestPhotoQualityZero.jpg", jgpEncoder,
            //    myEncoderParameters);

        }


        private void VaryQualityLevel2()
        {
            // Get a bitmap.
            Bitmap bmp1 = new Bitmap(file.FileName);
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder,
                3L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp1.Save("promocaoimagepequena.jpg", jgpEncoder,
                myEncoderParameters);

            //myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            //myEncoderParameters.Param[0] = myEncoderParameter;
            //bmp1.Save(@"c:\TestPhotoQualityHundred.jpg", jgpEncoder,
            //    myEncoderParameters);

            //// Save the bitmap as a JPG file with zero quality level compression.
            //myEncoderParameter = new EncoderParameter(myEncoder, 0L);
            //myEncoderParameters.Param[0] = myEncoderParameter;
            //bmp1.Save(@"c:\TestPhotoQualityZero.jpg", jgpEncoder,
            //    myEncoderParameters);

        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void imgApagar_Click(object sender, EventArgs e)
        {
            removerImagem();
        }

        public void removerImagem()
        {
            if (MessageBox.Show("Deseja apagar a imagem?", "Atenção", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            IqCard iqcard = new IqCard();
            try
            {
                iqcard.ApagarArquivo();
                imgPromocao.Image = Properties.Resources.upload;
                txtCodigo.Text = "";
                txtPreco.Text = "0";
                pnlPreco.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtCodigo.Text))
                return;


            FrmMsgOperador msg = new FrmMsgOperador("Um instante", "Formando anúncio e procurando imagens.");
            try
            {
                msg.Show();
                Application.DoEvents();
                Produtos produto = new Produtos();

                try
                {
                    var dadosProduto = produto.ProcurarCodigo(txtCodigo.Text, GlbVariaveis.glb_filial, false);
                    txtChamada.Text = produto.descricao;                    
                    txtPreco.Text = string.Format("{0:n2}", produto.preco);
                }
                catch (Exception)
                {

                }

                if (!string.IsNullOrEmpty(produto.codigoBarras))
                {
                    txtCodigo.Text = produto.codigoBarras;
                    pnlPreco.Visible = true;
                }


                try
                {
                    var dadosImagem = IqCard.DadosImagem(txtCodigo.Text);
                    if (dadosImagem != null)
                    {
                        imgPromocao.ImageLocation = dadosImagem.urlImage1;
                        txtChamada.Text = dadosImagem.descricao;
                        pnlPreco.Visible = true;

                        if (!Directory.Exists(@Application.StartupPath + @"\imagensprodutos"))
                        {
                            Directory.CreateDirectory(@Application.StartupPath + @"\imagensprodutos");
                        }
                        imgPromocao.Image.Save(@Application.StartupPath + @"\imagensprodutos\" + txtCodigo.Text + ".jpg");

                    }
                }
                catch (Exception)
                {

                }
            }
            catch (Exception)
            {

            }
            finally
            {
                msg.Dispose();
            }


        }


        private void ChamarProdutos(bool consultarPreco)
        {
            FrmProdutos frmprd = new FrmProdutos();
            FrmProdutos.consultarPreco = consultarPreco;

            if (consultarPreco)
                frmprd.rdbBarras.Checked = true;

            frmprd.ShowDialog();            
            txtCodigo.Text = FrmProdutos.ultCodigo;
            txtChamada.Text = FrmProdutos.ultDescricao;
            btnBuscar_Click(null, null);            
        }

        private void DivulgarPromocaoIQCARD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
                ChamarProdutos(false);

            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnBuscar_Click(sender, null);
            }

        }



        private void txtPreco_KeyPress(object sender, KeyPressEventArgs e)
        {
            Funcoes.DigitarNumerosPositivos(sender, e);
        }

        private void txtPreco_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtPreco.Text = Funcoes.FormatarDecimal(txtPreco.Text);
            }
        }

        private void txtPreco_Leave(object sender, EventArgs e)
        {
            txtPreco.Text = Funcoes.FormatarDecimal(txtPreco.Text);
        }

        private void checkTodos_CheckedChanged(object sender, EventArgs e)
        {
            if(checkTodos.Checked)
            {
                if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                {
                    MessageBox.Show("Para anunciar todos os seus itens no aplicativo é necessário você ter uma conta IQCARD. Clique em OK e crie sua conta");
                    checkTodos.Checked = false;
                    TokenIQCARD token = new TokenIQCARD();
                    token.ShowDialog();
                }
            };
        }

        private void lblTodos_Click(object sender, EventArgs e)
        {
            checkTodos.Checked = true;
        }

        private void pnlTodos_Click(object sender, EventArgs e)
        {
            checkTodos.Checked = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            LancarSorteio();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (teclado1 != false)
            {
                pnlTeclado.Visible = false;
                teclado1 = false;
                btnTeclado.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg"); ;

            }
            else
            {
                pnlTeclado.Visible = true;
                teclado1 = true;
                btnTeclado.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-enable.jpg"); ;
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            ChamarProdutos(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            escolherImagem();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            removerImagem();
        }



        /** Método que realiza a troca do conteúdo de acordo com o item clicado no menu superior */
        public void chamarPainelIqCard(String tela)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            
            /** Item selecionado */
            if (tela == "fidelize")
            {
                pnlFidelize.Width = 865;
                pnlFidelize.Height = 822;
                pnlFidelize.Visible = true;

                lblUsuariosIqcard.Text = IqCard.usuariosProcurandoPromo.ToString() + " USUÁRIOS ONLINE";
                toolTip1.SetToolTip(this.lblUsuariosIqcard, "Número de usuários que usaram o aplicativo recentemente.");
                
                // Demais itens
                pnlPublicacao.Visible = false;
                panel3.Visible = false;

            }
            /** Item selecionado */
            else if (tela == "publicacao")
            {
                if (dadosPromocao == null)
                {
                    MostrarPromocao();
                }
                pnlPublicacao.Width = 865;
                pnlPublicacao.Height = 822;
                pnlPublicacao.Visible = true;
                toolTip1.SetToolTip(this.lblUsuariosIqcard, "");

                // Demais itens
                pnlFidelize.Visible = false;
                panel3.Visible = false;
            }
            /** Item selecionado */
            if (tela == "sorteio")
            {
                panel3.Width = 865;
                panel3.Height = 822;
                panel3.Visible = true;

                try
                {
                    if (dadosPromocao != null)
                    {
                        int participantes = x.ContadorParticipacaoPromocao(GlbVariaveis.chavePrivada, GlbVariaveis.idCliente.PadLeft(4, '0'), dadosPromocao.RowKey, 90);
                        if (participantes == 0)
                        {
                            lblUsuariosIqcard.Visible = false;
                        }
                        else
                        {
                            lblUsuariosIqcard.Visible = true;
                            lblUsuariosIqcard.Text = participantes.ToString() + " USUÁRIOS QUE LERAM CUPONS FISCAIS";
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                // Demais itens
                pnlFidelize.Visible = false;
                pnlPublicacao.Visible = false;
            }
            /** Item selecionado */
            if (tela == "pedidos")
            {

                

                if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                {
                    MessageBox.Show("Crie sua conta IQCARD para começar com a loja virtual.");
                    chamarPainelIqCard("fidelize");
                    //MessageBox.Show("Crie sua conta no IQCARD e comece a fidelizar e interagir com mais consumidores.");
                    //TokenIQCARD token = new TokenIQCARD();
                    //token.ShowDialog();
                    return;
                }
                try
                {
                    IndexPedidoIQCard.iniciarmostrandoPedido = iniciarmostrandoPedido;
                    IndexPedidoIQCard ped = new IndexPedidoIQCard();
                    ped.ShowDialog();
                    Close();
                }
                catch (Exception)
                {
                    
                }               
                return;                
            }
        }

        private void label22_Click(object sender, EventArgs e)
        {
            chamarPainelIqCard("fidelize");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            chamarPainelIqCard("fidelize");
        }

        private void label23_Click(object sender, EventArgs e)
        {
            chamarPainelIqCard("publicacao");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            chamarPainelIqCard("publicacao");
        }

        private void label24_Click(object sender, EventArgs e)
        {
            chamarPainelIqCard("sorteio");
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            chamarPainelIqCard("sorteio");
        }

        private void label25_Click(object sender, EventArgs e)
        {
            chamarPainelIqCard("pedidos");
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            chamarPainelIqCard("pedidos");
        }

        private void btnSorteio_Click(object sender, EventArgs e)
        {
            LancarSorteio();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtIQCARDFidelidade.Text))
                    try
                    {
                        IqCard iqcard = new IqCard();
                        var dadosEmpresa = iqcard.DadosEmpresa(txtIQCARDFidelidade.Text);
                        if (dadosEmpresa == null)
                        {
                            throw new Exception("IQCARD inválido");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }

                if (GlbVariaveis.glb_chaveIQCard == txtIQCARDFidelidade.Text)
                    return;


                FrmLogon Logon = new FrmLogon();
                Operador.autorizado = false;
                Logon.idCliente = 0;
                Logon.campo = "gerente";
                Logon.lblDescricao.Text = "CRIAR CONTA IQCARD";
                Logon.txtDescricao.Text = "Digite a senha para confirmar. É necessário ter a permissão de Gerente";
                Logon.ShowDialog();
                if (!Operador.autorizado)
                    return;

                if (string.IsNullOrEmpty(txtIQCARDFidelidade.Text) && !string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                {
                    if (MessageBox.Show("Deseja desativar IQCARD e loja virtual", "SICEpdv.net", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return;
                    }

                    IqCard iqcard = new IqCard();
                    iqcard.ExcluirItensAplicativo(false, true);

                    string sqlDesativar = "UPDATE filiais SET tokeniqcard='" + txtIQCARDFidelidade.Text + "' WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                    Conexao.CriarEntidade().ExecuteStoreCommand(sqlDesativar);
                    GlbVariaveis.glb_chaveIQCard = txtIQCARDFidelidade.Text;
                    MessageBox.Show("IQCARD e Loja Virtual desativado com sucesso");
                    return;
                }


                ServiceReference1.WSIQPassClient card = new ServiceReference1.WSIQPassClient();
                FrmMsgOperador msg3 = new FrmMsgOperador("", "Aguardando confirmação pelo usuário");
                msg3.Show();
                Application.DoEvents();

                ServiceReference1.Resgate autorizacao = new ServiceReference1.Resgate()
                    {
                        idCartao = txtIQCARDFidelidade.Text,
                        idEmpresa = GlbVariaveis.glb_chaveIQCard,
                        mensagem = GlbVariaveis.nomeEmpresa + " Solicita autorização para ativar o IQCARD",
                        resgateConfirmado = false,
                        pontoResgate = 0,
                        idBrinde = ""
                    };
                    string idAutorizacao = card.IncluirResgate(GlbVariaveis.chavePrivada, autorizacao);
                    int tentativas = 0;
                    while (true)
                    {

                        if (tentativas > 20)
                        {
                            MessageBox.Show("Transação não foi permitida. É necessário que o usuário tenha conexão a internet e esteja com o aplicativo IQCARD instalado.");
                            msg3.Dispose();
                             return;                            
                        }

                        if (card.VerificarAutorizacao(GlbVariaveis.chavePrivada, idAutorizacao, txtIQCARDFidelidade.Text))
                        {
                            break;
                        }

                        Thread.Sleep(2000);
                        tentativas++;
                    }

                if (!string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard) && GlbVariaveis.glb_chaveIQCard != txtIQCARDFidelidade.Text)
                {
                    IqCard iqcard = new IqCard();
                    iqcard.ExcluirItensAplicativo(false, true);
                }
                msg3.Dispose();
                string sql = "UPDATE filiais SET tokeniqcard='" + txtIQCARDFidelidade.Text + "' WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                GlbVariaveis.glb_chaveIQCard = txtIQCARDFidelidade.Text;                

                if (string.IsNullOrEmpty(txtIQCARDFidelidade.Text) && !string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                {
                    if (MessageBox.Show("Salvo com sucesso. Deseja ativar sua loja virtual", "SICEpdv.net", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        chamarPainelIqCard("pedidos");
                    }
                }

            }
            catch (Exception)
            {                
                MessageBox.Show("Erro ao salvar");
            }

        }

        private void txtCodigo_Enter(object sender, EventArgs e)
        {
            
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                ChamarProdutos(false);
                return;
            }                

            btnBuscar_Click(null, null);
        }

        private void pnl25_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
            {
                MessageBox.Show("Crie sua conta IQCARD para poder desfrutar do serviço. Obrigado!");
                chamarPainelIqCard("fidelize");
                return;
            }

            PagamentoOnLine.linkPagSeguro = "https://iqcard.com.br/Empresa/Comprar/" + GlbVariaveis.glb_chaveIQCard+iqcardSorteado+"cartaopresente?valor=25.00";
            PagamentoOnLine.linkPayPal = "https://iqcard.com.br/Empresa/Comprar/" + GlbVariaveis.glb_chaveIQCard + iqcardSorteado + "cartaopresentepaypal?valor=25.00";
            PagamentoOnLine.cartaoPresente = true;
            PagamentoOnLine pag = new PagamentoOnLine();
            pag.ShowDialog();
        }

        private void pnl50_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
            {
                MessageBox.Show("Crie sua conta IQCARD para poder desfrutar do serviço. Obrigado!");
                chamarPainelIqCard("fidelize");
                return;
            }

            PagamentoOnLine.linkPagSeguro = "https://iqcard.com.br/Empresa/Comprar/" + GlbVariaveis.glb_chaveIQCard + iqcardSorteado + "cartaopresente?valor=50.00";
            PagamentoOnLine.linkPayPal = "https://iqcard.com.br/Empresa/Comprar/" + GlbVariaveis.glb_chaveIQCard + iqcardSorteado + "cartaopresentepaypal?valor=50.00";
            PagamentoOnLine.cartaoPresente = true;
            PagamentoOnLine pag = new PagamentoOnLine();
            pag.ShowDialog();
        }

        private void pnl100_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
            {
                MessageBox.Show("Crie sua conta IQCARD para poder desfrutar do serviço. Obrigado!");
                chamarPainelIqCard("fidelize");
                return;
            }

            PagamentoOnLine.linkPagSeguro = "https://iqcard.com.br/Empresa/Comprar/" + GlbVariaveis.glb_chaveIQCard + iqcardSorteado + "cartaopresente?valor=100.00";
            PagamentoOnLine.linkPayPal = "https://iqcard.com.br/Empresa/Comprar/" + GlbVariaveis.glb_chaveIQCard + iqcardSorteado + "cartaopresentepaypal?valor=100.00";
            PagamentoOnLine.cartaoPresente = true;
            PagamentoOnLine pag = new PagamentoOnLine();
            pag.ShowDialog();
        }

        private void DivulgarPromocaoIQCARD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }
        }

        

        private void pnlIQCard_Click_1(object sender, EventArgs e)
        {
            IndexPontosAcumulados   pnliqcard = new IndexPontosAcumulados();
            pnliqcard.ShowDialog();
        }
    }
    }


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
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class UplodImagemProduto : Form
    {
        public static string codigoBarrasPrd = "";
        public static string codigoPrd = "";
        public static string descricaoPrd = "";
        OpenFileDialog file = new OpenFileDialog();
        public UplodImagemProduto()
        {
            InitializeComponent();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UplodImagemProduto_Load(object sender, EventArgs e)
        {
            /*try
            {
                lblProduto.Text = codigoBarrasPrd + " " + descricaoPrd;
                if (!string.IsNullOrEmpty(codigoBarrasPrd))
                {
                    string arquivoImagem = @Application.StartupPath + @"\imagensprodutos\" + codigoBarrasPrd + ".jpg";

                    if (!File.Exists(arquivoImagem))
                    {
                        arquivoImagem = @Application.StartupPath + @"\imagensprodutos\" + codigoBarrasPrd + ".jpg";
                    }

                    if (File.Exists(arquivoImagem))
                    {
                        fotoPrd.Visible = true;
                        fotoPrd.Image = Image.FromFile(arquivoImagem);
                    }
                    else
                    {
                        if (codigoBarrasPrd.Length >= 8)
                        {
                            fotoPrd_Click(sender, e);
                        }
                    }
                   
                }
            }
            catch (Exception)
            {

            }*/


            try
            {
                string SQL = "select urlImagem from "+ GlbVariaveis.glb_estoque + " where codigo ='" + codigoPrd + "'";
                string url = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                fotoPrd.Visible = true;
                fotoPrd.ImageLocation = url;
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }
        }

        private void btnEscolher_Click(object sender, EventArgs e)
        {
            EscolherImagem();
        }

        public void EscolherImagem()
        {
            if (!Permissoes.administrador)
            {
                FrmLogon Logon = new FrmLogon();
                Operador.autorizado = false;
                Logon.campo = "estcad";
                Logon.lblDescricao.Text = "Excluir imagem: ";
                Logon.txtDescricao.Text = "Permissão alterar cadastro item: ";
                Logon.ShowDialog();

                if (!Operador.autorizado) return;
            }
            file.InitialDirectory = @"C:\iqsistemas\SICEpdv\galeriapromocao";
            file.Filter = "Image Files (JPG)|*.JPG";


            if (file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] buffer = File.ReadAllBytes(file.FileName);


                    long tamanhoArquivo = new System.IO.FileInfo(file.FileName).Length;


                    if (tamanhoArquivo > 100000 && tamanhoArquivo < 1000000)
                    {
                        VaryQualityLevel();
                        buffer = File.ReadAllBytes(@"imagepequena.jpg");
                    }


                    if (tamanhoArquivo >= 500000)
                    {
                        VaryQualityLevel2();
                        buffer = File.ReadAllBytes(@"imagepequena.jpg");
                    }

                    try
                    {
                        string codigoUpload = codigoBarrasPrd.Trim();

                        if (string.IsNullOrEmpty(codigoBarrasPrd) || codigoUpload.Length!=13)
                        {
                            codigoUpload = codigoPrd+GlbVariaveis.idCliente+GlbVariaveis.glb_filial;
                        }

                            IqCard iqcard = new IqCard();
                            iqcard.CadastrarImagem(codigoUpload, descricaoPrd);                                               

                            iqcard.UploadFile(buffer, codigoUpload, "imagens", codigoUpload + "_image1");
                        try
                        {
                          
                            try
                            {
                                string sqlApagar = "DELETE FROM produtosimagens WHERE codigoprd='" + codigoPrd + "' AND codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1";
                                Conexao.CriarEntidade().ExecuteStoreCommand(sqlApagar);

                            }
                            catch (Exception)
                            {
                                
                            }

                            siceEntities entidade = Conexao.CriarEntidade();
                            produtosimagens imagem = new produtosimagens()
                            {
                                codigofilial = GlbVariaveis.glb_filial,
                                codigoprod = codigoPrd,
                                imagem = buffer,

                            };

                            entidade.AddToprodutosimagens(imagem);
                            entidade.SaveChanges();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Erro ao salvar imagem no banco de dados!");
                        }

                        try
                            {
                            string urlImagem = @"https://iqcard.blob.core.windows.net/imagens/" + codigoUpload + "_image1";
                            string sql = "UPDATE produtos SET urlImagem='" + urlImagem + "' WHERE codigo='" + codigoPrd + "' OR codigobarras='"+codigoBarrasPrd+"'";
                            Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                            sql = "UPDATE produtosfilial SET urlImagem='" + urlImagem + "' WHERE codigo='" + codigoPrd + "' OR codigobarras='" + codigoBarrasPrd + "'";
                            Conexao.CriarEntidade().ExecuteStoreCommand(sql);

                            try
                            {
                                fotoPrd.Image = new Bitmap(file.FileName);
                                fotoPrd.ImageLocation = urlImagem;
                            }
                            catch (Exception)
                            {
                                fotoPrd_Click(null, null);                                
                            }
                            
                            }
                            catch (Exception)
                            {
                                
                            }

                            /*try
                            {
                                fotoPrd.Image.Save(@Application.StartupPath + @"\imagensprodutos\" + codigoBarrasPrd + ".jpg");
                            }
                            catch (Exception)
                            {
                                
                            }*/

                        //fotoPrd.Image = new Bitmap(file.FileName);

                        MessageBox.Show("Imagem salva");                       
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                   

                    //try
                    //{
                    //    byte[] bufferAnuncio = File.ReadAllBytes(file.FileName);
                    //    DriveCloud cloud = new DriveCloud();
                    //    if (cloud.UploadFile(buffer, "anuncio" + GlbVariaveis.idCliente, "Anuncio", Convert.ToInt16(GlbVariaveis.idCliente), GlbVariaveis.nomeEmpresa, "", dadosPromocao.RowKey))
                    //    {
                    //        MessageBox.Show("Imagem atualizada");
                    //    }

                    //}
                    //catch (Exception)
                    //{

                    //}

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Escolha imagem .jpeg com no máximo 100KB");
                }

            }
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

        private void fotoPrd_Click(object sender, EventArgs e)
        {
            /*string nomeArquivo = IqCard.UrlImagem(codigoBarrasPrd);
            if (string.IsNullOrEmpty(nomeArquivo))
                return;

            DownloadImagem(nomeArquivo);*/

        }

        private void DownloadImagem(string nomeArquivo)
        {
            try
            {
                var imagemBanco = (from n in Conexao.CriarEntidade().produtosimagens where n.codigoprod == codigoPrd && n.codigofilial == GlbVariaveis.glb_filial select n.imagem).FirstOrDefault();
                if (imagemBanco != null)
                {
                    TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                    Bitmap bitmap = (Bitmap)tc.ConvertFrom(imagemBanco);
                    fotoPrd.Image = bitmap;
                    return;
                }
            }
            catch (Exception)
            {
                
            }
            

            var request = WebRequest.Create(nomeArquivo);
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                fotoPrd.Image = Bitmap.FromStream(stream);
            }
            try
            {
                File.Delete(@Application.StartupPath + @"\imagensprodutos\" + codigoBarrasPrd + ".jpg");
                fotoPrd.Image.Save(@Application.StartupPath + @"\imagensprodutos\" + codigoBarrasPrd + ".jpg");
            }
            catch (Exception)
            {
                
            }
            
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            EscolherImagem();
        }

        private void btnEnviar_Click_1(object sender, EventArgs e)
        {
            fotoPrd_Click(sender, e);
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            FrmLogon Logon = new FrmLogon();
            Operador.autorizado = false;
            Logon.campo = "estcad";
            Logon.lblDescricao.Text = "Excluir imagem: ";
            Logon.txtDescricao.Text = "Permissão alterar cadastro item: ";
            Logon.ShowDialog();

            if (!Operador.autorizado) return;


            if (MessageBox.Show("Apagar a imagem do produto ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    File.Delete(@Application.StartupPath + @"\imagensprodutos\" + codigoBarrasPrd + ".jpg");

                    string sql = "UPDATE produtosfilial SET urlImagem='' WHERE codigo='" + codigoPrd + "'";
                    Conexao.CriarEntidade().ExecuteStoreCommand(sql);

                    sql = "UPDATE produtos SET urlImagem='' WHERE codigo='" + codigoPrd + "'";
                    Conexao.CriarEntidade().ExecuteStoreCommand(sql);


                    sql = "DELETE FROM produtosimagens WHERE codigoprod='" + codigoPrd + "' LIMIT 1";
                    Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                    fotoPrd.Image = null;


                    try
                    {
                        string codigoUpload = codigoBarrasPrd.Trim();
                        if (string.IsNullOrEmpty(codigoBarrasPrd) || codigoUpload.Length != 13)
                        {
                            codigoUpload = codigoPrd + GlbVariaveis.idCliente + GlbVariaveis.glb_filial;
                            IqCard iqcard = new IqCard();
                            iqcard.ApagarArquivo("imagens", codigoUpload + "_image1",codigoUpload);
                        }
                        
                    }
                    catch (Exception)
                    {
                        
                    }

                }
                catch (Exception)
                {

                }

            }
           
            

        }
    }
}

using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class FrmAnuncio : Form
    {

        List<listPesquisaProdutos> produtos = new List<listPesquisaProdutos>();
        int metadeDaTela = 0;
        int nvezes = 0;
        public static bool atualizarPromocoesIQCARD = true;
        public FrmAnuncio()
        {
            InitializeComponent();
            try
            {
                metadeDaTela = (Screen.PrimaryScreen.Bounds.Width / 2);

                panel18.Width = metadeDaTela;
                panel5.Width = metadeDaTela;
                panel5.BackColor = Color.Transparent;
                panel5.BackgroundImage = new Bitmap(@"imagensMetro\blue_1920x1080.png");
                fotoPrd.Dock = DockStyle.Left;
                panel1.Width = metadeDaTela;
                panel9.Width = metadeDaTela;
                var totalProdutos = 0;

                if (metadeDaTela < 600)
                {
                    txtDescricaoProduto.Font = new Font("Arial", 20, FontStyle.Bold);
                    lblOferta.Font = new Font("Arial", 20, FontStyle.Bold);
                    txtPrecoPromocao.Font = new Font("Arial", 50, FontStyle.Bold);
                    pnlPreco.BackgroundImage = new Bitmap(@"imagensMetro\preco_800x600.png");
                    pnlPreco.BackgroundImageLayout = ImageLayout.Stretch;
                    panel8.Height = 0;
                    panel18.BackgroundImageLayout = ImageLayout.Center;
                    panel18.Width = metadeDaTela;
                    panel9.Width = metadeDaTela;
                    panel1.Width = metadeDaTela;

                    txtDescricaoProduto.Width = 400;

                    fotoPrd.Dock = DockStyle.Right;
                }
                else
                {
                    pnlPreco.BackgroundImage = new Bitmap(@"imagensMetro\preco_541x303.png");
                }

                panel16.Height = (Screen.PrimaryScreen.Bounds.Height / 6);
            }
            catch(Exception)
            {
                panel5.BackgroundImage = new Bitmap(@"imagensMetro\background.jpg");
            }

        }

        private void carregarDados()
        {
            try
            {
                if(!Directory.Exists(@Application.StartupPath + @"\imagensprodutos\"))
                {
                    Directory.CreateDirectory(@Application.StartupPath + @"\imagensprodutos\");
                }


                var pesq = "";
                lblOferta.Text = "APENAS";


                pesq = @"SELECT codigo, descricao, precovenda as preco, codigobarras,localestoque, situacao, impulsionarvendas FROM produtos AS produto WHERE produto.codigofilial='" + GlbVariaveis.glb_filial + "' AND produto.situacao<>'Inativo' AND produto.situacao<>'Excluído' AND produto.situacao = 'Promoção' UNION " +
                        "(SELECT codigo, descricao, precovenda as preco, codigobarras,localestoque, situacao, impulsionarvendas FROM produtos AS produto WHERE produto.codigofilial='" + GlbVariaveis.glb_filial + "' AND produto.situacao<>'Inativo' AND produto.situacao<>'Excluído' AND produto.impulsionarvendas = 'S' AND validadeimpulso >= CURDATE() AND quantidadeimpulso > 0  ORDER BY RAND() limit 20);";

                if (GlbVariaveis.glb_filial != "00001")
                {

                    pesq = @"SELECT codigo, descricao, precovenda as preco, codigobarras,localestoque, situacao, impulsionarvendas FROM produtosfilial AS produto WHERE produto.codigofilial='" + GlbVariaveis.glb_filial + "' AND produto.situacao<>'Inativo' AND produto.situacao<>'Excluído' AND produto.situacao = 'Promoção' UNION " +
                            "(SELECT codigo, descricao, precovenda as preco, codigobarras,localestoque, situacao, impulsionarvendas FROM produtosfilial AS produto WHERE produto.codigofilial='" + GlbVariaveis.glb_filial + "' AND produto.situacao<>'Inativo' AND produto.situacao<>'Excluído' AND produto.impulsionarvendas = 'S' AND validadeimpulso >= CURDATE() AND quantidadeimpulso > 0  ORDER BY RAND() limit 20);";
                }

                var entidade = Conexao.CriarEntidade();
                produtos = entidade.ExecuteStoreQuery<listPesquisaProdutos>(pesq).ToList();

                var promocao = false;

                foreach(var i in produtos)
                {
                    if(i.situacao == "Promoção")
                    {
                        promocao = true;
                    }
                }


                if (produtos.Count==0 || !promocao)
                {
                    lblOferta.Text = "APENAS";
                    pesq = @"SELECT codigo, descricao, precovenda as preco, codigobarras,localestoque, situacao, impulsionarvendas FROM produtos AS produto WHERE produto.codigofilial='" + GlbVariaveis.glb_filial + "' AND produto.situacao<>'Inativo' AND produto.situacao<>'Excluído' AND produto.impulsionarvendas = 'S' AND validadeimpulso >= CURDATE() AND quantidadeimpulso > 0 UNION " + // AND produto.impulsionarvendasvalidade < CURDATE() AND produto.impulsionarvendasqtd > 0
                           "(SELECT codigo, descricao, precovenda as preco, codigobarras,localestoque, situacao, impulsionarvendas FROM produtos AS produto WHERE produto.codigofilial='" + GlbVariaveis.glb_filial + "' AND produto.situacao<>'Inativo' AND produto.situacao<>'Excluído' AND produto.dataultvenda> DATE_ADD(CURDATE(), INTERVAL -3 DAY)  ORDER BY RAND() limit 20);";

                    if (GlbVariaveis.glb_filial != "00001")
                    {
                        pesq = @"SELECT codigo, descricao, precovenda as preco, codigobarras,localestoque, situacao, impulsionarvendas FROM produtosfilial AS produto WHERE produto.codigofilial='" + GlbVariaveis.glb_filial + "' AND produto.situacao<>'Inativo' AND produto.situacao<>'Excluído' AND produto.impulsionarvendas = 'S' AND validadeimpulso >= CURDATE() AND quantidadeimpulso > 0 UNION " + // AND produto.impulsionarvendasvalidade < CURDATE() AND produto.impulsionarvendasqtd > 0
                               "(SELECT codigo, descricao, precovenda as preco, codigobarras,localestoque, situacao, impulsionarvendas FROM produtosfilial AS produto WHERE produto.codigofilial='" + GlbVariaveis.glb_filial + "' AND produto.situacao<>'Inativo' AND produto.situacao<>'Excluído' AND produto.dataultvenda> DATE_ADD(CURDATE(), INTERVAL -3 DAY) ORDER BY RAND() limit 20); ";
                    }
                    entidade = Conexao.CriarEntidade();
                    produtos = entidade.ExecuteStoreQuery<listPesquisaProdutos>(pesq).ToList();
                }

                if (produtos == null || produtos.Count == 0)
                {
                    this.Close();
                }

                var produtosFiltrados = (from p in produtos
                                            select new
                                            {
                                                codigo = p.codigo,
                                                descricao = p.descricao,
                                                preco = p.preco,
                                                codigobarras = p.codigobarras
                                            }).ToList();


                foreach (var item in produtosFiltrados)
                {
                    txtDescricaoProduto.Text = item.descricao;
                    txtPrecoPromocao.Text = "R$ "+ item.preco;


                    if (!string.IsNullOrEmpty(Convert.ToString(item.codigo)) || !string.IsNullOrEmpty(Convert.ToString(item.codigobarras)))
                    {
                        string arquivoImagem = @Application.StartupPath + @"\imagensprodutos\" + Convert.ToString(item.codigo + ".jpg");

                        if (!File.Exists(arquivoImagem))
                        {
                            arquivoImagem = @Application.StartupPath + @"\imagensprodutos\" + Convert.ToString(item.codigobarras + ".jpg");
                        }

                        if (!File.Exists(arquivoImagem))
                        {
                            try
                            {
                                if (metadeDaTela < 600)
                                {

                                }
                                else
                                {
                                    arquivoImagem = @Application.StartupPath + @"\imagensprodutos\sacola2.png";
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }

                        if (File.Exists(arquivoImagem))
                        {
                            fotoPrd.Visible = true;
                            fotoPrd.Image = Image.FromFile(arquivoImagem);
                        }
                        else
                        {
                            fotoPrd.Visible = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                this.Close();
            }
        }

        private void FrmAnuncio_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void FrmAnuncio_MouseHover(object sender, EventArgs e)
        {

            
        }

        private void FrmAnuncio_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            this.Close();

        }

        private void FrmAnuncio_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void FrmAnuncio_Shown(object sender, EventArgs e)
        {
            
            try
            { 
                carregarDados();
                
            }
            catch(Exception)
            {
                this.Close();
            } 
            
        }


        

        private void timeAnuncio_Tick(object sender, EventArgs e)
        {
            try
            {
                Random rd = new Random();
                int item = rd.Next(produtos.Count); 

                var itemSelecionado = produtos[item];

                if (itemSelecionado.impulsionarVendas == "S")
                {
                    timeAnuncio.Interval = 20000;
                }
                else
                {
                    timeAnuncio.Interval = 15000;
                }
                pnlPreco.Visible = true;

                if (nvezes > 3 && atualizarPromocoesIQCARD)
                {
                    try
                    {
                        atualizarPromocoesIQCARD = false;
                        // Aqui atualiza o site de promocoes com os itens com situacao igual a promocao
                        IqCard iqcard = new IqCard();
                        iqcard.AtualizarItensDelivery(false, true, false, true, false);
                    }
                    catch 
                    {                        
                    }
                   
                }

                if (nvezes >= 10 && !string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                {
                    try
                    {
                        nvezes = 0;
                        QRCodeGenerator qr = new QRCodeGenerator();
                        QRCodeData data = qr.CreateQrCode(GlbVariaveis.glb_chaveIQCard + "#avaliacao", QRCodeGenerator.ECCLevel.Q);
                        QRCode code = new QRCode(data);
                        fotoPrd.Image = code.GetGraphic(5);
                        fotoPrd.Image = Properties.Resources.mascote;
                        txtDescricaoProduto.Text = "😍 LEIA O QR-CODE DO CUPOM COM O APP IQCARD";
                        pnlPreco.Visible = false;
                        return;

                    }
                    catch (Exception)
                    {
                        pnlPreco.Visible = true;
                        nvezes = 0;
                    }
                    
                }


                txtDescricaoProduto.Text = itemSelecionado.descricao;
                txtPrecoPromocao.Text = "R$ "+ itemSelecionado.preco;
                nvezes++;

                if (!string.IsNullOrEmpty(Convert.ToString(itemSelecionado.codigo)) || !string.IsNullOrEmpty(Convert.ToString(itemSelecionado.codigobarras)))
                {
                    string arquivoImagem = @Application.StartupPath + @"\imagensprodutos\" + Convert.ToString(itemSelecionado.codigo + ".jpg");

                    if (!File.Exists(arquivoImagem))
                    {
                        arquivoImagem = @Application.StartupPath + @"\imagensprodutos\" + Convert.ToString(itemSelecionado.codigobarras + ".jpg");
                    }

                    if (!File.Exists(arquivoImagem))
                    {
                        arquivoImagem = @Application.StartupPath + @"\imagensprodutos\sacola2.png";
                    }

                    if (File.Exists(arquivoImagem))
                    {
                        fotoPrd.Visible = true;
                        fotoPrd.Image = Image.FromFile(arquivoImagem);
                    }

                    if (!File.Exists(arquivoImagem))
                    {
                        fotoPrd.Visible = false;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void FrmAnuncio_MouseClick(object sender, MouseEventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Compartilhar();
        }

        private static void Compartilhar()
        {
            if (!Funcoes.VerificarConexaoInternet())
            {
                MessageBox.Show("Sem internet");
                return;
            }
            DivulgarPromocaoIQCARD.iniciarmostrandoPromocao = true;
            DivulgarPromocaoIQCARD.iniciarmostrandoPedido = false;
            DivulgarPromocaoIQCARD promo = new DivulgarPromocaoIQCARD();
            promo.ShowDialog();
        }

        private void imgCompartilhar_Click(object sender, EventArgs e)
        {
            Compartilhar();
        }
    }
}


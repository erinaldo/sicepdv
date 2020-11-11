using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.IO;

namespace SICEpdv
{
    public partial class FrmProdutos : Form
    {
        

        TeclaNumerico teclado = new TeclaNumerico(Color.White);
        private string controle = "txtCodPrd";
        private string tecla = "";
        private string campoPreco = "precovenda";
        public static string ultCodigo = "";
        public static string ultDescricao = "";
        public static bool consultarPreco = false;
        public bool mostrarQuantidade = true;
        public bool teclado1 = false;


        siceEntities entidade;

        public FrmProdutos()
        {            
            InitializeComponent();
            pnlTeclado.Controls.Add(teclado);
            btnTeclado.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\btn-keyboard-disable.jpg"); ;

            if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
            {
                mostrarQuantidade = false;
                entidade = Conexao.CriarEntidade(false);
            }
            else
            {
                mostrarQuantidade = true;
                entidade = Conexao.CriarEntidade();
            }

            if (Configuracoes.mostrarPrecoMinimo == true)
            {
                lblPrecoMinimo.Visible = true;
            }
            else
            {
                lblPrecoMinimo.Visible = false;
            }



            #region delegates
            txtCodPrd.KeyUp += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.F7)
                    {
                        ChamarMenuFiscal();
                    }

                    if (evento.KeyCode == Keys.F2)
                        PosicaoFiliais();

                    if (evento.KeyCode == Keys.Return && consultarPreco)
                    {
                        txtCodPrd.Text = "";
                        evento.Handled = true;
                        return;
                    }

                    if (evento.KeyCode != Keys.Up && evento.KeyCode!=Keys.Down && evento.KeyCode!=Keys.Return && chkProcuraAut.Checked==true )
                        Procura(txtCodPrd.Text);
                };
            dtgProdutos.KeyDown += (objeto, evento) =>
                {
                    if (consultarPreco && evento.KeyCode == Keys.Return)
                    {
                        txtCodPrd.Text = "";
                        evento.SuppressKeyPress = true;
                        return;
                    }

                    if (evento.KeyCode == Keys.Return && !consultarPreco)
                    {
                        Sair();
                        evento.SuppressKeyPress = true;
                        return;
                    }

                    if (evento.KeyCode == Keys.F2)
                        PosicaoFiliais();

                };

            dtgProdutos.CellPainting += (objeto, evento) => MostrarPreco();
            
            txtCodPrd.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Up || evento.KeyCode == Keys.Down || evento.KeyCode == Keys.PageUp || evento.KeyCode == Keys.PageDown)
                    {                        
                        dtgProdutos.Focus();
                        if (evento.KeyCode == Keys.Up || evento.KeyCode==Keys.Down)
                        SendKeys.Send("{"+Convert.ToString(evento.KeyData)+"}");
                    }

                    if (evento.KeyCode == Keys.Return && !consultarPreco)
                    {
                        Sair();
                    }
                    if (evento.KeyCode == Keys.Escape)
                    {
                        ultCodigo = "";
                        this.Close();
                    }
                    if (evento.KeyCode == Keys.Return && consultarPreco)
                    {
                        txtCodPrd.Text = "";
                        evento.Handled = true;
                    }

                };
            dtgProdutos.KeyPress += (objeto, evento) =>
                {
                    if (evento.KeyChar == Convert.ToChar(Keys.Return))
                        return;


                    if (evento.KeyChar == Convert.ToChar(Keys.Escape))
                    {
                        ultCodigo = "";
                        this.Close();
                    }
                    //if (evento.KeyChar == Convert.ToChar(Keys.Return))
                    //{
                    //    Sair();
                    //    return;
                    //}

                    txtCodPrd.Focus();
                    SendKeys.Send("{RIGHT}");
                    txtCodPrd.Text=evento.KeyChar.ToString();
                    
                };
            #endregion

            this.teclado.clickBotao += new TeclaNumerico.ClicarNoBotao(DelegateTeclado);
            txtCodPrd.Enter += (objeto, evento) => controle = ActiveControl.Name;
        }

        private void Sair()
        {
            ultCodigo = "";
            if (dtgProdutos.RowCount > 0)
            {
                ultCodigo = Convert.ToString(dtgProdutos.CurrentRow.Cells["codigo"].Value);
                ultDescricao =Convert.ToString(dtgProdutos.CurrentRow.Cells["descricao"].Value);
            }
            this.Close();
        }

        private void Procura(string txtProcura)
        {

            if (!Conexao.onLine && Conexao.tipoConexao == 1)
            {
                dtgProdutos.DataSource = null;
                dtgProdutos.DataSource = (from StandAloneProdutos n in Produtos.ProdutosOFF
                                         where n.descricao.Contains(txtProcura)
                                         select n).AsQueryable();
                return;
            }
            /// <remarks>
            /// No método procura do Form Produto decidi usar
            /// uma seleção sql entity usando anonymous type e não as views
            /// pois assim fica mais flexível numa possível inclusão de nomes campos
            /// exigidos pelo PAF.
            /// </remarks>      
            /// 
            //if (txtProcura.Where(c => char.IsNumber(c)).Count() > 0 && chkTipoPesquisa.Checked==false)
            //{
            //    if (txtProcura.Length > 8)
            //        rdbBarras.Checked = true;
            //    else
            //        if (!chkTipoPesquisa.Checked)
            //        rdbCodigo.Checked = true;
            //}

            //if (txtProcura.Where(c => !char.IsNumber(c)).Count() > 0)
            //{
            //    rdbDescricao.Checked = true;
            //}

            string pesq = "";                        

            int limite = Configuracoes.limiteRegistroProdutos;
            if (limite == 0)
               limite = 50;          

            // Anonymus type (Selecionando os campos via sql entity)            

            pesq = @"select produto.codigo,IF(produto.precoatacado > 0,CONCAT(descricao,' | (',embalagem,')'),descricao) AS descricao," +
                    "produto.unidade,produto.STecf,produto.quantidade, produto." + campoPreco + " as preco, produto.icms," +
                    "produto.indicadorarredondamentotruncamento as IAT ," +
                    "produto.quantidade-produto.qtdprevenda as qtdDisponivel," +
                    "produto.qtdprateleiras as qtdprateleiras," +
                    "produto.quantidade-produto.qtdprateleiras as deposito," +
                    "produto.indicadorproducao as IPPT, "+
                    "produto.urlImagem as urlImagem," +
                    //"(select SUM(p.quantidade) from produtosfilial as p where p.codigo = produto.codigo) as filiais "+
                    "(SELECT IFNULL(SUM(p.quantidade),0) FROM produtosfilial AS p, filiais AS f WHERE p.codigo = produto.codigo AND f.codigofilial = p.codigoFilial AND f.ativa = 'S') as filiais, " +
                    "truncate(produto.precovenda - ((produto.precovenda * produto.descontomaximo) / 100),2) as precoMinimo,codigobarras," +
                    "produto.localestoque as localestoque "+
                    " from produtos as produto " +
                    //"WHERE produto.codigofilial=@filial " +
                    " WHERE produto.codigofilial='" + GlbVariaveis.glb_filial + "'" +
                    " AND produto.situacao<>'Inativo'" +
                    " AND produto.situacao<>'Excluído'" +
                    " AND produto.produtoInventario = 'S'" +
                    " AND produto." + campoPreco + ">0 ";

            if (GlbVariaveis.glb_filial != "00001")
            {
                pesq = @"select produto.codigo,IF(produto.precoatacado > 0,CONCAT(descricao,' | (',embalagem,')'),descricao) AS descricao," +
                        "produto.unidade,produto.STecf,produto.quantidade, produto." + campoPreco + " as preco, produto.icms," +
                        "produto.indicadorarredondamentotruncamento as IAT ," +
                        "produto.quantidade-produto.qtdprevenda as qtdDisponivel," +
                        "produto.qtdprateleiras as qtdprateleiras," +
                        "produto.quantidade-produto.qtdprateleiras as deposito," +
                        "produto.indicadorproducao as IPPT,"+
                        "produto.urlImagem as urlImagem," +
                        "((SELECT IFNULL(SUM(p.quantidade),0) FROM produtosfilial AS p, filiais AS f WHERE p.codigo = produto.codigo AND f.codigofilial = p.codigoFilial AND p.codigoFilial <> '" +GlbVariaveis.glb_filial+"' AND f.ativa = 'S') + (select sum(f.quantidade) from produtos as f where f.codigo = produto.codigo)) as filiais, " +
                        "truncate(produto.precovenda - ((produto.precovenda * produto.descontomaximo) / 100),2) as precoMinimo,codigobarras," +
                        "produto.localestoque as localestoque "+
                        " from produtosfilial as produto " +
                        //"WHERE produto.codigofilial=@filial " +
                        " WHERE produto.codigofilial='" + GlbVariaveis.glb_filial+"'"+
                        " AND produto.situacao<>'Inativo'" +
                        " AND produto.situacao<>'Excluído'" +
                        " AND produto.produtoInventario = 'S'" +
                        " AND produto." + campoPreco + ">0 ";
            }; 


                 if (rdbDescricao.Checked)
                 {
                     if (chkTipoPesquisa.Checked==true)
                     txtProcura = "%"+ txtCodPrd.Text.Trim() + "%";
                     else
                     txtProcura = txtCodPrd.Text.Trim() + "%";
                    pesq += " AND produto.descricao LIKE '"+txtProcura+"' ";
                    //pesq += " AND produto.descricao LIKE @procura";
                };
                 if (rdbCodigo.Checked)
                 {
                    pesq += " AND produto.codigo='"+txtProcura + "' ";
                    //pesq += " AND produto.codigo=@procura";
                };

                 if (rdbBarras.Checked)
                 {
                    pesq += " AND produto.codigobarras='"+txtProcura + "' ";
                    //pesq += " AND produto.codigobarras=@procura ";
                };

                 if (chkTipo.Checked)
                 {
                     pesq += " AND produto.tipo='0 - Produto'";
                 }

                 pesq += " ORDER BY produto.descricao LIMIT "+limite.ToString();

            /*var dados = entidade.CreateQuery<DbDataRecord>(pesq);
            dados.Parameters.Add(new ObjectParameter("filial",GlbVariaveis.glb_filial));
            dados.Parameters.Add(new ObjectParameter("procura", txtProcura));*/

            var produtos = entidade.ExecuteStoreQuery<listPesquisaProdutos>(pesq).ToList();

            var produtosFiltrados = (from p in produtos
                                     select new
                                     {
                                        codigo = p.codigo,
                                        descricao = p.descricao,
                                        unidade = p.unidade,
                                        STecf = p.STecf,
                                        quantidade = p.quantidade,
                                        preco = p.preco,
                                        filiais = p.filiais,
                                        icms = p.icms,
                                        IAT = p.IAT,
                                        qtdDisponivel = p.qtdDisponivel,
                                        qtdprateleiras = p.qtdprateleiras,
                                        deposito = p.deposito,
                                        IPPT = p.IPPT,
                                        precoMinimo = p.precoMinimo,
                                        codigobarras = p.codigobarras,
                                        localestoque = p.localestoque,
                                        urlImagem = p.urlImagem
                                     }).ToList();

            dtgProdutos.DataSource = produtosFiltrados;

            //dtgProdutos.DataSource = dados.AsQueryable();

            if (mostrarQuantidade == false)
                dtgProdutos.Columns.Remove("quantidade");

            MostrarPreco();
        }

        void MostrarPreco()
        {
            if (dtgProdutos.RowCount != 0)
            {
                lblPreco.Text = "R$" + string.Format("{0:c2}", dtgProdutos.CurrentRow.Cells["preco"].Value);
                lblProduto.Text = Convert.ToString(dtgProdutos.CurrentRow.Cells["descricao"].Value);
                lblLocalEstoque.Text = Convert.ToString(dtgProdutos.CurrentRow.Cells["localestoque"].Value) != "" ? "Local no Est.: " +Convert.ToString(dtgProdutos.CurrentRow.Cells["localestoque"].Value) : "";

                if (Configuracoes.mostrarPrecoMinimo == true)
                    lblPrecoMinimo.Text = " Preço Minimo: R$" + string.Format("{0:c2}", dtgProdutos.CurrentRow.Cells["precoMinimo"].Value);
            
            }
            else
            {
                lblPreco.Text = "R$ 0,00";
                lblProduto.Text = "";
            }

          
            /*try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtgProdutos.CurrentRow.Cells["codigo"].Value)) || !string.IsNullOrEmpty(Convert.ToString(dtgProdutos.CurrentRow.Cells["codigobarras"].Value)))
                {
                    string arquivoImagem = @Application.StartupPath + @"\imagensprodutos\" + Convert.ToString(dtgProdutos.CurrentRow.Cells["codigo"].Value + ".jpg");

                    if (!File.Exists(arquivoImagem))
                    {
                        arquivoImagem = @Application.StartupPath + @"\imagensprodutos\" + Convert.ToString(dtgProdutos.CurrentRow.Cells["codigobarras"].Value + ".jpg");
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

            }*/
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
                case "txtCodPrd":
                    Procura(txtCodPrd.Text);                   
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
            if (txtBox.Enabled == false)
                return;
            txtBox.Text += this.tecla;
            txtBox.Focus();
        }

        private void FrmProdutos_Load(object sender, EventArgs e)
        {
            chkProcuraAut.Checked = Configuracoes.procuraAutomaticaPrd;
            lblPreco.Text = "";
            lblLocalEstoque.Text = "";
            if (!string.IsNullOrEmpty(ultDescricao))
            {
                txtCodPrd.Text = (ultDescricao.Trim().PadRight(7, ' ').Substring(0, 7)).Trim();
                Procura(txtCodPrd.Text);
                ultDescricao = "";
                txtCodPrd.Text = "";
            }
            txtCodPrd.Focus();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            ultCodigo = "";
            ultDescricao = "";

            this.Close();
        }

        private void FocarProcura(object sender, EventArgs e)
        {
            txtCodPrd.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dtgProdutos.RowCount == 0)
                return;

            frmIndiceTecnicoProducao frm = new frmIndiceTecnicoProducao(Convert.ToString(dtgProdutos.CurrentRow.Cells["codigo"].Value));            
            frm.ShowDialog();
        }


        private void btnTabelaVarejo_Click(object sender, EventArgs e)
        {
            btnTabelaVarejo.BackColor = System.Drawing.Color.Green;
            btnTabelaAtacado.BackColor = System.Drawing.SystemColors.Control;
            campoPreco = "precovenda";
            Procura(txtCodPrd.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnTabelaAtacado.BackColor = System.Drawing.Color.Green;
            btnTabelaVarejo.BackColor = System.Drawing.SystemColors.Control;

            campoPreco = "precoatacado";
            Procura(txtCodPrd.Text);
        }

        private void chkTipoPesquisa_Click(object sender, EventArgs e)
        {
            txtCodPrd.Focus();
        }

        private static void ChamarMenuFiscal()
        {
            FrmMenuFiscal frmFiscal = new FrmMenuFiscal();
            frmFiscal.ShowDialog();
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            Procura(txtCodPrd.Text);
            dtgProdutos.Focus();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            PosicaoFiliais();
        }

        private void PosicaoFiliais()
        {
            if (dtgProdutos.RowCount != 0)
            {
                FrmPosicaoEstoqueFiliais posicao = new FrmPosicaoEstoqueFiliais();
                FrmPosicaoEstoqueFiliais.codigo = Convert.ToString(dtgProdutos.CurrentRow.Cells["codigo"].Value);
                posicao.ShowDialog();
                txtCodPrd.Focus();
            }
        }

        private void FrmProdutos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void dtgProdutos_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (GlbVariaveis.glb_filial == "00001")
            {
                string codigo = dtgProdutos.CurrentRow.Cells["codigo"].Value.ToString();

                var quantidade = (from n in Conexao.CriarEntidade().produtosfilial
                                  where n.codigo == codigo
                                  select (n.quantidade)).Sum();

                bntFiliais.Text = "Qtd Filiais .:"+quantidade;
            }
            else
            {

            }
            */
        }

        private void btnTeclado_Click(object sender, EventArgs e)
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

        private void dtgProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DivulgarPromocaoIQCARD.iniciarmostrandoPedido = false;
                DivulgarPromocaoIQCARD.iniciarmostrandoPromocao = true;
                DivulgarPromocaoIQCARD anuncio = new DivulgarPromocaoIQCARD();
                anuncio.txtChamada.Text = Convert.ToString(dtgProdutos.CurrentRow.Cells["descricao"].Value);
                anuncio.txtCodigo.Text = Convert.ToString(dtgProdutos.CurrentRow.Cells["codigo"].Value);
                anuncio.txtPreco.Text = Convert.ToString(dtgProdutos.CurrentRow.Cells["preco"].Value);

                anuncio.ShowDialog();
              
            };
        }

        private void btnContador_Click(object sender, EventArgs e)
        {
            menuContabil.Show(btnContador, new Point(btnContador.Width, 0));
            
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            UploadImagem();
        }

        private void UploadImagem()
        {
            UplodImagemProduto.codigoBarrasPrd = Convert.ToString(dtgProdutos.CurrentRow.Cells["codigobarras"].Value);            
            UplodImagemProduto.codigoPrd = Convert.ToString(dtgProdutos.CurrentRow.Cells["codigo"].Value);
            

            UplodImagemProduto.descricaoPrd = Convert.ToString(dtgProdutos.CurrentRow.Cells["descricao"].Value);

            UplodImagemProduto upload = new UplodImagemProduto();
            upload.ShowDialog();
        }

        private void fotoPrd_Click(object sender, EventArgs e)
        {
            UploadImagem();
        }

        private void enviarProdutosParaAContabilidadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Configuracoes.cnpj))
            {
                MessageBox.Show("CNPJ da filial é necessário para poder enviar os produtos");
                return;
            }

            FrmLogon Logon = new FrmLogon();
            Operador.autorizado = false;
            Logon.idCliente = 0;
            Logon.campo = "gerente";
            Logon.lblDescricao.Text = "ENVIAR PRODUTOS CONTADOR(A) CNPJ : "+Funcoes.FormatarCNPJ(Configuracoes.cnpj);
            Logon.txtDescricao.Text = "Digite a senha para confirmar. É necessário ter a permissão de Gerente";
            Logon.ShowDialog();
            if (!Operador.autorizado)
                return;

            if (string.IsNullOrEmpty(Configuracoes.emailContador))
            {
                MessageBox.Show("Cadastre o email do contador nas configurações da filial no SICE.net para que ele possa ter acesso ao portal. Os produtos serão enviados mesmo assim.");
            }

            if (MessageBox.Show("Ao enviar os produtos o seu contador terá as informações fiscais dos produtos como NFC,CEST,Tributação?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            Produtos produtos = new Produtos();
            var inicio = DateTime.Now;
            FrmMsgOperador msg3 = new FrmMsgOperador("", "Enviando informações fiscais dos produtos para a contabilidade. Pode demorar alguns minutos.");
            msg3.Show();
            Application.DoEvents();
            try
            {
                produtos.AtualizarProdutosContadorNuvem();
                MessageBox.Show("Produtos exportado com sucesso. Seu contador receberá um e-mail para poder acessar o portal");
                PortalContabilidade portal = new PortalContabilidade();
                portal.ShowDialog();
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

        private void retirarAcessoDosProdutosDaContabilidadeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Configuracoes.cnpj))
            {
                MessageBox.Show("CNPJ da filial é necessário para poder enviar os produtos");
                return;
            }



            FrmLogon Logon = new FrmLogon();
            Operador.autorizado = false;
            Logon.idCliente = 0;
            Logon.campo = "gerente";
            Logon.lblDescricao.Text = "RETIRAR ACESSO PRODUTOS CONTADOR(A)";
            Logon.txtDescricao.Text = "Digite a senha para confirmar. É necessário ter a permissão de Gerente";
            Logon.ShowDialog();
            if (!Operador.autorizado)
                return;


            Produtos produtos = new Produtos();
            var inicio = DateTime.Now;
            FrmMsgOperador msg3 = new FrmMsgOperador("", "Retirando o acesso da contabilidade.");
            msg3.Show();
            Application.DoEvents();
            try
            {
                produtos.ApagarProdutosContador();
                MessageBox.Show("Produtos retirado do acesso da contabilidade com sucesso. Seu contador(a) não terá mais acesso!");
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

        private void portalECredenciaisDeAcessoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Configuracoes.cnpj))
            {
                MessageBox.Show("CNPJ da filial é necessário para poder enviar os produtos");
                return;
            }

            FrmLogon Logon = new FrmLogon();
            Operador.autorizado = false;
            Logon.idCliente = 0;
            Logon.campo = "gerente";
            Logon.lblDescricao.Text = "ENVIAR PRODUTOS CONTADOR(A) CNPJ : " + Funcoes.FormatarCNPJ(Configuracoes.cnpj);
            Logon.txtDescricao.Text = "Digite a senha para confirmar. É necessário ter a permissão de Gerente";
            Logon.ShowDialog();
            if (!Operador.autorizado)
                return;

            if (string.IsNullOrEmpty(Configuracoes.emailContador))
            {
                MessageBox.Show("Cadastre o email do contador nas configurações da filial no SICE.net para que ele possa ter acesso ao portal.");
            }

            PortalContabilidade portal = new PortalContabilidade();
            portal.ShowDialog();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
namespace SICEpdv
{
    public partial class FrmMenuFiscal : Form
    {
        FuncoesECF fecf = new FuncoesECF();
        StringBuilder arquivosPAF = new StringBuilder();
        public bool  estoqueParcial = false;
        public bool  estoqueTotal = true;
        private int tipoConexao = 1;

        public FrmMenuFiscal()
        {
            InitializeComponent();

            if (ConfiguracoesECF.pdv == true && ConfiguracoesECF.idECF == 0)
            {
                btnLX.Enabled = false;
                btnLMFC.Enabled = false;
                btnLMFS.Enabled = false;
                btnArqAC1704.Enabled = false;
                btnArqMF.Enabled = false;
                btnArqMFD.Enabled = false;
                btnParametros.Enabled = false;
                btnIDPAF.Enabled = false;
                btnLMF.Enabled = false;
                btnEspelho.Enabled = false;
            }

            txtIntervaloInicial.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtIntervaloFinal.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            rdbCRZ.Click += (objeto, evento) =>
                {
                    grpIntervalo.Visible = true;
                    grpDatas.Visible = false;
                    grpIntervalo.Refresh();
                    Application.DoEvents();
                };
            rdbCOO.Click += (objeto, evento) =>
            {
                grpIntervalo.Visible = true;
                grpDatas.Visible = false;
                grpIntervalo.Refresh();
                Application.DoEvents();
            };


            rdbPeriodo.Click += (objeto, evento) =>
                {
                    grpDatas.Visible=true;
                    grpIntervalo.Visible = false;
                    txtIntervaloInicial.Text = "0";
                    txtIntervaloFinal.Text = "0";
                    grpDatas.Refresh();                    
                };
            txtECF.KeyPress += (objeto, evento) =>
                {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
                };
            txtECF.Enter += (objeto, evento) =>
                {
                    txtECF.Text = ConfiguracoesECF.numeroECF;
                };
            txtCodPrd.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Return)
                    {
                        if (string.IsNullOrEmpty(txtCodPrd.Text))
                        {
                            FrmProdutos chamaPrd = new FrmProdutos();
                            chamaPrd.ShowDialog();
                            txtCodPrd.Text = FrmProdutos.ultCodigo;
                        }
                        IncluirCodigoEstoqueParc();
                        txtCodPrd.Text = "";
                    }
                };
        }

        private void IncluirCodigoEstoqueParc()
        {
            try
            {
                estoqueTotal = false;
                estoqueParcial = true;
                Produtos prd = new Produtos();
                prd.ProcurarCodigo(txtCodPrd.Text, GlbVariaveis.glb_filial);
                lblDescricao.Text = prd.descricao;
                if (GlbVariaveis.glb_filial == "00001")
                {
                    siceEntities entidade = Conexao.CriarEntidade();
                    var update = (from n in entidade.produtos
                                  where n.codigo == prd.codigo
                                  select n).First();
                    update.marcado = "P";
                    entidade.SaveChanges();
                }
                else
                {
                    if (GlbVariaveis.glb_filial == "00001")
                    {
                        siceEntities entidade = Conexao.CriarEntidade();
                        var update = (from n in entidade.produtosfilial
                                      where n.codigo == prd.codigo
                                      && n.CodigoFilial == GlbVariaveis.glb_filial
                                      select n).First();
                        update.marcado = "P";
                        entidade.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLX_Click(object sender, EventArgs e)
        {
            FrmMsgOperador aguarde = new FrmMsgOperador("", "Executando comando !");
            aguarde.Show();
            Application.DoEvents();
            try
            {
                if (ConfiguracoesECF.NFC == false)
                    fecf.LeituraX();
                else
                {
                    if (MessageBox.Show("Apenas valores autorizados?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        fecf.LeituraXNFC(dataInicial.Value, dataFinal.Value, true);
                    }
                    else
                    {
                        fecf.LeituraXNFC(dataInicial.Value, dataFinal.Value);
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível emitir Leitura X: " + erro.Message);
            }
            finally
            {
                aguarde.Dispose();
            }

        }

        private void btnLMFC_Click(object sender, EventArgs e)
        {
            if (ConfiguracoesECF.NFC == false)
            {
                if (rdCAT52.Checked == true)
                {
                    System.Diagnostics.Process.Start("registra.bat", "");
                    System.Threading.Thread.Sleep(2000);
                    Application.DoEvents();

                    GerarEspelhoMFD();
                    System.Threading.Thread.Sleep(300);
                    FrmMsgOperador msg = new FrmMsgOperador("", "Gerando Cat52");
                    msg.Show();
                    Application.DoEvents();

                    try
                    {
                        while (dataInicial.Value.Date <= dataFinal.Value.Date)
                        {
                            FuncoesECF.GerarCat52("", dataInicial.Value.Date, dataFinal.Value.Date, ConfiguracoesECF.pathRetornoECF + "retorno.txt");
                            dataInicial.Value = dataInicial.Value.AddDays(1);
                            Application.DoEvents();
                        }

                        MessageBox.Show("Arquivos Gerados no diretório CAT52");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        msg.Dispose();
                    }
                    return;
                }

                if (rdAtoCotepe.Checked == true)
                    GerarArquivoMFD(false);
                else
                    LeituraMemoriaFiscal("c");
            }
            else
            {
                FrmMsgOperador aguarde = new FrmMsgOperador("", "Executando comando !");
                aguarde.Show();
                Application.DoEvents();

                    if (MessageBox.Show("Apenas valores autorizados?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        fecf.LeituraXNFCDetalhada(dataInicial.Value, dataFinal.Value, true);
                    }
                    else
                    {
                        fecf.LeituraXNFCDetalhada(dataInicial.Value, dataFinal.Value);
                    }

                aguarde.Dispose();
            }
        }

        private void LeituraMemoriaFiscal(string tipoRelatorio)
        {
            FrmMsgOperador aguarde = new FrmMsgOperador("", "Executando comando !");
            aguarde.Show();

            if (ConfiguracoesECF.NFC == false)
            {
                if (txtIntervaloInicial.Text == "")
                {
                    txtIntervaloInicial.Text = "0";
                    txtIntervaloFinal.Text = "0";
                }
                try
                {
                    fecf.LMFC(rdbImpressora.Checked == true ? "I" : "A", Convert.ToDateTime(dataInicial.Text), Convert.ToDateTime(dataFinal.Text), Convert.ToInt32(txtIntervaloInicial.Text), Convert.ToInt32(txtIntervaloFinal.Text), tipoRelatorio, true);
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
                finally
                {
                    aguarde.Dispose();
                }
                txtDiretorio.Text = Application.StartupPath.ToString() + @"\" + ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\LMF.txt";
            }
            else
            {
                if (MessageBox.Show("Apenas valores autorizados?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fecf.LeituraXNFCSimplificada(dataInicial.Value, dataFinal.Value, true);
                }
                else
                {
                    fecf.LeituraXNFCSimplificada(dataInicial.Value, dataFinal.Value);
                }
            }

            aguarde.Dispose();
        }

        private void btnLMFS_Click(object sender, EventArgs e)
        {
            LeituraMemoriaFiscal("S");
        }

        private void btnEspelhoMFD_Click(object sender, EventArgs e)
        {
            //GerarEspelhoMFD();
            GerarArquivoMFD(true);
        }

        private void GerarEspelhoMFD()
        {
            FrmMsgOperador aguarde = new FrmMsgOperador("", "Executando comando !");
            aguarde.Show();
            Application.DoEvents();
            try
            {
                FuncoesECF.EspelhoMFD(ConfiguracoesECF.pathRetornoECF+"download.mfd", ConfiguracoesECF.pathRetornoECF+"retorno.txt", rdbPeriodo.Checked == true ? "1" : "2", dataInicial.Value, dataFinal.Value, Convert.ToInt32(txtIntervaloInicial.Text), Convert.ToInt32(txtIntervaloFinal.Text));
                System.Threading.Thread.Sleep(300);

                File.Copy(ConfiguracoesECF.pathRetornoECF + "retorno.txt", Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\espelhoMFD.txt",true);
                txtDiretorio.Text = Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\espelhoMFD.txt";
                FuncoesECF.AssinarArquivo(@txtDiretorio.Text, false);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível gerar arquivo agora: " + erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                aguarde.Dispose();
            }
        }


        private void btnArqMFD_Click(object sender, EventArgs e)
        {
            GerarArquivoMFD(false);     
        }

        private void GerarArquivoMFD(bool cotepe)
        {
            FrmMsgOperador aguarde = new FrmMsgOperador("", "Executando comando Gerar Arq.MFD!");
            aguarde.Show();
            Application.DoEvents();

            if (cotepe == true)
            {
                var dataR02 = (from r in Conexao.CriarEntidade().r02
                            where r.codigofilial == GlbVariaveis.glb_filial
                            select r.datamovimento).Max();

                if ((dataInicial.Value > dataR02.Value || dataFinal.Value > dataR02.Value) && rdbPeriodo.Checked == true)
                {
                    MessageBox.Show("Data Maior que a última redução Z");
                    aguarde.Dispose();
                    return;
                }
                else if(rdbCOO.Checked == true)
                {
                    string SQL = "SELECT COUNT(*) AS linhas FROM contdocs " +
                                    " WHERE DATA > (SELECT datamovimento DATA FROM r02 WHERE datamovimento < CURRENT_DATE() ORDER BY datamovimento DESC LIMIT 1) "+
                                    " AND ncupomfiscal BETWEEN '"+txtIntervaloInicial.Text+"' AND '"+txtIntervaloFinal.Text+"'";

                    var quantidade = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL);
                    /*MessageBox.Show(SQL.ToString());
                    MessageBox.Show("Quantidade" + quantidade.ToString());
                    MessageBox.Show("Quantidade" + quantidade.FirstOrDefault().ToString());*/
                    if (quantidade.FirstOrDefault() > 0)
                    {
                        MessageBox.Show("intervalo de COO com movimento atual!");
                        aguarde.Dispose();
                        return;
                    }
                }
            }
            
            

            try
            {
                string nomeArquivo = "download.txt";

                //if (rdbPeriodo.Checked == true)
                //{
                //    nomeArquivo = @"\atocotepe_data.txt";
                //}
                //if (rdbCRZ.Checked == true)
                //{
                //    nomeArquivo = @"\atocotepe_crz.txt";
                //}
                
                if (GlbVariaveis.glb_Acbr == false)//antes não tinha esse if 2016-04-19
                {
                    #region
                    if (cotepe)
                        FuncoesECF.GerarAtoCotepe1704(rdbPeriodo.Checked == true ? "1" : "2", dataInicial.Value, dataFinal.Value, Convert.ToInt32(txtIntervaloInicial.Text), Convert.ToInt32(txtIntervaloFinal.Text));
                    else
                    {
                        //  nomeArquivo = @"\atocotepe.txt";
                        FuncoesECF.GerarAtoCotepe1704LMFC(rdbPeriodo.Checked == true ? "1" : "2", dataInicial.Value, dataFinal.Value, Convert.ToInt32(txtIntervaloInicial.Text), Convert.ToInt32(txtIntervaloFinal.Text));
                    }

                    System.Threading.Thread.Sleep(2000);

                    if (ConfiguracoesECF.idECF == 1)
                    {
                        File.Copy(ConfiguracoesECF.pathRetornoECF + "sintegra.txt", Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + nomeArquivo, true);
                        txtDiretorio.Text = Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\" + nomeArquivo;
                    }
                    if (ConfiguracoesECF.idECF == 2)
                    {

                        //if (rdbPeriodo.Checked == true)
                        //{

                        //    File.Copy(@ConfiguracoesECF.pathRetornoECF + "ATO_MFD_DATA.txt", Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"]  + nomeArquivo, true);
                        //    txtDiretorio.Text = Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + nomeArquivo;
                        //}
                        //if (rdbCRZ.Checked == true)
                        //{                        
                        //    File.Copy(@ConfiguracoesECF.pathRetornoECF + "ATO_MFD_COO.txt", Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + nomeArquivo, true);
                        //    txtDiretorio.Text = Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + nomeArquivo;
                        //}

                        if (File.Exists(@ConfiguracoesECF.pathRetornoECF + "Daruma.mfd"))
                        {
                            File.Copy(@ConfiguracoesECF.pathRetornoECF + "Daruma.mfd", Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\arquivoMFD.mfd", true);
                        }

                    }

                    System.Threading.Thread.Sleep(1000);
                    txtDiretorio.Text = Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\arquivoMFD.mfd";
                    System.Threading.Thread.Sleep(1000);
                    //  Paf.RetirarEAD(@txtDiretorio.Text);
                    System.Threading.Thread.Sleep(1000);

                    var md5Bin = FuncoesECF.AssinarArquivo(@txtDiretorio.Text, false, true, true);


                    using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\arquivoMFD.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            sw.Write(md5Bin);
                        }
                    };

                    #endregion
                }
                else
                {
                    #region
                    if (cotepe)
                        FuncoesECF.GerarAtoCotepe1704(rdbPeriodo.Checked == true ? "1" : "2", dataInicial.Value, dataFinal.Value, Convert.ToInt32(txtIntervaloInicial.Text), Convert.ToInt32(txtIntervaloFinal.Text));
                    else
                        FuncoesECF.GerarArqMFD();

                    txtDiretorio.Text = Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"];

                    var md5Bin = FuncoesECF.AssinarArquivo(@txtDiretorio.Text + @"\Daruma.mfd", false, true, true);


                    using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\arquivoMF.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            sw.Write(md5Bin);
                        }
                    };
                    #endregion

                }

                aguarde.Dispose();
               

                //FuncoesECF.AssinarArquivo(@txtDiretorio.Text.Replace(".mfd",".txt"), false);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível gerar arquivo agora: " + erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                aguarde.Dispose();
            }
            finally
            {
                aguarde.Dispose();
            }

            aguarde.Dispose();
        }

        private void GerarArqMF()
        {
            FrmMsgOperador aguarde = new FrmMsgOperador("", "Executando comando Gerar Arq.MF!");
            aguarde.Show();
            Application.DoEvents();
            try
            {
                string nomeArquivo = "arquivoMF.txt";

                //if (rdbPeriodo.Checked == true)
                //{
                //    nomeArquivo = @"\arqMF_data.txt";
                //}
                //if (rdbCRZ.Checked == true)
                //{
                //    nomeArquivo = @"\arqMF_crz.txt";
                //}

                FuncoesECF.GerarArqMF(rdbPeriodo.Checked == true ? "1" : "2",dataInicial.Value, dataFinal.Value, Convert.ToInt32(txtIntervaloInicial.Text), Convert.ToInt32(txtIntervaloFinal.Text));
                System.Threading.Thread.Sleep(1000);
                txtDiretorio.Text = Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"];
                System.Threading.Thread.Sleep(1000);
                System.Threading.Thread.Sleep(1000);

                System.Threading.Thread.Sleep(2000);



                if (GlbVariaveis.glb_Acbr == false)
                {
                    #region
                    if (ConfiguracoesECF.idECF == 1)
                    {
                        File.Copy(ConfiguracoesECF.pathRetornoECF + "arquivoMF.txt", Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + nomeArquivo, true);
                        txtDiretorio.Text = Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\" + nomeArquivo;
                    }
                    else if (ConfiguracoesECF.idECF == 2)
                    {
                        /* comentado por ivan
                        if (rdbPeriodo.Checked == true)
                        {

                            File.Copy(@ConfiguracoesECF.pathRetornoECF + "ATO_MFD_DATA.txt", Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + nomeArquivo, true);
                            txtDiretorio.Text = Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + nomeArquivo;
                        }
                        if (rdbCRZ.Checked == true)
                        {
                            File.Copy(@ConfiguracoesECF.pathRetornoECF + "ATO_MFD_COO.txt", Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + nomeArquivo, true);
                            txtDiretorio.Text = Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + nomeArquivo;
                        }
                        */

                        if (File.Exists(@ConfiguracoesECF.pathRetornoECF + "Daruma.mf"))
                        {
                            File.Copy(@ConfiguracoesECF.pathRetornoECF + "Daruma.mf", Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\arquivoMF.mf", true);
                        }

                    }

                    var md5Bin = FuncoesECF.AssinarArquivo(@txtDiretorio.Text, false, true, true);


                    using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\arquivoMF.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            sw.Write(md5Bin);
                        }
                    };

                    #endregion
                }
                else
                {
                    var md5Bin = FuncoesECF.AssinarArquivo(@txtDiretorio.Text + @"\Daruma.mf", false, true, true);


                    using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\arquivoMF.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            sw.Write(md5Bin);
                        }
                    };

                }

               // FuncoesECF.AssinarArquivo(@txtDiretorio.Text, false);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível gerar arquivo agora: " + erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                aguarde.Dispose();
            }
        }

        private void btnTabPrd_Click(object sender, EventArgs e)
        {
            GerarTabelaProdutos("varejo");
        }

        private void GerarTabelaProdutos(string tabelaPreco)
        {
            siceEntities entidade = Conexao.CriarEntidade();

            var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);

            if (!FuncoesPAFECF.VerificarQtdRegistro("produtos"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            StringBuilder conteudo = new StringBuilder();
           // conteudo = RegistroU1("");


            // Registro substituido peloU1
            /*conteudo.AppendLine("P1" +
                Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                razao);
            */
            var produtos = (from n in entidade.produtos
                            where n.CodigoFilial == GlbVariaveis.glb_filial
                            select new
                            {
                                n.codigo,
                                n.descricao,
                                n.unidade,
                                n.unidembalagem,
                                n.indicadorarredondamentotruncamento,
                                n.indicadorproducao,
                                n.tributacao,
                                n.tipo,
                                n.icms,                                
                                n.precovenda,
                                n.precoatacado,
                                n.EADP2relacaomercadoria
                            })
                           .Concat(
                           from n in entidade.produtosfilial
                           where n.CodigoFilial == GlbVariaveis.glb_filial
                           select new
                           {
                               n.codigo,
                               n.descricao,
                               n.unidade,
                               n.unidembalagem,
                               n.indicadorarredondamentotruncamento,
                               n.indicadorproducao,
                               n.tributacao,
                               n.tipo,
                               n.icms,
                               n.precovenda,
                               n.precoatacado,
                               n.EADP2relacaomercadoria
                           });

            int nRegistros = 0;
            if (tabelaPreco == "varejo")
            {
                produtos = from n in produtos
                           where n.precovenda > 0
                           select n;
            }

            if (tabelaPreco == "atacado")
            {
                produtos = from n in produtos
                           where n.precoatacado > 0
                           select n;
            }
            
            foreach (var item in produtos)
            {
                /*
                 * I = Isenção
                 * N = Não incidência
                 * F = Substituição Tributária
                 */
                decimal preco = item.precovenda;
                string unidade = item.unidade;

                var tributacao = SituacaoTributacao(Convert.ToInt16(item.icms), item.tributacao, item.tipo);

                if (tabelaPreco == "atacado")
                {
                    preco = item.precoatacado;
                    unidade = item.unidembalagem;

                }
                
                var cripto = Funcoes.CriptografarMD5( item.codigo.Trim() + item.descricao + item.tributacao+item.icms.ToString().Replace(",",".")+item.precovenda.ToString().Replace(",",".")+item.precoatacado.ToString().Replace(",",".") );
                if (cripto != item.EADP2relacaomercadoria && item.EADP2relacaomercadoria!=null)
                {                    
                    unidade = item.unidade.Trim().PadRight(6, '?');
                }
                conteudo.AppendLine("P2" +
                    Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                    item.codigo.Trim().PadRight(14, ' ').Substring(0, 14) +
                    item.descricao.Trim().PadRight(50, ' ').Substring(0, 50) +
                    unidade.PadRight(6, ' ').Substring(0, 6) +
                    item.indicadorarredondamentotruncamento +
                    item.indicadorproducao +
                    tributacao +
                    item.icms.ToString().Replace(",", "").PadRight(4,'0').Substring(0,4) +
                    Funcoes.FormatarZerosEsquerda(preco, 12,true));
                nRegistros++;
            }

            arquivosPAF.Append(conteudo);
            return;
            //Não existe mais
            /*
            conteudo.AppendLine("P9" +
                 Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                 Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                 string.Format("{0:000000}", nRegistros));
            */
            using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\TabProdutos.txt"))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                {
                    sw.Write(@conteudo);
                }
            };
            FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\TabProdutos.txt",false);
            txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\TabProdutos.txt";
        }

        private void GerarEstoque(string tipo)
        {
            // Antes da versao 01.08 do PAF criptografia do saldo em estoque
            //@tabelaProduto,'.EADE2mercadoriaEstoque=md5( concat(',@tabelaProduto,'.codigo,',@tabelaProduto,'.descricao,',@tabelaProduto,'.quantidade-( select sum(vendas.quantidade) from vendas where vendas.cancelado="N" and vendas.codigo=',@tabelaProduto,'.codigo ),',@tabelaProduto,'.dataultvenda) ),',
            string marcado = " ";
            if (tipo.ToLower() == "total")
            {
                string sql = "UPDATE produtos set marcado=' '";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                sql = "UPDATE produtosfilial set marcado=' '";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);

            }
            if (tipo == "parcial")
                marcado = "P";

            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando arquivo");
            msg.Show();
            Application.DoEvents();
            try
            {
                siceEntities entidade = Conexao.CriarEntidade();

                   string dataEstoque =  string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                   string horaEstoque = string.Format("{0:hhmmss}", DateTime.Now.TimeOfDay);

                   var estoque = (from n in entidade.produtos
                                  where n.CodigoFilial == GlbVariaveis.glb_filial
                                  && n.marcado == marcado
                                  where n.EADE2mercadoriaEstoque != null
                                  select new
                                  {
                                      n.codigo,
                                      n.descricao,
                                      n.unidade,
                                      n.saldofinalestoque,
                                      n.data,
                                      n.datafinalestoque,
                                      n.horafinalestoque,
                                      n.EADE2mercadoriaEstoque,
                                      n.EADE1,
                                      n.dataultvenda,
                                      n.marcado,
                                      n.ecffabricacao,
                                      
                                      
                                  });


                  

                   var itensEstoque = estoque.ToList();

                   if (estoque.Count() == 0 || estoque == null)
                   {
                       MessageBox.Show("Escolha os itens antes");
                       return;
                   };



                  
                   // Pegando dados ECF 
                   var dadosECF = (from n in Conexao.CriarEntidade().r01
                                   select new { n.fabricacaoECF, n.modeloECF, n.tipoECF,n.marcaECF }).FirstOrDefault();


                   string nrFabricacao = itensEstoque.First().ecffabricacao;// ConfiguracoesECF.nrFabricacaoECF;
                   string modeloECF = dadosECF.modeloECF ?? ConfiguracoesECF.modeloECF;
                   string marcaECF = dadosECF.marcaECF ?? ConfiguracoesECF.marcaECF;
                   string tipoECF = dadosECF.tipoECF ?? ConfiguracoesECF.tipoECF;

                   nrFabricacao = estoque.First().ecffabricacao;
                   horaEstoque = string.Format("{0:hhmmss}", itensEstoque.First().horafinalestoque.Value);
                   dataEstoque = string.Format("{0:yyyyMMdd}", itensEstoque.First().datafinalestoque.Value);

                   
                   foreach (var itemE1 in estoque)
                   {
                      
                       var criptoE1 = Funcoes.CriptografarMD5(string.Format("{0:yyyy-MM-dd}", itemE1.datafinalestoque) + itemE1.horafinalestoque.ToString() + itemE1.ecffabricacao);
                       if (criptoE1 != itemE1.EADE1)
                       {
                           nrFabricacao = itensEstoque.First().ecffabricacao;
                           modeloECF = ConfiguracoesECF.modeloECF.Trim().PadRight(20, '?'); ;
                           horaEstoque = string.Format("{0:hhmmss}", itemE1.horafinalestoque.Value);
                           dataEstoque = string.Format("{0:yyyyMMdd}", itemE1.datafinalestoque.Value);
                       }

                   }
                   var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);

                   if (!FuncoesPAFECF.VerificarQtdRegistro("produtos"))
                   {
                       razao = razao.Trim().PadRight(50, '?');
                   }

  
               
                StringBuilder conteudo = new StringBuilder();
                //Não existe mais
                /*
                conteudo.AppendLine("E1" +
                    Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                    Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                    Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                    razao+
                    nrFabricacao.PadRight(20,' ').Substring(0,20)+
                    "1"+ //mfadicional
                       tipoECF.PadRight(7,' ').Substring(0,7)+
                        marcaECF.PadRight(20,' ').Substring(0,20)+
                        modeloECF.PadRight(20,' ').Substring(0,20)+
                        dataEstoque+
                        horaEstoque
                    );
                 */
      
                 
                int nRegistros = 0;
                foreach (var item in estoque)
                {                                  
                    var unidade = item.unidade.Trim().PadRight(6, ' ').Substring(0, 6);
                    var cripto = Funcoes.CriptografarMD5(string.Format("{0:yyyy-MM-dd}", item.datafinalestoque) + item.horafinalestoque.ToString() + item.ecffabricacao);
                    if (cripto != item.EADE1)
                    {
                        unidade = item.unidade.Trim().PadRight(6, '?');
                       
                    }
                    string posicaoEstoque = "+";
                    if (item.saldofinalestoque < 0)
                        posicaoEstoque = "-";

                    conteudo.AppendLine("E2" +
                        Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                        item.codigo.Trim().PadRight(14, ' ').Substring(0, 14) +
                        item.descricao.Trim().PadRight(50, ' ').Substring(0, 50) +
                        unidade.PadRight(6,' ').Substring(0,6) +
                        posicaoEstoque+
                        Funcoes.FormatarZerosEsquerda(item.saldofinalestoque, 9, true).Replace("-","") );
                    nRegistros++;
                    if (marcado == "P")
                    {
                        siceEntities entidadePR = Conexao.CriarEntidade();
                        var alterarMarcado = (from n in entidadePR.produtos
                                              where n.codigo == item.codigo
                                              select n).First();
                        alterarMarcado.marcado = " ";
                        entidadePR.SaveChanges();
                    }
                }

                arquivosPAF.Append(conteudo);
                return;


                //Não existe mais
                /*
                conteudo.AppendLine("E9" +
                    Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                    Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                    string.Format("{0:000000}", nRegistros));
                */
                using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\Estoque.txt"))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        sw.Write(@conteudo);
                    }
                };

                FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\Estoque.txt", false);
                txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\Estoque.txt";
            }
            finally
            {
                msg.Dispose();
            }
        }

        private void btnIDPAF_Click(object sender, EventArgs e)
        {
            IdentificacaoPAF();
        }

        private void IdentificacaoPAF()
        {
            StringBuilder sb = new StringBuilder();

            

            //XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\sHouse.xml");

            //var dados = (from n in doc.Descendants("sHouse").Elements("Dados")
            //             select n).First();
            try
            {
               
                FuncoesECF.RelatorioGerencial("abrir", "", "PARAM.CONF");
                FuncoesECF.RelatorioGerencial("imprimir", "Nr. do Laudo: " + GlbVariaveis.laudoPAF + Environment.NewLine);//  Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("laudo").First().Value), GlbVariaveis.glbSenhaIQ) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Id. S House : IQ SISTEMAS"+ Environment.NewLine); //+ Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("razaoSocial").First().Value), GlbVariaveis.glbSenhaIQ) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "CNPJ        : " + GlbVariaveis.cnpjSH + Environment.NewLine); // Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("cnpj").First().Value), GlbVariaveis.glbSenhaIQ) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Razão Social: " + GlbVariaveis.razaoSH + Environment.NewLine); // Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("razaoSocial").First().Value), GlbVariaveis.glbSenhaIQ) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Endereço    : " + GlbVariaveis.enderecoSH + Environment.NewLine);// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("endereco").First().Value), GlbVariaveis.glbSenhaIQ) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Telefone    : " + Funcoes.FormatarTelefone(GlbVariaveis.telefoneSH) + Environment.NewLine);//   87 3821 2715") ;//Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("endereco").First().Value), GlbVariaveis.glbSenhaIQ) + Environment.NewLine);                
                FuncoesECF.RelatorioGerencial("imprimir", "Contacto    : " + GlbVariaveis.contatoSH + Environment.NewLine);// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("pessoaContato").First().Value), GlbVariaveis.glbSenhaIQ) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", " ");
                FuncoesECF.RelatorioGerencial("imprimir","IDENTIFICAÇÃO DO PAF-ECF");
                FuncoesECF.RelatorioGerencial("imprimir", " ");


                FuncoesECF.RelatorioGerencial("imprimir", "Nome comercial       : " + GlbVariaveis.nomeAplicativo + Environment.NewLine);// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("aplicativo").First().Value), GlbVariaveis.glbSenhaIQ) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Versão               : " + GlbVariaveis.glb_Versao + Environment.NewLine);// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("versao").First().Value), GlbVariaveis.glbSenhaIQ) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Principal Arquivo EXE: " + "SICEpdv.exe" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "MD5 .exe             : " + ConfiguracoesECF.md5PrincipalEXE + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Versão PAF-ECF       : ER PAF-ECF " + GlbVariaveis.versaoPAF + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("Demais Arquivos", " ");
                FuncoesECF.RelatorioGerencial("Demais Arquivos", " ");
                FuncoesECF.RelatorioGerencial("imprimir", " ");

                // Lendo lista de arquivos
                FileStream Arquivo = new FileStream(@Application.StartupPath + @"\lstArquivos.txt", FileMode.Open);
                StreamReader Ler = new StreamReader(Arquivo);

                string linha = null;
                while ((linha = Ler.ReadLine()) != null)
                {
                    FuncoesECF.RelatorioGerencial("imprimir", linha + Environment.NewLine);

                }
               
                Ler.Close();

                FuncoesECF.RelatorioGerencial("imprimir", "Ecfs Habilitados: " + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", " " + Environment.NewLine);
                XDocument xmlDoc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
                var listaECF = from cliente in xmlDoc.Descendants("ecf")
                               select cliente;

                foreach (var item in listaECF.Attributes("numeroFabricacaoCriptografado"))
                {
                    FuncoesECF.RelatorioGerencial("imprimir", Funcoes.DesCriptografarComSenha(Convert.FromBase64String(item.Value), GlbVariaveis.glbSenhaIQ) + Environment.NewLine);
                }              

                FuncoesECF.RelatorioGerencial("imprimir",@"MD5 principal: listaArquivos.txt ");
                FuncoesECF.RelatorioGerencial("imprimir", ConfiguracoesECF.md5Geral );

                FuncoesECF.RelatorioGerencial("fechar", "");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não é possível imprimir informação: " + erro.Message);
            };

        }

        private void btnPagamentos_Click(object sender, EventArgs e)
        {
            MeiosPagamento();
        }

        private void MeiosPagamento(bool arquivo=false)
        {
            if (rdbArquivo.Checked)
                arquivo = true;

            FuncoesECF fecf = new FuncoesECF();
            siceEntities entidade = Conexao.CriarEntidade();
            decimal dinheiro = 0;
            decimal cheque = 0;
            decimal crediario = 0;
            decimal ticket = 0;
            decimal cartaoDB = 0;
            decimal cartaoCR = 0;

            var pagamentos = (from c in entidade.caixaarquivo
                             where c.data >= dataInicial.Value.Date && c.data <= dataFinal.Value.Date                            
                             && c.estornado=="N"
                             //&& c.valor>0
                             select new
                             {
                                 c.tipopagamento,
                                 c.valor,
                                 c.data,
                                 c.vencimento,
                                 c.historico,
                                 c.coo,
                                 c.ecffabricacao,
                                 c.ccf,
                                 c.gnf,
                                 c.ecfmodelo,
                                 c.eaddados
                             });

            var pagamentosDia = (from c in entidade.caixa
                              where c.data >= dataInicial.Value.Date && c.data <= dataFinal.Value.Date
                              && c.estornado == "N"
                              //&& c.valor>0
                              select new
                              {
                                  c.tipopagamento,
                                  c.valor,
                                  c.data,
                                  c.vencimento,
                                  c.historico,
                                  c.coo,
                                  c.ecffabricacao,
                                  c.ccf,
                                  c.gnf,
                                  c.ecfmodelo,
                                  c.eaddados
                              });

            var listaPagamento = pagamentosDia.ToList();
            
            
            if (pagamentos!=null)
                listaPagamento = listaPagamento.Concat(pagamentos).ToList();

            var agrpData = (from n in listaPagamento
                            select n.data).Distinct();

            if (arquivo)
            {
                StringBuilder conteudo = new StringBuilder();
                //conteudo = RegistroU1("");


                foreach (var item in listaPagamento)
	                {
                    string tipoDoc = "1";
                    if (item.coo==null)
                        tipoDoc="2";                   
                    
                    string meioPagamento = item.tipopagamento.Trim().PadRight(25, ' ').Substring(0, 25);
                    var cripto = Funcoes.CriptografarMD5(item.ecffabricacao+item.coo+ item.ccf+ item.gnf+ item.ecfmodelo+ item.valor.ToString().Replace(",", ".")+ item.tipopagamento);

                    if(cripto!=item.eaddados)
                    {
                        meioPagamento = meioPagamento.Trim().PadRight(25, '?');
                    }

                    conteudo.AppendLine("A2" +
                    string.Format("{0:yyyyMMdd}", item.data) +
                    meioPagamento +
                    tipoDoc+
                    Funcoes.FormatarZerosEsquerda(item.valor, 12, true));
                	}

                arquivosPAF.Append(conteudo);
                return;

                using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\MeioPagamentos.txt"))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        sw.Write(@conteudo);
                    }
                };
                FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\MeioPagamentos.txt", false);
                txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\MeioPagamentos.txt";
                return;
            }

            FuncoesECF.RelatorioGerencial("abrir", "",GlbVariaveis.TipoRelatorioGerencial.MEIOSPGT.ToString());
            
            FuncoesECF.RelatorioGerencial("imprimir", "Periodo " + dataInicial.Value.Date + " a: " + dataFinal.Value.Date + Environment.NewLine);
            foreach (var item in agrpData)
            {
                dinheiro = (from n in listaPagamento
                            where n.tipopagamento == "DH"
                            && n.data == item.Value
                            select n.valor).Sum();
                cheque = (from n in listaPagamento
                            where n.tipopagamento == "CH"
                            && n.data == item.Value
                            select n.valor).Sum();
                crediario = (from n in listaPagamento
                            where n.tipopagamento == "CR"
                            && n.data == item.Value
                            select n.valor).Sum();
                ticket = (from n in listaPagamento
                            where n.tipopagamento == "TI"
                            && n.data == item.Value
                            select n.valor).Sum();
                cartaoCR = (from n in listaPagamento
                            where n.tipopagamento == "CA"
                            && n.data == item.Value
                            select n.valor).Sum();

                decimal? dinheiroNota = (from n in listaPagamento
                                    where n.tipopagamento == "DH"
                                    && n.historico == "Nota Fiscal"
                                    && n.data == item.Value
                                    select (decimal?)n.valor).Sum();
                decimal? dinheiroNFe = (from n in Conexao.CriarEntidade().contnfsaida
                                   where n.dataemissao == item.Value
                                   select (decimal?)n.totalNF).Sum();



                FuncoesECF.RelatorioGerencial("imprimir", "DATA " + item.ToString());
                if (dinheiro > 0)
                {
                    FuncoesECF.RelatorioGerencial("imprimir", "DH - Dinheiro   - CUPOM FISCAL " + string.Format("{0:n2}", dinheiro-dinheiroNota.GetValueOrDefault()-dinheiroNFe.GetValueOrDefault()) + Environment.NewLine);
                }

                if (dinheiroNota > 0)
                {
                    FuncoesECF.RelatorioGerencial("imprimir", "DH - Dinheiro   - NOTA FISCAL " + string.Format("{0:n2}", dinheiroNota) + Environment.NewLine);
                }

                if (dinheiroNFe > 0)
                {
                    FuncoesECF.RelatorioGerencial("imprimir", "DH - Dinheiro   - NOTA FISCAL " + string.Format("{0:n2}", dinheiroNFe) + Environment.NewLine);
                }

                if (cheque > 0)
                {
                    FuncoesECF.RelatorioGerencial("imprimir", "CH - Cheque     - CUPOM FISCAL  " + string.Format("{0:n2}", cheque) + Environment.NewLine);
                }
                if (crediario > 0)
                {
                    FuncoesECF.RelatorioGerencial("imprimir", "CR - Crediário  - CUPOM FISCAL  " + string.Format("{0:n2}", crediario) + Environment.NewLine);
                }
                if (cartaoDB > 0)
                {
                    FuncoesECF.RelatorioGerencial("imprimir", "CA - Cartao DB  - CUPOM FISCAL  " + string.Format("{0:n2}", cartaoDB) + Environment.NewLine);
                }
                if (cartaoCR > 0)
                {
                    FuncoesECF.RelatorioGerencial("imprimir", "CA - Cartao CR  - CUPOM FISCAL  " + string.Format("{0:n2}", cartaoCR) + Environment.NewLine);
                }
                if (ticket > 0)
                {
                    FuncoesECF.RelatorioGerencial("imprimir", "TI - Ticket     - CUPOM FISCAL  " + string.Format("{0:n2}", ticket) + Environment.NewLine);
                }

                FuncoesECF.RelatorioGerencial("imprimir", "SOMA DO DIA     " + item.Value + "  " + (dinheiro + cheque + crediario + cartaoDB + cartaoCR + ticket).ToString());                
            }


             dinheiro = 0;
             cheque = 0;
             crediario = 0;
             ticket = 0;
             cartaoDB = 0;
             cartaoCR = 0;

            foreach (var item in listaPagamento)
            {
                if (item.tipopagamento == "DH")
                    dinheiro += item.valor; // +item.troco;
                if (item.tipopagamento == "CH")
                    cheque += item.valor;
                if (item.tipopagamento == "CR")
                    crediario += item.valor;
                if (item.tipopagamento == "TI")
                    ticket+=item.valor;
                if (item.tipopagamento == "CA" && (item.vencimento.Value.Date.Subtract(item.data.Value.Date).Days <= 1))
                    cartaoDB += item.valor;
                if (item.tipopagamento == "CA" && (item.vencimento.Value.Date.Subtract(item.data.Value.Date).Days > 2))
                    cartaoCR += item.valor;

               // conteudo = item.tipopagamento+" Cupom Fiscal "+string.Format("{0:n2}",item.valor)+" "+string.Format("{0:dd/MM/yyyy}",item.data)+Environment.NewLine;
               // FuncoesECF.RelatorioGerencial("imprimir", conteudo);
            }
            // Imprimir os comprovantes Não Fiscais Sangria e Saldo Inicial
            //var suprimento = (from c in entidade.caixaarquivo
            //                  where (c.tipopagamento=="SI" || c.tipopagamento=="SU")
            //                  && c.data >= dataInicial.Value.Date && c.data <= dataFinal.Value.Date
            //                  && c.historico.Substring(0,1)=="E" // Aqui por comeca com o numero do ECF
            //                  //&& c.valor > 0
            //                  select new
            //                  {
            //                      c.tipopagamento,
            //                      c.valor,
            //                      c.data                                  
            //                  }).Concat(from c in entidade.caixa
            //                            where (c.tipopagamento == "SI" || c.tipopagamento == "SU")
            //                            && c.data >= dataInicial.Value.Date && c.data <= dataFinal.Value.Date
            //                            && c.historico.Substring(0, 1) == "E" // Aqui por comeca com o numero do ECF
            //                            //&& c.valor > 0
            //                            select new
            //                            {
            //                                c.tipopagamento,
            //                                c.valor,
            //                                c.data                                        
            //                            });
            //foreach (var item in suprimento)
            //{
            //    dinheiro += item.valor;
            //    conteudo = "DH CNF SUP "+ string.Format("{0:n2}", item.valor) + " " + string.Format("{0:dd/MM/yyyy}", item.data) + Environment.NewLine;
            //    FuncoesECF.RelatorioGerencial("imprimir", conteudo);                
            //}

            //var sangria = from s in entidade.movdespesas
            //              where s.data >= dataInicial.Value.Date && s.data <= dataFinal.Value.Date
            //              && s.ecfnumero != ""
            //              select new
            //              {
            //                  s.valor,
            //                  s.data
            //              };
            //foreach (var item in sangria)
            //{
            //    //dinheiro += item.valor;
            //    conteudo = "DH" + " CNF SANG " + string.Format("{0:n2}", item.valor) + " " + string.Format("{0:dd/MM/yyyy}", item.data) + Environment.NewLine;
            //    FuncoesECF.RelatorioGerencial("imprimir", conteudo);                  
            //}

            var total = dinheiro+cheque+cartaoDB+cartaoCR+ticket;

            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "TOTALIZAÇÃO" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "DH - Dinheiro   " + string.Format("{0:n2}", dinheiro) + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "CH - Cheque     " + string.Format("{0:n2}", cheque) + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "CR - Crediário  " + string.Format("{0:n2}", crediario) + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "CA - Cartao DB  " + string.Format("{0:n2}", cartaoDB) + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "CA - Cartao CR  " + string.Format("{0:n2}", cartaoCR) + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "TI - Ticket     " + string.Format("{0:n2}", ticket) + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "SOMA TOTAL R$:" + string.Format("{0:n2}", total) + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("fechar", "");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VendasPorPeriodo();
        }
        //private void VendasPorPeriodo()
        //{
        //     FrmMsgOperador msg = new FrmMsgOperador("", "Gerando SINTEGRA ICMS 57/95 \n\rPode demorar vários minutos. Não desligue o ECF.");
        //     msg.Show();
        //    try
        //    {
        //        Registro61Sintegra();

        //        Application.DoEvents();
        //        FuncoesECF.GerarSintegraECF(dataInicial.Value.Date.Month, dataInicial.Value.Date.Year, dataInicial.Value, dataFinal.Value);
        //        XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\sHouse.xml");
        //        var dados = (from n in doc.Descendants("sHouse").Elements("Dados")
        //                     select n).First();
        //        string laudo = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("laudo").First().Value), GlbVariaveis.glbSenhaIQ);
        //        string arquivo = laudo.Trim() + String.Format("{0:ddMMyyyy}", DateTime.Now.Date) + String.Format("{0:HHmmss}", DateTime.Now) + ".txt";
        //        txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\" + arquivo;                
        //        File.Copy(@ConfiguracoesECF.pathRetornoECF + @"sintegra.txt", txtDiretorio.Text,true);
        //        Paf.AcrescentarRegistro61Sintegra(@txtDiretorio.Text);
        //        FuncoesECF.AssinarArquivo(@txtDiretorio.Text, true);
        //    }
        //    catch (Exception erro)
        //    {
        //        MessageBox.Show("Não foi possível gerar Sintegra agora: "+erro.Message, "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        msg.Dispose();
        //    }
        //}

        private void VendasPorPeriodo()
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando SINTEGRA ICMS 57/95 \n\rPode demorar vários minutos.");
            msg.Show();
            Application.DoEvents();
            try
            {
                //XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\sHouse.xml");
                //        var dados = (from n in doc.Descendants("sHouse").Elements("Dados")
                //                     select n).First();
                        string laudo = GlbVariaveis.laudoPAF;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("laudo").First().Value), GlbVariaveis.glbSenhaIQ);
                        string arquivo = laudo.Trim() + String.Format("{0:ddMMyyyy}", DateTime.Now.Date) + String.Format("{0:HHmmss}", DateTime.Now) + ".txt";
                        txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\" + arquivo;                
                
                Sintegra sintegra = new Sintegra();
                sintegra.dataInicial = dataInicial.Value.Date;
                sintegra.dataFinal = dataFinal.Value.Date;
                sintegra.nomeArquivo = txtDiretorio.Text;
                sintegra.GerarPAF();
                FuncoesECF.AssinarArquivo(@txtDiretorio.Text,false);
                
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível gerar Sintegra agora: " + erro.Message, "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                msg.Dispose();
            }
        }

        private void Registro61Sintegra()
        {
            siceEntities entidade = Conexao.CriarEntidade();
            siceEntities entidade2 = Conexao.CriarEntidade();
            siceEntities entidade3 = Conexao.CriarEntidade();

            var mestre = from n in entidade.contdocs
                         where n.data>=dataInicial.Value.Date && n.data<=dataFinal.Value.Date
                         && (n.modeloDOCFiscal=="02" || n.modeloDOCFiscal=="D1")
                         select n;
            if (mestre.Count()==0)
                return;
           
            var numeroFinal = (from n in mestre 
                                select n.nrnotafiscal).Max();

            var numeroInicial = (from n in mestre
                                 select n.nrnotafiscal).Min();

            StringBuilder conteudo = new StringBuilder();
            
            foreach (var itemMestre in mestre)
	        {             
                var dadosItens = from n in entidade3.blococregc390
                                 where n.documento== itemMestre.documento
                                 select n;

                foreach (var item in dadosItens)
	                    {
                            string modDoc = itemMestre.modeloDOCFiscal.Trim();                            
                 
                            conteudo.AppendLine("61" + //01
                                " ".PadRight(14, ' ').Substring(0, 14)+
                                " ".PadRight(14, ' ').Substring(0, 14) +
                                string.Format("{0:yyyyMMdd}", item.data) +
                                 modDoc.Trim().PadRight(2, ' ').Substring(0, 2) +
                                 "D  "+// itemMestre.serienf.Trim().PadRight(3, ' ').Substring(0, 3) +
                                 itemMestre.subserienf.Trim().PadRight(2, ' ').Substring(0, 2) +
                                 numeroInicial.ToString().PadRight(6, '0').Substring(0, 6) +
                                 numeroFinal.ToString().PadRight(6, '0').Substring(0, 6) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.total), 13, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.baseCalculoICMS), 13, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.totalICMS), 12, true) +
                                 Funcoes.FormatarZerosEsquerda(0, 13, true) +
                                 Funcoes.FormatarZerosEsquerda(0, 13, true) +
                                 Funcoes.FormatarZerosEsquerda(item.icms, 4, true)+
                                 " ");
                       };
	        };


            var resumo = from n in entidade.blococregc425
                         where n.data >= dataInicial.Value.Date && n.data <= dataFinal.Value.Date
                         && (n.modelodocfiscal == "D1" || n.modelodocfiscal == "02")                        
                         select n;
            foreach (var item in resumo)
            {
                conteudo.AppendLine("61" +
                    "R" +
                    string.Format("{0:yyyyMMdd}", item.data).Substring(0, 6) +
                    item.codigo.Trim().PadRight(14, ' ').Substring(0, 14) +                    
                    Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.quantidade), 13, true)+
                    Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.total), 16, true) +
                    Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.total), 16, true) +
                    Funcoes.FormatarZerosEsquerda(item.icms, 4, true) +
                    " ".PadRight(54, ' ').Substring(0, 54));
            }


            using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\Registro61.txt"))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                {
                    sw.Write(@conteudo);
                }
            };

         
        }

        private void btnMovECF_Click(object sender, EventArgs e)
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Movimento por ECF, pode demorar alguns instantes");
            msg.Show();
            Application.DoEvents();
            try
            {
                Paf movECF = new Paf();
                if (txtECF.Text != "")
                    txtECF.Text = "001";
                string arquivoDestino = FuncoesPAFECF.CodNacionaciolECF()+ConfiguracoesECF.nrFabricacaoECF.Trim().PadLeft(20, '0').Substring(6, 14) + String.Format("{0:ddMMyyyy}", DateTime.Now.Date) + ".txt";

                movECF.GerarMovimentoPorECF(true,dataInicial.Value.Date, dataFinal.Value.Date, txtECF.Text, @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirMovimentoECF"] + @"\"+arquivoDestino);

                txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirMovimentoECF"] + @"\"+arquivoDestino;
            }
            catch (Exception ocorrencia)
            {
                MessageBox.Show(ocorrencia.Message);
            }
            finally
            {
                msg.Dispose();
            }
        }

        //private void DavEmitidos(object sender, EventArgs e)
        //{
        //    siceEntities entidade = Conexao.CriarEntidade();

        //    var dados = from n in entidade.contdocs
        //                where n.data >= dataInicial.Value.Date && n.data <= dataFinal.Value.Date
        //                && n.CodigoFilial == GlbVariaveis.glb_filial
        //                && n.davnumero > 0
        //                select new
        //                {
        //                    numeroFabECF = n.ecffabricacao,
        //                    MFAdicional = n.ecfMFadicional,
        //                    tipoECF = n.ecftipo,
        //                    marcaECF = n.ecfmarca,
        //                    modeloECF = n.ecfmodelo,
        //                    COO = n.ncupomfiscal, // O número do cupom fiscal COO
        //                    CCF = n.ecfcontadorcupomfiscal,
        //                    DAVNumero = n.davnumero,
        //                    data = n.data,
        //                    titulo = "ORÇAMENTO",
        //                    valor = n.total,
        //                    COOVinculado = n.contadordebitocreditoCDC,
        //                    ead = n.EADRegistroDAV,
        //                    nome = n.nome,
        //                    cnpjcpf = n.ecfCPFCNPJconsumidor == null ? "" : n.ecfCPFCNPJconsumidor
        //                };


        //    if (dados.Count() == 0)
        //    {
        //        MessageBox.Show("Não existe DAV emitidos no período selecionado");
        //        return;
        //    }

        //    if (rdbArquivo.Checked)
        //    {
        //        try
        //        {


        //            #region DAV Arquivo
        //            StringBuilder conteudo = new StringBuilder();

        //            conteudo.AppendLine("D1" +
        //                Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
        //                Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
        //                Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
        //                Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50));


        //            int nRegistrosD2 = 0;
        //            foreach (var item in dados)
        //            {
        //                var cripto = Funcoes.CriptografarMD5(item.COO + item.DAVNumero.ToString() + string.Format("{0:yyyy-MM-dd}", item.data.Value) + item.valor.ToString().Replace(",", "."));
        //                var numeroFabECF = item.numeroFabECF.Trim().PadRight(20, ' ').Substring(0, 20);
        //                var modeloECF = item.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);

        //                if (cripto != item.ead)
        //                    modeloECF = "????????????????????";

        //                conteudo.AppendLine("D2" +
        //                Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) + //02
        //                    //item.numeroFabECF.Trim().PadRight(20, ' ').Substring(0, 20) + //02
        //                    " ".PadRight(20, ' ').Substring(0, 20) + //02
        //                    //item.MFAdicional.Trim().PadRight(1, ' ').Substring(0, 1) + //03
        //                    " " +
        //                    //item.tipoECF.Trim().PadRight(7, ' ').Substring(0, 7) + //05
        //                    " ".PadRight(7, ' ').Substring(0, 7) + //05
        //                    //item.marcaECF.Trim().PadRight(20, ' ').Substring(0, 20) + //06
        //                    " ".PadRight(20, ' ').Substring(0, 20) + //06
        //                    //modeloECF + //07
        //                     " ".PadRight(20, ' ').Substring(0, 20) + //07
        //                    //item.CCF.Trim().PadRight(6, '0').Substring(0, 6) +
        //                    "0".PadRight(6, '0').Substring(0, 6) +
        //                    Funcoes.FormatarZerosEsquerda(item.DAVNumero.Value, 13, false) + // item.DAVNumero.ToString().Trim().PadRight(13, ' ').Substring(0, 13) +
        //                    string.Format("{0:yyyyMMdd}", item.data) +
        //                    item.titulo.Trim().PadRight(30, ' ').Substring(0, 30) +
        //                    Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.valor.Value), 08, true) +
        //                    //item.COO.Trim().PadRight(6, '0').Substring(0, 6) +                            
        //                    "0".PadRight(6, '0').Substring(0, 6) +
        //                    //item.modeloECF.PadRight(3,'0').Substring(0,3)+
        //                    "0".PadRight(3, '0').Substring(0, 3) +
        //                    item.nome.PadRight(40, ' ').Substring(0, 40) +
        //                    item.cnpjcpf.PadLeft(14, '0').Substring(0, 14)
        //                );
        //                nRegistrosD2++;
        //            }


        //            int nRegistrosD3 = 0;
        //            foreach (var item in dados)
        //            {
        //                var itensDAV = from n in Conexao.CriarEntidade().vendadav
        //                               where n.documento == item.DAVNumero
        //                               && n.codigofilial == GlbVariaveis.glb_filial
        //                               select new
        //                               {
        //                                   n.data,
        //                                   n.nrcontrole,
        //                                   n.codigo,
        //                                   n.produto,
        //                                   n.quantidade,
        //                                   n.unidade,
        //                                   n.preco,
        //                                   n.descontovalor,
        //                                   n.acrescimototalitem,
        //                                   n.total,
        //                                   n.tipo,
        //                                   n.tributacao,
        //                                   n.icms,
        //                                   n.cancelado
        //                               };


        //                foreach (var itens in itensDAV)
        //                {

        //                    try
        //                    {
        //                        conteudo.AppendLine("D3" +
        //                         Funcoes.FormatarZerosEsquerda(item.DAVNumero.Value, 13, false) +
        //                         string.Format("{0:yyyyMMdd}", itens.data) +
        //                         itens.nrcontrole.ToString().PadLeft(3, '0').Substring(0, 3) +
        //                         itens.codigo.PadRight(14, ' ').Substring(0, 14) +
        //                         itens.produto.PadRight(100, ' ').Substring(0, 100) +
        //                         Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.quantidade), 07, true) +
        //                         itens.unidade.PadRight(3, ' ').Substring(0, 3) +
        //                         Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.preco), 08, true) +
        //                         Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.descontovalor), 08, true) +
        //                         Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.acrescimototalitem), 08, true) +
        //                         Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.total), 14, true) +
        //                         SituacaoTributacao(itens.icms, itens.tributacao, itens.tipo) +
        //                         Funcoes.FormatarZerosEsquerda(itens.icms, 4, true) +
        //                         itens.cancelado +
        //                         "3" +
        //                         "2");
        //                        nRegistrosD3++;
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show(ex.InnerException.ToString());
        //                    }
        //                }
        //            }

        //            conteudo.AppendLine("D9" +
        //            Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
        //            Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
        //            string.Format("{0:000000}", nRegistrosD2) +
        //            string.Format("{0:000000}", nRegistrosD3)
        //            );

        //            using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\DAVEmitidos.txt"))
        //            {
        //                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
        //                {
        //                    sw.Write(@conteudo);
        //                }
        //            };

        //            FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\DAVEmitidos.txt", false);
        //            txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\DAVEmitidos.txt";
        //            #endregion
        //        }
        //        catch (Exception erro)
        //        {
        //            MessageBox.Show(erro.InnerException.ToString());
        //        }

        //    };

        //    if (rdbImpressora.Checked)
        //    {
        //        #region DAV Impressao
        //        FuncoesECF.RelatorioGerencial("abrir", "", GlbVariaveis.TipoRelatorioGerencial.DAVEMITIDOS.ToString());
        //        string conteudo = "";
        //        foreach (var item in dados)
        //        {
        //            conteudo = Funcoes.FormatarZerosEsquerda(item.DAVNumero.Value, 10, false) + " " + string.Format("{0:dd/MM/yyyy}", item.data) + " ORÇAMENTO " + string.Format("{0:n2}", item.valor) + " " + item.CCF + Environment.NewLine;
        //            FuncoesECF.RelatorioGerencial("imprimir", conteudo);
        //        }
        //        FuncoesECF.RelatorioGerencial("fechar", "");
        //        #endregion
        //    }

        //}

        private void DAVEmitidos()
        {
            siceEntities entidade = Conexao.CriarEntidade();
            var dados = from n in entidade.contdav
                        where n.data >= dataInicial.Value.Date && n.data <= dataFinal.Value.Date
                        select new
                        {
                            numeroFabECF = n.ecffabricacao,
                            MFAdicional = " ",
                            tipoECF = ConfiguracoesECF.tipoECF,
                            marcaECF = n.marca,
                            modeloECF = n.modelo,
                            COO = n.ncupomfiscal == null ? " " : n.ncupomfiscal, // O número do cupom fiscal COO
                            CCF = n.ecfcontadorcupomfiscal == null ? " " : n.ecfcontadorcupomfiscal,
                            DAVNumero = n.numeroDAVFilial,
                            data = n.data,
                            titulo = "ORÇAMENTO",
                            valor = n.valor,
                            COOVinculado = "",
                            ead = n.EADRegistroDAV,
                            nome = n.cliente,
                            cnpjcpf = n.ecfCPFCNPJconsumidor == null ? "" : n.ecfCPFCNPJconsumidor,
                            n.finalizada,
                            ecfNumero = n.numeroECF == null ? "001" : n.numeroECF,
                            n.contadorRGECF
                        };

            
            if (dados.Count() == 0)
            {
                MessageBox.Show("Não existe DAV emitidos no período selecionado");
                return;
            }

            if (rdbArquivo.Checked)
            {
                try
                {


                    #region DAV Arquivo
                    StringBuilder conteudo = new StringBuilder();

                    var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);
                    if (!FuncoesPAFECF.VerificarQtdRegistro("contdav"))
                    {
                        razao = razao.Trim().PadRight(50, '?');
                    }

                    if (!FuncoesPAFECF.VerificarQtdRegistro("vendadav"))
                    {
                        razao = razao.Trim().PadRight(50, '?');
                    }

                   // conteudo = RegistroU1("");
                    //Não existe mais
                    /*
                    conteudo.AppendLine("D1" +
                        Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                        Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                        Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                        razao);
                    */

                    int nRegistrosD2 = 0;
                    foreach (var item in dados)
                    {
                        
                        var cripto = Funcoes.CriptografarMD5(item.COO + item.DAVNumero.ToString() + string.Format("{0:yyyy-MM-dd}", item.data.Value) + item.valor.ToString().Replace(",", ".") + item.ecfNumero + item.contadorRGECF + item.nome + item.cnpjcpf);                        
                        var numeroFabECF = item.numeroFabECF.Trim().PadRight(20, ' ').Substring(0, 20);
                        var modeloECF = item.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);
                        
                        string marcaECF = item.marcaECF;
                        string tipoECF = item.tipoECF;
                        string ecfFabricacao = item.numeroFabECF;
                        string mfAdicional = ConfiguracoesECF.mfAdicionalECF;

                        if (ecfFabricacao == "" || ecfFabricacao == null)
                        {
                                marcaECF=" ";
                                tipoECF=" ";
                                modeloECF = " ";
                                mfAdicional = " ";
                        }

                        
                        string contadorRG = "0";
                        string numeroECF = "0";                        


                        if (!string.IsNullOrEmpty(item.CCF))
                        {
                            modeloECF = " ";

                        }
                        if (cripto != item.ead)
                        {

                            modeloECF = modeloECF.Trim().PadRight(20, '?');// "????????????????????";
                            numeroECF = item.ecfNumero;
                            contadorRG = item.contadorRGECF.Replace(" ", "");
                        }


                        conteudo.AppendLine("D2" +
                        Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) + //02                             
                            ecfFabricacao.PadRight(20, ' ').Substring(0, 20) + //02                           
                            mfAdicional +
                            tipoECF.PadRight(7, ' ').Substring(0, 7) + //05                            
                            marcaECF.PadRight(20, ' ').Substring(0, 20) + //06                            
                            modeloECF.Trim().PadRight(20, ' ').Substring(0, 20) + //??????????07                            
                            contadorRG.PadLeft(6, '0').Substring(0, 6) +
                            Funcoes.FormatarZerosEsquerda(item.DAVNumero, 10, false) + "   " + // item.DAVNumero.ToString().Trim().PadRight(13, ' ').Substring(0, 13) +
                            string.Format("{0:yyyyMMdd}", item.data) +
                            item.titulo.Trim().PadRight(30, ' ').Substring(0, 30) +
                            Funcoes.FormatarZerosEsquerda(item.valor, 08, true) +
                            item.COO.PadRight(6, '0').Substring(0, 6) +
                            numeroECF.PadRight(3, '0').Substring(0, 3) +
                            item.nome.PadRight(40, ' ').Substring(0, 40) +
                            item.cnpjcpf.PadLeft(14, '0').Substring(0, 14)
                        );
                        nRegistrosD2++;
                    }


                    int nRegistrosD3 = 0;
                    foreach (var item in dados)
                    {                        
                        var itensDAV = from n in Conexao.CriarEntidade().vendadav
                                       where n.documento == item.DAVNumero
                                       && n.codigofilial == GlbVariaveis.glb_filial
                                       select new
                                       {
                                           n.data,
                                           n.nrcontrole,
                                           n.codigo,
                                           n.produto,
                                           n.quantidade,
                                           n.unidade,
                                           n.preco,
                                           descontovalorTotal = (n.descontovalor * n.quantidade),
                                           n.acrescimototalitem,
                                           n.total,
                                           n.tipo,
                                           n.tributacao,
                                           n.icms,
                                           n.cancelado,
                                           n.documento,
                                           n.eaddados,
                                           n.Descontoperc,
                                           n.descontovalor,
                                           n.ccf,
                                           n.coo,
                                           n.ecffabricacao
                                       };


                        foreach (var itens in itensDAV)
                        {

                            var cripto = Funcoes.CriptografarMD5(itens.documento.ToString().Replace(",", ".") + string.Format("{0:yyyy-MM-dd}", itens.data.Value) + itens.nrcontrole.ToString().Replace(",", ".") + itens.codigo + itens.produto + itens.quantidade.ToString().Replace(",", ".") + itens.unidade + itens.preco.ToString().Replace(",", ".") + itens.descontovalor.ToString().Replace(",", ".") + itens.acrescimototalitem.ToString().Replace(",", ".") + itens.total.ToString().Replace(",", ".") + itens.tributacao.ToString() + itens.Descontoperc.ToString().Replace(",", ".") + itens.cancelado + itens.icms.ToString().Replace(",", ".") + itens.ccf + itens.coo + itens.ecffabricacao);


                            var descricaoPrd = itens.produto;
                            if (cripto != itens.eaddados)
                            {
                                descricaoPrd = itens.produto.Trim().PadRight(100, '?'); // "????????????????????????????????????????????????????????????????????????????????????????????????????";

                            }
                            try
                            {
                                conteudo.AppendLine("D3" +
                                 Funcoes.FormatarZerosEsquerda(item.DAVNumero, 13, false) +
                                 string.Format("{0:yyyyMMdd}", itens.data) +
                                 itens.nrcontrole.ToString().PadLeft(3, '0').Substring(0, 3) +
                                 itens.codigo.PadRight(14, ' ').Substring(0, 14) +
                                 descricaoPrd.PadRight(100, ' ').Substring(0, 100) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.quantidade), 07, true) +
                                 itens.unidade.PadRight(3, ' ').Substring(0, 3) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.preco), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.descontovalorTotal), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.acrescimototalitem), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.total), 14, true) +
                                 SituacaoTributacao(itens.icms, itens.tributacao, itens.tipo) +
                                 Funcoes.FormatarZerosEsquerda(itens.icms, 4, true) +
                                 itens.cancelado +
                                 "3" +
                                 "2");
                                nRegistrosD3++;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.InnerException.ToString());
                            }
                        }
                    }
                    //Não existe mais
                    /*
                    conteudo.AppendLine("D9" +
                    Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                    Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                    string.Format("{0:000000}", nRegistrosD2) +
                    string.Format("{0:000000}", nRegistrosD3)
                    );
                    */
                    arquivosPAF.Append(conteudo);
                    return;


                    using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\DAVEmitidos.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            sw.Write(@conteudo);
                        }
                    };

                    FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\DAVEmitidos.txt", false);
                    txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\DAVEmitidos.txt";
                    #endregion
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.InnerException.ToString());
                }

            };

            if (rdbImpressora.Checked)
            {
                #region DAV Impressao
                FuncoesECF.RelatorioGerencial("abrir", "", GlbVariaveis.TipoRelatorioGerencial.DAVEMITIDOS.ToString());
                string conteudo = "";
                FuncoesECF.RelatorioGerencial("imprimir", "DAV.NR     Data       Tipo     Valor      COO " + Environment.NewLine);
                foreach (var item in dados)
                {
                    conteudo = Funcoes.FormatarZerosEsquerda(item.DAVNumero, 10, false) + " " + string.Format("{0:dd/MM/yyyy}", item.data) + " ORÇAMENTO " + string.Format("{0:n2}", item.valor) + "    " + item.COO + Environment.NewLine;
                    FuncoesECF.RelatorioGerencial("imprimir", conteudo);
                }
                FuncoesECF.RelatorioGerencial("fechar", "");
                #endregion
            }
        }

        private void DAVOSEmitidos()
        {
            siceEntities entidade = Conexao.CriarEntidade();
            var dados = from n in entidade.contdavos
                        where n.data >= dataInicial.Value.Date && n.data <= dataFinal.Value.Date
                        select new
                        {
                            numeroFabECF = "",
                            MFAdicional = " ",
                            tipoECF = " ",
                            marcaECF = " ",
                            modeloECF = " ",
                            COO = n.ncupomfiscal == null ? " " : n.ncupomfiscal, // O número do cupom fiscal COO
                            CCF = n.ecfcontadorcupomfiscal == null ? " " : n.ecfcontadorcupomfiscal,
                            DAVNumero = n.numero,
                            data = n.data,
                            titulo = "ORÇAMENTO",
                            valor = n.valor,
                            COOVinculado = "",
                            ead = n.EADRegistroDAV,
                            nome = n.cliente,
                            cnpjcpf = n.ecfCPFCNPJconsumidor == null ? "" : n.ecfCPFCNPJconsumidor,
                            n.finalizada,
                            ecfNumero = n.numeroECF == null ? "001" : n.numeroECF,
                            n.contadorRGECF
                        };


            if (dados.Count() == 0)
            {
                MessageBox.Show("Não existe DAV emitidos no período selecionado");
                return;
            }

            if (rdbArquivo.Checked)
            {
                try
                {


                    #region DAV Arquivo
                    StringBuilder conteudo = new StringBuilder();

                    var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);
                    if (!FuncoesPAFECF.VerificarQtdRegistro("contdavos"))
                    {
                        razao = razao.Trim().PadRight(50, '?');
                    }

                    if (!FuncoesPAFECF.VerificarQtdRegistro("vendadavos"))
                    {
                        razao = razao.Trim().PadRight(50, '?');
                    }

                    //Não existe mais
                    /*
                    conteudo.AppendLine("D1" +
                        Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                        Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                        Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                        razao);
                    */

                    int nRegistrosD2 = 0;
                    foreach (var item in dados)
                    {
                        var cripto = Funcoes.CriptografarMD5(item.COO + item.DAVNumero.ToString() + string.Format("{0:yyyy-MM-dd}", item.data.Value) + item.valor.ToString().Replace(",", ".") + item.ecfNumero + item.contadorRGECF + item.nome + item.cnpjcpf);
                        var numeroFabECF = item.numeroFabECF.Trim().PadRight(20, ' ').Substring(0, 20);
                        var modeloECF = item.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);

                        string marcaECF = item.marcaECF  ;
                        string tipoECF = item.tipoECF;
                        string ecfFabricacao = item.numeroFabECF;
                        string mfAdicional = ConfiguracoesECF.mfAdicionalECF;
                        if (ecfFabricacao == "")
                            mfAdicional = " ";
                        string contadorRG = "0";
                        string numeroECF = "0";

                        if (!string.IsNullOrEmpty(item.CCF))
                        {
                            modeloECF = " ";

                        }
                        if (cripto != item.ead)
                        {
                            modeloECF = modeloECF.Trim().PadRight(20, '?');// "????????????????????";
                            numeroECF = item.ecfNumero;
                            contadorRG = item.contadorRGECF.Replace(" ", "");
                        }


                        conteudo.AppendLine("D2" +
                        Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) + //02                             
                            ecfFabricacao.PadRight(20, ' ').Substring(0, 20) + //02                           
                            mfAdicional +
                            tipoECF.PadRight(7, ' ').Substring(0, 7) + //05                            
                            marcaECF.PadRight(20, ' ').Substring(0, 20) + //06                            
                            modeloECF.Trim().PadRight(20, ' ').Substring(0, 20) + //07                            
                            contadorRG.PadLeft(6, '0').Substring(0, 6) +
                            Funcoes.FormatarZerosEsquerda(item.DAVNumero, 10, false) + "   " + // item.DAVNumero.ToString().Trim().PadRight(13, ' ').Substring(0, 13) +
                            string.Format("{0:yyyyMMdd}", item.data) +
                            item.titulo.Trim().PadRight(30, ' ').Substring(0, 30) +
                            Funcoes.FormatarZerosEsquerda(item.valor, 08, true) +
                            item.COO.PadRight(6, '0').Substring(0, 6) +
                            numeroECF.PadRight(3, '0').Substring(0, 3) +
                            item.nome.PadRight(40, ' ').Substring(0, 40) +
                            item.cnpjcpf.PadLeft(14, '0').Substring(0, 14)
                        );
                        nRegistrosD2++;
                    }


                    int nRegistrosD3 = 0;
                    foreach (var item in dados)
                    {
                        var itensDAV = from n in Conexao.CriarEntidade().vendadavos
                                       where n.documento == item.DAVNumero
                                       && n.codigofilial == GlbVariaveis.glb_filial
                                       select new
                                       {
                                           n.data,
                                           n.nrcontrole,
                                           n.codigo,
                                           n.produto,
                                           n.quantidade,
                                           n.unidade,
                                           n.preco,
                                           descontovalorTotal = (n.descontovalor * n.quantidade),
                                           n.acrescimototalitem,
                                           n.total,
                                           n.tipo,
                                           n.tributacao,
                                           n.icms,
                                           n.cancelado,
                                           n.documento,
                                           n.eaddados,
                                           n.Descontoperc,
                                           n.descontovalor,
                                           n.ccf,
                                           n.coo,
                                           n.ecffabricacao
                                       };


                        foreach (var itens in itensDAV)
                        {

                            var cripto = Funcoes.CriptografarMD5(itens.documento.ToString().Replace(",", ".") + string.Format("{0:yyyy-MM-dd}", itens.data.Value) + itens.nrcontrole.ToString().Replace(",", ".") + itens.codigo + itens.produto + itens.quantidade.ToString().Replace(",", ".") + itens.unidade + itens.preco.ToString().Replace(",", ".") + itens.descontovalor.ToString().Replace(",", ".") + itens.acrescimototalitem.ToString().Replace(",", ".") + itens.total.ToString().Replace(",", ".") + itens.tributacao.ToString() + itens.Descontoperc.ToString().Replace(",", ".") + itens.cancelado + itens.icms.ToString().Replace(",", ".") + itens.ccf + itens.coo + itens.ecffabricacao);


                            var descricaoPrd = itens.produto;
                            if (cripto != itens.eaddados)
                            {
                                descricaoPrd = itens.produto.Trim().PadRight(100, '?'); // "????????????????????????????????????????????????????????????????????????????????????????????????????";

                            }
                            try
                            {
                                conteudo.AppendLine("D3" +
                                 Funcoes.FormatarZerosEsquerda(item.DAVNumero, 13, false) +
                                 string.Format("{0:yyyyMMdd}", itens.data) +
                                 itens.nrcontrole.ToString().PadLeft(3, '0').Substring(0, 3) +
                                 itens.codigo.PadRight(14, ' ').Substring(0, 14) +
                                 descricaoPrd.PadRight(100, ' ').Substring(0, 100) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.quantidade), 07, true) +
                                 itens.unidade.PadRight(3, ' ').Substring(0, 3) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.preco), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.descontovalorTotal), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.acrescimototalitem), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.total), 14, true) +
                                 SituacaoTributacao(itens.icms, itens.tributacao, itens.tipo) +
                                 Funcoes.FormatarZerosEsquerda(itens.icms, 4, true) +
                                 itens.cancelado +
                                 "3" +
                                 "2");
                                nRegistrosD3++;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.InnerException.ToString());
                            }
                        }
                    }
                    //Não existe mais
                    /*
                    conteudo.AppendLine("D9" +
                    Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                    Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                    string.Format("{0:000000}", nRegistrosD2) +
                    string.Format("{0:000000}", nRegistrosD3)
                    );
                    */
                    using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\DAVEmitidos.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            sw.Write(@conteudo);
                        }
                    };

                    FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\DAVEmitidos.txt", false);
                    txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\DAVEmitidos.txt";
                    #endregion
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.InnerException.ToString());
                }

            };

            if (rdbImpressora.Checked)
            {
                #region DAV OS Impressao
                FuncoesECF.RelatorioGerencial("abrir", "", GlbVariaveis.TipoRelatorioGerencial.DAVOSEMITIDOS.ToString());
                string conteudo = "";
                FuncoesECF.RelatorioGerencial("imprimir", "DAV-OS     Data       Tipo     Valor      COO " + Environment.NewLine);
                foreach (var item in dados)
                {
                    conteudo = Funcoes.FormatarZerosEsquerda(item.DAVNumero, 10, false) + " " + string.Format("{0:dd/MM/yyyy}", item.data) + " CONTA CLIENTE " + string.Format("{0:n2}", item.valor) + "    " + item.COO + Environment.NewLine;
                    FuncoesECF.RelatorioGerencial("imprimir", conteudo);
                }
                FuncoesECF.RelatorioGerencial("fechar", "");
                #endregion
            }
        }

        private void DAVLogAlteracao()
        {
            siceEntities entidade = Conexao.CriarEntidade();
            var dados = from n in entidade.contdav
                        where n.data >= dataInicial.Value.Date && n.data <= dataFinal.Value.Date
                        select new
                        {
                            numeroFabECF = n.ecffabricacao,
                            MFAdicional = " ",
                            tipoECF =  " ",
                            marcaECF = n.marca,
                            modeloECF = n.modelo,
                            COO = n.ncupomfiscal == null ? " " : n.ncupomfiscal, // O número do cupom fiscal COO
                            CCF = n.ecfcontadorcupomfiscal == null ? " " : n.ecfcontadorcupomfiscal,
                            DAVNumero = n.numeroDAVFilial,
                            data = n.data,
                            titulo = "ORÇAMENTO",
                            valor = n.valor,
                            COOVinculado = "",
                            ead = n.EADRegistroDAV,
                            nome = n.cliente,
                            cnpjcpf = n.ecfCPFCNPJconsumidor == null ? "" : n.ecfCPFCNPJconsumidor,
                            n.finalizada,
                            ecfNumero = n.numeroECF == null ? "001" : n.numeroECF,
                            n.contadorRGECF
                        };


            if (dados.Count() == 0)
            {
                MessageBox.Show("Não existe DAV alterados no período selecionado");
                return;
            }
      
                try
                {


                    #region DAV Arquivo
                    StringBuilder conteudo = new StringBuilder();

                   // conteudo = RegistroU1("vendadav");

                    int nRegistrosD3 = 0;
                    foreach (var item in dados)
                    {
                        var itensDAV = from n in Conexao.CriarEntidade().vendadav
                                       where n.documento == item.DAVNumero
                                       && n.codigofilial == GlbVariaveis.glb_filial
                                       && (n.tipoalteracao=="I" || n.tipoalteracao=="A" || n.tipoalteracao=="E") 
                                       select new
                                       {                                           
                                           n.data,
                                           n.nrcontrole,
                                           n.codigo,
                                           n.produto,
                                           n.quantidade,
                                           n.unidade,
                                           n.preco,
                                           descontovalorTotal = (n.descontovalor * n.quantidade),
                                           n.acrescimototalitem,
                                           n.total,
                                           n.tipo,
                                           n.tributacao,
                                           n.icms,
                                           n.cancelado,
                                           n.documento,
                                           n.eaddados,
                                           n.Descontoperc,
                                           n.descontovalor,
                                           n.ccf,
                                           n.coo,
                                           n.ecffabricacao,
                                           n.dataalteracao,
                                           n.horaalteracao,
                                           n.tipoalteracao
                                       };


                        foreach (var itens in itensDAV)
                        {

                            var cripto = Funcoes.CriptografarMD5(itens.documento.ToString().Replace(",", ".") + string.Format("{0:yyyy-MM-dd}", itens.data.Value) + itens.nrcontrole.ToString().Replace(",", ".") + itens.codigo + itens.produto + itens.quantidade.ToString().Replace(",", ".") + itens.unidade + itens.preco.ToString().Replace(",", ".") + itens.descontovalor.ToString().Replace(",", ".") + itens.acrescimototalitem.ToString().Replace(",", ".") + itens.total.ToString().Replace(",", ".") + itens.tributacao.ToString() + itens.Descontoperc.ToString().Replace(",", ".") + itens.cancelado + itens.icms.ToString().Replace(",", ".") + itens.ccf + itens.coo + itens.ecffabricacao);


                            var descricaoPrd = itens.produto;
                            if (cripto != itens.eaddados)
                            {
                                descricaoPrd = itens.produto.Trim().PadRight(100, '?'); // "????????????????????????????????????????????????????????????????????????????????????????????????????";
                            }
                            try
                            {
                                conteudo.AppendLine("D4" +
                                 Funcoes.FormatarZerosEsquerda(item.DAVNumero, 13, false) +
                                 string.Format("{0:yyyyMMdd}", itens.dataalteracao) + //DataAlteracao ??
                                 string.Format("{0:hhmmss}", itens.horaalteracao) + // horaalteracao ??                                 
                                 itens.codigo.PadRight(14, ' ').Substring(0, 14) +
                                 descricaoPrd.PadRight(100, ' ').Substring(0, 100) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.quantidade), 07, true) +
                                 itens.unidade.PadRight(3, ' ').Substring(0, 3) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.preco), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.descontovalorTotal), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.acrescimototalitem), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.total), 14, true) +
                                 SituacaoTributacao(itens.icms, itens.tributacao, itens.tipo) +
                                 Funcoes.FormatarZerosEsquerda(itens.icms, 4, true) +
                                 itens.cancelado +
                                 "3" +
                                 "2" +
                                 itens.tipoalteracao);
                                nRegistrosD3++;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.InnerException.ToString());
                            }
                        }
                    }

                    arquivosPAF.Append(conteudo);
                    return;

                   using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\LogDAVAlterados.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            sw.Write(@conteudo);
                        }
                    };

                    FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\LogDAVAlterados.txt", false);
                    txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\LogDAVAlterados.txt";
                    #endregion
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.InnerException.ToString());
                }            
        }

        private void btnCotepe0908_Click(object sender, EventArgs e)
        {
            AtoCotepe0908();
        }

        private void AtoCotepe0908()
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando EFD");
            msg.Show();
            Application.DoEvents();
            try
            {
                var docExportacao = new[]                 
                    {
                        new ModeloDocFiscal{modeloDocFiscal="02",tipo="Saida"}, //Nota Fiscal de venda ao consumidor, modelo 2
                        new ModeloDocFiscal{modeloDocFiscal="2D",tipo="Saida"},   // Cupom Fiscal emitido por ECF
                        //new ModeloDocFiscal{modeloDocFiscal="Inventario",tipo="Inventario"}
                        //new ModeloDocFiscal{modeloDocFiscal="55",tipo="Entrada"},
                        new ModeloDocFiscal{modeloDocFiscal="01",tipo="Saida"},
                        new ModeloDocFiscal{modeloDocFiscal="55",tipo="Saida"}
                    };

                SPEDFiscal EFD = new SPEDFiscal();
                EFD.dataInicial = dataInicial.Value;
                EFD.dataFinal = dataFinal.Value;
                EFD.modeloDocFiscal = docExportacao.ToList();
                EFD.codigoLayout = "008";
                var conteudo = EFD.GerarSPEDFiscal(true);

                //XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\sHouse.xml");
                //var dados = (from n in doc.Descendants("sHouse").Elements("Dados")
                //             select n).First();
                string laudo = GlbVariaveis.laudoPAF;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("laudo").First().Value), GlbVariaveis.glbSenhaIQ);
                string arquivo = laudo.Trim() + String.Format("{0:ddMMyyyy}", DateTime.Now.Date) + String.Format("{0:HHmmss}", DateTime.Now) + ".txt";
                txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\" + arquivo;

                using (FileStream fs = File.Create(@txtDiretorio.Text))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        sw.Write(@conteudo);
                    }
                };

                FuncoesECF.AssinarArquivo(@txtDiretorio.Text, false);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível gerar o arquivo EFD agora: " + erro.Message, "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                msg.Dispose();
            }
        }

        private void btnTabPrdAtacado_Click(object sender, EventArgs e)
        {
            GerarTabelaProdutos("atacado");
        }

        private void varejoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GerarTabelaProdutos("varejo");
        }

        private void atacadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GerarTabelaProdutos("atacado");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            menuTabPrd.Show(btnTabPrd, new Point(btnTabPrd.Width, 0));                         
        }

        private void btnVenda_Click(object sender, EventArgs e)
        {
            menuVendas.Show(btnVenda, new Point(btnVenda.Width, 0));    
        }

        private void vendaPeríodoICMS5795ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VendasPorPeriodo();
        }

        private void vendaPeríodoCOTEPE0908ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rdbArquivo.Checked = true;
            if (rdbImpressora.Checked)
            {
                System.Diagnostics.Process.Start("registra.bat", "");
                System.Threading.Thread.Sleep(2000);
                Application.DoEvents();
                GerarEspelhoMFD();
                if( FuncoesECF.GerarSPEDECF("", dataInicial.Value.Date, dataFinal.Value.Date) )
                {
                    MessageBox.Show("SPED gerado em: " + @"c:\iqsistemas\SPED_ECF_" + ConfiguracoesECF.nrFabricacaoECF + ".txt");
                }

                return;
            }

            AtoCotepe0908();
        }

        private void FrmMenuFiscal_Load(object sender, EventArgs e)
        {
            if (Conexao.tipoConexao == 2)
                this.tipoConexao = 2; Conexao.tipoConexao = 1;

            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                btnDavEmitidos.Text = "Conta de Clientes Abertas";

            dataInicial.Text = DateTime.Now.Date.ToShortDateString();
            dataFinal.Text = DateTime.Now.Date.ToShortDateString();
            try
            {                
                FuncoesECF.VerificaImpressoraLigada(true,false);
                FuncoesECF.VerificarStatusPapel(true);
            }
            catch
            {
                ConfiguracoesECF.idECF = 0;
            }


            if (ConfiguracoesECF.idECF == 0 || !FuncoesECF.VerificaFabricacaoGrandeTotal() || FuncoesECF.CupomFiscalAberto() )
            {                                                                
                btnParametros.Enabled = false;                               
                btnIDPAF.Enabled = false;                
            }
        }

        private void btnIndice_Click(object sender, EventArgs e)
        {
            rdbArquivo.Checked = true;
            if (rdbImpressora.Checked)
            {
                var dados = (from n in Conexao.CriarEntidade().produtoscomposicao
                             select new { n.codigo, n.descricao }).Distinct().ToList();
                if (dados.Count > 0)
                {
                    FuncoesECF.RelatorioGerencial("abrir", "",GlbVariaveis.TipoRelatorioGerencial.INDICEPROD.ToString());
                }
                FrmMsgOperador msg = new FrmMsgOperador("", "Imprimindo");
                msg.Show();
                Application.DoEvents();
                try
                {
                    foreach (var item in dados)
                    {
                        FuncoesECF.RelatorioGerencial("Imprimir", item.codigo + " " + item.descricao + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("Imprimir", "** Composição **");
                        var dadosComposicao = from n in Conexao.CriarEntidade().produtoscomposicao
                                              where n.codigo == item.codigo
                                              select n;
                        foreach (var itemComposicao in dadosComposicao)
                        {

                            FuncoesECF.RelatorioGerencial("Imprimir", itemComposicao.codigomateria + " " + itemComposicao.descricaomateria + " " + itemComposicao.unidade + " " + string.Format("{0:N2}", itemComposicao.quantidade) + Environment.NewLine);
                        }
                    }

                    FuncoesECF.RelatorioGerencial("Fechar", "");
                }
                finally
                {
                    msg.Dispose();
                }
            }

            if (rdbArquivo.Checked)
            {
                StringBuilder conteudo = new StringBuilder();

                var dados = (from n in Conexao.CriarEntidade().produtoscomposicao
                             select new { n.codigo, n.descricao }).Distinct().ToList();

                FrmMsgOperador msg = new FrmMsgOperador("", "Gerando arquivo");
                msg.Show();
                Application.DoEvents();
                try
                {
                    foreach (var item in dados)
                    {
                        conteudo.AppendLine("|" + item.codigo + "|" + item.descricao);
                        var dadosComposicao = from n in Conexao.CriarEntidade().produtoscomposicao
                                              where n.codigo == item.codigo
                                              select n;
                        foreach (var itemComposicao in dadosComposicao)
                        {

                            conteudo.AppendLine("|" + itemComposicao.codigomateria + "|" + itemComposicao.descricaomateria + "|" + itemComposicao.unidade + "|" + string.Format("{0:N2}", itemComposicao.quantidade));
                        }
                    }


                    using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\Indice.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            sw.Write(@conteudo);
                        }
                    };

                    FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\Indice.txt", false);
                    txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\Indice.txt";
                }
                finally
                {
                    msg.Dispose();
                }
            }

        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", txtDiretorio.Text);
        }

        private void btnParametros_Click(object sender, EventArgs e)
        {
               
                FuncoesECF.RelatorioGerencial("abrir", "","PARAM.CONF");
                FuncoesECF.RelatorioGerencial("imprimir", "Esse PAF-ECF está configurado para o PERFIL: " + ConfiguracoesECF.perfil);
                FuncoesECF.RelatorioGerencial("fechar", "");

                return;


            //    FuncoesECF.RelatorioGerencial("imprimir", "Versão                         : " + GlbVariaveis.glb_Versao + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "Linguagem programação          :  C# 4.0" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "Sistema operacional            : Windows" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "Tipo de Funcionamento          : Em rede" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "Tipo desenvolvimento           : Comerzializável" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "Integração PAF-ECF             : PAF ECF" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO V Item 1  Pre-Venda  : Sim" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO IV Item 3 DAV Por ECF: Não" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO IV Item 4 DAV        : Sim" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO IV Item 6 DAV-OS     : Não" + Environment.NewLine);

            //    FuncoesECF.RelatorioGerencial("imprimir", "APLICAÇÕES ESPECIAIS");
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XXVII - Item 4       ");
            //    FuncoesECF.RelatorioGerencial("imprimir", "BAIXA DE ESTOQUE ATRAVÉS DE TABELA DE INDÍCE");
            //    FuncoesECF.RelatorioGerencial("imprimir", "TÉCNICO DE PRODUÇÃO            : Sim" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XXII A XXXVI");
            //    FuncoesECF.RelatorioGerencial("imprimir", "POSTO REVENDEDOR DE COMBUSTÍVEL: Não " + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XXXVII A XXXIX ");
            //    FuncoesECF.RelatorioGerencial("imprimir", "BAR, RESTAURANTE E SIMILAR, E CONTROLE DE" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "CONTA DE CLIENTE: Não " + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XL ");
            //    FuncoesECF.RelatorioGerencial("imprimir", "FARMÁCIA DE MANIPULAÇÃO        : Não "+Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XLI ");
            //    FuncoesECF.RelatorioGerencial("imprimir", "DAV-OS (Oficina de Conserto)");
            //    FuncoesECF.RelatorioGerencial("imprimir", "(PARAMETRIZÁVEL)               : Não " + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XLI-A ");
            //    FuncoesECF.RelatorioGerencial("imprimir", "SUBSTITUIÇÃO DO TERMO DAV-OS POR  CONTA" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "DO CLIENTE                     : Não " + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XLII: ");
            //    FuncoesECF.RelatorioGerencial("imprimir", "TRANSPORTE DE PASSAGEIROS      : Não " + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XLIV");
            //    FuncoesECF.RelatorioGerencial("imprimir", "POSTO DE PEDÁGIO               : Não" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "A CRITÉRIO DA UNIADE FEDERADA" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "TODAS AS PARAMETRIZAÇÕES RELACIONADAS NESTE" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "RELATÓRIO SÃO DE CONFIGURAÇÃO INACESSÍVEL AO" + Environment.NewLine);
            //    FuncoesECF.RelatorioGerencial("imprimir", "USUÁRIO PAF-ECF" + Environment.NewLine);

            //    FuncoesECF.RelatorioGerencial("imprimir", "A ATIVAÇÃO OU NÃO DESTES PARAMETROS E DETERMI-" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "NADA PELA UNIDADE FEDERADA E SOMENTE PODE SER" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "FEITA INTERVENÇÃO DA EMPRESA DESENVOLVE-" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "DORA DO PAF-ECF." + Environment.NewLine);

            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO IV - ITEM 2");
            //FuncoesECF.RelatorioGerencial("imprimir", "REALIZAR REGISTRO DE PRE-VEDA      : Sim" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO IV - ITEM 3");
            //FuncoesECF.RelatorioGerencial("imprimir", "EMITIR DAV IMPRESSO EQUIPAMENTO");
            //FuncoesECF.RelatorioGerencial("imprimir", "NAO FISCAL                         : Sim" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO IV - ITEM 4");
            //FuncoesECF.RelatorioGerencial("imprimir", "EMITIR DAV IMPRESSO NO ECF         ");
            //FuncoesECF.RelatorioGerencial("imprimir", "COMO RELATÓRIO GERENCIAL           : Nao "+Environment.NewLine);
            
            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO VIII - A ITEM 2");
            //if (dadosEmpresa.estado == "MG")
            //{
            //    FuncoesECF.RelatorioGerencial("imprimir", "MINAS LEGAL                        : Sim" + Environment.NewLine);
            //}
            //else
            //{
            //    FuncoesECF.RelatorioGerencial("imprimir", "MINAS LEGAL                        : Não" + Environment.NewLine);
            //}

            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO VIII - A ITEM 3");

            //if (dadosEmpresa.estado == "RJ")
            //{
            //    FuncoesECF.RelatorioGerencial("imprimir", "CUPOM MANIA                        : Sim" + Environment.NewLine);
            //}
            //else
            //{
            //    FuncoesECF.RelatorioGerencial("imprimir", "CUPOM MANIA                        : Não" + Environment.NewLine);
            //}

            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO VIII - ITEM 1 ALÍNEA a ");
            //FuncoesECF.RelatorioGerencial("imprimir", "CONSULTA DE PREÇOS - TOTALIZAÇÃO DOS");
            //FuncoesECF.RelatorioGerencial("imprimir", "VALORES DA LISTA DE ITENS           : Não "+ Environment.NewLine);

            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO VIII - ITEM 1 ALÍNEA b " + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "CONSULTA DE PREÇOS - TRANSFORMAÇÕES DAS INFORMA-" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "COES DIGITADAS EM REGISTRO DE PRE-VENDA:Nao" + Environment.NewLine);

            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO VIII - ITEM 1 ALÍNEA c " + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "CONSULTA DE PREÇOS - TRANSFORMAÇÕES DAS INFORMA-" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "COES DIGITADAS EM REGISTRO DE DAV   :Nao" + Environment.NewLine);

            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XXII - ITEM 7 ALÍNEA b " + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "NÃO COINCIDÊNCIA DO GT-RECOMPOSIÇÃO" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "DO GT NO ARQUIVO AUXILIAR CRIPTOGRAFADO" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "NO CASO DE INCREMENTO DE CRO:       : Sim" + Environment.NewLine);

            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XXVI - A item 1 ");
            //FuncoesECF.RelatorioGerencial("imprimir", "POSTO DE COMBUSTÍVEL - IMPEDE REGISTRO DE VEN-" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "DA OU EMISSAO DE CUPOM FISCAL QUANDO DETECTAR" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "ESTOQUE ZERO OU NEGATIVO            : Nao" + Environment.NewLine);

            //FuncoesECF.RelatorioGerencial("imprimir", "REQUISITO XXXIX - ITEM 1 " + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "BAR RESTAURANTE E SIMILAR - IMPRESSORA NAO" + Environment.NewLine);
            //FuncoesECF.RelatorioGerencial("imprimir", "FISCAL INSTALATA NOS AMBIENTES DE PRODUCAO :Nao" + Environment.NewLine);            
            //FuncoesECF.RelatorioGerencial("fechar", "");
        }

        private void estoqueTotalToolStripMenuItem_Click(object sender, EventArgs e)
        {
                        

        }

        private void btnGerarEstoqueParc_Click(object sender, EventArgs e)
        {
            estoqueParcial = true;
            estoqueTotal = false;
            //GerarEstoque("parcial");
            pnlEstoqueParcial.Visible = false;
            btnArqPAF_Click(sender, e);
        }

        private void estoqueParcialToolStripMenuItem_Click(object sender, EventArgs e)
        {
                string sql = "UPDATE produtos set marcado=' '";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                sql = "UPDATE produtosfilial set marcado=' '";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
            
            pnlEstoqueParcial.Visible = true;
            pnlEstoqueParcial.Location = new Point(165, 238);
            txtCodPrd.Focus();
        }

        private string SituacaoTributacao(int icms,string tributacao,string tipo)
        {
            //            7.2.1.5 - Campo 08: Tabela de Situações Tributárias: http://www.fazenda.gov.br/confaz/confaz/atos/atos_cotepe/2008/ac006_08.htm

           //CódigoSituação Tributária
            //I
            //Isento
            //N
            //Não Tributado
            //F
            //Substituição Tributária
            //T
            //Tributado pelo ICMS
            //S
            //Tributado pelo ISSQN

            if (icms == 0 || tributacao == "41")
                return "I"; // Isenção

            if (icms == 0 || tributacao == "40")
                return "I"; // Isenção
            if (tributacao == "10" || tributacao == "30" || tributacao == "60")
                tributacao = "F"; // Substituição Tributária
            if (tributacao == "80")
                tributacao = "N"; // Não Incidência
            if (tipo == "1")
                tributacao = "S";
           
            return "T";

        }

        

        private void preçoVarejoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rdCAT52_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Para gerar o arquivo clique no botão LMFC");
        }

        private void rdAtoCotepe_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Para gerar o arquivo clique no botão LMFC");
        }

        //public StringBuilder RegistroU1(string arquivoRegistro)
        //{
        //    siceEntities entidade = Conexao.CriarEntidade();

        //    var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);

        //    if (arquivoRegistro!="" && !FuncoesPAFECF.VerificarQtdRegistro(arquivoRegistro))
        //    {
        //        razao = razao.Trim().PadRight(50, '?');
        //    }

        //    StringBuilder conteudo = new StringBuilder();
        //    conteudo.AppendLine("U1" +
        //        Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
        //        Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
        //        Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
        //        razao);
        //    return conteudo;
        //}


        private void EstoqueECF_Click(object sender, EventArgs e)
        {
            StringBuilder conteudo = new StringBuilder();
           // conteudo = RegistroU1("produtos");

            siceEntities entidade = Conexao.CriarEntidade();

            string dataEstoque = string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
            string horaEstoque = string.Format("{0:hhmmss}", DateTime.Now.TimeOfDay);

            var estoque = (from n in entidade.produtos
                           where n.CodigoFilial == GlbVariaveis.glb_filial
                           && n.marcado == "X"
                           where n.EADE2mercadoriaEstoque != null
                           select new
                           {
                               n.codigo,
                               n.descricao,
                               n.unidade,
                               n.saldofinalestoque,
                               n.data,
                               n.datafinalestoque,
                               n.horafinalestoque,
                               n.EADE2mercadoriaEstoque,
                               n.EADE1,
                               n.dataultvenda,
                               n.marcado,
                               n.ecffabricacao,
                           });

            var itensEstoque = estoque.ToList();

            var dadosECF = (from n in Conexao.CriarEntidade().r01
                            select new { n.fabricacaoECF, n.modeloECF, n.tipoECF, n.marcaECF }).FirstOrDefault();


            string nrFabricacao = itensEstoque.First().ecffabricacao;// ConfiguracoesECF.nrFabricacaoECF;
            string modeloECF = dadosECF.modeloECF ?? ConfiguracoesECF.modeloECF;
            string marcaECF = dadosECF.marcaECF ?? ConfiguracoesECF.marcaECF;
            string tipoECF = dadosECF.tipoECF ?? ConfiguracoesECF.tipoECF;

            nrFabricacao = estoque.First().ecffabricacao;
            horaEstoque = string.Format("{0:hhmmss}", itensEstoque.First().horafinalestoque.Value);
            dataEstoque = string.Format("{0:yyyyMMdd}", itensEstoque.First().datafinalestoque.Value);


            foreach (var item in itensEstoque)
            {
                var unidade = item.unidade.Trim().PadRight(6, ' ').Substring(0, 6);
                var cripto = Funcoes.CriptografarMD5(string.Format("{0:yyyy-MM-dd}", item.datafinalestoque) + item.horafinalestoque.ToString() + item.ecffabricacao);
                if (cripto != item.EADE1)
                {
                    unidade = item.unidade.Trim().PadRight(6, '?');

                }
                conteudo.AppendLine("E3" +
                      nrFabricacao.PadRight(20, ' ').Substring(0, 20) +
                       "1" + //mfadicional
                        tipoECF.PadRight(7, ' ').Substring(0, 7) +
                        marcaECF.PadRight(20, ' ').Substring(0, 20) +
                        modeloECF.PadRight(20, ' ').Substring(0, 20) +
                        dataEstoque +
                        horaEstoque);
            }

            arquivosPAF.Append(conteudo);
            return;

            using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\EstoquePorECF.txt"))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                {
                    sw.Write(@conteudo);
                }
            };
            FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\EstoquePorECF.txt", false);
            txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\EstoquePorECF.txt";
        }

        private void dAVEmitidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Geral.ToString())
                DAVEmitidos();
            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                DAVOSEmitidos();
        }

        private void logDAVAlteradosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DAVLogAlteracao();
        }

        private void btnDavEmitidos_Click(object sender, EventArgs e)
        {
            menuDAV.Show(btnDavEmitidos, new Point(btnDavEmitidos.Width, 0));            
        }

        private void btnArqMF_Click(object sender, EventArgs e)
        {
            GerarArqMF();
        }

        private void menuEstoque_Opening(object sender, CancelEventArgs e)
        {

        }

        private void menuSplitRestroPAF(object sender, EventArgs e)
        {
            menuRegistroPAFECF.Show(btnArqPAF, new Point(btnArqPAF.Width, 0));
        }
        private void btnArqPAF_Click(object sender, EventArgs e)
        {
            //ConfiguracoesECF.nrFabricacaoECF = "DR0814BR000000424896";

            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando Arquivo");
            msg.Show();
            Application.DoEvents();

            StringBuilder conteudo = new StringBuilder();
            try
            {                
                PAFArquivos registrosPAF = new PAFArquivos();
                Paf rs = new Paf();
                //conteudo = registrosPAF.RegistroN1("");

                conteudo = registrosPAF.RegistroU1("");
             //  MessageBox.Show("vai gerar os A2");
                  conteudo.Append(registrosPAF.A2(dataInicial.Value.Date, dataFinal.Value.Date));
             //  MessageBox.Show("vai gerar os P2");


                  try
                  {
                      var R02 = (from n in Conexao.CriarEntidade().r02
                                 where n.dataemissaoreducaoz == GlbVariaveis.Sys_Data
                                 && n.codigofilial == GlbVariaveis.glb_filial && n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                 select n.id).ToList();

                      if (R02.Count() <= 1)
                      {
                          conteudo.Append(registrosPAF.P2("varejo"));
                          //   MessageBox.Show("vai gerar os P2 atacado");
                          conteudo.Append(registrosPAF.P2("atacado"));

                          //   MessageBox.Show("vai gerar os E2");
                          conteudo.Append(registrosPAF.E2(estoqueTotal == true ? "total" : "parcial"));
                      }
                  }
                  catch (Exception erro)
                  {
                      conteudo.Append(registrosPAF.P2("varejo"));
                      //   MessageBox.Show("vai gerar os P2 atacado");
                      conteudo.Append(registrosPAF.P2("atacado"));

                      //   MessageBox.Show("vai gerar os E2");
                      conteudo.Append(registrosPAF.E2(estoqueTotal == true ? "total" : "parcial"));
                  }


             //   MessageBox.Show("vai gerar os E3");
                    conteudo.Append(registrosPAF.E3());
             //   MessageBox.Show("vai gerar os D2");
                      conteudo.Append(registrosPAF.D2(dataInicial.Value.Date, dataFinal.Value.Date));
             //   MessageBox.Show("vai gerar os D4");
                      conteudo.Append(registrosPAF.D4(dataInicial.Value.Date, dataFinal.Value.Date));
             //   MessageBox.Show("vai gerar os Rs");

                    conteudo.Append(registrosPAF.GerarR(true, dataInicial.Value.Date, dataFinal.Value.Date,"001", "",false));

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                msg.Dispose();
            }

            try
            {
                using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\ArquivosPAF.txt"))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        sw.Write(@conteudo);
                    }
                };

                FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\ArquivosPAF.txt", false);
                txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\ArquivosPAF.txt";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro gerando arquivo: " + ex.Message);
            }

        }

        private void chkEstoqueParcial_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEstoqueParcial.Checked == true)
            {
                pnlEstoqueParcial.Visible = true;
                estoqueParcial = true;
            }
            else
            {
                pnlEstoqueParcial.Visible = false;
                estoqueParcial = false;
            }
        }

        private void simplificadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeituraMemoriaFiscal("S");
        }

        private void completaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnLMFC_Click(sender,e);
        }

        private void btnLMF_Click(object sender, EventArgs e)
        {
            menuLMF.Show(btnLMF, new Point(btnLMF.Width, 0));                         
        }

        private void btnFiscoRZ_Click(object sender, EventArgs e)
        {
            try
            {
                PAFArquivos paf = new PAFArquivos();
                string arquivo = "ReducaoZ_" + ConfiguracoesECF.nrFabricacaoECF.PadLeft(20, '0').Substring(6, 14) + String.Format("{0:ddMMyyyy}", DateTime.Now.Date) + ".xml";
                paf.ReducaoZXML(true, dataInicial.Value.Date, dataFinal.Value.Date, "001", @Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirReducaoZEnvio"] + @"\" + arquivo, false);
                txtDiretorio.Text = @Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirReducaoZEnvio"] + @"\" + arquivo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEnviarEstoque_Click(object sender, EventArgs e)
        {
            try
            {
                PAFArquivos paf = new PAFArquivos();
                string arquivo = "Estoque_" + ConfiguracoesECF.nrFabricacaoECF.PadLeft(20, '0').Substring(6, 14) + String.Format("{0:ddMMyyyy}", DateTime.Now.Date) + ".xml";
                paf.EnvioEstoqueXML(dataInicial.Value.Date, dataFinal.Value.Date, @Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEstoqueEnvio"] + @"\" + arquivo, "varejo", estoqueTotal == true ? "total" : "parcial");
                txtDiretorio.Text = @Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEstoqueEnvio"] + @"\" + arquivo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void FrmMenuFiscal_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.tipoConexao == 2)
                Conexao.tipoConexao = 2;
        }

        private void btnVendasIdentificadas_Click(object sender, EventArgs e)
        {
           // menuLMF.Show(btnLMF, new Point(btnLMF.Width, 0));
            VendaIdentificadaMenuStrip.Show(btnVendasIdentificadas,new Point(btnVendasIdentificadas.Width,0));
            
        }

        private void btnEspelho_Click(object sender, EventArgs e)
        {
            GerarEspelhoMFD();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            gerarVendaIdentificada(2);//mes/Ano

            pnVendaIdentificada.Visible = false;
            pnCPFCNPJ.Visible = false;
        }

        private void periodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnVendaIdentificada.Visible = true;
            pnCPFCNPJ.Visible = false;
            txtMes.Text = DateTime.Now.Month.ToString();
            txtAno.Text = DateTime.Now.Year.ToString();
        }

        private void cPFCNPJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnCPFCNPJ.Visible = true;
            pnVendaIdentificada.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gerarVendaIdentificada(1);//CPFCNPJ
            pnVendaIdentificada.Visible = false;
            pnCPFCNPJ.Visible = false;
        }

        private void gerarVendaIdentificada(int tipo)
        {
            PAFArquivos PAF = new PAFArquivos();
            StringBuilder Arquivo = PAF.Z1(tipo,txtCPFCNPJ.Text,txtAno.Text,txtMes.Text);

            try
            {
                string Caminho = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\VendasIdentificadasCPFCNPJ.txt";

                using (FileStream fs = File.Create(Caminho))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        sw.Write(@Arquivo);
                    }
                };

                FuncoesECF.AssinarArquivo(Caminho, false);
                txtDiretorio.Text = Caminho;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro gerando arquivo: " + ex.Message);
            }
        }

        private void resgistroDoPAFECFModeloDIEFMAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ConfiguracoesECF.nrFabricacaoECF = "DR0814BR000000424896";
            txtFabricacao.Text = ConfiguracoesECF.nrFabricacaoECF;
            pnDIEF.Visible = true;
            
        }

        private void menuSplitRestroPAF()
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            pnDIEF.Visible = false;

            string fabricacaoTerminal = ConfiguracoesECF.nrFabricacaoECF;
            ConfiguracoesECF.nrFabricacaoECF = txtFabricacao.Text;

            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando Arquivo DIEF-MA");
            msg.Show();
            Application.DoEvents();

            StringBuilder conteudo = new StringBuilder();
            try
            {
                PAFArquivos registrosPAF = new PAFArquivos();
                Paf rs = new Paf();
                //conteudo = registrosPAF.RegistroN1("");

                conteudo = registrosPAF.RegistroU1("");
                //  MessageBox.Show("vai gerar os A2");
                //  conteudo.Append(registrosPAF.A2(dataInicial.Value.Date, dataFinal.Value.Date));
                //  MessageBox.Show("vai gerar os P2");
                conteudo.Append(registrosPAF.E3());

                var ecf = (from n in Conexao.CriarEntidade().r01
                           where n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                           select n.numeroECF).FirstOrDefault();

                conteudo.Append(registrosPAF.GerarR(true, dataInicial.Value.Date, dataFinal.Value.Date, ecf.ToString(), "", false, true));


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ConfiguracoesECF.nrFabricacaoECF = fabricacaoTerminal;
            }
            finally
            {
                msg.Dispose();
                ConfiguracoesECF.nrFabricacaoECF = fabricacaoTerminal;
            }

            try
            {
                using (FileStream fs = File.Create(@ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\ArquivosPAF.txt"))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        sw.Write(@conteudo);
                    }
                };

                FuncoesECF.AssinarArquivo(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\ArquivosPAF.txt", false);
                txtDiretorio.Text = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\ArquivosPAF.txt";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro gerando arquivo: " + ex.Message);
                ConfiguracoesECF.nrFabricacaoECF = fabricacaoTerminal;
            }

                ConfiguracoesECF.nrFabricacaoECF = fabricacaoTerminal;

        }

    }
}

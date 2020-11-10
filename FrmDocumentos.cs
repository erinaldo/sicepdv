using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;



namespace SICEpdv
{
    public partial class FrmDocumentos : Form
    {
       
        static public string NumeroDocumento = "";
        int documento = 0;
        int qtdF4 = 0;
        int qtdF12 = 0;
        siceEntities Entidades = Conexao.CriarEntidade();


        public FrmDocumentos(string tipo = "Nota")
        {
            siceEntities Entidades = Conexao.CriarEntidade();

            InitializeComponent();
            /*bntNFCe.Enabled = false;
            bntCancelar.Enabled = false;

            if (tipo == "Nota")
                bntNFCe.Enabled = true;
            else
                bntCancelar.Enabled = true;
            */


            pnlIdentificacaoCliente.Visible = false;
            pnlInutilizarNfce.Visible = false;

            var operadores = (from op in Entidades.senhas
                              where op.CodigoFilial == GlbVariaveis.glb_filial
                              select op.operador).ToList();
                             

            operadores.Insert(0,"Todos");
            cbOperadores.DataSource = operadores;
            cbOperadores.DisplayMember = "operador";

            var filiais = (from f in Entidades.filiais
                          where f.ativa == "S"
                          select new
                          {
                              filial = f.CodigoFilial
                          }).ToList();

            cbFiliais.DataSource = filiais;
            cbFiliais.DisplayMember = "filial";
            cbFiliais.ValueMember = "filial";

            cbFiliais.Text = GlbVariaveis.glb_filial;

            vericaNFCePendentes();

            

        }

        public void vericaNFCePendentes()
        {
            DateTime DataAtual = DateTime.Now.Date;

            string serie = int.Parse(ConfiguracoesECF.NFCserie).ToString();

            int NFCPendente = (from n in Conexao.CriarEntidade().contdocs
                               where n.protocolo == "000000000000000"
                               //&& n.modeloDOCFiscal == "65"
                               && n.dpfinanceiro == "Venda"
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               && n.ecfcontadorcupomfiscal.Contains(serie)
                               && n.estornado != "S"
                               select n.documento).Count();


            if (NFCPendente > 0)
            {
                lblNFCPendente.Text = "Existem " + NFCPendente + " NFCe Pendente de Autorização!";
                lblNFCPendente.BackColor = Color.Red;

                DgDocumentos.DataSource = (from doc in Conexao.CriarEntidade().contdocs
                                           where doc.modeloDOCFiscal == "65"
                                           && doc.dpfinanceiro == "Venda"
                                           && doc.CodigoFilial == GlbVariaveis.glb_filial
                                           && doc.protocolo == "000000000000000"
                                           && doc.ecfcontadorcupomfiscal.Contains(serie)
                                           && doc.estornado != "S"
                                           orderby doc.documento, doc.ncupomfiscal
                                           select new
                                           {
                                               Documento = doc.documento,
                                               Estornado = doc.estornado,
                                               VlrTotal = doc.Totalbruto,
                                               Desconto = doc.desconto,
                                               Total = doc.total,
                                               Data = doc.data,
                                               Numero = doc.ncupomfiscal,
                                               Serie = doc.ecfcontadorcupomfiscal,
                                               Chave = doc.chaveNFC,
                                               Protocolo = doc.protocolo,
                                               Modelo = doc.modeloDOCFiscal,
                                               Vendedor = doc.vendedor,
                                               NFe = doc.nrnotafiscal,
                                               Operador = doc.operador,
                                               Cliente = doc.nome
                                           }).ToList();
            }


        }

        private void Filtro(object sender, EventArgs e)
        {
            
            DateTime DataIncio = Convert.ToDateTime(dtDataInicio.Text);
            DateTime DataFinal = Convert.ToDateTime(dtDataFinal.Text);
            string Operadores = cbOperadores.Text;
            string modelo = cbModeloCupom.Text.Replace("(","").Replace(")","").Substring(0,2);

            if (Operadores == "Todos")
                Operadores = "";

            string serie = int.Parse(ConfiguracoesECF.NFCserie).ToString();

            if (chkPendentes.Checked == false)
            {
                string SQL = "update contdocs set ecfcontadorcupomfiscal = '" + int.Parse(ConfiguracoesECF.NFCserie) + "' where abs(ecfcontadorcupomfiscal) = 0 and ip = '" + GlbVariaveis.glb_IP + "'" +
                       " and dpfinanceiro ='Venda' and codigoFilial ='" + cbFiliais.Text + "' and modeloDOCFiscal = '65' and estornado ='N' and data between '" + dtDataInicio.Value.ToString("yyyy-MM-dd") + "' and '" + dtDataFinal.Value.ToString("yyyy-MM-dd") + "'";


                Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

                if (txtCupom.Text.Length > 0)
                {
                    #region
                    string NumeroDoc = txtCupom.Text.ToString().PadLeft(9,'0');

                    DgDocumentos.DataSource = (from doc in Conexao.CriarEntidade().contdocs
                                               where doc.ncupomfiscal == NumeroDoc && doc.modeloDOCFiscal == modelo
                                               && doc.dpfinanceiro == "Venda"
                                               && doc.CodigoFilial == cbFiliais.Text
                                               && doc.ecfcontadorcupomfiscal.EndsWith(serie)
                                               orderby doc.documento descending, doc.ncupomfiscal descending
                                               select new
                                               {
                                                   Documento = doc.documento,
                                                   Estornado = doc.estornado,
                                                   VlrTotal = doc.Totalbruto,
                                                   Desconto = doc.desconto,
                                                   Total = doc.total,
                                                   Data = doc.data,
                                                   Numero = doc.ncupomfiscal,
                                                   Serie = doc.ecfcontadorcupomfiscal,
                                                   Chave = doc.chaveNFC,
                                                   Protocolo = doc.protocolo,
                                                   Modelo = doc.modeloDOCFiscal,
                                                   Vendedor = doc.vendedor,
                                                   NFe = doc.nrnotafiscal,
                                                   Operador = doc.operador,
                                                   Cliente = doc.nome
                                               }).ToList();
                    #endregion
                }
                else if (txtDocumento.Text.Trim().Length > 0)
                {
                    #region
                    int NumeroDoc = int.Parse(txtDocumento.Text.ToString());

                    var documento = (from doc in Conexao.CriarEntidade().contdocs
                                     where doc.documento == NumeroDoc && doc.modeloDOCFiscal == modelo
                                     && doc.dpfinanceiro == "Venda"
                                      && doc.CodigoFilial == cbFiliais.Text
                                      //&& doc.ecfcontadorcupomfiscal == ConfiguracoesECF.NFCserie
                                      && doc.ecfcontadorcupomfiscal.EndsWith(serie)
                                     orderby doc.documento descending, doc.ncupomfiscal descending
                                     select new
                                     {
                                         Documento = doc.documento,
                                         Estornado = doc.estornado,
                                         VlrTotal = doc.Totalbruto,
                                         Desconto = doc.desconto,
                                         Total = doc.total,
                                         Data = doc.data,
                                         Numero = doc.ncupomfiscal,
                                         Serie = doc.ecfcontadorcupomfiscal,
                                         Chave = doc.chaveNFC,
                                         Protocolo = doc.protocolo,
                                         Modelo = doc.modeloDOCFiscal,
                                         Vendedor = doc.vendedor,
                                         NFe = doc.nrnotafiscal,
                                         Operador = doc.operador,
                                         Cliente = doc.nome
                                     }).ToList();



                    DgDocumentos.DataSource = documento;/*(from doc in Conexao.CriarEntidade().contdocs
                                               where doc.documento == NumeroDoc && doc.modeloDOCFiscal == modelo
                                               && doc.dpfinanceiro == "Venda"
                                                && doc.CodigoFilial == cbFiliais.Text
                                                //&& doc.ecfcontadorcupomfiscal == ConfiguracoesECF.NFCserie
                                                && doc.ecfcontadorcupomfiscal.EndsWith(serie)
                                               orderby doc.documento descending, doc.ncupomfiscal descending
                                               select new
                                               {
                                                   Documento = doc.documento,
                                                   Estornado = doc.estornado,
                                                   VlrTotal = doc.Totalbruto,
                                                   Desconto = doc.desconto,
                                                   Total = doc.total,
                                                   Data = doc.data,
                                                   Numero = doc.ncupomfiscal,
                                                   Serie = doc.ecfcontadorcupomfiscal,
                                                   Chave = doc.chaveNFC,
                                                   Protocolo = doc.protocolo,
                                                   Modelo = doc.modeloDOCFiscal,
                                                   Vendedor = doc.vendedor,
                                                   NFe = doc.nrnotafiscal,
                                                   Operador = doc.operador,
                                                   Cliente = doc.nome
                                               }).ToList();*/
                    #endregion
                }
                else
                {

                    try
                    {
                        #region
                       
                        DgDocumentos.DataSource = (from doc in Conexao.CriarEntidade().contdocs
                                                   where doc.data >= DataIncio && doc.data <= DataFinal && doc.operador.Contains(Operadores)
                                                   && doc.dpfinanceiro == "Venda"
                                                   && doc.modeloDOCFiscal == modelo
                                                   && doc.CodigoFilial == cbFiliais.Text
                                                   && doc.ecfcontadorcupomfiscal.EndsWith(serie)
                                                   orderby doc.ncupomfiscal descending, doc.documento descending
                                                   select new
                                                   {
                                                       Documento = doc.documento,
                                                       Estornado = doc.estornado,
                                                       VlrTotal = doc.Totalbruto,
                                                       Desconto = doc.desconto,
                                                       Total = doc.total,
                                                       Data = doc.data,
                                                       Numero = doc.ncupomfiscal,
                                                       Serie = doc.ecfcontadorcupomfiscal,
                                                       Chave = doc.chaveNFC,
                                                       Protocolo = doc.protocolo,
                                                       Modelo = doc.modeloDOCFiscal,
                                                       Vendedor = doc.vendedor,
                                                       NFe = doc.nrnotafiscal,
                                                       Operador = doc.operador,
                                                       Cliente = doc.nome
                                                   }).ToList();
                        #endregion
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }

                }
            }
            else
            {
                #region

                try
                {
                    string SQL = "update contdocs set ecfcontadorcupomfiscal = '" + int.Parse(ConfiguracoesECF.NFCserie) + "' where abs(ecfcontadorcupomfiscal) = 0 and ip = '" + GlbVariaveis.glb_IP + "'" +
                       " and dpfinanceiro ='Venda' and codigoFilial ='" + cbFiliais.Text + "' and modeloDOCFiscal = '65' and estornado ='N' and data between '" + dtDataInicio.Value.ToString("yyyy-MM-dd") + "' and '" + dtDataFinal.Value.ToString("yyyy-MM-dd") + "'";

                    Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

                    string Operador = "";

                    if(cbOperadores.Text != "Todos" && cbOperadores.Text != "todos")
                    {
                        Operador = "AND operador = '" + cbOperadores.Text + "'";
                    }

                    SQL = " SELECT Documento AS 'Documento',Estornado AS 'Estornado',Totalbruto AS 'VlrTotal',Desconto,Total,DATA,ncupomfiscal AS 'Numero', ecfcontadorcupomfiscal AS 'Serie', chaveNFC AS 'Chave', " +
                        " protocolo AS 'Protocolo', modeloDOCFiscal AS 'Modelo', vendedor AS 'Vendedor', nrnotafiscal AS 'NFe', operador AS 'Operador', nome AS 'Cliente' FROM contdocs AS c " +
                        " WHERE (c.protocolo IS NULL OR c.protocolo = 'Erro' /*OR c.protocolo = '000000000000000'*/ OR c.protocolo = '0') AND c.modeloDOCFiscal = '65' AND c.dpfinanceiro = 'Venda' AND c.CodigoFilial = '" + cbFiliais.Text +
                        "' AND c.DATA BETWEEN '" + dtDataInicio.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDataFinal.Value.ToString("yyyy-MM-dd") + "' " + Operador + " AND ABS(ecfcontadorcupomfiscal) = '" + int.Parse(ConfiguracoesECF.NFCserie) +
                        "' AND c.estornado = 'N' and c.modeloDocFiscal = '65'  ORDER BY documento, ncupomfiscal desc";

                    var listaNFce = Conexao.CriarEntidade().ExecuteStoreQuery<NFC>(SQL).ToList();
                    DgDocumentos.DataSource = listaNFce;
                    lblNFCPendente.Text = "Existem ("+listaNFce.Count().ToString()+") NFCe com erros!";
                    lblNFCPendente.BackColor = Color.Yellow;
                    lblNFCPendente.ForeColor = Color.Black;

                }
                catch(Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }
                #endregion
            }

        }

        private void PassaDocumento()
        {
            try
                {
                      if (lblRTotalDoc.Text != "_")
                      {

                          NumeroDocumento = "";
                          documento = int.Parse(DgDocumentos.Rows[DgDocumentos.CurrentRow.Index].Cells["Documento"].Value.ToString());
                          NumeroDocumento = documento.ToString();

                          this.Close();
                      }

                }
                catch (Exception erro)
                {
                    MessageBox.Show("Ligue para o Suporte TECNICO /n"+erro, "Atenção");
                }
            
          }

        private void DgDocumentos_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           int linha = e.RowIndex;
           lblRNomeCliente.Text =  DgDocumentos.Rows[linha].Cells["Cliente"].Value.ToString();
           lblRTotalDoc.Text = DgDocumentos.Rows[linha].Cells["Total"].Value.ToString();
           lblRNrNFe.Text = DgDocumentos.Rows[linha].Cells["Numero"].Value.ToString();

            if ((DgDocumentos.Rows[linha].Cells["Protocolo"].Value == null ? "0" : DgDocumentos.Rows[linha].Cells["Protocolo"].Value).ToString().Length < 15)
            {
                lblStatusNF012.Visible = true;
                lblStatusNF012.Text = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT IFNULL(CONCAT(CbdStsRetCodigo,'-',CbdStsRetNome),'') FROM nfe012 WHERE cbdDocumento = '" + DgDocumentos.Rows[linha].Cells["Documento"].Value.ToString() + "'").FirstOrDefault();
            }

        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            PassaDocumento();
            this.Close();
               
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool mostrarMsg = true;

            if (chktodos.Checked == true)
                mostrarMsg = false;

            try
            {
                if(chktodos.Checked == true)
                {
                    FuncoesNFC.verificarGerenciadorNFCe("Fechar");
                    System.Threading.Thread.Sleep(5000);
                    FuncoesNFC.verificarGerenciadorNFCe("Abrir", chkImpressao.Checked);
                    System.Threading.Thread.Sleep(5000);

                    int atual = 0;
                    int total = DgDocumentos.Rows.Count;

                    foreach (DataGridViewRow item in DgDocumentos.Rows)
                    {
                        try
                        {
                            int DOc = int.Parse(item.Cells["documento"].Value.ToString());
                            if (item.Cells["Estornado"].Value.ToString() == "N")
                            {
                                var documento = (from d in Conexao.CriarEntidade().contdocs
                                                 where d.documento == DOc && d.CodigoFilial == GlbVariaveis.glb_filial
                                                 select d).FirstOrDefault();

                                nfe012E objNfe012 = new nfe012E();
                                bool gerar = objNfe012.gerarXML(objNfe012.notafiscalDocumento(DOc.ToString()));
                                //apagarNotafiscal(int.Parse(documento.ncupomfiscal), int.Parse(documento.ecfcontadorcupomfiscal).ToString(), GlbVariaveis.glb_filial, gerar);
                                reenviarNFCe(DOc,0,false, mostrarMsg, atual.ToString() + " de " + total.ToString(), gerar);
                            }
                        }
                        catch (Exception erro)
                        {
                            MessageBox.Show(erro.ToString());
                        }
                    }

                    FuncoesNFC.verificarGerenciadorNFCe("Fechar");
                    System.Threading.Thread.Sleep(5000);
                }
                else
                {

                    int DOc = int.Parse(DgDocumentos.CurrentRow.Cells[0].Value.ToString());

                    var documento = (from d in Conexao.CriarEntidade().contdocs
                                     where d.documento == DOc && d.CodigoFilial == GlbVariaveis.glb_filial
                                     select d).FirstOrDefault();

                    nfe012E objNfe012 = new nfe012E();
                    bool gerar = objNfe012.gerarXML(objNfe012.notafiscalDocumento(DOc.ToString()));
                    //apagarNotafiscal(int.Parse(documento.ncupomfiscal), int.Parse(documento.ecfcontadorcupomfiscal).ToString(), GlbVariaveis.glb_filial, gerar);
                    reenviarNFCe(DOc, int.Parse(DgDocumentos.CurrentRow.Cells[6].Value.ToString()),true,true,"", gerar);

                    FuncoesNFC objNFC = new FuncoesNFC();
                    objNFC.imprimirCupom(DOc, "Documento", false);
                }
                
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }
        }

        private void reenviarNFCe(int DOc, int numeroNF = 0, bool focarDocumento = true, bool mostrarMsg = true, string msgComplementar = "",bool gerar=true)
        {
            try
            {
                NFe objNFe = new NFe();
                objNFe.reenviar(numeroNF, DOc, mostrarMsg, msgComplementar,false,gerar);
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }



            if (focarDocumento == true)
            {
                DgDocumentos.DataSource = (from doc in Conexao.CriarEntidade().contdocs
                                           where doc.documento == DOc
                                           orderby doc.documento, doc.ncupomfiscal
                                           select new
                                           {
                                               Documento = doc.documento,
                                               Estornado = doc.estornado,
                                               VlrTotal = doc.Totalbruto,
                                               Desconto = doc.desconto,
                                               Total = doc.total,
                                               Data = doc.data,
                                               Numero = doc.ncupomfiscal,
                                               Serie = doc.ecfcontadorcupomfiscal,
                                               Chave = doc.chaveNFC,
                                               Protocolo = doc.protocolo,
                                               Modelo = doc.modeloDOCFiscal,
                                               Vendedor = doc.vendedor,
                                               NFe = doc.nrnotafiscal,
                                               Operador = doc.operador,
                                               Cliente = doc.nome
                                           }).ToList();
            }

        }

        private void apagarNotafiscal(int notafiscal, string serie, string codigoFilial, bool gerar=true)
        {
            try
            {
                if (gerar == false)
                    return;

                NFe NFCe = new NFe();
                NFCe.apagarNotaFiscal(notafiscal, serie, codigoFilial);

            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (chktodos.Checked == true)
                {
                    foreach (DataGridViewRow item in DgDocumentos.Rows)
                    {
                        if (item.Cells["Estornado"].Value.ToString() == "N")
                        {
                            int DOc = int.Parse(item.Cells["documento"].Value.ToString());
                            FuncoesNFC objNFC = new FuncoesNFC();
                            objNFC.imprimirCupom(DOc, "Documento", false, true);
                        }

                    }
                }
                else
                {
                    int documento = int.Parse(DgDocumentos.CurrentRow.Cells[0].Value.ToString());
                    string cliente = DgDocumentos.CurrentRow.Cells["cliente"].Value.ToString();

                    if (ConfiguracoesECF.NFC == true)
                    {
                        bool enviarEmail = false;

                        var chaveAcesso = (from d in Conexao.CriarEntidade().contdocs
                                           where d.documento == documento
                                           select d.chaveNFC).FirstOrDefault();

                        FuncoesNFC objNFC = new FuncoesNFC();
                        objNFC.imprimirCupom(documento, "Documento", enviarEmail,true);
                    }
                    else if (ConfiguracoesECF.NFC == false && ConfiguracoesECF.idECF == 9999)
                    {
                        Process myProcess = System.Diagnostics.Process.Start(@"ImpressorCupom.exe", " " + documento.ToString());
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void DgDocumentos_DoubleClick(object sender, EventArgs e)
        {

            try
            {
                int documento = int.Parse(DgDocumentos.CurrentRow.Cells[0].Value.ToString());
                FrmItensDocumento objDocumentos = new FrmItensDocumento(documento);
                objDocumentos.ShowDialog();
            }
            catch (Exception erro)
            {

            }
        }

        private void FrmDocumentos_Load(object sender, EventArgs e)
        {
            if (ConfiguracoesECF.idECF == 9999 && ConfiguracoesECF.NFC == false)
            {
                cbModeloCupom.Text = "(02) Venda";
                bntNFCe.Enabled = false;
                bntImpressao.Text = "Reimprimir";
            }
            else
            {
                cbModeloCupom.Text = "(65) NFC";
            }
            cbOperadores.Text = GlbVariaveis.glb_Usuario;

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            bool mostrarMsg = true;

            if (chktodos.Checked == true)
                mostrarMsg = false;

            DialogResult resultado  =  MessageBox.Show("Deseja Gerar um novo NFC-e desse documento, isso irá alterar o numero e a data do mesmo?", "Atenção", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            
            if (resultado == DialogResult.OK)
            {
                if (chktodos.Checked == true)
                {

                    FuncoesNFC.verificarGerenciadorNFCe("Fechar");
                    System.Threading.Thread.Sleep(5000);
                    FuncoesNFC.verificarGerenciadorNFCe("Abrir", chkImpressao.Checked);
                    System.Threading.Thread.Sleep(5000);

                    int atual = 0;
                    int total = DgDocumentos.Rows.Count;

                    foreach (DataGridViewRow item in DgDocumentos.Rows)
                    {

                        try
                        {
                            int DOc = int.Parse(item.Cells["documento"].Value.ToString());
                            if (item.Cells["Estornado"].Value.ToString() == "N")
                            {
                                var documento = (from d in Conexao.CriarEntidade().contdocs
                                                 where d.documento == DOc && d.CodigoFilial == GlbVariaveis.glb_filial
                                                 select d).FirstOrDefault();

                                nfe012E objNfe012 = new nfe012E();
                                bool gerar = objNfe012.gerarXML(objNfe012.notafiscalDocumento(DOc.ToString()));
                                apagarNotafiscal(int.Parse(documento.ncupomfiscal), int.Parse(documento.ecfcontadorcupomfiscal).ToString(), GlbVariaveis.glb_filial, gerar);
                                reenviarNFCe(DOc, int.Parse(documento.ncupomfiscal),false,mostrarMsg, int.Parse(documento.ncupomfiscal).ToString()+" - "+atual.ToString()+" de "+total.ToString(), gerar);

                                atual++;
                            }
                        }
                        catch (Exception erro)
                        {
                            MessageBox.Show(erro.ToString());
                        }
                    }

                    FuncoesNFC.verificarGerenciadorNFCe("Fechar");
                    System.Threading.Thread.Sleep(5000);
                }
                else
                {
                    int DOc = int.Parse(DgDocumentos.CurrentRow.Cells[0].Value.ToString());

                    var documento = (from d in Conexao.CriarEntidade().contdocs
                                     where d.documento == DOc && d.CodigoFilial == GlbVariaveis.glb_filial
                                     select d).FirstOrDefault();

                    

                    nfe012E objNfe012 = new nfe012E();
                    bool gerar = objNfe012.gerarXML(objNfe012.notafiscalDocumento(DOc.ToString()));

                    apagarNotafiscal(int.Parse(documento.ncupomfiscal), int.Parse(documento.ecfcontadorcupomfiscal).ToString(), GlbVariaveis.glb_filial, gerar);
                    reenviarNFCe(DOc, int.Parse(documento.ncupomfiscal),true,true,"", gerar);

                    FuncoesNFC objNFC = new FuncoesNFC();
                    objNFC.imprimirCupom(DOc, "Documento", false);
                }
            }
        }

        private void DgDocumentos_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode.ToString() == "F3")
            {
                try
                {
                    int documento = int.Parse(DgDocumentos.CurrentRow.Cells[0].Value.ToString());

                    var venda = (from d in Conexao.CriarEntidade().contdocs
                                 where d.documento == documento
                                 select d).FirstOrDefault();

                    if (venda.modeloDOCFiscal == "65")
                    {
                        if (venda.chaveNFC == "" || venda.chaveNFC == "Erro" || venda.protocolo == "000000000000000")
                        {

                            txtCPF.Text = venda.ecfCPFCNPJconsumidor.ToString();
                            txtNome.Text = venda.ecfConsumidor.ToString();
                            //txtEndereco.Text = venda.ecfEndConsumidor.ToString();

                            pnlIdentificacaoCliente.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Documento ja autorizado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Não é um documento NFC-e", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch(Exception erro)
                {
                    if (erro.Message == "Referência de objeto não definida para uma instância de um objeto.")
                    {
                        MessageBox.Show("Selecione um documento", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(erro.ToString(), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


            if (e.KeyCode.ToString() == "F4")
            {
                string SQL = "";
                string arquivado = "";

                qtdF4++;

                if(qtdF4 == 8)
                {
                    qtdF4 = 0;
                    if (MessageBox.Show("Deseja apagar XML do banco de dados?", "Atenção", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                    {
                        try
                        {
                            int documento = int.Parse(DgDocumentos.CurrentRow.Cells[0].Value.ToString());

                            var venda = (from d in Conexao.CriarEntidade().contdocs
                                         where d.documento == documento
                                         select d).FirstOrDefault();

                            if (venda.modeloDOCFiscal == "65")
                            {
                               
                                if (Configuracoes.cfgarquivardados == "S")
                                {
                                    SQL = "SELECT cbdArquivado FROM cbd001 WHERE CbdDocumento = '" + documento.ToString() + "' AND cbdmod = '65'";
                                    arquivado = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                    if (arquivado == "S")
                                    {

                                        //MessageBox.Show("Não é possivel alterar um NFCe Arquivado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        //return;

                                        Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE contdocs AS c SET "+
                                        "c.dataAutorizacao = (SELECT CbdDtaProcessamento FROM nfe012arquivo WHERE cbdDocumento = '" + documento.ToString() + "' LIMIT 1), " +
                                        "c.protocolo = (SELECT IF((CbdNumProtCanc IS NOT NULL AND CbdNumProtCanc <> '0'), CbdNumProtCanc,CbdNumProtocolo) " +
                                        "FROM nfe012arquivo WHERE cbdDocumento = '" + documento.ToString() + "' LIMIT 1), " +
                                        "c.chaveNFC = (SELECT CbdNFEChaAcesso FROM nfe012arquivo WHERE cbdDocumento = '" + documento.ToString() + "' LIMIT 1), " +
                                        "c.ncupomfiscal = (SELECT cbdntfnumero FROM nfe012arquivo WHERE cbdDocumento = '" + documento.ToString() + "' LIMIT 1), " +
                                        "c.modeloDOCFiscal = '65', " +
                                        "c.ecfcontadorcupomfiscal = (SELECT CbdNtfSerie FROM nfe012arquivo WHERE cbdDocumento = '" + documento.ToString() + "' LIMIT 1), " +
                                        "c.ecffabricacao = 'SICENFCe' " +
                                        "WHERE c.documento = '" + documento.ToString() + "'");

                                        return;

                                    }
                                }

                                    Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE nfe012 as n SET n.CbdNumProtocolo = NULL, n.CbdStsRetCodigo = NULL,  n.CbdNFEChaAcesso = NULL, n.CbdXML = NULL, n.CbdDigVal = NULL where " +
                                                                "n.CbdNtfNumero = '" + int.Parse(venda.ncupomfiscal) + "' AND n.CbdNtfSerie = '" + int.Parse(venda.ecfcontadorcupomfiscal) + "' AND n.cbdcodigofilial = '" + venda.CodigoFilial + "' AND n.CbdMod = '65'");

                                Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE contdocs AS c SET c.chaveNFC = 'Erro', c.protocolo = 'Erro', c.dataAutorizacao = NULL WHERE documento ='"+venda.documento+"'");

                            }
                        }
                        catch(Exception erro)
                        {
                            MessageBox.Show(erro.ToString());
                        }
                    }
                }
            }

            if (e.KeyCode.ToString() == "F12")
            {
                qtdF12++;
                string SQL = "";
                string status = "";
                string protocoloNFe012 = "";
                int qtdNfe012 = 0;
                int cbdNtfNumero = 0;
                int cbdNtfSerie = 0;
                string chaveNFe012 = "";
                string arquivado = "";


                if (qtdF12 == 8)
                {
                    qtdF12 = 0;
                    if (MessageBox.Show("Deseja apagar Numero NFCe?", "Atenção", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                    {
                        try
                        {
                            int documento = int.Parse(DgDocumentos.CurrentRow.Cells[0].Value.ToString());

                            var venda = (from d in Conexao.CriarEntidade().contdocs
                                         where d.documento == documento
                                         select d).FirstOrDefault();

                            if (venda.modeloDOCFiscal == "65")
                            {
                                if (Configuracoes.cfgarquivardados == "S")
                                {
                                    SQL = "SELECT cbdArquivado FROM cbd001 WHERE CbdDocumento = '" + documento.ToString() + "' AND cbdmod = '65'";
                                    arquivado = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                    if (arquivado == "S")
                                    {
                                        MessageBox.Show("Não é possivel alterar um NFCe Arquivado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }

                                SQL = "select IFNULL(count(1),0) as quantidade from nfe012 WHERE cbddocumento = '" + documento.ToString() + "' AND cbdmod = '65'";
                                qtdNfe012 = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                                SQL = "select CbdStsRetCodigo from nfe012 WHERE cbddocumento = '" + documento.ToString() + "' AND cbdmod = '65'";
                                status = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                SQL = "select CbdNumProtocolo from nfe012 WHERE cbddocumento = '" + documento.ToString() + "' AND cbdmod = '65'";
                                protocoloNFe012 = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                SQL = "select cbdntfNumero from nfe012 WHERE cbddocumento = '" + documento.ToString() + "' AND cbdmod = '65'";
                                cbdNtfNumero = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                                SQL = "select cbdNtfSerie from nfe012 WHERE cbddocumento = '" + documento.ToString() + "' AND cbdmod = '65'";
                                cbdNtfSerie = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                                if (qtdNfe012 > 0 && status == "100" && protocoloNFe012 != null && (venda.chaveNFC == "" || venda.chaveNFC == null || venda.chaveNFC == "Erro" || venda.protocolo == "000000000000000"))
                                {
                                    MessageBox.Show("Esse documento já foi emitido um NFCe (" + cbdNtfNumero + ") o sistema irá atualizar os dados!", "Atenção", MessageBoxButtons.OK);

                                    SQL = "select CbdNFEChaAcesso from nfe012 WHERE cbddocumento = '" + documento.ToString() + "' AND cbdmod = '65'";
                                    chaveNFe012 = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                    SQL = "UPDATE contdocs SET chaveNFC = '" + chaveNFe012 + "' , protocolo = '" + protocoloNFe012 + "', ecfcontadorcupomfiscal = '" + ConfiguracoesECF.NFCserie + "', ncupomfiscal = LPAD(ncupomfiscal,9,'0')  WHERE documento ='" + venda.documento + "' AND codigofilial = '" + venda.CodigoFilial + "' AND modelodocfiscal = '" + venda.modeloDOCFiscal + "'";
                                    Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

                                    FuncoesNFC objNFC = new FuncoesNFC();
                                    objNFC.imprimirCupom(documento, "Documento", false, true);
                                }
                                else if (qtdNfe012 > 0 && status == "100" && protocoloNFe012 != null && cbdNtfNumero == int.Parse(venda.ncupomfiscal) && cbdNtfSerie == int.Parse(venda.ecfcontadorcupomfiscal))
                                {
                                    MessageBox.Show("Não é possivel Alterar o numero de um NFCe aparentimente correto!", "Atenção", MessageBoxButtons.OK);
                                    FuncoesNFC objNFC = new FuncoesNFC();
                                    objNFC.imprimirCupom(documento, "Documento", false, true);
                                }
                                else
                                {
                                    if (qtdNfe012 > 0 && status != "100" && protocoloNFe012 == null && (venda.chaveNFC == "" || venda.chaveNFC == null || venda.chaveNFC == "Erro" || venda.protocolo == "000000000000000"))
                                    {
                                        /*SQL = "select cbdntfNumero from nfe012 WHERE cbddocumento = '" + documento.ToString() + "' AND cbdmod = '65'";
                                        cbdNtfNumero = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                                        SQL = "select cbdNtfSerie from nfe012 WHERE cbddocumento = '" + documento.ToString() + "' AND cbdmod = '65'";
                                        cbdNtfSerie = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();*/

                                        SQL = "update contdocs set ncupomfiscal = '" + cbdNtfNumero + "', ecfcontadorcupomfiscal = '" + cbdNtfSerie + "' where documento = '" + documento.ToString() + "'";
                                        Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                                    }
                                    //else
                                    //{
                                        //apagarNotafiscal(int.Parse(venda.ncupomfiscal), int.Parse(venda.ecfcontadorcupomfiscal).ToString(), GlbVariaveis.glb_filial);
                                        //Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE contdocs AS c SET c.chaveNFC = 'Erro', c.protocolo = 'Erro', c.dataAutorizacao = NULL, ncupomfiscal = '000000000' WHERE documento ='" + venda.documento + "'");
                                    //}
                                }

                                DgDocumentos.DataSource = (from doc in Conexao.CriarEntidade().contdocs
                                                           where doc.documento == documento
                                                           orderby doc.documento, doc.ncupomfiscal
                                                           select new
                                                           {
                                                               Documento = doc.documento,
                                                               Estornado = doc.estornado,
                                                               VlrTotal = doc.Totalbruto,
                                                               Desconto = doc.desconto,
                                                               Total = doc.total,
                                                               Data = doc.data,
                                                               Numero = doc.ncupomfiscal,
                                                               Serie = doc.ecfcontadorcupomfiscal,
                                                               Chave = doc.chaveNFC,
                                                               Protocolo = doc.protocolo,
                                                               Modelo = doc.modeloDOCFiscal,
                                                               Vendedor = doc.vendedor,
                                                               NFe = doc.nrnotafiscal,
                                                               Operador = doc.operador,
                                                               Cliente = doc.nome
                                                           }).ToList();
                            }
                        }
                        catch (Exception erro)
                        {
                            MessageBox.Show(erro.ToString());
                        }
                    }
                }
            }

        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            pnlIdentificacaoCliente.Visible = false;
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                int documento = int.Parse(DgDocumentos.CurrentRow.Cells[0].Value.ToString());

                siceEntities conexao = Conexao.CriarEntidade();

                var venda = (from d in conexao.contdocs
                             where d.documento == documento
                             select d).FirstOrDefault();

                venda.ecfCPFCNPJconsumidor = txtCPF.Text.Trim().Replace(".", "").Replace(",", "").Replace("-", "").Replace("_", "");
                venda.ecfConsumidor = txtNome.Text.Trim();

                if (chkFisica.Checked == true)
                {
                    conexao.ExecuteStoreCommand("UPDATE cbd001 SET CbdxNome_dest = '" + venda.ecfConsumidor + "', CbdCPF_dest = '" + venda.ecfCPFCNPJconsumidor + "' WHERE CbdNtfNumero ='" + int.Parse(venda.ncupomfiscal.ToString()).ToString() + "' AND CbdNtfSerie = '" + int.Parse(venda.ecfcontadorcupomfiscal.ToString()).ToString() + "' AND cbdcodigofilial = '" + venda.CodigoFilial.ToString() + "'");
                }
                else
                {
                    conexao.ExecuteStoreCommand("UPDATE cbd001 SET CbdxNome_dest = '" + venda.ecfConsumidor + "', CbdCNPJ_dest = '" + venda.ecfCPFCNPJconsumidor + "' WHERE CbdNtfNumero ='" + int.Parse(venda.ncupomfiscal.ToString()).ToString() + "' AND CbdNtfSerie = '" + int.Parse(venda.ecfcontadorcupomfiscal.ToString()).ToString() + "' AND cbdcodigofilial = '" + venda.CodigoFilial.ToString() + "'");
                }

                //conexao.ExecuteStoreCommand("UPDATE contdocs SET ecfConsumidor = '" + venda.ecfConsumidor + "', ecfCPFCNPJconsumidor = '" + venda.ecfCPFCNPJconsumidor + "' WHERE abs(ncupomfiscal) ='" + int.Parse(venda.ncupomfiscal.ToString()).ToString() + "' AND abs(ecfcontadorcupomfiscal) = '" + int.Parse(venda.ecfcontadorcupomfiscal.ToString()).ToString() + "' AND codigoFilial = '" + venda.CodigoFilial.ToString() + "'");

                conexao.SaveChanges();

                pnlIdentificacaoCliente.Visible = false;
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.ToString(), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chktodos_Click(object sender, EventArgs e)
        {
            if(chktodos.Checked == true)
            {
                chkImpressao.Visible = true;
            }
            else
            {
                chkImpressao.Visible = false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            siceEntities entidade = Conexao.CriarEntidade();
            var dados = (from n in entidade.senhas
                         where n.operador == GlbVariaveis.glb_Usuario
                         select n).First();

            System.Diagnostics.Process.Start(@"iEFD.exe", GlbVariaveis.glb_filial+" S "+ dados.codigo.ToString() + " "+GlbVariaveis.glb_senhaUsuario);

            /*string NotaFiscalErro = "";
            string SerieErro = "";

            try
            {
                FrmMsgOperador msg1 = new FrmMsgOperador("", @"Excluindo arquivos!");
                msg1.Show();
                Application.DoEvents();
                Directory.Delete(@"xmls", true);
                msg1.Dispose();
                          

                FrmMsgOperador msg2 = new FrmMsgOperador("", @"Filtrando XMLs!");
                msg2.Show();
                Application.DoEvents();

                List<string> listXMLs = new List<string>();
                List<dadosNFCe> listNFCe = new List<dadosNFCe>();

                lerXML objlerXML = new lerXML();
                

                DateTime DataIncio = Convert.ToDateTime(dtDataInicio.Text);
                DateTime DataFinal = Convert.ToDateTime(dtDataFinal.Text);

                string SQL = "SELECT documento AS Documento,estornado AS Estornado,Totalbruto AS VlrTotal,desconto AS Desconto,total AS Total,DATA AS DATA,ncupomfiscal AS Numero,ecfcontadorcupomfiscal AS Serie,chaveNFC AS Chave,protocolo AS Protocolo, " +
                           " modeloDOCFiscal AS Modelo,vendedor AS Vendedor,nrnotafiscal AS NFe,operador AS Operador,nome AS Cliente FROM contdocs AS doc " +
                           " WHERE doc.modeloDOCFiscal = '65' " +
                           " && DATE(doc.dataAutorizacao) >= '" + DataIncio.ToString("yyyy-MM-dd") + "'" +
                           " && DATE(doc.dataAutorizacao) <= '" + DataFinal.ToString("yyyy-MM-dd") + "'" +
                           " && doc.dpfinanceiro = 'Venda'" +
                           " && doc.CodigoFilial = '" + cbFiliais.Text.Substring(0, 5) + "'" +
                           " && doc.chaveNFC <> 'Erro'" +
                           " && doc.chaveNFC IS NOT NULL" +
                           " && doc.chaveNFC <> '' " +
                           " && doc.protocolo <> '000000000000000'" +
                           " && doc.protocolo <> 'Erro' " +
                           " && doc.protocolo IS NOT NULL " +
                           " && doc.protocolo <> '' " +
                           " && doc.estornado = 'N' " +
                           " ORDER BY documento, ncupomfiscal";

                var documento = Conexao.CriarEntidade().ExecuteStoreQuery<documentosXML>(SQL).ToList();

                if (!Directory.Exists("xmls"))
                    {
                        Directory.CreateDirectory("xmls");
                    }

                    if (!Directory.Exists(@"xmls"))
                    {
                        Directory.CreateDirectory(@"xmls");
                    }

                msg2.Dispose();

                FrmMsgOperador msg = new FrmMsgOperador("", @"Exportando XMLs para C:\iqsistemas\SICEpdv\xmls\");
                msg.Show();
                Application.DoEvents();

                foreach (var item in documento)
                {

                    var xml = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT cbdxml FROM nfe012 WHERE cbdntfnumero = '" + int.Parse(item.Numero).ToString() + "' AND cbdntfserie = '" + int.Parse(item.Serie).ToString() + "' AND cbdmod = '65' AND cbdcodigofilial = '" + GlbVariaveis.glb_filial + "'").FirstOrDefault();
                    var cancelado = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT ifnull(CbdNumProtCanc,0) FROM nfe012 WHERE cbdntfnumero = '" + int.Parse(item.Numero).ToString() + "' AND cbdntfserie = '" + int.Parse(item.Serie).ToString() + "' AND cbdmod = '65' AND cbdcodigofilial = '" + GlbVariaveis.glb_filial + "'").FirstOrDefault();

                    NotaFiscalErro = item.Numero.ToString();
                    SerieErro = item.Serie.ToString();

                    if (cancelado == "0")
                    {
                        if (xml != null)
                        {
                            XmlDocument xmlDocument = new XmlDocument();
                            xml = xml.ToString().Replace("\r\n-", "").Replace(@"\\", "").Replace("\r\n", "").Replace(@"[^\w\.@-]", "").Replace("  <?", "<?").Replace("This XML file does not appear to have any style information associated with it. The document tree is shown below.", "").Replace("&", "");
                            xmlDocument.LoadXml(xml.Replace(@"\r\n\r\n-", ""));

                            var n = objlerXML.lerdadosXML(xml, item.Data);
                            n.totaldocumento = item.Total;
                            n.cancelado = cancelado == "0" ? false : true;
                            n.grupo = n.dataAutorizacao.ToString();

                            if (int.Parse(n.dataAutorizacao.Month.ToString()) == int.Parse(DataIncio.Date.Month.ToString()))
                            {
                                listNFCe.Add(n);
                                xmlDocument.Save(@"xmls\" + item.Chave + ".xml");
                                listXMLs.Add(@"xmls\" + item.Chave + ".xml");
                            }
                        }
                        else
                        {
                            MessageBox.Show("NFCe.: " + item.Numero + " Serie.: " + item.Serie + " Nº documento.:" + item.Documento.ToString() + " sem XML!");
                            msg.Dispose();
                            listXMLs.Clear();
                            return;
                        }
                    }
               }

                msg.Dispose();


                if (chkZip.Checked == true)
                {
                    FrmMsgOperador msg5 = new FrmMsgOperador("", @" Compactando arquivos!");
                    msg5.Show();
                    Application.DoEvents();


                    ZipFile zip = new ZipFile();
                    foreach (var xml in listXMLs)
                    {
                        int q = (from x in zip where x.FileName == xml select x).Count();
                        if (q > 0)
                        {
                            MessageBox.Show("chave já lancada" + xml);
                        }

                        zip.AddFile(xml, "");
                    }

                    zip.Save(@"xmls\" + DataIncio.ToString("dd-MM-yyyy") + "_" + DataFinal.ToString("dd-MM-yyyy") + "_" + cbFiliais.Text.Substring(0, 5) + ".zip");
                    msg5.Dispose();

                    FrmMsgOperador msg6 = new FrmMsgOperador("", @" Esvaziando memoria!");
                    msg6.Show();
                    Application.DoEvents();

                    foreach (var xml in listXMLs)
                    {
                        File.Delete(xml.ToString());
                    }
                    msg6.Dispose();
                }

                FrmMsgOperador msg3 = new FrmMsgOperador("", @"Gerando Relatorio em PDF em C:\iqsistemas\SICEpdv\xmls\");
                msg3.Show();
                Application.DoEvents();

                frmRelatorioNFCe objFrm = new frmRelatorioNFCe();
                objFrm.listDadosNFCe = listNFCe.ToList();
                objFrm.dataIncio = dtDataInicio.Value;
                objFrm.dataFinal = dtDataFinal.Value;
                objFrm.ShowDialog();

                msg3.Dispose();

               
                MessageBox.Show("Arquivo Gerado com sucesso!","Atenção",MessageBoxButtons.OK);
                Process.Start(@"C:\iqsistemas\sicepdv\xmls\");

            }
            catch(Exception erro)
            {
                MessageBox.Show("Notafiscal.:"+ NotaFiscalErro + " Serie.:"+SerieErro+" -> "+erro.ToString());
            }
            */
           

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            FrmMsgOperador msg1 = new FrmMsgOperador("", @"Ajustando XMLs!");
            msg1.Show();
            Application.DoEvents();
            try
            {
                var listSerie = Conexao.CriarEntidade().ExecuteStoreQuery<int>("SELECT ABS(ecfcontadorcupomfiscal) FROM contdocs WHERE DATA BETWEEN '" + dtDataInicio.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDataFinal.Value.ToString("yyyy-MM-dd") + "' AND codigoFilial = '" + GlbVariaveis.glb_filial + "' AND modeloDOCFiscal = '65' AND ABS(ecfcontadorcupomfiscal) IS NOT NULL group by ABS(ecfcontadorcupomfiscal) ").ToList();

                foreach (var item in listSerie)
                {
                    FrmMsgOperador msg2 = new FrmMsgOperador("", "Ajustando Serie(" + item + ") Modelo(65) !");
                    msg2.Show();
                    Application.DoEvents();
                    Conexao.CriarEntidade().ExecuteStoreCommand("CALL FechamentoNFCe('" + dtDataInicio.Value.ToString("yyyy-MM-dd") + "','" + dtDataFinal.Value.ToString("yyyy-MM-dd") + "','" + item + "','" + GlbVariaveis.glb_filial + "','65')");
                    msg2.Dispose();
                }

                MessageBox.Show("XMLs Ajustados Com sucesso!", "atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }
            msg1.Dispose();
        }

        private void btnInutilizarNfce_Click(object sender, EventArgs e)
        {
            if (this.VerificarNumero() == false)
                return;


            FrmMsgOperador msg1 = new FrmMsgOperador("", @"Inutilizando Numeração!");
            msg1.Show();
            Application.DoEvents();

            String filial = cbFiliais.Text.Substring(0, 5);
            String NumeroInicialNFCe = txtNumeroInicialNFCe.Text;
            String NumeroFinalNFCe = txtNumeroFinalNFCe.Text;
            String SerieNFCe = txtSerieNFCe.Text;
            String justificativa = txtJustificativa.Text;
            String acao = "INUTILIZACAO DE FAIXA";
            String ip = GlbVariaveis.glb_IP;

            FuncoesNFC fn = new FuncoesNFC();

            if(fn.inutilizarNFCeReq(filial, NumeroInicialNFCe, NumeroFinalNFCe, SerieNFCe, "", acao, ip, justificativa))
            {
                frmOperadorTEF msg = new frmOperadorTEF(fn.inutilizarNFCeResp(), false);
                msg.mostarIQCARD = false;
                msg.ShowDialog();
                limparVariaveisInutilizacao();
                pnlInutilizarNfce.Visible = false;
            }
            else
            {
                MessageBox.Show("Erro na inutilização da numeração, tente novamente!");
            }

            msg1.Dispose();
        }

        private void btnAbrirPnlInutilizarNFCe_Click(object sender, EventArgs e)
        {

            pnlInutilizarNfce.Visible = true;
            txtSerieNFCe.Text = int.Parse(ConfiguracoesECF.NFCserie).ToString();
            limparVariaveisInutilizacao();
        }

        private void btnSairInutilizarNfe_Click(object sender, EventArgs e)
        {
            pnlInutilizarNfce.Visible = false;
            limparVariaveisInutilizacao();
            txtDocumento.Focus();
        }

        private void limparVariaveisInutilizacao()
        {
            txtNumeroInicialNFCe.Text = "0";
            txtNumeroFinalNFCe.Text = "0";
            txtJustificativa.Text = "";
        }


        private bool VerificarNumero()
        {
            try
            {
                int.Parse(txtNumeroInicialNFCe.Text);
                if (int.Parse(txtNumeroInicialNFCe.Text) <= 0)
                {
                    MessageBox.Show("Digite um número inteiro válido!");
                    return false;
                }
                if (int.Parse(txtNumeroFinalNFCe.Text) < int.Parse(txtNumeroInicialNFCe.Text))
                {
                    MessageBox.Show("O número final deve ser maior que o número inicial!");
                    txtNumeroFinalNFCe.Focus();
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Digite um número inteiro válido!");
                return false;
            }
            return true;
        }

        private void txtNumeroFinalNFCe_Leave(object sender, EventArgs e)
        {
            try
            {
                int.Parse(txtNumeroFinalNFCe.Text);
                if (int.Parse(txtNumeroFinalNFCe.Text) < int.Parse(txtNumeroInicialNFCe.Text))
                {
                    MessageBox.Show("O número final deve ser maior que o número inicial!");
                    txtNumeroFinalNFCe.Focus();
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("Digite um número inteiro válido!");
                txtNumeroFinalNFCe.Focus();
            }

        }

        private void bntConsultar_Click(object sender, EventArgs e)
        {
            string tabela = "nfe012";
            string SQL = "";
            string arquivado = "N";

            FrmMsgOperador msg = new FrmMsgOperador("", "Consultando NFCe ");
            FuncoesNFC NFCe = new FuncoesNFC();
            msg.Show();
            Application.DoEvents();

            try
            {
                if (chktodos.Checked == true)
                {
                    int atual = 0;
                    int total = DgDocumentos.Rows.Count;

                    foreach (DataGridViewRow item in DgDocumentos.Rows)
                    {
                        try
                        {
                            int DOc = int.Parse(item.Cells["documento"].Value.ToString());

                            var documento = (from c in Conexao.CriarEntidade().contdocs
                                             where c.documento == DOc
                                             select c).FirstOrDefault();


                            if (Configuracoes.cfgarquivardados == "S")
                            {
                                SQL = "SELECT cbdArquivado FROM cbd001 WHERE cbdDocumento = '" + DOc + "'";
                                arquivado = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                if (arquivado == "S")
                                    tabela = "nfe012arquivo";
                            }

                            if (documento.protocolo == null || documento.protocolo == "")
                            {
                                MessageBox.Show("Não é possivel consultar um NFCe não Autorizado", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                FuncoesNFC objFuncoes = new FuncoesNFC();
                                objFuncoes.GerarReq(documento.ncupomfiscal.ToString(), documento.ecfcontadorcupomfiscal.ToString(), documento.documento.ToString(), "P", GlbVariaveis.glb_IP, "");
                                var dadosNFC = objFuncoes.LerResp("P", true);

                                SQL = "SELECT CbdStsRetCodigo FROM "+ tabela + " AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.cbddocumento = '" +DOc + "' AND CbdMod = '65' ";
                                string status = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                MessageBox.Show(" Status da NFCe ("+status+") Atualizado! ","Atenção",MessageBoxButtons.OK,MessageBoxIcon.Information);

                            }

                        }
                        catch (Exception erro)
                        {
                            MessageBox.Show(erro.ToString());
                        }
                    }
                }
                else
                {
                    int DOc = int.Parse(DgDocumentos.CurrentRow.Cells[0].Value.ToString());

                    var documento = (from c in Conexao.CriarEntidade().contdocs
                                     where c.documento == DOc
                                     select c).FirstOrDefault();

                    if (Configuracoes.cfgarquivardados == "S")
                    {
                        SQL = "SELECT cbdArquivado FROM cbd001 WHERE cbdDocumento = '" + DOc + "'";
                        arquivado = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                        if (arquivado == "S")
                            tabela = "nfe012arquivo";
                    }

                    if (documento.protocolo == null || documento.protocolo == "")
                    {
                        MessageBox.Show("Não é possivel consultar um NFCe não Autorizado", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        FuncoesNFC objFuncoes = new FuncoesNFC();
                        objFuncoes.GerarReq(documento.ncupomfiscal.ToString(), documento.ecfcontadorcupomfiscal.ToString(), documento.documento.ToString(), "P", GlbVariaveis.glb_IP, "");
                        var dadosNFC = objFuncoes.LerResp("P", true);

                       

                            /*DgDocumentos.DataSource = (from doc in Conexao.CriarEntidade().contdocs
                                                       where doc.documento == DOc
                                                       orderby doc.documento, doc.ncupomfiscal
                                                       select new
                                                       {
                                                           Documento = doc.documento,
                                                           Estornado = doc.estornado,
                                                           VlrTotal = doc.Totalbruto,
                                                           Desconto = doc.desconto,
                                                           Total = doc.total,
                                                           Data = doc.data,
                                                           Numero = doc.ncupomfiscal,
                                                           Serie = doc.ecfcontadorcupomfiscal,
                                                           Chave = doc.chaveNFC,
                                                           Protocolo = doc.protocolo,
                                                           Modelo = doc.modeloDOCFiscal,
                                                           Vendedor = doc.vendedor,
                                                           NFe = doc.nrnotafiscal,
                                                           Operador = doc.operador,
                                                           Cliente = doc.nome
                                                       }).ToList();*/

                           var documentGrid = (from doc in Conexao.CriarEntidade().contdocs
                                             where doc.documento == DOc
                                             orderby doc.documento, doc.ncupomfiscal
                                             select new
                                             {
                                                 Documento = doc.documento,
                                                 Estornado = doc.estornado,
                                                 VlrTotal = doc.Totalbruto,
                                                 Desconto = doc.desconto,
                                                 Total = doc.total,
                                                 Data = doc.data,
                                                 Numero = doc.ncupomfiscal,
                                                 Serie = doc.ecfcontadorcupomfiscal,
                                                 Chave = doc.chaveNFC,
                                                 Protocolo = doc.protocolo,
                                                 Modelo = doc.modeloDOCFiscal,
                                                 Vendedor = doc.vendedor,
                                                 NFe = doc.nrnotafiscal,
                                                 Operador = doc.operador,
                                                 Cliente = doc.nome
                                             }).FirstOrDefault();

                            


                        SQL = "SELECT CbdStsRetCodigo FROM "+ tabela + " AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND abs(c.CbdNtfNumero) = '" + int.Parse(documentGrid.Numero).ToString() + "' AND abs(CbdNtfSerie) = '"+int.Parse(documentGrid.Serie).ToString()+"' ";
                        string status = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                        MessageBox.Show(" Status da NFCe (" + status + ") Atualizado! ", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DgDocumentos.DataSource = documentGrid;
                    }
                }

            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }
            finally
            {
                msg.Dispose();
            }
        }
    }

    class NFC
    {
        public string Documento { get; set; }
        public string Estornado { get; set; }
        public string VlrTotal { get; set; }
        public string Desconto { get; set; }
        public string Total { get; set; }
        public string Data { get; set; }
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string Chave { get; set; }
        public string Protocolo { get; set; }
        public string Modelo { get; set; }
        public string Vendedor { get; set; }
        public string NFe { get; set; }
        public string Operador { get; set; }
        public string Cliente { get; set; }
    }
}

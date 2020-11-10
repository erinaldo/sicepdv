using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
namespace SICEpdv
{
    public partial class Sincronizar : Form
    {
        private IEnumerable<StandAloneControneSincronizacao> listDocumentos;

        public Sincronizar()
        {
            InitializeComponent();
        }

        private void Sincronizar_Load(object sender, EventArgs e)
        {
            carregarDocumentoSincronizar();

        }

        private void carregarDocumentoSincronizar()
        {
            if (Conexao.tipoConexao == 1)
            {
                if (StandAlone.QuantidadeRegistro() == 0)
                {
                    MessageBox.Show("Sem registro para sincronizar", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnIniciar.Enabled = false;
                }
                lblQtdReg.Text = StandAlone.QuantidadeRegistro().ToString();

                try
                {
                    lblDoc.Text = StandAlone.SequenciaDoc().ToString();
                }
                catch
                {
                    //  btnIniciar.Enabled = false;
                    btnIniciar.Enabled = false;
                    lblStatus.Text = "Sem comunicação";
                    lblStatus.BackColor = Color.Red;
                }
            }
            else
            {
                /*string SQL = "SELECT documentoOrigem,documentoDestino,crediario,caixa,venda,cartao,cheque,produtos,vendanf,contnfsaida,datasincronizacao FROM controlesincronizacao WHERE documentoDestino = 0";

                listDocumentos = Conexao.CriarEntidade(false).ExecuteStoreQuery<StandAloneControneSincronizacao>(SQL);
                */

                var listDocumentos = StandAlone.documentosSincrolizar();
                var documentos = (from d in listDocumentos
                                  select new { documento = d.documentoOrigem }).ToList();

                dgDocumentos.DataSource = documentos.ToList();
                lblQtdReg.Text = documentos.Count().ToString();
                if(documentos.Count() > 0)
                    lblDoc.Text = documentos.FirstOrDefault().documento.ToString();
                else
                    lblDoc.Text = "0";


                if (Conexao.ConexaoOnline() == false)
                {
                    btnIniciar.Enabled = false;
                    lblStatus.Text = "Sem comunicação";
                    lblStatus.BackColor = Color.Red;
                }


                if (documentos.Count() == 0)
                    btnIniciar.Enabled = false;
                else
                    btnIniciar.Enabled = true;

            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            StandAlone atualizar = new StandAlone();
            FrmMsgOperador frmmsg = new FrmMsgOperador("","Sincronizando dados !");
            frmmsg.Show();
            Application.DoEvents();
            if (Conexao.tipoConexao == 1)
            {
                try
                {
                    atualizar.Sincronizar();
                    MessageBox.Show("Atualização bem sucedida!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Não foi possível sincronizar: " + erro.InnerException.ToString());
                }
                finally
                {
                    frmmsg.Dispose();
                }
            }
            else
            {
                /*string SQL = "SELECT documentoOrigem,documentoDestino,crediario,caixa,venda,cartao,cheque,produtos,vendanf,contnfsaida,datasincronizacao FROM controlesincronizacao WHERE documentoDestino = 0";

                listDocumentos = Conexao.CriarEntidade(false).ExecuteStoreQuery<StandAloneControneSincronizacao>(SQL);
                */
                var listDocumentos = StandAlone.documentosSincrolizar();
                progressBar1.Value = 0;
                int i = 0;
                foreach (var item in listDocumentos)
                {
                    atualizar.Sincronizar(item.documentoOrigem);
                    progressBar1.Value = i++;
                    Application.DoEvents();
                }
            }
            frmmsg.Dispose();
            carregarDocumentoSincronizar();
        }

        private void Sincronizar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                ChamarMenuFiscal();
               
        }

        private static void ChamarMenuFiscal()
        {
            FrmMenuFiscal frmFiscal = new FrmMenuFiscal();
            frmFiscal.ShowDialog();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            ChamarMenuFiscal();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class Suporte : Form
    {
        public Suporte()
        {
            InitializeComponent();
        }

        private void Suporte_Load(object sender, EventArgs e)
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Chamando suporte");
            lblSuporte.Text = "";
            try
            {
                
                msg.Show();
                Application.DoEvents();
                string iqcardSuporte = GlbVariaveis.iqcardsuporte;
                string iqcardSuporteNome = GlbVariaveis.iqcardsuporteNome;                
                if (string.IsNullOrEmpty(iqcardSuporte))
                {
                    
                    ServiceReference2.WSClientesClient suporte = new ServiceReference2.WSClientesClient();
                    var dados = suporte.DadosCliente(Convert.ToInt16(GlbVariaveis.idCliente));
                    iqcardSuporte = dados.iqcardsuporte;
                    iqcardSuporteNome = dados.conjuge;                    
                    GlbVariaveis.iqcardsuporte = dados.iqcardsuporte;
                    GlbVariaveis.iqcardsuporteNome = dados.conjuge;
                    lblSuporte.Text = dados.conjuge;
                    this.Text ="ID: "+ dados.codigo + " " + dados.fantasia;                                       
                }

                if(!string.IsNullOrEmpty(iqcardSuporte))
                {
                    pnlSuporte.Visible = true;
                    imgSuporte.Image = Funcoes.GerarQRCode(100, 100,iqcardSuporte+"chat");
                    lblSuporte.Text = GlbVariaveis.iqcardsuporteNome;
                }
                else
                {
                    pnlSuporte.Visible = false;
                }
            }
            catch (Exception)
            {
                pnlSuporte.Visible = false;                
            }
            finally
            {
                msg.Dispose();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://play.google.com/store/apps/details?id=com.iqsistemas.iqcard");
        }
    }
}

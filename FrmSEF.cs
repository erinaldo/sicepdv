using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SICEpdv
{
    public partial class FrmSEF : Form
    {
        public FrmSEF()
        {
            InitializeComponent();
        }
       
        private void btnGerar_Click_1(object sender, EventArgs e)
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando SEF, pode demorar vários minutos.");
            msg.Show();
            Application.DoEvents();
            SEF sef = new SEF();

            sef.dataInicial = dataInicial.Value.Date;
            sef.dataFinal = dataFinal.Value.Date;
            sef.numeroInventario = Convert.ToInt16(txtNrInventario.Value);
            sef.anoInventario = Convert.ToInt16(txtAnoInv.Text);

            sef.codigoLayout = 2;// Convert.ToInt16(cboEstrutura.SelectedValue);
            sef.codigoNaturezaOperacao = Convert.ToInt16(cboNatureza.SelectedValue);
            sef.codigoFinalidade = Convert.ToInt16(cboFinalidade.SelectedValue);

            
            
            try
            {
                if (rdEntradas.Checked)
                {
                    sef.nomeArquivo = "SEF_Entradas.txt";
                    sef.GerarEntradas();
                }

                if (rdSaidas.Checked)
                {
                    sef.nomeArquivo = "SEF_Saidas.txt";
                    sef.GerarSaidas();
                }

                if (rdMapa.Checked)
                {
                    sef.nomeArquivo = "SEF_MapaResumo.txt";
                    sef.GerarMapaResumo();
                }
                

                if (Convert.ToInt16(txtNrInventario.Value) > 0 && rdInventario.Checked)
                {
                    sef.nomeArquivo = "SEF_Inventario.txt";
                    sef.GerarInventario();
                }

                MessageBox.Show(@"Arquivos gerados com sucesso na pasta c:\iqsistemas\sice.net\exportacao\"+sef.nomeArquivo);
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
            finally
            {
                msg.Dispose();
            }            
        }

        private void FrmSEF_Load(object sender, EventArgs e)
        {
            dataInicial.Value = Convert.ToDateTime("01/" + DateTime.Now.Date.ToString("MM/yyyy"));
            dataFinal.Value = dataInicial.Value.AddMonths(1).AddDays(-1);
            cboEstrutura.DataSource = SEF.EstruturaLayoutSEF();
            cboFinalidade.DataSource = SEF.FinalidadeArquivoSEF();
            cboNatureza.DataSource = SEF.NaturezaOperSintegra();
            cboNatureza.SelectedIndex = 2;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\iqsistemas\sice.net\exportacao\sef"); 
        }
    }
}

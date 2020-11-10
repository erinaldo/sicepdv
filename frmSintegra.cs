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
    public partial class frmSintegra : Form
    {
        public frmSintegra()
        {
            InitializeComponent();
        }

        private void frmSintegra_Load(object sender, EventArgs e)
        {
            dataInicial.Value = Convert.ToDateTime("01/"+  DateTime.Now.Date.ToString("MM/yyyy"));
            dataFinal.Value = dataInicial.Value.AddMonths(1).AddDays(-1);
            cboEstrutura.DataSource = Sintegra.EstruturaLayoutSintegra();            
            cboFinalidade.DataSource = Sintegra.FinalidadeArquivoSintegra();
            
            cboNatureza.DataSource = Sintegra.NaturezaOperSintegra();
            cboNatureza.SelectedIndex = 2;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando sintegra");
            msg.Show();
            Application.DoEvents();
            Sintegra sintegra = new Sintegra();
            sintegra.dataInicial = dataInicial.Value.Date;            
            sintegra.dataFinal = dataFinal.Value.Date;
            sintegra.codigoLayout = 3;// Convert.ToInt16(cboEstrutura.SelectedValue);
            sintegra.codigoNaturezaOperacao = Convert.ToInt16(cboNatureza.SelectedValue);
            sintegra.codigoFinalidade = Convert.ToInt16(cboFinalidade.SelectedValue);
            sintegra.numeroInventario = Convert.ToInt16(txtNrInventario.Text);
            sintegra.anoInventario = Convert.ToInt16(txtAnoInv.Text);
            try
            {

                if (sintegra.numeroInventario > 0)
                {

                    sintegra.nomeArquivo = @"C:\iqsistemas\sice.net\exportacao\SEF\SINTEGRA_Inventario.txt";
                    sintegra.GerarInventario();
                }

                if (rdEntradas.Checked && !chkTodos.Checked)
                {
                    sintegra.nomeArquivo = @"C:\iqsistemas\sice.net\exportacao\SEF\SINTEGRA_entradas.txt";
                    sintegra.GerarEntradas();
                }

                if (rdSaidas.Checked && !chkTodos.Checked)
                {
                    sintegra.nomeArquivo = @"C:\iqsistemas\sice.net\exportacao\SEF\SINTEGRA_saidas.txt";
                    sintegra.GerarSaidas();
                }

                if (rdMapa.Checked && !chkTodos.Checked)
                {
                    //sef.nomeArquivo = "Mapa Resumo.txt";
                    sintegra.nomeArquivo = @"C:\iqsistemas\sice.net\exportacao\SEF\SINTEGRA_MapaResumo.txt";
                    sintegra.GerarMapaResumo();
                }

                if (chkTodos.Checked)
                {
                    sintegra.nomeArquivo = @"C:\iqsistemas\sice.net\exportacao\SEF\SINTEGRA_ENT_SAI_MapaResumo.txt";
                    sintegra.GeraTodos();
                }
                MessageBox.Show(@"Arquivos gerados com sucesso na pasta: "+sintegra.nomeArquivo);
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

        private void btnSintegra_Click(object sender, EventArgs e)
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando sintegra do ECF");
            msg.Show();
            Application.DoEvents();
            try
            {
                FuncoesECF.GerarSintegraECF(dataInicial.Value.Date.Month, dataInicial.Value.Date.Year, dataInicial.Value, dataFinal.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                msg.Dispose();
            }

            MessageBox.Show("Gerado em: " + @ConfiguracoesECF.pathRetornoECF + "SINTEGRA.TXT");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\iqsistemas\sice.net\exportacao\sef");
        }
    }
}

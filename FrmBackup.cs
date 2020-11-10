using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Configuration;

namespace SICEpdv
{
    public partial class FrmBackup : Form
    {
        public FrmBackup()
        {
            InitializeComponent();
        }

        private void FrmBackup_Load(object sender, EventArgs e)
        {           
            cboDrivers.DataSource = DriveInfo.GetDrives().ToList();
            CultureInfo ci = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtf = ci.DateTimeFormat;
            lblNomeArquivo.Text = "PAF_"+ConfiguracoesECF.numeroECF+"_"+dtf.GetDayName(DateTime.Now.DayOfWeek).Replace("-","_")+".zip";            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmMsgOperador msg = new FrmMsgOperador("","Salvando backup !");
            msg.Show();
            Application.DoEvents();
            try
            {
                List<String> origem = new List<string>();
                origem.Add(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["EcfMovimentoDiario"]);
                origem.Add(@Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirEspelhoECF"]);

                Funcoes.CompactarDiretorio(origem, @cboDrivers.Text + lblNomeArquivo.Text);
                MessageBox.Show("Backup realizado com sucesso.");
                if (chkCloud.Checked)
                {
                    var drive = (from n in Conexao.CriarEntidade().iqsistemas
                                 select n.idcliente).FirstOrDefault().ToString();
                   string validade = DateTime.Now.ToShortDateString();                                        
                   string chave = Funcoes.CriptografarMD5(drive + validade + GlbVariaveis.chavePrivada);
                   string link = @"https://siceweb.azurewebsites.net/DriveClientes/Novo?credenciais=s3amTcTmGjxFxgGiB1qfDqOFTEFGaA8C10yYwQx/XLeyhkJEx/dWri/VcQ7PMCiNsNQ5DUvmT8hX2hXS45UEfwnh4STk1BuKlLKHu2kImlYvvquwF3oj8hqLBUzlHEs7N2IxrvilAVHJrHoekJx77hBFaVJZOsq0jSYJZdpuWcaXw3mm3R9oz/wsI0bNPK6YiBsBkVxDJ+2r0WcBTdnEpry+HZFMm2VuXFMJuWy5Xy1gAnZg1mjlxr43WeJHsOSIQdrmkPhDKubzrR+87X4OtuVZ+YHJ9zzb5YqeaYgvRIrxrspQGdrn3iP9vngaeyWjNS8jcibx/AUY8PwgJ/b3zQ==&drive="+drive+"&chave="+chave+"&validade="+validade+"&origem=IQCloud";
                   System.Diagnostics.Process.Start(link);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                msg.Dispose();
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

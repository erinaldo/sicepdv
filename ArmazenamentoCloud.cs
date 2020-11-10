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
    public partial class ArmazenamentoCloud : Form
    {
        public static int idCliente;
        public static string nome;
        public static string drive = "";
        DriveCloud cloud = new DriveCloud();
        OpenFileDialog file = new OpenFileDialog();
        

        public ArmazenamentoCloud()
        {
            InitializeComponent();
        }

        private void ArmazenamentoCloud_Load(object sender, EventArgs e)
        {

            
            pnlEnviar.Visible = false;
            lblCliente.Text = idCliente.ToString() + " " + nome;
            dtgArquivos.AutoGenerateColumns = false;

            if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
            {
                pnlContratar.Visible = true;
            }
            else
                pnlContratar.Visible = false;

            MostrarArquivos();

        }

        private void MostrarArquivos()
        {
            var dadosCloud = cloud.ListarArquivos(drive);
            dtgArquivos.DataSource = dadosCloud.AsQueryable().ToList();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Enviar();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "";
           

      
            if (file.ShowDialog() == DialogResult.OK)
            {
                lblInfo.Text = file.FileName;                      
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            pnlEnviar.Visible = false;
            lblInfo.Text = "";
            txtDescricao.Text = "";
        }

        private void dtgArquivos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (!Funcoes.VerificarConexaoInternet())
                {
                    MessageBox.Show("Sem internet");
                    return;
                }


                if (!Permissoes.iqsCloud)
                {
                    FrmLogon logon = new FrmLogon();
                    logon.campo = "gerente";
                    logon.txtDescricao.Text = "Permissão de gerente";
                    logon.ShowDialog();
                    if (!Operador.autorizado)
                    {
                        MessageBox.Show("Sem permissão");
                        return;
                    }

                    Permissoes.iqsCloud = true;
                   
                }

                FrmMsgOperador msg = new FrmMsgOperador("", "Baixando arquivo");
                msg.Show();
                Application.DoEvents();

                try
                {
                    if (!Directory.Exists(@Application.StartupPath + @"\IQSCLOUD"))
                    {
                        Directory.CreateDirectory(@Application.StartupPath + @"\IQSCLOUD");
                    }
                    string arquivo = dtgArquivos.CurrentRow.Cells["arquivo"].Value.ToString();
                    DriveCloud cloud = new DriveCloud();
                    var dados = cloud.Download(drive, arquivo);


                    File.WriteAllBytes(@Application.StartupPath + @"\IQSCLOUD\"+arquivo, dados);
                    System.Diagnostics.Process.Start(@Application.StartupPath + @"\IQSCLOUD\"+arquivo);
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


            if (e.ColumnIndex == 3)
            {

                if (!Funcoes.VerificarConexaoInternet())
                {
                    MessageBox.Show("Sem internet");
                    return;
                }


                DialogResult resultado = MessageBox.Show("Confirma exclusão definitiva do arquivo?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.No)
                {
                    return;
                }

                Operador.autorizado = false;
                FrmLogon logon = new FrmLogon();
                logon.campo = "gerente";
                logon.txtDescricao.Text = "Permissão de gerente";
                logon.ShowDialog();
                if (!Operador.autorizado)
                {
                    MessageBox.Show("Sem permissão");
                    return;
                }



                FrmMsgOperador msg = new FrmMsgOperador("", "Apagando arquivo");
                msg.Show();
                Application.DoEvents();

                try
                {
                    string arquivo = dtgArquivos.CurrentRow.Cells["arquivo"].Value.ToString();
                    DriveCloud cloud = new DriveCloud();
                    var dados = cloud.Apagar(drive, arquivo);
                    File.Delete(arquivo);MostrarArquivos();
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


        }

        private void btnEnviar_Click_1(object sender, EventArgs e)
        {

            if (!Funcoes.VerificarConexaoInternet())
            {
                MessageBox.Show("Sem internet");
                return;
            }

            if (string.IsNullOrEmpty(txtDescricao.Text))
            {
                MessageBox.Show("Coloque uma descrição");
                return;
            }


            FrmMsgOperador msg = new FrmMsgOperador("", "Enviando arquivo");
            msg.Show();
            Application.DoEvents();
            try
            {
                string extensao = Path.GetExtension(file.FileName);

                long tamanhoArquivo = new System.IO.FileInfo(file.FileName).Length;

               if(tamanhoArquivo>13000000)
                {
                    MessageBox.Show("Arquivo muito grande. Use arquivo com até 10MB");
                    return;
                };

                byte[] buffer = File.ReadAllBytes(file.FileName);
                if (cloud.UploadFile(buffer, drive, txtDescricao.Text, idCliente, nome, extensao))
                {
                    lblInfo.Text = "Arquivo enviado com sucesso.";
                }
                MostrarArquivos();
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void btnContratar_Click(object sender, EventArgs e)
        {
            if (!Funcoes.VerificarConexaoInternet())
            {
                MessageBox.Show("Sem internet");
                return;
            }
            FrmMsgOperador msg = new FrmMsgOperador("", "Enviando solicitação");
            msg.Show();
            Application.DoEvents();
            try
            {                
                ServiceReference1.WSIQPassClient x = new ServiceReference1.WSIQPassClient();
                string mensagem = GlbVariaveis.idCliente + " " + GlbVariaveis.nomeEmpresa + "Tel.: "+GlbVariaveis.telefone+" Tem interesse no serviço de armazenamento digital";
                var resultado = x.EnviarEmail(GlbVariaveis.chavePrivada, "iqsistemas@iqsistemas.com.br", "no-reply@iqsistemas.com.br", "ARMAZENAMENTO EM NUVEM IQS CLOUD", mensagem, GlbVariaveis.nomeEmpresa);
                MessageBox.Show("Obrigado pelo interesse. Entraremos em conto o quanto antes");
                pnlContratar.Visible = false;
                btnEnviar.Enabled = false;
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

        private void btnNao_Click(object sender, EventArgs e)
        {
            pnlContratar.Visible = false;
            btnEnviar.Enabled = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Enviar();
        }

        private void Enviar()
        {
            Operador.autorizado = false;
            if (!Permissoes.iqsCloud)
            {
                FrmLogon logon = new FrmLogon();
                logon.campo = "gerente";
                logon.txtDescricao.Text = "Permissão de gerente";
                logon.ShowDialog();
                if (!Operador.autorizado)
                {
                    MessageBox.Show("Sem permissão");
                    return;
                }
                Permissoes.iqsCloud = true;
            }

            
            pnlEnviar.Visible = true;
            txtDescricao.Focus();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            Enviar();
        }
    }
}

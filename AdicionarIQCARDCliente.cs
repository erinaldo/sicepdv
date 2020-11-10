using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class AdicionarIQCARDCliente : Form
    {
        public static int idCliente = 0;
        public string iqcard = "";
        public AdicionarIQCARDCliente()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            FrmLogon Logon = new FrmLogon();
            Operador.autorizado = false;
            Logon.idCliente = 0;
            Logon.campo = "clialterarcad";
            Logon.lblDescricao.Text = "FIDELIDADE IQCARD";
            Logon.txtDescricao.Text = "É necessário ter a permissão de alterar clientes";
            Logon.ShowDialog();
            if (!Operador.autorizado)
                return;

            try
            {
                siceEntities entidade;
                if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();
                string sql = "SELECT nome FROM clientes WHERE cartaofidelidade='" + txtIQCARDFidelidade.Text + "'";


                if (!string.IsNullOrEmpty(iqcard) && string.IsNullOrEmpty(txtIQCARDFidelidade.Text))
                {
                    sql = "UPDATE clientes set cartaofidelidade='" + txtIQCARDFidelidade.Text + "' WHERE codigo='" + idCliente + "' LIMIT 1 ";
                    entidade.ExecuteStoreCommand(sql);
                    MessageBox.Show("IQCARD associado ao cliente foi retirado");
                    Close();
                }

                if (!IqCard.VerificarNumeroCartao(txtIQCARDFidelidade.Text))
                {
                    MessageBox.Show("IQCARD inválido");
                    return;
                }

               

                
                var resultado = entidade.ExecuteStoreQuery<string>(sql).FirstOrDefault();

                if (!string.IsNullOrEmpty(resultado))
                {
                    MessageBox.Show("IQCARD já está associado ao cliente: " + resultado);
                    return;
                }

                sql = "UPDATE clientes set cartaofidelidade='" + txtIQCARDFidelidade.Text + "' WHERE codigo='" + idCliente + "' LIMIT 1 ";
                entidade.ExecuteStoreCommand(sql);
                MessageBox.Show("Parabéns! Agora você tem mais um consumidor conectado");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }

        private void AdicionarIQCARDCliente_Load(object sender, EventArgs e)
        {

            siceEntities entidade;
            if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
                entidade = Conexao.CriarEntidade(false);
            else
                entidade = Conexao.CriarEntidade();
            string sql = "SELECT cartaofidelidade FROM clientes WHERE codigo='" + idCliente + "'";
            var resultado = entidade.ExecuteStoreQuery<string>(sql).FirstOrDefault();
            txtIQCARDFidelidade.Text = resultado;
            iqcard = resultado;

            webBrowser1.DocumentText =
            "<html>" +
            "<head>" +
            "<meta charset='utf-8' />" +
            "<meta http-equiv = 'X-UA-Compatible' content='IE=edge'>" +
            "<title>Page Title</title>" +
            "<meta name='viewport' content = 'width=device-width, initial-scale=1'>" +
            "<style> html, body, h2 { margin: 0; padding: 0; } </style>" +
            "</head>" +
            "<body style=' margin: 0; padding: 0; background' bgcolor='#004177'> " +
            "<iframe width='100%' style='height:90vh;' src='https://www.youtube.com/embed/OJmRrVJ-szY' frameborder='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe>" +
            "</body></html>";
        }
    }
}

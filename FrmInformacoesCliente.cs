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
    public partial class FrmInformacoesCliente : FormIQ
    {
        public static int codClient; 
        public FrmInformacoesCliente()
        {
            InitializeComponent();

        }
        
        private void FrmInformacoesCliente_Load(object sender, EventArgs e)
        {
            string codigoCliente = codClient.ToString();

            var cliente = (from n in Conexao.CriarEntidade().clientes
                            where n.Codigo == codClient
                            select new
                            {
                                n.Nome,
                                n.cpf,
                                n.telefone,
                                n.telefone2,
                                n.endereco,
                                n.numero,
                                n.bairro,
                                n.cidade,
                                n.cep
                            }).FirstOrDefault();

            lblNomeCliente.Text = cliente.Nome;
            lblCpfCliente.Text = "CPF: " + cliente.cpf;
            lblTelefoneCliente.Text = "FONE: " + cliente.telefone;
            // cliente.telefone2;
            lblEndereco.Text = cliente.endereco + ", Nº " + cliente.numero + " - " + cliente.bairro;
            lblCidadeCliente.Text = cliente.cidade;
            lblCepCliente.Text = "CEP: " + cliente.cep;

            try
            {
                byte[] foto = (from n in Conexao.CriarEntidade().clientesfoto
                               where n.codcli == codigoCliente
                               select n.fotocli).First();

                MemoryStream ms = new MemoryStream(foto);
                pictureBox1.Image = Image.FromStream(ms);
            }
            catch (Exception)
            {
                pictureBox1.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\user.jpg"); ;
                // return;
            }





        }
    }
}

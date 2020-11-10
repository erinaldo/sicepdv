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
    public partial class TokenIQCARD : Form
    {
        public TokenIQCARD()
        {
            InitializeComponent();
        }

        private void TokenIQCARD_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT tokeniqcard FROM filiais WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                txtToken.Text = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

            }
            catch (Exception)
            {
                txtToken.Text = "";
            }
           
        }

        private void label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://iqcard.com.br");
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txtToken.Text))
                try
                {
                    IqCard iqcard = new IqCard();
                    var dadosEmpresa = iqcard.DadosEmpresa(txtToken.Text);
                    if (dadosEmpresa == null)
                    {
                        throw new Exception("Token inválido");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }               

                string sql = "UPDATE filiais SET tokeniqcard='" + txtToken.Text + "' WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                GlbVariaveis.glb_chaveIQCard = txtToken.Text;
                MessageBox.Show("Salvo com sucesso");
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao salvar token");
            }
           
        }
    }
}

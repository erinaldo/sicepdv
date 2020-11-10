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
    public partial class AvisoVencimento : Form
    {
        string ativo = "";
        public AvisoVencimento()
        {
            InitializeComponent();
        }

        private void AvisoVencimento_Load(object sender, EventArgs e)
        {
            MostrarDados();
        }

        private void MostrarDados()
        {
            string sql = "SELECT ativo FROM lembretevencimento WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";

            try
            {
                ativo = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
            }
            catch
            {

            }

            if (ativo == "S")
            {
                lblStatus.BackColor = System.Drawing.Color.Green;
                lblStatus.Text = "ON";
                lblInfoStatus.Text = "Lembrete ligado";
            }
            else
            {
                lblStatus.BackColor = System.Drawing.Color.Red;
                lblStatus.Text = "OFF";
                lblInfoStatus.Text = "Lembrete desligado";
            }

            if(string.IsNullOrEmpty(ativo))
            {
                sql = "INSERT INTO lembretevencimento (codigofilial,ativo,dataenvio) VALUES('" + GlbVariaveis.glb_filial + "','S',CURRENT_DATE)";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                ativo = "N";
            }
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {
            if(ativo=="N")
            {
                string sql = "UPDATE lembretevencimento SET ativo='S' WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                ativo = "S";                
            }
            else
            {                
                    string sql = "UPDATE lembretevencimento SET ativo='N' WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                    Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                    ativo = "N";                
            };
            MostrarDados();
        }
    }
}

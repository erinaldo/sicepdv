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
    public partial class FrmTipoTerminal : FormIQ
    {
        public FrmTipoTerminal()
        {
            InitializeComponent();

            if (ConfiguracoesECF.pdv == true)
                this.Text = "Terminal (F7 menu Fiscal)";
            else
                this.Text = "Terminal";
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPreVenda_Click(object sender, EventArgs e)
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Carregando configurações");
            try
            {
                msg.Show();
                Application.DoEvents();
                if (ConfiguracoesECF.idECF == 0 || ConfiguracoesECF.pdv == true)
                {
                    Conexao.tipoConexao = 1;
                    ConfiguracoesECF.pdv = false;
                    ConfiguracoesECF.Carregar(false);
                    Configuracoes.carregar(GlbVariaveis.glb_filial);
                }

                if (ConfiguracoesECF.idECF != 0)
                {
                    MessageBox.Show("Prevenda não pode ser feita em um terminal conectado ao ECF");
                    return;
                }
            }
            catch
            {
                ConfiguracoesECF.idECF = 0;
                ConfiguracoesECF.prevenda = true;
                ConfiguracoesECF.idECF = 0;
                ConfiguracoesECF.prevenda = true;
                ConfiguracoesECF.pdv = false;
                ConfiguracoesECF.idECF = 0;
                ConfiguracoesECF.davporImpressoraNaoFiscal = false;
                Configuracoes.carregar(GlbVariaveis.glb_filial);
                

                this.Close();
                return;                
            }
            finally
            {                             
                msg.Dispose();
            }

            ConfiguracoesECF.prevenda = true;
            ConfiguracoesECF.idECF = 0;
            ConfiguracoesECF.davporImpressoraNaoFiscal = false;
            ConfiguracoesECF.pdv = false;
            this.Close();
        }

        private void btnDAV_Click(object sender, EventArgs e)
        {
            ConfiguracoesECF.davporImpressoraNaoFiscal = true;
            ConfiguracoesECF.idECF = 0;
            ConfiguracoesECF.prevenda = false;
            ConfiguracoesECF.pdv = false;
            ConfiguracoesECF.SetarPerfil();
            Configuracoes.carregar(GlbVariaveis.glb_filial);
            Conexao.tipoConexao = 1;
            this.Close();
        }

        private void btnPDV_Click(object sender, EventArgs e)
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Carregando configurações");
            try
            {
                msg.Show();
                Application.DoEvents();
                ConfiguracoesECF.idECF = 0;
                ConfiguracoesECF.pdv = true;
                ConfiguracoesECF.ultIDECF = ConfiguracoesECF.idECF;           
                ConfiguracoesECF.davporImpressoraNaoFiscal = false;
                ConfiguracoesECF.prevenda = false;
                ConfiguracoesECF.davporImpressoraNaoFiscal = false;
                ConfiguracoesECF.Carregar(true);
            }
            catch (Exception erro)
            {
                Application.DoEvents();
                MessageBox.Show(erro.Message);
                ConfiguracoesECF.idECF = 0;
            }
            finally
            {
                msg.Dispose();
                Application.DoEvents();
                this.Close();                
            }
            
        }

        private void FrmTipoTerminal_Load(object sender, EventArgs e)
        {
            /*if (ConfiguracoesECF.idECF != 0  || ConfiguracoesECF.davPorECF || ConfiguracoesECF.davporImpressoraNaoFiscal)
                btnPreVenda.Visible = false;*/

            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                btnDAV.Text = "DAV-OS";
        }

        private void FrmTipoTerminal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7 && ConfiguracoesECF.pdv == true)
                FuncoesPAFECF.ChamarMenuFiscal();
        }
    }
}

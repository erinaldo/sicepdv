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

    public partial class FrmMsgOperador : Form
    {
        public static string MessagemExterta = "";
        private static FrmMsgOperador instance;
        public string mensagem2 { get; set; }

        public FrmMsgOperador()
        {
            lblMensagem.Text = this.mensagem2;
            panel1.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\aguarde.png"); ;
        }

        public static FrmMsgOperador Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FrmMsgOperador();
                }
                return instance;
            }
        }



        /*
         * SINGLETON INCOMPLETO (SUELYTOHM)
         * 
         
        private static FrmMsgOperador instance;

        // private static string mensagem2 = "";
        public string mensagem2 { get; set; }

        private FrmMsgOperador()
        {
            lblMensagem.Text = this.mensagem2;
            panel1.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\aguarde.png"); ;
        }

        public static FrmMsgOperador Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FrmMsgOperador();
                }
                return instance;
            }
        }

        */

        string origem = "";
        string mensagem = "";
        public FrmMsgOperador(string tipo ,string msg)
        {
            origem = tipo;
            if (msg == "")
                mensagem = MessagemExterta;

            mensagem = msg;
            InitializeComponent();

            panel1.BackgroundImage = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\imagensMetro\aguarde.png"); ;
        }

        private void _frmTroco_Shown(object sender, EventArgs e)
        {
            lblMensagem.Text = mensagem;
            switch (origem)
            {
                case "troco":
                    this.BackColor = Color.PaleGreen;
                    Application.DoEvents();
            Timer time = new Timer();                    
            time.Interval = 5000;
            time.Enabled = true;
            time.Tick += (objeto, evento) => this.Close();
                    lblMensagem.Font =  new Font("Verdana",20);
            lblMensagem.Text = mensagem;
                    break;

                case "gaveta":
                    lblMensagem.Font = new Font("Arial", 20);
                    int x = 0;
                    int y = 0;
                    while (FuncoesECF.EstadoGaveta() == 0)
                    {
                        y++;
                        lblMensagem.Text = mensagem;
                        Application.DoEvents();
                        if (y > 10)
                            x++;
                    }
                    this.Close();
                    break;
            }

        }                

       
    }

}

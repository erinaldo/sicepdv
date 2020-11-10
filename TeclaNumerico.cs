using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class TeclaNumerico : UserControl
    {
        public delegate void ClicarNoBotao(object sender, string tecla);
        public event ClicarNoBotao clickBotao;
        private Color Cor;

        public TeclaNumerico(Color Cores)
        {
            
            InitializeComponent();

            this.Cor = Cores;
           
            bt0.BackColor = Cor;
            bt1.BackColor = Cor;
            bt2.BackColor = Cor;
            bt3.BackColor = Cor;
            bt4.BackColor = Cor;
            bt5.BackColor = Cor;
            bt6.BackColor = Cor;
            bt7.BackColor = Cor;
            bt8.BackColor = Cor;
            bt9.BackColor = Cor;
            btVirgula.BackColor = Cor;
            btX.BackColor = Cor;

            bt0.Click += (objeto, evento) => clickBotao(objeto, "0");
            bt1.Click += (objeto, evento) => clickBotao(objeto, "1");
            bt2.Click += (objeto, evento) => clickBotao(objeto, "2");
            bt3.Click += (objeto, evento) => clickBotao(objeto, "3");
            bt4.Click += (objeto, evento) => clickBotao(objeto, "4");
            bt5.Click += (objeto, evento) => clickBotao(objeto, "5");
            bt6.Click += (objeto, evento) => clickBotao(objeto, "6");
            bt7.Click += (objeto, evento) => clickBotao(objeto, "7");
            bt8.Click += (objeto, evento) => clickBotao(objeto, "8");
            bt9.Click += (objeto, evento) => clickBotao(objeto, "9");
            btVirgula.Click += (objeto,evento) => clickBotao(objeto,",");
            btEnter.Click += (objeto, evento) => clickBotao(objeto, "Enter");            
            btLimpar.Click += (objeto, evento) => clickBotao(objeto, "Limpar");
            btX.Click += (objeto, evento) => clickBotao(objeto, "X");

        }
    }
}

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
    public partial class UCMoedas : UserControl
    {
        public delegate void ClicarNoBotao(decimal valor);
        public event ClicarNoBotao clickBotao;
        public UCMoedas()
        {
            InitializeComponent();
        }

        private void UCMoedas_Load(object sender, EventArgs e)
        {
            Cedulas cedulas = new Cedulas();
            var colcedulas = cedulas.ListagemCedulas();

            int y = 0;
            foreach (var item in colcedulas)
            {
                Button btn = new Button();
                btn.Name = item.id.ToString();
                btn.Text = "";
                btn.Image = item.img;
                btn.Size = new Size(250, 90);
                btn.Location = new System.Drawing.Point(0, y);
                btn.TabStop = false;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                btn.Click += (objeto, evento) =>
                {
                    btn.BackColor = Color.Green;
                    clickBotao((from n in colcedulas where n.id == Convert.ToInt16(btn.Name) select n.valor).First());
                    btn.Text = (Convert.ToInt16("0" + btn.Text.PadRight(1, '0').Substring(0, 1)) + 1).ToString() + " X";
                };
                y = y + 90;
                this.Controls.Add(btn);
            }
        }
    }    
}

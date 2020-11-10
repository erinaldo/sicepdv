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
    public partial class FormIQ : Form
    {
        public FormIQ()
        {
            InitializeComponent();
        }

        public void fadeIn()
        {
            for (var fade = 0.0; fade <= 1.1; fade += 0.1)
            {
                this.Opacity = fade;
                this.Refresh();
                System.Threading.Thread.Sleep(7);
            }

        }

        public void fadeOut()
        {
            for (var fade = 90; fade >= 10; fade += -20)
            {
                this.Opacity = fade / (double)100;
                this.Refresh();
                System.Threading.Thread.Sleep(1);
            }
        }

        private void FormIQ_Shown(object sender, EventArgs e)
        {
            fadeIn();
        }

        private void FormIQ_FormClosed(object sender, FormClosedEventArgs e)
        {
            fadeOut();
        }
    }
}

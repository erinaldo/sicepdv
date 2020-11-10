using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;

namespace SICEpdv
{
    public partial class RedeCartoes : Form
    {
        XElement xmlCartoes = XElement.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\RedeCartoes.xml", LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
        ///Sumary
        ///As rede de Cartões são geradas através do XML
        ///se ocorrer algum erro o XML será criado como default na sequência
        ///string[] rede = { "AMEX","REDECARD","VISANET","TECBAN"};
        ///Erro que possam ocorrer é quanto a permissão de criar arquivo

        
        public RedeCartoes()
        {
            InitializeComponent();

            this.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Escape)
                        this.Close();
                };
            try
            {
                ConstruirCartoes();
            }
            catch
            {
                CriarCartoes();
                ConstruirCartoes();
            }
        }
       
        private void ConstruirCartoes()
        {
            int x = 0;
            const int tamanho = 120;
            int yaltura = 20;

            IEnumerable<XElement> enumerable = xmlCartoes.Elements();
                var redes = enumerable.Elements("Descricao");                                
            
            foreach (var item in redes)            
            {
                if (x > 2)
                {
                    x = 0;
                    yaltura = yaltura + 50; ;
                } 

               
                Button btCartao = new Button();
                btCartao.Location = new System.Drawing.Point(x * tamanho, yaltura);
                btCartao.Name = item.Value;
                btCartao.Size = new System.Drawing.Size(120, 50);
                btCartao.TabIndex = 0;
                btCartao.BackColor = Color.FromArgb(91, 191, 223);
                btCartao.Text = item.Value;
                btCartao.FlatStyle = FlatStyle.Flat;
                btCartao.Margin = new Padding(20);
                btCartao.FlatAppearance.BorderColor = Color.FromArgb(0, 65, 119);
                btCartao.FlatAppearance.BorderSize = 3;
                btCartao.ForeColor = System.Drawing.Color.White;


                btCartao.Click += (objeto, evento) =>
                {
                    this.Enabled = false;
                    var dados = (from n in xmlCartoes.Descendants("REDE")
                                 where n.Element("Descricao").Value == btCartao.Name.ToString()
                                 select n).First();

                    // Aqui para manter os diretório de solicitação e resposta do TEF dedicado
                    if (!ConfiguracoesECF.tefDedicado)
                    {
                        TEF.PathReq = @dados.Element("req").Value;
                        TEF.PathResp = @dados.Element("resp").Value;
                    };

                    FuncoesECF fecf = new FuncoesECF();
                    fecf.AdministrativoTEF(btCartao.Name);
                    this.Enabled = true;
                    
                };
                Controls.Add(btCartao);
                x++;
            }
        }

        private void CriarCartoes()
        {
            string[] rede = {"AMEX","REDECARD","VISANET","TECBAN","HIPERCARD"};
            string[] idRede = { "0", "0", "0", "1", "4" };
            string[] pathReq = { @"c:\tef_dial\req", @"c:\tef_dial\req", @"c:\tef_dial\req", @"c:\tef_disc\req", @"c:\HiperTEF\req" };
            string[] pathResp = { @"c:\tef_dial\resp", @"c:\tef_dial\resp", @"c:\tef_dial\resp", @"c:\tef_disc\resp", @"c:\HiperTEF\resp" };

            XElement documentoXml = new XElement("REDECARTOES");
            XElement element = null;
            
            for (int i = 0; i < rede.Length; i++)
            {              
                element = new XElement("REDE");               
                //Elementos   
                element.SetElementValue("Descricao", rede[i]);
                element.SetElementValue("idRede", idRede[i]);
                element.SetElementValue("req", pathReq[i]);
                element.SetElementValue("resp", pathResp[i]);
                //Adiciona elemento ao elemento root   
                documentoXml.Add(element);
            }
            documentoXml.Save(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\RedeCartoes.xml");   
        }

        private void RedeCartoes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }
 }      
}

using Sharpen.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SICEpdv
{
    class FuncoesBalanca
    {
        static private string modelo;
        static private string porta;
        static private string velocidade;
        static private string dataBits;
        static private string Parity;
        static private string StopBits;
        static private string HandShake;

        static public ACBrFramework.BAL.ACBrBAL acbrBAL = new ACBrFramework.BAL.ACBrBAL();

        static public bool ativarBalanca()
        {


            //if (File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Balanca.xml"))
            //{
            lerXMLBalanca();

            if (GlbVariaveis.glb_Acbr == true)
            {

                if (modelo != "Nenhum")
                {

                    if (modelo == "Filizola")
                        acbrBAL.Modelo = ACBrFramework.BAL.ModeloBal.Filizola;
                    else if (modelo == "Toledo")
                        acbrBAL.Modelo = ACBrFramework.BAL.ModeloBal.Toledo;
                    else if (modelo == "Toledo2180")
                        acbrBAL.Modelo = ACBrFramework.BAL.ModeloBal.Toledo2180;
                    else if (modelo == "Urano")
                        acbrBAL.Modelo = ACBrFramework.BAL.ModeloBal.Urano;
                    else if (modelo == "LucasTec")
                        acbrBAL.Modelo = ACBrFramework.BAL.ModeloBal.LucasTec;
                    else if (modelo == "Magna")
                        acbrBAL.Modelo = ACBrFramework.BAL.ModeloBal.Magna;
                    else if (modelo == "Digitron")
                        acbrBAL.Modelo = ACBrFramework.BAL.ModeloBal.Digitron;
                    else if (modelo == "Magellan")
                        acbrBAL.Modelo = ACBrFramework.BAL.ModeloBal.Magellan;
                    else if (modelo == "UranoPOP")
                        acbrBAL.Modelo = ACBrFramework.BAL.ModeloBal.UranoPOP;


                    acbrBAL.Porta = porta;
                    acbrBAL.Device.Baud = int.Parse(velocidade);
                    acbrBAL.Device.DataBits = int.Parse(dataBits);
                    //acbr.Device.Parity = ACBrFramework.SerialParity.None;

                    if (Parity == "none")
                        acbrBAL.Device.Parity = ACBrFramework.SerialParity.None;
                    else if (Parity == "odd")
                        acbrBAL.Device.Parity = ACBrFramework.SerialParity.Odd;
                    else if (Parity == "even")
                        acbrBAL.Device.Parity = ACBrFramework.SerialParity.Even;
                    else if (Parity == "mark")
                        acbrBAL.Device.Parity = ACBrFramework.SerialParity.Mark;
                    else if (Parity == "space")
                        acbrBAL.Device.Parity = ACBrFramework.SerialParity.Space;


                    //acbr.Device.StopBits = ACBrFramework.SerialStopBits.One;

                    if (StopBits == "One")
                        acbrBAL.Device.StopBits = ACBrFramework.SerialStopBits.One;
                    else if (StopBits == "OneAndHalf")
                        acbrBAL.Device.StopBits = ACBrFramework.SerialStopBits.OneAndHalf;
                    else if (StopBits == "Two")
                        acbrBAL.Device.StopBits = ACBrFramework.SerialStopBits.Two;

                    acbrBAL.Device.HandShake = ACBrFramework.SerialHandShake.None;

                    if (HandShake == "")
                        acbrBAL.Device.HandShake = ACBrFramework.SerialHandShake.None;
                    else if (HandShake == "")
                        acbrBAL.Device.HandShake = ACBrFramework.SerialHandShake.DTR_DSR;
                    else if (HandShake == "")
                        acbrBAL.Device.HandShake = ACBrFramework.SerialHandShake.RTS_CTS;
                    else if (HandShake == "")
                        acbrBAL.Device.HandShake = ACBrFramework.SerialHandShake.XON_XOFF;

                    acbrBAL.Ativar();
                }

                return acbrBAL.Ativo;
            }
            else
            {
                int retorno = Toledo.AbrePorta(int.Parse(porta), 0, 0, 2);

                if (retorno == 1)
                    return true;
                else
                    return false;
            }
        }

        static public decimal lerPeso()
        {
            if (GlbVariaveis.glb_Acbr == true)
                return acbrBAL.LePeso();
            else
            {
                try
                {
                    StringBuilder pesoString = new StringBuilder();
                    Toledo.PegaPeso(1, pesoString, "");
                    decimal peso = decimal.Parse(pesoString.ToString())/1000;
                    return peso;
                }
                catch (Exception erro)
                {
                    return 0;
                }
            }
        }

        static public bool ativo()
        {
            if (GlbVariaveis.glb_Acbr == true)
                return acbrBAL.Ativo;
            else
                return false;
        }

        static private bool lerXMLBalanca()
        {
            try
            {

                //MessageBox.Show(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Balanca.xml");

                XDocument xmlTerminal = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Balanca.xml");
                var config = from c in xmlTerminal.Descendants("Balanca")
                             select new
                             {
                                 Modelo = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Modelo").Value), GlbVariaveis.glbSenhaIQ),
                                 Porta = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Porta").Value), GlbVariaveis.glbSenhaIQ),
                                 Velocidade = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Velocidade").Value), GlbVariaveis.glbSenhaIQ),
                                 DataBits = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("DataBits").Value), GlbVariaveis.glbSenhaIQ),
                                 Parity = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Pariry").Value), GlbVariaveis.glbSenhaIQ),
                                 StopBits = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("StopBits").Value), GlbVariaveis.glbSenhaIQ),
                                 HandShake = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("HandShake").Value), GlbVariaveis.glbSenhaIQ)

                             };

                foreach (var item in config)
                {
                    modelo = item.Modelo;
                    porta = item.Porta;
                    velocidade = item.Velocidade;
                    dataBits = item.DataBits;
                    Parity = item.Parity;
                    StopBits = item.StopBits;
                    HandShake = item.HandShake;
                }

                return true;

            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.ToString());
                return false;
            }

        }
    }
}
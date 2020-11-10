using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace SICEpdv
{
    public class lerXML
    {
        public dadosNFCe lerdadosXML(string stringXml,DateTime dataVenda)
        {
            decimal vDesconto = 0;
            decimal vAcrescimo = 0;

            try
            {
                string xml = "";
                XmlDocument xmlDocument = new XmlDocument();
                xml = stringXml.ToString().Replace("\r\n-", "").Replace(@"\\", "").Replace("\r\n", "").Replace(@"[^\w\.@-]", "").Replace("  <?", "<?").Replace("This XML file does not appear to have any style information associated with it. The document tree is shown below.", "").Replace("&", "");
                xmlDocument.LoadXml(xml.Replace(@"\r\n\r\n-", ""));


                XmlNodeList nfeProc = xmlDocument.GetElementsByTagName("nfeProc");
                XmlNodeList total = ((XmlElement)nfeProc[0]).GetElementsByTagName("total");
                XmlNodeList ide = ((XmlElement)nfeProc[0]).GetElementsByTagName("ide");
                XmlNodeList prod = ((XmlElement)nfeProc[0]).GetElementsByTagName("prod");
                XmlNodeList infNFe = ((XmlElement)nfeProc[0]).GetElementsByTagName("infNFe");


                dadosNFCe n = new dadosNFCe();


                foreach (XmlElement nodoinfProt in ide)
                {
                    n.dataAutorizacao = DateTime.Parse(nodoinfProt.GetElementsByTagName("dhEmi")[0].InnerText.Substring(0, 10));
                }

                decimal valorST = 0;
                decimal valorOut = 0;
                foreach (XmlElement nodoinfProt in prod)
                {
                    if (nodoinfProt.GetElementsByTagName("CFOP")[0].InnerText.Substring(1, 3) == "405")
                    {
                        valorST = valorST + decimal.Parse(nodoinfProt.GetElementsByTagName("vProd")[0].InnerText.Replace(".", ","));
                    }
                    else
                    {
                        valorOut = valorOut + decimal.Parse(nodoinfProt.GetElementsByTagName("vProd")[0].InnerText.Replace(".", ","));
                    }
                }

                n.cfopST = valorST;
                n.cfopOut = valorOut;

                foreach (XmlElement nodoinfProt in infNFe)
                {
                    n.chaveNFe = nodoinfProt.GetAttribute("Id").Replace("NFe","");
                }

                foreach (XmlElement nodoTotal in total)
                {
                    n.total = decimal.Parse(nodoTotal.GetElementsByTagName("vNF")[0].InnerText.Replace(".",","));
                    n.valorIcms = decimal.Parse(nodoTotal.GetElementsByTagName("vICMS")[0].InnerText.Replace(".", ","));
                    n.baseICMS = decimal.Parse(nodoTotal.GetElementsByTagName("vBC")[0].InnerText.Replace(".", ","));
                    vDesconto = decimal.Parse(nodoTotal.GetElementsByTagName("vDesc")[0].InnerText.Replace(".", ","));
                    vAcrescimo = decimal.Parse(nodoTotal.GetElementsByTagName("vOutro")[0].InnerText.Replace(".", ",")); 
                }

                n.modelo = n.chaveNFe.Substring(20, 2);
                n.numeroNF = n.chaveNFe.Substring(25, 9);
                n.serieNF = n.chaveNFe.Substring(22, 3);
                n.dataVenda = dataVenda;
                if ((n.cfopOut + vAcrescimo) > vDesconto)
                {
                    n.cfopOut = ((n.cfopOut + vAcrescimo) - vDesconto);
                }
                else
                {
                    n.cfopST = ((n.cfopST + vAcrescimo) - vDesconto);
                }


                return n;
            }
            catch(Exception erro)
            {
                throw new Exception(erro.ToString());
            }
        }
    }
}

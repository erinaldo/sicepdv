using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Schema;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SICEpdv
{
    public class Formato : Attribute
    {
        public string formato;
        public string cultura;

        public Formato(string formatoInf, string culturaInf)
        {
            this.formato = formatoInf;
            this.cultura = culturaInf;
        }
    }

    public class Obrigatorio : Attribute
    {
        public bool propriedadeObrigatoria;

        public Obrigatorio()
        {
            propriedadeObrigatoria = true;
        }
    }

    public static class CertificadoDigital
    {
        /// <summary>
        /// Função retirada do UNINFE - http://www.unimake.com.br/uninfe/        
        /// 
        /// O método assina digitalmente o arquivo XML passado por parâmetro e 
        /// grava o XML assinado com o mesmo nome, sobreponto o XML informado por parâmetro.
        /// Disponibiliza também uma propriedade com uma string do xml assinado (this.vXmlStringAssinado)
        /// </summary>
        /// <param name="pArqXMLAssinar">Nome do arquivo XML a ser assinado</param>
        /// <param name="pUri">URI (TAG) a ser assinada</param>
        /// <param name="pCertificado">Certificado a ser utilizado na assinatura</param>
        /// <remarks>
        /// Podemos pegar como retorno do método as seguintes propriedades:
        /// 
        /// - Atualiza a propriedade this.vXMLStringAssinado com a string de
        ///   xml já assinada
        ///   
        /// - Grava o XML sobreponto o informado para o método com o conteúdo
        ///   já assinado
        ///   
        /// - Atualiza as propriedades this.vResultado e 
        ///   this.vResultadoString com os seguintes valores:
        ///   
        ///   0 - Assinatura realizada com sucesso
        ///   1 - Erro: Problema ao acessar o certificado digital - %exceção%
        ///   2 - Problemas no certificado digital
        ///   3 - XML mal formado + %exceção%
        ///   4 - A tag de assinatura %pUri% não existe 
        ///   5 - A tag de assinatura %pUri% não é unica
        ///   6 - Erro ao assinar o documento - %exceção%
        ///   7 - Falha ao tentar abrir o arquivo XML - %exceção%
        /// </remarks>
        public static XmlDocument Assinar(XmlDocument docXML, string pUri, X509Certificate2 pCertificado)
        {

            try
            {
                // Load the certificate from the certificate store.
                X509Certificate2 cert = pCertificado;

                // Create a new XML document.
                XmlDocument doc = new XmlDocument();

                // Format the document to ignore white spaces.
                doc.PreserveWhitespace = false;

                // Load the passed XML file using it's name.
                doc = docXML;

                // Create a SignedXml object.
                SignedXml signedXml = new SignedXml(doc);

                // Add the key to the SignedXml document. 
                signedXml.SigningKey = cert.PrivateKey;

                // Create a reference to be signed.
                Reference reference = new Reference();
                reference.Uri = "";
                // pega o uri que deve ser assinada
                
                //XmlAttributeCollection _Uri = doc.GetElementsByTagName(pUri).Item(0).Attributes;
                //foreach (XmlAttribute _atributo in _Uri)
                //{
                //    if (_atributo.Name == "Id")
                //    {
                //        reference.Uri = "#" + _atributo.InnerText;
                //    }
                //}


                // Add an enveloped transformation to the reference.
                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(env);

                XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();
                reference.AddTransform(c14);

                // Add the reference to the SignedXml object.
                signedXml.AddReference(reference);

                // Create a new KeyInfo object.
                KeyInfo keyInfo = new KeyInfo();

                // Load the certificate into a KeyInfoX509Data object
                // and add it to the KeyInfo object.
                keyInfo.AddClause(new KeyInfoX509Data(cert));

                // Add the KeyInfo object to the SignedXml object.
                signedXml.KeyInfo = keyInfo;

                // Compute the signature.
                signedXml.ComputeSignature();

                // Get the XML representation of the signature and save
                // it to an XmlElement object.
                XmlElement xmlDigitalSignature = signedXml.GetXml();

                // Append the element to the XML document.
                doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));


                if (doc.FirstChild is XmlDeclaration)
                {
                    doc.RemoveChild(doc.FirstChild);
                }

                return doc;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao efetuar assinatura digital, detalhes: " + ex.Message);
            }
        }


        public static X509Certificate2 SelecionarCertificado()
        {
            X509Certificate2 CertificadoDigital = new X509Certificate2();

            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            X509Certificate2Collection collection2 = (X509Certificate2Collection)collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);
            X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(collection2, "Certificados disponíveis", "Selecione o certificado digital", X509SelectionFlag.SingleSelection);

            if (scollection.Count == 0)
                throw new Exception("Nenhum certificado digital foi selecionado ou o certificado selecionado está com problemas.");

            return scollection[0];
        }

    }

    public static class ValidaXML
    {
        public static string Erro { get; set; }

        public static string ValidarXML(XmlDocument documento, string versao)
        {
            Stream xmlSaida = new MemoryStream();
            documento.Save(xmlSaida);

            xmlSaida.Flush();
            xmlSaida.Position = 0;
            string retorno = "";


            string xsd_file = "nfe_v2.00.xsd";

            if (versao == "1.10")
                xsd_file = "nfe_v1.10.xsd";
            //nfe_v1.10.xsd
            // 
            if ((documento != null) && (File.Exists(xsd_file))) //Vai encontrar o arquivo apenas se ele estiver na mesma pasta do executável
            {

                Erro = "";
                try
                {
                    XmlSchema xsd = new XmlSchema();
                    xsd.SourceUri = xsd_file;

                    XmlSchemaSet ss = new XmlSchemaSet();
                    ss.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);
                    ss.Add(null, xsd_file);
                    if (ss.Count > 0)
                    {
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.ValidationType = ValidationType.Schema;
                        settings.Schemas.Add(ss);
                        settings.Schemas.Compile();
                        settings.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);
                        XmlTextReader r = new XmlTextReader(xmlSaida);
                        using (XmlReader reader = XmlReader.Create(r, settings))
                        {
                            while (reader.Read())
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Erro = ex.Message;
                }


                if (Erro != "")
                {
                    retorno = "Resultado da validação \r\n\r\n";
                    retorno += Erro;
                    retorno += "\r\n...Fim da validação";
                }
            }
            else
            {
                retorno = "Documento XML inválido ou arquivo do Schema não foi encontrado.";
            }

            return retorno;
        }

        static void reader_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Erro = "Linha: " + e.Exception.LineNumber + " Coluna: " + e.Exception.LinePosition + " Erro: " + e.Exception.Message + "\r\n";
        }

    }

    public class NFe
    {

        public string filial { get; set; }
        public string ipTerminal { get; set; }
        public string criarNF { get; set; }
        public int NFeOrigem { get; set; }
        public string tipoNFe { get; set; } // 0-Saida; 1-Entrada
        public string tipoEmissaoNFe { get; set; }
        public string modeloNFe { get; set; }
        public string finalidadeNFe { get; set; }
        public string situacaoNFe { get; set; }
        public string naturezaOperacaoNFe { get; set; }
        public string gerarICMS { get; set; }
        public int crt { get; set; }
        public Int32 doc { get; set; }

        // Valores NFe
        public decimal subtotalNFe { get; set; }
        public decimal descontoNFe { get; set; }        
        public decimal freteNFe { get; set; }
        public decimal seguroNFe { get; set; }
        public decimal despesasNFe { get; set; }
        public decimal totalSTNFe { get; set; }
        public decimal totalNFe { get; set; }
        public decimal TotalPesoLiquidoNFe { get; set; }
        public decimal TotalPesoBrutoNFe { get; set; }


        // Dados NFe
        public string marcavolume { get; set; }
        public int idCliente { get; set; }
        public int idFornecedorNFe { get; set; }
        public int idTransportadoraNFe { get; set; }
        public int idVeiculoNFe { get; set; }
        public int idInfoComplementarNFe { get; set; }
        public string cfopTransporteNFE { get; set; }
        public int serieNFe { get; set; }
        public string operadorNFe { get; set; }
        public string cfopNFe { get; set; }
        public string dadosComplementarNFe { get; set; }
        public string tipoFreteNFe { get; set; }
        public int volumeNFe { get; set; }
        public int qtdVolumeNFe { get; set; }
        public string especieVolumeNFe { get; set; }
        public string chaveAcessoRefNFe { get; set; }
        public string colocarDataHoraNFe { get; set; }
        public string indPag { get; set; }
        public string NFeEntradaAdEstoque { get; set; }
        public int numeroNFe { get; set; }
        public string destino { get; set; } // D-Dentro do Estado F-Fora do Estado
        public string tipoCliente { get; set; } // F-Pessoa Física - J - Pessoa Jurídica
        public List<itensNFe> itens { get; set; }
        public List<PagamentoNFe> pagamento { get; set; }

        public string nomeConsumidor { get; set; }
        public string cpfConsumidor { get; set; }


        private infNFE _infNFE;

        //private string _Id;

        public string versao { get; set; }

        public string Id { get; set; }
        /// <summary>
        /// Utilizado na geração de XML para consulta de recibo
        /// </summary>
        public string nRec { get; set; }
        /// <summary>
        /// Informar o número do Protocolo de Autorização da NF-e a ser Cancelada.
        /// 1 posição (1 – Secretaria de Fazenda Estadual 
        /// 2 – Receita Federal); 2 posições para código da UF;
        /// 2 posições ano;
        /// 10 seqüencial no ano
        /// </summary>
        public string nProt { get; set; }
        public string xJust { get; set; }
        public string ano { get; set; }
        public int NumNf_Ini { get; set; }
        public int NumNf_Fin { get; set; }

        public infNFE infNFE
        {
            get
            {
                return _infNFE;
            }
        }                     

        public NFe()
        {
            itens = new List<itensNFe>();
            pagamento = new List<PagamentoNFe>();
            _infNFE = new infNFE();
        }

        public bool Restricoes()
        {           
            if (idCliente==0 && idFornecedorNFe==0)
            {
                throw new Exception("Escolha um destinatário para a NFe");
            }

            if (string.IsNullOrEmpty(modeloNFe))
                throw new Exception("Modelo da NFe não pode ser vazio");

            if (totalNFe == 0)
                throw new Exception("NFe sem itens ou com valor zerado");

            if (tipoNFe == "1")
            {
                if (idFornecedorNFe > 0)
                    throw new Exception("Nota Fiscal de saída. Escolha um cliente e não um fornecedor.");
                
                foreach (var item in itens)
                {
                    if ( item.cfop.StartsWith("1") || item.cfop.StartsWith("2") || item.cfop.StartsWith("3") )
                    {
                        throw new Exception("NFe de 1-Saida e o item não está com o CFOP correspondente. PRODUTO: "+ item.codigo+" "+item.descricao);
                    }
                }
            }

            if (tipoNFe == "0")
            {
                foreach (var item in itens)
                {
                    if (item.cfop.StartsWith("5") || item.cfop.StartsWith("6") || item.cfop.StartsWith("7"))
                    {
                        throw new Exception("NFe de 0-Entrada e o item não está com o CFOP correspondente. PRODUTO: " + item.codigo + " " + item.descricao);
                    }
                }
            }

            if (idCliente > 0)
            {
                var dadosClientes = (from n in Conexao.CriarEntidade().clientes
                                     where n.Codigo == idCliente
                                     select new
                                     {
                                         n.inscricao,
                                         n.cnpj,
                                         n.cpf,
                                         n.endereco,
                                         n.numero,
                                         n.cep,
                                         n.cidade,
                                         n.estado,
                                         n.bairro,
                                         n.telefone
                                     }).FirstOrDefault();
                if ( string.IsNullOrEmpty(dadosClientes.cnpj) && string.IsNullOrEmpty(dadosClientes.cpf) )
                    throw new Exception("CNPJ ou CPF do destinatário tem que existir.");

                if (!string.IsNullOrEmpty(dadosClientes.cnpj) && !Funcoes.ValidaCnpj(dadosClientes.cnpj))
                    throw new Exception("CNPJ inválido.");

                if (string.IsNullOrEmpty(dadosClientes.cidade) || string.IsNullOrEmpty(dadosClientes.numero) || string.IsNullOrEmpty(dadosClientes.estado) || string.IsNullOrEmpty(dadosClientes.cep))
                {
                    throw new Exception("CEP, cidade, endereço, número, bairro não podem ser vazios");
                }

                if (string.IsNullOrEmpty(dadosClientes.telefone))
                {
                    throw new Exception("Telefone não pode ser vazio");
                }

                var estadoEmpresa = (from n in Conexao.CriarEntidade().filiais
                                   where n.CodigoFilial == GlbVariaveis.glb_filial
                                   select new { n.estado }).FirstOrDefault();

                if (estadoEmpresa.estado != dadosClientes.estado)
                {

                    foreach (var item in itens)
                    {
                        if (tipoNFe != "0")
                        {
                            if (!item.cfop.StartsWith("6"))
                                throw new Exception("Venda para fora do estado. USE CFOP  que começe com 6 " + item.codigo + " " + item.descricao);
                        }
                        else
                        {
                            if (!item.cfop.StartsWith("2") && !item.cfop.StartsWith("3"))
                                throw new Exception("Entrada fora do estado. USE CFOP  que começe com 2 " + item.codigo + " " + item.descricao);
                        }
                    }

                }

                if (estadoEmpresa.estado == dadosClientes.estado)
                {
                    foreach (var item in itens)
                    {
                        if (tipoNFe != "0")
                        {
                            if (!item.cfop.StartsWith("5"))
                                throw new Exception("Venda para dentro do estado. USE CFOP  que começe com 5 " + item.codigo + " " + item.descricao);
                        }
                        else
                        {
                            if (!item.cfop.StartsWith("1"))
                                throw new Exception("Notas de Entrada  dentro do estado. USE CFOP  que começe com 1 " + item.codigo + " " + item.descricao);
                        }
                    }

                }

                if (totalNFe != (from n in pagamento select n.valor).Sum() && naturezaOperacaoNFe=="Venda")
                {
                    throw new Exception("Operação: Venda e pagamentos diferente do total da NFe");
                }

            }

            return true;
        }

        public int GerarNFe()
        {

            try
            {
                if(modeloNFe == "55")
                    Restricoes();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);                
            }
            try
            {
                #region comentado por marckvaldo 18/05/2016 loja araujo neto Camalaú
                /* tem problema quando é interligado fica retornando a mesagem Connection must be valid and open
                EntityConnection conn = new EntityConnection(Conexao.stringConexao);
                conn.Close();
                using (conn)
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 3800;
                    cmd.CommandText = "siceEntities.GerarNFe";
                    cmd.CommandType = CommandType.StoredProcedure;

                    #region parametros nota fiscal
                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;

                    EntityParameter criarNF = cmd.Parameters.Add("criarNF", DbType.String);
                    criarNF.Direction = ParameterDirection.Input;
                    criarNF.Value = this.criarNF;

                    EntityParameter NFeOrigem = cmd.Parameters.Add("NFeOrigem", DbType.String);
                    NFeOrigem.Direction = ParameterDirection.Input;
                    NFeOrigem.Value = this.NFeOrigem;


                    EntityParameter tipoNFe = cmd.Parameters.Add("tipoNFe", DbType.String);
                    tipoNFe.Direction = ParameterDirection.Input;
                    tipoNFe.Value = this.tipoNFe;

                    EntityParameter tipoEmissaoNFe = cmd.Parameters.Add("tipoEmissaoNFe", DbType.String);
                    tipoEmissaoNFe.Direction = ParameterDirection.Input;
                    tipoEmissaoNFe.Value = this.tipoEmissaoNFe;

                    EntityParameter modeloNFe = cmd.Parameters.Add("modeloNFe", DbType.String);
                    modeloNFe.Direction = ParameterDirection.Input;
                    modeloNFe.Value = this.modeloNFe;

                    EntityParameter finalidadeNFe = cmd.Parameters.Add("finalidadeNFe", DbType.String);
                    finalidadeNFe.Direction = ParameterDirection.Input;
                    finalidadeNFe.Value = this.finalidadeNFe;

                    EntityParameter situacaoNFe = cmd.Parameters.Add("situacaoNFe", DbType.String);
                    situacaoNFe.Direction = ParameterDirection.Input;
                    situacaoNFe.Value = this.situacaoNFe;

                    EntityParameter naturezaOperacaoNFe = cmd.Parameters.Add("naturezaOperacaoNFe", DbType.String);
                    naturezaOperacaoNFe.Direction = ParameterDirection.Input;
                    naturezaOperacaoNFe.Value = this.naturezaOperacaoNFe;

                    EntityParameter gerarICMS = cmd.Parameters.Add("gerarICMS", DbType.String);
                    gerarICMS.Direction = ParameterDirection.Input;
                    gerarICMS.Value = this.gerarICMS;

                    EntityParameter doc = cmd.Parameters.Add("doc", DbType.Int32);
                    doc.Direction = ParameterDirection.Input;
                    doc.Value = this.doc;

                    EntityParameter descontoNFe = cmd.Parameters.Add("descontoNFe", DbType.Decimal);
                    descontoNFe.Direction = ParameterDirection.Input;
                    descontoNFe.Value = this.descontoNFe;

                    EntityParameter freteNFe = cmd.Parameters.Add("freteNFe", DbType.Decimal);
                    freteNFe.Direction = ParameterDirection.Input;
                    freteNFe.Value = this.freteNFe;

                    EntityParameter seguroNFe = cmd.Parameters.Add("seguroNFe", DbType.Decimal);
                    seguroNFe.Direction = ParameterDirection.Input;
                    seguroNFe.Value = this.seguroNFe;

                    EntityParameter despesasNFe = cmd.Parameters.Add("despesasNFe", DbType.Decimal);
                    despesasNFe.Direction = ParameterDirection.Input;
                    despesasNFe.Value = this.despesasNFe;

                    EntityParameter marcavolume = cmd.Parameters.Add("marcavolume", DbType.String);
                    marcavolume.Direction = ParameterDirection.Input;
                    marcavolume.Value = this.marcavolume;

                    EntityParameter idCliente = cmd.Parameters.Add("idCliente", DbType.Int32);
                    idCliente.Direction = ParameterDirection.Input;
                    idCliente.Value = this.idCliente;

                    EntityParameter idFornecedorNFe = cmd.Parameters.Add("idFornecedorNFe", DbType.Int32);
                    idFornecedorNFe.Direction = ParameterDirection.Input;
                    idFornecedorNFe.Value = this.idFornecedorNFe;

                    EntityParameter idTransportadoraNFe = cmd.Parameters.Add("idTransportadoraNFe", DbType.Int32);
                    idTransportadoraNFe.Direction = ParameterDirection.Input;
                    idTransportadoraNFe.Value = this.idTransportadoraNFe;

                    EntityParameter idVeiculoNFe = cmd.Parameters.Add("idVeiculoNFe", DbType.Int32);
                    idVeiculoNFe.Direction = ParameterDirection.Input;
                    idVeiculoNFe.Value = this.idVeiculoNFe;

                    EntityParameter idInfoComplementarNFe = cmd.Parameters.Add("idInfoComplementarNFe", DbType.Int32);
                    idInfoComplementarNFe.Direction = ParameterDirection.Input;
                    idInfoComplementarNFe.Value = this.idInfoComplementarNFe;

                    EntityParameter cfopTransporteNFE = cmd.Parameters.Add("cfopTransporteNFE", DbType.String);
                    cfopTransporteNFE.Direction = ParameterDirection.Input;
                    cfopTransporteNFE.Value = this.cfopTransporteNFE;

                    EntityParameter serieNFe = cmd.Parameters.Add("serieNFe", DbType.String);
                    serieNFe.Direction = ParameterDirection.Input;
                    serieNFe.Value = Convert.ToString(this.serieNFe);

                    EntityParameter operadorNFe = cmd.Parameters.Add("operadorNFe", DbType.String);
                    operadorNFe.Direction = ParameterDirection.Input;
                    operadorNFe.Value = this.operadorNFe;

                    EntityParameter cfopNFe = cmd.Parameters.Add("cfopNFe", DbType.String);
                    cfopNFe.Direction = ParameterDirection.Input;
                    cfopNFe.Value = this.cfopNFe;

                    EntityParameter dadosComplementarNFe = cmd.Parameters.Add("dadosComplementarNFe", DbType.String);
                    dadosComplementarNFe.Direction = ParameterDirection.Input;
                    dadosComplementarNFe.Value = this.dadosComplementarNFe;

                    EntityParameter tipoFreteNFe = cmd.Parameters.Add("tipoFreteNFe", DbType.String);
                    tipoFreteNFe.Direction = ParameterDirection.Input;
                    tipoFreteNFe.Value = this.tipoFreteNFe;

                    EntityParameter volumeNFe = cmd.Parameters.Add("volumeNFe", DbType.Int16);
                    volumeNFe.Direction = ParameterDirection.Input;
                    volumeNFe.Value = this.volumeNFe;

                    EntityParameter qtdVolumeNFe = cmd.Parameters.Add("qtdVolumeNFe", DbType.Int32);
                    qtdVolumeNFe.Direction = ParameterDirection.Input;
                    qtdVolumeNFe.Value = this.qtdVolumeNFe;

                    EntityParameter especieVolumeNFe = cmd.Parameters.Add("especieVolumeNFe", DbType.String);
                    especieVolumeNFe.Direction = ParameterDirection.Input;
                    especieVolumeNFe.Value = this.especieVolumeNFe;

                    EntityParameter chaveAcessoRefNFe = cmd.Parameters.Add("chaveAcessoRefNFe", DbType.String);
                    chaveAcessoRefNFe.Direction = ParameterDirection.Input;
                    chaveAcessoRefNFe.Value = this.chaveAcessoRefNFe;

                    EntityParameter colocarDataHoraNFe = cmd.Parameters.Add("colocarDataHoraNFe", DbType.String);
                    colocarDataHoraNFe.Direction = ParameterDirection.Input;
                    colocarDataHoraNFe.Value = this.colocarDataHoraNFe;

                    EntityParameter indPag = cmd.Parameters.Add("indPag", DbType.String);
                    indPag.Direction = ParameterDirection.Input;
                    indPag.Value = this.indPag;

                    EntityParameter NFeEntradaAdEstoque = cmd.Parameters.Add("NFeEntradaAdEstoque", DbType.String);
                    NFeEntradaAdEstoque.Direction = ParameterDirection.Input;
                    NFeEntradaAdEstoque.Value = this.NFeEntradaAdEstoque;

                    EntityParameter crt = cmd.Parameters.Add("crtNFE", DbType.Int16);
                    crt.Direction = ParameterDirection.Input;
                    crt.Value = this.crt;

                    EntityParameter TotalPesoBrutoNFe = cmd.Parameters.Add("TotalPesoBrutoNFe", DbType.Decimal);
                    TotalPesoBrutoNFe.Direction = ParameterDirection.Input;
                    TotalPesoBrutoNFe.Value = this.TotalPesoBrutoNFe;

                    EntityParameter TotalPesoLiquidoNFe = cmd.Parameters.Add("TotalPesoLiquidoNFe", DbType.Decimal);
                    TotalPesoLiquidoNFe.Direction = ParameterDirection.Input;
                    TotalPesoLiquidoNFe.Value = this.TotalPesoLiquidoNFe;


                    EntityParameter DataSaida = cmd.Parameters.Add("DataSaida", DbType.Date);
                    DataSaida.Direction = ParameterDirection.Input;
                    DataSaida.Value = DateTime.Now.Date;

                    EntityParameter HoraSaida = cmd.Parameters.Add("HoraSaida", DbType.String);
                    HoraSaida.Direction = ParameterDirection.Input;
                    HoraSaida.Value = DateTime.Now.Date.TimeOfDay.ToString();

                    EntityParameter arredondar = cmd.Parameters.Add("arredondar", DbType.String);
                    arredondar.Direction = ParameterDirection.Input;
                    arredondar.Value ="S";


                    EntityParameter numeroNFe = cmd.Parameters.Add("numeroNFe", DbType.Int32);
                    numeroNFe.Direction = ParameterDirection.Output;
                    numeroNFe.Value = this.numeroNFe;
                    #endregion

                    MessageBox.Show("antes de executar");
                    //cmd.ExecuteNonQuery();
                    MessageBox.Show("executado");
                    conn.Close();
                    this.numeroNFe = Convert.ToInt32(cmd.Parameters["numeroNFe"].Value.ToString());
                    //return this.numeroNFe;
                }*/
                #endregion


                var usarTEF = Conexao.CriarEntidade().ExecuteStoreQuery<int>("SELECT COUNT(1) AS quantidade FROM caixa AS c, cartoes AS t WHERE c.tipopagamento = 'CA' AND c.Cartao = t.descricao AND t.TEF = 'S' AND c.documento = '" + this.doc + "'").FirstOrDefault();

                string tef = "1";
                if ((ConfiguracoesECF.tefDiscado == true || ConfiguracoesECF.tefDedicado == true) && (usarTEF > 0))
                    tef = "1";
                else
                    tef = "2";


                string SQL = "CALL GerarNFe('" 
                    + GlbVariaveis.glb_filial + "', '" 
                    + GlbVariaveis.glb_IP + "', '" 
                    + this.criarNF + "', '" 
                    + this.NFeOrigem + "', '" 
                    + this.tipoNFe + "', '" 
                    + this.tipoEmissaoNFe + "', '" 
                    + this.modeloNFe + "', '" 
                    + this.finalidadeNFe + "', '" 
                    + this.situacaoNFe + "', '" 
                    + this.naturezaOperacaoNFe + "', '" 
                    + this.gerarICMS + "', " 
                    + this.doc + ", " 
                    + this.descontoNFe.ToString().Replace(".","").Replace(",",".") + ", "
                    + this.freteNFe.ToString().Replace(".", "").Replace(",", ".") + ", "
                    + this.seguroNFe.ToString().Replace(".", "").Replace(",", ".") + ", "
                    + this.despesasNFe.ToString().Replace(".", "").Replace(",", ".") + ", '" 
                    + this.marcavolume + "', " 
                    + this.idCliente + ", " 
                    + this.idFornecedorNFe + ", " 
                    + this.idTransportadoraNFe + ", " 
                    + this.idVeiculoNFe + ", " 
                    + this.idInfoComplementarNFe + ", '" 
                    + this.cfopTransporteNFE + "', '" 
                    + this.serieNFe + "', '" 
                    + this.operadorNFe + "', '" 
                    + this.cfopNFe + "', '" 
                    + this.dadosComplementarNFe + "', '" 
                    + this.tipoFreteNFe + "', " 
                    + this.volumeNFe + ", " 
                    + this.qtdVolumeNFe + ", '" 
                    + this.especieVolumeNFe + "', '" 
                    + this.chaveAcessoRefNFe + "', '" 
                    + this.colocarDataHoraNFe + "', '" 
                    + this.indPag + "', '" 
                    + this.NFeEntradaAdEstoque + "', " 
                    + this.crt + ", "
                    + this.TotalPesoBrutoNFe.ToString().Replace(".", "").Replace(",", ".") + ", "
                    + this.TotalPesoLiquidoNFe.ToString().Replace(".", "").Replace(",", ".") + ", '"
                    + DateTime.Now.Date.ToString("yyyy-MM-dd") + "', '" 
                    + DateTime.Now.Date.TimeOfDay.ToString() + "',"
                    +"'N',"
                    +"'"+tef+"',"
                    + "'N',"
                    + "@_cnet_param_numeroNFe);";

                LogSICEpdv.Registrarlog(SQL, "", "FuncoesNFC");
                    
                //Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                //this.numeroNFe = Conexao.CriarEntidade().ExecuteStoreQuery<int>("SELECT @_cnet_param_numeroNFe").FirstOrDefault();
                this.numeroNFe = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();
                return this.numeroNFe;


            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar SP GerarNFe. " + ex.ToString());
            }

        }

        public bool apagarNotaFiscal(int notaFiscal, string serie, string codigoFilial)
        {
            try
            {



                string SQL = "call ApagaNFe(" + notaFiscal + ",'" + int.Parse(serie).ToString() + "','" + codigoFilial + "')";
                this.numeroNFe = Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                return true;
            }
            catch (Exception erro)
            {
                throw new Exception("Erro ao executar SP Apagar NFCe. " + erro.ToString());
                return false;
            }
        }

        public bool VerificaCFOP(string cfop)
        {

            try
            {
                var cfopProdutos = (from CfopItens in Conexao.CriarEntidade().nfoperacao
                                    where CfopItens.codigo == cfop
                                    select new
                                    {
                                        cfop = CfopItens.codigo ?? "",
                                        descricao = CfopItens.codigo + " - " + CfopItens.descricao ?? ""
                                    }).FirstOrDefault();
                if (cfopProdutos == null || string.IsNullOrEmpty(cfopProdutos.cfop))
                    throw new Exception("CFOP não foi encontrado");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<string> CSTPisCofins(string tipo)
        {
            if (tipo == "S")
            {
                List<string> CSTSaida = new List<string>(new string[]
                {
            
             "01|Operação Tributável com Alíquota Básica",
             "02|Operação Tributável com Alíquota Diferenciada",
             "03|Operação Tributável com Alíquota por Unidade de Medida de Produto",
             "04|Operação Tributável Monofásica - Revenda a Alíquota Zero",
             "05|Operação Tributável por Substituição Tributária",
             "06|Operação Tributável a Alíquota Zero",
             "07|Operação Isenta da Contribuição",
             "08|Operação sem Incidência da Contribuição",
             "09|Operação com Suspensão da Contribuição",
             "49|Outras Operações de Saída",
             "99|Outras Operações"
                });
                return CSTSaida.ToList();
            }
            //Entradas
            if (tipo == "E")
            {
                List<string> CSTSEntrada = new List<string>(new string[]                
            {

                "50|Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Tributada no Mercado Interno",
                "51|Operação com Direito a Crédito – Vinculada Exclusivamente a Receita Não Tributada no Mercado Interno",
                "52|Operação com Direito a Crédito - Vinculada Exclusivamente a Receita de Exportação",
                "53|Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno",
                "54|Operação com Direito a Crédito - Vinculada a Receitas Tributadas no Mercado Interno e de Exportação",
                "55|Operação com Direito a Crédito - Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação",
                "56|Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação",
                "60|Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Tributada no Mercado Interno",
                "61|Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Não-Tributada no Mercado Interno",
                "62|Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita de Exportação",
                "63|Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno",
                "64|Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas no Mercado Interno e de Exportação",
                "65|Crédito Presumido - Operação de Aquisição Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação",
                "66|Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação",
                "67|Crédito Presumido - Outras Operações",
                "70|Operação de Aquisição sem Direito a Crédito",
                "71|Operação de Aquisição com Isenção",
                "72|Operação de Aquisição com Suspensão",
                "73|Operação de Aquisição a Alíquota Zero",
                "74|Operação de Aquisição sem Incidência da Contribuição",
                "75|Operação de Aquisição por Substituição Tributária",
                "98|Outras Operações de Entrada",
                "99|Outras Operações"                
            });
                return CSTSEntrada.ToList();
            };
            return null;                
        }

        public List<nfoperacao> PopulaCfops(string tipo)
        {
           
            if (tipo == "1")//saida
            {
                var dados = (from n in Conexao.CriarEntidade().nfoperacao
                             where (n.codigo.StartsWith("5") || n.codigo.StartsWith("6") || n.codigo.StartsWith("7"))
                             select n);
                return dados.ToList();
            }

            if (tipo == "0") // Entrada
            {
                    var dados = (from n in Conexao.CriarEntidade().nfoperacao
                                 where (n.codigo.StartsWith("1") || n.codigo.StartsWith("2") || n.codigo.StartsWith("3"))
                                 select n).ToList();
                    return dados.ToList();

                   
            }
            return null;

        }

        public bool MudarCFOPItens(string acao,string cfop)
        {
            switch (acao)
            {
                case "itens":
                    foreach (var item in itens)
                    {
                        item.cfop = cfop.Substring(0, 5);
                    }
                    break;
                case "cadastro":
                    foreach (var item in itens)
                    {
                        string cfopCadastro = item.cfop;

                        if (GlbVariaveis.glb_filial == "00001")
                        {
                            var CfopProdutos = (from CP in Conexao.CriarEntidade().produtos
                                                where CP.codigo == item.codigo
                                                select new
                                                {
                                                    CFOPSaida = CP.cfopsaida ?? "",
                                                    CFOPEntrada = CP.cfopentrada ?? "",
                                                    classeFiscal = CP.codigofiscal
                                                }).FirstOrDefault();
                            if (tipoNFe == "0")
                                cfopCadastro = CfopProdutos.CFOPEntrada;
                            if (tipoNFe == "1")
                                cfopCadastro = CfopProdutos.CFOPSaida;
                            
                            if (CfopProdutos.classeFiscal!="000" && tipoNFe=="0")
                            {
                                var dadosClasse = (from n in Conexao.CriarEntidade().classefiscal
                                                   where n.codigo == CfopProdutos.classeFiscal
                                                   && n.UsarICMSDiferenciado=="S"
                                                   select n).First();
                                if (dadosClasse != null)
                                {
                                    #region Dentro do Estado
                                    // Atribuindo as configurações dentro do Estado
                                    if (destino == "D")
                                    {
                                        item.cfop = dadosClasse.cfopestado;
                                        //Pessoa Jurídica
                                        if (tipoCliente == "J")
                                        {
                                            item.csticms = dadosClasse.tributacaoEstado;
                                            item.icms = dadosClasse.ICMSEstadoPJ;
                                            item.redbc = dadosClasse.reducao;
                                            

                                            // PARA O ICMS ST
                                            if (item.csticms == "10" || item.csticms == "70")
                                            {
                                                item.icms = dadosClasse.ICMSOutroEstadoPJ;
                                                item.icmsst = dadosClasse.ICMSSTEstadoPJ;
                                                item.redbcst = dadosClasse.reducaoSTestado;
                                            }
                                        }

                                        //Pessoa Física
                                        if (tipoCliente == "F")
                                        {
                                            item.csticms = dadosClasse.tributacaoEstado;
                                            item.icms = dadosClasse.ICMSEstadoPF;
                                            item.redbc = dadosClasse.reducao;

                                            // PARA O ICMS ST
                                            if (item.csticms == "10" || item.csticms == "70")
                                            {
                                                item.icms = dadosClasse.ICMSEstadoPF;
                                                item.icmsst = dadosClasse.ICMSSTEstadoPF;
                                                item.redbcst = dadosClasse.reducaoSTestado;
                                            }
                                        }

                                    } // Dentro do Estado
                                    #endregion

                                    #region Fora do Estado
                                    // Atribuindo as configurações fora do Estado
                                    if (destino == "F")
                                    {
                                        item.cfop = dadosClasse.cfopoutroestado;
                                        //Pessoa Jurídica
                                        if (tipoCliente == "J")
                                        {
                                            item.csticms = dadosClasse.tributacaoForaEstado;
                                            item.icms = dadosClasse.ICMSOutroEstadoPJ;
                                            item.redbc = dadosClasse.reducaoforaEstado;

                                            // PARA O ICMS ST
                                            if (item.csticms == "10" || item.csticms == "70")
                                            {
                                                item.icms = dadosClasse.ICMSOutroEstadoPJ;
                                                item.icmsst = dadosClasse.ICMSSTOutroEstadoPJ;
                                                item.redbcst = dadosClasse.reducaoSTForaEstado;
                                            }
                                        }

                                        //Pessoa Física
                                        if (tipoCliente == "F")
                                        {
                                            item.csticms = dadosClasse.tributacaoForaEstado;
                                            item.icms = dadosClasse.ICMSOutroEstadoPF;
                                            item.redbc = dadosClasse.reducaoforaEstado;

                                            // PARA O ICMS ST
                                            if (item.csticms == "10" || item.csticms == "70")
                                            {
                                                item.icms = dadosClasse.ICMSOutroEstadoPF;
                                                item.icmsst = dadosClasse.ICMSSTOutroEstadoPF;
                                                item.redbcst = dadosClasse.reducaoSTForaEstado;
                                            }
                                        }

                                    } // Dentro do Estado
                                    #endregion


                                }

                            }


                        }

                        if (GlbVariaveis.glb_filial != "00001")
                        {
                            var CfopProdutos = (from CP in Conexao.CriarEntidade().produtosfilial
                                                where CP.codigo == item.codigo
                                                select new
                                                {
                                                    CFOPSaida = CP.cfopsaida ?? "",
                                                    CFOPEntrada = CP.cfopentrada ?? "",
                                                    classeFiscal = CP.codigofiscal
                                                }).FirstOrDefault();
                            if (tipoNFe == "0")
                                cfopCadastro = CfopProdutos.CFOPEntrada;
                            if (tipoNFe == "1")
                                cfopCadastro = CfopProdutos.CFOPSaida;

                            if (CfopProdutos.classeFiscal != "000" && tipoNFe == "0")
                            {
                                var dadosClasse = (from n in Conexao.CriarEntidade().classefiscal
                                                   where n.codigo == CfopProdutos.classeFiscal
                                                   && n.UsarICMSDiferenciado == "S"
                                                   select n).First();
                                if (dadosClasse != null)
                                {
                                    #region Dentro do Estado
                                    // Atribuindo as configurações dentro do Estado
                                    if (destino == "D")
                                    {
                                        item.cfop = dadosClasse.cfopestado;
                                        //Pessoa Jurídica
                                        if (tipoCliente == "J")
                                        {
                                            item.csticms = dadosClasse.tributacaoEstado;
                                            item.icms = dadosClasse.ICMSEstadoPJ;
                                            item.redbc = dadosClasse.reducao;


                                            // PARA O ICMS ST
                                            if (item.csticms == "10" || item.csticms == "70")
                                            {
                                                item.icms = dadosClasse.ICMSOutroEstadoPJ;
                                                item.icmsst = dadosClasse.ICMSSTEstadoPJ;
                                                item.redbcst = dadosClasse.reducaoSTestado;
                                            }
                                        }

                                        //Pessoa Física
                                        if (tipoCliente == "F")
                                        {
                                            item.csticms = dadosClasse.tributacaoEstado;
                                            item.icms = dadosClasse.ICMSEstadoPF;
                                            item.redbc = dadosClasse.reducao;

                                            // PARA O ICMS ST
                                            if (item.csticms == "10" || item.csticms == "70")
                                            {
                                                item.icms = dadosClasse.ICMSEstadoPF;
                                                item.icmsst = dadosClasse.ICMSSTEstadoPF;
                                                item.redbcst = dadosClasse.reducaoSTestado;
                                            }
                                        }

                                    } // Dentro do Estado
                                    #endregion

                                    #region Fora do Estado
                                    // Atribuindo as configurações fora do Estado
                                    if (destino == "F")
                                    {
                                        item.cfop = dadosClasse.cfopoutroestado;
                                        //Pessoa Jurídica
                                        if (tipoCliente == "J")
                                        {
                                            item.csticms = dadosClasse.tributacaoForaEstado;
                                            item.icms = dadosClasse.ICMSOutroEstadoPJ;
                                            item.redbc = dadosClasse.reducaoforaEstado;

                                            // PARA O ICMS ST
                                            if (item.csticms == "10" || item.csticms == "70")
                                            {
                                                item.icms = dadosClasse.ICMSOutroEstadoPJ;
                                                item.icmsst = dadosClasse.ICMSSTOutroEstadoPJ;
                                                item.redbcst = dadosClasse.reducaoSTForaEstado;
                                            }
                                        }

                                        //Pessoa Física
                                        if (tipoCliente == "F")
                                        {
                                            item.csticms = dadosClasse.tributacaoForaEstado;
                                            item.icms = dadosClasse.ICMSOutroEstadoPF;
                                            item.redbc = dadosClasse.reducaoforaEstado;

                                            // PARA O ICMS ST
                                            if (item.csticms == "10" || item.csticms == "70")
                                            {
                                                item.icms = dadosClasse.ICMSOutroEstadoPF;
                                                item.icmsst = dadosClasse.ICMSSTOutroEstadoPF;
                                                item.redbcst = dadosClasse.reducaoSTForaEstado;
                                            }
                                        }

                                    } // Dentro do Estado
                                    #endregion

                                }
                            }
                        }
                        
                    }
                break;
                case "cliente":
                                   var dados = (from CFC in Conexao.CriarEntidade().clientes
                                   where CFC.Codigo == idCliente
                                   select new
                                   {
                                       Cfop = CFC.cfopnfe,
                                       icms = CFC.icms,
                                       icmsst = CFC.icmsst,
                                       tributacao = CFC.csticms
                                   }).FirstOrDefault();

                                    if (dados.Cfop.Trim() != "" || dados.icms != 0)
                                    {
                                        foreach (var item in itens)
                                        {
                                            item.icms = dados.icms;
                                            item.cfop = dados.Cfop;
                                            item.csticms = dados.tributacao;
                                            item.icmsst = dados.icmsst;
                                            
                                        }

                                    }
                break;
                case "COF":

                var dadosCOF = (from n in Conexao.CriarEntidade().cof
                            where n.id == FrmCOF.idCOF
                            select n).FirstOrDefault();
                    

                foreach (var item in itens)
                {
                    item.cfop = dadosCOF.cfop;
                    item.csticms = dadosCOF.csticms;
                    item.icms = dadosCOF.picms.Value;
                    item.icmsst = dadosCOF.picmsst.Value;
                    item.mva = dadosCOF.mvaicmsst.Value;
                    item.cstpis = dadosCOF.cstpis;
                    item.cstcofins = dadosCOF.cstcofins;
                    item.pis = dadosCOF.ppis.Value;
                    item.cofins = dadosCOF.pcofins.Value;
                    item.cstipi = dadosCOF.cstipi;
                    item.ipi = dadosCOF.pipi.Value;                   
                }
                break;
            }

            return true;
        }

        public bool SalvarItensTabela()
        {
            if (Conexao.onLine)
            {
                siceEntities entidade = Conexao.CriarEntidade();

                int nrcontrole = 1;
                foreach (var item in itens)
                {
                    var produtoItem = (from pd in Conexao.CriarEntidade().produtos
                                    where pd.codigo == item.codigo
                                    select new
                                    {
                                        CodigoFiscal = pd.codigofiscal,
                                        Comissao = pd.tipocomissao,
                                        Grade = pd.grade,
                                        Lote = pd.lote,
                                        Tipo = pd.tipo,
                                        Embalabem = pd.embalagem,
                                        Descricao = pd.descricao,
                                        Complemento = pd.complementodescricao,
                                        PrecoOriginal = pd.precovenda,
                                        Custo = pd.custo,
                                        Unidade = pd.unidade,
                                        CstCofins = pd.tributacaoCOFINS,
                                        CstPIs = pd.tributacaoPIS,
                                        NCM = pd.ncm,
                                        NBM = pd.nbm,
                                        EspecieMcm = pd.ncmespecie,
                                        Situacao = pd.situacao,
                                        Grupo = pd.grupo,
                                        SubGrupo = pd.subgrupo,
                                        CustoMedio = pd.customedio,
                                        Fabricante = pd.fabricante,
                                        CodigoBarras = pd.codigobarras,  
                                        classe = pd.classe,
                                        cenqipi = pd.cenqipi
                                    }).FirstOrDefault();
                    try
                    {

                
                        #region
                        vendas novoItem = new vendas();
                        novoItem.aentregar = "N";
                        novoItem.codigofilial = GlbVariaveis.glb_filial;
                        novoItem.classe = produtoItem.classe;
                        novoItem.codigobarras = "12";
                        novoItem.codigofiscal = "000";
                        novoItem.comissao = "A";
                        novoItem.grade = "nenhuma";
                        novoItem.id = GlbVariaveis.glb_IP;
                        novoItem.lote = produtoItem.Lote;
                        novoItem.romaneio = "S";
                        novoItem.tipo = produtoItem.Tipo;
                        novoItem.embalagem = produtoItem.Embalabem;
                        novoItem.nrcontrole = nrcontrole;
                        novoItem.codigo = item.codigo;
                        novoItem.produto = (produtoItem.Descricao.Trim() + " " + produtoItem.Complemento.Trim()).Trim().PadRight(49, ' ').Substring(0, 49);
                        novoItem.quantidade = item.quantidade;
                        novoItem.preco = item.preco;
                        novoItem.custo = produtoItem.Custo;
                        novoItem.precooriginal = produtoItem.PrecoOriginal;
                        novoItem.acrescimototalitem = 0;
                        novoItem.unidade = produtoItem.Unidade;
                        novoItem.Descontoperc = item.desconto;
                        novoItem.descontovalor = 0;
                        novoItem.vendedor = "000";
                        novoItem.icms =Convert.ToInt16(item.icms);
                        novoItem.tributacao =item.csticms ;
                        novoItem.total = item.total; // Math.Round( quantidade * precooriginal);
                        novoItem.cfop = item.cfop;
                        novoItem.cstcofins = "01";
                        novoItem.cstpis = "01";
                        novoItem.serieNF = "1";
                        novoItem.subserienf = "1";
                        novoItem.modelodocfiscal = "2D";
                        novoItem.cancelado = "N";
                        novoItem.data = GlbVariaveis.Sys_Data.Date;
                        novoItem.operador = GlbVariaveis.glb_Usuario;
                        novoItem.grade = produtoItem.Grade;
                        novoItem.ratfrete = 0;
                        novoItem.ratseguro = 0;
                        novoItem.ratdespesas = 0;
                        novoItem.cstipi = "99";
                        novoItem.qUnidIPI = 0;
                        novoItem.vUnidIPI = 0;
                        novoItem.ncm = "";
                        novoItem.nbm = "";
                        novoItem.ncmespecie = "";
                        novoItem.origem = "0";
                        novoItem.ncm = produtoItem.NCM;
                        novoItem.ncmespecie =  produtoItem.NCM.Length > 2 ?  produtoItem.NCM.Substring(0, 2) : "";
                        novoItem.pis = item.pis;
                        novoItem.cofins = item.cofins;
                        novoItem.aliquotaIPI = 0;
                        novoItem.cenqipi = produtoItem.cenqipi;
                        novoItem.itemDAV = "S";
                        novoItem.canceladoECF = "N";
                        novoItem.vendaatacado = Produtos.tabelaPreco == "atacado" ? "S" : "N";
                        entidade.AddTovendas(novoItem);
                        entidade.SaveChanges();
                        nrcontrole++;
                        #endregion
                    }                        
                    catch (Exception erro)
                    {
                        throw new Exception("Erro ao gravar na tabela vendas. " + erro.Message);
                    }

                }

                try
                {
                    
                    foreach (var itemPag in pagamento)
                    {
                        caixas novo = new caixas();
                        novo.EnderecoIP = GlbVariaveis.glb_IP;
                        novo.valor = itemPag.valor;
                        novo.caixa = 0;
                        novo.historico = "NFe";
                        novo.data = GlbVariaveis.Sys_Data;
                        novo.tipopagamento = itemPag.tipo;
                        novo.operador = GlbVariaveis.glb_Usuario;
                        novo.dpfinanceiro = "Venda";
                        novo.filialorigem = GlbVariaveis.glb_filial;
                        novo.CodigoFilial = GlbVariaveis.glb_filial;
                        if (itemPag.tipo == "CR")
                            novo.vencimento = itemPag.vencimento;
                        novo.vendedor = "000";
                        entidade.AddTocaixas(novo);
                        entidade.SaveChanges();                        
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception("Erro ao gravar na tabela caixas. " + ex.Message);
                }


                return true;
            }
            return true;
        }

        public bool Parcelamento(int parcelas, decimal valor, DateTime vencimentoInicial, int intervalo = 30)
        {
            if (valor == 0)
                return true;
            // Para não adicionar um vencimento que fuja das regras de negócios.
            if (intervalo > 30)
                intervalo = 30;


            decimal valorParcela = Math.Round((valor / parcelas), 2);
            decimal restoDivisao = valor - Math.Round((valorParcela * parcelas), 2);
            List<PagamentoNFe> parcelamento = new List<PagamentoNFe>();
            DateTime vencimento = vencimentoInicial;
            int? seqPag = (from n in pagamento select (int?)n.parcela).Max() + 1;
            if (!seqPag.HasValue)
                seqPag = 1;

            for (int i = 0; i < parcelas; i++)
            {

                if (i == 0)
                    valorParcela += restoDivisao;

                if (i == 0)
                    vencimento = vencimentoInicial.Date;
                else
                {
                    if (intervalo == 30 && !Configuracoes.diasCorridos)
                        vencimento = vencimentoInicial.AddMonths(i);
                    if (intervalo != 30 || Configuracoes.diasCorridos)
                        vencimento = vencimentoInicial.AddDays(intervalo * i);
                }

                var dados = new[]
                                {
                                   new PagamentoNFe{tipo="CR",valor=valorParcela,parcela=seqPag.GetValueOrDefault(),vencimento=vencimento}
                                };
                pagamento.AddRange(dados);
                seqPag++;

                if (i == 0)
                {
                    valorParcela -= restoDivisao;
                    restoDivisao = 0;
                }
            }
            return true;
        }

        private void objetoParaXML(XmlWriter xmlWriter, object objeto, bool ignorarDeclaracaoElemento)
        {
            if (objeto == null)
                return;

            Type tipoObjeto;
            tipoObjeto = objeto.GetType();
            PropertyInfo[] propriedades;
            propriedades = tipoObjeto.GetProperties();

            if (!ignorarDeclaracaoElemento)
                xmlWriter.WriteStartElement(tipoObjeto.Name);

            foreach (PropertyInfo propriedade in propriedades)
            {
                if (FuncoesNFe.novaTag(propriedade) && !(propriedade.GetValue(objeto, null) == null))
                {
                    objetoParaXML(xmlWriter, propriedade.GetValue(objeto, null), false);
                    continue;
                }

                object[] obj = propriedade.GetCustomAttributes(false);
                FuncoesNFe.gravarElemento(xmlWriter, propriedade.Name, propriedade.GetValue(objeto, null), obj);
            }
            if (!ignorarDeclaracaoElemento)
                xmlWriter.WriteEndElement();
        }

        public XmlDocument GerarXML()
        {
            XmlWriterSettings configXML = new XmlWriterSettings();
            configXML.Indent = true;
            configXML.IndentChars = "";
            configXML.NewLineOnAttributes = false;
            configXML.OmitXmlDeclaration = false;

            Stream xmlSaida = new MemoryStream();

            XmlWriter oXmlGravar = XmlWriter.Create(xmlSaida, configXML);

            oXmlGravar.WriteStartDocument();
            oXmlGravar.WriteStartElement("NFe", "http://www.portalfiscal.inf.br/nfe"); //abre nfe
            oXmlGravar.WriteStartElement("infNFe");
            oXmlGravar.WriteAttributeString("Id", "NFe" + Id.ToString());
            oXmlGravar.WriteAttributeString("versao", versao.ToString());

            Type tipoObjeto;
            tipoObjeto = infNFE.Ide.GetType();
            PropertyInfo[] propriedades;
            propriedades = tipoObjeto.GetProperties();

            objetoParaXML(oXmlGravar, infNFE.Ide, false);
            objetoParaXML(oXmlGravar, infNFE.Emit, false);
            objetoParaXML(oXmlGravar, infNFE.Dest, false);


            foreach (infNFE.det detalhe in infNFE.Det)
            {
                oXmlGravar.WriteStartElement("det");
                oXmlGravar.WriteAttributeString("nItem", detalhe.nItem.ToString());

                objetoParaXML(oXmlGravar, detalhe.Prod, false);

                oXmlGravar.WriteStartElement("imposto");
                objetoParaXML(oXmlGravar, detalhe.Imposto.Icms, false);
                objetoParaXML(oXmlGravar, detalhe.Imposto.Ii, false);
                objetoParaXML(oXmlGravar, detalhe.Imposto.Ipi, false);
                objetoParaXML(oXmlGravar, detalhe.Imposto.Pis, false);
                objetoParaXML(oXmlGravar, detalhe.Imposto.Cofins, false);

                oXmlGravar.WriteEndElement(); //fecha TAG imposto...
                oXmlGravar.WriteEndElement(); //fecha TAG det...
            }

            objetoParaXML(oXmlGravar, infNFE.Total, false);
            objetoParaXML(oXmlGravar, infNFE.Transp, false);
            //objetoParaXML(oXmlGravar, infNFE.Cobr, false);

            if (infNFE.Cobr != null)
            {
                oXmlGravar.WriteStartElement("cobr");
                foreach (infNFE.cobr.dup duplicata in infNFE.Cobr.Dup)
                {
                    objetoParaXML(oXmlGravar, duplicata, false);
                }

                oXmlGravar.WriteEndElement(); //fecha tag cobr

            }
            objetoParaXML(oXmlGravar, infNFE.InfAdic, false);

            oXmlGravar.WriteEndElement(); //fecha infNFe
            oXmlGravar.WriteEndElement(); //fecha NFe            

            oXmlGravar.Flush();
            xmlSaida.Flush();
            xmlSaida.Position = 0;

            XmlDocument documento = new XmlDocument();
            documento.Load(xmlSaida);

            //documento.Save("c:\\testeXML.xml");

            oXmlGravar.Close();

            return documento;
        }        

        public void MontarNotaFiscal(int documento, string cfop = "5.929")
        {

            #region
            /*
            #region
            var ResumoDocumento = (from d in Conexao.CriarEntidade().contdocs
                                   where d.documento == documento
                                   select new
                                   {
                                       codigoCliente = d.codigocliente,
                                       parcelas = d.NrParcelas,
                                       TotalDocumento = d.Totalbruto,
                                       tipopagamento = d.tipopagamento,
                                       DataDocumento = d.data,
                                       Desconto = d.desconto,
                                       nfe = d.nrnotafiscal
                                   }).FirstOrDefault();


   
            var itensDocumento = (from it in Conexao.CriarEntidade().venda
                                  where it.documento == documento
                                  && it.cancelado == "N"
                                  && it.quantidade > 0
                                  orderby it.documento
                                  select new
                                  {
                                      codigo = it.codigo,
                                      descricao = it.produto,
                                      quantidade = it.quantidade,
                                      preco = it.preco,
                                      desconto = it.ratdesc,
                                      total = it.total,
                                      icms = it.icms,
                                      ResucaoICMS = it.percentualRedBaseCalcICMS,
                                      tributacaoICMS = it.tributacao,
                                      cstIPI = it.cstipi,
                                      aliqIPI = it.aliquotaIPI,
                                      cofins = it.cofins,
                                      pis = it.pis,
                                      cfop = it.cfop,
                                      cstpis = it.cstpis,
                                      cstcofins = it.cstcofins,
                                      icmsST = it.icmsst,
                                      ReducaoICMSST = it.percentualRedBaseCalcICMSST,
                                      MVA = it.mvast,
                                      ncm = it.ncm,
                                      coo = it.coo,
                                      ccf = it.ccf,
                                      doc = it.documento,
                                      ecf = it.ecffabricacao,
                                      seqECF = it.Ecfnumero,
                                      vendaAtacado = it.vendaatacado
                                  }).ToList();


            var itensPagamento = (from n in Conexao.CriarEntidade().caixa
                                  where n.documento == documento
                                  select new { n.Nrparcela, n.tipopagamento, n.valor, n.vencimento }).ToList();



            if (itensDocumento.Count == 0)
            {
                itensDocumento = (from it in Conexao.CriarEntidade().vendaarquivo
                                  where it.documento == documento
                                  && it.cancelado == "N"
                                  && it.quantidade > 0
                                  orderby it.documento
                                  select new
                                  {
                                      codigo = it.codigo,
                                      descricao = it.produto,
                                      quantidade = it.quantidade,
                                      preco = it.preco,
                                      desconto = it.ratdesc,
                                      total = it.total,
                                      icms = it.icms,
                                      ResucaoICMS = it.percentualRedBaseCalcICMS,
                                      tributacaoICMS = it.tributacao,
                                      cstIPI = it.cstipi,
                                      aliqIPI = it.aliquotaIPI,
                                      cofins = it.cofins,
                                      pis = it.pis,
                                      cfop = it.cfop,
                                      cstpis = it.cstpis,
                                      cstcofins = it.cstcofins,
                                      icmsST = it.icmsst,
                                      ReducaoICMSST = it.percentualRedBaseCalcICMSST,
                                      MVA = it.mvast,
                                      ncm = it.ncm,
                                      coo = it.coo,
                                      ccf = it.ccf,
                                      doc = it.documento,
                                      ecf = it.ecffabricacao,
                                      seqECF = it.ecfnumero,
                                      vendaAtacado = it.vendaatacado
                                  }).ToList();

                itensPagamento = (from n in Conexao.CriarEntidade().caixaarquivo
                                  where n.documento == documento
                                  select new { n.Nrparcela, n.tipopagamento, n.valor, n.vencimento }).ToList();

            }


            #endregion


            #region
            List<itensNFe> novo = new List<itensNFe>();

            int? seq = (from n in itens select (int?)n.sequencia).Max() + 1;
            if (!seq.HasValue)
                seq = 1;


            foreach (var item in itensDocumento)
            {
                var dados = new[]
                    {
                       new itensNFe{sequencia = seq.GetValueOrDefault(),codigo=item.codigo,descricao = item.descricao,
                           quantidade=item.quantidade,preco=item.preco.Value,desconto=item.desconto,total=item.total,cfop=cfop,
                           csticms=item.tributacaoICMS,icms=item.icms,ipi=item.aliqIPI.Value,icmsst = item.icmsST,
                           redbc = item.ResucaoICMS, redbcst = item.ReducaoICMSST, mva = item.MVA,cstipi = item.cstIPI,
                           cstpis=item.cstpis,pis=item.pis.Value,cofins=item.cofins.Value,cstcofins=item.cstcofins,
                           coo=item.coo,ccf=item.ccf,documento=item.doc,ecf=item.ecf,seqECF=item.seqECF,vendaAtacado=item.vendaAtacado},                           
                    };
                seq++;
                novo.AddRange(dados);
            };
            itens.AddRange(novo);

            List<PagamentoNFe> novoPag = new List<PagamentoNFe>();
            int? seqPag = (from n in pagamento select (int?)n.parcela).Max() + 1;
            if (!seqPag.HasValue)
                seqPag = 1;

            foreach (var item in itensPagamento)
            {
                var dados = new[]
                    {
                        new PagamentoNFe{parcela= seqPag.GetValueOrDefault(),valor=item.valor,vencimento = item.vencimento.GetValueOrDefault(),tipo=item.tipopagamento}
                    };
                seqPag++;
                novoPag.AddRange(dados);
            }
            pagamento.AddRange(novoPag);

            #endregion

            */

            #endregion

            string SQL = "CALL montarNFCe("+documento+",'"+GlbVariaveis.glb_filial+"','"+GlbVariaveis.glb_IP+"','N','"+GlbVariaveis.glb_Usuario+"')";
            LogSICEpdv.Registrarlog(SQL, "true", "NFe.cs");
            Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
        }

        public bool SalvarNFe()
        {

            NFe teste = new NFe();

            teste.versao = "3.1";

            teste.infNFE.Ide.cUF = 41;
            teste.infNFE.Ide.cNF = "00001430";
            teste.infNFE.Ide.natOp = "VENDAS DE MERCADORIA A PRAZO";
            teste.infNFE.Ide.indPag = "1";
            teste.infNFE.Ide.mod = "55";
            teste.infNFE.Ide.serie = "2";
            teste.infNFE.Ide.nNF = "1261";
            teste.infNFE.Ide.dEmi = DateTime.Now.AddDays(-1);
            teste.infNFE.Ide.dSaiEnt = DateTime.Now.AddDays(-1);
            teste.infNFE.Ide.tpNF = 1;
            teste.infNFE.Ide.cMunFG = 4106902;
            teste.infNFE.Ide.tpImp = 1;
            teste.infNFE.Ide.tpEmis = "1";
            teste.infNFE.Ide.tpAmb = 2;
            teste.infNFE.Ide.finNFe = "1";
            teste.infNFE.Ide.procEmi = "0";
            teste.infNFE.Ide.verProc = "3.1";



            teste.infNFE.Emit.CNPJ = "03590277000100";
            teste.infNFE.Emit.xNome = "TESTANDO A GERACAO DE XML";
            teste.infNFE.Emit.xFant = "TESTE TESTANDO A GERACAO DE XML";
            teste.infNFE.Emit.IE = "017882303";
          
                teste.infNFE.Emit.CRT = "3";

            string _codUF = teste.infNFE.Ide.cUF.ToString();
            string _dEmi = teste.infNFE.Ide.dEmi.ToString("yyMM");
            string _cnpj = FuncoesNFe.removeFormatacao(teste.infNFE.Emit.CNPJ);
            string _mod = teste.infNFE.Ide.mod;

            string _serie = string.Format("{0:000}", Int32.Parse(teste.infNFE.Ide.serie));
            string _numNF = string.Format("{0:000000000}", Int32.Parse(teste.infNFE.Ide.nNF));

            string _codigo = string.Format("{0:000000000}", Int32.Parse(teste.infNFE.Ide.cNF));

            string chaveNF = _codUF + _dEmi + _cnpj + _mod + _serie + _numNF + _codigo;

            int _dv = FuncoesNFe.modulo11(chaveNF);

            teste.Id = chaveNF + _dv.ToString();
            teste.infNFE.Ide.cDV = _dv.ToString();
            //MessageBox.Show("ID: " + teste.Id.ToString());


            teste.infNFE.Emit.EnderEmit.xLgr = "INFORMAR UM ENDERECO";
            teste.infNFE.Emit.EnderEmit.nro = "77";
            teste.infNFE.Emit.EnderEmit.xBairro = "INFORMAR BAIRRO";
            teste.infNFE.Emit.EnderEmit.cMun = 4106902;
            teste.infNFE.Emit.EnderEmit.xMun = "MARECHAL CANDIDO RONDON";
            teste.infNFE.Emit.EnderEmit.UF = "PR";
            teste.infNFE.Emit.EnderEmit.CEP = "85960000";
            teste.infNFE.Emit.EnderEmit.cPais = 1058;
            teste.infNFE.Emit.EnderEmit.xPais = "BRASIL";
            teste.infNFE.Emit.EnderEmit.fone = "30336300";

            teste.infNFE.Dest.CPF = "02371692409";
            teste.infNFE.Dest.xNome = "INFORMAR UM NOME";
            teste.infNFE.Dest.EnderDest.xLgr = "INFORMAR UM ENDERECO";
            teste.infNFE.Dest.EnderDest.nro = "0";
            teste.infNFE.Dest.EnderDest.xBairro = "INFORMAR UM BAIRRO";
            teste.infNFE.Dest.EnderDest.cMun = 4106902;
            teste.infNFE.Dest.EnderDest.xMun = "MARECHAL CANDIDO RONDON";
            teste.infNFE.Dest.EnderDest.UF = "PR";
            teste.infNFE.Dest.EnderDest.CEP = "85960000";
            teste.infNFE.Dest.EnderDest.cPais = 1058;
            teste.infNFE.Dest.EnderDest.xPais = "BRASIL";
            teste.infNFE.Dest.IE = "";

            infNFE.det detalhamento = new infNFE.det();
            detalhamento.nItem = 1;
            detalhamento.Prod.cProd = "1/1";
            detalhamento.Prod.cEAN = "";
            detalhamento.Prod.cEANTrib = "";
            detalhamento.Prod.xProd = "INFORMAR UM PRODUTO";
            detalhamento.Prod.NCM = "39174000";
            detalhamento.Prod.CFOP = "5102";
            detalhamento.Prod.uCom = "PC";
            detalhamento.Prod.qCom = 1;
            detalhamento.Prod.vUnCom = 550;
            detalhamento.Prod.vProd = 550;
            detalhamento.Prod.uTrib = "UN";
            detalhamento.Prod.qTrib = 1;
            detalhamento.Prod.vUnTrib = 550;

            detalhamento.Prod.indTot = "1";


            detalhamento.Imposto.Icms = new infNFE.det.imposto.ICMS();
            detalhamento.Imposto.Icms.Icms00 = new infNFE.det.imposto.ICMS.ICMS00();
            detalhamento.Imposto.Icms.Icms00.CST = "00";
            detalhamento.Imposto.Icms.Icms00.orig = "0";
            detalhamento.Imposto.Icms.Icms00.modBC = "0";
            detalhamento.Imposto.Icms.Icms00.vBC = 0;
            detalhamento.Imposto.Icms.Icms00.pICMS = 0;
            detalhamento.Imposto.Icms.Icms00.vICMS = 0;

            detalhamento.Imposto.Pis = new infNFE.det.imposto.PIS();
            detalhamento.Imposto.Pis.PisAliq = new infNFE.det.imposto.PIS.PISAliq();
            detalhamento.Imposto.Pis.PisAliq.CST = "01";
            detalhamento.Imposto.Pis.PisAliq.vBC = 0;
            detalhamento.Imposto.Pis.PisAliq.pPIS = 0;
            detalhamento.Imposto.Pis.PisAliq.vPIS = 0;


            detalhamento.Imposto.Cofins = new infNFE.det.imposto.COFINS();
            detalhamento.Imposto.Cofins.CofinsAliq = new infNFE.det.imposto.COFINS.COFINSAliq();
            detalhamento.Imposto.Cofins.CofinsAliq.CST = "01";
            detalhamento.Imposto.Cofins.CofinsAliq.vBC = 0;
            detalhamento.Imposto.Cofins.CofinsAliq.pCOFINS = 0;
            detalhamento.Imposto.Cofins.CofinsAliq.vCOFINS = 0;


            teste.infNFE.Total = new infNFE.total();

            teste.infNFE.Det.Add(detalhamento);

            teste.infNFE.Total.IcmsTot = new infNFE.total.ICMSTot();
            teste.infNFE.Total.IcmsTot.vBC = 0;
            teste.infNFE.Total.IcmsTot.vBCST = 0;
            teste.infNFE.Total.IcmsTot.vST = 0;
            teste.infNFE.Total.IcmsTot.vProd = 25;
            teste.infNFE.Total.IcmsTot.vFrete = 0;
            teste.infNFE.Total.IcmsTot.vSeg = 0;
            teste.infNFE.Total.IcmsTot.vDesc = 0;
            teste.infNFE.Total.IcmsTot.vII = 0;
            teste.infNFE.Total.IcmsTot.vIPI = 0;
            teste.infNFE.Total.IcmsTot.vPIS = 0;
            teste.infNFE.Total.IcmsTot.vCOFINS = 0;
            teste.infNFE.Total.IcmsTot.vOutro = 0;
            teste.infNFE.Total.IcmsTot.vNF = 25;

            teste.infNFE.Transp = new infNFE.transp();
            teste.infNFE.Transp.modFrete = "0";

            //Gera o XML
            XmlDocument xmlGerado = teste.GerarXML();

            xmlGerado.Save("c:\\iqsistemas\\testeXMLNaoAssinado.xml");
            //Seleciona o certificado
            X509Certificate2 certificado = CertificadoDigital.SelecionarCertificado();
            //assina o xml
            XmlDocument xmlAssinado = CertificadoDigital.Assinar(xmlGerado, "infNFe", certificado);

            //Valida o XML assinado
            string resultado = ValidaXML.ValidarXML(xmlAssinado, "3.1");

            if (resultado.Trim().Length == 0)
            {
                resultado = "Xml gerado com sucesso, nenhum erro encontrado.";
                
            }

            //Opcional - Função para gerar o Lote e deixar o arquivo pronto para ser enviado.
            //teste.GerarLoteNfe(ref xmlAssinado);

            //Importante:
            //Salvar através do TextWriter evita que o XML saia formatado no arquivo, desta forma o mesmo
            //pode ser rejeitado por alguns estados e/ou não validar nos programas teste
            using (XmlTextWriter xmltw = new XmlTextWriter("C:\\iqsistemas\\testeXML.xml", new UTF8Encoding(false)))
            {
                xmlAssinado.WriteTo(xmltw);
                xmltw.Close();
            }
            return true;
        }

        public bool reenviar(int numeroNF,int DOc, bool mostrarMsg, string msgComplementar = "", bool identificarCliente = false, bool gerar = true)
        {

            try
            {
                #region

                string SQL = "";
                string arquivado = "N";

                var documento = (from d in Conexao.CriarEntidade().contdocs
                                 where d.documento == DOc && d.CodigoFilial == GlbVariaveis.glb_filial
                                 select d).FirstOrDefault();


                var crt = (from f in Conexao.CriarEntidade().filiais where f.CodigoFilial == GlbVariaveis.glb_filial select f.crt).FirstOrDefault();

                if (crt.Length > 0)
                    crt = crt.Substring(0, 1).ToString();
                else if (crt == "")
                    crt = "1";

                string serieNFCe = "";
                if (documento.chaveNFC != "Erro" && documento.chaveNFC != null && documento.chaveNFC != "")
                {
                    serieNFCe = FuncoesNFC.lerChaveNFCe(documento.chaveNFC, "S");

                    /*if (int.Parse(documento.ecfcontadorcupomfiscal) != int.Parse(serieNFCe))
                        FuncoesNFC.ajustarNFCeChave(DOc);*/

                    if (int.Parse(documento.ecfcontadorcupomfiscal) != int.Parse(serieNFCe) || int.Parse(documento.ecfcontadorcupomfiscal) != int.Parse(ConfiguracoesECF.NFCserie))
                    {
                        if (mostrarMsg == true)
                        {
                            MessageBox.Show("NFCe não pertence a esse caixa!-> Caixa.:" + ConfiguracoesECF.NFCserie + " / Serie Documento.: " + documento.ecfcontadorcupomfiscal, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }

                    documento = (from d in Conexao.CriarEntidade().contdocs
                                where d.documento == DOc && d.CodigoFilial == GlbVariaveis.glb_filial
                                 select d).FirstOrDefault();
                }


                
                if (Configuracoes.cfgarquivardados == "S")
                {
                    SQL = "select cbdArquivado from cbd001 WHERE CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "' AND cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND cbdmod = '65'";
                    arquivado = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                    if (arquivado == "S")
                    {
                        //if (mostrarMsg == true)
                        //{
                        //MessageBox.Show("Não é possivel autorizar um NFCe Arquivado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //}
                        Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE contdocs AS c SET " +
                                        "c.dataAutorizacao = (SELECT CbdDtaProcessamento FROM nfe012arquivo WHERE cbdDocumento = '" + documento.documento.ToString() + "' LIMIT 1), " +
                                        "c.protocolo = (SELECT IF((CbdNumProtCanc IS NOT NULL AND CbdNumProtCanc <> '0'), CbdNumProtCanc,CbdNumProtocolo) " +
                                        "FROM nfe012arquivo WHERE cbdDocumento = '" + documento.documento.ToString() + "' LIMIT 1), " +
                                        "c.chaveNFC = (SELECT CbdNFEChaAcesso FROM nfe012arquivo WHERE cbdDocumento = '" + documento.documento.ToString() + "' LIMIT 1), " +
                                        "c.ncupomfiscal = (SELECT cbdntfnumero FROM nfe012arquivo WHERE cbdDocumento = '" + documento.documento.ToString() + "' LIMIT 1), " +
                                        "c.modeloDOCFiscal = '65', " +
                                        "c.ecfcontadorcupomfiscal = (SELECT CbdNtfSerie FROM nfe012arquivo WHERE cbdDocumento = '" + documento.documento.ToString() + "' LIMIT 1), " +
                                        "c.ecffabricacao = 'SICENFCe' " +
                                        "WHERE c.documento = '" + documento.documento.ToString() + "'");
                        return false;
                    }
                }


                int qtdNfe012 = 0;

                if(numeroNF > 0)
                {
                    SQL = "select IFNULL(count(1),0) as quantidade from nfe012 WHERE cbddocumento = '" + DOc.ToString() + "' AND cbdmod = '65'";
                    qtdNfe012 = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                    SQL = "select IFNULL(cbdntfNumero,0) from nfe012 WHERE cbddocumento = '" + DOc.ToString() + "' AND cbdmod = '65'";
                    int NumeroNfe012 = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                    if (NumeroNfe012 > 0 && NumeroNfe012 != numeroNF)
                    {
                        SQL = "UPDATE contdocs SET chaveNFC = (select CbdNFEChaAcesso from nfe012 WHERE cbddocumento = '" + DOc.ToString() + "' AND cbdmod = '65' LIMIT 1) , " +
                              "protocolo = (select CbdNumProtocolo from nfe012 WHERE cbddocumento = '" + DOc.ToString() + "' AND cbdmod = '65' LIMIT 1), " +
                              "ecfcontadorcupomfiscal = (select cbdNtfSerie from nfe012 WHERE cbddocumento = '" + DOc.ToString() + "' AND cbdmod = '65' LIMIT 1), " +
                              "ncupomfiscal = (select cbdNtfNumero from nfe012 WHERE cbddocumento = '" + DOc.ToString() + "' AND cbdmod = '65' LIMIT 1)  " +
                              "WHERE documento ='" + DOc.ToString() + "' "+
                              "AND codigofilial = '" + GlbVariaveis.glb_filial + "' AND modelodocfiscal = '65' ";

                        Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

                        SQL = "UPDATE contdocs SET ncupomfiscal = LPAD(ncupomfiscal,9,'0') , ecfcontadorcupomfiscal = LPAD(ecfcontadorcupomfiscal,3,'0') " +
                              "WHERE documento ='" + DOc.ToString() + "' " +
                              "AND codigofilial = '" + GlbVariaveis.glb_filial + "' AND modelodocfiscal = '65' ";

                        Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

                        documento = (from d in Conexao.CriarEntidade().contdocs
                                     where d.documento == DOc && d.CodigoFilial == GlbVariaveis.glb_filial
                                     select d).FirstOrDefault();

                        numeroNF = int.Parse(documento.ncupomfiscal);

                    }
                }

                //SQL = "select IFNULL(count(1),0) as quantidade from nfe012 WHERE cbddocumento = '" + DOc.ToString() + "' AND cbdmod = '65'";
                //qtdNfe012 = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                SQL = "update venda set ncmespecie = 0 where  ncmespecie = '' and documento = '" + DOc.ToString() + "'";
                Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                SQL = "update vendaarquivo set ncmespecie = 0 where  ncmespecie = '' and documento = '" + DOc.ToString() + "'";
                Conexao.CriarEntidade().ExecuteStoreCommand(SQL);


                string status = "";
                string protocoloNFe012 = "";
                string chaveNFe012 = "";
                string XMLNFe012 = "";
                string statusProc = "";
                int qtdNota = 0;
                int diasEmissao = 0;
                nfe012E objNfe012 = new nfe012E();



                if (qtdNfe012 == 0)
                {
                    /*SQL = "SELECT CbdStsRetCodigo FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'  AND CbdMod = '65' ";
                    status = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();
                    SQL = "SELECT CbdNumProtocolo FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'  AND CbdMod = '65' ";
                    protocoloNFe012 = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();
                    SQL = "SELECT CbdNFEChaAcesso FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'  AND CbdMod = '65' ";
                    chaveNFe012 = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                    SQL = "SELECT CbdXML FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'  AND CbdMod = '65' ";
                    XMLNFe012 = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                    SQL = "SELECT count(1) as quantidade FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'  AND CbdMod = '65' ";
                    qtdNota = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                    statusProc = "SELECT CbdProcStatus FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'  AND CbdMod = '65' ";
                    qtdNota = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();*/

                    /*SQL = "SELECT IFNULL(CbdEmpCodigo,0) AS CbdEmpCodigo,IFNULL(CbdNtfNumero,0) AS CbdNtfNumero,IFNULL(CbdNtfSerie,0) AS CbdNtfSerie,IFNULL(CbdAcao,'') AS CbdAcao,IFNULL(CbdSituacao,'') AS CbdSituacao, " +
                    "IFNULL(CbdDtaProcessamento, 0) AS CbdDtaProcessamento, IFNULL(CbdNumProtocolo,'') AS CbdNumProtocolo, IFNULL(CbdStsRetCodigo,'') AS CbdStsRetCodigo, IFNULL(CbdStsRetNome,'') AS CbdStsRetNome," +
                    "IFNULL(CbdProcStatus,'') AS CbdProcStatus, IFNULL(CbdNFEChaAcesso,'') AS CbdNFEChaAcesso, IFNULL(CbdxMotivo,'') AS CbdxMotivo, IFNULL(CbdXML,'') AS CbdXML," +
                    "IFNULL(CbdNumProtCanc,'') AS CbdNumProtCanc, IFNULL(CbdDtaCancelamento,'') AS CbdDtaCancelamento, IFNULL(CbdDtaInutilizacao,'') AS CbdDtaInutilizacao, IFNULL(CbdNumProtInut,'') AS CbdNumProtInut, " +
                    "IFNULL(CbdMarca,'') AS CbdMarca, IFNULL(CbdDigVal,'') AS CbdDigVal, IFNULL(cbdcodigofilial,'') AS cbdcodigofilial, IFNULL(cbdCCe,'') AS cbdCCe, IFNULL(cbdCCePath,'') AS cbdCCePath," +
                    "IFNULL(cbdCCenSeqEvento,'') AS cbdCCenSeqEvento, IFNULL(CbdMod,'') AS CbdMod, IFNULL(cbdDocumento,'') AS cbdDocumento, IFNULL(cbdNRec,'') AS cbdNRec," +
                    "IFNULL(cbdVerificado,'') AS cbdVerificado, IFNULL(CdbNtfNumeroFinal,'') AS CdbNtfNumeroFinal, IFNULL(Cbdflag,'') AS Cbdflag, IFNULL(count(1),0) as qtdNota FROM nfe012 WHERE cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'  AND CbdMod = '65' ";

                    var nfe012E = Conexao.CriarEntidade().ExecuteStoreQuery<nfe012E>(SQL).FirstOrDefault();*/


                   
                    var nfe012E = objNfe012.notafiscal(int.Parse(documento.ncupomfiscal).ToString(), int.Parse(documento.ecfcontadorcupomfiscal).ToString());

                    status = nfe012E.CbdStsRetCodigo;
                    protocoloNFe012 = nfe012E.CbdNumProtocolo;
                    chaveNFe012 = nfe012E.CbdNFEChaAcesso;
                    XMLNFe012 = nfe012E.CbdXML;
                    statusProc = nfe012E.CbdProcStatus;
                    qtdNota = int.Parse(nfe012E.qtdNota);
                    diasEmissao = int.Parse(nfe012E.dias);
                    //gerar = objNfe012.gerarXML(nfe012E);

                }
                else
                {
                    /*SQL = "SELECT CbdStsRetCodigo FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.cbddocumento = '" + DOc.ToString() + "' AND CbdMod = '65' ";
                    status = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();
                    SQL = "SELECT CbdNumProtocolo FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.cbddocumento = '" + DOc.ToString() + "' AND CbdMod = '65' ";
                    protocoloNFe012 = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();
                    SQL = "SELECT CbdNFEChaAcesso FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.cbddocumento = '" + DOc.ToString() + "' AND CbdMod = '65' ";
                    chaveNFe012 = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                    SQL = "SELECT CbdXML FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.cbddocumento = '" + DOc.ToString() + "' AND CbdMod = '65' ";
                    XMLNFe012 = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                    SQL = "SELECT count(1) as quantidade FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.cbddocumento = '" + DOc.ToString() + "'  AND CbdMod = '65' ";
                    qtdNota = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                    statusProc = "SELECT CbdProcStatus FROM nfe012 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'  AND CbdMod = '65' ";
                    qtdNota = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();*/

                    /*SQL = "SELECT IFNULL(CbdEmpCodigo,0) AS CbdEmpCodigo,IFNULL(CbdNtfNumero,0) AS CbdNtfNumero,IFNULL(CbdNtfSerie,0) AS CbdNtfSerie,IFNULL(CbdAcao,'') AS CbdAcao,IFNULL(CbdSituacao,'') AS CbdSituacao, " +
                    "IFNULL(CbdDtaProcessamento, 0) AS CbdDtaProcessamento, IFNULL(CbdNumProtocolo,'') AS CbdNumProtocolo, IFNULL(CbdStsRetCodigo,'') AS CbdStsRetCodigo, IFNULL(CbdStsRetNome,'') AS CbdStsRetNome," +
                    "IFNULL(CbdProcStatus,'') AS CbdProcStatus, IFNULL(CbdNFEChaAcesso,'') AS CbdNFEChaAcesso, IFNULL(CbdxMotivo,'') AS CbdxMotivo, IFNULL(CbdXML,'') AS CbdXML," +
                    "IFNULL(CbdNumProtCanc,'') AS CbdNumProtCanc, IFNULL(CbdDtaCancelamento,'') AS CbdDtaCancelamento, IFNULL(CbdDtaInutilizacao,'') AS CbdDtaInutilizacao, IFNULL(CbdNumProtInut,'') AS CbdNumProtInut, " +
                    "IFNULL(CbdMarca,'') AS CbdMarca, IFNULL(CbdDigVal,'') AS CbdDigVal, IFNULL(cbdcodigofilial,'') AS cbdcodigofilial, IFNULL(cbdCCe,'') AS cbdCCe, IFNULL(cbdCCePath,'') AS cbdCCePath," +
                    "IFNULL(cbdCCenSeqEvento,'') AS cbdCCenSeqEvento, IFNULL(CbdMod,'') AS CbdMod, IFNULL(cbdDocumento,'') AS cbdDocumento, IFNULL(cbdNRec,'') AS cbdNRec," +
                    "IFNULL(cbdVerificado,'') AS cbdVerificado, IFNULL(CdbNtfNumeroFinal,'') AS CdbNtfNumeroFinal, IFNULL(Cbdflag,'') AS Cbdflag, IFNULL(count(1),0) as qtdNota FROM nfe012 WHERE cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND cbddocumento = '" + DOc.ToString() + "' AND CbdMod = '65' ";

                    var nfe012E = Conexao.CriarEntidade().ExecuteStoreQuery<nfe012E>(SQL).FirstOrDefault();*/

                    
                    var nfe012E = objNfe012.notafiscalDocumento(DOc.ToString());

                    status = nfe012E.CbdStsRetCodigo;
                    protocoloNFe012 = nfe012E.CbdNumProtocolo;
                    chaveNFe012 = nfe012E.CbdNFEChaAcesso;
                    XMLNFe012 = nfe012E.CbdXML;
                    statusProc = nfe012E.CbdProcStatus;
                    qtdNota = int.Parse(nfe012E.qtdNota);
                    diasEmissao = int.Parse(nfe012E.dias);
                    //gerar = objNfe012.gerarXML(nfe012E);
                }



                if (documento.modeloDOCFiscal == "65")
                {
                    if (qtdNota > 0 && status == "100" && protocoloNFe012 != null && (documento.chaveNFC == "" || documento.chaveNFC == null || documento.chaveNFC == "Erro" || documento.protocolo == "000000000000000"))
                    {
                        serieNFCe = FuncoesNFC.lerChaveNFCe(chaveNFe012, "S");

                        SQL = "UPDATE contdocs SET chaveNFC = '" + chaveNFe012 + "' , protocolo = '" + protocoloNFe012 + "', ecfcontadorcupomfiscal = '" + serieNFCe + "', ncupomfiscal = LPAD(ncupomfiscal,9,'0')  WHERE documento ='" + documento.documento + "' AND codigofilial = '" + documento.CodigoFilial + "' AND modelodocfiscal = '" + documento.modeloDOCFiscal + "'";
                        Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

                        if (mostrarMsg == true)
                        {
                            MessageBox.Show("Dados da NFCe atualizado com sucesso!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MessageBox.Show("Agora tente imprimir!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return false;
                    }
                    else if (documento.chaveNFC != "Erro" && documento.chaveNFC != null && documento.chaveNFC != "" && int.Parse(documento.ecfcontadorcupomfiscal) != int.Parse(serieNFCe))
                    {
                        if (mostrarMsg == true)
                        {
                            MessageBox.Show("NFCe não pertence a esse caixa!-> Caixa.:" + ConfiguracoesECF.NFCserie + " / Serie Documento.: " + documento.ecfcontadorcupomfiscal, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    else if (int.Parse(documento.ecfcontadorcupomfiscal) != int.Parse(ConfiguracoesECF.NFCserie))
                    {
                        if (mostrarMsg == true)
                        {
                            MessageBox.Show("0 - NFCe não pertence a esse caixa!-> Caixa.:" + ConfiguracoesECF.NFCserie + " / Serie Documento.: " + documento.ecfcontadorcupomfiscal, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    else if (qtdNota > 0 && status == "100" && protocoloNFe012 != null && protocoloNFe012 != "0" && XMLNFe012 != null)
                    {
                        if (mostrarMsg == true)
                        {
                            MessageBox.Show(" 1 - Não é possivel reenviar um documento já autorizado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    else if (qtdNota > 0 && (documento.protocolo == null ? "" : documento.protocolo.ToString()) != "" && documento.protocolo != null && (documento.protocolo == null ? "" : documento.protocolo.ToString()) != "Erro" && protocoloNFe012 != "0" && protocoloNFe012 != "" && protocoloNFe012 != null && XMLNFe012 != null)
                    {
                        if (mostrarMsg == true)
                        {
                            MessageBox.Show("2 - Não é possivel reenviar um documento já autorizado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    else if (documento.estornado == "S")
                    {
                        if (mostrarMsg == true)
                        {
                            MessageBox.Show("3 - Não é possivel reenviar um documento Cancelado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    else if (documento.CodigoFilial != GlbVariaveis.glb_filial)
                    {
                        if (mostrarMsg == true)
                        {
                            MessageBox.Show("Documento não é desta Filial", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    else
                    {

                        try
                        {
                            #region

                            int quantidade = 0;
                            int nf = 0;


                            SQL = "SELECT ifnull(COUNT(1),0) AS quantidade FROM cbd001 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.cbddocumento = '" + DOc.ToString() + "' AND cbdmod = '65' ";
                            quantidade = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                            if (quantidade == 0)
                            {
                                SQL = "SELECT ifnull(COUNT(1),0) AS quantidade FROM cbd001 AS c WHERE c.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "' AND cbdmod = '65'";
                                quantidade = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();
                            }

                            if (quantidade == 0)
                            {

                                NFe NotaFiscal = new NFe();
                                NotaFiscal.MontarNotaFiscal(documento.documento, "5.102");

                                NotaFiscal.modeloNFe = "65";
                                NotaFiscal.cfopNFe = "5.102";
                                NotaFiscal.cfopTransporteNFE = "";
                                NotaFiscal.chaveAcessoRefNFe = "";
                                NotaFiscal.colocarDataHoraNFe = "S";

                                if (numeroNF == 0)
                                    NotaFiscal.criarNF = "S";
                                else
                                    NotaFiscal.criarNF = "N";

                                NotaFiscal.dadosComplementarNFe = FuncoesECF.MensagemCupomECF(documento.total.Value, 0, documento.davnumero.Value, documento.documento);
                                NotaFiscal.descontoNFe = documento.desconto.Value;
                                NotaFiscal.despesasNFe = documento.encargos;
                                NotaFiscal.doc = documento.documento;
                                NotaFiscal.especieVolumeNFe = "";
                                NotaFiscal.filial = GlbVariaveis.glb_filial;
                                NotaFiscal.finalidadeNFe = "1";//normal
                                NotaFiscal.freteNFe = 0;
                                NotaFiscal.gerarICMS = "S";
                                NotaFiscal.idInfoComplementarNFe = 0; // pegar id do combox da observação
                                NotaFiscal.idTransportadoraNFe = 0;
                                NotaFiscal.idVeiculoNFe = 0;
                                NotaFiscal.indPag = "0";
                                NotaFiscal.ipTerminal = GlbVariaveis.glb_IP;
                                NotaFiscal.marcavolume = "";
                                NotaFiscal.naturezaOperacaoNFe = "Venda";
                                NotaFiscal.NFeEntradaAdEstoque = "N";

                                if (numeroNF == 0)
                                    NotaFiscal.NFeOrigem = 0;
                                else
                                    NotaFiscal.NFeOrigem = int.Parse(numeroNF.ToString());

                                NotaFiscal.operadorNFe = GlbVariaveis.glb_Usuario;
                                NotaFiscal.qtdVolumeNFe = 0;
                                NotaFiscal.seguroNFe = 0;
                                NotaFiscal.serieNFe = (documento.ecfcontadorcupomfiscal == null || int.Parse(documento.ecfcontadorcupomfiscal) == 0) ? int.Parse(ConfiguracoesECF.NFCserie.PadLeft(6, '0')) : int.Parse(documento.ecfcontadorcupomfiscal.PadLeft(6, '0'));
                                NotaFiscal.situacaoNFe = "00"; // 00-Documento Regular
                                NotaFiscal.tipoEmissaoNFe = "1"; //1-Normal;
                                NotaFiscal.tipoFreteNFe = "9";//9 - sem frete
                                NotaFiscal.tipoNFe = "1";//1-Saida
                                NotaFiscal.volumeNFe = 0;
                                NotaFiscal.crt = int.Parse(crt);
                                NotaFiscal.TotalPesoBrutoNFe = 0;
                                NotaFiscal.TotalPesoLiquidoNFe = 0;
                                NotaFiscal.idCliente = 0;

                                nf = NotaFiscal.GerarNFe();

                            }


                            if (gerar == true)
                            {
                                if (GlbVariaveis.glb_filial == "00001")
                                {

                                    SQL = "UPDATE cbd001det AS c, produtos AS p SET c.cbdNCM = p.ncm," +
                                                  " c.CbdCEST = IF(CbdCEST = '', NULL, CbdCEST), c.cbdgenero = c.cbdgenero = IFNULL(IF(p.ncmespecie = '','0',p.ncmespecie),'0'), " +
                                                  " c.cbdCFOP = IF(p.cfopsaida = '','5102',REPLACE(p.cfopsaida,'.','')), c.cbdgenero = IFNULL(IF(p.ncmespecie = '','0',p.ncmespecie),'0') " +
                                                  " WHERE c.cbdcProd = p.codigo " +
                                                  " AND p.codigofilial = c.cbdcodigofilial " +
                                                  " AND c.CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "' " +
                                                  " AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' ";

                                    Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                                    Conexao.CriarEntidade().SaveChanges();

                                }
                                else
                                {

                                    SQL = "UPDATE cbd001det AS c, produtosfilial AS p SET c.cbdNCM = p.ncm, " +
                                                   " c.CbdCEST = IF(CbdCEST = '', NULL, CbdCEST), c.cbdgenero = IFNULL(IF(p.ncmespecie = '','0',p.ncmespecie),'0'), " +
                                                   " c.cbdCFOP = IF(p.cfopsaida = '','5102',REPLACE(p.cfopsaida,'.','')), c.cbdgenero = IFNULL(IF(p.ncmespecie = '','0',p.ncmespecie),'0')  " +
                                                   " WHERE c.cbdcProd = p.codigo " +
                                                   " AND p.codigofilial = c.cbdcodigofilial " +
                                                   " AND c.CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "' " +
                                                   " AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' " +
                                                   " AND p.codigofilial = '" + GlbVariaveis.glb_filial + "'";

                                    Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                                    Conexao.CriarEntidade().SaveChanges();

                                }
                            


                                //identificação do cliente
                                if (documento.codigocliente != 0 && identificarCliente == true)
                                {
                                    SQL = " START TRANSACTION;" +
                                            " UPDATE cbd001 AS c, contdocs AS d " +
                                            " SET c.CbdCPF_dest = (SELECT cpf FROM clientes WHERE codigo=d.codigocliente LIMIT 1)," +
                                            " c.CbdCNPJ_dest = (SELECT cnpj FROM clientes WHERE codigo=d.codigocliente LIMIT 1)," +
                                            " c.CbdxNome_dest = (SELECT nome FROM clientes WHERE codigo=d.codigocliente LIMIT 1)," +
                                            " c.cbdnro_dest = ifnull((SELECT numero FROM clientes WHERE codigo = d.codigocliente LIMIT 1),'SN')" +
                                            " WHERE c.CbdNtfNumero = d.ncupomfiscal" +
                                            " AND c.CbdNtfSerie = d.ecfcontadorcupomfiscal" +
                                            " AND c.CbdCodigoFilial = d.CodigoFilial" +
                                            " AND c.Cbdmod = d.modeloDOCFiscal" +
                                            " AND d.ecffabricacao = '" + ConfiguracoesECF.nrFabricacaoECF + "'" +
                                            " AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "'" +
                                            " AND c.CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'" +
                                            " AND d.ecftipo = 'NFCe'" +
                                            " AND c.CbdCodigoFilial = '" + GlbVariaveis.glb_filial + "';" +
                                            "COMMIT;";

                                    Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

                                    SQL = "UPDATE clientes AS c, cbd001 AS b SET " +
                                          " b.CbdCNPJ_dest = c.cnpj," +
                                          " b.CbdCPF_dest = c.cpf," +
                                          " b.CbdxNome_dest = c.Nome," +
                                          " b.CbdxLgr_dest = c.endereco," +
                                          " b.CbdxEmail_dest = c.email," +
                                          " b.Cbdnro_dest = c.numero," +
                                          " b.CbdxBairro_dest = c.bairro," +
                                          " b.CbdxMun_dest = c.cidade," +
                                          " b.CbdUF_dest = c.estado," +
                                          " b.CbdCEP_dest = c.cep," +
                                          " b.CbdIE_dest = c.inscricao," +
                                          " b.CbdcMun_dest = (SELECT id FROM tab_municipios WHERE tab_municipios.nome = c.cidade and iduf = (SELECT id FROM estados WHERE uf = c.estado limit 1) limit 1)," +
                                          " b.Cbdfone_dest = CAST(IFNULL(IF(REPLACE(c.telefone,' ','') = '',0,REPLACE(c.telefone,' ','')),0) AS UNSIGNED INTEGER) " +
                                          " WHERE b.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "'" +
                                          " AND b.CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'" +
                                          " AND c.Codigo = '" + documento.codigocliente.ToString() + "' AND b.CbdCodigoFilial ='" + GlbVariaveis.glb_filial + "' ";

                                    Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                                }
                                else
                                {
                                    SQL = "START TRANSACTION;" +
                                           "UPDATE cbd001 AS c, contdocs AS d " +
                                           " SET c.CbdCPF_dest = if(LENGTH(d.ecfCPFCNPJconsumidor) < 12,d.ecfCPFCNPJconsumidor,'')," +
                                           " c.CbdCNPJ_dest = if(LENGTH(d.ecfCPFCNPJconsumidor) > 12,d.ecfCPFCNPJconsumidor,'')," +
                                           " c.CbdxNome_dest = d.ecfConsumidor," +
                                           " c.cbdnro_dest = 'SN'" +
                                           " WHERE abs(c.CbdNtfNumero) = abs(d.ncupomfiscal)" +
                                           " AND abs(c.CbdNtfSerie) = abs(d.ecfcontadorcupomfiscal)" +
                                           " AND c.CbdCodigoFilial = d.CodigoFilial" +
                                           " AND c.Cbdmod = d.modeloDOCFiscal" +
                                           " AND d.ecffabricacao = '" + ConfiguracoesECF.nrFabricacaoECF + "'" +
                                           " AND c.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "'" +
                                           " AND c.CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "'" +
                                           " AND d.ecftipo = 'NFCe'" +
                                           " AND c.CbdCodigoFilial = '" + GlbVariaveis.glb_filial + "';" +
                                           "COMMIT;";
                                    LogSICEpdv.Registrarlog(SQL, "", "");
                                    Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                                }

                            }


                            if (quantidade > 0)
                            {
                                Venda venda = new Venda();
                                venda.autorizarNota(documento.ecfcontadorcupomfiscal, DOc, int.Parse(documento.ncupomfiscal), mostrarMsg,"",gerar);
                            }
                            else
                            {
                                Venda venda = new Venda();
                                venda.autorizarNota(ConfiguracoesECF.NFCserie.PadLeft(3, '0'), int.Parse(documento.documento.ToString()), nf, mostrarMsg, msgComplementar,gerar);
                                SQL = "UPDATE nfe012 AS n SET n.CbdNumProtocolo = (SELECT c.protocolo FROM contdocs as c WHERE c.documento = '" + DOc + "' AND c.codigofilial = n.cbdcodigofilial AND abs(c.ncupomfiscal) = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND abs(c.ecfcontadorcupomfiscal) = '" + int.Parse(documento.ecfcontadorcupomfiscal) + "' LIMIT 1) WHERE n.CbdNumProtocolo = 0 AND n.cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND n.CbdNtfNumero = '" + int.Parse(documento.ncupomfiscal).ToString() + "' AND n.CbdNtfSerie = '" + int.Parse(documento.ecfcontadorcupomfiscal).ToString() + "' and cbddocumento = '" + DOc + "' AND cbdmod = '65' ";
                                Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                            }


                            if (Configuracoes.cfgarquivardados == "S")
                            {
                                //SQL = "call atualizarNFe('0','0','65','" + GlbVariaveis.glb_filial + "','"+documento.documento+"')";
                                //Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                                //Conexao.CriarEntidade().SaveChanges();

                                try
                                {
                                    SQL = "call atualizarNFe('0','0','65','" + GlbVariaveis.glb_filial + "','" + documento.documento.ToString() + "')";
                                    string retorno = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                    if (retorno != "1")
                                        MessageBox.Show(retorno.ToString());

                                    //Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                                    //Conexao.CriarEntidade().SaveChanges();
                                }
                                catch (Exception)
                                {
                                    SQL = "call atualizarNFe('0','0','65','" + GlbVariaveis.glb_filial + "','" + documento.documento.ToString() + "','S')";
                                    string retorno = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                    if (retorno != "1")
                                        MessageBox.Show(retorno.ToString());

                                    //Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                                    //Conexao.CriarEntidade().SaveChanges();
                                }
                            }

                            return true;
                            #endregion
                        }
                        catch (Exception erro)
                        {
                            throw new Exception(erro.ToString());
                            //return false;
                        }
                    }

                    //return true;
                }
                else
                {
                    MessageBox.Show("Documento não é Modelo 65", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                #endregion
            }
            catch (Exception erro)
            {
                throw new Exception(erro.ToString());
            }
                
        }

       
        public bool Gerartransferencia(int documento)
        {
            //if (Configuracoes.gerarNFeVenda == false)
                //return true;

            var conexao = Conexao.CriarEntidade();
            string SQL = "";
            try
            {
                var itensTransf = conexao.ExecuteStoreQuery<transfvenda>("SELECT id,ip,DATA,operador,codigo,descricao,quantidade,preco,custo,filialdestino,filialorigem,numeroDav,documento,transferencia,cancelado FROM transfvenda WHERE documento = '" + documento.ToString() + "' AND cancelado = 'N' AND (SELECT nrnotafiscal FROM conttransf WHERE numero = transfvenda.transferencia) = 0 GROUP BY filialorigem").ToList();
                foreach (var item in itensTransf)
                {
                    SQL = "SELECT gerarNFeTransfVenda FROM configfinanc WHERE codigoFilial = '"+item.filialorigem+"'";
                    string gerarNFe = conexao.ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                    if (gerarNFe == "S")
                    {
                        SQL = "call montarNFeTransf('" + item.transferencia + "','" + item.ip + "','" + item.operador + "');";
                        var result = conexao.ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                        try
                        {
                            int.Parse(result);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(result.ToString(), "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }

            return true;
        }

    }



    public class itensNFe
    {
        public itensNFe()
        {
            sequencia = 0;
        }
        public Int64 documento { get; set; }
        public string coo { get; set; }
        public string ccf { get; set; }
        public string ecf { get; set; }
        public string seqECF  { get; set; }
        public int sequencia { get; set; }
        public string codigo { get; set; }
        public string descricao { get; set; }
        public decimal preco { get; set; }
        public decimal desconto { get; set; }
        public decimal quantidade { get; set; }
        public decimal total { get; set; }
        public decimal icms { get; set; }
        public string cfop { get; set; }
        public string csticms { get; set; }
        public decimal ipi { get; set; }
        public decimal icmsst { get; set; }
        public decimal redbcst { get; set; }
        public decimal redbc { get; set; }
        public decimal mva { get; set; }
        public string cstipi { get; set; }
        public string cstpis { get; set; }
        public decimal pis { get; set; }
        public string cstcofins { get; set; }
        public decimal cofins { get; set; }
        public string vendaAtacado { get; set; }
       
    }

    public class PagamentoNFe
    {
        public int parcela { get; set; }
        public string tipo { get; set; }
        public decimal valor { get; set; }
        public DateTime vencimento { get; set; }
    }

    public class infNFE
    {
        private emit _emit;
        public ide _ide;
        private dest _dest;
        private List<det> _det;

        public infNFE()
        {
            //Inicializa automaticamente apenas aqueles que são obrigatórios
            _emit = new emit();
            _dest = new dest();
            _ide = new ide();
            _det = new List<det>();
            /*_retirada = new retirada();
            _avulsa = new avulsa(); */
        }


        /*       Por enquanto não vou gerar o ID aqui... para que a Danfe possa ser impressa de imediato,
         *       o ERP deve gerar este ID
         * 
         * public string gerarChaveNFE() 
                {
            
                } */

        public ide Ide
        {
            get { return _ide; }
        }

        public emit Emit
        {
            get
            {
                return _emit;
            }
        }

        public avulsa Avulsa { get; set; }

        public total Total { get; set; }

        public cobr Cobr { get; set; }

        public infAdic InfAdic { get; set; }

        public dest Dest
        {
            get { return _dest; }
        }

        public List<det> Det
        {
            get { return _det; }
        }


        public retirada Retirada { get; set; }
        public entrega Entrega { get; set; }

        public transp Transp { get; set; }



        public class ide
        {
            public ide()
            {

            }

            /// <summary>
            /// Código da UF do emitente do Documento Fiscal. Utilizar a Tabela do IBGE de código de unidades da
            /// federação (Anexo IV - Tabela de UF, Município e País)
            /// </summary>            
            public int cUF { get; set; }
            /// <summary>
            /// Código numérico que compõe a Chave de Acesso. Número aleatório gerado pelo emitente para
            /// cada NF-e para evitar acessos indevidos da NF-e.
            /// </summary>
            public string cNF { get; set; }
            /// <summary>
            /// Informar a natureza da operação de que decorrer a saída ou a entrada, tais como: venda, compra,
            /// transferência, devolução, importação, consignação, remessa (para fins de demonstração, de
            /// industrialização ou outra), conforme previsto na alínea 'i', inciso I, art. 19
            /// do CONVÊNIO S/Nº, de 15 de dezembro de 1970
            /// </summary>
            public string natOp { get; set; }
            /// <summary>
            /// 0 – pagamento à vista;
            /// 1 – pagamento à prazo;
            /// 2 - outros.
            /// </summary>
            public string indPag { get; set; }
            /// <summary>
            /// Utilizar o código 55 para identificação da NF-e, emitida em substituição ao modelo 1 ou 1A.
            /// </summary>        
            public string mod { get; set; }
            /// <summary>
            /// Série do Documento Fiscal, informar 0 (zero) para série única.
            /// </summary>
            public string serie { get; set; }
            /// <summary>
            /// Número do Documento Fiscal.
            /// </summary>
            public string nNF { get; set; }
            /// <summary>
            /// Data de emissão do documento fiscal, Formato “AAAA-MM-DD”
            /// </summary>
            public DateTime dEmi { get; set; }
            /// <summary>
            /// Data de saída ou da entrada da mercadoria/produto,  Formato “AAAA-MM-DD”
            /// </summary>
            public DateTime dSaiEnt { get; set; }
            /// <summary>
            /// 0-entrada / 1-saída
            /// </summary>
            public int tpNF { get; set; }
            /// <summary>
            /// Informar o município de ocorrência do fato gerador do ICMS. Utilizar a 
            /// Tabela do IBGE (Anexo IV - Tabela de UF, Município e País).
            /// </summary>
            public int cMunFG { get; set; }
            /// <summary>
            /// Grupo com as informações das NF/NF-e referenciadas.
            /// </summary>   

            public int tpImp { get; set; }
            /// <summary>
            /// 1 – Normal – emissão normal;
            /// 2 – Contingência FS – emissão em contingência com impressão do DANFE em Formulário de Segurança;
            /// 3 – Contingência SCAN – emissão em contingência no Sistema de Contingência do Ambiente Nacional – SCAN;
            /// 4 – Contingência DPEC - emissão em contingência com envio da Declaração Prévia de Emissão em Contingência – DPEC;
            /// 5 – Contingência FS-DA - emissão em contingência com impressão do DANFE em Formulário de Segurança para Impressão de
            /// Documento Auxiliar de Documento Fiscal Eletrônico (FS-DA).
            /// </summary>
            public string tpEmis { get; set; }
            /// <summary>
            /// Informar o DV da Chave de Acesso da NF-e, o DV será calculado com a aplicação do algoritmo módulo 11
            /// (base 2,9) da Chave de Acesso. (vide item 5 do Manual de Integração)
            /// </summary>
            public string cDV { get; set; }
            /// <summary>
            /// 1-Produção/ 2-Homologação
            /// </summary>
            public int tpAmb { get; set; }
            /// <summary>
            /// 1- NF-e normal/ 2-NF-e complementar / 3 – NF-e de ajuste
            /// </summary>            
            public string finNFe { get; set; }
            /// <summary>
            /// Identificador do processo de emissão da NF-e: 0 - emissão de NF-e com aplicativo do contribuinte;
            /// 1 - emissão de NF-e avulsa pelo Fisco;
            /// 2 - emissão de NF-e avulsa, pelo contribuinte com seu certificado digital, através do site do Fisco;
            /// 3- emissão NF-e pelo contribuinte com aplicativo fornecido pelo Fisco.
            /// </summary>
            public string procEmi { get; set; }
            /// <summary>
            /// Identificador da versão do processo de emissão (informar a versão do aplicativo emissor de NF-e).
            /// </summary>
            public string verProc { get; set; }

            public NFref NFRef { get; set; }

            /// <summary>
            /// Grupo com as informações das NF/NF-e referenciadas
            /// </summary>
            public class NFref
            {
                private NFref _nfRef;
                private refNF _RefNF;
                /// <summary>
                /// Utilizar esta TAG para referenciar uma Nota Fiscal Eletrônica emitida
                /// anteriormente, vinculada a NF-e atual.
                /// Esta informação será utilizada nas hipóteses previstas na legislação.
                /// (Ex.: Devolução de Mercadorias, Substituição de NF cancelada, Complementação de NF, etc.).
                /// </summary>

                public NFref()
                {
                    _RefNF = new refNF();
                    _nfRef = new NFref();
                }

                public string refNFE { get; set; }

                public NFref nfRef
                {
                    get
                    {
                        return _nfRef;
                    }
                }

                public refNF RefNF
                {
                    get
                    {
                        return _RefNF;
                    }
                }
                /// <summary>
                /// Grupo com as informações das NF referenciadas Idem a informação da TAG anterior, referenciando 
                /// uma Nota Fiscal modelo 1/1A normal (a NF referenciada não é uma NF-e).
                /// </summary>
                public class refNF
                {
                    /// <summary>
                    /// Utilizar a Tabela do IBGE (Anexo IV - Tabela de UF, Município e País)
                    /// </summary>
                    public int cUF { get; set; }
                    /// <summary>
                    /// AAMM da emissão da NF
                    /// </summary>
                    public string AAMM { get; set; }
                    /// <summary>
                    /// Informar o CNPJ do emitente da NF
                    /// </summary>
                    public string CNPJ { get; set; }
                    /// <summary>
                    /// Informar o código do modelo do Documento fiscal: 01 – modelo 01
                    /// </summary>
                    public string mod { get; set; }
                    /// <summary>
                    /// Informar a série do documento fiscal (informar zero se inexistente).
                    /// </summary>
                    public string serie { get; set; }
                    /// <summary>
                    /// 1 – 999999999
                    /// </summary>
                    public string nNF { get; set; }
                    /// <summary>
                    /// 1-Retrato/ 2-Paisagem
                    /// </summary>
                }
            }
        }

        /// <summary>
        /// Grupo com as informações do emitente da NF-e
        /// </summary>
        public class emit
        {
            public emit()
            {
                _enderEmit = new enderEmit();
            }

            private enderEmit _enderEmit;

            /// <summary>
            /// Informar o CNPJ do emitente. Em se tratando de emissão de NF-e avulsa pelo Fisco, as informações
            /// do remente serão informadas neste grupo. O CNPJ ou CPF deverão ser informados com os zeros não
            /// significativos.
            /// </summary>
            public string CNPJ { get; set; }
            /// <summary>
            /// Informar o CNPJ do emitente. Em se tratando de emissão de NF-e avulsa pelo Fisco, as informações
            /// do remente serão informadas neste grupo. O CNPJ ou CPF deverão ser informados com os zeros não
            /// significativos.
            /// </summary>
            public string CPF { get; set; }
            /// <summary>
            /// Razão social ou nome do emitente
            /// </summary>
            public string xNome { get; set; }
            /// <summary>
            /// Nome fantasia
            /// </summary>
            public string xFant { get; set; }
            /// <summary>
            /// TAG de grupo do Endereço do emitente
            /// </summary>
            public enderEmit EnderEmit
            {
                get
                {
                    return _enderEmit;
                }
            }

            /// <summary>
            /// Campo de informação obrigatória nos casos de emissão própria (procEmi = 0, 2 ou 3).
            /// A IE deve ser informada apenas com algarismos para destinatários contribuintes do ICMS, sem
            /// caracteres de formatação (ponto,barra, hífen, etc.);
            /// O literal “ISENTO” deve ser informado apenas para contribuintes do ICMS que são isentos de inscrição no cadastro de
            /// contribuintes do ICMS e estejam emitindo NF-e avulsa;
            /// </summary>
            [Obrigatorio]
            public string IE { get; set; }
            /// <summary>
            /// Informar a IE do ST da UF de destino da mercadoria, quando houver a retenção do ICMS ST para a UF de destino.
            /// </summary>
            public string CRT { get; set; }
            public string IEST { get; set; }
            /// <summary>
            /// Este campo deve ser informado, quando ocorrer a emissão de NF-e conjugada, com prestação de
            /// serviços sujeitos ao ISSQN e fornecimento de peças sujeitos ao ICMS.
            /// </summary>
            public string IM { get; set; }
            /// <summary>
            /// Este campo deve ser informado quando o campo IM (C19) for informado.
            /// </summary>
            public string CNAE { get; set; }

            public class enderEmit
            {
                /// <summary>
                /// Logradouro
                /// </summary>
                public string xLgr { get; set; }
                /// <summary>
                /// Número
                /// </summary>
                public string nro { get; set; }
                /// <summary>
                /// Complemento
                /// </summary>
                public string xCpl { get; set; }
                /// <summary>
                /// Bairro
                /// </summary>
                public string xBairro { get; set; }
                /// <summary>
                /// Código do municipio, Utilizar a Tabela do IBGE (Anexo IV - Tabela de UF, Município e País).
                /// Informar ‘9999999 ‘para operações com o exterior.
                /// </summary>
                public int cMun { get; set; }
                /// <summary>
                /// Informar ‘EXTERIOR ‘para operações com o exterior.
                /// </summary>
                public string xMun { get; set; }
                /// <summary>
                /// Informar ‘EX ‘para operações com o exterior.
                /// </summary>
                public string UF { get; set; }
                /// <summary>
                /// Informar os zeros não significativos.
                /// </summary>
                public string CEP { get; set; }
                /// <summary>
                /// 1058 - Brasil
                /// </summary>
                public int cPais { get; set; }
                /// <summary>
                /// Brasil ou BRASIL
                /// </summary>
                public string xPais { get; set; }
                /// <summary>
                /// Preencher com Código DDD + número do telefone.
                /// </summary>
                public string fone { get; set; }
            }
        }

        /// <summary>
        /// Informações do fisco emitente, grupo de uso exclusivo do fisco.
        /// </summary>
        public class avulsa
        {
            public avulsa()
            {

            }

            /// <summary>
            /// CNPJ do órgão emitente
            /// </summary>
            public string CNPJ { get; set; }
            /// <summary>
            /// Órgão emitente
            /// </summary>
            public string xOrgao { get; set; }
            /// <summary>
            /// Matrícula do agente
            /// </summary>
            public string matr { get; set; }
            /// <summary>
            /// Nome do agente
            /// </summary>
            public string xAgente { get; set; }
            /// <summary>
            /// Preencher com Código DDD + número do telefone
            /// </summary>
            public string fone { get; set; }
            /// <summary>
            /// Sigla da UF
            /// </summary>
            public string UF { get; set; }
            /// <summary>
            /// Número do Documento de Arrecadação de Receita
            /// </summary>
            public string nDAR { get; set; }
            /// <summary>
            /// Data de emissão do Documento de Arrecadação
            /// </summary>
            public DateTime dEmi { get; set; }
            /// <summary>
            /// Valor Total constante no Documento de arrecadação de Receita
            /// </summary>
            public double vDAR { get; set; }
            /// <summary>
            /// Repartição Fiscal emitente
            /// </summary>
            public string repEmi { get; set; }
            /// <summary>
            /// Data de pagamento do Documento de Arrecadação
            /// </summary>
            public DateTime dPag { get; set; }
        }

        /// <summary>
        /// Grupo com as informações do destinatário da NF-e.
        /// </summary>
        public class dest
        {
            private enderDest _enderDest;
            public dest()
            {
                _enderDest = new enderDest();
            }
            /// <summary>
            /// Informar o CNPJ ou o CPF do destinatário, preenchendo os zeros não significativos.
            /// Não informar o conteúdo da TAG se a operação for realizada com o exterior.
            /// </summary>
            public string CNPJ { get; set; }
            /// <summary>
            /// Informar o CNPJ ou o CPF do destinatário, preenchendo os zeros não significativos.
            /// Não informar o conteúdo da TAG se a operação for realizada com o exterior.
            /// </summary>
            public string CPF { get; set; }
            /// <summary>
            /// Razão Social ou nome do destinatário
            /// </summary>
            public string xNome { get; set; }
            /// <summary>
            /// Obrigatório, nas operações que se beneficiam de incentivos fiscais existentes nas áreas sob controle
            /// da SUFRAMA. A omissão da Inscrição SUFRAMA impede o processamento da operação pelo Sistema de
            /// Mercadoria Nacional da SUFRAMA e a liberação da Declaração de Ingresso, prejudicando a
            /// comprovação do ingresso/internamento da mercadoria nas áreas sob controle da SUFRAMA.
            /// </summary>
            public string ISUF { get; set; }
            public enderDest EnderDest
            {
                get
                {
                    return _enderDest;
                }
            }
            /// <summary>
            /// Informar a IE quando o destinatário for contribuinto do ICMS. Informar ISENTO quando o
            /// destinatário for contribuinto do ICMS, mas não estiver obrigado à inscrição no cadastro de
            /// contribuintes do ICMS. Não informar o conteúdo da TAG se o destinatário não for contribuinte do ICMS.
            /// Esta tag aceita apenas: . ausência de conteúdo (<IE></IE> ou <IE/>) para destinatários não
            /// contribuintes do ICMS; . algarismos para destinatários contribuintes do ICMS, sem caracteres de formatação (ponto,
            /// barra, hífen, etc.); . literal “ISENTO” para destinatários contribuintes do ICMS que são isentos de inscrição no cadastro de
            /// contribuintes do ICMS;
            /// </summary>
            [Obrigatorio]
            public string IE { get; set; }
            public class enderDest
            {
                /// <summary>
                /// Logradouro
                /// </summary>
                public string xLgr { get; set; }
                /// <summary>
                /// Número
                /// </summary>
                public string nro { get; set; }
                /// <summary>
                /// Complemento
                /// </summary>
                public string xCpl { get; set; }
                /// <summary>
                /// Bairro
                /// </summary>
                public string xBairro { get; set; }
                /// <summary>
                /// Código do municipio, Utilizar a Tabela do IBGE (Anexo IV - Tabela de UF, Município e País).
                /// Informar ‘9999999 ‘para operações com o exterior.
                /// </summary>
                public int cMun { get; set; }
                /// <summary>
                /// Informar ‘EXTERIOR ‘para operações com o exterior.
                /// </summary>
                public string xMun { get; set; }
                /// <summary>
                /// Informar ‘EX ‘para operações com o exterior.
                /// </summary>
                public string UF { get; set; }
                /// <summary>
                /// Informar os zeros não significativos.
                /// </summary>
                public string CEP { get; set; }
                /// <summary>
                /// 1058 - Brasil
                /// </summary>
                public int cPais { get; set; }
                /// <summary>
                /// Brasil ou BRASIL
                /// </summary>
                public string xPais { get; set; }
                /// <summary>
                /// Preencher com Código DDD + número do telefone.
                /// </summary>
                public string fone { get; set; }
            }

        }

        /// <summary>
        /// Informar apenas quando for diferente do endereço do remetente.
        /// </summary>
        public class retirada
        {
            public string CNPJ { get; set; }
            public string xLgr { get; set; }
            public string nro { get; set; }
            public string xCpl { get; set; }
            public string xBairro { get; set; }
            /// <summary>
            /// Utilizar a Tabela do IBGE (Anexo IV - Tabela de UF, Município e País). Informar ‘9999999 ‘para operações
            /// com o exterior.
            /// </summary>
            public int cMun { get; set; }
            public string xMun { get; set; }
            public string UF { get; set; }
        }

        public class entrega
        {
            public string CNPJ { get; set; }
            public string xLgr { get; set; }
            public string nro { get; set; }
            public string xCpl { get; set; }
            public string xBairro { get; set; }
            /// <summary>
            /// Utilizar a Tabela do IBGE (Anexo IV - Tabela de UF, Município e País). Informar ‘9999999 ‘para operações
            /// com o exterior.
            /// </summary>
            public int cMun { get; set; }
            public string xMun { get; set; }
            public string UF { get; set; }
        }

        /// <summary>
        /// H01
        /// </summary>
        public class det
        {
            private prod _prod;
            private imposto _imposto;
            public det()
            {
                _prod = new prod();
                _imposto = new imposto();
            }

            public int nItem { get; set; }
            public prod Prod
            {
                get { return _prod; }
            }

            public imposto Imposto
            {
                get { return _imposto; }
            }

            /// <summary>
            /// TAG de grupo do detalhamento de Produtos e Serviços da NF-e
            /// </summary>
            public class prod
            {
                public prod()
                {
                    //vDesc = null;
                }
                /// <summary>
                /// Código de produto ou serviço
                /// Preencher com CFOP, caso se trate de itens não relacionados com mercadorias/produto e que o
                /// contribuinte não possua codificação própria.
                /// Formato ”CFOP9999”
                /// </summary>


                public string cProd { get; set; }
                /// <summary>
                /// GTIN (Global Trade Item Number) do produto, antigo código EAN ou código de barras
                /// 
                /// Preencher com o código GTIN-8, GTIN-12, GTIN-13 ou GTIN-14 
                /// (antigos códigos EAN, UPC e DUN-14), não informar o conteúdo da TAG em caso de o produto não
                /// possuir este código.
                /// </summary>

                [Obrigatorio]
                public string cEAN { get; set; }
                /// <summary>
                /// Descrição do produto ou serviço
                /// </summary>
                public string xProd { get; set; }
                /// <summary>
                /// Preencher de acordo com a Tabela de Capítulos da NCM. Em caso deserviço, não incluir a TAG.
                /// </summary>
                public string NCM { get; set; }
                /// <summary>
                /// Preencher de acordo com o código EX da TIPI. Em caso de serviço,não incluir a TAG
                /// </summary>
                public string EXTIPI { get; set; }
                /// <summary>
                /// Gênero do produto ou serviço. Preencher de acordo com a Tabela de Capítulos da NCM. Em caso de serviço, não incluir a TAG.
                /// </summary>
                public string genero { get; set; }
                /// <summary>
                /// Utilizar Tabela de CFOP.
                /// </summary>
                public string CFOP { get; set; }
                /// <summary>
                /// Informar a unidade de comercialização do produto.
                /// </summary>
                public string uCom { get; set; }
                /// <summary>
                /// Informar a quantidade de comercialização do produto
                /// </summary>
                //[Formato("N4", "pt-BR")]
                [Formato("#####0.0000", "en-US")]
                public decimal qCom { get; set; }
                /// <summary>
                /// Informar o valor unitário de comercialização do produto
                /// </summary>
                //[Formato("N4", "pt-BR")]
                [Formato("#####0.0000", "en-US")]
                public double vUnCom { get; set; }
                /// <summary>
                /// Valor Total Bruto dos Produtos ou Serviços
                /// </summary>
                public double vProd { get; set; }
                /// <summary>
                /// GTIN (Global Trade Item Number) da unidade tributável,antigo código EAN ou código de barras
                /// 
                /// Preencher com o código GTIN-8, GTIN-12, GTIN-13 ou GTIN-14 (antigos códigos EAN, UPC e DUN-14) da unidade tributável do
                /// produto, não informar o conteúdo da TAG em caso de o produto não possuir este código.
                /// </summary>                
                [Obrigatorio]
                public string cEANTrib { get; set; }
                /// <summary>
                /// Unidade Tributável
                /// </summary>
                public string uTrib { get; set; }
                /// <summary>
                /// Quantidade Tributável
                /// </summary>                
                [Formato("#####0.0000", "en-US")]
                public decimal? qTrib { get; set; }
                /// <summary>
                /// Valor Unitário de tributação
                /// 
                /// Informar o valor unitário de tributação do produto
                /// </summary>                
                [Formato("#####0.0000", "en-US")]
                public double? vUnTrib { get; set; }
                /// <summary>
                /// Valor total do frete
                /// </summary>
                public double? vFrete { get; set; }
                /// <summary>
                /// Valor total do seguro
                /// </summary>
                public double? vSeg { get; set; }
                /// <summary>
                /// Valor do desconto
                /// </summary>
                public double? vDesc { get; set; }

                public double? vOutro { get; set; }

                public string indTot { get; set; }

                public DI Di { get; set; }
                public veicProd VeicProd { get; set; }
                public med Med { get; set; }
                public arma Arma { get; set; }
                public comb Comb { get; set; }

                /// <summary>
                /// Tag da Declaração de Importação
                /// </summary>
                public class DI
                {
                    private List<adi> _adi;
                    public DI()
                    {
                        _adi = new List<adi>();
                    }

                    /// <summary>
                    /// Número do Documento de Importação DI/DSI/DA (DI/DSI/DA)
                    /// </summary>
                    public string nDI { get; set; }
                    /// <summary>
                    /// Data de Registro da DI/DSI/DA
                    /// Formato “AAAA-MM-DD”
                    /// </summary>
                    public DateTime dDI { get; set; }
                    /// <summary>
                    /// Local de desembaraço
                    /// </summary>
                    public string xLocDesemb { get; set; }
                    /// <summary>
                    /// Sigla da UF onde ocorreu o Desembaraço Aduaneiro
                    /// </summary>
                    public string UFDesemb { get; set; }
                    /// <summary>
                    /// Data do Desembaraço Aduaneiro
                    /// </summary>
                    public DateTime dDesemb { get; set; }
                    /// <summary>
                    /// Código do exportador, usado nos sistemas internos de informação do emitente da NF-e
                    /// </summary>
                    public string cExportador { get; set; }

                    /// <summary>
                    /// Adições
                    /// </summary>
                    public List<adi> Adi
                    {
                        get { return _adi; }
                    }

                    public class adi
                    {
                        /// <summary>
                        /// Numero da adição
                        /// </summary>
                        public string nAdicao { get; set; }
                        /// <summary>
                        /// Numero seqüencial do item dentro da adição
                        /// </summary>
                        public string nSeqAdic { get; set; }
                        /// <summary>
                        /// Código do fabricante estrangeiro, usado nos sistemas internos de
                        /// informação do emitente da NF-e
                        /// </summary>
                        public string cFabricante { get; set; }
                        /// <summary>
                        /// Valor do desconto do item da DI – adição
                        /// </summary>
                        public double vDescDI { get; set; }
                    }
                }

                /// <summary>
                /// TAG de grupo do detalhamento de Veículos novos
                /// 
                /// Informar apenas quando se tratar de veículos novos
                /// </summary>
                public class veicProd
                {
                    /// <summary>
                    /// 1 – Venda concessionária,
                    /// 2 – Faturamento direto
                    /// 3 – Venda direta
                    /// 0 – Outros
                    /// </summary>
                    public int tpOp { get; set; }
                    /// <summary>
                    /// Chassi do veículo
                    /// </summary>
                    public string chassi { get; set; }
                    /// <summary>
                    /// Código de cada montadora
                    /// </summary>
                    public string cCor { get; set; }
                    /// <summary>
                    /// Descrição da Cor
                    /// </summary>
                    public string xCor { get; set; }
                    /// <summary>
                    /// Potência Motor
                    /// </summary>
                    public string pot { get; set; }
                    /// <summary>
                    /// CM3 (Potência)
                    /// </summary>
                    public string CM3 { get; set; }
                    /// <summary>
                    /// Peso Líquido
                    /// </summary>
                    public double pesoL { get; set; }
                    /// <summary>
                    /// Peso bruto
                    /// </summary>
                    public double pesoB { get; set; }
                    /// <summary>
                    /// Serial (série)
                    /// </summary>
                    public string nSerie { get; set; }
                    /// <summary>
                    /// Tipo de combustível
                    /// </summary>
                    public string tpComb { get; set; }
                    /// <summary>
                    /// Numero do motor
                    /// </summary>
                    public string nMotor { get; set; }
                    public string CMKG { get; set; }
                    /// <summary>
                    /// Distância entre eixos
                    /// </summary>
                    public string dist { get; set; }
                    /// <summary>
                    /// RENAVAM
                    /// </summary>
                    public string RENAVAM { get; set; }
                    /// <summary>
                    /// Ano Modelo de Fabricação
                    /// </summary>
                    public string anoMod { get; set; }
                    /// <summary>
                    /// Ano de fabricação
                    /// </summary>
                    public string anoFab { get; set; }
                    /// <summary>
                    /// Tipo de pintura
                    /// </summary>
                    public string tpPint { get; set; }
                    /// <summary>
                    /// Tipo de veiculo
                    /// </summary>
                    public string tpVeic { get; set; }
                    /// <summary>
                    /// Espécie do veiculo
                    /// </summary>
                    public string espVeic { get; set; }
                    /// <summary>
                    /// Condição do VIN
                    /// </summary>
                    public string VIN { get; set; }
                    /// <summary>
                    /// Condição do veiculo
                    /// </summary>
                    public string condVeic { get; set; }
                    /// <summary>
                    /// Código Marca Modelo
                    /// </summary>
                    public string cMod { get; set; }
                }

                /// <summary>
                /// Informar apenas quando se tratar de medicamentos, permite múltiplas ocorrências (ilimitado)
                /// </summary>
                public class med
                {
                    /// <summary>
                    /// Número do Lote do medicamento
                    /// </summary>
                    public string nLote { get; set; }
                    /// <summary>
                    /// Quantidade de produto no Lote do medicamento
                    /// </summary>
                    [Formato("N0", "pt-BR")]
                    public decimal qLote { get; set; }
                    /// <summary>
                    /// Data de fabricação
                    /// </summary>
                    public DateTime dFab { get; set; }
                    /// <summary>
                    /// Data de validade
                    /// </summary>
                    public DateTime dVal { get; set; }
                    /// <summary>
                    /// Preço máximo consumidor
                    /// </summary>
                    public decimal vPMC { get; set; }
                }

                /// <summary>
                /// Informar apenas quando se tratar de armamento, permite múltiplas
                /// ocorrências (ilimitado)
                /// </summary>
                public class arma
                {
                    /// <summary>
                    /// Indicador do tipo de arma de fogo
                    /// 
                    /// 0 - Uso permitido;
                    /// 1 - Uso restrito;
                    /// </summary>
                    public string tpArma { get; set; }
                    /// <summary>
                    /// Número de série da arma
                    /// </summary>
                    public string nSerie { get; set; }
                    /// <summary>
                    /// Número de série do cano
                    /// </summary>
                    public string nCano { get; set; }
                    /// <summary>
                    /// Descrição completa da arma, compreendendo: calibre, marca, capacidade, tipo de
                    /// funcionamento, comprimento e demais elementos que permitam a sua perfeita identificação.
                    /// </summary>
                    public string descr { get; set; }
                }


                /// <summary>
                /// TAG de grupo de informações específicas para combustíveis líquidos
                /// </summary>
                public class comb
                {
                    private CIDE _CIDE;
                    public comb()
                    {
                        _CIDE = new CIDE();
                    }

                    public CIDE Cide
                    {
                        get { return _CIDE; }
                    }

                    /// <summary>
                    /// Informar apenas quando se tratar de produtos regulados pela ANP -Agência Nacional do Petróleo.
                    /// Utilizar a codificação de produtos do Sistema de Informações de Movimentação de produtos - SIMP
                    /// (http://www.anp.gov.br/simp/index.htm)
                    /// </summary>
                    public string cProdANP { get; set; }
                    /// <summary>
                    /// Informar apenas quando a UF utilizar o CODIF (Sistema de Controle do Diferimento do Imposto nas Operações com AEAC - Álcool
                    /// Etílico Anidro Combustível).
                    /// </summary>
                    public string CODIF { get; set; }
                    /// <summary>
                    /// Informar quando a quantidade faturada informada no campo qCom (I10) tiver sido ajustada para
                    /// uma temperatura diferente da ambiente.
                    /// </summary>
                    public string qTemp { get; set; }

                    public class CIDE
                    {
                        public decimal qBCProd { get; set; }
                        public double vAliqProd { get; set; }
                        public double vCIDE { get; set; }
                    }

                    public class ICMSComb
                    {
                        public double vBCICMS { get; set; }
                        public double vICMS { get; set; }
                        public double vBCICMSST { get; set; }
                        public double vICMSST { get; set; }
                    }

                    public class ICMSInter
                    {
                        public double vBCICMSSTDest { get; set; }
                        public double vICMSSTDest { get; set; }
                    }

                    public class ICMSCons
                    {
                        public double vBCICMSSTCons { get; set; }
                        public double vICMSSTCons { get; set; }
                        public string UFcons { get; set; }
                    }





                }
            }

            public class imposto
            {
                public imposto()
                {
                    //Só instância aqueles que são obritórios e utilizados sempre...                    
                }

                public ICMS Icms { get; set; }

                public IPI Ipi { get; set; }

                public II Ii { get; set; }

                public PIS Pis { get; set; }
                public PISST PisST { get; set; }

                public COFINS Cofins { get; set; }
                public COFINSST CofinsST { get; set; }

                public ISSQN Issqn { get; set; }

                /// <summary>
                /// Informar apenas um dos grupos N02, N03, N04, N05, N06, N07, N08, N09 ou N10, com base no
                /// conteúdo informado na TAG CST.
                /// </summary>
                public class ICMS
                {
                    public ICMS()
                    {

                    }

                    public ICMS00 Icms00 { get; set; }
                    public ICMS10 Icms10 { get; set; }
                    public ICMS20 Icms20 { get; set; }
                    public ICMS30 Icms30 { get; set; }
                    public ICMS40 Icms40 { get; set; }
                    public ICMS51 Icms51 { get; set; }
                    public ICMS60 Icms60 { get; set; }
                    public ICMS70 Icms70 { get; set; }
                    public ICMS90 Icms90 { get; set; }

                    public class ICMS00
                    {
                        /// <summary>
                        /// Origem da mercadoria: 
                        /// 0 – Nacional; 
                        /// 1 – Estrangeira – Importação direta;
                        /// 2 – Estrangeira – Adquirida no mercado interno.
                        /// </summary>
                        public string orig { get; set; }
                        /// <summary>
                        /// Tributação do ICMS: 00 – Tributada integralmente
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// 0 - Margem Valor Agregado (%);
                        /// 1 - Pauta (Valor);
                        /// 2 - Preço Tabelado Máx. (valor);
                        /// 3 - valor da operação.
                        /// </summary>
                        public string modBC { get; set; }
                        /// <summary>
                        /// Valor da base de calculo do imposto
                        /// </summary>
                        public double vBC { get; set; }
                        /// <summary>
                        /// Aliquota do imposto
                        /// </summary>
                        public double pICMS { get; set; }
                        /// <summary>
                        /// Valor do ICMS
                        /// </summary>
                        public double vICMS { get; set; }
                    }

                    public class ICMS10
                    {
                        /// <summary>
                        /// Origem da mercadoria: 
                        /// 0 – Nacional; 
                        /// 1 – Estrangeira – Importação direta;
                        /// 2 – Estrangeira – Adquirida no mercado interno.
                        /// </summary>
                        public string orig { get; set; }
                        /// <summary>
                        /// Tributação do ICMS: 00 – Tributada integralmente
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// 0 - Margem Valor Agregado (%);
                        /// 1 - Pauta (Valor);
                        /// 2 - Preço Tabelado Máx. (valor);
                        /// 3 - valor da operação.
                        /// </summary>
                        public string modBC { get; set; }
                        /// <summary>
                        /// Valor da base de calculo do imposto
                        /// </summary>
                        public double vBC { get; set; }
                        /// <summary>
                        /// Aliquota do imposto
                        /// </summary>
                        public double pICMS { get; set; }
                        /// <summary>
                        /// Valor do ICMS
                        /// </summary>
                        public double vICMS { get; set; }
                        /// <summary>
                        /// Modalidade de determinação da BC do ICMS ST
                        /// 0 – Preço tabelado ou máximo sugerido;
                        /// 1 - Lista Negativa (valor);
                        /// 2 - Lista Positiva (valor);
                        /// 3 - Lista Neutra (valor);
                        /// 4 - Margem Valor Agregado (%);
                        /// 5 - Pauta (valor);
                        /// </summary>
                        public string modBCST { get; set; }
                        /// <summary>
                        /// Percentual da margem de valor Adicionado do ICMS ST
                        /// </summary>
                        public double? pMVAST { get; set; }
                        /// <summary>
                        /// Percentual da Redução de BC do ICMS ST
                        /// </summary>                    
                        public double? pRedBCST { get; set; }
                        /// <summary>
                        /// Valor do ICMS ST
                        /// </summary>
                        public double vBCST { get; set; }
                        /// <summary>
                        /// Alíquota do imposto do ICMS ST
                        /// </summary>
                        public double pICMSST { get; set; }
                        /// <summary>
                        /// Valor do ICMS ST retido
                        /// </summary>
                        public double vICMSST { get; set; }
                    }

                    /// <summary>
                    /// CST – 20 - Com redução de base de cálculo
                    /// </summary>
                    public class ICMS20
                    {
                        /// <summary>
                        /// Origem da mercadoria: 
                        /// 0 – Nacional; 
                        /// 1 – Estrangeira – Importação direta;
                        /// 2 – Estrangeira – Adquirida no mercado interno.
                        /// </summary>
                        public string orig { get; set; }
                        /// <summary>
                        /// Tributação do ICMS: 00 – Tributada integralmente
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// 0 - Margem Valor Agregado (%);
                        /// 1 - Pauta (Valor);
                        /// 2 - Preço Tabelado Máx. (valor);
                        /// 3 - valor da operação.
                        /// </summary>
                        public string modBC { get; set; }

                        /// <summary>
                        /// Percentual da Redução de BC do ICMS
                        /// </summary>                    
                        public double pRedBC { get; set; }

                        /// <summary>
                        /// Valor da base de calculo do imposto
                        /// </summary>
                        public double vBC { get; set; }

                        /// <summary>
                        /// Aliquota do imposto
                        /// </summary>
                        public double pICMS { get; set; }
                        /// <summary>
                        /// Valor do ICMS
                        /// </summary>
                        public double vICMS { get; set; }
                    }

                    /// <summary>
                    /// CST – 30 - Isenta ou não tributada e com cobrança do ICMS por
                    /// substituição tributária
                    /// </summary>
                    public class ICMS30
                    {
                        /// <summary>
                        /// Origem da mercadoria: 
                        /// 0 – Nacional; 
                        /// 1 – Estrangeira – Importação direta;
                        /// 2 – Estrangeira – Adquirida no mercado interno.
                        /// </summary>
                        public string orig { get; set; }
                        /// <summary>
                        /// Tributação do ICMS: 00 – Tributada integralmente
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// 0 - Margem Valor Agregado (%);
                        /// 1 - Pauta (Valor);
                        /// 2 - Preço Tabelado Máx. (valor);
                        /// 3 - valor da operação.
                        /// </summary>
                        public string modBCST { get; set; }
                        /// <summary>
                        /// Valor da base de calculo do imposto
                        /// </summary>
                        public double? pMVAST { get; set; }
                        /// <summary>
                        /// Aliquota do imposto
                        /// </summary>
                        /// <summary>
                        /// Percentual da Redução de BC do ICMS ST
                        /// </summary>                    
                        public double? pRedBCST { get; set; }
                        /// <summary>
                        /// Valor do ICMS ST
                        /// </summary>
                        public double vBCST { get; set; }
                        /// <summary>
                        /// Alíquota do imposto do ICMS ST
                        /// </summary>
                        public double pICMSST { get; set; }
                        /// <summary>
                        /// Valor do ICMS ST retido
                        /// </summary>
                        public double vICMSST { get; set; }
                    }
                    /// <summary>
                    /// CST 
                    /// 40 - Isenta
                    /// 41 - Não tributada
                    /// 50 - Suspensão
                    /// </summary>
                    public class ICMS40
                    {
                        /// <summary>
                        /// Origem da mercadoria: 
                        /// 0 – Nacional; 
                        /// 1 – Estrangeira – Importação direta;
                        /// 2 – Estrangeira – Adquirida no mercado interno.
                        /// </summary>
                        public string orig { get; set; }
                        /// <summary>
                        /// Tributação do ICMS: 00 – Tributada integralmente
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// 0 - Margem Valor Agregado (%);
                        /// 1 - Pauta (Valor);
                        /// 2 - Preço Tabelado Máx. (valor);
                        /// 3 - valor da operação.
                        /// </summary>
                    }

                    public class ICMS51
                    {
                        /// <summary>
                        /// Origem da mercadoria: 
                        /// 0 – Nacional; 
                        /// 1 – Estrangeira – Importação direta;
                        /// 2 – Estrangeira – Adquirida no mercado interno.
                        /// </summary>
                        public string orig { get; set; }
                        /// <summary>
                        /// Tributação do ICMS: 00 – Tributada integralmente
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// 0 - Margem Valor Agregado (%);
                        /// 1 - Pauta (Valor);
                        /// 2 - Preço Tabelado Máx. (valor);
                        /// 3 - valor da operação.
                        /// </summary>
                        public string modBC { get; set; }

                        /// <summary>
                        /// Percentual da Redução de BC do ICMS
                        /// </summary>                    
                        public double pRedBC { get; set; }

                        /// <summary>
                        /// Valor da base de calculo do imposto
                        /// </summary>
                        public double vBC { get; set; }
                        /// <summary>
                        /// Aliquota do imposto
                        /// </summary>
                        public double pICMS { get; set; }
                        /// <summary>
                        /// Valor do ICMS
                        /// </summary>
                        public double vICMS { get; set; }
                        /// <summary>
                        /// Modalidade de determinação da BC do ICMS ST
                        /// 0 – Preço tabelado ou máximo sugerido;
                        /// 1 - Lista Negativa (valor);
                        /// 2 - Lista Positiva (valor);
                        /// 3 - Lista Neutra (valor);
                        /// 4 - Margem Valor Agregado (%);
                        /// 5 - Pauta (valor);
                        /// </summary>


                    }

                    /// <summary>
                    /// CST – 60 - ICMS cobrado anteriormente por substituição tributária
                    /// </summary>
                    public class ICMS60
                    {
                        /// <summary>
                        /// Origem da mercadoria: 
                        /// 0 – Nacional; 
                        /// 1 – Estrangeira – Importação direta;
                        /// 2 – Estrangeira – Adquirida no mercado interno.
                        /// </summary>
                        public string orig { get; set; }
                        /// <summary>
                        /// Tributação do ICMS: 00 – Tributada integralmente
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// Valor da base de calculo
                        /// </summary>
                        public double vBCST { get; set; }
                        /// <summary>
                        /// Valor do ICMS ST cobrado anteriormente por ST
                        /// </summary>
                        public double vICMSST { get; set; }
                    }

                    public class ICMS70
                    {
                        /// <summary>
                        /// Origem da mercadoria: 
                        /// 0 – Nacional; 
                        /// 1 – Estrangeira – Importação direta;
                        /// 2 – Estrangeira – Adquirida no mercado interno.
                        /// </summary>
                        public string orig { get; set; }
                        /// <summary>
                        /// Tributação do ICMS: 00 – Tributada integralmente
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// 0 - Margem Valor Agregado (%);
                        /// 1 - Pauta (Valor);
                        /// 2 - Preço Tabelado Máx. (valor);
                        /// 3 - valor da operação.
                        /// </summary>
                        public string modBC { get; set; }
                        /// <summary>
                        /// Percentual da Redução de BC
                        /// </summary>
                        public double pRedBC { get; set; }
                        /// <summary>
                        /// Valor da base de calculo do imposto
                        /// </summary>
                        public double vBC { get; set; }
                        /// <summary>
                        /// Aliquota do imposto
                        /// </summary>
                        public double pICMS { get; set; }
                        /// <summary>
                        /// Valor do ICMS
                        /// </summary>
                        public double vICMS { get; set; }
                        /// <summary>
                        /// Modalidade de determinação da BC do ICMS ST
                        /// 0 – Preço tabelado ou máximo sugerido;
                        /// 1 - Lista Negativa (valor);
                        /// 2 - Lista Positiva (valor);
                        /// 3 - Lista Neutra (valor);
                        /// 4 - Margem Valor Agregado (%);
                        /// 5 - Pauta (valor);
                        /// </summary>
                        public string modBCST { get; set; }
                        /// <summary>
                        /// Percentual da margem de valor Adicionado do ICMS ST
                        /// </summary>
                        public double? pMVAST { get; set; }
                        /// <summary>
                        /// Percentual da Redução de BC do ICMS ST
                        /// </summary>                    
                        public double? pRedBCST { get; set; }
                        /// <summary>
                        /// Valor do ICMS ST
                        /// </summary>
                        public double vBCST { get; set; }
                        /// <summary>
                        /// Alíquota do imposto do ICMS ST
                        /// </summary>
                        public double pICMSST { get; set; }
                        /// <summary>
                        /// Valor do ICMS ST retido
                        /// </summary>
                        public double vICMSST { get; set; }
                    }

                    public class ICMS90
                    {
                        /// <summary>
                        /// Origem da mercadoria: 
                        /// 0 – Nacional; 
                        /// 1 – Estrangeira – Importação direta;
                        /// 2 – Estrangeira – Adquirida no mercado interno.
                        /// </summary>
                        public string orig { get; set; }
                        /// <summary>
                        /// Tributação do ICMS: 00 – Tributada integralmente
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// 0 - Margem Valor Agregado (%);
                        /// 1 - Pauta (Valor);
                        /// 2 - Preço Tabelado Máx. (valor);
                        /// 3 - valor da operação.
                        /// </summary>
                        public string modBC { get; set; }
                        /// <summary>
                        /// Valor da base de calculo do imposto
                        /// </summary>
                        public double vBC { get; set; }

                        /// <summary>
                        /// Percentual da Redução de BC
                        /// </summary>
                        public double? pRedBC { get; set; }

                        /// <summary>
                        /// Aliquota do imposto
                        /// </summary>
                        public double pICMS { get; set; }
                        /// <summary>
                        /// Valor do ICMS
                        /// </summary>
                        public double vICMS { get; set; }
                        /// <summary>
                        /// Modalidade de determinação da BC do ICMS ST
                        /// 0 – Preço tabelado ou máximo sugerido;
                        /// 1 - Lista Negativa (valor);
                        /// 2 - Lista Positiva (valor);
                        /// 3 - Lista Neutra (valor);
                        /// 4 - Margem Valor Agregado (%);
                        /// 5 - Pauta (valor);
                        /// </summary>
                        public string modBCST { get; set; }
                        /// <summary>
                        /// Percentual da margem de valor Adicionado do ICMS ST
                        /// </summary>
                        public double? pMVAST { get; set; }
                        /// <summary>
                        /// Percentual da Redução de BC do ICMS ST
                        /// </summary>                    
                        public double? pRedBCST { get; set; }
                        /// <summary>
                        /// Valor do ICMS ST
                        /// </summary>
                        public double vBCST { get; set; }
                        /// <summary>
                        /// Alíquota do imposto do ICMS ST
                        /// </summary>
                        public double pICMSST { get; set; }
                        /// <summary>
                        /// Valor do ICMS ST retido
                        /// </summary>
                        public double vICMSST { get; set; }
                    }
                }

                /// <summary>
                /// Informar apenas quando o item for sujeito ao IPI
                /// </summary>
                public class IPI
                {
                    public string clEnq { get; set; }
                    public string CNPJProd { get; set; }
                    public string cSelo { get; set; }
                    public decimal qSelo { get; set; }
                    public string cEnq { get; set; }

                    public IPITrib IpiTrib { get; set; }
                    public IPINT IpiNT { get; set; }

                    /// <summary>
                    /// Informar apenas um dos grupos O07 ou O08 com base valor atribuído ao campo O09 – CST do
                    /// IPI
                    /// </summary>
                    public class IPITrib
                    {
                        /// <summary>
                        /// 00-Entrada com recuperação de crédito
                        /// 49-Outras entradas
                        /// 50-Saída tributada
                        /// 99-Outras saídas
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// Campo 010
                        /// 
                        /// Informar os campos O10 e O14 caso o cálculo do IPI seja por 
                        /// alíquota ou os campos O11 e O12 caso o cálculo do IPI seja valor por
                        /// unidade.
                        /// </summary>
                        public double vBC { get; set; }
                        /// <summary>
                        /// Campo 011
                        /// 
                        /// Informar os campos O10 e O14 caso o cálculo do IPI seja por 
                        /// alíquota ou os campos O11 e O12 caso o cálculo do IPI seja valor por
                        /// unidade.
                        /// </summary>
                        public decimal qUnid { get; set; }
                        /// <summary>
                        /// Campo 012
                        /// 
                        /// Informar os campos O10 e O14 caso o cálculo do IPI seja por 
                        /// alíquota ou os campos O11 e O12 caso o cálculo do IPI seja valor por
                        /// unidade.
                        /// </summary>
                        public double vUnid { get; set; }
                        /// <summary>
                        /// Campo 013
                        /// 
                        /// Informar os campos O10 e O14 caso o cálculo do IPI seja por 
                        /// alíquota ou os campos O11 e O12 caso o cálculo do IPI seja valor por
                        /// unidade.
                        /// </summary>
                        public double pIPI { get; set; }
                        /// <summary>
                        /// Valor do IPI
                        /// </summary>
                        public double vIPI { get; set; }
                    }

                    /// <summary>
                    /// TAG de grupo do CST 01, 02, 03,04, 51, 52, 53, 54 e 55
                    /// </summary>
                    public class IPINT
                    {
                        /// <summary>
                        /// 01-Entrada tributada com alíquota zero
                        /// 02-Entrada isenta
                        /// 03-Entrada não-tributada
                        /// 04-Entrada imune
                        /// 05-Entrada com suspensão
                        /// 51-Saída tributada com alíquota zero
                        /// 52-Saída isenta
                        /// 53-Saída não-tributada
                        /// 54-Saída imune
                        /// 55-Saída com suspensão
                        /// </summary>
                        public string CST { get; set; }
                    }
                }

                /// <summary>
                /// TAG de grupo do Imposto de Importação
                /// Informar apenas quando o item for sujeito ao II
                /// </summary>
                public class II
                {
                    /// <summary>
                    /// Valor da BC do Imposto de Importação
                    /// </summary>
                    public double vBC { get; set; }
                    /// <summary>
                    /// Valor das despesas aduaneiras
                    /// </summary>
                    public double vDespAdu { get; set; }
                    /// <summary>
                    /// Valor do Imposto de Importação
                    /// </summary>
                    public double vII { get; set; }
                    /// <summary>
                    /// Valor do Imposto sobre Operações Financeiras
                    /// </summary>
                    public double vIOF { get; set; }
                }

                /// <summary>
                /// Informar apenas um dos grupos Q02, Q03, Q04 ou Q05 com base valor atribuído ao campo Q06 –CST do PIS
                /// </summary>
                public class PIS
                {
                    public PISAliq PisAliq { get; set; }
                    public PISQtde PisQtd { get; set; }
                    public PISNT PisNT { get; set; }
                    public PISOutr PisOutr { get; set; }

                    public class PISAliq
                    {
                        /// <summary>
                        /// Código de situação tributária do PIS
                        /// 01 – Operação Tributável (base de cálculo = valor da operação alíquota normal (cumulativo/não cumulativo));
                        /// 02 - Operação Tributável (base de cálculo = valor da operação(alíquota diferenciada));
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// Valor da Base de Cálculo do PIS
                        /// </summary>
                        public double vBC { get; set; }
                        /// <summary>
                        /// Alíquota do PIS (em percentual)
                        /// </summary>
                        public double pPIS { get; set; }
                        /// <summary>
                        /// Valor do PIS
                        /// </summary>
                        public double vPIS { get; set; }
                    }

                    /// <summary>
                    /// TAG do grupo de PIS tributado por Qtde
                    /// CST = 03
                    /// </summary>
                    public class PISQtde
                    {
                        /// <summary>
                        /// 03 - Operação Tributável (base de cálculo = quantidade vendida x alíquota por unidade de produto);
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// Quantidade Vendida
                        /// </summary>
                        public decimal qBCProd { get; set; }
                        /// <summary>
                        /// Alíquota do PIS (em reais)
                        /// </summary>
                        public double vAliqProd { get; set; }
                        /// <summary>
                        /// Valor do PIS
                        /// </summary>
                        public double vPIS { get; set; }
                    }

                    /// <summary>
                    /// TAG do grupo de PIS não tributado
                    /// CST = 04, 06, 07, 08 ou 09
                    /// </summary>
                    public class PISNT
                    {
                        /// <summary>
                        /// 04 - Operação Tributável (tributação monofásica (alíquota zero));
                        /// 06 - Operação Tributável (alíquota zero);
                        /// 07 - Operação Isenta da Contribuição;
                        /// 08 - Operação Sem Incidência da Contribuição;
                        /// 09 - Operação com Suspensão da Contribuição;
                        /// </summary>
                        public string CST { get; set; }
                    }

                    /// <summary>
                    /// TAG do grupo de PIS Outras Operações
                    /// CST = 99
                    /// </summary>
                    public class PISOutr
                    {
                        /// <summary>
                        /// 99 - Outras Operações;
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// Informar campos para cálculo do PIS em percentual (P07 e P08) ou campos para PIS em valor (P10 e P11).
                        /// </summary>
                        public double vBC { get; set; }
                        public double pPIS { get; set; }
                        public decimal qBCProd { get; set; }
                        public double vAliqProd { get; set; }
                        public double vPIS { get; set; }
                    }
                }

                /// <summary>
                /// TAG do grupo de PIS Substituição Tributária
                /// </summary>
                public class PISST
                {
                    /// <summary>
                    /// Informar campos para cálculo do PIS em percentual (R02 e R03) ou campos para PIS em valor (R04 e R05).
                    /// </summary>
                    public double vBC { get; set; }
                    public double pPIS { get; set; }
                    public decimal qBCProd { get; set; }
                    public double vAliqProd { get; set; }
                    public double vPIS { get; set; }
                }

                /// <summary>
                /// TAG de grupo do COFINS
                /// Informar apenas um dos grupos S02, S03, S04 ou S04 com base valor atribuído ao campo 
                /// S06 – CST do COFINS
                /// </summary>
                public class COFINS
                {
                    public COFINSAliq CofinsAliq { get; set; }
                    public COFINSQtde CofinsQtde { get; set; }
                    public COFINSNT CofinsNT { get; set; }
                    public COFINSOutr CofinsOutr { get; set; }

                    /// <summary>
                    /// TAG do grupo de COFINS tributado pela alíquota
                    /// CST = 01 ou 02
                    /// </summary>
                    public class COFINSAliq
                    {
                        /// <summary>
                        /// 01 – Operação Tributável (base de cálculo = valor da operação alíquota normal (cumulativo/não cumulativo));
                        /// 02 - Operação Tributável (base de cálculo = valor da operação (alíquota diferenciada));
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// Valor da Base de Cálculo da COFINS
                        /// </summary>
                        public double vBC { get; set; }
                        /// <summary>
                        /// Alíquota da COFINS (em percentual)
                        /// </summary>
                        public double pCOFINS { get; set; }
                        /// <summary>
                        /// Valor do COFINS
                        /// </summary>
                        public double vCOFINS { get; set; }
                    }
                    /// <summary>
                    /// CST = 03
                    /// </summary>
                    public class COFINSQtde
                    {
                        /// <summary>
                        /// 03 - Operação Tributável (base de cálculo = quantidade vendida x  alíquota por unidade de produto);
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// Quantidade Vendida
                        /// </summary>
                        public decimal qBCProd { get; set; }
                        /// <summary>
                        /// Aliquota do COFINS (em Reais)
                        /// </summary>
                        public double vAliqProd { get; set; }
                        /// <summary>
                        /// Valor do COFINS
                        /// </summary>
                        public double vCOFINS { get; set; }
                    }

                    /// <summary>
                    /// TAG do grupo de COFINS não tributado
                    /// CST = 04, 06, 07, 08, 09
                    /// </summary>
                    public class COFINSNT
                    {
                        /// <summary>
                        /// 04 - Operação Tributável (tributação monofásica (alíquota zero));
                        /// 06 - Operação Tributável (alíquota zero);
                        /// 07 - Operação Isenta da Contribuição; 
                        /// 08 - Operação Sem Incidência da Contribuição;
                        /// 09 - Operação com Suspensão da Contribuição;
                        /// </summary>
                        public string CST { get; set; }
                    }

                    /// <summary>
                    /// TAG do grupo de COFINS outras operações
                    /// CST = 99
                    /// </summary>
                    public class COFINSOutr
                    {
                        /// <summary>
                        /// 99 - Outras Operações;
                        /// </summary>
                        public string CST { get; set; }
                        /// <summary>
                        /// Informar campos para cálculo do COFINS em percentual (S07 e S08) ou campos para COFINS em valor (S09 e S10).
                        /// </summary>
                        public double vBC { get; set; }
                        public double pCOFINS { get; set; }
                        public decimal qBCProd { get; set; }
                        public double vAliqProd { get; set; }
                        public double vCOFINS { get; set; }
                    }
                }

                /// <summary>
                /// TAG do grupo de COFINS Substituição Tributária
                /// </summary>
                public class COFINSST
                {
                    /// <summary>
                    /// Informar campos para cálculo do COFINS Substituição Tributária em percentual (T02 e T03) ou campos
                    /// para COFINS em valor (T04 e T05).
                    /// </summary>
                    public double vBC { get; set; }
                    public double pCOFINS { get; set; }
                    public decimal qBCProd { get; set; }
                    public double vAliqProd { get; set; }
                    public double vCOFINS { get; set; }
                }

                /// <summary>
                /// TAG do grupo do ISSQN
                /// Informar os campos para cálculo do ISSQN nas NFe conjugadas, onde há a prestação de serviços
                /// sujeitos ao ISSQN e fornecimento de peças sujeitas ao ICMS 
                /// </summary>
                public class ISSQN
                {
                    public double vBC { get; set; }
                    public double vAliq { get; set; }
                    public double vISSQN { get; set; }
                    /// <summary>
                    /// Informar o município de ocorrência do fato gerador do ISSQN. Utilizar
                    /// a Tabela do IBGE (Anexo IV - Tabela de UF, Município e País)
                    /// </summary>
                    public int cMunFG { get; set; }
                    /// <summary>
                    /// Informar o código da lista de serviços da LC 116/03 em que se
                    /// classifica o serviço.
                    /// </summary>
                    public string cListServ { get; set; }
                }
            }

            /// <summary>
            /// Informações adicionais do produto
            /// Norma referenciada, informações complementares, etc...
            /// </summary>
            public string infAdProd { get; set; }

        }

        /// <summary>
        /// TAG de valores totais
        /// </summary>
        public class total
        {
            public ICMSTot IcmsTot { get; set; }
            public ISSQNtot ISSQNTot { get; set; }
            public retTrib RetTrib { get; set; }

            /// <summary>
            /// TAG de grupo de Valores Totais referentes ao ICMS
            /// </summary>
            public class ICMSTot
            {
                /// <summary>
                /// Base de Cálculo do ICMS
                /// </summary>
                public double vBC { get; set; }
                /// <summary>
                /// Valor Total do ICMS
                /// </summary>
                public double vICMS { get; set; }
                /// <summary>
                /// Base de Cálculo do ICMS ST
                /// </summary>
                public double vBCST { get; set; }
                /// <summary>
                /// Valor Total do ICMS ST
                /// </summary>
                public double vST { get; set; }
                /// <summary>
                /// Valor Total dos produtos e serviços
                /// </summary>
                public double vProd { get; set; }
                /// <summary>
                /// Valor Total do Frete
                /// </summary>
                public double vFrete { get; set; }
                /// <summary>
                /// Valor Total do Seguro
                /// </summary>                
                public double vSeg { get; set; }
                /// <summary>
                /// Valor Total do Desconto
                /// </summary>                
                public double vDesc { get; set; }
                /// <summary>
                /// Valor Total do II
                /// </summary>                
                public double vII { get; set; }
                /// <summary>
                /// Valor Total do IPI
                /// </summary>                
                public double vIPI { get; set; }
                /// <summary>
                /// Valor Total do PIS
                /// </summary>                
                public double vPIS { get; set; }
                /// <summary>
                /// Valor total do COFINS
                /// </summary>                
                public double vCOFINS { get; set; }
                /// <summary>
                /// Outras Despesas acessórias
                /// </summary>                
                public double vOutro { get; set; }
                /// <summary>
                /// Valor total da NFE
                /// </summary>
                public double vNF { get; set; }
            }

            /// <summary>
            /// TAG de grupo de Valores Totais referentes ao ISSQN
            /// </summary>
            public class ISSQNtot
            {
                /// <summary>
                /// Valor Total dos Serviços sob não incidência ou não tributados pelo ICMS
                /// </summary>
                public double vServ { get; set; }
                /// <summary>
                /// Base de Cálculo do ISS
                /// </summary>
                public double vBC { get; set; }
                /// <summary>
                /// Valor Total do ISS
                /// </summary>
                public double vISS { get; set; }
                /// <summary>
                /// Valor do PIS sobre serviços
                /// </summary>
                public double vPIS { get; set; }
                /// <summary>
                /// Valor do COFINS sobre serviços
                /// </summary>
                public double vCOFINS { get; set; }
            }

            /// <summary>
            /// TAG de grupo de retenções de tributos
            /// 
            /// Exemplos de atos normativos que definem obrigatoriedade da retenção de contribuições:
            /// a) IRPJ/CSLL/PIS/COFINS - Fonte - Recebimentos de Órgãos Públicos Federais
            /// Lei nº 9.430, de 27 de dezembro de 1996, art. 64 Lei nº 10.833/2003, art. 34
            /// como normas infra-legais, temos como exemplo:
            /// Instrução Normativa SRF nº 480/2004 e Instrução Normativa nº 539, de 25/04/2005.
            /// b) Retenção do Imposto de Renda pelas Fontes Pagadoras REMUNERAÇÃO DE SERVIÇOS
            /// PROFISSIONAIS PRESTADOS POR PESSOA JURÍDICA LEI Nº 7.450/85, ART. 52 
            /// c) IRPJ, CSLL, COFINS e PIS - Serviços Prestados por Pessoas Jurídicas - Retenção na Fonte
            /// Lei nº 10.833 de 29.12.2003, arts. 30, 31, 32, 35 e 36
            /// </summary>
            public class retTrib
            {
                /// <summary>
                /// Valor Retido de PIS
                /// </summary>
                public double vRetPIS { get; set; }
                /// <summary>
                /// Valor Retido de COFINS
                /// </summary>
                public double vRetCOFINS { get; set; }
                /// <summary>
                /// Valor Retido de CSLL
                /// </summary>
                public double vRetCSSL { get; set; }
                /// <summary>
                /// Base de Cálculo do IRRF
                /// </summary>
                public double vBCIRRF { get; set; }
                /// <summary>
                /// Valor Retido do IRRF
                /// </summary>
                public double vIRRF { get; set; }
                /// <summary>
                /// Base de Cálculo da Retenção da Previdência Social
                /// </summary>
                public double vBCRetPrev { get; set; }
                /// <summary>
                /// Valor da Retenção da Previdência Social
                /// </summary>
                public double vRetPrev { get; set; }
            }
        }

        /// <summary>
        /// Tag de grupo de informações do transporte da NFE
        /// </summary>
        public class transp
        {
            public transporta Transporta { get; set; }
            public retTransp RetTransp { get; set; }
            public veicTransp VeicTransp { get; set; }
            public reboque Reboque { get; set; }
            public vol Vol { get; set; }
            /// <summary>
            /// Modalidade do frete
            /// </summary>
            public string modFrete { get; set; }

            /// <summary>
            /// TAG de grupo transportador
            /// </summary>
            public class transporta
            {
                /// <summary>
                /// CNPJ
                /// Informar o CNPJ do Transportador, preenchendo os zeros não significativos.
                /// </summary>
                public string CNPJ { get; set; }
                /// <summary>
                /// CNPJ
                /// Informar o CPF do Transportador, preenchendo os zeros não significativos.
                /// </summary>
                public string CPF { get; set; }
                /// <summary>
                /// Razão social ou nome
                /// </summary>
                public string xNome { get; set; }
                /// <summary>
                /// Inscrição estadual
                /// </summary>
                public string IE { get; set; }
                /// <summary>
                /// Endereço completo
                /// </summary>
                public string xEnder { get; set; }
                /// <summary>
                /// Nome do municipio
                /// </summary>
                public string xMun { get; set; }
                /// <summary>
                /// Sigla da UF
                /// </summary>
                public string UF { get; set; }
            }

            /// <summary>
            /// TAG de grupo de Retenção do ICMS do transporte
            /// 
            /// Informar o valor do ICMS do serviço de transporte retido.
            /// </summary>
            public class retTransp
            {
                /// <summary>
                /// Valor do Serviço
                /// </summary>
                public double vServ { get; set; }
                /// <summary>
                /// BC da Retenção do ICMS
                /// </summary>
                public double vBCRet { get; set; }
                /// <summary>
                /// Alíquota da Retenção
                /// </summary>
                public double pICMSRet { get; set; }
                /// <summary>
                /// Valor do ICMS Retido
                /// </summary>
                public double vICMSRet { get; set; }
                /// <summary>
                /// CFOP
                /// </summary>
                public string CFOP { get; set; }
                /// <summary>
                /// Código do município de ocorrência do fato gerador do ICMS do transporte
                /// 
                /// Informar o município de ocorrência do fato gerador do ICMS do transporte. 
                /// Utilizar a Tabela do IBGE (Anexo IV - Tabela de UF, Município e País)
                /// </summary>
                public string cMunFG { get; set; }
            }

            /// <summary>
            /// TAG de grupo Veículo
            /// </summary>
            public class veicTransp
            {
                /// <summary>
                /// Placa do Veículo
                /// </summary>
                public string placa { get; set; }
                /// <summary>
                /// Sigla da UF
                /// </summary>
                public string UF { get; set; }
                /// <summary>
                /// Registro Nacional de Transportador de Carga (ANTT)
                /// </summary>
                public string RNTC { get; set; }
            }

            /// <summary>
            /// TAG de grupo Reboque
            /// </summary>
            public class reboque
            {
                /// <summary>
                /// Placa do veiculo
                /// </summary>
                public string placa { get; set; }
                /// <summary>
                /// Sigla da UF
                /// </summary>
                public string UF { get; set; }
                /// <summary>
                /// Registro Nacional de Transportador de Carga (ANTT)
                /// </summary>
                public string RTNC { get; set; }
            }

            /// <summary>
            /// TAG de grupo Volumes
            /// </summary>
            public class vol
            {
                /// <summary>
                /// Quantidade de volumes transportados
                /// </summary>
                public string qVol { get; set; }
                /// <summary>
                /// Espécie dos volumes transportados
                /// </summary>
                public string esp { get; set; }
                /// <summary>
                /// Marca dos volumes transportados
                /// </summary>
                public string marca { get; set; }
                /// <summary>
                /// Numeração dos volumes transportados
                /// </summary>
                public string nVol { get; set; }
                /// <summary>
                /// Peso Líquido (em kg)
                /// </summary>
                [Formato("####0.000", "pt-BR")]
                public decimal pesoL { get; set; }
                /// <summary>
                /// Peso Bruto (em kg)
                /// </summary>
                [Formato("####0.000", "pt-BR")]
                public decimal pesoB { get; set; }

                /// <summary>
                /// TAG de grupo de Lacres
                /// </summary>
                public class lacres
                {
                    /// <summary>
                    /// Número dos lacres
                    /// </summary>
                    public string nLacre { get; set; }
                }
            }

        }

        /// <summary>
        /// TAG de grupo de Cobrança
        /// </summary>
        public class cobr
        {
            /// <summary>
            /// TAG de grupo da Fatura
            /// </summary>
            /// 

            public List<dup> Dup { get; set; }

            public class fat
            {
                /// <summary>
                /// Número da Fatura
                /// </summary>
                public string nFat { get; set; }
                /// <summary>
                /// Valor Original da Fatura
                /// </summary>
                public string vOrig { get; set; }
                /// <summary>
                /// Valor do desconto
                /// </summary>
                public string vDesc { get; set; }
                /// <summary>
                /// Valor Líquido da Fatura
                /// </summary>
                public string vLiq { get; set; }
            }

            public class dup
            {
                /// <summary>
                /// Número da duplicata
                /// </summary>
                public string nDup { get; set; }
                /// <summary>
                /// Data de vencimento
                /// </summary>
                public DateTime dVenc { get; set; }
                /// <summary>
                /// Valor da duplicata
                /// </summary>                                                
                public double vDup { get; set; }
            }
        }


        /// <summary>
        /// TAG de grupo de Informações Adicionais
        /// </summary>
        public class infAdic
        {
            /// <summary>
            /// Informações Adicionais de Interesse do Fisco
            /// </summary>
            public string infAdFisco { get; set; }
            /// <summary>
            /// Informações Complementares de interesse do Contribuinte
            /// </summary>
            public string infCpl { get; set; }

            /// <summary>
            /// TAG de grupo do campo de uso livre do contribuinte
            /// 
            /// Campo de uso livre do contribuinte, informar o nome do campo no atributo xCampo e o conteúdo do campo no xTexto
            /// </summary>
            public class obsCont
            {
                /// <summary>
                /// Identificação do campo
                /// </summary>
                public string xCampo { get; set; }
                /// <summary>
                /// Conteúdo do campo
                /// </summary>
                public string xTexto { get; set; }
            }

            /// <summary>
            /// Campo de uso livre do Fisco Informar o nome do campo no atributo xCampo
            /// e o conteúdo do campo no xTexto
            /// </summary>
            public class obsFisco
            {
                /// <summary>
                /// Identificação do campo
                /// </summary>
                public string xCampo { get; set; }
                /// <summary>
                /// Conteúdo do campo
                /// </summary>
                public string xTexto { get; set; }
            }

            /// <summary>
            /// Tag de grupo do processo
            /// Campo de uso livre do Fisco 
            /// Informar o nome do campo no atributo xCampo
            /// e o conteúdo do campo no xTexto
            /// </summary>
            public class procRef
            {
                /// <summary>
                /// Indentificador do processo ou ato concessório
                /// </summary>
                public string nProc { get; set; }
                /// <summary>
                /// Origem do processo, informar com:
                /// 0 - SEFAZ;
                /// 1 - Justiça Federal;
                /// 2 - Justiça Estadual;
                /// 3 - Secex/RFB;
                /// 9 - Outros
                /// </summary>
                public string indProc { get; set; }
            }

        }

        /// <summary>
        /// TAG do grupo de exportação 
        /// 
        /// Informar apenas na exportação
        /// </summary>
        public class exporta
        {
            /// <summary>
            /// Sigla da UF onde ocorrerá o Embarque dos produtos
            /// </summary>
            public string UFEmbarq { get; set; }
            /// <summary>
            /// Local onde ocorrerá o Embarque dos produtos
            /// </summary>
            public string xLocEmbarq { get; set; }
        }


        /// <summary>
        /// TAG do Grupo de Compra
        /// Informar adicionais de compra
        /// </summary>
        public class compra
        {
            /// <summary>
            /// Informar a identificação da Nota de Empenho, quando se tratar de compras públicas
            /// </summary>
            public string xNEmp { get; set; }
            /// <summary>
            /// Informar o pedido.
            /// </summary>
            public string xPed { get; set; }
            /// <summary>
            /// Informar o contrato de compra
            /// </summary>
            public string xCont { get; set; }
        }
    }

    public static class FuncoesNFe
    {
        public static string removeFormatacao(string texto)
        {
            string txt = "";

            txt = texto.Replace(".", "");
            txt = txt.Replace("-", "");
            txt = txt.Replace("/", "");
            txt = txt.Replace("(", "");
            txt = txt.Replace(")", "");
            txt = txt.Replace(" ", "");

            return txt;
        }

        public static void retornaAtributos(object[] obj, out CultureInfo cultura, out string formato, out bool obrigatorio)
        {
            cultura = CultureInfo.CreateSpecificCulture("en-US");
            formato = "###0.00";
            obrigatorio = false;
            foreach (object objeto in obj)
            {
                if (objeto is Formato)
                {
                    string culturaStr = ((Formato)objeto).cultura;
                    formato = ((Formato)objeto).formato;
                    cultura = CultureInfo.CreateSpecificCulture(culturaStr);
                }
                else
                    if (objeto is Obrigatorio)
                        obrigatorio = ((Obrigatorio)objeto).propriedadeObrigatoria;

            }

            //cultura.NumberFormat.NumberDecimalSeparator = ",";
            //cultura.NumberFormat.NumberGroupSeparator = ".";
        }

        public static int modulo11(string chaveNFE)
        {
            int soma = 0; // Vai guardar a Soma
            int mod = -1; // Vai guardar o Resto da divisão
            int dv = -1;  // Vai guardar o DigitoVerificador
            int pesso = 2; // vai guardar o pesso de multiplicacao

            //percorrendo cada caracter da chave da direita para esquerda para fazer os calculos com o pesso
            for (int i = chaveNFE.Length - 1; i != -1; i--)
            {
                int ch = Convert.ToInt32(chaveNFE[i].ToString());
                soma += ch * pesso;
                //sempre que for 9 voltamos o pesso a 2
                if (pesso < 9)
                    pesso += 1;
                else
                    pesso = 2;
            }

            //Agora que tenho a soma vamos pegar o resto da divisão por 11
            mod = soma % 11;
            //Aqui temos uma regrinha, se o resto da divisão for 0 ou 1 então o dv vai ser 0
            if (mod == 0 || mod == 1)
                dv = 0;
            else
                dv = 11 - mod;

            return dv;

        }

        public static string TirarAcento(string palavra)
        {
            string palavraSemAcento = "";
            string caracterComAcento = "áàãâäéèêëíìîïóòõôöúùûüçÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÖÔÚÙÛÜÇ";
            string caracterSemAcento = "aaaaaeeeeiiiiooooouuuucAAAAAEEEEIIIIOOOOOUUUUC";

            for (int i = 0; i < palavra.Length; i++)
            {
                if (caracterComAcento.IndexOf(Convert.ToChar(palavra.Substring(i, 1))) >= 0)
                {
                    int car = caracterComAcento.IndexOf(Convert.ToChar(palavra.Substring(i, 1)));
                    palavraSemAcento += caracterSemAcento.Substring(car, 1);
                }
                else
                {
                    palavraSemAcento += palavra.Substring(i, 1);
                }
            }

            return palavraSemAcento;
        }

        /// <summary>
        /// Função que utilizo para saber se é uma propriedade simples (String, Int) ou uma nova classe, que deve gerar
        /// uma nova tag xml
        /// </summary>
        /// <param name="propriedade"></param>
        /// <returns></returns>
        public static bool novaTag(PropertyInfo propriedade)
        {
            //TODO: Buscar uma forma melhor de identificar as subclasses.

            switch (propriedade.PropertyType.ToString())
            {
                case "System.DateTime":
                case "System.Int32":
                case "System.String":
                case "System.Double":
                case "System.Nullable":
                case "System.Decimal":
                //Propriedades que podem ser nulas (com ?)...
                case "System.Nullable`1[System.Int32]":
                case "System.Nullable`1[System.DateTime]":
                case "System.Nullable`1[System.Decimal]":
                case "System.Nullable`1[System.Double]":
                    return false;
                default: return true;
            }
        }

        public static void gravarElemento(XmlWriter writer, string nomeTag, object valorTag, object[] atributos)
        {
            /*CultureInfo cultBR = new CultureInfo("pt-BR");
            CultureInfo cultUS = new CultureInfo("en-US");*/
            CultureInfo cult;
            string formato;
            bool obrigatorio;
            retornaAtributos(atributos, out cult, out formato, out obrigatorio);

            if (valorTag != null)
            {
                string valor = "";
                switch (valorTag.GetType().ToString())
                {
                    case "System.DateTime":
                        valor = ((DateTime)(valorTag)).ToString("yyyy-MM-dd");   //formata no padrão necessário para a NFe                     
                        break;
                    case "System.Int32":
                        valor = valorTag.ToString();
                        if (valor.Trim() == "0") //campos do tipo inteiro com valor 0 serão ignorados
                            valor = "";
                        break;
                    case "System.String":
                        valor = TirarAcento(valorTag.ToString().Replace("\r\n", " - ")).Trim(); //remove linhas... e tira acentos
                        break;
                    case "System.Double":
                        valor = ((double)valorTag).ToString(formato, cult.NumberFormat); //formata de acordo com o atributo
                        break;
                    case "System.Decimal":
                        valor = ((decimal)valorTag).ToString(formato, cult.NumberFormat); //formata de acordo com o atributo
                        break;

                }
                if ((valor.Trim().Length > 0) || (obrigatorio))
                    writer.WriteElementString(nomeTag, valor);
            }
        }

        public static long tamanhoXML(XmlDocument documento)
        {
            string nomeArquivo = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

            try
            {
                documento.Save(nomeArquivo);
                FileInfo info = new FileInfo(nomeArquivo);
                long tamanhoArquivo = info.Length;

                info.Delete();

                return tamanhoArquivo;
            }
            catch
            {
                return 0;
            }

        }
        
    }



}

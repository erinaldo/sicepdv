using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Objects;


namespace SICEpdv
{
    public partial class FrmNotaFiscal : Form
    {
        siceEntities Entidades = Conexao.CriarEntidade();
        NFe NotaFiscal = new NFe();
        List<nfoperacao> cfops = new List<nfoperacao>();


        string[] CST = new string[36];
        string[] CSTIPI = new string[4];
        string[] Pagamento = new string[3];
        static public string NumeroDocumento;
        string TipoDestinatario = "", GeraICMS = "S";
        int SerieNota = 0;
        long NDocumento;
        bool modificarItens = true;
        bool lancarItens = true;
        public List<Produtos> DadosItens = new List<Produtos>();



        public FrmNotaFiscal()
        {

            InitializeComponent();


            //MontaGrid("Itens");
            //MontaGrid("ICMS");
            //MontaGrid("IPI");
            //MontaGrid("Pis");
            //MontaGrid("Cofins");
            //MontaGrid("pagamento");           

            cbTipo.Items.Add("0-Entrada");
            cbTipo.Items.Add("1-Saida");

            //cbTipo.Text = "0-Saida";

            cbModeloNFe.Items.Add("55 - NF-e Nota Fiscal Eletronica");
            cbModeloNFe.Items.Add("01-Nota Fiscal, modelos 1 e 1-A  - ");
            cbModeloNFe.Text = "55 - NF-e Nota Fiscal Eletronica";

            cbEmissao.Items.Add("1-Normal");
            cbEmissao.Items.Add("2-Contingência");
            cbEmissao.Text = "1-Normal";

            cbFinalidade.Items.Add("1-Normal");
            cbFinalidade.Items.Add("2-NF_e Complementar");
            cbFinalidade.Items.Add("3-NF_e Ajuste");
            cbFinalidade.Text = "1-Normal";

            cbSituacaoNF.Items.Add("00-Documento Regular");
            cbSituacaoNF.Items.Add("01-Escrituração extemporânea de doc. regular");
            cbSituacaoNF.Items.Add("02-Documento cancelado");
            cbSituacaoNF.Items.Add("03-Escrituração extemporânea de doc. cancelado");
            cbSituacaoNF.Items.Add("04-NF-e ou CT-e denegado");
            cbSituacaoNF.Items.Add("05-NF-e ou CT-e numeração inutilizada");
            cbSituacaoNF.Items.Add("06-Documento fiscal complementar");
            cbSituacaoNF.Items.Add("07-Escrituração extemporânea de doc. complementar");
            cbSituacaoNF.Items.Add("08-Doc. Fiscal emitido com base em Regime Especial ou Norma específica");
            cbSituacaoNF.Text = "00-Documento Regular";


            var naturezanfe = (from n in Entidades.naturezanf
                               where n.codigofilial == GlbVariaveis.glb_filial
                               select n.naturezaoperacao
                              ).ToList();


            naturezanfe.Insert(0, "Venda");
            naturezanfe.Insert(1, "Compra");
            naturezanfe.Insert(2, "Transferencia");
            naturezanfe.Insert(3, "Devolucao");
            naturezanfe.Insert(4, "Importacao");
            naturezanfe.Insert(5, "Consignacao");
            naturezanfe.Insert(6, "Remessa");
            naturezanfe.Insert(7, "Retorno de remessa para venda fora do estabelecimento");
            naturezanfe.Insert(8, "Consumo Interno");


            cbOperacao.DataSource = naturezanfe;
            cbOperacao.DisplayMember = "naturezaoperacao";




            var dados = (from n in Entidades.filiais
                         where n.ativa == "S"
                         select n.CodigoFilial + " - " + n.descricao).ToList();





            var Serie = (from sr in Entidades.serienf
                         where sr.codigofilial == GlbVariaveis.glb_filial
                         select sr.descricao).ToList();



            //Serie.Insert(0, "");


            cbSerie.DataSource = Serie;
            cbSerie.DisplayMember = "descricao";


            SelecionaNumerodeSerie();


            CST[0] = "000 - Tributada Integralmente";
            CST[1] = "010 - Tributada e com cob. de ICMS por ST";
            CST[2] = "020 - Com redução da Base de Cálculo";
            CST[3] = "030 - Isenta ou não trib. com cob.  de ICMS ST";
            CST[4] = "040 - Isenta";
            CST[5] = "050 - Suspensão";
            CST[6] = "060 - ICMS cobrado ant.  por ST";
            CST[7] = "070 - Com red. de base de cálc e cob. de icms por ST";
            CST[8] = "090 - Outras";
            CST[9] = "100 - Estrang- Tributada Integralmente";
            CST[10] = "101 - Tributada pelo Simples Nacional com permissão de crédito ";
            CST[11] = "102 -Tributada pelo Simples Nacional sem permissão de crédito ";
            CST[12] = "103 - Isenção do ICMS no Simples Nacional para faixa de receita bruta. ";
            CST[13] = "110 - Estrang- Trib. e com cob. de ICMS por ST";
            CST[14] = "120 - Estrang- Com redução da Base de Cálculo";
            CST[15] = "130 - Est-Isenta ou não trib. c/ cob. de ICMS por ST";
            CST[16] = "140 - Estrangeira Isenta";
            CST[17] = "150 - Estrangeira Suspensão";
            CST[18] = "160 - Estrang- ICMS cob. anteriormente  por ST";
            CST[19] = "170 - Est-Com red.de base de cálc e cob. icms por ST";
            CST[20] = "190 - Estrangeira Outras";
            CST[21] = "200 - Estrang-Compra Interna - Trib Integralmente";
            CST[22] = "201 - Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS por Substituição Tributária. ";
            CST[23] = "202 - Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por Substituição Tributária. ";
            CST[24] = "203 - Isenção do ICMS nos Simples Nacional para faixa de receita bruta e com cobrança do ICMS por Substituição Tributária. ";
            CST[25] = "210 - Est-Compra Int Trib. e com cob. de ICMS por ST";
            CST[26] = "220 - Estrang-Compra Int. Com red da Base de Cálc";
            CST[27] = "230 - Est-Com Int.Is ou ñ trib. cob.de ICMS por ST";
            CST[28] = "240 - Estrang-Compra Interna Isenta";
            CST[29] = "250 - Estrang-Compra Interna Suspensão";
            CST[30] = "260 - Estrang-Compra Int. ICMS cob. ant.  por ST";
            CST[31] = "270 - Est-Com Int C/ red. b. de cálc.e cob. icms por ST";
            CST[32] = "290 - Estrang-Compra Interna Outras";
            CST[33] = "300 – Imune. 400 – Não tributada pelo Simples Nacional. ";
            CST[34] = "500 – ICMS cobrado anteriormente por substituição tributária (substituído) ou por antecipação. ";
            CST[35] = "900 – Outros.";

            cbCSTICMS.DataSource = CST;

            CSTIPI[0] = "00 - Entrada com recuperação de crédito";
            CSTIPI[1] = "49 - Outras Entradas";
            CSTIPI[2] = "50 - Saida Tributada";
            CSTIPI[3] = "99 - Outras Saídas";

            cbTributacaoIPI.DataSource = CSTIPI;


            cbCstPis.DataSource = NotaFiscal.CSTPisCofins(cbTipo.Text == "1" ? "S" : "E");
            cbCstCofins.DataSource = NotaFiscal.CSTPisCofins(cbTipo.Text == "1" ? "S" : "E");

            Pagamento[0] = "0-Pagamento Avista";
            Pagamento[1] = "1-Pagamento a Prazo";
            Pagamento[2] = "2-Outros";

            cbPagamento.DataSource = Pagamento;


            //Cblyout.Items.Add("1 - Retrato");
            //Cblyout.Items.Add("2 - Paisagem");
            //Cblyout.Text = "1 - Retrato";
            //cbModelo.Items.Add("1-Padrão carta");
            //cbModelo.Items.Add("2-Padrão carta pequeno");
            //cbModelo.Items.Add("3-Carta (novo)");
            //cbModelo.Items.Add("4-Carta com fatura");
            //cbModelo.Items.Add("5-Nota fiscal de serviço");
            //cbModelo.Text = "1-Padrão carta";




            cbTipoFrete.Items.Add("0 - Por conta do emitente");
            cbTipoFrete.Items.Add("1 - Por conta do destinatário/remetente");
            cbTipoFrete.Items.Add("2 - Por conta de terceiros");
            cbTipoFrete.Items.Add("9 - Sem frete");
            cbTipoFrete.Text = "9 - Sem frete";

            PopulaOBS();
            aplicamascaras();

        }

        private void PopulaTransportes()
        {
            var transportes = (from T in Entidades.transportadoras
                               select new
                               {
                                   Fantasia = T.fantasia,
                                   id = T.inc

                               }).ToList();

            cbTransportes.DataSource = transportes;
            cbTransportes.ValueMember = "id";
            cbTransportes.DisplayMember = "fantasia";
        }


        private void Transportes(int codigo)
        {

            var transportes = (from T in Entidades.transportadoras
                               where T.inc == codigo
                               select new
                               {
                                   Fantasia = T.fantasia,
                                   Endereco = T.endereco,
                                   Cidade = T.cidade,
                                   IE = T.inscricao,
                                   id = T.inc,
                                   UF = T.estado
                               }).FirstOrDefault();


            txtEnredecoTransp.Text = transportes.Endereco.ToString();
            txtCidadeTransp.Text = transportes.Cidade.ToString();
            txtIETransp.Text = transportes.IE.ToString();




            int CodTransportadora = transportes.id;

            var veiculos = (from V in Entidades.veiculos
                            where V.idtransportadora == CodTransportadora
                            select new
                            {
                                Veiculo = V.veiculo,
                                id = V.inc
                            }).ToList();

            cbVeiculo.DataSource = veiculos;
            cbVeiculo.ValueMember = "id";
            cbVeiculo.DisplayMember = "Veiculo";
        }

        private void DadosVeiculos(int codigo)
        {

            var veiculos = (from V in Entidades.veiculos
                            where V.inc == codigo
                            select new
                            {
                                Veiculo = V.veiculo,
                                placa = V.placa,
                                UF = V.estadoplaca,
                                ANTT = V.ANTT

                            }).FirstOrDefault();

            txtPlaca.Text = veiculos.placa.ToString();
            txtANTT.Text = veiculos.ANTT.ToString();
            txtUFTransp.Text = veiculos.UF.ToString();


        }

        private void PopulaVeiculos(int codigo)
        {
            var veiculos = (from V in Entidades.veiculos
                            where V.idtransportadora == codigo
                            select new
                            {
                                Veiculo = V.veiculo,
                                id = V.inc

                            }).ToList();

            cbVeiculo.DataSource = veiculos;
            cbVeiculo.ValueMember = "id";
            cbVeiculo.DisplayMember = "Veiculo";
        }

        private void PopulaOBS()
        {

            var OBS = (from I in Entidades.infocomplementarnf
                       select new
                       {
                           codigo = I.id,
                           descricao = I.descricao

                       }).ToList();
            CbOBS.DataSource = OBS;
            CbOBS.ValueMember = "codigo";
            CbOBS.DisplayMember = "descricao";
        }

        private void SelecionaSerie(object sender, EventArgs e)
        {

            SelecionaNumerodeSerie();

        }


        private void SelecionaNumerodeSerie()
        {
            if (cbSerie.Text != "")
            {
                var Serie = (from sr in Entidades.serienf
                             where sr.codigofilial == GlbVariaveis.glb_filial && (sr.descricao == cbSerie.Text || sr.serie == cbSerie.Text)
                             select new
                             {
                                 Serie = sr.serie,
                                 Descricao = sr.descricao,
                                 Sequencia = sr.sequencial
                             }).AsQueryable();

                lblNumNota2.Text = (Serie.First().Sequencia + 1).ToString();
                cbSerie.Text = Serie.First().Descricao;

                SerieNota = int.Parse(Serie.First().Serie);
            }
            else
            {
                lblNumNota2.Text = "1";
            }
        }


        private void clientes(object sender, EventArgs e)
        {
            FrmClientes Clientes = new FrmClientes();
            Clientes.ShowDialog();
            int CodigoCliente = FrmClientes.codigoCliente;
            ChamaCliente(CodigoCliente);
            NotaFiscal.idCliente = CodigoCliente;
            NotaFiscal.idFornecedorNFe = 0;
        }

        private void ChamaCliente(int CodigoCliente)
        {


            if (CodigoCliente != 0)
            {
                //Pesquisar clientes
                #region
                var DadosClientesOrig = (from c in Entidades.clientes
                                         where c.Codigo == CodigoCliente
                                         select new
                                         {
                                             Codigo = c.Codigo,
                                             Nome = c.Nome,
                                             CPF = c.cpf,
                                             CNPJ = c.cnpj,
                                             Inscricao = c.inscricao,
                                             Estado = c.estado,
                                             Endereco = c.endereco,
                                             Numero = c.numero,
                                             Bairro = c.bairro,
                                             Municipio = c.cidade,
                                             CEP = c.cep,
                                             Fone = c.telefone,
                                             Fax = c.fax,
                                             Fone2 = c.telefone2,
                                             Fone3 = c.telefone3

                                         }).AsQueryable();


                var DadosClientes = DadosClientesOrig.ToList();


                var dadosFilial = (from n in Entidades.filiais
                                   where n.CodigoFilial == GlbVariaveis.glb_filial
                                   select new { n.CodigoFilial, n.estado }).FirstOrDefault();

                NotaFiscal.destino = "D";
                if (DadosClientes.First().Estado != dadosFilial.estado)
                {
                    NotaFiscal.destino = "F";
                }

                NotaFiscal.tipoCliente = "F";
                if (DadosClientes.First().CNPJ.Trim().Length > 1)
                    NotaFiscal.tipoCliente = "J";


                #endregion


                //popula clientes
                #region
                if ((DadosClientes.First().Nome != null ? DadosClientes.First().Nome : string.Empty) == "")
                {
                    MessageBox.Show("Nome do Destinatário não pode ser Vazio!");

                }
                else if (((DadosClientes.First().CPF != null ? DadosClientes.First().CPF : string.Empty) == "" || (DadosClientes.First().CPF != null ? DadosClientes.First().CPF : string.Empty).Length < 11) || ((DadosClientes.First().CNPJ != null ? DadosClientes.First().CNPJ : string.Empty).ToString() == "" || (DadosClientes.First().CNPJ != null ? DadosClientes.First().CNPJ : string.Empty).Length < 14))
                {
                    MessageBox.Show("CPF ou CNPJ do Destinatário invalido!");
                }
                else if ((DadosClientes.First().Estado != null ? DadosClientes.First().Estado : string.Empty) == "")
                {
                    MessageBox.Show("Estado do Destinatário não pode ser Vazio!");
                }
                else if ((DadosClientes.First().Endereco != null ? DadosClientes.First().Endereco : string.Empty) == "")
                {
                    MessageBox.Show("Endereço do Destinatário não pode ser Vazio!");
                }
                else if ((DadosClientes.First().Numero != null ? DadosClientes.First().Numero : string.Empty) == "")
                {
                    MessageBox.Show("Numero do Destinatário não pode ser Vazio!");
                }
                else if ((DadosClientes.First().Bairro != null ? DadosClientes.First().Bairro : string.Empty) == "")
                {
                    MessageBox.Show("Bairro do Destinatário não pode ser Vazio!");
                }

                else if ((DadosClientes.First().Municipio != null ? DadosClientes.First().Municipio : string.Empty) == "")
                {

                    MessageBox.Show("Munícipio do Destinatário não pode ser Vazio!");
                }
                else if ((DadosClientes.First().CEP != null ? DadosClientes.First().CEP : string.Empty) == "" || (DadosClientes.First().CEP != null ? DadosClientes.First().CEP : string.Empty).Length < 8)
                {
                    MessageBox.Show("CEP do Destinatário Inválido ou esta vázio!");

                }


                else
                {
                    bool liberacao = false;
                    string contato = "";

                    if ((DadosClientes.First().Fone != null ? DadosClientes.First().Fone : string.Empty) != "" || (DadosClientes.First().Fone != null ? DadosClientes.First().Fone : string.Empty).Length == 8)
                    {

                        contato = DadosClientes.First().Fone.ToString();
                        liberacao = true;
                    }
                    else if ((DadosClientes.First().Fone2 != null ? DadosClientes.First().Fone2 : string.Empty) != "" || (DadosClientes.First().Fone2 != null ? DadosClientes.First().Fone2 : string.Empty).Length == 8)
                    {
                        contato = DadosClientes.First().Fone2.ToString();
                        liberacao = true;
                    }
                    else if ((DadosClientes.First().Fone3 != null ? DadosClientes.First().Fone3 : string.Empty) != "" || (DadosClientes.First().Fone3 != null ? DadosClientes.First().Fone3 : string.Empty).Length == 8)
                    {
                        contato = DadosClientes.First().Fone3.ToString();
                        liberacao = true;
                    }
                    else if ((DadosClientes.First().Fax != null ? DadosClientes.First().Fax : string.Empty) != "" || (DadosClientes.First().Fone2 != null ? DadosClientes.First().Fone2 : string.Empty).Length == 8)
                    {
                        contato = DadosClientes.First().Fax.ToString();
                        liberacao = true;
                    }
                    else
                    {
                        MessageBox.Show("Contato do Destinatário não pode ser Vazio!");
                        liberacao = false;
                    }





                    if (liberacao == true)
                    {

                        txtCodCliente.Text = DadosClientes.First().Codigo.ToString();
                        txtDestinatario.Text = DadosClientes.First().Nome.ToString();
                        if (DadosClientes.First().CPF.ToString() != "")
                        {
                            txtCPF_CNPJ.Text = DadosClientes.First().CPF.ToString();
                        }
                        else
                        {
                            txtCPF_CNPJ.Text = DadosClientes.First().CNPJ.ToString();
                        }
                        cbUF.Text = DadosClientes.First().Estado.ToString();
                        txtEndereco.Text = DadosClientes.First().Endereco.ToString();
                        txtNumDestinatario.Text = DadosClientes.First().Numero.ToString();
                        txtBairro.Text = DadosClientes.First().Bairro.ToString();
                        txtMunicipio.Text = DadosClientes.First().Municipio.ToString();
                        txtCep.Text = DadosClientes.First().CEP.ToString();
                        txtFoneFax.Text = DadosClientes.First().Fone.ToString();
                        TipoDestinatario = "C";
                    }




                }
                #endregion

            }
        }

        private void bntBuscFornecedores_Click(object sender, EventArgs e)
        {
            FrmFornecedores Fornecedores = new FrmFornecedores();
            Fornecedores.ShowDialog();

            int CodDestinatario = FrmFornecedores.codigoDestinatario;
            NotaFiscal.idFornecedorNFe = CodDestinatario;
            NotaFiscal.idCliente = 0;
            if (CodDestinatario != 0)
            {

                //pesquisa fornecedo
                #region
                var DadosDestinatario = (from F in Entidades.fornecedores
                                         where F.Codigo == CodDestinatario
                                         select new
                                         {
                                             Codigo = F.Codigo,
                                             RazaoSocial = F.razaosocial ?? "",
                                             CNPJ = F.CGC ?? "",
                                             Inscricao = F.INSCRICAO ?? "",
                                             Estado = F.ESTADO ?? "",
                                             Endereco = F.ENDERECO ?? "",
                                             Numero = F.numero ?? "",
                                             Bairro = F.BAIRRO ?? "",
                                             Municipio = F.CIDADE ?? "",
                                             CEP = F.CEP ?? "",
                                             Fone = F.TELEFONE ?? "",
                                             CPF = F.CPF

                                         }).AsQueryable();
                #endregion

                var dadosFilial = (from n in Entidades.filiais
                                   where n.CodigoFilial == GlbVariaveis.glb_filial
                                   select new { n.CodigoFilial, n.estado }).FirstOrDefault();

                NotaFiscal.destino = "D";
                if (DadosDestinatario.First().Estado != dadosFilial.estado)
                {
                    NotaFiscal.destino = "F";
                }
                NotaFiscal.tipoCliente = "F";
                if (DadosDestinatario.First().CNPJ.Trim().Length > 1)
                    NotaFiscal.tipoCliente = "J";



                //valida e popula do os dados
                #region
                if (DadosDestinatario.First().RazaoSocial == "")
                {

                    MessageBox.Show("Razão Social do Destinatário não pode Ser vazio!");

                }
                else if ((DadosDestinatario.First().CNPJ == "" || DadosDestinatario.First().CNPJ.Length < 14) && (DadosDestinatario.First().CPF == "" || DadosDestinatario.First().CPF.Length < 11))
                {
                    MessageBox.Show("CNPJ do Destinatário Invalido!");
                }
                else if (DadosDestinatario.First().Inscricao == "")
                {
                    MessageBox.Show("Inscrição do Destinatário Invalido!");
                }
                else if (DadosDestinatario.First().Estado == "")
                {
                    MessageBox.Show("Estado do Destinatário não pode Ser vazio!");
                }
                else if (DadosDestinatario.First().Endereco == "")
                {
                    MessageBox.Show("Endereço do Destinatário não pode Ser vazio!");
                }
                else if (DadosDestinatario.First().Numero == "")
                {
                    MessageBox.Show("Numero de Endereço do Destinatário não pode Ser vazio!");
                }
                else if (DadosDestinatario.First().Bairro == "")
                {
                    MessageBox.Show("Bairro do Destinatário não pode Ser vazio!");
                }
                else if (DadosDestinatario.First().Municipio == "")
                {
                    MessageBox.Show("Municipio do Destinatário não pode Ser vazio!");
                }
                else if (DadosDestinatario.First().CEP == "")
                {
                    MessageBox.Show("CEP do Destinatário não pode Ser vazio!");
                }
                else if (DadosDestinatario.First().Fone == "")
                {
                    MessageBox.Show("Fone do Destinatário não pode Ser vazio!");
                }
                else
                {

                    txtCodCliente.Text = DadosDestinatario.First().Codigo.ToString();
                    txtDestinatario.Text = DadosDestinatario.First().RazaoSocial;
                    txtCPF_CNPJ.Text = DadosDestinatario.First().CNPJ;
                    txtI_E.Text = DadosDestinatario.First().Inscricao;
                    cbUF.Text = DadosDestinatario.First().Estado;
                    txtEndereco.Text = DadosDestinatario.First().Endereco;
                    txtBairro.Text = DadosDestinatario.First().Bairro;
                    txtMunicipio.Text = DadosDestinatario.First().Municipio;
                    txtNumDestinatario.Text = DadosDestinatario.First().Numero;
                    txtCep.Text = DadosDestinatario.First().CEP;
                    txtFoneFax.Text = DadosDestinatario.First().Fone;
                    TipoDestinatario = "FN";

                }
                #endregion


            }




        }


        private bool chamaProdutos(string chamaProdutos)
        {
            bool resutado = true;

            if (chamaProdutos == "S")
            {
                FrmProdutos ObjProdutos = new FrmProdutos();
                ObjProdutos.ShowDialog();

                string Codigo = FrmProdutos.ultCodigo;
                txtCodigoProduto.Text = Codigo;

                if (Codigo != "")
                {
                    var Itens = (from p in Entidades.produtos
                                 where p.codigo == Codigo
                                 select new
                                 {
                                     Descricao = p.descricao,
                                     ValorUnitario = p.precovenda,
                                     ICMS = p.icms,
                                     Tributacao = p.tributacao,
                                     CstPis = p.tributacaoPIS,
                                     CstCofins = p.tributacaoCOFINS,
                                     CodigoSuspensaoPis = p.codigosuspensaopis,
                                     CodigoSuspensaoCofins = p.codigosuspensaocofins,
                                     CFOP = p.cfopsaida
                                 }).AsQueryable();

                    txtPrecoItem.Text = Itens.First().ValorUnitario.ToString();
                    txtIcmsItem.Text = Itens.First().ICMS.ToString();
                    lbl_descricao.Text = Itens.First().Descricao.ToString();


                    resutado = true;



                }
            }
            else
            {

                try
                {

                    var Itens = (from p in Entidades.produtos
                                 where p.codigo == txtCodigoProduto.Text
                                 select new
                                 {
                                     Descricao = p.descricao,
                                     ValorUnitario = p.precovenda,
                                     ICMS = p.icms,
                                     Tributacao = p.tributacao,
                                     CstPis = p.tributacaoPIS,
                                     CstCofins = p.tributacaoCOFINS,
                                     CodigoSuspensaoPis = p.codigosuspensaopis,
                                     CodigoSuspensaoCofins = p.codigosuspensaocofins,
                                     CFOP = p.cfopsaida
                                 }).AsQueryable();

                    txtPrecoItem.Text = Itens.First().ValorUnitario.ToString();
                    txtIcmsItem.Text = Itens.First().ICMS.ToString();
                    lbl_descricao.Text = Itens.First().Descricao.ToString();


                    resutado = true;

                }
                catch (Exception erro)
                {

                    MessageBox.Show("Codigo Invalido: " + erro.Message);
                    txtCodigoProduto.Focus();
                    txtQuantidade.Text = "";
                    txtPrecoItem.Text = "";
                    txtIcmsItem.Text = "";

                    resutado = false;
                }



            }

            return resutado;
        }

        private void txtCodigoProduto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 37 || e.KeyChar == 13)
            {
                if (txtCodigoProduto.Text.Trim() == "")
                {
                    chamaProdutos("S");
                }
                else
                {
                    if (chamaProdutos("N") == true)
                    {
                        txtQuantidade.Focus();
                    }
                }

            }
        }

        private void txtQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 37 || e.KeyChar == 13)
            {
                if (txtQuantidade.Text.Trim() != "")
                {
                    txtPrecoItem.Focus();
                }
                else
                {
                    txtCodigoProduto.Focus();
                }
            }
        }

        private void txtPrecoItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 37 || e.KeyChar == 13)
            {
                if (txtPrecoItem.Text.Trim() != "")
                {
                    cbCFOPItens.Focus();
                }
                else
                {
                    txtQuantidade.Focus();
                }
            }
        }

        private void cbCFOPItens_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 37 || e.KeyChar == 13)
            {
                if (cbCFOPItens.Text.Trim() != "")
                {
                    txtIcmsItem.Focus();
                }
                else
                {
                    txtPrecoItem.Focus();
                }
            }

        }

        private void txtIcmsItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 37 || e.KeyChar == 13)
            {
                if (txtIcmsItem.Text.Trim() != "")
                {
                    AdicionarItem(txtCodigoProduto.Text, 1);
                    Totalizadores();
                    txtCodigoProduto.Focus();
                    txtCodigoProduto.Text = "";
                    txtQuantidade.Text = "";
                    txtPrecoItem.Text = "";
                    txtIcmsItem.Text = "";

                }
                else
                {
                    cbCFOPItens.Focus();
                }
            }
        }

        private void MontaGrid()
        {

            dgIitens.DataSource = (from n in NotaFiscal.itens
                                   select new { n.sequencia, n.codigo, n.descricao, n.quantidade, n.preco, n.total, n.csticms, n.icms, n.cfop, n.ipi }).ToList();

            dgItensIPI.DataSource = (from n in NotaFiscal.itens
                                     select new { n.sequencia, n.codigo, n.descricao, n.ipi, n.cstipi }).ToList();

            dgItensCofins.DataSource = (from n in NotaFiscal.itens
                                        select new { n.sequencia, n.codigo, n.descricao, n.cstcofins, n.cofins }).ToList();

            dgItensPis.DataSource = (from n in NotaFiscal.itens
                                     select new { n.sequencia, n.codigo, n.descricao, n.cstpis, n.pis }).ToList();

            dgItensICMS.DataSource = (from n in NotaFiscal.itens
                                      select new { n.sequencia, n.codigo, n.descricao, n.cfop, n.csticms, n.icms, n.icmsst, n.redbc, n.redbcst, n.mva }).ToList();

            dgPagamento.DataSource = (from p in NotaFiscal.pagamento
                                      select new { p.parcela, p.valor, p.vencimento, p.tipo }).ToList();

        }


        private void AdicionarItem(string Codigo, int acao)
        {
            string origem = "";

            if (NotaFiscal.doc > 0)
            {
                MessageBox.Show("Nota Fiscal possui itens de um documento, finalize primeiro");
                return;
            };
            if (!modificarItens)
            {
                MessageBox.Show("Documento referenciado não é possível adicionar ou excluir item !");
            };




            var Itens = ((from p in Entidades.produtos
                          where p.codigo == Codigo
                          select new
                          {
                              Descricao = p.descricao,
                              ValorUnitario = p.precovenda,
                              ICMS = p.icms,
                              ICMSST = p.ICMsST,
                              Tributacao = p.tributacao,
                              CstPis = p.tributacaoPIS,
                              CstCofins = p.tributacaoCOFINS,
                              AliqPis = p.pis,
                              AliqCofins = p.cofins,
                              CodigoSuspensaoPis = p.codigosuspensaopis,
                              CodigoSuspensaoCofins = p.codigosuspensaocofins,
                              CFOP = p.cfopsaida,
                              CSTIPI = p.cstipi,
                              IPI = p.aliquotaIPI,
                              MVA = p.percentualMargVlrAdICMsST,
                              RedBC = p.percentualRedBaseCalcICMS,
                              RedBCST = p.percentualRedICMsST,
                              Origem = p.origem

                          })).FirstOrDefault();

            try
            {
                Itens.Origem.Substring(0, 1);
            }
            catch
            {
                origem = "0";
            }



            int? seq = (from n in NotaFiscal.itens
                        select (int?)n.sequencia).Max() + 1;

            if (!seq.HasValue)
                seq = 1;

            itensNFe novo = new itensNFe()
            {
                sequencia = seq.Value,
                codigo = txtCodigoProduto.Text,
                descricao = Itens.Descricao,
                quantidade = Convert.ToDecimal(txtQuantidade.Text),
                preco = Convert.ToDecimal(txtPrecoItem.Text),
                total = Convert.ToDecimal(txtQuantidade.Text) * Convert.ToDecimal(txtPrecoItem.Text),
                cfop = cbCFOPItens.Text.Substring(0, 5),
                icms = Convert.ToDecimal(txtIcmsItem.Text),
                ipi = Itens.IPI.Value,
                cstipi = Itens.CSTIPI,
                csticms = origem + Itens.Tributacao,
                icmsst = Itens.ICMSST,
                redbc = Itens.RedBC,
                redbcst = Itens.RedBCST,
                mva = Itens.MVA,
                cstpis = Itens.CstPis,
                pis = Itens.AliqPis.Value,
                cstcofins = Itens.CstCofins,
                cofins = Itens.AliqCofins.Value,
            };

            NotaFiscal.itens.Add(novo);

            MontaGrid();

        }




        private void Totalizadores()
        {

            MontaGrid();
            NotaFiscal.subtotalNFe = (from n in NotaFiscal.itens
                                      select n.total).Sum();


            if (NotaFiscal.doc > 0)
            {
                NotaFiscal.descontoNFe = (from n in NotaFiscal.itens select n.desconto).Sum();
            }
            else
            {
                NotaFiscal.descontoNFe = Convert.ToDecimal("0" + txtDesconto.Text);
            }
            NotaFiscal.totalNFe = (NotaFiscal.subtotalNFe + NotaFiscal.despesasNFe + NotaFiscal.freteNFe + NotaFiscal.seguroNFe + NotaFiscal.totalSTNFe) - NotaFiscal.descontoNFe;

            txtSubTotal.Text = string.Format("{0:N2}", NotaFiscal.subtotalNFe);
            txtDesconto.Text = string.Format("{0:N2}", NotaFiscal.descontoNFe);
            txtTotalNFe.Text = string.Format("{0:N2}", NotaFiscal.totalNFe);

        }

        private void CalcularTributos()
        {
            var total = from n in NotaFiscal.itens
                        select n;

            Decimal vBCICMSnormal, vICMSnormal;
            Decimal vBCICMSST, vICMSST;
            Decimal vPIS, vIPI, vCOFINS;

            Decimal vTotBCICMSnormal, vTotICMSnormal;
            Decimal vTotBCICMSST, vTotICMSST;
            Decimal vTotPIS, vTotCOFINS, vTotIPI;
            Decimal vTotDesconto, vTotProduto, vTotNFe;

            Decimal vTotalItem, vRatSeguro, vRatFrete, vRatDesconto, vRatDespesas;

            vTotBCICMSnormal = 0;
            vTotICMSnormal = 0;
            vTotBCICMSST = 0;
            vTotICMSST = 0;
            vTotIPI = 0;
            vTotPIS = 0;
            vTotCOFINS = 0;
            vTotDesconto = 0;
            vTotProduto = 0;
            vTotNFe = 0;



            foreach (var item in NotaFiscal.itens)
            {
                //Variáveis de calculo para cada item, será inicializado a cada loop
                vBCICMSnormal = 0;
                vICMSnormal = 0;
                vBCICMSST = 0;
                vICMSST = 0;
                vPIS = 0;
                vIPI = 0;
                vCOFINS = 0;

                vTotalItem = 0;
                vRatDesconto = 0;
                vRatDespesas = 0;
                vRatSeguro = 0;
                vRatFrete = 0;


                vTotalItem = (item.preco * item.quantidade);
                vRatDesconto = (vTotalItem / NotaFiscal.totalNFe) * NotaFiscal.descontoNFe;
                vRatDespesas = (vTotalItem / NotaFiscal.totalNFe) * NotaFiscal.despesasNFe;
                vRatSeguro = (vTotalItem / NotaFiscal.totalNFe) * NotaFiscal.seguroNFe;
                vRatFrete = (vTotalItem / NotaFiscal.totalNFe) * NotaFiscal.freteNFe;

                if (item.ipi > 0)
                {
                    vIPI = (vTotalItem * item.ipi) / 100;
                }

                //Falta o qUnidIPI

                if (item.icms > 0)
                {
                    vBCICMSnormal = (vTotalItem + vRatDespesas + vRatSeguro + vRatFrete) - vRatDesconto;
                    vICMSnormal = (vBCICMSnormal * item.icms) / 100;
                }



                if (item.csticms.Equals("10") || item.csticms.Equals("110") || item.csticms.Equals("210"))
                {
                    vBCICMSST = ((vTotalItem * item.mva) / 100) + vTotalItem + vIPI;

                    vICMSST = ((vBCICMSST * item.icmsst) / 100) - vICMSnormal;

                    if (vICMSST < 0)
                    {
                        vICMSST = 0;
                    }
                }

                if (item.csticms.Equals("20") || item.csticms.Equals("120") || item.csticms.Equals("220"))
                {
                    vBCICMSnormal = vBCICMSnormal * ((100 - item.redbc) / 100);
                    vICMSnormal = vBCICMSnormal + item.icms;
                }

                if (item.csticms.Equals("70") || item.csticms.Equals("170") || item.csticms.Equals("270"))
                {
                    vBCICMSST = ((vTotalItem * item.mva) / 100) + vTotalItem + vIPI;

                    vBCICMSST = vBCICMSST * ((100 - item.redbcst) / 100);

                    vICMSST = ((vBCICMSST * item.icmsst) / 100) - vICMSnormal;

                    if (vICMSST < 0)
                    {
                        vICMSST = 0;
                    }
                }

                if (item.pis > 0)
                {
                    vPIS = (vTotalItem * item.pis) / 100;
                }

                if (item.cofins > 0)
                {
                    vCOFINS = (vTotalItem * item.cofins) / 100;
                }

                vTotProduto += vTotalItem;

                vTotBCICMSnormal += vBCICMSnormal;
                vTotBCICMSST += vICMSST;

                vTotICMSnormal += vICMSnormal;
                vTotICMSST += vICMSST;

                vTotPIS += vPIS;
                vTotCOFINS += vCOFINS;
                vTotIPI += vIPI;

                vTotDesconto += vRatDesconto;


            } //for each

            vTotNFe = (vTotProduto + (NotaFiscal.despesasNFe + NotaFiscal.seguroNFe + NotaFiscal.freteNFe) + vTotICMSST + vTotIPI) - NotaFiscal.descontoNFe;

            txtResTotBCICMS.Text = string.Format("{0:N2}", vTotBCICMSnormal);
            txtResTotBCICMSST.Text = string.Format("{0:N2}", vTotBCICMSST);

            txtResTotCOFINS.Text = string.Format("{0:N2}", vTotCOFINS);
            txtResTotPIS.Text = string.Format("{0:N2}", vTotPIS);
            txtResTotIPI.Text = string.Format("{0:N2}", vTotIPI);

            txtResTotICMS.Text = string.Format("{0:N2}", vTotICMSnormal);
            txtResTotICMSST.Text = string.Format("{0:N2}", vTotICMSST);

            txtResTotDescontos.Text = string.Format("{0:N2}", NotaFiscal.descontoNFe);

            txtResTotDespesas.Text = string.Format("{0:N2}", NotaFiscal.despesasNFe + NotaFiscal.freteNFe + NotaFiscal.seguroNFe);

            txtResTotProduto.Text = string.Format("{0:N2}", (vTotProduto));

            txtResTotNF.Text = string.Format("{0:N2}", (vTotNFe));

        }


        private void dgIitens_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)
            {
                if (dgIitens.Rows.Count > 0)
                {
                    if (!modificarItens)
                    {
                        MessageBox.Show("CFOP referenciado não é possível excluir item");
                        return;
                    }
                    RemoverItem(int.Parse(dgIitens.Rows[dgIitens.CurrentRow.Index].Cells["colunSequenciaItens"].Value.ToString()));
                    Totalizadores();
                }
            }
        }


        public string FormatarDecimal(string valor, int casaDecimal)
        {
            try
            {
                if (casaDecimal == 3)
                    return string.Format("{0:N3}", Convert.ToDecimal(valor));

                return string.Format("{0:n2}", Convert.ToDecimal(valor));

            }
            catch
            {
                return "0,00";
            }
        }



        private void dgItensICMS_CellEnter(object sender, DataGridViewCellEventArgs e)
        {


            int linha = e.RowIndex;
            txtAliquotaICMS.Text = dgItensICMS.Rows[linha].Cells["colunIcms"].Value.ToString();
            txtPercRedBCICMS.Text = dgItensICMS.Rows[linha].Cells["colunRedBaseCalculo"].Value.ToString();
            txtAlquotaICMSST.Text = dgItensICMS.Rows[linha].Cells["colunIcmsSt"].Value.ToString();
            txtPercRedBCICMSST.Text = dgItensICMS.Rows[linha].Cells["colunRedBaseCalucSt"].Value.ToString();
            txtPercMargValorAdcICMSST.Text = dgItensICMS.Rows[linha].Cells["colunMVAIcms"].Value.ToString();

            for (int i = 0; i < CST.Count(); i++)
            {
                if (CST[i].Substring(0, 3) == dgItensICMS.Rows[linha].Cells["colunCstIcms"].Value.ToString())
                {
                    cbCSTICMS.Text = CST[i];
                }
            }



            cfops = NotaFiscal.PopulaCfops(cbTipo.Text.Substring(0, 1));

            var dadosCbo = (from n in cfops
                            where n.codigo == dgItensICMS.Rows[linha].Cells["colunCfopIcms"].Value.ToString()
                            select new { n.codigo, descricao = n.codigo + " - " + n.descricao }).FirstOrDefault();

            cbCFOPICMS.Text = dadosCbo.descricao;




        }

        private void dgItensIPI_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int linha = e.RowIndex;

            for (int i = 0; i < CSTIPI.Count(); i++)
            {
                if (CSTIPI[i].Substring(0, 2) == dgItensIPI.Rows[linha].Cells["colunCstIPI"].Value.ToString())
                {
                    cbTributacaoIPI.Text = CSTIPI[i];
                    break;
                }
                else
                {
                    cbTributacaoIPI.Text = CSTIPI[0];
                }

            }

            txtAliqIPI.Text = dgItensIPI.Rows[linha].Cells["colunIPI"].Value.ToString();
        }

        private void dgItensPis_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

            int linha = e.RowIndex;

            cbCstPis.Text = dgItensPis.Rows[linha].Cells["coluncstPis"].Value.ToString();

            //int total = CstPis.Count();

            //for (int i = 0; i < total; i++)
            //{
            //    if (CstPis[i].Substring(0, 2) == dgItensPis.Rows[linha].Cells["coluncstPis"].Value.ToString())
            //    {
            //        cbCstPis.Text = CstPis[i];
            //    }




            txtAliqPis.Text = dgItensPis.Rows[linha].Cells["colunPis"].Value.ToString();
        }

        private void dgItensCofins_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int linha = e.RowIndex;

            cbCstCofins.Text = dgItensCofins.Rows[linha].Cells["colunCstCofins"].Value.ToString();

            //int total = CstPis.Count();

            //for (int i = 0; i < total; i++)
            //{
            //    if (CstPis[i].Substring(0, 2) == dgItensCofins.Rows[linha].Cells["colunCstCofins"].Value.ToString())
            //    {
            //        cbCstCofins.Text = CstPis[i];
            //    }


            //}

            txtAliqCofins.Text = dgItensCofins.Rows[linha].Cells["colunCofins"].Value.ToString();

        }

        private void AlterarItensICMS(object sender, EventArgs e)
        {


            foreach (var c in NotaFiscal.itens)
            {
                if (c.sequencia == Convert.ToInt16(dgItensICMS.CurrentRow.Cells["colunSequenciaIcms"].Value))
                {
                    c.cfop = cbCFOP.Text.Substring(0, 5);
                    c.csticms = cbCSTICMS.Text.Substring(0, 3);
                    c.icms = decimal.Parse(txtAliquotaICMS.Text.Replace(".", ",") ?? "");
                    c.icmsst = decimal.Parse(txtAlquotaICMSST.Text.Replace(".", ",") ?? "");
                    c.redbc = decimal.Parse(txtPercRedBCICMS.Text.Replace(".", ",") ?? "");
                    c.redbcst = decimal.Parse(txtPercRedBCICMSST.Text.Replace(".", ",") ?? "");
                    c.mva = decimal.Parse(txtPercMargValorAdcICMSST.Text.Replace(".", ",") ?? "");
                }
            }

            MontaGrid();
        }

        private void bntAlterarIPI_Click(object sender, EventArgs e)
        {

            try
            {


                foreach (var c in NotaFiscal.itens)
                {
                    if (c.sequencia == Convert.ToInt16(dgItensIPI.CurrentRow.Cells["colunSequenciaIPI"].Value))
                    {
                        c.ipi = decimal.Parse(txtAliqIPI.Text.Replace(".", ",") ?? "");
                        c.cstipi = cbTributacaoIPI.Text.Substring(0, 2) ?? "";
                    }
                }

                MontaGrid();

            }
            catch (Exception erro)
            {
                MessageBox.Show("Ligue para o Suporte Tecnico \n" + erro, "Mensagem Atenção");
            }

        }

        private void bntAlterarPis_Click(object sender, EventArgs e)
        {


            try
            {

                foreach (var c in NotaFiscal.itens)
                {
                    if (c.sequencia == Convert.ToInt16(dgItensPis.CurrentRow.Cells["colunSequanciaPis"].Value))
                    {
                        c.pis = decimal.Parse(txtAliqPis.Text.Replace(".", ",") ?? "");
                        c.cstpis = cbCstPis.Text.Substring(0, 2) ?? "";
                    }

                }
                MontaGrid();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Ligue para o Suporte Tecnico \n" + erro, "Mensagem Atenção");
            }

        }

        private void bntAlterarCofins_Click(object sender, EventArgs e)
        {


            try
            {


                foreach (var c in NotaFiscal.itens)
                {
                    if (c.sequencia == Convert.ToInt16(dgItensCofins.CurrentRow.Cells["colunSequenciaCofins"].Value))
                    {
                        c.cofins = decimal.Parse(txtAliqCofins.Text.Replace(".", ",") ?? "");
                        c.cstcofins = cbCstCofins.Text.Substring(0, 2) ?? "";
                    }
                }
                MontaGrid();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Ligue para o Suporte Tecnico \n" + erro, "Mensagem Atenção");
            }

        }

        private void SelectionaTipoFrete(object sender, EventArgs e)
        {
            if (cbTipoFrete.Text.Substring(0, 1) != "9")
            {

                gpDadosFrete.Enabled = true;

                PopulaTransportes();
                try
                {
                    Transportes(int.Parse(cbTransportes.SelectedValue.ToString()));
                    PopulaVeiculos(int.Parse(cbTransportes.SelectedValue.ToString()));
                    DadosVeiculos(int.Parse(cbVeiculo.SelectedValue.ToString()));
                }
                catch
                {

                }
            }
            else
            {
                gpDadosFrete.Enabled = false;
            }



        }

        private void cbTransportes_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbTransportes.Focused != false)
            {
                Transportes(int.Parse(cbTransportes.SelectedValue.ToString()));
                PopulaVeiculos(int.Parse(cbTransportes.SelectedValue.ToString()));
                DadosVeiculos(int.Parse(cbVeiculo.SelectedValue.ToString()));
            }
        }

        private void cbVeiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVeiculo.Focused != false)
            {
                DadosVeiculos(int.Parse(cbVeiculo.SelectedValue.ToString()));
            }
        }

        private void CbOBS_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CbOBS.Focused != false)
            {
                rchOBS.Text = CbOBS.Text;
            }
        }


        private void dgPagamento_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AlterarPagamento(object sender, EventArgs e)
        {
            // For do Parcelamento   
            var pagamentoCR = from n in NotaFiscal.pagamento
                              where n.tipo != "CR"
                              select n;
            NotaFiscal.pagamento = pagamentoCR.ToList();

            int parcelas = Convert.ToInt16(txtParcelamento.Text);
            decimal valor = NotaFiscal.totalNFe - (from n in NotaFiscal.pagamento select n.valor).Sum();
            DateTime vencimento = txtVencimento.Value.Date;
            NotaFiscal.Parcelamento(parcelas, valor, txtVencimento.Value.Date, Convert.ToInt16(cboIntervalo.SelectedItem));
            MontaGrid();
        }

        private void bntSair_Click(object sender, EventArgs e)
        {

            if (NotaFiscal.itens.Count > 0)
            {
                if (MessageBox.Show("NFe não foi finalizada. Deseja sair os itens serão perdidos ?", "Continua", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            this.Close();
        }




        private void PopulaNFNumeroDocumento(int documento)
        {


            FrmMsgOperador msg = new FrmMsgOperador("", "Aguarde... Gerando NF-e!");
            msg.Show();
            Application.DoEvents();

            System.Threading.Thread.Sleep(500);


            var ResumoDocumento = (from d in Entidades.contdocs
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

            if (ResumoDocumento.nfe > 0)
            {
                if (MessageBox.Show("Já foi emititda NF-e para este documento. Emitir novamente ?", "Continua", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    msg.Dispose();
                    return;
                }
            }


            if (ResumoDocumento.codigoCliente > 0)
            {
                ChamaCliente(int.Parse(ResumoDocumento.codigoCliente.ToString()));
            }

            #region
            /*

            
            var itensDocumento = (from it in Entidades.venda
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
                                      seqECF = it.Ecfnumero
                                  }).ToList();

            var itensPagamento = (from n in Conexao.CriarEntidade().caixa
                                  where n.documento == documento
                                  select new { n.Nrparcela, n.tipopagamento, n.valor, n.vencimento }).ToList();



            if (itensDocumento.Count == 0)
            {
                itensDocumento = (from it in Entidades.vendaarquivo
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
                                      seqECF = it.ecfnumero
                                  }).ToList();
                itensPagamento = (from n in Conexao.CriarEntidade().caixaarquivo
                                  where n.documento == documento
                                  select new { n.Nrparcela, n.tipopagamento, n.valor, n.vencimento }).ToList();

            }



            List<itensNFe> novo = new List<itensNFe>();

            int? seq = (from n in NotaFiscal.itens select (int?)n.sequencia).Max() + 1;
            if (!seq.HasValue)
                seq = 1;


            foreach (var item in itensDocumento)
            {
                var dados = new[]
                    {
                       new itensNFe{sequencia = seq.GetValueOrDefault(),codigo=item.codigo,descricao = item.descricao,
                           quantidade=item.quantidade,preco=item.preco.Value,desconto=item.desconto,total=item.total,cfop="5.929",
                           csticms=item.tributacaoICMS,icms=item.icms,ipi=item.aliqIPI.Value,icmsst = item.icmsST,
                           redbc = item.ResucaoICMS, redbcst = item.ReducaoICMSST, mva = item.MVA,cstipi = item.cstIPI,
                           cstpis=item.cstpis,pis=item.pis.Value,cofins=item.cofins.Value,cstcofins=item.cstcofins,
                           coo=item.coo,ccf=item.ccf,documento=item.doc,ecf=item.ecf,seqECF=item.seqECF},                           
                    };
                seq++;
                novo.AddRange(dados);
            };
            NotaFiscal.itens.AddRange(novo);

            List<PagamentoNFe> novoPag = new List<PagamentoNFe>();
            int? seqPag = (from n in NotaFiscal.pagamento select (int?)n.parcela).Max() + 1;
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
            NotaFiscal.pagamento.AddRange(novoPag);
            */
            #endregion

            NotaFiscal.MontarNotaFiscal(documento);

            var cfops = (from cfop in Entidades.nfoperacao
                         where cfop.codigo == "5.929"
                         select cfop).ToList().Count();
            if (cfops < 1)
            {
                nfoperacao _nfoperacao = new nfoperacao();

                _nfoperacao.codigo = "5.929";
                _nfoperacao.descricao = "Lanç. efetuado em decorrência emis. doc. fisc.";
                _nfoperacao.tipo = "S";
                _nfoperacao.geraicms = "N";
                _nfoperacao.tributacao = "";

                Entidades.nfoperacao.AddObject(_nfoperacao);
                Entidades.SaveChanges();
            }


            NotaFiscal.doc = documento;

            Totalizadores();
            cbTipo.Text = "1 Saida";
            populaCfops();
            cbCFOP.Text = "5.929 Lanç. efetuado em decorrência emis. doc. fisc.";
            cbCFOPItens.Text = "5.929 Lanç. efetuado em decorrência emis. doc. fisc.";
            cbCFOPICMS.Text = "5.929 Lanç. efetuado em decorrência emis. doc. fisc.";

            msg.Dispose();


        }



        private void aplicamascaras()
        {

            txtQuantidade.KeyPress += (objeto, evento) =>
            #region
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            };

            txtQuantidade.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {
                    txtQuantidade.Text = Funcoes.FormatarDecimal(txtQuantidade.Text);
                };

            };

            #endregion

            txtPrecoItem.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            };

            txtPrecoItem.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {
                    txtPrecoItem.Text = Funcoes.FormatarDecimal(txtPrecoItem.Text);
                };
            };




            txtIcmsItem.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtIcmsItem.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtIcmsItem.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion

            txtAliquotaICMS.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtAliquotaICMS.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtAliquotaICMS.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion


            txtPercRedBCICMS.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtPercRedBCICMS.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtPercRedBCICMS.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion


            txtAlquotaICMSST.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtAlquotaICMSST.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtAlquotaICMSST.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion

            txtPercRedBCICMSST.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtPercRedBCICMSST.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtPercRedBCICMSST.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion



            txtPercMargValorAdcICMSST.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtPercMargValorAdcICMSST.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtPercMargValorAdcICMSST.Text, 3, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion


            txtAliqIPI.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtAliqIPI.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtAliqIPI.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion



            txtAliqPis.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtAliqPis.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtAliqPis.Text, 3, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion




            txtAliqCofins.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtAliqCofins.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtAliqCofins.Text, 3, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion



            txtvalorFrete.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtvalorFrete.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtvalorFrete.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion




            txtValorSeguro.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtValorSeguro.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtValorSeguro.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion




            txtDespesas.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtDespesas.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtDespesas.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion



            txtVolumeFrete.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtVolumeFrete.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtVolumeFrete.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion


            txtQuantidadeFrete.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtQuantidadeFrete.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtQuantidadeFrete.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion

            txtNumeroFrete.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtNumeroFrete.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtNumeroFrete.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion


            txtPesoBruto.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtPesoBruto.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtPesoBruto.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion



            txtPesoLiquido.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtPesoLiquido.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtPesoLiquido.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion


            txtValorDH.KeyPress += (objeto, evento) =>
            #region
            {
                int conteudo = this.verifica_caracteres(txtValorDH.Text, ".", 1);
                int casa = this.VerificarCasasDecimais(txtValorDH.Text, 2, 1);

                if (conteudo >= 1)
                {

                    // if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)'.' && e.KeyChar != (char)8)
                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)8)
                    {

                        evento.Handled = true;



                    }
                    else
                    {
                        if (casa == 1 || evento.KeyChar == (char)8)
                        {
                            evento.Handled = false;
                        }
                        else
                        {
                            evento.Handled = true;
                        }
                    }
                }
                else
                {

                    if (!Char.IsDigit(evento.KeyChar) && evento.KeyChar != (char)'.' && evento.KeyChar != (char)8)
                    //if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                    {

                        evento.Handled = true;


                    }
                    else
                    {
                        evento.Handled = false;
                    }
                }
            };
            #endregion


            txtDesconto.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            };

            txtDesconto.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {
                    txtDesconto.Text = Funcoes.FormatarDecimal(txtDesconto.Text);
                    CalcularDesconto();
                    evento.SuppressKeyPress = true;
                };
            };
        }

        public int verifica_caracteres(string palavra, string caracteres, int espacamento)
        {

            palavra = palavra.Replace(" ", "");


            int resultado;
            int cont = palavra.Length;
            int quantidade = 0;



            for (int i = 0; i < cont; i = i + espacamento)
            {

                resultado = cont - i;

                if (resultado >= espacamento)
                {
                    string caracterer = palavra.Substring(i, espacamento);

                    if (caracterer == caracteres)
                    {
                        quantidade = quantidade + 1;
                    }
                }

            }

            return quantidade;


        }


        private int VerificarCasasDecimais(string palavra, int casa, int espacamento)
        {
            int cont = palavra.Length;
            int retur = 0;

            for (int i = 0; i < cont; i = i + espacamento)
            {

                int resultado = cont - i;

                if (resultado >= espacamento)
                {
                    string caracterer = palavra.Substring(i, espacamento);



                    if (caracterer == ".")
                    {
                        int cont1 = palavra.Substring(i, resultado).Length;

                        for (int d = 1; d <= cont1; d++)
                        {
                            if (d <= casa)
                            {
                                retur = 1;
                            }
                            else
                            {
                                retur = 0;
                            }
                        }
                    }
                }

            }

            return retur;
        }

        private void cbCFOP_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbCFOP.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCFOP.AutoCompleteSource = AutoCompleteSource.ListItems;

            modificarItens = true;
            if (cbCFOP.Text.Substring(0, 5) == "5.929" || cbCFOP.Text.Substring(0, 5) == "6.929")
            {
                modificarItens = false;
                bntDocumento_Click(sender, e);
            }
        }

        private void bntDocumento_Click(object sender, EventArgs e)
        {
            pnDocumento.Visible = true;
            txtNumero.Focus();
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            if (NotaFiscal.itens.Count > 0 && NotaFiscal.doc == 0)
            {
                MessageBox.Show("Finalize a NFe, não é possível emitir NFe de documento com itens em aberto");
                return;
            }

            if (txtNumero.Text.Trim() == "")
            {
                txtNumero.Focus();
            }
            else
            {

                PopulaNFNumeroDocumento(int.Parse(txtNumero.Text));
                pnDocumento.Visible = false;
                FrmDocumentos.NumeroDocumento = "";

            }
        }





        private void bntCancelar_Click(object sender, EventArgs e)
        {
            pnDocumento.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmDocumentos ObjDocumento = new FrmDocumentos();
            ObjDocumento.ShowDialog();

            txtNumero.Text = FrmDocumentos.NumeroDocumento;
        }

        private void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

            populaCfops();
            NotaFiscal.tipoNFe = cbTipo.Text.Substring(0, 1);
        }

        private void populaCfops()
        {
            cfops = NotaFiscal.PopulaCfops(cbTipo.Text.Substring(0, 1));

            var dadosCbo = (from n in cfops
                            select new { n.codigo, descricao = n.codigo + " - " + n.descricao }).ToList();

            var cfopsTransp = (from n in cfops
                               select new { n.codigo, descricao = n.codigo + " - " + n.descricao }).ToList();

            cbCFOP.DataSource = dadosCbo;
            cbCFOP.DisplayMember = "descricao";
            cbCFOP.ValueMember = "codigo";

            cbCFOPItens.DataSource = dadosCbo;
            cbCFOPItens.DisplayMember = "descricao";
            cbCFOPItens.ValueMember = "codigo";

            cbCFOPICMS.DataSource = dadosCbo;
            cbCFOPICMS.DisplayMember = "descricao";
            cbCFOPICMS.ValueMember = "codigo";

            cbCFOPfrete.DataSource = cfopsTransp;
            cbCFOPfrete.DisplayMember = "descricao";
            cbCFOPfrete.ValueMember = "codigo";

            cbCstPis.DataSource = NotaFiscal.CSTPisCofins(cbTipo.Text.Substring(0, 1) == "1" ? "S" : "E");
            cbCstCofins.DataSource = NotaFiscal.CSTPisCofins(cbTipo.Text.Substring(0, 1) == "1" ? "S" : "E");

        }

        private void bntCFOPCabecario_Click(object sender, EventArgs e)
        {
            NotaFiscal.tipoNFe = cbTipo.Text.Substring(0, 1);
            NotaFiscal.MudarCFOPItens("itens", cbCFOP.Text.Substring(0, 5));
            MontaGrid();
        }

        private void bntCFOPCadastro_Click(object sender, EventArgs e)
        {
            NotaFiscal.tipoNFe = cbTipo.Text.Substring(0, 1);
            NotaFiscal.MudarCFOPItens("cadastro", "");
            MontaGrid();
        }

        private void bntCFOPClientes_Clik(object sender, EventArgs e)
        {
            NotaFiscal.MudarCFOPItens("cliente", "");
            MontaGrid();

            //if (txtCodCliente.Text != "")
            //{
            //    int codigo = int.Parse(txtCodCliente.Text);

            //    var CfopCliente = (from CFC in Entidades.clientes
            //                       where CFC.Codigo == codigo
            //                       select new
            //                       {
            //                           Cfop = CFC.cfopnfe,
            //                           icms = CFC.icms,
            //                           icmsst = CFC.icmsst,
            //                           tributacao = CFC.csticms
            //                       }).FirstOrDefault();

            //    if (CfopCliente.Cfop.Trim() != "" || CfopCliente.icms != 0)
            //    {

            //        foreach (DataGridViewRow linhaProduto in dgIitens.Rows)
            //        {

            //            dgIitens.Rows[linhaProduto.Index].Cells[5].Value = CfopCliente.icms;
            //            dgIitens.Rows[linhaProduto.Index].Cells[6].Value = CfopCliente.Cfop;
            //            dgIitens.Rows[linhaProduto.Index].Cells[7].Value = CfopCliente.tributacao;

            //            dgItensICMS.Rows[linhaProduto.Index].Cells[2].Value = "0" + CfopCliente.tributacao;
            //            dgItensICMS.Rows[linhaProduto.Index].Cells[3].Value = CfopCliente.icms;
            //            dgItensICMS.Rows[linhaProduto.Index].Cells[4].Value = CfopCliente.icmsst;
            //            dgItensICMS.Rows[linhaProduto.Index].Cells[5].Value = CfopCliente.Cfop;

            //        }
            //    }
            //}
        }





        private void GerarNF(object sender, EventArgs e)
        {


            int Documento = 0, CodigoTransportes = 0, CodigoVeiculo = 0, QtVolume = 0, VolumeFrete = 0;
            decimal Frete = 0, VlSeguro = 0, Despesas = 0, Desconto = 0, pesoBruto = 0, pesoLiquido = 0;
            string estoque = "X";

            #region
            if (txtQuantidadeFrete.Text != "")
            {
                QtVolume = int.Parse(txtQuantidade.Text);
            }

            if (txtValorSeguro.Text != "")
            {
                VlSeguro = decimal.Parse(txtValorSeguro.Text);
            }


            if (txtVolumeFrete.Text != "")
            {
                VolumeFrete = int.Parse(txtVolumeFrete.Text);
            }

            if (txtNumero.Text != "")
            {
                Documento = int.Parse(txtNumero.Text);
            }

            if (txtvalorFrete.Text != "")
            {
                Frete = decimal.Parse(txtvalorFrete.Text);
            }

            if (cbTransportes.Text.ToString() != "")
            {
                CodigoTransportes = int.Parse(cbTransportes.SelectedValue.ToString());
            }

            if (cbVeiculo.Text.ToString() != "")
            {
                CodigoVeiculo = int.Parse(cbVeiculo.SelectedValue.ToString());
            }

            if (txtDespesas.Text != "")
            {
                Despesas = decimal.Parse(txtDespesas.Text);
            }

            if (txtDesconto.Text != "")
            {
                Desconto = Decimal.Parse(txtDesconto.Text);
            }

            if (txtPesoBruto.Text != "")
            {
                pesoBruto = Decimal.Parse(txtPesoBruto.Text);
            }

            if (txtPesoLiquido.Text != "")
            {
                pesoLiquido = Decimal.Parse(txtPesoLiquido.Text);
            }

            #endregion

            estoque = "X";

            if (string.IsNullOrEmpty(cbTipo.Text))
            {
                MessageBox.Show("Escolha o tipo 0-Entrada, 1-Sáida");
                return;
            }

            if (string.IsNullOrEmpty(cbSerie.Text))
            {
                MessageBox.Show("Escolha uma série.");
                return;
            }

            if (cbTipo.Text.PadLeft(1, ' ').Substring(0, 1) == "0")
            {
                if (MessageBox.Show("NFe de entrada. Deseja adicionar quantidades ao Estoque ?", "Atenção", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    estoque = " ";
                }
                else
                {
                    estoque = "X";
                }
            }

            try
            {
                if (lancarItens == true)
                    NotaFiscal.SalvarItensTabela();

                lancarItens = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            };

            NotaFiscal.numeroNFe = int.Parse(lblNumNota2.Text);
            NotaFiscal.modeloNFe = "55";
            NotaFiscal.cfopNFe = cbCFOP.SelectedValue.ToString().Substring(0, 5);
            NotaFiscal.cfopTransporteNFE = cbCFOPfrete.SelectedValue.ToString().Substring(0, 5);
            NotaFiscal.chaveAcessoRefNFe = "";
            NotaFiscal.colocarDataHoraNFe = "S";
            NotaFiscal.criarNF = "S";
            NotaFiscal.dadosComplementarNFe = rchOBS.Text;
            NotaFiscal.descontoNFe = Desconto; // não entendo? mas tem que implementar
            NotaFiscal.despesasNFe = Despesas;
            NotaFiscal.doc = Documento;
            NotaFiscal.especieVolumeNFe = txtEspecie.Text;
            NotaFiscal.filial = GlbVariaveis.glb_filial;
            NotaFiscal.finalidadeNFe = cbFinalidade.Text.Substring(0, 1);
            NotaFiscal.freteNFe = Frete;
            NotaFiscal.gerarICMS = GeraICMS;
            NotaFiscal.idInfoComplementarNFe = 0; // pegar id do combox da observação
            NotaFiscal.idTransportadoraNFe = CodigoTransportes;
            NotaFiscal.idVeiculoNFe = CodigoVeiculo;
            NotaFiscal.indPag = cbPagamento.Text.Substring(0, 1);
            NotaFiscal.ipTerminal = GlbVariaveis.glb_IP;
            NotaFiscal.marcavolume = txtMarcas.Text;
            NotaFiscal.naturezaOperacaoNFe = cbOperacao.Text;
            NotaFiscal.NFeEntradaAdEstoque = estoque;
            NotaFiscal.NFeOrigem = 0; // não entendo;
            NotaFiscal.operadorNFe = GlbVariaveis.glb_Usuario;
            NotaFiscal.qtdVolumeNFe = QtVolume;
            NotaFiscal.seguroNFe = VlSeguro;
            NotaFiscal.serieNFe = SerieNota;
            NotaFiscal.situacaoNFe = cbSituacaoNF.Text.Substring(0, 2);
            NotaFiscal.tipoEmissaoNFe = cbEmissao.Text.Substring(0, 1);
            NotaFiscal.tipoFreteNFe = cbTipoFrete.Text.Substring(0, 1);
            NotaFiscal.tipoNFe = cbTipo.Text.Substring(0, 1);
            NotaFiscal.volumeNFe = VolumeFrete;
            NotaFiscal.crt = Convert.ToInt16(cboRegime.Text.Substring(0, 1));
            NotaFiscal.TotalPesoBrutoNFe = pesoBruto;
            NotaFiscal.TotalPesoLiquidoNFe = pesoLiquido;


            try
            {
                int nf = NotaFiscal.GerarNFe();
                lblNumNota2.Text = nf.ToString();
                //MessageBox.Show("NFe Enviada: " + lblNumNota2.Text);
                ZerarVariaveis();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void RemoverItem(int seq)
        {
            NotaFiscal.itens.RemoveAt(seq - 1);
            MontaGrid();
        }

        private void RemoveParcelas(int parcela)
        {
            NotaFiscal.pagamento.RemoveAt(parcela - 1);
            MontaGrid();
        }

        private void cbCFOPICMS_Leave(object sender, EventArgs e)
        {
            try
            {
                NotaFiscal.VerificaCFOP(cbCFOPICMS.Text.Substring(0, 5));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cbCFOPICMS.SelectAll();
                cbCFOPICMS.Focus();
            };

        }

        private void cbCFOP_Leave(object sender, EventArgs e)
        {
            cbCFOPItens.Text = cbCFOP.Text;
            cbCFOPICMS.Text = cbCFOP.Text;
        }

        private void dgPagamento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)
            {
                if (dgPagamento.Rows.Count > 0)
                {
                    RemoveParcelas(int.Parse(dgPagamento.Rows[dgPagamento.CurrentRow.Index].Cells["colunParcela"].Value.ToString()));
                    Totalizadores();
                }
            }
        }

        private void txtDesconto_Leave(object sender, EventArgs e)
        {
            CalcularDesconto();
        }

        private void CalcularDesconto()
        {
            txtDesconto.Text = Funcoes.FormatarDecimal(txtDesconto.Text);

            if (Convert.ToDecimal(txtDesconto.Text) >= NotaFiscal.totalNFe)
            {
                txtDesconto.Text = "0.00";
                NotaFiscal.descontoNFe = 0;
            }

            NotaFiscal.descontoNFe = Convert.ToDecimal(txtDesconto.Text);
            NotaFiscal.totalNFe = NotaFiscal.subtotalNFe - NotaFiscal.descontoNFe;
            txtTotalNFe.Text = string.Format("{0:N2}", NotaFiscal.totalNFe);
        }

        private void bntLancar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtValorDH.Text))
                txtValorDH.Text = string.Format("{0:N2}", NotaFiscal.totalNFe);

            if (Convert.ToDecimal(txtValorDH.Text) > NotaFiscal.totalNFe)
                txtValorDH.Text = string.Format("{0:N2}", NotaFiscal.totalNFe);

            if (txtValorDH.Text == "")
            {
                txtValorDH.Text = string.Format("{0:N2}", NotaFiscal.totalNFe);
            }
            NotaFiscal.pagamento.Clear();
            PagamentoNFe novo = new PagamentoNFe()
            {
                tipo = "DH",
                parcela = 1,
                valor = Convert.ToDecimal("0" + txtValorDH.Text),
                vencimento = DateTime.Now.Date
            };
            NotaFiscal.pagamento.Add(novo);
            MontaGrid();
        }

        private void txtValorDH_Enter(object sender, EventArgs e)
        {
            txtValorDH.Text = string.Format("{0:N2}", NotaFiscal.totalNFe - (from n in NotaFiscal.pagamento select n.valor).Sum());
        }

        private void dgIitens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void ZerarVariaveis()
        {
            Venda.ApagarItensFormaPagamento("itenspagamentos");
            NotaFiscal.itens.Clear();
            NotaFiscal.pagamento.Clear();
            txtValorDH.Text = "0";
            txtParcelamento.Text = "1";

            txtvalorFrete.Text = "0.00";
            txtValorSeguro.Text = "0.00";
            txtDespesas.Text = "0.00";
            rchOBS.Text = "";
            CbOBS.SelectedIndex = -1;
            CbOBS.Text = "";
            Totalizadores();
            SelecionaNumerodeSerie();
        }

        private void txtvalorFrete_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtvalorFrete.Text))
                txtvalorFrete.Text = "0";

            NotaFiscal.freteNFe = Convert.ToDecimal(txtvalorFrete.Text);
            Totalizadores();
        }

        private void txtValorSeguro_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtValorSeguro.Text))
                txtValorSeguro.Text = "0";

            NotaFiscal.seguroNFe = Convert.ToDecimal(txtValorSeguro.Text);
            Totalizadores();
        }

        private void txtDespesas_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDespesas.Text))
                txtDespesas.Text = "0";

            NotaFiscal.despesasNFe = Convert.ToDecimal(txtDespesas.Text);
            Totalizadores();
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            CalcularTributos();
        }

        private void tabCupons_Enter(object sender, EventArgs e)
        {
            dtgCupons.DataSource = (from n in NotaFiscal.itens
                                    select new { n.ecf, n.coo }).Distinct().ToList();
        }

        private void FrmNotaFiscal_Load(object sender, EventArgs e)
        {
            cboRegime.Text = (from n in Conexao.CriarEntidade().filiais
                              where n.CodigoFilial == GlbVariaveis.glb_filial
                              select n.crt).FirstOrDefault();
        }

        private void cbOperacao_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbOperacao.Text == "Devolucao")
            {
                cboRegime.Enabled = true;
            }
            else
                cboRegime.Enabled = false;
        }

        private void btnEntrada_Click(object sender, EventArgs e)
        {
            if (cbTipo.Text == "")
            {
                MessageBox.Show("Informe primeiro os dados da NF.");
                return;
            }
            ImportacaoNFe import = new ImportacaoNFe();
            ImportacaoNFe.tipo = ImportacaoNFe.TipoImportacao.entrada.ToString();
            import.ShowDialog();

            if (ImportacaoNFe.numero > 0)
            {
                var itensDocumento = (from n in Conexao.CriarEntidade().entradas
                                      where n.numero == ImportacaoNFe.numero
                                      select n);

                List<itensNFe> novo = new List<itensNFe>();

                int? seq = (from n in NotaFiscal.itens select (int?)n.sequencia).Max() + 1;
                if (!seq.HasValue)
                    seq = 1;


                foreach (var item in itensDocumento)
                {
                    var dados = new[]
                    {
                       new itensNFe{sequencia = seq.GetValueOrDefault(),codigo=item.codigo,descricao = item.descricao,
                           quantidade=item.quantidade,preco=item.PrecoVenda,desconto=item.totaldesconto,total=item.totalitem,cfop=cbCFOP.Text.Substring(0,5),
                           csticms=item.tributacao,icms=item.icms,cstipi="00",ipi=item.IPI,icmsst = item.icmsst,
                           redbc = item.percentualRedBaseCalcICMS, redbcst = 0, mva = 0,                           
                           cstpis= cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           pis=item.pis,
                           cofins=item.cofins.Value,
                           cstcofins=cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           },                           
                    };
                    seq++;
                    novo.AddRange(dados);
                };
                NotaFiscal.itens.AddRange(novo);
                Totalizadores();
            }
        }

        private void btnCOF_Click(object sender, EventArgs e)
        {
            FrmCOF cof = new FrmCOF();
            cof.ShowDialog();

            if (FrmCOF.idCOF > 0)
            {
                var dadosCOF = (from n in Conexao.CriarEntidade().cof
                                where n.id == FrmCOF.idCOF
                                select n).FirstOrDefault();
                cbTipo.Text = dadosCOF.tipo == "S" ? "1" : "0";
                cbCFOP.Text = dadosCOF.cfop;
                cbSerie.Text = dadosCOF.serie;
                SelecionaNumerodeSerie();
                cbOperacao.Text = dadosCOF.operacao;
                NotaFiscal.MudarCFOPItens("COF", "");
                MontaGrid();
            }
        }

        private void btnDevCliente_Click(object sender, EventArgs e)
        {
            if (cbTipo.Text == "")
            {
                MessageBox.Show("Informe primeiro os dados da NF.");
                return;
            }
            ImportacaoNFe import = new ImportacaoNFe();
            ImportacaoNFe.tipo = ImportacaoNFe.TipoImportacao.entradaDevolucao.ToString();
            import.ShowDialog();

            if (ImportacaoNFe.numero > 0)
            {
                var itensDocumento = (from n in Conexao.CriarEntidade().devolucao
                                      where n.numero == ImportacaoNFe.numero
                                      select n);

                List<itensNFe> novo = new List<itensNFe>();

                int? seq = (from n in NotaFiscal.itens select (int?)n.sequencia).Max() + 1;
                if (!seq.HasValue)
                    seq = 1;


                foreach (var item in itensDocumento)
                {
                    var dados = new[]
                    {
                       new itensNFe{sequencia = seq.GetValueOrDefault(),codigo=item.codigo,descricao = item.produto,
                           quantidade=item.quantidade,preco=item.preco.Value,desconto=0,total=item.total,cfop=cbCFOP.Text.Substring(0,5),
                           csticms="000",icms=0,cstipi="00",ipi=0,icmsst = 0,
                           redbc = 0, redbcst = 0, mva = 0,                           
                           cstpis= cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           pis=0,
                           cofins=0,
                           cstcofins=cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           },                           
                    };
                    seq++;
                    novo.AddRange(dados);
                };
                NotaFiscal.itens.AddRange(novo);
                Totalizadores();

            }

        }

        private void btnTransfEnt_Click(object sender, EventArgs e)
        {

            if (cbTipo.Text == "")
            {
                MessageBox.Show("Informe primeiro os dados da NF.");
                return;
            }
            ImportacaoNFe import = new ImportacaoNFe();
            ImportacaoNFe.tipo = ImportacaoNFe.TipoImportacao.entradaTransf.ToString();
            import.ShowDialog();

            if (ImportacaoNFe.numero > 0)
            {
                var itensDocumento = (from n in Conexao.CriarEntidade().movtransf
                                      where n.numero == ImportacaoNFe.numero
                                      && n.Filialdestino == GlbVariaveis.glb_filial
                                      select n);

                List<itensNFe> novo = new List<itensNFe>();

                int? seq = (from n in NotaFiscal.itens select (int?)n.sequencia).Max() + 1;
                if (!seq.HasValue)
                    seq = 1;


                foreach (var item in itensDocumento)
                {
                    var dados = new[]
                    {
                       new itensNFe{sequencia = seq.GetValueOrDefault(),codigo=item.codigo,descricao = item.descricao,
                           quantidade=item.quantidade,preco=item.preco.Value,desconto=0,total=item.quantidade*item.preco.Value,cfop=cbCFOP.Text.Substring(0,5),
                           csticms="000",icms=0,cstipi="00",ipi=0,icmsst = 0,
                           redbc = 0, redbcst = 0, mva = 0,                           
                           cstpis= cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           pis=0,
                           cofins=0,
                           cstcofins=cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           },                           
                    };
                    seq++;
                    novo.AddRange(dados);
                };
                NotaFiscal.itens.AddRange(novo);
                Totalizadores();

            }

        }

        private void btnTransfSaida_Click(object sender, EventArgs e)
        {
            if (cbTipo.Text == "")
            {
                MessageBox.Show("Informe primeiro os dados da NF.");
                return;
            }
            ImportacaoNFe import = new ImportacaoNFe();
            ImportacaoNFe.tipo = ImportacaoNFe.TipoImportacao.saidaTransf.ToString();
            import.ShowDialog();

            if (ImportacaoNFe.numero > 0)
            {
                var itensDocumento = (from n in Conexao.CriarEntidade().movtransf
                                      where n.numero == ImportacaoNFe.numero
                                      && n.filialorigem == GlbVariaveis.glb_filial
                                      select n);

                List<itensNFe> novo = new List<itensNFe>();

                int? seq = (from n in NotaFiscal.itens select (int?)n.sequencia).Max() + 1;
                if (!seq.HasValue)
                    seq = 1;


                foreach (var item in itensDocumento)
                {
                    var dados = new[]
                    {
                       new itensNFe{sequencia = seq.GetValueOrDefault(),codigo=item.codigo,descricao = item.descricao,
                           quantidade=item.quantidade,preco=item.preco.Value,desconto=0,total=item.quantidade*item.preco.Value,cfop=cbCFOP.Text.Substring(0,5),
                           csticms="000",icms=0,cstipi="00",ipi=0,icmsst = 0,
                           redbc = 0, redbcst = 0, mva = 0,                           
                           cstpis= cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           pis=0,
                           cofins=0,
                           cstcofins=cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           },                           
                    };
                    seq++;
                    novo.AddRange(dados);
                };
                NotaFiscal.itens.AddRange(novo);
                Totalizadores();

            }
        }

        private void btnDevForn_Click(object sender, EventArgs e)
        {

            if (cbTipo.Text == "")
            {
                MessageBox.Show("Informe primeiro os dados da NF.");
                return;
            }
            ImportacaoNFe import = new ImportacaoNFe();
            ImportacaoNFe.tipo = ImportacaoNFe.TipoImportacao.saidaDevolucao.ToString();
            import.ShowDialog();

            if (ImportacaoNFe.numero > 0)
            {
                var itensDocumento = (from n in Conexao.CriarEntidade().produtosvencidos
                                      where n.numero == ImportacaoNFe.numero
                                      && n.numero == ImportacaoNFe.numero
                                      select n);

                List<itensNFe> novo = new List<itensNFe>();

                int? seq = (from n in NotaFiscal.itens select (int?)n.sequencia).Max() + 1;
                if (!seq.HasValue)
                    seq = 1;


                foreach (var item in itensDocumento)
                {
                    var dados = new[]
                    {
                       new itensNFe{sequencia = seq.GetValueOrDefault(),codigo=item.codigo,descricao = item.produto,
                           quantidade=item.quantidade,preco=item.custo.Value,desconto=0,total=item.quantidade*item.custo.Value,cfop=cbCFOP.Text.Substring(0,5),
                           csticms="000",icms=0,cstipi="00",ipi=0,icmsst = 0,
                           redbc = 0, redbcst = 0, mva = 0,                           
                           cstpis= cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           pis=0,
                           cofins=0,
                           cstcofins=cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           },                           
                    };
                    seq++;
                    novo.AddRange(dados);
                };
                NotaFiscal.itens.AddRange(novo);
                Totalizadores();

            }
        }

        private void btnPerdas_Click(object sender, EventArgs e)
        {

            if (cbTipo.Text == "")
            {
                MessageBox.Show("Informe primeiro os dados da NF.");
                return;
            }
            ImportacaoNFe import = new ImportacaoNFe();
            ImportacaoNFe.tipo = ImportacaoNFe.TipoImportacao.saidaPerdas.ToString();
            import.ShowDialog();

            if (ImportacaoNFe.numero > 0)
            {
                var itensDocumento = (from n in Conexao.CriarEntidade().produtosperdas
                                      where n.numero == ImportacaoNFe.numero
                                      && n.numero == ImportacaoNFe.numero
                                      select n);

                List<itensNFe> novo = new List<itensNFe>();

                int? seq = (from n in NotaFiscal.itens select (int?)n.sequencia).Max() + 1;
                if (!seq.HasValue)
                    seq = 1;


                foreach (var item in itensDocumento)
                {
                    var dados = new[]
                    {
                       new itensNFe{sequencia = seq.GetValueOrDefault(),codigo=item.codigo,descricao = item.produto,
                           quantidade=item.quantidade,preco=item.custo.Value,desconto=0,total=item.quantidade*item.custo.Value,cfop=cbCFOP.Text.Substring(0,5),
                           csticms="000",icms=0,cstipi="00",ipi=0,icmsst = 0,
                           redbc = 0, redbcst = 0, mva = 0,                           
                           cstpis= cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           pis=0,
                           cofins=0,
                           cstcofins=cbTipo.Text.Substring(0,1) == "0" ? "50" : "01",
                           },                           
                    };
                    seq++;
                    novo.AddRange(dados);
                };
                NotaFiscal.itens.AddRange(novo);
                Totalizadores();

            }
        }
    }

}

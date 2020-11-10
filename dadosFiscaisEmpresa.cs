using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SICEpdv
{
    public abstract class dadosFiscaisEmpresa
    {
        public DateTime dataInicial { get; set; }
        public DateTime dataFinal { get; set; }
        public string codPais { get; set; }                   

        public string nomeEmpresa { get; set; }
        public string cnpj { get; set; }
        public string cpf { get; set; }
        public string uf { get; set; }
        public string ie { get; set; }
        public string cod_mun { get; set; }
        public string im { get; set; } //Inscrição Municipal
        public string suframa { get; set; }
        public string NIRE { get; set; } // NIRE Número de Identificação do Registro da Empresa da Junta Comercial
        public string ind_ativ { get; set; }
        public string fantasia { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string end { get; set; }
        public string num { get; set; }
        public string comp { get; set; }
        public string bairro { get; set; }
        public string fone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string contato { get; set; }
        public string responsavel { get; set; }
        public string cpfresponsavel { get; set; }
        // Contador
        public string nomeContador { get; set; }
        public string cpfContador { get; set; }
        public string CRC { get; set; }
        public string cnpjContador { get; set; }
        public string cepContador { get; set; }
        public string endContador { get; set; }
        public string EndNumeroContador { get; set; }
        public string complementoContador { get; set; }
        public string bairroContador { get; set; }
        public string foneContador { get; set; }
        public string faxContador { get; set; }
        public string emailContador { get; set; }     
    }
}

public struct ModeloDocFiscal
{
    public string modeloDocFiscal;
    public string tipo;
}
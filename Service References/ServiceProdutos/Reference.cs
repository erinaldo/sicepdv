//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SICEpdv.ServiceProdutos {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Produtos", Namespace="http://schemas.datacontract.org/2004/07/WCFSice")]
    [System.SerializableAttribute()]
    public partial class Produtos : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigofilialField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal depositoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string descricaoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal disponivelField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal prateleirasField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal precominimoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal precovendaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal quantidadeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ultcompraField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ultvendaField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigo {
            get {
                return this.codigoField;
            }
            set {
                if ((object.ReferenceEquals(this.codigoField, value) != true)) {
                    this.codigoField = value;
                    this.RaisePropertyChanged("codigo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigofilial {
            get {
                return this.codigofilialField;
            }
            set {
                if ((object.ReferenceEquals(this.codigofilialField, value) != true)) {
                    this.codigofilialField = value;
                    this.RaisePropertyChanged("codigofilial");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal deposito {
            get {
                return this.depositoField;
            }
            set {
                if ((this.depositoField.Equals(value) != true)) {
                    this.depositoField = value;
                    this.RaisePropertyChanged("deposito");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string descricao {
            get {
                return this.descricaoField;
            }
            set {
                if ((object.ReferenceEquals(this.descricaoField, value) != true)) {
                    this.descricaoField = value;
                    this.RaisePropertyChanged("descricao");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal disponivel {
            get {
                return this.disponivelField;
            }
            set {
                if ((this.disponivelField.Equals(value) != true)) {
                    this.disponivelField = value;
                    this.RaisePropertyChanged("disponivel");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int id {
            get {
                return this.idField;
            }
            set {
                if ((this.idField.Equals(value) != true)) {
                    this.idField = value;
                    this.RaisePropertyChanged("id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal prateleiras {
            get {
                return this.prateleirasField;
            }
            set {
                if ((this.prateleirasField.Equals(value) != true)) {
                    this.prateleirasField = value;
                    this.RaisePropertyChanged("prateleiras");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal precominimo {
            get {
                return this.precominimoField;
            }
            set {
                if ((this.precominimoField.Equals(value) != true)) {
                    this.precominimoField = value;
                    this.RaisePropertyChanged("precominimo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal precovenda {
            get {
                return this.precovendaField;
            }
            set {
                if ((this.precovendaField.Equals(value) != true)) {
                    this.precovendaField = value;
                    this.RaisePropertyChanged("precovenda");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal quantidade {
            get {
                return this.quantidadeField;
            }
            set {
                if ((this.quantidadeField.Equals(value) != true)) {
                    this.quantidadeField = value;
                    this.RaisePropertyChanged("quantidade");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ultcompra {
            get {
                return this.ultcompraField;
            }
            set {
                if ((this.ultcompraField.Equals(value) != true)) {
                    this.ultcompraField = value;
                    this.RaisePropertyChanged("ultcompra");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ultvenda {
            get {
                return this.ultvendaField;
            }
            set {
                if ((this.ultvendaField.Equals(value) != true)) {
                    this.ultvendaField = value;
                    this.RaisePropertyChanged("ultvenda");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PosicaoFiliais", Namespace="http://schemas.datacontract.org/2004/07/WCFSice")]
    [System.SerializableAttribute()]
    public partial class PosicaoFiliais : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal custoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal depositoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string descricaoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string filialField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string grupoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal prateleirasField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal precoVendaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal qtdMatrizField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal quantidadeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal somaField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigo {
            get {
                return this.codigoField;
            }
            set {
                if ((object.ReferenceEquals(this.codigoField, value) != true)) {
                    this.codigoField = value;
                    this.RaisePropertyChanged("codigo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal custo {
            get {
                return this.custoField;
            }
            set {
                if ((this.custoField.Equals(value) != true)) {
                    this.custoField = value;
                    this.RaisePropertyChanged("custo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal deposito {
            get {
                return this.depositoField;
            }
            set {
                if ((this.depositoField.Equals(value) != true)) {
                    this.depositoField = value;
                    this.RaisePropertyChanged("deposito");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string descricao {
            get {
                return this.descricaoField;
            }
            set {
                if ((object.ReferenceEquals(this.descricaoField, value) != true)) {
                    this.descricaoField = value;
                    this.RaisePropertyChanged("descricao");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string filial {
            get {
                return this.filialField;
            }
            set {
                if ((object.ReferenceEquals(this.filialField, value) != true)) {
                    this.filialField = value;
                    this.RaisePropertyChanged("filial");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string grupo {
            get {
                return this.grupoField;
            }
            set {
                if ((object.ReferenceEquals(this.grupoField, value) != true)) {
                    this.grupoField = value;
                    this.RaisePropertyChanged("grupo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal prateleiras {
            get {
                return this.prateleirasField;
            }
            set {
                if ((this.prateleirasField.Equals(value) != true)) {
                    this.prateleirasField = value;
                    this.RaisePropertyChanged("prateleiras");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal precoVenda {
            get {
                return this.precoVendaField;
            }
            set {
                if ((this.precoVendaField.Equals(value) != true)) {
                    this.precoVendaField = value;
                    this.RaisePropertyChanged("precoVenda");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal qtdMatriz {
            get {
                return this.qtdMatrizField;
            }
            set {
                if ((this.qtdMatrizField.Equals(value) != true)) {
                    this.qtdMatrizField = value;
                    this.RaisePropertyChanged("qtdMatriz");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal quantidade {
            get {
                return this.quantidadeField;
            }
            set {
                if ((this.quantidadeField.Equals(value) != true)) {
                    this.quantidadeField = value;
                    this.RaisePropertyChanged("quantidade");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal soma {
            get {
                return this.somaField;
            }
            set {
                if ((this.somaField.Equals(value) != true)) {
                    this.somaField = value;
                    this.RaisePropertyChanged("soma");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Faturamento", Namespace="http://schemas.datacontract.org/2004/07/WCFSice")]
    [System.SerializableAttribute()]
    public partial class Faturamento : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string descricaoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string filialField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal lucroOperativoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal margemLucroField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int mediaRenovacaoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal totalCustosField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal totalQuantidadeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal totalVendasField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string descricao {
            get {
                return this.descricaoField;
            }
            set {
                if ((object.ReferenceEquals(this.descricaoField, value) != true)) {
                    this.descricaoField = value;
                    this.RaisePropertyChanged("descricao");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string filial {
            get {
                return this.filialField;
            }
            set {
                if ((object.ReferenceEquals(this.filialField, value) != true)) {
                    this.filialField = value;
                    this.RaisePropertyChanged("filial");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal lucroOperativo {
            get {
                return this.lucroOperativoField;
            }
            set {
                if ((this.lucroOperativoField.Equals(value) != true)) {
                    this.lucroOperativoField = value;
                    this.RaisePropertyChanged("lucroOperativo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal margemLucro {
            get {
                return this.margemLucroField;
            }
            set {
                if ((this.margemLucroField.Equals(value) != true)) {
                    this.margemLucroField = value;
                    this.RaisePropertyChanged("margemLucro");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int mediaRenovacao {
            get {
                return this.mediaRenovacaoField;
            }
            set {
                if ((this.mediaRenovacaoField.Equals(value) != true)) {
                    this.mediaRenovacaoField = value;
                    this.RaisePropertyChanged("mediaRenovacao");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal totalCustos {
            get {
                return this.totalCustosField;
            }
            set {
                if ((this.totalCustosField.Equals(value) != true)) {
                    this.totalCustosField = value;
                    this.RaisePropertyChanged("totalCustos");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal totalQuantidade {
            get {
                return this.totalQuantidadeField;
            }
            set {
                if ((this.totalQuantidadeField.Equals(value) != true)) {
                    this.totalQuantidadeField = value;
                    this.RaisePropertyChanged("totalQuantidade");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal totalVendas {
            get {
                return this.totalVendasField;
            }
            set {
                if ((this.totalVendasField.Equals(value) != true)) {
                    this.totalVendasField = value;
                    this.RaisePropertyChanged("totalVendas");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProdutosContador", Namespace="http://schemas.datacontract.org/2004/07/WCFSice")]
    [System.SerializableAttribute()]
    public partial class ProdutosContador : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string cestField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string cfopsaidaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string cnpjField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigobarrasField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigosuspensaocofinsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codigosuspensaopisField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double cofinsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string descricaoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double icmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ncmField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string origemField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double pisField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string situacaoinventarioField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string tributacaoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string tributacaocofinsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string tributacaopisField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string cest {
            get {
                return this.cestField;
            }
            set {
                if ((object.ReferenceEquals(this.cestField, value) != true)) {
                    this.cestField = value;
                    this.RaisePropertyChanged("cest");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string cfopsaida {
            get {
                return this.cfopsaidaField;
            }
            set {
                if ((object.ReferenceEquals(this.cfopsaidaField, value) != true)) {
                    this.cfopsaidaField = value;
                    this.RaisePropertyChanged("cfopsaida");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string cnpj {
            get {
                return this.cnpjField;
            }
            set {
                if ((object.ReferenceEquals(this.cnpjField, value) != true)) {
                    this.cnpjField = value;
                    this.RaisePropertyChanged("cnpj");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigo {
            get {
                return this.codigoField;
            }
            set {
                if ((object.ReferenceEquals(this.codigoField, value) != true)) {
                    this.codigoField = value;
                    this.RaisePropertyChanged("codigo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigobarras {
            get {
                return this.codigobarrasField;
            }
            set {
                if ((object.ReferenceEquals(this.codigobarrasField, value) != true)) {
                    this.codigobarrasField = value;
                    this.RaisePropertyChanged("codigobarras");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigosuspensaocofins {
            get {
                return this.codigosuspensaocofinsField;
            }
            set {
                if ((object.ReferenceEquals(this.codigosuspensaocofinsField, value) != true)) {
                    this.codigosuspensaocofinsField = value;
                    this.RaisePropertyChanged("codigosuspensaocofins");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string codigosuspensaopis {
            get {
                return this.codigosuspensaopisField;
            }
            set {
                if ((object.ReferenceEquals(this.codigosuspensaopisField, value) != true)) {
                    this.codigosuspensaopisField = value;
                    this.RaisePropertyChanged("codigosuspensaopis");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double cofins {
            get {
                return this.cofinsField;
            }
            set {
                if ((this.cofinsField.Equals(value) != true)) {
                    this.cofinsField = value;
                    this.RaisePropertyChanged("cofins");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string descricao {
            get {
                return this.descricaoField;
            }
            set {
                if ((object.ReferenceEquals(this.descricaoField, value) != true)) {
                    this.descricaoField = value;
                    this.RaisePropertyChanged("descricao");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double icms {
            get {
                return this.icmsField;
            }
            set {
                if ((this.icmsField.Equals(value) != true)) {
                    this.icmsField = value;
                    this.RaisePropertyChanged("icms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ncm {
            get {
                return this.ncmField;
            }
            set {
                if ((object.ReferenceEquals(this.ncmField, value) != true)) {
                    this.ncmField = value;
                    this.RaisePropertyChanged("ncm");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string origem {
            get {
                return this.origemField;
            }
            set {
                if ((object.ReferenceEquals(this.origemField, value) != true)) {
                    this.origemField = value;
                    this.RaisePropertyChanged("origem");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double pis {
            get {
                return this.pisField;
            }
            set {
                if ((this.pisField.Equals(value) != true)) {
                    this.pisField = value;
                    this.RaisePropertyChanged("pis");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string situacaoinventario {
            get {
                return this.situacaoinventarioField;
            }
            set {
                if ((object.ReferenceEquals(this.situacaoinventarioField, value) != true)) {
                    this.situacaoinventarioField = value;
                    this.RaisePropertyChanged("situacaoinventario");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string tributacao {
            get {
                return this.tributacaoField;
            }
            set {
                if ((object.ReferenceEquals(this.tributacaoField, value) != true)) {
                    this.tributacaoField = value;
                    this.RaisePropertyChanged("tributacao");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string tributacaocofins {
            get {
                return this.tributacaocofinsField;
            }
            set {
                if ((object.ReferenceEquals(this.tributacaocofinsField, value) != true)) {
                    this.tributacaocofinsField = value;
                    this.RaisePropertyChanged("tributacaocofins");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string tributacaopis {
            get {
                return this.tributacaopisField;
            }
            set {
                if ((object.ReferenceEquals(this.tributacaopisField, value) != true)) {
                    this.tributacaopisField = value;
                    this.RaisePropertyChanged("tributacaopis");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceProdutos.IWSProdutos")]
    public interface IWSProdutos {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWSProdutos/ListagemProdutos", ReplyAction="http://tempuri.org/IWSProdutos/ListagemProdutosResponse")]
        SICEpdv.ServiceProdutos.Produtos[] ListagemProdutos(string credenciaisDB, string contrato, string filial, string pesquisa);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWSProdutos/PosicaoFiliais", ReplyAction="http://tempuri.org/IWSProdutos/PosicaoFiliaisResponse")]
        SICEpdv.ServiceProdutos.PosicaoFiliais[] PosicaoFiliais(string credenciaisDB, string contrato, string idProduto, string filial);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWSProdutos/FaturamentoFiliais", ReplyAction="http://tempuri.org/IWSProdutos/FaturamentoFiliaisResponse")]
        SICEpdv.ServiceProdutos.Faturamento[] FaturamentoFiliais(string credenciaisDB);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWSProdutos/IncluirProdutosContador", ReplyAction="http://tempuri.org/IWSProdutos/IncluirProdutosContadorResponse")]
        bool IncluirProdutosContador(string assinatura, string codigofilial, string cnpj, string nomeEmpresa, string fantasia, string tipoEmpresa, SICEpdv.ServiceProdutos.ProdutosContador[] dados);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWSProdutos/IncluirContador", ReplyAction="http://tempuri.org/IWSProdutos/IncluirContadorResponse")]
        bool IncluirContador(string assinatura, int idCliente, string filial, string nomeContador, string email, string crc, string cnpj, string nomeEmpresa, string fantaria, string tipoEmpresa, string cnae);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWSProdutos/ExcluirProdutosContador", ReplyAction="http://tempuri.org/IWSProdutos/ExcluirProdutosContadorResponse")]
        bool ExcluirProdutosContador(string assinatura, string cnpj);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWSProdutos/ListagemProdutosContadorCNPJ", ReplyAction="http://tempuri.org/IWSProdutos/ListagemProdutosContadorCNPJResponse")]
        SICEpdv.ServiceProdutos.ProdutosContador[] ListagemProdutosContadorCNPJ(string cnpj, string procura, int limite);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWSProdutosChannel : SICEpdv.ServiceProdutos.IWSProdutos, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WSProdutosClient : System.ServiceModel.ClientBase<SICEpdv.ServiceProdutos.IWSProdutos>, SICEpdv.ServiceProdutos.IWSProdutos {
        
        public WSProdutosClient() {
        }
        
        public WSProdutosClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WSProdutosClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSProdutosClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSProdutosClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SICEpdv.ServiceProdutos.Produtos[] ListagemProdutos(string credenciaisDB, string contrato, string filial, string pesquisa) {
            return base.Channel.ListagemProdutos(credenciaisDB, contrato, filial, pesquisa);
        }
        
        public SICEpdv.ServiceProdutos.PosicaoFiliais[] PosicaoFiliais(string credenciaisDB, string contrato, string idProduto, string filial) {
            return base.Channel.PosicaoFiliais(credenciaisDB, contrato, idProduto, filial);
        }
        
        public SICEpdv.ServiceProdutos.Faturamento[] FaturamentoFiliais(string credenciaisDB) {
            return base.Channel.FaturamentoFiliais(credenciaisDB);
        }
        
        public bool IncluirProdutosContador(string assinatura, string codigofilial, string cnpj, string nomeEmpresa, string fantasia, string tipoEmpresa, SICEpdv.ServiceProdutos.ProdutosContador[] dados) {
            return base.Channel.IncluirProdutosContador(assinatura, codigofilial, cnpj, nomeEmpresa, fantasia, tipoEmpresa, dados);
        }
        
        public bool IncluirContador(string assinatura, int idCliente, string filial, string nomeContador, string email, string crc, string cnpj, string nomeEmpresa, string fantaria, string tipoEmpresa, string cnae) {
            return base.Channel.IncluirContador(assinatura, idCliente, filial, nomeContador, email, crc, cnpj, nomeEmpresa, fantaria, tipoEmpresa, cnae);
        }
        
        public bool ExcluirProdutosContador(string assinatura, string cnpj) {
            return base.Channel.ExcluirProdutosContador(assinatura, cnpj);
        }
        
        public SICEpdv.ServiceProdutos.ProdutosContador[] ListagemProdutosContadorCNPJ(string cnpj, string procura, int limite) {
            return base.Channel.ListagemProdutosContadorCNPJ(cnpj, procura, limite);
        }
    }
}

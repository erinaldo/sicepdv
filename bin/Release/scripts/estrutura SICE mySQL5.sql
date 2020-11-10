/*
SQLyog Enterprise v8.71 
MySQL - 5.1.34-community : Database - sice
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
/*Table structure for table `60a` */

CREATE TABLE `60a` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `tipo` char(2) NOT NULL DEFAULT '60',
  `subtipo` char(1) NOT NULL DEFAULT 'A',
  `data` date DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `ECFnumeroserie` varchar(20) DEFAULT NULL,
  `aliquotaICMS` varchar(4) DEFAULT NULL,
  `acumuladoTotalizadorParcial` decimal(12,2) NOT NULL DEFAULT '0.00',
  `codigofilial` varchar(5) NOT NULL DEFAULT '00001',
  `ecfnumero` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=1557 DEFAULT CHARSET=latin1;

/*Table structure for table `60m` */

CREATE TABLE `60m` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `tipo` char(2) NOT NULL DEFAULT '60',
  `subtipo` char(1) NOT NULL DEFAULT 'M',
  `data` date DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `ECFnumeroserie` varchar(20) DEFAULT NULL,
  `ECFnumero` varchar(4) DEFAULT NULL,
  `modeloDocFiscal` char(2) NOT NULL DEFAULT '2D',
  `contadorinicial` varchar(10) NOT NULL DEFAULT '0',
  `contadorfinal` varchar(10) NOT NULL DEFAULT '0',
  `numeroreducaoZ` varchar(10) NOT NULL DEFAULT '0',
  `contadorreinicio` varchar(10) NOT NULL DEFAULT '0',
  `vendabruta` decimal(16,2) NOT NULL DEFAULT '0.00',
  `totalgeralECF` decimal(16,2) NOT NULL DEFAULT '0.00',
  `gtinicialdia` decimal(13,2) NOT NULL DEFAULT '0.00',
  `vendaliquida` decimal(13,2) NOT NULL DEFAULT '0.00',
  `TotalICMSdebitado` decimal(13,2) NOT NULL DEFAULT '0.00',
  `ValorICMS` decimal(12,2) NOT NULL DEFAULT '0.00',
  `codigofilial` varchar(5) NOT NULL DEFAULT '00001',
  `origem` varchar(15) NOT NULL DEFAULT 'ECF',
  `totaldescontos` decimal(12,2) NOT NULL DEFAULT '0.00',
  `totalcancelamentos` decimal(12,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=86 DEFAULT CHARSET=latin1;

/*Table structure for table `abastecimento` */

CREATE TABLE `abastecimento` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `value` int(1) DEFAULT '0',
  `totaldinheiro` decimal(10,2) DEFAULT '0.00',
  `totallitros` decimal(10,2) DEFAULT '0.00',
  `pu` decimal(10,3) DEFAULT '0.000',
  `tempo` varchar(10) DEFAULT NULL,
  `canal` char(2) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `hora` varchar(10) DEFAULT NULL,
  `registro` int(11) DEFAULT NULL,
  `stfull` varchar(55) DEFAULT NULL,
  `encerrante` double DEFAULT NULL,
  `integridade` int(1) DEFAULT '0',
  `checksum` int(1) DEFAULT '0',
  `documento` int(10) DEFAULT '0',
  `finalizado` char(1) DEFAULT 'N',
  `finalizadopor` varchar(10) DEFAULT NULL,
  `ip` varchar(20) DEFAULT NULL,
  `selecionado` char(1) DEFAULT 'N',
  `codigofilial` varchar(5) DEFAULT NULL,
  `itemindex` char(3) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `acerto` */

CREATE TABLE `acerto` (
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `qtdanterior` decimal(10,2) DEFAULT NULL,
  `qtdnova` decimal(10,2) DEFAULT NULL,
  `diferenca` decimal(10,2) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `usuario` varchar(10) DEFAULT NULL,
  `observacao` text,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `origem` varchar(15) NOT NULL DEFAULT 'produtos',
  `notafiscal` int(6) NOT NULL DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `acessooperadores` */

CREATE TABLE `acessooperadores` (
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `ip` varchar(15) DEFAULT NULL,
  `usuarioBD` varchar(30) DEFAULT NULL,
  `mac` varchar(40) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `agenda` */

CREATE TABLE `agenda` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `usuario` varchar(20) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `hora` varchar(8) DEFAULT NULL,
  `resumo` varchar(60) DEFAULT NULL,
  `texto` text,
  `acao` varchar(30) DEFAULT NULL,
  `status` varchar(10) DEFAULT NULL,
  `tipo` varchar(20) DEFAULT NULL,
  `visibilidade` varchar(20) DEFAULT NULL,
  `diasaviso` int(3) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;

/*Table structure for table `agenda_contatos` */

CREATE TABLE `agenda_contatos` (
  `cont_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `cont_nome` varchar(150) NOT NULL DEFAULT '',
  `cont_obs` text,
  `cont_ddd` char(2) NOT NULL DEFAULT '',
  `cont_tel` varchar(9) NOT NULL DEFAULT '',
  `cont_cel` varchar(9) DEFAULT NULL,
  `cont_email` varchar(64) NOT NULL DEFAULT '',
  `cont_blog` varchar(255) DEFAULT NULL,
  `cont_msn` varchar(64) DEFAULT NULL,
  `cont_gtalk` varchar(64) DEFAULT NULL,
  `cont_skype` varchar(32) DEFAULT NULL,
  `cont_data_cad` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`cont_id`),
  KEY `cont_nome` (`cont_nome`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COMMENT='Cadastros da minha agenda de contatos.';

/*Table structure for table `agendacontactos` */

CREATE TABLE `agendacontactos` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(15) DEFAULT NULL,
  `usuario` varchar(30) DEFAULT NULL,
  `nome` varchar(90) DEFAULT NULL,
  `telefone` varchar(30) DEFAULT NULL,
  `telefoneComercial` varchar(30) DEFAULT NULL,
  `Celular` varchar(30) DEFAULT NULL,
  `email` varchar(180) DEFAULT NULL,
  `tipo` char(3) DEFAULT NULL,
  `informacoes` text,
  `data` date DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

/*Table structure for table `aniversarios` */

CREATE TABLE `aniversarios` (
  `id` int(5) NOT NULL AUTO_INCREMENT,
  `nome` varchar(200) NOT NULL DEFAULT '',
  `email` varchar(200) NOT NULL DEFAULT '',
  `aniversario` date NOT NULL DEFAULT '0000-00-00',
  `idade` int(3) DEFAULT NULL,
  `telefone` varchar(15) DEFAULT NULL,
  `usuario` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `areas` */

CREATE TABLE `areas` (
  `Codigofilial` char(5) DEFAULT NULL,
  `codigo` char(4) DEFAULT NULL,
  `descricao` char(25) DEFAULT NULL,
  `percentual` decimal(6,2) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `assistenciaprodutos` */

CREATE TABLE `assistenciaprodutos` (
  `codigo` int(5) NOT NULL AUTO_INCREMENT,
  `nome` varchar(50) DEFAULT NULL,
  `endereco` varchar(50) DEFAULT NULL,
  `bairro` varchar(20) DEFAULT NULL,
  `cep` varchar(8) DEFAULT NULL,
  `cidade` varchar(20) DEFAULT NULL,
  `estado` char(2) NOT NULL DEFAULT 'PE',
  `especialidade` varchar(50) DEFAULT NULL,
  `cnpjcpf` varchar(18) DEFAULT NULL,
  `telefone1` varchar(15) DEFAULT NULL,
  `telefone2` varchar(15) DEFAULT NULL,
  `telefone3` varchar(15) DEFAULT NULL,
  `fax` varchar(15) DEFAULT NULL,
  `contactos` varchar(70) DEFAULT NULL,
  `email` varchar(60) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `observacao` text,
  PRIMARY KEY (`codigo`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `atualizacoes` */

CREATE TABLE `atualizacoes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `produto` varchar(60) NOT NULL DEFAULT '',
  `descricao` text,
  `versao` varchar(10) DEFAULT NULL,
  `caminho` varchar(60) DEFAULT NULL,
  `data` date DEFAULT '0000-00-00',
  `hora` time DEFAULT '00:00:00',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `auditoria` */

CREATE TABLE `auditoria` (
  `usuario` varchar(10) DEFAULT NULL,
  `usuariosolicitante` varchar(10) DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `data` date DEFAULT NULL,
  `tabela` varchar(15) DEFAULT NULL,
  `acao` text,
  `documento` int(6) DEFAULT NULL,
  `observacao` text,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `local` varchar(100) DEFAULT NULL,
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `codigoproduto` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=65581 DEFAULT CHARSET=latin1;

/*Table structure for table `balanco` */

CREATE TABLE `balanco` (
  `inc` int(7) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `numero` int(6) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `usuario` varchar(10) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `quantidadeatual` decimal(10,2) NOT NULL DEFAULT '0.00',
  `diferenca` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdprateleiras` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtddeposito` decimal(10,2) NOT NULL DEFAULT '0.00',
  `encerrado` char(1) NOT NULL DEFAULT 'N',
  `horaencerramento` time DEFAULT NULL,
  `dataencerramento` date DEFAULT NULL,
  `codigobarras` varchar(20) DEFAULT NULL,
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;

/*Table structure for table `bancos` */

CREATE TABLE `bancos` (
  `codigo` int(5) DEFAULT NULL,
  `descricao` char(15) DEFAULT NULL,
  `CodigoFilial` char(5) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `banners` */

CREATE TABLE `banners` (
  `id` tinyint(3) NOT NULL AUTO_INCREMENT,
  `nome` varchar(50) DEFAULT '0',
  `url` varchar(100) DEFAULT '0',
  `imagem` varchar(100) DEFAULT '0',
  `alt` varchar(50) DEFAULT '0',
  `cliques` int(6) DEFAULT '0',
  `data_clique` varchar(10) DEFAULT '0',
  `total_exibicoes` int(6) DEFAULT '0',
  `data_add` varchar(10) DEFAULT '0',
  UNIQUE KEY `id` (`id`),
  KEY `id_2` (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `bico` */

CREATE TABLE `bico` (
  `inc` int(3) NOT NULL AUTO_INCREMENT,
  `canal` char(2) DEFAULT NULL,
  `indiceautomacao` int(5) DEFAULT NULL,
  `ordem` int(3) NOT NULL DEFAULT '0',
  `codigoproduto` varchar(20) NOT NULL DEFAULT '',
  `precovenda` decimal(10,3) NOT NULL DEFAULT '0.000',
  `legenda` varchar(20) DEFAULT NULL,
  `tanque` int(5) DEFAULT '0',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `bombas` */

CREATE TABLE `bombas` (
  `inc` int(3) NOT NULL AUTO_INCREMENT,
  `bombanumero` char(3) DEFAULT NULL,
  `ordem` int(3) NOT NULL DEFAULT '0',
  `codigoproduto` varchar(20) DEFAULT NULL,
  `precovenda` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `legenda` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `caixa` */

CREATE TABLE `caixa` (
  `id` int(8) NOT NULL AUTO_INCREMENT,
  `horaabertura` time DEFAULT NULL,
  `EnderecoIP` varchar(15) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `tipopagamento` char(2) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dataexe` date DEFAULT NULL,
  `data` date DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `VrJuros` decimal(10,2) NOT NULL DEFAULT '0.00',
  `jurosch` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vrdesconto` decimal(8,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) NOT NULL DEFAULT '000',
  `datapagamento` date DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `sequencia` int(10) DEFAULT NULL,
  `caixa` decimal(10,2) DEFAULT NULL,
  `financeira` char(3) DEFAULT NULL,
  `CrInicial` decimal(12,2) NOT NULL DEFAULT '0.00',
  `CrFinal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `banco` varchar(15) DEFAULT NULL,
  `cheque` int(10) DEFAULT NULL,
  `agencia` varchar(8) DEFAULT NULL,
  `valorCheque` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Cartao` varchar(15) DEFAULT NULL,
  `numeroCartao` varchar(25) DEFAULT NULL,
  `Nrparcela` varchar(5) DEFAULT NULL,
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `NomeCheque` varchar(40) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `historico` varchar(25) NOT NULL DEFAULT '*',
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `custos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `ocorrencia` varchar(250) DEFAULT NULL,
  `filialorigem` varchar(5) NOT NULL DEFAULT '',
  `valortarifabloquete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `cobrador` char(3) DEFAULT NULL,
  `contacorrentecheque` varchar(15) DEFAULT NULL,
  `jurosfactoring` decimal(6,4) NOT NULL DEFAULT '0.0000',
  `versao` varchar(8) DEFAULT NULL,
  `valorservicos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descontoservicos` decimal(8,2) NOT NULL DEFAULT '0.00',
  `jurosCA` decimal(10,2) NOT NULL DEFAULT '0.00',
  `cpfcnpjch` varchar(14) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `gnf` varchar(6) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  `estornado` varchar(1) DEFAULT NULL,
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `ecfmodelo` varchar(20) DEFAULT NULL,
  `eaddados` varchar(33) DEFAULT NULL,
  `ecfnumero` varchar(3) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IndDocData` (`data`),
  KEY `IndDocCaixa` (`documento`)
) ENGINE=MyISAM AUTO_INCREMENT=971 DEFAULT CHARSET=latin1;

/*Table structure for table `caixaarquivo` */

CREATE TABLE `caixaarquivo` (
  `id` int(8) DEFAULT NULL,
  `horaabertura` time DEFAULT NULL,
  `EnderecoIP` varchar(15) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `tipopagamento` char(2) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dataexe` date DEFAULT NULL,
  `data` date DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `VrJuros` decimal(10,2) NOT NULL DEFAULT '0.00',
  `jurosch` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vrdesconto` decimal(8,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) NOT NULL DEFAULT '000',
  `datapagamento` date DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `sequencia` int(10) DEFAULT NULL,
  `caixa` decimal(10,2) DEFAULT NULL,
  `financeira` char(3) DEFAULT NULL,
  `CrInicial` decimal(12,2) NOT NULL DEFAULT '0.00',
  `CrFinal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `banco` varchar(15) DEFAULT NULL,
  `cheque` int(10) DEFAULT NULL,
  `agencia` varchar(8) DEFAULT NULL,
  `valorCheque` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Cartao` varchar(15) DEFAULT NULL,
  `numeroCartao` varchar(25) DEFAULT NULL,
  `Nrparcela` varchar(5) DEFAULT NULL,
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `NomeCheque` varchar(40) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `historico` varchar(25) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `custos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `ocorrencia` varchar(250) DEFAULT NULL,
  `filialorigem` varchar(5) NOT NULL DEFAULT '',
  `valortarifabloquete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `cobrador` char(3) DEFAULT NULL,
  `contacorrentecheque` varchar(15) DEFAULT NULL,
  `jurosfactoring` decimal(6,4) NOT NULL DEFAULT '0.0000',
  `versao` varchar(8) DEFAULT NULL,
  `valorservicos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descontoservicos` decimal(8,2) NOT NULL DEFAULT '0.00',
  `jurosCA` decimal(10,2) NOT NULL DEFAULT '0.00',
  `cpfcnpjch` varchar(14) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `gnf` varchar(6) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  `estornado` varchar(1) DEFAULT NULL,
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `ecfmodelo` varchar(20) DEFAULT NULL,
  `eaddados` varchar(33) DEFAULT NULL,
  `ecfnumero` varchar(3) DEFAULT NULL,
  KEY `data` (`data`),
  KEY `documento` (`documento`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `caixadav` */

CREATE TABLE `caixadav` (
  `id` int(8) NOT NULL AUTO_INCREMENT,
  `horaabertura` time DEFAULT NULL,
  `EnderecoIP` varchar(15) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `tipopagamento` char(2) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dataexe` date DEFAULT NULL,
  `data` date DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `VrJuros` decimal(10,2) NOT NULL DEFAULT '0.00',
  `jurosch` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vrdesconto` decimal(8,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `datapagamento` date DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `sequencia` int(10) DEFAULT NULL,
  `caixa` decimal(10,2) DEFAULT NULL,
  `financeira` char(3) DEFAULT NULL,
  `CrInicial` decimal(12,2) NOT NULL DEFAULT '0.00',
  `CrFinal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `banco` varchar(15) DEFAULT NULL,
  `cheque` int(10) DEFAULT NULL,
  `agencia` varchar(8) DEFAULT NULL,
  `valorCheque` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Cartao` varchar(15) DEFAULT NULL,
  `numeroCartao` varchar(25) DEFAULT NULL,
  `Nrparcela` varchar(5) DEFAULT NULL,
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `NomeCheque` varchar(40) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `historico` varchar(25) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `custos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `ocorrencia` varchar(250) DEFAULT NULL,
  `filialorigem` varchar(5) NOT NULL DEFAULT '',
  `valortarifabloquete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `cobrador` char(3) DEFAULT NULL,
  `contacorrentecheque` varchar(15) DEFAULT NULL,
  `jurosfactoring` decimal(6,4) NOT NULL DEFAULT '0.0000',
  `versao` varchar(8) DEFAULT NULL,
  `valorservicos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descontoservicos` decimal(8,2) NOT NULL DEFAULT '0.00',
  `jurosCA` decimal(10,2) NOT NULL DEFAULT '0.00',
  `cpfcnpjch` varchar(14) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `gnf` varchar(6) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  `estornado` varchar(1) DEFAULT NULL,
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `ecfmodelo` varchar(20) DEFAULT NULL,
  `eaddados` varchar(33) DEFAULT NULL,
  `ecfnumero` varchar(3) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=477 DEFAULT CHARSET=latin1;

/*Table structure for table `caixadavos` */

CREATE TABLE `caixadavos` (
  `id` int(8) NOT NULL AUTO_INCREMENT,
  `horaabertura` time DEFAULT NULL,
  `EnderecoIP` varchar(15) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `tipopagamento` char(2) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dataexe` date DEFAULT NULL,
  `data` date DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `VrJuros` decimal(10,2) NOT NULL DEFAULT '0.00',
  `jurosch` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vrdesconto` decimal(8,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `datapagamento` date DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `sequencia` int(10) DEFAULT NULL,
  `caixa` decimal(10,2) DEFAULT NULL,
  `financeira` char(3) DEFAULT NULL,
  `CrInicial` decimal(12,2) NOT NULL DEFAULT '0.00',
  `CrFinal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `banco` varchar(15) DEFAULT NULL,
  `cheque` int(10) DEFAULT NULL,
  `agencia` varchar(8) DEFAULT NULL,
  `valorCheque` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Cartao` varchar(15) DEFAULT NULL,
  `numeroCartao` varchar(25) DEFAULT NULL,
  `Nrparcela` varchar(5) DEFAULT NULL,
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `NomeCheque` varchar(40) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `historico` varchar(25) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `custos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `ocorrencia` varchar(250) DEFAULT NULL,
  `filialorigem` varchar(5) NOT NULL DEFAULT '',
  `valortarifabloquete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `cobrador` char(3) DEFAULT NULL,
  `contacorrentecheque` varchar(15) DEFAULT NULL,
  `jurosfactoring` decimal(6,4) NOT NULL DEFAULT '0.0000',
  `versao` varchar(8) DEFAULT NULL,
  `valorservicos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descontoservicos` decimal(8,2) NOT NULL DEFAULT '0.00',
  `jurosCA` decimal(10,2) NOT NULL DEFAULT '0.00',
  `cpfcnpjch` varchar(14) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `gnf` varchar(6) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  `estornado` varchar(1) DEFAULT NULL,
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `ecfmodelo` varchar(20) DEFAULT NULL,
  `eaddados` varchar(33) DEFAULT NULL,
  `ecfnumero` varchar(3) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=275 DEFAULT CHARSET=latin1;

/*Table structure for table `caixaprevenda` */

CREATE TABLE `caixaprevenda` (
  `id` int(8) DEFAULT NULL,
  `horaabertura` time DEFAULT NULL,
  `EnderecoIP` varchar(15) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `tipopagamento` char(2) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dataexe` date DEFAULT NULL,
  `data` date DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `VrJuros` decimal(10,2) NOT NULL DEFAULT '0.00',
  `jurosch` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vrdesconto` decimal(8,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `datapagamento` date DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `sequencia` int(10) DEFAULT NULL,
  `caixa` decimal(10,2) DEFAULT NULL,
  `financeira` char(3) DEFAULT NULL,
  `CrInicial` decimal(12,2) NOT NULL DEFAULT '0.00',
  `CrFinal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `banco` varchar(15) DEFAULT NULL,
  `cheque` int(10) DEFAULT NULL,
  `agencia` varchar(8) DEFAULT NULL,
  `valorCheque` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Cartao` varchar(15) DEFAULT NULL,
  `numeroCartao` varchar(25) DEFAULT NULL,
  `Nrparcela` varchar(5) DEFAULT NULL,
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `NomeCheque` varchar(40) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `historico` varchar(25) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `custos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `ocorrencia` varchar(250) DEFAULT NULL,
  `filialorigem` varchar(5) NOT NULL DEFAULT '',
  `valortarifabloquete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `cobrador` char(3) DEFAULT NULL,
  `contacorrentecheque` varchar(15) DEFAULT NULL,
  `jurosfactoring` decimal(6,4) NOT NULL DEFAULT '0.0000',
  `versao` varchar(8) DEFAULT NULL,
  `valorservicos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descontoservicos` decimal(8,2) NOT NULL DEFAULT '0.00',
  `jurosCA` decimal(10,2) NOT NULL DEFAULT '0.00',
  `cpfcnpjch` varchar(14) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `gnf` varchar(6) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  `estornado` varchar(1) DEFAULT NULL,
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `ecfmodelo` varchar(20) DEFAULT NULL,
  `eaddados` varchar(33) DEFAULT NULL,
  `ecfnumero` varchar(3) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `caixaprevendapaf` */

CREATE TABLE `caixaprevendapaf` (
  `id` int(8) NOT NULL AUTO_INCREMENT,
  `horaabertura` time DEFAULT NULL,
  `EnderecoIP` varchar(15) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `tipopagamento` char(2) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dataexe` date DEFAULT NULL,
  `data` date DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `VrJuros` decimal(10,2) NOT NULL DEFAULT '0.00',
  `jurosch` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vrdesconto` decimal(8,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `datapagamento` date DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `sequencia` int(10) DEFAULT NULL,
  `caixa` decimal(10,2) DEFAULT NULL,
  `financeira` char(3) DEFAULT NULL,
  `CrInicial` decimal(12,2) NOT NULL DEFAULT '0.00',
  `CrFinal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `banco` varchar(15) DEFAULT NULL,
  `cheque` int(10) DEFAULT NULL,
  `agencia` varchar(8) DEFAULT NULL,
  `valorCheque` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Cartao` varchar(15) DEFAULT NULL,
  `numeroCartao` varchar(25) DEFAULT NULL,
  `Nrparcela` varchar(5) DEFAULT NULL,
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `NomeCheque` varchar(40) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `historico` varchar(25) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `custos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `ocorrencia` varchar(250) DEFAULT NULL,
  `filialorigem` varchar(5) NOT NULL DEFAULT '',
  `valortarifabloquete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `cobrador` char(3) DEFAULT NULL,
  `contacorrentecheque` varchar(15) DEFAULT NULL,
  `jurosfactoring` decimal(6,4) NOT NULL DEFAULT '0.0000',
  `versao` varchar(8) DEFAULT NULL,
  `valorservicos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descontoservicos` decimal(8,2) NOT NULL DEFAULT '0.00',
  `jurosCA` decimal(10,2) NOT NULL DEFAULT '0.00',
  `cpfcnpjch` varchar(14) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `gnf` varchar(6) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  `estornado` varchar(1) DEFAULT NULL,
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `ecfmodelo` varchar(20) DEFAULT NULL,
  `eaddados` varchar(33) DEFAULT NULL,
  `ecfnumero` varchar(3) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=39 DEFAULT CHARSET=latin1;

/*Table structure for table `caixas` */

CREATE TABLE `caixas` (
  `id` int(8) NOT NULL AUTO_INCREMENT,
  `horaabertura` time DEFAULT NULL,
  `EnderecoIP` varchar(15) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `tipopagamento` char(2) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dataexe` date DEFAULT NULL,
  `data` date DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `VrJuros` decimal(10,2) NOT NULL DEFAULT '0.00',
  `jurosch` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vrdesconto` decimal(8,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) NOT NULL DEFAULT '000',
  `datapagamento` date DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `sequencia` int(10) DEFAULT NULL,
  `caixa` decimal(10,2) DEFAULT NULL,
  `financeira` char(3) DEFAULT NULL,
  `CrInicial` decimal(12,2) NOT NULL DEFAULT '0.00',
  `CrFinal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `banco` varchar(15) DEFAULT NULL,
  `cheque` int(10) DEFAULT NULL,
  `agencia` varchar(8) DEFAULT NULL,
  `valorCheque` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Cartao` varchar(15) DEFAULT NULL,
  `numeroCartao` varchar(25) DEFAULT NULL,
  `Nrparcela` varchar(5) DEFAULT NULL,
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `NomeCheque` varchar(40) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `historico` varchar(25) NOT NULL DEFAULT '*',
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `custos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `ocorrencia` varchar(250) DEFAULT NULL,
  `filialorigem` varchar(5) NOT NULL DEFAULT '',
  `valortarifabloquete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `cobrador` char(3) DEFAULT NULL,
  `contacorrentecheque` varchar(15) DEFAULT NULL,
  `jurosfactoring` decimal(6,4) NOT NULL DEFAULT '0.0000',
  `versao` varchar(8) DEFAULT NULL,
  `valorservicos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descontoservicos` decimal(8,2) NOT NULL DEFAULT '0.00',
  `jurosCA` decimal(10,2) NOT NULL DEFAULT '0.00',
  `cpfcnpjch` varchar(14) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `gnf` varchar(6) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  `estornado` varchar(1) DEFAULT NULL,
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `ecfmodelo` varchar(20) DEFAULT NULL,
  `eaddados` varchar(33) DEFAULT NULL,
  `ecfnumero` varchar(3) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=1817 DEFAULT CHARSET=latin1;

/*Table structure for table `caixassoma` */

CREATE TABLE `caixassoma` (
  `horaabertura` time DEFAULT NULL,
  `horafechamento` time DEFAULT NULL,
  `inc` bigint(10) NOT NULL AUTO_INCREMENT,
  `saldo` decimal(14,2) NOT NULL DEFAULT '0.00',
  `dinheiro` decimal(14,2) NOT NULL DEFAULT '0.00',
  `entradadh` decimal(14,2) NOT NULL DEFAULT '0.00',
  `entradach` decimal(14,2) NOT NULL DEFAULT '0.00',
  `entradaca` decimal(14,2) NOT NULL DEFAULT '0.00',
  `entradafi` decimal(14,2) NOT NULL DEFAULT '0.00',
  `cheque` decimal(14,2) NOT NULL DEFAULT '0.00',
  `chequepre` decimal(14,2) NOT NULL DEFAULT '0.00',
  `chequefi` decimal(14,2) NOT NULL DEFAULT '0.00',
  `chequefipre` decimal(14,2) NOT NULL DEFAULT '0.00',
  `cartao` decimal(14,2) NOT NULL DEFAULT '0.00',
  `recebimento` decimal(14,2) NOT NULL DEFAULT '0.00',
  `recebimentodh` decimal(14,2) NOT NULL DEFAULT '0.00',
  `recebimentoch` decimal(14,2) NOT NULL DEFAULT '0.00',
  `recebimentoca` decimal(14,2) NOT NULL DEFAULT '0.00',
  `recebimentobl` decimal(14,2) NOT NULL DEFAULT '0.00',
  `recebimentodc` decimal(14,2) NOT NULL DEFAULT '0.00',
  `crediario` decimal(14,2) NOT NULL DEFAULT '0.00',
  `emprestimoDH` decimal(14,2) NOT NULL DEFAULT '0.00',
  `emprestimoCH` decimal(14,2) NOT NULL DEFAULT '0.00',
  `encargos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `encargosrecebidos` decimal(12,2) NOT NULL DEFAULT '0.00',
  `sangria` decimal(14,2) NOT NULL DEFAULT '0.00',
  `vendas` decimal(14,2) NOT NULL DEFAULT '0.00',
  `SaldoCaixa` decimal(14,2) NOT NULL DEFAULT '0.00',
  `custos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `juros` decimal(14,2) NOT NULL DEFAULT '0.00',
  `jurosrecch` decimal(12,2) NOT NULL DEFAULT '0.00',
  `devolucao` decimal(14,2) NOT NULL DEFAULT '0.00',
  `devolucaocr` decimal(10,2) NOT NULL DEFAULT '0.00',
  `devolucaoprd` decimal(10,2) NOT NULL DEFAULT '0.00',
  `renegociacao` decimal(14,2) NOT NULL DEFAULT '0.00',
  `perdao` decimal(14,2) NOT NULL DEFAULT '0.00',
  `descontovenda` decimal(14,2) NOT NULL DEFAULT '0.00',
  `descontoreceb` decimal(14,2) NOT NULL DEFAULT '0.00',
  `descontorecebjuros` decimal(14,2) NOT NULL DEFAULT '0.00',
  `crediariocr` decimal(14,2) NOT NULL DEFAULT '0.00',
  `jurosperdao` decimal(14,2) NOT NULL DEFAULT '0.00',
  `jurosrenegociacao` decimal(14,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `ocorrencia` varchar(250) DEFAULT NULL,
  `diferenca` decimal(12,2) NOT NULL DEFAULT '0.00',
  `financiamento` decimal(14,2) NOT NULL DEFAULT '0.00',
  `qtdcupons` int(7) NOT NULL DEFAULT '0',
  `suprimento` decimal(14,2) NOT NULL DEFAULT '0.00',
  `dpfinanceiro` varchar(15) DEFAULT NULL,
  `receitas` decimal(14,2) NOT NULL DEFAULT '0.00',
  `financeira` decimal(14,2) NOT NULL DEFAULT '0.00',
  `ip` varchar(15) DEFAULT NULL,
  `compraTI` decimal(14,2) NOT NULL DEFAULT '0.00',
  `trocaCH` decimal(14,2) NOT NULL DEFAULT '0.00',
  `ticket` decimal(14,2) NOT NULL DEFAULT '0.00',
  `entradati` decimal(14,2) NOT NULL DEFAULT '0.00',
  `valorservicos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `descontoservicos` decimal(14,2) NOT NULL DEFAULT '0.00',
  `descontocapitalrn` decimal(14,2) NOT NULL DEFAULT '0.00',
  `crediarioservicosCR` decimal(14,2) NOT NULL DEFAULT '0.00',
  `jurosrecca` decimal(12,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=1122 DEFAULT CHARSET=latin1;

/*Table structure for table `cartoes` */

CREATE TABLE `cartoes` (
  `id` int(3) NOT NULL AUTO_INCREMENT,
  `codigo` int(5) DEFAULT NULL,
  `descricao` varchar(15) DEFAULT NULL,
  `taxaadministracao` decimal(6,2) DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `tipo` char(2) NOT NULL DEFAULT 'CA',
  `rede` int(2) NOT NULL DEFAULT '0',
  `DescricaoPgtCupom` varchar(15) NOT NULL DEFAULT 'Cartao',
  `tipopagamento` char(2) NOT NULL DEFAULT 'CR',
  `pathreq` varchar(100) NOT NULL DEFAULT '',
  `pathresp` varchar(100) NOT NULL DEFAULT '',
  `transacao` varchar(8) NOT NULL DEFAULT 'CRT',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=15 DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001` */

CREATE TABLE `cbd001` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdcUF` smallint(6) DEFAULT NULL,
  `CbdnatOp` varchar(60) DEFAULT NULL,
  `CbdindPag` smallint(6) DEFAULT NULL,
  `Cbdmod` char(2) DEFAULT NULL,
  `CbddEmi` datetime DEFAULT NULL,
  `CbddSaiEnt` datetime DEFAULT NULL,
  `CbdtpNf` smallint(6) DEFAULT NULL,
  `CbdcMunFg` int(11) DEFAULT NULL,
  `CbdtpImp` smallint(6) DEFAULT NULL,
  `CbdtpEmis` smallint(6) DEFAULT NULL,
  `CbdfinNFe` smallint(6) DEFAULT NULL,
  `CbdCNPJ_emit` varchar(14) DEFAULT NULL,
  `CbdCPF_emit` varchar(11) DEFAULT NULL,
  `CbdxNome` varchar(60) DEFAULT NULL,
  `CbdxFant` varchar(60) DEFAULT NULL,
  `CbdxLgr` varchar(60) DEFAULT NULL,
  `Cbdnro` varchar(60) DEFAULT NULL,
  `CbdxCpl` varchar(60) DEFAULT NULL,
  `CbdxBairro` varchar(60) DEFAULT NULL,
  `CbdcMun` int(11) DEFAULT NULL,
  `CbdxMun` varchar(60) DEFAULT NULL,
  `CbdUF` char(2) DEFAULT NULL,
  `CbdCEP` int(11) DEFAULT NULL,
  `CbdcPais` smallint(6) DEFAULT NULL,
  `CbdxPais` varchar(60) DEFAULT NULL,
  `Cbdfone` bigint(14) DEFAULT NULL,
  `CbdIE` varchar(14) DEFAULT NULL,
  `CbdIEST` varchar(14) DEFAULT NULL,
  `CbdIM` varchar(11) DEFAULT NULL,
  `CbdCNAE` varchar(7) DEFAULT NULL,
  `CbdCNPJ_fisco` varchar(14) DEFAULT NULL,
  `CbdxOrgao` varchar(60) DEFAULT NULL,
  `Cbdmatr` varchar(60) DEFAULT NULL,
  `CbdxAgente` varchar(60) DEFAULT NULL,
  `Cbdfone_fisco` bigint(14) DEFAULT NULL,
  `CbdUF_fisco` char(2) DEFAULT NULL,
  `CbdnDAR` varchar(60) DEFAULT NULL,
  `CbddEmi_fisco` datetime DEFAULT NULL,
  `CbdvDAR` decimal(16,6) DEFAULT NULL,
  `CbdrepEmi` varchar(60) DEFAULT NULL,
  `CbddPag` datetime DEFAULT NULL,
  `CbdCNPJ_dest` varchar(14) DEFAULT NULL,
  `CbdCPF_dest` varchar(11) DEFAULT NULL,
  `CbdxNome_dest` varchar(60) DEFAULT NULL,
  `CbdxLgr_dest` varchar(60) DEFAULT NULL,
  `CbdxEmail_dest` varchar(60) DEFAULT NULL,
  `Cbdnro_dest` varchar(60) DEFAULT NULL,
  `CbdxCpl_dest` varchar(60) DEFAULT NULL,
  `CbdxBairro_dest` varchar(60) DEFAULT NULL,
  `CbdcMun_dest` int(11) DEFAULT NULL,
  `CbdxMun_dest` varchar(60) DEFAULT NULL,
  `CbdUF_dest` char(2) DEFAULT NULL,
  `CbdCEP_dest` int(11) DEFAULT NULL,
  `CbdcPais_dest` smallint(6) DEFAULT NULL,
  `CbdxPais_dest` varchar(60) DEFAULT NULL,
  `Cbdfone_dest` bigint(14) DEFAULT NULL,
  `CbdIE_dest` varchar(14) DEFAULT NULL,
  `CbdISUF` varchar(9) DEFAULT NULL,
  `CbdCNPJ_ret` varchar(14) DEFAULT NULL,
  `CbdxLgr_ret` varchar(60) DEFAULT NULL,
  `Cbdnro_ret` varchar(60) DEFAULT NULL,
  `CbdxCpl_ret` varchar(60) DEFAULT NULL,
  `CbdxBairro_ret` varchar(60) DEFAULT NULL,
  `CbdcMun_ret` int(11) DEFAULT NULL,
  `CbdxMun_ret` varchar(60) DEFAULT NULL,
  `CbdUF_ret` char(2) DEFAULT NULL,
  `CbdCNPJ_entr` varchar(14) DEFAULT NULL,
  `CbdxLgr_entr` varchar(60) DEFAULT NULL,
  `Cbdnro_entr` varchar(60) DEFAULT NULL,
  `CbdxBairro_entr` varchar(60) DEFAULT NULL,
  `CbdcMun_entr` int(11) DEFAULT NULL,
  `CbdxMun_entr` varchar(60) DEFAULT NULL,
  `CbdUF_entr` char(2) DEFAULT NULL,
  `CbdvBC_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvICMS_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvBCST_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvST_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvProd_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvFrete_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvSeg_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvDesc_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvII_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvIPI_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvPIS_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvCOFINS_ttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvOutro` decimal(16,6) DEFAULT NULL,
  `CbdvNF` decimal(16,6) DEFAULT NULL,
  `CbdvServ` decimal(16,6) DEFAULT NULL,
  `CbdvBC_ttlnfe_iss` decimal(16,6) DEFAULT NULL,
  `CbdvISS` decimal(16,6) DEFAULT NULL,
  `CbdvPIS_servttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvCOFINS_servttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvRetPIS` decimal(16,6) DEFAULT NULL,
  `CbdvRetCOFINS_servttlnfe` decimal(16,6) DEFAULT NULL,
  `CbdvRetCSLL` decimal(16,6) DEFAULT NULL,
  `CbdvBCIRRF` decimal(16,6) DEFAULT NULL,
  `CbdvIRRF` decimal(16,6) DEFAULT NULL,
  `CbdvBCRetPrev` decimal(16,6) DEFAULT NULL,
  `CbdvRetPrev` decimal(16,6) DEFAULT NULL,
  `CbdmodFrete` smallint(6) DEFAULT NULL,
  `CbdCNPJ_transp` varchar(14) DEFAULT NULL,
  `CbdCPF_transp` varchar(11) DEFAULT NULL,
  `CbdxNome_transp` varchar(60) DEFAULT NULL,
  `CbdIE_transp` varchar(14) DEFAULT NULL,
  `CbdxEnder` varchar(60) DEFAULT NULL,
  `CbdxMun_transp` varchar(60) DEFAULT NULL,
  `CbdUF_transp` char(2) DEFAULT NULL,
  `CbdvServ_transp` decimal(16,6) DEFAULT NULL,
  `CbdvBCRet` decimal(16,6) DEFAULT NULL,
  `CbdpICMSRet` decimal(7,2) DEFAULT NULL,
  `CbdvICMSRet` decimal(16,6) DEFAULT NULL,
  `CbdCFOP_transp` smallint(6) DEFAULT NULL,
  `CbdcMunFG_transp` int(11) DEFAULT NULL,
  `Cbdplaca` varchar(8) DEFAULT NULL,
  `CbdUF_veictransp` char(2) DEFAULT NULL,
  `CbdRNTC` varchar(20) DEFAULT NULL,
  `CbdnFat` varchar(60) DEFAULT NULL,
  `CbdvOrig` decimal(16,6) DEFAULT NULL,
  `CbdvDesc_cob` decimal(16,6) DEFAULT NULL,
  `CbdvLiq` decimal(16,6) DEFAULT NULL,
  `CbdinfAdFisco` varchar(200) DEFAULT NULL,
  `CbdinfCpl` text,
  `CbdUFEmbarq` char(2) DEFAULT NULL,
  `CbdxLocEmbarq` varchar(60) DEFAULT NULL,
  `CbdxNEmp` varchar(17) DEFAULT NULL,
  `CbdxPed` varchar(60) DEFAULT NULL,
  `CbdxCont` varchar(60) DEFAULT NULL,
  `CbdxCpl_entr` varchar(60) DEFAULT NULL,
  `CbdUsuImpPadrao` varchar(60) DEFAULT NULL,
  `CbdhrSaiEnt` varchar(8) DEFAULT NULL,
  `CbdUsuModDANFE` smallint(6) DEFAULT NULL,
  `CbdFax` bigint(14) DEFAULT NULL,
  `CbdCRT` int(1) DEFAULT '3',
  `CbdEmail_dest` varchar(60) DEFAULT NULL,
  `CbdVagao` varchar(20) DEFAULT NULL,
  `CbdBalsa` varchar(20) DEFAULT NULL,
  `CbdCPF_ret` varchar(11) DEFAULT NULL,
  `CbdCPF_entr` varchar(11) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfNumero`,`CbdNtfSerie`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001det` */

CREATE TABLE `cbd001det` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnItem` smallint(6) NOT NULL DEFAULT '0',
  `CbdcProd` varchar(60) DEFAULT NULL,
  `CbdcEAN` varchar(14) DEFAULT NULL,
  `CbdxProd` varchar(120) DEFAULT NULL,
  `CbdNCM` varchar(8) DEFAULT NULL,
  `CbdEXTIPI` char(3) DEFAULT NULL,
  `Cbdgenero` smallint(6) DEFAULT NULL,
  `CbdCFOP` smallint(6) DEFAULT NULL,
  `CbduCOM` varchar(6) DEFAULT NULL,
  `CbdqCOM` decimal(15,4) DEFAULT NULL,
  `CbdvUnCom` decimal(21,6) DEFAULT NULL,
  `CbdvProd` decimal(16,6) DEFAULT NULL,
  `CbdcEANTrib` varchar(14) DEFAULT NULL,
  `CbduTrib` varchar(6) DEFAULT NULL,
  `CbdqTrib` decimal(15,4) DEFAULT NULL,
  `CbdvUnTrib` decimal(21,6) DEFAULT NULL,
  `CbdvFrete` decimal(16,6) DEFAULT NULL,
  `CbdvSeg` decimal(16,6) DEFAULT NULL,
  `CbdvDesc` decimal(16,6) DEFAULT NULL,
  `CbdtpOp` smallint(6) DEFAULT NULL,
  `Cbdchassi` varchar(17) DEFAULT NULL,
  `CbdcCor` varchar(4) DEFAULT NULL,
  `CbdxCor` varchar(40) DEFAULT NULL,
  `Cbdpot` varchar(4) DEFAULT NULL,
  `CbdCM3` varchar(4) DEFAULT NULL,
  `CbdPesoL` varchar(9) DEFAULT NULL,
  `CbdPesoB` varchar(9) DEFAULT NULL,
  `CbdnSerie` varchar(9) DEFAULT NULL,
  `CbdtpComb` varchar(2) DEFAULT NULL,
  `CbdnMotor` varchar(21) DEFAULT NULL,
  `CbdCMKG` varchar(9) DEFAULT NULL,
  `Cbddist` varchar(4) DEFAULT NULL,
  `CbdRENAVAM` varchar(9) DEFAULT NULL,
  `CbdanoMod` smallint(6) DEFAULT NULL,
  `CbdanoFab` smallint(6) DEFAULT NULL,
  `CbdtpPint` char(1) DEFAULT NULL,
  `CbdtpVeic` smallint(6) DEFAULT NULL,
  `CbdespVeic` smallint(6) DEFAULT NULL,
  `CbdVIN` char(1) DEFAULT NULL,
  `CbdcondVeic` smallint(6) DEFAULT NULL,
  `CbdcMod` int(11) DEFAULT NULL,
  `CbdcProdANP` int(11) DEFAULT NULL,
  `CbdCODIF` varchar(21) DEFAULT NULL,
  `CbdqTemp` decimal(17,6) DEFAULT NULL,
  `CbdqBCprod` decimal(17,6) DEFAULT NULL,
  `CbdvAliqProd` decimal(17,6) DEFAULT NULL,
  `CbdvCIDE` decimal(17,6) DEFAULT NULL,
  `CbdvBCICMS` decimal(16,6) DEFAULT NULL,
  `CbdvICMS` decimal(16,6) DEFAULT NULL,
  `CbdvBCICMSST` decimal(16,6) DEFAULT NULL,
  `CbdvICMSST` decimal(16,6) DEFAULT NULL,
  `CbdvBCICMSSTDest` decimal(16,6) DEFAULT NULL,
  `CbdvICMSSTDest` decimal(16,6) DEFAULT NULL,
  `CbdvBCICMSSTCons` decimal(16,6) DEFAULT NULL,
  `CbdvICMSSTCons` decimal(16,6) DEFAULT NULL,
  `CbdUFcons` smallint(6) DEFAULT NULL,
  `CbdclEnq` varchar(5) DEFAULT NULL,
  `CbdCNPJProd` varchar(14) DEFAULT NULL,
  `CbdcSelo` varchar(200) DEFAULT NULL,
  `CbdqSelo` decimal(17,6) DEFAULT NULL,
  `CbdcEnq` char(3) DEFAULT NULL,
  `CbdvBC_imp` decimal(16,6) DEFAULT NULL,
  `CbdvDespAdu` decimal(16,6) DEFAULT NULL,
  `CbdvII` decimal(16,6) DEFAULT NULL,
  `CbdvIOF` decimal(16,6) DEFAULT NULL,
  `CbdvBC_issqn` decimal(16,6) DEFAULT NULL,
  `CbdvAliq` decimal(7,2) DEFAULT NULL,
  `CbdvISSQN` decimal(16,6) DEFAULT NULL,
  `CbdcMunFg_issqn` int(11) DEFAULT NULL,
  `CbdcListServ` smallint(6) DEFAULT NULL,
  `CbdnTipoItem` smallint(6) DEFAULT NULL,
  `CbdinfAdProd` varchar(200) DEFAULT NULL,
  `CbdIndTot` int(1) DEFAULT '1',
  `CbdvOutro` decimal(16,6) DEFAULT NULL,
  `CbdCilin` varchar(4) DEFAULT NULL,
  `CbdCMT` varchar(9) DEFAULT NULL,
  `CbdcCorDENATRAN` char(2) DEFAULT NULL,
  `Cbdlota` int(3) DEFAULT NULL,
  `CbdtpRest` int(1) DEFAULT NULL,
  `CbdcSitTrib` char(1) DEFAULT NULL,
  `CbdxPed_item` varchar(15) DEFAULT NULL,
  `CbdnItemPed` int(6) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnItem`),
  KEY `ICBD` (`CbdEmpCodigo`,`CbdNtfNumero`,`CbdNtfSerie`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001detadicoes` */

CREATE TABLE `cbd001detadicoes` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnItem` smallint(6) NOT NULL DEFAULT '0',
  `CbdnAdicao` smallint(6) NOT NULL DEFAULT '0',
  `CbdnSeqAdic` smallint(6) DEFAULT NULL,
  `CbdcFabricante` varchar(60) DEFAULT NULL,
  `CbdvDescDI` decimal(16,6) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnItem`,`CbdnAdicao`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001detarma` */

CREATE TABLE `cbd001detarma` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnItem` smallint(6) NOT NULL DEFAULT '0',
  `CbdnSerie_arma` int(11) NOT NULL DEFAULT '0',
  `CbdtpArma` smallint(6) DEFAULT NULL,
  `CbdnCano` int(11) DEFAULT NULL,
  `Cbddescr` varchar(200) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnItem`,`CbdnSerie_arma`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001detcofins` */

CREATE TABLE `cbd001detcofins` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnItem` smallint(6) NOT NULL DEFAULT '0',
  `CbdCST_cofins` smallint(6) NOT NULL DEFAULT '0',
  `CbdvBC_cofins` decimal(16,6) DEFAULT NULL,
  `CbdpCOFINS` decimal(7,2) DEFAULT NULL,
  `CbdvCOFINS` decimal(16,6) DEFAULT NULL,
  `CbdqBCProd_cofins` decimal(17,6) DEFAULT NULL,
  `CbdvAliqProd_cofins` decimal(17,6) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnItem`,`CbdCST_cofins`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001detdi` */

CREATE TABLE `cbd001detdi` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnItem` smallint(6) NOT NULL DEFAULT '0',
  `CbdnDI` varchar(10) NOT NULL DEFAULT '',
  `CbddDi` datetime DEFAULT NULL,
  `CbdxLocDesemb` varchar(60) DEFAULT NULL,
  `CbdUFDesemb` char(2) DEFAULT NULL,
  `CbdcExportador` varchar(60) DEFAULT NULL,
  `CbddDesemb` datetime DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnItem`,`CbdnDI`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001deticmsnormalst` */

CREATE TABLE `cbd001deticmsnormalst` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnItem` smallint(6) NOT NULL DEFAULT '0',
  `CbdCST` smallint(6) NOT NULL DEFAULT '0',
  `Cbdorig` smallint(6) DEFAULT NULL,
  `CbdmodBC` smallint(6) DEFAULT NULL,
  `CbdvBC` decimal(16,6) DEFAULT NULL,
  `CbdpICMS` decimal(7,2) DEFAULT NULL,
  `CbdvICMS_icms` decimal(16,6) DEFAULT NULL,
  `CbdmodBCST` smallint(6) DEFAULT NULL,
  `CbdpMVAST` decimal(7,2) DEFAULT NULL,
  `CbdpRedBCST` decimal(7,2) DEFAULT NULL,
  `CbdvBCST` decimal(16,6) DEFAULT NULL,
  `CbdpICMSST` decimal(7,2) DEFAULT NULL,
  `CbdvICMSST_icms` decimal(16,6) DEFAULT NULL,
  `CbdpRedBC` decimal(7,2) DEFAULT NULL,
  `CbdmotDesICMS` int(1) DEFAULT NULL,
  `CbdvBCSTRet` decimal(16,6) DEFAULT NULL,
  `CbdvICMSSTRet` decimal(16,6) DEFAULT NULL,
  `CbdpBCOp` decimal(7,2) DEFAULT NULL,
  `CbdUFST` char(2) DEFAULT NULL,
  `CbdvBCSTDest` decimal(16,6) DEFAULT NULL,
  `CbdvICMSSTDest_icms` decimal(16,6) DEFAULT NULL,
  `CbdpCredSN` decimal(7,2) DEFAULT NULL,
  `CbdvCredICMSSN` decimal(16,6) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnItem`,`CbdCST`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001detipi` */

CREATE TABLE `cbd001detipi` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnItem` smallint(6) NOT NULL DEFAULT '0',
  `CbdCST_IPI` char(2) NOT NULL DEFAULT '',
  `CbdvBC_IPI` decimal(16,6) DEFAULT NULL,
  `CbdqUnid_IPI` decimal(17,6) DEFAULT NULL,
  `CbdvUnid_IPI` decimal(17,6) DEFAULT NULL,
  `CbdpIPI` decimal(7,2) DEFAULT NULL,
  `CbdvIPI` decimal(16,6) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnItem`,`CbdCST_IPI`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001detmed` */

CREATE TABLE `cbd001detmed` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnItem` smallint(6) NOT NULL DEFAULT '0',
  `CbdnLote` varchar(20) NOT NULL DEFAULT '',
  `CbdqLote` decimal(16,6) DEFAULT NULL,
  `CbddFab` datetime DEFAULT NULL,
  `CbddVal` datetime DEFAULT NULL,
  `CbdvPMC` decimal(16,6) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnItem`,`CbdnLote`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001detpis` */

CREATE TABLE `cbd001detpis` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnItem` smallint(6) NOT NULL DEFAULT '0',
  `CbdCST_pis` smallint(6) NOT NULL DEFAULT '0',
  `CbdvBC_pis` decimal(16,6) DEFAULT NULL,
  `CbdpPIS` decimal(7,2) DEFAULT NULL,
  `CbdvPIS` decimal(16,6) DEFAULT NULL,
  `CbdqBCprod_pis` decimal(17,6) DEFAULT NULL,
  `CbdvAliqProd_pis` decimal(17,6) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnItem`,`CbdCST_pis`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001duplicatas` */

CREATE TABLE `cbd001duplicatas` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnDup` varchar(60) NOT NULL DEFAULT '',
  `CbddVenc` datetime DEFAULT NULL,
  `CbdvDup` decimal(16,6) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnDup`),
  KEY `ICBD8` (`CbdEmpCodigo`,`CbdNtfNumero`,`CbdNtfSerie`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001lacres` */

CREATE TABLE `cbd001lacres` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnLacre` varchar(60) NOT NULL DEFAULT '',
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnLacre`),
  KEY `ICBD10` (`CbdEmpCodigo`,`CbdNtfNumero`,`CbdNtfSerie`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001nref` */

CREATE TABLE `cbd001nref` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdrefNFe` varchar(44) NOT NULL DEFAULT '',
  `CbdcUF_refNFE` smallint(6) DEFAULT NULL,
  `CbdAAMM` smallint(6) DEFAULT NULL,
  `CbdCNPJ` varchar(14) DEFAULT NULL,
  `Cbdmod_refNFE` smallint(6) DEFAULT NULL,
  `Cbdserie_refNFE` smallint(6) DEFAULT NULL,
  `CbdnNF_refNFE` int(11) DEFAULT NULL,
  `CbdIE_refNFP` varchar(14) DEFAULT NULL,
  `CbdRefCte` varchar(44) DEFAULT NULL,
  `CbdnECF_refECF` int(3) DEFAULT NULL,
  `CbdnCOO_refECF` decimal(6,0) DEFAULT NULL,
  `CbdCPF` varchar(11) DEFAULT NULL,
  `Cbdmod_refECF` char(2) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdrefNFe`),
  KEY `ICBD12` (`CbdEmpCodigo`,`CbdNtfNumero`,`CbdNtfSerie`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001obsfisco` */

CREATE TABLE `cbd001obsfisco` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdxCampo` varchar(20) NOT NULL DEFAULT '',
  `CbdxTexto` varchar(60) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdxCampo`),
  KEY `ICBD13` (`CbdEmpCodigo`,`CbdNtfNumero`,`CbdNtfSerie`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001procref` */

CREATE TABLE `cbd001procref` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnProc` varchar(60) NOT NULL DEFAULT '',
  `CbdindProc` smallint(6) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnProc`),
  KEY `ICBD14` (`CbdEmpCodigo`,`CbdNtfNumero`,`CbdNtfSerie`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001reboque` */

CREATE TABLE `cbd001reboque` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `Cbdplaca_rebtransp` varchar(8) NOT NULL DEFAULT '',
  `CbdUF_rebtransp` char(2) DEFAULT NULL,
  `CbdRNTC_rebtransp` varchar(20) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`Cbdplaca_rebtransp`),
  KEY `ICBD15` (`CbdEmpCodigo`,`CbdNtfNumero`,`CbdNtfSerie`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cbd001vol` */

CREATE TABLE `cbd001vol` (
  `CbdEmpCodigo` smallint(6) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdNtfNumero` int(11) NOT NULL DEFAULT '0',
  `CbdnVol` varchar(60) NOT NULL DEFAULT '',
  `CbdqVol` decimal(17,6) DEFAULT NULL,
  `Cbdesp` varchar(60) DEFAULT NULL,
  `Cbdmarca` varchar(60) DEFAULT NULL,
  `CbdpesoL_transp` decimal(16,6) DEFAULT NULL,
  `CbdpesoB_transp` decimal(16,6) DEFAULT NULL,
  `CbdCodigoFilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfSerie`,`CbdNtfNumero`,`CbdnVol`),
  KEY `ICBD16` (`CbdEmpCodigo`,`CbdNtfNumero`,`CbdNtfSerie`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cheques` */

CREATE TABLE `cheques` (
  `id` int(6) NOT NULL AUTO_INCREMENT,
  `destinatario` varchar(50) DEFAULT NULL,
  `repassado` char(1) NOT NULL DEFAULT 'N',
  `datarepasse` date DEFAULT NULL,
  `banco` varchar(15) DEFAULT NULL,
  `cheque` varchar(10) DEFAULT NULL,
  `agencia` varchar(8) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `valor` decimal(12,2) DEFAULT NULL,
  `ValorCheque` decimal(12,2) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `nomecheque` varchar(50) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `cpf` varchar(15) NOT NULL DEFAULT '',
  `telefone` varchar(15) DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `observacao` varchar(250) DEFAULT NULL,
  `marcado` char(1) DEFAULT NULL,
  `semfundo` char(1) NOT NULL DEFAULT 'N',
  `taxadesconto` decimal(10,2) DEFAULT NULL,
  `datadesconto` date DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `Historico` varchar(15) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `cpfcheque` varchar(18) NOT NULL DEFAULT '',
  `depositado` char(1) NOT NULL DEFAULT 'N',
  `encargos` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `operador` varchar(10) NOT NULL DEFAULT '',
  `datadevolvido` date DEFAULT NULL,
  `interpolador` varchar(8) DEFAULT NULL,
  `interpoladorant` varchar(8) DEFAULT NULL,
  `contabancaria` varchar(10) DEFAULT NULL,
  `ip` varchar(15) DEFAULT NULL,
  `tipo` char(2) DEFAULT NULL,
  `vencimentooriginal` date DEFAULT NULL,
  `operadorprorrogacao` varchar(10) DEFAULT NULL,
  `dataprorrogacao` date DEFAULT NULL,
  `datatroca` date DEFAULT NULL,
  `valordesconto` decimal(8,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `comissaopaga` char(1) NOT NULL DEFAULT 'N',
  PRIMARY KEY (`id`,`cpf`,`cpfcheque`),
  KEY `documento` (`documento`)
) ENGINE=MyISAM AUTO_INCREMENT=57 DEFAULT CHARSET=latin1;

/*Table structure for table `classe` */

CREATE TABLE `classe` (
  `codigo` char(4) NOT NULL DEFAULT '0',
  `descricao` char(20) DEFAULT '0',
  `percentual` decimal(6,2) DEFAULT '2.00',
  `Somar` char(1) DEFAULT '0',
  `codigofilial` char(5) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `classefiscal` */

CREATE TABLE `classefiscal` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigo` char(3) NOT NULL DEFAULT '000',
  `descricao` varchar(40) DEFAULT NULL,
  `reducao` decimal(6,2) NOT NULL DEFAULT '0.00',
  `lei` text,
  `UsarICMSDiferenciado` char(1) NOT NULL DEFAULT 'N',
  `ICMSpadrao` decimal(5,2) NOT NULL DEFAULT '0.00',
  `ICMSEstadoPJ` decimal(5,2) NOT NULL DEFAULT '0.00',
  `ICMSEstadoPF` decimal(5,2) NOT NULL DEFAULT '0.00',
  `ICMSOutroEstadoPJ` decimal(5,2) NOT NULL DEFAULT '0.00',
  `ICMSOutroEstadoPF` decimal(5,2) NOT NULL DEFAULT '0.00',
  `lei2` text,
  `tributacaoEstado` char(3) NOT NULL DEFAULT '00',
  `tributacaoForaEstado` char(3) NOT NULL DEFAULT '00',
  `reducaoforaEstado` decimal(6,2) NOT NULL DEFAULT '0.00',
  `ICMSSTEstadoPJ` decimal(5,2) NOT NULL DEFAULT '0.00',
  `ICMSSTEstadoPF` decimal(5,2) NOT NULL DEFAULT '0.00',
  `ICMSSTOutroEstadoPJ` decimal(5,2) NOT NULL DEFAULT '0.00',
  `ICMSSTOutroEstadoPF` decimal(5,2) NOT NULL DEFAULT '0.00',
  `reducaoSTestado` decimal(5,2) NOT NULL DEFAULT '0.00',
  `reducaoSTForaEstado` decimal(5,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

/*Table structure for table `clientes` */

CREATE TABLE `clientes` (
  `Codigo` int(6) NOT NULL AUTO_INCREMENT,
  `Nome` varchar(50) NOT NULL DEFAULT '',
  `conjuge` varchar(40) DEFAULT NULL,
  `estadocivil` varchar(11) NOT NULL DEFAULT 'Solteiro(a)',
  `pai` varchar(40) DEFAULT NULL,
  `mae` varchar(40) DEFAULT NULL,
  `sexo` char(1) DEFAULT NULL,
  `apelido` varchar(30) DEFAULT NULL,
  `endereco` varchar(50) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `bairro` varchar(20) DEFAULT NULL,
  `cidade` varchar(30) DEFAULT NULL,
  `estado` char(2) DEFAULT NULL,
  `endereco2` varchar(50) DEFAULT NULL,
  `cep2` varchar(10) DEFAULT NULL,
  `bairro2` varchar(20) DEFAULT NULL,
  `cidade2` varchar(30) DEFAULT NULL,
  `estado2` char(2) DEFAULT NULL,
  `cnpj` varchar(18) DEFAULT NULL,
  `inscricao` varchar(20) DEFAULT NULL,
  `cpf` varchar(14) DEFAULT NULL,
  `identidade` varchar(15) DEFAULT NULL,
  `observacao` text,
  `telefone` varchar(16) DEFAULT NULL,
  `telefone2` varchar(16) DEFAULT NULL,
  `telefone3` varchar(16) DEFAULT NULL,
  `fax` varchar(16) DEFAULT NULL,
  `celular` varchar(16) DEFAULT NULL,
  `datacadastro` date DEFAULT NULL,
  `nascimento` date DEFAULT NULL,
  `datacobranca` date DEFAULT NULL,
  `localtrabalho` varchar(30) DEFAULT NULL,
  `profissao` varchar(40) DEFAULT NULL,
  `creditoprovisorio` decimal(10,2) NOT NULL DEFAULT '0.00',
  `credito` decimal(10,2) DEFAULT NULL,
  `debito` decimal(10,2) DEFAULT NULL,
  `saldo` decimal(10,2) DEFAULT NULL,
  `renda` decimal(10,2) DEFAULT NULL,
  `situacao` varchar(15) DEFAULT NULL,
  `restritiva` char(1) NOT NULL DEFAULT 'N',
  `ultcompra` date DEFAULT NULL,
  `ultpagamento` date DEFAULT NULL,
  `ultvrpago` decimal(10,2) DEFAULT NULL,
  `ultvencimento` date DEFAULT NULL,
  `area` varchar(20) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `chequedevolvido` char(1) DEFAULT NULL,
  `cobrarnovencimento` char(1) DEFAULT NULL,
  `enderecotrab` varchar(40) DEFAULT NULL,
  `bairrotrab` varchar(20) DEFAULT NULL,
  `cidadetrab` varchar(20) DEFAULT NULL,
  `estadotrab` char(2) DEFAULT NULL,
  `telefonetrab` varchar(16) DEFAULT NULL,
  `cnpjtrab` varchar(18) DEFAULT NULL,
  `salario` decimal(10,2) NOT NULL DEFAULT '0.00',
  `cpfconj` varchar(14) DEFAULT NULL,
  `identidadeconj` varchar(15) DEFAULT NULL,
  `nascimentoconj` date DEFAULT NULL,
  `localtrabconj` varchar(25) DEFAULT NULL,
  `enderecoconj` varchar(40) DEFAULT NULL,
  `bairroconj` varchar(25) DEFAULT NULL,
  `cidadeconj` varchar(20) DEFAULT NULL,
  `estadoconj` char(2) DEFAULT NULL,
  `profissaoconj` varchar(25) DEFAULT NULL,
  `salarioconj` decimal(10,2) DEFAULT NULL,
  `ncartas` int(3) NOT NULL DEFAULT '0',
  `dataultcarta` date NOT NULL DEFAULT '0000-00-00',
  `cobranca` char(1) DEFAULT NULL,
  `datadesativo` date DEFAULT NULL,
  `avalista` varchar(40) DEFAULT NULL,
  `enderecoaval` varchar(40) DEFAULT NULL,
  `bairroaval` varchar(20) DEFAULT NULL,
  `cidadeaval` varchar(20) DEFAULT NULL,
  `estadoaval` char(2) DEFAULT NULL,
  `telefoneaval` varchar(16) DEFAULT NULL,
  `rendaavalista` decimal(10,2) DEFAULT NULL,
  `cpfavalista` varchar(14) DEFAULT NULL,
  `identidadeaval` varchar(15) DEFAULT NULL,
  `nascimentoaval` date DEFAULT NULL,
  `referencias` text,
  `ndiascobranca` int(3) DEFAULT NULL,
  `ceptrab` varchar(10) DEFAULT NULL,
  `cepConj` varchar(10) DEFAULT NULL,
  `TelefoneConj` varchar(16) DEFAULT NULL,
  `LocalTrabAval` varchar(25) DEFAULT NULL,
  `ProfissaoAval` varchar(25) DEFAULT NULL,
  `cepaval` varchar(10) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `codarea` varchar(4) DEFAULT NULL,
  `marcado` char(1) NOT NULL DEFAULT '',
  `codigoserv` int(5) unsigned NOT NULL DEFAULT '0',
  `valorcontrato` decimal(10,2) NOT NULL DEFAULT '0.00',
  `valorultimocontrato` decimal(10,2) NOT NULL DEFAULT '0.00',
  `percentualsalario` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `atualizarpelosalario` char(1) NOT NULL DEFAULT 'S',
  `desconto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `datalimitedesconto` date DEFAULT NULL,
  `vendedor` char(3) DEFAULT NULL,
  `servico` varchar(60) DEFAULT NULL,
  `codigosice` bigint(6) DEFAULT NULL,
  `produto` varchar(20) DEFAULT NULL,
  `numerofiliais` int(3) NOT NULL DEFAULT '1',
  `senha` varchar(60) DEFAULT NULL,
  `dataalteracaosenha` datetime DEFAULT NULL,
  `tiporesidencia` varchar(15) NOT NULL DEFAULT '',
  `valoraluguel` decimal(10,2) NOT NULL DEFAULT '0.00',
  `outrasrendas` decimal(10,2) NOT NULL DEFAULT '0.00',
  `totalrenda` decimal(10,2) NOT NULL DEFAULT '0.00',
  `databoleto` date NOT NULL DEFAULT '2001-01-20',
  `versaolite` char(1) NOT NULL DEFAULT 'N',
  `contabil` char(1) NOT NULL DEFAULT 'N',
  `tipo` varchar(20) NOT NULL DEFAULT '',
  `operadoralteracao` varchar(10) DEFAULT NULL,
  `fabricacao` char(1) NOT NULL DEFAULT 'N',
  `servicos` char(1) NOT NULL DEFAULT 'N',
  `debitoch` decimal(10,2) NOT NULL DEFAULT '0.00',
  `diasprimeirovencimento` int(3) NOT NULL DEFAULT '0',
  `cobrarnodia` int(2) NOT NULL DEFAULT '0',
  `cartaofidelidade` varchar(20) DEFAULT '',
  `proprietario` varchar(30) DEFAULT NULL,
  `fidelizacao` char(1) NOT NULL DEFAULT 'N',
  `valorcartaofidelidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `datacontrato` date NOT NULL DEFAULT '0000-00-00',
  `validadecontrato` date DEFAULT '0000-00-00',
  `numero` varchar(8) NOT NULL DEFAULT '',
  `obsservicos` text,
  `site` varchar(100) DEFAULT NULL,
  `mapa` text,
  `tipoBairro` varchar(10) DEFAULT NULL,
  `tipoEndereco` varchar(10) DEFAULT NULL,
  `ativo` char(1) NOT NULL DEFAULT 'S',
  `newsletter` char(1) NOT NULL DEFAULT 'S',
  `complementoEnd` varchar(15) NOT NULL DEFAULT '15',
  PRIMARY KEY (`Codigo`),
  UNIQUE KEY `Codigo` (`Codigo`),
  KEY `Nomes` (`Nome`)
) ENGINE=MyISAM AUTO_INCREMENT=2551 DEFAULT CHARSET=latin1;

/*Table structure for table `clientesexternos` */

CREATE TABLE `clientesexternos` (
  `id` int(8) NOT NULL AUTO_INCREMENT,
  `filial` varchar(5) DEFAULT '00001',
  `nome` varchar(50) DEFAULT NULL,
  `fantasia` varchar(30) DEFAULT NULL,
  `nascimento` date DEFAULT NULL,
  `diretor` varchar(30) DEFAULT NULL,
  `tipoEndereco` varchar(20) DEFAULT NULL,
  `endereco` varchar(50) DEFAULT NULL,
  `numero` varchar(10) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `bairro` varchar(30) DEFAULT NULL,
  `cidade` varchar(30) DEFAULT NULL,
  `estado` varchar(2) DEFAULT NULL,
  `cnpj` varchar(14) DEFAULT NULL,
  `cpf` varchar(11) DEFAULT NULL,
  `ie` varchar(18) DEFAULT NULL,
  `situacao` varchar(20) DEFAULT NULL,
  `telefone` varchar(16) DEFAULT NULL,
  `telefone2` varchar(16) DEFAULT NULL,
  `celular` varchar(16) DEFAULT NULL,
  `email` varchar(70) DEFAULT NULL,
  `datacobranca` date DEFAULT NULL,
  `valorcontrato` decimal(8,2) NOT NULL DEFAULT '0.00',
  `observacao` text,
  `datacadastro` date DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Table structure for table `clientesfoto` */

CREATE TABLE `clientesfoto` (
  `inc` int(11) NOT NULL AUTO_INCREMENT,
  `codcli` varchar(20) DEFAULT NULL,
  `fotocli` blob,
  `codigofilial` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Table structure for table `cmsarticles` */

CREATE TABLE `cmsarticles` (
  `ID` int(6) unsigned NOT NULL AUTO_INCREMENT,
  `title` varchar(200) DEFAULT NULL,
  `tagline` varchar(255) DEFAULT NULL,
  `section` int(4) DEFAULT '0',
  `thearticle` text,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cmsgroups` */

CREATE TABLE `cmsgroups` (
  `ID` int(4) unsigned NOT NULL AUTO_INCREMENT,
  `groupname` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cmssections` */

CREATE TABLE `cmssections` (
  `ID` int(4) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(20) DEFAULT NULL,
  `parentid` int(4) DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cmsusers` */

CREATE TABLE `cmsusers` (
  `ID` int(4) unsigned NOT NULL AUTO_INCREMENT,
  `user` varchar(20) DEFAULT NULL,
  `pass` varchar(20) DEFAULT NULL,
  `thegroup` int(4) DEFAULT '10',
  `firstname` varchar(20) DEFAULT NULL,
  `surname` varchar(20) DEFAULT NULL,
  `enabled` int(1) DEFAULT '1',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `cobradores` */

CREATE TABLE `cobradores` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(30) DEFAULT NULL,
  `taxacapital` decimal(6,2) DEFAULT NULL,
  `taxajuros` decimal(6,2) DEFAULT NULL,
  `taxarenegociacao` decimal(6,2) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `comissao` */

CREATE TABLE `comissao` (
  `codigofilial` char(5) DEFAULT NULL,
  `id` char(15) DEFAULT NULL,
  `vendedor` char(3) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `valor` decimal(10,2) DEFAULT '0.00',
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `devolucao` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Percentual` decimal(6,2) NOT NULL DEFAULT '0.00',
  `comissaodev` char(1) DEFAULT NULL,
  `ItensVendidos` int(4) unsigned NOT NULL DEFAULT '0',
  `ItensDevolvidos` int(4) unsigned NOT NULL DEFAULT '0',
  `Valorcomissao` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ValorComisDev` decimal(10,2) NOT NULL DEFAULT '0.00',
  `valorliquido` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Desconto` decimal(10,2) NOT NULL DEFAULT '0.00'
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COMMENT='Tabela de controle de comissões de vendedor';

/*Table structure for table `configfinanc` */

CREATE TABLE `configfinanc` (
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `fatmaiordesvenda` decimal(5,2) NOT NULL DEFAULT '0.00',
  `limitevalor` decimal(10,2) NOT NULL DEFAULT '250.00',
  `fatmaiordescrec` decimal(5,2) NOT NULL DEFAULT '0.00',
  `fatjurdia` decimal(5,3) NOT NULL DEFAULT '0.000',
  `fatmaiordescrecjur` decimal(5,2) NOT NULL DEFAULT '0.00',
  `fatjurant` decimal(5,3) NOT NULL DEFAULT '0.000',
  `fatnrdias` int(2) DEFAULT NULL,
  `fatultnumnf` bigint(12) NOT NULL DEFAULT '0',
  `Climesesrencad` int(3) unsigned DEFAULT NULL,
  `CliData` char(1) DEFAULT NULL,
  `clindiasnaocobrar` int(3) unsigned DEFAULT NULL,
  `PerClasse` char(1) DEFAULT NULL,
  `Codigo` int(4) NOT NULL AUTO_INCREMENT,
  `produtoscadastro` char(1) NOT NULL DEFAULT 'N',
  `recalcularprevenda` char(1) NOT NULL DEFAULT 'N',
  `valorsalario` decimal(10,2) NOT NULL DEFAULT '0.00',
  `cpfobrigatorio` char(1) NOT NULL DEFAULT 'S',
  `msg1` varchar(255) DEFAULT NULL,
  `msg2` varchar(255) DEFAULT NULL,
  `diasvencimento` int(3) NOT NULL DEFAULT '0',
  `intervalocrediario` int(3) NOT NULL DEFAULT '30',
  `qtdprateleiras` char(1) NOT NULL DEFAULT 'N',
  `instrucaocobranca` varchar(200) DEFAULT NULL,
  `boletobancario` char(1) NOT NULL DEFAULT 'N',
  `devolucaocomnota` char(1) NOT NULL DEFAULT 'S',
  `recibocaixa` char(1) NOT NULL DEFAULT 'S',
  `despesacontasapagar` char(1) NOT NULL DEFAULT 'N',
  `posicaocodigobalanca` char(2) NOT NULL DEFAULT '24',
  `alterarpreco` char(1) NOT NULL DEFAULT 'S',
  `cliusafatorlimite` char(1) NOT NULL DEFAULT 'N',
  `clifatorlimite` decimal(5,2) NOT NULL DEFAULT '0.00',
  `conta` varchar(5) DEFAULT NULL,
  `subconta` varchar(5) DEFAULT NULL,
  `maxarredondamento` decimal(10,2) NOT NULL DEFAULT '0.00',
  `alterarprecotransferencia` char(1) NOT NULL DEFAULT 'S',
  `restricaoprevenda` char(1) NOT NULL DEFAULT 'N',
  `qtditenstabela` int(5) NOT NULL DEFAULT '50',
  `taxafinanciamento` decimal(7,3) NOT NULL DEFAULT '0.000',
  `escolherfilialtransferencia` char(1) NOT NULL DEFAULT 'S',
  `impentradacarne` char(1) NOT NULL DEFAULT 'N',
  `descontocartaofidelidade` decimal(6,2) NOT NULL DEFAULT '0.00',
  `diasparatrocasenhasoperadores` int(3) NOT NULL DEFAULT '0',
  `diasparamudarsituacao` int(3) NOT NULL DEFAULT '0',
  `situacaoautomatica` varchar(15) DEFAULT NULL,
  `localcobrancaboleto` varchar(50) DEFAULT NULL,
  `abatercreditocompraCH` char(1) NOT NULL DEFAULT 'N',
  `diascorridosvencimentos` char(1) NOT NULL DEFAULT 'N',
  `retidosbaixarestoque` char(1) NOT NULL DEFAULT 'S',
  `escolherfiliallctdb` char(1) NOT NULL DEFAULT 'N',
  `cpmf` decimal(5,2) NOT NULL DEFAULT '0.00',
  `iss` decimal(5,2) NOT NULL DEFAULT '0.00',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `bdorigemprevenda` varchar(30) NOT NULL DEFAULT 'sice',
  `operadordigitarsenhanaprevenda` char(1) NOT NULL DEFAULT 'N',
  `abaterestoqueprevenda` char(1) NOT NULL DEFAULT 'S',
  `textogarantia` text,
  `mostraqtddisponivel` char(1) NOT NULL DEFAULT 'N',
  `mostrarprecominimo` char(1) NOT NULL DEFAULT 'N',
  `valorbonificaocartaofidelidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `pontuacaobonificacaocartaofidelidade` decimal(8,2) NOT NULL DEFAULT '0.00',
  `iof` float(8,5) NOT NULL DEFAULT '0.00000',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `irrf` decimal(5,2) NOT NULL DEFAULT '0.00',
  `situacaoautomaticainferior` varchar(15) DEFAULT NULL,
  `diasparamudarsituacaoinferior` int(3) NOT NULL DEFAULT '0',
  `buscaautomatica` char(1) NOT NULL DEFAULT 'S',
  `mostrarprecoatacado` char(1) NOT NULL DEFAULT 'N',
  `iof2` float(8,5) NOT NULL DEFAULT '0.00000',
  `prodcodaut` char(1) NOT NULL DEFAULT 'S',
  `boletocobrebemX` char(1) NOT NULL DEFAULT 'S',
  `especieNF` varchar(5) NOT NULL DEFAULT 'NF',
  `serieNF` varchar(5) DEFAULT '1',
  `modeloDocFiscal` char(2) DEFAULT '01',
  `CSLL` decimal(5,2) NOT NULL DEFAULT '0.00',
  `EmitirNF_e` char(1) NOT NULL DEFAULT 'N',
  `msgnotafiscal` text,
  `cfopsaida` varchar(5) NOT NULL DEFAULT '5.102',
  `cfopsaidaECF` varchar(5) NOT NULL DEFAULT '5.102',
  `calculamargempordentro` char(1) NOT NULL DEFAULT 'N',
  `ultimoselonf` bigint(20) NOT NULL DEFAULT '0',
  `buscaautomaticaprd` char(1) NOT NULL DEFAULT 'S',
  `origembackup` varchar(200) DEFAULT NULL,
  `destinobackup` varchar(200) DEFAULT NULL,
  `digitoIniBal` char(1) DEFAULT NULL,
  `devolucaomaiorrec` char(1) NOT NULL DEFAULT 'S',
  `filialmae` varchar(5) NOT NULL DEFAULT '00001',
  `descontoservico` decimal(5,2) NOT NULL DEFAULT '0.00',
  `prodcadcustopreco` char(1) NOT NULL DEFAULT 'N',
  `entradaperguntaalteracusto` char(1) NOT NULL DEFAULT 'S',
  `negociacaojurosnavenda` char(1) NOT NULL DEFAULT 'N',
  `permitirporcontacliente` char(1) NOT NULL DEFAULT 'S',
  `BCICMSProduto` char(1) NOT NULL DEFAULT 'S',
  `emissornfe` char(1) NOT NULL DEFAULT '0',
  KEY `CfIndCod` (`Codigo`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Table structure for table `consultaclientes` */

CREATE TABLE `consultaclientes` (
  `inc` int(10) NOT NULL AUTO_INCREMENT,
  `codigo` int(6) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `filial` varchar(50) DEFAULT NULL,
  `cpfcnpj` varchar(18) DEFAULT NULL,
  PRIMARY KEY (`inc`),
  KEY `codigo` (`codigo`),
  KEY `cpfcnpj` (`cpfcnpj`)
) ENGINE=MyISAM AUTO_INCREMENT=36099 DEFAULT CHARSET=latin1;

/*Table structure for table `contactos` */

CREATE TABLE `contactos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `dataagenda` date DEFAULT NULL,
  `operadoragendado` varchar(10) DEFAULT NULL,
  `dataresolucao` date DEFAULT NULL,
  `concluidopor` varchar(10) NOT NULL DEFAULT '',
  `contactos` int(5) DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `dialogo` text,
  `resposta` text,
  `servicosolicitado` varchar(30) DEFAULT NULL,
  `codigofilial` varchar(10) DEFAULT NULL,
  `solicitante` varchar(30) DEFAULT NULL,
  `vendedor` char(3) NOT NULL DEFAULT '',
  `concluido` char(1) DEFAULT 'N',
  `prioridade` int(1) NOT NULL DEFAULT '1',
  `anexo` char(1) NOT NULL DEFAULT 'N',
  `anexodetalhe` varchar(90) DEFAULT NULL,
  `status` varchar(30) DEFAULT NULL,
  `departamento` varchar(15) NOT NULL DEFAULT 'SUPORTE',
  PRIMARY KEY (`id`),
  KEY `codigocliente` (`codigocliente`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `contador` */

CREATE TABLE `contador` (
  `area` varchar(60) NOT NULL DEFAULT '0',
  `data` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `acessos` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`area`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `contador_f` */

CREATE TABLE `contador_f` (
  `area` varchar(60) NOT NULL DEFAULT '0',
  `data` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `acessos` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`area`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `contasbanco` */

CREATE TABLE `contasbanco` (
  `id_conta` int(3) NOT NULL AUTO_INCREMENT,
  `banco` varchar(15) DEFAULT NULL,
  `agencia` varchar(10) DEFAULT NULL,
  `conta` varchar(20) DEFAULT NULL,
  `tipo` varchar(15) DEFAULT NULL,
  `saldo` decimal(12,2) DEFAULT NULL,
  `limite` decimal(12,2) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `grupo` char(2) DEFAULT NULL,
  `codigocedente` varchar(30) DEFAULT NULL,
  `cobrebemlicenca` varchar(100) DEFAULT NULL,
  `cobrebeminicionumero` varchar(20) DEFAULT NULL,
  `cobrebemfimnumero` varchar(20) DEFAULT NULL,
  `cobrebemlayoutboleto` varchar(50) DEFAULT NULL,
  `cobrebemproximonumero` int(11) DEFAULT NULL,
  `cobrebemvariacaocarteira` char(2) DEFAULT NULL,
  `codigobanco` int(4) NOT NULL DEFAULT '0',
  `convenio` varchar(10) DEFAULT NULL,
  `gerarremessa` char(1) DEFAULT 'N',
  `percentualmulta` decimal(4,2) DEFAULT NULL,
  `percentualjuros` decimal(4,2) DEFAULT NULL,
  `jurosdeumdia` decimal(12,2) DEFAULT NULL,
  `diasdesconto` int(3) DEFAULT NULL,
  `percentualdesconto` decimal(12,2) DEFAULT NULL,
  `diasdesconto2` int(3) DEFAULT NULL,
  `percentualdesconto2` decimal(12,2) DEFAULT NULL,
  `especie` char(2) DEFAULT NULL,
  `aceite` char(1) DEFAULT NULL,
  `valorabatimento` decimal(12,2) DEFAULT NULL,
  `diasparaprotesto` int(3) DEFAULT NULL,
  PRIMARY KEY (`id_conta`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

/*Table structure for table `contaspagar` */

CREATE TABLE `contaspagar` (
  `Codigo` int(6) NOT NULL AUTO_INCREMENT,
  `documento` varchar(20) DEFAULT NULL,
  `empresa` varchar(30) DEFAULT NULL,
  `historico` text,
  `valor` decimal(10,2) DEFAULT NULL,
  `multa` decimal(5,2) DEFAULT NULL,
  `jurosaodia` decimal(5,2) DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `datadocumento` date DEFAULT NULL,
  `data` date DEFAULT NULL,
  `setor` varchar(40) DEFAULT NULL,
  `subsetor` varchar(40) DEFAULT NULL,
  `codcontadespesa` int(4) DEFAULT NULL,
  `descricao` varchar(40) DEFAULT NULL,
  `codsubcontadespesa` int(3) DEFAULT NULL,
  `descricaosubconta` varchar(40) DEFAULT NULL,
  `despesa` char(1) DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `codigoset` int(4) DEFAULT NULL,
  `codigosubset` int(4) DEFAULT NULL,
  `usuario` varchar(10) DEFAULT NULL,
  `marcado` char(1) DEFAULT NULL,
  `quitado` char(1) DEFAULT 'N',
  `dataPagamento` date DEFAULT NULL,
  `grupo` varchar(15) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `contabancaria` varchar(20) DEFAULT NULL,
  `interpolador` varchar(8) DEFAULT NULL,
  `recebida` char(1) NOT NULL DEFAULT 'S',
  `conta` varchar(5) NOT NULL DEFAULT '',
  `subconta` varchar(5) NOT NULL DEFAULT '',
  `ip` varchar(15) DEFAULT NULL,
  `operadorlancamento` varchar(10) DEFAULT NULL,
  `cheque` bigint(15) DEFAULT NULL,
  `nrparcela` varchar(5) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `nomeCliente` varchar(50) DEFAULT NULL,
  KEY `IndCodCP` (`Codigo`),
  KEY `documento` (`documento`),
  KEY `marcado` (`marcado`),
  KEY `cheque` (`cheque`)
) ENGINE=MyISAM AUTO_INCREMENT=3412 DEFAULT CHARSET=latin1;

/*Table structure for table `contbalanco` */

CREATE TABLE `contbalanco` (
  `inc` int(6) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `usuario` varchar(10) DEFAULT NULL,
  `encerrado` char(1) NOT NULL DEFAULT 'N',
  `dataencerramento` date DEFAULT NULL,
  `horaencerramento` time DEFAULT NULL,
  `ip` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Table structure for table `contdav` */

CREATE TABLE `contdav` (
  `enderecoip` varchar(15) DEFAULT NULL,
  `numero` int(5) NOT NULL AUTO_INCREMENT,
  `numeroDAVFilial` int(7) NOT NULL DEFAULT '0',
  `data` date DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `finalizada` char(1) NOT NULL DEFAULT 'N',
  `observacao` varchar(400) DEFAULT NULL,
  `devolucao` int(8) DEFAULT NULL,
  `responsavelreceber` varchar(50) NOT NULL DEFAULT '',
  `enderecoentrega` varchar(50) NOT NULL DEFAULT '',
  `numeroentrega` varchar(10) NOT NULL DEFAULT '',
  `cidadeentrega` varchar(30) DEFAULT NULL,
  `cepentrega` varchar(8) DEFAULT NULL,
  `bairroentrega` varchar(20) DEFAULT NULL,
  `horaentrega` time DEFAULT NULL,
  `dataentrega` date DEFAULT NULL,
  `valor` decimal(12,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `desconto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `origem` varchar(20) NOT NULL DEFAULT 'Balcao',
  `datafinalizacao` date DEFAULT NULL,
  `cartaofidelidade` varchar(20) DEFAULT NULL,
  `estadoentrega` char(2) NOT NULL DEFAULT 'PE',
  `ncupomfiscal` varchar(6) NOT NULL DEFAULT '0',
  `cancelada` char(1) NOT NULL DEFAULT 'N',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `ecfCPFCNPJconsumidor` varchar(14) DEFAULT NULL,
  `numeroECF` varchar(3) DEFAULT NULL,
  `ecfcontadorcupomfiscal` varchar(10) DEFAULT NULL,
  `classe` varchar(4) NOT NULL DEFAULT '0000',
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `osnrfabricacao` varchar(40) DEFAULT NULL,
  `marca` varchar(30) DEFAULT NULL,
  `modelo` varchar(30) DEFAULT NULL,
  `ano` int(4) DEFAULT NULL,
  `placa` varchar(8) DEFAULT NULL,
  `renavam` varchar(40) DEFAULT NULL,
  `EADRegistroDAV` varchar(33) DEFAULT NULL,
  `contadorRGECF` varchar(6) NOT NULL DEFAULT ' ',
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=1433 DEFAULT CHARSET=latin1;

/*Table structure for table `contdavos` */

CREATE TABLE `contdavos` (
  `enderecoip` varchar(15) DEFAULT NULL,
  `numero` int(5) NOT NULL AUTO_INCREMENT,
  `numeroDAVFilial` int(7) NOT NULL DEFAULT '0',
  `data` date DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `finalizada` char(1) NOT NULL DEFAULT 'N',
  `observacao` varchar(150) DEFAULT NULL,
  `devolucao` int(8) DEFAULT NULL,
  `responsavelreceber` varchar(50) NOT NULL DEFAULT '',
  `enderecoentrega` varchar(50) NOT NULL DEFAULT '',
  `numeroentrega` varchar(10) NOT NULL DEFAULT '',
  `cidadeentrega` varchar(30) DEFAULT NULL,
  `cepentrega` varchar(8) DEFAULT NULL,
  `bairroentrega` varchar(20) DEFAULT NULL,
  `horaentrega` time DEFAULT NULL,
  `dataentrega` date DEFAULT NULL,
  `valor` decimal(12,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `desconto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `origem` varchar(20) NOT NULL DEFAULT 'Balcao',
  `datafinalizacao` date DEFAULT NULL,
  `cartaofidelidade` varchar(20) DEFAULT NULL,
  `estadoentrega` char(2) NOT NULL DEFAULT 'PE',
  `ncupomfiscal` varchar(6) NOT NULL DEFAULT '0',
  `cancelada` char(1) NOT NULL DEFAULT 'N',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `ecfCPFCNPJconsumidor` varchar(14) DEFAULT NULL,
  `numeroECF` varchar(3) DEFAULT NULL,
  `ecfcontadorcupomfiscal` varchar(10) DEFAULT NULL,
  `classe` varchar(4) NOT NULL DEFAULT '0000',
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `osnrfabricacao` varchar(40) DEFAULT NULL,
  `marca` varchar(30) DEFAULT NULL,
  `modelo` varchar(30) DEFAULT NULL,
  `ano` int(4) DEFAULT NULL,
  `placa` varchar(8) DEFAULT NULL,
  `renavam` varchar(40) DEFAULT NULL,
  `EADRegistroDAV` varchar(33) DEFAULT NULL,
  `contadorRGECF` varchar(6) NOT NULL DEFAULT ' ',
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;

/*Table structure for table `contdevolucao` */

CREATE TABLE `contdevolucao` (
  `numero` int(8) NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `finalizada` char(1) NOT NULL DEFAULT 'N',
  `documento` int(8) DEFAULT NULL,
  `ip` varchar(15) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `notafiscal` int(6) NOT NULL DEFAULT '0',
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=53 DEFAULT CHARSET=latin1;

/*Table structure for table `contdocs` */

CREATE TABLE `contdocs` (
  `ip` varchar(15) DEFAULT NULL,
  `documento` int(10) NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `Totalbruto` decimal(12,2) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `desconto` decimal(10,2) DEFAULT NULL,
  `total` decimal(12,2) DEFAULT NULL,
  `NrParcelas` int(3) DEFAULT NULL,
  `vendedor` char(3) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `Observacao` text,
  `classe` varchar(4) DEFAULT NULL,
  `dataexe` date DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `historico` varchar(25) DEFAULT NULL,
  `vrjuros` decimal(10,2) NOT NULL DEFAULT '0.00',
  `tipopagamento` char(2) NOT NULL DEFAULT '',
  `encargos` decimal(12,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `estornado` char(1) NOT NULL DEFAULT '',
  `enderecoentrega` varchar(150) DEFAULT NULL,
  `custos` decimal(12,2) NOT NULL DEFAULT '0.00',
  `devolucaovenda` decimal(12,2) NOT NULL DEFAULT '0.00',
  `devolucaorecebimento` decimal(12,2) NOT NULL DEFAULT '0.00',
  `nrboletobancario` bigint(12) NOT NULL DEFAULT '0',
  `nrnotafiscal` bigint(12) NOT NULL DEFAULT '0',
  `classedevolucao` varchar(4) DEFAULT NULL,
  `responsavelreceber` varchar(60) NOT NULL DEFAULT '',
  `numeroentrega` varchar(10) NOT NULL DEFAULT '',
  `cidadeentrega` varchar(60) DEFAULT NULL,
  `cepentrega` varchar(8) DEFAULT NULL,
  `bairroentrega` varchar(20) DEFAULT NULL,
  `horaentrega` time DEFAULT NULL,
  `dataentrega` date DEFAULT NULL,
  `obsentrega` varchar(150) DEFAULT NULL,
  `concluido` char(1) NOT NULL DEFAULT 'N',
  `cartaofidelidade` varchar(20) NOT NULL DEFAULT '0',
  `bordero` char(1) NOT NULL DEFAULT 'S',
  `valorservicos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descontoservicos` decimal(8,2) NOT NULL DEFAULT '0.00',
  `romaneio` char(1) NOT NULL DEFAULT 'N',
  `hora` time DEFAULT NULL,
  `entregaconcluida` char(1) NOT NULL DEFAULT 'N',
  `dataentregaconcluida` date DEFAULT NULL,
  `operadorentrega` varchar(10) DEFAULT NULL,
  `ncupomfiscal` varchar(10) DEFAULT NULL,
  `nreducaoz` varchar(10) DEFAULT NULL,
  `ecfnumero` char(3) DEFAULT NULL,
  `TEF` char(1) NOT NULL DEFAULT 'N',
  `ecfValorCancelamentos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `NF_e` char(1) NOT NULL DEFAULT 'N',
  `estadoentrega` char(2) NOT NULL DEFAULT 'PE',
  `ecfConsumidor` varchar(30) DEFAULT NULL,
  `ecfCPFCNPJconsumidor` varchar(14) DEFAULT NULL,
  `ecfEndConsumidor` text,
  `prevendanumero` varchar(30) NOT NULL DEFAULT '0',
  `ecfcontadorcupomfiscal` varchar(10) DEFAULT NULL,
  `ecftotalliquido` decimal(10,2) NOT NULL DEFAULT '0.00',
  `contadornaofiscalGNF` varchar(6) DEFAULT NULL,
  `contadordebitocreditoCDC` varchar(4) DEFAULT NULL,
  `totalICMScupomfiscal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `troco` decimal(8,2) NOT NULL DEFAULT '0.00',
  `davnumero` int(10) DEFAULT '0',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `ecfMFadicional` varchar(2) DEFAULT NULL,
  `ecftipo` varchar(7) DEFAULT NULL,
  `ecfmarca` varchar(15) DEFAULT NULL,
  `ecfmodelo` varchar(20) DEFAULT NULL,
  `estoqueatualizado` char(1) NOT NULL DEFAULT 'S',
  `serienf` varchar(3) DEFAULT NULL,
  `EADRegistroDAV` varchar(33) DEFAULT NULL,
  `EADr06` varchar(33) DEFAULT NULL,
  `tipopagamentoECF` varchar(2) DEFAULT NULL,
  `modeloDOCFiscal` varchar(2) NOT NULL DEFAULT '2D',
  `subserienf` varchar(3) NOT NULL DEFAULT '1',
  `COOGNF` varchar(6) NOT NULL DEFAULT ' ',
  `devolucaonumero` int(7) NOT NULL DEFAULT '0',
  `dependente` varchar(30) DEFAULT NULL,
  `ecfusuariosubstituicao` varchar(2) DEFAULT NULL,
  PRIMARY KEY (`documento`),
  KEY `ip` (`ip`),
  KEY `codigocliente` (`codigocliente`)
) ENGINE=MyISAM AUTO_INCREMENT=85321 DEFAULT CHARSET=latin1;

/*Table structure for table `contdownload` */

CREATE TABLE `contdownload` (
  `id` int(5) NOT NULL AUTO_INCREMENT,
  `empresa` varchar(100) DEFAULT NULL,
  `responsavel` varchar(100) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `telefone` varchar(20) DEFAULT NULL,
  `cidade` varchar(60) DEFAULT NULL,
  `estado` char(2) DEFAULT NULL,
  `sistema` varchar(30) DEFAULT NULL,
  `respondida` char(1) DEFAULT 'N',
  `textoresposta` text,
  `data` date DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `contentradamateriaprima` */

CREATE TABLE `contentradamateriaprima` (
  `ip` varchar(15) DEFAULT NULL,
  `numero` int(8) NOT NULL AUTO_INCREMENT,
  `notafiscal` varchar(20) DEFAULT NULL,
  `dataemissao` date DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `operadorfinalizacao` varchar(10) DEFAULT NULL,
  `finalizado` char(1) NOT NULL DEFAULT 'N',
  `datafinalizado` date DEFAULT NULL,
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `contfinanceiro` */

CREATE TABLE `contfinanceiro` (
  `codigofilial` varchar(5) DEFAULT NULL,
  `conta` int(5) DEFAULT NULL,
  `descricao` varchar(30) DEFAULT NULL,
  `subconta` int(5) NOT NULL DEFAULT '0',
  `descricaosub` varchar(30) DEFAULT NULL,
  `abater` char(1) NOT NULL DEFAULT 'S'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `continventario` */

CREATE TABLE `continventario` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `ano` int(4) DEFAULT NULL,
  `numero` int(2) NOT NULL DEFAULT '0',
  `encerrado` char(1) NOT NULL DEFAULT 'N',
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `total` decimal(14,0) NOT NULL DEFAULT '0',
  `codigofilial` varchar(5) DEFAULT NULL,
  `dataencerramento` date DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=19 DEFAULT CHARSET=latin1;

/*Table structure for table `contlicitacao` */

CREATE TABLE `contlicitacao` (
  `numero` int(10) NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `encerrada` char(1) NOT NULL DEFAULT 'N',
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `cliente` varchar(50) NOT NULL DEFAULT '',
  `cabecalho` text,
  `rodape` text,
  `ip` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `contnfsaida` */

CREATE TABLE `contnfsaida` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `notafiscal` bigint(12) DEFAULT NULL,
  `documento` int(8) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `dataemissao` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `cfop` varchar(50) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `cnpjcpf` varchar(18) DEFAULT NULL,
  `endereco` varchar(50) DEFAULT NULL,
  `bairro` varchar(20) DEFAULT NULL,
  `cidade` varchar(30) DEFAULT NULL,
  `cep` varchar(8) DEFAULT NULL,
  `estado` char(2) DEFAULT NULL,
  `telefone` varchar(15) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `inscricao` varchar(20) DEFAULT NULL,
  `total` decimal(12,2) NOT NULL DEFAULT '0.00',
  `desconto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `liquido` decimal(12,2) NOT NULL DEFAULT '0.00',
  `transportadora` varchar(40) DEFAULT NULL,
  `placa` varchar(8) DEFAULT NULL,
  `inscricaotr` varchar(20) DEFAULT NULL,
  `cnpjcpftr` varchar(18) DEFAULT NULL,
  `obs` text,
  `basecalculo` decimal(10,2) NOT NULL DEFAULT '0.00',
  `totalicms` decimal(10,2) NOT NULL DEFAULT '0.00',
  `basecalculoICMSST` decimal(10,2) NOT NULL DEFAULT '0.00',
  `totalICMSST` decimal(10,2) NOT NULL DEFAULT '0.00',
  `TotalProduto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `totalfrete` decimal(8,2) NOT NULL DEFAULT '0.00',
  `totalseguro` decimal(8,2) NOT NULL DEFAULT '0.00',
  `totaldesconto` decimal(8,2) NOT NULL DEFAULT '0.00',
  `totalipi` decimal(8,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cofins` decimal(8,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(8,2) NOT NULL DEFAULT '0.00',
  `totalNF` decimal(10,2) NOT NULL DEFAULT '0.00',
  `totalservicos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `basecalculoservicos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `totalISSQN` decimal(10,2) NOT NULL DEFAULT '0.00',
  `tipofrete` char(1) NOT NULL DEFAULT '1',
  `codcliente` int(6) NOT NULL DEFAULT '0',
  `basecalculoipi` decimal(10,2) NOT NULL DEFAULT '0.00',
  `situacaoNF` char(1) NOT NULL DEFAULT 'N',
  `pesobruto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `volumes` int(4) NOT NULL DEFAULT '0',
  `especievolume` varchar(20) DEFAULT NULL,
  `marca` varchar(20) DEFAULT NULL,
  `numero` int(10) NOT NULL DEFAULT '0',
  `quantidadevolume` int(4) NOT NULL DEFAULT '0',
  `tipo` varchar(10) NOT NULL DEFAULT '0-Entrada',
  `tipoemissao` varchar(15) NOT NULL DEFAULT '1-Normal',
  `finalidade` varchar(15) DEFAULT '1-Normal',
  `codfornecedor` int(8) NOT NULL DEFAULT '0',
  `aliquotaICMS` decimal(5,2) NOT NULL DEFAULT '17.00',
  `selofiscal` bigint(20) NOT NULL DEFAULT '0',
  `codigoANTT` varchar(30) DEFAULT NULL,
  `totalICMSfrete` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cfoptransportador` varchar(5) DEFAULT NULL,
  `exportarfiscal` char(1) NOT NULL DEFAULT 'S',
  `serie` char(3) NOT NULL DEFAULT '1',
  `ncupomfiscal` varchar(10) DEFAULT NULL,
  `modelodocfiscal` char(2) NOT NULL DEFAULT '55',
  `chave_nfe` varchar(44) DEFAULT NULL,
  `indicadorpagamento` char(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

/*Table structure for table `contpedido` */

CREATE TABLE `contpedido` (
  `codigofilial` varchar(5) DEFAULT NULL,
  `ip` varchar(15) DEFAULT NULL,
  `numero` int(8) NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `observacao` text,
  `codigofornecedor` int(6) NOT NULL DEFAULT '0',
  `encerrado` char(1) NOT NULL DEFAULT 'N',
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Table structure for table `contperdas` */

CREATE TABLE `contperdas` (
  `numero` int(10) NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `encerrada` char(1) NOT NULL DEFAULT 'N',
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `ip` varchar(15) DEFAULT NULL,
  `nf` varchar(12) DEFAULT NULL,
  `nfserie` varchar(3) DEFAULT NULL,
  `cfop` char(5) DEFAULT NULL,
  `total` decimal(12,2) DEFAULT '0.00',
  `tipo` varchar(1) NOT NULL DEFAULT 'P',
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;

/*Table structure for table `contprevendas` */

CREATE TABLE `contprevendas` (
  `enderecoip` varchar(15) DEFAULT NULL,
  `numero` int(5) unsigned NOT NULL AUTO_INCREMENT,
  `numeroDAVFilial` int(7) NOT NULL DEFAULT '0',
  `data` date DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `finalizada` char(1) NOT NULL DEFAULT 'N',
  `observacao` varchar(400) DEFAULT NULL,
  `devolucao` int(8) DEFAULT NULL,
  `responsavelreceber` varchar(50) NOT NULL DEFAULT '',
  `enderecoentrega` varchar(50) NOT NULL DEFAULT '',
  `numeroentrega` varchar(10) NOT NULL DEFAULT '',
  `cidadeentrega` varchar(30) DEFAULT NULL,
  `cepentrega` varchar(8) DEFAULT NULL,
  `bairroentrega` varchar(20) DEFAULT NULL,
  `horaentrega` time DEFAULT NULL,
  `dataentrega` date DEFAULT NULL,
  `valor` decimal(12,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `desconto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `origem` varchar(20) NOT NULL DEFAULT 'Balcao',
  `datafinalizacao` date DEFAULT NULL,
  `cartaofidelidade` varchar(20) DEFAULT NULL,
  `estadoentrega` char(2) NOT NULL DEFAULT 'PE',
  `ncupomfiscal` varchar(6) NOT NULL DEFAULT '0',
  `cancelada` char(1) NOT NULL DEFAULT 'N',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `ecfCPFCNPJconsumidor` varchar(14) DEFAULT NULL,
  `numeroECF` char(3) DEFAULT NULL,
  `ecfcontadorcupomfiscal` varchar(10) DEFAULT NULL,
  `classe` varchar(4) NOT NULL DEFAULT '0000',
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `osnrfabricacao` varchar(40) DEFAULT NULL,
  `marca` varchar(30) DEFAULT NULL,
  `modelo` varchar(30) DEFAULT NULL,
  `ano` int(4) DEFAULT NULL,
  `placa` varchar(8) DEFAULT NULL,
  `renavam` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`numero`),
  UNIQUE KEY `numero` (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=9441 DEFAULT CHARSET=latin1;

/*Table structure for table `contprevendaspaf` */

CREATE TABLE `contprevendaspaf` (
  `enderecoip` varchar(15) DEFAULT NULL,
  `numero` int(5) NOT NULL AUTO_INCREMENT,
  `numeroDAVFilial` int(7) NOT NULL DEFAULT '0',
  `data` date DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `finalizada` char(1) NOT NULL DEFAULT 'N',
  `observacao` varchar(400) DEFAULT NULL,
  `devolucao` int(8) DEFAULT NULL,
  `responsavelreceber` varchar(50) NOT NULL DEFAULT '',
  `enderecoentrega` varchar(50) NOT NULL DEFAULT '',
  `numeroentrega` varchar(10) NOT NULL DEFAULT '',
  `cidadeentrega` varchar(30) DEFAULT NULL,
  `cepentrega` varchar(8) DEFAULT NULL,
  `bairroentrega` varchar(20) DEFAULT NULL,
  `horaentrega` time DEFAULT NULL,
  `dataentrega` date DEFAULT NULL,
  `valor` decimal(12,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `desconto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `origem` varchar(20) NOT NULL DEFAULT 'Balcao',
  `datafinalizacao` date DEFAULT NULL,
  `cartaofidelidade` varchar(20) DEFAULT NULL,
  `estadoentrega` char(2) NOT NULL DEFAULT 'PE',
  `ncupomfiscal` varchar(6) NOT NULL DEFAULT '0',
  `cancelada` char(1) NOT NULL DEFAULT 'N',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `ecfCPFCNPJconsumidor` varchar(14) DEFAULT NULL,
  `numeroECF` varchar(3) DEFAULT NULL,
  `ecfcontadorcupomfiscal` varchar(10) DEFAULT NULL,
  `classe` varchar(4) NOT NULL DEFAULT '0000',
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `osnrfabricacao` varchar(40) DEFAULT NULL,
  `marca` varchar(30) DEFAULT NULL,
  `modelo` varchar(30) DEFAULT NULL,
  `ano` int(4) DEFAULT NULL,
  `placa` varchar(8) DEFAULT NULL,
  `renavam` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=37 DEFAULT CHARSET=latin1;

/*Table structure for table `contproducao` */

CREATE TABLE `contproducao` (
  `ip` varchar(15) DEFAULT NULL,
  `numero` int(8) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `operadorfinalizacao` varchar(10) DEFAULT NULL,
  `finalizado` char(1) NOT NULL DEFAULT 'N',
  `datafinalizado` date DEFAULT NULL,
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `contratos` */

CREATE TABLE `contratos` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `contrato` varchar(20) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `datavenda` date DEFAULT NULL,
  `valorcomissao` decimal(10,2) NOT NULL DEFAULT '0.00',
  `operador` varchar(10) DEFAULT NULL,
  `cliente` varchar(40) DEFAULT NULL,
  `modelo` varchar(30) DEFAULT NULL,
  `vendedor` varchar(40) DEFAULT NULL,
  `situacao` varchar(10) DEFAULT 'ABERTO',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `contratosclientes` */

CREATE TABLE `contratosclientes` (
  `inc` int(3) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(40) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `texto` text,
  `codigofilial` varchar(5) DEFAULT NULL,
  `cabecalho` varchar(100) DEFAULT NULL,
  `denominacaocliente` varchar(30) DEFAULT NULL,
  `denominacaoempresa` varchar(30) DEFAULT NULL,
  `clausulacabecalho` text,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `contrelatoriogerencial` */

CREATE TABLE `contrelatoriogerencial` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `ecfnumero` varchar(4) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `gnf` varchar(6) DEFAULT NULL,
  `grg` varchar(6) DEFAULT NULL,
  `cdc` varchar(4) DEFAULT NULL,
  `horaemissao` time DEFAULT NULL,
  `tipopagamentoECF` char(2) NOT NULL DEFAULT 'RG',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `EADDados` varchar(33) DEFAULT NULL,
  `denominacao` varchar(2) NOT NULL DEFAULT 'RG',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=877 DEFAULT CHARSET=latin1;

/*Table structure for table `contreposicao` */

CREATE TABLE `contreposicao` (
  `numero` int(10) NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `encerrada` char(1) NOT NULL DEFAULT 'N',
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `ip` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

/*Table structure for table `contromaneio` */

CREATE TABLE `contromaneio` (
  `inc` int(7) NOT NULL AUTO_INCREMENT,
  `ip` varchar(15) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `finalizado` char(1) DEFAULT NULL,
  `dataencerramento` date DEFAULT NULL,
  `operadorencerramento` varchar(10) DEFAULT NULL,
  `transportadora` varchar(40) DEFAULT NULL,
  `veiculo` varchar(20) DEFAULT NULL,
  `placa` varchar(8) DEFAULT NULL,
  `distanciakm` decimal(8,2) DEFAULT NULL,
  `observacao` text,
  `pesobruto` decimal(10,2) DEFAULT NULL,
  `pesoliquido` decimal(10,2) DEFAULT NULL,
  `m3` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `conttransf` */

CREATE TABLE `conttransf` (
  `numero` int(5) unsigned NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `lancada` char(1) NOT NULL DEFAULT '',
  `codigofilial` varchar(5) DEFAULT NULL,
  `Filial` varchar(20) DEFAULT NULL,
  `FilialDestino` varchar(5) NOT NULL DEFAULT '',
  `filialorigem` varchar(5) NOT NULL DEFAULT '',
  `operador` varchar(10) NOT NULL DEFAULT '',
  `observacao` varchar(40) DEFAULT NULL,
  `totitens` int(4) NOT NULL DEFAULT '0',
  `totcusto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `totvenda` decimal(10,2) NOT NULL DEFAULT '0.00',
  `nrnotafiscal` bigint(12) NOT NULL DEFAULT '0',
  `cfop` varchar(5) NOT NULL DEFAULT '5.152',
  UNIQUE KEY `numero` (`numero`),
  KEY `numero_2` (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=15 DEFAULT CHARSET=latin1;

/*Table structure for table `contvencidos` */

CREATE TABLE `contvencidos` (
  `numero` int(10) NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `encerrada` char(1) NOT NULL DEFAULT 'N',
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `ip` varchar(15) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Table structure for table `contvendapadronizada` */

CREATE TABLE `contvendapadronizada` (
  `numero` int(3) NOT NULL AUTO_INCREMENT,
  `codigofilial` char(5) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `desconto` decimal(6,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`numero`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `crmovclientes` */

CREATE TABLE `crmovclientes` (
  `nome` varchar(50) DEFAULT NULL,
  `codigo` int(6) DEFAULT NULL,
  `documento` int(10) NOT NULL DEFAULT '0',
  `datacompra` date DEFAULT NULL,
  `datarenegociacao` date DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `jurospago` decimal(12,2) NOT NULL DEFAULT '0.00',
  `datapagamento` date DEFAULT NULL,
  `Ultjurospago` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ultvencimento` date DEFAULT NULL,
  `parcela` int(3) DEFAULT NULL,
  `Valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `percmulta` decimal(5,2) DEFAULT NULL,
  `vrmulta` decimal(8,2) DEFAULT NULL,
  `percjuro` decimal(5,3) DEFAULT NULL,
  `vrjuros` decimal(8,2) NOT NULL DEFAULT '0.00',
  `valoratual` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ultvaloratual` decimal(12,2) NOT NULL DEFAULT '0.00',
  `VrCapitalRec` decimal(10,2) NOT NULL DEFAULT '0.00',
  `DescontoCap` decimal(10,2) DEFAULT NULL,
  `UltCapRec` decimal(12,2) DEFAULT NULL,
  `DescontoJur` decimal(10,2) DEFAULT NULL,
  `TotalDescontos` decimal(10,2) DEFAULT NULL,
  `ValorliquidoRec` decimal(10,2) DEFAULT NULL,
  `ultpagamento` date DEFAULT NULL,
  `usuario` varchar(10) DEFAULT NULL,
  `vendedor` char(3) DEFAULT NULL,
  `Diasdecorrido` int(4) NOT NULL DEFAULT '0',
  `sequencia` varchar(50) NOT NULL DEFAULT '0',
  `Observacao` text,
  `nrParcela` varchar(5) DEFAULT NULL,
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `bloquete` varchar(20) NOT NULL DEFAULT '',
  `classe` varchar(20) DEFAULT NULL,
  `vrultpagamento` decimal(10,2) DEFAULT NULL,
  `quitado` char(1) NOT NULL DEFAULT 'N',
  `datacalcjuros` date DEFAULT NULL,
  `sequenciainc` int(10) NOT NULL AUTO_INCREMENT,
  `valorcorrigido` decimal(12,2) NOT NULL DEFAULT '0.00',
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `porconta` decimal(12,2) NOT NULL DEFAULT '0.00',
  `valorpago` decimal(10,2) NOT NULL DEFAULT '0.00',
  `porcontd` decimal(10,2) NOT NULL DEFAULT '0.00',
  `jurosacumulado` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ultjurosacumulado` decimal(10,2) NOT NULL DEFAULT '0.00',
  `desconto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dependente` varchar(50) DEFAULT NULL,
  `tipopagamento` char(2) NOT NULL DEFAULT '',
  `ultporconta` decimal(12,2) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `cpfcnpj` varchar(14) DEFAULT NULL,
  `cobrador` char(3) DEFAULT NULL,
  `filialpagamento` varchar(5) NOT NULL DEFAULT '00000',
  `ip` varchar(15) DEFAULT NULL,
  `comissaopaga` char(1) NOT NULL DEFAULT 'N',
  `interpolador` varchar(8) DEFAULT NULL,
  `mes` int(2) DEFAULT NULL,
  PRIMARY KEY (`sequenciainc`),
  KEY `IndDocCrMovCli` (`documento`),
  KEY `IndSeqCrMovCli` (`sequenciainc`),
  KEY `IndCodCrMovCli` (`codigo`,`valoratual`),
  KEY `IndCrValorAtual` (`valoratual`),
  KEY `IndCrVencimento` (`vencimento`),
  KEY `IndNomecodigo` (`nome`,`codigo`),
  KEY `datacompra` (`datacompra`)
) ENGINE=MyISAM AUTO_INCREMENT=59895 DEFAULT CHARSET=latin1;

/*Table structure for table `crmovclientespagas` */

CREATE TABLE `crmovclientespagas` (
  `nome` varchar(50) DEFAULT NULL,
  `codigo` int(6) DEFAULT NULL,
  `documento` int(10) NOT NULL DEFAULT '0',
  `datacompra` date DEFAULT NULL,
  `datarenegociacao` date DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `jurospago` decimal(12,2) NOT NULL DEFAULT '0.00',
  `datapagamento` date DEFAULT NULL,
  `Ultjurospago` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ultvencimento` date DEFAULT NULL,
  `parcela` int(3) DEFAULT NULL,
  `Valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `percmulta` decimal(5,2) DEFAULT NULL,
  `vrmulta` decimal(8,2) DEFAULT NULL,
  `percjuro` decimal(5,3) DEFAULT NULL,
  `vrjuros` decimal(8,2) NOT NULL DEFAULT '0.00',
  `valoratual` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ultvaloratual` decimal(12,2) NOT NULL DEFAULT '0.00',
  `VrCapitalRec` decimal(10,2) NOT NULL DEFAULT '0.00',
  `DescontoCap` decimal(10,2) DEFAULT NULL,
  `UltCapRec` decimal(12,2) DEFAULT NULL,
  `DescontoJur` decimal(10,2) DEFAULT NULL,
  `TotalDescontos` decimal(10,2) DEFAULT NULL,
  `ValorliquidoRec` decimal(10,2) DEFAULT NULL,
  `ultpagamento` date DEFAULT NULL,
  `usuario` varchar(10) DEFAULT NULL,
  `vendedor` char(3) DEFAULT NULL,
  `Diasdecorrido` int(4) NOT NULL DEFAULT '0',
  `sequencia` varchar(35) NOT NULL DEFAULT '0',
  `Observacao` text,
  `nrParcela` varchar(5) DEFAULT NULL,
  `encargos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `bloquete` varchar(20) NOT NULL DEFAULT '',
  `classe` varchar(20) DEFAULT NULL,
  `vrultpagamento` decimal(10,2) DEFAULT NULL,
  `quitado` char(1) NOT NULL DEFAULT 'N',
  `datacalcjuros` date DEFAULT NULL,
  `sequenciainc` int(10) NOT NULL AUTO_INCREMENT,
  `valorcorrigido` decimal(12,2) NOT NULL DEFAULT '0.00',
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `porconta` decimal(12,2) NOT NULL DEFAULT '0.00',
  `valorpago` decimal(10,2) NOT NULL DEFAULT '0.00',
  `porcontd` decimal(10,2) NOT NULL DEFAULT '0.00',
  `jurosacumulado` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ultjurosacumulado` decimal(10,2) NOT NULL DEFAULT '0.00',
  `desconto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dependente` varchar(50) DEFAULT NULL,
  `tipopagamento` char(2) NOT NULL DEFAULT '',
  `ultporconta` decimal(12,2) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `cpfcnpj` varchar(14) DEFAULT NULL,
  `cobrador` char(3) DEFAULT NULL,
  `filialpagamento` varchar(5) NOT NULL DEFAULT '00000',
  `ip` varchar(15) DEFAULT NULL,
  `comissaopaga` char(1) NOT NULL DEFAULT 'N',
  `interpolador` varchar(8) DEFAULT NULL,
  `mes` int(2) DEFAULT NULL,
  KEY `IndDocCrMovCli` (`documento`),
  KEY `IndSeqCrMovCli` (`sequenciainc`),
  KEY `IndCodCrMovCli` (`codigo`,`valoratual`),
  KEY `IndCrValorAtual` (`valoratual`),
  KEY `IndCrVencimento` (`vencimento`),
  KEY `IndNomecodigo` (`nome`,`codigo`),
  KEY `datacompra` (`datacompra`)
) ENGINE=MyISAM AUTO_INCREMENT=59895 DEFAULT CHARSET=latin1;

/*Table structure for table `curriculum` */

CREATE TABLE `curriculum` (
  `id` int(5) NOT NULL AUTO_INCREMENT,
  `cpf` varchar(14) DEFAULT NULL,
  `nome` varchar(60) DEFAULT NULL,
  `apelido` varchar(15) DEFAULT NULL,
  `senha` varchar(20) DEFAULT NULL,
  `lembretesenha` varchar(30) DEFAULT NULL,
  `nascimento` date DEFAULT NULL,
  `estadocivil` varchar(15) DEFAULT NULL,
  `escolaridade` varchar(30) DEFAULT NULL,
  `linguas` varchar(30) DEFAULT NULL,
  `sexo` char(1) DEFAULT NULL,
  `pais` varchar(30) DEFAULT NULL,
  `estado` char(2) DEFAULT NULL,
  `cidade` varchar(30) DEFAULT NULL,
  `bairro` varchar(30) DEFAULT NULL,
  `endereco` varchar(50) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `email` varchar(60) DEFAULT NULL,
  `url` varchar(60) DEFAULT NULL,
  `telefone` varchar(20) DEFAULT NULL,
  `celular` varchar(20) DEFAULT NULL,
  `cargo` varchar(30) DEFAULT NULL,
  `salario` decimal(10,2) DEFAULT NULL,
  `referencias` varchar(50) DEFAULT NULL,
  `apresentacao` text,
  `datacad` date DEFAULT NULL,
  `foto` varchar(60) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `curriculumcargos` */

CREATE TABLE `curriculumcargos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cargo` varchar(40) DEFAULT NULL,
  `vagas` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `dbcartoes` */

CREATE TABLE `dbcartoes` (
  `codigofilial` varchar(5) DEFAULT '0',
  `id` varchar(15) DEFAULT '0',
  `valor` decimal(12,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `interpolador` varchar(8) DEFAULT NULL,
  `contabancaria` varchar(10) NOT NULL DEFAULT '',
  `historico` varchar(15) NOT NULL DEFAULT '',
  `tipopagamento` char(2) DEFAULT NULL,
  `datadeposito` date DEFAULT NULL,
  `despesas` decimal(10,2) NOT NULL DEFAULT '0.00'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `dbcheques` */

CREATE TABLE `dbcheques` (
  `codigofilial` varchar(5) DEFAULT '0',
  `id` varchar(15) DEFAULT '0',
  `valor` decimal(12,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `interpolador` varchar(8) DEFAULT NULL,
  `contabancaria` varchar(10) NOT NULL DEFAULT '',
  `historico` varchar(15) NOT NULL DEFAULT '',
  `tipopagamento` char(2) DEFAULT NULL,
  `datadeposito` date DEFAULT NULL,
  `despesas` decimal(10,2) NOT NULL DEFAULT '0.00'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `dbpagamentos` */

CREATE TABLE `dbpagamentos` (
  `codigofilial` char(5) DEFAULT '0',
  `id` bigint(15) NOT NULL AUTO_INCREMENT,
  `valor` decimal(12,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `operador` char(10) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=757 DEFAULT CHARSET=latin1;

/*Table structure for table `dependentes` */

CREATE TABLE `dependentes` (
  `id` int(6) NOT NULL AUTO_INCREMENT,
  `codigocliente` int(6) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `nome` varchar(30) DEFAULT NULL,
  `telefone` varchar(16) DEFAULT NULL,
  `cpf` varchar(14) DEFAULT NULL,
  `identidade` varchar(15) DEFAULT NULL,
  `nascimento` date DEFAULT NULL,
  `profissao` varchar(40) DEFAULT NULL,
  `salario` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `despesas` */

CREATE TABLE `despesas` (
  `codigofilial` varchar(5) DEFAULT NULL,
  `conta` varchar(5) NOT NULL DEFAULT '0',
  `descricao` varchar(30) DEFAULT NULL,
  `grupo` varchar(15) DEFAULT NULL,
  `liberada` char(1) NOT NULL DEFAULT 'S',
  `natureza` char(2) NOT NULL DEFAULT '01',
  KEY `conta` (`descricao`,`conta`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `despesasub` */

CREATE TABLE `despesasub` (
  `codigofilial` varchar(5) DEFAULT NULL,
  `idsubconta` varchar(5) NOT NULL DEFAULT '0',
  `descricao` varchar(30) DEFAULT NULL,
  `idconta` varchar(5) DEFAULT NULL,
  `despesa` char(1) DEFAULT NULL,
  `grupo` varchar(15) DEFAULT NULL,
  `creditobancario` char(1) NOT NULL DEFAULT 'N',
  `debitobancario` char(1) NOT NULL DEFAULT 'N',
  `vendedorcomissao` char(3) NOT NULL DEFAULT '',
  `liberada` char(1) NOT NULL DEFAULT 'S',
  `cobradorcomissao` char(3) NOT NULL DEFAULT '',
  `tipodespesa` char(1) NOT NULL DEFAULT 'F',
  KEY `idconta` (`idconta`,`descricao`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `devolucao` */

CREATE TABLE `devolucao` (
  `codigofilial` varchar(5) DEFAULT NULL,
  `id` varchar(20) DEFAULT NULL,
  `idinc` int(3) unsigned NOT NULL AUTO_INCREMENT,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `encerrado` char(1) NOT NULL DEFAULT 'N',
  `usuario` varchar(10) DEFAULT NULL,
  `documento` int(6) unsigned NOT NULL DEFAULT '0',
  `numero` int(6) unsigned NOT NULL DEFAULT '0',
  `vendedor` char(3) NOT NULL DEFAULT '',
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `entrada` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descontoclasse` decimal(10,2) NOT NULL DEFAULT '0.00',
  `abatimentofechamento` decimal(10,2) NOT NULL DEFAULT '0.00',
  `finalizada` char(1) NOT NULL DEFAULT 'N',
  `situacao` varchar(15) DEFAULT NULL,
  `observacao` text,
  `data` date DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT 'Devolução',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `deposito` decimal(10,2) NOT NULL DEFAULT '0.00',
  `prateleiras` decimal(10,2) NOT NULL DEFAULT '0.00',
  `notafiscal` int(6) NOT NULL DEFAULT '0',
  KEY `idinc_2` (`idinc`),
  KEY `indexNumero` (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=58 DEFAULT CHARSET=latin1;

/*Table structure for table `dicodprodutos` */

CREATE TABLE `dicodprodutos` (
  `id` int(7) NOT NULL AUTO_INCREMENT,
  `codigo` varchar(20) NOT NULL DEFAULT '',
  `descricao` varchar(50) NOT NULL DEFAULT '',
  `fornecedor` varchar(50) NOT NULL DEFAULT '',
  `codfornecedor` varchar(20) NOT NULL DEFAULT '',
  `fracao` int(3) NOT NULL DEFAULT '0',
  `fracaoembala` int(3) NOT NULL DEFAULT '0',
  `cnpjcpf` varchar(14) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

/*Table structure for table `downloads` */

CREATE TABLE `downloads` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `caminho` varchar(60) DEFAULT NULL,
  `arquivo` varchar(60) NOT NULL DEFAULT '',
  `descricao` text,
  `versao` varchar(10) DEFAULT NULL,
  `empresa` varchar(30) DEFAULT NULL,
  `os` varchar(10) DEFAULT NULL,
  `data` date NOT NULL DEFAULT '0000-00-00',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `ecf` */

CREATE TABLE `ecf` (
  `modelo` varchar(30) NOT NULL DEFAULT '0',
  `id` varchar(15) DEFAULT '0',
  `aliquota07` char(2) DEFAULT '0',
  `aliquota12` char(2) DEFAULT '0',
  `aliquota17` char(2) DEFAULT '0',
  `aliquota25` char(2) DEFAULT '0',
  `aliquota27` char(2) DEFAULT '0',
  `isencao` char(2) DEFAULT 'II',
  `substituicao` char(2) DEFAULT 'FF',
  `naoincide` char(2) DEFAULT 'NN',
  `dinheiro` char(2) DEFAULT '0',
  `cheque` char(2) DEFAULT '0',
  `cartao` char(2) DEFAULT '0',
  `crediario` char(2) DEFAULT '0',
  `recebimento` char(2) DEFAULT '0',
  `ticket` char(2) DEFAULT '00',
  `mensagem` varchar(50) DEFAULT NULL,
  `pdv` char(1) NOT NULL DEFAULT 'N',
  `prevenda` char(1) NOT NULL DEFAULT 'N',
  `numero` varchar(4) DEFAULT NULL,
  `modelocupom` char(2) NOT NULL DEFAULT '01',
  `tef` char(1) NOT NULL DEFAULT 'N',
  `modelocarne` char(2) NOT NULL DEFAULT '01',
  `modeloimpressora` char(1) NOT NULL DEFAULT '0',
  `impdiretolpt1` char(1) NOT NULL DEFAULT 'N',
  `iniciarchat` char(1) NOT NULL DEFAULT 'N',
  `impressoracheque` varchar(30) NOT NULL DEFAULT '0 - Nenhuma',
  `avancolinhas` char(2) NOT NULL DEFAULT '01',
  `modelorecibo` char(2) NOT NULL DEFAULT '02',
  `usargerenciador` char(1) NOT NULL DEFAULT 'S',
  `preencherboleto` char(1) NOT NULL DEFAULT 'N',
  `buscaautomatica` char(1) NOT NULL DEFAULT 'S',
  `isencaoiss` char(2) NOT NULL DEFAULT 'SI',
  `substituicaoiss` char(2) NOT NULL DEFAULT 'SF',
  `naoincideiss` char(2) NOT NULL DEFAULT 'SN',
  `imprimepesocupom` char(1) NOT NULL DEFAULT 'N',
  `impcabecalhocupom` char(1) NOT NULL DEFAULT 'S',
  `modelopromiss` char(1) NOT NULL DEFAULT '1',
  `modeloetiquetadora` char(1) NOT NULL DEFAULT '0',
  `tefdedicado` char(1) NOT NULL DEFAULT 'N',
  `setupimpressora` char(1) NOT NULL DEFAULT 'N',
  `consultapreco` char(1) NOT NULL DEFAULT 'S',
  `ecfConsumidor` varchar(30) DEFAULT NULL,
  `ecfCPFCNPJconsumidor` varchar(14) DEFAULT NULL,
  `ecfEndConsumidor` text,
  `modeloGaveta` char(1) NOT NULL DEFAULT '0',
  `numerofabricacaoECF` varchar(20) DEFAULT NULL,
  `tipoECF` varchar(7) DEFAULT NULL,
  `marcaECF` varchar(20) DEFAULT NULL,
  `modeloECF` varchar(20) DEFAULT NULL,
  `imprimeprazodias` char(1) NOT NULL DEFAULT 'N'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `empresa` */

CREATE TABLE `empresa` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `nome` varchar(60) NOT NULL DEFAULT '',
  `fantasia` varchar(60) DEFAULT NULL,
  `endereco` varchar(60) DEFAULT NULL,
  `bairro` varchar(60) DEFAULT NULL,
  `cidade` varchar(60) DEFAULT NULL,
  `estado` char(2) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `fone` varchar(20) DEFAULT NULL,
  `fax` varchar(20) DEFAULT NULL,
  `email` varchar(60) DEFAULT NULL,
  `url` varchar(60) DEFAULT NULL,
  `ie` varchar(20) DEFAULT NULL,
  `cnpj` varchar(20) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`,`nome`,`cnpj`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `empresas` */

CREATE TABLE `empresas` (
  `id` int(3) NOT NULL AUTO_INCREMENT,
  `empresa` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Table structure for table `entradagrade` */

CREATE TABLE `entradagrade` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `numero` int(6) NOT NULL DEFAULT '0',
  `codigofilial` varchar(5) NOT NULL DEFAULT '00000',
  `descricaograde` varchar(10) DEFAULT NULL,
  `grade` varchar(15) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `codigograde` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `entradamateriaprima` */

CREATE TABLE `entradamateriaprima` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `numero` int(6) NOT NULL DEFAULT '0',
  `codigofilial` varchar(5) DEFAULT '00000',
  `codigomateria` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `unidade` char(2) NOT NULL DEFAULT 'UN',
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `finalizado` char(1) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `entradas` */

CREATE TABLE `entradas` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `numero` int(6) unsigned NOT NULL DEFAULT '0',
  `codigo` varchar(20) NOT NULL DEFAULT '',
  `descricao` varchar(50) DEFAULT NULL,
  `Lancada` char(1) NOT NULL DEFAULT 'N',
  `fornecedor` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `codfornecedor` varchar(20) NOT NULL DEFAULT '',
  `NF` varchar(15) DEFAULT NULL,
  `QuantNF` decimal(8,2) NOT NULL DEFAULT '0.00',
  `CustoNF` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `CustoCalculado` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `UltCusto` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `PrecoVenda` decimal(10,2) NOT NULL DEFAULT '0.00',
  `PrecoAnterior` decimal(10,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `dataemissao` date DEFAULT NULL,
  `dataentrada` date DEFAULT NULL,
  `IcmsEntrada` decimal(8,2) NOT NULL DEFAULT '0.00',
  `IPI` decimal(8,2) NOT NULL DEFAULT '0.00',
  `frete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `MargemLucro` decimal(8,2) NOT NULL DEFAULT '0.00',
  `usuario` varchar(10) NOT NULL DEFAULT '',
  `codigofilial` varchar(5) NOT NULL DEFAULT '',
  `lote` varchar(15) NOT NULL DEFAULT '',
  `vencimento` date DEFAULT NULL,
  `grupo` varchar(30) NOT NULL DEFAULT '',
  `subgrupo` varchar(30) NOT NULL DEFAULT '',
  `qtdprateleiras` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtddeposito` decimal(10,2) NOT NULL DEFAULT '0.00',
  `operacao` varchar(5) DEFAULT NULL,
  `margemsemfinanciamento` decimal(8,2) NOT NULL DEFAULT '0.00',
  `icms` decimal(8,2) NOT NULL DEFAULT '0.00',
  `quantidadeanterior` decimal(10,2) NOT NULL DEFAULT '0.00',
  `customedioanterior` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(10,2) NOT NULL DEFAULT '0.00',
  `producaonumero` int(8) NOT NULL DEFAULT '0',
  `icmsproduto` decimal(5,2) NOT NULL DEFAULT '0.00',
  `rateiodespesas` decimal(10,5) DEFAULT '0.00000',
  `itemICMSST` char(1) NOT NULL DEFAULT 'N',
  `tributacao` char(3) NOT NULL DEFAULT '000',
  `cfopentrada` varchar(5) DEFAULT NULL,
  `precoatacado` decimal(10,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `bcpis` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` char(2) NOT NULL DEFAULT '01',
  `aliquotapis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(8,2) NOT NULL DEFAULT '0.00',
  `bccofins` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstcofins` char(2) NOT NULL DEFAULT '01',
  `aliquotacofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `cofins` decimal(8,2) NOT NULL DEFAULT '0.00',
  `serienf` char(3) NOT NULL DEFAULT '1',
  `unidade` char(3) NOT NULL DEFAULT 'UND',
  `bcicms` decimal(10,2) NOT NULL DEFAULT '0.00',
  `totaldesconto` decimal(8,2) NOT NULL DEFAULT '0.00',
  `valorunitario` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `totalitem` decimal(10,2) NOT NULL DEFAULT '0.00',
  `bcicmsST` decimal(10,2) NOT NULL DEFAULT '0.00',
  `valoricmsST` decimal(8,2) NOT NULL DEFAULT '0.00',
  `valorICMS` decimal(8,2) NOT NULL DEFAULT '0.00',
  `modeloNF` varchar(2) NOT NULL DEFAULT '55',
  `sequencia` int(4) NOT NULL DEFAULT '0',
  `datafabricacao` date DEFAULT NULL,
  PRIMARY KEY (`inc`),
  KEY `numero` (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=2919 DEFAULT CHARSET=latin1;

/*Table structure for table `estados` */

CREATE TABLE `estados` (
  `id` int(2) NOT NULL AUTO_INCREMENT,
  `uf` char(2) DEFAULT NULL,
  `estado` varchar(40) DEFAULT NULL,
  `codigo` int(2) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=54 DEFAULT CHARSET=latin1;

/*Table structure for table `fabricantes` */

CREATE TABLE `fabricantes` (
  `razaosocial` varchar(50) DEFAULT NULL,
  `ENDERECO` varchar(50) DEFAULT NULL,
  `CEP` varchar(10) DEFAULT NULL,
  `BAIRRO` varchar(20) DEFAULT NULL,
  `CIDADE` varchar(30) DEFAULT NULL,
  `ESTADO` char(2) DEFAULT NULL,
  `CGC` varchar(18) DEFAULT NULL,
  `INSCRICAO` varchar(20) DEFAULT NULL,
  `CPF` varchar(14) DEFAULT NULL,
  `OBSERVACAO` text,
  `TELEFONE` varchar(15) DEFAULT NULL,
  `FAX` varchar(15) DEFAULT NULL,
  `TELEFONE2` varchar(15) DEFAULT NULL,
  `FAX2` varchar(15) DEFAULT NULL,
  `TELEFONE3` varchar(15) DEFAULT NULL,
  `FAX3` varchar(15) DEFAULT NULL,
  `DATACAD` date DEFAULT NULL,
  `ULT_COMPRA` date DEFAULT NULL,
  `PRAZO_ENTG` smallint(6) DEFAULT NULL,
  `PED_MINIMO` float DEFAULT NULL,
  `EMAIL` varchar(50) DEFAULT NULL,
  `CONTATO1` varchar(25) DEFAULT NULL,
  `CONTATO2` varchar(25) DEFAULT NULL,
  `CONTATO3` varchar(25) DEFAULT NULL,
  `empresa` varchar(30) DEFAULT NULL,
  `Codigo` int(4) NOT NULL AUTO_INCREMENT,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `bonificacao` decimal(10,2) NOT NULL DEFAULT '0.00',
  `fabricante` char(1) NOT NULL DEFAULT 'S',
  `fornecedor` char(1) NOT NULL DEFAULT 'N',
  `categoria` varchar(30) NOT NULL DEFAULT '1 - Normal',
  `numero` varchar(8) DEFAULT NULL,
  `retencao` char(1) NOT NULL DEFAULT '0',
  `tipoBairro` varchar(10) DEFAULT NULL,
  `tipoEndereco` varchar(10) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `nomecliente` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Codigo`),
  KEY `Fabricantes` (`empresa`)
) ENGINE=MyISAM AUTO_INCREMENT=1031 DEFAULT CHARSET=latin1;

/*Table structure for table `filiais` */

CREATE TABLE `filiais` (
  `descricao` varchar(35) DEFAULT NULL,
  `empresa` varchar(50) DEFAULT NULL,
  `fantasia` varchar(50) DEFAULT NULL,
  `cnpj` varchar(18) DEFAULT NULL,
  `inscricao` varchar(20) DEFAULT NULL,
  `endereco` varchar(40) DEFAULT NULL,
  `numero` varchar(10) DEFAULT NULL,
  `complemento` varchar(50) DEFAULT NULL,
  `cidade` varchar(30) DEFAULT NULL,
  `bairro` varchar(20) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `estado` char(2) DEFAULT NULL,
  `telefone1` varchar(16) DEFAULT NULL,
  `telefone2` varchar(16) DEFAULT NULL,
  `telefone3` varchar(16) DEFAULT NULL,
  `email` varchar(40) DEFAULT NULL,
  `inscricaomunicipal` varchar(20) DEFAULT NULL,
  `CodigoFilial` varchar(5) NOT NULL DEFAULT '',
  `codigobanco` int(5) DEFAULT NULL,
  `banco` varchar(15) DEFAULT NULL,
  `agencia` varchar(6) DEFAULT NULL,
  `conta` varchar(20) DEFAULT NULL,
  `contactocobranca` varchar(30) DEFAULT NULL,
  `telefonecobranca` varchar(11) DEFAULT NULL,
  `grupo` varchar(15) DEFAULT NULL,
  `ativa` char(1) NOT NULL DEFAULT 'S',
  `contadespesa` varchar(5) DEFAULT NULL,
  `subconta` varchar(5) DEFAULT NULL,
  `descricaoconta` varchar(30) DEFAULT NULL,
  `descricaosubconta` varchar(30) DEFAULT NULL,
  `smtp` varchar(60) DEFAULT NULL,
  `porta` int(5) DEFAULT NULL,
  `usuarioemail` varchar(30) DEFAULT NULL,
  `senhaemail` varchar(20) DEFAULT NULL,
  `website` varchar(100) DEFAULT NULL,
  `liberacao` varchar(30) DEFAULT NULL,
  `validade` varchar(30) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `datacontrato` date DEFAULT NULL,
  `loginiqsistemas` varchar(60) DEFAULT NULL,
  `senhaiqsistemas` varchar(60) DEFAULT NULL,
  `logomarca` mediumblob,
  `descricaoCNAE` varchar(100) DEFAULT NULL,
  `CNAEsecundario` varchar(10) DEFAULT NULL,
  `descricaoCNAEsecundario` varchar(100) DEFAULT NULL,
  `CNAEsecundario2` varchar(10) DEFAULT NULL,
  `descricaoCNAEsecundario2` varchar(100) DEFAULT NULL,
  `inscricaoestadualinsctributario` varchar(20) DEFAULT NULL,
  `CNAE` varchar(10) DEFAULT NULL,
  `tipoempresa` varchar(30) DEFAULT NULL,
  `contador` varchar(50) DEFAULT NULL,
  `crccontador` varchar(10) DEFAULT NULL,
  `protect` varchar(100) DEFAULT NULL,
  `contadespesaCA` varchar(5) DEFAULT NULL,
  `subcontadespesaCA` varchar(5) DEFAULT NULL,
  `descricaocontaCA` varchar(30) DEFAULT NULL,
  `descricaosubcontaCA` varchar(30) DEFAULT NULL,
  `nomeCobranca` varchar(50) DEFAULT NULL,
  `enderecoCobranca` varchar(100) DEFAULT NULL,
  `bairroCobranca` varchar(20) DEFAULT NULL,
  `cidadeCobranca` varchar(60) DEFAULT NULL,
  `cepCobranca` varchar(10) DEFAULT NULL,
  `ufCobranca` char(2) DEFAULT NULL,
  `indicadoratividade` char(1) NOT NULL DEFAULT '1',
  `cpfcontador` varchar(11) DEFAULT NULL,
  `cnpjcontador` varchar(14) DEFAULT NULL,
  `cepcontador` varchar(8) DEFAULT NULL,
  `enderecocontador` varchar(40) DEFAULT NULL,
  `numerocontador` varchar(10) DEFAULT NULL,
  `complementocontador` varchar(15) DEFAULT NULL,
  `bairrocontador` varchar(20) DEFAULT NULL,
  `telefonecontador` varchar(16) DEFAULT NULL,
  `faxcontador` varchar(16) DEFAULT NULL,
  `emailcontador` varchar(100) DEFAULT NULL,
  `responsavel` varchar(50) DEFAULT NULL,
  `cpfresponsavel` varchar(11) DEFAULT NULL,
  `crt` char(1) DEFAULT '3',
  `versaopaf` varchar(5) NOT NULL DEFAULT '00000',
  PRIMARY KEY (`CodigoFilial`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

/*Table structure for table `financeira` */

CREATE TABLE `financeira` (
  `id` int(3) NOT NULL AUTO_INCREMENT,
  `ncontrato` varchar(10) DEFAULT NULL,
  `datacontrato` date DEFAULT NULL,
  `empresa` varchar(30) DEFAULT NULL,
  `fantasia` varchar(30) DEFAULT NULL,
  `cnpj` varchar(18) DEFAULT NULL,
  `inscricao` varchar(20) DEFAULT NULL,
  `endereco` varchar(40) DEFAULT NULL,
  `cidade` varchar(30) DEFAULT NULL,
  `bairro` varchar(20) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `estado` char(2) DEFAULT NULL,
  `telefone1` varchar(16) DEFAULT NULL,
  `telefone2` varchar(16) DEFAULT NULL,
  `telefone3` varchar(16) DEFAULT NULL,
  `email` varchar(40) DEFAULT NULL,
  `inscricaomunicipal` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `fluxocaixa` */

CREATE TABLE `fluxocaixa` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `conta` varchar(10) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `tipo` varchar(20) DEFAULT NULL,
  `dia1` float(12,2) DEFAULT '0.00',
  `dia2` float(12,2) DEFAULT '0.00',
  `dia3` float(12,2) DEFAULT '0.00',
  `dia4` float(12,2) DEFAULT '0.00',
  `dia5` float(12,2) DEFAULT '0.00',
  `dia6` float(12,2) DEFAULT '0.00',
  `dia7` float(12,2) DEFAULT '0.00',
  `dia8` float(12,2) DEFAULT '0.00',
  `dia9` float(12,2) DEFAULT '0.00',
  `dia10` float(12,2) DEFAULT '0.00',
  `dia11` float(12,2) DEFAULT '0.00',
  `dia12` float(12,2) DEFAULT '0.00',
  `dia13` float(12,2) DEFAULT '0.00',
  `dia14` float(12,2) DEFAULT '0.00',
  `dia15` float(12,2) DEFAULT '0.00',
  `dia16` float(12,2) DEFAULT '0.00',
  `dia17` float(12,2) DEFAULT '0.00',
  `dia18` float(12,2) DEFAULT '0.00',
  `dia19` float(12,2) DEFAULT '0.00',
  `dia20` float(12,2) DEFAULT '0.00',
  `dia21` float(12,2) DEFAULT '0.00',
  `dia22` float(12,2) DEFAULT '0.00',
  `dia23` float(12,2) DEFAULT '0.00',
  `dia24` float(12,2) DEFAULT '0.00',
  `dia25` float(12,2) DEFAULT '0.00',
  `dia26` float(12,2) DEFAULT '0.00',
  `dia27` float(12,2) DEFAULT '0.00',
  `dia28` float(12,2) DEFAULT '0.00',
  `dia29` float(12,2) DEFAULT '0.00',
  `dia30` float(12,2) DEFAULT '0.00',
  `dia31` float(12,2) DEFAULT '0.00',
  `mes` int(6) DEFAULT NULL,
  `ip` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

/*Table structure for table `fornecedores` */

CREATE TABLE `fornecedores` (
  `razaosocial` varchar(50) DEFAULT NULL,
  `ENDERECO` varchar(50) DEFAULT NULL,
  `CEP` varchar(10) DEFAULT NULL,
  `BAIRRO` varchar(20) DEFAULT NULL,
  `CIDADE` varchar(30) DEFAULT NULL,
  `ESTADO` char(2) DEFAULT NULL,
  `CGC` varchar(18) DEFAULT NULL,
  `INSCRICAO` varchar(20) DEFAULT NULL,
  `CPF` varchar(14) DEFAULT NULL,
  `OBSERVACAO` text,
  `TELEFONE` varchar(15) DEFAULT NULL,
  `FAX` varchar(15) DEFAULT NULL,
  `TELEFONE2` varchar(15) DEFAULT NULL,
  `FAX2` varchar(15) DEFAULT NULL,
  `TELEFONE3` varchar(15) DEFAULT NULL,
  `FAX3` varchar(15) DEFAULT NULL,
  `DATACAD` date DEFAULT NULL,
  `ULT_COMPRA` date DEFAULT NULL,
  `PRAZO_ENTG` smallint(6) DEFAULT NULL,
  `PED_MINIMO` float DEFAULT NULL,
  `EMAIL` varchar(50) DEFAULT NULL,
  `CONTATO1` varchar(25) DEFAULT NULL,
  `CONTATO2` varchar(25) DEFAULT NULL,
  `CONTATO3` varchar(25) DEFAULT NULL,
  `empresa` varchar(30) DEFAULT NULL,
  `Codigo` int(11) NOT NULL AUTO_INCREMENT,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `bonificacao` decimal(10,2) NOT NULL DEFAULT '0.00',
  `fabricante` char(1) NOT NULL DEFAULT 'N',
  `fornecedor` char(1) NOT NULL DEFAULT 'S',
  `categoria` varchar(30) NOT NULL DEFAULT '1 - Normal',
  `numero` varchar(8) DEFAULT NULL,
  `retencao` char(1) NOT NULL DEFAULT '0',
  `tipoBairro` varchar(10) DEFAULT NULL,
  `tipoEndereco` varchar(10) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `nomecliente` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Codigo`),
  KEY `Fornecedores` (`empresa`)
) ENGINE=MyISAM AUTO_INCREMENT=337 DEFAULT CHARSET=latin1;

/*Table structure for table `funcionarios` */

CREATE TABLE `funcionarios` (
  `inc` int(4) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `nome` varchar(40) DEFAULT NULL,
  `endereco` varchar(40) DEFAULT NULL,
  `bairro` varchar(25) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `cidade` varchar(25) DEFAULT NULL,
  `telefone` varchar(15) DEFAULT NULL,
  `celular` varchar(15) DEFAULT NULL,
  `cpf` varchar(18) DEFAULT NULL,
  `identidade` varchar(15) DEFAULT NULL,
  `pis` varchar(20) DEFAULT NULL,
  `dataadmissao` date DEFAULT NULL,
  `demissao` date DEFAULT NULL,
  `funcao` varchar(20) DEFAULT NULL,
  `datanascimento` date DEFAULT NULL,
  `salario` decimal(10,2) DEFAULT NULL,
  `observacao` text,
  `dataultferias` date DEFAULT NULL,
  `dataacertocontas` date DEFAULT NULL,
  `sexo` char(1) DEFAULT NULL,
  `email` varchar(60) DEFAULT NULL,
  `qtdsalario` decimal(5,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `gerenciaservicos` */

CREATE TABLE `gerenciaservicos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `os` int(11) NOT NULL DEFAULT '0',
  `cliente` varchar(60) DEFAULT NULL,
  `motor` varchar(30) DEFAULT NULL,
  `motorista` varchar(20) DEFAULT NULL,
  `status` varchar(15) DEFAULT NULL,
  `dataentrega` date DEFAULT NULL,
  `horaentrega` varchar(5) DEFAULT NULL,
  `situacao` int(1) DEFAULT '0',
  `codigofilial` varchar(5) DEFAULT NULL,
  `datafinalizacao` date DEFAULT NULL,
  `horafinalizacao` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `grade` */

CREATE TABLE `grade` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `modelo` varchar(15) DEFAULT NULL,
  `grade` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Table structure for table `grupos` */

CREATE TABLE `grupos` (
  `codigo` int(4) unsigned NOT NULL AUTO_INCREMENT,
  `grupo` char(30) DEFAULT '0',
  `CodigoFilial` char(5) DEFAULT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=MyISAM AUTO_INCREMENT=362 DEFAULT CHARSET=latin1;

/*Table structure for table `ie_st_outrosestados` */

CREATE TABLE `ie_st_outrosestados` (
  `id` int(4) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) NOT NULL DEFAULT '00001',
  `IE_ST` varchar(14) DEFAULT NULL,
  `UF_ST` char(2) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `imobilizados` */

CREATE TABLE `imobilizados` (
  `codigofilial` varchar(5) DEFAULT NULL,
  `inc` int(6) NOT NULL AUTO_INCREMENT,
  `numero` varchar(10) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `grupo` varchar(40) DEFAULT NULL,
  `subgrupo` varchar(40) DEFAULT NULL,
  `quantidade` decimal(8,2) NOT NULL DEFAULT '0.00',
  `valor` decimal(12,2) NOT NULL DEFAULT '0.00',
  `operadorcadastro` varchar(10) DEFAULT NULL,
  `operadortransferencia` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `dataaquisicao` date DEFAULT NULL,
  `obs` text,
  `situacao` varchar(15) DEFAULT NULL,
  `datasituacao` date DEFAULT NULL,
  `empresafornecedora` varchar(30) DEFAULT NULL,
  `notafiscalcompra` varchar(10) DEFAULT NULL,
  `validadegarantia` date DEFAULT NULL,
  `taxadepreciacaoano` decimal(6,2) NOT NULL DEFAULT '0.00',
  `depreciacaoacumulada` decimal(10,2) NOT NULL DEFAULT '0.00',
  `datamanutencao` date DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `imobilizadosgrupo` */

CREATE TABLE `imobilizadosgrupo` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `imobilizadossituacao` */

CREATE TABLE `imobilizadossituacao` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `imobilizadossubgrupo` */

CREATE TABLE `imobilizadossubgrupo` (
  `inc` int(4) NOT NULL AUTO_INCREMENT,
  `grupo` varchar(20) DEFAULT NULL,
  `descricao` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `itensaentregar` */

CREATE TABLE `itensaentregar` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `unidade` char(3) DEFAULT NULL,
  `quantidade` decimal(10,2) DEFAULT '0.00',
  `quantidadeaentregar` decimal(10,2) DEFAULT '0.00',
  `dataentrega` date DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `entregue` char(1) NOT NULL DEFAULT 'N',
  `enderecoentrega` varchar(150) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `custo` decimal(10,2) NOT NULL DEFAULT '0.00',
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Table structure for table `juros` */

CREATE TABLE `juros` (
  `Codigo` int(5) NOT NULL AUTO_INCREMENT,
  `parcelas` int(2) DEFAULT NULL,
  `intervalo` int(3) DEFAULT NULL,
  `descricao` varchar(15) DEFAULT NULL,
  `coeficiente` decimal(8,5) DEFAULT NULL,
  `juros` decimal(5,2) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `numero` varchar(20) DEFAULT NULL,
  `cabecalho` varchar(20) DEFAULT NULL,
  `CodClasse` varchar(4) DEFAULT NULL,
  `DescClasse` varchar(20) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `entradaobrigatoria` char(1) NOT NULL DEFAULT 'N',
  `percentualentrada` decimal(5,2) NOT NULL DEFAULT '0.00',
  `tipopagamento` char(2) NOT NULL DEFAULT '00',
  `entradamaiorigualparcela` char(1) NOT NULL DEFAULT 'N',
  `aceitadescontos` char(1) NOT NULL DEFAULT 'N',
  PRIMARY KEY (`Codigo`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

/*Table structure for table `lmccontrole` */

CREATE TABLE `lmccontrole` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `codigoproduto` varchar(13) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `estoqueabertura` float(12,3) DEFAULT NULL,
  `totalrecebido` float(12,3) DEFAULT NULL,
  `volumedisponivel` float(12,3) DEFAULT NULL,
  `vendasdia` float(12,3) DEFAULT NULL,
  `estoqueescritural` float(12,3) DEFAULT NULL,
  `estoquefechamento` float(12,3) DEFAULT NULL,
  `perdassobras` float(12,3) DEFAULT NULL,
  `fechamentofisico` float(12,3) DEFAULT NULL,
  `valorvendas` float(12,3) DEFAULT NULL,
  `acumuladomes` float(12,3) DEFAULT NULL,
  `dnc` text,
  `orgaosfiscais` text,
  `precobomba` float(12,3) DEFAULT NULL,
  `finalizado` char(1) DEFAULT NULL,
  `folha` int(5) DEFAULT NULL,
  `livro` int(5) DEFAULT NULL,
  `restricoes` text,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `lmcrecebido` */

CREATE TABLE `lmcrecebido` (
  `idlinha` int(11) NOT NULL AUTO_INCREMENT,
  `lmcid` int(11) DEFAULT NULL,
  `lmccodigoproduto` varchar(13) DEFAULT NULL,
  `notafiscal` varchar(15) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `tanque` int(11) DEFAULT NULL,
  `volumerecebido` float(12,3) DEFAULT NULL,
  PRIMARY KEY (`idlinha`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `lmctanque` */

CREATE TABLE `lmctanque` (
  `idlinha` int(11) NOT NULL AUTO_INCREMENT,
  `lmcid` int(11) DEFAULT NULL,
  `lmccodigoproduto` char(13) DEFAULT NULL,
  `abertura` float(12,3) DEFAULT NULL,
  `fechamentofisico` float(12,3) DEFAULT NULL,
  `tanque` int(11) DEFAULT NULL,
  PRIMARY KEY (`idlinha`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `lmcvendas` */

CREATE TABLE `lmcvendas` (
  `idlinha` int(11) NOT NULL AUTO_INCREMENT,
  `lmcid` int(11) DEFAULT NULL,
  `lmccodigoproduto` varchar(13) DEFAULT NULL,
  `tanque` int(11) DEFAULT NULL,
  `bico` int(5) DEFAULT NULL,
  `fechamento` float(12,3) DEFAULT NULL,
  `abertura` float(12,3) DEFAULT NULL,
  `afericoes` float(12,3) DEFAULT NULL,
  `vendasbico` float(12,3) DEFAULT NULL,
  PRIMARY KEY (`idlinha`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `maladireta` */

CREATE TABLE `maladireta` (
  `inc` int(3) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(40) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `texto` text,
  `codigofilial` varchar(5) DEFAULT NULL,
  `titulo` varchar(100) DEFAULT NULL,
  `rodape` text,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `marcadorcrmensal` */

CREATE TABLE `marcadorcrmensal` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `classe` varchar(5) DEFAULT NULL,
  `ano` int(4) DEFAULT NULL,
  `vendedor` varchar(5) DEFAULT NULL,
  `filial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `cobrador` varchar(5) DEFAULT NULL,
  `janeiro` decimal(14,2) NOT NULL DEFAULT '0.00',
  `fevereiro` decimal(14,2) NOT NULL DEFAULT '0.00',
  `marco` decimal(14,2) NOT NULL DEFAULT '0.00',
  `abril` decimal(14,2) NOT NULL DEFAULT '0.00',
  `maio` decimal(14,2) NOT NULL DEFAULT '0.00',
  `junho` decimal(14,2) NOT NULL DEFAULT '0.00',
  `julho` decimal(14,2) NOT NULL DEFAULT '0.00',
  `agosto` decimal(14,2) NOT NULL DEFAULT '0.00',
  `setembro` decimal(14,2) NOT NULL DEFAULT '0.00',
  `outubro` decimal(14,2) NOT NULL DEFAULT '0.00',
  `novembro` decimal(14,2) NOT NULL DEFAULT '0.00',
  `dezembro` decimal(14,2) NOT NULL DEFAULT '0.00',
  `Total` decimal(14,2) NOT NULL DEFAULT '0.00',
  `ultimodocumento` int(8) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=41 DEFAULT CHARSET=latin1;

/*Table structure for table `modelos` */

CREATE TABLE `modelos` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `modelo` char(30) DEFAULT NULL,
  `valorcomissao` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `modistru` */

CREATE TABLE `modistru` (
  `data` datetime DEFAULT NULL,
  `versao` varchar(10) DEFAULT '0',
  `commando` text,
  `inc` int(11) DEFAULT '0',
  KEY `data` (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `modistru_antigo` */

CREATE TABLE `modistru_antigo` (
  `data` datetime DEFAULT NULL,
  `versao` varchar(10) DEFAULT '0',
  `commando` text,
  `inc` int(11) DEFAULT '0',
  KEY `data` (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `movcartoes` */

CREATE TABLE `movcartoes` (
  `id` bigint(15) NOT NULL AUTO_INCREMENT,
  `cartao` varchar(15) DEFAULT NULL,
  `numero` varchar(20) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `taxadesconto` decimal(6,2) DEFAULT NULL,
  `valor` decimal(12,2) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `Historico` varchar(15) DEFAULT NULL,
  `dpfinanceiro` varchar(15) NOT NULL DEFAULT '',
  `depositado` char(1) NOT NULL DEFAULT 'N',
  `marcado` char(1) DEFAULT NULL,
  `encargos` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `interpolador` varchar(8) DEFAULT NULL,
  `autorizacao` varchar(40) DEFAULT NULL,
  `datadeposito` date DEFAULT NULL,
  `tipo` char(2) NOT NULL DEFAULT '',
  `ip` varchar(15) DEFAULT NULL,
  `vendedor` char(3) DEFAULT NULL,
  `comissaopaga` char(1) NOT NULL DEFAULT 'N',
  PRIMARY KEY (`id`),
  KEY `documento` (`documento`)
) ENGINE=MyISAM AUTO_INCREMENT=31 DEFAULT CHARSET=latin1;

/*Table structure for table `movcontasbanco` */

CREATE TABLE `movcontasbanco` (
  `id` int(5) NOT NULL AUTO_INCREMENT,
  `id_conta` int(3) DEFAULT NULL,
  `conta` varchar(20) DEFAULT NULL,
  `movimento` varchar(10) DEFAULT NULL,
  `valordebito` decimal(10,2) DEFAULT NULL,
  `valorcredito` decimal(10,2) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `historico` varchar(100) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(30) DEFAULT NULL,
  `grupo` char(2) DEFAULT NULL,
  `interpolador` varchar(8) DEFAULT NULL,
  `cheque` bigint(15) NOT NULL DEFAULT '0',
  `cpmfcalculado` char(1) NOT NULL DEFAULT 'N',
  `cpmf` decimal(8,4) NOT NULL DEFAULT '0.0000',
  PRIMARY KEY (`id`),
  KEY `cheque` (`cheque`)
) ENGINE=MyISAM AUTO_INCREMENT=1424 DEFAULT CHARSET=latin1;

/*Table structure for table `movdespesas` */

CREATE TABLE `movdespesas` (
  `id_inc` int(6) NOT NULL AUTO_INCREMENT,
  `id` varchar(15) NOT NULL DEFAULT '',
  `conta` varchar(5) NOT NULL DEFAULT '0',
  `descricaoconta` varchar(40) DEFAULT NULL,
  `subconta` varchar(5) NOT NULL DEFAULT '0',
  `descricaosubconta` varchar(40) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `despesa` char(1) NOT NULL DEFAULT 'S',
  `historico` varchar(70) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `grupo` varchar(15) NOT NULL DEFAULT '',
  `sangria` char(1) NOT NULL DEFAULT 'S',
  `encerrado` char(1) NOT NULL DEFAULT 'N',
  `OpCxAdm` varchar(10) NOT NULL DEFAULT '',
  `contabancaria` varchar(20) DEFAULT NULL,
  `interpolador` varchar(8) DEFAULT NULL,
  `vendedorcomissao` char(3) NOT NULL DEFAULT '',
  `cobradorcomissao` char(3) NOT NULL DEFAULT '',
  `devolucaonumero` int(7) NOT NULL DEFAULT '0',
  `tipodespesa` char(1) NOT NULL DEFAULT 'F',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `valorbonificacaocliente` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ncupomfiscalCOO` varchar(6) DEFAULT NULL,
  `contadornaofiscalGNF` varchar(6) DEFAULT NULL,
  `ecfcontadorcupomfiscal` varchar(6) DEFAULT NULL,
  `ecfnumero` varchar(3) DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `datafiscal` date NOT NULL,
  `cheque` bigint(15) DEFAULT NULL,
  `EADDados` varchar(33) DEFAULT NULL,
  `tipopgamento` varchar(2) NOT NULL DEFAULT 'DH',
  PRIMARY KEY (`id_inc`),
  KEY `id` (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;

/*Table structure for table `moventradas` */

CREATE TABLE `moventradas` (
  `numero` int(11) NOT NULL AUTO_INCREMENT,
  `fornecedor` varchar(50) DEFAULT NULL,
  `NF` varchar(15) NOT NULL DEFAULT '',
  `Documento` varchar(15) DEFAULT NULL,
  `DataEmissao` date DEFAULT NULL,
  `dataEntrada` date DEFAULT NULL,
  `Data` date DEFAULT NULL,
  `ValorProdutos` decimal(12,2) NOT NULL DEFAULT '0.00',
  `ValorNota` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Icms` decimal(10,2) NOT NULL DEFAULT '0.00',
  `BaseIcms` decimal(12,2) NOT NULL DEFAULT '0.00',
  `IPI` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Frete` decimal(10,2) NOT NULL DEFAULT '0.00',
  `IcmsSubst` decimal(10,2) NOT NULL DEFAULT '0.00',
  `BaseIcmsSubst` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Despesas` decimal(10,2) NOT NULL DEFAULT '0.00',
  `usuario` varchar(10) NOT NULL DEFAULT '',
  `Codigofilial` varchar(5) NOT NULL DEFAULT '',
  `lancada` char(1) NOT NULL DEFAULT '',
  `operacao` varchar(5) NOT NULL DEFAULT '',
  `tipofrete` char(1) DEFAULT '1',
  `serie` char(3) DEFAULT NULL,
  `subserie` int(3) NOT NULL DEFAULT '1',
  `descontos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ip` varchar(15) DEFAULT NULL,
  `importada` char(1) NOT NULL DEFAULT 'N',
  `valorseguro` decimal(10,2) NOT NULL DEFAULT '0.00',
  `codigofornecedor` int(11) DEFAULT NULL,
  `especie` varchar(5) DEFAULT NULL,
  `UFemitente` char(2) DEFAULT NULL,
  `horaemissao` time DEFAULT NULL,
  `horaentrada` time DEFAULT NULL,
  `basecalculoipi` decimal(10,2) NOT NULL DEFAULT '0.00',
  `modeloNF` char(2) NOT NULL DEFAULT '01',
  `situacaoNF` char(1) NOT NULL DEFAULT 'N',
  `aliquotaICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `Emitente` char(1) NOT NULL DEFAULT 'T',
  `cfopentrada` varchar(5) DEFAULT NULL,
  `exportarfiscal` char(1) NOT NULL DEFAULT 'S',
  `sefvalidado` char(1) NOT NULL DEFAULT 'N',
  `chave_nfe` varchar(44) DEFAULT NULL,
  `indicadorpagamento` char(1) NOT NULL DEFAULT '0',
  `pis` decimal(8,2) NOT NULL DEFAULT '0.00',
  `pis_st` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cofins` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cofins_st` decimal(8,2) NOT NULL DEFAULT '0.00',
  `pedidocompra` int(7) NOT NULL DEFAULT '0',
  UNIQUE KEY `numero` (`numero`),
  KEY `numero_2` (`numero`)
) ENGINE=MyISAM AUTO_INCREMENT=329 DEFAULT CHARSET=latin1;

/*Table structure for table `movfuncionarios` */

CREATE TABLE `movfuncionarios` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `interpolador` varchar(8) DEFAULT NULL,
  `codigo` int(5) DEFAULT NULL,
  `nome` varchar(40) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `credito` decimal(10,2) NOT NULL DEFAULT '0.00',
  `debito` decimal(10,2) NOT NULL DEFAULT '0.00',
  `historico` varchar(150) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `marcado` char(1) NOT NULL DEFAULT 'N',
  `quitado` char(1) NOT NULL DEFAULT 'N',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `movimento` */

CREATE TABLE `movimento` (
  `inc` bigint(5) NOT NULL AUTO_INCREMENT,
  `data` date DEFAULT NULL,
  `finalizado` char(1) NOT NULL DEFAULT '',
  `SaldoCaixa` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Credito` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Debito` decimal(12,2) NOT NULL DEFAULT '0.00',
  `Saldocrediario` decimal(12,2) NOT NULL DEFAULT '0.00',
  `creditocr` decimal(12,2) NOT NULL DEFAULT '0.00',
  `debitocr` decimal(12,2) NOT NULL DEFAULT '0.00',
  `saldocartao` decimal(12,2) NOT NULL DEFAULT '0.00',
  `creditoca` decimal(12,2) NOT NULL DEFAULT '0.00',
  `debitoca` decimal(12,2) NOT NULL DEFAULT '0.00',
  `saldocheques` decimal(12,2) NOT NULL DEFAULT '0.00',
  `creditoch` decimal(12,2) NOT NULL DEFAULT '0.00',
  `debitoch` decimal(12,2) NOT NULL DEFAULT '0.00',
  `id` char(15) DEFAULT NULL,
  `codigofilial` char(5) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=707 DEFAULT CHARSET=latin1;

/*Table structure for table `movreceitas` */

CREATE TABLE `movreceitas` (
  `id_inc` int(6) NOT NULL AUTO_INCREMENT,
  `id` varchar(15) NOT NULL DEFAULT '',
  `conta` varchar(5) NOT NULL DEFAULT '0',
  `descricaoconta` varchar(30) DEFAULT NULL,
  `subconta` varchar(5) NOT NULL DEFAULT '0',
  `descricaosubconta` varchar(30) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `receita` char(1) NOT NULL DEFAULT 'S',
  `historico` varchar(70) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `grupo` varchar(15) NOT NULL DEFAULT '',
  `sangria` char(1) NOT NULL DEFAULT 'S',
  `encerrado` char(1) NOT NULL DEFAULT 'N',
  `OpCxAdm` varchar(10) NOT NULL DEFAULT '',
  `contabancaria` varchar(20) DEFAULT NULL,
  `cheque` varchar(10) DEFAULT NULL,
  `interpolador` varchar(8) DEFAULT NULL,
  `datafiscal` date NOT NULL DEFAULT '0000-00-00',
  PRIMARY KEY (`id_inc`),
  KEY `id` (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `movtransf` */

CREATE TABLE `movtransf` (
  `codigofilial` varchar(5) DEFAULT NULL,
  `Filialdestino` varchar(5) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `usuario` varchar(10) DEFAULT NULL,
  `numero` int(5) unsigned NOT NULL DEFAULT '0',
  `lancada` char(1) NOT NULL DEFAULT '',
  `filialorigem` varchar(5) NOT NULL DEFAULT '',
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `customedioanteriordestino` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `Quantidadeatualizadadestino` decimal(10,2) NOT NULL DEFAULT '0.00',
  `quantidadeanteriordestino` decimal(10,2) NOT NULL DEFAULT '0.00',
  `customediorefeitodestino` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `deposito` decimal(10,2) NOT NULL DEFAULT '0.00',
  `prateleiras` decimal(10,2) NOT NULL DEFAULT '0.00',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `notafiscal` int(6) NOT NULL DEFAULT '0',
  KEY `numero` (`numero`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `naturezanf` */

CREATE TABLE `naturezanf` (
  `inc` int(4) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `naturezaoperacao` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `nfe012` */

CREATE TABLE `nfe012` (
  `CbdEmpCodigo` decimal(3,0) NOT NULL DEFAULT '1',
  `CbdNtfNumero` decimal(9,0) NOT NULL DEFAULT '0',
  `CbdNtfSerie` char(3) NOT NULL DEFAULT '',
  `CbdAcao` char(1) DEFAULT NULL,
  `CbdSituacao` decimal(1,0) DEFAULT '0',
  `CbdDtaProcessamento` date DEFAULT NULL,
  `CbdNumProtocolo` decimal(15,0) DEFAULT NULL,
  `CbdStsRetCodigo` decimal(3,0) DEFAULT NULL,
  `CbdStsRetNome` varchar(100) DEFAULT NULL,
  `CbdProcStatus` char(1) DEFAULT 'A',
  `CbdNFEChaAcesso` varchar(44) DEFAULT NULL,
  `CbdxMotivo` varchar(100) DEFAULT NULL,
  `CbdXML` blob,
  `CbdNumProtCanc` decimal(15,0) DEFAULT NULL,
  `CbdDtaCancelamento` date DEFAULT NULL,
  `CbdDtaInutilizacao` date DEFAULT NULL,
  `CbdNumProtInut` decimal(15,0) DEFAULT NULL,
  `CbdMarca` char(1) DEFAULT 'N',
  `CbdDigVal` varchar(100) DEFAULT NULL,
  `cbdcodigofilial` varchar(5) NOT NULL DEFAULT '00001',
  PRIMARY KEY (`CbdEmpCodigo`,`CbdNtfNumero`,`CbdNtfSerie`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `nfoperacao` */

CREATE TABLE `nfoperacao` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigo` varchar(10) NOT NULL DEFAULT '',
  `descricao` varchar(50) DEFAULT NULL,
  `tipo` char(1) NOT NULL DEFAULT 'E',
  `geraicms` char(1) NOT NULL DEFAULT 'S',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

/*Table structure for table `orcamentodetail` */

CREATE TABLE `orcamentodetail` (
  `numero` int(11) DEFAULT NULL,
  `quantidade` double(7,3) DEFAULT NULL,
  `codigo` varchar(14) DEFAULT NULL,
  `servico` varchar(50) DEFAULT NULL,
  `tipo` char(1) DEFAULT NULL,
  `valorunitario` decimal(11,2) DEFAULT NULL,
  `subtotal` decimal(11,2) DEFAULT NULL,
  `situacao` int(1) DEFAULT NULL,
  `icms` decimal(6,2) DEFAULT '0.00',
  `tributacao` char(2) DEFAULT NULL,
  `valoricms` decimal(11,2) DEFAULT '0.00',
  `codigofilial` varchar(5) DEFAULT NULL,
  KEY `numero` (`numero`,`servico`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `orcamentomaster` */

CREATE TABLE `orcamentomaster` (
  `numero` int(11) NOT NULL DEFAULT '0',
  `tipo` int(1) DEFAULT '0',
  `responsavelnome` varchar(15) DEFAULT NULL,
  `codigocliente` varchar(7) DEFAULT NULL,
  `nomecliente` varchar(50) DEFAULT NULL,
  `dataabertura` date DEFAULT NULL,
  `item` varchar(50) DEFAULT NULL,
  `item2` varchar(50) DEFAULT NULL,
  `item3` varchar(50) DEFAULT NULL,
  `situacao` int(1) DEFAULT '0',
  `vencimento` date DEFAULT NULL,
  `totalpecas` decimal(12,2) DEFAULT '0.00',
  `totalservicos` decimal(12,2) DEFAULT '0.00',
  `descppecas` decimal(12,2) DEFAULT '0.00',
  `descpservico` decimal(12,2) DEFAULT '0.00',
  `descpecas` decimal(12,2) DEFAULT '0.00',
  `descservico` decimal(12,2) DEFAULT '0.00',
  `total` decimal(12,2) DEFAULT '0.00',
  `totalreal` decimal(12,2) DEFAULT '0.00',
  `fpagamento` char(2) DEFAULT NULL,
  `entrada` decimal(12,2) DEFAULT '0.00',
  `pentrada` int(4) DEFAULT '0',
  `parcelas` int(3) DEFAULT '0',
  `valorparcelas` decimal(12,2) DEFAULT '0.00',
  `servico` int(11) DEFAULT '0',
  `desconto` int(1) DEFAULT '0',
  `codigofilial` varchar(5) DEFAULT NULL,
  `vendedor` char(3) DEFAULT NULL,
  `usuarioip` varchar(20) DEFAULT NULL,
  `ip` varchar(20) DEFAULT NULL,
  `placa` varchar(7) DEFAULT NULL,
  PRIMARY KEY (`numero`),
  KEY `vendedor` (`vendedor`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `orcamentopendencia` */

CREATE TABLE `orcamentopendencia` (
  `numero` int(11) DEFAULT NULL,
  `quantidade` double(7,3) DEFAULT NULL,
  `codigo` varchar(14) DEFAULT NULL,
  `servico` varchar(50) DEFAULT NULL,
  `tipo` char(1) DEFAULT NULL,
  `valorunitario` decimal(11,2) DEFAULT NULL,
  `subtotal` decimal(11,2) DEFAULT NULL,
  `situacao` int(1) DEFAULT NULL,
  `icms` decimal(6,2) DEFAULT '0.00',
  `tributacao` char(2) DEFAULT NULL,
  `valoricms` decimal(11,2) DEFAULT '0.00',
  `codigofilial` varchar(5) DEFAULT NULL,
  KEY `numero` (`numero`,`servico`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `pedidocompra` */

CREATE TABLE `pedidocompra` (
  `numero` int(8) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(8,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) DEFAULT '0.00000',
  `fornecedor` varchar(30) DEFAULT NULL,
  `codigofornecedor` varchar(20) DEFAULT NULL,
  `ultcusto` decimal(10,2) NOT NULL DEFAULT '0.00',
  `diferencaqtdrecebida` decimal(8,2) NOT NULL DEFAULT '0.00',
  `qtdrecebida` decimal(8,2) NOT NULL DEFAULT '0.00',
  `numeroitem` int(5) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `pendencias` */

CREATE TABLE `pendencias` (
  `Id_inc` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `descricao` text,
  `data` date DEFAULT NULL,
  `dataresolucao` date DEFAULT NULL,
  `setor` varchar(60) DEFAULT NULL,
  `resolvido` char(1) NOT NULL DEFAULT 'N',
  `marcado` char(1) NOT NULL DEFAULT '',
  `valor` decimal(12,2) NOT NULL DEFAULT '0.00',
  `operador` varchar(10) NOT NULL DEFAULT '',
  `interpolador` varchar(8) DEFAULT NULL,
  PRIMARY KEY (`Id_inc`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Table structure for table `premios` */

CREATE TABLE `premios` (
  `id` int(5) NOT NULL AUTO_INCREMENT,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `quantidade` decimal(8,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,2) NOT NULL DEFAULT '0.00',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `pontos` decimal(8,2) NOT NULL DEFAULT '0.00',
  `baixarestoque` char(1) NOT NULL DEFAULT 'S',
  `produtovinculado` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `processo` */

CREATE TABLE `processo` (
  `ip` char(20) DEFAULT NULL,
  `processo` char(30) DEFAULT NULL,
  `documento` int(8) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `operador` char(10) DEFAULT NULL,
  `cliente` char(50) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `hora` time DEFAULT NULL,
  `inc` bigint(5) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=86 DEFAULT CHARSET=latin1;

/*Table structure for table `producao` */

CREATE TABLE `producao` (
  `inc` int(10) NOT NULL AUTO_INCREMENT,
  `numero` int(8) NOT NULL DEFAULT '0',
  `codigofilial` varchar(5) DEFAULT NULL,
  `finalizado` char(1) NOT NULL DEFAULT 'N',
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `quantidadeproduzida` decimal(10,2) NOT NULL DEFAULT '0.00',
  `quantidadeentrada` decimal(10,2) NOT NULL DEFAULT '0.00',
  `restante` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custoproducao` decimal(10,2) NOT NULL DEFAULT '0.00',
  `customaodeobra` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custototal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `tempodeproducao` time NOT NULL DEFAULT '00:00:00',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `produtos` */

CREATE TABLE `produtos` (
  `codigo` varchar(20) DEFAULT '0',
  `codigoinc` int(6) NOT NULL AUTO_INCREMENT,
  `descecf` varchar(25) DEFAULT NULL,
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtddeposito` decimal(10,0) NOT NULL DEFAULT '0',
  `qtdprateleiras` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdvencidos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdanterior` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdultent` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dataultent` date DEFAULT NULL,
  `qtdprovisoria` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descprovisorio` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pedidoand` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdultpedido` decimal(10,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `dataultpedido` date DEFAULT NULL,
  `icms` decimal(6,2) NOT NULL DEFAULT '0.00',
  `ipi` decimal(6,2) NOT NULL DEFAULT '0.00',
  `grupo` varchar(30) NOT NULL DEFAULT '',
  `subgrupo` varchar(30) NOT NULL DEFAULT '',
  `custo` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `customedio` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `ultcusto` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `custototal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `margemlucro` decimal(8,2) NOT NULL DEFAULT '0.00',
  `precovenda` decimal(12,2) NOT NULL DEFAULT '0.00',
  `dataultvenda` date DEFAULT NULL,
  `dataaltpreco` date DEFAULT NULL,
  `ultpreco` decimal(12,2) NOT NULL DEFAULT '0.00',
  `estminimo` decimal(10,2) NOT NULL DEFAULT '0.00',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `situacao` varchar(15) DEFAULT NULL,
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `fornecedor` varchar(30) NOT NULL DEFAULT '',
  `fabricante` varchar(30) NOT NULL DEFAULT '',
  `tipocomissao` char(1) NOT NULL DEFAULT 'A',
  `datacadastro` date DEFAULT NULL,
  `validade` date DEFAULT NULL,
  `aceitadesconto` char(1) DEFAULT NULL,
  `descontopromocao` decimal(6,2) NOT NULL DEFAULT '0.00',
  `validadepromoc` date DEFAULT NULL,
  `descontomaximo` decimal(6,2) NOT NULL DEFAULT '0.00',
  `operador` varchar(15) DEFAULT NULL,
  `pesobruto` decimal(10,3) NOT NULL DEFAULT '0.000',
  `pesoliquido` decimal(10,3) NOT NULL DEFAULT '0.000',
  `marcado` char(1) DEFAULT NULL,
  `embalagem` int(5) NOT NULL DEFAULT '1',
  `unidembalagem` char(3) DEFAULT NULL,
  `localestoque` varchar(15) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `frete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `01qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `01custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `01vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `02qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `02custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `02vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `03qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `03custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `03vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `04qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `04custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `04vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `05qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `05custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `05vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `06qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `06custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `06vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `07qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `07custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `07vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `08qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `08custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `08vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `09qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `09custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `09vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `10qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `10custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `10vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `11qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `11custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `11vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `12qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `12custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `12vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `CodigoFilial` varchar(5) NOT NULL DEFAULT '00001',
  `codigovinculado` varchar(20) DEFAULT NULL,
  `inventario` decimal(10,2) NOT NULL DEFAULT '0.00',
  `documento` int(10) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '0000',
  `qtdretida` decimal(8,2) NOT NULL DEFAULT '0.00',
  `secao` varchar(20) DEFAULT NULL,
  `diasparavencimento` int(3) NOT NULL DEFAULT '0',
  `lote` varchar(15) NOT NULL DEFAULT '',
  `vencimento` date DEFAULT NULL,
  `anoinventario` int(4) NOT NULL DEFAULT '0',
  `inventarioencerrado` char(1) NOT NULL DEFAULT 'N',
  `dataencerramentoinventario` date DEFAULT NULL,
  `qtdaentregar` decimal(10,2) NOT NULL DEFAULT '0.00',
  `precounidade` decimal(12,2) NOT NULL DEFAULT '0.00',
  `generico` char(3) DEFAULT NULL,
  `princativo` varchar(130) DEFAULT NULL,
  `margemsemfinanciamento` decimal(8,2) NOT NULL DEFAULT '0.00',
  `precosemfinanciamento` decimal(8,2) NOT NULL DEFAULT '0.00',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `ativacompdesc` char(1) NOT NULL DEFAULT 'N',
  `inventarionumero` int(4) NOT NULL DEFAULT '1',
  `custofornecedor` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `qtdminimadesc` decimal(8,2) NOT NULL DEFAULT '0.00',
  `qtdprevenda` decimal(10,2) NOT NULL DEFAULT '0.00',
  `parcelamentomax` int(3) NOT NULL DEFAULT '0',
  `precoatacado` decimal(10,2) NOT NULL DEFAULT '0.00',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `detalhetecnico` text,
  `origem` varchar(50) NOT NULL DEFAULT '0 - Nacional',
  `modalidadeDetBaseCalcICMS` varchar(100) DEFAULT NULL,
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `modalidadedetBaseCalcICMsST` varchar(100) DEFAULT NULL,
  `ICMsST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedICMsST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualMargVlrAdICMsST` decimal(5,3) NOT NULL DEFAULT '0.000',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `PIS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `COFINS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `margemlucroliquida` decimal(8,2) NOT NULL DEFAULT '0.00',
  `volumes` int(3) NOT NULL DEFAULT '1',
  `ncm` varchar(8) DEFAULT NULL,
  `nbm` varchar(10) DEFAULT NULL,
  `ncmespecie` char(2) DEFAULT NULL,
  `capacidadevolML` int(5) DEFAULT NULL,
  `situacaoinventario` char(2) NOT NULL DEFAULT '00',
  `tributacaoPIS` char(2) NOT NULL DEFAULT '01',
  `tributacaoCOFINS` char(2) NOT NULL DEFAULT '01',
  `codigoservico` varchar(4) NOT NULL DEFAULT '0000',
  `aliquotaISS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `indicadorproducao` char(1) NOT NULL DEFAULT 'T',
  `indicadorarredondamentotruncamento` char(1) NOT NULL DEFAULT 'A',
  `cfopsaida` varchar(5) NOT NULL DEFAULT '5.102',
  `cfopentrada` varchar(5) NOT NULL DEFAULT '1.102',
  `EADE2mercadoriaEstoque` varchar(33) DEFAULT NULL,
  `EADP2relacaomercadoria` varchar(33) DEFAULT NULL,
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `datafabricacao` date DEFAULT NULL,
  `saldofinalestoque` decimal(10,2) NOT NULL DEFAULT '0.00',
  `complementodescricao` varchar(100) NOT NULL DEFAULT ' ',
  `datafinalestoque` date DEFAULT NULL,
  `horafinalestoque` time DEFAULT NULL,
  `ecffabricacao` varchar(20) NOT NULL DEFAULT ' ',
  PRIMARY KEY (`codigoinc`),
  KEY `PrdCodigo` (`codigo`),
  KEY `descricao` (`descricao`),
  KEY `codigobarras` (`codigobarras`)
) ENGINE=MyISAM AUTO_INCREMENT=2147483648 DEFAULT CHARSET=latin1;

/*Table structure for table `produtos_iqs` */

CREATE TABLE `produtos_iqs` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `ip` varchar(15) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `detalhestecnico` text,
  `validade` date DEFAULT NULL,
  `imagem` longblob,
  `imagem1` longblob,
  `imagem2` longblob,
  `imagem3` longblob,
  `imagem4` longblob,
  `imagemthumb` mediumblob,
  `preco` decimal(10,2) DEFAULT NULL,
  `detalhespagamento` text,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `produtoscomposicao` */

CREATE TABLE `produtoscomposicao` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `codigomateria` varchar(20) DEFAULT NULL,
  `descricaomateria` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `unidade` char(3) NOT NULL DEFAULT 'UN',
  `custo` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custototal` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Table structure for table `produtosfilial` */

CREATE TABLE `produtosfilial` (
  `codigo` varchar(20) DEFAULT '0',
  `codigoinc` int(6) NOT NULL DEFAULT '0',
  `descecf` varchar(25) DEFAULT NULL,
  `unidade` char(3) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtddeposito` decimal(10,0) NOT NULL DEFAULT '0',
  `qtdprateleiras` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdvencidos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdanterior` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdultent` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dataultent` date DEFAULT NULL,
  `qtdprovisoria` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descprovisorio` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pedidoand` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdultpedido` decimal(10,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `dataultpedido` date DEFAULT NULL,
  `icms` decimal(6,2) NOT NULL DEFAULT '0.00',
  `ipi` decimal(6,2) NOT NULL DEFAULT '0.00',
  `grupo` varchar(30) NOT NULL DEFAULT '',
  `subgrupo` varchar(30) NOT NULL DEFAULT '',
  `custo` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `customedio` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `ultcusto` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `custototal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `margemlucro` decimal(8,2) NOT NULL DEFAULT '0.00',
  `precovenda` decimal(12,2) NOT NULL DEFAULT '0.00',
  `dataultvenda` date DEFAULT NULL,
  `dataaltpreco` date DEFAULT NULL,
  `ultpreco` decimal(12,2) NOT NULL DEFAULT '0.00',
  `estminimo` decimal(10,2) NOT NULL DEFAULT '0.00',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `situacao` varchar(15) DEFAULT NULL,
  `tributacao` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) NOT NULL DEFAULT '',
  `fabricante` varchar(30) NOT NULL DEFAULT '',
  `tipocomissao` char(1) DEFAULT NULL,
  `datacadastro` date DEFAULT NULL,
  `validade` date DEFAULT NULL,
  `aceitadesconto` char(1) DEFAULT NULL,
  `descontopromocao` decimal(6,2) NOT NULL DEFAULT '0.00',
  `validadepromoc` date DEFAULT NULL,
  `descontomaximo` decimal(6,2) NOT NULL DEFAULT '0.00',
  `operador` varchar(15) DEFAULT NULL,
  `pesobruto` decimal(10,3) NOT NULL DEFAULT '0.000',
  `pesoliquido` decimal(10,3) NOT NULL DEFAULT '0.000',
  `marcado` char(1) DEFAULT NULL,
  `embalagem` int(5) DEFAULT NULL,
  `unidembalagem` char(3) DEFAULT NULL,
  `localestoque` varchar(15) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `frete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `01qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `01custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `01vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `02qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `02custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `02vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `03qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `03custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `03vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `04qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `04custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `04vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `05qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `05custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `05vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `06qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `06custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `06vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `07qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `07custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `07vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `08qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `08custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `08vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `09qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `09custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `09vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `10qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `10custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `10vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `11qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `11custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `11vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `12qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `12custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `12vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `CodigoFilial` varchar(5) NOT NULL DEFAULT '00001',
  `codigovinculado` varchar(20) DEFAULT NULL,
  `inventario` decimal(10,2) NOT NULL DEFAULT '0.00',
  `documento` int(10) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '0000',
  `qtdretida` decimal(8,2) NOT NULL DEFAULT '0.00',
  `secao` varchar(20) DEFAULT NULL,
  `diasparavencimento` int(3) NOT NULL DEFAULT '0',
  `lote` varchar(15) NOT NULL DEFAULT '',
  `vencimento` date DEFAULT NULL,
  `anoinventario` int(4) NOT NULL DEFAULT '0',
  `inventarioencerrado` char(1) NOT NULL DEFAULT 'N',
  `dataencerramentoinventario` date DEFAULT NULL,
  `qtdaentregar` decimal(10,2) NOT NULL DEFAULT '0.00',
  `precounidade` decimal(12,2) NOT NULL DEFAULT '0.00',
  `generico` char(3) DEFAULT NULL,
  `princativo` varchar(130) DEFAULT NULL,
  `margemsemfinanciamento` decimal(8,2) NOT NULL DEFAULT '0.00',
  `precosemfinanciamento` decimal(8,2) NOT NULL DEFAULT '0.00',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `ativacompdesc` char(1) NOT NULL DEFAULT 'N',
  `inventarionumero` int(4) NOT NULL DEFAULT '1',
  `custofornecedor` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `qtdminimadesc` decimal(8,2) NOT NULL DEFAULT '0.00',
  `qtdprevenda` decimal(10,2) NOT NULL DEFAULT '0.00',
  `parcelamentomax` int(3) NOT NULL DEFAULT '0',
  `precoatacado` decimal(10,2) NOT NULL DEFAULT '0.00',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `detalhetecnico` text,
  `origem` varchar(50) NOT NULL DEFAULT '0 - Nacional',
  `modalidadeDetBaseCalcICMS` varchar(100) DEFAULT NULL,
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `modalidadedetBaseCalcICMsST` varchar(100) DEFAULT NULL,
  `ICMsST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedICMsST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualMargVlrAdICMsST` decimal(5,3) NOT NULL DEFAULT '0.000',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `PIS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `COFINS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `margemlucroliquida` decimal(8,2) NOT NULL DEFAULT '0.00',
  `volumes` int(3) NOT NULL DEFAULT '1',
  `ncm` varchar(8) DEFAULT NULL,
  `nbm` varchar(10) DEFAULT NULL,
  `ncmespecie` char(2) DEFAULT NULL,
  `capacidadevolML` int(5) DEFAULT NULL,
  `situacaoinventario` char(2) NOT NULL DEFAULT '00',
  `tributacaoPIS` char(2) NOT NULL DEFAULT '01',
  `tributacaoCOFINS` char(2) NOT NULL DEFAULT '01',
  `codigoservico` varchar(4) NOT NULL DEFAULT '0000',
  `aliquotaISS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `indicadorproducao` char(1) NOT NULL DEFAULT 'T',
  `indicadorarredondamentotruncamento` char(1) NOT NULL DEFAULT 'A',
  `cfopsaida` varchar(5) NOT NULL DEFAULT '5.102',
  `cfopentrada` varchar(5) NOT NULL DEFAULT '1.102',
  `EADE2mercadoriaEstoque` varchar(33) DEFAULT NULL,
  `EADP2relacaomercadoria` varchar(33) DEFAULT NULL,
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `datafabricacao` date DEFAULT NULL,
  `saldofinalestoque` decimal(10,2) NOT NULL DEFAULT '0.00',
  `complementodescricao` varchar(100) NOT NULL DEFAULT ' ',
  `datafinalestoque` date DEFAULT NULL,
  `horafinalestoque` time DEFAULT NULL,
  `ecffabricacao` varchar(20) NOT NULL DEFAULT ' ',
  UNIQUE KEY `codigo` (`codigo`,`CodigoFilial`),
  KEY `descricao` (`descricao`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `produtosgrade` */

CREATE TABLE `produtosgrade` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) NOT NULL DEFAULT '00000',
  `descricaograde` varchar(10) DEFAULT NULL,
  `grade` varchar(15) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `codigograde` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=25 DEFAULT CHARSET=latin1;

/*Table structure for table `produtosimagens` */

CREATE TABLE `produtosimagens` (
  `inc` int(11) NOT NULL AUTO_INCREMENT,
  `codigoprod` varchar(20) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `imagem` longblob,
  `imagem1` longblob,
  `imagem2` longblob,
  `imagem3` longblob,
  `imagem4` longblob,
  `imagemthumb` blob,
  PRIMARY KEY (`inc`),
  UNIQUE KEY `codigoprod` (`codigoprod`,`codigofilial`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC COMMENT='armazena imagens da tabela produtos';

/*Table structure for table `produtosinventario` */

CREATE TABLE `produtosinventario` (
  `codigo` varchar(20) DEFAULT '0',
  `codigoinc` int(6) NOT NULL DEFAULT '0',
  `descecf` varchar(25) DEFAULT NULL,
  `unidade` char(3) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtddeposito` decimal(10,0) NOT NULL DEFAULT '0',
  `qtdprateleiras` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdvencidos` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdanterior` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdultent` decimal(10,2) NOT NULL DEFAULT '0.00',
  `dataultent` date DEFAULT NULL,
  `qtdprovisoria` decimal(10,2) NOT NULL DEFAULT '0.00',
  `descprovisorio` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pedidoand` decimal(10,2) NOT NULL DEFAULT '0.00',
  `qtdultpedido` decimal(10,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `dataultpedido` date DEFAULT NULL,
  `icms` decimal(6,2) NOT NULL DEFAULT '0.00',
  `ipi` decimal(6,2) NOT NULL DEFAULT '0.00',
  `grupo` varchar(30) NOT NULL DEFAULT '',
  `subgrupo` varchar(30) NOT NULL DEFAULT '',
  `custo` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `customedio` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `ultcusto` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `custototal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `margemlucro` decimal(8,2) NOT NULL DEFAULT '0.00',
  `precovenda` decimal(12,2) NOT NULL DEFAULT '0.00',
  `dataultvenda` date DEFAULT NULL,
  `dataaltpreco` date DEFAULT NULL,
  `ultpreco` decimal(12,2) NOT NULL DEFAULT '0.00',
  `estminimo` decimal(10,2) NOT NULL DEFAULT '0.00',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `situacao` varchar(15) DEFAULT NULL,
  `tributacao` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) NOT NULL DEFAULT '',
  `fabricante` varchar(30) NOT NULL DEFAULT '',
  `tipocomissao` char(1) DEFAULT NULL,
  `datacadastro` date DEFAULT NULL,
  `validade` date DEFAULT NULL,
  `aceitadesconto` char(1) DEFAULT NULL,
  `descontopromocao` decimal(6,2) DEFAULT NULL,
  `validadepromoc` date DEFAULT NULL,
  `descontomaximo` decimal(6,2) NOT NULL DEFAULT '0.00',
  `operador` varchar(15) DEFAULT NULL,
  `pesobruto` decimal(10,3) NOT NULL DEFAULT '0.000',
  `pesoliquido` decimal(10,3) NOT NULL DEFAULT '0.000',
  `marcado` char(1) DEFAULT NULL,
  `embalagem` int(5) DEFAULT NULL,
  `unidembalagem` char(2) DEFAULT NULL,
  `localestoque` varchar(15) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `frete` decimal(6,2) NOT NULL DEFAULT '0.00',
  `01qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `01custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `01vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `02qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `02custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `02vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `03qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `03custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `03vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `04qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `04custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `04vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `05qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `05custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `05vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `06qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `06custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `06vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `07qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `07custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `07vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `08qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `08custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `08vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `09qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `09custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `09vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `10qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `10custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `10vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `11qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `11custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `11vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `12qtd` decimal(15,2) NOT NULL DEFAULT '0.00',
  `12custos` decimal(15,2) NOT NULL DEFAULT '0.00',
  `12vendas` decimal(15,2) NOT NULL DEFAULT '0.00',
  `CodigoFilial` varchar(5) NOT NULL DEFAULT '00001',
  `codigovinculado` varchar(20) DEFAULT NULL,
  `inventario` decimal(10,2) NOT NULL DEFAULT '0.00',
  `documento` int(10) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '0000',
  `qtdretida` decimal(8,2) NOT NULL DEFAULT '0.00',
  `secao` varchar(20) DEFAULT NULL,
  `diasparavencimento` int(3) NOT NULL DEFAULT '0',
  `lote` varchar(15) NOT NULL DEFAULT '',
  `vencimento` date DEFAULT NULL,
  `anoinventario` int(4) NOT NULL DEFAULT '0',
  `inventarioencerrado` char(1) NOT NULL DEFAULT 'N',
  `dataencerramentoinventario` date DEFAULT NULL,
  `qtdaentregar` decimal(10,2) NOT NULL DEFAULT '0.00',
  `precounidade` decimal(12,2) NOT NULL DEFAULT '0.00',
  `generico` char(3) DEFAULT NULL,
  `princativo` varchar(130) DEFAULT NULL,
  `margemsemfinanciamento` decimal(8,2) NOT NULL DEFAULT '0.00',
  `precosemfinanciamento` decimal(8,2) NOT NULL DEFAULT '0.00',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `ativacompdesc` char(1) NOT NULL DEFAULT 'N',
  `inventarionumero` int(4) NOT NULL DEFAULT '1',
  `custofornecedor` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `qtdminimadesc` decimal(8,2) NOT NULL DEFAULT '0.00',
  `qtdprevenda` decimal(10,2) NOT NULL DEFAULT '0.00',
  `parcelamentomax` int(3) NOT NULL DEFAULT '0',
  `precoatacado` decimal(10,2) NOT NULL DEFAULT '0.00',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `detalhetecnico` text,
  `origem` varchar(50) NOT NULL DEFAULT '0 - Nacional',
  `modalidadeDetBaseCalcICMS` varchar(100) DEFAULT NULL,
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `modalidadedetBaseCalcICMsST` varchar(100) DEFAULT NULL,
  `ICMsST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedICMsST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualMargVlrAdICMsST` decimal(5,3) NOT NULL DEFAULT '0.000',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `PIS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `COFINS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `margemlucroliquida` decimal(8,2) NOT NULL DEFAULT '0.00',
  `volumes` int(3) NOT NULL DEFAULT '1',
  `ncm` varchar(8) DEFAULT NULL,
  `nbm` varchar(10) DEFAULT NULL,
  `ncmespecie` char(2) DEFAULT NULL,
  `capacidadevolML` int(5) DEFAULT NULL,
  `situacaoinventario` char(2) NOT NULL DEFAULT '00',
  `tributacaoPIS` char(2) NOT NULL DEFAULT '01',
  `tributacaoCOFINS` char(2) NOT NULL DEFAULT '01',
  `codigoservico` varchar(4) NOT NULL DEFAULT '0000',
  `aliquotaISS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `indicadorproducao` char(1) NOT NULL DEFAULT 'T',
  `indicadorarredondamentotruncamento` char(1) NOT NULL DEFAULT 'A',
  `cfopsaida` varchar(5) NOT NULL DEFAULT '5.102',
  `cfopentrada` varchar(5) NOT NULL DEFAULT '1.102',
  `EADE2mercadoriaEstoque` varchar(33) DEFAULT NULL,
  `EADP2relacaomercadoria` varchar(33) DEFAULT NULL,
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `datafabricacao` date DEFAULT NULL,
  `saldofinalestoque` decimal(10,2) NOT NULL DEFAULT '0.00',
  `complementodescricao` varchar(100) NOT NULL DEFAULT ' ',
  `datafinalestoque` date DEFAULT NULL,
  `horafinalestoque` time DEFAULT NULL,
  `ecffabricacao` varchar(20) NOT NULL DEFAULT ' ',
  KEY `codigo` (`codigo`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `produtoslicitacao` */

CREATE TABLE `produtoslicitacao` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(8,2) NOT NULL DEFAULT '0.00',
  `qtddeposito` decimal(10,0) NOT NULL DEFAULT '0',
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `precovenda` decimal(10,2) NOT NULL DEFAULT '0.00',
  `numero` int(5) NOT NULL DEFAULT '0',
  `fabricante` varchar(30) NOT NULL DEFAULT '',
  `item` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `produtosmateriaprima` */

CREATE TABLE `produtosmateriaprima` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `unidade` char(2) NOT NULL DEFAULT 'UN',
  `custo` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `custototal` decimal(12,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `produtosperdas` */

CREATE TABLE `produtosperdas` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(8,2) NOT NULL DEFAULT '0.00',
  `qtddeposito` decimal(10,0) NOT NULL DEFAULT '0',
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `destino` varchar(50) DEFAULT NULL,
  `observacao` text,
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `tipo` char(1) NOT NULL DEFAULT 'P',
  `numero` int(8) NOT NULL DEFAULT '0',
  `datavencimento` date DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `situacao` varchar(15) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `secao` varchar(20) DEFAULT NULL,
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `cliente` varchar(50) NOT NULL DEFAULT '',
  `icms` int(2) NOT NULL DEFAULT '0',
  `tributacao` char(2) NOT NULL DEFAULT '00',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `cstpis` char(2) NOT NULL DEFAULT '01',
  `cstcofins` char(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(7,2) NOT NULL DEFAULT '0.00',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

/*Table structure for table `produtosretidos` */

CREATE TABLE `produtosretidos` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(8,2) NOT NULL DEFAULT '0.00',
  `qtddeposito` decimal(10,0) NOT NULL DEFAULT '0',
  `data` date DEFAULT NULL,
  `dataretorno` date DEFAULT NULL,
  `qtdretorno` decimal(8,2) NOT NULL DEFAULT '0.00',
  `operador` varchar(10) DEFAULT NULL,
  `assistencia` varchar(50) DEFAULT NULL,
  `previsao` date DEFAULT NULL,
  `contacto` varchar(70) DEFAULT NULL,
  `resolvido` char(1) NOT NULL DEFAULT 'N',
  `tipo` varchar(15) NOT NULL DEFAULT 'Assistencia',
  `custo` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `customedio` decimal(12,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(12,2) NOT NULL DEFAULT '0.00',
  `baixarestoque` char(1) NOT NULL DEFAULT 'S',
  `codigocli` varchar(6) DEFAULT NULL,
  `obs` text,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `produtosvencidos` */

CREATE TABLE `produtosvencidos` (
  `ip` varchar(15) DEFAULT NULL,
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(8,2) NOT NULL DEFAULT '0.00',
  `qtddeposito` decimal(10,0) NOT NULL DEFAULT '0',
  `data` date DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `destino` varchar(50) DEFAULT NULL,
  `observacao` text,
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precovenda` decimal(10,2) NOT NULL DEFAULT '0.00',
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `tipo` char(1) NOT NULL DEFAULT 'P',
  `numero` int(5) NOT NULL DEFAULT '0',
  `datavencimento` date DEFAULT NULL,
  `lote` varchar(15) DEFAULT NULL,
  `fornecedor` varchar(30) NOT NULL DEFAULT '',
  `encerrado` char(1) NOT NULL DEFAULT 'N',
  `qtdprateleiras` decimal(10,2) NOT NULL DEFAULT '0.00',
  `marcado` char(1) NOT NULL DEFAULT 'N',
  `interpolador` datetime DEFAULT NULL,
  `pedidocompra` int(8) DEFAULT NULL,
  `grupo` varchar(30) NOT NULL DEFAULT '',
  `subgrupo` varchar(30) NOT NULL DEFAULT '',
  `situacao` varchar(15) NOT NULL DEFAULT '',
  `fabricante` varchar(30) NOT NULL DEFAULT '',
  `valor` decimal(8,2) DEFAULT '0.00',
  `icms` int(2) NOT NULL DEFAULT '0',
  `tributacao` char(2) NOT NULL DEFAULT '00',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `cstpis` char(2) NOT NULL DEFAULT '01',
  `cstcofins` char(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(7,2) NOT NULL DEFAULT '0.00',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Table structure for table `produtosvencidosubtitutos` */

CREATE TABLE `produtosvencidosubtitutos` (
  `ip` varchar(15) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `pedidocompra` int(8) DEFAULT NULL,
  `interpolador` datetime DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) DEFAULT NULL,
  `custo` decimal(10,5) DEFAULT NULL,
  `tipo` char(1) DEFAULT NULL,
  `observacao` text,
  `resolvido` char(1) NOT NULL DEFAULT 'N',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `situacao` varchar(15) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `produtosweb` */

CREATE TABLE `produtosweb` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `produto` varchar(60) NOT NULL DEFAULT '',
  `chamada` varchar(100) DEFAULT NULL,
  `descricao` text,
  `valor` decimal(10,2) DEFAULT NULL,
  `versao` varchar(10) DEFAULT NULL,
  `imagem` varchar(60) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `profissao` */

CREATE TABLE `profissao` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=19 DEFAULT CHARSET=latin1;

/*Table structure for table `quantidaderegistros` */

CREATE TABLE `quantidaderegistros` (
  `id` int(7) NOT NULL AUTO_INCREMENT,
  `contdav` varchar(33) DEFAULT NULL,
  `vendadav` varchar(33) DEFAULT NULL,
  `contprevendaspaf` varchar(33) DEFAULT NULL,
  `vendaprevendapaf` varchar(33) DEFAULT NULL,
  `produtos` varchar(33) DEFAULT NULL,
  `contdocs` varchar(33) DEFAULT NULL,
  `vendaarquivo` varchar(33) DEFAULT NULL,
  `caixaarquivo` varchar(33) DEFAULT NULL,
  `contrelatoriogerencial` varchar(33) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

/*Table structure for table `r01` */

CREATE TABLE `r01` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `tipo` varchar(3) NOT NULL DEFAULT 'R01',
  `fabricacaoECF` varchar(20) DEFAULT NULL,
  `MFAdicional` varchar(2) DEFAULT NULL,
  `tipoECF` varchar(7) DEFAULT NULL,
  `marcaECF` varchar(20) DEFAULT NULL,
  `modeloECF` varchar(20) DEFAULT NULL,
  `versaoSB` varchar(10) DEFAULT NULL,
  `datainstalacaoSB` date DEFAULT NULL,
  `horainstalacaoSB` time DEFAULT NULL,
  `numeroECF` varchar(3) DEFAULT NULL,
  `numeroUsuarioSubstituicaoECF` varchar(2) DEFAULT NULL,
  `cnpj` varchar(14) DEFAULT NULL,
  `inscricao` varchar(14) DEFAULT NULL,
  `cnpjdesenvolvedora` varchar(14) DEFAULT NULL,
  `inscricaodesenvolvedora` varchar(14) DEFAULT NULL,
  `inscricaomunicipaldesenvolvedora` varchar(14) DEFAULT NULL,
  `razaosocialdesenvolvedora` varchar(40) DEFAULT NULL,
  `aplicativo` varchar(40) DEFAULT NULL,
  `versao` varchar(10) DEFAULT NULL,
  `md5` varchar(33) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `versaoERPAF` varchar(4) DEFAULT NULL,
  `EADdados` varchar(33) DEFAULT NULL,
  `md5exe` varchar(33) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

/*Table structure for table `r02` */

CREATE TABLE `r02` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `tipo` varchar(3) NOT NULL DEFAULT 'R02',
  `fabricacaoECF` varchar(20) DEFAULT NULL,
  `MFadicional` char(1) DEFAULT NULL,
  `modeloECF` varchar(20) DEFAULT NULL,
  `numeroUsuarioSubstituicaoECF` varchar(2) DEFAULT NULL,
  `crz` varchar(6) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cro` varchar(6) DEFAULT NULL,
  `datamovimento` date DEFAULT NULL,
  `dataemissaoreducaoz` date DEFAULT NULL,
  `horaemissaoreducaoz` time DEFAULT NULL,
  `vendabrutadiaria` decimal(12,2) NOT NULL DEFAULT '0.00',
  `parametroISSQNdesconto` char(1) DEFAULT NULL,
  `numeroECF` varchar(3) DEFAULT NULL,
  `gtfinal` decimal(12,2) NOT NULL DEFAULT '0.00',
  `EADdados` varchar(33) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

/*Table structure for table `r03` */

CREATE TABLE `r03` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `tipo` varchar(3) NOT NULL DEFAULT 'R03',
  `fabricacaoECF` varchar(20) DEFAULT NULL,
  `MFAdicional` char(1) DEFAULT NULL,
  `modeloECF` varchar(20) DEFAULT NULL,
  `numeroUsuarioSubstituicaoECF` varchar(2) DEFAULT NULL,
  `CRZ` varchar(6) DEFAULT NULL,
  `totalizadorParcial` varchar(7) DEFAULT NULL,
  `valoracumulado` decimal(12,2) DEFAULT NULL,
  `numeroECF` varchar(3) DEFAULT NULL,
  `EADdados` varchar(33) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=151 DEFAULT CHARSET=latin1;

/*Table structure for table `receitas` */

CREATE TABLE `receitas` (
  `codigofilial` varchar(5) DEFAULT NULL,
  `conta` varchar(5) NOT NULL DEFAULT '0',
  `descricao` varchar(30) DEFAULT NULL,
  `grupo` varchar(15) DEFAULT NULL,
  `liberada` char(1) NOT NULL DEFAULT 'S',
  KEY `conta` (`descricao`,`conta`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `receitasub` */

CREATE TABLE `receitasub` (
  `codigofilial` varchar(5) DEFAULT NULL,
  `idsubconta` varchar(5) NOT NULL DEFAULT '0',
  `descricao` varchar(30) DEFAULT NULL,
  `idconta` varchar(5) DEFAULT NULL,
  `receita` char(1) DEFAULT NULL,
  `grupo` varchar(15) DEFAULT NULL,
  `liberada` char(1) NOT NULL DEFAULT 'S',
  KEY `idconta` (`idconta`,`descricao`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `reposicao` */

CREATE TABLE `reposicao` (
  `numero` int(10) DEFAULT '0',
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00',
  `operador` varchar(10) DEFAULT NULL,
  `encerrada` char(1) NOT NULL DEFAULT 'N',
  `codigofilial` varchar(5) NOT NULL DEFAULT '',
  `observacao` text
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `restricoes` */

CREATE TABLE `restricoes` (
  `codigofilial` varchar(5) DEFAULT NULL,
  `descricaofilial` varchar(20) DEFAULT NULL,
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `ip` varchar(15) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `descricao` varchar(100) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `codigoproduto` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `liberada` char(1) NOT NULL DEFAULT 'N',
  `hora` time DEFAULT NULL,
  `documento` int(8) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=315 DEFAULT CHARSET=latin1;

/*Table structure for table `retornoboleto` */

CREATE TABLE `retornoboleto` (
  `inc` int(6) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `ip` varchar(15) DEFAULT NULL,
  `codigocliente` int(6) DEFAULT NULL,
  `nome` varchar(50) DEFAULT NULL,
  `documento` varchar(10) DEFAULT NULL,
  `vencimento` date DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `juros` decimal(8,2) NOT NULL DEFAULT '0.00',
  `descontos` decimal(8,2) NOT NULL DEFAULT '0.00',
  `tarifa` decimal(8,2) NOT NULL DEFAULT '0.00',
  `valorpago` decimal(10,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `baixado` char(1) NOT NULL DEFAULT 'N',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `romaneio` */

CREATE TABLE `romaneio` (
  `inc` int(7) NOT NULL AUTO_INCREMENT,
  `numeroromaneio` int(7) DEFAULT NULL,
  `documento` int(10) DEFAULT NULL,
  `datadocumento` date DEFAULT NULL,
  `data` date DEFAULT NULL,
  `ordem` int(4) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `responsavelreceber` varchar(60) DEFAULT NULL,
  `qtditens` int(5) DEFAULT NULL,
  `endereco` varchar(60) DEFAULT NULL,
  `numero` varchar(10) DEFAULT NULL,
  `bairro` varchar(20) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `cidade` varchar(30) DEFAULT NULL,
  `estado` char(2) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `secao` */

CREATE TABLE `secao` (
  `inc` int(11) NOT NULL AUTO_INCREMENT,
  `secao` varchar(20) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `senhacontrole` */

CREATE TABLE `senhacontrole` (
  `codigocliente` int(6) DEFAULT NULL,
  `cliente` varchar(50) DEFAULT NULL,
  `data` datetime DEFAULT NULL,
  `usuario` varchar(30) DEFAULT NULL,
  `produto` varchar(20) DEFAULT NULL,
  `senha` varchar(40) DEFAULT NULL,
  `validade` date DEFAULT NULL,
  `emailenviado` char(1) NOT NULL DEFAULT 'N'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `senhas` */

CREATE TABLE `senhas` (
  `administrador` char(1) NOT NULL DEFAULT 'S',
  `ip` varchar(15) DEFAULT NULL,
  `operador` varchar(10) NOT NULL DEFAULT '',
  `codigo` int(4) NOT NULL AUTO_INCREMENT,
  `codigovendedor` char(3) NOT NULL DEFAULT '000',
  `senha` varchar(60) NOT NULL DEFAULT '',
  `grupo` varchar(15) DEFAULT NULL,
  `gerente` char(1) NOT NULL DEFAULT 'S',
  `estcad` char(1) NOT NULL DEFAULT 'S',
  `estalt` char(1) NOT NULL DEFAULT 'S',
  `estcusto` char(1) NOT NULL DEFAULT 'S',
  `estentrada` char(1) NOT NULL DEFAULT 'S',
  `Estvercustos` char(1) NOT NULL DEFAULT 'S',
  `estforn` char(1) NOT NULL DEFAULT 'S',
  `estprat` char(1) NOT NULL DEFAULT 'S',
  `estacerto` char(1) NOT NULL DEFAULT 'S',
  `esttransf` char(1) NOT NULL DEFAULT 'S',
  `estperdas` char(1) NOT NULL DEFAULT 'S',
  `estreter` char(1) NOT NULL DEFAULT 'S',
  `estreverter` char(1) NOT NULL DEFAULT 'S',
  `estnegativo` char(1) NOT NULL DEFAULT 'S',
  `estprovisorio` char(1) NOT NULL DEFAULT 'S',
  `estexcluir` char(1) NOT NULL DEFAULT 'S',
  `estinventario` char(1) NOT NULL DEFAULT 'S',
  `estinventarioencerrar` char(1) NOT NULL DEFAULT 'S',
  `estbalanco` char(1) NOT NULL DEFAULT 'S',
  `estvenderabaixocusto` char(1) NOT NULL DEFAULT 'S',
  `venvenda` char(1) NOT NULL DEFAULT 'S',
  `venalterarpreco` char(1) NOT NULL DEFAULT 'S',
  `venliberarrest` char(1) NOT NULL DEFAULT 'S',
  `vendesconto` char(1) NOT NULL DEFAULT 'S',
  `vendevolucao` char(1) NOT NULL DEFAULT 'S',
  `venexcluir` char(1) NOT NULL DEFAULT 'S',
  `venaltervendedor` char(1) NOT NULL DEFAULT 'S',
  `vendaaltvenc` char(1) NOT NULL DEFAULT 'S',
  `vendexcluiritem` char(1) NOT NULL DEFAULT 'S',
  `relcaixa` char(1) NOT NULL DEFAULT 'S',
  `relvendas` char(1) NOT NULL DEFAULT 'S',
  `relreceber` char(1) NOT NULL DEFAULT 'S',
  `relcomissao` char(1) NOT NULL DEFAULT 'S',
  `outcadcp` char(1) NOT NULL DEFAULT 'S',
  `outcontpag` char(1) NOT NULL DEFAULT 'S',
  `outmovbanco` char(1) NOT NULL DEFAULT 'S',
  `outbackup` char(1) NOT NULL DEFAULT 'S',
  `outretirada` char(1) NOT NULL DEFAULT 'S',
  `outimpcupom` char(1) NOT NULL DEFAULT 'S',
  `outreimprecibo` char(1) NOT NULL DEFAULT 'S',
  `outfechmen` char(1) NOT NULL DEFAULT 'S',
  `outfechanu` char(1) NOT NULL DEFAULT 'S',
  `outcheques` char(1) NOT NULL DEFAULT 'S',
  `rotcadoper` char(1) NOT NULL DEFAULT 'S',
  `outcartoes` char(1) NOT NULL DEFAULT 'S',
  `rotconf` char(1) NOT NULL DEFAULT 'S',
  `rotprazos` char(1) NOT NULL DEFAULT 'S',
  `rotcaixa` char(1) NOT NULL DEFAULT 'S',
  `rotiniciardia` char(1) NOT NULL DEFAULT 'S',
  `rotaltersaldo` char(1) NOT NULL DEFAULT 'S',
  `rotclasse` char(3) NOT NULL DEFAULT 'S',
  `rotcheques` char(1) NOT NULL DEFAULT 'S',
  `rotcartoes` char(1) NOT NULL DEFAULT 'S',
  `rotcaixaadm` char(1) NOT NULL DEFAULT 'S',
  `rotcadvendedor` char(1) NOT NULL DEFAULT 'S',
  `rotlogar` char(1) NOT NULL DEFAULT 'S',
  `clialterarcad` char(1) NOT NULL DEFAULT 'S',
  `clialtercad2` char(1) NOT NULL DEFAULT 'S',
  `clicadastrar` char(1) NOT NULL DEFAULT 'S',
  `clirestricao` char(1) NOT NULL DEFAULT 'S',
  `clireceber` char(1) NOT NULL DEFAULT 'S',
  `cliestornar` char(1) NOT NULL DEFAULT 'S',
  `clivermovimento` char(1) NOT NULL DEFAULT 'S',
  `cliverdados` char(1) NOT NULL DEFAULT 'S',
  `clialterarsenha` char(1) NOT NULL DEFAULT 'S',
  `clialterarpar` char(1) NOT NULL DEFAULT 'S',
  `cliinadimplente` char(1) NOT NULL DEFAULT 'S',
  `clidescontos` char(1) NOT NULL DEFAULT 'S',
  `cliperdoar` char(1) NOT NULL DEFAULT 'S',
  `climovextra` char(1) NOT NULL DEFAULT 'S',
  `clirenegocia` char(1) NOT NULL DEFAULT 'S',
  `CliCredSit` char(1) NOT NULL DEFAULT 'S',
  `Clisaldo` char(1) NOT NULL DEFAULT 'S',
  `Cliexcluir` char(1) NOT NULL DEFAULT 'S',
  `clialterarvencimento` char(1) NOT NULL DEFAULT 'S',
  `outfaturamento` char(1) NOT NULL DEFAULT 'S',
  `clisituacao` char(1) NOT NULL DEFAULT 'S',
  `clijurosch` char(1) NOT NULL DEFAULT 'S',
  `CodigoFilial` varchar(5) NOT NULL DEFAULT 'S',
  `Clidispensa` char(1) NOT NULL DEFAULT 'S',
  `outmudamsg` char(1) NOT NULL DEFAULT 'S',
  `OutFilial` char(1) NOT NULL DEFAULT 'S',
  `despesasCad` char(1) NOT NULL DEFAULT 'S',
  `despesasmudar` char(1) NOT NULL DEFAULT 'S',
  `Despesaslancar` char(1) NOT NULL DEFAULT 'S',
  `DespesasFec` char(1) NOT NULL DEFAULT 'S',
  `receitascad` char(1) NOT NULL DEFAULT 'S',
  `receitaslancar` char(1) NOT NULL DEFAULT 'S',
  `receitasfec` char(1) NOT NULL DEFAULT 'S',
  `servicogerar` char(1) NOT NULL DEFAULT 'S',
  `filialacesso` varchar(5) NOT NULL DEFAULT 'Todas',
  `outalterarboleto` char(1) NOT NULL DEFAULT 'S',
  `clicredprov` char(1) NOT NULL DEFAULT 'S',
  `estcontrolevencidos` char(1) NOT NULL DEFAULT 'S',
  `rotsobrafalta` char(1) NOT NULL DEFAULT 'S',
  `venmudarvendedor` char(1) NOT NULL DEFAULT 'S',
  `relacessorelmatriz` char(1) NOT NULL DEFAULT 'S',
  `relreceberacompmensal` char(1) NOT NULL DEFAULT 'S',
  `relreceberanalcli` char(1) NOT NULL DEFAULT 'S',
  `relcontasgeradas` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecsintfilial` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecanacli` char(1) NOT NULL DEFAULT 'S',
  `relmovclientes` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecsintFil` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecanalcli` char(1) NOT NULL DEFAULT 'S',
  `rotcontratos` char(1) NOT NULL DEFAULT 'S',
  `relinadimplencia` char(1) NOT NULL DEFAULT 'S',
  `outpatrimonio` char(1) NOT NULL DEFAULT 'S',
  `cliRecBoletoDeposito` char(1) NOT NULL DEFAULT 'S',
  `clilancempdh` char(1) NOT NULL DEFAULT 'S',
  `clilancempch` char(1) NOT NULL DEFAULT 'S',
  `clilanccomprati` char(1) NOT NULL DEFAULT 'S',
  `clitrocach` char(1) NOT NULL DEFAULT 'S',
  `horainiciartrabalho` time NOT NULL DEFAULT '00:01:00',
  `horafinalizartrabalho` time NOT NULL DEFAULT '23:59:00',
  `outfuncionarios` char(1) NOT NULL DEFAULT 'S',
  `ultimasenha` varchar(60) DEFAULT NULL,
  `dataultimasenha` date DEFAULT NULL,
  `logado` char(1) NOT NULL DEFAULT 'N',
  `fabricaacessoestoque` char(1) NOT NULL DEFAULT 'S',
  `fabricafinalizarproducao` char(1) NOT NULL DEFAULT 'S',
  `fabricaentradamateriaprima` char(1) NOT NULL DEFAULT 'S',
  `esthist` char(1) NOT NULL DEFAULT 'S',
  `vendarredondamento` char(1) NOT NULL DEFAULT 'S',
  `outcontpagquitar` char(1) NOT NULL DEFAULT 'S',
  `vendecfprevenda` char(1) NOT NULL DEFAULT 'S',
  `clirenegvalor` char(1) NOT NULL DEFAULT 'N',
  `despesasrel` char(1) NOT NULL DEFAULT 'S',
  `osabrir` char(1) NOT NULL DEFAULT 'S',
  `osfechar` char(1) NOT NULL DEFAULT 'S',
  `osincluirpecas` char(1) NOT NULL DEFAULT 'S',
  `osverdadoscliente` char(1) NOT NULL DEFAULT 'S',
  `osmodificar` char(1) NOT NULL DEFAULT 'S',
  `venditemgeral` char(1) NOT NULL DEFAULT 'S',
  `relauditoria` char(1) NOT NULL DEFAULT 'S',
  `venfinprevendapre` char(1) NOT NULL DEFAULT 'N',
  `vendprevenda` char(1) NOT NULL DEFAULT 'S',
  `mudarenderecoentrega` char(1) NOT NULL DEFAULT 'S',
  `entregaposterior` char(1) NOT NULL DEFAULT 'S',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `fidelizacao` char(1) NOT NULL DEFAULT 'S',
  `relfecfinanceiro` char(1) NOT NULL DEFAULT 'S',
  `codigodefault` varchar(4) DEFAULT NULL,
  `concluirentrega` char(1) NOT NULL DEFAULT 'S',
  `lancarbonusvenda` char(1) NOT NULL DEFAULT 'S',
  `trocarsenha` char(1) NOT NULL DEFAULT 'N',
  `usaragenda` char(1) NOT NULL DEFAULT 'S',
  `vendescontogerencial` char(1) NOT NULL DEFAULT 'N',
  `estconstransf` char(1) NOT NULL DEFAULT 'N',
  `gerenciaos` char(1) NOT NULL DEFAULT 'N',
  `excluiritemos` char(1) DEFAULT 'N',
  `controlecheques` char(1) NOT NULL DEFAULT 'S',
  `mudarnomeclientecupom` char(1) NOT NULL DEFAULT 'N',
  `pesquisadocs` char(1) NOT NULL DEFAULT 'S',
  `modificarmesfiscal` char(1) NOT NULL DEFAULT 'N',
  `atribuirfuncionarioos` char(1) NOT NULL DEFAULT 'N',
  `relmetasvendas` char(1) NOT NULL DEFAULT 'N',
  `controleproducaoos` char(1) NOT NULL DEFAULT 'N',
  `outrestricoesbloq` char(1) NOT NULL DEFAULT 'N',
  `outrosvervalorcontaspagar` char(1) NOT NULL DEFAULT 'S',
  `verhistoricocompracliente` char(1) NOT NULL DEFAULT 'N',
  `funcao` varchar(15) DEFAULT NULL,
  `biometria` text,
  `ativabiometria` char(1) NOT NULL DEFAULT 'N',
  `venitenscodigoduplos` char(1) NOT NULL DEFAULT 'S',
  PRIMARY KEY (`codigo`),
  KEY `operador_2` (`operador`)
) ENGINE=MyISAM AUTO_INCREMENT=19 DEFAULT CHARSET=latin1;

/*Table structure for table `senhasdefault` */

CREATE TABLE `senhasdefault` (
  `administrador` char(1) NOT NULL DEFAULT 'S',
  `ip` varchar(15) DEFAULT NULL,
  `operador` varchar(10) NOT NULL DEFAULT '',
  `codigo` int(4) NOT NULL DEFAULT '0',
  `codigovendedor` char(3) NOT NULL DEFAULT '000',
  `senha` varchar(60) NOT NULL DEFAULT '',
  `grupo` varchar(15) DEFAULT NULL,
  `gerente` char(1) NOT NULL DEFAULT 'S',
  `estcad` char(1) NOT NULL DEFAULT 'S',
  `estalt` char(1) NOT NULL DEFAULT 'S',
  `estcusto` char(1) NOT NULL DEFAULT 'S',
  `estentrada` char(1) NOT NULL DEFAULT 'S',
  `Estvercustos` char(1) NOT NULL DEFAULT 'S',
  `estforn` char(1) NOT NULL DEFAULT 'S',
  `estprat` char(1) NOT NULL DEFAULT 'S',
  `estacerto` char(1) NOT NULL DEFAULT 'S',
  `esttransf` char(1) NOT NULL DEFAULT 'S',
  `estperdas` char(1) NOT NULL DEFAULT 'S',
  `estreter` char(1) NOT NULL DEFAULT 'S',
  `estreverter` char(1) NOT NULL DEFAULT 'S',
  `estnegativo` char(1) NOT NULL DEFAULT 'S',
  `estprovisorio` char(1) NOT NULL DEFAULT 'S',
  `estexcluir` char(1) NOT NULL DEFAULT 'S',
  `estinventario` char(1) NOT NULL DEFAULT 'S',
  `estinventarioencerrar` char(1) NOT NULL DEFAULT 'S',
  `estbalanco` char(1) NOT NULL DEFAULT 'S',
  `estvenderabaixocusto` char(1) NOT NULL DEFAULT 'S',
  `venvenda` char(1) NOT NULL DEFAULT 'S',
  `venalterarpreco` char(1) NOT NULL DEFAULT 'S',
  `venliberarrest` char(1) NOT NULL DEFAULT 'S',
  `vendesconto` char(1) NOT NULL DEFAULT 'S',
  `vendevolucao` char(1) NOT NULL DEFAULT 'S',
  `venexcluir` char(1) NOT NULL DEFAULT 'S',
  `venaltervendedor` char(1) NOT NULL DEFAULT 'S',
  `vendaaltvenc` char(1) NOT NULL DEFAULT 'S',
  `vendexcluiritem` char(1) NOT NULL DEFAULT 'S',
  `relcaixa` char(1) NOT NULL DEFAULT 'S',
  `relvendas` char(1) NOT NULL DEFAULT 'S',
  `relreceber` char(1) NOT NULL DEFAULT 'S',
  `relcomissao` char(1) NOT NULL DEFAULT 'S',
  `outcadcp` char(1) NOT NULL DEFAULT 'S',
  `outcontpag` char(1) NOT NULL DEFAULT 'S',
  `outmovbanco` char(1) NOT NULL DEFAULT 'S',
  `outbackup` char(1) NOT NULL DEFAULT 'S',
  `outretirada` char(1) NOT NULL DEFAULT 'S',
  `outimpcupom` char(1) NOT NULL DEFAULT 'S',
  `outreimprecibo` char(1) NOT NULL DEFAULT 'S',
  `outfechmen` char(1) NOT NULL DEFAULT 'S',
  `outfechanu` char(1) NOT NULL DEFAULT 'S',
  `outcheques` char(1) NOT NULL DEFAULT 'S',
  `rotcadoper` char(1) NOT NULL DEFAULT 'S',
  `outcartoes` char(1) NOT NULL DEFAULT 'S',
  `rotconf` char(1) NOT NULL DEFAULT 'S',
  `rotprazos` char(1) NOT NULL DEFAULT 'S',
  `rotcaixa` char(1) NOT NULL DEFAULT 'S',
  `rotiniciardia` char(1) NOT NULL DEFAULT 'S',
  `rotaltersaldo` char(1) NOT NULL DEFAULT 'S',
  `rotclasse` char(3) NOT NULL DEFAULT 'S',
  `rotcheques` char(1) NOT NULL DEFAULT 'S',
  `rotcartoes` char(1) NOT NULL DEFAULT 'S',
  `rotcaixaadm` char(1) NOT NULL DEFAULT 'S',
  `rotcadvendedor` char(1) NOT NULL DEFAULT 'S',
  `rotlogar` char(1) NOT NULL DEFAULT 'S',
  `clialterarcad` char(1) NOT NULL DEFAULT 'S',
  `clialtercad2` char(1) NOT NULL DEFAULT 'S',
  `clicadastrar` char(1) NOT NULL DEFAULT 'S',
  `clirestricao` char(1) NOT NULL DEFAULT 'S',
  `clireceber` char(1) NOT NULL DEFAULT 'S',
  `cliestornar` char(1) NOT NULL DEFAULT 'S',
  `clivermovimento` char(1) NOT NULL DEFAULT 'S',
  `cliverdados` char(1) NOT NULL DEFAULT 'S',
  `clialterarsenha` char(1) NOT NULL DEFAULT 'S',
  `clialterarpar` char(1) NOT NULL DEFAULT 'S',
  `cliinadimplente` char(1) NOT NULL DEFAULT 'S',
  `clidescontos` char(1) NOT NULL DEFAULT 'S',
  `cliperdoar` char(1) NOT NULL DEFAULT 'S',
  `climovextra` char(1) NOT NULL DEFAULT 'S',
  `clirenegocia` char(1) NOT NULL DEFAULT 'S',
  `CliCredSit` char(1) NOT NULL DEFAULT 'S',
  `Clisaldo` char(1) NOT NULL DEFAULT 'S',
  `Cliexcluir` char(1) NOT NULL DEFAULT 'S',
  `clialterarvencimento` char(1) NOT NULL DEFAULT 'S',
  `outfaturamento` char(1) NOT NULL DEFAULT 'S',
  `clisituacao` char(1) NOT NULL DEFAULT 'S',
  `clijurosch` char(1) NOT NULL DEFAULT 'S',
  `CodigoFilial` varchar(5) NOT NULL DEFAULT 'S',
  `Clidispensa` char(1) NOT NULL DEFAULT 'S',
  `outmudamsg` char(1) NOT NULL DEFAULT 'S',
  `OutFilial` char(1) NOT NULL DEFAULT 'S',
  `despesasCad` char(1) NOT NULL DEFAULT 'S',
  `despesasmudar` char(1) NOT NULL DEFAULT 'S',
  `Despesaslancar` char(1) NOT NULL DEFAULT 'S',
  `DespesasFec` char(1) NOT NULL DEFAULT 'S',
  `receitascad` char(1) NOT NULL DEFAULT 'S',
  `receitaslancar` char(1) NOT NULL DEFAULT 'S',
  `receitasfec` char(1) NOT NULL DEFAULT 'S',
  `servicogerar` char(1) NOT NULL DEFAULT 'S',
  `filialacesso` varchar(5) NOT NULL DEFAULT 'Todas',
  `outalterarboleto` char(1) NOT NULL DEFAULT 'S',
  `clicredprov` char(1) NOT NULL DEFAULT 'S',
  `estcontrolevencidos` char(1) NOT NULL DEFAULT 'S',
  `rotsobrafalta` char(1) NOT NULL DEFAULT 'S',
  `venmudarvendedor` char(1) NOT NULL DEFAULT 'S',
  `relacessorelmatriz` char(1) NOT NULL DEFAULT 'S',
  `relreceberacompmensal` char(1) NOT NULL DEFAULT 'S',
  `relreceberanalcli` char(1) NOT NULL DEFAULT 'S',
  `relcontasgeradas` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecsintfilial` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecanacli` char(1) NOT NULL DEFAULT 'S',
  `relmovclientes` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecsintFil` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecanalcli` char(1) NOT NULL DEFAULT 'S',
  `rotcontratos` char(1) NOT NULL DEFAULT 'S',
  `relinadimplencia` char(1) NOT NULL DEFAULT 'S',
  `outpatrimonio` char(1) NOT NULL DEFAULT 'S',
  `cliRecBoletoDeposito` char(1) NOT NULL DEFAULT 'S',
  `clilancempdh` char(1) NOT NULL DEFAULT 'S',
  `clilancempch` char(1) NOT NULL DEFAULT 'S',
  `clilanccomprati` char(1) NOT NULL DEFAULT 'S',
  `clitrocach` char(1) NOT NULL DEFAULT 'S',
  `horainiciartrabalho` time NOT NULL DEFAULT '00:01:00',
  `horafinalizartrabalho` time NOT NULL DEFAULT '23:59:00',
  `outfuncionarios` char(1) NOT NULL DEFAULT 'S',
  `ultimasenha` varchar(60) DEFAULT NULL,
  `dataultimasenha` date DEFAULT NULL,
  `logado` char(1) NOT NULL DEFAULT 'N',
  `fabricaacessoestoque` char(1) NOT NULL DEFAULT 'S',
  `fabricafinalizarproducao` char(1) NOT NULL DEFAULT 'S',
  `fabricaentradamateriaprima` char(1) NOT NULL DEFAULT 'S',
  `esthist` char(1) NOT NULL DEFAULT 'S',
  `vendarredondamento` char(1) NOT NULL DEFAULT 'S',
  `outcontpagquitar` char(1) NOT NULL DEFAULT 'S',
  `vendecfprevenda` char(1) NOT NULL DEFAULT 'S',
  `clirenegvalor` char(1) NOT NULL DEFAULT 'N',
  `despesasrel` char(1) NOT NULL DEFAULT 'S',
  `osabrir` char(1) NOT NULL DEFAULT 'S',
  `osfechar` char(1) NOT NULL DEFAULT 'S',
  `osincluirpecas` char(1) NOT NULL DEFAULT 'S',
  `osverdadoscliente` char(1) NOT NULL DEFAULT 'S',
  `osmodificar` char(1) NOT NULL DEFAULT 'S',
  `venditemgeral` char(1) NOT NULL DEFAULT 'S',
  `relauditoria` char(1) NOT NULL DEFAULT 'S',
  `venfinprevendapre` char(1) NOT NULL DEFAULT 'N',
  `vendprevenda` char(1) NOT NULL DEFAULT 'S',
  `mudarenderecoentrega` char(1) NOT NULL DEFAULT 'S',
  `entregaposterior` char(1) NOT NULL DEFAULT 'S',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `fidelizacao` char(1) NOT NULL DEFAULT 'S',
  `relfecfinanceiro` char(1) NOT NULL DEFAULT 'S',
  `codigodefault` varchar(4) DEFAULT NULL,
  `concluirentrega` char(1) NOT NULL DEFAULT 'S',
  `lancarbonusvenda` char(1) NOT NULL DEFAULT 'S',
  `trocarsenha` char(1) NOT NULL DEFAULT 'N',
  `usaragenda` char(1) NOT NULL DEFAULT 'S',
  `vendescontogerencial` char(1) NOT NULL DEFAULT 'N',
  `estconstransf` char(1) NOT NULL DEFAULT 'N',
  `gerenciaos` char(1) NOT NULL DEFAULT 'N',
  `excluiritemos` char(1) DEFAULT 'N',
  `controlecheques` char(1) NOT NULL DEFAULT 'S',
  `mudarnomeclientecupom` char(1) NOT NULL DEFAULT 'N',
  `pesquisadocs` char(1) NOT NULL DEFAULT 'S',
  `modificarmesfiscal` char(1) NOT NULL DEFAULT 'N',
  `atribuirfuncionarioos` char(1) NOT NULL DEFAULT 'N',
  `relmetasvendas` char(1) NOT NULL DEFAULT 'N',
  `controleproducaoos` char(1) NOT NULL DEFAULT 'N',
  `outrestricoesbloq` char(1) NOT NULL DEFAULT 'N',
  `outrosvervalorcontaspagar` char(1) NOT NULL DEFAULT 'S',
  `verhistoricocompracliente` char(1) NOT NULL DEFAULT 'N',
  `funcao` varchar(15) DEFAULT NULL,
  `biometria` text,
  `ativabiometria` char(1) NOT NULL DEFAULT 'N',
  `venitenscodigoduplos` char(1) NOT NULL DEFAULT 'S',
  KEY `operador_2` (`operador`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `senhaspadrao` */

CREATE TABLE `senhaspadrao` (
  `administrador` char(1) NOT NULL DEFAULT 'S',
  `ip` varchar(15) DEFAULT NULL,
  `operador` varchar(10) NOT NULL DEFAULT '',
  `codigo` int(4) NOT NULL AUTO_INCREMENT,
  `codigovendedor` char(3) NOT NULL DEFAULT '000',
  `senha` varchar(60) NOT NULL DEFAULT '',
  `grupo` varchar(15) DEFAULT NULL,
  `gerente` char(1) NOT NULL DEFAULT 'S',
  `estcad` char(1) NOT NULL DEFAULT 'S',
  `estalt` char(1) NOT NULL DEFAULT 'S',
  `estcusto` char(1) NOT NULL DEFAULT 'S',
  `estentrada` char(1) NOT NULL DEFAULT 'S',
  `Estvercustos` char(1) NOT NULL DEFAULT 'S',
  `estforn` char(1) NOT NULL DEFAULT 'S',
  `estprat` char(1) NOT NULL DEFAULT 'S',
  `estacerto` char(1) NOT NULL DEFAULT 'S',
  `esttransf` char(1) NOT NULL DEFAULT 'S',
  `estperdas` char(1) NOT NULL DEFAULT 'S',
  `estreter` char(1) NOT NULL DEFAULT 'S',
  `estreverter` char(1) NOT NULL DEFAULT 'S',
  `estnegativo` char(1) NOT NULL DEFAULT 'S',
  `estprovisorio` char(1) NOT NULL DEFAULT 'S',
  `estexcluir` char(1) NOT NULL DEFAULT 'S',
  `estinventario` char(1) NOT NULL DEFAULT 'S',
  `estinventarioencerrar` char(1) NOT NULL DEFAULT 'S',
  `estbalanco` char(1) NOT NULL DEFAULT 'S',
  `estvenderabaixocusto` char(1) NOT NULL DEFAULT 'S',
  `venvenda` char(1) NOT NULL DEFAULT 'S',
  `venalterarpreco` char(1) NOT NULL DEFAULT 'S',
  `venliberarrest` char(1) NOT NULL DEFAULT 'S',
  `vendesconto` char(1) NOT NULL DEFAULT 'S',
  `vendevolucao` char(1) NOT NULL DEFAULT 'S',
  `venexcluir` char(1) NOT NULL DEFAULT 'S',
  `venaltervendedor` char(1) NOT NULL DEFAULT 'S',
  `vendaaltvenc` char(1) NOT NULL DEFAULT 'S',
  `vendexcluiritem` char(1) NOT NULL DEFAULT 'S',
  `relcaixa` char(1) NOT NULL DEFAULT 'S',
  `relvendas` char(1) NOT NULL DEFAULT 'S',
  `relreceber` char(1) NOT NULL DEFAULT 'S',
  `relcomissao` char(1) NOT NULL DEFAULT 'S',
  `outcadcp` char(1) NOT NULL DEFAULT 'S',
  `outcontpag` char(1) NOT NULL DEFAULT 'S',
  `outmovbanco` char(1) NOT NULL DEFAULT 'S',
  `outbackup` char(1) NOT NULL DEFAULT 'S',
  `outretirada` char(1) NOT NULL DEFAULT 'S',
  `outimpcupom` char(1) NOT NULL DEFAULT 'S',
  `outreimprecibo` char(1) NOT NULL DEFAULT 'S',
  `outfechmen` char(1) NOT NULL DEFAULT 'S',
  `outfechanu` char(1) NOT NULL DEFAULT 'S',
  `outcheques` char(1) NOT NULL DEFAULT 'S',
  `rotcadoper` char(1) NOT NULL DEFAULT 'S',
  `outcartoes` char(1) NOT NULL DEFAULT 'S',
  `rotconf` char(1) NOT NULL DEFAULT 'S',
  `rotprazos` char(1) NOT NULL DEFAULT 'S',
  `rotcaixa` char(1) NOT NULL DEFAULT 'S',
  `rotiniciardia` char(1) NOT NULL DEFAULT 'S',
  `rotaltersaldo` char(1) NOT NULL DEFAULT 'S',
  `rotclasse` char(3) NOT NULL DEFAULT 'S',
  `rotcheques` char(1) NOT NULL DEFAULT 'S',
  `rotcartoes` char(1) NOT NULL DEFAULT 'S',
  `rotcaixaadm` char(1) NOT NULL DEFAULT 'S',
  `rotcadvendedor` char(1) NOT NULL DEFAULT 'S',
  `rotlogar` char(1) NOT NULL DEFAULT 'S',
  `clialterarcad` char(1) NOT NULL DEFAULT 'S',
  `clialtercad2` char(1) NOT NULL DEFAULT 'S',
  `clicadastrar` char(1) NOT NULL DEFAULT 'S',
  `clirestricao` char(1) NOT NULL DEFAULT 'S',
  `clireceber` char(1) NOT NULL DEFAULT 'S',
  `cliestornar` char(1) NOT NULL DEFAULT 'S',
  `clivermovimento` char(1) NOT NULL DEFAULT 'S',
  `cliverdados` char(1) NOT NULL DEFAULT 'S',
  `clialterarsenha` char(1) NOT NULL DEFAULT 'S',
  `clialterarpar` char(1) NOT NULL DEFAULT 'S',
  `cliinadimplente` char(1) NOT NULL DEFAULT 'S',
  `clidescontos` char(1) NOT NULL DEFAULT 'S',
  `cliperdoar` char(1) NOT NULL DEFAULT 'S',
  `climovextra` char(1) NOT NULL DEFAULT 'S',
  `clirenegocia` char(1) NOT NULL DEFAULT 'S',
  `CliCredSit` char(1) NOT NULL DEFAULT 'S',
  `Clisaldo` char(1) NOT NULL DEFAULT 'S',
  `Cliexcluir` char(1) NOT NULL DEFAULT 'S',
  `clialterarvencimento` char(1) NOT NULL DEFAULT 'S',
  `outfaturamento` char(1) NOT NULL DEFAULT 'S',
  `clisituacao` char(1) NOT NULL DEFAULT 'S',
  `clijurosch` char(1) NOT NULL DEFAULT 'S',
  `CodigoFilial` varchar(5) NOT NULL DEFAULT 'S',
  `Clidispensa` char(1) NOT NULL DEFAULT 'S',
  `outmudamsg` char(1) NOT NULL DEFAULT 'S',
  `OutFilial` char(1) NOT NULL DEFAULT 'S',
  `despesasCad` char(1) NOT NULL DEFAULT 'S',
  `despesasmudar` char(1) NOT NULL DEFAULT 'S',
  `Despesaslancar` char(1) NOT NULL DEFAULT 'S',
  `DespesasFec` char(1) NOT NULL DEFAULT 'S',
  `receitascad` char(1) NOT NULL DEFAULT 'S',
  `receitaslancar` char(1) NOT NULL DEFAULT 'S',
  `receitasfec` char(1) NOT NULL DEFAULT 'S',
  `servicogerar` char(1) NOT NULL DEFAULT 'S',
  `filialacesso` varchar(5) NOT NULL DEFAULT 'Todas',
  `outalterarboleto` char(1) NOT NULL DEFAULT 'S',
  `clicredprov` char(1) NOT NULL DEFAULT 'S',
  `estcontrolevencidos` char(1) NOT NULL DEFAULT 'S',
  `rotsobrafalta` char(1) NOT NULL DEFAULT 'S',
  `venmudarvendedor` char(1) NOT NULL DEFAULT 'S',
  `relacessorelmatriz` char(1) NOT NULL DEFAULT 'S',
  `relreceberacompmensal` char(1) NOT NULL DEFAULT 'S',
  `relreceberanalcli` char(1) NOT NULL DEFAULT 'S',
  `relcontasgeradas` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecsintfilial` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecanacli` char(1) NOT NULL DEFAULT 'S',
  `relmovclientes` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecsintFil` char(1) NOT NULL DEFAULT 'S',
  `relcontasrecanalcli` char(1) NOT NULL DEFAULT 'S',
  `rotcontratos` char(1) NOT NULL DEFAULT 'S',
  `relinadimplencia` char(1) NOT NULL DEFAULT 'S',
  `outpatrimonio` char(1) NOT NULL DEFAULT 'S',
  `cliRecBoletoDeposito` char(1) NOT NULL DEFAULT 'S',
  `clilancempdh` char(1) NOT NULL DEFAULT 'S',
  `clilancempch` char(1) NOT NULL DEFAULT 'S',
  `clilanccomprati` char(1) NOT NULL DEFAULT 'S',
  `clitrocach` char(1) NOT NULL DEFAULT 'S',
  `horainiciartrabalho` time NOT NULL DEFAULT '00:01:00',
  `horafinalizartrabalho` time NOT NULL DEFAULT '23:59:00',
  `outfuncionarios` char(1) NOT NULL DEFAULT 'S',
  `ultimasenha` varchar(20) DEFAULT NULL,
  `dataultimasenha` date DEFAULT NULL,
  `logado` char(1) NOT NULL DEFAULT 'N',
  `fabricaacessoestoque` char(1) NOT NULL DEFAULT 'S',
  `fabricafinalizarproducao` char(1) NOT NULL DEFAULT 'S',
  `fabricaentradamateriaprima` char(1) NOT NULL DEFAULT 'S',
  `esthist` char(1) NOT NULL DEFAULT 'S',
  `vendarredondamento` char(1) NOT NULL DEFAULT 'S',
  `outcontpagquitar` char(1) NOT NULL DEFAULT 'S',
  `vendecfprevenda` char(1) NOT NULL DEFAULT 'S',
  `clirenegvalor` char(1) NOT NULL DEFAULT 'N',
  `despesasrel` char(1) NOT NULL DEFAULT 'S',
  `osabrir` char(1) NOT NULL DEFAULT 'S',
  `osfechar` char(1) NOT NULL DEFAULT 'S',
  `osincluirpecas` char(1) NOT NULL DEFAULT 'S',
  `osverdadoscliente` char(1) NOT NULL DEFAULT 'S',
  `osmodificar` char(1) NOT NULL DEFAULT 'S',
  `venditemgeral` char(1) NOT NULL DEFAULT 'S',
  `relauditoria` char(1) NOT NULL DEFAULT 'S',
  `venfinprevendapre` char(1) NOT NULL DEFAULT 'N',
  `vendprevenda` char(1) NOT NULL DEFAULT 'S',
  `mudarenderecoentrega` char(1) NOT NULL DEFAULT 'S',
  `entregaposterior` char(1) NOT NULL DEFAULT 'S',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `fidelizacao` char(1) NOT NULL DEFAULT 'S',
  `relfecfinanceiro` char(1) NOT NULL DEFAULT 'S',
  PRIMARY KEY (`codigo`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `seqsice` */

CREATE TABLE `seqsice` (
  `data` date DEFAULT NULL,
  `sequencia` char(6) DEFAULT NULL,
  `enviada` char(1) DEFAULT NULL,
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `serienf` */

CREATE TABLE `serienf` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `serie` char(3) DEFAULT NULL,
  `descricao` varchar(20) DEFAULT NULL,
  `sequencial` int(8) NOT NULL DEFAULT '0',
  `codigofilial` varchar(5) DEFAULT '00001',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Table structure for table `servicocliente` */

CREATE TABLE `servicocliente` (
  `id` int(7) NOT NULL AUTO_INCREMENT,
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `nomecliente` varchar(50) DEFAULT NULL,
  `codigoservico` varchar(20) DEFAULT NULL,
  `descricaoservico` varchar(50) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `ativo` char(1) NOT NULL DEFAULT 'S',
  `datacancelamento` date DEFAULT NULL,
  `operadorcancelamento` varchar(10) DEFAULT NULL,
  `observacao` text,
  `operador` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Table structure for table `servicodetail` */

CREATE TABLE `servicodetail` (
  `numero` int(11) DEFAULT NULL,
  `quantidade` double(7,3) DEFAULT NULL,
  `codigo` varchar(14) DEFAULT NULL,
  `servico` varchar(50) DEFAULT NULL,
  `tipo` char(1) DEFAULT NULL,
  `valorunitario` decimal(11,2) DEFAULT NULL,
  `subtotal` decimal(11,2) DEFAULT NULL,
  `situacao` int(1) DEFAULT NULL,
  `icms` decimal(6,2) DEFAULT '0.00',
  `tributacao` char(2) DEFAULT NULL,
  `valoricms` decimal(11,2) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `funcionario` varchar(20) DEFAULT NULL,
  `valordesconto` decimal(11,2) DEFAULT '0.00',
  KEY `numero` (`numero`,`servico`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `servicomaster` */

CREATE TABLE `servicomaster` (
  `numero` int(11) NOT NULL DEFAULT '0',
  `tipo` int(1) DEFAULT '0',
  `responsavelnome` varchar(15) DEFAULT NULL,
  `codigocliente` varchar(7) DEFAULT NULL,
  `nomecliente` varchar(50) DEFAULT NULL,
  `dataabertura` date DEFAULT NULL,
  `item` varchar(70) DEFAULT NULL,
  `item2` varchar(70) DEFAULT NULL,
  `item3` varchar(70) DEFAULT NULL,
  `situacao` int(1) DEFAULT '0',
  `vencimento` date DEFAULT NULL,
  `totalpecas` decimal(12,2) DEFAULT '0.00',
  `totalservicos` decimal(12,2) DEFAULT '0.00',
  `descppecas` decimal(12,2) DEFAULT '0.00',
  `descpservico` decimal(12,2) DEFAULT '0.00',
  `descpecas` decimal(12,2) DEFAULT '0.00',
  `descservico` decimal(12,2) DEFAULT '0.00',
  `total` decimal(12,2) DEFAULT '0.00',
  `totalreal` decimal(12,2) DEFAULT '0.00',
  `fpagamento` char(2) DEFAULT NULL,
  `entrada` decimal(12,2) DEFAULT '0.00',
  `pentrada` int(4) DEFAULT '0',
  `porconta` decimal(12,2) DEFAULT '0.00',
  `lancamentoporconta` int(1) DEFAULT '0',
  `parcelas` int(3) DEFAULT '0',
  `valorparcelas` decimal(12,2) DEFAULT '0.00',
  `datafechamento` date DEFAULT NULL,
  `orcamento` int(11) DEFAULT '0',
  `lancado` int(1) DEFAULT '0',
  `desconto` int(1) DEFAULT '0',
  `codigofilial` varchar(5) DEFAULT NULL,
  `documento` int(8) NOT NULL DEFAULT '0',
  `fechadopor` varchar(10) DEFAULT NULL,
  `comissionado` varchar(10) DEFAULT NULL,
  `entrega` varchar(10) DEFAULT 'S',
  `vendedor` char(3) DEFAULT NULL,
  `usuarioip` varchar(20) DEFAULT NULL,
  `ip` varchar(20) DEFAULT NULL,
  `dataentrega` date DEFAULT NULL,
  `placa` varchar(7) DEFAULT NULL,
  PRIMARY KEY (`numero`),
  KEY `vendedor` (`vendedor`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `servicos` */

CREATE TABLE `servicos` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(60) NOT NULL DEFAULT '',
  `codigofilial` varchar(10) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `iss` decimal(6,2) DEFAULT NULL,
  `tributacao` char(2) DEFAULT NULL,
  `codigoservico` varchar(20) DEFAULT NULL,
  `ativo` char(1) NOT NULL DEFAULT 'S',
  PRIMARY KEY (`codigo`,`descricao`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `servicos_original` */

CREATE TABLE `servicos_original` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(60) NOT NULL DEFAULT '',
  `codigofilial` varchar(10) DEFAULT NULL,
  `valor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ativo` char(1) NOT NULL DEFAULT 'S',
  `iss` decimal(6,2) DEFAULT NULL,
  `tributacao` char(2) DEFAULT NULL,
  PRIMARY KEY (`codigo`,`descricao`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `setorcp` */

CREATE TABLE `setorcp` (
  `codigo` char(4) DEFAULT NULL,
  `descricao` char(40) DEFAULT '0',
  `CodigoFilial` char(5) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `situacaocli` */

CREATE TABLE `situacaocli` (
  `codigo` int(3) NOT NULL AUTO_INCREMENT,
  `descricao` char(15) NOT NULL DEFAULT '',
  `restritiva` char(1) DEFAULT NULL,
  `CodigoFilial` char(5) DEFAULT NULL,
  PRIMARY KEY (`descricao`),
  UNIQUE KEY `codigo` (`codigo`),
  KEY `descricao` (`descricao`)
) ENGINE=MyISAM AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

/*Table structure for table `subgrupos` */

CREATE TABLE `subgrupos` (
  `codigogrupo` int(3) unsigned DEFAULT '0',
  `grupo` char(30) NOT NULL DEFAULT '0',
  `codigosubgrupo` int(4) unsigned NOT NULL AUTO_INCREMENT,
  `subgrupo` char(30) DEFAULT '0',
  `CodigoFilial` char(5) DEFAULT NULL,
  PRIMARY KEY (`codigosubgrupo`)
) ENGINE=MyISAM AUTO_INCREMENT=986 DEFAULT CHARSET=latin1;

/*Table structure for table `subsetorcp` */

CREATE TABLE `subsetorcp` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigo` varchar(4) DEFAULT NULL,
  `descricao` varchar(40) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `codigosetor` varchar(4) DEFAULT NULL,
  `descricaosetor` varchar(40) DEFAULT NULL,
  `contadebito` char(1) NOT NULL DEFAULT 'N',
  `contabancaria` varchar(20) NOT NULL DEFAULT '',
  `conta` varchar(5) DEFAULT NULL,
  `descricaoconta` varchar(30) DEFAULT NULL,
  `subconta` varchar(5) DEFAULT NULL,
  `descricaosubconta` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

/*Table structure for table `sugestao` */

CREATE TABLE `sugestao` (
  `id` char(15) DEFAULT NULL,
  `codigofilial` char(5) DEFAULT NULL,
  `codigo` char(20) DEFAULT NULL,
  `produto` char(50) DEFAULT NULL,
  `fornecedor` char(35) DEFAULT NULL,
  `pedido` int(5) NOT NULL DEFAULT '0',
  `saldo` decimal(8,2) NOT NULL DEFAULT '0.00',
  `mediadiaria` decimal(8,2) NOT NULL DEFAULT '0.00',
  `media3meses` decimal(8,2) NOT NULL DEFAULT '0.00',
  `mes` decimal(8,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(8,2) NOT NULL DEFAULT '0.00',
  `qtdultent` decimal(8,2) NOT NULL DEFAULT '0.00',
  `ultentrada` date DEFAULT NULL,
  `ultvenda` date DEFAULT NULL,
  `diferenca` int(5) unsigned NOT NULL DEFAULT '0',
  `pedidoandamento` decimal(8,2) NOT NULL DEFAULT '0.00',
  `grupo` char(30) DEFAULT NULL,
  `subgrupo` year(4) DEFAULT NULL,
  `fabricante` char(35) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `tab_municipios` */

CREATE TABLE `tab_municipios` (
  `id` int(11) NOT NULL DEFAULT '0',
  `iduf` int(11) NOT NULL DEFAULT '0',
  `nome` varchar(55) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `tabmun_nome` (`nome`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `tabelancm` */

CREATE TABLE `tabelancm` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CodigoNcm` varchar(11) DEFAULT NULL,
  `DescricaoNcm` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9151 DEFAULT CHARSET=latin1;

/*Table structure for table `tabella_utente` */

CREATE TABLE `tabella_utente` (
  `id_utente` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `cognome` varchar(255) DEFAULT NULL,
  `username` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `sfida_corrente` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_utente`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `tanque` */

CREATE TABLE `tanque` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `codigoproduto` varchar(20) DEFAULT NULL,
  `capacidade` decimal(10,2) DEFAULT '0.00',
  `quantidade` decimal(10,2) DEFAULT '0.00',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `temp` */

CREATE TABLE `temp` (
  `Codigo` int(5) unsigned NOT NULL DEFAULT '0',
  `parcelas` int(2) DEFAULT NULL,
  `intervalo` int(3) DEFAULT NULL,
  `descricao` varchar(15) DEFAULT NULL,
  `coeficiente` decimal(8,5) DEFAULT NULL,
  `juros` decimal(5,2) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `numero` varchar(20) DEFAULT NULL,
  `cabecalho` varchar(20) DEFAULT NULL,
  `CodClasse` varchar(4) DEFAULT NULL,
  `DescClasse` varchar(20) DEFAULT NULL,
  `classe` varchar(4) DEFAULT NULL,
  `entradaobrigatoria` char(1) NOT NULL DEFAULT 'N',
  `percentualentrada` decimal(5,2) NOT NULL DEFAULT '0.00',
  `tipopagamento` char(2) NOT NULL DEFAULT '00'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `tickets` */

CREATE TABLE `tickets` (
  `codigo` int(5) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(15) DEFAULT NULL,
  `taxaadministracao` decimal(6,2) DEFAULT NULL,
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `tipo` char(2) DEFAULT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `tipocliente` */

CREATE TABLE `tipocliente` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(20) NOT NULL DEFAULT '',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

/*Table structure for table `transportadoras` */

CREATE TABLE `transportadoras` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `razaosocial` varchar(40) DEFAULT NULL,
  `fantasia` varchar(30) DEFAULT NULL,
  `endereco` varchar(50) DEFAULT NULL,
  `bairro` varchar(20) DEFAULT NULL,
  `cidade` varchar(30) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `estado` char(2) DEFAULT 'PE',
  `inscricao` varchar(20) DEFAULT NULL,
  `cnpj` varchar(18) DEFAULT NULL,
  `cpf` varchar(14) DEFAULT NULL,
  `telefone` varchar(15) DEFAULT NULL,
  `fax` varchar(15) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `site` varchar(100) DEFAULT NULL,
  `obs` text,
  `numero` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `transprd` */

CREATE TABLE `transprd` (
  `inc` bigint(20) NOT NULL AUTO_INCREMENT,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `codigodestino` varchar(20) DEFAULT NULL,
  `descricaodestino` varchar(50) DEFAULT NULL,
  `quantidade` decimal(8,2) NOT NULL DEFAULT '0.00',
  `quantidadedestino` decimal(8,2) NOT NULL DEFAULT '0.00',
  `data` date DEFAULT NULL,
  `usuario` varchar(10) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `historico` text,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `unidademedidas` */

CREATE TABLE `unidademedidas` (
  `id` int(5) NOT NULL AUTO_INCREMENT,
  `tipo` varchar(10) NOT NULL DEFAULT 'SI',
  `grandeza` varchar(30) DEFAULT NULL,
  `unidade` varchar(20) DEFAULT NULL,
  `simbolo` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Table structure for table `useronline` */

CREATE TABLE `useronline` (
  `timestamp` int(15) NOT NULL DEFAULT '0',
  `ip` varchar(40) NOT NULL DEFAULT '',
  `file` varchar(100) NOT NULL DEFAULT '',
  PRIMARY KEY (`timestamp`),
  KEY `ip` (`ip`),
  KEY `file` (`file`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `veiculos` */

CREATE TABLE `veiculos` (
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `idtransportadora` int(5) DEFAULT NULL,
  `veiculo` varchar(20) DEFAULT NULL,
  `placa` varchar(8) DEFAULT NULL,
  `motorista` varchar(30) DEFAULT NULL,
  `capacidadeKg` decimal(10,2) DEFAULT NULL,
  `capacidadem3` decimal(10,2) DEFAULT NULL,
  `combustivel` varchar(15) DEFAULT NULL,
  `consumoKm` int(3) NOT NULL DEFAULT '0',
  `ANTT` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `venda` */

CREATE TABLE `venda` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Descontoperc` decimal(6,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `descontovalor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  `documento` int(10) unsigned NOT NULL DEFAULT '0',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `ratdesc` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `rateioencargos` decimal(8,4) NOT NULL DEFAULT '0.0000',
  `situacao` varchar(15) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `Ecfnumero` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `NotaFiscal` varchar(15) DEFAULT NULL,
  `icms` int(2) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '',
  `secao` varchar(20) DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `aentregar` char(1) NOT NULL DEFAULT 'N',
  `quantidadeanterior` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `numerodevolucao` int(7) NOT NULL DEFAULT '0',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `embalagem` int(3) NOT NULL DEFAULT '1',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `serieNF` varchar(3) DEFAULT NULL,
  `cfop` varchar(5) NOT NULL DEFAULT '5.102',
  `acrescimototalitem` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` varchar(2) NOT NULL DEFAULT '01',
  `cstcofins` varchar(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(5,3) NOT NULL DEFAULT '0.000',
  `subserienf` varchar(3) DEFAULT NULL,
  `modelodocfiscal` varchar(2) NOT NULL DEFAULT '2D',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `eaddados` varchar(33) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`inc`),
  KEY `documento` (`documento`)
) ENGINE=MyISAM AUTO_INCREMENT=810 DEFAULT CHARSET=latin1;

/*Table structure for table `vendaarquivo` */

CREATE TABLE `vendaarquivo` (
  `inc` int(10) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Descontoperc` decimal(6,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `descontovalor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  `documento` int(10) unsigned NOT NULL DEFAULT '0',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `ratdesc` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `rateioencargos` decimal(8,4) NOT NULL DEFAULT '0.0000',
  `situacao` varchar(15) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `ecfnumero` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `NotaFiscal` varchar(15) DEFAULT NULL,
  `icms` int(2) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '',
  `secao` varchar(20) DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `aentregar` char(1) NOT NULL DEFAULT 'N',
  `quantidadeanterior` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `numerodevolucao` int(7) NOT NULL DEFAULT '0',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `embalagem` int(3) NOT NULL DEFAULT '1',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `serieNF` char(3) DEFAULT NULL,
  `cfop` varchar(5) NOT NULL DEFAULT '5.102',
  `acrescimototalitem` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` char(2) NOT NULL DEFAULT '01',
  `cstcofins` char(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(5,3) NOT NULL DEFAULT '0.000',
  `subserienf` char(3) DEFAULT NULL,
  `modelodocfiscal` char(2) NOT NULL DEFAULT '2D',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `eaddados` varchar(33) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  KEY `documento` (`documento`),
  KEY `data` (`data`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `vendadav` */

CREATE TABLE `vendadav` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Descontoperc` decimal(6,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `descontovalor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  `documento` int(10) unsigned NOT NULL DEFAULT '0',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `ratdesc` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `rateioencargos` decimal(8,4) NOT NULL DEFAULT '0.0000',
  `situacao` varchar(15) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `Ecfnumero` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `NotaFiscal` varchar(15) DEFAULT NULL,
  `icms` int(2) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '',
  `secao` varchar(20) DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `aentregar` char(1) NOT NULL DEFAULT 'N',
  `quantidadeanterior` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `numerodevolucao` int(7) NOT NULL DEFAULT '0',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `embalagem` int(3) NOT NULL DEFAULT '1',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `serieNF` varchar(3) DEFAULT NULL,
  `cfop` varchar(5) NOT NULL DEFAULT '5.102',
  `acrescimototalitem` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` varchar(2) NOT NULL DEFAULT '01',
  `cstcofins` varchar(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(5,3) NOT NULL DEFAULT '0.000',
  `subserienf` varchar(3) DEFAULT NULL,
  `modelodocfiscal` varchar(2) NOT NULL DEFAULT '2D',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `eaddados` varchar(33) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=474 DEFAULT CHARSET=latin1;

/*Table structure for table `vendadavos` */

CREATE TABLE `vendadavos` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Descontoperc` decimal(6,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `descontovalor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  `documento` int(10) unsigned NOT NULL DEFAULT '0',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `ratdesc` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `rateioencargos` decimal(8,4) NOT NULL DEFAULT '0.0000',
  `situacao` varchar(15) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `Ecfnumero` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `NotaFiscal` varchar(15) DEFAULT NULL,
  `icms` int(2) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '',
  `secao` varchar(20) DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `aentregar` char(1) NOT NULL DEFAULT 'N',
  `quantidadeanterior` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `numerodevolucao` int(7) NOT NULL DEFAULT '0',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `embalagem` int(3) NOT NULL DEFAULT '1',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `serieNF` varchar(3) DEFAULT NULL,
  `cfop` varchar(5) NOT NULL DEFAULT '5.102',
  `acrescimototalitem` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` varchar(2) NOT NULL DEFAULT '01',
  `cstcofins` varchar(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(5,3) NOT NULL DEFAULT '0.000',
  `subserienf` varchar(3) DEFAULT NULL,
  `modelodocfiscal` varchar(2) NOT NULL DEFAULT '2D',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `eaddados` varchar(33) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=259 DEFAULT CHARSET=latin1;

/*Table structure for table `vendaexclusao` */

CREATE TABLE `vendaexclusao` (
  `inc` int(8) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Descontoperc` decimal(6,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `descontovalor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  `documento` int(10) unsigned NOT NULL DEFAULT '0',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `ratdesc` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `rateioencargos` decimal(8,4) NOT NULL DEFAULT '0.0000',
  `situacao` varchar(15) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `Ecfnumero` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `NotaFiscal` varchar(15) DEFAULT NULL,
  `icms` int(2) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '',
  `secao` varchar(20) DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `aentregar` char(1) NOT NULL DEFAULT 'N',
  `quantidadeanterior` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `numerodevolucao` int(7) NOT NULL DEFAULT '0',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `embalagem` int(3) NOT NULL DEFAULT '1',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `serieNF` char(3) DEFAULT NULL,
  `cfop` varchar(5) NOT NULL DEFAULT '5.102',
  `acrescimototalitem` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` char(2) NOT NULL DEFAULT '01',
  `cstcofins` char(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(5,3) NOT NULL DEFAULT '0.000',
  `subserienf` char(3) DEFAULT NULL,
  `modelodocfiscal` char(2) NOT NULL DEFAULT '2D',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `eaddados` varchar(33) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `vendanf` */

CREATE TABLE `vendanf` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Descontoperc` decimal(6,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `descontovalor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  `documento` int(10) unsigned NOT NULL DEFAULT '0',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `ratdesc` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `rateioencargos` decimal(8,4) NOT NULL DEFAULT '0.0000',
  `situacao` varchar(15) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `Ecfnumero` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `NotaFiscal` varchar(15) DEFAULT NULL,
  `icms` int(2) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '',
  `secao` varchar(20) DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `aentregar` char(1) NOT NULL DEFAULT 'N',
  `quantidadeanterior` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `numerodevolucao` int(7) NOT NULL DEFAULT '0',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `embalagem` int(3) NOT NULL DEFAULT '1',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `serieNF` char(3) DEFAULT NULL,
  `cfop` varchar(5) NOT NULL DEFAULT '5.102',
  `acrescimototalitem` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` char(2) NOT NULL DEFAULT '01',
  `cstcofins` char(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(5,3) NOT NULL DEFAULT '0.000',
  `subserienf` char(3) DEFAULT NULL,
  `modelodocfiscal` char(2) NOT NULL DEFAULT '2D',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `eaddados` varchar(33) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

/*Table structure for table `vendapadronizada` */

CREATE TABLE `vendapadronizada` (
  `numero` int(3) DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,2) NOT NULL DEFAULT '0.00'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `vendaprevenda` */

CREATE TABLE `vendaprevenda` (
  `inc` int(8) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Descontoperc` decimal(6,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `descontovalor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  `documento` int(10) unsigned NOT NULL DEFAULT '0',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `ratdesc` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `rateioencargos` decimal(8,4) NOT NULL DEFAULT '0.0000',
  `situacao` varchar(15) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `Ecfnumero` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `NotaFiscal` varchar(15) DEFAULT NULL,
  `icms` int(2) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '',
  `secao` varchar(20) DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `aentregar` char(1) NOT NULL DEFAULT 'N',
  `quantidadeanterior` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `numerodevolucao` int(7) NOT NULL DEFAULT '0',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `embalagem` int(3) NOT NULL DEFAULT '1',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `serieNF` char(3) DEFAULT NULL,
  `cfop` varchar(5) NOT NULL DEFAULT '5.102',
  `acrescimototalitem` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` char(2) NOT NULL DEFAULT '01',
  `cstcofins` char(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(5,3) NOT NULL DEFAULT '0.000',
  `subserienf` char(3) DEFAULT NULL,
  `modelodocfiscal` char(2) NOT NULL DEFAULT '2D',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `eaddados` varchar(33) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `vendaprevendapaf` */

CREATE TABLE `vendaprevendapaf` (
  `inc` int(8) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Descontoperc` decimal(6,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `descontovalor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  `documento` int(10) unsigned NOT NULL DEFAULT '0',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `ratdesc` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `rateioencargos` decimal(8,4) NOT NULL DEFAULT '0.0000',
  `situacao` varchar(15) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `Ecfnumero` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `NotaFiscal` varchar(15) DEFAULT NULL,
  `icms` int(2) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '',
  `secao` varchar(20) DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `aentregar` char(1) NOT NULL DEFAULT 'N',
  `quantidadeanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `numerodevolucao` int(7) NOT NULL DEFAULT '0',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `embalagem` int(3) NOT NULL DEFAULT '1',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `serieNF` varchar(3) DEFAULT NULL,
  `cfop` varchar(5) NOT NULL DEFAULT '5.102',
  `acrescimototalitem` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` varchar(2) NOT NULL DEFAULT '01',
  `cstcofins` varchar(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(5,3) NOT NULL DEFAULT '0.000',
  `subserienf` varchar(3) DEFAULT NULL,
  `modelodocfiscal` varchar(2) NOT NULL DEFAULT '2D',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `eaddados` varchar(33) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=67 DEFAULT CHARSET=latin1;

/*Table structure for table `vendas` */

CREATE TABLE `vendas` (
  `inc` int(10) NOT NULL AUTO_INCREMENT,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Descontoperc` decimal(6,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `descontovalor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  `documento` int(10) unsigned NOT NULL DEFAULT '0',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `ratdesc` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `rateioencargos` decimal(8,4) NOT NULL DEFAULT '0.0000',
  `situacao` varchar(15) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `ecfnumero` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `NotaFiscal` varchar(15) DEFAULT NULL,
  `icms` int(2) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '',
  `secao` varchar(20) DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `aentregar` char(1) NOT NULL DEFAULT 'N',
  `quantidadeanterior` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `numerodevolucao` int(7) NOT NULL DEFAULT '0',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `embalagem` int(3) NOT NULL DEFAULT '1',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `serieNF` char(3) DEFAULT NULL,
  `cfop` varchar(5) NOT NULL DEFAULT '5.102',
  `acrescimototalitem` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` char(2) NOT NULL DEFAULT '01',
  `cstcofins` char(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(5,3) NOT NULL DEFAULT '0.000',
  `subserienf` char(3) DEFAULT NULL,
  `modelodocfiscal` char(2) NOT NULL DEFAULT '2D',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `eaddados` varchar(33) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`inc`),
  KEY `NewIndex` (`codigo`)
) ENGINE=MyISAM AUTO_INCREMENT=3781 DEFAULT CHARSET=latin1;

/*Table structure for table `vendatmp` */

CREATE TABLE `vendatmp` (
  `inc` int(10) DEFAULT NULL,
  `codigofilial` varchar(5) DEFAULT NULL,
  `operador` varchar(10) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `codigo` varchar(20) DEFAULT NULL,
  `produto` varchar(50) DEFAULT NULL,
  `quantidade` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `preco` decimal(10,2) NOT NULL DEFAULT '0.00',
  `custo` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `precooriginal` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Descontoperc` decimal(6,2) NOT NULL DEFAULT '0.00',
  `id` varchar(15) NOT NULL DEFAULT '',
  `descontovalor` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `vendedor` char(3) DEFAULT NULL,
  `nrcontrole` int(5) NOT NULL DEFAULT '0',
  `documento` int(10) unsigned NOT NULL DEFAULT '0',
  `grupo` varchar(30) DEFAULT NULL,
  `subgrupo` varchar(30) DEFAULT NULL,
  `comissao` char(1) NOT NULL DEFAULT 'A',
  `ratdesc` decimal(10,4) NOT NULL DEFAULT '0.0000',
  `rateioencargos` decimal(8,4) NOT NULL DEFAULT '0.0000',
  `situacao` varchar(15) DEFAULT NULL,
  `customedio` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `ecfnumero` char(3) DEFAULT NULL,
  `fornecedor` varchar(30) DEFAULT NULL,
  `fabricante` varchar(30) DEFAULT NULL,
  `NotaFiscal` varchar(15) DEFAULT NULL,
  `icms` int(2) NOT NULL DEFAULT '0',
  `classe` varchar(4) NOT NULL DEFAULT '',
  `secao` varchar(20) DEFAULT NULL,
  `lote` varchar(15) NOT NULL DEFAULT '',
  `tributacao` char(3) NOT NULL DEFAULT '00',
  `aentregar` char(1) NOT NULL DEFAULT 'N',
  `quantidadeanterior` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `quantidadeatualizada` decimal(14,5) NOT NULL DEFAULT '0.00000',
  `codigofiscal` char(3) NOT NULL DEFAULT '000',
  `customedioanterior` decimal(10,5) NOT NULL DEFAULT '0.00000',
  `codigocliente` int(6) NOT NULL DEFAULT '0',
  `numerodevolucao` int(7) NOT NULL DEFAULT '0',
  `codigobarras` varchar(20) NOT NULL DEFAULT '',
  `ipi` int(2) NOT NULL DEFAULT '0',
  `unidade` char(3) NOT NULL DEFAULT 'UNI',
  `embalagem` int(3) NOT NULL DEFAULT '1',
  `grade` varchar(10) NOT NULL DEFAULT 'nenhuma',
  `romaneio` char(1) NOT NULL DEFAULT 'S',
  `tipo` varchar(15) NOT NULL DEFAULT '0 - Produto',
  `cofins` decimal(5,2) NOT NULL DEFAULT '0.00',
  `pis` decimal(5,2) NOT NULL DEFAULT '0.00',
  `despesasacessorias` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMS` decimal(5,2) NOT NULL DEFAULT '0.00',
  `serieNF` char(3) DEFAULT NULL,
  `cfop` varchar(5) NOT NULL DEFAULT '5.102',
  `acrescimototalitem` decimal(8,2) NOT NULL DEFAULT '0.00',
  `cstpis` char(2) NOT NULL DEFAULT '01',
  `cstcofins` char(2) NOT NULL DEFAULT '01',
  `icmsst` decimal(5,2) NOT NULL DEFAULT '0.00',
  `percentualRedBaseCalcICMSST` decimal(5,2) NOT NULL DEFAULT '0.00',
  `mvast` decimal(5,3) NOT NULL DEFAULT '0.000',
  `subserienf` char(3) DEFAULT NULL,
  `modelodocfiscal` char(2) NOT NULL DEFAULT '2D',
  `aliquotaIPI` decimal(5,2) DEFAULT '0.00',
  `ecffabricacao` varchar(20) DEFAULT NULL,
  `coo` varchar(6) DEFAULT NULL,
  `cancelado` char(1) NOT NULL DEFAULT 'N',
  `eaddados` varchar(33) DEFAULT NULL,
  `ccf` varchar(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Table structure for table `vendedores` */

CREATE TABLE `vendedores` (
  `id` int(4) NOT NULL AUTO_INCREMENT,
  `codigo` char(3) DEFAULT NULL,
  `nome` varchar(30) DEFAULT NULL,
  `endereco` varchar(30) DEFAULT NULL,
  `bairro` varchar(20) DEFAULT NULL,
  `cidade` varchar(20) DEFAULT NULL,
  `cep` varchar(10) DEFAULT NULL,
  `estado` char(2) DEFAULT NULL,
  `cotamensal` decimal(10,2) DEFAULT NULL,
  `cotaqtdmensal` decimal(10,2) DEFAULT NULL,
  `a` decimal(6,2) NOT NULL DEFAULT '0.00',
  `b` decimal(6,2) NOT NULL DEFAULT '0.00',
  `c` decimal(6,2) NOT NULL DEFAULT '0.00',
  `d` decimal(6,2) NOT NULL DEFAULT '0.00',
  `e` decimal(6,2) NOT NULL DEFAULT '0.00',
  `f` decimal(6,2) NOT NULL DEFAULT '0.00',
  `g` decimal(6,2) NOT NULL DEFAULT '0.00',
  `h` decimal(6,2) NOT NULL DEFAULT '0.00',
  `i` decimal(6,2) NOT NULL DEFAULT '0.00',
  `j` decimal(6,2) NOT NULL DEFAULT '0.00',
  `CodigoFilial` varchar(5) DEFAULT NULL,
  `telefone` varchar(15) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `dinheiro` decimal(6,2) NOT NULL DEFAULT '0.00',
  `cartao` decimal(6,2) NOT NULL DEFAULT '0.00',
  `cheque` decimal(6,2) NOT NULL DEFAULT '0.00',
  `crediario` decimal(6,2) NOT NULL DEFAULT '0.00',
  `devolucaorec` decimal(6,2) NOT NULL DEFAULT '0.00',
  `devolucaovenda` decimal(6,2) NOT NULL DEFAULT '0.00',
  `recebimento` decimal(6,2) NOT NULL DEFAULT '0.00',
  `cpf` varchar(11) DEFAULT NULL,
  `cnpj` varchar(14) DEFAULT NULL,
  `rg` varchar(15) DEFAULT NULL,
  `telefone1` varchar(15) DEFAULT NULL,
  `telefone2` varchar(15) DEFAULT NULL,
  `email` varchar(60) DEFAULT NULL,
  `observacao` text,
  `inscricao` varchar(20) DEFAULT NULL,
  `ativo` char(1) NOT NULL DEFAULT 'S',
  `senha` varchar(20) DEFAULT NULL,
  `financeira` decimal(6,2) NOT NULL DEFAULT '0.00',
  `categoria` enum('Vendedor','Distribuidor','Indicador') DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `nome` (`nome`)
) ENGINE=MyISAM AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

/*Table structure for table `versao` */

CREATE TABLE `versao` (
  `versao` char(10) DEFAULT '1.0',
  `dataatualizacao` datetime DEFAULT NULL,
  `ipatualizacao` char(15) DEFAULT NULL,
  `inc` int(5) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

/*Table structure for table `web_casos_sucesso` */

CREATE TABLE `web_casos_sucesso` (
  `id` int(4) NOT NULL AUTO_INCREMENT,
  `empresa` varchar(60) NOT NULL DEFAULT '',
  `caso` text NOT NULL,
  `chamada` text NOT NULL,
  `img_chamada` varchar(60) DEFAULT NULL,
  `data` date NOT NULL DEFAULT '0000-00-00',
  `autor` varchar(30) DEFAULT NULL,
  `hora` time DEFAULT '00:00:00',
  `ver` char(1) NOT NULL DEFAULT 'N',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `web_downloads` */

CREATE TABLE `web_downloads` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `caminho` varchar(60) DEFAULT NULL,
  `arquivo` varchar(60) NOT NULL DEFAULT '',
  `descricao` text,
  `versao` varchar(10) DEFAULT NULL,
  `empresa` varchar(30) DEFAULT NULL,
  `os` varchar(10) DEFAULT NULL,
  `data` date NOT NULL DEFAULT '0000-00-00',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `web_noticias` */

CREATE TABLE `web_noticias` (
  `id` int(4) NOT NULL AUTO_INCREMENT,
  `titulo` varchar(100) NOT NULL DEFAULT '',
  `noticia` text NOT NULL,
  `chamada` text NOT NULL,
  `data` date NOT NULL DEFAULT '0000-00-00',
  `autor` varchar(30) DEFAULT NULL,
  `hora` time DEFAULT '00:00:00',
  `ver` char(1) NOT NULL DEFAULT 'N',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `web_paginas_genericas` */

CREATE TABLE `web_paginas_genericas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `chave_pagina` varchar(20) DEFAULT NULL,
  `titulo_pagina` varchar(40) DEFAULT NULL,
  `texto_pagina` text,
  `include_pagina` varchar(40) DEFAULT NULL,
  `data_pagina` date DEFAULT NULL,
  `autor_pagina` varchar(30) DEFAULT NULL,
  `botao_flash` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `web_produtos_atualizacoes` */

CREATE TABLE `web_produtos_atualizacoes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `produto` varchar(60) NOT NULL DEFAULT '',
  `descricao` text,
  `versao` varchar(10) DEFAULT NULL,
  `caminho` varchar(60) DEFAULT NULL,
  `data` date DEFAULT '0000-00-00',
  `hora` time DEFAULT '00:00:00',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/*Table structure for table `web_tutoriais` */

CREATE TABLE `web_tutoriais` (
  `id` int(4) NOT NULL AUTO_INCREMENT,
  `titulo` varchar(100) NOT NULL DEFAULT '',
  `tutorial` text NOT NULL,
  `chamada` text NOT NULL,
  `data` date NOT NULL DEFAULT '0000-00-00',
  `autor` varchar(30) DEFAULT NULL,
  `hora` time DEFAULT '00:00:00',
  `ver` char(1) NOT NULL DEFAULT 'N',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

/* Procedure structure for procedure `AjustarCamposNulos` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AjustarCamposNulos`()
BEGIN
UPDATE produtos SET unidade="" WHERE unidade IS NULL;
UPDATE produtos SET ncm="" WHERE ncm IS NULL;
UPDATE entradas,moventradas SET entradas.dataentrada = moventradas.dataEntrada
WHERE entradas.numero = moventradas.numero;
UPDATE fornecedores SET cpf="" WHERE cpf IS NULL;
UPDATE fornecedores SET cgc="" WHERE CGC IS NULL;
UPDATE fornecedores SET telefone="" WHERE telefone IS NULL;
UPDATE fornecedores SET fax="" WHERE fax IS NULL;
UPDATE fornecedores SET email="" WHERE email IS NULL;
UPDATE fornecedores SET inscricao="" WHERE inscricao IS NULL;
UPDATE fornecedores SET cep="" WHERE cep IS NULL;
UPDATE entradas,moventradas SET entradas.modeloNF=moventradas.modeloNF
WHERE entradas.numero=moventradas.numero;
UPDATE moventradas SET DataEmissao=dataEntrada WHERE dataemissao>dataentrada;
UPDATE vendanf SET serienf="1" WHERE serienf IS NULL;
UPDATE produtos SET ncm="" WHERE ncm IS NULL;
UPDATE produtosfilial SET ncm="" WHERE ncm IS NULL;
UPDATE produtos SET nbm="" WHERE nbm IS NULL;
UPDATE produtosfilial SET nbm="" WHERE nbm IS NULL;
UPDATE produtos SET ncmespecie="" WHERE ncmespecie IS NULL;
UPDATE produtosfilial SET ncmespecie="" WHERE ncmespecie IS NULL;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarContRelGerencial` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AtualizarContRelGerencial`(in cooRG varchar(6))
BEGIN
 UPDATE contrelatoriogerencial
SET contrelatoriogerencial.EADDados= MD5(CONCAT( ecffabricacao,coo,gnf, denominacao,DATA,cdc,denominacao))
where coo=cooRG;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarDadosOff` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AtualizarDadosOff`()
BEGIN
 UPDATE contdocs SET concluido='S',
 EADr06=MD5(CONCAT(ecffabricacao,ncupomfiscal,contadornaofiscalGNF,contadordebitocreditoCDC,DATA,coognf,tipopagamento )),
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,davnumero,DATA,total))
 WHERE historico="OFF";
 
 update contdocs,venda
 set venda.coo = contdocs.ncupomfiscal,
 venda.ccf = contdocs.ecfcontadorcupomfiscal,
 venda.Ecfnumero = contdocs.ecfnumero,
 venda.ecffabricacao = contdocs.ecffabricacao
 where venda.documento = contdocs.documento
 and contdocs.historico="OFF";
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarDebitoCliente` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AtualizarDebitoCliente`(in codCliente int,in taxaJuros double,in filial varchar(5))
BEGIN
 IF (codCliente>0) THEN
 UPDATE crmovclientes set datacalcjuros=vencimento,
 diasdecorrido=to_days(current_date)-to_days(datacalcjuros),
 vrjuros = 0,
 valorcorrigido = valoratual 
 WHERE codigo=codCliente
 AND valoratual >0 AND datacalcjuros<=vencimento;
 UPDATE crmovclientes SET jurosacumulado=0 
 WHERE codigo =codCliente
 AND valoratual >0 AND jurosacumulado<0;
 UPDATE crmovclientes SET diasdecorrido = to_days(current_date)-to_days(datacalcjuros),
 vrjuros = 0,
 vrjuros = taxaJuros*(to_days(current_date)-to_days(datacalcjuros))*valoratual/100
 ,valorcorrigido = valoratual+vrjuros 
 WHERE codigo =codCliente
 AND to_days(current_date)-to_days(vencimento)>(select fatnrdias from configfinanc where codigofilial=filial limit 1)
 AND valoratual > 0
 AND vencimento<current_date ;
 
 UPDATE clientes SET debito=(SELECT IFNULL(SUM(valor),0) FROM crmovclientes WHERE crmovclientes.codigo=clientes.Codigo),saldo=credito-debito
 WHERE codigo=codCliente;
 UPDATE clientes SET debitoch=(SELECT IFNULL(SUM(valor),0) FROM cheques WHERE codigocliente=codCliente AND repassado="N" and depositado="N")
 WHERE codigo=codCliente; 
  
 END IF;
 IF (codCliente=0) THEN
 UPDATE crmovclientes set datacalcjuros=vencimento,
 diasdecorrido=to_days(current_date)-to_days(datacalcjuros),
 vrjuros = 0,
 valorcorrigido = valoratual 
 WHERE valoratual >0 AND datacalcjuros<=vencimento;
 END IF;
 END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarEstoqueOff` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AtualizarEstoqueOff`(in filial varchar(5))
BEGIN
 IF (filial="00001") THEN
 UPDATE produtos,contdocs,venda
 SET produtos.quantidade=produtos.quantidade-(select sum(venda.quantidade) from venda where codigo=produtos.codigo and documento=contdocs.documento ),
 contdocs.estoqueatualizado="S"
 WHERE produtos.codigo=venda.codigo 
 and venda.documento=contdocs.documento
 and contdocs.estoqueatualizado="N"
 AND produtos.codigo=venda.codigo
 AND produtos.codigofilial=filial;
 END IF;
 IF (filial<>"00001") THEN
 UPDATE produtosfilial,contdocs,venda
 SET produtosfilial.quantidade=produtosfilial.quantidade-(select sum(venda.quantidade) from venda where codigo=produtosfilial.codigo and documento=contdocs.documento ),
 contdocs.estoqueatualizado="S"
 WHERE produtosfilial.codigo=venda.codigo 
 and venda.documento=contdocs.documento
 and contdocs.estoqueatualizado="N"
 AND produtosfilial.codigo=venda.codigo
 AND produtosfilial.codigofilial=filial;
 END IF;
 END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarQdtRegistros` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AtualizarQdtRegistros`()
BEGIN
UPDATE quantidaderegistros SET 
  contdav = (SELECT MD5(COUNT(*)) FROM contdav), 
  vendadav = (SELECT MD5(COUNT(*)) FROM vendadav), 
  contprevendaspaf = (SELECT MD5(COUNT(*)) FROM contprevendaspaf), 
  vendaprevendapaf = (SELECT MD5(COUNT(*)) FROM vendaprevendapaf), 
  produtos = (SELECT MD5(COUNT(*)) FROM produtos),
  contdocs = (SELECT MD5(COUNT(*)) FROM contdocs),
  vendaarquivo = (SELECT MD5(COUNT(*)) FROM vendaarquivo),
  caixaarquivo = (SELECT MD5(COUNT(*)) FROM caixaarquivo),
  contrelatoriogerencial = (SELECT MD5(COUNT(*)) FROM contrelatoriogerencial);
    END */$$
DELIMITER ;

/* Procedure structure for procedure `CriarTabelasTemp` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `CriarTabelasTemp`(IN tabela VARCHAR(10),IN filial VARCHAR(5),IN dataInicial DATE,IN dataFinal DATE)
BEGIN
IF (tabela="venda") THEN
DROP TABLE IF EXISTS `vendatmp`;
CREATE TABLE `vendatmp` SELECT * FROM `vendaarquivo` WHERE codigofilial=filial AND DATA BETWEEN dataInicial AND datafinal;
END IF;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `DescontoMaximo` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `DescontoMaximo`(IN acao VARCHAR(10), IN filial VARCHAR(5), IN enderecoip VARCHAR(15))
BEGIN
IF (acao='desconto') THEN
IF (filial='00001') THEN
UPDATE vendas,produtos 
set vendas.Descontoperc=produtos.descontomaximo,
vendas.precooriginal=produtos.precovenda,
vendas.descontovalor=TRUNCATE(produtos.precovenda*(produtos.descontomaximo/100),2)*vendas.quantidade,
vendas.preco=vendas.precooriginal-TRUNCATE(produtos.precovenda*(produtos.descontomaximo/100),2),
vendas.total=vendas.quantidade*(vendas.precooriginal-TRUNCATE(produtos.precovenda*(produtos.descontomaximo/100),2))
WHERE produtos.CodigoFilial=filial
and vendas.codigo=produtos.codigo
AND vendas.id=enderecoip;
END IF;
IF (filial<>'00001') THEN
UPDATE vendas,produtosfilial
set vendas.Descontoperc=produtosfilial.descontomaximo,
vendas.precooriginal=produtosfilial.precovenda,
vendas.descontovalor=TRUNCATE(produtosfilial.precovenda*(produtosfilial.descontomaximo/100),2)*vendas.quantidade,
vendas.preco=vendas.precooriginal-TRUNCATE(produtosfilial.precovenda*(produtosfilial.descontomaximo/100),2),
vendas.total=vendas.quantidade*(vendas.precooriginal-TRUNCATE(produtosfilial.precovenda*(produtosfilial.descontomaximo/100),2))
WHERE produtosfilial.CodigoFilial=filial
AND vendas.codigo=produtosfilial.codigo
AND vendas.id=enderecoip;
END IF;
END IF;
IF (acao='retirar') THEN
UPDATE vendas SET preco=precooriginal,descontoperc=0,descontovalor=0,
total=quantidade*precooriginal 
WHERE id=enderecoip;
END IF;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `EncerrarCaixa` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `EncerrarCaixa`(in idOperador varchar(10),in filial varchar(5),in ipTerminal varchar(15),in nrFabricaoECF varchar(20)  )
BEGIN
 DECLARE totalRecBL decimal default 0;
 DECLARE totalRecDC decimal default 0;
 DECLARE SomaRecBL CURSOR for select IFNULL( sum(valor) ,0) from caixa where operador=idOperador
 AND tipopagamento="BL"
 AND dpfinanceiro like ("Receb%")
 AND codigofilial=filial;
 DECLARE SomaRecDC CURSOR for select IFNULL( sum(valor) ,0) from caixa where operador=idOperador
 AND tipopagamento="DC"
 AND dpfinanceiro like ("Receb%")
 AND codigofilial=filial;
 OPEN SomaRecBL;
 OPEN SomaRecDC;
 FETCH SomaRecBL INTO totalRecBL;
 FETCH SomaRecDC INTO totalRecDC;
 INSERT INTO vendaarquivo SELECT * FROM venda 
 WHERE codigofilial=codigoFilial
 AND operador=idOperador;
 DELETE FROM venda
 WHERE codigofilial=codigoFilial
 AND operador=idOperador;
 INSERT INTO caixaarquivo SELECT * FROM caixa
 WHERE codigofilial=codigoFilial
 AND operador=idOperador;
 DELETE FROM caixa
 WHERE codigofilial=codigoFilial
 AND operador=idOperador;
 UPDATE movdespesas set encerrado='S'
 where operador=idOperador
 and codigofilial=filial	
 and sangria='S';
 UPDATE movreceitas set encerrado='S'
 WHERE operador=idOperador
 AND codigofilial=filial;	
 IF (totalRecBL>0) THEN
 INSERT INTO  movcontasbanco(conta, movimento, valorcredito, data, historico, codigofilial,operador)
 VALUES ((SELECT conta from filiais where codigofilial=filial),"credito",totalRecBL,current_date,"Crédito Bloqueto",
 codigofilial,operador);
 INSERT INTO  movcontasbanco(conta, movimento, valorcredito, data, historico, codigofilial,operador)
 VALUES ((SELECT conta from filiais where codigofilial=filial),"debito",
 (SELECT IFNULL(sum(valortarifabloquete),0) from caixa where codigofilial=filial and operador=idOperador),
 current_date,"Tarifação recebimento bloquete "+filial,
 codigofilial,operador);
 INSERT INTO movdespesas (id,grupo,codigofilial,data,valor,conta,
 subconta,despesa,operador,historico,descricaoconta,descricaosubconta,
 sangria,contabancaria)
 VALUES (ipLocal,"1",filial,current_date,
 (SELECT IFNUL(sum(valortarifabloquete),0) from caixa where codigofilial=filial and operador=idOperador),
 (SELECT contadespesa from filiais where codigofilial=filial),
 (SELECT subconta from filiais where codigofilial=filial),
 "S",idOperador,"Tarifação da taxa de recebimento dos bloquetes",
 (SELECT descricaoconta from filiais where codigofilial=filial),
 (SELECT descricaosubconta from filiais where codigofilial=filial),
 "N",
 (SELECT conta from filiais where codigofilial=filial));
 END IF;
 IF (totalRecDC>0) THEN
 INSERT INTO  movcontasbanco(conta, movimento, valorcredito, data, historico, codigofilial,operador)
 VALUES ((SELECT conta from filiais where codigofilial=filial),"credito",
 (SELECT IFNULL( sum(valor),0) from caixa where codigofilial=filial and operador=idOperador and tipopagamento="DC"),
 current_date,"Recebimento: Depósito Conta corrente",codigofilial,operador);
 END IF;
 UPDATE produtos SET datafinalestoque=current_date,horafinalestoque=current_time,ecffabricacao=nrFabricaoECF;
 UPDATE produtosfilial set datafinalestoque=CURRENT_DATE,horafinalestoque=CURRENT_TIME,ecffabricacao=nrFabricaoECF;
UPDATE produtos SET saldofinalestoque=quantidade,EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,datafinalestoque,horafinalestoque,ecffabricacao));
UPDATE produtosfilial SET saldofinalestoque=quantidade,EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,datafinalestoque,horafinalestoque,ecffabricacao)); 
 call FechamentoDiario(filial);
 
 CALL AtualizarQdtRegistros();	
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `EstornarDevolucao` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `EstornarDevolucao`(in doc int,in filial varchar(5), in ipTerminal varchar(15),in devolucaoNR int, in operadorAcao varchar(10))
BEGIN
 set @tabelaProduto='produtosfilial';
 if (filial='00001') THEN
 set @tabelaProduto='produtos';
 END IF;

 set @qDevolucao= CONCAT('update ',@tabelaProduto,',devolucao set '
 ,@tabelaProduto,'.qtdanterior=',@tabelaProduto,'.quantidade,'
 ,@tabelaProduto,'.quantidade=',@tabelaProduto,'.quantidade-devolucao.quantidade,'
 ,@tabelaProduto,'.qtdprateleiras=',@tabelaProduto,'.qtdprateleiras-devolucao.prateleiras,
 devolucao.situacao=',@tabelaProduto,'.situacao,
 devolucao.dpfinanceiro=','"','Vendas','",',
 @tabelaProduto,'.customedioanterior=',@tabelaProduto,'.customedio,',
 @tabelaProduto,'.customedio=( ( (devolucao.quantidade*devolucao.custo)+( (',@tabelaProduto,'.quantidade+',@tabelaProduto,
 '.qtdretida)*',@tabelaProduto,'.custo) ) / (',@tabelaProduto,'.quantidade+',@tabelaProduto,
 '.qtdretida+devolucao.quantidade) ) 		
 where devolucao.codigo=',@tabelaProduto,'.codigo and devolucao.numero=',devolucaoNR,
 ' and ',@tabelaProduto,'.codigofilial=',filial);
 PREPARE st FROM @qDevolucao;
 EXECUTE st;

 INSERT INTO venda (codigofilial,id,operador,codigo,produto,quantidade,preco,
 precooriginal,customedio,total,vendedor,comissao,data,documento,custo,
 grupo,subgrupo,situacao,numerodevolucao,grade) 
 SELECT filial,ipTerminal,usuario,
 codigo,produto,quantidade*-1,precooriginal*-1,preco*-1,customedio*-1,
 total*-1,vendedor,comissao,current_date,doc,
 custo*-1,grupo,subgrupo,situacao,devolucaoNR,grade
 FROM 	devolucao where numero=devolucaoNR
 AND	finalizada='N';
 UPDATE	contdocs SET observacao=(select observacao from devolucao where numero=devolucaoNR AND observacao<>'') 
 WHERE	documento=doc;
 UPDATE devolucao set finalizada='N',data=current_date where numero=devolucaoNR;
 UPDATE contdevolucao set finalizada='N',data=current_date where numero=devolucaoNR;
 
IF (doc=0) THEN
INSERT INTO caixa (enderecoip,codigofilial,valor,data,dataexe,tipopagamento,operador,vendedor,dpfinanceiro)
values (ipTerminal,filial,(select sum(preco*quantidade)*-1 from devolucao where numero=devolucaoNR),current_date,current_date,"DV",operadorAcao,
(select vendedor from devolucao where numero=devolucaoNR limit 1),"Venda");
END IF;

 UPDATE  produtosgrade,devolucao SET 
 produtosgrade.quantidade=produtosgrade.quantidade-devolucao.quantidade 
 WHERE 	devolucao.numero=devolucaoNR
 AND 	produtosgrade.codigo=devolucao.codigo
 AND 	produtosgrade.grade=devolucao.grade 
 AND 	produtosgrade.codigofilial=filial;

DELETE FROM venda where numerodevolucao=devolucaoNR;


    END */$$
DELIMITER ;

/* Procedure structure for procedure `EstornarQuitacao` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `EstornarQuitacao`(in docEstorno int,in codCliente int,in ipTerminal varchar(15),in filial varchar(5),in operadorEstorno varchar(10))
BEGIN
 DELETE FROM caixas where enderecoip=ipTerminal;
 INSERT INTO contdocs (ip,codigofilial,data,dataexe,totalbruto,desconto,vrjuros,encargos,total,nome,
 codigocliente,NrParcelas,vendedor,operador,observacao,classe,
 historico,dpfinanceiro,tipopagamento,devolucaorecebimento,estornado,hora,concluido)	
 SELECT  ipTerminal,filial,current_date,current_date,totalbruto*-1,desconto*-1,vrjuros*-1,encargos*-1,total*-1,nome,
 codigocliente,NrParcelas,vendedor,operadorEstorno,observacao,classe,
 CONCAT("Rec est ",docEstorno),"Recebimento est",tipopagamento,devolucaorecebimento,estornado,current_time,"S"
 FROM contdocs where documento=docEstorno;
 INSERT INTO  caixa (codigofilial,enderecoIP,nome,codigocliente,valor,dataexe,data,vencimento,
 tipopagamento,banco,agencia,cheque,valorcheque,
 nomecheque,cartao,numerocartao,operador,dpfinanceiro,historico,documento, 
 vrjuros,vrdesconto,encargos) 
 SELECT filial,ipTerminal,nome,codigocliente,valor*-1,current_date,current_date,vencimento,
 tipopagamento,banco,agencia,cheque,valorcheque*-1,nomecheque,cartao,numerocartao,
 operadorEstorno,"Recebimento est",operadorEstorno,(select max(documento) from contdocs where ip=ipTerminal),
 vrjuros*-1,vrdesconto*-1,encargos*-1
 FROM CAIXA WHERE documento=docEstorno;
 IF ( (select tipopagamento from contdocs where documento=docEstorno)="RN") THEN
 UPDATE crmovclientes 
 SET valoratual=valor 
 WHERE sequencia REGEXP CONCAT("s",docEstorno,"s")
 AND codigo = codCliente;
 DELETE FROM crmovclientes 
 WHERE documento = docEstorno
 AND documento<>0
 AND codigo = codCliente;
 END IF;

 IF ( (select tipopagamento from contdocs where documento=docEstorno)<>"RN") THEN	
 UPDATE crmovclientes
 SET datacalcjuros=datapagamento,
 valoratual=ultvaloratual,
 jurosacumulado=ultjurosacumulado,
 vrcapitalrec=ultcaprec,
 jurospago=ultjurospago,
 porconta=porconta-vrultpagamento,
 vrultpagamento=ultporconta,
 datacalcjuros=ultvencimento
 WHERE sequencia regexp CONCAT("s",docEstorno,"s")
 AND codigo = codCliente;
 END IF;
 DELETE FROM cheques where documento=docEstorno;
 DELETE FROM movcartoes where documento=docEstorno;
 UPDATE contdocs SET estornado="S" where documento=docEstorno;

call EstornarDevolucao(docEstorno,filial,ipTerminal,(select contdocs.devolucaonumero from contdocs where contdocs.documento=docEstorno),operadorEstorno);

 END */$$
DELIMITER ;

/* Procedure structure for procedure `ExcluirDocumento` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ExcluirDocumento`(in ipTerminal varchar(15), IN nrDocumento INT,IN filial VARCHAR(5),IN operador VARCHAR(10),IN cooECF VARCHAR(6), in ccfECF varchar(6) )
BEGIN
 SET @tabelaProduto='produtosfilial';
 IF (filial='00001') THEN
 SET @tabelaProduto='produtos';
 END IF;
 INSERT INTO contdocs 
 (ip,codigofilial,DATA,dataexe,totalbruto,desconto,vrjuros,encargos,total,nome,
 codigocliente,NrParcelas,vendedor,operador,observacao,classe,
 historico,dpfinanceiro,tipopagamento,id,custos,
 devolucaovenda,devolucaorecebimento,valorservicos,descontoservicos,hora,estornado,concluido,ncupomfiscal,ecfcontadorcupomfiscal,ecffabricacao,ecfmarca,ecfmodelo,ecfnumero,ecfMFadicional,
 contadordebitocreditoCDC,contadornaofiscalGNF,COOGNF ) 
 SELECT ipTerminal,codigofilial,DATA,dataexe,totalbruto*-1,desconto*-1,vrjuros*-1,encargos*-1,total*-1,nome,
 codigocliente,NrParcelas,vendedor,operador,observacao,classe,
 "Venda est",dpfinanceiro,tipopagamento,id,custos*-1,
 devolucaovenda*-1,devolucaorecebimento*-1,valorservicos*-1,descontoservicos*-1,CURRENT_TIME,"S","S",cooECF,ccfECF,ecffabricacao, ecfmarca,ecfmodelo,ecfnumero,"1",contadordebitocreditoCDC,contadornaofiscalGNF,COOGNF  
 FROM contdocs WHERE documento=nrDocumento;
 SET @RetornaPrd = CONCAT ('UPDATE ',@tabelaProduto,',venda 
 set ',@tabelaProduto,'.qtdanterior=',@tabelaProduto,'.quantidade,',
 @tabelaProduto,'.quantidade=',@tabelaProduto,'.quantidade+(SELECT IFNULL(SUM(venda.quantidade),0) from venda where venda.codigo=',@tabelaProduto,'.codigo and venda.documento=','"',nrDocumento,'" ),',		
 @tabelaProduto,'.qtdprateleiras=',@tabelaProduto,'.qtdprateleiras+(SELECT IFNULL(SUM(venda.quantidade),0) from venda where venda.codigo=',@tabelaProduto,'.codigo and venda.documento=','"',nrDocumento,'"), ',
 @tabelaProduto,'.documento=',nrDocumento,'  
 where venda.codigo=',@tabelaProduto,'.codigo
 AND ',@tabelaProduto,'.documento<>',nrDocumento,' 
 AND ',@tabelaProduto,'.codigofilial=venda.codigofilial  
 AND venda.documento=','"',nrDocumento,'"');
 PREPARE st FROM @RetornaPrd;
 EXECUTE st;
 SET @Entrega = CONCAT ('UPDATE ',@tabelaProduto,',itensaentregar
 SET ',
 @tabelaProduto,'.qtdaentregar=',@tabelaProduto,'.qtdaentregar-(SELECT IFNULL(SUM(itensaentregar.quantidade),0) from itensaentregar where itensaentregar.codigo=',@tabelaProduto,'.codigo and itensaentregar.documento=','"',nrDocumento,'" ) ',
 ' where itensaentregar.codigo=',@tabelaProduto,'.codigo 		
 AND ',@tabelaProduto,'.codigofilial=','"',filial,'"', '
 AND ',@tabelaProduto,'.codigofilial=itensaentregar.codigofilial  
 AND itensaentregar.documento=','"',nrDocumento,'"');
 PREPARE st FROM @Entrega;
 EXECUTE st;
 DELETE FROM itensaentregar WHERE documento=nrDocumento;
 UPDATE produtosgrade,venda
 SET produtosgrade.quantidade=produtosgrade.quantidade+(SELECT SUM(venda.quantidade) FROM venda WHERE venda.documento=nrdocumento AND produtosgrade.codigo=venda.codigo AND venda.grade=produtosgrade.grade)
 WHERE produtosgrade.codigo=venda.codigo
 AND produtosgrade.grade=venda.grade
 AND venda.documento=nrDocumento
 AND produtosgrade.codigofilial=filial;
 UPDATE vendas SET cancelado="S" WHERE documento=nrDocumento;
 UPDATE venda SET cancelado="S" WHERE documento=nrDocumento;
 UPDATE venda SET cancelado="S" WHERE coo=cooECF;
  
 INSERT INTO vendaexclusao SELECT * FROM venda WHERE documento=nrDocumento; 
 INSERT INTO caixa (codigofilial,enderecoIP,nome,codigocliente,valor,dataexe,DATA,vencimento,
 tipopagamento,banco,agencia,cheque,valorcheque,
 nomecheque,cartao,numerocartao,operador,dpfinanceiro,historico,documento, 
 vrjuros,vrdesconto,encargos,custos,vendedor)
 SELECT codigofilial,enderecoIP,nome,codigocliente,valor*-1,dataexe,DATA,vencimento,
 tipopagamento,banco,agencia,cheque,valorcheque*-1,
 nomecheque,cartao,numerocartao,operador,dpfinanceiro,historico,(SELECT MAX(documento) FROM contdocs), 
 vrjuros*-1,vrdesconto*-1,encargos*-1,custos*-1,vendedor 
 FROM caixa WHERE documento=nrDocumento;	
 DELETE FROM crmovclientes WHERE documento=nrDocumento 
 AND documento<>0;
 DELETE FROM cheques WHERE documento=nrDocumento;
 DELETE FROM movcartoes WHERE documento=nrDocumento;
 INSERT INTO auditoria (codigofilial,usuario,hora,DATA,tabela,acao,documento,LOCAL) 
 VALUES (
 filial,operador,CURRENT_TIME,CURRENT_DATE,'Venda','Estorno',nrDocumento,
 (SELECT nome FROM contdocs WHERE documento=nrDocumento LIMIT 1)
 );
 UPDATE clientes 
 SET valorcartaofidelidade=valorcartaofidelidade+(SELECT total FROM contdocs WHERE documento=nrDocumento)
 WHERE codigo=(SELECT cartaofidelidade FROM contdocs WHERE documento=nrDocumento);
 
 UPDATE contdocs SET concluido='S',estornado='S',
 EADr06=MD5(CONCAT(ecffabricacao,ncupomfiscal,IFNULL(contadornaofiscalGNF,""),IFNULL(contadordebitocreditoCDC,""),DATA,IFNULL(coognf,""),tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,davnumero,DATA,total))
 WHERE documento=nrDocumento;
 UPDATE contdocs SET 
 EADr06=MD5(CONCAT(IFNULL(ecffabricacao,""),IFNULL(ncupomfiscal,""),IFNULL(contadornaofiscalGNF,""),IFNULL(contadordebitocreditoCDC,""),DATA,IFNULL(coognf,""),tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(ifnull(ncupomfiscal,""),davnumero,DATA,total))
 WHERE ip=ipTerminal and ncupomfiscal=cooECF; 
UPDATE venda SET 
 venda.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")))
 WHERE coo=cooECF; 
 UPDATE venda SET 
 venda.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")))
 WHERE documento=nrDocumento; 
 
   CALL AtualizarQdtRegistros();	
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FechamentoDiario` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `FechamentoDiario`(IN filial VARCHAR(5))
BEGIN
  DECLARE qtdCxAbertos INT DEFAULT 0;
  
  DECLARE cursorQtdCxAbertos CURSOR FOR SELECT IFNULL( COUNT(1) ,0) FROM caixa WHERE codigofilial=filial; 
 
  OPEN cursorQtdCxAbertos;
 
  FETCH cursorQtdCxAbertos INTO qtdCxAbertos;
  
 if (qtdCxAbertos=0) then
	update movimento set finalizado="X" 
	where codigofilial=filial and finalizado=" ";
	
	update produtos set descontopromocao=current_date,
	validadepromoc = current_date,situacao="Normal"
	where validadepromoc<=current_date
	AND codigofilial=filial
	and situacao="Promoção";
	
	UPDATE produtosfilial SET descontopromocao=CURRENT_DATE,
	validadepromoc = CURRENT_DATE,situacao="Normal"
	WHERE validadepromoc<=CURRENT_DATE
	AND codigofilial=filial
	AND situacao="Promoção";
	
if ( (select abaterestoqueprevenda from configfinanc where codigofilial=filial limit 1) = "S") THEN	
	
	update produtos set qtdprevenda=0
	where qtdprevenda<0 and codigofilial=filial;
	
	UPDATE produtosfilial SET qtdprevenda=0
	WHERE qtdprevenda<0 AND codigofilial=filial;
	
	update produtos set qtdprevenda=0 where codigofilial=filial;
	UPDATE produtosfilial SET qtdprevenda=0 WHERE codigofilial=filial;
	
	UPDATE vendadav,produtos SET
	produtos.qtdprevenda=produtos.qtdprevenda+(SELECT SUM(vendadav.quantidade) FROM vendadav WHERE vendadav.codigo=produtos.codigo AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)>30 )
	WHERE vendadav.codigofilial= filial
	AND vendadav.codigo=produtos.codigo
	AND produtos.codigofilial=filial
	AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)>30;
	
	UPDATE vendadav,produtosfilial SET
	produtosfilial.qtdprevenda=produtosfilial.qtdprevenda+(SELECT SUM(vendadav.quantidade) FROM vendadav WHERE vendadav.codigo=produtosfilial.codigo AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)>30 )
	WHERE vendadav.codigofilial= filial
	AND vendadav.codigo=produtosfilial.codigo
	AND produtosfilial.codigofilial=filial
	AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)>30;
	
END IF;
	
	insert into crmovclientespagas select * from crmovclientes
	where valoratual=0 and filialpagamento=filial;
	
	delete from crmovclientespagas where valoratual=0 and filialpagamento=filial;	
	
if ( (select diasparamudarsituacao from configfinanc where codigofilial=filial limit 1)>0) THEN	
  IF ( (SELECT iasparamudarsituacaoinferior FROM configfinanc WHERE codigofilial=filial)>0) THEN	
	UPDATE clientes,crmovclientes 
	SET clientes.situacao=(SELECT situacaoautomaticainferior FROM configfinanc WHERE codigofilial=filial LIMIT 1),
	clientes.restritiva=(SELECT restritiva FROM situacaocli WHERE descricao=(SELECT situacaoautomaticainferior FROM configfinanc WHERE codigofilial=filial LIMIT 1))
	WHERE TO_DAYS(CURRENT_DATE)-TO_DAYS(crmovclientes.vencimento)<=(SELECT diasparamudarsituacaoinferior FROM configfinanc WHERE codigofilial=filial LIMIT 1)
	AND TO_DAYS(CURRENT_DATE)-TO_DAYS(crmovclientes.vencimento)>0
	AND clientes.codigo=crmovclientes.codigo
	AND clientes.restritiva='N'
	AND clientes.codigofilial=filial;
  END IF;
	
	UPDATE clientes,crmovclientes 
	SET clientes.situacao=(SELECT situacaoautomatica FROM configfinanc WHERE codigofilial=filial LIMIT 1),
	clientes.restritiva="S"
	WHERE TO_DAYS(CURRENT_DATE)-TO_DAYS(crmovclientes.vencimento)>=(SELECT diasparamudarsituacao FROM configfinanc WHERE codigofilial=filial LIMIT 1)
	AND clientes.codigo=crmovclientes.codigo
	AND clientes.restritiva='N'
	AND clientes.codigofilial=filial;
END IF;	
	UPDATE crmovclientes set quitado="N"
	where quitado="S" and valoratual>0
	and codigofilial=filial;	
 
	
 end if;
 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarDAV` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `FinalizarDAV`(in DAVNumero int,in filial varchar(5),in ipTerminal varchar(15))
BEGIN
UPDATE contdav SET 
EADRegistroDAV=MD5(CONCAT(ncupomfiscal,numero,DATA,valor,IFNULL(numeroECF,"001"),contadorRGECF,cliente,ecfCPFCNPJconsumidor))
WHERE numero=davNumero;
 UPDATE caixas SET 
 documento=DAVNumero,
 codigofilial=filial,
 vendedor=(select vendedor from contdav where numero=DAVNumero limit 1),
 vrdesconto=0,
 operador=(select operador from contdav where numero=DAVNumero limit 1)
 WHERE enderecoip=ipTerminal;
 
 
 update vendas set 
 vendas.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),ecffabricacao))
 where id=ipTerminal;
 
 UPDATE caixas set
 vrdesconto=(select desconto from contdav where numero=DAVNumero limit 1)
 WHERE enderecoip=ipTerminal;
 UPDATE vendas SET 
 documento=DAVNumero,
 codigofilial=filial,
 vendedor=(select vendedor from contdav where numero=DAVNumero limit 1),		
 operador=(select operador from contdav where numero=DAVNumero limit 1)
 WHERE id=ipTerminal;
 
 
 INSERT INTO `vendadav` (`acrescimototalitem`, `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`
 FROM vendas where id=ipTerminal;
 
 
 INSERT INTO `caixadav` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas where enderecoip=ipTerminal and valor<>0;
if ( (select abaterestoqueprevenda from configfinanc where codigofilial=filial limit 1)="S") then
if (filial="00001") THEN
UPDATE vendas,produtos SET
	produtos.qtdprevenda=produtos.qtdprevenda+(SELECT SUM(vendas.quantidade) FROM vendas WHERE vendas.codigo=produtos.codigo )
	WHERE vendas.codigofilial= filial
	AND vendas.codigo=produtos.codigo
	and vendas.id=ipTerminal
	AND produtos.codigofilial=filial;
END IF; 
IF (filial<>"00001") THEN
UPDATE vendas,produtosfilial SET
	produtosfilial.qtdprevenda=produtosfilial.qtdprevenda+(SELECT SUM(vendas.quantidade) FROM vendas WHERE vendas.codigo=produtosfilial.codigo )
	WHERE vendas.codigofilial= filial
	AND vendas.codigo=produtosfilial.codigo
	AND vendas.id=ipTerminal
	AND produtosfilial.codigofilial=filial;
END IF;
 END IF;
 
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
 
 UPDATE filiais SET versaopaf="1.7" WHERE codigofilial=filial;
 CALL AtualizarQdtRegistros();	
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarDAVOS` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `FinalizarDAVOS`(in DAVNumero int,in filial varchar(5),in ipTerminal varchar(15))
BEGIN
 UPDATE contdavos SET 
EADRegistroDAV=MD5(CONCAT(ncupomfiscal,numero,DATA,valor,IFNULL(numeroECF,"001"),contadorRGECF,cliente,ecfCPFCNPJconsumidor))
WHERE numero=davNumero;
 UPDATE caixas SET 
 documento=DAVNumero,
 codigofilial=filial,
 vendedor=(select vendedor from contdav where numero=DAVNumero limit 1),
 vrdesconto=0,
 operador=(select operador from contdav where numero=DAVNumero limit 1)
 WHERE enderecoip=ipTerminal;
 
 UPDATE vendas SET 
 vendas.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),ecffabricacao))
 WHERE id=ipTerminal; 
 
 UPDATE caixas set
 vrdesconto=(select desconto from contdav where numero=DAVNumero limit 1)
 WHERE enderecoip=ipTerminal;
 UPDATE vendas SET 
 documento=DAVNumero,
 codigofilial=filial,
 vendedor=(select vendedor from contdav where numero=DAVNumero limit 1),		
 operador=(select operador from contdav where numero=DAVNumero limit 1)
 WHERE id=ipTerminal;
 
 
 INSERT INTO `vendadavos` (`acrescimototalitem`, `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`
 FROM vendas where id=ipTerminal;
 
 
 INSERT INTO `caixadavos` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas where enderecoip=ipTerminal and valor<>0;
IF ( (SELECT abaterestoqueprevenda FROM configfinanc WHERE codigofilial=filial LIMIT 1)="S") THEN
IF (filial="00001") THEN
UPDATE vendas,produtos SET
	produtos.qtdprevenda=produtos.qtdprevenda+(SELECT SUM(vendas.quantidade) FROM vendas WHERE vendas.codigo=produtos.codigo )
	WHERE vendas.codigofilial= filial
	AND vendas.codigo=produtos.codigo
	AND vendas.id=ipTerminal
	AND produtos.codigofilial=filial;
END IF; 
IF (filial<>"00001") THEN
UPDATE vendas,produtosfilial SET
	produtosfilial.qtdprevenda=produtosfilial.qtdprevenda+(SELECT SUM(vendas.quantidade) FROM vendas WHERE vendas.codigo=produtosfilial.codigo )
	WHERE vendas.codigofilial= filial
	AND vendas.codigo=produtosfilial.codigo
	AND vendas.id=ipTerminal
	AND produtosfilial.codigofilial=filial;
END IF;
 END IF;
 
  
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
 CALL AtualizarQdtRegistros();	
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarPreVenda` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `FinalizarPreVenda`(in preVendaNumero int,in filial varchar(5),in ipTerminal varchar(15))
BEGIN
 UPDATE caixas SET 
 documento=preVendaNumero,
 codigofilial=filial,
 vendedor=(select vendedor from contprevendasPAF where numero=preVendaNumero limit 1),
 vrdesconto=0,
 operador=(select operador from contprevendasPAF where numero=preVendaNumero limit 1)
 WHERE enderecoip=ipTerminal;
 
 UPDATE caixas set
 vrdesconto=(select desconto from contprevendasPAF where numero=preVendaNumero limit 1)
 WHERE enderecoip=ipTerminal;
 
  
 UPDATE vendas SET 
 documento=preVendaNumero,data=current_date,
 codigofilial=filial,
 vendedor=(select vendedor from contprevendasPAF where numero=preVendaNumero limit 1),	
 eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),ecffabricacao)),
 operador=(select operador from contprevendasPAF where numero=preVendaNumero limit 1)
 WHERE id=ipTerminal;
  
 INSERT INTO `vendaprevendapaf` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`) 
 SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`
 FROM vendas where id=ipTerminal;
 
 
 INSERT INTO `caixaprevendapaf` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas where enderecoip=ipTerminal and valor<>0;
IF ( (SELECT abaterestoqueprevenda FROM configfinanc WHERE codigofilial=filial LIMIT 1)="S") THEN
IF (filial="00001") THEN
UPDATE vendas,produtos SET
	produtos.qtdprevenda=produtos.qtdprevenda+(SELECT SUM(vendas.quantidade) FROM vendas WHERE vendas.codigo=produtos.codigo )
	WHERE vendas.codigofilial= filial
	AND vendas.codigo=produtos.codigo
	AND vendas.id=ipTerminal
	AND produtos.codigofilial=filial;
END IF; 
IF (filial<>"00001") THEN
UPDATE vendas,produtosfilial SET
	produtosfilial.qtdprevenda=produtosfilial.qtdprevenda+(SELECT SUM(vendas.quantidade) FROM vendas WHERE vendas.codigo=produtosfilial.codigo )
	WHERE vendas.codigofilial= filial
	AND vendas.codigo=produtosfilial.codigo
	AND vendas.id=ipTerminal
	AND produtosfilial.codigofilial=filial;
END IF;
 END IF;
 
 
 
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
 CALL AtualizarQdtRegistros();	
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarVenda` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `FinalizarVenda`(IN doc INT,IN filial VARCHAR(5), IN ipTerminal VARCHAR(15),IN devolucaoNR INT,IN preVenda INT,IN DAVNumero INT )
BEGIN 	
 DECLARE qtdItens INT DEFAULT 0;
 DECLARE vlrSemEncargos REAL DEFAULT 0;
 DECLARE baixarPreVenda CHAR DEFAULT 'N';
 DECLARE valorDescontoItens REAL DEFAULT 0;
 DECLARE cursorQtdItens CURSOR FOR SELECT IFNULL( COUNT(1) ,0) FROM vendas WHERE id=ipTerminal AND cancelado='N';
 DECLARE cursorSencargos CURSOR FOR SELECT IFNULL( SUM(ABS(quantidade)*precooriginal-ratdesc) ,0 ) AS totalsemencargos FROM vendas WHERE id=ipTerminal AND cancelado='N';	
 DECLARE cnfPreVenda CURSOR FOR SELECT abaterestoqueprevenda FROM configfinanc WHERE codigofilial=filial;
 DECLARE cursorDescontoItens CURSOR FOR SELECT ifnull( SUM(ratdesc),0) FROM vendas WHERE id=ipTerminal AND cancelado='N';
 OPEN cursorQtdItens;
 OPEN cursorSencargos;
 OPEN cnfPreVenda;
 FETCH cursorQtdItens INTO qtdITens;
 FETCH cursorSencargos INTO vlrSemEncargos;
 FETCH cnfPrevenda INTO baixarPreVenda;
 CLOSE cursorQtdItens;
 CLOSE cursorSencargos;   
 SET @tabelaProduto='produtosfilial';
 IF (filial='00001') THEN
 SET @tabelaProduto='produtos';
 END IF;
 
UPDATE vendas SET 
vendas.modelodocfiscal=(SELECT contdocs.modeloDOCFiscal FROM contdocs WHERE contdocs.documento=doc),
vendas.serieNF=(SELECT contdocs.serienf FROM contdocs WHERE contdocs.documento=doc),
vendas.subserienf=(SELECT contdocs.subserienf FROM contdocs WHERE contdocs.documento=doc),
vendas.ecffabricacao=(SELECT contdocs.ecffabricacao FROM contdocs WHERE contdocs.documento=doc), 
vendas.codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE contdocs.documento=doc), 
vendas.operador=(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc),
vendas.coo=(SELECT contdocs.ncupomfiscal FROM contdocs WHERE contdocs.documento=doc),
vendas.ccf=(SELECT contdocs.ecfcontadorcupomfiscal FROM contdocs WHERE contdocs.documento=doc),
vendas.data=current_date
WHERE vendas.id=ipTerminal;
 
 UPDATE caixas SET 
 documento=doc,codigofilial=filial,
 dpfinanceiro=(SELECT contdocs.dpfinanceiro FROM contdocs WHERE contdocs.documento=doc),
 operador=(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc),
 classe=(SELECT contdocs.classe FROM contdocs WHERE contdocs.documento=doc),
 vendedor=(SELECT contdocs.vendedor FROM contdocs WHERE contdocs.documento=doc),
 coo=(SELECT contdocs.ncupomfiscal FROM contdocs WHERE contdocs.documento=doc),
 ccf=(SELECT contdocs.ecfcontadorcupomfiscal FROM contdocs WHERE contdocs.documento=doc),
 gnf=(SELECT contdocs.COOGNF FROM contdocs WHERE contdocs.documento=doc),
 ecfmodelo=(SELECT contdocs.ecfmodelo FROM contdocs WHERE contdocs.documento=doc),
 ecffabricacao=(SELECT contdocs.ecffabricacao FROM contdocs WHERE contdocs.documento=doc),
 data=current_date,
 ecfnumero=(SELECT contdocs.ecfnumero FROM contdocs WHERE contdocs.documento=doc)
 WHERE  caixas.enderecoip=ipTerminal;
 
 UPDATE caixas SET 
 vrdesconto=(SELECT contdocs.desconto FROM contdocs WHERE contdocs.documento=doc),
 valorservicos=(SELECT contdocs.valorservicos FROM contdocs WHERE contdocs.documento=doc)
 WHERE caixas.enderecoip=ipTerminal LIMIT 1;
 
 UPDATE caixas SET 
 nome=(SELECT contdocs.nome FROM contdocs WHERE contdocs.documento=doc)
 WHERE historico='Entrada'
 AND tipopagamento='DH'
 AND caixas.enderecoip=ipTerminal;
 SET @qVenda = CONCAT('UPDATE vendas,',@tabelaProduto,' set  
 vendas.documento=',doc,', 
 vendas.quantidadeanterior=',@tabelaProduto,'.quantidade,
 vendas.customedioanterior=',@tabelaProduto,'.customedio,
 vendas.quantidadeatualizada=',@tabelaProduto,'.quantidade-vendas.quantidade,		
 vendas.codigocliente=(select codigocliente from contdocs where documento=',doc,'),		
 vendas.comissao=',@tabelaProduto,'.tipocomissao,
 vendas.grupo=',@tabelaProduto,'.grupo,
 vendas.subgrupo=',@tabelaProduto,'.subgrupo,
 vendas.custo=',@tabelaProduto,'.custo,
 vendas.situacao=',@tabelaProduto,'.situacao,
 vendas.customedio=',@tabelaProduto,'.customedio,
 vendas.ecfnumero=(select ecfnumero from contdocs where documento=',doc,'),
 vendas.secao=',@tabelaProduto,'.secao,
 vendas.fornecedor=',@tabelaProduto,'.fornecedor,
 vendas.fabricante=',@tabelaProduto,'.fabricante,
 vendas.data=current_date,		
 vendas.classe=(select classe from contdocs where documento=',doc,'),
 vendas.lote=',@tabelaProduto,'.lote,
 vendas.codigobarras=',@tabelaProduto,'.codigobarras,		
 vendas.tipo=',@tabelaProduto,'.tipo,
 vendas.cofins=',@tabelaProduto,'.cofins,
 vendas.pis=',@tabelaProduto,'.pis,
 vendas.despesasacessorias=',@tabelaProduto,'.despesasacessorias 
 where vendas.id=(SELECT id from contdocs where documento=',doc,') and vendas.codigo=',@tabelaProduto,'.codigo 
 and ',@tabelaProduto,'.codigofilial=',filial);	
 PREPARE st FROM @qVenda;
 EXECUTE st;
 OPEN cursorDescontoItens;
 FETCH cursorDescontoItens INTO valorDescontoItens;
 IF (valorDescontoItens<>(SELECT desconto FROM contdocs WHERE documento=doc) ) THEN
 UPDATE vendas SET ratdesc=ratdesc+(SELECT desconto FROM contdocs WHERE documento=doc)-valorDescontoItens 
 WHERE id=ipTerminal LIMIT 1;
 END IF;
 SET @qbaixaPrd = CONCAT ('UPDATE ',@tabelaProduto,',vendas 
 set ',@tabelaProduto,'.qtdanterior=',@tabelaProduto,'.quantidade ,'
 ,@tabelaProduto,'.quantidade=',@tabelaProduto,'.quantidade-(SELECT IFNULL(SUM(vendas.quantidade*vendas.embalagem),0) from vendas where vendas.codigo=',@tabelaProduto,'.codigo and vendas.id=','"',ipTerminal,'"),',
 @tabelaProduto,'.dataultvenda=current_date,',
 @tabelaProduto,'.qtdprovisoria=0,',
 @tabelaProduto,'.EADP2relacaomercadoria=md5( concat(',@tabelaProduto,'.codigo,',@tabelaProduto,'.descricao,',@tabelaProduto,'.tributacao,',@tabelaProduto,'.icms,',@tabelaProduto,'.precovenda,',@tabelaProduto,'.precoatacado) ),',
 @tabelaProduto,'.qtdprateleiras=',@tabelaProduto,'.qtdprateleiras-(select IFNULL(sum(vendas.quantidade),0) from vendas where vendas.cancelado="N" and vendas.codigo=',@tabelaProduto,'.codigo and vendas.id=','"',ipTerminal,'")  
 where vendas.codigo=',@tabelaProduto,'.codigo 
 and ',@tabelaProduto,'.codigofilial=vendas.codigofilial  
 and vendas.id=','"',ipTerminal,'"');
 PREPARE st FROM @qbaixaPrd;
 EXECUTE st;
 IF(baixarPreVenda='S' AND (preVenda>0 OR DAVNumero>0)  ) THEN
 SET @qpreVenda = CONCAT ('UPDATE ',@tabelaProduto,',vendas 
 set ',@tabelaProduto,'.qtdprevenda=',@tabelaProduto,'.qtdprevenda-(SELECT IFNULL(SUM(vendas.quantidade*vendas.embalagem),0) from vendas where vendas.cancelado="N" and vendas.codigo=',@tabelaProduto,'.codigo and vendas.id=','"',ipTerminal,'")
 where vendas.codigo=',@tabelaProduto,'.codigo 
 and ',@tabelaProduto,'.codigofilial=vendas.codigofilial  
 and vendas.id=','"',ipTerminal,'"');
 PREPARE st FROM @qpreVenda;
 EXECUTE st;
 END IF;
 SET @qtdcomposicao = CONCAT('UPDATE ',@tabelaProduto,',produtoscomposicao',',vendas  
 SET ',@tabelaProduto,'.quantidade=',@tabelaProduto,'.quantidade-(produtoscomposicao.quantidade*(vendas.quantidade*vendas.embalagem) ) 
 WHERE produtoscomposicao.codigomateria=',@tabelaProduto,'.codigo and vendas.quantidade>0
 AND produtoscomposicao.codigo=vendas.codigo 
 AND vendas.cancelado="N" 
 AND vendas.id=','"',ipTerminal,'"');
 PREPARE st FROM @qtdcomposicao;
 EXECUTE st;
 SET @qQtdAnt = CONCAT ('UPDATE vendas,',@tabelaProduto,' SET 
 vendas.quantidadeatualizada=',@tabelaProduto,'.quantidade 
 WHERE vendas.id=','"',ipTerminal,'"
 AND vendas.codigo=',@tabelaProduto,'.codigo 
 AND ',@tabelaProduto,'.codigofilial=',filial);
 PREPARE st FROM @qQtdAnt;
 EXECUTE st;
 UPDATE produtosgrade,vendas SET
 produtosgrade.quantidade=produtosgrade.quantidade-(SELECT SUM(vendas.quantidade*vendas.embalagem) FROM vendas WHERE documento=doc AND cancelado='N' and vendas.grade=produtosgrade.grade)
 WHERE 	vendas.id=ipTerminal
 AND	 produtosgrade.codigo=vendas.codigo
 AND	 produtosgrade.grade=vendas.grade
 AND	 produtosgrade.codigofilial=filial;
 UPDATE clientes SET
 creditoprovisorio=0,ultcompra=CURRENT_DATE,
 debito=debito+IFNULL((SELECT SUM(valor) FROM caixas WHERE tipopagamento='CR' AND caixas.documento=doc ),0),
 debitoch=debitoch+IFNULL((SELECT SUM(valor) FROM caixas WHERE tipopagamento='CH' AND caixas.documento=doc),0),
 saldo=credito-debito
 WHERE codigo=(SELECT codigocliente FROM contdocs WHERE documento=doc);
 UPDATE clientes SET valorcartaofidelidade=valorcartaofidelidade+(SELECT total FROM contdocs WHERE documento=doc)
 WHERE codigo=(SELECT cartaofidelidade FROM contdocs WHERE documento=doc);
 
IF (devolucaoNR<>0) THEN
	CALL ProcessarDevolucao(doc,filial,ipTerminal,devolucaoNR,(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc)); 
END IF; 
 INSERT INTO movcartoes (codigofilial,documento,cartao,numero,DATA,
 vencimento,valor,operador,dpfinanceiro,encargos,nome)
 SELECT 	codigofilial,documento,cartao,numerocartao,CURRENT_DATE,vencimento,
 valor,operador,dpfinanceiro,
 ((SELECT total FROM contdocs WHERE documento=doc)-VlrSemEncargos)/qtdItens,nome
 FROM 	caixas
 WHERE 	enderecoip=ipTerminal
 AND 	tipopagamento IN ('CA','FI','TI','FN');
 INSERT INTO cheques (codigofilial,documento,banco,cheque,agencia,DATA,
 valor,valorcheque,vencimento,cliente,codigocliente,nomecheque,dpfinanceiro,
 cpf,cpfcheque,telefone,encargos) 
 SELECT codigofilial,documento,banco,cheque,agencia,DATA,valor,valorcheque,vencimento,
 nome,codigocliente,nomecheque,dpfinanceiro,
 (SELECT IF(cpf<>'',cpf,cnpj) FROM clientes WHERE codigo=(SELECT codigocliente FROM contdocs WHERE documento=doc)),	
 cpfcnpjch,(SELECT telefone FROM clientes WHERE codigo=(SELECT codigocliente FROM contdocs WHERE documento=doc)),
 ((SELECT total FROM contdocs WHERE documento=doc)-VlrSemEncargos)/qtdItens
 FROM caixas WHERE enderecoip=ipTerminal
 AND tipopagamento='CH';
 INSERT INTO crmovclientes (codigofilial,usuario,documento,nome,codigo,datacompra,
 vencimento,datacalcjuros,parcela,nrparcela,valor,valoratual,dependente,
 vendedor,classe,valorcorrigido,vrcapitalrec,porconta,valorpago,
 jurospago,jurosacumulado,ultvaloratual,encargos,dpfinanceiro,cpfcnpj) 
 SELECT codigofilial,operador,documento,nome,codigocliente,CURRENT_DATE,vencimento,vencimento,
 (SELECT nrparcelas FROM contdocs WHERE documento=doc),
 nrparcela,valor,valor,(SELECT dependente FROM contdocs WHERE documento=doc),vendedor,classe,valor,
 0,0,0,0,0,valor,
 ((SELECT total FROM contdocs WHERE documento=doc)-VlrSemEncargos)/qtdItens,
 dpfinanceiro,
 (SELECT IF(cpf<>'',cpf,cnpj) FROM clientes WHERE codigo=(SELECT codigocliente FROM contdocs WHERE documento=doc))
 FROM caixas WHERE enderecoip=ipTerminal
 AND tipopagamento='CR';
 
 INSERT INTO `venda` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`modelodocfiscal`,`serienf`,`subserienf`,`ecffabricacao`,`coo`,`acrescimototalitem`,`cancelado`,`eaddados`,`ccf`) 
 SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`modelodocfiscal` ,`serienf`,`subserienf`,`ecffabricacao`,`coo`,`acrescimototalitem`,`cancelado`,`eaddados`,`ccf`
 FROM vendas
 WHERE id=ipTerminal;
 
 UPDATE venda SET 
venda.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),ifnull(ecffabricacao,"")))
where documento=doc;
 
 INSERT INTO caixa (horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,DATA,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch,ecfnumero,ecffabricacao,ecfmodelo,coo,ccf,gnf)  
 SELECT horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,DATA,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch,ecfnumero,ecffabricacao,ecfmodelo,coo,ccf,gnf
 FROM caixas 
 WHERE enderecoip=ipTerminal;
 
 
  UPDATE caixa SET 
 eaddados=MD5(CONCAT(ecffabricacao,coo,ccf,gnf,ecfmodelo,valor,tipopagamento))
 WHERE  caixa.documento=doc;
 
 UPDATE contdocs SET concluido='S',
 EADr06=MD5(CONCAT(ifnull(ecffabricacao,""),ifnull(ncupomfiscal,""),IFNULL(contadornaofiscalGNF,""),IFNULL(contadordebitocreditoCDC,""),DATA,IFNULL(coognf,""),tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(ifnull(ncupomfiscal,""),davnumero,DATA,total))
 WHERE documento=doc;
 
 
 DELETE FROM caixas WHERE enderecoip=ipTerminal;
 DELETE FROM vendas WHERE id=ipTerminal;
 
 IF (preVenda>0) THEN
 UPDATE contprevendaspaf SET finalizada='S',
 datafinalizacao=CURRENT_DATE,
 ncupomfiscal=(SELECT ncupomfiscal FROM contdocs WHERE documento=doc)
 WHERE numeroDAVfilial=preVenda AND codigofilial=filial;	
 END IF;
 
 IF (DAVNumero>0) THEN
 UPDATE contdav SET finalizada='S',
 datafinalizacao=CURRENT_DATE,
 EADRegistroDAV=MD5(CONCAT(ifnull(ncupomfiscal,""),numero,DATA,valor,ifnull(numeroECF,"001"))),
 ncupomfiscal=(SELECT ncupomfiscal FROM contdocs WHERE documento=doc)
 WHERE numeroDAVfilial=DAVNumero and codigofilial=filial;	
 
 UPDATE contdavos SET finalizada='S',
 datafinalizacao=CURRENT_DATE,
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,numero,DATA,valor,IFNULL(numeroECF,"001"),ifnull(contadorRGECF,""),ifnull(cliente,""),ifnull(ecfCPFCNPJconsumidor,""))),
 ncupomfiscal=(SELECT ncupomfiscal FROM contdocs WHERE documento=doc)
 WHERE numero=DAVNumero AND codigofilial=filial;
 
 END IF; 
 
  CALL AtualizarQdtRegistros();	
 END */$$
DELIMITER ;

/* Procedure structure for procedure `GravarR` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `GravarR`(in dataMovimento DATETIME )
BEGIN
update r01 set eaddados=md5(concat(fabricacaoECF,cnpj,cnpjdesenvolvedora,aplicativo,r01.md5))
where data=datamovimento;
update r02 set eaddados=md5(concat(fabricacaoECF,crz,coo,cro,data,dataemissaoreducaoz,horaemissaoreducaoz,vendabrutadiaria))
where data=datamovimento;
update r03 set eaddados=md5(concat(fabricacaoECF,CRZ,totalizadorParcial))
where data=datamovimento;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarDAV` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProcessarDAV`(in numeroDAV int,in ipTerminal varchar(15),in filial varchar(5))
BEGIN
 UPDATE vendadav 
 SET 	codigofilial=filial,id=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial;
 
 INSERT INTO `vendas` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`) 
 SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`
 FROM vendadav where documento=numeroDAV AND codigofilial=filial;
 
 UPDATE caixadav 
 SET	codigofilial=filial,enderecoip=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial;
 
 INSERT INTO `caixas` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixadav where documento=numeroDAV AND codigofilial=filial;
 END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarDAVOS` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProcessarDAVOS`(in numeroDAV int,in ipTerminal varchar(15),in filial varchar(5))
BEGIN
 UPDATE vendadavos 
 SET 	codigofilial=filial,id=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial;
 
 INSERT INTO `vendas` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`) 
 SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`
 FROM vendadavos where documento=numeroDAV AND codigofilial=filial;
 
 UPDATE caixadavos 
 SET	codigofilial=filial,enderecoip=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial;
 
 INSERT INTO `caixas` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixadavos where documento=numeroDAV AND codigofilial=filial;
 END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarDevolucao` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProcessarDevolucao`(in doc int,in filial varchar(5), in ipTerminal varchar(15),in devolucaoNR int, in operadorAcao varchar(10))
BEGIN
 set @tabelaProduto='produtosfilial';
 if (filial='00001') THEN
 set @tabelaProduto='produtos';
 END IF;
 set @qDevolucao= CONCAT('update ',@tabelaProduto,',devolucao set '
 ,@tabelaProduto,'.qtdanterior=',@tabelaProduto,'.quantidade,'
 ,@tabelaProduto,'.quantidade=',@tabelaProduto,'.quantidade+devolucao.quantidade,'
 ,@tabelaProduto,'.qtdprateleiras=',@tabelaProduto,'.qtdprateleiras+devolucao.prateleiras,
 devolucao.situacao=',@tabelaProduto,'.situacao,
 devolucao.dpfinanceiro=','"','Vendas','",',
 @tabelaProduto,'.customedioanterior=',@tabelaProduto,'.customedio,',
 @tabelaProduto,'.customedio=( ( (devolucao.quantidade*devolucao.custo)+( (',@tabelaProduto,'.quantidade+',@tabelaProduto,
 '.qtdretida)*',@tabelaProduto,'.custo) ) / (',@tabelaProduto,'.quantidade+',@tabelaProduto,
 '.qtdretida+devolucao.quantidade) ) 		
 where devolucao.codigo=',@tabelaProduto,'.codigo and devolucao.numero=',devolucaoNR,
 ' and ',@tabelaProduto,'.codigofilial=',filial);
 PREPARE st FROM @qDevolucao;
 EXECUTE st;
 INSERT INTO venda (codigofilial,id,operador,codigo,produto,quantidade,preco,
 precooriginal,customedio,total,vendedor,comissao,data,documento,custo,
 grupo,subgrupo,situacao,numerodevolucao,grade) 
 SELECT filial,ipTerminal,operadorAcao,
 codigo,produto,quantidade*-1,precooriginal*-1,preco*-1,customedio*-1,
 total*-1,vendedor,comissao,current_date,doc,
 custo*-1,grupo,subgrupo,situacao,devolucaoNR,grade
 FROM 	devolucao where numero=devolucaoNR
 AND	finalizada='N';
 UPDATE	contdocs SET observacao=(select observacao from devolucao where numero=devolucaoNR AND observacao<>'') 
 WHERE	documento=doc;
 UPDATE devolucao set finalizada='S',data=current_date where numero=devolucaoNR;
 UPDATE contdevolucao set finalizada='S',data=current_date where numero=devolucaoNR;
 
set @qCustoMedio = CONCAT('update venda,',@tabelaProduto,' 
 SET venda.customedio=',@tabelaProduto,'.customedio*-1,
 venda.quantidadeanterior=',@tabelaProduto,'.qtdanterior,
 venda.customedioanterior=',@tabelaProduto,'.customedioanterior,
 venda.quantidadeatualizada=',@tabelaProduto,'.quantidade  
 WHERE venda.id=','"',ipTerminal,'"
 AND venda.codigo=',@tabelaProduto,'.codigo
 AND ',@tabelaProduto,'.codigofilial=',filial,'
 AND venda.quantidade<0 ');
 PREPARE st FROM @qCustoMedio;
 EXECUTE st;
IF (doc=0) THEN
INSERT INTO caixa (enderecoip,codigofilial,valor,data,dataexe,tipopagamento,operador,vendedor,dpfinanceiro)
values (ipTerminal,filial,(select sum(preco*quantidade) from devolucao where numero=devolucaoNR),current_date,current_date,"DV",operadorAcao,
(select vendedor from devolucao where numero=devolucaoNR limit 1),"Venda");
END IF;
 UPDATE  produtosgrade,devolucao SET 
 produtosgrade.quantidade=produtosgrade.quantidade+devolucao.quantidade 
 WHERE 	devolucao.numero=devolucaoNR
 AND 	produtosgrade.codigo=devolucao.codigo
 AND 	produtosgrade.grade=devolucao.grade 
 AND 	produtosgrade.codigofilial=filial;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarPreVenda` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProcessarPreVenda`(in numeroPreVenda int,in ipTerminal varchar(15),in filial varchar(5))
BEGIN
 UPDATE vendaprevendapaf 
 SET 	codigofilial=filial,id=ipTerminal
 WHERE 	documento=numeroPreVenda AND codigofilial=filial;
 
 INSERT INTO `vendas` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`) 
 SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`
 FROM vendaprevendapaf where documento=numeroPreVenda and codigofilial=filial;
 
 UPDATE caixaprevendapaf 
 SET	codigofilial=filial,enderecoip=ipTerminal
 WHERE 	documento=numeroPreVenda AND codigofilial=filial;
 
 INSERT INTO `caixas` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixaprevendapaf where documento=numeroPreVenda AND codigofilial=filial;
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `QuitarDebitoCliente` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `QuitarDebitoCliente`(in codigoCliente int,in doc int,in ipTerminal varchar(15),in filial varchar(5))
BEGIN
 UPDATE caixas SET 
 documento=doc,codigofilial=filial,
 dpfinanceiro=(select contdocs.dpfinanceiro from contdocs where contdocs.documento=doc),
 operador=(select contdocs.operador from contdocs where contdocs.documento=doc),
 classe=(select contdocs.classe from contdocs where contdocs.documento=doc),
 vendedor=(select contdocs.vendedor from contdocs where contdocs.documento=doc),
 historico=(select contdocs.dpfinanceiro from contdocs where contdocs.documento=doc)
 WHERE  caixas.enderecoip=ipTerminal;
 
 UPDATE caixas SET 
 vrdesconto=(select contdocs.desconto from contdocs where contdocs.documento=doc),
 vrjuros=(select contdocs.vrjuros from contdocs where contdocs.documento=doc)
 WHERE caixas.enderecoip=ipTerminal limit 1;
 
 INSERT INTO movcartoes (codigofilial,documento,cartao,numero,data,
 vencimento,valor,operador,dpfinanceiro,encargos,nome)
 SELECT 	codigofilial,documento,cartao,numerocartao,current_date,vencimento,
 valor,operador,dpfinanceiro,
 (select encargos from contdocs where documento=doc),
 '0'
 FROM 	caixas
 WHERE 	enderecoip=ipTerminal
 AND 	tipopagamento IN ('CA','FI','TI','FN');
 
 INSERT INTO cheques (codigofilial,documento,banco,cheque,agencia,data,
 valor,valorcheque,vencimento,cliente,codigocliente,nomecheque,dpfinanceiro,
 cpf,cpfcheque,telefone,encargos) 
 SELECT codigofilial,documento,banco,cheque,agencia,data,valor,valorcheque,vencimento,
 nome,codigocliente,nomecheque,dpfinanceiro,
 (select if(cpf<>'',cpf,cnpj) from clientes where codigo=(select codigocliente from contdocs where documento=doc)),	
 cpfcnpjch,(select telefone from clientes where codigo=(select codigocliente from contdocs where documento=doc)),
 (select encargos from contdocs where documento=doc)
 FROM caixas where enderecoip=ipTerminal
 AND tipopagamento='CH';
 
 UPDATE crmovclientes SET
 ultvaloratual=valoratual,
 valoratual= valorcorrigido-valorpago,
 valorcorrigido= Valorcorrigido-valorpago,
 datapagamento = current_date,
 ultvencimento=datacalcjuros,
 datacalcjuros = current_date,
 ultporconta=porconta,
 porconta = valorpago+porconta,
 vrultpagamento = valorpago,
 ultjurospago=jurospago,
 jurospago = jurospago+vrjuros,
 ultcaprec=vrcapitalrec,
 vrcapitalrec = porconta-jurospago,
 ultjurosacumulado=jurosacumulado,
 desconto=0,
 jurosacumulado=valoratual-valor+vrcapitalrec,
 tipopagamento=(SELECT tipopagamento FROM caixas WHERE documento=doc LIMIT 1),
 filialpagamento=filial,
 sequencia = IF(sequencia=0,CONCAT('s',doc,'s'),CONCAT('',doc,'s'))
 WHERE codigo=codigoCliente
 AND quitado='S'
 AND valoratual>0
 AND ip=ipTerminal;
 
 INSERT INTO caixa (horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,data,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch)  
 SELECT horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,data,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch 
 FROM caixas 
 WHERE enderecoip=ipTerminal;
 UPDATE contdocs set concluido='S' WHERE documento=doc;
 UPDATE crmovclientes set quitado='N' 
 WHERE codigo=codigoCliente;		
 
DELETE FROM caixas where enderecoip=ipTerminal;
 UPDATE clientes SET ultvrpago=(select contdocs.totalbruto FROM contdocs 
 WHERE contdocs.documento=doc),ultpagamento=CURRENT_DATE,
 debito=debito-(SELECT contdocs.totalbruto FROM contdocs WHERE contdocs.documento=doc)-(SELECT contdocs.vrjuros FROM contdocs WHERE contdocs.documento=doc limit 1)
 WHERE codigo=codigoCliente;
 
  UPDATE contdocs SET 
 EADr06=MD5(CONCAT(ecffabricacao,ncupomfiscal,contadornaofiscalGNF,contadordebitocreditoCDC,DATA,coognf,tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,davnumero,DATA,total))
 WHERE documento=doc;
 
  CALL AtualizarQdtRegistros();
 END */$$
DELIMITER ;

/* Procedure structure for procedure `ReservarPreVenda` */

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ReservarPreVenda`(in filial varchar(5),in codigoProduto varchar(20),in quantidade decimal(10,3))
BEGIN
  IF (filial="00001") THEN
	UPDATE produtos set qtdprevenda=qtdprevenda+quantidade
	where codigofilial=filial and codigo=codigoProduto; 
  END IF;
 
   IF (filial<>"00001") THEN
	UPDATE produtosfilial SET qtdprevenda=qtdprevenda+quantidade
	WHERE codigofilial=filial AND codigo=codigoProduto; 
  END IF;
    END */$$
DELIMITER ;

/*Table structure for table `60d` */

DROP TABLE IF EXISTS `60d`;

/*!50001 CREATE TABLE  `60d`(
 `codigofilial` varchar(5) ,
 `notafiscal` varchar(15) ,
 `serienf` char(3) ,
 `modelodocfiscal` char(2) ,
 `documento` int(10) unsigned ,
 `DATA` date ,
 `cfop` varchar(5) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `SUM(quantidade)` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `ecffabricacao` varchar(20) ,
 `coo` varchar(6) ,
 `preco` decimal(10,2) ,
 `totalicms` decimal(47,2) ,
 `descontovalor` decimal(32,2) ,
 `descontovalorCupom` decimal(32,4) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `acrescimototalitem` decimal(30,2) ,
 `baseCalculoICMS` decimal(37,2) 
)*/;

/*Table structure for table `60i` */

DROP TABLE IF EXISTS `60i`;

/*!50001 CREATE TABLE  `60i`(
 `codigofilial` varchar(5) ,
 `notafiscal` varchar(15) ,
 `serienf` char(3) ,
 `modelodocfiscal` char(2) ,
 `documento` int(10) unsigned ,
 `DATA` date ,
 `cfop` varchar(5) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `SUM(quantidade)` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `ecffabricacao` varchar(20) ,
 `coo` varchar(6) ,
 `preco` decimal(10,2) ,
 `cancelado` char(1) ,
 `totalicms` decimal(47,2) ,
 `descontovalor` decimal(32,2) ,
 `descontovalorCupom` decimal(32,4) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `baseCalculoICMS` decimal(37,2) 
)*/;

/*Table structure for table `60r` */

DROP TABLE IF EXISTS `60r`;

/*!50001 CREATE TABLE  `60r`(
 `codigofilial` varchar(5) ,
 `notafiscal` varchar(15) ,
 `serienf` char(3) ,
 `modelodocfiscal` char(2) ,
 `documento` int(10) unsigned ,
 `DATA` date ,
 `cfop` varchar(5) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `SUM(quantidade)` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `ecffabricacao` varchar(20) ,
 `coo` varchar(6) ,
 `preco` decimal(10,2) ,
 `totalicms` decimal(47,2) ,
 `descontovalor` decimal(32,2) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `baseCalculoICMS` decimal(37,2) 
)*/;

/*Table structure for table `blococregc190` */

DROP TABLE IF EXISTS `blococregc190`;

/*!50001 CREATE TABLE  `blococregc190`(
 `codigofilial` varchar(5) ,
 `numero` int(6) unsigned ,
 `nf` varchar(15) ,
 `modelonf` varchar(2) ,
 `dataentrada` date ,
 `cfopentrada` varchar(5) ,
 `icmsentrada` decimal(8,2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `sequencia` int(4) ,
 `quantidade` decimal(32,2) ,
 `unidade` char(3) ,
 `custo` decimal(10,5) ,
 `bcicms` decimal(32,2) ,
 `toticms` decimal(39,2) ,
 `ipiItem` decimal(39,2) ,
 `totalProduto` decimal(32,2) ,
 `totalNF` decimal(40,2) 
)*/;

/*Table structure for table `blococregc190_saida` */

DROP TABLE IF EXISTS `blococregc190_saida`;

/*!50001 CREATE TABLE  `blococregc190_saida`(
 `codigofilial` varchar(5) ,
 `notafiscal` varchar(15) ,
 `serienf` char(3) ,
 `modelodocfiscal` char(2) ,
 `documento` int(10) unsigned ,
 `DATA` date ,
 `cfop` varchar(5) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `SUM(quantidade)` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `totalicms` decimal(47,2) ,
 `descontovalor` decimal(32,2) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `baseCalculoICMS` decimal(37,2) 
)*/;

/*Table structure for table `blococregc300` */

DROP TABLE IF EXISTS `blococregc300`;

/*!50001 CREATE TABLE  `blococregc300`(
 `ip` varchar(15) ,
 `documento` int(10) ,
 `data` date ,
 `Totalbruto` decimal(12,2) ,
 `dpfinanceiro` varchar(15) ,
 `desconto` decimal(10,2) ,
 `total` decimal(12,2) ,
 `NrParcelas` int(3) ,
 `vendedor` char(3) ,
 `operador` varchar(10) ,
 `Observacao` text ,
 `classe` varchar(4) ,
 `dataexe` date ,
 `codigocliente` int(6) ,
 `nome` varchar(50) ,
 `CodigoFilial` varchar(5) ,
 `historico` varchar(25) ,
 `vrjuros` decimal(10,2) ,
 `tipopagamento` char(2) ,
 `encargos` decimal(12,2) ,
 `id` varchar(15) ,
 `estornado` char(1) ,
 `enderecoentrega` varchar(150) ,
 `custos` decimal(12,2) ,
 `devolucaovenda` decimal(12,2) ,
 `devolucaorecebimento` decimal(12,2) ,
 `nrboletobancario` bigint(12) ,
 `nrnotafiscal` bigint(12) ,
 `classedevolucao` varchar(4) ,
 `responsavelreceber` varchar(60) ,
 `numeroentrega` varchar(10) ,
 `cidadeentrega` varchar(60) ,
 `cepentrega` varchar(8) ,
 `bairroentrega` varchar(20) ,
 `horaentrega` time ,
 `dataentrega` date ,
 `obsentrega` varchar(150) ,
 `concluido` char(1) ,
 `cartaofidelidade` varchar(20) ,
 `bordero` char(1) ,
 `valorservicos` decimal(10,2) ,
 `descontoservicos` decimal(8,2) ,
 `romaneio` char(1) ,
 `hora` time ,
 `entregaconcluida` char(1) ,
 `dataentregaconcluida` date ,
 `operadorentrega` varchar(10) ,
 `ncupomfiscal` varchar(10) ,
 `nreducaoz` varchar(10) ,
 `ecfnumero` char(3) ,
 `TEF` char(1) ,
 `ecfValorCancelamentos` decimal(10,2) ,
 `NF_e` char(1) ,
 `estadoentrega` char(2) ,
 `ecfConsumidor` varchar(30) ,
 `ecfCPFCNPJconsumidor` varchar(14) ,
 `ecfEndConsumidor` text ,
 `prevendanumero` varchar(30) ,
 `ecfcontadorcupomfiscal` varchar(10) ,
 `ecftotalliquido` decimal(10,2) ,
 `contadornaofiscalGNF` varchar(6) ,
 `contadordebitocreditoCDC` varchar(4) ,
 `totalICMScupomfiscal` decimal(10,2) ,
 `troco` decimal(8,2) ,
 `davnumero` int(10) ,
 `ecffabricacao` varchar(20) ,
 `ecfMFadicional` varchar(2) ,
 `ecftipo` varchar(7) ,
 `ecfmarca` varchar(15) ,
 `ecfmodelo` varchar(20) ,
 `estoqueatualizado` char(1) ,
 `serienf` varchar(3) ,
 `EADRegistroDAV` varchar(33) ,
 `EADr06` varchar(33) ,
 `tipopagamentoECF` varchar(2) ,
 `modeloDOCFiscal` varchar(2) ,
 `subserienf` varchar(3) ,
 `totalDocumento` decimal(34,2) 
)*/;

/*Table structure for table `blococregc320` */

DROP TABLE IF EXISTS `blococregc320`;

/*!50001 CREATE TABLE  `blococregc320`(
 `data` date ,
 `documento` int(11) unsigned ,
 `serieNF` varchar(3) ,
 `subserienf` varchar(3) ,
 `modelodocfiscal` varchar(2) ,
 `ecfnumero` char(3) ,
 `NotaFiscal` varchar(15) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `icms` int(11) ,
 `total` decimal(32,2) ,
 `bcICMS` decimal(32,2) ,
 `totalICMS` decimal(24,6) 
)*/;

/*Table structure for table `blococregc321` */

DROP TABLE IF EXISTS `blococregc321`;

/*!50001 CREATE TABLE  `blococregc321`(
 `inc` int(11) ,
 `codigofilial` varchar(5) ,
 `operador` varchar(10) ,
 `data` date ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `quantidade` decimal(10,5) ,
 `preco` decimal(10,2) ,
 `custo` decimal(10,5) ,
 `precooriginal` decimal(10,2) ,
 `Descontoperc` decimal(6,2) ,
 `id` varchar(15) ,
 `descontovalor` decimal(10,2) ,
 `total` decimal(10,2) ,
 `vendedor` char(3) ,
 `nrcontrole` int(11) ,
 `documento` int(11) unsigned ,
 `grupo` varchar(30) ,
 `subgrupo` varchar(30) ,
 `comissao` char(1) ,
 `ratdesc` decimal(10,4) ,
 `rateioencargos` decimal(8,4) ,
 `situacao` varchar(15) ,
 `customedio` decimal(10,5) ,
 `Ecfnumero` char(3) ,
 `fornecedor` varchar(30) ,
 `fabricante` varchar(30) ,
 `NotaFiscal` varchar(15) ,
 `icms` int(11) ,
 `classe` varchar(4) ,
 `secao` varchar(20) ,
 `lote` varchar(15) ,
 `tributacao` char(3) ,
 `aentregar` char(1) ,
 `quantidadeanterior` decimal(14,5) ,
 `quantidadeatualizada` decimal(14,5) ,
 `codigofiscal` char(3) ,
 `customedioanterior` decimal(10,5) ,
 `codigocliente` int(11) ,
 `numerodevolucao` int(11) ,
 `codigobarras` varchar(20) ,
 `ipi` int(11) ,
 `unidade` char(3) ,
 `embalagem` int(11) ,
 `grade` varchar(10) ,
 `romaneio` char(1) ,
 `tipo` varchar(15) ,
 `cofins` decimal(5,2) ,
 `pis` decimal(5,2) ,
 `despesasacessorias` decimal(5,2) ,
 `percentualRedBaseCalcICMS` decimal(5,2) ,
 `serieNF` varchar(3) ,
 `subserienf` varchar(3) ,
 `cfop` varchar(5) ,
 `acrescimototalitem` decimal(8,2) ,
 `cstpis` varchar(2) ,
 `cstcofins` varchar(2) ,
 `icmsst` decimal(5,2) ,
 `percentualRedBaseCalcICMSST` decimal(5,2) ,
 `mvast` decimal(5,3) ,
 `modelodocfiscal` varchar(2) ,
 `somaQuantidade` decimal(32,5) ,
 `totalItem` decimal(35,4) ,
 `totalDesconto` decimal(32,4) ,
 `bcICMS` decimal(32,2) ,
 `totalICMS` decimal(46,6) ,
 `totalPIS` decimal(27,2) ,
 `totalCOFINS` decimal(27,2) 
)*/;

/*Table structure for table `blococregc390` */

DROP TABLE IF EXISTS `blococregc390`;

/*!50001 CREATE TABLE  `blococregc390`(
 `documento` int(11) unsigned ,
 `data` date ,
 `icms` int(11) ,
 `cfop` varchar(5) ,
 `tributacao` char(3) ,
 `total` decimal(33,2) ,
 `baseCalculoICMS` decimal(33,2) ,
 `totalICMS` decimal(42,2) 
)*/;

/*Table structure for table `blococregc400` */

DROP TABLE IF EXISTS `blococregc400`;

/*!50001 CREATE TABLE  `blococregc400`(
 `id` int(10) ,
 `codigofilial` varchar(5) ,
 `data` date ,
 `tipo` varchar(3) ,
 `fabricacaoECF` varchar(20) ,
 `MFadicional` char(1) ,
 `modeloECF` varchar(20) ,
 `numeroUsuarioSubstituicaoECF` varchar(2) ,
 `crz` varchar(6) ,
 `coo` varchar(6) ,
 `cro` varchar(6) ,
 `datamovimento` date ,
 `dataemissaoreducaoz` date ,
 `horaemissaoreducaoz` time ,
 `vendabrutadiaria` decimal(12,2) ,
 `parametroISSQNdesconto` char(1) ,
 `numeroECF` varchar(3) ,
 `gtfinal` decimal(12,2) ,
 `EADdados` varchar(33) 
)*/;

/*Table structure for table `blococregc425` */

DROP TABLE IF EXISTS `blococregc425`;

/*!50001 CREATE TABLE  `blococregc425`(
 `documento` int(11) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(10,2) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(32,2) ,
 `icms` int(11) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `cancelado` char(1) 
)*/;

/*Table structure for table `blococregc470` */

DROP TABLE IF EXISTS `blococregc470`;

/*!50001 CREATE TABLE  `blococregc470`(
 `documento` int(10) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(8,3) ,
 `unidade` char(3) ,
 `preco` decimal(10,2) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(12,2) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) 
)*/;

/*Table structure for table `blococregc490` */

DROP TABLE IF EXISTS `blococregc490`;

/*!50001 CREATE TABLE  `blococregc490`(
 `documento` int(10) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(8,3) ,
 `unidade` char(3) ,
 `preco` decimal(10,2) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `baseCalculoICMS` decimal(34,2) ,
 `totalICMS` decimal(44,2) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) 
)*/;

/*Table structure for table `r05` */

DROP TABLE IF EXISTS `r05`;

/*!50001 CREATE TABLE  `r05`(
 `ncupomfiscal` varchar(6) ,
 `ecfcontadorcupomfiscal` varchar(6) ,
 `data` date ,
 `documento` int(11) ,
 `ecfnumero` char(3) ,
 `estornado` char(1) ,
 `dpfinanceiro` varchar(15) ,
 `ecffabricacao` varchar(20) ,
 `ecfMFadicional` varchar(2) ,
 `ecfmodelo` varchar(20) ,
 `nrcontrole` int(11) ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `quantidade` decimal(10,5) ,
 `unidade` char(3) ,
 `precooriginal` decimal(10,2) ,
 `descontovalor` decimal(10,2) ,
 `preco` decimal(10,2) ,
 `total` decimal(10,2) ,
 `icms` int(11) ,
 `tributacao` char(3) ,
 `cancelado` char(1) ,
 `ccf` varchar(6) ,
 `coo` varchar(6) ,
 `acrescimototalitem` decimal(8,2) ,
 `Descontoperc` decimal(6,2) ,
 `eaddados` varchar(33) ,
 `indicadorproducao` char(1) ,
 `indicadorarredondamentotruncamento` char(1) 
)*/;

/*Table structure for table `registro50entradas_agr` */

DROP TABLE IF EXISTS `registro50entradas_agr`;

/*!50001 CREATE TABLE  `registro50entradas_agr`(
 `codigofilial` varchar(5) ,
 `numero` int(6) unsigned ,
 `nf` varchar(15) ,
 `modelonf` varchar(2) ,
 `dataentrada` date ,
 `cfopentrada` varchar(5) ,
 `icmsentrada` decimal(8,2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `sequencia` int(4) ,
 `quantidade` decimal(32,2) ,
 `unidade` char(3) ,
 `custo` decimal(10,5) ,
 `bcicms` decimal(32,2) ,
 `toticms` decimal(39,2) ,
 `ipiItem` decimal(39,2) ,
 `totalProduto` decimal(32,2) ,
 `totalNF` decimal(40,2) ,
 `lancada` char(1) 
)*/;

/*Table structure for table `registro50entradas_itens` */

DROP TABLE IF EXISTS `registro50entradas_itens`;

/*!50001 CREATE TABLE  `registro50entradas_itens`(
 `codigofilial` varchar(5) ,
 `numero` int(6) unsigned ,
 `nf` varchar(15) ,
 `modelonf` varchar(2) ,
 `dataentrada` date ,
 `cfopentrada` varchar(5) ,
 `icmsentrada` decimal(8,2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `sequencia` int(4) ,
 `quantidade` decimal(32,2) ,
 `unidade` char(3) ,
 `custo` decimal(10,5) ,
 `bcicms` decimal(32,2) ,
 `toticms` decimal(39,2) ,
 `ipiItem` decimal(39,2) ,
 `totalProduto` decimal(32,2) ,
 `totalNF` decimal(40,2) 
)*/;

/*Table structure for table `registro50saida_agr` */

DROP TABLE IF EXISTS `registro50saida_agr`;

/*!50001 CREATE TABLE  `registro50saida_agr`(
 `codigofilial` varchar(5) ,
 `notafiscal` varchar(15) ,
 `serienf` char(3) ,
 `modelodocfiscal` char(2) ,
 `documento` int(10) unsigned ,
 `DATA` date ,
 `cfop` varchar(5) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `SUM(quantidade)` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `totalicms` decimal(47,2) ,
 `descontovalor` decimal(32,2) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `baseCalculoICMS` decimal(37,2) 
)*/;

/*Table structure for table `registro50saidas_itens` */

DROP TABLE IF EXISTS `registro50saidas_itens`;

/*!50001 CREATE TABLE  `registro50saidas_itens`(
 `inc` int(8) ,
 `codigofilial` varchar(5) ,
 `notafiscal` varchar(15) ,
 `serienf` char(3) ,
 `modelodocfiscal` char(2) ,
 `documento` int(10) unsigned ,
 `DATA` date ,
 `cfop` varchar(5) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `SUM(quantidade)` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `totalicms` decimal(47,2) ,
 `descontovalor` decimal(32,2) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `baseCalculoICMS` decimal(37,2) 
)*/;

/*View structure for view 60d */

/*!50001 DROP TABLE IF EXISTS `60d` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `60d` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`ratdesc`) AS `descontovalorCupom`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` group by `vendatmp`.`data`,`vendatmp`.`codigo` */;

/*View structure for view 60i */

/*!50001 DROP TABLE IF EXISTS `60i` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `60i` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`cancelado` AS `cancelado`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`ratdesc`) AS `descontovalorCupom`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` group by `vendatmp`.`codigo` */;

/*View structure for view 60r */

/*!50001 DROP TABLE IF EXISTS `60r` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `60r` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` group by month(`vendatmp`.`data`),`vendatmp`.`icms`,`vendatmp`.`codigo` */;

/*View structure for view blococregc190 */

/*!50001 DROP TABLE IF EXISTS `blococregc190` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc190` AS select `entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`bcicms`) AS `bcicms`,round(sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),2) AS `toticms`,sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) AS `ipiItem`,sum(`entradas`.`totalitem`) AS `totalProduto`,(sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`totalitem`)) AS `totalNF` from `entradas` group by `entradas`.`NF`,`entradas`.`cfopentrada`,`entradas`.`IcmsEntrada`,`entradas`.`tributacao`,`entradas`.`numero` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view blococregc190_saida */

/*!50001 DROP TABLE IF EXISTS `blococregc190_saida` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc190_saida` AS select `vendanf`.`codigofilial` AS `codigofilial`,`vendanf`.`NotaFiscal` AS `notafiscal`,`vendanf`.`serieNF` AS `serienf`,`vendanf`.`modelodocfiscal` AS `modelodocfiscal`,`vendanf`.`documento` AS `documento`,`vendanf`.`data` AS `DATA`,`vendanf`.`cfop` AS `cfop`,`vendanf`.`icms` AS `icms`,`vendanf`.`tributacao` AS `tributacao`,`vendanf`.`codigo` AS `codigo`,`vendanf`.`produto` AS `produto`,sum(`vendanf`.`quantidade`) AS `SUM(quantidade)`,`vendanf`.`unidade` AS `unidade`,`vendanf`.`nrcontrole` AS `nrcontrole`,sum(round((((`vendanf`.`total` - ((`vendanf`.`total` * `vendanf`.`percentualRedBaseCalcICMS`) / 100)) * `vendanf`.`icms`) / 100),2)) AS `totalicms`,sum(`vendanf`.`descontovalor`) AS `descontovalor`,sum(`vendanf`.`total`) AS `SUM(TOTAL)`,if((`vendanf`.`icms` > 0),sum(round((`vendanf`.`total` - (`vendanf`.`total` * (`vendanf`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendanf` group by `vendanf`.`NotaFiscal`,`vendanf`.`serieNF`,`vendanf`.`icms`,`vendanf`.`cfop`,`vendanf`.`codigofilial`,`vendanf`.`tributacao` */;

/*View structure for view blococregc300 */

/*!50001 DROP TABLE IF EXISTS `blococregc300` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc300` AS select `contdocs`.`ip` AS `ip`,`contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`Totalbruto` AS `Totalbruto`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,`contdocs`.`desconto` AS `desconto`,`contdocs`.`total` AS `total`,`contdocs`.`NrParcelas` AS `NrParcelas`,`contdocs`.`vendedor` AS `vendedor`,`contdocs`.`operador` AS `operador`,`contdocs`.`Observacao` AS `Observacao`,`contdocs`.`classe` AS `classe`,`contdocs`.`dataexe` AS `dataexe`,`contdocs`.`codigocliente` AS `codigocliente`,`contdocs`.`nome` AS `nome`,`contdocs`.`CodigoFilial` AS `CodigoFilial`,`contdocs`.`historico` AS `historico`,`contdocs`.`vrjuros` AS `vrjuros`,`contdocs`.`tipopagamento` AS `tipopagamento`,`contdocs`.`encargos` AS `encargos`,`contdocs`.`id` AS `id`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`enderecoentrega` AS `enderecoentrega`,`contdocs`.`custos` AS `custos`,`contdocs`.`devolucaovenda` AS `devolucaovenda`,`contdocs`.`devolucaorecebimento` AS `devolucaorecebimento`,`contdocs`.`nrboletobancario` AS `nrboletobancario`,`contdocs`.`nrnotafiscal` AS `nrnotafiscal`,`contdocs`.`classedevolucao` AS `classedevolucao`,`contdocs`.`responsavelreceber` AS `responsavelreceber`,`contdocs`.`numeroentrega` AS `numeroentrega`,`contdocs`.`cidadeentrega` AS `cidadeentrega`,`contdocs`.`cepentrega` AS `cepentrega`,`contdocs`.`bairroentrega` AS `bairroentrega`,`contdocs`.`horaentrega` AS `horaentrega`,`contdocs`.`dataentrega` AS `dataentrega`,`contdocs`.`obsentrega` AS `obsentrega`,`contdocs`.`concluido` AS `concluido`,`contdocs`.`cartaofidelidade` AS `cartaofidelidade`,`contdocs`.`bordero` AS `bordero`,`contdocs`.`valorservicos` AS `valorservicos`,`contdocs`.`descontoservicos` AS `descontoservicos`,`contdocs`.`romaneio` AS `romaneio`,`contdocs`.`hora` AS `hora`,`contdocs`.`entregaconcluida` AS `entregaconcluida`,`contdocs`.`dataentregaconcluida` AS `dataentregaconcluida`,`contdocs`.`operadorentrega` AS `operadorentrega`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`nreducaoz` AS `nreducaoz`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`TEF` AS `TEF`,`contdocs`.`ecfValorCancelamentos` AS `ecfValorCancelamentos`,`contdocs`.`NF_e` AS `NF_e`,`contdocs`.`estadoentrega` AS `estadoentrega`,`contdocs`.`ecfConsumidor` AS `ecfConsumidor`,`contdocs`.`ecfCPFCNPJconsumidor` AS `ecfCPFCNPJconsumidor`,`contdocs`.`ecfEndConsumidor` AS `ecfEndConsumidor`,`contdocs`.`prevendanumero` AS `prevendanumero`,`contdocs`.`ecfcontadorcupomfiscal` AS `ecfcontadorcupomfiscal`,`contdocs`.`ecftotalliquido` AS `ecftotalliquido`,`contdocs`.`contadornaofiscalGNF` AS `contadornaofiscalGNF`,`contdocs`.`contadordebitocreditoCDC` AS `contadordebitocreditoCDC`,`contdocs`.`totalICMScupomfiscal` AS `totalICMScupomfiscal`,`contdocs`.`troco` AS `troco`,`contdocs`.`davnumero` AS `davnumero`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfMFadicional` AS `ecfMFadicional`,`contdocs`.`ecftipo` AS `ecftipo`,`contdocs`.`ecfmarca` AS `ecfmarca`,`contdocs`.`ecfmodelo` AS `ecfmodelo`,`contdocs`.`estoqueatualizado` AS `estoqueatualizado`,`contdocs`.`serienf` AS `serienf`,`contdocs`.`EADRegistroDAV` AS `EADRegistroDAV`,`contdocs`.`EADr06` AS `EADr06`,`contdocs`.`tipopagamentoECF` AS `tipopagamentoECF`,`contdocs`.`modeloDOCFiscal` AS `modeloDOCFiscal`,`contdocs`.`subserienf` AS `subserienf`,sum(`contdocs`.`total`) AS `totalDocumento` from `contdocs` where ((`contdocs`.`modeloDOCFiscal` = '02') or (`contdocs`.`modeloDOCFiscal` = 'D1')) group by `contdocs`.`data`,`contdocs`.`serienf`,`contdocs`.`subserienf` */;

/*View structure for view blococregc320 */

/*!50001 DROP TABLE IF EXISTS `blococregc320` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc320` AS select `vendatmp`.`data` AS `data`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`serieNF` AS `serieNF`,`vendatmp`.`subserienf` AS `subserienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`ecfnumero` AS `ecfnumero`,`vendatmp`.`NotaFiscal` AS `NotaFiscal`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`total` AS `total`,sum(if((`vendatmp`.`icms` > 0),`vendatmp`.`total`,0)) AS `bcICMS`,((`vendatmp`.`total` * `vendatmp`.`icms`) / 100) AS `totalICMS` from `vendatmp` group by `vendatmp`.`tributacao`,`vendatmp`.`cfop`,`vendatmp`.`icms`,`vendatmp`.`data` union all select `venda`.`data` AS `data`,`venda`.`documento` AS `documento`,`venda`.`serieNF` AS `serieNF`,`venda`.`subserienf` AS `subserienf`,`venda`.`modelodocfiscal` AS `modelodocfiscal`,`venda`.`Ecfnumero` AS `ecfnumero`,`venda`.`NotaFiscal` AS `NotaFiscal`,`venda`.`tributacao` AS `tributacao`,`venda`.`cfop` AS `cfop`,`venda`.`icms` AS `icms`,sum(`venda`.`total`) AS `total`,sum(if((`venda`.`icms` > 0),`venda`.`total`,0)) AS `bcICMS`,((`venda`.`total` * `venda`.`icms`) / 100) AS `totalICMS` from `venda` group by `venda`.`tributacao`,`venda`.`cfop`,`venda`.`icms`,`venda`.`data` */;

/*View structure for view blococregc321 */

/*!50001 DROP TABLE IF EXISTS `blococregc321` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc321` AS select `venda`.`inc` AS `inc`,`venda`.`codigofilial` AS `codigofilial`,`venda`.`operador` AS `operador`,`venda`.`data` AS `data`,`venda`.`codigo` AS `codigo`,`venda`.`produto` AS `produto`,`venda`.`quantidade` AS `quantidade`,`venda`.`preco` AS `preco`,`venda`.`custo` AS `custo`,`venda`.`precooriginal` AS `precooriginal`,`venda`.`Descontoperc` AS `Descontoperc`,`venda`.`id` AS `id`,`venda`.`descontovalor` AS `descontovalor`,`venda`.`total` AS `total`,`venda`.`vendedor` AS `vendedor`,`venda`.`nrcontrole` AS `nrcontrole`,`venda`.`documento` AS `documento`,`venda`.`grupo` AS `grupo`,`venda`.`subgrupo` AS `subgrupo`,`venda`.`comissao` AS `comissao`,`venda`.`ratdesc` AS `ratdesc`,`venda`.`rateioencargos` AS `rateioencargos`,`venda`.`situacao` AS `situacao`,`venda`.`customedio` AS `customedio`,`venda`.`Ecfnumero` AS `Ecfnumero`,`venda`.`fornecedor` AS `fornecedor`,`venda`.`fabricante` AS `fabricante`,`venda`.`NotaFiscal` AS `NotaFiscal`,`venda`.`icms` AS `icms`,`venda`.`classe` AS `classe`,`venda`.`secao` AS `secao`,`venda`.`lote` AS `lote`,`venda`.`tributacao` AS `tributacao`,`venda`.`aentregar` AS `aentregar`,`venda`.`quantidadeanterior` AS `quantidadeanterior`,`venda`.`quantidadeatualizada` AS `quantidadeatualizada`,`venda`.`codigofiscal` AS `codigofiscal`,`venda`.`customedioanterior` AS `customedioanterior`,`venda`.`codigocliente` AS `codigocliente`,`venda`.`numerodevolucao` AS `numerodevolucao`,`venda`.`codigobarras` AS `codigobarras`,`venda`.`ipi` AS `ipi`,`venda`.`unidade` AS `unidade`,`venda`.`embalagem` AS `embalagem`,`venda`.`grade` AS `grade`,`venda`.`romaneio` AS `romaneio`,`venda`.`tipo` AS `tipo`,`venda`.`cofins` AS `cofins`,`venda`.`pis` AS `pis`,`venda`.`despesasacessorias` AS `despesasacessorias`,`venda`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`venda`.`serieNF` AS `serieNF`,`venda`.`subserienf` AS `subserienf`,`venda`.`cfop` AS `cfop`,`venda`.`acrescimototalitem` AS `acrescimototalitem`,`venda`.`cstpis` AS `cstpis`,`venda`.`cstcofins` AS `cstcofins`,`venda`.`icmsst` AS `icmsst`,`venda`.`percentualRedBaseCalcICMSST` AS `percentualRedBaseCalcICMSST`,`venda`.`mvast` AS `mvast`,`venda`.`modelodocfiscal` AS `modelodocfiscal`,sum(`venda`.`quantidade`) AS `somaQuantidade`,sum((`venda`.`total` - `venda`.`ratdesc`)) AS `totalItem`,sum(`venda`.`ratdesc`) AS `totalDesconto`,if((`venda`.`icms` > 0),sum(`venda`.`total`),0) AS `bcICMS`,if((`venda`.`icms` > 0),sum(((`venda`.`total` * `venda`.`icms`) / 100)),0) AS `totalICMS`,sum(`venda`.`pis`) AS `totalPIS`,sum(`venda`.`cofins`) AS `totalCOFINS` from `venda` where ((`venda`.`modelodocfiscal` = '02') or (`venda`.`modelodocfiscal` = 'D1')) group by `venda`.`codigo` union all select `vendatmp`.`inc` AS `inc`,`vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`operador` AS `operador`,`vendatmp`.`data` AS `data`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,`vendatmp`.`quantidade` AS `quantidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`custo` AS `custo`,`vendatmp`.`precooriginal` AS `precooriginal`,`vendatmp`.`Descontoperc` AS `Descontoperc`,`vendatmp`.`id` AS `id`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`total` AS `total`,`vendatmp`.`vendedor` AS `vendedor`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`grupo` AS `grupo`,`vendatmp`.`subgrupo` AS `subgrupo`,`vendatmp`.`comissao` AS `comissao`,`vendatmp`.`ratdesc` AS `ratdesc`,`vendatmp`.`rateioencargos` AS `rateioencargos`,`vendatmp`.`situacao` AS `situacao`,`vendatmp`.`customedio` AS `customedio`,`vendatmp`.`ecfnumero` AS `Ecfnumero`,`vendatmp`.`fornecedor` AS `fornecedor`,`vendatmp`.`fabricante` AS `fabricante`,`vendatmp`.`NotaFiscal` AS `NotaFiscal`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`classe` AS `classe`,`vendatmp`.`secao` AS `secao`,`vendatmp`.`lote` AS `lote`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`aentregar` AS `aentregar`,`vendatmp`.`quantidadeanterior` AS `quantidadeanterior`,`vendatmp`.`quantidadeatualizada` AS `quantidadeatualizada`,`vendatmp`.`codigofiscal` AS `codigofiscal`,`vendatmp`.`customedioanterior` AS `customedioanterior`,`vendatmp`.`codigocliente` AS `codigocliente`,`vendatmp`.`numerodevolucao` AS `numerodevolucao`,`vendatmp`.`codigobarras` AS `codigobarras`,`vendatmp`.`ipi` AS `ipi`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`embalagem` AS `embalagem`,`vendatmp`.`grade` AS `grade`,`vendatmp`.`romaneio` AS `romaneio`,`vendatmp`.`tipo` AS `tipo`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`despesasacessorias` AS `despesasacessorias`,`vendatmp`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`vendatmp`.`serieNF` AS `serieNF`,`vendatmp`.`subserienf` AS `subserienf`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`acrescimototalitem` AS `acrescimototalitem`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`icmsst` AS `icmsst`,`vendatmp`.`percentualRedBaseCalcICMSST` AS `percentualRedBaseCalcICMSST`,`vendatmp`.`mvast` AS `mvast`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,sum(`vendatmp`.`quantidade`) AS `somaQuantidade`,sum(`vendatmp`.`total`) AS `totalItem`,sum(`vendatmp`.`ratdesc`) AS `totalDesconto`,if((`vendatmp`.`icms` > 0),sum(`vendatmp`.`total`),0) AS `bcICMS`,if((`vendatmp`.`icms` > 0),sum(((`vendatmp`.`total` * `vendatmp`.`icms`) / 100)),0) AS `totalICMS`,sum(`vendatmp`.`pis`) AS `totalPIS`,sum(`vendatmp`.`cofins`) AS `totalCOFINS` from `vendatmp` where ((`vendatmp`.`modelodocfiscal` = '02') or (`vendatmp`.`modelodocfiscal` = 'D1')) group by `vendatmp`.`codigo` */;

/*View structure for view blococregc390 */

/*!50001 DROP TABLE IF EXISTS `blococregc390` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc390` AS select `venda`.`documento` AS `documento`,`venda`.`data` AS `data`,`venda`.`icms` AS `icms`,`venda`.`cfop` AS `cfop`,`venda`.`tributacao` AS `tributacao`,truncate(sum((`venda`.`total` - `venda`.`ratdesc`)),2) AS `total`,if((`venda`.`icms` > 0),truncate(sum((`venda`.`total` - `venda`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(`venda`.`total`) * `venda`.`icms`) / 100),2) AS `totalICMS` from `venda` where (`venda`.`quantidade` > 0) group by `venda`.`tributacao`,`venda`.`cfop`,`venda`.`icms`,`venda`.`documento` union all select `vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `data`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`tributacao` AS `tributacao`,truncate(sum(`vendatmp`.`total`),2) AS `total`,if((`vendatmp`.`icms` > 0),truncate(sum(`vendatmp`.`total`),2),0) AS `baseCalculoICMS`,truncate(((sum(`vendatmp`.`total`) * `vendatmp`.`icms`) / 100),2) AS `totalICMS` from `vendatmp` where (`vendatmp`.`quantidade` > 0) group by `vendatmp`.`tributacao`,`vendatmp`.`cfop`,`vendatmp`.`icms`,`vendatmp`.`documento` */;

/*View structure for view blococregc400 */

/*!50001 DROP TABLE IF EXISTS `blococregc400` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`127.0.0.1` SQL SECURITY DEFINER VIEW `blococregc400` AS select `r02`.`id` AS `id`,`r02`.`codigofilial` AS `codigofilial`,`r02`.`data` AS `data`,`r02`.`tipo` AS `tipo`,`r02`.`fabricacaoECF` AS `fabricacaoECF`,`r02`.`MFadicional` AS `MFadicional`,`r02`.`modeloECF` AS `modeloECF`,`r02`.`numeroUsuarioSubstituicaoECF` AS `numeroUsuarioSubstituicaoECF`,`r02`.`crz` AS `crz`,`r02`.`coo` AS `coo`,`r02`.`cro` AS `cro`,`r02`.`datamovimento` AS `datamovimento`,`r02`.`dataemissaoreducaoz` AS `dataemissaoreducaoz`,`r02`.`horaemissaoreducaoz` AS `horaemissaoreducaoz`,`r02`.`vendabrutadiaria` AS `vendabrutadiaria`,`r02`.`parametroISSQNdesconto` AS `parametroISSQNdesconto`,`r02`.`numeroECF` AS `numeroECF`,`r02`.`gtfinal` AS `gtfinal`,`r02`.`EADdados` AS `EADdados` from `r02` group by `r02`.`fabricacaoECF` */;

/*View structure for view blococregc425 */

/*!50001 DROP TABLE IF EXISTS `blococregc425` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc425` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(sum(`vendatmp`.`total`),2) AS `total`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`cancelado` AS `cancelado` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`vendatmp`.`quantidade` > 0)) group by `vendatmp`.`codigo` union all select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`venda`.`codigo` AS `codigo`,`venda`.`produto` AS `descricao`,sum(truncate(`venda`.`quantidade`,3)) AS `quantidade`,`venda`.`unidade` AS `unidade`,`venda`.`preco` AS `preco`,`venda`.`descontovalor` AS `descontovalor`,`venda`.`Descontoperc` AS `descontoperc`,truncate(sum(`venda`.`total`),2) AS `total`,`venda`.`icms` AS `icms`,`venda`.`tributacao` AS `tributacao`,`venda`.`cfop` AS `cfop`,`venda`.`cancelado` AS `cancelado` from (`contdocs` join `venda`) where ((`contdocs`.`documento` = `venda`.`documento`) and (`venda`.`quantidade` > 0)) group by `venda`.`codigo` order by `descricao` */;

/*View structure for view blococregc470 */

/*!50001 DROP TABLE IF EXISTS `blococregc470` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc470` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,truncate(`vendatmp`.`quantidade`,3) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2) AS `total`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc490 */

/*!50001 DROP TABLE IF EXISTS `blococregc490` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc490` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,truncate(`vendatmp`.`quantidade`,3) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2) AS `total`,if((`vendatmp`.`icms` > 0),truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`vendatmp`.`quantidade` > 0) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`cancelado` = 'N') and (`contdocs`.`modeloDOCFiscal` = '2D')) group by `vendatmp`.`tributacao`,`vendatmp`.`cfop`,`vendatmp`.`icms`,`vendatmp`.`data`,`vendatmp`.`modelodocfiscal` */;

/*View structure for view r05 */

/*!50001 DROP TABLE IF EXISTS `r05` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `r05` AS select ifnull(`venda`.`coo`,'') AS `ncupomfiscal`,ifnull(`venda`.`ccf`,'') AS `ecfcontadorcupomfiscal`,`contdocs`.`data` AS `data`,`contdocs`.`documento` AS `documento`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,`venda`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfMFadicional` AS `ecfMFadicional`,`contdocs`.`ecfmodelo` AS `ecfmodelo`,`venda`.`nrcontrole` AS `nrcontrole`,`venda`.`codigo` AS `codigo`,`venda`.`produto` AS `produto`,`venda`.`quantidade` AS `quantidade`,`venda`.`unidade` AS `unidade`,`venda`.`precooriginal` AS `precooriginal`,`venda`.`descontovalor` AS `descontovalor`,`venda`.`preco` AS `preco`,`venda`.`total` AS `total`,`venda`.`icms` AS `icms`,`venda`.`tributacao` AS `tributacao`,`venda`.`cancelado` AS `cancelado`,`venda`.`ccf` AS `ccf`,`venda`.`coo` AS `coo`,`venda`.`acrescimototalitem` AS `acrescimototalitem`,`venda`.`Descontoperc` AS `Descontoperc`,`venda`.`eaddados` AS `eaddados`,`produtos`.`indicadorproducao` AS `indicadorproducao`,`produtos`.`indicadorarredondamentotruncamento` AS `indicadorarredondamentotruncamento` from ((`contdocs` join `venda`) join `produtos`) where ((`venda`.`documento` = `contdocs`.`documento`) and (`produtos`.`codigo` = `venda`.`codigo`)) union all select ifnull(`vendatmp`.`coo`,'') AS `ncupomfiscal`,ifnull(`vendatmp`.`ccf`,'') AS `ecfcontadorcupomfiscal`,`contdocs`.`data` AS `data`,`contdocs`.`documento` AS `documento`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfMFadicional` AS `ecfMFadicional`,`contdocs`.`ecfmodelo` AS `ecfmodelo`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,`vendatmp`.`quantidade` AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`precooriginal` AS `precooriginal`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`total` AS `total`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cancelado` AS `cancelado`,`vendatmp`.`ccf` AS `ccf`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`acrescimototalitem` AS `acrescimototalitem`,`vendatmp`.`Descontoperc` AS `Descontoperc`,`vendatmp`.`eaddados` AS `eaddados`,`produtos`.`indicadorproducao` AS `indicadorproducao`,`produtos`.`indicadorarredondamentotruncamento` AS `indicadorarredondamentotruncamento` from ((`contdocs` join `vendatmp`) join `produtos`) where ((`vendatmp`.`documento` = `contdocs`.`documento`) and (`produtos`.`codigo` = `vendatmp`.`codigo`) and (`contdocs`.`documento` <> '') and (`produtos`.`CodigoFilial` = `contdocs`.`CodigoFilial`) and (`vendatmp`.`ecfnumero` <> '')) */;

/*View structure for view registro50entradas_agr */

/*!50001 DROP TABLE IF EXISTS `registro50entradas_agr` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50entradas_agr` AS select `entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`bcicms`) AS `bcicms`,round(sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),2) AS `toticms`,sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) AS `ipiItem`,sum(`entradas`.`totalitem`) AS `totalProduto`,(sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`totalitem`)) AS `totalNF`,`entradas`.`Lancada` AS `lancada` from `entradas` group by `entradas`.`NF`,`entradas`.`cfopentrada`,`entradas`.`IcmsEntrada`,`entradas`.`numero` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view registro50entradas_itens */

/*!50001 DROP TABLE IF EXISTS `registro50entradas_itens` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50entradas_itens` AS select `entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`bcicms`) AS `bcicms`,round(sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),2) AS `toticms`,sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) AS `ipiItem`,sum(`entradas`.`totalitem`) AS `totalProduto`,(sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`totalitem`)) AS `totalNF` from `entradas` group by `entradas`.`NF`,`entradas`.`inc` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view registro50saida_agr */

/*!50001 DROP TABLE IF EXISTS `registro50saida_agr` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50saida_agr` AS select `vendanf`.`codigofilial` AS `codigofilial`,`vendanf`.`NotaFiscal` AS `notafiscal`,`vendanf`.`serieNF` AS `serienf`,`vendanf`.`modelodocfiscal` AS `modelodocfiscal`,`vendanf`.`documento` AS `documento`,`vendanf`.`data` AS `DATA`,`vendanf`.`cfop` AS `cfop`,`vendanf`.`icms` AS `icms`,`vendanf`.`tributacao` AS `tributacao`,`vendanf`.`codigo` AS `codigo`,`vendanf`.`produto` AS `produto`,sum(`vendanf`.`quantidade`) AS `SUM(quantidade)`,`vendanf`.`unidade` AS `unidade`,`vendanf`.`nrcontrole` AS `nrcontrole`,sum(round((((`vendanf`.`total` - ((`vendanf`.`total` * `vendanf`.`percentualRedBaseCalcICMS`) / 100)) * `vendanf`.`icms`) / 100),2)) AS `totalicms`,sum(`vendanf`.`descontovalor`) AS `descontovalor`,sum(`vendanf`.`total`) AS `SUM(TOTAL)`,if((`vendanf`.`icms` > 0),sum(round((`vendanf`.`total` - (`vendanf`.`total` * (`vendanf`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendanf` group by `vendanf`.`NotaFiscal`,`vendanf`.`serieNF`,`vendanf`.`icms`,`vendanf`.`cfop`,`vendanf`.`codigofilial` */;

/*View structure for view registro50saidas_itens */

/*!50001 DROP TABLE IF EXISTS `registro50saidas_itens` */;
/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50saidas_itens` AS select `vendanf`.`inc` AS `inc`,`vendanf`.`codigofilial` AS `codigofilial`,`vendanf`.`NotaFiscal` AS `notafiscal`,`vendanf`.`serieNF` AS `serienf`,`vendanf`.`modelodocfiscal` AS `modelodocfiscal`,`vendanf`.`documento` AS `documento`,`vendanf`.`data` AS `DATA`,`vendanf`.`cfop` AS `cfop`,`vendanf`.`icms` AS `icms`,`vendanf`.`tributacao` AS `tributacao`,`vendanf`.`codigo` AS `codigo`,`vendanf`.`produto` AS `produto`,sum(`vendanf`.`quantidade`) AS `SUM(quantidade)`,`vendanf`.`unidade` AS `unidade`,`vendanf`.`nrcontrole` AS `nrcontrole`,sum(round((((`vendanf`.`total` - ((`vendanf`.`total` * `vendanf`.`percentualRedBaseCalcICMS`) / 100)) * `vendanf`.`icms`) / 100),2)) AS `totalicms`,sum(`vendanf`.`descontovalor`) AS `descontovalor`,sum(`vendanf`.`total`) AS `SUM(TOTAL)`,if((`vendanf`.`icms` > 0),sum(round((`vendanf`.`total` - (`vendanf`.`total` * (`vendanf`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendanf` group by `vendanf`.`inc` order by `vendanf`.`NotaFiscal`,`vendanf`.`nrcontrole` */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

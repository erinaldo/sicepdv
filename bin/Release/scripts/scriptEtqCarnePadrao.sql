/*
SQLyog Enterprise v8.71 
MySQL - 5.5.8 : Database - sice
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
/*Table structure for table `geradordetalhe` */

DROP TABLE IF EXISTS `geradordetalhe`;

CREATE TABLE `geradordetalhe` (
  `inc` int(11) NOT NULL AUTO_INCREMENT,
  `relatorio` int(5) DEFAULT NULL,
  `linha` decimal(6,3) DEFAULT '0.000',
  `coluna` decimal(6,3) DEFAULT '0.000',
  `tabela` char(30) DEFAULT NULL,
  `campo` char(50) DEFAULT NULL,
  `mascara` char(20) DEFAULT NULL,
  `tabela2` char(30) DEFAULT NULL,
  `campo2` char(50) DEFAULT NULL,
  `mascara2` char(20) DEFAULT NULL,
  `tabela3` char(30) DEFAULT NULL,
  `campo3` char(50) DEFAULT NULL,
  `mascara3` char(20) DEFAULT NULL,
  `tipocampo` char(20) DEFAULT NULL,
  `fonte` char(40) DEFAULT 'NORMAL',
  `fonteTamanho` int(2) DEFAULT '0',
  PRIMARY KEY (`inc`)
) ENGINE=MyISAM AUTO_INCREMENT=39 DEFAULT CHARSET=latin1;

/*Data for the table `geradordetalhe` */

insert  into `geradordetalhe`(`inc`,`relatorio`,`linha`,`coluna`,`tabela`,`campo`,`mascara`,`tabela2`,`campo2`,`mascara2`,`tabela3`,`campo3`,`mascara3`,`tipocampo`,`fonte`,`fonteTamanho`) values (1,1,'4.000','6.000','CLIENTES','CODIGO','','TEXTO','-','','CLIENTES','nome','','INT','NORMAL',0),(2,1,'4.000','47.000','CLIENTES','CODIGO','','TEXTO','-','','CLIENTES','nome','','VAR','NORMAL',0),(3,1,'6.000','6.000','CAIXA','DATA','','','','','','','','DAT','COMP17',0),(4,1,'6.000','47.000','CAIXA','DATA','','','','','','','','DAT','COMP17',0),(5,1,'6.000','17.000','CAIXA','DOCUMENTO','','','','','','','','VAR','NORMAL',0),(6,1,'6.000','57.000','CAIXA','DOCUMENTO','','','','','','','','VAR','NORMAL',0),(7,1,'6.000','27.000','CAIXA','VALOR','###,##0.00','','','','','','','DEC','COMP12',0),(8,1,'6.000','67.000','CAIXA','VALOR','###,##0.00','','','','','','','DEC','COMP12+NEGRITO',0),(9,1,'8.000','6.000','CAIXA','VENCIMENTO','','','','','','','','DAT','COMP12',0),(10,1,'8.000','46.000','CAIXA','VENCIMENTO','','','','','','','','DAT','COMP12',0),(11,1,'8.000','18.000','CAIXA','NRPARCELA','','','','','','','','VAR','NORMAL',0),(12,1,'8.000','57.000','CAIXA','NRPARCELA','','','','','','','','VAR','NORMAL',0),(25,1,'12.000','47.000','FILIAIS','CIDADE','','TEXTO','-','','FILIAIS','TELEFONE1','','VAR','NORMAL',0),(24,1,'12.000','7.000','FILIAIS','CIDADE','','TEXTO','-','','FILIAIS','TELEFONE1','','VAR','NORMAL',0),(15,1,'10.000','6.000','CLIENTES','CODIGO','','TEXTO','','','TEXTO','','','INT','NORMAL',0),(16,1,'10.000','47.000','CLIENTES','CODIGO','','TEXTO','','','TEXTO','','','INT','NORMAL',0),(27,2,'0.200','0.500','PRODUTOS','CODIGO','','TEXTO','-','','PRODUTOS','DESCRICAO','','VAR','IMPACT',12),(20,1,'2.000','14.000','FILIAIS','CIDADE','','TEXTO','  -','','FILIAIS','TELEFONE1','','VAR','NORMAL',0),(21,1,'2.000','53.000','FILIAIS','CIDADE','','TEXTO','  -','','FILIAIS','TELEFONE1','','VAR','NORMAL',0),(22,1,'10.000','16.000','TEXTO','ENTRADA','','','','','','','','','',8),(23,1,'10.000','56.000','TEXTO','ENTRADA','','','','','','','','','',8),(29,1,'17.000','5.000','TEXTO','.','','','','','','','','','NORMAL',0),(35,2,'1.000','0.500','PRODUTOS','CODIGOBARRAS','||BARRAS||','','','','','','','VAR','IMPACT',10),(36,2,'2.000','0.500','PRODUTOS','CODIGOBARRAS','','','','','','','','VAR','IMPACT',8),(37,2,'0.700','0.500','PRODUTOS','PRECOVENDA','###,##0.00','','','','','','','DEC','IMPACT',14),(38,2,'2.500','0.500','TEXTO','TESTE TEXTO','','','','','','','','VAR','ARIAL',8);

/*Table structure for table `geradormaster` */

DROP TABLE IF EXISTS `geradormaster`;

CREATE TABLE `geradormaster` (
  `id` int(5) NOT NULL AUTO_INCREMENT,
  `tipoRelatorio` char(10) DEFAULT NULL,
  `tamanhoLargura` decimal(6,3) DEFAULT NULL,
  `tamanhoAltura` decimal(6,3) DEFAULT NULL,
  `nomeRelatorio` char(20) DEFAULT NULL,
  `MargemEsquerda` decimal(6,3) DEFAULT '0.000',
  `MargemSuperior` decimal(6,3) DEFAULT '0.000',
  `MargemInferior` decimal(6,3) DEFAULT '0.000',
  `EtiquetaLinhas` decimal(6,3) DEFAULT '0.000',
  `EtiquetaColunas` decimal(6,3) DEFAULT '0.000',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Data for the table `geradormaster` */

insert  into `geradormaster`(`id`,`tipoRelatorio`,`tamanhoLargura`,`tamanhoAltura`,`nomeRelatorio`,`MargemEsquerda`,`MargemSuperior`,`MargemInferior`,`EtiquetaLinhas`,`EtiquetaColunas`) values (1,'Carnê','80.000','15.000','CARNÊ PADRAO','0.000','0.000','0.000','0.000','0.000'),(2,'Etiqueta','6.000','3.500','ETIQUETA 6CMX3,5CM','0.100','0.200','0.500','6.000','1.000');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

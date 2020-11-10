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
/*!50106 set global event_scheduler = 1*/;

/* Event structure for event `e_analiseGerencial` */

/*!50106 DROP EVENT IF EXISTS `e_analiseGerencial`*/;

DELIMITER $$

/*!50106 CREATE DEFINER=`root`@`%` EVENT `e_analiseGerencial` ON SCHEDULE EVERY 1 HOUR STARTS '2012-08-16 13:27:05' ON COMPLETION PRESERVE ENABLE DO BEGIN 
   
    DECLARE fil VARCHAR(5);
    DECLARE i, t INT;
    
    DECLARE listaFiliais CURSOR FOR SELECT codigofilial FROM configfinanc WHERE ativarassistentegerencial = 'S';
    DECLARE totalFiliais CURSOR FOR SELECT COUNT(codigofilial) FROM configfinanc WHERE ativarassistentegerencial = 'S';
    
    SET i = 1;    
    
    
    OPEN listaFiliais;
    OPEN totalFiliais;
    FETCH totalFiliais INTO t;
    
    WHILE i <= t DO
     
       FETCH listaFiliais INTO fil;    
    
        CALL sp_gravaInfoGerencial(fil) ; 
         
        SET i = i+1;
    END WHILE ;
    
    SELECT fil;
    CLOSE listaFiliais;
    CLOSE totalFiliais;
    
  END */$$
DELIMITER ;

/* Function  structure for function  `fCodigoEstadoMunIBGE` */

/*!50003 DROP FUNCTION IF EXISTS `fCodigoEstadoMunIBGE` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fCodigoEstadoMunIBGE`(tipo char(1),cidade varchar(40),siglaUF varchar(2)) RETURNS varchar(100) CHARSET latin1
    DETERMINISTIC
BEGIN
 if (tipo="M") then
  return (select a.id
    from tab_municipios as a, estados as b 
    where a.iduf=b.id and a.nome=cidade and b.uf=siglaUF);
 end if;
 
 if (tipo="E") then
 return (select id from estados 
     where uf=siglaUF);
 end if;
 
 return "";
    END */$$
DELIMITER ;

/* Function  structure for function  `fObterMVA` */

/*!50003 DROP FUNCTION IF EXISTS `fObterMVA` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fObterMVA`(uf VARCHAR(2), ncm VARCHAR(8), pICMSEntrada DECIMAL(8,3)) RETURNS double
    DETERMINISTIC
BEGIN
RETURN(
SELECT 
CASE pICMSEntrada
      WHEN '4.00' THEN aliquota4
      WHEN '7.00' THEN aliquota7
      WHEN '12.00' THEN aliquota12
      WHEN '17.00' THEN aliquotaimportacao
 ELSE
	0	
END FROM tabelastncm WHERE estado=uf AND nbmsh LIKE CONCAT('%', MID(ncm, 1, 4), '%') LIMIT 1
);
END */$$
DELIMITER ;

/* Function  structure for function  `fPesoBrutoVenda` */

/*!50003 DROP FUNCTION IF EXISTS `fPesoBrutoVenda` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fPesoBrutoVenda`(filial varchar(5),ipTerminal varchar(15)) RETURNS double
    DETERMINISTIC
BEGIN
 
 if (filial="00001") then
 return (SELECT IFNULL( SUM(p.pesobruto * v.quantidade),0 ) AS bruto
FROM  vendas AS v, produtos AS p
 WHERE v.id = ipTerminal 
 AND v.codigo = p.codigo
 AND v.codigofilial=filial);
 end if;
 
  IF (filial<>"00001") THEN
 RETURN (SELECT IFNULL( SUM(p.pesobruto * v.quantidade),0 ) AS bruto
FROM  vendas AS v, produtosfilial AS p
 WHERE v.id = ipTerminal 
 AND v.codigo = p.codigo
 AND v.codigofilial=filial);
 END IF;
 
 return 0;
    
END */$$
DELIMITER ;

/* Function  structure for function  `fpesoLiquidoVenda` */

/*!50003 DROP FUNCTION IF EXISTS `fpesoLiquidoVenda` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fpesoLiquidoVenda`(filial varchar(5),ipTerminal varchar(15)) RETURNS double
    DETERMINISTIC
BEGIN
 
 if (filial="00001") then
 return (SELECT IFNULL( SUM(p.pesoliquido * v.quantidade),0 ) AS bruto
FROM  vendas AS v, produtos AS p
 WHERE v.id = ipTerminal 
 AND v.codigo = p.codigo
 AND v.codigofilial=filial);
 end if;
 
  IF (filial<>"00001") THEN
 RETURN (SELECT IFNULL( SUM(p.pesoliquido * v.quantidade),0 ) AS bruto
FROM  vendas AS v, produtosfilial AS p
 WHERE v.id = ipTerminal 
 AND v.codigo = p.codigo
 AND v.codigofilial=filial);
 END IF;
 
 return 0;
    
END */$$
DELIMITER ;

/* Function  structure for function  `fSaldoCliente` */

/*!50003 DROP FUNCTION IF EXISTS `fSaldoCliente` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fSaldoCliente`(iddocumento INT(6),
  idcliente INT (6),
  ipTerminal VARCHAR (15),
 tabela VARCHAR(15)
) RETURNS double
    DETERMINISTIC
BEGIN
IF (tabela='vendas') THEN
  RETURN (
    SELECT SUM(clientes.saldo - 
    (SELECT 
      SUM(vendas.total - vendas.ratdesc + vendas.rateioencargos) 
    FROM
      vendas 
    WHERE vendas.id = ipTerminal 
      AND cancelado = "N") ) FROM clientes WHERE clientes.Codigo = idcliente
  ) ;
 END IF;
 
 IF (tabela='vendaprevenda') THEN
  RETURN (
    SELECT SUM(clientes.saldo - 
    (SELECT 
      SUM(vendaprevenda.total - vendaprevenda.ratdesc + vendaprevenda.rateioencargos) 
    FROM
      vendaprevenda
    WHERE vendaprevenda.documento = iddocumento
      AND cancelado = "N") ) FROM clientes WHERE clientes.Codigo = idcliente
  ) ;
 END IF;
 
END */$$
DELIMITER ;

/* Function  structure for function  `fvBCICMS` */

/*!50003 DROP FUNCTION IF EXISTS `fvBCICMS` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvBCICMS`(filial VARCHAR(5),ipTerminal VARCHAR(15), idproduto VARCHAR(10), controle int(5) ) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 select 
 TRUNCATE((ratfrete + ratdespesas + ratseguro - ratdesc + total) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1), 2) 
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.codigo = idproduto
 AND v.cancelado = "N"  
 AND v.inc = controle 
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvBCICMSst` */

/*!50003 DROP FUNCTION IF EXISTS `fvBCICMSst` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvBCICMSst`(filial VARCHAR(5),ipTerminal VARCHAR(15), idproduto VARCHAR(10), controle int(5) ) RETURNS double
    DETERMINISTIC
BEGIN
 RETURN (
 SELECT 
 if((tributacao="10" OR tributacao="70" OR tributacao="110" OR tributacao="210"),
 TRUNCATE( 
 (((v.ratfrete + v.ratdespesas + v.ratseguro - v.ratdesc + fvIPI(filial, ipterminal, idproduto, controle) + v.total)*(v.mvast/100))+(v.ratfrete + v.ratdespesas + v.ratseguro - v.ratdesc + fvIPI(filial, ipterminal, idproduto, controle) + v.total))
 * IF(v.percentualRedBaseCalcICMSST>0, ((100-v.percentualRedBaseCalcICMSST)/100), 1)
 , 2 ), 0)  
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 and v.codigo=idproduto 
 and v.inc=controle 
 AND v.cancelado = "N"  
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvBCIPI` */

/*!50003 DROP FUNCTION IF EXISTS `fvBCIPI` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvBCIPI`(filial VARCHAR(5),ipTerminal VARCHAR(15), idproduto VARCHAR(10), controle int(5) ) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 select 
 TRUNCATE(
 
 IF( (SELECT crt FROM filiais WHERE codigofilial=filial) ="1", 0.00, IF(aliquotaipi>0 OR vUnidIPI>0, (ratfrete + ratdespesas + ratseguro - ratdesc + total),0) )
 
 , 2) 
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.codigo = idproduto
 AND v.cancelado = "N"  
 AND v.inc = controle 
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvICMS` */

/*!50003 DROP FUNCTION IF EXISTS `fvICMS` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvICMS`(filial VARCHAR(5),ipTerminal VARCHAR(15), idproduto VARCHAR(10), controle int(5) ) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 SELECT 
 truncate( 
 ((ratfrete + ratdespesas + ratseguro - ratdesc + total) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1) )*(icms/100)
 
  , 2)   
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.codigo = idproduto
 AND v.cancelado = "N"  
 AND v.inc = controle 
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvICMSst` */

/*!50003 DROP FUNCTION IF EXISTS `fvICMSst` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvICMSst`(filial VARCHAR(5),ipTerminal VARCHAR(15), codigo VARCHAR(10), controle int(5) ) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE vICMSst REAL DEFAULT 0;	   
DECLARE cursorvICMSst CURSOR FOR SELECT 
 IF((tributacao="10" OR tributacao="70" OR tributacao="110" OR tributacao="210"),
 TRUNCATE((fvBCICMSst(filial, ipTerminal, codigo, inc)*(icmsst/100))- fvICMS(filial, ipTerminal, codigo, inc), 2), 0)    
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.codigo = codigo
 and v.cancelado = "N" 
 AND v.inc = controle 
 AND v.codigofilial=filial;
 
OPEN cursorvICMSst;
FETCH cursorvICMSst INTO vICMSst;
 
RETURN (SELECT IF(vICMSst>0, vICMSst, 0));
 
    END */$$
DELIMITER ;

/* Function  structure for function  `fvIPI` */

/*!50003 DROP FUNCTION IF EXISTS `fvIPI` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvIPI`(filial VARCHAR(5),ipTerminal VARCHAR(15), idproduto VARCHAR(10), controle int(5) ) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 select 
 TRUNCATE(
 
 IF( (SELECT crt FROM filiais WHERE codigofilial=filial) ="1", 0.00, IF(aliquotaipi>0 OR vUnidIPI>0,(ratfrete + ratdespesas + ratseguro - ratdesc + total),0) ) * (aliquotaipi/100)
 
 , 2) 
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.codigo = idproduto
 AND v.cancelado = "N"  
 AND v.inc = controle 
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvTotalBCICMSnfe` */

/*!50003 DROP FUNCTION IF EXISTS `fvTotalBCICMSnfe` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvTotalBCICMSnfe`(filial VARCHAR(5),ipTerminal VARCHAR(15)) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 select  
 sum(
 IF(icms>0,
 TRUNCATE((ratfrete + ratdespesas + ratseguro - ratdesc + total) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1), 2)
 , 0)) 
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.cancelado = "N"  
 AND v.codigofilial=filial
 );
    END */$$
DELIMITER ;

/* Function  structure for function  `fvTotalBCICMSSTnfe` */

/*!50003 DROP FUNCTION IF EXISTS `fvTotalBCICMSSTnfe` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvTotalBCICMSSTnfe`(filial VARCHAR(5), ipTerminal VARCHAR(15) ) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN ( 
 SELECT 
 SUM(
 IF((tributacao="10" OR tributacao="70" OR tributacao="110" OR tributacao="210"),
 TRUNCATE( 
 (((v.ratfrete + v.ratdespesas + v.ratseguro - v.ratdesc + (v.total*v.aliquotaipi/100) + v.total)*(v.mvast/100))+(v.ratfrete + v.ratdespesas + v.ratseguro - v.ratdesc + (v.total*v.aliquotaipi/100) + v.total))
 * IF(v.percentualRedBaseCalcICMSST>0, ((100-v.percentualRedBaseCalcICMSST)/100), 1)
 , 2 ), 0)  
 )
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.cancelado = "N"  
 AND v.icmsst > 0
 AND v.codigofilial=codigofilial 
 );
 
END */$$
DELIMITER ;

/* Function  structure for function  `fvTotalICMSnfe` */

/*!50003 DROP FUNCTION IF EXISTS `fvTotalICMSnfe` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvTotalICMSnfe`(filial VARCHAR(5),ipTerminal VARCHAR(15)) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 SELECT 
 SUM(
 TRUNCATE( 
 ((ratfrete + ratdespesas + ratseguro - ratdesc + total) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1) )*(icms/100)
 
  , 2 )   
 )
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.cancelado = "N"  
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvTotalICMSSTnfe` */

/*!50003 DROP FUNCTION IF EXISTS `fvTotalICMSSTnfe` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvTotalICMSSTnfe`(filial VARCHAR(5),ipTerminal VARCHAR(15)) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE totalICMSSTnfe REAL DEFAULT 0;	   
 DECLARE cursorTotal CURSOR FOR SELECT 
 ifnull(
 SUM(
 TRUNCATE(
 IF(v.mvast>0,
 (((((v.ratfrete + v.ratdespesas + v.ratseguro - v.ratdesc + (v.total*v.aliquotaipi/100) + v.total)*(v.mvast/100))+
 (v.ratfrete + v.ratdespesas + v.ratseguro - v.ratdesc + (v.total*v.aliquotaipi/100) + v.total))*
 ((100-v.percentualRedBaseCalcICMSST)/100))*(icmsst/100))-
 (((ratfrete + ratdespesas + ratseguro - ratdesc + total) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1) )*(icms/100)), 0), 2)), 0)  
 FROM  vendas AS v 
 WHERE v.id = ipTerminal
 AND v.cancelado = "N"  
 AND v.codigofilial=filial;
 
 OPEN cursorTotal;
 FETCH cursorTotal INTO totalICMSSTnfe;
 
 return (SELECT IF(totalICMSSTnfe>0, totalICMSSTnfe, 0));
END */$$
DELIMITER ;

/* Function  structure for function  `fvTotalIPInfe` */

/*!50003 DROP FUNCTION IF EXISTS `fvTotalIPInfe` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `fvTotalIPInfe`(filial VARCHAR(5),ipTerminal VARCHAR(15)) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 select 
 TRUNCATE(
 Sum(
 IF( (SELECT crt FROM filiais WHERE codigofilial=filial) ="1", 0.00, IF(aliquotaipi>0 OR vUnidIPI>0,total,0) ) * (aliquotaipi/100)
 )
 , 2) 
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.cancelado = "N"  
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `f_ticketMedio` */

/*!50003 DROP FUNCTION IF EXISTS `f_ticketMedio` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `f_ticketMedio`( filial VARCHAR(5) ) RETURNS double
    DETERMINISTIC
BEGIN
	
	DECLARE X INT DEFAULT 0;
	DECLARE Y DOUBLE DEFAULT 0;
	
	SET Y = (SELECT (SUM(total) - SUM(ratdesc)) FROM venda WHERE DATA = CURRENT_DATE AND codigofilial = filial AND cancelado = "N");
	SET X = (SELECT COUNT(1)  FROM caixa WHERE dpfinanceiro IN ("venda", "crediario") AND codigofilial = filial AND DATA = CURRENT_DATE );
	RETURN  Y / X ;
	
    END */$$
DELIMITER ;

/* Function  structure for function  `f_totalOcorrenciasAuditoria` */

/*!50003 DROP FUNCTION IF EXISTS `f_totalOcorrenciasAuditoria` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `f_totalOcorrenciasAuditoria`( filial VARCHAR(5), tipo VARCHAR(3)  ) RETURNS int(11)
    DETERMINISTIC
BEGIN
     IF (tipo = "tot") THEN	
      RETURN (SELECT COUNT(1) FROM auditoria WHERE DATA = CURRENT_DATE AND codigofilial = filial); 
     END IF;
     
     IF (tipo = "cli") THEN
      RETURN (SELECT COUNT(1) FROM auditoria WHERE DATA = CURRENT_DATE AND codigofilial = filial AND tabela LIKE "cli%" ); 
     END IF; 
     IF (tipo = "pro") THEN
      RETURN (SELECT COUNT(1) FROM auditoria WHERE DATA = CURRENT_DATE AND codigofilial = filial AND tabela LIKE 'pro%' ); 
     END IF; 
     IF (tipo = "ven" ) THEN
      RETURN (SELECT COUNT(1) FROM auditoria WHERE DATA = CURRENT_DATE AND codigofilial = filial AND tabela LIKE 'ven%' ); 
     END IF; 
     IF (tipo = "pag") THEN
      RETURN (SELECT COUNT(1) FROM auditoria WHERE DATA = CURRENT_DATE AND codigofilial = filial AND tabela LIKE 'contas%');
     END IF; 
     IF (tipo = "ace" ) THEN
      RETURN (SELECT COUNT(1) FROM auditoria WHERE DATA = CURRENT_DATE AND codigofilial = filial AND tabela LIKE 'ace%' ); 
     END IF; 
     IF tipo = ("est") THEN
      RETURN (SELECT COUNT(1) FROM auditoria WHERE DATA = CURRENT_DATE AND codigofilial = filial AND tabela LIKE 'cli%' AND acao LIKE 'est%' );
     END IF; 
 
    RETURN 0;
 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AberturaDia` */

/*!50003 DROP PROCEDURE IF EXISTS  `AberturaDia` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AberturaDia`(IN filial VARCHAR(5),in dataAtual date,in hora time,in nrFabricacaoECF varchar(20))
BEGIN
UPDATE produtos SET datafinalestoque=dataAtual,horafinalestoque=hora,ecffabricacao=nrFabricacaoECF where  codigofilial=filial;
UPDATE produtosfilial SET datafinalestoque=dataAtual,horafinalestoque=hora,ecffabricacao=nrFabricacaoECF where codigofilial=filial;
  
UPDATE produtos SET saldofinalestoque=quantidade,EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,datafinalestoque,horafinalestoque,ecffabricacao)),EADP2relacaomercadoria=MD5(CONCAT(codigo,descricao,tributacao,icms,precovenda,precoatacado)) where codigofilial=filial;
UPDATE produtosfilial SET saldofinalestoque=quantidade,EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,datafinalestoque,horafinalestoque,ecffabricacao)),EADP2relacaomercadoria=MD5(CONCAT(codigo,descricao,tributacao,icms,precovenda,precoatacado)) where codigofilial=filial; 
  
UPDATE produtos SET eade1=MD5(CONCAT(horafinalestoque,ecffabricacao)) WHERE codigofilial=filial;
UPDATE produtosfilial SET eade1=MD5(CONCAT(datafinalestoque,horafinalestoque,ecffabricacao)) WHERE codigofilial=filial; 
call CriarInventario(filial,"");
call AtualizarQdtRegistros(); 
 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AjustarCamposNulos` */

/*!50003 DROP PROCEDURE IF EXISTS  `AjustarCamposNulos` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AjustarCamposNulos`()
BEGIN
UPDATE produtos SET aceitadesconto="S" WHERE aceitadesconto="" OR aceitadesconto=NULL;
UPDATE produtosfilial SET aceitadesconto="S" WHERE aceitadesconto="" OR aceitadesconto=NULL;
update produtos set customedio=0 where customedio is null;
UPDATE produtosfilial SET customedio=0 WHERE customedio IS NULL;
UPDATE produtos SET unidade="" WHERE unidade IS NULL;
UPDATE produtos SET ncm="" WHERE ncm IS NULL;
UPDATE fornecedores SET cpf="" WHERE cpf IS NULL;
UPDATE fornecedores SET cgc="" WHERE CGC IS NULL;
UPDATE fornecedores SET telefone="" WHERE telefone IS NULL;
UPDATE fornecedores SET fax="" WHERE fax IS NULL;
UPDATE fornecedores SET email="" WHERE email IS NULL;
UPDATE fornecedores SET inscricao="" WHERE inscricao IS NULL;
UPDATE fornecedores SET cep="" WHERE cep IS NULL;
UPDATE produtos SET ncm="" WHERE ncm IS NULL;
UPDATE produtosfilial SET ncm="" WHERE ncm IS NULL;
UPDATE produtos SET nbm="" WHERE nbm IS NULL;
UPDATE produtosfilial SET nbm="" WHERE nbm IS NULL;
UPDATE produtos SET ncmespecie="" WHERE ncmespecie IS NULL;
UPDATE produtosfilial SET ncmespecie="" WHERE ncmespecie IS NULL;
UPDATE cartoes SET `pathresp`='c:\\tef_dial\\resp' WHERE `pathresp`='';
UPDATE cartoes  SET `pathreq`='c:\\tef_dial\\req' WHERE `pathreq`='';
UPDATE produtos SET tipo="0 - Produto" WHERE tipo="" OR tipo IS NULL;
UPDATE produtosfilial SET tipo="0 - Produto" WHERE tipo="" OR tipo IS NULL;
UPDATE filiais SET cpfresponsavel="" WHERE cpfresponsavel IS NULL;
UPDATE contdocs SET ecfcpfcnpjconsumidor="" WHERE ecfcpfcnpjconsumidor IS NULL AND DATA=CURRENT_DATE;
UPDATE contdocs SET ecfmfadicional="" WHERE ecfmfadicional IS NULL AND DATA=CURRENT_DATE;
UPDATE contdocs SET ecfconsumidor="" WHERE ecfconsumidor IS NULL AND DATA=CURRENT_DATE;
UPDATE contdocs SET ecfmodelo="" WHERE ecfmodelo IS NULL and data=current_date;
UPDATE contdocs SET ecffabricacao="" WHERE ecffabricacao IS NULL AND DATA=CURRENT_DATE;
UPDATE venda SET coo="" WHERE coo IS NULL ;
UPDATE venda SET ccf="" WHERE ccf IS NULL;
UPDATE venda SET ecffabricacao=" " WHERE ecffabricacao IS NULL;
update filiais set complemento="sem comp" where complemento is null;
UPDATE produtos SET ncm = "", ncmespecie = "" WHERE ncm IS NULL;
UPDATE produtosfilial SET ncm = "", ncmespecie = "" WHERE ncm IS NULL;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AjustarCamposNulosManual` */

/*!50003 DROP PROCEDURE IF EXISTS  `AjustarCamposNulosManual` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AjustarCamposNulosManual`(in filial varchar(5))
BEGIN
UPDATE clientes SET cpf="" WHERE cpf IS NULL;
UPDATE clientes SET cnpj="" WHERE cnpj IS NULL;
UPDATE clientes SET endereco="" WHERE endereco IS NULL;
UPDATE clientes SET debito=0 WHERE (debito IS NULL or debito<0);
UPDATE produtos SET ncm="" WHERE ncm LIKE "%A%";
UPDATE produtos SET ncm="" WHERE ncm is null;
UPDATE produtos SET unidade="UNI" WHERE unidade="0KG";
UPDATE produtos SET unidade="UNI" WHERE unidade="0PC";
UPDATE produtos SET unidade="UNI" WHERE unidade="UND";
UPDATE produtos SET unidade="UNI" WHERE unidade="YND";
UPDATE produtos SET unidade="M2" WHERE unidade="MT2";
UPDATE produtos SET unidade="M3" WHERE unidade="MT3";
UPDATE produtos SET unidade="UNI" WHERE unidade="UNS";
UPDATE produtos SET unidade="UNI" WHERE unidade="CAR";
UPDATE produtos SET unidade="UNI" WHERE unidade="Un.";
UPDATE produtos SET unidade="M" WHERE unidade="MT";
update produtos set unidade="MIL" where unidade="MI";
UPDATE produtosfilial SET unidade="UNI" WHERE unidade="0KG";
UPDATE produtosfilial SET unidade="UNI" WHERE unidade="0PC";
UPDATE produtosfilial SET unidade="UNI" WHERE unidade="UND";
UPDATE produtosfilial SET unidade="UNI" WHERE unidade="YND";
UPDATE produtosfilial SET unidade="M2" WHERE unidade="MT2";
UPDATE produtosfilial SET unidade="M3" WHERE unidade="MT3";
UPDATE produtosfilial SET unidade="UNI" WHERE unidade="UNS";
UPDATE produtosfilial SET unidade="UNI" WHERE unidade="CAR";
UPDATE produtosfilial SET unidade="UNI" WHERE unidade="Un.";
UPDATE produtosfilial SET unidade="UNI" WHERE unidade="";
UPDATE produtosfilial SET unidade="M" WHERE unidade="MT";
UPDATE produtosfilial SET unidade="MIL" WHERE unidade="MI";
UPDATE produtosfilial SET ncm="" WHERE ncm IS NULL;
UPDATE vendaarquivo SET unidade="UNI" WHERE unidade="0PC";
UPDATE vendaarquivo SET unidade="UNI" WHERE unidade="0KG";
UPDATE vendaarquivo SET unidade="M3" WHERE unidade="MT3";
UPDATE vendaarquivo SET unidade="UNI" WHERE unidade="UNS";
UPDATE vendaarquivo SET unidade="UNI" WHERE unidade="";
UPDATE vendaarquivo SET unidade="M2" WHERE unidade="MT2";
UPDATE vendaarquivo SET unidade="UNI" WHERE unidade="YND";
UPDATE vendaarquivo SET unidade="UNI" WHERE unidade="UND";
UPDATE vendaarquivo SET unidade="UNI" WHERE unidade="CAR";
UPDATE vendaarquivo SET unidade="UNI" WHERE unidade="Un.";
UPDATE vendaarquivo SET unidade="M" WHERE unidade="MT";
UPDATE vendaarquivo SET unidade="MIL" WHERE unidade="MI";
UPDATE vendanf SET unidade="UNI" WHERE unidade="0PC";
UPDATE vendanf SET unidade="UNI" WHERE unidade="0KG";
UPDATE vendanf SET unidade="M3" WHERE unidade="MT3";
UPDATE vendanf SET unidade="UNI" WHERE unidade="UNS";
UPDATE vendanf SET unidade="M2" WHERE unidade="MT2";
UPDATE vendanf SET unidade="UNI" WHERE unidade="YND";
UPDATE vendanf SET unidade="UNI" WHERE unidade="UND";
UPDATE vendanf SET unidade="UNI" WHERE unidade="CAR";
UPDATE vendanf SET unidade="UNI" WHERE unidade="Un.";
UPDATE vendanf SET unidade="UNI" WHERE unidade="";
UPDATE vendanf SET unidade="MIL" WHERE unidade="MI";
UPDATE vendanf SET unidade="M" WHERE unidade="MT";
UPDATE entradas SET unidade="UNI" WHERE unidade="0KG";
UPDATE entradas SET unidade="UNI" WHERE unidade="0PC";
UPDATE entradas SET unidade="UNI" WHERE unidade="UNS";
UPDATE entradas SET unidade="M2" WHERE unidade="MT2";
UPDATE entradas SET unidade="UNI" WHERE unidade="YND";
UPDATE entradas SET unidade="UNI" WHERE unidade="UND";
UPDATE entradas SET unidade="UNI" WHERE unidade="Un.";
UPDATE entradas SET unidade="UNI" WHERE unidade="CAR";
update entradas set unidade="M" where unidade="MT";
UPDATE entradas SET unidade="UNI" WHERE unidade="";
UPDATE entradas SET unidade="UNI" WHERE unidade="0UN";
update entradas set cstpis="50",cstcofins="50",pis=1.65,cofins=7.60 where cstpis="01";
UPDATE entradas SET icmsentrada=ifnull((valoricms/totalitem)*100,0)  WHERE tributacao="000" AND icmsentrada=0 AND bcicms>0 AND valoricms>0; 
update entradas set cstpis="73",cstcofins="73",pis=0,cofins=0 where cstpis="73";
UPDATE entradas SET unidade="UNI" WHERE unidade="1";
update vendaarquivo set rateioencargos=0 where rateioencargos<0;
update vendaarquivo set icms=0 where tributacao="60";
UPDATE produtos SET icms=0 WHERE tributacao="60";
UPDATE produtosfilial SET icms=0 WHERE tributacao="60";
UPDATE produtos SET icms=0 WHERE tributacao="40";
UPDATE produtosfilial SET icms=0 WHERE tributacao="40";
UPDATE produtos SET situacaoinventario="00" WHERE situacaoinventario="";
UPDATE produtosfilial SET situacaoinventario="00" WHERE situacaoinventario="";
UPDATE produtos SET situacaoinventario="00" WHERE situacaoinventario is null;
UPDATE produtosfilial SET situacaoinventario="00" WHERE situacaoinventario is null;
UPDATE produtos SET situacaoinventario="00" WHERE left(tipo,1)="0";
UPDATE produtosfilial SET situacaoinventario="09" WHERE LEFT(tipo,1)="1";
UPDATE produtosfilial SET situacaoinventario="01" WHERE LEFT(tipo,1)="2";
UPDATE produtos SET tributacao="60" WHERE tributacao="06";
UPDATE produtosfilial SET tributacao="60" WHERE tributacao="06";
UPDATE produtosinventario SET tributacao="60" WHERE tributacao="06";
UPDATE vendaarquivo SET pis=0,cofins=0 WHERE pis IS NULL;
UPDATE vendaarquivo SET pis=0,cofins=0 WHERE cofins IS NULL;
UPDATE entradas,moventradas 
SET entradas.cfopentrada=moventradas.cfopentrada 
WHERE entradas.numero=moventradas.numero
AND entradas.cfopentrada IS NULL;
UPDATE entradas,moventradas SET entradas.dataentrada = moventradas.dataEntrada,
entradas.NF = moventradas.NF
WHERE entradas.numero = moventradas.numero;
UPDATE entradas,moventradas SET entradas.modeloNF=moventradas.modeloNF
WHERE entradas.numero=moventradas.numero;
UPDATE moventradas SET DataEmissao=dataEntrada WHERE dataemissao>dataentrada;
UPDATE moventradas,fornecedores SET
moventradas.codigofornecedor=fornecedores.Codigo
WHERE moventradas.fornecedor=fornecedores.razaosocial
AND moventradas.codigofornecedor IS NULL;
update moventradas set exportarfiscal="N" where codigofornecedor is null;
UPDATE contnfsaida SET codcliente=0 WHERE codfornecedor=codcliente;
update fornecedores set numero="" where numero is null;
update produtos set codigosuspensaocofins=codigosuspensaopis where codigosuspensaocofins="000" and (tributacaopis="04" or tributacaopis="06");
UPDATE produtosfilial SET codigosuspensaocofins=codigosuspensaopis WHERE codigosuspensaocofins="000" AND (tributacaopis="04" OR tributacaopis="06");
IF ( (SELECT left(filiais.tipoempresa,1) FROM filiais where codigofilial=filial LIMIT 1)="3") THEN
UPDATE produtos SET pis=1.65 WHERE tributacaoPIS="01"  AND codigofilial=filial ;
UPDATE produtos SET cofins=7.60 WHERE tributacaoCOFINS="01"  AND codigofilial=filial;
update produtos set pisentrada=1.65 where cstpisentrada="50"  AND codigofilial=filial;
UPDATE produtos SET cofinsentrada=7.60 WHERE cstcofinsentrada="50"  AND codigofilial=filial;
UPDATE produtosfilial SET pisentrada=1.65 WHERE cstpisentrada="50"  AND codigofilial=filial;
UPDATE produtosfilial SET cofinsentrada=7.60 WHERE cstcofinsentrada="50"  AND codigofilial=filial;
UPDATE produtosfilial SET pis=1.65 WHERE tributacaoPIS="01"  AND codigofilial=filial ;
UPDATE produtosfilial SET cofins=7.6 WHERE tributacaoCOFINS="01"  AND codigofilial=filial ;
UPDATE entradas SET pis=1.65 WHERE cstpis="01"  AND codigofilial=filial;
UPDATE entradas SET cofins=7.6 WHERE cstcofins="01"  AND codigofilial=filial;
UPDATE entradas SET cstpis="50",pis=1.65,cofins=7.60 WHERE cstpis="01"  AND codigofilial=filial;
UPDATE entradas SET cstcofins="50",pis=1.65,cofins=7.60 WHERE cstcofins="01"  AND codigofilial=filial;
UPDATE entradas SET pis=1.65,cofins=7.60 WHERE cstpis="50"  AND codigofilial=filial;
UPDATE entradas SET pis=1.65,cofins=7.60 WHERE cstcofins="50"  AND codigofilial=filial;
UPDATE vendaarquivo SET pis=1.65 WHERE (cstpis="01" or cstpis="50")  AND codigofilial=filial ;
UPDATE vendaarquivo SET cofins=7.6 WHERE (cstcofins="01" or cstcofins="50")  AND codigofilial=filial ;
UPDATE vendanf SET pis=1.65 WHERE (cstpis="01" or cstpis="50")   AND codigofilial=filial ;
UPDATE vendanf SET cofins=7.6 WHERE (cstcofins="01" or cstcofins="50")  AND codigofilial=filial;  
UPDATE produtos SET cstpisentrada="50",cstcofinsentrada="50",pisentrada=1.65,cofinsentrada=7.60 WHERE cstpisentrada="01"  AND codigofilial=filial;
UPDATE produtosfilial SET cstpisentrada="50",cstcofinsentrada="50",pisentrada=1.65,cofinsentrada=7.60 WHERE cstpisentrada="01"  AND codigofilial=filial;
update vendanf set pis=1.65,cofins=7.60 where cstcofins="01"  AND codigofilial=filial; 
UPDATE vendanf SET pis=1.65,cofins=7.60 WHERE cstcofins="50"  AND codigofilial=filial;
UPDATE entradas,produtos SET entradas.cstpis=produtos.cstpisEntrada,
entradas.cstcofins=produtos.cstcofinsEntrada,entradas.pis=produtos.pisentrada,
entradas.cofins=produtos.cofinsentrada 
WHERE entradas.codigo=produtos.codigo
AND entradas.codigofilial=produtos.CodigoFilial AND entradas.cstpis="01"
AND YEAR(entradas.DATA)>="2012"  AND entradas.codigofilial=filial;
END IF;
IF ( (SELECT LEFT(filiais.tipoempresa,1) FROM filiais WHERE codigofilial=filial LIMIT 1)="1") THEN
UPDATE produtos SET pis=0.65 WHERE tributacaoPIS="01" and codigofilial=filial;
UPDATE produtos SET cofins=3 WHERE tributacaoCOFINS="01"  AND codigofilial=filial;
UPDATE produtos SET pisentrada=0.65 WHERE cstpisentrada="50"  AND codigofilial=filial;
UPDATE produtos SET cofinsentrada=3 WHERE cstcofinsentrada="50"  AND codigofilial=filial;
UPDATE produtosfilial SET pisentrada=0.65 WHERE cstpisentrada="50"  AND codigofilial=filial;
UPDATE produtosfilial SET cofinsentrada=3 WHERE cstcofinsentrada="50"  AND codigofilial=filial;
UPDATE produtosfilial SET pis=0.65 WHERE tributacaoPIS="01"  AND codigofilial=filial;
UPDATE produtosfilial SET cofins=3 WHERE tributacaoCOFINS="01"  AND codigofilial=filial ;
UPDATE entradas SET pis=0.65 WHERE cstpis="01"  AND codigofilial=filial;
UPDATE entradas SET cofins=3 WHERE cstcofins="01"  AND codigofilial=filial;
UPDATE entradas SET cstpis="50",pis=0.65,cofins=3 WHERE cstpis="01"  AND codigofilial=filial;
UPDATE entradas SET cstcofins="50",pis=0.65,cofins=3 WHERE cstcofins="01"  AND codigofilial=filial;
UPDATE entradas SET pis=0.65,cofins=3 WHERE cstpis="50"  AND codigofilial=filial;
UPDATE entradas SET pis=0.65,cofins=3 WHERE cstcofins="50"  AND codigofilial=filial;
UPDATE vendaarquivo SET pis=0.65 WHERE (cstpis="01" OR cstpis="50")  AND codigofilial=filial  ;
UPDATE vendaarquivo SET cofins=3 WHERE (cstcofins="01" OR cstcofins="50")  AND codigofilial=filial ;
UPDATE vendanf SET pis=0.65 WHERE (cstpis="01" OR cstpis="50")  AND codigofilial=filial  ;
UPDATE vendanf SET cofins=3 WHERE (cstcofins="01" OR cstcofins="50")  AND codigofilial=filial;  
UPDATE produtos SET cstpisentrada="50",cstcofinsentrada="50",pisentrada=0.65,cofinsentrada=3 WHERE cstpisentrada="01"  AND codigofilial=filial;
UPDATE produtosfilial SET cstpisentrada="50",cstcofinsentrada="50",pisentrada=0.65,cofinsentrada=3 WHERE cstpisentrada="01"  AND codigofilial=filial;
UPDATE vendanf SET pis=0.65,cofins=3 WHERE cstcofins="01"  AND codigofilial=filial; 
UPDATE vendanf SET pis=0.65,cofins=3 WHERE cstcofins="50"  AND codigofilial=filial;
UPDATE entradas,produtos SET entradas.cstpis=produtos.cstpisEntrada,
entradas.cstcofins=produtos.cstcofinsEntrada,entradas.pis=produtos.pisentrada,
entradas.cofins=produtos.cofinsentrada 
WHERE entradas.codigo=produtos.codigo
AND entradas.codigofilial=produtos.CodigoFilial AND entradas.cstpis="01"
AND YEAR(entradas.DATA)>="2012"  AND entradas.codigofilial=filial;
end if;
UPDATE entradas SET percentualRedBaseCalcICMS=1 WHERE tributacao="070" AND entradas.percentualRedBaseCalcICMS=0;
UPDATE entradas SET percentualRedBaseCalcICMS=1 WHERE tributacao="020" AND entradas.percentualRedBaseCalcICMS=0;
UPDATE clientes SET datacadastro=CURRENT_DATE WHERE datacadastro IS NULL;
UPDATE clientes SET ultcompra=datacadastro WHERE ultcompra IS NULL;
UPDATE clientes SET saldo=0 WHERE saldo IS NULL;
 
 UPDATE vendaarquivo SET tributacao="60" WHERE tributacao="06";
 UPDATE vendanf SET tributacao="60" WHERE tributacao="06";
 UPDATE vendanf set tributacao="00" where tributacao="" or tributacao is null; 
 
 update filiais set cpfresponsavel="" where cpfresponsavel is null;
 DELETE FROM produtos WHERE codigo="";
 DELETE FROM produtosfilial WHERE codigo="";
 update moventradas set codigofornecedor=(select codigo from fornecedores where empresa=moventradas.fornecedor limit 1) 
where moventradas.codigofornecedor=0;
UPDATE contdocs SET estornado="N" WHERE estornado=" ";
update moventradas set tipofrete="9" where (tipofrete=" " or tipofrete is null);
UPDATE moventradas SET cfopentrada="1.102" WHERE (cfopentrada="" or cfopentrada is null or left(cfopentrada,1)="5");
update moventradas set exportarfiscal="N" where nf="";
delete from vendanf where quantidade=0;
UPDATE vendanf SET tributacao="00" WHERE tributacao="0";
update produtos set cstpisentrada="50",cstcofinsentrada="50" where cstpisentrada="01";
UPDATE produtos SET cstpisentrada="50",cstcofinsentrada="50" WHERE cstpisentrada="01";
update fornecedores set cidade="CABO DE SANTO AGOSTINHO" where cidade="CABO" and estado="PE";
update entradas set icmsentrada=17 where icmsentrada>27;
UPDATE entradas SET cfopentrada="1.102" WHERE (cfopentrada="" OR cfopentrada IS NULL OR LEFT(cfopentrada,1)="5");
UPDATE entradas SET cstpis="98",cstcofins="98",pis=0,cofins=0 WHERE cfopentrada="1.910";
update entradas set cstpis="70",cstcofins="70",pis=0,cofins=0 WHERE cstpis="04";
UPDATE entradas SET cstpis="70",cstcofins="70",pis=0,cofins=0 WHERE cstpis="05";
UPDATE entradas SET cstpis="70",cstcofins="70",pis=0,cofins=0 WHERE cstpis="06";
UPDATE entradas SET cstpis="70",cstcofins="70",pis=0,cofins=0 WHERE cstpis="0";
update entradas set cfopentrada="2.102" where (cfopentrada="6101" or cfopentrada="6.101");
update entradas set icmsentrada=round(icmsentrada);
UPDATE vendaarquivo SET tributacao="20" WHERE tributacao="02";
UPDATE vendaarquivo SET tributacao="60",icms=0 WHERE tributacao="10" AND modelodocfiscal="2D";
UPDATE vendaarquivo SET tributacao="00" WHERE tributacao="20" AND modelodocfiscal="2D";
UPDATE vendaarquivo SET tributacao="00" WHERE tributacao="0";
UPDATE vendanf SET tributacao="20" WHERE tributacao="02";
update moventradas set chave_nfe="" where chave_nfe is null;
UPDATE contnfsaida SET chave_nfe="" WHERE chave_nfe IS NULL;
UPDATE vendaarquivo SET tributacao="60",icms=0 WHERE tributacao="03" AND modelodocfiscal="2D";
UPDATE vendaarquivo SET tributacao="60",icms=0 WHERE tributacao="30" AND modelodocfiscal="2D";
UPDATE vendaarquivo SET tributacao="40",icms=0 WHERE tributacao="04" AND modelodocfiscal="2D"; 
UPDATE vendaarquivo SET tributacao="80",icms=0 WHERE tributacao="08" AND modelodocfiscal="2D";
UPDATE vendaarquivo SET tributacao="60",icms=0 WHERE tributacao="70" AND modelodocfiscal="2D";
UPDATE vendaarquivo SET tributacao="60",icms=0 WHERE tributacao="07" AND modelodocfiscal="2D";
UPDATE produtos SET tributacao="30",icms=0 WHERE tributacao="03";
update produtos set tributacao="40",icms=0 where tributacao="04";
UPDATE produtos SET tributacao="50",icms=0 WHERE tributacao="05";
UPDATE produtos SET tributacao="70",icms=0 WHERE tributacao="07";
UPDATE produtos SET tributacao="80",icms=0 WHERE tributacao="08";
UPDATE produtosfilial SET tributacao="30",icms=0 WHERE tributacao="03";
UPDATE produtosfilial SET tributacao="40",icms=0 WHERE tributacao="04";
UPDATE produtosfilial SET tributacao="50",icms=0 WHERE tributacao="05";
UPDATE produtosfilial SET tributacao="70",icms=0 WHERE tributacao="07";
UPDATE produtosfilial SET tributacao="80",icms=0 WHERE tributacao="08";
update entradas set tributacao="000" where tributacao="00";
update entradas set bcicms=0 where icmsentrada=0 and dataentrada>"2012-06-19";
update entradas set cfopentrada="1.102" where cfopentrada="6102";
update entradas set cstcofins=cstpis where cstcofins="7.";
update moventradas set exportarfiscal="N" where nf="0";
UPDATE entradas SET nf=RIGHT(nf,6) WHERE LEFT(nf,3)="000";
UPDATE moventradas SET nf=RIGHT(nf,6) WHERE LEFT(nf,3)="000";
UPDATE entradas SET cstcofins=cstpis WHERE cstpis<>cstcofins;
 UPDATE vendanf,contnfsaida SET vendanf.serienf=contnfsaida.serie
WHERE contnfsaida.cfop='5.929'
and vendanf.NotaFiscal=contnfsaida.notafiscal
AND vendanf.codigofilial=contnfsaida.codigofilial
AND YEAR(contnfsaida.DATA)>="2012"
AND YEAR(vendanf.DATA)>="2012"
AND vendanf.DATA=contnfsaida.DATA;
update vendanf set icms=0 where cfop="5.929";
UPDATE entradas SET cfopentrada="1.102" WHERE cfopentrada="1102";
UPDATE entradas SET cfopentrada="1.102"
WHERE LEFT(cfopentrada,1)="5";
UPDATE entradas SET cfopentrada="2.102"
WHERE LEFT(cfopentrada,1)="6";
UPDATE 
    vendanf AS tabelaVenda,
    produtos 
  SET
    tabelaVenda.cstpis = produtos.cstpisEntrada,
    tabelaVenda.pis = produtos.pisentrada,
    tabelaVenda.cstcofins = produtos.cstcofinsEntrada,
    tabelaVenda.cofins = produtos.cofinsentrada 
  WHERE tabelaVenda.codigo = produtos.codigo 
    AND tabelaVenda.codigofilial = produtos.CodigoFilial
 AND tabelaVenda.cfop="1.202" AND tabelaVenda.cstpis="04";
 
 UPDATE 
    vendanf AS tabelaVenda,
    produtos 
  SET
    tabelaVenda.cstpis = produtos.cstpisEntrada,
    tabelaVenda.pis = produtos.pisentrada,
    tabelaVenda.cstcofins = produtos.cstcofinsEntrada,
    tabelaVenda.cofins = produtos.cofinsentrada 
  WHERE tabelaVenda.codigo = produtos.codigo 
    AND tabelaVenda.codigofilial = produtos.CodigoFilial
 AND tabelaVenda.cfop="1.202" AND tabelaVenda.cstpis="01";
 
 UPDATE entradas SET cfopentrada="1.152" WHERE cfopentrada="1152";
 
 UPDATE vendanf SET codigofilial="00001" WHERE codigofilial IS NULL;
 update contnfsaida set serie=abs(serie);
 update vendanf set serienf=abs(serienf);
 
 UPDATE entradas SET icmsst=0,bcicmsst=0,entradas.valoricmsST=0 WHERE tributacao<>"010" AND tributacao<>"030" AND tributacao<>"070";
 
  
UPDATE moventradas AS m, fornecedores AS f, entradas AS e 
SET e.cfopentrada=CONCAT('2.', MID(e.cfopentrada, 3, 4))  
WHERE m.codigofornecedor=f.codigo AND e.numero=m.numero
AND f.estado<> (SELECT estado FROM filiais WHERE codigofilial=filial LIMIT 1)
AND m.dataentrada>'2012-07-01'
AND MID(e.cfopentrada, 1, 1)='1';
CREATE TEMPORARY TABLE `produtos_tmp` ( PRIMARY KEY(`codigoinc`),KEY `PrdCodigo`( `codigo` ), KEY `descricao`( `descricao` ), KEY `codigobarras`( `codigobarras` ), KEY `documento`( `documento` ))ENGINE=MYISAM  COLLATE = latin1_swedish_ci COMMENT = '' SELECT `id`, `codigo`, `codigoinc`, `descecf`, `unidade`, `quantidade`, `qtddeposito`, `qtdprateleiras`, `qtdvencidos`, `qtdanterior`, `qtdultent`, `dataultent`, `qtdprovisoria`, `descprovisorio`, `pedidoand`, `qtdultpedido`, `data`, `dataultpedido`, `icms`, `ipi`, `grupo`, `subgrupo`, `custo`, `customedio`, `ultcusto`, `custototal`, `margemlucro`, `precovenda`, `dataultvenda`, `dataaltpreco`, `ultpreco`, `estminimo`, `codigobarras`, `situacao`, `tributacao`, `fornecedor`, `fabricante`, `tipocomissao`, `datacadastro`, `validade`, `aceitadesconto`, `descontopromocao`, `validadepromoc`, `descontomaximo`, `operador`, `pesobruto`, `pesoliquido`, `marcado`, `embalagem`, `unidembalagem`, `localestoque`, `descricao`, `frete`, `01qtd`, `01custos`, `01vendas`, `02qtd`, `02custos`, `02vendas`, `03qtd`, `03custos`, `03vendas`, `04qtd`, `04custos`, `04vendas`, `05qtd`, `05custos`, `05vendas`, `06qtd`, `06custos`, `06vendas`, `07qtd`, `07custos`, `07vendas`, `08qtd`, `08custos`, `08vendas`, `09qtd`, `09custos`, `09vendas`, `10qtd`, `10custos`, `10vendas`, `11qtd`, `11custos`, `11vendas`, `12qtd`, `12custos`, `12vendas`, `CodigoFilial`, `codigovinculado`, `inventario`, `documento`, `classe`, `qtdretida`, `secao`, `diasparavencimento`, `lote`, `vencimento`, `anoinventario`, `inventarioencerrado`, `dataencerramentoinventario`, `qtdaentregar`, `precounidade`, `generico`, `princativo`, `margemsemfinanciamento`, `precosemfinanciamento`, `codigofiscal`, `customedioanterior`, `ativacompdesc`, `inventarionumero`, `custofornecedor`, `qtdminimadesc`, `qtdprevenda`, `parcelamentomax`, `precoatacado`, `grade`, `detalhetecnico`, `origem`, `modalidadeDetBaseCalcICMS`, `percentualRedBaseCalcICMS`, `modalidadedetBaseCalcICMsST`, `ICMsST`, `percentualRedICMsST`, `percentualMargVlrAdICMsST`, `tipo`, `pis`, `cofins`, `despesasacessorias`, `margemlucroliquida`, `volumes`, `ncm`, `nbm`, `ncmespecie`, `capacidadevolML`, `situacaoinventario`, `tributacaoPIS`, `tributacaoCOFINS`, `codigoservico`, `aliquotaISS`, `indicadorproducao`, `indicadorarredondamentotruncamento`, `cfopsaida`, `cfopentrada`, `EADE2mercadoriaEstoque`, `EADP2relacaomercadoria`, `aliquotaIPI`, `datafabricacao`, `saldofinalestoque`, `datafinalestoque`, `horafinalestoque`, `ecffabricacao`, `complementodescricao`, `pcredsn`, `cstpisEntrada`, `cstcofinsEntrada`, `pisentrada`, `cofinsentrada`, `codigosuspensaopis`, `codigosuspensaocofins`, `vendainternet`, `cstipi`, `EADE1` FROM `produtos`;
UPDATE produtos AS a SET a.ncm=(SELECT b.ncm FROM produtos_tmp AS b WHERE b.ncm<>'' AND b.grupo=a.grupo LIMIT 1) WHERE a.ncm='';
UPDATE produtos SET ncmespecie = MID(ncm, 1, 2) WHERE nbm='';
UPDATE filiais SET responsavel='' WHERE responsavel IS NULL;
UPDATE filiais SET emailcontador='' WHERE emailcontador IS NULL;
UPDATE filiais SET cpfcontador='' WHERE cpfcontador IS NULL;
UPDATE filiais SET cnpjcontador='' WHERE cnpjcontador IS NULL;
UPDATE filiais SET cepcontador='' WHERE cepcontador IS NULL;
UPDATE filiais SET enderecocontador='' WHERE enderecocontador IS NULL;
UPDATE filiais SET numerocontador='' WHERE numerocontador IS NULL;
UPDATE entradas SET pis = '0',cofins = '0',cstpis = '98', cstcofins = '98' WHERE cfopentrada = '1.910';
UPDATE entradas SET pis = '0',cofins = '0',cstpis = '98', cstcofins = '98' WHERE cfopentrada = '1.556';
UPDATE entradas SET pis = '0',cofins = '0',cstpis = '98', cstcofins = '98' WHERE cfopentrada = '1.906';
UPDATE clientes SET dataalteracaosenha='1899-12-30 11:11:11' WHERE dataalteracaosenha IS NULL;
UPDATE clientes SET dataultcarta='1899-12-30' WHERE dataultcarta IS NULL;
UPDATE clientes SET ultvencimento='1899-12-30' WHERE ultvencimento IS NULL; 
UPDATE produtos SET ncm = "", ncmespecie = "" WHERE ncm IS NULL;
UPDATE produtosfilial SET ncm = "", ncmespecie = "" WHERE ncm IS NULL;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AjustarTributos` */

/*!50003 DROP PROCEDURE IF EXISTS  `AjustarTributos` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AjustarTributos`(in acao varchar (20))
BEGIN
  UPDATE 
    moventradas 
  SET
    exportarfiscal = "N" 
  WHERE moventradas.codigofornecedor = 
    (SELECT 
      fornecedores.Codigo 
    FROM
      fornecedores 
    WHERE fornecedores.Codigo = moventradas.codigofornecedor 
      AND LENGTH(TRIM(fornecedores.CPF)) = 11 
    LIMIT 1) and moventradas.modeloNF="55" ;
  UPDATE 
    moventradas 
  SET
    exportarfiscal = "N" 
  WHERE moventradas.codigofornecedor = 
    (SELECT 
      fornecedores.Codigo 
    FROM
      fornecedores 
    WHERE fornecedores.Codigo = moventradas.codigofornecedor 
      AND LENGTH(TRIM(fornecedores.CPF)) = 11 
    LIMIT 1) ;
  if (acao = "ajustarPISCOFINS") THEN
  UPDATE produtos SET tributacaocofins=tributacaopis WHERE tributacaopis<>tributacaocofins;
  update produtos set codigosuspensaocofins=codigosuspensaopis;
  UPDATE produtosfilial SET codigosuspensaocofins=codigosuspensaopis;
 
   UPDATE 
    vendaarquivo AS tabelaVenda,
    produtos 
  SET
    tabelaVenda.cstpis = produtos.tributacaoPIS,
    tabelaVenda.pis = produtos.PIS,
    tabelaVenda.cstcofins = produtos.tributacaoCOFINS,
    tabelaVenda.cofins = produtos.COFINS 
  WHERE tabelaVenda.codigo = produtos.codigo 
    AND tabelaVenda.codigofilial = produtos.CodigoFilial ;
 
 
  UPDATE 
    vendanf AS tabelaVenda,
    produtos 
  SET
    tabelaVenda.cstpis = produtos.tributacaoPIS,
    tabelaVenda.pis = produtos.PIS,
    tabelaVenda.cstcofins = produtos.tributacaoCOFINS,
    tabelaVenda.cofins = produtos.COFINS 
  WHERE tabelaVenda.codigo = produtos.codigo 
    AND tabelaVenda.codigofilial = produtos.CodigoFilial ;
UPDATE vendanf AS v, produtos AS p
SET v.cstpis = p.cstpisEntrada,
v.cstcofins = p.cstcofinsEntrada,
v.pis = p.pisentrada,
v.cofins = p.cofinsentrada
WHERE v.codigo = p.codigo
AND MID(cfop,1,1) < 4; 
  UPDATE 
    entradas,
    produtos 
  SET
    entradas.cstpis = produtos.cstpisEntrada,
    entradas.pis = produtos.pisentrada,
    entradas.cstcofins = produtos.cstcofinsEntrada,
    entradas.cofins = produtos.cofinsentrada 
  WHERE entradas.codigo = produtos.codigo 
    AND entradas.codigofilial = produtos.CodigoFilial ;
 
  end if ;
END */$$
DELIMITER ;

/* Procedure structure for procedure `AjusteRemovidos` */

/*!50003 DROP PROCEDURE IF EXISTS  `AjusteRemovidos` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AjusteRemovidos`()
BEGIN
 
 UPDATE vendanf,contnfsaida SET vendanf.serienf=contnfsaida.serie
WHERE vendanf.NotaFiscal=contnfsaida.notafiscal
AND vendanf.codigofilial=contnfsaida.codigofilial
AND YEAR(contnfsaida.DATA)>="2012"
AND YEAR(vendanf.DATA)>="2012"
and vendanf.data=contnfsaida.data;
 UPDATE contdocs,vendaarquivo 
 SET contdocs.DATA=vendaarquivo.DATA
 WHERE contdocs.DATA = "0001-01-01"
 AND contdocs.documento = vendaarquivo.documento;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `arquivomorto` */

/*!50003 DROP PROCEDURE IF EXISTS  `arquivomorto` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `arquivomorto`(in tabela varchar(30))
BEGIN
DECLARE qtdRegistroOrigemCaixa INT DEFAULT 0;
DECLARE qtdRegistroDestinoCaixa INT DEFAULT 0;
declare qtdRegistroDestinoCaixaRepassado int default 0;
SET qtdRegistroOrigemCaixa =(SELECT COUNT(1) FROM caixaarquivo WHERE year(DATA)<=YEAR(CURRENT_DATE))-1 ;
SET qtdRegistroDestinoCaixa = (SELECT COUNT(1) FROM caixaarquivo_arq WHERE year(DATA)<=YEAR(CURRENT_DATE))-1;
IF (tabela="caixa") THEN
	INSERT INTO caixaarquivo_arq SELECT * FROM caixaarquivo WHERE year(DATA)<=YEAR(CURRENT_DATE)-1;
	SET qtdRegistroDestinoCaixaRepassado = (SELECT COUNT(1) FROM caixaarquivo_arq WHERE YEAR(DATA)<=YEAR(CURRENT_DATE))-1;
	 IF ( qtdRegistroDestinoCaixaRepassado>qtdRegistroDestinoCaixa ) THEN 
		DELETE FROM caixaarquivo WHERE year(DATA)<=YEAR(CURRENT_DATE)-1;
	  END IF; 
END IF;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarContRelGerencial` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarContRelGerencial` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AtualizarContRelGerencial`(in cooRG varchar(6))
BEGIN
 UPDATE contrelatoriogerencial
SET contrelatoriogerencial.EADDados= MD5(CONCAT( ecffabricacao,coo,gnf, denominacao,DATA,cdc,denominacao))
where coo=cooRG;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarDadosOff` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarDadosOff` */;

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
 
 UPDATE venda SET coo="" WHERE coo IS NULL;
 UPDATE venda SET ccf="" WHERE ccf IS NULL;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarDebitoCliente` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarDebitoCliente` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AtualizarDebitoCliente`(in codCliente int,in taxaJuros double,in filial varchar(5))
BEGIN
 IF (codCliente>0) THEN
UPDATE clientes SET dataultcarta='1899-12-30' WHERE dataultcarta IS NULL AND Codigo=codCliente;
UPDATE clientes SET ultvencimento='1899-12-30' WHERE ultvencimento IS NULL AND Codigo=codCliente;  
 
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
 and quitado="N"
 AND to_days(current_date)-to_days(vencimento)>(select fatnrdias from configfinanc where codigofilial=filial limit 1)
 AND valoratual > 0
 AND vencimento<current_date ;
 
 UPDATE clientes SET debito=(SELECT IFNULL(SUM(valor),0) FROM crmovclientes WHERE crmovclientes.codigo=clientes.Codigo and valoratual>0),saldo=credito-debito
 WHERE codigo=codCliente;
 UPDATE clientes SET debitoch=(SELECT IFNULL(SUM(valor),0) FROM cheques WHERE codigocliente=codCliente AND repassado="N" and depositado="N")
 WHERE codigo=codCliente; 
 UPDATE clientes SET saldo=credito-(debito+debitoch) WHERE credito>0
 and codigo=codCliente;
 
 UPDATE clientes SET creditoav=0 WHERE codigofilial=filial and codigo=codCliente; 
 UPDATE clientes,contaspagar SET clientes.creditoav=(SELECT SUM(valor) FROM contaspagar WHERE codigocliente=clientes.Codigo AND quitado="N" AND cancelado="N") 
WHERE clientes.Codigo=contaspagar.codigocliente
AND clientes.Codigo=codCliente;
  
 END IF;
 IF (codCliente=0) THEN
 
 UPDATE crmovclientes set datacalcjuros=vencimento,
 diasdecorrido=to_days(current_date)-to_days(datacalcjuros),
 vrjuros = 0,
 vrjuros = taxaJuros*(TO_DAYS(CURRENT_DATE)-TO_DAYS(datacalcjuros))*valoratual/100,
 valorcorrigido = valoratual 
 WHERE valoratual >0 AND datacalcjuros<=vencimento
 AND quitado="N";
 
  UPDATE crmovclientes SET encargos=0
  WHERE encargos < 0 ;
   UPDATE crmovclientes SET vrjuros=0
  WHERE vrjuros < 0 ;
update clientes set creditoav=0 where codigofilial=filial and creditoav>0; 
UPDATE clientes,contaspagar SET clientes.creditoav=(SELECT SUM(valor) FROM contaspagar WHERE codigocliente=clientes.Codigo AND quitado="N" AND cancelado="N") 
WHERE clientes.Codigo=contaspagar.codigocliente
AND clientes.codigofilial=filial;
 
 END IF;
 
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarEstoqueOff` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarEstoqueOff` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AtualizarEstoqueOff`(in filial varchar(5))
BEGIN
 UPDATE contdocs set
 EADr06=MD5(CONCAT(IFNULL(ecffabricacao,""),IFNULL(ncupomfiscal,""),IFNULL(contadornaofiscalGNF,""),IFNULL(contadordebitocreditoCDC,""),DATA,IFNULL(coognf,""),tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(IFNULL(ncupomfiscal,""),davnumero,DATA,total))
 where estoqueatualizado="N";
 
UPDATE venda SET 
venda.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")));
 
  
  UPDATE caixa SET 
 eaddados=MD5(CONCAT(ecffabricacao,coo,ccf,gnf,ecfmodelo,valor,tipopagamento));
 
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
CALL AtualizarQdtRegistros();
 END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarQdtRegistros` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarQdtRegistros` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AtualizarQdtRegistros`()
BEGIN
IF ((SELECT COUNT(1) FROM quantidaderegistros)=0) THEN 
INSERT INTO quantidaderegistros (contdav) VALUES ("1");
END IF;
UPDATE quantidaderegistros SET 
  contdav = (SELECT MD5(COUNT(*)) FROM contdav), 
  vendadav = (SELECT MD5(COUNT(*)) FROM vendadav), 
  contprevendaspaf = (SELECT MD5(COUNT(*)) FROM contprevendaspaf), 
  vendaprevendapaf = (SELECT MD5(COUNT(*)) FROM vendaprevendapaf), 
  produtos = (SELECT MD5(COUNT(*)) FROM produtos),
  contdocs = (SELECT MD5(COUNT(*)) FROM contdocs),
  vendaarquivo = (SELECT MD5(COUNT(*)) FROM vendaarquivo),
  caixaarquivo = (SELECT MD5(COUNT(*)) FROM caixaarquivo),
  contrelatoriogerencial = (SELECT MD5(COUNT(*)) FROM contrelatoriogerencial),
  r01=(select MD5(count(*)) from r01),
 r02=(SELECT MD5(COUNT(*)) FROM r02),
 r03=(SELECT MD5(COUNT(*)) FROM r03); 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ChecarIntegridadeCaixa` */

/*!50003 DROP PROCEDURE IF EXISTS  `ChecarIntegridadeCaixa` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ChecarIntegridadeCaixa`(in filial varchar(5),in _operador varchar(10))
BEGIN
DROP TABLE IF EXISTS `tempintegridadecaixa`; 
create temporary table tempintegridadecaixa 
(SELECT contdocs.documento as documento FROM contdocs,caixa
WHERE contdocs.documento NOT IN ( caixa.documento )
AND contdocs.operador=_operador and caixa.operador=_operador
and contdocs.CodigoFilial=filial and caixa.CodigoFilial=filial
and caixa.data=contdocs.data
and contdocs.documento>caixa.documento
group by documento
)
union all 
(select caixa.documento as documento from caixa
where caixa.operador=_operador and codigofilial=filial
and dpfinanceiro='Recebimento'
group by documento,tipopagamento,valor
having count(*)>1);
    END */$$
DELIMITER ;

/* Procedure structure for procedure `CriarInventario` */

/*!50003 DROP PROCEDURE IF EXISTS  `CriarInventario` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `CriarInventario`(in filial varchar(5),in _operador varchar(10))
BEGIN
DECLARE _anoInventario INT DEFAULT 0;
DECLARE _numeroInventario INT DEFAULT 0;
DECLARE cursorAno cursor for select year(current_date);
declare cursorNumero cursor for select ifnull(max(numero)+1,1) from continventario where codigofilial=filial and year(data)=year(current_date);
open cursorAno;
open cursorNumero;
fetch cursorAno into _anoInventario;
fetch cursorNumero into _numeroInventario;
 
 IF (  ( SELECT CONCAT( MONTH( CURRENT_DATE),YEAR( CURRENT_DATE) ) ) > (SELECT CONCAT( MONTH( MAX(DATA)),YEAR( MAX(DATA)) ) FROM continventario where codigofilial=filial)  ) THEN
	INSERT INTO continventario (ano,numero,encerrado,operador,data,total,codigofilial)
	values (_anoInventario,_numeroInventario,"N",_operador,current_date,0,filial);
	
	if (filial='00001') then
		insert into produtosinventario select * from produtos where codigofilial=filial
		AND tipo='0 - Produto';
		update produtosinventario set anoinventario=_anoInventario,inventarionumero=_numeroInventario,
		quantidade=quantidade+qtdretida,
		custototal=IFNULL(quantidade*customedio,0) where codigofilial=filial and anoinventario=0;	
	end if;	
	
		IF (filial<>'00001') THEN
		INSERT INTO produtosinventario SELECT * FROM produtosfilial WHERE codigofilial=filial
		AND tipo='0 - Produto';
		UPDATE produtosinventario SET anoinventario=_anoInventario,inventarionumero=_numeroInventario,
		quantidade=quantidade+qtdretida,
		custototal=IFNULL(quantidade*customedio,0) WHERE codigofilial=filial AND anoinventario=0;	
	END IF;	
	
	
 END IF;	
    END */$$
DELIMITER ;

/* Procedure structure for procedure `CriarTabelasTemp` */

/*!50003 DROP PROCEDURE IF EXISTS  `CriarTabelasTemp` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `CriarTabelasTemp`(IN tabela VARCHAR(10),IN filial VARCHAR(5),IN dataInicial DATE,IN dataFinal DATE)
BEGIN
IF (tabela="venda") THEN
DROP TABLE IF EXISTS `vendatmp`;
DROP TABLE IF EXISTS `vendanftmp`;
if (filial<>"00000") THEN
CREATE TABLE `vendatmp` SELECT * FROM `vendaarquivo` WHERE codigofilial=filial AND DATA BETWEEN dataInicial AND datafinal;
CREATE TABLE `vendanftmp` SELECT * FROM `vendanf` WHERE codigofilial=filial AND DATA BETWEEN dataInicial AND datafinal;
END IF;
IF (tabela="venda" and filial="00000") THEN
CREATE TABLE `vendatmp` SELECT * FROM `vendaarquivo` WHERE DATA BETWEEN dataInicial AND datafinal;
CREATE TABLE `vendanftmp` SELECT * FROM `vendanf` WHERE DATA BETWEEN dataInicial AND datafinal;
END IF;
END IF;
IF (tabela="caixa") THEN
DROP TABLE IF EXISTS `caixatmp`;
CREATE TABLE `caixatmp` SELECT * FROM `caixaarquivo` WHERE codigofilial=filial AND DATA BETWEEN dataInicial AND datafinal;
END IF;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `DescontoMaximo` */

/*!50003 DROP PROCEDURE IF EXISTS  `DescontoMaximo` */;

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

/*!50003 DROP PROCEDURE IF EXISTS  `EncerrarCaixa` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `EncerrarCaixa`(in idOperador varchar(10),in filial varchar(5),in ipTerminal varchar(15),in nrFabricaoECF varchar(20)  )
BEGIN
 DECLARE totalRecBL decimal default 0;
 DECLARE totalRecDC decimal default 0;
 DECLARE totalFN DECIMAL DEFAULT 0;
 DECLARE saldoFinal decimal DEFAULT 0;
 
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
 
 
 if ( (nrFabricaoECF)="incluir") then
 
  insert into caixassoma (ip,codigofilial,data,horaabertura,horafechamento,operador,
  saldo,dinheiro,entradadh,entradach,
  entradaca,entradafi,cheque,chequepre,cartao,recebimento,
  recebimentodh,recebimentoch,recebimentoca,
  crediario,sangria,vendas,custos,juros,devolucao,
  renegociacao,perdao,descontovenda,descontoreceb,
  descontorecebjuros,crediariocr,jurosperdao,jurosrenegociacao,
  encargos,devolucaocr,devolucaoprd,jurosrecch,encargosrecebidos, 
  diferenca,ocorrencia,chequefi,chequefipre,financiamento,financeira,
  qtdcupons,suprimento,dpfinanceiro,receitas,recebimentobl,recebimentodc,
  emprestimodh,emprestimoch,comprati,trocach,ticket,valorservicos,
  descontoservicos,descontocapitalrn,crediarioservicosCR,jurosrecca,
  recebimentoAV )
   values (ipTerminal,filial,current_date,
  (SELECT horaabertura FROM caixa WHERE operador=idOperador and codigofilial=filial and tipopagamento="SI" limit 1),
  current_time,
  idOperador,
 (SELECT ifnull(sum(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND dpfinanceiro="Saldo Inicial"),
 (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="DH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"),
 (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="DH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  and historico="Entrada"
 ), 
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada"
 ), 
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CA" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada"
 ),  
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="FI" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada"
 ), 
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
  AND tipopagamento="CH" 
  AND dpfinanceiro<>"Crediario" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND vencimento=data),
   (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
  AND tipopagamento="CH" 
  AND dpfinanceiro<>"Crediario" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  and vencimento>current_date
 ),
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CA" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"),
 
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'), 
  
 (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  and tipopagamento="DH"
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'), 
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="CH"
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'), 
  
 (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="CA"
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'), 
 
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="CR"
  AND dpfinanceiro="Crediario"), 
 
 (SELECT IFNULL(SUM(valor),0) FROM movdespesas WHERE operador=idOperador AND codigofilial=filial
  AND encerrado<>'S'
  AND sangria='S'),
 
 (select IFNULL(sum(total-ratdesc+rateioencargos),0) from venda WHERE operador=idOperador AND codigofilial=filial
 and cancelado="N"),
 (SELECT ifnull(SUM(ABS( (quantidade *embalagem)  )*custo),0) FROM venda WHERE operador=idOperador AND codigofilial=filial
 AND cancelado="N"), 
  
(SELECT IF(SUM(vrjuros-vrdesconto)>0,SUM(vrjuros-vrdesconto),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND dpfinanceiro LIKE 'Receb%'),
(SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="DV"),  
 
 (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="RN"), 
 
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="PD"), 
 
  (SELECT ifnull(SUM(ratdesc),0) FROM venda WHERE  operador=idOperador AND codigofilial=filial
 AND cancelado="N"), 
 (
 SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND dpfinanceiro LIKE 'Receb%'
AND vrdesconto>vrjuros),
 (
 SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
 AND dpfinanceiro LIKE 'Receb%'
 AND (vrdesconto>0 OR vrdesconto>vrjuros) 
 AND dpfinanceiro<>'Recebimento s/j'
 ),
   (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="CR"
  AND dpfinanceiro="Venda"), 
 (
 SELECT ifnull( IF(SUM(vrjuros-vrdesconto)>0,SUM(vrjuros-vrdesconto),0),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
AND tipopagamento="PD"
 ),
 (
 SELECT ifnull( IF(SUM(vrjuros-vrdesconto)>0,SUM(vrjuros-vrdesconto),0),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
AND tipopagamento="RN"
 ),
   (SELECT ifnull(SUM(rateioencargos),0) FROM venda WHERE  operador=idOperador AND codigofilial=filial
 AND cancelado="N"), 
 
 (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="DV"
 AND dpfinanceiro LIKE 'Receb%'),
 
   (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="DV" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"),
   (SELECT IFNULL(SUM(jurosch),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD' 
  AND dpfinanceiro LIKE 'Receb%'), 
   (SELECT IFNULL(SUM(encargos),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD' 
  AND dpfinanceiro LIKE 'Receb%'), 
  (SELECT IFNULL(SUM(valor),0)FROM caixa  WHERE operador=idOperador AND codigofilial=filial 
   and dpfinanceiro="Diferenca"),
 "WEB-Ocorrencia",
 
   (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Crediario"
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND vencimento=data
 ), 
 
    (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Crediario"
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND vencimento>data
 ), 
 (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="FI" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"),
 
 (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="FN" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"),
 
 (select count(1) as quantidade from contdocs WHERE operador=idOperador AND codigofilial=filial
  AND dpfinanceiro<>"Recebimento"
 AND dpfinanceiro<>"Recebimento est"
 AND dpfinanceiro<>"Recebimento s/j"
 AND data=current_date 
 ),
 (SELECT IFNULL(SUM(valor),0) AS suprimento FROM caixa WHERE operador=idOperador AND codigofilial=filial
 AND tipopagamento="SU"),
 "Venda",
 (SELECT ifnull(SUM(valor),0) AS receita FROM movreceitas  WHERE operador=idOperador AND codigofilial=filial
  AND encerrado<>'S'
  AND sangria='S'),
 
(SELECT IFNULL(SUM(valor),0) FROM caixa  WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND tipopagamento="BL"
AND dpfinanceiro LIKE 'Receb%'),
(SELECT IFNULL(SUM(valor),0) FROM caixa  WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND tipopagamento="DC"
AND dpfinanceiro LIKE 'Receb%'),
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Emprestimo DH"
  and tipopagamento="CR" ), 
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Emprestimo CH"
  AND tipopagamento="CH" ), 
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Compra TI"
  AND tipopagamento="CR" ), 
 
   (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Troca CH"
  AND tipopagamento="CH" ), 
(SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="TI" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"),
 
 (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
  AND tipopagamento="TI" 
  AND dpfinanceiro="Servicos"),
 (SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
  AND tipopagamento="TI" 
  AND dpfinanceiro="Servicos"),
 (SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="RN"),  
 
  (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
  AND dpfinanceiro="Servicos"
  and tipopagamento="CR"),
 
 (SELECT IFnull(SUM(jurosca),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND dpfinanceiro LIKE 'Receb%'),
(SELECT IFnull(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
and tipopagamento="AV"
AND dpfinanceiro LIKE 'Receb%') 
 
 );  
set @id = (SELECT max(inc) from caixassoma where  operador=idOperador AND codigofilial=filial);
SET @saldoFinal = (SELECT SUM( (saldo+dinheiro+cheque+chequefi+chequepre+chequepre+emprestimoch+trocach+cartao+financiamento+ticket+(recebimento-recebimentobl-recebimentoDC)+ receitas - sangria )) FROM caixassoma WHERE operador=idOperador AND codigofilial=filial AND caixassoma.inc=(SELECT MAX(inc) FROM caixassoma));
update caixassoma set saldocaixa=@saldoFinal 
where  inc=@id;
 end if;
 
 
 INSERT INTO vendatmp SELECT * FROM venda 
 WHERE codigofilial=filial
 AND operador=idOperador;
 
 INSERT INTO caixatmp SELECT * FROM caixa 
 WHERE codigofilial=filial
 AND operador=idOperador;
 
 
 INSERT INTO vendaarquivo SELECT * FROM venda 
 WHERE codigofilial=filial
 AND operador=idOperador;
  
 DELETE FROM venda
 WHERE codigofilial=filial
 AND operador=idOperador;
 INSERT INTO caixaarquivo SELECT * FROM caixa
 WHERE codigofilial=filial
 AND operador=idOperador;
 DELETE FROM caixa
 WHERE codigofilial=filial
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
 VALUES (ipTerminal,"1",filial,current_date,
 (SELECT IFNULL(sum(valortarifabloquete),0) from caixa where codigofilial=filial and operador=idOperador),
 (SELECT contadespesa from filiais where codigofilial=filial),
 (SELECT subconta from filiais where codigofilial=filial),
 "S",idOperador,"Tarifação da taxa de recebimento dos bloquetes",
 (SELECT descricaoconta from filiais where codigofilial=filial limit 1),
 (SELECT descricaosubconta from filiais where codigofilial=filial limit 1),
 "N",
 (SELECT conta from filiais where codigofilial=filial limit 1));
 END IF;
 IF (totalRecDC>0) THEN
 INSERT INTO  movcontasbanco(conta, movimento, valorcredito, data, historico, codigofilial,operador)
 VALUES ((SELECT conta from filiais where codigofilial=filial),"credito",
 (SELECT IFNULL( sum(valor),0) from caixa where codigofilial=filial and operador=idOperador and tipopagamento="DC"),
 current_date,"Recebimento: Depósito Conta corrente",codigofilial,operador);
 END IF;
 
 CALL FechamentoDiario(filial); 
 CALL AtualizarQdtRegistros();	
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `EstornarDevolucao` */

/*!50003 DROP PROCEDURE IF EXISTS  `EstornarDevolucao` */;

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
 ' and devolucao.numero>0 '
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
DELETE FROM venda where numerodevolucao=devolucaoNR AND numerodevolucao>0;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `EstornarQuitacao` */

/*!50003 DROP PROCEDURE IF EXISTS  `EstornarQuitacao` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `EstornarQuitacao`(in docEstorno int,in codCliente int,in ipTerminal varchar(15),in filial varchar(5),in operadorEstorno varchar(10))
BEGIN
 DELETE FROM caixas where enderecoip=ipTerminal and codigofilial=filial;
 INSERT INTO contdocs (ip,codigofilial,data,dataexe,totalbruto,desconto,vrjuros,encargos,total,nome,
 codigocliente,NrParcelas,vendedor,operador,observacao,classe,
 historico,dpfinanceiro,tipopagamento,devolucaorecebimento,estornado,hora,concluido)	
 SELECT  ipTerminal,filial,current_date,current_date,totalbruto*-1,desconto*-1,vrjuros*-1,encargos*-1,total*-1,nome,
 codigocliente,NrParcelas,vendedor,operadorEstorno,observacao,classe,
 CONCAT("Rec est ",docEstorno),"Recebimento est",tipopagamento,devolucaorecebimento*-1,estornado,current_time,"S"
 FROM contdocs where documento=docEstorno;
 INSERT INTO  caixa (codigofilial,enderecoIP,nome,codigocliente,valor,dataexe,data,vencimento,
 tipopagamento,banco,agencia,cheque,valorcheque,
 nomecheque,cartao,numerocartao,operador,dpfinanceiro,historico,documento, 
 vrjuros,vrdesconto,encargos) 
 SELECT filial,ipTerminal,nome,codigocliente,valor*-1,current_date,current_date,vencimento,
 tipopagamento,banco,agencia,cheque,valorcheque*-1,nomecheque,cartao,numerocartao,
 operadorEstorno,"Recebimento est",operadorEstorno,(select max(documento) from contdocs where ip=ipTerminal),
 vrjuros*-1,vrdesconto*-1,encargos*-1
 FROM caixa WHERE documento=docEstorno;
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
 update contaspagar set cancelado="S" where interpolador=docEstorno;
call EstornarDevolucao(docEstorno,filial,ipTerminal,(select contdocs.devolucaonumero from contdocs where contdocs.documento=docEstorno),operadorEstorno);
insert into auditoria (codigofilial,usuario,hora,data,tabela,acao,documento,local)
values (filial,operadorEstorno,current_time,current_date,"Clientes","Estorno",codCliente,
(select nome from contdocs where documento=docEstorno limit 1) );
 END */$$
DELIMITER ;

/* Procedure structure for procedure `ExcluirDocumento` */

/*!50003 DROP PROCEDURE IF EXISTS  `ExcluirDocumento` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ExcluirDocumento`(IN ipTerminal VARCHAR(15), IN nrDocumento INT,IN filial VARCHAR(5),IN operador VARCHAR(10),IN cooECF VARCHAR(6), IN ccfECF VARCHAR(6),in motivoObs varchar(150),in usuarioSolicitante varchar(10) )
BEGIN
 DECLARE valorEstorno REAL DEFAULT 0;
 DECLARE cursorValorEstorno CURSOR FOR SELECT IFNULL(total,0 ) FROM contdocs WHERE documento=nrDocumento LIMIT 1;
 OPEN cursorValorEstorno;
 FETCH cursorValorEstorno INTO valorEstorno;
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
 codigocliente,NrParcelas,vendedor,operador,concat(motivoObs,observacao),classe,
 concat("Venda est ",nrDocumento),dpfinanceiro,tipopagamento,id,custos*-1,
 devolucaovenda*-1,devolucaorecebimento*-1,valorservicos*-1,descontoservicos*-1,CURRENT_TIME,"S","S",cooECF,ccfECF,ecffabricacao, ecfmarca,ecfmodelo,ecfnumero,"1",contadordebitocreditoCDC,contadornaofiscalGNF,COOGNF  
 FROM contdocs WHERE documento=nrDocumento;
 SET @RetornaPrd = CONCAT ('UPDATE ',@tabelaProduto,',venda 
 set ',@tabelaProduto,'.qtdanterior=',@tabelaProduto,'.quantidade,',
 @tabelaProduto,'.quantidade=',@tabelaProduto,'.quantidade+(SELECT IFNULL(SUM(venda.quantidade),0) from venda where venda.cancelado="N" and venda.codigo=',@tabelaProduto,'.codigo and venda.documento=','"',nrDocumento,'" ),',		
 @tabelaProduto,'.qtdprateleiras=',@tabelaProduto,'.qtdprateleiras+(SELECT IFNULL(SUM(venda.quantidade),0) from venda where venda.cancelado="N" AND venda.codigo=',@tabelaProduto,'.codigo and venda.documento=','"',nrDocumento,'"), ',
 @tabelaProduto,'.documento=',nrDocumento,'  
 where venda.codigo=',@tabelaProduto,'.codigo
 AND ',@tabelaProduto,'.documento<>',nrDocumento,' 
 AND ',@tabelaProduto,'.codigofilial=venda.codigofilial  
 AND ',@tabelaProduto,'.codigofilial=',filial, '
 AND venda.documento>0 
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
 SET produtosgrade.quantidade=produtosgrade.quantidade+(SELECT SUM(venda.quantidade) FROM venda WHERE venda.cancelado="N" AND venda.documento=nrdocumento AND produtosgrade.codigo=venda.codigo AND venda.grade=produtosgrade.grade)
 WHERE produtosgrade.codigo=venda.codigo
 AND produtosgrade.grade=venda.grade
 AND venda.documento=nrDocumento
 AND produtosgrade.codigofilial=filial;
 UPDATE vendas SET cancelado="S" WHERE documento=nrDocumento and id=ipTerminal;
 UPDATE venda SET cancelado="S" WHERE documento=nrDocumento;
 UPDATE venda SET cancelado="S" WHERE coo=cooECF and codigofilial=filial;
  
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
 DELETE FROM cheques WHERE documento=nrDocumento AND documento<>0;
 DELETE FROM movcartoes WHERE documento=nrDocumento AND documento<>0;
 INSERT INTO auditoria (codigofilial,usuario,usuariosolicitante,hora,DATA,tabela,acao,documento,LOCAL,observacao) 
 VALUES (
 filial,operador,usuarioSolicitante,CURRENT_TIME,CURRENT_DATE,'Venda',concat('Estorno, valor R$: ',valorEstorno),nrDocumento,
 (SELECT nome FROM contdocs WHERE documento=nrDocumento LIMIT 1),motivoObs
 );
 UPDATE clientes 
 SET valorcartaofidelidade=valorcartaofidelidade+(SELECT total FROM contdocs WHERE documento=nrDocumento)
 WHERE codigo=(SELECT cartaofidelidade FROM contdocs WHERE documento=nrDocumento);
 
 UPDATE contdocs SET concluido='S',estornado='S',
 EADr06=MD5(CONCAT(ecffabricacao,ncupomfiscal,IFNULL(contadornaofiscalGNF,""),IFNULL(contadordebitocreditoCDC,""),DATA,IFNULL(coognf,""),tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,davnumero,DATA,total)),observacao=motivoObs 
 WHERE documento=nrDocumento;
 UPDATE contdocs SET 
 EADr06=MD5(CONCAT(IFNULL(ecffabricacao,""),IFNULL(ncupomfiscal,""),IFNULL(contadornaofiscalGNF,""),IFNULL(contadordebitocreditoCDC,""),DATA,IFNULL(coognf,""),tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(IFNULL(ncupomfiscal,""),davnumero,DATA,total))
 WHERE ip=ipTerminal AND ncupomfiscal=cooECF; 
UPDATE venda SET 
 venda.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")))
 WHERE coo=cooECF; 
 UPDATE venda SET 
 venda.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")))
 WHERE documento=nrDocumento; 
  
   CALL AtualizarQdtRegistros();	
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `ExecutarApuracaoFiscal` */

/*!50003 DROP PROCEDURE IF EXISTS  `ExecutarApuracaoFiscal` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ExecutarApuracaoFiscal`(in dataInicial date,in dataFinal date)
BEGIN
update entradas set pis=0,cofins=0,cstpis="98",cstcofins="98"  where (cfopentrada<>'1.101' and cfopentrada<>'1.102' and 
cfopentrada<>'2.102' AND cfopentrada<>'1.401' 
AND cfopentrada<>'1.403' AND cfopentrada<>'1.410' 
AND cfopentrada<>'2.403' 
AND cfopentrada<>'1.933' AND cfopentrada<>'2.933'
AND cfopentrada<>'1.201' AND cfopentrada<>'1.202' 
AND cfopentrada<>'2.201' AND cfopentrada<>'2.202'
AND cfopentrada<>'2.410' and cfopentrada<>'1.411'
And cfopentrada<>'2.411' AND cfopentrada<>'2.122' 
and cfopentrada<>'2.124' And cfopentrada<>'2.902');
UPDATE vendatmp SET pis=0,cofins=0,cstpis="49",cstcofins="49"  WHERE 
(cfop<>"5.102" AND cfop<> "6.102" 
AND cfop<>"5.101" AND cfop<> "6.101"
AND cfop<>"5.402" AND cfop<>"6.402"
AND cfop<>"5.401" AND cfop<>"6.401" 
AND cfop<> "5.403" AND cfop<> "6.403"
AND cfop<> "6.404" AND cfop<> "5.405");
 
SELECT
  `blococregc190`.`codigofilial` AS `filial`,
  `blococregc190`.`cfopentrada`  AS `cfop`,
  `blococregc190`.`modelonf`     AS `modeloDOC`,
  `blococregc190`.`tributacao`   AS `CSTICMS`,
  SUM(`blococregc190`.`totalProduto`) AS `totalproduto`,
  SUM(`blococregc190`.`totalNF`) AS `total`,
  SUM(`blococregc190`.`bcicms`)  AS `bcICMS`,
  SUM(`blococregc190`.`toticms`) AS `totICMS`,
  SUM(`blococregc190`.`baseCalculoIPI`) AS `baseCalculoIPI`,
  SUM(`blococregc190`.`ipiItem`) AS `totalIPI`,
  SUM(`blococregc190`.`baseCalculoPIS`) AS `bcPIS`,
  SUM(`blococregc190`.`baseCalculoCOFINS`) AS `bcCOFINS`,
  SUM(`blococregc190`.`totalPIS`) AS `totalPIS`,
  SUM(`blococregc190`.`totalCOFINS`) AS `totalCOFINS`,
  SUM(`blococregc190`.`bcicmsST`) AS `bcICMSST`,
  SUM(`blococregc190`.`valoricmsST`) AS `totalICMSST`,
  SUM(`blococregc190`.`valoroutrasdespesas`) AS `totalOutrasDespesas`,
  SUM(`blococregc190`.`valorisentas`) AS `totalIsentas`  
FROM `blococregc190`
WHERE ((`blococregc190`.`lancada` = 'X')
       AND (`blococregc190`.`dataentrada` >= dataInicial)
       AND (`blococregc190`.`dataentrada` <= dataFinal))
GROUP BY `blococregc190`.`cfopentrada`,`blococregc190`.`codigofilial`,`blococregc190`.`modelonf`  UNION ALL SELECT
                                                                                  `blococregc190_saida`.`codigofilial`    AS `filial`,
                                                                                  `blococregc190_saida`.`cfop`            AS `cfop`,
                                                                                  `blococregc190_saida`.`modelodocfiscal` AS `modeloDOC`,
                                                                                  `blococregc190_saida`.`tributacao`      AS `cstICMS`,
                                                                                  SUM(`blococregc190_saida`.`totalItem`)  AS `totalProduto`,
                                                                                  SUM(`blococregc190_saida`.`totalItem`)  AS `total`,
                                                                                  SUM(`blococregc190_saida`.`baseCalculoICMS`) AS `bcICMS`,
                                                                                  SUM(`blococregc190_saida`.`totalicms`)  AS `totICMS`,
                                                                                  SUM(`blococregc190_saida`.`baseCalculoIPI`) AS `baseCalculoIPI`,
                                                                                  SUM(`blococregc190_saida`.`totalIPI`)   AS `totalIPI`,
                                                                                  SUM(`blococregc190_saida`.`baseCalculoPIS`+`blococregc190_saida`.`baseCalculoPIS_QTD`) AS `bcPIS`,
                                                                                  SUM(`blococregc190_saida`.`baseCalculoCOFINS`+`blococregc190_saida`.`baseCalculoCOFINS_QTD`) AS `bcCOFINS`,
                                                                                  SUM(`blococregc190_saida`.`totalPIS`+`blococregc190_saida`.`totalPIS_QTD`)   AS `totalPIS`,
                                                                                  SUM(`blococregc190_saida`.`totalCOFINS`+`blococregc190_saida`.`totalCOFINS_QTD`) AS `totalCOFINS`,
                                                                                  SUM(`blococregc190_saida`.`bcICMSST`)   AS `bcICMSST`,
                                                                                  SUM(`blococregc190_saida`.`totalICMSST`) AS `totalICMSST`, 
 										  SUM(0) AS `totalOutrasDespesas`,
									          SUM(0) AS `totalIsentas`
                                                                                FROM `blococregc190_saida`
                                                                                WHERE ((`blococregc190_saida`.`DATA` >= dataInicial)
                                                                                       AND (`blococregc190_saida`.`DATA` <= dataFinal))
                                                                                GROUP BY `blococregc190_saida`.`cfop`,`blococregc190_saida`.`codigofilial`,`blococregc190_saida`.`modelodocfiscal` UNION ALL SELECT
                                                                                                                                                                       `blococregc381_pis`.`codigofilial`       AS `filial`,
                                                                                                                                                                       `blococregc381_pis`.`cfop`               AS `cfop`,
                                                                                                                                                                       `blococregc381_pis`.`modelodocfiscal`    AS `modeloDOC`,
                                                                                                                                                                       `blococregc381_pis`.`tributacao`         AS `cstICMS`,
                                                                                                                                                                       SUM(`blococregc381_pis`.`total`)         AS `totalProduto`,
                                                                                                                                                                       SUM(`blococregc381_pis`.`total`)         AS `total`,
                                                                                                                                                                       SUM(`blococregc381_pis`.`baseCalculoICMS`) AS `bcICMS`,
                                                                                                                                                                       SUM(`blococregc381_pis`.`totalicms`)     AS `totICMS`,
                                                                                                                                                                       SUM(0)                                   AS `baceCalculoIPI`,
                                                                                                                                                                       SUM(0)                                   AS `totalIPI`,
                                                                                                                                                                       SUM(`blococregc381_pis`.`baseCalculoPIS`) AS `bcPIS`,
                                                                                                                                                                       SUM(`blococregc381_pis`.`baseCalculoCOFINS`) AS `bcCOFINS`,
                                                                                                                                                                       SUM(`blococregc381_pis`.`totalPIS`)      AS `totalPIS`,
                                                                                                                                                                       SUM(`blococregc381_pis`.`totalCOFINS`)   AS `totalCOFINS`,
                                                                                                                                                                       SUM(0)                                   AS `bcICMSST`,
                                                                                                                                                                       SUM(0)                                   AS `totalICMSST`,
																				       SUM(0) AS `totalOutrasDespesas`,
																				       SUM(0) AS `totalIsentas`
                                                                                                                                                                     FROM `blococregc381_pis`
                                                                                                                                                                     WHERE ((`blococregc381_pis`.`data` >= dataInicial)
                                                                                                                                                                            AND (`blococregc381_pis`.`data` <= dataFinal))
                                                                                                                                                                     GROUP BY `blococregc381_pis`.`cfop`,`blococregc381_pis`.`codigofilial` UNION ALL SELECT
                                                                                                                                                                                                                                                        `blococregc491_pis`.`codigofilial`        AS `filial`,
                                                                                                                                                                                                                                                        `blococregc491_pis`.`cfop`                AS `cfop`,
                                                                                                                                                                                                                                                        `blococregc491_pis`.`modelodocfiscal`     AS `modeloDOC`,
                                                                                                                                                                                                                                                        `blococregc491_pis`.`tributacao`          AS `cstICMS`,
                                                                                                                                                                                                                                                        SUM(`blococregc491_pis`.`total`)          AS `totalProduto`,
                                                                                                                                                                                                                                                        SUM(`blococregc491_pis`.`total`)          AS `total`,
                                                                                                                                                                                                                                                        SUM(`blococregc491_pis`.`baseCalculoICMS`) AS `bcICMS`,
                                                                                                                                                                                                                                                        SUM(`blococregc491_pis`.`totalicms`)      AS `totICMS`,
                                                                                                                                                                                                                                                        SUM(0)                                    AS `baceCalculoIPI`,
                                                                                                                                                                                                                                                        SUM(0)                                    AS `totalIPI`,
                                                                                                                                                                                                                                                        SUM(`blococregc491_pis`.`baseCalculoPIS`) AS `bcPIS`,
                                                                                                                                                                                                                                                        SUM(`blococregc491_pis`.`baseCalculoCOFINS`) AS `bcCOFINS`,
                                                                                                                                                                                                                                                        SUM(`blococregc491_pis`.`totalPIS`)       AS `totalPIS`,
                                                                                                                                                                                                                                                        SUM(`blococregc491_pis`.`totalCOFINS`)    AS `totalCOFINS`,
                                                                                                                                                                                                                                                        SUM(0)                                    AS `bcICMSST`,
                                                                                                                                                                                                                                                        SUM(0)                                    AS `totalICMSST`,
																															SUM(0) AS `totalOutrasDespesas`,
																															SUM(0) AS `totalIsentas`
                                                                                                                                                                                                                                                      FROM `blococregc491_pis`
                                                                                                                                                                                                                                                      WHERE ((`blococregc491_pis`.`data` >= dataInicial)
                                                                                                                                                                                                                                                             AND (`blococregc491_pis`.`data` <= dataFinal))
                                                                                                                                                                                                                                                      GROUP BY `blococregc491_pis`.`cfop`,`blococregc491_pis`.`codigofilial`;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FechamentoDiario` */

/*!50003 DROP PROCEDURE IF EXISTS  `FechamentoDiario` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `FechamentoDiario`(IN filial VARCHAR(5))
BEGIN
  DECLARE qtdCxAbertos INT DEFAULT 0;  
  DECLARE totalCreditos real DEFAULT 0;
  declare totalDebitos REAL default 0;  
  declare totalDiferenca REAL default 0;
  declare totalCreditosCR REAL default 0;
  declare totalDebitosCR REAL default 0;
  declare creditosCH REAL default 0;
  DECLARE creditosCA REAL DEFAULT 0;
  declare custoFinalPrd real default 0;
  DECLARE custoMedioFinalPrd REAL DEFAULT 0;
  declare custoFinalRetidos real default 0;
 
  DECLARE cursorQtdCxAbertos CURSOR FOR SELECT IFNULL( COUNT(1) ,0) FROM caixa WHERE codigofilial=filial;    
  DECLARE cursorCreditos CURSOR FOR SELECT IFNULL(SUM(saldo+dinheiro+cheque+chequefi+chequefipre+chequepre+emprestimoch+trocach+cartao+financiamento+ticket+(recebimento-recebimentobl-recebimentoDC)+receitas),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1);  
  DECLARE cursorDebitos CURSOR FOR SELECT IFNULL(SUM(sangria),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorDiferenca CURSOR FOR SELECT IFNULL(SUM(diferenca),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorCreditosCR CURSOR FOR SELECT IFNULL(SUM(crediario+crediariocr+jurosrenegociacao+crediarioservicosCR),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorDebitosCR CURSOR FOR SELECT IFNULL(SUM(recebimento+juros+jurosrecch+JurosRecCA+descontoreceb+(perdao-jurosperdao)),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorCreditosCH CURSOR FOR SELECT IFNULL(SUM(cheque+chequepre),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorCreditosCA CURSOR FOR SELECT IFNULL(SUM(cartao),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  declare cursorCustoFinalPrd Cursor for select ifnull(sum((quantidade+qtdretida)*custo),0) from produtos where codigofilial=filial and tipo='0 - Produto';
  DECLARE cursorCustoMedioFinalPrd CURSOR FOR SELECT IFNULL(SUM((quantidade+qtdretida)*customedio),0) FROM produtos WHERE codigofilial=filial AND tipo='0 - Produto';
  DECLARE cursorCustoFinalPrdRet CURSOR FOR SELECT IFNULL(SUM(qtdretida*custo),0) FROM produtos WHERE codigofilial=filial AND tipo='0 - Produto';
 
  OPEN cursorQtdCxAbertos; 
  OPEN cursorCreditos;
  OPEN cursorDebitos;
  open cursorDiferenca;
  open cursorCreditosCR;
  open cursorDebitosCR;
  OPEN cursorCreditosCH;
  OPEN cursorCreditosCA;
  open cursorCustoFinalPrd;
  OPEN cursorCustoMedioFinalPrd;
  open cursorCustoFinalPrdRet;
 
  FETCH cursorQtdCxAbertos INTO qtdCxAbertos;     		
  FETCH cursorCreditos INTO totalCreditos; 
  FETCH cursorDebitos INTO totalDebitos; 
  fetch cursorDiferenca into totalDiferenca;
  FETCH cursorCreditosCR into totalCreditosCR;
  FETCH cursorDebitosCR into totalDebitosCR;
  FETCH cursorCreditosCH into creditosCH;
  FETCH cursorCreditosCA INTO creditosCA;
  fetch cursorCustoFinalPrd into custoFinalPrd;
  FETCH cursorCustoMedioFinalPrd INTO custoMedioFinalPrd;
  fetch cursorCustoFinalPrdRet into custoFinalRetidos;
 
 
 if (qtdCxAbertos=0) then
 
    if  ((select count(1) from movimento where finalizado=" ")=0) then
      insert into movimento (codigofilial,finalizado,data) values(filial," ",current_date);
    end if;
 
	update movimento set finalizado="X",credito=totalCreditos,debito=totalDebitos,
	saldocaixa=totalCreditos-totalDebitos+totalDiferenca,
	creditocr=totalCreditosCR,
	debitocr=totalDebitosCR,
	creditoch=creditosCH,
	creditoca=creditosCA,
	custofinalestoque=custoFinalPrd,
	customediofinalestoque=custoMedioFinalPrd,
	custofinalretidos=custoFinalRetidos  
	where codigofilial=filial and finalizado=" ";
	
	update produtos set descontopromocao=0,
	validadepromoc = current_date,situacao="Normal"
	where validadepromoc<current_date
	AND codigofilial=filial
	and situacao="Promoção";
	
	UPDATE produtosfilial SET descontopromocao=0,
	validadepromoc = CURRENT_DATE,situacao="Normal"
	WHERE validadepromoc<CURRENT_DATE
	AND codigofilial=filial
	AND situacao="Promoção";
	
if ( (select abaterestoqueprevenda from configfinanc where codigofilial=filial limit 1) = "S") THEN	
	
	
	update produtos set qtdprevenda=0 where codigofilial=filial;
	UPDATE produtosfilial SET qtdprevenda=0 WHERE codigofilial=filial;
	
	UPDATE vendadav,produtos SET
	produtos.qtdprevenda=(SELECT SUM(vendadav.quantidade) FROM vendadav WHERE vendadav.codigo=produtos.codigo AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)<(select diasreservaestoquedav from configfinanc where codigofilial=filial limit 1) )
	WHERE vendadav.codigofilial= filial
	AND vendadav.codigo=produtos.codigo
	AND produtos.codigofilial=filial
	AND vendadav.codigofilial = produtos.CodigoFilial
	and (vendadav.coo is null or vendadav.coo='')
	AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)<(SELECT diasreservaestoquedav FROM configfinanc WHERE codigofilial=filial LIMIT 1)
	AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)>=0;
	
	UPDATE vendadav,produtosfilial SET
	produtosfilial.qtdprevenda=(SELECT SUM(vendadav.quantidade) FROM vendadav WHERE vendadav.codigo=produtosfilial.codigo AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)<(SELECT diasreservaestoquedav FROM configfinanc WHERE codigofilial=filial LIMIT 1) )
	WHERE vendadav.codigofilial= filial
	AND vendadav.codigo=produtosfilial.codigo
	AND produtosfilial.codigofilial=filial
	AND vendadav.codigofilial = produtosfilial.CodigoFilial
	AND (vendadav.coo IS NULL OR vendadav.coo='')
	AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)<(SELECT diasreservaestoquedav FROM configfinanc WHERE codigofilial=filial LIMIT 1)
	AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)>=0;
	
END IF;
	
	insert into crmovclientespagas select * from crmovclientes
	where valoratual=0 and filialpagamento=filial;
	
	delete from crmovclientes where valoratual=0 and filialpagamento=filial;	
	
if ( (select diasparamudarsituacao from configfinanc where codigofilial=filial limit 1)>0) THEN	
  IF ( (SELECT diasparamudarsituacaoinferior FROM configfinanc WHERE codigofilial=filial limit 1)>0) THEN	
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
 
 
delete from vendatmp where data<current_date;
call AtualizarQdtRegistros();
    END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarDAV` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarDAV` */;

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
 WHERE id=ipTerminal and codigofilial=filial ;
 
 
 INSERT INTO `vendadav` (`acrescimototalitem`, `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`
 FROM vendas where id=ipTerminal and documento=DAVNumero;
 
 
 INSERT INTO `caixadav` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas where enderecoip=ipTerminal and valor<>0 and documento=DAVNumero;
 
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
 
 UPDATE filiais SET versaopaf="1.9" WHERE codigofilial=filial;
 CALL AtualizarQdtRegistros();	
 
 
  
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarDAVOS` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarDAVOS` */;

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
 
 
 INSERT INTO `vendadavos` (`acrescimototalitem`, `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`
 FROM vendas where id=ipTerminal;
 
 
 INSERT INTO `caixadavos` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas where enderecoip=ipTerminal and valor<>0;
  
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
 CALL AtualizarQdtRegistros();	
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarPreVenda` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarPreVenda` */;

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
  
 INSERT INTO `vendaprevendapaf` (`acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`
 FROM vendas where id=ipTerminal;
 
 
 INSERT INTO `caixaprevendapaf` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas where enderecoip=ipTerminal and valor<>0;
 
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
 CALL AtualizarQdtRegistros();	
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarVenda` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarVenda` */;

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
 
  UPDATE contdocs SET DATA=CURRENT_DATE WHERE DATA="0001-01-01"
 AND documento=doc;
 
 
 
UPDATE vendas SET 
vendas.modelodocfiscal=(SELECT contdocs.modeloDOCFiscal FROM contdocs WHERE contdocs.documento=doc),
vendas.serieNF=(SELECT contdocs.serienf FROM contdocs WHERE contdocs.documento=doc),
vendas.subserienf=(SELECT contdocs.subserienf FROM contdocs WHERE contdocs.documento=doc),
vendas.ecffabricacao=(SELECT contdocs.ecffabricacao FROM contdocs WHERE contdocs.documento=doc), 
vendas.codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE contdocs.documento=doc), 
vendas.operador=(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc),
vendas.coo=(SELECT contdocs.ncupomfiscal FROM contdocs WHERE contdocs.documento=doc),
vendas.ccf=(SELECT contdocs.ecfcontadorcupomfiscal FROM contdocs WHERE contdocs.documento=doc),
vendas.data=(SELECT contdocs.data FROM contdocs WHERE contdocs.documento=doc),
vendas.ratdesc= truncate((vendas.total/(select contdocs.Totalbruto from contdocs where documento=doc))*(select desconto from contdocs where documento=doc),2)
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
 data=(SELECT contdocs.DATA FROM contdocs WHERE contdocs.documento=doc),
 ecfnumero=(SELECT contdocs.ecfnumero FROM contdocs WHERE contdocs.documento=doc),
 estornado="N", historico="Cupom Fiscal" 
 WHERE caixas.enderecoip=ipTerminal;
 
 UPDATE caixas SET 
 vrdesconto=(SELECT contdocs.desconto FROM contdocs WHERE contdocs.documento=doc),
 valorservicos=(SELECT contdocs.valorservicos FROM contdocs WHERE contdocs.documento=doc)
 WHERE caixas.enderecoip=ipTerminal LIMIT 1;
 
IF ( (SELECT modeloDOCFiscal FROM contdocs WHERE documento=doc LIMIT 1)="02") THEN
UPDATE caixas SET historico="Nota Fiscal" WHERE  enderecoip=ipTerminal and codigofilial=filial; 
END IF; 
if ( (SELECT COUNT(1) FROM caixas WHERE tipopagamento="CR" and caixas.enderecoip=ipTerminal) >0) THEN
UPDATE caixas SET historico="Entrada" WHERE  tipopagamento<>"CR" AND caixas.enderecoip=ipTerminal;
END IF;
IF ( (SELECT enderecoentrega FROM contdocs WHERE documento=doc LIMIT 1)<>'') THEN
update contdocs set romaneio='S' where documento=doc;
update vendas set aentregar='S' where vendas.id=ipTerminal;
END IF; 
 
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
 vendas.idfornecedor=',@tabelaProduto,'.idfornecedor,
 vendas.fabricante=',@tabelaProduto,'.fabricante,		
 vendas.classe=(select classe from contdocs where documento=',doc,'),
 vendas.lote=',@tabelaProduto,'.lote,
 vendas.codigobarras=',@tabelaProduto,'.codigobarras,		
 vendas.tipo=',@tabelaProduto,'.tipo,
 vendas.pis=',@tabelaProduto,'.pis,  
 vendas.cofins=',@tabelaProduto,'.cofins,
 vendas.cstpis=',@tabelaProduto,'.tributacaoPIS, 
 vendas.cstcofins=',@tabelaProduto,'.tributacaoCOFINS,
 vendas.despesasacessorias=',@tabelaProduto,'.despesasacessorias,
 vendas.cfop=IF(',@tabelaProduto,'.cfopsaida="","5.102",',@tabelaProduto,'.cfopsaida)
 WHERE vendas.id=(SELECT id from contdocs where documento=',doc,') and vendas.codigo=',@tabelaProduto,'.codigo 
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
 ,@tabelaProduto,'.quantidade=',@tabelaProduto,'.quantidade-(SELECT IFNULL(SUM(vendas.quantidade*vendas.embalagem),0) from vendas where vendas.cancelado="N" AND vendas.codigo=',@tabelaProduto,'.codigo and vendas.documento=',doc,' and vendas.id=','"',ipTerminal,'"),',
 @tabelaProduto,'.dataultvenda=current_date,',
 @tabelaProduto,'.qtdprovisoria=0,',
 @tabelaProduto,'.EADP2relacaomercadoria=md5( concat(',@tabelaProduto,'.codigo,',@tabelaProduto,'.descricao,',@tabelaProduto,'.tributacao,',@tabelaProduto,'.icms,',@tabelaProduto,'.precovenda,',@tabelaProduto,'.precoatacado) ),',
 @tabelaProduto,'.qtdprateleiras=',@tabelaProduto,'.qtdprateleiras-(select IFNULL(sum(vendas.quantidade),0) from vendas where vendas.cancelado="N" and vendas.codigo=',@tabelaProduto,'.codigo and vendas.id=','"',ipTerminal,'")  
 where vendas.codigo=',@tabelaProduto,'.codigo 
 and ',@tabelaProduto,'.codigofilial=vendas.codigofilial  
 AND vendas.cancelado="N"  
 AND ',@tabelaProduto,'.codigofilial=',filial, '
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
 AND ',@tabelaProduto,'.codigofilial=',filial, '
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
 SELECT codigofilial,documento,banco,cheque,agencia,DATA,valor,valor,vencimento,
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
 (SELECT ifnull( (encargos/nrparcelas) ,0) FROM contdocs WHERE documento=doc),
 dpfinanceiro,
 (SELECT IF(cpf<>'',cpf,cnpj) FROM clientes WHERE codigo=(SELECT codigocliente FROM contdocs WHERE documento=doc))
 FROM caixas WHERE enderecoip=ipTerminal
 AND tipopagamento='CR';
 
 INSERT INTO `venda` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`aliquotaipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`cstcofins`,`cstpis`, `despesasacessorias`,`percentualRedBaseCalcICMS`,`modelodocfiscal`,`serienf`,`subserienf`,`ecffabricacao`,`coo`,`acrescimototalitem`,`cancelado`,`eaddados`,`ccf`,`idfornecedor`,`icmsst`, `mvast`, `cfop`) 
 SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`aliquotaipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`cstcofins`,`cstpis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`modelodocfiscal` ,`serienf`,`subserienf`,`ecffabricacao`,`coo`,`acrescimototalitem`,`cancelado`,`eaddados`,`ccf`,`idfornecedor`,`icmsst`, `mvast`, `cfop`
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
 
 UPDATE contdocs SET concluido='S',devolucaovenda=(select sum(valor) from caixas where enderecoip=ipTerminal and tipopagamento="DV"),
 EADr06=MD5(CONCAT(ifnull(ecffabricacao,""),ifnull(ncupomfiscal,""),IFNULL(contadornaofiscalGNF,""),IFNULL(contadordebitocreditoCDC,""),DATA,IFNULL(coognf,""),tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(ifnull(ncupomfiscal,""),davnumero,DATA,total)),
 estoqueatualizado='S' 
 WHERE documento=doc;
 
 update contdocs set data=current_date where data="0001-01-01"
 and documento=doc;
 
 DELETE FROM caixas WHERE enderecoip=ipTerminal and codigofilial=filial;
 DELETE FROM vendas WHERE id=ipTerminal and codigofilial=filial;
 
 IF (preVenda>0) THEN
 UPDATE contprevendaspaf SET finalizada='S',
 datafinalizacao=(SELECT contdocs.DATA FROM contdocs WHERE contdocs.documento=doc),
 ncupomfiscal=(SELECT ncupomfiscal FROM contdocs WHERE documento=doc),
 ecfcontadorcupomfiscal=(SELECT ecfcontadorcupomfiscal FROM contdocs WHERE documento=doc) 
 WHERE numeroDAVfilial=preVenda AND codigofilial=filial;	
 END IF;
 
 IF (DAVNumero>0) THEN
 UPDATE contdav SET finalizada='S',
 datafinalizacao=(SELECT contdocs.DATA FROM contdocs WHERE contdocs.documento=doc),
 EADRegistroDAV=MD5(CONCAT(ifnull(ncupomfiscal,""),numero,DATA,valor,ifnull(numeroECF,"001"))),
 ncupomfiscal=(SELECT ncupomfiscal FROM contdocs WHERE documento=doc),
 ecfcontadorcupomfiscal=(select ecfcontadorcupomfiscal fROM contdocs WHERE documento=doc)
 WHERE numeroDAVfilial=DAVNumero and codigofilial=filial AND finalizada='N';	
 
 UPDATE contdavos SET finalizada='S',
 datafinalizacao=(SELECT contdocs.DATA FROM contdocs WHERE contdocs.documento=doc),
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,numero,DATA,valor,IFNULL(numeroECF,"001"),ifnull(contadorRGECF,""),ifnull(cliente,""),ifnull(ecfCPFCNPJconsumidor,""))),
 ncupomfiscal=(SELECT ncupomfiscal FROM contdocs WHERE documento=doc),
 ecfcontadorcupomfiscal=(SELECT ecfcontadorcupomfiscal FROM contdocs WHERE documento=doc) 
 WHERE numero=DAVNumero AND codigofilial=filial and finalizada='N';
UPDATE vendadav SET 
vendadav.modelodocfiscal=(SELECT contdocs.modeloDOCFiscal FROM contdocs WHERE contdocs.documento=doc),
vendadav.serieNF=(SELECT contdocs.serienf FROM contdocs WHERE contdocs.documento=doc),
vendadav.subserienf=(SELECT contdocs.subserienf FROM contdocs WHERE contdocs.documento=doc),
vendadav.ecffabricacao=(SELECT contdocs.ecffabricacao FROM contdocs WHERE contdocs.documento=doc), 
vendadav.codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE contdocs.documento=doc), 
vendadav.operador=(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc),
vendadav.coo=(SELECT contdocs.ncupomfiscal FROM contdocs WHERE contdocs.documento=doc),
vendadav.ccf=(SELECT contdocs.ecfcontadorcupomfiscal FROM contdocs WHERE contdocs.documento=doc),
vendadav.DATA=(SELECT contdocs.DATA FROM contdocs WHERE contdocs.documento=doc)
WHERE documento=DAVNumero;
update vendadav set coo="" where documento=DAVNumero and coo is null;
UPDATE vendadav SET ccf="" WHERE documento=DAVNumero AND ccf IS NULL;
UPDATE vendadav SET ecffabricacao="" WHERE documento=DAVNumero AND ecffabricacao IS NULL;
UPDATE vendadav SET 
vendadav.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")))
WHERE documento=DAVNumero; 
 
 
 END IF; 
 
 if ((select totalbrutocomencargos from configfinanc where codigofilial=filial limit 1)="S") then
 update contdocs set totalbruto=total where documento=doc limit 1;
 end if;
 
  CALL AtualizarQdtRegistros();	
 END */$$
DELIMITER ;

/* Procedure structure for procedure `GerarDebitoServicos` */

/*!50003 DROP PROCEDURE IF EXISTS  `GerarDebitoServicos` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `GerarDebitoServicos`(IN operador VARCHAR(10),IN filial VARCHAR(5),IN ipTerminal VARCHAR(15),IN dataVencimento DATE, IN nGerarParcelas INT,IN gravarSeqBoleto CHAR(1), OUT qtdBoletosGerados INT,OUT totalGerado DECIMAL(10,2))
BEGIN
DECLARE done INT DEFAULT FALSE;
DECLARE idCliente INT DEFAULT 0;
DECLARE qtdParcelasGerada INT DEFAULT 0;
DECLARE cursorClientes CURSOR FOR SELECT codigo FROM clientes 
WHERE restritiva="N"
AND datacobranca<=CURRENT_DATE 
AND valorcontrato>0 AND databoleto<>CURRENT_DATE ORDER BY nome ;
DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
SET qtdBoletosGerados =0;
SET totalGerado = 0;
UPDATE clientes SET marcado=" ";
	OPEN cursorClientes;
	read_loop: LOOP
		FETCH cursorClientes INTO idCliente;	
		IF done THEN
			LEAVE read_loop;
		END IF;
IF ( (SELECT COUNT(1) FROM crmovclientes WHERE codigo=idcliente)>nGerarParcelas) THEN
UPDATE clientes SET marcado="X" WHERE codigo=idcliente;
END IF;
	
IF ( (SELECT COUNT(1) FROM crmovclientes WHERE codigo=idcliente)<=nGerarParcelas) THEN
INSERT INTO contdocs (ip,codigofilial,DATA,dataexe,
totalbruto,desconto,encargos,total,nome,
codigocliente,NrParcelas,vendedor,operador,
dpfinanceiro,concluido,hora,nrboletobancario) 
VALUES(ipTerminal,
filial,CURRENT_DATE,CURRENT_DATE,(SELECT valorcontrato FROM clientes WHERE codigo=idcliente),
(SELECT desconto FROM clientes WHERE codigo=idcliente),0,(IF( (SELECT datalimitedesconto FROM clientes WHERE codigo=idCliente) >CURRENT_DATE, (SELECT valorcontrato-desconto FROM clientes  WHERE codigo=idcliente),(SELECT valorcontrato FROM clientes WHERE codigo=idcliente)) ) ,
(SELECT nome FROM clientes WHERE codigo=idcliente),idCliente,1,(SELECT vendedor FROM clientes WHERE codigo=idcliente),operador,
"Servicos","S",CURRENT_TIME,0 ); 
INSERT INTO caixa (codigofilial,enderecoIP,nome,codigocliente,
valor,documento,dataexe,DATA,vencimento,
tipopagamento,operador,vendedor,
dpfinanceiro)
VALUES (filial,ipTerminal,(SELECT nome FROM clientes WHERE codigo=idcliente),idCliente,
(SELECT valorcontrato FROM clientes WHERE codigo=idcliente),
(SELECT MAX(documento) FROM contdocs WHERE contdocs.ip=ipTerminal AND codigocliente=idcliente),
CURRENT_DATE,CURRENT_DATE,dataVencimento,"CR",operador,(SELECT vendedor FROM clientes WHERE codigo=idcliente),
"Servicos");
INSERT INTO crmovclientes (codigofilial,documento,nome,codigo,datacompra,
vencimento,datacalcjuros,parcela,nrparcela,valor,valoratual,
vendedor,valorcorrigido,
dpfinanceiro,cpfcnpj)
VALUES (filial,
(SELECT MAX(documento) FROM contdocs WHERE contdocs.ip=ipTerminal AND codigocliente=idcliente),
(SELECT nome FROM clientes WHERE codigo=idcliente),
idCliente,
CURRENT_DATE,
datavencimento,
datavencimento,
"1",
"1/1",
(SELECT valorcontrato FROM clientes WHERE codigo=idcliente),
(SELECT valorcontrato FROM clientes WHERE codigo=idcliente),
(SELECT vendedor FROM clientes WHERE codigo=idcliente),
(SELECT valorcontrato FROM clientes WHERE codigo=idcliente),
"Servicos",
(SELECT cnpj FROM clientes WHERE codigo=idcliente));
UPDATE clientes SET databoleto=CURRENT_DATE WHERE codigo=idCliente;
IF (gravarSeqBoleto="S") THEN
	CALL GerarNumeroBoleto(filial,(SELECT MAX(documento) FROM contdocs WHERE contdocs.ip=ipTerminal AND codigocliente=idcliente));
	SET qtdBoletosGerados = qtdBoletosGerados+1;
	SET totalGerado = totalGerado+(SELECT total FROM contdocs WHERE contdocs.ip=ipTerminal AND codigocliente=idcliente);
END IF;
END IF;
		
	END LOOP;
	CLOSE cursorClientes;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `GerarNFe` */

/*!50003 DROP PROCEDURE IF EXISTS  `GerarNFe` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `GerarNFe`( IN filial VARCHAR(5),
IN ipTerminal VARCHAR(15),
IN criarNF CHAR(1),
IN NFeOrigem BIGINT(9),
in tipoNFe char(1),
in tipoEmissaoNFe char(1),
IN modeloNFe VARCHAR(2),
in finalidadeNFe char(1),
in situacaoNFe varchar(2),
in naturezaOperacaoNFe varchar(60),
IN gerarICMS CHAR(1),
in doc int(9),
in descontoNFe decimal(9,2),
in freteNFe decimal(10,2),
in seguroNFe decimal(10,2),
in despesasNFe decimal(10,2),
IN marcavolume varchar(20), 
IN idCliente INT(6),
IN idFornecedorNFe INT(6),
in idTransportadoraNFe int(6),
in idVeiculoNFe int(5),
in idInfoComplementarNFe int(4), 
in cfopTransporteNFE varchar(5), 
in serieNFe varchar(3),
in operadorNFe varchar(10),
in cfopNFe varchar(5),
in dadosComplementarNFe text,
in tipoFreteNFe char(1),
in volumeNFe int(1),
in qtdVolumeNFe int(3),
in especieVolumeNFe varchar(20),
in chaveAcessoRefNFe varchar(44),
IN colocarDataHoraNFe CHAR(1),
IN indPag CHAR(1),
in NFeEntradaAdEstoque char(1),
IN crtNFE CHAR(1),
 OUT numeroNFe BIGINT(9) )
BEGIN
DECLARE tentativas INT DEFAULT 0;
declare totalLiquidoNFe real default 0;
declare totalBrutoNFe real default 0;
declare totalDescontoNFe real default 0;
declare totalAcrescimoNFe real default 0;
declare baseCalculoICMSNFe real default 0;
DECLARE totalICMSNFe REAL DEFAULT 0;
DECLARE baseCalculoICMSSTNFe REAL DEFAULT 0;
declare baseCalculoIPINFe real default 0;
DECLARE totalICMSSTNFe REAL DEFAULT 0;
declare totalFreteNFe real default 0;
declare totalICMSFreteNFe real default 0;
declare totalSeguroNFe real default 0;
declare totalDespesasNFe real default 0;
declare totalIPINFe real default 0;
declare pesoBrutoNFe real default 0;
DECLARE pesoLiquidoNFe REAL DEFAULT 0;
declare baseCalculoPISNFe real default 0;
declare baseCalculoCOFINSNFe real default 0;
declare totalPISNFe real default 0;
declare totalCOFINSNFe real default 0;
declare totalCSLLNFe real default 0;
declare baseCalculoICMSSTNFeSIMPLES real default 0;
declare totalICMSSTNFeSIMPLES real default 0;
DECLARE done INT DEFAULT FALSE;
declare docCupom int default 0;
 
DECLARE cursorTotalBruto CURSOR FOR SELECT IFNULL(SUM(total),0) FROM vendas WHERE id=ipTerminal AND cancelado='N';
DECLARE cursorDesconto CURSOR FOR SELECT IFNULL( SUM(ratdesc) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N';	
DECLARE cursorIPI CURSOR FOR SELECT IFNULL( SUM(ROUND(((((`vendas`.`total` + `vendas`.`ratfrete` + `vendas`.`ratdespesas` + `vendas`.`ratseguro` ) - `vendas`.`ratdesc`) * `vendas`.`aliquotaipi`) / 100),2)) +(qUnidIPI*vUnidIPI) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N';
declare cursorPesoBruto cursor for select fpesoBrutoVenda(filial,ipTerminal);
DECLARE cursorPesoLiq CURSOR FOR SELECT fpesoLiquidoVenda(filial,ipTerminal);
declare cursorBCICMS cursor for SELECT fvTotalBCICMSnfe(filial,ipTerminal);
declare cursortotalICMS Cursor for SELECT fvTotalICMSnfe(filial,ipTerminal);
DECLARE cursorBCICMSST CURSOR FOR SELECT ifnull(fvTotalBCICMSSTnfe(filial,ipTerminal), 0);
declare cursortotalICMSST cursor for SELECT fvTotalICMSSTnfe(filial,ipTerminal);
DECLARE cursorBCICMSSTSIMPLES CURSOR FOR SELECT IFNULL (SUM(ROUND(( (`vendas`.`total` -((`vendas`.`total` * `vendas`.`percentualRedBaseCalcICMSST`) / 100)   ) + ( (`vendas`.`total` - ((`vendas`.`total` * `vendas`.`percentualRedBaseCalcICMSST`) / 100) ) * (`vendas`.`mvast` / 100))),2)),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND icmsst>0 and (tributacao="10" or tributacao="70");
DECLARE cursortotalICMSSTSIMPLES CURSOR FOR SELECT IFNULL( TRUNCATE( (((SUM(ROUND(( (`vendas`.`total` - ((`vendas`.`total` * `vendas`.`percentualRedBaseCalcICMSST`) / 100) ) + ( (`vendas`.`total` - ((`vendas`.`total` * `vendas`.`percentualRedBaseCalcICMSST`) / 100) ) * (`vendas`.`mvast` / 100))),2)) * `vendas`.`icmsst`) / 100) - SUM(ROUND(((`vendas`.`total` * `vendas`.`icms`) / 100),2))),2),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND icmsst>0 AND (tributacao="10" OR tributacao="70");
declare cursorBCPIS cursor for select Ifnull (SUM(total),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND pis>0;
DECLARE cursorBCCOFINS CURSOR FOR SELECT IFNULL (SUM(total),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND cofins>0;
declare cursortotalPIS cursor for select IFnull (SUM(ROUND((`vendas`.`total` * (`vendas`.`pis` / 100)),2)),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND pis>0;
DECLARE cursortotalCOFINS CURSOR FOR SELECT IFNULL (SUM(ROUND((`vendas`.`total` * (`vendas`.`cofins` / 100)),2)),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND cofins>0;
DECLARE cursorCupons CURSOR FOR SELECT documento FROM vendas WHERE id=ipTerminal AND cancelado='N' and (vendas.coo<>'' and vendas.coo is not null);
DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
OPEN cursorTotalBruto;
FETCH cursorTotalBruto INTO totalBrutoNFe;
SET @nrItem  = 0;
UPDATE vendas SET nrcontrole=(@nrItem:=@nrItem+1) WHERE id=ipTerminal;
IF (descontoNFe>0) THEN
UPDATE vendas SET ratdesc=TRUNCATE((total)*  ( (descontoNFe*100/totalBrutoNFe) /100 ) ,2) 
WHERE id=ipTerminal AND cancelado="N";
SET totalDescontoNFe = (SELECT IFNULL( SUM(ratdesc) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N');
IF (descontoNFe<>(SELECT SUM(ratdesc) FROM vendas WHERE id=ipTerminal AND cancelado='N') ) THEN
UPDATE vendas SET ratdesc=ratdesc+(descontoNFe)-totalDescontoNFe
WHERE id=ipTerminal AND cancelado="N" LIMIT 1;
END IF;
SET totalDescontoNFe = (SELECT IFNULL( SUM(ratdesc) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N');
END IF;
IF (freteNFe>0) THEN
UPDATE vendas SET ratfrete=TRUNCATE((total)*  ( (freteNFe*100/totalBrutoNFe) /100 ) ,2) 
WHERE id=ipTerminal AND cancelado="N";
SET totalFreteNFe = (SELECT IFNULL( SUM(ratfrete) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N');
IF (freteNFe<>(SELECT SUM(ratfrete) FROM vendas WHERE id=ipTerminal AND cancelado='N') ) THEN
UPDATE vendas SET ratfrete=ratfrete+(freteNFe)-totalFreteNFe
WHERE id=ipTerminal AND cancelado="N" LIMIT 1;
END IF;
SET totalFreteNFe = (SELECT IFNULL( SUM(ratfrete) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N');
END IF;
IF (despesasNFe>0) THEN
UPDATE vendas SET ratdespesas=TRUNCATE((total)*  ( (despesasNFe*100/totalBrutoNFe) /100 ) ,2) 
WHERE id=ipTerminal AND cancelado="N";
SET totaldespesasNFe = (SELECT IFNULL( SUM(ratdespesas) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N');
IF (despesasNFe<>(SELECT SUM(ratdespesas) FROM vendas WHERE id=ipTerminal AND cancelado='N') ) THEN
UPDATE vendas SET ratdespesas=ratdespesas+(despesasNFe)-totaldespesasNFe
WHERE id=ipTerminal AND cancelado="N" LIMIT 1;
END IF;
SET totaldespesasNFe = (SELECT IFNULL( SUM(ratdespesas) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N');
END IF;
IF (seguroNFe>0) THEN
UPDATE vendas SET ratseguro=TRUNCATE((total)*  ( (seguroNFe*100/totalBrutoNFe) /100 ) ,2) 
WHERE id=ipTerminal AND cancelado="N";
SET totalseguroNFe = (SELECT IFNULL( SUM(ratseguro) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N');
IF (seguroNFe<>(SELECT SUM(ratseguro) FROM vendas WHERE id=ipTerminal AND cancelado='N') ) THEN
UPDATE vendas SET ratseguro=ratseguro+(seguroNFe)-totalSeguroNFe
WHERE id=ipTerminal AND cancelado="N" LIMIT 1;
END IF;
SET totalseguroNFe = (SELECT IFNULL( SUM(ratseguro) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N');
END IF;
OPEN cursorDesconto;
oPEN cursorIPI;
open cursorPesoBruto;
open cursorPesoLiq;
open cursorBCICMS;
open cursortotalICMS;
open cursorBCICMSST;
open cursortotalICMSST;
OPEN cursorBCICMSSTSIMPLES;
OPEN cursortotalICMSSTSIMPLES;
open cursorBCPIS;
OPEN cursorBCCOFINS;
open cursortotalPIS;
open cursortotalCOFINS;
fetch cursorDesconto into totalDescontoNFe;
fetch cursorIPI into totalIPINFe;
fetch cursorPesoBruto into pesoBrutoNFe;
fetch cursorPesoLiq into pesoLiquidoNFe;
fetch cursorBCICMS into baseCalculoICMSNFe;
fetch cursortotalICMS into totalICMSNFe;
FETCH cursorBCICMSST INTO baseCalculoICMSSTNFe;
fetch cursortotalICMSST into totalICMSSTNFe;
FETCH cursorBCICMSSTSIMPLES INTO baseCalculoICMSSTNFeSIMPLES;
FETCH cursortotalICMSSTSIMPLES INTO totalICMSSTNFeSIMPLES;
fetch cursorBCPIS into baseCalculoPISNFe;
FETCH cursorBCCOFINS INTO baseCalculoCOFINSNFe;
fetch cursortotalPIS into totalpisNFe;
FETCH cursortotalCOFINS INTO totalCOFINSNFe;
 SET @tabelaProduto='produtos';
 IF (filial<>'00001') THEN
 SET @tabelaProduto='produtosfilial';
 END IF;
 
if ( crtNFE="1") then
update vendas set icms=0 
WHERE id=ipTerminal;
set baseCalculoPISNFe = 0;
SET baseCalculoCOFINSNFe = 0;
SET totalPISNFe = 0;
SET totalCOFINSNFe = 0;
SET baseCalculoICMSNFe = 0;
SET totalICMSNFe = 0;
SET baseCalculoICMSSTNFe = baseCalculoICMSSTNFeSIMPLES ;
set totalICMSSTNFe = totalICMSSTNFeSIMPLES ;
end if;
 
  SET @qVenda = CONCAT('UPDATE vendas,',@tabelaProduto,' set   
 vendas.quantidadeanterior=',@tabelaProduto,'.quantidade,
 vendas.customedioanterior=',@tabelaProduto,'.customedio,
 vendas.quantidadeatualizada=',@tabelaProduto,'.quantidade-vendas.quantidade,		 
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
 vendas.lote=',@tabelaProduto,'.lote,
 vendas.codigobarras=',@tabelaProduto,'.codigobarras,		
 vendas.tipo=',@tabelaProduto,'.tipo,
 vendas.ncm=',@tabelaProduto,'.ncm,
 vendas.nbm=',@tabelaProduto,'.nbm,
 vendas.origem=',@tabelaProduto,'.origem,
 vendas.ncmespecie=',@tabelaProduto,'.ncmespecie 
 where vendas.id=','"',ipTerminal,'" and vendas.codigo=',@tabelaProduto,'.codigo 
 and ',@tabelaProduto,'.codigofilial=',filial);	
 PREPARE st FROM @qVenda;
 EXECUTE st;
set totalCSLLNFe = (totalBrutoNFe*(select CSLL from configfinanc where codigofilial=filial limit 1) ) /100;
set totalICMSFreteNFe = (totalFreteNFe *(SELECT aliquotaretencaofrete FROM configfinanc WHERE codigofilial=filial LIMIT 1) ) /100;
SET totalLiquidoNFe=(totalBrutoNFe+totalIPINFe+totalICMSSTNFe+totalDespesasNFe+totalSeguroNFe+totalFreteNFe)-totalDescontoNFe ;
IF (criarNF="S") THEN
repeat
set tentativas = tentativas +1;
update configfinanc set ultimoselonf = ultimoselonf+1
where codigofilial=filial;
update serienf set
         sequencial = sequencial+1 
         where codigofilial=filial
         and abs(serie)=abs(serieNFe);
select max(sequencial) into numeroNFe from serienf where ABS(serie)=ABS(serieNFe) and codigofilial=filial;
        
until (SELECT COUNT(1) FROM nfe012
         WHERE (cbdprocStatus='P' OR cbdprocstatus='E')
         AND cbdntfnumero=numeroNFe
          AND cbdntfserie=serieNFe
         AND cbdcodigofilial=filial) = 0 or tentativas > 10
 end REPEAT;
END IF;
if (criarNF="N") then
set numeroNFe = NFeOrigem;
end if;
IF (tipoNFe="0") THEN
UPDATE vendas SET pis=0,cofins=0,cstpis="98",cstcofins="98"  
WHERE (cfop<>"1.101" AND cfop<>"1.102" 
AND cfop<>"2.102" AND cfop<>"1.401" 
AND cfop<>'1.201' AND cfop<>'1.202'
AND cfop<>"1.403" AND cfop<>"1.410" AND cfop<>"2.403" 
AND cfop<>"1.933" AND cfop<>"2.933"
AND cfop<>"1.411" AND cfop<>"2.411"
and cfop<>"2.122" AND cfop<>"2.124"
and cfop<>"2.902") 
AND id=ipTerminal;
END IF;
if (tipoNFe="1") then
  UPDATE vendas SET pis=0,cofins=0,cstpis="49",cstcofins="49" 
  WHERE (cfop<>"5.102" AND cfop<> "6.102" AND cfop<>"5.101" 
  AND cfop<> "6.101" AND cfop<>"5.402" AND cfop<>"6.402" 
  AND cfop<>"5.401" AND cfop<>"6.401" AND cfop<> "5.403" 
  AND cfop<> "6.403" AND cfop<> "6.404" AND cfop<>"5.405" 
  AND cfop<> "5.202" AND cfop<> "6.202")
  AND id=ipTerminal;
 END IF;
   
  update vendas set notafiscal=numeroNFe,
  serieNF=serieNFe, modelodocfiscal=modeloNFe where id=ipTerminal; 
  if (gerarICMS="N") then
      UPDATE vendas SET icms=0 WHERE id=ipTerminal; 
   end if;
IF (criarNF="N") THEN
      IF  ( ( SELECT chave_nfe FROM contnfsaida WHERE notafiscal=NFeOrigem AND serie=serieNFe  AND codigofilial=filial limit 1) is null ) then                   
	delete from contnfsaida where notafiscal=NFeOrigem and serie=serieNFe and codigofilial=filial;                                                  
        delete from vendanf where notafiscal=NFeOrigem and serienf=serieNFe and codigofilial=filial;    
	DELETE FROM nfe012 WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	delete from cbd001 where CbdNtfNumero=NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001det where CbdNtfNumero=NFeOrigem  and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
        delete from cbd001detadicoes where CbdNtfNumero=NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001detarma where CbdNtfNumero=NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001detcofins where CbdNtfNumero=NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001deticmsnormalst where CbdNtfNumero=NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001detdi where CbdNtfNumero=NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;	
	delete from cbd001detipi where CbdNtfNumero=NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001detmed where CbdNtfNumero=NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001detpis where CbdNtfNumero= NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;	
	delete from cbd001duplicatas where CbdNtfNumero=NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001lacres where CbdNtfNumero= NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001nref where CbdNtfNumero= NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial; 	
	delete from cbd001obsfisco where CbdNtfNumero= NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001procref where CbdNtfNumero= NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
        delete from cbd001reboque where CbdNtfNumero= NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
	delete from cbd001vol where CbdNtfNumero= NFeOrigem and CbdNtfSerie=serieNFe and CbdCodigoFilial=filial;
      END IF;     
END IF;
IF (criarNF="S") THEN
      IF  ( ( SELECT chave_nfe FROM contnfsaida WHERE notafiscal=numeroNFe AND serie=serieNFe  AND codigofilial=filial LIMIT 1) IS NULL ) THEN                   
	DELETE FROM contnfsaida WHERE notafiscal=numeroNFe AND serie=serieNFe AND codigofilial=filial;                                                  
        DELETE FROM vendanf WHERE notafiscal=numeroNFe AND serienf=serieNFe AND codigofilial=filial;    
	DELETE FROM nfe012 WHERE CbdNtfNumero=numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001 WHERE CbdNtfNumero=numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001det WHERE CbdNtfNumero=numeroNFe  AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
        DELETE FROM cbd001detadicoes WHERE CbdNtfNumero=numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detarma WHERE CbdNtfNumero=numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detcofins WHERE CbdNtfNumero=numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001deticmsnormalst WHERE CbdNtfNumero=numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detdi WHERE CbdNtfNumero=numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;	
	DELETE FROM cbd001detipi WHERE CbdNtfNumero=numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detmed WHERE CbdNtfNumero=numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detpis WHERE CbdNtfNumero= numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;	
	DELETE FROM cbd001duplicatas WHERE CbdNtfNumero=numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001lacres WHERE CbdNtfNumero= numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001nref WHERE CbdNtfNumero= numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial; 	
	DELETE FROM cbd001obsfisco WHERE CbdNtfNumero= numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001procref WHERE CbdNtfNumero= numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
        DELETE FROM cbd001reboque WHERE CbdNtfNumero= numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001vol WHERE CbdNtfNumero= numeroNFe AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
      END IF;     
END IF;
UPDATE vendas SET codigobarras = ' 'WHERE id=ipTerminal AND LENGTH(codigobarras)<8;
INSERT INTO contnfsaida (notafiscal, documento, codigofilial, DATA,
dataemissao, operador,cfop,codcliente,cliente, cnpjcpf, endereco, bairro, cidade,
cep, estado, telefone, email, inscricao, total, desconto, liquido,
transportadora,placa, inscricaotr, cnpjcpftr, obs,
basecalculo,totalicms,basecalculoICMSST,totalICMSST,TotalProduto,
totalfrete,totalseguro,totaldesconto,despesasacessorias,totalipi,
totalNF,tipofrete,pesobruto,volumes,quantidadevolume,especievolume,marca,numero,tipo,
tipoemissao,finalidade,codfornecedor,situacaonf,selofiscal,cfoptransportador,
codigoANTT,totalICMSfrete, serie, nrefcuf, nrefaamm, nrefcnpj, nrefmodelo,
nrefserie,nrefnnf, nrefnfe, modelodocfiscal,idinfocomplementar ) 
VALUES(
numeroNFe,
doc,
filial,
CURRENT_DATE,
current_date,
operadorNFe,
cfopNFe,
idcliente,
IF(idcliente>0,(select nome from clientes where codigo=idcliente limit 1),(select razaosocial from fornecedores where codigo=idFornecedorNFe) ),
if(idcliente>0,(SELECT if(cnpj<>"",cnpj,cpf) FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT if(cgc<>"",cgc,cpf) FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
IF(idcliente>0,(SELECT CONCAT(endereco,numero) FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT CONCAT(endereco,numero) FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
IF(idcliente>0,(SELECT bairro FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT bairro FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
IF(idcliente>0,(SELECT cidade FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cidade FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
IF(idcliente>0,(SELECT cep FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cep FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
IF(idcliente>0,(SELECT estado FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT estado FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
IF(idcliente>0,(SELECT telefone FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT telefone FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
IF(idcliente>0,(SELECT email FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT email FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
IF(idcliente>0,(SELECT inscricao FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT inscricao FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
totalBrutoNFe,
totalDescontoNFe,
totalLiquidoNFe,
ifnull((select razaosocial from transportadoras where inc=idTransportadoraNFe limit 1),""),
ifnull((select placa from veiculos where idtransportadora=idtransportadoraNFe and inc=idVeiculoNFe limit 1),""),
ifnull((SELECT inscricao FROM transportadoras WHERE inc=idTransportadoraNFe LIMIT 1),""),
ifnull((SELECT cnpj FROM transportadoras WHERE inc=idTransportadoraNFe LIMIT 1),""),
dadosComplementarNFe,
baseCalculoICMSNFe,
totalICMSNFe,
baseCalculoICMSSTNFe,
totalICMSSTNFe,
totalBrutoNFe,
totalFreteNFe,
totalSeguroNFe,
totalDescontoNFe,
totalDespesasNFe,
totalIPINFe,
totalLiquidoNFe,
tipoFreteNFe,
pesoBrutoNFe,
volumeNFe,
qtdVolumeNFe,
especieVolumeNFe,
marcavolume,
numeroNFe,
tipoNFe,
tipoEmissaoNFe,
finalidadeNFe,
idFornecedorNFe,
situacaoNFe,
'0',
cfopTransporteNFE,
ifnull((SELECT ANTT FROM veiculos WHERE idtransportadora=idtransportadoraNFe AND inc=idVeiculoNFe limit 1),""),
'0',
serieNFe,
'0',
'0',
'',
'0',
'0',
'0',
chaveAcessoRefNFe,
modeloNFe,
idInfoComplementarNFe);
   INSERT INTO `vendanf` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`serieNF`,`cfop`,`acrescimototalitem`,`cstpis`,`cstcofins`,`icmsst`,`percentualRedBaseCalcICMSST`,`mvast`,`subserienf`,`modelodocfiscal`,`aliquotaIPI`,`ecffabricacao`,`coo`,`cancelado`,`eaddados`,`ccf`,`pcredsn`,`qUnidIPI`,`vUnidIPI`,`ncm`,`nbm`,`ncmespecie`,`ratfrete`,`ratseguro`,`ratdespesas`,`cstipi`,`origem`,`datafabricacao`,`vencimentoproduto`,`modalidadeDetBaseCalcICMS`,`modalidadeDetBaseCalcICMSst`) 
   SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`serieNF`,`cfop`,`acrescimototalitem`,`cstpis`,`cstcofins`,`icmsst`,`percentualRedBaseCalcICMSST`,`mvast`,`subserienf`,`modelodocfiscal`,`aliquotaIPI`,`ecffabricacao`,`coo`,`cancelado`,`eaddados`,`ccf`,`pcredsn`,`qUnidIPI`,`vUnidIPI`,`ncm`,`nbm`,`ncmespecie`,`ratfrete`,`ratseguro`,`ratdespesas`,`cstipi`,`origem`,`datafabricacao`,`vencimentoproduto`,`modalidadeDetBaseCalcICMS`,`modalidadeDetBaseCalcICMSst`
  FROM vendas
  WHERE id=ipTerminal;   
IF (tipoNFe="0") THEN
INSERT INTO moventradas (codigofilial,nf,codigofornecedor,fornecedor,dataemissao,
baseicms,icms,baseicmssubst,icmssubst,ipi,frete,
despesas,valorprodutos,valornota,DATA,usuario,cfopentrada,
tipofrete,serie, descontos,ip,especie,UFemitente,horaemissao,
basecalculoipi,modeloNF,situacaoNF,lancada,emitente,dataentrada) 
VALUES (
filial,
numeroNFe,
idFornecedorNFe,
(select razaosocial from fornecedores where codigo=idFornecedorNFe limit 1),
CURRENT_DATE,
baseCalculoICMSNFe,
totalICMSNFe,
baseCalculoICMSSTNFe,
totalICMSSTNFe,
totalIPINFe,
totalFreteNFe,
totalDespesasNFe,
totalBrutoNFe,
totalLiquidoNFe,
CURRENT_DATE,
operadorNFe,
cfopNFe,
tipoFreteNFe,
serieNFe,
descontoNFe,
ipTerminal,
'NF',
IF(idcliente>0,(SELECT estado FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT estado FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
CURRENT_TIME,
baseCalculoIPINFe,
modeloNFe,
situacaoNFe,
NFeEntradaAdEstoque,
'P',
CURRENT_DATE);
INSERT INTO entradas (
codigofilial,numero,codigo,descricao,fornecedor,codfornecedor,quantidade,
nf,quantnf,custonf,custo,custocalculado,ultcusto,
precovenda,precoanterior,DATA,icmsentrada,ipi,
margemlucro,usuario,frete,customedio,grupo,subgrupo,
icms,lancada,dataentrada,totalitem )
SELECT 
filial,
(select max(numero) from moventradas where id=ipTerminal),
codigo,
produto,
(SELECT razaosocial FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1),
idFornecedorNFe,
quantidade,
numeroNFe,
0,
0,
preco,
'0',
'0',
'0',
'0',
CURRENT_DATE,
icms,
'0',
'0',
operadorNFe,
'0',
'0',
grupo,
subgrupo,
icms,
NFeEntradaAdEstoque,
CURRENT_DATE,total 
 FROM vendas WHERE id=ipTerminal;
end if;
INSERT INTO cbd001 
(CbdEmpCodigo, CbdNtfNumero, CbdNtfSerie, CbdUsuImpPadrao, CbdcUF, CbdnatOp, CbdindPag,
Cbdmod, CbddEmi, CbddSaiEnt, CbdhrSaiEnt, CbdtpNf, CbdcMunFg, CbdtpImp, CbdtpEmis,
CbdfinNFe, CbdCNPJ_emit, CbdCPF_emit, CbdxNome, CbdxFant, CbdxLgr,
Cbdnro, CbdxCpl, CbdxBairro, CbdcMun, CbdxMun, CbdUF, CbdCEP, CbdcPais,
CbdxPais, Cbdfone, CbdIE, CbdIEST, CbdIM, CbdCNAE, 
CbdCNPJ_dest, CbdCPF_dest, CbdxNome_dest, 
CbdxLgr_dest, CbdxEmail_dest, Cbdnro_dest, CbdxCpl_dest, CbdxBairro_dest,
CbdcMun_dest, CbdxMun_dest, CbdUF_dest, CbdCEP_dest, CbdcPais_dest,
CbdxPais_dest, Cbdfone_dest, CbdIE_dest, CbdISUF, 
CbdCNPJ_ret, CbdxLgr_ret, Cbdnro_ret, CbdxCpl_ret, CbdxBairro_ret, 
CbdcMun_ret, CbdxMun_ret, CbdUF_ret, 
CbdCNPJ_entr, CbdxLgr_entr, Cbdnro_entr, CbdxBairro_entr, 
CbdcMun_entr, CbdxMun_entr, CbdUF_entr, CbdvBC_ttlnfe, CbdvICMS_ttlnfe,
CbdvBCST_ttlnfe, CbdvST_ttlnfe, CbdvProd_ttlnfe,  CbdvFrete_ttlnfe,
CbdvSeg_ttlnfe, CbdvDesc_ttlnfe, CbdvII_ttlnfe, CbdvIPI_ttlnfe,
CbdvPIS_ttlnfe, CbdvCOFINS_ttlnfe, CbdvOutro, CbdvNF, CbdvServ,
CbdvBC_ttlnfe_iss, CbdvISS, CbdvPIS_servttlnfe, CbdvCOFINS_servttlnfe,
CbdvRetPIS, CbdvRetCOFINS_servttlnfe, CbdvRetCSLL, CbdvBCIRRF,
CbdvIRRF, CbdvBCRetPrev, CbdvRetPrev, CbdmodFrete, CbdCNPJ_transp,
CbdCPF_transp, CbdxNome_transp, 	CbdIE_transp, CbdxEnder,
CbdxMun_transp, CbdUF_transp, CbdvServ_transp, CbdvBCRet, CbdpICMSRet,
CbdvICMSRet, CbdCFOP_transp, CbdcMunFG_transp, Cbdplaca, CbdUF_veictransp,
CbdRNTC, CbdnFat, CbdvOrig, CbdvDesc_cob, CbdvLiq, CbdinfAdFisco, CbdinfCpl,
CbdUFEmbarq, CbdxLocEmbarq, CbdxNEmp, CbdxPed, CbdxCont, CbdxCpl_entr, 
CbdFax, CbdCRT, CbdEmail_dest, CbdVagao, CbdBalsa, CbdCPF_ret, CbdCPF_entr, CbdCodigoFilial)
 VALUES (
filial, 
numeroNFe, 
serieNFe, 
'', 
(SELECT fCodigoEstadoMunIBGE("E",(SELECT cidade FROM filiais WHERE codigofilial=filial LIMIT 1),(SELECT estado FROM filiais WHERE codigofilial=filial LIMIT 1 )) ),
naturezaOperacaoNFe, 
indPag, 
modeloNFe, 
current_date, 
if (colocarDataHoraNFe="S",current_date,null) , 
IF (colocarDataHoraNFe="S",current_time,null),
tipoNFe, 
(SELECT fCodigoEstadoMunIBGE("M",(SELECT cidade FROM filiais WHERE codigofilial=filial LIMIT 1),(SELECT estado FROM filiais WHERE codigofilial=filial LIMIT 1 )) ),
'1', 
tipoEmissaoNFe, 
finalidadeNFe, 
(select cnpj from filiais where codigofilial=filial limit 1), 
'', 
(SELECT empresa FROM filiais WHERE codigofilial=filial LIMIT 1), 
(SELECT fantasia FROM filiais WHERE codigofilial=filial LIMIT 1), 
(SELECT endereco FROM filiais WHERE codigofilial=filial LIMIT 1), 
(SELECT numero FROM filiais WHERE codigofilial=filial LIMIT 1), 
(SELECT complemento FROM filiais WHERE codigofilial=filial LIMIT 1), 
(SELECT bairro FROM filiais WHERE codigofilial=filial LIMIT 1), 
(SELECT fCodigoEstadoMunIBGE("M",(SELECT cidade FROM filiais WHERE codigofilial=filial LIMIT 1),(SELECT estado FROM filiais WHERE codigofilial=filial LIMIT 1 )) ),
(SELECT cidade FROM filiais WHERE codigofilial=filial LIMIT 1), 
(SELECT estado FROM filiais WHERE codigofilial=filial LIMIT 1), 
(SELECT cep FROM filiais WHERE codigofilial=filial LIMIT 1), 
'1058', 
'BRASIL', 
(SELECT telefone1 FROM filiais WHERE codigofilial=filial LIMIT 1), 
(SELECT inscricao FROM filiais WHERE codigofilial=filial LIMIT 1), 
'', 
(SELECT inscricaomunicipal FROM filiais WHERE codigofilial=filial LIMIT 1), 
(SELECT CNAE FROM filiais WHERE codigofilial=filial LIMIT 1), 
(IF(idcliente>0,(SELECT cnpj FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cgc FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ) ,
(IF(idcliente>0,(SELECT cpf FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cpf FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ) ,
(IF(idcliente>0,(SELECT nome FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT razaosocial FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
(IF(idcliente>0,(SELECT endereco FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT endereco FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
(IF(idcliente>0,(SELECT email FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT email FROM fornecedores WHERE codigo=idFornecedorNFe limit 1) ) ), 
(IF(idcliente>0,(SELECT numero FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT numero FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
'', 
(IF(idcliente>0,(SELECT bairro FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT bairro FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
(SELECT fCodigoEstadoMunIBGE("M",IF(idcliente>0,(SELECT cidade FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cidade FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ,IF(idcliente>0,(SELECT estado FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT estado FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1)) ) ) ,
(IF(idcliente>0,(SELECT cidade FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cidade FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
(IF(idcliente>0,(SELECT estado FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT estado FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
(IF(idcliente>0,(SELECT cep FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cep FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
'1058', 
'BRASIL', 
(IF(idcliente>0,(SELECT telefone FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT telefone FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
(IF(idcliente>0,(SELECT inscricao FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT inscricao FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
'', 
'', 
'', 
'', 
'', 
'', 
'0', 
'', 
'', 
(IF(idcliente>0,(SELECT cnpj FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cgc FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ) ,
'', 
'', 
'', 
(SELECT fCodigoEstadoMunIBGE("M",IF(idcliente>0,(SELECT cidade FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cidade FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ,IF(idcliente>0,(SELECT estado FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT estado FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1)) ) ) ,
'', 
(IF(idcliente>0,(SELECT estado FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT estado FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
fvTotalBCICMSnfe(filial,ipTerminal), 
fvTotalICMSnfe(filial,ipTerminal), 
fvTotalBCICMSSTnfe(filial,ipTerminal),
fvTotalICMSSTnfe(filial,ipTerminal),
if (finalidadeNFe<>'2',totalBrutoNFe,0), 
totalFreteNFe, 
totalSeguroNFe, 
descontoNFe, 
'0', 
totalIPINFe, 
(SELECT IFNULL (SUM(ROUND((`vendas`.`total` * (`vendas`.`pis` / 100)),2)),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND pis>0), 
(SELECT IFNULL (SUM(ROUND((`vendas`.`total` * (`vendas`.`cofins` / 100)),2)),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND cofins>0), 
totalDespesasNFe, 
totalLiquidoNFe, 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
totalCSLLNFe, 
'0', 
'0', 
'0', 
'0', 
tipoFreteNFe, 
IF (idtransportadoraNFe>0,(SELECT cnpj FROM transportadoras WHERE inc=idtransportadoraNFe limit 1),''), 
IF (idtransportadoraNFe>0,(SELECT cpf FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT razaosocial FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT inscricao FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT concat(endereco,numero) FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT cidade FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT estado FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
totalFreteNFe, 
totalFreteNFe, 
(select aliquotaretencaofrete from configfinanc where codigofilial=filial limit 1), 
totalICMSFreteNFe, 
replace(cfopTransporteNFE, '.', ''), 
(SELECT fCodigoEstadoMunIBGE("M",(SELECT cidade FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),(SELECT estado FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1)) ), 
if (idVeiculoNFe>0,(select placa from veiculos where inc=idVeiculoNFe limit 1),''), 
IF (idVeiculoNFe>0,(SELECT estadoplaca FROM veiculos WHERE inc=idVeiculoNFe LIMIT 1),''),
IF (idVeiculoNFe>0,(SELECT ANTT FROM veiculos WHERE inc=idVeiculoNFe LIMIT 1),''), 
'', 
if(naturezaOperacaoNFe="Venda",totalBrutoNFe+totalFreteNFe+totalDespesasNFe,'0'), 
IF(naturezaOperacaoNFe="Venda",descontoNFe,'0'), 
IF(naturezaOperacaoNFe="Venda",totalBrutoNFe-descontoNFe,'0'), 
'', 
dadosComplementarNFe, 
(IF(idcliente>0,(SELECT estado FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT estado FROM fornecedores WHERE codigo=idFornecedorNFe) ) ),
(IF(idcliente>0,(SELECT cidade FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cidade FROM fornecedores WHERE codigo=idFornecedorNFe) ) ),
'', 
'', 
'', 
'', 
'0', 
crtNFE, 
'', 
'', 
'', 
'', 
'', 
filial);
INSERT INTO cbd001det (CbdEmpCodigo,  CbdNtfSerie, CbdNtfNumero, CbdnItem, CbdcProd, CbdcEAN, CbdxProd, CbdNCM, 
CbdEXTIPI, Cbdgenero, CbdCFOP, CbduCOM, CbdqCOM, CbdvUnCom, CbdvProd, CbdcEANTrib, CbduTrib, CbdqTrib, CbdvUnTrib,
CbdvFrete, CbdvSeg, CbdvDesc, CbdtpOp, Cbdchassi, CbdcCor, CbdxCor, Cbdpot, CbdCM3, CbdPesoL, CbdPesoB, CbdnSerie,
CbdtpComb, CbdnMotor, CbdCMKG, Cbddist, CbdRENAVAM, CbdanoMod, CbdanoFab, CbdtpPint, CbdtpVeic, CbdespVeic, CbdVIN,
CbdcondVeic, CbdcMod, CbdcProdANP, CbdCODIF, CbdqTemp, CbdqBCprod, CbdvAliqProd, CbdvCIDE, CbdvBCICMS, CbdvICMS,
CbdvBCICMSST, CbdvICMSST, CbdvBCICMSSTDest, CbdvICMSSTDest, CbdvBCICMSSTCons, CbdvICMSSTCons, CbdUFcons, CbdclEnq,
CbdCNPJProd, CbdcSelo, CbdqSelo, CbdcEnq, CbdvBC_imp, CbdvDespAdu, CbdvII, CbdvIOF, CbdvBC_issqn, CbdvAliq, 
CbdvISSQN, CbdcMunFg_issqn, CbdcListServ, CbdnTipoItem, CbdinfAdProd, 
CbdIndTot, CbdvOutro, CbdCilin, CbdCMT, CbdcCorDENATRAN, CbdcSitTrib, CbdxPed_item, CbdCodigoFilial)
SELECT 
filial, 
serieNF, 
numeroNFe, 
nrcontrole, 
codigo, 
codigobarras, 
produto, 
ncm,
nbm, 
IF (ncm<>'',LEFT(ncm,2),NULL), 
REPLACE(cfop, '.', ''), 
IF(unidade<>'',unidade,"UND"), 
quantidade, 
preco, 
total, 
codigobarras, 
IF(unidade<>'',unidade,"UND"), 
quantidade, 
preco, 
ratfrete, 
ratseguro, 
ratdesc, 
'0', 
'', 
'', 
'', 
'', 
'', 
'', 
'', 
'', 
'', 
'', 
'', 
'', 
'', 
'0', 
'0', 
'', 
'0', 
'0', 
'', 
'0', 
'0', 
(SELECT IF(p.cprodanp='', '0', p.cprodanp) FROM produtos as p WHERE p.codigo=vendas.codigo),
(SELECT IF(p.codif='', '0', p.codif) FROM produtos as p WHERE p.codigo=vendas.codigo),
quantidade,
((total+ratfrete+ratdespesas+ratseguro)-ratdesc),
(SELECT IF(p.cide>0, p.cide, 0) FROM produtos AS p WHERE p.codigo=vendas.codigo),
(((total+ratfrete+ratdespesas+ratseguro)-ratdesc)*(SELECT IF(p.cide>0, p.cide, 0) FROM produtos AS p WHERE p.codigo=vendas.codigo)/100),
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
(SELECT fCodigoEstadoMunIBGE("E","",(select estado from filiais where codigofilial=filial limit 1)) ) , 
'', 
'', 
'', 
'0', 
'', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'999999999', 
'0', 
'0', 
'', 
'1', 
ratdespesas, 
'', 
'', 
'', 
'', 
'', 
filial 
FROM vendas WHERE id=ipTerminal;
INSERT INTO cbd001detcofins 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnItem, CbdCST_cofins,
CbdvBC_cofins, 	CbdpCOFINS, CbdvCOFINS, CbdqBCProd_cofins, CbdvAliqProd_cofins, CbdCodigoFilial) 
select 
filial, 
serieNFe, 
numeroNFe, 
nrcontrole, 
cstcofins, 
if(cofins>0,total,0), 
cofins, 
IF(cofins>0,total*cofins/100,0), 
'0', 
'0', 
filial
from vendas where id=ipTerminal and cancelado="N";
INSERT INTO cbd001detipi 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnItem, CbdCST_IPI, CbdvBC_IPI, CbdqUnid_IPI,
CbdvUnid_IPI, CbdpIPI, CbdvIPI, CbdCodigoFilial) 
select
filial, 
serieNFe, 
numeroNFe, 
nrcontrole, 
cstIPI, 
TRUNCATE(fvBCIPI(filial,ipTerminal, vendas.codigo, vendas.inc), 2), 
qUnidIPI, 
vUnidIPI, 
aliquotaipi, 
fvIPI(filial,ipTerminal, vendas.codigo, vendas.inc),
filial 
FROM vendas WHERE id=ipTerminal AND cancelado="N";
if ( crtNFE="1") then
INSERT INTO cbd001deticmsnormalst 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnItem, CbdCST, Cbdorig, 
CbdmodBC, CbdvBC, CbdpICMS, CbdvICMS_icms, CbdmodBCST, CbdpMVAST, CbdpRedBCST, CbdvBCST, CbdpICMSST, 
CbdvICMSST_icms, CbdpRedBC, 
CbdvBCSTRet, CbdvICMSSTRet, CbdpBCOp, CbdUFST, CbdvBCSTDest, CbdvICMSSTDest_icms,
 CbdpCredSN, CbdvCredICMSSN, CbdCodigoFilial) 
 select 
filial, 
serieNFe, 
numeroNFe, 
nrcontrole,
tributacao, 
origem, 
`vendas`.modalidadeDetBaseCalcICMS, 
IF(icms>0 and (tributacao="10" or tributacao="70") , 0 ,0 ) , 
icms, 
IF(icms>0 AND (tributacao="10" OR tributacao="70"), 0 ,0), 
`vendas`.modalidadeDetBaseCalcICMSst, 
mvast, 
percentualRedBaseCalcICMSST, 
IF(icmsst>0 AND (tributacao="10" OR tributacao="70"), 0 ,0),
icmsst, 
IF(icmsst>0 AND (tributacao="10" OR tributacao="70"), 0 ,0),
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0',
filial
 FROM vendas WHERE id=ipTerminal AND cancelado="N";
end if;
IF ( crtNFE<>"1") THEN
INSERT INTO cbd001deticmsnormalst 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnItem, CbdCST, Cbdorig, 
CbdmodBC, CbdvBC, CbdpICMS, CbdvICMS_icms, CbdmodBCST, CbdpMVAST, CbdpRedBCST, CbdvBCST, CbdpICMSST, 
CbdvICMSST_icms, CbdpRedBC, 
CbdvBCSTRet, CbdvICMSSTRet, CbdpBCOp, CbdUFST, CbdvBCSTDest, CbdvICMSSTDest_icms,
 CbdpCredSN, CbdvCredICMSSN, CbdCodigoFilial) 
 select
filial, 
serieNFe, 
numeroNFe, 
nrcontrole,
tributacao, 
origem,  
'0', 
if(icms>0, TRUNCATE(fvBCICMS(filial, ipterminal, vendas.codigo, vendas.inc), 2), 0) , 
icms, 
IF(icms>0, TRUNCATE(fvICMS(filial, ipterminal, vendas.codigo, vendas.inc), 2), 0), 
'0', 
mvast, 
percentualRedBaseCalcICMSST, 
IF(icmsst>0, TRUNCATE(fvBCICMSst(filial, ipterminal, vendas.codigo, vendas.inc), 2) ,0),
icmsst, 
if(icmsst>0, TRUNCATE(fvICMSst(filial, ipterminal, vendas.codigo, vendas.inc), 2) ,0),
percentualRedBaseCalcICMS, 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
filial
 FROM vendas WHERE id=ipTerminal AND cancelado="N";
END IF;
INSERT INTO cbd001detmed (CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, 
CbdnItem, CbdnLote, CbdqLote, CbddFab, CbddVal, CbdvPMC, CbdCodigoFilial) 
select 
filial, 
serieNFe, 
numeroNFe, 
nrcontrole, 
lote, 
'0', 
datafabricacao, 
vencimentoproduto, 
'0', 
filial
FROM vendas WHERE id=ipTerminal AND cancelado="N";
INSERT INTO cbd001detpis 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnItem, CbdCST_pis, CbdvBC_pis, 
CbdpPIS, CbdvPIS, CbdqBCprod_pis, CbdvAliqProd_pis, CbdCodigoFilial) 
SELECT 
filial, 
serieNFe, 
numeroNFe, 
nrcontrole, 
cstcofins, 
IF(pis>0,total,0), 
cofins, 
IF(pis>0,total*pis/100,0), 
'0', 
'0', 
filial
FROM vendas WHERE id=ipTerminal AND cancelado="N";
INSERT INTO cbd001duplicatas 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnDup, CbddVenc, CbdvDup, CbdCodigoFilial) 
select 
filial, 
serieNFe, 
numeroNFe, 
IF(tipopagamento<>'OU',CONCAT(tipopagamento,LEFT(nrparcela,1)), CONCAT('OUTROS ',LEFT(nrparcela,1)) ), 
vencimento, 
valor, 
filial
from caixas where EnderecoIP=ipTerminal and tipopagamento<>'DH' group by vencimento;
 insert into cbd001lacres 
 (CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnLacre, CbdCodigoFilial) 
 values (
 filial,
 serieNFe,
 numeroNFe,
 '',
filial);
insert into cbd001procref 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnProc, CbdindProc, CbdCodigoFilial)
values (
 filial,
 serieNFe,
 numeroNFe,
 ' ',
 '0',
 filial); 
 
 INSERT INTO cbd001vol 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnVol, CbdqVol, Cbdesp, Cbdmarca,
CbdpesoL_transp, CbdpesoB_transp, CbdCodigoFilial) 
VALUES (
filial, 
serieNFe, 
numeroNFe, 
numeroNFe, 
volumeNFe, 
especieVolumeNFe, 
(SELECT marca FROM contnfsaida WHERE numero=numeroNFe AND serie=serieNFe limit 1), 
pesoLiquidoNFe, 
pesoBrutoNFe, 
filial);
insert into nfe012 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdAcao, CbdProcStatus, CbdCodigoFilial) 
values (
filial,
serieNFe,
numeroNFe,
'E',
if ( (select previsualizarnfe from configfinanc where codigofilial=filial limit 1)="N",'N','V'),
filial
 );
 if (doc>0) then
	OPEN cursorCupons;
	read_loop: LOOP
		IF done THEN
			LEAVE read_loop;
		END IF;
		FETCH cursorCupons INTO docCupom;
 UPDATE contdocs 
    SET nrnotafiscal=numeroNFe,
    serienf=serieNFe
    WHERE documento=docCupom LIMIT 1;	
  UPDATE crmovclientes SET nfe=numeroNFe WHERE documento=doc;
		
	END LOOP;
	CLOSE cursorCupons;
 
 end if;
 IF ( crtNFE="1") THEN
 
                      update cbd001deticmsnormalst set cbdcst="102" 
                      where (cbdcst="0" or cbdcst="20")
                      and CbdEmpCodigo=filial
                      and CbdNtfSerie= serieNFe
                       and CbdNtfNumero= numeroNFe
                       and CbdCodigoFilial=filial;
 
                       UPDATE cbd001deticmsnormalst SET cbdcst="202" 
                      WHERE (cbdcst="10")
                      AND CbdEmpCodigo=filial
                      AND CbdNtfSerie= serieNFe
                       AND CbdNtfNumero= numeroNFe
                       AND CbdCodigoFilial=filial;
 
                       UPDATE cbd001deticmsnormalst SET cbdcst="102" 
                      WHERE (cbdcst="40" OR cbdcst="41")
                      AND CbdEmpCodigo=filial
                      AND CbdNtfSerie= serieNFe
                       AND CbdNtfNumero= numeroNFe
                       AND CbdCodigoFilial=filial;
 
                       UPDATE cbd001deticmsnormalst SET cbdcst="400" 
                      WHERE (cbdcst="50" OR cbdcst="51")
                      AND CbdEmpCodigo=filial
                      AND CbdNtfSerie= serieNFe
                       AND CbdNtfNumero= numeroNFe
                       AND CbdCodigoFilial=filial;
 
                       UPDATE cbd001deticmsnormalst SET cbdcst="202" 
                      WHERE (cbdcst="30")
                      AND CbdEmpCodigo=filial
                      AND CbdNtfSerie= serieNFe
                       AND CbdNtfNumero= numeroNFe
                       AND CbdCodigoFilial=filial;
 
                       UPDATE cbd001deticmsnormalst SET cbdcst="500" 
                      WHERE (cbdcst="60")
                      AND CbdEmpCodigo=filial
                      AND CbdNtfSerie= serieNFe
                       AND CbdNtfNumero= numeroNFe
                       AND CbdCodigoFilial=filial;
 
                       UPDATE cbd001deticmsnormalst SET cbdcst="900" 
                      WHERE (cbdcst="90")
                      AND CbdEmpCodigo=filial
                      AND CbdNtfSerie= serieNFe
                       AND CbdNtfNumero= numeroNFe
                       AND CbdCodigoFilial=filial;
 
                       UPDATE cbd001deticmsnormalst SET cbdcst="202" 
                      WHERE (cbdcst="30")
                      AND CbdEmpCodigo=filial
                      AND CbdNtfSerie= serieNFe
                       AND CbdNtfNumero= numeroNFe
                       AND CbdCodigoFilial=filial;
 
 
 end if;
 
 
END */$$
DELIMITER ;

/* Procedure structure for procedure `GerarNumeroBoleto` */

/*!50003 DROP PROCEDURE IF EXISTS  `GerarNumeroBoleto` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `GerarNumeroBoleto`(in filial varchar(5),in doc int)
BEGIN
DECLARE done INT DEFAULT FALSE;
DECLARE seq INT DEFAULT 0;
DECLARE cursorSeq CURSOR FOR SELECT sequenciainc FROM crmovclientes where documento=doc and bloquete=""
order by vencimento; 
DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
 
update contdocs set nrboletobancario=(SELECT cobrebemproximonumero FROM contasbanco WHERE
conta=(SELECT conta FROM filiais WHERE codigofilial=filial))
WHERE documento=doc and nrboletobancario=0;
	OPEN cursorSeq;
	read_loop: LOOP
		FETCH cursorSeq INTO seq;	
		IF done THEN
			LEAVE read_loop;
		END IF; 
 
 IF (SELECT conta FROM filiais WHERE codigofilial=filial<>'') THEN
UPDATE crmovclientes SET bloquete=(SELECT cobrebemproximonumero FROM contasbanco WHERE
conta=(SELECT conta FROM filiais WHERE codigofilial=filial))
WHERE sequenciainc=seq;
       UPDATE contasbanco SET 
       cobrebemproximonumero=cobrebemproximonumero+1
       WHERE conta=(SELECT conta FROM filiais WHERE codigofilial=filial);
	
END IF;
	END LOOP;
	CLOSE cursorSeq;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `GravarR` */

/*!50003 DROP PROCEDURE IF EXISTS  `GravarR` */;

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

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarDAV` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProcessarDAV`(in numeroDAV int,in ipTerminal varchar(15),in filial varchar(5))
BEGIN
 
DECLARE done INT DEFAULT FALSE;
DECLARE idSeq INT DEFAULT 0;
DECLARE seq INT DEFAULT 1;
DECLARE cursorSeq CURSOR FOR SELECT inc FROM vendas where vendas.id=ipTerminal ;
DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
 SET @tabelaProduto='produtosfilial';
 IF (filial='00001') THEN
 SET @tabelaProduto='produtos';
 END IF;
 
 
 UPDATE vendadav 
 SET 	codigofilial=filial,id=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial;
 
 INSERT INTO `vendas` (`acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`,`idfornecedor`,`icmsst`, `mvast`, `cfop`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`,`idfornecedor`,`icmsst`, `mvast`, `cfop`
 FROM vendadav where documento=numeroDAV AND codigofilial=filial and documento>0;
 
 OPEN cursorSeq;
	read_loop: LOOP
		FETCH cursorSeq INTO idSeq;	
		IF done THEN
			LEAVE read_loop;
		END IF;
	
	update vendas set nrcontrole=seq 
	where vendas.inc = idSeq;
	
	set seq = seq+1;
	
	
	
END LOOP;
	CLOSE cursorSeq;	
 
 UPDATE caixadav 
 SET	codigofilial=filial,enderecoip=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial and documento>0;
 
 INSERT INTO `caixas` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixadav where documento=numeroDAV AND codigofilial=filial and documento>0;
 
 
 SET @qVenda = CONCAT('UPDATE vendas,',@tabelaProduto,' set  
 vendas.quantidadeanterior=',@tabelaProduto,'.quantidade,
 vendas.quantidadeatualizada=',@tabelaProduto,'.quantidade-vendas.quantidade	
 where vendas.id=','"',ipTerminal,'" and vendas.codigo=',@tabelaProduto,'.codigo 
 and ',@tabelaProduto,'.codigofilial=',filial);	
 PREPARE st FROM @qVenda;
 EXECUTE st;
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarDAVOS` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarDAVOS` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProcessarDAVOS`(in numeroDAV int,in ipTerminal varchar(15),in filial varchar(5))
BEGIN
 UPDATE vendadavos 
 SET 	codigofilial=filial,id=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial;
 
 INSERT INTO `vendas` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`) 
 SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`
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

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarDevolucao` */;

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
 ' AND devolucao.numero>0 AND ',@tabelaProduto,'.codigofilial=',filial);
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
 UPDATE	contdocs SET observacao=(select observacao from devolucao where numero=devolucaoNR AND observacao<>'' limit 1),
 devolucaonumero=devolucaoNR WHERE documento=doc;
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
INSERT INTO caixa (documento,enderecoip,codigofilial,valor,data,dataexe,tipopagamento,operador,vendedor,dpfinanceiro)
values (0,ipTerminal,filial,(select ifnull(sum(preco*quantidade),0) from devolucao where numero=devolucaoNR),current_date,current_date,"DV",operadorAcao,
(select vendedor from devolucao where numero=devolucaoNR limit 1),IFNULL((select dpfinanceiro from contdocs where documento=doc),"Venda") );
END IF;
 UPDATE  produtosgrade,devolucao SET 
 produtosgrade.quantidade=produtosgrade.quantidade+devolucao.quantidade 
 WHERE 	devolucao.numero=devolucaoNR
 AND 	devolucao.numero>0
 AND 	produtosgrade.codigo=devolucao.codigo
 AND 	produtosgrade.grade=devolucao.grade 
 AND 	produtosgrade.codigofilial=filial;
 DELETE FROM caixa WHERE tipopagamento="DV" and enderecoip=ipTerminal AND (documento IS NULL or documento=0);
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarEntrada` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarEntrada` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProcessarEntrada`(in filial varchar(5), in numeroEntrada int,in alterar char(1),in operadorLanc varchar(10),in pedCompra int,in numeroProducao int, in numeroTransf int)
BEGIN
DECLARE totalFreteNFe REAL DEFAULT 0;
DECLARE totalSeguroNFe REAL DEFAULT 0;
DECLARE totalDespesasNFe REAL DEFAULT 0;
declare despesasNFe real default 0;
DECLARE totalDescontoNFe REAL DEFAULT 0;
declare seguroNFe real default 0;
declare freteNFe real default 0;
DECLARE descontoNFe real default 0;
declare totalBrutoNFe real default 0;
declare cursorDespesas cursor for select despesas from moventradas where numero=numeroEntrada limit 1;
DECLARE cursorDesconto CURSOR FOR SELECT descontos FROM moventradas WHERE numero=numeroEntrada LIMIT 1;
DECLARE cursorSeguro CURSOR FOR SELECT valorseguro FROM moventradas WHERE numero=numeroEntrada limit 1;
DECLARE cursorFrete CURSOR FOR SELECT frete FROM moventradas WHERE numero=numeroEntrada limit 1;
DECLARE cursorTotalBruto CURSOR FOR SELECT valorprodutos FROM moventradas WHERE numero=numeroEntrada LIMIT 1;
open cursorDespesas;
open cursorDesconto;
open cursorSeguro;
open cursorFrete;
open cursorTotalBruto;
fetch cursorDespesas into despesasNFe;
FETCH cursorDesconto into descontoNFe; 
fetch cursorSeguro into seguroNFe;
fetch cursorFrete into freteNFe;
fetch cursorTotalBruto into totalBrutoNFe;
if (numeroTransf>0) then
update moventradas set lancada="X",data=current_date,horaentrada=current_time WHERE numero=numeroEntrada;
 UPDATE entradas SET lancada="X" WHERE numero=numeroEntrada;
 UPDATE entradas SET cfopentrada=(SELECT cfopentrada FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada AND cfopentrada IS NULL;
 UPDATE entradas SET dataentrada=(SELECT dataentrada FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada;
 UPDATE entradas SET modeloNF=(SELECT modelonf FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada;
UPDATE entradas SET dataemissao=(SELECT moventradas.DataEmissao FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada;
  
END IF; 
if (numeroTransf=0) then
if (alterar="S") then
if (filial="00001") then
Update produtos,entradas 
set produtos.qtdanterior=produtos.quantidade,
produtos.customedioanterior=produtos.customedio,
produtos.quantidade=produtos.quantidade+(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
produtos.qtdultent=(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
produtos.ultcusto=produtos.custo,
produtos.custo=entradas.custocalculado,
produtos.custofornecedor=entradas.custo,
produtos.idfornecedor=(select moventradas.codigofornecedor FROM moventradas WHERE  moventradas.numero=numeroEntrada),
produtos.dataultent=current_date,
produtos.ultpreco=produtos.precovenda,
produtos.fornecedor=entradas.fornecedor,
produtos.precovenda=entradas.precovenda,
produtos.precoatacado=entradas.precoatacado,
produtos.margemlucro=entradas.margemlucro,
produtos.marcado='X',
produtos.ipi=entradas.ipi,
produtos.frete=entradas.frete,
produtos.operador=operadorLanc,
produtos.lote=entradas.lote,
produtos.vencimento=entradas.vencimento,
produtos.datafabricacao=entradas.datafabricacao,
produtos.qtdprateleiras=produtos.qtdprateleiras+(SELECT SUM(entradas.qtdprateleiras) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
produtos.dataaltpreco=current_date 
where produtos.codigo=entradas.codigo 
and entradas.numero=numeroEntrada
and entradas.lancada<>'X'
and produtos.CodigoFilial=filial;
update produtos,entradas 
set produtos.customedio=IFNULL(( ( (entradas.quantidade*entradas.custo)+( (produtos.qtdanterior+produtos.qtdretida)*produtos.ultcusto) ) / (produtos.quantidade+produtos.qtdretida)),produtos.customedio) 
where produtos.codigo=entradas.codigo and entradas.numero=numeroEntrada
 and entradas.lancada<>'X'
 and produtos.codigofilial=filial;
 
update entradas,produtos 
set entradas.customedio=produtos.customedio,
entradas.quantidadeanterior=produtos.qtdanterior,
entradas.quantidadeatualizada=produtos.quantidade,
entradas.serienf='1',
entradas.customedioanterior=produtos.customedioanterior,
entradas.operacao=' .   ',
entradas.lancada='X',
entradas.icms=produtos.icms 
 where entradas.numero=numeroEntrada
 and entradas.codigo=produtos.codigo 
 and entradas.codigofilial=produtos.codigofilial 
 and produtos.codigofilial=filial; 
end if;
IF (filial<>"00001") THEN
UPDATE produtosfilial as produtos,entradas 
SET produtos.qtdanterior=produtos.quantidade,
produtos.customedioanterior=produtos.customedio,
produtos.quantidade=produtos.quantidade+(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
produtos.qtdultent=entradas.quantidade,
produtos.ultcusto=produtos.custo,
produtos.custo=entradas.custocalculado,
produtos.custofornecedor=entradas.custo,
produtos.idfornecedor=(SELECT moventradas.codigofornecedor FROM moventradas WHERE  moventradas.numero=numeroEntrada),
produtos.dataultent=CURRENT_DATE,
produtos.ultpreco=produtos.precovenda,
produtos.fornecedor=entradas.fornecedor,
produtos.precovenda=entradas.precovenda,
produtos.precoatacado=entradas.precoatacado,
produtos.margemlucro=entradas.margemlucro,
produtos.marcado='X',
produtos.ipi=entradas.ipi,
produtos.frete=entradas.frete,
produtos.operador=operadorLanc,
produtos.lote=entradas.lote,
produtos.vencimento=entradas.vencimento,
produtos.datafabricacao=entradas.datafabricacao,
produtos.qtdprateleiras=produtos.qtdprateleiras+(SELECT SUM(entradas.qtdprateleiras) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
produtos.dataaltpreco=CURRENT_DATE 
WHERE produtos.codigo=entradas.codigo 
AND entradas.numero=numeroEntrada
AND entradas.lancada<>'X'
AND produtos.CodigoFilial=filial;
UPDATE produtosfilial as produtos,entradas 
SET produtos.customedio=IFNULL(( ( (entradas.quantidade*entradas.custo)+( (produtos.qtdanterior+produtos.qtdretida)*produtos.ultcusto) ) / (produtos.quantidade+produtos.qtdretida)),produtos.customedio) 
WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
 AND entradas.lancada<>'X'
 AND produtos.codigofilial=filial;
update entradas,produtosfilial as produtos 
set entradas.customedio=produtos.customedio,
entradas.quantidadeanterior=produtos.qtdanterior,
entradas.quantidadeatualizada=produtos.quantidade,
entradas.serienf='1',
entradas.customedioanterior=produtos.customedioanterior,
entradas.operacao=' .   ',
entradas.lancada='X',
entradas.icms=produtos.icms 
 where entradas.numero=numeroEntrada 
 and entradas.codigo=produtos.codigo 
 and entradas.codigofilial=produtos.codigofilial 
 and produtos.codigofilial=filial;
END IF;
end if; 
IF (alterar="N") THEN
IF (filial="00001") THEN
UPDATE produtos,entradas 
SET produtos.qtdanterior=produtos.quantidade,
produtos.quantidade=produtos.quantidade+(select sum(entradas.quantidade) from entradas where entradas.codigo=produtos.codigo and entradas.numero=numeroEntrada),
produtos.qtdultent=(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
produtos.dataultent=CURRENT_DATE,
produtos.marcado='X',
produtos.operador=operadorLanc,
produtos.lote=entradas.lote,
produtos.vencimento=entradas.vencimento,
produtos.datafabricacao=entradas.datafabricacao,
produtos.qtdprateleiras=produtos.qtdprateleiras+(SELECT SUM(entradas.qtdprateleiras) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada) 
WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada and entradas.Lancada<>'X';
UPDATE entradas,produtos 
SET entradas.quantidadeanterior=produtos.qtdanterior,
entradas.quantidadeatualizada=produtos.quantidade,
entradas.operacao=' .   '
 WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada;
END IF;
IF (filial<>"00001") THEN
UPDATE produtosfilial as produtos,entradas 
SET produtos.qtdanterior=produtos.quantidade,
produtos.quantidade=produtos.quantidade+(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
produtos.qtdultent=(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
produtos.dataultent=CURRENT_DATE,
produtos.marcado='X',
produtos.operador=operadorLanc,
produtos.lote=entradas.lote,
produtos.vencimento=entradas.vencimento,
produtos.datafabricacao=entradas.datafabricacao,
produtos.qtdprateleiras=produtos.qtdprateleiras+(SELECT SUM(entradas.qtdprateleiras) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada) 
WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
and produtos.CodigoFilial=filial AND entradas.Lancada<>'X';
UPDATE entradas,produtosfilial as produtos 
SET entradas.quantidadeanterior=produtos.qtdanterior,
entradas.quantidadeatualizada=produtos.quantidade,
entradas.operacao=' .   '
 WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
 and produtos.CodigoFilial=filial ;
END IF;
end if;
UPDATE entradas 
SET customedio=custocalculado 
 WHERE numero=numeroEntrada
 AND customedio=0;
  
UPDATE produtosgrade,entradagrade
SET produtosgrade.quantidade=produtosgrade.quantidade+entradagrade.quantidade 
 WHERE entradagrade.numero=numeroEntrada
 AND produtosgrade.codigo=entradagrade.codigo
 AND produtosgrade.grade=entradagrade.grade 
 AND produtosgrade.codigofilial=filial
 and entradagrade.origem='Entrada';
 
 IF (numeroProducao>0) THEN 
 UPDATE 
    produtosgrade,
    producao 
  SET
    produtosgrade.quantidade = produtosgrade.quantidade + 
    (SELECT 
      SUM(producao.quantidade) 
    FROM
      producao 
    WHERE producao.codigo = produtosgrade.codigo 
      AND producao.grade = produtosgrade.grade 
      AND producao.numero = numeroProducao) 
  WHERE producao.numero = numeroProducao 
    AND produtosgrade.codigo = producao.codigo 
    AND produtosgrade.grade = producao.grade 
    AND produtosgrade.codigofilial = filial 
    AND producao.grade <> 'nenhuma' ;
 
	IF (filial="00001") THEN 
	update produtos,produtoscomposicao,producao 
                       set produtos.quantidade=produtos.quantidade-(produtoscomposicao.quantidade*(select sum(producao.quantidadeentrada) from producao where producao.codigo=produtoscomposicao.codigo and numero=numeroProducao) )                       
                       where produtoscomposicao.codigomateria=produtos.codigo and producao.numero=numeroProducao
                        and produtoscomposicao.codigo=producao.codigo 
                        and producao.quantidadeentrada>0;
	END IF;
	IF (filial<>"00001") THEN 
	UPDATE produtosfilial as produtos,produtoscomposicao,producao 
                       SET produtos.quantidade=produtos.quantidade-(produtoscomposicao.quantidade*(SELECT SUM(producao.quantidadeentrada) FROM producao WHERE producao.codigo=produtoscomposicao.codigo AND numero=numeroProducao)) 
                       WHERE produtoscomposicao.codigomateria=produtos.codigo AND producao.numero=numeroProducao
                        AND produtoscomposicao.codigo=producao.codigo 
                        AND producao.quantidadeentrada>0;
	END IF;
 
 
	update contproducao set finalizado='S',
	datafinalizado=current_date,operadorfinalizacao=operadorLanc
	where numero=numeroProducao;
 END IF;
 
 if (pedCompra>0) then
 update pedidocompra,entradas
set pedidocompra.qtdrecebida=(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=pedidocompra.codigo AND entradas.numero=numeroEntrada) 
 where entradas.numero=numeroEntrada
 and pedidocompra.numero=pedCompra
 and entradas.codigo=pedidocompra.codigo;
update pedidocompra set pedidocompra.diferencaqtdrecebida=pedidocompra.quantidade-pedidocompra.qtdrecebida 
 where pedidocompra.numero=pedCompra;
update contpedido set encerrado='S'
 where numero=pedCompra;
 end if;
 
 update moventradas set lancada="X",data=current_date,horaentrada=current_time where numero=numeroEntrada;
 update entradas set lancada="X" where numero=numeroEntrada;
 update entradas set cfopentrada=(SELECT cfopentrada FROM moventradas WHERE numero=numeroEntrada LIMIT 1) where numero=numeroEntrada and cfopentrada is null;
 UPDATE entradas SET dataentrada=(SELECT dataentrada FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada;
 
 UPDATE entradas SET fornecedor=(SELECT fornecedor FROM moventradas WHERE numero=numeroEntrada LIMIT 1),codfornecedor=(SELECT codigofornecedor FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada;
 
 UPDATE entradas SET icmsst=0,bcicmsst=0,entradas.valoricmsST=0 WHERE tributacao<>"010" AND tributacao<>"030" AND tributacao<>"070" and numero=numeroEntrada;
  
 IF (despesasNFe>0) THEN
UPDATE entradas SET ratdespesas=TRUNCATE((totalitem)*  ( (despesasNFe*100/totalBrutoNFe) /100 ) ,2) 
WHERE numero=numeroEntrada;
SET totaldespesasNFe = (SELECT IFNULL( SUM(ratdespesas) ,0 ) FROM entradas WHERE numero=numeroEntrada);
IF (despesasNFe<>(SELECT SUM(ratdespesas) FROM entradas WHERE numero=numeroEntrada) ) THEN
UPDATE entradas SET ratdespesas=ratdespesas+(despesasNFe)-totaldespesasNFe
WHERE numero=numeroEntrada LIMIT 1;
END IF;
SET totaldespesasNFe = (SELECT IFNULL( SUM(ratdespesas) ,0 ) FROM entradas WHERE numero=numeroEntrada);
END IF;
 
IF (descontoNFe>0) THEN
UPDATE entradas SET ratdesconto=TRUNCATE((totalitem)*  ( (descontoNFe*100/totalBrutoNFe) /100 ) ,2) 
WHERE numero=numeroEntrada;
SET totalDescontoNFe = (SELECT IFNULL( SUM(ratdesconto) ,0 ) FROM entradas WHERE numero=numeroEntrada);
IF (totalDescontoNFe<>(SELECT SUM(ratdesconto) FROM entradas WHERE numero=numeroEntrada) ) THEN
UPDATE entradas SET ratdesconto=ratdesconto+(descontoNFe)-totalDescontoNFe
WHERE numero=numeroEntrada LIMIT 1;
END IF;
SET totalDescontoNFe = (SELECT IFNULL( SUM(ratdesconto) ,0 ) FROM entradas WHERE numero=numeroEntrada);
END IF;
IF (freteNFe>0) THEN
UPDATE entradas SET ratfrete=TRUNCATE((totalitem)*  ( (freteNFe*100/totalBrutoNFe) /100 ) ,2) 
WHERE numero=numeroEntrada;
SET totalFreteNFe = (SELECT IFNULL( SUM(ratfrete) ,0 ) FROM entradas WHERE numero=numeroEntrada);
IF (freteNFe<>(SELECT SUM(ratfrete) FROM entradas WHERE numero=numeroEntrada) ) THEN
UPDATE entradas SET ratfrete=ratfrete+(freteNFe)-totalFreteNFe
WHERE numero=numeroEntrada LIMIT 1;
END IF;
SET totalFreteNFe = (SELECT IFNULL( SUM(ratfrete) ,0 ) FROM entradas WHERE numero=numeroEntrada);
END IF;
 
IF (seguroNFe>0) THEN
UPDATE entradas SET ratseguro=TRUNCATE((totalitem)*  ( (seguroNFe*100/totalBrutoNFe) /100 ) ,2) 
WHERE numero=numeroEntrada;
SET totalseguroNFe = (SELECT IFNULL( SUM(ratseguro) ,0 ) FROM entradas WHERE numero=numeroEntrada);
IF (seguroNFe<>(SELECT SUM(ratseguro) FROM entradas WHERE numero=numeroEntrada) ) THEN
UPDATE entradas SET ratseguro=ratseguro+(seguroNFe)-totalSeguroNFe
WHERE numero=numeroEntrada LIMIT 1;
END IF;
SET totalseguroNFe = (SELECT IFNULL( SUM(ratseguro) ,0 ) FROM entradas WHERE numero=numeroEntrada);
END IF; 
end if; 
 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarPedidoCompra` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarPedidoCompra` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProcessarPedidoCompra`(in numeroEntrada int,in pedCompra int,in encerrarPedido char(1))
BEGIN
 UPDATE pedidocompra,entradas
SET pedidocompra.qtdrecebida=entradas.quantidade 
 WHERE entradas.numero=numeroEntrada
 AND pedidocompra.numero=pedCompra
 AND entradas.codigo=pedidocompra.codigo;
UPDATE pedidocompra SET pedidocompra.diferencaqtdrecebida=pedidocompra.quantidade-pedidocompra.qtdrecebida 
 WHERE pedidocompra.numero=pedCompra;
 if (encerrarPedido="S") then
UPDATE contpedido SET encerrado='S'
 WHERE numero=pedCompra;
 end if;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarPreVenda` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarPreVenda` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProcessarPreVenda`(in numeroPreVenda int,in ipTerminal varchar(15),in filial varchar(5))
BEGIN
 UPDATE vendaprevendapaf 
 SET 	codigofilial=filial,id=ipTerminal
 WHERE 	documento=numeroPreVenda AND codigofilial=filial;
 
 INSERT INTO `vendas` (`acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`
 FROM vendaprevendapaf where documento=numeroPreVenda and codigofilial=filial;
 
 UPDATE caixaprevendapaf 
 SET	codigofilial=filial,enderecoip=ipTerminal
 WHERE 	documento=numeroPreVenda AND codigofilial=filial;
 
 INSERT INTO `caixas` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixaprevendapaf where documento=numeroPreVenda AND codigofilial=filial;
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarTransferencia` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarTransferencia` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProcessarTransferencia`(
  in numeroTransf int,
  in filial varchar (5),
  in filialOrigem varchar (5),
  in filialDestino varchar (5),  
  in operadorLanc varchar (10)
)
BEGIN
 SET @tabelaOrigem='produtos';
 IF (filialOrigem<>'00001') THEN
 SET @tabelaOrigem='produtosfilial';
 END IF;
 
  SET @tabelaDestino='produtosfilial';
 IF (filialDestino='00001') THEN
 SET @tabelaDestino='produtos';
 END IF;
 
 
 SET @queryOrigem = CONCAT('update ',@tabelaOrigem,' as produtos,
    movtransf 
  set
    produtos.qtdanterior = produtos.quantidade,
    produtos.quantidade = produtos.quantidade - 
    (select 
      sum(movtransf.quantidade) 
    from
      movtransf 
    where movtransf.codigo = produtos.codigo 
      and movtransf.numero = ',numeroTransf,'),
    produtos.qtdprateleiras = produtos.qtdprateleiras - 
    (select 
      sum(movtransf.prateleiras) 
    from
      movtransf 
    where movtransf.codigo = produtos.codigo 
      and movtransf.numero = ',numeroTransf,'),
    produtos.documento = ',numeroTransf,'
  where produtos.codigo = movtransf.codigo 
    and produtos.codigofilial = ',filialOrigem,'
    and produtos.documento <> ',numeroTransf,' 
    and movtransf.numero = ',numeroTransf) ;
 
  PREPARE st FROM @queryOrigem;
 EXECUTE st;
  
 
 update 
    produtosgrade,
    movtransf 
  set
    produtosgrade.quantidade = produtosgrade.quantidade - 
    (select 
      sum(movtransf.quantidade) 
    from
      movtransf 
    where movtransf.codigo = produtosgrade.codigo 
      and movtransf.grade = produtosgrade.grade 
      and movtransf.numero = numeroTransf) 
  where movtransf.numero = numeroTransf 
    and produtosgrade.codigo = movtransf.codigo 
    and produtosgrade.grade = movtransf.grade 
    and produtosgrade.codigofilial = filialOrigem 
    and movtransf.grade <> 'nenhuma' ;
 SET @queryDestino = CONCAT('update ',@tabelaDestino,' as produtosfilial,
    movtransf 
  set
    movtransf.customedioanteriordestino = produtosfilial.customedio,
    movtransf.quantidadeanteriordestino = produtosfilial.quantidade,
    movtransf.quantidadeatualizadadestino = produtosfilial.quantidade + movtransf.quantidade,
    movtransf.customediorefeitodestino = produtosfilial.customedio 
  where produtosfilial.codigo = movtransf.codigo 
    and produtosfilial.codigofilial = ', filialDestino,'
    and produtosfilial.documento <> ',numeroTransf,' 
    and movtransf.numero = ',numeroTransf) ;
  PREPARE st FROM @queryDestino;
  EXECUTE st;
 
 SET @queryAlterar = ',produtosfilial.precovenda = movtransf.preco,produtosfilial.custo = movtransf.custo ';
 IF ( (SELECT alterarprecotransferencia FROM configfinanc WHERE codigofilial=filial LIMIT 1)="N") THEN
 SET @queryAlterar = "";
 END IF;
 
 
    SET @queryDestino2 = CONCAT('update ',@tabelaDestino,' as produtosfilial,
    movtransf 
   set
    produtosfilial.qtdanterior = produtosfilial.quantidade,
    produtosfilial.quantidade = produtosfilial.quantidade + 
    (select 
      sum(movtransf.quantidade) 
    from
      movtransf 
    where movtransf.codigo = produtosfilial.codigo 
      and movtransf.numero = ',numeroTransf,'),
    produtosfilial.documento = ',numeroTransf,',
    produtosfilial.qtdprateleiras = produtosfilial.qtdprateleiras + 
    (select 
      sum(movtransf.prateleirasdestino) 
    from
      movtransf 
    where movtransf.codigo = produtosfilial.codigo 
      and movtransf.numero = ',numeroTransf,'), 
 
    produtosfilial.customedio = movtransf.customedio',@queryAlterar,'   
    where produtosfilial.codigo = movtransf.codigo 
    and produtosfilial.codigofilial = ',filialDestino,'
    and produtosfilial.documento <> ',numeroTransf,' 
    and movtransf.numero = ',numeroTransf ) ;
   PREPARE st FROM @queryDestino2;
  EXECUTE st;
 
 
      SET @queryDestino3 = CONCAT('update ',@tabelaDestino,' as produtosfilial,
    movtransf 
  set
    movtransf.quantidadeatualizadadestino = produtosfilial.quantidade 
  where produtosfilial.codigo = movtransf.codigo 
    and produtosfilial.codigofilial = ',filialDestino,'
    and produtosfilial.documento <> ',numeroTransf,' 
    and movtransf.numero = ',numeroTransf ); 
    PREPARE st FROM @queryDestino3;
    EXECUTE st;
  
 update 
    produtosgrade,
    movtransf 
  set
    produtosgrade.quantidade = produtosgrade.quantidade + 
    (select 
      sum(movtransf.quantidade) 
    from
      movtransf 
    where movtransf.codigo = produtosgrade.codigo 
      and movtransf.grade = produtosgrade.grade 
      and movtransf.numero = numeroTransf) 
  where movtransf.numero = numeroTransf 
    and produtosgrade.codigo = movtransf.codigo 
    and produtosgrade.grade = movtransf.grade 
    and produtosgrade.codigofilial = filialDestino 
    and movtransf.grade <> 'nenhuma' ;
  
 
 update 
    movtransf 
  set
    lancada = 'X',
    data = current_date,
    usuario = operadorLanc 
  where numero = numeroTransf ;
  update 
    conttransf 
  set
    lancada = 'X' 
  where numero = numeroTransf ;
END */$$
DELIMITER ;

/* Procedure structure for procedure `QuitarDebitoCliente` */

/*!50003 DROP PROCEDURE IF EXISTS  `QuitarDebitoCliente` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `QuitarDebitoCliente`(in codigoCliente int,in doc int,in ipTerminal varchar(15),in filial varchar(5))
BEGIN
 UPDATE caixas SET 
 documento=doc,codigofilial=filial,
 dpfinanceiro=(select contdocs.dpfinanceiro from contdocs where contdocs.documento=doc),
 operador=(select contdocs.operador from contdocs where contdocs.documento=doc),
 classe=(select contdocs.classe from contdocs where contdocs.documento=doc),
 vendedor=(select contdocs.vendedor from contdocs where contdocs.documento=doc),
 historico=(select contdocs.dpfinanceiro from contdocs where contdocs.documento=doc),
 nome=(SELECT contdocs.nome FROM contdocs WHERE contdocs.documento=doc)
 WHERE  caixas.enderecoip=ipTerminal
 and codigofilial=filial;
 
 UPDATE caixas SET 
 vrdesconto=(select contdocs.desconto from contdocs where contdocs.documento=doc),
 vrjuros=(select contdocs.vrjuros from contdocs where contdocs.documento=doc),
 cobrador = (select cobrador from crmovclientes where codigo=codigoCliente and ip=ipTerminal and quitado="S" limit 1) 
 WHERE caixas.enderecoip=ipTerminal and codigofilial=filial limit 1;
 
 
 UPDATE caixas SET  
 valortarifabloquete = (SELECT contasbanco.taxarecbloquete FROM contasbanco WHERE contasbanco.conta= (SELECT filiais.conta FROM filiais WHERE codigofilial=filial LIMIT 1)  )  
 WHERE caixas.enderecoip=ipTerminal AND codigofilial=filial and valortarifabloquete=0 and tipopagamento="BL" LIMIT 1;
 
 INSERT INTO movcartoes (codigofilial,documento,cartao,numero,data,
 vencimento,valor,operador,dpfinanceiro,encargos,nome)
 SELECT 	codigofilial,documento,cartao,numerocartao,current_date,vencimento,
 valor,operador,dpfinanceiro,
 (select encargos from contdocs where documento=doc),
 '0'
 FROM 	caixas
 WHERE 	enderecoip=ipTerminal and codigofilial=filial
 AND 	tipopagamento IN ('CA','FI','TI','FN');
 
 INSERT INTO cheques (codigofilial,documento,banco,cheque,agencia,data,
 valor,valorcheque,vencimento,cliente,codigocliente,nomecheque,dpfinanceiro,
 cpf,cpfcheque,telefone,encargos) 
 SELECT codigofilial,documento,banco,cheque,agencia,data,valor,valorcheque,vencimento,
 nome,codigocliente,nomecheque,dpfinanceiro,
 (select if(cpf<>'',cpf,cnpj) from clientes where codigo=(select codigocliente from contdocs where documento=doc)),	
 cpfcnpjch,(select telefone from clientes where codigo=(select codigocliente from contdocs where documento=doc)),
 (select encargos from contdocs where documento=doc)
 FROM caixas where enderecoip=ipTerminal and codigofilial=filial
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
 jurosacumulado=valoratual-valor+vrcapitalrec,
 tipopagamento=(SELECT tipopagamento FROM caixas WHERE documento=doc LIMIT 1),
 filialpagamento=filial,
 sequencia = IF(left(sequencia,1)="0",CONCAT('s',doc,'s'),CONCAT(sequencia,doc,'s'))
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
 WHERE enderecoip=ipTerminal and codigofilial=filial;
 UPDATE contdocs set concluido='S' WHERE documento=doc;
 UPDATE crmovclientes set quitado='N' 
 WHERE codigo=codigoCliente;		
 
DELETE FROM caixas where enderecoip=ipTerminal and codigofilial=filial;
 UPDATE clientes SET ultvrpago=(select contdocs.totalbruto FROM contdocs 
 WHERE contdocs.documento=doc),ultpagamento=CURRENT_DATE,
 debito=debito-(SELECT contdocs.totalbruto FROM contdocs WHERE contdocs.documento=doc)-(SELECT contdocs.vrjuros FROM contdocs WHERE contdocs.documento=doc limit 1)
 WHERE codigo=codigoCliente;
 
  UPDATE contdocs SET 
 EADr06=MD5(CONCAT(ecffabricacao,ncupomfiscal,contadornaofiscalGNF,contadordebitocreditoCDC,DATA,coognf,tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,davnumero,DATA,total))
 WHERE documento=doc;
 
  CALL AtualizarQdtRegistros();
 
 call atualizarDebitoCliente(codigoCliente,(select fatjurdia from configfinanc where codigofilial=filial limit 1),filial);
 END */$$
DELIMITER ;

/* Procedure structure for procedure `QuitarDebitoClientePorValor` */

/*!50003 DROP PROCEDURE IF EXISTS  `QuitarDebitoClientePorValor` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `QuitarDebitoClientePorValor`(IN idCliente INT,in idNumero int,IN idRecebimento INT,IN ipTerminal VARCHAR(15),IN filial VARCHAR(5),in valorPago decimal,in _operador varchar(10))
BEGIN
declare valorRestante real default 0;
DECLARE valorAbatido REAL DEFAULT 0;
set valorRestante = valorPago;
 WHILE valorRestante>0 DO
set valorAbatido = (select valor from crmovclientes  WHERE quitado="N" AND codigo=idcliente  order by vencimento LIMIT 1);
update crmovclientes set valorpago=if(valorcorrigido<valorRestante,valorcorrigido,valorREstante), quitado="S",ip=ipTerminal where quitado="N" and codigo=idcliente ORDER BY vencimento limit 1;
SET valorRestante = valorRestante - valorAbatido;
 END WHILE;   
delete from caixas where caixas.EnderecoIP=ipTerminal;
 
INSERT INTO contdocs (ip,codigofilial,DATA,dataexe,totalbruto,desconto,encargos,total,nome,
codigocliente,NrParcelas,vendedor,operador,observacao,classe,
dpfinanceiro,vrjuros,tipopagamento,devolucaorecebimento,classedevolucao,historico,hora,devolucaonumero) 
 VALUES (ipTerminal,filial,CURRENT_DATE,CURRENT_DATE,valorPago,0,0,valorPago,(SELECT nome FROM clientes WHERE codigo=idCliente limit 1),
idCliente,NULL,'000',_operador,'Recebimento SICE.mobile',NULL,'Recebimento',0,'DH',0,' ',' ',CURRENT_TIME,'0');
insert into caixas (codigofilial,enderecoIP,nome,codigocliente,valor,dataexe,data,
dpfinanceiro,tipopagamento) VALUES (filial,ipTerminal,(SELECT nome FROM clientes 
WHERE codigo=idCliente LIMIT 1),idCliente, valorPago,current_date,current_date,'Recebimento','DH');
CALL QuitarDebitoCliente(idCliente,(SELECT MAX(documento) FROM contdocs WHERE contdocs.ip=ipTerminal),ipTerminal,filial);
 
update retornorecebimento set documento=(select max(documento) from contdocs where ip=ipTerminal),baixado="S" where inc=idRecebimento; 
update contrecebimento set contrecebimento.dataencerramento=current_date,contrecebimento.encerrado="S",operadorencerramento=_operador where contrecebimento.id=idNumero;
 END */$$
DELIMITER ;

/* Procedure structure for procedure `recriarCodigoProduto` */

/*!50003 DROP PROCEDURE IF EXISTS  `recriarCodigoProduto` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `recriarCodigoProduto`(IN filial VARCHAR(5), IN novoCodigo VARCHAR(20))
BEGIN
 
IF (filial="00001") THEN
	IF ( (SELECT COUNT(1) FROM produtos WHERE codigo=novoCodigo AND codigofilial=filial )=0) THEN
	INSERT INTO produtos(codigofilial,codigo,codigobarras,descricao,unidade,unidembalagem,grupo,subgrupo,fornecedor,icms,tributacao,custo,precovenda,pis,cofins,tributacaopis,tributacaocofins,situacao,percentualRedBaseCalcICMS,aceitadesconto,ativacompdesc,ncm,nbm,ncmespecie )
	SELECT filial,codigo,codigobarras,produto,unidade,unidade,grupo,subgrupo,fornecedor,icms,tributacao,custo,preco,pis,cofins,cstpis,cstcofins,"Excluído",percentualRedBaseCalcICMS,"S","N"," "," "," "
	FROM vendatmp WHERE codigo=novoCodigo LIMIT 1;
	END IF;
	
	IF ( (SELECT COUNT(1) FROM produtos WHERE codigo=novoCodigo AND codigofilial=filial )=0) THEN
	INSERT INTO produtos(codigofilial,codigo,codigobarras,descricao,unidade,unidembalagem,grupo,subgrupo,fornecedor,icms,tributacao,custo,precovenda,pis,cofins,tributacaopis,tributacaocofins,situacao,percentualRedBaseCalcICMS,aceitadesconto,ativacompdesc,ncm,nbm,ncmespecie )
	SELECT filial,codigo,codigobarras,produto,unidade,unidade,grupo,subgrupo,fornecedor,icms,tributacao,custo,preco,pis,cofins,cstpis,cstcofins,"Excluído",percentualRedBaseCalcICMS,"S","N"," "," "," "
	FROM vendanf WHERE codigo=novoCodigo LIMIT 1;
	END IF;
		
	IF ( (SELECT COUNT(1) FROM produtos WHERE codigo=novoCodigo AND codigofilial=filial )=0) THEN
	INSERT INTO produtos(codigofilial,codigo,codigobarras,descricao,unidade,unidembalagem,grupo,subgrupo,fornecedor,icms,tributacao,custo,precovenda,pis,cofins,tributacaopis,tributacaocofins,situacao,percentualRedBaseCalcICMS,aceitadesconto,ativacompdesc,ncm,nbm,ncmespecie )
	SELECT filial,codigo," " ,descricao,unidade,unidade,grupo,subgrupo, fornecedor,icms,tributacao,custo,precovenda,pis,cofins, cstpis,cstcofins,"Excluído",percentualRedBaseCalcICMS ,"S","N"," "," "," "
	FROM entradas WHERE codigo=novoCodigo LIMIT 1;
	END IF;
END IF;
IF (filial<>"00001") THEN
	IF ( (SELECT COUNT(1) FROM produtosfilial WHERE codigo=novoCodigo AND codigofilial=filial )=0) THEN
	INSERT INTO produtos(codigofilial,codigo,codigobarras,descricao,unidade,unidembalagem,grupo,subgrupo,fornecedor,icms,tributacao,custo,precovenda,pis,cofins,tributacaopis,tributacaocofins,situacao,percentualRedBaseCalcICMS,aceitadesconto,ativacompdesc,ncm,nbm,ncmespecie )
	SELECT filial,codigo,codigobarras,produto,unidade,unidade,grupo,subgrupo,fornecedor,icms,tributacao,custo,preco,pis,cofins,cstpis,cstcofins,"Excluído",percentualRedBaseCalcICMS,"S","N"," "," "," "
	FROM vendatmp WHERE codigo=novoCodigo LIMIT 1;
	END IF;
	
	IF ( (SELECT COUNT(1) FROM produtosfilial WHERE codigo=novoCodigo AND codigofilial=filial )=0) THEN
	INSERT INTO produtos(codigofilial,codigo,codigobarras,descricao,unidade,unidembalagem,grupo,subgrupo,fornecedor,icms,tributacao,custo,precovenda,pis,cofins,tributacaopis,tributacaocofins,situacao,percentualRedBaseCalcICMS,aceitadesconto,ativacompdesc,ncm,nbm,ncmespecie )
	SELECT filial,codigo,codigobarras,produto,unidade,unidade,grupo,subgrupo,fornecedor,icms,tributacao,custo,preco,pis,cofins,cstpis,cstcofins,"Excluído",percentualRedBaseCalcICMS,"S","N"," "," "," "
	FROM vendanf WHERE codigo=novoCodigo LIMIT 1;
	END IF;
	
	IF ( (SELECT COUNT(1) FROM produtosfilial WHERE codigo=novoCodigo AND codigofilial=filial )=0) THEN
	INSERT INTO produtos(codigofilial,codigo,codigobarras,descricao,unidade,unidembalagem,grupo,subgrupo,fornecedor,icms,tributacao,custo,precovenda,pis,cofins,tributacaopis,tributacaocofins,situacao,percentualRedBaseCalcICMS,aceitadesconto,ativacompdesc,ncm,nbm,ncmespecie )
	SELECT filial,codigo," " ,descricao,unidade,unidade,grupo,subgrupo, fornecedor,icms,tributacao,custo,precovenda,pis,cofins, cstpis,cstcofins,"Excluído",percentualRedBaseCalcICMS ,"S","N"," "," "," "
	FROM entradas WHERE codigo=novoCodigo LIMIT 1;
	END IF;
END IF;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ReservarPreVenda` */

/*!50003 DROP PROCEDURE IF EXISTS  `ReservarPreVenda` */;

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

/* Procedure structure for procedure `sp_gravaInfoGerencial` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_gravaInfoGerencial` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_gravaInfoGerencial`(IN filial VARCHAR(5))
BEGIN
 
 DECLARE mov VARCHAR(10) ;
 DECLARE v_dia, v_dia2 DECIMAL (10,2) DEFAULT 0 ; 
 DECLARE v_receber, v_recebido DECIMAL (12,2) DEFAULT 0;
 
 SET mov = (SELECT inc FROM movimento WHERE codigofilial = filial AND DATA = CURRENT_DATE AND finalizado = '');
 
 IF (mov<> "") THEN 
	
	
	SET v_dia = (SELECT  IFNULL(SUM(ABS(quantidade)*precooriginal-ratdesc),0 )AS t_vendas FROM venda	WHERE DATA = CURRENT_DATE AND cancelado = 'N' AND codigofilial = filial); 
	SET v_dia2 = (SELECT IFNULL(SUM(vendas), 0) FROM caixassoma WHERE DATA = CURRENT_DATE AND codigofilial = filial);
	
	
	 SET v_receber =(SELECT IFNULL(SUM(valor),0) AS tot_rec FROM crmovclientes WHERE vencimento = CURRENT_DATE AND codigofilial = filial );
	 SET v_recebido = (SELECT IFNULL(SUM(vrultpagamento),0) AS  tot_recebido FROM crmovclientes WHERE vencimento = CURRENT_DATE AND datapagamento = CURRENT_DATE AND codigofilial = filial);
		INSERT INTO assistentegerencial( DATA, codigofilial, hora, ocorrenciasauditoria, ticketmedio, totalpagar,percinadimplenciacrediario, vendadiaria, auditoriacliente, auditoriaacessos, auditoriaprodutos, auditoriavendas, auditoriacontaspagar, auditoriaestorno, totalreceber, totalrecebido )
		VALUES (CURRENT_DATE, filial, CURRENT_TIME, f_totalOcorrenciasAuditoria(filial, 'tot'), f_ticketMedio(filial), '0','0', v_dia+ v_dia2  , f_totalOcorrenciasAuditoria(filial, 'cli'), f_totalOcorrenciasAuditoria(filial, 'ace'), f_totalOcorrenciasAuditoria(filial, 'pro'), f_totalOcorrenciasAuditoria(filial, 'ven'),
		f_totalOcorrenciasAuditoria(filial, 'pag'), f_totalOcorrenciasAuditoria(filial, 'est') , v_receber, v_recebido);
END IF;	
 
    END */$$
DELIMITER ;

/*Table structure for table `60d` */

DROP TABLE IF EXISTS `60d`;

/*!50001 DROP VIEW IF EXISTS `60d` */;
/*!50001 DROP TABLE IF EXISTS `60d` */;

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
 `preco` decimal(12,5) ,
 `totalicms` decimal(38,2) ,
 `descontovalor` decimal(32,2) ,
 `descontovalorCupom` decimal(32,4) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `acrescimototalitem` decimal(30,2) ,
 `baseCalculoICMS` decimal(37,2) 
)*/;

/*Table structure for table `60i` */

DROP TABLE IF EXISTS `60i`;

/*!50001 DROP VIEW IF EXISTS `60i` */;
/*!50001 DROP TABLE IF EXISTS `60i` */;

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
 `preco` decimal(12,5) ,
 `cancelado` char(1) ,
 `totalicms` decimal(38,2) ,
 `descontovalor` decimal(32,2) ,
 `descontovalorCupom` decimal(32,4) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `baseCalculoICMS` decimal(37,2) 
)*/;

/*Table structure for table `60r` */

DROP TABLE IF EXISTS `60r`;

/*!50001 DROP VIEW IF EXISTS `60r` */;
/*!50001 DROP TABLE IF EXISTS `60r` */;

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
 `preco` decimal(12,5) ,
 `totalicms` decimal(38,2) ,
 `descontovalor` decimal(32,2) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `baseCalculoICMS` decimal(37,2) 
)*/;

/*Table structure for table `apuracaofiscal` */

DROP TABLE IF EXISTS `apuracaofiscal`;

/*!50001 DROP VIEW IF EXISTS `apuracaofiscal` */;
/*!50001 DROP TABLE IF EXISTS `apuracaofiscal` */;

/*!50001 CREATE TABLE  `apuracaofiscal`(
 `filial` varchar(5) ,
 `cfop` varchar(5) ,
 `modeloDOC` varchar(2) ,
 `CSTICMS` char(3) ,
 `totalproduto` decimal(56,2) ,
 `total` decimal(65,2) ,
 `bcICMS` decimal(63,2) ,
 `totICMS` decimal(64,2) ,
 `baseCalculoIPI` decimal(59,2) ,
 `totalIPI` decimal(62,2) ,
 `bcPIS` decimal(62,2) ,
 `bcCOFINS` decimal(62,2) ,
 `totalPIS` decimal(65,2) ,
 `totalCOFINS` decimal(65,2) ,
 `bcICMSST` decimal(60,2) ,
 `totalICMSST` decimal(64,2) ,
 `totalOutrasDespesas` decimal(54,2) ,
 `totalIsentas` decimal(54,2) 
)*/;

/*Table structure for table `blococregc190` */

DROP TABLE IF EXISTS `blococregc190`;

/*!50001 DROP VIEW IF EXISTS `blococregc190` */;
/*!50001 DROP TABLE IF EXISTS `blococregc190` */;

/*!50001 CREATE TABLE  `blococregc190`(
 `codigofilial` varchar(5) ,
 `numero` int(6) unsigned ,
 `nf` varchar(15) ,
 `modelonf` varchar(2) ,
 `dataentrada` date ,
 `cfopentrada` varchar(5) ,
 `icmsentrada` decimal(8,2) ,
 `icmsst` decimal(8,2) ,
 `tributacao` char(3) ,
 `percentualRedBaseCalcICMS` decimal(5,2) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `sequencia` int(4) ,
 `quantidade` decimal(32,2) ,
 `unidade` char(3) ,
 `custo` decimal(12,5) ,
 `desconto` decimal(32,4) ,
 `totalDespesas` decimal(32,4) ,
 `totalSeguro` decimal(32,4) ,
 `totalFrete` decimal(32,4) ,
 `valoroutrasdespesas` decimal(32,2) ,
 `bcicms` decimal(32,2) ,
 `valorisentas` decimal(32,2) ,
 `toticms` decimal(39,2) ,
 `bcicmsST` decimal(32,2) ,
 `valoricmsST` decimal(30,2) ,
 `ipiItem` decimal(39,2) ,
 `totalPIS` decimal(45,2) ,
 `totalCOFINS` decimal(45,2) ,
 `totalProduto` decimal(32,2) ,
 `totalNF` decimal(45,2) ,
 `valoroutrasaliquotas` decimal(43,2) ,
 `totalReducaoICMS` decimal(36,2) ,
 `baseCalculoIPI` decimal(32,2) ,
 `baseCalculoPIS` decimal(40,2) ,
 `baseCalculoCOFINS` decimal(40,2) ,
 `lancada` char(1) 
)*/;

/*Table structure for table `blococregc190_saida` */

DROP TABLE IF EXISTS `blococregc190_saida`;

/*!50001 DROP VIEW IF EXISTS `blococregc190_saida` */;
/*!50001 DROP TABLE IF EXISTS `blococregc190_saida` */;

/*!50001 CREATE TABLE  `blococregc190_saida`(
 `inc` int(8) ,
 `codigofilial` varchar(5) ,
 `notafiscal` varchar(15) ,
 `serienf` char(3) ,
 `modelodocfiscal` char(2) ,
 `documento` int(10) unsigned ,
 `DATA` date ,
 `cfop` varchar(5) ,
 `icms` int(2) ,
 `ipi` decimal(5,2) ,
 `pis` decimal(5,3) ,
 `cofins` decimal(5,3) ,
 `cstpis` char(2) ,
 `cstcofins` char(2) ,
 `cstipi` varchar(2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `SUM(quantidade)` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `origem` char(1) ,
 `totalicms` decimal(42,2) ,
 `totalIPI` decimal(40,2) ,
 `totalPIS` decimal(37,2) ,
 `totalCOFINS` decimal(37,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `descontovalor` decimal(35,4) ,
 `SUM(TOTAL)` decimal(36,4) ,
 `totalItem` decimal(32,2) ,
 `baseCalculoICMS` decimal(41,2) ,
 `totalReducaoICMS` decimal(36,2) ,
 `baseCalculoIPI` decimal(37,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) ,
 `bcICMSST` decimal(38,2) ,
 `totalICMSST` decimal(42,2) 
)*/;

/*Table structure for table `blococregc300` */

DROP TABLE IF EXISTS `blococregc300`;

/*!50001 DROP VIEW IF EXISTS `blococregc300` */;
/*!50001 DROP TABLE IF EXISTS `blococregc300` */;

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

/*!50001 DROP VIEW IF EXISTS `blococregc320` */;
/*!50001 DROP TABLE IF EXISTS `blococregc320` */;

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
 `totalICMS` decimal(15,6) 
)*/;

/*Table structure for table `blococregc321` */

DROP TABLE IF EXISTS `blococregc321`;

/*!50001 DROP VIEW IF EXISTS `blococregc321` */;
/*!50001 DROP TABLE IF EXISTS `blococregc321` */;

/*!50001 CREATE TABLE  `blococregc321`(
 `inc` int(11) ,
 `codigofilial` varchar(5) ,
 `operador` varchar(10) ,
 `data` date ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `quantidade` decimal(10,5) ,
 `preco` decimal(12,5) ,
 `custo` decimal(12,5) ,
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
 `customedio` decimal(12,5) ,
 `Ecfnumero` char(3) ,
 `fornecedor` varchar(50) ,
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
 `ipi` decimal(5,2) ,
 `unidade` char(3) ,
 `embalagem` int(11) ,
 `grade` varchar(20) ,
 `romaneio` char(1) ,
 `tipo` varchar(15) ,
 `cofins` decimal(5,3) ,
 `pis` decimal(5,3) ,
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
 `mvast` decimal(7,3) ,
 `modelodocfiscal` varchar(2) ,
 `somaQuantidade` decimal(32,5) ,
 `totalItem` decimal(35,4) ,
 `totalDesconto` decimal(32,4) ,
 `bcICMS` decimal(34,2) ,
 `baseCalculoICMS` decimal(34,2) ,
 `totalICMS` decimal(35,2) ,
 `totalPIS` decimal(27,3) ,
 `totalCOFINS` decimal(27,3) 
)*/;

/*Table structure for table `blococregc381_pis` */

DROP TABLE IF EXISTS `blococregc381_pis`;

/*!50001 DROP VIEW IF EXISTS `blococregc381_pis` */;
/*!50001 DROP TABLE IF EXISTS `blococregc381_pis` */;

/*!50001 CREATE TABLE  `blococregc381_pis`(
 `documento` int(10) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `codigofilial` varchar(5) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(36,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `pis` decimal(5,3) ,
 `cofins` decimal(5,3) ,
 `cstpis` char(2) ,
 `cstcofins` char(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `baseCalculoICMS` decimal(34,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) 
)*/;

/*Table structure for table `blococregc385_cofins` */

DROP TABLE IF EXISTS `blococregc385_cofins`;

/*!50001 DROP VIEW IF EXISTS `blococregc385_cofins` */;
/*!50001 DROP TABLE IF EXISTS `blococregc385_cofins` */;

/*!50001 CREATE TABLE  `blococregc385_cofins`(
 `documento` int(10) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `codigofilial` varchar(5) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(36,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `pis` decimal(5,3) ,
 `cofins` decimal(5,3) ,
 `cstpis` char(2) ,
 `cstcofins` char(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `baseCalculoICMS` decimal(34,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) 
)*/;

/*Table structure for table `blococregc390` */

DROP TABLE IF EXISTS `blococregc390`;

/*!50001 DROP VIEW IF EXISTS `blococregc390` */;
/*!50001 DROP TABLE IF EXISTS `blococregc390` */;

/*!50001 CREATE TABLE  `blococregc390`(
 `documento` int(11) unsigned ,
 `data` date ,
 `icms` int(11) ,
 `cfop` varchar(5) ,
 `tributacao` char(3) ,
 `total` decimal(33,2) ,
 `baseCalculoICMS` decimal(33,2) ,
 `totalICMS` decimal(33,2) 
)*/;

/*Table structure for table `blococregc400` */

DROP TABLE IF EXISTS `blococregc400`;

/*!50001 DROP VIEW IF EXISTS `blococregc400` */;
/*!50001 DROP TABLE IF EXISTS `blococregc400` */;

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

/*!50001 DROP VIEW IF EXISTS `blococregc425` */;
/*!50001 DROP TABLE IF EXISTS `blococregc425` */;

/*!50001 CREATE TABLE  `blococregc425`(
 `documento` int(10) ,
 `nrnotafiscal` bigint(12) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `ecfCPFCNPJconsumidor` varchar(14) ,
 `nrcontrole` int(5) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(8,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `baseCalculoICMS` decimal(34,2) ,
 `totalICMS` decimal(35,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `totalIPI` decimal(37,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `acrescimototalitem` decimal(30,2) ,
 `valorisentas` decimal(36,4) ,
 `valorNT` decimal(36,4) ,
 `valorST` decimal(36,4) 
)*/;

/*Table structure for table `blococregc470` */

DROP TABLE IF EXISTS `blococregc470`;

/*!50001 DROP VIEW IF EXISTS `blococregc470` */;
/*!50001 DROP TABLE IF EXISTS `blococregc470` */;

/*!50001 CREATE TABLE  `blococregc470`(
 `documento` int(10) ,
 `nrnotafiscal` bigint(12) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `ecfCPFCNPJconsumidor` varchar(14) ,
 `nrcontrole` int(5) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(8,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(12,2) ,
 `baseCalculoICMS` decimal(34,2) ,
 `totalICMS` decimal(35,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `totalIPI` decimal(37,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `acrescimototalitem` decimal(30,2) ,
 `valorisentas` decimal(36,4) ,
 `valorNT` decimal(36,4) ,
 `valorST` decimal(36,4) 
)*/;

/*Table structure for table `blococregc470_02_itens` */

DROP TABLE IF EXISTS `blococregc470_02_itens`;

/*!50001 DROP VIEW IF EXISTS `blococregc470_02_itens` */;
/*!50001 DROP TABLE IF EXISTS `blococregc470_02_itens` */;

/*!50001 CREATE TABLE  `blococregc470_02_itens`(
 `documento` int(10) ,
 `nrnotafiscal` bigint(12) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `ecfCPFCNPJconsumidor` varchar(14) ,
 `nrcontrole` int(5) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(8,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(12,2) ,
 `baseCalculoICMS` decimal(34,2) ,
 `totalICMS` decimal(35,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `totalIPI` decimal(37,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `acrescimototalitem` decimal(30,2) ,
 `valorisentas` decimal(36,4) ,
 `valorNT` decimal(36,4) ,
 `valorST` decimal(36,4) 
)*/;

/*Table structure for table `blococregc481_pis` */

DROP TABLE IF EXISTS `blococregc481_pis`;

/*!50001 DROP VIEW IF EXISTS `blococregc481_pis` */;
/*!50001 DROP TABLE IF EXISTS `blococregc481_pis` */;

/*!50001 CREATE TABLE  `blococregc481_pis`(
 `documento` int(10) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `codigofilial` varchar(5) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(36,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `pis` decimal(5,3) ,
 `cofins` decimal(5,3) ,
 `cstpis` char(2) ,
 `cstcofins` char(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `baseCalculoICMS` decimal(34,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) 
)*/;

/*Table structure for table `blococregc485_cofins` */

DROP TABLE IF EXISTS `blococregc485_cofins`;

/*!50001 DROP VIEW IF EXISTS `blococregc485_cofins` */;
/*!50001 DROP TABLE IF EXISTS `blococregc485_cofins` */;

/*!50001 CREATE TABLE  `blococregc485_cofins`(
 `documento` int(10) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `codigofilial` varchar(5) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(36,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `pis` decimal(5,3) ,
 `cofins` decimal(5,3) ,
 `cstpis` char(2) ,
 `cstcofins` char(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `baseCalculoICMS` decimal(34,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `baseCalculoPIS` decimal(32,2) ,
 `baseCalculoCOFINS` decimal(32,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) 
)*/;

/*Table structure for table `blococregc490` */

DROP TABLE IF EXISTS `blococregc490`;

/*!50001 DROP VIEW IF EXISTS `blococregc490` */;
/*!50001 DROP TABLE IF EXISTS `blococregc490` */;

/*!50001 CREATE TABLE  `blococregc490`(
 `codigofilial` varchar(5) ,
 `documento` int(10) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `nrcontrole` int(5) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(8,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(32,2) ,
 `ratdesc` decimal(32,4) ,
 `rateioencargos` decimal(32,4) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `baseCalculoICMS` decimal(34,2) ,
 `totalICMS` decimal(35,2) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `acrescimototalitem` decimal(30,2) ,
 `valorisentas` decimal(36,4) ,
 `valorNT` decimal(36,4) ,
 `valorST` decimal(36,4) 
)*/;

/*Table structure for table `blococregc491_pis` */

DROP TABLE IF EXISTS `blococregc491_pis`;

/*!50001 DROP VIEW IF EXISTS `blococregc491_pis` */;
/*!50001 DROP TABLE IF EXISTS `blococregc491_pis` */;

/*!50001 CREATE TABLE  `blococregc491_pis`(
 `documento` int(10) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `codigofilial` varchar(5) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(36,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `pis` decimal(5,3) ,
 `cofins` decimal(5,3) ,
 `cstpis` char(2) ,
 `cstcofins` char(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `baseCalculoICMS` decimal(34,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `baseCalculoPIS` decimal(32,2) ,
 `baseCalculoCOFINS` decimal(32,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) 
)*/;

/*Table structure for table `blococregc495_cofins` */

DROP TABLE IF EXISTS `blococregc495_cofins`;

/*!50001 DROP VIEW IF EXISTS `blococregc495_cofins` */;
/*!50001 DROP TABLE IF EXISTS `blococregc495_cofins` */;

/*!50001 CREATE TABLE  `blococregc495_cofins`(
 `documento` int(10) ,
 `data` date ,
 `ecffabricacao` varchar(20) ,
 `ecfnumero` char(3) ,
 `ncupomfiscal` varchar(10) ,
 `modelodocfiscal` varchar(2) ,
 `codigofilial` varchar(5) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(36,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `pis` decimal(5,3) ,
 `cofins` decimal(5,3) ,
 `cstpis` char(2) ,
 `cstcofins` char(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `baseCalculoICMS` decimal(34,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) 
)*/;

/*Table structure for table `endereco_completo` */

DROP TABLE IF EXISTS `endereco_completo`;

/*!50001 DROP VIEW IF EXISTS `endereco_completo` */;
/*!50001 DROP TABLE IF EXISTS `endereco_completo` */;

/*!50001 CREATE TABLE  `endereco_completo`(
 `logradouro` varchar(300) ,
 `endereco` varchar(300) ,
 `bairro` varchar(200) ,
 `cidade` varchar(200) ,
 `uf` char(2) ,
 `cep` varchar(9) 
)*/;

/*Table structure for table `r05` */

DROP TABLE IF EXISTS `r05`;

/*!50001 DROP VIEW IF EXISTS `r05` */;
/*!50001 DROP TABLE IF EXISTS `r05` */;

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
 `preco` decimal(12,5) ,
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

/*!50001 DROP VIEW IF EXISTS `registro50entradas_agr` */;
/*!50001 DROP TABLE IF EXISTS `registro50entradas_agr` */;

/*!50001 CREATE TABLE  `registro50entradas_agr`(
 `UFemitente` varchar(2) ,
 `codigofilial` varchar(5) ,
 `numero` int(6) unsigned ,
 `nf` varchar(15) ,
 `modelonf` varchar(2) ,
 `dataentrada` date ,
 `cfopentrada` varchar(5) ,
 `icmsentrada` decimal(8,2) ,
 `icmsst` decimal(8,2) ,
 `tributacao` char(3) ,
 `percentualRedBaseCalcICMS` decimal(5,2) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `sequencia` int(4) ,
 `quantidade` decimal(32,2) ,
 `unidade` char(3) ,
 `custo` decimal(12,5) ,
 `desconto` decimal(32,4) ,
 `totalDespesas` decimal(32,4) ,
 `totalSeguro` decimal(32,4) ,
 `totalFrete` decimal(32,4) ,
 `valoroutrasdespesas` decimal(32,2) ,
 `bcicms` decimal(32,2) ,
 `valorisentas` decimal(32,2) ,
 `toticms` decimal(39,2) ,
 `bcicmsST` decimal(32,2) ,
 `valoricmsST` decimal(30,2) ,
 `ipiItem` decimal(39,2) ,
 `totPIS` decimal(45,2) ,
 `totCOFINS` decimal(45,2) ,
 `totalProduto` decimal(32,2) ,
 `totalNF` decimal(45,2) ,
 `valoroutrasaliquotas` decimal(44,2) ,
 `totalReducaoICMS` decimal(36,2) ,
 `baseCalculoIPI` decimal(32,2) ,
 `baseCalculoPIS` decimal(40,2) ,
 `baseCalculoCOFINS` decimal(40,2) ,
 `lancada` char(1) 
)*/;

/*Table structure for table `registro50entradas_itens` */

DROP TABLE IF EXISTS `registro50entradas_itens`;

/*!50001 DROP VIEW IF EXISTS `registro50entradas_itens` */;
/*!50001 DROP TABLE IF EXISTS `registro50entradas_itens` */;

/*!50001 CREATE TABLE  `registro50entradas_itens`(
 `UFemitente` varchar(2) ,
 `codigofilial` varchar(5) ,
 `numero` int(6) unsigned ,
 `nf` varchar(15) ,
 `modelonf` varchar(2) ,
 `dataentrada` date ,
 `cfopentrada` varchar(5) ,
 `icmsentrada` decimal(8,2) ,
 `icmsst` decimal(8,2) ,
 `ipi` decimal(8,2) ,
 `pis` decimal(8,3) ,
 `cofins` decimal(8,3) ,
 `cstpis` char(2) ,
 `cstcofins` char(2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `sequencia` int(4) ,
 `quantidade` decimal(32,2) ,
 `unidade` char(3) ,
 `custo` decimal(12,5) ,
 `desconto` decimal(32,4) ,
 `totalDespesas` decimal(32,4) ,
 `totalSeguro` decimal(32,4) ,
 `totalFrete` decimal(32,4) ,
 `valoroutrasdespesas` decimal(32,2) ,
 `bcicms` decimal(32,2) ,
 `valorisentas` decimal(32,2) ,
 `toticms` decimal(39,2) ,
 `bcicmsST` decimal(32,2) ,
 `valoricmsST` decimal(30,2) ,
 `ipiItem` decimal(39,2) ,
 `totPIS` decimal(45,2) ,
 `totCOFINS` decimal(45,2) ,
 `totalProduto` decimal(32,2) ,
 `totalNF` decimal(45,2) ,
 `valoroutrasaliquotas` decimal(44,2) ,
 `baseCalculoIPI` decimal(32,2) ,
 `baseCalculoPIS` decimal(40,2) ,
 `baseCalculoCOFINS` decimal(40,2) ,
 `lancada` char(1) 
)*/;

/*Table structure for table `registro50saida_agr` */

DROP TABLE IF EXISTS `registro50saida_agr`;

/*!50001 DROP VIEW IF EXISTS `registro50saida_agr` */;
/*!50001 DROP TABLE IF EXISTS `registro50saida_agr` */;

/*!50001 CREATE TABLE  `registro50saida_agr`(
 `inc` int(8) ,
 `codigofilial` varchar(5) ,
 `notafiscal` varchar(15) ,
 `serienf` char(3) ,
 `modelodocfiscal` char(2) ,
 `documento` int(10) unsigned ,
 `DATA` date ,
 `cfop` varchar(5) ,
 `icms` int(2) ,
 `icmsst` decimal(5,2) ,
 `ipi` decimal(5,2) ,
 `pis` decimal(5,3) ,
 `cofins` decimal(5,3) ,
 `cstpis` char(2) ,
 `cstcofins` char(2) ,
 `cstipi` varchar(2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `SUM(quantidade)` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `origem` char(1) ,
 `totalicms` decimal(42,2) ,
 `totalIPI` decimal(40,2) ,
 `totalPIS` decimal(37,2) ,
 `totalCOFINS` decimal(37,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `descontovalor` decimal(35,4) ,
 `valorisentas` decimal(36,4) ,
 `totalbruto` decimal(32,2) ,
 `SUM(TOTAL)` decimal(35,2) ,
 `totalItem` decimal(32,2) ,
 `baseCalculoICMS` decimal(41,2) ,
 `baseCalculoIPI` decimal(37,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) ,
 `bcICMSST` decimal(38,2) ,
 `totalICMSST` decimal(42,2) 
)*/;

/*Table structure for table `registro50saidas_itens` */

DROP TABLE IF EXISTS `registro50saidas_itens`;

/*!50001 DROP VIEW IF EXISTS `registro50saidas_itens` */;
/*!50001 DROP TABLE IF EXISTS `registro50saidas_itens` */;

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
 `icmsst` decimal(5,2) ,
 `ipi` decimal(5,2) ,
 `pis` decimal(5,3) ,
 `cofins` decimal(5,3) ,
 `cstpis` char(2) ,
 `cstcofins` char(2) ,
 `cstipi` varchar(2) ,
 `tributacao` char(3) ,
 `codigo` varchar(20) ,
 `produto` varchar(50) ,
 `SUM(quantidade)` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `origem` char(1) ,
 `totalicms` decimal(42,2) ,
 `totalIPI` decimal(40,2) ,
 `totalPIS` decimal(37,2) ,
 `totalCOFINS` decimal(37,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `descontovalor` decimal(35,4) ,
 `valorisentas` decimal(36,4) ,
 `SUM(TOTAL)` decimal(35,2) ,
 `totalbruto` decimal(32,2) ,
 `baseCalculoICMS` decimal(41,2) ,
 `baseCalculoIPI` decimal(37,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) ,
 `bcICMSST` decimal(38,2) ,
 `totalICMSST` decimal(42,2) 
)*/;

/*Table structure for table `v_assistentegerencial` */

DROP TABLE IF EXISTS `v_assistentegerencial`;

/*!50001 DROP VIEW IF EXISTS `v_assistentegerencial` */;
/*!50001 DROP TABLE IF EXISTS `v_assistentegerencial` */;

/*!50001 CREATE TABLE  `v_assistentegerencial`(
 `codigofilial` varchar(5) ,
 `ticketmedio` decimal(10,2) ,
 `data` date ,
 `hora` time ,
 `ocorrenciasauditoria` int(11) ,
 `auditoriacliente` int(11) ,
 `auditoriaacessos` int(11) ,
 `auditoriaprodutos` int(11) ,
 `auditoriavendas` int(11) ,
 `auditoriacontaspagar` int(11) ,
 `auditoriaestorno` int(11) ,
 `vendadiaria` decimal(10,2) ,
 `totalreceber` decimal(12,2) ,
 `totalrecebido` decimal(12,2) 
)*/;

/*Table structure for table `valoresinventario` */

DROP TABLE IF EXISTS `valoresinventario`;

/*!50001 DROP VIEW IF EXISTS `valoresinventario` */;
/*!50001 DROP TABLE IF EXISTS `valoresinventario` */;

/*!50001 CREATE TABLE  `valoresinventario`(
 `codigofilial` varchar(5) ,
 `tipo` varchar(15) ,
 `grupo` varchar(30) ,
 `custo` decimal(40,2) ,
 `customedio` decimal(40,2) ,
 `custofornecedor` decimal(40,2) ,
 `total` decimal(43,2) ,
 `custoretidos` decimal(37,2) ,
 `custovencidos` decimal(39,2) ,
 `quantidade` decimal(32,2) ,
 `icmsRecuperar` decimal(44,2) ,
 `ipiRecuperar` decimal(43,2) ,
 `pisRecuperar` decimal(42,2) ,
 `cofinsRecuperar` decimal(42,2) 
)*/;

/*View structure for view 60d */

/*!50001 DROP TABLE IF EXISTS `60d` */;
/*!50001 DROP VIEW IF EXISTS `60d` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `60d` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`ratdesc`) AS `descontovalorCupom`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` where (`vendatmp`.`quantidade` > 0) group by `vendatmp`.`data`,`vendatmp`.`codigo` */;

/*View structure for view 60i */

/*!50001 DROP TABLE IF EXISTS `60i` */;
/*!50001 DROP VIEW IF EXISTS `60i` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `60i` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`cancelado` AS `cancelado`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`ratdesc`) AS `descontovalorCupom`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` group by `vendatmp`.`codigo` */;

/*View structure for view 60r */

/*!50001 DROP TABLE IF EXISTS `60r` */;
/*!50001 DROP VIEW IF EXISTS `60r` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `60r` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` where (`vendatmp`.`quantidade` > 0) group by month(`vendatmp`.`data`),`vendatmp`.`icms`,`vendatmp`.`codigo` */;

/*View structure for view apuracaofiscal */

/*!50001 DROP TABLE IF EXISTS `apuracaofiscal` */;
/*!50001 DROP VIEW IF EXISTS `apuracaofiscal` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `apuracaofiscal` AS select `blococregc190`.`codigofilial` AS `filial`,`blococregc190`.`cfopentrada` AS `cfop`,`blococregc190`.`modelonf` AS `modeloDOC`,`blococregc190`.`tributacao` AS `CSTICMS`,sum(`blococregc190`.`totalProduto`) AS `totalproduto`,sum(`blococregc190`.`totalNF`) AS `total`,sum(`blococregc190`.`bcicms`) AS `bcICMS`,sum(`blococregc190`.`toticms`) AS `totICMS`,sum(`blococregc190`.`baseCalculoIPI`) AS `baseCalculoIPI`,sum(`blococregc190`.`ipiItem`) AS `totalIPI`,sum(`blococregc190`.`baseCalculoPIS`) AS `bcPIS`,sum(`blococregc190`.`baseCalculoCOFINS`) AS `bcCOFINS`,sum(`blococregc190`.`totalPIS`) AS `totalPIS`,sum(`blococregc190`.`totalCOFINS`) AS `totalCOFINS`,sum(`blococregc190`.`bcicmsST`) AS `bcICMSST`,sum(`blococregc190`.`valoricmsST`) AS `totalICMSST`,sum(`blococregc190`.`valoroutrasdespesas`) AS `totalOutrasDespesas`,sum(`blococregc190`.`valorisentas`) AS `totalIsentas` from `blococregc190` where ((`blococregc190`.`lancada` = 'X') and (`blococregc190`.`dataentrada` >= '2012-01-01') and (`blococregc190`.`dataentrada` <= '2012-03-31')) group by `blococregc190`.`cfopentrada`,`blococregc190`.`codigofilial` union all select `blococregc190_saida`.`codigofilial` AS `filial`,`blococregc190_saida`.`cfop` AS `cfop`,`blococregc190_saida`.`modelodocfiscal` AS `modeloDOC`,`blococregc190_saida`.`tributacao` AS `cstICMS`,sum(`blococregc190_saida`.`totalItem`) AS `totalProduto`,sum(`blococregc190_saida`.`totalItem`) AS `total`,sum(`blococregc190_saida`.`baseCalculoICMS`) AS `bcICMS`,sum(`blococregc190_saida`.`totalicms`) AS `totICMS`,sum(`blococregc190_saida`.`baseCalculoIPI`) AS `baseCalculoIPI`,sum(`blococregc190_saida`.`totalIPI`) AS `totalIPI`,sum(`blococregc190_saida`.`baseCalculoPIS`) AS `bcPIS`,sum(`blococregc190_saida`.`baseCalculoCOFINS`) AS `bcCOFINS`,sum(`blococregc190_saida`.`totalPIS`) AS `totalPIS`,sum(`blococregc190_saida`.`totalCOFINS`) AS `totalCOFINS`,sum(`blococregc190_saida`.`bcICMSST`) AS `bcICMSST`,sum(`blococregc190_saida`.`totalICMSST`) AS `totalICMSST`,sum(0) AS `totalOutrasDespesas`,sum(0) AS `totalIsentas` from `blococregc190_saida` where ((`blococregc190_saida`.`DATA` >= '2012-01-01') and (`blococregc190_saida`.`DATA` <= '2012-03-31')) group by `blococregc190_saida`.`cfop`,`blococregc190_saida`.`codigofilial` union all select `blococregc381_pis`.`codigofilial` AS `filial`,`blococregc381_pis`.`cfop` AS `cfop`,`blococregc381_pis`.`modelodocfiscal` AS `modeloDOC`,`blococregc381_pis`.`tributacao` AS `cstICMS`,sum(`blococregc381_pis`.`total`) AS `totalProduto`,sum(`blococregc381_pis`.`total`) AS `total`,sum(`blococregc381_pis`.`baseCalculoICMS`) AS `bcICMS`,sum(`blococregc381_pis`.`totalicms`) AS `totICMS`,sum(0) AS `baceCalculoIPI`,sum(0) AS `totalIPI`,sum(`blococregc381_pis`.`baseCalculoPIS`) AS `bcPIS`,sum(`blococregc381_pis`.`baseCalculoCOFINS`) AS `bcCOFINS`,sum(`blococregc381_pis`.`totalPIS`) AS `totalPIS`,sum(`blococregc381_pis`.`totalCOFINS`) AS `totalCOFINS`,sum(0) AS `bcICMSST`,sum(0) AS `totalICMSST`,sum(0) AS `totalOutrasDespesas`,sum(0) AS `totalIsentas` from `blococregc381_pis` where ((`blococregc381_pis`.`data` >= '2012-01-01') and (`blococregc381_pis`.`data` <= '2012-03-31')) group by `blococregc381_pis`.`cfop`,`blococregc381_pis`.`codigofilial` union all select `blococregc491_pis`.`codigofilial` AS `filial`,`blococregc491_pis`.`cfop` AS `cfop`,`blococregc491_pis`.`modelodocfiscal` AS `modeloDOC`,`blococregc491_pis`.`tributacao` AS `cstICMS`,sum(`blococregc491_pis`.`total`) AS `totalProduto`,sum(`blococregc491_pis`.`total`) AS `total`,sum(`blococregc491_pis`.`baseCalculoICMS`) AS `bcICMS`,sum(`blococregc491_pis`.`totalicms`) AS `totICMS`,sum(0) AS `baceCalculoIPI`,sum(0) AS `totalIPI`,sum(`blococregc491_pis`.`baseCalculoPIS`) AS `bcPIS`,sum(`blococregc491_pis`.`baseCalculoCOFINS`) AS `bcCOFINS`,sum(`blococregc491_pis`.`totalPIS`) AS `totalPIS`,sum(`blococregc491_pis`.`totalCOFINS`) AS `totalCOFINS`,sum(0) AS `bcICMSST`,sum(0) AS `totalICMSST`,sum(0) AS `totalOutrasDespesas`,sum(0) AS `totalIsentas` from `blococregc491_pis` where ((`blococregc491_pis`.`data` >= '2012-01-01') and (`blococregc491_pis`.`data` <= '2012-03-31')) group by `blococregc491_pis`.`cfop`,`blococregc491_pis`.`codigofilial` */;

/*View structure for view blococregc190 */

/*!50001 DROP TABLE IF EXISTS `blococregc190` */;
/*!50001 DROP VIEW IF EXISTS `blococregc190` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc190` AS select `entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`icmsst` AS `icmsst`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`ratdesconto`) AS `desconto`,sum(`entradas`.`ratdespesas`) AS `totalDespesas`,sum(`entradas`.`ratseguro`) AS `totalSeguro`,sum(`entradas`.`ratfrete`) AS `totalFrete`,sum(`entradas`.`valoroutrasdespesas`) AS `valoroutrasdespesas`,sum(`entradas`.`bcicms`) AS `bcicms`,sum(if((`entradas`.`IcmsEntrada` = 0),`entradas`.`totalitem`,0)) AS `valorisentas`,round(if((`entradas`.`IcmsEntrada` > 0),sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),0),2) AS `toticms`,sum(`entradas`.`bcicmsST`) AS `bcicmsST`,sum(`entradas`.`valoricmsST`) AS `valoricmsST`,sum(round((`entradas`.`totalitem` * if((`entradas`.`IPI` > 0),(`entradas`.`IPI` / 100),0)),2)) AS `ipiItem`,round(if((`entradas`.`pis` > 0),sum(((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)) * (`entradas`.`pis` / 100))),0),2) AS `totalPIS`,round(if((`entradas`.`cofins` > 0),sum(((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)) * (`entradas`.`cofins` / 100))),0),2) AS `totalCOFINS`,sum(`entradas`.`totalitem`) AS `totalProduto`,((((((sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) + sum(`entradas`.`totalitem`)) - round(sum(`entradas`.`ratdesconto`),2)) AS `totalNF`,((((sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) AS `valoroutrasaliquotas`,sum(round((`entradas`.`totalitem` * (`entradas`.`percentualRedBaseCalcICMS` / 100)),2)) AS `totalReducaoICMS`,sum(if((`entradas`.`IPI` > 0),`entradas`.`totalitem`,0)) AS `baseCalculoIPI`,if((`entradas`.`pis` > 0),sum(round((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)),2)),0) AS `baseCalculoPIS`,if((`entradas`.`cofins` > 0),sum(round((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)),2)),0) AS `baseCalculoCOFINS`,`entradas`.`Lancada` AS `lancada` from `entradas` where (`entradas`.`exportarfiscal` = 'S') group by `entradas`.`NF`,`entradas`.`cfopentrada`,`entradas`.`IcmsEntrada`,`entradas`.`tributacao`,`entradas`.`numero` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view blococregc190_saida */

/*!50001 DROP TABLE IF EXISTS `blococregc190_saida` */;
/*!50001 DROP VIEW IF EXISTS `blococregc190_saida` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc190_saida` AS select `vendanftmp`.`inc` AS `inc`,`vendanftmp`.`codigofilial` AS `codigofilial`,`vendanftmp`.`NotaFiscal` AS `notafiscal`,`vendanftmp`.`serieNF` AS `serienf`,`vendanftmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendanftmp`.`documento` AS `documento`,`vendanftmp`.`data` AS `DATA`,`vendanftmp`.`cfop` AS `cfop`,`vendanftmp`.`icms` AS `icms`,ifnull(`vendanftmp`.`aliquotaIPI`,0) AS `ipi`,`vendanftmp`.`pis` AS `pis`,`vendanftmp`.`cofins` AS `cofins`,`vendanftmp`.`cstpis` AS `cstpis`,`vendanftmp`.`cstcofins` AS `cstcofins`,`vendanftmp`.`cstipi` AS `cstipi`,`vendanftmp`.`tributacao` AS `tributacao`,`vendanftmp`.`codigo` AS `codigo`,`vendanftmp`.`produto` AS `produto`,sum(`vendanftmp`.`quantidade`) AS `SUM(quantidade)`,`vendanftmp`.`unidade` AS `unidade`,`vendanftmp`.`nrcontrole` AS `nrcontrole`,`vendanftmp`.`origem` AS `origem`,sum(round((((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - ((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`icms`) / 100),2)) AS `totalicms`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((((`vendanftmp`.`total` - ((`vendanftmp`.`total` * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`aliquotaIPI`) / 100),2)),0) AS `totalIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`pis`) / 100),2)),0) AS `totalPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`cofins`) / 100),2)),0) AS `totalCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`cofins`)),0) AS `totalCOFINS_QTD`,sum((`vendanftmp`.`descontovalor` + `vendanftmp`.`ratdesc`)) AS `descontovalor`,sum(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`)) AS `SUM(TOTAL)`,sum(`vendanftmp`.`total`) AS `totalItem`,if((`vendanftmp`.`icms` > 0),sum(round((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - (((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS`,sum(round((`vendanftmp`.`total` * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100)),2)) AS `totalReducaoICMS`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((`vendanftmp`.`total` - (`vendanftmp`.`total` * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD`,if((`vendanftmp`.`icmsst` > 0),sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)),0) AS `bcICMSST`,truncate(if(((`vendanftmp`.`icmsst` > 0) and (`vendanftmp`.`icmsst` >= `vendanftmp`.`icms`)),(((sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)) * `vendanftmp`.`icmsst`) / 100) - sum(round(((`vendanftmp`.`total` * `vendanftmp`.`icms`) / 100),2))),0),2) AS `totalICMSST` from `vendanftmp` where (`vendanftmp`.`quantidade` > 0) group by `vendanftmp`.`NotaFiscal`,`vendanftmp`.`serieNF`,`vendanftmp`.`icms`,`vendanftmp`.`cfop`,`vendanftmp`.`codigofilial`,`vendanftmp`.`tributacao` */;

/*View structure for view blococregc300 */

/*!50001 DROP TABLE IF EXISTS `blococregc300` */;
/*!50001 DROP VIEW IF EXISTS `blococregc300` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc300` AS select `contdocs`.`ip` AS `ip`,`contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`Totalbruto` AS `Totalbruto`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,`contdocs`.`desconto` AS `desconto`,`contdocs`.`total` AS `total`,`contdocs`.`NrParcelas` AS `NrParcelas`,`contdocs`.`vendedor` AS `vendedor`,`contdocs`.`operador` AS `operador`,`contdocs`.`Observacao` AS `Observacao`,`contdocs`.`classe` AS `classe`,`contdocs`.`dataexe` AS `dataexe`,`contdocs`.`codigocliente` AS `codigocliente`,`contdocs`.`nome` AS `nome`,`contdocs`.`CodigoFilial` AS `CodigoFilial`,`contdocs`.`historico` AS `historico`,`contdocs`.`vrjuros` AS `vrjuros`,`contdocs`.`tipopagamento` AS `tipopagamento`,`contdocs`.`encargos` AS `encargos`,`contdocs`.`id` AS `id`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`enderecoentrega` AS `enderecoentrega`,`contdocs`.`custos` AS `custos`,`contdocs`.`devolucaovenda` AS `devolucaovenda`,`contdocs`.`devolucaorecebimento` AS `devolucaorecebimento`,`contdocs`.`nrboletobancario` AS `nrboletobancario`,`contdocs`.`nrnotafiscal` AS `nrnotafiscal`,`contdocs`.`classedevolucao` AS `classedevolucao`,`contdocs`.`responsavelreceber` AS `responsavelreceber`,`contdocs`.`numeroentrega` AS `numeroentrega`,`contdocs`.`cidadeentrega` AS `cidadeentrega`,`contdocs`.`cepentrega` AS `cepentrega`,`contdocs`.`bairroentrega` AS `bairroentrega`,`contdocs`.`horaentrega` AS `horaentrega`,`contdocs`.`dataentrega` AS `dataentrega`,`contdocs`.`obsentrega` AS `obsentrega`,`contdocs`.`concluido` AS `concluido`,`contdocs`.`cartaofidelidade` AS `cartaofidelidade`,`contdocs`.`bordero` AS `bordero`,`contdocs`.`valorservicos` AS `valorservicos`,`contdocs`.`descontoservicos` AS `descontoservicos`,`contdocs`.`romaneio` AS `romaneio`,`contdocs`.`hora` AS `hora`,`contdocs`.`entregaconcluida` AS `entregaconcluida`,`contdocs`.`dataentregaconcluida` AS `dataentregaconcluida`,`contdocs`.`operadorentrega` AS `operadorentrega`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`nreducaoz` AS `nreducaoz`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`TEF` AS `TEF`,`contdocs`.`ecfValorCancelamentos` AS `ecfValorCancelamentos`,`contdocs`.`NF_e` AS `NF_e`,`contdocs`.`estadoentrega` AS `estadoentrega`,`contdocs`.`ecfConsumidor` AS `ecfConsumidor`,`contdocs`.`ecfCPFCNPJconsumidor` AS `ecfCPFCNPJconsumidor`,`contdocs`.`ecfEndConsumidor` AS `ecfEndConsumidor`,`contdocs`.`prevendanumero` AS `prevendanumero`,`contdocs`.`ecfcontadorcupomfiscal` AS `ecfcontadorcupomfiscal`,`contdocs`.`ecftotalliquido` AS `ecftotalliquido`,`contdocs`.`contadornaofiscalGNF` AS `contadornaofiscalGNF`,`contdocs`.`contadordebitocreditoCDC` AS `contadordebitocreditoCDC`,`contdocs`.`totalICMScupomfiscal` AS `totalICMScupomfiscal`,`contdocs`.`troco` AS `troco`,`contdocs`.`davnumero` AS `davnumero`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfMFadicional` AS `ecfMFadicional`,`contdocs`.`ecftipo` AS `ecftipo`,`contdocs`.`ecfmarca` AS `ecfmarca`,`contdocs`.`ecfmodelo` AS `ecfmodelo`,`contdocs`.`estoqueatualizado` AS `estoqueatualizado`,`contdocs`.`serienf` AS `serienf`,`contdocs`.`EADRegistroDAV` AS `EADRegistroDAV`,`contdocs`.`EADr06` AS `EADr06`,`contdocs`.`tipopagamentoECF` AS `tipopagamentoECF`,`contdocs`.`modeloDOCFiscal` AS `modeloDOCFiscal`,`contdocs`.`subserienf` AS `subserienf`,sum(`contdocs`.`total`) AS `totalDocumento` from `contdocs` where ((`contdocs`.`modeloDOCFiscal` = '02') or (`contdocs`.`modeloDOCFiscal` = 'D1')) group by `contdocs`.`data`,`contdocs`.`serienf`,`contdocs`.`subserienf` */;

/*View structure for view blococregc320 */

/*!50001 DROP TABLE IF EXISTS `blococregc320` */;
/*!50001 DROP VIEW IF EXISTS `blococregc320` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc320` AS select `vendatmp`.`data` AS `data`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`serieNF` AS `serieNF`,`vendatmp`.`subserienf` AS `subserienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`ecfnumero` AS `ecfnumero`,`vendatmp`.`NotaFiscal` AS `NotaFiscal`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`total` AS `total`,sum(if((`vendatmp`.`icms` > 0),`vendatmp`.`total`,0)) AS `bcICMS`,((`vendatmp`.`total` * `vendatmp`.`icms`) / 100) AS `totalICMS` from `vendatmp` group by `vendatmp`.`tributacao`,`vendatmp`.`cfop`,`vendatmp`.`icms`,`vendatmp`.`data` union all select `venda`.`data` AS `data`,`venda`.`documento` AS `documento`,`venda`.`serieNF` AS `serieNF`,`venda`.`subserienf` AS `subserienf`,`venda`.`modelodocfiscal` AS `modelodocfiscal`,`venda`.`Ecfnumero` AS `ecfnumero`,`venda`.`NotaFiscal` AS `NotaFiscal`,`venda`.`tributacao` AS `tributacao`,`venda`.`cfop` AS `cfop`,`venda`.`icms` AS `icms`,sum(`venda`.`total`) AS `total`,sum(if((`venda`.`icms` > 0),`venda`.`total`,0)) AS `bcICMS`,((`venda`.`total` * `venda`.`icms`) / 100) AS `totalICMS` from `venda` group by `venda`.`tributacao`,`venda`.`cfop`,`venda`.`icms`,`venda`.`data` */;

/*View structure for view blococregc321 */

/*!50001 DROP TABLE IF EXISTS `blococregc321` */;
/*!50001 DROP VIEW IF EXISTS `blococregc321` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc321` AS select `venda`.`inc` AS `inc`,`venda`.`codigofilial` AS `codigofilial`,`venda`.`operador` AS `operador`,`venda`.`data` AS `data`,`venda`.`codigo` AS `codigo`,`venda`.`produto` AS `produto`,`venda`.`quantidade` AS `quantidade`,`venda`.`preco` AS `preco`,`venda`.`custo` AS `custo`,`venda`.`precooriginal` AS `precooriginal`,`venda`.`Descontoperc` AS `Descontoperc`,`venda`.`id` AS `id`,`venda`.`descontovalor` AS `descontovalor`,`venda`.`total` AS `total`,`venda`.`vendedor` AS `vendedor`,`venda`.`nrcontrole` AS `nrcontrole`,`venda`.`documento` AS `documento`,`venda`.`grupo` AS `grupo`,`venda`.`subgrupo` AS `subgrupo`,`venda`.`comissao` AS `comissao`,`venda`.`ratdesc` AS `ratdesc`,`venda`.`rateioencargos` AS `rateioencargos`,`venda`.`situacao` AS `situacao`,`venda`.`customedio` AS `customedio`,`venda`.`Ecfnumero` AS `Ecfnumero`,`venda`.`fornecedor` AS `fornecedor`,`venda`.`fabricante` AS `fabricante`,`venda`.`NotaFiscal` AS `NotaFiscal`,`venda`.`icms` AS `icms`,`venda`.`classe` AS `classe`,`venda`.`secao` AS `secao`,`venda`.`lote` AS `lote`,`venda`.`tributacao` AS `tributacao`,`venda`.`aentregar` AS `aentregar`,`venda`.`quantidadeanterior` AS `quantidadeanterior`,`venda`.`quantidadeatualizada` AS `quantidadeatualizada`,`venda`.`codigofiscal` AS `codigofiscal`,`venda`.`customedioanterior` AS `customedioanterior`,`venda`.`codigocliente` AS `codigocliente`,`venda`.`numerodevolucao` AS `numerodevolucao`,`venda`.`codigobarras` AS `codigobarras`,`venda`.`aliquotaIPI` AS `ipi`,`venda`.`unidade` AS `unidade`,`venda`.`embalagem` AS `embalagem`,`venda`.`grade` AS `grade`,`venda`.`romaneio` AS `romaneio`,`venda`.`tipo` AS `tipo`,`venda`.`cofins` AS `cofins`,`venda`.`pis` AS `pis`,`venda`.`despesasacessorias` AS `despesasacessorias`,`venda`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`venda`.`serieNF` AS `serieNF`,`venda`.`subserienf` AS `subserienf`,`venda`.`cfop` AS `cfop`,`venda`.`acrescimototalitem` AS `acrescimototalitem`,`venda`.`cstpis` AS `cstpis`,`venda`.`cstcofins` AS `cstcofins`,`venda`.`icmsst` AS `icmsst`,`venda`.`percentualRedBaseCalcICMSST` AS `percentualRedBaseCalcICMSST`,`venda`.`mvast` AS `mvast`,`venda`.`modelodocfiscal` AS `modelodocfiscal`,sum(`venda`.`quantidade`) AS `somaQuantidade`,sum((`venda`.`total` - `venda`.`ratdesc`)) AS `totalItem`,sum(`venda`.`ratdesc`) AS `totalDesconto`,if((`venda`.`icms` > 0),truncate(sum(((`venda`.`total` + `venda`.`rateioencargos`) - `venda`.`ratdesc`)),2),0) AS `bcICMS`,if((`venda`.`icms` > 0),truncate(sum(((`venda`.`total` + `venda`.`rateioencargos`) - `venda`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(((`venda`.`total` + `venda`.`rateioencargos`) - `venda`.`ratdesc`)) * `venda`.`icms`) / 100),2) AS `totalICMS`,sum(`venda`.`pis`) AS `totalPIS`,sum(`venda`.`cofins`) AS `totalCOFINS` from `venda` where ((`venda`.`modelodocfiscal` = '02') or (`venda`.`modelodocfiscal` = 'D1')) group by `venda`.`codigo` union all select `vendatmp`.`inc` AS `inc`,`vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`operador` AS `operador`,`vendatmp`.`data` AS `data`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,`vendatmp`.`quantidade` AS `quantidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`custo` AS `custo`,`vendatmp`.`precooriginal` AS `precooriginal`,`vendatmp`.`Descontoperc` AS `Descontoperc`,`vendatmp`.`id` AS `id`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`total` AS `total`,`vendatmp`.`vendedor` AS `vendedor`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`grupo` AS `grupo`,`vendatmp`.`subgrupo` AS `subgrupo`,`vendatmp`.`comissao` AS `comissao`,`vendatmp`.`ratdesc` AS `ratdesc`,`vendatmp`.`rateioencargos` AS `rateioencargos`,`vendatmp`.`situacao` AS `situacao`,`vendatmp`.`customedio` AS `customedio`,`vendatmp`.`ecfnumero` AS `Ecfnumero`,`vendatmp`.`fornecedor` AS `fornecedor`,`vendatmp`.`fabricante` AS `fabricante`,`vendatmp`.`NotaFiscal` AS `NotaFiscal`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`classe` AS `classe`,`vendatmp`.`secao` AS `secao`,`vendatmp`.`lote` AS `lote`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`aentregar` AS `aentregar`,`vendatmp`.`quantidadeanterior` AS `quantidadeanterior`,`vendatmp`.`quantidadeatualizada` AS `quantidadeatualizada`,`vendatmp`.`codigofiscal` AS `codigofiscal`,`vendatmp`.`customedioanterior` AS `customedioanterior`,`vendatmp`.`codigocliente` AS `codigocliente`,`vendatmp`.`numerodevolucao` AS `numerodevolucao`,`vendatmp`.`codigobarras` AS `codigobarras`,`vendatmp`.`aliquotaIPI` AS `ipi`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`embalagem` AS `embalagem`,`vendatmp`.`grade` AS `grade`,`vendatmp`.`romaneio` AS `romaneio`,`vendatmp`.`tipo` AS `tipo`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`despesasacessorias` AS `despesasacessorias`,`vendatmp`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`vendatmp`.`serieNF` AS `serieNF`,`vendatmp`.`subserienf` AS `subserienf`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`acrescimototalitem` AS `acrescimototalitem`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`icmsst` AS `icmsst`,`vendatmp`.`percentualRedBaseCalcICMSST` AS `percentualRedBaseCalcICMSST`,`vendatmp`.`mvast` AS `mvast`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,sum(`vendatmp`.`quantidade`) AS `somaQuantidade`,sum(`vendatmp`.`total`) AS `totalItem`,sum(`vendatmp`.`ratdesc`) AS `totalDesconto`,if((`vendatmp`.`icms` > 0),truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2),0) AS `bcICMS`,if((`vendatmp`.`icms` > 0),truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,sum(`vendatmp`.`pis`) AS `totalPIS`,sum(`vendatmp`.`cofins`) AS `totalCOFINS` from `vendatmp` where ((`vendatmp`.`modelodocfiscal` = '02') or (`vendatmp`.`modelodocfiscal` = 'D1')) group by `vendatmp`.`codigo` */;

/*View structure for view blococregc381_pis */

/*!50001 DROP TABLE IF EXISTS `blococregc381_pis` */;
/*!50001 DROP VIEW IF EXISTS `blococregc381_pis` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc381_pis` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`contdocs`.`NF_e` = 'N') and (`contdocs`.`serienf` = 'D') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '02')) group by `vendatmp`.`codigo` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc385_cofins */

/*!50001 DROP TABLE IF EXISTS `blococregc385_cofins` */;
/*!50001 DROP VIEW IF EXISTS `blococregc385_cofins` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc385_cofins` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`contdocs`.`NF_e` = 'N') and (`contdocs`.`serienf` = 'D') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '02')) group by `vendatmp`.`codigo` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc390 */

/*!50001 DROP TABLE IF EXISTS `blococregc390` */;
/*!50001 DROP VIEW IF EXISTS `blococregc390` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc390` AS select `venda`.`documento` AS `documento`,`venda`.`data` AS `data`,`venda`.`icms` AS `icms`,`venda`.`cfop` AS `cfop`,`venda`.`tributacao` AS `tributacao`,truncate(sum((`venda`.`total` - `venda`.`ratdesc`)),2) AS `total`,if((`venda`.`icms` > 0),truncate(sum((`venda`.`total` - `venda`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(`venda`.`total`) * `venda`.`icms`) / 100),2) AS `totalICMS` from `venda` where (`venda`.`quantidade` > 0) group by `venda`.`tributacao`,`venda`.`cfop`,`venda`.`icms`,`venda`.`documento` union all select `vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `data`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`tributacao` AS `tributacao`,truncate(sum(`vendatmp`.`total`),2) AS `total`,if((`vendatmp`.`icms` > 0),truncate(sum(`vendatmp`.`total`),2),0) AS `baseCalculoICMS`,truncate(((sum(`vendatmp`.`total`) * `vendatmp`.`icms`) / 100),2) AS `totalICMS` from `vendatmp` where (`vendatmp`.`quantidade` > 0) group by `vendatmp`.`tributacao`,`vendatmp`.`cfop`,`vendatmp`.`icms`,`vendatmp`.`documento` */;

/*View structure for view blococregc400 */

/*!50001 DROP TABLE IF EXISTS `blococregc400` */;
/*!50001 DROP VIEW IF EXISTS `blococregc400` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`127.0.0.1` SQL SECURITY DEFINER VIEW `blococregc400` AS select `r02`.`id` AS `id`,`r02`.`codigofilial` AS `codigofilial`,`r02`.`data` AS `data`,`r02`.`tipo` AS `tipo`,`r02`.`fabricacaoECF` AS `fabricacaoECF`,`r02`.`MFadicional` AS `MFadicional`,`r02`.`modeloECF` AS `modeloECF`,`r02`.`numeroUsuarioSubstituicaoECF` AS `numeroUsuarioSubstituicaoECF`,`r02`.`crz` AS `crz`,`r02`.`coo` AS `coo`,`r02`.`cro` AS `cro`,`r02`.`datamovimento` AS `datamovimento`,`r02`.`dataemissaoreducaoz` AS `dataemissaoreducaoz`,`r02`.`horaemissaoreducaoz` AS `horaemissaoreducaoz`,`r02`.`vendabrutadiaria` AS `vendabrutadiaria`,`r02`.`parametroISSQNdesconto` AS `parametroISSQNdesconto`,`r02`.`numeroECF` AS `numeroECF`,`r02`.`gtfinal` AS `gtfinal`,`r02`.`EADdados` AS `EADdados` from `r02` group by `r02`.`fabricacaoECF` */;

/*View structure for view blococregc425 */

/*!50001 DROP TABLE IF EXISTS `blococregc425` */;
/*!50001 DROP VIEW IF EXISTS `blococregc425` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc425` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`nrnotafiscal` AS `nrnotafiscal`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`ecfCPFCNPJconsumidor` AS `ecfCPFCNPJconsumidor`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,truncate(`vendatmp`.`quantidade`,3) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2) AS `total`,truncate(sum(if((`vendatmp`.`icms` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoICMS`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,truncate(sum(if((`vendatmp`.`aliquotaIPI` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoIPI`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`aliquotaIPI`) / 100),2) AS `totalIPI`,truncate(sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`pis`),0)),2) AS `totalPIS`,truncate(sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`cofins`),0)),2) AS `totalCOFINS`,sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`),0)) AS `totalPIS_QTD`,sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`),0)) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,sum(if(((`vendatmp`.`icms` = 0) and (`vendatmp`.`tributacao` = '40')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorisentas`,sum(if(((`vendatmp`.`tributacao` = '41') or (`vendatmp`.`tributacao` = '80')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorNT`,sum(if(((`vendatmp`.`tributacao` = '60') or (`vendatmp`.`tributacao` = '30')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorST` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`codigo`,`vendatmp`.`data`,`vendatmp`.`ecffabricacao` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc470 */

/*!50001 DROP TABLE IF EXISTS `blococregc470` */;
/*!50001 DROP VIEW IF EXISTS `blococregc470` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc470` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`nrnotafiscal` AS `nrnotafiscal`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`ecfCPFCNPJconsumidor` AS `ecfCPFCNPJconsumidor`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,truncate(`vendatmp`.`quantidade`,3) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2) AS `total`,truncate(sum(if((`vendatmp`.`icms` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoICMS`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,truncate(sum(if((`vendatmp`.`aliquotaIPI` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoIPI`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`aliquotaIPI`) / 100),2) AS `totalIPI`,truncate(sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`pis`),0)),2) AS `totalPIS`,truncate(sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`cofins`),0)),2) AS `totalCOFINS`,sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`),0)) AS `totalPIS_QTD`,sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`),0)) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,sum(if(((`vendatmp`.`icms` = 0) and (`vendatmp`.`tributacao` = '40')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorisentas`,sum(if(((`vendatmp`.`tributacao` = '41') or (`vendatmp`.`tributacao` = '80')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorNT`,sum(if(((`vendatmp`.`tributacao` = '60') or (`vendatmp`.`tributacao` = '30')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorST` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`inc`,`vendatmp`.`documento`,`vendatmp`.`nrcontrole`,`vendatmp`.`ccf`,`vendatmp`.`ecffabricacao` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc470_02_itens */

/*!50001 DROP TABLE IF EXISTS `blococregc470_02_itens` */;
/*!50001 DROP VIEW IF EXISTS `blococregc470_02_itens` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc470_02_itens` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`nrnotafiscal` AS `nrnotafiscal`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`ecfCPFCNPJconsumidor` AS `ecfCPFCNPJconsumidor`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,truncate(`vendatmp`.`quantidade`,3) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2) AS `total`,truncate(sum(if((`vendatmp`.`icms` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoICMS`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,truncate(sum(if((`vendatmp`.`aliquotaIPI` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoIPI`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`aliquotaIPI`) / 100),2) AS `totalIPI`,truncate(sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`pis`),0)),2) AS `totalPIS`,truncate(sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`cofins`),0)),2) AS `totalCOFINS`,sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`),0)) AS `totalPIS_QTD`,sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`),0)) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,sum(if(((`vendatmp`.`icms` = 0) and (`vendatmp`.`tributacao` = '40')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorisentas`,sum(if(((`vendatmp`.`tributacao` = '41') or (`vendatmp`.`tributacao` = '80')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorNT`,sum(if(((`vendatmp`.`tributacao` = '60') or (`vendatmp`.`tributacao` = '30')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorST` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and ((`vendatmp`.`modelodocfiscal` = '2D') or (`vendatmp`.`modelodocfiscal` = 'D1'))) group by `vendatmp`.`inc`,`vendatmp`.`documento`,`vendatmp`.`nrcontrole`,`vendatmp`.`ccf`,`vendatmp`.`ecffabricacao` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc481_pis */

/*!50001 DROP TABLE IF EXISTS `blococregc481_pis` */;
/*!50001 DROP VIEW IF EXISTS `blococregc481_pis` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc481_pis` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`codigo`,`vendatmp`.`codigofilial`,`vendatmp`.`data` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc485_cofins */

/*!50001 DROP TABLE IF EXISTS `blococregc485_cofins` */;
/*!50001 DROP VIEW IF EXISTS `blococregc485_cofins` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc485_cofins` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(`vendatmp`.`total`,2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(`vendatmp`.`total`,2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`codigo`,`vendatmp`.`codigofilial`,`vendatmp`.`data` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc490 */

/*!50001 DROP TABLE IF EXISTS `blococregc490` */;
/*!50001 DROP VIEW IF EXISTS `blococregc490` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc490` AS select `contdocs`.`CodigoFilial` AS `codigofilial`,`contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,truncate(`vendatmp`.`quantidade`,3) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`ratdesc`) AS `ratdesc`,sum(`vendatmp`.`ratdesc`) AS `rateioencargos`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2) AS `total`,if((`vendatmp`.`icms` > 0),truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,sum(if(((`vendatmp`.`icms` = 0) and (`vendatmp`.`tributacao` <> '60')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorisentas`,sum(if(((`vendatmp`.`tributacao` = '41') or (`vendatmp`.`tributacao` = '80')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorNT`,sum(if(((`vendatmp`.`tributacao` = '60') or (`vendatmp`.`tributacao` = '30')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorST` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`vendatmp`.`quantidade` > 0) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D') and (`contdocs`.`modeloDOCFiscal` = '2D')) group by `vendatmp`.`tributacao`,`vendatmp`.`cfop`,`vendatmp`.`icms`,`vendatmp`.`data`,`vendatmp`.`modelodocfiscal`,`vendatmp`.`ecffabricacao` */;

/*View structure for view blococregc491_pis */

/*!50001 DROP TABLE IF EXISTS `blococregc491_pis` */;
/*!50001 DROP VIEW IF EXISTS `blococregc491_pis` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc491_pis` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(`vendatmp`.`total`,2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(`vendatmp`.`total`,2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`codigo`,`vendatmp`.`codigofilial` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc495_cofins */

/*!50001 DROP TABLE IF EXISTS `blococregc495_cofins` */;
/*!50001 DROP VIEW IF EXISTS `blococregc495_cofins` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc495_cofins` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`codigo`,`vendatmp`.`codigofilial` order by `vendatmp`.`nrcontrole` */;

/*View structure for view endereco_completo */

/*!50001 DROP TABLE IF EXISTS `endereco_completo` */;
/*!50001 DROP VIEW IF EXISTS `endereco_completo` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `endereco_completo` AS select `e`.`logradouro` AS `logradouro`,`e`.`endereco` AS `endereco`,`b`.`bairro` AS `bairro`,`c`.`cidade` AS `cidade`,`uf`.`uf` AS `uf`,`e`.`cep` AS `cep` from (((`enderecos` `e` join `cidades` `c` on((`e`.`id_cidade` = `c`.`id_cidade`))) join `estados` `uf` on((`c`.`id_estado` = `uf`.`id_estado`))) join `bairros` `b` on((`e`.`id_bairro` = `b`.`id_bairro`))) */;

/*View structure for view r05 */

/*!50001 DROP TABLE IF EXISTS `r05` */;
/*!50001 DROP VIEW IF EXISTS `r05` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `r05` AS select ifnull(`venda`.`coo`,'') AS `ncupomfiscal`,ifnull(`venda`.`ccf`,'') AS `ecfcontadorcupomfiscal`,`contdocs`.`data` AS `data`,`contdocs`.`documento` AS `documento`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,ifnull(`venda`.`ecffabricacao`,'') AS `ecffabricacao`,`contdocs`.`ecfMFadicional` AS `ecfMFadicional`,`contdocs`.`ecfmodelo` AS `ecfmodelo`,`venda`.`nrcontrole` AS `nrcontrole`,`venda`.`codigo` AS `codigo`,`venda`.`produto` AS `produto`,`venda`.`quantidade` AS `quantidade`,`venda`.`unidade` AS `unidade`,`venda`.`precooriginal` AS `precooriginal`,`venda`.`descontovalor` AS `descontovalor`,`venda`.`preco` AS `preco`,`venda`.`total` AS `total`,`venda`.`icms` AS `icms`,`venda`.`tributacao` AS `tributacao`,`venda`.`cancelado` AS `cancelado`,`venda`.`ccf` AS `ccf`,`venda`.`coo` AS `coo`,`venda`.`acrescimototalitem` AS `acrescimototalitem`,`venda`.`Descontoperc` AS `Descontoperc`,ifnull(`venda`.`eaddados`,'') AS `eaddados`,`produtos`.`indicadorproducao` AS `indicadorproducao`,`produtos`.`indicadorarredondamentotruncamento` AS `indicadorarredondamentotruncamento` from ((`contdocs` join `venda`) join `produtos`) where ((`venda`.`documento` = `contdocs`.`documento`) and (`produtos`.`codigo` = `venda`.`codigo`) and (`venda`.`quantidade` > 0)) union all select ifnull(`vendatmp`.`coo`,'') AS `ncupomfiscal`,ifnull(`vendatmp`.`ccf`,'') AS `ecfcontadorcupomfiscal`,`contdocs`.`data` AS `data`,`contdocs`.`documento` AS `documento`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,ifnull(`vendatmp`.`ecffabricacao`,'') AS `ecffabricacao`,`contdocs`.`ecfMFadicional` AS `ecfMFadicional`,`contdocs`.`ecfmodelo` AS `ecfmodelo`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,`vendatmp`.`quantidade` AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`precooriginal` AS `precooriginal`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`total` AS `total`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cancelado` AS `cancelado`,ifnull(`vendatmp`.`ccf`,'') AS `ccf`,ifnull(`vendatmp`.`coo`,'') AS `coo`,`vendatmp`.`acrescimototalitem` AS `acrescimototalitem`,`vendatmp`.`Descontoperc` AS `Descontoperc`,ifnull(`vendatmp`.`eaddados`,'') AS `eaddados`,`produtos`.`indicadorproducao` AS `indicadorproducao`,`produtos`.`indicadorarredondamentotruncamento` AS `indicadorarredondamentotruncamento` from ((`contdocs` join `vendatmp`) join `produtos`) where ((`vendatmp`.`documento` = `contdocs`.`documento`) and (`produtos`.`codigo` = `vendatmp`.`codigo`) and (`contdocs`.`documento` <> '') and (`vendatmp`.`quantidade` > 0) and (`produtos`.`CodigoFilial` = `contdocs`.`CodigoFilial`) and (`vendatmp`.`ecfnumero` <> '')) */;

/*View structure for view registro50entradas_agr */

/*!50001 DROP TABLE IF EXISTS `registro50entradas_agr` */;
/*!50001 DROP VIEW IF EXISTS `registro50entradas_agr` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50entradas_agr` AS select (select `moventradas`.`UFemitente` AS `UFemitente` from `moventradas` where (`moventradas`.`numero` = `entradas`.`numero`)) AS `UFemitente`,`entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`icmsst` AS `icmsst`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`ratdesconto`) AS `desconto`,sum(`entradas`.`ratdespesas`) AS `totalDespesas`,sum(`entradas`.`ratseguro`) AS `totalSeguro`,sum(`entradas`.`ratfrete`) AS `totalFrete`,sum(`entradas`.`valoroutrasdespesas`) AS `valoroutrasdespesas`,if((`entradas`.`IcmsEntrada` > 0),sum(`entradas`.`bcicms`),0) AS `bcicms`,if((`entradas`.`IcmsEntrada` = 0),sum(`entradas`.`totalitem`),0) AS `valorisentas`,round(if((`entradas`.`IcmsEntrada` > 0),sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),0),2) AS `toticms`,sum(`entradas`.`bcicmsST`) AS `bcicmsST`,sum(`entradas`.`valoricmsST`) AS `valoricmsST`,if((`entradas`.`IPI` > 0),sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)),0) AS `ipiItem`,round(if((`entradas`.`pis` > 0),sum(((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)) * (`entradas`.`pis` / 100))),0),2) AS `totPIS`,round(if((`entradas`.`cofins` > 0),sum(((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)) * (`entradas`.`cofins` / 100))),0),2) AS `totCOFINS`,sum(`entradas`.`totalitem`) AS `totalProduto`,((((((sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)) AS `totalNF`,(((((sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) AS `valoroutrasaliquotas`,sum(round((`entradas`.`totalitem` * (`entradas`.`percentualRedBaseCalcICMS` / 100)),2)) AS `totalReducaoICMS`,if((`entradas`.`IPI` > 0),sum(`entradas`.`totalitem`),0) AS `baseCalculoIPI`,if((`entradas`.`pis` > 0),sum(round((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)),2)),0) AS `baseCalculoPIS`,if((`entradas`.`cofins` > 0),sum(round((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)),2)),0) AS `baseCalculoCOFINS`,`entradas`.`Lancada` AS `lancada` from `entradas` where ((`entradas`.`exportarfiscal` = 'S') and (`entradas`.`Lancada` = 'X')) group by `entradas`.`NF`,`entradas`.`cfopentrada`,`entradas`.`IcmsEntrada`,`entradas`.`numero` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view registro50entradas_itens */

/*!50001 DROP TABLE IF EXISTS `registro50entradas_itens` */;
/*!50001 DROP VIEW IF EXISTS `registro50entradas_itens` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50entradas_itens` AS select (select `moventradas`.`UFemitente` AS `UFemitente` from `moventradas` where (`moventradas`.`numero` = `entradas`.`numero`)) AS `UFemitente`,`entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`icmsst` AS `icmsst`,`entradas`.`IPI` AS `ipi`,`entradas`.`pis` AS `pis`,`entradas`.`cofins` AS `cofins`,`entradas`.`cstpis` AS `cstpis`,`entradas`.`cstcofins` AS `cstcofins`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`ratdesconto`) AS `desconto`,sum(`entradas`.`ratdespesas`) AS `totalDespesas`,sum(`entradas`.`ratseguro`) AS `totalSeguro`,sum(`entradas`.`ratfrete`) AS `totalFrete`,sum(`entradas`.`valoroutrasdespesas`) AS `valoroutrasdespesas`,if((`entradas`.`IcmsEntrada` > 0),sum(`entradas`.`bcicms`),0) AS `bcicms`,if((`entradas`.`IcmsEntrada` = 0),sum(`entradas`.`totalitem`),0) AS `valorisentas`,round(if((`entradas`.`IcmsEntrada` > 0),sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),0),2) AS `toticms`,sum(`entradas`.`bcicmsST`) AS `bcicmsST`,sum(`entradas`.`valoricmsST`) AS `valoricmsST`,if((`entradas`.`IPI` > 0),sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)),0) AS `ipiItem`,round(if((`entradas`.`pis` > 0),sum(((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)) * (`entradas`.`pis` / 100))),0),2) AS `totPIS`,round(if((`entradas`.`cofins` > 0),sum(((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)) * (`entradas`.`cofins` / 100))),0),2) AS `totCOFINS`,sum(`entradas`.`totalitem`) AS `totalProduto`,((((((sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)) AS `totalNF`,(((((sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) AS `valoroutrasaliquotas`,if((`entradas`.`IPI` > 0),sum(`entradas`.`totalitem`),0) AS `baseCalculoIPI`,if((`entradas`.`pis` > 0),sum(round((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)),2)),0) AS `baseCalculoPIS`,if((`entradas`.`cofins` > 0),sum(round((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)),2)),0) AS `baseCalculoCOFINS`,`entradas`.`Lancada` AS `lancada` from `entradas` where ((`entradas`.`exportarfiscal` = 'S') and (`entradas`.`Lancada` = 'X')) group by `entradas`.`NF`,`entradas`.`inc` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view registro50saida_agr */

/*!50001 DROP TABLE IF EXISTS `registro50saida_agr` */;
/*!50001 DROP VIEW IF EXISTS `registro50saida_agr` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50saida_agr` AS select `vendanftmp`.`inc` AS `inc`,`vendanftmp`.`codigofilial` AS `codigofilial`,`vendanftmp`.`NotaFiscal` AS `notafiscal`,`vendanftmp`.`serieNF` AS `serienf`,`vendanftmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendanftmp`.`documento` AS `documento`,`vendanftmp`.`data` AS `DATA`,`vendanftmp`.`cfop` AS `cfop`,`vendanftmp`.`icms` AS `icms`,`vendanftmp`.`icmsst` AS `icmsst`,ifnull(`vendanftmp`.`aliquotaIPI`,0) AS `ipi`,`vendanftmp`.`pis` AS `pis`,`vendanftmp`.`cofins` AS `cofins`,`vendanftmp`.`cstpis` AS `cstpis`,`vendanftmp`.`cstcofins` AS `cstcofins`,`vendanftmp`.`cstipi` AS `cstipi`,`vendanftmp`.`tributacao` AS `tributacao`,`vendanftmp`.`codigo` AS `codigo`,`vendanftmp`.`produto` AS `produto`,sum(`vendanftmp`.`quantidade`) AS `SUM(quantidade)`,`vendanftmp`.`unidade` AS `unidade`,`vendanftmp`.`nrcontrole` AS `nrcontrole`,`vendanftmp`.`origem` AS `origem`,sum(round((((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - ((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`icms`) / 100),2)) AS `totalicms`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((((`vendanftmp`.`total` - ((`vendanftmp`.`total` * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`aliquotaIPI`) / 100),2)),0) AS `totalIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`pis`) / 100),2)),0) AS `totalPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`cofins`) / 100),2)),0) AS `totalCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`cofins`)),0) AS `totalCOFINS_QTD`,sum((`vendanftmp`.`descontovalor` + `vendanftmp`.`ratdesc`)) AS `descontovalor`,if(((`vendanftmp`.`icms` = 0) or (`vendanftmp`.`tributacao` = '010')),sum(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`)),0) AS `valorisentas`,sum(round(`vendanftmp`.`total`,2)) AS `totalbruto`,sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)) AS `SUM(TOTAL)`,sum(`vendanftmp`.`total`) AS `totalItem`,if((`vendanftmp`.`icms` > 0),sum(round((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - (((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((`vendanftmp`.`total` - (`vendanftmp`.`total` * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD`,if((`vendanftmp`.`icmsst` > 0),sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)),0) AS `bcICMSST`,truncate(if(((`vendanftmp`.`icmsst` > 0) and (`vendanftmp`.`icmsst` >= `vendanftmp`.`icms`)),(((sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)) * `vendanftmp`.`icmsst`) / 100) - sum(round(((`vendanftmp`.`total` * `vendanftmp`.`icms`) / 100),2))),0),2) AS `totalICMSST` from `vendanftmp` where (`vendanftmp`.`quantidade` > 0) group by `vendanftmp`.`NotaFiscal`,`vendanftmp`.`serieNF`,`vendanftmp`.`icms`,`vendanftmp`.`cfop`,`vendanftmp`.`codigofilial` */;

/*View structure for view registro50saidas_itens */

/*!50001 DROP TABLE IF EXISTS `registro50saidas_itens` */;
/*!50001 DROP VIEW IF EXISTS `registro50saidas_itens` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50saidas_itens` AS select `vendanftmp`.`inc` AS `inc`,`vendanftmp`.`codigofilial` AS `codigofilial`,`vendanftmp`.`NotaFiscal` AS `notafiscal`,`vendanftmp`.`serieNF` AS `serienf`,`vendanftmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendanftmp`.`documento` AS `documento`,`vendanftmp`.`data` AS `DATA`,`vendanftmp`.`cfop` AS `cfop`,`vendanftmp`.`icms` AS `icms`,`vendanftmp`.`icmsst` AS `icmsst`,ifnull(`vendanftmp`.`aliquotaIPI`,0) AS `ipi`,`vendanftmp`.`pis` AS `pis`,`vendanftmp`.`cofins` AS `cofins`,`vendanftmp`.`cstpis` AS `cstpis`,`vendanftmp`.`cstcofins` AS `cstcofins`,`vendanftmp`.`cstipi` AS `cstipi`,`vendanftmp`.`tributacao` AS `tributacao`,`vendanftmp`.`codigo` AS `codigo`,`vendanftmp`.`produto` AS `produto`,sum(`vendanftmp`.`quantidade`) AS `SUM(quantidade)`,`vendanftmp`.`unidade` AS `unidade`,`vendanftmp`.`nrcontrole` AS `nrcontrole`,`vendanftmp`.`origem` AS `origem`,sum(round((((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - ((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`icms`) / 100),2)) AS `totalicms`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((((`vendanftmp`.`total` - ((`vendanftmp`.`total` * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`aliquotaIPI`) / 100),2)),0) AS `totalIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`pis`) / 100),2)),0) AS `totalPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`cofins`) / 100),2)),0) AS `totalCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`cofins`)),0) AS `totalCOFINS_QTD`,sum((`vendanftmp`.`descontovalor` + `vendanftmp`.`ratdesc`)) AS `descontovalor`,if(((`vendanftmp`.`icms` = 0) or (`vendanftmp`.`tributacao` = '010')),sum(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`)),0) AS `valorisentas`,sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)) AS `SUM(TOTAL)`,sum(round(`vendanftmp`.`total`,2)) AS `totalbruto`,if((`vendanftmp`.`icms` > 0),sum(round((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - (((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((`vendanftmp`.`total` - (`vendanftmp`.`total` * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD`,if((`vendanftmp`.`icmsst` > 0),sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)),0) AS `bcICMSST`,truncate(if(((`vendanftmp`.`icmsst` > 0) and (`vendanftmp`.`icmsst` >= `vendanftmp`.`icms`)),(((sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)) * `vendanftmp`.`icmsst`) / 100) - sum(round(((`vendanftmp`.`total` * `vendanftmp`.`icms`) / 100),2))),0),2) AS `totalICMSST` from `vendanftmp` where (`vendanftmp`.`quantidade` > 0) group by `vendanftmp`.`inc` order by `vendanftmp`.`NotaFiscal`,`vendanftmp`.`nrcontrole` */;

/*View structure for view v_assistentegerencial */

/*!50001 DROP TABLE IF EXISTS `v_assistentegerencial` */;
/*!50001 DROP VIEW IF EXISTS `v_assistentegerencial` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_assistentegerencial` AS (select `assistentegerencial`.`codigofilial` AS `codigofilial`,`assistentegerencial`.`ticketmedio` AS `ticketmedio`,`assistentegerencial`.`data` AS `data`,`assistentegerencial`.`hora` AS `hora`,`assistentegerencial`.`ocorrenciasauditoria` AS `ocorrenciasauditoria`,`assistentegerencial`.`auditoriacliente` AS `auditoriacliente`,`assistentegerencial`.`auditoriaacessos` AS `auditoriaacessos`,`assistentegerencial`.`auditoriaprodutos` AS `auditoriaprodutos`,`assistentegerencial`.`auditoriavendas` AS `auditoriavendas`,`assistentegerencial`.`auditoriacontaspagar` AS `auditoriacontaspagar`,`assistentegerencial`.`auditoriaestorno` AS `auditoriaestorno`,`assistentegerencial`.`vendadiaria` AS `vendadiaria`,`assistentegerencial`.`totalreceber` AS `totalreceber`,`assistentegerencial`.`totalrecebido` AS `totalrecebido` from `assistentegerencial` where (`assistentegerencial`.`data` = curdate())) */;

/*View structure for view valoresinventario */

/*!50001 DROP TABLE IF EXISTS `valoresinventario` */;
/*!50001 DROP VIEW IF EXISTS `valoresinventario` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `valoresinventario` AS select `produtos`.`CodigoFilial` AS `codigofilial`,`produtos`.`tipo` AS `tipo`,`produtos`.`grupo` AS `grupo`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`custo`),2)) AS `custo`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`),2)) AS `customedio`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`custofornecedor`),2)) AS `custofornecedor`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`precovenda`),2)) AS `total`,sum(truncate((`produtos`.`qtdretida` * `produtos`.`custo`),2)) AS `custoretidos`,sum(truncate((`produtos`.`qtdvencidos` * `produtos`.`custo`),2)) AS `custovencidos`,sum(truncate(`produtos`.`quantidade`,2)) AS `quantidade`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`icms`) / 100),2)) AS `icmsRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`aliquotaIPI`) / 100),2)) AS `ipiRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`pis`) / 100),2)) AS `pisRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`cofins`) / 100),2)) AS `cofinsRecuperar` from `produtos` where (`produtos`.`quantidade` > 0) group by `produtos`.`CodigoFilial`,`produtos`.`tipo` union all select `produtos`.`CodigoFilial` AS `codigofilial`,`produtos`.`tipo` AS `tipo`,`produtos`.`grupo` AS `grupo`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`custo`),2)) AS `custo`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`),2)) AS `customedio`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`custofornecedor`),2)) AS `custofornecedor`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`precovenda`),2)) AS `total`,sum(truncate((`produtos`.`qtdretida` * `produtos`.`custo`),2)) AS `custoretidos`,sum(truncate((`produtos`.`qtdvencidos` * `produtos`.`custo`),2)) AS `custovencidos`,sum(truncate(`produtos`.`quantidade`,2)) AS `quantidade`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`icms`) / 100),2)) AS `icmsRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`aliquotaIPI`) / 100),2)) AS `ipiRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`pis`) / 100),2)) AS `pisRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`cofins`) / 100),2)) AS `cofinsRecuperar` from `produtosfilial` `produtos` where (`produtos`.`quantidade` > 0) group by `produtos`.`CodigoFilial`,`produtos`.`tipo` */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

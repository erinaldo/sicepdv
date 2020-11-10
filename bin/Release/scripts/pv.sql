/*
SQLyog Enterprise v8.71 
MySQL - 5.5.5-10.1.20-MariaDB : Database - sice
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
/*!50106 set global event_scheduler = 1*/;

/* Event structure for event `AtualizarStatusOperadores` */

/*!50106 DROP EVENT IF EXISTS `AtualizarStatusOperadores`*/;

DELIMITER $$

/*!50106 CREATE  EVENT `AtualizarStatusOperadores` ON SCHEDULE EVERY 1 DAY STARTS '2013-07-30 00:00:00' ON COMPLETION NOT PRESERVE ENABLE DO BEGIN
	    
		
	UPDATE senhas SET senhas.disponivel="N";
	UPDATE senhas SET senhas.logado="N";
	END */$$
DELIMITER ;

/* Event structure for event `e_analiseGerencial` */

/*!50106 DROP EVENT IF EXISTS `e_analiseGerencial`*/;

DELIMITER $$

/*!50106 CREATE  EVENT `e_analiseGerencial` ON SCHEDULE EVERY 1 HOUR STARTS '2012-08-16 08:27:05' ON COMPLETION PRESERVE ENABLE DO BEGIN 
   
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

/* Function  structure for function  `dataBr` */

/*!50003 DROP FUNCTION IF EXISTS `dataBr` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `dataBr`(datap VARCHAR(10)) RETURNS char(10) CHARSET latin1
    DETERMINISTIC
BEGIN
		RETURN CONCAT(MID(datap,9,2),'/',MID(datap,6,2),'/',MID(datap,1,4));
    END */$$
DELIMITER ;

/* Function  structure for function  `fCodigoEstadoMunIBGE` */

/*!50003 DROP FUNCTION IF EXISTS `fCodigoEstadoMunIBGE` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fCodigoEstadoMunIBGE`(tipo char(1),cidade varchar(40),siglaUF varchar(2)) RETURNS varchar(100) CHARSET latin1
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

/* Function  structure for function  `fDescCPL` */

/*!50003 DROP FUNCTION IF EXISTS `fDescCPL` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fDescCPL`(xfilial VARCHAR(5), xcodigo VARCHAR(20)) RETURNS varchar(100) CHARSET latin1
    DETERMINISTIC
BEGIN
DECLARE DESC_CPL VARCHAR(100);
RETURN (SELECT IF (xfilial='00001', (SELECT TRIM(complementodescricao) FROM produtos WHERE codigo=xcodigo LIMIT 1), (SELECT TRIM(complementodescricao) FROM produtosfilial WHERE codigo=xcodigo and codigofilial=xfilial LIMIT 1)));
 
END */$$
DELIMITER ;

/* Function  structure for function  `fObterMVA` */

/*!50003 DROP FUNCTION IF EXISTS `fObterMVA` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fObterMVA`(uf VARCHAR(2), ncm VARCHAR(8), pICMSEntrada DECIMAL(8,3)) RETURNS double
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

/*!50003 CREATE  FUNCTION `fPesoBrutoVenda`(
  filial VARCHAR (5),
  ipTerminal VARCHAR (15)
) RETURNS double
    DETERMINISTIC
BEGIN
  IF (filial = "00001") 
  THEN RETURN 
  (SELECT 
    IFNULL(SUM(p.pesobruto * v.quantidade), 0) AS bruto 
  FROM
    vendas AS v,
    produtos AS p 
  WHERE v.id = ipTerminal 
    AND v.codigo = p.codigo 
    AND v.codigofilial = filial) ;
  END IF ;
  IF (filial <> "00001") 
  THEN RETURN 
  (SELECT     
    IFNULL(SUM(p.pesobruto * v.quantidade), 0) AS bruto
  FROM
    vendas AS v,
    produtosfilial AS p 
  WHERE v.id = ipTerminal 
    AND v.codigo = p.codigo 
    AND v.codigofilial = filial 
    AND p.codigofilial = filial) ;
  END IF ;
  RETURN 0 ;
  
END */$$
DELIMITER ;

/* Function  structure for function  `fvBCICMS` */

/*!50003 DROP FUNCTION IF EXISTS `fvBCICMS` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fvBCICMS`(filial VARCHAR(5),ipTerminal VARCHAR(15), idproduto VARCHAR(20), controle int(5), arredondar char(1) ) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 select 
 if(arredondar='N', 
 TRUNCATE((ratfrete + ratdespesas + ratseguro - ratdesc + IF(v.pautaicms=0,v.total,v.pautaicms)) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1), 2), 
  ROUND((ratfrete + ratdespesas + ratseguro - ratdesc + IF(v.pautaicms=0,v.total,v.pautaicms)) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1), 2))
  
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.codigo = idproduto
 AND v.cancelado = "N"  
 AND v.inc = controle 
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvBCIPI` */

/*!50003 DROP FUNCTION IF EXISTS `fvBCIPI` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fvBCIPI`(filial VARCHAR(5),ipTerminal VARCHAR(15), idproduto VARCHAR(20), controle int(5), crt VARCHAR(1), arredondar char(1)) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 select 
 if(arredondar='N',  
 TRUNCATE(
 
 IF( crt ="1", 0.00, IF(aliquotaipi>0 OR vUnidIPI>0, (ratfrete + ratdespesas + ratseguro - ratdesc + total),0) )
 
 , 2), 
 round(
 
 IF( crt ="1", 0.00, IF(aliquotaipi>0 OR vUnidIPI>0, (ratfrete + ratdespesas + ratseguro - ratdesc + total),0) )
 
 , 2)) 
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.codigo = idproduto
 AND v.cancelado = "N"  
 AND v.inc = controle 
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvicms` */

/*!50003 DROP FUNCTION IF EXISTS `fvicms` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fvicms`(filial VARCHAR(5),ipTerminal VARCHAR(15), idproduto VARCHAR(20), controle int(5), arredondar CHAR(1)) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 SELECT 
 IF(arredondar="N", 
 truncate( 
 ((ratfrete + ratdespesas + ratseguro - ratdesc + IF(v.pautaicms=0,v.total,v.pautaicms)) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1) )*(icms/100) 
  , 2), 
 ROUND( 
 ((ratfrete + ratdespesas + ratseguro - ratdesc + IF(v.pautaicms=0,v.total,v.pautaicms)) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1) )*(icms/100) 
  , 2))   
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.codigo = idproduto
 AND v.cancelado = "N"  
 AND v.inc = controle 
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvIPI` */

/*!50003 DROP FUNCTION IF EXISTS `fvIPI` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fvIPI`(filial VARCHAR(5),ipTerminal VARCHAR(15), idproduto VARCHAR(20), controle int(5),  crt VARCHAR(1), arredondar char(1)) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 select 
 if(arredondar='N', 
 TRUNCATE( 
 IF( crt ="1", 0.00, IF(aliquotaipi>0 OR vUnidIPI>0,(ratfrete + ratdespesas + ratseguro - ratdesc + total),0) ) * (aliquotaipi/100) 
 , 2), 
 round( 
 IF( crt ="1", 0.00, IF(aliquotaipi>0 OR vUnidIPI>0,(ratfrete + ratdespesas + ratseguro - ratdesc + total),0) ) * (aliquotaipi/100) 
 , 2)) 
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

/*!50003 CREATE  FUNCTION `fvTotalBCICMSnfe`(filial VARCHAR(5),ipTerminal VARCHAR(15), crt VARCHAR(1), arredondar char(1)) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 select  
 if(crt="1", 0, 
 sum(
 IF(icms>0,
 if(arredondar='N', 
 TRUNCATE((ratfrete + ratdespesas + ratseguro - ratdesc + if(pautaICMS=0, total,pautaICMS) ) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1), 2), 
 round((ratfrete + ratdespesas + ratseguro - ratdesc + IF(pautaICMS=0, total,pautaICMS) ) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1), 2))
 , 0))) 
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.cancelado = "N"  
 AND v.codigofilial=filial
 );
    END */$$
DELIMITER ;

/* Function  structure for function  `fvTotalICMSnfe` */

/*!50003 DROP FUNCTION IF EXISTS `fvTotalICMSnfe` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fvTotalICMSnfe`(filial VARCHAR(5),ipTerminal VARCHAR(15), crt varchar(1), arredondar char(1)) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 SELECT 
 if(crt="1", 0, SUM(
 if (arredondar='N', 
 TRUNCATE( 
 ((ratfrete + ratdespesas + ratseguro - ratdesc + IF(pautaICMS=0, total,pautaICMS)) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1) )*(icms/100)
 
  , 2 ), 
 ROUND( 
 ((ratfrete + ratdespesas + ratseguro - ratdesc + IF(pautaICMS=0, total,pautaICMS)) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1) )*(icms/100)
 
  , 2 ))   
 ))
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.cancelado = "N"  
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvTotalIPInfe` */

/*!50003 DROP FUNCTION IF EXISTS `fvTotalIPInfe` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fvTotalIPInfe`(filial VARCHAR(5),ipTerminal VARCHAR(15), crt varchar(5), arredondar CHAR(1)) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN (
 select 
 IF(arredondar='N', 
 TRUNCATE(
 Sum(
 IF( crt ="1", 0.00, IF(aliquotaipi>0 OR vUnidIPI>0,total,0) ) * (aliquotaipi/100)
 )
 , 2), 
 round(
 SUM(
 IF( crt ="1", 0.00, IF(aliquotaipi>0 OR vUnidIPI>0,total,0) ) * (aliquotaipi/100)
 )
 , 2)) 
 
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.cancelado = "N"  
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `f_ticketMedio` */

/*!50003 DROP FUNCTION IF EXISTS `f_ticketMedio` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `f_ticketMedio`( filial VARCHAR(5) ) RETURNS double
    DETERMINISTIC
BEGIN
	
	DECLARE X INT DEFAULT 0;
	DECLARE Y DOUBLE DEFAULT 0;
	
	SET Y = (SELECT (SUM(total) - SUM(ratdesc)) FROM venda WHERE DATA = CURRENT_DATE AND codigofilial = filial AND cancelado = "N");
	SET X = (SELECT COUNT(1)  FROM caixa WHERE dpfinanceiro IN ("venda", "crediario") AND codigofilial = filial AND DATA = CURRENT_DATE );
	RETURN  Y / X ;
	
    END */$$
DELIMITER ;

/* Function  structure for function  `valor` */

/*!50003 DROP FUNCTION IF EXISTS `valor` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `valor`(valor_passado decimal(6,2)) RETURNS varchar(10) CHARSET latin1
    DETERMINISTIC
BEGIN
	
		RETURN CONCAT('R$ ',valor_passado);
	
    END */$$
DELIMITER ;

/* Function  structure for function  `fpesoLiquidoVenda` */

/*!50003 DROP FUNCTION IF EXISTS `fpesoLiquidoVenda` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fpesoLiquidoVenda`(
  filial VARCHAR (5),
  ipTerminal VARCHAR (15)
) RETURNS double
    DETERMINISTIC
BEGIN
  IF (filial = "00001") 
  THEN RETURN 
  (SELECT 
    IFNULL(
      SUM(p.pesoliquido * v.quantidade),
      0
    ) AS bruto 
  FROM
    vendas AS v,
    produtos AS p 
  WHERE v.id = ipTerminal 
    AND v.codigo = p.codigo 
    AND v.codigofilial = filial) ;
  END IF ;
  IF (filial <> "00001") 
  THEN RETURN 
  (SELECT 
    
 IFNULL(
      SUM(p.pesoliquido * v.quantidade),
      0
    ) AS bruto  
  FROM
    vendas AS v,
    produtosfilial AS p 
  WHERE v.id = ipTerminal 
    AND v.codigo = p.codigo 
    AND v.codigofilial = filial 
    AND p.codigofilial = filial) ;
  END IF ;
  RETURN 0 ;
 
END */$$
DELIMITER ;

/* Function  structure for function  `fSaldoCliente` */

/*!50003 DROP FUNCTION IF EXISTS `fSaldoCliente` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fSaldoCliente`(iddocumento INT(6),
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

/* Function  structure for function  `fvBCICMSst` */

/*!50003 DROP FUNCTION IF EXISTS `fvBCICMSst` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fvBCICMSst`(filial VARCHAR(5),ipTerminal VARCHAR(15), idproduto VARCHAR(20), controle int(5), crt varchar(5), arredondar char(1)) RETURNS double
    DETERMINISTIC
BEGIN
 RETURN (
 SELECT 
 if((modalidadeDetBaseCalcICMSst='0' OR modalidadeDetBaseCalcICMSst='3'), vbcICMSST,  
 if((tributacao="10" OR tributacao="70" OR tributacao="110" OR tributacao="210"),
 if(arredondar='N', 
 TRUNCATE( 
 (((v.ratfrete + v.ratdespesas + v.ratseguro + fvIPI(filial, ipterminal, idproduto, controle, crt, arredondar) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst))*(v.mvast/100))+(v.ratfrete + v.ratdespesas + v.ratseguro + fvIPI(filial, ipterminal, idproduto, controle, crt, arredondar) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst)))
 * IF(v.percentualRedBaseCalcICMSST>0, ((100-v.percentualRedBaseCalcICMSST)/100), 1)
 , 2 ), 
 ROUND( 
 (((v.ratfrete + v.ratdespesas + v.ratseguro + fvIPI(filial, ipterminal, idproduto, controle, crt, arredondar) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst))*(v.mvast/100))+(v.ratfrete + v.ratdespesas + v.ratseguro + fvIPI(filial, ipterminal, idproduto, controle, crt, arredondar) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst)))
 * IF(v.percentualRedBaseCalcICMSST>0, ((100-v.percentualRedBaseCalcICMSST)/100), 1)
 , 2 )) 
 , 0))   
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 and v.codigo=idproduto 
 and v.inc=controle 
 AND v.cancelado = "N"  
 AND v.codigofilial=filial);
    END */$$
DELIMITER ;

/* Function  structure for function  `fvICMSst` */

/*!50003 DROP FUNCTION IF EXISTS `fvICMSst` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fvICMSst`(filial VARCHAR(5),ipTerminal VARCHAR(15), codigo VARCHAR(20), controle int(5), crt varchar(5), arredondar char(1) ) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE vICMSst REAL DEFAULT 0;	   
DECLARE cursorvICMSst CURSOR FOR SELECT 
 IF((tributacao="10" OR tributacao="70" OR tributacao="110" OR tributacao="210"),
 if(arredondar='N', 	
 TRUNCATE((fvBCICMSst(filial, ipTerminal, codigo, inc, crt, arredondar)*(icmsst/100))- fvICMS(filial, ipTerminal, codigo, inc, arredondar), 2),
 round((fvBCICMSst(filial, ipTerminal, codigo, inc, crt, arredondar)*(icmsst/100))- fvICMS(filial, ipTerminal, codigo, inc, arredondar), 2)) 
 , 0)
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

/* Function  structure for function  `fvTotalBCICMSSTnfe` */

/*!50003 DROP FUNCTION IF EXISTS `fvTotalBCICMSSTnfe` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fvTotalBCICMSSTnfe`(filial VARCHAR(5), ipTerminal VARCHAR(15), arredondar char(1)) RETURNS double
    DETERMINISTIC
BEGIN
  
 RETURN ( 
 SELECT 
 SUM(
 IF((tributacao="10" OR tributacao="70" OR tributacao="110" OR tributacao="210"),
 if(arredondar='N', 
 TRUNCATE( 
 (((v.ratfrete + v.ratdespesas + v.ratseguro + ( if(v.pautaicmsst=0,v.total,v.pautaicmsst) *v.aliquotaipi/100) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst) )*(v.mvast/100))+(v.ratfrete + v.ratdespesas + v.ratseguro + (IF(v.pautaicmsst=0,v.total,v.pautaicmsst)*v.aliquotaipi/100) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst)))
 * IF(v.percentualRedBaseCalcICMSST>0, ((100-v.percentualRedBaseCalcICMSST)/100), 1)
 , 2 ), 
  ROUND( 
 (((v.ratfrete + v.ratdespesas + v.ratseguro + ( IF(v.pautaicmsst=0,v.total,v.pautaicmsst) *v.aliquotaipi/100) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst) )*(v.mvast/100))+(v.ratfrete + v.ratdespesas + v.ratseguro + (IF(v.pautaicmsst=0,v.total,v.pautaicmsst)*v.aliquotaipi/100) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst)))
 * IF(v.percentualRedBaseCalcICMSST>0, ((100-v.percentualRedBaseCalcICMSST)/100), 1)
 , 2 ))
 
 , 0)  
 )
 FROM  vendas AS v 
 WHERE v.id = ipTerminal 
 AND v.cancelado = "N"  
 AND v.icmsst > 0
 AND v.codigofilial=codigofilial 
 );
 
END */$$
DELIMITER ;

/* Function  structure for function  `fvTotalICMSSTnfe` */

/*!50003 DROP FUNCTION IF EXISTS `fvTotalICMSSTnfe` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `fvTotalICMSSTnfe`(filial VARCHAR(5),ipTerminal VARCHAR(15), arredondar char(1)) RETURNS double
    DETERMINISTIC
BEGIN
DECLARE totalICMSSTnfe REAL DEFAULT 0;	   
 DECLARE cursorTotal CURSOR FOR SELECT 
IF((modalidadeDetBaseCalcICMSst='0' OR modalidadeDetBaseCalcICMSst='3'), SUM(vICMSST), 
  ifnull(
 SUM(
 if(arredondar='N', 
 TRUNCATE(
 IF((v.tributacao=10 OR v.tributacao=70),
 (((((v.ratfrete + v.ratdespesas + v.ratseguro + (IF(v.pautaicmsst=0,v.total,v.pautaicmsst) *v.aliquotaipi/100) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst))*(v.mvast/100))+
 (v.ratfrete + v.ratdespesas + v.ratseguro + (IF(v.pautaicmsst=0,v.total,v.pautaicmsst)*v.aliquotaipi/100) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst)))*
 ((100-v.percentualRedBaseCalcICMSST)/100))*(icmsst/100))-
 (((ratfrete + ratdespesas + ratseguro - ratdesc + IF(v.pautaicmsst=0,v.total,v.pautaicmsst)) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1) )*(icms/100)), 0), 2), 
  round(
 IF((v.tributacao=10 OR v.tributacao=70),
 (((((v.ratfrete + v.ratdespesas + v.ratseguro + (IF(v.pautaicmsst=0,v.total,v.pautaicmsst) *v.aliquotaipi/100) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst))*(v.mvast/100))+
 (v.ratfrete + v.ratdespesas + v.ratseguro + (IF(v.pautaicmsst=0,v.total,v.pautaicmsst)*v.aliquotaipi/100) + IF(v.pautaicmsst=0,v.total,v.pautaicmsst)))*
 ((100-v.percentualRedBaseCalcICMSST)/100))*(icmsst/100))-
 (((ratfrete + ratdespesas + ratseguro - ratdesc + IF(v.pautaicmsst=0,v.total,v.pautaicmsst)) * IF(percentualRedBaseCalcICMS>0, (100-percentualRedBaseCalcICMS)/100, 1) )*(icms/100)), 0), 2)) 
 ), 0))
 FROM  vendas AS v 
 WHERE v.id = ipTerminal
 AND v.cancelado = "N"  
 AND v.codigofilial=filial;
 
 OPEN cursorTotal;
 FETCH cursorTotal INTO totalICMSSTnfe;
 
 return (SELECT IF(totalICMSSTnfe>0, totalICMSSTnfe, 0));
END */$$
DELIMITER ;

/* Function  structure for function  `f_totalOcorrenciasAuditoria` */

/*!50003 DROP FUNCTION IF EXISTS `f_totalOcorrenciasAuditoria` */;
DELIMITER $$

/*!50003 CREATE  FUNCTION `f_totalOcorrenciasAuditoria`( filial VARCHAR(5), tipo VARCHAR(3)  ) RETURNS int(11)
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

/* Procedure structure for procedure `AjustarCamposNulosManual` */

/*!50003 DROP PROCEDURE IF EXISTS  `AjustarCamposNulosManual` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `AjustarCamposNulosManual`(in filial varchar(5))
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
 UPDATE contdav SET ecffabricacao="" WHERE ecffabricacao IS NULL;
 UPDATE contdavos SET ecffabricacao="" WHERE ecffabricacao IS NULL;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AjusteRemovidos` */

/*!50003 DROP PROCEDURE IF EXISTS  `AjusteRemovidos` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `AjusteRemovidos`()
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

/* Procedure structure for procedure `AtualizarContRelGerencial` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarContRelGerencial` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `AtualizarContRelGerencial`(in cooRG varchar(6))
BEGIN
 UPDATE contrelatoriogerencial
SET contrelatoriogerencial.EADDados= MD5(CONCAT( ecffabricacao,coo,gnf, denominacao,DATA,cdc,denominacao))
where coo=cooRG;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarDadosOff` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarDadosOff` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `AtualizarDadosOff`()
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

/* Procedure structure for procedure `calcularCustosEntrada` */

/*!50003 DROP PROCEDURE IF EXISTS  `calcularCustosEntrada` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `calcularCustosEntrada`(IN _numeroEntrada INT,IN _codigo VARCHAR(20),IN empresaLucroReal CHAR(1))
BEGIN
	DECLARE done INT DEFAULT 0;
	DECLARE _bcIcms DECIMAL(15,5) DEFAULT 0;
	DECLARE _vlIcms DECIMAL(15,5) DEFAULT 0;
	DECLARE _vlIPI DECIMAL(15,5) DEFAULT 0;
	DECLARE _bcIcmsSt DECIMAL(15,5) DEFAULT 0;
	DECLARE _vlIcmsSt DECIMAL(15,5) DEFAULT 0;
	DECLARE _bcCofins DECIMAL(15,5) DEFAULT 0;
	DECLARE _bcPis DECIMAL(15,5) DEFAULT 0;
	DECLARE _vlCofins DECIMAL(15,5) DEFAULT 0;
	DECLARE _vlPis DECIMAL(15,5) DEFAULT 0;
	DECLARE _aliquotaPis DECIMAL(15,5) DEFAULT 0;
	DECLARE _aliquotaCofins DECIMAL(15,5) DEFAULT 0;
	DECLARE _tributacaoPIS CHAR(2);
	DECLARE _tributacaoCOFINS CHAR(2);
	DECLARE _custoCalculado DECIMAL(15,5) DEFAULT 0;
	DECLARE _precoVenda DECIMAL(15,5) DEFAULT 0;
	DECLARE _custoCalculadoGerencial DECIMAL(15,5) DEFAULT 0;
	DECLARE _frete DECIMAL(15,5) DEFAULT 0;
	DECLARE _valorProdutoNota DECIMAL(15,5) DEFAULT 0;
	DECLARE _valorDespesas DECIMAL(15,5) DEFAULT 0;
	DECLARE _codigo VARCHAR(100);
	DECLARE _parcentualDesconto DECIMAL(15,5) DEFAULT 0;
	DECLARE _valorDesconto DECIMAL(15,5) DEFAULT 0;
	DECLARE _quantidadeEmbalagem DECIMAL(15,5) DEFAULT 0;
	
	DECLARE _percentualMaremLucro DECIMAL(15,5) DEFAULT 0;
	DECLARE _percentualPis DECIMAL(15,5) DEFAULT 0;
	DECLARE _percentualCofins DECIMAL(15,5) DEFAULT 0;
	DECLARE _percentualDespesas DECIMAL(15,5) DEFAULT 0;
	DECLARE _percentualMargemLucroLiquida DECIMAL(15,5);
	DECLARE _codigoFilial VARCHAR(5);	
	
	DECLARE _ncm VARCHAR(8);
	DECLARE _estado CHAR(2);
	DECLARE _cfop CHAR(5);
	DECLARE _tributacao CHAR(3);
	DECLARE _mvaGerencial DECIMAL(15,5);
	DECLARE _icmsSTGerencial DECIMAL(15,5);
	DECLARE _resultado VARCHAR(100);
	DECLARE _creditarIcms CHAR(1);
	DECLARE _codigoMascaraFiscal INT DEFAULT 0;
	DECLARE itensEntrada CURSOR FOR SELECT codigo FROM entradas WHERE numero = _numeroEntrada;
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
	
	
	IF((SELECT importada FROM moventradas WHERE numero = _numeroEntrada) = 'N')THEN 
	
		SET _resultado = "Entrada não foi importada por XML!";
	
	ELSEIF((SELECT lancada FROM moventradas WHERE numero = _numeroEntrada) = 'X')THEN
	
		SET _resultado = "Entrada ja encerrada!";
	
	ELSEIF((SELECT COUNT(1)  FROM entradas WHERE numero = _numeroEntrada AND codigo = '' OR codigo IS NULL) > 0)THEN
		
		SET _resultado = "Existem produtos não relacionados com o dicionário de codigo!";
	ELSE 
	
		OPEN itensEntrada;
		REPEAT
		FETCH itensEntrada INTO _codigo;
		
			 SELECT codigofilial INTO _codigoFilial FROM moventradas WHERE numero = _numeroEntrada;
			 SELECT cfopentrada INTO _cfop FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			 SELECT UFemitente INTO _estado FROM moventradas WHERE numero = _numeroEntrada;
	
			IF(_codigoFilial = '00001')THEN
				SELECT ncm INTO _ncm FROM produtos WHERE codigo = _codigo LIMIT 1;
				SELECT margemlucro INTO _percentualMaremLucro FROM produtos WHERE codigo = _codigo LIMIT 1;
			ELSE 
		
				SELECT ncm INTO _ncm FROM produtosfilial WHERE codigo = _codigo AND codigoFilial = _codigoFilial LIMIT 1;
				SELECT margemlucro INTO _percentualMaremLucro FROM produtosfilial WHERE codigo = _codigo AND codigoFilial = _codigoFilial LIMIT 1;
			END IF;
					
			
			 IF((SELECT importada FROM moventradas WHERE numero = _numeroEntrada) = 'S')THEN
				
				UPDATE entradas AS e SET e.quantidade = e.quantNF,e.custo = e.custoNf, e.valorUnitario = e.custonf WHERE e.numero = _numeroEntrada AND e.codigo = _codigo;
				 
				IF(_codigoFilial = '00001')THEN
					 UPDATE entradas AS e, produtos AS p SET e.quantidade = p.embalagem, e.custo = (e.custo / p.embalagem), 
					 e.valorunitario = (e.valorunitario /p.embalagem) WHERE e.codigo = p.codigo AND e.codigofilial = p.codigofilial AND e.numero = _numeroEntrada AND e.codigo = _codigo AND p.embalagem > 1;
				ELSE
					 UPDATE entradas AS e, produtosfilial AS p SET e.quantidade = p.embalagem, e.custo = (e.custo / p.embalagem), 
					 e.valorunitario = (e.valorunitario /p.embalagem) WHERE e.codigo = p.codigo AND e.codigofilial = p.codigofilial AND e.numero = _numeroEntrada AND e.codigo = _codigo AND p.embalagem > 1 AND p.codigoFilial = _codigoFilial;
				END IF;
			
			 END IF;
			
			
			 SELECT TRUNCATE(bcicms  / quantidade, 2) INTO _bcIcms FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			 SELECT TRUNCATE(valorIcms  / quantidade,2)INTO _vlIcms FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			 SELECT TRUNCATE((((bcicms  / quantidade) * IPI) /100),2) INTO _vlIPI FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			 SELECT TRUNCATE(bcicmsST  / quantidade ,2) INTO _bcIcmsSt FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			 SELECT TRUNCATE(valoricmsSt / quantidade ,2) INTO _vlIcmsSt FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			 SELECT TRUNCATE(valoroutrasdespesas  / quantidade,2) INTO _valorDespesas FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
	
				
			 SELECT IFNULL(inc,0) INTO _codigoMascaraFiscal FROM mascarafiscal WHERE ncm = _ncm AND cfop = REPLACE(_cfop,'.','') AND estadoorigem = _estado AND codigoFilial = _codigoFilial LIMIT 1;
			 #select _codigoMascaraFiscal;
			
			IF(_codigoMascaraFiscal > 0)THEN
				SELECT pis INTO _aliquotaPis FROM mascarafiscal WHERE inc = _codigoMascaraFiscal;
				SELECT cofins INTO _aliquotaCofins FROM mascarafiscal WHERE inc = _codigoMascaraFiscal;
				SELECT tributacaopis INTO _tributacaoPIS FROM mascarafiscal WHERE inc = _codigoMascaraFiscal;
				SELECT tributacaocofins INTO _tributacaoCOFINS FROM mascarafiscal WHERE inc = _codigoMascaraFiscal;
				UPDATE entradas SET entradas.pis = _aliquotaPis, entradas.cofins = _aliquotaCofins,entradas.cstcofins = _tributacaoCOFINS,entradas.cstpis = _tributacaoPIS, 
				entradas.codigoMascaraFiscal = _codigoMascaraFiscal WHERE numero = _numeroEntrada AND codigo = _codigo;
			 END IF;
		
	
			 IF((SELECT pis FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo) > 0 AND (SELECT IFNULL(bcpis,0) FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo) > 0)THEN
			
				SELECT TRUNCATE(((bcpis  / quantidade) * pis / 100),2) INTO _vlPis FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			
			 ELSEIF((SELECT pis FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo) > 0 AND (SELECT IFNULL(bcpis,0) FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo)=0)THEN
			
				SELECT TRUNCATE(((bcicms  / quantidade) * pis / 100),2) INTO _vlPis FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			 END IF;
	
			
			 IF((SELECT cofins FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo) > 0 AND (SELECT IFNULL(bccofins,0) FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo) > 0)THEN
			 
				SELECT TRUNCATE(((bccofins  / quantidade) * cofins / 100),2) INTO _vlCofins FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			
			 ELSEIF((SELECT cofins FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo) > 0 AND (SELECT IFNULL(bccofins,0) FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo)=0)THEN
			
				SELECT TRUNCATE(((bcicms  / quantidade) * cofins / 100),2) INTO _vlCofins FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo; 
			
			 END IF;
	
	
			 SELECT TRUNCATE((((valorunitario) * frete) / 100),2) INTO _frete  FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			 SELECT TRUNCATE(((descontos / valorProdutos) * 100),2) INTO _parcentualDesconto  FROM moventradas WHERE numero = _numeroEntrada;
			 SELECT TRUNCATE((((valorunitario) * _parcentualDesconto) / 100),2) INTO _valorDesconto FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
				
			 IF(IFNULL((SELECT valorFreteNF FROM entradas WHERE numero =_numeroEntrada AND codigo = _codigo),0) > 0)THEN
				SELECT TRUNCATE((valorFreteNF / quantidade),2) INTO _frete  FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			 END IF;
			
			 IF((SELECT creditar FROM mascarafiscal WHERE ncm = _ncm AND cfop = REPLACE(_cfop,'.','') AND estadoorigem =_estado AND codigoFilial = _codigoFilial) = 'N')THEN
				SET _vlIcms = 0;
			 END IF;
			
	
			 IF((SELECT MID(LPAD(tributacao,3,'0'),2,2) FROM entradas WHERE numero =_numeroEntrada AND codigo = _codigo) = '00')THEN	
				SELECT TRUNCATE((valorunitario + _frete + _vlIPI + _valorDespesas + _vlIcmsSt) - (_valorDesconto + _vlPis + _vlCofins + _vlIcms),2) INTO _custoCalculado 
				FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			
			 ELSEIF((SELECT MID(LPAD(tributacao,3,'0'),2,2) FROM entradas WHERE numero =_numeroEntrada AND codigo = _codigo) = '10')THEN
			
				SELECT TRUNCATE((valorunitario + _frete + _vlIPI + _valorDespesas + _vlIcmsSt) - (_valorDesconto + _vlPis + _vlCofins),2) INTO _custoCalculado 
				FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
			
			 END IF;
		
	
			
			IF((SELECT mascarafiscal FROM configfinanc WHERE codigoFilial = _codigoFilial LIMIT 1) = 'S')THEN
				
				SELECT MID(LPAD(tributacao,3,'0'),2,2) INTO _tributacao FROM entradas WHERE numero = _numeroEntrada AND codigo = _codigo;
				SELECT mvagerencial INTO _mvaGerencial  FROM mascarafiscal WHERE ncm = _ncm AND cfop = REPLACE(_cfop,'.','') AND estadoorigem =_estado AND codigoFilial = _codigoFilial;
				SELECT icmsstgerencial INTO _icmsSTGerencial FROM mascarafiscal WHERE ncm = _ncm AND cfop = REPLACE(_cfop,'.','') AND estadoorigem =_estado AND codigoFilial = _codigoFilial;
					
				SET _custoCalculadoGerencial = ((((((_custoCalculado * _mvaGerencial)/100) + _custoCalculado) * _icmsSTGerencial)/100)+_custoCalculado);
				
				SET _precoVenda = _custoCalculadoGerencial*(_percentualMaremLucro/100)+_custoCalculadoGerencial;
	
			ELSE
				SET _precoVenda = _custoCalculado*(_percentualMaremLucro/100)+_custoCalculado;
			
			END IF;
	
			UPDATE entradas AS e SET 
			e.precovenda = _precoVenda,
			e.margemlucro = _percentualMaremLucro, 
			e.custocalculado = _custoCalculado, 
			e.custoCalculadoGerencial = _custoCalculadoGerencial,
			e.mvagerencial =_mvaGerencial,
			e.aliquotastgerencial =_icmsSTGerencial 
			WHERE e.numero = _numeroEntrada AND e.codigo = _codigo;
	
	
			UPDATE entradas AS e, produtos AS p SET 
			e.PrecoAnterior = p.precovenda
			WHERE e.numero = _numeroEntrada AND e.codigo = p.codigo AND p.codigoFilial = _codigoFilial;
			
	
			SET _resultado = 'TRUE';
	
		UNTIL done
		END REPEAT;
		CLOSE itensEntrada;
	
	END IF;
	
	SELECT _resultado;
	
END */$$
DELIMITER ;

/* Procedure structure for procedure `ExecutarApuracaoFiscal` */

/*!50003 DROP PROCEDURE IF EXISTS  `ExecutarApuracaoFiscal` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `ExecutarApuracaoFiscal`(in dataInicial date,in dataFinal date)
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

/*!50003 CREATE  PROCEDURE `FechamentoDiario`(IN filial VARCHAR(5))
BEGIN
  DECLARE qtdCxAbertos INT DEFAULT 0;  
  DECLARE totalCreditos REAL DEFAULT 0;
  DECLARE totalDebitos REAL DEFAULT 0;  
  DECLARE totalDiferenca REAL DEFAULT 0;
  DECLARE totalCreditosCR REAL DEFAULT 0;
  DECLARE totalDebitosCR REAL DEFAULT 0;
  DECLARE creditosCH REAL DEFAULT 0;
  DECLARE creditosCA REAL DEFAULT 0;
  DECLARE custoFinalPrd REAL DEFAULT 0;
  DECLARE custoMedioFinalPrd REAL DEFAULT 0;
  DECLARE custoFinalRetidos REAL DEFAULT 0;
 
  DECLARE cursorQtdCxAbertos CURSOR FOR SELECT IFNULL( COUNT(1) ,0) FROM caixa WHERE codigofilial=filial;    
  DECLARE cursorCreditos CURSOR FOR SELECT IFNULL(SUM(saldo+dinheiro+cheque+chequefi+chequefipre+chequepre+emprestimoch+trocach+cartao+financiamento+ticket+(recebimento-recebimentobl-recebimentoDC)+receitas),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1);  
  DECLARE cursorDebitos CURSOR FOR SELECT IFNULL(SUM(sangria),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorDiferenca CURSOR FOR SELECT IFNULL(SUM(diferenca),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorCreditosCR CURSOR FOR SELECT IFNULL(SUM(crediario+crediariocr+jurosrenegociacao+crediarioservicosCR),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorDebitosCR CURSOR FOR SELECT IFNULL(SUM(recebimento+juros+jurosrecch+JurosRecCA+descontoreceb+(perdao-jurosperdao)),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorCreditosCH CURSOR FOR SELECT IFNULL(SUM(cheque+chequepre),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorCreditosCA CURSOR FOR SELECT IFNULL(SUM(cartao),0) FROM caixassoma WHERE codigofilial=filial AND DATA>=(SELECT DATA FROM movimento WHERE finalizado=" " AND codigofilial=filial ORDER BY DATA DESC LIMIT 1); 
  DECLARE cursorCustoFinalPrd CURSOR FOR SELECT IFNULL(SUM((quantidade+qtdretida)*custo),0) FROM produtos WHERE codigofilial=filial AND tipo='0 - Produto';
  DECLARE cursorCustoMedioFinalPrd CURSOR FOR SELECT IFNULL(SUM((quantidade+qtdretida)*customedio),0) FROM produtos WHERE codigofilial=filial AND tipo='0 - Produto';
  DECLARE cursorCustoFinalPrdRet CURSOR FOR SELECT IFNULL(SUM(qtdretida*custo),0) FROM produtos WHERE codigofilial=filial AND tipo='0 - Produto';
 
  OPEN cursorQtdCxAbertos; 
  OPEN cursorCreditos;
  OPEN cursorDebitos;
  OPEN cursorDiferenca;
  OPEN cursorCreditosCR;
  OPEN cursorDebitosCR;
  OPEN cursorCreditosCH;
  OPEN cursorCreditosCA;
  OPEN cursorCustoFinalPrd;
  OPEN cursorCustoMedioFinalPrd;
  OPEN cursorCustoFinalPrdRet;
 
  FETCH cursorQtdCxAbertos INTO qtdCxAbertos;     		
  FETCH cursorCreditos INTO totalCreditos; 
  FETCH cursorDebitos INTO totalDebitos; 
  FETCH cursorDiferenca INTO totalDiferenca;
  FETCH cursorCreditosCR INTO totalCreditosCR;
  FETCH cursorDebitosCR INTO totalDebitosCR;
  FETCH cursorCreditosCH INTO creditosCH;
  FETCH cursorCreditosCA INTO creditosCA;
  FETCH cursorCustoFinalPrd INTO custoFinalPrd;
  FETCH cursorCustoMedioFinalPrd INTO custoMedioFinalPrd;
  FETCH cursorCustoFinalPrdRet INTO custoFinalRetidos;
 
 
 IF (qtdCxAbertos=0) THEN
	
	
	    IF  ((SELECT COUNT(1) FROM movimento WHERE DATA=(SELECT IFNULL(MAX(DATA),CURRENT_DATE) FROM caixassoma WHERE codigofilial=filial) AND codigofilial=filial )=0) THEN
	      INSERT INTO movimento (codigofilial,finalizado,DATA) VALUES(filial," ",CURRENT_DATE);
	    END IF;
 
 
		IF((SELECT COUNT(1) FROM movimento WHERE codigofilial=filial AND finalizado=" " AND DATA < CURRENT_DATE) > 0)THEN
	
			UPDATE movimento SET finalizado="X",credito=totalCreditos,debito=totalDebitos,
			saldocaixa=totalCreditos-totalDebitos+totalDiferenca,
			creditocr=totalCreditosCR,
			debitocr=totalDebitosCR,
			creditoch=creditosCH,
			creditoca=creditosCA,
			custofinalestoque=custoFinalPrd,
			customediofinalestoque=custoMedioFinalPrd,
			custofinalretidos=custoFinalRetidos  
			WHERE codigofilial=filial AND finalizado=" " AND DATA < CURRENT_DATE;
		ELSE
		
	
			UPDATE movimento SET finalizado="X",credito=totalCreditos,debito=totalDebitos,
			saldocaixa=totalCreditos-totalDebitos+totalDiferenca,
			creditocr=totalCreditosCR,
			debitocr=totalDebitosCR,
			creditoch=creditosCH,
			creditoca=creditosCA,
			custofinalestoque=custoFinalPrd,
			customediofinalestoque=custoMedioFinalPrd,
			custofinalretidos=custoFinalRetidos  
			WHERE codigofilial=filial AND finalizado=" ";
		END IF;
	
		UPDATE produtos SET descontopromocao=0,
		validadepromoc = CURRENT_DATE,situacao="Normal"
		WHERE validadepromoc < CURRENT_DATE
		AND codigofilial=filial
		AND situacao="Promoção";
		
		UPDATE produtosfilial SET descontopromocao=0,
		validadepromoc = CURRENT_DATE,situacao="Normal"
		WHERE validadepromoc < CURRENT_DATE
		AND codigofilial=filial
		AND situacao="Promoção";
	
	IF ( (SELECT abaterestoqueprevenda FROM configfinanc WHERE codigofilial=filial LIMIT 1) = "S") THEN	
		
		
		UPDATE produtos SET qtdprevenda=0 WHERE codigofilial=filial;
		UPDATE produtosfilial SET qtdprevenda=0 WHERE codigofilial=filial;
		
		UPDATE vendadav,produtos SET
		produtos.qtdprevenda=(SELECT SUM(vendadav.quantidade) FROM vendadav WHERE vendadav.codigo=produtos.codigo AND TO_DAYS(CURRENT_DATE)-TO_DAYS(vendadav.DATA)<(SELECT diasreservaestoquedav FROM configfinanc WHERE codigofilial=filial LIMIT 1) )
		WHERE vendadav.codigofilial= filial
		AND vendadav.codigo=produtos.codigo
		AND produtos.codigofilial=filial
		AND vendadav.codigofilial = produtos.CodigoFilial
		AND (vendadav.coo IS NULL OR vendadav.coo='')
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
	
	
        INSERT INTO crmovclientespagas SELECT * FROM crmovclientes
	WHERE valoratual=0 AND filialpagamento=filial;
	
	DELETE FROM crmovclientes WHERE valoratual=0 AND filialpagamento=filial;	
	
	IF ( (SELECT diasparamudarsituacao FROM configfinanc WHERE codigofilial=filial LIMIT 1)>0) THEN	
		  IF ( (SELECT diasparamudarsituacaoinferior FROM configfinanc WHERE codigofilial=filial LIMIT 1)>0) THEN	
			UPDATE clientes,crmovclientes 
			SET clientes.situacao=(SELECT situacaoautomaticainferior FROM configfinanc WHERE codigofilial=filial LIMIT 1),
			clientes.restritiva=(SELECT restritiva FROM situacaocli WHERE descricao=(SELECT situacaoautomaticainferior FROM configfinanc WHERE codigofilial=filial LIMIT 1))
			WHERE TO_DAYS(CURRENT_DATE)-TO_DAYS(crmovclientes.vencimento)<=(SELECT diasparamudarsituacaoinferior FROM configfinanc WHERE codigofilial=filial LIMIT 1)
			AND TO_DAYS(CURRENT_DATE)-TO_DAYS(crmovclientes.vencimento)>0
			AND clientes.codigo=crmovclientes.codigo
			AND clientes.restritiva<>'S'
			AND clientes.codigofilial=filial;
		  END IF;
		
			UPDATE clientes,crmovclientes 
			SET clientes.situacao=(SELECT situacaoautomatica FROM configfinanc WHERE codigofilial=filial LIMIT 1),
			clientes.restritiva="S"
			WHERE TO_DAYS(CURRENT_DATE)-TO_DAYS(crmovclientes.vencimento)>=(SELECT diasparamudarsituacao FROM configfinanc WHERE codigofilial=filial LIMIT 1)
			AND crmovclientes.valoratual>(SELECT dbclienterestricao FROM configfinanc WHERE codigofilial=filial LIMIT 1)
			AND clientes.codigo=crmovclientes.codigo
			AND clientes.restritiva<>'S'
			AND clientes.codigofilial=filial;
	END IF;	
	UPDATE crmovclientes SET quitado="N"
	WHERE quitado="S" AND valoratual>0
	AND codigofilial=filial;
	
		
 END IF;
 
 
DELETE FROM vendatmp WHERE DATA < CURRENT_DATE;
DELETE FROM restricoes WHERE codigofilial=filial;
CALL AtualizarQdtRegistros();
    END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarVenda` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarVenda` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `FinalizarVenda`(IN doc INT,IN filial VARCHAR(5), IN ipTerminal VARCHAR(15),IN devolucaoNR INT,IN preVenda INT,IN DAVNumero INT)
BEGIN 	
 DECLARE qtdItens INT DEFAULT 0;
 DECLARE vlrSemEncargos REAL DEFAULT 0;
 DECLARE baixarPreVenda CHAR DEFAULT 'N';
 DECLARE valorDescontoItens REAL DEFAULT 0;
 DECLARE valorRestanteAV REAL DEFAULT 0;
 DECLARE valorAbatidoAV REAL DEFAULT 0;  
 
 
 DECLARE cursorQtdItens CURSOR FOR SELECT IFNULL( COUNT(1) ,0) FROM vendas WHERE id=ipTerminal AND cancelado='N';
 DECLARE cursorSencargos CURSOR FOR SELECT IFNULL( SUM(ABS(quantidade)*precooriginal-ratdesc) ,0 ) AS totalsemencargos FROM vendas WHERE id=ipTerminal AND cancelado='N';	
 DECLARE cnfPreVenda CURSOR FOR SELECT abaterestoqueprevenda FROM configfinanc WHERE codigofilial=filial;
 DECLARE cursorDescontoItens CURSOR FOR SELECT IFNULL( SUM(ratdesc),0) FROM vendas WHERE id=ipTerminal AND cancelado='N';
 
 
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
DELETE FROM vendas WHERE documento=doc AND doc>0 ;
DELETE FROM caixas WHERE documento=doc AND doc>0; 
UPDATE vendas SET 
vendas.rateioencargos= TRUNCATE((vendas.total/(SELECT contdocs.Totalbruto FROM contdocs WHERE documento=doc))*(SELECT encargos FROM contdocs WHERE documento=doc),2)  
WHERE vendas.id=ipTerminal;
UPDATE vendas SET vendas.total= vendas.total - vendas.rateioencargos  WHERE vendas.id=ipTerminal AND modelodocfiscal='02'; 
 
UPDATE vendas SET 
vendas.modelodocfiscal=(SELECT contdocs.modeloDOCFiscal FROM contdocs WHERE contdocs.documento=doc),
vendas.serieNF=(SELECT contdocs.serienf FROM contdocs WHERE contdocs.documento=doc),
vendas.subserienf=(SELECT contdocs.subserienf FROM contdocs WHERE contdocs.documento=doc),
vendas.ecffabricacao=(SELECT contdocs.ecffabricacao FROM contdocs WHERE contdocs.documento=doc), 
vendas.codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE contdocs.documento=doc), 
vendas.operador=(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc),
vendas.coo=(SELECT contdocs.ncupomfiscal FROM contdocs WHERE contdocs.documento=doc),
vendas.ccf=(SELECT contdocs.ecfcontadorcupomfiscal FROM contdocs WHERE contdocs.documento=doc),
vendas.DATA=(SELECT contdocs.DATA FROM contdocs WHERE contdocs.documento=doc),
vendas.ratdesc= TRUNCATE((vendas.total/(SELECT contdocs.Totalbruto FROM contdocs WHERE documento=doc))*(SELECT desconto FROM contdocs WHERE documento=doc),2)  
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
 DATA=(SELECT contdocs.DATA FROM contdocs WHERE contdocs.documento=doc),
 ecfnumero=(SELECT contdocs.ecfnumero FROM contdocs WHERE contdocs.documento=doc),
 estornado="N", historico="Cupom Fiscal",tipodoc="1" 
 WHERE caixas.enderecoip=ipTerminal;
 
  
 
 UPDATE caixas SET 
 vrdesconto=(SELECT contdocs.desconto FROM contdocs WHERE contdocs.documento=doc),
 valorservicos=(SELECT contdocs.valorservicos FROM contdocs WHERE contdocs.documento=doc)
 WHERE caixas.enderecoip=ipTerminal LIMIT 1;
 
UPDATE caixas SET descricaopag="Dinheiro" WHERE tipopagamento="DH" AND enderecoip=ipTerminal;
UPDATE caixas SET descricaopag="Cheque" WHERE tipopagamento="CH" AND enderecoip=ipTerminal;
UPDATE caixas SET descricaopag="Cartão" WHERE tipopagamento="CA" AND enderecoip=ipTerminal;
UPDATE caixas SET descricaopag="Crediário" WHERE tipopagamento="CR" AND enderecoip=ipTerminal;
UPDATE caixas SET descricaopag="Ticket" WHERE tipopagamento="TI" AND enderecoip=ipTerminal;
UPDATE caixas SET descricaopag="Compra TI" WHERE tipopagamento="CT" AND enderecoip=ipTerminal;
 
IF ( (SELECT modeloDOCFiscal FROM contdocs WHERE documento=doc LIMIT 1)="02") THEN
UPDATE caixas SET historico="Nota Fiscal",tipodoc="3" WHERE  enderecoip=ipTerminal AND codigofilial=filial; 
END IF; 
IF ( (SELECT COUNT(1) FROM caixas WHERE tipopagamento="CR" AND caixas.enderecoip=ipTerminal) >0) THEN
UPDATE caixas SET historico="Entrada" WHERE  tipopagamento<>"CR" AND caixas.enderecoip=ipTerminal;
END IF;
IF ( (SELECT enderecoentrega FROM contdocs WHERE documento=doc LIMIT 1)<>'') THEN
UPDATE contdocs SET romaneio='S' WHERE documento=doc;
UPDATE vendas SET aentregar='S' WHERE vendas.id=ipTerminal;
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
 vendas.ncm=',@tabelaProduto,'.ncm,
 vendas.nbm=',@tabelaProduto,'.nbm,
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
 @tabelaProduto,'.EADP2relacaomercadoria=md5( concat(',@tabelaProduto,'.codigo,',@tabelaProduto,'.descricao,',@tabelaProduto,'.icms,',@tabelaProduto,'.precovenda,',@tabelaProduto,'.precoatacado,',@tabelaProduto,'.unidade,',@tabelaProduto,'.indicadorarredondamentotruncamento,',@tabelaProduto,'.indicadorproducao,',@tabelaProduto,'.tributacao) ),',
 @tabelaProduto,'.qtdprateleiras=',@tabelaProduto,'.qtdprateleiras-(select IFNULL(sum(vendas.quantidade),0) from vendas where vendas.cancelado="N" and vendas.codigo=',@tabelaProduto,'.codigo and vendas.id=','"',ipTerminal,'")  
 where vendas.codigo=',@tabelaProduto,'.codigo 
 and ',@tabelaProduto,'.codigofilial=vendas.codigofilial  
 AND vendas.cancelado="N"  
 AND ',@tabelaProduto,'.codigofilial=',filial, '
 AND vendas.id=','"',ipTerminal,'"');
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
 AND ',@tabelaProduto,'.indicadorproducao="T"
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
 SET @Acrescimo = CONCAT ('UPDATE vendas,',@tabelaProduto,' SET 
 vendas.quantidadeatualizada=',@tabelaProduto,'.quantidade,
 vendas.acrescimototalitem=(vendas.precooriginal-',@tabelaProduto,'.precovenda)*vendas.quantidade 
 WHERE vendas.id=','"',ipTerminal,'"
 AND vendas.codigo=',@tabelaProduto,'.codigo 
 AND vendas.precooriginal>',@tabelaProduto,'.precovenda
 AND vendas.acrescimototalitem=0
 AND ',@tabelaProduto,'.codigofilial=',filial);
 PREPARE st FROM @Acrescimo;
 EXECUTE st;
 
 
 UPDATE produtosgrade,vendas SET
 produtosgrade.quantidade=produtosgrade.quantidade-(SELECT SUM(vendas.quantidade*vendas.embalagem) FROM vendas WHERE documento=doc AND cancelado='N' AND vendas.grade=produtosgrade.grade)
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
 WHERE cartaofidelidade=(SELECT cartaofidelidade FROM contdocs WHERE documento=doc) AND cartaofidelidade<>'';
IF (devolucaoNR<>0) THEN
	CALL ProcessarDevolucao(doc,filial,ipTerminal,devolucaoNR,(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc)); 
END IF; 
 INSERT INTO movcartoes (codigofilial,documento,cartao,numero,DATA,
 vencimento,valor,operador,dpfinanceiro,encargos,nome,tipo)
 SELECT 	codigofilial,documento,cartao,numerocartao,CURRENT_DATE,vencimento,
 valor,operador,dpfinanceiro,
 
 ((SELECT total FROM contdocs WHERE documento=doc)-VlrSemEncargos)/qtdItens,nome,(SELECT cartoes.tipopagamento FROM cartoes WHERE cartoes.descricao = cartao LIMIT 1)
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
 (SELECT IFNULL( (encargos/nrparcelas) ,0) FROM contdocs WHERE documento=doc),
 dpfinanceiro,
 (SELECT IF(cpf<>'',cpf,cnpj) FROM clientes WHERE codigo=(SELECT codigocliente FROM contdocs WHERE documento=doc))
 FROM caixas WHERE enderecoip=ipTerminal
 AND tipopagamento='CR';
IF ( (SELECT COUNT(1) FROM caixas WHERE enderecoip=ipTerminal AND tipopagamento="CT")>0) THEN  
INSERT INTO movcompratitulos(codigofilial,idCliente,nomecliente,nome,documento,valor,DATA,
vencimento,operador,ip,numeroboleto) 
SELECT codigofilial,(SELECT codigocliente FROM contdocs WHERE documento=doc),
(SELECT contdocs.nome FROM contdocs WHERE documento=doc),NomeCheque,doc,valor,CURRENT_DATE,
vencimento,operador,ipTerminal,cheque FROM caixas WHERE enderecoip=ipTerminal
 AND tipopagamento='CT';
END IF;
 
 
 IF ( (SELECT COUNT(1) FROM venda WHERE documento=doc AND numerodevolucao=0)=0) THEN 
 INSERT INTO venda (codigofilial,operador,DATA,codigo,produto,quantidade,preco,custo,precooriginal,Descontoperc,id,descontovalor,total,vendedor,nrcontrole,documento,grupo,subgrupo,comissao,ratdesc,rateioencargos,situacao,customedio,Ecfnumero,fornecedor,fabricante,NotaFiscal,icms,classe,secao,lote,tributacao,aentregar,quantidadeanterior,quantidadeatualizada,codigofiscal,customedioanterior,codigocliente,numerodevolucao,codigobarras,aliquotaipi,unidade,embalagem,grade,romaneio,tipo,cofins,pis,cstcofins,cstpis, despesasacessorias,percentualRedBaseCalcICMS,modelodocfiscal,serienf,subserienf,ecffabricacao,coo,acrescimototalitem,cancelado,eaddados,ccf,idfornecedor,icmsst, mvast, cfop,dataalteracao,horaalteracao,tipoalteracao,ncm,canceladoECF,vendaatacado) 
 SELECT codigofilial,operador,DATA,codigo,produto,quantidade,preco,custo,precooriginal,Descontoperc,id,descontovalor,total,vendedor,nrcontrole,documento,grupo,subgrupo,comissao,ratdesc,rateioencargos,situacao,customedio,Ecfnumero,fornecedor,fabricante,NotaFiscal,icms,classe,secao,lote,tributacao,aentregar,quantidadeanterior,quantidadeatualizada,codigofiscal,customedioanterior,codigocliente,numerodevolucao,codigobarras,aliquotaipi,unidade,embalagem,grade,romaneio,tipo,cofins,pis,cstcofins,cstpis,despesasacessorias,percentualRedBaseCalcICMS,modelodocfiscal ,serienf,subserienf,ecffabricacao,coo,acrescimototalitem,cancelado,eaddados,ccf,idfornecedor,icmsst, mvast, cfop,dataalteracao,horaalteracao,tipoalteracao,ncm,canceladoECF,vendaatacado
 FROM vendas
 WHERE id=ipTerminal;
 END IF;
 
 UPDATE venda SET 
venda.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")))
WHERE documento=doc;
 
 IF ( (SELECT COUNT(1) FROM caixa WHERE documento=doc)=0) THEN 
 
 INSERT INTO caixa (horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,DATA,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch,ecfnumero,ecffabricacao,ecfmodelo,coo,ccf,gnf,descricaopag,tipodoc)  
 SELECT horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,DATA,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch,ecfnumero,ecffabricacao,ecfmodelo,coo,ccf,gnf,descricaopag,tipodoc
 FROM caixas 
 WHERE enderecoip=ipTerminal;
 END IF;
 
 
  UPDATE caixa SET 
 eaddados=MD5(CONCAT(ecffabricacao,coo,ccf,gnf,ecfmodelo,valor,tipopagamento,DATA,tipodoc)),
 estornado='N'
 WHERE  caixa.documento=doc;
 
 UPDATE contdocs SET concluido='S',devolucaovenda=IFNULL((SELECT SUM(valor) FROM caixas WHERE enderecoip=ipTerminal AND tipopagamento="DV"), 0),
 EADr06=MD5(CONCAT(ecffabricacao, ncupomfiscal, IFNULL(contadornaofiscalGNF,""), IFNULL(contadordebitocreditoCDC,""), DATA, COOGNF, tipopagamento, IFNULL(ecfcontadorcupomfiscal,""), ecftotalliquido, estornado,ecfMFadicional,ecfmodelo,IFNULL(ecfcontadorcupomfiscal,""),Totalbruto,desconto,encargos,IFNULL(ecfConsumidor,""),IFNULL(ecfCPFCNPJconsumidor,""))),
 EADRegistroDAV=MD5(CONCAT(IFNULL(ncupomfiscal,""),davnumero,DATA,total)),
 estoqueatualizado='S' 
 WHERE documento=doc;
 
 UPDATE contdocs SET DATA=CURRENT_DATE WHERE DATA="0001-01-01"
 AND documento=doc;
 
 
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
 ncupomfiscal=(SELECT ncupomfiscal FROM contdocs WHERE documento=doc),
 ecfcontadorcupomfiscal=(SELECT ecfcontadorcupomfiscal FROM contdocs WHERE documento=doc),
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,numeroDAVFilial,DATA,valor,IFNULL(numeroECF,"001"),IFNULL(contadorRGECF,""),IFNULL(cliente,""),IFNULL(ecfCPFCNPJconsumidor,""))) 
 WHERE numeroDAVfilial=DAVNumero AND codigofilial=filial AND finalizada='N';
 
 UPDATE contdocs SET 
 contdocs.codigocliente = (SELECT codigocliente FROM contdav WHERE numeroDAVfilial = DAVNumero AND codigofilial = filial LIMIT 1),
 contdocs.nome = (SELECT cliente FROM contdav WHERE numeroDAVfilial = DAVNumero AND codigofilial = filial LIMIT 1)
 WHERE documento =doc;	
 
 UPDATE contdavos SET finalizada='S',
 datafinalizacao=(SELECT contdocs.DATA FROM contdocs WHERE contdocs.documento=doc),
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,numeroDAVFilial,DATA,valor,IFNULL(numeroECF,"001"),IFNULL(contadorRGECF,""),IFNULL(cliente,""),IFNULL(ecfCPFCNPJconsumidor,""))),
 ncupomfiscal=(SELECT ncupomfiscal FROM contdocs WHERE documento=doc),
 ecffabricacao=(SELECT contdocs.ecffabricacao FROM contdocs WHERE contdocs.documento=doc),
 marca=(SELECT contdocs.ecfmarca FROM contdocs WHERE contdocs.documento=doc),
 modelo=(SELECT contdocs.ecfmodelo FROM contdocs WHERE contdocs.documento=doc),
 ecfcontadorcupomfiscal=(SELECT ecfcontadorcupomfiscal FROM contdocs WHERE documento=doc)
 WHERE numeroDAVfilial=DAVNumero AND codigofilial=filial;
 
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
UPDATE vendadav SET coo="" WHERE documento=DAVNumero AND coo IS NULL;
UPDATE vendadav SET ccf="" WHERE documento=DAVNumero AND ccf IS NULL;
UPDATE vendadav SET ecffabricacao="" WHERE documento=DAVNumero AND ecffabricacao IS NULL;
 INSERT INTO vendadav (codigofilial,operador,DATA,codigo,produto,quantidade,preco,custo,precooriginal,Descontoperc,id,descontovalor,total,vendedor,nrcontrole,documento,grupo,subgrupo,comissao,ratdesc,rateioencargos,situacao,customedio,Ecfnumero,fornecedor,fabricante,NotaFiscal,icms,classe,secao,lote,tributacao,aentregar,quantidadeanterior,quantidadeatualizada,codigofiscal,customedioanterior,codigocliente,numerodevolucao,codigobarras,aliquotaipi,unidade,embalagem,grade,romaneio,tipo,cofins,pis,cstcofins,cstpis, despesasacessorias,percentualRedBaseCalcICMS,modelodocfiscal,serienf,subserienf,ecffabricacao,coo,acrescimototalitem,cancelado,eaddados,ccf,idfornecedor,icmsst, mvast, cfop,dataalteracao,horaalteracao,tipoalteracao) 
 SELECT codigofilial,operador,DATA,codigo,produto,quantidade,preco,custo,precooriginal,Descontoperc,id,descontovalor,total,vendedor,nrcontrole,DAVNumero,grupo,subgrupo,comissao,ratdesc,rateioencargos,situacao,customedio,Ecfnumero,fornecedor,fabricante,NotaFiscal,icms,classe,secao,lote,tributacao,aentregar,quantidadeanterior,quantidadeatualizada,codigofiscal,customedioanterior,codigocliente,numerodevolucao,codigobarras,aliquotaipi,unidade,embalagem,grade,romaneio,tipo,cofins,pis,cstcofins,cstpis,despesasacessorias,percentualRedBaseCalcICMS,modelodocfiscal ,serienf,subserienf,ecffabricacao,coo,acrescimototalitem,cancelado,eaddados,ccf,idfornecedor,icmsst, mvast, cfop,dataalteracao,horaalteracao,tipoalteracao
 FROM vendas
 WHERE id=ipTerminal AND vendas.tipoalteracao IN("A","E","I");
 
UPDATE vendadav SET 
vendadav.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,""),IFNULL(horaalteracao,""),IFNULL(tipoalteracao,"")))
WHERE documento=DAVNumero; 
 
 END IF; 
 
CALL finalizarAv(ipTerminal,doc);
 
 DELETE FROM caixas WHERE enderecoip=ipTerminal AND codigofilial=filial AND operador = (SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc)
 AND dpfinanceiro = 'Venda';
 DELETE FROM vendas WHERE id=ipTerminal AND codigofilial=filial;
 
 IF ((SELECT totalbrutocomencargos FROM configfinanc WHERE codigofilial=filial LIMIT 1)="S") THEN
 UPDATE contdocs SET totalbruto=total WHERE documento=doc LIMIT 1;
 END IF;
 
UPDATE contdav AS c SET c.ecffabricacao = (SELECT ecffabricacao FROM contdocs WHERE documento = doc LIMIT 1) WHERE c.numeroDAVFilial = DAVNumero;
UPDATE contdav AS c SET c.marca = (SELECT marca FROM contdocs WHERE documento = doc LIMIT 1) WHERE c.numeroDAVFilial = DAVNumero;
UPDATE contdav AS c SET c.modelo = (SELECT modelo FROM contdocs WHERE documento = doc LIMIT 1) WHERE c.numeroDAVFilial = DAVNumero;
UPDATE contdav AS c SET c.tipoECF = (SELECT tipoECF FROM contdocs WHERE documento = doc LIMIT 1) WHERE c.numeroDAVFilial = DAVNumero;
UPDATE contdav AS c SET c.mfAdicional = 0 WHERE c.numeroDAVFilial = DAVNumero;
CALL AtualizarQdtRegistros();	
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FluxoCaixa` */

/*!50003 DROP PROCEDURE IF EXISTS  `FluxoCaixa` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `FluxoCaixa`(
  IN idOperador VARCHAR (10),  
  IN filial VARCHAR (5))
BEGIN
SET @operador=idOperador;
SET @filialFluxo = filial;
SET @horafechamento = CURRENT_TIME;
SET @dataCaixa = IFNULL((SELECT MAX(DATA) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial LIMIT 1), CURRENT_DATE); 
SET @horaabertura=IFNULL((SELECT horaabertura FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND (tipopagamento="SI" OR dpfinanceiro="Saldo Inicial")  LIMIT 1), "08:00:01"); 
SET @saldo =(SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND dpfinanceiro="Saldo Inicial" AND tipopagamento <> "SU" AND DATA = @dataCaixa);
SET @dataMovimento = (SELECT MAX(DATA) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND DATA = @dataCaixa LIMIT 1);
SET @dinheiro = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="DH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND dpfinanceiro<>"Recebimento CH"
  AND DATA = @dataCaixa);
 
SET @entradadh = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="DH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada" 
  AND DATA = @dataCaixa);
 
 SET @entradach = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada" 
  AND DATA = @dataCaixa);
 
 SET @entradaca = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="CA" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada" 
  AND DATA = @dataCaixa);
 
 SET @entradafi = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="FI" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada"
  AND DATA = @dataCaixa);
 
 SET @cheque = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
  AND tipopagamento="CH" 
  AND dpfinanceiro<>"Crediario" 
  AND dpfinanceiro<>"Troca CH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND dpfinanceiro<>"Emprestimo CH" 
   AND vencimento = @dataCaixa); 
 
 SET @chequepre = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
  AND tipopagamento="CH" 
  AND dpfinanceiro<>"Crediario" 
  AND dpfinanceiro<>"Troca CH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
   AND vencimento > @dataCaixa);
 
 SET @cartao = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
 AND tipopagamento="CA" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND DATA = @dataCaixa);
 
   SET @recebimento = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'
  AND DATA = @dataCaixa);
 
 SET @recebimentodh = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento="DH"
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'
  AND DATA = @dataCaixa); 
 
 SET @recebimentoch = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento="CH"
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'
  AND DATA = @dataCaixa);  
 
 SET @recebimentoca = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento="CA"
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'
  AND DATA = @dataCaixa); 
 
SET @crediario = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento="CR"
  AND dpfinanceiro="Venda"
  AND DATA = @dataCaixa);
 
 SET @sangria = (SELECT IFNULL(SUM(valor),0) FROM movdespesas WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
AND DATA>=(SELECT MAX(DATA) FROM caixa  WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND DATA = @dataCaixa) 
  AND encerrado<>'S'
  AND sangria='S');
 
 SET @vendas = (SELECT IFNULL(SUM(total-ratdesc+rateioencargos),0) FROM venda WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
 AND cancelado="N"
 AND DATA = @dataCaixa);
 
 SET @custos = (SELECT IFNULL(SUM(ABS( (quantidade *embalagem)  )*custo),0) FROM venda WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
 AND cancelado="N"
 AND DATA = @dataCaixa);
 
 SET @juros = (SELECT IF(SUM(vrjuros-vrdesconto)>=0,SUM(vrjuros),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND dpfinanceiro LIKE 'Receb%'
AND DATA = @dataCaixa);
SET @devolucao = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento="DV" AND `dpfinanceiro` <> 'Venda'
 AND DATA = @dataCaixa);
 
SET @renegociacao = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento="RN"
 AND DATA = @dataCaixa);
SET @perdao = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento="PD"
 AND DATA = @dataCaixa);
 
 SET @descontovenda = (SELECT IFNULL(SUM(ratdesc+descontovalor),0) FROM venda WHERE  operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
 AND cancelado="N"
 AND DATA = @dataCaixa);
 
 
 SET @descontoreceb = (SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND dpfinanceiro LIKE 'Receb%'
AND vrdesconto>vrjuros
AND estornado <> 'N'
AND DATA = @dataCaixa);
SET @descontorecebjuros = ( SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
 AND dpfinanceiro LIKE 'Receb%'
 AND (vrdesconto>0 OR vrdesconto>vrjuros) 
 AND dpfinanceiro<>'Recebimento s/j' 
 AND DATA = @dataCaixa);
 
 
 SET @crediariocr = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento="CR"
  AND dpfinanceiro="Crediario"
 AND DATA = @dataCaixa);
 
 
 SET @jurosperdao = ( SELECT IFNULL( IF(SUM(vrjuros-vrdesconto)>=0,SUM(vrjuros-vrdesconto),0),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
AND tipopagamento="PD" 
AND DATA = @dataCaixa);
SET @jurosrenegociacao = (SELECT IFNULL( IF(SUM(vrjuros-vrdesconto)>=0,SUM(vrjuros-vrdesconto),0),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
AND tipopagamento="RN"
AND DATA = @dataCaixa);
SET @encargos = (SELECT IFNULL(SUM(rateioencargos),0) FROM venda WHERE  operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
 AND cancelado="N"
 AND DATA = @dataCaixa);
SET @devolucaocr = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento="DV"
 AND dpfinanceiro LIKE 'Receb%'
 AND DATA = @dataCaixa);
 
 SET @devolucaoprd = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="DV" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND dpfinanceiro<>"Sangria"
 AND DATA = @dataCaixa);
 
 
 SET @jurosrecch = (SELECT IFNULL(SUM(jurosch),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD' 
  AND dpfinanceiro LIKE 'Receb%'
 AND DATA = @dataCaixa);
 
SET @encargosrecebidos = (SELECT IFNULL(SUM(encargos),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD' 
  AND dpfinanceiro LIKE 'Receb%'
 AND DATA = @dataCaixa);
 
 SET @diferenca = (SELECT IFNULL(SUM(valor),0)FROM caixa  WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
   AND dpfinanceiro="Diferenca"
 AND DATA = @dataCaixa);
 
 SET @ocorrencia = "";
 
 SET @chequefi = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Crediario"
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND DATA = @dataCaixa );
 
 SET @chequefipre = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Crediario"
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND DATA > @dataCaixa);
 
 SET @financiamento = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
 AND tipopagamento="FN" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND dpfinanceiro = "Crediario"
  AND DATA = @dataCaixa);
 
 SET @financeira = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
 AND  (tipopagamento="FI" OR tipopagamento="FN")
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND dpfinanceiro="Venda"
  AND DATA = @dataCaixa);
 
 SET @qtdcupons = (SELECT COUNT(1) AS quantidade FROM contdocs WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
  AND dpfinanceiro="Venda"  
 AND DATA=(SELECT DATA FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND DATA = @dataCaixa LIMIT 1) );
 
 SET @suprimento = (SELECT IFNULL(SUM(valor),0) AS suprimento FROM caixa WHERE operador=idOperador AND codigofilial=filial
 AND tipopagamento="SU"
 AND DATA = @dataCaixa);
 
 SET @dpfinanceiro = "Venda";
 
 SET @receitas = (SELECT IFNULL(SUM(valor),0) AS receita FROM movreceitas  WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
  AND encerrado<>'S'
 AND DATA>=(SELECT MAX(DATA) FROM caixa  WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND DATA = @dataCaixa) 
  AND sangria='S');
 
 SET @recebimentobl = (SELECT IFNULL(SUM(valor),0) FROM caixa  WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND tipopagamento="BL"
AND dpfinanceiro LIKE 'Receb%'
AND DATA = @dataCaixa);
SET @recebimentodc = (SELECT IFNULL(SUM(valor),0) FROM caixa  WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND tipopagamento="DC"
AND dpfinanceiro LIKE 'Receb%'
AND DATA = @dataCaixa);
SET @recebimentoAV = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND tipopagamento="AV"
AND dpfinanceiro LIKE 'Receb%'
AND DATA = @dataCaixa);
  SET @emprestimodh = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND dpfinanceiro="Emprestimo DH"
  AND (tipopagamento="CR" OR tipopagamento="FN")
 AND DATA = @dataCaixa);
 
 SET @emprestimoch = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND dpfinanceiro="Emprestimo CH"
  AND (tipopagamento="CH" OR tipopagamento="FN")
 AND DATA = @dataCaixa);
 
 SET @comprati = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="CT" 
  AND dpfinanceiro="Compra TI"
 AND DATA = @dataCaixa);
 
  SET @trocach = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Troca CH"
 AND DATA = @dataCaixa);
  
 
 SET @ticket = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial AND tipopagamento="TI" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
 AND DATA = @dataCaixa);
 
 SET @valorservicos = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial  
  AND dpfinanceiro="Servicos"
 AND DATA = @dataCaixa);
 
 SET @descontoservicos = (SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial  
  AND dpfinanceiro="Servicos"
 AND DATA = @dataCaixa);
 
 SET @descontocapitalrn = (SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND tipopagamento="RN"
 AND DATA = @dataCaixa);
 
 
 SET @crediarioservicosCR = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
  AND dpfinanceiro="Servicos"
  AND tipopagamento="CR"
 AND DATA = @dataCaixa);
 
 SET @jurosrecca = (SELECT IFNULL(SUM(jurosca),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND dpfinanceiro LIKE 'Receb%'
AND DATA = @dataCaixa);
SET @creditorecebimentoAV = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
AND tipopagamento="AV"
AND dpfinanceiro LIKE 'CreditoAV%'
AND DATA = @dataCaixa);
SET @vendaAV = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial
AND tipopagamento="AV"
AND dpfinanceiro='Venda'
AND DATA = @dataCaixa);
SET @saldoFinal = (@saldo+@suprimento+@dinheiro+@cheque+@chequefi+@financeira+@chequepre+@emprestimoch+@trocach+@cartao+@ticket+@vendaAV+(@recebimento -@recebimentoAV)+ @receitas - @sangria )+@diferenca;
SET @saldoLiquidoEspecie = (@saldo+@suprimento+@dinheiro+@cheque+@chequepre+@RecebimentoDH+@RecebimentoCh+@Receitas)-@sangria+@diferenca;
 
 
 SELECT @operador AS operador,@filialFluxo AS codigofilial,IFNULL(@dataMovimento,CURRENT_DATE) AS dataMovimento, @horaabertura AS horaabertura,(@saldo + @suprimento) AS saldo,@dinheiro AS dinheiro,@entradadh AS entradaDH,@entradach AS entradaCH,@entradaca AS entradaCA,
 @entradafi AS entradaFI,@cheque AS cheque,@chequepre AS chequePre,@cartao AS cartao,@recebimento AS recebimento,@recebimentodh AS recebimentoDH,
 @recebimentoch AS recebimentoCH,@recebimentoca AS recebimentoCA,@crediario AS crediario,@sangria AS sangria,@vendas AS totalVenda,
 @custos AS totalCustos,@juros AS juros,@renegociacao AS renegociacao,@perdao AS perdao,@descontovenda AS descontoTotalVenda,
 @descontoreceb AS descontoRecebimento,@descontorecebjuros AS descontoRecebimentoJuros,@crediariocr AS crediariocr,@jurosperdao AS jurosPerdao,
 @jurosrenegociacao AS jurosRenegociacao,@encargos AS encargos,@devolucao AS devolucao,@devolucaocr AS devolucaoRec,@devolucaoprd AS devolucaoVenda,
 @jurosrecch AS jurosRecebimentoCH,@encargosrecebidos AS encargosRecebimento,@diferenca AS diferenca,@ocorrencia AS ocorrencia,
 @chequefi AS chequeFI,@chequefipre AS chequeFIPre,@financiamento AS financiamento,@financeira AS financeira,@qtdcupons AS qtdCupons,
 @suprimento AS suprimento,@dpfinanceiro AS dpfinanceiro,@receitas AS receitas,@recebimentobl AS recebimentoBL,@recebimentodc AS recebimentoDC,
 @emprestimodh AS emprestimoDH,@emprestimoch AS emprestimoCH,@comprati AS compraTI,@trocach AS trocaCH,@ticket AS ticket,@valorservicos AS valorServicos,
 @descontoservicos AS descontoServicos,@descontocapitalrn AS descontoCapitalRN,@crediarioservicosCR AS crediarioServicos,@jurosrecca AS jurosRecebimentoCA,
 @creditorecebimentoAV AS creditoRecebimentoAV,@recebimentoAV AS recebimentoAV,@vendaAV AS vendaAV, @saldoFinal AS saldoFinal,@saldoLiquidoEspecie AS saldoFinalLiquidoEspecie;
 
END */$$
DELIMITER ;

/* Procedure structure for procedure `GerarNFe` */

/*!50003 DROP PROCEDURE IF EXISTS  `GerarNFe` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `GerarNFe`( IN filial VARCHAR(5),
IN ipTerminal VARCHAR(15),
IN criarNF CHAR(1),
IN NFeOrigem BIGINT(9),
IN tipoNFe CHAR(1),
IN tipoEmissaoNFe CHAR(1),
IN modeloNFe VARCHAR(2),
IN finalidadeNFe CHAR(1),
IN situacaoNFe VARCHAR(2),
IN naturezaOperacaoNFe VARCHAR(60),
IN gerarICMS CHAR(1),
IN doc INT(9),
IN descontoNFe DECIMAL(9,2),
IN freteNFe DECIMAL(10,2),
IN seguroNFe DECIMAL(10,2),
IN despesasNFe DECIMAL(10,2),
IN marcavolume VARCHAR(20), 
IN idCliente INT(6),
IN idFornecedorNFe INT(6),
IN idTransportadoraNFe INT(6),
IN idVeiculoNFe INT(5),
IN idInfoComplementarNFe INT(4), 
IN cfopTransporteNFE VARCHAR(5), 
IN serieNFe VARCHAR(3),
IN operadorNFe VARCHAR(10),
IN cfopNFe VARCHAR(5),
IN dadosComplementarNFe TEXT,
IN tipoFreteNFe CHAR(1),
IN volumeNFe INT(1),
IN qtdVolumeNFe INT(3),
IN especieVolumeNFe VARCHAR(20),
IN chaveAcessoRefNFe VARCHAR(44),
IN colocarDataHoraNFe CHAR(1),
IN indPag CHAR(1),
IN NFeEntradaAdEstoque CHAR(1),
IN crtNFE CHAR(1),
IN TotalPesoBrutoNFe DECIMAL(10,2),
IN TotalPesoLiquidoNFe DECIMAL(10,2),
IN DataSaida DATE, 
IN HoraSaida CHAR(8), 
IN arredondar CHAR(8),
IN tpIntegracao CHAR(1), 
 OUT numeroNFe BIGINT(9) )
BEGIN
DECLARE tentativas INT DEFAULT 0;
DECLARE totalLiquidoNFe REAL DEFAULT 0;
DECLARE totalBrutoNFe REAL DEFAULT 0;
DECLARE totalDescontoNFe REAL DEFAULT 0;
DECLARE totalAcrescimoNFe REAL DEFAULT 0;
DECLARE baseCalculoICMSNFe REAL DEFAULT 0;
DECLARE totalICMSNFe REAL DEFAULT 0;
DECLARE baseCalculoICMSSTNFe REAL DEFAULT 0;
DECLARE baseCalculoIPINFe REAL DEFAULT 0;
DECLARE totalICMSSTNFe REAL DEFAULT 0;
DECLARE totalFreteNFe REAL DEFAULT 0;
DECLARE totalICMSFreteNFe REAL DEFAULT 0;
DECLARE totalSeguroNFe REAL DEFAULT 0;
DECLARE totalDespesasNFe REAL DEFAULT 0;
DECLARE totalIPINFe REAL DEFAULT 0;
DECLARE pesoBrutoNFe REAL DEFAULT 0;
DECLARE pesoLiquidoNFe REAL DEFAULT 0;
DECLARE baseCalculoPISNFe REAL DEFAULT 0;
DECLARE baseCalculoCOFINSNFe REAL DEFAULT 0;
DECLARE totalPISNFe REAL DEFAULT 0;
DECLARE totalCOFINSNFe REAL DEFAULT 0;
DECLARE totalCSLLNFe REAL DEFAULT 0;
DECLARE baseCalculoICMSSTNFeSIMPLES REAL DEFAULT 0;
DECLARE totalICMSSTNFeSIMPLES REAL DEFAULT 0;
DECLARE done INT DEFAULT FALSE;
DECLARE docCupom INT DEFAULT 0;
 
DECLARE cursorTotalBruto CURSOR FOR SELECT IFNULL(TRUNCATE(SUM(preco*quantidade), 2),0) FROM vendas WHERE id=ipTerminal AND cancelado='N';
DECLARE cursorDesconto CURSOR FOR SELECT IFNULL( SUM(ratdesc) ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N';	
DECLARE cursorIPI CURSOR FOR SELECT IFNULL( SUM(ROUND(((((`vendas`.`total` + `vendas`.`ratfrete` + `vendas`.`ratdespesas` + `vendas`.`ratseguro` ) - `vendas`.`ratdesc`) * `vendas`.`aliquotaipi`) / 100),2))  ,0 ) FROM vendas WHERE id=ipTerminal AND cancelado='N';
DECLARE cursorPesoBruto CURSOR FOR SELECT fpesoBrutoVenda(filial,ipTerminal);
DECLARE cursorPesoLiq CURSOR FOR SELECT fpesoLiquidoVenda(filial,ipTerminal);
DECLARE cursorBCICMS CURSOR FOR SELECT fvTotalBCICMSnfe(filial,ipTerminal, crtNFE, arredondar);
DECLARE cursortotalICMS CURSOR FOR SELECT fvTotalICMSnfe(filial,ipTerminal, crtNFE, arredondar);
DECLARE cursorBCICMSST CURSOR FOR SELECT IFNULL(fvTotalBCICMSSTnfe(filial,ipTerminal, arredondar), 0);
DECLARE cursortotalICMSST CURSOR FOR SELECT fvTotalICMSSTnfe(filial,ipTerminal, arredondar);
DECLARE cursorBCICMSSTSIMPLES CURSOR FOR SELECT IFNULL (SUM(ROUND(( (`vendas`.`total` -((`vendas`.`total` * `vendas`.`percentualRedBaseCalcICMSST`) / 100)   ) + ( (`vendas`.`total` - ((`vendas`.`total` * `vendas`.`percentualRedBaseCalcICMSST`) / 100) ) * (`vendas`.`mvast` / 100))),2)),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND icmsst>0 AND (tributacao="10" OR tributacao="70");
DECLARE cursortotalICMSSTSIMPLES CURSOR FOR SELECT IFNULL( TRUNCATE( (((SUM(ROUND(( (`vendas`.`total` - ((`vendas`.`total` * `vendas`.`percentualRedBaseCalcICMSST`) / 100) ) + ( (`vendas`.`total` - ((`vendas`.`total` * `vendas`.`percentualRedBaseCalcICMSST`) / 100) ) * (`vendas`.`mvast` / 100))),2)) * `vendas`.`icmsst`) / 100) - SUM(ROUND(((`vendas`.`total` * `vendas`.`icms`) / 100),2))),2),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND icmsst>0 AND (tributacao="10" OR tributacao="70");
DECLARE cursorBCPIS CURSOR FOR SELECT IFNULL (SUM(total),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND pis>0;
DECLARE cursorBCCOFINS CURSOR FOR SELECT IFNULL (SUM(total),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND cofins>0;
DECLARE cursortotalPIS CURSOR FOR SELECT IFNULL (SUM(ROUND((`vendas`.`total` * (`vendas`.`pis` / 100)),2)),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND pis>0;
DECLARE cursortotalCOFINS CURSOR FOR SELECT IFNULL (SUM(ROUND((`vendas`.`total` * (`vendas`.`cofins` / 100)),2)),0) FROM vendas WHERE id=ipTerminal AND cancelado='N' AND cofins>0;
DECLARE cursorCupons CURSOR FOR SELECT documento FROM vendas WHERE id=ipTerminal AND cancelado='N' AND (vendas.coo<>'' AND vendas.coo IS NOT NULL);
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
OPEN cursorIPI;
OPEN cursorPesoBruto;
OPEN cursorPesoLiq;
OPEN cursorBCICMS;
OPEN cursortotalICMS;
OPEN cursorBCICMSST;
OPEN cursortotalICMSST;
OPEN cursorBCICMSSTSIMPLES;
OPEN cursortotalICMSSTSIMPLES;
OPEN cursorBCPIS;
OPEN cursorBCCOFINS;
OPEN cursortotalPIS;
OPEN cursortotalCOFINS;
FETCH cursorDesconto INTO totalDescontoNFe;
FETCH cursorIPI INTO totalIPINFe;
FETCH cursorPesoBruto INTO pesoBrutoNFe;
FETCH cursorPesoLiq INTO pesoLiquidoNFe;
FETCH cursorBCICMS INTO baseCalculoICMSNFe;
FETCH cursortotalICMS INTO totalICMSNFe;
FETCH cursorBCICMSST INTO baseCalculoICMSSTNFe;
FETCH cursortotalICMSST INTO totalICMSSTNFe;
FETCH cursorBCICMSSTSIMPLES INTO baseCalculoICMSSTNFeSIMPLES;
FETCH cursortotalICMSSTSIMPLES INTO totalICMSSTNFeSIMPLES;
FETCH cursorBCPIS INTO baseCalculoPISNFe;
FETCH cursorBCCOFINS INTO baseCalculoCOFINSNFe;
FETCH cursortotalPIS INTO totalpisNFe;
FETCH cursortotalCOFINS INTO totalCOFINSNFe;
 SET @tabelaProduto='produtos';
 IF (filial<>'00001') THEN
 SET @tabelaProduto='produtosfilial';
 END IF;
 
IF ( crtNFE="1") THEN
UPDATE vendas SET icms=0 
WHERE id=ipTerminal;
SET baseCalculoPISNFe = 0;
SET baseCalculoCOFINSNFe = 0;
SET totalPISNFe = 0;
SET totalCOFINSNFe = 0;
SET baseCalculoICMSNFe = 0;
SET totalICMSNFe = 0;
SET baseCalculoICMSSTNFe = baseCalculoICMSSTNFeSIMPLES ;
SET totalICMSSTNFe = totalICMSSTNFeSIMPLES ;
END IF;
 
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
SET totalCSLLNFe = (totalBrutoNFe*(SELECT CSLL FROM configfinanc WHERE codigofilial=filial LIMIT 1) ) /100;
SET totalICMSFreteNFe = (totalFreteNFe *(SELECT aliquotaretencaofrete FROM configfinanc WHERE codigofilial=filial LIMIT 1) ) /100;
SET totalLiquidoNFe=(totalBrutoNFe+totalIPINFe+totalICMSSTNFe+totalDespesasNFe+totalSeguroNFe+totalFreteNFe)-totalDescontoNFe ;
IF (criarNF="S") THEN
REPEAT
SET tentativas = tentativas +1;
UPDATE configfinanc SET ultimoselonf = ultimoselonf+1
WHERE codigofilial=filial;
UPDATE serienf SET
         sequencial = sequencial+1 
         WHERE codigofilial=filial
         AND ABS(serie)=ABS(serieNFe) LIMIT 1;
SELECT MAX(sequencial) INTO numeroNFe FROM serienf WHERE ABS(serie)=ABS(serieNFe) AND codigofilial=filial LIMIT 1;
        
UNTIL (SELECT COUNT(1) FROM nfe012
         WHERE (cbdprocStatus='P' OR cbdprocstatus='E')
         AND cbdntfnumero=numeroNFe
          AND cbdntfserie=serieNFe
         AND cbdcodigofilial=filial) = 0 OR tentativas > 10
 END REPEAT;
END IF;
IF (criarNF="N") THEN
SET numeroNFe = NFeOrigem;
END IF;
IF (tipoNFe="0") THEN
UPDATE vendas SET pis=0,cofins=0,cstpis="98",cstcofins="98"  
WHERE (cfop<>"1.101" AND cfop<>"1.102" 
AND cfop<>"2.102" AND cfop<>"1.401" 
AND cfop<>'1.201' AND cfop<>'1.202'
AND cfop<>"1.403" AND cfop<>"1.410" AND cfop<>"2.403" 
AND cfop<>"1.933" AND cfop<>"2.933"
AND cfop<>"1.411" AND cfop<>"2.411"
AND cfop<>"2.122" AND cfop<>"2.124"
AND cfop<>"2.902") 
AND id=ipTerminal;
END IF;
IF (tipoNFe="1") THEN
  UPDATE vendas SET pis=0,cofins=0,cstpis="49",cstcofins="49" 
  WHERE (cfop<>"5.102" AND cfop<> "6.102" AND cfop<>"5.101" 
  AND cfop<> "6.101" AND cfop<>"5.402" AND cfop<>"6.402" 
  AND cfop<>"5.401" AND cfop<>"6.401" AND cfop<> "5.403" 
  AND cfop<> "6.403" AND cfop<> "6.404" AND cfop<>"5.405" 
  AND cfop<> "5.202" AND cfop<> "6.202" AND cfop <> "5.152" AND cfop <> "6.152")
  AND id=ipTerminal;
 END IF;
   
  UPDATE vendas SET notafiscal=numeroNFe,
  serieNF=serieNFe, modelodocfiscal=modeloNFe WHERE id=ipTerminal; 
  IF (gerarICMS="N") THEN
      UPDATE vendas SET icms=0 WHERE id=ipTerminal; 
   END IF;
 
IF (criarNF="N") THEN
      IF  ( ( SELECT chave_nfe FROM contnfsaida WHERE notafiscal=NFeOrigem AND serie=serieNFe  AND codigofilial=filial LIMIT 1) IS NULL ) THEN                   
	DELETE FROM contnfsaida WHERE notafiscal=NFeOrigem AND serie=serieNFe AND codigofilial=filial;                                                  
        DELETE FROM vendanf WHERE notafiscal=NFeOrigem AND serienf=serieNFe AND codigofilial=filial;    
	DELETE FROM nfe012 WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001 WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001det WHERE CbdNtfNumero=NFeOrigem  AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
        DELETE FROM cbd001detadicoes WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detarma WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detcofins WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001deticmsnormalst WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detdi WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;	
	DELETE FROM cbd001detipi WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detmed WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detpis WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;	
	DELETE FROM cbd001duplicatas WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001lacres WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001obsfisco WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001procref WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
        DELETE FROM cbd001reboque WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001vol WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
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
CURRENT_DATE,
operadorNFe,
cfopNFe,
idcliente,
IF(idcliente>0,(SELECT nome FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT razaosocial FROM fornecedores WHERE codigo=idFornecedorNFe) ),
IF(idcliente>0,(SELECT IF(cnpj<>"",cnpj,cpf) FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT IF(cgc<>"",cgc,cpf) FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ),
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
IFNULL((SELECT razaosocial FROM transportadoras WHERE inc=idTransportadoraNFe LIMIT 1),""),
IFNULL((SELECT placa FROM veiculos WHERE idtransportadora=idtransportadoraNFe AND inc=idVeiculoNFe LIMIT 1),""),
IFNULL((SELECT inscricao FROM transportadoras WHERE inc=idTransportadoraNFe LIMIT 1),""),
IFNULL((SELECT cnpj FROM transportadoras WHERE inc=idTransportadoraNFe LIMIT 1),""),
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
IF(TotalPesoLiquidoNFe=0, pesoLiquidoNFe, TotalPesoLiquidoNFe), 
tipoFreteNFe,
IF(TotalPesoBrutoNFe=0, pesoBrutoNFe, TotalPesoBrutoNFe),  
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
IFNULL((SELECT ANTT FROM veiculos WHERE idtransportadora=idtransportadoraNFe AND inc=idVeiculoNFe LIMIT 1),""),
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
   INSERT INTO `vendanf` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`serieNF`,`cfop`,`acrescimototalitem`,`cstpis`,`cstcofins`,`icmsst`,`percentualRedBaseCalcICMSST`,`mvast`,`subserienf`,`modelodocfiscal`,`aliquotaIPI`,`ecffabricacao`,`coo`,`cancelado`,`eaddados`,`ccf`,`pcredsn`,`qUnidIPI`,`vUnidIPI`,`ncm`,`nbm`,`ncmespecie`,`ratfrete`,`ratseguro`,`ratdespesas`,`cstipi`,`origem`,`datafabricacao`,`vencimentoproduto`,`modalidadeDetBaseCalcICMS`,`modalidadeDetBaseCalcICMSst`,`pautaICMS`,`pautaICMSST`) 
   SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`serieNF`,`cfop`,`acrescimototalitem`,`cstpis`,`cstcofins`,`icmsst`,`percentualRedBaseCalcICMSST`,`mvast`,`subserienf`,`modelodocfiscal`,`aliquotaIPI`,`ecffabricacao`,`coo`,`cancelado`,`eaddados`,`ccf`,`pcredsn`,`qUnidIPI`,`vUnidIPI`,`ncm`,`nbm`,`ncmespecie`,`ratfrete`,`ratseguro`,`ratdespesas`,`cstipi`,`origem`,`datafabricacao`,`vencimentoproduto`,`modalidadeDetBaseCalcICMS`,`modalidadeDetBaseCalcICMSst`,`pautaICMS`,`pautaICMSST`
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
(SELECT razaosocial FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1),
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
(SELECT MAX(numero) FROM moventradas WHERE id=ipTerminal),
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
END IF;
IF (tipoNFe="1") AND ( crtNFE<>"1") AND ((SELECT estado FROM filiais WHERE codigofilial=filial LIMIT 1)<>(SELECT estado FROM contnfsaida WHERE notafiscal=numeroNFe AND serie=serieNFe  AND codigofilial=filial LIMIT 1)) THEN
UPDATE vendas SET icms='4.00' WHERE id=ipTerminal AND cancelado='N' AND ABS(tributacao)=0 AND origem IN (1, 2, 3, 8);
END IF;
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
CURRENT_TIMESTAMP, 
IF (colocarDataHoraNFe="S",CONCAT(DATE_FORMAT(DataSaida, '%Y-%m-%d'), ' ', CURRENT_TIME),NULL) , 
IF (colocarDataHoraNFe="S",HoraSaida,NULL),
tipoNFe, 
(SELECT fCodigoEstadoMunIBGE("M",(SELECT cidade FROM filiais WHERE codigofilial=filial LIMIT 1),(SELECT estado FROM filiais WHERE codigofilial=filial LIMIT 1 )) ),
'1', 
tipoEmissaoNFe, 
finalidadeNFe, 
(SELECT cnpj FROM filiais WHERE codigofilial=filial LIMIT 1), 
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
(IF(idcliente>0,(SELECT email FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT email FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ), 
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
fvTotalBCICMSnfe(filial,ipTerminal, crtNFE, arredondar), 
fvTotalICMSnfe(filial,ipTerminal, crtNFE, arredondar), 
fvTotalBCICMSSTnfe(filial,ipTerminal, arredondar),
fvTotalICMSSTnfe(filial,ipTerminal, arredondar),
IF (finalidadeNFe<>'2',totalBrutoNFe,0), 
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
IF (idtransportadoraNFe>0,(SELECT cnpj FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT cpf FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT razaosocial FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT inscricao FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT CONCAT(endereco,numero) FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT cidade FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
IF (idtransportadoraNFe>0,(SELECT estado FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),''), 
totalFreteNFe, 
totalFreteNFe, 
(SELECT aliquotaretencaofrete FROM configfinanc WHERE codigofilial=filial LIMIT 1), 
totalICMSFreteNFe, 
REPLACE(cfopTransporteNFE, '.', ''), 
(SELECT fCodigoEstadoMunIBGE("M",(SELECT cidade FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1),(SELECT estado FROM transportadoras WHERE inc=idtransportadoraNFe LIMIT 1)) ), 
IF (idVeiculoNFe>0,(SELECT placa FROM veiculos WHERE inc=idVeiculoNFe LIMIT 1),''), 
IF (idVeiculoNFe>0,(SELECT estadoplaca FROM veiculos WHERE inc=idVeiculoNFe LIMIT 1),''),
IF (idVeiculoNFe>0,(SELECT ANTT FROM veiculos WHERE inc=idVeiculoNFe LIMIT 1),''), 
'', 
IF(naturezaOperacaoNFe="Venda",totalBrutoNFe+totalFreteNFe+totalDespesasNFe+totalIPINFe,'0'), 
IF(naturezaOperacaoNFe="Venda",descontoNFe,'0'), 
IF(naturezaOperacaoNFe="Venda",(totalBrutoNFe+totalFreteNFe+totalDespesasNFe+totalIPINFe)-descontoNFe,'0'), 
'', 
dadosComplementarNFe, 
(IF(idcliente>0,(SELECT estado FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT estado FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
(IF(idcliente>0,(SELECT cidade FROM clientes WHERE codigo=idcliente LIMIT 1),(SELECT cidade FROM fornecedores WHERE codigo=idFornecedorNFe LIMIT 1) ) ),
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
(SELECT IF(p.cprodanp='', '0', p.cprodanp) FROM produtos AS p WHERE p.codigo=vendas.codigo LIMIT 1),
(SELECT IF(p.codif='', '0', p.codif) FROM produtos AS p WHERE p.codigo=vendas.codigo LIMIT 1),
quantidade,
((total+ratfrete+ratdespesas+ratseguro)-ratdesc),
(SELECT IF(p.cide>0, p.cide, 0) FROM produtos AS p WHERE p.codigo=vendas.codigo LIMIT 1),
(((total+ratfrete+ratdespesas+ratseguro)-ratdesc)*(SELECT IF(p.cide>0, p.cide, 0) FROM produtos AS p WHERE p.codigo=vendas.codigo LIMIT 1)/100),
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
'0', 
(SELECT fCodigoEstadoMunIBGE("E","",(SELECT estado FROM filiais WHERE codigofilial=filial LIMIT 1)) ) , 
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
fdescCPL(filial, codigo), 
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
SELECT 
filial, 
serieNFe, 
numeroNFe, 
nrcontrole, 
cstcofins, 
IF(cofins>0,total,0), 
cofins, 
IF(cofins>0,total*cofins/100,0), 
'0', 
'0', 
filial
FROM vendas WHERE id=ipTerminal AND cancelado="N";
INSERT INTO cbd001detipi 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnItem, CbdCST_IPI, CbdvBC_IPI, CbdqUnid_IPI,
CbdvUnid_IPI, CbdpIPI, CbdvIPI, CbdCodigoFilial) 
SELECT
filial, 
serieNFe, 
numeroNFe, 
nrcontrole, 
cstIPI, 
fvBCIPI(filial,ipTerminal, vendas.codigo, vendas.inc, crtNFE, arredondar), 
qUnidIPI, 
vUnidIPI, 
aliquotaipi, 
ROUND(((fvBCIPI(filial,ipTerminal, vendas.codigo, vendas.inc, crtNFE, arredondar)*aliquotaipi)/100), 2),
filial 
FROM vendas WHERE id=ipTerminal AND cancelado="N";
IF ( crtNFE="1") THEN
INSERT INTO cbd001deticmsnormalst 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnItem, CbdCST, Cbdorig, 
CbdmodBC, CbdvBC, CbdpICMS, CbdvICMS_icms, CbdmodBCST, CbdpMVAST, CbdpRedBCST, CbdvBCST, CbdpICMSST, 
CbdvICMSST_icms, CbdpRedBC, 
CbdvBCSTRet, CbdvICMSSTRet, CbdpBCOp, CbdUFST, CbdvBCSTDest, CbdvICMSSTDest_icms,
 CbdpCredSN, CbdvCredICMSSN, CbdCodigoFilial) 
 SELECT 
filial, 
serieNFe, 
numeroNFe, 
nrcontrole,
tributacao, 
origem, 
`vendas`.modalidadeDetBaseCalcICMS, 
IF(icms>0 AND (tributacao="10" OR tributacao="70") , 0 ,0 ) , 
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
END IF;
IF ( crtNFE<>"1") THEN
INSERT INTO cbd001deticmsnormalst 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnItem, CbdCST, Cbdorig, 
CbdmodBC, CbdvBC, CbdpICMS, CbdvICMS_icms, CbdmodBCST, CbdpMVAST, CbdpRedBCST, CbdvBCST, CbdpICMSST, 
CbdvICMSST_icms, CbdpRedBC, 
CbdvBCSTRet, CbdvICMSSTRet, CbdpBCOp, CbdUFST, CbdvBCSTDest, CbdvICMSSTDest_icms,
 CbdpCredSN, CbdvCredICMSSN, CbdCodigoFilial) 
 SELECT
filial, 
serieNFe, 
numeroNFe, 
nrcontrole,
tributacao, 
origem,  
`vendas`.modalidadeDetBaseCalcICMS, 
IF(icms>0, fvBCICMS(filial, ipterminal, vendas.codigo, vendas.inc, arredondar), 0) , 
icms, 
IF(icms>0, fvICMS(filial, ipterminal, vendas.codigo, vendas.inc, arredondar), 0), 
`vendas`.modalidadeDetBaseCalcICMSst,  
mvast, 
percentualRedBaseCalcICMSST, 
IF((`vendas`.modalidadeDetBaseCalcICMSst='0' OR `vendas`.modalidadeDetBaseCalcICMSst='3'), vbcICMSST, IF(icmsst>0, fvBCICMSst(filial, ipterminal, vendas.codigo, vendas.inc, crtNFE, arredondar),0)),
icmsst, 
IF((`vendas`.modalidadeDetBaseCalcICMSst='0' OR `vendas`.modalidadeDetBaseCalcICMSst='3'), vICMSST, IF(icmsst>0, fvICMSst(filial, ipterminal, vendas.codigo, vendas.inc, crtNFE, arredondar),0)),
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
SELECT 
filial, 
serieNFe, 
numeroNFe, 
nrcontrole, 
lote, 
quantidade, 
datafabricacao, 
vencimento, 
preco, 
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
cstpis, 
IF(pis>0,total,0), 
pis, 
IF(pis>0,total*pis/100,0), 
'0', 
'0', 
filial
FROM vendas WHERE id=ipTerminal AND cancelado="N";
INSERT INTO cbd001duplicatas 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnDup, CbddVenc, CbdvDup, CbdCodigoFilial) 
SELECT 
filial, 
serieNFe, 
numeroNFe, 
IF(tipopagamento<>'OU',IF((historico<>'*' AND historico<>'Nota Fiscal' AND historico<>'Cupom Fiscal' AND historico<>'NFe'), historico, CONCAT(tipopagamento,"-",IFNULL(nrparcela,DATE_FORMAT(IFNULL(vencimento, DATA), '%d/%m/%Y')))), CONCAT('OUTROS ',nrparcela) ) AS ndup,
IFNULL(vencimento, DATA), 
valor, 
filial
FROM caixas WHERE EnderecoIP=ipTerminal AND historico<>'Entrada' GROUP BY vencimento, nrparcela;
IF(modeloNFe = '55')THEN
	INSERT INTO cbd001pag(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, cbdcodigofilial, CbdtPag, CbdvPag, cbdtpIntegra, CbdtBand,CbdcAut,CbdCNPJ)  
	SELECT ABS(filial), serieNFe, numeroNFe, filial, CASE tipopagamento 
	WHEN "DH" THEN "1" 
	WHEN "CH" THEN "2" 
	WHEN "CA" THEN "3" 
	WHEN "CR" THEN "5" 
	WHEN "TI" THEN "10" 
	WHEN "OU" THEN "99" 
	END  AS tpag, SUM(valor), tpIntegracao, 
	(SELECT CASE cartao WHEN "VISA" THEN "01" WHEN "MASTERCARD" THEN "02" WHEN "AMERICAN EXPRESS" THEN "03" WHEN "SOROCRED" THEN "04" END FROM movcartoes WHERE documento=doc GROUP BY documento) AS tBand,
	(SELECT autorizacao FROM movcartoes WHERE documento=doc GROUP BY documento),
	(SELECT cnpj FROM cartoes WHERE descricao = IF((SELECT DATA FROM contdocs WHERE documento = doc AND codigoFilial = filial)=CURRENT_DATE,(SELECT Cartao FROM caixa WHERE documento = doc AND tipopagamento = 'CA' AND codigofilial = filial AND dpfinanceiro = "Venda" LIMIT 1),(SELECT Cartao FROM caixaarquivo WHERE documento = doc AND tipopagamento = 'CA' AND codigofilial = filial AND dpfinanceiro = "Venda" LIMIT 1)))
	FROM caixas WHERE EnderecoIP=ipTerminal 
	AND historico<>'Entrada' AND dpfinanceiro ='Venda' 
	GROUP BY vencimento, nrparcela, tipopagamento;
ELSE 
	INSERT INTO cbd001pag(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, cbdcodigofilial, CbdtPag, CbdvPag, cbdtpIntegra, CbdtBand,CbdcAut,CbdCNPJ)  
	SELECT ABS(filial), serieNFe, numeroNFe, filial, CASE tipopagamento 
	WHEN "DH" THEN "1" 
	WHEN "CH" THEN "2" 
	WHEN "CA" THEN "3" 
	WHEN "CR" THEN "5" 
	WHEN "TI" THEN "10" 
	WHEN "OU" THEN "99" 
	END  AS tpag, SUM(valor), tpIntegracao, 
	(SELECT CASE cartao WHEN "VISA" THEN "01" WHEN "MASTERCARD" THEN "02" WHEN "AMERICAN EXPRESS" THEN "03" WHEN "SOROCRED" THEN "04" END FROM movcartoes WHERE documento=doc GROUP BY documento) AS tBand,
	(SELECT autorizacao FROM movcartoes WHERE documento=doc GROUP BY documento),
	(SELECT cnpj FROM cartoes WHERE descricao = IF((SELECT DATA FROM contdocs WHERE documento = doc AND codigoFilial = filial)=CURRENT_DATE,(SELECT Cartao FROM caixa WHERE documento = doc AND tipopagamento = 'CA' AND codigofilial = filial AND dpfinanceiro = "Venda" LIMIT 1),(SELECT Cartao FROM caixaarquivo WHERE documento = doc AND tipopagamento = 'CA' AND codigofilial = filial AND dpfinanceiro = "Venda" LIMIT 1)))
	FROM caixas WHERE EnderecoIP=ipTerminal 
	AND dpfinanceiro ='Venda' 
	GROUP BY vencimento, nrparcela, tipopagamento;
END IF;
 INSERT INTO cbd001lacres 
 (CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnLacre, CbdCodigoFilial) 
 VALUES (
 filial,
 serieNFe,
 numeroNFe,
 '',
filial);
INSERT INTO cbd001procref 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdnProc, CbdindProc, CbdCodigoFilial)
VALUES (
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
(SELECT marca FROM contnfsaida WHERE numero=numeroNFe AND serie=serieNFe AND codigofilial=filial LIMIT 1), 
IF(TotalPesoLiquidoNFe=0, pesoLiquidoNFe, TotalPesoLiquidoNFe),  
IF(TotalPesoBrutoNFe=0, pesoBrutoNFe, TotalPesoBrutoNFe),  
filial);
INSERT INTO nfe012 
(CbdEmpCodigo, CbdNtfSerie, CbdNtfNumero, CbdAcao, CbdProcStatus, CbdCodigoFilial) 
VALUES (
filial,
serieNFe,
numeroNFe,
'E',
IF ( (SELECT previsualizarnfe FROM configfinanc WHERE codigofilial=filial LIMIT 1)="N",'N','V'),
filial
 );
 IF (doc>0) THEN
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
 
 END IF;
 
 
 IF ( crtNFE="1") THEN
 
                      UPDATE cbd001deticmsnormalst SET cbdcst="102" 
                      WHERE (cbdcst="0" OR cbdcst="20")
                      AND CbdEmpCodigo=filial
                      AND CbdNtfSerie= serieNFe
                       AND CbdNtfNumero= numeroNFe
                       AND CbdCodigoFilial=filial;
 
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
 
			if (select cbdcst from cbd001deticmsnormalst WHERE CbdEmpCodigo=filial AND CbdNtfSerie= serieNFe AND CbdNtfNumero= numeroNFe AND CbdCodigoFilial=filial)="500" then
				UPDATE cbd001det SET cbdCFOP="5405"  WHERE CbdEmpCodigo=filial AND CbdNtfSerie= serieNFe AND CbdNtfNumero= numeroNFe AND CbdCodigoFilial=filial;
			end if;
 
 
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
 
 
 END IF;
 
  UPDATE caixas SET 
 documento=numeroNFe,
 estornado="N", historico="NFe",tipodoc="3",
 nome = (SELECT nome FROM clientes WHERE codigo=idcliente LIMIT 1),
 codigocliente = (SELECT codigo FROM clientes WHERE codigo=idcliente LIMIT 1)
 WHERE caixas.enderecoip=ipTerminal;
UPDATE caixas SET descricaopag="Dinheiro" WHERE tipopagamento="DH" AND enderecoip=ipTerminal;
UPDATE caixas SET descricaopag="Cheque" WHERE tipopagamento="CH" AND enderecoip=ipTerminal;
UPDATE caixas SET descricaopag="Cartão" WHERE tipopagamento="CA" AND enderecoip=ipTerminal;
UPDATE caixas SET descricaopag="Crediário" WHERE tipopagamento="CR" AND enderecoip=ipTerminal;
UPDATE caixas SET descricaopag="Ticket" WHERE tipopagamento="TI" AND enderecoip=ipTerminal;
 
 
 
 INSERT INTO caixanf (horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,DATA,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch,ecfnumero,ecffabricacao,ecfmodelo,coo,ccf,gnf,descricaopag,tipodoc)  
 SELECT horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,DATA,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch,ecfnumero,ecffabricacao,ecfmodelo,coo,ccf,gnf,descricaopag,tipodoc
 FROM caixas 
 WHERE enderecoip=ipTerminal;
 
 DELETE FROM vendas WHERE id=ipTerminal;
 DELETE FROM caixas WHERE caixas.enderecoip=ipTerminal; 
 
SELECT numeroNFe;  
 
END */$$
DELIMITER ;

/* Procedure structure for procedure `GravarCaixaSoma` */

/*!50003 DROP PROCEDURE IF EXISTS  `GravarCaixaSoma` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `GravarCaixaSoma`(IN idOperador VARCHAR(10),IN filial VARCHAR(5),IN ipTerminal VARCHAR(15),IN nrFabricaoECF VARCHAR(20),IN dataCaixa DATE )
BEGIN
SET @horaabertura=(SELECT horaabertura FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="SI" LIMIT 1); 
SET @saldo =(SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND dpfinanceiro="Saldo Inicial" AND tipopagamento <> "SU" AND DATA = dataCaixa);
SET @dinheiro = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="DH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND dpfinanceiro<>"Recebimento CH"
  AND DATA = dataCaixa);
 
SET @entradadh = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="DH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada" 
   AND DATA = dataCaixa);
 
 SET @entradach = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada" 
   AND DATA = dataCaixa);
 
 SET @entradaca = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CA" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada" 
   AND DATA = dataCaixa);
 
 SET @entradafi = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="FI" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND historico="Entrada"
   AND DATA = dataCaixa);
 
 SET @cheque = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
  AND tipopagamento="CH" 
  AND dpfinanceiro<>"Crediario" 
  AND dpfinanceiro<>"Troca CH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND vencimento=dataCaixa); 
 
 SET @chequepre = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
  AND tipopagamento="CH" 
  AND dpfinanceiro<>"Crediario" 
  AND dpfinanceiro<>"Troca CH" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND vencimento>dataCaixa
   AND DATA = dataCaixa);
 
 SET @cartao = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CA" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
    AND DATA = dataCaixa);
 
   SET @recebimento = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'
    AND DATA = dataCaixa);
 
 SET @recebimentodh = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="DH"
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'
    AND DATA = dataCaixa); 
 
 SET @recebimentoch = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="CH"
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'
    AND DATA = dataCaixa);  
 
 SET @recebimentoca = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="CA"
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
  AND dpfinanceiro LIKE 'Receb%'
    AND DATA = dataCaixa); 
 
SET @crediario = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="CR"
  AND dpfinanceiro="Venda"
    AND DATA = dataCaixa);
 
 SET @sangria = (SELECT IFNULL(SUM(valor),0) FROM movdespesas WHERE operador=idOperador AND codigofilial=filial
  AND sangria='S'
    AND DATA = dataCaixa);
 SET @vendas = (SELECT IFNULL(SUM(total-ratdesc+rateioencargos),0) FROM venda WHERE operador=idOperador AND codigofilial=filial
 AND cancelado="N"
    AND DATA = dataCaixa);
 
 SET @custos = (SELECT IFNULL(SUM(ABS( (quantidade *embalagem)  )*custo),0) FROM venda WHERE operador=idOperador AND codigofilial=filial
 AND cancelado="N"
    AND DATA = dataCaixa);
 
 SET @juros = (SELECT IF(SUM(vrjuros-vrdesconto)>=0,SUM(vrjuros),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND dpfinanceiro LIKE 'Receb%'
   AND DATA = dataCaixa);
SET @devolucao = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="DV" AND dpfinanceiro <> 'Venda'
    AND DATA = dataCaixa);
 
SET @renegociacao = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="RN"
    AND DATA = dataCaixa);
SET @perdao = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="PD"
    AND DATA = dataCaixa);
 
 SET @descontovenda = (SELECT IFNULL(SUM(ratdesc+descontovalor),0) FROM venda WHERE  operador=idOperador AND codigofilial=filial
 AND cancelado="N"
    AND DATA = dataCaixa);
    
 SET @descontoreceb = (SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND dpfinanceiro LIKE 'Receb%'
AND estornado <> 'N'
AND vrdesconto>vrjuros 
AND DATA = dataCaixa);
SET @descontorecebjuros = ( SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
 AND dpfinanceiro LIKE 'Receb%'
 AND (vrdesconto>0 OR vrdesconto>vrjuros) 
 AND dpfinanceiro<>'Recebimento s/j' 
    AND DATA = dataCaixa);
 
 
 SET @crediariocr = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="CR"
  AND dpfinanceiro="Crediario"
    AND DATA = dataCaixa);
 SET @jurosperdao = ( SELECT IFNULL( IF(SUM(vrjuros-vrdesconto)>0,SUM(vrjuros-vrdesconto),0),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
AND tipopagamento="PD" 
   AND DATA = dataCaixa);
SET @jurosrenegociacao = (SELECT IFNULL( IF(SUM(vrjuros-vrdesconto)>0,SUM(vrjuros-vrdesconto),0),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
AND tipopagamento="RN"
   AND DATA = dataCaixa);
SET @encargos = (SELECT IFNULL(SUM(rateioencargos),0) FROM venda WHERE  operador=idOperador AND codigofilial=filial
 AND cancelado="N"
    AND DATA = dataCaixa);
SET @devolucaocr = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="DV"
 AND dpfinanceiro LIKE 'Receb%'
    AND DATA = dataCaixa);
 
 SET @devolucaoprd = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="DV" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND dpfinanceiro<>"Sangria"
    AND DATA = dataCaixa);
 
 
 SET @jurosrecch = (SELECT IFNULL(SUM(jurosch),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD' 
  AND dpfinanceiro LIKE 'Receb%'
    AND DATA = dataCaixa);
 
SET @encargosrecebidos = (SELECT IFNULL(SUM(encargos),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD' 
  AND dpfinanceiro LIKE 'Receb%'
    AND DATA = dataCaixa);
 
 SET @diferenca = (SELECT IFNULL(SUM(valor),0)FROM caixa  WHERE operador=idOperador AND codigofilial=filial 
   AND dpfinanceiro="Diferenca"
    AND DATA = dataCaixa);
 SET @ocorrencia = "";
 
 SET @chequefi = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Crediario"
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND vencimento=dataCaixa 
    AND DATA = dataCaixa);
 
 SET @chequefipre = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Crediario"
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
  AND vencimento>DATA 
    AND DATA = dataCaixa);
 
 SET @financiamento = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
 AND tipopagamento="FN" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
    AND dpfinanceiro = "Crediario"
    AND DATA = dataCaixa);
 
 SET @financeira = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
 AND (tipopagamento="FI" OR tipopagamento="FN")
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
 AND dpfinanceiro = 'Venda'
    AND DATA = dataCaixa);
 
 SET @qtdcupons = (SELECT COUNT(1) AS quantidade FROM contdocs WHERE operador=idOperador AND codigofilial=filial
  AND dpfinanceiro="Venda"  
 AND DATA=(SELECT DATA FROM caixa WHERE operador=idoperador LIMIT 1) 
   AND DATA = dataCaixa);
 
 SET @suprimento = (SELECT IFNULL(SUM(valor),0) AS suprimento FROM caixa WHERE operador=idOperador AND codigofilial=filial
 AND tipopagamento="SU"
    AND DATA = dataCaixa);
 
 SET @dpfinanceiro = "Venda";
 
 
 
 SET @receitas = (SELECT IFNULL(SUM(valor),0) AS receita FROM movreceitas  WHERE operador=idOperador AND codigofilial=filial
  AND sangria='S'
    AND DATA = dataCaixa);
 
 SET @recebimentobl = (SELECT IFNULL(SUM(valor),0) FROM caixa  WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND tipopagamento="BL"
AND dpfinanceiro LIKE 'Receb%'
   AND DATA = dataCaixa);
SET @recebimentodc = (SELECT IFNULL(SUM(valor),0) FROM caixa  WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND tipopagamento="DC"
AND dpfinanceiro LIKE 'Receb%'
   AND DATA = dataCaixa);
  SET @emprestimodh = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND dpfinanceiro="Emprestimo DH"
  AND (tipopagamento="CR" OR tipopagamento="FN")
 AND DATA = dataCaixa);
 
 SET @emprestimoch = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=IF( (idOperador="" OR idOperador="Todos"),operador, idOperador) AND codigofilial=filial 
  AND dpfinanceiro="Emprestimo CH"
  AND (tipopagamento="CH" OR tipopagamento="FN")
 AND DATA = dataCaixa);
 
 SET @comprati = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CT" 
  AND dpfinanceiro="Compra TI"
    AND DATA = dataCaixa);
 
  SET @trocach = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="CH" 
  AND dpfinanceiro="Troca CH"
    AND DATA = dataCaixa);
  
 
 SET @ticket = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial AND tipopagamento="TI" 
  AND dpfinanceiro<>"Recebimento"
  AND dpfinanceiro<>"Recebimento est"
  AND dpfinanceiro<>"Recebimento s/j"
    AND DATA = dataCaixa);
 
 SET @valorservicos = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial  
  AND dpfinanceiro="Servicos"
    AND DATA = dataCaixa);
 
 SET @descontoservicos = (SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial  
  AND dpfinanceiro="Servicos"
    AND DATA = dataCaixa);
 
 SET @descontocapitalrn = (SELECT IFNULL(SUM(vrdesconto),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial 
  AND tipopagamento="RN"
    AND DATA = dataCaixa);
 
 
 SET @crediarioservicosCR = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
  AND dpfinanceiro="Servicos"
  AND tipopagamento="CR"
    AND DATA = dataCaixa);
 
 SET @jurosrecca = (SELECT IFNULL(SUM(jurosca),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND dpfinanceiro LIKE 'Receb%'
   AND DATA = dataCaixa);
SET @recebimentoAV = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento<>'RN' AND tipopagamento<>'DV' AND tipopagamento<>'PD'
AND tipopagamento="AV"
AND dpfinanceiro LIKE 'Receb%'
   AND DATA = dataCaixa);
   
SET @vendaAV = (SELECT IFNULL(SUM(valor),0) FROM caixa WHERE operador=idOperador AND codigofilial=filial
AND tipopagamento="AV"
AND dpfinanceiro='Venda'
AND DATA = dataCaixa);
 
SET @saldoFinal = (@saldo+@suprimento+@dinheiro+@cheque+@chequefi+@financeira+@chequepre+@emprestimoch+@trocach+@cartao+@ticket+@vendaAV+(@recebimento-@recebimentoAV)+ @receitas - @sangria )+@diferenca;
SET @saldoLiquidoEspecie = (@saldo+@suprimento+@dinheiro+@cheque+@chequepre+@RecebimentoDH+@RecebimentoCh+@Receitas)-@sangria+@diferenca;
INSERT INTO caixassoma (ip,codigofilial,DATA,horaabertura,horafechamento,operador,
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
  recebimentoAV,saldoLiquidoEspecie,saldocaixa )
   VALUES (ipTerminal,filial,dataCaixa,
  @horaabertura,
  CURRENT_TIME,
  idOperador, 
  @saldo,@dinheiro,@entradadh,@entradach,
  @entradaca,@entradafi,@cheque,@chequepre,@cartao,@recebimento,
  @recebimentodh,@recebimentoch,@recebimentoca,
  @crediario,@sangria,@vendas,@custos,@juros,@devolucao,
  @renegociacao,@perdao,@descontovenda,@descontoreceb,
  @descontorecebjuros,@crediariocr,@jurosperdao,@jurosrenegociacao,
  @encargos,@devolucaocr,@devolucaoprd,@jurosrecch,encargosrecebidos, 
  @diferenca,@ocorrencia,@chequefi,@chequefipre,@financiamento,@financeira,
  @qtdcupons,@suprimento,@dpfinanceiro,@receitas,@recebimentobl,@recebimentodc,
  @emprestimodh,@emprestimoch,@comprati,@trocach,ticket,@valorservicos,
  @descontoservicos,@descontocapitalrn,@crediarioservicosCR,@jurosrecca,
  @recebimentoAV,@saldoLiquidoEspecie,@saldoFinal);
    END */$$
DELIMITER ;

/* Procedure structure for procedure `gravarR` */

/*!50003 DROP PROCEDURE IF EXISTS  `gravarR` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `gravarR`(in dataMovimento DATETIME )
BEGIN
UPDATE r01 SET EADdados = MD5(CONCAT(fabricacaoECF, cnpj , cnpjdesenvolvedora , aplicativo , MD5,MFAdicional,tipoECF,modeloECF,versaoSB,datainstalacaoSB,horainstalacaoSB,numeroECF,inscricao,inscricaodesenvolvedora,inscricaomunicipaldesenvolvedora,razaosocialdesenvolvedora,versao,versaoERPAF));
update r02 set eaddados=md5(concat(fabricacaoECF,crz,coo,cro,data,dataemissaoreducaoz,horaemissaoreducaoz,vendabrutadiaria,modeloECF,MFadicional,datamovimento))
where data=datamovimento;
update r03 set eaddados=md5(concat(fabricacaoECF,CRZ,totalizadorParcial))
where data=datamovimento;
CALL AtualizarQdtRegistros();
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarEntrada` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarEntrada` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `ProcessarEntrada`(IN filial VARCHAR(5), IN numeroEntrada INT,IN alterar CHAR(1),IN operadorLanc VARCHAR(10),IN pedCompra INT,IN numeroProducao INT, IN numeroTransf INT)
BEGIN
DECLARE totalFreteNFe REAL DEFAULT 0;
DECLARE totalSeguroNFe REAL DEFAULT 0;
DECLARE totalDespesasNFe REAL DEFAULT 0;
DECLARE despesasNFe REAL DEFAULT 0;
DECLARE totalDescontoNFe REAL DEFAULT 0;
DECLARE seguroNFe REAL DEFAULT 0;
DECLARE freteNFe REAL DEFAULT 0;
DECLARE descontoNFe REAL DEFAULT 0;
DECLARE totalBrutoNFe REAL DEFAULT 0;
DECLARE cursorDespesas CURSOR FOR SELECT despesas FROM moventradas WHERE numero=numeroEntrada LIMIT 1;
DECLARE cursorDesconto CURSOR FOR SELECT descontos FROM moventradas WHERE numero=numeroEntrada LIMIT 1;
DECLARE cursorSeguro CURSOR FOR SELECT valorseguro FROM moventradas WHERE numero=numeroEntrada LIMIT 1;
DECLARE cursorFrete CURSOR FOR SELECT frete FROM moventradas WHERE numero=numeroEntrada LIMIT 1;
DECLARE cursorTotalBruto CURSOR FOR SELECT valorprodutos FROM moventradas WHERE numero=numeroEntrada LIMIT 1;
OPEN cursorDespesas;
OPEN cursorDesconto;
OPEN cursorSeguro;
OPEN cursorFrete;
OPEN cursorTotalBruto;
FETCH cursorDespesas INTO despesasNFe;
FETCH cursorDesconto INTO descontoNFe; 
FETCH cursorSeguro INTO seguroNFe;
FETCH cursorFrete INTO freteNFe;
FETCH cursorTotalBruto INTO totalBrutoNFe;
IF (numeroTransf>0) THEN
UPDATE moventradas SET lancada="X",DATA=CURRENT_DATE,horaentrada=CURRENT_TIME WHERE numero=numeroEntrada;
 UPDATE entradas SET lancada="X" WHERE numero=numeroEntrada;
 UPDATE entradas SET cfopentrada=(SELECT cfopentrada FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada AND cfopentrada IS NULL;
 UPDATE entradas SET dataentrada=(SELECT dataentrada FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada;
 UPDATE entradas SET modeloNF=(SELECT modelonf FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada;
UPDATE entradas SET dataemissao=(SELECT moventradas.DataEmissao FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada;
  
END IF; 
IF (numeroTransf=0) THEN
IF (alterar="S") THEN
	IF (filial="00001") THEN
		UPDATE produtos,entradas 
		SET produtos.qtdanterior=produtos.quantidade,
		produtos.customedioanterior=produtos.customedio,
		produtos.quantidade=produtos.quantidade+(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
		produtos.qtdultent=(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
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
		produtos.dataaltpreco=CURRENT_DATE,
		produtos.custogerencial = entradas.custoCalculadoGerencial,
		produtos.MVAFronteira = entradas.mvagerencial,
		produtos.aliquotaStFronteira = entradas.aliquotastgerencial
		WHERE produtos.codigo=entradas.codigo 
		AND entradas.numero=numeroEntrada
		AND entradas.lancada<>'X'
		AND produtos.CodigoFilial=filial;
		UPDATE produtos,entradas 
		SET produtos.customedio=IFNULL(( ( (entradas.quantidade*entradas.custo)+( (produtos.qtdanterior+produtos.qtdretida)*produtos.ultcusto) ) / (produtos.quantidade+produtos.qtdretida)),produtos.customedio) 
		WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
		 AND entradas.lancada<>'X'
		 AND produtos.codigofilial=filial;
 
		UPDATE entradas,produtos 
		SET entradas.customedio=produtos.customedio,
		entradas.quantidadeanterior=produtos.qtdanterior,
		entradas.quantidadeatualizada=produtos.quantidade,
		entradas.serienf='1',
		entradas.customedioanterior=produtos.customedioanterior,
		entradas.operacao=' .   ',
		entradas.lancada='X',
		entradas.icms=produtos.icms 
		 WHERE entradas.numero=numeroEntrada
		 AND entradas.codigo=produtos.codigo 
		 AND entradas.codigofilial=produtos.codigofilial 
		 AND produtos.codigofilial=filial;
	
		UPDATE produtos AS p,entradas 
		SET 
		p.custo=entradas.custoMascaraCalculado
		WHERE p.codigo=entradas.codigo 
		AND entradas.numero=numeroEntrada
		AND entradas.lancada='X'
		AND entradas.CodigoFilial=filial
		AND entradas.custoMascaraCalculado > 0;
		IF((SELECT alterarprecofiliaisentrada FROM configfinanc WHERE codigofilial = '00001' LIMIT 1) = 'S')THEN
			UPDATE produtosfilial AS p,entradas 
			SET 
			p.custo=entradas.custocalculado,
			p.precovenda=entradas.precovenda,
			p.precoatacado=entradas.precoatacado,
			p.margemlucro=entradas.margemlucro,
			p.marcado='X',
			p.ipi=entradas.ipi,
			p.frete=entradas.frete,
			p.operador=operadorLanc,
			p.dataaltpreco=CURRENT_DATE,
			p.custogerencial = entradas.custoCalculadoGerencial 
			WHERE p.codigo=entradas.codigo 
			AND entradas.numero=numeroEntrada
			AND entradas.lancada='X'
			AND entradas.CodigoFilial=filial; 
	
			UPDATE produtosfilial AS p,entradas 
			SET 
			p.custo=entradas.custoMascaraCalculado
			WHERE p.codigo=entradas.codigo 
			AND entradas.numero=numeroEntrada
			AND entradas.lancada='X'
			AND entradas.CodigoFilial=filial
			AND entradas.custoMascaraCalculado > 0;
			
		END IF;
	
	       
	
	
	END IF;
	
	IF (filial<>"00001") THEN
		UPDATE produtosfilial AS produtos,entradas 
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
		produtos.dataaltpreco=CURRENT_DATE,
		produtos.MVAFronteira = entradas.mvagerencial,
		produtos.aliquotaStFronteira = entradas.aliquotastgerencial
		WHERE produtos.codigo=entradas.codigo 
		AND entradas.numero=numeroEntrada
		AND entradas.lancada<>'X'
		AND produtos.CodigoFilial=filial;
		UPDATE produtosfilial AS produtos,entradas 
		SET produtos.customedio=IFNULL(( ( (entradas.quantidade*entradas.custo)+( (produtos.qtdanterior+produtos.qtdretida)*produtos.ultcusto) ) / (produtos.quantidade+produtos.qtdretida)),produtos.customedio) 
		WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
		 AND entradas.lancada<>'X'
		 AND produtos.codigofilial=filial;
		UPDATE entradas,produtosfilial AS produtos 
		SET entradas.customedio=produtos.customedio,
		entradas.quantidadeanterior=produtos.qtdanterior,
		entradas.quantidadeatualizada=produtos.quantidade,
		entradas.serienf='1',
		entradas.customedioanterior=produtos.customedioanterior,
		entradas.operacao=' .   ',
		entradas.lancada='X',
		entradas.icms=produtos.icms 
		 WHERE entradas.numero=numeroEntrada 
		 AND entradas.codigo=produtos.codigo 
		 AND entradas.codigofilial=produtos.codigofilial 
		 AND produtos.codigofilial=filial;
		
		UPDATE produtosfilial AS p,entradas 
		SET 
		p.custo=entradas.custoMascaraCalculado
		WHERE p.codigo=entradas.codigo 
		AND entradas.numero=numeroEntrada
		AND entradas.lancada='X'
		AND entradas.CodigoFilial=filial
		AND entradas.custoMascaraCalculado > 0;
	
		
	END IF;
END IF; 
	IF (alterar="N") THEN
		IF (filial="00001") THEN
			UPDATE produtos,entradas 
			SET produtos.qtdanterior=produtos.quantidade,
			produtos.quantidade=produtos.quantidade+(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
			produtos.qtdultent=(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
			produtos.dataultent=CURRENT_DATE,
			produtos.idfornecedor=(SELECT moventradas.codigofornecedor FROM moventradas WHERE  moventradas.numero=numeroEntrada),
			produtos.fornecedor=entradas.fornecedor,
			produtos.marcado='X',
			produtos.operador=operadorLanc,
			produtos.lote=entradas.lote,
			produtos.vencimento=entradas.vencimento,
			produtos.datafabricacao=entradas.datafabricacao,
			produtos.qtdprateleiras=produtos.qtdprateleiras+(SELECT SUM(entradas.qtdprateleiras) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada) 
			WHERE produtos.codigo=entradas.codigo 
			AND entradas.numero=numeroEntrada 
			AND entradas.Lancada<>'X';
			UPDATE entradas,produtos 
			SET entradas.quantidadeanterior=produtos.qtdanterior,
			entradas.quantidadeatualizada=produtos.quantidade,
			entradas.operacao=' .   '
			 WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada;
		END IF;
		IF (filial<>"00001") THEN
			UPDATE produtosfilial AS produtos,entradas 
			SET produtos.qtdanterior=produtos.quantidade,
			produtos.quantidade=produtos.quantidade+(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
			produtos.qtdultent=(SELECT SUM(entradas.quantidade) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada),
			produtos.dataultent=CURRENT_DATE,
			produtos.idfornecedor=(SELECT moventradas.codigofornecedor FROM moventradas WHERE  moventradas.numero=numeroEntrada),
			produtos.fornecedor=entradas.fornecedor,
			produtos.marcado='X',
			produtos.operador=operadorLanc,
			produtos.lote=entradas.lote,
			produtos.vencimento=entradas.vencimento,
			produtos.datafabricacao=entradas.datafabricacao,
			produtos.qtdprateleiras=produtos.qtdprateleiras+(SELECT SUM(entradas.qtdprateleiras) FROM entradas WHERE entradas.codigo=produtos.codigo AND entradas.numero=numeroEntrada) 
			WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
			AND produtos.CodigoFilial=filial AND entradas.Lancada<>'X';
			UPDATE entradas,produtosfilial AS produtos 
			SET entradas.quantidadeanterior=produtos.qtdanterior,
			entradas.quantidadeatualizada=produtos.quantidade,
			entradas.operacao=' .   '
			 WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
			 AND produtos.CodigoFilial=filial ;
		END IF;
	END IF;
	
	
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
 AND entradagrade.origem='Entrada';
 
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
	UPDATE produtos,produtoscomposicao,producao 
                       SET produtos.qtdproducao=produtos.qtdproducao-(produtoscomposicao.quantidade*(SELECT SUM(producao.quantidadeentrada) FROM producao WHERE producao.codigo=produtoscomposicao.codigo AND numero=numeroProducao) )                       
                       WHERE produtoscomposicao.codigomateria=produtos.codigo AND producao.numero=numeroProducao
                        AND produtoscomposicao.codigo=producao.codigo 
                        AND producao.quantidadeentrada>0;
	END IF;
	IF (filial<>"00001") THEN 
	UPDATE produtosfilial AS produtos,produtoscomposicao,producao 
                       SET produtos.qtdproducao=produtos.qtdproducao-(produtoscomposicao.quantidade*(SELECT SUM(producao.quantidadeentrada) FROM producao WHERE producao.codigo=produtoscomposicao.codigo AND numero=numeroProducao))
                       WHERE produtoscomposicao.codigomateria=produtos.codigo AND producao.numero=numeroProducao
                        AND produtoscomposicao.codigo=producao.codigo 
                        AND producao.quantidadeentrada>0;
	END IF;
	
 
	UPDATE contproducao SET finalizado='S',
	datafinalizado=CURRENT_DATE,operadorfinalizacao=operadorLanc
	WHERE numero=numeroProducao;
	
	
	IF (filial="00001") THEN 
	UPDATE produtos,producaomovmateria
                       SET producaomovmateria.grupo=produtos.grupo,producaomovmateria.subgrupo=produtos.subgrupo
                       WHERE producaomovmateria.idproducao=numeroProducao
                        AND producaomovmateria.codigomateria=produtos.codigo
                        AND producaomovmateria.codigoFilial=produtos.CodigoFilial;
	END IF;
	IF (filial<>"00001") THEN 
		UPDATE produtosfilial AS produtos,producaomovmateria
                       SET producaomovmateria.grupo=produtos.grupo,producaomovmateria.subgrupo=produtos.subgrupo
                       WHERE producaomovmateria.idproducao=numeroProducao
                        AND producaomovmateria.codigomateria=produtos.codigo
                        AND producaomovmateria.codigoFilial=produtos.CodigoFilial;
	END IF;
	
	
	
 END IF;
 
 IF (pedCompra>0) THEN
 CALL ProcessarPedidoCompra(numeroEntrada,pedCompra,"S");
 END IF;
 
 UPDATE moventradas SET lancada="X",DATA=CURRENT_DATE,horaentrada=CURRENT_TIME WHERE numero=numeroEntrada;
 UPDATE entradas SET lancada="X" WHERE numero=numeroEntrada;
 UPDATE entradas SET cfopentrada=(SELECT cfopentrada FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada AND cfopentrada IS NULL;
 UPDATE entradas SET dataentrada=(SELECT dataentrada FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada;
 
 UPDATE entradas SET fornecedor=(SELECT fornecedor FROM moventradas WHERE numero=numeroEntrada LIMIT 1),idfornecedor=(SELECT codigofornecedor FROM moventradas WHERE numero=numeroEntrada LIMIT 1) WHERE numero=numeroEntrada;
 
 UPDATE entradas SET icmsst=0,bcicmsst=0,entradas.valoricmsST=0 WHERE tributacao<>"010" AND tributacao<>"030" AND tributacao<>"070" AND numero=numeroEntrada;
  
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
UPDATE entradas SET ratdesconto=IFNULL(TRUNCATE((totalitem)*  ( (descontoNFe*100/totalBrutoNFe) /100 ) ,2),0) 
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
END IF; 
IF (filial="00001") THEN 
		UPDATE produtos,produtoscomposicao 
			       SET produtoscomposicao.custo=produtos.custo,
			       produtoscomposicao.custototal=(produtoscomposicao.quantidade*produtoscomposicao.custo)  
			       WHERE produtoscomposicao.codigomateria=produtos.codigo AND produtoscomposicao.codigofilial=produtos.CodigoFilial;
			     
	END IF;
	IF (filial<>"00001") THEN 
		UPDATE produtosfilial AS produtos,produtoscomposicao 
                       SET produtoscomposicao.custo=produtos.custo,
		       produtoscomposicao.custototal=(produtoscomposicao.quantidade*produtoscomposicao.custo)  
                       WHERE produtoscomposicao.codigomateria=produtos.codigo AND produtoscomposicao.codigofilial=produtos.CodigoFilial;                     
	END IF;
	
	
	UPDATE produtos AS p, entradas AS e 
	SET e.unidade = p.unidade 
	WHERE (e.unidade IS NULL || e.unidade = "")
	AND p.codigo = e.codigo
	AND e.codigofilial = filial
	AND e.numero = numeroEntrada;
	
	
	
	IF((SELECT mascarafiscal FROM configfinanc WHERE codigoFilial = filial) = 'S' AND (SELECT importada FROM moventradas WHERE numero = numeroEntrada) = 'S')THEN
		
		UPDATE produtos AS p, entradas AS e 
		SET p.pisentrada = e.pis,
		p.cofinsentrada = e.cofins,
		p.cstcofinsEntrada = e.cstcofins,
		p.cstpisEntrada = e.cstpis,
		p.cfopentrada = e.cfopentrada
		WHERE p.codigo = e.codigo
		AND e.codigofilial = filial
		AND e.numero = numeroEntrada
		AND e.codigoMascaraFiscal > 0;
	END IF; 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `QuitarDebitoCliente` */

/*!50003 DROP PROCEDURE IF EXISTS  `QuitarDebitoCliente` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `QuitarDebitoCliente`(IN codigoCliente INT,IN doc INT,IN ipTerminal VARCHAR(15),IN filial VARCHAR(5))
BEGIN
DECLARE valorRestanteAV REAL DEFAULT 0;
 DECLARE valorAbatidoAV REAL DEFAULT 0;
IF ((SELECT contdocs.dpfinanceiro FROM contdocs WHERE contdocs.documento=doc)="Rec renegoc") THEN
 UPDATE caixas SET 
 documento=doc,codigofilial=filial,
 tipopagamento="RN",
 dpfinanceiro=(SELECT contdocs.dpfinanceiro FROM contdocs WHERE contdocs.documento=doc),
 operador=(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc),
 classe=(SELECT contdocs.classe FROM contdocs WHERE contdocs.documento=doc),
 vendedor=(SELECT contdocs.vendedor FROM contdocs WHERE contdocs.documento=doc),
 historico=(SELECT contdocs.dpfinanceiro FROM contdocs WHERE contdocs.documento=doc),
 nome=(SELECT contdocs.nome FROM contdocs WHERE contdocs.documento=doc)
 WHERE  caixas.enderecoip=ipTerminal
 AND codigofilial=filial;
INSERT INTO crmovclientes (codigofilial,usuario,documento,nome,codigo,datacompra,
 vencimento,datacalcjuros,parcela,nrparcela,valor,valoratual,dependente,
 vendedor,classe,valorcorrigido,vrcapitalrec,porconta,valorpago,
 jurospago,jurosacumulado,ultvaloratual,encargos,dpfinanceiro,cpfcnpj) 
 SELECT codigofilial,operador,documento,nome,codigocliente,CURRENT_DATE,vencimento,vencimento,
 (SELECT nrparcelas FROM contdocs WHERE documento=doc),
 nrparcela,valor,valor,(SELECT dependente FROM contdocs WHERE documento=doc),vendedor,classe,valor,
 0,0,0,0,0,valor,
 (SELECT IFNULL( (encargos/nrparcelas) ,0) FROM contdocs WHERE documento=doc),
 dpfinanceiro,
 (SELECT IF(cpf<>'',cpf,cnpj) FROM clientes WHERE codigo=(SELECT codigocliente FROM contdocs WHERE documento=doc))
 FROM caixas WHERE enderecoip=ipTerminal
 AND tipopagamento='RN';
  
 CALL GerarNumeroBoleto(filial,doc);
 
 END IF;
 
 
 UPDATE caixas SET 
 documento=doc,codigofilial=filial,
  operador=(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc),
 classe=(SELECT contdocs.classe FROM contdocs WHERE contdocs.documento=doc),
 vendedor=(SELECT contdocs.vendedor FROM contdocs WHERE contdocs.documento=doc),
 historico=(SELECT contdocs.dpfinanceiro FROM contdocs WHERE contdocs.documento=doc),
 nome=(SELECT contdocs.nome FROM contdocs WHERE contdocs.documento=doc),
 cobrador = (SELECT cobrador FROM crmovclientes WHERE codigo=codigoCliente AND ip=ipTerminal AND quitado="S" LIMIT 1)
 WHERE  caixas.enderecoip=ipTerminal
 AND codigofilial=filial;
 
 
  
 UPDATE caixas SET 
 dpfinanceiro=(SELECT contdocs.dpfinanceiro FROM contdocs WHERE contdocs.documento=doc)
 WHERE  caixas.enderecoip=ipTerminal AND (dpfinanceiro IS NULL OR dpfinanceiro="")
 AND codigofilial=filial;
 
 UPDATE caixas SET 
 vrdesconto=(SELECT contdocs.desconto FROM contdocs WHERE contdocs.documento=doc),
 vrjuros=(SELECT contdocs.vrjuros FROM contdocs WHERE contdocs.documento=doc),
 cobrador = (SELECT cobrador FROM crmovclientes WHERE codigo=codigoCliente AND ip=ipTerminal AND quitado="S" LIMIT 1),
 filialorigem = (SELECT crmovclientes.CodigoFilial FROM crmovclientes WHERE codigo=codigoCliente AND ip=ipTerminal AND quitado="S" LIMIT 1),
 cpfcnpjch = (SELECT contdocs.ecfConsumidor FROM contdocs WHERE documento=doc LIMIT 1),
 estornado='N'
 WHERE caixas.enderecoip=ipTerminal AND codigofilial=filial LIMIT 1;
 
 
 UPDATE caixas SET  
 valortarifabloquete = (SELECT contasbanco.taxarecbloquete FROM contasbanco WHERE contasbanco.conta= (SELECT filiais.conta FROM filiais WHERE codigofilial=filial LIMIT 1)  )  
 WHERE caixas.enderecoip=ipTerminal AND codigofilial=filial AND valortarifabloquete=0 AND tipopagamento="BL" LIMIT 1;
 
 INSERT INTO movcartoes (codigofilial,documento,cartao,numero,DATA,
 vencimento,valor,operador,dpfinanceiro,encargos,nome)
 SELECT 	codigofilial,documento,cartao,numerocartao,CURRENT_DATE,vencimento,
 valor,operador,dpfinanceiro,
 (SELECT encargos FROM contdocs WHERE documento=doc),
 '0'
 FROM 	caixas
 WHERE 	enderecoip=ipTerminal AND codigofilial=filial
 AND 	tipopagamento IN ('CA','FI','TI','FN');
 
 INSERT INTO cheques (codigofilial,documento,banco,cheque,agencia,DATA,
 valor,valorcheque,vencimento,cliente,codigocliente,nomecheque,dpfinanceiro,
 cpf,cpfcheque,telefone,encargos) 
 SELECT codigofilial,documento,banco,cheque,agencia,DATA,valor,valorcheque,vencimento,
 nome,codigocliente,nomecheque,dpfinanceiro,
 (SELECT IF(cpf<>'',cpf,cnpj) FROM clientes WHERE codigo=(SELECT codigocliente FROM contdocs WHERE documento=doc)),	
 cpfcnpjch,(SELECT telefone FROM clientes WHERE codigo=(SELECT codigocliente FROM contdocs WHERE documento=doc)),
 (SELECT encargos FROM contdocs WHERE documento=doc)
 FROM caixas WHERE enderecoip=ipTerminal AND codigofilial=filial
 AND tipopagamento='CH';
 
 UPDATE crmovclientes SET
 ultvaloratual=valoratual,
 valoratual= valorcorrigido-valorpago,
 valorcorrigido= Valorcorrigido-valorpago,
 datapagamento = CURRENT_DATE,
 ultvencimento=datacalcjuros,
 datacalcjuros = CURRENT_DATE,
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
 sequencia = IF(LEFT(sequencia,1)="0",CONCAT('s',doc,'s'),CONCAT(sequencia,doc,'s'))
 WHERE codigo=codigoCliente
 AND quitado='S'
 AND valoratual>0
 AND ip=ipTerminal;
 
 INSERT INTO caixa (horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,DATA,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch,estornado,contabancaria)  
 SELECT horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,DATA,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch,estornado,contabancaria 
 FROM caixas 
 WHERE enderecoip=ipTerminal AND codigofilial=filial;
 UPDATE contdocs SET concluido='S',hora=CURRENT_TIME WHERE documento=doc;
 UPDATE crmovclientes SET quitado='N' 
 WHERE codigo=codigoCliente;	
 
  IF ( (SELECT COUNT(1) FROM caixas WHERE tipopagamento="DC" AND enderecoip=ipTerminal)>0) THEN       
	INSERT INTO movcontasbanco (codigofilial,conta, movimento,
       valorcredito, DATA, historico,operador, tipo, codigo, nome, cheque) VALUES (
        filial,(SELECT contabancaria FROM caixas WHERE codigofilial=filial AND tipopagamento="DC" AND enderecoip=ipTerminal LIMIT 1),
        'credito',(SELECT SUM(valor) FROM caixas WHERE enderecoip=ipTerminal AND tipopagamento="DC"),
	CURRENT_DATE,'RECEBIMENTO DE CLIENTE',
	(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc),
	'CLIENTE',	
	(SELECT contdocs.codigocliente FROM contdocs WHERE contdocs.documento=doc),
	(SELECT contdocs.nome FROM contdocs WHERE contdocs.documento=doc),	
	""
	); 
 END IF;
 
 IF ( (SELECT COUNT(1) FROM caixas WHERE tipopagamento="AV" AND dpfinanceiro="CreditoAV" AND enderecoip=ipTerminal)>0) THEN 
     INSERT INTO contaspagar (codigofilial,documento,empresa,valor,DATA,datadocumento,vencimento,historico,       
    cheque, nrparcela,
    codigocliente,nomecliente )
    VALUES (filial,doc,"",(SELECT SUM(valor) FROM caixas WHERE enderecoip=ipTerminal AND tipopagamento="AV"),
	CURRENT_DATE,CURRENT_DATE,CURRENT_DATE,CONCAT("Recebimento AV CLIENTE PAG. EM CH Nr. ",(SELECT caixas.cheque FROM caixas WHERE tipopagamento="AV" AND enderecoip=ipTerminal LIMIT 1)),
	(SELECT caixas.cheque FROM caixas WHERE tipopagamento="AV" AND enderecoip=ipTerminal LIMIT 1),
	"1/1",
	(SELECT contdocs.codigocliente FROM contdocs WHERE contdocs.documento=doc),
	(SELECT contdocs.nome FROM contdocs WHERE contdocs.documento=doc)); 
 END IF;
	
	
IF ( (SELECT COUNT(1) FROM caixas WHERE tipopagamento="AV" AND dpfinanceiro="RecebimentoAV" AND enderecoip=ipTerminal)>0) THEN 
SET valorRestanteAV = (SELECT SUM(valor) FROM caixas WHERE tipopagamento="AV" AND dpfinanceiro="RecebimentoAV" AND enderecoip=ipTerminal);
WHILE valorRestanteAV>0 DO
SET valorAbatidoAV = (SELECT valor FROM contaspagar  WHERE codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE documento=doc AND valor>0 LIMIT 1)  AND  quitado="N"  ORDER BY codigo LIMIT 1);
UPDATE contaspagar SET valor=valor-IF(valor<valorRestanteAV,valor,valorRestanteAV),
historico=CONCAT('AV de cliente no recebimento  doc: ',doc),
datapagamento=CURRENT_DATE,
usuario=(SELECT contdocs.operador FROM contdocs WHERE documento=doc LIMIT 1) 
WHERE contaspagar.codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE documento=doc LIMIT 1)
AND quitado='N' ORDER BY codigo LIMIT 1;
SET valorRestanteAV = valorRestanteAV - valorAbatidoAV;
 END WHILE;   
 
UPDATE contaspagar SET quitado="S" WHERE valor<=0 AND 
contaspagar.codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE documento=doc LIMIT 1);
 UPDATE clientes,contaspagar SET clientes.creditoav=(SELECT SUM(valor) FROM contaspagar
 WHERE codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE documento=doc LIMIT 1) AND quitado="N" AND cancelado="N") 
WHERE clientes.Codigo=(SELECT contdocs.codigocliente FROM contdocs WHERE documento=doc LIMIT 1);
 END IF; 
	
 
DELETE FROM caixas WHERE enderecoip=ipTerminal AND codigofilial=filial;
 UPDATE clientes SET ultvrpago=(SELECT contdocs.totalbruto FROM contdocs 
 WHERE contdocs.documento=doc),ultpagamento=CURRENT_DATE,
 debito=debito-(SELECT contdocs.totalbruto FROM contdocs WHERE contdocs.documento=doc)-(SELECT contdocs.vrjuros FROM contdocs WHERE contdocs.documento=doc LIMIT 1)
 WHERE codigo=codigoCliente;
 
  UPDATE contdocs SET 
 EADr06=MD5(CONCAT(ecffabricacao,ncupomfiscal,contadornaofiscalGNF,contadordebitocreditoCDC,DATA,coognf,tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,davnumero,DATA,total))
 WHERE documento=doc;
 
  CALL AtualizarQdtRegistros();
 
 CALL atualizarDebitoCliente(codigoCliente,(SELECT fatjurdia FROM configfinanc WHERE codigofilial=filial LIMIT 1),filial);
 END */$$
DELIMITER ;

/* Procedure structure for procedure `ReservarPreVenda` */

/*!50003 DROP PROCEDURE IF EXISTS  `ReservarPreVenda` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `ReservarPreVenda`(in filial varchar(5),in codigoProduto varchar(20),in quantidade decimal(10,3))
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

/* Procedure structure for procedure `AberturaDia` */

/*!50003 DROP PROCEDURE IF EXISTS  `AberturaDia` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `AberturaDia`(IN filial VARCHAR(5),IN dataAtual DATE,IN hora TIME,IN nrFabricacaoECF VARCHAR(20))
BEGIN
UPDATE produtos SET datafinalestoque=dataAtual,horafinalestoque=hora,ecffabricacao=nrFabricacaoECF WHERE codigofilial=filial AND indicadorproducao="T" AND codigofilial = filial;
UPDATE produtosfilial SET datafinalestoque=dataAtual,horafinalestoque=hora,ecffabricacao=nrFabricacaoECF WHERE codigofilial=filial AND indicadorproducao="T" AND codigofilial = filial;
  
UPDATE produtos SET saldofinalestoque=quantidade,EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,datafinalestoque,horafinalestoque,ecffabricacao)),EADP2relacaomercadoria=MD5(CONCAT(codigo,descricao,icms,precovenda,precoatacado,unidade,indicadorarredondamentotruncamento,indicadorproducao,tributacao)) WHERE codigofilial=filial AND indicadorproducao="T" AND codigofilial = filial;
UPDATE produtosfilial SET saldofinalestoque=quantidade,EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,datafinalestoque,horafinalestoque,ecffabricacao)),EADP2relacaomercadoria=MD5(CONCAT(codigo,descricao,icms,precovenda,precoatacado,unidade,indicadorarredondamentotruncamento,indicadorproducao,tributacao)) WHERE codigofilial=filial AND indicadorproducao="T" AND codigofilial = filial; 
  
UPDATE produtos SET eade1=MD5(CONCAT(datafinalestoque,horafinalestoque,ecffabricacao,codigo,descricao,unidade,saldofinalestoque)) WHERE codigofilial=filial AND indicadorproducao="T" AND codigofilial = filial;
UPDATE produtosfilial SET eade1=MD5(CONCAT(datafinalestoque,horafinalestoque,ecffabricacao,codigo,descricao,unidade,saldofinalestoque))  WHERE codigofilial=filial AND indicadorproducao="T" AND codigofilial = filial; 
UPDATE produtos AS p SET 
p.eade3 = MD5(CONCAT(p.datafinalestoque, p.horafinalestoque, 
(SELECT fabricacaoECF FROM r01 WHERE fabricacaoECF = nrFabricacaoECF LIMIT 1),
(SELECT tipoECF FROM r01 WHERE fabricacaoECF = nrFabricacaoECF LIMIT 1), 
(SELECT marcaECF FROM r01 WHERE fabricacaoECF = nrFabricacaoECF LIMIT 1),
(SELECT modeloECF FROM r01 WHERE fabricacaoECF = nrFabricacaoECF LIMIT 1))) WHERE codigofilial = filial;
CALL CriarInventario(filial,"");
CALL AtualizarQdtRegistros(); 
 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AjustarCamposNulos` */

/*!50003 DROP PROCEDURE IF EXISTS  `AjustarCamposNulos` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `AjustarCamposNulos`()
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
update produtos set indicadorproducao="T" where (indicadorproducao="" or indicadorproducao is null);
UPDATE produtosfilial SET indicadorproducao="T" WHERE (indicadorproducao="" OR indicadorproducao IS NULL);
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AjustarTributos` */

/*!50003 DROP PROCEDURE IF EXISTS  `AjustarTributos` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `AjustarTributos`(
  IN acao VARCHAR (20),
  IN dataInicio DATE,
  IN dataFinal DATE,
  IN codigoFilial VARCHAR (5),
  IN numeroInventario INT
)
BEGIN
  DECLARE _anoinventario VARCHAR (4) ;
  DECLARE _numeroinventario VARCHAR (4) ;
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
    LIMIT 1) 
    AND moventradas.modeloNF = "55" ;
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
  IF (acao = "ajustarPISCOFINS") 
  THEN 
 
 UPDATE 
    produtos 
  SET
    tributacaocofins = tributacaopis 
  WHERE tributacaopis <> tributacaocofins ;
 
  UPDATE 
    produtos 
  SET
    codigosuspensaocofins = codigosuspensaopis ;
 
  UPDATE 
    produtosfilial 
  SET
    codigosuspensaocofins = codigosuspensaopis ;
 
  UPDATE 
    vendaarquivo AS tabelaVenda,
    produtos 
  SET
    tabelaVenda.cstpis = produtos.tributacaoPIS,
    tabelaVenda.pis = produtos.PIS,
    tabelaVenda.cstcofins = produtos.tributacaoCOFINS,
    tabelaVenda.cofins = produtos.COFINS 
  WHERE tabelaVenda.codigo = produtos.codigo 
    AND tabelaVenda.codigofilial = codigoFilial 
    AND tabelaVenda.DATA BETWEEN dataInicio 
    AND dataFinal ;
 
  UPDATE 
    vendanf AS tabelaVenda,
    produtos 
  SET
    tabelaVenda.cstpis = produtos.tributacaoPIS,
    tabelaVenda.pis = produtos.PIS,
    tabelaVenda.cstcofins = produtos.tributacaoCOFINS,
    tabelaVenda.cofins = produtos.COFINS 
  WHERE tabelaVenda.codigo = produtos.codigo 
    AND tabelaVenda.codigofilial = codigoFilial 
    AND tabelaVenda.DATA BETWEEN dataInicio 
    AND dataFinal ;
 
  UPDATE 
    vendanf AS v,
    produtos AS p 
  SET
    v.cstpis = p.cstpisEntrada,
    v.cstcofins = p.cstcofinsEntrada,
    v.pis = p.pisentrada,
    v.cofins = p.cofinsentrada 
  WHERE v.codigo = p.codigo 
    AND MID(cfop, 1, 1) < 4 
    AND v.DATA BETWEEN dataInicio 
    AND dataFinal 
    AND v.codigofilial = codigoFilial ;
  UPDATE 
    entradas,
    produtos 
  SET
    entradas.cstpis = produtos.cstpisEntrada,
    entradas.pis = produtos.pisentrada,
    entradas.cstcofins = produtos.cstcofinsEntrada,
    entradas.cofins = produtos.cofinsentrada 
  WHERE entradas.codigo = produtos.codigo 
    AND entradas.codigofilial = codigoFilial 
 AND entradas.dataentrada BETWEEN dataInicio 
    AND dataFinal ;
 
  UPDATE 
    vendanf 
  SET
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfop = '1.102' ;
  UPDATE 
    vendanf 
  SET
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfop = '1.202' ;
  UPDATE 
    vendanf 
  SET
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfop = '2.202' ;
  UPDATE 
    vendanf 
  SET
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfop = '2.102' ;
 
 UPDATE 
    vendanf 
  SET
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfop = '1.411' ;
 
 UPDATE 
    vendanf 
  SET
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfop = '2.411' ;
 
  UPDATE 
    entradas 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfopentrada = '1.910' ;
   UPDATE 
    entradas 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfopentrada = '2.910' ;
 
 UPDATE 
    vendanf 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfop = '1.910' AND DATA BETWEEN dataInicio AND dataFinal AND vendanf.codigofilial = codigoFilial;
 
  UPDATE 
    vendanf 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfop = '2.910' AND DATA BETWEEN dataInicio AND dataFinal AND vendanf.codigofilial = codigoFilial;
 
  
 UPDATE 
    vendanf 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '49',
    cstcofins = '49' 
  WHERE cfop = '5.905' AND DATA BETWEEN dataInicio AND dataFinal AND vendanf.codigofilial = codigoFilial;
 
  UPDATE 
    vendanf 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '49',
    cstcofins = '49' 
  WHERE cfop = '6.905' AND DATA BETWEEN dataInicio AND dataFinal AND vendanf.codigofilial = codigoFilial;
 
 
  UPDATE 
    vendanf 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '49',
    cstcofins = '49' 
  WHERE cfop = '5.929' AND DATA BETWEEN dataInicio AND dataFinal AND vendanf.codigofilial = codigoFilial;
 
  UPDATE 
    vendanf 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '49',
    cstcofins = '49' 
  WHERE cfop = '6.929' AND DATA BETWEEN dataInicio AND dataFinal AND vendanf.codigofilial = codigoFilial;
 
 
  UPDATE 
    entradas 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfopentrada = '1.556' ;
  UPDATE 
    entradas 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfopentrada = '1.906';
   UPDATE 
    entradas 
  SET
    pis = '0',
    cofins = '0',
    cstpis = '98',
    cstcofins = '98' 
  WHERE cfopentrada = '2.906';
  END IF ;
  IF (acao = "Inventario") 
  THEN 
  SELECT 
    ano INTO _anoinventario 
  FROM
    continventario 
  WHERE inc = numeroInventario ;
  
  SELECT 
    numero INTO _numeroinventario 
  FROM
    continventario 
  WHERE inc = numeroInventario ;
  
  SELECT 
    numero INTO codigoFilial 
  FROM
    continventario 
  WHERE inc = numeroInventario ;
  UPDATE 
    produtosinventario AS i,
    produtos AS p 
  SET
    i.ncm = p.ncm,
    i.ncmespecie = p.ncmespecie 
  WHERE i.codigo = p.codigo 
    AND i.codigofilial = codigoFilial 
    AND i.anoinventario = _anoinventario 
    AND i.inventarionumero = _numeroinventario ;
  END IF ;
  IF(acao = "ajustarICMS") 
  THEN UPDATE 
    vendaarquivo AS tabelaVenda,
    produtos 
  SET
    tabelaVenda.icms = produtos.icms,
    tabelaVenda.tributacao = produtos.tributacao,
    tabelaVenda.origem = 0 
  WHERE tabelaVenda.codigo = produtos.codigo 
    AND tabelaVenda.codigofilial = codigoFilial 
    AND tabelaVenda.DATA BETWEEN dataInicio 
    AND dataFinal ;
  UPDATE 
    vendanf 
  SET
    origem = 0 
  WHERE (origem <> 0 || origem = "")
    AND DATA BETWEEN dataInicio 
    AND dataFinal ;
  END IF ;
  IF(acao = "ajustarCFOPSAIDA") 
  THEN UPDATE 
    vendaarquivo AS tabelaVenda,
    produtos 
  SET
    tabelaVenda.cfop = produtos.cfopsaida 
  WHERE tabelaVenda.codigo = produtos.codigo 
    AND tabelaVenda.codigofilial = codigoFilial 
    AND tabelaVenda.DATA BETWEEN dataInicio 
    AND dataFinal ;
  UPDATE 
    vendanf 
  SET
    origem = 0 
  WHERE origem <> 0 
    AND DATA BETWEEN dataInicio 
    AND dataFinal ;
 
  UPDATE 
    vendanf 
  SET
    tributacao = "00"
  WHERE tributacao = "0" 
    AND DATA BETWEEN dataInicio 
    AND dataFinal ;
  END IF ;
  
END */$$
DELIMITER ;

/* Procedure structure for procedure `ajustarValores` */

/*!50003 DROP PROCEDURE IF EXISTS  `ajustarValores` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `ajustarValores`(IN filial CHAR(5),IN dataDocumento DATE)
BEGIN
DECLARE done INT DEFAULT 0;
DECLARE _documento INT DEFAULT 0;
DECLARE _quantidade INT DEFAULT 0;
	DECLARE cursorDocumentos CURSOR FOR SELECT documento,COUNT(1) AS quantidade FROM contdocs AS c WHERE c.dpfinanceiro = "recebimento" AND c.codigofilial = filial AND DATA = dataDocumento GROUP BY c.codigocliente,c.total,c.DATA
HAVING quantidade > 1;
	
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
	
	OPEN cursorDocumentos;
	REPEAT
	FETCH cursorDocumentos INTO _documento,_quantidade;
		INSERT INTO documentos(documento) VALUES (_documento);
		DELETE FROM contdocs WHERE documento = _documento;
		DELETE FROM caixa WHERE documento = _documento;
		DELETE FROM caixaarquivo WHERE documento = _documento;
		DELETE FROM crmovclientes WHERE documento = _documento;	
	UNTIL done END REPEAT;
	CLOSE cursorDocumentos;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AjustarVendatmp` */

/*!50003 DROP PROCEDURE IF EXISTS  `AjustarVendatmp` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `AjustarVendatmp`(IN filial CHAR(5))
BEGIN
	UPDATE produtos AS p, vendatmp AS v 
	SET v.unidade = p.unidade 
	WHERE (v.unidade IS NULL || v.unidade = "")
	AND p.codigo = v.codigo;
	
	UPDATE vendatmp SET cfop = '5.102' ;
	UPDATE vendatmp SET cfop = '5.405' WHERE icms = 0 AND tributacao = 60;
	UPDATE vendatmp SET TRIBUTACAO = 40 WHERE icms = 0 AND tributacao = '00';
	UPDATE vendatmp SET tributacao = '00' WHERE icms > 0 AND tributacao = 10;
	
	UPDATE vendatmp SET ratdesc = (total - 1) WHERE ratdesc >= (total + rateioencargos) ;
	UPDATE vendatmp SET TRIBUTACAO = '00' WHERE tributacao = '0';
	
	UPDATE vendanftmp SET cfop = '5.405' WHERE icms = 0 AND tributacao = 60 AND cfop = '5.102';
	UPDATE vendanftmp SET TRIBUTACAO = 40 WHERE icms = 0 AND tributacao = '00' ;
	UPDATE vendanftmp SET TRIBUTACAO = '00' WHERE tributacao = '0';
        
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ApagaNFe` */

/*!50003 DROP PROCEDURE IF EXISTS  `ApagaNFe` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `ApagaNFe`(
IN NFeOrigem BIGINT(9),
IN serieNFe VARCHAR(3),
 IN filial VARCHAR(5))
BEGIN
      IF  ( ( SELECT CbdStsRetCodigo FROM nfe012 WHERE cbdntfnumero=NFeOrigem AND CbdNtfSerie=serieNFe AND cbdcodigofilial=filial AND (CbdStsRetCodigo="100" OR CbdStsRetCodigo="102") LIMIT 1) IS NULL ) THEN                    
	DELETE FROM contnfsaida WHERE notafiscal=NFeOrigem AND serie=serieNFe AND codigofilial=filial;                                                  
        DELETE FROM vendanf WHERE notafiscal=NFeOrigem AND serienf=serieNFe AND codigofilial=filial;    
	DELETE FROM nfe012 WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001 WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001det WHERE CbdNtfNumero=NFeOrigem  AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
        DELETE FROM cbd001detadicoes WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detarma WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detcofins WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001deticmsnormalst WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detdi WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;	
	DELETE FROM cbd001detipi WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detmed WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001detpis WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;	
	DELETE FROM cbd001duplicatas WHERE CbdNtfNumero=NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001lacres WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001obsfisco WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001procref WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
        DELETE FROM cbd001reboque WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001vol WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	DELETE FROM cbd001pag WHERE CbdNtfNumero= NFeOrigem AND CbdNtfSerie=serieNFe AND CbdCodigoFilial=filial;
	
	END IF;  
    END */$$
DELIMITER ;

/* Procedure structure for procedure `aplicarTaxaFinanciamentoEntrada` */

/*!50003 DROP PROCEDURE IF EXISTS  `aplicarTaxaFinanciamentoEntrada` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `aplicarTaxaFinanciamentoEntrada`(IN numeroEntrada INT, IN filial CHAR(5),IN alterar CHAR(1),IN numeroTransf INT )
BEGIN
 
DECLARE _taxafinanciamento DOUBLE;
DECLARE margemfinanc DOUBLE; 
DECLARE precofinanc DOUBLE;
IF (numeroTransf=0) THEN
	IF (alterar="S") THEN
		IF (filial="00001") THEN
				SELECT taxafinanciamento INTO _taxafinanciamento FROM configfinanc WHERE codigofilial = filial;
				SET margemfinanc =   (SELECT (((entradas.precovenda + ((entradas.precovenda * _taxafinanciamento)/100)) - entradas.Custo) * 100) / entradas.custo FROM entradas,produtos
				WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
				AND produtos.codigofilial=filial);
				SET precofinanc = (SELECT entradas.precovenda + ((entradas.precovenda * _taxafinanciamento)/100) FROM entradas,produtos
				WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
				AND produtos.codigofilial=filial);
			
				
				IF(_taxafinanciamento>0)THEN
					UPDATE produtos,entradas SET produtos.precovenda = precofinanc, 
					produtos.margemlucro = margemfinanc,
					produtos.precosemfinanciamento=entradas.PrecoVenda,
					produtos.margemsemfinanciamento=entradas.MargemLucro,
					produtos.precovenda = precofinanc
					WHERE produtos.codigo=entradas.codigo 
					AND entradas.numero=numeroEntrada
					AND produtos.CodigoFilial=filial;
				END IF;
	
	
				IF((SELECT alterarprecofiliaisentrada FROM configfinanc WHERE codigofilial = '00001' LIMIT 1) = 'S')THEN
					
					IF(_taxafinanciamento>0)THEN
						UPDATE produtosfilial AS produtos,entradas SET produtos.precovenda = precofinanc, 
						produtos.margemlucro = margemfinanc,
						produtos.precosemfinanciamento=entradas.PrecoVenda,
						produtos.margemsemfinanciamento=entradas.MargemLucro,
						produtos.precovenda = precofinanc
						WHERE produtos.codigo=entradas.codigo 
						AND entradas.numero=numeroEntrada
						AND produtos.CodigoFilial=filial;
					END IF;
			
				END IF;
		END IF;	#fim filial matriz
		IF (filial<>"00001") THEN
			
			SELECT taxafinanciamento INTO _taxafinanciamento FROM configfinanc WHERE codigofilial = filial;
	
			SET margemfinanc =   (SELECT (((entradas.precovenda + ((entradas.precovenda * _taxafinanciamento)/100)) - entradas.Custo) * 100) / entradas.custo FROM entradas,produtosfilial AS produtos
			WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
			AND produtos.codigofilial=filial);
			
		
			SET precofinanc = (SELECT entradas.precovenda + ((entradas.precovenda * _taxafinanciamento)/100) FROM entradas,produtosfilial AS produtos
			WHERE produtos.codigo=entradas.codigo AND entradas.numero=numeroEntrada
			AND produtos.codigofilial=filial);
		
		
			IF(_taxafinanciamento>0)THEN
				UPDATE produtosfilial AS produtos,entradas SET produtos.precovenda = precofinanc, 
				produtos.margemlucro = margemfinanc,
				produtos.precosemfinanciamento=entradas.PrecoVenda,
				produtos.margemsemfinanciamento=entradas.MargemLucro,
				produtos.precovenda = precofinanc
				WHERE produtos.codigo=entradas.codigo 
				AND entradas.numero=numeroEntrada
				AND produtos.CodigoFilial=filial;
			END IF;
	
		END IF; 
	END IF; 
END IF; 
END */$$
DELIMITER ;

/* Procedure structure for procedure `arquivomorto` */

/*!50003 DROP PROCEDURE IF EXISTS  `arquivomorto` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `arquivomorto`(in tabela varchar(30))
BEGIN
DECLARE qtdRegistroOrigemCaixa INT DEFAULT 0;
DECLARE qtdRegistroDestinoCaixa INT DEFAULT 0;
declare qtdRegistroDestinoCaixaRepassado int default 0;
DECLARE qtdRegistroOrigemContdocs INT DEFAULT 0;
DECLARE qtdRegistroDestinoContdocs INT DEFAULT 0;
DECLARE qtdRegistroDestinoContdocsRepassado INT DEFAULT 0;
SET qtdRegistroOrigemCaixa =(SELECT COUNT(1) FROM caixaarquivo WHERE year(DATA)<=YEAR(CURRENT_DATE))-1 ;
SET qtdRegistroDestinoCaixa = (SELECT COUNT(1) FROM caixaarquivo_arq WHERE year(DATA)<=YEAR(CURRENT_DATE))-1;
SET qtdRegistroOrigemContdocs =(SELECT COUNT(1) FROM contdocs WHERE YEAR(DATA)<=YEAR(CURRENT_DATE))-1 ;
SET qtdRegistroDestinoContdocs = (SELECT COUNT(1) FROM contdocs_arq WHERE YEAR(DATA)<=YEAR(CURRENT_DATE))-1;
IF (tabela="caixa") THEN
	INSERT INTO caixaarquivo_arq SELECT * FROM caixaarquivo WHERE year(DATA)<=YEAR(CURRENT_DATE)-1;
	SET qtdRegistroDestinoCaixaRepassado = (SELECT COUNT(1) FROM caixaarquivo_arq WHERE YEAR(DATA)<=YEAR(CURRENT_DATE))-1;
	 IF ( qtdRegistroDestinoCaixaRepassado>qtdRegistroDestinoCaixa ) THEN 
		DELETE FROM caixaarquivo WHERE year(DATA)<=YEAR(CURRENT_DATE)-1;
	  END IF; 
END IF;
IF (tabela="contdocs") THEN
	INSERT INTO contdocs_arq SELECT * FROM contdocs WHERE YEAR(DATA)<=YEAR(CURRENT_DATE)-1;
	SET qtdRegistroDestinoContdocsRepassado = (SELECT COUNT(1) FROM contdocs_arq WHERE YEAR(DATA)<=YEAR(CURRENT_DATE))-1;
	 IF ( qtdRegistroDestinoContdocsRepassado>qtdRegistroDestinoContdocs ) THEN 
		DELETE FROM contdocs WHERE YEAR(DATA)<=YEAR(CURRENT_DATE)-1;
	  END IF; 
END IF;
IF (tabela="ApagarInventario") THEN
		DELETE FROM produtosinventario WHERE inventarioencerrado="N" and anoinventario<YEAR(CURRENT_DATE);
	delete from continventario where continventario.encerrado="N" and continventario.ano<YEAR(CURRENT_DATE);
END IF;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarDebitoCliente` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarDebitoCliente` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `AtualizarDebitoCliente`(in codCliente int,in taxaJuros double,in filial varchar(5))
BEGIN
 IF (codCliente>0) THEN
UPDATE clientes SET dataultcarta='1899-12-30' WHERE dataultcarta IS NULL AND Codigo=codCliente;
UPDATE clientes SET ultvencimento='1899-12-30' WHERE ultvencimento IS NULL AND Codigo=codCliente;  
update crmovclientes set desconto=0 where codigo=codCliente and valoratual>0;
 
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
 UPDATE crmovclientes SET valoratual=0
  WHERE valoratual < 0 and codigo =codCliente;
  
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
 UPDATE crmovclientes SET valoratual=0
  WHERE valoratual < 0 ;
  
  
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

/*!50003 CREATE  PROCEDURE `AtualizarEstoqueOff`(in filial varchar(5))
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

/* Procedure structure for procedure `AtualizarEstoqueOffline` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarEstoqueOffline` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `AtualizarEstoqueOffline`(IN filial VARCHAR(5),IN documento INT)
BEGIN
	
	 UPDATE clientes SET
	 creditoprovisorio=0,ultcompra=CURRENT_DATE,
	 debito=debito+IFNULL((SELECT SUM(valor) FROM caixas WHERE tipopagamento='CR' AND caixas.documento=documento ),0),
	 debitoch=debitoch+IFNULL((SELECT SUM(valor) FROM caixas WHERE tipopagamento='CH' AND caixas.documento=documento),0),
	 saldo=credito-debito
	 WHERE codigo=(SELECT codigocliente FROM contdocs WHERE documento=documento LIMIT 1);
			 
	 UPDATE clientes SET valorcartaofidelidade=valorcartaofidelidade+(SELECT total FROM contdocs WHERE documento=documento LIMIT 1)
	 WHERE cartaofidelidade=(SELECT cartaofidelidade FROM contdocs WHERE documento=documento LIMIT 1) AND cartaofidelidade<>'';
	
	 UPDATE contdocs SET
	 EADr06=MD5(CONCAT(IFNULL(ecffabricacao,""),IFNULL(ncupomfiscal,""),IFNULL(contadornaofiscalGNF,""),IFNULL(contadordebitocreditoCDC,""),DATA,IFNULL(coognf,""),tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
	 EADRegistroDAV=MD5(CONCAT(IFNULL(ncupomfiscal,""),davnumero,DATA,total))
	 WHERE estoqueatualizado="N" AND contdocs.documento = documento;
	 
	UPDATE venda SET 
	venda.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,""))) WHERE venda.documento = documento;
	 
	  
	  UPDATE caixa SET 
	 eaddados=MD5(CONCAT(ecffabricacao,coo,ccf,gnf,ecfmodelo,valor,tipopagamento)) WHERE caixa.documento = documento;
	 
	 IF (filial="00001") THEN
	 UPDATE produtos,contdocs,venda
	 SET produtos.quantidade=produtos.quantidade-(SELECT SUM(venda.quantidade) FROM venda WHERE codigo=produtos.codigo AND venda.documento=contdocs.documento ),
	 contdocs.estoqueatualizado="S"
	 WHERE produtos.codigo=venda.codigo 
	 AND venda.documento=contdocs.documento
	 AND contdocs.estoqueatualizado="N"
	 AND produtos.codigo=venda.codigo
	 AND produtos.codigofilial=filial
	 AND contdocs.documento = documento;
	 END IF;
	 
	 IF (filial<>"00001") THEN
	 UPDATE produtosfilial,contdocs,venda
	 SET produtosfilial.quantidade=produtosfilial.quantidade-(SELECT SUM(venda.quantidade) FROM venda WHERE codigo=produtosfilial.codigo AND venda.documento=contdocs.documento ),
	 contdocs.estoqueatualizado="S"
	 WHERE produtosfilial.codigo=venda.codigo 
	 AND venda.documento=contdocs.documento
	 AND contdocs.estoqueatualizado="N"
	 AND produtosfilial.codigo=venda.codigo
	 AND produtosfilial.codigofilial=filial
	 AND contdocs.documento = documento;
	 END IF;
	CALL AtualizarQdtRegistros();
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarQdtRegistros` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarQdtRegistros` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `AtualizarQdtRegistros`()
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
 r01=(SELECT MD5(COUNT(*)) FROM r01),
 r02=(SELECT MD5(COUNT(*)) FROM r02),
 r03=(SELECT MD5(COUNT(*)) FROM r03),
 contdavos = (SELECT MD5(COUNT(*)) FROM contdavos), 
 vendadavos = (SELECT MD5(COUNT(*)) FROM vendadavos),
  filiais = (SELECT MD5(COUNT(*)) FROM filiais);
 
 
UPDATE quantidaderegistros SET 
 caixaDH = (SELECT MD5(COUNT(*)) FROM caixa WHERE tipopagamento = 'DH'),
 caixaCR = (SELECT MD5(COUNT(*)) FROM caixa WHERE tipopagamento = 'CR'),
 caixaCA = (SELECT MD5(COUNT(*)) FROM caixa WHERE tipopagamento = 'CA'),
 caixaCH = (SELECT MD5(COUNT(*)) FROM caixa WHERE tipopagamento = 'CH'),
 caixaOutros = (SELECT MD5(COUNT(*)) FROM caixa WHERE tipopagamento <> 'DH' AND tipopagamento <> 'CR' AND tipopagamento <> 'CA' AND tipopagamento <> 'CH' AND tipopagamento <> 'SI' AND tipopagamento <> 'SU');
END */$$
DELIMITER ;

/* Procedure structure for procedure `BaixarCP` */

/*!50003 DROP PROCEDURE IF EXISTS  `BaixarCP` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `BaixarCP`(IN filial VARCHAR(5),IN operadorBaixa VARCHAR(10),IN lancarCaixa CHAR(1) ,IN  valorJuros  DOUBLE,IN  contaBanco VARCHAR(10),IN nrCheque VARCHAR(10),IN ipTerminal VARCHAR(15) )
BEGIN
DECLARE done INT DEFAULT FALSE;
DECLARE seq INT DEFAULT 0;
DECLARE cursorSeq CURSOR FOR SELECT Codigo FROM contaspagar WHERE (marcado='S' OR marcado="X") AND cancelado='N' AND quitado='N' AND ip=ipTerminal;
DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
SET @interpoladorChave = CURRENT_TIME;
OPEN cursorSeq;
	read_loop: LOOP
		FETCH cursorSeq INTO seq;	
		IF done THEN
			LEAVE read_loop;
		END IF; 
   INSERT INTO movdespesas 
      (id,grupo,codigofilial,DATA,valor,conta,
      subconta,despesa,operador,
      historico,descricaoconta,descricaosubconta,
      sangria,contabancaria,cheque,interpolador,datafiscal) 
      VALUES ( 
      ipTerminal,(SELECT filiais.grupo FROM filiais WHERE codigofilial=filial),
      filial,CURRENT_DATE,(SELECT contaspagar.valor FROM contaspagar WHERE codigo=seq),
      (SELECT contaspagar.conta FROM contaspagar WHERE codigo=seq),
      (SELECT contaspagar.subconta FROM contaspagar WHERE codigo=seq),
      (SELECT contaspagar.despesa FROM contaspagar WHERE codigo=seq),
       operadorBaixa,
      (SELECT contaspagar.empresa FROM contaspagar WHERE codigo=seq),
      (SELECT contaspagar.descricao FROM contaspagar WHERE codigo=seq), 
      (SELECT contaspagar.descricaosubconta FROM contaspagar WHERE codigo=seq),      
      lancarCaixa,
      contaBanco,
      cheque,
      @interpoladorChave,
      CURRENT_DATE);       
IF (contaBanco<>'') THEN
	INSERT INTO movcontasbanco (codigofilial,conta, movimento,
        valordebito, DATA, historico,operador,cheque,interpolador) 
	VALUES (filial,contaBanco,'debito',(SELECT valor FROM contaspagar WHERE codigo=seq),
	CURRENT_DATE,
	(CONCAT( 'CP: ',(SELECT empresa FROM contaspagar WHERE codigo=seq),' doc: ',(SELECT documento FROM contaspagar WHERE codigo=seq))),
	operadorBaixa,
	nrCheque,
	@interpoladorChave);       
 
 
	UPDATE contaspagar SET contabancaria=contaBanco,cheque=nrCheque
	WHERE codigo=seq;
	
END IF;
END LOOP;
INSERT INTO dbpagamentos (codigofilial,valor,DATA,operador ) 
VALUES(filial,(SELECT IFNULL(SUM(valor),0) FROM contaspagar WHERE  (marcado='X' OR marcado='S') AND cancelado='N' AND quitado='N' AND ip=ipTerminal),CURRENT_DATE,operadorBaixa);
IF (valorJuros>0) THEN
 INSERT INTO movdespesas 
      (id,grupo,codigofilial,DATA,valor,conta,
      subconta,despesa,operador,
      historico,descricaoconta,descricaosubconta,
      sangria,contabancaria,cheque,interpolador,datafiscal) 
      VALUES ( 
      ipTerminal,(SELECT filiais.grupo FROM filiais WHERE codigofilial=filial),
      filial,CURRENT_DATE,valorJuros,
      (SELECT conta FROM configfinanc WHERE codigofilial=filial LIMIT 1),
      (SELECT subconta FROM configfinanc WHERE codigofilial=filial LIMIT 1),
      'S',
       operadorBaixa,
      'Origem: Contas a Pagar Juros',
      (SELECT despesas.descricao FROM despesas WHERE conta= (SELECT conta FROM configfinanc WHERE codigofilial=filial LIMIT 1) ), 
      (SELECT despesasub.descricao FROM despesasub WHERE despesasub.idconta = (SELECT conta FROM configfinanc WHERE codigofilial=filial LIMIT 1) AND despesasub.idsubconta=(SELECT subconta FROM configfinanc WHERE codigofilial=filial LIMIT 1) ),   
      "N",
      contaBanco,
      cheque,
      @interpoladorChave,
      CURRENT_DATE);       
END IF;
UPDATE contaspagar SET quitado='S',marcado='',datapagamento=CURRENT_DATE,interpolador=@interpoladorChave,
usuario=operadorBaixa 
WHERE (marcado='X' OR marcado='S') AND ip=ipTerminal AND quitado='N';
	CLOSE cursorSeq;
		
 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ChecarIntegridadeCaixa` */

/*!50003 DROP PROCEDURE IF EXISTS  `ChecarIntegridadeCaixa` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `ChecarIntegridadeCaixa`(in filial varchar(5),in _operador varchar(10))
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

/* Procedure structure for procedure `CriarTabelasTemp` */

/*!50003 DROP PROCEDURE IF EXISTS  `CriarTabelasTemp` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `CriarTabelasTemp`(IN tabela VARCHAR(10),IN filial VARCHAR(5),IN dataInicial DATE,IN dataFinal DATE)
BEGIN
IF (tabela="venda") THEN
DROP TABLE IF EXISTS `vendatmp`;
DROP TABLE IF EXISTS `vendanftmp`;
IF (filial<>"00000") THEN
CREATE TABLE `vendatmp` SELECT * FROM `vendaarquivo` WHERE codigofilial=filial AND DATA BETWEEN dataInicial AND datafinal AND modelodocfiscal = "2D";
CREATE TABLE `vendanftmp` SELECT * FROM `vendanf` WHERE codigofilial=filial AND DATA BETWEEN dataInicial AND datafinal;
END IF;
IF (tabela="venda" AND filial="00000") THEN
CREATE TABLE `vendatmp` SELECT * FROM `vendaarquivo` WHERE DATA BETWEEN dataInicial AND datafinal AND modelodocfiscal = "2D";
CREATE TABLE `vendanftmp` SELECT * FROM `vendanf` WHERE DATA BETWEEN dataInicial AND datafinal;
END IF;
END IF;
IF (tabela="caixa") THEN
DROP TABLE IF EXISTS `caixatmp`;
CREATE TABLE `caixatmp` SELECT * FROM `caixaarquivo` WHERE codigofilial=filial AND DATA BETWEEN dataInicial AND datafinal;
END IF;
CALL AjustarVendatmp(filial);
    END */$$
DELIMITER ;

/* Procedure structure for procedure `CriarInventario` */

/*!50003 DROP PROCEDURE IF EXISTS  `CriarInventario` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `CriarInventario`(IN filial VARCHAR(5),IN _operador VARCHAR(10))
BEGIN
DECLARE _anoInventario INT DEFAULT 0;
DECLARE _numeroInventario INT DEFAULT 0;
DECLARE cursorAno CURSOR FOR SELECT YEAR(CURRENT_DATE);
DECLARE cursorNumero CURSOR FOR SELECT IFNULL(MAX(numero)+1,1) FROM continventario WHERE codigofilial=filial AND YEAR(DATA)=YEAR(CURRENT_DATE);
OPEN cursorAno;
OPEN cursorNumero;
FETCH cursorAno INTO _anoInventario;
FETCH cursorNumero INTO _numeroInventario;
 
 IF (  ( SELECT CONCAT( MONTH( CURRENT_DATE),YEAR( CURRENT_DATE) ) ) > (SELECT IFNULL(CONCAT( MONTH( MAX(DATA)),YEAR( MAX(DATA)) ),0) FROM continventario WHERE codigofilial=filial)  ) THEN
	INSERT INTO continventario (ano,numero,encerrado,operador,DATA,total,codigofilial)
	VALUES (_anoInventario,_numeroInventario,"N",_operador,CURRENT_DATE,0,filial);
	SELECT "entrou";
	IF (filial='00001') THEN
		INSERT INTO produtosinventario
		(codigo,codigoinc,descecf,unidade,quantidade,qtddeposito,qtdprateleiras,qtdvencidos,qtdanterior,qtdultent,dataultent,qtdprovisoria,descprovisorio,qtdultpedido,DATA,
		dataultpedido,icms,ipi,grupo,subgrupo,custo,customedio,ultcusto,custototal,precovenda,dataultvenda,ultpreco,estminimo,codigobarras,situacao,tributacao,
		fornecedor,fabricante,embalagem,unidembalagem,descricao,codigoFilial,qtdretida,secao,margemsemfinanciamento,precosemfinanciamento,custofornecedor,qtdprevenda,precoatacado,
		grade,origem,pis,cofins,margemlucro,volumes,ncm,tributacaopis,tributacaocofins,saldofinalestoque,complementodescricao,cstpisentrada,cstcofinsentrada,pisentrada,cofinsentrada,
		codigosuspensaopis,codigosuspensaocofins,idfornecedor,custogerencial,aliquotaStFronteira,MVAFronteira,qtdproducao,margemlucroliquida) 
		SELECT codigo,codigoinc,descecf,unidade,quantidade,qtddeposito,qtdprateleiras,qtdvencidos,qtdanterior,qtdultent,dataultent,qtdprovisoria,descprovisorio,qtdultpedido,DATA,
		dataultpedido,icms,ipi,grupo,subgrupo,custo,customedio,ultcusto,custototal,precovenda,dataultvenda,ultpreco,estminimo,codigobarras,situacao,tributacao,
		fornecedor,fabricante,embalagem,unidembalagem,descricao,codigoFilial,qtdretida,secao,margemsemfinanciamento,precosemfinanciamento,custofornecedor,qtdprevenda,precoatacado,
		grade,origem,pis,cofins,margemlucro,volumes,ncm,tributacaopis,tributacaocofins,saldofinalestoque,complementodescricao,cstpisentrada,cstcofinsentrada,pisentrada,cofinsentrada,
		codigosuspensaopis,codigosuspensaocofins,idfornecedor,custogerencial,aliquotaStFronteira,MVAFronteira,qtdproducao,margemlucroliquida FROM produtos WHERE codigofilial=filial
		AND tipo='0 - Produto' GROUP BY codigo;
		UPDATE produtosinventario SET anoinventario=_anoInventario,inventarionumero=_numeroInventario,
		custototal=IFNULL(quantidade*customedio,0) WHERE codigofilial=filial AND anoinventario=0;	
	END IF;	
	
		IF (filial<>'00001') THEN
		INSERT INTO produtosinventario
		(codigo,codigoinc,descecf,unidade,quantidade,qtddeposito,qtdprateleiras,qtdvencidos,qtdanterior,qtdultent,dataultent,qtdprovisoria,descprovisorio,qtdultpedido,DATA,
		dataultpedido,icms,ipi,grupo,subgrupo,custo,customedio,ultcusto,custototal,precovenda,dataultvenda,ultpreco,estminimo,codigobarras,situacao,tributacao,
		fornecedor,fabricante,embalagem,unidembalagem,descricao,codigoFilial,qtdretida,secao,margemsemfinanciamento,precosemfinanciamento,custofornecedor,qtdprevenda,precoatacado,
		grade,origem,pis,cofins,margemlucro,volumes,ncm,tributacaopis,tributacaocofins,saldofinalestoque,complementodescricao,cstpisentrada,cstcofinsentrada,pisentrada,cofinsentrada,
		codigosuspensaopis,codigosuspensaocofins,idfornecedor,custogerencial,aliquotaStFronteira,MVAFronteira,qtdproducao,margemlucroliquida) 
		SELECT codigo,codigoinc,descecf,unidade,quantidade,qtddeposito,qtdprateleiras,qtdvencidos,qtdanterior,qtdultent,dataultent,qtdprovisoria,descprovisorio,qtdultpedido,DATA,
		dataultpedido,icms,ipi,grupo,subgrupo,custo,customedio,ultcusto,custototal,precovenda,dataultvenda,ultpreco,estminimo,codigobarras,situacao,tributacao,
		fornecedor,fabricante,embalagem,unidembalagem,descricao,codigoFilial,qtdretida,secao,margemsemfinanciamento,precosemfinanciamento,custofornecedor,qtdprevenda,precoatacado,
		grade,origem,pis,cofins,margemlucro,volumes,ncm,tributacaopis,tributacaocofins,saldofinalestoque,complementodescricao,cstpisentrada,cstcofinsentrada,pisentrada,cofinsentrada,
		codigosuspensaopis,codigosuspensaocofins,idfornecedor,custogerencial,aliquotaStFronteira,MVAFronteira,qtdproducao,margemlucroliquida FROM produtosfilial WHERE codigofilial=filial
		AND tipo='0 - Produto' GROUP BY codigo;
		UPDATE produtosinventario SET anoinventario=_anoInventario,inventarionumero=_numeroInventario,
		custototal=IFNULL(quantidade*customedio,0) WHERE codigofilial=filial AND anoinventario=0;	
	END IF;	
 END IF;
IF(((SELECT COUNT(1) FROM  posicaoestoque WHERE codigofilial = filial AND DATA = CURRENT_DATE) = 0) AND (SELECT gerarposicaoestoque FROM configfinanc WHERE codigofilial =filial) = 'S')THEN
	IF(filial = '00001')THEN
		INSERT INTO posicaoestoque
		(codigo,quantidade,qtdprateleira,qtddeposito,ncm,cstpisentrada,cstcofinsentrada,cstcofinssaida,cstpissaida,pissaida,cofinssaida,pisentrada,cofinsentrada,codigosuspensaopis,
		codigosuspensaocofins,codigofilial,precovenda,precoatacado,custo,custogenrencial,qtdproducao,qtdprevenda,qtdretencao,tributacaoicms,aliquotaicms,DATA,customedio)
		SELECT codigo,quantidade,qtdprateleiras,qtddeposito,ncm,cstpisEntrada,cstcofinsentrada,tributacaocofins,tributacaopis,pis,cofins,pisentrada,cofinsentrada,codigosuspensaopis,
		codigosuspensaocofins,codigofilial,precovenda,precoatacado,custo,custogerencial,qtdproducao,qtdprevenda,qtdretida,tributacao,icms,CURRENT_DATE,customedio FROM produtos 
		WHERE codigofilial = filial AND (situacao = 'Normal' OR situacao ='Promoção' OR situacao ='Item da Balança' OR situacao ='Consignação');
	ELSE
	
		INSERT INTO posicaoestoque
		(codigo,quantidade,qtdprateleira,qtddeposito,ncm,cstpisentrada,cstcofinsentrada,cstcofinssaida,cstpissaida,pissaida,cofinssaida,pisentrada,cofinsentrada,codigosuspensaopis,
		codigosuspensaocofins,codigofilial,precovenda,precoatacado,custo,custogenrencial,qtdproducao,qtdprevenda,qtdretencao,tributacaoicms,aliquotaicms,DATA,customedio)
		SELECT codigo,quantidade,qtdprateleiras,qtddeposito,ncm,cstpisEntrada,cstcofinsentrada,tributacaocofins,tributacaopis,pis,cofins,pisentrada,cofinsentrada,codigosuspensaopis,
		codigosuspensaocofins,codigofilial,precovenda,precoatacado,custo,custogerencial,qtdproducao,qtdprevenda,qtdretida,tributacao,icms,CURRENT_DATE,customedio FROM produtosfilial 
		WHERE codigofilial = filial AND (situacao = 'Normal' OR situacao ='Promoção' OR situacao ='Item da Balança' OR situacao ='Consignação');
	END IF;
END IF;
	
    END */$$
DELIMITER ;

/* Procedure structure for procedure `DescontoMaximo` */

/*!50003 DROP PROCEDURE IF EXISTS  `DescontoMaximo` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `DescontoMaximo`(IN acao VARCHAR(10), IN filial VARCHAR(5), IN enderecoip VARCHAR(15))
BEGIN
IF (acao='desconto') THEN
IF (filial='00001') THEN
	IF((SELECT aceitardescontoatacado FROM configfinanc WHERE codigofilial = filial) = 'S')THEN
		UPDATE vendas,produtos 
		SET vendas.Descontoperc=produtos.descontomaximo,
		vendas.precooriginal=produtos.precovenda,
		vendas.descontovalor=TRUNCATE(produtos.precovenda*(produtos.descontomaximo/100),2)*vendas.quantidade,
		vendas.preco=vendas.precooriginal-TRUNCATE(produtos.precovenda*(produtos.descontomaximo/100),2),
		vendas.total=vendas.quantidade*(vendas.precooriginal-TRUNCATE(produtos.precovenda*(produtos.descontomaximo/100),2))
		WHERE produtos.CodigoFilial=filial
		AND vendas.codigo=produtos.codigo
		AND vendas.id=enderecoip;
	ELSE 
		UPDATE vendas,produtos 
		SET vendas.Descontoperc=produtos.descontomaximo,
		vendas.precooriginal=produtos.precovenda,
		vendas.descontovalor=TRUNCATE(produtos.precovenda*(produtos.descontomaximo/100),2)*vendas.quantidade,
		vendas.preco=vendas.precooriginal-TRUNCATE(produtos.precovenda*(produtos.descontomaximo/100),2),
		vendas.total=vendas.quantidade*(vendas.precooriginal-TRUNCATE(produtos.precovenda*(produtos.descontomaximo/100),2))
		WHERE produtos.CodigoFilial=filial
		AND vendas.codigo=produtos.codigo
		AND vendas.id=enderecoip
		AND vendas.vendaatacado = 'N';
	END IF;
END IF;
IF (filial<>'00001') THEN
	IF((SELECT aceitardescontoatacado FROM configfinanc WHERE codigofilial = filial) = 'S')THEN
		UPDATE vendas,produtosfilial
		SET vendas.Descontoperc=produtosfilial.descontomaximo,
		vendas.precooriginal=produtosfilial.precovenda,
		vendas.descontovalor=TRUNCATE(produtosfilial.precovenda*(produtosfilial.descontomaximo/100),2)*vendas.quantidade,
		vendas.preco=vendas.precooriginal-TRUNCATE(produtosfilial.precovenda*(produtosfilial.descontomaximo/100),2),
		vendas.total=vendas.quantidade*(vendas.precooriginal-TRUNCATE(produtosfilial.precovenda*(produtosfilial.descontomaximo/100),2))
		WHERE produtosfilial.CodigoFilial=filial
		AND vendas.codigo=produtosfilial.codigo
		AND vendas.id=enderecoip;
	ELSE 
		UPDATE vendas,produtosfilial
		SET vendas.Descontoperc=produtosfilial.descontomaximo,
		vendas.precooriginal=produtosfilial.precovenda,
		vendas.descontovalor=TRUNCATE(produtosfilial.precovenda*(produtosfilial.descontomaximo/100),2)*vendas.quantidade,
		vendas.preco=vendas.precooriginal-TRUNCATE(produtosfilial.precovenda*(produtosfilial.descontomaximo/100),2),
		vendas.total=vendas.quantidade*(vendas.precooriginal-TRUNCATE(produtosfilial.precovenda*(produtosfilial.descontomaximo/100),2))
		WHERE produtosfilial.CodigoFilial=filial
		AND vendas.codigo=produtosfilial.codigo
		AND vendas.id=enderecoip
		AND vendas.vendaatacado = 'N';
	
	END IF;
END IF;
END IF;
IF (acao='retirar') THEN
	UPDATE vendas SET preco=precooriginal,descontoperc=0,descontovalor=0,
	total=quantidade*precooriginal 
	WHERE id=enderecoip;
END IF;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `EstornarBaixaCP` */

/*!50003 DROP PROCEDURE IF EXISTS  `EstornarBaixaCP` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `EstornarBaixaCP`(IN id INT(7))
BEGIN
 DECLARE chave VARCHAR(10);
 DECLARE cursorSeq CURSOR FOR SELECT interpolador FROM contaspagar WHERE codigo=id;
 OPEN cursorSeq;
 FETCH cursorSeq INTO chave;	
 
 IF(  (SELECT conta FROM contaspagar WHERE codigo=id)<>'') THEN
      INSERT INTO movdespesas 
      (id,grupo,codigofilial,DATA,valor,conta,
      subconta,despesa,operador,
      historico,descricaoconta,descricaosubconta,
      sangria,contabancaria,interpolador, datafiscal)
     SELECT id,grupo,codigofilial,DATA,valor*-1,conta,
      subconta,despesa,operador,
      historico,descricaoconta,descricaosubconta,
      sangria,contabancaria,interpolador, datafiscal
      FROM movdespesas WHERE movdespesas.interpolador=chave AND DATA=CURRENT_DATE LIMIT 1;    
 END IF;
 
 IF(  (SELECT contabancaria FROM contaspagar WHERE codigo=id)<>'') THEN
	INSERT INTO movcontasbanco(conta, movimento, valorcredito,valordebito, DATA, historico, codigofilial,operador,interpolador)
	SELECT conta, movimento, valorcredito*-1,valordebito*-1,DATA, historico, codigofilial,operador,interpolador FROM movcontasbanco
	WHERE interpolador=chave AND DATA=CURRENT_DATE;
 
 END IF;
 
 update contaspagar set quitado="N",
 marcado=""
 where codigo=id;
 
 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `EstornarDevolucao` */

/*!50003 DROP PROCEDURE IF EXISTS  `EstornarDevolucao` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `EstornarDevolucao`(in doc int,in filial varchar(5), in ipTerminal varchar(15),in devolucaoNR int, in operadorAcao varchar(10))
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

/* Procedure structure for procedure `EncerrarCaixa` */

/*!50003 DROP PROCEDURE IF EXISTS  `EncerrarCaixa` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `EncerrarCaixa`(IN idOperador VARCHAR(10),IN filial VARCHAR(5),IN ipTerminal VARCHAR(15),IN nrFabricaoECF VARCHAR(20)  )
BEGIN
 DECLARE totalRecBL DECIMAL DEFAULT 0;
 DECLARE totalRecDC DECIMAL DEFAULT 0;
 DECLARE totalFN DECIMAL DEFAULT 0;
 DECLARE saldoFinal DECIMAL DEFAULT 0;
 DECLARE dataCaixa DATE;
 
 DECLARE SomaRecBL CURSOR FOR SELECT IFNULL( SUM(valor) ,0) FROM caixa WHERE operador=idOperador
 AND tipopagamento="BL"
 AND dpfinanceiro LIKE ("Receb%")
 AND codigofilial=filial;
 DECLARE SomaRecDC CURSOR FOR SELECT IFNULL( SUM(valor) ,0) FROM caixa WHERE operador=idOperador
 AND tipopagamento="DC"
 AND dpfinanceiro LIKE ("Receb%")
 AND codigofilial=filial;
  
 SELECT MAX(DATA) INTO dataCaixa FROM caixa WHERE codigofilial = filial AND caixa.operador = idOperador;
 DELETE FROM caixassoma WHERE operador = idOperador AND codigofilial = filial AND DATA = dataCaixa;
 CALL GravarCaixaSoma(idOperador,filial,ipTerminal,nrFabricaoECF,dataCaixa);
 
 OPEN SomaRecBL;
 OPEN SomaRecDC;
 
 
 FETCH SomaRecBL INTO totalRecBL;
 FETCH SomaRecDC INTO totalRecDC;
 
 
 IF ( (nrFabricaoECF)="incluir") THEN 
   CALL GravarCaixaSoma(idOperador,filial,ipTerminal,nrFabricaoECF,(SELECT MAX(DATA) FROM caixa WHERE operador = idOperador AND codigofilial = filial));
 END IF;
 
 
UPDATE caixassoma SET recebimentoBL=totalRecBL,recebimentoDC=totalRecDC
WHERE DATA=CURRENT_DATE AND operador=idOperador AND
recebimentoBL=0 AND recebimentoDC=0; 
 
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
 
 UPDATE movdespesas SET encerrado='S'
 WHERE operador=idOperador
 AND codigofilial=filial	
 AND encerrado='N';
 
 UPDATE movreceitas SET encerrado='S'
 WHERE operador=idOperador
 AND codigofilial=filial;
 	
 IF (totalRecBL>0) THEN
 INSERT INTO  movcontasbanco(conta, movimento, valorcredito, DATA, historico, codigofilial,operador)
 VALUES ((SELECT conta FROM filiais WHERE codigofilial=filial),"credito",totalRecBL,CURRENT_DATE,"Crédito Bloqueto",
 codigofilial,operador);
 INSERT INTO  movcontasbanco(conta, movimento, valorcredito, DATA, historico, codigofilial,operador)
 VALUES ((SELECT conta FROM filiais WHERE codigofilial=filial),"debito",
 (SELECT IFNULL(SUM(valortarifabloquete),0) FROM caixa WHERE codigofilial=filial AND operador=idOperador),
 CURRENT_DATE,"Tarifação recebimento bloquete "+filial,
 codigofilial,operador);
 INSERT INTO movdespesas (id,grupo,codigofilial,DATA,datafiscal,valor,conta,
 subconta,despesa,operador,historico,descricaoconta,descricaosubconta,
 sangria,contabancaria)
 VALUES (ipTerminal,"1",filial,CURRENT_DATE,CURRENT_DATE,
 (SELECT IFNULL(SUM(valortarifabloquete),0) FROM caixa WHERE codigofilial=filial AND operador=idOperador),
 (SELECT contadespesa FROM filiais WHERE codigofilial=filial),
 (SELECT subconta FROM filiais WHERE codigofilial=filial),
 "S",idOperador,"Tarifação da taxa de recebimento dos bloquetes",
 (SELECT descricaoconta FROM filiais WHERE codigofilial=filial LIMIT 1),
 (SELECT descricaosubconta FROM filiais WHERE codigofilial=filial LIMIT 1),
 "N",
 (SELECT conta FROM filiais WHERE codigofilial=filial LIMIT 1));
 END IF;
 
 
 CALL FechamentoDiario(filial); 
 CALL AtualizarQdtRegistros();	
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `EstornarQuitacao` */

/*!50003 DROP PROCEDURE IF EXISTS  `EstornarQuitacao` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `EstornarQuitacao`(in docEstorno int,in codCliente int,in ipTerminal varchar(15),in filial varchar(5),in operadorEstorno varchar(10))
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

/*!50003 CREATE  PROCEDURE `ExcluirDocumento`(IN ipTerminal VARCHAR(15), IN nrDocumento INT,IN filial VARCHAR(5),IN operador VARCHAR(10),IN cooECF VARCHAR(9), IN ccfECF VARCHAR(9),IN motivoObs VARCHAR(150),IN usuarioSolicitante VARCHAR(10) )
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
 contadordebitocreditoCDC,contadornaofiscalGNF,COOGNF,modeloDocFiscal ) 
 SELECT ipTerminal,codigofilial,DATA,dataexe,totalbruto*-1,desconto*-1,vrjuros*-1,encargos*-1,total*-1,nome,
 codigocliente,NrParcelas,vendedor,operador,CONCAT(motivoObs,observacao),classe,
 CONCAT("Venda est ",nrDocumento),dpfinanceiro,tipopagamento,id,custos*-1,
 devolucaovenda*-1,devolucaorecebimento*-1,valorservicos*-1,descontoservicos*-1,CURRENT_TIME,"S","S",cooECF,ccfECF,ecffabricacao, ecfmarca,ecfmodelo,ecfnumero,"1",contadordebitocreditoCDC,contadornaofiscalGNF,COOGNF,modeloDocFiscal  
 FROM contdocs WHERE documento=nrDocumento;
 SET @RetornaPrd = CONCAT ('UPDATE ',@tabelaProduto,',venda 
 set ',@tabelaProduto,'.qtdanterior=',@tabelaProduto,'.quantidade,',
 @tabelaProduto,'.quantidade=',@tabelaProduto,'.quantidade+(SELECT IFNULL(SUM(venda.quantidade*venda.embalagem),0) from venda where venda.cancelado="N" and venda.codigo=',@tabelaProduto,'.codigo and venda.documento=','"',nrDocumento,'" ),',		
 @tabelaProduto,'.qtdprateleiras=',@tabelaProduto,'.qtdprateleiras+(SELECT IFNULL(SUM(venda.quantidade*venda.embalagem),0) from venda where venda.cancelado="N" AND venda.codigo=',@tabelaProduto,'.codigo and venda.documento=','"',nrDocumento,'"), ',
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
 SET produtosgrade.quantidade=produtosgrade.quantidade+(SELECT SUM(venda.quantidade*venda.embalagem) FROM venda WHERE venda.cancelado="N" AND venda.documento=nrdocumento AND produtosgrade.codigo=venda.codigo AND venda.grade=produtosgrade.grade)
 WHERE produtosgrade.codigo=venda.codigo
 AND produtosgrade.grade=venda.grade
 AND venda.documento=nrDocumento
 AND produtosgrade.codigofilial=filial;
 
 
 SET @qtdcomposicao = CONCAT('UPDATE ',@tabelaProduto,',produtoscomposicao',',venda  
 SET ',@tabelaProduto,'.quantidade=',@tabelaProduto,'.quantidade+(produtoscomposicao.quantidade*(venda.quantidade*venda.embalagem) ) 
 WHERE produtoscomposicao.codigomateria=',@tabelaProduto,'.codigo and venda.quantidade>0
 AND produtoscomposicao.codigo=venda.codigo 
 AND ',@tabelaProduto,'.codigofilial=',filial, '
 AND venda.cancelado="N" 
 AND ',@tabelaProduto,'.indicadorproducao="T" 
 AND venda.documento=',nrDocumento);
 PREPARE st FROM @qtdcomposicao;
 EXECUTE st; 
   
 UPDATE vendas SET cancelado="S" WHERE documento=nrDocumento AND id=ipTerminal;
 UPDATE venda SET cancelado="S" WHERE documento=nrDocumento;
 UPDATE venda SET cancelado="S" WHERE coo=cooECF AND codigofilial=filial;
  
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
 UPDATE caixa SET estornado = 'S' WHERE documento = nrDocumento;	
 DELETE FROM crmovclientes WHERE documento=nrDocumento 
 AND documento<>0;
 DELETE FROM cheques WHERE documento=nrDocumento AND documento<>0;
 DELETE FROM movcartoes WHERE documento=nrDocumento AND documento<>0;
 INSERT INTO auditoria (codigofilial,usuario,usuariosolicitante,hora,DATA,tabela,acao,documento,LOCAL,observacao) 
 VALUES (
 filial,operador,usuarioSolicitante,CURRENT_TIME,CURRENT_DATE,'Venda',CONCAT('Estorno, valor R$: ',valorEstorno),nrDocumento,
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

/* Procedure structure for procedure `finalizarAv` */

/*!50003 DROP PROCEDURE IF EXISTS  `finalizarAv` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `finalizarAv`(IN ipTerminal VARCHAR(20), IN doc INT)
BEGIN
	DECLARE valorRestanteAV REAL DEFAULT 0;
	DECLARE valorAbatidoAV REAL DEFAULT 0;
IF ( (SELECT COUNT(1) FROM caixas WHERE tipopagamento="AV" AND enderecoip=ipTerminal)>0) THEN 
	SET valorRestanteAV = (SELECT SUM(valor) FROM caixas WHERE tipopagamento="AV" AND enderecoip=ipTerminal);
	
	WHILE valorRestanteAV>0 DO
		SET valorAbatidoAV = (SELECT valor FROM contaspagar  WHERE codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE documento=doc AND valor>0 LIMIT 1)  AND  quitado="N"  ORDER BY codigo LIMIT 1);
		
		UPDATE contaspagar SET valor=valor-IF(valor<valorRestanteAV,valor,valorRestanteAV),
		historico=CONCAT('AV de cliente na compra  doc: ',doc),
		datapagamento=CURRENT_DATE,
		usuario=(SELECT contdocs.operador FROM contdocs WHERE documento=doc LIMIT 1) 
		WHERE contaspagar.codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE documento=doc LIMIT 1)
		AND quitado='N' ORDER BY codigo LIMIT 1;
	
		UPDATE contaspagar SET quitado="S" WHERE valor<=0 AND 
		contaspagar.codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE documento=doc LIMIT 1);
		UPDATE clientes,contaspagar SET clientes.creditoav=(SELECT SUM(valor) FROM contaspagar
		WHERE codigocliente=(SELECT contdocs.codigocliente FROM contdocs WHERE documento=doc LIMIT 1) AND quitado="N" AND cancelado="N") 
		WHERE clientes.Codigo=(SELECT contdocs.codigocliente FROM contdocs WHERE documento=doc LIMIT 1);
		SET valorRestanteAV = valorRestanteAV - valorAbatidoAV;
	END WHILE;   
 
 
 END IF; 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarDAV` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarDAV` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `FinalizarDAV`(IN DAVNumero INT,IN filial VARCHAR(5),IN ipTerminal VARCHAR(15))
BEGIN	
UPDATE contdav SET 
EADRegistroDAV=MD5(CONCAT(ncupomfiscal,numeroDAVFilial,DATA,valor,IFNULL(numeroECF,"001"),IFNULL(contadorRGECF,""),IFNULL(cliente,""),IFNULL(ecfCPFCNPJconsumidor,"")))
WHERE numeroDAVFilial=davNumero;
UPDATE contdav SET 
ecffabricacao=""
WHERE numeroDAVFilial=davNumero AND ecffabricacao IS NULL;
 UPDATE caixas SET 
 documento=DAVNumero,
 codigofilial=filial,
 vendedor=(SELECT vendedor FROM contdav WHERE numero=DAVNumero LIMIT 1),
 vrdesconto=0,
 operador=(SELECT operador FROM contdav WHERE numero=DAVNumero LIMIT 1)
 WHERE enderecoip=ipTerminal;
 
 UPDATE caixas SET
 vrdesconto=(SELECT desconto FROM contdav WHERE numero=DAVNumero LIMIT 1)
 WHERE enderecoip=ipTerminal;
 
 UPDATE vendas SET 
 documento= DAVNumero,
 codigofilial=filial,
 operador=(SELECT operador FROM contdav WHERE numeroDAVFilial=DAVNumero LIMIT 1)
 WHERE id=ipTerminal AND codigofilial=filial ;
 
 INSERT INTO `vendadav` (`acrescimototalitem`, `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`,`ncm`,`canceladoECF`,`ccf`,`coo`,`ecffabricacao`,vendaatacado) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`,`ncm`,`canceladoECF`,`ccf`,`coo`,`ecffabricacao`,vendaatacado
 FROM vendas WHERE id=ipTerminal;
 
 UPDATE vendadav,contdav 
 SET vendadav.coo=contdav.ncupomfiscal,vendadav.ecffabricacao=contdav.ecffabricacao
 WHERE vendadav.documento=contdav.numeroDAVFilial AND contdav.numeroDAVFilial=DAVNumero;
 
UPDATE contdav SET 
EADRegistroDAV=MD5(CONCAT(TRIM(ecffabricacao), IFNULL(mfAdicional,""), IFNULL(tipoECF,""), marca, modelo, contadorRGECF, numeroDAVFilial, DATA, valor, ncupomfiscal, numeroECF, cliente, ecfCPFCNPJconsumidor))
WHERE numeroDAVFilial=davNumero;
IF((SELECT COUNT(1) FROM vendadav WHERE vendadav.documento = DAVNumero AND vendadav.codigofilial = filial AND vendadav.vendaatacado = "S") > 0)THEN
	UPDATE contdav SET contdav.vendaatacado = 'S' WHERE numeroDAVFilial = DAVNumero AND codigofilial = filial;
 END IF; 
  
 
INSERT INTO `caixadav` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
FROM caixas WHERE enderecoip=ipTerminal AND valor<>0 AND documento=DAVNumero;
 
 DELETE FROM caixas WHERE enderecoip=ipTerminal;
 DELETE FROM vendas WHERE id=ipTerminal;
 
 CALL AtualizarQdtRegistros();	
  
  
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarDAVOS` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarDAVOS` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `FinalizarDAVOS`(in DAVNumero int,in filial varchar(5),in ipTerminal varchar(15))
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
 
 INSERT INTO `vendadav` (`acrescimototalitem`, `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`,`ncm`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`,`ncm`
 FROM vendas WHERE id=ipTerminal;
 
  
 INSERT INTO `caixadav` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas WHERE enderecoip=ipTerminal AND valor<>0 AND documento=DAVNumero;
  
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
 CALL AtualizarQdtRegistros();	
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarDAV_backup` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarDAV_backup` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `FinalizarDAV_backup`(in DAVNumero int,in filial varchar(5),in ipTerminal varchar(15))
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
 documento= (SELECT MAX(numero) from contdav where contdav.enderecoip=ipTerminal) ,
 codigofilial=filial,
 vendedor=(select vendedor from contdav where numero=DAVNumero limit 1),		
 operador=(select operador from contdav where numero=DAVNumero limit 1)
 WHERE id=ipTerminal and codigofilial=filial ;
 
 
 INSERT INTO `vendadav` (`acrescimototalitem`, `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`
 FROM vendas where id=ipTerminal;
 
  
 INSERT INTO `caixadav` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas where enderecoip=ipTerminal and valor<>0 and documento=DAVNumero;
 
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
 
 UPDATE filiais SET versaopaf="1.9" WHERE codigofilial=filial;
 CALL AtualizarQdtRegistros();	
 
 
  
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarPreVenda` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarPreVenda` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `FinalizarPreVenda`(in preVendaNumero int,in filial varchar(5),in ipTerminal varchar(15))
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
 
 
 INSERT INTO `vendaprevendapaf` (`acrescimototalitem`, `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`,`ncm`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`eaddados`,`aliquotaipi`,`ncm`
 FROM vendas WHERE id=ipTerminal;
 
 
 UPDATE vendaprevendapaf SET 
 eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),ecffabricacao))
 WHERE documento=preVendaNumero;
  
 INSERT INTO `caixaprevendapaf` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas WHERE enderecoip=ipTerminal AND valor<>0 AND documento=preVendaNumero;
 
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
 CALL AtualizarQdtRegistros();	
 END */$$
DELIMITER ;

/* Procedure structure for procedure `GerarNumeroBoleto` */

/*!50003 DROP PROCEDURE IF EXISTS  `GerarNumeroBoleto` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `GerarNumeroBoleto`(in filial varchar(5),in doc int)
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

/* Procedure structure for procedure `GerarDebitoServicos` */

/*!50003 DROP PROCEDURE IF EXISTS  `GerarDebitoServicos` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `GerarDebitoServicos`(IN operador VARCHAR(10),IN filial VARCHAR(5),IN ipTerminal VARCHAR(15),IN dataVencimento DATE, IN nGerarParcelas INT,IN gravarSeqBoleto CHAR(1), OUT qtdBoletosGerados INT,OUT totalGerado DECIMAL(10,2))
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
	
IF ( (SELECT COUNT(1) FROM crmovclientes WHERE codigo=idcliente AND valoratual>0)<=nGerarParcelas) THEN
INSERT INTO contdocs (ip,codigofilial,DATA,dataexe,
totalbruto,desconto,encargos,total,nome,
codigocliente,NrParcelas,vendedor,operador,
dpfinanceiro,concluido,hora,nrboletobancario) 
VALUES(ipTerminal,
filial,CURRENT_DATE,CURRENT_DATE,(SELECT valorcontrato FROM clientes WHERE codigo=idcliente),
(SELECT desconto FROM clientes WHERE codigo=idcliente),0,(IF( (SELECT datalimitedesconto FROM clientes WHERE codigo=idCliente) >CURRENT_DATE, (SELECT valorcontrato-desconto FROM clientes  WHERE codigo=idcliente),(SELECT valorcontrato FROM clientes WHERE codigo=idcliente)) ) ,
(SELECT nome FROM clientes WHERE codigo=idcliente),idCliente,1,IFNULL((SELECT vendedor FROM clientes WHERE codigo=idcliente), "000"),operador,
"Servicos","S",CURRENT_TIME,0 ); 
INSERT INTO caixa (codigofilial,enderecoIP,nome,codigocliente,
valor,documento,dataexe,DATA,vencimento,
tipopagamento,operador,vendedor,
dpfinanceiro)
VALUES (filial,ipTerminal,(SELECT nome FROM clientes WHERE codigo=idcliente),idCliente,
(SELECT valorcontrato FROM clientes WHERE codigo=idcliente),
(SELECT MAX(documento) FROM contdocs WHERE contdocs.ip=ipTerminal AND codigocliente=idcliente),
CURRENT_DATE,CURRENT_DATE,dataVencimento,"CR",operador,IFNULL((SELECT vendedor FROM clientes WHERE codigo=idcliente), "000"),
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
IFNULL((SELECT vendedor FROM clientes WHERE codigo=idcliente), "000"),
(SELECT valorcontrato FROM clientes WHERE codigo=idcliente),
"Servicos",
(SELECT cnpj FROM clientes WHERE codigo=idcliente));
UPDATE clientes SET databoleto=CURRENT_DATE WHERE codigo=idCliente;
IF (gravarSeqBoleto="S") THEN
	CALL GerarNumeroBoleto(filial,(SELECT MAX(documento) FROM contdocs WHERE contdocs.ip=ipTerminal AND codigocliente=idcliente));
	SET qtdBoletosGerados = qtdBoletosGerados+1;
	SET totalGerado = totalGerado+(SELECT total FROM contdocs WHERE contdocs.ip=ipTerminal AND codigocliente=idcliente LIMIT 1);
END IF;
END IF;
		
	END LOOP;	
	CLOSE cursorClientes;
	
	UPDATE remessa SET marcado="N";
	INSERT INTO remessa (documento, codigocliente, parcela, nossonumero, situacao, datavencimento, dataprocessamento, codigofilial, conta, marcado)
	SELECT documento, codigo, "1", bloquete, "01", vencimento, CURRENT_DATE(), codigofilial, (SELECT conta FROM filiais WHERE codigofilial=filial LIMIT 1), "S"
	FROM crmovclientes WHERE datacompra=CURRENT_DATE() AND codigofilial=filial GROUP BY bloquete;
	
    END */$$
DELIMITER ;

/* Procedure structure for procedure `GravarControleSincronia` */

/*!50003 DROP PROCEDURE IF EXISTS  `GravarControleSincronia` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `GravarControleSincronia`(IN _codigoFilial CHAR(5))
BEGIN
	
 DECLARE t VARCHAR(50);
 DECLARE done INT DEFAULT 0;
 DECLARE i INT DEFAULT 0;
 DECLARE arrayTerminal CURSOR FOR  SELECT terminal FROM terminais WHERE codigofilial = _codigoFilial;
 DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
 SET done = 0;
 OPEN arrayTerminal;
 REPEAT 
  FETCH arrayTerminal INTO t;
  	IF(done < 1)THEN	
		INSERT INTO controlesincronizacaoserver(terminal,codigo,tabela,codigofilial) SELECT t,codigo,'produtos',_codigoFilial FROM produtos WHERE sincronizar = 'S' AND codigofilial = _codigoFilial;
		INSERT INTO controlesincronizacaoserver(terminal,codigo,tabela,codigofilial) SELECT t,codigo,'produtosfilial',_codigoFilial FROM produtosfilial WHERE sincronizar = 'S' AND codigofilial = _codigoFilial;
		INSERT INTO controlesincronizacaoserver(terminal,codigo,tabela,codigofilial) SELECT t,codigo,'clientes',_codigoFilial FROM clientes WHERE sincronizar = 'S';
		INSERT INTO controlesincronizacaoserver(terminal,codigo,tabela,codigofilial) SELECT t,codigofilial,'filiais',_codigoFilial FROM filiais WHERE sincronizar = 'S' AND codigofilial = _codigoFilial;
		INSERT INTO controlesincronizacaoserver(terminal,codigo,tabela,codigofilial) SELECT t,codigo,'senhas',_codigoFilial FROM senhas WHERE sincronizar = 'S' AND codigofilial = _codigoFilial;
		INSERT INTO controlesincronizacaoserver(terminal,codigo,tabela,codigofilial) SELECT t,codigo,'configfinanc',_codigoFilial FROM configfinanc WHERE sincronizar = 'S' AND codigofilial = _codigoFilial;
		INSERT INTO controlesincronizacaoserver(terminal,codigo,tabela,codigofilial) SELECT t,codigo,'vendedores',_codigoFilial FROM vendedores WHERE sincronizar = 'S' AND codigofilial = _codigoFilial;
		
		
	END IF;
  UNTIL done
  END REPEAT;
  CLOSE arrayTerminal;
 SELECT i;
 UPDATE produtos SET sincronizar = 'N' WHERE codigofilial = _codigoFilial;
 UPDATE produtosfilial SET sincronizar = 'N' WHERE codigofilial = _codigoFilial;
 UPDATE clientes SET sincronizar = 'N' WHERE codigofilial = _codigoFilial;
 UPDATE filiais SET sincronizar = 'N' WHERE codigofilial = _codigoFilial;
 UPDATE senhas SET sincronizar = 'N' WHERE codigofilial = _codigoFilial;
 UPDATE configfinanc SET sincronizar = 'N' WHERE codigofilial = _codigoFilial;
 UPDATE vendedores SET sincronizar = 'N' WHERE codigofilial = _codigoFilial;
 UPDATE cidades SET sincronizar = 'N';
 UPDATE juros SET sincronizar = 'N' WHERE codigofilial = _codigoFilial;
 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `gerarEntrada` */

/*!50003 DROP PROCEDURE IF EXISTS  `gerarEntrada` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `gerarEntrada`(IN _cfopEntrada CHAR(5),IN _transferencia INT,IN _notafiscal INT,IN _serie INT,IN _codigoFilial CHAR(5))
BEGIN
	DECLARE _codigoFornecedor INT DEFAULT 0;
	DECLARE _razaosocial VARCHAR(100);
	DECLARE _ufEmitente CHAR(2);
	DECLARE _numeroEntrada INT DEFAULT 0;
	DECLARE _filialDestino CHAR(5);
	DECLARE _operador VARCHAR(20);
	DECLARE _chaveAcesso VARCHAR(100);
		
	SELECT codigo INTO _codigoFornecedor FROM fornecedores WHERE CGC = (SELECT cnpj FROM filiais WHERE codigofilial=_codigoFilial LIMIT 1) AND situacao ='Ativo';
	SELECT empresa INTO _razaosocial FROM fornecedores WHERE CGC = (SELECT cnpj FROM filiais WHERE codigofilial=_codigoFilial LIMIT 1) AND situacao ='Ativo';
	SELECT estado INTO _ufEmitente FROM fornecedores WHERE CGC = (SELECT cnpj FROM filiais WHERE codigofilial=_codigoFilial LIMIT 1) AND situacao ='Ativo';
	SELECT codigoFilial INTO _filialDestino FROM filiais WHERE cnpj = (SELECT c.cnpjcpf FROM contnfsaida AS c WHERE c.notafiscal = _notafiscal AND c.serie = _serie 
	AND c.codigofilial = _codigoFilial LIMIT 1) AND ativa ='S' LIMIT 1;
		
	SELECT c.operador INTO _operador  FROM contnfsaida AS c WHERE c.notafiscal = _notafiscal AND c.serie = _serie AND c.codigofilial = _codigoFilial LIMIT 1;
	
	SELECT c.chave_nfe INTO _chaveAcesso  FROM contnfsaida AS c WHERE c.notafiscal = _notafiscal AND c.serie = _serie AND c.codigofilial = _codigoFilial LIMIT 1;
	
	IF((SELECT COUNT(1) FROM moventradas WHERE chave_nfe = _chaveAcesso AND codigoFilial = _filialDestino) = 0)THEN
		
		INSERT INTO moventradas(fornecedor,nf,dataEmissao,dataEntrada,DATA,ValorProdutos,valornota,icms,baseicms,ipi,frete,IcmsSubst,BaseIcmsSubst,despesas,usuario,codigofilial,operacao,
		tipofrete,serie,descontos,importada,valorseguro,codigofornecedor,UFemitente,horaemissao,baseCalculoIPI,modelonf,emitente,cfopentrada,exportarfiscal,
		sefvalidado,chave_nfe,indicadorpagamento,transferencia,confirmacao,especie)
		
		SELECT _razaosocial,c.notafiscal,CURRENT_DATE,CURRENT_DATE,CURRENT_DATE,c.TotalProduto,c.total,c.totalicms,c.basecalculo,c.basecalculoipi,c.totalfrete,
		c.totalICMSST,c.basecalculoICMSST,c.despesasacessorias,c.operador,_filialDestino,c.cfop,c.tipofrete,c.serie,c.desconto,'N',c.totalseguro,
		_codigoFornecedor,_ufEmitente,CURRENT_TIME,c.totalipi,c.modelodocfiscal,'T',_cfopEntrada,'S','N',c.chave_nfe,c.indicadorpagamento,_transferencia,'N','NF' 
		FROM contnfsaida AS c WHERE c.notafiscal = _notafiscal AND c.serie = _serie AND c.codigofilial = _codigoFilial;
		
		SELECT MAX(numero) INTO _numeroEntrada FROM moventradas WHERE codigofilial = _filialDestino AND nf = _notafiscal AND serie = _serie;
		
		INSERT INTO entradas(numero,codigo,descricao,fornecedor,quantidade,codfornecedor,nf,QuantNF,CustoNF,custo,customedio,custocalculado,precovenda,DATA,dataemissao,dataentrada,icmsentrada,
		ipi,frete,usuario,codigofilial,lote,vencimento,grupo,subgrupo,qtdprateleiras,qtddeposito,operacao,icms,quantidadeanterior,customedioanterior,quantidadeatualizada,producaonumero,
		icmsproduto,ratdespesas,itemICMSST,tributacao,cfopentrada,precoatacado,percentualRedBaseCalcICMS,bcpis,cstpis,pis,bccofins,cstcofins,cofins,serienf,unidade,bcicms,totaldesconto,
		valorunitario,totalitem,bcICMSST,valoricmsST,valoricms,modelonf,sequencia,datafabricacao,icmsst,exportarfiscal,ratfrete,ratseguro,ratdesconto,embalagem,mva,idfornecedor)
		
		SELECT _numeroEntrada,v.codigo,v.produto,_razaosocial,v.quantidade,
		_codigoFornecedor,
		v.NotaFiscal,0,0,v.custo,v.custo,v.custo,v.preco,
		CURRENT_DATE,CURRENT_DATE,CURRENT_DATE,v.icms,v.ipi,v.ratfrete,v.operador,_filialDestino,v.lote,'1899-12-30',v.grupo,v.subgrupo,v.quantidade,0,_cfopEntrada,v.icms,
		0,0,0,0,v.icms,v.ratdespesas,
		IF((SELECT c.CbdvICMSST_icms FROM cbd001deticmsnormalst AS c WHERE c.CbdNtfNumero = v.NotaFiscal AND c.CbdNtfSerie = v.serieNF AND c.CbdCodigoFilial = v.codigofilial AND c.CbdnItem = v.nrcontrole) = 0,'N','S'),
		v.tributacao,_cfopEntrada,v.preco,
		(SELECT c.CbdpRedBC FROM cbd001deticmsnormalst AS c WHERE c.CbdNtfNumero = v.NotaFiscal AND c.CbdNtfSerie = v.serieNF AND c.CbdCodigoFilial = v.codigofilial AND c.CbdnItem = v.nrcontrole) AS 'reducaoicms',
		(SELECT c.CbdvBC_pis FROM cbd001detpis AS c WHERE c.CbdNtfNumero = v.NotaFiscal AND c.CbdNtfSerie = v.serieNF AND c.CbdCodigoFilial = v.codigofilial AND c.CbdnItem = v.nrcontrole) AS 'valorBasepis',
		v.cstpis,v.pis,
		(SELECT c.CbdvBC_cofins FROM cbd001detcofins AS c WHERE c.CbdNtfNumero = v.NotaFiscal AND c.CbdNtfSerie = v.serieNF AND c.CbdCodigoFilial = v.codigofilial AND c.CbdnItem = v.nrcontrole) AS 'valorBaseCofins',
		v.cstcofins,v.cofins,v.serieNF,v.unidade,
		(SELECT c.CbdvBC FROM cbd001deticmsnormalst AS c WHERE c.CbdNtfNumero = v.NotaFiscal AND c.CbdNtfSerie = v.serieNF AND c.CbdCodigoFilial = v.codigofilial AND c.CbdnItem = v.nrcontrole) AS 'baloricms',
		v.descontovalor,v.preco,v.total,
		(SELECT c.CbdvBCST FROM cbd001deticmsnormalst AS c WHERE c.CbdNtfNumero = v.NotaFiscal AND c.CbdNtfSerie = v.serieNF AND c.CbdCodigoFilial = v.codigofilial AND c.CbdnItem = v.nrcontrole) AS 'baseIcmsST',
		(SELECT c.CbdvICMSST_icms FROM cbd001deticmsnormalst AS c WHERE c.CbdNtfNumero = v.NotaFiscal AND c.CbdNtfSerie = v.serieNF AND c.CbdCodigoFilial = v.codigofilial AND c.CbdnItem = v.nrcontrole) AS 'valorIcmsST',
		(SELECT c.CbdvICMS_icms FROM cbd001deticmsnormalst AS c WHERE c.CbdNtfNumero = v.NotaFiscal AND c.CbdNtfSerie = v.serieNF AND c.CbdCodigoFilial = v.codigofilial AND c.CbdnItem = v.nrcontrole) AS 'baseIcmstSt',
		v.modelodocfiscal,v.nrcontrole,v.datafabricacao,v.icmsst,'S',v.ratfrete,v.ratseguro,v.ratdesc,
		v.embalagem,v.mvast,
		_codigoFornecedor FROM vendanf AS v WHERE v.NotaFiscal = _notafiscal AND v.serieNF = _serie AND v.codigofilial = _codigoFilial;
		
		CALL ProcessarEntrada(_filialDestino,_numeroEntrada,'N',_operador,0,0,_transferencia);
	ELSE 
		SELECT numero INTO _numeroEntrada FROM moventradas WHERE codigofilial = _filialDestino AND chave_nfe = _chaveAcesso LIMIT 1;
	
	END IF;	
	
	SELECT 	_numeroEntrada AS entrada;
	
    END */$$
DELIMITER ;

/* Procedure structure for procedure `gerarLivroFiscal` */

/*!50003 DROP PROCEDURE IF EXISTS  `gerarLivroFiscal` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `gerarLivroFiscal`(IN dataIncio DATE, IN dataFinal DATE,IN codigoFilial CHAR(5))
BEGIN
		CALL CriarTabelasTemp('venda',codigoFilial,dataIncio,dataFinal);
		DELETE FROM livrofiscal WHERE DATA BETWEEN dataIncio AND dataFinal AND codigofilial = codigoFilial;
		
		INSERT INTO livrofiscal(DATA,especie,serie,documento,dataEmissao,Emitente,cnpj,estado,valorcontabil,cfop,baseicms,totalicms,icms,basepis,totalpis,basecofins,
		totalcofins,baseicmsst,totalicmsst,baseIPI,totalIPI,valorisentos,codigoFilial,codigoproduto,tributacao)
		SELECT moventradas.dataentrada,moventradas.modeloNF,moventradas.serie,registro50entradas_itens.nf,moventradas.DataEmissao,moventradas.fornecedor,
		(SELECT cgc FROM fornecedores WHERE fornecedores.Codigo = moventradas.codigofornecedor LIMIT 1) AS cnpj,registro50entradas_itens.UFemitente,totalNF,
		registro50entradas_itens.cfopentrada,bcicms,toticms,icmsentrada,basecalculopis,totpis,
		baseCalculoCOFINS,totcofins,bcicmsst,valoricmsst,registro50entradas_itens.baseCalculoIPI,ipiItem,(valorisentas - registro50entradas_itens.desconto),registro50entradas_itens.codigoFilial,
		registro50entradas_itens.codigo,registro50entradas_itens.tributacao
		FROM registro50entradas_itens INNER JOIN 
		moventradas ON moventradas.numero = registro50entradas_itens.numero
		WHERE registro50entradas_itens.dataentrada BETWEEN dataIncio AND dataFinal AND registro50entradas_itens.codigoFilial = codigoFilial
		AND moventradas.exportarfiscal = 'S' AND moventradas.transferencia = 0;
		
		INSERT INTO livrofiscal(DATA,especie,serie,documento,dataEmissao,Emitente,cnpj,estado,valorcontabil,cfop,baseicms,totalicms,icms,basepis,totalpis,basecofins,
		totalcofins,baseicmsst,totalicmsst,baseIPI,totalIPI,valorisentos,codigoFilial,codigoproduto,tributacao)
		SELECT registro50saidas_itens.DATA,registro50saidas_itens.modelodocfiscal,serienf,registro50saidas_itens.notafiscal,registro50saidas_itens.DATA,
		cliente,cnpjcpf,(SELECT estado FROM filiais WHERE codigoFilial = codigoFilial LIMIT 1) AS estadoEmitente,registro50saidas_itens.total,registro50saidas_itens.cfop,
		baseCalculoICMS,registro50saidas_itens.totalicms,icms,baseCalculoPIS,totalpis,baseCalculoCOFINS,
		totalCOFINS,bcICMSST,registro50saidas_itens.totalICMSST,registro50saidas_itens.baseCalculoIPI,registro50saidas_itens.totalIPI,valorisentas,
		registro50saidas_itens.codigoFilial,registro50saidas_itens.codigo,CONCAT(registro50saidas_itens.origem,registro50saidas_itens.tributacao) 
		FROM registro50saidas_itens
		INNER JOIN contnfsaida ON (contnfsaida.notafiscal = registro50saidas_itens.notafiscal AND contnfsaida.serie = registro50saidas_itens.serienf 
		AND contnfsaida.codigofilial = registro50saidas_itens.codigoFilial) WHERE contnfsaida.tipo = '0' AND contnfsaida.DATA BETWEEN dataIncio AND dataFinal
		AND contnfsaida.codigofilial = codigoFilial AND contnfsaida.situacaoNF = '00' AND contnfsaida.exportarfiscal = 'S';
		
		INSERT INTO livrofiscal(DATA,especie,serie,documento,dataEmissao,Emitente,cnpj,estado,valorcontabil,cfop,baseicms,totalicms,icms,basepis,totalpis,basecofins,
		totalcofins,baseicmsst,totalicmsst,baseIPI,totalIPI,valorisentos,codigoFilial,codigoproduto,tributacao)
		SELECT registro50saidas_itens.DATA,registro50saidas_itens.modelodocfiscal,serienf,registro50saidas_itens.notafiscal,registro50saidas_itens.DATA,
		cliente,cnpjcpf,(SELECT estado FROM filiais WHERE codigoFilial = codigoFilial LIMIT 1) AS estadoEmitente,registro50saidas_itens.total,registro50saidas_itens.cfop,
		baseCalculoICMS,registro50saidas_itens.totalicms,icms,baseCalculoPIS,totalpis,baseCalculoCOFINS,
		totalCOFINS,bcICMSST,registro50saidas_itens.totalICMSST,registro50saidas_itens.baseCalculoIPI,registro50saidas_itens.totalIPI,valorisentas,
		registro50saidas_itens.codigoFilial,registro50saidas_itens.codigo,CONCAT(registro50saidas_itens.origem,registro50saidas_itens.tributacao)
		FROM registro50saidas_itens
		INNER JOIN contnfsaida ON (contnfsaida.notafiscal = registro50saidas_itens.notafiscal AND contnfsaida.serie = registro50saidas_itens.serienf 
		AND contnfsaida.codigofilial = registro50saidas_itens.codigoFilial) WHERE contnfsaida.tipo = '1' AND contnfsaida.DATA BETWEEN dataIncio AND dataFinal 
		AND contnfsaida.codigofilial = codigoFilial AND contnfsaida.situacaoNF = '00' AND contnfsaida.exportarfiscal = 'S';
		
		INSERT INTO livrofiscal(DATA,especie,serie,documento,dataEmissao,Emitente,cnpj,estado,valorcontabil,cfop,baseicms,totalicms,icms,basepis,totalpis,basecofins,
		totalcofins,baseicmsst,totalicmsst,baseIPI,totalIPI,valorisentos,codigoFilial,codigoproduto,tributacao)
		SELECT v.DATA,v.modelodocfiscal,v.ecfnumero,v.coo,v.DATA,contdocs.ecfConsumidor,contdocs.ecfCPFCNPJconsumidor,'',(v.total-v.ratdesc) AS total,v.cfop,(v.total - v.ratdesc) AS baseICMS,TRUNCATE((((v.total - v.ratdesc) * v.icms) / 100),2) AS totalIcms,
		v.icms,(v.total-v.ratdesc)AS baseCalculoPIS,TRUNCATE((((v.total-v.ratdesc)*v.pis)/100),2) AS totalpis, (v.total-v.ratdesc) AS baseCalculoCofins, TRUNCATE((((v.total-v.ratdesc)*v.cofins)/100),2) AS totalcofins,
		0,0,0,0,(SELECT (v.total-v.ratdesc) FROM vendaarquivo WHERE documento = v.documento AND codigo = v.codigo AND tributacao <> '00' AND tributacao <> '60' AND tributacao <> '20' AND tributacao <> '30') AS valorIsentas,
		v.codigofilial,v.codigo,CONCAT(v.origem,v.tributacao)
		FROM vendatmp AS v  INNER JOIN contdocs ON v.documento = contdocs.documento
		WHERE v.DATA BETWEEN dataIncio AND dataFinal AND v.codigofilial = codigoFilial AND v.cancelado = 'N' AND v.quantidade > 0;
		
    END */$$
DELIMITER ;

/* Procedure structure for procedure `montarNFCe` */

/*!50003 DROP PROCEDURE IF EXISTS  `montarNFCe` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `montarNFCe`(IN _documento INT,IN _codigoFilial CHAR(5),IN _terminal VARCHAR(30),IN _atualizar CHAR(1),IN _operador VARCHAR(30))
BEGIN
DELETE FROM vendas WHERE codigoFilial =_codigoFilial AND id = _terminal AND operador =_operador;
DELETE FROM caixas WHERE codigoFilial =_codigoFilial AND enderecoIP = _terminal AND operador =_operador;
IF((SELECT DATA FROM contdocs WHERE documento = _documento AND codigoFilial = _codigoFilial) = CURRENT_DATE)THEN
	
	INSERT INTO vendas (aentregar,codigofilial,classe,codigobarras,codigofiscal,comissao,id,lote,romaneio,tipo,embalagem,nrcontrole,codigo,produto,quantidade,preco,custo,precooriginal,acrescimototalitem,unidade,Descontoperc,
	descontovalor,vendedor,icms,tributacao,total,cfop,cstcofins,cstpis,serieNF,subserienf,modelodocfiscal,cancelado,DATA,operador,grade,ratfrete,ratseguro,ratdespesas,cstipi,qUnidIPI,vUnidIPI,ncm,nbm,ncmespecie,origem,
	pis,cofins,aliquotaIPI,cenqipi,itemDAV,canceladoECF,vendaatacado,ratdesc)
	SELECT "N",codigofilial,classe,codigobarras,codigofiscal,comissao,_terminal,lote,romaneio,tipo,embalagem,nrcontrole,codigo,produto,quantidade,preco,custo,precooriginal,acrescimototalitem,unidade,Descontoperc,
	descontovalor,vendedor,icms,tributacao,total,cfop,cstcofins,cstpis,serieNF,subserienf,modelodocfiscal,cancelado,DATA,operador,grade,ratfrete,ratseguro,ratdespesas,'99','0','0',ncm,nbm,ncmespecie,origem,
	pis,cofins,'0',cenqipi,itemDAV,canceladoECF,vendaatacado,ratdesc FROM venda AS v WHERE documento = _documento AND cancelado = 'N' AND quantidade > 0 AND codigoFilial = _codigoFilial;
	
	INSERT INTO caixas(EnderecoIP,valor,caixa,historico,DATA,tipopagamento,operador,dpfinanceiro,filialorigem,CodigoFilial,vencimento,vendedor)
	SELECT  _terminal,valor,caixa,historico,DATA,tipopagamento,operador,dpfinanceiro,filialorigem,CodigoFilial,vencimento,vendedor FROM caixa WHERE documento =_documento AND codigoFilial = _codigoFilial;
	
ELSE 
	INSERT INTO vendas (aentregar,codigofilial,classe,codigobarras,codigofiscal,comissao,id,lote,romaneio,tipo,embalagem,nrcontrole,codigo,produto,quantidade,preco,custo,precooriginal,acrescimototalitem,unidade,Descontoperc,
	descontovalor,vendedor,icms,tributacao,total,cfop,cstcofins,cstpis,serieNF,subserienf,modelodocfiscal,cancelado,DATA,operador,grade,ratfrete,ratseguro,ratdespesas,cstipi,qUnidIPI,vUnidIPI,ncm,nbm,ncmespecie,origem,
	pis,cofins,aliquotaIPI,cenqipi,itemDAV,canceladoECF,vendaatacado,ratdesc)
	SELECT "N",codigofilial,classe,codigobarras,codigofiscal,comissao,_terminal,lote,romaneio,tipo,embalagem,nrcontrole,codigo,produto,quantidade,preco,custo,precooriginal,acrescimototalitem,unidade,Descontoperc,
	descontovalor,vendedor,icms,tributacao,total,cfop,cstcofins,cstpis,serieNF,subserienf,modelodocfiscal,cancelado,DATA,operador,grade,ratfrete,ratseguro,ratdespesas,'99','0','0',ncm,nbm,ncmespecie,origem,
	pis,cofins,'0',cenqipi,itemDAV,canceladoECF,vendaatacado,ratdesc FROM vendaarquivo AS v WHERE documento = _documento AND cancelado = 'N' AND quantidade > 0 AND codigoFilial = _codigoFilial;
	
	INSERT INTO caixas(EnderecoIP,valor,caixa,historico,DATA,tipopagamento,operador,dpfinanceiro,filialorigem,CodigoFilial,vencimento,vendedor)
	SELECT  _terminal,valor,caixa,historico,DATA,tipopagamento,operador,dpfinanceiro,filialorigem,CodigoFilial,vencimento,vendedor FROM caixaarquivo WHERE documento =_documento AND codigoFilial = _codigoFilial;
END IF;
END */$$
DELIMITER ;

/* Procedure structure for procedure `obterSaldo` */

/*!50003 DROP PROCEDURE IF EXISTS  `obterSaldo` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `obterSaldo`(IN _codigoFilial CHAR(5),IN _operador VARCHAR(100))
BEGIN
	DECLARE _saldo DECIMAL(10,2) DEFAULT 0;
	
	IF((SELECT saldoporoperador FROM configfinanc WHERE codigoFilial = _codigoFilial LIMIT 1) = "S") THEN
	
                    IF((SELECT mostrarsaldoliquido FROM configfinanc WHERE codigoFilial = _codigoFilial LIMIT 1) = "S")THEN
                        SELECT saldoLiquidoEspecie INTO _saldo FROM caixassoma WHERE operador=_operador AND inc=(SELECT MAX(inc) FROM caixassoma WHERE operador=_operador AND codigoFilial = _codigoFilial) AND codigoFilial = _codigoFilial;
                    ELSE
                        SELECT saldocaixa INTO _saldo FROM caixassoma WHERE operador=_operador AND inc=(SELECT MAX(inc) FROM caixassoma WHERE operador=_operador AND codigoFilial = _codigoFilial) AND codigoFilial = _codigoFilial;
		    END IF;
        ELSE           
		IF((SELECT COUNT(1) FROM caixa WHERE codigoFilial = _codigoFilial AND tipopagamento = 'SI') > 0)THEN
			
			SET @_saldo = 0;
		ELSE
			
		    IF((SELECT mostrarsaldoliquido FROM configfinanc WHERE codigoFilial = _codigoFilial LIMIT 1) = "S") THEN
                        SELECT SUM(saldoLiquidoEspecie) INTO _saldo  FROM caixassoma WHERE DATA=(SELECT MAX(DATA) FROM caixassoma WHERE codigoFilial = _codigoFilial AND DATA <> CURRENT_DATE) AND codigoFilial = _codigoFilial;
                    ELSE
                        SELECT SUM(saldocaixa) INTO _saldo  FROM caixassoma WHERE DATA=(SELECT MAX(DATA) FROM caixassoma WHERE codigoFilial = _codigoFilial AND DATA <> CURRENT_DATE) AND codigoFilial = _codigoFilial;
                    END IF;
		END IF;
 
	END IF;
	
	SELECT IFNULL(_saldo,0) AS saldo; 
		
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarDAV` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarDAV` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`%` PROCEDURE `ProcessarDAV`(IN numeroDAV INT,IN ipTerminal VARCHAR(15),IN filial VARCHAR(5))
BEGIN
 
DECLARE done INT DEFAULT FALSE;
DECLARE idSeq INT DEFAULT 0;
DECLARE seq INT DEFAULT 1;
DECLARE cursorSeq CURSOR FOR SELECT inc FROM vendas WHERE vendas.id=ipTerminal ;
DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
 SET @tabelaProduto='produtosfilial';
 IF (filial='00001') THEN
 SET @tabelaProduto='produtos';
 END IF;
 
 
 UPDATE vendadav 
 SET 	codigofilial=filial,id=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial;
 
 INSERT INTO `vendas` (`acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`,`idfornecedor`,`icmsst`, `mvast`, `cfop`,`dataalteracao`,`horaalteracao`,`tipoalteracao`,`ncm`,`canceladoECF`,vendaatacado) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,ipTerminal,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`,`idfornecedor`,`icmsst`, `mvast`, `cfop`,`dataalteracao`,`horaalteracao`,`tipoalteracao`,`ncm`,canceladoECF,vendaatacado
 FROM vendadav WHERE documento=numeroDAV AND codigofilial=filial AND documento>0 AND itemdav='S';
 
 OPEN cursorSeq;
	read_loop: LOOP
		FETCH cursorSeq INTO idSeq;	
		IF done THEN
			LEAVE read_loop;
		END IF;
	
	UPDATE vendas SET nrcontrole=seq 
	WHERE vendas.inc = idSeq;
	
	SET seq = seq+1;
	
	
	
END LOOP;
	CLOSE cursorSeq;	
 
 UPDATE caixadav 
 SET	codigofilial=filial,enderecoip=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial AND documento>0;
 
 INSERT INTO `caixas` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixadav WHERE documento=numeroDAV AND codigofilial=filial AND documento>0;
 
 
 SET @qVenda = CONCAT('UPDATE vendas,',@tabelaProduto,' set  
 vendas.quantidadeanterior=',@tabelaProduto,'.quantidade,
 vendas.quantidadeatualizada=',@tabelaProduto,'.quantidade-vendas.quantidade	
 where vendas.id=','"',ipTerminal,'" and vendas.codigo=',@tabelaProduto,'.codigo 
 and ',@tabelaProduto,'.codigofilial=',filial);	
 PREPARE st FROM @qVenda;
 EXECUTE st;
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarPedidoCompra` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarPedidoCompra` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `ProcessarPedidoCompra`(in numeroEntrada int,in pedCompra int,in encerrarPedido char(1))
BEGIN
 UPDATE pedidocompra,entradas
SET pedidocompra.qtdrecebida=entradas.quantidade 
 WHERE entradas.numero=numeroEntrada
 AND pedidocompra.numero=pedCompra
 AND entradas.codigo=pedidocompra.codigo
 AND entradas.codigofilial=pedidocompra.codigofilial;
 
 UPDATE pedidocompra SET pedidocompra.diferencaqtdrecebida=pedidocompra.qtdrecebida-pedidocompra.quantidade
 WHERE pedidocompra.numero=pedCompra AND pedidocompra.codigofilial=(Select entradas.codigofilial from entradas WHERE entradas.numero=numeroEntrada limit 1);
 
 
 if (encerrarPedido="S") then
 
 if ( (select count(1) from pedidocompra where diferencaqtdrecebida<>0 and numero=pedCompra and codigofilial=(SELECT entradas.codigofilial FROM entradas WHERE entradas.numero=numeroEntrada LIMIT 1))=0) then 
	UPDATE contpedido SET encerrado='S'
	WHERE numero=pedCompra;
 end if;
 end if;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarDAVOS` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarDAVOS` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `ProcessarDAVOS`(in numeroDAV int,in ipTerminal varchar(15),in filial varchar(5))
BEGIN
 UPDATE vendadavos 
 SET 	codigofilial=filial,id=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial;
INSERT INTO `vendas` (`acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`,`idfornecedor`,`icmsst`, `mvast`, `cfop`,`dataalteracao`,`horaalteracao`,`tipoalteracao`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,ipTerminal,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`,`idfornecedor`,`icmsst`, `mvast`, `cfop`,`dataalteracao`,`horaalteracao`,`tipoalteracao`
 FROM vendadavos WHERE documento=numeroDAV AND codigofilial=filial AND documento>0;
 
 
 UPDATE caixadavos 
 SET	codigofilial=filial,enderecoip=ipTerminal
 WHERE 	documento=numeroDAV AND codigofilial=filial;
 
 INSERT INTO `caixas` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixadavos where documento=numeroDAV AND codigofilial=filial;
 END */$$
DELIMITER ;

/* Procedure structure for procedure `QuitarDebitoClienteBoletos` */

/*!50003 DROP PROCEDURE IF EXISTS  `QuitarDebitoClienteBoletos` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `QuitarDebitoClienteBoletos`(IN ipTerminal VARCHAR(15),IN filial VARCHAR(5),in operadorLanc varchar(5))
BEGIN
DECLARE totalBaixa REAL DEFAULT 0;
declare totalDescontos real default 0;
declare totalTarifa real default 0;
DECLARE totalJuros REAL DEFAULT 0;
DECLARE valorAbatido REAL DEFAULT 0;
DECLARE interpolador VARCHAR(10);
set interpolador=(SELECT FLOOR(10000000+(RAND()*(90000000-10000000))));
SET totalBaixa = (SELECT SUM(valor) from retornoboleto WHERE baixado="N");
SET totalDescontos = (SELECT SUM(descontos) FROM retornoboleto WHERE baixado="N");
SET totalTarifa = (SELECT SUM(tarifa) FROM retornoboleto WHERE baixado="N");
SET totalJuros = (SELECT SUM(juros) FROM retornoboleto WHERE baixado="N");
 
DELETE FROM caixas WHERE caixas.EnderecoIP=ipTerminal;
 
INSERT INTO contdocs (ip,codigofilial,DATA,dataexe,totalbruto,desconto,encargos,total,nome,
codigocliente,NrParcelas,vendedor,operador,observacao,classe,
dpfinanceiro,vrjuros,tipopagamento,devolucaorecebimento,classedevolucao,historico,hora,devolucaonumero) 
 VALUES (ipTerminal,filial,CURRENT_DATE,CURRENT_DATE,totalBaixa,totalDescontos,0,totalBaixa,"",
0,NULL,'000',operadorLanc,'Recebimento bol',NULL,'Recebimento',0,'BL',0,' ','Baixa retorno boletos',CURRENT_TIME,'0');
update crmovclientes,retornoboleto set 
crmovclientes.ultvaloratual=crmovclientes.valoratual,
crmovclientes.valoratual=0,
crmovclientes.valorcorrigido=crmovclientes.Valorcorrigido-crmovclientes.valorpago,
crmovclientes.datapagamento = current_date,
crmovclientes.ultvencimento=crmovclientes.datacalcjuros,
crmovclientes.datacalcjuros = current_date,
crmovclientes.ultporconta=crmovclientes.porconta,
crmovclientes.porconta = crmovclientes.valor,
crmovclientes.vrultpagamento = crmovclientes.valorpago,
crmovclientes.ultjurospago=crmovclientes.jurospago,
crmovclientes.jurospago = crmovclientes.jurospago+crmovclientes.vrjuros,
crmovclientes.ultcaprec=crmovclientes.vrcapitalrec,
crmovclientes.vrcapitalrec = crmovclientes.porconta-crmovclientes.jurospago,
crmovclientes.ultjurosacumulado=crmovclientes.jurosacumulado,
crmovclientes.desconto=retornoboleto.descontos,
crmovclientes.jurosacumulado=crmovclientes.valoratual-crmovclientes.valor+crmovclientes.vrcapitalrec,
crmovclientes.tipopagamento='BL',
crmovclientes.filialpagamento=filial,
crmovclientes.quitado='S',
crmovclientes.sequencia = interpolador
 where crmovclientes.codigo=retornoboleto.codigocliente 
and retornoboleto.baixado='N'
and retornoboleto.vencimento=crmovclientes.vencimento 
and  ABS(retornoboleto.documento)=crmovclientes.bloquete 
and crmovclientes.valoratual>0;
update retornoboleto set
documentobaixa=(SELECT MAX(documento) from contdocs where contdocs.ip=ipTErminal),
interpolador=interpolador,
databaixa=current_date,
baixado='S'
where baixado='N';
insert into caixa(codigofilial,enderecoIP,nome,codigocliente,valor,
vrjuros,vrdesconto,valortarifabloquete,dataexe,data,vencimento,
tipopagamento,operador,dpfinanceiro) 
values (filial,ipTerminal,"","0",totalBaixa,totalJuros,totalDescontos,totalTarifa,
current_date,current_date,current_date,"BL",operadorLanc,"Recebimento bol");
 
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarDevolucao` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarDevolucao` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `ProcessarDevolucao`(IN doc INT,IN filial VARCHAR(5), IN ipTerminal VARCHAR(15),IN devolucaoNR INT, IN operadorAcao VARCHAR(10))
BEGIN
 SET @tabelaProduto='produtosfilial';
 IF (filial='00001') THEN
 SET @tabelaProduto='produtos';
 END IF;
 SET @qDevolucao= CONCAT('update ',@tabelaProduto,',devolucao set '
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
 precooriginal,customedio,total,vendedor,comissao,DATA,documento,custo,
 grupo,subgrupo,situacao,numerodevolucao,grade) 
 SELECT filial,ipTerminal,operadorAcao,
 codigo,produto,quantidade*-1,precooriginal*-1,preco*-1,customedio*-1,
 total*-1,vendedor,comissao,CURRENT_DATE,doc,
 custo*-1,grupo,subgrupo,situacao,devolucaoNR,grade
 FROM 	devolucao WHERE numero=devolucaoNR
 AND	finalizada='N';
 UPDATE	contdocs SET observacao=(SELECT observacao FROM devolucao WHERE numero=devolucaoNR AND observacao<>'' LIMIT 1),
 devolucaonumero=devolucaoNR WHERE documento=doc;
 UPDATE devolucao SET finalizada='S',DATA=CURRENT_DATE WHERE numero=devolucaoNR;
 UPDATE contdevolucao SET finalizada='S',DATA=CURRENT_DATE WHERE numero=devolucaoNR;
 
SET @qCustoMedio = CONCAT('update venda,',@tabelaProduto,' 
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
	SET @dpFinanceiro = "";
	IF((SELECT COUNT(1) FROM movdespesas WHERE codigofilial =filial AND devolucaonumero = devolucaoNR AND sangria = 'S') > 0)THEN
		SET @dpFinanceiro = "Sangria";
	END IF;
	
	INSERT INTO caixa (documento,enderecoip,codigofilial,valor,DATA,dataexe,tipopagamento,operador,vendedor,dpfinanceiro)
	VALUES (0,ipTerminal,filial,(SELECT IFNULL(SUM(preco*quantidade),0) FROM devolucao WHERE numero=devolucaoNR),CURRENT_DATE,CURRENT_DATE,"DV",operadorAcao,
	(SELECT vendedor FROM devolucao WHERE numero=devolucaoNR LIMIT 1),@dpFinanceiro);
	
	UPDATE venda SET dpfinanceiro=@dpFinanceiro WHERE numerodevolucao = devolucaoNR;
ELSE 
	UPDATE venda SET dpfinanceiro=IFNULL((SELECT dpfinanceiro FROM contdocs WHERE documento = doc),"Venda") WHERE numerodevolucao = devolucaoNR AND documento = doc;
END IF;
 UPDATE  produtosgrade,devolucao SET 
 produtosgrade.quantidade=produtosgrade.quantidade+devolucao.quantidade 
 WHERE 	devolucao.numero=devolucaoNR
 AND 	devolucao.numero>0
 AND 	produtosgrade.codigo=devolucao.codigo
 AND 	produtosgrade.grade=devolucao.grade 
 AND 	produtosgrade.codigofilial=filial;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `QuitarDebitoClientePorValor` */

/*!50003 DROP PROCEDURE IF EXISTS  `QuitarDebitoClientePorValor` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `QuitarDebitoClientePorValor`(IN idCliente INT,in idNumero int,IN idRecebimento INT,IN ipTerminal VARCHAR(15),IN filial VARCHAR(5),in valorPago decimal (10,2),in _operador varchar(10))
BEGIN
declare valorRestante real default 0;
DECLARE valorAbatido REAL DEFAULT 0;
set valorRestante = valorPago;
 WHILE valorRestante>0 DO
set valorAbatido = (select valoratual from crmovclientes  WHERE quitado="N" AND codigo=idcliente  order by sequenciainc LIMIT 1);
update crmovclientes set valorpago=if(valorcorrigido<valorRestante,valorcorrigido,valorRestante), quitado="S",ip=ipTerminal where quitado="N" and codigo=idcliente ORDER BY sequenciainc limit 1;
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
 END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarPreVenda` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarPreVenda` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `ProcessarPreVenda`(in numeroPreVenda int,in ipTerminal varchar(15),in filial varchar(5))
BEGIN
 UPDATE vendaprevendapaf 
 SET 	codigofilial=filial,id=ipTerminal
 WHERE 	documento=numeroPreVenda AND codigofilial=filial;
 
 INSERT INTO `vendas` (`acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`,`idfornecedor`,`icmsst`, `mvast`, `cfop`,`dataalteracao`,`horaalteracao`,`tipoalteracao`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,ipTerminal,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`,`aliquotaipi`,`idfornecedor`,`icmsst`, `mvast`, `cfop`,`dataalteracao`,`horaalteracao`,`tipoalteracao`
 FROM vendaprevendapaf WHERE documento=numeroPreVenda AND codigofilial=filial AND documento>0;
  
 
 UPDATE caixaprevendapaf 
 SET	codigofilial=filial,enderecoip=ipTerminal
 WHERE 	documento=numeroPreVenda AND codigofilial=filial;
 
 INSERT INTO `caixas` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixaprevendapaf where documento=numeroPreVenda AND codigofilial=filial;
 
 END */$$
DELIMITER ;

/* Procedure structure for procedure `resumoCaixas` */

/*!50003 DROP PROCEDURE IF EXISTS  `resumoCaixas` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `resumoCaixas`(in dataMovimento date)
BEGIN
 
 SET @movimento=0.00;
SET @saldoInicial=0.00;
SET @receitas=0.00;
SET @sangrias=0.00;
SET @dataini=dataMovimento;
DROP TABLE IF EXISTS fluxo_caixas_abertos;
CREATE  TABLE fluxo_caixas_abertos
SELECT codigofilial,SUM(valor) AS saldoInicial,@movimento AS Movimento,
@receitas AS Receitas,@sangrias AS sangrias 
FROM caixa
WHERE DATA =@dataini AND tipopagamento IN ('SI')
GROUP BY codigofilial
UNION
SELECT codigofilial,SUM(valor)AS saldoInicial,@movimento AS Movimento,
@receitas AS Receitas,@sangrias AS sangrias 
FROM caixaarquivo
WHERE DATA=@dataini AND tipopagamento IN ('SI')
GROUP BY codigofilial
UNION
SELECT codigofilial,@saldoInicial AS saldoInicial,SUM(valor)AS Movimento,
@receitas AS Receitas,@sangrias AS sangrias 
FROM caixa
WHERE DATA=@dataini AND tipopagamento IN ('CA','CH','DH')
GROUP BY codigofilial
UNION
SELECT codigofilial,@saldoInicial AS saldoInicial,SUM(valor)AS Movimento,
@receitas AS Receitas,@sangrias AS sangrias 
FROM caixaarquivo
WHERE DATA=@dataini AND tipopagamento IN ('CA','CH','DH')
GROUP BY codigofilial
UNION
SELECT codigofilial,@saldoInicial AS saldoInicial,@movimento AS Movimento,
SUM(valor) AS Receitas,@sangrias AS sangrias 
FROM movreceitas
WHERE DATA=@dataini
GROUP BY codigofilial
UNION
SELECT codigofilial,@saldoInicial AS saldoInicial,@movimento AS Movimento,
@receitas AS Receitas,SUM(valor)AS sangrias 
FROM movdespesas
WHERE DATA=@dataini
AND sangria="S"
GROUP BY codigofilial
;
SELECT fluxo_caixas_abertos.codigofilial AS Filial,
filiais.descricao AS Descricao,
ROUND(SUM(saldoInicial),2) AS saldoInicial,
ROUND(SUM(Movimento),2) AS Movimento,
ROUND(SUM(Receitas),2) AS Receitas,
ROUND(SUM(sangrias),2) AS Sangrias, 
ROUND(SUM(saldoInicial+movimento+Receitas)-SUM(sangrias),2) AS saldoFinal ,
telefone1 AS telefoneLoja
FROM  fluxo_caixas_abertos,filiais
WHERE fluxo_caixas_abertos.codigofilial=filiais.CodigoFilial
GROUP BY fluxo_caixas_abertos.codigofilial
UNION
SELECT @Filial:='TODAS' AS Filial,
@descricao:='TOTAL GERAL :' AS Descricao,
ROUND(SUM(saldoInicial),2) AS saldoInicial,
ROUND(SUM(Movimento),2) AS Movimento,
ROUND(SUM(Receitas),2) AS Receitas,
ROUND(SUM(sangrias),2) AS Sangrias, 
ROUND(SUM(saldoInicial+movimento+Receitas)-SUM(sangrias),2) AS saldoFinal,
@Telefone:=SPACE(10) AS telefoneLoja 
FROM  fluxo_caixas_abertos,filiais
WHERE fluxo_caixas_abertos.codigofilial=filiais.CodigoFilial
GROUP BY '';
    END */$$
DELIMITER ;

/* Procedure structure for procedure `ProcessarTransferencia` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarTransferencia` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `ProcessarTransferencia`(
  IN numeroTransf INT,
  IN filial VARCHAR (5),
  IN filialOrigem VARCHAR (5),
  IN filialDestino VARCHAR (5),  
  IN operadorLanc VARCHAR (10),
  in estoqueOrigem char(1),
  in estoqueDestino char(1)
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
 
 
 
 if(estoqueOrigem = "S")then
 
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
  
 
 UPDATE 
    produtosgrade,
    movtransf 
  SET
    produtosgrade.quantidade = produtosgrade.quantidade - 
    (SELECT 
      SUM(movtransf.quantidade) 
    FROM
      movtransf 
    WHERE movtransf.codigo = produtosgrade.codigo 
      AND movtransf.grade = produtosgrade.grade 
      AND movtransf.numero = numeroTransf) 
  WHERE movtransf.numero = numeroTransf 
    AND produtosgrade.codigo = movtransf.codigo 
    AND produtosgrade.grade = movtransf.grade 
    AND produtosgrade.codigofilial = filialOrigem 
    AND movtransf.grade <> 'nenhuma' ;
end if;
 
 
 
 
 
 if(estoqueDestino = "S")then
 
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
  
 UPDATE 
    produtosgrade,
    movtransf 
  SET
    produtosgrade.quantidade = produtosgrade.quantidade + 
    (SELECT 
      SUM(movtransf.quantidade) 
    FROM
      movtransf 
    WHERE movtransf.codigo = produtosgrade.codigo 
      AND movtransf.grade = produtosgrade.grade 
      AND movtransf.numero = numeroTransf) 
  WHERE movtransf.numero = numeroTransf 
    AND produtosgrade.codigo = movtransf.codigo 
    AND produtosgrade.grade = movtransf.grade 
    AND produtosgrade.codigofilial = filialDestino 
    AND movtransf.grade <> 'nenhuma' ;
  
 UPDATE 
    movtransf 
  SET
    lancada = 'X',
    DATA = CURRENT_DATE,
    usuario = operadorLanc 
  WHERE numero = numeroTransf ;
  UPDATE 
    conttransf 
  SET
    lancada = 'X',
    totcusto = (SELECT TRUNCATE(SUM(custo * quantidade),2) FROM movtransf WHERE numero = numeroTransf),
    totvenda = (SELECT TRUNCATE(SUM(preco * quantidade),2) FROM movtransf WHERE numero = numeroTransf)
  WHERE numero = numeroTransf;
 
 end if;
 
 
IF ((SELECT transferenciatransito FROM configfinanc WHERE codigofilial=filialOrigem LIMIT 1)='S' and estoqueOrigem = "S") THEN 
	
	INSERT INTO conttransftransito(transferencia, emtransito, datasaida, filialorigem, filialdestino) VALUES (numeroTransf, 'S', CURRENT_DATE, filialOrigem, filialDestino);
	INSERT INTO movtransftransito(transferencia, codigoproduto, descricao, quantidade, encerrada, filialorigem, filialdestino) SELECT numero, codigo, descricao, quantidade, 'N', filialOrigem, filialDestino FROM movtransf WHERE numero=numeroTransf;
	
	if filialdestino="00001" then
		UPDATE produtos AS b, movtransftransito AS m SET b.qtdemtransito=b.qtdemtransito+m.quantidade 
		WHERE b.codigo=m.codigoproduto 
		AND b.CodigoFilial=m.filialdestino
		AND m.transferencia=numeroTransf;
	end if;
	
	IF filialdestino<>"00001" THEN
		UPDATE produtosfilial AS b, movtransftransito AS m SET b.qtdemtransito=b.qtdemtransito+m.quantidade 
		WHERE b.codigo=m.codigoproduto 
		AND b.CodigoFilial=m.filialdestino
		AND m.transferencia=numeroTransf;
	END IF;
	
	
END IF ;
IF ((SELECT transferenciatransito FROM configfinanc WHERE codigofilial=filialOrigem LIMIT 1)='S' AND estoqueDestino = "S") THEN 
 
		
	IF filialdestino="00001" THEN
		UPDATE produtos AS b, movtransftransito AS m SET b.qtdemtransito=b.qtdemtransito-m.quantidade 
		WHERE b.codigo=m.codigoproduto 
		AND b.CodigoFilial=m.filialdestino
		AND m.transferencia=numeroTransf;
	END IF;
	
	IF filialdestino<>"00001" THEN
		UPDATE produtosfilial AS b, movtransftransito AS m SET b.qtdemtransito=b.qtdemtransito-m.quantidade 
		WHERE b.codigo=m.codigoproduto 
		AND b.CodigoFilial=m.filialdestino
		AND m.transferencia=numeroTransf;
	END IF;
	
END IF ;
 
END */$$
DELIMITER ;

/* Procedure structure for procedure `recriarCodigoProduto` */

/*!50003 DROP PROCEDURE IF EXISTS  `recriarCodigoProduto` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `recriarCodigoProduto`(IN filial VARCHAR(5), IN novoCodigo VARCHAR(20))
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
 
 IF ( (SELECT COUNT(1) FROM produtos WHERE codigo=novoCodigo AND codigofilial=filial )=0) THEN
 INSERT INTO produtos(codigofilial,codigo,codigobarras,descricao,unidade,unidembalagem,grupo,subgrupo,fornecedor,icms,tributacao,custo,precovenda,pis,cofins,tributacaopis,tributacaocofins,pisentrada,cofinsentrada,cstpisentrada,cstcofinsentrada,situacao,percentualRedBaseCalcICMS,aceitadesconto,ativacompdesc,ncm,nbm,ncmespecie )
 SELECT filial,codigo," " ,descricao,unidade,unidade,grupo,subgrupo, fornecedor,icms,tributacao,custo,precovenda,pis,cofins, tributacaopis,tributacaocofins,pisentrada,cofinsentrada,cstpisentrada,cstcofinsentrada,
 "Excluído",percentualRedBaseCalcICMS ,"S","N"," "," "," "
 FROM produtosinventario WHERE codigo=novoCodigo LIMIT 1;
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
 
 IF ( (SELECT COUNT(1) FROM produtos WHERE codigo=novoCodigo AND codigofilial=filial )=0) THEN
 INSERT INTO produtos(codigofilial,codigo,codigobarras,descricao,unidade,unidembalagem,grupo,subgrupo,fornecedor,icms,tributacao,custo,precovenda,pis,cofins,tributacaopis,tributacaocofins,pisentrada,cofinsentrada,cstpisentrada,cstcofinsentrada,situacao,percentualRedBaseCalcICMS,aceitadesconto,ativacompdesc,ncm,nbm,ncmespecie )
 SELECT filial,codigo," " ,descricao,unidade,unidade,grupo,subgrupo, fornecedor,icms,tributacao,custo,precovenda,pis,cofins, tributacaopis,tributacaocofins,pisentrada,cofinsentrada,cstpisentrada,cstcofinsentrada,
 "Excluído",percentualRedBaseCalcICMS ,"S","N"," "," "," "
 FROM produtosinventario WHERE codigo=novoCodigo LIMIT 1;
 END IF;
 
 
 
END IF;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_gravaInfoGerencial` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_gravaInfoGerencial` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `sp_gravaInfoGerencial`(IN filial VARCHAR(5))
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

/* Procedure structure for procedure `zajustes` */

/*!50003 DROP PROCEDURE IF EXISTS  `zajustes` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `zajustes`()
BEGIN
 
UPDATE vendadav SET 
vendadav.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")))
;
UPDATE vendatmp SET 
vendatmp.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")))
;
UPDATE vendaarquivo SET 
vendaarquivo.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")))
;
UPDATE venda SET 
venda.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")))
;
 UPDATE contdocs SET concluido='S',
 EADr06=MD5(CONCAT(IFNULL(ecffabricacao,""),IFNULL(ncupomfiscal,""),IFNULL(contadornaofiscalGNF,""),IFNULL(contadordebitocreditoCDC,""),DATA,IFNULL(coognf,""),tipopagamento,IFNULL(ecfcontadorcupomfiscal,""),ecftotalliquido,estornado )),
 EADRegistroDAV=MD5(CONCAT(IFNULL(ncupomfiscal,""),davnumero,DATA,total)),
 estoqueatualizado='S';
 
 UPDATE contdav SET 
EADRegistroDAV=MD5(CONCAT(ncupomfiscal,numeroDAVFilial,DATA,valor,IFNULL(numeroECF,"001"),IFNULL(contadorRGECF,""),IFNULL(cliente,""),IFNULL(ecfCPFCNPJconsumidor,"")));
  
UPDATE produtos SET EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,datafinalestoque,horafinalestoque,ecffabricacao)),EADP2relacaomercadoria=MD5(CONCAT(codigo,descricao,STecf,icms,precovenda,precoatacado));
UPDATE produtosfilial SET EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,datafinalestoque,horafinalestoque,ecffabricacao)),EADP2relacaomercadoria=MD5(CONCAT(codigo,descricao,STecf,icms,precovenda,precoatacado));
 
CALL gravarR(CURRENT_DATE);
UPDATE r01 SET eaddados=MD5(CONCAT(fabricacaoECF,cnpj,cnpjdesenvolvedora,aplicativo,r01.MD5));
UPDATE r02 SET eaddados=MD5(CONCAT(fabricacaoECF,crz,coo,cro,DATA,dataemissaoreducaoz,horaemissaoreducaoz,vendabrutadiaria));
UPDATE r03 SET eaddados=MD5(CONCAT(fabricacaoECF,CRZ,totalizadorParcial));
CALL AtualizarQdtRegistros();
    END */$$
DELIMITER ;

/* Procedure structure for procedure `verificaVenda` */

/*!50003 DROP PROCEDURE IF EXISTS  `verificaVenda` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `verificaVenda`(IN _documento INT,IN _terminal  VARCHAR(30),IN _codigoFilial CHAR(5),IN _cancelar CHAR(1))
BEGIN
		DECLARE totalVenda DECIMAL(12,2) DEFAULT 0;
		DECLARE aprovada CHAR(1) DEFAULT "S";
		
		DECLARE _filial CHAR(5);
		DECLARE _operador VARCHAR(10);
		DECLARE _cooECF VARCHAR(10);
		DECLARE _ccfECF VARCHAR(10);
		DECLARE _motivoObs VARCHAR(10);
		DECLARE _usuarioSolicitante VARCHAR(10);
	
		SET @aprovada = "S";
	
		IF((SELECT dpfinanceiro FROM contdocs WHERE documento = _documento) = 'Venda')THEN
	
	
				IF((SELECT estornado FROM contdocs WHERE documento = _documento) <> "S" AND (SELECT DATA FROM contdocs WHERE documento = _documento) = CURRENT_DATE) THEN
				
					IF((SELECT tipopagamento FROM contdocs WHERE documento = _documento) <> 'DV')THEN
	
						IF((SELECT ecffabricacao FROM contdocs WHERE documento = _documento) IS NULL)THEN
							IF((SELECT IF(SUM(total + devolucaovenda) IS NULL,'0',SUM(total + devolucaovenda)) FROM contdocs WHERE documento = _documento) <> (SELECT TRUNCATE(SUM((total + rateioencargos) - ratdesc),2) FROM venda WHERE total > 0 AND documento= _documento AND cancelado = 'N'))THEN
								IF(ABS(((SELECT TRUNCATE(SUM((total + rateioencargos) - ratdesc),2) FROM venda WHERE total > 0 AND documento= _documento AND cancelado = 'N') - (SELECT IF(SUM(total + devolucaovenda) IS NULL,'0',SUM(total + devolucaovenda)) FROM contdocs WHERE documento = _documento))) > 0.10)THEN
									SET @aprovada = "N";
								END IF;
							END IF;
						ELSE
							IF((SELECT IF(SUM(total) IS NULL,'0',SUM(total)) FROM contdocs WHERE documento = _documento) <> (SELECT TRUNCATE(SUM((total + rateioencargos) - ratdesc),2) FROM venda WHERE total > 0 AND documento= _documento AND cancelado = 'N'))THEN
								IF(ABS(((SELECT TRUNCATE(SUM((total + rateioencargos) - ratdesc),2) FROM venda WHERE total > 0 AND documento= _documento AND cancelado = 'N') - (SELECT IF(SUM(total) IS NULL,'0',SUM(total)) FROM contdocs WHERE documento = _documento))) > 0.10)THEN
									SET @aprovada = "N";
								END IF;
							END IF;
							
						END IF;
						
	
						IF((SELECT ecffabricacao FROM contdocs WHERE documento = _documento) IS NULL)THEN
							IF((SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM caixa WHERE documento= _documento) <> (SELECT IF(SUM(total) IS NULL,'0',SUM(total + devolucaovenda)) FROM contdocs WHERE documento = _documento))THEN
								SET @aprovada = "N";
							END IF;
						ELSE 
							IF((SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM caixa WHERE documento= _documento) <> (SELECT IF(SUM(total) IS NULL,'0',SUM(total)) FROM contdocs WHERE documento = _documento))THEN
								SET @aprovada = "N";
							END IF;
						END IF;
						
						
						IF((SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM caixa WHERE documento= _documento AND tipopagamento='CR') <> 
						   (SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM crmovclientes WHERE documento = _documento))THEN
		
							IF((SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM caixa WHERE documento= _documento AND tipopagamento='CR') <> 
							(SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM crmovclientespagas WHERE documento = _documento))THEN
		
								SET @aprovada = "N";
		
							END IF;
						END IF;
					
						IF((SELECT IF(SUM(valor) IS NULL,'0',SUM(valor))FROM caixa WHERE documento= _documento AND (tipopagamento='CA' || tipopagamento = 'FN' || tipopagamento = 'FI' || tipopagamento = 'TI') AND dpfinanceiro = 'Venda') <> 
						   (SELECT IF(SUM(valor) IS NULL,'0',SUM(valor))FROM movcartoes WHERE documento = _documento))THEN
							SET @aprovada = "N";
						END IF;
					
						
						IF(@aprovada = "N" AND _cancelar = "S")THEN
							SELECT ip INTO _terminal FROM contdocs WHERE documento = _documento;
							SELECT codigoFilial INTO _filial FROM contdocs WHERE documento = _documento;
							SELECT operador INTO _operador FROM contdocs WHERE documento = _documento;
							SELECT ncupomfiscal INTO _cooECF FROM contdocs WHERE documento = _documento;
							SELECT ecfcontadorcupomfiscal INTO _ccfECF FROM contdocs WHERE documento = _documento;
							SET @_motivoObs = 'Cancelado pela verificação do Sistema ';
							SELECT operador INTO _usuarioSolicitante FROM contdocs WHERE documento = _documento;
							CALL ExcluirDocumento(_terminal,_documento,_filial,_operador,_cooECF,_ccfECF,@_motivoObs,_usuarioSolicitante);
						END IF;
					ELSE 
						SET @aprovada = "S";
					END IF;
			
				ELSE 
					
			
					IF((SELECT IF(SUM(total + devolucaovenda) IS NULL,'0',SUM(total + devolucaovenda)) FROM contdocs WHERE documento = _documento) <> (SELECT TRUNCATE(SUM((total + rateioencargos) - ratdesc),2) FROM vendaarquivo WHERE total > 0 AND documento= _documento AND cancelado = 'N'))THEN
						IF(ABS(((SELECT TRUNCATE(SUM((total + rateioencargos) - ratdesc),2) FROM vendaarquivo WHERE total > 0 AND documento= _documento AND cancelado = 'N') - (SELECT IF(SUM(total + devolucaovenda) IS NULL,'0',SUM(total + devolucaovenda)) FROM contdocs WHERE documento = _documento))) > 0.10)THEN
							SET @aprovada = "N";
						END IF;
					END IF;
					
					IF((SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM caixaarquivo WHERE documento= _documento) <> (SELECT IF(SUM(total + devolucaovenda) IS NULL,'0',SUM(total + devolucaovenda)) FROM contdocs WHERE documento = _documento))THEN
						SET @aprovada = "N";
					END IF;
					
					
					IF((SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM caixaarquivo WHERE documento= _documento AND tipopagamento='CR') <> 
					   (SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM crmovclientes WHERE documento = _documento))THEN
						IF((SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM caixaarquivo WHERE documento= _documento AND tipopagamento='CR') <> 
							(SELECT IF(SUM(valor) IS NULL,'0',SUM(valor)) FROM crmovclientespagas WHERE documento = _documento))THEN
	
							SET @aprovada = "N";
						END IF;
					END IF;
				
					IF((SELECT IF(SUM(valor) IS NULL,'0',SUM(valor))FROM caixaarquivo WHERE documento= _documento AND (tipopagamento='CA' || tipopagamento = 'FN' || tipopagamento = 'FI' || tipopagamento = 'TI') AND dpfinanceiro = 'Venda') <> 
					   (SELECT IF(SUM(valor) IS NULL,'0',SUM(valor))FROM movcartoes WHERE documento = _documento))THEN
						SET @aprovada = "N";
					END IF;
				
					
						SELECT ip INTO _terminal FROM contdocs WHERE documento = _documento;
						SELECT codigoFilial INTO _filial FROM contdocs WHERE documento = _documento;
						SELECT operador INTO _operador FROM contdocs WHERE documento = _documento;
						SELECT ncupomfiscal INTO _cooECF FROM contdocs WHERE documento = _documento;
						SELECT ecfcontadorcupomfiscal INTO _ccfECF FROM contdocs WHERE documento = _documento;
						SET @_motivoObs = 'Cancelado pela verificação do Sistema ';
						SELECT operador INTO _usuarioSolicitante FROM contdocs WHERE documento = _documento;
			
					
				END IF;
		END IF;
		
		IF(_documento = 0)THEN
			SET @aprovada = "N";
		END IF;
	
		IF(@aprovada = "N" AND _cancelar = "S")THEN
	
			INSERT INTO auditoria(usuario,usuariosolicitante,hora,DATA,tabela,acao,documento,observacao,codigoFilial,LOCAL,codigoproduto)
			VALUES 
			((SELECT operador FROM contdocs WHERE ip = _terminal LIMIT 1),
			 (SELECT operador FROM contdocs WHERE ip = _terminal LIMIT 1),
		         CURRENT_TIME,CURRENT_DATE,'Venda','Finalizando venda',_documento,
			'Inconsistência na venda por segurança a mesma foi cancelada',
			_codigoFilial,'','');
	
			DELETE FROM vendas WHERE id = _terminal AND codigofilial = _codigoFilial;
		END IF;
	
		
		SELECT @aprovada AS aprovada;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `zerarPAF` */

/*!50003 DROP PROCEDURE IF EXISTS  `zerarPAF` */;

DELIMITER $$

/*!50003 CREATE  PROCEDURE `zerarPAF`()
BEGIN
 
UPDATE contdav SET 
EADRegistroDAV=MD5(CONCAT(TRIM(ecffabricacao), IFNULL(mfAdicional,""), IFNULL(tipoECF,""), marca, modelo, contadorRGECF, numeroDAVFilial, DATA, valor, ncupomfiscal, numeroECF, cliente, ecfCPFCNPJconsumidor));	
UPDATE contdocs SET
EADr06=MD5(CONCAT(ecffabricacao, ncupomfiscal, IFNULL(contadornaofiscalGNF,""), IFNULL(contadordebitocreditoCDC,""), DATA, COOGNF, tipopagamento, IFNULL(ecfcontadorcupomfiscal,""), ecftotalliquido, estornado,ecfMFadicional,ecfmodelo,IFNULL(ecfcontadorcupomfiscal,""),Totalbruto,desconto,encargos,IFNULL(ecfConsumidor,""),IFNULL(ecfCPFCNPJconsumidor,""))),
EADRegistroDAV=MD5(CONCAT(IFNULL(ncupomfiscal,""),davnumero,DATA,total)), estoqueatualizado='S' ;
UPDATE vendadav SET 
vendadav.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,""),IFNULL(horaalteracao,""),IFNULL(tipoalteracao,"")));
UPDATE vendaprevendapaf SET 
 eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),ecffabricacao));
UPDATE venda SET 
venda.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")));
UPDATE vendaarquivo SET 
vendaarquivo.eaddados=MD5(CONCAT(documento,DATA,nrcontrole,codigo,produto,quantidade,unidade,preco,descontovalor,acrescimototalitem,total,tributacao,descontoperc,cancelado,icms,IFNULL(ccf,""),IFNULL(coo,""),IFNULL(ecffabricacao,"")));
UPDATE produtos SET EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,datafinalestoque,horafinalestoque,ecffabricacao)),
EADP2relacaomercadoria=MD5(CONCAT(codigo,descricao,icms,precovenda,precoatacado,unidade,indicadorarredondamentotruncamento,indicadorproducao,tributacao));
UPDATE produtosfilial SET EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,datafinalestoque,horafinalestoque,ecffabricacao)),
EADP2relacaomercadoria=MD5(CONCAT(codigo,descricao,STecf,icms,precovenda,precoatacado));
UPDATE produtos SET eade1=MD5(CONCAT(datafinalestoque,horafinalestoque,ecffabricacao,codigo,descricao,unidade,saldofinalestoque));
UPDATE produtosfilial SET eade1=MD5(CONCAT(datafinalestoque,horafinalestoque,ecffabricacao,codigo,descricao,unidade,saldofinalestoque));
UPDATE N1 SET MD5 = MD5(CONCAT(cnpjdesenvolvedora,inscricaodesenvolvedora,inscricaomunicipaldesenvolvedora
,razaosocialdesenvolvedora));
UPDATE caixa SET eaddados = MD5(CONCAT(`ecffabricacao`,`coo`,`ccf`,gnf,ecfmodelo,valor,tipopagamento,DATA,tipodoc));
UPDATE caixaarquivo SET eaddados = MD5(CONCAT(`ecffabricacao`,`coo`,`ccf`,gnf,ecfmodelo,valor,tipopagamento,DATA,tipodoc));
UPDATE produtos AS p SET 
p.eade3 = MD5(CONCAT(p.datafinalestoque, p.horafinalestoque, 
(SELECT fabricacaoECF FROM r01 WHERE fabricacaoECF = 'DR0514BR000000440535'),
(SELECT tipoECF FROM r01 WHERE fabricacaoECF = 'DR0514BR000000440535'), 
(SELECT marcaECF FROM r01 WHERE fabricacaoECF = 'DR0514BR000000440535'),
(SELECT modeloECF FROM r01 WHERE fabricacaoECF = 'DR0514BR000000440535')));
UPDATE r01 SET EADdados = MD5(CONCAT(fabricacaoECF, cnpj , cnpjdesenvolvedora , aplicativo , MD5,MFAdicional,tipoECF,modeloECF,versaoSB,datainstalacaoSB,horainstalacaoSB,numeroECF,inscricao,inscricaodesenvolvedora,inscricaomunicipaldesenvolvedora,razaosocialdesenvolvedora,versao,versaoERPAF));
UPDATE r02 SET eaddados=MD5(CONCAT(fabricacaoECF,crz,coo,cro,DATA,dataemissaoreducaoz,horaemissaoreducaoz,vendabrutadiaria,modeloECF,MFadicional,datamovimento));
UPDATE r03 SET eaddados=MD5(CONCAT(fabricacaoECF, CRZ, totalizadorParcial, valoracumulado, numeroUsuarioSubstituicaoECF, modeloECF, MFAdicional));
UPDATE movdespesas SET EADDados = MD5(CONCAT(contadornaofiscalGNF, ncupomfiscalCOO, ecfcontadorcupomfiscal, tipopgamento, ECFfabricacao,modelo ,MfAdicionalECF,DATA,hora));
CALL AtualizarQdtRegistros();
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
 `totalicms` decimal(47,2) ,
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
 `totalicms` decimal(47,2) ,
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
 `totalicms` decimal(47,2) ,
 `descontovalor` decimal(32,2) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `baseCalculoICMS` decimal(37,2) 
)*/;

/*Table structure for table `analise_venda_caixa` */

DROP TABLE IF EXISTS `analise_venda_caixa`;

/*!50001 DROP VIEW IF EXISTS `analise_venda_caixa` */;
/*!50001 DROP TABLE IF EXISTS `analise_venda_caixa` */;

/*!50001 CREATE TABLE  `analise_venda_caixa`(
 `data` date ,
 `documento` int(10) ,
 `totalDocumento` decimal(12,2) ,
 `totalCaixa` decimal(32,2) ,
 `totalVenda` decimal(34,2) 
)*/;

/*Table structure for table `analisevalores` */

DROP TABLE IF EXISTS `analisevalores`;

/*!50001 DROP VIEW IF EXISTS `analisevalores` */;
/*!50001 DROP TABLE IF EXISTS `analisevalores` */;

/*!50001 CREATE TABLE  `analisevalores`(
 `documento` int(10) ,
 `totalDocumento` decimal(12,2) ,
 `totalCaixa` decimal(32,2) ,
 `totalVenda` decimal(33,2) 
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
 `totICMS` decimal(65,2) ,
 `baseCalculoIPI` decimal(59,2) ,
 `totalIPI` decimal(62,2) ,
 `bcPIS` decimal(62,2) ,
 `bcCOFINS` decimal(62,2) ,
 `totalPIS` decimal(65,2) ,
 `totalCOFINS` decimal(65,2) ,
 `bcICMSST` decimal(60,2) ,
 `totalICMSST` decimal(65,2) ,
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
 `totalicms` decimal(51,2) ,
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
 `totalICMSST` decimal(44,2) 
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
 `obsentrega` varchar(400) ,
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
 `totalICMS` decimal(24,6) 
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
 `totalICMS` decimal(44,2) ,
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
 `devolucaonumero` int(7) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(45,2) ,
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
 `devolucaonumero` int(7) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(45,2) ,
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
 `totalICMS` decimal(42,2) ,
 `notafiscal` varchar(15) 
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
 `devolucaonumero` int(7) ,
 `numeroPED` int(8) ,
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
 `totalICMS` decimal(44,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `totalIPI` decimal(37,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `tributacao` varchar(3) ,
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
 `devolucaonumero` int(7) ,
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
 `totalICMS` decimal(44,2) ,
 `baseCalculoIPI` decimal(34,2) ,
 `totalIPI` decimal(37,2) ,
 `totalPIS` decimal(36,2) ,
 `totalCOFINS` decimal(36,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `icms` int(2) ,
 `tributacao` varchar(3) ,
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
 `devolucaonumero` int(7) ,
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
 `totalICMS` decimal(44,2) ,
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
 `devolucaonumero` int(7) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(45,2) ,
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
 `devolucaonumero` int(7) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(45,2) ,
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
 `devolucaonumero` int(7) ,
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
 `totalICMS` decimal(44,2) ,
 `icms` int(2) ,
 `tributacao` varchar(3) ,
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
 `devolucaonumero` int(7) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(45,2) ,
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
 `devolucaonumero` int(7) ,
 `codigo` varchar(20) ,
 `descricao` varchar(50) ,
 `quantidade` decimal(30,3) ,
 `unidade` char(3) ,
 `preco` decimal(12,5) ,
 `descontovalor` decimal(10,2) ,
 `descontoperc` decimal(6,2) ,
 `total` decimal(34,2) ,
 `totalicms` decimal(45,2) ,
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

/*Table structure for table `blocomregm100_500` */

DROP TABLE IF EXISTS `blocomregm100_500`;

/*!50001 DROP VIEW IF EXISTS `blocomregm100_500` */;
/*!50001 DROP TABLE IF EXISTS `blocomregm100_500` */;

/*!50001 CREATE TABLE  `blocomregm100_500`(
 `valornota` decimal(40,2) ,
 `valorprodutos` decimal(32,2) ,
 `pis` decimal(8,3) ,
 `cstpis` char(2) ,
 `cofins` decimal(8,3) ,
 `cstcofins` char(2) ,
 `cfopentrada` varchar(5) ,
 `valorpis` decimal(38,2) ,
 `valorcofins` decimal(38,2) ,
 `dataentrada` date ,
 `codigofilial` varchar(5) 
)*/;

/*Table structure for table `blocomregm210_610` */

DROP TABLE IF EXISTS `blocomregm210_610`;

/*!50001 DROP VIEW IF EXISTS `blocomregm210_610` */;
/*!50001 DROP TABLE IF EXISTS `blocomregm210_610` */;

/*!50001 CREATE TABLE  `blocomregm210_610`(
 `total` decimal(35,4) ,
 `codigofilial` varchar(5) ,
 `tipo` varchar(10) ,
 `cstpis` char(2) ,
 `pis` decimal(5,3) ,
 `cstcofins` char(2) ,
 `cofins` decimal(5,3) ,
 `valorpis` decimal(36,2) ,
 `valorcofins` decimal(36,2) ,
 `DATA` date ,
 `cfop` varchar(5) 
)*/;

/*Table structure for table `posicaoestoquefiliais` */

DROP TABLE IF EXISTS `posicaoestoquefiliais`;

/*!50001 DROP VIEW IF EXISTS `posicaoestoquefiliais` */;
/*!50001 DROP TABLE IF EXISTS `posicaoestoquefiliais` */;

/*!50001 CREATE TABLE  `posicaoestoquefiliais`(
 `codigofilial` varchar(5) ,
 `codigo` varchar(20) ,
 `quantidade` decimal(10,2) ,
 `qtddeposito` decimal(10,0) ,
 `qtdprateleiras` decimal(10,2) ,
 `qtdretida` decimal(8,2) ,
 `qtdvencidos` decimal(10,2) ,
 `qtdprevenda` decimal(10,2) ,
 `qtdaentregar` decimal(10,2) ,
 `qtdemtransito` decimal(10,2) ,
 `pedidoand` decimal(10,2) 
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
 `indicadorarredondamentotruncamento` char(1) ,
 `canceladoECF` char(1) 
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
 `ipiItem` decimal(43,2) ,
 `totPIS` decimal(41,2) ,
 `totCOFINS` decimal(41,2) ,
 `totalProduto` decimal(32,2) ,
 `totalNF` decimal(48,2) ,
 `valoroutrasaliquotas` decimal(44,2) ,
 `totalReducaoICMS` decimal(36,2) ,
 `baseCalculoIPI` decimal(32,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `lancada` char(1) ,
 `valorICMS` decimal(30,2) 
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
 `ipiItem` decimal(43,2) ,
 `totPIS` decimal(41,2) ,
 `totCOFINS` decimal(41,2) ,
 `totalProduto` decimal(32,2) ,
 `totalNF` decimal(48,2) ,
 `valoroutrasaliquotas` decimal(44,2) ,
 `baseCalculoIPI` decimal(32,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `lancada` char(1) ,
 `valoricms` decimal(8,2) 
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
 `quantidade` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `origem` char(1) ,
 `totalicms` decimal(51,2) ,
 `totalIPI` decimal(40,2) ,
 `totalPIS` decimal(37,2) ,
 `totalCOFINS` decimal(37,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `descontovalor` decimal(35,4) ,
 `valorisentas` decimal(36,4) ,
 `totalbruto` decimal(32,2) ,
 `SUM(TOTAL)` decimal(35,2) ,
 `total` decimal(35,2) ,
 `totalItem` decimal(32,2) ,
 `baseCalculoICMS` decimal(41,2) ,
 `baseCalculoIPI` decimal(37,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) ,
 `bcICMSST` decimal(38,2) ,
 `totalICMSST` decimal(44,2) 
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
 `quantidade` decimal(32,5) ,
 `unidade` char(3) ,
 `nrcontrole` int(5) ,
 `origem` char(1) ,
 `totalicms` decimal(51,2) ,
 `totalIPI` decimal(40,2) ,
 `totalPIS` decimal(37,2) ,
 `totalCOFINS` decimal(37,2) ,
 `totalPIS_QTD` decimal(35,5) ,
 `totalCOFINS_QTD` decimal(35,5) ,
 `descontovalor` decimal(35,4) ,
 `valorisentas` decimal(36,4) ,
 `SUM(TOTAL)` decimal(35,2) ,
 `TOTAL` decimal(35,2) ,
 `totalbruto` decimal(33,2) ,
 `baseCalculoICMS` decimal(41,2) ,
 `baseCalculoIPI` decimal(37,2) ,
 `baseCalculoPIS` decimal(35,2) ,
 `baseCalculoCOFINS` decimal(35,2) ,
 `baseCalculoPIS_QTD` decimal(30,2) ,
 `baseCalculoCOFINS_QTD` decimal(30,2) ,
 `bcICMSST` decimal(38,2) ,
 `totalICMSST` decimal(44,2) 
)*/;

/*Table structure for table `resumovendas` */

DROP TABLE IF EXISTS `resumovendas`;

/*!50001 DROP VIEW IF EXISTS `resumovendas` */;
/*!50001 DROP TABLE IF EXISTS `resumovendas` */;

/*!50001 CREATE TABLE  `resumovendas`(
 `TotalLiquido` decimal(36,2) ,
 `ticket` decimal(18,6) ,
 `DATA` date 
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

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `60d` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`ratdesc`) AS `descontovalorCupom`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` where (`vendatmp`.`quantidade` > 0) group by `vendatmp`.`data`,`vendatmp`.`codigo` */;

/*View structure for view 60i */

/*!50001 DROP TABLE IF EXISTS `60i` */;
/*!50001 DROP VIEW IF EXISTS `60i` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `60i` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`cancelado` AS `cancelado`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`ratdesc`) AS `descontovalorCupom`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` group by `vendatmp`.`codigo` */;

/*View structure for view 60r */

/*!50001 DROP TABLE IF EXISTS `60r` */;
/*!50001 DROP VIEW IF EXISTS `60r` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `60r` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` where (`vendatmp`.`quantidade` > 0) group by month(`vendatmp`.`data`),`vendatmp`.`icms`,`vendatmp`.`codigo` */;

/*View structure for view analise_venda_caixa */

/*!50001 DROP TABLE IF EXISTS `analise_venda_caixa` */;
/*!50001 DROP VIEW IF EXISTS `analise_venda_caixa` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `analise_venda_caixa` AS (select `contdocs`.`data` AS `data`,`contdocs`.`documento` AS `documento`,`contdocs`.`total` AS `totalDocumento`,(select sum(`caixaarquivo`.`valor`) from `caixaarquivo` where (`caixaarquivo`.`documento` = `contdocs`.`documento`)) AS `totalCaixa`,truncate((select sum(((`vendaarquivo`.`total` - `vendaarquivo`.`ratdesc`) + `vendaarquivo`.`rateioencargos`)) from `vendaarquivo` where ((`vendaarquivo`.`documento` = `contdocs`.`documento`) and (`vendaarquivo`.`cancelado` = 'N') and (`vendaarquivo`.`total` > 0))),2) AS `totalVenda` from `contdocs` where (`contdocs`.`dpfinanceiro` = 'Venda')) */;

/*View structure for view analisevalores */

/*!50001 DROP TABLE IF EXISTS `analisevalores` */;
/*!50001 DROP VIEW IF EXISTS `analisevalores` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `analisevalores` AS (select `contdocs`.`documento` AS `documento`,`contdocs`.`total` AS `totalDocumento`,(select sum(`caixaarquivo`.`valor`) from `caixaarquivo` where (`caixaarquivo`.`documento` = `contdocs`.`documento`)) AS `totalCaixa`,truncate((select sum((`vendaarquivo`.`total` - `vendaarquivo`.`ratdesc`)) from `vendaarquivo` where ((`vendaarquivo`.`documento` = `contdocs`.`documento`) and (`vendaarquivo`.`cancelado` = 'N'))),2) AS `totalVenda` from `contdocs` where (`contdocs`.`dpfinanceiro` = 'Venda')) */;

/*View structure for view apuracaofiscal */

/*!50001 DROP TABLE IF EXISTS `apuracaofiscal` */;
/*!50001 DROP VIEW IF EXISTS `apuracaofiscal` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `apuracaofiscal` AS select `blococregc190`.`codigofilial` AS `filial`,`blococregc190`.`cfopentrada` AS `cfop`,`blococregc190`.`modelonf` AS `modeloDOC`,`blococregc190`.`tributacao` AS `CSTICMS`,sum(`blococregc190`.`totalProduto`) AS `totalproduto`,sum(`blococregc190`.`totalNF`) AS `total`,sum(`blococregc190`.`bcicms`) AS `bcICMS`,sum(`blococregc190`.`toticms`) AS `totICMS`,sum(`blococregc190`.`baseCalculoIPI`) AS `baseCalculoIPI`,sum(`blococregc190`.`ipiItem`) AS `totalIPI`,sum(`blococregc190`.`baseCalculoPIS`) AS `bcPIS`,sum(`blococregc190`.`baseCalculoCOFINS`) AS `bcCOFINS`,sum(`blococregc190`.`totalPIS`) AS `totalPIS`,sum(`blococregc190`.`totalCOFINS`) AS `totalCOFINS`,sum(`blococregc190`.`bcicmsST`) AS `bcICMSST`,sum(`blococregc190`.`valoricmsST`) AS `totalICMSST`,sum(`blococregc190`.`valoroutrasdespesas`) AS `totalOutrasDespesas`,sum(`blococregc190`.`valorisentas`) AS `totalIsentas` from `blococregc190` where ((`blococregc190`.`lancada` = 'X') and (`blococregc190`.`dataentrada` >= '2012-01-01') and (`blococregc190`.`dataentrada` <= '2012-03-31')) group by `blococregc190`.`cfopentrada`,`blococregc190`.`codigofilial` union all select `blococregc190_saida`.`codigofilial` AS `filial`,`blococregc190_saida`.`cfop` AS `cfop`,`blococregc190_saida`.`modelodocfiscal` AS `modeloDOC`,`blococregc190_saida`.`tributacao` AS `cstICMS`,sum(`blococregc190_saida`.`totalItem`) AS `totalProduto`,sum(`blococregc190_saida`.`totalItem`) AS `total`,sum(`blococregc190_saida`.`baseCalculoICMS`) AS `bcICMS`,sum(`blococregc190_saida`.`totalicms`) AS `totICMS`,sum(`blococregc190_saida`.`baseCalculoIPI`) AS `baseCalculoIPI`,sum(`blococregc190_saida`.`totalIPI`) AS `totalIPI`,sum(`blococregc190_saida`.`baseCalculoPIS`) AS `bcPIS`,sum(`blococregc190_saida`.`baseCalculoCOFINS`) AS `bcCOFINS`,sum(`blococregc190_saida`.`totalPIS`) AS `totalPIS`,sum(`blococregc190_saida`.`totalCOFINS`) AS `totalCOFINS`,sum(`blococregc190_saida`.`bcICMSST`) AS `bcICMSST`,sum(`blococregc190_saida`.`totalICMSST`) AS `totalICMSST`,sum(0) AS `totalOutrasDespesas`,sum(0) AS `totalIsentas` from `blococregc190_saida` where ((`blococregc190_saida`.`DATA` >= '2012-01-01') and (`blococregc190_saida`.`DATA` <= '2012-03-31')) group by `blococregc190_saida`.`cfop`,`blococregc190_saida`.`codigofilial` union all select `blococregc381_pis`.`codigofilial` AS `filial`,`blococregc381_pis`.`cfop` AS `cfop`,`blococregc381_pis`.`modelodocfiscal` AS `modeloDOC`,`blococregc381_pis`.`tributacao` AS `cstICMS`,sum(`blococregc381_pis`.`total`) AS `totalProduto`,sum(`blococregc381_pis`.`total`) AS `total`,sum(`blococregc381_pis`.`baseCalculoICMS`) AS `bcICMS`,sum(`blococregc381_pis`.`totalicms`) AS `totICMS`,sum(0) AS `baceCalculoIPI`,sum(0) AS `totalIPI`,sum(`blococregc381_pis`.`baseCalculoPIS`) AS `bcPIS`,sum(`blococregc381_pis`.`baseCalculoCOFINS`) AS `bcCOFINS`,sum(`blococregc381_pis`.`totalPIS`) AS `totalPIS`,sum(`blococregc381_pis`.`totalCOFINS`) AS `totalCOFINS`,sum(0) AS `bcICMSST`,sum(0) AS `totalICMSST`,sum(0) AS `totalOutrasDespesas`,sum(0) AS `totalIsentas` from `blococregc381_pis` where ((`blococregc381_pis`.`data` >= '2012-01-01') and (`blococregc381_pis`.`data` <= '2012-03-31')) group by `blococregc381_pis`.`cfop`,`blococregc381_pis`.`codigofilial` union all select `blococregc491_pis`.`codigofilial` AS `filial`,`blococregc491_pis`.`cfop` AS `cfop`,`blococregc491_pis`.`modelodocfiscal` AS `modeloDOC`,`blococregc491_pis`.`tributacao` AS `cstICMS`,sum(`blococregc491_pis`.`total`) AS `totalProduto`,sum(`blococregc491_pis`.`total`) AS `total`,sum(`blococregc491_pis`.`baseCalculoICMS`) AS `bcICMS`,sum(`blococregc491_pis`.`totalicms`) AS `totICMS`,sum(0) AS `baceCalculoIPI`,sum(0) AS `totalIPI`,sum(`blococregc491_pis`.`baseCalculoPIS`) AS `bcPIS`,sum(`blococregc491_pis`.`baseCalculoCOFINS`) AS `bcCOFINS`,sum(`blococregc491_pis`.`totalPIS`) AS `totalPIS`,sum(`blococregc491_pis`.`totalCOFINS`) AS `totalCOFINS`,sum(0) AS `bcICMSST`,sum(0) AS `totalICMSST`,sum(0) AS `totalOutrasDespesas`,sum(0) AS `totalIsentas` from `blococregc491_pis` where ((`blococregc491_pis`.`data` >= '2012-01-01') and (`blococregc491_pis`.`data` <= '2012-03-31')) group by `blococregc491_pis`.`cfop`,`blococregc491_pis`.`codigofilial` */;

/*View structure for view blococregc190 */

/*!50001 DROP TABLE IF EXISTS `blococregc190` */;
/*!50001 DROP VIEW IF EXISTS `blococregc190` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc190` AS select `entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`icmsst` AS `icmsst`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`ratdesconto`) AS `desconto`,sum(`entradas`.`ratdespesas`) AS `totalDespesas`,sum(`entradas`.`ratseguro`) AS `totalSeguro`,sum(`entradas`.`ratfrete`) AS `totalFrete`,sum(`entradas`.`valoroutrasdespesas`) AS `valoroutrasdespesas`,sum(`entradas`.`bcicms`) AS `bcicms`,sum(if((`entradas`.`IcmsEntrada` = 0),`entradas`.`totalitem`,0)) AS `valorisentas`,round(if((`entradas`.`IcmsEntrada` > 0),sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),0),2) AS `toticms`,sum(`entradas`.`bcicmsST`) AS `bcicmsST`,sum(`entradas`.`valoricmsST`) AS `valoricmsST`,sum(round((`entradas`.`totalitem` * if((`entradas`.`IPI` > 0),(`entradas`.`IPI` / 100),0)),2)) AS `ipiItem`,round(if((`entradas`.`pis` > 0),sum(((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)) * (`entradas`.`pis` / 100))),0),2) AS `totalPIS`,round(if((`entradas`.`cofins` > 0),sum(((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)) * (`entradas`.`cofins` / 100))),0),2) AS `totalCOFINS`,sum(`entradas`.`totalitem`) AS `totalProduto`,((((((sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) + sum(`entradas`.`totalitem`)) - round(sum(`entradas`.`ratdesconto`),2)) AS `totalNF`,((((sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) AS `valoroutrasaliquotas`,sum(round((`entradas`.`totalitem` * (`entradas`.`percentualRedBaseCalcICMS` / 100)),2)) AS `totalReducaoICMS`,sum(if((`entradas`.`IPI` > 0),`entradas`.`totalitem`,0)) AS `baseCalculoIPI`,if((`entradas`.`pis` > 0),sum(round((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)),2)),0) AS `baseCalculoPIS`,if((`entradas`.`cofins` > 0),sum(round((`entradas`.`totalitem` + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100)),2)),0) AS `baseCalculoCOFINS`,`entradas`.`Lancada` AS `lancada` from `entradas` where (`entradas`.`exportarfiscal` = 'S') group by `entradas`.`NF`,`entradas`.`cfopentrada`,`entradas`.`IcmsEntrada`,`entradas`.`tributacao`,`entradas`.`numero` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view blococregc190_saida */

/*!50001 DROP TABLE IF EXISTS `blococregc190_saida` */;
/*!50001 DROP VIEW IF EXISTS `blococregc190_saida` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc190_saida` AS select `vendanftmp`.`inc` AS `inc`,`vendanftmp`.`codigofilial` AS `codigofilial`,`vendanftmp`.`NotaFiscal` AS `notafiscal`,`vendanftmp`.`serieNF` AS `serienf`,`vendanftmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendanftmp`.`documento` AS `documento`,`vendanftmp`.`data` AS `DATA`,`vendanftmp`.`cfop` AS `cfop`,`vendanftmp`.`icms` AS `icms`,ifnull(`vendanftmp`.`aliquotaIPI`,0) AS `ipi`,`vendanftmp`.`pis` AS `pis`,`vendanftmp`.`cofins` AS `cofins`,`vendanftmp`.`cstpis` AS `cstpis`,`vendanftmp`.`cstcofins` AS `cstcofins`,`vendanftmp`.`cstipi` AS `cstipi`,`vendanftmp`.`tributacao` AS `tributacao`,`vendanftmp`.`codigo` AS `codigo`,`vendanftmp`.`produto` AS `produto`,sum(`vendanftmp`.`quantidade`) AS `SUM(quantidade)`,`vendanftmp`.`unidade` AS `unidade`,`vendanftmp`.`nrcontrole` AS `nrcontrole`,`vendanftmp`.`origem` AS `origem`,sum(round((((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - ((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`icms`) / 100),2)) AS `totalicms`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((((`vendanftmp`.`total` - ((`vendanftmp`.`total` * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`aliquotaIPI`) / 100),2)),0) AS `totalIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`pis`) / 100),2)),0) AS `totalPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`cofins`) / 100),2)),0) AS `totalCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`cofins`)),0) AS `totalCOFINS_QTD`,sum((`vendanftmp`.`descontovalor` + `vendanftmp`.`ratdesc`)) AS `descontovalor`,sum(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`)) AS `SUM(TOTAL)`,sum(`vendanftmp`.`total`) AS `totalItem`,if((`vendanftmp`.`icms` > 0),sum(round((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - (((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS`,sum(round((`vendanftmp`.`total` * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100)),2)) AS `totalReducaoICMS`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((`vendanftmp`.`total` - (`vendanftmp`.`total` * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD`,if((`vendanftmp`.`icmsst` > 0),sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)),0) AS `bcICMSST`,truncate(if(((`vendanftmp`.`icmsst` > 0) and (`vendanftmp`.`icmsst` >= `vendanftmp`.`icms`)),(((sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)) * `vendanftmp`.`icmsst`) / 100) - sum(round(((`vendanftmp`.`total` * `vendanftmp`.`icms`) / 100),2))),0),2) AS `totalICMSST` from `vendanftmp` where (`vendanftmp`.`quantidade` > 0) group by `vendanftmp`.`NotaFiscal`,`vendanftmp`.`serieNF`,`vendanftmp`.`icms`,`vendanftmp`.`cfop`,`vendanftmp`.`codigofilial`,`vendanftmp`.`tributacao` */;

/*View structure for view blococregc300 */

/*!50001 DROP TABLE IF EXISTS `blococregc300` */;
/*!50001 DROP VIEW IF EXISTS `blococregc300` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc300` AS select `contdocs`.`ip` AS `ip`,`contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`Totalbruto` AS `Totalbruto`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,`contdocs`.`desconto` AS `desconto`,`contdocs`.`total` AS `total`,`contdocs`.`NrParcelas` AS `NrParcelas`,`contdocs`.`vendedor` AS `vendedor`,`contdocs`.`operador` AS `operador`,`contdocs`.`Observacao` AS `Observacao`,`contdocs`.`classe` AS `classe`,`contdocs`.`dataexe` AS `dataexe`,`contdocs`.`codigocliente` AS `codigocliente`,`contdocs`.`nome` AS `nome`,`contdocs`.`CodigoFilial` AS `CodigoFilial`,`contdocs`.`historico` AS `historico`,`contdocs`.`vrjuros` AS `vrjuros`,`contdocs`.`tipopagamento` AS `tipopagamento`,`contdocs`.`encargos` AS `encargos`,`contdocs`.`id` AS `id`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`enderecoentrega` AS `enderecoentrega`,`contdocs`.`custos` AS `custos`,`contdocs`.`devolucaovenda` AS `devolucaovenda`,`contdocs`.`devolucaorecebimento` AS `devolucaorecebimento`,`contdocs`.`nrboletobancario` AS `nrboletobancario`,`contdocs`.`nrnotafiscal` AS `nrnotafiscal`,`contdocs`.`classedevolucao` AS `classedevolucao`,`contdocs`.`responsavelreceber` AS `responsavelreceber`,`contdocs`.`numeroentrega` AS `numeroentrega`,`contdocs`.`cidadeentrega` AS `cidadeentrega`,`contdocs`.`cepentrega` AS `cepentrega`,`contdocs`.`bairroentrega` AS `bairroentrega`,`contdocs`.`horaentrega` AS `horaentrega`,`contdocs`.`dataentrega` AS `dataentrega`,`contdocs`.`obsentrega` AS `obsentrega`,`contdocs`.`concluido` AS `concluido`,`contdocs`.`cartaofidelidade` AS `cartaofidelidade`,`contdocs`.`bordero` AS `bordero`,`contdocs`.`valorservicos` AS `valorservicos`,`contdocs`.`descontoservicos` AS `descontoservicos`,`contdocs`.`romaneio` AS `romaneio`,`contdocs`.`hora` AS `hora`,`contdocs`.`entregaconcluida` AS `entregaconcluida`,`contdocs`.`dataentregaconcluida` AS `dataentregaconcluida`,`contdocs`.`operadorentrega` AS `operadorentrega`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`nreducaoz` AS `nreducaoz`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`TEF` AS `TEF`,`contdocs`.`ecfValorCancelamentos` AS `ecfValorCancelamentos`,`contdocs`.`NF_e` AS `NF_e`,`contdocs`.`estadoentrega` AS `estadoentrega`,`contdocs`.`ecfConsumidor` AS `ecfConsumidor`,`contdocs`.`ecfCPFCNPJconsumidor` AS `ecfCPFCNPJconsumidor`,`contdocs`.`ecfEndConsumidor` AS `ecfEndConsumidor`,`contdocs`.`prevendanumero` AS `prevendanumero`,`contdocs`.`ecfcontadorcupomfiscal` AS `ecfcontadorcupomfiscal`,`contdocs`.`ecftotalliquido` AS `ecftotalliquido`,`contdocs`.`contadornaofiscalGNF` AS `contadornaofiscalGNF`,`contdocs`.`contadordebitocreditoCDC` AS `contadordebitocreditoCDC`,`contdocs`.`totalICMScupomfiscal` AS `totalICMScupomfiscal`,`contdocs`.`troco` AS `troco`,`contdocs`.`davnumero` AS `davnumero`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfMFadicional` AS `ecfMFadicional`,`contdocs`.`ecftipo` AS `ecftipo`,`contdocs`.`ecfmarca` AS `ecfmarca`,`contdocs`.`ecfmodelo` AS `ecfmodelo`,`contdocs`.`estoqueatualizado` AS `estoqueatualizado`,`contdocs`.`serienf` AS `serienf`,`contdocs`.`EADRegistroDAV` AS `EADRegistroDAV`,`contdocs`.`EADr06` AS `EADr06`,`contdocs`.`tipopagamentoECF` AS `tipopagamentoECF`,`contdocs`.`modeloDOCFiscal` AS `modeloDOCFiscal`,`contdocs`.`subserienf` AS `subserienf`,sum(`contdocs`.`total`) AS `totalDocumento` from `contdocs` where ((`contdocs`.`modeloDOCFiscal` = '02') or (`contdocs`.`modeloDOCFiscal` = 'D1')) group by `contdocs`.`data`,`contdocs`.`serienf`,`contdocs`.`subserienf` */;

/*View structure for view blococregc320 */

/*!50001 DROP TABLE IF EXISTS `blococregc320` */;
/*!50001 DROP VIEW IF EXISTS `blococregc320` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc320` AS select `vendatmp`.`data` AS `data`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`serieNF` AS `serieNF`,`vendatmp`.`subserienf` AS `subserienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`ecfnumero` AS `ecfnumero`,`vendatmp`.`NotaFiscal` AS `NotaFiscal`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`total` AS `total`,sum(if((`vendatmp`.`icms` > 0),`vendatmp`.`total`,0)) AS `bcICMS`,((`vendatmp`.`total` * `vendatmp`.`icms`) / 100) AS `totalICMS` from `vendatmp` group by `vendatmp`.`tributacao`,`vendatmp`.`cfop`,`vendatmp`.`icms`,`vendatmp`.`data` union all select `venda`.`data` AS `data`,`venda`.`documento` AS `documento`,`venda`.`serieNF` AS `serieNF`,`venda`.`subserienf` AS `subserienf`,`venda`.`modelodocfiscal` AS `modelodocfiscal`,`venda`.`Ecfnumero` AS `ecfnumero`,`venda`.`NotaFiscal` AS `NotaFiscal`,`venda`.`tributacao` AS `tributacao`,`venda`.`cfop` AS `cfop`,`venda`.`icms` AS `icms`,sum(`venda`.`total`) AS `total`,sum(if((`venda`.`icms` > 0),`venda`.`total`,0)) AS `bcICMS`,((`venda`.`total` * `venda`.`icms`) / 100) AS `totalICMS` from `venda` group by `venda`.`tributacao`,`venda`.`cfop`,`venda`.`icms`,`venda`.`data` */;

/*View structure for view blococregc321 */

/*!50001 DROP TABLE IF EXISTS `blococregc321` */;
/*!50001 DROP VIEW IF EXISTS `blococregc321` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc321` AS select `venda`.`inc` AS `inc`,`venda`.`codigofilial` AS `codigofilial`,`venda`.`operador` AS `operador`,`venda`.`data` AS `data`,`venda`.`codigo` AS `codigo`,`venda`.`produto` AS `produto`,`venda`.`quantidade` AS `quantidade`,`venda`.`preco` AS `preco`,`venda`.`custo` AS `custo`,`venda`.`precooriginal` AS `precooriginal`,`venda`.`Descontoperc` AS `Descontoperc`,`venda`.`id` AS `id`,`venda`.`descontovalor` AS `descontovalor`,`venda`.`total` AS `total`,`venda`.`vendedor` AS `vendedor`,`venda`.`nrcontrole` AS `nrcontrole`,`venda`.`documento` AS `documento`,`venda`.`grupo` AS `grupo`,`venda`.`subgrupo` AS `subgrupo`,`venda`.`comissao` AS `comissao`,`venda`.`ratdesc` AS `ratdesc`,`venda`.`rateioencargos` AS `rateioencargos`,`venda`.`situacao` AS `situacao`,`venda`.`customedio` AS `customedio`,`venda`.`Ecfnumero` AS `Ecfnumero`,`venda`.`fornecedor` AS `fornecedor`,`venda`.`fabricante` AS `fabricante`,`venda`.`NotaFiscal` AS `NotaFiscal`,`venda`.`icms` AS `icms`,`venda`.`classe` AS `classe`,`venda`.`secao` AS `secao`,`venda`.`lote` AS `lote`,`venda`.`tributacao` AS `tributacao`,`venda`.`aentregar` AS `aentregar`,`venda`.`quantidadeanterior` AS `quantidadeanterior`,`venda`.`quantidadeatualizada` AS `quantidadeatualizada`,`venda`.`codigofiscal` AS `codigofiscal`,`venda`.`customedioanterior` AS `customedioanterior`,`venda`.`codigocliente` AS `codigocliente`,`venda`.`numerodevolucao` AS `numerodevolucao`,`venda`.`codigobarras` AS `codigobarras`,`venda`.`aliquotaIPI` AS `ipi`,`venda`.`unidade` AS `unidade`,`venda`.`embalagem` AS `embalagem`,`venda`.`grade` AS `grade`,`venda`.`romaneio` AS `romaneio`,`venda`.`tipo` AS `tipo`,`venda`.`cofins` AS `cofins`,`venda`.`pis` AS `pis`,`venda`.`despesasacessorias` AS `despesasacessorias`,`venda`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`venda`.`serieNF` AS `serieNF`,`venda`.`subserienf` AS `subserienf`,`venda`.`cfop` AS `cfop`,`venda`.`acrescimototalitem` AS `acrescimototalitem`,`venda`.`cstpis` AS `cstpis`,`venda`.`cstcofins` AS `cstcofins`,`venda`.`icmsst` AS `icmsst`,`venda`.`percentualRedBaseCalcICMSST` AS `percentualRedBaseCalcICMSST`,`venda`.`mvast` AS `mvast`,`venda`.`modelodocfiscal` AS `modelodocfiscal`,sum(`venda`.`quantidade`) AS `somaQuantidade`,sum((`venda`.`total` - `venda`.`ratdesc`)) AS `totalItem`,sum(`venda`.`ratdesc`) AS `totalDesconto`,if((`venda`.`icms` > 0),truncate(sum(((`venda`.`total` + `venda`.`rateioencargos`) - `venda`.`ratdesc`)),2),0) AS `bcICMS`,if((`venda`.`icms` > 0),truncate(sum(((`venda`.`total` + `venda`.`rateioencargos`) - `venda`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(((`venda`.`total` + `venda`.`rateioencargos`) - `venda`.`ratdesc`)) * `venda`.`icms`) / 100),2) AS `totalICMS`,sum(`venda`.`pis`) AS `totalPIS`,sum(`venda`.`cofins`) AS `totalCOFINS` from `venda` where ((`venda`.`modelodocfiscal` = '02') or (`venda`.`modelodocfiscal` = 'D1')) group by `venda`.`codigo` union all select `vendatmp`.`inc` AS `inc`,`vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`operador` AS `operador`,`vendatmp`.`data` AS `data`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,`vendatmp`.`quantidade` AS `quantidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`custo` AS `custo`,`vendatmp`.`precooriginal` AS `precooriginal`,`vendatmp`.`Descontoperc` AS `Descontoperc`,`vendatmp`.`id` AS `id`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`total` AS `total`,`vendatmp`.`vendedor` AS `vendedor`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`grupo` AS `grupo`,`vendatmp`.`subgrupo` AS `subgrupo`,`vendatmp`.`comissao` AS `comissao`,`vendatmp`.`ratdesc` AS `ratdesc`,`vendatmp`.`rateioencargos` AS `rateioencargos`,`vendatmp`.`situacao` AS `situacao`,`vendatmp`.`customedio` AS `customedio`,`vendatmp`.`ecfnumero` AS `Ecfnumero`,`vendatmp`.`fornecedor` AS `fornecedor`,`vendatmp`.`fabricante` AS `fabricante`,`vendatmp`.`NotaFiscal` AS `NotaFiscal`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`classe` AS `classe`,`vendatmp`.`secao` AS `secao`,`vendatmp`.`lote` AS `lote`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`aentregar` AS `aentregar`,`vendatmp`.`quantidadeanterior` AS `quantidadeanterior`,`vendatmp`.`quantidadeatualizada` AS `quantidadeatualizada`,`vendatmp`.`codigofiscal` AS `codigofiscal`,`vendatmp`.`customedioanterior` AS `customedioanterior`,`vendatmp`.`codigocliente` AS `codigocliente`,`vendatmp`.`numerodevolucao` AS `numerodevolucao`,`vendatmp`.`codigobarras` AS `codigobarras`,`vendatmp`.`aliquotaIPI` AS `ipi`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`embalagem` AS `embalagem`,`vendatmp`.`grade` AS `grade`,`vendatmp`.`romaneio` AS `romaneio`,`vendatmp`.`tipo` AS `tipo`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`despesasacessorias` AS `despesasacessorias`,`vendatmp`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`vendatmp`.`serieNF` AS `serieNF`,`vendatmp`.`subserienf` AS `subserienf`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`acrescimototalitem` AS `acrescimototalitem`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`icmsst` AS `icmsst`,`vendatmp`.`percentualRedBaseCalcICMSST` AS `percentualRedBaseCalcICMSST`,`vendatmp`.`mvast` AS `mvast`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,sum(`vendatmp`.`quantidade`) AS `somaQuantidade`,sum(`vendatmp`.`total`) AS `totalItem`,sum(`vendatmp`.`ratdesc`) AS `totalDesconto`,if((`vendatmp`.`icms` > 0),truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2),0) AS `bcICMS`,if((`vendatmp`.`icms` > 0),truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,sum(`vendatmp`.`pis`) AS `totalPIS`,sum(`vendatmp`.`cofins`) AS `totalCOFINS` from `vendatmp` where ((`vendatmp`.`modelodocfiscal` = '02') or (`vendatmp`.`modelodocfiscal` = 'D1')) group by `vendatmp`.`codigo` */;

/*View structure for view blococregc381_pis */

/*!50001 DROP TABLE IF EXISTS `blococregc381_pis` */;
/*!50001 DROP VIEW IF EXISTS `blococregc381_pis` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc381_pis` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`contdocs`.`devolucaonumero` AS `devolucaonumero`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`contdocs`.`NF_e` = 'N') and (`contdocs`.`serienf` = 'D') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '02')) group by `vendatmp`.`codigo` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc385_cofins */

/*!50001 DROP TABLE IF EXISTS `blococregc385_cofins` */;
/*!50001 DROP VIEW IF EXISTS `blococregc385_cofins` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc385_cofins` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`contdocs`.`devolucaonumero` AS `devolucaonumero`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`contdocs`.`NF_e` = 'N') and (`contdocs`.`serienf` = 'D') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '02')) group by `vendatmp`.`codigo` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc390 */

/*!50001 DROP TABLE IF EXISTS `blococregc390` */;
/*!50001 DROP VIEW IF EXISTS `blococregc390` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc390` AS select `venda`.`documento` AS `documento`,`venda`.`data` AS `data`,`venda`.`icms` AS `icms`,`venda`.`cfop` AS `cfop`,`venda`.`tributacao` AS `tributacao`,truncate(sum((`venda`.`total` - `venda`.`ratdesc`)),2) AS `total`,if((`venda`.`icms` > 0),truncate(sum((`venda`.`total` - `venda`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(`venda`.`total`) * `venda`.`icms`) / 100),2) AS `totalICMS`,`venda`.`NotaFiscal` AS `notafiscal` from `venda` where (`venda`.`quantidade` > 0) group by `venda`.`tributacao`,`venda`.`cfop`,`venda`.`icms`,`venda`.`documento` union all select `vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `data`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`tributacao` AS `tributacao`,truncate(sum(`vendatmp`.`total`),2) AS `total`,if((`vendatmp`.`icms` > 0),truncate(sum(`vendatmp`.`total`),2),0) AS `baseCalculoICMS`,truncate(((sum(`vendatmp`.`total`) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,`vendatmp`.`NotaFiscal` AS `notafiscal` from `vendatmp` where (`vendatmp`.`quantidade` > 0) group by `vendatmp`.`tributacao`,`vendatmp`.`cfop`,`vendatmp`.`icms`,`vendatmp`.`documento` */;

/*View structure for view blococregc400 */

/*!50001 DROP TABLE IF EXISTS `blococregc400` */;
/*!50001 DROP VIEW IF EXISTS `blococregc400` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`127.0.0.1` SQL SECURITY DEFINER VIEW `blococregc400` AS select `r02`.`id` AS `id`,`r02`.`codigofilial` AS `codigofilial`,`r02`.`data` AS `data`,`r02`.`tipo` AS `tipo`,`r02`.`fabricacaoECF` AS `fabricacaoECF`,`r02`.`MFadicional` AS `MFadicional`,`r02`.`modeloECF` AS `modeloECF`,`r02`.`numeroUsuarioSubstituicaoECF` AS `numeroUsuarioSubstituicaoECF`,`r02`.`crz` AS `crz`,`r02`.`coo` AS `coo`,`r02`.`cro` AS `cro`,`r02`.`datamovimento` AS `datamovimento`,`r02`.`dataemissaoreducaoz` AS `dataemissaoreducaoz`,`r02`.`horaemissaoreducaoz` AS `horaemissaoreducaoz`,`r02`.`vendabrutadiaria` AS `vendabrutadiaria`,`r02`.`parametroISSQNdesconto` AS `parametroISSQNdesconto`,`r02`.`numeroECF` AS `numeroECF`,`r02`.`gtfinal` AS `gtfinal`,`r02`.`EADdados` AS `EADdados` from `r02` group by `r02`.`fabricacaoECF` */;

/*View structure for view blococregc425 */

/*!50001 DROP TABLE IF EXISTS `blococregc425` */;
/*!50001 DROP VIEW IF EXISTS `blococregc425` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc425` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`nrnotafiscal` AS `nrnotafiscal`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`ecfCPFCNPJconsumidor` AS `ecfCPFCNPJconsumidor`,`contdocs`.`devolucaonumero` AS `devolucaonumero`,`contdocs`.`numeroPED` AS `numeroPED`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,truncate(`vendatmp`.`quantidade`,3) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2) AS `total`,truncate(sum(if((`vendatmp`.`icms` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoICMS`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,truncate(sum(if((`vendatmp`.`aliquotaIPI` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoIPI`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`aliquotaIPI`) / 100),2) AS `totalIPI`,truncate(sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`pis`),0)),2) AS `totalPIS`,truncate(sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`cofins`),0)),2) AS `totalCOFINS`,sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`),0)) AS `totalPIS_QTD`,sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`),0)) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,if(((`vendatmp`.`tributacao` = '00') and (`vendatmp`.`icms` = 0)),'40',`vendatmp`.`tributacao`) AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,sum(if(((`vendatmp`.`icms` = 0) and (`vendatmp`.`tributacao` = '40')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorisentas`,sum(if(((`vendatmp`.`tributacao` = '41') or (`vendatmp`.`tributacao` = '80')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorNT`,sum(if(((`vendatmp`.`tributacao` = '60') or (`vendatmp`.`tributacao` = '30')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorST` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and ((`vendatmp`.`modelodocfiscal` = '2D') or (`vendatmp`.`NotaFiscal` is not null))) group by `vendatmp`.`codigo`,`vendatmp`.`data`,`vendatmp`.`ecffabricacao` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc470 */

/*!50001 DROP TABLE IF EXISTS `blococregc470` */;
/*!50001 DROP VIEW IF EXISTS `blococregc470` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc470` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`nrnotafiscal` AS `nrnotafiscal`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`ecfCPFCNPJconsumidor` AS `ecfCPFCNPJconsumidor`,`contdocs`.`devolucaonumero` AS `devolucaonumero`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,truncate(`vendatmp`.`quantidade`,3) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2) AS `total`,truncate(sum(if((`vendatmp`.`icms` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoICMS`,if(((truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) <= 0) and (`vendatmp`.`icms` > 0)),0.01,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2)) AS `totalICMS`,truncate(sum(if((`vendatmp`.`aliquotaIPI` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoIPI`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`aliquotaIPI`) / 100),2) AS `totalIPI`,truncate(sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`pis`),0)),2) AS `totalPIS`,truncate(sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`cofins`),0)),2) AS `totalCOFINS`,sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`),0)) AS `totalPIS_QTD`,sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`),0)) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,if(((`vendatmp`.`tributacao` = '00') and (`vendatmp`.`icms` = 0)),'40',`vendatmp`.`tributacao`) AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,sum(if(((`vendatmp`.`icms` = 0) and (if(((`vendatmp`.`tributacao` = '00') and (`vendatmp`.`tributacao` <> '41') and (`vendatmp`.`icms` = 0)),'40',`vendatmp`.`tributacao`) = '40')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorisentas`,sum(if(((`vendatmp`.`tributacao` = '41') or (`vendatmp`.`tributacao` = '80')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorNT`,sum(if(((`vendatmp`.`tributacao` = '60') or (`vendatmp`.`tributacao` = '30')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorST` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`inc`,`vendatmp`.`documento`,`vendatmp`.`nrcontrole`,`vendatmp`.`ccf`,`vendatmp`.`ecffabricacao` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc470_02_itens */

/*!50001 DROP TABLE IF EXISTS `blococregc470_02_itens` */;
/*!50001 DROP VIEW IF EXISTS `blococregc470_02_itens` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc470_02_itens` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`nrnotafiscal` AS `nrnotafiscal`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`ecfCPFCNPJconsumidor` AS `ecfCPFCNPJconsumidor`,`contdocs`.`devolucaonumero` AS `devolucaonumero`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,truncate(`vendatmp`.`quantidade`,3) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2) AS `total`,truncate(sum(if((`vendatmp`.`icms` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoICMS`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,truncate(sum(if((`vendatmp`.`aliquotaIPI` > 0),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)),2) AS `baseCalculoIPI`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`aliquotaIPI`) / 100),2) AS `totalIPI`,truncate(sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`pis`),0)),2) AS `totalPIS`,truncate(sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`cofins`),0)),2) AS `totalCOFINS`,sum(if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`),0)) AS `totalPIS_QTD`,sum(if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),(round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`),0)) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,sum(if(((`vendatmp`.`icms` = 0) and (`vendatmp`.`tributacao` = '40')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorisentas`,sum(if(((`vendatmp`.`tributacao` = '41') or (`vendatmp`.`tributacao` = '80')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorNT`,sum(if(((`vendatmp`.`tributacao` = '60') or (`vendatmp`.`tributacao` = '30')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorST` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and ((`vendatmp`.`modelodocfiscal` = '2D') or (`vendatmp`.`modelodocfiscal` = 'D1'))) group by `vendatmp`.`inc`,`vendatmp`.`documento`,`vendatmp`.`nrcontrole`,`vendatmp`.`ccf`,`vendatmp`.`ecffabricacao` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc481_pis */

/*!50001 DROP TABLE IF EXISTS `blococregc481_pis` */;
/*!50001 DROP VIEW IF EXISTS `blococregc481_pis` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc481_pis` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`contdocs`.`devolucaonumero` AS `devolucaonumero`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`codigo`,`vendatmp`.`codigofilial`,`vendatmp`.`data` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc485_cofins */

/*!50001 DROP TABLE IF EXISTS `blococregc485_cofins` */;
/*!50001 DROP VIEW IF EXISTS `blococregc485_cofins` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc485_cofins` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`contdocs`.`devolucaonumero` AS `devolucaonumero`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(`vendatmp`.`total`,2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(`vendatmp`.`total`,2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`codigo`,`vendatmp`.`codigofilial`,`vendatmp`.`data` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc490 */

/*!50001 DROP TABLE IF EXISTS `blococregc490` */;
/*!50001 DROP VIEW IF EXISTS `blococregc490` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc490` AS select `contdocs`.`CodigoFilial` AS `codigofilial`,`contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`devolucaonumero` AS `devolucaonumero`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,truncate(`vendatmp`.`quantidade`,3) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`ratdesc`) AS `ratdesc`,sum(`vendatmp`.`ratdesc`) AS `rateioencargos`,`vendatmp`.`Descontoperc` AS `descontoperc`,truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2) AS `total`,if((`vendatmp`.`icms` > 0),truncate(sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`icms`) / 100),2) AS `totalICMS`,`vendatmp`.`icms` AS `icms`,if(((`vendatmp`.`tributacao` = '00') and (`vendatmp`.`icms` = 0)),'40',`vendatmp`.`tributacao`) AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,sum(if(((`vendatmp`.`icms` = 0) and (`vendatmp`.`tributacao` <> '60')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorisentas`,sum(if(((`vendatmp`.`tributacao` = '41') or (`vendatmp`.`tributacao` = '80')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorNT`,sum(if(((`vendatmp`.`tributacao` = '60') or (`vendatmp`.`tributacao` = '30')),((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),0)) AS `valorST` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`vendatmp`.`quantidade` > 0) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D') and (`contdocs`.`modeloDOCFiscal` = '2D')) group by `vendatmp`.`tributacao`,`vendatmp`.`cfop`,`vendatmp`.`icms`,`vendatmp`.`data`,`vendatmp`.`modelodocfiscal`,`vendatmp`.`ecffabricacao` */;

/*View structure for view blococregc491_pis */

/*!50001 DROP TABLE IF EXISTS `blococregc491_pis` */;
/*!50001 DROP VIEW IF EXISTS `blococregc491_pis` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc491_pis` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`contdocs`.`devolucaonumero` AS `devolucaonumero`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`codigo`,`vendatmp`.`codigofilial` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blococregc495_cofins */

/*!50001 DROP TABLE IF EXISTS `blococregc495_cofins` */;
/*!50001 DROP VIEW IF EXISTS `blococregc495_cofins` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blococregc495_cofins` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`contdocs`.`CodigoFilial` AS `codigofilial`,`contdocs`.`devolucaonumero` AS `devolucaonumero`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `descricao`,sum(truncate(`vendatmp`.`quantidade`,3)) AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`Descontoperc` AS `descontoperc`,sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)) AS `total`,sum(round(((((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`pis`) / 100),2),0) AS `totalPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),truncate(((sum(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`)) * `vendatmp`.`cofins`) / 100),2),0) AS `totalCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum((round(`vendatmp`.`quantidade`,2) * `vendatmp`.`cofins`)),0) AS `totalCOFINS_QTD`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`pis` AS `pis`,`vendatmp`.`cofins` AS `cofins`,`vendatmp`.`cstpis` AS `cstpis`,`vendatmp`.`cstcofins` AS `cstcofins`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cfop` AS `cfop`,if((`vendatmp`.`icms` > 0),sum(truncate(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoICMS`,if((`vendatmp`.`aliquotaIPI` > 0),sum(round((`vendatmp`.`total` - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoIPI`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` <> '03')),sum(round(((`vendatmp`.`total` + `vendatmp`.`rateioencargos`) - `vendatmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendatmp`.`pis` > 0) and (`vendatmp`.`cstpis` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendatmp`.`cofins` > 0) and (`vendatmp`.`cstcofins` = '03')),sum(round(`vendatmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD` from (`contdocs` join `vendatmp`) where ((`contdocs`.`documento` = `vendatmp`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendatmp`.`quantidade` > 0) and (`vendatmp`.`cancelado` = 'N') and (`vendatmp`.`modelodocfiscal` = '2D')) group by `vendatmp`.`codigo`,`vendatmp`.`codigofilial` order by `vendatmp`.`nrcontrole` */;

/*View structure for view blocomregm100_500 */

/*!50001 DROP TABLE IF EXISTS `blocomregm100_500` */;
/*!50001 DROP VIEW IF EXISTS `blocomregm100_500` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blocomregm100_500` AS (select round(sum(((((((`entradas`.`totalitem` - `entradas`.`ratdesconto`) + `entradas`.`ratdespesas`) + `entradas`.`ratfrete`) + `entradas`.`ratseguro`) + `entradas`.`valoricmsST`) + ((`entradas`.`totalitem` * `entradas`.`IPI`) / 100))),2) AS `valornota`,sum(`entradas`.`totalitem`) AS `valorprodutos`,`entradas`.`pis` AS `pis`,`entradas`.`cstpis` AS `cstpis`,`entradas`.`cofins` AS `cofins`,`entradas`.`cstcofins` AS `cstcofins`,`entradas`.`cfopentrada` AS `cfopentrada`,round(sum(((`entradas`.`totalitem` * `entradas`.`pis`) / 100)),2) AS `valorpis`,round(sum(((`entradas`.`totalitem` * `entradas`.`cofins`) / 100)),2) AS `valorcofins`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`codigofilial` AS `codigofilial` from (`entradas` join `moventradas` on(((`entradas`.`numero` = `moventradas`.`numero`) and (`moventradas`.`exportarfiscal` = 'S') and (`moventradas`.`modeloNF` = '55') and (`moventradas`.`Emitente` = 'T') and (`moventradas`.`lancada` = 'X') and (`moventradas`.`NF` <> '')))) group by `entradas`.`cstpis`,`entradas`.`pis`,`entradas`.`numero`,`entradas`.`cfopentrada`,`entradas`.`data`,`entradas`.`codigofilial`) */;

/*View structure for view blocomregm210_610 */

/*!50001 DROP TABLE IF EXISTS `blocomregm210_610` */;
/*!50001 DROP VIEW IF EXISTS `blocomregm210_610` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `blocomregm210_610` AS (select sum((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`)) AS `total`,`contnfsaida`.`codigofilial` AS `codigofilial`,`contnfsaida`.`tipo` AS `tipo`,`vendanftmp`.`cstpis` AS `cstpis`,`vendanftmp`.`pis` AS `pis`,`vendanftmp`.`cstcofins` AS `cstcofins`,`vendanftmp`.`cofins` AS `cofins`,round(((sum((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`)) * `vendanftmp`.`pis`) / 100),2) AS `valorpis`,round(((sum((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`)) * `vendanftmp`.`cofins`) / 100),2) AS `valorcofins`,`vendanftmp`.`data` AS `DATA`,`vendanftmp`.`cfop` AS `cfop` from (`vendanftmp` join `contnfsaida` on(((`vendanftmp`.`serieNF` = `contnfsaida`.`serie`) and (`vendanftmp`.`NotaFiscal` = `contnfsaida`.`notafiscal`)))) where ((`vendanftmp`.`cancelado` = 'N') and (`contnfsaida`.`chave_nfe` <> '') and (`contnfsaida`.`exportarfiscal` = 'S') and (`contnfsaida`.`finalidade` = '1') and ((`contnfsaida`.`codfornecedor` > 0) or (`contnfsaida`.`codcliente` > 0))) group by `contnfsaida`.`codigofilial`,`vendanftmp`.`cstpis`,`vendanftmp`.`pis`,`vendanftmp`.`data`,`vendanftmp`.`cfop`) */;

/*View structure for view posicaoestoquefiliais */

/*!50001 DROP TABLE IF EXISTS `posicaoestoquefiliais` */;
/*!50001 DROP VIEW IF EXISTS `posicaoestoquefiliais` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `posicaoestoquefiliais` AS select `produtosfilial`.`CodigoFilial` AS `codigofilial`,`produtosfilial`.`codigo` AS `codigo`,`produtosfilial`.`quantidade` AS `quantidade`,`produtosfilial`.`qtddeposito` AS `qtddeposito`,`produtosfilial`.`qtdprateleiras` AS `qtdprateleiras`,`produtosfilial`.`qtdretida` AS `qtdretida`,`produtosfilial`.`qtdvencidos` AS `qtdvencidos`,`produtosfilial`.`qtdprevenda` AS `qtdprevenda`,`produtosfilial`.`qtdaentregar` AS `qtdaentregar`,`produtosfilial`.`qtdemtransito` AS `qtdemtransito`,`produtosfilial`.`pedidoand` AS `pedidoand` from `produtosfilial` union all select `produtos`.`CodigoFilial` AS `codigofilial`,`produtos`.`codigo` AS `codigo`,`produtos`.`quantidade` AS `quantidade`,`produtos`.`qtddeposito` AS `qtddeposito`,`produtos`.`qtdprateleiras` AS `qtdprateleiras`,`produtos`.`qtdretida` AS `qtdretida`,`produtos`.`qtdvencidos` AS `qtdvencidos`,`produtos`.`qtdprevenda` AS `qtdprevenda`,`produtos`.`qtdaentregar` AS `qtdaentregar`,`produtos`.`qtdemtransito` AS `qtdemtransito`,`produtos`.`pedidoand` AS `pedidoand` from `produtos` order by `codigofilial` */;

/*View structure for view r05 */

/*!50001 DROP TABLE IF EXISTS `r05` */;
/*!50001 DROP VIEW IF EXISTS `r05` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `r05` AS select ifnull(`venda`.`coo`,'') AS `ncupomfiscal`,ifnull(`venda`.`ccf`,'') AS `ecfcontadorcupomfiscal`,`contdocs`.`data` AS `data`,`contdocs`.`documento` AS `documento`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,ifnull(`venda`.`ecffabricacao`,'') AS `ecffabricacao`,`contdocs`.`ecfMFadicional` AS `ecfMFadicional`,`contdocs`.`ecfmodelo` AS `ecfmodelo`,`venda`.`nrcontrole` AS `nrcontrole`,`venda`.`codigo` AS `codigo`,`venda`.`produto` AS `produto`,`venda`.`quantidade` AS `quantidade`,`venda`.`unidade` AS `unidade`,`venda`.`precooriginal` AS `precooriginal`,`venda`.`descontovalor` AS `descontovalor`,`venda`.`preco` AS `preco`,`venda`.`total` AS `total`,`venda`.`icms` AS `icms`,`venda`.`tributacao` AS `tributacao`,`venda`.`cancelado` AS `cancelado`,`venda`.`ccf` AS `ccf`,`venda`.`coo` AS `coo`,`venda`.`acrescimototalitem` AS `acrescimototalitem`,`venda`.`Descontoperc` AS `Descontoperc`,ifnull(`venda`.`eaddados`,'') AS `eaddados`,`produtos`.`indicadorproducao` AS `indicadorproducao`,`produtos`.`indicadorarredondamentotruncamento` AS `indicadorarredondamentotruncamento`,`venda`.`canceladoECF` AS `canceladoECF` from ((`contdocs` join `venda`) join `produtos`) where ((`venda`.`documento` = `contdocs`.`documento`) and (`produtos`.`codigo` = `venda`.`codigo`) and (`venda`.`quantidade` > 0)) union all select ifnull(`vendatmp`.`coo`,'') AS `ncupomfiscal`,ifnull(`vendatmp`.`ccf`,'') AS `ecfcontadorcupomfiscal`,`contdocs`.`data` AS `data`,`contdocs`.`documento` AS `documento`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,ifnull(`vendatmp`.`ecffabricacao`,'') AS `ecffabricacao`,`contdocs`.`ecfMFadicional` AS `ecfMFadicional`,`contdocs`.`ecfmodelo` AS `ecfmodelo`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,`vendatmp`.`quantidade` AS `quantidade`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`precooriginal` AS `precooriginal`,`vendatmp`.`descontovalor` AS `descontovalor`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`total` AS `total`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`cancelado` AS `cancelado`,ifnull(`vendatmp`.`ccf`,'') AS `ccf`,ifnull(`vendatmp`.`coo`,'') AS `coo`,`vendatmp`.`acrescimototalitem` AS `acrescimototalitem`,`vendatmp`.`Descontoperc` AS `Descontoperc`,ifnull(`vendatmp`.`eaddados`,'') AS `eaddados`,`produtos`.`indicadorproducao` AS `indicadorproducao`,`produtos`.`indicadorarredondamentotruncamento` AS `indicadorarredondamentotruncamento`,`vendatmp`.`canceladoECF` AS `canceladoECF` from ((`contdocs` join `vendatmp`) join `produtos`) where ((`vendatmp`.`documento` = `contdocs`.`documento`) and (`produtos`.`codigo` = `vendatmp`.`codigo`) and (`contdocs`.`documento` <> '') and (`vendatmp`.`quantidade` > 0) and (`produtos`.`CodigoFilial` = `contdocs`.`CodigoFilial`) and (`vendatmp`.`ecfnumero` <> '')) */;

/*View structure for view registro50entradas_agr */

/*!50001 DROP TABLE IF EXISTS `registro50entradas_agr` */;
/*!50001 DROP VIEW IF EXISTS `registro50entradas_agr` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `registro50entradas_agr` AS select (select `moventradas`.`UFemitente` AS `UFemitente` from `moventradas` where (`moventradas`.`numero` = `entradas`.`numero`)) AS `UFemitente`,`entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`icmsst` AS `icmsst`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`ratdesconto`) AS `desconto`,sum(`entradas`.`ratdespesas`) AS `totalDespesas`,sum(`entradas`.`ratseguro`) AS `totalSeguro`,sum(`entradas`.`ratfrete`) AS `totalFrete`,sum(`entradas`.`valoroutrasdespesas`) AS `valoroutrasdespesas`,if((`entradas`.`IcmsEntrada` > 0),sum(`entradas`.`bcicms`),0) AS `bcicms`,if((`entradas`.`IcmsEntrada` = 0),sum(`entradas`.`totalitem`),0) AS `valorisentas`,if((`entradas`.`IcmsEntrada` > 0),sum(round((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100)),2)),0) AS `toticms`,sum(`entradas`.`bcicmsST`) AS `bcicmsST`,sum(`entradas`.`valoricmsST`) AS `valoricmsST`,if((`entradas`.`IPI` > 0),sum(round((((((`entradas`.`totalitem` + `entradas`.`ratdespesas`) + `entradas`.`ratseguro`) + `entradas`.`ratfrete`) - `entradas`.`ratdesconto`) * (`entradas`.`IPI` / 100)),2)),0) AS `ipiItem`,round(if((`entradas`.`pis` > 0),(((((sum(round(`entradas`.`ratdespesas`,2)) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)) * (`entradas`.`pis` / 100)),0),2) AS `totPIS`,round(if((`entradas`.`cofins` > 0),(((((sum(round(`entradas`.`ratdespesas`,2)) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)) * (`entradas`.`cofins` / 100)),0),2) AS `totCOFINS`,sum(`entradas`.`totalitem`) AS `totalProduto`,((((((sum(round(((((`entradas`.`totalitem` + `entradas`.`ratseguro`) + `entradas`.`ratfrete`) - `entradas`.`ratdesconto`) * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)) AS `totalNF`,(((((sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) AS `valoroutrasaliquotas`,sum(round((`entradas`.`totalitem` * (`entradas`.`percentualRedBaseCalcICMS` / 100)),2)) AS `totalReducaoICMS`,if((`entradas`.`IPI` > 0),sum(`entradas`.`totalitem`),0) AS `baseCalculoIPI`,if((`entradas`.`pis` > 0),((((sum(round(`entradas`.`ratdespesas`,2)) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)),0) AS `baseCalculoPIS`,if((`entradas`.`cofins` > 0),((((sum(round(`entradas`.`ratdespesas`,2)) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)),0) AS `baseCalculoCOFINS`,`entradas`.`Lancada` AS `lancada`,sum(`entradas`.`valorICMS`) AS `valorICMS` from `entradas` where ((`entradas`.`exportarfiscal` = 'S') and (`entradas`.`Lancada` = 'X')) group by `entradas`.`NF`,`entradas`.`cfopentrada`,`entradas`.`IcmsEntrada`,`entradas`.`numero` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view registro50entradas_itens */

/*!50001 DROP TABLE IF EXISTS `registro50entradas_itens` */;
/*!50001 DROP VIEW IF EXISTS `registro50entradas_itens` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `registro50entradas_itens` AS select (select `moventradas`.`UFemitente` AS `UFemitente` from `moventradas` where (`moventradas`.`numero` = `entradas`.`numero`)) AS `UFemitente`,`entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`icmsst` AS `icmsst`,`entradas`.`IPI` AS `ipi`,`entradas`.`pis` AS `pis`,`entradas`.`cofins` AS `cofins`,`entradas`.`cstpis` AS `cstpis`,`entradas`.`cstcofins` AS `cstcofins`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`ratdesconto`) AS `desconto`,sum(`entradas`.`ratdespesas`) AS `totalDespesas`,sum(`entradas`.`ratseguro`) AS `totalSeguro`,sum(`entradas`.`ratfrete`) AS `totalFrete`,sum(`entradas`.`valoroutrasdespesas`) AS `valoroutrasdespesas`,if((`entradas`.`IcmsEntrada` > 0),sum(`entradas`.`bcicms`),0) AS `bcicms`,if((`entradas`.`IcmsEntrada` = 0),sum(`entradas`.`totalitem`),0) AS `valorisentas`,round(if((`entradas`.`IcmsEntrada` > 0),sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),0),2) AS `toticms`,sum(`entradas`.`bcicmsST`) AS `bcicmsST`,sum(`entradas`.`valoricmsST`) AS `valoricmsST`,if((`entradas`.`IPI` > 0),sum(round((((((`entradas`.`totalitem` + `entradas`.`ratdespesas`) + `entradas`.`ratseguro`) + `entradas`.`ratfrete`) - `entradas`.`ratdesconto`) * (`entradas`.`IPI` / 100)),2)),0) AS `ipiItem`,round(if((`entradas`.`pis` > 0),(((((sum(round(`entradas`.`ratdespesas`,2)) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)) * (`entradas`.`pis` / 100)),0),2) AS `totPIS`,round(if((`entradas`.`cofins` > 0),(((((sum(round(`entradas`.`ratdespesas`,2)) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)) * (`entradas`.`cofins` / 100)),0),2) AS `totCOFINS`,sum(`entradas`.`totalitem`) AS `totalProduto`,((((((sum(round(((((`entradas`.`totalitem` + `entradas`.`ratseguro`) + `entradas`.`ratfrete`) - `entradas`.`ratdesconto`) * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)) AS `totalNF`,(((((sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`valoricmsST`)) + sum(round(`entradas`.`ratdespesas`,2))) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) AS `valoroutrasaliquotas`,if((`entradas`.`IPI` > 0),sum(`entradas`.`totalitem`),0) AS `baseCalculoIPI`,if((`entradas`.`pis` > 0),((((sum(round(`entradas`.`ratdespesas`,2)) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)),0) AS `baseCalculoPIS`,if((`entradas`.`cofins` > 0),((((sum(round(`entradas`.`ratdespesas`,2)) + sum(round(`entradas`.`ratfrete`,2))) + sum(round(`entradas`.`ratseguro`,2))) - sum(round(`entradas`.`ratdesconto`,2))) + sum(`entradas`.`totalitem`)),0) AS `baseCalculoCOFINS`,`entradas`.`Lancada` AS `lancada`,`entradas`.`valorICMS` AS `valoricms` from `entradas` where ((`entradas`.`exportarfiscal` = 'S') and (`entradas`.`Lancada` = 'X')) group by `entradas`.`NF`,`entradas`.`inc` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view registro50saida_agr */

/*!50001 DROP TABLE IF EXISTS `registro50saida_agr` */;
/*!50001 DROP VIEW IF EXISTS `registro50saida_agr` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `registro50saida_agr` AS select `vendanftmp`.`inc` AS `inc`,`vendanftmp`.`codigofilial` AS `codigofilial`,`vendanftmp`.`NotaFiscal` AS `notafiscal`,`vendanftmp`.`serieNF` AS `serienf`,`vendanftmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendanftmp`.`documento` AS `documento`,`vendanftmp`.`data` AS `DATA`,`vendanftmp`.`cfop` AS `cfop`,`vendanftmp`.`icms` AS `icms`,`vendanftmp`.`icmsst` AS `icmsst`,ifnull(`vendanftmp`.`aliquotaIPI`,0) AS `ipi`,`vendanftmp`.`pis` AS `pis`,`vendanftmp`.`cofins` AS `cofins`,`vendanftmp`.`cstpis` AS `cstpis`,`vendanftmp`.`cstcofins` AS `cstcofins`,`vendanftmp`.`cstipi` AS `cstipi`,`vendanftmp`.`tributacao` AS `tributacao`,`vendanftmp`.`codigo` AS `codigo`,`vendanftmp`.`produto` AS `produto`,sum(`vendanftmp`.`quantidade`) AS `SUM(quantidade)`,sum(`vendanftmp`.`quantidade`) AS `quantidade`,`vendanftmp`.`unidade` AS `unidade`,`vendanftmp`.`nrcontrole` AS `nrcontrole`,`vendanftmp`.`origem` AS `origem`,sum(round((((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - ((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`icms`) / 100),2)) AS `totalicms`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((((`vendanftmp`.`total` - ((`vendanftmp`.`total` * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`aliquotaIPI`) / 100),2)),0) AS `totalIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`pis`) / 100),2)),0) AS `totalPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`cofins`) / 100),2)),0) AS `totalCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`cofins`)),0) AS `totalCOFINS_QTD`,sum((`vendanftmp`.`descontovalor` + `vendanftmp`.`ratdesc`)) AS `descontovalor`,if(((`vendanftmp`.`icms` = 0) or (`vendanftmp`.`tributacao` = '010')),sum(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`)),0) AS `valorisentas`,sum(round(`vendanftmp`.`total`,2)) AS `totalbruto`,sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)) AS `SUM(TOTAL)`,sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)) AS `total`,sum(`vendanftmp`.`total`) AS `totalItem`,if((`vendanftmp`.`icms` > 0),sum(round((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - (((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((`vendanftmp`.`total` - (`vendanftmp`.`total` * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD`,if((`vendanftmp`.`icmsst` > 0),sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)),0) AS `bcICMSST`,truncate(if(((`vendanftmp`.`icmsst` > 0) and (`vendanftmp`.`icmsst` >= `vendanftmp`.`icms`)),(((sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)) * `vendanftmp`.`icmsst`) / 100) - sum(round(((`vendanftmp`.`total` * `vendanftmp`.`icms`) / 100),2))),0),2) AS `totalICMSST` from `vendanftmp` where (`vendanftmp`.`quantidade` > 0) group by `vendanftmp`.`NotaFiscal`,`vendanftmp`.`serieNF`,`vendanftmp`.`icms`,`vendanftmp`.`cfop`,`vendanftmp`.`codigofilial` */;

/*View structure for view registro50saidas_itens */

/*!50001 DROP TABLE IF EXISTS `registro50saidas_itens` */;
/*!50001 DROP VIEW IF EXISTS `registro50saidas_itens` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `registro50saidas_itens` AS select `vendanftmp`.`inc` AS `inc`,`vendanftmp`.`codigofilial` AS `codigofilial`,`vendanftmp`.`NotaFiscal` AS `notafiscal`,`vendanftmp`.`serieNF` AS `serienf`,`vendanftmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendanftmp`.`documento` AS `documento`,`vendanftmp`.`data` AS `DATA`,`vendanftmp`.`cfop` AS `cfop`,`vendanftmp`.`icms` AS `icms`,`vendanftmp`.`icmsst` AS `icmsst`,ifnull(`vendanftmp`.`aliquotaIPI`,0) AS `ipi`,`vendanftmp`.`pis` AS `pis`,`vendanftmp`.`cofins` AS `cofins`,`vendanftmp`.`cstpis` AS `cstpis`,`vendanftmp`.`cstcofins` AS `cstcofins`,`vendanftmp`.`cstipi` AS `cstipi`,`vendanftmp`.`tributacao` AS `tributacao`,`vendanftmp`.`codigo` AS `codigo`,`vendanftmp`.`produto` AS `produto`,sum(`vendanftmp`.`quantidade`) AS `SUM(quantidade)`,sum(`vendanftmp`.`quantidade`) AS `quantidade`,`vendanftmp`.`unidade` AS `unidade`,`vendanftmp`.`nrcontrole` AS `nrcontrole`,`vendanftmp`.`origem` AS `origem`,sum(round((((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - ((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`icms`) / 100),2)) AS `totalicms`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((((`vendanftmp`.`total` - ((`vendanftmp`.`total` * `vendanftmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendanftmp`.`aliquotaIPI`) / 100),2)),0) AS `totalIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`pis`) / 100),2)),0) AS `totalPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`) * `vendanftmp`.`cofins`) / 100),2)),0) AS `totalCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`pis`)),0) AS `totalPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum((round(`vendanftmp`.`quantidade`,2) * `vendanftmp`.`cofins`)),0) AS `totalCOFINS_QTD`,sum((`vendanftmp`.`descontovalor` + `vendanftmp`.`ratdesc`)) AS `descontovalor`,if(((`vendanftmp`.`icms` = 0) or (`vendanftmp`.`tributacao` = '010')),sum(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`)),0) AS `valorisentas`,sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)) AS `SUM(TOTAL)`,sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)) AS `TOTAL`,sum((round(`vendanftmp`.`total`,2) + round(`vendanftmp`.`rateioencargos`,2))) AS `totalbruto`,if((`vendanftmp`.`icms` > 0),sum(round((((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) - (((((`vendanftmp`.`total` - `vendanftmp`.`ratdesc`) + `vendanftmp`.`ratdespesas`) + `vendanftmp`.`ratfrete`) + `vendanftmp`.`ratseguro`) * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS`,if((`vendanftmp`.`aliquotaIPI` > 0),sum(round((`vendanftmp`.`total` - (`vendanftmp`.`total` * (`vendanftmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoIPI`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoPIS`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` <> '03')),sum(round(((`vendanftmp`.`total` + `vendanftmp`.`rateioencargos`) - `vendanftmp`.`ratdesc`),2)),0) AS `baseCalculoCOFINS`,if(((`vendanftmp`.`pis` > 0) and (`vendanftmp`.`cstpis` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoPIS_QTD`,if(((`vendanftmp`.`cofins` > 0) and (`vendanftmp`.`cstcofins` = '03')),sum(round(`vendanftmp`.`quantidade`,2)),0) AS `baseCalculoCOFINS_QTD`,if((`vendanftmp`.`icmsst` > 0),sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)),0) AS `bcICMSST`,truncate(if(((`vendanftmp`.`icmsst` > 0) and (`vendanftmp`.`icmsst` >= `vendanftmp`.`icms`)),(((sum(round((`vendanftmp`.`total` + (`vendanftmp`.`total` * (`vendanftmp`.`mvast` / 100))),2)) * `vendanftmp`.`icmsst`) / 100) - sum(round(((`vendanftmp`.`total` * `vendanftmp`.`icms`) / 100),2))),0),2) AS `totalICMSST` from `vendanftmp` where (`vendanftmp`.`quantidade` > 0) group by `vendanftmp`.`inc` order by `vendanftmp`.`NotaFiscal`,`vendanftmp`.`nrcontrole` */;

/*View structure for view resumovendas */

/*!50001 DROP TABLE IF EXISTS `resumovendas` */;
/*!50001 DROP VIEW IF EXISTS `resumovendas` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `resumovendas` AS (select sum(((`contdocs`.`Totalbruto` - `contdocs`.`desconto`) + `contdocs`.`encargos`)) AS `TotalLiquido`,avg(((`contdocs`.`Totalbruto` - `contdocs`.`desconto`) + `contdocs`.`encargos`)) AS `ticket`,`contdocs`.`data` AS `DATA` from `contdocs` where (`contdocs`.`dpfinanceiro` = 'Venda') group by `contdocs`.`data`) */;

/*View structure for view v_assistentegerencial */

/*!50001 DROP TABLE IF EXISTS `v_assistentegerencial` */;
/*!50001 DROP VIEW IF EXISTS `v_assistentegerencial` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `v_assistentegerencial` AS (select `assistentegerencial`.`codigofilial` AS `codigofilial`,`assistentegerencial`.`ticketmedio` AS `ticketmedio`,`assistentegerencial`.`data` AS `data`,`assistentegerencial`.`hora` AS `hora`,`assistentegerencial`.`ocorrenciasauditoria` AS `ocorrenciasauditoria`,`assistentegerencial`.`auditoriacliente` AS `auditoriacliente`,`assistentegerencial`.`auditoriaacessos` AS `auditoriaacessos`,`assistentegerencial`.`auditoriaprodutos` AS `auditoriaprodutos`,`assistentegerencial`.`auditoriavendas` AS `auditoriavendas`,`assistentegerencial`.`auditoriacontaspagar` AS `auditoriacontaspagar`,`assistentegerencial`.`auditoriaestorno` AS `auditoriaestorno`,`assistentegerencial`.`vendadiaria` AS `vendadiaria`,`assistentegerencial`.`totalreceber` AS `totalreceber`,`assistentegerencial`.`totalrecebido` AS `totalrecebido` from `assistentegerencial` where (`assistentegerencial`.`data` = curdate())) */;

/*View structure for view valoresinventario */

/*!50001 DROP TABLE IF EXISTS `valoresinventario` */;
/*!50001 DROP VIEW IF EXISTS `valoresinventario` */;

/*!50001 CREATE ALGORITHM=UNDEFINED  SQL SECURITY DEFINER VIEW `valoresinventario` AS select `produtos`.`CodigoFilial` AS `codigofilial`,`produtos`.`tipo` AS `tipo`,`produtos`.`grupo` AS `grupo`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`custo`),2)) AS `custo`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`),2)) AS `customedio`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`custofornecedor`),2)) AS `custofornecedor`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`precovenda`),2)) AS `total`,sum(truncate((`produtos`.`qtdretida` * `produtos`.`custo`),2)) AS `custoretidos`,sum(truncate((`produtos`.`qtdvencidos` * `produtos`.`custo`),2)) AS `custovencidos`,sum(truncate(`produtos`.`quantidade`,2)) AS `quantidade`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`icms`) / 100),2)) AS `icmsRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`aliquotaIPI`) / 100),2)) AS `ipiRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`pis`) / 100),2)) AS `pisRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`cofins`) / 100),2)) AS `cofinsRecuperar` from `produtos` where (`produtos`.`quantidade` > 0) group by `produtos`.`CodigoFilial`,`produtos`.`tipo` union all select `produtos`.`CodigoFilial` AS `codigofilial`,`produtos`.`tipo` AS `tipo`,`produtos`.`grupo` AS `grupo`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`custo`),2)) AS `custo`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`),2)) AS `customedio`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`custofornecedor`),2)) AS `custofornecedor`,sum(truncate(((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`precovenda`),2)) AS `total`,sum(truncate((`produtos`.`qtdretida` * `produtos`.`custo`),2)) AS `custoretidos`,sum(truncate((`produtos`.`qtdvencidos` * `produtos`.`custo`),2)) AS `custovencidos`,sum(truncate(`produtos`.`quantidade`,2)) AS `quantidade`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`icms`) / 100),2)) AS `icmsRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`aliquotaIPI`) / 100),2)) AS `ipiRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`pis`) / 100),2)) AS `pisRecuperar`,sum(truncate(((((`produtos`.`quantidade` + `produtos`.`qtdretida`) * `produtos`.`customedio`) * `produtos`.`cofins`) / 100),2)) AS `cofinsRecuperar` from `produtosfilial` `produtos` where (`produtos`.`quantidade` > 0) group by `produtos`.`CodigoFilial`,`produtos`.`tipo` */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

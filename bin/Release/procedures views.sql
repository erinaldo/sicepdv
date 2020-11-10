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
/* Procedure structure for procedure `AjustarCamposNulos` */

/*!50003 DROP PROCEDURE IF EXISTS  `AjustarCamposNulos` */;

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
update vendaarquivo set coo = "" where coo is null;
UPDATE vendanf SET serienf="1" WHERE serienf IS NULL;
UPDATE produtos set marcado="" where marcado="P";
UPDATE produtosfilial SET marcado="" WHERE marcado="P";
UPDATE produtos SET ncm="" WHERE ncm IS NULL;
UPDATE produtosfilial SET ncm="" WHERE ncm IS NULL;
UPDATE produtos SET nbm="" WHERE nbm IS NULL;
UPDATE produtosfilial SET nbm="" WHERE nbm IS NULL;
UPDATE produtos SET ncmespecie="" WHERE ncmespecie IS NULL;
UPDATE produtosfilial SET ncmespecie="" WHERE ncmespecie IS NULL;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `AtualizarDebitoCliente` */

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarDebitoCliente` */;

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

/*!50003 DROP PROCEDURE IF EXISTS  `AtualizarEstoqueOff` */;

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

/* Procedure structure for procedure `CriarTabelasTemp` */

/*!50003 DROP PROCEDURE IF EXISTS  `CriarTabelasTemp` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `CriarTabelasTemp`(IN tabela VARCHAR(10),IN filial VARCHAR(5),IN dataInicial DATE,IN dataFinal DATE)
BEGIN
IF (tabela="venda") THEN
DROP TABLE IF EXISTS `vendatmp`;
CREATE TABLE `vendaTMP` SELECT * FROM `vendaarquivo` WHERE codigofilial=filial AND DATA BETWEEN dataInicial AND datafinal;
END IF;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `EncerrarCaixa` */

/*!50003 DROP PROCEDURE IF EXISTS  `EncerrarCaixa` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `EncerrarCaixa`(in idOperador varchar(10),in filial varchar(5),in ipTerminal varchar(15)  )
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
UPDATE produtos SET saldofinalestoque=quantidade,EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,dataultvenda));
UPDATE produtosfilial SET saldofinalestoque=quantidade,EADE2mercadoriaEstoque=MD5(CONCAT(codigo,descricao,saldofinalestoque,dataultvenda)); 
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

/*!50003 DROP PROCEDURE IF EXISTS  `EstornarQuitacao` */;

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

/*!50003 DROP PROCEDURE IF EXISTS  `ExcluirDocumento` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ExcluirDocumento`(in nrDocumento int,in filial varchar(5),in operador varchar(10) )
BEGIN
 set @tabelaProduto='produtosfilial';
 IF (filial='00001') THEN
 SET @tabelaProduto='produtos';
 END IF;
 INSERT INTO contdocs 
 (ip,codigofilial,data,dataexe,totalbruto,desconto,vrjuros,encargos,total,nome,
 codigocliente,NrParcelas,vendedor,operador,observacao,classe,
 historico,dpfinanceiro,tipopagamento,id,custos,
 devolucaovenda,devolucaorecebimento,valorservicos,descontoservicos,hora,estornado,concluido) 
 SELECT ip,codigofilial,data,dataexe,totalbruto*-1,desconto*-1,vrjuros*-1,encargos*-1,total*-1,nome,
 codigocliente,NrParcelas,vendedor,operador,observacao,classe,
 "Venda est",dpfinanceiro,tipopagamento,id,custos*-1,
 devolucaovenda*-1,devolucaorecebimento*-1,valorservicos*-1,descontoservicos*-1,current_time,"S","S"  
 FROM contdocs where documento=nrDocumento;
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
 DELETE FROM itensaentregar where documento=nrDocumento;
  
 UPDATE produtosgrade,venda
 SET produtosgrade.quantidade=produtosgrade.quantidade+(select sum(venda.quantidade) from venda where venda.documento=nrdocumento and produtosgrade.codigo=venda.codigo AND venda.grade=produtosgrade.grade)
 WHERE produtosgrade.codigo=venda.codigo
 AND produtosgrade.grade=venda.grade
 AND venda.documento=nrDocumento
 AND produtosgrade.codigofilial=filial;
 INSERT INTO vendaexclusao select * from venda where documento=nrDocumento;
 DELETE from venda WHERE documento=nrDocumento;
 INSERT INTO caixa (codigofilial,enderecoIP,nome,codigocliente,valor,dataexe,data,vencimento,
 tipopagamento,banco,agencia,cheque,valorcheque,
 nomecheque,cartao,numerocartao,operador,dpfinanceiro,historico,documento, 
 vrjuros,vrdesconto,encargos,custos,vendedor)
 SELECT codigofilial,enderecoIP,nome,codigocliente,valor*-1,dataexe,data,vencimento,
 tipopagamento,banco,agencia,cheque,valorcheque*-1,
 nomecheque,cartao,numerocartao,operador,dpfinanceiro,historico,(select max(documento) from contdocs), 
 vrjuros*-1,vrdesconto*-1,encargos*-1,custos*-1,vendedor 
 FROM caixa where documento=nrDocumento;	
 DELETE FROM crmovclientes where documento=nrDocumento 
 AND documento<>0;
 DELETE FROM cheques where documento=nrDocumento;
 DELETE FROM movcartoes where documento=nrDocumento;
 INSERT INTO auditoria (codigofilial,usuario,hora,data,tabela,acao,documento,local) 
 values (
 filial,operador,current_time,current_date,'Venda','Estorno',nrDocumento,
 (select nome from contdocs where documento=nrDocumento limit 1)
 );
 UPDATE clientes 
 SET valorcartaofidelidade=valorcartaofidelidade+(select total from contdocs where documento=nrDocumento)
 WHERE codigo=(select cartaofidelidade from contdocs where documento=nrDocumento);
 UPDATE contdocs set concluido='S',estornado='S' 
 WHERE documento=nrDocumento;
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarDAV` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarDAV` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `FinalizarDAV`(in DAVNumero int,in filial varchar(5),in ipTerminal varchar(15))
BEGIN
 UPDATE caixas SET 
 documento=DAVNumero,
 codigofilial=filial,
 vendedor=(select vendedor from contdav where numero=DAVNumero limit 1),
 vrdesconto=0,
 operador=(select operador from contdav where numero=DAVNumero limit 1)
 WHERE enderecoip=ipTerminal;
 
 UPDATE caixas set
 vrdesconto=(select desconto from contdav where numero=DAVNumero limit 1)
 WHERE enderecoip=ipTerminal;
 UPDATE vendas SET 
 documento=DAVNumero,
 codigofilial=filial,
 vendedor=(select vendedor from contdav where numero=DAVNumero limit 1),		
 operador=(select operador from contdav where numero=DAVNumero limit 1)
 WHERE id=ipTerminal;
 
 
 INSERT INTO `vendadav` (`acrescimototalitem`, `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`
 FROM vendas where id=ipTerminal;
 
 
 INSERT INTO `caixadav` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas where enderecoip=ipTerminal and valor<>0;
  
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
 END */$$
DELIMITER ;

/* Procedure structure for procedure `FinalizarDAVOS` */

/*!50003 DROP PROCEDURE IF EXISTS  `FinalizarDAVOS` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `FinalizarDAVOS`(in DAVNumero int,in filial varchar(5),in ipTerminal varchar(15))
BEGIN
 UPDATE caixas SET 
 documento=DAVNumero,
 codigofilial=filial,
 vendedor=(select vendedor from contdav where numero=DAVNumero limit 1),
 vrdesconto=0,
 operador=(select operador from contdav where numero=DAVNumero limit 1)
 WHERE enderecoip=ipTerminal;
 
 UPDATE caixas set
 vrdesconto=(select desconto from contdav where numero=DAVNumero limit 1)
 WHERE enderecoip=ipTerminal;
 UPDATE vendas SET 
 documento=DAVNumero,
 codigofilial=filial,
 vendedor=(select vendedor from contdav where numero=DAVNumero limit 1),		
 operador=(select operador from contdav where numero=DAVNumero limit 1)
 WHERE id=ipTerminal;
 
 
 INSERT INTO `vendadavos` (`acrescimototalitem`, `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`) 
 SELECT `acrescimototalitem`,`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`
 FROM vendas where id=ipTerminal;
 
 
 INSERT INTO `caixadavos` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas where enderecoip=ipTerminal and valor<>0;
  
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
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
 operador=(select operador from contprevendasPAF where numero=preVendaNumero limit 1)
 WHERE id=ipTerminal;
  
 INSERT INTO `vendaprevendapaf` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`) 
 SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`cancelado`
 FROM vendas where id=ipTerminal;
 
 
 INSERT INTO `caixaprevendapaf` (`horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`)
 SELECT `horaabertura`,`EnderecoIP`,`documento`,`tipopagamento`,`valor`,`dataexe`,`data`,`CodigoFilial`,`VrJuros`,`jurosch`,`vrdesconto`,`vendedor`,`datapagamento`,`vencimento`,`nome`,`sequencia`,`caixa`,`financeira`,`CrInicial`,`CrFinal`,`banco`,`cheque`,`agencia`,`valorCheque`,`Cartao`,`numeroCartao`,`Nrparcela`,`encargos`,`NomeCheque`,`classe`,`codigocliente`,`operador`,`historico`,`dpfinanceiro`,`custos`,`ocorrencia`,`filialorigem`,`valortarifabloquete`,`cobrador`,`contacorrentecheque`,`jurosfactoring`,`versao`,`valorservicos`,`descontoservicos`,`jurosCA`,`cpfcnpjch`
 FROM caixas where enderecoip=ipTerminal and valor<>0;
 
 DELETE from caixas where enderecoip=ipTerminal;
 DELETE from vendas where id=ipTerminal;
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
 DECLARE percDesconto REAL DEFAULT 0;
 DECLARE valorDescontoItens REAL DEFAULT 0;
 DECLARE cursorQtdItens CURSOR FOR SELECT IFNULL( COUNT(1) ,0) FROM vendas WHERE id=ipTerminal AND cancelado='N';
 DECLARE cursorSencargos CURSOR FOR SELECT IFNULL( SUM(ABS(quantidade)*precooriginal-ratdesc) ,0 ) AS totalsemencargos FROM vendas WHERE id=ipTerminal AND cancelado='N';	
 DECLARE cnfPreVenda CURSOR FOR SELECT abaterestoqueprevenda FROM configfinanc WHERE codigofilial=filial;
 DECLARE cursorDesconto CURSOR FOR SELECT (desconto*100)/totalbruto FROM contdocs WHERE documento=doc;
 DECLARE cursorDescontoItens CURSOR FOR SELECT SUM(ratdesc) FROM vendas WHERE id=ipTerminal AND cancelado='N';
 OPEN cursorQtdItens;
 OPEN cursorSencargos;
 OPEN cnfPreVenda;
 OPEN cursorDesconto;
 FETCH cursorQtdItens INTO qtdITens;
 FETCH cursorSencargos INTO vlrSemEncargos;
 FETCH cnfPrevenda INTO baixarPreVenda;
 FETCH cursorDesconto INTO percDesconto;
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
vendas.data=current_date 
WHERE vendas.id=ipTerminal;
 
 UPDATE caixas SET 
 documento=doc,codigofilial=filial,
 dpfinanceiro=(SELECT contdocs.dpfinanceiro FROM contdocs WHERE contdocs.documento=doc),
 operador=(SELECT contdocs.operador FROM contdocs WHERE contdocs.documento=doc),
 classe=(SELECT contdocs.classe FROM contdocs WHERE contdocs.documento=doc),
 vendedor=(SELECT contdocs.vendedor FROM contdocs WHERE contdocs.documento=doc),
 data=current_date
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
 vendas.ratdesc=truncate((vendas.preco*vendas.quantidade)*(',percDesconto,'/100),2),
 vendas.rateioencargos=((select total from contdocs where documento=',doc,')-',VlrSemEncargos,')/',qtdItens,',
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
 
 INSERT INTO `venda` (`codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`modelodocfiscal`,`serienf`,`subserienf`,`ecffabricacao`,`coo`,`acrescimototalitem`,`cancelado`) 
 SELECT `codigofilial`,`operador`,`data`,`codigo`,`produto`,`quantidade`,`preco`,`custo`,`precooriginal`,`Descontoperc`,`id`,`descontovalor`,`total`,`vendedor`,`nrcontrole`,`documento`,`grupo`,`subgrupo`,`comissao`,`ratdesc`,`rateioencargos`,`situacao`,`customedio`,`Ecfnumero`,`fornecedor`,`fabricante`,`NotaFiscal`,`icms`,`classe`,`secao`,`lote`,`tributacao`,`aentregar`,`quantidadeanterior`,`quantidadeatualizada`,`codigofiscal`,`customedioanterior`,`codigocliente`,`numerodevolucao`,`codigobarras`,`ipi`,`unidade`,`embalagem`,`grade`,`romaneio`,`tipo`,`cofins`,`pis`,`despesasacessorias`,`percentualRedBaseCalcICMS`,`modelodocfiscal` ,`serienf`,`subserienf`,`ecffabricacao`,`coo`,`acrescimototalitem`,`cancelado`
 FROM vendas
 WHERE id=ipTerminal;
 
 INSERT INTO caixa (horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,DATA,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch)  
 SELECT horaabertura,EnderecoIP,documento,tipopagamento,
 valor,dataexe,DATA,CodigoFilial,VrJuros,jurosch,vrdesconto,vendedor,datapagamento,
 vencimento,nome,sequencia,caixa,financeira,CrInicial,CrFinal,banco,cheque,agencia,
 valorCheque,Cartao,numeroCartao,Nrparcela,encargos,NomeCheque,classe,codigocliente,
 operador,historico,dpfinanceiro,custos,ocorrencia,filialorigem,valortarifabloquete,
 cobrador,contacorrentecheque,jurosfactoring,versao,valorservicos,descontoservicos,
 jurosCA,cpfcnpjch 
 FROM caixas 
 WHERE enderecoip=ipTerminal;
 
 UPDATE contdocs SET concluido='S',
 EADr06=MD5(CONCAT(ecffabricacao,ncupomfiscal,contadornaofiscalGNF,contadordebitocreditoCDC,DATA,coognf,tipopagamento )),
 EADRegistroDAV=MD5(CONCAT(ncupomfiscal,davnumero,DATA,total))
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
 ncupomfiscal=(SELECT ncupomfiscal FROM contdocs WHERE documento=doc)
 WHERE numeroDAVfilial=DAVNumero and codigofilial=filial;	
 UPDATE contdavos SET finalizada='S',
 datafinalizacao=CURRENT_DATE,
 ncupomfiscal=(SELECT ncupomfiscal FROM contdocs WHERE documento=doc)
 WHERE numero=DAVNumero AND codigofilial=filial;	
 
 END IF; 
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

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarDAVOS` */;

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

/*!50003 DROP PROCEDURE IF EXISTS  `ProcessarPreVenda` */;

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
 `tributacao` char(2) ,
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
 `tributacao` char(2) ,
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
 `preco` decimal(10,2) ,
 `totalicms` decimal(47,2) ,
 `descontovalor` decimal(32,2) ,
 `SUM(TOTAL)` decimal(32,2) ,
 `baseCalculoICMS` decimal(37,2) 
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
 `total` decimal(35,4) ,
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
 `totalICMS` decimal(42,2) 
)*/;

/*Table structure for table `blococregc425` */

DROP TABLE IF EXISTS `blococregc425`;

/*!50001 DROP VIEW IF EXISTS `blococregc425` */;
/*!50001 DROP TABLE IF EXISTS `blococregc425` */;

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
 `total` decimal(33,2) ,
 `icms` int(11) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) ,
 `cancelado` char(1) 
)*/;

/*Table structure for table `blococregc470` */

DROP TABLE IF EXISTS `blococregc470`;

/*!50001 DROP VIEW IF EXISTS `blococregc470` */;
/*!50001 DROP TABLE IF EXISTS `blococregc470` */;

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
 `total` decimal(11,2) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) 
)*/;

/*Table structure for table `blococregc490` */

DROP TABLE IF EXISTS `blococregc490`;

/*!50001 DROP VIEW IF EXISTS `blococregc490` */;
/*!50001 DROP TABLE IF EXISTS `blococregc490` */;

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
 `total` decimal(33,2) ,
 `baseCalculoICMS` decimal(31,0) ,
 `totalICMS` decimal(42,2) ,
 `icms` int(2) ,
 `tributacao` char(3) ,
 `cfop` varchar(5) 
)*/;

/*Table structure for table `r05` */

DROP TABLE IF EXISTS `r05`;

/*!50001 DROP VIEW IF EXISTS `r05` */;
/*!50001 DROP TABLE IF EXISTS `r05` */;

/*!50001 CREATE TABLE  `r05`(
 `ncupomfiscal` varchar(10) ,
 `ecfcontadorcupomfiscal` varchar(10) ,
 `data` date ,
 `ecfnumero` char(3) ,
 `estornado` char(1) ,
 `dpfinanceiro` varchar(15) ,
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
 `indicadorproducao` char(1) ,
 `indicadorarredondamentotruncamento` char(1) 
)*/;

/*Table structure for table `registro50entradas_agr` */

DROP TABLE IF EXISTS `registro50entradas_agr`;

/*!50001 DROP VIEW IF EXISTS `registro50entradas_agr` */;
/*!50001 DROP TABLE IF EXISTS `registro50entradas_agr` */;

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

/*!50001 DROP VIEW IF EXISTS `registro50entradas_itens` */;
/*!50001 DROP TABLE IF EXISTS `registro50entradas_itens` */;

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

/*!50001 DROP VIEW IF EXISTS `registro50saida_agr` */;
/*!50001 DROP TABLE IF EXISTS `registro50saida_agr` */;

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

/*Table structure for table `vprodutos` */

DROP TABLE IF EXISTS `vprodutos`;

/*!50001 DROP VIEW IF EXISTS `vprodutos` */;
/*!50001 DROP TABLE IF EXISTS `vprodutos` */;

/*!50001 CREATE TABLE  `vprodutos`(
 `codigofilial` varchar(5) ,
 `codigo` varchar(20) ,
 `codigobarras` varchar(20) ,
 `descricao` varchar(50) ,
 `marcado` char(1) ,
 `quantidade` decimal(10,2) ,
 `precovenda` decimal(12,2) ,
 `custo` decimal(12,5) ,
 `situacao` varchar(15) ,
 `qtdprateleiras` decimal(10,2) ,
 `unidade` char(3) ,
 `icms` decimal(6,2) ,
 `descontopromocao` decimal(6,2) ,
 `descontomaximo` decimal(6,2) ,
 `qtdprovisoria` decimal(10,2) ,
 `qtdretida` decimal(8,2) ,
 `classe` varchar(4) ,
 `tributacao` char(3) ,
 `deposito` decimal(11,2) ,
 `qtdvencidos` decimal(10,2) ,
 `qtdaentregar` decimal(10,2) ,
 `codigofiscal` char(3) ,
 `ativacompdesc` char(1) ,
 `qtdminimadesc` decimal(8,2) ,
 `qtdprevenda` decimal(10,2) ,
 `disponivel` decimal(11,2) ,
 `precominimo` decimal(23,8) ,
 `ipi` decimal(6,2) ,
 `precosemfinanciamento` decimal(8,2) ,
 `precoatacado` decimal(10,2) ,
 `embalagem` int(5) ,
 `grade` varchar(10) ,
 `volumes` int(3) ,
 `tipo` varchar(15) ,
 `percentualRedBaseCalcICMS` decimal(5,2) ,
 `unidembalagem` char(3) ,
 `lote` varchar(15) ,
 `aceitadesconto` char(1) 
)*/;

/*Table structure for table `vprodutosfilial` */

DROP TABLE IF EXISTS `vprodutosfilial`;

/*!50001 DROP VIEW IF EXISTS `vprodutosfilial` */;
/*!50001 DROP TABLE IF EXISTS `vprodutosfilial` */;

/*!50001 CREATE TABLE  `vprodutosfilial`(
 `codigofilial` varchar(5) ,
 `codigo` varchar(20) ,
 `codigobarras` varchar(20) ,
 `descricao` varchar(50) ,
 `marcado` char(1) ,
 `quantidade` decimal(10,2) ,
 `precovenda` decimal(12,2) ,
 `custo` decimal(12,5) ,
 `situacao` varchar(15) ,
 `qtdprateleiras` decimal(10,2) ,
 `unidade` char(3) ,
 `icms` decimal(6,2) ,
 `descontopromocao` decimal(6,2) ,
 `descontomaximo` decimal(6,2) ,
 `qtdprovisoria` decimal(10,2) ,
 `qtdretida` decimal(8,2) ,
 `classe` varchar(4) ,
 `tributacao` char(3) ,
 `deposito` decimal(11,2) ,
 `qtdvencidos` decimal(10,2) ,
 `qtdaentregar` decimal(10,2) ,
 `codigofiscal` char(3) ,
 `ativacompdesc` char(1) ,
 `qtdminimadesc` decimal(8,2) ,
 `qtdprevenda` decimal(10,2) ,
 `disponivel` decimal(11,2) ,
 `precominimo` decimal(23,8) ,
 `ipi` decimal(6,2) ,
 `precosemfinanciamento` decimal(8,2) ,
 `precoatacado` decimal(10,2) ,
 `embalagem` int(5) ,
 `grade` varchar(10) ,
 `volumes` int(3) ,
 `tipo` varchar(15) ,
 `percentualRedBaseCalcICMS` decimal(5,2) ,
 `unidembalagem` char(3) ,
 `lote` varchar(15) ,
 `aceitadesconto` char(1) 
)*/;

/*View structure for view 60d */

/*!50001 DROP TABLE IF EXISTS `60d` */;
/*!50001 DROP VIEW IF EXISTS `60d` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `60d` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`ratdesc`) AS `descontovalorCupom`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,sum(`vendatmp`.`acrescimototalitem`) AS `acrescimototalitem`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` group by `vendatmp`.`data`,`vendatmp`.`codigo` */;

/*View structure for view 60i */

/*!50001 DROP TABLE IF EXISTS `60i` */;
/*!50001 DROP VIEW IF EXISTS `60i` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `60i` AS select `vendatmp`.`codigofilial` AS `codigofilial`,`vendatmp`.`NotaFiscal` AS `notafiscal`,`vendatmp`.`serieNF` AS `serienf`,`vendatmp`.`modelodocfiscal` AS `modelodocfiscal`,`vendatmp`.`documento` AS `documento`,`vendatmp`.`data` AS `DATA`,`vendatmp`.`cfop` AS `cfop`,`vendatmp`.`icms` AS `icms`,`vendatmp`.`tributacao` AS `tributacao`,`vendatmp`.`codigo` AS `codigo`,`vendatmp`.`produto` AS `produto`,sum(`vendatmp`.`quantidade`) AS `SUM(quantidade)`,`vendatmp`.`unidade` AS `unidade`,`vendatmp`.`nrcontrole` AS `nrcontrole`,`vendatmp`.`ecffabricacao` AS `ecffabricacao`,`vendatmp`.`coo` AS `coo`,`vendatmp`.`preco` AS `preco`,`vendatmp`.`cancelado` AS `cancelado`,sum(round((((`vendatmp`.`total` - ((`vendatmp`.`total` * `vendatmp`.`percentualRedBaseCalcICMS`) / 100)) * `vendatmp`.`icms`) / 100),2)) AS `totalicms`,sum(`vendatmp`.`descontovalor`) AS `descontovalor`,sum(`vendatmp`.`ratdesc`) AS `descontovalorCupom`,sum(`vendatmp`.`total`) AS `SUM(TOTAL)`,if((`vendatmp`.`icms` > 0),sum(round((`vendatmp`.`total` - (`vendatmp`.`total` * (`vendatmp`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendatmp` group by `vendatmp`.`codigo` */;

/*View structure for view 60r */

/*!50001 DROP TABLE IF EXISTS `60r` */;
/*!50001 DROP VIEW IF EXISTS `60r` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `60r` AS select `vendaarquivo`.`codigofilial` AS `codigofilial`,`vendaarquivo`.`NotaFiscal` AS `notafiscal`,`vendaarquivo`.`serieNF` AS `serienf`,`vendaarquivo`.`modelodocfiscal` AS `modelodocfiscal`,`vendaarquivo`.`documento` AS `documento`,`vendaarquivo`.`data` AS `DATA`,`vendaarquivo`.`cfop` AS `cfop`,`vendaarquivo`.`icms` AS `icms`,`vendaarquivo`.`tributacao` AS `tributacao`,`vendaarquivo`.`codigo` AS `codigo`,`vendaarquivo`.`produto` AS `produto`,sum(`vendaarquivo`.`quantidade`) AS `SUM(quantidade)`,`vendaarquivo`.`unidade` AS `unidade`,`vendaarquivo`.`nrcontrole` AS `nrcontrole`,`vendaarquivo`.`ecffabricacao` AS `ecffabricacao`,`vendaarquivo`.`coo` AS `coo`,`vendaarquivo`.`preco` AS `preco`,sum(round((((`vendaarquivo`.`total` - ((`vendaarquivo`.`total` * `vendaarquivo`.`percentualRedBaseCalcICMS`) / 100)) * `vendaarquivo`.`icms`) / 100),2)) AS `totalicms`,sum(`vendaarquivo`.`descontovalor`) AS `descontovalor`,sum(`vendaarquivo`.`total`) AS `SUM(TOTAL)`,if((`vendaarquivo`.`icms` > 0),sum(round((`vendaarquivo`.`total` - (`vendaarquivo`.`total` * (`vendaarquivo`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendaarquivo` group by month(`vendaarquivo`.`data`) */;

/*View structure for view blococregc190 */

/*!50001 DROP TABLE IF EXISTS `blococregc190` */;
/*!50001 DROP VIEW IF EXISTS `blococregc190` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc190` AS select `entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`bcicms`) AS `bcicms`,round(sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),2) AS `toticms`,sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) AS `ipiItem`,sum(`entradas`.`totalitem`) AS `totalProduto`,(sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`totalitem`)) AS `totalNF` from `entradas` group by `entradas`.`NF`,`entradas`.`cfopentrada`,`entradas`.`IcmsEntrada`,`entradas`.`tributacao`,`entradas`.`numero` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view blococregc300 */

/*!50001 DROP TABLE IF EXISTS `blococregc300` */;
/*!50001 DROP VIEW IF EXISTS `blococregc300` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc300` AS select `contdocs`.`ip` AS `ip`,`contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`Totalbruto` AS `Totalbruto`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,`contdocs`.`desconto` AS `desconto`,`contdocs`.`total` AS `total`,`contdocs`.`NrParcelas` AS `NrParcelas`,`contdocs`.`vendedor` AS `vendedor`,`contdocs`.`operador` AS `operador`,`contdocs`.`Observacao` AS `Observacao`,`contdocs`.`classe` AS `classe`,`contdocs`.`dataexe` AS `dataexe`,`contdocs`.`codigocliente` AS `codigocliente`,`contdocs`.`nome` AS `nome`,`contdocs`.`CodigoFilial` AS `CodigoFilial`,`contdocs`.`historico` AS `historico`,`contdocs`.`vrjuros` AS `vrjuros`,`contdocs`.`tipopagamento` AS `tipopagamento`,`contdocs`.`encargos` AS `encargos`,`contdocs`.`id` AS `id`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`enderecoentrega` AS `enderecoentrega`,`contdocs`.`custos` AS `custos`,`contdocs`.`devolucaovenda` AS `devolucaovenda`,`contdocs`.`devolucaorecebimento` AS `devolucaorecebimento`,`contdocs`.`nrboletobancario` AS `nrboletobancario`,`contdocs`.`nrnotafiscal` AS `nrnotafiscal`,`contdocs`.`classedevolucao` AS `classedevolucao`,`contdocs`.`responsavelreceber` AS `responsavelreceber`,`contdocs`.`numeroentrega` AS `numeroentrega`,`contdocs`.`cidadeentrega` AS `cidadeentrega`,`contdocs`.`cepentrega` AS `cepentrega`,`contdocs`.`bairroentrega` AS `bairroentrega`,`contdocs`.`horaentrega` AS `horaentrega`,`contdocs`.`dataentrega` AS `dataentrega`,`contdocs`.`obsentrega` AS `obsentrega`,`contdocs`.`concluido` AS `concluido`,`contdocs`.`cartaofidelidade` AS `cartaofidelidade`,`contdocs`.`bordero` AS `bordero`,`contdocs`.`valorservicos` AS `valorservicos`,`contdocs`.`descontoservicos` AS `descontoservicos`,`contdocs`.`romaneio` AS `romaneio`,`contdocs`.`hora` AS `hora`,`contdocs`.`entregaconcluida` AS `entregaconcluida`,`contdocs`.`dataentregaconcluida` AS `dataentregaconcluida`,`contdocs`.`operadorentrega` AS `operadorentrega`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`nreducaoz` AS `nreducaoz`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`TEF` AS `TEF`,`contdocs`.`ecfValorCancelamentos` AS `ecfValorCancelamentos`,`contdocs`.`NF_e` AS `NF_e`,`contdocs`.`estadoentrega` AS `estadoentrega`,`contdocs`.`ecfConsumidor` AS `ecfConsumidor`,`contdocs`.`ecfCPFCNPJconsumidor` AS `ecfCPFCNPJconsumidor`,`contdocs`.`ecfEndConsumidor` AS `ecfEndConsumidor`,`contdocs`.`prevendanumero` AS `prevendanumero`,`contdocs`.`ecfcontadorcupomfiscal` AS `ecfcontadorcupomfiscal`,`contdocs`.`ecftotalliquido` AS `ecftotalliquido`,`contdocs`.`contadornaofiscalGNF` AS `contadornaofiscalGNF`,`contdocs`.`contadordebitocreditoCDC` AS `contadordebitocreditoCDC`,`contdocs`.`totalICMScupomfiscal` AS `totalICMScupomfiscal`,`contdocs`.`troco` AS `troco`,`contdocs`.`davnumero` AS `davnumero`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfMFadicional` AS `ecfMFadicional`,`contdocs`.`ecftipo` AS `ecftipo`,`contdocs`.`ecfmarca` AS `ecfmarca`,`contdocs`.`ecfmodelo` AS `ecfmodelo`,`contdocs`.`estoqueatualizado` AS `estoqueatualizado`,`contdocs`.`serienf` AS `serienf`,`contdocs`.`EADRegistroDAV` AS `EADRegistroDAV`,`contdocs`.`EADr06` AS `EADr06`,`contdocs`.`tipopagamentoECF` AS `tipopagamentoECF`,`contdocs`.`modeloDOCFiscal` AS `modeloDOCFiscal`,`contdocs`.`subserienf` AS `subserienf`,sum(`contdocs`.`total`) AS `totalDocumento` from `contdocs` where ((`contdocs`.`modeloDOCFiscal` = '02') or (`contdocs`.`modeloDOCFiscal` = 'D1')) group by `contdocs`.`data`,`contdocs`.`serienf`,`contdocs`.`subserienf` */;

/*View structure for view blococregc320 */

/*!50001 DROP TABLE IF EXISTS `blococregc320` */;
/*!50001 DROP VIEW IF EXISTS `blococregc320` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc320` AS select `vendaarquivo`.`data` AS `data`,`vendaarquivo`.`documento` AS `documento`,`vendaarquivo`.`serieNF` AS `serieNF`,`vendaarquivo`.`subserienf` AS `subserienf`,`vendaarquivo`.`modelodocfiscal` AS `modelodocfiscal`,`vendaarquivo`.`ecfnumero` AS `ecfnumero`,`vendaarquivo`.`NotaFiscal` AS `NotaFiscal`,`vendaarquivo`.`tributacao` AS `tributacao`,`vendaarquivo`.`cfop` AS `cfop`,`vendaarquivo`.`icms` AS `icms`,`vendaarquivo`.`total` AS `total`,sum(if((`vendaarquivo`.`icms` > 0),`vendaarquivo`.`total`,0)) AS `bcICMS`,((`vendaarquivo`.`total` * `vendaarquivo`.`icms`) / 100) AS `totalICMS` from `vendaarquivo` group by `vendaarquivo`.`tributacao`,`vendaarquivo`.`cfop`,`vendaarquivo`.`icms`,`vendaarquivo`.`data` union all select `venda`.`data` AS `data`,`venda`.`documento` AS `documento`,`venda`.`serieNF` AS `serieNF`,`venda`.`subserienf` AS `subserienf`,`venda`.`modelodocfiscal` AS `modelodocfiscal`,`venda`.`Ecfnumero` AS `ecfnumero`,`venda`.`NotaFiscal` AS `NotaFiscal`,`venda`.`tributacao` AS `tributacao`,`venda`.`cfop` AS `cfop`,`venda`.`icms` AS `icms`,sum((`venda`.`total` - `venda`.`ratdesc`)) AS `total`,sum(if((`venda`.`icms` > 0),`venda`.`total`,0)) AS `bcICMS`,((`venda`.`total` * `venda`.`icms`) / 100) AS `totalICMS` from `venda` group by `venda`.`tributacao`,`venda`.`cfop`,`venda`.`icms`,`venda`.`data` */;

/*View structure for view blococregc321 */

/*!50001 DROP TABLE IF EXISTS `blococregc321` */;
/*!50001 DROP VIEW IF EXISTS `blococregc321` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc321` AS select `venda`.`inc` AS `inc`,`venda`.`codigofilial` AS `codigofilial`,`venda`.`operador` AS `operador`,`venda`.`data` AS `data`,`venda`.`codigo` AS `codigo`,`venda`.`produto` AS `produto`,`venda`.`quantidade` AS `quantidade`,`venda`.`preco` AS `preco`,`venda`.`custo` AS `custo`,`venda`.`precooriginal` AS `precooriginal`,`venda`.`Descontoperc` AS `Descontoperc`,`venda`.`id` AS `id`,`venda`.`descontovalor` AS `descontovalor`,`venda`.`total` AS `total`,`venda`.`vendedor` AS `vendedor`,`venda`.`nrcontrole` AS `nrcontrole`,`venda`.`documento` AS `documento`,`venda`.`grupo` AS `grupo`,`venda`.`subgrupo` AS `subgrupo`,`venda`.`comissao` AS `comissao`,`venda`.`ratdesc` AS `ratdesc`,`venda`.`rateioencargos` AS `rateioencargos`,`venda`.`situacao` AS `situacao`,`venda`.`customedio` AS `customedio`,`venda`.`Ecfnumero` AS `Ecfnumero`,`venda`.`fornecedor` AS `fornecedor`,`venda`.`fabricante` AS `fabricante`,`venda`.`NotaFiscal` AS `NotaFiscal`,`venda`.`icms` AS `icms`,`venda`.`classe` AS `classe`,`venda`.`secao` AS `secao`,`venda`.`lote` AS `lote`,`venda`.`tributacao` AS `tributacao`,`venda`.`aentregar` AS `aentregar`,`venda`.`quantidadeanterior` AS `quantidadeanterior`,`venda`.`quantidadeatualizada` AS `quantidadeatualizada`,`venda`.`codigofiscal` AS `codigofiscal`,`venda`.`customedioanterior` AS `customedioanterior`,`venda`.`codigocliente` AS `codigocliente`,`venda`.`numerodevolucao` AS `numerodevolucao`,`venda`.`codigobarras` AS `codigobarras`,`venda`.`ipi` AS `ipi`,`venda`.`unidade` AS `unidade`,`venda`.`embalagem` AS `embalagem`,`venda`.`grade` AS `grade`,`venda`.`romaneio` AS `romaneio`,`venda`.`tipo` AS `tipo`,`venda`.`cofins` AS `cofins`,`venda`.`pis` AS `pis`,`venda`.`despesasacessorias` AS `despesasacessorias`,`venda`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`venda`.`serieNF` AS `serieNF`,`venda`.`subserienf` AS `subserienf`,`venda`.`cfop` AS `cfop`,`venda`.`acrescimototalitem` AS `acrescimototalitem`,`venda`.`cstpis` AS `cstpis`,`venda`.`cstcofins` AS `cstcofins`,`venda`.`icmsst` AS `icmsst`,`venda`.`percentualRedBaseCalcICMSST` AS `percentualRedBaseCalcICMSST`,`venda`.`mvast` AS `mvast`,`venda`.`modelodocfiscal` AS `modelodocfiscal`,sum(`venda`.`quantidade`) AS `somaQuantidade`,sum((`venda`.`total` - `venda`.`ratdesc`)) AS `totalItem`,sum(`venda`.`ratdesc`) AS `totalDesconto`,if((`venda`.`icms` > 0),sum(`venda`.`total`),0) AS `bcICMS`,if((`venda`.`icms` > 0),sum(((`venda`.`total` * `venda`.`icms`) / 100)),0) AS `totalICMS`,sum(`venda`.`pis`) AS `totalPIS`,sum(`venda`.`cofins`) AS `totalCOFINS` from `venda` where ((`venda`.`modelodocfiscal` = '02') or (`venda`.`modelodocfiscal` = 'D1')) group by `venda`.`codigo` union all select `vendaarquivo`.`inc` AS `inc`,`vendaarquivo`.`codigofilial` AS `codigofilial`,`vendaarquivo`.`operador` AS `operador`,`vendaarquivo`.`data` AS `data`,`vendaarquivo`.`codigo` AS `codigo`,`vendaarquivo`.`produto` AS `produto`,`vendaarquivo`.`quantidade` AS `quantidade`,`vendaarquivo`.`preco` AS `preco`,`vendaarquivo`.`custo` AS `custo`,`vendaarquivo`.`precooriginal` AS `precooriginal`,`vendaarquivo`.`Descontoperc` AS `Descontoperc`,`vendaarquivo`.`id` AS `id`,`vendaarquivo`.`descontovalor` AS `descontovalor`,`vendaarquivo`.`total` AS `total`,`vendaarquivo`.`vendedor` AS `vendedor`,`vendaarquivo`.`nrcontrole` AS `nrcontrole`,`vendaarquivo`.`documento` AS `documento`,`vendaarquivo`.`grupo` AS `grupo`,`vendaarquivo`.`subgrupo` AS `subgrupo`,`vendaarquivo`.`comissao` AS `comissao`,`vendaarquivo`.`ratdesc` AS `ratdesc`,`vendaarquivo`.`rateioencargos` AS `rateioencargos`,`vendaarquivo`.`situacao` AS `situacao`,`vendaarquivo`.`customedio` AS `customedio`,`vendaarquivo`.`ecfnumero` AS `Ecfnumero`,`vendaarquivo`.`fornecedor` AS `fornecedor`,`vendaarquivo`.`fabricante` AS `fabricante`,`vendaarquivo`.`NotaFiscal` AS `NotaFiscal`,`vendaarquivo`.`icms` AS `icms`,`vendaarquivo`.`classe` AS `classe`,`vendaarquivo`.`secao` AS `secao`,`vendaarquivo`.`lote` AS `lote`,`vendaarquivo`.`tributacao` AS `tributacao`,`vendaarquivo`.`aentregar` AS `aentregar`,`vendaarquivo`.`quantidadeanterior` AS `quantidadeanterior`,`vendaarquivo`.`quantidadeatualizada` AS `quantidadeatualizada`,`vendaarquivo`.`codigofiscal` AS `codigofiscal`,`vendaarquivo`.`customedioanterior` AS `customedioanterior`,`vendaarquivo`.`codigocliente` AS `codigocliente`,`vendaarquivo`.`numerodevolucao` AS `numerodevolucao`,`vendaarquivo`.`codigobarras` AS `codigobarras`,`vendaarquivo`.`ipi` AS `ipi`,`vendaarquivo`.`unidade` AS `unidade`,`vendaarquivo`.`embalagem` AS `embalagem`,`vendaarquivo`.`grade` AS `grade`,`vendaarquivo`.`romaneio` AS `romaneio`,`vendaarquivo`.`tipo` AS `tipo`,`vendaarquivo`.`cofins` AS `cofins`,`vendaarquivo`.`pis` AS `pis`,`vendaarquivo`.`despesasacessorias` AS `despesasacessorias`,`vendaarquivo`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`vendaarquivo`.`serieNF` AS `serieNF`,`vendaarquivo`.`subserienf` AS `subserienf`,`vendaarquivo`.`cfop` AS `cfop`,`vendaarquivo`.`acrescimototalitem` AS `acrescimototalitem`,`vendaarquivo`.`cstpis` AS `cstpis`,`vendaarquivo`.`cstcofins` AS `cstcofins`,`vendaarquivo`.`icmsst` AS `icmsst`,`vendaarquivo`.`percentualRedBaseCalcICMSST` AS `percentualRedBaseCalcICMSST`,`vendaarquivo`.`mvast` AS `mvast`,`vendaarquivo`.`modelodocfiscal` AS `modelodocfiscal`,sum(`vendaarquivo`.`quantidade`) AS `somaQuantidade`,sum((`vendaarquivo`.`total` - `vendaarquivo`.`ratdesc`)) AS `totalItem`,sum(`vendaarquivo`.`ratdesc`) AS `totalDesconto`,if((`vendaarquivo`.`icms` > 0),sum(`vendaarquivo`.`total`),0) AS `bcICMS`,if((`vendaarquivo`.`icms` > 0),sum(((`vendaarquivo`.`total` * `vendaarquivo`.`icms`) / 100)),0) AS `totalICMS`,sum(`vendaarquivo`.`pis`) AS `totalPIS`,sum(`vendaarquivo`.`cofins`) AS `totalCOFINS` from `vendaarquivo` where ((`vendaarquivo`.`modelodocfiscal` = '02') or (`vendaarquivo`.`modelodocfiscal` = 'D1')) group by `vendaarquivo`.`codigo` */;

/*View structure for view blococregc390 */

/*!50001 DROP TABLE IF EXISTS `blococregc390` */;
/*!50001 DROP VIEW IF EXISTS `blococregc390` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc390` AS select `venda`.`documento` AS `documento`,`venda`.`data` AS `data`,`venda`.`icms` AS `icms`,`venda`.`cfop` AS `cfop`,`venda`.`tributacao` AS `tributacao`,truncate(sum((`venda`.`total` - `venda`.`ratdesc`)),2) AS `total`,if((`venda`.`icms` > 0),truncate(sum((`venda`.`total` - `venda`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(`venda`.`total`) * `venda`.`icms`) / 100),2) AS `totalICMS` from `venda` where (`venda`.`quantidade` > 0) group by `venda`.`tributacao`,`venda`.`cfop`,`venda`.`icms`,`venda`.`documento` union all select `vendaarquivo`.`documento` AS `documento`,`vendaarquivo`.`data` AS `data`,`vendaarquivo`.`icms` AS `icms`,`vendaarquivo`.`cfop` AS `cfop`,`vendaarquivo`.`tributacao` AS `tributacao`,truncate(sum((`vendaarquivo`.`total` - `vendaarquivo`.`ratdesc`)),2) AS `total`,if((`vendaarquivo`.`icms` > 0),truncate(sum((`vendaarquivo`.`total` - `vendaarquivo`.`ratdesc`)),2),0) AS `baseCalculoICMS`,truncate(((sum(`vendaarquivo`.`total`) * `vendaarquivo`.`icms`) / 100),2) AS `totalICMS` from `vendaarquivo` where (`vendaarquivo`.`quantidade` > 0) group by `vendaarquivo`.`tributacao`,`vendaarquivo`.`cfop`,`vendaarquivo`.`icms`,`vendaarquivo`.`documento` */;

/*View structure for view blococregc425 */

/*!50001 DROP TABLE IF EXISTS `blococregc425` */;
/*!50001 DROP VIEW IF EXISTS `blococregc425` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc425` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`vendaarquivo`.`codigo` AS `codigo`,`vendaarquivo`.`produto` AS `descricao`,sum(truncate(`vendaarquivo`.`quantidade`,3)) AS `quantidade`,`vendaarquivo`.`unidade` AS `unidade`,`vendaarquivo`.`preco` AS `preco`,`vendaarquivo`.`descontovalor` AS `descontovalor`,`vendaarquivo`.`Descontoperc` AS `descontoperc`,truncate(sum((`vendaarquivo`.`total` - `vendaarquivo`.`ratdesc`)),2) AS `total`,`vendaarquivo`.`icms` AS `icms`,`vendaarquivo`.`tributacao` AS `tributacao`,`vendaarquivo`.`cfop` AS `cfop`,`vendaarquivo`.`cancelado` AS `cancelado` from (`contdocs` join `vendaarquivo`) where ((`contdocs`.`documento` = `vendaarquivo`.`documento`) and (`vendaarquivo`.`quantidade` > 0)) group by `vendaarquivo`.`codigo` union all select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`venda`.`codigo` AS `codigo`,`venda`.`produto` AS `descricao`,sum(truncate(`venda`.`quantidade`,3)) AS `quantidade`,`venda`.`unidade` AS `unidade`,`venda`.`preco` AS `preco`,`venda`.`descontovalor` AS `descontovalor`,`venda`.`Descontoperc` AS `descontoperc`,truncate(sum((`venda`.`total` - `venda`.`ratdesc`)),2) AS `total`,`venda`.`icms` AS `icms`,`venda`.`tributacao` AS `tributacao`,`venda`.`cfop` AS `cfop`,`venda`.`cancelado` AS `cancelado` from (`contdocs` join `venda`) where ((`contdocs`.`documento` = `venda`.`documento`) and (`venda`.`quantidade` > 0)) group by `venda`.`codigo` order by `descricao` */;

/*View structure for view blococregc470 */

/*!50001 DROP TABLE IF EXISTS `blococregc470` */;
/*!50001 DROP VIEW IF EXISTS `blococregc470` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc470` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`vendaarquivo`.`codigo` AS `codigo`,`vendaarquivo`.`produto` AS `descricao`,truncate(`vendaarquivo`.`quantidade`,3) AS `quantidade`,`vendaarquivo`.`unidade` AS `unidade`,`vendaarquivo`.`preco` AS `preco`,`vendaarquivo`.`descontovalor` AS `descontovalor`,`vendaarquivo`.`Descontoperc` AS `descontoperc`,truncate((`vendaarquivo`.`total` - `vendaarquivo`.`ratdesc`),2) AS `total`,`vendaarquivo`.`icms` AS `icms`,`vendaarquivo`.`tributacao` AS `tributacao`,`vendaarquivo`.`cfop` AS `cfop` from (`contdocs` join `vendaarquivo`) where ((`contdocs`.`documento` = `vendaarquivo`.`documento`) and (`contdocs`.`estornado` = 'N') and (`vendaarquivo`.`quantidade` > 0)) order by `vendaarquivo`.`nrcontrole` */;

/*View structure for view blococregc490 */

/*!50001 DROP TABLE IF EXISTS `blococregc490` */;
/*!50001 DROP VIEW IF EXISTS `blococregc490` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `blococregc490` AS select `contdocs`.`documento` AS `documento`,`contdocs`.`data` AS `data`,`contdocs`.`ecffabricacao` AS `ecffabricacao`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`modeloDOCFiscal` AS `modelodocfiscal`,`vendaarquivo`.`codigo` AS `codigo`,`vendaarquivo`.`produto` AS `descricao`,truncate(`vendaarquivo`.`quantidade`,3) AS `quantidade`,`vendaarquivo`.`unidade` AS `unidade`,`vendaarquivo`.`preco` AS `preco`,`vendaarquivo`.`descontovalor` AS `descontovalor`,`vendaarquivo`.`Descontoperc` AS `descontoperc`,truncate(sum((`vendaarquivo`.`total` - `vendaarquivo`.`ratdesc`)),2) AS `total`,if((`vendaarquivo`.`icms` > 0),truncate(sum((`vendaarquivo`.`total` - `vendaarquivo`.`ratdesc`)),0),2) AS `baseCalculoICMS`,truncate(((sum(`vendaarquivo`.`total`) * `vendaarquivo`.`icms`) / 100),2) AS `totalICMS`,`vendaarquivo`.`icms` AS `icms`,`vendaarquivo`.`tributacao` AS `tributacao`,`vendaarquivo`.`cfop` AS `cfop` from (`contdocs` join `vendaarquivo`) where ((`contdocs`.`documento` = `vendaarquivo`.`documento`) and (`vendaarquivo`.`quantidade` > 0) and (`contdocs`.`estornado` = 'N')) group by `vendaarquivo`.`tributacao`,`vendaarquivo`.`cfop`,`vendaarquivo`.`icms` */;

/*View structure for view r05 */

/*!50001 DROP TABLE IF EXISTS `r05` */;
/*!50001 DROP VIEW IF EXISTS `r05` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `r05` AS select `contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`ecfcontadorcupomfiscal` AS `ecfcontadorcupomfiscal`,`contdocs`.`data` AS `data`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,`venda`.`nrcontrole` AS `nrcontrole`,`venda`.`codigo` AS `codigo`,`venda`.`produto` AS `produto`,`venda`.`quantidade` AS `quantidade`,`venda`.`unidade` AS `unidade`,`venda`.`precooriginal` AS `precooriginal`,`venda`.`descontovalor` AS `descontovalor`,`venda`.`preco` AS `preco`,`venda`.`total` AS `total`,`venda`.`icms` AS `icms`,`venda`.`tributacao` AS `tributacao`,`produtos`.`indicadorproducao` AS `indicadorproducao`,`produtos`.`indicadorarredondamentotruncamento` AS `indicadorarredondamentotruncamento` from ((`contdocs` join `venda`) join `produtos`) where ((`venda`.`documento` = `contdocs`.`documento`) and (`produtos`.`codigo` = `venda`.`codigo`)) union all select `contdocs`.`ncupomfiscal` AS `ncupomfiscal`,`contdocs`.`ecfcontadorcupomfiscal` AS `ecfcontadorcupomfiscal`,`contdocs`.`data` AS `data`,`contdocs`.`ecfnumero` AS `ecfnumero`,`contdocs`.`estornado` AS `estornado`,`contdocs`.`dpfinanceiro` AS `dpfinanceiro`,`vendaarquivo`.`nrcontrole` AS `nrcontrole`,`vendaarquivo`.`codigo` AS `codigo`,`vendaarquivo`.`produto` AS `produto`,`vendaarquivo`.`quantidade` AS `quantidade`,`vendaarquivo`.`unidade` AS `unidade`,`vendaarquivo`.`precooriginal` AS `precooriginal`,`vendaarquivo`.`descontovalor` AS `descontovalor`,`vendaarquivo`.`preco` AS `preco`,`vendaarquivo`.`total` AS `total`,`vendaarquivo`.`icms` AS `icms`,`vendaarquivo`.`tributacao` AS `tributacao`,`produtos`.`indicadorproducao` AS `indicadorproducao`,`produtos`.`indicadorarredondamentotruncamento` AS `indicadorarredondamentotruncamento` from ((`contdocs` join `vendaarquivo`) join `produtos`) where ((`vendaarquivo`.`documento` = `contdocs`.`documento`) and (`produtos`.`codigo` = `vendaarquivo`.`codigo`) and (`contdocs`.`documento` <> '') and (`produtos`.`CodigoFilial` = `contdocs`.`CodigoFilial`) and (`vendaarquivo`.`ecfnumero` <> '')) */;

/*View structure for view registro50entradas_agr */

/*!50001 DROP TABLE IF EXISTS `registro50entradas_agr` */;
/*!50001 DROP VIEW IF EXISTS `registro50entradas_agr` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50entradas_agr` AS select `entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`bcicms`) AS `bcicms`,round(sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),2) AS `toticms`,sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) AS `ipiItem`,sum(`entradas`.`totalitem`) AS `totalProduto`,(sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`totalitem`)) AS `totalNF`,`entradas`.`Lancada` AS `lancada` from `entradas` group by `entradas`.`NF`,`entradas`.`cfopentrada`,`entradas`.`IcmsEntrada`,`entradas`.`numero` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view registro50entradas_itens */

/*!50001 DROP TABLE IF EXISTS `registro50entradas_itens` */;
/*!50001 DROP VIEW IF EXISTS `registro50entradas_itens` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50entradas_itens` AS select `entradas`.`codigofilial` AS `codigofilial`,`entradas`.`numero` AS `numero`,`entradas`.`NF` AS `nf`,`entradas`.`modeloNF` AS `modelonf`,`entradas`.`dataentrada` AS `dataentrada`,`entradas`.`cfopentrada` AS `cfopentrada`,`entradas`.`IcmsEntrada` AS `icmsentrada`,`entradas`.`tributacao` AS `tributacao`,`entradas`.`codigo` AS `codigo`,`entradas`.`descricao` AS `descricao`,`entradas`.`sequencia` AS `sequencia`,sum(`entradas`.`quantidade`) AS `quantidade`,`entradas`.`unidade` AS `unidade`,`entradas`.`Custo` AS `custo`,sum(`entradas`.`bcicms`) AS `bcicms`,round(sum((`entradas`.`bcicms` * (`entradas`.`IcmsEntrada` / 100))),2) AS `toticms`,sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) AS `ipiItem`,sum(`entradas`.`totalitem`) AS `totalProduto`,(sum(round((`entradas`.`totalitem` * (`entradas`.`IPI` / 100)),2)) + sum(`entradas`.`totalitem`)) AS `totalNF` from `entradas` group by `entradas`.`NF`,`entradas`.`inc` order by `entradas`.`NF`,`entradas`.`sequencia` */;

/*View structure for view registro50saida_agr */

/*!50001 DROP TABLE IF EXISTS `registro50saida_agr` */;
/*!50001 DROP VIEW IF EXISTS `registro50saida_agr` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50saida_agr` AS select `vendanf`.`codigofilial` AS `codigofilial`,`vendanf`.`NotaFiscal` AS `notafiscal`,`vendanf`.`serieNF` AS `serienf`,`vendanf`.`modelodocfiscal` AS `modelodocfiscal`,`vendanf`.`documento` AS `documento`,`vendanf`.`data` AS `DATA`,`vendanf`.`cfop` AS `cfop`,`vendanf`.`icms` AS `icms`,`vendanf`.`tributacao` AS `tributacao`,`vendanf`.`codigo` AS `codigo`,`vendanf`.`produto` AS `produto`,sum(`vendanf`.`quantidade`) AS `SUM(quantidade)`,`vendanf`.`unidade` AS `unidade`,`vendanf`.`nrcontrole` AS `nrcontrole`,sum(round((((`vendanf`.`total` - ((`vendanf`.`total` * `vendanf`.`percentualRedBaseCalcICMS`) / 100)) * `vendanf`.`icms`) / 100),2)) AS `totalicms`,sum(`vendanf`.`descontovalor`) AS `descontovalor`,sum(`vendanf`.`total`) AS `SUM(TOTAL)`,if((`vendanf`.`icms` > 0),sum(round((`vendanf`.`total` - (`vendanf`.`total` * (`vendanf`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendanf` group by `vendanf`.`NotaFiscal`,`vendanf`.`serieNF`,`vendanf`.`icms`,`vendanf`.`cfop`,`vendanf`.`codigofilial` */;

/*View structure for view registro50saidas_itens */

/*!50001 DROP TABLE IF EXISTS `registro50saidas_itens` */;
/*!50001 DROP VIEW IF EXISTS `registro50saidas_itens` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `registro50saidas_itens` AS select `vendanf`.`inc` AS `inc`,`vendanf`.`codigofilial` AS `codigofilial`,`vendanf`.`NotaFiscal` AS `notafiscal`,`vendanf`.`serieNF` AS `serienf`,`vendanf`.`modelodocfiscal` AS `modelodocfiscal`,`vendanf`.`documento` AS `documento`,`vendanf`.`data` AS `DATA`,`vendanf`.`cfop` AS `cfop`,`vendanf`.`icms` AS `icms`,`vendanf`.`tributacao` AS `tributacao`,`vendanf`.`codigo` AS `codigo`,`vendanf`.`produto` AS `produto`,sum(`vendanf`.`quantidade`) AS `SUM(quantidade)`,`vendanf`.`unidade` AS `unidade`,`vendanf`.`nrcontrole` AS `nrcontrole`,sum(round((((`vendanf`.`total` - ((`vendanf`.`total` * `vendanf`.`percentualRedBaseCalcICMS`) / 100)) * `vendanf`.`icms`) / 100),2)) AS `totalicms`,sum(`vendanf`.`descontovalor`) AS `descontovalor`,sum(`vendanf`.`total`) AS `SUM(TOTAL)`,if((`vendanf`.`icms` > 0),sum(round((`vendanf`.`total` - (`vendanf`.`total` * (`vendanf`.`percentualRedBaseCalcICMS` / 100))),2)),0) AS `baseCalculoICMS` from `vendanf` group by `vendanf`.`inc` order by `vendanf`.`NotaFiscal`,`vendanf`.`nrcontrole` */;

/*View structure for view vprodutos */

/*!50001 DROP TABLE IF EXISTS `vprodutos` */;
/*!50001 DROP VIEW IF EXISTS `vprodutos` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vprodutos` AS select `produtos`.`CodigoFilial` AS `codigofilial`,`produtos`.`codigo` AS `codigo`,`produtos`.`codigobarras` AS `codigobarras`,`produtos`.`descricao` AS `descricao`,`produtos`.`marcado` AS `marcado`,`produtos`.`quantidade` AS `quantidade`,`produtos`.`precovenda` AS `precovenda`,`produtos`.`custo` AS `custo`,`produtos`.`situacao` AS `situacao`,`produtos`.`qtdprateleiras` AS `qtdprateleiras`,`produtos`.`unidade` AS `unidade`,`produtos`.`icms` AS `icms`,`produtos`.`descontopromocao` AS `descontopromocao`,`produtos`.`descontomaximo` AS `descontomaximo`,`produtos`.`qtdprovisoria` AS `qtdprovisoria`,`produtos`.`qtdretida` AS `qtdretida`,`produtos`.`classe` AS `classe`,`produtos`.`tributacao` AS `tributacao`,if((`produtos`.`qtdprateleiras` > 0),(`produtos`.`quantidade` - `produtos`.`qtdprateleiras`),(`produtos`.`quantidade` - `produtos`.`qtdprateleiras`)) AS `deposito`,`produtos`.`qtdvencidos` AS `qtdvencidos`,`produtos`.`qtdaentregar` AS `qtdaentregar`,`produtos`.`codigofiscal` AS `codigofiscal`,`produtos`.`ativacompdesc` AS `ativacompdesc`,`produtos`.`qtdminimadesc` AS `qtdminimadesc`,`produtos`.`qtdprevenda` AS `qtdprevenda`,if((`produtos`.`qtdprevenda` > 0),(`produtos`.`quantidade` - `produtos`.`qtdprevenda`),`produtos`.`quantidade`) AS `disponivel`,if((`produtos`.`descontomaximo` > 0),((`produtos`.`precovenda` * (-(`produtos`.`descontomaximo`) / 100)) + `produtos`.`precovenda`),((`produtos`.`precovenda` * (-((select `configfinanc`.`fatmaiordesvenda` AS `fatmaiordesvenda` from `configfinanc` where (`configfinanc`.`CodigoFilial` = `produtos`.`CodigoFilial`) limit 1)) / 100)) + `produtos`.`precovenda`)) AS `precominimo`,`produtos`.`ipi` AS `ipi`,`produtos`.`precosemfinanciamento` AS `precosemfinanciamento`,`produtos`.`precoatacado` AS `precoatacado`,`produtos`.`embalagem` AS `embalagem`,`produtos`.`grade` AS `grade`,`produtos`.`volumes` AS `volumes`,`produtos`.`tipo` AS `tipo`,`produtos`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`produtos`.`unidembalagem` AS `unidembalagem`,`produtos`.`lote` AS `lote`,`produtos`.`aceitadesconto` AS `aceitadesconto` from `produtos` */;

/*View structure for view vprodutosfilial */

/*!50001 DROP TABLE IF EXISTS `vprodutosfilial` */;
/*!50001 DROP VIEW IF EXISTS `vprodutosfilial` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vprodutosfilial` AS select `produtosfilial`.`CodigoFilial` AS `codigofilial`,`produtosfilial`.`codigo` AS `codigo`,`produtosfilial`.`codigobarras` AS `codigobarras`,`produtosfilial`.`descricao` AS `descricao`,`produtosfilial`.`marcado` AS `marcado`,`produtosfilial`.`quantidade` AS `quantidade`,`produtosfilial`.`precovenda` AS `precovenda`,`produtosfilial`.`custo` AS `custo`,`produtosfilial`.`situacao` AS `situacao`,`produtosfilial`.`qtdprateleiras` AS `qtdprateleiras`,`produtosfilial`.`unidade` AS `unidade`,`produtosfilial`.`icms` AS `icms`,`produtosfilial`.`descontopromocao` AS `descontopromocao`,`produtosfilial`.`descontomaximo` AS `descontomaximo`,`produtosfilial`.`qtdprovisoria` AS `qtdprovisoria`,`produtosfilial`.`qtdretida` AS `qtdretida`,`produtosfilial`.`classe` AS `classe`,`produtosfilial`.`tributacao` AS `tributacao`,if((`produtosfilial`.`qtdprateleiras` > 0),(`produtosfilial`.`quantidade` - `produtosfilial`.`qtdprateleiras`),(`produtosfilial`.`quantidade` - `produtosfilial`.`qtdprateleiras`)) AS `deposito`,`produtosfilial`.`qtdvencidos` AS `qtdvencidos`,`produtosfilial`.`qtdaentregar` AS `qtdaentregar`,`produtosfilial`.`codigofiscal` AS `codigofiscal`,`produtosfilial`.`ativacompdesc` AS `ativacompdesc`,`produtosfilial`.`qtdminimadesc` AS `qtdminimadesc`,`produtosfilial`.`qtdprevenda` AS `qtdprevenda`,if((`produtosfilial`.`qtdprevenda` > 0),(`produtosfilial`.`quantidade` - `produtosfilial`.`qtdprevenda`),`produtosfilial`.`quantidade`) AS `disponivel`,if((`produtosfilial`.`descontomaximo` > 0),((`produtosfilial`.`precovenda` * (-(`produtosfilial`.`descontomaximo`) / 100)) + `produtosfilial`.`precovenda`),((`produtosfilial`.`precovenda` * (-((select `configfinanc`.`fatmaiordesvenda` AS `fatmaiordesvenda` from `configfinanc` where (`configfinanc`.`CodigoFilial` = `produtosfilial`.`CodigoFilial`) limit 1)) / 100)) + `produtosfilial`.`precovenda`)) AS `precominimo`,`produtosfilial`.`ipi` AS `ipi`,`produtosfilial`.`precosemfinanciamento` AS `precosemfinanciamento`,`produtosfilial`.`precoatacado` AS `precoatacado`,`produtosfilial`.`embalagem` AS `embalagem`,`produtosfilial`.`grade` AS `grade`,`produtosfilial`.`volumes` AS `volumes`,`produtosfilial`.`tipo` AS `tipo`,`produtosfilial`.`percentualRedBaseCalcICMS` AS `percentualRedBaseCalcICMS`,`produtosfilial`.`unidembalagem` AS `unidembalagem`,`produtosfilial`.`lote` AS `lote`,`produtosfilial`.`aceitadesconto` AS `aceitadesconto` from `produtosfilial` */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

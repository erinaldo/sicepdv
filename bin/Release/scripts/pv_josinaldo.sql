-- evento que dispara procedure para gravar informações no banco  de dados

DELIMITER $$

DROP EVENT IF EXISTS  `e_analiseGerencial`$$

CREATE DEFINER=`root`@`localhost` EVENT `e_analiseGerencial` ON SCHEDULE EVERY 1 HOUR STARTS NOW() ON COMPLETION PRESERVE ENABLE DO BEGIN 
   -- declara variaveis e cursores
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
    
  END$$

DELIMITER ;


-- procedure que grava todas as informações

DELIMITER $$

DROP PROCEDURE IF EXISTS `sp_gravaInfoGerencial`$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_gravaInfoGerencial`(IN filial VARCHAR(5))
BEGIN
 
 DECLARE mov VARCHAR(10) ;
 DECLARE v_dia, v_dia2 DECIMAL (10,2) DEFAULT 0 ; 
 DECLARE v_receber, v_recebido DECIMAL (12,2) DEFAULT 0;
 -- verifica movimento aberto
 SET mov = (SELECT inc FROM movimento WHERE codigofilial = filial AND DATA = CURRENT_DATE AND finalizado = '');
 
 IF (mov<> "") THEN -- se movimento aberto executa
	-- vendas do dia
	
	SET v_dia = (SELECT  IFNULL(SUM(ABS(quantidade)*precooriginal-ratdesc),0 )AS t_vendas FROM venda	WHERE DATA = CURRENT_DATE AND cancelado = 'N' AND codigofilial = filial); 
	SET v_dia2 = (SELECT IFNULL(SUM(vendas), 0) FROM caixassoma WHERE DATA = CURRENT_DATE AND codigofilial = filial);
	
	-- acompanhamento de valores a receber e recebidos
	 SET v_receber =(SELECT IFNULL(SUM(valor),0) AS tot_rec FROM crmovclientes WHERE vencimento = CURRENT_DATE AND codigofilial = filial );
	 SET v_recebido = (SELECT IFNULL(SUM(vrultpagamento),0) AS  tot_recebido FROM crmovclientes WHERE vencimento = CURRENT_DATE AND datapagamento = CURRENT_DATE AND codigofilial = filial);
		INSERT INTO assistentegerencial( DATA, codigofilial, hora, ocorrenciasauditoria, ticketmedio, totalpagar,percinadimplenciacrediario, vendadiaria, auditoriacliente, auditoriaacessos, auditoriaprodutos, auditoriavendas, auditoriacontaspagar, auditoriaestorno, totalreceber, totalrecebido )
		VALUES (CURRENT_DATE, filial, CURRENT_TIME, f_totalOcorrenciasAuditoria(filial, 'tot'), f_ticketMedio(filial), '0','0', v_dia+ v_dia2  , f_totalOcorrenciasAuditoria(filial, 'cli'), f_totalOcorrenciasAuditoria(filial, 'ace'), f_totalOcorrenciasAuditoria(filial, 'pro'), f_totalOcorrenciasAuditoria(filial, 'ven'),
		f_totalOcorrenciasAuditoria(filial, 'pag'), f_totalOcorrenciasAuditoria(filial, 'est') , v_receber, v_recebido);
END IF;	
 
    END$$

DELIMITER ;
-- funcao que conta o total de ocorrencias do dia
DELIMITER $$

DROP FUNCTION IF EXISTS `f_totalOcorrenciasAuditoria`$$

CREATE DEFINER=`root`@`localhost` FUNCTION `f_totalOcorrenciasAuditoria`( filial VARCHAR(5), tipo VARCHAR(3)  ) RETURNS INT(11)
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
 
    END$$

DELIMITER ;


-- funcao de ticket medio

DELIMITER $$

DROP FUNCTION IF EXISTS `f_ticketMedio`$$

CREATE DEFINER=`root`@`localhost` FUNCTION `f_ticketMedio`( filial VARCHAR(5) ) RETURNS DOUBLE
BEGIN
	
	DECLARE X INT DEFAULT 0;
	DECLARE Y DOUBLE DEFAULT 0;
	
	SET Y = (SELECT (SUM(total) - SUM(ratdesc)) FROM venda WHERE DATA = CURRENT_DATE AND codigofilial = filial AND cancelado = "N");
	SET X = (SELECT COUNT(1)  FROM caixa WHERE dpfinanceiro IN ("venda", "crediario") AND codigofilial = filial AND DATA = CURRENT_DATE );
	RETURN  Y / X ;
	
    END$$

DELIMITER ;




-- view que visualiza na tela do sistema as últimas informações do dia

DELIMITER $$

DROP VIEW IF EXISTS `v_assistentegerencial`$$

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_assistentegerencial` AS (
SELECT
  `assistentegerencial`.`codigofilial`         AS `codigofilial`,
  `assistentegerencial`.`ticketmedio`          AS `ticketmedio`,
  `assistentegerencial`.`data`                 AS `data`,
  `assistentegerencial`.`hora`                 AS `hora`,
  `assistentegerencial`.`ocorrenciasauditoria` AS `ocorrenciasauditoria`,
`assistentegerencial`.`auditoriacliente` AS `auditoriacliente`,
`assistentegerencial`.`auditoriaacessos` AS `auditoriaacessos`,
`assistentegerencial`.`auditoriaprodutos` AS `auditoriaprodutos`,
`assistentegerencial`.`auditoriavendas` AS `auditoriavendas`,
`assistentegerencial`.`auditoriacontaspagar` AS `auditoriacontaspagar`,
`assistentegerencial`.`auditoriaestorno` AS `auditoriaestorno`,
`assistentegerencial`.`vendadiaria`          AS `vendadiaria`,
`assistentegerencial`.`totalreceber`     AS `totalreceber`,
`assistentegerencial`.`totalrecebido`     AS `totalrecebido`
FROM `assistentegerencial`
WHERE (`assistentegerencial`.`data` = CURDATE()))$$

DELIMITER ;
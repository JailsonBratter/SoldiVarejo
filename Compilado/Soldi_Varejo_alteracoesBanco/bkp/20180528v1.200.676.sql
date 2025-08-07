

/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_FluxoCaixa]    Script Date: 05/28/2018 15:53:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_FluxoCaixa]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa]
GO


/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_FluxoCaixa]    Script Date: 05/28/2018 15:53:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--PROCEDURES =======================================================================================

-- exec sp_Rel_Fin_FluxoCaixa 'MATRIZ','20160101','20160131'
CREATE  PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8)
AS

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Totais]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)

Begin

Drop table Totais

End
	
	Create table Totais
(
	Descricao  varchar(400),
	Total	 Decimal(18,2)
);
	
	
	select count(*) as Qtde into #cupons from saida_Estoque WITH(INDEX(Ix_Fluxo_Caixa)) where 
		Filial  = @Filial And
		Data_Movimento BETWEEN @DataDe AND @DataAte AND 
		Data_Cancelamento IS NULL
	group by documento, filial, caixa_saida
	
	insert into #cupons 
	select COUNT(*) from nf where nf.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf.Filial = @FILIAL and nf.status='AUTORIZADO'
	group by Codigo
	
	insert into #cupons
	select COUNT(*) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte  and pedido.Status <>3
	GROUP by pedido
	
	----# Insere Todo Faturamento em uma tabela temp (Para fazer calculo) #------
	Insert into Totais
	SELECT 'TOTAL FATURAMENTO' AS Descricao,CONVERT(VARCHAR,
					Isnull((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE Filial = @FILIAL and  Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL 
					),0)
					+Isnull((SELECT Sum(isnull(convert(decimal(10,2),nf_item.Total),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo AND nf_item.Filial = nf.FILIAL where  nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf_item.Filial = @FILIAL and nf.status='AUTORIZADO' ),0) 
					+Isnull((SELECT Sum(isnull(Total,0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte),0)
					)  Total

	----# Insere Todas VENDAS CANCELADAS do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'VENDAS CANCELADAS', CONVERT(VARCHAR,ISNULL((SELECT SUM(VLR-isnull(desconto,0)) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  FILIAL = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte  AND Data_Cancelamento IS NOT NULL),0))  				
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'ITENS CANCELADOS PEDIDO', CONVERt(VARCHAR,ISNULL(Sum(isnull(Total,0)),0)) from pedido with(index(ix_pedido_fluxo_caixa)) where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte And Pedido.Status in (3)

	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'AJUSTE DEVOLUCAO', isnull(CONVERt(VARCHAR,Convert(Decimal(18,2),Sum(isnull(Contada*Custo,0)))),0) from Inventario_itens itens inner join Inventario i on i.Codigo_inventario =  itens.Codigo_inventario 
		where Data BETWEEN @DataDe AND @DataAte  and status = 'ENCERRADO' And tipoMovimentacao = 'DEVOLUCAO'		
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'NF DEVOLUCAO', CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(10,2),nf.Total),0)),0)) from nf Where nf.codigo_operacao in ('1202', '5202', '5411', '6202') and nf.Tipo_NF=2 and Isnull(nf.nf_Canc,0) = 0 and nf.Emissao between @dataDe and @DataAte		
		
	SELECT '' As Descritivo,'' As Total
	UNION ALL
	SELECT 'VENDAS PEDIDOS SIMPLIFICADOS', replace(CONVERt(VARCHAR,Sum(convert(decimal(18,2),isnull(Total,0),0))),'.',',') from pedido with(index(ix_pedido_fluxo_caixa)) where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte and pedido.Status <>3 
	UNION ALL
	SELECT 'VENDAS NOTA FISCAL', replace(CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(18,2),nf_item.Total),0)-(isnull(NF_Item.Total,0)*isnull(nf_item.desconto,0)/100)),0)),'.',',') from nf_item inner join nf on nf_item.codigo= nf.Codigo  AND nf_item.Filial = nf.FILIAL where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf_item.Filial = @FILIAL and nf.status='AUTORIZADO'
	UNION ALL
	SELECT 'VENDAS CUPOM',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE FILIAL = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL 
			AND Filial = @FILIAL),0)),'.',',')
	UNION ALL
	SELECT '|-SUB-|'+Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'TOTAL FATURAMENTO'
					 
	UNION ALL
				
		SELECT '',''
	UNION ALL
	SELECT 'NUMERO DE CLIENTES', Convert(varchar, (select count(*) from #Cupons))		
	UNION ALL
	SELECT 'VENDAS COM NFP', REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where Filial = @filial and data_movimento between @DataDe and @DataAte and data_cancelamento is null  and len(isnull(cpf_cnpj,''))>10))),'.',',')
	UNION ALL
	SELECT 'PORC DE VENDAS COM NFP',replace(CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where filial = @filial and data_movimento between @DataDe and @DataAte and data_cancelamento is null  and len(isnull(cpf_cnpj,''))>10) / (Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null)*100)),'.',',')
	UNION ALL
	SELECT 'QTDE ITENS VENDIDOS',REPLACE( CONVERT(VARCHAR,CONVERt(Int,ISNULL((SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  Filial = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL ),0))),'.',',')
	UNION ALL
	SELECT 'QTDE ITENS CANCELADOS VENDA', replace(CONVERT(VARCHAR,ISNULL(convert(Numeric(18),(SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  Filial = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NOT NULL )),0)),'.',',')	
	UNION ALL
	SELECT '',''
	
	UNION ALL
	SELECT Descricao,replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'VENDAS CANCELADAS'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'ITENS CANCELADOS PEDIDO'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'AJUSTE DEVOLUCAO'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'NF DEVOLUCAO'

	--UNION ALL
	
	--SELECT 'RESULTADO TOTAL',
	--	replace(CONVERT(VARCHAR(90),Convert(Decimal(18,2),SUM(Total) - (Select SUM(Total) From TOTAIS  Where Descricao = 'VENDAS CANCELADAS') - (Select SUM(Total) From TOTAIS  Where Descricao = 'ITENS CANCELADOS PEDIDO') - (Select SUM(Total) From TOTAIS  Where Descricao = 'AJUSTE DEVOLUCAO') - (Select Sum(Total) From TOTAIS Where Descricao = 'NF DEVOLUCAO'))),'.',',')
	--FROM  TOTAIS
	--Where Descricao = 'TOTAL FATURAMENTO'

	UNION ALL
	SELECT '',''
	UNION ALL
	SELECT 'DESCONTOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(desconto,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial))),'.',',')
	UNION ALL
	SELECT 'ACRESCIMOS SERVICOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(Acrescimo,0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where filial = @filial and  data_movimento between @DataDe and @DataAte and data_cancelamento is null ))),'.',',')
	UNION ALL
	SELECT 'INDUSTRIA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES EMITIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES RECEBIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS EMITIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS RECEBIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'PAGAMENTO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'ESTORNO DE DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
    SELECT 'GERENCIAL ',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE Filial = @FILIAL AND Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND CONVERT(NUMERIC, ISNULL(COO,0)) <= 0 ),0)),'.',',')
	UNION ALL
	SELECT 'REPIQUE', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT '',''
	UNION ALL
	
	SELECT 'Forma de Pagamento |-SUB-|','Valor Total'
	UNION ALL
	SELECT FINALIZADORA.FINALIZADORA,replace(CONVERt(VARCHAR,SUM(TOTAL)),'.',',')
		FROM LISTA_FINALIZADORA  WITH(INDEX(ix_Lista_Fluxo_Caixa))
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO between @dataDe and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL
		GROUP BY FINALIZADORA.FINALIZADORA
	UNION ALL	
	SELECT 'Valor Total',replace(CONVERt(VARCHAR,SUM(ISNULL(TOTAL,0))),'.',',')
		FROM LISTA_FINALIZADORA WITH(INDEX(ix_Lista_Fluxo_Caixa))
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO  between @datade and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL 
	UNION ALL
	SELECT '',''
	  





GO



/****** Object:  StoredProcedure [dbo].[sp_Movimento_Venda]    Script Date: 05/28/2018 16:20:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Movimento_Venda]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Movimento_Venda]
GO


/****** Object:  StoredProcedure [dbo].[sp_Movimento_Venda]    Script Date: 05/28/2018 16:20:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--PROCEDURES =======================================================================================
CREATE Procedure [dbo].[sp_Movimento_Venda]

                @Filial				As Varchar(20),
                @DataDe				As Varchar(8),
                @DataAte			As Varchar(8),
                @finalizadora		As varchar(30),
                @plu				As varchar(17),
                @cupom				As varchar(20),
                @pdv				as varchar(10),                
                @horaInicio			as varchar(5),				
				@horafim			as varchar(5),
				@cancelados			as varchar(5),
				@comanda			as varchar(20),
				@ext_sat			as varchar(6)

AS

begin 

IF(@plu='' AND @cupom='' and @ext_sat ='')

      BEGIN

            IF(@finalizadora ='TODOS')

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,lista.EMISSAO,103),

                             lista.PDV,

                             CUPOM = lista.DOCUMENTO,
                             [Ext SAT] = SUBSTRING(S.ID_CHAVE,35,6),
                             CONVERT(varchar , s.Hora_venda,108) as HORA,
                             
							[COMANDA/PEDIDOS] =  '_'+(SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st  with (index(IX_Movimento_venda_01))

								WHERE st.Filial = @FILIAL And st.data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento),
                        
                             
                             FINALIZADORA = lista.id_finalizadora,
							
						     VLR = (SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) 
										FROM Lista_finalizadora list1
				                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 
								  			 --INNER JOIN  Saida_estoque S  with (index(IX_Movimento_venda_01)) ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv and CONVERT(varchar , s.Hora_venda,108) between @horaInicio and @horafim
									WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
											 AND (list1.Emissao = lista.Emissao)
											 and list1.pdv =lista.pdv
											 and list1.documento = lista.documento
											 AND LIST1.id_finalizadora = LISTA.id_finalizadora
                         ),
                             CANCELADOS = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st  with (index(IX_Movimento_venda_01)) 

								WHERE st.Filial = @FILIAL And data_cancelamento is not null 
								and CONVERT(varchar , st.Hora_venda,108) between @horaInicio and @horafim 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento)FROM
                             Lista_finalizadora lista
                             INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 
							INNER JOIN  Saida_estoque S  with (index(IX_Movimento_venda_01))  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv	and s.Data_movimento = lista.Emissao
                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )
								  and CONVERT(varchar , s.Hora_venda,108) between @horaInicio and @horafim 
                                   and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
								   and (LEN(@cancelados)=0 OR
										
								   (@cancelados ='TODOS' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='ITEM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=0 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0) 
										OR (@cancelados='CUPOM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=1 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0)												   
																						   
																						   ) 
																						   
						and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')																   
                                   
						GROUP BY lista.Emissao, lista.pdv, lista.Documento ,lista.id_finalizadora, CONVERT(varchar , s.Hora_venda,108),SUBSTRING(S.ID_CHAVE,35,6) 
						

                  END

            ELSE

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = lista.DOCUMENTO,
                             [Ext SAT] = SUBSTRING(S.ID_CHAVE,35,6),
                             CONVERT(varchar , Hora_venda,108) AS HORA,

                             [COMANDA/PEDIDOS] = '_'+(SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento),
                                   VLR =(SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) FROM Lista_finalizadora list1

                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
									AND (list1.Emissao = lista.Emissao)
                                   and list1.pdv =lista.pdv
                                   and list1.documento = lista.documento
                                   
                         ),        

                             FINALIZADORA = id_finalizadora,
                             
                     
                             CANCELADO = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st  with (index(IX_Movimento_venda_01))

								WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento
                                    )

                        FROM

                             Lista_finalizadora lista

                             INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 
                             INNER JOIN  Saida_estoque S  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv and s.Data_movimento = lista.Emissao

                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                        AND finalizadora.Finalizadora  = @finalizadora 
						and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
                         and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
						 and (LEN(@cancelados)=0 OR
								(@cancelados ='TODOS' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='ITEM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=0 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0) 
										OR (@cancelados='CUPOM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=1 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0)												   
																						   
																						   ) 
									
                       and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                        GROUP BY Emissao, PDV, lista.DOCUMENTO ,id_finalizadora,CONVERT(varchar , Hora_venda,108),SUBSTRING(S.ID_CHAVE,35,6)

                            

                  END

      END

 

ELSE IF (@plu<>'' AND @cupom='' and @ext_sat = '')

BEGIN

      SELECT CUPOM = S.Documento,
			 [Ext SAT] = SUBSTRING(S.ID_CHAVE,35,6),
             DATA = CONVERT(VARCHAR,s.Data_movimento,103),
             HORA = convert(varchar,Hora_venda),
	         PDV=convert(varchar,s.caixa_saida) ,
			 'PLU'+S.PLU as PLU,
			 DESCRICAO =M.Descricao,
             QTDE=replace(convert(varchar,S.Qtde),'.',','),
             VLR=replace(convert(varchar,S.vlr),'.',','),
			 [-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),
			 [+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),
			 TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

      FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) 
					--INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
                    INNER JOIN Mercadoria M ON S.PLU = M.PLU      
	  where (LEN(@cupom)=0 or  s.Documento  =  @cupom  )
                        and (len(@plu)=0 or s.PLU = @plu )
                        And s.Data_Cancelamento is null
                        and s.Data_movimento BETWEEN @DataDe  AND  @DataAte
						and s.Data_movimento between @DataDe aND @DataAte
                         --and (LEN(@pdv)=0 or l.pdv = @pdv)
                        and s.caixa_saida like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
						and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                        --Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
                        order by s.Data_movimento , Hora_venda

      END

ELSE

      BEGIN
      
            SELECT	CUPOM = S.Documento, --1
					[Ext SAT] = SUBSTRING(S.ID_CHAVE,35,6),--2
					DATA = CONVERT(VARCHAR,s.Data_movimento,103), --3
					PDV=convert(varchar,s.caixa_saida) ,--4
					'PLU'+S.PLU AS PLU, --5
					DESCRICAO = M.Descricao, --6
					QTDE=replace(convert(varchar,S.Qtde),'.',','),--7
					VLR=replace(convert(varchar,S.vlr),'.',','),--8
					[-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),--9
					[+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),--10
					TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') --11
            FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) 
						--INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
						INNER JOIN Mercadoria M ON S.PLU = M.PLU      
			where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
						and (len(@ext_sat)=0 or SUBSTRING(S.ID_CHAVE,35,6)= @ext_sat)
                        and s.PLU like (case when @plu <>'' then @plu else '%' end )
                        and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                        And s.Data_Cancelamento is null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and s.caixa_saida like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                       Group by s.Documento,s.data_movimento,s.caixa_saida,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda,SUBSTRING(S.ID_CHAVE,35,6)
		union all                      
			SELECT '',--1
				   '',--2
                   '|-CANCELADO-|',--3
				   pdv=convert(varchar,s.caixa_saida) ,--4
                   '|-'+S.PLU+'-|',--5
                   '|-'+M.Descricao+'-|', --6
                   Qtde=replace(convert(varchar,S.Qtde),'.',','),--7
                   Vlr=replace(convert(varchar,S.vlr),'.',','),--8
                   [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),--9
                   [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),--10
                   Total='0,000' --11
            FROM Saida_estoque S  with (index(IX_Movimento_venda_01))
				--INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
				INNER JOIN Mercadoria M ON S.PLU = M.PLU      
			where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
					and (len(@ext_sat)=0 or SUBSTRING(S.ID_CHAVE,35,6)= @ext_sat)
					and s.PLU like (case when @plu <>'' then @plu else '%' end )
                    and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                	And s.Data_Cancelamento is NOT null
					and s.Data_movimento between @DataDe aND @DataAte 
                    and s.caixa_saida like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                    and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
		union all

            select  '',--1
					'',--2
					'',--3
					'',--4
					'',--5
					'|-'+ id_finalizadora+'-|'  ,--6
					'',--7
					'',--8
					'',--9
					'',--10
					 '|-'+replace(convert(varchar,(SUM(Lista_Finalizadora.Total))),'.',',')+'-|' --11
			from Lista_finalizadora
			where  Documento like (case when @cupom <>'TODOS' then @cupom  else '%' end  )
		
                   and Emissao BETWEEN @DataDe  AND  @DataAte
                   And Isnull(Cancelado,0) = 0
                   and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
			GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END


end





GO




IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.200.676', getdate();
GO

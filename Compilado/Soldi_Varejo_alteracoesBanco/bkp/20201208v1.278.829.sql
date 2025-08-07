IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('Altura'))
begin
	alter table Mercadoria alter column Altura Numeric(9,3)
	alter table Mercadoria alter column Largura Numeric(9,3)
	alter table Mercadoria alter column Profundidade Numeric(9, 3)
	alter table Mercadoria alter column Categoria_eCommerce VARCHAR(20)
end
else
begin
	alter table Mercadoria Add Altura Numeric(9,3), Largura Numeric(9,3), Profundidade Numeric(9, 3), Categoria_eCommerce VARCHAR(20)
end 



go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Finalizadora') 
            AND  UPPER(COLUMN_NAME) = UPPER('Valida_Cliente'))
begin
	alter table Finalizadora alter column Valida_Cliente Tinyint
	alter table Finalizadora alter column Checa_Limite Tinyint
end
else
begin
	alter table Finalizadora add Valida_Cliente Tinyint, Checa_Limite Tinyint
end 

go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('Lugar_Entrega'))
begin
	alter table pedido alter column Lugar_Entrega Varchar(20)
	alter table pedido alter column Taxas Decimal(12,2)
end
else
begin
	alter table pedido add Lugar_Entrega Varchar(20), Taxas Decimal(12,2)
end 

go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido_itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('ID_Fracao'))
begin
	alter table pedido_itens alter column ID_Fracao Tinyint
end
else
begin
	alter table pedido_itens add ID_Fracao Tinyint
end 

go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('total_produto'))
begin
	alter table nf_item alter column total_produto numeric(12,4)
end
else
begin
	alter table nf_item add total_produto numeric(12,4)
end 

go 

update nf_item set total_produto =(qtde*Embalagem) * Unitario

go 



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].sp_Rel_Venda_nf') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Venda_nf
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Rel_Venda_nf] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8)
		as
begin
	-- exec sp_Rel_Venda_nf 'MATRIZ','20141201','20141220'
	Select nf.CODIGO,convert(varchar,NF.Data,103) Data, nf.Cliente_Fornecedor Cod,cliente.Nome_Cliente,Total = nf.Total
	from NF inner join cliente on NF.Cliente_Fornecedor= cliente.Codigo_Cliente 
			INNER JOIN NF_Item ON NF.Codigo = NF_ITEM.CODIGO 		
	where NF.Tipo_NF = 1 and (NF.Data between @DataDe and @DataAte )AND nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403')  
	group by nf.Codigo,nf.data,nf.Cliente_Fornecedor,cliente.Nome_Cliente,nf.total
end
		  



go 
ALTER  Procedure [dbo].[sp_Movimento_Venda]

                @Filial				As Varchar(20),
                @DataDe				As Varchar(8),
                @DataAte			As Varchar(8),
                @finalizadora		As varchar(30),
                @plu				As varchar(17),
                @cupom				As varchar(20),
                @pdv				as varchar(10),                
                @horaInicio			as varchar(5),				
				@horafim			as varchar(5),
				@cancelados			as varchar(15),
				@comanda			as varchar(20),
				@ext_sat			as varchar(6)

AS

begin 
--exec     sp_Movimento_Venda   @Filial='MATRIZ', @DataDe = '20180602',  @dataate = '20180602',  @horaInicio = '00:00',  @horafim = '23:59',  @finalizadora = 'TODOS',  @Pdv = 'TODOS',  @plu = '',  @cupom = '',  @comanda = '',  @cancelados = 'TODOS',  @ext_sat = '' 

Select Data_movimento, S.Documento, S.PLU, m.DESCRICAO, S.QTDE, vlr, Desconto,  Acrescimo ,
	Caixa_Saida, Ext_SAT = '', Hora_venda, ComandaPedidoCupom, Finalizadora = 'NAO FINALIZADOS'
Into 
	#CupomSemPagamento
From 
	Saida_estoque s INNER JOIN Mercadoria m on s.plu = m.plu 
	Where 
		s.Data_movimento BETWEEN @DataDe AND @DataAte
	And 
		not s.data_cancelamento is null
	And 
		s.Filial = @FILIAL   
	And 
		not exists(select * from Lista_finalizadora l 
					Where l.documento = s.documento and l.Emissao = s.data_movimento
						And l.pdv = s.caixa_saida
						And l.filial = s.Filial)


--exec sp_Movimento_Venda @Filial='MATRIZ', @DataDe = '20180705',  @dataate = '20180705',  @horaInicio = '00:00',  @horafim = '23:59',  @finalizadora = 'TODOS',  @Pdv = 'TODOS',  @plu = '',  @cupom = '',  @comanda = '',  @cancelados = '',  @ext_sat = '' 
IF(@plu='' AND @cupom='' and @ext_sat ='')

      BEGIN

            IF(@finalizadora ='TODOS')

                  BEGIN

                        SELECT DISTINCT

                             DATA = CONVERT(VARCHAR,lista.EMISSAO,103),

                             lista.PDV,

                             CUPOM = lista.DOCUMENTO,
                             [Ext SAT ] = 'SFT_'+SUBSTRING(isnull(S.ID_CHAVE,(
																Select TOP 1 S2.ID_CHAVE		 
																FROM SAIDA_ESTOQUE  AS S2
																WHERE S2.Documento = S.Documento
																  AND S2.Caixa_Saida = S.Caixa_Saida
																  AND S2.Data_movimento = S.Data_movimento
																  AND S2.Filial = S.Filial
																  AND S2.ID_Chave IS NOT NULL
															)
													)
							 					,35,6),
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
                             CANCELADOS = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) 
										    FROM Saida_estoque st  with (index(IX_Movimento_venda_01)) 
											WHERE st.Filial = @FILIAL And data_cancelamento is not null 
													and CONVERT(varchar , st.Hora_venda,108) between @horaInicio and @horafim 
													AND (st.Data_movimento = lista.Emissao)
													and st.Caixa_Saida =lista.pdv
													and st.documento = lista.documento),
						 [CHAVE] = '|chave|'+isnull(S.ID_CHAVE,(
																Select TOP 1 S2.ID_CHAVE		 
																FROM SAIDA_ESTOQUE  AS S2
																WHERE S2.Documento = S.Documento
																  AND S2.Caixa_Saida = S.Caixa_Saida
																  AND S2.Data_movimento = S.Data_movimento
																  AND S2.Filial = S.Filial
																  AND S2.ID_Chave IS NOT NULL
															)
													)

						INTO #LISTA_TEMP FROM Lista_finalizadora lista
                            INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 
							INNER JOIN Saida_estoque S  with (index(IX_Movimento_venda_01))  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv	and s.Data_movimento = lista.Emissao
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
						
						--Incluir Registros sem Pagamento
						Insert into #LISTA_TEMP
						Select  DISTINCT
							DATA = CONVERT(VARCHAR,c.Data_movimento,103)
						  , PDV = c.Caixa_Saida
						  , Cupom = Documento
						  , Ext_SAT
						  , CONVERT(varchar , C.Hora_venda,108) as HORA
						  , [COMANDA/PEDIDOS] = '_'+ Max(ComandaPedidoCupom)
						  , Finalizadora
						  , vlr = 0
						  , Cancelados = SUM(Vlr-isnull(Desconto,0)) 
						  ,''
						From 
							#CupomSemPagamento c 
						Where
							CONVERT(varchar , c.Hora_venda,108) between @horaInicio and @horafim 
						--And 
						--	(LEN(@comanda)=0 or c.ComandaPedidoCupom  like '%'+@comanda+'%')
						and 
							Finalizadora = (case when @cancelados = ''				 then 'NAO FINALIZADOS' 
													when @cancelados = 'NAO FINALIZADOS' then 'NAO FINALIZADOS' 
													when @cancelados = 'TODOS'			 then 'NAO FINALIZADOS' 
													else 'NAO APARECE NADA' end)								
						Group By 			
							c.Data_movimento, c.Caixa_Saida, Documento, Ext_SAT, Finalizadora	, HORA_VENDA																																	   
                       
                       ----*******************************************************---------
                       
					   
						UPDATE #LISTA_TEMP SET CANCELADOS = (CANCELADOS/(SELECT COUNT(B.CUPOM) FROM #LISTA_TEMP AS B WHERE  B.CUPOM = #LISTA_TEMP.CUPOM and b.pdv = #LISTA_TEMP.pdv)  ) 
						WHERE CUPOM IN (SELECT CUPOM FROM #LISTA_TEMP where CANCELADOS >0 GROUP BY CUPOM,pdv HAVING   COUNT(CUPOM) > 1   )
						AND CANCELADOS >0
						

                       SELECT DISTINCT * FROM #LISTA_TEMP
					   

                  END

            ELSE

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = lista.DOCUMENTO,
                             [Ext SAT] = 'SFT_'+SUBSTRING(isnull(S.ID_CHAVE,(
																Select TOP 1 S2.ID_CHAVE		 
																FROM SAIDA_ESTOQUE  AS S2
																WHERE S2.Documento = S.Documento
																  AND S2.Caixa_Saida = S.Caixa_Saida
																  AND S2.Data_movimento = S.Data_movimento
																  AND S2.Filial = S.Filial
																  AND S2.ID_Chave IS NOT NULL
															)
													)
							 					,35,6),
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
                                    ),
							[CHAVE] = '|chave|'+isnull(S.ID_CHAVE,(
													Select TOP 1 S2.ID_CHAVE		 
													FROM SAIDA_ESTOQUE  AS S2
													WHERE S2.Documento = S.Documento
														AND S2.Caixa_Saida = S.Caixa_Saida
														AND S2.Data_movimento = S.Data_movimento
														AND S2.Filial = S.Filial
														AND S2.ID_Chave IS NOT NULL
												)
										)


                      INTO #LISTA_TEMP2   FROM

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
                      --  GROUP BY Emissao, PDV, lista.DOCUMENTO ,id_finalizadora,CONVERT(varchar , Hora_venda,108),SUBSTRING(S.ID_CHAVE,35,6)




                             SELECT DISTINCT * FROM #LISTA_TEMP2

                  END

      END

 

ELSE IF (@plu<>'' AND @cupom='' and @ext_sat = '')

BEGIN

      SELECT CUPOM = S.Documento,
			 [Ext SAT] = SUBSTRING(isnull(S.ID_CHAVE,(
																Select TOP 1 S2.ID_CHAVE		 
																FROM SAIDA_ESTOQUE  AS S2
																WHERE S2.Documento = S.Documento
																  AND S2.Caixa_Saida = S.Caixa_Saida
																  AND S2.Data_movimento = S.Data_movimento
																  AND S2.Filial = S.Filial
																  AND S2.ID_Chave IS NOT NULL
															)
													)
							 					,35,6),
             DATA = CONVERT(VARCHAR,s.Data_movimento,103),
             HORA = convert(varchar,Hora_venda),
	         PDV=convert(varchar,s.caixa_saida) ,
			 'PLU'+S.PLU as PLU,
			 DESCRICAO =M.Descricao,
             QTDE=replace(convert(varchar,S.Qtde),'.',','),
             VLR=replace(convert(varchar,S.vlr),'.',','),
			 [-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),
			 [+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),
			 TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') ,
			 [CHAVE] = '|chave|'+isnull(S.ID_CHAVE,(
					Select TOP 1 S2.ID_CHAVE		 
					FROM SAIDA_ESTOQUE  AS S2
					WHERE S2.Documento = S.Documento
						AND S2.Caixa_Saida = S.Caixa_Saida
						AND S2.Data_movimento = S.Data_movimento
						AND S2.Filial = S.Filial
						AND S2.ID_Chave IS NOT NULL
					)
			 )

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
					[Ext SAT] = SUBSTRING(isnull(S.ID_CHAVE,(
																Select TOP 1 S2.ID_CHAVE		 
																FROM SAIDA_ESTOQUE  AS S2
																WHERE S2.Documento = S.Documento
																  AND S2.Caixa_Saida = S.Caixa_Saida
																  AND S2.Data_movimento = S.Data_movimento
																  AND S2.Filial = S.Filial
																  AND S2.ID_Chave IS NOT NULL
															)
													)
							 					,35,6),--2
					DATA = CONVERT(VARCHAR,s.Data_movimento,103), --3
					PDV=convert(varchar,s.caixa_saida) ,--4
					'PLU'+S.PLU AS PLU, --5
					DESCRICAO = M.Descricao, --6
					QTDE=replace(convert(varchar,S.Qtde),'.',','),--7
					VLR=replace(convert(varchar,S.vlr),'.',','),--8
					[-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),--9
					[+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),--10
					TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',','), --11
					[CHAVE] = '|chave|'+isnull(S.ID_CHAVE,(
							Select TOP 1 S2.ID_CHAVE		 
							FROM SAIDA_ESTOQUE  AS S2
							WHERE S2.Documento = S.Documento
								AND S2.Caixa_Saida = S.Caixa_Saida
								AND S2.Data_movimento = S.Data_movimento
								AND S2.Filial = S.Filial
								AND S2.ID_Chave IS NOT NULL
							)
					 )

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
                       --Group by s.Documento,s.data_movimento,s.caixa_saida,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda,SUBSTRING(S.ID_CHAVE,35,6)
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
                   Total='0,000', --11
				   ''--12
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
					,'' -- 12
			from Lista_finalizadora
			where  Documento like (case when @cupom <>'TODOS' then @cupom  else '%' end  )
		
                   and Emissao BETWEEN @DataDe  AND  @DataAte
                   And Isnull(Cancelado,0) = 0
                   and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
			GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END


end






go 



go 



insert into Versoes_Atualizadas select 'Versão:1.278.829', getdate();
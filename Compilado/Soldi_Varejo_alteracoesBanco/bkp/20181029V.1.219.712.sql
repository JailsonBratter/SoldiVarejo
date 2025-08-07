


if(OBJECT_ID('sp_Movimento_Venda','P') is not null)
	drop procedure [sp_Movimento_Venda]
go 




--PROCEDURES =======================================================================================
create Procedure [dbo].[sp_Movimento_Venda]

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
--exec sp_Movimento_Venda @Filial='MATRIZ', @DataDe = '20180705',  @dataate = '20180705',  @horaInicio = '00:00',  @horafim = '23:59',  @finalizadora = 'TODOS',  @Pdv = 'TODOS',  @plu = '',  @cupom = '',  @comanda = '',  @cancelados = '',  @ext_sat = '' 
IF(@plu='' AND @cupom='' and @ext_sat ='')

      BEGIN

            IF(@finalizadora ='TODOS')

                  BEGIN

                        SELECT

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
                             CANCELADOS = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st  with (index(IX_Movimento_venda_01)) 
													WHERE st.Filial = @FILIAL And data_cancelamento is not null 
													and CONVERT(varchar , st.Hora_venda,108) between @horaInicio and @horafim 
													AND (st.Data_movimento = lista.Emissao)
													and st.Caixa_Saida =lista.pdv
													and st.documento = lista.documento)
						INTO #LISTA_TEMP FROM Lista_finalizadora lista
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


if(OBJECT_ID('sp_EFD_PisCofins_C010','P') is not null)
	drop procedure [sp_EFD_PisCofins_C010]
go 

-- STORED PROCEDURE
-- exec sp_EFD_PisCofins_0000 @FILIAL='MATRIZ',@Data_Ini='01072018',@Data_Fim='31072018',@Tipo=0
CREATE    Procedure [dbo].[sp_EFD_PisCofins_C010]   
	@Filial			AS Varchar(20),
	@DataInicio		As Varchar(8),
	@DataFim		As Varchar(8)
AS                            
BEGIN

	SELECT DISTINCT
		REG = 'C010',
		--COD_PART = Filial,
		CNPJ = SUBSTRING(REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(cnpj,'')), '.', ''), '/', ''), '-', ''), 1, 14),
		IND_ESCRI = ''
	FROM
		Filial

	WHERE
		Filial = @Filial

	RETURN                          
                      
END






GO





if(OBJECT_ID('sp_EFD_PisCofins_0000','P') is not null)
	drop procedure [sp_EFD_PisCofins_0000]
go 
--STORED PROCEDURE

--Exec SP_EFD_PISCOFINS_0000 'MATRIZ', '01072018', '31012018', 0

CREATE   procedure [dbo].[sp_EFD_PisCofins_0000] 
	@Filial Varchar(20),
	@data_ini Varchar(8),
	@data_Fim Varchar(8),
	@Tipo Integer
AS              
              
BEGIN

	DECLARE  @errno   INT,                            
			 @errmsg  VARCHAR(255),
			 @ano	  int                            

			 set @ano = convert(int, substring(@data_ini,5,4));
  

	IF @data_ini is null or @data_fim is null                            
		BEGIN                            
			SELECT 
				@errno  = 99999,                            
				@errmsg = 'Você deve preencher a Data Inicio e a Data Final'                          
			GOTO error                            
		END
	IF @Tipo = 1 
		BEGIN
			SELECT 
			--** Campo [REG] Texto fixo contendo "0000" C 004 - O
			'|0000' + 
			--** Campo [COD_VER] Código da versão do leiaute conforme a tabela indicada no Ato COTEPE N 003* - O
			'|' + replicate('0',3-len(rtrim(ltrim( convert(varchar(3),@ano-2006))))) +rtrim(ltrim( convert(varchar(3),@ano-2006)))+
			--** Campo [COD_FIN] Código da finalidade do arquivo: N 001 - O
				--** 0 - Remessa do arquivo original;
				--** 1 - Remessa do arquivo substituto.
			'|0' +
			--** Campo DT_INI Data inicial das informações contidas no arquivo. N 008* - O
			'|' + @DATA_INI + 
			--** Campo DT_FIN Data final das informações contidas no arquivo. N 008* - O
			'|' + @DATA_FIM + 
			--** Campo NOME Nome empresarial da entidade. C 100 - O
			'|' + CASE WHEN LEN(RTRIM(LTRIM(ISNULL(razao_social,'')))) < 100 THEN LTRIM(RTRIM(ISNULL(razao_social,''))) ELSE SUBSTRING(LTRIM(RTRIM(ISNULL(razao_social,''))), 1, 100) END +        
			--** Campo CNPJ Número de inscrição da entidade no CNPJ. N 014* - OC
			-- TEMPORARIO
		        '|' + SUBSTRING(REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(cnpj,'')), '.', ''), '/', ''), '-', ''), 1, 14) +
			--** CPF
			'|' + 
			--** Campo UF Sigla da unidade da federação da entidade. C 002* - O
			'|' + LTRIM(RTRIM(UF)) + 
			--** IE
		        '|' + ISNULL(IE,'') +
			--** Campo COD_MUN Código do município do domicílio fiscal da entidade, conforme a tabela IBGE N 007* - O
			'|' + '3550308' + 
			--** Campo IM Inscrição Municipal da Entidade
			'|' + 
			--** Campo SUFRAMA Inscrição da entidade na SUFRAMA C 009* - OC
			'|' + 
			--** Perfil de apresentação do arquivo fiscal
				--** A - Perfil A;
				--** B - Perfil B;
				--** C - Perfil C.
			'|A' + 
			--** Campo IND_ATIV Indicador de tipo de atividade: N 001 - O
				--** 0 – Industrial ou equiparado a industrial;
				--** 1 – Outros.
			'|1|'
	
			 FROM Filial  WHERE filial = @Filial
		END
	ELSE
		BEGIN
			SELECT 
			--** Campo [REG] Texto fixo contendo "0000" C 004 - O
			'|0000' + 
			--** Campo [COD_VER] Código da versão do leiaute conforme a tabela indicada no Ato COTEPE N 003* - O
			'|' + replicate('0',3-len(rtrim(ltrim( convert(varchar(3),@ano-2014))))) +rtrim(ltrim( convert(varchar(3),@ano-2014)))+
			 
			--** Campo [COD_FIN] Código da finalidade do arquivo: N 001 - O
				--** 0 - Remessa do arquivo original;
				--** 1 - Remessa do arquivo substituto.
			'|0' +
			--** Indicador de situação especial
			'|' +
			--** Número do Recibo da Escrituração anterior a ser retificada, utilizando quando TIPO_ESCRIT for igual a 1
			'|' +
			--** Campo DT_INI Data inicial das informações contidas no arquivo. N 008* - O
			'|' + @DATA_INI + 
			--** Campo DT_FIN Data final das informações contidas no arquivo. N 008* - O
			'|' + @DATA_FIM + 
			--** Campo NOME Nome empresarial da entidade. C 100 - O
			'|' + CASE WHEN LEN(RTRIM(LTRIM(ISNULL(razao_social,'')))) < 100 THEN LTRIM(RTRIM(ISNULL(razao_social,''))) ELSE SUBSTRING(LTRIM(RTRIM(ISNULL(razao_social,''))), 1, 100) END +        
			--** Campo CNPJ Número de inscrição da entidade no CNPJ. N 014* - OC
			-- TEMPORARIO
		        '|' + SUBSTRING(REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(cnpj,'')), '.', ''), '/', ''), '-', ''), 1, 14) +
			--** Campo UF Sigla da unidade da federação da entidade. C 002* - O
			'|' + LTRIM(RTRIM(UF)) + 
			--** Campo COD_MUN Código do município do domicílio fiscal da entidade, conforme a tabela IBGE N 007* - O
			'|' + '3534401' + 
			--** Campo SUFRAMA Inscrição da entidade na SUFRAMA C 009* - OC
			'|' + 
			--** Indicador da natureza da pessoa jurídica
				--** 00 - Sociedade empresária em geral
				--** 01 - Sociedade cooperativa
				--** 02 - Entidade sujeita ao PIS/Pasep exclusivamente com base na Folha de Salários
			'|00' + 
			--** Campo IND_ATIV Indicador de tipo de atividade: N 001 - O
				--** 0 – Industrial ou equiparado a industrial;
				--** 1 – Prestador de serviços;
				--** 2 - Atividade de comércio;
				--** 3 – Atividade financeira;
				--** 4 – Atividade imobiliária;
				--** 9 – Outros.
			'|2|'
	
			 FROM Filial  WHERE filial = @Filial
		END


		RETURN                          

	error:                            
		--RAISERROR @errno @errmsg                            
		RETURN                          
END


go 


if(OBJECT_ID('sp_rel_Vendas_por_tabela','P') is not null)
	drop procedure [sp_rel_Vendas_por_tabela]
go 






CREATE PROCEDURE sp_rel_Vendas_por_tabela

		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8)
		
		as
BEGIN
Select 
	Tabela =isnull(c.Codigo_tabela,''),
	Total = Sum(nf_item.Total) 
from NF inner join cliente AS C on NF.Cliente_Fornecedor= C.Codigo_Cliente 
			     INNER JOIN NF_Item ON NF.Codigo = NF_ITEM.CODIGO 		
		 where NF.Tipo_NF = 1 
			and (NF.Data between @DataDe and @DataAte )
			AND nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403')  
			and nf.Filial = @FILIAL
		 group by C.Codigo_tabela
		 ORDER BY Sum(nf_item.Total) DESC


end
GO 



if(OBJECT_ID('sp_rel_cliente','P') is not null)
	drop procedure [sp_rel_cliente]
go 


GO
--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_rel_cliente]  
  @Filial                 As Varchar(20),
  @codigo as varchar(30),
  @nome as varchar(50),
  @cnpj as varchar(20),
  @tabela as varchar(10),
  @tipoRel as varchar(40)
  
  as
  
  Begin
  
  --[sp_rel_cliente] 'MATRIZ','','','','TODOS','SIM'


		if(@tipoRel = 'NAO')
		BEGIN
		 select '|-CODIGO:-|'+a.codigo_cliente+ '|| |-NOME-|: '+a.nome_cliente +' || |-FANTASIA:-|'+A.nome_fantasia+' || |-CNPJ-|: '+ isnull(a.cnpj,'_____________________')+ '  |-IE-|: '+ISNULL(a.Ie,'_____________________')+  '|| ||' AS 'DADOS GERAIS',
			'|-ENDERECO-|:'+isnull(a.endereco,'_______________________________') +','+isnull(a.endereco_nro,'___')+' |-BAIRRO-|:'+isnull(a.Bairro,'____________')+' |-CIDADE-|:'+ isnull(a.cidade,'_________')+' |-UF-|:'+isnull(a.Uf,'__') + '||' +
			'|-DATA CADASTRO-|:'+ convert(varchar,a.data_cadastro,103) +'  || |-TELEFONE-| :' + ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS b where b.codigo_cliente = a.codigo_cliente and (b.meio_comunicacao like 'FONE%' OR b.meio_comunicacao like 'CELULAR%') ),'_____________') +
			' |-E-MAIL-|:'+ ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS c where c.codigo_cliente = a.codigo_cliente and c.meio_comunicacao like 'EMAIL%'),'_________________________')  +  '|| '+ '   |-TABELA:-|'+a.Codigo_tabela+' ||'
			as 'DETALHES' --into #ConsultaCliente
			from cliente as  a
			WHERE (LEN(@codigo)=0 OR a.Codigo_Cliente = @codigo)
				  AND (LEN(@nome)=0 OR  a.Nome_Cliente like '%'+@nome+'%')
				  and (len(@cnpj)=0 or replace(replace(replace(a.CNPJ,'.',''),'-',''),'/','')= replace(replace(replace(@cnpj,'.',''),'-',''),'/',''))
				  and (@tabela='TODOS' OR A.Codigo_tabela = @tabela)
		  ORDER BY A.Nome_Cliente	
		END
		ELSE
		BEGIN

		  select 
				'SFT_'+a.codigo_cliente As CODIGO, 
				Ltrim(Rtrim(a.nome_cliente)) as NOME,
				LTRIM(RTRIM(a.nome_fantasia)) as FANTASIA,
				a.CNPJ,
				TABELA = a.codigo_tabela,
				TELEFONE= 'SFT_'+ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS b where b.codigo_cliente = a.codigo_cliente and (b.meio_comunicacao like 'FONE%' OR b.meio_comunicacao like 'CELULAR%')), ''),
				EMAIL = 'SFT_'+ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS c where c.codigo_cliente = a.codigo_cliente and c.meio_comunicacao like 'EMAIL%'), '')
			from cliente as  a 
			WHERE (LEN(@codigo)=0 OR a.Codigo_Cliente = @codigo)
				  AND (LEN(@nome)=0 OR  a.Nome_Cliente like '%'+@nome+'%')
				  and (len(@cnpj)=0 or replace(replace(replace(a.CNPJ,'.',''),'-',''),'/','')= replace(replace(replace(@cnpj,'.',''),'-',''),'/',''))
				  and (@tabela='TODOS' OR A.Codigo_tabela = @tabela)
			Order by 2				  
		
			
		   END
END




GO
insert into Versoes_Atualizadas select 'Versão:1.219.712', getdate();
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Devolucao_NFe')
CREATE TABLE [dbo].[Devolucao_NFe](
	[Filial] [varchar](20) NOT NULL,
	[Codigo] [int] NOT NULL,
	[Status] [int] NULL,
	[Codigo_Cliente] [varchar](20) NULL,
	[Data_cadastro] [datetime] NULL,
	[horaCadastro] [varchar](5) NULL,
	[Total] [decimal](12, 2) NULL,
	[Origem][varchar](100) NULL,
	[EmissaoNFe] [datetime] null,
	[NumeroNFe][int] null,
	[ChaveNFe][varchar](47) null,
	[Usuario] [varchar](20) NULL,
	[Obs] [text] NULL,
 CONSTRAINT [PK_DevolucaoNFe] PRIMARY KEY CLUSTERED 
(
	[Filial] ASC,
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Devolucao_NFe_Item')

CREATE TABLE [dbo].[Devolucao_NFe_Item](
	[Filial] [varchar](20) NOT NULL,
	[Codigo] [int] NOT NULL,
	[PLU] [varchar](17) NOT NULL,
	[Qtde] [decimal](9, 3) NULL,
	[unitario] [decimal](12, 4) NULL,
	[TotalItem] [decimal](12, 4) NULL,
	[ChaveCFe] [varchar](47)
) ON [PRIMARY]

GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Funcionario') 
            AND  UPPER(COLUMN_NAME) = UPPER('Inativo'))
	alter table Funcionario alter column Inativo tinyint
	else
	alter table Funcionario add Inativo tinyint default 0

go 



if not exists(select * from parametros where parametro = 'PERMITE_VENDA_DE_INATIVO')
begin
INSERT PARAMETROS
(PARAMETRO, PENULT_ATUALIZACAO, VALOR_DEFAULT, ULT_ATUALIZACAO, VALOR_ATUAL, DESC_PARAMETRO, TIPO_DADO, 
RANGE_VALOR_ATUAL, GLOBAL, NOTA_PROGRAMADOR, ESCOPO, POR_USUARIO_OK, DATA_PARA_TRANSFERENCIA, PERMITE_POR_EMPRESA)
VALUES
('PROIBE_VENDER_INATIVO', GETDATE(), 'FALSE', GETDATE(), 'FALSE', 'NAO PERMITE A LISTAGEM PARA VENDA DE PRODUTOS INATIVOS', 'L', 0, 1, 'CRIADO PARA O GATAO', 0, 0, NULL, 0)
end

go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Movimento_Venda')
DROP PROCEDURE sp_Movimento_Venda
go


SET QUOTED_IDENTIFIER ON
GO
create  Procedure [dbo].[sp_Movimento_Venda]

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
				@ext_sat			as varchar(6),
				@cpf				as varchar(14)

AS

begin 
--exec     sp_Movimento_Venda   @Filial='MATRIZ', @DataDe = '20220701',  @dataate = '20220731',  @horaInicio = '00:00',  @horafim = '23:59',  @finalizadora = 'TODOS',  @Pdv = 'TODOS',  @plu = '',  @cupom = '',  @comanda = '',  @cancelados = 'TODOS',  @ext_sat = '', @CPF = '08785616842'

Select Data_movimento, S.Documento, S.PLU, m.DESCRICAO, S.QTDE, vlr, Desconto,  Acrescimo ,
	Caixa_Saida, Ext_SAT = '', Hora_venda, ComandaPedidoCupom, Finalizadora = 'NAO FINALIZADOS'
Into 
	#CupomSemPagamento
From 
	Saida_estoque  s WITH (nolock) INNER JOIN Mercadoria m on s.plu = m.plu 
	Where 

		s.Filial = @FILIAL   
	and 	
		s.Data_movimento BETWEEN @DataDe AND @DataAte
	And 
		not s.data_cancelamento is null
	and 
		Caixa_Saida like (case when @pdv <> 'TODOS' then @pdv else '%' end)
	And 
		not exists(select * from  Lista_finalizadora l with(nolock)
					Where l.filial = s.Filial 
					and l.Emissao = s.data_movimento
					and l.documento = s.documento 
						And l.pdv = s.caixa_saida
						)

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
													and CONVERT(varchar , st.Hora_venda,108) between @horaInicio +':00' and @horafim +':59' 
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
								  and CONVERT(varchar , s.Hora_venda,108) between @horaInicio +':00' and @horafim +':59' 
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
						and (REPLACE(REPLACE(REPLACE(ISNULL(S.CPF_CNPJ,''),'.',''),'-',''),'/','') LIKE CASE WHEN LEN(@CPF)=0 THEN '%%' ELSE @CPF END)

						
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
							CONVERT(varchar , c.Hora_venda,108) between @horaInicio +':00' and @horafim +':59' 
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
						and CONVERT(varchar , Hora_venda,108) between @horaInicio +':00' and @horafim +':59' 
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
						and (REPLACE(REPLACE(REPLACE(S.CPF_CNPJ,'.',''),'-',''),'/','') LIKE CASE WHEN LEN(@CPF)=0 THEN '%%' ELSE @CPF END)
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
                        and CONVERT(varchar , Hora_venda,108) between @horaInicio +':00' and @horafim +':59' 
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

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Rel_Venda_pedido_cliente')
DROP PROCEDURE sp_Rel_Venda_pedido_cliente
go


GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_Venda_pedido_cliente]    Script Date: 18/04/2023 14:45:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--PROCEDURES =======================================================================================
create PROCEDURE [dbo].[sp_Rel_Venda_pedido_cliente] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cliente   as varchar(40),
		@simples	as varchar(5),
		@vendedor   as varchar(30),
		@TipoData as varchar(30)
		
    	as
			-- exec sp_Rel_Venda_pedido_cliente 'MATRIZ','20141201','20141220','',''
	if @TipoData = 'ENTREGA'
		begin
		Select 
			Simples = case when Pedido.pedido_simples=1 then 'SIM'ELSE 'NAO' END ,
			Pedido,
			convert(varchar,pedido.Data_entrega,103) Data, 
			Cliente_Fornec Cod,
			cliente.Nome_Cliente,
			[Total Venda]=ROUND(Total,2), 
			[Total Frete] = pedido.frete,
			ISNULL(Pedido.funcionario, '') As Vendedor 
		 From pedido inner join cliente on pedido.Cliente_Fornec= cliente.Codigo_Cliente 
		 Where  
			CONVERT(VARCHAR, Pedido.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
		  and 
			pedido.Data_entrega between @DataDe and @DataAte
		  AND 
			Pedido.Tipo =1 
		  AND 
			Pedido.Status in (1,2)
          And 
			(len(@cliente) =0 or  (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/',''))) 
		and 
			(@vendedor ='TODOS' OR Pedido.funcionario =@vendedor)
		
end				
if @TipoData = 'CADASTRO'
begin

Select 
			Simples = case when Pedido.pedido_simples=1 then 'SIM'ELSE 'NAO' END ,
			Pedido,
			convert(varchar,pedido.Data_entrega,103) Data, 
			Cliente_Fornec Cod,
			cliente.Nome_Cliente,
			[Total Venda]=ROUND(Total,2), 
			[Total Frete] = pedido.frete,
			ISNULL(Pedido.funcionario, '') As Vendedor 
		 From pedido inner join cliente on pedido.Cliente_Fornec= cliente.Codigo_Cliente 
		 Where  
			CONVERT(VARCHAR, Pedido.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
		  and 
			pedido.Data_cadastro between @DataDe and @DataAte
		  AND 
			Pedido.Tipo =1 
		  AND 
			Pedido.Status in (1,2)
          And 
			(len(@cliente) =0 or  (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/',''))) 
		and 
			(@vendedor ='TODOS' OR Pedido.funcionario =@vendedor)
			
end
go

insert into Versoes_Atualizadas
select 'Versão:1.318.902', getdate()
go
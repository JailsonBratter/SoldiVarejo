go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('saida_estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('id_chave'))
begin
	alter table saida_estoque alter column id_chave varchar(47)
end
else
begin
	alter table saida_estoque add id_chave varchar(47)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('saida_estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('id_chave_cancelamento'))
begin
	alter table saida_estoque alter column id_chave_cancelamento varchar(47)
end
else
begin
	alter table saida_estoque add id_chave_cancelamento varchar(47)
end 


go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('saida_estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('naoComputa'))
begin
	alter table saida_estoque alter column naoComputa tinyint
end
else
begin
	alter table saida_estoque add naoComputa tinyint
end 
go 




/****** Object:  StoredProcedure [dbo].[sp_Movimento_Venda]    Script Date: 05/11/2018 09:36:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Movimento_Venda]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Movimento_Venda]
GO



/****** Object:  StoredProcedure [dbo].[sp_Movimento_Venda]    Script Date: 05/11/2018 09:36:37 ******/
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
             DATA = CONVERT(VARCHAR,L.Emissao,103),
             HORA = convert(varchar,Hora_venda),
	         PDV=convert(varchar,L.pdv) ,
			 'PLU'+S.PLU as PLU,
			 DESCRICAO =M.Descricao,
             QTDE=replace(convert(varchar,S.Qtde),'.',','),
             VLR=replace(convert(varchar,S.vlr),'.',','),
			 [-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),
			 [+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),
			 TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

      FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) 
					INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
                    INNER JOIN Mercadoria M ON S.PLU = M.PLU      
	  where (LEN(@cupom)=0 or  s.Documento  =  @cupom  )
                        and (len(@plu)=0 or s.PLU = @plu )
                        And s.Data_Cancelamento is null
                        and l.Emissao BETWEEN @DataDe  AND  @DataAte
						and s.Data_movimento between @DataDe aND @DataAte
                         --and (LEN(@pdv)=0 or l.pdv = @pdv)
                        and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
						and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                        --Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
                        order by l.Emissao , Hora_venda

      END

ELSE

      BEGIN
      
            SELECT	CUPOM = S.Documento, --1
					[Ext SAT] = SUBSTRING(S.ID_CHAVE,35,6),--2
					DATA = CONVERT(VARCHAR,L.Emissao,103), --3
					PDV=convert(varchar,L.pdv) ,--4
					'PLU'+S.PLU AS PLU, --5
					DESCRICAO = M.Descricao, --6
					QTDE=replace(convert(varchar,S.Qtde),'.',','),--7
					VLR=replace(convert(varchar,S.vlr),'.',','),--8
					[-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),--9
					[+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),--10
					TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') --11
            FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) 
						INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
						INNER JOIN Mercadoria M ON S.PLU = M.PLU      
			where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
						and (len(@ext_sat)=0 or SUBSTRING(S.ID_CHAVE,35,6)= @ext_sat)
                        and s.PLU like (case when @plu <>'' then @plu else '%' end )
                        and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                        and l.Emissao BETWEEN @DataDe  AND  @DataAte
						And s.Data_Cancelamento is null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and l.pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                       Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda,SUBSTRING(S.ID_CHAVE,35,6)
		union all                      
			SELECT '',--1
				   '',--2
                   '|-CANCELADO-|',--3
				   pdv=convert(varchar,L.pdv) ,--4
                   '|-'+S.PLU+'-|',--5
                   '|-'+M.Descricao+'-|', --6
                   Qtde=replace(convert(varchar,S.Qtde),'.',','),--7
                   Vlr=replace(convert(varchar,S.vlr),'.',','),--8
                   [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),--9
                   [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),--10
                   Total='0,000' --11
            FROM Saida_estoque S  with (index(IX_Movimento_venda_01))
				INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
				INNER JOIN Mercadoria M ON S.PLU = M.PLU      
			where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
					and (len(@ext_sat)=0 or SUBSTRING(S.ID_CHAVE,35,6)= @ext_sat)
					and s.PLU like (case when @plu <>'' then @plu else '%' end )
                    and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                    and l.Emissao BETWEEN @DataDe  AND  @DataAte
					And s.Data_Cancelamento is NOT null
					and s.Data_movimento between @DataDe aND @DataAte 
                    and l.pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
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





IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.197.665', getdate();
GO

update fornecedor set indIEDest =9 where isnull(pessoa_fisica,0) = 1;
update fornecedor set indIEDest = 9 where isnull(pessoa_fisica,0) = 0 and IE = '' ;
update fornecedor set indIEDest = 1 where isnull(pessoa_fisica,0) = 0 and IE <> '' ;



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('tesouraria_detalhes') 
            AND  UPPER(COLUMN_NAME) = UPPER('hora_venda'))
begin
	alter table tesouraria_detalhes alter column hora_venda varchar(20)
end
else
begin
		
		alter table tesouraria_detalhes add hora_venda varchar(8)

end 


go 


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Resumo_Vendas]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Resumo_Vendas]
end
GO
--PROCEDURES =======================================================================================
CREATE   Procedure [dbo].[sp_Rel_Resumo_Vendas]

            @Filial           As Varchar(20),

            @DataDe           As Varchar(8),

            @DataAte    As Varchar(8)

 

AS

 SELECT
            Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            Vlr = Convert(Decimal(15,2),SUM(Vlr-isnull(Desconto,0)+isnull(acrescimo,0))),
            CPF_CNPJ
      INTO
            #Lixo
      FROM
            Saida_Estoque
      WHERE
            Filial = @Filial
      AND
            Data_Movimento BETWEEN @DataDe AND @DataAte
      AND
            Data_Cancelamento IS NULL
      and   hora_venda between '00:00:00' and '23:59:59'
      GROUP BY
            Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            CPF_CNPJ

    Select  Data,
			Dia_Semana,
			Sum(Venda) as Venda,
			--Sum(Vlr_Cancel) as Vlr_Cancel ,
			SUM(Clientes) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
    from(

      SELECT

      Data = Convert(Varchar,Data_Movimento,103),

      Dia_Semana = CASE

            WHEN DATEPART(dw, Data_Movimento) = 1 THEN 'DOMINGO'

            WHEN DATEPART(dw, Data_Movimento) = 2 THEN 'SEGUNDA'

            WHEN DATEPART(dw, Data_Movimento) = 3 THEN 'TERÇA'

            WHEN DATEPART(dw, Data_Movimento) = 4 THEN 'QUARTA'

            WHEN DATEPART(dw, Data_Movimento) = 5 THEN 'QUINTA'

            WHEN DATEPART(dw, Data_Movimento) = 6 THEN 'SEXTA'

            WHEN DATEPART(dw, Data_Movimento) = 7 THEN 'SABADO'

      END,

      --Qtde =        SUM(CASE WHEN Data_Cancelamento IS Null THEN Qtde ELSE 0 END),

      Venda =       SUM(CASE WHEN Data_Cancelamento IS Null THEN Convert(Decimal(18,2),VLR-desconto) ELSE 0 END),

      --Qtde_Cancel = SUM(CASE WHEN Data_Cancelamento IS NOT Null THEN Qtde ELSE 0 END),

      -- Vlr_Cancel =  SUM(CASE WHEN ISNULL(cupom_cancelado,0)=1 THEN Convert(Decimal(18,2),Vlr) ELSE 0 END),

      Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento),

      Venda_MD =    CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #LIXO WHERE #LIXO.Data_Movimento = Saida_Estoque.Data_Movimento) > 0 THEN

                  ((SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #LIXO WHERE #LIXO.Data_Movimento = Saida_Estoque.Data_Movimento) /  (SELECT COUNT(*) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento))

                  ELSE 0 END),

      NFP =         (SELECT COUNT(*) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> ''),

      Vlr_NFP =     ISNULL((SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> ''),0),

     

      Perc_NFP =    CASE WHEN (SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> '') > 0 THEN

                  CONVERT(DECIMAL(8,2), ((SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> '') /

                        (SELECT Convert(Decimal(18,2),SUM(#Lixo.Vlr)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento)) * 100)

                  ELSE 0 END

      FROM

            Saida_Estoque

      WHERE

            Filial = @Filial

      AND

            Data_Movimento BETWEEN @DataDe AND @DataAte

--    AND

--          Data_Cancelamento IS NULL

      GROUP BY

            Data_Movimento
union all


select 
	CONVERT(VARCHAR,N.Emissao,103) AS DATA ,
	  Dia_Semana = CASE

            WHEN DATEPART(dw, N.Emissao) = 1 THEN 'DOMINGO'

            WHEN DATEPART(dw, N.Emissao) = 2 THEN 'SEGUNDA'

            WHEN DATEPART(dw, N.Emissao) = 3 THEN 'TERÇA'

            WHEN DATEPART(dw, N.Emissao) = 4 THEN 'QUARTA'

            WHEN DATEPART(dw, N.Emissao) = 5 THEN 'QUINTA'

            WHEN DATEPART(dw, N.Emissao) = 6 THEN 'SEXTA'

            WHEN DATEPART(dw, N.Emissao) = 7 THEN 'SABADO'

      END,
      SUM(N.TOTAL) AS Venda,
  --    (
		--Select isnull(SUM(nf.total),0) 
		--from nf where NF.FILIAL=@filial 
		--		AND  (NF.Emissao= n.Emissao )
		--			 AND   NF.TIPO_NF = 1	
		--		--GROUP BY NF.Emissao 
  --    ) as [Vlr Cancel],
      (
		  	Select COUNT(*) 
		from nf where NF.FILIAL=@filial 
				AND  (NF.Emissao= n.Emissao )
					 AND   NF.TIPO_NF = 1	
	
    
      ) 
      as Clientes ,
      (
      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM nf WHERE nf.Emissao = n.Emissao) > 0 THEN

                  (
                  Select isnull(SUM(nf.total),0) 
					from nf where NF.FILIAL=@filial 
							 AND (NF.Emissao= n.Emissao )
							 AND NF.TIPO_NF = 1
							) 
							 /   
					(	
					Select COUNT(*) 
					from nf where NF.FILIAL=@filial 
								AND  (NF.Emissao= n.Emissao )
								AND   NF.TIPO_NF = 1	
					)

                  ELSE 0 END)
                  
                  
      ) as [Venda MD],
      0 as [NFP],
      0 AS [Vlr NFP],
      0 as [Perc NP]
       
from  NF as N
WHERE  N.FILIAL=@filial 
AND  (N.Emissao BETWEEN @DataDe AND @DataAte)
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
GROUP BY N.Emissao 

) as a
GROUP BY Data,Dia_Semana
ORDER BY Data






GO 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('saida_estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('cupom_cancelado'))
begin
	alter table saida_estoque alter column cupom_cancelado varchar(20)
end
else
begin
		alter table saida_estoque add cupom_cancelado tinyint

end 
go 


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Movimento_Venda]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Movimento_Venda]
end
GO
--PROCEDURES =======================================================================================
CREATE Procedure [dbo].[sp_Movimento_Venda]

                @Filial          As Varchar(20),

                @DataDe          As Varchar(8),

                @DataAte         As Varchar(8),

                @finalizadora    As varchar(30),
                @plu               As varchar(17),
                @cupom             As varchar(20),
                @pdv               as varchar(10),                
                @horaInicio      as varchar(5),				
				@horafim	     as varchar(5),
				@cancelados		as varchar(5),
				@comanda      as varchar(20)

AS

 

IF(@plu='' AND @cupom='')

      BEGIN

            IF(@finalizadora ='TODOS')

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,lista.EMISSAO,103),

                             lista.PDV,

                             CUPOM = lista.DOCUMENTO,
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
							INNER JOIN  Saida_estoque S  with (index(IX_Movimento_venda_01))  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv	
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
                                   
						GROUP BY lista.Emissao, lista.pdv, lista.Documento ,lista.id_finalizadora, CONVERT(varchar , s.Hora_venda,108) 
						

                  END

            ELSE

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = lista.DOCUMENTO,
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
                             INNER JOIN  Saida_estoque S  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv

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
                        GROUP BY Emissao, PDV, lista.DOCUMENTO ,id_finalizadora,CONVERT(varchar , Hora_venda,108)

                            

                  END

      END

 

ELSE IF (@plu<>'' AND @cupom='')

BEGIN

      SELECT CUPOM = S.Documento,

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

            FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

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

           

            SELECT CUPOM = S.Documento,

                        DATA = CONVERT(VARCHAR,L.Emissao,103),
					    PDV=convert(varchar,L.pdv) ,

                        'PLU'+S.PLU AS PLU,

                        DESCRICAO = M.Descricao,

                        QTDE=replace(convert(varchar,S.Qtde),'.',','),

                        VLR=replace(convert(varchar,S.vlr),'.',','),

                        [-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),

                        [+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),

                        TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

            FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                         
                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and l.pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                       Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
						union all

                        
						SELECT '',

                        '|-CANCELADO-|',
					     pdv=convert(varchar,L.pdv) ,

                        '|-'+S.PLU+'-|',

                        '|-'+M.Descricao+'-|',

                        Qtde=replace(convert(varchar,S.Qtde),'.',','),

                        Vlr=replace(convert(varchar,S.vlr),'.',','),

                        [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),

                        [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),

                        Total='0,000' 

            FROM Saida_estoque S  with (index(IX_Movimento_venda_01))INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                         
                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is NOT null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and l.pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                        union all

                        select '','','','','|-'+ id_finalizadora+'-|'  ,'','','','', '|-'+replace(convert(varchar,(SUM(Lista_Finalizadora.Total))),'.',',')+'-|' 

                             from Lista_finalizadora

                             where  Documento like (case when @cupom <>'TODOS' then @cupom  else '%' end  )

                             and Emissao BETWEEN @DataDe  AND  @DataAte

                             And Isnull(Cancelado,0) = 0

                             and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END










IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_Cadastro_Mercadoria]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Cons_Cadastro_Mercadoria]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria]
    @Filial            Varchar(20),
    @TipoCadastro    Int = 0,
    @Alterados        int = 0
AS
    Declare @StringSQL    AS nVarChar(max)
    Declare @StringSQL2 As nVarChar(max)
    Declare @Where        As nVarChar(max)
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
	DELETE FROM BUSCA_PRECO;
	insert into Busca_Preco 
	SELECT DISTINCT  PLU,Descricao_resumida,PRECO = CASE WHEN Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND Data_Fim <= CONVERT(VARCHAR,GETDATE(),102) THEN Preco_Promocao ELSE PRECO END  
	FROM (

	SELECT [plu] = CONVERT(BIGINT, MERCADORIA.PLU), Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim  
		FROM Mercadoria INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU  
		WHERE ISNULL(Inativo, 0) <= 0    AND MERCADORIA_LOJA.Filial = @Filial

	UNION ALL 


	SELECT [plu] = CONVERT(BIGINT, EAN.EAN),Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim FROM Mercadoria  INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU    INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU 
	 WHERE ISNULL(Inativo, 0) <= 0   AND MERCADORIA_LOJA.Filial = @Filial
	   
	 GROUP BY CONVERT(BIGINT, EAN.EAN),Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim 
	 HAVING CONVERT(BIGINT, EAN.EAN) <=99999999999999 
	) A
	GROUP BY PLU,Descricao_resumida,CASE WHEN Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND Data_Fim <= CONVERT(VARCHAR,GETDATE(),102) THEN Preco_Promocao ELSE PRECO END ;


    
    
    SET NOCOUNT ON;

    SET @StringSQL = ''
    SET @StringSQL2 = ''



    SET @StringSQL = 'SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, MERCADORIA.PLU), EAN.EAN , '
        + '    descricao=isnull(Mercadoria.descricao_Resumida,Mercadoria.descricao), Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
        + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
        + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica, ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
        + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
        + '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
        + '	  ,CST=ISNULL(tributacao.indice_st,'+ char(39) + char(39) +')'
        + '   ,und '+
        + '   ,CST_PIS_COFINS =Mercadoria.cst_saida,pis_perc_saida=ISNULL(Mercadoria.pis_perc_saida,0),cofins_perc_saida=ISNULL(Mercadoria.cofins_perc_saida,0) '+
        + '   ,Origem = ISNULL(mercadoria.Origem,'+ char(39) + char(39) +')'
        + '   ,CEST =ISNULL(mercadoria.CEST,'+ char(39) + char(39) +')'
        + '   ,mercadoria_loja.preco_atacado '
        + '   ,mercadoria_loja.qtde_atacado'
        + '   ,mercadoria.embalagem '
        + '   ,mercadoria.Porcao '+
        + '    FROM Mercadoria '
        + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
        --+ '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
        + ' LEFT OUTER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
        + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
        + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
        + ' WHERE ISNULL(Inativo, 0) <= 0  '

    IF @TipoCadastro < 100
    BEGIN
            SET @Where = ' '
        END
    ELSE
        BEGIN
            SET @Where = ' AND Peso_Variavel.Codigo > 0 '
        END
    IF @Alterados =1
        begin
            SET @Where = @Where+ ' AND estado_mercadoria=1'
        end

    SET @Where = @Where+ ' AND MERCADORIA_LOJA.Filial = ' + CHAR(39) + @Filial + CHAR(39)

    -- Adicionar o EAN
    IF @TipoCadastro < 100
        BEGIN
            SET @StringSQL2 = ' UNION ALL SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, EAN.EAN), EAN.EAN, '
                + '    descricao=isnull(Mercadoria.descricao_Resumida,Mercadoria.descricao), Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
                + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
                + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica,  ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
                + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
				+ '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
				+ '	  ,CST=ISNULL(tributacao.indice_st,'+ char(39) + char(39) +')'
				+ '   ,und '+
				+ '   ,CST_PIS_COFINS =Mercadoria.cst_saida,pis_perc_saida=ISNULL(Mercadoria.pis_perc_saida,0),cofins_perc_saida=ISNULL(Mercadoria.cofins_perc_saida,0) '+
				+ '   ,Origem = ISNULL(mercadoria.Origem,'+ char(39) + char(39) +')'
				+ '   ,CEST =ISNULL(mercadoria.CEST,'+ char(39) + char(39) +')'
				+ '   ,mercadoria_loja.preco_atacado '
				+ '   ,mercadoria_loja.qtde_atacado'
				+ '   ,mercadoria.embalagem '
				+ '   ,mercadoria.Porcao '+
				+ '    FROM Mercadoria '
                + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
                + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        --        + '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
                + ' INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
                + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
                + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
                + ' WHERE ISNULL(Inativo, 0) <= 0 '

        END


    SET @StringSQL = @StringSQL + @Where + @StringSQL2 +@Where +' ORDER BY 1, 2' 
		
    --PRINT (@StringSQL)
       
    
    EXEC(@StringSQL)
END 



 

 
 go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fin_Ficha_Cliente_Pedido]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_fin_Ficha_Cliente_Pedido]
end
GO
--PROCEDURES =======================================================================================
CREATE    procedure [dbo].[sp_rel_fin_Ficha_Cliente_Pedido](
@filial varchar(20),
@datade varchar(8),
@dataate varchar(8),
@tipo varchar(50),
@Codigo_cliente varchar(50),
@status varchar(10),
@TipoRec varchar(20),
@AVista Varchar(10),
@simples    as varchar(5)     
)
--sp_rel_fin_Ficha_Cliente_Pedido 'MATRIZ', '20150101', '20161231', 'EMISSAO', '2369', '', '', '', ''
As

Declare @String as nvarchar(4000)
Declare @Where as nvarchar(4000)

SET @AVista = CASE
WHEN @AVista = 'AVISTA' THEN 'AV'
WHEN @AVista = 'PRAZO' THEN 'PZ'
ELSE ''
END
Begin
--Monta Clausula Where da Procura
set @where = 'Where Conta_a_receber.Filial = '+ char(39) + @filial + char(39) + ' and '
set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
--Verifica se o Parametro @fornecedor tem conteudo
if len(rtrim(ltrim(@Codigo_cliente))) > 0
Begin
set @where = @where + ' And cliente.Codigo_Cliente = '+ char(39) + @Codigo_Cliente + char(39) 
End

if LEN(@status)>0
begin
set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' Conta_a_receber.status =1'
WHEN @STATUS='CONCLUIDO' THEN ' Conta_a_receber.status =2'
WHEN @STATUS='CANCELADO' THEN ' Conta_a_receber.status =3'
WHEN @STATUS='LANCADO' THEN ' Conta_a_receber.status =4'
ELSE  ' and Conta_a_receber.status =1' end --'status like '+CHAR(39)+'%'+CHAR(39) END

)

end
if LEN(@status)=0
begin
set @Where = @Where + ' and Conta_a_receber.status <>3'

end

IF LEN(@TipoRec) >0
begin
SET @Where = @Where + ' and Conta_a_receber.Tipo_Recebimento like '+ CHAR(39) + '%' + @TipoRec + '%' + CHAR(39)
end

IF LEN(@AVista ) >0
begin
SET @Where = @Where + ' and CASE WHEN ISNULL(dbo.Tipo_Pagamento.A_Vista, 1) = 1 THEN ' + CHAR(39) + 'AV' +CHAR(39) + ' ELSE ' + CHAR(39) + 'PZ' + CHAR(39) + ' END = ' + CHAR(39) + @AVista + CHAR(39)
end

SET @Where = @Where + ' AND PD.Tipo =1 AND Pd.Status in (1,2) And CONVERT(VARCHAR, PD.pedido_simples) LIKE case when ' + CHAR(39) + @simples + CHAR(39) + ' = '+ CHAR(39) + 'SIM' + CHAR(39) + ' THEN  '+ CHAR(39) +  '1' + CHAR(39) + '  ELSE case when ' + CHAR(39) + @simples + CHAR(39) + ' = '+ CHAR(39) + 'NAO' + CHAR(39) + ' THEN  '+ CHAR(39) +  '0' + CHAR(39) + ' ELSE ' + CHAR(39) +  '%' + CHAR(39) + 'END END '

SET @Where = @Where + ' AND LEN(Ltrim(Rtrim(Conta_a_receber.Documento))) > 3 ' 



--Monta Select
set @string = 'select
Simples_Cliente = Convert(varchar(200),Ltrim(Rtrim(Case When Isnull(PD.pedido_simples,0) = 1 Then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END ' + ' + ' + CHAR(39) + SPACE(1) + CHAR(39) + ' + ' + 'Ltrim(Rtrim(cliente.Codigo_Cliente))' + ' + ' + CHAR(39) + '  -  ' + CHAR(39) + '+' + 'Ltrim(Rtrim(cliente.nome_Cliente))))) ' + ', 
Documento = rtrim(ltrim(documento)),
Codigo = Convert(int,Ltrim(Rtrim(cliente.Codigo_Cliente))),
Cliente = Substring(Ltrim(Rtrim(cliente.nome_Cliente)),1,1),
Nome = ' + CHAR(39) + 'VALOR TOTAL DO CLIENTE ' + CHAR(39) + ',
VlrReceber = Isnull(Conta_a_receber.Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0),
VlrAberto = case WHEN conta_a_Receber.status = 1 then Isnull(Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0) - Isnull(Conta_a_receber.Valor_Pago,0) else 0 end,
convert(varchar ,emissao,103) as Emissao,
convert(varchar ,vencimento,103) as Vencimento,
Prazo = Convert(Varchar,DATEDIFF(DAY,GETDATE(), vencimento )) ,
TipoRecebimento = ISNULL(Conta_a_Receber.Tipo_Recebimento,' + CHAR(39) + CHAR(39) + ') ' +
' into #t from dbo.Conta_a_receber Conta_a_receber INNER JOIN Cliente ON conta_a_receber.codigo_cliente = cliente.codigo_cliente LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente ON Conta_a_receber.id_cc = Conta_corrente.id_cc LEFT OUTER JOIN dbo.Centro_Custo Centro_custo ON Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND Conta_a_receber.Filial = Centro_custo.filial
--Inner Join Pedido PD on Conta_a_receber.Codigo_Cliente = PD.Cliente_Fornec And Substring(Conta_a_receber.Documento,2,LEN(PD.Pedido)) = PD.Pedido And Conta_a_receber.Emissao = PD.Data_cadastro      
Inner Join Pedido PD on Conta_a_receber.Codigo_Cliente = PD.Cliente_Fornec And Substring(Conta_a_receber.Documento,2,LEN(PD.Pedido)) = PD.Pedido And Conta_a_receber.Emissao = PD.Data_cadastro      
LEFT OUTER JOIN dbo.Tipo_Pagamento on Tipo_Pagamento.Tipo_Pagamento = Conta_a_Receber.Tipo_Recebimento
'+@where+' Order By cliente.nome_Cliente, cliente.Codigo_Cliente, PD.pedido_simples '--'+@where+' Order By '+ @Tipo + ', Fornecedor, Documento '

--Set @String = @string + 'insert into #t select Simples_Cliente, ' + CHAR(39) + CHAR(39) + ',Codigo,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Simples_Cliente,Codigo,Nome order by 3 '
Set @String = @string + 'insert into #t select Simples_Cliente + ' + CHAR(39) + ' - Sub Total' + CHAR(39) + ', ' + CHAR(39) +  CHAR(39) + ',Codigo,cliente,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Simples_Cliente,Codigo,Nome,Cliente order by 3 '
Set @String = @string + 'insert into #t select Nome, ' + CHAR(39) + CHAR(39) + ',Codigo,Cliente,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Codigo,Nome,Cliente order by 3 '
Set @String = @string + 'insert into #t select ' + CHAR(39) + 'TOTAL GERAL - FICHA DE CLIENTE' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' +  CHAR(39) + '999999' + CHAR(39) + ',' + CHAR(39) + 'Z' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + 'SUM(VlrReceber),Sum(VlrAberto),' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t WHERE NOME = '+ CHAR(39) + 'VALOR TOTAL DO CLIENTE' + CHAR(39) 
Set @String = @string + 'Select Simples_Cliente , Documento, VlrReceber, VlrAberto, Emissao, Vencimento, Convert(Varchar,Prazo) Prazo, TipoRecebimento, Convert(int,Codigo) Codigo, Cliente From #t Where Simples_Cliente <> ' + CHAR(39) + CHAR(39) + ' Order by 10,9,1,2,3'
--Union Select ' + CHAR(39) + 'TOTAL CLIENTE' + CHAR(39) + ' + ' + 'Ltrim(Rtrim(cliente.Codigo_Cliente))' + ' + ' + CHAR(39) + '  -  ' + CHAR(39) + '+' + 'Ltrim(Rtrim(cliente.nome_Cliente)))) ' + ', ' + CHAR(39) + CHAR(39) + ',sum(VlrReceber), SUM(VlrAberto),'+ CHAR(39) + CHAR(39) + ','+ CHAR(39) + CHAR(39) + ','+ CHAR(39) + CHAR(39) + ','+ CHAR(39) + CHAR(39) + ', Codigo from #t group by Codigo Order by Simples_Cliente '
-- set @string = @string + 'Documento = rtrim(ltrim(documento)), '
--set @string = @string + 'Fornecedor = rtrim(ltrim(fornecedor)), '
--set @string = @string + @tipo + '= '+ @tipo + ', '
--set @string = @string + 'Total = valor - isnull(desconto,0), '
--set @string = @string + 'Status = case when status = 1 then '+ char(39) +' ABERTO' + char(39)
--set @string = @string + 'when status = 2 then '+ char(39) +'CONCLUÍDO'+ char(39)
--set @string = @string + 'WHEN status = 3 then '+ char(39) +'CANCELADO'+ char(39)
--set @string = @string + 'WHEN status = 4 then '+ char(39) +'LANÇADO'+ char(39) + 'End '
--set @string = @string + 'From Conta_a_pagar '
--set @string = @string + @where
--set @string = @string + ' Order By convert('+ @Tipo +',102) '
--Print @string
Exec(@string)
End



go 

insert into Versoes_Atualizadas select 'Versão:1.143.576', getdate();
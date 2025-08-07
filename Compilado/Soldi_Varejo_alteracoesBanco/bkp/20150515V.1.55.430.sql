
--=========================================================================================================================================================

ALTER  PROCEDURE [dbo].[sp_Rel_Venda_pedido_simplificado] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cliente   as varchar(40),
		@simples	as varchar(5)
		as
			-- exec sp_rel_venda_pedido_simplificado 'MATRIZ','20141201','20141220','',''
	
if len(@cliente)>0
begin

	SELECT  P.PLU ,M.Descricao, QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),VALOR=ROUND(SUM(p.total),2),UNITARIO = ROUND( UNITARIO ,2)
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')   )   
	And PD.Data_cadastro between @DataDe and @DataAte
	GROUP BY P.PLU,M.Descricao,P.unitario
	UNION ALL
	SELECT '','TOTAL',QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),VALOR=ROUND(SUM(p.total),0),0 	 
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')    )
	And PD.Data_cadastro between @DataDe and @DataAte
	--GROUP BY P.PLU,M.Descricao,P.unitario	
	
end
else		
begin
		Select Simples = case when Pedido.pedido_simples=1 then 'SIM'ELSE 'NAO' END ,Pedido,convert(varchar,pedido.Data_cadastro,103) Data, Cliente_Fornec Cod,cliente.Nome_Cliente,Total=ROUND(Total,2) 
		 from pedido inner join cliente on pedido.Cliente_Fornec= cliente.Codigo_Cliente 
		 where  CONVERT(VARCHAR, Pedido.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
		  and pedido.Data_cadastro between @DataDe and @DataAte
		  AND Pedido.Tipo =1 
	  
		
		 UNION ALL
		 SELECT '', '','','','TOTAL',round(SUM(TOTAL),2)
		 from pedido inner join cliente on pedido.Cliente_Fornec= cliente.Codigo_Cliente 
		 where  CONVERT(VARCHAR, Pedido.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
		  and pedido.Data_cadastro between @DataDe and @DataAte
		  AND Pedido.Tipo =1 
	  
		 
 end
GO

--=========================================================================================================================================================

GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_NotasFiscais2]    Script Date: 05/15/2015 11:25:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--sp_Rel_NotasFiscais 'matriz','20150513','20150513','','','','NAO'

ALTER  procedure [dbo].[sp_Rel_NotasFiscais]

     

      @Filial           varchar(20),                -- Loja

      @DataDe           varchar(8),                 -- Data Inicial

      @DataAte          varchar(8),                 -- Data Fim

      @Tipo             varchar(20),                -- Tipo = 1 - Saidas, Tipo = 2 - Entrada

      @Nota             varchar(20),                -- Nmero Nota Fiscal

      @Fornecedor       varchar(20),                -- Nome Fornecedor
      
      @NF_CANC          varchar(20)		    -- Normal = NAO, Cancelada = SIM
 

As

      DECLARE @NF varchar(5000)

     

      SET @NF = 'SELECT NF.Filial, NF.Codigo ' + 'Nota' + ', (Case When NF.Tipo_NF = 1 Then (Select Distinct Ltrim(Rtrim(Nome_Cliente)) From Cliente c where c.Codigo_Cliente = NF.Cliente_Fornecedor) Else  NF.Cliente_Fornecedor End) ' + 'Fornecedor' + ', Convert(Varchar,NF.Emissao,103) ' + 'Emisso'

      SET @NF = @NF + ', Convert(Varchar,NF.Data,103) ' + 'Entrada' + ',Convert(decimal(15,2),Sum(NF_Item.Total+Isnull(NF.Frete,0))) ' + 'VlrProd' + ', NF.Total ' + 'VlrNota'

      SET @NF = @NF + ' From NF_Item Inner Join NF ON NF.Codigo = NF_Item.Codigo '

      SET @NF = @NF + 'AND NF.Cliente_Fornecedor = NF_Item.Cliente_Fornecedor AND NF.Filial = NF_Item.Filial '

      SET @NF = @NF + 'WHERE NF.Data BETWEEN ' + CHAR(39) +  convert(varchar,@DataDe,112) + CHAR(39) + ' AND '

      SET @NF = @NF + CHAR(39) + convert(varchar,@DataAte,112)  + char(39) + ' '
      
      
      --Exibe todas as notas fiscais (canceladas)
      IF LTRIM(RTRIM(@NF_CANC)) = 'SIM'

            SET @NF = @NF + 'AND Isnull(NF.NF_Canc,0) = 1 '
            

      --Exibe todas as notas fiscais (exceto canceladas)     
      IF LTRIM(RTRIM(@NF_CANC)) = 'NAO'

            SET @NF = @NF + 'AND Isnull(NF.NF_Canc,0) = 0 '
      

      --Verifica se sera aplicado filtro por Filial

      IF LTRIM(RTRIM(@Filial)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Filial) = ' + CHAR(39) + @Filial + CHAR(39) + ' '

     

      --Verifica se sera aplicado filtro por Fornecedor

      IF LTRIM(RTRIM(@Fornecedor)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Cliente_Fornecedor) = ' + CHAR(39) + @Fornecedor + CHAR(39) + ' '

 

      --Verifica se sera aplicado filtro por Nota

      IF LTRIM(RTRIM(@Nota)) <> ''

            SET @NF = @NF + 'AND NF.Codigo = ' + CAST(@Nota as Varchar) + ' '

 

      ----Verifica se sera aplicado filtro por Tipo da Nota Fiscal

      IF LTRIM(RTRIM(@Tipo)) = '1-Saida'

            SET @NF = @NF + 'AND NF.Tipo_NF = 1 '

     

      IF LTRIM(RTRIM(@Tipo)) = '2-Entrada'

            SET @NF = @NF + 'AND NF.Tipo_NF = 2 '

 

      SET @NF = @NF + 'Group by NF.Filial, NF.Codigo, NF.Cliente_Fornecedor, NF.Emissao, NF.data, NF.Tipo_NF, NF.Total '

      SET @NF = @NF + 'Order By NF.Filial, NF.Entrada '

     

    exec(@NF)

--print @nf


--=========================================================================================================================================================

go 

CREATE NONCLUSTERED INDEX [index_historico_comanda] ON [dbo].[Comanda_item] 
(
	[filial] ASC,
	[comanda] ASC,
	[data] ASC,
	[hora_evento] ASC,
	[usuario] ASC,
	[data_cancelamento] ASC,
	[cupom] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

--=========================================================================================================================================================

go 

CREATE PROCEDURE SP_REL_COMANDA_HISTORICO (
		@filial varchar(30),
		@comanda varchar(20),
		@dataInicio VARCHAR(8),@dataFim VARCHAR(8), 
		@horaInicio time ,@horafim time,
		--@status varchar(10),
		@usuario varchar(20),
		@cancelamento varchar(4),
		@finalizado varchar(4))
		AS 
/*
set @comanda='1144'
set @dataInicio='20150501' 
set @datafim = '20150514'

set @horaInicio='00:00' 
set @horafim='23:59' 
--set @status =''
set @usuario =''
set @cancelamento=''
set @finalizado =''
*/		
--sp_help comanda_item
--select * from Comanda_Item where data between '20150514' and '20150515'
select i.comanda, 
	   data=convert(varchar,i.data,103),
	   hora = convert(varchar,i.hora_evento,108),
	 --  Status = case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END,
	   i.usuario,
	   i.plu,
	   m.descricao,
	   i.qtde,
	   i.unitario,
	   i.total,
	   Cancelado=case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END,	 
	   Finalizado=case when i.data_cancelamento IS NOT NULL then '---' else case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END end	 
	     
	    
from Comanda_Item i  WITH (INDEX(index_historico_comanda)) inner join mercadoria m on i.plu=m.PLU
where  i.filial = @filial   
	  and i.comanda like case when LEN(@comanda)>0 then @comanda else '%'end
	  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
	  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
	  and i.usuario like case when LEN(@usuario)>0 then @usuario else '%' end 
	  and (case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END = @cancelamento
			OR LEN(@cancelamento)=0 )
	--  AND ( case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END=@status
	--		OR LEN(@status)=0)
	  And( case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END = @finalizado
			OR LEN(@finalizado)=0)
order by i.hora_evento desc 
	  
	  	  	  

GO

--=========================================================================================================================================================

GO 
ALTER Procedure [dbo].[sp_Movimento_Venda]
                @Filial          As Varchar(20),
                @DataDe          As Varchar(8),
                @DataAte         As Varchar(8),
                @finalizadora    As varchar(30),
                @plu			 As varchar(17),
                @cupom			 As varchar(20),
                @pdv			 as varchar(2)
AS

IF(@plu='' AND @cupom='')
	BEGIN
		IF(@finalizadora ='')
			BEGIN
				SELECT 
					DATA = CONVERT(VARCHAR,EMISSAO,103), 
					PDV, 
					CUPOM = DOCUMENTO, 
					VLR = SUM(Lista_Finalizadora.Total - ISNULL(Lista_Finalizadora.Troco, 0)), 
					FINALIZADORA = id_finalizadora 
				FROM 
					Lista_finalizadora 
					INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora  
				WHERE Lista_Finalizadora.Filial = @FILIAL AND (Emissao BETWEEN @DataDe  AND  @DataAte )
						 and pdv like (case when @pdv <> '' then @pdv else '%' end)
				GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora
		 
			END
		ELSE
			BEGIN
				SELECT 
					DATA = CONVERT(VARCHAR,EMISSAO,103), 
					PDV, 
					CUPOM = DOCUMENTO, 
					VLR = SUM(Lista_Finalizadora.Total - ISNULL(Lista_Finalizadora.Troco, 0)), 
					FINALIZADORA = id_finalizadora 
				FROM 
					Lista_finalizadora 
					INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora  
				WHERE Lista_Finalizadora.Filial = @FILIAL AND (Emissao BETWEEN @DataDe  AND  @DataAte )
				AND finalizadora.Finalizadora  = @finalizadora  
				 and pdv like (case when @pdv <> '' then @pdv else '%' end)
				GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora
					
			END
	END

ELSE IF (@plu<>'' AND @cupom='')
BEGIN
	SELECT S.Documento,
				Emissao = CONVERT(VARCHAR,L.Emissao,103),
				pdv=convert(varchar,L.pdv) ,
				S.PLU,
				M.Descricao,
				Qtde=convert(varchar,S.Qtde),
				Vlr=convert(varchar,S.vlr),
				[-Desconto]=convert(varchar,isnull(s.desconto,0)),
				[+Acrescimo]=convert(varchar,isnull(s.Acrescimo,0)),
				Total=convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0))  
		FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento
					INNER JOIN Mercadoria M ON S.PLU = M.PLU 	
				where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
				 and s.PLU like (case when @plu <>'' then @plu else '%' end ) 
				 and l.Emissao BETWEEN @DataDe  AND  @DataAte 
				 and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
				 Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo
	END
ELSE
	BEGIN
		
		SELECT S.Documento,
				Emissao = CONVERT(VARCHAR,L.Emissao,103),
				pdv=convert(varchar,L.pdv) ,
				S.PLU,
				M.Descricao,
				Qtde=convert(varchar,S.Qtde),
				Vlr=convert(varchar,S.vlr),
				[-Desconto]=convert(varchar,isnull(s.desconto,0)),
				[+Acrescimo]=convert(varchar,isnull(s.Acrescimo,0)),
				Total=convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0))  
		FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento
					INNER JOIN Mercadoria M ON S.PLU = M.PLU 	
				where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
				 and s.PLU like (case when @plu <>'' then @plu else '%' end ) 
				 and l.Emissao BETWEEN @DataDe  AND  @DataAte 
				 and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
				 Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo
				union all
				select '','','','', id_finalizadora  ,'','','','', convert(varchar,(SUM(Lista_Finalizadora.Total - ISNULL(Lista_Finalizadora.Troco, 0)))) 
					from Lista_finalizadora 
					where  Documento like (case when @cupom <>'' then @cupom  else '%' end  )
					and Emissao BETWEEN @DataDe  AND  @DataAte 
					and pdv like (case when @pdv <> '' then @pdv else '%' end)
				 GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora
				
	END

go
--=====================================================================================================================================
go
/****** Object:  Index [index_item_agregado]    Script Date: 05/18/2015 16:37:54 ******/
CREATE NONCLUSTERED INDEX [index_item_agregado] ON [dbo].[Saida_estoque] 
(
	[Filial] ASC,
	[Documento] ASC,
	[PLU] ASC,
	[Data_movimento] ASC,
	[Hora_venda] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
--====================================================================================================================================
go 

create procedure   sp_rel_venda_itens_agregado   @Filial   As Varchar(20),
					@DataDe          As Varchar(8),
					@DataAte         As Varchar(8),
					@horaInicio      as time,
					@horafim	     as time,
					@plu			as varchar(20) 
	as 
/*
set @plu='4'
set @Filial='MATRIZ'
set @DataDe='20150514' 
set @DataAte = '20150514'
set @horaInicio='08:00'
set @horafim = '10:00'
*/
-- select top 100 * from saida_estoque 
SELECT s.PLU,m.descricao, Qtde= SUM(s.qtde),Unitario = m.Preco,Total = sum(vlr) 
	FROM Saida_estoque s with (index(index_item_agregado))  inner join mercadoria m on s.PLU=m.PLU
where s.Filial=@Filial and  Documento in (
		select Documento 
		from Saida_estoque 
		where Filial=@Filial and  PLU  = @plu and Data_movimento between @DataDe and @DataAte and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim
		) 
		and s.PLU<> @plu 
		and s.Data_movimento between @DataDe and @DataAte 
		and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
group by s.PLU,m.descricao,m.Preco
order by SUM(qtde) desc				 



go

--==========================================


CREATE NONCLUSTERED INDEX [index_comanda_status] ON [dbo].[Comanda_controle] 
(
	[comanda] ASC,
	[status] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

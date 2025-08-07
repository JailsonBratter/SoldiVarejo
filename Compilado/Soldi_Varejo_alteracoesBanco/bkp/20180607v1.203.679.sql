/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas]    Script Date: 06/07/2018 10:28:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Resumo_Vendas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Resumo_Vendas]
GO

/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas]    Script Date: 06/07/2018 10:28:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--PROCEDURES =======================================================================================
CREATE    Procedure [dbo].[sp_Rel_Resumo_Vendas]
            @Filial		  As Varchar(20),
            @DataDe		  As Varchar(8),
            @DataAte	  As Varchar(8),
            @plu		  As Varchar (20),
            @descricao	  As varchar(50),
            @grupo	      As Varchar(50),
            @subGrupo	  As varchar(50),
            @departamento as varchar(50),
            @relatorio	  as varchar(40)

 

AS
-- EXEC sp_Rel_Resumo_Vendas 'MATRIZ','20161101','20161130','','','','','','PEDIDO SIMPLES'
-- @relatorio = TODOS ,CUPOM , NOTA SAIDA, PEDIDO SIMPLES 	
	IF OBJECT_ID(N'tempdb..#tmpVendas', N'U') IS NOT NULL   
	begin
		DROP TABLE #tmpVendas;  
	end
CREATE TABLE #tmpVendas
(
	Data varchar(30),
	Dia_Semana varchar(30),
	Venda Decimal(18,2),
	Clientes int,
	Venda_MD  Decimal (12,2),
	NFP INT,
	Vlr_NFP Decimal(18,2),
	Perc_NFP Decimal(8,2)
)

declare @ini_periodo varchar(5),@fim_periodo varchar(5), @dias_periodo int
select @ini_periodo = inicio_periodo
	 , @fim_periodo =fim_periodo
	 , @dias_periodo = dias_periodo 
		
from filial where filial = @filial
		



if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN
SELECT
            saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            Vlr = Convert(Decimal(15,2),SUM(isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0))),
            CPF_CNPJ,
			hora_venda
		  INTO
				#Lixo
      FROM
            Saida_Estoque with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria  as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  

            
      WHERE
            saida_estoque.Filial = @Filial
      AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
      and   hora_venda between '00:00:00' and '23:59:59'
      AND   Data_Movimento BETWEEN @DataDe AND dateadd(day,@dias_periodo,@DataAte)
      AND   Data_Cancelamento IS NULL
      AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
      
      GROUP BY
           Saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            CPF_CNPJ,
			Hora_venda;

  

insert into #tmpVendas
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

      Venda =       (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)  ),
      Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ),
      Venda_MD =    CONVERT(DECIMAL(12,2), 
						CASE WHEN (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ) > 0 THEN

                  ((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ) /  (SELECT COUNT(*) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ))

                  ELSE 0 END),

      NFP =         (SELECT COUNT(*) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> '' and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)  ),

      Vlr_NFP =     ISNULL((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)AND (#lixo.CPF_CNPJ <> '')  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)  ),0),

     

      Perc_NFP =    CASE WHEN (SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo AND #lixo.CPF_CNPJ <> '' )  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)  ) > 0 THEN

                  CONVERT(DECIMAL(8,2), ((SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo AND #lixo.CPF_CNPJ <> '')  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)  ) /

                        (SELECT Convert(Decimal(18,2),SUM(#Lixo.Vlr)) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) )) * 100)

                  ELSE 0 END
	 
      FROM

            Saida_Estoque  with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  


      WHERE

            Saida_Estoque.Filial = @Filial

    

	  AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
	  AND (Data_Movimento BETWEEN @DataDe AND @DataAte)
	  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
	   
      GROUP BY

            Data_Movimento
 END

if(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
BEGIN
insert into #tmpVendas
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
      SUM(ni.TOTAL-(isnull(ni.Total,0)*isnull(ni.desconto,0)/100)) AS Venda,
      
      (
		  	Select COUNT(*) 
		from nf 
			inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 

		where NF.FILIAL=@filial 
				and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
				and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') 
				AND  (NF.Emissao= n.Emissao )
				AND   NF.TIPO_NF = 1 
				AND ISNULL(NF.nf_Canc,0)=0	
				and nf.status='AUTORIZADO'
				AND (NF.Codigo IN (SELECT DISTINCT CODIGO 
											FROM NF_Item as	nii										
											inner join mercadoria  as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO as cdi  ON cdi.codigo_departamento = mi.Codigo_departamento  
											

											 WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
										)
							)
      ) 
      as Clientes ,
      (
      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM nf WHERE nf.Emissao = n.Emissao) > 0 THEN

                  (
                  Select isnull(SUM(nii.TOTAL-(isnull(nii.Total,0)*isnull(nii.desconto,0)/100)),0) 
					from nf 
					inner join nf_item as nii on NF.codigo=nii.codigo and NF.Filial=nii.Filial and NF.Tipo_NF = nii.Tipo_NF
					inner join mercadoria as mi on mi.PLU = nii.PLU
					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
					inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 
					where NF.FILIAL=@filial 
							and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
							 AND (NF.Emissao= n.Emissao )
							 AND NF.TIPO_NF = 1
							 and nf.status='AUTORIZADO'
							 AND (LEN(@PLU)=0 OR nii.PLU = @plu)
							 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
							 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
							 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
							) 
							 /   
					(	
					Select COUNT(*) 
					from nf 
					inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 
					where NF.FILIAL=@filial 
								AND  (NF.Emissao= n.Emissao )
								AND   NF.TIPO_NF = 1	
								and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
								and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
								and nf.status='AUTORIZADO'
								AND (
						NF.Codigo IN (SELECT DISTINCT CODIGO 
											FROM NF_Item as nii  inner join mercadoria as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
											 WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
											
													  )
									)
					)

                  ELSE 0 END)
                  
                  
      ) as [Venda_MD],
      0 as [NFP],
      0 AS [Vlr NFP],
      0 as [Perc NP]

from  NF as N
inner join nf_item as ni on ni.codigo=n.codigo and ni.Filial=n.Filial and n.Tipo_NF = ni.Tipo_NF
inner join mercadoria as m on m.PLU = ni.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
inner join Natureza_operacao as np on n.Codigo_operacao = np.Codigo_operacao 
WHERE  N.FILIAL=@filial 
AND  (N.Emissao BETWEEN @DataDe AND @DataAte)
	 and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
	 and n.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
	 and n.status='AUTORIZADO'						
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
	 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY N.Emissao 
END


if(@relatorio='TODOS' OR @relatorio='PEDIDO SIMPLES')
BEGIN
insert into #tmpVendas
select 
	CONVERT(VARCHAR,p.Data_cadastro,103) AS DATA ,
	  Dia_Semana = CASE
            WHEN DATEPART(dw, p.Data_cadastro) = 1 THEN 'DOMINGO'
            WHEN DATEPART(dw, p.Data_cadastro) = 2 THEN 'SEGUNDA'
            WHEN DATEPART(dw, p.Data_cadastro) = 3 THEN 'TERÇA'
            WHEN DATEPART(dw, p.Data_cadastro) = 4 THEN 'QUARTA'
            WHEN DATEPART(dw, p.Data_cadastro) = 5 THEN 'QUINTA'
            WHEN DATEPART(dw, p.Data_cadastro) = 6 THEN 'SEXTA'
            WHEN DATEPART(dw, p.Data_cadastro) = 7 THEN 'SABADO'

      END,
      SUM(pit.TOTAL) AS Venda,
      
      (
		  	Select COUNT(*) 
		from pedido  with (index(ix_pedido_fluxo_caixa))
		where pedido.pedido_simples = 1  
				AND  (pedido.Data_cadastro= p.Data_cadastro )
				and isnull(pedido.Status,0)<>3
				AND   pedido.Tipo = 1 
				and pedido.FILIAL=@filial 
				AND (pedido.pedido IN (SELECT DISTINCT pedido 
											FROM pedido_Itens as	nii										
											inner join mercadoria  as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO as cdi  ON cdi.codigo_departamento = mi.Codigo_departamento  
											WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
										)
							)
      ) 
      as Clientes ,
      (
      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM pedido WHERE pedido.Data_cadastro = p.Data_cadastro) > 0 THEN

                  (
                  Select isnull(SUM(pii.total),0) 
					from pedido with (index(ix_pedido_fluxo_caixa))
					inner join pedido_itens as pii on pedido.pedido=pii.Pedido and pedido.Filial=pii.Filial and pedido.Tipo = pii.Tipo
					inner join mercadoria as mi on mi.PLU = pii.PLU
					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
					where pedido.FILIAL=@filial 
							 AND (pedido.Data_cadastro= p.Data_cadastro)
							 AND pedido.TIPO = 1
							 and pedido.pedido_simples = 1
							 AND (LEN(@PLU)=0 OR pii.PLU = @plu)
							 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
							 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
							 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
							) 
							 /   
					(	
					Select COUNT(*) 
					from pedido with (index(ix_pedido_fluxo_caixa))
					where pedido.FILIAL=@filial 
								AND  (pedido.Data_cadastro= p.Data_cadastro )
								AND   pedido.TIPO = 1	
								and pedido.pedido_simples = 1
								
								AND (
						pedido.pedido IN (SELECT DISTINCT pedido 
											FROM pedido_Itens as pii  inner join mercadoria as mi on mi.PLU = pii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
											 WHERE  (LEN(@PLU)=0 OR pii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
											
													  )
									)
					)

                  ELSE 0 END)
                  
                  
      ) as [Venda_MD],
      0 as [NFP],
      0 AS [Vlr NFP],
      0 as [Perc NP]

from  Pedido as p with (index(ix_pedido_fluxo_caixa))
inner join Pedido_itens  as pit with (index(ix_sp_rel_resumo_vendas)) on pit.Pedido=p.pedido and pit.Filial=p.Filial and p.Tipo = pit.Tipo
inner join mercadoria as m on m.PLU = pit.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
WHERE p.pedido_simples = 1   
	AND  (p.Data_cadastro BETWEEN @DataDe AND @DataAte)
	 and isnull(p.Status,0)<>3
	 AND   p.TIPO = 1	
	 and p.FILIAL=@filial 
	 AND (LEN(@PLU)=0 OR pit.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY p.Data_cadastro
END








 Select  Data,
			Dia_Semana,
			Sum(Venda) as Venda,
			SUM(Clientes) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
	from 
	#tmpVendas
	
	


GROUP BY Data,Dia_Semana
ORDER BY Data ;

--EXEC @SQL;








/****** Object:  StoredProcedure [dbo].[sp_Rel_Mapa_Resumo_SAT]    Script Date: 06/06/2018 12:27:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Mapa_Resumo_SAT]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_Rel_Mapa_Resumo_SAT]
GO


/****** Object:  StoredProcedure [dbo].[sp_Rel_Mapa_Resumo_SAT]    Script Date: 06/06/2018 12:27:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE Procedure [dbo].[sp_Rel_Mapa_Resumo_SAT]

	@Filial		varchar(20),
	@DataDe		varchar(8),
	@DataAte	varchar(8),
	@tipo	    varchar(30),
	@pdv		varchar(10)
	
As

Begin
		-- 

		--set @filial ='MATRIZ'
		--SET @DataDe = '20170720'
		--set @DataAte =@DataDe
		
		--exec     sp_Rel_Mapa_Resumo_SAT    @Filial='MATRIZ', @datade = '20170711',  @dataate = '20170711',  @tipo = 'ANALITICO',@pdv='TODOS' 
		
		declare @ini_periodo varchar(5),@fim_periodo varchar(5), @dias_periodo int
		select @ini_periodo = inicio_periodo
			 , @fim_periodo =fim_periodo
			 , @dias_periodo = dias_periodo 
				
		from filial where filial = @filial
				

		IF OBJECT_ID(N'tempdb..#tbFiscal', N'U') IS NOT NULL  
			DROP TABLE #tbFiscal; 


			
		Select 
			saida_estoque.Filial,
			Data_movimento, 
			Caixa_saida,
			convert(int,coo)  cupom, 
			Vlr = Convert(Decimal(15,2),isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0)) , 
			case when data_cancelamento is not null then Convert(Decimal(15,2),isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0)) else 0 end 'Canc',
			cst = isnull((Select top 1 indice_st from tributacao where tributacao.nro_ecf= saida_estoque.nro_ecf and tributacao.saida_icms = saida_estoque.aliquota_icms ),'90'),
			aliq = aliquota_icms,
			hora_venda
		Into #tbFiscal
		From 
			saida_estoque
			inner join mercadoria  as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento 
		Where saida_estoque.Filial = @Filial
			and   hora_venda between '00:00:00' and '23:59:59'
			And Data_movimento between @DataDe and dateadd(day,@dias_periodo,@DataAte)
			and isnull(coo,0)> 0
			and (@PDV ='TODOS' OR CONVERT(VARCHAR,Caixa_saida) =@pdv)
		Order by 
			saida_estoque.Filial, Data_movimento, Caixa_saida
			
			
		if(@tipo='ANALITICO')
		BEGIN 
		
			select * into #tbFiscalFinal from (	
			Select 
				ordem =convert(varchar,Data_movimento,102)+'01'+CONVERT(VARCHAR,Caixa_saida), 
				Filial,
				[Data Movimento]	= convert(varchar,Data_movimento,103), 
				Caixa				= 'SFT_'+CONVERT(VARCHAR,Caixa_saida),
				[Cupom Inicial]		= 'SFT_'+CONVERT(VARCHAR,min(cupom)), 
				[Cupom Final]		= 'SFT_'+CONVERT(VARCHAR,max(cupom)),  
				[Venda Bruta]		= (Select sum(vlr) 
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Total Canc]		= (Select sum(Canc)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Venda Contabil]	= (Select sum(vlr) - sum(canc) 
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Subs]		= (Select sum(case when cst ='60' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Isento]		= (Select sum(case when cst ='40' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Nao Trib]	= (Select sum(case when cst ='41' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[Total Outros]		= (Select sum(case when cst ='90' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[3,2%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),	
				[7%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[12%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[18%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[25%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where t1.caixa_saida = #tbFiscal.caixa_saida 
										 and (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											))
			From 
				#tbFiscal 
			where Data_movimento between @DataDe and @DataAte
			
			group by Filial, data_movimento, caixa_saida
			union  all 
				Select 
				ordem =convert(varchar,Data_movimento,102)+'00' ,
				'|-TITULO-||CONCAT|'+convert(varchar,Data_movimento,103),
				'', 
				'',
				'', 
				'',  
				0, 
				0, 
				0, 
				0, 
				0, 
				0,
				0,
				0,	
				0,
				0,
				0,
				0
			From 
				#tbFiscal 
			where Data_movimento between @DataDe and @DataAte AND @PDV='TODOS'
			group by data_movimento
			union all 
			Select 
				ordem =convert(varchar,Data_movimento,102)+'99' ,
				'|-SUB-|',
				[Data Movimento]	= '', 
				Caixa				= '',
				[Cupom Inicial]		= '', 
				[Cupom Final]		= '',  
				[Venda Bruta]		= (Select sum(vlr) 
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Total Canc]		= (Select sum(Canc)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Venda Contabil]	= (Select sum(vlr) - sum(canc) 
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Subs]		= (Select sum(case when cst ='60' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Isento]		= (Select sum(case when cst ='40' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Nao Trib]	= (Select sum(case when cst ='41' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[Total Outros]		= (Select sum(case when cst ='90' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[3,2%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),	
				[7%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[12%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[18%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[25%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											))
			From 
				#tbFiscal 
			where Data_movimento between @DataDe and @DataAte AND @PDV='TODOS'
			group by data_movimento
			
			
			
			) as a 
			order by ordem
			
			alter table #tbFiscalFinal drop column ordem
			
			
			
			
			SELECT * INTO #tbFiscalFinal1  FROM (
			Select * from #tbFiscalFinal
			union all 
			Select 
				'|-SUB-|TOTAL:',
				Data				= '',
				Caixa				= '',
				[Cupom Inicial]		= '', 
				[Cupom Final]		= '',  
				[Venda Bruta]		= sum([Venda Bruta]), 
				[Total Canc]		= sum([Total Canc]) , 
				[Venda Contabil]	= sum([Venda Contabil]), 
				[Total Subs]		= sum([Total Subs]), 
				[Total Isento]		= sum([Total Isento]), 
				[Total Nao Trib]	= sum([Total Nao Trib]),
				[Total Outros]		= sum([Total Outros]),
				[3,2%]				= sum([3,2%]),	
				[7%]				= sum([7%]),
				[12%]				= sum([12%]),
				[18%]				= sum([18%]),
				[25%]				= sum([25%])
			From 
				#tbFiscalFinal 
			where filial ='|-SUB-|' OR @PDV <>'TODOS'
			) AS F
			
			
			
			if(@pdv='TODOS')
			BEGIN
				alter table #tbFiscalFinal1 drop column [Data Movimento]
			END 
			
			SELECT * FROM #tbFiscalFinal1
			
			
		END
		ELSE
		BEGIN
			
			Select 
				Filial,
				[Data Movimento]	= convert(varchar,Data_movimento,103), 
				[Venda Bruta]		= (Select sum(vlr) 
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Total Canc]		= (Select sum(Canc)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)) , 
				[Venda Contabil]	= (Select sum(vlr) - sum(canc) 
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Subs]		= (Select sum(case when cst ='60' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Isento]		= (Select sum(case when cst ='40' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)), 
				[Total Nao Trib]	= (Select sum(case when cst ='41' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[Total Outros]		= (Select sum(case when cst ='90' AND canc=0 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[3,2%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=3.20 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),	
				[7%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=7.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[12%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=12.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[18%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=18.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											)),
				[25%]				= (Select sum(case when cst ='00' AND canc=0 and aliq=25.00 then vlr else 0 end)
										from #tbFiscal  as t1 
										where (
											(t1.Data_Movimento = #tbFiscal.Data_Movimento and t1.Hora_venda >= @ini_periodo)  
											or (t1.Data_Movimento = dateadd(day,@dias_periodo,#tbFiscal.Data_Movimento) and t1.Hora_venda <= @fim_periodo)
											))
				
			into #tbFiscal2
			From 
				#tbFiscal
			where Data_movimento between @DataDe and @DataAte
			group by Filial, data_movimento
			
			select * from (
			Select * from #tbFiscal2
			union all
			Select 
				'|-SUB-|TOTAL:',
				[Data Movimento]	= '', 
				[Venda Bruta]		= sum([Venda Bruta]), 
				[Total Canc]		= sum([Total Canc]), 
				[Venda Contabil]	= sum([Venda Contabil]), 
				[Total Subs]		= sum([Total Subs]) , 
				[Total Isento]		= sum([Total Isento]), 
				[Total Nao Trib]	= sum([Total Nao Trib]),
				[Total Outros]		= sum([Total Outros]),
				[3,2%]				= sum([3,2%]),	
				[7%]				= sum([7%]),
				[12%]				= sum([12%]),
				[18%]				= sum([18%]),
				[25%]				= sum([25%])
			From 
				#tbFiscal2

			)as a 
			order by filial desc, convert(varchar,[Data movimento],102) 


		END



End




GO








GO




IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END




go 






insert into Versoes_Atualizadas select 'Versão:1.203.679', getdate();
GO

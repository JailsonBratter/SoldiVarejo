



--PROCEDURES =======================================================================================
ALTER     Procedure [dbo].[sp_Rel_Resumo_Vendas_Mes]
            @Filial           As Varchar(20),
            @DataDe           As Varchar(8),
            @DataAte    As Varchar(8),
            @plu as Varchar (20),
            @descricao as varchar(50),
            @grupo as Varchar(50),
            @subGrupo as varchar(50),
            @departamento as varchar(50),
            @relatorio varchar(40)

 

AS
-- EXEC  sp_Rel_Resumo_Vendas_Mes 'MATRIZ','20180110','20180110','','','','','','TODOS'
	IF OBJECT_ID(N'tempdb..#tmpVendas', N'U') IS NOT NULL   
	begin
		DROP TABLE #tmpVendas;  
	end
CREATE TABLE #tmpVendas
(
	MES varchar(30),
	NOME varchar(30),
	Venda Decimal(18,2),
	Clientes int,
	Venda_MD  Decimal (12,2),
	NFP INT,
	Vlr_NFP Decimal(18,2),
	Perc_NFP Decimal(8,2)
)



if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN

 SELECT
            SAIDA_ESTOQUE.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            Vlr = Convert(Decimal(15,2),SUM(Vlr-isnull(Desconto,0)+isnull(acrescimo,0))),
            CPF_CNPJ
		  INTO
				#Lixo
      FROM
            Saida_Estoque
              inner join mercadoria as m on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  

      WHERE
            SAIDA_ESTOQUE.Filial = @Filial
      AND
            Data_Movimento BETWEEN @DataDe AND @DataAte
      AND
            Data_Cancelamento IS NULL
      and   hora_venda between '00:00:00' and '23:59:59'
       AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
      
      GROUP BY
            SAIDA_ESTOQUE.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            CPF_CNPJ



  insert into #tmpVendas
SELECT

      Mes = 'SFT_' +convert(varchar,DATEPART(m, Data_Movimento))+'/'+convert(varchar,DATEPART(YY, Data_Movimento)),

      Nome = 
		CASE

            WHEN DATEPART(M, Data_Movimento) = 1 THEN 'JANEIRO'

            WHEN DATEPART(M, Data_Movimento) = 2 THEN 'FEVEREIRO'

            WHEN DATEPART(M, Data_Movimento) = 3 THEN 'MARÇO'

            WHEN DATEPART(M, Data_Movimento) = 4 THEN 'ABRIL'

            WHEN DATEPART(M, Data_Movimento) = 5 THEN 'MAIO'

            WHEN DATEPART(M, Data_Movimento) = 6 THEN 'JUNHO'

            WHEN DATEPART(M, Data_Movimento) = 7 THEN 'JULHO'
            WHEN DATEPART(M, Data_Movimento) = 8 THEN 'AGOSTO'
            WHEN DATEPART(M, Data_Movimento) = 9 THEN 'SETEMBRO'
            WHEN DATEPART(M, Data_Movimento) = 10 THEN 'OUTUBRO'
            WHEN DATEPART(M, Data_Movimento) = 11 THEN 'NOVEMBRO'
            WHEN DATEPART(M, Data_Movimento) = 12 THEN 'DEZEMBRO'
      
      
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
            inner join mercadoria as m on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  


      WHERE

            Saida_Estoque.Filial = @Filial

      AND

            Data_Movimento BETWEEN @DataDe AND @DataAte

	  AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
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
	'SFT_' +convert(varchar,DATEPART(m, N.Emissao))+'/'+convert(varchar,DATEPART(YY, N.Emissao))  AS MES ,
	  NOME = CASE

            WHEN DATEPART(M, N.Emissao) = 1 THEN 'JANEIRO'

            WHEN DATEPART(M, N.Emissao) = 2 THEN 'FEVEREIRO'

            WHEN DATEPART(M, N.Emissao) = 3 THEN 'MARÇO'

            WHEN DATEPART(M, N.Emissao) = 4 THEN 'ABRIL'

            WHEN DATEPART(M, N.Emissao) = 5 THEN 'MAIO'

            WHEN DATEPART(M, N.Emissao) = 6 THEN 'JUNHO'

            WHEN DATEPART(M, N.Emissao) = 7 THEN 'JULHO'
            WHEN DATEPART(M, N.Emissao) = 8 THEN 'AGOSTO'
            WHEN DATEPART(M, N.Emissao) = 9 THEN 'SETEMBRO'
            WHEN DATEPART(M, N.Emissao) = 10 THEN 'OUTUBRO'
            WHEN DATEPART(M, N.Emissao) = 11 THEN 'NOVEMBRO'
            WHEN DATEPART(M, N.Emissao) = 12 THEN 'DEZEMBRO'

      END,
      SUM(ni.TOTAL) AS Venda,
      
      (
		  	Select COUNT(*) 
		from nf 
			
		where NF.FILIAL=@filial 
				AND  (DATEPART(m, N.Emissao)= DATEPART(m, NF.Emissao))
					 AND   NF.TIPO_NF = 1 
					 AND ISNULL(NF.nf_Canc,0)=0	
				
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
      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM nf WHERE DATEPART(m, N.Emissao)= DATEPART(m, NF.Emissao)) > 0 THEN

                  (
                  Select isnull(SUM(nii.total),0) 
					from nf 
					inner join nf_item as nii on NF.codigo=nii.codigo and NF.Filial=nii.Filial and NF.Tipo_NF = nii.Tipo_NF
					inner join mercadoria as mi on mi.PLU = nii.PLU
					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  

					where NF.FILIAL=@filial 
							 AND (DATEPART(m, N.Emissao)= DATEPART(m, NF.Emissao) )
							 AND NF.TIPO_NF = 1
							 AND (LEN(@PLU)=0 OR nii.PLU = @plu)
							 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
							 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
							 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
							) 
							 /   
					(	
					Select COUNT(*) 
					from nf 
					
					where NF.FILIAL=@filial 
								AND  (DATEPART(m, N.Emissao)= DATEPART(m, NF.Emissao))
								AND   NF.TIPO_NF = 1	
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
		WHERE  N.FILIAL=@filial 
		AND  (N.Emissao BETWEEN  @DataDe AND   @DataAte)
			 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
			 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
			 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
			 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
			 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
		GROUP BY N.Emissao

END



Select 
	Mes,
	Nome,
	Venda,
	Clientes,
	[Venda_MD],
	NFP,
	VLR_NFP,
	PERC_NFP
 from (
		Select  ORDEM ='0000',
				MES,
				NOME,
				Sum(Venda) as Venda,
				--Sum(Vlr_Cancel) as Vlr_Cancel ,
				'|-NI-|' +CONVERT(VARCHAR,SUM(Clientes)) as Clientes ,
				SUM(venda)/SUM(Clientes) as [Venda_MD],
				SUM(nfp) as NFP,
				SUM(VLR_NFP) as VLR_NFP,
				SUM(PERC_NFP) AS PERC_NFP
		from #tmpVendas
		GROUP BY MES,NOME
		union all 
		Select     ORDEM ='ZZZZ',
					'|-SUB-|TOTAL',
					'',
					Sum(Venda) as Venda,
					--Sum(Vlr_Cancel) as Vlr_Cancel ,
					'|-NI-|' +CONVERT(VARCHAR,SUM(Clientes)) as Clientes ,
					SUM(venda)/SUM(Clientes) as [Venda_MD],
					SUM(nfp) as NFP,
					SUM(VLR_NFP) as VLR_NFP,
					SUM(PERC_NFP) AS PERC_NFP
			from #tmpVendas

	
) as a
ORDER BY  ORDEM, MES







GO




--PROCEDURES =======================================================================================
ALTER    Procedure [dbo].[sp_Rel_Resumo_Vendas]
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






Select 
			Data,
			Dia_Semana,
			Venda,
			 Clientes ,
			 [Venda_MD],
			 NFP,
			 VLR_NFP,
			 PERC_NFP
from (
 Select 
			ORDEM='0000',
			 Data,
			Dia_Semana,
			Sum(Venda) as Venda,
			'|-NI-|' +CONVERT(VARCHAR,SUM(Clientes)) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
	from 
	#tmpVendas
GROUP BY Data,Dia_Semana
UNION ALL
 Select 
			ORDEM='ZZZZ',
			'|-SUB-|TOTAL',
			'',
			Sum(Venda) as Venda,
			'|-NI-|' +CONVERT(VARCHAR,SUM(Clientes)) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
	from 
	#tmpVendas

) as a 

ORDER BY ordem ,Data ;

go 



/****** Object:  StoredProcedure [dbo].[SP_REL_VENDAS_POR_GRUPO]    Script Date: 13/09/2018 13:20:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--PROCEDURES =======================================================================================
ALTER PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO] 
	@FILIAL VARCHAR(20),
	@datade varchar(10), 
	@dataate varchar(10),
	@grupo varchar(max),
	@subgrupo varchar(max),
	@relatorio varchar(40)
as
begin 
	-- exec SP_REL_VENDAS_POR_GRUPO 'MATRIZ','20180801','20180913','|MERCEARIA SECA|','','TODOS'
	-- select top 10 * from saida_estoque
	-- @relatorio = TODOS ,CUPONS , NOTA SAIDA 	
declare @sql nvarchar(max);



 if(len(@subgrupo)>0)
	begin

		set @subgrupo = REPLACE(@subgrupo,'|',CHAR(39));
		set @subgrupo = REPLACE(@subgrupo,CHAR(39)+char(39),char(39)+','+char(39));
		
		set @sql ='declare @total decimal(12,2);
				  declare @totalnf decimal(12,2); '
		IF(@relatorio='CUPONS' OR @relatorio = 'TODOS')
		BEGIN	
		set @sql =@SQL+' 
						select @total = isnull(((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))),0) 
							from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
												 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
							where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and (a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') and c.Descricao_subgrupo in ('+@subgrupo+') and	a.data_cancelamento is null;	'		
		END 
		ELSE
		BEGIN
			set @sql =@SQL+' SET @TOTAL =0;'
		
		END
		IF(@relatorio='NOTA SAIDA' OR @relatorio='TODOS')
		BEGIN 
		set @sql =@SQL+' 
						select @totalnf =  isnull((SUM(a.Total)),0)
						 from NF_Item a inner join mercadoria b on a.PLU = b.plu 
											 inner join nf on nf.Filial = a.Filial and nf.Codigo = a.codigo and nf.Tipo_NF = a.Tipo_NF
											 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
											 INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
						where a.tipo_nf=1 AND a.Codigo_operacao <>'+ CHAR(39) +'5929'+ CHAR(39) +'
								and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
								and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and (nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') and c.Descricao_subgrupo in ('+@subgrupo+')	and nf.nf_Canc <>1;'			
		END
		ELSE
		BEGIN
				set @sql =@SQL+' SET @totalnf =0;'
		END
		
	 set @sql =@SQL+'	Select COD,DESCRICAO , Venda = sum(Venda),[%]=CONVERT(DECIMAL(12,2),((sum(venda)/(@total+@totalnf))*100)) 
		from 
		('
		IF(@relatorio='TODOS' OR @relatorio='CUPOM')
		BEGIN
			SET @sql =@SQL+'	select	COD=c.codigo_departamento,
					DESCRICAO= c.descricao_departamento , 
					Venda = (SUM(ISNULL(a.vlr,0))-SUM(isnull(a.desconto,0)))+SUM(isnull(a.acrescimo,0))
				from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
			where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and(a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +' )and c.Descricao_subgrupo in ('+@subgrupo+')  and a.data_cancelamento is null				
			group by c.codigo_departamento,c.descricao_departamento '
		END
		IF(@relatorio='TODOS')
		BEGIN
		  SET @sql =@SQL+' union all '
		END
		IF(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
		BEGIN
		
		SET @sql =@SQL+'	select COD=c.codigo_departamento,DESCRICAO= c.descricao_departamento , Venda = sum(isnull(a.Total,0))
				from NF_Item a inner join mercadoria b on a.PLU = b.plu 
							   inner join nf on nf.Filial =a.Filial and nf.Codigo = a.codigo and a.Tipo_NF = nf.Tipo_NF
							   inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
							   INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
			where a.tipo_nf=1 AND a.Codigo_operacao <>'+ CHAR(39) +'5929'+ CHAR(39) +'
							  and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							  and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and(nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +' )and c.Descricao_subgrupo in ('+@subgrupo+')  and nf.nf_Canc <>1				
			group by c.codigo_departamento,c.descricao_departamento '
		END
		SET @sql =@SQL+')as a
		group by COD,DESCRICAO
		'
	end
else
begin
	if len(@grupo)>0 
	begin 
		
		set @grupo = REPLACE(@grupo,'|',CHAR(39));
		set @grupo = REPLACE(@grupo,CHAR(39)+char(39),char(39)+','+char(39));
		
		set @sql ='declare @total decimal(12,2);
				  declare @totalnf decimal(12,2); '
		IF(@relatorio='CUPONS' OR @relatorio = 'TODOS')
		BEGIN	
		set @sql =@SQL+' 
						select @total = isnull(((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))),0) 
							from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
												 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
							where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and (a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') and c.Descricao_grupo in ('+@grupo+') and	a.data_cancelamento is null;	'		
		END 
		ELSE
		BEGIN
			set @sql =@SQL+' SET @TOTAL =0;'
		
		END
		IF(@relatorio='NOTA SAIDA' OR @relatorio='TODOS')
		BEGIN 
		set @sql =@SQL+' 
						select @totalnf =  isnull((SUM(a.Total)),0)
						 from NF_Item a inner join mercadoria b on a.PLU = b.plu 
											 inner join nf on nf.Filial = a.Filial and nf.Codigo = a.codigo and nf.Tipo_NF = a.Tipo_NF
											 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
											 INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
						where a.tipo_nf=1 AND a.Codigo_operacao <>'+ CHAR(39) +'5929'+ CHAR(39) +'
								and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
								and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and (nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') and c.Descricao_grupo in ('+@grupo+')	and nf.nf_Canc <>1;'			
		END
		ELSE
		BEGIN
				set @sql =@SQL+' SET @totalnf =0;'
		END
		
	 set @sql =@SQL+'	Select COD,DESCRICAO , Venda = sum(Venda),[%]=CONVERT(DECIMAL(12,2),((sum(venda)/(@total+@totalnf))*100)) 
		from 
		('
		IF(@relatorio='TODOS' OR @relatorio='CUPOM')
		BEGIN
			SET @sql =@SQL+'	select	COD=c.codigo_subgrupo,
					DESCRICAO= c.descricao_subGrupo , 
					Venda = (SUM(ISNULL(a.vlr,0))-SUM(isnull(a.desconto,0)))+SUM(isnull(a.acrescimo,0))
				from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
			where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and(a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +' )and c.Descricao_grupo in ('+@grupo+')  and a.data_cancelamento is null				
			group by c.codigo_subgrupo,c.descricao_subgrupo '
		END
		IF(@relatorio='TODOS')
		BEGIN
		  SET @sql =@SQL+' union all '
		END
		IF(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
		BEGIN
		
		SET @sql =@SQL+'	select COD=c.codigo_subgrupo,DESCRICAO= c.descricao_subgrupo , Venda = sum(isnull(a.Total,0))
				from NF_Item a inner join mercadoria b on a.PLU = b.plu 
							   inner join nf on nf.Filial =a.Filial and nf.Codigo = a.codigo and a.Tipo_NF = nf.Tipo_NF
							   inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
							   INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
			where a.tipo_nf=1 AND a.Codigo_operacao <>'+ CHAR(39) +'5929'+ CHAR(39) +'
							  and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							  and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and(nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +' )and c.Descricao_grupo in ('+@grupo+')  and nf.nf_Canc <>1				
			group by c.codigo_subgrupo,c.descricao_subgrupo '
		END
		SET @sql =@SQL+')as a
		group by COD,DESCRICAO'

	end
	else
	begin
		set @sql ='
		declare @total decimal(12,2);
		declare @totalnf decimal(12,2);	'
		IF(@relatorio='CUPOM' OR @relatorio = 'TODOS')
		BEGIN	
			set @sql =@SQL+'
						select @total = isnull(((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))),0) 
						from Saida_estoque 
						where Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and data_cancelamento is null  and Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +';'
		END
		ELSE
		BEGIN
			SET	@sql =@SQL+' SET @TOTAL=0; '
		END
		IF(@relatorio='NOTA SAIDA' OR @relatorio='TODOS')
		BEGIN 
		set @sql =@SQL+' 
							select @totalnf =  isnull((SUM(a.Total)) ,0)
							from NF_Item a inner join nf on nf.Filial = a.Filial and nf.Codigo = a.codigo and nf.Tipo_NF = a.Tipo_NF
								INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
							where a.tipo_nf=1 AND a.Codigo_operacao <>'+ CHAR(39) +'5929'+ CHAR(39) +'
							    and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							    and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' 
								and (nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +') 
								and nf.nf_Canc <>1
								
								;			
							'
		END
		ELSE
		BEGIN
			SET	@sql =@SQL+' SET @totalnf=0; '
		END
		
		SET	@sql =@SQL+' Select COD,DESCRICAO,VENDA =SUM(VENDA),[%]=CONVERT(DECIMAL(12,2),(SUM(VENDA)/(@total+@totalnf))*100)  
		from ('
		IF(@relatorio='CUPOM' OR @relatorio = 'TODOS')
		BEGIN
		set @sql =@SQL+'	
			select COD=c.codigo_grupo,DESCRICAO= c.descricao_grupo , Venda = ((SUM(ISNULL(vlr,0))-SUM(isnull(desconto,0)))+SUM(isnull(acrescimo,0))) 
			from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
			WHERE a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +'	and a.data_cancelamento is null				
			group by c.codigo_grupo,c.Descricao_grupo '
		END
		IF(@relatorio='TODOS')
		BEGIN
		  SET @sql=@sql+'	UNION ALL '
		END 
		IF(@relatorio='NOTA SAIDA' OR @relatorio='TODOS')
		BEGIN
			SET	@sql =@sql+' select COD=c.codigo_grupo,DESCRICAO= c.descricao_grupo , Venda = (SUM(A.Total)) 
			from NF_Item a inner join mercadoria b on a.PLU = b.plu 
						   INNER JOIN NF ON NF.Filial= A.Filial AND NF.Codigo=A.CODIGO AND NF.Tipo_NF=A.Tipo_NF	
						   inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
						   INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
			where a.tipo_nf=1 AND a.Codigo_operacao <>'+ CHAR(39) +'5929'+ CHAR(39) +'
							    and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							    and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' 
				and (NF.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +'	)
				and NF.nf_Canc<>1		
			group by c.codigo_grupo,c.Descricao_grupo'
		END
			
		SET @sql=@sql+') as a
		GROUP BY COD,DESCRICAO'
	end
end
 print @sql;
 exec(@sql);
end









insert into Versoes_Atualizadas select 'Versão:1.211.699', getdate();
GO


/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas]    Script Date: 03/09/2017 15:13:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Resumo_Vendas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Resumo_Vendas]
GO



/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas]    Script Date: 03/09/2017 15:13:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--PROCEDURES =======================================================================================
CREATE    Procedure [dbo].[sp_Rel_Resumo_Vendas]

            @Filial           As Varchar(20),

            @DataDe           As Varchar(8),

            @DataAte    As Varchar(8),
            @plu as Varchar (20),
            @descricao as varchar(50),
            @grupo as Varchar(50),
            @subGrupo as varchar(50),
            @departamento as varchar(50)

 

AS

 SELECT
            saida_estoque.Filial,
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
            saida_estoque.Filial = @Filial
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
           Saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            CPF_CNPJ;

   Select  Data,
			Dia_Semana,
			Sum(Venda) as Venda,
			--Sum(Vlr_Cancel) as Vlr_Cancel ,
			SUM(Clientes) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
	from (

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
      SUM(ni.TOTAL) AS Venda,
      
      (
		  	Select COUNT(*) 
		from nf 
			
		where NF.FILIAL=@filial 
				AND  (NF.Emissao= n.Emissao )
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
      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM nf WHERE nf.Emissao = n.Emissao) > 0 THEN

                  (
                  Select isnull(SUM(nii.total),0) 
					from nf 
					inner join nf_item as nii on NF.codigo=nii.codigo and NF.Filial=nii.Filial and NF.Tipo_NF = nii.Tipo_NF
					inner join mercadoria as mi on mi.PLU = nii.PLU
					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  

					where NF.FILIAL=@filial 
							 AND (NF.Emissao= n.Emissao )
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
								AND  (NF.Emissao= n.Emissao )
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
AND  (N.Emissao BETWEEN @DataDe AND @DataAte)
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
	 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY N.Emissao 

) as a
GROUP BY Data,Dia_Semana
ORDER BY Data







GO




/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas]    Script Date: 03/09/2017 15:13:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Resumo_Vendas_Mes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Resumo_Vendas_Mes]
GO



/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas]    Script Date: 03/09/2017 15:13:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--PROCEDURES =======================================================================================
CREATE     Procedure [dbo].[sp_Rel_Resumo_Vendas_Mes]

            @Filial           As Varchar(20),

            @DataDe           As Varchar(8),

            @DataAte    As Varchar(8),
            @plu as Varchar (20),
            @descricao as varchar(50),
            @grupo as Varchar(50),
            @subGrupo as varchar(50),
            @departamento as varchar(50)

 

AS

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



    Select  MES,
			NOME,
			Sum(Venda) as Venda,
			--Sum(Vlr_Cancel) as Vlr_Cancel ,
			SUM(Clientes) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
    from(
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
            
     union all


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

) as a
GROUP BY MES,NOME
ORDER BY MES








go 


/****** Object:  StoredProcedure [dbo].[sp_Prc_ins_Conta_a_Receber]    Script Date: 03/09/2017 16:55:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER	PROCEDURE [dbo].[sp_Prc_ins_Conta_a_Receber]
	--@Documento 		AS varchar(20), --**
	@Codigo_cliente 	AS varchar(20),
	@filial 		AS varchar(20), --**
	@Valor 			AS Float,
	@Desconto		As Float,
	@Emissao 		AS DateTime,
	@cheque 		AS varchar(10),
	@agencia 		AS varchar(6),
	@banco 			AS varchar(3),
	@conta 			AS varchar(10),
	@operador 		AS varchar(9),
	@pdv 			AS Int,
	@finalizadora 		AS Int,	--**
	@rede_cartao		As varchar(3) = '0' ,
	@id_Bandeira		As varchar(3) = '0' 
AS

Declare
	@Documento		As Varchar(20),
	@CCC			As Varchar(10),
	@ID_CC	 		As Varchar(20),
	@id_finalizadora	As Varchar(20),
	@taxa			As Numeric(18,2),
	@ValorTaxa		As Float,
	@ValorPago		As Float,	
	@status			As Int,
	@dias			As Int,
	@vencimento		As datetime,
	@Baixa_Automatica AS Int
----------------------------------Preenchendo VÃ¡riaveis--------------------------

Set @Codigo_Cliente = Isnull((Select Distinct Isnull(Codigo_Cliente,0) From Cliente c Where Ltrim(Rtrim(Replace(Replace(Replace(c.CNPJ,'/',''),'.',''),'-',''))) = @Codigo_cliente),0) 

Select 	
	@id_Finalizadora = Finalizadora, 
	@Status = 2, --Case When Finalizadora = 'DINHEIRO' Then 2 else 1 end,
	@CCC = codigo_centro_custo,
	@Taxa = Isnull((Select ISNULL(Taxa,0) From Cartao c Where c.Nro_Finalizadora = @Finalizadora And convert(int,c.id_rede) = convert(int,@rede_cartao) and convert(int,c.id_bandeira) = convert(int,@id_Bandeira)),0),
	@Dias = isnull(dias, 0)
From 
	finalizadora Left Outer Join Cartao on finalizadora.Nro_Finalizadora = cartao.Nro_Finalizadora
where 
	finalizadora.Nro_Finalizadora = @Finalizadora

Select @ID_CC = id_cc from centro_custo where codigo_centro_custo = @CCC

Set @Vencimento = master.dbo.F_BR_PROX_DIA_UTIL(dateadd(day,@dias,@emissao))

Set @Documento = replace(convert(varchar, @emissao, 3),'/','') + replicate('0',2-(len(@rede_cartao))) + Convert(Varchar, @rede_cartao) +
			replicate('0',2-(len(@id_Bandeira))) + Convert(Varchar, @id_Bandeira) + 
			replicate('0',2-(len(@PDV))) + Convert(Varchar, @PDV) + '-' + 
			replicate('0',2-(len(@Finalizadora))) + Convert(Varchar, @Finalizadora)

----------------------------------Fim De Preenchimento------------------------------

Set @ValorTaxa = 0
Set @ValorPago = 0
Set @Baixa_Automatica = 0

IF @taxa > 0
	Set @ValorTaxa = @valor - isnull(@desconto, 0)
	Set @ValorTaxa = (@taxa/100) * @ValorTaxa

Begin

IF @finalizadora = 1 
Begin
	Set @ValorPago = @valor - isnull(@desconto, 0)
	Set @Baixa_Automatica = 1
End

If Exists(select * from conta_a_receber where
		Emissao = @Emissao And Filial = @Filial And Finalizadora = @Finalizadora 
		and pdv = @pdv and isnull(rede_cartao,'0') = isnull(@rede_cartao,'0') and isnull(id_Bandeira,'0') = isnull(@id_Bandeira,'0') 
		--and operador = @operador 
		And cheque = @cheque) 
	--And cheque = @cheque --is null --Com EstÃ¡ Linha NÃ£o IrÃ¡ Agrupar Cheques
	Begin
		--IF @Finalizadora = 1
			Update conta_a_receber set 
				Valor = valor + @Valor, desconto = isnull(Desconto, 0) + isnull(@desconto, 0),
				Valor_Pago = valor_pago + @ValorPago
			Where 
				Emissao = @Emissao And Filial = @Filial And Finalizadora = @Finalizadora 
				and pdv = @pdv and isnull(rede_cartao,0) = isnull(@rede_cartao,0) 
				and isnull(id_Bandeira,'0') = isnull(@id_Bandeira,'0') 
				--and operador = @operador 
				And cheque = @cheque
		--ELSE
		--	Update conta_a_receber set 
		--		Valor = valor + @Valor, desconto = isnull(Desconto, 0) + isnull(@desconto, 0),
		--		Taxa = Taxa + @ValorTaxa
		--	Where 
		--		Emissao = @Emissao And Filial = @Filial And Finalizadora = @Finalizadora 
		--		and pdv = @pdv and isnull(rede_cartao,0) = isnull(@rede_cartao,0) 
		--		and isnull(id_Bandeira,'0') = isnull(@id_Bandeira,'0') 
				--and operador = @operador 
		--		And cheque = @cheque	
	End		
Else
	INSERT INTO
		Conta_a_Receber
		(
			Documento, Codigo_cliente, filial, Codigo_Centro_Custo, Valor, Desconto, Emissao, Vencimento,
			Entrada, Pagamento, Valor_Pago,	ID_CC, cheque, agencia, banco, conta, Baixa_Automatica,	usuario,
			status, operador, pdv, finalizadora, id_finalizadora, taxa, rede_cartao, obs,Documento_emitido,id_Bandeira)
	VALUES
		(
			@Documento, @Codigo_cliente, @filial, @CCC, @Valor, @Desconto, @Emissao, @Vencimento,
			@Emissao,--@Entrada,
			case when @status = 2 then @emissao else null end, --@Pagamento,
			case when @status = 2 then @ValorPago else @ValorPago end, --@Valor_Pago,
			@ID_CC,	@cheque, @agencia, @banco, @conta, 
			@Baixa_Automatica,		--@Baixa_Automatica,
			'CAIXA',	--@usuario,
			@status, @operador, @pdv, @finalizadora, @id_finalizadora, @ValorTaxa, @rede_cartao,
			'Lancamento Automatico PDV',--Obs
			'999999999',   ---Documento emitido
			@id_bandeira
		)
End
----------------------------------Atualizando Saldo Da Conta Corrente-----------------
IF @status = 2 
Begin
	Update Conta_Corrente set saldo = Saldo + @Valor - isnull(@desconto, 0)
		where id_cc = @id_cc
End

go 

update conta_a_receber set pagamento = emissao where pagamento is null and status=2


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.156.591', getdate();
GO
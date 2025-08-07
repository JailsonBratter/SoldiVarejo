/****** Object:  StoredProcedure [dbo].[SP_REL_VENDAS_POR_GRUPO]    Script Date: 03/10/2017 09:14:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_VENDAS_POR_GRUPO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO]
GO


/****** Object:  StoredProcedure [dbo].[SP_REL_VENDAS_POR_GRUPO]    Script Date: 03/10/2017 09:14:36 ******/
SET QUOTED_IDENTIFIER ON
GO


--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO] 
	@FILIAL VARCHAR(20),
	@datade varchar(10), 
	@dataate varchar(10),
	@grupo varchar(max),
	@relatorio varchar(40)
as
begin 
	-- exec SP_REL_VENDAS_POR_GRUPO 'MATRIZ','20160601','20170308','','CUPOM'
	-- select top 10 * from saida_estoque
	-- @relatorio = TODOS ,CUPONS , NOTA SAIDA 	
declare @sql nvarchar(max);



if len(@grupo)>0 
	begin
		
		
		set @grupo = REPLACE(@grupo,'|',CHAR(39));
		
		
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
			SET @sql =@SQL+'	select	COD=c.codigo_departamento,
					DESCRICAO= c.descricao_departamento , 
					Venda = (SUM(ISNULL(a.vlr,0))-SUM(isnull(a.desconto,0)))+SUM(isnull(a.acrescimo,0))
				from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
			where a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and(a.Data_movimento between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +' )and c.Descricao_grupo in ('+@grupo+')  and a.data_cancelamento is null				
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
							  and a.Filial='+ CHAR(39) +@FILIAL+ CHAR(39) +' and(nf.Emissao between '+ CHAR(39) +@datade+ CHAR(39) +' and '+ CHAR(39) +@dataate+ CHAR(39) +' )and c.Descricao_grupo in ('+@grupo+')  and nf.nf_Canc <>1				
			group by c.codigo_departamento,c.descricao_departamento '
		END
		SET @sql =@SQL+')as a
		group by COD,DESCRICAO
		'
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
 --print @sql;
 exec(@sql);
end




GO




/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas]    Script Date: 03/10/2017 09:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Resumo_Vendas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Resumo_Vendas]
GO



/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas]    Script Date: 03/10/2017 09:09:35 ******/
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
            @departamento as varchar(50),
            @relatorio as varchar(40)

 

AS
-- EXEC sp_Rel_Resumo_Vendas 'MATRIZ','20160601','20170309','','','','','','TODOS'
-- @relatorio = TODOS ,CUPOM , NOTA SAIDA 	
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

if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN
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
      AND   Data_Movimento BETWEEN @DataDe AND @DataAte
      AND   Data_Cancelamento IS NULL
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

      Venda =       SUM(CASE WHEN Data_Cancelamento IS Null THEN Convert(Decimal(18,2),VLR-desconto) ELSE 0 END),
      Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento),
      Venda_MD =    CONVERT(DECIMAL(12,2), 
						CASE WHEN (SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #LIXO WHERE #LIXO.Data_Movimento = Saida_Estoque.Data_Movimento) > 0 THEN

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
			inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 

		where NF.FILIAL=@filial 
				and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
				AND NF.Codigo_operacao <>'5929'
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
					inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 
					where NF.FILIAL=@filial 
							and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							AND NF.Codigo_operacao <>'5929'
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
					inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 
					where NF.FILIAL=@filial 
								AND  (NF.Emissao= n.Emissao )
								AND   NF.TIPO_NF = 1	
								and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
								AND NF.Codigo_operacao <>'5929'
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
	 AND N.Codigo_operacao <>'5929'
								
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
	 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY N.Emissao 
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



GO





/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas_Mes]    Script Date: 03/10/2017 11:46:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Resumo_Vendas_Mes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Resumo_Vendas_Mes]
GO



/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas_Mes]    Script Date: 03/10/2017 11:46:10 ******/
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
            @departamento as varchar(50),
            @relatorio varchar(40)

 

AS
-- EXEC  sp_Rel_Resumo_Vendas_Mes 'MATRIZ','20160101','20160401','','','','','','TODOS'
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

  Select  MES,
			NOME,
			Sum(Venda) as Venda,
			--Sum(Vlr_Cancel) as Vlr_Cancel ,
			SUM(Clientes) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
    from #tmpVendas
GROUP BY MES,NOME
ORDER BY MES









GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.157.592', getdate();
GO
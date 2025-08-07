


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_VENDAS_POR_GRUPO]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO]
GO
--PROCEDURES =======================================================================================
create PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO] 
	@FILIAL VARCHAR(20),
	@datade varchar(10), 
	@dataate varchar(10),
	@grupo varchar(max),
	@subgrupo varchar(max),
	@relatorio varchar(40)
as
begin 
	-- exec SP_REL_VENDAS_POR_GRUPO 'MATRIZ','20180701','20181003','|MERCEARIA SECA|','|MATINAIS|','TODOS'
	


Create table #VendasGrupo
(
 PLU varchar(40),
 DESCRICAO VARCHAR(100),
 QTDE NUMERIC(12,2),
 VALOR NUMERIC(12,2),
 COD_GRUPO VARCHAR(3),
 GRUPO VARCHAR(100),
 COD_SUBGRUPO VARCHAR(6),
 SUBGRUPO VARCHAR(100),
 COD_DEPARTAMENTO VARCHAR(9),
 DEPARTAMENTO VARCHAR(100),

)

		IF(@relatorio='NOTA SAIDA' OR @relatorio='TODOS')
		BEGIN 
			INSERT INTO #VendasGrupo
			SELECT NFi.PLU
				 ,M.Descricao
				 ,(NFi.Qtde * NFi.Embalagem)
				 ,NFi.Total
				 ,CONVERT(VARCHAR(3),dep.codigo_grupo)
				 ,DEP.Descricao_grupo
				 ,dep.codigo_subgrupo
				 ,DEP.descricao_subgrupo
				 ,dep.codigo_departamento
				 ,DEP.descricao_departamento
			from NF_Item NFi inner join mercadoria M on NFi.PLU = M.plu 
											 inner join nf on nf.Filial = NFi.Filial and nf.Codigo = NFi.codigo and nf.Tipo_NF = NFi.Tipo_NF
											 inner join W_BR_CADASTRO_DEPARTAMENTO DEP on m.Codigo_departamento = DEP.codigo_departamento
											 INNER JOIN natureza_operacao AS np on nf.codigo_operacao=np.codigo_operacao
						where NFi.tipo_nf=1 AND NFi.Codigo_operacao <>5929
								and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
								and NFi.Filial=@FILIAL 
								and (nf.Emissao between @datade and @dataate) 
								and (LEN(@grupo)=0 OR  @grupo LIKE '%|'+DEP.Descricao_grupo+'|%')
								and (LEN(@subgrupo)=0 OR  @subgrupo LIKE '%|'+DEP.descricao_subgrupo+'|%')	
								and nf.nf_Canc <>1;			
								
		END 


		IF(@relatorio='TODOS' OR @relatorio='CUPOM')
		BEGIN

			INSERT INTO #VendasGrupo
			select	s.plu
				   ,m.Descricao
				   ,isnull(s.Qtde,0)
				   ,(isnull(s.vlr,0) - isnull(s.Desconto,0))+isnull(s.acrescimo,0)
				   ,CONVERT(VARCHAR(3),DEP.codigo_grupo)
				   ,DEP.Descricao_grupo
				   ,DEP.codigo_subgrupo
				   ,DEP.descricao_subgrupo
				   ,DEP.codigo_departamento
				   ,DEP.descricao_departamento
				from Saida_estoque s inner join mercadoria m on s.PLU = m.plu 
								 inner join W_BR_CADASTRO_DEPARTAMENTO dep on m.Codigo_departamento = dep.codigo_departamento
			where s.Filial=@FILIAL 
				and(s.Data_movimento between @datade and +@dataate )
				and (LEN(@grupo)=0 OR @GRUPO='TODOS' OR  @grupo LIKE '%|'+DEP.Descricao_grupo+'|%')
				and (LEN(@subgrupo)=0 OR @subgrupo='TODOS' OR   @subgrupo LIKE '%|'+DEP.descricao_subgrupo+'|%')	
				and s.data_cancelamento is null				
			

		END 

DECLARE @TotalVenda numeric(12,2)
Select @TotalVenda = sum(valor) from #VendasGrupo

IF(LEN(@subgrupo)>0) 
BEGIN 
		SELECT PLU,DESCRICAO,QTDE,VENDA,[%]  FROM (
			SELECT ORDEM = convert(varchar(100),departamento) +'000'
				,PLU ='|-TITULO-||CONCAT|'+COD_DEPARTAMENTO +'-'+ DEPARTAMENTO
				,DESCRICAO=''
				,QTDE = 0
				,VENDA  = 0
				,[%]=0
			 FROM #VendasGrupo
			 group by COD_DEPARTAMENTO,departamento	
			 UNION ALL 
			 SELECT ORDEM = convert(varchar(100),departamento)  +'111'
					,'PLU'+PLU
					,DESCRICAO
					,QTDE = SUM(QTDE)
					,TOTAL = SUM(VALOR)
					,[%]=CONVERT(DECIMAL(12,2),((sum(VALOR)/(@TotalVenda))*100))
			FROM #VendasGrupo
			GROUP BY PLU,DESCRICAO,departamento
			UNION ALL
			SELECT ORDEM = convert(varchar(100),departamento)  +'999'
				,PLU ='|-SUB-|SUB-TOTAL'
				,DESCRICAO=''
				,QTDE = SUM(QTDE)
				,TOTAL  = SUM(VALOR)
				,[%]=CONVERT(DECIMAL(12,2),((sum(VALOR)/(@TotalVenda))*100))
			 FROM #VendasGrupo
			 group by departamento	
			UNION ALL 
			SELECT ORDEM ='ZZZZ9999'
			  , PLU='|-SUB-|TOTAL'
			  ,DESCRICAO= ''
			  ,QTDE = SUM(QTDE)
			  ,VENDA = SUM(VALOR)
			  ,[%]=100
		FROM #VendasGrupo
		) AS A
		ORDER BY ORDEM,DESCRICAO
END  
ELSE 
BEGIN
	IF(LEN(@GRUPO)>0)
	BEGIN 
		SELECT COD, DESCRICAO, VENDA,[%] FROM (
		SELECT ORDEM = SubGrupo
				,COD='SFD_'+COD_SUBGRUPO
			  ,DESCRICAO= SUBGRUPO
			  ,VENDA = SUM(VALOR)
			  ,[%]=CONVERT(DECIMAL(12,2),((sum(VALOR)/(@TotalVenda))*100))
		FROM #VendasGrupo
		GROUP BY COD_SUBGRUPO,SUBGRUPO
		UNION ALL 
		SELECT ORDEM ='ZZZZ9999'
			  , COD='|-SUB-|TOTAL'
			  ,DESCRICAO= ''
			  ,VENDA = SUM(VALOR)
			  ,[%]=100
		FROM #VendasGrupo
		)AS A
		ORDER BY ORDEM
	END 
	ELSE
	BEGIN 
		SELECT COD, DESCRICAO, VENDA,[%] FROM (
	    SELECT ORDEM = GRUPO,
			   COD='SFD_'+COD_GRUPO
			  ,DESCRICAO= GRUPO
			  ,VENDA = SUM(VALOR)
			  ,[%]=CONVERT(DECIMAL(12,2),((sum(VALOR)/(@TotalVenda))*100))
		FROM #VendasGrupo
		GROUP BY COD_GRUPO,GRUPO
		UNION ALL 
		SELECT ORDEM ='ZZZZ9999'
			  ,COD='|-SUB-|TOTAL'
			  ,DESCRICAO= ''
			  ,VENDA = SUM(VALOR)
			  ,[%]=100
		FROM #VendasGrupo
		)AS A
		ORDER BY ORDEM
	END 
END 


end





insert into Versoes_Atualizadas select 'Versão:1.216.708', getdate();
GO




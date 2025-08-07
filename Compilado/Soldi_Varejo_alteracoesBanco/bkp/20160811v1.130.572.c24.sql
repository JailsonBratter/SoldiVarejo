IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Vendedor]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[sp_Rel_Vendedor]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Rel_Vendedor](
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Vendedor	  as varchar(40),
	@tipo		  as Varchar(20)
	)
as
begin 
-- exec sp_rel_Vendedor 'MATRIZ', '20160421', '20160421','VENDEDOR','ANALITICO'


IF(@Vendedor='TODOS')
BEGIN
	if(@tipo='SINTETICO')
	BEGIN
		SELECT  Funcionario.Codigo,
				Funcionario.Nome,
				Funcionario.Funcao,
				CONVERT(VARCHAR(10),isnull(Funcionario.Comissao,0))+'%' as [Comissao (%)],    
				sum(se.vlr - se.Desconto) as Total,
				(sum(se.vlr - se.Desconto)* isnull(funcionario.Comissao,0))/100 as [Vlr Comissao]
		FROM Saida_estoque se inner join Funcionario on LTRIM(RTRIM(se.vendedor)) = LTRIM(RTRIM(Funcionario.codigo))
		where se.Filial = @FILIAL and  se.Data_movimento between @Datade and @Dataate and data_cancelamento is null
			-- and (@Vendedor='TODOS' or Funcionario.Nome = @Vendedor)
		GROUP BY Funcionario.codigo, Funcionario.Nome,Funcionario.Funcao,Funcionario.Comissao
	END
	ELSE
	BEGIN
		SELECT  Funcionario.Codigo,
				Funcionario.Nome,
				Funcionario.Funcao,
				convert(varchar,se.Data_movimento,103) as Data,
				'SFT_'+se.Documento as Documento,
				'SFT_'+convert(varchar,se.Caixa_Saida) as Pdv,
				'PLU'+SE.PLU AS PLU,
				M.DESCRICAO,
				CONVERT(VARCHAR(10),isnull(Funcionario.Comissao,0))+'%' as [Comissao (%)],    
				(se.vlr - se.Desconto) as Total,
				((se.vlr - se.Desconto)* isnull(funcionario.Comissao,0))/100 as [Vlr Comissao]
		FROM Saida_estoque se inner join Funcionario on LTRIM(RTRIM(se.vendedor)) = LTRIM(RTRIM(Funcionario.codigo))
				INNER JOIN Mercadoria AS M ON M.PLU=SE.PLU
		where se.Filial = @FILIAL and  se.Data_movimento between @Datade and @Dataate and data_cancelamento is null
	END
END
ELSE
BEGIN
	if(@tipo='SINTETICO')
		BEGIN
			SELECT  
					CONVERT(VARCHAR,SE.Data_movimento,103)AS DATA,
					
					CONVERT(VARCHAR(10),isnull(Funcionario.Comissao,0))+'%' as [Comissao (%)],
					SUM(se.vlr - se.Desconto) as TOTAL,
					round(SUM(((se.vlr - se.Desconto)* isnull(funcionario.Comissao,0))/100),3) as [Vlr Comissao]
			FROM Saida_estoque se inner join Funcionario on LTRIM(RTRIM(se.vendedor)) = LTRIM(RTRIM(Funcionario.codigo))
			
			where se.Filial = @FILIAL and  se.Data_movimento between @Datade and @Dataate and data_cancelamento is null
				and ( Funcionario.Nome = @Vendedor)
			GROUP BY SE.Data_movimento,Funcionario.Comissao
		END
		ELSE
		BEGIN 
			SELECT  
				convert(varchar,se.Data_movimento,103) as Data,
				'SFT_'+se.Documento as Documento,
				'SFT_'+convert(varchar,se.Caixa_Saida) as Pdv,
				'PLU'+SE.PLU AS PLU,
				M.DESCRICAO,
				CONVERT(VARCHAR(10),isnull(Funcionario.Comissao,0))+'%' as [Comissao (%)],
				(se.vlr - se.Desconto) as TOTAL,
				round((((se.vlr - se.Desconto)* isnull(funcionario.Comissao,0))/100),3) as [Vlr Comissao]
			FROM Saida_estoque se inner join Funcionario on LTRIM(RTRIM(se.vendedor)) = LTRIM(RTRIM(Funcionario.codigo))
				INNER JOIN Mercadoria AS M ON M.PLU=SE.PLU
			where se.Filial = @FILIAL and  se.Data_movimento between @Datade and @Dataate and data_cancelamento is null
				and ( Funcionario.Nome = @Vendedor)
			
		END
END
end



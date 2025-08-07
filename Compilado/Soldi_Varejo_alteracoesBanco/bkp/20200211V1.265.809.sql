
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('und_compra'))
begin
	alter table mercadoria alter column und_compra varchar(2)
	
end
else
begin
	alter table mercadoria add  und_compra varchar(2)
end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('descricao_producao'))
begin
	alter table mercadoria alter column descricao_producao varchar(60)
	alter table mercadoria alter column embalagem_producao int
	alter table mercadoria alter column custo_producao numeric(18,2)
	
end
else
begin
	alter table mercadoria add  descricao_producao varchar(60);
	alter table mercadoria add  embalagem_producao int
	alter table mercadoria add  custo_producao numeric(18,2)
end 
go 



CREATE TABLE [dbo].[agrupamento_producao](
	[Agrupamento] [varchar](50) NULL
) ON [PRIMARY]
GO

insert into agrupamento_producao
values ('BOLO TIRA'),
	   ('BOLO PLACA'),
	   ('BOLO REDONDO'),
	   ('PROD.INDUSTRIAL'),
	   ('PROD.MANUAL');

GO 




 
 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_SmartPhone]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_Cons_SmartPhone]
END
GO
CREATE  PROCEDURE [dbo].[sp_Cons_SmartPhone]
	@filial    as varchar(20),
	@Tipo		as integer
AS
--	exec sp_Cons_SmartPhone 'MATRIZ',0
DECLARE @DATE AS DATETIME
SET @DATE = GETDATE()

IF @Tipo = 0
	BEGIN
		SELECT  
			G.Descricao_Grupo AS Coluna , Valor = SUM(NF_Item.Total) , VisualizaGrafico = 1
		 into #venda	FROM nf INNER JOIN NF_Item ON NF.Codigo = NF_Item.Codigo 
									  AND NF.Filial = NF_ITEM.Filial
									  AND NF.Tipo_NF = NF_Item.Tipo_NF
									  AND NF.Cliente_Fornecedor = NF_ITEM.Cliente_Fornecedor
			 inner join mercadoria as m on NF_Item.PLU = m.PLU
			 inner join Grupo as g on convert(int, substring(m.Codigo_departamento,1,3) )= g.Codigo_Grupo
			WHERE NF.FILIAL = @filial 
				AND nf.Tipo_NF = 1 
				AND YEAR(NF.Data) = YEAR(@DATE) 
				AND MONTH(nf.Data) = MONTH(@DATE) 
				AND ISNULL(NF.nf_Canc,0)=0
				AND NF.status ='AUTORIZADO'
			group by g.Descricao_Grupo 
			order by 2 desc ;
		
		Select * from #venda
		UNION ALL
		SELECT  
			'TOTAL' AS Coluna , Valor = SUM(NF_Item.Total) , VisualizaGrafico = 0 
			FROM nf INNER JOIN NF_Item ON NF.Codigo = NF_Item.Codigo 
									  AND NF.Filial = NF_ITEM.Filial
									  AND NF.Tipo_NF = NF_Item.Tipo_NF
									  AND NF.Cliente_Fornecedor = NF_ITEM.Cliente_Fornecedor
				WHERE NF.FILIAL = @filial 
				AND nf.Tipo_NF = 1 
				AND YEAR(NF.Data) = YEAR(@DATE) 
				AND MONTH(nf.Data) = MONTH(@DATE) 
				AND ISNULL(NF.nf_Canc,0)=0
				AND NF.status ='AUTORIZADO'

		
	END

IF @Tipo = 1
	BEGIN
		SELECT 'Total' AS Coluna, Valor = ISNULL(SUM(CASE WHEN Conta_a_pagar.Status NOT IN(3) THEN Conta_a_pagar.Valor ELSE 0 END), 0), VisualizaGrafico = 0  FROM Conta_a_pagar WHERE FILIAL = @filial AND YEAR(Conta_a_pagar.Vencimento) = YEAR(@DATE) AND MONTH(Conta_a_pagar.Vencimento) = MONTH(@DATE)
		UNION ALL
		SELECT 'Aberto' AS Coluna, Valor = ISNULL(SUM(CASE WHEN Conta_a_pagar.Status NOT IN(2, 3) THEN Conta_a_pagar.Valor ELSE 0 END), 0), VisualizaGrafico = 1  FROM Conta_a_pagar WHERE FILIAL = @filial AND YEAR(Conta_a_pagar.Vencimento) = YEAR(@DATE) AND MONTH(Conta_a_pagar.Vencimento) = MONTH(@DATE)
		UNION ALL
		SELECT 'Concluido' AS Coluna, Valor = ISNULL(SUM(CASE WHEN Conta_a_pagar.Status IN(2) THEN Conta_a_pagar.Valor ELSE 0 END), 0), VisualizaGrafico = 1  FROM Conta_a_pagar WHERE FILIAL = @filial AND YEAR(Conta_a_pagar.Vencimento) = YEAR(@DATE) AND MONTH(Conta_a_pagar.Vencimento) = MONTH(@DATE)
	END
IF @Tipo = 2
	BEGIN
		SELECT 'Total' as Coluna, Valor = ISNULL(SUM(
		CASE 
		   WHEN Conta_a_receber.Status NOT IN(3) THEN (Conta_a_receber.Valor - ISNULL(Desconto, 0) + ISNULL(Acrescimo, 0) )  ELSE 0 END), 0), 
		   VisualizaGrafico = 0  FROM Conta_a_receber 
		   WHERE FILIAL = @filial 
		   AND YEAR(Conta_a_receber.Vencimento) = YEAR(@DATE) 
		   AND MONTH(Conta_a_receber.Vencimento) = MONTH(@DATE) 
		   --AND status_cnab NOT LIKE '%TRANSF%DESCONTO%' 
		   --AND status_cnab NOT LIKE '%ERRO%' 
		   AND Banco NOT IN(SELECT Numero_Banco FROM BANCO WHERE Numero_Banco IN(888, 111, 101))
		union all
		select 'Aberto', ISNULL(SUM(
		CASE 
		   WHEN Conta_a_receber.Status NOT IN(2, 3) THEN (Conta_a_receber.Valor - ISNULL(Desconto, 0) + ISNULL(Acrescimo, 0) ) ELSE 0 END), 0), 
		   VisualizaGrafico = 1   FROM Conta_a_receber 
		   WHERE FILIAL = @filial 
		   AND YEAR(Conta_a_receber.Vencimento) = YEAR(@DATE) 
		   AND MONTH(Conta_a_receber.Vencimento) = MONTH(@DATE) 
		   --AND status_cnab NOT LIKE '%TRANSF%DESCONTO%'
		   --AND status_cnab NOT LIKE '%ERRO%' 
		union all
		select 'Concluido', ISNULL(SUM(
		CASE 
		   WHEN Conta_a_receber.Status = 2 THEN (Conta_a_receber.Valor - ISNULL(Desconto, 0) + ISNULL(Acrescimo, 0)  ) ELSE 0 END), 0), 
		   VisualizaGrafico = 1   FROM Conta_a_receber 
		   WHERE FILIAL = @filial 
		   AND YEAR(Conta_a_receber.Vencimento) = YEAR(@DATE) 
		   AND MONTH(Conta_a_receber.Vencimento) = MONTH(@DATE) 
		   --AND status_cnab NOT LIKE '%TRANSF%DESCONTO%'
		   --AND status_cnab NOT LIKE '%ERRO%' 
		   AND Banco NOT IN(SELECT Numero_Banco FROM BANCO WHERE Numero_Banco IN(888, 111, 101))
	END
	go 
	
	
	IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('Pontos_Fidelizacao'))
begin
	alter table mercadoria alter column Pontos_Fidelizacao integer
end
else
begin
	alter table mercadoria add Pontos_Fidelizacao integer
end 
go 
	
	
	
	

go
insert into Versoes_Atualizadas select 'Versão:1.265.809', getdate();


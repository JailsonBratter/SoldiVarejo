IF OBJECT_ID (N'cliente_fidelidade', N'U') IS NULL 
begin
create table Cliente_fidelidade(
	Codigo_cliente varchar(11)
	,Data_venda datetime
	,Caixa_saida int
	,Documento varchar(20)
	,PLU varchar(17)
	,Qtde_pontos numeric(18,3)
)

end
go 


 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_br_Mercadoria_Acum_Dia]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_br_Mercadoria_Acum_Dia]
END
GO
CREATE  PROCEDURE [dbo].[sp_br_Mercadoria_Acum_Dia] 
	@PLU Char(6), @Filial varchar(20) output
AS
	begin
	-- sp_br_mercadoria_acum_dia '26897', 'matriz'
--DECLARE @PLU Char(6), @Filial varchar(20) 

--set @PLU = '10057'
--set @Filial= 'MATRIZ'


IF object_id('tempdb..#temUlt30') IS NOT NULL 
BEGIN
	DROP TABLE #temUlt30
END

create table #temUlt30 
(
	plu varchar(6),
	dia datetime
)
insert into #temUlt30 
values  (@plu,dateadd(Day,-30,GetDate())),
		(@plu,dateadd(Day,-29,GetDate())),
		(@plu,dateadd(Day,-28,GetDate())),
		(@plu,dateadd(Day,-27,GetDate())),
		(@plu,dateadd(Day,-26,GetDate())),
		(@plu,dateadd(Day,-25,GetDate())),
		(@plu,dateadd(Day,-24,GetDate())),
		(@plu,dateadd(Day,-23,GetDate())),
		(@plu,dateadd(Day,-22,GetDate())),
		(@plu,dateadd(Day,-21,GetDate())),
		(@plu,dateadd(Day,-20,GetDate())),
		(@plu,dateadd(Day,-19,GetDate())),
		(@plu,dateadd(Day,-18,GetDate())),
		(@plu,dateadd(Day,-17,GetDate())),
		(@plu,dateadd(Day,-16,GetDate())),
		(@plu,dateadd(Day,-15,GetDate())),
		(@plu,dateadd(Day,-14,GetDate())),
		(@plu,dateadd(Day,-13,GetDate())),
		(@plu,dateadd(Day,-12,GetDate())),
		(@plu,dateadd(Day,-11,GetDate())),
		(@plu,dateadd(Day,-10,GetDate())),
		(@plu,dateadd(Day,-9,GetDate())),
		(@plu,dateadd(Day,-8,GetDate())),
		(@plu,dateadd(Day,-7,GetDate())),
		(@plu,dateadd(Day,-6,GetDate())),
		(@plu,dateadd(Day,-5,GetDate())),
		(@plu,dateadd(Day,-4,GetDate())),
		(@plu,dateadd(Day,-3,GetDate())),
		(@plu,dateadd(Day,-2,GetDate())),
		(@plu,dateadd(Day,-1,GetDate())),
		(@plu,GetDate())
		
-- select *	from #temUlt30 	;
		select 
			Dia = #temUlt30.dia
			,qtde =  isnull(sum(qtde),0),
			vlr = isnull(sum(vlr),0) 
			
	  into #venda from #temUlt30 
				left join saida_Estoque WITH (index=ix_saida_estoque_01) on 
					#temUlt30.plu collate database_default = saida_estoque.plu collate database_default 
				and convert(varchar,#temUlt30.dia, 101)=convert(varchar,Data_Movimento, 101) 
				and data_cancelamento is null 
				and saida_estoque.Filial collate database_default = @Filial collate database_default
		where 
		
			#temUlt30.plu collate database_default = @PLU collate database_default
			and #temUlt30.dia >= dateadd(Day,-30,GetDate())
		
		group by Saida_estoque.Data_movimento, #temUlt30.dia, #temUlt30.plu
		
		
		insert into #venda 
		select 
			#temUlt30.dia , 
			qtde =  isnull(sum(ni.Qtde*ni.Embalagem),0),
			vlr = isnull(sum(ni.Total),0) 
			
	  from #temUlt30 
				inner join nf_item as ni on 
					#temUlt30.plu collate database_default = ni.plu collate database_default 
				inner join nf  on nf.codigo=ni.Codigo 
								and nf.Tipo_NF = ni.Tipo_NF
								and nf.filial = ni.filial 
								and nf.Cliente_Fornecedor = ni.Cliente_Fornecedor
								and convert(varchar,#temUlt30.dia, 101)=convert(varchar,nf.Emissao, 101) 
								and isnull(nf.nf_Canc,0) =0 
								and nf.Filial collate database_default = @Filial collate database_default
				inner join Natureza_operacao as nop on nf.Codigo_operacao = nop.Codigo_operacao
				
				
		where 
		
			#temUlt30.plu collate database_default = @PLU collate database_default
			and #temUlt30.dia >= dateadd(Day,-30,GetDate())
			and nop.Saida = 1
		group by nf.Emissao, #temUlt30.dia, #temUlt30.plu
		order by convert(varchar,#temUlt30.dia, 102) Desc
		
	
		insert into #venda 
		select 
			#temUlt30.dia , 
			qtde =  sum(ISNULL(pdi.Qtde,0)*ISNULL(pdi.Embalagem,0)),
			vlr = isnull(sum(pdi.Total),0) 
			
	  from #temUlt30 
				inner join Pedido_itens as pdi on 
					#temUlt30.plu collate database_default = pdi.plu collate database_default 
				inner join pedido as pd  on pd.Pedido=pdi.Pedido
								and pd.Tipo = pdi.Tipo
								and pd.filial = pdi.filial 
								and convert(varchar,#temUlt30.dia, 101)=convert(varchar,pd.Data_cadastro, 101) 
								and isnull(pd.Status,0)<> 3 
								and pd.Filial collate database_default = @Filial collate database_default
								AND pd.pedido_simples = 1
		where 
		
			#temUlt30.plu collate database_default = @PLU collate database_default
			and #temUlt30.dia >= dateadd(Day,-30,GetDate())
			and pd.Tipo= 1
		group by pd.Data_cadastro, #temUlt30.dia, #temUlt30.plu
		order by convert(varchar,#temUlt30.dia, 102) Desc

	Select Dia = case 
				when datepart(weekday, dia)= 1 then 'DOM'
				when datepart(weekday, dia)= 2 then 'SEG'
				when datepart(weekday, dia)= 3 then 'TER'
				when datepart(weekday, dia)= 4 then 'QUA'
				when datepart(weekday, dia)= 5 then 'QUI'
				when datepart(weekday, dia)= 6 then 'SEX'
				else 'SAB' end + '-' + substring(convert(varchar,dia, 103), 1, 5)
			--data_movimento
		 , qtde =sum(qtde)
		 , vlr  =sum(vlr)
		 ,PrcMD = ISNULL(
			convert(decimal(9,2),
			(case when SUM(isnull(Vlr,0)) > 0 and
						  SUM(isnull(Qtde,0)) > 0 then
						SUM(isnull(Vlr,0))/SUM(isnull(Qtde,0)) else 0 end)),0)
	 From 	  #venda  
	group by dia
	order by convert(varchar,dia, 102) Desc
end


GO 


 
 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_br_Mercadoria_Acum]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_br_Mercadoria_Acum]
END
GO
CREATE  PROCEDURE [dbo].[sp_br_Mercadoria_Acum] 
	@PLU Char(6), @Filial varchar(20) output
AS

begin
-- sp_br_mercadoria_Acum '27456', 'matriz'
	declare @contador as integer
	declare @data as datetime

	set @contador = -12
	set @data = dateadd(month, @contador,getdate())
	
	create table #lixo (dataref datetime)
	
	while @contador <= 0
      begin
        insert into #lixo select dateadd(month, @contador, getdate())
		set @contador = @contador + 1
      end

	Select Data_movimento =sai.Data_movimento
		  ,Qtde = sum(sai.Qtde)
		  ,Vlr = sum(ISNULL(Sai.vlr,0)-ISNULL(sai.Desconto,0))
	 into #venda From #lixo lixo left join saida_Estoque Sai with(index=IX_saida_estoque_01 ) on 
						substring(convert(varchar,lixo.dataref,102),1,7)=substring(convert(varchar,sai.data_movimento,102),1,7) 
		And Sai.Data_Cancelamento is null 
		AND SAI.PLU=@PLU 
		AND Filial=@Filial
	 group by sai.Data_movimento
  
  insert into #venda
	  Select nf.emissao 
			 ,Qtde = sum(ni.Qtde*ni.Embalagem)
			 ,Vlr = sum(ni.total)
	  from #lixo lixo left join nf  on 
			substring(convert(varchar,lixo.dataref,102),1,7)=substring(convert(varchar,nf.Emissao,102),1,7)  
			inner join nf_item as ni on nf.codigo=ni.Codigo 
								and nf.Tipo_NF = ni.Tipo_NF
								and nf.filial = ni.filial 
								and nf.Cliente_Fornecedor = ni.Cliente_Fornecedor
			inner join Natureza_operacao as nop on nf.Codigo_operacao = nop.Codigo_operacao
			where 	isnull(nf.nf_Canc,0)=0
					AND ni.PLU=@PLU 
					AND nf.Filial=@Filial
					and nop.Saida = 1
	   group by nf.Emissao				


	   insert into #venda
	  Select pd.Data_cadastro
			 ,Qtde = sum(pdi.Qtde*pdi.Embalagem)
			 ,Vlr = sum(pdi.total)
	  from #lixo lixo left join Pedido as pd  on 
			substring(convert(varchar,lixo.dataref,102),1,7)=substring(convert(varchar,pd.Data_entrega,102),1,7)  
			inner join Pedido_itens as pdi on pd.Pedido=pdi.Pedido
								and pd.Tipo = pdi.Tipo
								and pd.filial = pdi.filial 
								
			where 	isnull(pd.Status,0)<>3
					AND pdi.PLU=@PLU 
					AND pd.Filial=@Filial
					AND pd.TIPO = 1
					AND pd.pedido_simples = 1
	   group by pd.Data_cadastro			


	select ORDEM =substring(convert(varchar,lixo.dataref,102),1,7),
		   MesAno = substring(convert(varchar,lixo.dataref,103),4,7),
		   qtde = sum(ISNULL(SAI.Qtde,0)), 
		   vlr = sum(ISNULL(SAI.vlr,0)), 
		   PrcMD =convert(decimal(9,2),avg(case when isnull(SAI.Vlr,0) > 0 
												 and isnull(SAI.Qtde,0) > 0 then 
												isnull(SAI.Vlr,0)/isnull(SAI.Qtde,0) 
											else 
												0
											end))
	From #lixo lixo left join #venda Sai  on 
		substring(convert(varchar,lixo.dataref,102),1,7)=substring(convert(varchar,sai.data_movimento,102),1,7) 
	group by substring(convert(varchar,lixo.dataref,102),1,7), substring(convert(varchar,lixo.dataref,103),4,7)
	order by substring(convert(varchar,lixo.dataref,102),1,7) desc
	
end


GO 

if not exists(select 1 from PARAMETROS where PARAMETRO='NAO_ATUALIZA_FORNECEDOR')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('NAO_ATUALIZA_FORNECEDOR'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'NÃO ATUALIZA DADOS REF FORNECEDOR MERCADORIA'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO

if not exists(select 1 from PARAMETROS where PARAMETRO='PED_DESC_PERMITIDO')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('PED_DESC_PERMITIDO'
           ,GETDATE()
           ,'0'
           ,GETDATE()
           ,'1,5'
           ,'Desconto maximo permitido sem senha'
           ,'D'
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_pagamento') 
            AND  UPPER(COLUMN_NAME) = UPPER('cod_barras'))
begin
	alter table nf_pagamento alter column cod_barras varchar(50)
end
else
begin
	alter table nf_pagamento add cod_barras varchar(50)
end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('conta_a_pagar') 
            AND  UPPER(COLUMN_NAME) = UPPER('cod_barras'))
begin
	alter table conta_a_pagar alter column cod_barras varchar(50)
end
else
begin
	alter table conta_a_pagar add cod_barras varchar(50)
end 
go 



 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_SmartPhone]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_Cons_SmartPhone]
END
GO
CREATE  PROCEDURE [dbo].[sp_Cons_SmartPhone]
	@filial    as varchar(20),
	@Tipo		as integer,
	@DATE      as varchar(11)
AS
--	exec sp_Cons_SmartPhone 'MATRIZ',0,'20200101'
--DECLARE @DATE AS DATETIME
--SET @DATE = GETDATE()

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
	
	
	


GO 
insert into Versoes_Atualizadas select 'Versão:1.266.810', getdate();


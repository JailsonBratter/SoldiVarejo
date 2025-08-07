
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('CST_IPI'))
begin
	alter table nf_item alter column CST_IPI VARCHAR(3)
end
else
begin
		
		alter table nf_item add CST_IPI VARCHAR(3)

end 

GO 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('base_ipi'))
begin
	alter table nf_item alter column base_ipi numeric(18,2)
end
else
begin
		
		alter table nf_item add base_ipi numeric(18,2)

end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('finNFe'))
begin
	alter table nf alter column finNFe int
end
else
begin
		
		alter table nf add finNFe int

end 
go

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('finNFe'))
begin
	alter table nf_item alter column finNFe int
end
else
begin
		
		alter table nf_item add finNFe int

end 

go
update nf set finNFe = 1 
from Natureza_operacao as np
where  nf.codigo_operacao =np.codigo_operacao and  finNFe is Null and np.NF_devolucao <> 1
go
update NF_Item set finNFe = 1  
from  Natureza_operacao as np, nf  
where  nf.Codigo=NF_Item.Codigo and  nf.codigo_operacao =np.codigo_operacao and  nf_item.finNFe is Null and np.NF_devolucao <>1

go
update nf set finNFe = 4 
from Natureza_operacao as np
where  nf.codigo_operacao =np.codigo_operacao and  finNFe is Null and np.NF_devolucao = 1
go

update NF_Item set finNFe = 4  
from  Natureza_operacao as np, nf  
where  nf.Codigo=NF_Item.Codigo and  nf.codigo_operacao =np.codigo_operacao and  nf_item.finNFe is Null and np.NF_devolucao =1

go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('total_faturado'))
begin
	alter table nf_item alter column total_faturado decimal(18,2)
end
else
begin
	alter table nf_item add total_faturado decimal(12,2)
end 

go 
update nf_item set total_faturado = total+iva 
go


/****** Object:  Index [ix_analise_de_vendas_por_dia]    Script Date: 04/13/2017 14:16:46 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Saida_estoque]') AND name = N'ix_analise_de_vendas_por_dia')
DROP INDEX [ix_analise_de_vendas_por_dia] ON [dbo].[Saida_estoque] WITH ( ONLINE = OFF )
GO



/****** Object:  Index [ix_analise_de_vendas_por_dia]    Script Date: 04/13/2017 14:16:46 ******/
CREATE NONCLUSTERED INDEX [ix_analise_de_vendas_por_dia] ON [dbo].[Saida_estoque] 
(
	[Filial] ASC,
	[PLU] ASC,
	[Hora_venda] ASC,
	[Data_movimento] ASC,
	[data_cancelamento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



/****** Object:  Index [ix_sp_rel_resumo_vendas]    Script Date: 04/26/2017 11:30:44 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Pedido_itens]') AND name = N'ix_sp_rel_resumo_vendas')
DROP INDEX [ix_sp_rel_resumo_vendas] ON [dbo].[Pedido_itens] WITH ( ONLINE = OFF )
GO


/****** Object:  Index [ix_sp_rel_resumo_vendas]    Script Date: 04/26/2017 11:30:44 ******/
CREATE NONCLUSTERED INDEX [ix_sp_rel_resumo_vendas] ON [dbo].[Pedido_itens] 
(
	[Pedido] ASC,
	[Filial] ASC,
	[tipo] ASC,
	[PLU] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas]    Script Date: 04/26/2017 11:26:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Resumo_Vendas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Resumo_Vendas]
GO


/****** Object:  StoredProcedure [dbo].[sp_Rel_Resumo_Vendas]    Script Date: 04/26/2017 11:26:34 ******/
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

if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN
SELECT
            saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            Vlr = Convert(Decimal(15,2),SUM(isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0))),
            CPF_CNPJ
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
      AND   Data_Movimento BETWEEN @DataDe AND @DataAte
      AND   Data_Cancelamento IS NULL
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

      Venda =       SUM(CASE WHEN Data_Cancelamento IS Null THEN Convert(Decimal(18,2),ISNULL(VLR,0)-ISNULL(desconto,0)) ELSE 0 END),
      Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento),
      Venda_MD =    CONVERT(DECIMAL(12,2), 
						CASE WHEN (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE #LIXO.Data_Movimento = Saida_Estoque.Data_Movimento) > 0 THEN

                  ((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE #LIXO.Data_Movimento = Saida_Estoque.Data_Movimento) /  (SELECT COUNT(*) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento))

                  ELSE 0 END),

      NFP =         (SELECT COUNT(*) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> ''),

      Vlr_NFP =     ISNULL((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> ''),0),

     

      Perc_NFP =    CASE WHEN (SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> '') > 0 THEN

                  CONVERT(DECIMAL(8,2), ((SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento AND #lixo.CPF_CNPJ <> '') /

                        (SELECT Convert(Decimal(18,2),SUM(#Lixo.Vlr)) FROM #Lixo WHERE #Lixo.Data_Movimento = Saida_Estoque.Data_Movimento)) * 100)

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









GO




/****** Object:  Index [Ix_Fluxo_Caixa]    Script Date: 04/17/2017 10:54:25 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Saida_estoque]') AND name = N'Ix_Fluxo_Caixa')
DROP INDEX [Ix_Fluxo_Caixa] ON [dbo].[Saida_estoque] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Ix_Fluxo_Caixa]    Script Date: 04/17/2017 10:54:25 ******/
CREATE NONCLUSTERED INDEX [Ix_Fluxo_Caixa] ON [dbo].[Saida_estoque] 
(
	[Filial] ASC,
	[Data_movimento] ASC,
	[data_cancelamento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




/****** Object:  Index [ix_pedido_fluxo_caixa]    Script Date: 04/17/2017 10:55:18 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Pedido]') AND name = N'ix_pedido_fluxo_caixa')
DROP INDEX [ix_pedido_fluxo_caixa] ON [dbo].[Pedido] WITH ( ONLINE = OFF )
GO



/****** Object:  Index [ix_pedido_fluxo_caixa]    Script Date: 04/17/2017 10:55:18 ******/
CREATE NONCLUSTERED INDEX [ix_pedido_fluxo_caixa] ON [dbo].[Pedido] 
(
	[Tipo] ASC,
	[pedido_simples] ASC,
	[Data_cadastro] ASC,
	[Status] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Lista_finalizadora]') AND name = N'ix_Lista_Fluxo_Caixa')
DROP INDEX [ix_Lista_Fluxo_Caixa] ON [dbo].[Lista_finalizadora] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [ix_Lista_Fluxo_Caixa]    Script Date: 04/17/2017 10:56:50 ******/
CREATE NONCLUSTERED INDEX [ix_Lista_Fluxo_Caixa] ON [dbo].[Lista_finalizadora] 
(
	[finalizadora] ASC,
	[Emissao] ASC,
	[cancelado] ASC,
	[filial] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_FluxoCaixa]    Script Date: 04/26/2017 10:07:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_FluxoCaixa]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa]
GO


/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_FluxoCaixa]    Script Date: 04/26/2017 10:07:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--PROCEDURES =======================================================================================

-- exec sp_Rel_Fin_FluxoCaixa 'MATRIZ','20160101','20160131'
CREATE  PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8)
AS

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Totais]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)

Begin

Drop table Totais

End
	
	Create table Totais
(
	Descricao  varchar(90),
	Total	 Decimal(18,2)
);
	
	
	select count(*) as Qtde into #cupons from saida_Estoque WITH(INDEX(Ix_Fluxo_Caixa)) where 
		Filial  = @Filial And
		Data_Movimento BETWEEN @DataDe AND @DataAte AND 
		Data_Cancelamento IS NULL
	group by documento, filial, caixa_saida
	
	insert into #cupons 
	select COUNT(*) from nf where nf.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf.Filial = @FILIAL and nf.status='AUTORIZADO'
	group by Codigo
	
	insert into #cupons
	select COUNT(*) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte  and pedido.Status <>3
	GROUP by pedido
	
	----# Insere Todo Faturamento em uma tabela temp (Para fazer calculo) #------
	Insert into Totais
	SELECT 'TOTAL FATURAMENTO' Descricao,CONVERT(VARCHAR,
					Isnull((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE Filial = @FILIAL and  Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL 
					),0)
					+Isnull((SELECT Sum(isnull(convert(decimal(10,2),nf_item.Total),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo AND nf_item.Filial = nf.FILIAL where  nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf_item.Filial = @FILIAL and nf.status='AUTORIZADO' ),0) 
					+Isnull((SELECT Sum(isnull(Total,0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte),0)
					)  Total

	----# Insere Todas VENDAS CANCELADAS do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'VENDAS CANCELADAS', CONVERT(VARCHAR,ISNULL((SELECT SUM(VLR-isnull(desconto,0)) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  FILIAL = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte  AND Data_Cancelamento IS NOT NULL),0))  				
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'ITENS CANCELADOS PEDIDO', CONVERt(VARCHAR,ISNULL(Sum(isnull(Total,0)),0)) from pedido with(index(ix_pedido_fluxo_caixa)) where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte And Pedido.Status in (3)

	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'AJUSTE DEVOLUCAO', isnull(CONVERt(VARCHAR,Convert(Decimal(18,2),Sum(isnull(Contada*Custo,0)))),0) from Inventario_itens itens inner join Inventario i on i.Codigo_inventario =  itens.Codigo_inventario 
		where Data BETWEEN @DataDe AND @DataAte  and status = 'ENCERRADO' And tipoMovimentacao = 'DEVOLUCAO'		
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'NF DEVOLUCAO', CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(10,2),nf.Total),0)),0)) from nf Where nf.codigo_operacao in ('1202', '5202', '5411', '6202') and nf.Tipo_NF=2 and Isnull(nf.nf_Canc,0) = 0 and nf.Emissao between @dataDe and @DataAte		
		
	SELECT '' As Descritivo,'' As Total
	UNION ALL
	SELECT 'VENDAS PEDIDOS SIMPLIFICADOS', replace(CONVERt(VARCHAR,Sum(convert(decimal(18,2),isnull(Total,0),0))),'.',',') from pedido with(index(ix_pedido_fluxo_caixa)) where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte and pedido.Status <>3 
	UNION ALL
	SELECT 'VENDAS NOTA FISCAL', replace(CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(18,2),nf_item.Total),0)-(isnull(NF_Item.Total,0)*isnull(nf_item.desconto,0)/100)),0)),'.',',') from nf_item inner join nf on nf_item.codigo= nf.Codigo  AND nf_item.Filial = nf.FILIAL where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf_item.Filial = @FILIAL and nf.status='AUTORIZADO'
	UNION ALL
	SELECT 'VENDAS CUPOM',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE FILIAL = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL 
			AND Filial = @FILIAL),0)),'.',',')
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'TOTAL FATURAMENTO'
					 
	UNION ALL
				
		SELECT '',''
	UNION ALL
	SELECT 'NUMERO DE CLIENTES', Convert(varchar, (select count(*) from #Cupons))		
	UNION ALL
	SELECT 'VENDAS COM NFP', REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where Filial = @filial and data_movimento between @DataDe and @DataAte and data_cancelamento is null  and len(isnull(cpf_cnpj,''))>10))),'.',',')
	UNION ALL
	SELECT '% DE VENDAS COM NFP',replace(CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where filial = @filial and data_movimento between @DataDe and @DataAte and data_cancelamento is null  and len(isnull(cpf_cnpj,''))>10) / (Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null)*100)),'.',',')
	UNION ALL
	SELECT 'QTDE ITENS VENDIDOS',REPLACE( CONVERT(VARCHAR,CONVERt(Int,ISNULL((SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  Filial = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL ),0))),'.',',')
	UNION ALL
	SELECT 'QTDE ITENS CANCELADOS VENDA', replace(CONVERT(VARCHAR,ISNULL(convert(Numeric(18),(SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  Filial = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NOT NULL )),0)),'.',',')	
	UNION ALL
	SELECT '',''
	
	UNION ALL
	SELECT Descricao,replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'VENDAS CANCELADAS'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'ITENS CANCELADOS PEDIDO'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'AJUSTE DEVOLUCAO'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'NF DEVOLUCAO'

	UNION ALL
	
	SELECT 'RESULTADO TOTAL',
		replace(CONVERT(VARCHAR(90),Convert(Decimal(18,2),SUM(Total) - (Select SUM(Total) From TOTAIS  Where Descricao = 'VENDAS CANCELADAS') - (Select SUM(Total) From TOTAIS  Where Descricao = 'ITENS CANCELADOS PEDIDO') - (Select SUM(Total) From TOTAIS  Where Descricao = 'AJUSTE DEVOLUCAO') - (Select Sum(Total) From TOTAIS Where Descricao = 'NF DEVOLUCAO'))),'.',',')
	FROM  TOTAIS
	Where Descricao = 'TOTAL FATURAMENTO'

	UNION ALL
	SELECT '',''
	UNION ALL
	SELECT 'DESCONTOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(desconto,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial))),'.',',')
	UNION ALL
	SELECT 'ACRESCIMOS SERVICOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(Acrescimo,0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where filial = @filial and  data_movimento between @DataDe and @DataAte and data_cancelamento is null ))),'.',',')
	UNION ALL
	SELECT 'INDUSTRIA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES EMITIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES RECEBIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS EMITIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS RECEBIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'PAGAMENTO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'ESTORNO DE DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
    SELECT 'GERENCIAL ',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE Filial = @FILIAL AND Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND CONVERT(NUMERIC, ISNULL(COO,0)) <= 0 ),0)),'.',',')
	UNION ALL
	SELECT 'REPIQUE', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT '',''
	UNION ALL
	
	SELECT 'Forma de Pagamento','Valor Total'
	UNION ALL
	SELECT FINALIZADORA.FINALIZADORA,replace(CONVERt(VARCHAR,SUM(TOTAL)),'.',',')
		FROM LISTA_FINALIZADORA  WITH(INDEX(ix_Lista_Fluxo_Caixa))
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO between @dataDe and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL
		GROUP BY FINALIZADORA.FINALIZADORA
	UNION ALL	
	SELECT 'Valor Total',replace(CONVERt(VARCHAR,SUM(ISNULL(TOTAL,0))),'.',',')
		FROM LISTA_FINALIZADORA WITH(INDEX(ix_Lista_Fluxo_Caixa))
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO  between @datade and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL 
	UNION ALL
	SELECT '',''
	  




GO





update mercadoria set curva_a=1 ,curva_b=1,curva_c=1
go 





/****** Object:  StoredProcedure [dbo].[sp_Rel_Curva_ABC]    Script Date: 04/17/2017 11:08:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Curva_ABC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Curva_ABC]
GO


/****** Object:  StoredProcedure [dbo].[sp_Rel_Curva_ABC]    Script Date: 04/17/2017 11:08:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--PROCEDURES =======================================================================================
CREATE    Procedure [dbo].[sp_Rel_Curva_ABC] 
                @Filial                  As Varchar(20),
                @DataDe                            As Varchar(8),
                @DataAte          As Varchar(8),
                @nLinhas           As Integer,
                @Descricao        As Varchar(60),
                @Grupo            As Varchar(60),
                @subGrupo		as Varchar(60),
                @Departamento		as Varchar(60),
                @Familia		as Varchar(60),
                @Ordem			as Varchar(40)
AS
                
                Declare @String               As nVarchar(2000)
		Declare @Where               As nVarchar(1024)
BEGIN
	
                --** Cria A String Com Os filtros selecionados
		Set @Where = ' WHERE Saida_Estoque.Filial = ' + CHAR(39) + @Filial + CHAR(39) + ' And  Data_Movimento BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		Set @Where = @Where + ' And  data_cancelamento is null and (isnull(mercadoria.curva_a,0)=1 and isnull(mercadoria.curva_b,0)=1 and isnull(mercadoria.curva_c,0) = 1) '
		--** Checa se o parametro @DESCRICAO contem alguma informação. Se tiver, 
                --** o sistema faz um LIKE no campo descrição recebido no parametro.
                BEGIN
                               IF LEN(ISNULL(@DESCRICAO,'')) > 0 
                                               SET @Where = @Where + ' AND Mercadoria.Descricao LIKE '+ CHAR(39) + @DESCRICAO + CHAR(39)
                END       
                --** Checa se o parametro @Grupo contem alguma informação. Se tiver, 
                
                BEGIN
                               IF LEN(ISNULL(@GRUPO,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_GRUPO='+ CHAR(39) + @GRUPO + CHAR(39) 
                               IF LEN(ISNULL(@subGrupo,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_SUBGRUPO='+ CHAR(39) + @subGrupo + CHAR(39) 
							   IF LEN(ISNULL(@Departamento,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_DEPARTAMENTO='+ CHAR(39) + @Departamento + CHAR(39) 
							   IF LEN(ISNULL(@Familia,'')) > 0 
                                               SET @Where = @Where + ' AND familia.DESCRICAO_familia='+ CHAR(39) + @Familia + CHAR(39) 
                
                END       
		
		Begin
			if LEN(ISNULL(@Ordem,'')) = 0 
			Set @Ordem ='Qtde'
		end
		
		-- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
                --** Checa se o parametro @nLinhas está com valor maior que 0. Se estiver, 
                --** o sistema retorna o numero de linhas recebido no parametro.
                BEGIN
                               if ISNULL(@nLinhas, 0) > 0 
                                               SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)
                END
                SET @String = @String + ' ROW_NUMBER() over(order by Convert(Numeric(10,3), Sum(Qtde)) desc) as RANK, Mercadoria.PLU, EAN = ISNULL((SELECT MAX(EAN.EAN) From EAN WHERE EAN.PLU = Mercadoria.PLU), ' + CHAR(39) + CHAR(39) + '), Mercadoria.Descricao,'
                SET @String = @String + ' [Custo] = MERCADORIA.PRECO_CUSTO, '
                SET @String = @String + ' [Margem] = MERCADORIA.MARGEM, '
                SET @String = @String + ' [Preco] = MERCADORIA.PRECO, '
                                 
                SET @String = @String + ' Qtde = Convert(Numeric(10,3), Sum(Qtde)), '
                --SET @String = @String + ' [Total Custo] = Convert(Numeric(12,2), SUM(Qtde * Mercadoria.Preco_Custo)),'
                SET @String = @String + ' [Valor] = Convert(Numeric(12,2), Sum(isnull(VLR,0)-isnull(desconto,0))), '
                set @String = @String + ' [Rentabilidade] = Convert(Numeric(12,2),SUM((isnull(VLR,0)-isnull(desconto,0)) - (Qtde * isnull(Mercadoria.Preco_Custo,0))))'
  --              Set @String = @String + ', [%] = Convert(Varchar,convert(numeric(12,2), Convert(Numeric(12,2),'
		--if(@Ordem='VALOR')
		--BEGIN
		--		 Set @String = @String + ' Sum(VLR-desconto)) / SELECT SUM(VLR)FROM ((select '
		--		 if ISNULL(@nLinhas, 0) > 0 
  --                      SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)	
		--		Set @String = @String + ' convert(numeric(12,2), (vlr-desconto)) AS VLR from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia) AS T'
		--END
		--ELSE if(@Ordem='QTDE')
		--BEGIN
		--		Set @String = @String + ' Sum(Qtde)) / (select '
		--		 if ISNULL(@nLinhas, 0) > 0 
  --                      SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)	
		--		Set @String = @String + ' Sum(ISNULL(saida_estoque.Qtde,0)) from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia'
		--END
		
		--ELSE if(@Ordem='RENTABILIDADE')
		--BEGIN
		--		Set @String = @String + ' SUM((VLR-desconto) - (Qtde * Mercadoria.Preco_Custo))) / (select '
		--		 if ISNULL(@nLinhas, 0) > 0 
  --                      SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)	
		--		Set @String = @String + ' SUM((VLR-desconto) - (Qtde * Mercadoria.Preco_Custo)) from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia'
		--END
		----** Inserindo Clausula Subquery
		--Set @String = @String + @Where + ' )  * 100)) + '+ char(39)+ '%'+ char(39)
        SET @String = @String + ' From Mercadoria INNER JOIN Saida_Estoque  WITH (INDEX(Ix_Fluxo_Caixa)) ON Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia '
 		--**Adciona aqui a Variavel @where da Query
		set @String = @String + @Where
	
		
                SET @String = @string + ' GROUP BY  Mercadoria.PLU, Mercadoria.Descricao, MERCADORIA.PRECO_CUSTO,MERCADORIA.MARGEM,MERCADORIA.PRECO  Order by ['+@Ordem+'] Desc'
 
 -- PRINT @STRING
                EXECUTE(@String)
END

GO



/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_PorOperadorCancelamento]    Script Date: 04/18/2017 09:19:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperadorCancelamento]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Rel_Fin_PorOperadorCancelamento]
GO



/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_PorOperadorCancelamento]    Script Date: 04/18/2017 09:19:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--PROCEDURES =======================================================================================
-- exec sp_Rel_Fin_PorOperadorCancelamento 'MATRIZ','20160101','20160131','TODOS','TODOS'
CREATE  PROCEDURE [dbo].[sp_Rel_Fin_PorOperadorCancelamento](

      @FILIAL			AS VARCHAR(17),
      @Datade           As varchar(8),
      @Dataate			As varchar(8),
      @Operador         As varchar(20),
      @Pdv				As varchar(20))

as
	declare @strQuery as nvarchar(3000)
	declare @strWhere As nVarchar(1024)
    set @strWhere = ''
    
if len(@Operador) > 0 AND @Operador <>'TODOS'
begin
    set @strWhere = ' AND b.Nome LIKE ' + CHAR(39) + @Operador + '%' + CHAR(39)     
end
      
if (LEN(@Pdv) > 0 AND @Pdv <>'TODOS')
begin
	set @strWhere = @strWhere + ' AND a.PDV = ' + @PDV 
end
 
set @strQuery = 'select b.Nome, a.Pdv, convert(varchar,a.emissao,103)Data ' 
set @strQuery = @strQuery + ', Isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv = c.pdv and isnull(c.cancelado,0)=0), 0) as  Vendas'
set @strQuery = @strQuery + ', isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv= c.pdv and   isnull(c.cancelado,0)=1),0)as  Cancelados'
set @strQuery = @strQuery + ' from lista_finalizadora a inner join operadores b on  a.operador= b.id_operador '
set @strQuery = @strQuery + ' where a.filial= ' + char(39) + @FILIAL + char(39) + ' and a.emissao  BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39) 
set @strQuery = @strQuery + @strWhere
set @strQuery = @strQuery + ' group by a.operador,a.Pdv, b.nome,a.emissao '
set @strQuery = @strQuery + ' order by a.Pdv, b.nome'

--print @strQuery 
execute(@strQuery)


go 

/****** Object:  Index [IX_Saida_Estoque_ResumoPorDia]    Script Date: 04/18/2017 09:29:14 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Saida_estoque]') AND name = N'IX_Saida_Estoque_ResumoPorDia')
DROP INDEX [IX_Saida_Estoque_ResumoPorDia] ON [dbo].[Saida_estoque] WITH ( ONLINE = OFF )
GO


/****** Object:  Index [IX_Saida_Estoque_ResumoPorDia]    Script Date: 04/18/2017 09:29:14 ******/
CREATE NONCLUSTERED INDEX [IX_Saida_Estoque_ResumoPorDia] ON [dbo].[Saida_estoque] 
(
	[Filial] ASC,
	[Data_movimento] ASC,
	[data_cancelamento] ASC,
	[Hora_venda] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




/****** Object:  StoredProcedure [dbo].[sp_rel_Resumo_Vendas_Pdv]    Script Date: 04/18/2017 09:24:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_Resumo_Vendas_Pdv]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_rel_Resumo_Vendas_Pdv]
GO



/****** Object:  StoredProcedure [dbo].[sp_rel_Resumo_Vendas_Pdv]    Script Date: 04/18/2017 09:24:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--PROCEDURES =======================================================================================
CREATE  Procedure [dbo].[sp_rel_Resumo_Vendas_Pdv] 
		@Filial		As Varchar(20),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@PDV		AS VARCHAR(10)

AS
	Declare @String	As nVarchar(1024)
begin
	set @String= '	SELECT '
	
	SET @String = @String + '	convert(varchar,Data_Movimento,103) as Data,'
	Set @String = @String + 'Dia_da_Semana = case 
								when datepart(dw, Data_Movimento) = 2 then '+ CHAR(39) +'Segunda'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 3 then '+ CHAR(39) +'Terça'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 4 then '+ CHAR(39) +'Quarta'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 5 then '+ CHAR(39) +'Quinta'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 6 then '+ CHAR(39) +'Sexta'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 7 then '+ CHAR(39) +'Sabado'+ CHAR(39) +
								'Else '+ CHAR(39) +'Domingo'+ CHAR(39) +' end,'
	SET @String = @String + '	Caixa_Saida as PDV, '
	SET @String = @String + '	Valor = SUM(Vlr-isnull(Desconto,0)+isnull(acrescimo,0))'
		
	SET @String = @String + ' FROM '
	SET @String = @String + '	Saida_Estoque WITH (INDEX(IX_Saida_Estoque_ResumoPorDia))'
	SET @String = @String + ' WHERE '
	SET @String = @String + '	Filial = '+ CHAR(39) +@Filial+ CHAR(39) 
	SET @String = @String + ' AND '
	SET @String = @String + '	Data_Movimento BETWEEN '+ CHAR(39) + @DataDe + CHAR(39) +' AND '+ CHAR(39) +@DataAte + CHAR(39) 
	SET @String = @String + ' AND '
	SET @String = @String + '	Data_Cancelamento IS NULL ' 
	IF (LEN(ISNULL(@PDV,'')) > 0 and @PDV <> 'TODOS')
	begin 
		SET @String = @String + ' and CAIXA_SAIDA IN (' +@PDV+ ')'
	end	
				
	SET @String = @String + 'GROUP BY Filial,Data_Movimento, Caixa_Saida'
	EXECUTE(@String)
--	print @string
end



GO



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.158.596', getdate();
GO
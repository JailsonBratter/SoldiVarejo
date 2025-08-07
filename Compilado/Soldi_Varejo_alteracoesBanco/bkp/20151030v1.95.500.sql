
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Venda_pedido_simplificado]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Venda_pedido_simplificado
end
GO
CREATE PROCEDURE [dbo].[sp_Rel_Venda_pedido_simplificado] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cliente   as varchar(40),
		@simples	as varchar(5),
		@nome		as Varchar(30) = ''
		
    	as
			-- exec sp_rel_venda_pedido_simplificado 'MATRIZ','20150101','20151021','160','',''
	
if len(@cliente)>0
begin

	SELECT  P.PLU ,M.Descricao, QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),UNITARIO = ROUND( UNITARIO ,2),VALOR=ROUND(SUM(p.total),2)
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND Pd.Status in (1,2)
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')   )   
	And PD.Data_cadastro between @DataDe and @DataAte
	GROUP BY P.PLU,M.Descricao,P.unitario

	UNION ALL

	SELECT '','TOTAL',QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),0,VALOR=ROUND(SUM(p.total),0)
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, isnull(PD.pedido_simples,0)) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND Pd.Status in (1,2)
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')    )
	And PD.Data_cadastro between @DataDe and @DataAte
	
end
else		
begin
		Select 
			Simples = case when Pedido.pedido_simples=1 then 'SIM'ELSE 'NAO' END ,
			Pedido,
			convert(varchar,pedido.Data_cadastro,103) Data, 
			Cliente_Fornec Cod,
			cliente.Nome_Cliente,
			--VlrVenda=ROUND(Total,2), 
			
			[Total Compra] = (SELECT Convert(Decimal(12,2),Isnull(SUM(m.Preco_Custo*P.Qtde),0))
			FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
			where  P.Pedido = Pedido.Pedido And P.Filial = Pedido.Filial And P.tipo = Pedido.Tipo ),
			
			[Total Venda]=ROUND(Total,2), 

			[Total Pedido] = (SELECT Convert(Decimal(12,2),Isnull(SUM(P.Qtde*P.Embalagem*(P.unitario)),0))
			FROM Pedido_itens P where  P.Pedido = Pedido.Pedido And P.Filial = Pedido.Filial And P.tipo = Pedido.Tipo),
			ISNULL(Pedido.funcionario, '' ) As Vendedor 
		 From pedido inner join cliente on pedido.Cliente_Fornec= cliente.Codigo_Cliente 
		 Where  
			CONVERT(VARCHAR, Pedido.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
		  and 
			pedido.Data_cadastro between @DataDe and @DataAte
		  AND 
			Pedido.Tipo =1 
		  AND 
			Pedido.Status in (1,2)
          And 
			isnull(Pedido.funcionario,'') like (case when @nome  <> '' then @Nome else '%' end)
		UNION ALL 
		Select 
			Simples = '',
			Pedido ='',
			Data='', 
			Cod='',
			Nome_Cliente='TOTAL',
			[Total Compra] = (SELECT Convert(Decimal(12,2),Isnull(SUM(m.Preco_Custo*P.Qtde),0))
									FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU INNER JOIN PEDIDO PD ON PD.Pedido = P.PEDIDO And P.tipo = PD.Tipo
								where   P.Filial = Pedido.Filial  AND PD.Data_cadastro between @DataDe and @DataAte AND PD.Status in (1,2) and Pd.Tipo=1 
								and CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END	) ,
						
		    [Total Venda]=ROUND(SUM(Total),2), 

			[Total Pedido] = (SELECT Convert(Decimal(12,2),Isnull(SUM(P.Qtde*P.Embalagem*(P.unitario)),0))
								FROM Pedido_itens P INNER JOIN PEDIDO PD ON PD.Pedido = P.PEDIDO And P.tipo = PD.Tipo where  P.Filial = Pedido.Filial AND 
								PD.Tipo =1 AND PD.Status in (1,2) AND PD.Data_cadastro between @DataDe and @DataAte
								and CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END	
								 ),
			Vendedor = ''
		 From pedido inner join cliente on pedido.Cliente_Fornec= cliente.Codigo_Cliente 
		 Where  
			CONVERT(VARCHAR, Pedido.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
		  and 
			pedido.Data_cadastro between @DataDe and @DataAte
		  AND 
			Pedido.Tipo =1 
		 AND 
			Pedido.Status in (1,2)
		  And 
			isnull(Pedido.funcionario,'') like (case when @nome  <> '' then @Nome else '%' end)
		GROUP BY PEDIDO.Filial		 
 end


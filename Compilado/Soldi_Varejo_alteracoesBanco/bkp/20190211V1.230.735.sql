
if not exists(select 1 from PARAMETROS where PARAMETRO='PEDIDO_VENDA_RAPIDO')
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
           ('PEDIDO_VENDA_RAPIDO'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'INCLUIR ITENS DO PEDIDO DE FORMA RAPIDA'
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
IF OBJECT_ID('[sp_br_gera_pedidos_GV]', 'P') IS NOT NULL
begin 
      drop procedure [sp_br_gera_pedidos_GV]
end 

go
CREATE   procedure [dbo].[sp_br_gera_pedidos_GV]
	@cotacao int , @filial varchar(20)
as
	declare  @vencedor varchar(20),
		@pedido varchar(20),
		@mercadoria varchar(17),
		@preco decimal(12,2),
		@qtde decimal(9,3),
		@embalagem decimal(9,3),
		@prazoPgo  dateTime ,
		@prazoEntrega dateTime,
		@ean varchar(20)

--GV

	select @vencedor = (select top 1 vencedor from (select vencedor, count(*) vzs from cotacao_item where cotacao=@cotacao group by vencedor) x order by vzs desc)

	exec LX_SEQUENCIAL 'COMPRAS.PEDIDO', null, @pedido output

	set @prazoPgo =(
	Select top 1  (getdate()+cotacao_DIGITACAO.Prazo_pgto)  from cotacao_DIGITACAO 
		inner join cotacao_item 
			on (cotacao_item.cotacao = cotacao_DIGITACAO.cotacao and cotacao_item.filial = cotacao_DIGITACAO.filial and 
					cotacao_item.mercadoria = cotacao_digitacao.mercadoria)
		where cotacao_DIGITACAO.cotacao = @cotacao and cotacao_DIGITACAO.filial = @filial AND FORNECEDOR = @vencedor and cotacao_DIGITACAO.preco > 0 and cotacao_DIGITACAO.qtde > 0 
		
	)
	set  @prazoEntrega=(Select top 1  (getdate()+cotacao_DIGITACAO.Prazo_entrega)from cotacao_DIGITACAO 
		inner join cotacao_item 
			on (cotacao_item.cotacao = cotacao_DIGITACAO.cotacao and cotacao_item.filial = cotacao_DIGITACAO.filial and 
					cotacao_item.mercadoria = cotacao_digitacao.mercadoria)
		where cotacao_DIGITACAO.cotacao = @cotacao and cotacao_DIGITACAO.filial = @filial AND FORNECEDOR = @vencedor and cotacao_DIGITACAO.preco > 0 and cotacao_DIGITACAO.qtde > 0 
		);
		

	insert into pedido (filial, pedido, status, tipo, cliente_fornec, Data_cadastro, Data_entrega, hora, desconto, total, usuario, obs, cotacao) 
		values(@filial,@pedido,1,2,@vencedor,convert(varchar,getdate(),102), convert(varchar,@prazoEntrega,102), null, 0, 0, 'COTACAO', 'GERADO VIA COTACAO GV', @cotacao)
		
	
	insert into pedido_Pagamento(Filial,Pedido,Tipo,Tipo_pagamento,Valor,Vencimento)
							values(@filial,@pedido,2,'BOLETO',0,convert(varchar,@prazoPgo,102))


	declare x_itensA cursor for 
		select cotacao_DIGITACAO.MERCADORIA, cotacao_DIGITACAO.PRECO, cotacao_item.quantidade,cotacao_item.embalagem ,cotacao_item.EAN
		from cotacao_DIGITACAO 
		inner join cotacao_item 
			on (cotacao_item.cotacao = cotacao_DIGITACAO.cotacao and cotacao_item.filial = cotacao_DIGITACAO.filial and 
					cotacao_item.mercadoria = cotacao_digitacao.mercadoria)
		where cotacao_DIGITACAO.cotacao = @cotacao and cotacao_DIGITACAO.filial = @filial AND FORNECEDOR = @vencedor and cotacao_DIGITACAO.preco > 0 and cotacao_DIGITACAO.qtde > 0

	open x_itensA 
		
	FETCH NEXT FROM x_itensA
		INTO @mercadoria, @preco, @qtde,@embalagem,@ean

	

	WHILE @@FETCH_STATUS = 0
	BEGIN
	--iTENS

		insert into pedido_itens (filial, pedido, plu,ean, qtde, embalagem, unitario, total,tipo)
			values(@filial, @pedido, @mercadoria,@ean, @qtde, @embalagem, @preco, @qtde * @preco,2)
		
		
		FETCH NEXT FROM x_itensA
			INTO @mercadoria, @preco, @qtde, @embalagem, @ean

	END


	close x_itensA
	deallocate x_itensA

-- Faz o restante

	declare x_fornecedores cursor for select distinct a.vencedor 
		from cotacao_item a
		where cotacao = @cotacao and filial = @filial and mercadoria not in (
			select plu 
				from pedido_itens 
				where pedido in (select pedido from pedido where cotacao = @cotacao))

	open x_fornecedores

	FETCH NEXT FROM x_fornecedores
	INTO @vencedor

	WHILE @@FETCH_STATUS = 0
	BEGIN

		exec LX_SEQUENCIAL 'PEDIDO.PEDIDO', null, @pedido output

		insert into pedido (filial, pedido, status, tipo, cliente_fornec, Data_cadastro, Data_entrega, hora, desconto, total, usuario, obs, cotacao) 
			values(@filial,@pedido,1,2,@vencedor,convert(varchar,getdate(),102), null, null, 0, 0, 'COTACAO', 'GERADO VIA COTACAO', @cotacao)

		declare x_itens cursor for 
			select cotacao_DIGITACAO.MERCADORIA, cotacao_DIGITACAO.PRECO, cotacao_item.quantidade,cotacao_item.embalagem
			from cotacao_DIGITACAO 
			inner join cotacao_item 
				on (cotacao_item.cotacao = cotacao_DIGITACAO.cotacao and cotacao_item.filial = cotacao_DIGITACAO.filial and 
						cotacao_item.mercadoria = cotacao_digitacao.mercadoria and vencedor = fornecedor)
			where cotacao_DIGITACAO.cotacao = @cotacao and cotacao_DIGITACAO.filial = @filial AND FORNECEDOR = @vencedor

		open x_itens 

		FETCH NEXT FROM x_itens
			INTO @mercadoria, @preco, @qtde,@embalagem

		WHILE @@FETCH_STATUS = 0
		BEGIN
		--iTENS

			insert into pedido_itens (filial, pedido, plu, qtde, embalagem, unitario, total,tipo)
				values(@filial, @pedido, @mercadoria, @qtde, @embalagem, @preco, @qtde * @preco,2)


			FETCH NEXT FROM x_itens
				INTO @mercadoria, @preco, @qtde

		END


		close x_itens
		deallocate x_itens


		FETCH NEXT FROM x_fornecedores
		INTO @vencedor

	END

	close x_fornecedores
	deallocate x_fornecedores


	update a set total = (select sum(pedido_itens.total) from pedido_itens where pedido_itens.pedido = a.pedido and pedido_itens.filial = a.filial)
	from pedido a
	where filial = @filial and cotacao = @cotacao

	update pedido_Pagamento set  valor = (select total from pedido where Filial = @filial and  pedido=@pedido and tipo=2)
	where Filial = @filial and  pedido=@pedido and tipo=2

	return (1)

go 




IF OBJECT_ID('[sp_br_gera_pedidos]', 'P') IS NOT NULL
begin 
      drop procedure [sp_br_gera_pedidos]
end 

go
CREATE   procedure [dbo].[sp_br_gera_pedidos]
	@cotacao int , @filial varchar(20)
as

	declare  @vencedor varchar(20),
		@pedido varchar(20),
		@mercadoria varchar(17),
		@preco decimal(12,2),
		@qtde decimal(9,3),
		@embalagem decimal(9,3),
		@prazoPgo  dateTime ,
		@prazoEntrega dateTime,
		@ean varchar(20)

		


	declare x_fornecedores cursor for select distinct vencedor from cotacao_item where cotacao = @cotacao and filial = @filial and Vencedor is not null

	open x_fornecedores

	FETCH NEXT FROM x_fornecedores
	INTO @vencedor

	WHILE @@FETCH_STATUS = 0
	BEGIN

		exec LX_SEQUENCIAL 'COMPRAS.PEDIDO', null, @pedido output
		set @prazoPgo =(
	Select top 1  (getdate()+cotacao_DIGITACAO.Prazo_pgto)  from cotacao_DIGITACAO 
		inner join cotacao_item 
			on (cotacao_item.cotacao = cotacao_DIGITACAO.cotacao and cotacao_item.filial = cotacao_DIGITACAO.filial and 
					cotacao_item.mercadoria = cotacao_digitacao.mercadoria)
		where cotacao_DIGITACAO.cotacao = @cotacao and cotacao_DIGITACAO.filial = @filial AND FORNECEDOR = @vencedor and cotacao_DIGITACAO.preco > 0 and cotacao_DIGITACAO.qtde > 0 
		
	)
	set  @prazoEntrega=(Select top 1  (getdate()+cotacao_DIGITACAO.Prazo_entrega)from cotacao_DIGITACAO 
		inner join cotacao_item 
			on (cotacao_item.cotacao = cotacao_DIGITACAO.cotacao and cotacao_item.filial = cotacao_DIGITACAO.filial and 
					cotacao_item.mercadoria = cotacao_digitacao.mercadoria)
		where cotacao_DIGITACAO.cotacao = @cotacao and cotacao_DIGITACAO.filial = @filial AND FORNECEDOR = @vencedor and cotacao_DIGITACAO.preco > 0 and cotacao_DIGITACAO.qtde > 0 
		);
		
			
		insert into pedido (filial, pedido, status, tipo, cliente_fornec, Data_cadastro, Data_entrega, hora, desconto, total, usuario, obs, cotacao) 
			values(@filial,@pedido,1,2,@vencedor,convert(varchar,getdate(),102), @prazoEntrega, null, 0, 0, 'COTACAO', 'GERADO VIA COTACAO', @cotacao)
		
		
		insert into pedido_Pagamento(Filial,Pedido,Tipo,Tipo_pagamento,Valor,Vencimento)
							values(@filial,@pedido,2,'BOLETO',0,convert(varchar,@prazoPgo,102))


		declare x_itens cursor for 
			select cotacao_DIGITACAO.MERCADORIA, cotacao_DIGITACAO.PRECO, cotacao_item.quantidade,cotacao_item.embalagem, cotacao_item.EAN
			from cotacao_DIGITACAO 
			inner join cotacao_item 
				on (cotacao_item.cotacao = cotacao_DIGITACAO.cotacao and cotacao_item.filial = cotacao_DIGITACAO.filial and 
						cotacao_item.mercadoria = cotacao_digitacao.mercadoria and vencedor = fornecedor)
			where cotacao_DIGITACAO.cotacao = @cotacao and cotacao_DIGITACAO.filial = @filial AND FORNECEDOR = @vencedor

		open x_itens 

		FETCH NEXT FROM x_itens
			INTO @mercadoria, @preco, @qtde,@embalagem,@ean

		WHILE @@FETCH_STATUS = 0
		BEGIN
		--iTENS

			insert into pedido_itens (filial, pedido, plu,ean, qtde, embalagem, unitario, total,tipo)
				values(@filial, @pedido, @mercadoria,@ean, @qtde, @embalagem, @preco, ((@qtde*@embalagem) * @preco),2)


			FETCH NEXT FROM x_itens
				INTO @mercadoria, @preco, @qtde,@embalagem,@ean

		END
		update a set total = (select sum(pedido_itens.total) from pedido_itens where pedido_itens.pedido = a.pedido and pedido_itens.filial = a.filial)
		from pedido a
		where filial = @filial and pedido = @pedido
		
		update pedido_Pagamento set  valor = (select total from pedido where Filial = @filial and  pedido=@pedido and Tipo=2)
		where Filial = @filial and  pedido=@pedido and tipo=2

		close x_itens
		deallocate x_itens


		FETCH NEXT FROM x_fornecedores
		INTO @vencedor

	END

	close x_fornecedores
	deallocate x_fornecedores


	update a set total = (select sum(pedido_itens.total) from pedido_itens where pedido_itens.pedido = a.pedido and pedido_itens.filial = a.filial)
	from pedido a
	where filial = @filial and cotacao = @cotacao
	
	update pedido_Pagamento set  valor = (select total from pedido where Filial = @filial and  pedido=@pedido and Tipo=2)
		where Filial = @filial and  pedido=@pedido and tipo=2


	return (1)

go 


IF OBJECT_ID('[sp_Rel_Resumo_Vendas_hora_media]', 'P') IS NOT NULL
begin 
      drop procedure [sp_Rel_Resumo_Vendas_hora_media]
end 

go
CREATE  Procedure [dbo].[sp_Rel_Resumo_Vendas_hora_media]
            @Filial           As Varchar(20),
            @DataDe           As Varchar(8),
			@DataAte          As Varchar(8),
			@ini_periodo	  as varchar(5),
			@fim_periodo	  as varchar(5),
            @plu			  as Varchar (20),
            @descricao		  as varchar(50),
            @grupo			  as Varchar(50),
            @subGrupo		  as varchar(50),
            @departamento	  as varchar(50),
            @relatorio		  as varchar(40)

 

AS
-- EXEC sp_Rel_Resumo_Vendas_hora_media 'MATRIZ','20180601','20180630','00:00','23:59','','','','','','TODOS'
-- @relatorio = TODOS ,CUPOM , NOTA SAIDA, PEDIDO SIMPLES 	
	IF OBJECT_ID(N'tempdb..#tmpVendas', N'U') IS NOT NULL   
	begin
		DROP TABLE #tmpVendas;  
	end
CREATE TABLE #tmpVendas
(
	hora varchar(5),
	Venda Decimal(18,2),
	Clientes int,
	Venda_MD  Decimal (12,2),
	NFP INT,
	Vlr_NFP Decimal(18,2),
	Perc_NFP Decimal(8,2)
)

	declare @dias int 

	Select @dias = DATEDIFF(day,@DataDe,@DataAte)+1
	print(convert(varchar,@dias))

if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN
SELECT      saida_estoque.Filial,
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
			  and   hora_venda between @ini_periodo and @fim_periodo
			  AND   Data_Movimento  between @DataDe and @DataAte
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

PRINT('#TEMPVENDAS')
insert into #tmpVendas
 SELECT
	  substring(Hora_venda,1,2),
      Venda =       (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ),
      Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ) ,
      Venda_MD =    CONVERT(DECIMAL(12,2), 
						CASE WHEN (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2)  ) > 0 THEN

                  ((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ) /  (SELECT COUNT(*) FROM #Lixo WHERE  substring(#Lixo.Hora_venda,1,2) =substring(Saida_Estoque.Hora_venda,1,2) ))

                  ELSE 0 END),

      NFP =         (SELECT COUNT(*) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_estoque.Hora_venda,1,2)  AND #lixo.CPF_CNPJ <> ''),

      Vlr_NFP =     ISNULL((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_estoque.Hora_venda,1,2)  AND #lixo.CPF_CNPJ <> ''),0),

     

      Perc_NFP =    CASE WHEN (SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_estoque.Hora_venda,1,2) AND #lixo.CPF_CNPJ <> '') > 0 THEN

                  CONVERT(DECIMAL(8,2), ((SELECT Convert(Decimal(18,2),SUM(VLR)) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(Saida_estoque.Hora_venda,1,2)  AND #lixo.CPF_CNPJ <> '') /

                        (SELECT Convert(Decimal(18,2),SUM(#Lixo.Vlr)) FROM #Lixo WHERE Saida_estoque.Data_movimento= #lixo.Data_movimento and  substring(#Lixo.Hora_venda,1,2) =substring(saida_estoque.Hora_venda,1,2)  )) * 100)

                  ELSE 0 END
	 
      FROM

            Saida_Estoque  with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  


      WHERE

            Saida_Estoque.Filial = @Filial

    

		  AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
		  AND (Data_Movimento between @DataDe and @DataAte)
		  AND (hora_venda between @ini_periodo and @fim_periodo)
		  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
		  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
		  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
	   
      GROUP BY    Saida_Estoque.Data_movimento , substring(Hora_venda,1,2)
 END

if(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
BEGIN
insert into #tmpVendas
select 
		substring(N.emissao_hora,1,2)  as horas,
		SUM(ni.TOTAL-(isnull(ni.Total,0)*isnull(ni.desconto,0)/100)) AS Venda,
      
		(
		  	Select COUNT(*) 
		from nf 
			inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 

		where NF.FILIAL=@filial 
				and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
				and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') 
				AND  (NF.Emissao = n.Emissao )
				and  (substring(N.emissao_hora,1,2) = substring(NF.emissao_hora,1,2))
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
							 and  (substring(N.emissao_hora,1,2) = substring(NF.emissao_hora,1,2))
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
								and  nf.status='AUTORIZADO'
								and  (substring(N.emissao_hora,1,2) = substring(NF.emissao_hora,1,2))
								AND   NF.TIPO_NF = 1	
								and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
								and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
								
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
AND  (N.Emissao between @DataDe and @DataAte )
	 and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
	 and n.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
	 and n.status='AUTORIZADO'						
	 and  (n.emissao_hora between @ini_periodo and @fim_periodo)
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
	 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY substring(N.emissao_hora,1,2),n.emissao
END


--if(@relatorio='TODOS' OR @relatorio='PEDIDO SIMPLES')
--BEGIN
--insert into #tmpVendas
--select 
--	CONVERT(VARCHAR,p.Data_cadastro,103) AS DATA ,
--	  Dia_Semana = CASE
--            WHEN DATEPART(dw, p.Data_cadastro) = 1 THEN 'DOMINGO'
--            WHEN DATEPART(dw, p.Data_cadastro) = 2 THEN 'SEGUNDA'
--            WHEN DATEPART(dw, p.Data_cadastro) = 3 THEN 'TERÇA'
--            WHEN DATEPART(dw, p.Data_cadastro) = 4 THEN 'QUARTA'
--            WHEN DATEPART(dw, p.Data_cadastro) = 5 THEN 'QUINTA'
--            WHEN DATEPART(dw, p.Data_cadastro) = 6 THEN 'SEXTA'
--            WHEN DATEPART(dw, p.Data_cadastro) = 7 THEN 'SABADO'

--      END,
--      SUM(pit.TOTAL) AS Venda,
      
--      (
--		  	Select COUNT(*) 
--		from pedido  with (index(ix_pedido_fluxo_caixa))
--		where pedido.pedido_simples = 1  
--				AND  (pedido.Data_cadastro= p.Data_cadastro )
--				and isnull(pedido.Status,0)<>3
--				AND   pedido.Tipo = 1 
--				and pedido.FILIAL=@filial 
--				AND (pedido.pedido IN (SELECT DISTINCT pedido 
--											FROM pedido_Itens as	nii										
--											inner join mercadoria  as mi on mi.PLU = nii.PLU
--											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO as cdi  ON cdi.codigo_departamento = mi.Codigo_departamento  
--											WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
--													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
--													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
--													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
--										)
--							)
--      ) 
--      as Clientes ,
--      (
--      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM pedido WHERE pedido.Data_cadastro = p.Data_cadastro) > 0 THEN

--                  (
--                  Select isnull(SUM(pii.total),0) 
--					from pedido with (index(ix_pedido_fluxo_caixa))
--					inner join pedido_itens as pii on pedido.pedido=pii.Pedido and pedido.Filial=pii.Filial and pedido.Tipo = pii.Tipo
--					inner join mercadoria as mi on mi.PLU = pii.PLU
--					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
--					where pedido.FILIAL=@filial 
--							 AND (pedido.Data_cadastro= p.Data_cadastro)
--							 AND pedido.TIPO = 1
--							 and pedido.pedido_simples = 1
--							 AND (LEN(@PLU)=0 OR pii.PLU = @plu)
--							 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
--							 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
--							 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
--							) 
--							 /   
--					(	
--					Select COUNT(*) 
--					from pedido with (index(ix_pedido_fluxo_caixa))
--					where pedido.FILIAL=@filial 
--								AND  (pedido.Data_cadastro= p.Data_cadastro )
--								AND   pedido.TIPO = 1	
--								and pedido.pedido_simples = 1
								
--								AND (
--						pedido.pedido IN (SELECT DISTINCT pedido 
--											FROM pedido_Itens as pii  inner join mercadoria as mi on mi.PLU = pii.PLU
--											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
--											 WHERE  (LEN(@PLU)=0 OR pii.PLU = @plu)
--													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
--													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
--													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
											
--													  )
--									)
--					)

--                  ELSE 0 END)
                  
                  
--      ) as [Venda_MD],
--      0 as [NFP],
--      0 AS [Vlr NFP],
--      0 as [Perc NP]

--from  Pedido as p with (index(ix_pedido_fluxo_caixa))
--inner join Pedido_itens  as pit with (index(ix_sp_rel_resumo_vendas)) on pit.Pedido=p.pedido and pit.Filial=p.Filial and p.Tipo = pit.Tipo
--inner join mercadoria as m on m.PLU = pit.PLU
--INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
--WHERE p.pedido_simples = 1   
--	AND  (p.Data_cadastro BETWEEN @DataDe AND @DataAte)
--	 and isnull(p.Status,0)<>3
--	 AND   p.TIPO = 1	
--	 and p.FILIAL=@filial 
--	 AND (LEN(@PLU)=0 OR pit.PLU = @plu)
--	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
--	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
--	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
--GROUP BY p.Data_cadastro
--END

print 'media'
 Select Hora,
		case when Sum(Venda)>0 then  (Sum(Venda)/@dias) else 0 end as Venda
		,case when SUM(Clientes) >0 then (SUM(Clientes)/@dias) else 0 end as Clientes 
		,case when (Sum(Venda)/@dias)>0  and (SUM(Clientes)/@dias)>0 then (Sum(Venda)/@dias)/(SUM(Clientes)/@dias) else 0 end as [Venda_MD]
		,case when SUM(nfp)>0 then (SUM(nfp)/@dias) else 0 end as NFP
		,case when SUM(VLR_NFP)>0 then  (SUM(VLR_NFP)/@dias) else 0 end as VLR_NFP
		, '|-NI-|'+convert(varchar,case when SUM(VLR_NFP)>0 then  ((SUM(VLR_NFP)/@dias)/(Sum(Venda)/@dias))*100 else 0 end) AS PERC_NFP
	from 
	#tmpVendas
GROUP BY hora
ORDER BY hora ;



--EXEC @SQL;




go 




IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Transportadora') 
            AND  UPPER(COLUMN_NAME) = UPPER('padrao'))
begin
	alter table Transportadora alter column padrao tinyint
end
else
begin
	alter table Transportadora add  padrao tinyint
end 
go 




IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Tipo_pagamento') 
            AND  UPPER(COLUMN_NAME) = UPPER('padrao'))
begin
	alter table Tipo_pagamento alter column padrao tinyint
end
else
begin
	alter table Tipo_pagamento add  padrao tinyint
end 
go 











go
insert into Versoes_Atualizadas select 'Versão:1.230.735', getdate();
GO

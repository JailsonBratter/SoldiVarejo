
-- PROCEDURE ======================================================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_br_gera_pedidos_GV]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_br_gera_pedidos_GV
end
go 

CREATE procedure [dbo].[sp_br_gera_pedidos_GV]
	@cotacao int , @filial varchar(20)
as
	declare  @vencedor varchar(20),
		@pedido varchar(20),
		@mercadoria varchar(17),
		@preco decimal(12,2),
		@qtde decimal(9,3),
		@embalagem decimal(9,3),
		@prazoPgo  dateTime ,
		@prazoEntrega dateTime

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
		select cotacao_DIGITACAO.MERCADORIA, cotacao_DIGITACAO.PRECO, cotacao_item.quantidade,cotacao_item.embalagem 
		from cotacao_DIGITACAO 
		inner join cotacao_item 
			on (cotacao_item.cotacao = cotacao_DIGITACAO.cotacao and cotacao_item.filial = cotacao_DIGITACAO.filial and 
					cotacao_item.mercadoria = cotacao_digitacao.mercadoria)
		where cotacao_DIGITACAO.cotacao = @cotacao and cotacao_DIGITACAO.filial = @filial AND FORNECEDOR = @vencedor and cotacao_DIGITACAO.preco > 0 and cotacao_DIGITACAO.qtde > 0

	open x_itensA 
		
	FETCH NEXT FROM x_itensA
		INTO @mercadoria, @preco, @qtde,@embalagem

	

	WHILE @@FETCH_STATUS = 0
	BEGIN
	--iTENS

		insert into pedido_itens (filial, pedido, plu, qtde, embalagem, unitario, total,tipo)
			values(@filial, @pedido, @mercadoria, @qtde, @embalagem, @preco, @qtde * @preco,2)
		
		
		FETCH NEXT FROM x_itensA
			INTO @mercadoria, @preco, @qtde, @embalagem

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

GO




-- PROCEDURE ======================================================================================================================

go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_br_gera_pedidos]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_br_gera_pedidos
end
go 

create  procedure [dbo].[sp_br_gera_pedidos]
	@cotacao int , @filial varchar(20)
as

	declare  @vencedor varchar(20),
		@pedido varchar(20),
		@mercadoria varchar(17),
		@preco decimal(12,2),
		@qtde decimal(9,3),
		@embalagem decimal(9,3),
		@prazoPgo  dateTime ,
		@prazoEntrega dateTime

		


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
				INTO @mercadoria, @preco, @qtde,@embalagem

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


-- PROCEDURE ======================================================================================================================
go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_br_finaliza_cotacao]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_br_finaliza_cotacao
end
go 
CREATE PROCEDURE [dbo].[sp_br_finaliza_cotacao]
@cotacao int,
@filial varchar(20),
@GrandeVencedor int
as

update a set vencedor = (
	select top 1 B.fornecedor 
	from cotacao_digitacao b 
	where b.cotacao = a.cotacao and b.filial = a.filial and 
		a.mercadoria = b.mercadoria and b.preco > 0 and b.qtde > 0
	order by preco, prazo_pgto desc, prazo_entrega) 
from cotacao_item a
where cotacao = @cotacao and filial = @filial

if @GrandeVencedor = 1
begin
	exec sp_br_gera_pedidos_GV @cotacao, @filial
end
else
begin
	exec sp_br_gera_pedidos @cotacao, @filial
end

go

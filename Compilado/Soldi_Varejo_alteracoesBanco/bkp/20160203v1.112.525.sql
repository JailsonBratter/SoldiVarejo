
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_br_gera_pedidos]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_br_gera_pedidos]
end
GO
--PROCEDURES =======================================================================================
CREATE  procedure [dbo].[sp_br_gera_pedidos]
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
				values(@filial, @pedido, @mercadoria, @qtde, @embalagem, @preco, ((@qtde*@embalagem) * @preco),2)


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


go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_br_gera_pedidos]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_br_gera_pedidos]
end
GO
--PROCEDURES =======================================================================================
CREATE  procedure [dbo].[sp_br_gera_pedidos]
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
				values(@filial, @pedido, @mercadoria, @qtde, @embalagem, @preco, ((@qtde*@embalagem) * @preco),2)


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

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Outras_Movimentacoes]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Outras_Movimentacoes]
end
GO
--PROCEDURES =======================================================================================
CREATE    PROCEDURE [dbo].[sp_Rel_Outras_Movimentacoes]
	@Filial				As Varchar(20),
	@DataDe				As Varchar(8),
	@DataAte			As Varchar(8),
	@plu				AS Varchar(20),
	@ean				as varchar(20),
	@ref				as Varchar(20),
	@descricao			as Varchar(40),
	@Movimentacao	As Varchar(30) = '',
	@tipo			 as varchar(20)
AS
	Declare @String	As nVarchar(1024)
if @tipo = 'PRODUTO'
BEGIN		
		SET @String = ' SELECT'
		SET @String = @String + ' i.Codigo_inventario As Codigo,'
		SET @String = @String + ' CONVERT(VARCHAR, i.Data, 103) AS Data,'
		SET @String = @String + ' i.tipoMovimentacao,'
		SET @String = @String + ' i.Usuario,'
		SET @String = @String +   CHAR(39) +'PLU' +CHAR(39) +'+ ii.plu as PLU,'
		SET @String = @String + ' m.descricao,'
		SET @String = @String + ' ii.Contada AS Qtde,'
		SET @String = @String + ' ii.custo as Vlr_Unit,'
		SET @String = @String + ' Total_Item = CONVERT(DECIMAL(12,2), (ii.custo * ii.Contada ))'
		SET @String = @String + ' FROM inventario i inner join inventario_itens ii on i.Codigo_inventario = ii.Codigo_inventario '
		SET @String = @String + ' inner join mercadoria m on ii.PLU = m.PLU '
		SET @String = @String + ' left join ean  on m.PLU = ean.PLU '
		SET @String = @String + ' WHERE i.status = ' + CHAR(39) + 'ENCERRADO' + CHAR(39) + ' AND i.Data BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		BEGIN
			IF @Movimentacao<> 'TODOS' 
				SET @String = @String + ' AND i.tipoMovimentacao = ' + CHAR(39) + @Movimentacao + CHAR(39)
		END
		BEGIN
			IF LEN(ISNULL(@plu ,'')) > 0 
				SET @String = @String + ' AND ii.plu = ' + CHAR(39) + @plu + CHAR(39)
		END
		BEGIN
			IF LEN(ISNULL(@ean ,'')) > 0 
				SET @String = @String + ' AND ean = ' + CHAR(39) + @ean + CHAR(39)
		END
		BEGIN
			IF LEN(ISNULL(@ref ,'')) > 0 
				SET @String = @String + ' AND m.Ref_fornecedor = ' + CHAR(39) + @ref + CHAR(39)
		END
		BEGIN
			IF LEN(ISNULL(@descricao ,'')) > 0 
				SET @String = @String + ' AND m.descricao like ' + CHAR(39) + '%'+@descricao +'%'+ CHAR(39)
		END
		SET @String = @String + ' ORDER BY 1'

	-- PRINT @STRING
	 EXECUTE(@String)

END
ELSE
	BEGIN
			
		SET @String = ' SELECT'
		SET @String = @String + ' i.Codigo_inventario As Codigo,'
		SET @String = @String + ' CONVERT(VARCHAR, i.Data, 103) AS Data,'
		SET @String = @String + ' i.tipoMovimentacao,'
		SET @String = @String + ' i.Usuario,'
		SET @String = @String + ' Total_Item = SUM(CONVERT(DECIMAL(12,2), (ii.custo * ii.Contada )))'
		SET @String = @String + ' FROM inventario i inner join inventario_itens ii on i.Codigo_inventario = ii.Codigo_inventario '
		SET @String = @String + ' inner join mercadoria m on ii.PLU = m.PLU '
		SET @String = @String + ' left join ean  on m.PLU = ean.PLU '
		SET @String = @String + ' WHERE i.status = ' + CHAR(39) + 'ENCERRADO' + CHAR(39) + ' AND i.Data BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		BEGIN
			IF @Movimentacao<> 'TODOS' 
				SET @String = @String + ' AND i.tipoMovimentacao = ' + CHAR(39) + @Movimentacao + CHAR(39)
		END
		BEGIN
			IF LEN(ISNULL(@plu ,'')) > 0 
				SET @String = @String + ' AND ii.plu = ' + CHAR(39) + @plu + CHAR(39)
		END
		BEGIN
			IF LEN(ISNULL(@ean ,'')) > 0 
				SET @String = @String + ' AND ean = ' + CHAR(39) + @ean + CHAR(39)
		END
		BEGIN
			IF LEN(ISNULL(@ref ,'')) > 0 
				SET @String = @String + ' AND m.Ref_fornecedor = ' + CHAR(39) + @ref + CHAR(39)
		END
		BEGIN
			IF LEN(ISNULL(@descricao ,'')) > 0 
				SET @String = @String + ' AND m.descricao like ' + CHAR(39) + '%'+@descricao +'%'+ CHAR(39)
		END
		SET @String = @String + ' GROUP BY i.Codigo_inventario,i.Data,i.tipoMovimentacao,i.Usuario ORDER BY 1'

	-- PRINT @STRING
	 EXECUTE(@String)	
	END
go 


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_ProdutosAlterados]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_ProdutosAlterados]
end
GO
--PROCEDURES =======================================================================================
CREATE     Procedure [dbo].[sp_Rel_ProdutosAlterados]

                @Filial                 As Varchar(20),
                @plu					as Varchar(20),
                @ean					as Varchar(20),
                @ncm					as Varchar(20),
             	@Ref_Fornecedor         As Varchar(20),
                @Descricao              As Varchar(60) ,
                @DataDe                 As Varchar(8),
                @DataAte                As Varchar(8),
                @grupo					As varchar(20)
				,@subGrupo				As varchar(20)
				,@departamento			As varchar(20)
				,@familia				As varchar(40),
				@custoMargem			as varchar(40)


AS

Declare @String               As Varchar(max)
Declare @Where               As Varchar(max)

BEGIN
                --** Cria A String Com Os filtros selecionados

	
                               -- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
		SET @String = @String + 'mercadoria.PLU,  isnull(e.EAN , '+ CHAR(39) + CHAR(39) +') as EAN, NCM = ISNULL(MERCADORIA.CF, '+ CHAR(39) + CHAR(39) +'),'
		SET @String = @String + 'mercadoria.ref_fornecedor AS REF_FORN, mercadoria.DESCRICAO , '
		if(@custoMargem='ALTERADOS' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin

		SET @String = @String + 'CONVERT(VARCHAR, mercadoria.Data_Alteracao, 103) As DATA_ALTERACAO,'
		end		
		if(@custoMargem='CUSTO-MARGEM-VENDA' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin
			set @String = @String + ' mercadoria_loja.PRECO_CUSTO,MARGEM = convert(decimal(10,2), Case when mercadoria_loja.Preco_Custo > 0 and mercadoria_loja.Preco > 0 then'
			SET @String = @String + '((mercadoria_loja.Preco - mercadoria_loja.Preco_Custo ) / mercadoria_loja.Preco_Custo ) * 100 else 0 end),'
		end
	
	
		SET @String = @String + 'mercadoria_loja.preco as VENDA '
		SET @String = @String + ' from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu'
        SET @String = @String + ' left join EAN e on mercadoria.plu=e.PLU
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on (mercadoria.codigo_departamento= c.codigo_Departamento and mercadoria.filial=c.filial)'
                               --PRINT @STRING + @Where + ' GROUP BY Mercadoria.PLU, Mercadoria.Descricao ORDER BY vlr DESC'
		Set @Where = ' WHERE  (Mercadoria_Loja.Filial = ' + CHAR(39) + @Filial + CHAR(39)+')'
        
         IF (LEN(@DESCRICAO) > 0)
		BEGIN
            SET @Where = @Where + ' AND ( mercadoria.DESCRICAO LIKE '+ CHAR(39) +'%'+ @DESCRICAO + '%'+CHAR(39)+')'

        END      
        
        
		if(@custoMargem='ALTERADOS' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin
		
			Set @Where = @Where +' And (Mercadoria.Data_Alteracao BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)+')'
		end
	       
        
      if LEN(@plu)>0
      begin
		    SET @Where = @Where + ' AND (mercadoria.plu = '+ CHAR(39) + @plu + CHAR(39)+')'

      end	
      if LEN(@ean)>0
      begin
		    SET @Where = @Where + ' AND (e.ean = '+ CHAR(39) + @ean + CHAR(39)+')'

      end	
      if LEN(@ncm)>0
      begin
		    SET @Where = @Where + ' AND (mercadoria.CF = '+ CHAR(39) + @ncm + CHAR(39)+')'

      end	
      
      IF LEN(ISNULL(@Ref_Fornecedor,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (Mercadoria.Ref_Fornecedor LIKE '+ CHAR(39) + @Ref_Fornecedor + CHAR(39)+')'

        END     
	
	  IF LEN(ISNULL(@grupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (c.descricao_grupo = '+ CHAR(39) + @grupo + CHAR(39)+')'

        END     
	IF LEN(ISNULL(@subGrupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (c.descricao_subgrupo = '+ CHAR(39) + @subGrupo + CHAR(39)+')'

        END     
        IF LEN(ISNULL(@departamento,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND c.descricao_departamento = '+ CHAR(39) + @departamento + CHAR(39)

        END    
	   IF LEN(ISNULL(@familia,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND mercadoria.descricao_familia = '+ CHAR(39) + @familia + CHAR(39)
		end
     	
	 

		--print(@Where)
	   EXECUTE(@String + @Where)

END

 


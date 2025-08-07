if not exists(Select 1 from web_telas where item='23-Media_de_Vendas_por_Hora')
begin
	insert into web_telas values
	('R002','23-Media_de_Vendas_por_Hora',230),
	('R002','24-Apuracao_Por_Atendente',240),
	('R002','25-Vendas_Por_Cliente_Produtos',250),
	('R002','26-Vendas_Por_Cliente_Tabela_Desconto',260),
	('R002','27-Produtos_Cancelados',270)
end
GO 
if exists(
	select 1 
	from sys.sysobjects 
	where xtype ='TR' and 
		  name ='utg_atualiza_saldo_atual_tipo' and  
		  object_name(parent_obj) ='mercadoria'
)
begin
	DROP TRIGGER [dbo].[utg_atualiza_saldo_atual_tipo]
end

go

CREATE TRIGGER [dbo].[utg_atualiza_saldo_atual_tipo] ON [dbo].[Mercadoria_Loja]
FOR UPDATE
AS
declare
  @filial varchar(20),
  @PLUOriginal Varchar(17),
  @plu varchar(17),
  @qtdeSubtrai decimal(9,3),
  @Fator_Conv Decimal(9,3),
  @QtdeSoma Decimal(9,3),
  @Qtde Decimal(9,3),
  @PLUVinculado VARCHAR(17)
BEGIN
   SELECT
		@PLUVinculado = ISNULL(Mercadoria.PLU_Vinculado, ''),
		@Fator_Conv = ISNULL(Mercadoria.fator_Estoque_Vinculado, 0),
		@PLUOriginal = DELETED.PLU
	FROM
		MERCADORIA INNER JOIN DELETED ON MERCADORIA.PLU = DELETED.PLU
END

  if @PLUVinculado <>''
    BEGIN
        declare x_updatem cursor for select DELETED.filial, @PLUVinculado, DELETED.Saldo_Atual * -1, INSERTED.saldo_atual, @Fator_Conv
        from DELETED
        inner join INSERTED on (DELETED.filial = INSERTED.filial and DELETED.PLU = INSERTED.PLU)
        inner join Mercadoria ON Mercadoria.PLU = DELETED.PLU
        inner join Tipo ON Tipo.Tipo = Mercadoria.Tipo  AND ISNULL(Tipo.PLUAssociado,0) = 1
    END
  ELSE
	BEGIN
       declare x_updatem cursor for select DELETED.filial, Item.Plu_item, DELETED.Saldo_Atual * -1, INSERTED.saldo_atual, Item.fator_conversao
       from DELETED
            inner join INSERTED on (DELETED.filial = INSERTED.filial and DELETED.PLU = INSERTED.PLU)
            inner join Mercadoria ON Mercadoria.PLU = DELETED.PLU
			inner join Tipo ON Tipo.Tipo = Mercadoria.Tipo  AND ISNULL(Tipo.Movimenta_Estoque_Item,0) = 1
			inner join Item ON Item.Plu = DELETED.PLU
    END
Open x_updatem
FETCH NEXT FROM x_updatem
INTO @filial, @plu, @qtdeSubtrai, @QtdeSoma, @Fator_Conv
WHILE @@FETCH_STATUS = 0
BEGIN
   BEGIN
		if update(saldo_Atual)
			SET @Qtde = @qtdeSubtrai + @QtdeSoma
		begin
			update mercadoria set saldo_atual = isnull(saldo_atual,0) + (@qtde * @Fator_Conv) where plu =  @plu
			update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) + (@qtde * @Fator_Conv) where plu =  @plu and filial = @filial
			update mercadoria set saldo_atual = isnull(saldo_atual,0) + (@qtde) where plu =  @PLUOriginal
        end
	end

	FETCH NEXT FROM x_updatem
	INTO @filial, @plu, @qtdeSubtrai, @QtdeSoma, @Fator_Conv
END
close x_updatem
deallocate x_updatem

go 


ALTER TRIGGER [dbo].[utg_atualiza_saldo_atual] ON [dbo].[Saida_estoque] 
FOR UPDATE
AS

declare @plu varchar(17),
 @filial varchar(20),
 @qtde decimal(9,3),
 @odt_canc datetime,
 @ndt_canc datetime,
 @tipo_p varchar(20)


declare x_update cursor for select DELETED.plu, DELETED.filial, DELETED.qtde, DELETED.data_cancelamento odt_canc, INSERTED.data_cancelamento ndt_canc 
   from DELETED
   inner join INSERTED
      on (DELETED.filial = INSERTED.filial and DELETED.data_movimento = INSERTED.data_movimento and DELETED.documento = INSERTED.documento and DELETED.origem = INSERTED.origem and DELETED.plu = INSERTED.plu and DELETED.caixa_saida = INSERTED.caixa_saida and DELETED.codigo_funcionario = INSERTED.codigo_funcionario and DELETED.ean = INSERTED.ean and DELETED.pedido = INSERTED.pedido)

open x_update

FETCH NEXT FROM x_update
 INTO @plu, @filial, @qtde, @odt_canc , @ndt_canc

WHILE @@FETCH_STATUS = 0
BEGIN
 select @tipo_p = tipo from mercadoria where plu = @plu and filial = @filial
 if @tipo_p  != 'PRODUCAO'
 BEGIN
  if update(data_cancelamento)
  begin
   if @odt_canc is null and @ndt_canc is not null 
   begin
    update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) + @qtde where plu =  @plu and filial = @filial
   end 
  end
 end
 FETCH NEXT FROM x_update
  INTO @plu, @filial, @qtde, @odt_canc , @ndt_canc 
END

close x_update

deallocate x_update


go


ALTER  TRIGGER [dbo].[dtg_atualiza_saldo_atual] ON [dbo].[Saida_estoque] 
FOR DELETE 
AS

declare @plu varchar(17),
 @filial varchar(20),
 @qtde decimal(9,3),
 @dt_canc datetime,
 @tipo_p varchar(20)

--select @plu = plu, @filial = filial, @qtde = qtde, @dt_canc = data_cancelamento from DELETED

declare x_deleted cursor for select plu, filial, qtde, data_cancelamento from DELETED

open x_deleted

FETCH NEXT FROM x_deleted
 INTO @plu, @filial, @qtde, @dt_canc

 WHILE @@FETCH_STATUS = 0
 BEGIN
  select @tipo_p = tipo from mercadoria where plu = @plu and filial = @filial
  if @tipo_p  != 'PRODUCAO'
  BEGIN
   if @dt_canc is null
   begin 
    --update mercadoria set saldo_atual = isnull(saldo_atual,0) + @qtde where plu =  @plu --and filial = @filial 
	update mercadoria_loja set saldo_atual = isnull(saldo_atual, 0) + @qtde where filial = @filial and plu = @plu
   end
   FETCH NEXT FROM x_deleted
    INTO @plu, @filial, @qtde, @dt_canc
  end
 END

close x_deleted
deallocate x_deleted

go

ALTER TRIGGER [dbo].[itg_atualiza_saldo_atual] ON [dbo].[Saida_estoque] 
FOR INSERT
AS

declare @plu varchar(17),
 @filial varchar(20),
 @qtde decimal(9,3),
 @plu_item varchar(17),
 @fator_conversao decimal(9,3),
 @tipo_p varchar(20)

select @plu = plu, @filial = filial, @qtde = qtde from INSERTED where data_cancelamento is null

select @tipo_p = tipo from mercadoria where plu = @plu and filial = @filial
if @tipo_p  != 'PRODUCAO'
BEGIN
	--update mercadoria set saldo_atual = isnull(saldo_atual,0) - @qtde where plu =  @plu  
	update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) - @qtde where plu =  @plu and filial = @filial 
end


insert into Versoes_Atualizadas select 'Versão:1.291.861', getdate();
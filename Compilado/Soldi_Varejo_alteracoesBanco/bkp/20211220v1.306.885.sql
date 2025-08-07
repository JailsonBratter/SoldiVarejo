
if (exists(select 1 Tabela from sys.sysobjects 
where xtype ='TR' and name ='utg_atualiza_saldo_atual_tipo'  and object_name(parent_obj) = 'Mercadoria_loja'))
begin
	DROP TRIGGER [dbo].[utg_atualiza_saldo_atual_tipo]
end 

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
		update mercadoria_loja set saldo_atual = isnull(saldo_atual, 0) + @qtde where filial = @filial and plu = @plu
		update mercadoria 
				set mercadoria.Saldo_atual = (select sum(mercadoria_loja.saldo_atual) 
														from mercadoria_loja 
														where Mercadoria_Loja.plu = @plu)
		where mercadoria.plu = @plu
   end
   FETCH NEXT FROM x_deleted
    INTO @plu, @filial, @qtde, @dt_canc
  end
 END

close x_deleted
deallocate x_deleted
GO

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
	update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) - @qtde where plu =  @plu and filial = @filial 
		update mercadoria 
			set mercadoria.Saldo_atual = (select sum(mercadoria_loja.saldo_atual) 
													from mercadoria_loja 
													where Mercadoria_Loja.plu = @plu)
		where mercadoria.plu = @plu

end

GO

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
		update mercadoria 
				set mercadoria.Saldo_atual = (select sum(mercadoria_loja.saldo_atual) 
														from mercadoria_loja 
														where Mercadoria_Loja.plu = @plu)
		where mercadoria.plu = @plu

   end 
  end
 end
 FETCH NEXT FROM x_update
  INTO @plu, @filial, @qtde, @odt_canc , @ndt_canc 
END

close x_update

deallocate x_update

go 



 insert into Versoes_Atualizadas select 'Versão:1.306.885', getdate();
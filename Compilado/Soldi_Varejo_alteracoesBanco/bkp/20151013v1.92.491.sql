alter table conta_a_pagar alter column acrescimo decimal(18,2)
go 
alter table conta_a_receber alter column acrescimo decimal(18,2)

GO

INSERT INTO [BratterWeb].[dbo].[SEQUENCIAIS]
           ([TABELA_COLUNA]
           ,[DESCRICAO]
           ,[SEQUENCIA]
           ,[TAMANHO]
           ,[OBS1]
           ,[OBS2]
           ,[OBS3]
           ,[OBS4]
           ,[OBS5]
           ,[OBS6]
           ,[OBS7]
           ,[OBS8]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('AGENDA.PEDIDO'
           ,'CODIGO PEDIDO PETSHOP'
           ,'00'
           ,3
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,GETDATE()
           ,0);
GO


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
 update mercadoria set saldo_atual = isnull(saldo_atual,0) - @qtde where plu =  @plu  
update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) - @qtde where plu =  @plu and filial = @filial 
end













IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('entrega'))
begin
	alter table pedido alter column entrega tinyint
end
else
begin
	alter table pedido add entrega tinyint
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('hora_cadastro'))
begin
	alter table pedido alter column hora_cadastro varchar(5)
end
else
begin
	alter table pedido add hora_cadastro varchar(5)
end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido_itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('obs'))
begin
	alter table pedido_itens alter column obs nvarchar(255)
end
else
begin
	alter table pedido_itens add obs nvarchar(255)
end 
go 





IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido_itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('num_item'))
begin
	alter table pedido_itens alter column num_item int
end
else
begin
	alter table pedido_itens add num_item int
end 
go 


if not exists(select 1 from PARAMETROS where PARAMETRO='PEDIDO_IMP_40_DELIVERY')
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
           ('PEDIDO_IMP_40_DELIVERY'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE' -- TRUE
           ,'IMPRIME PEDIDO DELIVERY 40 POSIÇOES'
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


if not exists(select 1 from PARAMETROS where PARAMETRO='IMP_REMOTA_PEDIDO')
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
           ('IMP_REMOTA_PEDIDO'
           ,GETDATE()
           ,'\\127.0.0.1\ImpressoraPedidoTeste'
           ,GETDATE()
           ,'' --'\\127.0.0.1\ImpressoraPedidoTeste'
           ,'CAMINHO IMPRESSO REMOTA IMPRESSAO PEDIDO'
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
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido_itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('produzir'))
begin
	alter table pedido_itens alter column produzir tinyint
end
else
begin
	alter table pedido_itens add produzir tinyint
end
 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido_itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('data_hora_produzir'))
begin
	alter table pedido_itens alter column data_hora_produzir DateTime
end
else
begin
	alter table pedido_itens add data_hora_produzir DateTime
end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('cliente') 
            AND  UPPER(COLUMN_NAME) = UPPER('conta_assinada'))
begin
	alter table cliente alter column conta_assinada tinyint
end
else
begin
	alter table cliente add conta_assinada tinyint
end 

go

insert into Versoes_Atualizadas select 'Versão:1.214.706', getdate();
GO


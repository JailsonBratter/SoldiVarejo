
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('margem_terceiro_preco'))
begin
	alter table mercadoria alter column margem_terceiro_preco numeric(12,4);
	alter table mercadoria alter column terceiro_preco numeric(12,2);
end
else
begin
	alter table mercadoria add margem_terceiro_preco numeric(12,4);
	alter table mercadoria add terceiro_preco numeric(12,2);
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('cliente') 
            AND  UPPER(COLUMN_NAME) = UPPER('ativa_terceiro_preco'))
begin
	alter table cliente alter column ativa_terceiro_preco tinyint
end
else
begin
	alter table cliente add ativa_terceiro_preco tinyint
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Produtos_Carga') 
            AND  UPPER(COLUMN_NAME) = UPPER('terceiro_preco'))
begin
	alter table Produtos_Carga alter column terceiro_preco float
end
else
begin
	alter table Produtos_Carga add terceiro_preco float
end 
go 


insert into Versoes_Atualizadas select 'Versão:1.311.893', getdate();
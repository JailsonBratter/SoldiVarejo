
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('descricao'))
begin
	alter table nf_item alter column descricao varchar(200)
end
else
begin
	alter table nf_item add descricao varchar(200)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item_log') 
            AND  UPPER(COLUMN_NAME) = UPPER('descricao'))
begin
	alter table nf_item_log alter column descricao varchar(200)
end
else
begin
	alter table nf_item_log add descricao varchar(200)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('fornecedor_mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('descricao'))
begin
	alter table fornecedor_mercadoria alter column descricao varchar(200)
end
else
begin
	alter table fornecedor_mercadoria add descricao varchar(200)
end 
go 

insert into Versoes_Atualizadas select 'Versão:1.310.892', getdate();
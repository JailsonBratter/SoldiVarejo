IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('natureza_operacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('utilizaCFOP'))
begin
	alter table natureza_operacao alter column utilizaCFOP tinyint
end
else
begin
	alter table natureza_operacao add utilizaCFOP tinyint
end 
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('ordem_compra'))
begin
	alter table nf alter column ordem_compra varchar(40)
end
else
begin
	alter table nf add ordem_compra varchar(40)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('ordem_compra'))
begin
	alter table nf_item alter column ordem_compra varchar(40)
end
else
begin
	alter table nf_item add ordem_compra varchar(40)
end 
go 
go

insert into Versoes_Atualizadas select 'Versão:1.289.855', getdate();
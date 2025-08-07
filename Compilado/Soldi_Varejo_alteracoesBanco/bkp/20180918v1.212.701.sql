
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('data_validade'))
begin
	alter table nf_item alter column data_validade varchar(20)
end
else
begin
	alter table nf_item add data_validade datetime
end 
go 








insert into Versoes_Atualizadas select 'Versão:1.212.701', getdate();
GO
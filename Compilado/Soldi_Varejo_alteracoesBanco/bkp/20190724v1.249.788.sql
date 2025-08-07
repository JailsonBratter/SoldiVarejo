
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('natureza_operacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('ipi_base'))
begin
	alter table natureza_operacao alter column ipi_base tinyint
end
else
begin
	alter table natureza_operacao add ipi_base tinyint
end 
go 

go 

insert into Versoes_Atualizadas select 'Versão:1.249.788', getdate();
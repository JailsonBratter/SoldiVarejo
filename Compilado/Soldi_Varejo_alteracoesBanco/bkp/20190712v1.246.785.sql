IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('cfop'))
begin
	alter table mercadoria alter column cfop varchar(4)
end
else
begin
	alter table mercadoria add cfop varchar(4)
end 



go 

	insert into Versoes_Atualizadas select 'Versão:1.246.785', getdate();
GO

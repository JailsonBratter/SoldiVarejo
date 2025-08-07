IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('tesouraria_detalhes') 
            AND  UPPER(COLUMN_NAME) = UPPER('vencimento'))
begin
	alter table tesouraria_detalhes alter column vencimento datetime
end
else
begin
		
		alter table tesouraria_detalhes add vencimento datetime


end 

go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('tesouraria_detalhes') 
            AND  UPPER(COLUMN_NAME) = UPPER('taxa'))
begin
	alter table tesouraria_detalhes alter column taxa numeric(12,2)
end
else
begin
		
		alter table tesouraria_detalhes add taxa numeric(12,2)


end 

insert into Versoes_Atualizadas select 'Versão:1.146.580', getdate();
